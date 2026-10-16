<#
.SYNOPSIS
    Optional first-use setup: import a handoff file, build the team roster now, or write your email address. Nothing has to be set up first; every command works on a fresh clone.
.DESCRIPTION
    Run it from the skill's scripts/ folder. Options are the parameters of the function below; run Get-Help on the
    function after dot-sourcing, or read its comment block here. Settings are optional (local/qb-config.local.json).
#>
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot '..\history\history.ps1')

function Invoke-QBSetup {
    <#
    .SYNOPSIS
    Optional. Nothing has to be set up: every command works as soon as the skill is cloned. Use this to import a
    handoff file from the previous QB, to build the team roster now, or to put your email address in local/qb-config.local.json.
    .PARAMETER Recipient
    Your own email address: where the reports are emailed, and the Outlook account that sends them. Several addresses may be separated by ';' to send to more people; at least one must be your own. Written to local/qb-config.local.json (copying qb-config.template.json and editing it does the same).
    .PARAMETER ImportHandoff
    A handoff file from the previous QB (or a plain history export). Import merges; it never discards
    anything you recorded.
    #>
    [CmdletBinding()]
    param([string] $Recipient, [Alias('ImportHistory')][string] $ImportHandoff)

    $paths = (Initialize-VmrQBLocal).Paths
    Write-Host 'VMR QB' -ForegroundColor Cyan
    Write-Host "  local folder: $($paths.StateRoot)  (git-ignored)"
    if ($Recipient) {
        Set-VmrQBConfig -Recipient $Recipient -Path $paths.ConfigPath | Out-Null
        Write-Host "  wrote your email address to $($paths.ConfigPath) (git-ignored)." -ForegroundColor Green
    }
    $config = Get-VmrQBConfig -Path $paths.ConfigPath
    if ([string]::IsNullOrWhiteSpace([string]$config.recipient)) {
        Write-Host "  email: off (no recipient). Reports are saved to $($paths.ReportsDir). To get them by email, copy qb-config.template.json to $($paths.ConfigPath) and set `"recipient`" (see README.md)." -ForegroundColor DarkGray
    }
    else { Write-Host "  email: reports go to $($config.recipient)" -ForegroundColor Green }

    Initialize-VmrIssueStore -Path $paths.HistoryPath | Out-Null
    Write-Host "  history store: $($paths.HistoryPath) (schema $(Get-VmrIssueStoreSchemaVersion -Path $paths.HistoryPath))" -ForegroundColor Green
    if (Test-Path -LiteralPath $paths.LegacyDatabasePath) {
        Write-Warning ("A legacy SQLite history exists at $($paths.LegacyDatabasePath). This version stores history as JSON and cannot read it. " +
            'With the previous version export it to JSON, then re-run this with -ImportHandoff <that file>.')
    }

    $waiting = @(if (-not $ImportHandoff -and (Test-Path -LiteralPath $paths.HandoffDir)) { Get-ChildItem -LiteralPath $paths.HandoffDir -Filter *.json -File })
    foreach ($file in $waiting) { Write-Host "  handoff file found: $($file.FullName)" -ForegroundColor DarkGray }
    if ($waiting.Count) { Write-Host '    To start with their history: ./common/Invoke-Setup.ps1 -ImportHandoff <file>' -ForegroundColor DarkGray }

    if ($ImportHandoff) {
        $result = Import-VmrQBHandoff -InputPath $ImportHandoff -HistoryPath $paths.HistoryPath
        Write-Host "  imported handoff: $(Format-QBHandoffSummary $result)" -ForegroundColor Green
    }

    if (Test-VmrQBContactsStale) {
        Write-Host '  building the team contact roster from the VMR QB guide and the GitHub teams (once; rebuilt only when a pulled guide adds a team)...' -ForegroundColor DarkGray
        try { Write-QBContactsOutcome (Update-VmrQBContacts) -Brief }
        catch { Write-Warning "Team roster not built: $($_.Exception.Message)  Run ./common/Invoke-Contacts.ps1 once GitHub access works; reports ping teams without naming a person until then." }
    }
    else { Write-Host "  team roster: up to date ($(Get-VmrQBContactsPath))" -ForegroundColor Green }
}

Invoke-QBSetup @args
