<#
.SYNOPSIS
    Retrieves historical failure timeline for Known Build Error issues by analyzing
    the edit history of the issue body's hit count table.

.DESCRIPTION
    The dotnet Build Analysis bot periodically edits Known Build Error issues to update
    rolling hit count tables (24-hour, 7-day, 1-month). This script queries the GitHub
    GraphQL API for the edit history (userContentEdits) and reconstructs a failure
    timeline by detecting when the 24-hour count transitions from 0 to a positive value.

    Can analyze a single issue or scan all open Known Build Error issues in a repository
    to find the most actively failing tests.

.PARAMETER IssueNumber
    The GitHub issue number of a Known Build Error to analyze.

.PARAMETER Repository
    The GitHub repository (owner/repo format). Default: dotnet/runtime

.PARAMETER ListActive
    Scan all open "Known Build Error" issues and rank by recent failure activity.

.PARAMETER MaxIssues
    Maximum number of issues to scan when using -ListActive. Default: 50

.PARAMETER Since
    Only show failure events and periods on or after this date (ISO 8601 format,
    e.g. 2025-01-01). Failure periods are included if their End date is on or after
    the filter date, even if their Start date is earlier.

.PARAMETER Json
    Output structured JSON summary instead of human-readable table.

.EXAMPLE
    .\Get-KnownIssueHistory.ps1 -IssueNumber 100088

.EXAMPLE
    .\Get-KnownIssueHistory.ps1 -IssueNumber 100088 -Since 2025-01-01

.EXAMPLE
    .\Get-KnownIssueHistory.ps1 -IssueNumber 100088 -Json

.EXAMPLE
    .\Get-KnownIssueHistory.ps1 -ListActive -Repository dotnet/runtime
#>

[CmdletBinding(DefaultParameterSetName = 'SingleIssue')]
param(
    [Parameter(Mandatory, ParameterSetName = 'SingleIssue', Position = 0)]
    [ValidateRange(1, [int]::MaxValue)]
    [int]$IssueNumber,

    [Parameter()]
    [string]$Repository = "dotnet/runtime",

    [Parameter(Mandatory, ParameterSetName = 'ListActive')]
    [switch]$ListActive,

    [Parameter(ParameterSetName = 'ListActive')]
    [ValidateRange(1,100)]
    [int]$MaxIssues = 50,

    [Parameter()]
    [string]$Since,

    [Parameter()]
    [switch]$Json
)

$ErrorActionPreference = 'Stop'

function Split-Repository {
    param([string]$Repo)
    if ($Repo -notmatch '^[a-zA-Z0-9._-]+/[a-zA-Z0-9._-]+$') {
        throw "Invalid repository format '$Repo'. Expected 'owner/repo' (alphanumeric, dots, hyphens, underscores only)."
    }
    $parts = $Repo -split '/'
    return @{ Owner = $parts[0]; Name = $parts[1] }
}

function Get-IssueEditHistory {
    param(
        [string]$Owner,
        [string]$RepoName,
        [int]$Number,
        [int]$MaxPages = 50
    )

    $allNodes = [System.Collections.Generic.List[object]]::new()
    $totalCount = 0
    $hasNextPage = $true
    $endCursor = $null
    $pageCount = 0
    $truncated = $false

    # Paginate through edits (API returns max 100 per page)
    while ($hasNextPage) {
        $afterClause = if ($endCursor) { ", after: `"$endCursor`"" } else { "" }
        $query = @"
{
  repository(owner: "$Owner", name: "$RepoName") {
    issue(number: $Number) {
      title
      createdAt
      state
      labels(first: 100) { nodes { name } }
      userContentEdits(first: 100$afterClause) {
        totalCount
        pageInfo { hasNextPage endCursor }
        nodes {
          editedAt
          editor { login }
          diff
        }
      }
    }
  }
}
"@
        $result = gh api graphql -f "query=$query" 2>&1
        if ($LASTEXITCODE -ne 0) {
            throw "GraphQL query failed: $result"
        }

        $data = ($result -join "`n") | ConvertFrom-Json

        if ($data.errors) {
            $msgs = ($data.errors | ForEach-Object { $_.message }) -join '; '
            throw "GraphQL errors for issue #$Number in ${Owner}/${RepoName}: $msgs"
        }

        $issue = $data.data.repository.issue

        if (-not $issue) {
            throw "Issue #$Number not found in $Owner/$RepoName"
        }

        $edits = $issue.userContentEdits
        if ($totalCount -eq 0) { $totalCount = $edits.totalCount }
        $allNodes.AddRange(@($edits.nodes))
        $hasNextPage = $edits.pageInfo.hasNextPage
        $endCursor = $edits.pageInfo.endCursor
        $pageCount++

        if ($hasNextPage -and $pageCount -ge $MaxPages) {
            $hasNextPage = $false
            $truncated = $true
            Write-Warning "Issue #$Number has more than $($pageCount * 100) edits; results truncated to $($allNodes.Count) edits."
        }
    }

    return @{
        Title      = $issue.title
        CreatedAt  = $issue.createdAt
        State      = $issue.state
        Labels     = @($issue.labels.nodes | ForEach-Object { $_.name })
        Edits      = @($allNodes | Sort-Object { $_.editedAt })
        TotalEdits = $totalCount
        Truncated  = $truncated
    }
}

function Parse-HitCounts {
    param([array]$Edits)

    $timeline = [System.Collections.Generic.List[object]]::new()
    $prev24h = $null

    foreach ($edit in $Edits) {
        $date = $edit.editedAt
        $editor = if ($edit.editor) { $edit.editor.login } else { "unknown" }
        $diff = if ($edit.diff) { $edit.diff } else { "" }

        # Match hit count data rows, optionally prefixed with a unified diff marker (+, -, or space).
        # Skip removed (-) rows so each edit contributes only the current/added snapshot.
        $matches = [regex]::Matches($diff, '(?m)^([+ -])?\|(\d+)\|(\d+)\|(\d+)\|\s*$')

        foreach ($m in $matches) {
            $prefix = $m.Groups[1].Value
            if ($prefix -eq '-') { continue }

            $h24 = [int]$m.Groups[2].Value
            $h7d = [int]$m.Groups[3].Value
            $h1mo = [int]$m.Groups[4].Value

            $event = "update"
            if ($null -eq $prev24h -and $h24 -gt 0) {
                # First edit with hit counts and 24h > 0 — treat as initial failure
                $event = "new_failure"
            }
            elseif ($null -ne $prev24h -and $h24 -gt 0 -and $prev24h -eq 0) {
                $event = "new_failure"
            }
            elseif ($null -ne $prev24h -and $h24 -gt $prev24h) {
                $event = "additional_failure"
            }
            elseif ($h24 -eq 0 -and $h7d -eq 0 -and $h1mo -eq 0) {
                $event = "all_clear"
            }

            $entry = [PSCustomObject]@{
                Date    = $date
                Editor  = $editor
                Hit24h  = $h24
                Hit7d   = $h7d
                Hit1mo  = $h1mo
                Event   = $event
            }

            $prev24h = $h24
            $timeline.Add($entry)
        }
    }

    return $timeline
}

function ConvertTo-UtcDateTime {
    param([object]$Value)
    if ($Value -is [datetime]) { return $Value.ToUniversalTime() }
    return [DateTime]::Parse(
        $Value,
        [System.Globalization.CultureInfo]::InvariantCulture,
        [System.Globalization.DateTimeStyles]::AssumeUniversal -bor [System.Globalization.DateTimeStyles]::AdjustToUniversal
    )
}

function Filter-Since {
    param([array]$Items, [string]$SinceDate, [string]$DateProperty = 'Date')
    if (-not $SinceDate) { return $Items }
    $sinceDateTime = ConvertTo-UtcDateTime $SinceDate
    return @($Items | Where-Object { (ConvertTo-UtcDateTime $_.$DateProperty) -ge $sinceDateTime })
}

function Get-FailureEvents {
    param([array]$Timeline)

    return @($Timeline | Where-Object { $_.Event -eq 'new_failure' -or $_.Event -eq 'additional_failure' })
}

function Get-FailurePeriods {
    param([array]$Timeline)

    $periods = @()
    $inFailure = $false
    $currentPeriod = $null

    foreach ($entry in $Timeline) {
        if (($entry.Event -eq 'new_failure' -or $entry.Event -eq 'additional_failure') -and -not $inFailure) {
            # Start of a new failure period
            $inFailure = $true
            $currentPeriod = @{
                Start      = $entry.Date
                End        = $entry.Date
                PeakHit24h = $entry.Hit24h
                PeakHit1mo = $entry.Hit1mo
                Events     = @($entry)
            }
        }
        elseif ($inFailure -and ($entry.Event -eq 'new_failure' -or $entry.Event -eq 'additional_failure')) {
            # Continuation of existing period (another failure before all_clear)
            $currentPeriod.Events += $entry
            $currentPeriod.End = $entry.Date
            if ($entry.Hit24h -gt $currentPeriod.PeakHit24h) {
                $currentPeriod.PeakHit24h = $entry.Hit24h
            }
            if ($entry.Hit1mo -gt $currentPeriod.PeakHit1mo) {
                $currentPeriod.PeakHit1mo = $entry.Hit1mo
            }
        }
        elseif ($inFailure -and $entry.Event -ne 'all_clear') {
            # Non-failure update within a period — track peak counts as they decay
            $currentPeriod.End = $entry.Date
            if ($entry.Hit1mo -gt $currentPeriod.PeakHit1mo) {
                $currentPeriod.PeakHit1mo = $entry.Hit1mo
            }
        }
        elseif ($entry.Event -eq 'all_clear' -and $inFailure) {
            $inFailure = $false
            $currentPeriod.End = $entry.Date
            $periods += [PSCustomObject]$currentPeriod
            $currentPeriod = $null
        }
    }

    # Close any open period
    if ($inFailure -and $currentPeriod) {
        $periods += [PSCustomObject]$currentPeriod
    }

    return $periods
}

function Format-ShortDate {
    param([object]$Value)
    $dt = ConvertTo-UtcDateTime $Value
    return $dt.ToString("yyyy-MM-dd")
}

function Format-Timeline {
    param(
        [hashtable]$IssueData,
        [array]$Timeline,
        [array]$FailureEvents,
        [array]$FailurePeriods,
        [int]$IssueNum,
        [string]$Repo,
        [string]$SinceFilter,
        [int]$TotalFailureEvents,
        [int]$TotalFailurePeriods
    )

    $sb = [System.Text.StringBuilder]::new()
    $suffix = if ($SinceFilter) { " (since $SinceFilter)" } else { "" }

    [void]$sb.AppendLine("=" * 70)
    [void]$sb.AppendLine("KNOWN ISSUE FAILURE HISTORY")
    [void]$sb.AppendLine("=" * 70)
    [void]$sb.AppendLine("")
    [void]$sb.AppendLine("Issue:      #$IssueNum - $($IssueData.Title)")
    [void]$sb.AppendLine("Repository: $Repo")
    [void]$sb.AppendLine("State:      $($IssueData.State)")
    [void]$sb.AppendLine("Created:    $($IssueData.CreatedAt)")
    [void]$sb.AppendLine("Edits:      $($IssueData.TotalEdits) total ($($Timeline.Count) with hit counts)")
    [void]$sb.AppendLine("")

    # Summary
    [void]$sb.AppendLine("-" * 70)
    [void]$sb.AppendLine("FAILURE SUMMARY$suffix")
    [void]$sb.AppendLine("-" * 70)
    [void]$sb.AppendLine("")
    [void]$sb.AppendLine("Failure events:        $($FailureEvents.Count)$suffix")
    [void]$sb.AppendLine("Failure periods:       $($FailurePeriods.Count)$suffix")
    if ($SinceFilter) {
        [void]$sb.AppendLine("Total failure events:  $TotalFailureEvents (all time)")
        [void]$sb.AppendLine("Total failure periods:  $TotalFailurePeriods (all time)")
    }

    if ($FailureEvents.Count -gt 0) {
        $lastFailure = $FailureEvents[-1]
        [void]$sb.AppendLine("Last failure:          $(Format-ShortDate $lastFailure.Date)")

        $lastDate = ConvertTo-UtcDateTime $lastFailure.Date
        $daysSince = [math]::Floor(([DateTime]::UtcNow - $lastDate).TotalDays)
        [void]$sb.AppendLine("Days since last:       $daysSince")
    }
    else {
        [void]$sb.AppendLine("Last failure:          (none detected in edit history)")
    }

    [void]$sb.AppendLine("")

    # Failure periods
    if ($FailurePeriods.Count -gt 0) {
        [void]$sb.AppendLine("-" * 70)
        [void]$sb.AppendLine("FAILURE PERIODS")
        [void]$sb.AppendLine("-" * 70)
        [void]$sb.AppendLine("")
        [void]$sb.AppendLine(("{0,-4} {1,-14} {2,-14} {3,10} {4,10}" -f "#", "Start", "End", "Peak 24h", "Peak 1mo"))
        [void]$sb.AppendLine(("{0,-4} {1,-14} {2,-14} {3,10} {4,10}" -f "---", "---", "---", "---", "---"))

        $i = 1
        foreach ($p in $FailurePeriods) {
            [void]$sb.AppendLine(("{0,-4} {1,-14} {2,-14} {3,10} {4,10}" -f $i, (Format-ShortDate $p.Start), (Format-ShortDate $p.End), $p.PeakHit24h, $p.PeakHit1mo))
            $i++
        }
        [void]$sb.AppendLine("")
    }

    # Full timeline
    [void]$sb.AppendLine("-" * 70)
    [void]$sb.AppendLine("EDIT TIMELINE")
    [void]$sb.AppendLine("-" * 70)
    [void]$sb.AppendLine("")
    [void]$sb.AppendLine(("{0,-28} {1,5} {2,5} {3,5}  {4}" -f "Date", "24h", "7d", "1mo", "Event"))
    [void]$sb.AppendLine(("{0,-28} {1,5} {2,5} {3,5}  {4}" -f "---", "---", "---", "---", "---"))

    foreach ($entry in $Timeline) {
        $marker = switch ($entry.Event) {
            'new_failure'        { "<-- NEW FAILURE" }
            'additional_failure' { "<-- ADDITIONAL FAILURE" }
            'all_clear'          { "(cleared)" }
            default              { "" }
        }
        [void]$sb.AppendLine(("{0,-28} {1,5} {2,5} {3,5}  {4}" -f $entry.Date, $entry.Hit24h, $entry.Hit7d, $entry.Hit1mo, $marker))
    }

    return $sb.ToString()
}

function Build-JsonSummary {
    param(
        [hashtable]$IssueData,
        [array]$FullTimeline,
        [array]$FailureEvents,
        [array]$FailurePeriods,
        [int]$IssueNum,
        [string]$Repo,
        [string]$SinceFilter,
        [int]$TotalFailureEvents,
        [int]$TotalFailurePeriods
    )

    $lastFailure = if ($FailureEvents.Count -gt 0) { $FailureEvents[-1].Date } else { $null }
    $daysSince = if ($lastFailure) {
        $lastDate = ConvertTo-UtcDateTime $lastFailure
        [math]::Floor(([DateTime]::UtcNow - $lastDate).TotalDays)
    } else { $null }

    # Current counts from most recent entry in full (unfiltered) timeline
    $current = if ($FullTimeline.Count -gt 0) { $FullTimeline[-1] } else { $null }

    $summary = [ordered]@{
        issueNumber            = $IssueNum
        repository             = $Repo
        title                  = $IssueData.Title
        state                  = $IssueData.State
        createdAt              = $IssueData.CreatedAt
        totalEdits             = $IssueData.TotalEdits
        sinceFilter            = $SinceFilter
        failureEvents          = $FailureEvents.Count
        failurePeriodCount     = $FailurePeriods.Count
        totalFailureEvents     = $TotalFailureEvents
        totalFailurePeriods    = $TotalFailurePeriods
        lastFailureDate        = $lastFailure
        daysSinceLastFailure   = $daysSince
        currentHitCounts       = if ($current) {
            [ordered]@{ h24 = $current.Hit24h; h7d = $current.Hit7d; h1mo = $current.Hit1mo }
        } else { $null }
        failureTimeline        = @($FailureEvents | ForEach-Object {
            [ordered]@{ date = $_.Date; hit24h = $_.Hit24h; hit7d = $_.Hit7d; hit1mo = $_.Hit1mo }
        })
        periods                = @($FailurePeriods | ForEach-Object {
            [ordered]@{ start = $_.Start; end = $_.End; peakHit24h = $_.PeakHit24h; peakHit1mo = $_.PeakHit1mo }
        })
    }

    return $summary
}

# --- ListActive mode ---
function Invoke-ListActive {
    param(
        [string]$Owner,
        [string]$RepoName,
        [int]$Max,
        [string]$SinceDate
    )

    Write-Host "Scanning open 'Known Build Error' issues in $Owner/$RepoName..."
    Write-Host ""

    $issues = gh api "repos/$Owner/$RepoName/issues?labels=Known+Build+Error&state=open&per_page=$Max&sort=updated&direction=desc" 2>&1
    if ($LASTEXITCODE -ne 0) {
        throw "Failed to list issues: $issues"
    }

    $issueList = ($issues -join "`n") | ConvertFrom-Json

    if ($issueList.Count -eq 0) {
        Write-Host "No open 'Known Build Error' issues found."
        return
    }

    Write-Host "Found $($issueList.Count) open issues. Analyzing edit history..."
    Write-Host ""

    $results = @()

    foreach ($issue in $issueList) {
        # REST issues endpoint can return PRs; skip them
        if ($issue.pull_request) { continue }

        $num = $issue.number
        $title = $issue.title
        if ($title.Length -gt 60) { $title = $title.Substring(0, 57) + "..." }

        try {
            $issueData = Get-IssueEditHistory -Owner $Owner -RepoName $RepoName -Number $num -MaxPages 5
            if (-not $issueData) { continue }

            $timeline = Parse-HitCounts -Edits $issueData.Edits
            $failureEvents = Get-FailureEvents -Timeline $timeline
            $failureEvents = Filter-Since -Items $failureEvents -SinceDate $SinceDate

            $lastFailure = if ($failureEvents.Count -gt 0) { $failureEvents[-1].Date } else { $null }
            $daysSince = if ($lastFailure) {
                $lastDate = ConvertTo-UtcDateTime $lastFailure
                [math]::Floor(([DateTime]::UtcNow - $lastDate).TotalDays)
            } else { 99999 }

            $lastFailureDisplay = if ($lastFailure) {
                $d = ConvertTo-UtcDateTime $lastFailure
                $d.ToString("yyyy-MM-dd")
            } elseif ($SinceDate) { "none since $SinceDate" } else { "never" }

            $results += [PSCustomObject]@{
                Number        = $num
                Title         = $title
                FailureCount  = $failureEvents.Count
                LastFailure   = $lastFailureDisplay
                DaysSince     = $daysSince
            }
        }
        catch {
            Write-Warning "  Skipping #$num ($($_.Exception.Message))"
        }
    }

    # Sort by most recent failure
    $results = $results | Sort-Object DaysSince

    Write-Host ("=" * 70)
    Write-Host "KNOWN BUILD ERROR ACTIVITY REPORT"
    Write-Host ("=" * 70)
    Write-Host ""
    Write-Host ("{0,-8} {1,8} {2,12} {3,-12} {4}" -f "Issue", "Failures", "Last Failure", "Days Ago", "Title")
    Write-Host ("{0,-8} {1,8} {2,12} {3,-12} {4}" -f "---", "---", "---", "---", "---")

    foreach ($r in $results) {
        $daysLabel = if ($r.DaysSince -eq 99999) { "n/a" } else { $r.DaysSince.ToString() }
        Write-Host ("{0,-8} {1,8} {2,12} {3,-12} {4}" -f "#$($r.Number)", $r.FailureCount, $r.LastFailure, $daysLabel, $r.Title)
    }
}

# =============================================================================
# Main
# =============================================================================

$repo = Split-Repository -Repo $Repository

# Verify GitHub CLI is available and authenticated
if (-not (Get-Command gh -ErrorAction SilentlyContinue)) {
    throw "GitHub CLI (gh) is not installed. See https://cli.github.com/ for installation instructions."
}
$null = gh auth status 2>&1
if ($LASTEXITCODE -ne 0) {
    throw "GitHub CLI is not authenticated. Run 'gh auth login' first."
}

if ($ListActive) {
    Invoke-ListActive -Owner $repo.Owner -RepoName $repo.Name -Max $MaxIssues -SinceDate $Since
    return
}

# Single issue mode
Write-Host "Fetching edit history for issue #$IssueNumber in $Repository..."
Write-Host ""

$issueData = Get-IssueEditHistory -Owner $repo.Owner -RepoName $repo.Name -Number $IssueNumber

if (-not $issueData) { return }

# Verify it's a Known Build Error
if ($issueData.Labels -notcontains 'Known Build Error') {
    Write-Warning "Issue #$IssueNumber does not have the 'Known Build Error' label. Results may not be meaningful."
}

$fullTimeline = Parse-HitCounts -Edits $issueData.Edits
$allFailureEvents = Get-FailureEvents -Timeline $fullTimeline
$allFailurePeriods = Get-FailurePeriods -Timeline $fullTimeline

# Apply -Since filter after analysis so periods are computed on full data
$timeline = Filter-Since -Items $fullTimeline -SinceDate $Since
$failureEvents = Filter-Since -Items $allFailureEvents -SinceDate $Since
$failurePeriods = Filter-Since -Items $allFailurePeriods -SinceDate $Since -DateProperty 'End'

if ($Json) {
    $summary = Build-JsonSummary `
        -IssueData $issueData `
        -FullTimeline $fullTimeline `
        -FailureEvents $failureEvents `
        -FailurePeriods $failurePeriods `
        -IssueNum $IssueNumber `
        -Repo $Repository `
        -SinceFilter $Since `
        -TotalFailureEvents $allFailureEvents.Count `
        -TotalFailurePeriods $allFailurePeriods.Count

    Write-Host ""
    Write-Host "[KNOWN_ISSUE_HISTORY]"
    Write-Host ($summary | ConvertTo-Json -Depth 5)
    Write-Host "[/KNOWN_ISSUE_HISTORY]"
}
else {
    $output = Format-Timeline `
        -IssueData $issueData `
        -Timeline $timeline `
        -FailureEvents $failureEvents `
        -FailurePeriods $failurePeriods `
        -IssueNum $IssueNumber `
        -Repo $Repository `
        -SinceFilter $Since `
        -TotalFailureEvents $allFailureEvents.Count `
        -TotalFailurePeriods $allFailurePeriods.Count

    Write-Host $output
}
