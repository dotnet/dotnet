<#
.SYNOPSIS
    Maintainer checks before publishing: publish readiness, knowledge pack, generated docs.
.DESCRIPTION
    Run it from the skill's scripts/ folder. Options are the parameters of the function below; run Get-Help on the
    function after dot-sourcing, or read its comment block here. Settings are optional (local/qb-config.local.json).
#>
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot '..\common\core.ps1')
. (Join-Path $PSScriptRoot 'knowledge.ps1')
. (Join-Path $PSScriptRoot 'publish.ps1')

function Invoke-QBVerify {
    <#
    .SYNOPSIS
    Is this skill fit to publish? Runs the publish-readiness gate (binaries, secrets, session-specific data,
    state beside the scripts, dangling references), the knowledge-pack check, and confirms the generated
    routing tables in the docs match knowledge/routing.json. Exit 1 on any FAIL.
    .PARAMETER Online
    Also confirm every team handle resolves and every linked issue/PR is in the state the knowledge says.
    .PARAMETER SyncDocs
    Regenerate the routing tables in the docs instead of only checking them.
    .PARAMETER Root
    The skills directory to scan. Default: this skill's parent; only this skill's folder is scanned unless -SkillFolder says otherwise.
    #>
    [CmdletBinding()]
    param([switch] $Online, [switch] $SyncDocs, [int] $MaxAgeDays = 30, [int] $MaxFileKB = 256, [int] $MaxTotalKB = 1536,
        [string] $Root, [string[]] $SkillFolder, [switch] $Json)
    $paths = Get-VmrQBPaths
    if (-not $Root) {
        # The publish gate scans folders beneath a skills directory, so the default root is this skill's PARENT.
        $Root = Split-Path -Parent $paths.SkillRoot
        if (-not $SkillFolder) { $SkillFolder = @(Split-Path -Leaf $paths.SkillRoot) }
    }
    $publish = @(Invoke-VmrQBPublishReadiness -Root $Root -MaxFileKB $MaxFileKB -MaxTotalKB $MaxTotalKB -SkillFolder $SkillFolder)
    $knowledge = @(Invoke-VmrQBKnowledgeCheck -Online:$Online -MaxAgeDays $MaxAgeDays)

    $docs = @()
    $routingBlock = Get-VmrQBRoutingDocMarkdown
    foreach ($target in @(Join-Path $paths.ReferencesRoot 'triage.md')) {
        $inSync = Sync-VmrQBGeneratedBlock -Path $target -Name 'routing' -Content $routingBlock -Check:(-not $SyncDocs)
        if ($SyncDocs) { $docs += [pscustomobject]@{ severity = 'info'; check = 'docs'; subject = $target; message = 'Regenerated the routing block.' } }
        elseif (-not $inSync) { $docs += [pscustomobject]@{ severity = 'fail'; check = 'docs'; subject = $target; message = 'Out of sync with knowledge/routing.json. Run: ./verify/Invoke-Verify.ps1 -SyncDocs' } }
    }
    $findings = @($publish | ForEach-Object { $_ | Add-Member -NotePropertyName area -NotePropertyValue 'publish' -PassThru -Force }) +
        @($knowledge | ForEach-Object { $_ | Add-Member -NotePropertyName area -NotePropertyValue 'knowledge' -PassThru -Force }) +
        @($docs | ForEach-Object { $_ | Add-Member -NotePropertyName area -NotePropertyValue 'docs' -PassThru -Force })
    if ($Json) { $findings | ConvertTo-Json -Depth 5 }
    else {
        $color = @{ fail = 'Red'; warn = 'Yellow'; info = 'Cyan' }
        Write-Host "Skill verification ($(if ($Online) { 'online' } else { 'offline' })) for $Root" -ForegroundColor Cyan
        foreach ($group in ($findings | Group-Object severity | Sort-Object { @('fail', 'warn', 'info').IndexOf($_.Name) })) {
            Write-Host ("`n{0} ({1})" -f $group.Name.ToUpperInvariant(), $group.Count) -ForegroundColor $color[$group.Name]
            foreach ($finding in $group.Group) {
                $where = if ((Get-VmrQBField $finding 'line')) { "$(Get-VmrQBField $finding 'path'):$($finding.line)" } elseif ((Get-VmrQBField $finding 'path')) { $finding.path } else { Get-VmrQBField $finding 'subject' '' }
                Write-Host ("  [{0}/{1}] {2} - {3}" -f $finding.area, $finding.check, $where, $finding.message)
            }
        }
        if (-not $findings) { Write-Host 'No findings.' -ForegroundColor Green }
    }
    $failed = @($findings | Where-Object severity -eq 'fail').Count
    if ($failed) { if (-not $Json) { Write-Host "`n$failed blocking finding(s). Do not publish." -ForegroundColor Red }; exit 1 }
    if (-not $Json) { Write-Host "`nNo blocking findings." -ForegroundColor Green }
}

Invoke-QBVerify @args
