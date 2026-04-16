# Phase 20 Dispatch Notes

## New flags
- `--owner`: active handoff owner override.
- `--model`: active handoff model override.
- `--run-file`: prompt file override (repo-relative path).
- `--log-tail-lines`: dispatch fail durumunda log sonundan basilan satir sayisi.

## Quick usage
- `python dispatch.py --owner claude --model claude-sonnet-4-6 --run-file tasks/sample_prompt.md`
- `python dispatch.py --execute --owner gemini --model gemini-3-flash --run-file tasks/sample_prompt.md --timeout-sec 90 --retries 1`
