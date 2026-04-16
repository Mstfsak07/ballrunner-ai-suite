$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $MyInvocation.MyCommand.Path | Split-Path -Parent
$out = Join-Path $root "PREFLIGHT_ARTIFACT_SUMMARY.md"

$files = @(
  "BUILD_VERSION.json",
  "RELEASE_NOTES.md",
  "RELEASE_MANIFEST.json",
  "OPS_DAILY_REPORT.md",
  "PREFLIGHT_RESULT.md"
)

$lines = @()
$lines += "# Preflight Artifact Summary"
$lines += ""
$lines += "Generated: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')"
$lines += ""
$lines += "## Files"
foreach ($f in $files) {
  $p = Join-Path $root $f
  if (Test-Path $p) {
    $it = Get-Item $p
    $lines += "- [OK] $f ($($it.Length) bytes, $($it.LastWriteTime.ToString('yyyy-MM-dd HH:mm:ss')))"
  } else {
    $lines += "- [MISSING] $f"
  }
}

$snaps = Join-Path $root "release-snapshots"
$lines += ""
$lines += "## Snapshot Folders"
if (Test-Path $snaps) {
  $dirs = Get-ChildItem $snaps -Directory | Sort-Object LastWriteTime -Descending
  foreach ($d in $dirs | Select-Object -First 5) {
    $lines += "- $($d.Name) ($($d.LastWriteTime.ToString('yyyy-MM-dd HH:mm:ss')))"
  }
} else {
  $lines += "- none"
}

$lines -join "`n" | Set-Content -Encoding UTF8 $out
Write-Host "Summary generated: $out"
