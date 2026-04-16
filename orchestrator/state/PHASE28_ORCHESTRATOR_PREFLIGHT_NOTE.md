# Phase 28 Orchestrator Preflight Note

## Added scripts
- `check_prereqs.ps1`: validates `python`, `git`, `claude`, `gemini` availability.
- `run_orchestrator_preflight.ps1`: full orchestrator readiness flow.

## Preflight flow
1. Prerequisite check
2. Agent stack validation (healthcheck + smoke)
3. Dispatch analytics report
4. Compact queue export

## Wrapper flags
- `-CheckPrereqs`
- `-OrchestratorPreflight`
