[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# One gate, three boundaries. The same report, the same date, the same contacts, the same Build Health list and the same history
# must be accepted or refused identically when it is generated, when saved reports are reused, and when a person checks it by hand.
# Advisory findings stay advisory at every boundary.
$scripts = Split-Path -Parent $PSScriptRoot
. (Join-Path $scripts 'common\core.ps1')
. (Join-Path $scripts 'history\history.ps1')
. (Join-Path $scripts 'daily\contract.ps1')

$failures = 0
function Check([string] $Name, [scriptblock] $Body) {
    try { & $Body; Write-Host "PASS: $Name" -ForegroundColor Green }
    catch { $script:failures++; Write-Host "FAIL: $Name -> $($_.Exception.Message)" -ForegroundColor Red }
}
function Assert($Condition, [string] $Message) { if (-not $Condition) { throw $Message } }

$sandbox = Join-Path ([IO.Path]::GetTempPath()) ('vmrqb-parity-' + [guid]::NewGuid().ToString('N'))
[void][IO.Directory]::CreateDirectory($sandbox)
$originalHome = $env:VMR_QB_HOME
$env:VMR_QB_HOME = Join-Path $sandbox 'state'
$paths = Get-VmrQBPaths -Create
$date = '2026-10-08'
$pwsh = (Get-Process -Id $PID).Path

# The report is judged against earlier days: Alpha is known and open, Beta is known and owned by someone.
Initialize-VmrIssueStore -Path $paths.HistoryPath | Out-Null
Add-VmrIssueOccurrence -Path $paths.HistoryPath -Name 'Alpha failure' -ReportDate '2026-10-01' -BuildIds @('1') | Out-Null
Add-VmrIssueOccurrence -Path $paths.HistoryPath -Name 'Beta failure' -ReportDate '2026-10-02' -BuildIds @('2') | Out-Null
Set-VmrIssueStatus -Path $paths.HistoryPath -Name 'Beta failure' -Status owned -Note 'The owner has it.' -On '2026-10-02' -Source 'operator: test' | Out-Null

$header = '| Issue category | Builds / PRs (links) | New Issue (Y/N) | Existing fix PR (status) | Action as VMR QB (who / where to ping) | Links that QB reported |'
$where = 'Teams - Unified Build Technical and General Discussions'
function New-Row([string] $Finding, [int] $BuildId, [string] $Novelty = 'Y', [string] $Description = 'Please check the build failure. The build failed', [string] $Owner = '@dotnet/dnceng') {
    $link = "[$BuildId](https://dev.azure.com/dnceng/internal/_build/results?buildId=$BuildId)"
    $message = "${Owner}, ${Description}: $link"
    return "| BH #- - $Finding | $link | $Novelty | None found | Where: $where.<br>Copy/paste: `"$message`" | None found |"
}function New-Overall([string[]] $Rows, [string] $TitleDate = $date, [string] $MonitorRows = '') {
    $first = if ($Rows.Count) { [regex]::Match($Rows[0], '\[(\d+)\]\((https://[^)]+)\)') } else { $null }
    $top = if ($first -and $first.Success) { "1. [$($first.Groups[1].Value)]($($first.Groups[2].Value)) - A failure needs the QB. Where: $where." } else { 'No QB action required.' }
    $text = @("# VMR QB Daily Overall Report - $TitleDate", '', '## Top actions for the QB today', '', $top, '', '## Summary', '', $header, '|---|---|---|---|---|---|') + $Rows
    if ($MonitorRows) { $text += @('', '## Monitor only - no QB action needed', '', $header, '|---|---|---|---|---|---|', $MonitorRows) }
    return ($text -join "`n") + "`n"
}
$buildHealth = "# Build Health Monitoring - $date`n`n## Priority actions`n1. **Alpha failure:** [1](https://dev.azure.com/dnceng/internal/_build/results?buildId=1) failed.`n"

# --- the three boundaries -----------------------------------------------------------------------
$runner = [Management.Automation.Language.Parser]::ParseFile((Join-Path $scripts 'daily\Invoke-Daily.ps1'), [ref]$null, [ref]$null)
foreach ($name in @('Get-QBTeamContacts', 'Get-QBTranscriptModel', 'Test-QBPinnedModelRejected', 'Invoke-QBModelRun', 'Complete-QBReportDocument', 'New-Report')) {
    $function = $runner.Find({ param($node) $node -is [Management.Automation.Language.FunctionDefinitionAst] -and $node.Name -eq $name }, $false)
    . ([scriptblock]::Create($function.Extent.Text))
}
$Date = $date
$stamp = 'parity'
$logDir = Join-Path $sandbox 'logs'; [void][IO.Directory]::CreateDirectory($logDir)
$runWorkspace = [pscustomobject]@{ Path = $sandbox }
$teamContacts = @{}
$script:generated = ''
function Invoke-CopilotReport { param($Prompt, $TranscriptPath, $ErrorPath, [switch] $Synthesis) return $script:generated }

# What each boundary does with a report. Generation and reuse never refuse one: the checks they run become run notes and the
# report is still saved. The manual check is the one that refuses (a non-zero exit), because a person asked for a verdict.
function Get-CheckNotes { return (@(Get-QBRunNotes | Where-Object { $_.source -eq 'report-check' } | ForEach-Object { $_.message }) -join ' | ') }
function Get-GenerationOutcome([string] $Report, [string] $BuildHealthPath) {
    $script:generated = $Report
    Clear-QBRunNotes
    $outputPath = Join-Path $sandbox ('generated-' + [guid]::NewGuid().ToString('N') + '.md')
    $result = New-Report -Name overall-summary -Prompt 'fixture' -OutputPath $outputPath -Synthesis -BuildHealthPath $BuildHealthPath 6>$null 3>$null
    return [pscustomobject]@{ saved = ($result.status -eq 'succeeded' -and (Test-Path -LiteralPath $outputPath)); message = (Get-CheckNotes) }
}
function Get-ReuseOutcome([string] $Report) {
    $root = Join-Path $sandbox ('reuse-' + [guid]::NewGuid().ToString('N'))
    $reports = Join-Path $root 'reports'; [void][IO.Directory]::CreateDirectory($reports)
    [IO.File]::WriteAllText((Join-Path $reports "build-health-$date.md"), $buildHealth)
    [IO.File]::WriteAllText((Join-Path $reports "code-flow-$date.md"), "# Code Flow Monitoring - $date`n")
    [IO.File]::WriteAllText((Join-Path $reports "overall-summary-$date.md"), $Report)
    Clear-QBRunNotes
    $records = @(Get-QBExistingReports -Root $root -Date $date -TeamContacts $teamContacts -HistoryPath $paths.HistoryPath)
    return [pscustomobject]@{ saved = ($records.Count -eq 3); message = (Get-CheckNotes) }
}
function Get-ManualOutcome([string] $Report, [string] $BuildHealthPath) {
    $file = Join-Path $sandbox ("manual-$([guid]::NewGuid().ToString('N'))-$date.md")
    [IO.File]::WriteAllText($file, $Report)
    $text = & $pwsh -NoProfile -NonInteractive -File (Join-Path $scripts 'daily\Invoke-Report.ps1') -ReportPath $file -BuildHealthPath $BuildHealthPath 2>&1 | Out-String
    return [pscustomobject]@{ blocked = ($LASTEXITCODE -ne 0); message = $text }
}
$buildHealthFile = Join-Path $sandbox "build-health-$date.md"
[IO.File]::WriteAllText($buildHealthFile, $buildHealth)

$scenarios = @(
    @{ name = 'a clean report'; report = (New-Overall @((New-Row 'Gamma failure' 3))); blocked = $false },
    @{ name = 'New Issue = Y for a finding history already holds'; report = (New-Overall @((New-Row 'Alpha failure' 1 'Y'))); blocked = $true; expect = 'says New Issue = Y but history first saw it' },
    @{ name = 'an owned finding in the main table'; report = (New-Overall @((New-Row 'Beta failure' 2 'N'))); blocked = $true; expect = 'belongs in the Monitor only table' },
    @{ name = 'a description over 30 words'; report = (New-Overall @((New-Row 'Gamma failure' 3 'Y' ('Please check the build failure. ' + ((1..31 | ForEach-Object { "word$_" }) -join ' '))))); blocked = $true; expect = 'maximum is 30' },
    @{ name = 'a ping to a team that does not exist'; report = (New-Overall @((New-Row 'Gamma failure' 3 'Y' 'Please check the build failure. The build failed' '@dotnet/totally-fake'))); blocked = $true; expect = 'totally-fake' },
    @{ name = 'unfinished placeholder text'; report = (New-Overall @((New-Row 'Gamma failure' 3 'Y' 'Please check the build failure. TODO describe it'))); blocked = $true; expect = 'placeholder' },
    @{ name = 'advisory findings only (New Issue = N with no history under that name)'; report = (New-Overall @((New-Row 'Delta failure' 4 'N'))); blocked = $false }
)

Check 'Every boundary finds the same things in a report: generation and reuse note them and still save it, the manual check refuses it' {
    foreach ($scenario in $scenarios) {
        $generation = Get-GenerationOutcome $scenario.report $buildHealthFile
        $reuse = Get-ReuseOutcome $scenario.report
        $manual = Get-ManualOutcome $scenario.report $buildHealthFile
        Assert ($generation.saved -and $reuse.saved) "[$($scenario.name)] generation saved=$($generation.saved), reuse kept=$($reuse.saved): a report is never withheld"
        Assert ($manual.blocked -eq $scenario.blocked) "[$($scenario.name)] the manual check $(if ($manual.blocked) { 'refused' } else { 'accepted' }) it, expected $(if ($scenario.blocked) { 'refusal' } else { 'acceptance' }): $($manual.message)"
        foreach ($pair in @(@('generation', $generation), @('reuse', $reuse))) {
            if ($scenario.blocked) { Assert ($pair[1].message -match $scenario.expect) "[$($scenario.name)] $($pair[0]) did not note it: '$($pair[1].message)'" }
            else { Assert (-not $pair[1].message) "[$($scenario.name)] $($pair[0]) noted something about a clean report: '$($pair[1].message)'" }
        }
        if ($scenario.ContainsKey('expect')) { Assert ($manual.message -match $scenario.expect) "[$($scenario.name)] the manual check refused it for another reason: $($manual.message)" }
    }
}

Check 'A numbered finding outside the visible Build Health list is noted when reused and refused by the manual check' {
    $row = "| BH #7 - Alpha failure | [1](https://dev.azure.com/dnceng/internal/_build/results?buildId=1) | N | None found | Where: $where.<br>Copy/paste: `"@dotnet/dnceng, Please check the build failure. Alpha failed: [1](https://dev.azure.com/dnceng/internal/_build/results?buildId=1)`" | None found |"
    $report = (New-Overall @($row)).Replace('A failure needs the QB.', 'BH #7: Alpha failure needs the QB.')
    $reuse = Get-ReuseOutcome $report
    $manual = Get-ManualOutcome $report $buildHealthFile
    Assert ($reuse.saved -and $manual.blocked) "reuse kept=$($reuse.saved), manual blocked=$($manual.blocked)"
    Assert ($reuse.message -match 'BH #7 does not exist' -and $manual.message -match 'BH #7 does not exist') "both must name the missing action: $($reuse.message) | $($manual.message)"
}

Check 'A team ping without its named contact is noted at generation and reuse and refused by the manual check once a roster exists, and a report with no history store still passes with a warning' {
    $roster = @{ '@dotnet/dnceng' = [pscustomobject]@{ github = 'someone'; teams = 'Some One'; basis = 'test' } }
    [IO.File]::WriteAllText((Get-VmrQBContactsPath -ForWrite), (ConvertTo-Json -InputObject ([ordered]@{ contacts = $roster }) -Depth 6))
    $script:teamContacts = $roster
    $report = New-Overall @((New-Row 'Gamma failure' 3))
    $generation = Get-GenerationOutcome $report $buildHealthFile
    $reuse = Get-ReuseOutcome $report
    $manual = Get-ManualOutcome $report $buildHealthFile
    foreach ($pair in @(@('generation', $generation), @('reuse', $reuse))) {
        Assert ($pair[1].saved -and $pair[1].message -match 'without naming its contact') "$($pair[0]) must save the report and note the missing contact: $($pair[1].message)"
    }
    Assert ($manual.blocked -and $manual.message -match 'without naming its contact') "the manual check must demand the named contact: $($manual.message)"
    $script:teamContacts = @{}
    Remove-Item -LiteralPath (Get-VmrQBContactsPath) -Force
    $findings = @(Invoke-QBOverallReportGate -Content (New-Overall @((New-Row 'Gamma failure' 3))) -Date $date -HistoryPath (Join-Path $sandbox 'no-such-history.json'))
    Assert (@($findings | Where-Object severity -eq 'fail').Count -eq 0 -and @($findings | Where-Object { $_.severity -eq 'warn' -and $_.check -eq 'history' }).Count -eq 1) 'a missing history store is an advisory warning, never a block'
}

Check 'The run notes block at the top of a saved report is never checked' {
    $clean = New-Overall @((New-Row 'Gamma failure' 3))
    Clear-QBRunNotes
    Add-QBRunNote -Source 'owners' -Message "Owner '@dotnet/not-a-team' for finding 'x' is not a team in the Guide."
    $withNotes = Set-QBRunNotesBlock -Content $clean
    Assert ($withNotes -match 'VMR-QB-RUN-NOTES' -and $withNotes -match 'not-a-team') 'the block is in the saved report'
    $findings = @(Invoke-QBOverallReportGate -Content $withNotes -Date $date -HistoryPath $paths.HistoryPath -BuildHealthActions $null)
    Assert (-not (@($findings | Where-Object { $_.message -match 'not-a-team' }).Count)) 'a handle named in a note must not be raised by the report checks'
    Clear-QBRunNotes
}
if ($null -eq $originalHome) { Remove-Item Env:\VMR_QB_HOME -ErrorAction SilentlyContinue } else { $env:VMR_QB_HOME = $originalHome }
Remove-Item -LiteralPath $sandbox -Recurse -Force -ErrorAction SilentlyContinue
if ($failures) { Write-Host "$failures gate-parity test(s) failed." -ForegroundColor Red; exit 1 }
Write-Host 'All gate-parity tests passed.' -ForegroundColor Green
