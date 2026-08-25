[CmdletBinding()]
param(
    [string] $OutputDirectory = "dist/vpm",
    [string] $Repository = $(
        if ([string]::IsNullOrWhiteSpace($env:GITHUB_REPOSITORY)) {
            "VRCLearn/unity-mcp"
        } else {
            $env:GITHUB_REPOSITORY
        }
    ),
    [string] $Revision = ""
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

function Add-OrReplaceProperty {
    param(
        [Parameter(Mandatory)]
        [psobject] $Object,

        [Parameter(Mandatory)]
        [string] $Name,

        [AllowNull()]
        [object] $Value
    )

    if ($null -eq $Object.PSObject.Properties[$Name]) {
        $Object | Add-Member -MemberType NoteProperty -Name $Name -Value $Value
    } else {
        $Object.$Name = $Value
    }
}

$repositoryRoot = (& git rev-parse --show-toplevel).Trim()
if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($repositoryRoot)) {
    throw "Unable to locate the Git repository root."
}

if ([string]::IsNullOrWhiteSpace($Revision)) {
    $Revision = (& git -C $repositoryRoot rev-parse HEAD).Trim()
}

if ($Repository -notmatch "^[A-Za-z0-9_.-]+/[A-Za-z0-9_.-]+$") {
    throw "Invalid GitHub repository '$Repository'. Expected owner/name."
}

if ($Revision -notmatch "^[0-9a-fA-F]{40}$") {
    throw "Invalid Git revision '$Revision'."
}

$packageRoot = Join-Path $repositoryRoot "MCPForUnity"
$manifestPath = Join-Path $packageRoot "package.json"
$serverProjectPath = Join-Path $repositoryRoot "Server/pyproject.toml"
$mcpManifestPath = Join-Path $repositoryRoot "manifest.json"
$licensePath = Join-Path $repositoryRoot "LICENSE"

foreach ($requiredPath in @($manifestPath, $serverProjectPath, $mcpManifestPath, $licensePath)) {
    if (-not (Test-Path -LiteralPath $requiredPath -PathType Leaf)) {
        throw "Required file not found: $requiredPath"
    }
}

$manifest = Get-Content -Raw -LiteralPath $manifestPath | ConvertFrom-Json
$mcpManifest = Get-Content -Raw -LiteralPath $mcpManifestPath | ConvertFrom-Json
$serverProjectText = Get-Content -Raw -LiteralPath $serverProjectPath
$serverVersionMatch = [regex]::Match(
    $serverProjectText,
    '(?m)^version\s*=\s*"(?<version>[^"]+)"\s*$'
)
if (-not $serverVersionMatch.Success) {
    throw "Unable to read the project version from Server/pyproject.toml."
}

$packageVersion = [string] $manifest.version
$serverVersion = $serverVersionMatch.Groups["version"].Value
$mcpVersion = [string] $mcpManifest.version
if ($packageVersion -ne $serverVersion -or $packageVersion -ne $mcpVersion) {
    throw "Version mismatch: MCPForUnity=$packageVersion, Server=$serverVersion, manifest=$mcpVersion."
}

if ($packageVersion -notmatch "^(0|[1-9]\d*)\.(0|[1-9]\d*)\.(0|[1-9]\d*)$") {
    throw "Stable VPM releases require an X.Y.Z version; found '$packageVersion'."
}

foreach ($propertyName in @("name", "displayName", "version", "author")) {
    if ($null -eq $manifest.PSObject.Properties[$propertyName]) {
        throw "MCPForUnity/package.json is missing '$propertyName'."
    }
}

if ([string]::IsNullOrWhiteSpace($manifest.author.name) -or
    [string]::IsNullOrWhiteSpace($manifest.author.email)) {
    throw "package.json author.name and author.email must not be empty."
}

if ($manifest.name -notmatch "^[a-z0-9]+(?:[._-][a-z0-9]+)+$") {
    throw "The package name '$($manifest.name)' is not a valid lowercase package ID."
}

$releaseTag = "vpm-$packageVersion"
$artifactName = "$($manifest.name)-$packageVersion.zip"
$artifactUrl = "https://github.com/$Repository/releases/download/$releaseTag/$artifactName"
$repositoryUrl = "https://github.com/$Repository"

$stagedManifest = $manifest | ConvertTo-Json -Depth 100 | ConvertFrom-Json
Add-OrReplaceProperty -Object $stagedManifest -Name "license" -Value "MIT"
Add-OrReplaceProperty -Object $stagedManifest -Name "url" -Value $artifactUrl
Add-OrReplaceProperty -Object $stagedManifest -Name "changelogUrl" -Value "$repositoryUrl/releases/tag/$releaseTag"
Add-OrReplaceProperty -Object $stagedManifest -Name "vpmDependencies" -Value ([pscustomobject] @{})
Add-OrReplaceProperty -Object $stagedManifest -Name "repository" -Value ([pscustomobject] @{
    type = "git"
    url = "$repositoryUrl.git"
    revision = $Revision.ToLowerInvariant()
})

$trackedFiles = @(
    & git -C $repositoryRoot ls-files -- MCPForUnity |
        Where-Object { -not [string]::IsNullOrWhiteSpace($_) }
)
if ($LASTEXITCODE -ne 0 -or $trackedFiles.Count -eq 0) {
    throw "Unable to enumerate tracked MCPForUnity package files."
}

$resolvedOutputDirectory = if ([System.IO.Path]::IsPathRooted($OutputDirectory)) {
    [System.IO.Path]::GetFullPath($OutputDirectory)
} else {
    [System.IO.Path]::GetFullPath((Join-Path $repositoryRoot $OutputDirectory))
}
[System.IO.Directory]::CreateDirectory($resolvedOutputDirectory) | Out-Null

$zipPath = Join-Path $resolvedOutputDirectory $artifactName
$stagingDirectory = Join-Path (
    [System.IO.Path]::GetTempPath()
) "unity-mcp-vpm-$([guid]::NewGuid().ToString('N'))"

try {
    [System.IO.Directory]::CreateDirectory($stagingDirectory) | Out-Null

    foreach ($relativePath in $trackedFiles) {
        $packageRelativePath = $relativePath.Substring("MCPForUnity/".Length)
        $sourcePath = Join-Path $repositoryRoot $relativePath
        $destinationPath = Join-Path $stagingDirectory $packageRelativePath
        $destinationDirectory = Split-Path -Parent $destinationPath
        [System.IO.Directory]::CreateDirectory($destinationDirectory) | Out-Null
        Copy-Item -LiteralPath $sourcePath -Destination $destinationPath
    }

    $stagedManifest |
        ConvertTo-Json -Depth 100 |
        Set-Content -LiteralPath (Join-Path $stagingDirectory "package.json") -Encoding utf8NoBOM
    Copy-Item -LiteralPath $licensePath -Destination (Join-Path $stagingDirectory "LICENSE.md")

    if (Test-Path -LiteralPath $zipPath) {
        Remove-Item -LiteralPath $zipPath -Force
    }

    Add-Type -AssemblyName System.IO.Compression.FileSystem
    [System.IO.Compression.ZipFile]::CreateFromDirectory(
        $stagingDirectory,
        $zipPath,
        [System.IO.Compression.CompressionLevel]::Optimal,
        $false
    )
} finally {
    if (Test-Path -LiteralPath $stagingDirectory) {
        Remove-Item -LiteralPath $stagingDirectory -Recurse -Force
    }
}

$archive = [System.IO.Compression.ZipFile]::OpenRead($zipPath)
try {
    $entryNames = @($archive.Entries | ForEach-Object { $_.FullName })
    foreach ($requiredEntry in @("package.json", "LICENSE.md")) {
        if ($requiredEntry -notin $entryNames) {
            throw "The VPM archive does not contain $requiredEntry at its root."
        }
    }

    $manifestEntry = $archive.Entries |
        Where-Object { $_.FullName -eq "package.json" } |
        Select-Object -First 1
    $reader = [System.IO.StreamReader]::new($manifestEntry.Open())
    try {
        $archivedManifest = $reader.ReadToEnd() | ConvertFrom-Json
    } finally {
        $reader.Dispose()
    }

    if ($archivedManifest.name -ne $manifest.name -or
        $archivedManifest.version -ne $packageVersion -or
        $archivedManifest.url -ne $artifactUrl) {
        throw "The archived package manifest does not match the expected VPM metadata."
    }
} finally {
    $archive.Dispose()
}

$outputs = [ordered] @{
    package_name = $manifest.name
    display_name = $manifest.displayName
    version = $packageVersion
    tag = $releaseTag
    artifact_name = $artifactName
    zip_path = $zipPath
    sha256 = (Get-FileHash -Algorithm SHA256 -LiteralPath $zipPath).Hash.ToLowerInvariant()
}

if (-not [string]::IsNullOrWhiteSpace($env:GITHUB_OUTPUT)) {
    foreach ($output in $outputs.GetEnumerator()) {
        Add-Content -LiteralPath $env:GITHUB_OUTPUT -Value "$($output.Key)=$($output.Value)"
    }
}

$outputs | ConvertTo-Json
