<#
.SYNOPSIS
    Record what an owner told you, in one validated step.
.DESCRIPTION
    Run it from the skill's scripts/ folder. Options are the parameters of the function below; run Get-Help on the
    function after dot-sourcing, or read its comment block here. Settings are optional (local/qb-config.local.json).
#>
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot 'history.ps1')

function Invoke-QBRecord {
    <#
    .SYNOPSIS
    Records an owner's statement: history store (status, owner, tracker, audit note), the "links the QB
    reported" column and, with -AddContext, the model's context. Nothing is written unless everything validates.
    .EXAMPLE
    ./history/Invoke-Record.ps1 -Finding 'Example finding name' -Status owned -Owner '@dotnet/dnceng' -SaidBy 'Alex' `
        -Note 'Alex says it is a feed outage and will check.' -Builds 9000001 -Match 'Example' -WhatIf
    #>
    [CmdletBinding(SupportsShouldProcess)]
    param(
        # The finding's name or slug as history knows it. Typos are refused, never turned into a new finding.
        [Parameter(Mandatory)][string] $Finding,
        [Parameter(Mandatory)][ValidateSet('open', 'routed', 'owned', 'resolved', 'regressed', 'noise', 'wontfix')][string] $Status,
        # What was said, as close to the speaker's words as you can.
        [Parameter(Mandatory)][string] $Note,
        [string] $SaidBy,
        # A team from knowledge/routing.json, or a person (@login).
        [string] $Owner,
        [string] $Tracker,
        [int[]] $Builds = @(),
        # Regex for the report's Issue category; required with -Builds so the note lands on the right row only.
        [string] $Match,
        [string] $LinkText,
        [switch] $AddContext,
        [string] $AppliesTo,
        [switch] $Create,
        [switch] $AllowUnknownOwner,
        [string] $On = (Get-Date -Format 'yyyy-MM-dd')
    )
    $arguments = @{} + $PSBoundParameters
    $result = Set-VmrQBOperatorUpdate @arguments
    # The update returns an object, not a dictionary: read optional fields through the shared helper.
    $contextPath = Get-VmrQBField $result 'contextPath'
    $verb = if (Get-VmrQBField $result 'applied' $false) { 'Recorded' } else { 'Would record (nothing was written)' }
    Write-Host "$verb '$($result.status)' for '$($result.finding)' on $($result.on)." -ForegroundColor Green
    if ($result.owner) { Write-Host "  owner: $($result.owner)" }
    if ($result.tracker) { Write-Host "  tracker: $($result.tracker)" }
    Write-Host "  history: $($result.historyPath)"
    if ($result.links) { Write-Host "  links:   $($result.linksPath)" }
    if ($result.context -and $contextPath) { Write-Host "  context: $contextPath" }
    $result
}

Invoke-QBRecord @args
