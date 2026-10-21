<#
.SYNOPSIS
    Preflight: will a daily run be able to see what it needs to see? Run it first. Authentication problems print first.
.DESCRIPTION
    Only PowerShell 7.3, a writable state folder, the GitHub sign-in and a config holding your email address that Git tracks or
    does not ignore can stop a run (exit code 2). Everything else is a warning that the run turns into a note at the top of its
    report. With no sender configured, sending is not part of the default check.
#>
[CmdletBinding()]
param(
    [ValidateSet('Collection', 'Synthesis', 'Sending')][string[]] $Scope = @('Collection', 'Synthesis', 'Sending'),
    [switch] $SkipNetwork
)
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot 'preflight.ps1')

if (-not $PSBoundParameters.ContainsKey('Scope') -and [string]::IsNullOrWhiteSpace([string](Get-VmrQBConfigLenient).Config.sender)) { $Scope = @('Collection', 'Synthesis') }
$result = Invoke-VmrQBPreflight -Scope $Scope -SkipNetwork:$SkipNetwork

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
if (-not $result.ok) {
    Write-Host "`n$($result.blocking.Count) blocking problem(s): the round would not start (exit code 2)." -ForegroundColor Red
    exit 2
}
Write-Host "`nNothing stops the round.$(if ($result.warnings.Count) { " $($result.warnings.Count) warning(s) would become notes at the top of the report." })" -ForegroundColor Green
