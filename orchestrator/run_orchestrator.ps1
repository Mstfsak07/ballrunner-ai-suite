param(
  [string]$TaskId,
  [switch]$AssignNext,
  [switch]$Status,
  [string]$CompleteTaskId,
  [string]$CompletionNote,
  [switch]$Dispatch,
  [switch]$ExecuteDispatch,
  [switch]$HealthCheck,
  [switch]$CleanupStale,
  [switch]$Cycle,
  [int]$CycleLimit = 1,
  [int]$DispatchTimeoutSec = 120,
  [int]$DispatchRetries = 2,
  [switch]$AutoComplete,
  [switch]$ContinueOnError,
  [switch]$ExportCompactQueue,
  [int]$CompactLimit = 25,
  [switch]$CompactIncludeAssigned,
  [switch]$UseDispatchProfile,
  [switch]$DispatchAnalytics,
  [int]$AnalyticsLimit = 30
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
    $args = @("$root\dispatch.py", "--execute")
    if ($CleanupStale) {
      $args += "--cleanup-stale"
    }
    if ($UseDispatchProfile) {
      $args += "--use-owner-profile"
    }
    python @args
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

if ($Cycle) {
  if ($CycleLimit -lt 1) { throw "CycleLimit must be >= 1" }

  for ($i = 1; $i -le $CycleLimit; $i++) {
    Write-Host "[cycle $i/$CycleLimit] assign-next"
    $assignOut = python "$root\orchestrator.py" assign-next 2>&1
    if ($LASTEXITCODE -ne 0) {
      Write-Host $assignOut
      if ($ContinueOnError) { continue } else { exit $LASTEXITCODE }
    }
    if ($assignOut -match "No pending task.") {
      Write-Host "No pending task. Cycle stopped."
      break
    }

    Write-Host "[cycle $i/$CycleLimit] dispatch --execute"
    $dispatchArgs = @(
      "$root\dispatch.py",
      "--execute",
      "--timeout-sec", "$DispatchTimeoutSec",
      "--retries", "$DispatchRetries"
    )
    if ($CleanupStale) { $dispatchArgs += "--cleanup-stale" }
    if ($UseDispatchProfile) { $dispatchArgs += "--use-owner-profile" }

    python @dispatchArgs
    $dispatchCode = $LASTEXITCODE
    if ($dispatchCode -ne 0) {
      if ($ContinueOnError) { continue } else { exit $dispatchCode }
    }

    if ($AutoComplete) {
      $handoffPath = Join-Path $root "state\CURRENT_HANDOFF.md"
      $taskLine = (Get-Content -Path $handoffPath | Where-Object { $_ -like "Task:*" } | Select-Object -First 1)
      if ($taskLine -match "^Task:\s*([^\s]+)\s*-") {
        $taskId = $Matches[1]
        $note = "auto-completed by cycle dispatch at $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')"
        Write-Host "[cycle $i/$CycleLimit] complete $taskId"
        python "$root\orchestrator.py" complete --task-id $taskId --note "$note"
        if ($LASTEXITCODE -ne 0) {
          if ($ContinueOnError) { continue } else { exit $LASTEXITCODE }
        }
      }
    }
  }
  exit 0
}

if ($ExportCompactQueue) {
  $args = @("$root\export_compact_queue.py", "--limit", "$CompactLimit")
  if ($CompactIncludeAssigned) { $args += "--include-assigned" }
  python @args
  exit $LASTEXITCODE
}

if ($DispatchAnalytics) {
  python "$root\analyze_dispatch_runs.py" --limit $AnalyticsLimit
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
Write-Host "  .\\run_orchestrator.ps1 -Cycle -CycleLimit 3 -DispatchTimeoutSec 120 -DispatchRetries 1 -AutoComplete"
Write-Host "  .\\run_orchestrator.ps1 -ExportCompactQueue -CompactLimit 30 -CompactIncludeAssigned"
Write-Host "  .\\run_orchestrator.ps1 -Dispatch -ExecuteDispatch -UseDispatchProfile"
Write-Host "  .\\run_orchestrator.ps1 -DispatchAnalytics -AnalyticsLimit 40"
