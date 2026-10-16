<#
.SYNOPSIS
    Preflight: will a daily run be able to see what it needs to see? Run this first each day. Authentication problems are printed first.
    Only PowerShell, the state folder, the GitHub sign-in and a config holding your email address that Git tracks or does not ignore
    can stop a run (exit code 2). Every other problem is a warning that the run turns into a note at the top of its report, so the
    round can still produce a report.
.DESCRIPTION
    Run it from the skill's scripts/ folder. Options are the parameters of the function below; run Get-Help on the
    function after dot-sourcing, or read its comment block here. Settings are optional (local/qb-config.local.json).
#>
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot 'preflight.ps1')

function Invoke-QBCheck {
    <#
    .PARAMETER Scope
    What you intend to do. Only checks that serve these scopes can block.
    #>
    [CmdletBinding()]
    param(
        [ValidateSet('Collection', 'Synthesis', 'Sending')][string[]] $Scope = @('Collection', 'Synthesis', 'Sending'),
        [switch] $SkipNetwork, [switch] $SkipOutlook, [switch] $Json
    )
    # With no recipient configured nothing is emailed, so sending is not part of the default check. A config that cannot be read counts
    # as no recipient here; the config check below says what is wrong with it.
    if (-not $PSBoundParameters.ContainsKey('Scope') -and [string]::IsNullOrWhiteSpace([string](Get-VmrQBConfigLenient).Config.recipient)) { $Scope = @('Collection', 'Synthesis') }
    $result = Invoke-VmrQBPreflight -Scope $Scope -SkipNetwork:$SkipNetwork -SkipOutlook:$SkipOutlook
    if ($Json) { $result | ConvertTo-Json -Depth 6 }
    else {
        Write-Host "VMR QB preflight (scope: $($Scope -join ', '))" -ForegroundColor Cyan
        if ($result.authErrors.Count) {
            Write-Host "`nAUTHENTICATION - fix these first" -ForegroundColor Red
            foreach ($check in $result.authErrors) {
                Write-Host ("  {0}: {1}" -f $check.name, $check.detail) -ForegroundColor Red
                if ($check.remedy) { Write-Host ("      -> {0}" -f $check.remedy) -ForegroundColor Yellow }
            }
        }
        Write-Host ''
        foreach ($check in $result.checks) {
            $mark = switch ($check.status) { 'ok' { '[ ok ]' } 'warn' { '[warn]' } 'fail' { '[FAIL]' } default { '[skip]' } }
            $color = switch ($check.status) { 'ok' { 'Green' } 'warn' { 'Yellow' } 'fail' { 'Red' } default { 'DarkGray' } }
            Write-Host ("{0} {1}{2}" -f $mark, $check.name, $(if ($check.detail) { " - $($check.detail)" } else { '' })) -ForegroundColor $color
            if ($check.status -in 'fail', 'warn' -and $check.remedy -and $check -notin $result.authErrors) { Write-Host ("       -> {0}" -f $check.remedy) -ForegroundColor Yellow }
        }
    }
    # Machine-readable mode must stay machine-readable: nothing but the JSON on stdout.
    if (-not $result.ok) {
        if (-not $Json) { Write-Host "`n$($result.blocking.Count) blocking problem(s): the round would not start (exit code 2)." -ForegroundColor Red }
        exit 2
    }
    if (-not $Json) { Write-Host "`nNothing stops the round.$(if ($result.warnings.Count) { " $($result.warnings.Count) warning(s) would become notes at the top of the report." })" -ForegroundColor Green }
}

Invoke-QBCheck @args
