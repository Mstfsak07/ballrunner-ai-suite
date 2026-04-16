param(
  [string]$OutputPath = "RELEASE_MANIFEST.json"
)

$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $MyInvocation.MyCommand.Path | Split-Path -Parent
$out = Join-Path $root $OutputPath

$include = @(
  "Assets/Scripts",
  "Assets/Scenes/SCENE_SETUP.md",
  "LAUNCH_CHECKLIST.md",
  "RELEASE_NOTES.md",
  "PHASE7_REGRESSION_CHECKLIST.md",
  "PHASE9_WIRING_NOTES.md"
)

$files = @()
foreach ($item in $include) {
  $path = Join-Path $root $item
  if (Test-Path $path) {
    if ((Get-Item $path).PSIsContainer) {
      $files += Get-ChildItem $path -Recurse -File
    } else {
      $files += Get-Item $path
    }
  }
}

$records = foreach ($f in $files | Sort-Object FullName -Unique) {
  $rel = $f.FullName.Substring($root.Length + 1).Replace('\\','/')
  $hash = (Get-FileHash -Algorithm SHA256 -Path $f.FullName).Hash.ToLowerInvariant()
  [pscustomobject]@{
    path = $rel
    size = $f.Length
    sha256 = $hash
    modified_utc = $f.LastWriteTimeUtc.ToString("yyyy-MM-ddTHH:mm:ssZ")
  }
}

$manifest = [pscustomobject]@{
  generated_utc = (Get-Date).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ")
  project = "BallRunnerHybrid"
  file_count = @($records).Count
  files = @($records)
}

$manifest | ConvertTo-Json -Depth 6 | Set-Content -Encoding UTF8 $out
Write-Host "Manifest generated: $out"
