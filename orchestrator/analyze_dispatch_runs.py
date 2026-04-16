import argparse
import re
from collections import defaultdict
from datetime import datetime
from pathlib import Path

ROOT = Path(__file__).resolve().parent
RUNS_DIR = ROOT / "runs"

META_RE = re.compile(r"^(task|owner|model|timeout_sec|retries)=(.*)$")
EXIT_RE = re.compile(r"^exit_code=(\d+)$")
TIMEOUT_RE = re.compile(r"^timeout_after=(\d+)s$")


def parse_dispatch_log(path: Path) -> dict:
    meta = {"task": "", "owner": "", "model": "", "timeout_sec": "", "retries": ""}
    attempts = 0
    exits = []
    timeouts = 0

    for raw in path.read_text(encoding="utf-8", errors="ignore").splitlines():
        line = raw.strip()
        m = META_RE.match(line)
        if m and meta.get(m.group(1), "") == "":
            meta[m.group(1)] = m.group(2)
            continue

        m = EXIT_RE.match(line)
        if m:
            exits.append(int(m.group(1)))
            attempts += 1
            continue

        if TIMEOUT_RE.match(line):
            timeouts += 1
            attempts += 1

    final_exit = exits[-1] if exits else (124 if timeouts > 0 else -1)
    success = final_exit == 0

    return {
        "file": path.name,
        "task": meta["task"],
        "owner": meta["owner"],
        "model": meta["model"],
        "attempts": attempts,
        "timeouts": timeouts,
        "final_exit": final_exit,
        "success": success,
        "mtime": datetime.fromtimestamp(path.stat().st_mtime),
    }


def main() -> None:
    parser = argparse.ArgumentParser(description="Analyze dispatch logs and output compact health summary")
    parser.add_argument("--limit", type=int, default=30, help="max latest log files to include")
    parser.add_argument("--out", help="optional output markdown path")
    args = parser.parse_args()

    logs = sorted(RUNS_DIR.glob("dispatch-*.log"), key=lambda p: p.stat().st_mtime, reverse=True)
    if args.limit > 0:
        logs = logs[: args.limit]

    records = [parse_dispatch_log(p) for p in logs]

    owner_stats = defaultdict(lambda: {"total": 0, "ok": 0, "timeouts": 0, "attempts": 0})
    for r in records:
        owner = r["owner"] or "unknown"
        s = owner_stats[owner]
        s["total"] += 1
        s["ok"] += 1 if r["success"] else 0
        s["timeouts"] += r["timeouts"]
        s["attempts"] += r["attempts"]

    lines = [
        "# Dispatch Analytics",
        "",
        f"Generated: {datetime.now().strftime('%Y-%m-%d %H:%M:%S')}",
        f"Included logs: {len(records)}",
        "",
        "## Owner Summary",
    ]

    if owner_stats:
        for owner in sorted(owner_stats.keys()):
            s = owner_stats[owner]
            success_rate = (s["ok"] / s["total"] * 100.0) if s["total"] else 0.0
            lines.append(
                f"- {owner}: total={s['total']} ok={s['ok']} success_rate={success_rate:.1f}% timeouts={s['timeouts']} attempts={s['attempts']}"
            )
    else:
        lines.append("- no logs")

    lines += ["", "## Recent Logs"]
    if records:
        for r in records:
            lines.append(
                f"- {r['file']} | owner={r['owner']} model={r['model']} exit={r['final_exit']} attempts={r['attempts']} timeouts={r['timeouts']}"
            )
    else:
        lines.append("- none")

    if args.out:
        out_path = Path(args.out)
        if not out_path.is_absolute():
            out_path = ROOT / out_path
    else:
        out_path = RUNS_DIR / f"dispatch-analytics-{datetime.now().strftime('%Y%m%d-%H%M%S')}.md"

    out_path.parent.mkdir(parents=True, exist_ok=True)
    out_path.write_text("\n".join(lines) + "\n", encoding="utf-8")

    print(f"Analytics report: {out_path}")


if __name__ == "__main__":
    main()
