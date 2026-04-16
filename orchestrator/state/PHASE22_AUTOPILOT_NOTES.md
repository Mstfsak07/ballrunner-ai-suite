# Phase 22 Autopilot Notes

## New cycle mode
- `run_orchestrator.ps1 -Cycle` adds a looped execution mode.
- Flow per cycle: `assign-next` -> `dispatch --execute` -> optional `complete`.

## Key flags
- `-CycleLimit <n>`: max cycle count.
- `-DispatchTimeoutSec <sec>` and `-DispatchRetries <n>`: forwarded to `dispatch.py`.
- `-AutoComplete`: marks active task completed after successful dispatch.
- `-ContinueOnError`: continues to next cycle when assign/dispatch/complete fails.
- `-CleanupStale`: cleans stale CLI node processes before dispatch.

## Example
- `.\run_orchestrator.ps1 -Cycle -CycleLimit 2 -DispatchTimeoutSec 90 -DispatchRetries 1 -AutoComplete -CleanupStale`
