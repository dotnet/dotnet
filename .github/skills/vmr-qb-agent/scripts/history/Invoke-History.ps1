<#
.SYNOPSIS
    Search what was seen before, find renamed duplicates, replay reports, fold aliases, repair, and export/import the handoff file. The first argument is the action.
.DESCRIPTION
    Run it from the skill's scripts/ folder. Options are the parameters of the function below; run Get-Help on the
    function after dot-sourcing, or read its comment block here. Settings are optional (local/qb-config.local.json).
#>
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot 'history.ps1')

function Invoke-QBHistory {
    <#
    .PARAMETER Action
    search      what do we know? -Text, -BuildId, -Owner, -Status, -SinceDays, -Limit
    duplicates  findings probably recorded under different names (-Threshold, -SinceDays, -Limit); changes nothing
    replay      record delivered overall-summary reports into history (-Date, -ReportDirectory); idempotent
    alias       fold one finding name into another (-Alias <name> -Canonical <name>); it is stored in the history and travels with it.
                With neither, re-applies the optional local alias file in bulk (-AliasFile)
    repair      apply aliases, make owner handles real teams, count duplicates left to review (-Preview)
    export      write the handoff file: all session data in one JSON file (-OutputPath)
    import      merge a handoff file or a plain history export (-InputPath, -Preview)
    Every action accepts -Json and then writes one JSON document to the output; a list with nothing in it is [], never nothing.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory, Position = 0)][ValidateSet('search', 'duplicates', 'replay', 'alias', 'repair', 'export', 'import')][string] $Action,
        [string] $Text, [string] $BuildId, [string] $Owner,
        [ValidateSet('', 'open', 'routed', 'owned', 'resolved', 'regressed', 'noise', 'wontfix')][string] $Status = '',
        [int] $SinceDays = 0, [int] $Limit = 0,
        [ValidateRange(0.1, 1.0)][double] $Threshold = 0.55,
        [ValidatePattern('^\d{4}-\d{2}-\d{2}$')][string] $Date, [string] $ReportDirectory,
        [string] $AliasFile, [string] $Alias, [string] $Canonical, [string] $InputPath, [string] $OutputPath, [string] $HistoryPath,
        [switch] $Preview, [switch] $Json
    )
    $paths = Get-VmrQBPaths
    if (-not $HistoryPath) { $HistoryPath = $paths.HistoryPath }
    if ($Action -ne 'import' -and -not (Test-Path -LiteralPath $HistoryPath)) { Initialize-VmrIssueStore -Path $HistoryPath | Out-Null }

    switch ($Action) {
        'search' {
            $take = if ($Limit) { $Limit } else { 25 }
            $hits = @(Search-VmrIssueHistory -Path $HistoryPath -Text $Text -BuildId $BuildId -Owner $Owner -Status $Status -SinceDays $SinceDays -Limit $take)
            if ($Json) { ConvertTo-Json -InputObject @($hits) -Depth 5; return }
            if (-not $hits.Count) { Write-Host 'Nothing in history matches. That does not prove it is new: try a distinctive word from the diagnostic, or run: ./history/Invoke-History.ps1 duplicates' -ForegroundColor Yellow; return }
            $hits | Format-Table @{ n = 'Status'; e = 'status' }, @{ n = 'Last seen'; e = 'last_seen' }, @{ n = 'Days'; e = 'occurrences' }, @{ n = 'Owner'; e = 'owner_team' }, @{ n = 'Issue'; e = 'name' }, @{ n = 'Matched on'; e = 'matchedOn' } -AutoSize -Wrap
        }
        'duplicates' {
            $take = if ($Limit) { $Limit } else { 40 }
            $pairs = @(Find-VmrIssueDuplicates -Path $HistoryPath -Threshold $Threshold -SinceDays $SinceDays -Limit $take)
            if ($Json) { ConvertTo-Json -InputObject @($pairs) -Depth 5; return }
            if (-not $pairs.Count) { Write-Host "No likely duplicates at threshold $Threshold." -ForegroundColor Green; return }
            Write-Host "$($pairs.Count) likely duplicate pair(s). Nothing was changed.`n" -ForegroundColor Cyan
            foreach ($pair in $pairs) {
                Write-Host ("[{0:n2}] keep: {1} ({2}x)" -f $pair.score, $pair.keep, $pair.keepOccurrences) -ForegroundColor Yellow
                Write-Host ("       fold: {0} ({1}x)   shared builds: {2}" -f $pair.fold, $pair.foldOccurrences, $pair.sharedBuilds)
                Write-Host ("       {0}`n" -f $pair.suggestedCli) -ForegroundColor DarkGray
            }
        }
        'replay' {
            $arguments = @{ Database = $HistoryPath }
            if ($Date) { $arguments.Date = $Date }
            if ($ReportDirectory) { $arguments.ReportDirectory = $ReportDirectory }
            $result = Update-VmrIssueHistory @arguments
            if ($Json) { ConvertTo-Json -InputObject $result -Depth 6; return }
            Write-Host "Imported $($result.imported) occurrence rows from $($result.reports) report(s)." -ForegroundColor Green
            Write-Host "Distinct issues: $($result.issues); recurring (>1 day): $(@($result.recurring).Count)" -ForegroundColor Green
            if (@($result.recurring).Count) {
                Write-Host "`nTop recurring issues:" -ForegroundColor Cyan
                @($result.recurring) | Select-Object -First 12 | Format-Table @{ n = 'Days'; e = { $_.occurrences } }, @{ n = 'First'; e = { $_.first_seen } },
                    @{ n = 'Last'; e = { $_.last_seen } }, @{ n = 'Owner'; e = { $_.owner_team } }, @{ n = 'Issue'; e = { $_.name } } -AutoSize
            }
        }
        'alias' {
            if ($Alias -or $Canonical) {
                if (-not ($Alias -and $Canonical)) { throw 'history alias needs both -Alias <name to fold> and -Canonical <name to keep>.' }
                Register-VmrIssueAlias -Path $HistoryPath -Alias $Alias -CanonicalName $Canonical | Out-Null
                if ($Json) { ConvertTo-Json -InputObject ([ordered]@{ action = 'alias'; alias = $Alias; canonical = $Canonical }); return }
                Write-Host "Folded '$Alias' into '$Canonical'. Its occurrences and status trail now belong to '$Canonical'; the alias is stored in the history, so a handoff carries it." -ForegroundColor Green
                return
            }
            $arguments = @{ Database = $HistoryPath }
            if ($AliasFile) { $arguments.AliasFile = $AliasFile }
            $result = Set-VmrIssueAliases @arguments
            if ($Json) { ConvertTo-Json -InputObject $result -Depth 6; return }
            Write-Host "Applied $($result.aliases) aliases across $($result.groups) issue groups. Recurring issues after folding: $($result.recurring)" -ForegroundColor Green
        }
        'repair' {
            $arguments = @{ HistoryPath = $HistoryPath; Preview = $Preview }
            if ($AliasFile) { $arguments.AliasFile = $AliasFile }
            $result = Repair-VmrIssueHistory @arguments
            if ($Json) { $result | ConvertTo-Json -Depth 5; return }
            $prefix = if ($Preview) { 'PREVIEW (nothing changed): ' } else { '' }
            Write-Host "${prefix}alias groups: $($result.aliasGroups); owner handles rewritten: $($result.ownersRewritten); unknown handles: $($result.unknownOwners); likely duplicates left to review: $($result.likelyDuplicates)" -ForegroundColor Green
            foreach ($row in @($result.owners | Where-Object { $_.result -eq 'fixed' })) { Write-Host ("  owner {0}: {1} -> {2}" -f $row.slug, $row.from, $row.to) -ForegroundColor DarkGray }
            foreach ($row in @($result.owners | Where-Object { $_.result -eq 'unknown' })) { Write-Host ("  UNKNOWN owner on {0}: {1} (not in routing.json; fix it with ./history/Invoke-Record.ps1)" -f $row.slug, $row.from) -ForegroundColor Yellow }
            if ($result.likelyDuplicates) { Write-Host '  Review the duplicates with: ./history/Invoke-History.ps1 duplicates -Threshold 0.95' -ForegroundColor DarkGray }
        }
        'export' {
            $arguments = @{ HistoryPath = $HistoryPath }
            if ($OutputPath) { $arguments.OutputPath = $OutputPath }
            $result = Export-VmrQBHandoff @arguments
            if ($Json) { ConvertTo-Json -InputObject $result -Depth 6; return }
            Write-Host "Handoff file written: $($result.path)" -ForegroundColor Green
            Write-Host "  $($result.issues) issues, $($result.occurrences) occurrences, $($result.contextEntries) context entries, $($result.aliasGroups) alias groups, $($result.reportedLinks) links, $($result.contacts) hand-picked contacts, $($result.routingChanges) routing changes"
            Write-Host 'Review it before sending: it holds your triage notes, links to internal builds and any contact you chose. Give it to the next QB directly; never commit it.' -ForegroundColor Yellow
        }
        'import' {
            if (-not $InputPath) { throw 'history import needs -InputPath <handoff file>.' }
            $result = Import-VmrQBHandoff -InputPath $InputPath -HistoryPath $HistoryPath -Preview:$Preview
            if ($Json) { ConvertTo-Json -InputObject $result -Depth 8; return }
            Write-Host (Format-QBHandoffSummary $result) -ForegroundColor Green
        }
    }
}

Invoke-QBHistory @args
