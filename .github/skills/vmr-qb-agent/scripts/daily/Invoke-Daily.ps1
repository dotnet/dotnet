<#
.SYNOPSIS
    VMR Quarterback (QB) daily round: collect evidence, have the Copilot CLI write three reports, save them locally.

.DESCRIPTION
    Writes build-health-<date>.md, code-flow-<date>.md and overall-summary-<date>.md to local/reports. The rules for the reports
    live in references/ (report-contract.md, runner-instructions.md, triage.md); this script only collects, runs the model, saves
    the reports locally. It is read-only: nothing is merged, approved, closed, commented on or retried.

    Something that could not be read (the internal sign-in, a source) is a run note at the top of the overall report and in the
    run log, and the internal builds are then UNKNOWN, never healthy. A failed run (a model run that fails, a crash) stops the round:
    exit 1 and the console and run log say why. Exit codes: 0 saved, 1 failed, 2 preflight stop.

    -CollectOnly stops after the evidence. Settings are optional: local/qb-config.local.json (model, pluginRoot).
    The Copilot CLI and its plugins are prerequisites (README.md) and are not checked.
#>
[CmdletBinding()]
param(
    [string] $Date,
    [switch] $CollectOnly
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

# Limits for one nested model run.
$TimeoutMinutes = 25
$MaxOutputMB = 250

# Libraries only. They define functions and run nothing, so loading this file has no side effects until the guard below.
. (Join-Path $PSScriptRoot '..\common\core.ps1')
. (Join-Path $PSScriptRoot 'notes.ps1')
. (Join-Path $PSScriptRoot 'evidence.ps1')
. (Join-Path $PSScriptRoot 'report.ps1')
. (Join-Path $PSScriptRoot 'preflight.ps1')
. (Join-Path $PSScriptRoot '..\triage\issue.ps1')

# --------------------------------------------------------------------------- #
# The nested Copilot CLI: arguments, launcher, answer                          #
# --------------------------------------------------------------------------- #
function Get-CopilotArgs {
    param(
        [Parameter(Mandatory)][string] $Prompt,
        [switch] $EvidenceOnly,
        [string[]] $DataDirectory,
        # The model starts in the current evidence directory.
        [Parameter(Mandatory)][string] $WorkDirectory
    )
    $a = [Collections.Generic.List[string]]::new()
    $a.AddRange([string[]]@(
        '-C', $WorkDirectory, '--prompt', $Prompt, '--silent', '--stream', 'off',
        '--output-format', 'json', '--no-color', '--no-ask-user',
        '--sync-session=off', '--no-auto-update', '--deny-tool=write'
    ))
    $model = [string](Get-VmrQBField $config 'model')
    if (-not [string]::IsNullOrWhiteSpace($model)) { $a.Add('--model'); $a.Add($model.Trim()) }
    foreach ($n in @('task', 'task_complete', 'run_factory', 'factories_manage', 'manage_schedule', 'write_agent',
        'session_store_sql', 'sql', 'apply_patch', 'create', 'edit')) {
        $a.Add("--excluded-tools=$n")
    }
    foreach ($directory in @($DataDirectory)) { if ($directory) { $a.Add('--add-dir'); $a.Add($directory) } }

    # Grant only the plugins this round names, each found where it is installed (the configured pluginRoot, then COPILOT_HOME
    # and ~/.copilot, in any marketplace folder). Naming them one by one, rather than a whole marketplace, keeps stale copies
    # of the same skills in other plugins unreadable, so they cannot shadow the versions dotnet/arcade-skills publishes now.
    $pluginRoots = @(Get-VmrQBPluginSearchRoots -ConfiguredRoot (Get-VmrQBField $config 'pluginRoot'))
    $granted = [Collections.Generic.List[string]]::new()
    foreach ($plugin in @(Find-VmrQBPlugin -SearchRoot $pluginRoots)) { if ($plugin.Path -and $plugin.Path -notin $granted) { $granted.Add($plugin.Path) } }
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
    # gh api permissions match the subcommand, not its flags, so allowing it would permit POST with the QB's credentials.
    # The collector owns API access; the model gets only read-only gh subcommands.
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

# Streaming JSONL reader - constant memory, extracts the final assistant answer.
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

# The model that actually answered. Without a pinned model the CLI's default is used and can change between CLI updates,
# so a report's quality can shift with no change here; recording it makes that visible.
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

# Stream to file with a hard timeout, output-size cap and process-tree cleanup.
function Start-CopilotReport {
    param(
        [Parameter(Mandatory)][ValidateSet('build-health', 'code-flow', 'overall-summary')][string] $Stage,
        [Parameter(Mandatory)][string] $EvidencePath,
        [Parameter(Mandatory)][string] $TranscriptPath,
        [Parameter(Mandatory)][string] $ErrorPath,
        [switch] $EvidenceOnly,
        [string[]] $DataDirectory,
        [Parameter(Mandatory)][string] $WorkDirectory
    )
    $copilot = (Get-Command copilot -ErrorAction Stop).Source
    $paths = Get-VmrQBPaths
    $skill = Join-Path $paths.SkillRoot 'SKILL.md'
    $instructions = Join-Path $paths.ReferencesRoot 'runner-instructions.md'
    # The locator leads, so the prompt never starts with the skill's "---" front matter; the skill text follows unchanged.
    $instruction = "Read `"$instructions`". Stage: $Stage. Current evidence: `"$EvidencePath`". The skill instructions follow, unchanged.`n`n" + [IO.File]::ReadAllText($skill)
    $arguments = Get-CopilotArgs -Prompt $instruction -EvidenceOnly:$EvidenceOnly -DataDirectory $DataDirectory -WorkDirectory $WorkDirectory

    $psi = [Diagnostics.ProcessStartInfo]::new()
    if ([IO.Path]::GetExtension($copilot) -ieq '.ps1') {
        # `pwsh -File` reads an argument that starts with a dash and holds a colon (--allow-url=https://..., a prompt) as -Name:value
        # and splits it, so the launcher script gets the arguments splatted from the environment instead.
        $psi.FileName = (Get-Process -Id $PID).Path
        $command = "`$a = ConvertFrom-Json -InputObject `$env:VMR_QB_COPILOT_ARGS -NoEnumerate; `$env:VMR_QB_COPILOT_ARGS = `$null; & '$($copilot.Replace("'", "''"))' @a; exit `$LASTEXITCODE"
        foreach ($a in @('-NoLogo', '-NoProfile', '-NonInteractive', '-EncodedCommand', [Convert]::ToBase64String([Text.Encoding]::Unicode.GetBytes($command)))) { $psi.ArgumentList.Add($a) }
        $psi.Environment['VMR_QB_COPILOT_ARGS'] = ConvertTo-Json -InputObject $arguments -Compress
    }
    else {
        $psi.FileName = $copilot
        foreach ($a in $arguments) { $psi.ArgumentList.Add($a) }
    }
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
    return [pscustomobject]@{
        process = $proc
        outTask = $proc.StandardOutput.BaseStream.CopyToAsync($outFs)
        errTask = $proc.StandardError.BaseStream.CopyToAsync($errFs)
        outStream = $outFs; errStream = $errFs
        transcriptPath = $TranscriptPath; errorPath = $ErrorPath
        startedAt = [DateTime]::UtcNow
        deadline = [DateTime]::UtcNow.AddMinutes($TimeoutMinutes)
        stage = $Stage
    }
}

function Test-CopilotLimits {
    # Why a run is over its time or output limit, or $null while it is within both. A run that is still going is killed. The output
    # cap is also checked after a run has exited: it limits what was written, however quickly.
    param([Parameter(Mandatory)][object] $Handle)
    $process = $Handle.process
    $length = 0L
    try { $length = [IO.FileInfo]::new($Handle.transcriptPath).Length } catch { }
    $reason = if ($length -gt ([long]$MaxOutputMB * 1MB)) { "Copilot ($($Handle.stage)) output exceeded $MaxOutputMB MB (runaway) and the run was stopped. Transcript: $($Handle.transcriptPath)" }
              elseif (-not $process.HasExited -and [DateTime]::UtcNow -gt $Handle.deadline) { "Copilot ($($Handle.stage)) exceeded the $TimeoutMinutes-minute limit and was terminated. Transcript: $($Handle.transcriptPath)" }
    if ($reason -and -not $process.HasExited) { try { $process.Kill($true) } catch { } }
    return $reason
}

function Wait-CopilotReport {
    param(
        [Parameter(Mandatory)][object] $Handle,
        # Runs started earlier and still going (code flow while build health is awaited) stay within their limits too.
        [object[]] $Watch = @()
    )

    $proc = $Handle.process
    $failure = $null
    try {
        while (-not $proc.WaitForExit(2000)) {
            foreach ($run in @(@($Handle) + @($Watch) | Where-Object { $_ })) { $failure = Test-CopilotLimits $run; if ($failure) { break } }
            if ($failure) { break }
        }
        if (-not $failure) { $failure = Test-CopilotLimits $Handle }
        if ($failure -and -not $proc.HasExited) { try { $proc.Kill($true) } catch { } }
        [void]$proc.WaitForExit(30000)
        try { [void]$Handle.outTask.Wait(20000) } catch { }
        try { [void]$Handle.errTask.Wait(20000) } catch { }
        if (-not $failure) { $failure = Test-CopilotLimits $Handle }
        $exit = if ($proc.HasExited) { $proc.ExitCode } else { -1 }
    }
    finally {
        # Whatever ended the wait, no model process is left running behind a failed round.
        if (-not $proc.HasExited) { try { $proc.Kill($true) } catch { } }
        $Handle.outStream.Dispose(); $Handle.errStream.Dispose(); $proc.Dispose()
    }
    if ($failure) { throw $failure }
    if ($exit -ne 0) { throw "Copilot exited with code $exit. Errors: $($Handle.errorPath)" }
    return Get-FinalAnswer -Path $Handle.transcriptPath
}

function Invoke-CopilotReport {
    param(
        [Parameter(Mandatory)][string] $Stage,
        [Parameter(Mandatory)][string] $EvidencePath,
        [Parameter(Mandatory)][string] $TranscriptPath,
        [Parameter(Mandatory)][string] $ErrorPath,
        [switch] $EvidenceOnly,
        [string[]] $DataDirectory,
        [Parameter(Mandatory)][string] $WorkDirectory,
        [object[]] $Watch = @()
    )
    return Wait-CopilotReport -Handle (Start-CopilotReport -Stage $Stage -EvidencePath $EvidencePath -TranscriptPath $TranscriptPath `
        -ErrorPath $ErrorPath -EvidenceOnly:$EvidenceOnly -DataDirectory $DataDirectory -WorkDirectory $WorkDirectory) -Watch $Watch
}

# --------------------------------------------------------------------------- #
# One report: run the model and save what it wrote, as written                  #
# --------------------------------------------------------------------------- #
function New-Report {
    param(
        [Parameter(Mandatory)][string] $Name,
        [string] $EvidencePath,
        [Parameter(Mandatory)][string] $OutputPath,
        [switch] $EvidenceOnly,
        [string[]] $DataDirectory,
        # A run started earlier (code flow starts together with build health).
        [object] $Handle,
        # Runs still going while this one is awaited, held to the same limits.
        [object[]] $Watch = @()
    )
    $transcript = Join-Path $logDir "copilot-$Name-$Date-$stamp.jsonl"
    $errPath    = Join-Path $logDir "copilot-$Name-$Date-$stamp.stderr.log"
    Write-Host "[$([DateTime]::Now.ToString('HH:mm:ss'))] $(if ($null -ne $Handle) { "Awaiting $Name report (started earlier)" } else { "Generating $Name report" })..." -ForegroundColor Cyan
    $startedAt = if ($null -ne $Handle) { $Handle.startedAt } else { [DateTime]::UtcNow }
    $text = if ($null -ne $Handle) { Wait-CopilotReport -Handle $Handle }
            else { Invoke-CopilotReport -Stage $Name -EvidencePath $EvidencePath -TranscriptPath $transcript -ErrorPath $errPath -EvidenceOnly:$EvidenceOnly -DataDirectory $DataDirectory -WorkDirectory $runDirectory -Watch $Watch }
    Set-Content -LiteralPath $OutputPath -Value $text -Encoding utf8
    $seconds = ([DateTime]::UtcNow - $startedAt).TotalSeconds
    $model = Get-QBTranscriptModel -Path $transcript
    Write-Host ("  -> {0} ({1:n0}s, {2:n0} KB{3})" -f (Split-Path $OutputPath -Leaf), $seconds, ((Get-Item -LiteralPath $OutputPath).Length / 1KB), $(if ($model) { ", $model" } else { '' })) -ForegroundColor Green
    return [pscustomobject]@{ name = $Name; status = 'succeeded'; path = $OutputPath; seconds = [int]$seconds; model = $model }
}

# --------------------------------------------------------------------------- #
# Notes                                                                        #
# --------------------------------------------------------------------------- #
function Add-QBCoverageNotes {
    <#
    .SYNOPSIS
    Turns coverage alerts into run notes. Internal authentication comes first, in red, and is recorded once.
    #>
    param([AllowEmptyCollection()][string[]] $Alerts = @())
    foreach ($alert in @($Alerts)) {
        if ([string]::IsNullOrWhiteSpace($alert)) { continue }
        $script:QBCoverageGap = $true
        if ($alert -match '^(?i)(internal|collection):' -and $alert -match '(?i)authenticat|sign[ -]?in|az login|token acquisition|HTTP 40[13]') {
            Add-QBRunNote -Source 'internal-auth' -Key 'internal-auth' -Severity first -Message "The internal builds are UNKNOWN, not healthy: $alert" -Fix 'az login --allow-no-subscriptions'
        }
        elseif ($alert -match '^Issue(/owner)? evidence|^\d+ additional issue') {
            $fix = if ($alert -match 'gh auth login') { 'gh auth login --hostname github.com' } else { '' }
            Add-QBRunNote -Source 'evidence' -Message $alert -Fix $fix
        }
        else {
            Add-QBRunNote -Source 'evidence' -Severity first -Message $alert
        }
    }
}

function Add-QBPreflightNotes {
    <#
    .SYNOPSIS
    Records preflight observations separately from the collector's final coverage decision.
    #>
    param([Parameter(Mandatory)][object] $Preflight)
    foreach ($warning in @($Preflight.warnings)) {
        Write-Warning "$($warning.name): $($warning.detail)"
        $severity = if ([bool]$warning.showFirst) { 'first' } else { 'note' }
        Add-QBRunNote -Source 'preflight' -Severity $severity -Fix $warning.remedy `
            -Message "Preflight warning - $($warning.name): $($warning.detail)"
    }
}

# --------------------------------------------------------------------------- #
# Dot-sourcing this file only defines the functions above.                      #
# --------------------------------------------------------------------------- #
if ($MyInvocation.InvocationName -eq '.') { return }

$runStartedUtc = [DateTime]::UtcNow
$script:QBStage = 'setup'
# True when something the report depends on could not be seen (the internal builds, missing evidence): the run is then partial.
$script:QBCoverageGap = $false
Clear-QBRunNotes
$paths = Get-VmrQBPaths
# Preflight reports config problems; the defaults stand in for an unreadable config.
$config = (Get-VmrQBConfigLenient -Path $paths.ConfigPath).Config
$localNow  = [DateTimeOffset]::Now
$windowEnd = $localNow.ToUniversalTime()
if (-not $Date) { $Date = $localNow.ToString('yyyy-MM-dd') }
$stamp     = $localNow.ToString('yyyy-MM-dd-HHmmss-fffffff')
$reportDir   = $paths.ReportsDir
$logDir      = $paths.LogsDir
$artifactDir = $paths.ArtifactsDir
$dayDirectory = Join-Path $artifactDir $Date
$runDirectory = Join-Path $dayDirectory $stamp
$buildHealthPath = Join-Path $reportDir "build-health-$Date.md"
$codeFlowPath    = Join-Path $reportDir "code-flow-$Date.md"
$overallPath     = Join-Path $reportDir "overall-summary-$Date.md"
$codeFlowHandle = $null
$results = [Collections.Generic.List[object]]::new()

# Everything from here on runs inside one safety net. A failed run is shown in the console and written to the run log.
try {
    # The date names every file of the run, so it is checked first.
    $parsedDate = [datetime]::MinValue
    if (-not [datetime]::TryParseExact($Date, 'yyyy-MM-dd', [Globalization.CultureInfo]::InvariantCulture, [Globalization.DateTimeStyles]::None, [ref]$parsedDate)) {
        throw "-Date must be a calendar date written yyyy-MM-dd, not '$Date'."
    }
    # Preflight: check every dependency this run needs BEFORE spending an hour on it. Only three failures stop the run; every other
    # problem is a warning, and the report opens with a note for each, internal authentication first.
    $script:QBStage = 'preflight'
    $scopes = @('Collection')
    if (-not $CollectOnly) { $scopes += 'Synthesis' }
    $preflight = Invoke-VmrQBPreflight -Scope $scopes
    Add-QBPreflightNotes -Preflight $preflight
    if (-not $preflight.ok) {
        Write-Host "`nPREFLIGHT FAILED - the run did not start." -ForegroundColor Red
        foreach ($check in @($preflight.blocking)) {
            Write-Host ("  {0}: {1}" -f $check.name, $check.detail) -ForegroundColor Red
            if ($check.remedy) { Write-Host ("      -> {0}" -f $check.remedy) -ForegroundColor Yellow }
        }
        exit 2
    }

    foreach ($directory in @($reportDir, $logDir)) { [void](New-Item -ItemType Directory -Force -Path $directory) }
    Write-Host "=== VMR QB daily monitoring for $Date ===" -ForegroundColor Yellow

    [void](New-Item -ItemType Directory -Force -Path $runDirectory)
    [void](Invoke-QBOptional -Name 'Local routing' -Script { Initialize-VmrQBLocalRouting })
    [void](Invoke-QBOptional -Name 'Local ownership' -Script { Initialize-VmrQBGuideOwnership })

    # Evidence for both pipelines. A collection that fails entirely is UNKNOWN evidence, never "healthy".
    $script:QBStage = 'collection'
    $evidencePath = Join-Path $runDirectory 'sources.json'
    $issueEvidencePath = Join-Path $runDirectory 'issue-evidence.json'
    try {
        Invoke-VmrDailyEvidence -ReportDate $Date -WindowStart $windowEnd.AddHours(-24) -WindowEnd $windowEnd -OutputDirectory $runDirectory | Out-Host
        $evidence = Get-Content -LiteralPath $evidencePath -Raw | ConvertFrom-Json -Depth 100
        if ($evidence.schemaVersion -ne 2 -or $evidence.reportDate -ne $Date) { throw 'Collector returned an unsupported or incorrectly dated evidence index.' }
    }
    catch {
        Write-Warning "Pipeline evidence collection failed: $($_.Exception.Message)"
        $evidence = New-QBUnavailableEvidence -Date $Date -WindowEnd $windowEnd -Message $_.Exception.Message
        $evidence | ConvertTo-Json -Depth 100 | Set-Content -LiteralPath $evidencePath -Encoding utf8
    }
    try {
        $problemIds = @($evidence.pipelines | ForEach-Object { @(Get-QBField $_ 'requiredProblemBuildIds' @()) } | Sort-Object -Unique)
        if ($evidence.rootLogsPath -and (Test-Path -LiteralPath $evidence.rootLogsPath)) {
            $logIndex = Get-Content -LiteralPath $evidence.rootLogsPath -Raw | ConvertFrom-Json -Depth 100
            $problemIds += @($logIndex.logs | ForEach-Object { Get-QBField $_ 'promotionBuildId' } | Where-Object { $null -ne $_ })
        }
        [void](New-VmrIssueTriageEvidence -OutputDirectory $runDirectory -BuildIds @($problemIds | Sort-Object -Unique) `
            -ObservationCutoffUtc ([DateTimeOffset]$evidence.observationCutoffUtc))
        $issueEvidence = Get-Content -LiteralPath $issueEvidencePath -Raw | ConvertFrom-Json -Depth 100
    }
    catch {
        Write-Warning "Issue evidence collection failed: $($_.Exception.Message)"
        $issueEvidence = [pscustomobject]@{
            schemaVersion = 1; status = 'partial'; observedAtUtc = [DateTimeOffset]::UtcNow.ToString('o')
            issues = @(); assigneeCandidates = @(); fixPullRequests = @()
            errors = @([pscustomobject]@{ resource = 'GitHub issue evidence'; message = $_.Exception.Message })
        }
        $issueEvidence | ConvertTo-Json -Depth 100 | Set-Content -LiteralPath $issueEvidencePath -Encoding utf8
    }
    $coverageAlerts = @(Get-QBCoverageAlerts -Evidence $evidence -IssueEvidence $issueEvidence)
    foreach ($alert in $coverageAlerts) { Write-Warning $alert }
    Add-QBCoverageNotes -Alerts $coverageAlerts
    $ownerNotes = $null
    if (-not $CollectOnly -and (Test-Path -LiteralPath $paths.OwnerNotesPath -PathType Leaf)) {
        $ownerNotes = Join-Path $dayDirectory 'owner-notes.md'
        Copy-Item -LiteralPath $paths.OwnerNotesPath -Destination $ownerNotes -Force
    }
    $runContext = [ordered]@{
        computerTime = [DateTimeOffset]::Now.ToString('o')
        timeZone = [TimeZoneInfo]::Local.Id
        reportDirectory = $reportDir
        guideOwnershipPath = $paths.GuideOwnershipPath
        routingPath = $paths.LocalRoutingPath
        contactsPath = $paths.ContactsPath
        ownerNotesPath = $ownerNotes
    }
    $modelEvidence = New-QBModelEvidence -Evidence $evidence -IssueEvidence $issueEvidence -OutputDirectory $runDirectory -RunContext $runContext
    if ($CollectOnly) {
        Write-Host "Evidence: $evidencePath"
        Write-Host "Issue evidence: $issueEvidencePath"
        if ($coverageAlerts.Count) { exit 1 }
        exit 0
    }

    $script:QBStage = 'model reports'
    $runnerDoc = Join-Path $paths.ReferencesRoot 'runner-instructions.md'
    $triageDoc = Join-Path $paths.ReferencesRoot 'triage.md'
    $contractDoc = Join-Path $paths.ReferencesRoot 'report-contract.md'
    foreach ($document in @($runnerDoc, $triageDoc, $contractDoc)) { if (-not (Test-Path -LiteralPath $document -PathType Leaf)) { throw "The reference document $document is missing." } }
    # Grant data folders, not the state root holding the local configuration.
    $dataDirectories = @($reportDir, $dayDirectory, $paths.ReferencesRoot, $paths.LocalKnowledgeDir |
        Where-Object { Test-Path -LiteralPath $_ -PathType Container })

    # Code flow starts first and overlaps build health; the two are independent.
    $codeFlowHandle = Start-CopilotReport -Stage 'code-flow' -EvidencePath $modelEvidence.pipelinePath `
        -TranscriptPath (Join-Path $logDir "copilot-code-flow-$Date-$stamp.jsonl") `
        -ErrorPath (Join-Path $logDir "copilot-code-flow-$Date-$stamp.stderr.log") -DataDirectory $dataDirectories -WorkDirectory $runDirectory
    Write-Host "[$([DateTime]::Now.ToString('HH:mm:ss'))] Started code flow report concurrently with build health." -ForegroundColor DarkCyan

    $script:QBStage = 'build-health'
    $results.Add((New-Report -Name 'build-health' -EvidencePath $modelEvidence.pipelinePath -OutputPath $buildHealthPath -EvidenceOnly -DataDirectory $dataDirectories -Watch @($codeFlowHandle)))

    $script:QBStage = 'code-flow'
    $results.Add((New-Report -Name 'code-flow' -OutputPath $codeFlowPath -Handle $codeFlowHandle))

    $script:QBStage = 'overall-summary'
    $results.Add((New-Report -Name 'overall-summary' -EvidencePath $modelEvidence.pipelinePath -OutputPath $overallPath -DataDirectory $dataDirectories))

    # The run notes go at the top of the overall summary, under its title: this is where the QB reads that something was stale,
    # missing or unreadable. The report itself is saved exactly as the model wrote it.
    $script:QBStage = 'finish'
    $runStatus = if ($script:QBCoverageGap) { 'partial' } else { 'complete' }
    [void](Invoke-QBOptional -Name 'The run notes block' -Script {
        Set-Content -LiteralPath $overallPath -Value (Set-QBRunNotesBlock -Content (Get-Content -LiteralPath $overallPath -Raw)) -Encoding utf8
    })

    # The log records a run that is already done: failing to write it cannot make a saved run a failed one.
    try {
        [ordered]@{
            date = $Date; startedAt = $localNow.ToString('o'); runStatus = $runStatus
            notes = @(Get-QBRunNotes | ForEach-Object { [ordered]@{ severity = $_.severity; source = $_.source; message = $_.message; fix = $_.fix } })
            reports = @($results | ForEach-Object { [ordered]@{ name = $_.name; status = $_.status; path = $_.path; seconds = $_.seconds; model = $_.model } })
        } | ConvertTo-Json -Depth 6 | Set-Content -LiteralPath (Join-Path $logDir "run-$Date-$stamp.json") -Encoding utf8
    }
    catch { Write-Warning "The run log could not be written ($($_.Exception.Message)); the reports are saved in $reportDir." }

    $notesToShow = @(Get-QBRunNotes)
    if ($notesToShow.Count) {
        Write-Host ''
        Write-Host "Run notes ($($notesToShow.Count)):" -ForegroundColor Yellow
        foreach ($note in $notesToShow) { Write-Host ("  - {0}{1}" -f $note.message, $(if ($note.fix) { " Fix: $($note.fix)" } else { '' })) -ForegroundColor $(if ($note.severity -eq 'first') { 'Red' } else { 'DarkYellow' }) }
    }
    Write-Host "Reports: $reportDir"
    exit 0
}
catch {
    $failedStage = $script:QBStage
    $failureMessage = $_.Exception.Message
    Write-Host "`nRUN FAILED at ${failedStage}: $failureMessage" -ForegroundColor Red
    Add-QBRunNote -Source 'run' -Severity first -Message "The run stopped at '$failedStage': $failureMessage"
    # A code-flow run that is still going must not outlive the failed round.
    if ($null -ne $codeFlowHandle) { try { $codeFlowHandle.process.Kill($true) } catch { } }
    $generated = @($buildHealthPath, $codeFlowPath, $overallPath | Where-Object {
            $_ -and (Test-Path -LiteralPath $_ -PathType Leaf) -and ((Get-Item -LiteralPath $_).LastWriteTimeUtc -ge $runStartedUtc.AddSeconds(-1))
        })
    try {
        [void](New-Item -ItemType Directory -Force -Path $logDir)
        [ordered]@{
            date = $Date; startedAt = $localNow.ToString('o'); runStatus = 'failed'; stage = $failedStage; error = $failureMessage
            notes = @(Get-QBRunNotes | ForEach-Object { [ordered]@{ severity = $_.severity; source = $_.source; message = $_.message; fix = $_.fix } })
            reports = @($generated)
        } | ConvertTo-Json -Depth 6 | Set-Content -LiteralPath (Join-Path $logDir "run-$Date-$stamp.json") -Encoding utf8
    }
    catch { Write-Warning "Could not write the run log: $($_.Exception.Message)" }
    exit 1
}
