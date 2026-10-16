[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$scripts = Join-Path $PSScriptRoot '..'
. (Join-Path $scripts 'common\core.ps1')
. (Join-Path $scripts 'daily\report.ps1')
. (Join-Path $scripts 'daily\gate.ps1')
. (Join-Path $scripts 'daily\preflight.ps1')
. (Join-Path $scripts 'verify\knowledge.ps1')
. (Join-Path $scripts 'verify\publish.ps1')

$failures = 0
function Check([string] $Name, [scriptblock] $Body) {
    try { & $Body; Write-Host "PASS: $Name" -ForegroundColor Green }
    catch { $script:failures++; Write-Host "FAIL: $Name -> $($_.Exception.Message)" -ForegroundColor Red }
}
function Assert($Condition, [string] $Message) { if (-not $Condition) { throw $Message } }

$sandbox = Join-Path ([IO.Path]::GetTempPath()) ('vmrqb-integration-' + [guid]::NewGuid().ToString('N'))
[void][IO.Directory]::CreateDirectory($sandbox)
$env:VMR_QB_HOME = Join-Path $sandbox 'state'
$runnerPath = Join-Path $scripts 'daily\Invoke-Daily.ps1'
$runnerText = [IO.File]::ReadAllText($runnerPath)

. (Join-Path $scripts 'daily\contract.ps1')

function Build-Link([int] $Id) { "[$Id](https://dev.azure.com/dnceng/internal/_build/results?buildId=$Id)" }
$header = '| Issue category | Builds / PRs (links) | New Issue (Y/N) | Existing fix PR (status) | Action as VMR QB (who / where to ping) | Links that QB reported |'
function Make-Report([string] $Owner) {
    $row = "| BH #1 - Arcade SDK package unavailable | $(Build-Link 100) | N | None found | Where: Teams - Unified Build Technical and General Discussions.<br>Copy/paste: ""$Owner, Please check the build failure. Arcade SDK package unavailable: $(Build-Link 100)"" | None found |"
    @('# Overall QB Summary - 2026-10-07', '', '## Top actions for the QB today', '',
      "1. $(Build-Link 100) - **BH #1:** Arcade SDK package unavailable on Windows. Where: Teams - Unified Build Technical and General Discussions.",
      '', '## Summary', '', $header, '|---|---|---|---|---|---|', $row) -join "`n"
}

Check 'The model instructions are rendered from the registry with no placeholder left' {
    $template = [IO.File]::ReadAllText((Join-Path (Get-VmrQBPaths).ReferencesRoot 'runner-instructions.md'))
    Assert ($template.Contains('{{CATEGORY_TABLE}}')) 'the shipped template must carry the placeholder'
    $text = Expand-VmrQBInstructionTemplate -Template $template
    Assert ($text -notmatch '\{\{') 'a placeholder was left in the rendered instructions'
    Assert ($text -match 'logs\[\]\.repoBuild') 'the model must be told to use the computed timeout attribution'
    # Same adjacency-aware rule as the knowledge check: naming a bad handle only to warn against it is fine.
    $rendered = Join-Path $sandbox 'AGENTS.md'; [IO.File]::WriteAllText($rendered, $text)
    $bad = @(Test-VmrQBHandleMentions -Path $rendered -RelativeName 'AGENTS.md' | Where-Object severity -eq 'fail')
    Assert ($bad.Count -eq 0) "the rendered instructions tell the model to use a handle that does not exist: $(($bad | ForEach-Object { $_.message }) -join ' | ')"
}

Check 'A template that is missing or leaves a placeholder never reaches the model' {
    $threw = 0
    foreach ($bad in @('no placeholder here', '{{CATEGORY_TABLE}} and {{SOMETHING_ELSE}}')) { try { Expand-VmrQBInstructionTemplate -Template $bad | Out-Null } catch { $threw++ } }
    Assert ($threw -eq 2) 'both a missing and a leftover placeholder must throw'
}

Check 'The validator rewrites mistaken handles from the registry, not from a hand-kept table' {
    $aliases = (Get-QBRoutingFacts).RepoTeamAliases
    Assert ($aliases['@dotnet/runtime'] -eq '@dotnet/dnr-codeflow') '@dotnet/runtime must map to dnr-codeflow'
    Assert ($aliases['@dotnet/roslyn'] -eq '@dotnet/roslyn-infrastructure') 'the roslyn redirect must be honoured'
    $routing = Get-VmrQBRouting
    Assert ($aliases.Count -eq ($routing.HandleAliases.Count + $routing.PreferredRedirects.Count)) "alias table has $($aliases.Count) entries; registry has $($routing.HandleAliases.Count + $routing.PreferredRedirects.Count)"
    foreach ($key in $aliases.Keys) { Assert ($key -ceq $key.ToLowerInvariant()) "alias key '$key' must be lowercase (lookups lowercase the handle)" }
}

Check 'A report that pings a non-existent team is rejected by the runner validator with the real team named' {
    try { Assert-QBOverallReport -Content (Make-Report '@dotnet/runtime @Second Example') } catch { $message = $_.Exception.Message }
    Assert ($message -match 'Report check failed' -and $message -match '@dotnet/runtime' -and $message -match '@dotnet/dnr-codeflow') "expected the objective check to fire and suggest the real team: $message"
}

Check 'The same report with a real team passes the whole validator' {
    Assert-QBOverallReport -Content (Make-Report '@dotnet/dnceng @Third Example')
}

Check 'Several objective problems are reported together so one repair pass can fix them all' {
    $report = (Make-Report '@dotnet/runtime @Second Example') -replace 'Where: Teams - Unified Build Technical and General Discussions\.<br>', 'Where: Teams - First Responders.<br>'
    try { Assert-QBOverallReport -Content $report } catch { $message = $_.Exception.Message }
    Assert ($message -match '@dotnet/runtime' -and $message -match 'First Responders') "both the handle and the First Responders problem must be in one message: $message"
}

Check 'VMR_QB_GATE=warn downgrades the objective checks to a warning instead of failing the run' {
    $env:VMR_QB_GATE = 'warn'
    try {
        $warnings = @()
        Assert-QBOverallReport -Content (Make-Report '@dotnet/runtime @Second Example') -WarningVariable warnings -WarningAction SilentlyContinue
        Assert (@($warnings | Where-Object { "$_" -match 'Report check failed' }).Count -eq 1) 'the problem must still be surfaced'
    }
    finally { Remove-Item Env:\VMR_QB_GATE -ErrorAction SilentlyContinue }
}

Check 'A timed-out repo-build log is attached to the evidence packet with the right repo; anything else is left alone' {
    $lines = @(
        '2026-10-05T10:00:00.0000000Z   [10:00:00.00] Building arcade', '2026-10-05T10:01:00.0000000Z   [10:01:00.00] Building arcade...done',
        '2026-10-05T10:01:02.0000000Z   [10:01:02.00] Building runtime', '2026-10-05T10:01:03.0000000Z   [10:01:03.00] Building emsdk',
        '2026-10-05T12:30:00.0000000Z ##[section]Finishing: Linux_x64')
    $log = Join-Path $sandbox 'leg.log'; $lines | Set-Content -LiteralPath $log -Encoding utf8
    $packet = Get-QBRepoBuildPacket -Path $log
    Assert ($null -ne $packet -and $packet.timedOut -and ($packet.repoAtTimeout -join ',') -eq 'runtime,emsdk') "wrong packet: $($packet | ConvertTo-Json -Compress)"
    Assert ($packet.statement -match 'runtime \(running 149 min\)') $packet.statement
    $plain = Join-Path $sandbox 'plain.log'; 'nothing to see' | Set-Content -LiteralPath $plain -Encoding utf8
    Assert ($null -eq (Get-QBRepoBuildPacket -Path $plain)) 'a log with no repo markers must not get a packet'
    Assert ($null -eq (Get-QBRepoBuildPacket -Path (Join-Path $sandbox 'missing.log'))) 'a missing log must not throw'
    Assert ($null -eq (Get-QBRepoBuildPacket -Path '')) 'an empty path must not throw'
    $partial = Join-Path $sandbox 'cut.log.partial'; $lines | Set-Content -LiteralPath $partial -Encoding utf8
    $cut = Get-QBRepoBuildPacket -Path $partial
    Assert ($null -ne $cut -and -not $cut.reliable -and @($cut.repoAtTimeout).Count -eq 0) 'a truncated log must be marked unreliable and name no repo'
}

Check 'Preflight runs after the dot-source guard, is skipped for flows that need nothing, and cannot poison the catch-up receipt' {
    $guard = $runnerText.IndexOf('if ($MyInvocation.InvocationName -eq ''.'') { return }')
    $preflight = $runnerText.IndexOf('Invoke-VmrQBPreflight -Scope $preflightScopes')
    Assert ($guard -gt 0 -and $preflight -gt $guard) 'preflight must come after the guard, or a test that dot-sources the runner would run it'
    Assert ($runnerText -match "(?s)if \(-not \(\`$UseExistingReports -or \`$UseCollectedEvidence\)\) \{ \`$preflightScopes \+= 'Collection' \}") 'reusing evidence must not demand AzDO access'
    Assert ($runnerText -match "(?s)if \(-not \(\`$SkipEmail -or \`$CollectOnly\)\) \{ \`$preflightScopes \+= 'Sending' \}") 'rehearsals must not demand Outlook'
    Assert ($runnerText -match 'email-preflight-\$Date\.json') 'the failure notice needs its own receipt'
    Assert ($runnerText.Contains('if (-not ($UseExistingReports -or $CollectOnly)) { $preflightScopes += ''Synthesis'' }')) 'collection-only runs must not demand the Copilot CLI'
    Assert ($runnerText -match 'exit 2') 'a failed preflight must exit non-zero'
    $noticeLine = ($runnerText -split "`n" | Where-Object { $_ -match 'email-preflight-' } | Select-Object -First 1)
    Assert ($noticeLine -notmatch '"email-\$Date\.json"') 'the notice must not write the day''s real receipt (a scheduled catch-up run would then skip the report)'
}

Check 'The nested model can read this run and the reports, but not the state root' {
    Assert ($runnerText -notmatch '-DataDirectory \$runRoot') 'the model must not be given the whole state root'
    Assert ($runnerText -match '\$dataDirectories = @\(\$reportDir, \(Join-Path \$artifactDir \$Date\)\)') 'the readable set must be exactly the reports and this date''s artifacts'
    Assert (([regex]::Matches($runnerText, '-DataDirectory \$dataDirectories')).Count -eq 4) 'all four model runs must use the narrowed set'
    Assert ($runnerText -match 'foreach \(\$directory in @\(\$DataDirectory\)\)') 'each directory must become its own --add-dir'
}

Check 'Owners come from the registry, not from tables kept in the runner' {
    Assert ($runnerText -match '\$ownershipBlock = \[string\]\(Invoke-QBOptional[^\n]*\n[^\n]*\{ Get-VmrQBOwnershipBlock \}') 'the owner table in the prompt must be generated'
    Assert ($runnerText -match '\$script:QBKnownOwnerTeams = @\(Invoke-QBOptional[^\n]*\{ Get-VmrQBKnownTeams \}') 'the known-teams list must be the registry'
    Assert ($runnerText -notmatch 'arcade @dotnet/dnceng; aspnetcore') 'a hand-written owner table has crept back in'
    Assert ($runnerText -notmatch "'@dotnet/aspnet-build', '@dotnet/deployment-tools-admins'") 'a hand-written team list has crept back in'
    Assert ($runnerText -notmatch "Unified Build Technical and General Discussions\|First Responders") 'the channel list must come from the registry'
}

Check 'History replay is limited to the run date, the mailer takes a subject suffix, and the merged triage context is used' {
    Assert ($runnerText -match 'Update-VmrIssueHistory .*-Date \$Date') 'the replay must be limited to today'
    Assert ($runnerText -match '\[string\] \$SubjectSuffix') 'Send-QBEmail needs the suffix for the notice'
    Assert ($runnerText -match 'triage-context\.merged\.json') 'local operator context must be merged for the model and the collector'
    Assert (([regex]::Matches($runnerText, '\$triageContextPath')).Count -ge 4) 'every triage-context consumer must read the merged file'
}

Remove-Item $sandbox -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item Env:\VMR_QB_HOME -ErrorAction SilentlyContinue
if ($failures) { Write-Host "$failures integration test(s) failed." -ForegroundColor Red; exit 1 }
Write-Host '12 offline integration tests. The runner is read, never executed; no network, email, or external changes.' -ForegroundColor Green
