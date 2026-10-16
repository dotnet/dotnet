[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# One model run and the glue around it. A failed run is tried once more unless the clock ran out; a pinned model the CLI refused is
# set aside for the rest of the run (with a note) and the CLI's own default answers; the checks on a finished document only ever
# add notes; coverage alerts and preflight warnings become notes with internal authentication first. The functions are lifted from
# the runner by name, so this tests the code that runs, and no model, network or Outlook is involved.
$scripts = Split-Path -Parent $PSScriptRoot
. (Join-Path $scripts 'common\core.ps1')
. (Join-Path $scripts 'history\history.ps1')
. (Join-Path $scripts 'daily\contract.ps1')

$runner = [Management.Automation.Language.Parser]::ParseFile((Join-Path $scripts 'daily\Invoke-Daily.ps1'), [ref]$null, [ref]$null)
foreach ($name in @('Test-QBPinnedModelIgnored', 'Test-QBPinnedModelRejected', 'Invoke-QBModelRun', 'Complete-QBReportDocument', 'Add-QBCoverageNotes',
        'Add-QBPreflightNotes', 'New-QBSkippedReport', 'Invoke-QBStage', 'Get-CopilotArgs', 'Get-QBTeamContacts', 'Resolve-QBTriageContext', 'Start-CopilotReport')) {
    $function = $runner.Find({ param($node) $node -is [Management.Automation.Language.FunctionDefinitionAst] -and $node.Name -eq $name }, $false)
    if ($null -eq $function) { throw "Invoke-Daily.ps1 has no function $name." }
    . ([scriptblock]::Create($function.Extent.Text))
}

$failures = 0
function Check([string] $Name, [scriptblock] $Body) {
    try { & $Body; Write-Host "PASS: $Name" -ForegroundColor Green }
    catch { $script:failures++; Write-Host "FAIL: $Name -> $($_.Exception.Message)" -ForegroundColor Red }
}
function Assert($Condition, [string] $Message) { if (-not $Condition) { throw $Message } }
function Assert-Throws([scriptblock] $Body, [string] $Pattern) {
    try { & $Body | Out-Null } catch {
        if ($_.Exception.Message -notmatch $Pattern) { throw "wrong error: $($_.Exception.Message)" }
        return
    }
    throw "Expected an error matching: $Pattern"
}

$sandbox = Join-Path ([IO.Path]::GetTempPath()) ('vmrqb-modelrun-' + [guid]::NewGuid().ToString('N'))
[void][IO.Directory]::CreateDirectory($sandbox)

# The model is replaced by a queue of outcomes: each is a document or an error, optionally with what the CLI wrote to stderr.
$script:calls = [Collections.Generic.List[string]]::new()
$script:outcomes = [Collections.Generic.Queue[object]]::new()
function Use-Outcome([string] $ErrorPath) {
    $next = $script:outcomes.Dequeue()
    if ($next.ContainsKey('ErrorText')) { [IO.File]::WriteAllText($ErrorPath, $next.ErrorText) }
    if ($next.ContainsKey('Throw')) { throw $next.Throw }
    return $next.Text
}
function Invoke-CopilotReport {
    param([string] $Prompt, [string] $TranscriptPath, [string] $ErrorPath, [switch] $Synthesis, [switch] $FormattingOnly, [switch] $EvidenceOnly, [string[]] $DataDirectory, [string] $WorkDirectory)
    $script:calls.Add("invoke:$(Split-Path -Leaf $TranscriptPath)")
    return Use-Outcome $ErrorPath
}
function Wait-CopilotReport {
    param([object] $Handle)
    $script:calls.Add('wait')
    return Use-Outcome $Handle.errorPath
}
function Reset-Run([string] $Model = '') {
    $script:calls.Clear(); $script:outcomes.Clear(); Clear-QBRunNotes
    $script:QBIgnorePinnedModel = $false; $script:QBCoverageGap = $false
    $script:config = [pscustomobject]@{ model = $Model }
}
$runWorkspace = [pscustomobject]@{ Path = (Join-Path $sandbox 'work') }
$transcript = Join-Path $sandbox 'copilot-x.jsonl'; $errPath = Join-Path $sandbox 'copilot-x.stderr.log'

try {
    Check 'A PowerShell Copilot launcher preserves CLI arguments, captures both streams, and removes allow-all permission' {
        Reset-Run
        $work = Join-Path $sandbox 'launcher work'
        [void][IO.Directory]::CreateDirectory($work)
        $shim = Join-Path $work 'copilot.ps1'
        [IO.File]::WriteAllText($shim, @'
[pscustomobject]@{
    arguments = @($args)
    allowAllPresent = Test-Path Env:\COPILOT_ALLOW_ALL
} | ConvertTo-Json -Depth 5 -Compress
[Console]::Error.WriteLine('Synthetic CLI diagnostic.')
'@)
        function Get-Command { param([string] $Name) [pscustomobject]@{ Source = $shim } }
        $previousAllowAll = $env:COPILOT_ALLOW_ALL
        $env:COPILOT_ALLOW_ALL = '1'
        $TimeoutMinutes = 1
        $stdout = Join-Path $work 'stdout.json'
        $stderr = Join-Path $work 'stderr.log'
        $prompt = 'A prompt with spaces, "quotes", and an apostrophe: it''s preserved.'
        try {
            $handle = Start-CopilotReport -Prompt $prompt -TranscriptPath $stdout -ErrorPath $stderr -WorkDirectory $work -FormattingOnly
            try {
                Assert ($handle.process.StartInfo.FileName -eq (Get-Process -Id $PID).Path) 'a script launcher must run through the current PowerShell host'
                Assert ($handle.process.WaitForExit(15000)) 'the synthetic launcher must finish'
                Assert ($handle.outTask.Wait(5000) -and $handle.errTask.Wait(5000)) 'both output streams must be captured'
                Assert ($handle.process.ExitCode -eq 0) 'the synthetic launcher must succeed'
                Assert ($handle.budgetMinutes -eq 1) 'the time budget must be preserved'
            }
            finally {
                if (-not $handle.process.HasExited) { $handle.process.Kill($true); [void]$handle.process.WaitForExit(5000) }
                $handle.outStream.Dispose(); $handle.errStream.Dispose(); $handle.process.Dispose()
            }
        }
        finally {
            if ($null -eq $previousAllowAll) { Remove-Item Env:\COPILOT_ALLOW_ALL -ErrorAction SilentlyContinue }
            else { $env:COPILOT_ALLOW_ALL = $previousAllowAll }
        }
        $result = Get-Content -LiteralPath $stdout -Raw | ConvertFrom-Json
        $promptPath = Join-Path $work 'prompts\stdout.json.prompt.txt'
        Assert ([IO.File]::ReadAllText($promptPath) -ceq $prompt) 'the original task document must be unchanged'
        $instruction = "Read the complete task document at $promptPath using view with forceReadLargeFiles=true. Follow that document and return only its requested report."
        $instruction += ' This is a document-repair task, not an investigation. After reading that document, use no tools, skills, research, or shell commands.'
        $expected = @(Get-CopilotArgs -Prompt $instruction -WorkDirectory $work -FormattingOnly)
        Assert (($result.arguments | ConvertTo-Json -Compress) -ceq ($expected | ConvertTo-Json -Compress)) 'CLI arguments must survive script launching exactly'
        Assert (-not $result.allowAllPresent) 'the child must not inherit allow-all permission'
        Assert ([IO.File]::ReadAllText($stderr).Trim() -eq 'Synthetic CLI diagnostic.') 'stderr must still be captured separately'
    }

    Check 'A native Copilot executable is still launched directly' {
        $native = (Get-Process -Id $PID).Path
        function Get-Command { param([string] $Name) [pscustomobject]@{ Source = $native } }
        function Get-CopilotArgs { @('-NoLogo', '-NoProfile', '-NonInteractive', '-Command', "[Console]::WriteLine('Synthetic native CLI.')") }
        $TimeoutMinutes = 1
        $work = Join-Path $sandbox 'native work'
        [void][IO.Directory]::CreateDirectory($work)
        $stdout = Join-Path $work 'stdout.txt'
        $handle = Start-CopilotReport -Prompt 'Synthetic task.' -TranscriptPath $stdout -ErrorPath (Join-Path $work 'stderr.log') -WorkDirectory $work
        try {
            Assert ($handle.process.StartInfo.FileName -eq $native) 'the native executable must be started directly'
            Assert ($handle.process.StartInfo.ArgumentList[0] -eq '-NoLogo') 'native launch must not prepend a script host'
            Assert ($handle.process.WaitForExit(15000)) 'the synthetic native command must finish'
            Assert ($handle.outTask.Wait(5000) -and $handle.errTask.Wait(5000)) 'both output streams must finish'
            Assert ($handle.process.ExitCode -eq 0) 'the native command must succeed'
        }
        finally {
            if (-not $handle.process.HasExited) { $handle.process.Kill($true); [void]$handle.process.WaitForExit(5000) }
            $handle.outStream.Dispose(); $handle.errStream.Dispose(); $handle.process.Dispose()
        }
        Assert ([IO.File]::ReadAllText($stdout).Trim() -eq 'Synthetic native CLI.') 'native stdout must still be captured'
    }

    Check 'The CLI saying the pinned model does not exist or is not available is told apart from every other failure' {
        function Rejected([string] $Text) {
            $path = Join-Path $sandbox ('stderr-' + [guid]::NewGuid().ToString('N') + '.log')
            [IO.File]::WriteAllText($path, $Text)
            return (Test-QBPinnedModelRejected -ErrorPath $path)
        }
        foreach ($text in @('Error: Model "gpt-nope" from --model flag is not available.', "Error: model 'gpt-nope' not found", "The model gpt-x is not available for your plan.", "unknown model: abc", "Invalid model specified",
                "Error: the model `"zzz`" does not exist", "unsupported model", "Model claude-old is deprecated")) {
            Assert (Rejected $text) "must be recognized: $text"
        }
        foreach ($text in @('Error: network timeout while reaching the service', 'rate limit exceeded, retry later', 'Copilot exited with code 1', 'modeling data not found', '')) {
            Assert (-not (Rejected $text)) "must not be recognized: $text"
        }
        Assert (-not (Test-QBPinnedModelRejected -ErrorPath (Join-Path $sandbox 'nope.log'))) 'a missing stderr file is not a refusal'
        Assert (-not (Test-QBPinnedModelRejected -ErrorPath $null)) 'no path is not a refusal'
        # Only the end of a long error file is read.
        $early = Join-Path $sandbox 'early.log'; [IO.File]::WriteAllText($early, "model 'a' not found`n" + ('x' * 30000))
        Assert (-not (Test-QBPinnedModelRejected -ErrorPath $early)) 'the refusal text is read from the end of the file'
        $late = Join-Path $sandbox 'late.log'; [IO.File]::WriteAllText($late, ('x' * 30000) + "`nmodel 'a' not found")
        Assert (Test-QBPinnedModelRejected -ErrorPath $late) 'and found there'
    }

    Check 'A run that works is run once; a run that failed is tried once more, from a fresh transcript, and a second failure is the answer' {
        Reset-Run
        $script:outcomes.Enqueue(@{ Text = 'document' })
        $run = Invoke-QBModelRun -Name 'build-health' -Prompt 'p' -Transcript $transcript -ErrPath $errPath
        Assert ($run.Text -eq 'document' -and $run.Transcript -eq $transcript -and $script:calls.Count -eq 1) 'one run, its own transcript'
        Reset-Run
        $script:outcomes.Enqueue(@{ Throw = 'Copilot exited with code 1. Errors: x' }); $script:outcomes.Enqueue(@{ Text = 'second' })
        $run = Invoke-QBModelRun -Name 'build-health' -Prompt 'p' -Transcript $transcript -ErrPath $errPath 3>$null
        Assert ($run.Text -eq 'second' -and $run.Transcript -eq "$transcript.retry.jsonl" -and $script:calls.Count -eq 2) 'the retry has its own transcript'
        Assert (-not (Test-QBPinnedModelIgnored) -and @(Get-QBRunNotes).Count -eq 0) 'an ordinary failure sets nothing aside and adds no note'
        Reset-Run
        $script:outcomes.Enqueue(@{ Throw = 'first failure' }); $script:outcomes.Enqueue(@{ Throw = 'second failure' })
        Assert-Throws { Invoke-QBModelRun -Name 'build-health' -Prompt 'p' -Transcript $transcript -ErrPath $errPath 3>$null } 'second failure'
        Assert ($script:calls.Count -eq 2) 'only one more try'
    }

    Check 'A run that ran out of time, or ran away, is not tried again: it would spend the same time for the same result' {
        foreach ($message in @('Copilot exceeded the 25-minute limit and was terminated. Transcript: x', 'Copilot output exceeded 250 MB (runaway) and was terminated. Transcript: x')) {
            Reset-Run
            $script:outcomes.Enqueue(@{ Throw = $message }); $script:outcomes.Enqueue(@{ Text = 'must not be reached' })
            Assert-Throws { Invoke-QBModelRun -Name 'overall-summary' -Prompt 'p' -Transcript $transcript -ErrPath $errPath 3>$null } 'limit|runaway'
            Assert ($script:calls.Count -eq 1) "no second try for: $message"
        }
    }

    Check 'A run that ended abnormally after it wrote its report gives that report, with a highlighted note; one that wrote none is still a failure' {
        $title = '# VMR QB Daily Overall Report - 2026-10-14'
        function Write-Transcript([string] $Path, [object[]] $Messages) {
            $lines = foreach ($message in $Messages) { ConvertTo-Json -Compress -Depth 5 -InputObject ([ordered]@{ type = 'assistant.message'; data = [ordered]@{ content = $message.Text; phase = $message.Phase } }) }
            [IO.File]::WriteAllLines($Path, [string[]]$lines)
        }
        $document = "$title`n`n## Top actions for the QB today`nNo QB action required.`n"
        Write-Transcript $transcript @(@{ Text = 'Reading the evidence now.'; Phase = 'commentary' }, @{ Text = $document; Phase = 'final_answer' })
        foreach ($message in @('Copilot exited with code 1. Errors: x', 'Copilot exceeded the 25-minute limit and was terminated. Transcript: x')) {
            Reset-Run
            $script:outcomes.Enqueue(@{ Throw = $message }); $script:outcomes.Enqueue(@{ Text = 'must not be reached' })
            $run = Invoke-QBModelRun -Name 'overall-summary' -Prompt 'p' -Transcript $transcript -ErrPath $errPath -ExpectedTitle $title 3>$null
            Assert ($run.Text -ceq $document -and $run.Transcript -eq $transcript -and $script:calls.Count -eq 1) "the written report is used without another run: $message"
            $note = @(Get-QBRunNotes)
            Assert ($note.Count -eq 1 -and $note[0].severity -eq 'first' -and $note[0].source -eq 'model' -and $note[0].message -match 'ended abnormally' -and $note[0].message -match [regex]::Escape($message.Substring(0, 20))) "one highlighted note naming why: $(($note | ForEach-Object message) -join ' | ')"
        }
        # A progress message, another day's title or a half-written document is not a report.
        Write-Transcript $transcript @(@{ Text = $document; Phase = 'commentary' }, @{ Text = '# VMR QB Daily Overall Report - 2000-01-01'; Phase = 'final_answer' }, @{ Text = 'I could not finish.'; Phase = 'final_answer' })
        Reset-Run
        $script:outcomes.Enqueue(@{ Throw = 'Copilot exceeded the 25-minute limit and was terminated. Transcript: x' })
        Assert-Throws { Invoke-QBModelRun -Name 'overall-summary' -Prompt 'p' -Transcript $transcript -ErrPath $errPath -ExpectedTitle $title 3>$null } 'limit'
        Assert (@(Get-QBRunNotes).Count -eq 0) 'no document, so no note and the failure stands'
        # A started run is salvaged from the handle's transcript, not from the path of a retry.
        Write-Transcript $transcript @(@{ Text = $document; Phase = 'final_answer' })
        Reset-Run
        $handle = [pscustomobject]@{ errorPath = (Join-Path $sandbox 'handle.stderr.log'); transcriptPath = $transcript }
        $script:outcomes.Enqueue(@{ Throw = 'Copilot exited with code 1.' })
        $run = Invoke-QBModelRun -Name 'overall-summary' -Prompt 'p' -Transcript $transcript -ErrPath $errPath -Handle $handle -ExpectedTitle $title 3>$null
        Assert ($run.Text -ceq $document -and ($script:calls -join ',') -eq 'wait') 'a started run is salvaged after waiting for it'
        Reset-Run
    }

    Check 'A repair pass is only taken when it leaves fewer problems; the original is never traded for a worse document' {
        $original = [pscustomobject]@{ Content = 'original'; Payload = 'p'; Problems = @('one', 'two') }
        function New-Outcome([string] $Content, [string[]] $Problems) { [pscustomobject]@{ Content = $Content; Payload = $null; Problems = @($Problems) } }
        Assert ((Select-QBBetterOutcome -Original $original -Repaired (New-Outcome 'better' @('one'))).Content -eq 'better') 'fewer problems: the repair'
        Assert ((Select-QBBetterOutcome -Original $original -Repaired (New-Outcome 'same' @('a', 'b'))).Content -eq 'original') 'the same number: the original'
        Assert ((Select-QBBetterOutcome -Original $original -Repaired (New-Outcome 'worse' @('a', 'b', 'c'))).Content -eq 'original') 'more problems: the original'
        Assert ((Select-QBBetterOutcome -Original $original -Repaired (New-Outcome '   ' @())).Content -eq 'original') 'an empty repair is never taken'
        Assert ((Select-QBBetterOutcome -Original $original -Repaired $null).Content -eq 'original') 'no repair, the original'
        Assert ((Select-QBBetterOutcome -Original $original -Repaired (New-Outcome 'clean' @())).Content -eq 'clean') 'a clean repair is taken'
    }

    Check 'A stage that throws is a failed result with its reason and never stops the run; a stage that works returns its result' {
        $failed = Invoke-QBStage -Name 'overall-summary' -Script { throw 'the prompt could not be built' } 6>$null
        Assert ($failed.name -eq 'overall-summary' -and $failed.status -eq 'failed' -and $null -eq $failed.path -and $failed.error -match 'overall-summary stage stopped unexpectedly: the prompt could not be built') 'a failed result that says why'
        $worked = Invoke-QBStage -Name 'code-flow' -Script { [pscustomobject]@{ name = 'code-flow'; status = 'succeeded'; path = 'x'; error = $null; seconds = 1 } }
        Assert ($worked.status -eq 'succeeded' -and $worked.path -eq 'x') 'the stage''s own result is returned'
    }

    Check 'The email body is never lost to its formatting: an unreadable report is shown as plain text, and a formatter that fails entirely has a plain body' {
        function ConvertFrom-Markdown { param($InputObject) throw 'the markdown engine failed' }
        $html = ConvertTo-QBHtmlBody -Date '2026-10-14' -RunStatus partial -Results @() -Attachments @('overall.md') -OverallMarkdown "# VMR QB Daily Overall Report - 2026-10-14`n| a | b |`n"
        Assert ($html -match 'could not be formatted for email' -and $html -match '<pre' -and $html -match 'VMR QB Daily Overall Report') 'the report is in the body as text'
        Remove-Item Function:\ConvertFrom-Markdown
        $results = @([pscustomobject]@{ name = 'overall-summary'; status = 'failed'; error = 'x <y>' })
        $plain = New-QBFallbackHtmlBody -Date '2026-10-14' -RunStatus partial -Results $results -Attachments @('C:\r\build-health-2026-10-14.md') -Notes @([pscustomobject]@{ severity = 'first'; source = 'x'; message = 'a note'; fix = '' }) -Reason 'the formatter failed'
        Assert ($plain -match 'FAILED\] overall-summary' -and $plain -match 'x &lt;y&gt;' -and $plain -match 'build-health-2026-10-14\.md' -and $plain -match 'a note' -and $plain -match 'plain version') 'status, escaped error, attachment and notes'
        $bare = New-QBFallbackHtmlBody -Date '2026-10-14' -RunStatus failed
        Assert ($bare -match '2026-10-14') 'it needs nothing but a date'
    }

    Check 'A pinned model the CLI refused is set aside for the rest of the run with a note, and the retry uses the CLI''s default' {
        Reset-Run 'gpt-nope'
        $script:outcomes.Enqueue(@{ Throw = 'Copilot exited with code 1.'; ErrorText = "Error: model 'gpt-nope' not found" }); $script:outcomes.Enqueue(@{ Text = 'by the default model' })
        $run = Invoke-QBModelRun -Name 'build-health' -Prompt 'p' -Transcript $transcript -ErrPath $errPath 3>$null
        Assert ($run.Text -eq 'by the default model' -and $script:calls.Count -eq 2) 'the second try succeeds'
        Assert (Test-QBPinnedModelIgnored) 'the pinned model is set aside'
        $note = @(Get-QBRunNotes)
        Assert ($note.Count -eq 1 -and $note[0].source -eq 'model' -and $note[0].message -match "'gpt-nope' was refused" -and $note[0].fix -match 'Clear or change model') 'the note names the model and the fix'
        $withIt = @(Get-CopilotArgs -Prompt 'x' -WorkDirectory 'C:\w')
        Assert ('--model' -notin $withIt) 'the next run no longer passes --model'
        $script:QBIgnorePinnedModel = $false
        $index = [array]::IndexOf(@(Get-CopilotArgs -Prompt 'x' -WorkDirectory 'C:\w'), '--model')
        Assert ($index -ge 0) 'and when it is not set aside, --model is passed'

        # The same when the run was started earlier and its handle is waited on; the error file is the handle's.
        Reset-Run 'gpt-nope'
        $handleErrors = Join-Path $sandbox 'handle.stderr.log'
        $handle = [pscustomobject]@{ errorPath = $handleErrors; transcriptPath = $transcript }
        $script:outcomes.Enqueue(@{ Throw = 'Copilot exited with code 1.'; ErrorText = 'The model gpt-nope is not available.' }); $script:outcomes.Enqueue(@{ Text = 'ok' })
        $run = Invoke-QBModelRun -Name 'code-flow' -Prompt 'p' -Transcript $transcript -ErrPath $errPath -Handle $handle 3>$null
        Assert ($run.Text -eq 'ok' -and ($script:calls -join ',') -match '^wait,invoke:copilot-x\.jsonl\.retry\.jsonl$' -and (Test-QBPinnedModelIgnored)) 'a started run is waited on first, then retried'

        Reset-Run 'gpt-nope'
        $script:outcomes.Enqueue(@{ Throw = 'Copilot exited with code 1.'; ErrorText = 'rate limit exceeded' }); $script:outcomes.Enqueue(@{ Text = 'ok' })
        [void](Invoke-QBModelRun -Name 'build-health' -Prompt 'p' -Transcript $transcript -ErrPath $errPath 3>$null)
        Assert (-not (Test-QBPinnedModelIgnored) -and @(Get-QBRunNotes).Count -eq 0) 'another failure leaves the pinned model alone'

        Reset-Run ''
        $script:outcomes.Enqueue(@{ Throw = 'Copilot exited with code 1.'; ErrorText = "model 'x' not found" }); $script:outcomes.Enqueue(@{ Text = 'ok' })
        [void](Invoke-QBModelRun -Name 'build-health' -Prompt 'p' -Transcript $transcript -ErrPath $errPath 3>$null)
        Assert (-not (Test-QBPinnedModelIgnored) -and @(Get-QBRunNotes).Count -eq 0) 'with no pinned model there is nothing to set aside'
        Reset-Run
    }

    Check 'The triage context is written beside the run''s evidence directory, never inside it, so the collector can claim that directory' {
        # The collector refuses an evidence directory that already holds files, so a file the runner writes there before collection
        # fails the whole collection. This is the real check the collector makes, applied after what the runner writes first.
        $artifacts = Join-Path $sandbox 'artifacts'
        $cases = [ordered]@{
            missing = $null
            valid   = '{ "schemaVersion": 1, "entries": [ { "id": "local-one", "title": "One" } ] }'
            corrupt = '{ nope'
        }
        $number = 0
        foreach ($name in $cases.Keys) {
            Reset-Run
            $number++
            $stamp = '2026-10-09-13280{0}-0000001' -f $number
            $local = Join-Path $sandbox "triage-$name.local.json"
            if ($null -ne $cases[$name]) { [IO.File]::WriteAllText($local, $cases[$name]) }
            $runDirectory = Join-Path $artifacts "2026-10-09\$stamp"
            [void][IO.Directory]::CreateDirectory($runDirectory)
            $path = Resolve-QBTriageContext -ArtifactDir $artifacts -Date '2026-10-09' -Stamp $stamp -LocalContextPath $local
            Assert (Test-Path -LiteralPath $path -PathType Leaf) "${name}: a context file exists"
            Assert (-not $path.StartsWith($runDirectory + [IO.Path]::DirectorySeparatorChar, [StringComparison]::OrdinalIgnoreCase)) "${name}: the context is not inside the evidence directory"
            Assert (@([IO.Directory]::EnumerateFileSystemEntries($runDirectory)).Count -eq 0) "${name}: nothing was written into the evidence directory"
            Assert ((Initialize-VmrEvidenceDirectory -OutputDirectory $runDirectory) -eq [IO.Path]::GetFullPath($runDirectory)) "${name}: the collector can claim the directory"
            $entries = @((Get-Content -LiteralPath $path -Raw | ConvertFrom-Json).entries)
            $notes = @(Get-QBRunNotes)
            switch ($name) {
                'missing' { Assert ($entries.Count -eq 0 -and $notes.Count -eq 0) 'missing: an empty context and no note' }
                'valid' { Assert ($entries.Count -eq 1 -and $entries[0].id -eq 'local-one' -and $notes.Count -eq 0) 'valid: the local entries are used' }
                'corrupt' {
                    Assert ($entries.Count -eq 0 -and $notes.Count -eq 1 -and $notes[0].message -match 'triage context.*could not be used' -and $notes[0].fix -match 'Fix or move') 'corrupt: skipped with a note'
                    Assert ((Get-Content -LiteralPath $local -Raw).Trim() -eq '{ nope') 'corrupt: the file is left as it was'
                }
            }
        }
        Reset-Run
    }

    Check 'A plugin folder is granted for each skill by the skill''s own name, so a renamed plugin still works' {
        Reset-Run
        $pluginRoot = Join-Path $sandbox 'plugins'
        foreach ($pair in @(@('renamed-helix', 'pipeline-investigation'), @('moved-flow', 'flow-analysis'))) {
            $dir = Join-Path $pluginRoot "$($pair[0])\skills\$($pair[1])"; [void][IO.Directory]::CreateDirectory($dir)
            Set-Content -LiteralPath (Join-Path $dir 'SKILL.md') -Value '# skill'
        }
        [void][IO.Directory]::CreateDirectory((Join-Path $pluginRoot 'unrelated-plugin'))
        $script:config = [pscustomobject]@{ pluginRoot = $pluginRoot }
        $arguments = @(Get-CopilotArgs -Prompt 'x' -WorkDirectory 'C:\w')
        $granted = @(for ($i = 0; $i -lt $arguments.Count - 1; $i++) { if ($arguments[$i] -ceq '--add-dir') { $arguments[$i + 1] } })
        Assert ($granted -contains (Join-Path $pluginRoot 'renamed-helix') -and $granted -contains (Join-Path $pluginRoot 'moved-flow')) 'both skill holders are granted'
        Assert ($granted -notcontains (Join-Path $pluginRoot 'unrelated-plugin') -and $granted -notcontains $pluginRoot) 'nothing else in that folder is'
        Reset-Run
    }

    Check 'Coverage alerts become notes: authentication first and once, missing evidence first, background context as an ordinary note; a gap makes the run partial' {
        Reset-Run
        Add-QBCoverageNotes -Alerts @('Internal pipeline evidence is unavailable: Azure DevOps sign-in is required.', 'HTTP 401 from dnceng/internal.', '', '   ')
        $notes = @(Get-QBRunNotes)
        Assert ($notes.Count -eq 1 -and $notes[0].severity -eq 'first' -and $notes[0].key -eq 'internal-auth') 'every authentication alert is the one internal-auth note'
        Assert ($notes[0].message -match '^The internal builds are UNKNOWN, not healthy: Internal pipeline evidence' -and $notes[0].fix -eq 'az login --allow-no-subscriptions') 'it says UNKNOWN and how to fix it'
        Assert ($script:QBCoverageGap) 'and the run has a coverage gap'
        Reset-Run
        Add-QBCoverageNotes -Alerts @('context: the team roster is older than the Guide.', 'Issue/owner evidence is partial.', '3 additional issue/owner gaps.')
        Assert (@(Get-QBRunNotes).Count -eq 3 -and @(Get-QBRunNotes | Where-Object severity -eq 'first').Count -eq 0 -and -not $script:QBCoverageGap) 'background context is only a note and no gap'
        Reset-Run
        Add-QBCoverageNotes -Alerts @('Build 4002 has no timeline.')
        $notes = @(Get-QBRunNotes)
        Assert ($notes.Count -eq 1 -and $notes[0].severity -eq 'first' -and $notes[0].source -eq 'evidence' -and $script:QBCoverageGap) 'other missing evidence is first-severity and a gap'
        Reset-Run
        Add-QBCoverageNotes -Alerts @()
        Add-QBCoverageNotes
        Assert (@(Get-QBRunNotes).Count -eq 0) 'no alerts, no notes'
    }

    Check 'Preflight warnings become notes: internal authentication leads and is recorded once with its fix, the owner notes are not repeated, the rest are ordinary' {
        Reset-Run
        $preflight = [pscustomobject]@{ warnings = @(
                [pscustomobject]@{ id = 'az-token'; name = 'Azure CLI token'; detail = 'Token acquisition failed.'; remedy = 'az login --allow-no-subscriptions'; showFirst = $true }
                [pscustomobject]@{ id = 'azdo-internal'; name = 'dnceng/internal access'; detail = 'HTTP 401.'; remedy = 'az login'; showFirst = $true }
                [pscustomobject]@{ id = 'knowledge'; name = 'Routing registry loads'; detail = '12 teams; 2 owner note(s): x'; remedy = 'diff'; showFirst = $false }
                [pscustomobject]@{ id = 'copilot-cli'; name = 'Copilot CLI is installed'; detail = 'not on PATH'; remedy = 'Install it.'; showFirst = $false }) }
        Add-QBPreflightNotes -Preflight $preflight 3>$null
        $notes = @(Get-QBRunNotes)
        Assert ($notes.Count -eq 2) "two notes, got $($notes.Count): $(($notes | ForEach-Object message) -join ' | ')"
        Assert ($notes[0].key -eq 'internal-auth' -and $notes[0].severity -eq 'first' -and $notes[0].message -match 'UNKNOWN, not healthy - Azure CLI token: Token acquisition failed' -and $notes[0].fix -eq 'az login --allow-no-subscriptions') 'authentication first, once, with the first fix'
        Assert ($notes[1].source -eq 'preflight' -and $notes[1].message -match '^Copilot CLI is installed: not on PATH' -and $notes[1].fix -eq 'Install it.') 'an ordinary warning is an ordinary note'
        Assert ($script:QBCoverageGap) 'internal authentication is a coverage gap'
        Reset-Run
        $quiet = [pscustomobject]@{ warnings = @([pscustomobject]@{ id = 'knowledge'; name = 'Routing registry loads'; detail = '12 teams. Your local routing copy was made from an older knowledge/routing.json.'; remedy = 'diff'; showFirst = $false }) }
        Add-QBPreflightNotes -Preflight $quiet 3>$null
        Assert (@(Get-QBRunNotes).Count -eq 1 -and -not $script:QBCoverageGap) 'a stale local routing copy is one ordinary note'
        Reset-Run
        Add-QBPreflightNotes -Preflight ([pscustomobject]@{ warnings = @() })
        Assert (@(Get-QBRunNotes).Count -eq 0) 'no warnings, no notes'
    }

    Check 'A report that was skipped is a failed result with its reason, so the run goes on and the reason is told' {
        $result = New-QBSkippedReport -Name 'code-flow' -Reason 'The flow-analysis skill (the Maestro tools) is not installed.' 6>$null
        Assert ($result.name -eq 'code-flow' -and $result.status -eq 'failed' -and $result.path -eq $null -and $result.error -match 'flow-analysis skill' -and $result.seconds -eq 0) 'name, status, reason'
    }

    Check 'Finishing a document never throws and never withholds it: what cannot be verified comes back as problems, with the document as written' {
        $script:Date = '2026-10-09'
        $script:paths = [pscustomobject]@{ HistoryPath = (Join-Path $sandbox 'no-history.json'); ReportedLinksPath = (Join-Path $sandbox 'no-links.json') }
        $evidence = [pscustomobject]@{ pipelines = @(); requiredErrors = @() }
        $junk = "this is not a report at all`n"
        $health = Complete-QBReportDocument -Name 'build-health' -Candidate $junk -Evidence $evidence -IssueEvidence $null -InternalFindings $null -BuildHealthPath $null
        Assert ($health.Content -ceq $junk -and $null -eq $health.Payload -and @($health.Problems).Count -eq 1 -and $health.Problems[0] -match '^Build health: .*could not be verified') 'build health: the document as written, one problem'
        $overall = Complete-QBReportDocument -Name 'overall-summary' -Candidate $junk -Synthesis -Evidence $evidence -IssueEvidence $null -InternalFindings $null -BuildHealthPath $null
        Assert ($overall.Content -is [string] -and $overall.Content.Contains('this is not a report at all') -and @($overall.Problems).Count -ge 1) 'overall: a document and problems, never an exception'
        Assert (@($overall.Problems | Where-Object { $_ -match 'action plan could not be verified' }).Count -eq 1) 'the unverifiable action plan is one of them'
        $codeFlow = Complete-QBReportDocument -Name 'code-flow' -Candidate $junk -Evidence $evidence
        Assert ($codeFlow.Content -ceq $junk -and @($codeFlow.Problems).Count -eq 0) 'code flow has nothing to verify'
        $noEvidence = Complete-QBReportDocument -Name 'build-health' -Candidate $junk
        Assert ($noEvidence.Content -ceq $junk -and @($noEvidence.Problems).Count -eq 0) 'with no evidence there is nothing to verify against'
    }

    Check 'The runner wires these together: a skipped or failed overall report goes to the digest, and the exit code follows delivery' {
        $text = [IO.File]::ReadAllText((Join-Path $scripts 'daily\Invoke-Daily.ps1'))
        Assert ($text -match '(?s)if \(\$overallResult\.status -ne ''succeeded''\) \{.{0,200}New-QBEvidenceDigest') 'the digest is built exactly when the overall report is not produced'
        Assert ($text -match '\$delivered = \(\(\$succeeded\.Count -gt 0\) -or \[bool\]\$digestMarkdown\) -and \(\$SkipEmail -or \$emailSent\)') 'delivered = a report or the digest, and the email sent or skipped'
        Assert ($text -match '(?s)if \(-not \$delivered\) \{ exit 1 \}\s+exit 0') 'exit 1 only when nothing was delivered'
        Assert ($text -match 'Add-QBPreflightNotes -Preflight \$preflight' -and $text -notmatch '(?s)foreach \(\$warning in \$preflight\.warnings\)') 'preflight warnings go through the tested function'
        Assert ($text -match '(?s)exit 2' -and $text -match '(?s)exit 3') 'the preflight stop (2) and the busy claim (3) keep their codes'
    }
}
finally { Remove-Item -LiteralPath $sandbox -Recurse -Force -ErrorAction SilentlyContinue }

if ($failures) { Write-Host "$failures model-run test(s) failed." -ForegroundColor Red; exit 1 }
Write-Host 'All model-run tests passed.' -ForegroundColor Green
