param(
  [Parameter(Mandatory=$true)][string]$VersionName,
  [Parameter(Mandatory=$true)][int]$VersionCode
)

$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $MyInvocation.MyCommand.Path | Split-Path -Parent

$buildMeta = Join-Path $root "Assets\Scripts\Core\BuildMetadata.cs"
$releaseNotes = Join-Path $root "RELEASE_NOTES.md"
$versionJson = Join-Path $root "BUILD_VERSION.json"

if (-not (Test-Path $buildMeta)) {
  throw "BuildMetadata.cs not found"
}

$content = Get-Content -Raw $buildMeta
$content = $content -replace 'private string versionName = ".*?";', ('private string versionName = "{0}";' -f $VersionName)
$content = $content -replace 'private int versionCode = \d+;', ('private int versionCode = {0};' -f $VersionCode)
Set-Content -Encoding UTF8 $buildMeta $content

if (Test-Path $releaseNotes) {
  $rn = Get-Content -Raw $releaseNotes
  $rn = $rn -replace 'VersionName:\s*.+', ('VersionName: {0}' -f $VersionName)
  $rn = $rn -replace 'VersionCode:\s*.+', ('VersionCode: {0}' -f $VersionCode)
  Set-Content -Encoding UTF8 $releaseNotes $rn
}

$versionObj = [pscustomobject]@{
  version_name = $VersionName
  version_code = $VersionCode
  bumped_utc = (Get-Date).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ")
}
$versionObj | ConvertTo-Json -Depth 4 | Set-Content -Encoding UTF8 $versionJson

Write-Host "Version bump applied: $VersionName ($VersionCode)"
