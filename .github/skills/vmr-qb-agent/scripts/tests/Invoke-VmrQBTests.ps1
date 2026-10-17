<#
.SYNOPSIS
    Runs every Test-*.ps1 in this directory, each in its own pwsh process, and prints a PASS/FAIL table.

.DESCRIPTION
    A separate process per test keeps them from leaking into one another: several tests redefine
    functions named after production ones, override gh/Start-Sleep, or set environment variables.
    The tests are offline; none sends mail or reaches the network.

    A test passes when its process exits 0 within -TimeoutSeconds. The output of a failing test is
    printed after the table; -ShowOutput prints every test's output. The exit code is 1 when any
    test fails or times out, otherwise 0.

.EXAMPLE
    ./Invoke-VmrQBTests.ps1

.EXAMPLE
    ./Invoke-VmrQBTests.ps1 -Name Test-VmrQBPackage, Test-VmrIssueStore -ShowOutput
#>
#Requires -Version 7.0
[CmdletBinding()]
param(
    [string[]] $Name,
    [ValidateRange(10, 7200)][int] $TimeoutSeconds = 900,
    [switch] $ShowOutput
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$pwsh = (Get-Process -Id $PID).Path
$tests = @(Get-ChildItem -LiteralPath $PSScriptRoot -Filter 'Test-*.ps1' -File | Sort-Object Name)
if ($Name) {
    $wanted = @($Name | ForEach-Object { [IO.Path]::GetFileNameWithoutExtension($_) })
    $tests = @($tests | Where-Object { $_.BaseName -in $wanted })
    $missing = @($wanted | Where-Object { $_ -notin @($tests | ForEach-Object BaseName) })
    if ($missing) { throw "No such test: $($missing -join ', ')" }
}
if ($tests.Count -eq 0) { throw "No Test-*.ps1 files found in $PSScriptRoot." }

function Invoke-IsolatedTest {
    param([Parameter(Mandatory)][IO.FileInfo] $Test)

    $start = [Diagnostics.ProcessStartInfo]::new()
    $start.FileName = $pwsh
    foreach ($argument in @('-NoProfile', '-NonInteractive', '-File', $Test.FullName)) { $start.ArgumentList.Add($argument) }
    $start.WorkingDirectory = $PSScriptRoot
    $start.RedirectStandardOutput = $true
    $start.RedirectStandardError = $true
    $start.UseShellExecute = $false
    $start.CreateNoWindow = $true

    $stopwatch = [Diagnostics.Stopwatch]::StartNew()
    $process = [Diagnostics.Process]::new()
    $process.StartInfo = $start
    [void]$process.Start()
    # Both pipes are drained concurrently so a chatty test can never block on a full buffer.
    $outTask = $process.StandardOutput.ReadToEndAsync()
    $errTask = $process.StandardError.ReadToEndAsync()
    $finished = $process.WaitForExit($TimeoutSeconds * 1000)
    if (-not $finished) {
        try { $process.Kill($true) } catch { }
        [void]$process.WaitForExit(30000)
    }
    $stopwatch.Stop()
    $exitCode = if ($finished) { $process.ExitCode } else { -1 }
    $output = ($outTask.GetAwaiter().GetResult() + $errTask.GetAwaiter().GetResult()).TrimEnd()
    $process.Dispose()
    if (-not $finished) { $output += "`nTimed out after $TimeoutSeconds seconds and was terminated." }

    return [pscustomobject]@{
        Test = $Test.BaseName
        Result = if ($finished -and $exitCode -eq 0) { 'PASS' } elseif ($finished) { 'FAIL' } else { 'TIMEOUT' }
        Seconds = [math]::Round($stopwatch.Elapsed.TotalSeconds, 1)
        ExitCode = $exitCode
        Output = $output
    }
}

$results = [Collections.Generic.List[object]]::new()
foreach ($test in $tests) {
    Write-Host "Running $($test.BaseName)..." -ForegroundColor DarkGray
    $results.Add((Invoke-IsolatedTest -Test $test))
}

Write-Host ''
$results | Format-Table Test, Result, Seconds, ExitCode -AutoSize | Out-String -Width 200 | Write-Host
foreach ($result in $results) {
    if ($ShowOutput -or $result.Result -ne 'PASS') {
        Write-Host "---- $($result.Test) [$($result.Result)] output ----" -ForegroundColor $(if ($result.Result -eq 'PASS') { 'DarkGray' } else { 'Red' })
        Write-Host $result.Output
    }
}

$failed = @($results | Where-Object Result -ne 'PASS')
$total = [math]::Round(($results | Measure-Object Seconds -Sum).Sum, 1)
if ($failed.Count) {
    Write-Host "$($failed.Count) of $($results.Count) test(s) failed in ${total}s: $(($failed | ForEach-Object Test) -join ', ')" -ForegroundColor Red
    exit 1
}
Write-Host "All $($results.Count) test(s) passed in ${total}s." -ForegroundColor Green
exit 0
