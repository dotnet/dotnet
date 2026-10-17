<#
.SYNOPSIS
    VMR Quarterback (QB) daily monitoring - clean rewrite.

.DESCRIPTION
    Follows the VMR QB daily checklist (docs/VMR-QB-Guide.md) and produces three
    Markdown reports for a given day, then emails them:

      1. Build Health Monitoring   (pipeline-investigation skill)  -> build-health-<date>.md
      2. Code Flow Monitoring      (flow-analysis skill)           -> code-flow-<date>.md
      3. Overall QB report         (summarizes steps 1-4)          -> overall-summary-<date>.md

    The overall report contains a "Top actions for the QB" section and one or more
    summary tables with the columns:
        Issue category | Builds / PRs (links) | New Issue (Y/N) | Existing fix PR (status) | Action as VMR QB (who / where to ping) | Links that QB reported
    Rows are ordered so "New Issue = Y" appears first, then "?", then "N".
    The "Existing fix PR (status)" column cross-references each identified issue
    against the operator's known-fix triage context (triage-context.json) and a
    bounded GitHub PR search, so the report shows whether a fix already exists.
    The "Links that QB reported" column carries links to where the failure was already
    reported. The runner fills it deterministically from collected issue/comment
    evidence, merged with operator-supplied Teams permalinks from reported-links.json.
    Actions lead with evidence links and address one issue at a time. Each
    actionable table row names its destination and provides a quoted check
    request plus factual failure/status, without remediation instructions.
    That text is at most 30 words, excluding owners/links.

    Robustness (this is why the previous engine failed):
      * copilot stdout is streamed straight to a transcript file - it is NEVER
        materialized in memory (the old code piped a 1.37 GB stream through
        Out-String and hit OutOfMemory).
      * Each report has a hard per-report timeout; on timeout the copilot process
        TREE is killed so a hung generation cannot block the run for hours.
      * A per-report output size cap kills runaway generations.
      * Report generation failures are caught individually so the run always
        reaches the email step and sends what it has (or a failure notice).
      * Nothing stale, missing or damaged stops the run: internal sign-in, routing, an owner the Guide no longer lists, the
        config, the time zone, the history, the triage context, a missing skill or a refused model are run notes at the top
        of the overall report and the email, each with its fix (daily/notes.ps1). Only PowerShell, the state folder, the
        GitHub sign-in and a config holding your email address that Git tracks or does not ignore stop it. Without the model,
        an evidence digest (daily/digest.ps1) goes out instead of the narrative.
        Exit codes: 0 a report (or the digest) was delivered, 1 nothing was delivered or the run crashed, 2 preflight
        stop, 3 another run holds the day.

    Read-only: the reports never merge, approve, close, comment, retry, or post.
    To recover a failed generation, use -UseCollectedEvidence -SkipCodeFlow to
    preserve the dated cutoff and successful code-flow report. -OperatorContextPath
    accepts a local text file of scoped, verified operator corrections for both
    build-health and overall-summary generation.
    -UseExistingReports -RefreshReportReferences repairs BH cross-references from
    the visible Build Health action list without another monitoring generation.

    Configuration and state:
      * local/qb-config.local.json (optional, git-ignored; copy qb-config.template.json to create it: recipient,
        optional model) is read through Get-VmrQBConfigLenient: a problem with it is a note and never
        stops the run. An empty recipient means reports are saved and no mail is sent. The report date follows this
        computer's time zone, detected from the system (an optional timeZone in the file overrides it).
        A non-blank model is passed to copilot as --model.
      * Reports, logs, evidence artifacts, the issue history and reported-links.json
        live in the skill's git-ignored local/ folder (Get-VmrQBPaths), never beside these scripts.
        -OutputRoot re-roots a rehearsal: its reports, logs, artifacts, run folder and a private copy of the history live
        under that folder, so it cannot overwrite a delivered report or receipt or replay itself into the real history.
        It reads (never writes) the configuration, local knowledge and reported links, and it does not build the roster.
      * routing.json ships in the skill's knowledge directory. Verified findings and aliases are one QB's data:
        they live in the git-ignored local/ folder, are recorded with ./history/Invoke-Record.ps1, and travel in the handoff file.
        The team roster names people, so it is never shipped: `./common/Invoke-Contacts.ps1` builds it
        into local/ and the runner reuses it. copilot runs in its own per-run directory (<state>\run\<stamp>; the newest ten
        are kept). It holds the rendered references/runner-instructions.md as AGENTS.md, a copy of each reference document its
        stage is told to read (references/triage.md and report-contract.md) and the task documents, so nothing is shared
        between runs and nothing it must read lies outside the folders it was granted.
      * Structure: this file orchestrates. daily/prompts.ps1 builds the prompts, daily/contract.ps1 holds the report format
        validator, daily/workspace.ps1 stages the run directory, and the tests and Invoke-Report.ps1 load those directly.
        Dot-sourcing this file only defines functions.
#>
[CmdletBinding()]
param(
    [string] $Date,
    [int]    $TimeoutMinutes = 25,
    [int]    $MaxOutputMB = 250,
    [switch] $SkipBuildHealth,
    [switch] $SkipCodeFlow,
    [switch] $UseExistingReports,
    [switch] $RefreshReportReferences,
    [switch] $UseCollectedEvidence,
    [string] $OperatorContextPath,
    [string] $OutputRoot,
    [switch] $CollectOnly,
    [switch] $SkipEmail,
    [switch] $Force,
    [switch] $Scheduled,
    [switch] $ShowSummary
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

# Libraries only. They define functions and run nothing, so loading this file (the tests do) has no side effects until
# the guard below. Everything that reads state, writes a file or reaches the network is in the main section.
. (Join-Path $PSScriptRoot '..\common\core.ps1')
. (Join-Path $PSScriptRoot '..\history\history.ps1')
. (Join-Path $PSScriptRoot 'evidence.ps1')
. (Join-Path $PSScriptRoot 'report.ps1')
. (Join-Path $PSScriptRoot 'gate.ps1')
. (Join-Path $PSScriptRoot 'preflight.ps1')
. (Join-Path $PSScriptRoot 'contract.ps1')
. (Join-Path $PSScriptRoot 'prompts.ps1')
. (Join-Path $PSScriptRoot 'workspace.ps1')
. (Join-Path $PSScriptRoot 'digest.ps1')
. (Join-Path $PSScriptRoot '..\triage\issue.ps1')
# --------------------------------------------------------------------------- #
# Copilot argument profiles                                                    #
# --------------------------------------------------------------------------- #
function Get-CopilotArgs {
    param(
        [Parameter(Mandatory)][string] $Prompt,
        [switch] $Synthesis,
        [switch] $FormattingOnly,
        [switch] $EvidenceOnly,
        [string[]] $DataDirectory,
        # The per-run directory (New-QBRunWorkspace): the model starts there, and it holds its instructions, the staged
        # reference documents and the task document it is told to read.
        [Parameter(Mandatory)][string] $WorkDirectory
    )
    $a = [Collections.Generic.List[string]]::new()
    $a.AddRange([string[]]@(
        '-C', $WorkDirectory, '--prompt', $Prompt, '--silent', '--stream', 'off',
        '--output-format', 'json', '--no-color', '--no-ask-user',
        '--no-remote', '--no-remote-export', '--no-auto-update', '--deny-tool=write'
    ))
    # A pinned model the CLI refused is set aside for the rest of the run (Invoke-QBModelRun), so the CLI's own default answers.
    $model = if (Test-QBPinnedModelIgnored) { '' } else { [string](Get-VmrQBField $config 'model') }
    if (-not [string]::IsNullOrWhiteSpace($model)) { $a.Add('--model'); $a.Add($model.Trim()) }
    foreach ($n in @('task', 'task_complete', 'run_factory', 'factories_manage', 'manage_schedule', 'write_agent',
        'session_store_sql', 'sql', 'apply_patch', 'create', 'edit')) {
        $a.Add("--excluded-tools=$n")
    }
    foreach ($directory in @($DataDirectory)) { if ($directory) { $a.Add('--add-dir'); $a.Add($directory) } }
    if ($FormattingOnly) {
        foreach ($name in @('skill', 'powershell', 'web_search', 'web_fetch', 'list_resources', 'tool_search_tool')) {
            $a.Add("--excluded-tools=$name")
        }
        return $a.ToArray()
    }

    # Grant only the plugins this round names, each found where it is installed (the configured pluginRoot, then COPILOT_HOME
    # and ~/.copilot, in any marketplace folder). Naming them one by one, rather than a whole marketplace, keeps stale copies
    # of the same skills in other plugins (dotnet-dnceng once held June copies of pipeline-investigation/flow-analysis)
    # unreadable, so they cannot shadow the versions dotnet/arcade-skills publishes now.
    $pluginRoots = @(Get-VmrQBPluginSearchRoots -ConfiguredRoot (Get-VmrQBField $config 'pluginRoot'))
    $granted = [Collections.Generic.List[string]]::new()
    foreach ($plugin in @(Find-VmrQBPlugin -SearchRoot $pluginRoots)) { if ($plugin.Path -and $plugin.Path -notin $granted) { $granted.Add($plugin.Path) } }
    # A plugin that was renamed, or a skill that moved to another one, is still found by the skill's own name.
    foreach ($skill in @('pipeline-investigation', 'flow-analysis')) {
        $holder = Find-VmrQBSkillPlugin -SkillName $skill -SearchRoot $pluginRoots
        if ($holder -and $holder -notin $granted) { $granted.Add($holder) }
    }
    foreach ($directory in $granted) { $a.Add('--add-dir'); $a.Add($directory) }
    if ($EvidenceOnly) {
        foreach ($name in @('powershell', 'glob', 'web_search', 'web_fetch', 'list_resources', 'tool_search_tool')) {
            $a.Add("--excluded-tools=$name")
        }
        return $a.ToArray()
    }

    # Helix / AzDO investigation tools (Build Health).
    foreach ($tool in @(
        'azdo_auth_status', 'helix_ci_guide', 'azdo_builds', 'azdo_build',
        'azdo_timeline', 'azdo_search_timeline', 'azdo_build_analysis',
        'azdo_test_results', 'azdo_test_runs', 'azdo_search_log', 'azdo_log',
        'azdo_helix_jobs', 'helix_status', 'helix_batch_status', 'helix_search',
        'helix_logs', 'helix_work_item')) {
        $a.Add("--allow-tool=hlx($tool)"); $a.Add("--allow-tool=hlx(hlx-$tool)")
    }
    # Maestro codeflow tools (Code Flow).
    foreach ($tool in @(
        'maestro_codeflow_statuses', 'maestro_subscription_health', 'maestro_subscriptions',
        'maestro_subscription', 'maestro_subscription_history', 'maestro_subscription_outcomes',
        'maestro_build_freshness', 'maestro_builds', 'maestro_build', 'maestro_latest_build',
        'maestro_channels', 'maestro_channel', 'maestro_default_channels',
        'maestro_codeflow_pr', 'maestro_codeflow_prs', 'maestro_backflow_status')) {
        $a.Add("--allow-tool=maestro($tool)"); $a.Add("--allow-tool=maestro(maestro-$tool)")
    }
    foreach ($tool in @('get_file_contents', 'search_code')) {
        $a.Add("--allow-tool=github-mcp-server($tool)"); $a.Add("--allow-tool=github-mcp-server(github-mcp-server-$tool)")
    }
    # Read-only gh subcommands only. `gh api` is deliberately absent: the permission matches the subcommand,
    # not its flags (checked on Copilot CLI 1.0.89: with shell(gh api) allowed the model ran `gh api -X POST
    # markdown`), so allowing it would also allow `gh api -X POST` with the QB's credentials, and a model
    # steered by text in a public issue or PR could comment, close or edit. In 78 recorded generations the
    # model never issued one. The collector reads through the API itself (daily/evidence.ps1).
    foreach ($cmd in @('gh pr list', 'gh pr view', 'gh pr checks', 'gh pr diff',
                       'gh issue list', 'gh issue view', 'gh search issues', 'gh search prs')) {
        $a.Add("--allow-tool=shell($cmd)")
    }
    foreach ($domain in @('github.com', 'api.github.com', 'raw.githubusercontent.com', 'dev.azure.com',
                          'maestro.dot.net', 'helix.dot.net', 'aka.ms', 'repos.opensource.microsoft.com',
                          'login.microsoftonline.com')) {
        $a.Add("--allow-url=https://$domain")
    }
    return $a.ToArray()
}

# --------------------------------------------------------------------------- #
# Streaming JSONL reader - constant memory, extracts the final assistant answer #
# --------------------------------------------------------------------------- #
function Get-FinalAnswer {
    param([Parameter(Mandatory)][string] $Path)
    if (-not (Test-Path -LiteralPath $Path)) { throw "Copilot transcript not found: $Path" }
    $content = $null
    $ok = $false
    foreach ($line in [IO.File]::ReadLines($Path)) {
        if ([string]::IsNullOrWhiteSpace($line)) { continue }
        try { $e = $line.TrimStart([char]0xFEFF) | ConvertFrom-Json -Depth 100 } catch { continue }
        if ($e.type -eq 'result') {
            if ($e.PSObject.Properties['exitCode'] -and $e.exitCode -eq 0) { $ok = $true }
        }
        elseif ($e.type -eq 'assistant.message') {
            $c  = $e.data.PSObject.Properties['content']
            $ph = $e.data.PSObject.Properties['phase']
            if ($ph -and $ph.Value -and $ph.Value -ne 'final_answer') { continue }
            if ($c -and $c.Value -is [string] -and -not [string]::IsNullOrWhiteSpace($c.Value)) { $content = $c.Value }
        }
    }
    if (-not $ok)             { throw "Transcript has no successful CLI result (incomplete/terminated): $Path" }
    if ($null -eq $content)   { throw "Transcript has no final assistant report message: $Path" }
    return $content.Trim() + "`n"
}

# The model that actually answered. Without a pinned model the CLI's default is used and can change
# between CLI updates, so a report's quality can shift with no change here; recording it makes that visible.
function Get-QBTranscriptModel {
    param([string] $Path)
    if (-not $Path -or -not (Test-Path -LiteralPath $Path)) { return $null }
    # An explicit reader: returning early from a foreach over File.ReadLines leaves the handle open until GC.
    $reader = [IO.File]::OpenText($Path)
    try {
        while ($null -ne ($line = $reader.ReadLine())) {
            if ($line -match '"type":"assistant\.message".*?"model":"([^"]+)"') { return $Matches[1] }
        }
    }
    finally { $reader.Dispose() }
    return $null
}
# --------------------------------------------------------------------------- #
# Robust copilot launcher: stream to file + hard timeout + size cap + tree kill #
# --------------------------------------------------------------------------- #
function Start-CopilotReport {
    param(
        [Parameter(Mandatory)][string] $Prompt,
        [Parameter(Mandatory)][string] $TranscriptPath,
        [Parameter(Mandatory)][string] $ErrorPath,
        [switch] $Synthesis,
        [switch] $FormattingOnly,
        [switch] $EvidenceOnly,
        [string[]] $DataDirectory,
        [Parameter(Mandatory)][string] $WorkDirectory
    )
    $copilot = (Get-Command copilot -ErrorAction Stop).Source
    # The task document lives in the model's own run directory, not in logs\, which is outside everything it may read.
    $promptDirectory = Join-Path $WorkDirectory 'prompts'
    [void][IO.Directory]::CreateDirectory($promptDirectory)
    $promptPath = Join-Path $promptDirectory ((Split-Path -Leaf $TranscriptPath) + '.prompt.txt')
    Set-Content -LiteralPath $promptPath -Value $Prompt -Encoding utf8 -NoNewline
    $instruction = "Read the complete task document at $promptPath using view with forceReadLargeFiles=true. Follow that document and return only its requested report."
    if ($FormattingOnly) {
        $instruction += ' This is a document-repair task, not an investigation. After reading that document, use no tools, skills, research, or shell commands.'
    }
    $arguments = Get-CopilotArgs -Prompt $instruction -Synthesis:$Synthesis -FormattingOnly:$FormattingOnly -EvidenceOnly:$EvidenceOnly `
        -DataDirectory $DataDirectory -WorkDirectory $WorkDirectory

    $psi = [Diagnostics.ProcessStartInfo]::new()
    if ([IO.Path]::GetExtension($copilot) -ieq '.ps1') {
        $psi.FileName = (Get-Process -Id $PID).Path
        foreach ($a in @('-NoLogo', '-NoProfile', '-NonInteractive', '-File', $copilot)) { $psi.ArgumentList.Add($a) }
    }
    else { $psi.FileName = $copilot }
    foreach ($a in $arguments) { $psi.ArgumentList.Add($a) }
    $psi.RedirectStandardOutput = $true
    $psi.RedirectStandardError  = $true
    $psi.UseShellExecute        = $false
    $psi.CreateNoWindow         = $true
    if ($psi.Environment.ContainsKey('COPILOT_ALLOW_ALL')) { [void]$psi.Environment.Remove('COPILOT_ALLOW_ALL') }

    $proc  = [Diagnostics.Process]::new()
    $proc.StartInfo = $psi
    $outFs = [IO.File]::Create($TranscriptPath)
    $errFs = [IO.File]::Create($ErrorPath)
    [void]$proc.Start()
    # Drain both pipes on .NET threadpool threads straight to disk (constant memory).
    $budgetMinutes = if ($FormattingOnly) { [Math]::Min($TimeoutMinutes, 5) } else { $TimeoutMinutes }
    return [pscustomobject]@{
        process = $proc
        outTask = $proc.StandardOutput.BaseStream.CopyToAsync($outFs)
        errTask = $proc.StandardError.BaseStream.CopyToAsync($errFs)
        outStream = $outFs; errStream = $errFs
        transcriptPath = $TranscriptPath; errorPath = $ErrorPath
        budgetMinutes = $budgetMinutes
        startedAt = [DateTime]::UtcNow
        deadline = [DateTime]::UtcNow.AddMinutes($budgetMinutes)
    }
}

function Wait-CopilotReport {
    param([Parameter(Mandatory)][object] $Handle)

    $proc = $Handle.process
    $timedOut = $false
    $runaway  = $false
    try {
        $maxBytes = [long]$MaxOutputMB * 1MB
        while (-not $proc.WaitForExit(2000)) {
            if ([DateTime]::UtcNow -gt $Handle.deadline) { $timedOut = $true; break }
            try {
                $fi = [IO.FileInfo]::new($Handle.transcriptPath); $fi.Refresh()
                if ($fi.Length -gt $maxBytes) { $runaway = $true; break }
            } catch { }
        }
        if ($timedOut -or $runaway) {
            try { $proc.Kill($true) } catch { }
            [void]$proc.WaitForExit(30000)
        }
        try { [void]$Handle.outTask.Wait(20000) } catch { }
        try { [void]$Handle.errTask.Wait(20000) } catch { }
        $exit = if ($proc.HasExited) { $proc.ExitCode } else { -1 }
    }
    finally {
        $Handle.outStream.Dispose(); $Handle.errStream.Dispose(); $proc.Dispose()
    }
    if ($timedOut) { throw "Copilot exceeded the $($Handle.budgetMinutes)-minute limit and was terminated. Transcript: $($Handle.transcriptPath)" }
    if ($runaway)  { throw "Copilot output exceeded $MaxOutputMB MB (runaway) and was terminated. Transcript: $($Handle.transcriptPath)" }
    if ($exit -ne 0) { throw "Copilot exited with code $exit. Errors: $($Handle.errorPath)" }
    return Get-FinalAnswer -Path $Handle.transcriptPath
}

function Invoke-CopilotReport {
    param(
        [Parameter(Mandatory)][string] $Prompt,
        [Parameter(Mandatory)][string] $TranscriptPath,
        [Parameter(Mandatory)][string] $ErrorPath,
        [switch] $Synthesis,
        [switch] $FormattingOnly,
        [switch] $EvidenceOnly,
        [string[]] $DataDirectory,
        [Parameter(Mandatory)][string] $WorkDirectory
    )
    return Wait-CopilotReport -Handle (Start-CopilotReport -Prompt $Prompt -TranscriptPath $TranscriptPath `
        -ErrorPath $ErrorPath -Synthesis:$Synthesis -FormattingOnly:$FormattingOnly `
        -EvidenceOnly:$EvidenceOnly -DataDirectory $DataDirectory -WorkDirectory $WorkDirectory)
}

# --------------------------------------------------------------------------- #
# Team roster accessor                                                          #
# --------------------------------------------------------------------------- #
function Get-QBTeamContacts {
    <#
    .SYNOPSIS
    Null-safe accessor for the team -> named contact roster.
    .DESCRIPTION
    Returns an empty hashtable when the roster is unavailable (for example when a test
    extracts these functions in isolation), so a missing roster degrades to "no contact
    enforcement" instead of throwing under StrictMode. Reads .Value rather than using
    Get-Variable -ValueOnly, which collapses a collection into a single object.
    #>
    $variable = Get-Variable -Name teamContacts -Scope Script -ErrorAction SilentlyContinue
    if ($null -eq $variable -or $null -eq $variable.Value) { return @{} }
    return $variable.Value
}

# Script settings are read through Get-Variable, so a caller that only loads these functions (the tests do) does not need them set.
function Test-QBPinnedModelIgnored {
    # True once the CLI refused the pinned model (Invoke-QBModelRun): the CLI's own default answers for the rest of the run.
    $variable = Get-Variable -Name QBIgnorePinnedModel -Scope Script -ErrorAction SilentlyContinue
    return ($null -ne $variable -and [bool]$variable.Value)
}

function Get-QBEmailRetryPolicy {
    $attempts = Get-Variable -Name QBEmailAttempts -Scope Script -ErrorAction SilentlyContinue
    $delay = Get-Variable -Name QBEmailRetryDelaySeconds -Scope Script -ErrorAction SilentlyContinue
    return [pscustomobject]@{ Attempts = $(if ($attempts) { [int]$attempts.Value } else { 3 }); DelaySeconds = $(if ($delay) { [int]$delay.Value } else { 30 }) }
}

# --------------------------------------------------------------------------- #
# One model run, tried once more when it fails for a reason that is not the clock #
# --------------------------------------------------------------------------- #
function Test-QBPinnedModelRejected {
    # True when the CLI's error output says the model it was asked for does not exist or is not available.
    param([string] $ErrorPath)
    if (-not $ErrorPath -or -not (Test-Path -LiteralPath $ErrorPath -PathType Leaf)) { return $false }
    try { $text = [IO.File]::ReadAllText($ErrorPath) } catch { return $false }
    if ($text.Length -gt 20000) { $text = $text.Substring($text.Length - 20000) }
    return ($text -match '(?i)\bmodel\b[^\r\n]{0,120}\b(not (found|available|supported|enabled)|unknown|invalid|unsupported|does not exist|no access|deprecated|retired)\b' -or
        $text -match '(?i)\b(unknown|invalid|unsupported|unavailable) model\b')
}

function Invoke-QBModelRun {
    <#
    .SYNOPSIS
    One model run. A failure that is not a time limit is tried once more; when the pinned model was refused, without --model.
    .DESCRIPTION
    A run that ends abnormally (the time limit, a CLI error on the way out) may already have written its final answer. With
    -ExpectedTitle, that last complete document is used (and noted) instead of losing the report; without a document in the
    transcript the failure stands.
    #>
    param(
        [Parameter(Mandatory)][string] $Name, [Parameter(Mandatory)][string] $Prompt, [Parameter(Mandatory)][string] $Transcript,
        [Parameter(Mandatory)][string] $ErrPath, [switch] $Synthesis, [switch] $EvidenceOnly, [string[]] $DataDirectory, [object] $Handle,
        [string] $ExpectedTitle
    )
    $attempt = 0
    while ($true) {
        $transcriptPath = if ($attempt) { "$Transcript.retry.jsonl" } else { $Transcript }
        $errorPath = if ($attempt) { "$ErrPath.retry.log" } else { $ErrPath }
        try {
            $text = if ($attempt -eq 0 -and $null -ne $Handle) { Wait-CopilotReport -Handle $Handle }
            else {
                Invoke-CopilotReport -Prompt $Prompt -TranscriptPath $transcriptPath -ErrorPath $errorPath -Synthesis:$Synthesis `
                    -EvidenceOnly:$EvidenceOnly -DataDirectory $DataDirectory -WorkDirectory $runWorkspace.Path
            }
            return [pscustomobject]@{ Text = $text; Transcript = $transcriptPath }
        }
        catch {
            $message = $_.Exception.Message
            # What the run wrote before it ended is used when it is a complete document: a retry or a lost report costs more.
            if ($ExpectedTitle) {
                $usedTranscript = if ($null -ne $Handle -and $attempt -eq 0) { [string]$Handle.transcriptPath } else { $transcriptPath }
                $salvaged = $null
                try { $salvaged = Get-QBSalvagedAnswer -Path $usedTranscript -Title $ExpectedTitle } catch { $salvaged = $null }
                if ($salvaged) {
                    Add-QBRunNote -Source 'model' -Severity first -Message "The $Name model run ended abnormally ($message) after it had written the report, so that report was used." -Fix 'Read the report more closely than usual; it did not finish cleanly.'
                    return [pscustomobject]@{ Text = $salvaged; Transcript = $usedTranscript }
                }
            }
            # A time limit or runaway output is not tried again: it would spend the same time for the same result.
            if ($attempt -ge 1 -or $message -match 'exceeded the \d+-minute limit|runaway') { throw }
            $failedErrors = if ($null -ne $Handle -and $attempt -eq 0) { [string]$Handle.errorPath } else { $errorPath }
            $pinned = [string](Get-VmrQBField $config 'model')
            if (-not (Test-QBPinnedModelIgnored) -and -not [string]::IsNullOrWhiteSpace($pinned) -and (Test-QBPinnedModelRejected -ErrorPath $failedErrors)) {
                $script:QBIgnorePinnedModel = $true
                Add-QBRunNote -Source 'model' -Message "The pinned model '$($pinned.Trim())' was refused, so the Copilot CLI's default model was used." -Fix 'Clear or change model in local/qb-config.local.json.'
            }
            Write-Warning "${Name}: the model run failed ($message); trying once more."
            $attempt++
        }
    }
}

# --------------------------------------------------------------------------- #
# Finishing a model document: checks that add notes, never a rejection         #
# --------------------------------------------------------------------------- #
function Complete-QBReportDocument {
    <#
    .SYNOPSIS
    Verifies and finishes a model document. It never throws and never withholds the document.
    .DESCRIPTION
    What is wrong comes back in Problems (they become run notes at the top of the report) and whatever could not be verified is left
    out: without a verified findings payload the cross-references and the internal appendix are not generated; without a verified
    action plan the action drafts are not generated. Content is the text to save and Payload the verified structured data.
    #>
    param(
        [Parameter(Mandatory)][string] $Name, [Parameter(Mandatory)][string] $Candidate, [switch] $Synthesis,
        [object] $Evidence, [object] $IssueEvidence, [object] $InternalFindings, [string] $BuildHealthPath
    )
    $content = $Candidate
    $payload = $null
    $problems = [Collections.Generic.List[string]]::new()
    if ($null -ne $Evidence -and $Name -eq 'build-health') {
        try {
            $section = Read-QBStructuredSection -Content $Candidate -Marker 'VMR-INTERNAL-FINDINGS'
            if ($section.Content -notmatch ('(?m)^# Build Health Monitoring - ' + [regex]::Escape($Date) + '[ \t]*\r?$')) {
                throw 'The document has the wrong dated title.'
            }
            $section.Data | Add-Member -NotePropertyName reportDate -NotePropertyValue $Date -Force
            $collectorStatus = (Get-QBInternalPipeline $Evidence).status
            if ((Get-QBField $section.Data 'sourceStatus') -ne $collectorStatus) {
                Write-Warning "Using collector-owned internal coverage status '$collectorStatus'; the generated metadata did not match. Diagnostic quotes and build coverage still require validation."
                $section.Data | Add-Member -NotePropertyName sourceStatus -NotePropertyValue $collectorStatus -Force
            }
            # Get-Variable -ValueOnly emits an array as a single object, so @() would
            # wrap it into one bogus "System.Object[]" entry. Read .Value directly.
            $knownTeamsVariable = Get-Variable -Name QBKnownOwnerTeams -Scope Script -ErrorAction SilentlyContinue
            $knownTeams = if ($null -ne $knownTeamsVariable) { @($knownTeamsVariable.Value) } else { @() }
            Assert-QBInternalFindings -Findings $section.Data -Evidence $Evidence -Date $Date -KnownOwnerTeams $knownTeams
            $verified = Set-VmrQBBuildHealthReferences -Findings $section.Data -Markdown $section.Content
            $content = $section.Content + "`n" + (ConvertTo-QBInternalSection -Evidence $Evidence -Findings $verified)
            $payload = $verified
        }
        catch {
            $problems.Add("Build health: $($_.Exception.Message) The internal findings could not be verified, so the BH cross-references and the internal appendix were not generated.")
            $content = $Candidate
            $payload = $null
        }
    }
    if ($Synthesis) {
        $plan = $null
        if ($null -ne $Evidence) {
            try {
                $section = Read-QBStructuredSection -Content $Candidate -Marker 'VMR-QB-ACTIONS'
                $trackers = Invoke-QBOptional -Name 'The issue history (known trackers)' -Default @{} -Fix 'Fix or move local/issue-history.json; without it the report cannot tell which findings already have an owner.' -Script {
                    Get-VmrKnownTrackerMap -Path $paths.HistoryPath
                }
                Assert-QBDailyActions -Plan $section.Data -Findings $InternalFindings -IssueEvidence $IssueEvidence -Date $Date -KnownTrackers $trackers
                $plan = $section.Data
                $content = $section.Content
            }
            catch {
                $problems.Add("Overall report: $($_.Exception.Message) The action plan could not be verified, so the action drafts were not generated.")
                $content = $Candidate
            }
        }
        try {
            $content = ConvertTo-QBActionFormat -Content $content -Evidence $Evidence -IssueEvidence $IssueEvidence -InternalFindings $InternalFindings `
                -OperatorLinksPath $paths.ReportedLinksPath
        }
        catch { $problems.Add("Overall report: the action table could not be normalized ($($_.Exception.Message)) and was left as the model wrote it.") }
        if ($null -ne $plan) {
            try {
                $content += "`n" + (ConvertTo-QBDailyActionSection -Plan $plan -Findings $InternalFindings)
                $payload = $plan
            }
            catch { $problems.Add("Overall report: the action drafts could not be rendered ($($_.Exception.Message)) and were left out.") }
        }
        # The checks. What they find is a note at the top of the report; the report is never withheld because of one.
        try {
            $findings = Invoke-QBOverallReportGate -Content $content -Date $Date -TeamContacts (Get-QBTeamContacts) -HistoryPath $paths.HistoryPath `
                -BuildHealthActions (Get-QBVisibleBuildHealthActions -BuildHealthPath $BuildHealthPath)
            foreach ($finding in @($findings | Where-Object { $_.severity -eq 'fail' })) { $problems.Add("Report check: $($finding.message)") }
        }
        catch { $problems.Add("The report checks could not run: $($_.Exception.Message)") }
    }
    return [pscustomobject]@{ Content = $content; Payload = $payload; Problems = @($problems) }
}

# --------------------------------------------------------------------------- #
# One report generation with transcript logging and independent failure capture #
# --------------------------------------------------------------------------- #
function New-Report {
    param(
        [Parameter(Mandatory)][string] $Name,
        [Parameter(Mandatory)][string] $Prompt,
        [Parameter(Mandatory)][string] $OutputPath,
        [switch] $Synthesis,
        [object] $Evidence,
        [object] $IssueEvidence,
        [object] $InternalFindings,
        [string[]] $DataDirectory,
        [string] $ModelEvidencePath,
        [object] $Handle,
        # The build-health report whose visible action list every BH number in the overall report must come from.
        [string] $BuildHealthPath
    )
    $transcript = Join-Path $logDir "copilot-$Name-$Date-$stamp.jsonl"
    $errPath    = Join-Path $logDir "copilot-$Name-$Date-$stamp.stderr.log"
    Write-Host "[$([DateTime]::Now.ToString('HH:mm:ss'))] $(if ($null -ne $Handle) { "Awaiting $Name report (started earlier)" } else { "Generating $Name report" })..." -ForegroundColor Cyan
    $sw = [Diagnostics.Stopwatch]::StartNew()
    $startedAt = if ($null -ne $Handle) { $Handle.startedAt } else { [DateTime]::UtcNow }
    try {
        if ($null -ne $Handle -and $Handle.transcriptPath -ne $transcript) {
            throw "Pre-started $Name generation does not match its expected transcript path."
        }
        $run = Invoke-QBModelRun -Name $Name -Prompt $Prompt -Transcript $transcript -ErrPath $errPath -Synthesis:$Synthesis `
            -EvidenceOnly:($Name -eq 'build-health' -and $null -ne $Evidence) -DataDirectory $DataDirectory -Handle $Handle `
            -ExpectedTitle (Get-QBReportTitle -Name $Name -Date $Date)
        $usedTranscript = $run.Transcript
        $finish = @{
            Name = $Name; Synthesis = $Synthesis; Evidence = $Evidence; IssueEvidence = $IssueEvidence
            InternalFindings = $InternalFindings; BuildHealthPath = $BuildHealthPath
        }
        $outcome = Complete-QBReportDocument -Candidate $run.Text @finish
        # One document-only repair when something could not be verified, then whatever is best is saved with its notes. Whatever goes
        # wrong in the repair (the prompt, the model, the checks) leaves the original document in place: it is never traded for a worse one.
        if ($outcome.Problems.Count -and $null -ne $Evidence) {
            try {
                Set-Content -LiteralPath "$transcript.invalid.md" -Value $run.Text -Encoding utf8
                $validationError = $outcome.Problems -join ' '
                Write-Warning "$Name could not be fully verified; attempting one document-only repair: $validationError"
                $repairEvidence = if ($ModelEvidencePath -and (Test-Path -LiteralPath $ModelEvidencePath)) {
                    Get-Content -LiteralPath $ModelEvidencePath -Raw
                } else { 'No additional evidence is available beyond the existing document.' }
                $repairPrompt = @"
This is a bounded document-repair task, not a monitoring investigation.
Use no tools, skills, shell, or research after reading this instruction document.
Return the complete corrected document and its required JSON comment. Preserve all
observed facts, source URLs, diagnostic quotes, uncertainty, and the observation cutoff.
Do not add facts you cannot support from the existing document. Fix formatting or
omit an unverified individual assignee instead of guessing. If evidence is genuinely
missing, keep the coverage gap explicit. There will not be another repair attempt.

VALIDATION ERROR:
$validationError

ORIGINAL OUTPUT REQUIREMENTS:
$Prompt

RETAINED COMPACT EVIDENCE (use these exact diagnostics, not guessed checkpoint facts):
$repairEvidence

DOCUMENT:
$($run.Text)
"@
                $repaired = Invoke-CopilotReport -Prompt $repairPrompt -TranscriptPath "$transcript.repair.jsonl" `
                    -ErrorPath "$errPath.repair.log" -Synthesis:$Synthesis -FormattingOnly -DataDirectory $DataDirectory -WorkDirectory $runWorkspace.Path
                $original = $outcome
                $outcome = Select-QBBetterOutcome -Original $original -Repaired (Complete-QBReportDocument -Candidate $repaired @finish)
                if ([object]::ReferenceEquals($outcome, $original)) {
                    Add-QBRunNote -Source 'report-check' -Message "The repair pass for the $Name report did not improve it, so the original document is kept."
                }
            }
            catch { Add-QBRunNote -Source 'report-check' -Message "The repair pass for the $Name report failed ($($_.Exception.Message)), so the original document is kept." }
        }
        foreach ($problem in $outcome.Problems) { Add-QBRunNote -Source 'report-check' -Severity first -Message $problem }
        Set-Content -LiteralPath $OutputPath -Value $outcome.Content -Encoding utf8
        # From here the report is saved and delivered: nothing below may turn it into a failed report.
        if ($null -ne $outcome.Payload) {
            try { ConvertTo-Json -InputObject $outcome.Payload -Depth 100 | Set-Content -LiteralPath "$OutputPath.data.json" -Encoding utf8 }
            catch { Add-QBRunNote -Source 'report-check' -Message "The structured data of the $Name report could not be saved ($($_.Exception.Message)); the report itself is saved." }
        }
        $sw.Stop()
        $elapsed = ([DateTime]::UtcNow - $startedAt).TotalSeconds
        $modelUsed = $null
        $sizeKb = 0
        try {
            $modelUsed = Get-QBTranscriptModel -Path $usedTranscript
            $pinnedModel = ''
            $configVariable = Get-Variable -Name config -Scope Script -ErrorAction SilentlyContinue
            if ($null -ne $configVariable -and $null -ne $configVariable.Value -and $configVariable.Value.PSObject.Properties['model']) { $pinnedModel = [string]$configVariable.Value.model }
            if ($modelUsed -and -not [string]::IsNullOrWhiteSpace($pinnedModel) -and -not (Test-QBPinnedModelIgnored) -and $modelUsed -notlike "*$($pinnedModel.Trim())*") {
                Write-Warning "$Name was generated by '$modelUsed' although qb-config.json pins '$($pinnedModel.Trim())'."
            }
            $sizeKb = (Get-Item -LiteralPath $OutputPath).Length / 1KB
        }
        catch { Write-Verbose "The model name and size of the $Name report were not read: $($_.Exception.Message)" }
        Write-Host ("  -> {0} ({1:n0}s, {2:n0} KB{3})" -f (Split-Path $OutputPath -Leaf), $elapsed, $sizeKb, $(if ($modelUsed) { ", $modelUsed" } else { '' })) -ForegroundColor Green
        return [pscustomobject]@{ name = $Name; status = 'succeeded'; path = $OutputPath; error = $null; seconds = [int]$elapsed; payload = $outcome.Payload; model = $modelUsed }
    }
    catch {
        $sw.Stop()
        $elapsed = ([DateTime]::UtcNow - $startedAt).TotalSeconds
        Write-Host "  -> FAILED: $($_.Exception.Message)" -ForegroundColor Red
        return [pscustomobject]@{ name = $Name; status = 'failed'; path = $null; error = $_.Exception.Message; seconds = [int]$elapsed }
    }
}

# --------------------------------------------------------------------------- #
# Notes and notices                                                           #
# --------------------------------------------------------------------------- #
function Add-QBCoverageNotes {
    <#
    .SYNOPSIS
    Turns coverage alerts into run notes. Internal authentication comes first, in red, and is recorded once.
    #>
    param([AllowEmptyCollection()][string[]] $Alerts = @())
    foreach ($alert in @($Alerts)) {
        if ([string]::IsNullOrWhiteSpace($alert)) { continue }
        if ($alert -match '(?i)authenticat|sign[ -]?in|az login|token acquisition|HTTP 40[13]') {
            $script:QBCoverageGap = $true
            Add-QBRunNote -Source 'internal-auth' -Key 'internal-auth' -Severity first -Message "The internal builds are UNKNOWN, not healthy: $alert" -Fix 'az login --allow-no-subscriptions'
        }
        elseif ($alert -match '^(context|team-|branches|release-schedule)\b' -or $alert -match '^Issue/owner evidence|^\d+ additional issue/owner') {
            Add-QBRunNote -Source 'evidence' -Message $alert
        }
        else {
            $script:QBCoverageGap = $true
            Add-QBRunNote -Source 'evidence' -Severity first -Message $alert
        }
    }
}

function Add-QBPreflightNotes {
    <#
    .SYNOPSIS
    Turns preflight warnings into run notes. Internal authentication (showFirst) leads, in red, and is recorded once.
    #>
    param([Parameter(Mandatory)][object] $Preflight)
    foreach ($warning in @($Preflight.warnings)) {
        Write-Warning "$($warning.name): $($warning.detail)"
        # The routing load's own notes are added from the routing object in the main section.
        if ($warning.id -eq 'knowledge' -and $warning.detail -match 'owner note\(s\)') { continue }
        if ([bool]$warning.showFirst) {
            $script:QBCoverageGap = $true
            Add-QBRunNote -Source 'internal-auth' -Key 'internal-auth' -Severity first -Fix $warning.remedy `
                -Message "The internal builds are UNKNOWN, not healthy - $($warning.name): $($warning.detail)"
        }
        else { Add-QBRunNote -Source 'preflight' -Fix $warning.remedy -Message "$($warning.name): $($warning.detail)" }
    }
}

function New-QBSkippedReport {
    param([Parameter(Mandatory)][string] $Name, [Parameter(Mandatory)][string] $Reason)
    Write-Host "  -> $Name report not generated: $Reason" -ForegroundColor Yellow
    return [pscustomobject]@{ name = $Name; status = 'failed'; path = $null; error = $Reason; seconds = 0 }
}

function Invoke-QBStage {
    <#
    .SYNOPSIS
    Runs one report stage. Whatever it throws, the report is a failed result with the reason; the run goes on with the next stage.
    .DESCRIPTION
    New-Report already turns a failed model run into a failed result. This covers everything around it (building the prompt, reading
    the saved context), so one unexpected error in one report can never stop the other two, the digest or the email.
    #>
    param([Parameter(Mandatory)][string] $Name, [Parameter(Mandatory)][scriptblock] $Script)
    try { return (& $Script) }
    catch {
        Write-Host "  -> FAILED: $($_.Exception.Message)" -ForegroundColor Red
        return [pscustomobject]@{ name = $Name; status = 'failed'; path = $null; error = "The $Name stage stopped unexpectedly: $($_.Exception.Message)"; seconds = 0 }
    }
}

function Send-QBNoticeEmail {
    <#
    .SYNOPSIS
    A short email that says the run did not start or did not finish, and why, with whatever reports exist attached. One per day.
    #>
    param(
        [Parameter(Mandatory)][string] $Heading, [Parameter(Mandatory)][string] $Intro,
        [AllowEmptyCollection()][object[]] $Items = @(), [string[]] $Attachments = @(),
        [Parameter(Mandatory)][string] $ReceiptName, [Parameter(Mandatory)][string] $SubjectSuffix
    )
    $listed = foreach ($item in @($Items)) {
        "<li><b>$([Net.WebUtility]::HtmlEncode([string]$item.name))</b>: $([Net.WebUtility]::HtmlEncode([string]$item.detail))" +
        $(if ($item.remedy) { "<br/>Fix: <code>$([Net.WebUtility]::HtmlEncode([string]$item.remedy))</code>" } else { '' }) + '</li>'
    }
    $body = "<html><body style='font-family:Segoe UI,Arial,sans-serif'><h3>$([Net.WebUtility]::HtmlEncode($Heading))</h3><p>$([Net.WebUtility]::HtmlEncode($Intro))</p><ul>$($listed -join '')</ul>$(Format-QBRunNotesHtml)<p>Run <code>./daily/Invoke-Preflight.ps1</code> to re-check.</p></body></html>"
    [void](Send-QBEmail -Attachments $Attachments -HtmlBody $body -ReceiptPath (Join-Path $logDir $ReceiptName) -RunStatus 'failed' -SubjectSuffix $SubjectSuffix)
}

# --------------------------------------------------------------------------- #
# The triage context every reader shares                                       #
# --------------------------------------------------------------------------- #
function Resolve-QBTriageContext {
    <#
    .SYNOPSIS
    Writes the one triage-context file all readers use (this QB's verified findings) and returns its path.
    .DESCRIPTION
    The file goes beside the run's evidence directory, never inside it: the collector insists that directory is new or empty, so a
    file placed there before collection makes the whole collection fail. A local file that does not parse is skipped with a note
    and left as it is; with nothing readable the context is simply empty.
    #>
    param(
        [Parameter(Mandatory)][string] $ArtifactDir, [Parameter(Mandatory)][string] $Date, [Parameter(Mandatory)][string] $Stamp,
        [string] $LocalContextPath
    )
    $outputPath = Join-Path (Join-Path $ArtifactDir $Date) "$Stamp.triage-context.merged.json"
    [void][IO.Directory]::CreateDirectory((Split-Path -Parent $outputPath))
    $path = Invoke-QBOptional -Name 'Your triage context (local/knowledge/triage-context.local.json)' -Default $null -Fix 'Fix or move that file; this run used no local triage context.' -Script {
        Merge-VmrQBTriageContext -LocalPath $LocalContextPath -OutputPath $outputPath
    }
    if (-not $path) {
        $path = Invoke-QBOptional -Name 'The triage context' -Default $null -Script { Merge-VmrQBTriageContext -LocalPath $null -OutputPath $outputPath }
    }
    if (-not $path) {
        Write-VmrQBJsonFile -Path $outputPath -Value ([pscustomobject]@{ schemaVersion = 1; entries = @() })
        $path = $outputPath
    }
    return $path
}

# --------------------------------------------------------------------------- #
# Simple, reliable Outlook send                                               #
# --------------------------------------------------------------------------- #
# A send that did not go out (nothing was submitted) is tried again after a short wait, because Outlook is often just busy or
# still starting. A send that may have been delivered is never repeated: the claim stays until a QB asks for a resend (-Force).

function Send-QBEmail {
    param(
        [Parameter(Mandatory)][AllowEmptyCollection()][string[]] $Attachments,
        [Parameter(Mandatory)][string] $HtmlBody,
        [Parameter(Mandatory)][string] $ReceiptPath,
        [Parameter(Mandatory)][ValidateSet('complete', 'partial', 'failed')][string] $RunStatus,
        [string] $SubjectSuffix = '',
        [switch] $Force
    )
    $subject = "[VMR QB] Daily monitoring - $Date$SubjectSuffix"
    # An attachment that is not there is left out and noted; it never costs the email (and so the reports that are there).
    $present = [Collections.Generic.List[string]]::new()
    foreach ($path in @($Attachments | Where-Object { $_ })) {
        if (Test-Path -LiteralPath $path -PathType Leaf) { $present.Add($path) }
        else {
            Write-Warning "Report attachment is missing and was left out: $path"
            Add-QBRunNote -Source 'email' -Severity first -Message "A report attachment is missing and was left out of the email: $path"
        }
    }
    $Attachments = $present.ToArray()
    # Claim the send before Outlook is asked to do it (atomic; see Enter-QBEmailClaim). A send that fails before anything is
    # submitted gives the claim back; one that may have been delivered keeps it, so a day is never mailed twice by accident.
    $claimPath = [IO.Path]::ChangeExtension($ReceiptPath, 'claim')
    Enter-QBEmailClaim -ClaimPath $claimPath -ReceiptPath $ReceiptPath -Subject $subject -Force:$Force
    $submitted = $false
    $lastError = $null
    $policy = Get-QBEmailRetryPolicy
    for ($attempt = 1; $attempt -le $policy.Attempts; $attempt++) {
        $com = [Collections.Generic.List[object]]::new()
        try {
            $outlook = New-Object -ComObject Outlook.Application; $com.Add($outlook)
            $session = $outlook.Session; $com.Add($session)
            if ($session.Offline) { throw 'Outlook is offline; the monitoring email was not submitted.' }
            $accounts = $session.Accounts; $com.Add($accounts)
            $accountList = @(for ($i = 1; $i -le $accounts.Count; $i++) { $c = $accounts.Item($i); $com.Add($c); $c })
            # The reports are sent from the Outlook account that matches a recipient (the first one listed that has an account).
            $recipients = @(Get-VmrQBRecipientList $config.recipient)
            $account = $null
            foreach ($recipient in $recipients) {
                $account = @($accountList | Where-Object { $_.SmtpAddress -ieq $recipient }) | Select-Object -First 1
                if ($null -ne $account) { break }
            }
            if ($null -eq $account) { throw "Outlook has no account matching any recipient ($($recipients -join ', ')); the reports are sent from the account that matches one of them, so at least one recipient must be your own address." }
            $store = $account.DeliveryStore; $com.Add($store)
            $sentFolder = $store.GetDefaultFolder(5); $com.Add($sentFolder)

            $mail = $outlook.CreateItem(0); $com.Add($mail)
            $mail.SendUsingAccount = $account
            $mail.SaveSentMessageFolder = $sentFolder
            $mail.To = $recipients -join '; '
            $mail.Subject = $subject
            $mail.HTMLBody = $HtmlBody
            foreach ($att in $Attachments) {
                [void]$mail.Attachments.Add($att)
            }
            $mail.Send()
            $submitted = $true
            $receipt = [ordered]@{
                status = 'sent'; subject = $subject; recipient = $config.recipient
                runStatus = $RunStatus; attachments = @($Attachments | ForEach-Object { Split-Path $_ -Leaf })
                sentAt = [DateTimeOffset]::Now.ToString('o')
            }
            $receipt | ConvertTo-Json -Depth 5 | Set-Content -LiteralPath $ReceiptPath -Encoding utf8
            Write-Host "  -> email sent to $($recipients -join '; ') ($($Attachments.Count) attachment(s))" -ForegroundColor Green
            return $true
        }
        catch {
            if ($submitted) { throw }
            $lastError = $_
            if ($attempt -lt $policy.Attempts) {
                Write-Warning "The email was not submitted (attempt $attempt of $($policy.Attempts): $($_.Exception.Message)); trying again in $($policy.DelaySeconds) seconds."
                if ($policy.DelaySeconds -gt 0) { Start-Sleep -Seconds $policy.DelaySeconds }
            }
        }
        finally {
            foreach ($o in $com) { if ($null -ne $o) { try { [void][Runtime.InteropServices.Marshal]::ReleaseComObject($o) } catch { Write-Verbose "Not a COM object: $($_.Exception.Message)" } } }
            [GC]::Collect(); [GC]::WaitForPendingFinalizers()
        }
    }
    Clear-QBEmailClaim -ClaimPath $claimPath
    throw $lastError
}

# --------------------------------------------------------------------------- #
# Dot-sourcing this file only defines the functions above (the tests do that).  #
# --------------------------------------------------------------------------- #
if ($MyInvocation.InvocationName -eq '.') { return }

# --------------------------------------------------------------------------- #
# Setup: resolve everything this run needs to know. Nothing is created here.   #
# --------------------------------------------------------------------------- #
$runStartedUtc = [DateTime]::UtcNow
$script:QBStage = 'setup'
# True when something the report depends on could not be seen (the internal builds, missing evidence): the run is then partial.
$script:QBCoverageGap = $false
# Set when the pinned model was refused and the CLI's own default was used instead.
$script:QBIgnorePinnedModel = $false
Clear-QBRunNotes
$paths     = Get-VmrQBPaths
# Nothing in the config can stop a run: it says where the email goes and which time zone the date follows. A problem is a note at
# the top of the report (and a warning here) and the default stands in for it.
$configResult = Get-VmrQBConfigLenient -Path $paths.ConfigPath
$config    = $configResult.Config
foreach ($problem in @($configResult.Problems)) {
    Write-Warning $problem
    Add-QBRunNote -Source 'config' -Severity first -Message $problem
}
# An empty recipient in qb-config.json means: save the reports, send no email.
if (-not $SkipEmail -and -not $CollectOnly -and [string]::IsNullOrWhiteSpace([string]$config.recipient)) {
    if (-not @($configResult.Problems).Count) {
        Write-Host "No recipient configured: reports are saved to $($paths.ReportsDir) and not emailed. To get them by email, copy qb-config.template.json to $($paths.ConfigPath) and set ""recipient"" (see README.md)." -ForegroundColor Yellow
    }
    $SkipEmail = [switch]::new($true)
}
$operatorContext = if ($OperatorContextPath) {
    Invoke-QBOptional -Name "The operator context file ($OperatorContextPath)" -Default '' -Fix 'Check the -OperatorContextPath you passed.' -Script {
        Get-Content -LiteralPath $OperatorContextPath -Raw -ErrorAction Stop
    }
} else { '' }
$zoneResult = Resolve-VmrQBTimeZone -Id ([string](Get-VmrQBField $config 'timeZone' '')) -ConfigPath $paths.ConfigPath
if ($zoneResult.Problem) { Write-Warning $zoneResult.Problem; Add-QBRunNote -Source 'config' -Message $zoneResult.Problem }
$zone      = $zoneResult.Zone
$windowEnd = [DateTimeOffset]::UtcNow
$localNow  = [TimeZoneInfo]::ConvertTime($windowEnd, $zone)
if (-not $Date) { $Date = $localNow.ToString('yyyy-MM-dd') }
$prevDate  = [datetime]::ParseExact($Date, 'yyyy-MM-dd', [Globalization.CultureInfo]::InvariantCulture).AddDays(-1).ToString('yyyy-MM-dd')
$stamp     = $localNow.ToString('yyyy-MM-dd-HHmmss-fffffff')

if ($OutputRoot -and -not $SkipEmail) { throw '-OutputRoot requires -SkipEmail; rehearsal output must never be emailed accidentally.' }
if ($RefreshReportReferences -and -not $UseExistingReports) { throw '-RefreshReportReferences requires -UseExistingReports.' }
# A rehearsal gets its own reports, logs, artifacts, run folder and a private COPY of the history (copied below), so it
# can neither overwrite a delivered report or receipt nor replay itself into the real history. Configuration, local
# knowledge and reported links are only read.
$realHistoryPath = $paths.HistoryPath
if ($OutputRoot) { $paths = Get-VmrQBRehearsalPaths -Paths $paths -OutputRoot $OutputRoot }
$runRoot     = $paths.StateRoot
$reportDir   = $paths.ReportsDir
$logDir      = $paths.LogsDir
$artifactDir = $paths.ArtifactsDir
$buildHealthPath = Join-Path $reportDir "build-health-$Date.md"
$codeFlowPath    = Join-Path $reportDir "code-flow-$Date.md"
$overallPath     = Join-Path $reportDir "overall-summary-$Date.md"
$digestPath      = Join-Path $reportDir "evidence-digest-$Date.md"
# The only things written before preflight: the audit line and the preflight record both live in the log directory.
[void](New-Item -ItemType Directory -Force -Path $logDir)

# Entry audit: record every invocation (UTC + local + computed date + mode) BEFORE any
# early-skip, so a scheduled run that fires at the wrong local time is diagnosable.
try {
    $auditLine = '{0} | utc={1} | localNow={2} | date={3} | scheduled={4} force={5} skipEmail={6}' -f `
        (Get-Date).ToString('yyyy-MM-dd HH:mm:ss'),
        ([DateTimeOffset]::UtcNow.ToString('o')),
        ($localNow.ToString('o')),
        $Date, [bool]$Scheduled, [bool]$Force, [bool]$SkipEmail
    Add-Content -LiteralPath (Join-Path $logDir 'schedule-audit.log') -Value $auditLine -Encoding utf8
} catch { }

# Everything from here on runs inside one safety net. An unexpected failure is never a silent non-run: it is written to the run
# log and, when email is on, sent as a short notice with whatever reports were already generated attached.
$preflight = $null
try {
    # Scheduled logon catch-up: if today's email already went out, do nothing.
    # (The task fires at 06:00 and again shortly after logon; this keeps the logon run
    #  from regenerating all three reports when the 06:00 run already delivered them.)
    $script:QBStage = 'scheduled catch-up check'
    if ($Scheduled -and -not $Force) {
        $todaysReceipt = Join-Path $logDir "email-$Date.json"
        if (Test-Path -LiteralPath $todaysReceipt) {
            $existing = Invoke-QBOptional -Name "Today's email receipt" -Default $null -Script { Get-Content -LiteralPath $todaysReceipt -Raw | ConvertFrom-Json }
            if ($null -ne $existing -and $existing.PSObject.Properties['status'] -and $existing.status -eq 'sent') {
                Write-Host "Email already sent for $Date; scheduled run has nothing to do." -ForegroundColor DarkYellow
                exit 0
            }
        }
    }

    # One run per state and date. A second run for the same day would write the same reports, context and receipt at the same
    # time (a scheduled run and a manual one can overlap). The claim is released when this process ends, however it ends.
    $script:QBStage = 'daily claim'
    $claimHolder = $null
    $dailyClaim = Enter-QBDailyClaim -LogDirectory $logDir -Date $Date -Holder ([ref]$claimHolder)
    if ($null -eq $dailyClaim) {
        Write-Host "Another VMR QB run for $Date is already in progress ($claimHolder). This run stopped without changing anything: wait for it, or use -Date for another day." -ForegroundColor Yellow
        exit 3
    }

    # Preflight: check every dependency this run needs BEFORE spending an hour on it. Only four failures stop the run (PowerShell,
    # the state folder, the GitHub sign-in, and a config holding your email address that Git tracks or does not ignore). Every other
    # problem is a warning: the run goes on with what it has, and the report
    # opens with a note for each, internal authentication first.
    $script:QBStage = 'preflight'
    $preflightScopes = @()
    if (-not ($UseExistingReports -or $UseCollectedEvidence)) { $preflightScopes += 'Collection' }
    if (-not ($UseExistingReports -or $CollectOnly)) { $preflightScopes += 'Synthesis' }
    if (-not ($SkipEmail -or $CollectOnly)) { $preflightScopes += 'Sending' }
    if ($preflightScopes.Count) {
        # A preflight that itself crashes is a note, not a stop: only what it reports as blocking stops the run.
        $preflight = Invoke-QBOptional -Name 'Preflight' -Default $null -Severity first -Fix 'Run ./daily/Invoke-Preflight.ps1 to see what it could not check.' -Script {
            Invoke-VmrQBPreflight -Scope $preflightScopes
        }
        if ($null -eq $preflight) { $preflight = [pscustomobject]@{ ok = $true; blocking = @(); warnings = @(); checks = @() } }
        try { $preflight | ConvertTo-Json -Depth 6 | Set-Content -LiteralPath (Join-Path $logDir "preflight-$Date-$stamp.json") -Encoding utf8 } catch { }
        Add-QBPreflightNotes -Preflight $preflight

        if (-not $preflight.ok) {
            Write-Host "`nPREFLIGHT FAILED - the run did not start." -ForegroundColor Red
            foreach ($check in @($preflight.blocking)) {
                Write-Host ("  {0}: {1}" -f $check.name, $check.detail) -ForegroundColor Red
                if ($check.remedy) { Write-Host ("      -> {0}" -f $check.remedy) -ForegroundColor Yellow }
            }
            # Tell the QB by email when we can: a silent non-run is how a day gets missed. A separate receipt keeps
            # a later scheduled catch-up run from mistaking this notice for the real report.
            if ('Sending' -in $preflightScopes) {
                try {
                    Send-QBNoticeEmail -Heading "The VMR QB run for $Date did not start" -ReceiptName "email-preflight-$Date.json" `
                        -Intro 'Preflight found a problem that stops the run. No report was generated.' -Items @($preflight.blocking) `
                        -SubjectSuffix ' - NOT RUN (preflight failed)'
                }
                catch { Write-Warning "Could not send the preflight notice: $($_.Exception.Message)" }
            }
            exit 2
        }
    }

    # --------------------------------------------------------------------------- #
    # Prepare: only now, with preflight passed, create state and start spending time. #
    # --------------------------------------------------------------------------- #
    $script:QBStage = 'prepare'
    $runDirectory = Join-Path $artifactDir "$Date\$stamp"
    # What the nested model may read besides its run directory: this run's evidence and the reports. Not the state root,
    # which also holds the identity file, the history and every other day's artifacts (the instructions forbid reading
    # those; this makes it impossible rather than merely discouraged).
    $dataDirectories = @($reportDir, (Join-Path $artifactDir $Date))
    foreach ($directory in @($paths.RunDir, $runDirectory) + $dataDirectories) { [void](New-Item -ItemType Directory -Force -Path $directory) }
    if ($OutputRoot -and (Test-Path -LiteralPath $realHistoryPath -PathType Leaf)) {
        Copy-Item -LiteralPath $realHistoryPath -Destination $paths.HistoryPath -Force
    }

    # The roster names the person to call beside each team. It is built once from the Guide's team table and the GitHub teams,
    # then reused from local/; it is rebuilt only for a team a pulled Guide adds. A failure keeps whatever roster exists and the
    # run carries on. A rehearsal reads the roster but never builds it, because the roster is real, shared local state; an
    # evidence-only run reads it (the issue collector uses it for standing team context) and never reaches out to build it.
    $teamContacts = @{}
    try {
        if (-not $CollectOnly -and -not $OutputRoot -and (Test-VmrQBContactsStale)) {
            try {
                $refreshed = Update-VmrQBContacts
                if ($refreshed.AuthProblem) { Write-Warning "Team roster not refreshed (GitHub sign-in): $($refreshed.AuthProblem)" }
                elseif ($refreshed.Built.Count) { Write-Host "Team roster built for $($refreshed.Built.Count) team(s): $($refreshed.Path)" }
            }
            catch { Write-Warning "Team roster not refreshed: $($_.Exception.Message)" }
        }
        $contactsPath = Get-VmrQBContactsPath
        if (-not $CollectOnly -or (Test-Path -LiteralPath $contactsPath -PathType Leaf)) {
            $contactsFile = Get-Content -LiteralPath $contactsPath -Raw | ConvertFrom-Json
            foreach ($property in $contactsFile.contacts.PSObject.Properties) { $teamContacts[$property.Name] = $property.Value }
        }
    }
    catch {
        Write-Warning "The team roster could not be read; team pings will not name an individual. Run ./common/Invoke-Contacts.ps1. ($($_.Exception.Message))"
        Add-QBRunNote -Source 'roster' -Message "The team roster could not be read, so team pings do not name an individual ($($_.Exception.Message))." -Fix 'Run ./common/Invoke-Contacts.ps1.'
    }

    # One ownership packet (repo owners from the Guide in this checkout, redirects, support destinations, channels) feeds every
    # stage's prompt, and the registry behind it is what the validators check handles against. An owner never stops the run:
    # an entry that does not fit the Guide is matched to the closest team, kept as written or dropped, and listed in the notes.
    $routing = Invoke-QBOptional -Name 'The routing registry' -Default $null -Fix './common/Invoke-Routing.ps1 diff shows how your routing copy differs from the shipped file.' -Script { Get-VmrQBRouting }
    if ($null -ne $routing) { foreach ($warning in @($routing.Warnings)) { Add-QBRunNote -Source 'owners' -Message $warning } }
    $ownershipBlock = [string](Invoke-QBOptional -Name 'The owner table for the prompts' -Fix './common/Invoke-Routing.ps1 diff shows how your routing copy differs from the shipped file.' `
        -Default 'No owner map is available this run: write "No identified owner" for every owner and say what evidence is missing.' -Script { Get-VmrQBOwnershipBlock })
    $script:QBKnownOwnerTeams = @(Invoke-QBOptional -Name 'The list of known teams' -Default @() -Script { Get-VmrQBKnownTeams })

    # The QB's own verified findings (local/knowledge/triage-context.local.json, written by ./history/Invoke-Record.ps1 -AddContext), one file for all readers.
    # A file that does not parse is skipped for this run (and noted); the file itself is left as it is.
    # It is written beside the evidence directory, not in it: the collector claims $runDirectory and refuses one that already has files.
    $triageContextPath = Resolve-QBTriageContext -ArtifactDir $artifactDir -Date $Date -Stamp $stamp `
        -LocalContextPath (Join-Path $paths.LocalKnowledgeDir 'triage-context.local.json')

    # The nested model's own directory: its instructions, the reference documents each stage reads, and its task documents.
    $runWorkspace = if ('Synthesis' -in $preflightScopes) { New-QBRunWorkspace -RunDir $paths.RunDir -Stamp $stamp -ReferencesRoot $paths.ReferencesRoot -TimeZoneId $zone.Id } else { $null }
    function Get-StageReference([string] $Stage) { if ($null -eq $runWorkspace) { return @() } return @(Get-QBStageReferencePath -Workspace $runWorkspace -Stage $Stage) }
    $copilotAvailable = [bool](Get-Command copilot -ErrorAction SilentlyContinue)
    # The code-flow report needs the Maestro tools of its plugin; without them it is skipped (and noted), not attempted.
    $flowSkillMissing = ($null -ne $preflight) -and @($preflight.checks | Where-Object { $_.id -eq 'skill-flow-analysis' -and $_.status -ne 'ok' }).Count -gt 0

    Write-Host "=== VMR QB daily monitoring for $Date ===" -ForegroundColor Yellow
    $results = [Collections.Generic.List[object]]::new()

    $script:QBStage = 'collection'
    $existingReports = if ($UseExistingReports) { @(Get-QBExistingReports -Root $runRoot -Date $Date -TeamContacts $teamContacts -HistoryPath $paths.HistoryPath) } else { @() }
    $contextPath = Join-Path $reportDir "run-context-$Date.json"
    $evidencePath = Join-Path $runDirectory 'sources.json'
    $issueEvidencePath = Join-Path $runDirectory 'issue-evidence.json'
    $savedContext = $null
    $reuseEvidence = $UseExistingReports -or $UseCollectedEvidence -or $SkipBuildHealth -or $SkipCodeFlow
    if ($reuseEvidence) { [void][IO.Directory]::CreateDirectory($runDirectory) }
    if ($reuseEvidence -and (Test-Path -LiteralPath $contextPath)) {
        $savedContext = Get-Content -LiteralPath $contextPath -Raw | ConvertFrom-Json -Depth 100
        if ($savedContext.reportDate -ne $Date) { throw 'Saved run context has the wrong report date.' }
        $evidencePath = $savedContext.evidencePath
        $evidence = Get-Content -LiteralPath $evidencePath -Raw | ConvertFrom-Json -Depth 100
    } elseif ($reuseEvidence) {
        if ($UseCollectedEvidence) { throw '-UseCollectedEvidence requires an existing dated run-context manifest; no collection was started.' }
        $evidence = New-QBUnavailableEvidence -Date $Date -WindowEnd $windowEnd `
            -Message 'Existing reports have no fresh-evidence manifest. Their internal coverage cannot be certified; a full run is required.'
        $evidence | ConvertTo-Json -Depth 100 | Set-Content -LiteralPath $evidencePath -Encoding utf8
    } else {
        try {
            # The collector writes its evidence index into $runDirectory; the old script wrapper only forwarded these arguments.
            Invoke-VmrDailyEvidence -ReportDate $Date -WindowStart $windowEnd.AddHours(-24) -WindowEnd $windowEnd `
                -OutputDirectory $runDirectory -LegacyDirectory (Join-Path $paths.ArtifactsDir $Date) -CoreOnly | Out-Host
            $evidence = Get-Content -LiteralPath $evidencePath -Raw | ConvertFrom-Json -Depth 100
            if ($evidence.schemaVersion -ne 2 -or $evidence.reportDate -ne $Date) {
                throw 'Collector returned an unsupported or incorrectly dated evidence index.'
            }
        }
        catch {
            Write-Warning "Pipeline evidence collection failed: $($_.Exception.Message)"
            [void][IO.Directory]::CreateDirectory($runDirectory)
            $evidence = New-QBUnavailableEvidence -Date $Date -WindowEnd $windowEnd -Message $_.Exception.Message
            $evidence | ConvertTo-Json -Depth 100 | Set-Content -LiteralPath $evidencePath -Encoding utf8
        }
    }

    if ($UseExistingReports -or $UseCollectedEvidence) {
        if ($null -ne $savedContext -and (Test-Path -LiteralPath $savedContext.issueEvidencePath)) {
            $issueEvidencePath = $savedContext.issueEvidencePath
            $issueEvidence = Get-Content -LiteralPath $issueEvidencePath -Raw | ConvertFrom-Json -Depth 100
        } else {
            if ($UseCollectedEvidence) { throw 'The retained issue-evidence index is missing; no collection was started.' }
            $issueEvidence = [pscustomobject]@{
                schemaVersion = 1; status = 'partial'; issues = @(); assigneeCandidates = @()
                errors = @([pscustomobject]@{ resource = 'saved reports'; message = 'Existing reports have no retained issue/owner evidence; no new queries were made during reuse.' })
            }
            $issueEvidence | ConvertTo-Json -Depth 100 | Set-Content -LiteralPath $issueEvidencePath -Encoding utf8
        }
    } else {
        try {
            $problemIds = @($evidence.pipelines | ForEach-Object { @(Get-QBField $_ 'requiredProblemBuildIds' @()) } | Sort-Object -Unique)
            if ($evidence.rootLogsPath -and (Test-Path -LiteralPath $evidence.rootLogsPath)) {
                $logIndex = Get-Content -LiteralPath $evidence.rootLogsPath -Raw | ConvertFrom-Json -Depth 100
                $problemIds += @($logIndex.logs | ForEach-Object { Get-QBField $_ 'promotionBuildId' } | Where-Object { $null -ne $_ })
            }
            [void](New-VmrIssueTriageEvidence -OutputDirectory $runDirectory `
                -TriageContextPath $triageContextPath -BuildIds @($problemIds | Sort-Object -Unique) `
                -ObservationCutoffUtc ([DateTimeOffset]$evidence.observationCutoffUtc) -TeamRoster $teamContacts)
            $issueEvidence = Get-Content -LiteralPath $issueEvidencePath -Raw | ConvertFrom-Json -Depth 100
        }
        catch {
            Write-Warning "Issue/owner evidence collection failed: $($_.Exception.Message)"
            $issueEvidence = [pscustomobject]@{
                schemaVersion = 1; status = 'partial'; observedAtUtc = [DateTimeOffset]::UtcNow.ToString('o')
                issues = @(); assigneeCandidates = @()
                errors = @([pscustomobject]@{ resource = 'GitHub issue/owner evidence'; message = $_.Exception.Message })
            }
            $issueEvidence | ConvertTo-Json -Depth 100 | Set-Content -LiteralPath $issueEvidencePath -Encoding utf8
        }
    }
    $coverageAlerts = @(Get-QBCoverageAlerts -Evidence $evidence -IssueEvidence $issueEvidence)
    foreach ($alert in $coverageAlerts) { Write-Warning $alert }
    Add-QBCoverageNotes -Alerts $coverageAlerts
    # What the model is given. If it cannot be built the model cannot be run, and the reports are skipped with that reason.
    $modelEvidence = Invoke-QBOptional -Name 'The compact evidence for the model' -Default $null -Severity first -Script {
        New-QBModelEvidence -Evidence $evidence -IssueEvidence $issueEvidence -OutputDirectory $runDirectory
    }
    [ordered]@{
        reportDate = $Date; evidencePath = $evidencePath; issueEvidencePath = $issueEvidencePath
        observationCutoffUtc = $evidence.observationCutoffUtc; runDirectory = $runDirectory
    } | ConvertTo-Json -Depth 10 | Set-Content -LiteralPath $contextPath -Encoding utf8
    if ($CollectOnly) {
        Write-Host "Evidence: $evidencePath"
        Write-Host "Issue/owner evidence: $issueEvidencePath"
        if ($coverageAlerts.Count) { exit 1 }
        exit 0
    }

    # Step 1 - Build Health (code flow is started first; the two are independent and overlap)
    $script:QBStage = 'build-health'
    # The reports need the Copilot CLI and the compact evidence. Without either they are skipped with the reason, and the scripts'
    # evidence digest goes out instead.
    $skipReason = if (-not $copilotAvailable) { 'The Copilot CLI is not installed.' } elseif ($null -eq $modelEvidence) { 'The compact evidence for the model could not be built.' } else { '' }
    $canGenerate = -not $skipReason
    $codeFlowHandle = $null
    $generateCodeFlow = -not $UseExistingReports -and -not ($SkipCodeFlow -and (Test-Path $codeFlowPath))
    $generateBuildHealth = -not $UseExistingReports -and -not ($SkipBuildHealth -and (Test-Path $buildHealthPath))
    if ($canGenerate -and -not $flowSkillMissing -and $generateCodeFlow -and $generateBuildHealth) {
        try {
            $codeFlowHandle = Start-CopilotReport -Prompt (Get-CodeFlowPrompt -Date $Date -EvidencePath $modelEvidence.pipelinePath -ReferenceDocument (Get-StageReference 'code-flow')) `
                -TranscriptPath (Join-Path $logDir "copilot-code-flow-$Date-$stamp.jsonl") `
                -ErrorPath (Join-Path $logDir "copilot-code-flow-$Date-$stamp.stderr.log") -DataDirectory $dataDirectories -WorkDirectory $runWorkspace.Path
            Write-Host "[$([DateTime]::Now.ToString('HH:mm:ss'))] Started code flow report concurrently with build health." -ForegroundColor DarkCyan
        }
        catch {
            Write-Warning "Could not start code flow concurrently; it will run after build health: $($_.Exception.Message)"
            $codeFlowHandle = $null
        }
    }
    $bhResult = $null
    if ($UseExistingReports) {
        $bhResult = $existingReports[0]
    } elseif ($SkipBuildHealth -and (Test-Path $buildHealthPath)) {
        Write-Host "Skipping build health generation (using existing file)." -ForegroundColor DarkYellow
        $bhResult = [pscustomobject]@{ name = 'build-health'; status = 'succeeded'; path = $buildHealthPath; error = $null; seconds = 0 }
    } elseif (-not $canGenerate) {
        $bhResult = New-QBSkippedReport -Name 'build-health' -Reason $skipReason
    } else {
        $bhResult = Invoke-QBStage -Name 'build-health' -Script {
            New-Report -Name 'build-health' -Prompt (Get-BuildHealthPrompt -Date $Date -EvidencePath $modelEvidence.pipelinePath `
                -InternalStatus (Get-QBInternalPipeline $evidence).status -OwnershipBlock $ownershipBlock `
                -KnownFixContext ([string](Invoke-QBOptional -Name 'The known-fix context' -Default '' -Script { Get-KnownFixContext -Path $triageContextPath })) -OperatorContext $operatorContext `
                -ReferenceDocument (Get-StageReference 'build-health')) `
                -OutputPath $buildHealthPath -Evidence $evidence -DataDirectory $dataDirectories -ModelEvidencePath $modelEvidence.pipelinePath
        }
    }
    $results.Add($bhResult)
    $internalFindingsPath = "$buildHealthPath.data.json"
    # A payload that cannot be read is a gap in the cross-references, never a stop.
    $internalFindings = if ($bhResult.status -eq 'succeeded' -and (Test-Path -LiteralPath $internalFindingsPath)) {
        Invoke-QBOptional -Name 'The saved build-health findings' -Default $null -Script { Get-Content -LiteralPath $internalFindingsPath -Raw | ConvertFrom-Json -Depth 100 }
    } else { $null }
    if ($null -ne $internalFindings) {
        try {
            Assert-QBInternalFindings -Findings $internalFindings -Evidence $evidence -Date $Date -KnownOwnerTeams $script:QBKnownOwnerTeams
            $internalFindings = Set-VmrQBBuildHealthReferences -Findings $internalFindings -Markdown (Get-Content -LiteralPath $buildHealthPath -Raw)
            $internalFindings | ConvertTo-Json -Depth 100 | Set-Content -LiteralPath $internalFindingsPath -Encoding utf8
        }
        catch {
            $coverageAlerts += "Saved internal findings do not match this evidence snapshot: $($_.Exception.Message)"
            $internalFindings = $null
        }
    }
    if ($null -eq $internalFindings) {
        $coverageAlerts += 'The build-health report has no verified internal findings payload; internal action coverage is incomplete.'
    }

    # Step 2 - Code Flow
    $script:QBStage = 'code-flow'
    $cfResult = $null
    if ($UseExistingReports) {
        $cfResult = $existingReports[1]
    } elseif ($SkipCodeFlow -and (Test-Path $codeFlowPath)) {
        Write-Host "Skipping code flow generation (using existing file)." -ForegroundColor DarkYellow
        $cfResult = [pscustomobject]@{ name = 'code-flow'; status = 'succeeded'; path = $codeFlowPath; error = $null; seconds = 0 }
    } elseif (-not $canGenerate) {
        $cfResult = New-QBSkippedReport -Name 'code-flow' -Reason $skipReason
    } elseif ($flowSkillMissing) {
        $cfResult = New-QBSkippedReport -Name 'code-flow' -Reason 'The flow-analysis skill (the Maestro tools) is not installed.'
    } else {
        $cfResult = Invoke-QBStage -Name 'code-flow' -Script {
            New-Report -Name 'code-flow' -Prompt (Get-CodeFlowPrompt -Date $Date -EvidencePath $modelEvidence.pipelinePath -ReferenceDocument (Get-StageReference 'code-flow')) `
                -OutputPath $codeFlowPath -DataDirectory $dataDirectories -Handle $codeFlowHandle
        }
    }
    $results.Add($cfResult)

    # Steps 3-4 + overall summary
    $script:QBStage = 'overall-summary'
    $prevBH = Join-Path $reportDir "build-health-$prevDate.md"
    $prevCF = Join-Path $reportDir "code-flow-$prevDate.md"
    if ($UseExistingReports) {
        $overallResult = $existingReports[2]
        if ($RefreshReportReferences -and $overallResult.status -eq 'succeeded') {
            # Repairing the references is a courtesy to the saved report: when it cannot be done the report stays as it was, with a note.
            try {
                if ($null -eq $internalFindings) { throw 'Cannot repair references without verified Build Health findings.' }
                $planPath = "$overallPath.data.json"
                $plan = Get-Content -LiteralPath $planPath -Raw | ConvertFrom-Json -Depth 100
                Assert-QBDailyActions -Plan $plan -Findings $internalFindings -IssueEvidence $issueEvidence -Date $Date
                $content = Update-QBReportReferences -Content (Get-Content -LiteralPath $overallPath -Raw) `
                    -Findings $internalFindings -Plan $plan -Evidence $evidence -IssueEvidence $issueEvidence `
                    -OperatorLinksPath $paths.ReportedLinksPath -TeamContacts $teamContacts `
                    -Date $Date -HistoryPath $paths.HistoryPath -BuildHealthPath $buildHealthPath
                Set-Content -LiteralPath $overallPath -Value $content -Encoding utf8
                Write-Host 'BH references repaired from the visible Build Health action list.' -ForegroundColor Green
            }
            catch { Add-QBRunNote -Source 'report-check' -Message "The BH references of the saved overall report were not repaired, so it is unchanged ($($_.Exception.Message))." }
        }
    } elseif (-not $canGenerate) {
        $overallResult = New-QBSkippedReport -Name 'overall-summary' -Reason $skipReason
    } else {
        $overallResult = Invoke-QBStage -Name 'overall-summary' -Script {
            $overallPrompt = Get-OverallPrompt `
                -Date $Date -OwnershipBlock $ownershipBlock -TeamContacts $teamContacts -ReferenceDocument (Get-StageReference 'overall-summary') `
                -BuildHealthPath ($(if ($bhResult.status -eq 'succeeded') { $buildHealthPath } else { $null })) `
                -CodeFlowPath    ($(if ($cfResult.status -eq 'succeeded') { $codeFlowPath } else { $null })) `
                -PrevBuildHealthPath ($(if (Test-Path $prevBH) { $prevBH } else { $null })) `
                -PrevCodeFlowPath    ($(if (Test-Path $prevCF) { $prevCF } else { $null })) `
                -KnownFixContext ([string](Invoke-QBOptional -Name 'The known-fix context' -Default '' -Script { Get-KnownFixContext -Path $triageContextPath })) `
                -PreviousFindingNames ([string](Invoke-QBOptional -Name 'The names of earlier findings' -Default '' -Fix 'The saved reports are read for this; check that local/reports holds readable files.' -Script { Get-PreviousFindingNames -ReportDirectory (Split-Path $overallPath -Parent) -Date $Date })) `
                -IssueHistoryContext (Invoke-QBOptional -Name 'The issue history' -Default $null -Fix 'Fix or move local/issue-history.json (the saved reports can rebuild it); this run compares nothing with earlier days.' -Script { Format-VmrIssueHistoryContext -Path $paths.HistoryPath }) `
                -OperatorContext $operatorContext `
                -EvidencePath $modelEvidence.pipelinePath -IssueEvidencePath $modelEvidence.issuePath `
                -InternalFindingsPath $(if ($null -ne $internalFindings) { $internalFindingsPath } else { 'Unavailable: build-health findings were not verified; return an empty actions array and a coverage gap.' })
            New-Report -Name 'overall-summary' -Prompt $overallPrompt -OutputPath $overallPath -Synthesis `
                -Evidence $evidence -IssueEvidence $issueEvidence -InternalFindings $internalFindings -DataDirectory $dataDirectories `
                -BuildHealthPath ($(if ($bhResult.status -eq 'succeeded') { $buildHealthPath } else { $null }))
        }
    }
    $results.Add($overallResult)

    # --------------------------------------------------------------------------- #
    # Compose run outcome                                                          #
    # --------------------------------------------------------------------------- #
    $script:QBStage = 'run outcome'
    Add-QBCoverageNotes -Alerts $coverageAlerts
    foreach ($failedReport in @($results | Where-Object { $_.status -ne 'succeeded' })) {
        Add-QBRunNote -Source 'report' -Severity first -Message "The $($failedReport.name) report was not generated: $($failedReport.error)"
    }
    $succeeded = @($results | Where-Object status -eq 'succeeded')
    $runStatus = if ($succeeded.Count -eq $results.Count) { 'complete' }
                 elseif ($succeeded.Count -eq 0)          { 'failed' }
                 else                                      { 'partial' }
    # Something the report depends on could not be seen (the internal builds, missing evidence): the run is partial, loudly.
    if ($script:QBCoverageGap -and $runStatus -eq 'complete') { $runStatus = 'partial' }

    # Record today's findings into the persistent issue history, then fold operator-declared aliases
    # so a renamed finding keeps one timeline. This is what lets tomorrow's investigation answer
    # "have we seen this before and what happened last time" without grepping old markdown.
    if ($overallResult.status -eq 'succeeded' -and (Test-Path -LiteralPath $overallPath)) {
        $script:QBStage = 'issue history'
        try {
            # A rehearsal replays its own reports into its own copy of the history ($paths was re-rooted at -OutputRoot).
            $null = Update-VmrIssueHistory -Database $paths.HistoryPath -ReportDirectory $paths.ReportsDir -Date $Date
            $null = Set-VmrIssueAliases -Database $paths.HistoryPath
            Write-Host 'Issue history updated.' -ForegroundColor DarkGray
        }
        catch {
            # History is a reporting aid, never a reason to lose a delivered report or block the email. The saved reports can rebuild it.
            Write-Host "Issue history update failed (non-fatal): $($_.Exception.Message)" -ForegroundColor DarkYellow
            Add-QBRunNote -Source 'history' -Message "Today's findings were not added to the issue history ($($_.Exception.Message))." -Fix 'Fix or move local/issue-history.json; the saved reports can rebuild it.'
        }
    }

    # When the overall report could not be produced, the scripts' own digest of the collected evidence goes out in its place.
    $digestMarkdown = $null
    if ($overallResult.status -ne 'succeeded') {
        $script:QBStage = 'evidence digest'
        $digestMarkdown = Invoke-QBOptional -Name 'The evidence digest' -Default $null -Script {
            New-QBEvidenceDigest -Date $Date -Evidence $evidence -IssueEvidence $issueEvidence -Results $results.ToArray()
        }
        if ($digestMarkdown) { Set-Content -LiteralPath $digestPath -Value (Set-QBRunNotesBlock -Content $digestMarkdown) -Encoding utf8 }
    }

    # The run notes go at the top of the overall summary, under its title: this is where the QB reads that something was stale,
    # missing or failed. They are added after the report's own checks, which never read them.
    $script:QBStage = 'run notes'
    if ($overallResult.status -eq 'succeeded' -and (Test-Path -LiteralPath $overallPath)) {
        [void](Invoke-QBOptional -Name 'The run notes block' -Script {
            Set-Content -LiteralPath $overallPath -Value (Set-QBRunNotesBlock -Content (Get-Content -LiteralPath $overallPath -Raw)) -Encoding utf8
        })
    }
    $attachments = @($succeeded | ForEach-Object { $_.path })
    if ($digestMarkdown) { $attachments += $digestPath }

    # Email body: full overall report rendered as formatted HTML (tables + links),
    # with a per-report status header. Falls back to the digest, then to a status-only body.
    $script:QBStage = 'email'
    $overallMarkdown = if ($overallResult.status -eq 'succeeded' -and (Test-Path -LiteralPath $overallPath)) {
        Invoke-QBOptional -Name 'The overall report for the email body' -Default $null -Script { Get-Content -LiteralPath $overallPath -Raw }
    } else { $null }
    # The email body is a convenience: if it cannot be formatted, a plain one goes out with the reports attached.
    $htmlBody = Invoke-QBOptional -Name 'The email formatting' -Default $null -Fix 'The reports are attached and saved; the email was sent as plain text.' -Script {
        ConvertTo-QBHtmlBody -Date $Date -RunStatus $runStatus -Results $results.ToArray() `
            -Attachments $attachments -OverallMarkdown $overallMarkdown -CoverageAlerts @() -Notes @(Get-QBRunNotes) -DigestMarkdown $digestMarkdown
    }
    if (-not $htmlBody) {
        $htmlBody = New-QBFallbackHtmlBody -Date $Date -RunStatus $runStatus -Results $results.ToArray() -Attachments $attachments -Notes @(Get-QBRunNotes) -Reason 'the formatter failed'
    }
    [void](Invoke-QBOptional -Name 'The saved copy of the email body' -Script { Set-Content -LiteralPath (Join-Path $logDir "email-body-$Date.html") -Value $htmlBody -Encoding utf8 })

    $receiptPath = Join-Path $logDir "email-$Date.json"
    $emailSent = $false
    if ($SkipEmail) {
        Write-Host "Email skipped (-SkipEmail)." -ForegroundColor DarkYellow
    } elseif ((Test-Path $receiptPath) -and -not $Force) {
        $prev = Invoke-QBOptional -Name "Today's email receipt" -Default $null -Script { Get-Content -LiteralPath $receiptPath -Raw | ConvertFrom-Json }
        if ($null -ne $prev -and $prev.PSObject.Properties['status'] -and $prev.status -eq 'sent') {
            Write-Host "Email already sent for $Date (receipt exists). Use -Force to resend." -ForegroundColor DarkYellow
            $emailSent = $true
        }
    }
    if (-not $SkipEmail -and -not $emailSent) {
        try {
            $suffix = if ($runStatus -eq 'failed') { $(if ($digestMarkdown) { ' - evidence digest only (reports not generated)' } else { ' - reports not generated' }) } else { '' }
            $emailSent = Send-QBEmail -Attachments $attachments -HtmlBody $htmlBody -ReceiptPath $receiptPath -RunStatus $runStatus -SubjectSuffix $suffix -Force:$Force
        } catch {
            Write-Host "  -> EMAIL FAILED: $($_.Exception.Message)" -ForegroundColor Red
            Add-QBRunNote -Source 'email' -Severity first -Message "The email was not sent: $($_.Exception.Message) The reports are saved in $reportDir." -Fix 'Start classic Outlook, then run ./daily/Invoke-Daily.ps1 -Force -Date to send it.'
            [ordered]@{ status = 'failed'; error = $_.Exception.Message; attemptedAt = [DateTimeOffset]::Now.ToString('o') } |
                ConvertTo-Json | Set-Content -LiteralPath $receiptPath -Encoding utf8
        }
    }

    # What was delivered is a report (or the digest) that was saved, and, when email is on, mailed.
    $delivered = (($succeeded.Count -gt 0) -or [bool]$digestMarkdown) -and ($SkipEmail -or $emailSent)
    $runLog = Join-Path $logDir "run-$Date-$stamp.json"
    # The log is a record of a run that is already done: failing to write it cannot make a delivered run a failed one.
    try {
        [ordered]@{
            date = $Date; startedAt = $localNow.ToString('o'); runStatus = $runStatus; delivered = $delivered; emailSent = $emailSent
            reusedReports = [bool]$UseExistingReports
            evidencePath = $evidencePath; issueEvidencePath = $issueEvidencePath
            coverageAlerts = $coverageAlerts
            notes = @(Get-QBRunNotes | ForEach-Object { [ordered]@{ severity = $_.severity; source = $_.source; message = $_.message; fix = $_.fix } })
            digest = $(if ($digestMarkdown) { $digestPath } else { $null })
            reports = @($results | ForEach-Object { [ordered]@{ name = $_.name; status = $_.status; path = $_.path; error = $_.error; seconds = $_.seconds } })
        } | ConvertTo-Json -Depth 6 | Set-Content -LiteralPath $runLog -Encoding utf8
    }
    catch { Write-Warning "The run log could not be written ($($_.Exception.Message)); the reports are saved in $reportDir." }

    # --------------------------------------------------------------------------- #
    # Console summary                                                              #
    # --------------------------------------------------------------------------- #
    $notesToShow = @(Get-QBRunNotes)
    if ($notesToShow.Count) {
        Write-Host ''
        Write-Host "Run notes ($($notesToShow.Count)):" -ForegroundColor Yellow
        foreach ($note in $notesToShow) { Write-Host ("  - {0}{1}" -f $note.message, $(if ($note.fix) { " Fix: $($note.fix)" } else { '' })) -ForegroundColor $(if ($note.severity -eq 'first') { 'Red' } else { 'DarkYellow' }) }
    }
    if ($ShowSummary) {
        try {
            Write-Host ''
            Write-Host "=== VMR QB run summary - $Date ===" -ForegroundColor Yellow
            Write-Host "Run status : $runStatus"
            Write-Host "Email      : $(if ($emailSent) { 'sent' } elseif ($SkipEmail) { 'skipped' } else { 'not sent' })"
            Write-Host 'Reports    :'
            foreach ($r in $results) {
                $mark = if ($r.status -eq 'succeeded') { 'OK  ' } else { 'FAIL' }
                Write-Host ("  [{0}] {1,-16} {2}" -f $mark, $r.name, $(if ($r.path) { Split-Path $r.path -Leaf } else { $r.error }))
            }
            if ($overallResult.status -eq 'succeeded') {
                $overallText = Get-Content -LiteralPath $overallPath -Raw
                $m = [regex]::Match($overallText, '(?ms)^##\s*Top actions.*?(?=^##\s|\z)')
                if ($m.Success) {
                    Write-Host ''
                    Write-Host $m.Value.Trim()
                }
            }
        }
        catch { Write-Warning "The summary could not be shown ($($_.Exception.Message)); the reports are saved in $reportDir." }
    }

    # Exit 0 whenever a report was delivered, whatever the notes say; 1 when nothing was (no report, or the email was not sent).
    # 2 is a preflight stop, 3 another run holds this date.
    if (-not $delivered) { exit 1 }
    exit 0
}
catch {
    $failedStage = $script:QBStage
    $failureMessage = $_.Exception.Message
    Write-Host "`nRUN FAILED at ${failedStage}: $failureMessage" -ForegroundColor Red
    Add-QBRunNote -Source 'run' -Severity first -Message "The run stopped unexpectedly at '$failedStage': $failureMessage"
    $generated = @($buildHealthPath, $codeFlowPath, $overallPath, $digestPath | Where-Object {
            $_ -and (Test-Path -LiteralPath $_ -PathType Leaf) -and ((Get-Item -LiteralPath $_).LastWriteTimeUtc -ge $runStartedUtc.AddSeconds(-1))
        })
    try {
        [ordered]@{
            date = $Date; startedAt = $localNow.ToString('o'); runStatus = 'failed'; delivered = $false; stage = $failedStage; error = $failureMessage
            notes = @(Get-QBRunNotes | ForEach-Object { [ordered]@{ severity = $_.severity; source = $_.source; message = $_.message; fix = $_.fix } })
            reports = @($generated)
        } | ConvertTo-Json -Depth 6 | Set-Content -LiteralPath (Join-Path $logDir "run-$Date-$stamp.json") -Encoding utf8
    }
    catch { Write-Warning "Could not write the run log: $($_.Exception.Message)" }
    # A silent non-run is how a day gets missed: say so by email when email is on, with whatever reports were already generated.
    if (-not $SkipEmail -and -not $CollectOnly) {
        try {
            Send-QBNoticeEmail -Heading "The VMR QB run for $Date stopped unexpectedly" -ReceiptName "email-failed-$Date.json" `
                -Intro "The run stopped at '$failedStage'. $(if ($generated.Count) { 'The reports that were already generated are attached.' } else { 'No report was generated.' })" `
                -Items @([pscustomobject]@{ name = $failedStage; detail = $failureMessage; remedy = 'See the run log in the logs folder; run ./daily/Invoke-Preflight.ps1 to re-check.' }) `
                -Attachments $generated -SubjectSuffix ' - RUN FAILED'
        }
        catch { Write-Warning "Could not send the failure notice: $($_.Exception.Message)" }
    }
    exit 1
}
