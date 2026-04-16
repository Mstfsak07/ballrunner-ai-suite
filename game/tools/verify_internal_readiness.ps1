param(
  [switch]$Strict
)

$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $MyInvocation.MyCommand.Path | Split-Path -Parent

$checks = @(
  @{ key = "scene_setup"; path = "Assets/Scenes/SCENE_SETUP.md" },
  @{ key = "launch_checklist"; path = "LAUNCH_CHECKLIST.md" },
  @{ key = "release_notes"; path = "RELEASE_NOTES.md" },
  @{ key = "release_manifest"; path = "RELEASE_MANIFEST.json" },
  @{ key = "smoke_protocol"; path = "SMOKE_TEST_PROTOCOL.md" },
  @{ key = "phase7_regression"; path = "PHASE7_REGRESSION_CHECKLIST.md" },
  @{ key = "phase9_wiring"; path = "PHASE9_WIRING_NOTES.md" },
  @{ key = "phase8_setup"; path = "PHASE8_SETUP_NOTES.md" }
)

$missing = @()
$present = @()
foreach ($c in $checks) {
  $p = Join-Path $root $c.path
  if (Test-Path $p) {
    $present += $c.path
  } else {
    $missing += $c.path
  }
}

Write-Host "Readiness summary"
Write-Host "- Present:" $present.Count
Write-Host "- Missing:" $missing.Count

if ($missing.Count -gt 0) {
  Write-Host "Missing files:"
  $missing | ForEach-Object { Write-Host " - $_" }
  if ($Strict) {
    exit 2
  }
}

# Basic JSON validity check for manifest
$manifestPath = Join-Path $root "RELEASE_MANIFEST.json"
if (Test-Path $manifestPath) {
  try {
    Get-Content -Raw $manifestPath | ConvertFrom-Json | Out-Null
    Write-Host "Manifest JSON valid"
  }
  catch {
    Write-Host "Manifest JSON invalid"
    if ($Strict) { exit 3 }
  }
}

Write-Host "Readiness verification done."
