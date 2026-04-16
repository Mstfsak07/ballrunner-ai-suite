$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $MyInvocation.MyCommand.Path | Split-Path -Parent
$out = Join-Path $root "OPS_DAILY_REPORT.md"

$files = @(
  "RELEASE_NOTES.md",
  "RELEASE_MANIFEST.json",
  "LAUNCH_CHECKLIST.md",
  "BUILD_VERSION.json",
  "OPS_HANDOFF.md"
)

$lines = @()
$lines += "# Ops Daily Report"
$lines += ""
$lines += "Generated: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')"
$lines += ""
$lines += "## Artifact Status"
foreach ($f in $files) {
  $p = Join-Path $root $f
  if (Test-Path $p) {
    $it = Get-Item $p
    $lines += "- [OK] $f (updated: $($it.LastWriteTime.ToString('yyyy-MM-dd HH:mm:ss')) )"
  } else {
    $lines += "- [MISSING] $f"
  }
}

$snaps = Join-Path $root "release-snapshots"
$lines += ""
$lines += "## Snapshot Summary"
if (Test-Path $snaps) {
  $count = (Get-ChildItem $snaps -Directory).Count
  $lines += "- snapshot_count: $count"
} else {
  $lines += "- snapshot_count: 0"
}

$lines += ""
$lines += "## Next Ops Actions"
$lines += "1. Run verify_internal_readiness.ps1 -Strict"
$lines += "2. Regenerate manifest if code changed"
$lines += "3. Export fresh snapshot before upload"

$lines -join "`n" | Set-Content -Encoding UTF8 $out
Write-Host "Report generated: $out"
