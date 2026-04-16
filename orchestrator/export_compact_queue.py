import argparse
import json
from datetime import datetime
from pathlib import Path

ROOT = Path(__file__).resolve().parent
BACKLOG_PATH = ROOT / "tasks" / "backlog.json"
RUNS_DIR = ROOT / "runs"


def now_tag() -> str:
    return datetime.now().strftime("%Y%m%d-%H%M%S")


def load_backlog() -> dict:
    return json.loads(BACKLOG_PATH.read_text(encoding="utf-8"))


def build_compact_items(tasks: list[dict], include_assigned: bool, limit: int) -> list[dict]:
    allowed = {"pending"}
    if include_assigned:
        allowed.add("assigned")

    filtered = [t for t in tasks if t.get("status") in allowed]
    filtered.sort(key=lambda x: (x.get("priority", 9999), x.get("id", "")))

    if limit > 0:
        filtered = filtered[:limit]

    compact = []
    for t in filtered:
        compact.append(
            {
                "id": t.get("id"),
                "phase": t.get("phase"),
                "priority": t.get("priority"),
                "title": t.get("title"),
                "description": t.get("description"),
                "tags": t.get("tags", []),
                "status": t.get("status"),
                "assigned_to": t.get("assigned_to", ""),
                "assigned_model": t.get("assigned_model", ""),
            }
        )
    return compact


def main() -> None:
    parser = argparse.ArgumentParser(description="Export compact queue for low-token routing")
    parser.add_argument("--include-assigned", action="store_true", help="include assigned tasks too")
    parser.add_argument("--limit", type=int, default=25, help="max task count (0 = no limit)")
    parser.add_argument("--out", help="optional output path")
    args = parser.parse_args()

    backlog = load_backlog()
    compact_items = build_compact_items(backlog.get("tasks", []), args.include_assigned, args.limit)

    payload = {
        "project": backlog.get("project", "Unknown"),
        "exported_at": datetime.now().strftime("%Y-%m-%d %H:%M:%S"),
        "count": len(compact_items),
        "mode": "pending+assigned" if args.include_assigned else "pending-only",
        "items": compact_items,
    }

    if args.out:
        out_path = Path(args.out)
        if not out_path.is_absolute():
            out_path = ROOT / out_path
    else:
        RUNS_DIR.mkdir(parents=True, exist_ok=True)
        out_path = RUNS_DIR / f"compact-queue-{now_tag()}.json"

    out_path.parent.mkdir(parents=True, exist_ok=True)
    out_path.write_text(json.dumps(payload, ensure_ascii=False, indent=2), encoding="utf-8")

    print(f"Wrote compact queue: {out_path}")
    print(f"Tasks: {len(compact_items)}")


if __name__ == "__main__":
    main()
