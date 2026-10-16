[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# The stage inputs of the daily round: what each nested stage is handed (ownership packet, reference documents), where it
# works (a per-run directory that holds its instructions, references and task documents), and the rehearsal paths that keep
# a rehearsal out of real state. A fake copilot executable stands in for the CLI, so nothing here reaches the network.
$scripts = Split-Path -Parent $PSScriptRoot
. (Join-Path $scripts 'common\core.ps1')
. (Join-Path $scripts 'daily\prompts.ps1')
. (Join-Path $scripts 'daily\workspace.ps1')

$failures = 0
function Check([string] $Name, [scriptblock] $Body) {
    try { & $Body; Write-Host "PASS: $Name" -ForegroundColor Green }
    catch { $script:failures++; Write-Host "FAIL: $Name -> $($_.Exception.Message)" -ForegroundColor Red }
}
function Assert($Condition, [string] $Message) { if (-not $Condition) { throw $Message } }

$sandbox = Join-Path ([IO.Path]::GetTempPath()) ('vmrqb-workspace-' + [guid]::NewGuid().ToString('N'))
[void][IO.Directory]::CreateDirectory($sandbox)
$originalHome = $env:VMR_QB_HOME
$originalPath = $env:PATH
$env:VMR_QB_HOME = Join-Path $sandbox 'state'
$date = '2026-10-08'
$stamp = '2026-10-08-120000-1234567'
$paths = Get-VmrQBPaths

try {
    Check 'The build-health prompt carries the ownership packet and cannot be built without it' {
        $block = Get-VmrQBOwnershipBlock
        $routing = Get-VmrQBRouting
        Assert ($routing.RepoOwners.Count -gt 0) 'the Guide in this checkout must yield repo owners'
        $prompt = Get-BuildHealthPrompt -Date $date -EvidencePath 'digest.json' -OwnershipBlock $block
        foreach ($repo in $routing.RepoOwners.Keys) {
            Assert ($prompt.Contains("$repo $($routing.RepoOwners[$repo])")) "the build-health prompt has no '$repo $($routing.RepoOwners[$repo])' mapping"
        }
        Assert ($prompt.IndexOf($block, [StringComparison]::Ordinal) -lt $prompt.IndexOf('Use the repo-to-owner table above', [StringComparison]::Ordinal)) 'the table must come before the sentence that refers to it as "above"'
        $parameter = (Get-Command Get-BuildHealthPrompt).Parameters['OwnershipBlock']
        Assert (@($parameter.Attributes | Where-Object { $_ -is [Management.Automation.ParameterAttribute] -and $_.Mandatory }).Count -eq 1) 'OwnershipBlock must be mandatory'
        $threw = $false
        try { Get-BuildHealthPrompt -Date $date -EvidencePath 'digest.json' -OwnershipBlock '' | Out-Null } catch { $threw = $true }
        Assert $threw 'an empty ownership packet must be rejected'
    }

    Check 'Both stages and the validators read one registry: the same packet reaches build health and the overall report' {
        $block = Get-VmrQBOwnershipBlock
        $buildHealth = Get-BuildHealthPrompt -Date $date -EvidencePath 'digest.json' -OwnershipBlock $block
        $overall = Get-OverallPrompt -Date $date -OwnershipBlock $block -TeamContacts @{ '@dotnet/example' = [pscustomobject]@{ github = 'login'; teams = 'Display Name' } } -BuildHealthPath 'bh.md'
        Assert ($overall.Contains($block)) 'the overall prompt must contain the same packet'
        Assert ($overall.Contains('@dotnet/example -> GitHub @login; Teams @Display Name')) 'the roster pair must reach the overall prompt'
        Assert (-not $buildHealth.Contains('NAMED CONTACT')) 'build health names owners, not people, so it gets no roster section'
        # Built from parts so this file does not itself hold a real person's name.
        foreach ($real in @(('T' + '-Gro'), ('Tomas ' + 'Grosup'), ('jeffschw' + 'MSFT'))) { Assert (-not $overall.Contains($real)) "the prompt text must not hold a real person ($real)" }
    }

    Check 'A stage prompt names its reference documents; one without any carries no block' {
        $with = Get-OverallPrompt -Date $date -OwnershipBlock 'packet' -ReferenceDocument @('C:\run\references\report-contract.md', 'C:\run\references\triage.md')
        Assert ($with -match 'REFERENCE DOCUMENTS') 'the block is missing'
        Assert ($with.Contains('C:\run\references\report-contract.md') -and $with.Contains('C:\run\references\triage.md')) 'each staged path must be named'
        Assert ((Get-BuildHealthPrompt -Date $date -OwnershipBlock 'packet' -ReferenceDocument @('C:\run\references\triage.md')) -match 'REFERENCE DOCUMENTS') 'build health reads triage.md'
        Assert ((Get-CodeFlowPrompt -Date $date -EvidencePath 'digest.json') -notmatch 'REFERENCE DOCUMENTS') 'code flow has no reference document'
        Assert ((Get-OverallPrompt -Date $date -OwnershipBlock 'packet') -notmatch 'REFERENCE DOCUMENTS') 'no staged reference, no block'
    }

    Check 'A run directory holds the instructions, the staged references and a prompts folder; stages map to the right documents' {
        $workspace = New-QBRunWorkspace -RunDir $paths.RunDir -Stamp $stamp -ReferencesRoot $paths.ReferencesRoot
        Assert ($workspace.Path -eq (Join-Path $paths.RunDir $stamp)) 'the run directory is <run>\<stamp>'
        $agents = [IO.File]::ReadAllText((Join-Path $workspace.Path 'AGENTS.md'))
        Assert ($agents.Length -gt 0 -and $agents -notmatch '\{\{') 'AGENTS.md must be rendered with no placeholder left'
        Assert ($agents -match 'single read-only `gh` commands' -and $agents -match 'no pipes' -and $agents -match 'denied command is not a reason to skip a check') 'AGENTS.md must tell the model to run each gh command alone, since a compound command is denied'
        Assert (Test-Path -LiteralPath $workspace.PromptsDirectory -PathType Container) 'the prompts folder is missing'
        foreach ($name in @('triage.md', 'report-contract.md')) {
            Assert (Test-Path -LiteralPath $workspace.References[$name] -PathType Leaf) "$name was not staged"
            Assert ($workspace.References[$name].StartsWith($workspace.Path, [StringComparison]::OrdinalIgnoreCase)) "$name must live inside the run directory"
        }
        Assert (@(Get-QBStageReferencePath -Workspace $workspace -Stage 'build-health').Count -eq 1) 'build health reads one document'
        Assert (@(Get-QBStageReferencePath -Workspace $workspace -Stage 'overall-summary').Count -eq 2) 'the overall report reads two documents'
        Assert (@(Get-QBStageReferencePath -Workspace $workspace -Stage 'code-flow').Count -eq 0) 'code flow reads none'
    }

    Check 'The staged triage.md shows the ACTIVE routing; the public copy is never touched' {
        $public = Join-Path $paths.ReferencesRoot 'triage.md'
        $before = (Get-FileHash -LiteralPath $public).Hash
        $local = Join-Path $paths.LocalKnowledgeDir 'routing.local.json'
        $localRouting = [ordered]@{ schemaVersion = 1; channelRules = [ordered]@{ 'Servicing chat' = 'Local rule that only this QB has.' } }
        [void][IO.Directory]::CreateDirectory((Split-Path -Parent $local))
        [IO.File]::WriteAllText($local, ($localRouting | ConvertTo-Json -Depth 6), [Text.UTF8Encoding]::new($false))
        $workspace = New-QBRunWorkspace -RunDir $paths.RunDir -Stamp '2026-10-08-120001-1234567' -ReferencesRoot $paths.ReferencesRoot
        $staged = [IO.File]::ReadAllText($workspace.References['triage.md'])
        Assert ($staged.Contains('Local rule that only this QB has.')) 'the staged copy must show this QB''s effective channel rule'
        Assert ((Get-FileHash -LiteralPath $public).Hash -eq $before) 'the public triage.md must not change'
        Assert (-not ([IO.File]::ReadAllText($public)).Contains('Local rule that only this QB has.')) 'a local rule must never reach the public document'
        Remove-Item -LiteralPath $local -Force
    }

    Check 'Runs do not share a directory, and only the newest are kept' {
        $runRoot = Join-Path $sandbox 'prune-run'
        $first = New-QBRunWorkspace -RunDir $runRoot -Stamp '2026-10-01-000000-0000001' -ReferencesRoot $paths.ReferencesRoot
        [IO.File]::WriteAllText((Join-Path $first.Path 'AGENTS.md'), 'first run marker')
        $second = New-QBRunWorkspace -RunDir $runRoot -Stamp '2026-10-01-000000-0000002' -ReferencesRoot $paths.ReferencesRoot
        Assert ([IO.File]::ReadAllText((Join-Path $first.Path 'AGENTS.md')) -eq 'first run marker') 'a later run rewrote an earlier run''s instructions'
        Assert ($first.Path -ne $second.Path) 'two runs must not share a directory'
        [void][IO.Directory]::CreateDirectory((Join-Path $runRoot 'keep-this-unrelated-folder'))
        foreach ($n in 3..13) { [void](New-QBRunWorkspace -RunDir $runRoot -Stamp ('2026-10-01-000000-{0:D7}' -f $n) -ReferencesRoot $paths.ReferencesRoot -Keep 10) }
        $kept = @(Get-ChildItem -LiteralPath $runRoot -Directory | Where-Object { $_.Name -match '^\d{4}-' })
        Assert ($kept.Count -eq 10) "expected the newest 10 run directories, found $($kept.Count)"
        Assert ($kept.Name -contains '2026-10-01-000000-0000013') 'the newest run must be kept'
        Assert ($kept.Name -notcontains '2026-10-01-000000-0000001') 'the oldest run must be pruned'
        Assert (Test-Path -LiteralPath (Join-Path $runRoot 'keep-this-unrelated-folder')) 'a folder that is not a run directory must never be pruned'
    }

    Check 'A missing reference document stops the run before the model starts' {
        $emptyRoot = Join-Path $sandbox 'references-without-docs'
        [void][IO.Directory]::CreateDirectory($emptyRoot)
        Copy-Item -LiteralPath (Join-Path $paths.ReferencesRoot 'runner-instructions.md') -Destination $emptyRoot
        $threw = $false
        try { New-QBRunWorkspace -RunDir (Join-Path $sandbox 'none') -Stamp '2026-10-08-120002-1234567' -ReferencesRoot $emptyRoot | Out-Null } catch { $threw = $_.Exception.Message -match 'missing' }
        Assert $threw 'a stage that is told to read a document the skill does not have must fail loudly'
    }

    Check 'The nested copilot starts in the run directory and reads its task document from inside it' {
        $bin = Join-Path $sandbox 'bin'
        [void][IO.Directory]::CreateDirectory($bin)
        $argsFile = Join-Path $sandbox 'copilot-args.json'
        $fakePs1 = Join-Path $bin 'fake-copilot.ps1'
        [IO.File]::WriteAllText($fakePs1, @"
`$args | ConvertTo-Json -Compress | Set-Content -LiteralPath '$argsFile' -Encoding utf8
'{"type":"assistant.message","data":{"content":"# fixture report","model":"fake"}}'
'{"type":"result","exitCode":0}'
"@)
        [IO.File]::WriteAllText((Join-Path $bin 'copilot.cmd'), "@echo off`r`npwsh -NoProfile -File `"$fakePs1`" %*`r`n")
        $env:PATH = "$bin;$originalPath"
        $runner = [Management.Automation.Language.Parser]::ParseFile((Join-Path $scripts 'daily\Invoke-Daily.ps1'), [ref]$null, [ref]$null)
        foreach ($name in @('Get-CopilotArgs', 'Test-QBPinnedModelIgnored', 'Get-FinalAnswer', 'Start-CopilotReport', 'Wait-CopilotReport', 'Invoke-CopilotReport')) {
            $function = $runner.Find({ param($node) $node -is [Management.Automation.Language.FunctionDefinitionAst] -and $node.Name -eq $name }, $false)
            Assert ($null -ne $function) "$name was not found in the runner."
            . ([scriptblock]::Create($function.Extent.Text))
        }
        $config = [pscustomobject]@{ recipient = '' }
        $TimeoutMinutes = 2
        $MaxOutputMB = 10
        $workspace = New-QBRunWorkspace -RunDir $paths.RunDir -Stamp '2026-10-08-120003-1234567' -ReferencesRoot $paths.ReferencesRoot
        $transcript = Join-Path $sandbox 'logs\copilot-build-health-fixture.jsonl'
        [void][IO.Directory]::CreateDirectory((Split-Path -Parent $transcript))
        $answer = Invoke-CopilotReport -Prompt 'the task document text' -TranscriptPath $transcript -ErrorPath "$transcript.stderr.log" -WorkDirectory $workspace.Path
        Assert ($answer -match 'fixture report') 'the fake CLI answer was not read back'
        $arguments = @(Get-Content -LiteralPath $argsFile -Raw | ConvertFrom-Json)
        Assert ($arguments[0] -ceq '-C' -and $arguments[1] -ceq $workspace.Path) 'copilot must start in the per-run directory'
        $instruction = $arguments[$arguments.IndexOf('--prompt') + 1]
        $documentPath = [regex]::Match($instruction, 'task document at (?<p>.+?) using view').Groups['p'].Value
        Assert ($documentPath.StartsWith($workspace.PromptsDirectory, [StringComparison]::OrdinalIgnoreCase)) "the task document must be under the run's prompts folder, not logs: $documentPath"
        Assert ([IO.File]::ReadAllText($documentPath) -eq 'the task document text') 'the task document must hold the prompt'
        Assert (-not (Test-Path -LiteralPath "$transcript.prompt.txt")) 'no task document may be left under logs\'
        $env:PATH = $originalPath
    }

    Check 'A rehearsal is re-rooted: its own reports, logs, artifacts, run folder and history; inputs stay real' {
        $real = Get-VmrQBPaths
        $output = Join-Path $sandbox 'rehearsal'
        $rehearsal = Get-VmrQBRehearsalPaths -Paths $real -OutputRoot $output
        foreach ($pair in @(@('ReportsDir', 'reports'), @('LogsDir', 'logs'), @('ArtifactsDir', 'artifacts'), @('RunDir', 'run'))) {
            Assert ($rehearsal.($pair[0]) -eq (Join-Path ([IO.Path]::GetFullPath($output)) $pair[1])) "$($pair[0]) must be rooted at the output folder"
            Assert ($rehearsal.($pair[0]) -ne $real.($pair[0])) "$($pair[0]) must differ from the real one"
        }
        Assert ($rehearsal.HistoryPath -eq (Join-Path ([IO.Path]::GetFullPath($output)) 'issue-history.json') -and $rehearsal.HistoryPath -ne $real.HistoryPath) 'the rehearsal replays into its own history file'
        foreach ($input in @('ConfigPath', 'LocalKnowledgeDir', 'ReportedLinksPath', 'KnowledgeRoot', 'ReferencesRoot')) { Assert ($rehearsal.$input -eq $real.$input) "$input is read-only input and must stay real" }
        Assert (-not (Test-Path -LiteralPath $output)) 'computing the paths must create nothing'
        foreach ($unsafe in @($real.StateRoot, $real.ReportsDir, (Join-Path $real.LogsDir 'nested'), $real.LocalKnowledgeDir)) {
            $threw = $false
            try { Get-VmrQBRehearsalPaths -Paths $real -OutputRoot $unsafe | Out-Null } catch { $threw = $true }
            Assert $threw "an output root of '$unsafe' would let a rehearsal touch real state"
        }
        Assert ($null -ne (Get-VmrQBRehearsalPaths -Paths $real -OutputRoot (Join-Path $real.StateRoot 'rehearsal-elsewhere'))) 'a separate folder under the state root is fine'
    }

    Check 'Loading the runner only defines functions: nothing is created, no roster is built, nothing signs in' {
        $empty = Join-Path $sandbox 'dot-source-state'
        [void][IO.Directory]::CreateDirectory($empty)
        $env:VMR_QB_HOME = $empty
        $output = & pwsh -NoProfile -Command ". '$(Join-Path $scripts 'daily\Invoke-Daily.ps1')'; 'loaded'" 2>&1 | Out-String
        $env:VMR_QB_HOME = Join-Path $sandbox 'state'
        Assert ($output -match 'loaded') "the runner did not load: $output"
        $created = @(Get-ChildItem -LiteralPath $empty -Recurse -Force)
        Assert ($created.Count -eq 0) "loading the runner created: $(($created | ForEach-Object FullName) -join ', ')"
        Assert ($output -notmatch '(?i)roster|sign-in|warning') "loading the runner did work it should not: $output"
    }

    Check 'Every library loads without creating anything or signing in, so importing one is always safe' {
        $empty = Join-Path $sandbox 'library-state'
        [void][IO.Directory]::CreateDirectory($empty)
        $env:VMR_QB_HOME = $empty
        $libraries = @(Get-ChildItem -LiteralPath $scripts -Recurse -Filter '*.ps1' -File |
            Where-Object { $_.Name -notlike 'Invoke-*' -and $_.FullName -notmatch '\\tests\\' } | Sort-Object FullName | ForEach-Object FullName)
        Assert ($libraries.Count -ge 15) "expected the workflow libraries, found $($libraries.Count)"
        $lines = @('$ErrorActionPreference = ''Stop''') + @($libraries | ForEach-Object { ". '$_'" }) + @('''all loaded''')
        $output = & pwsh -NoProfile -Command ($lines -join '; ') 2>&1 | Out-String
        $env:VMR_QB_HOME = Join-Path $sandbox 'state'
        Assert ($output -match 'all loaded') "a library failed to load: $output"
        $created = @(Get-ChildItem -LiteralPath $empty -Recurse -Force)
        Assert ($created.Count -eq 0) "loading the libraries created: $(($created | ForEach-Object FullName) -join ', ')"
    }
}
finally {
    $env:PATH = $originalPath
    if ($null -eq $originalHome) { Remove-Item Env:\VMR_QB_HOME -ErrorAction SilentlyContinue } else { $env:VMR_QB_HOME = $originalHome }
    Remove-Item -LiteralPath $sandbox -Recurse -Force -ErrorAction SilentlyContinue
}

if ($failures) { Write-Host "$failures workspace test(s) failed." -ForegroundColor Red; exit 1 }
Write-Host 'All workspace tests passed.' -ForegroundColor Green
