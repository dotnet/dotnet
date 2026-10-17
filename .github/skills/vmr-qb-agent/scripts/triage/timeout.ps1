Set-StrictMode -Version Latest

# triage/timeout.ps1 - Which repo was actually building when a leg hit its time limit.
# Dot-sourced by Invoke-Triage.ps1 and the tests; it defines functions only and runs nothing.

# ---------------------------------------------------------------------------------------------
# evidence/RepoBuild.ps1
# ---------------------------------------------------------------------------------------------
# Which repo was actually building when a leg hit its time limit?
#
# repo-projects/Directory.Build.targets logs one marker when a repo build starts and one when it
# finishes:
#     [10:31:21.05] Building arcade
#     [10:32:01.82] Building arcade...done
# Repo builds run in parallel, so several repos are "open" at once, and a repo that merely logged an
# error in the same file is NOT necessarily the one that timed out. Three timeout reports in a row
# named the wrong repo because that was read off the log by eye. This reads it mechanically.
#
# Two further traps are handled because real data hit both:
#   * "Started but never finished" is also true of whichever repo started last when an upstream
#     failure tore the build down. In one real build 19 legs ended with sdk open for 0-17 minutes
#     while the actual timeouts were runtime and emsdk after 150+ minutes. A timeout needs the log to
#     have run to the time limit; otherwise the leg ended by failure or cancellation.
#   * The log download cap keeps the HEAD of a file, but the evidence for a timeout is in the TAIL.
#     A truncated log makes every repo that finishes after the cut look unfinished, so nothing is
#     concluded from one (-Truncated, or a .partial file name).

function Get-VmrQBRepoBuildState {
    [CmdletBinding(DefaultParameterSetName = 'Lines')]
    param(
        [Parameter(Mandatory, ParameterSetName = 'Lines', ValueFromPipeline)][AllowEmptyString()][AllowEmptyCollection()][string[]] $Line,
        [Parameter(Mandatory, ParameterSetName = 'Path')][string] $Path,
        [switch] $Truncated,
        [ValidateRange(1, 200)][int] $MaxErrors = 20
    )
    begin { $all = [Collections.Generic.List[string]]::new() }
    process { if ($PSCmdlet.ParameterSetName -eq 'Lines') { foreach ($l in $Line) { $all.Add($l) } } }
    end {
        if ($PSCmdlet.ParameterSetName -eq 'Path') {
            if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) { throw "Log not found: $Path" }
            foreach ($l in [IO.File]::ReadLines($Path)) { $all.Add($l) }
            # The collector marks a capped download with a .partial suffix.
            if ($Path -match '\.partial$') { $Truncated = $true }
        }

        $marker = [regex]'^(?:(?<ts>\d{4}-\d\d-\d\dT\d\d:\d\d:\d\d(?:\.\d+)?Z)\s*)?\[(?<clock>\d\d:\d\d:\d\d(?:\.\d+)?)\]\s+Building\s+(?<repo>[A-Za-z0-9][A-Za-z0-9._-]*?)(?<done>\.\.\.done)?\s*$'
        $errorPattern = [regex]'(?i)(?:\berror\b\s*[A-Z]*\d*\s*:|##\[error\]|\bFAILED\b|exited with code [1-9])'
        $repoPath = [regex]'(?i)(?:^|[\\/ "''(])src[\\/](?<repo>[a-z0-9][a-z0-9._-]*)[\\/]'
        $prefixPattern = [regex]'^\d{4}-\d\d-\d\dT\d\d:\d\d:\d\d(?:\.\d+)?Z'
        $parseUtc = { param($text) [DateTimeOffset]::Parse($text, [Globalization.CultureInfo]::InvariantCulture, [Globalization.DateTimeStyles]::AssumeUniversal) }

        $state = [ordered]@{}
        $lastMarkerStamp = $null
        $lastClock = $null
        $dayOffset = 0
        $errors = [Collections.Generic.List[object]]::new()
        $lineNumber = 0
        foreach ($text in $all) {
            $lineNumber++
            # Cheap string tests first: the regexes only run on the few lines that can match.
            if ($text.IndexOf('] Building ', [StringComparison]::Ordinal) -lt 0) {
                if ($errors.Count -lt $MaxErrors -and
                    ($text.IndexOf('error', [StringComparison]::OrdinalIgnoreCase) -ge 0 -or
                     $text.IndexOf('FAILED', [StringComparison]::Ordinal) -ge 0 -or
                     $text.IndexOf('exited with code', [StringComparison]::Ordinal) -ge 0) -and
                    $errorPattern.IsMatch($text)) {
                    $repoHit = $repoPath.Match($text)
                    $clipped = $text.Trim(); if ($clipped.Length -gt 220) { $clipped = $clipped.Substring(0, 220) }
                    $errors.Add([pscustomobject]@{
                        line = $lineNumber; text = $clipped
                        repo = if ($repoHit.Success) { $repoHit.Groups['repo'].Value.ToLowerInvariant() } else { $null }
                    })
                }
                continue
            }
            $match = $marker.Match($text)
            if (-not $match.Success) { continue }

            # Prefer the agent's UTC prefix; fall back to the bracketed local clock, which wraps at midnight.
            if ($match.Groups['ts'].Success) { $stamp = & $parseUtc $match.Groups['ts'].Value }
            else {
                $clock = [TimeSpan]::Parse($match.Groups['clock'].Value, [Globalization.CultureInfo]::InvariantCulture)
                if ($null -ne $lastClock -and ($lastClock - $clock).TotalHours -gt 12) { $dayOffset++ }
                $lastClock = $clock
                $stamp = [DateTimeOffset]::new(2000, 1, 1, 0, 0, 0, [TimeSpan]::Zero).AddDays($dayOffset).Add($clock)
            }
            $lastMarkerStamp = $stamp

            $repo = $match.Groups['repo'].Value.ToLowerInvariant()
            if (-not $state.Contains($repo)) {
                $state[$repo] = [pscustomobject]@{ repo = $repo; starts = 0; completions = 0; firstStart = $null; lastStart = $null; lastDone = $null; startLine = 0 }
            }
            $entry = $state[$repo]
            if ($match.Groups['done'].Success) { $entry.completions++; $entry.lastDone = $stamp }
            else {
                $entry.starts++; $entry.lastStart = $stamp
                if ($null -eq $entry.firstStart) { $entry.firstStart = $stamp; $entry.startLine = $lineNumber }
            }
        }

        # First and last timestamped lines of the whole log (not just markers), found from each end.
        $firstTimestamp = $null; $lastTimestamp = $null
        for ($i = 0; $i -lt $all.Count; $i++) {
            $hit = $prefixPattern.Match($all[$i]); if ($hit.Success) { $firstTimestamp = & $parseUtc $hit.Value; break }
        }
        for ($i = $all.Count - 1; $i -ge 0; $i--) {
            $hit = $prefixPattern.Match($all[$i]); if ($hit.Success) { $lastTimestamp = & $parseUtc $hit.Value; break }
        }
        if ($null -eq $lastTimestamp) { $lastTimestamp = $lastMarkerStamp }
        elseif ($null -ne $lastMarkerStamp -and $lastMarkerStamp -gt $lastTimestamp) { $lastTimestamp = $lastMarkerStamp }
        if ($null -eq $firstTimestamp) {
            # No marker has a start either on a plain log, so there may be nothing to pick: that leaves $null, never an index error.
            $firstTimestamp = $state.Values | Where-Object { $null -ne $_.firstStart } | ForEach-Object firstStart | Sort-Object | Select-Object -First 1
        }
        $lastLines = [Collections.Generic.List[string]]::new()
        for ($i = $all.Count - 1; $i -ge 0 -and $lastLines.Count -lt 3; $i--) {
            if (-not [string]::IsNullOrWhiteSpace($all[$i])) { $lastLines.Insert(0, $all[$i].Trim()) }
        }

        # A nested folder such as src\sdk\src\Layout\ looks like a repo path. Only names that have
        # build markers in THIS log are repos.
        foreach ($errorEntry in $errors) { if ($errorEntry.repo -and -not $state.Contains($errorEntry.repo)) { $errorEntry.repo = $null } }
        $errorRepos = @($errors | Where-Object repo | ForEach-Object repo | Select-Object -Unique)

        $repos = @(foreach ($entry in $state.Values) {
            $open = [Math]::Max(0, $entry.starts - $entry.completions)
            [pscustomobject]@{
                repo = $entry.repo; starts = $entry.starts; completions = $entry.completions; open = $open
                running = ($open -gt 0); firstStart = $entry.firstStart; lastDone = $entry.lastDone; startLine = $entry.startLine
                runningSeconds = if ($open -gt 0 -and $null -ne $entry.lastStart -and $null -ne $lastTimestamp) { [int]($lastTimestamp - $entry.lastStart).TotalSeconds } else { $null }
                # First start to last finish (or to the end of the log while still open): where the time went.
                spanSeconds = $(
                    $end = if ($open -gt 0) { $lastTimestamp } else { $entry.lastDone }
                    if ($null -ne $entry.firstStart -and $null -ne $end) { [int]($end - $entry.firstStart).TotalSeconds } else { $null })
            }
        })
        $hasMarkers = $repos.Count -gt 0
        $reliable = $hasMarkers -and -not $Truncated
        $unfinished = @($repos | Where-Object running | Sort-Object firstStart, startLine | ForEach-Object repo)
        $elapsedMinutes = if ($null -ne $firstTimestamp -and $null -ne $lastTimestamp) { [int](($lastTimestamp - $firstTimestamp).TotalMinutes) } else { $null }

        $summary = if (-not $hasMarkers) { 'No "Building <repo>" markers in this log; it is not a repo-build log, so no repo can be named from it.' }
            elseif ($Truncated) { "Log is truncated (head kept, tail missing); $($unfinished.Count) repo(s) look unfinished but that cannot be trusted. Fetch the tail of the log before naming a repo." }
            elseif ($unfinished.Count -eq 0) { 'Every repo build that started also finished.' }
            else { "Unfinished when the log ended: $($unfinished -join ', ')." }

        return [pscustomobject]@{
            hasMarkers     = $hasMarkers
            truncated      = [bool]$Truncated
            reliable       = $reliable
            repos          = $repos
            unfinished     = $unfinished
            errorRepos     = $errorRepos
            errors         = $errors.ToArray()
            firstTimestamp = $firstTimestamp
            lastTimestamp  = $lastTimestamp
            elapsedMinutes = $elapsedMinutes
            lastLines      = $lastLines.ToArray()
            summary        = $summary
        }
    }
}

function Get-VmrQBTimeoutAttribution {
    <#
    .SYNOPSIS
    The sentence-sized answer for a leg reported as timed out: was it a timeout, and which repo was running?
    .DESCRIPTION
    A leg counts as having run to its limit only if the log spans at least 90% of -TimeLimitMinutes
    (default 150, the VMR leg limit; pass the number from the job's own "maximum time of N minutes"
    message when you have it). A shorter log ended by failure or cancellation, and no repo is blamed
    for a timeout that did not happen.
    For a real timeout, a repo is a SUSPECT only if it was unfinished AND had been running at least
    -MinRunningMinutes when the log ended; repos started moments before the end are BYSTANDERS.
    Repos that logged errors but were not suspects are listed separately, so an unrelated MSBuild error
    in the same file can never be mistaken for the cause. Nothing is concluded from a truncated log.
    -LimitIsAssumed says the limit is the default rather than the job's own. A log that stops short of an assumed limit is then
    reported as uncertain (the job may have had a shorter limit and timed out), never as a leg that failed or was cancelled.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][object] $State,
        [ValidateRange(1, 1440)][int] $TimeLimitMinutes = 150,
        [ValidateRange(1, 600)][int] $MinRunningMinutes = 10,
        [switch] $LimitIsAssumed
    )
    $open = @($State.repos | Where-Object running)
    $nearLimit = $null -ne $State.elapsedMinutes -and $State.elapsedMinutes -ge [Math]::Floor($TimeLimitMinutes * 0.9)
    $trusted = $State.reliable -and $nearLimit
    $suspects = @(if ($trusted) { $open | Where-Object { $null -ne $_.runningSeconds -and $_.runningSeconds -ge ($MinRunningMinutes * 60) } })
    $bystanders = @(if ($trusted) { $open | Where-Object { $_ -notin $suspects } })
    $suspectNames = @($suspects | ForEach-Object repo)
    $errorOnly = @($State.errorRepos | Where-Object { $_ -notin $suspectNames })
    $describe = { param($repo) '{0} (running {1} min)' -f $repo.repo, [int]($repo.runningSeconds / 60) }
    $longest = @($State.repos | Where-Object { $null -ne $_.spanSeconds } | Sort-Object spanSeconds -Descending | Select-Object -First 3)
    $spans = if ($trusted -and $longest.Count) { " Where the time went (first start to last finish): $(@($longest | ForEach-Object { '{0} {1} min{2}' -f $_.repo, [int]($_.spanSeconds / 60), $(if ($_.running) { ' (never finished)' } else { '' }) }) -join ', ')." } else { '' }

    $uncertain = [bool]($LimitIsAssumed -and $State.reliable -and -not $nearLimit)
    $assumption = if ($LimitIsAssumed) { " The $TimeLimitMinutes-minute limit is the VMR default; the job's own limit was not available." } else { '' }
    $statement = if (-not $State.reliable) { $State.summary }
        elseif (-not $nearLimit -and $LimitIsAssumed) {
            $names = if ($open.Count) { " Repo build(s) open at the end: $(@($open | ForEach-Object { & $describe $_ }) -join ', ')." } else { '' }
            "This log spans $($State.elapsedMinutes) minutes, short of the $TimeLimitMinutes-minute default, but the job's own time limit was not available, so whether this leg timed out is uncertain.$names Do not report it as a timeout, and do not name a repo as its cause, until the job's own limit (its 'maximum time of N minutes' message) is known."
        }
        elseif (-not $nearLimit) {
            $names = if ($open.Count) { " Repo build(s) open at the end: $(@($open | ForEach-Object { & $describe $_ }) -join ', ')." } else { '' }
            "This log spans $($State.elapsedMinutes) minutes, short of the $TimeLimitMinutes-minute limit, so the leg ended by failure or cancellation, not by timing out.$names Do not report it as a timeout."
        }
        elseif ($suspects.Count -eq 0 -and $open.Count -eq 0) { "The log ran to the time limit but every repo build that started also finished; the time was spent outside a repo build.$spans$assumption" }
        elseif ($suspects.Count -eq 0) { "No repo had been running for $MinRunningMinutes minutes or more when the log ended; $(@($bystanders | ForEach-Object { & $describe $_ }) -join ', ') only started just before the end, so none of them is the cause.$spans$assumption" }
        else {
            $text = "The log ran $($State.elapsedMinutes) minutes (limit $TimeLimitMinutes). Repo build(s) with no '...done' after running at least $MinRunningMinutes minutes: $(@($suspects | ForEach-Object { & $describe $_ }) -join ', ')."
            $text += $spans
            $text += $assumption
            if ($bystanders.Count) { $text += " Started just before the end, not suspects: $(@($bystanders | ForEach-Object repo) -join ', ')." }
            if ($errorOnly.Count) { $text += " Logged errors but were not running long: $($errorOnly -join ', ') - do not attribute the timeout to them." }
            $text
        }
    return [pscustomobject]@{
        reliable       = [bool]$State.reliable
        timedOut       = [bool]$trusted
        uncertain      = $uncertain
        timeLimitMinutes = $TimeLimitMinutes
        limitAssumed   = [bool]$LimitIsAssumed
        elapsedMinutes = $State.elapsedMinutes
        repoAtTimeout  = $suspectNames
        bystanders     = @($bystanders | ForEach-Object repo)
        errorOnlyRepos = $errorOnly
        statement      = $statement
    }
}
