Set-StrictMode -Version Latest

# daily/gate.ps1 - The report gate: objective checks on a finished overall report (handles, First Responders, numbering, placeholders, novelty, placement).
# Kept apart from report.ps1; contract.ps1 loads it, so the runner's format validator always runs these checks.
. (Join-Path $PSScriptRoot '..\common\core.ps1')
. (Join-Path $PSScriptRoot 'report.ps1')

# Objective checks on a finished report, beyond what the format validator already enforces.
#
# Each of these was a real defect that reached a delivered report because only a model was checking:
#   * a copy/paste message pinging a team that does not exist (a ping delivered to nobody)
#   * "First Responders" used for a failure that is not Helix (the team pushed back on this)
#   * BH numbers that disagree between Top actions and the summary table (the same finding was
#     "BH #5" in one place and "BH #1" in another)
#   * template placeholders left in the text
#   * a row claiming "New Issue = Y" for something history has seen for weeks (or the reverse)
#   * a row in the main table for something already recorded as owned/resolved/noise
# Findings are fail (objective; blocks) or warn (advisory; a person decides, e.g. anything that
# depends on the history store being complete).


$script:VmrQBGateHeader = '| Issue category | Builds / PRs (links) | New Issue (Y/N) | Existing fix PR (status) | Action as VMR QB (who / where to ping) | Links that QB reported |'

function New-VmrQBGateFinding {
    param([string] $Severity, [string] $Check, [string] $Message, [int] $Line = 0)
    [pscustomobject]@{ severity = $Severity; check = $Check; message = $Message; line = $Line }
}

function Get-VmrQBReportRows {
    <#
    .SYNOPSIS
    Parses the six-column tables of an overall report into rows, noting which section each is in.
    #>
    param([Parameter(Mandatory)][string] $Content)
    $rows = [Collections.Generic.List[object]]::new()
    $section = 'main'
    $inTable = $false
    $number = 0
    foreach ($line in ($Content -split '\r?\n')) {
        $number++
        $trimmed = $line.Trim()
        if ($trimmed -match '^##\s+Monitor only') { $section = 'monitor' }
        elseif ($trimmed -match '^##\s+' -and $trimmed -notmatch '^##\s+Monitor only') { if (-not $inTable) { $section = if ($trimmed -match 'Monitor only') { 'monitor' } else { 'main' } } }
        if ($trimmed -eq $script:VmrQBGateHeader) { $inTable = $true; continue }
        if (-not $inTable) { continue }
        if ($trimmed -notmatch '^\|.*\|$') { $inTable = $false; continue }
        if ($trimmed -match '^\|(?:\s*:?-+:?\s*\|){6}$') { continue }
        $cells = @($trimmed.Substring(1, $trimmed.Length - 2) -split '(?<!\\)\|' | ForEach-Object { $_.Trim() })
        if ($cells.Count -ne 6) { continue }
        $category = $cells[0].Replace('**', '')
        $bh = $null
        if ($category -match '^BH #(?<n>\d+|-)\s*-\s*') { $bh = $Matches.n }
        $rows.Add([pscustomobject]@{
            line = $number; section = $section; bh = $bh
            name = [regex]::Replace($category, '^BH #(?:\d+|-)\s*-\s*', '').Trim()
            evidence = $cells[1]; newToday = $cells[2].Trim(); fix = $cells[3]; action = $cells[4]; links = $cells[5]
        })
    }
    return $rows.ToArray()
}

function Get-VmrQBTopActions {
    param([Parameter(Mandatory)][string] $Content)
    $top = [regex]::Match($Content, '(?ms)^## Top actions for the QB today[ \t]*\r?\n(?<body>.*?)(?=^## |\z)')
    if (-not $top.Success) { return @() }
    return @([regex]::Matches($top.Groups['body'].Value, '(?m)^[ \t]*(?:\d+\.|[-*])[ \t]+(?<text>[^\r\n]+)') | ForEach-Object {
        $text = $_.Groups['text'].Value.Replace('**', '')
        $bh = [regex]::Match($text, 'BH #(?<n>\d+|-)')
        [pscustomobject]@{ text = $text; bh = $(if ($bh.Success) { $bh.Groups['n'].Value } else { $null }) }
    })
}

function Test-VmrQBReportHandles {
    <#
    .SYNOPSIS
    Every @dotnet/<team> in the report must be a real team. A redirect target is a warning, anything else fails.
    #>
    param([Parameter(Mandatory)][string] $Content)
    $findings = [Collections.Generic.List[object]]::new()
    $number = 0
    foreach ($line in ($Content -split '\r?\n')) {
        $number++
        foreach ($unknown in @(Find-VmrQBUnknownHandles -Text $line)) {
            $hint = if ($unknown.suggestion) { " Use $($unknown.suggestion)." } else { ' It is not in knowledge/routing.json.' }
            $severity = if ($unknown.kind -eq 'redirect') { 'warn' } else { 'fail' }
            $what = if ($unknown.kind -eq 'redirect') { 'is a real but broad team; routing prefers a narrower owner.' } else { 'is not a team that can be pinged; the message would reach nobody.' }
            $findings.Add((New-VmrQBGateFinding $severity 'handle' "'$($unknown.handle)' $what$hint" $number))
        }
    }
    return $findings.ToArray()
}

function Test-VmrQBReportFirstResponders {
    <#
    .SYNOPSIS
    First Responders is Helix-only. A row or action routed there must be about Helix.
    #>
    param([Parameter(Mandatory)][string] $Content)
    $findings = [Collections.Generic.List[object]]::new()
    foreach ($row in @(Get-VmrQBReportRows -Content $Content)) {
        if ($row.action -notmatch 'Teams - First Responders') { continue }
        if (("$($row.name) $($row.evidence) $($row.action)") -notmatch '(?i)helix') {
            $findings.Add((New-VmrQBGateFinding 'fail' 'first-responders' "'$($row.name)' is routed to First Responders but never mentions Helix. First Responders is Helix-only; 1ES, signing, feed, agent and build failures do not belong there." $row.line))
        }
    }
    $number = 0
    foreach ($item in @(Get-VmrQBTopActions -Content $Content)) {
        $number++
        if ($item.text -match 'Where: Teams - First Responders' -and $item.text -notmatch '(?i)helix') {
            $findings.Add((New-VmrQBGateFinding 'fail' 'first-responders' "Top action $number is routed to First Responders but never mentions Helix." 0))
        }
    }
    return $findings.ToArray()
}

function Test-VmrQBReportPlaceholders {
    param([Parameter(Mandatory)][string] $Content)
    $findings = [Collections.Generic.List[object]]::new()
    $number = 0
    foreach ($line in ($Content -split '\r?\n')) {
        $number++
        $stripped = [regex]::Replace($line, '`[^`]*`', '')
        foreach ($pattern in @('\{\{[^}]*\}\}', '(?i)\bTODO\b', '(?i)\bTBD\b', '(?i)lorem ipsum', '<(?:team|owner|link|build|channel|what failed|name)[^>]*>', '(?i)\bXXX+\b')) {
            if ($stripped -match $pattern) { $findings.Add((New-VmrQBGateFinding 'fail' 'placeholder' "Unfinished placeholder text: '$($Matches[0])'." $number)); break }
        }
    }
    return $findings.ToArray()
}

function Test-VmrQBReportNumbering {
    <#
    .SYNOPSIS
    BH numbers must mean the same thing in Top actions and in the table.
    #>
    param([Parameter(Mandatory)][string] $Content, [object[]] $BuildHealthActions)
    $findings = [Collections.Generic.List[object]]::new()
    $rows = @(Get-VmrQBReportRows -Content $Content)
    $actions = @(Get-VmrQBTopActions -Content $Content)
    $stop = @('the', 'and', 'for', 'from', 'with', 'this', 'that', 'not', 'was', 'build', 'failed', 'failure', 'error', 'task', 'job')

    # One finding must not carry two numbers.
    $byName = @{}
    foreach ($row in $rows | Where-Object { $_.bh -and $_.bh -ne '-' }) {
        $key = $row.name.ToLowerInvariant()
        if (-not $byName.ContainsKey($key)) { $byName[$key] = [Collections.Generic.HashSet[string]]::new() }
        [void]$byName[$key].Add($row.bh)
    }
    foreach ($key in $byName.Keys) {
        if ($byName[$key].Count -gt 1) { $findings.Add((New-VmrQBGateFinding 'fail' 'numbering' "'$key' is numbered BH #$(($byName[$key] | Sort-Object) -join ' and BH #'). One finding, one number.")) }
    }

    # A number must describe the same finding in both places.
    foreach ($action in $actions | Where-Object { $_.bh -and $_.bh -ne '-' }) {
        $matching = @($rows | Where-Object { $_.bh -eq $action.bh })
        if (-not $matching.Count) {
            $findings.Add((New-VmrQBGateFinding 'fail' 'numbering' "Top action tagged BH #$($action.bh) has no row numbered BH #$($action.bh) in the table."))
            continue
        }
        $actionWords = Get-VmrQBReferenceWords -Text $action.text
        $overlap = $false
        foreach ($row in $matching) {
            $rowWords = Get-VmrQBReferenceWords -Text $row.name
            foreach ($word in $rowWords) { if ($actionWords.Contains($word) -and $word -notin $stop) { $overlap = $true; break } }
            if ($overlap) { break }
        }
        if (-not $overlap) {
            $findings.Add((New-VmrQBGateFinding 'fail' 'numbering' "BH #$($action.bh) names different findings: Top action says '$($action.text.Substring(0, [Math]::Min(80, $action.text.Length)))...' but the table row is '$($matching[0].name)'."))
        }
    }

    # Numbers must exist in the Build Health action list when it is supplied.
    if ($BuildHealthActions) {
        $max = @($BuildHealthActions).Count
        foreach ($number in @(($rows + $actions) | Where-Object { $_.bh -and $_.bh -ne '-' } | ForEach-Object { [int]$_.bh } | Sort-Object -Unique)) {
            if ($number -gt $max -or $number -lt 1) { $findings.Add((New-VmrQBGateFinding 'fail' 'numbering' "BH #$number does not exist: the Build Health action list has $max item(s).")) }
        }
    }
    return $findings.ToArray()
}

function Test-VmrQBReportHistory {
    <#
    .SYNOPSIS
    Advisory checks against the history store: novelty claims and monitor-only placement.
    #>
    param([Parameter(Mandatory)][string] $Content, [Parameter(Mandatory)][string] $HistoryPath, [Parameter(Mandatory)][string] $ReportDate)
    $findings = [Collections.Generic.List[object]]::new()
    if (-not (Test-Path -LiteralPath $HistoryPath)) {
        $findings.Add((New-VmrQBGateFinding 'warn' 'history' "No history store at $HistoryPath; 'New Issue' and monitor-only placement cannot be checked."))
        return $findings.ToArray()
    }
    foreach ($row in @(Get-VmrQBReportRows -Content $Content)) {
        $history = Get-VmrIssueHistory -Path $HistoryPath -Name $row.name
        $earlier = @(if ($history) { $history.occurrences | Where-Object { [string]$_.report_date -lt $ReportDate } })
        if ($row.newToday -eq 'Y' -and $earlier.Count) {
            $first = ($earlier | ForEach-Object report_date | Sort-Object | Select-Object -First 1)
            $findings.Add((New-VmrQBGateFinding 'fail' 'novelty' "'$($row.name)' says New Issue = Y but history first saw it on $first ($($earlier.Count) earlier day(s))." $row.line))
        }
        elseif ($row.newToday -eq 'N' -and -not $earlier.Count -and $row.bh) {
            $findings.Add((New-VmrQBGateFinding 'warn' 'novelty' "'$($row.name)' says New Issue = N but history has no earlier record under that name. If it was renamed, run ./history/Invoke-History.ps1 duplicates and alias it; otherwise it is new." $row.line))
        }
        if ($history) {
            $status = [string]$history.issue.status
            if ($row.section -eq 'main' -and $status -in 'owned', 'resolved', 'noise', 'wontfix') {
                $findings.Add((New-VmrQBGateFinding 'fail' 'placement' "'$($row.name)' is recorded as '$status' in history, so it belongs in the Monitor only table, not the main one." $row.line))
            }
            if ($row.section -eq 'monitor' -and $row.bh -and $row.bh -ne '-' -and $status -in 'open', 'regressed') {
                $findings.Add((New-VmrQBGateFinding 'warn' 'placement' "'$($row.name)' is in Monitor only but history says '$status'. Nothing recorded justifies not acting on it." $row.line))
            }
        }
    }
    return $findings.ToArray()
}

function Invoke-VmrQBReportGate {
    <#
    .SYNOPSIS
    Runs every objective and advisory check on an overall report. Returns findings.
    .PARAMETER ContractValidator
    Optional scriptblock taking the report content and throwing on a format violation (the runner's
    Assert-QBOverallReport). Its message becomes a 'format' finding.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][string] $Content,
        [string] $HistoryPath,
        [string] $ReportDate,
        [object[]] $BuildHealthActions,
        [scriptblock] $ContractValidator
    )
    $findings = [Collections.Generic.List[object]]::new()
    $add = { param($items) foreach ($item in @($items)) { if ($null -ne $item) { $findings.Add($item) } } }

    if ($Content -notmatch '(?m)^## Top actions for the QB today') { & $add (New-VmrQBGateFinding 'fail' 'structure' 'Missing "## Top actions for the QB today".') }
    if (-not $Content.Contains($script:VmrQBGateHeader)) { & $add (New-VmrQBGateFinding 'fail' 'structure' 'Missing the six-column summary table header.') }
    if ($Content -notmatch '(?m)^##\s+Monitor only') { & $add (New-VmrQBGateFinding 'warn' 'structure' 'No "## Monitor only" section. Findings already owned or resolved belong there.') }
    $rows = @(Get-VmrQBReportRows -Content $Content)
    if (@($rows | Where-Object { $_.bh -and $_.bh -ne '-' }).Count -and $Content -notmatch '(?m)^## Internal issue tracking and action drafts') {
        & $add (New-VmrQBGateFinding 'warn' 'structure' 'Numbered findings exist but the "Internal issue tracking and action drafts" appendix is missing.')
    }
    $title = [regex]::Match($Content, '(?m)^#\s+.*?(\d{4}-\d{2}-\d{2})')
    if ($ReportDate -and $title.Success -and $title.Groups[1].Value -ne $ReportDate) {
        & $add (New-VmrQBGateFinding 'fail' 'structure' "The title says $($title.Groups[1].Value) but this is the $ReportDate report.")
    }

    if ($ContractValidator) {
        # A validator that found several problems lists them in Data['Problems']: each one is its own finding, so each is its own
        # run note and none hides another.
        try { & $ContractValidator $Content }
        catch {
            $listed = @($_.Exception.Data['Problems'] | Where-Object { $_ })
            if ($listed.Count) { foreach ($problem in $listed) { & $add (New-VmrQBGateFinding 'fail' 'format' ([string]$problem)) } }
            else { & $add (New-VmrQBGateFinding 'fail' 'format' $_.Exception.Message) }
        }
    }
    & $add (Test-VmrQBReportHandles -Content $Content)
    & $add (Test-VmrQBReportFirstResponders -Content $Content)
    & $add (Test-VmrQBReportPlaceholders -Content $Content)
    & $add (Test-VmrQBReportNumbering -Content $Content -BuildHealthActions $BuildHealthActions)
    if ($HistoryPath -and $ReportDate) { & $add (Test-VmrQBReportHistory -Content $Content -HistoryPath $HistoryPath -ReportDate $ReportDate) }
    return $findings.ToArray()
}
