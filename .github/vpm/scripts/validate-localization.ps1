[CmdletBinding()]
param()

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$repositoryRoot = (& git rev-parse --show-toplevel).Trim()
if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($repositoryRoot)) {
    throw "Unable to locate the Git repository root."
}

$localizationPath = Join-Path $repositoryRoot "MCPForUnity/Editor/Helpers/EditorLocalization.cs"
$editorRoot = Join-Path $repositoryRoot "MCPForUnity/Editor"
if (-not (Test-Path -LiteralPath $localizationPath -PathType Leaf)) {
    throw "Localization source not found: $localizationPath"
}

$source = Get-Content -Raw -LiteralPath $localizationPath
$csharpStringPattern = '"(?<value>(?:\\.|[^"\\])*)"'
$entryPattern = '(?m)^\s*\{\s*(?<key>"(?:\\.|[^"\\])*")\s*,\s*Values\((?<values>.*)\)\s*\},\s*$'
$entryMatches = [regex]::Matches($source, $entryPattern)
if ($entryMatches.Count -lt 400) {
    throw "Parsed only $($entryMatches.Count) localization entries; expected at least 400."
}

$keys = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::Ordinal)
foreach ($entryMatch in $entryMatches) {
    $stringMatches = [regex]::Matches($entryMatch.Value, $csharpStringPattern)
    if ($stringMatches.Count -ne 5) {
        throw "A localization entry must contain one key and four language values: $($entryMatch.Value.Trim())"
    }

    $values = @(
        foreach ($stringMatch in $stringMatches) {
            [regex]::Unescape($stringMatch.Groups["value"].Value)
        }
    )
    $key = $values[0]
    $translations = $values[1..4]

    if (-not $keys.Add($key)) {
        throw "Duplicate localization key: $key"
    }
    if ($translations[0] -ne $key) {
        throw "English localization value does not match its source key: $key"
    }
    if ($translations.Where({ [string]::IsNullOrWhiteSpace($_) }).Count -gt 0) {
        throw "Localization entry contains an empty language value: $key"
    }

    $expectedPlaceholders = @(
        [regex]::Matches($translations[0], '\{[0-9]+(?:[^{}]*)\}') |
            ForEach-Object Value |
            Sort-Object
    )
    foreach ($translation in $translations[1..3]) {
        $actualPlaceholders = @(
            [regex]::Matches($translation, '\{[0-9]+(?:[^{}]*)\}') |
                ForEach-Object Value |
                Sort-Object
        )
        if (($expectedPlaceholders -join "`n") -ne ($actualPlaceholders -join "`n")) {
            throw "Format placeholders do not match for localization key: $key"
        }
    }
}

$nonLocalizableUxmlValues = [System.Collections.Generic.HashSet[string]]::new(
    [string[]] @("...", "✓", "MCP for Unity", "Python"),
    [System.StringComparer]::Ordinal
)
$missingUxmlValues = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::Ordinal)
$uxmlFiles = Get-ChildItem -LiteralPath $editorRoot -Filter "*.uxml" -File -Recurse
foreach ($uxmlFile in $uxmlFiles) {
    [xml] $document = Get-Content -Raw -LiteralPath $uxmlFile.FullName
    $attributes = $document.SelectNodes("//@text | //@label | //@tooltip")
    foreach ($attribute in $attributes) {
        $value = [string] $attribute.Value
        if (
            -not [string]::IsNullOrWhiteSpace($value) -and
            -not $value.StartsWith("{") -and
            -not $nonLocalizableUxmlValues.Contains($value) -and
            $value -notmatch '^v[0-9]+(?:\.[0-9]+)+(?:[-+][0-9A-Za-z.-]+)?$' -and
            -not $keys.Contains($value)
        ) {
            [void] $missingUxmlValues.Add($value)
        }
    }
}

if ($missingUxmlValues.Count -gt 0) {
    $formattedValues = ($missingUxmlValues | Sort-Object | ForEach-Object { "- $_" }) -join "`n"
    throw "UXML text is missing from the localization table:`n$formattedValues"
}

Write-Host "Validated $($keys.Count) four-language entries across $($uxmlFiles.Count) UXML files."
