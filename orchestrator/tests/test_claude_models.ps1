param(
  [int]$TimeoutSec = 90
)

$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $MyInvocation.MyCommand.Path | Split-Path -Parent
$ts = Get-Date -Format "yyyyMMdd-HHmmss"
$runDir = Join-Path $root "runs\model-test-$ts"
New-Item -ItemType Directory -Force -Path $runDir | Out-Null

$promptPath = Join-Path $runDir "prompt.md"
@"
ACCOUNT-CHECK: claude | task=MODEL-TEST | model=<dynamic>

You are in connectivity/model validation test.
Return EXACTLY this JSON and nothing else:
{"status":"ok","model":"<model>","test":"connectivity"}
"@ | Set-Content -Encoding UTF8 $promptPath

$models = @("claude-sonnet-4-6", "claude-opus-4-6")
$summary = @()

foreach ($model in $models) {
  $outFile = Join-Path $runDir ("response-" + $model + ".txt")
  $prompt = (Get-Content -Raw $promptPath).Replace("<model>", $model)
  try {
    $output = & claude --dangerously-skip-permissions --model $model -p $prompt 2>&1 | Out-String
    $output | Set-Content -Encoding UTF8 $outFile
    $status = if ([string]::IsNullOrWhiteSpace($output)) { "empty" } else { "ok" }
    $summary += "[$model] status=$status file=$outFile"
  }
  catch {
    $err = $_ | Out-String
    $err | Set-Content -Encoding UTF8 $outFile
    $summary += "[$model] status=error file=$outFile"
  }
}

$summaryPath = Join-Path $runDir "summary.txt"
$summary -join "`n" | Set-Content -Encoding UTF8 $summaryPath
Get-Content -Raw $summaryPath
