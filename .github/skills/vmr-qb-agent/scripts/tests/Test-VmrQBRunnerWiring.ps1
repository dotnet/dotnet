[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# Checks that the QB runner and its modules are wired to the package layout. Static analysis plus
# the one pure function (Get-CopilotArgs); the runner itself is never executed here.
$scripts = Split-Path -Parent $PSScriptRoot
$skill = Split-Path -Parent $scripts
$runnerPath = Join-Path $scripts 'daily\Invoke-Daily.ps1'
. (Join-Path $scripts 'common\core.ps1')

$ported = @('daily\Invoke-Daily.ps1', 'daily\Invoke-Preflight.ps1', 'daily\Invoke-Report.ps1', 'common\core.ps1', 'common\routing-edit.ps1', 'common\Invoke-Setup.ps1', 'common\Invoke-Contacts.ps1', 'common\Invoke-Routing.ps1', 'history\store.ps1', 'history\history.ps1', 'history\Invoke-History.ps1', 'history\Invoke-Record.ps1', 'daily\evidence.ps1', 'daily\report.ps1', 'daily\gate.ps1', 'daily\preflight.ps1', 'daily\contract.ps1', 'daily\prompts.ps1', 'daily\workspace.ps1', 'triage\issue.ps1', 'triage\timeout.ps1', 'triage\Invoke-Triage.ps1', 'verify\knowledge.ps1', 'verify\publish.ps1', 'verify\Invoke-Verify.ps1')
$portedTests = @('Invoke-VmrQBTests.ps1', 'Test-VmrDailyEvidence.ps1', 'Test-VmrIssueTriage.ps1', 'Test-VmrDailyReportContract.ps1',
    'Test-VmrQBActionFormat.ps1', 'Test-VmrQBReferences.ps1', 'Test-VmrSummaryTable.ps1', 'Test-VmrEvidenceTransport.ps1',
    'Test-VmrModelPackets.ps1', 'Test-VmrRetainedEvidence.ps1', 'Test-VmrQBRunnerWiring.ps1', 'Test-VmrQBWorkspace.ps1', 'Test-VmrQBLocalRouting.ps1')

$failures = 0
function Check([string] $Name, [scriptblock] $Body) {
    try { & $Body; Write-Host "PASS: $Name" -ForegroundColor Green }
    catch { $script:failures++; Write-Host "FAIL: $Name -> $($_.Exception.Message)" -ForegroundColor Red }
}
function Assert([bool] $Condition, [string] $Message) { if (-not $Condition) { throw $Message } }
function Read-Script([string] $Path) {
    $errors = $null
    $ast = [Management.Automation.Language.Parser]::ParseFile($Path, [ref]$null, [ref]$errors)
    return [pscustomobject]@{ Ast = $ast; Errors = @($errors); Text = [IO.File]::ReadAllText($Path) }
}
function Get-ParameterNames($Command) {
    return @($Command.CommandElements | Where-Object { $_ -is [Management.Automation.Language.CommandParameterAst] } | ForEach-Object ParameterName)
}
$runner = Read-Script $runnerPath
# Get-CopilotArgs asks this whether the pinned model was set aside; the tests that lift it need it too.
foreach ($helperName in @('Test-QBPinnedModelIgnored')) {
    $helperNode = $runner.Ast.Find({ param($node) $node -is [Management.Automation.Language.FunctionDefinitionAst] -and $node.Name -eq $helperName }, $true)
    . ([scriptblock]::Create($helperNode.Extent.Text))
}

Check 'Every ported script and test parses cleanly' {
    foreach ($path in @($ported | ForEach-Object { Join-Path $scripts $_ }) + @($portedTests | ForEach-Object { Join-Path $PSScriptRoot $_ })) {
        $parsed = Read-Script $path
        Assert ($parsed.Errors.Count -eq 0) "$(Split-Path -Leaf $path) has a parse error: $($parsed.Errors | Select-Object -First 1 -ExpandProperty Message)"
    }
}

Check 'Every file reached through $PSScriptRoot exists beside the script that names it' {
    foreach ($name in $ported) {
        $ast = (Read-Script (Join-Path $scripts $name)).Ast
        $base = Split-Path -Parent (Join-Path $scripts $name)
        $uses = @($ast.FindAll({ param($node)
            $node -is [Management.Automation.Language.VariableExpressionAst] -and $node.VariablePath.UserPath -eq 'PSScriptRoot' }, $true))
        foreach ($use in $uses) {
            $command = $use.Parent
            Assert ($command -is [Management.Automation.Language.CommandAst] -and $command.GetCommandName() -eq 'Join-Path' -and
                $command.CommandElements.Count -eq 3 -and $command.CommandElements[2] -is [Management.Automation.Language.StringConstantExpressionAst]) `
                "$name uses `$PSScriptRoot for something other than locating a sibling script (line $($use.Extent.StartLineNumber))."
            $target = $command.CommandElements[2].Value
            Assert (Test-Path -LiteralPath (Join-Path $base $target)) "$name refers to a missing path: $target"
        }
    }
}

Check 'State and configuration come from Get-VmrQBPaths, not from the script directory' {
    foreach ($name in $ported) {
        $text = (Read-Script (Join-Path $scripts $name)).Text
        foreach ($stale in @('monitoring-config', 'VmrMonitor\.Common', 'Split-Path -Parent \$PSScriptRoot')) {
            Assert ($text -notmatch $stale) "$name still contains '$stale'."
        }
    }
    Assert ($runner.Text -notmatch '(?m)^\s*\$root\s*=') 'The runner still defines a $root.'
    foreach ($required in @('$paths.ReportsDir', '$paths.LogsDir', '$paths.ArtifactsDir', '$paths.ReportedLinksPath', '$paths.HistoryPath', '$paths.RunDir')) {
        Assert ($runner.Text.Contains($required)) "The runner never uses $required."
    }
}

Check 'The default report location is what existing-report recovery reads' {
    # Get-QBExistingReports looks in <Root>\reports and the runner passes the state root, so the two must agree.
    $resolved = Get-VmrQBPaths -StateRoot (Join-Path ([IO.Path]::GetTempPath()) 'vmr-qb-wiring-never-created')
    Assert ($resolved.ReportsDir -eq (Join-Path $resolved.StateRoot 'reports')) 'ReportsDir is no longer <state>\reports; change Get-QBExistingReports and the runner together.'
    $contractText = [IO.File]::ReadAllText((Join-Path $scripts 'daily\contract.ps1'))
    Assert ($contractText.Contains('"reports\$name-$Date.md"')) 'Get-QBExistingReports no longer reads <Root>\reports\<name>-<date>.md.'
}

Check 'Email is optional: an empty recipient turns sending off' {
    $calls = @($runner.Ast.FindAll({ param($node)
        $node -is [Management.Automation.Language.CommandAst] -and $node.GetCommandName() -eq 'Get-VmrQBConfigLenient' }, $true))
    Assert ($calls.Count -eq 1) 'The runner must read its configuration through Get-VmrQBConfigLenient exactly once: a config problem is a note, never a stop.'
    Assert (@($runner.Ast.FindAll({ param($node) $node -is [Management.Automation.Language.CommandAst] -and $node.GetCommandName() -eq 'Get-VmrQBConfig' }, $true)).Count -eq 0) 'The strict config reader must not be used by a daily run.'
    Assert (-not ($calls[0].CommandElements | Where-Object { $_ -is [Management.Automation.Language.CommandParameterAst] -and $_.ParameterName -eq 'AllowMissingIdentity' })) 'there is no identity requirement any more'
    Assert ($runner.Text -match '(?s)IsNullOrWhiteSpace\(\[string\]\$config\.recipient\).{0,900}\$SkipEmail = ') 'an empty recipient must switch sending off before anything is sent'
}
Check 'Nothing the runner writes before collection lands in the evidence directory: the triage context is merged in one function that writes beside it' {
    $resolver = $runner.Ast.Find({ param($node) $node -is [Management.Automation.Language.FunctionDefinitionAst] -and $node.Name -eq 'Resolve-QBTriageContext' }, $true)
    Assert ($null -ne $resolver) 'Resolve-QBTriageContext was not found.'
    $merges = @($runner.Ast.FindAll({ param($node) $node -is [Management.Automation.Language.CommandAst] -and $node.GetCommandName() -eq 'Merge-VmrQBTriageContext' }, $true))
    Assert ($merges.Count -ge 1) 'The triage context is never merged.'
    foreach ($merge in $merges) {
        Assert ($merge.Extent.StartOffset -ge $resolver.Extent.StartOffset -and $merge.Extent.EndOffset -le $resolver.Extent.EndOffset) "The triage context is merged outside Resolve-QBTriageContext (line $($merge.Extent.StartLineNumber))."
    }
    Assert (@($runner.Ast.FindAll({ param($node) $node -is [Management.Automation.Language.CommandAst] -and $node.GetCommandName() -eq 'Resolve-QBTriageContext' }, $true)).Count -eq 1) 'The runner must resolve the triage context exactly once.'
    Assert ($resolver.Extent.Text -notmatch 'runDirectory') 'The resolver must not write into the run directory: the collector claims it and refuses one that already has files.'
}
Check 'History replay calls the library functions with explicit paths' {
    . (Join-Path $scripts 'history\history.ps1')
    $expected = @{
        'Update-VmrIssueHistory' = @('Database', 'ReportDirectory', 'Date')
        'Set-VmrIssueAliases'    = @('Database')
    }
    foreach ($name in $expected.Keys) {
        $calls = @($runner.Ast.FindAll({ param($node)
            $node -is [Management.Automation.Language.CommandAst] -and $node.GetCommandName() -eq $name }, $true))
        Assert ($calls.Count -eq 1) "Expected exactly one call to $name, found $($calls.Count)."
        $names = Get-ParameterNames $calls[0]
        $declared = (Get-Command $name -ErrorAction Stop).Parameters.Keys
        foreach ($parameter in $expected[$name]) {
            Assert ($parameter -in $names) "$name is called without -$parameter."
            Assert ($parameter -in $declared) "$name no longer declares -$parameter, which the runner passes."
        }
    }
}
Check 'copilot starts in the per-run directory it is given and honours the optional model' {
    $function = $runner.Ast.Find({ param($node)
        $node -is [Management.Automation.Language.FunctionDefinitionAst] -and $node.Name -eq 'Get-CopilotArgs' }, $true)
    Assert ($null -ne $function) 'Get-CopilotArgs was not found.'
    . ([scriptblock]::Create($function.Extent.Text))
    $config = [pscustomobject]@{ recipient = 'qb@example.com' }
    $profiles = @(@{}, @{ Synthesis = $true }, @{ FormattingOnly = $true }, @{ EvidenceOnly = $true })

    foreach ($variant in $profiles) {
        $arguments = @(Get-CopilotArgs -Prompt 'fixture' -WorkDirectory 'C:\runs\fixture-run' @variant)
        Assert ($arguments[0] -ceq '-C' -and $arguments[1] -ceq 'C:\runs\fixture-run') 'copilot must start in the per-run directory.'
        Assert ('--model' -notin $arguments) 'No --model may be passed when the config has none.'
    }
    $config = [pscustomobject]@{ model = ' fixture-model ' }
    foreach ($variant in $profiles) {
        $arguments = @(Get-CopilotArgs -Prompt 'fixture' -WorkDirectory 'C:\runs\fixture-run' @variant)
        $index = [array]::IndexOf($arguments, '--model')
        Assert ($index -ge 0 -and $arguments[$index + 1] -ceq 'fixture-model') 'A configured model must be passed (trimmed) as --model <value>.'
    }
    $config = [pscustomobject]@{ model = '   ' }
    Assert ('--model' -notin @(Get-CopilotArgs -Prompt 'fixture' -WorkDirectory 'C:\runs\fixture-run')) 'A blank model must be ignored.'
}

Check 'The nested model is granted exactly the plugins that were found, wherever they are installed, and never a whole marketplace' {
    $function = $runner.Ast.Find({ param($node)
        $node -is [Management.Automation.Language.FunctionDefinitionAst] -and $node.Name -eq 'Get-CopilotArgs' }, $true)
    . ([scriptblock]::Create($function.Extent.Text))
    $root = Join-Path ([IO.Path]::GetTempPath()) ('vmrqb-plugins-' + [guid]::NewGuid().ToString('N'))
    try {
        foreach ($name in @('dotnet-helix', 'dotnet-codeflow', 'stale-copy-plugin')) { [void][IO.Directory]::CreateDirectory((Join-Path $root $name)) }
        $config = [pscustomobject]@{ pluginRoot = $root }
        $arguments = @(Get-CopilotArgs -Prompt 'fixture' -WorkDirectory 'C:\runs\fixture-run')
        $granted = @(for ($i = 0; $i -lt $arguments.Count - 1; $i++) { if ($arguments[$i] -ceq '--add-dir') { $arguments[$i + 1] } })
        Assert ($granted -contains (Join-Path $root 'dotnet-helix') -and $granted -contains (Join-Path $root 'dotnet-codeflow')) 'each plugin found under the configured pluginRoot must be granted'
        Assert ($granted -notcontains $root) 'the folder that holds the plugins must not be granted as a whole'
        Assert (@($granted | Where-Object { $_ -match 'stale-copy-plugin' }).Count -eq 0) 'a plugin this round does not name must not be readable'
        $formatting = @(Get-CopilotArgs -Prompt 'fixture' -WorkDirectory 'C:\runs\fixture-run' -FormattingOnly)
        Assert (@($formatting | Where-Object { $_ -match 'dotnet-helix' }).Count -eq 0) 'a document-repair run needs no plugin'
    }
    finally { Remove-Item -LiteralPath $root -Recurse -Force -ErrorAction SilentlyContinue }
    Assert ($runner.Text -notmatch 'installed-plugins\\dotnet-arcade-skills') 'the runner must not assume one install tree'
}
Check 'The nested model can read but never write: no gh api, no blanket allow, writes denied in every profile' {
    $function = $runner.Ast.Find({ param($node)
        $node -is [Management.Automation.Language.FunctionDefinitionAst] -and $node.Name -eq 'Get-CopilotArgs' }, $true)
    . ([scriptblock]::Create($function.Extent.Text))
    $config = [pscustomobject]@{ recipient = 'qb@example.com' }
    $readOnlyShell = @('gh pr list', 'gh pr view', 'gh pr checks', 'gh pr diff', 'gh issue list', 'gh issue view', 'gh search issues', 'gh search prs')
    foreach ($variant in @(@{}, @{ Synthesis = $true }, @{ FormattingOnly = $true }, @{ EvidenceOnly = $true })) {
        $arguments = @(Get-CopilotArgs -Prompt 'fixture' -WorkDirectory 'C:\runs\fixture-run' @variant)
        Assert ('--deny-tool=write' -in $arguments) 'Writes must be denied in every profile.'
        foreach ($blanket in @('--allow-all', '--allow-all-tools', '--yolo', '--allow-all-paths', '--allow-all-urls')) {
            Assert ($blanket -notin $arguments) "A profile grants $blanket."
        }
        foreach ($argument in @($arguments | Where-Object { $_ -like '--allow-tool=shell(*' })) {
            $command = $argument.Substring('--allow-tool=shell('.Length).TrimEnd(')')
            Assert ($command -in $readOnlyShell) "shell($command) is not on the read-only list; it could change something with the QB's credentials."
        }
        Assert (($arguments -join ' ') -notmatch 'shell\(gh api') 'gh api matches by subcommand, so it would also allow -X POST.'
    }
}

Check 'The model that answered is read from the transcript and shown in the email, so a changed CLI default is visible' {
    $function = $runner.Ast.Find({ param($node)
        $node -is [Management.Automation.Language.FunctionDefinitionAst] -and $node.Name -eq 'Get-QBTranscriptModel' }, $true)
    Assert ($null -ne $function) 'Get-QBTranscriptModel was not found.'
    . ([scriptblock]::Create($function.Extent.Text))
    . (Join-Path $scripts 'daily\contract.ps1')
    $dir = Join-Path ([IO.Path]::GetTempPath()) ('vmrqb-model-' + [guid]::NewGuid().ToString('N'))
    [void][IO.Directory]::CreateDirectory($dir)
    try {
        $transcript = Join-Path $dir 'copilot-fixture.jsonl'
        $lines = @('{"type":"session.info","data":{"x":1}}',
            '{"type":"assistant.message","data":{"messageId":"m1","model":"fixture-model-1","content":"hello"}}',
            '{"type":"assistant.message","data":{"messageId":"m2","model":"fixture-model-2","content":"later"}}')
        [IO.File]::WriteAllText($transcript, ($lines -join "`n") + "`n", [Text.UTF8Encoding]::new($false))
        Assert ((Get-QBTranscriptModel -Path $transcript) -ceq 'fixture-model-1') 'The first assistant message names the model.'
        [IO.File]::Delete($transcript)
        Assert (-not (Test-Path -LiteralPath $transcript)) 'The transcript must not stay locked after the model is read.'
        [IO.File]::WriteAllText($transcript, ($lines -join "`n") + "`n", [Text.UTF8Encoding]::new($false))
        Assert ($null -eq (Get-QBTranscriptModel -Path (Join-Path $dir 'missing.jsonl'))) 'A missing transcript has no model.'
        $empty = Join-Path $dir 'empty.jsonl'
        [IO.File]::WriteAllText($empty, '{"type":"session.info","data":{}}' + "`n")
        Assert ($null -eq (Get-QBTranscriptModel -Path $empty)) 'A transcript with no assistant message has no model.'
    }
    finally { Remove-Item -LiteralPath $dir -Recurse -Force -ErrorAction SilentlyContinue }

    $Date = '2026-10-07'
    $results = @(
        [pscustomobject]@{ name = 'build-health'; status = 'succeeded'; error = $null; model = 'fixture-model-1' },
        [pscustomobject]@{ name = 'code-flow'; status = 'succeeded'; error = $null })
    $html = ConvertTo-QBHtmlBody -Date $Date -RunStatus complete -Results $results -Attachments @() -OverallMarkdown $null
    Assert ($html.Contains('build-health <span style="color:#777;">(model: fixture-model-1)</span>')) 'The email must show which model wrote the report.'
    Assert ($html -notmatch 'code-flow <span[^>]*>\(model') 'A result recorded without a model must still render.'
}

Check 'The runner instructions ship with the skill' {
    $instructions = Join-Path $skill 'references\runner-instructions.md'
    Assert (Test-Path -LiteralPath $instructions -PathType Leaf) 'references/runner-instructions.md is missing.'
    Assert ((Get-Item -LiteralPath $instructions).Length -gt 0) 'references/runner-instructions.md is empty.'
}

Check 'No checkout path, email address, user-profile path or Teams permalink in the ported files' {
    $forbidden = @(
        @{ What = 'a checkout path'; Pattern = [regex]::Escape('C:\' + 'projects\' + 'VM' + 'RQB') }
        @{ What = 'an email address'; Pattern = '[A-Za-z0-9._%+-]+@[A-Za-z0-9-]+\.(?:com|net|org|io|gov|edu)\b' }
        @{ What = 'a user-profile path'; Pattern = '(?i)\\Users\\[A-Za-z0-9._-]+\\' }
        @{ What = 'a Teams message permalink'; Pattern = 'teams\.microsoft\.com/l/' }
    )
    $files = @($ported | ForEach-Object { Join-Path $scripts $_ }) +
        @($portedTests | Where-Object { $_ -ne (Split-Path -Leaf $PSCommandPath) } | ForEach-Object { Join-Path $PSScriptRoot $_ }) +
        @(Get-ChildItem -LiteralPath (Join-Path $PSScriptRoot 'fixtures') -File | ForEach-Object FullName)
    foreach ($file in $files) {
        $text = [IO.File]::ReadAllText($file)
        foreach ($rule in $forbidden) {
            Assert ($text -notmatch $rule.Pattern) "$(Split-Path -Leaf $file) contains $($rule.What)."
        }
    }
}

if ($failures) { Write-Host "$failures runner wiring check(s) failed." -ForegroundColor Red; exit 1 }
Write-Host '13 offline runner wiring checks. The runner is parsed, never executed; no network, email, or external changes.' -ForegroundColor Green
