#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Validates a Known Build Error (KBE) ErrorMessage or ErrorPattern against a failure log
    using the same matching logic as Build Analysis.

.DESCRIPTION
    Tests whether an ErrorMessage (String.Contains, case-sensitive) or ErrorPattern
    (Regex with Singleline | IgnoreCase | NonBacktracking, 50ms timeout) would match
    against the provided log content. Supports single-string and multi-line array matching.

    Outputs PASS/FAIL, matched lines with line numbers, and a validated JSON blob ready
    to paste into a Known Build Error GitHub issue.

.PARAMETER ErrorMessage
    String or string array for String.Contains matching (case-sensitive).
    Use for exact substring matching. Cannot be combined with -ErrorPattern.

.PARAMETER ErrorPattern
    String or string array for regex matching.
    Uses RegexOptions: Singleline, IgnoreCase, NonBacktracking with 50ms timeout.
    Cannot be combined with -ErrorMessage.

.PARAMETER LogFile
    Path to a log file to test against. Cannot be combined with -LogContent.

.PARAMETER LogContent
    Log text string to test against. Cannot be combined with -LogFile.

.PARAMETER BuildRetry
    Whether Build Analysis should retry the build on match. Default: false.

.PARAMETER ExcludeConsoleLog
    Whether to exclude console logs from matching. Default: false.

.EXAMPLE
    ./Test-KnownIssuePattern.ps1 -ErrorMessage "Failed to retrieve information" -LogFile ./build.log

.EXAMPLE
    ./Test-KnownIssuePattern.ps1 -ErrorPattern "The command .+ failed" -LogContent "The command 'dotnet build' failed"

.EXAMPLE
    ./Test-KnownIssuePattern.ps1 -ErrorMessage "Assert.True() Failure","Actual:   False" -LogFile ./test.log
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory = $false)]
    [string[]]$ErrorMessage,

    [Parameter(Mandatory = $false)]
    [string[]]$ErrorPattern,

    [Parameter(Mandatory = $false)]
    [string]$LogFile,

    [Parameter(Mandatory = $false)]
    [string]$LogContent,

    [switch]$BuildRetry,

    [switch]$ExcludeConsoleLog
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

# --- Input validation ---

if ($ErrorMessage -and $ErrorPattern) {
    throw "Specify either -ErrorMessage or -ErrorPattern, not both."
}

if (-not $ErrorMessage -and -not $ErrorPattern) {
    throw "Specify either -ErrorMessage or -ErrorPattern."
}

if ($LogFile -and $LogContent) {
    throw "Specify either -LogFile or -LogContent, not both."
}

if (-not $LogFile -and -not $LogContent) {
    throw "Specify either -LogFile or -LogContent."
}

$useRegex = [bool]$ErrorPattern
$patterns = @(if ($useRegex) { $ErrorPattern } else { $ErrorMessage })

for ($i = 0; $i -lt $patterns.Count; $i++) {
    if ([string]::IsNullOrWhiteSpace($patterns[$i])) {
        $parameterName = if ($useRegex) { 'ErrorPattern' } else { 'ErrorMessage' }
        throw "-$parameterName contains a null, empty, or whitespace-only entry at position $i."
    }
}

$isMultiLine = $patterns.Count -gt 1

# Load log lines - use streaming for file input to avoid high memory usage on large logs
if ($LogFile) {
    if (-not (Test-Path $LogFile)) {
        throw "Log file not found: $LogFile"
    }
    $resolvedPath = (Resolve-Path $LogFile).Path
    if ((Get-Item $resolvedPath).Length -eq 0) {
        throw "Log file is empty."
    }
    $logLineSource = [System.IO.File]::ReadLines($resolvedPath)
} else {
    $logLineSource = @($LogContent -split "\r?\n")
    if ($logLineSource.Count -eq 0) {
        throw "Log content is empty."
    }
}

Write-Host "`nKnown Build Error Pattern Validator" -ForegroundColor Cyan
Write-Host "===================================" -ForegroundColor Cyan
Write-Host "Mode: $(if ($useRegex) { 'Regex (ErrorPattern)' } else { 'String (ErrorMessage)' })"
Write-Host "Pattern count: $($patterns.Count) $(if ($isMultiLine) { '(multi-line AND matching)' } else { '(single-line)' })"
Write-Host ""

# --- Validate regex patterns ---

if ($useRegex) {
    $regexOptions = [System.Text.RegularExpressions.RegexOptions]::Singleline -bor
                    [System.Text.RegularExpressions.RegexOptions]::IgnoreCase -bor
                    [System.Text.RegularExpressions.RegexOptions]::NonBacktracking
    $timeout = [TimeSpan]::FromMilliseconds(50)

    $compiledRegexes = @()
    for ($i = 0; $i -lt $patterns.Count; $i++) {
        try {
            $regex = [System.Text.RegularExpressions.Regex]::new($patterns[$i], $regexOptions, $timeout)
            $compiledRegexes += $regex
        } catch {
            Write-Host "FAIL: Invalid regex at position $($i): $($patterns[$i])" -ForegroundColor Red
            Write-Host "  Error: $_" -ForegroundColor Red
            exit 1
        }
    }
    Write-Host "Regex validation: OK" -ForegroundColor Green
}

# --- Matching logic (replicates Build Analysis) ---

function Test-SingleLineMatch {
    param([string]$line, [int]$patternIndex)

    if ($useRegex) {
        try {
            return $compiledRegexes[$patternIndex].IsMatch($line)
        } catch [System.Text.RegularExpressions.RegexMatchTimeoutException] {
            Write-Verbose "Regex timeout on line (50ms exceeded)"
            return $false
        }
    } else {
        return $line.IndexOf($patterns[$patternIndex], [System.StringComparison]::Ordinal) -ge 0
    }
}

$matchResults = [System.Collections.Generic.List[hashtable]]::new()
$overallMatch = $false
$totalLineCount = 0

if (-not $isMultiLine) {
    # Single pattern: stream lines and match each independently
    foreach ($line in $logLineSource) {
        $totalLineCount++
        if (Test-SingleLineMatch -line $line -patternIndex 0) {
            $matchResults.Add(@{
                LineNumber   = $totalLineCount
                LineContent  = $line.Trim()
                PatternIndex = 0
            })
            $overallMatch = $true
        }
    }
} else {
    # Multi-line: single-pass state machine matching patterns in order (O(N))
    $patternIdx = 0
    $currentMatches = [System.Collections.Generic.List[hashtable]]::new()
    foreach ($line in $logLineSource) {
        $totalLineCount++
        if (-not $overallMatch -and (Test-SingleLineMatch -line $line -patternIndex $patternIdx)) {
            $currentMatches.Add(@{
                LineNumber   = $totalLineCount
                LineContent  = $line.Trim()
                PatternIndex = $patternIdx
            })
            $patternIdx++
            if ($patternIdx -eq $patterns.Count) {
                $overallMatch = $true
                foreach ($m in $currentMatches) { $matchResults.Add($m) }
            }
        }
    }
}

Write-Host "Log lines processed: $totalLineCount"

# --- Output results ---

Write-Host ""
if ($overallMatch) {
    Write-Host "RESULT: PASS - Pattern matches the log" -ForegroundColor Green
    Write-Host ""
    Write-Host "Matched lines:" -ForegroundColor Yellow
    foreach ($match in $matchResults) {
        $patternLabel = if ($isMultiLine) { " [pattern $($match.PatternIndex)]" } else { "" }
        Write-Host "  Line $($match.LineNumber)${patternLabel}: $($match.LineContent)" -ForegroundColor Gray
    }
} else {
    Write-Host "RESULT: FAIL - Pattern does not match the log" -ForegroundColor Red
    Write-Host ""
    if ($isMultiLine) {
        Write-Host "Matched $patternIdx of $($patterns.Count) patterns. Stuck on pattern[$patternIdx]:" -ForegroundColor Yellow
        Write-Host "  ``$($patterns[$patternIdx])``" -ForegroundColor Yellow
        if ($currentMatches.Count -gt 0) {
            Write-Host ""
            Write-Host "Partial matches before failure:" -ForegroundColor Yellow
            foreach ($m in $currentMatches) {
                Write-Host "  pattern[$($m.PatternIndex)] matched line $($m.LineNumber): $($m.LineContent)" -ForegroundColor Gray
            }
        }
        Write-Host ""
        Write-Host "Possible causes:" -ForegroundColor Yellow
        Write-Host "  - The substring does not appear in any log line" -ForegroundColor Yellow
        Write-Host "  - It appears on a line already consumed by an earlier pattern (each element must match a *different* line)" -ForegroundColor Yellow
    } else {
        Write-Host "No log line contains the specified $(if ($useRegex) { 'pattern' } else { 'substring' })." -ForegroundColor Yellow
    }
    Write-Host ""
    exit 1
}

# --- Broadness warning ---

if (-not $isMultiLine -and $totalLineCount -gt 0) {
    $matchRatio = $matchResults.Count / $totalLineCount
    if ($matchRatio -gt 0.5) {
        Write-Host ""
        Write-Host "WARNING: Pattern matches $($matchResults.Count)/$($totalLineCount) lines ($([math]::Round($matchRatio * 100))%)." -ForegroundColor Red
        Write-Host "  This pattern is too broad and will cause false positives." -ForegroundColor Red
        Write-Host "  Add more specific text or use multi-line matching to narrow it down." -ForegroundColor Red
    } elseif ($matchRatio -gt 0.1) {
        Write-Host ""
        Write-Host "NOTE: Pattern matches $($matchResults.Count) lines. Review matches to ensure they all represent the same error." -ForegroundColor Yellow
    }
}

# --- Output validated JSON blob ---

Write-Host ""
Write-Host "Validated JSON blob:" -ForegroundColor Cyan
Write-Host "--------------------" -ForegroundColor Cyan

$jsonObj = [ordered]@{}
if ($useRegex) {
    if ($isMultiLine) {
        $jsonObj["ErrorPattern"] = $patterns
    } else {
        $jsonObj["ErrorPattern"] = $patterns[0]
    }
} else {
    if ($isMultiLine) {
        $jsonObj["ErrorMessage"] = $patterns
    } else {
        $jsonObj["ErrorMessage"] = $patterns[0]
    }
}
$jsonObj["BuildRetry"] = [bool]$BuildRetry
$jsonObj["ExcludeConsoleLog"] = [bool]$ExcludeConsoleLog

$jsonBlob = $jsonObj | ConvertTo-Json -Depth 3
Write-Host $jsonBlob -ForegroundColor White

# --- Output full issue template ---

Write-Host ""
Write-Host "Issue body template:" -ForegroundColor Cyan
Write-Host "--------------------" -ForegroundColor Cyan

# Build a code fence longer than any backtick sequence in the JSON to prevent markdown breakout
$fenceLen = 3
$backtickRuns = [regex]::Matches($jsonBlob, '`+')
foreach ($run in $backtickRuns) {
    if ($run.Length -ge $fenceLen) { $fenceLen = $run.Length + 1 }
}
$fence = '`' * $fenceLen

$template = @(
    "## Build Information"
    "Build: <!-- Add link to the AzDO build -->"
    "Leg Name: <!-- Add failing job name -->"
    ""
    "## Error Details"
    "<!-- Paste the full stack trace or exception output below -->"
    '```'
    "<!-- Replace this line with the full error output -->"
    '```'
    ""
    "## Error Message"
    "${fence}json"
    $jsonBlob
    $fence
) -join "`n"

Write-Host $template -ForegroundColor White
Write-Host ""
