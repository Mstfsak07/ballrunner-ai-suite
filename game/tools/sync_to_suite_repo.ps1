param(
  [string]$SourceGame = "C:\Users\akMuratNET\Desktop\BallRunnerHybrid",
  [string]$SourceOrchestrator = "C:\Users\akMuratNET\ai-game-orchestrator",
  [string]$SuiteRepo = "C:\Users\akMuratNET\Desktop\ballrunner-ai-suite"
)

$ErrorActionPreference = "Stop"

if (-not (Test-Path $SuiteRepo)) { throw "Suite repo not found: $SuiteRepo" }

$gameTarget = Join-Path $SuiteRepo "game"
$orcTarget = Join-Path $SuiteRepo "orchestrator"

New-Item -ItemType Directory -Force -Path $gameTarget,$orcTarget | Out-Null

robocopy $SourceGame $gameTarget /E /XD .git Library Temp Logs Obj Build .vs release-snapshots /XF *.user *.suo | Out-Null
robocopy $SourceOrchestrator $orcTarget /E /XD .git runs __pycache__ | Out-Null

Write-Host "Suite sync done."
