# Phase 16 Ops Note

## Auto Commit
```powershell
.\tools\suite_auto_commit.ps1 -Message "feat: ..."
```

## Auto Push
```powershell
.\tools\suite_auto_push.ps1 -Branch main
```

## Suggested Sequence
1. Sync source into suite repo
2. Pre-commit check
3. Auto commit
4. Auto push

## Guardrails
- Always run from trusted local machine.
- Check `suite_precommit_check.ps1` output before push.
