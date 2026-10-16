Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
. (Join-Path $root 'daily\contract.ps1')
. (Join-Path $root 'daily\prompts.ps1')

$failures = 0
function Check([string] $Name, [scriptblock] $Body) {
    try { & $Body; Write-Host "PASS: $Name" -ForegroundColor Green }
    catch { $script:failures++; Write-Host "FAIL: $Name -> $($_.Exception.Message)" -ForegroundColor Red }
}
function Assert-True([bool] $Condition, [string] $Message) { if (-not $Condition) { throw $Message } }

$issueEvidence = [pscustomobject]@{
    issues = @(
        [pscustomobject]@{
            repository = 'dotnet/example'; number = 101
            buildReferences = @(
                [pscustomobject]@{ buildId = 9100101; requested = $true; sourceUrl = 'https://github.com/dotnet/example/issues/101#issuecomment-5000000001' },
                [pscustomobject]@{ buildId = 9100102; requested = $false; sourceUrl = 'https://github.com/dotnet/example/issues/101' }
            )
        },
        [pscustomobject]@{
            repository = 'dotnet/dotnet'; number = 102
            buildReferences = @(
                [pscustomobject]@{ buildId = 9100103; requested = $true; sourceUrl = 'https://github.com/dotnet/dotnet/issues/102' }
            )
        }
    )
}
$operatorPath = Join-Path ([IO.Path]::GetTempPath()) ('rl-' + [guid]::NewGuid().ToString('N') + '.json')
@{ reports = @(@{ label = 'Teams - First Responders'; url = 'https://teams.microsoft.com/fixture/first-responders/1700000000'; buildIds = @(9100104) }) } |
    ConvertTo-Json -Depth 10 | Set-Content -LiteralPath $operatorPath -Encoding utf8

try {
    Check 'Only requested build references become report links' {
        $map = Get-QBReportedLinkMap -IssueEvidence $issueEvidence -OperatorLinksPath $null
        Assert-True ($map.ContainsKey('9100101')) 'Requested reference missing.'
        Assert-True (-not $map.ContainsKey('9100102')) 'Unrequested reference was included.'
        Assert-True ($map['9100101'].Keys -contains 'https://github.com/dotnet/example/issues/101#issuecomment-5000000001') 'Comment permalink lost.'
        Assert-True ([string]$map['9100101']['https://github.com/dotnet/example/issues/101#issuecomment-5000000001'].label -eq 'dotnet/example#101 comment mentions 9100101') 'Comment label wrong.'
        Assert-True ([string]$map['9100103']['https://github.com/dotnet/dotnet/issues/102'].label -eq 'dotnet/dotnet#102 issue mentions 9100103') 'Issue label wrong.'
    }

    Check 'Operator Teams permalinks merge in (Teams has no API)' {
        $map = Get-QBReportedLinkMap -IssueEvidence $issueEvidence -OperatorLinksPath $operatorPath
        Assert-True ($map.ContainsKey('9100104')) 'Operator-supplied link was not merged.'
        Assert-True ([string]$map['9100104']['https://teams.microsoft.com/fixture/first-responders/1700000000'].label -eq 'Teams - First Responders') 'Operator label lost.'
    }

    $header = '| Issue category | Builds / PRs (links) | New Issue (Y/N) | Existing fix PR (status) | Action as VMR QB (who / where to ping) | Links that QB reported |'
    $build = { param($id) "[$id](https://dev.azure.com/dnceng/internal/_build/results?buildId=$id)" }
    $content = @"
## Summary

$header
|$(' --- |' * 6)
| Repeat failure | $(& $build 9100101) | N | None found | Where: Teams - Unified Build Technical and General Discussions.<br>Copy/paste: "@dotnet/dnceng, Please check the build failure. Promotion failed: $(& $build 9100101)" |  |
| Brand new | $(& $build 9100104) | Y | None found | Where: Teams - Unified Build Technical and General Discussions.<br>Copy/paste: "@dotnet/source-build, Please check the build failure. Installer scenario failed: $(& $build 9100104)" |  |
| Unknown novelty | $(& $build 9100103) | ? | None found | Where: None - monitor only.<br>Action: Awaiting owner confirmation. |  |
"@

    Check 'Rows are sorted new issues first, then unknown, then repeats' {
        $out = ConvertTo-QBActionFormat -Content $content -Evidence $null -IssueEvidence $issueEvidence -OperatorLinksPath $operatorPath
        $rows = @(($out -split "`n") | Where-Object { $_ -match '^\|\s*(?:BH #\S* - )?(Repeat|Brand|Unknown)' })
        Assert-True ($rows.Count -eq 3) "Expected 3 data rows, got $($rows.Count)."
        Assert-True ($rows[0] -match '^\|\s*(?:BH #\S* - )?Brand new') "Y row is not first: $($rows[0])"
        Assert-True ($rows[1] -match '^\|\s*(?:BH #\S* - )?Unknown novelty') "? row is not second: $($rows[1])"
        Assert-True ($rows[2] -match '^\|\s*(?:BH #\S* - )?Repeat failure') "N row is not last: $($rows[2])"
    }

    Check 'Top actions and rows share one BH number and the same order' {
        $findings = @{
            bhReferences = @{ 'macos-capacity-agent-loss' = 1; 'runtime-round0-signing-failure' = 2 }
            builds = @(@{ buildId = 501; roots = @(
                @{ id = 'macos-capacity-agent-loss' },
                @{ id = 'runtime-round0-signing-failure' }
            ) })
        }
        $url = 'https://dev.azure.com/dnceng/internal/_build/results?buildId=501'
        $document = @"
## Top actions for the QB today
1. [501]($url) **BH #99 - Runtime Round0 signing failure:** Where: Teams - Unified Build Technical and General Discussions.
2. [501]($url) **BH #98 - macOS capacity agent loss:** Where: Teams - Unified Build Technical and General Discussions.

## Summary
$header
| --- | --- | --- | --- | --- | --- |
| BH #98 - macOS capacity agent loss | [501]($url) | Y | N/A | Where: None - monitor only.<br>Action: Recorded. | None found |
| BH #99 - Runtime Round0 signing failure | [501]($url) | N | N/A | Where: None - monitor only.<br>Action: Recorded. | None found |
"@
        $out = ConvertTo-QBActionFormat -Content $document -InternalFindings $findings
        $top = [regex]::Match($out, '(?ms)^## Top actions for the QB today\n(?<body>.*?)(?=^## |\z)').Groups['body'].Value
        $tags = @([regex]::Matches($top, 'BH #\d+') | ForEach-Object Value)
        Assert-True (($tags -join ',') -eq 'BH #2,BH #1') "Top actions retained inconsistent or duplicate tags: $top"
        $rows = @($out -split "`n" | Where-Object { $_ -match '^\| BH #' })
        Assert-True ($rows[0] -match 'BH #2 - Runtime') 'Table did not follow the first action instead of novelty.'
        Assert-True ($rows[1] -match 'BH #1 - macOS') 'The second action and table row disagree.'
        Assert-True ((ConvertTo-QBActionFormat -Content $out -InternalFindings $findings) -eq $out) 'Repeated formatting must not add another BH tag.'
    }

    Check 'Report links are auto-filled from evidence and operator file' {
        $out = ConvertTo-QBActionFormat -Content $content -Evidence $null -IssueEvidence $issueEvidence -OperatorLinksPath $operatorPath
        Assert-True ($out -match '\[dotnet/example#101 comment mentions 9100101\]\(https://github\.com/dotnet/example/issues/101#issuecomment-5000000001\)') 'GitHub comment permalink not filled.'
        Assert-True ($out -match '\[Teams - First Responders\]\(https://teams\.microsoft\.com/') 'Operator Teams link not filled.'
        Assert-True ($out -match '\[dotnet/dotnet#102 issue mentions 9100103\]\(https://github\.com/dotnet/dotnet/issues/102\)') 'Issue link not filled.'
    }

    Check 'A five-column row is normalized instead of failing the run' {
        $legacyHeader = '| Issue category | Builds / PRs (links) | New Issue (Y/N) | Existing fix PR (status) | Action as VMR QB (who / where to ping) |'
        $legacy = $content.Replace(' |  |', ' |').Replace($header, $legacyHeader).Replace('|' + (' --- |' * 6), '|' + (' --- |' * 5))
        Assert-True ($legacy.Contains($legacyHeader)) 'Test fixture did not produce a legacy header.'
        $out = ConvertTo-QBActionFormat -Content $legacy -Evidence $null -IssueEvidence $issueEvidence -OperatorLinksPath $operatorPath
        Assert-True ($out.Contains('Links that QB reported')) 'Legacy header was not upgraded.'
        foreach ($row in @(($out -split "`n") | Where-Object { $_ -match '^\|\s*(?:BH #\S* - )?(Repeat|Brand|Unknown)' })) {
            $cells = @($row.Trim().TrimStart('|').TrimEnd('|') -split '(?<!\\)\|')
            Assert-True ($cells.Count -eq 6) "Normalized row has $($cells.Count) columns: $row"
        }
    }

    Check 'The six-column table still passes overall validation' {
        $out = ConvertTo-QBActionFormat -Content $content -Evidence $null -IssueEvidence $issueEvidence -OperatorLinksPath $operatorPath
        $document = "# VMR QB Daily Overall Report - 2026-09-28`n`n## Top actions for the QB today`n`n1. [9100104](https://dev.azure.com/dnceng/internal/_build/results?buildId=9100104) - installer scenario failed. Where: Teams - Unified Build Technical and General Discussions.`n`n" + $out
        Assert-QBOverallReport -Content $document
    }

    Check 'No report link yields None found, never a blank cell' {
        $bare = [pscustomobject]@{ issues = @() }
        $out = ConvertTo-QBActionFormat -Content $content -Evidence $null -IssueEvidence $bare -OperatorLinksPath $null
        foreach ($row in @(($out -split "`n") | Where-Object { $_ -match '^\|\s*(?:BH #\S* - )?(Repeat|Brand|Unknown)' })) {
            $cells = @($row.Trim().TrimStart('|').TrimEnd('|') -split '(?<!\\)\|' | ForEach-Object { $_.Trim() })
            Assert-True ($cells[5] -eq 'None found') "Empty report cell not defaulted: $row"
        }
    }

    Check 'Operator notes render as plain text and honour their category scope' {
        $notesPath = Join-Path ([IO.Path]::GetTempPath()) ('notes-' + [guid]::NewGuid().ToString('N') + '.json')
        @{ reports = @(
            @{ note = 'Owner took the item'; match = 'Brand'; buildIds = @(9100104, 9100101) },
            @{ note = 'Applies to every row of this build'; buildIds = @(9100103) }
        ) } | ConvertTo-Json -Depth 10 | Set-Content -LiteralPath $notesPath -Encoding utf8
        try {
            $out = ConvertTo-QBActionFormat -Content $content -Evidence $null -IssueEvidence $issueEvidence -OperatorLinksPath $notesPath
            $rows = @{}
            foreach ($row in @(($out -split "`n") | Where-Object { $_ -match '^\|\s*(?:BH #\S* - )?(Repeat|Brand|Unknown)' })) {
                $cells = @($row.Trim().TrimStart('|').TrimEnd('|') -split '(?<!\\)\|' | ForEach-Object { $_.Trim() })
                $rows[($cells[0] -replace '^BH #\S* - ','')] = $cells[5]
            }
            # A note is plain text, never a dead [label](note:...) link.
            Assert-True ($out -notmatch '\]\(note:') 'A note was rendered as a link.'
            Assert-True ($rows['Brand new'] -like '*Owner took the item*') 'Scoped note missing from its own row.'
            # 9100101 is on the Repeat row, but the note is scoped to "Brand" only.
            Assert-True ($rows['Repeat failure'] -notlike '*Owner took the item*') 'Scoped note leaked onto an unrelated row sharing the build.'
            Assert-True ($rows['Unknown novelty'] -like '*Applies to every row of this build*') 'Unscoped note missing.'
        }
        finally { if (Test-Path -LiteralPath $notesPath) { Remove-Item -LiteralPath $notesPath -Force } }
    }
}
finally {
    if (Test-Path -LiteralPath $operatorPath) { Remove-Item -LiteralPath $operatorPath -Force }
}

# Recurring findings must keep the same name, so yesterday's names are fed back into the prompt.
Check 'Previous finding names are harvested, de-prefixed and de-duplicated' {
    $nameDir = Join-Path ([IO.Path]::GetTempPath()) ('vmr-names-' + [guid]::NewGuid().ToString('N'))
    [void][IO.Directory]::CreateDirectory($nameDir)
    try {
        $hdr = '| Issue category | Builds / PRs (links) | New Issue (Y/N) | Existing fix PR (status) | Action as VMR QB (who / where to ping) | Links that QB reported |'
        Set-Content -LiteralPath (Join-Path $nameDir 'overall-summary-2026-10-01.md') -Value @(
            $hdr, '|---|---|---|---|---|---|',
            '| BH #5 - Azure source-index subscription missing | x | Y | N/A | y | z |',
            '| BH #- - Darc promotion artifact download/channel failure | x | N | N/A | y | z |')
        Set-Content -LiteralPath (Join-Path $nameDir 'overall-summary-2026-10-02.md') -Value @(
            $hdr, '|---|---|---|---|---|---|',
            '| BH #7 - Azure source-index subscription missing | x | N | N/A | y | z |')
        # Today's own report must never seed the list, or the model just confirms its own rename.
        Set-Content -LiteralPath (Join-Path $nameDir 'overall-summary-2026-10-03.md') -Value @(
            $hdr, '|---|---|---|---|---|---|',
            '| BH #1 - Renamed today and must not be offered | x | Y | N/A | y | z |')

        $result = Get-PreviousFindingNames -ReportDirectory $nameDir -Date '2026-10-03'
        if ($result -notmatch 'Azure source-index subscription missing') { throw "Lost a prior name: $result" }
        if ($result -match 'BH #') { throw "BH number prefix was not stripped: $result" }
        if ($result -match 'Renamed today') { throw "Today's own report must be excluded: $result" }
        if ($result -match 'Issue category') { throw "Table header leaked in: $result" }
        $occurrences = ([regex]::Matches($result, 'Azure source-index subscription missing')).Count
        if ($occurrences -ne 1) { throw "Name repeated $occurrences times instead of de-duplicated" }
        if ($result -notmatch 'last used 2026-10-02') { throw "Most recent use was not recorded: $result" }
        if ($null -ne (Get-PreviousFindingNames -ReportDirectory (Join-Path $nameDir 'absent') -Date '2026-10-03')) {
            throw 'A missing report directory must yield null'
        }
    }
    finally { Remove-Item -LiteralPath $nameDir -Recurse -Force -ErrorAction SilentlyContinue }
}

if ($failures) { Write-Host "$failures summary-table test(s) failed." -ForegroundColor Red; exit 1 }
Write-Host '10 offline summary-table tests. No network, reports, email, or external changes.' -ForegroundColor Green
