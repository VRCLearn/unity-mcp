[CmdletBinding()]
param(
    [string] $SourcePath = ".github/vpm/source.json",
    [string] $SitePath = ".github/vpm/migration",
    [string] $OutputDirectory = "dist/vpm-site",
    [string] $ListingBuilder = "launcher-tools/tools/build_listing.py"
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

# Use the canonical repository's validator to mirror both packages, while
# source.json preserves this legacy URL and ID for existing VPM clients.
& python $ListingBuilder --source $SourcePath --site-path $SitePath --output $OutputDirectory
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
