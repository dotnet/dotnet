<#
.SYNOPSIS
    Validate a finished overall report against the contract and the history store.
.DESCRIPTION
    Run it from the skill's scripts/ folder. Options are the parameters of the function below; run Get-Help on the
    function after dot-sourcing, or read its comment block here. Settings are optional (local/qb-config.local.json).
#>
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot '..\history\history.ps1')
. (Join-Path $PSScriptRoot 'report.ps1')
. (Join-Path $PSScriptRoot 'gate.ps1')
. (Join-Path $PSScriptRoot 'contract.ps1')

function Invoke-QBReport {
    <#
    .SYNOPSIS
    Validates a finished overall report: the runner's format validator plus checks that were each a real
    defect in a delivered report. This manual check exits 1 on a FAIL finding and says the report should not be sent
    as it is; WARN findings are advisory. The daily round never withholds a report for these checks: it lists each
    finding as a note at the top of the report and the email.
    #>
    [CmdletBinding()]
    param(
        # The overall-summary-<date>.md to check.
        [Parameter(Mandatory)][string] $ReportPath,
        # build-health-<date>.md; lets the gate check that BH numbers exist in its visible action list.
        [string] $BuildHealthPath,
        [ValidatePattern('^\d{4}-\d{2}-\d{2}$')][string] $Date,
        [string] $HistoryPath,
        [switch] $NoHistory,
        # Skip the runner's strict format validator (links, destinations, 30-word limit, named contacts).
        [switch] $SkipFormat,
        [switch] $Json
    )
    if (-not (Test-Path -LiteralPath $ReportPath -PathType Leaf)) { throw "Report not found: $ReportPath" }
    $content = Get-Content -LiteralPath $ReportPath -Raw
    if (-not $Date) { $Date = if ((Split-Path -Leaf $ReportPath) -match '(\d{4}-\d{2}-\d{2})') { $Matches[1] } else { '' } }
    if (-not $Date) { throw 'The report date could not be read from the file name. Pass -Date yyyy-MM-dd: the gate judges novelty and the title against it.' }
    $paths = Get-VmrQBPaths
    if (-not $HistoryPath) { $HistoryPath = $paths.HistoryPath }

    # The contacts, the visible Build Health list, the history and the routing are the same inputs generation and reuse give the
    # one gate, so this check cannot disagree with them.
    $contacts = @{}
    $contactsFile = Get-VmrQBContactsPath
    if (Test-Path -LiteralPath $contactsFile) {
        foreach ($property in (Get-Content -LiteralPath $contactsFile -Raw | ConvertFrom-Json).contacts.PSObject.Properties) { $contacts[$property.Name] = $property.Value }
    }
    $buildHealthActions = $null
    if ($BuildHealthPath) {
        if (-not (Test-Path -LiteralPath $BuildHealthPath -PathType Leaf)) { throw "Build health report not found: $BuildHealthPath" }
        $buildHealthActions = Get-QBVisibleBuildHealthActions -BuildHealthPath $BuildHealthPath
    }
    $gateArguments = @{ Content = $content; Date = $Date; TeamContacts = $contacts; BuildHealthActions = $buildHealthActions; SkipFormat = $SkipFormat }
    if (-not $NoHistory) { $gateArguments.HistoryPath = $HistoryPath }
    $findings = @(Invoke-QBOverallReportGate @gateArguments)
    if ($Json) { $findings | ConvertTo-Json -Depth 4 }
    else {
        Write-Host "Report gate: $ReportPath" -ForegroundColor Cyan
        foreach ($group in ($findings | Group-Object severity | Sort-Object { @('fail', 'warn').IndexOf($_.Name) })) {
            $color = if ($group.Name -eq 'fail') { 'Red' } else { 'Yellow' }
            Write-Host ("`n{0} ({1})" -f $group.Name.ToUpperInvariant(), $group.Count) -ForegroundColor $color
            foreach ($finding in $group.Group) { Write-Host ("  [{0}]{1} {2}" -f $finding.check, $(if ($finding.line) { " line $($finding.line):" } else { '' }), $finding.message) }
        }
        if (-not $findings) { Write-Host 'No findings.' -ForegroundColor Green }
    }
    if (@($findings | Where-Object severity -eq 'fail').Count) {
        if (-not $Json) { Write-Host "`nThis report should not be sent as it is." -ForegroundColor Red }
        exit 1
    }
    if (-not $Json) { Write-Host "`nNo blocking findings." -ForegroundColor Green }
}

Invoke-QBReport @args
