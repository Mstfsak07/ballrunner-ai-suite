# Phase 12 Usage Notes

## Scripts
- `tools/export_release_snapshot.ps1`
- `tools/bump_version.ps1`

## Export Snapshot
```powershell
.\tools\export_release_snapshot.ps1
```
Creates `release-snapshots/snapshot-<timestamp>/` with release artifacts.

## Version Bump
```powershell
.\tools\bump_version.ps1 -VersionName "0.1.1-internal" -VersionCode 2
```
Updates:
- `Assets/Scripts/Core/BuildMetadata.cs`
- `RELEASE_NOTES.md` (if exists)
- `BUILD_VERSION.json`

## Recommended Order
1. Bump version
2. Regenerate release notes
3. Generate manifest
4. Export snapshot
5. Verify readiness strict
