[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot '..\history\store.ps1')

$failures = 0
$checks = 0
function Check([string] $Name, [scriptblock] $Body) {
    $script:checks++
    try { & $Body; Write-Host "PASS: $Name" -ForegroundColor Green }
    catch { $script:failures++; Write-Host "FAIL: $Name -> $($_.Exception.Message)" -ForegroundColor Red }
}

# Every store this script creates lives in one temporary folder, removed when the script ends.
$testRoot = Join-Path ([IO.Path]::GetTempPath()) ('vmrqb-store-' + [guid]::NewGuid().ToString('N'))
[void][IO.Directory]::CreateDirectory($testRoot)

function New-TestStorePath {
    return (Join-Path $testRoot ('store-' + [guid]::NewGuid().ToString('N') + '.json'))
}
function New-TestDatabase {
    $path = New-TestStorePath
    Initialize-VmrIssueStore -Path $path | Out-Null
    return $path
}
function Write-TestJson([string] $Path, $Document) {
    ConvertTo-Json -InputObject $Document -Depth 10 | Set-Content -LiteralPath $Path -Encoding utf8
}
function Write-TestExport([string] $Path, [int] $Schema = 2, $Issues = @(), $Aliases = @(), $Occurrences = @(), $History = @()) {
    Write-TestJson $Path ([ordered]@{
        schemaVersion = $Schema; exportedAtUtc = '2026-10-07T00:00:00.0000000Z'
        issues = @($Issues); aliases = @($Aliases); occurrences = @($Occurrences); statusHistory = @($History)
    })
}
# A store or export with its timestamps removed, for byte-for-byte comparisons of everything else.
function Get-WithoutTimestamps([string] $Path) {
    return ([IO.File]::ReadAllText($Path) -replace '(?m)^\s*"(updatedAtUtc|exportedAtUtc)":[^\r\n]*\r?\n', '')
}
function Get-Pair([string] $Path) {
    return [pscustomobject]@{ Bytes = [IO.File]::ReadAllBytes($Path); Stamp = ([IO.FileInfo]::new($Path)).LastWriteTimeUtc.Ticks }
}
function Assert-Unchanged([string] $Path, $Before, [string] $What) {
    $now = Get-Pair $Path
    if ($now.Stamp -ne $Before.Stamp) { throw "$What rewrote the store (timestamp changed)" }
    if ([Convert]::ToBase64String($now.Bytes) -cne [Convert]::ToBase64String($Before.Bytes)) { throw "$What changed the store bytes" }
}

try {

Check 'Slug normalization strips BH numbers, backticks and punctuation' {
    $cases = @{
        'BH #7 - Source-index Azure subscription missing' = 'source-index-azure-subscription-missing'
        'SDK `NuGet.Versioning` central-version downgrade' = 'sdk-nuget-versioning-central-version-downgrade'
        'BH #- - iOS Simulator 150-minute build timeout'  = 'ios-simulator-150-minute-build-timeout'
    }
    foreach ($input in $cases.Keys) {
        $actual = ConvertTo-VmrIssueSlug -Name $input
        if ($actual -ne $cases[$input]) { throw "'$input' -> '$actual', expected '$($cases[$input])'" }
    }
}

Check 'Repeat occurrences accumulate rather than duplicating the issue' {
    $db = New-TestDatabase
    Add-VmrIssueOccurrence -Path $db -Name 'Widget build break' -ReportDate '2026-10-01' -BuildIds @('1') | Out-Null
    Add-VmrIssueOccurrence -Path $db -Name 'Widget build break' -ReportDate '2026-10-02' -BuildIds @('2', '3') | Out-Null
    $history = Get-VmrIssueHistory -Path $db -Name 'Widget build break'
    if ($history.issue.occurrences -ne 2) { throw "occurrences=$($history.issue.occurrences), expected 2" }
    if ($history.issue.first_seen -ne '2026-10-01') { throw "first_seen=$($history.issue.first_seen)" }
    if ($history.issue.last_seen -ne '2026-10-02') { throw "last_seen=$($history.issue.last_seen)" }
    # Re-importing the same day must update, never double-count.
    Add-VmrIssueOccurrence -Path $db -Name 'Widget build break' -ReportDate '2026-10-02' -BuildIds @('2', '3', '4') | Out-Null
    $again = Get-VmrIssueHistory -Path $db -Name 'Widget build break'
    if ($again.issue.occurrences -ne 2) { throw "re-import duplicated to $($again.issue.occurrences)" }
    if ($again.occurrences[0].build_count -ne 3) { throw "build_count=$($again.occurrences[0].build_count), expected 3" }
}

Check 'A renamed finding keeps one timeline once aliased' {
    $db = New-TestDatabase
    Add-VmrIssueOccurrence -Path $db -Name 'Azure source-index subscription missing' -ReportDate '2026-10-01' -BuildIds @('9100001') | Out-Null
    Add-VmrIssueOccurrence -Path $db -Name 'Source-index Azure subscription missing' -ReportDate '2026-10-02' -BuildIds @('9100002') -TrackerUrl 'https://github.com/dotnet/dotnet/issues/101' | Out-Null
    $split = Get-VmrIssueHistory -Path $db -Limit 50
    if (@($split).Count -ne 2) { throw "expected the rename to split into 2 issues first, got $(@($split).Count)" }

    Register-VmrIssueAlias -Path $db -Alias 'Source-index Azure subscription missing' -CanonicalName 'Azure source-index subscription missing' | Out-Null
    $merged = Get-VmrIssueHistory -Path $db -Limit 50
    if (@($merged).Count -ne 1) { throw "alias did not fold the history; $(@($merged).Count) issues remain" }

    # Either wording must now resolve to the same, complete timeline.
    foreach ($name in @('Azure source-index subscription missing', 'Source-index Azure subscription missing')) {
        $history = Get-VmrIssueHistory -Path $db -Name $name
        if ($history.issue.occurrences -ne 2) { throw "'$name' occurrences=$($history.issue.occurrences), expected 2" }
        if ($history.issue.first_seen -ne '2026-10-01') { throw "'$name' lost the earlier occurrence" }
        if ($history.issue.tracker_url -ne 'https://github.com/dotnet/dotnet/issues/101') {
            throw "'$name' lost the tracker during the merge: $($history.issue.tracker_url)"
        }
    }
}

Check 'Recurrence after resolution is recorded as a regression' {
    $db = New-TestDatabase
    Add-VmrIssueOccurrence -Path $db -Name 'Flaky installer test' -ReportDate '2026-10-01' -BuildIds @('1') | Out-Null
    Set-VmrIssueStatus -Path $db -Name 'Flaky installer test' -Status resolved -Resolution 'Fixed by PR 1' -On '2026-10-02' | Out-Null
    $resolved = Get-VmrIssueHistory -Path $db -Name 'Flaky installer test'
    if ($resolved.issue.status -ne 'resolved') { throw "status=$($resolved.issue.status)" }
    if ($resolved.issue.resolved_on -ne '2026-10-02') { throw "resolved_on=$($resolved.issue.resolved_on)" }

    Add-VmrIssueOccurrence -Path $db -Name 'Flaky installer test' -ReportDate '2026-10-05' -BuildIds @('2') | Out-Null
    $regressed = Get-VmrIssueHistory -Path $db -Name 'Flaky installer test'
    if ($regressed.issue.status -ne 'regressed') { throw "a resolved issue that recurred shows '$($regressed.issue.status)'" }
    $trail = @($regressed.statusTrail | ForEach-Object { $_.status })
    if ($trail -notcontains 'regressed') { throw "status trail missing the regression: $($trail -join ',')" }
    if ($trail -notcontains 'resolved') { throw "status trail lost the resolution: $($trail -join ',')" }
}

Check 'Status filtering and prompt context render the recorded facts' {
    $db = New-TestDatabase
    $today = (Get-Date).ToString('yyyy-MM-dd')
    Add-VmrIssueOccurrence -Path $db -Name 'Noisy analyzer warning' -ReportDate $today -BuildIds @('1') | Out-Null
    Set-VmrIssueStatus -Path $db -Name 'Noisy analyzer warning' -Status noise -OwnerTeam '@dotnet/roslyn-infrastructure' -TrackerUrl 'https://github.com/dotnet/dotnet/issues/102' -On $today | Out-Null
    Add-VmrIssueOccurrence -Path $db -Name 'Real build break' -ReportDate $today -BuildIds @('2') | Out-Null

    $noise = Get-VmrIssueHistory -Path $db -Status 'noise' -Limit 50
    if (@($noise).Count -ne 1) { throw "status filter returned $(@($noise).Count) rows" }

    $context = Format-VmrIssueHistoryContext -Path $db -SinceDays 30
    if ($context -notmatch 'Noisy analyzer warning \[noise\]') { throw "context lost the status: $context" }
    if ($context -notmatch 'issues/102') { throw "context lost the tracker: $context" }
    if ($context -notmatch 'Real build break .*no tracker') { throw "context should say no tracker: $context" }
}

Check 'Unknown issues and missing databases degrade quietly' {
    $db = New-TestDatabase
    if ($null -ne (Get-VmrIssueHistory -Path $db -Name 'Never seen before')) { throw 'unknown issue should return null' }
    $missing = Join-Path ([IO.Path]::GetTempPath()) ('absent-' + [guid]::NewGuid().ToString('N') + '.json')
    if ($null -ne (Format-VmrIssueHistoryContext -Path $missing)) { throw 'missing database should return null, not throw' }
    if (@(Get-VmrIssueHistory -Path $missing).Count -ne 0) { throw 'a missing store should list nothing' }
    if ($null -ne (Get-VmrIssueHistory -Path $missing -Name 'x')) { throw 'a missing store has no named issue' }
    if ((Get-VmrKnownTrackerMap -Path $missing).Count -ne 0) { throw 'a missing store has no known trackers' }
    if ((Get-VmrIssueStoreSchemaVersion -Path $missing) -ne 0) { throw 'a missing store reports schema 0' }
    if (Test-Path -LiteralPath $missing) { throw 'reading a missing store must not create it' }
}

# --- Provenance: a human decision outranks an LLM-written report ------------------------------

Check 'An operator-recorded owner and tracker survive a later daily-run occurrence' {
    $db = New-TestDatabase
    Add-VmrIssueOccurrence -Path $db -Name 'Sticky finding' -ReportDate '2026-10-01' -OwnerTeam '@dotnet/dnceng' -TrackerUrl 'https://github.com/dotnet/dotnet/issues/1' | Out-Null
    $first = (Get-VmrIssueHistory -Path $db -Name 'Sticky finding').issue
    if ($first.owner_source -ne 'daily-run' -or $first.tracker_source -ne 'daily-run') { throw "a report-written value must be tagged daily-run: $($first.owner_source)/$($first.tracker_source)" }

    Set-VmrIssueStatus -Path $db -Name 'Sticky finding' -Status owned -OwnerTeam '@dotnet/runtime' -TrackerUrl 'https://github.com/dotnet/dotnet/issues/2' -On '2026-10-02' | Out-Null
    $decided = (Get-VmrIssueHistory -Path $db -Name 'Sticky finding').issue
    if ($decided.owner_team -ne '@dotnet/runtime' -or $decided.owner_source -ne 'operator') { throw "the operator owner was not recorded: $($decided.owner_team)/$($decided.owner_source)" }
    if ($decided.tracker_url -notmatch 'issues/2$' -or $decided.tracker_source -ne 'operator') { throw "the operator tracker was not recorded: $($decided.tracker_url)/$($decided.tracker_source)" }

    # The model-written report goes on naming whatever it thinks; none of it may win.
    Add-VmrIssueOccurrence -Path $db -Name 'Sticky finding' -ReportDate '2026-10-03' -OwnerTeam '@dotnet/dnceng' -TrackerUrl 'https://github.com/dotnet/dotnet/issues/1' | Out-Null
    Add-VmrIssueOccurrence -Path $db -Name 'Sticky finding' -ReportDate '2026-10-04' -OwnerTeam '@dotnet/aspnet-build' | Out-Null
    $after = (Get-VmrIssueHistory -Path $db -Name 'Sticky finding').issue
    if ($after.owner_team -ne '@dotnet/runtime') { throw "a daily run overwrote the operator owner: $($after.owner_team)" }
    if ($after.tracker_url -notmatch 'issues/2$') { throw "a daily run overwrote the operator tracker: $($after.tracker_url)" }
    if ($after.occurrences -ne 3) { throw "the occurrences themselves must still be recorded: $($after.occurrences)" }

    # A later operator decision is a decision too, and replaces the earlier one.
    Set-VmrIssueStatus -Path $db -Name 'Sticky finding' -Status owned -OwnerTeam '@dotnet/sdk' -On '2026-10-05' | Out-Null
    if ((Get-VmrIssueHistory -Path $db -Name 'Sticky finding').issue.owner_team -ne '@dotnet/sdk') { throw 'a newer operator owner must replace an older one' }
}

Check 'A daily-run owner is replaced by a later daily-run value and filled when blank; blanks never clear' {
    $db = New-TestDatabase
    Add-VmrIssueOccurrence -Path $db -Name 'Drifting owner' -ReportDate '2026-10-01' -OwnerTeam '@dotnet/dnceng' -TrackerUrl 'https://github.com/dotnet/dotnet/issues/1' | Out-Null
    Add-VmrIssueOccurrence -Path $db -Name 'Drifting owner' -ReportDate '2026-10-02' -OwnerTeam '@dotnet/runtime' -TrackerUrl 'https://github.com/dotnet/dotnet/issues/2' | Out-Null
    $issue = (Get-VmrIssueHistory -Path $db -Name 'Drifting owner').issue
    if ($issue.owner_team -ne '@dotnet/runtime') { throw "a later daily-run owner should replace an earlier one: $($issue.owner_team)" }
    if ($issue.tracker_url -notmatch 'issues/2$') { throw "a later daily-run tracker should replace an earlier one: $($issue.tracker_url)" }
    if ($issue.owner_source -ne 'daily-run') { throw "provenance should stay daily-run: $($issue.owner_source)" }

    # A day whose report names nothing must not erase what is known.
    Add-VmrIssueOccurrence -Path $db -Name 'Drifting owner' -ReportDate '2026-10-03' | Out-Null
    $kept = (Get-VmrIssueHistory -Path $db -Name 'Drifting owner').issue
    if ($kept.owner_team -ne '@dotnet/runtime' -or $kept.tracker_url -notmatch 'issues/2$') { throw 'a report that names no owner cleared the recorded one' }

    # A blank value is filled by a report, and tagged as such.
    Set-VmrIssueStatus -Path $db -Name 'Unowned so far' -Status open -On '2026-10-04' | Out-Null
    Add-VmrIssueOccurrence -Path $db -Name 'Unowned so far' -ReportDate '2026-10-05' -OwnerTeam '@dotnet/msbuild' | Out-Null
    $filled = (Get-VmrIssueHistory -Path $db -Name 'Unowned so far').issue
    if ($filled.owner_team -ne '@dotnet/msbuild' -or $filled.owner_source -ne 'daily-run') { throw "a blank owner should be filled by the report: $($filled.owner_team)/$($filled.owner_source)" }
}

Check 'Blank or omitted status parameters never clear recorded values' {
    $db = New-TestDatabase
    Set-VmrIssueStatus -Path $db -Name 'Keep my facts' -Status owned -OwnerTeam '@dotnet/dnceng' -TrackerUrl 'https://github.com/dotnet/dotnet/issues/5' -Note 'first' -On '2026-10-01' | Out-Null
    Set-VmrIssueStatus -Path $db -Name 'Keep my facts' -Status routed -OwnerTeam '' -TrackerUrl '   ' -Note 'second' -On '2026-10-02' | Out-Null
    Set-VmrIssueStatus -Path $db -Name 'Keep my facts' -Status owned -On '2026-10-03' | Out-Null
    $issue = (Get-VmrIssueHistory -Path $db -Name 'Keep my facts').issue
    if ($issue.owner_team -ne '@dotnet/dnceng' -or $issue.owner_source -ne 'operator') { throw "owner was cleared: $($issue.owner_team)/$($issue.owner_source)" }
    if ($issue.tracker_url -notmatch 'issues/5$' -or $issue.tracker_source -ne 'operator') { throw "tracker was cleared: $($issue.tracker_url)" }
    $trail = (Get-VmrIssueHistory -Path $db -Name 'Keep my facts').statusTrail
    if (@($trail).Count -ne 3) { throw "every status change belongs in the trail, got $(@($trail).Count)" }
    if ($trail[0].source -ne 'operator') { throw "the default source of a status change is operator: $($trail[0].source)" }
}

Check 'Out-of-order replay keeps first_seen as the earliest and last_seen as the latest date' {
    $db = New-TestDatabase
    foreach ($day in @('2026-10-05', '2026-10-01', '2026-10-03', '2026-10-09', '2026-10-02')) {
        Add-VmrIssueOccurrence -Path $db -Name 'Late delivered report' -ReportDate $day -BuildIds @($day) | Out-Null
    }
    $issue = (Get-VmrIssueHistory -Path $db -Name 'Late delivered report').issue
    if ($issue.first_seen -ne '2026-10-01') { throw "first_seen=$($issue.first_seen), expected 2026-10-01" }
    if ($issue.last_seen -ne '2026-10-09') { throw "last_seen=$($issue.last_seen), expected 2026-10-09" }
    if ($issue.occurrences -ne 5) { throw "occurrences=$($issue.occurrences), expected 5" }
    $dates = @((Get-VmrIssueHistory -Path $db -Name 'Late delivered report').occurrences | ForEach-Object { $_.report_date })
    if (($dates -join ',') -ne '2026-10-09,2026-10-05,2026-10-03,2026-10-02,2026-10-01') { throw "occurrences are not newest first: $($dates -join ',')" }
    # A set-status call that created the issue on a later date must not pin first_seen there either.
    Set-VmrIssueStatus -Path $db -Name 'Created by status' -Status open -On '2026-10-08' | Out-Null
    Add-VmrIssueOccurrence -Path $db -Name 'Created by status' -ReportDate '2026-10-02' | Out-Null
    $created = (Get-VmrIssueHistory -Path $db -Name 'Created by status').issue
    if ($created.first_seen -ne '2026-10-02' -or $created.last_seen -ne '2026-10-08') { throw "range=$($created.first_seen)..$($created.last_seen)" }
}

Check 'Replaying days that are already recorded changes nothing, including after a resolution' {
    $db = New-TestDatabase
    Add-VmrIssueOccurrence -Path $db -Name 'Replayed finding' -ReportDate '2026-10-01' -BuildIds @('1') -OwnerTeam '@dotnet/dnceng' -ReportPath 'overall-summary-2026-10-01.md' | Out-Null
    Add-VmrIssueOccurrence -Path $db -Name 'Replayed finding' -ReportDate '2026-10-02' -BuildIds @('2') | Out-Null
    $before = Get-Pair $db
    Start-Sleep -Milliseconds 30
    Add-VmrIssueOccurrence -Path $db -Name 'Replayed finding' -ReportDate '2026-10-01' -BuildIds @('1') -OwnerTeam '@dotnet/dnceng' -ReportPath 'overall-summary-2026-10-01.md' | Out-Null
    Add-VmrIssueOccurrence -Path $db -Name 'Replayed finding' -ReportDate '2026-10-02' -BuildIds @('2') | Out-Null
    Assert-Unchanged $db $before 'replaying identical occurrences'

    # The daily replay re-reads every report, old ones included. That must not make a resolved issue regress.
    Set-VmrIssueStatus -Path $db -Name 'Replayed finding' -Status resolved -Resolution 'Fixed by PR 2' -On '2026-10-03' | Out-Null
    $resolved = Get-Pair $db
    Add-VmrIssueOccurrence -Path $db -Name 'Replayed finding' -ReportDate '2026-10-01' -BuildIds @('1') -OwnerTeam '@dotnet/dnceng' -ReportPath 'overall-summary-2026-10-01.md' | Out-Null
    Add-VmrIssueOccurrence -Path $db -Name 'Replayed finding' -ReportDate '2026-10-02' -BuildIds @('2') | Out-Null
    Assert-Unchanged $db $resolved 'replaying old days of a resolved issue'
    $history = Get-VmrIssueHistory -Path $db -Name 'Replayed finding'
    if ($history.issue.status -ne 'resolved') { throw "a replayed old day regressed the issue to '$($history.issue.status)'" }

    # A genuinely new day does regress it, and the entry carries the source that observed it.
    Add-VmrIssueOccurrence -Path $db -Name 'Replayed finding' -ReportDate '2026-10-06' -Source 'scheduled-run' | Out-Null
    $regressed = Get-VmrIssueHistory -Path $db -Name 'Replayed finding'
    if ($regressed.issue.status -ne 'regressed') { throw "a new occurrence after resolution shows '$($regressed.issue.status)'" }
    if ($null -ne $regressed.issue.resolution -or $null -ne $regressed.issue.resolved_on) { throw 'regression should clear the resolution' }
    $entry = @($regressed.statusTrail | Where-Object { $_.status -eq 'regressed' })
    if ($entry.Count -ne 1 -or $entry[0].note -ne 'Recurred after being marked resolved.' -or $entry[0].source -ne 'scheduled-run') {
        throw "regression entry is wrong: $($entry | ConvertTo-Json -Compress)"
    }
    Add-VmrIssueOccurrence -Path $db -Name 'Brand new via backfill' -ReportDate '2026-10-06' -Source 'backfill' | Out-Null
    $new = (Get-VmrIssueHistory -Path $db -Name 'Brand new via backfill').statusTrail
    if (@($new).Count -ne 1 -or $new[0].note -ne 'First observed.' -or $new[0].source -ne 'backfill' -or $new[0].status -ne 'open') { throw "first-observed entry is wrong: $($new | ConvertTo-Json -Compress)" }
}

# --- Owner canonicalisation through Resolve-VmrQBOwnerTeam (supplied by another module) -----------

Check 'A report naming an unknown team has the owner dropped with a warning' {
    $db = New-TestDatabase
    $calls = [Collections.Generic.List[string]]::new()
    function Resolve-VmrQBOwnerTeam {
        param([string] $Handle)
        $calls.Add($Handle)
        if ($Handle -eq '@dotnet/dnceng') { return '@dotnet/dnceng' }
        return $null
    }
    try {
        $out = Add-VmrIssueOccurrence -Path $db -Name 'Ghost owner' -ReportDate '2026-10-01' -OwnerTeam '@dotnet/no-such-team' -TrackerUrl 'https://github.com/dotnet/dotnet/issues/9' 3>&1
        $warnings = @($out | Where-Object { $_ -is [System.Management.Automation.WarningRecord] })
        if ($warnings.Count -ne 1) { throw "expected exactly one warning, got $($warnings.Count)" }
        if ($warnings[0].Message -notmatch 'no-such-team') { throw "the warning should name the handle: $($warnings[0].Message)" }
        $issue = (Get-VmrIssueHistory -Path $db -Name 'Ghost owner').issue
        if ($issue.owner_team) { throw "an unknown handle was stored: $($issue.owner_team)" }
        if ($issue.tracker_url -notmatch 'issues/9$') { throw 'dropping the owner must not drop the rest of the occurrence' }
        if ($issue.occurrences -ne 1) { throw 'the occurrence itself must still be recorded' }

        # Once the bad value was dropped, a valid one can still fill the blank on a later day.
        Add-VmrIssueOccurrence -Path $db -Name 'Ghost owner' -ReportDate '2026-10-02' -OwnerTeam '@dotnet/dnceng' | Out-Null
        if ((Get-VmrIssueHistory -Path $db -Name 'Ghost owner').issue.owner_team -ne '@dotnet/dnceng') { throw 'a valid owner should have filled the blank' }
    }
    finally { Remove-Item -LiteralPath function:\Resolve-VmrQBOwnerTeam -ErrorAction SilentlyContinue }
    if (Test-Path -LiteralPath function:\Resolve-VmrQBOwnerTeam) { throw 'the stub resolver leaked out of the check' }
}

Check 'An operator status rejects an unknown team unless -AllowUnknownOwner is given' {
    $db = New-TestDatabase
    function Resolve-VmrQBOwnerTeam {
        param([string] $Handle)
        if ($Handle -eq '@dotnet/dnceng') { return '@dotnet/dnceng' }
        return $null
    }
    try {
        $caught = $null
        try { Set-VmrIssueStatus -Path $db -Name 'Typo owner' -Status owned -OwnerTeam '@dotnet/dnsceng' -On '2026-10-01' | Out-Null } catch { $caught = $_.Exception.Message }
        if (-not $caught) { throw 'an unknown team handle must be refused' }
        if ($caught -notmatch 'dnsceng' -or $caught -notmatch 'AllowUnknownOwner') { throw "the error should name the handle and the escape hatch: $caught" }
        if ($null -ne (Get-VmrIssueHistory -Path $db -Name 'Typo owner')) { throw 'a refused status change must not leave a half-written issue behind' }

        Set-VmrIssueStatus -Path $db -Name 'Typo owner' -Status owned -OwnerTeam '@dotnet/legacy-team' -AllowUnknownOwner -On '2026-10-02' | Out-Null
        $issue = (Get-VmrIssueHistory -Path $db -Name 'Typo owner').issue
        if ($issue.owner_team -ne '@dotnet/legacy-team' -or $issue.owner_source -ne 'operator') { throw "-AllowUnknownOwner should store it as given: $($issue.owner_team)/$($issue.owner_source)" }

        Set-VmrIssueStatus -Path $db -Name 'Typo owner' -Status routed -OwnerTeam '@dotnet/dnceng' -On '2026-10-03' | Out-Null
        if ((Get-VmrIssueHistory -Path $db -Name 'Typo owner').issue.owner_team -ne '@dotnet/dnceng') { throw 'a known team must be accepted without the switch' }
    }
    finally { Remove-Item -LiteralPath function:\Resolve-VmrQBOwnerTeam -ErrorAction SilentlyContinue }
    if (Test-Path -LiteralPath function:\Resolve-VmrQBOwnerTeam) { throw 'the stub resolver leaked out of the check' }
}

Check 'Person handles skip the resolver and team handles are stored in canonical form' {
    $db = New-TestDatabase
    $calls = [Collections.Generic.List[string]]::new()
    function Resolve-VmrQBOwnerTeam {
        param([string] $Handle)
        $calls.Add($Handle)
        if ($Handle -ieq '@dotnet/dnceng') { return '@dotnet/dnceng' }
        return $null
    }
    try {
        Set-VmrIssueStatus -Path $db -Name 'Person owned' -Status owned -OwnerTeam '@ViktorHofer' -On '2026-10-01' | Out-Null
        if ((Get-VmrIssueHistory -Path $db -Name 'Person owned').issue.owner_team -ne '@ViktorHofer') { throw 'a person handle should be stored unchanged' }
        Add-VmrIssueOccurrence -Path $db -Name 'Person reported' -ReportDate '2026-10-01' -OwnerTeam '@some-person-1' | Out-Null
        if ((Get-VmrIssueHistory -Path $db -Name 'Person reported').issue.owner_team -ne '@some-person-1') { throw 'a person handle in a report should be stored unchanged' }
        if ($calls.Count -ne 0) { throw "the resolver must not see person handles, saw: $($calls -join ', ')" }

        Set-VmrIssueStatus -Path $db -Name 'Team owned' -Status owned -OwnerTeam '@dotnet/DNCENG' -On '2026-10-01' | Out-Null
        if ((Get-VmrIssueHistory -Path $db -Name 'Team owned').issue.owner_team -ne '@dotnet/dnceng') { throw 'the canonical form should be stored, not what was typed' }
        if ($calls.Count -ne 1 -or $calls[0] -ne '@dotnet/DNCENG') { throw "the resolver should be asked once with the typed handle: $($calls -join ', ')" }
    }
    finally { Remove-Item -LiteralPath function:\Resolve-VmrQBOwnerTeam -ErrorAction SilentlyContinue }
    if (Test-Path -LiteralPath function:\Resolve-VmrQBOwnerTeam) { throw 'the stub resolver leaked out of the check' }
}

Check 'Without a resolver module the owner is stored exactly as given' {
    if (Test-Path -LiteralPath function:\Resolve-VmrQBOwnerTeam) { throw 'precondition: no resolver is loaded' }
    $db = New-TestDatabase
    Add-VmrIssueOccurrence -Path $db -Name 'No resolver' -ReportDate '2026-10-01' -OwnerTeam '@dotnet/anything-at-all' | Out-Null
    if ((Get-VmrIssueHistory -Path $db -Name 'No resolver').issue.owner_team -ne '@dotnet/anything-at-all') { throw 'the value should be kept as given' }
    Set-VmrIssueStatus -Path $db -Name 'No resolver two' -Status owned -OwnerTeam '@dotnet/also-anything' -On '2026-10-01' | Out-Null
    if ((Get-VmrIssueHistory -Path $db -Name 'No resolver two').issue.owner_team -ne '@dotnet/also-anything') { throw 'the operator value should be kept as given' }
}

Check 'The owner resolver is never called while the store lock is held, and is skipped when it cannot matter' {
    $db = New-TestDatabase
    $observations = [Collections.Generic.List[string]]::new()
    function Resolve-VmrQBOwnerTeam {
        param([string] $Handle)
        # A slow lookup (a network call, say) must not be able to hold up other writers.
        $free = $true
        try { [IO.File]::Open("$db.lock", [IO.FileMode]::OpenOrCreate, [IO.FileAccess]::ReadWrite, [IO.FileShare]::None).Dispose() } catch { $free = $false }
        $observations.Add("$Handle lockFree=$free")
        return $Handle
    }
    try {
        Add-VmrIssueOccurrence -Path $db -Name 'Resolver timing' -ReportDate '2026-10-01' -OwnerTeam '@dotnet/dnceng' | Out-Null
        Set-VmrIssueStatus -Path $db -Name 'Resolver timing two' -Status owned -OwnerTeam '@dotnet/runtime' -On '2026-10-01' | Out-Null
        if ($observations.Count -ne 2) { throw "expected 2 resolver calls, saw: $($observations -join '; ')" }
        foreach ($seen in $observations) { if ($seen -notmatch 'lockFree=True') { throw "the resolver ran under the store lock: $seen" } }

        # Nothing to decide: the same daily-run value is already recorded, and an operator owns the other.
        $observations.Clear()
        Add-VmrIssueOccurrence -Path $db -Name 'Resolver timing' -ReportDate '2026-10-02' -OwnerTeam '@dotnet/dnceng' | Out-Null
        Add-VmrIssueOccurrence -Path $db -Name 'Resolver timing two' -ReportDate '2026-10-02' -OwnerTeam '@dotnet/aspnet-build' | Out-Null
        if ($observations.Count -ne 0) { throw "the resolver was asked needlessly: $($observations -join '; ')" }
        # A different value does need checking.
        Add-VmrIssueOccurrence -Path $db -Name 'Resolver timing' -ReportDate '2026-10-03' -OwnerTeam '@dotnet/sdk' | Out-Null
        if ($observations.Count -ne 1) { throw "a changed owner should be checked once: $($observations -join '; ')" }
        if ((Get-VmrIssueHistory -Path $db -Name 'Resolver timing').issue.owner_team -ne '@dotnet/sdk') { throw 'the checked owner should have been recorded' }
    }
    finally { Remove-Item -LiteralPath function:\Resolve-VmrQBOwnerTeam -ErrorAction SilentlyContinue }
    if (Test-Path -LiteralPath function:\Resolve-VmrQBOwnerTeam) { throw 'the stub resolver leaked out of the check' }
}

# --- Import: merge, aliases, status, provenance, idempotency -------------------------------------

Check 'Importing an alias folds a local issue that carries the alias name into its canonical issue' {
    $local = New-TestDatabase
    Add-VmrIssueOccurrence -Path $local -Name 'Alpha wording of the break' -ReportDate '2026-10-01' -BuildIds @('1') | Out-Null
    Add-VmrIssueOccurrence -Path $local -Name 'Alpha wording of the break' -ReportDate '2026-10-02' -BuildIds @('2') -OwnerTeam '@dotnet/dnceng' | Out-Null

    $theirs = New-TestDatabase
    Add-VmrIssueOccurrence -Path $theirs -Name 'Beta wording of the break' -ReportDate '2026-10-03' -BuildIds @('3') | Out-Null
    Register-VmrIssueAlias -Path $theirs -Alias 'Alpha wording of the break' -CanonicalName 'Beta wording of the break' | Out-Null
    $export = Join-Path $testRoot ('fold-' + [guid]::NewGuid().ToString('N') + '.json')
    Export-VmrIssueHistory -Path $theirs -OutputPath $export | Out-Null

    Import-VmrIssueHistory -Path $local -InputPath $export | Out-Null
    $issues = @(Get-VmrIssueHistory -Path $local -Limit 50)
    if ($issues.Count -ne 1) { throw "the alias-named issue was left behind: $($issues.Count) issues, $((@($issues | ForEach-Object { $_.slug })) -join ', ')" }
    if ($issues[0].slug -ne 'beta-wording-of-the-break') { throw "the canonical issue should remain: $($issues[0].slug)" }
    foreach ($wording in @('Alpha wording of the break', 'Beta wording of the break')) {
        $history = Get-VmrIssueHistory -Path $local -Name $wording
        if ($null -eq $history) { throw "'$wording' is unreachable after the import" }
        if ($history.issue.occurrences -ne 3) { throw "'$wording' occurrences=$($history.issue.occurrences), expected 3" }
        $days = @($history.occurrences | ForEach-Object { $_.report_date })
        if (($days -join ',') -ne '2026-10-03,2026-10-02,2026-10-01') { throw "'$wording' lost occurrences: $($days -join ',')" }
        if ($history.issue.first_seen -ne '2026-10-01' -or $history.issue.last_seen -ne '2026-10-03') { throw "range=$($history.issue.first_seen)..$($history.issue.last_seen)" }
    }
    $store = Read-VmrIssueStore -Path $local
    $stranded = @($store.occurrences | Where-Object { $_.slug -ne 'beta-wording-of-the-break' })
    if ($stranded.Count -ne 0) { throw "$($stranded.Count) occurrence rows are still filed under the alias slug" }
    if ((Get-VmrIssueHistory -Path $local -Name 'Beta wording of the break').issue.owner_team -ne '@dotnet/dnceng') { throw 'the folded issue lost its owner' }
}

Check 'A colleague newer status arrives on import, and a newer local status is kept' {
    # Theirs moved on to owned; mine only saw it open.
    $mine = New-TestDatabase
    Add-VmrIssueOccurrence -Path $mine -Name 'Moving finding' -ReportDate '2026-10-02' | Out-Null
    $theirs = New-TestDatabase
    Add-VmrIssueOccurrence -Path $theirs -Name 'Moving finding' -ReportDate '2026-10-01' | Out-Null
    Set-VmrIssueStatus -Path $theirs -Name 'Moving finding' -Status owned -OwnerTeam '@dotnet/dnceng' -Note 'Taken by dnceng.' -On '2026-10-06' | Out-Null
    $export = Join-Path $testRoot ('status-' + [guid]::NewGuid().ToString('N') + '.json')
    Export-VmrIssueHistory -Path $theirs -OutputPath $export | Out-Null

    Import-VmrIssueHistory -Path $mine -InputPath $export | Out-Null
    $arrived = Get-VmrIssueHistory -Path $mine -Name 'Moving finding'
    if ($arrived.issue.status -ne 'owned') { throw "the colleague's newer status did not arrive: $($arrived.issue.status)" }
    $statuses = @($arrived.statusTrail | ForEach-Object { "$($_.changed_on):$($_.status)" })
    if (($statuses -join ',') -ne '2026-10-01:open,2026-10-02:open,2026-10-06:owned') { throw "the audit trail must hold both sides: $($statuses -join ',')" }
    if ($arrived.issue.owner_team -ne '@dotnet/dnceng') { throw 'the colleague owner should arrive with the status' }

    # Now the other way round: I moved on later than the export says.
    $other = New-TestDatabase
    Add-VmrIssueOccurrence -Path $other -Name 'Moving finding' -ReportDate '2026-10-01' | Out-Null
    Set-VmrIssueStatus -Path $other -Name 'Moving finding' -Status routed -Note 'Routed by me.' -On '2026-10-09' | Out-Null
    Import-VmrIssueHistory -Path $other -InputPath $export | Out-Null
    $kept = Get-VmrIssueHistory -Path $other -Name 'Moving finding'
    if ($kept.issue.status -ne 'routed') { throw "a newer local status was replaced by an older colleague one: $($kept.issue.status)" }
    if (@($kept.statusTrail | Where-Object { $_.status -eq 'owned' }).Count -ne 1) { throw 'the colleague history entry should still be in the trail' }

    # An import that brings no new history must not touch the status of an issue the operator set.
    Set-VmrIssueStatus -Path $other -Name 'Moving finding' -Status noise -Note 'Backdated decision.' -On '2026-10-01' | Out-Null
    Import-VmrIssueHistory -Path $other -InputPath $export | Out-Null
    if ((Get-VmrIssueHistory -Path $other -Name 'Moving finding').issue.status -ne 'noise') { throw 'an import with nothing new rewrote a locally set status' }
}

Check 'Import conflicts honour provenance, keep the earliest first_seen, and never replace a recorded day' {
    $mine = New-TestDatabase
    Add-VmrIssueOccurrence -Path $mine -Name 'Contended finding' -ReportDate '2026-10-05' -BuildIds @('mine') -OwnerTeam '@dotnet/dnceng' -TrackerUrl 'https://github.com/dotnet/dotnet/issues/1' | Out-Null
    Add-VmrIssueOccurrence -Path $mine -Name 'Operator kept' -ReportDate '2026-10-05' | Out-Null
    Set-VmrIssueStatus -Path $mine -Name 'Operator kept' -Status owned -OwnerTeam '@dotnet/runtime' -TrackerUrl 'https://github.com/dotnet/dotnet/issues/50' -On '2026-10-05' | Out-Null
    Add-VmrIssueOccurrence -Path $mine -Name 'Both daily' -ReportDate '2026-10-05' -OwnerTeam '@dotnet/sdk' | Out-Null

    $theirs = New-TestDatabase
    Add-VmrIssueOccurrence -Path $theirs -Name 'Contended finding' -ReportDate '2026-10-05' -BuildIds @('theirs-different-evidence') | Out-Null
    Add-VmrIssueOccurrence -Path $theirs -Name 'Contended finding' -ReportDate '2026-09-20' | Out-Null
    Set-VmrIssueStatus -Path $theirs -Name 'Contended finding' -Status owned -OwnerTeam '@dotnet/aspnet-build' -TrackerUrl 'https://github.com/dotnet/dotnet/issues/2' -On '2026-10-06' | Out-Null
    Add-VmrIssueOccurrence -Path $theirs -Name 'Operator kept' -ReportDate '2026-10-05' -OwnerTeam '@dotnet/msbuild' | Out-Null
    Set-VmrIssueStatus -Path $theirs -Name 'Operator kept' -Status owned -OwnerTeam '@dotnet/fsharp' -On '2026-10-06' | Out-Null
    Add-VmrIssueOccurrence -Path $theirs -Name 'Both daily' -ReportDate '2026-10-05' -OwnerTeam '@dotnet/nuget-team' | Out-Null
    $export = Join-Path $testRoot ('conflict-' + [guid]::NewGuid().ToString('N') + '.json')
    Export-VmrIssueHistory -Path $theirs -OutputPath $export | Out-Null

    Import-VmrIssueHistory -Path $mine -InputPath $export | Out-Null

    $contended = Get-VmrIssueHistory -Path $mine -Name 'Contended finding'
    # My daily-run values give way to their operator-recorded ones.
    if ($contended.issue.owner_team -ne '@dotnet/aspnet-build' -or $contended.issue.owner_source -ne 'operator') { throw "an operator owner from the export should replace my daily-run owner: $($contended.issue.owner_team)/$($contended.issue.owner_source)" }
    if ($contended.issue.tracker_url -notmatch 'issues/2$' -or $contended.issue.tracker_source -ne 'operator') { throw "an operator tracker from the export should replace my daily-run tracker: $($contended.issue.tracker_url)" }
    if ($contended.issue.first_seen -ne '2026-09-20' -or $contended.issue.last_seen -ne '2026-10-05') { throw "range=$($contended.issue.first_seen)..$($contended.issue.last_seen)" }
    $day = @($contended.occurrences | Where-Object { $_.report_date -eq '2026-10-05' })
    if ($day.Count -ne 1 -or $day[0].build_ids -ne 'mine') { throw "the row I already had for a day must win: $($day | ConvertTo-Json -Compress)" }

    $kept = (Get-VmrIssueHistory -Path $mine -Name 'Operator kept').issue
    if ($kept.owner_team -ne '@dotnet/runtime' -or $kept.tracker_url -notmatch 'issues/50$') { throw "my operator facts were replaced: $($kept.owner_team) $($kept.tracker_url)" }
    $daily = (Get-VmrIssueHistory -Path $mine -Name 'Both daily').issue
    if ($daily.owner_team -ne '@dotnet/sdk') { throw "a daily-run value from the export must not replace my daily-run value: $($daily.owner_team)" }

    # The reverse direction is the plain additive case: nothing of theirs is lost when I am the source.
    $fresh = New-TestDatabase
    Import-VmrIssueHistory -Path $fresh -InputPath $export | Out-Null
    $copied = Get-VmrIssueHistory -Path $fresh -Name 'Contended finding'
    if ($copied.issue.owner_source -ne 'operator' -or $copied.issue.status -ne 'owned') { throw 'a fresh import should keep what the export said' }
}

Check 'Importing a schema-1 export infers operator provenance from the status history' {
    $export = Join-Path $testRoot ('schema1-' + [guid]::NewGuid().ToString('N') + '.json')
    Write-TestExport -Path $export -Schema 1 `
        -Issues @(
            @{ slug = 'human-routed'; name = 'Human routed'; kind = 'build'; status = 'routed'; owner_team = '@dotnet/dnceng'; tracker_url = 'https://github.com/dotnet/dotnet/issues/7'; first_seen = '2026-10-01'; last_seen = '2026-10-02'; occurrences = 1 }
            @{ slug = 'machine-guessed'; name = 'Machine guessed'; kind = 'build'; status = 'open'; owner_team = '@dotnet/runtime'; first_seen = '2026-10-01'; last_seen = '2026-10-01'; occurrences = 1 }
        ) `
        -Occurrences @(
            @{ slug = 'human-routed'; report_date = '2026-10-02'; build_count = 0 }
            @{ slug = 'machine-guessed'; report_date = '2026-10-01'; build_count = 0 }
        ) `
        -History @(
            @{ slug = 'human-routed'; changed_on = '2026-10-01'; status = 'open'; note = 'First observed.'; source = 'daily-run' }
            @{ slug = 'human-routed'; changed_on = '2026-10-02'; status = 'routed'; note = 'Routed.'; source = 'operator' }
            @{ slug = 'machine-guessed'; changed_on = '2026-10-01'; status = 'open'; note = 'First observed.'; source = 'daily-run' }
        )
    $db = New-TestStorePath
    $result = Import-VmrIssueHistory -Path $db -InputPath $export
    if ($result.schemaVersion -ne 1) { throw "the import should report the export schema: $($result.schemaVersion)" }
    if ($result.issues -ne 2 -or $result.occurrences -ne 2 -or $result.statusEntries -ne 3) { throw "counts: $($result | ConvertTo-Json -Compress)" }

    $human = (Get-VmrIssueHistory -Path $db -Name 'Human routed').issue
    if ($human.owner_source -ne 'operator' -or $human.tracker_source -ne 'operator') { throw "an issue with an operator history entry should have operator facts: $($human.owner_source)/$($human.tracker_source)" }
    $machine = (Get-VmrIssueHistory -Path $db -Name 'Machine guessed').issue
    if ($machine.owner_source -ne 'daily-run') { throw "an issue with only daily-run history should have daily-run facts: $($machine.owner_source)" }
    if ($null -ne $machine.tracker_source) { throw 'a blank tracker has no provenance' }

    # And the inferred provenance has the same consequences as a recorded one.
    Add-VmrIssueOccurrence -Path $db -Name 'Human routed' -ReportDate '2026-10-03' -OwnerTeam '@dotnet/sdk' | Out-Null
    Add-VmrIssueOccurrence -Path $db -Name 'Machine guessed' -ReportDate '2026-10-03' -OwnerTeam '@dotnet/sdk' | Out-Null
    if ((Get-VmrIssueHistory -Path $db -Name 'Human routed').issue.owner_team -ne '@dotnet/dnceng') { throw 'an inferred operator owner was overwritten by a report' }
    if ((Get-VmrIssueHistory -Path $db -Name 'Machine guessed').issue.owner_team -ne '@dotnet/sdk') { throw 'an inferred daily-run owner should still be replaceable' }
}

Check 'A schema-1 file used directly as a store reads as it is and is upgraded on the first write' {
    $db = New-TestStorePath
    Write-TestExport -Path $db -Schema 1 `
        -Issues @(@{ slug = 'legacy-finding'; name = 'Legacy finding'; status = 'open'; owner_team = '@dotnet/dnceng'; first_seen = '2026-09-01'; last_seen = '2026-09-02'; occurrences = 2 }) `
        -Occurrences @(@{ slug = 'legacy-finding'; report_date = '2026-09-01' }, @{ slug = 'legacy-finding'; report_date = '2026-09-02' })
    if ((Get-VmrIssueStoreSchemaVersion -Path $db) -ne 1) { throw "schema=$(Get-VmrIssueStoreSchemaVersion -Path $db)" }
    $before = Get-Pair $db
    $legacy = (Get-VmrIssueHistory -Path $db -Name 'Legacy finding').issue
    if ($legacy.owner_source -ne 'daily-run' -or $legacy.kind -ne 'build') { throw "missing fields should default: $($legacy | ConvertTo-Json -Compress)" }
    Initialize-VmrIssueStore -Path $db | Out-Null
    Assert-Unchanged $db $before 'reading and initialising an existing store'

    Add-VmrIssueOccurrence -Path $db -Name 'Legacy finding' -ReportDate '2026-09-03' | Out-Null
    if ((Get-VmrIssueStoreSchemaVersion -Path $db) -ne 2) { throw 'the first write should upgrade the file to schema 2' }
    $upgraded = [IO.File]::ReadAllText($db)
    if ($upgraded -notmatch '"owner_source": "daily-run"') { throw 'the upgraded file should carry the inferred provenance' }
    if ((Get-VmrIssueHistory -Path $db -Name 'Legacy finding').issue.occurrences -ne 3) { throw 'the existing rows were lost in the upgrade' }
}

Check 'Rows left without an issue by the SQLite era are adopted when the issue is next recorded' {
    # The original could fold history onto a canonical slug that had no issue row; such rows must stay
    # (nothing is dropped on import) and be picked up when the issue appears.
    $export = Join-Path $testRoot ('orphans-' + [guid]::NewGuid().ToString('N') + '.json')
    Write-TestExport -Path $export -Schema 1 `
        -Aliases @(@{ alias = 'old-wording'; slug = 'real-finding'; first_seen = '2026-10-02' }) `
        -Occurrences @(@{ slug = 'real-finding'; report_date = '2026-09-21' }, @{ slug = 'real-finding'; report_date = '2026-09-22' }) `
        -History @(@{ slug = 'real-finding'; changed_on = '2026-09-21'; status = 'open'; note = 'First observed.'; source = 'daily-run' })
    $db = New-TestStorePath
    $result = Import-VmrIssueHistory -Path $db -InputPath $export
    if ($result.issues -ne 0 -or $result.occurrences -ne 2 -or $result.statusEntries -ne 1) { throw "nothing may be dropped: $($result | ConvertTo-Json -Compress)" }
    if ($null -ne (Get-VmrIssueHistory -Path $db -Name 'Old wording')) { throw 'there is no issue yet' }

    Add-VmrIssueOccurrence -Path $db -Name 'Old wording' -ReportDate '2026-10-10' | Out-Null
    $issue = (Get-VmrIssueHistory -Path $db -Name 'Real finding').issue
    if ($issue.occurrences -ne 3) { throw "the orphaned rows were not adopted: occurrences=$($issue.occurrences)" }
    if ($issue.first_seen -ne '2026-09-21' -or $issue.last_seen -ne '2026-10-10') { throw "range=$($issue.first_seen)..$($issue.last_seen)" }
}

Check 'Importing the same export twice changes nothing' {
    $mine = New-TestDatabase
    Add-VmrIssueOccurrence -Path $mine -Name 'Alpha wording' -ReportDate '2026-10-01' -OwnerTeam '@dotnet/dnceng' | Out-Null
    Add-VmrIssueOccurrence -Path $mine -Name 'Shared failure' -ReportDate '2026-10-05' -BuildIds @('9') | Out-Null
    Set-VmrIssueStatus -Path $mine -Name 'Shared failure' -Status routed -OwnerTeam '@dotnet/runtime' -On '2026-10-05' | Out-Null

    $theirs = New-TestDatabase
    Add-VmrIssueOccurrence -Path $theirs -Name 'Beta wording' -ReportDate '2026-10-03' | Out-Null
    Register-VmrIssueAlias -Path $theirs -Alias 'Alpha wording' -CanonicalName 'Beta wording' | Out-Null
    Add-VmrIssueOccurrence -Path $theirs -Name 'Shared failure' -ReportDate '2026-09-20' | Out-Null
    Set-VmrIssueStatus -Path $theirs -Name 'Shared failure' -Status owned -OwnerTeam '@dotnet/aspnet-build' -On '2026-10-07' | Out-Null
    Add-VmrIssueOccurrence -Path $theirs -Name 'Only theirs' -ReportDate '2026-10-04' -TrackerUrl 'https://github.com/dotnet/dotnet/issues/3' | Out-Null
    $export = Join-Path $testRoot ('twice-' + [guid]::NewGuid().ToString('N') + '.json')
    Export-VmrIssueHistory -Path $theirs -OutputPath $export | Out-Null

    $initial = Get-Pair $mine
    $first = Import-VmrIssueHistory -Path $mine -InputPath $export
    $afterFirst = Get-Pair $mine
    $textFirst = Get-WithoutTimestamps $mine
    if ([Convert]::ToBase64String($initial.Bytes) -ceq [Convert]::ToBase64String($afterFirst.Bytes)) { throw 'the first import should have changed the store' }
    Start-Sleep -Milliseconds 30
    $second = Import-VmrIssueHistory -Path $mine -InputPath $export
    Assert-Unchanged $mine $afterFirst 'the second import'
    if (($first | ConvertTo-Json -Compress) -ne ($second | ConvertTo-Json -Compress)) { throw "results differ: $($first | ConvertTo-Json -Compress) vs $($second | ConvertTo-Json -Compress)" }
    if ((Get-WithoutTimestamps $mine) -cne $textFirst) { throw 'the store differs once timestamps are ignored' }
}

# --- Aliases ------------------------------------------------------------------------------------

Check 'Alias registration is idempotent, never forms chains, and never orphans history' {
    $db = New-TestDatabase
    Add-VmrIssueOccurrence -Path $db -Name 'Wording one' -ReportDate '2026-10-01' -BuildIds @('1') | Out-Null
    Add-VmrIssueOccurrence -Path $db -Name 'Wording two' -ReportDate '2026-10-02' -BuildIds @('2') | Out-Null

    # The canonical issue does not exist yet: the alias-named issue becomes it rather than being orphaned.
    $slug = Register-VmrIssueAlias -Path $db -Alias 'Wording one' -CanonicalName 'Canonical wording' -On '2026-10-03'
    if ($slug -ne 'canonical-wording') { throw "returned '$slug'" }
    $canonical = Get-VmrIssueHistory -Path $db -Name 'Canonical wording'
    if ($null -eq $canonical -or $canonical.issue.name -ne 'Canonical wording' -or $canonical.issue.occurrences -ne 1) { throw "canonical issue is wrong: $($canonical.issue | ConvertTo-Json -Compress)" }
    if (@($canonical.statusTrail).Count -ne 1) { throw 'the status history should have moved with the issue' }
    if (@($canonical.aliases).Count -ne 1 -or $canonical.aliases[0].alias -ne 'wording-one') { throw 'the alias should be listed on the canonical issue' }
    $store = Read-VmrIssueStore -Path $db
    if (@($store.issues | Where-Object { $_.slug -eq 'wording-one' }).Count -ne 0) { throw 'the alias-named issue still exists' }
    if (@($store.occurrences | Where-Object { $_.slug -eq 'wording-one' }).Count -ne 0) { throw 'occurrences are still filed under the alias slug' }

    # Re-applying the alias file is a no-op, even days later.
    $before = Get-Pair $db
    Register-VmrIssueAlias -Path $db -Alias 'Wording one' -CanonicalName 'Canonical wording' | Out-Null
    Assert-Unchanged $db $before 're-registering an alias'
    if ((Register-VmrIssueAlias -Path $db -Alias 'Canonical wording' -CanonicalName 'Canonical wording') -ne 'canonical-wording') { throw 'an alias to itself should return the canonical slug' }
    Assert-Unchanged $db $before 'an alias to itself'

    # A canonical named through an existing alias resolves to the real canonical, so no chain forms.
    Register-VmrIssueAlias -Path $db -Alias 'Wording three' -CanonicalName 'Wording one' | Out-Null
    $three = @((Read-VmrIssueStore -Path $db).aliases | Where-Object { $_.alias -eq 'wording-three' })
    if ($three.Count -ne 1 -or $three[0].slug -ne 'canonical-wording') { throw "wording-three points at '$($three[0].slug)'" }

    # Aliasing a canonical away re-points everything that referred to it.
    Register-VmrIssueAlias -Path $db -Alias 'Canonical wording' -CanonicalName 'Final wording' | Out-Null
    $aliases = @((Read-VmrIssueStore -Path $db).aliases)
    $wrong = @($aliases | Where-Object { $_.slug -ne 'final-wording' })
    if ($wrong.Count -ne 0) { throw "alias chains remain: $(($wrong | ForEach-Object { "$($_.alias)->$($_.slug)" }) -join ', ')" }
    foreach ($wording in @('Wording one', 'Wording three', 'Canonical wording', 'Final wording')) {
        $history = Get-VmrIssueHistory -Path $db -Name $wording
        if ($null -eq $history -or $history.issue.slug -ne 'final-wording' -or $history.issue.occurrences -ne 1) { throw "'$wording' does not reach the final issue" }
    }
    if ($null -eq (Get-VmrIssueHistory -Path $db -Name 'Wording two')) { throw 'an unrelated issue was disturbed' }
}

Check 'Folding an alias keeps the canonical value, else the alias value, with its provenance' {
    $db = New-TestDatabase
    # Canonical value wins when both are daily-run; the alias fills blanks.
    Add-VmrIssueOccurrence -Path $db -Name 'Canonical one' -ReportDate '2026-10-01' -OwnerTeam '@dotnet/dnceng' | Out-Null
    Add-VmrIssueOccurrence -Path $db -Name 'Alias one' -ReportDate '2026-10-02' -OwnerTeam '@dotnet/runtime' -TrackerUrl 'https://github.com/dotnet/dotnet/issues/11' | Out-Null
    Set-VmrIssueStatus -Path $db -Name 'Alias one' -Status resolved -Resolution 'Fixed by PR 11' -On '2026-10-03' | Out-Null
    Register-VmrIssueAlias -Path $db -Alias 'Alias one' -CanonicalName 'Canonical one' | Out-Null
    $one = (Get-VmrIssueHistory -Path $db -Name 'Canonical one').issue
    if ($one.owner_team -ne '@dotnet/dnceng') { throw "a non-blank canonical owner must be kept: $($one.owner_team)" }
    if ($one.tracker_url -notmatch 'issues/11$' -or $one.tracker_source -ne 'daily-run') { throw "the blank tracker should be filled with its provenance: $($one.tracker_url)/$($one.tracker_source)" }
    if ($one.resolution -ne 'Fixed by PR 11') { throw "the blank resolution should be filled: $($one.resolution)" }
    if ($one.status -ne 'open') { throw "the canonical keeps its own status: $($one.status)" }
    if ($one.occurrences -ne 2 -or $one.first_seen -ne '2026-10-01' -or $one.last_seen -ne '2026-10-02') { throw "merged range is wrong: $($one | ConvertTo-Json -Compress)" }

    # An operator-recorded value is never given up in a fold, on either side.
    Set-VmrIssueStatus -Path $db -Name 'Canonical two' -Status owned -OwnerTeam '@dotnet/sdk' -On '2026-10-01' | Out-Null
    Add-VmrIssueOccurrence -Path $db -Name 'Alias two' -ReportDate '2026-10-02' -OwnerTeam '@dotnet/msbuild' | Out-Null
    Register-VmrIssueAlias -Path $db -Alias 'Alias two' -CanonicalName 'Canonical two' | Out-Null
    $two = (Get-VmrIssueHistory -Path $db -Name 'Canonical two').issue
    if ($two.owner_team -ne '@dotnet/sdk' -or $two.owner_source -ne 'operator') { throw "the canonical operator owner was lost: $($two.owner_team)/$($two.owner_source)" }

    Add-VmrIssueOccurrence -Path $db -Name 'Canonical three' -ReportDate '2026-10-01' -OwnerTeam '@dotnet/dnceng' | Out-Null
    Set-VmrIssueStatus -Path $db -Name 'Alias three' -Status owned -OwnerTeam '@dotnet/fsharp' -On '2026-10-02' | Out-Null
    Register-VmrIssueAlias -Path $db -Alias 'Alias three' -CanonicalName 'Canonical three' | Out-Null
    $three = (Get-VmrIssueHistory -Path $db -Name 'Canonical three').issue
    if ($three.owner_team -ne '@dotnet/fsharp' -or $three.owner_source -ne 'operator') { throw "the alias operator owner was lost to a daily-run canonical: $($three.owner_team)/$($three.owner_source)" }
}

# --- Failure modes ------------------------------------------------------------------------------

Check 'A SQLite store is refused with instructions for migrating it' {
    $db = New-TestStorePath
    $header = [Text.Encoding]::ASCII.GetBytes("SQLite format 3`0")
    [IO.File]::WriteAllBytes($db, [byte[]]($header + [byte[]]::new(4096 - $header.Length)))
    $good = New-TestDatabase
    $before = Get-Pair $db
    $cases = [ordered]@{
        'Initialize-VmrIssueStore'       = { Initialize-VmrIssueStore -Path $db }
        'Add-VmrIssueOccurrence'         = { Add-VmrIssueOccurrence -Path $db -Name 'Anything goes here' -ReportDate '2026-10-01' }
        'Set-VmrIssueStatus'             = { Set-VmrIssueStatus -Path $db -Name 'Anything goes here' -Status open }
        'Get-VmrIssueHistory'            = { Get-VmrIssueHistory -Path $db }
        'Get-VmrIssueStoreSchemaVersion' = { Get-VmrIssueStoreSchemaVersion -Path $db }
        'Read-VmrIssueStore'             = { Read-VmrIssueStore -Path $db }
        'Export-VmrIssueHistory'         = { Export-VmrIssueHistory -Path $db -OutputPath (Join-Path $testRoot 'never-written.json') }
        'Import-VmrIssueHistory (store)' = { Import-VmrIssueHistory -Path $db -InputPath $good }
        'Import-VmrIssueHistory (input)' = { Import-VmrIssueHistory -Path (New-TestStorePath) -InputPath $db }
    }
    # $good is itself a valid store (and so a valid export): it isolates the SQLite file as the thing refused.
    foreach ($name in $cases.Keys) {
        $caught = $null
        try { & $cases[$name] | Out-Null } catch { $caught = $_.Exception.Message }
        if (-not $caught) { throw "$name accepted a SQLite file" }
        if ($caught -notmatch 'SQLite' -or $caught -notmatch 'Export-VmrIssueHistory' -or $caught -notmatch 'Import-VmrIssueHistory') { throw "$name should explain how to migrate: $caught" }
    }
    Assert-Unchanged $db $before 'refusing a SQLite file'
    if (Test-Path -LiteralPath (Join-Path $testRoot 'never-written.json')) { throw 'an export of a refused store was written' }
}

Check 'Invalid JSON is refused with a message naming the file and is never overwritten' {
    $db = New-TestStorePath
    Set-Content -LiteralPath $db -Value '{ "schemaVersion": 2, "issues": [ { oops' -Encoding utf8
    $before = Get-Pair $db
    $cases = [ordered]@{
        'Initialize-VmrIssueStore' = { Initialize-VmrIssueStore -Path $db }
        'Add-VmrIssueOccurrence'   = { Add-VmrIssueOccurrence -Path $db -Name 'Anything goes here' -ReportDate '2026-10-01' }
        'Register-VmrIssueAlias'   = { Register-VmrIssueAlias -Path $db -Alias 'Some alias' -CanonicalName 'Some canonical' }
        'Get-VmrIssueHistory'      = { Get-VmrIssueHistory -Path $db }
    }
    foreach ($name in $cases.Keys) {
        $caught = $null
        try { & $cases[$name] | Out-Null } catch { $caught = $_.Exception.Message }
        if (-not $caught) { throw "$name accepted invalid JSON" }
        if (-not $caught.Contains($db) -or $caught -notmatch 'not valid JSON') { throw "$name should name the file and say it is invalid: $caught" }
    }
    Assert-Unchanged $db $before 'refusing invalid JSON'

    $badExport = New-TestStorePath
    Set-Content -LiteralPath $badExport -Value 'this is not json' -Encoding utf8
    $caught = $null
    try { Import-VmrIssueHistory -Path (New-TestStorePath) -InputPath $badExport | Out-Null } catch { $caught = $_.Exception.Message }
    if (-not $caught -or -not $caught.Contains($badExport)) { throw "an invalid export should be named in the message: $caught" }
    $caught = $null
    try { Import-VmrIssueHistory -Path (New-TestStorePath) -InputPath (Join-Path $testRoot 'absent-export.json') | Out-Null } catch { $caught = $_.Exception.Message }
    if (-not $caught -or $caught -notmatch 'not found') { throw "a missing export should say so: $caught" }
}

Check 'A newer schema is refused for stores and exports, naming the version it saw' {
    $db = New-TestStorePath
    Write-TestExport -Path $db -Schema 3 -Issues @(@{ slug = 'future'; name = 'Future'; status = 'open'; first_seen = '2026-10-01'; last_seen = '2026-10-01' })
    $before = Get-Pair $db
    if ((Get-VmrIssueStoreSchemaVersion -Path $db) -ne 3) { throw "the schema version probe should still report 3: $(Get-VmrIssueStoreSchemaVersion -Path $db)" }
    foreach ($case in @({ Get-VmrIssueHistory -Path $db }, { Add-VmrIssueOccurrence -Path $db -Name 'Anything goes here' -ReportDate '2026-10-01' }, { Initialize-VmrIssueStore -Path $db })) {
        $caught = $null
        try { & $case | Out-Null } catch { $caught = $_.Exception.Message }
        if (-not $caught -or $caught -notmatch '\b3\b' -or $caught -notmatch 'schema') { throw "a newer-schema store must be refused naming the version: $caught" }
    }
    Assert-Unchanged $db $before 'refusing a newer-schema store'
    $caught = $null
    try { Import-VmrIssueHistory -Path (New-TestStorePath) -InputPath $db | Out-Null } catch { $caught = $_.Exception.Message }
    if (-not $caught -or $caught -notmatch '\b3\b') { throw "a newer-schema export must be refused naming the version: $caught" }
}

Check 'A JSON file that is not an issue history is never mistaken for a store and overwritten' {
    $db = New-TestStorePath
    Set-Content -LiteralPath $db -Value '{ "recipient": "someone@example.com" }' -Encoding utf8
    $before = Get-Pair $db
    foreach ($case in @({ Initialize-VmrIssueStore -Path $db }, { Add-VmrIssueOccurrence -Path $db -Name 'Anything goes here' -ReportDate '2026-10-01' })) {
        $caught = $null
        try { & $case | Out-Null } catch { $caught = $_.Exception.Message }
        if (-not $caught -or $caught -notmatch 'not an issue history') { throw "unrelated JSON must be refused: $caught" }
    }
    Assert-Unchanged $db $before 'refusing unrelated JSON'
}

Check 'A blank placeholder file is initialised as an empty store' {
    $db = New-TestStorePath
    [IO.File]::WriteAllBytes($db, [byte[]]@())
    if (@(Get-VmrIssueHistory -Path $db).Count -ne 0) { throw 'a blank file has no history' }
    Initialize-VmrIssueStore -Path $db | Out-Null
    if ((Get-VmrIssueStoreSchemaVersion -Path $db) -ne 2) { throw 'initialising should have written an empty schema-2 store' }
    Add-VmrIssueOccurrence -Path $db -Name 'First in a placeholder' -ReportDate '2026-10-01' | Out-Null
    if ((Get-VmrIssueHistory -Path $db -Name 'First in a placeholder').issue.occurrences -ne 1) { throw 'the store did not work after initialising' }
}

Check 'Initialize creates missing folders, returns the path, and leaves an existing store alone' {
    $db = Join-Path $testRoot ('nested-' + [guid]::NewGuid().ToString('N') + '\deeper\issue-history.json')
    if ((Initialize-VmrIssueStore -Path $db) -ne $db) { throw 'Initialize-VmrIssueStore should return the path it was given' }
    if (-not (Test-Path -LiteralPath $db)) { throw 'the store was not created' }
    if ((Get-VmrIssueStoreSchemaVersion -Path $db) -ne 2) { throw 'a new store is schema 2' }
    $empty = Read-VmrIssueStore -Path $db
    if ($empty.issues.Count -ne 0 -or $empty.aliases.Count -ne 0 -or $empty.occurrences.Count -ne 0 -or $empty.statusHistory.Count -ne 0) { throw 'a new store is empty' }
    Add-VmrIssueOccurrence -Path $db -Name 'Survives initialisation' -ReportDate '2026-10-01' | Out-Null
    $before = Get-Pair $db
    Initialize-VmrIssueStore -Path $db | Out-Null
    Assert-Unchanged $db $before 'initialising an existing store'
    if ($null -eq (Get-VmrIssueHistory -Path $db -Name 'Survives initialisation')) { throw 'initialising an existing store lost its data' }

    $caught = $null
    try { Initialize-VmrIssueStore -Path (Split-Path -Parent $db) | Out-Null } catch { $caught = $_.Exception.Message }
    if (-not $caught -or $caught -notmatch 'folder') { throw "a folder is not a store and should say so: $caught" }
}

Check 'A store that stays locked fails with a clear message instead of hanging' {
    $db = New-TestDatabase
    $holder = [IO.File]::Open("$db.lock", [IO.FileMode]::OpenOrCreate, [IO.FileAccess]::ReadWrite, [IO.FileShare]::None)
    $previous = $script:VmrIssueStoreLockTimeoutSeconds
    $script:VmrIssueStoreLockTimeoutSeconds = 1
    try {
        $watch = [Diagnostics.Stopwatch]::StartNew()
        $caught = $null
        try { Add-VmrIssueOccurrence -Path $db -Name 'Blocked finding' -ReportDate '2026-10-01' | Out-Null } catch { $caught = $_.Exception.Message }
        $watch.Stop()
        if (-not $caught) { throw 'a held lock must block a writer' }
        if ($caught -notmatch 'Timed out' -or -not $caught.Contains($db)) { throw "the message should say what happened and name the store: $caught" }
        if ($watch.Elapsed.TotalSeconds -gt 10) { throw "waited $($watch.Elapsed.TotalSeconds) s for a 1 s timeout" }
        # Readers are never blocked by a writer.
        if (@(Get-VmrIssueHistory -Path $db).Count -ne 0) { throw 'a reader should see the unchanged store while it is locked' }
    }
    finally {
        $script:VmrIssueStoreLockTimeoutSeconds = $previous
        $holder.Dispose()
    }
    Add-VmrIssueOccurrence -Path $db -Name 'Blocked finding' -ReportDate '2026-10-01' | Out-Null
    if ((Get-VmrIssueHistory -Path $db -Name 'Blocked finding').issue.occurrences -ne 1) { throw 'the store should work again once the lock is released' }
}

# --- Concurrency, files on disk, determinism --------------------------------------------------

Check 'Two processes writing to the same store at once lose nothing' {
    $dir = Join-Path $testRoot ('concurrent-' + [guid]::NewGuid().ToString('N'))
    [void][IO.Directory]::CreateDirectory($dir)
    $store = Join-Path $dir 'issue-history.json'
    Initialize-VmrIssueStore -Path $store | Out-Null
    $library = (Resolve-Path -LiteralPath (Join-Path $PSScriptRoot '..\history\store.ps1')).Path
    $worker = Join-Path $dir 'worker.ps1'
    Set-Content -LiteralPath $worker -Encoding utf8 -Value @'
param([string] $Library, [string] $Store, [string] $Worker, [int] $Count)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
. $Library
$offset = if ($Worker -eq 'B') { 100 } else { 0 }
for ($i = 0; $i -lt $Count; $i++) {
    $day = ([datetime]'2026-01-01').AddDays($offset + $i).ToString('yyyy-MM-dd')
    # Five shared issues, so both processes keep rewriting the same records.
    Add-VmrIssueOccurrence -Path $Store -Name ('Concurrent finding {0}' -f ($i % 5)) -ReportDate $day -BuildIds @("$Worker-$i") | Out-Null
}
'@
    $pwsh = (Get-Process -Id $PID).Path
    $processes = @(foreach ($id in @('A', 'B')) {
        $arguments = @('-NoProfile', '-NonInteractive', '-File', "`"$worker`"", '-Library', "`"$library`"", '-Store', "`"$store`"", '-Worker', $id, '-Count', '40')
        $started = Start-Process -FilePath $pwsh -ArgumentList $arguments -PassThru -NoNewWindow `
            -RedirectStandardOutput (Join-Path $dir "worker-$id.out") -RedirectStandardError (Join-Path $dir "worker-$id.err")
        $null = $started.Handle
        $started
    })
    $deadline = [DateTime]::UtcNow.AddSeconds(60)
    foreach ($process in $processes) {
        $remaining = [int][Math]::Max(1, ($deadline - [DateTime]::UtcNow).TotalMilliseconds)
        if (-not $process.WaitForExit($remaining)) {
            foreach ($other in $processes) { if (-not $other.HasExited) { Stop-Process -Id $other.Id -Force } }
            throw 'the writers did not finish within 60 seconds'
        }
    }
    foreach ($id in @('A', 'B')) {
        $errors = Get-Content -LiteralPath (Join-Path $dir "worker-$id.err") -Raw -ErrorAction SilentlyContinue
        if ($errors) { throw "worker $id reported: $errors" }
    }
    foreach ($process in $processes) { if ($process.ExitCode -ne 0) { throw "a writer exited with code $($process.ExitCode)" } }

    $parsed = Get-Content -LiteralPath $store -Raw | ConvertFrom-Json
    if (@($parsed.occurrences).Count -ne 80) { throw "the store holds $(@($parsed.occurrences).Count) occurrence rows, expected 80" }
    $data = Read-VmrIssueStore -Path $store
    $keys = @($data.occurrences | ForEach-Object { "$($_.slug)|$($_.report_date)" } | Sort-Object -Unique)
    if ($keys.Count -ne 80) { throw "only $($keys.Count) distinct occurrences survived" }
    $evidence = @($data.occurrences | ForEach-Object { $_.build_ids } | Sort-Object -Unique)
    if ($evidence.Count -ne 80) { throw "only $($evidence.Count) of 80 writes are present" }
    if (@($data.issues).Count -ne 5) { throw "expected 5 issues, found $(@($data.issues).Count)" }
    $counted = 0
    foreach ($issue in $data.issues) {
        $rows = @($data.occurrences | Where-Object { $_.slug -eq $issue.slug }).Count
        if ($issue.occurrences -ne $rows) { throw "$($issue.slug) says $($issue.occurrences) occurrences but has $rows rows" }
        if ($issue.first_seen -ne '2026-01-01' -and $issue.first_seen -ne '2026-01-02' -and $issue.first_seen -ne '2026-01-03' -and $issue.first_seen -ne '2026-01-04' -and $issue.first_seen -ne '2026-01-05') { throw "$($issue.slug) first_seen=$($issue.first_seen)" }
        $counted += $issue.occurrences
    }
    if ($counted -ne 80) { throw "issue counters add up to $counted, expected 80" }
    if (@($data.statusHistory).Count -ne 5) { throw "expected one first-observed entry per issue, found $(@($data.statusHistory).Count)" }
    $leftovers = @(Get-ChildItem -LiteralPath $dir -Force | Where-Object { $_.Name -like '*.tmp' })
    if ($leftovers.Count -ne 0) { throw "temp files left behind: $($leftovers.Name -join ', ')" }
}

Check 'No temporary or partial files are left behind, and a crashed writer''s leftovers are swept' {
    $dir = Join-Path $testRoot ('files-' + [guid]::NewGuid().ToString('N'))
    [void][IO.Directory]::CreateDirectory($dir)
    $db = Join-Path $dir 'issue-history.json'
    # What a writer that died before replacing the store would leave, and something unrelated that must stay.
    $stale = "$db.$([guid]::NewGuid().ToString('N')).tmp"
    Set-Content -LiteralPath $stale -Value 'partial write' -Encoding utf8
    $unrelated = Join-Path $dir 'issue-history.json.notes.tmp'
    Set-Content -LiteralPath $unrelated -Value 'not ours' -Encoding utf8

    Initialize-VmrIssueStore -Path $db | Out-Null
    foreach ($day in 1..12) {
        Add-VmrIssueOccurrence -Path $db -Name "Churning finding $($day % 3)" -ReportDate ('2026-10-{0:D2}' -f $day) -BuildIds @("$day") | Out-Null
    }
    Set-VmrIssueStatus -Path $db -Name 'Churning finding 1' -Status owned -OwnerTeam '@dotnet/dnceng' -On '2026-10-13' | Out-Null
    Register-VmrIssueAlias -Path $db -Alias 'Churning finding 0' -CanonicalName 'Churning finding 2' | Out-Null
    $export = Join-Path $dir 'export.json'
    Export-VmrIssueHistory -Path $db -OutputPath $export | Out-Null
    Import-VmrIssueHistory -Path (Join-Path $dir 'other.json') -InputPath $export | Out-Null

    $names = @(Get-ChildItem -LiteralPath $dir -Force | ForEach-Object { $_.Name } | Sort-Object)
    $expected = @('export.json', 'issue-history.json', 'issue-history.json.lock', 'issue-history.json.notes.tmp', 'other.json', 'other.json.lock')
    if (($names -join '|') -ne ($expected -join '|')) { throw "unexpected files: $($names -join ', ')" }
    if (Test-Path -LiteralPath $stale) { throw 'the crashed writer leftover was not swept' }
    $null = Get-Content -LiteralPath $db -Raw | ConvertFrom-Json
    $null = Get-Content -LiteralPath (Join-Path $dir 'other.json') -Raw | ConvertFrom-Json
}

Check 'Output is deterministic: sorted, LF-only, BOM-free, and stable across exports and a round trip' {
    $db = New-TestDatabase
    # Recorded deliberately out of order, with a same-day tie in the status trail.
    foreach ($entry in @(@('Zulu finding', '2026-10-03'), @('Alpha finding', '2026-10-02'), @('Mike finding', '2026-10-01'), @('Alpha finding', '2026-10-01'), @('Zulu finding', '2026-10-01'))) {
        Add-VmrIssueOccurrence -Path $db -Name $entry[0] -ReportDate $entry[1] -BuildIds @($entry[1]) | Out-Null
    }
    Register-VmrIssueAlias -Path $db -Alias 'Mike old name' -CanonicalName 'Mike finding' | Out-Null
    Register-VmrIssueAlias -Path $db -Alias 'Alpha old name' -CanonicalName 'Alpha finding' | Out-Null
    Set-VmrIssueStatus -Path $db -Name 'Zulu finding' -Status routed -Note 'Routed first.' -On '2026-10-05' | Out-Null
    Set-VmrIssueStatus -Path $db -Name 'Zulu finding' -Status owned -Note 'Then owned.' -On '2026-10-05' | Out-Null

    $bytes = [IO.File]::ReadAllBytes($db)
    if ($bytes.Length -ge 3 -and $bytes[0] -eq 0xEF -and $bytes[1] -eq 0xBB -and $bytes[2] -eq 0xBF) { throw 'the store must be UTF-8 without a BOM' }
    if ($bytes -contains 13) { throw 'the store must use LF line endings only' }
    if ($bytes[$bytes.Length - 1] -ne 10) { throw 'the store should end with a newline' }
    $text = [Text.Encoding]::UTF8.GetString($bytes)
    if (-not $text.StartsWith("{`n  `"schemaVersion`": 2,`n  `"updatedAtUtc`": `"")) { throw "unexpected header: $($text.Substring(0, 60))" }

    $parsed = $text | ConvertFrom-Json -DateKind String
    $sortedOrdinal = { param($values) $copy = [string[]]@($values); [Array]::Sort($copy, [StringComparer]::Ordinal); ($copy -join '|') }
    if ((& $sortedOrdinal @($parsed.issues | ForEach-Object { $_.slug })) -cne (@($parsed.issues | ForEach-Object { $_.slug }) -join '|')) { throw 'issues are not sorted by slug' }
    if ((& $sortedOrdinal @($parsed.aliases | ForEach-Object { $_.alias })) -cne (@($parsed.aliases | ForEach-Object { $_.alias }) -join '|')) { throw 'aliases are not sorted by alias' }
    $occurrenceKeys = @($parsed.occurrences | ForEach-Object { "$($_.slug)`0$($_.report_date)" })
    if ((& $sortedOrdinal $occurrenceKeys) -cne ($occurrenceKeys -join '|')) { throw 'occurrences are not sorted by slug then report_date' }
    $historyKeys = @($parsed.statusHistory | ForEach-Object { "$($_.slug)`0$($_.changed_on)" })
    $sortedHistory = [string[]]@($historyKeys); [Array]::Sort($sortedHistory, [StringComparer]::Ordinal)
    if (($sortedHistory -join '|') -cne ($historyKeys -join '|')) { throw 'status history is not sorted by slug then changed_on' }
    $tie = @($parsed.statusHistory | Where-Object { $_.slug -eq 'zulu-finding' -and $_.changed_on -eq '2026-10-05' } | ForEach-Object { $_.status })
    if (($tie -join ',') -ne 'routed,owned') { throw "same-day entries must keep the order they were recorded in: $($tie -join ',')" }

    $first = Join-Path $testRoot ('export-a-' + [guid]::NewGuid().ToString('N') + '.json')
    $second = Join-Path $testRoot ('export-b-' + [guid]::NewGuid().ToString('N') + '.json')
    Export-VmrIssueHistory -Path $db -OutputPath $first | Out-Null
    Start-Sleep -Milliseconds 20
    Export-VmrIssueHistory -Path $db -OutputPath $second | Out-Null
    if ((Get-WithoutTimestamps $first) -cne (Get-WithoutTimestamps $second)) { throw 'two exports of the same data differ' }
    if ((Get-WithoutTimestamps $first) -cne (Get-WithoutTimestamps $db)) { throw 'an export should have the shape of the store itself' }
    if ([IO.File]::ReadAllText($first) -notmatch '"exportedAtUtc"') { throw 'an export should say when it was made' }

    $roundTrip = New-TestStorePath
    Import-VmrIssueHistory -Path $roundTrip -InputPath $first | Out-Null
    if ((Get-WithoutTimestamps $roundTrip) -cne (Get-WithoutTimestamps $db)) { throw 'export followed by import did not reproduce the store' }
    $third = Join-Path $testRoot ('export-c-' + [guid]::NewGuid().ToString('N') + '.json')
    Export-VmrIssueHistory -Path $roundTrip -OutputPath $third | Out-Null
    if ((Get-WithoutTimestamps $third) -cne (Get-WithoutTimestamps $first)) { throw 'a round trip changed the export' }
}

Check 'A reader polling the store while another process rewrites it never sees it missing or half written' {
    $dir = Join-Path $testRoot ('readers-' + [guid]::NewGuid().ToString('N'))
    [void][IO.Directory]::CreateDirectory($dir)
    $store = Join-Path $dir 'issue-history.json'
    Initialize-VmrIssueStore -Path $store | Out-Null
    foreach ($i in 1..30) { Add-VmrIssueOccurrence -Path $store -Name "Seed finding $i" -ReportDate '2026-10-01' -BuildIds @("$i") | Out-Null }
    $library = (Resolve-Path -LiteralPath (Join-Path $PSScriptRoot '..\history\store.ps1')).Path
    $worker = Join-Path $dir 'writer.ps1'
    Set-Content -LiteralPath $worker -Encoding utf8 -Value @'
param([string] $Library, [string] $Store, [int] $Count)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
. $Library
for ($i = 0; $i -lt $Count; $i++) { Add-VmrIssueOccurrence -Path $Store -Name "Rewritten finding $($i % 6)" -ReportDate ('2026-11-{0:D2}' -f (1 + $i)) -BuildIds @("r-$i") | Out-Null }
'@
    $pwsh = (Get-Process -Id $PID).Path
    $writer = Start-Process -FilePath $pwsh -ArgumentList @('-NoProfile', '-NonInteractive', '-File', "`"$worker`"", '-Library', "`"$library`"", '-Store', "`"$store`"", '-Count', '28') `
        -PassThru -NoNewWindow -RedirectStandardOutput (Join-Path $dir 'writer.out') -RedirectStandardError (Join-Path $dir 'writer.err')
    $null = $writer.Handle
    $deadline = [DateTime]::UtcNow.AddSeconds(60)
    $reads = 0; $problems = [Collections.Generic.List[string]]::new()
    while (-not $writer.HasExited -and [DateTime]::UtcNow -lt $deadline) {
        if (-not [IO.File]::Exists($store)) { $problems.Add('the store was missing for an instant'); continue }
        try {
            $stream = [IO.File]::Open($store, [IO.FileMode]::Open, [IO.FileAccess]::Read, ([IO.FileShare]::ReadWrite -bor [IO.FileShare]::Delete))
            try { $text = [IO.StreamReader]::new($stream).ReadToEnd() } finally { $stream.Dispose() }
            $parsed = $text | ConvertFrom-Json
            if (@($parsed.issues).Count -lt 30) { $problems.Add("a read saw only $(@($parsed.issues).Count) issues") }
            if (@(Get-VmrIssueHistory -Path $store -Limit 1).Count -ne 1) { $problems.Add('the store reader found no history') }
            $reads++
        }
        catch { $problems.Add($_.Exception.Message) }
    }
    if (-not $writer.WaitForExit(60000)) { $writer.Kill(); throw 'the writer did not finish' }
    $errors = Get-Content -LiteralPath (Join-Path $dir 'writer.err') -Raw -ErrorAction SilentlyContinue
    if ($errors) { throw "the writer reported: $errors" }
    if ($writer.ExitCode -ne 0) { throw "the writer exited with code $($writer.ExitCode)" }
    if ($reads -lt 3) { throw "only $reads reads overlapped the writes; the check did not exercise anything" }
    if ($problems.Count -ne 0) { throw "$($problems.Count) problem(s) in $reads reads, e.g. $($problems | Select-Object -Unique -First 2)" }
    $rows = @((Read-VmrIssueStore -Path $store).occurrences | Where-Object { $_.build_ids -like 'r-*' })
    if ($rows.Count -ne 28) { throw "the writer's $($rows.Count) of 28 rows are present" }
}

Check 'Read-VmrIssueStore returns a deep copy with array-shaped collections' {
    $missing = Read-VmrIssueStore -Path (New-TestStorePath)
    if ($missing.schemaVersion -ne 0 -or $missing.issues.Count -ne 0 -or $missing.statusHistory.Count -ne 0) { throw 'a missing store reads as empty with schema 0' }

    $empty = Read-VmrIssueStore -Path (New-TestDatabase)
    foreach ($name in @('issues', 'aliases', 'occurrences', 'statusHistory')) {
        if ($empty.$name -isnot [array] -or $empty.$name.Count -ne 0) { throw "$name should be an empty array" }
    }

    $db = New-TestDatabase
    Add-VmrIssueOccurrence -Path $db -Name 'Only finding' -ReportDate '2026-10-01' -BuildIds @('1') | Out-Null
    $one = Read-VmrIssueStore -Path $db
    foreach ($name in @('issues', 'occurrences', 'statusHistory')) {
        if ($one.$name -isnot [array] -or $one.$name.Count -ne 1) { throw "a single $name entry must still be an array" }
    }
    if ($one.schemaVersion -ne 2 -or -not $one.updatedAtUtc) { throw 'schemaVersion and updatedAtUtc should be present' }

    Add-VmrIssueOccurrence -Path $db -Name 'Second finding' -ReportDate '2026-10-02' -OwnerTeam '@dotnet/dnceng' | Out-Null
    Register-VmrIssueAlias -Path $db -Alias 'Second wording' -CanonicalName 'Second finding' | Out-Null
    $copy = Read-VmrIssueStore -Path $db
    $copy.issues[0].status = 'tampered'
    $copy.issues[1].owner_team = 'tampered'
    $copy.occurrences[0].build_count = 99
    $copy.aliases[0].slug = 'tampered'
    $copy.statusHistory[0].status = 'tampered'
    $copy.issues = @()
    $again = Read-VmrIssueStore -Path $db
    if ($again.issues.Count -ne 2 -or $again.issues[0].status -ne 'open' -or $again.issues[1].owner_team -ne '@dotnet/dnceng') { throw 'editing a copy changed the store' }
    if ($again.occurrences[0].build_count -ne 1 -or $again.aliases[0].slug -ne 'second-finding' -or $again.statusHistory[0].status -ne 'open') { throw 'editing a copy changed the store' }
    if ((Get-VmrIssueHistory -Path $db -Name 'Only finding').issue.status -ne 'open') { throw 'editing a copy reached the issue reader' }
}

Check 'Changes made outside this process are picked up, even when the file looks unchanged' {
    $db = New-TestDatabase
    $utf8 = [Text.UTF8Encoding]::new($false)
    Add-VmrIssueOccurrence -Path $db -Name 'Watched finding' -ReportDate '2026-10-01' -OwnerTeam '@dotnet/dnceng' | Out-Null
    if ((Get-VmrIssueHistory -Path $db -Name 'Watched finding').issue.name -ne 'Watched finding') { throw 'precondition' }

    # Another writer replaces the file with something of a different size.
    $edited = [IO.File]::ReadAllText($db).Replace('"name": "Watched finding"', '"name": "Watched finding, renamed by hand"')
    [IO.File]::WriteAllText($db, $edited, $utf8)
    if ((Get-VmrIssueHistory -Path $db -Name 'Watched finding').issue.name -ne 'Watched finding, renamed by hand') { throw 'a reader kept serving the cached copy after the file changed' }

    # Same length and the same timestamp: only a writer that checks the content can notice.
    $stamp = ([IO.FileInfo]::new($db)).LastWriteTimeUtc
    $sameLength = $edited.Replace('"owner_team": "@dotnet/dnceng"', '"owner_team": "@dotnet/runtim"')
    if ($sameLength.Length -ne $edited.Length -or $sameLength -ceq $edited) { throw 'test bug: the edit must keep the length' }
    [IO.File]::WriteAllText($db, $sameLength, $utf8)
    [IO.File]::SetLastWriteTimeUtc($db, $stamp)
    Add-VmrIssueOccurrence -Path $db -Name 'Watched finding' -ReportDate '2026-10-02' | Out-Null
    $final = (Get-VmrIssueHistory -Path $db -Name 'Watched finding').issue
    if ($final.owner_team -ne '@dotnet/runtim') { throw "a writer built on a stale cache and undid the outside edit: $($final.owner_team)" }
    if ($final.occurrences -ne 2) { throw 'the new occurrence was not recorded' }
}

}
finally { Remove-Item -LiteralPath $testRoot -Recurse -Force -ErrorAction SilentlyContinue }

if ($failures) { Write-Host "$failures issue-store test(s) failed." -ForegroundColor Red; exit 1 }
Write-Host "$checks offline issue-store tests. Temporary files only; no reports, email, or external changes." -ForegroundColor Green
