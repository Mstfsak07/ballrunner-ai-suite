import argparse
import json
import re
import shutil
import subprocess
from datetime import datetime
from pathlib import Path

ROOT = Path(__file__).resolve().parent
HANDOFF = ROOT / "state" / "CURRENT_HANDOFF.md"
RUNNERS = ROOT / "config" / "agent_runners.json"
CONFIG = ROOT / "config" / "orchestrator_config.json"
RUNS = ROOT / "runs"


def parse_handoff(text: str) -> dict:
    text = text.replace("\ufeff", "")
    def pick(label: str) -> str:
        m = re.search(rf"^{label}:\s*(.+)$", text, re.MULTILINE)
        return m.group(1).strip() if m else ""

    return {
        "task": pick("Task"),
        "owner": pick("Owner"),
        "model": pick("Model"),
        "run_file": pick("Run File"),
    }


def shell_quote(text: str) -> str:
    return text.replace("\ufeff", "").replace("\"", "\\\"")


def cleanup_stale_node_processes() -> None:
    # Clean up stale node processes started by claude/gemini CLIs.
    command = (
        "Get-CimInstance Win32_Process | "
        "Where-Object { $_.Name -eq 'node.exe' -and "
        "($_.CommandLine -like '*claude*' -or $_.CommandLine -like '*gemini*') } | "
        "ForEach-Object { Stop-Process -Id $_.ProcessId -Force -ErrorAction SilentlyContinue }"
    )
    subprocess.run(["powershell", "-NoProfile", "-Command", command], check=False)


def run_with_retries(command: str, timeout_sec: int, retries: int, run_log: Path) -> int:
    last_code = 1
    for attempt in range(1, retries + 1):
        started = datetime.now().strftime("%Y-%m-%d %H:%M:%S")
        try:
            result = subprocess.run(
                command,
                shell=True,
                check=False,
                timeout=timeout_sec,
                capture_output=True,
                text=True,
            )
            run_log.write_text(
                run_log.read_text(encoding="utf-8")
                + (
                    f"\n--- attempt {attempt} @ {started} ---\n"
                    f"exit_code={result.returncode}\n"
                    f"stdout:\n{result.stdout}\n"
                    f"stderr:\n{result.stderr}\n"
                ),
                encoding="utf-8",
            )
            if result.returncode == 0:
                print(result.stdout.strip())
                return 0
            last_code = result.returncode
        except subprocess.TimeoutExpired as exc:
            run_log.write_text(
                run_log.read_text(encoding="utf-8")
                + (
                    f"\n--- attempt {attempt} @ {started} ---\n"
                    f"timeout_after={timeout_sec}s\n"
                    f"partial_stdout:\n{(exc.stdout or '')}\n"
                    f"partial_stderr:\n{(exc.stderr or '')}\n"
                ),
                encoding="utf-8",
            )
            last_code = 124
    return last_code


def main() -> None:
    parser = argparse.ArgumentParser(description="Dispatch active handoff to selected agent CLI")
    parser.add_argument("--execute", action="store_true", help="actually run command")
    parser.add_argument("--timeout-sec", type=int, default=120, help="timeout per attempt")
    parser.add_argument("--retries", type=int, default=2, help="retry count")
    parser.add_argument("--cleanup-stale", action="store_true", help="cleanup stale node processes first")
    parser.add_argument("--owner", help="override owner (claude/gemini/codex)")
    parser.add_argument("--model", help="override model")
    parser.add_argument("--run-file", help="override run file path relative to repo root")
    parser.add_argument("--log-tail-lines", type=int, default=60, help="lines to print from log on failure")
    parser.add_argument("--use-owner-profile", action="store_true", help="use owner dispatch profile from orchestrator config")
    args = parser.parse_args()

    handoff_text = HANDOFF.read_text(encoding="utf-8")
    data = parse_handoff(handoff_text)
    if args.owner:
        data["owner"] = args.owner
    if args.model:
        data["model"] = args.model
    if args.run_file:
        data["run_file"] = args.run_file

    if not data["owner"] or data["owner"] == "none":
        raise SystemExit("No active handoff.")

    runners = json.loads(RUNNERS.read_text(encoding="utf-8"))
    runner = runners.get(data["owner"])
    if not runner:
        raise SystemExit(f"No runner config for owner={data['owner']}")

    prompt_file = ROOT / data["run_file"]
    if not prompt_file.exists():
        raise SystemExit(f"Prompt file not found: {prompt_file}")

    prompt_text = prompt_file.read_text(encoding="utf-8")
    command = runner["template"].format(
        model=data["model"],
        prompt_file=str(prompt_file),
        prompt_text=shell_quote(prompt_text),
    )

    print(f"Task: {data['task']}")
    print(f"Owner: {data['owner']}")
    print(f"Model: {data['model']}")
    safe_command = command.encode("cp1254", errors="replace").decode("cp1254")
    print(f"Command: {safe_command}")

    if not args.execute:
        print("Dry run only. Use --execute to run.")
        return

    binary = runner["binary"]
    if shutil.which(binary) is None:
        raise SystemExit(f"Binary not found in PATH: {binary}")

    if args.cleanup_stale:
        cleanup_stale_node_processes()

    timeout_sec = args.timeout_sec
    retries = args.retries
    if args.use_owner_profile:
        config = json.loads(CONFIG.read_text(encoding="utf-8"))
        profiles = config.get("dispatch_profiles", {})
        profile = profiles.get(data["owner"], {})
        timeout_sec = int(profile.get("timeout_sec", timeout_sec))
        retries = int(profile.get("retries", retries))
        print(f"Dispatch profile active: owner={data['owner']} timeout={timeout_sec}s retries={retries}")

    RUNS.mkdir(parents=True, exist_ok=True)
    run_log = RUNS / f"dispatch-{datetime.now().strftime('%Y%m%d-%H%M%S')}.log"
    run_log.write_text(
        (
            f"task={data['task']}\nowner={data['owner']}\nmodel={data['model']}\n"
            f"timeout_sec={timeout_sec}\nretries={retries}\ncommand={command}\n"
        ),
        encoding="utf-8",
    )
    code = run_with_retries(command, timeout_sec, retries, run_log)
    print(f"Dispatch log: {run_log}")
    if code != 0:
        try:
            lines = run_log.read_text(encoding="utf-8").splitlines()
            tail = lines[-args.log_tail_lines :] if args.log_tail_lines > 0 else lines
            print("\n--- log tail ---")
            print("\n".join(tail))
        except Exception:
            pass
        raise SystemExit(code)


if __name__ == "__main__":
    main()
