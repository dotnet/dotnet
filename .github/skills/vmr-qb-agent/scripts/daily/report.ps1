Set-StrictMode -Version Latest

# daily/report.ps1 - The report contract: references (BH numbering), format validation and action text, and the email body.
# Dot-sourced by the entry scripts and the tests; it defines functions only and runs nothing.
. (Join-Path $PSScriptRoot '..\common\core.ps1')
. (Join-Path $PSScriptRoot 'notes.ps1')
. (Join-Path $PSScriptRoot 'evidence.ps1')
. (Join-Path $PSScriptRoot '..\triage\timeout.ps1')

# ---------------------------------------------------------------------------------------------
# report/References.ps1
# ---------------------------------------------------------------------------------------------
function Get-VmrQBReferenceField {
    param([AllowNull()][object] $Object, [string] $Name, [AllowNull()][object] $Default = $null)
    if ($null -eq $Object) { return $Default }
    if ($Object -is [Collections.IDictionary]) {
        if ($Object.Contains($Name)) { return $Object[$Name] }
    } else {
        $property = $Object.PSObject.Properties[$Name]
        if ($null -ne $property) { return $property.Value }
    }
    return $Default
}

function Get-VmrQBReferenceWords {
    param([string] $Text)
    $textOnly = [regex]::Replace($Text, '\[[^\]]*\]\(https://[^)\s]+\)|https://\S+|@[A-Za-z0-9_.-]+(?:/[A-Za-z0-9_.-]+)?', ' ')
    $words = [Collections.Generic.HashSet[string]]::new()
    $stop = @('the', 'and', 'for', 'from', 'with', 'this', 'that', 'build', 'failed', 'failure',
        'error', 'task', 'job', 'internal', 'reported', 'route', 'both', 'not', 'was', 'could' |
        ForEach-Object { $_.TrimEnd('s') })
    foreach ($match in [regex]::Matches($textOnly.ToLowerInvariant(), '[a-z0-9]{3,}')) {
        $word = $match.Value.TrimEnd('s')
        if ($word -notin $stop) { [void]$words.Add($word) }
    }
    return ,$words
}

function Get-VmrQBBuildIdsInText {
    # The build ids a Build Health action refers to: those in build links (...?buildId=123) and any plain number that looks like one
    # (6-9 digits, not part of a longer token, a #reference, a version or a path), because a model often writes the ids without their
    # links. A number only counts when a finding has that same build id, so one that is not a build id cannot match anything.
    param([AllowNull()][string] $Text)
    $ids = [Collections.Generic.HashSet[string]]::new()
    foreach ($match in [regex]::Matches([string]$Text, 'buildId=(\d+)')) { [void]$ids.Add($match.Groups[1].Value) }
    foreach ($match in [regex]::Matches([string]$Text, '(?<![\w#./=:-])(\d{6,9})(?![\w/-]|\.\d)')) { [void]$ids.Add($match.Groups[1].Value) }
    return @($ids | Sort-Object)
}

function Get-VmrQBReferenceActions {
    param([Parameter(Mandatory)][string] $Markdown)
    $section = [regex]::Match($Markdown, '(?ims)^##[ \t]+Priority actions[ \t]*\r?\n(?<body>.*?)(?=^##[ \t]|\z)')
    if (-not $section.Success) {
        $section = [regex]::Match($Markdown, '(?ims)^##[ \t]+(?:Verdict and top actions|Verdict and recommended actions|Top actions(?: for the QB today)?)[ \t]*\r?\n(?<body>.*?)(?=^##[ \t]|\z)')
    }
    if (-not $section.Success) { return @() }
    $seen = [Collections.Generic.HashSet[int]]::new()
    foreach ($item in [regex]::Matches($section.Groups['body'].Value, '(?ms)^(?<number>[1-9]\d*)\.[ \t]+(?<text>.*?)(?=^[1-9]\d*\.[ \t]|\z)')) {
        $number = [int]$item.Groups['number'].Value
        if (-not $seen.Add($number)) { throw "Build Health repeats visible action number $number." }
        $text = $item.Groups['text'].Value.Trim()
        $heading = [regex]::Match($text, '^\*\*(?<title>[^*]+)\*\*')
        $title = if ($heading.Success) { $heading.Groups['title'].Value.TrimEnd(':', '.', ' ') } else { $text }
        [pscustomobject]@{
            number = $number
            title = $title
            text = $text
            buildIds = @(Get-VmrQBBuildIdsInText $text)
        }
    }
}

function Find-VmrQBVisibleReference {
    param([object[]] $Actions, [string[]] $BuildIds, [string] $Label)
    $words = Get-VmrQBReferenceWords $Label
    $best = 0
    $pick = $null
    $ambiguous = $false
    foreach ($action in $Actions) {
        $overlap = @($BuildIds | Where-Object { $_ -in $action.buildIds }).Count
        if ($overlap -eq 0) { continue }
        $titleWords = Get-VmrQBReferenceWords $action.title
        $bodyWords = Get-VmrQBReferenceWords $action.text
        $titleMatch = @($titleWords | Where-Object { $words.Contains($_) }).Count
        $bodyMatch = @($bodyWords | Where-Object { $words.Contains($_) }).Count
        # Shared builds alone never establish that two rows describe the same failure.
        if ($titleMatch -eq 0 -and $bodyMatch -lt 2) { continue }
        $exact = $overlap -eq $BuildIds.Count -and $overlap -eq $action.buildIds.Count
        $score = $titleMatch * 100 + $bodyMatch * 10 + $overlap + $(if ($exact) { 20 } else { 0 })
        if ($score -gt $best) {
            $best = $score
            $pick = [int]$action.number
            $ambiguous = $false
        } elseif ($score -eq $best -and $pick -ne [int]$action.number) {
            $ambiguous = $true
        }
    }
    if ($ambiguous) { throw "Ambiguous Build Health reference for '$Label'; no number was guessed." }
    return $pick
}

function Set-VmrQBBuildHealthReferences {
    param([Parameter(Mandatory)][object] $Findings, [Parameter(Mandatory)][string] $Markdown)
    $actions = @(Get-VmrQBReferenceActions $Markdown)
    $groups = [ordered]@{}
    foreach ($build in @(Get-VmrQBReferenceField $Findings 'builds' @())) {
        foreach ($root in @(Get-VmrQBReferenceField $build 'roots' @())) {
            $id = [string]$root.id
            if (-not $groups.Contains($id)) {
                $groups[$id] = @{ ids = [Collections.Generic.HashSet[string]]::new(); labels = [Collections.Generic.List[string]]::new(); active = $false }
            }
            [void]$groups[$id].ids.Add([string]$build.buildId)
            $groups[$id].labels.Add("$id $($root.description) $($root.quote)")
            if ([int](Get-VmrQBReferenceField $root 'priority' 0) -gt 0) { $groups[$id].active = $true }
        }
    }
    $map = @{}
    foreach ($id in $groups.Keys) {
        $group = $groups[$id]
        if (-not $group.active) { continue }
        $number = Find-VmrQBVisibleReference -Actions $actions -BuildIds @($group.ids) -Label ($group.labels -join ' ')
        if ($null -eq $number) {
            throw "Finding '$id' has no matching visible Build Health action. Fix the BH list rather than inventing a summary number."
        }
        $map[[string]$id] = $number
    }
    foreach ($build in @(Get-VmrQBReferenceField $Findings 'builds' @())) {
        foreach ($root in @(Get-VmrQBReferenceField $build 'roots' @())) {
            $number = if ($map.ContainsKey([string]$root.id)) { $map[[string]$root.id] } else { 0 }
            if ($root -is [Collections.IDictionary]) { $root['priority'] = $number }
            else { $root | Add-Member -NotePropertyName priority -NotePropertyValue $number -Force }
        }
    }
    $values = @{
        bhReferences = $map
        bhReferenceActions = $actions
        bhReferenceSource = 'visible-build-health-actions'
    }
    foreach ($name in $values.Keys) {
        if ($Findings -is [Collections.IDictionary]) { $Findings[$name] = $values[$name] }
        else { $Findings | Add-Member -NotePropertyName $name -NotePropertyValue $values[$name] -Force }
    }
    return $Findings
}

function Get-VmrQBBuildHealthReference {
    param([AllowNull()][object] $Findings, [string] $FindingId)
    $map = Get-VmrQBReferenceField $Findings 'bhReferences'
    $value = Get-VmrQBReferenceField $map $FindingId
    if ($null -eq $value) { return $null }
    if ([string]$value -notmatch '^[1-9]\d*$') { throw "Invalid stored Build Health reference for '$FindingId'." }
    return [int]$value
}

# ---------------------------------------------------------------------------------------------
# report/Contract.ps1
# ---------------------------------------------------------------------------------------------
# A wrapper diagnostic (MSB3073 and friends) normally hides the real error and is dropped. When a job
# retains nothing else - common on Windows legs, where repo builds redirect inner output to a log file -
# dropping it leaves the finding with no evidence and invites a vague summary. Rewrite the wrapper into a
# specific statement naming the repo, exit code and the inner log that was not collected.
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

function Read-QBStructuredSection {
    param([string] $Content, [string] $Marker)
    # The JSON is on its own lines in the documented form; the same comment on one line, or with other spacing, is read too.
    $pattern = '(?s)<!--\s*' + [regex]::Escape($Marker) + '\s*(?<json>.*?)\s*' +
        [regex]::Escape($Marker) + '\s*-->'
    $matches = [regex]::Matches($Content, $pattern)
    if ($matches.Count -ne 1) { throw "Expected exactly one $Marker JSON comment." }
    try { $data = $matches[0].Groups['json'].Value | ConvertFrom-Json -Depth 100 }
    catch { throw "Invalid $Marker JSON: $($_.Exception.Message)" }
    if ((Get-QBField $data 'schemaVersion') -ne 1) { throw "Unsupported $Marker schema." }
    [pscustomobject]@{
        Content = $Content.Remove($matches[0].Index, $matches[0].Length).TrimEnd() + "`n"
        Data = $data
    }
}

function Get-QBInternalPipeline {
    param([object] $Evidence)
    $pipelines = @(Get-QBField $Evidence 'pipelines' @())
    $internal = @($pipelines | Where-Object { (Get-QBField $_ 'name') -eq 'internal' })
    if ($internal.Count -ne 1) { throw 'Fresh run evidence must contain exactly one internal pipeline record.' }
    return $internal[0]
}

function Format-QBUtcTimestamp {
    param([Parameter(Mandatory)][object] $Value)
    $timestamp = if ($Value -is [DateTimeOffset]) { $Value }
        elseif ($Value -is [DateTime]) { [DateTimeOffset]::new($Value) }
        else { [DateTimeOffset]::Parse([string]$Value, [Globalization.CultureInfo]::InvariantCulture) }
    return $timestamp.ToUniversalTime().ToString('yyyy-MM-ddTHH:mm:ss.fffZ')
}

function Get-QBInventoryBuilds {
    param([string] $Path)
    if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) { throw "Missing scoped inventory: $Path" }
    $value = Get-Content -LiteralPath $Path -Raw | ConvertFrom-Json -Depth 100
    if ($null -ne (Get-QBField $value 'value')) { return @(Get-QBField $value 'value') }
    if ($null -ne (Get-QBField $value 'builds')) { return @(Get-QBField $value 'builds') }
    return @($value)
}

function Get-QBInternalEvidenceText {
    param([object] $Evidence, [int] $BuildId)
    $parts = [Collections.Generic.List[string]]::new()
    $logsPath = [string](Get-QBField $Evidence 'rootLogsPath')
    if ($logsPath -and (Test-Path -LiteralPath $logsPath)) {
        $logs = Get-Content -LiteralPath $logsPath -Raw | ConvertFrom-Json -Depth 100
        foreach ($log in @(Get-QBField $logs 'logs' @())) {
            if ((Get-QBField $log 'buildId') -ne $BuildId -or -not (Get-QBField $log 'available' $false)) { continue }
            $path = [string](Get-QBField $log 'path')
            if ($path -and (Test-Path -LiteralPath $path)) { $parts.Add((Get-Content -LiteralPath $path -Raw)) }
        }
    }
    foreach ($source in @(Get-QBField $Evidence 'sources' @())) {
        if ((Get-QBField $source 'organization') -ne 'dnceng' -or
            (Get-QBField $source 'apiPath') -notmatch "^build/builds/$BuildId/timeline(?:\?|$)" -or
            (Get-QBField $source 'status') -ne 'available') { continue }
        $timeline = Get-Content -LiteralPath $source.path -Raw | ConvertFrom-Json -Depth 100
        foreach ($record in $timeline.records) {
            foreach ($issue in @(Get-QBField $record 'issues' @())) {
                $message = [string](Get-QBField $issue 'message')
                if ($message) { $parts.Add($message) }
            }
        }
    }
    # Retained problem-task issues. This is the only evidence for a build that produced no
    # timeline at all (YAML pre-flight rejection), where the diagnosis is validationResults.
    $tasksPath = [string](Get-QBField $Evidence 'failureTasksPath')
    if ($tasksPath -and (Test-Path -LiteralPath $tasksPath)) {
        $failures = Get-Content -LiteralPath $tasksPath -Raw | ConvertFrom-Json -Depth 100
        foreach ($build in @(Get-QBField $failures 'builds' @())) {
            if ((Get-QBField $build 'buildId') -ne $BuildId -or
                (Get-QBField $build 'organization') -ne 'dnceng') { continue }
            foreach ($task in @(Get-QBField $build 'tasks' @())) {
                foreach ($issue in @(Get-QBField $task 'issues' @())) {
                    $message = [string](Get-QBField $issue 'message')
                    if ($message) { $parts.Add($message) }
                }
            }
        }
    }
    return [regex]::Replace(($parts -join "`n"), '\s+', ' ')
}

function Get-QBRoutingFacts {
    <#
    .SYNOPSIS
    What the findings validator needs from the routing registry, read when it runs and never when this file loads.
    .DESCRIPTION
    RepoTeamAliases: repo names the model mistakes for team handles, each mapped to the real owning team from the Guide's
    Repo Ownership table (derived by the registry loader, never listed by hand), so an obvious slip is corrected rather
    than failing the whole run. UnmappedLabel: the owner a finding gets when nothing maps; it names no team, so it is never
    validated as a handle. SupportNames: external support destinations (for example 1ES support), valid owners that are
    places rather than GitHub teams.
    #>
    $routing = Get-VmrQBRouting
    $aliases = @{}
    foreach ($handle in $routing.HandleAliases.Keys) { $aliases[$handle.ToLowerInvariant()] = $routing.HandleAliases[$handle] }
    foreach ($handle in $routing.PreferredRedirects.Keys) { $aliases[$handle.ToLowerInvariant()] = $routing.PreferredRedirects[$handle] }
    return [pscustomobject]@{
        RepoTeamAliases = $aliases
        UnmappedLabel   = [string]$routing.UnmappedLabel
        SupportNames    = @($routing.SupportDestinations | ForEach-Object { $_.Name })
    }
}

function Assert-QBInternalFindings {
    param([object] $Findings, [object] $Evidence, [string] $Date, [string[]] $KnownOwnerTeams = @())
    $routingFacts = Get-QBRoutingFacts
    $internal = Get-QBInternalPipeline $Evidence
    if ((Get-QBField $Findings 'reportDate') -ne $Date) { throw 'Internal findings have the wrong report date.' }
    if ((Get-QBField $Findings 'sourceStatus') -ne $internal.status) {
        throw 'Internal findings contradict the collector authentication/coverage status.'
    }
    $reported = @(Get-QBField $Findings 'builds' @())
    if (@($reported | Group-Object buildId | Where-Object Count -gt 1).Count) { throw 'Duplicate internal build assessments.' }
    $required = @((Get-QBField $internal 'requiredProblemBuildIds' @()) | ForEach-Object { [int]$_ } | Sort-Object)
    $reportedIds = @($reported | ForEach-Object { [int]$_.buildId } | Sort-Object)
    if (($required -join ',') -ne ($reportedIds -join ',')) {
        throw "Internal findings must cover exactly the scoped problem builds: $($required -join ', ')."
    }
    $misflagged = [Collections.Generic.List[string]]::new()
    $misnamed = [Collections.Generic.List[string]]::new()
    # A task can fail without failing the build. Findings on a partiallySucceeded build
    # are monitor-only: escalating them is a false alarm.
    $buildResults = @{}
    foreach ($pipelineRecord in @(Get-QBField $Evidence 'pipelines' @())) {
        $inventory = [string](Get-QBField $pipelineRecord 'inventoryPath')
        if (-not $inventory -or -not (Test-Path -LiteralPath $inventory)) { continue }
        foreach ($build in @(Get-QBInventoryBuilds $inventory)) {
            $buildResults[[string]$build.id] = [string](Get-QBField $build 'result')
        }
    }
    # Only accept a well-formed roster. A malformed list means the caller's wiring is
    # broken, and silently rejecting every real owner is far worse than skipping the check.
    $suppliedTeams = @($KnownOwnerTeams)
    $knownHandles = @($suppliedTeams | Where-Object { $_ -match '^@[A-Za-z0-9][A-Za-z0-9-]*/[A-Za-z0-9._-]+$' })
    if ($suppliedTeams.Count -gt 0 -and $knownHandles.Count -eq 0) {
        Write-Warning 'Owner-handle validation was skipped: the supplied roster contained no valid @org/team entries.'
    }
    foreach ($build in $reported) {
        $roots = @(Get-QBField $build 'roots' @())
        if ($roots.Count -eq 0) { throw "Internal build $($build.buildId) has no failure/cancellation assessment." }
        $evidenceText = Get-QBInternalEvidenceText -Evidence $Evidence -BuildId $build.buildId
        foreach ($root in $roots) {
            foreach ($field in @('id', 'description', 'quote')) {
                if ([string]::IsNullOrWhiteSpace([string](Get-QBField $root $field))) {
                    throw "Internal build $($build.buildId) has no $field for a finding."
                }
            }
            $quote = [regex]::Replace([string]$root.quote, '\s+', ' ').Trim()
            if ($quote.Length -lt 12 -or $evidenceText.IndexOf($quote, [StringComparison]::OrdinalIgnoreCase) -lt 0) {
                throw "Finding '$($root.id)' for $($build.buildId) lacks a diagnostic quote from that build's retained evidence."
            }
            if ($quote -match '^(?:Bash exited with code|Script failed with exit code|The operation was canceled\.?$|Encountered an error in generation workflow$)') {
                throw "Finding '$($root.id)' only quotes a wrapper/cascade symptom; use the retained initiating diagnostic or explicitly unavailable root evidence."
            }
            $cancellation = $quote -match '(?i)build was cancel[le]+d by|cancel[le]+d by its requester'
            $buildResult = [string]$buildResults[[string]$build.buildId]
            # A finding named after its hosting test misroutes the escalation. If the id or
            # description calls this a dnx failure while the quote is a different terminating
            # error, the name is wrong.
            # Check the ID only: the description legitimately mentions the dnx console noise
            # when explaining what the failure is NOT, and that must not trip the guard.
            $rootLabel = ([string]$root.id).ToLowerInvariant()
            if ($rootLabel -match '\bdnx\b' -and $evidenceText -match 'NETSDK1045|does not support targeting \.NET') {
                $entry = "$($root.id) (build has a NETSDK1045 target-framework failure; this is not a dnx defect)"
                if ($entry -notin $misnamed) { $misnamed.Add($entry) }
            }
            # A bare test identity (Namespace.Class.TestName(args)) names the host, not the
            # failure. Without an error token it cannot establish a root cause, and it is how
            # a leg gets mislabelled after whichever test happened to run.
            if ($quote -match '^[A-Za-z][A-Za-z0-9_]*(?:\.[A-Za-z0-9_]+){2,}\s*\(' -and
                $quote -notmatch '(?i)error|fail|exception|cannot|could not|unable|timed out|timeout|denied|not found|busy|exit code') {
                $entry = "$($root.id) (quote is a test identity, not a diagnostic)"
                if ($entry -notin $misnamed) { $misnamed.Add($entry) }
            }
            if ($buildResult -eq 'partiallySucceeded' -and -not (Get-QBField $root 'nonBlocking' $false)) {
                if ([string]$root.id -notin $misflagged) { $misflagged.Add([string]$root.id) }
            }
            $teams = @(Get-QBField $root 'ownerTeams' @())
            if ($teams.Count -eq 0 -and -not $cancellation) {
                # Never fail because of an owner: a finding with none is shown as unidentified, and a note says so.
                $teams = @($routingFacts.UnmappedLabel)
                $root | Add-Member -NotePropertyName ownerTeams -NotePropertyValue $teams -Force
                Add-QBRunNote -Source 'owners' -Message "Finding '$($root.id)' had no owner, so it is shown as '$($routingFacts.UnmappedLabel)'."
            }
            if ($knownHandles.Count -gt 0) {
                $corrected = [Collections.Generic.List[string]]::new()
                foreach ($team in $teams) {
                    $handle = ([string]$team).Trim()
                    if (-not $handle) { continue }
                    if ($handle -ieq $routingFacts.UnmappedLabel) { $corrected.Add($routingFacts.UnmappedLabel); continue }
                    $supportName = @($routingFacts.SupportNames | Where-Object { $_ -ieq $handle }) | Select-Object -First 1
                    if ($supportName) { $corrected.Add([string]$supportName); continue }
                    if ($handle -in $knownHandles) { $corrected.Add($handle); continue }
                    # The model repeatedly writes the REPO name as the handle (@dotnet/efcore,
                    # @dotnet/sdk). Those are not teams. When the repo maps to a
                    # real team, auto-correct instead of failing the whole report.
                    $alias = $routingFacts.RepoTeamAliases[$handle.ToLowerInvariant()]
                    if ($alias -and $alias -in $knownHandles) {
                        Write-Warning "Owner handle '$handle' is not a GitHub team; using the mapped owner '$alias'."
                        $corrected.Add($alias)
                        continue
                    }
                    # Never fail because of an owner: the closest team, else the handle as written, and a note says which.
                    $close = Find-VmrQBClosestTeam -Handle $handle -Team $knownHandles
                    if ($close) {
                        Add-QBRunNote -Source 'owners' -Message "Owner '$handle' for finding '$($root.id)' is not a team in the Guide; using the closest team $close."
                        $corrected.Add($close)
                    }
                    else {
                        Add-QBRunNote -Source 'owners' -Message "Owner '$handle' for finding '$($root.id)' is not a team in the Guide and nothing is close to it; kept as written. Confirm it before pinging."
                        $corrected.Add($handle)
                    }
                }
                if ($corrected.Count) {
                    $root | Add-Member -NotePropertyName ownerTeams -NotePropertyValue @($corrected | Select-Object -Unique) -Force
                }
            }
        }
    }
    if ($misnamed.Count) {
        throw ("These findings are named after the hosting test rather than the diagnostic that fired: " +
            ($misnamed -join '; ') + '. Name the finding after its terminating diagnostic.')
    }
    if ($misflagged.Count) {
        throw ("These findings are on a partiallySucceeded build, so the failing task did not fail the build: " +
            ($misflagged -join ', ') + '. Set nonBlocking:true on each; they are monitor-only and must not be escalated.')
    }
}

function ConvertTo-QBInternalSection {
    param([object] $Evidence, [object] $Findings)
    $internal = Get-QBInternalPipeline $Evidence
    $lines = [Collections.Generic.List[string]]::new()
    $lines.Add('## Internal pipeline health (verified snapshot)')
    $lines.Add('')
    $start = Format-QBUtcTimestamp $Evidence.windowStartUtc
    $end = Format-QBUtcTimestamp $Evidence.observationCutoffUtc
    $lines.Add("Source: dnceng/internal. Collector status: **$($internal.status)**. Window (UTC): $start through $end.")
    $lines.Add('')
    $counts = Get-QBField $internal 'counts'
    if ($null -ne (Get-QBField $counts 'completed')) {
        $total = [int]$counts.completed
        $rate = if ($total -gt 0) { '{0:F1}%' -f (100.0 * [int]$counts.succeeded / $total) } else { 'N/A' }
        $lines.Add('| Type | Completed | Pass | Fail | Partially succeeded | Canceled | Pass rate |')
        $lines.Add('|---|---:|---:|---:|---:|---:|---:|')
        $lines.Add("| Rolling | $total | $($counts.succeeded) | $($counts.failed) | $($counts.partiallySucceeded) | $($counts.canceled) | $rate |")
        $lines.Add('')
        $lines.Add('Pass rate counts only fully successful builds. Cancellations and partial successes remain separate outcomes. All source branches in this section belong to the internal pipeline.')
    } else {
        $lines.Add('**Internal health is unavailable, not healthy. No internal pass rate can be computed.**')
    }
    $errorText = [string](Get-QBField $internal 'error')
    if ($errorText) { $lines.Add(''); $lines.Add("Coverage error: $errorText") }
    $login = [string](Get-QBField $internal 'loginCommand')
    if ($login) {
        $lines.Add('')
        $lines.Add('**Authentication required:** sign in with the Microsoft work account authorized for dnceng/internal:')
        $lines.Add('```powershell'); $lines.Add($login); $lines.Add('```')
        $lines.Add('This is monitoring access, distinct from HTTP 401 errors occurring inside a CI test.')
    }
    $builds = @(Get-QBField $Findings 'builds' @())
    if ($builds.Count) {
        $inventory = @(Get-QBInventoryBuilds -Path $internal.inventoryPath)
        foreach ($family in @('internal/release/*', 'main + release/*')) {
            $selected = @($inventory | Where-Object {
                $_.id -in @($builds.buildId) -and
                (($_.sourceBranch -like 'refs/heads/internal/release/*') -eq ($family -eq 'internal/release/*'))
            } | Sort-Object finishTime)
            if (-not $selected.Count) { continue }
            $lines.Add(''); $lines.Add("### $family"); $lines.Add('')
            $lines.Add('| Build | Type | Source | Failure detail | Mapped category / owner(s) |')
            $lines.Add('|---|---|---|---|---|')
            foreach ($build in $selected) {
                $finding = $builds | Where-Object buildId -eq $build.id | Select-Object -First 1
                $detail = (@($finding.roots | ForEach-Object { ([string]$_.description).Replace('|', '\|') }) -join '<br>')
                $owners = (@($finding.roots | ForEach-Object { @(Get-QBField $_ 'ownerTeams' @()) } | Sort-Object -Unique) -join ', ')
                if (-not $owners) { $owners = 'N/A - requester cancellation' }
                $branch = $build.sourceBranch.Replace('refs/heads/', '')
                $lines.Add("| [$($build.id)](https://dev.azure.com/dnceng/internal/_build/results?buildId=$($build.id)) | Rolling ($($build.result)) | $branch | $detail | $owners |")
            }
        }
    }
    $lines.Add('')
    return $lines -join "`n"
}

function Assert-QBDailyActions {
    param([object] $Plan, [object] $Findings, [object] $IssueEvidence, [string] $Date, [hashtable] $KnownTrackers = @{})
    if ((Get-QBField $Plan 'reportDate') -ne $Date) { throw 'Daily action plan has the wrong report date.' }
    $actions = @(Get-QBField $Plan 'actions' @())
    $required = @((Get-QBField $Findings 'builds' @()) | ForEach-Object { @(Get-QBField $_ 'roots' @()) } |
        ForEach-Object id | Sort-Object -Unique)
    foreach ($id in $required) {
        if (@($actions | Where-Object findingId -eq $id).Count -ne 1) { throw "Action plan must cover internal finding '$id' exactly once." }
    }
    if (@($actions | Where-Object { $_.findingId -notin $required }).Count) {
        throw 'The internal action plan contains a finding absent from the verified internal assessment.'
    }
    $issues = @(Get-QBField $IssueEvidence 'issues' @())
    foreach ($action in $actions) {
        if ($action.decision -notin @('new_issue', 'existing_issue', 'possible_recurrence', 'monitor_only', 'needs_triage')) {
            throw "Unsupported action decision for $($action.findingId)."
        }
        if (@(Get-QBField $action 'buildIds' @()).Count -eq 0) { throw "Action '$($action.findingId)' has no affected build IDs." }
        $matchingBuildIds = @((Get-QBField $Findings 'builds' @()) | Where-Object {
            $action.findingId -in @($_.roots.id)
        } | ForEach-Object buildId)
        foreach ($buildId in $matchingBuildIds) {
            if ($buildId -notin @($action.buildIds)) { throw "Action '$($action.findingId)' omits internal build $buildId." }
        }
        if (@($action.buildIds | Where-Object { $_ -notin $matchingBuildIds }).Count) {
            throw "Action '$($action.findingId)' attributes the failure to an unrelated internal build."
        }
        if ($action.decision -in @('existing_issue', 'possible_recurrence')) {
            $url = [string](Get-QBField $action 'issueUrl')
            if (-not $url -or $url -notin @($issues.url)) { throw "Existing tracker for '$($action.findingId)' is not in the collected issue evidence." }
        }
        $hasDraft = -not [string]::IsNullOrWhiteSpace([string](Get-QBField $action 'body'))
        if ($action.decision -eq 'needs_triage' -and -not (Get-QBField $action 'issueUrl') -and -not $hasDraft) {
            # The finding still appears, without a draft; it must not cost every other finding its draft.
            Add-QBRunNote -Source 'action-plan' -Message "Untracked actionable finding '$($action.findingId)' has no conditional copy-ready title/body, only a triage label." -Fix 'Draft the issue by hand from the finding''s evidence and owner.'
        }
        if ($action.decision -eq 'new_issue' -or $hasDraft) {
            # A finding we already track must never be drafted as new. The model only sees today's
            # evidence, so a tracker recorded on a previous day is invisible to it and it proposes
            # filing a duplicate. Operator-recorded trackers are authoritative here.
            $tracked = $null
            foreach ($key in @([string]$action.findingId, [string](Get-QBField $action 'title'))) {
                if ($key -and $KnownTrackers.ContainsKey($key)) { $tracked = $KnownTrackers[$key]; break }
            }
            if ($tracked) {
                throw "Finding '$($action.findingId)' is already tracked by $tracked; record it as existing_issue instead of drafting a new one."
            }
            if ($action.decision -eq 'new_issue' -and (Get-QBField $IssueEvidence 'status') -ne 'complete') {
                throw 'A new-issue recommendation cannot claim deduplication while issue evidence collection is incomplete.'
            }
            if ($action.decision -notin @('new_issue', 'needs_triage', 'possible_recurrence')) {
                throw 'Existing tracked or monitor-only cases must not generate duplicate new-issue drafts.'
            }
            foreach ($field in @('title', 'body', 'searchBasis')) {
                if ([string]::IsNullOrWhiteSpace([string](Get-QBField $action $field))) { throw "New issue '$($action.findingId)' has no $field." }
            }
            foreach ($heading in @('### Error', '### Links', '### Root Cause', '### Blocking', '### Affected Releases')) {
                if (-not $action.body.Contains($heading)) { throw "New issue '$($action.findingId)' is missing $heading." }
            }
            # Report every missing link at once: the single repair pass cannot converge
            # if it is told about one absent build URL per attempt.
            $missing = @($action.buildIds | Where-Object {
                -not $action.body.Contains("https://dev.azure.com/dnceng/internal/_build/results?buildId=$_")
            })
            if ($missing.Count) {
                throw ("New issue '$($action.findingId)' does not include affected build(s) " +
                    ($missing -join ', ') + " in its copy-ready body. Its ### Links section must list the internal build URL for all " +
                    "$(@($action.buildIds).Count) affected build IDs: " + (@($action.buildIds) -join ', ') + '.')
            }
        }
        $assignee = [string](Get-QBField $action 'assignee')
        if ($assignee) {
            $candidate = @((Get-QBField $IssueEvidence 'assigneeCandidates' @()) | Where-Object {
                $_.login -eq $assignee.TrimStart('@') -and $_.assignable -eq $true
            })
            if (-not $candidate.Count) { throw "Suggested assignee '$assignee' has not been verified as selectable." }
            if ([string]::IsNullOrWhiteSpace([string](Get-QBField $action 'assigneeBasis'))) {
                throw "Suggested assignee '$assignee' has no stated triage basis."
            }
        }
    }
}

function ConvertTo-QBDailyActionSection {
    param([object] $Plan, [object] $Findings)
    # findingId is the same stable key the build-health roots use, so the two documents can
    # be cross-referenced by number instead of by eye.
    $lines = [Collections.Generic.List[string]]::new()
    $lines.Add('## Internal issue tracking and action drafts')
    $lines.Add('')
    $lines.Add('Drafts only: no issues, comments, assignments, messages, or reruns were submitted. Individual names are suggested triage assignees, not confirmed defect owners.')
    $first = $true
    foreach ($action in @(Get-QBField $Plan 'actions' @())) {
        $links = @($action.buildIds | ForEach-Object { "[$_](https://dev.azure.com/dnceng/internal/_build/results?buildId=$_)" }) -join ', '
        # Rule between drafts: consecutive copy-ready bodies otherwise run together.
        if (-not $first) { $lines.Add(''); $lines.Add('---') }
        $first = $false
        $reference = Get-VmrQBBuildHealthReference -Findings $Findings -FindingId ([string]$action.findingId)
        $badge = if ($null -ne $reference) {
            "Build health priority #$reference &mdash; "
        } else { '' }
        $lines.Add(''); $lines.Add("### $badge$($action.findingId)"); $lines.Add('')
        $lines.Add("**Builds:** $links")
        $lines.Add("**Disposition:** $($action.decision)")
        $issue = [string](Get-QBField $action 'issueUrl')
        if ($issue) { $lines.Add("**Existing / related tracker:** $issue") }
        $reason = [string](Get-QBField $action 'reason')
        if ($reason) { $lines.Add("**Evidence / scope:** $reason") }
        $assignee = [string](Get-QBField $action 'assignee')
        if ($assignee) {
            $lines.Add("**Suggested triage assignee:** @$($assignee.TrimStart('@')) - $($action.assigneeBasis)")
        } else { $lines.Add('**Individual assignee:** not confirmed; use the owner-team routing in the summary table.') }
        if (-not [string]::IsNullOrWhiteSpace([string](Get-QBField $action 'body'))) {
            $lines.Add('')
            if ($action.decision -ne 'new_issue') {
                $lines.Add('**Conditional draft only:** check the related tracking/scope with the owner before opening a new issue.')
            }
            $lines.Add('**Proposed title:** ' + $action.title)
            $lines.Add('**Copy-ready body:**')
            $lines.Add('````markdown'); $lines.Add([string]$action.body); $lines.Add('````')
            $lines.Add('**Search basis:** ' + $action.searchBasis)
        }
        $comment = [string](Get-QBField $action 'comment')
        if ($comment -and $action.decision -in @('existing_issue', 'possible_recurrence')) {
            $lines.Add(''); $lines.Add('**Proposed comment (only if this evidence is not already posted):**')
            $lines.Add('````markdown'); $lines.Add($comment); $lines.Add('````')
        }
    }
    $lines.Add('')
    return $lines -join "`n"
}

function Get-QBCoverageAlerts {
    param([object] $Evidence, [object] $IssueEvidence)
    $alerts = [Collections.Generic.List[string]]::new()
    $errors = @(Get-QBField $Evidence 'requiredErrors' @())
    $orderedErrors = @($errors | Sort-Object { -not [bool](Get-QBField $_ 'authenticationRequired' $false) })
    foreach ($error in @($orderedErrors | Select-Object -First 8)) {
        $message = [string](Get-QBField $error 'message')
        if ($message.Length -gt 1200) { $message = $message.Substring(0, 1200) + ' (full details retained in sources.json)' }
        if ($message) { $alerts.Add("$($error.scope): $message") }
        $signIn = [string](Get-QBField $error 'signInCommand')
        if ($signIn) { $alerts.Add("Authenticate the Microsoft work account for dnceng/internal: $signIn") }
    }
    if ($errors.Count -gt 8) { $alerts.Add("$($errors.Count - 8) additional pipeline evidence errors are retained in the run's sources.json.") }
    $internal = Get-QBInternalPipeline $Evidence
    if ($internal.status -ne 'available' -and $alerts.Count -eq 0) {
        $alerts.Add("Internal pipeline coverage is $($internal.status), not verified complete.")
        $command = [string](Get-QBField $internal 'loginCommand')
        if ($command) { $alerts.Add("Authenticate the Microsoft work account for dnceng/internal: $command") }
    }
    if ((Get-QBField $Evidence 'status') -ne 'complete' -and $alerts.Count -eq 0) {
        $alerts.Add('Pipeline evidence is incomplete; do not treat missing internal results as healthy.')
    }
    if ((Get-QBField $IssueEvidence 'status') -ne 'complete') {
        $alerts.Add('Issue/owner evidence is incomplete; new-issue and individual-assignee recommendations are limited.')
        $issueErrors = @(Get-QBField $IssueEvidence 'errors' @())
        foreach ($error in @($issueErrors | Select-Object -First 5)) {
            $message = [string](Get-QBField $error 'message')
            if ($message) { $alerts.Add($message) }
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
        if (-not $state.hasMarkers) { return $null }
        $attribution = if ($null -ne $TimeLimitMinutes -and $TimeLimitMinutes -ge 1) { Get-VmrQBTimeoutAttribution -State $state -TimeLimitMinutes ([int]$TimeLimitMinutes) }
                       else { Get-VmrQBTimeoutAttribution -State $state -LimitIsAssumed }
        return [ordered]@{
            reliable = $state.reliable; truncated = $state.truncated; elapsedMinutes = $state.elapsedMinutes
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

function ConvertTo-QBModelPullRequest {
    <#
    .SYNOPSIS
    The compact view of the collector's referenced PRs (triage/issue.ps1) that the model reads, under the collector's own field names.
    .DESCRIPTION
    One schema from the collector to the prompt: merged (the flag), mergedAtUtc, baseRefName (the branch it targets), headRefName,
    observedAtUtc and metadataPath (where the retained API response is, to verify against). A missing field stays null rather than
    being renamed away; selecting names the collector never wrote is how a merged PR once reached the model as closed with no
    merge time and no pointer.
    #>
    param([AllowNull()][AllowEmptyCollection()][object[]] $PullRequest = @())
    foreach ($pr in @($PullRequest)) {
        [ordered]@{
            repository = Get-QBField $pr 'repository'; number = Get-QBField $pr 'number'; title = Get-QBField $pr 'title'
            state = Get-QBField $pr 'state'; url = Get-QBField $pr 'url'
            merged = Get-QBField $pr 'merged'; mergedAtUtc = Get-QBField $pr 'mergedAtUtc'
            baseRefName = Get-QBField $pr 'baseRefName'; headRefName = Get-QBField $pr 'headRefName'
            observedAtUtc = Get-QBField $pr 'observedAtUtc'; metadataPath = Get-QBField $pr 'metadataPath'
            relationship = Get-QBField $pr 'relationship'
        }
    }
}

function New-QBModelEvidence {
    param([object] $Evidence, [object] $IssueEvidence, [string] $OutputDirectory)
    [void][IO.Directory]::CreateDirectory($OutputDirectory)
    function Save-ModelJson([string] $Name, [object] $Value) {
        $path = Join-Path $OutputDirectory $Name
        ConvertTo-Json -InputObject $Value -Depth 40 -Compress:($Name -eq 'model-pipeline-evidence.json') |
            Set-Content -LiteralPath $path -Encoding utf8
        return $path
    }
    function Get-DiagnosticSeverity([string] $Text, [string] $TaskResult, [string] $JobResult) {
        # Record-based first, matching the pipeline-investigation skill: an issue is fatal
        # only if its task failed AND its parent Job failed. 1ES/Guardian tasks emit
        # ##[error] and still let the job pass (continueOnError), so log text alone lies.
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
        $value = [string]$Text
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
    function Get-DiagnosticSummary([object[]] $Tasks, [object[]] $Logs, [object[]] $AllRecords = @()) {

        # Parent Job results, so a task failure can be tested against whether its job failed.
        # Built from the UNFILTERED record set: a job that SUCCEEDED is exactly the case that
        # proves its failed task was non-fatal, and the filtered task list drops those.
        $jobResults = @{}
        $jobResultsById = @{}
        foreach ($record in $AllRecords) {
            if ((Get-QBField $record 'recordType') -ne 'Job') { continue }
            $jobName = [string](Get-QBField $record 'task')
            if ($jobName) { $jobResults[$jobName] = [string](Get-QBField $record 'result') }
            # Two legs can share a display name; the record id is the unambiguous key when the task carries it.
            $jobRecordId = [string](Get-QBField $record 'id')
            if ($jobRecordId) { $jobResultsById[$jobRecordId] = [string](Get-QBField $record 'result') }
        }
        $messages = @(
            foreach ($task in $Tasks) {
                foreach ($text in @($task.issues)) {
                    @{ text = [string]$text; task = $task.task; job = $task.job; jobId = [string](Get-QBField $task 'jobId'); path = $null; line = $null
                        taskResult = [string](Get-QBField $task 'result') }
                }
            }
            foreach ($log in $Logs) {
                if ((Get-QBField $log 'kind') -eq 'cascadeSummary') { continue }
                foreach ($excerpt in @(Get-QBField $log 'excerpts' @())) {
                    @{ text = [string]$excerpt.text; task = $log.task; job = $log.job; jobId = [string](Get-QBField $log 'jobId'); path = $log.path; line = $excerpt.line
                        taskResult = [string](Get-QBField $log 'result') }
                }
            }
        )
        $patterns = @(
            "Request was blocked due to exceeding usage of resource '[^']+' in namespace '[^']+'"
            'MSBuild server failed to shut down:[^\r\n]+'
            'Object reference not set to an instance of an object\.?'
            'Problem starting[^\r\n]*CredentialProvider[^\r\n]*'
            'Unhandled exception:\s*A task was canceled\.?'
            'Invalid argument:\s*[-/][A-Za-z][A-Za-z0-9_-]*'
            'Text file busy'
            'The task has timed out\.?'
            'ran longer than the maximum time of \d+ minutes'
            'We stopped hearing from agent [^.\r\n]+\.?'
            'The build was canceled by [^\r\n]+'
            "Channel with ID '[^']+' is not configured[^\r\n]*"
            'Unable to finish invocation or drain its output after[^\r\n]+'
            'did not become ready for publishing[^\r\n]+'
            'Signatures failed to update[^\r\n]*'
            'Response status code does not indicate success:\s*(?:401|403|429)[^\r\n]*'
            'Prebuilt packages are not allowed[^\r\n]*'
            'Static graph-based restore failed[^\r\n]*'
            'error MSB4166:[^\r\n]*'
            "The file '[^']+' is a tooling file reserved[^\r\n]*"
            'getaddrinfo ENOTFOUND [^\s,;]+'
            'BA\d{4}[^\r\n]*'
        )
        $groups = [ordered]@{}

        # A wrapper diagnostic (MSB3073 and friends) normally hides the real error, so it is dropped.
        # But when a job retains nothing else, dropping it leaves the finding with no evidence at all
        # and invites a vague summary. Track which jobs have a real diagnostic so those jobs can keep
        # the original wrapper quote with a separate explanation of the missing inner diagnostic.
        $jobsWithRealDiagnostic = [Collections.Generic.HashSet[string]]::new()
        foreach ($message in $messages) {
            $hasQuote = $false
            foreach ($pattern in $patterns) {
                if ([regex]::IsMatch($message.text, $pattern, [Text.RegularExpressions.RegexOptions]::IgnoreCase)) { $hasQuote = $true; break }
            }
            if (-not $hasQuote -and $message.text -match '(?i)MSB3073|Bash exited|Script failed|The command |0 Error|Build failed with exit') { continue }
            [void]$jobsWithRealDiagnostic.Add([string]$message.job)
        }

        foreach ($message in $messages) {
            $quotes = @(
                foreach ($pattern in $patterns) {
                    [regex]::Matches($message.text, $pattern, [Text.RegularExpressions.RegexOptions]::IgnoreCase) |
                        ForEach-Object Value
                }
            )
            $rank = 0
            $explanation = $null
            if (-not $quotes.Count) {
                if ($message.text -match '(?i)MSB3073|Bash exited|Script failed|The command |0 Error|Build failed with exit') {
                    if ($jobsWithRealDiagnostic.Contains([string]$message.job)) { continue }
                    $wrapper = ConvertTo-WrapperDiagnostic $message.text
                    if (-not $wrapper) { continue }
                    $quotes = @($message.text)
                    $explanation = $wrapper
                    $rank = 2
                }
                else {
                    $text = [regex]::Replace($message.text, '^\d{4}-\d{2}-\d{2}T\S+Z\s*', '').Trim()
                    $error = [regex]::Match($text, '(?i)(?:error [A-Z]+\d+:\s*(?:\([^)]*\)\s*)?|error\s*:\s*)(?<message>.+)')
                    if ($error.Success) { $text = $error.Groups['message'].Value }
                    if ($text -match '(?i)^(?:at |--- End|Stack trace:|Warning:|\+ echo|\* \[new branch\]|If there were failures)') { continue }
                    $quotes = @($text)
                    $rank = 1
                }
            }
            foreach ($quote in $quotes) {
                if ($quote.Length -lt 12) { continue }
                $quote = Short-Text $quote 350
                $groupText = if ($explanation) { $explanation } else { $quote }
                $key = [regex]::Replace($groupText.ToLowerInvariant(), '[0-9a-f]{8}(?:-[0-9a-f]{4}){3}-[0-9a-f]{12}', '<id>')
                $key = [regex]::Replace($key, '\s*\[[^\]]*\]\s*$', '')
                $key = [regex]::Replace($key, 'https?://[^\s''"]+', '<url>')
                $key = [regex]::Replace($key, '[a-z]:[\\/][^''"\r\n]+|/(?:__w|mnt|users|home)/[^\s''"]+', '<path>')
                $key = [regex]::Replace($key, "failed to retrieve information about '[^']+'", "failed to retrieve information about '<package>'")
                $key = [regex]::Replace($key, '\(in [\d.]+ \w+\)', '(duration)')
                $key = [regex]::Replace($key, 'global\\msbuild-server-running-[a-z0-9_-]+', 'global\msbuild-server-running-<id>')
                # Severity belongs to the OCCURRENCE (its task failed and its own job failed, or not), so it is part of
                # the group identity. Grouping by text alone let whichever occurrence came first decide for all of them:
                # a non-fatal copy listed first hid a fatal one, and the reverse made non-fatal copies look fatal.
                $jobResult = $null
                if ($message.jobId -and $jobResultsById.ContainsKey([string]$message.jobId)) { $jobResult = [string]$jobResultsById[[string]$message.jobId] }
                elseif ($message.job -and $jobResults.ContainsKey([string]$message.job)) { $jobResult = [string]$jobResults[[string]$message.job] }
                $severity = Get-DiagnosticSeverity $message.text ([string]$message.taskResult) $jobResult
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
        }
        $ordered = @($groups.Values | Sort-Object rank, @{e={-$_.occurrences}})
        # Terminating diagnostics first: a root cause must outrank non-fatal noise that
        # merely contains the word "error" (for example an analyzer that failed to load
        # while the compilation still succeeded).
        $ordered = @($ordered | Sort-Object @{ e = { if ($_.severity -eq 'terminating') { 0 } else { 1 } } },
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

    $failureIndex = if ($Evidence.failureTasksPath -and (Test-Path -LiteralPath $Evidence.failureTasksPath)) {
        Get-Content -LiteralPath $Evidence.failureTasksPath -Raw | ConvertFrom-Json -Depth 100
    } else { @{ builds = @() } }
    $logIndex = if ($Evidence.rootLogsPath -and (Test-Path -LiteralPath $Evidence.rootLogsPath)) {
        Get-Content -LiteralPath $Evidence.rootLogsPath -Raw | ConvertFrom-Json -Depth 100
    } else { @{ logs = @() } }
    $pipelines = @(
        foreach ($pipeline in $Evidence.pipelines) {
            $builds = if ($pipeline.inventoryPath) { @(Get-QBInventoryBuilds $pipeline.inventoryPath) } else { @() }
            $problems = @(
                foreach ($build in @($builds | Where-Object id -in @($pipeline.requiredProblemBuildIds))) {
                    $record = @($failureIndex.builds | Where-Object {
                        $_.buildId -eq $build.id -and
                        (Get-QBField $_ 'organization') -eq $(if ($pipeline.name -eq 'internal') { 'dnceng' } else { 'dnceng-public' })
                    } | Select-Object -First 1)
                    $tasks = @(
                        foreach ($task in @($record | ForEach-Object { @(Get-QBField $_ 'tasks' @()) })) {
                            if ($task.result -eq 'succeeded') { continue }
                            $cascadeIds = @($record | ForEach-Object { @(Get-QBField $_ 'cascadeSummaries' @()) } | ForEach-Object recordId)
                            if ($task.id -in $cascadeIds) { continue }
                            $messages = @($task.issues | ForEach-Object {
                                Short-Text (Get-QBField $_ 'message') 900
                            } | Select-Object -Unique -First 3)
                            if ($task.recordType -ne 'Task' -and $messages.Count -eq 0) { continue }
                            [ordered]@{ task = $task.task; job = $task.job; jobId = Get-QBField $task 'jobId'; recordType = $task.recordType; result = $task.result; issues = $messages }
                        }
                    )
                    $buildLogs = @($logIndex.logs | Where-Object buildId -eq $build.id)
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
                            if ((Get-QBField $log 'evidenceKind') -eq 'cascadeSummary') {
                                [ordered]@{
                                    task = $log.task; job = Get-QBField $log 'job'; kind = 'cascadeSummary'
                                    diagnostic = Short-Text (Get-QBField $log 'diagnosticExcerpt') 350
                                    rootRecordId = Get-QBField (Get-QBField $log 'cascade') 'rootRecordId'
                                }
                                continue
                            }
                            $excerpts = @(
                                foreach ($excerpt in @(Get-DiagnosticLines -Path $log.path -Fallback @(Get-QBField $log 'diagnosticExcerpts' @()))) {
                                    [ordered]@{ line = $excerpt.line; text = $excerpt.text }
                                }
                            )
                            $entry = [ordered]@{
                                task = $log.task; job = Get-QBField $log 'job'; jobId = Get-QBField $log 'jobId'; result = Get-QBField $log 'result'
                                available = $log.available; logId = $log.logId
                                promotionBuildId = Get-QBField $log 'promotionBuildId'
                                path = $log.path; excerpts = $excerpts
                                diagnostic = if ($excerpts.Count) { $null } else { Short-Text (Get-QBField $log 'diagnosticExcerpt') 1200 }
                            }
                            # The same failure as another build: no raw log was fetched for this one, and the model is told which build's log stands for it.
                            $sameFailure = Get-QBField $log 'sameFailureAs'
                            if ($null -ne $sameFailure) {
                                $entry['sameFailureAs'] = [ordered]@{ buildId = Get-QBField $sameFailure 'buildId'; logId = Get-QBField $sameFailure 'logId'; task = Get-QBField $sameFailure 'task' }
                                $entry['rawLog'] = "Not fetched: the same failure has its raw log in build $(Get-QBField $sameFailure 'buildId'); this build's timeline message is the diagnostic."
                            }
                            $jobLimit = Get-QBJobTimeLimitMinutes -JobId ([string](Get-QBField $log 'jobId')) -Task $tasks -Log $log
                            $repoBuild = Get-QBRepoBuildPacket -Path ([string](Get-QBField $log 'path')) -TimeLimitMinutes $jobLimit
                            if ($null -ne $repoBuild) { $entry['repoBuild'] = $repoBuild }
                            $entry
                        }
                    )
                    $detailPath = Save-ModelJson "model-$($pipeline.name)-$($build.id).json" @{
                        buildId = $build.id; sourceBranch = $build.sourceBranch; result = $build.result
                        tasks = $tasks; logs = $logs
                        reading = 'Read this compact evidence once. For a truncated diagnostic, view only the relevant line range of the named raw log. Do not load whole raw timeline/index files.'
                    }
                    $diagnostics = Get-DiagnosticSummary -Tasks $tasks -Logs $logs -AllRecords @($record | ForEach-Object { @(Get-QBField $_ 'tasks' @()) })
                    [ordered]@{
                        id = $build.id; branch = $build.sourceBranch; result = $build.result
                        classification = Get-QBField $build 'vmrClassification'
                        diagnostics = $diagnostics.diagnostics
                        terminatingDiagnosticCount = $diagnostics.terminatingCount
                        additionalDistinctDiagnostics = $diagnostics.additionalDistinctDiagnostics
                        detailPath = $detailPath
                    }
                }
            )
            [ordered]@{
                name = $pipeline.name; status = $pipeline.status; counts = $pipeline.counts
                error = Short-Text (Get-QBField $pipeline 'error') 1600
                loginCommand = Get-QBField $pipeline 'loginCommand'; requiredProblemBuildIds = $pipeline.requiredProblemBuildIds
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
        schemaVersion = 1; status = $Evidence.status; reportDate = $Evidence.reportDate
        windowStartUtc = $Evidence.windowStartUtc; observationCutoffUtc = $Evidence.observationCutoffUtc
        collectedAtUtc = $Evidence.collectedAtUtc; pipelines = $pipelines
        requiredErrors = @($Evidence.requiredErrors | Select-Object -First 8)
        reading = 'This diagnostic digest is the primary report input. Read it once. It includes actual diagnostic quotes for every problem build. Open a per-build detailPath only when diagnostics are empty or additionalDistinctDiagnostics is nonzero; otherwise do not reread the same evidence. Repeated task/leg diagnostics are grouped without treating SBOM cascades as new roots.'
    }
    $issueRows = @(
        foreach ($issue in @(Get-QBField $IssueEvidence 'issues' @())) {
            $name = $issue.repository.Replace('/', '-') + '-' + $issue.number
            $comments = @($issue.commentSummaries)
            $body = if ($issue.bodyPath -and (Test-Path -LiteralPath $issue.bodyPath)) {
                Short-Text (Get-Content -LiteralPath $issue.bodyPath -Raw) 8000
            } else { '' }
            $detail = Save-ModelJson "model-issue-$name.json" @{
                repository = $issue.repository; number = $issue.number; title = $issue.title; state = $issue.state
                url = $issue.url; assignees = $issue.assignees; body = $body
                bodyTruncated = [bool]($body.Length -ge 8000); bodyPath = $issue.bodyPath
                commentsComplete = $issue.commentsComplete; commentSummaries = $comments
                commentsPath = $issue.commentsPath; matchedBuildIds = $issue.matchedBuildIds
                buildReferences = @($issue.buildReferences | Where-Object requested)
                pullRequestReferences = $issue.pullRequestReferences
                reading = 'This is a compact candidate view, not a complete comment history. If relevant comment text is needed, use targeted text search/line ranges of commentsPath, not a full large-file view.'
            }
            [ordered]@{
                repository = $issue.repository; number = $issue.number; title = $issue.title
                state = $issue.state; url = $issue.url; matchedBuildIds = $issue.matchedBuildIds
                detailPath = $detail
            }
        }
    )
    $issuePath = Save-ModelJson 'model-issue-evidence.json' @{
        schemaVersion = 1; status = $IssueEvidence.status; observedAtUtc = Get-QBField $IssueEvidence 'observedAtUtc'
        issues = $issueRows; assigneeCandidates = $IssueEvidence.assigneeCandidates
        fixPullRequests = @(ConvertTo-QBModelPullRequest @(Get-QBField $IssueEvidence 'fixPullRequests' @()))
        errors = @($IssueEvidence.errors | Select-Object -First 5)
        reading = 'Read this compact issue list once; inspect only relevant candidate model-issue files. Optional historical lookups remain unchecked, not proven absent. Do not read full raw issue-evidence.json or every issue/comment history.'
    }
    return [pscustomobject]@{ pipelinePath = $pipelinePath; issuePath = $issuePath }
}

# ---------------------------------------------------------------------------------------------
# report/Email.ps1
# ---------------------------------------------------------------------------------------------
# Renders a QB report (Markdown) into HTML suitable for Outlook's HTMLBody.
#
# Outlook strips <style> blocks and most external CSS, so table styling must be inline on every
# <table>/<th>/<td> tag. PowerShell's own ConvertFrom-Markdown does the Markdown -> HTML
# conversion (headers, tables, bold, links); this only adds the inline styling Outlook needs.

function ConvertTo-VmrQBEmailBody {
    param(
        [Parameter(Mandatory)][string] $Markdown,
        [string] $Title = 'VMR QB daily monitoring'
    )
    $body = (ConvertFrom-Markdown -InputObject $Markdown).Html
    $body = $body `
        -replace '<table>', '<table style="border-collapse:collapse;width:100%;margin:10px 0;font-size:13px;">' `
        -replace '<th>', '<th style="border:1px solid #c8c8c8;padding:6px 9px;background:#f3f3f3;text-align:left;vertical-align:top;">' `
        -replace '<td>', '<td style="border:1px solid #c8c8c8;padding:6px 9px;text-align:left;vertical-align:top;">'

    return @"
<html><head><meta charset="utf-8"></head>
<body style="font-family:'Segoe UI',Arial,sans-serif;font-size:14px;color:#1b1b1b;line-height:1.45;">
$body
<hr style="border:none;border-top:1px solid #e1e1e1;margin:12px 0;">
<p style="margin:6px 0;color:#777;font-size:12px;">Generated by the vmr-qb-agent package. Monitoring is read-only &mdash; no PRs were approved, merged, or closed; no Teams messages, subscription changes, or retries were issued.</p>
</body></html>
"@
}

function Send-VmrQBReport {
    param(
        [Parameter(Mandatory)][string] $ReportPath,
        [Parameter(Mandatory)][string] $Subject,
        [string] $ConfigPath = (Get-VmrQBConfigPath)
    )
    if (-not (Test-Path -LiteralPath $ReportPath -PathType Leaf)) { throw "Report not found: $ReportPath" }
    $config = Get-VmrQBConfig -Path $ConfigPath
    $markdown = Get-Content -LiteralPath $ReportPath -Raw
    $html = ConvertTo-VmrQBEmailBody -Markdown $markdown

    $com = [Collections.Generic.List[object]]::new()
    try {
        $outlook = New-Object -ComObject Outlook.Application; $com.Add($outlook)
        $session = $outlook.Session; $com.Add($session)
        if ($session.Offline) { throw 'Outlook is offline; the report was not sent.' }
        $mail = $outlook.CreateItem(0); $com.Add($mail)
        $mail.To = $config.recipient
        $mail.Subject = $Subject
        $mail.HTMLBody = $html
        $mail.Attachments.Add((Resolve-Path $ReportPath).Path) | Out-Null
        $mail.Send()
        return [pscustomobject]@{ status = 'sent'; recipient = $config.recipient; subject = $Subject; sentAt = [DateTimeOffset]::Now.ToString('o') }
    }
    finally {
        foreach ($o in $com) { if ($null -ne $o) { [void][Runtime.InteropServices.Marshal]::ReleaseComObject($o) } }
        [GC]::Collect(); [GC]::WaitForPendingFinalizers()
    }
}
