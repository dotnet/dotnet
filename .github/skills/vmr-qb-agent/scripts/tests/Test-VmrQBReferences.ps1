[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
. (Join-Path (Split-Path $PSScriptRoot -Parent) 'daily\contract.ps1')

function Assert([bool] $Condition, [string] $Message) { if (-not $Condition) { throw $Message } }
function Assert-Throws([scriptblock] $Body, [string] $Pattern) {
    try { & $Body | Out-Null } catch {
        if ($_.Exception.Message -notmatch $Pattern) { throw }
        return
    }
    throw "Expected rejection: $Pattern"
}
function Build-Link([int] $Id) { return "[$Id](https://dev.azure.com/dnceng/internal/_build/results?buildId=$Id)" }
$links = @((Build-Link 101), (Build-Link 102))
$markdown = @"
# Build Health Monitoring - fixture
## Priority actions
1. **macOS runner capacity:** $($links -join ', ') reported provider-capacity constraints.
2. **Arcade availability:** $(Build-Link 201) had HTTP 503 and could not resolve Microsoft.DotNet.Arcade.Sdk.

## Summary
Retained snapshot.
"@
$findings = @{
    builds = @(
        @{ buildId = 201; roots = @(
            @{ id = 'arcade-http-503'; priority = 1; description = 'Arcade returned HTTP 503.'; quote = '503 Service Unavailable' },
            @{ id = 'arcade-sdk-unavailable'; priority = 1; description = 'Could not resolve Microsoft.DotNet.Arcade.Sdk.'; quote = 'Unable to find package Microsoft.DotNet.Arcade.Sdk' }
        ) },
        @{ buildId = 101; roots = @(
            @{ id = 'nuget-version-rollover'; priority = 0; description = 'Known NuGet rollover.'; quote = 'Detected package downgrade: NuGet.Versioning' },
            @{ id = 'macos-arm64-provider-capacity'; priority = 1; description = 'macOS arm64 provider capacity constraints.'; quote = 'Remote provider capacity constraints' }
        ) },
        @{ buildId = 102; roots = @(
            @{ id = 'macos-arm64-provider-capacity'; priority = 1; description = 'macOS arm64 provider capacity constraints.'; quote = 'Remote provider capacity constraints' }
        ) }
    )
}
$findings = Set-VmrQBBuildHealthReferences -Findings $findings -Markdown $markdown
Assert ((Get-VmrQBBuildHealthReference $findings 'macos-arm64-provider-capacity') -eq 1) 'JSON order must not override visible BH #1.'
Assert ((Get-VmrQBBuildHealthReference $findings 'arcade-http-503') -eq 2) 'The unreliable input priority=1 must not override visible BH #2.'
Assert ((Get-VmrQBBuildHealthReference $findings 'arcade-sdk-unavailable') -eq 2) 'Two findings under one visible action must share BH #2.'
Assert ($null -eq (Get-VmrQBBuildHealthReference $findings 'nuget-version-rollover')) 'Monitor-only findings absent from the list must not consume numbers.'
Assert ($null -eq (Get-VmrQBBuildHealthReference @{ builds = $findings.builds } 'arcade-http-503')) 'Missing reference metadata must not fall back to another counter.'
'PASS: Visible order, grouped findings, monitor-only and no-counter fallback'

$roundTrip = $findings | ConvertTo-Json -Depth 20 | ConvertFrom-Json -Depth 20
Assert ((Get-VmrQBBuildHealthReference $roundTrip 'macos-arm64-provider-capacity') -eq 1) 'Reference metadata must survive JSON persistence.'
$plan = @{ actions = @(@{ findingId = 'macos-arm64-provider-capacity'; buildIds = @(101,102); decision = 'monitor_only'; reason = 'Recorded.' }) }
$appendix = ConvertTo-QBDailyActionSection -Plan $plan -Findings $roundTrip
Assert ($appendix -match 'Build health priority #1 &mdash; macos-arm64-provider-capacity') 'The appendix must reuse the visible BH number.'
'PASS: JSON round-trip and appendix reference'

$header = '| Issue category | Builds / PRs (links) | New Issue (Y/N) | Existing fix PR (status) | Action as VMR QB (who / where to ping) | Links that QB reported |'
$content = @"
## Top actions for the QB today
1. $(Build-Link 101), - **BH #5:** and $(Build-Link 102) - BH #1 macOS arm64 provider capacity: source constraints. Where: Teams - Unified Build Technical and General Discussions.

## Summary
$header
| --- | --- | --- | --- | --- | --- |
| BH #5 - macOS arm64 provider capacity | $($links -join ', ') | N | N/A | Where: None - monitor only.<br>Action: Recorded. | None found |
"@
$formatted = ConvertTo-QBActionFormat -Content $content -InternalFindings $roundTrip
$top = [regex]::Match($formatted, '(?ms)^## Top actions for the QB today\n(?<body>.*?)(?=^## |\z)').Groups['body'].Value
Assert (([regex]::Matches($top, 'BH #')).Count -eq 1) 'Bare and formatted duplicate BH tags must be removed.'
Assert ($top -match '\*\*BH #1:\*\* macOS arm64') 'The primary action must use visible BH #1.'
Assert (([regex]::Matches($top, 'buildId=')).Count -eq 2) 'The final and-linked evidence URL must not be lost.'
Assert ($formatted -match '^\| BH #1 - macOS arm64' -or $formatted -match '(?m)^\| BH #1 - macOS arm64') 'The row must use the same visible number.'
Assert ((ConvertTo-QBActionFormat -Content $formatted -InternalFindings $roundTrip) -eq $formatted) 'Reference formatting must be idempotent.'
'PASS: Embedded duplicate tags, and-linked evidence, table consistency and idempotence'

# A mention of another item inside a sentence is kept as words; only the model's own tags are replaced.
$prose = @"
## Top actions for the QB today
1. $(Build-Link 101), $(Build-Link 102) - **BH #9:** macOS arm64 provider capacity, independent of BH #2 and (BH #2) too. Where: Teams - Unified Build Technical and General Discussions.
"@
$proseOut = ConvertTo-QBActionFormat -Content $prose -InternalFindings $roundTrip
$proseTop = [regex]::Match($proseOut, '(?ms)^## Top actions for the QB today\n(?<body>.*?)(?=^## |\z)').Groups['body'].Value
Assert (([regex]::Matches($proseTop, 'BH #')).Count -eq 1 -and $proseTop -match '\*\*BH #1:\*\* macOS arm64') 'the item keeps exactly one tag, its own authoritative number'
Assert ($proseTop.Contains('independent of build-health item 2 and (build-health item 2) too.')) "a reference to another item stays in the sentence as words: $proseTop"
Assert ((ConvertTo-QBActionFormat -Content $proseOut -InternalFindings $roundTrip) -eq $proseOut) 'and the result is stable'
'PASS: A reference to another build-health item inside a sentence is kept, not stripped to a dangling phrase'

$ambiguous = @"
## Priority actions
1. **macOS capacity:** $(Build-Link 101) capacity constraints.
2. **macOS capacity:** $(Build-Link 101) capacity constraints.
"@
Assert-Throws { Find-VmrQBVisibleReference -Actions @(Get-VmrQBReferenceActions $ambiguous) -BuildIds @('101') -Label 'macOS capacity' } 'Ambiguous'
Assert-Throws { Set-VmrQBBuildHealthReferences -Findings $roundTrip -Markdown '# No numbered actions' } 'no matching visible'
'PASS: Ambiguous or missing source numbers are rejected instead of guessed'
# A model often writes the build ids of a priority action as plain numbers instead of links; they still identify the builds.
$plain = @"
# Build Health Monitoring - fixture
## Priority actions
1. **Package hotfix not found** (internal release/10.0.1xx): 5100002, 5100003, 5100004. Owner: @dotnet/dnceng.
2. **Promotion channel association failure** ("The promotion build finished but the build isn't associated"): 5100001, 5100002, $(Build-Link 5100005). Owner: @dotnet/dnceng.
3. **5100006 macOS child node exit**: possible recurrence of #101 and dotnet/dotnet#102 on SDK 10.0.1234567.1, exit code 3221225786, build 2026-10-09.
"@
$plainActions = @(Get-VmrQBReferenceActions $plain)
Assert (($plainActions[0].buildIds -join ',') -eq '5100002,5100003,5100004') "plain build ids are read: $($plainActions[0].buildIds -join ',')"
Assert (($plainActions[1].buildIds -join ',') -eq '5100001,5100002,5100005') 'plain ids and linked ids are read together'
Assert (($plainActions[2].buildIds -join ',') -eq '5100006') "a #reference, a version, an exit code and a date are not build ids: $($plainActions[2].buildIds -join ',')"
$plainFindings = @{ builds = @(
    @{ buildId = 5100001; roots = @(@{ id = 'promotion-channel-association'; priority = 2; description = 'Promotion build not associated with a target channel.'; quote = 'the build isn''t associated with at least one of the target channels' }) },
    @{ buildId = 5100002; roots = @(
        @{ id = 'package-hotfix-missing'; priority = 1; description = 'Pinned package hotfix not found.'; quote = 'Unable to find package Example.Package' },
        @{ id = 'promotion-channel-association'; priority = 2; description = 'Promotion build not associated with a target channel.'; quote = 'not associated' }) },
    @{ buildId = 5100006; roots = @(@{ id = 'macos-child-node-exit'; priority = 2; description = 'OSX_x64 MSBuild child node exited prematurely.'; quote = 'MSB4166: Child node exited prematurely' }) }
) }
$plainFindings = Set-VmrQBBuildHealthReferences -Findings $plainFindings -Markdown $plain
Assert ((Get-VmrQBBuildHealthReference $plainFindings 'package-hotfix-missing') -eq 1) 'a finding is matched to the plain-number action by builds and words (BH #1)'
Assert ((Get-VmrQBBuildHealthReference $plainFindings 'promotion-channel-association') -eq 2) 'a finding on several builds is matched to the action that lists them (BH #2)'
Assert ((Get-VmrQBBuildHealthReference $plainFindings 'macos-child-node-exit') -eq 3) 'a build id in the action title is enough (BH #3)'
# A number that is not one of the finding's builds never matches, however the words overlap.
$elsewhere = @{ builds = @(@{ buildId = 4000001; roots = @(@{ id = 'package-hotfix-missing'; priority = 1; description = 'Pinned package hotfix not found.'; quote = 'Unable to find package' }) }) }
Assert-Throws { Set-VmrQBBuildHealthReferences -Findings $elsewhere -Markdown $plain } 'no matching visible'
'PASS: Plain-number build ids in the priority list are read and matched; numbers that are not build ids are not'
'All BH reference tests passed. No network or email.'
