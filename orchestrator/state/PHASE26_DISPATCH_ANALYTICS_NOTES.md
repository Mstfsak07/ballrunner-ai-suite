# Phase 26 Dispatch Analytics Notes

- New script: `analyze_dispatch_runs.py`
- Reads recent `runs/dispatch-*.log` files and generates owner-based success/timeout summary.
- Wrapper command added: `run_orchestrator.ps1 -DispatchAnalytics -AnalyticsLimit <n>`.

## Example
- `python analyze_dispatch_runs.py --limit 20`
- `.\run_orchestrator.ps1 -DispatchAnalytics -AnalyticsLimit 15`
