Set-StrictMode -Version Latest

# daily/report.ps1 - The digest the model reads (New-QBModelEvidence), the coverage alerts and the stand-in evidence for a
# collection that failed. Nothing here checks, repairs or renders what the model writes: the model writes the whole report from
# the digest, by the rules in references/. Defines functions only; loading it runs nothing.
. (Join-Path $PSScriptRoot '..\triage\timeout.ps1')
. (Join-Path $PSScriptRoot 'evidence.ps1')

# A wrapper diagnostic (MSB3073 and friends) normally hides the real error and is dropped. When a job retains nothing else -
# common on Windows legs, where repo builds redirect inner output to a log file - dropping it leaves the finding with no
# evidence and invites a vague summary. Rewrite the wrapper into a specific statement naming the repo, exit code and the inner
# log that was not collected.
function ConvertTo-WrapperDiagnostic([string] $Text) {
    if (-not $Text) { return $null }
    $repo = $null
    $match = [regex]::Match($Text, 'src[\\/](?<repo>[A-Za-z0-9._-]+)[\\/](?:eng[\\/]common[\\/])?build\.(?:cmd|sh)')
    if ($match.Success) { $repo = $match.Groups['repo'].Value }
    if (-not $repo) {
        $match = [regex]::Match($Text, 'src[\\/](?<repo>[A-Za-z0-9._-]+)[\\/]')
        if ($match.Success) { $repo = $match.Groups['repo'].Value }
    }
    if (-not $repo) { return $null }

    $code = $null
    $match = [regex]::Match($Text, 'exited with code (?<code>-?\d+)')
    if ($match.Success) { $code = $match.Groups['code'].Value }

    $log = $null
    $match = [regex]::Match($Text, '>\s*(?<log>\S+\.log)\b')
    if ($match.Success) {
        $log = $match.Groups['log'].Value
        $trim = [regex]::Match($log, '(?<tail>artifacts[\\/].*)$')
        if ($trim.Success) { $log = $trim.Groups['tail'].Value }
    }

    $summary = "'$repo' repo build exited"
    if ($code) { $summary += " with code $code" }
    $summary += '; the timeline retained only this wrapper and no inner compiler or restore diagnostic'
    if ($log) { $summary += " (inner log not collected: $log)" }
    return $summary
}

function Get-QBField {
    param([AllowNull()][object] $Object, [string] $Name, [AllowNull()][object] $Default = $null)
    if ($null -eq $Object) { return $Default }
    if ($Object -is [System.Collections.IDictionary]) {
        if ($Object.Contains($Name)) { return $Object[$Name] }
    } else {
        $property = $Object.PSObject.Properties[$Name]
        if ($null -ne $property) { return $property.Value }
    }
    return $Default
}

function Get-QBInternalPipeline {
    param([object] $Evidence)
    $pipelines = @(Get-QBField $Evidence 'pipelines' @())
    $internal = @($pipelines | Where-Object { (Get-QBField $_ 'name') -eq 'internal' })
    if ($internal.Count -ne 1) { throw 'Fresh run evidence must contain exactly one internal pipeline record.' }
    return $internal[0]
}

function Get-QBInventoryBuilds {
    param([string] $Path)
    if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) { throw "Missing scoped inventory: $Path" }
    $value = Get-Content -LiteralPath $Path -Raw | ConvertFrom-Json -Depth 100
    if ($null -eq $value) { return @() }
    # An empty list is a real answer (no builds), not a reason to treat the wrapper object as a build.
    foreach ($name in 'value', 'builds') {
        $property = $value.PSObject.Properties[$name]
        if ($null -ne $property) { return @($property.Value | Where-Object { $null -ne $_ }) }
    }
    return @($value)
}

function Get-QBCoverageAlerts {
    param([object] $Evidence, [object] $IssueEvidence)
    $alerts = [Collections.Generic.List[string]]::new()
    $errors = @(Get-QBField $Evidence 'requiredErrors' @())
    $orderedErrors = @($errors | Sort-Object { -not [bool](Get-QBField $_ 'authenticationRequired' $false) })
    foreach ($requiredError in @($orderedErrors | Select-Object -First 8)) {
        $scope = [string](Get-QBField $requiredError 'scope')
        $message = [string](Get-QBField $requiredError 'message')
        if ($message.Length -gt 1200) { $message = $message.Substring(0, 1200) + ' (full details retained in sources.json)' }
        if ($message) { $alerts.Add("${scope}: $message") }
        $signIn = [string](Get-QBField $requiredError 'signInCommand')
        if ($signIn) { $alerts.Add("${scope}: Authentication required. Run: $signIn") }
    }
    if ($errors.Count -gt 8) { $alerts.Add("$($errors.Count - 8) additional pipeline evidence errors are retained in the run's sources.json.") }
    $internal = Get-QBInternalPipeline $Evidence
    if ((Get-QBField $internal 'status') -ne 'available' -and $alerts.Count -eq 0) {
        $alerts.Add("internal: Pipeline coverage is $(Get-QBField $internal 'status'), not verified complete.")
        $command = [string](Get-QBField $internal 'loginCommand')
        if ($command) { $alerts.Add("internal: Authentication required. Run: $command") }
    }
    if ((Get-QBField $Evidence 'status') -ne 'complete' -and $alerts.Count -eq 0) {
        $alerts.Add('Pipeline evidence is incomplete; do not treat missing internal results as healthy.')
    }
    if ((Get-QBField $IssueEvidence 'status') -ne 'complete') {
        $alerts.Add('Issue/owner evidence is incomplete; new-issue recommendations are limited.')
        $issueErrors = @(Get-QBField $IssueEvidence 'errors' @())
        foreach ($issueError in @($issueErrors | Select-Object -First 5)) {
            $message = [string](Get-QBField $issueError 'message')
            if ($message) { $alerts.Add("Issue evidence: $message") }
        }
        if ($issueErrors.Count -gt 5) { $alerts.Add("$($issueErrors.Count - 5) additional issue/owner errors are retained in issue-evidence.json.") }
    }
    return $alerts.ToArray()
}

function New-QBUnavailableEvidence {
    param([string] $Date, [DateTimeOffset] $WindowEnd, [string] $Message)
    $authRequired = $Message -match 'AUTH_ERROR|AUTH_REQUIRED|token acquisition failed'
    $command = if ($authRequired) { 'az login --allow-no-subscriptions' } else { $null }
    [pscustomobject]@{
        schemaVersion = 2; reportDate = $Date; status = 'partial'
        windowStartUtc = $WindowEnd.AddHours(-24).ToUniversalTime().ToString('o')
        observationCutoffUtc = $WindowEnd.ToUniversalTime().ToString('o')
        collectedAtUtc = [DateTimeOffset]::UtcNow.ToString('o')
        sources = @(); failureTasksPath = $null; rootLogsPath = $null
        pipelines = @(
            [pscustomobject]@{
                name = 'public'; status = 'unavailable'; inventoryPath = $null
                requiredProblemBuildIds = @(); counts = $null; error = $Message; loginCommand = $null
            },
            [pscustomobject]@{
                name = 'internal'; status = if ($authRequired) { 'authentication_required' } else { 'unavailable' }
                inventoryPath = $null; requiredProblemBuildIds = @(); counts = $null
                error = $Message; loginCommand = $command
            }
        )
        requiredErrors = @([pscustomobject]@{
            scope = 'collection'; stage = 'pipeline-evidence'; message = $Message
            authenticationRequired = $authRequired; signInCommand = $command
        })
    }
}

function Get-QBRepoBuildPacket {
    <#
    .SYNOPSIS
    The repo-build attribution for one downloaded log, or $null when it is not a repo-build log.
    .DESCRIPTION
    Derived mechanically from the log's own start/done markers and elapsed time, so the model is handed
    the answer rather than reading it off the log. Failures here must never break packet generation.
    #>
    param(
        [string] $Path,
        # The job's own time limit when the timeline stated it ("... maximum time of N minutes"). Without it the 150-minute VMR
        # default is assumed, and the attribution says so instead of calling a short log a failure or cancellation.
        [AllowNull()][Nullable[int]] $TimeLimitMinutes
    )
    if (-not $Path -or -not (Get-Command Get-VmrQBRepoBuildState -ErrorAction SilentlyContinue)) { return $null }
    try {
        if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) { return $null }
        if (-not (Select-String -LiteralPath $Path -Pattern '\] Building [A-Za-z0-9._-]+' -Quiet)) { return $null }
        $state = Get-VmrQBRepoBuildState -Path $Path
        if (-not (Get-QBField $state 'hasMarkers' $false)) { return $null }
        $attribution = if ($null -ne $TimeLimitMinutes -and $TimeLimitMinutes -ge 1) { Get-VmrQBTimeoutAttribution -State $state -TimeLimitMinutes ([int]$TimeLimitMinutes) }
                       else { Get-VmrQBTimeoutAttribution -State $state -LimitIsAssumed }
        return [ordered]@{
            reliable = Get-QBField $state 'reliable'; elapsedMinutes = Get-QBField $state 'elapsedMinutes'
            timeLimitMinutes = $attribution.timeLimitMinutes
            limitSource = $(if ($attribution.limitAssumed) { 'assumed 150-minute VMR default; the job''s own limit was not in the evidence' } else { 'the job''s own limit from the timeline' })
            timedOut = $attribution.timedOut; uncertain = $attribution.uncertain
            repoAtTimeout = @($attribution.repoAtTimeout); bystanders = @($attribution.bystanders)
            errorOnlyRepos = @($attribution.errorOnlyRepos); statement = $attribution.statement
        }
    }
    catch { return $null }
}

function Get-QBJobTimeLimitMinutes {
    <#
    .SYNOPSIS
    The time limit the pipeline itself reported for a job ("... ran longer than the maximum time of N minutes"), or $null.
    .DESCRIPTION
    Looked for in the job's retained timeline messages and then in the log's own diagnostic excerpts, by job record id, so one
    leg's limit is never applied to another leg that shares its display name. $null means the limit is unknown, not 150.
    #>
    param([string] $JobId, [AllowNull()][object[]] $Task = @(), [AllowNull()][object] $Log)
    $pattern = 'ran longer than the maximum time of (?<minutes>\d+) minutes'
    $texts = [Collections.Generic.List[string]]::new()
    if ($JobId) {
        foreach ($record in @($Task)) {
            if ([string](Get-QBField $record 'jobId') -ne $JobId) { continue }
            foreach ($message in @(Get-QBField $record 'issues' @())) { $texts.Add([string]$message) }
        }
    }
    if ($null -ne $Log) {
        $texts.Add([string](Get-QBField $Log 'diagnosticExcerpt'))
        foreach ($excerpt in @(Get-QBField $Log 'diagnosticExcerpts' @())) { $texts.Add([string](Get-QBField $excerpt 'text')) }
    }
    foreach ($text in $texts) {
        $match = [regex]::Match($text, $pattern)
        if ($match.Success) { return [int]$match.Groups['minutes'].Value }
    }
    return $null
}

function New-QBModelEvidence {
    <#
    .SYNOPSIS
    Shrinks the collector's evidence into what the model reads: a digest of the pipelines with a detail file per problem build, and
    the compact list of issues. Returns the paths of the two digest files.
    #>
    param([object] $Evidence, [object] $IssueEvidence, [string] $OutputDirectory, [Parameter(Mandatory)][object] $RunContext)
    [void][IO.Directory]::CreateDirectory($OutputDirectory)
    function Save-ModelJson([string] $Name, [object] $Value) {
        $path = Join-Path $OutputDirectory $Name
        ConvertTo-Json -InputObject $Value -Depth 40 -Compress:($Name -eq 'model-pipeline-evidence.json') |
            Set-Content -LiteralPath $path -Encoding utf8
        return $path
    }
    function Get-DiagnosticSeverity([string] $Text, [string] $TaskResult, [string] $JobResult) {
        # Record-based first: an issue is fatal only if its task failed AND its parent Job failed. 1ES/Guardian tasks emit
        # ##[error] and still let the job pass (continueOnError), so log text alone lies. Only a record that could not be tied to a
        # Job (no result for it) falls back to the message text.
        if ($TaskResult -and $JobResult) {
            if ($TaskResult -eq 'failed' -and $JobResult -eq 'failed') { return 'terminating' }
            return 'nonTerminating'
        }
        # Terminating: the pipeline or a build tool actually failed the job here.
        if ($Text -match '(?i)##\[error\]|(?:^|\s|\():\s*error\s|\berror (?:MSB|CS|NU|NETSDK|BA|AD)\d+\b|\berror\s*:\s|\bexited with code [1-9]|\bTask failed\b') {
            return 'terminating'
        }
        # Non-terminating: text that merely contains "error"/"could not load". Roslyn logs
        # analyzer load failures this way and still completes the compilation.
        return 'nonTerminating'
    }
    function Short-Text([object] $Text, [int] $Limit = 800) {
        $value = Protect-VmrEvidenceDiagnostic -Text ([string]$Text)
        if ($value.Length -le $Limit) { return $value }
        return $value.Substring(0, $Limit)
    }
    function Get-DiagnosticLines([string] $Path, [object[]] $Fallback) {
        if (-not $Path -or -not (Test-Path -LiteralPath $Path -PathType Leaf)) { return $Fallback }
        $selected = [Collections.Generic.List[object]]::new()
        $seen = [Collections.Generic.HashSet[string]]::new([StringComparer]::OrdinalIgnoreCase)
        $lineNumber = 0
        foreach ($line in [IO.File]::ReadLines($Path)) {
            $lineNumber++
            if ($line -notmatch '(?i)\bexception\b|\bfailed\b|\berror\b|timed out|unauthorized|\bcannot |could not |unable to |channel with ID|Invalid argument:') { continue }
            $message = [regex]::Replace($line, '^\d{4}-\d{2}-\d{2}T\S+Z\s*', '').Trim()
            if ($message -match '(?i)\b(?:0 errors?|failed:\s*0)\b') { continue }
            $rank = if ($message -match '(?i)Unhandled exception|MSBuild server failed|401 \(Unauthorized|Channel with ID|Unable to finish invocation|error NU\d+|Text file busy|stopped hearing from agent|task has timed out|Invalid argument:') { 0 }
                elseif ($message.Length -gt 1600 -or $message -match '(?i)MSB3073|Bash exited|Script failed|##\[section\]') { 2 }
                else { 1 }
            $text = Short-Text $message 900
            if (-not $seen.Add($text)) { continue }
            $selected.Add([pscustomobject]@{ line = $lineNumber; text = $text; rank = $rank })
            if ($selected.Count -gt 64) {
                $keep = @($selected | Sort-Object rank, line | Select-Object -First 32)
                $selected.Clear(); $selected.AddRange([object[]]$keep)
            }
        }
        if ($selected.Count -eq 0) { return $Fallback }
        return @($selected | Sort-Object rank, line | Select-Object -First 8 line, text)
    }
    function Get-DiagnosticSummary([object[]] $Tasks, [object[]] $Logs) {
        # Each record and log entry carries its parent Job's result (jobResult, set by the collector from the whole timeline,
        # including a Job that succeeded and was not itself kept), so a task failure is tested against its own job and no timeline
        # has to be loaded here.
        $messages = @(
            foreach ($task in $Tasks) {
                foreach ($text in @($task.issues)) {
                    @{ text = [string]$text; task = $task.task; job = $task.job; path = $null; line = $null
                        taskResult = [string](Get-QBField $task 'result'); jobResult = [string](Get-QBField $task 'jobResult') }
                }
            }
            foreach ($log in $Logs) {
                foreach ($excerpt in @(Get-QBField $log 'excerpts' @())) {
                    @{ text = [string]$excerpt.text; task = $log.task; job = $log.job; path = $log.path; line = $excerpt.line
                        taskResult = [string](Get-QBField $log 'result'); jobResult = [string](Get-QBField $log 'jobResult') }
                }
            }
        )
        # A wrapper diagnostic hides the real error, so it is dropped - unless its job retains nothing else, in which case it is
        # kept with a separate explanation of the missing inner diagnostic.
        $wrapperPattern = '(?i)MSB3073|Bash exited|Script failed|The command |0 Error|Build failed with exit'
        $jobsWithRealDiagnostic = [Collections.Generic.HashSet[string]]::new()
        foreach ($message in $messages) {
            if ($message.text -notmatch $wrapperPattern) { [void]$jobsWithRealDiagnostic.Add([string]$message.job) }
        }

        $groups = [ordered]@{}
        foreach ($message in $messages) {
            $rank = 1
            $explanation = $null
            if ($message.text -match $wrapperPattern) {
                if ($jobsWithRealDiagnostic.Contains([string]$message.job)) { continue }
                $explanation = ConvertTo-WrapperDiagnostic $message.text
                if (-not $explanation) { continue }
                $quote = $message.text
                $rank = 2
            }
            else {
                $quote = [regex]::Replace($message.text, '^\d{4}-\d{2}-\d{2}T\S+Z\s*', '').Trim()
                $errorText = [regex]::Match($quote, '(?i)(?:error [A-Z]+\d+:\s*(?:\([^)]*\)\s*)?|error\s*:\s*)(?<message>.+)')
                if ($errorText.Success) { $quote = $errorText.Groups['message'].Value }
                if ($quote -match '(?i)^(?:at |--- End|Stack trace:|Warning:|\+ echo|\* \[new branch\]|If there were failures)') { continue }
            }
            if ($quote.Length -lt 12) { continue }
            $quote = Short-Text $quote 900
            $groupText = if ($explanation) { $explanation } else { $quote }
            $key = [regex]::Replace($groupText.ToLowerInvariant(), '[0-9a-f]{8}(?:-[0-9a-f]{4}){3}-[0-9a-f]{12}', '<id>')
            $key = [regex]::Replace($key, '\s*\[[^\]]*\]\s*$', '')
            $key = [regex]::Replace($key, 'https?://[^\s''"]+', '<url>')
            $key = [regex]::Replace($key, '[a-z]:[\\/][^''"\r\n]+|/(?:__w|mnt|users|home)/[^\s''"]+', '<path>')
            $key = [regex]::Replace($key, "failed to retrieve information about '[^']+'", "failed to retrieve information about '<package>'")
            $key = [regex]::Replace($key, '\(in [\d.]+ \w+\)', '(duration)')
            $key = [regex]::Replace($key, 'global\\msbuild-server-running-[a-z0-9_-]+', 'global\msbuild-server-running-<id>')
            # Include each occurrence's severity in the group key so fatal and non-fatal copies remain separate.
            $severity = Get-DiagnosticSeverity $message.text ([string]$message.taskResult) ([string]$message.jobResult)
            $key = "$severity|$key"
            if (-not $groups.Contains($key)) {
                $groups[$key] = @{ quote = $quote; explanation = $explanation; rank = $rank; tasks = [Collections.Generic.HashSet[string]]::new()
                    locations = [Collections.Generic.List[object]]::new(); occurrences = 0; severity = $severity }
            }
            $group = $groups[$key]
            [void]$group.tasks.Add("$($message.job) / $($message.task)")
            $group.occurrences++
            if ($message.path -and $group.locations.Count -lt 1) {
                $group.locations.Add(@{ path = $message.path; line = $message.line })
            }
        }
        # Terminating diagnostics first: a root cause must outrank non-fatal noise that merely contains the word "error" (for
        # example an analyzer that failed to load while the compilation still succeeded).
        $ordered = @($groups.Values | Sort-Object @{ e = { if ($_.severity -eq 'terminating') { 0 } else { 1 } } },
            rank, @{ e = { -$_.occurrences } })
        [pscustomobject]@{
            diagnostics = @($ordered | Select-Object -First 32 | ForEach-Object {
                @{ quote = $_.quote; explanation = $_.explanation; tasks = @($_.tasks | Select-Object -First 3); affectedTaskCount = $_.tasks.Count
                    locations = $_.locations.ToArray(); occurrences = $_.occurrences; severity = $_.severity }
            })
            terminatingCount = @($ordered | Where-Object { $_.severity -eq 'terminating' }).Count
            additionalDistinctDiagnostics = [Math]::Max(0, $ordered.Count - 32)
        }
    }

    $failuresPath = [string](Get-QBField $Evidence 'failureTasksPath')
    $failureIndex = if ($failuresPath -and (Test-Path -LiteralPath $failuresPath)) {
        Get-Content -LiteralPath $failuresPath -Raw | ConvertFrom-Json -Depth 100
    } else { @{ builds = @() } }
    $logsPath = [string](Get-QBField $Evidence 'rootLogsPath')
    $logIndex = if ($logsPath -and (Test-Path -LiteralPath $logsPath)) {
        Get-Content -LiteralPath $logsPath -Raw | ConvertFrom-Json -Depth 100
    } else { @{ logs = @() } }
    $pipelines = @(
        foreach ($pipeline in @(Get-QBField $Evidence 'pipelines' @())) {
            $organization = if ((Get-QBField $pipeline 'name') -eq 'internal') { 'dnceng' } else { 'dnceng-public' }
            # A partial inventory still lists the problem builds that were read, so their evidence is kept; its counts stay unknown.
            $inventoryState = if (Get-QBField $pipeline 'inventoryPath') { 'complete' } elseif (Get-QBField $pipeline 'partialInventoryPath') { 'partial' } else { 'none' }
            $inventoryPath = [string](Get-QBField $pipeline 'inventoryPath')
            if (-not $inventoryPath) { $inventoryPath = [string](Get-QBField $pipeline 'partialInventoryPath') }
            $builds = if ($inventoryPath) { @(Get-QBInventoryBuilds $inventoryPath) } else { @() }
            $problems = @(
                foreach ($build in @($builds | Where-Object id -in @(Get-QBField $pipeline 'requiredProblemBuildIds' @()))) {
                    $record = @($failureIndex.builds | Where-Object {
                        $_.buildId -eq $build.id -and
                        (Get-QBField $_ 'organization') -eq $organization
                    } | Select-Object -First 1)
                    $tasks = @(
                        foreach ($task in @($record | ForEach-Object { @(Get-QBField $_ 'tasks' @()) })) {
                            if ((Get-QBField $task 'result') -eq 'succeeded') { continue }
                            $messages = @(@(Get-QBField $task 'issues' @()) | ForEach-Object {
                                Short-Text (Get-QBField $_ 'message') 900
                            } | Select-Object -Unique -First 3)
                            if ((Get-QBField $task 'recordType') -ne 'Task' -and $messages.Count -eq 0) { continue }
                            [ordered]@{ task = Get-QBField $task 'task'; job = Get-QBField $task 'job'; jobId = Get-QBField $task 'jobId'
                                jobResult = Get-QBField $task 'jobResult'
                                recordType = Get-QBField $task 'recordType'; result = Get-QBField $task 'result'; issues = $messages }
                        }
                    )
                    $buildLogs = @($logIndex.logs | Where-Object { $_.buildId -eq $build.id -and (Get-QBField $_ 'organization') -eq $organization })
                    $logs = @(
                        foreach ($log in $buildLogs) {
                            if ((Get-QBField $log 'evidenceKind') -in @('warningSample', 'warning')) { continue }
                            if ((Get-QBField $log 'recordType') -eq 'Job' -and
                                @($buildLogs | Where-Object {
                                    (Get-QBField $_ 'recordType') -eq 'Task' -and
                                    (Get-QBField $_ 'jobId') -eq (Get-QBField $log 'jobId') -and
                                    (Get-QBField $_ 'actualBuildId') -eq (Get-QBField $log 'actualBuildId')
                                }).Count) {
                                continue
                            }
                            $excerpts = @(
                                foreach ($excerpt in @(Get-DiagnosticLines -Path (Get-QBField $log 'path') -Fallback @(Get-QBField $log 'diagnosticExcerpts' @()))) {
                                    [ordered]@{ line = $excerpt.line; text = $excerpt.text }
                                }
                            )
                            $entry = [ordered]@{
                                task = Get-QBField $log 'task'; job = Get-QBField $log 'job'; jobId = Get-QBField $log 'jobId'; result = Get-QBField $log 'result'
                                jobResult = Get-QBField $log 'jobResult'
                                available = Get-QBField $log 'available'; logId = Get-QBField $log 'logId'
                                promotionBuildId = Get-QBField $log 'promotionBuildId'
                                path = Get-QBField $log 'path'; excerpts = $excerpts
                                diagnostic = if ($excerpts.Count) { $null } else { Short-Text (Get-QBField $log 'diagnosticExcerpt') 1200 }
                            }
                            $sameFailure = Get-QBField $log 'sameFailureAs'
                            if ($null -ne $sameFailure) { $entry['sameFailureAs'] = $sameFailure }
                            $jobLimit = Get-QBJobTimeLimitMinutes -JobId ([string](Get-QBField $log 'jobId')) -Task $tasks -Log $log
                            $repoBuild = Get-QBRepoBuildPacket -Path ([string](Get-QBField $log 'path')) -TimeLimitMinutes $jobLimit
                            if ($null -ne $repoBuild) { $entry['repoBuild'] = $repoBuild }
                            $entry
                        }
                    )
                    $diagnostics = Get-DiagnosticSummary -Tasks $tasks -Logs $logs
                    $detailPath = Save-ModelJson "model-$(Get-QBField $pipeline 'name')-$($build.id).json" ([ordered]@{
                        buildId = $build.id; sourceBranch = $build.sourceBranch; result = $build.result
                        diagnostics = $diagnostics.diagnostics
                        additionalDistinctDiagnostics = $diagnostics.additionalDistinctDiagnostics
                        logs = $logs; tasks = $tasks
                    })
                    [ordered]@{
                        id = $build.id; branch = $build.sourceBranch; result = $build.result
                        classification = Get-QBField $build 'vmrClassification'
                        diagnostics = @($diagnostics.diagnostics | Select-Object -First 8 | ForEach-Object {
                            [ordered]@{
                                quote = Short-Text $_.quote 350; explanation = $_.explanation; severity = $_.severity
                                tasks = @($_.tasks | Select-Object -First 1)
                                affectedTaskCount = $_.affectedTaskCount; occurrences = $_.occurrences
                            }
                        })
                        terminatingDiagnosticCount = $diagnostics.terminatingCount
                        additionalDistinctDiagnostics = $diagnostics.additionalDistinctDiagnostics + [Math]::Max(0, $diagnostics.diagnostics.Count - 8)
                        sameFailureAs = @($logs | ForEach-Object { Get-QBField $_ 'sameFailureAs' } |
                            Where-Object { $null -ne $_ } | Sort-Object buildId, logId -Unique | Select-Object buildId, logId)
                        detailPath = $detailPath
                    }
                }
            )
            [ordered]@{
                name = Get-QBField $pipeline 'name'; status = Get-QBField $pipeline 'status'; counts = Get-QBField $pipeline 'counts'
                inventory = $inventoryState
                error = Short-Text (Get-QBField $pipeline 'error') 1600
                loginCommand = Get-QBField $pipeline 'loginCommand'; requiredProblemBuildIds = @(Get-QBField $pipeline 'requiredProblemBuildIds' @())
                completed = @($builds | Select-Object id, sourceBranch, result,
                    @{n='classification';e={
                        $classification = Get-QBField $_ 'vmrClassification'
                        @{
                            kind = Get-QBField $classification 'kind'; complete = Get-QBField $classification 'complete'
                            pullRequest = (Get-QBField $classification 'pullRequest') |
                                Select-Object number, title, author, baseRefName, url
                        }
                    }})
                problemBuilds = $problems
                active = @(
                    foreach ($path in @(Get-QBField $pipeline 'activeInventoryPaths' @())) {
                        Get-QBInventoryBuilds $path | Select-Object id, sourceBranch, status, queueTime
                    }
                )
            }
        }
    )
    $pipelinePath = Save-ModelJson 'model-pipeline-evidence.json' @{
        schemaVersion = 1; status = Get-QBField $Evidence 'status'; reportDate = Get-QBField $Evidence 'reportDate'
        windowStartUtc = Get-QBField $Evidence 'windowStartUtc'; observationCutoffUtc = Get-QBField $Evidence 'observationCutoffUtc'
        collectedAtUtc = Get-QBField $Evidence 'collectedAtUtc'; pipelines = $pipelines
        runContext = $RunContext
        requiredErrors = @(@(Get-QBField $Evidence 'requiredErrors' @()) | Select-Object -First 8)
    }
    $issueRows = @(
        foreach ($issue in @(Get-QBField $IssueEvidence 'issues' @())) {
            [ordered]@{
                repository = Get-QBField $issue 'repository'; number = Get-QBField $issue 'number'; title = Get-QBField $issue 'title'
                state = Get-QBField $issue 'state'; url = Get-QBField $issue 'url'; kind = Get-QBField $issue 'kind'
                matchedBuildIds = @(Get-QBField $issue 'matchedBuildIds' @())
            }
        }
    )
    $issuePath = Save-ModelJson 'model-issue-evidence.json' @{
        schemaVersion = 1; status = Get-QBField $IssueEvidence 'status'; observedAtUtc = Get-QBField $IssueEvidence 'observedAtUtc'
        issues = $issueRows; errors = @(@(Get-QBField $IssueEvidence 'errors' @()) | Select-Object -First 5)
    }
    return [pscustomobject]@{ pipelinePath = $pipelinePath; issuePath = $issuePath }
}
