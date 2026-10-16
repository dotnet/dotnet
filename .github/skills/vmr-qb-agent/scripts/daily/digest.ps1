Set-StrictMode -Version Latest

# daily/digest.ps1 - The evidence digest: what the scripts collected, as a plain list, built without the model.
# It is what goes out in place of the overall summary when that report could not be produced (the Copilot CLI is missing, a run
# timed out, the evidence for the model could not be built). It has no narrative and no owner routing: problem builds with links,
# the failed jobs and tasks, the first error line, what each pipeline could and could not see, and what happened to each report.
# Every part is read defensively: a file that cannot be read leaves that part out and says so, never fails the digest.
# Defines functions only; loading it runs nothing.
. (Join-Path $PSScriptRoot 'report.ps1')

$script:QBDigestBuildLimit = 25

function Read-QBDigestJson {
    param([AllowNull()][string] $Path)
    if (-not $Path -or -not (Test-Path -LiteralPath $Path -PathType Leaf)) { return $null }
    try { return (Get-Content -LiteralPath $Path -Raw | ConvertFrom-Json -Depth 100) } catch { return $null }
}

function Format-QBDigestCell {
    # One table cell: a single line, no pipes or angle brackets, bounded.
    param([AllowNull()][object] $Text, [int] $Max = 160)
    $line = ([regex]::Replace([string]$Text, '\s+', ' ')).Trim().Replace('|', '/').Replace('<', '(').Replace('>', ')')
    if ($line.Length -gt $Max) { $line = $line.Substring(0, $Max - 3) + '...' }
    return $line
}

function Get-QBDigestUniqueText {
    # The collector joins one message per error; the same sentence often repeats, so each is kept once.
    param([AllowNull()][string] $Text)
    $parts = @(([string]$Text -split '(?<=\.)\s+') | Where-Object { $_.Trim() } | ForEach-Object { $_.Trim() } | Select-Object -Unique)
    return ($parts -join ' ')
}

function Get-QBDigestFirstError {
    # The first error line the collected evidence has for one build: a task's own error issue, else the first ##[error] line of a log excerpt.
    param([object] $Build, [object[]] $FailedTasks, [object[]] $RootLogs)
    foreach ($task in @($FailedTasks)) {
        foreach ($issue in @(Get-QBField $task 'issues' @())) {
            if ((Get-QBField $issue 'type') -eq 'error' -and (Get-QBField $issue 'message')) { return [string](Get-QBField $issue 'message') }
        }
    }
    $id = [string](Get-QBField $Build 'buildId')
    foreach ($log in @($RootLogs | Where-Object { [string](Get-QBField $_ 'buildId') -eq $id -or [string](Get-QBField $_ 'actualBuildId') -eq $id })) {
        $excerpt = [string](Get-QBField $log 'diagnosticExcerpt')
        if (-not $excerpt) { continue }
        $match = [regex]::Match($excerpt, '##\[error\](?<text>[^\r\n]+)')
        if ($match.Success) { return $match.Groups['text'].Value }
    }
    return ''
}

function Get-QBDigestProblemBuilds {
    # Problem builds of one pipeline scope (internal = dnceng, public = dnceng-public), newest first.
    param([object] $FailureTasks, [string] $Organization)
    return @(@(Get-QBField $FailureTasks 'builds' @()) | Where-Object {
            (Get-QBField $_ 'organization') -eq $Organization -and (Get-QBField $_ 'result') -in @('failed', 'canceled', 'partiallySucceeded')
        } | Sort-Object { try { [DateTimeOffset]::Parse([string](Get-QBField $_ 'finishTime'), [Globalization.CultureInfo]::InvariantCulture) } catch { [DateTimeOffset]::MinValue } } -Descending)
}

function New-QBEvidenceDigest {
    <#
    .SYNOPSIS
    The evidence digest as markdown. Never throws on missing or unreadable evidence; it says what it could not read.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][ValidatePattern('^\d{4}-\d{2}-\d{2}$')][string] $Date,
        [AllowNull()][object] $Evidence,
        [AllowNull()][object] $IssueEvidence,
        [AllowEmptyCollection()][object[]] $Results = @()
    )
    $lines = [Collections.Generic.List[string]]::new()
    $lines.Add("# VMR QB evidence digest - $Date")
    $lines.Add('')
    $lines.Add('> Generated without the model: the scripts built this list from the evidence they collected. It has no narrative and no owner routing, so decide who to ask yourself, and use the Build Health and Code Flow reports when they exist.')
    $lines.Add('')

    # What each report did.
    $lines.Add('## Reports')
    $lines.Add('')
    $lines.Add('| Report | Result | Detail |')
    $lines.Add('|---|---|---|')
    foreach ($result in @($Results)) {
        $ok = (Get-QBField $result 'status') -eq 'succeeded'
        $detail = if ($ok) { Split-Path ([string](Get-QBField $result 'path')) -Leaf } else { Format-QBDigestCell (Get-QBField $result 'error') 200 }
        $lines.Add("| $(Format-QBDigestCell (Get-QBField $result 'name' '?')) | $(if ($ok) { 'generated' } else { 'not generated' }) | $detail |")
    }
    if (-not @($Results).Count) { $lines.Add('| (none) | - | - |') }
    $lines.Add('')

    # What each pipeline could see.
    $pipelines = @(Get-QBField $Evidence 'pipelines' @())
    $lines.Add('## Coverage')
    $lines.Add('')
    $lines.Add('| Pipeline | What was collected | Completed | Failed | Canceled | Partially succeeded | Detail |')
    $lines.Add('|---|---|---|---|---|---|---|')
    foreach ($name in @('internal', 'public')) {
        $pipeline = @($pipelines | Where-Object { (Get-QBField $_ 'name') -eq $name }) | Select-Object -First 1
        if ($null -eq $pipeline) { $lines.Add("| $name | no evidence | - | - | - | - | The collector returned no record for this pipeline. |"); continue }
        $status = [string](Get-QBField $pipeline 'status' 'unknown')
        $counts = Get-QBField $pipeline 'counts'
        $cell = { param($key) if ($null -ne $counts) { [string](Get-QBField $counts $key '-') } else { 'unknown' } }
        $detail = Get-QBDigestUniqueText (Get-QBField $pipeline 'error')
        $fix = [string](Get-QBField $pipeline 'loginCommand')
        if ($fix) { $detail = "$detail Fix: $fix" }
        $label = switch ($status) {
            'available' { 'complete' }
            'partial' { 'partial - some evidence is missing' }
            'authentication_required' { "sign-in required - the $name builds are UNKNOWN, not healthy" }
            'unavailable' { "unavailable - the $name builds are UNKNOWN, not healthy" }
            default { $status }
        }
        $lines.Add("| $name | $(Format-QBDigestCell $label 80) | $(& $cell 'completed') | $(& $cell 'failed') | $(& $cell 'canceled') | $(& $cell 'partiallySucceeded') | $(Format-QBDigestCell $detail 220) |")
    }
    $lines.Add('')

    # The problem builds, internal first.
    $failureTasks = Read-QBDigestJson (Get-QBField $Evidence 'failureTasksPath')
    $rootLogs = @(Get-QBField (Read-QBDigestJson (Get-QBField $Evidence 'rootLogsPath')) 'logs' @())
    foreach ($scope in @(@{ name = 'internal'; org = 'dnceng'; title = 'Internal builds (dnceng/internal)' }, @{ name = 'public'; org = 'dnceng-public'; title = 'Public builds (dnceng-public/public)' })) {
        $lines.Add("## $($scope.title)")
        $lines.Add('')
        if ($null -eq $failureTasks) {
            $lines.Add('The failure evidence could not be read, so no builds are listed.')
            $lines.Add('')
            continue
        }
        $builds = @(Get-QBDigestProblemBuilds -FailureTasks $failureTasks -Organization $scope.org)
        if (-not $builds.Count) {
            $pipeline = @($pipelines | Where-Object { (Get-QBField $_ 'name') -eq $scope.name }) | Select-Object -First 1
            $seen = $null -ne $pipeline -and (Get-QBField $pipeline 'status') -eq 'available'
            $lines.Add($(if ($seen) { 'No failed, canceled or partially succeeded builds in the window.' } else { 'No builds are listed: this pipeline was not fully collected (see Coverage).' }))
            $lines.Add('')
            continue
        }
        $lines.Add('| Build | Branch | Result | Failed jobs and tasks | First error |')
        $lines.Add('|---|---|---|---|---|')
        foreach ($build in @($builds | Select-Object -First $script:QBDigestBuildLimit)) {
            $id = [string](Get-QBField $build 'buildId')
            $url = "https://dev.azure.com/$(Get-QBField $build 'organization')/$(Get-QBField $build 'project')/_build/results?buildId=$id"
            $failed = @(@(Get-QBField $build 'tasks' @()) | Where-Object { (Get-QBField $_ 'result') -in @('failed', 'canceled') })
            $where = @($failed | Select-Object -First 3 | ForEach-Object {
                    $job = [string](Get-QBField $_ 'job'); $task = [string](Get-QBField $_ 'task' '?')
                    if ($job -and $job -ne $task) { "$job / $task" } else { $task }
                }) -join '; '
            if ($failed.Count -gt 3) { $where += "; +$($failed.Count - 3) more" }
            if (-not $failed.Count) { $where = '(no failed task in the collected timeline)' }
            $first = Get-QBDigestFirstError -Build $build -FailedTasks $failed -RootLogs $rootLogs
            $branch = ([string](Get-QBField $build 'sourceBranch')) -replace '^refs/heads/', ''
            $lines.Add("| [$id]($url) | $(Format-QBDigestCell $branch 60) | $(Get-QBField $build 'result' '?') | $(Format-QBDigestCell $where 200) | $(Format-QBDigestCell $first 200) |")
        }
        if ($builds.Count -gt $script:QBDigestBuildLimit) { $lines.Add(''); $lines.Add("... and $($builds.Count - $script:QBDigestBuildLimit) more problem builds in the collected evidence.") }
        $lines.Add('')
    }

    # What the collector could not read.
    $errors = @(Get-QBField $Evidence 'requiredErrors' @())
    if ($errors.Count) {
        $lines.Add('## Evidence gaps')
        $lines.Add('')
        foreach ($error in @($errors | Select-Object -First 10)) {
            $lines.Add("- $(Format-QBDigestCell (Get-QBField $error 'scope' 'collector')): $(Format-QBDigestCell (Get-QBField $error 'message') 300)")
        }
        if ($errors.Count -gt 10) { $lines.Add("- ... and $($errors.Count - 10) more in the run's sources.json.") }
        $lines.Add('')
    }
    $issueErrors = @(Get-QBField $IssueEvidence 'errors' @())
    if ($issueErrors.Count) {
        $lines.Add('Issue and owner evidence was incomplete: ' + (Format-QBDigestCell ((@($issueErrors | Select-Object -First 3 | ForEach-Object { Get-QBField $_ 'message' }) -join ' ')) 300))
        $lines.Add('')
    }
    return (($lines -join "`n").TrimEnd() + "`n")
}
