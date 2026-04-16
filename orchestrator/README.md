# AI Game Orchestrator

Bu klasor, oyun projesi icin task dagitimini orchestrator mantigi ile yonetir.
Orchestrator hangi isi Gemini, Claude (Opus/Sonnet) veya Codex'in yapacagini secer.

## Hedef
- Tek backlog dosyasindan ilerlemek
- Hesap degisince kafa karisikligini sifira indirmek
- Her task icin net owner + model + handoff kaydi tutmak

## Dosyalar
- `orchestrator.py`: routing ve durum guncelleme motoru
- `run_orchestrator.ps1`: kolay komut arayuzu
- `PHASE_PLAN.md`: fazlara ayrilmis uygulama plani
- `tasks/backlog.json`: tum gorevler
- `state/READ_THIS_ON_ACCOUNT_SWITCH.md`: hesap degisiminde once okunacak dosya
- `state/SESSION_STATE.md`: tek source of truth durum dosyasi
- `state/CURRENT_HANDOFF.md`: aktif gorevin kime verildigi
- `runs/`: her atama icin assignment kayitlari

## Kullanim
PowerShell:

```powershell
cd C:\Users\akMuratNET\ai-game-orchestrator
.\run_orchestrator.ps1 -Status
.\run_orchestrator.ps1 -AssignNext
.\run_orchestrator.ps1 -TaskId TASK-001
.\run_orchestrator.ps1 -CompleteTaskId TASK-001 -CompletionNote "core loop done"
.\run_orchestrator.ps1 -Dispatch
.\run_orchestrator.ps1 -Dispatch -ExecuteDispatch
.\run_orchestrator.ps1 -Dispatch -ExecuteDispatch -UseDispatchProfile
.\run_orchestrator.ps1 -HealthCheck
.\run_orchestrator.ps1 -HealthCheck -CleanupStale
.\run_orchestrator.ps1 -DispatchAnalytics -AnalyticsLimit 40
.\run_orchestrator.ps1 -ExportCompactQueue -CompactLimit 30 -CompactIncludeAssigned
.\run_orchestrator.ps1 -Cycle -CycleLimit 2 -DispatchTimeoutSec 90 -DispatchRetries 1 -AutoComplete
.\run_orchestrator.ps1 -CheckPrereqs
.\run_orchestrator.ps1 -OrchestratorPreflight -CleanupStale
```

Python direkt:

```powershell
python orchestrator.py status
python orchestrator.py assign-next
python orchestrator.py assign --task-id TASK-001
python orchestrator.py complete --task-id TASK-001 --note "done"
python dispatch.py
python dispatch.py --execute
python dispatch.py --execute --cleanup-stale --timeout-sec 120 --retries 2
python dispatch.py --execute --use-owner-profile
python analyze_dispatch_runs.py --limit 30
python export_compact_queue.py --limit 25 --include-assigned
```

## Routing mantigi
- `claude-opus-4-6`: architecture / critical / complex
- `claude-sonnet-4-6`: review / analysis / plan
- `gemini-3-pro`: ui / prototype / creative
- `gemini-3-flash`: simple / quick / draft / lightweight Gemini gorevleri
- `gpt-5.3-codex (default)`: standard implementation tasks
- `gpt-5.4 (codex)`: critical / complex implementation
- `gpt-5.4-mini (codex)`: quick scaffold / lightweight tasks

Anahtar kelimeler `config/orchestrator_config.json` icinden degistirilebilir.

## Dispatch
- `dispatch.py` aktif handoff'u okur ve owner'a gore CLI komutu uretir.
- Varsayilan mod `dry-run` (sadece komutu yazdirir).
- `--execute` ile komutu gercekten calistirir.
- Claude komutu: `claude --dangerously-skip-permissions --model claude-sonnet-4-6` veya `claude-opus-4-6`
- Gemini komutu: `gemini --approval-mode yolo -m gemini-3-pro` veya `gemini-3-flash`
- CLI parametre farklari varsa `config/agent_runners.json` icindeki template'i kendi kurulumuna gore guncelle.

## Token Saving
- Assignment promptlari compact formatta uretilir (daha az token).
- Claude gorevlerinde kisa `CLAUDE_HINTS` satiri eklenir.
- PDF extract: `state/CLAUDE_CODES_EXTRACT.txt`
- Claude hint referansi: `state/CLAUDE_SKILLS_REFERENCE.md`

## CLI Stabilization
- `cli_healthcheck.ps1`: Claude/Gemini non-interactive health check.
- `dispatch.py` artik timeout + retry + run log yazar (`runs/dispatch-*.log`).

## Hesap degisim protokolu
1. Hesap degistiginde once `state/READ_THIS_ON_ACCOUNT_SWITCH.md` okunur.
2. Sonra `state/SESSION_STATE.md` ve `state/CURRENT_HANDOFF.md` kontrol edilir.
3. Agent ilk mesajinda su satiri yazar:

`ACCOUNT-CHECK: <agent-name> | task=<TASK-ID> | model=<MODEL>`

Bu protokol agentlarin yanlis goreve kaymasini engeller.
