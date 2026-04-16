import argparse
import json
from datetime import datetime
from pathlib import Path
from typing import Dict, List, Optional, Tuple

ROOT = Path(__file__).resolve().parent
CONFIG_PATH = ROOT / "config" / "orchestrator_config.json"
BACKLOG_PATH = ROOT / "tasks" / "backlog.json"
STATE_PATH = ROOT / "state" / "SESSION_STATE.md"
HANDOFF_PATH = ROOT / "state" / "CURRENT_HANDOFF.md"
RUNS_DIR = ROOT / "runs"


def now_iso() -> str:
    return datetime.now().strftime("%Y-%m-%d %H:%M:%S")


def load_json(path: Path) -> Dict:
    with path.open("r", encoding="utf-8") as f:
        return json.load(f)


def save_json(path: Path, data: Dict) -> None:
    with path.open("w", encoding="utf-8") as f:
        json.dump(data, f, ensure_ascii=False, indent=2)


def contains_keyword(text: str, keywords: List[str]) -> bool:
    lowered = text.lower()
    return any(k.lower() in lowered for k in keywords)


def choose_owner_and_model(task: Dict, config: Dict) -> Tuple[str, str, str]:
    title_desc = f"{task.get('title', '')} {task.get('description', '')}"
    tags = " ".join(task.get("tags", []))
    bag = f"{title_desc} {tags}"

    routing = config["routing"]
    defaults = config["default_models"]

    if contains_keyword(bag, routing["claude_opus_keywords"]):
        return "claude", "claude-opus-4-6", "deep architecture / critical complexity"

    if contains_keyword(bag, routing["claude_sonnet_keywords"]):
        return "claude", defaults["claude"], "analysis and planning workload"

    if contains_keyword(bag, routing["codex_keywords"]):
        heavy_keywords = routing.get("codex_heavy_keywords", [])
        fast_keywords = routing.get("codex_fast_keywords", [])
        if contains_keyword(bag, heavy_keywords):
            return "codex", "gpt-5.4", "critical implementation and deeper reasoning"
        if contains_keyword(bag, fast_keywords):
            return "codex", "gpt-5.4-mini", "fast scaffold or lightweight execution"
        return "codex", defaults["codex"], "implementation heavy and deterministic execution"

    if contains_keyword(bag, routing["gemini_keywords"]):
        flash_keywords = routing.get("gemini_flash_keywords", [])
        if contains_keyword(bag, flash_keywords):
            return "gemini", "gemini-3-flash", "rapid and lightweight Gemini task"
        return "gemini", defaults["gemini"], "creative or rapid prototyping"

    return "codex", defaults["codex"], "default fallback"


def find_task(backlog: Dict, task_id: str) -> Optional[Dict]:
    for task in backlog.get("tasks", []):
        if task.get("id") == task_id:
            return task
    return None


def next_pending_task(backlog: Dict) -> Optional[Dict]:
    pending = [t for t in backlog.get("tasks", []) if t.get("status") == "pending"]
    if not pending:
        return None
    pending.sort(key=lambda x: (x.get("priority", 9999), x.get("id", "")))
    return pending[0]


def ensure_run_dir() -> Path:
    RUNS_DIR.mkdir(parents=True, exist_ok=True)
    run_dir = RUNS_DIR / datetime.now().strftime("%Y%m%d-%H%M%S")
    run_dir.mkdir(parents=True, exist_ok=True)
    return run_dir


def build_handoff_text(task: Dict, owner: str, model: str, reason: str, run_file: Path) -> str:
    return f"""# Current Handoff

Timestamp: {now_iso()}
Task: {task['id']} - {task['title']}
Owner: {owner}
Model: {model}
Reason: {reason}
Run File: {run_file}

## Required Start Line
ACCOUNT-CHECK: {owner} | task={task['id']} | model={model}

## Execution Brief
- Read `state/SESSION_STATE.md`
- Execute only this task scope.
- Save outputs in `runs/` and note key result.
- Do not touch unrelated tasks.
"""


def build_assignment_prompt(task: Dict, owner: str, model: str, reason: str) -> str:
    tags = ", ".join(task.get("tags", []))
    lines = [
        f"TASK:{task['id']} | {task['title']}",
        f"OWNER:{owner} MODEL:{model}",
        f"GOAL:{task.get('description', '')}",
        f"TAGS:{tags}",
        f"WHY_SELECTED:{reason}",
        "OUTPUT: concrete deliverable + short changelog",
        "RULES: scope-only; concise; blocker=>workaround",
    ]

    if owner == "claude":
        hints = select_claude_hints(task)
        if hints:
            lines.append(f"CLAUDE_HINTS(use-if-helpful): {' '.join(hints)}")

    return "\n".join(lines) + "\n"


def select_claude_hints(task: Dict) -> List[str]:
    text = (
        f"{task.get('title', '')} {task.get('description', '')} "
        f"{' '.join(task.get('tags', []))}"
    ).lower()
    hints: List[str] = []

    # Extracted from Claude_Codes PDF; mapped to task intent.
    if any(k in text for k in ["qa", "review", "analysis", "spec", "checklist", "risk", "bug"]):
        hints += ["/audit", "/redteam", "/premortem", "SENTINEL", "/digest"]
    if any(k in text for k in ["launch", "plan", "roadmap", "strategy"]):
        hints += ["PARETO", "/scenario", "BLINDSPOT", "/trim"]
    if any(k in text for k in ["architecture", "design", "refactor", "complex", "critical"]):
        hints += ["ARCHITECT", "XRAY", "/blindspots", "CHAINLOGIC"]

    deduped: List[str] = []
    for hint in hints:
        if hint not in deduped:
            deduped.append(hint)

    return deduped[:4]


def write_state(backlog: Dict, active: Optional[Dict]) -> None:
    completed = [t for t in backlog.get("tasks", []) if t.get("status") == "completed"]

    lines = [
        "# Session State",
        "",
        f"Last updated: {now_iso()}",
        "Orchestrator: codex",
        f"Project: {backlog.get('project', 'Unknown')}",
        "",
        "## Active Task",
    ]

    if active:
        lines += [
            f"- id: {active.get('id', 'none')}",
            f"- owner: {active.get('assigned_to', 'none')}",
            f"- model: {active.get('assigned_model', 'none')}",
            f"- status: {active.get('status', 'none')}",
        ]
    else:
        lines += ["- id: none", "- owner: none", "- model: none", "- status: idle"]

    lines += ["", "## Completed Tasks"]

    if completed:
        for item in completed:
            lines.append(f"- {item['id']}: {item['title']}")
    else:
        lines.append("- none")

    lines += [
        "",
        "## Open Questions",
        "- none",
        "",
        "## Notes",
        "- This is the single state file all accounts must read after switching.",
    ]

    STATE_PATH.write_text("\n".join(lines) + "\n", encoding="utf-8")


def assign_task(task: Dict, backlog: Dict, config: Dict) -> None:
    owner, model, reason = choose_owner_and_model(task, config)
    if owner == "claude" and model not in {"claude-sonnet-4-6", "claude-opus-4-6"}:
        raise SystemExit("Claude model must be claude-sonnet-4-6 or claude-opus-4-6")
    if owner == "gemini" and model not in {"gemini-3-pro", "gemini-3-flash"}:
        raise SystemExit("Gemini model must be gemini-3-pro or gemini-3-flash")
    if owner == "codex" and model not in {"gpt-5.3-codex", "gpt-5.4", "gpt-5.4-mini"}:
        raise SystemExit("Codex model must be gpt-5.3-codex, gpt-5.4, or gpt-5.4-mini")

    run_dir = ensure_run_dir()
    assignment_text = build_assignment_prompt(task, owner, model, reason)
    assignment_file = run_dir / f"assignment_{task['id']}.md"
    assignment_file.write_text(assignment_text, encoding="utf-8")

    task["status"] = "assigned"
    task["assigned_to"] = owner
    task["assigned_model"] = model
    task["assigned_at"] = now_iso()
    task["selection_reason"] = reason

    backlog["last_updated"] = now_iso()
    save_json(BACKLOG_PATH, backlog)

    HANDOFF_PATH.write_text(
        build_handoff_text(task, owner, model, reason, assignment_file.relative_to(ROOT)),
        encoding="utf-8",
    )

    write_state(backlog, task)

    print(f"Assigned {task['id']} to {owner} ({model})")
    print(f"Reason: {reason}")
    print(f"Brief: {assignment_file}")


def complete_task(task: Dict, backlog: Dict, note: str) -> None:
    task["status"] = "completed"
    task["completed_at"] = now_iso()
    task["completion_note"] = note or "completed"
    backlog["last_updated"] = now_iso()
    save_json(BACKLOG_PATH, backlog)

    HANDOFF_PATH.write_text(
        "# Current Handoff\n\nNo active handoff right now.\n",
        encoding="utf-8",
    )

    write_state(backlog, None)
    print(f"Completed {task['id']}")


def show_status(backlog: Dict) -> None:
    print(f"Project: {backlog.get('project', 'Unknown')}")
    print(f"Last updated: {backlog.get('last_updated', '')}")

    for task in backlog.get("tasks", []):
        print(
            f"{task.get('id')} | {task.get('status')} | p{task.get('priority')} | "
            f"{task.get('assigned_to', '-')}/{task.get('assigned_model', '-')} | {task.get('title')}"
        )


def main() -> None:
    parser = argparse.ArgumentParser(description="Game AI orchestrator")
    sub = parser.add_subparsers(dest="cmd", required=True)

    assign = sub.add_parser("assign", help="assign a specific task")
    assign.add_argument("--task-id", required=True)

    sub.add_parser("assign-next", help="assign next pending task by priority")

    complete = sub.add_parser("complete", help="mark task as complete")
    complete.add_argument("--task-id", required=True)
    complete.add_argument("--note", default="")

    sub.add_parser("status", help="show backlog status")

    args = parser.parse_args()

    config = load_json(CONFIG_PATH)
    backlog = load_json(BACKLOG_PATH)

    if args.cmd == "assign":
        task = find_task(backlog, args.task_id)
        if not task:
            raise SystemExit(f"Task not found: {args.task_id}")
        if task.get("status") not in ["pending", "assigned"]:
            raise SystemExit(f"Task not assignable in status={task.get('status')}")
        assign_task(task, backlog, config)
        return

    if args.cmd == "assign-next":
        task = next_pending_task(backlog)
        if not task:
            print("No pending task.")
            return
        assign_task(task, backlog, config)
        return

    if args.cmd == "complete":
        task = find_task(backlog, args.task_id)
        if not task:
            raise SystemExit(f"Task not found: {args.task_id}")
        complete_task(task, backlog, args.note)
        return

    if args.cmd == "status":
        show_status(backlog)
        return


if __name__ == "__main__":
    main()
