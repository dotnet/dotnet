Set-StrictMode -Version Latest

# history/history.ps1 - Searching history, recording what an owner said, replaying reports, folding aliases, and the handoff bundle.
# Dot-sourced by the entry scripts and the tests; it defines functions only and runs nothing.
. (Join-Path $PSScriptRoot 'store.ps1')
. (Join-Path $PSScriptRoot '..\common\core.ps1')
. (Join-Path $PSScriptRoot '..\common\routing-edit.ps1')

# ---------------------------------------------------------------------------------------------
# history/Record.ps1
# ---------------------------------------------------------------------------------------------
# Recording what an owner told you.
#
# "An owner says it is a known outage and they are checking" has to reach three places
# or it is lost by tomorrow: the history store (so the finding is no longer an action item), the
# "links the QB reported" column (so the row explains itself), and the model's context (so it does
# not re-derive a different answer). Doing that by hand across several files is how corrections get
# dropped. This does all of it in one validated step.
#
# Everything is validated BEFORE anything is written. Each file is then replaced atomically.


function Write-VmrQBJsonFile {
    <#
    .SYNOPSIS
    Atomically replaces a JSON file (temp file in the same directory, then move).
    #>
    param([Parameter(Mandatory)][string] $Path, [Parameter(Mandatory)][object] $Value, [int] $Depth = 20)
    $directory = Split-Path -Parent $Path
    if ($directory -and -not (Test-Path -LiteralPath $directory)) { [void][IO.Directory]::CreateDirectory($directory) }
    $temporary = "$Path.$([guid]::NewGuid().ToString('N')).tmp"
    try {
        [IO.File]::WriteAllText($temporary, ((ConvertTo-Json -InputObject $Value -Depth $Depth) -replace "`r`n", "`n") + "`n", [Text.UTF8Encoding]::new($false))
        [IO.File]::Move($temporary, $Path, $true)
    }
    finally { if ([IO.File]::Exists($temporary)) { [IO.File]::Delete($temporary) } }
}

function Get-VmrQBStatusAction {
    param([string] $Status)
    switch ($Status) {
        'owned'     { 'Monitor only. Someone owns it: do not ping, do not draft a new issue, do not list it in Top actions.' }
        'routed'    { 'Monitor only. It was already reported to the owner: do not ping again unless it recurs after a fix.' }
        'resolved'  { 'Monitor only. Treat a recurrence as a regression and escalate it.' }
        'noise'     { 'Monitor only. Real but non-blocking: do not raise it.' }
        'wontfix'   { 'Monitor only. Accepted; do not raise it.' }
        default     { 'Handle as a normal finding.' }
    }
}

function Read-VmrQBOperatorDocument {
    <#
    .SYNOPSIS
    Reads one QB-local JSON document (reported links, triage context) and checks its shape, or returns a new empty one.
    .DESCRIPTION
    Throws, with the file named and "Nothing was written", for a file that is not valid JSON, not an object, or whose list
    is not a list, so a caller can stop before it changes anything else.
    #>
    param(
        [Parameter(Mandatory)][string] $Path,
        [Parameter(Mandatory)][string] $Label,
        [Parameter(Mandatory)][string] $ListProperty,
        [Parameter(Mandatory)][scriptblock] $New
    )
    if (-not (Test-Path -LiteralPath $Path)) { return (& $New) }
    if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) { throw "The $Label path $Path is not a file. Nothing was written." }
    try { $document = Get-Content -LiteralPath $Path -Raw | ConvertFrom-Json -Depth 20 }
    catch { throw "The $Label file $Path is not valid JSON ($($_.Exception.Message)). Nothing was written; repair or move that file and run the command again." }
    if ($document -isnot [System.Management.Automation.PSCustomObject]) { throw "The $Label file $Path must hold a JSON object. Nothing was written." }
    $property = $document.PSObject.Properties[$ListProperty]
    if ($null -eq $property) { Add-Member -InputObject $document -NotePropertyName $ListProperty -NotePropertyValue @() }
    elseif ($null -ne $property.Value -and $property.Value -isnot [array]) { throw "The $Label file $Path has a '$ListProperty' that is not a list. Nothing was written." }
    return $document
}

function New-VmrQBStagedJsonFile {
    <#
    .SYNOPSIS
    Writes a JSON document to a temp file beside its target and returns the temp path; the caller moves it into place.
    #>
    param([Parameter(Mandatory)][string] $Path, [Parameter(Mandatory)][object] $Value, [Parameter(Mandatory)][string] $Label, [int] $Depth = 20)
    $temporary = "$Path.$([guid]::NewGuid().ToString('N')).tmp"
    try {
        $directory = Split-Path -Parent $Path
        if ($directory) { [void][IO.Directory]::CreateDirectory($directory) }
        [IO.File]::WriteAllText($temporary, ((ConvertTo-Json -InputObject $Value -Depth $Depth) -replace "`r`n", "`n") + "`n", [Text.UTF8Encoding]::new($false))
    }
    catch {
        if ([IO.File]::Exists($temporary)) { try { [IO.File]::Delete($temporary) } catch { Write-Verbose "Could not remove $temporary" } }
        throw "Cannot write the $Label file $Path ($($_.Exception.Message)). Nothing was written."
    }
    return $temporary
}

function Test-VmrQBDirectoryWritable {
    <#
    .SYNOPSIS
    Throws, naming the folder, unless a file can be created in it (the folder is created when missing).
    #>
    param([Parameter(Mandatory)][string] $Directory, [Parameter(Mandatory)][string] $Label)
    $probe = $null
    try {
        [void][IO.Directory]::CreateDirectory($Directory)
        $probe = Join-Path $Directory ('.write-test-' + [guid]::NewGuid().ToString('N') + '.tmp')
        [IO.File]::WriteAllText($probe, 'x')
    }
    catch { throw "Cannot write to the $Label folder $Directory ($($_.Exception.Message)). Nothing was written." }
    finally { if ($probe -and [IO.File]::Exists($probe)) { try { [IO.File]::Delete($probe) } catch { Write-Verbose "Could not remove $probe" } } }
}
function Set-VmrQBOperatorUpdate {
    <#
    .SYNOPSIS
    Records an owner's statement about a finding in the history store, the QB links, and (optionally) the model context.
    .PARAMETER Finding
    The finding's name or slug. It must already exist in history; a typo must never silently create a new issue.
    Use -Create only for a genuinely new finding.
    .PARAMETER Note
    What was said, in the speaker's words as closely as you can. This is what the next QB will read.
    .PARAMETER SaidBy
    Who said it, e.g. "Alex (infra team)". Stored with the status change.
    .PARAMETER Builds
    Build ids the statement applies to. Requires -Match so the note does not leak onto unrelated failures in the same builds.
    .PARAMETER Match
    A regex tested against the report's "Issue category" so the note only appears on the right row.
    .PARAMETER AddContext
    Also add an entry to the QB-local triage context the model reads. Requires -AppliesTo.
    #>
    [CmdletBinding(SupportsShouldProcess)]
    param(
        [Parameter(Mandatory)][string] $Finding,
        [Parameter(Mandatory)][ValidateSet('open', 'routed', 'owned', 'resolved', 'regressed', 'noise', 'wontfix')][string] $Status,
        [Parameter(Mandatory)][string] $Note,
        [string] $SaidBy,
        [string] $Owner,
        [string] $Tracker,
        [int[]] $Builds = @(),
        [string] $Match,
        [string] $LinkText,
        [switch] $AddContext,
        [string] $AppliesTo,
        [switch] $Create,
        [switch] $AllowUnknownOwner,
        [string] $On = (Get-Date -Format 'yyyy-MM-dd'),
        [string] $HistoryPath,
        [string] $LinksPath,
        [string] $LocalKnowledgeDir
    )
    $paths = Get-VmrQBPaths
    if (-not $HistoryPath) { $HistoryPath = $paths.HistoryPath }
    if (-not $LinksPath) { $LinksPath = $paths.ReportedLinksPath }
    if (-not $LocalKnowledgeDir) { $LocalKnowledgeDir = $paths.LocalKnowledgeDir }

    # ---- validate everything first --------------------------------------------------------------
    if ($On -notmatch '^\d{4}-\d{2}-\d{2}$') { throw "-On must be yyyy-MM-dd, got '$On'." }
    if ([string]::IsNullOrWhiteSpace($Note) -or $Note.Trim().Length -lt 12) { throw '-Note must say what was actually said (at least a short sentence).' }
    if ($Tracker -and $Tracker -notmatch '^https://') { throw "-Tracker must be an https URL, got '$Tracker'." }
    if ($Builds.Count -and -not $Match) { throw '-Builds requires -Match, a regex for the issue category. Without it the note would appear on every failure in those builds.' }
    if ($Match) { try { [void][regex]::new($Match) } catch { throw "-Match is not a valid regex: $($_.Exception.Message)" } }
    if ($AddContext -and -not $AppliesTo) { throw '-AddContext requires -AppliesTo: when does this statement apply (and when does it not)?' }

    $canonicalOwner = $null
    if ($Owner) {
        $canonicalOwner = Resolve-VmrQBOwnerTeam -Handle $Owner
        if ($null -eq $canonicalOwner) {
            if (-not $AllowUnknownOwner) {
                $suggest = @(Find-VmrQBUnknownHandles -Text $Owner | Where-Object suggestion | ForEach-Object suggestion)
                $hint = if ($suggest.Count) { " Did you mean $($suggest[0])?" } else { ' See knowledge/routing.json for the real teams.' }
                throw "Owner '$Owner' is not a team that exists in the routing registry.$hint Use -AllowUnknownOwner only if you have verified it."
            }
            $canonicalOwner = $Owner
        }
        elseif ($canonicalOwner -ne $Owner.Trim() -and $Owner.Trim().StartsWith('@') -and $Owner.Contains('/') -and $canonicalOwner -ine $Owner.Trim()) {
            # A known mistake (for example @dotnet/runtime, which GitHub does not have) is corrected, but never silently.
            Write-Warning "Owner '$($Owner.Trim())' is not a routable team; recording '$canonicalOwner' instead."
        }
    }

    $existing = $null
    if (Test-Path -LiteralPath $HistoryPath) { $existing = Get-VmrIssueHistory -Path $HistoryPath -Name $Finding }
    if ($null -eq $existing -and -not $Create) {
        $near = @()
        if (Get-Command Search-VmrIssueHistory -ErrorAction SilentlyContinue) { $near = @(Search-VmrIssueHistory -Path $HistoryPath -Text $Finding -Limit 5 | ForEach-Object { "'$($_.name)'" }) }
        $hint = if ($near.Count) { " Close matches: $($near -join ', ')." } else { '' }
        throw "No finding named '$Finding' in history.$hint Use the exact name, or pass -Create if this is genuinely new."
    }
    $slug = if ($existing) { [string]$existing.issue.slug } else { ConvertTo-VmrIssueSlug -Name $Finding }
    $displayName = if ($existing) { [string]$existing.issue.name } else { $Finding }
    $source = if ($SaidBy) { "operator: $SaidBy" } else { 'operator' }
    $statusNote = if ($SaidBy) { "$($SaidBy): $Note" } else { $Note }

    # `applied` is false for a -WhatIf plan and flips to true only after every write below has happened, so a
    # caller can say "Would record" or "Recorded" truthfully.
    $plan = [ordered]@{
        finding = $displayName; slug = $slug; status = $Status; owner = $canonicalOwner; tracker = $Tracker; on = $On
        store = $true; links = [bool]($Builds.Count -or $Tracker); context = [bool]$AddContext
        historyPath = $HistoryPath; linksPath = $LinksPath; applied = $false
    }

    # Every document this update touches is read and checked BEFORE any of them is written, so a malformed or unreadable
    # file stops the command with nothing changed. -WhatIf runs the same checks.
    $contextPath = Join-Path $LocalKnowledgeDir 'triage-context.local.json'
    $touchesLinks = [bool]($Builds.Count -or $Tracker)
    $readDocuments = {
        $links = $null; $context = $null
        if ($touchesLinks) {
            $links = Read-VmrQBOperatorDocument -Path $LinksPath -Label 'reported links' -ListProperty 'reports' -New {
                [pscustomobject]@{ '$schema' = 'operator-maintained report links and notes'; readme = @('Written by ./history/Invoke-Record.ps1. A note has no link; an entry with a url links to where it was reported.', "'match' is a regex tested against the Issue category so an entry only appears on the failure it was recorded for."); reports = @() } }
        }
        if ($AddContext) {
            $context = Read-VmrQBOperatorDocument -Path $contextPath -Label 'triage context' -ListProperty 'entries' -New { [pscustomobject]@{ schemaVersion = 1; entries = @() } }
        }
        [pscustomobject]@{ Links = $links; Context = $context }
    }
    if (-not $PSCmdlet.ShouldProcess("$displayName ($slug)", "Record '$Status' $(if ($canonicalOwner) { "owned by $canonicalOwner" })")) {
        [void](& $readDocuments)
        return [pscustomobject]$plan
    }

    # Fail fast on a malformed document before taking the lock or creating anything; the documents are read again under
    # the lock below, because another update may have changed them in between.
    [void](& $readDocuments)

    # One update at a time: two recordings must not read the same links file and then overwrite each other's entry.
    try { [void][IO.Directory]::CreateDirectory($LocalKnowledgeDir) }
    catch { throw "Cannot create the local knowledge folder $LocalKnowledgeDir ($($_.Exception.Message)). Nothing was written." }
    $lock = Enter-VmrIssueStoreLock -FullPath (Join-Path $LocalKnowledgeDir 'operator-update')
    $staged = [Collections.Generic.List[object]]::new()
    $keepStaged = $false
    try {
        $documents = & $readDocuments
        Test-VmrQBDirectoryWritable -Directory (Split-Path -Parent $HistoryPath) -Label 'issue history'

        # ---- stage: build the new links and context documents in temp files beside their targets ------
        if ($touchesLinks) {
            $reports = [Collections.Generic.List[object]]::new()
            foreach ($entry in @($documents.Links.reports)) { $reports.Add($entry) }
            if ($Builds.Count) {
                $text = if ($LinkText) { $LinkText } else { $statusNote }
                if ($text.Length -gt 400) { $text = $text.Substring(0, 397) + '...' }
                $reports.Add([pscustomobject][ordered]@{ note = $text; match = $Match; buildIds = @($Builds | Sort-Object -Unique) })
            }
            if ($Tracker -and $Tracker -match 'github\.com/(?<o>[^/]+)/(?<r>[^/]+)/(?:issues|pull)/(?<n>\d+)') {
                $label = "$($Matches.o)/$($Matches.r)#$($Matches.n)"
                $entry = [ordered]@{ label = $label; url = $Tracker }
                if ($Match) { $entry.match = $Match }
                $entry.buildIds = @($Builds | Sort-Object -Unique)
                if ($Builds.Count) { $reports.Add([pscustomobject]$entry) }
            }
            $documents.Links.reports = $reports.ToArray()
            $staged.Add([pscustomobject]@{ Label = 'the reported links'; Path = $LinksPath; Temp = (New-VmrQBStagedJsonFile -Path $LinksPath -Value $documents.Links -Label 'reported links') })
        }
        if ($AddContext) {
            $id = "$slug-$On"
            $entries = [Collections.Generic.List[object]]::new()
            foreach ($entry in @($documents.Context.entries)) { if ($entry.id -ne $id) { $entries.Add($entry) } }
            $first = ($Note -split '(?<=[.!?])\s+')[0]
            $entries.Add([pscustomobject][ordered]@{
                id = $id
                title = $displayName
                operatorStatus = "$($Status.ToUpperInvariant()) as of $On. $first"
                operatorNote = $statusNote
                relatedHistoryIds = @("build:$slug")
                applicability = $AppliesTo
                action = Get-VmrQBStatusAction -Status $Status
                links = @(@($Tracker) | Where-Object { $_ })
                verification = "Operator statement on $On$(if ($SaidBy) { " from $SaidBy" }). Not independently verified."
            })
            $documents.Context.entries = $entries.ToArray()
            $staged.Add([pscustomobject]@{ Label = 'the triage context'; Path = $contextPath; Temp = (New-VmrQBStagedJsonFile -Path $contextPath -Value $documents.Context -Label 'triage context') })
            $plan.contextPath = $contextPath
        }

        # ---- commit: history first (its own lock and atomic replace), then each staged file ------------
        # If the history write fails, nothing else has changed. Moving a staged file onto its target is a single rename
        # into a folder that was just written to, so a failure there is rare, and it is reported as a partial update.
        if (-not (Test-Path -LiteralPath $HistoryPath)) { Initialize-VmrIssueStore -Path $HistoryPath | Out-Null }
        $arguments = @{ Path = $HistoryPath; Name = $displayName; Status = $Status; Note = $statusNote; On = $On; Source = $source }
        if ($canonicalOwner) { $arguments.OwnerTeam = $canonicalOwner }
        if ($Tracker) { $arguments.TrackerUrl = $Tracker }
        if ($AllowUnknownOwner) { $arguments.AllowUnknownOwner = $true }
        $slug = Set-VmrIssueStatus @arguments
        $done = [Collections.Generic.List[string]]::new()
        $done.Add('the issue history')
        foreach ($item in @($staged)) {
            try {
                [IO.File]::Move($item.Temp, $item.Path, $true)
                $done.Add($item.Label)
                $item.Temp = $null
            }
            catch {
                $keepStaged = $true
                $cause = $_.Exception.Message
                $left = @($staged | Where-Object { $_.Temp } | ForEach-Object { "$($_.Label) (new content saved at $($_.Temp); move it over $($_.Path))" })
                throw ("Only part of this update was recorded. Done: $($done -join ', '). Not done: $($left -join '; '). Cause: $cause " +
                    'Move the saved file(s) into place once the problem is fixed. Do not run the command again: it would record the history entry twice.')
            }
        }
    }
    finally {
        $lock.Dispose()
        # A run that failed before changing anything leaves no staged file behind. After a partial commit the unmoved
        # files are kept, because the error message names them.
        if (-not $keepStaged) {
            foreach ($item in @($staged)) {
                if ($item.Temp -and [IO.File]::Exists($item.Temp)) { try { [IO.File]::Delete($item.Temp) } catch { Write-Verbose "Could not remove $($item.Temp)" } }
            }
        }
    }
    $plan.applied = $true
    return [pscustomobject]$plan
}

function Merge-VmrQBTriageContext {
    <#
    .SYNOPSIS
    Writes shipped + QB-local triage context into one file the collector and the prompts can read.
    .DESCRIPTION
    Local entries replace shipped ones with the same id. Either side may be absent: the skill ships no
    verified findings, so a fresh install has only what this QB records. Returns the
    shipped path unchanged when there is no local file and the shipped file exists; otherwise the output path.
    #>
    [CmdletBinding()]
    param([string] $ShippedPath, [string] $LocalPath, [Parameter(Mandatory)][string] $OutputPath)
    $hasLocal = $LocalPath -and (Test-Path -LiteralPath $LocalPath)
    $hasShipped = $ShippedPath -and (Test-Path -LiteralPath $ShippedPath)
    if (-not $hasLocal -and $hasShipped) { return $ShippedPath }
    $base = if ($hasShipped) { Get-Content -LiteralPath $ShippedPath -Raw | ConvertFrom-Json -Depth 20 } else { [pscustomobject][ordered]@{ schemaVersion = 1; entries = @() } }
    $local = if ($hasLocal) { Get-Content -LiteralPath $LocalPath -Raw | ConvertFrom-Json -Depth 20 } else { [pscustomobject]@{ entries = @() } }
    $localIds = @(@($local.entries) | ForEach-Object { $_.id })
    $merged = [Collections.Generic.List[object]]::new()
    foreach ($entry in @($base.entries)) { if ($entry.id -notin $localIds) { $merged.Add($entry) } }
    foreach ($entry in @($local.entries)) { $merged.Add($entry) }
    $base.entries = $merged.ToArray()
    Write-VmrQBJsonFile -Path $OutputPath -Value $base
    return $OutputPath
}

# ---------------------------------------------------------------------------------------------
# history/Tools.ps1
# ---------------------------------------------------------------------------------------------
# Questions the history store should answer without anyone grepping markdown.
#
#   Search-VmrIssueHistory      "what do we know about build 1234567?" / "everything mentioning socket"
#   Find-VmrIssueDuplicates     "which findings are probably the same failure under different names?"
#
# The second one exists because finding names are written by a model each day. In the data this was
# built from, one macOS failure had been recorded under six different names, each with one
# occurrence, so its recurrence was invisible. Names are weak evidence; shared builds and shared
# diagnostic wording are strong. This only SUGGESTS aliases. A person confirms them.


function Get-VmrQBIssueField {
    param([AllowNull()][object] $Object, [string] $Name, [AllowNull()][object] $Default = $null)
    if ($null -eq $Object) { return $Default }
    $property = $Object.PSObject.Properties[$Name]
    if ($null -ne $property -and $null -ne $property.Value) { return $property.Value }
    return $Default
}

function Search-VmrIssueHistory {
    <#
    .SYNOPSIS
    Finds issues by text, by a build id they appeared in, by owner, or by status.
    .DESCRIPTION
    Text must match (every word) somewhere in the issue's name, aliases, notes, resolution or status-history notes.
    -BuildId is the reverse lookup: which findings did this build contribute to, and what was decided?
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][string] $Path,
        [string] $Text,
        [string] $BuildId,
        [string] $Owner,
        [ValidateSet('', 'open', 'routed', 'owned', 'resolved', 'regressed', 'noise', 'wontfix')][string] $Status = '',
        [int] $SinceDays = 0,
        [int] $Limit = 25
    )
    if (-not ($Text -or $BuildId -or $Owner -or $Status)) { throw 'Give at least one of -Text, -BuildId, -Owner, -Status.' }
    $data = Read-VmrIssueStore -Path $Path
    $occurrencesBySlug = @{}
    foreach ($row in @($data.occurrences)) {
        if (-not $occurrencesBySlug.ContainsKey($row.slug)) { $occurrencesBySlug[$row.slug] = [Collections.Generic.List[object]]::new() }
        $occurrencesBySlug[$row.slug].Add($row)
    }
    $aliasesBySlug = @{}
    foreach ($row in @($data.aliases)) {
        if (-not $aliasesBySlug.ContainsKey($row.slug)) { $aliasesBySlug[$row.slug] = [Collections.Generic.List[string]]::new() }
        $aliasesBySlug[$row.slug].Add([string]$row.alias)
    }
    $notesBySlug = @{}
    foreach ($row in @($data.statusHistory)) {
        if (-not $notesBySlug.ContainsKey($row.slug)) { $notesBySlug[$row.slug] = [Collections.Generic.List[string]]::new() }
        $notesBySlug[$row.slug].Add([string](Get-VmrQBIssueField $row 'note' ''))
    }
    $words = @(([string]$Text).ToLowerInvariant() -split '[^a-z0-9]+' | Where-Object { $_.Length -ge 2 })
    $ownerCanonical = if ($Owner) { $c = Resolve-VmrQBOwnerTeam -Handle $Owner; if ($c) { $c } else { $Owner } } else { $null }
    $cutoff = if ($SinceDays -gt 0) { (Get-Date).AddDays(-$SinceDays).ToString('yyyy-MM-dd') } else { $null }

    $hits = foreach ($issue in @($data.issues)) {
        $slug = [string]$issue.slug
        $why = [Collections.Generic.List[string]]::new()
        if ($Status -and [string]$issue.status -ne $Status) { continue }
        if ($cutoff -and [string](Get-VmrQBIssueField $issue 'last_seen' '') -lt $cutoff) { continue }
        if ($ownerCanonical) {
            if ([string](Get-VmrQBIssueField $issue 'owner_team' '') -ine $ownerCanonical) { continue }
            $why.Add("owner $ownerCanonical")
        }
        if ($BuildId) {
            $seen = @($occurrencesBySlug[$slug] | Where-Object { @(([string](Get-VmrQBIssueField $_ 'build_ids' '')) -split ',') -contains $BuildId })
            if (-not $seen) { continue }
            $why.Add("build $BuildId seen on $((@($seen | ForEach-Object report_date) | Sort-Object) -join ', ')")
        }
        if ($words.Count) {
            $haystack = (@($slug, [string]$issue.name, [string](Get-VmrQBIssueField $issue 'notes' ''), [string](Get-VmrQBIssueField $issue 'resolution' '')) +
                @($aliasesBySlug[$slug]) + @($notesBySlug[$slug])) -join ' '
            $haystack = $haystack.ToLowerInvariant()
            $missing = @($words | Where-Object { -not $haystack.Contains($_) })
            if ($missing.Count) { continue }
            $inName = @($words | Where-Object { ($slug + ' ' + ([string]$issue.name).ToLowerInvariant()).Contains($_) }).Count
            $why.Add($(if ($inName -eq $words.Count) { 'name' } else { 'notes/aliases' }))
        }
        [pscustomobject]@{
            slug = $slug; name = [string]$issue.name; status = [string]$issue.status
            owner_team = Get-VmrQBIssueField $issue 'owner_team'; tracker_url = Get-VmrQBIssueField $issue 'tracker_url'
            first_seen = Get-VmrQBIssueField $issue 'first_seen'; last_seen = Get-VmrQBIssueField $issue 'last_seen'
            occurrences = [int](Get-VmrQBIssueField $issue 'occurrences' 0)
            matchedOn = ($why -join '; ')
        }
    }
    return @($hits | Sort-Object @{ e = { $_.matchedOn -match 'name' }; Descending = $true }, @{ e = 'last_seen'; Descending = $true } | Select-Object -First $Limit)
}

$script:VmrQBTokenStopWords = [Collections.Generic.HashSet[string]]::new([string[]]@(
    'the', 'and', 'for', 'from', 'with', 'into', 'not', 'after', 'during', 'when', 'that', 'this', 'are', 'was', 'bh',
    'failure', 'failures', 'failed', 'fail', 'error', 'errors', 'issue', 'issues', 'build', 'builds', 'task', 'tasks'))

# Words that flip what a name claims. Two names that differ on these describe different states of a thing,
# not two spellings of one finding.
$script:VmrQBNegationWords = [Collections.Generic.HashSet[string]]::new([string[]]@('no', 'not', 'without', 'never', 'none'))
$script:VmrQBPositiveStateWords = [Collections.Generic.HashSet[string]]::new([string[]]@('green', 'resolved', 'fixed', 'recovered', 'healthy', 'passing'))
$script:VmrQBNegativeStateWords = [Collections.Generic.HashSet[string]]::new([string[]]@('red', 'broken', 'blocked', 'failing'))

function Get-VmrQBIssueTokens {
    <#
    .SYNOPSIS
    Splits a finding slug into descriptive words, version-ish tokens, and polarity markers.
    .DESCRIPTION
    Versions and branches (10, 11, 2xx, 1xx, main, v2, rc2, preview3) are kept apart from the words. They
    are not used to measure similarity, but they veto a match: "SDK release/10.0.1xx flow blocked" and
    "SDK release/11.0.1xx flow blocked" are different branches, so they are different findings.
    Polarity vetoes work the same way: "No open PRs" versus "Open PRs", or "flow remains red" versus
    "green flow", describe opposite states.
    #>
    param([Parameter(Mandatory)][string] $Slug)
    $words = [Collections.Generic.HashSet[string]]::new()
    $versions = [Collections.Generic.HashSet[string]]::new()
    $negated = $false; $positive = $false; $negative = $false
    foreach ($part in ($Slug.ToLowerInvariant() -split '[^a-z0-9]+')) {
        if (-not $part) { continue }
        if ($part -match '^\d+$' -or $part -match '^\d+x+$' -or $part -eq 'main' -or $part -match '^(v|rc|preview|beta|net)\d+$') { [void]$versions.Add($part); continue }
        if ($script:VmrQBNegationWords.Contains($part)) { $negated = $true; continue }
        if ($script:VmrQBPositiveStateWords.Contains($part)) { $positive = $true }
        if ($script:VmrQBNegativeStateWords.Contains($part)) { $negative = $true }
        if ($part.Length -lt 3 -or $script:VmrQBTokenStopWords.Contains($part)) { continue }
        # Light singularization so "exhaustion"/"exhaustions" and "limit"/"limits" agree.
        if ($part.Length -gt 4 -and $part.EndsWith('s') -and -not $part.EndsWith('ss')) { $part = $part.Substring(0, $part.Length - 1) }
        [void]$words.Add($part)
    }
    return [pscustomobject]@{ words = $words; versions = $versions; negated = $negated; positive = $positive; negative = $negative }
}

function Test-VmrQBVersionsCompatible {
    # Compatible when either side names no version, or one side's versions contain the other's.
    param($A, $B)
    if ($A.Count -eq 0 -or $B.Count -eq 0) { return $true }
    return $A.IsSubsetOf($B) -or $B.IsSubsetOf($A)
}

function Test-VmrQBPolarityCompatible {
    # Incompatible when the names disagree on negation, or one says green/resolved and the other red/blocked.
    param($A, $B)
    if ($A.negated -ne $B.negated) { return $false }
    if (($A.positive -and $B.negative -and -not $A.negative -and -not $B.positive) -or
        ($B.positive -and $A.negative -and -not $B.negative -and -not $A.positive)) { return $false }
    return $true
}

function Find-VmrIssueDuplicates {
    <#
    .SYNOPSIS
    Suggests issues that are probably one failure recorded under different names.
    .DESCRIPTION
    Scores pairs on shared name tokens (at least two), plus a bonus for builds they both appeared in and
    for the same owner. Pairs that are already aliased are skipped. Nothing is changed; each result
    carries the exact Register-VmrIssueAlias command to run if you agree.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][string] $Path,
        [ValidateRange(0.1, 1.0)][double] $Threshold = 0.55,
        [int] $SinceDays = 0,
        [int] $Limit = 40
    )
    $data = Read-VmrIssueStore -Path $Path
    $cutoff = if ($SinceDays -gt 0) { (Get-Date).AddDays(-$SinceDays).ToString('yyyy-MM-dd') } else { $null }
    $issues = @($data.issues | Where-Object { -not $cutoff -or [string](Get-VmrQBIssueField $_ 'last_seen' '') -ge $cutoff })
    $builds = @{}
    foreach ($row in @($data.occurrences)) {
        if (-not $builds.ContainsKey($row.slug)) { $builds[$row.slug] = [Collections.Generic.HashSet[string]]::new() }
        foreach ($id in (([string](Get-VmrQBIssueField $row 'build_ids' '')) -split ',')) { if ($id) { [void]$builds[$row.slug].Add($id.Trim()) } }
    }
    $tokens = @{}; $index = @{}; $versionsBySlug = @{}; $parsedBySlug = @{}
    foreach ($issue in $issues) {
        $parsed = Get-VmrQBIssueTokens -Slug ([string]$issue.slug)
        $set = $parsed.words
        $tokens[[string]$issue.slug] = $set
        $versionsBySlug[[string]$issue.slug] = $parsed.versions
        $parsedBySlug[[string]$issue.slug] = $parsed
        foreach ($token in $set) {
            if (-not $index.ContainsKey($token)) { $index[$token] = [Collections.Generic.List[string]]::new() }
            $index[$token].Add([string]$issue.slug)
        }
    }
    $bySlug = @{}; foreach ($issue in $issues) { $bySlug[[string]$issue.slug] = $issue }

    $results = [Collections.Generic.List[object]]::new()
    $seenPairs = [Collections.Generic.HashSet[string]]::new()
    foreach ($slug in $tokens.Keys) {
        # Only slugs that share at least two tokens can score; the inverted index finds them without comparing every pair.
        $shared = @{}
        foreach ($token in $tokens[$slug]) { foreach ($other in $index[$token]) { if ($other -ne $slug) { $shared[$other] = 1 + [int]$(if ($shared.ContainsKey($other)) { $shared[$other] } else { 0 }) } } }
        foreach ($other in $shared.Keys) {
            if ($shared[$other] -lt 2) { continue }
            $pairKey = if ([string]::CompareOrdinal($slug, $other) -lt 0) { "$slug|$other" } else { "$other|$slug" }
            if (-not $seenPairs.Add($pairKey)) { continue }
            if (-not (Test-VmrQBVersionsCompatible -A $versionsBySlug[$slug] -B $versionsBySlug[$other])) { continue }
            if (-not (Test-VmrQBPolarityCompatible -A $parsedBySlug[$slug] -B $parsedBySlug[$other])) { continue }
            $a = $tokens[$slug]; $b = $tokens[$other]
            $intersection = [double]$shared[$other]
            $union = [double]($a.Count + $b.Count - $shared[$other])
            $jaccard = $intersection / $union
            $containment = $intersection / [Math]::Min($a.Count, $b.Count)
            $nameScore = [Math]::Max($jaccard, 0.85 * $containment)
            $commonBuilds = 0
            if ($builds.ContainsKey($slug) -and $builds.ContainsKey($other)) {
                foreach ($id in $builds[$slug]) { if ($builds[$other].Contains($id)) { $commonBuilds++ } }
            }
            $bonus = 0.0
            if ($nameScore -ge 0.3 -and $commonBuilds -gt 0) { $bonus += [Math]::Min(0.25, 0.1 * $commonBuilds) }
            $ownerA = [string](Get-VmrQBIssueField $bySlug[$slug] 'owner_team' ''); $ownerB = [string](Get-VmrQBIssueField $bySlug[$other] 'owner_team' '')
            if ($ownerA -and $ownerA -eq $ownerB) { $bonus += 0.05 }
            $score = [Math]::Min(1.0, $nameScore + $bonus)
            if ($score -lt $Threshold) { continue }

            $first = $bySlug[$slug]; $second = $bySlug[$other]
            # Keep the one with more history as the canonical name, so the alias folds the thinner record into it.
            $keep, $fold = if ([int]$first.occurrences -gt [int]$second.occurrences -or
                ([int]$first.occurrences -eq [int]$second.occurrences -and [string]$first.first_seen -le [string]$second.first_seen)) { $first, $second } else { $second, $first }
            $results.Add([pscustomobject]@{
                score = [Math]::Round($score, 2); nameScore = [Math]::Round($nameScore, 2); sharedBuilds = $commonBuilds
                keep = [string]$keep.name; fold = [string]$fold.name
                keepOccurrences = [int]$keep.occurrences; foldOccurrences = [int]$fold.occurrences
                suggestedCli = "./history/Invoke-History.ps1 alias -Alias '$(([string]$fold.name).Replace("'", "''"))' -Canonical '$(([string]$keep.name).Replace("'", "''"))'"
                suggestedCommand = "Register-VmrIssueAlias -Path '$Path' -Alias '$(([string]$fold.name).Replace("'", "''"))' -CanonicalName '$(([string]$keep.name).Replace("'", "''"))'"
            })
        }
    }
    return @($results | Sort-Object @{ e = 'score'; Descending = $true }, @{ e = 'sharedBuilds'; Descending = $true } | Select-Object -First $Limit)
}

function Repair-VmrIssueOwners {
    <#
    .SYNOPSIS
    Rewrites owner handles that are not real teams (for example @dotnet/runtime, a repo name) to the
    real team named in routing.json.
    .DESCRIPTION
    History written before the registry existed, or imported from a colleague's older export, can carry
    handles that notify nobody. The fix keeps each owner's provenance, so an operator's decision stays an
    operator's decision. Handles the registry does not know are left alone and returned as 'unknown' for
    a person to look at. Nothing is written when nothing needs fixing. Returns one row per issue touched.
    #>
    [CmdletBinding()]
    param([Parameter(Mandatory)][string] $Path, [switch] $Preview)
    $rows = [Collections.Generic.List[object]]::new()
    $action = {
        param($m)
        foreach ($slug in @($m.Issues.Keys)) {
            $issue = $m.Issues[$slug]
            $current = $issue['owner_team']
            if ([string]::IsNullOrWhiteSpace([string]$current)) { continue }
            $canonical = Resolve-VmrIssueOwnerHandle -Handle ([string]$current)
            if ($null -eq $canonical) { $rows.Add([pscustomobject]@{ slug = $slug; from = [string]$current; to = $null; result = 'unknown' }); continue }
            if ($canonical -ceq [string]$current) { continue }
            $rows.Add([pscustomobject]@{ slug = $slug; from = [string]$current; to = $canonical; result = 'fixed' })
            if (-not $Preview) { [void](Set-VmrIssueStoreField -Model $m -Record $issue -Field 'owner_team' -Value $canonical) }
        }
    }
    [void](Invoke-VmrIssueStoreTransaction -StorePath $Path -Action $action)
    return @($rows)
}

function Get-VmrQBAliasFiles {
    # Alias groups are this QB's own session data (finding names are written by a model and drift), so none ship with the skill.
    # Aliases registered with `./history/Invoke-History.ps1 alias -Alias ... -Canonical ...` live in the history store itself and travel with it.
    # This optional local file only re-applies a list in bulk. With -AliasFile only that file is used.
    param([string] $AliasFile)
    if ($AliasFile) { return @($AliasFile | Where-Object { Test-Path -LiteralPath $_ -PathType Leaf }) }
    $paths = Get-VmrQBPaths
    return @($paths.AliasesPath | Where-Object { Test-Path -LiteralPath $_ -PathType Leaf })
}

function Set-VmrIssueAliases {
    <#
    .SYNOPSIS
    Folds the alternate names in the local alias file (if any) into one canonical issue each.
    .DESCRIPTION
    Findings get renamed between reports, which splits their history. Each group names a canonical
    issue and the other wordings seen for it. Folding is idempotent and keeps the earliest first_seen,
    every occurrence, and the status trail.
    #>
    [CmdletBinding()]
    param([string] $Database, [string] $AliasFile)
    $paths = Get-VmrQBPaths
    if (-not $Database) { $Database = $paths.HistoryPath }
    $files = @(Get-VmrQBAliasFiles -AliasFile $AliasFile)
    if ($AliasFile -and -not $files.Count) { throw "Alias file not found: $AliasFile" }
    Initialize-VmrIssueStore -Path $Database | Out-Null
    $groupCount = 0; $applied = 0
    foreach ($file in $files) {
        $groups = @((Get-Content -LiteralPath $file -Raw | ConvertFrom-Json).groups)
        $groupCount += $groups.Count
        foreach ($group in $groups) {
            foreach ($alias in @($group.aliases)) {
                Register-VmrIssueAlias -Path $Database -Alias $alias -CanonicalName $group.canonical | Out-Null
                $applied++
            }
        }
    }
    $recurring = @(Get-VmrIssueHistory -Path $Database -Limit 5000 | Where-Object { $_.occurrences -gt 1 })
    return [pscustomobject]@{ aliases = $applied; groups = $groupCount; recurring = $recurring.Count }
}

function Repair-VmrIssueHistory {
    <#
    .SYNOPSIS
    Brings an issue history up to the current rules: alias groups applied, owner handles real, duplicates counted.
    .DESCRIPTION
    Safe to re-run; it changes nothing that is already correct.
      1. Folds every group in the local alias file (if any) into its canonical issue.
      2. Rewrites owner handles that are not real teams (for example @dotnet/runtime) to the team in
         routing.json, keeping who recorded them. Handles it cannot place are listed, never guessed.
      3. Counts likely duplicates (score 0.95 or higher) that a person still has to fold or dismiss.
    Setup and import run this after bringing history in.
    #>
    [CmdletBinding()]
    param([string] $HistoryPath, [string] $AliasFile, [switch] $Preview)
    $paths = Get-VmrQBPaths
    if (-not $HistoryPath) { $HistoryPath = $paths.HistoryPath }
    if (-not (Test-Path -LiteralPath $HistoryPath)) { throw "No history at $HistoryPath. Run ./common/Invoke-Setup.ps1 first." }
    $files = @(Get-VmrQBAliasFiles -AliasFile $AliasFile)
    $groupCount = 0
    foreach ($file in $files) { $groupCount += @((Get-Content -LiteralPath $file -Raw | ConvertFrom-Json).groups).Count }
    if ($files.Count -and -not $Preview) { $null = Set-VmrIssueAliases -Database $HistoryPath -AliasFile $AliasFile }
    $owners = @(Repair-VmrIssueOwners -Path $HistoryPath -Preview:$Preview)
    $duplicates = @(Find-VmrIssueDuplicates -Path $HistoryPath -Threshold 0.95 -Limit 1000)
    return [pscustomobject]@{
        aliasGroups = $groupCount
        ownersRewritten = @($owners | Where-Object { $_.result -eq 'fixed' }).Count
        unknownOwners = @($owners | Where-Object { $_.result -eq 'unknown' }).Count
        likelyDuplicates = $duplicates.Count
        owners = $owners
    }
}
function Update-VmrIssueHistory {
    <#
    .SYNOPSIS
    Replays delivered overall-summary reports into the issue-history store. Idempotent.
    .DESCRIPTION
    Reads only the rows of the six-column summary tables (main and monitor-only), so a different table
    in the same report can never be mistaken for a finding. Each row becomes one occurrence for that
    day. Source is 'daily-run': an owner or tracker an operator recorded is never overwritten by a
    report (see ./history/Invoke-Record.ps1).
    #>
    [CmdletBinding()]
    param(
        # Replay only this report date (yyyy-MM-dd). Default: every report in the directory.
        [ValidatePattern('^\d{4}-\d{2}-\d{2}$')][string] $Date,
        [string] $Database,
        [string] $ReportDirectory
    )
    $paths = Get-VmrQBPaths
    if (-not $Database) { $Database = $paths.HistoryPath }
    if (-not $ReportDirectory) { $ReportDirectory = $paths.ReportsDir }
    Initialize-VmrIssueStore -Path $Database | Out-Null

    $header = '| Issue category | Builds / PRs (links) | New Issue (Y/N) | Existing fix PR (status) | Action as VMR QB (who / where to ping) | Links that QB reported |'
    $buildPattern = 'buildId=(\d+)'
    $prPattern = '/(?:pull|issues)/(\d+)'
    $trackerPattern = 'https://github\.com/[^/\s)]+/[^/\s)]+/(?:issues|pull)/\d+'
    $teamPattern = '@dotnet/[A-Za-z0-9][A-Za-z0-9._-]*'

    $files = @(Get-ChildItem -LiteralPath $ReportDirectory -Filter 'overall-summary-*.md' -File -ErrorAction SilentlyContinue | Sort-Object Name)
    if ($Date) { $files = @($files | Where-Object { $_.BaseName -eq "overall-summary-$Date" }) }

    $imported = 0
    foreach ($file in $files) {
        $reportDate = if ($file.BaseName -match '(\d{4}-\d{2}-\d{2})') { $Matches[1] } else { continue }
        $inTable = $false
        foreach ($line in (Get-Content -LiteralPath $file.FullName)) {
            $trimmed = $line.Trim()
            if ($trimmed -eq $header) { $inTable = $true; continue }
            if (-not $inTable) { continue }
            if ($trimmed -notmatch '^\|.*\|$') { $inTable = $false; continue }
            if ($trimmed -match '^\|(?:\s*:?-+:?\s*\|){6}$') { continue }

            # Split on pipes that are not escaped (\|) so a pipe inside a message cannot shift the columns.
            $cells = @($trimmed.Substring(1, $trimmed.Length - 2) -split '(?<!\\)\|' | ForEach-Object { $_.Trim() })
            if ($cells.Count -ne 6) { continue }

            $category = $cells[0].Replace('**', '')
            $bh = $null
            if ($category -match '^BH #(\d+|-)\s*-\s*') { $bh = "BH #$($Matches[1])" }
            $name = [regex]::Replace($category, '^BH #(?:\d+|-)\s*-\s*', '').Trim()
            if ($name.Length -lt 8) { continue }

            $evidence = $cells[1]
            $ids = @([regex]::Matches($evidence, $buildPattern) | ForEach-Object { $_.Groups[1].Value } | Select-Object -Unique)
            if (-not $ids) { $ids = @([regex]::Matches($evidence, $prPattern) | ForEach-Object { 'pr-' + $_.Groups[1].Value } | Select-Object -Unique) }
            $kind = if ($evidence -match '/pull/|/issues/' -and $evidence -notmatch $buildPattern) { 'codeflow' } else { 'build' }

            $owner = $null
            $ownerMatch = [regex]::Match($cells[4], $teamPattern)
            if ($ownerMatch.Success) { $owner = $ownerMatch.Value.TrimEnd('.') }

            $tracker = $null
            foreach ($source in @($cells[5], $cells[3], $cells[4])) {
                $trackerMatch = [regex]::Match([string]$source, $trackerPattern)
                if ($trackerMatch.Success) { $tracker = $trackerMatch.Value; break }
            }

            Add-VmrIssueOccurrence -Path $Database -Name $name -ReportDate $reportDate -Kind $kind -BhNumber $bh `
                -NewToday $cells[2] -BuildIds $ids -OwnerTeam $owner -TrackerUrl $tracker -ReportPath $file.Name -Source 'daily-run' | Out-Null
            $imported++
        }
    }
    $all = @(Get-VmrIssueHistory -Path $Database -Limit 5000)
    return [pscustomobject]@{
        imported = $imported; reports = $files.Count; issues = $all.Count
        recurring = @($all | Where-Object { $_.occurrences -gt 1 } | Sort-Object occurrences -Descending)
    }
}

# --- Handoff: everything session-specific, in ONE file, passed to the next QB out of band -----------------
#
# What a QB learns (history, findings an owner confirmed, alias groups, links reported, contacts chosen by
# hand) is session data. It never ships in the skill and is never committed: it lives in the state
# directory, and this is the one way it leaves it. The file is plain JSON, reviewable before it is sent.

function Read-VmrQBJsonOrNull {
    param([string] $Path)
    if (-not $Path -or -not (Test-Path -LiteralPath $Path -PathType Leaf)) { return $null }
    return (Get-Content -LiteralPath $Path -Raw | ConvertFrom-Json -Depth 50)
}

function Export-VmrQBHandoff {
    <#
    .SYNOPSIS
    Writes one handoff file holding all of this QB's session data.
    #>
    [CmdletBinding()]
    param([string] $OutputPath, [string] $HistoryPath, [datetime] $Now = (Get-Date))
    $paths = Get-VmrQBPaths
    if (-not $HistoryPath) { $HistoryPath = $paths.HistoryPath }
    if (-not $OutputPath) { $OutputPath = Join-Path $paths.HandoffDir ('vmr-qb-handoff-{0}.json' -f $Now.ToString('yyyy-MM-dd')) }

    $history = $null
    if (Test-Path -LiteralPath $HistoryPath) {
        $temporary = Join-Path ([IO.Path]::GetTempPath()) ('vmrqb-handoff-' + [guid]::NewGuid().ToString('N') + '.json')
        try { $null = Export-VmrIssueHistory -Path $HistoryPath -OutputPath $temporary; $history = Read-VmrQBJsonOrNull $temporary }
        finally { Remove-Item -LiteralPath $temporary, "$temporary.lock" -Force -ErrorAction SilentlyContinue }
    }
    $context = Read-VmrQBJsonOrNull $paths.LocalContextPath
    $aliases = Read-VmrQBJsonOrNull $paths.AliasesPath
    $links = Read-VmrQBJsonOrNull $paths.ReportedLinksPath
    $handWritten = [ordered]@{}
    $roster = Read-VmrQBContacts -Path (Get-VmrQBContactsPath)
    foreach ($team in $roster.Keys) { if (-not (Test-VmrQBContactGenerated $roster[$team])) { $handWritten[$team] = $roster[$team] } }

    $routing = Get-VmrQBRoutingExport

    $bundle = [ordered]@{
        schema        = 'vmr-qb-handoff'
        schemaVersion = 1
        exportedAt    = $Now.ToString('s')
        readme        = @(
            'One QB''s session data for the next QB. Review it before sending: it holds triage notes, links to internal builds and any contact you chose.',
            'Import with: ./common/Invoke-Setup.ps1 -ImportHandoff <this file>   (or ./history/Invoke-History.ps1 import -InputPath <this file>; add -Preview to see what would change).',
            'Importing is additive and idempotent; what the importing QB already recorded always wins, routing included.'
        )
        history       = $history
        triageContext = $context
        aliases       = $aliases
        reportedLinks = $links
        contacts      = $handWritten
        routing       = $routing
    }
    Write-VmrQBJsonFile -Path $OutputPath -Value $bundle -Depth 50
    $counts = if ($history) { [pscustomobject]@{ issues = @($history.issues).Count; occurrences = @($history.occurrences).Count } } else { [pscustomobject]@{ issues = 0; occurrences = 0 } }
    return [pscustomobject]@{
        path = $OutputPath; issues = $counts.issues; occurrences = $counts.occurrences
        contextEntries = $(if ($context) { @($context.entries).Count } else { 0 })
        aliasGroups = $(if ($aliases) { @($aliases.groups).Count } else { 0 })
        reportedLinks = $(if ($links) { @($links.reports).Count } else { 0 })
        contacts = $handWritten.Count
        routingChanges = $(if ($routing) { @($routing.changes).Count } else { 0 })
    }
}

function Import-VmrQBHandoff {
    <#
    .SYNOPSIS
    Merges a handoff file (or a plain history export) into this QB's state. Additive, idempotent; what you
    already recorded always wins.
    .PARAMETER Preview
    Report what would change; write nothing.
    #>
    [CmdletBinding()]
    param([Parameter(Mandatory)][string] $InputPath, [string] $HistoryPath, [switch] $Preview)
    if (-not (Test-Path -LiteralPath $InputPath -PathType Leaf)) { throw "Handoff file not found: $InputPath" }
    $paths = Get-VmrQBPaths
    if (-not $HistoryPath) { $HistoryPath = $paths.HistoryPath }
    $document = Get-Content -LiteralPath $InputPath -Raw | ConvertFrom-Json -Depth 50
    $isBundle = [string](Get-VmrQBField $document 'schema') -eq 'vmr-qb-handoff'
    if ($isBundle -and [int](Get-VmrQBField $document 'schemaVersion' 0) -gt 1) { throw "Handoff schemaVersion $($document.schemaVersion) is newer than this skill understands (1). Update the skill." }

    # Your own local documents are read and checked before anything is imported, so a malformed one stops the import with
    # nothing changed instead of after the history has been merged.
    if ($isBundle) {
        foreach ($local in @(@($paths.LocalContextPath, 'triage context'), @($paths.AliasesPath, 'issue aliases'), @($paths.ReportedLinksPath, 'reported links'))) {
            try { [void](Read-VmrQBJsonOrNull $local[0]) }
            catch { throw "Your $($local[1]) file $($local[0]) is not valid JSON ($($_.Exception.Message)). Nothing was imported; repair or move that file and import again." }
        }
    }

    # One update at a time, like ./history/Invoke-Record.ps1: an import and a recording must not overwrite each other's entry.
    $lock = $null
    if (-not $Preview) {
        try { [void][IO.Directory]::CreateDirectory($paths.LocalKnowledgeDir) }
        catch { throw "Cannot create the local knowledge folder $($paths.LocalKnowledgeDir) ($($_.Exception.Message)). Nothing was imported." }
        $lock = Enter-VmrIssueStoreLock -FullPath (Join-Path $paths.LocalKnowledgeDir 'operator-update')
    }
    $temporaries = [Collections.Generic.List[string]]::new()
    try {
        $historyFile = $InputPath
        if ($isBundle) {
            $historyFile = $null
            if ($null -ne (Get-VmrQBField $document 'history')) {
                $historyFile = Join-Path ([IO.Path]::GetTempPath()) ('vmrqb-handoff-history-' + [guid]::NewGuid().ToString('N') + '.json')
                $temporaries.Add($historyFile)
                [IO.File]::WriteAllText($historyFile, (ConvertTo-Json -InputObject $document.history -Depth 50), [Text.UTF8Encoding]::new($false))
            }
        }

        # History: merge into the real store, or into a throwaway copy for -Preview.
        $historyResult = $null
        if ($historyFile) {
            $target = $HistoryPath
            if ($Preview) {
                $target = Join-Path ([IO.Path]::GetTempPath()) ('vmrqb-import-preview-' + [guid]::NewGuid().ToString('N') + '.json')
                $temporaries.Add($target); $temporaries.Add("$target.lock")
                if (Test-Path -LiteralPath $HistoryPath) { Copy-Item -LiteralPath $HistoryPath -Destination $target } else { Initialize-VmrIssueStore -Path $target | Out-Null }
            }
            elseif (-not (Test-Path -LiteralPath $HistoryPath)) { Initialize-VmrIssueStore -Path $HistoryPath | Out-Null }
            $before = if (Test-Path -LiteralPath $target) { @(Get-VmrIssueHistory -Path $target -Limit 100000).Count } else { 0 }
            $historyResult = Import-VmrIssueHistory -Path $target -InputPath $historyFile
            $historyResult | Add-Member -NotePropertyName issuesBefore -NotePropertyValue $before -Force
            $historyResult | Add-Member -NotePropertyName issuesAfter -NotePropertyValue @(Get-VmrIssueHistory -Path $target -Limit 100000).Count -Force
        }

        # Everything else: merge into local files by a stable key; the importing QB's own entries win.
        $summary = [ordered]@{ history = $historyResult; contextAdded = 0; aliasGroupsAdded = 0; reportedLinksAdded = 0; contactsAdded = 0; routing = $null; preview = [bool]$Preview }
        if ($isBundle) {
            $incoming = Get-VmrQBField $document 'triageContext'
            if ($null -ne $incoming) {
                $local = Read-VmrQBJsonOrNull $paths.LocalContextPath
                if ($null -eq $local) { $local = [pscustomobject][ordered]@{ schemaVersion = 1; entries = @() } }
                $known = @(@($local.entries) | ForEach-Object { $_.id })
                $fresh = @(@($incoming.entries) | Where-Object { $_.id -notin $known })
                $summary.contextAdded = $fresh.Count
                if ($fresh.Count -and -not $Preview) { $local.entries = @(@($local.entries) + $fresh); Write-VmrQBJsonFile -Path $paths.LocalContextPath -Value $local }
            }
            $incoming = Get-VmrQBField $document 'aliases'
            if ($null -ne $incoming) {
                $local = Read-VmrQBJsonOrNull $paths.AliasesPath
                if ($null -eq $local) { $local = [pscustomobject][ordered]@{ readme = @(Get-VmrQBField $incoming 'readme' @()); groups = @() } }
                $merged = [Collections.Generic.List[object]]::new()
                foreach ($group in @($local.groups)) { $merged.Add($group) }
                foreach ($group in @($incoming.groups)) {
                    $existing = $merged | Where-Object { $_.canonical -eq $group.canonical } | Select-Object -First 1
                    if ($null -eq $existing) { $merged.Add($group); $summary.aliasGroupsAdded++; continue }
                    $newAliases = @(@($group.aliases) | Where-Object { $_ -notin @($existing.aliases) })
                    if ($newAliases.Count) { $existing.aliases = @(@($existing.aliases) + $newAliases); $summary.aliasGroupsAdded++ }
                }
                if ($summary.aliasGroupsAdded -and -not $Preview) { $local.groups = $merged.ToArray(); Write-VmrQBJsonFile -Path $paths.AliasesPath -Value $local }
            }
            $incoming = Get-VmrQBField $document 'reportedLinks'
            if ($null -ne $incoming) {
                $local = Read-VmrQBJsonOrNull $paths.ReportedLinksPath
                if ($null -eq $local) { $local = [pscustomobject][ordered]@{ '$schema' = 'operator-maintained report links and notes'; readme = @(Get-VmrQBField $incoming 'readme' @()); reports = @() } }
                $seen = @(@($local.reports) | ForEach-Object { ConvertTo-Json -InputObject $_ -Depth 10 -Compress })
                $fresh = @(@($incoming.reports) | Where-Object { (ConvertTo-Json -InputObject $_ -Depth 10 -Compress) -notin $seen })
                $summary.reportedLinksAdded = $fresh.Count
                if ($fresh.Count -and -not $Preview) { $local.reports = @(@($local.reports) + $fresh); Write-VmrQBJsonFile -Path $paths.ReportedLinksPath -Value $local }
            }
            $incoming = Get-VmrQBField $document 'contacts'
            if ($null -ne $incoming) {
                $rosterPath = Get-VmrQBContactsPath -ForWrite
                $roster = Read-VmrQBContacts -Path (Get-VmrQBContactsPath)
                $added = 0
                foreach ($property in $incoming.PSObject.Properties) {
                    if (-not $roster.Contains($property.Name)) { $roster[$property.Name] = $property.Value; $added++ }
                }
                $summary.contactsAdded = $added
                if ($added -and -not $Preview) {
                    Write-VmrQBJsonFile -Path $rosterPath -Value ([ordered]@{ '$schema' = 'operator-maintained team contacts'; schemaVersion = 2; contacts = $roster })
                }
            }
            # Routing: the sender's own additions and changes, applied only where that is safe (see Import-VmrQBRoutingChanges). An old
            # bundle has no routing section, and a newer section version is skipped rather than guessed at.
            $incoming = Get-VmrQBField $document 'routing'
            if ($null -ne $incoming) {
                $version = [int](Get-VmrQBField $incoming 'schemaVersion' 0)
                if ($version -ne 1) { $summary.routing = [pscustomobject]@{ skipped = "routing section version $version is not understood by this skill (1)"; results = @() } }
                else {
                    $routingResults = @(Import-VmrQBRoutingChanges -Changes @(Get-VmrQBField $incoming 'changes' @()) -Preview:$Preview)
                    $summary.routing = [pscustomobject]@{ skipped = $null; results = $routingResults }
                }
            }
        }

        # A colleague's older export can carry owner handles that are not teams, and alias groups that
        # have not been folded yet; settle both on the way in.
        if ($historyFile) {
            $repairTarget = if ($Preview) { $target } else { $HistoryPath }
            $repair = Repair-VmrIssueHistory -HistoryPath $repairTarget -Preview:$Preview
            $summary.repair = $repair
        }
        return [pscustomobject]$summary
    }
    finally {
        foreach ($file in $temporaries) { Remove-Item -LiteralPath $file -Force -ErrorAction SilentlyContinue }
        if ($null -ne $lock) { $lock.Dispose() }
    }
}

# Console summary of an export/import result, shared by Invoke-Setup.ps1 and Invoke-History.ps1.
function Format-QBHandoffSummary {
    param($Result)
    $history = $Result.history
    $parts = @()
    if ($null -ne $history) { $parts += "history $($history.issuesBefore) -> $($history.issuesAfter) issues ($($history.occurrences) occurrences, $($history.statusEntries) status entries)" }
    $parts += "$($Result.contextAdded) context entries, $($Result.aliasGroupsAdded) alias groups, $($Result.reportedLinksAdded) links, $($Result.contactsAdded) contacts added"
    $routing = Get-VmrQBField $Result 'routing'
    if ($null -ne $routing) {
        if ($routing.skipped) { $parts += "routing skipped ($($routing.skipped))" }
        else {
            $count = { param($name) @($routing.results | Where-Object Result -eq $name).Count }
            $parts += "routing: $(& $count 'added') added, $(& $count 'replaced') replaced, $(& $count 'conflict') kept as conflicts, $(& $count 'invalid') refused"
        }
    }
    return ($(if ($Result.preview) { 'PREVIEW (nothing changed): ' } else { '' }) + ($parts -join '; '))
}
