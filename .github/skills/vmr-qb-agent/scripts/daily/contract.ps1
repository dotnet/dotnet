Set-StrictMode -Version Latest

# daily/contract.ps1 - The overall report's contract: the strict format validator, the action-table normalizer, the
# "links the QB reported" map, the existing-report recovery check and the email body.
# Every input is a parameter and none of it reads the runner's state, so Invoke-Daily.ps1, Invoke-Report.ps1 and the
# tests all load this one file instead of lifting functions out of the runner. Loading it runs nothing.
. (Join-Path $PSScriptRoot '..\common\core.ps1')
. (Join-Path $PSScriptRoot 'report.ps1')
. (Join-Path $PSScriptRoot 'gate.ps1')
function Get-QBReportedLinkMap {
    <#
    .SYNOPSIS
    Maps each problem build ID to the places the failure was already reported.
    .DESCRIPTION
    Uses collected issue evidence: buildReferences.sourceUrl is the exact issue or
    issue-comment permalink that mentions the build. Operator-supplied Teams permalinks
    in reported-links.json are merged in, because Teams chat has no API and can never be
    discovered automatically. Only requested (today's scoped) build references are used.
    #>
    param([object] $IssueEvidence, [string] $OperatorLinksPath)

    $map = @{}
    function Add-Link([string] $Id, [string] $Label, [string] $Url, [string] $Match) {
        if (-not $Id -or -not $Url) { return }
        if (-not $map.ContainsKey($Id)) { $map[$Id] = [Collections.Specialized.OrderedDictionary]::new() }
        if (-not $map[$Id].Contains($Url)) { $map[$Id].Add($Url, @{ label = $Label; match = $Match }) }
    }
    foreach ($issue in @(Get-QBField $IssueEvidence 'issues' @())) {
        $repo = [string](Get-QBField $issue 'repository')
        $number = [string](Get-QBField $issue 'number')
        foreach ($reference in @(Get-QBField $issue 'buildReferences' @())) {
            if (-not (Get-QBField $reference 'requested' $false)) { continue }
            $sourceUrl = [string](Get-QBField $reference 'sourceUrl')
            if ($sourceUrl -notmatch '^https://github\.com/') { continue }
            # State plainly that the BUILD is mentioned there. It is a lead to an existing
            # thread, not proof that this exact failure category was the one reported.
            $buildId = [string](Get-QBField $reference 'buildId')
            $kind = if ($sourceUrl -match '#issuecomment-') { 'comment' } else { 'issue' }
            Add-Link $buildId "$repo#$number $kind mentions $buildId" $sourceUrl $null
        }
    }
    if ($OperatorLinksPath -and (Test-Path -LiteralPath $OperatorLinksPath -PathType Leaf)) {
        try {
            $operator = Get-Content -LiteralPath $OperatorLinksPath -Raw | ConvertFrom-Json -Depth 20
            foreach ($entry in @(Get-QBField $operator 'reports' @())) {
                $url = [string](Get-QBField $entry 'url')
                $note = [string](Get-QBField $entry 'note')
                $label = [string](Get-QBField $entry 'label')
                # One build can carry several unrelated failures, so an operator entry may
                # scope itself to a category; otherwise it would leak onto every row.
                $match = [string](Get-QBField $entry 'match')
                if (-not $label) { $label = 'operator post' }
                foreach ($id in @(Get-QBField $entry 'buildIds' @())) {
                    if ($url -match '^https://') { Add-Link ([string]$id) $label $url $match }
                    # A note carries operator knowledge that has no link (a Teams remark,
                    # "owner took it"). Key it by text so it renders as plain text.
                    elseif ($note) { Add-Link ([string]$id) $note "note:$note" $match }
                }
            }
        }
        catch { Write-Warning "reported-links.json could not be read; operator report links were skipped: $($_.Exception.Message)" }
    }
    return $map
}

function ConvertTo-QBActionFormat {
    param([Parameter(Mandatory)][string] $Content, [object] $Evidence, [object] $IssueEvidence, [string] $OperatorLinksPath, [object] $InternalFindings)
    # BH numbers come exclusively from the visible Build Health action list.
    $findingOrder = [Collections.Specialized.OrderedDictionary]::new()
    foreach ($build in @(Get-QBField $InternalFindings 'builds' @())) {
        foreach ($root in @(Get-QBField $build 'roots' @())) {
            $rootId = [string]$root.id
            if (-not $findingOrder.Contains($rootId)) {
                $findingOrder.Add($rootId, [Collections.Generic.HashSet[string]]::new())
            }
            [void]$findingOrder[$rootId].Add([string]$build.buildId)
        }
    }
    $findingNumbers = @{}
    $findingBuilds = @{}
    foreach ($key in $findingOrder.Keys) {
        $reference = Get-VmrQBBuildHealthReference -Findings $InternalFindings -FindingId ([string]$key)
        if ($null -eq $reference) { continue }
        $findingNumbers[[string]$key] = $reference
        $findingBuilds[[string]$key] = $findingOrder[$key]
    }
    function Get-FindingNumber([Collections.Generic.HashSet[string]] $Ids, [string] $Label) {
        $words = [Collections.Generic.HashSet[string]]::new()
        foreach ($word in [regex]::Matches($Label.ToLowerInvariant(), '[a-z0-9]{3,}')) {
            [void]$words.Add($word.Value.TrimEnd('s'))
        }
        $pick = $null
        $best = 0
        foreach ($key in $findingOrder.Keys) {
            if (-not $findingNumbers.ContainsKey([string]$key)) { continue }
            $candidate = $findingBuilds[$key]
            $overlap = @($Ids | Where-Object { $candidate.Contains($_) }).Count
            if ($overlap -eq 0) { continue }
            $exact = $overlap -eq $candidate.Count -and $overlap -eq $Ids.Count
            $wordOverlap = @([regex]::Matches(([string]$key).ToLowerInvariant(), '[a-z0-9]{3,}') |
                Where-Object { $words.Contains($_.Value.TrimEnd('s')) }).Count
            if ($wordOverlap -eq 0) { continue }
            $score = $(if ($exact) { 1000 } else { 0 }) + ($overlap * 10) + $wordOverlap
            if ($score -gt $best) { $best = $score; $pick = $findingNumbers[[string]$key] }
        }
        return $pick
    }
    $topRanks = @{}
    $topUrlRanks = @{}
    $topSection = [regex]::Match($Content, '(?ms)^## Top actions for the QB today[ \t]*\r?\n(?<body>.*?)(?=^## |\z)')
    $actionIndex = 0
    foreach ($item in [regex]::Matches($topSection.Groups['body'].Value, '(?m)^\s*\d+\.\s+(?<text>[^\r\n]+)')) {
        $ids = [Collections.Generic.HashSet[string]]::new()
        foreach ($hit in [regex]::Matches($item.Groups['text'].Value, 'buildId=(\d+)')) {
            [void]$ids.Add($hit.Groups[1].Value)
        }
        $number = Get-FindingNumber $ids $item.Groups['text'].Value
        if ($null -ne $number -and -not $topRanks.ContainsKey([string]$number)) {
            $topRanks[[string]$number] = $actionIndex
        }
        foreach ($hit in [regex]::Matches($item.Groups['text'].Value, '\[[^\]]+\]\((?<url>https://[^)\s]+)\)')) {
            $url = $hit.Groups['url'].Value
            if (-not $topUrlRanks.ContainsKey($url)) { $topUrlRanks[$url] = $actionIndex }
        }
        $actionIndex++
    }
    $prByBuild = @{}
    if ($null -ne $Evidence) {
        foreach ($pipeline in $Evidence.pipelines) {
            if (-not $pipeline.inventoryPath) { continue }
            foreach ($build in @(Get-QBInventoryBuilds $pipeline.inventoryPath)) {
                $pr = Get-QBField (Get-QBField $build 'vmrClassification') 'pullRequest'
                $url = [string](Get-QBField $pr 'url')
                if ($url -match '^https://github\.com/[^/]+/[^/]+/pull/\d+$') {
                    $prByBuild[[string]$build.id] = [pscustomobject]@{ url = $url; number = $pr.number }
                }
            }
        }
    }
    $reportedByBuild = Get-QBReportedLinkMap -IssueEvidence $IssueEvidence -OperatorLinksPath $OperatorLinksPath
    $header = '| Issue category | Builds / PRs (links) | New Issue (Y/N) | Existing fix PR (status) | Action as VMR QB (who / where to ping) | Links that QB reported |'
    $legacyHeader = '| Issue category | Builds / PRs (links) | New Issue (Y/N) | Existing fix PR (status) | Action as VMR QB (who / where to ping) |'
    $inTable = $false
    $pending = [Collections.Generic.List[object]]::new()
    # "New Issue" first, then unknown, then repeats; original order is preserved within a group.
    function Get-NoveltyRank([string] $Value) {
        $text = $Value.Replace('**', '').Trim()
        if ($text -match '^\s*Y') { return 0 }
        if ($text -match '^\s*N') { return 2 }
        return 1
    }
    $lines = [Collections.Generic.List[string]]::new()
    function Flush-Pending {
        if ($pending.Count -eq 0) { return }
        $index = 0
        foreach ($row in $pending) { $row.order = $index; $index++ }
        foreach ($row in @($pending | Sort-Object actionRank, rank, order)) { $lines.Add($row.text) }
        $pending.Clear()
    }
    foreach ($line in ($Content -split '\r?\n')) {
        $trimmed = $line.Trim()
        if ($trimmed -eq $header -or $trimmed -eq $legacyHeader) {
            Flush-Pending
            $inTable = $true
            $lines.Add($header)
            continue
        }
        if ($inTable -and $trimmed -notmatch '^\|.*\|$') { Flush-Pending; $inTable = $false }
        if (-not $inTable) { $lines.Add($line); continue }
        if ($trimmed -match '^\|(?:\s*:?-+:?\s*\|){5,6}\s*$') {
            $lines.Add('|' + (' --- |' * 6))
            continue
        }
        $cells = @($trimmed.Substring(1, $trimmed.Length - 2) -split '(?<!\\)\|' | ForEach-Object { $_.Trim() })
        # Accept a five-column row and normalize it, rather than failing the whole run.
        if ($cells.Count -eq 5) { $cells += '' }
        if ($cells.Count -ne 6) { $lines.Add($line); continue }
        # Whole-message code spans prevent clickable links; removing only this wrapper preserves the text.
        $cells[4] = [regex]::Replace($cells[4], '(Copy/paste:\s*)`(".*")`$', '$1$2')
        # A bare URL (a model forgetting the link syntax at the end of the quoted message) becomes a Markdown link labelled with its build
        # id or PR/issue number: the script does this mechanical part, so it never costs the row its place in the report.
        $cells[4] = [regex]::Replace($cells[4], '(?<![\[\]\w/(])https://[^\s)\]"<>|]*[^\s)\]"<>|.,;:]', {
            param($bare)
            $label = if ($bare.Value -match 'buildId=(\d+)') { $Matches[1] } elseif ($bare.Value -match '/(?:pull|issues)/(\d+)') { "#$($Matches[1])" } else { 'link' }
            "[$label]($($bare.Value))"
        })
        $buildIds = [Collections.Generic.List[string]]::new()
        foreach ($match in [regex]::Matches($cells[1], 'https://dev\.azure\.com/[^/\s]+/[^/\s]+/_build/results\?[^)\s]*\bbuildId=(\d+)')) {
            $id = $match.Groups[1].Value
            if (-not $buildIds.Contains($id)) { $buildIds.Add($id) }
            if (-not $prByBuild.ContainsKey($id)) { continue }
            $pr = $prByBuild[$id]
            if ($cells[4].Contains($pr.url) -and -not $cells[1].Contains($pr.url)) {
                $cells[1] += ", [PR #$($pr.number)]($($pr.url))"
            }
        }
        $existing = @([regex]::Matches($cells[5], 'https://[^)\s]+') | ForEach-Object Value)
        $additions = [Collections.Generic.List[string]]::new()
        foreach ($id in $buildIds) {
            if (-not $reportedByBuild.ContainsKey($id)) { continue }
            foreach ($url in $reportedByBuild[$id].Keys) {
                if ($url -in $existing -or $cells[5].Contains($url)) { continue }
                $info = $reportedByBuild[$id][$url]
                # A scoped entry only belongs on the failure category it was recorded for.
                if ($info.match -and $cells[0] -notmatch $info.match) { continue }
                $label = [string]$info.label
                # Notes have no destination, so render them as plain text, not a dead link.
                $entry = if ($url.StartsWith('note:', [StringComparison]::Ordinal)) { $label } else { "[$label]($url)" }
                if (-not $additions.Contains($entry) -and -not $cells[5].Contains($entry)) { $additions.Add($entry) }
            }
        }
        if ($additions.Count) {
            $current = ($cells[5] -replace '(?i)^\s*(none found|none|n/?a|-)\s*$', '').Trim()
            $cells[5] = if ($current) { $current + ', ' + ($additions -join ', ') } else { $additions -join ', ' }
        }
        elseif (-not $cells[5]) { $cells[5] = 'None found' }
        # Replace whatever prefix the model wrote with the authoritative number, matched by
        # build-id set. A row whose builds match no finding is code-flow/PR work: "BH #-".
        $rowBuilds = [Collections.Generic.HashSet[string]]::new()
        foreach ($id in $buildIds) { [void]$rowBuilds.Add($id) }
        $matchedNumber = Get-FindingNumber $rowBuilds $cells[0]
        $cells[0] = [regex]::Replace($cells[0], '^\**\s*BH\s*#\S*\s*-\s*', '')
        $cells[0] = if ($matchedNumber) { "BH #$matchedNumber - $($cells[0])" } else { "BH #- - $($cells[0])" }
        $actionRank = [int]::MaxValue
        if ($null -ne $matchedNumber -and $topRanks.ContainsKey([string]$matchedNumber)) {
            $actionRank = $topRanks[[string]$matchedNumber]
        }
        elseif ($null -eq $matchedNumber) {
            foreach ($hit in [regex]::Matches($cells[1], '\[[^\]]+\]\((?<url>https://[^)\s]+)\)')) {
                if ($topUrlRanks.ContainsKey($hit.Groups['url'].Value)) {
                    $actionRank = [Math]::Min($actionRank, $topUrlRanks[$hit.Groups['url'].Value])
                }
            }
        }
        $pending.Add([pscustomobject]@{
            actionRank = $actionRank
            rank = Get-NoveltyRank $cells[2]
            order = 0
            text = '| ' + ($cells -join ' | ') + ' |'
        })
    }
    Flush-Pending
    # Top actions carry the same authoritative numbers as the table. The model writes these
    # inline tags unreliably (it has emitted only #1/#2 for ten distinct findings), so strip
    # whatever it wrote and re-tag from the build-id match.
    $output = ($lines -join "`n")
    $output = [regex]::Replace($output, '(?m)(^\s*\d+\.\s+)(.*)$', {
        param($m)
        $prefix = $m.Groups[1].Value
        # A mention of another item inside the sentence ("independent of BH #2") stays as words: it names a number from the same visible
        # list, and stripping it along with the model's own tags would leave "independent of .".
        $text = [regex]::Replace($m.Groups[2].Value.Replace('**', ''), '(?<=(?:[A-Za-z]\s+|\())BH\s*#(\d+)', 'build-health item $1')
        $body = [regex]::Replace($text, '\bBH\s*#(?:\d+|-)(?:\s*[-:\u2014])?\s*', '')
        $ids = [Collections.Generic.HashSet[string]]::new()
        foreach ($hit in [regex]::Matches($body, 'buildId=(\d+)')) { [void]$ids.Add($hit.Groups[1].Value) }
        if ($ids.Count -eq 0) { return $prefix + $body }
        $pick = Get-FindingNumber $ids $body
        if (-not $pick) { return $prefix + $body }
        $tag = '**BH #' + $pick + ':** '
        # Keep the tag where the model put it (after the evidence links); every action must
        # still START with a link, so never prepend it. Note [regex]::Replace's 4th argument
        # is RegexOptions, not a count, so use an instance to bound this to one replacement.
        $linkToken = '\[[^\]]+\]\(https://[^)\s]+\)'
        $leading = [regex]::Match($body, '^(?<links>' + $linkToken + '(?:\s*[,;]?\s*(?:[-\u2014]\s*)?(?:and\s+)?' + $linkToken + ')*)\s*[-\u2014:]?\s*(?<rest>.*)$')
        if ($leading.Success) {
            $evidenceLinks = @([regex]::Matches($leading.Groups['links'].Value, $linkToken) | ForEach-Object Value)
            return $prefix + ($evidenceLinks -join ', ') + ' - ' + $tag + $leading.Groups['rest'].Value
        }
        $separator = [regex]::new('(\s+[-\u2014]\s+)')
        if ($separator.IsMatch($body)) {
            return $prefix + $separator.Replace($body, ('$1' + $tag), 1)
        }
        return $prefix + $body
    })
    return ($output.TrimEnd() + "`n")
}

function Assert-QBOverallReport {
    <#
    .SYNOPSIS
    Throws when an overall report breaks the format contract. Every row problem is reported together, never only the first.
    .DESCRIPTION
    The exception's Data['Problems'] holds each problem on its own, so the gate can turn each into its own finding (and run note).
    -SkipObjective leaves out the handle, First Responders, placeholder and numbering checks: the gate runs those itself, so a failing
    objective check can no longer hide a format problem (and the same problem is no longer reported twice).
    #>
    param([Parameter(Mandatory)][string] $Content, [hashtable] $TeamContacts = @{}, [switch] $SkipObjective)

    # Objective checks first, reported together, so one repair pass can fix all of them: handles that do not
    # exist, First Responders used for a non-Helix failure, BH numbers that disagree, leftover placeholders.
    # VMR_QB_GATE=warn downgrades them to a console warning (for comparing runs; not for production).
    if (-not $SkipObjective) {
        $objective = @(
            @(Test-VmrQBReportHandles -Content $Content) + @(Test-VmrQBReportFirstResponders -Content $Content) +
            @(Test-VmrQBReportPlaceholders -Content $Content) + @(Test-VmrQBReportNumbering -Content $Content)
        ) | Where-Object { $_ -and $_.severity -eq 'fail' }
        if ($objective) {
            $summary = 'Report check failed: ' + (($objective | ForEach-Object { $_.message }) -join ' ')
            if ($env:VMR_QB_GATE -eq 'warn') { Write-Warning $summary } else { throw $summary }
        }
    }
    $problems = [Collections.Generic.List[string]]::new()

    if ($Content -match '\[[^\]\r\n]+\]\((?!https://)[^)\s]+\)') {
        throw 'Overall report links must be HTTPS web URLs, not local files. For a monitoring gap, use the affected service page.'
    }
    $link = '\[[^\]\r\n]+\]\(https://[^)\s]+\)'
    $url = 'https://[^)\s<>"|]+'
    $activeRouting = Get-VmrQBRouting
    $channels = (@($activeRouting.Channels.Values) | ForEach-Object { [regex]::Escape($_) }) -join '|'
    $github = '\[[^\]\r\n]+\]\(https://github\.com/[^/\s]+/[^/\s]+/(?:pull|issues)/\d+[^)\s]*\)'
    # Destinations: a Teams channel, a GitHub issue/PR, an external support destination from the routing registry
    # (a place with no GitHub team, for example 1ES support), "owner not identified" (the QB still has to route it),
    # or monitor-only (someone else has it).
    $supportDestinations = @($activeRouting.SupportDestinations)
    $supportPattern = (@($supportDestinations | ForEach-Object { 'Support - \[' + [regex]::Escape($_.Name) + '\]\(' + [regex]::Escape($_.Url) + '\)' }) -join '|')
    $supportNames = (@($supportDestinations | ForEach-Object { [regex]::Escape($_.Name) }) -join '|')
    $destinationParts = @("Teams - (?:$channels)", "GitHub - $github", 'None - monitor only', 'None - owner not identified')
    if ($supportPattern) { $destinationParts += "(?:$supportPattern)" }
    $destination = '(?:' + ($destinationParts -join '|') + ')'
    $top = [regex]::Match($Content, '(?ms)^## Top actions for the QB today[ \t]*\r?\n(?<body>.*?)(?=^## |\z)')
    if (-not $top.Success) { throw 'Overall report is missing Top actions for the QB today.' }
    $items = [regex]::Matches($top.Groups['body'].Value, '(?m)^[ \t]*(?:\d+\.|[-*])[ \t]+(?<text>[^\r\n]+)')
    if ($items.Count -eq 0 -and $top.Groups['body'].Value.Trim() -ne 'No QB action required.') {
        throw 'Top actions must contain evidence-linked items or No QB action required.'
    }
    foreach ($item in $items) {
        $text = $item.Groups['text'].Value.Replace('**', '')
        if ($text -notmatch "^$link") { $problems.Add('Every top action and sub-bullet must start with an evidence link.'); continue }
        if ($text -notmatch "Where: $destination\.?[ \t]*$") {
            $problems.Add('Every top action must end with an exact Teams/GitHub destination or monitor-only status.')
        }
    }

    $header = '| Issue category | Builds / PRs (links) | New Issue (Y/N) | Existing fix PR (status) | Action as VMR QB (who / where to ping) | Links that QB reported |'
    $inTable = $false
    $rowCount = 0
    $actionableRowCount = 0
    # A mention may be a GitHub handle (@octocat, @dotnet/example-team) or a Teams display name
    # (@Example Person), so allow up to two capitalised words after the handle. Names are Unicode (Levé, Přemek Vysoký).
    $mention = '@[\p{L}\p{N}_-]+(?:/[A-Za-z0-9_.-]+)?(?:\s+\p{Lu}[\p{L}.''-]*){0,2}'
    $owner = "(?:$mention|First Responders" + $(if ($supportNames) { "|$supportNames" } else { '' }) + ')'
    $messagePattern = '^Copy/paste: "(?<owners>' + $owner + '(?:[,\s]+' + $owner +
        ')*),\s+(?<description>[^"\r\n]+?):\s+(?<links>' + $link + '(?:[,; ]+' + $link + ')*)(?:\.)?"$'
    $directivePattern = '(?i)\b(?:please|kindly|can you|could you|should|must|needs? to)\b|' +
        '(?:^|[.;!?]\s+|\b(?:and|then)\s+)(?:correct|fix|change|update|upgrade|downgrade|backport|apply|bump|pin|rerun|retry|requeue|merge|approve|resolve|investigate|review|verify|validate|restore|check|await|wait|monitor)\b' +
        '(?:\s+(?:the|this|that|these|those|it|them|a|an|its|your|our)\b|\s+rollout\b|[.!]?$)'
    foreach ($line in ($Content -split '\r?\n')) {
        $line = $line.Trim()
        if ($line -eq $header) { $inTable = $true; continue }
        if (-not $inTable) { continue }
        if ($line -notmatch '^\|.*\|$') { $inTable = $false; continue }
        if ($line -match '^\|(?:\s*:?-+:?\s*\|){6}$') { continue }
        $cells = @($line.Substring(1, $line.Length - 2) -split '(?<!\\)\|' | ForEach-Object { $_.Trim() })
        # A row that breaks the contract is one problem; the rows after it are still checked, so every problem is reported together.
        if ($cells.Count -ne 6) { $problems.Add('Every overall summary row must retain all six columns.'); continue }
        $rowCount++
        $action = [regex]::Match($cells[4].Replace('**', ''), "^Where: (?<where>$destination)\.?<br\s*/?>\s*(?<detail>.+)$")
        if (-not $action.Success) {
            $problems.Add("Action for '$($cells[0])' needs Where: <destination>.<br> followed by Copy/paste: or Action:.")
            continue
        }
        if ($action.Groups['where'].Value -eq 'None - owner not identified') {
            # Nothing maps to a team, so there is nobody to ping and nothing to copy/paste. The row still needs the QB
            # (to find an owner), so it counts as actionable, and it must say so in the registry's own words.
            $actionableRowCount++
            $unidentified = [regex]::Match($action.Groups['detail'].Value, '^Action: (?<status>\S.+)$')
            if (-not $unidentified.Success) {
                $problems.Add("Row '$($cells[0])' has no identified owner, so it needs Action: $($activeRouting.UnmappedLabel). <what failed and what evidence is missing>, with no copy/paste message.")
                continue
            }
            $statusText = $unidentified.Groups['status'].Value
            if ($statusText -notmatch [regex]::Escape($activeRouting.UnmappedLabel)) {
                $problems.Add("Row '$($cells[0])' has no identified owner; its Action must say `"$($activeRouting.UnmappedLabel)`".")
                continue
            }
            if ($statusText -match '@[A-Za-z0-9_-]+') { $problems.Add("Row '$($cells[0])' has no identified owner, so it must not ping anyone."); continue }
            if ($statusText -match $directivePattern) { $problems.Add("Row '$($cells[0])' with no identified owner must state a factual status, not remediation instructions."); continue }
            continue
        }
        if ($action.Groups['where'].Value -eq 'None - monitor only') {
            $waiting = [regex]::Match($action.Groups['detail'].Value, '^Action: (?<status>\S.+)$')
            if (-not $waiting.Success) {
                $problems.Add("Monitor-only row '$($cells[0])' must explain what it is waiting for, without a ping.")
                continue
            }
            if ($waiting.Groups['status'].Value -match $directivePattern) {
                $problems.Add("Monitor-only row '$($cells[0])' must state a factual status, not remediation instructions.")
            }
            continue
        }
        $actionableRowCount++
        $message = [regex]::Match($action.Groups['detail'].Value, $messagePattern)
        if (-not $message.Success) {
            $problems.Add("Action for '$($cells[0])' needs a quoted message: owner, check request and failure/status: Markdown evidence links.")
            continue
        }
        $wordCount = [regex]::Matches($message.Groups['description'].Value, '\S+').Count
        if ($wordCount -gt 30) {
            $problems.Add("Action for '$($cells[0])' has $wordCount check-request/failure words; the maximum is 30 (excluding owners and links).")
        }
        # A team ping must also name one person, so someone actually owns the follow-up.
        if ($TeamContacts.Count -gt 0) {
            $owners = $message.Groups['owners'].Value
            $isTeamsDestination = $action.Groups['where'].Value -like 'Teams -*'
            foreach ($team in @([regex]::Matches($owners, '@dotnet/[A-Za-z0-9._-]+') | ForEach-Object Value)) {
                if (-not $TeamContacts.ContainsKey($team)) { continue }
                $contact = $TeamContacts[$team]
                $expected = if ($isTeamsDestination) { "@$($contact.teams)" } else { "@$($contact.github)" }
                if ($owners -notlike "*$expected*") {
                    $problems.Add("Action for '$($cells[0])' pings $team without naming its contact; add $expected to the message.")
                }
            }
        }
        $notice = [regex]::Match($message.Groups['description'].Value,
            '^Please check (?:the build failure|this issue)\.\s+(?<observation>\S.+)$')
        if (-not $notice.Success) {
            $problems.Add("Action for '$($cells[0])' must start with the brief check request, followed by the observed failure/status.")
        }
        elseif ($notice.Groups['observation'].Value -match $directivePattern) {
            $problems.Add("Action for '$($cells[0])' must report what failed, not remediation instructions.")
        }
        $evidenceUrls = @([regex]::Matches($cells[1], $url) | ForEach-Object Value)
        $allowedUrls = @([regex]::Matches(($cells[1] + ' ' + $cells[3]), $url) | ForEach-Object Value)
        $messageUrls = @([regex]::Matches($message.Groups['links'].Value, $url) | ForEach-Object Value)
        if (@($messageUrls | Where-Object { $_ -in $evidenceUrls }).Count -eq 0) {
            $problems.Add("Action for '$($cells[0])' must include an evidence link from that row.")
        }
        if (@($messageUrls | Where-Object { $_ -notin $allowedUrls }).Count -gt 0) {
            $problems.Add("Action for '$($cells[0])' contains a link absent from its evidence/fix columns.")
        }
    }
    if ($rowCount -eq 0) { $problems.Add('Overall report has no rows with the required six-column summary header.') }
    if ($actionableRowCount -gt 0 -and $items.Count -eq 0) {
        $problems.Add('Top actions cannot say no action is required when the summary contains outreach messages.')
    }
    if ($problems.Count) {
        $unique = @($problems | Select-Object -Unique)
        $failure = [InvalidOperationException]::new($unique -join ' ')
        $failure.Data['Problems'] = $unique
        throw $failure
    }
}

function Get-QBVisibleBuildHealthActions {
    <#
    .SYNOPSIS
    The numbered Build Health action list a report's BH numbers must come from, or $null when there is no report to read.
    #>
    param([AllowNull()][string] $BuildHealthPath)
    if (-not $BuildHealthPath -or -not (Test-Path -LiteralPath $BuildHealthPath -PathType Leaf)) { return $null }
    return @(Get-VmrQBReferenceActions -Markdown (Get-Content -LiteralPath $BuildHealthPath -Raw))
}

function Invoke-QBOverallReportGate {
    <#
    .SYNOPSIS
    The one gate for an overall report. Returns every finding (fail and warn), sorted as the checks produce them.
    .DESCRIPTION
    The same function, with the same inputs, is used when a report is generated, when saved reports are reused or repaired, and
    when a person checks one by hand (./daily/Invoke-Report.ps1), so a report cannot pass at one boundary and fail at the next:
      - the report text and its date;
      - the team contacts (a team ping must name its person);
      - the visible Build Health action list, which is where every BH number must come from;
      - the history as it was BEFORE today's replay (novelty and monitor-only placement are judged against earlier days);
      - the active routing, which the checks read themselves.
    -SkipFormat leaves out only the strict format validator (links, destinations, 30-word limit, named contacts).
    Fail findings block; warn findings are advisory and never block.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][string] $Content,
        [Parameter(Mandatory)][ValidatePattern('^\d{4}-\d{2}-\d{2}$')][string] $Date,
        [hashtable] $TeamContacts = @{},
        [AllowNull()][object[]] $BuildHealthActions,
        [string] $HistoryPath,
        [switch] $SkipFormat
    )
    # The run notes at the top of a saved report are the runner's, not the model's: they are never checked.
    $Content = Remove-QBRunNotesBlock -Content $Content
    $validator = if ($SkipFormat) { $null } else { { param($text) Assert-QBOverallReport -Content $text -TeamContacts $TeamContacts -SkipObjective }.GetNewClosure() }
    $arguments = @{ Content = $Content; ReportDate = $Date; BuildHealthActions = $BuildHealthActions; ContractValidator = $validator }
    if ($HistoryPath) { $arguments.HistoryPath = $HistoryPath }
    return @(Invoke-VmrQBReportGate @arguments)
}

function Add-QBReportCheckNotes {
    <#
    .SYNOPSIS
    Records each failing check as a run note. A report is never withheld because of a check: the QB sees the note at the top of it.
    .DESCRIPTION
    A failed check is drawn first, in red: it means a line of the report did not meet the contract and a person has to read it.
    #>
    [CmdletBinding()]
    param([AllowEmptyCollection()][AllowNull()][object[]] $Findings)
    # No findings arrives as $null, which binds as an array holding one $null.
    foreach ($finding in @($Findings | Where-Object { $_ -and $_.severity -eq 'fail' })) {
        Add-QBRunNote -Source 'report-check' -Severity first -Message "Report check: $($finding.message)"
    }
}

function Select-QBBetterOutcome {
    <#
    .SYNOPSIS
    After a repair pass, keeps the document with fewer problems. The repair is only taken when it is strictly better.
    .DESCRIPTION
    A repair pass is one more model run: it can come back worse than what it was given (a shorter document, or one that lost a
    section). The original is never traded for a worse document. Each outcome is what Complete-QBReportDocument returns.
    #>
    [CmdletBinding()]
    param([Parameter(Mandatory)][object] $Original, [AllowNull()][object] $Repaired)
    if ($null -eq $Repaired -or [string]::IsNullOrWhiteSpace([string]$Repaired.Content)) { return $Original }
    if (@($Repaired.Problems).Count -lt @($Original.Problems).Count) { return $Repaired }
    return $Original
}

function Get-QBReportTitle {
    <#
    .SYNOPSIS
    The dated title line a report of this name starts with (the same text the reuse check looks for).
    #>
    param([Parameter(Mandatory)][string] $Name, [Parameter(Mandatory)][string] $Date)
    switch ($Name) {
        'build-health'    { return "# Build Health Monitoring - $Date" }
        'code-flow'       { return "# Code Flow Monitoring - $Date" }
        'overall-summary' { return "# VMR QB Daily Overall Report - $Date" }
        default           { return '' }
    }
}

function Get-QBSalvagedAnswer {
    <#
    .SYNOPSIS
    The last complete document in a transcript of a model run that ended abnormally, or $null when there is none.
    .DESCRIPTION
    A run can be cut off (the time limit, a CLI error on the way out) after the model already wrote its final answer. That answer
    is a document; losing it with the run is what failed a whole report. It counts only when it is a final answer (not a progress
    message) and starts with the report's own dated title, so a half-written or unrelated message is never taken for a report.
    #>
    param([Parameter(Mandatory)][AllowEmptyString()][string] $Path, [Parameter(Mandatory)][string] $Title)
    if (-not $Path -or -not (Test-Path -LiteralPath $Path -PathType Leaf)) { return $null }
    $found = $null
    $reader = [IO.File]::OpenText($Path)
    try {
        while ($null -ne ($line = $reader.ReadLine())) {
            if ([string]::IsNullOrWhiteSpace($line) -or $line -notmatch '"type"\s*:\s*"assistant\.message"') { continue }
            try { $entry = $line.TrimStart([char]0xFEFF) | ConvertFrom-Json -Depth 100 } catch { continue }
            $content = $entry.data.PSObject.Properties['content']
            $phase = $entry.data.PSObject.Properties['phase']
            if ($phase -and $phase.Value -and $phase.Value -ne 'final_answer') { continue }
            if ($content -and $content.Value -is [string] -and $content.Value -match ('(?m)^' + [regex]::Escape($Title) + '[ \t]*\r?$')) { $found = $content.Value }
        }
    }
    finally { $reader.Dispose() }
    if ($null -eq $found) { return $null }
    return $found.Trim() + "`n"
}

function Assert-QBOverallReportGate {
    <#
    .SYNOPSIS
    Runs the one gate and throws, naming every failure together, when anything blocks. Returns the findings otherwise.
    .DESCRIPTION
    Every failure is reported in one message so a single repair pass can fix them all. VMR_QB_GATE=warn downgrades a failure to a
    console warning (for comparing runs; not for production).
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][string] $Content,
        [Parameter(Mandatory)][ValidatePattern('^\d{4}-\d{2}-\d{2}$')][string] $Date,
        [hashtable] $TeamContacts = @{},
        [AllowNull()][object[]] $BuildHealthActions,
        [string] $HistoryPath
    )
    $findings = @(Invoke-QBOverallReportGate -Content $Content -Date $Date -TeamContacts $TeamContacts -BuildHealthActions $BuildHealthActions -HistoryPath $HistoryPath)
    $failures = @($findings | Where-Object severity -eq 'fail')
    if ($failures.Count) {
        # A format failure's message already says what is wrong; the objective checks it also runs report the same problems again,
        # so identical messages are listed once.
        $messages = @($failures | ForEach-Object { [string]$_.message } | Select-Object -Unique)
        $summary = 'Report check failed: ' + ($messages -join ' ')
        if ($env:VMR_QB_GATE -eq 'warn') { Write-Warning $summary } else { throw $summary }
    }
    return $findings
}

function Get-QBExistingReports {
    <#
    .SYNOPSIS
    The saved reports of a day, for a run that reuses them instead of generating them. It never throws for what it finds.
    .DESCRIPTION
    A report that is missing or empty comes back as a failed result with its reason (the run goes on with the others and the
    evidence digest stands in for a missing overall report); one with the wrong dated title is kept with a note. A report is never
    lost to the check that was meant to protect it.
    #>
    param(
        [Parameter(Mandatory)][string] $Root,
        [Parameter(Mandatory)][ValidatePattern('^\d{4}-\d{2}-\d{2}$')][string] $Date,
        [hashtable] $TeamContacts = @{},
        # The history as it was before today's replay; the same one a generated report is checked against.
        [string] $HistoryPath
    )

    $titles = [ordered]@{
        'build-health' = (Get-QBReportTitle -Name 'build-health' -Date $Date)
        'code-flow' = (Get-QBReportTitle -Name 'code-flow' -Date $Date)
        'overall-summary' = (Get-QBReportTitle -Name 'overall-summary' -Date $Date)
    }
    $records = [Collections.Generic.List[object]]::new()
    foreach ($name in $titles.Keys) {
        $path = Join-Path $Root "reports\$name-$Date.md"
        $content = $null
        $problem = $null
        if (-not (Test-Path -LiteralPath $path -PathType Leaf)) { $problem = "The saved $name report is missing: $path" }
        else {
            try { $content = Get-Content -LiteralPath $path -Raw } catch { $problem = "The saved $name report could not be read ($($_.Exception.Message)): $path" }
            if (-not $problem -and [string]::IsNullOrWhiteSpace($content)) { $problem = "The saved $name report is empty: $path" }
        }
        if ($problem) {
            # The run's outcome step turns a failed result into a note, so none is added here.
            $records.Add([pscustomobject]@{ name = $name; status = 'failed'; path = $null; error = $problem; seconds = 0 })
            continue
        }
        if ($content -notmatch ('(?m)^' + [regex]::Escape($titles[$name]) + '[ \t]*\r?$')) {
            Add-QBRunNote -Source 'report-check' -Severity first -Message "The saved $name report has no title line '$($titles[$name])', so it may be another day's report: $path"
        }
        if ($name -eq 'overall-summary') {
            $findings = Invoke-QBOptional -Name 'The report checks' -Default @() -Script {
                Invoke-QBOverallReportGate -Content $content -Date $Date -TeamContacts $TeamContacts -HistoryPath $HistoryPath `
                    -BuildHealthActions (Get-QBVisibleBuildHealthActions -BuildHealthPath (Join-Path $Root "reports\build-health-$Date.md"))
            }
            Add-QBReportCheckNotes -Findings $findings
        }
        $records.Add([pscustomobject]@{
            name = $name; status = 'succeeded'; path = $path; error = $null; seconds = 0
        })
    }
    return $records.ToArray()
}

function Update-QBReportReferences {
    param(
        [Parameter(Mandatory)][string] $Content,
        [Parameter(Mandatory)][object] $Findings,
        [Parameter(Mandatory)][object] $Plan,
        [object] $Evidence,
        [object] $IssueEvidence,
        [string] $OperatorLinksPath,
        [hashtable] $TeamContacts = @{},
        [Parameter(Mandatory)][ValidatePattern('^\d{4}-\d{2}-\d{2}$')][string] $Date,
        [string] $HistoryPath,
        [string] $BuildHealthPath
    )
    $appendix = $Content.IndexOf('## Internal issue tracking and action drafts', [StringComparison]::Ordinal)
    $body = if ($appendix -ge 0) { $Content.Substring(0, $appendix) } else { $Content }
    $body = ConvertTo-QBActionFormat -Content $body -Evidence $Evidence -IssueEvidence $IssueEvidence `
        -OperatorLinksPath $OperatorLinksPath -InternalFindings $Findings
    $final = $body.TrimEnd() + "`n`n" + (ConvertTo-QBDailyActionSection -Plan $Plan -Findings $Findings)
    # The same checks as generation and the manual check, on the report as it will be saved; what they find is noted, not thrown.
    $findings = Invoke-QBOptional -Name 'The report checks' -Default @() -Script {
        Invoke-QBOverallReportGate -Content $final -Date $Date -TeamContacts $TeamContacts -HistoryPath $HistoryPath `
            -BuildHealthActions (Get-QBVisibleBuildHealthActions -BuildHealthPath $BuildHealthPath)
    }
    Add-QBReportCheckNotes -Findings $findings
    return $final
}

function ConvertTo-QBHtmlBody {
    param(
        [Parameter(Mandatory)][ValidatePattern('^\d{4}-\d{2}-\d{2}$')][string] $Date,
        [Parameter(Mandatory)][string] $RunStatus,
        [Parameter(Mandatory)][AllowEmptyCollection()][object[]] $Results,
        [Parameter(Mandatory)][AllowEmptyCollection()][string[]] $Attachments,
        [AllowNull()][string] $OverallMarkdown,
        [string[]] $CoverageAlerts = @(),
        # The run's notes (Add-QBRunNote), drawn in a box at the top of the email.
        [AllowEmptyCollection()][object[]] $Notes = @(),
        # What the scripts built from the collected evidence when the overall report could not be produced.
        [AllowNull()][string] $DigestMarkdown
    )
    $esc = { param($s) if ($null -eq $s) { '' } else { [System.Net.WebUtility]::HtmlEncode([string]$s) } }

    # Per-report status chips.
    $statusRows = foreach ($r in $Results) {
        $color = if ($r.status -eq 'succeeded') { '#107c10' } else { '#d13438' }
        $label = if ($r.status -eq 'succeeded') { 'OK' } else { 'FAILED' }
        $detail = if ($r.error) { ' &mdash; ' + (& $esc $r.error) } else { '' }
        $modelNote = if ($r.PSObject.Properties['model'] -and $r.model) { " <span style=""color:#777;"">(model: $(& $esc $r.model))</span>" } else { '' }
        "<li><strong style=""color:$color;"">[$label]</strong> $(& $esc $r.name)$modelNote$detail</li>"
    }

    # Convert the overall report markdown -> HTML, then inline-style the tables for Outlook. A document the converter cannot read is
    # shown as plain text: the email is never lost to its own formatting.
    $toHtml = {
        param([string] $Markdown)
        try {
            return ((ConvertFrom-Markdown -InputObject $Markdown).Html `
                -replace '<table>', '<table style="border-collapse:collapse;width:100%;margin:10px 0;font-size:13px;">' `
                -replace '<th>', '<th style="border:1px solid #c8c8c8;padding:6px 9px;background:#f3f3f3;text-align:left;vertical-align:top;">' `
                -replace '<td>', '<td style="border:1px solid #c8c8c8;padding:6px 9px;text-align:left;vertical-align:top;">')
        }
        catch {
            return '<p style="color:#d13438;"><strong>This report could not be formatted for email, so it is shown as plain text.</strong> It is attached in full.</p><pre style="white-space:pre-wrap;font-size:12px;">' + (& $esc $Markdown) + '</pre>'
        }
    }
    $overallHtml = ''
    if ($OverallMarkdown) {
        # The notes are drawn as their own box above, so the markdown block that carries them in the saved report is left out here.
        $overallHtml = & $toHtml (Remove-QBRunNotesBlock -Content $OverallMarkdown)
    } elseif ($DigestMarkdown) {
        $overallHtml = '<p style="color:#d13438;"><strong>The overall summary report was not produced this run.</strong> The scripts built this digest from the collected evidence without the model.</p>' + (& $toHtml $DigestMarkdown)
    } else {
        $overallHtml = '<p style="color:#d13438;"><strong>The overall summary report was not produced this run.</strong> See the per-report status above and any attached reports.</p>'
    }

    $attachLine = if ($Attachments.Count) {
        'Attached (full detail): ' + (($Attachments | ForEach-Object { & $esc (Split-Path $_ -Leaf) }) -join ', ')
    } else { 'No reports were successfully generated to attach.' }

    $statusColor = switch ($RunStatus) { 'complete' { '#107c10' } 'partial' { '#c19c00' } default { '#d13438' } }
    $coverageHtml = if ($CoverageAlerts.Count) {
        '<div style="border:2px solid #d13438;padding:10px;"><strong>Monitoring coverage incomplete / action required</strong><ul>' +
        (($CoverageAlerts | ForEach-Object { '<li>' + (& $esc $_) + '</li>' }) -join '') + '</ul></div>'
    } else { '' }
    $notesHtml = Format-QBRunNotesHtml -Notes $Notes

    @"
<html><head><meta charset="utf-8"></head>
<body style="font-family:'Segoe UI',Arial,sans-serif;font-size:14px;color:#1b1b1b;line-height:1.45;">
<h2 style="margin:0 0 4px 0;">VMR QB daily monitoring &mdash; $Date</h2>
<p style="margin:0 0 10px 0;">Run status: <strong style="color:$statusColor;">$RunStatus</strong></p>
$coverageHtml
$notesHtml
<ul style="margin:0 0 14px 18px;padding:0;">
$($statusRows -join "`n")
</ul>
<hr style="border:none;border-top:1px solid #e1e1e1;margin:12px 0;">
$overallHtml
<hr style="border:none;border-top:1px solid #e1e1e1;margin:12px 0;">
<p style="margin:6px 0;color:#444;">$(& $esc $attachLine)</p>
<p style="margin:6px 0;color:#777;font-size:12px;">Monitoring is read-only. No PRs were approved, merged, or closed; no Teams messages, subscription changes, or retries were issued.</p>
</body></html>
"@
}

function New-QBFallbackHtmlBody {
    <#
    .SYNOPSIS
    The plainest possible email body, for when ConvertTo-QBHtmlBody itself failed. It cannot fail for what the report says.
    .DESCRIPTION
    Reports are attached and saved either way; this keeps the run's status and notes in front of the QB instead of losing the email.
    #>
    param(
        [string] $Date, [string] $RunStatus, [AllowNull()][object[]] $Results = @(), [AllowNull()][string[]] $Attachments = @(),
        [AllowNull()][object[]] $Notes = @(), [string] $Reason = ''
    )
    $esc = { param($s) [System.Net.WebUtility]::HtmlEncode([string]$s) }
    $lines = foreach ($r in @($Results | Where-Object { $null -ne $_ })) {
        "<li>[$(if ($r.status -eq 'succeeded') { 'OK' } else { 'FAILED' })] $(& $esc $r.name)$(if ($r.error) { ' - ' + (& $esc $r.error) })</li>"
    }
    $attached = (@($Attachments | Where-Object { $_ } | ForEach-Object { & $esc (Split-Path $_ -Leaf) }) -join ', ')
    $notesHtml = try { Format-QBRunNotesHtml -Notes @($Notes | Where-Object { $null -ne $_ }) } catch { '' }
    return "<html><body style=""font-family:Segoe UI,Arial,sans-serif""><h2>VMR QB daily monitoring - $(& $esc $Date)</h2><p>Run status: <strong>$(& $esc $RunStatus)</strong></p>" +
        "<p style=""color:#d13438;"">The email could not be formatted ($(& $esc $Reason)), so this is the plain version. The reports are attached and saved.</p>" +
        "$notesHtml<ul>$($lines -join '')</ul><p>Attached: $attached</p></body></html>"
}
