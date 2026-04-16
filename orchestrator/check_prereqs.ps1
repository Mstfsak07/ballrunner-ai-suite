param()

$ErrorActionPreference = "Stop"
$checks = @(
  @{ name = "python"; cmd = "python --version" },
  @{ name = "git"; cmd = "git --version" },
  @{ name = "claude"; cmd = "claude --version" },
  @{ name = "gemini"; cmd = "gemini --version" }
)

$failed = $false
foreach ($c in $checks) {
  try {
    $out = cmd /c $c.cmd 2>&1
    if ($LASTEXITCODE -ne 0) { throw "exit=$LASTEXITCODE" }
    Write-Host "[OK] $($c.name): $out"
  }
  catch {
    Write-Host "[FAIL] $($c.name): $($_.Exception.Message)"
    $failed = $true
  }
}

if ($failed) { exit 1 }
