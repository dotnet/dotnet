---
name: trx-analysis
description: Parse and analyze Visual Studio TRX test result files. Use when asked about slow tests, test durations, test frequency, flaky tests, failure analysis, or test execution patterns from TRX files.
---

# TRX Test Results Analysis

Parse `.trx` files (Visual Studio Test Results XML) to answer questions about test performance, frequency, failures, and patterns.

## TRX File Format

TRX files use XML namespace `http://microsoft.com/schemas/VisualStudio/TeamTest/2010`. Key elements:

- `TestRun.Results.UnitTestResult` — individual test executions with `testName`, `duration` (HH:mm:ss.fffffff), `outcome` (Passed/Failed/NotExecuted)
- `TestRun.TestDefinitions.UnitTest` — test metadata including class and method info
- `TestRun.ResultSummary` — aggregate pass/fail/skip counts

## Loading a TRX File

```powershell
[xml]$trx = Get-Content "path/to/file.trx"
$results = $trx.TestRun.Results.UnitTestResult
```

## Common Queries

### Top N slowest tests

```powershell
$results | ForEach-Object {
    [PSCustomObject]@{
        Test     = $_.testName
        Seconds  = [TimeSpan]::Parse($_.duration).TotalSeconds
        Outcome  = $_.outcome
    }
} | Sort-Object Seconds -Descending | Select-Object -First 25 |
  Format-Table @{L='Sec';E={'{0,6:N1}' -f $_.Seconds}}, Outcome, Test -AutoSize
```

### Slowest test from each distinct class (top N)

```powershell
$results | ForEach-Object {
    $parts = $_.testName -split '\.'
    [PSCustomObject]@{
        Test      = $_.testName
        ClassName = ($parts[0..($parts.Length-2)] -join '.')
        Seconds   = [TimeSpan]::Parse($_.duration).TotalSeconds
    }
} | Sort-Object Seconds -Descending |
  Group-Object ClassName | ForEach-Object { $_.Group | Select-Object -First 1 } |
  Sort-Object Seconds -Descending | Select-Object -First 10 |
  Format-Table @{L='Sec';E={'{0,6:N1}' -f $_.Seconds}}, ClassName, Test -AutoSize
```

### Most-executed tests (parameterization frequency)

Extract the base method name before parameterization and count runs:

```powershell
$results | ForEach-Object {
    $name = $_.testName
    if ($name -match '^(\S+?)[\s(]') { $base = $Matches[1] } else { $base = $name }
    [PSCustomObject]@{ Base = $base; Seconds = [TimeSpan]::Parse($_.duration).TotalSeconds }
} | Group-Object Base | ForEach-Object {
    [PSCustomObject]@{
        Runs     = $_.Count
        TotalSec = ($_.Group | Measure-Object Seconds -Sum).Sum
        Test     = $_.Name
    }
} | Sort-Object TotalSec -Descending | Select-Object -First 20 |
  Format-Table @{L='Runs';E={$_.Runs}}, @{L='TotalSec';E={'{0,7:N1}' -f $_.TotalSec}}, Test -AutoSize
```

### Failed tests

```powershell
$results | Where-Object { $_.outcome -eq 'Failed' } | ForEach-Object {
    [PSCustomObject]@{
        Test    = $_.testName
        Seconds = [TimeSpan]::Parse($_.duration).TotalSeconds
        Error   = $_.Output.ErrorInfo.Message
    }
} | Format-Table -Wrap
```

### Summary statistics

```powershell
$summary = $trx.TestRun.ResultSummary.Counters
[PSCustomObject]@{
    Total    = $summary.total
    Passed   = $summary.passed
    Failed   = $summary.failed
    Skipped  = $summary.notExecuted
    Duration = $trx.TestRun.Times.finish
} | Format-List
```

## Cross-File Duplicate Analysis

Compare two TRX files to find tests that appear in both and ran (were not skipped) in both. Useful for identifying redundant CI work across different configurations (e.g., net9.0 x64 vs net48 x86).

### Load and find duplicates that ran in both files

```powershell
[xml]$trx1 = Get-Content "path/to/file1.trx"
[xml]$trx2 = Get-Content "path/to/file2.trx"

$r1 = $trx1.TestRun.Results.UnitTestResult
$r2 = $trx2.TestRun.Results.UnitTestResult

# Build lookup: testName -> (outcome, duration) keeping best outcome per name
function Get-TestLookup($results) {
    $lookup = @{}
    foreach ($r in $results) {
        $name = $r.testName
        $outcome = $r.outcome
        $dur = [TimeSpan]::Parse($r.duration)
        if (-not $lookup.ContainsKey($name) -or ($lookup[$name].Outcome -eq 'NotExecuted' -and $outcome -ne 'NotExecuted')) {
            $lookup[$name] = [PSCustomObject]@{ Outcome = $outcome; Duration = $dur }
        }
    }
    $lookup
}

$t1 = Get-TestLookup $r1
$t2 = Get-TestLookup $r2

$skipped = @('NotExecuted','Pending','Disconnected','Warning','InProgress','Inconclusive')
$common = $t1.Keys | Where-Object { $t2.ContainsKey($_) -and $t1[$_].Outcome -notin $skipped -and $t2[$_].Outcome -notin $skipped }
```

### Separate non-parametrized vs parametrized duplicates

Parametrized tests contain `(` in their name (e.g., `RunAllTests (Row: 0, Runner = net10.0, ...)`). The base method name is everything before the first `(`.

```powershell
$nonParam = $common | Where-Object { $_ -notmatch '\(' }
$param    = $common | Where-Object { $_ -match '\(' }
```

### Non-parametrized duplicates ordered by duration

```powershell
$nonParam | ForEach-Object {
    $d1 = $t1[$_].Duration; $d2 = $t2[$_].Duration
    [PSCustomObject]@{
        Test       = $_
        File1Sec   = $d1.TotalSeconds
        File2Sec   = $d2.TotalSeconds
        TotalSec   = $d1.TotalSeconds + $d2.TotalSeconds
    }
} | Sort-Object TotalSec -Descending |
  Format-Table @{L='File1';E={'{0,6:N1}' -f $_.File1Sec}},
               @{L='File2';E={'{0,6:N1}' -f $_.File2Sec}},
               @{L='Total';E={'{0,6:N1}' -f $_.TotalSec}}, Test -AutoSize
```

### Parametrized duplicates squashed by base method

Tests with `(Row: ...)` or other parameterization are instances of the same test. Squash them into one row per base method, showing variant count, max single-instance duration, and total duration across all instances in both files.

```powershell
$param | ForEach-Object {
    if ($_ -match '^(.+?)\s*\(') { $base = $Matches[1] } else { $base = $_ }
    $d1 = $t1[$_].Duration; $d2 = $t2[$_].Duration
    [PSCustomObject]@{ Base = $base; D1 = $d1.TotalSeconds; D2 = $d2.TotalSeconds; Max = [Math]::Max($d1.TotalSeconds, $d2.TotalSeconds) }
} | Group-Object Base | ForEach-Object {
    [PSCustomObject]@{
        Test         = $_.Name
        Variants     = $_.Count
        OneInstance   = ($_.Group | Measure-Object Max -Maximum).Maximum
        AllInstances  = ($_.Group | Measure-Object { $_.D1 + $_.D2 } -Sum).Sum
    }
} | Sort-Object AllInstances -Descending |
  Format-Table @{L='Variants';E={$_.Variants}},
               @{L='1 Instance';E={'{0,7:N1}s' -f $_.OneInstance}},
               @{L='All Instances';E={'{0,7:N1}s' -f $_.AllInstances}}, Test -AutoSize
```

## Tips

- Parameterized tests appear as separate `UnitTestResult` entries. Use regex `'^(\S+?)[\s(]'` to extract the base method name.
- Sort by **TotalSec** (runs × avg duration) to find tests that consume the most CI time overall, even if each individual run is fast.
- When comparing files, filter out `NotExecuted` tests — many parameterized tests are skipped in one configuration but not the other, so raw name overlap overstates true duplication.
- TRX files from CI are typically found in `TestResults/` or as pipeline artifacts.
