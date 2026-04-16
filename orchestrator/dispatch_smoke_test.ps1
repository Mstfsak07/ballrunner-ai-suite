param(
  [int]$TimeoutSec = 45,
  [switch]$CleanupStale
)

$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $MyInvocation.MyCommand.Path
$dispatch = Join-Path $root "dispatch.py"

function Invoke-DispatchTest {
  param(
    [string]$Owner,
    [string]$Model,
    [string]$PromptText
  )

  $tempRel = "tasks/_smoke_$Owner.md"
  $tempAbs = Join-Path $root $tempRel
  Set-Content -Path $tempAbs -Value $PromptText -Encoding ASCII

  $args = @(
    $dispatch,
    "--execute",
    "--owner", $Owner,
    "--model", $Model,
    "--run-file", $tempRel,
    "--timeout-sec", "$TimeoutSec",
    "--retries", "1",
    "--log-tail-lines", "40"
  )
  if ($CleanupStale) { $args += "--cleanup-stale" }

  try {
    & python @args
    if ($LASTEXITCODE -ne 0) { throw "dispatch failed for $Owner/$Model" }
    Write-Host "PASS: $Owner / $Model"
  }
  finally {
    if (Test-Path $tempAbs) { Remove-Item -LiteralPath $tempAbs -Force }
  }
}

Invoke-DispatchTest -Owner "gemini" -Model "gemini-3-flash" -PromptText "Reply with: SMOKE_OK_GEMINI"
Invoke-DispatchTest -Owner "claude" -Model "claude-sonnet-4-6" -PromptText "Reply with: SMOKE_OK_CLAUDE"

Write-Host "Smoke tests completed."
