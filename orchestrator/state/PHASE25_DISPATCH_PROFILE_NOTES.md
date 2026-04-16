# Phase 25 Dispatch Profile Notes

- Added `dispatch_profiles` in `config/orchestrator_config.json`.
- Added `--use-owner-profile` flag in `dispatch.py`.
- Added `-UseDispatchProfile` switch in `run_orchestrator.ps1` (dispatch and cycle paths).

## Default profiles
- codex: timeout 90s, retries 1
- gemini: timeout 60s, retries 1
- claude: timeout 120s, retries 2
