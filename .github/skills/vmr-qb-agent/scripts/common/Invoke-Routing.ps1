<#
.SYNOPSIS
    Show, compare and edit YOUR routing copy (local/knowledge/routing.local.json), and take in a changed shipped file.
.DESCRIPTION
    ./common/Invoke-Routing.ps1 show
    ./common/Invoke-Routing.ps1 diff
    ./common/Invoke-Routing.ps1 set-category -Id <id> -Name <text> [-OwnerRepos <repo,...>] [-Owners <@dotnet/team,...>] [-Support <id>] [-Note <text>]
    ./common/Invoke-Routing.ps1 set-channel-rule -Channel <name> -Rule <text>
    ./common/Invoke-Routing.ps1 adopt -Section <section> -Key <key>      (or: adopt -All)
    ./common/Invoke-Routing.ps1 acknowledge
    ./common/Invoke-Routing.ps1 refresh-ownership [-FromGitHub]

    The copy is made once, on first use, from knowledge/routing.json and is never overwritten. Every edit here must leave a routing
    registry that loads; if it would not, nothing is written. -WhatIf previews any edit. -Json gives machine-readable output
    (an empty result is an empty list). This edits YOUR copy. To change what the skill ships, edit knowledge/routing.json and run
    ./verify/Invoke-Verify.ps1 -SyncDocs instead (see references/triage.md).

      show         The routing in effect: where the files are, whether the shipped file changed since you copied it, and the entries.
      diff         Your copy against the shipped file, one entry at a time, saying who changed each: yours / upstream / both.
      adopt        Take the shipped version of one entry (or -All upstream entries that you never changed) into your copy.
      acknowledge  "I have reviewed the differences": restamp your copy and refresh the base snapshot. Your edits are kept.
      refresh-ownership  Renew the saved copy of the Guide's Repo Ownership table (local/knowledge/guide-ownership.json). It is made the
                   first time the Guide is read and renewed on its own only when the Guide file changes after a pull, so this is only for
                   a copy you want renewed now. It reads the Guide of this checkout; -FromGitHub fetches it from GitHub main instead.

    Run it from the skill's scripts/ folder.
#>
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot 'core.ps1')
. (Join-Path $PSScriptRoot 'routing-edit.ps1')

function Invoke-QBRouting {
    [CmdletBinding(SupportsShouldProcess)]
    param(
        [Parameter(Position = 0)][ValidateSet('show', 'diff', 'set-category', 'set-channel-rule', 'adopt', 'acknowledge', 'refresh-ownership')][string] $Action = 'show',
        # set-category
        [string] $Id,
        [string] $Name,
        [string[]] $OwnerRepos,
        [string[]] $Owners,
        [string] $Support,
        [string] $Note,
        [string[]] $Links,
        # set-channel-rule
        [string] $Channel,
        [string] $Rule,
        # adopt
        [ValidateSet('categories', 'supportDestinations', 'channels', 'channelRules', 'preferredRedirects', 'handleAliases', 'repoOwners', 'teams', 'vmrTestsOwner', 'unmappedLabel', 'verifiedOn')][string] $Section,
        [string] $Key,
        [switch] $All,
        # refresh-ownership
        [switch] $FromGitHub,
        [switch] $Json
    )
    # Needs none of the routing files, so it works even when one of them is broken.
    if ($Action -eq 'refresh-ownership') {
        $renewed = Update-VmrQBGuideOwnershipCopy -FromGitHub:$FromGitHub
        if ($Json) { $renewed | ConvertTo-Json } else { Write-Host "Ownership copy renewed from $($renewed.Source): $($renewed.Rows) repo(s) in $($renewed.Path)." -ForegroundColor Green }
        return
    }
    $paths = Get-VmrQBPaths
    $shippedPath = Join-Path $paths.KnowledgeRoot 'routing.json'
    $localPath = $paths.LocalRoutingPath
    $basePath = $paths.RoutingBasePath
    # Reading creates your copy the first time, exactly as every other script does.
    [void](Get-VmrQBRouting)
    $shipped = Read-VmrQBRoutingFile -Path $shippedPath -Label 'Shipped'
    $local = Read-VmrQBRoutingFile -Path $localPath -Label 'Local'
    $base = if (Test-Path -LiteralPath $basePath -PathType Leaf) { Read-VmrQBRoutingFile -Path $basePath -Label 'Base' } else { $null }

    switch ($Action) {
        'show' {
            $routing = Get-VmrQBRouting
            $differences = @(Get-VmrQBRoutingDiff -Local $local -Shipped $shipped -Base $base)
            $summary = [ordered]@{
                shippedPath = $shippedPath; localPath = $localPath; hasBaseSnapshot = [bool]$base
                behind = [bool](Test-VmrQBLocalRoutingBehind)
                differences = $differences.Count
                categories = @($routing.Categories | ForEach-Object { [ordered]@{ id = $_.id; name = $_.name; owners = @($_.owners); support = [string](Get-VmrQBRoutingField $_ 'support') } })
                channels = $routing.Channels; channelRules = $routing.ChannelRules
                supportDestinations = @($routing.SupportDestinations | ForEach-Object { [ordered]@{ id = $_.Id; name = $_.Name; url = $_.Url } })
                unmappedLabel = $routing.UnmappedLabel
                guideSource = $routing.GuideSource; warnings = @($routing.Warnings)
            }
            if ($Json) { $summary | ConvertTo-Json -Depth 8; return }
            Write-Host "Shipped routing : $shippedPath"
            Write-Host "Your copy       : $localPath"
            if ($summary.behind) { Write-Host 'The shipped file changed since you copied it. Run: ./common/Invoke-Routing.ps1 diff' -ForegroundColor Yellow }
            Write-Host "Differences from the shipped file: $($differences.Count)$(if (-not $base) { ' (no base snapshot, so they cannot be attributed; acknowledge creates one)' })"
            foreach ($warning in @($routing.Warnings)) { Write-Host "Note: $warning" -ForegroundColor Yellow }
            Write-Host "`nCategories (owners come from the Guide for ownerRepos):"
            foreach ($category in $summary.categories) {
                $who = if ($category.support) { "support: $($category.support)" } elseif ($category.owners.Count) { $category.owners -join ', ' } else { $routing.UnmappedLabel }
                Write-Host ("  {0,-30} {1}" -f $category.id, $who)
            }
            Write-Host "`nUnmapped failures are labelled: $($routing.UnmappedLabel)"
        }
        'diff' {
            $differences = @(Get-VmrQBRoutingDiff -Local $local -Shipped $shipped -Base $base)
            if ($Json) { if ($differences.Count) { ConvertTo-Json -InputObject $differences -Depth 6 } else { '[]' }; return }
            if (-not $differences.Count) { Write-Host 'Your routing copy matches the shipped file.' -ForegroundColor Green; return }
            $meaning = @{
                yours = 'you changed it; kept'; upstream = 'only the shipped file changed; safe to adopt'; both = 'you and the shipped file both changed it; review'
                differs = 'differs (no base snapshot, so who changed it is unknown)'; onlyLocal = 'only in your copy'; onlyShipped = 'only in the shipped file'
            }
            foreach ($difference in $differences) {
                Write-Host ("{0,-9} {1} / {2}  - {3}" -f $difference.State, $difference.Section, $difference.Key, $meaning[$difference.State])
            }
            if (@($differences | Where-Object State -eq 'upstream').Count) { Write-Host "`nTake the upstream ones: ./common/Invoke-Routing.ps1 adopt -All" -ForegroundColor Cyan }
            Write-Host 'When you have decided about every entry: ./common/Invoke-Routing.ps1 acknowledge' -ForegroundColor Cyan
        }
        'set-category' {
            if (-not $Id -or -not $Name) { throw 'set-category needs -Id and -Name.' }
            if ($Id -notmatch '^[a-z0-9][a-z0-9-]*$') { throw "-Id must be lowercase letters, digits and hyphens, got '$Id'." }
            $entry = [ordered]@{ id = $Id; name = $Name }
            if ($OwnerRepos) { $entry.ownerRepos = @($OwnerRepos) }
            if ($Owners) { $entry.owners = @($Owners | ForEach-Object { Resolve-VmrQBOwnerTeam -Handle $_ } | ForEach-Object { if ($null -eq $_) { throw 'An owner is not a team that exists in the routing registry.' } $_ }) }
            if ($Support) { $entry.support = $Support }
            if ($Links) { $entry.links = @($Links) }
            if ($Note) { $entry.note = $Note }
            Set-VmrQBLocalRoutingEntry -Section categories -Key $Id -Value ([pscustomobject]$entry) -WhatIf:$WhatIfPreference
            if (-not $WhatIfPreference -and -not $Json) { Write-Host "Saved category '$Id' in $localPath." -ForegroundColor Green }
            if ($Json) { [ordered]@{ action = 'set-category'; id = $Id; saved = (-not $WhatIfPreference) } | ConvertTo-Json }
        }
        'set-channel-rule' {
            if (-not $Channel -or -not $Rule) { throw 'set-channel-rule needs -Channel and -Rule.' }
            $routing = Get-VmrQBRouting
            if (@($routing.Channels.Values) -notcontains $Channel) { throw "'$Channel' is not a channel in the registry. Channels: $(@($routing.Channels.Values) -join '; ')." }
            Set-VmrQBLocalRoutingEntry -Section channelRules -Key $Channel -Value $Rule -WhatIf:$WhatIfPreference
            if (-not $WhatIfPreference -and -not $Json) { Write-Host "Saved the rule for '$Channel' in $localPath." -ForegroundColor Green }
            if ($Json) { [ordered]@{ action = 'set-channel-rule'; channel = $Channel; saved = (-not $WhatIfPreference) } | ConvertTo-Json }
        }
        'adopt' {
            $differences = @(Get-VmrQBRoutingDiff -Local $local -Shipped $shipped -Base $base)
            if ($All) {
                if (-not $base) { throw 'adopt -All needs the base snapshot to know which differences are only upstream. Name entries with -Section and -Key, or run acknowledge to create a snapshot.' }
                $targets = @($differences | Where-Object State -eq 'upstream')
            }
            elseif ($Section -and $Key) { $targets = @($differences | Where-Object { $_.Section -eq $Section -and $_.Key -eq $Key }); if (-not $targets.Count) { throw "'$Section / $Key' is already the same as the shipped file." } }
            else { throw 'adopt needs -Section and -Key, or -All.' }
            $done = [Collections.Generic.List[string]]::new()
            foreach ($target in $targets) {
                if ($null -eq $target.Shipped) { Set-VmrQBLocalRoutingEntry -Section $target.Section -Key $target.Key -Remove -WhatIf:$WhatIfPreference }
                else { Set-VmrQBLocalRoutingEntry -Section $target.Section -Key $target.Key -Value ($target.Shipped | ConvertFrom-Json -Depth 20) -WhatIf:$WhatIfPreference }
                $done.Add("$($target.Section) / $($target.Key)")
            }
            if ($Json) { if ($done.Count) { ConvertTo-Json -InputObject @($done) } else { '[]' } }
            elseif ($done.Count) { Write-Host "Adopted from the shipped file: $($done -join '; ')." -ForegroundColor Green }
            else { Write-Host 'Nothing to adopt.' }
        }
        'acknowledge' {
            Confirm-VmrQBLocalRoutingCurrent -WhatIf:$WhatIfPreference
            if ($Json) { [ordered]@{ action = 'acknowledge'; saved = (-not $WhatIfPreference) } | ConvertTo-Json }
            elseif (-not $WhatIfPreference) { Write-Host 'Your routing copy is restamped against the current shipped file. Your edits were kept.' -ForegroundColor Green }
        }
    }
}

Invoke-QBRouting @args
