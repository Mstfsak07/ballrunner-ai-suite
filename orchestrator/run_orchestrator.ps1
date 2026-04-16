param(
  [string]$TaskId,
  [switch]$AssignNext,
  [switch]$Status,
  [string]$CompleteTaskId,
  [string]$CompletionNote,
  [switch]$Dispatch,
  [switch]$ExecuteDispatch,
  [switch]$HealthCheck,
  [switch]$CleanupStale
)

$root = Split-Path -Parent $MyInvocation.MyCommand.Path

if ($AssignNext) {
  python "$root\orchestrator.py" assign-next
  exit $LASTEXITCODE
}

if ($TaskId) {
  python "$root\orchestrator.py" assign --task-id $TaskId
  exit $LASTEXITCODE
}

if ($Status) {
  python "$root\orchestrator.py" status
  exit $LASTEXITCODE
}

if ($CompleteTaskId) {
  python "$root\orchestrator.py" complete --task-id $CompleteTaskId --note "$CompletionNote"
  exit $LASTEXITCODE
}

if ($Dispatch) {
  if ($ExecuteDispatch) {
    if ($CleanupStale) {
      python "$root\dispatch.py" --execute --cleanup-stale
    } else {
      python "$root\dispatch.py" --execute
    }
  } else {
    python "$root\dispatch.py"
  }
  exit $LASTEXITCODE
}

if ($HealthCheck) {
  if ($CleanupStale) {
    & "$root\cli_healthcheck.ps1" -CleanupStale
  } else {
    & "$root\cli_healthcheck.ps1"
  }
  exit $LASTEXITCODE
}

Write-Host "Usage:"
Write-Host "  .\\run_orchestrator.ps1 -AssignNext"
Write-Host "  .\\run_orchestrator.ps1 -TaskId TASK-001"
Write-Host "  .\\run_orchestrator.ps1 -Status"
Write-Host "  .\\run_orchestrator.ps1 -CompleteTaskId TASK-001 -CompletionNote \"done\""
Write-Host "  .\\run_orchestrator.ps1 -Dispatch"
Write-Host "  .\\run_orchestrator.ps1 -Dispatch -ExecuteDispatch"
Write-Host "  .\\run_orchestrator.ps1 -HealthCheck"
Write-Host "  .\\run_orchestrator.ps1 -HealthCheck -CleanupStale"
