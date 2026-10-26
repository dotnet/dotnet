Set-StrictMode -Version Latest

# triage/timeout.ps1 - Which repo was actually building when a leg hit its time limit.
# Dot-sourced by report.ps1 and the tests; it defines functions only and runs nothing.
#
# repo-projects/Directory.Build.targets logs one marker when a repo build starts and one when it finishes:
#     2026-10-05T10:31:21.0000000Z   [10:31:21.05] Building arcade
#     2026-10-05T10:32:01.0000000Z   [10:32:01.82] Building arcade...done
# Repo builds run in parallel, so several repos are "open" at once, and a repo that merely logged an error in the same file is NOT
# necessarily the one that timed out.
# Job timeline evidence establishes a timeout; task-log markers identify which repos were still building.

function Get-VmrQBRepoBuildState {
    [CmdletBinding()]
    param([Parameter(Mandatory)][string] $Path)
    if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) { throw "Log not found: $Path" }
    $prefix = [regex]'^\d{4}-\d\d-\d\dT\d\d:\d\d:\d\d(?:\.\d+)?Z'
    $marker = [regex]'^(?<ts>\d{4}-\d\d-\d\dT\d\d:\d\d:\d\d(?:\.\d+)?Z)\s*\[[\d:.]+\]\s+Building\s+(?<repo>[A-Za-z0-9][A-Za-z0-9._-]*?)(?<done>\.\.\.done)?\s*$'
    $errorPattern = [regex]'(?i)(?:\berror\b\s*[A-Z]*\d*\s*:|##\[error\]|\bFAILED\b|exited with code [1-9])'
    $repoPath = [regex]'(?i)(?:^|[\\/ "''(])src[\\/](?<repo>[a-z0-9][a-z0-9._-]*)[\\/]'
    $stampOf = {
        param($line)
        $hit = $prefix.Match($line)
        if ($hit.Success) { [DateTimeOffset]::Parse($hit.Value, [Globalization.CultureInfo]::InvariantCulture, [Globalization.DateTimeStyles]::AssumeUniversal) }
    }
    $lines = [IO.File]::ReadAllLines($Path)
    $first = $null; $last = $null
    for ($i = 0; $i -lt $lines.Count -and $null -eq $first; $i++) { $first = & $stampOf $lines[$i] }
    for ($i = $lines.Count - 1; $i -ge 0 -and $null -eq $last; $i--) { $last = & $stampOf $lines[$i] }

    $repos = [ordered]@{}
    $errorRepos = [Collections.Generic.List[string]]::new()
    foreach ($text in $lines) {
        # Cheap string tests first: the regexes only run on the few lines that can match.
        if ($text.IndexOf('] Building ', [StringComparison]::Ordinal) -ge 0) {
            $match = $marker.Match($text)
            if (-not $match.Success) { continue }
            $repo = $match.Groups['repo'].Value.ToLowerInvariant()
            if (-not $repos.Contains($repo)) { $repos[$repo] = [pscustomobject]@{ repo = $repo; open = 0; lastStart = $null } }
            if ($match.Groups['done'].Success) { $repos[$repo].open = [Math]::Max(0, $repos[$repo].open - 1) }
            else { $repos[$repo].open++; $repos[$repo].lastStart = & $stampOf $match.Groups['ts'].Value }
        }
        elseif (($text.IndexOf('error', [StringComparison]::OrdinalIgnoreCase) -ge 0 -or $text.IndexOf('FAILED', [StringComparison]::Ordinal) -ge 0 -or
                $text.IndexOf('exited with code', [StringComparison]::Ordinal) -ge 0) -and $errorPattern.IsMatch($text)) {
            $hit = $repoPath.Match($text)
            if ($hit.Success -and $hit.Groups['repo'].Value.ToLowerInvariant() -notin $errorRepos) { $errorRepos.Add($hit.Groups['repo'].Value.ToLowerInvariant()) }
        }
    }
    return [pscustomobject]@{
        hasMarkers     = $repos.Count -gt 0
        reliable       = $repos.Count -gt 0
        repos          = @(foreach ($entry in $repos.Values) {
                [pscustomobject]@{
                    repo = $entry.repo; open = $entry.open; running = ($entry.open -gt 0)
                    runningSeconds = if ($entry.open -gt 0 -and $null -ne $entry.lastStart -and $null -ne $last) { [int]($last - $entry.lastStart).TotalSeconds } else { $null }
                }
            })
        # A nested folder such as src\sdk\src\Layout\ looks like a repo path; only names with build markers in THIS log are repos.
        errorRepos     = @($errorRepos | Where-Object { $repos.Contains($_) })
        elapsedMinutes = if ($null -ne $first -and $null -ne $last) { [int](($last - $first).TotalMinutes) } else { $null }
    }
}

function Get-VmrQBTimeoutAttribution {
    <#
    .SYNOPSIS
    The sentence-sized answer for a leg reported as timed out: was it a timeout, and which repo was running?
    .DESCRIPTION
    -JobTimedOut confirms the job timeout from its timeline evidence. A task log can span only part of a job, so its length
    neither proves nor disproves a job timeout. Without confirming evidence the verdict is uncertain.
    For a confirmed timeout, a repo is a SUSPECT only if it was unfinished AND had been running at least 10 minutes when the log ended;
    repos started moments before the end are BYSTANDERS. Repos that logged errors but are not suspects are listed separately, so an
    unrelated error in the same file can never be mistaken for the cause.
    -LimitIsAssumed labels the default limit; it is not evidence that the job timed out.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][object] $State,
        [ValidateRange(1, 1440)][int] $TimeLimitMinutes = 150,
        [switch] $LimitIsAssumed,
        [switch] $JobTimedOut
    )
    $minRunningMinutes = 10
    $open = @($State.repos | Where-Object running)
    $trusted = $State.reliable -and $JobTimedOut
    $suspects = @(if ($trusted) { $open | Where-Object { $null -ne $_.runningSeconds -and $_.runningSeconds -ge ($minRunningMinutes * 60) } })
    $bystanders = @(if ($trusted) { $open | Where-Object { $_ -notin $suspects } })
    $suspectNames = @($suspects | ForEach-Object repo)
    $errorOnly = @($State.errorRepos | Where-Object { $_ -notin $suspectNames })
    $describe = { param($repo) '{0} (running {1} min)' -f $repo.repo, [int]($repo.runningSeconds / 60) }
    $uncertain = -not [bool]$JobTimedOut
    $assumption = if ($LimitIsAssumed) { " The $TimeLimitMinutes-minute limit is the VMR default; the job's own limit was not available." } else { '' }
    $openText = if ($open.Count) { " Repo build(s) open at the end: $(@($open | ForEach-Object { & $describe $_ }) -join ', ')." } else { '' }
    $statement = if (-not $State.reliable) { 'No "Building <repo>" markers in this log; it is not a repo-build log, so no repo can be named from it.' }
        elseif ($uncertain) {
            "This task log spans $($State.elapsedMinutes) minutes, but the job's timeline did not establish a timeout, so the timeout verdict is uncertain.$openText Do not report a timeout or name a repo as its cause without confirming job evidence.$assumption"
        }
        elseif ($suspects.Count -eq 0) {
            $why = if ($bystanders.Count) { "$(@($bystanders | ForEach-Object { & $describe $_ }) -join ', ') only started just before the end, so none of them is the cause" }
                   else { 'every repo build that started also finished, so the time was spent outside a repo build' }
            "The job timeline confirms a timeout (limit $TimeLimitMinutes minutes), but no repo had been running for $minRunningMinutes minutes or more when this task log ended: $why."
        }
        else {
            $text = "The job timeline confirms a timeout (limit $TimeLimitMinutes minutes); this task log spans $($State.elapsedMinutes) minutes. Repo build(s) with no '...done' after running at least $minRunningMinutes minutes: $(@($suspects | ForEach-Object { & $describe $_ }) -join ', ')."
            if ($bystanders.Count) { $text += " Started just before the end, not suspects: $(@($bystanders | ForEach-Object repo) -join ', ')." }
            if ($errorOnly.Count) { $text += " Logged errors but were not running long: $($errorOnly -join ', ') - do not attribute the timeout to them." }
            $text
        }
    return [pscustomobject]@{
        reliable         = [bool]$State.reliable
        timedOut         = [bool]$JobTimedOut
        uncertain        = $uncertain
        timeLimitMinutes = $TimeLimitMinutes
        limitAssumed     = [bool]$LimitIsAssumed
        elapsedMinutes   = $State.elapsedMinutes
        repoAtTimeout    = $suspectNames
        bystanders       = @($bystanders | ForEach-Object repo)
        errorOnlyRepos   = $errorOnly
        statement        = $statement
    }
}
