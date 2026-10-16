<#
.SYNOPSIS
    Build the team contact roster once (-Refresh re-queries GitHub).
.DESCRIPTION
    Run it from the skill's scripts/ folder. Options are the parameters of the function below; run Get-Help on the
    function after dot-sourcing, or read its comment block here. Settings are optional (local/qb-config.local.json).
#>
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot 'core.ps1')

function Invoke-QBContacts {
    <#
    .SYNOPSIS
    Builds the team contact roster once and reuses it. Reads the Repo Ownership table in docs/VMR-QB-Guide.md,
    opens each team's GitHub team, reads its maintainers and picks the most recently active one. The result is
    cached in the local folder; entries a person edited are kept.
    .PARAMETER GuidePath
    Read the team table from this guide instead of docs/VMR-QB-Guide.md in the checkout. The guide is never fetched; it changes when you git pull.
    .PARAMETER Refresh
    Re-query GitHub for every generated entry. Without it, GitHub is asked only about teams the roster does not have yet.
    .PARAMETER Force
    Also replace entries a person wrote or edited.
    #>
    [CmdletBinding()]
    param([string] $GuidePath, [switch] $Refresh, [switch] $Force, [switch] $Preview, [switch] $Json)
    $result = Update-VmrQBContacts -GuidePath $GuidePath -Refresh:$Refresh -Force:$Force -Preview:$Preview
    if ($Json) { $result | ConvertTo-Json -Depth 8 } else { Write-QBContactsOutcome $result }
    if ($result.Missing.Count -or $result.AuthProblem) { exit 1 }
}

Invoke-QBContacts @args
