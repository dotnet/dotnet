<#
.SYNOPSIS
    Triage helpers that answer a question mechanically instead of by reading a log by eye.

.DESCRIPTION
    ./triage/Invoke-Triage.ps1 timeout -LogPath <build log> [-TimeLimitMinutes <the job's own limit>] [-Json]

      timeout   Was the leg really a timeout, and which repo was running when time ran out?
                Reads the "Building <repo>" / "Building <repo>...done" markers. A repo that merely logged an
                error is never named as the cause, and a truncated (.partial) log concludes nothing.

    timeout is the only action this entry exposes. Issue and owner evidence is collected by the daily round
    (daily/Invoke-Daily.ps1, through triage/issue.ps1), and whether a finding is a duplicate is
    ./history/Invoke-History.ps1 duplicates.

    Run it from the skill's scripts/ folder. The classification rules it supports are in references/triage.md.
#>
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot 'timeout.ps1')

function Invoke-QBTriage {
    [CmdletBinding()]
    param(
        [Parameter(Position = 0)][ValidateSet('timeout')][string] $Action = 'timeout',
        # The root build log (a .partial file is treated as truncated).
        [Parameter(Mandatory)][string] $LogPath,
        [ValidateRange(1, 1440)][int] $TimeLimitMinutes = 150,
        [switch] $Json
    )
    $state = Get-VmrQBRepoBuildState -Path $LogPath
    # A limit the caller did not state is the default, not the job's own: say so rather than assert a different cause.
    $result = Get-VmrQBTimeoutAttribution -State $state -TimeLimitMinutes $TimeLimitMinutes -LimitIsAssumed:(-not $PSBoundParameters.ContainsKey('TimeLimitMinutes'))
    if ($Json) { $result | ConvertTo-Json -Depth 6; return }
    $result | Format-List statement, repoAtTimeout, bystanders, errorOnlyRepos, reliable, uncertain, limitAssumed
}

Invoke-QBTriage @args
