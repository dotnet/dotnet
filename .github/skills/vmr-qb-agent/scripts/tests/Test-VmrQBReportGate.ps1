[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot '..\common\core.ps1')
. (Join-Path $PSScriptRoot '..\history\history.ps1')
. (Join-Path $PSScriptRoot '..\daily\gate.ps1')

$failures = 0
function Check([string] $Name, [scriptblock] $Body) {
    try { & $Body; Write-Host "PASS: $Name" -ForegroundColor Green }
    catch { $script:failures++; Write-Host "FAIL: $Name -> $($_.Exception.Message)" -ForegroundColor Red }
}
function Assert($Condition, [string] $Message) { if (-not $Condition) { throw $Message } }

$sandbox = Join-Path ([IO.Path]::GetTempPath()) ('vmrqb-gate-' + [guid]::NewGuid().ToString('N'))
[void][IO.Directory]::CreateDirectory($sandbox)
$env:VMR_QB_HOME = Join-Path $sandbox 'state'

$header = '| Issue category | Builds / PRs (links) | New Issue (Y/N) | Existing fix PR (status) | Action as VMR QB (who / where to ping) | Links that QB reported |'
$sep = '|---|---|---|---|---|---|'
function Build-Link([int] $Id) { "[$Id](https://dev.azure.com/dnceng/internal/_build/results?buildId=$Id)" }
function Row([string] $Category, [int] $Build, [string] $New = 'N', [string] $Action = $null) {
    if (-not $Action) { $Action = "Where: Teams - Unified Build Technical and General Discussions.<br>Copy/paste: ""@dotnet/dnceng @Some One, Please check the build failure. $Category failed: $(Build-Link $Build)""" }
    "| $Category | $(Build-Link $Build) | $New | None found | $Action | None found |"
}
function Report {
    param([string[]] $Top, [string[]] $Main, [string[]] $Monitor = @(), [string] $Date = '2026-10-07', [switch] $NoAppendix)
    $text = @("# Overall QB Summary - $Date", '', '## Top actions for the QB today', '') + $Top + @('', '## Summary', '', $header, $sep) + $Main
    $text += @('', '## Monitor only - no QB action needed', '', $header, $sep) + $Monitor
    if (-not $NoAppendix) { $text += @('', '## Internal issue tracking and action drafts', '', 'x') }
    return ($text -join "`n")
}
function Top([int] $N, [string] $Text, [int] $Build) { "$N. $(Build-Link $Build) - **BH #${N}:** $Text Where: Teams - Unified Build Technical and General Discussions." }

$goodTop = @((Top 1 'Arcade SDK package unavailable on Windows.' 100), (Top 2 'macOS socket exhaustion lost an MSBuild node.' 101))
$goodMain = @((Row 'BH #1 - Arcade SDK package unavailable' 100), (Row 'BH #2 - macOS socket exhaustion' 101))

Check 'A consistent report has no objective failures' {
    $f = @(Invoke-VmrQBReportGate -Content (Report $goodTop $goodMain))
    $bad = @($f | Where-Object severity -eq 'fail')
    Assert ($bad.Count -eq 0) "unexpected: $(($bad | ForEach-Object { "$($_.check): $($_.message)" }) -join ' | ')"
}

Check 'A copy/paste ping to a team that does not exist fails, with the real team suggested' {
    $bad = Row 'BH #1 - Crossgen2 Text file busy' 100 'N' 'Where: GitHub - [dotnet/dotnet#101](https://github.com/dotnet/dotnet/issues/101).<br>Copy/paste: "@dotnet/runtime, Please check the build failure. crossgen2 failed: [100](https://dev.azure.com/dnceng/internal/_build/results?buildId=100)"'
    $f = @(Invoke-VmrQBReportGate -Content (Report @((Top 1 'Crossgen2 Text file busy.' 100)) @($bad)) | Where-Object check -eq 'handle')
    Assert ($f.Count -ge 1 -and $f[0].severity -eq 'fail') 'an unknown handle must fail'
    Assert ($f[0].message -match '@dotnet/dnr-codeflow') "the suggestion must name the real team: $($f[0].message)"
    $broad = Row 'BH #1 - Roslyn analyzer' 100 'N' 'Where: Teams - Unified Build Technical and General Discussions.<br>Copy/paste: "@dotnet/roslyn @Fred, Please check the build failure. x: [100](https://dev.azure.com/dnceng/internal/_build/results?buildId=100)"'
    $w = @(Invoke-VmrQBReportGate -Content (Report @((Top 1 'Roslyn analyzer.' 100)) @($broad)) | Where-Object check -eq 'handle')
    Assert ($w.Count -ge 1 -and $w[0].severity -eq 'warn') 'a real-but-broad team is advisory, not a failure'
}

Check 'First Responders is accepted for Helix and rejected for anything else' {
    $fr = 'Where: Teams - First Responders.<br>Copy/paste: "First Responders, Please check the build failure. x: [100](https://dev.azure.com/dnceng/internal/_build/results?buildId=100)"'
    $nonHelix = Row 'BH #1 - 1ES binary analysis task failed' 100 'N' $fr
    $f = @(Invoke-VmrQBReportGate -Content (Report @((Top 1 '1ES binary analysis task failed.' 100)) @($nonHelix)) | Where-Object check -eq 'first-responders')
    Assert ($f.Count -eq 1 -and $f[0].severity -eq 'fail' -and $f[0].message -match 'Helix-only') "non-Helix must fail: $($f | ConvertTo-Json -Compress)"
    $helix = Row 'BH #1 - Helix work item timeout' 100 'N' $fr
    $ok = @(Invoke-VmrQBReportGate -Content (Report @((Top 1 'Helix work item timeout.' 100)) @($helix)) | Where-Object check -eq 'first-responders')
    Assert ($ok.Count -eq 0) 'a Helix failure may go to First Responders'
    $topOnly = @(Invoke-VmrQBReportGate -Content (Report @("1. $(Build-Link 100) - **BH #1:** Signing failed. Where: Teams - First Responders.") @((Row 'BH #1 - Signing failed' 100))) | Where-Object check -eq 'first-responders')
    Assert ($topOnly.Count -eq 1) 'a top action routed to First Responders without Helix must also fail'
}

Check 'Numbering: permuted numbers, two numbers for one finding, and a missing row all fail' {
    $swappedMain = @((Row 'BH #2 - Arcade SDK package unavailable' 100), (Row 'BH #1 - macOS socket exhaustion' 101))
    $f = @(Invoke-VmrQBReportGate -Content (Report $goodTop $swappedMain) | Where-Object check -eq 'numbering')
    Assert ($f.Count -ge 2) "a permutation must be caught (the 2026-10-01 report had one): got $($f.Count)"
    $twoNumbers = @((Row 'BH #1 - Arcade SDK package unavailable' 100), (Row 'BH #3 - Arcade SDK package unavailable' 101), (Row 'BH #2 - macOS socket exhaustion' 102))
    $f = @(Invoke-VmrQBReportGate -Content (Report $goodTop $twoNumbers) | Where-Object check -eq 'numbering')
    Assert (@($f | Where-Object { $_.message -match 'One finding, one number' }).Count -eq 1) 'one finding with two numbers must fail'
    $f = @(Invoke-VmrQBReportGate -Content (Report $goodTop @((Row 'BH #1 - Arcade SDK package unavailable' 100))) | Where-Object check -eq 'numbering')
    Assert (@($f | Where-Object { $_.message -match 'no row numbered BH #2' }).Count -eq 1) 'a top action without a row must fail'
}

Check 'Numbering: BH numbers beyond the Build Health action list fail when it is supplied' {
    $f = @(Invoke-VmrQBReportGate -Content (Report $goodTop $goodMain) -BuildHealthActions @([pscustomobject]@{ n = 1 }) | Where-Object check -eq 'numbering')
    Assert (@($f | Where-Object { $_.message -match 'BH #2 does not exist' }).Count -eq 1) "BH #2 is not in a 1-item list: $($f.message -join ' | ')"
}

Check 'Leftover placeholders fail, but the same words inside code spans do not' {
    $f = @(Invoke-VmrQBReportGate -Content (Report @((Top 1 'Arcade SDK package unavailable. Owner: <team>.' 100)) @((Row 'BH #1 - Arcade SDK package unavailable' 100))) | Where-Object check -eq 'placeholder')
    Assert ($f.Count -eq 1) 'a <team> placeholder must fail'
    $ok = @(Invoke-VmrQBReportGate -Content (Report @((Top 1 'Arcade SDK package unavailable; see `<team>` in the docs.' 100)) @((Row 'BH #1 - Arcade SDK package unavailable' 100))) | Where-Object check -eq 'placeholder')
    Assert ($ok.Count -eq 0) 'code spans are documentation, not placeholders'
    $todo = @(Invoke-VmrQBReportGate -Content (Report @((Top 1 'Arcade SDK package unavailable. TODO confirm.' 100)) @((Row 'BH #1 - Arcade SDK package unavailable' 100))) | Where-Object check -eq 'placeholder')
    Assert ($todo.Count -eq 1) 'TODO must fail'
}

Check 'Structure: missing sections and a wrong title date are reported' {
    $f = @(Invoke-VmrQBReportGate -Content "# Overall QB Summary - 2026-10-06`n`nnothing here" -ReportDate '2026-10-07')
    Assert (@($f | Where-Object { $_.check -eq 'structure' -and $_.message -match 'Top actions' }).Count -eq 1) 'missing top actions'
    Assert (@($f | Where-Object { $_.check -eq 'structure' -and $_.message -match 'six-column' }).Count -eq 1) 'missing table'
    Assert (@($f | Where-Object { $_.check -eq 'structure' -and $_.message -match '2026-10-06 but this is the 2026-10-07' }).Count -eq 1) 'wrong title date'
    $noAppendix = @(Invoke-VmrQBReportGate -Content (Report $goodTop $goodMain -NoAppendix) | Where-Object { $_.message -match 'appendix' })
    Assert ($noAppendix.Count -eq 1 -and $noAppendix[0].severity -eq 'warn') 'a missing appendix is advisory'
}

Check 'The format validator hook turns a throw into a failure finding' {
    $f = @(Invoke-VmrQBReportGate -Content (Report $goodTop $goodMain) -ContractValidator { param($t) throw 'Every overall summary row must retain all six columns.' } | Where-Object check -eq 'format')
    Assert ($f.Count -eq 1 -and $f[0].message -match 'six columns') 'the validator message must be preserved'
}

Check 'Rows are parsed per section, with escaped pipes kept inside their cell' {
    $piped = '| BH #1 - Arcade SDK package unavailable | ' + (Build-Link 100) + ' | N | None found | Where: None - monitor only.<br>Action: Owner reports a \| in text. | None found |'
    $content = Report $goodTop @($piped) @((Row 'BH #2 - macOS socket exhaustion' 101))
    $rows = @(Get-VmrQBReportRows -Content $content)
    Assert ($rows.Count -eq 2) "expected 2 rows, got $($rows.Count)"
    Assert ($rows[0].section -eq 'main' -and $rows[1].section -eq 'monitor') 'sections wrong'
    Assert ($rows[0].action -match '\\\| in text') 'the escaped pipe must stay in the action cell'
}

Check 'History: novelty claims and table placement are checked against the store' {
    $history = Join-Path $sandbox 'issue-history.json'
    Initialize-VmrIssueStore -Path $history | Out-Null
    Add-VmrIssueOccurrence -Path $history -Name 'Arcade SDK package unavailable' -ReportDate '2026-10-05' -BuildIds @('1') | Out-Null
    Add-VmrIssueOccurrence -Path $history -Name 'Arcade SDK package unavailable' -ReportDate '2026-10-07' -BuildIds @('100') | Out-Null
    Add-VmrIssueOccurrence -Path $history -Name 'macOS socket exhaustion' -ReportDate '2026-10-06' -BuildIds @('99') | Out-Null
    Add-VmrIssueOccurrence -Path $history -Name 'macOS socket exhaustion' -ReportDate '2026-10-07' -BuildIds @('101') | Out-Null
    Set-VmrIssueStatus -Path $history -Name 'macOS socket exhaustion' -Status owned -OwnerTeam '@dotnet/msbuild' -Note 'The msbuild team owns this one.' -On '2026-10-07' | Out-Null
    $claimsNew = @((Row 'BH #1 - Arcade SDK package unavailable' 100 'Y'), (Row 'BH #2 - macOS socket exhaustion' 101 'N'))
    $f = @(Invoke-VmrQBReportGate -Content (Report $goodTop $claimsNew) -HistoryPath $history -ReportDate '2026-10-07')
    $novelty = @($f | Where-Object { $_.check -eq 'novelty' })
    Assert ($novelty.Count -eq 1 -and $novelty[0].severity -eq 'fail' -and $novelty[0].message -match 'first saw it on 2026-10-05') "Y for a known finding must fail: $($novelty | ConvertTo-Json -Compress)"
    $placement = @($f | Where-Object { $_.check -eq 'placement' })
    Assert ($placement.Count -eq 1 -and $placement[0].message -match 'recorded as ''owned''') "an owned finding in the main table must fail: $($placement | ConvertTo-Json -Compress)"
    # N for something history has never seen is only advisory (it may have been renamed).
    $unknownN = @(Invoke-VmrQBReportGate -Content (Report @((Top 1 'Brand new failure appeared.' 100)) @((Row 'BH #1 - Brand new failure appeared' 100 'N'))) -HistoryPath $history -ReportDate '2026-10-07' | Where-Object check -eq 'novelty')
    Assert ($unknownN.Count -eq 1 -and $unknownN[0].severity -eq 'warn') 'N without a record is a warning'
    # A missing store must say so, not crash.
    $none = @(Invoke-VmrQBReportGate -Content (Report $goodTop $goodMain) -HistoryPath (Join-Path $sandbox 'nope.json') -ReportDate '2026-10-07' | Where-Object check -eq 'history')
    Assert ($none.Count -eq 1 -and $none[0].severity -eq 'warn') 'a missing store is advisory'
}

Remove-Item $sandbox -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item Env:\VMR_QB_HOME -ErrorAction SilentlyContinue
if ($failures) { Write-Host "$failures report-gate test(s) failed." -ForegroundColor Red; exit 1 }
Write-Host '10 offline report-gate tests. Synthetic reports and temporary stores only; no network, email, or external changes.' -ForegroundColor Green
