# Phase 13 Runbook

## Scripts
- `tools/clean_release_snapshots.ps1`
- `tools/generate_ops_daily_report.ps1`

## Routine
1. Generate daily ops report.
2. Run strict readiness check.
3. Generate/re-generate release manifest if code changed.
4. Export snapshot.
5. Clean old snapshots (keep latest N).

## Commands
```powershell
.\tools\generate_ops_daily_report.ps1
.\tools\verify_internal_readiness.ps1 -Strict
.\tools\generate_release_manifest.ps1
.\tools\export_release_snapshot.ps1
.\tools\clean_release_snapshots.ps1 -KeepLatest 5
```

## Notes
- Keep `OPS_DAILY_REPORT.md` updated before each internal upload.
- Snapshot cleanup should run after successful upload.
