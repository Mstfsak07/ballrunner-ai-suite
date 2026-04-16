param(
  [int]$TimeoutSec = 45,
  [switch]$CleanupStale
)

$ErrorActionPreference = "Continue"
$root = Split-Path -Parent $MyInvocation.MyCommand.Path
$runDir = Join-Path $root "runs"
New-Item -ItemType Directory -Force -Path $runDir | Out-Null
$stamp = Get-Date -Format "yyyyMMdd-HHmmss"
$log = Join-Path $runDir "healthcheck-$stamp.log"
$tempRoot = Join-Path $env:TEMP "ai-orchestrator-healthcheck"
New-Item -ItemType Directory -Force -Path $tempRoot | Out-Null

function Write-Log($text) {
  $text | Tee-Object -FilePath $log -Append
}

if ($CleanupStale) {
  Get-CimInstance Win32_Process |
    Where-Object { $_.Name -eq 'node.exe' -and ($_.CommandLine -like '*claude*' -or $_.CommandLine -like '*gemini*') } |
    ForEach-Object { Stop-Process -Id $_.ProcessId -Force -ErrorAction SilentlyContinue }
  Write-Log "stale cleanup done"
}

$checks = @(
  @{
    name = 'claude';
    cmd = 'claude --dangerously-skip-permissions --model claude-sonnet-4-6 -p "Reply exactly: OK"';
    workdir = $tempRoot
  },
  @{
    name = 'gemini';
    cmd = 'gemini --approval-mode yolo -m gemini-3-flash -o text -p "Reply exactly OK. Do not use tools. Do not read files. Do not inspect workspace."';
    workdir = $tempRoot
  }
)

foreach ($c in $checks) {
  Write-Log "=== $($c.name) ==="
  try {
    $job = Start-Job -ScriptBlock {
      param($command, $workdir)
      Set-Location $workdir
      powershell -NoProfile -NonInteractive -Command $command 2>&1 | Out-String
    } -ArgumentList $c.cmd, $c.workdir

    $completed = Wait-Job $job -Timeout $TimeoutSec
    if ($completed) {
      $out = Receive-Job $job
      Write-Log "status=ok"
      Write-Log $out
    } else {
      Stop-Job $job | Out-Null
      Write-Log "status=timeout"
    }
    Remove-Job $job | Out-Null
  }
  catch {
    Write-Log "status=error"
    Write-Log ($_ | Out-String)
  }
}

Write-Output "Health log: $log"
