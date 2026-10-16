Set-StrictMode -Version Latest

# history/store.ps1 - The issue-history store: one JSON file, locked and replaced atomically. No dependencies.
# Dot-sourced by history.ps1 and by the store tests; it defines functions only and runs nothing.

# Persistent history of VMR QB findings: what was seen, when, how it was routed, and how it ended.
# A daily report only shows today. Investigations keep needing "have we seen this before, what did
# we decide, did it ever get fixed" - which previously meant grepping markdown across many days and
# still missing renamed findings. This store answers that directly and survives renames by keying on
# a stable slug plus recorded aliases.
#
# STORAGE. One UTF-8 JSON file (conventionally issue-history.json, but the extension is never
# relied on). It replaced a SQLite database because dotnet/dotnet's binary detector forbids shipping
# the SQLite DLLs, and a text file also diffs cleanly in a handoff. Output is deterministic (sorted
# collections, 2-space indent, LF line endings), so two stores holding the same data are
# byte-identical apart from updatedAtUtc.
#
# CONCURRENCY. Every write takes an exclusive cross-process lock (<path>.lock, opened with
# FileShare.None), re-reads the store inside the lock, mutates, writes a temp file beside the store,
# flushes it, and atomically replaces the store. The .lock file is intentionally never deleted:
# deleting a lock file races with processes already waiting on it. Readers never lock; they see
# either the old or the new complete file. Calls that change nothing (a daily replay of reports that
# were already recorded) do not rewrite the file at all.
#
# PROVENANCE. owner_team and tracker_url remember who wrote them ('operator' or 'daily-run'). A
# daily run may fill a blank value or replace another daily-run value, but never an operator value:
# a human decision must not be silently overwritten by an LLM-written report.

$script:VmrIssueStoreSchema = 2
$script:VmrIssueStoreLockTimeoutSeconds = 15
$script:VmrIssueStoreCache = @{}
$script:VmrIssueStoreSwept = [Collections.Generic.HashSet[string]]::new()
$script:VmrIssueStoreJsonHasDateKind = [bool](Get-Command -Name 'ConvertFrom-Json' -CommandType Cmdlet).Parameters.ContainsKey('DateKind')
$script:VmrIssueStoreIssueFields = @('slug', 'name', 'kind', 'status', 'owner_team', 'owner_source', 'tracker_url', 'tracker_source', 'first_seen', 'last_seen', 'occurrences', 'resolution', 'resolved_on', 'notes')
$script:VmrIssueStoreOccurrenceFields = @('slug', 'report_date', 'name_used', 'bh_number', 'new_today', 'build_ids', 'build_count', 'report_path')
$script:VmrIssueStoreAliasFields = @('alias', 'slug', 'first_seen')
$script:VmrIssueStoreStatusFields = @('slug', 'changed_on', 'status', 'note', 'source')

# ---------------------------------------------------------------------------------------------
# File access: paths, locking, atomic writes
# ---------------------------------------------------------------------------------------------

function Resolve-VmrIssueStorePath {
    param([string] $Path)
    # PowerShell's location and the process working directory differ; honour the former.
    $unresolved = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($Path)
    return [IO.Path]::GetFullPath($unresolved)
}

function Get-VmrIssueStoreCacheKey {
    param([string] $FullPath)
    if ($IsWindows) { return $FullPath.ToUpperInvariant() }
    return $FullPath
}

function Get-VmrIssueStoreInnerException {
    param([System.Management.Automation.ErrorRecord] $ErrorRecord)
    $inner = $ErrorRecord.Exception
    if ($inner -is [System.Management.Automation.MethodException] -and $null -ne $inner.InnerException) { $inner = $inner.InnerException }
    return $inner
}

function Test-VmrIssueStoreTransientError {
    # Sharing violations and "access denied" while another process (a reader, an indexer, antivirus)
    # briefly holds the file. Missing paths are real errors and are never retried.
    param([System.Management.Automation.ErrorRecord] $ErrorRecord)
    $inner = Get-VmrIssueStoreInnerException -ErrorRecord $ErrorRecord
    if ($inner -is [UnauthorizedAccessException]) { return $true }
    if ($inner -is [IO.FileNotFoundException] -or $inner -is [IO.DirectoryNotFoundException] -or $inner -is [IO.PathTooLongException]) { return $false }
    return ($inner -is [IO.IOException])
}

function Read-VmrIssueStoreBytes {
    param([string] $FullPath)
    $deadline = [DateTime]::UtcNow.AddSeconds(5)
    while ($true) {
        $stream = $null
        try {
            # FileShare.Delete lets File.Replace (the fallback writer path) proceed while a read is in flight;
            # reads are short, so the preferred rename simply retries until this handle closes.
            $stream = [IO.FileStream]::new($FullPath, [IO.FileMode]::Open, [IO.FileAccess]::Read, ([IO.FileShare]::ReadWrite -bor [IO.FileShare]::Delete))
            $buffer = [IO.MemoryStream]::new()
            $stream.CopyTo($buffer)
            return , $buffer.ToArray()
        }
        catch {
            if (-not (Test-VmrIssueStoreTransientError -ErrorRecord $_) -or [DateTime]::UtcNow -ge $deadline) { throw }
            [Threading.Thread]::Sleep(20)
        }
        finally { if ($null -ne $stream) { $stream.Dispose() } }
    }
}

function Enter-VmrIssueStoreLock {
    param([string] $FullPath)
    $lockPath = $FullPath + '.lock'
    $timeout = [double]$script:VmrIssueStoreLockTimeoutSeconds
    $deadline = [DateTime]::UtcNow.AddSeconds($timeout)
    $pause = 5
    while ($true) {
        try {
            return [IO.FileStream]::new($lockPath, [IO.FileMode]::OpenOrCreate, [IO.FileAccess]::ReadWrite, [IO.FileShare]::None)
        }
        catch {
            # Exactly IOException is "somebody else holds it"; subclasses are missing directories and the like.
            $inner = Get-VmrIssueStoreInnerException -ErrorRecord $_
            if ($inner.GetType() -ne [IO.IOException]) { throw }
            if ([DateTime]::UtcNow -ge $deadline) {
                throw "Timed out after $timeout seconds waiting for another process to finish writing the issue history store $FullPath (lock file $lockPath is held). Retry once the other VMR QB script has finished."
            }
            [Threading.Thread]::Sleep($pause)
            $pause = [Math]::Min($pause * 2, 50)
        }
    }
}

function Remove-VmrIssueStoreStaleTempFiles {
    # Only ever called while holding the store lock, so any temp file left here belongs to a writer
    # that crashed before it could replace the store.
    param([string] $FullPath)
    try {
        $directory = [IO.Path]::GetDirectoryName($FullPath)
        $leaf = [IO.Path]::GetFileName($FullPath)
        $pattern = '^' + [regex]::Escape($leaf) + '\.[0-9a-f]{32}\.tmp$'
        foreach ($candidate in [IO.Directory]::EnumerateFiles($directory, $leaf + '.*.tmp')) {
            if ([IO.Path]::GetFileName($candidate) -match $pattern) {
                try { [IO.File]::Delete($candidate) } catch { Write-Verbose "Could not remove stale temp file $candidate" }
            }
        }
    }
    catch { Write-Verbose "Stale temp file sweep skipped: $($_.Exception.Message)" }
}

function Write-VmrIssueStoreFile {
    # Atomic: the target is either the previous complete file or the new complete file, never a mix.
    param([string] $FullPath, [byte[]] $Bytes, [switch] $SweepStale)
    if ($SweepStale -and $script:VmrIssueStoreSwept.Add($FullPath)) { Remove-VmrIssueStoreStaleTempFiles -FullPath $FullPath }
    $temp = '{0}.{1}.tmp' -f $FullPath, [guid]::NewGuid().ToString('N')
    $stream = $null
    try {
        $stream = [IO.FileStream]::new($temp, [IO.FileMode]::CreateNew, [IO.FileAccess]::Write, [IO.FileShare]::None)
        $stream.Write($Bytes, 0, $Bytes.Length)
        $stream.Flush($true)
        $stream.Dispose()
        $stream = $null
        $written = [IO.FileInfo]::new($temp)
        $stamp = @{ Length = $written.Length; Ticks = $written.LastWriteTimeUtc.Ticks }

        $moveUntil = [DateTime]::UtcNow.AddSeconds(2)
        $deadline = [DateTime]::UtcNow.AddSeconds(10)
        while ($true) {
            try {
                # Move-with-overwrite is a single rename, so a reader never finds the store missing or half
                # written. It fails while a reader has the old file open, which is brief, so it is retried first.
                # Only if that keeps failing does File.Replace take over: it tolerates open readers, but there is
                # an instant during it when the target name does not exist.
                if ([DateTime]::UtcNow -lt $moveUntil -or -not [IO.File]::Exists($FullPath)) { [IO.File]::Move($temp, $FullPath, $true) }
                else { [IO.File]::Replace($temp, $FullPath, [NullString]::Value) }
                break
            }
            catch {
                if (-not (Test-VmrIssueStoreTransientError -ErrorRecord $_) -or [DateTime]::UtcNow -ge $deadline) { throw }
                [Threading.Thread]::Sleep(10)
            }
        }
        return $stamp
    }
    finally {
        if ($null -ne $stream) { $stream.Dispose() }
        if ([IO.File]::Exists($temp)) { try { [IO.File]::Delete($temp) } catch { Write-Verbose "Could not remove temp file $temp" } }
    }
}

# ---------------------------------------------------------------------------------------------
# Serialisation
# ---------------------------------------------------------------------------------------------

function Get-VmrIssueStoreSortedValues {
    # The values of a slug- or alias-keyed dictionary in ordinal key order. Array.Sort(keys, values) only
    # permutes the values under PowerShell when both arguments are typed [Array], hence the casts.
    param($Dictionary)
    $count = $Dictionary.Count
    $keys = [string[]]::new($count)
    $values = [object[]]::new($count)
    $Dictionary.Keys.CopyTo($keys, 0)
    $Dictionary.Values.CopyTo($values, 0)
    [Array]::Sort([Array]$keys, [Array]$values, [StringComparer]::Ordinal)
    return , $values
}

function Get-VmrIssueStoreSortedView {
    # Deterministic order: issues by slug, aliases by alias, occurrences by slug then report_date,
    # status history by slug then changed_on then the order the entries were recorded in.
    param($Model)
    $ordinal = [StringComparer]::Ordinal
    $issues = [Collections.Generic.List[object]]::new((Get-VmrIssueStoreSortedValues -Dictionary $Model.Issues))
    $aliases = [Collections.Generic.List[object]]::new((Get-VmrIssueStoreSortedValues -Dictionary $Model.Aliases))

    $occurrences = [Collections.Generic.List[object]]::new()
    $history = [Collections.Generic.List[object]]::new()
    foreach ($pair in @(@($Model.Occurrences, $occurrences, 'report_date'), @($Model.History, $history, 'changed_on'))) {
        $source = $pair[0]; $target = $pair[1]; $field = $pair[2]
        foreach ($rows in (Get-VmrIssueStoreSortedValues -Dictionary $source)) {
            $count = $rows.Count
            if ($count -eq 0) { continue }
            if ($count -eq 1) { $target.Add($rows[0]); continue }
            # Ordinal sort of "<key><NUL><original position>" is stable: equal keys keep the order they were
            # recorded in.
            $sortKeys = [string[]]::new($count)
            for ($i = 0; $i -lt $count; $i++) { $sortKeys[$i] = "$($rows[$i][$field])`0$($i.ToString('D8'))" }
            [Array]::Sort($sortKeys, $ordinal)
            foreach ($sortKey in $sortKeys) { $target.Add($rows[[int]$sortKey.Substring($sortKey.Length - 8)]) }
        }
    }
    return @{ Issues = $issues; Aliases = $aliases; Occurrences = $occurrences; History = $history }
}
function ConvertTo-VmrIssueStoreBytes {
    param($Model, [string] $ExportedAtUtc)
    $view = Get-VmrIssueStoreSortedView -Model $Model
    $document = [ordered]@{ schemaVersion = $script:VmrIssueStoreSchema; updatedAtUtc = $Model.UpdatedAtUtc }
    if ($ExportedAtUtc) { $document['exportedAtUtc'] = $ExportedAtUtc }
    $document['issues'] = $view.Issues
    $document['aliases'] = $view.Aliases
    $document['occurrences'] = $view.Occurrences
    $document['statusHistory'] = $view.History
    $json = ConvertTo-Json -InputObject $document -Depth 20
    # ConvertTo-Json uses the platform newline; pin LF so the file is identical everywhere.
    $text = $json.Replace("`r`n", "`n") + "`n"
    $bytes = [Text.UTF8Encoding]::new($false).GetBytes($text)
    return , $bytes
}

function Save-VmrIssueStoreModel {
    param([string] $FullPath, $Model)
    $Model.UpdatedAtUtc = [DateTime]::UtcNow.ToString('o')
    $Model.SchemaVersion = $script:VmrIssueStoreSchema
    $bytes = ConvertTo-VmrIssueStoreBytes -Model $Model
    $stamp = Write-VmrIssueStoreFile -FullPath $FullPath -Bytes $bytes -SweepStale
    $Model.Dirty = $false
    $script:VmrIssueStoreCache[(Get-VmrIssueStoreCacheKey -FullPath $FullPath)] = @{
        Hash = [Convert]::ToBase64String([Security.Cryptography.SHA256]::HashData($bytes))
        Length = $stamp.Length; Ticks = $stamp.Ticks; Model = $Model
    }
}

# ---------------------------------------------------------------------------------------------
# Loading: bytes -> normalised in-memory model
# ---------------------------------------------------------------------------------------------
# The model is a hashtable of dictionaries keyed by slug (Issues), alias (Aliases) and slug again for
# the per-issue lists (Occurrences, History). Records are ordered dictionaries with every field
# present, so nothing downstream has to cope with missing properties under StrictMode.

$script:VmrIssueStoreFactFields = @(@('owner_team', 'owner_source'), @('tracker_url', 'tracker_source'))

function New-VmrIssueStoreModel {
    return @{
        SchemaVersion = $script:VmrIssueStoreSchema
        UpdatedAtUtc  = $null
        Issues        = [Collections.Generic.Dictionary[string, object]]::new([StringComparer]::Ordinal)
        Aliases       = [Collections.Generic.Dictionary[string, object]]::new([StringComparer]::Ordinal)
        Occurrences   = [Collections.Generic.Dictionary[string, object]]::new([StringComparer]::Ordinal)
        History       = [Collections.Generic.Dictionary[string, object]]::new([StringComparer]::Ordinal)
        Dirty         = $false
    }
}

function ConvertTo-VmrIssueStoreText {
    # Blank means absent everywhere in this store, exactly as it did when it was a database.
    param($Value)
    if ($null -eq $Value) { return $null }
    if ($Value -is [string]) {
        if ([string]::IsNullOrWhiteSpace($Value)) { return $null }
        return $Value
    }
    if ($Value -is [datetime]) { return $Value.ToString('o') }
    $text = [string]$Value
    if ([string]::IsNullOrWhiteSpace($text)) { return $null }
    return $text
}

function ConvertTo-VmrIssueStoreInt {
    param($Value)
    if ($null -eq $Value) { return 0 }
    try { return [int]$Value } catch { return 0 }
}

function ConvertTo-VmrIssueStoreRecord {
    param($Source, [string[]] $Fields)
    $record = [ordered]@{}
    foreach ($field in $Fields) {
        $value = $Source[$field]
        if ($value -is [string]) { if ([string]::IsNullOrWhiteSpace($value)) { $value = $null } }
        elseif ($null -ne $value) { $value = ConvertTo-VmrIssueStoreText -Value $value }
        $record[$field] = $value
    }
    return $record
}

function Copy-VmrIssueStoreRecord {
    param($Record)
    $copy = [ordered]@{}
    foreach ($key in $Record.Keys) { $copy[$key] = $Record[$key] }
    return $copy
}

function Test-VmrIssueStoreHasOperatorHistory {
    param($Model, [string] $Slug)
    $entries = $null
    if (-not $Model.History.TryGetValue($Slug, [ref]$entries)) { return $false }
    foreach ($entry in $entries) { if ($entry['source'] -ieq 'operator') { return $true } }
    return $false
}

function ConvertTo-VmrIssueStoreProvenance {
    # Normalises recorded provenance and infers it for data written before provenance existed
    # (schema 1): a slug with any operator status entry has operator-recorded facts, otherwise they
    # came from the daily run.
    param($Model)
    foreach ($issue in $Model.Issues.Values) {
        $hasOperator = $null
        foreach ($pair in $script:VmrIssueStoreFactFields) {
            $valueField = $pair[0]; $sourceField = $pair[1]
            if ($null -eq $issue[$valueField]) { $issue[$sourceField] = $null; continue }
            $recorded = $issue[$sourceField]
            if ($null -eq $recorded) {
                if ($null -eq $hasOperator) { $hasOperator = Test-VmrIssueStoreHasOperatorHistory -Model $Model -Slug $issue['slug'] }
                $issue[$sourceField] = if ($hasOperator) { 'operator' } else { 'daily-run' }
            }
            else { $issue[$sourceField] = if ($recorded -ieq 'operator') { 'operator' } else { 'daily-run' } }
        }
    }
}

function ConvertTo-VmrIssueStoreModel {
    param($Parsed, [string] $Label)
    $model = New-VmrIssueStoreModel
    $stamp = $Parsed['updatedAtUtc']
    if ($null -eq $stamp) { $stamp = $Parsed['exportedAtUtc'] }
    $model.UpdatedAtUtc = ConvertTo-VmrIssueStoreText -Value $stamp

    $items = $Parsed['issues']
    if ($items -is [System.Collections.IDictionary]) { $items = @($items) }
    foreach ($source in $items) {
        if ($source -isnot [System.Collections.IDictionary]) { continue }
        $issue = ConvertTo-VmrIssueStoreRecord -Source $source -Fields $script:VmrIssueStoreIssueFields
        $slug = $issue['slug']
        if ($null -eq $slug) { continue }
        if ($null -eq $issue['name']) { $issue['name'] = $slug }
        if ($null -eq $issue['kind']) { $issue['kind'] = 'build' }
        if ($null -eq $issue['status']) { $issue['status'] = 'open' }
        $issue['occurrences'] = ConvertTo-VmrIssueStoreInt -Value $source['occurrences']
        if ($model.Issues.ContainsKey($slug)) { Write-Warning "$Label lists issue '$slug' more than once; keeping the first entry."; continue }
        $model.Issues[$slug] = $issue
    }

    $items = $Parsed['aliases']
    if ($items -is [System.Collections.IDictionary]) { $items = @($items) }
    foreach ($source in $items) {
        if ($source -isnot [System.Collections.IDictionary]) { continue }
        $alias = ConvertTo-VmrIssueStoreRecord -Source $source -Fields $script:VmrIssueStoreAliasFields
        if ($null -eq $alias['alias'] -or $null -eq $alias['slug']) { continue }
        if (-not $model.Aliases.ContainsKey($alias['alias'])) { $model.Aliases[$alias['alias']] = $alias }
    }

    $items = $Parsed['occurrences']
    if ($items -is [System.Collections.IDictionary]) { $items = @($items) }
    foreach ($source in $items) {
        if ($source -isnot [System.Collections.IDictionary]) { continue }
        $row = ConvertTo-VmrIssueStoreRecord -Source $source -Fields $script:VmrIssueStoreOccurrenceFields
        if ($null -eq $row['slug'] -or $null -eq $row['report_date']) { continue }
        $row['build_count'] = ConvertTo-VmrIssueStoreInt -Value $source['build_count']
        $rows = $null
        if (-not $model.Occurrences.TryGetValue($row['slug'], [ref]$rows)) {
            $rows = [Collections.Generic.List[object]]::new()
            $model.Occurrences[$row['slug']] = $rows
        }
        $duplicate = $false
        foreach ($existing in $rows) { if ($existing['report_date'] -ceq $row['report_date']) { $duplicate = $true; break } }
        if (-not $duplicate) { $rows.Add($row) }
    }

    $items = $Parsed['statusHistory']
    if ($items -is [System.Collections.IDictionary]) { $items = @($items) }
    foreach ($source in $items) {
        if ($source -isnot [System.Collections.IDictionary]) { continue }
        $entry = ConvertTo-VmrIssueStoreRecord -Source $source -Fields $script:VmrIssueStoreStatusFields
        if ($null -eq $entry['slug'] -or $null -eq $entry['status']) { continue }
        $entries = $null
        if (-not $model.History.TryGetValue($entry['slug'], [ref]$entries)) {
            $entries = [Collections.Generic.List[object]]::new()
            $model.History[$entry['slug']] = $entries
        }
        $entries.Add($entry)
    }

    ConvertTo-VmrIssueStoreProvenance -Model $model
    return $model
}

function Assert-VmrIssueStoreNotSqlite {
    param([byte[]] $Bytes, [string] $Label)
    if ($Bytes.Length -ge 15 -and [Text.Encoding]::ASCII.GetString($Bytes, 0, 15) -ceq 'SQLite format 3') {
        throw "$Label is a SQLite database. SQLite issue-history stores are no longer supported because the native libraries they need cannot ship in this repository. Run Export-VmrIssueHistory from the previous version of the package to write a JSON export, then load it into a new JSON store with Import-VmrIssueHistory -Path <new-store>.json -InputPath <export>.json."
    }
}

function ConvertFrom-VmrIssueStoreBytes {
    # Context 'store' tolerates a blank file (a placeholder that was never initialised); 'export' does not.
    # -SchemaOnly validates the document and returns just its schemaVersion, even a newer one.
    param([byte[]] $Bytes, [string] $Label, [string] $Context, [switch] $SchemaOnly)
    Assert-VmrIssueStoreNotSqlite -Bytes $Bytes -Label $Label
    $reader = [IO.StreamReader]::new([IO.MemoryStream]::new($Bytes), [Text.UTF8Encoding]::new($false), $true)
    try { $text = $reader.ReadToEnd() } finally { $reader.Dispose() }
    if ([string]::IsNullOrWhiteSpace($text)) {
        if ($Context -eq 'export') { throw "$Label is empty." }
        if ($SchemaOnly) { return 0 }
        $blank = New-VmrIssueStoreModel
        $blank.SchemaVersion = 0
        $blank.Dirty = $true
        return $blank
    }
    try {
        $parsed = if ($script:VmrIssueStoreJsonHasDateKind) { ConvertFrom-Json -InputObject $text -AsHashtable -DateKind String }
                  else { ConvertFrom-Json -InputObject $text -AsHashtable }
    }
    catch { throw "$Label is not valid JSON: $($_.Exception.Message)" }

    # A JSON file that merely happens to exist (a config, say) must never be mistaken for a store
    # and then overwritten, so it has to look like one.
    $looksLikeStore = $false
    if ($parsed -is [System.Collections.IDictionary]) {
        foreach ($key in @('schemaVersion', 'issues', 'aliases', 'occurrences', 'statusHistory')) { if ($parsed.Contains($key)) { $looksLikeStore = $true; break } }
    }
    if (-not $looksLikeStore) { throw "$Label is not an issue history document (expected a JSON object with schemaVersion and issues)." }

    $schema = 0
    if ($null -ne $parsed['schemaVersion']) {
        try { $schema = [int]$parsed['schemaVersion'] } catch { throw "$Label has an unreadable schemaVersion '$($parsed['schemaVersion'])'." }
    }
    if ($schema -eq 0 -and $Context -eq 'store') { $schema = 1 }
    if ($SchemaOnly) { return $schema }
    if ($schema -gt $script:VmrIssueStoreSchema) {
        if ($Context -eq 'export') { throw "This export is schema $schema but these scripts understand schema $($script:VmrIssueStoreSchema). Update the package before importing." }
        throw "$Label is schema $schema but these scripts understand schema $($script:VmrIssueStoreSchema). Update the package before using it."
    }
    $model = ConvertTo-VmrIssueStoreModel -Parsed $parsed -Label $Label
    $model.SchemaVersion = $schema
    return $model
}

function Get-VmrIssueStoreModel {
    # Read-only callers (no -Locked) trust the cache while the file's size and timestamp are unchanged.
    # Writers (-Locked) must see the latest bytes, so they verify the cache against a content hash:
    # reading and hashing the file is far cheaper than re-parsing it, and a timestamp alone can be
    # fooled by two writes inside one clock tick.
    param([string] $FullPath, [switch] $Locked)
    $key = Get-VmrIssueStoreCacheKey -FullPath $FullPath
    $entry = $script:VmrIssueStoreCache[$key]
    $info = [IO.FileInfo]::new($FullPath)
    if (-not $info.Exists) {
        [void]$script:VmrIssueStoreCache.Remove($key)
        if ($Locked) {
            $fresh = New-VmrIssueStoreModel
            $fresh.Dirty = $true
            return $fresh
        }
        return $null
    }
    $length = $info.Length
    $ticks = $info.LastWriteTimeUtc.Ticks
    if (-not $Locked -and $null -ne $entry -and $entry.Length -eq $length -and $entry.Ticks -eq $ticks) { return $entry.Model }

    $bytes = Read-VmrIssueStoreBytes -FullPath $FullPath
    $hash = [Convert]::ToBase64String([Security.Cryptography.SHA256]::HashData($bytes))
    if ($null -ne $entry -and $entry.Hash -ceq $hash) {
        $entry.Length = $length
        $entry.Ticks = $ticks
        return $entry.Model
    }
    $model = ConvertFrom-VmrIssueStoreBytes -Bytes $bytes -Label "Issue history store $FullPath" -Context 'store'
    $script:VmrIssueStoreCache[$key] = @{ Hash = $hash; Length = $length; Ticks = $ticks; Model = $model }
    return $model
}

function Invoke-VmrIssueStoreTransaction {
    # Runs -Action under the exclusive lock against the freshest model and saves only if the action
    # changed something. Locals are prefixed so they can never shadow a variable an action reads from
    # the calling function's scope.
    param(
        [Parameter(Mandatory)][string] $StorePath,
        [Parameter(Mandatory)][scriptblock] $Action,
        [switch] $Create
    )
    $txFull = Resolve-VmrIssueStorePath -Path $StorePath
    if ([IO.Directory]::Exists($txFull)) { throw "$StorePath is a folder, not an issue history store file." }
    if (-not $Create -and -not [IO.File]::Exists($txFull)) {
        throw "Issue history store not found: $StorePath. Create it with Initialize-VmrIssueStore first."
    }
    $txDirectory = [IO.Path]::GetDirectoryName($txFull)
    if ($Create -and $txDirectory -and -not [IO.Directory]::Exists($txDirectory)) { [void][IO.Directory]::CreateDirectory($txDirectory) }

    $txLock = Enter-VmrIssueStoreLock -FullPath $txFull
    try {
        $txModel = Get-VmrIssueStoreModel -FullPath $txFull -Locked
        $txResult = & $Action $txModel
        if ($txModel.Dirty) { Save-VmrIssueStoreModel -FullPath $txFull -Model $txModel }
        return $txResult
    }
    catch {
        # The cached model may be half-mutated; make the next call re-read the file.
        [void]$script:VmrIssueStoreCache.Remove((Get-VmrIssueStoreCacheKey -FullPath $txFull))
        throw
    }
    finally { $txLock.Dispose() }
}

# ---------------------------------------------------------------------------------------------
# Model primitives. Every real change marks the model Dirty; a call that changes nothing never writes.
# ---------------------------------------------------------------------------------------------

function ConvertTo-VmrIssueSlug {
    param([Parameter(Mandatory)][AllowEmptyString()][string] $Name)
    $slug = $Name.ToLowerInvariant()
    $slug = [regex]::Replace($slug, '^bh #(?:\d+|-)\s*-\s*', '')
    $slug = [regex]::Replace($slug, '[`*"]', '')
    $slug = [regex]::Replace($slug, '[^a-z0-9]+', '-')
    return $slug.Trim('-')
}

function Resolve-VmrIssueSlugValue {
    # Follows alias rows to the canonical slug. Registration and import keep aliases flat, but a hand
    # edited store might not, so follow a bounded chain rather than trust it.
    param($Model, [string] $Slug)
    $hops = 0
    while ($hops -lt 16 -and $Model.Aliases.ContainsKey($Slug)) {
        $target = $Model.Aliases[$Slug]['slug']
        if ($target -ceq $Slug) { break }
        $Slug = $target
        $hops++
    }
    return $Slug
}

function Resolve-VmrIssueSlug {
    param($Model, [string] $Name)
    return Resolve-VmrIssueSlugValue -Model $Model -Slug (ConvertTo-VmrIssueSlug -Name $Name)
}

function Test-VmrIssueStoreSame {
    param($Left, $Right)
    if ($null -eq $Left) { return ($null -eq $Right) }
    if ($null -eq $Right) { return $false }
    return ($Left -ceq $Right)
}

function Set-VmrIssueStoreField {
    param($Model, $Record, [string] $Field, $Value)
    if (Test-VmrIssueStoreSame -Left $Record[$Field] -Right $Value) { return $false }
    $Record[$Field] = $Value
    $Model.Dirty = $true
    return $true
}

function Get-VmrIssueStoreEarlier {
    param($Left, $Right)
    if ($null -eq $Left) { return $Right }
    if ($null -eq $Right) { return $Left }
    if ([string]::CompareOrdinal($Left, $Right) -le 0) { return $Left }
    return $Right
}

function Get-VmrIssueStoreLater {
    param($Left, $Right)
    if ($null -eq $Left) { return $Right }
    if ($null -eq $Right) { return $Left }
    if ([string]::CompareOrdinal($Left, $Right) -ge 0) { return $Left }
    return $Right
}

function Add-VmrIssueStoreHistoryEntry {
    param($Model, [string] $Slug, [string] $ChangedOn, [string] $Status, [string] $Note, [string] $Source)
    $entries = $null
    if (-not $Model.History.TryGetValue($Slug, [ref]$entries)) {
        $entries = [Collections.Generic.List[object]]::new()
        $Model.History[$Slug] = $entries
    }
    $entries.Add([ordered]@{
        slug = $Slug; changed_on = $ChangedOn; status = $Status
        note = (ConvertTo-VmrIssueStoreText -Value $Note); source = (ConvertTo-VmrIssueStoreText -Value $Source)
    })
    $Model.Dirty = $true
}

function Update-VmrIssueStoreDerivedFields {
    # occurrences is always the number of occurrence rows. Unless -CountOnly, first_seen/last_seen are
    # also widened to cover every row, so the range is the min/max of all dates seen whatever order
    # the reports were replayed in.
    param($Model, $Issue, [switch] $CountOnly)
    $rows = $null
    $count = 0
    $first = $Issue['first_seen']
    $last = $Issue['last_seen']
    if ($Model.Occurrences.TryGetValue($Issue['slug'], [ref]$rows)) {
        $count = $rows.Count
        if (-not $CountOnly) {
            foreach ($row in $rows) {
                $first = Get-VmrIssueStoreEarlier $first $row['report_date']
                $last = Get-VmrIssueStoreLater $last $row['report_date']
            }
        }
    }
    [void](Set-VmrIssueStoreField -Model $Model -Record $Issue -Field 'occurrences' -Value $count)
    if (-not $CountOnly) {
        [void](Set-VmrIssueStoreField -Model $Model -Record $Issue -Field 'first_seen' -Value $first)
        [void](Set-VmrIssueStoreField -Model $Model -Record $Issue -Field 'last_seen' -Value $last)
    }
}

function Merge-VmrIssueStoreFact {
    # The target's non-blank value wins, except that a daily-run value yields to an operator value, so
    # an operator-recorded fact is never lost to an automated one whichever side of a merge it is on.
    param($Model, $Target, $Other, [string] $ValueField, [string] $SourceField)
    $incoming = $Other[$ValueField]
    if ($null -eq $incoming) { return }
    $incomingSource = if ($Other[$SourceField] -ceq 'operator') { 'operator' } else { 'daily-run' }
    $take = ($null -eq $Target[$ValueField]) -or ($Target[$SourceField] -cne 'operator' -and $incomingSource -ceq 'operator')
    if (-not $take) { return }
    [void](Set-VmrIssueStoreField -Model $Model -Record $Target -Field $ValueField -Value $incoming)
    [void](Set-VmrIssueStoreField -Model $Model -Record $Target -Field $SourceField -Value $incomingSource)
}

function Merge-VmrIssueStoreRecord {
    # Used for both an import conflict and an alias fold: Target is the record that is kept. Other
    # only fills what Target leaves blank, in the fields named by -FillFields.
    param($Model, $Target, $Other, [string[]] $FillFields = @('resolution', 'resolved_on', 'notes'))
    [void](Set-VmrIssueStoreField -Model $Model -Record $Target -Field 'first_seen' -Value (Get-VmrIssueStoreEarlier $Target['first_seen'] $Other['first_seen']))
    [void](Set-VmrIssueStoreField -Model $Model -Record $Target -Field 'last_seen' -Value (Get-VmrIssueStoreLater $Target['last_seen'] $Other['last_seen']))
    foreach ($pair in $script:VmrIssueStoreFactFields) {
        Merge-VmrIssueStoreFact -Model $Model -Target $Target -Other $Other -ValueField $pair[0] -SourceField $pair[1]
    }
    foreach ($field in $FillFields) {
        if ($null -eq $Target[$field] -and $null -ne $Other[$field]) {
            [void](Set-VmrIssueStoreField -Model $Model -Record $Target -Field $field -Value $Other[$field])
        }
    }
}

function Move-VmrIssueStoreRows {
    # Re-homes an alias slug's occurrences and status history onto the canonical slug. When both
    # already have an occurrence for the same day, the canonical's row wins.
    param($Model, [string] $FromSlug, [string] $ToSlug)
    $rows = $null
    if ($Model.Occurrences.TryGetValue($FromSlug, [ref]$rows)) {
        if ($rows.Count -gt 0) {
            $destination = $null
            if (-not $Model.Occurrences.TryGetValue($ToSlug, [ref]$destination)) {
                $destination = [Collections.Generic.List[object]]::new()
                $Model.Occurrences[$ToSlug] = $destination
            }
            foreach ($row in $rows) {
                $exists = $false
                foreach ($candidate in $destination) { if ($candidate['report_date'] -ceq $row['report_date']) { $exists = $true; break } }
                if (-not $exists) { $row['slug'] = $ToSlug; $destination.Add($row) }
            }
            $Model.Dirty = $true
        }
        [void]$Model.Occurrences.Remove($FromSlug)
    }
    $entries = $null
    if ($Model.History.TryGetValue($FromSlug, [ref]$entries)) {
        if ($entries.Count -gt 0) {
            $destination = $null
            if (-not $Model.History.TryGetValue($ToSlug, [ref]$destination)) {
                $destination = [Collections.Generic.List[object]]::new()
                $Model.History[$ToSlug] = $destination
            }
            foreach ($entry in $entries) { $entry['slug'] = $ToSlug; $destination.Add($entry) }
            $Model.Dirty = $true
        }
        [void]$Model.History.Remove($FromSlug)
    }
}

function Merge-VmrIssueStoreIntoCanonical {
    # Folds the issue recorded under an alias slug into its canonical issue so that a rename never
    # strands history. Returns 'merged', or 'renamed' when the canonical issue did not exist yet and
    # the alias's issue simply became it (the original left such rows orphaned and unreachable).
    param($Model, [string] $AliasSlug, [string] $Canonical, [string] $CanonicalName)
    $alias = $Model.Issues[$AliasSlug]
    if (-not $Model.Issues.ContainsKey($Canonical)) {
        [void]$Model.Issues.Remove($AliasSlug)
        $alias['slug'] = $Canonical
        if (-not [string]::IsNullOrWhiteSpace($CanonicalName)) { $alias['name'] = $CanonicalName.Trim() }
        $Model.Issues[$Canonical] = $alias
        Move-VmrIssueStoreRows -Model $Model -FromSlug $AliasSlug -ToSlug $Canonical
        Update-VmrIssueStoreDerivedFields -Model $Model -Issue $alias
        $Model.Dirty = $true
        return 'renamed'
    }
    $target = $Model.Issues[$Canonical]
    # Like the original fold this fills resolution (and, additionally, notes) but not resolved_on,
    # which describes the canonical's own status.
    Merge-VmrIssueStoreRecord -Model $Model -Target $target -Other $alias -FillFields @('resolution', 'notes')
    Move-VmrIssueStoreRows -Model $Model -FromSlug $AliasSlug -ToSlug $Canonical
    [void]$Model.Issues.Remove($AliasSlug)
    Update-VmrIssueStoreDerivedFields -Model $Model -Issue $target -CountOnly
    $Model.Dirty = $true
    return 'merged'
}

function Get-VmrIssueFactSource {
    # Fact provenance is only ever 'operator' or 'daily-run'. Any other label (a status entry can carry
    # a free-form source such as an investigation name) takes the calling function's default nature.
    param([string] $Source, [string] $Default)
    if ($Source -ieq 'operator') { return 'operator' }
    if ($Source -ieq 'daily-run') { return 'daily-run' }
    return $Default
}

function Test-VmrIssueStoreFactWritable {
    # A daily-run write may fill a blank or replace another daily-run value, never an operator value.
    param($Issue, [string] $ValueField, [string] $SourceField, [string] $Source)
    if ($null -eq $Issue[$ValueField]) { return $true }
    return ($Issue[$SourceField] -cne 'operator' -or $Source -ceq 'operator')
}

function Update-VmrIssueStoreFact {
    param($Model, $Issue, [string] $ValueField, [string] $SourceField, [string] $Value, [string] $Source)
    $valueChanged = Set-VmrIssueStoreField -Model $Model -Record $Issue -Field $ValueField -Value $Value
    $sourceChanged = Set-VmrIssueStoreField -Model $Model -Record $Issue -Field $SourceField -Value $Source
    return ($valueChanged -or $sourceChanged)
}

function Resolve-VmrIssueOwnerHandle {
    # Returns the canonical handle, or $null when it names a team that does not exist. A person handle
    # (no slash) never reaches the resolver. When the resolver (Resolve-VmrQBOwnerTeam, supplied by
    # another module) is not loaded the value is kept as given. Get-Command is deliberately avoided: for
    # a name that is not defined it falls back to a slow module search on every call.
    param([string] $Handle)
    $trimmed = $Handle.Trim()
    if ($trimmed -match '^@[A-Za-z0-9][A-Za-z0-9-]*$') { return $trimmed }
    if (-not (Test-Path -LiteralPath 'function:\Resolve-VmrQBOwnerTeam')) { return $trimmed }
    $resolved = Resolve-VmrQBOwnerTeam -Handle $trimmed
    if ([string]::IsNullOrWhiteSpace([string]$resolved)) { return $null }
    return ([string]$resolved).Trim()
}

# ---------------------------------------------------------------------------------------------
# Public API: writers
# ---------------------------------------------------------------------------------------------

function New-VmrIssueStoreIssue {
    param([string] $Slug, [string] $Name, [string] $Kind, [string] $Status, [string] $Date)
    return [ordered]@{
        slug = $Slug; name = $Name; kind = $Kind; status = $Status
        owner_team = $null; owner_source = $null; tracker_url = $null; tracker_source = $null
        first_seen = $Date; last_seen = $Date; occurrences = 0
        resolution = $null; resolved_on = $null; notes = $null
    }
}

function Initialize-VmrIssueStore {
    param([Parameter(Mandatory)][string] $Path)
    $full = Resolve-VmrIssueStorePath -Path $Path
    $directory = [IO.Path]::GetDirectoryName($full)
    if ($directory -and -not [IO.Directory]::Exists($directory)) { [void][IO.Directory]::CreateDirectory($directory) }
    $info = [IO.FileInfo]::new($full)
    if ($info.Exists -and $info.Length -gt 0) {
        # Validates an existing file (SQLite, invalid JSON, newer schema, not a store) without rewriting it.
        $existing = Get-VmrIssueStoreModel -FullPath $full
        if (-not $existing.Dirty) { return $Path }
    }
    # Missing, empty, or whitespace-only: write an empty store.
    Invoke-VmrIssueStoreTransaction -StorePath $Path -Create -Action { param($m) $null } | Out-Null
    return $Path
}

function Register-VmrIssueAlias {
    param(
        [Parameter(Mandatory)][string] $Path,
        [Parameter(Mandatory)][string] $Alias,
        [Parameter(Mandatory)][string] $CanonicalName,
        [string] $On = (Get-Date -Format 'yyyy-MM-dd')
    )
    $aliasSlug = ConvertTo-VmrIssueSlug -Name $Alias
    $canonicalSlug = ConvertTo-VmrIssueSlug -Name $CanonicalName
    if (-not $aliasSlug -or -not $canonicalSlug) {
        throw "Alias '$Alias' and canonical name '$CanonicalName' must each contain at least one letter or digit."
    }
    return Invoke-VmrIssueStoreTransaction -StorePath $Path -Action {
        param($m)
        # Resolve through existing aliases first so a chain can never form.
        $canonical = Resolve-VmrIssueSlugValue -Model $m -Slug $canonicalSlug
        if ($aliasSlug -ceq $canonical) { return $canonical }

        # If the alias already accumulated its own history, fold it in rather than orphan it -
        # the whole point of aliasing is that a rename must not lose prior occurrences.
        if ($m.Issues.ContainsKey($aliasSlug)) {
            [void](Merge-VmrIssueStoreIntoCanonical -Model $m -AliasSlug $aliasSlug -Canonical $canonical -CanonicalName $CanonicalName)
        }
        foreach ($other in $m.Aliases.Values) {
            if ($other['slug'] -ceq $aliasSlug) { $other['slug'] = $canonical; $m.Dirty = $true }
        }
        $existing = $null
        if ($m.Aliases.TryGetValue($aliasSlug, [ref]$existing)) {
            # Re-registering keeps the original date, so re-applying the alias file daily changes nothing.
            [void](Set-VmrIssueStoreField -Model $m -Record $existing -Field 'slug' -Value $canonical)
            [void](Set-VmrIssueStoreField -Model $m -Record $existing -Field 'first_seen' -Value (Get-VmrIssueStoreEarlier $existing['first_seen'] $On))
        }
        else {
            $m.Aliases[$aliasSlug] = [ordered]@{ alias = $aliasSlug; slug = $canonical; first_seen = $On }
            $m.Dirty = $true
        }
        return $canonical
    }
}

function Add-VmrIssueOccurrence {
    param(
        [Parameter(Mandatory)][string] $Path,
        [Parameter(Mandatory)][string] $Name,
        [Parameter(Mandatory)][string] $ReportDate,
        [string] $Kind = 'build',
        [string] $BhNumber,
        [string] $NewToday,
        [string[]] $BuildIds = @(),
        [string] $OwnerTeam,
        [string] $TrackerUrl,
        [string] $ReportPath,
        [string] $Source = 'daily-run'
    )
    $factSource = Get-VmrIssueFactSource -Source $Source -Default 'daily-run'
    $kindText = ConvertTo-VmrIssueStoreText -Value $Kind
    if ($null -eq $kindText) { $kindText = 'build' }
    $ownerText = ConvertTo-VmrIssueStoreText -Value $OwnerTeam
    if ($null -ne $ownerText) { $ownerText = $ownerText.Trim() }
    $trackerText = ConvertTo-VmrIssueStoreText -Value $TrackerUrl
    if ($null -ne $trackerText) { $trackerText = $trackerText.Trim() }
    $bhText = ConvertTo-VmrIssueStoreText -Value $BhNumber
    $newTodayText = ConvertTo-VmrIssueStoreText -Value $NewToday
    $reportPathText = ConvertTo-VmrIssueStoreText -Value $ReportPath
    $buildIdList = @($BuildIds | Where-Object { $_ })
    $buildIdText = if ($buildIdList.Count -gt 0) { $buildIdList -join ',' } else { $null }

    # Validate the owner BEFORE taking the store lock, so a slow Resolve-VmrQBOwnerTeam can never hold other
    # writers up. A look at the current store avoids asking when the answer cannot matter (an operator owns
    # the value, or it is already recorded). The decision to write is still made under the lock.
    $ownerToApply = $null
    if ($null -ne $ownerText) {
        $needsOwner = $true
        $peek = Get-VmrIssueStoreModel -FullPath (Resolve-VmrIssueStorePath -Path $Path)
        if ($null -ne $peek) {
            $peekSlug = Resolve-VmrIssueSlug -Model $peek -Name $Name
            if ($peek.Issues.ContainsKey($peekSlug)) {
                $peekIssue = $peek.Issues[$peekSlug]
                if (-not (Test-VmrIssueStoreFactWritable -Issue $peekIssue -ValueField 'owner_team' -SourceField 'owner_source' -Source $factSource)) { $needsOwner = $false }
                elseif ((Test-VmrIssueStoreSame $peekIssue['owner_team'] $ownerText) -and (Test-VmrIssueStoreSame $peekIssue['owner_source'] $factSource)) { $needsOwner = $false }
            }
        }
        if ($needsOwner) {
            $reason = 'it is not a known team handle'
            try { $ownerToApply = Resolve-VmrIssueOwnerHandle -Handle $ownerText }
            catch { $reason = "the team lookup failed: $($_.Exception.Message)" }
            if ($null -eq $ownerToApply) { Write-Warning "Ignored owner '$ownerText' for '$Name' ($ReportDate): $reason." }
        }
    }

    return Invoke-VmrIssueStoreTransaction -StorePath $Path -Action {
        param($m)
        $slug = Resolve-VmrIssueSlug -Model $m -Name $Name
        if (-not $slug) {
            Write-Warning "Skipped the $ReportDate occurrence of '$Name': it has no letters or digits to build an issue identity from."
            return $null
        }
        $issue = $null
        if ($m.Issues.ContainsKey($slug)) { $issue = $m.Issues[$slug] }
        $rows = $null
        if (-not $m.Occurrences.TryGetValue($slug, [ref]$rows)) {
            $rows = [Collections.Generic.List[object]]::new()
            $m.Occurrences[$slug] = $rows
        }
        $row = $null
        foreach ($candidate in $rows) { if ($candidate['report_date'] -ceq $ReportDate) { $row = $candidate; break } }

        if ($null -eq $issue) {
            $issue = New-VmrIssueStoreIssue -Slug $slug -Name $Name -Kind $kindText -Status 'open' -Date $ReportDate
            $m.Issues[$slug] = $issue
            Add-VmrIssueStoreHistoryEntry -Model $m -Slug $slug -ChangedOn $ReportDate -Status 'open' -Note 'First observed.' -Source $Source
        }
        elseif ($null -eq $row -and $issue['status'] -ceq 'resolved') {
            # A resolved issue that fires again is a regression, and that transition must be visible.
            # Replaying a day that is already recorded is not a new occurrence, so it cannot regress anything.
            $issue['status'] = 'regressed'
            $issue['resolution'] = $null
            $issue['resolved_on'] = $null
            Add-VmrIssueStoreHistoryEntry -Model $m -Slug $slug -ChangedOn $ReportDate -Status 'regressed' -Note 'Recurred after being marked resolved.' -Source $Source
        }

        if ($null -eq $row) {
            $rows.Add([ordered]@{
                slug = $slug; report_date = $ReportDate; name_used = $Name; bh_number = $bhText; new_today = $newTodayText
                build_ids = $buildIdText; build_count = $buildIdList.Count; report_path = $reportPathText
            })
            $m.Dirty = $true
        }
        else {
            # The same day again replaces that day's evidence rather than adding to it.
            foreach ($pair in @(@('name_used', $Name), @('bh_number', $bhText), @('new_today', $newTodayText), @('build_ids', $buildIdText), @('build_count', $buildIdList.Count), @('report_path', $reportPathText))) {
                [void](Set-VmrIssueStoreField -Model $m -Record $row -Field $pair[0] -Value $pair[1])
            }
        }

        # Owner and tracker written by a report fill blanks and replace other daily-run values only.
        if ($null -ne $ownerToApply -and (Test-VmrIssueStoreFactWritable -Issue $issue -ValueField 'owner_team' -SourceField 'owner_source' -Source $factSource)) {
            [void](Update-VmrIssueStoreFact -Model $m -Issue $issue -ValueField 'owner_team' -SourceField 'owner_source' -Value $ownerToApply -Source $factSource)
        }
        if ($null -ne $trackerText -and (Test-VmrIssueStoreFactWritable -Issue $issue -ValueField 'tracker_url' -SourceField 'tracker_source' -Source $factSource)) {
            [void](Update-VmrIssueStoreFact -Model $m -Issue $issue -ValueField 'tracker_url' -SourceField 'tracker_source' -Value $trackerText -Source $factSource)
        }

        Update-VmrIssueStoreDerivedFields -Model $m -Issue $issue
        return $slug
    }
}

function Set-VmrIssueStatus {
    param(
        [Parameter(Mandatory)][string] $Path,
        [Parameter(Mandatory)][string] $Name,
        [Parameter(Mandatory)][ValidateSet('open', 'routed', 'owned', 'resolved', 'regressed', 'noise', 'wontfix')][string] $Status,
        [string] $Note,
        [string] $Resolution,
        [string] $OwnerTeam,
        [string] $TrackerUrl,
        [string] $On = (Get-Date -Format 'yyyy-MM-dd'),
        [string] $Source = 'operator',
        [switch] $AllowUnknownOwner
    )
    if (-not (ConvertTo-VmrIssueSlug -Name $Name)) { throw "'$Name' has no letters or digits to build an issue identity from." }
    $statusText = $Status.ToLowerInvariant()
    $factSource = Get-VmrIssueFactSource -Source $Source -Default 'operator'

    # Validate the owner before taking the lock or touching anything: an unknown team is rejected, not stored.
    $ownerText = ConvertTo-VmrIssueStoreText -Value $OwnerTeam
    if ($null -ne $ownerText) {
        $ownerText = $ownerText.Trim()
        $canonicalOwner = Resolve-VmrIssueOwnerHandle -Handle $ownerText
        if ($null -eq $canonicalOwner) {
            if (-not $AllowUnknownOwner) {
                throw "'$ownerText' is not a known team handle, so it was not recorded for '$Name'. Check the spelling, or pass -AllowUnknownOwner to record it anyway."
            }
            $canonicalOwner = $ownerText
        }
        $ownerText = $canonicalOwner
    }
    $trackerText = ConvertTo-VmrIssueStoreText -Value $TrackerUrl
    if ($null -ne $trackerText) { $trackerText = $trackerText.Trim() }
    $noteText = ConvertTo-VmrIssueStoreText -Value $Note
    $resolutionText = ConvertTo-VmrIssueStoreText -Value $Resolution

    return Invoke-VmrIssueStoreTransaction -StorePath $Path -Action {
        param($m)
        $slug = Resolve-VmrIssueSlug -Model $m -Name $Name
        $issue = $null
        if ($m.Issues.ContainsKey($slug)) { $issue = $m.Issues[$slug] }
        if ($null -eq $issue) {
            $issue = New-VmrIssueStoreIssue -Slug $slug -Name $Name -Kind 'build' -Status $statusText -Date $On
            $issue['resolution'] = $resolutionText
            if ($statusText -ceq 'resolved') { $issue['resolved_on'] = $On }
            $m.Issues[$slug] = $issue
            $m.Dirty = $true
        }
        else {
            [void](Set-VmrIssueStoreField -Model $m -Record $issue -Field 'status' -Value $statusText)
            if ($statusText -ceq 'resolved') {
                if ($null -ne $resolutionText) { [void](Set-VmrIssueStoreField -Model $m -Record $issue -Field 'resolution' -Value $resolutionText) }
                [void](Set-VmrIssueStoreField -Model $m -Record $issue -Field 'resolved_on' -Value $On)
            }
        }
        # A value given here is a deliberate decision: it always lands and records who made it.
        # A blank or omitted one never clears what is already known.
        if ($null -ne $ownerText) {
            [void](Update-VmrIssueStoreFact -Model $m -Issue $issue -ValueField 'owner_team' -SourceField 'owner_source' -Value $ownerText -Source $factSource)
        }
        if ($null -ne $trackerText) {
            [void](Update-VmrIssueStoreFact -Model $m -Issue $issue -ValueField 'tracker_url' -SourceField 'tracker_source' -Value $trackerText -Source $factSource)
        }
        Add-VmrIssueStoreHistoryEntry -Model $m -Slug $slug -ChangedOn $On -Status $statusText -Note $noteText -Source $Source
        Update-VmrIssueStoreDerivedFields -Model $m -Issue $issue -CountOnly
        return $slug
    }
}

# ---------------------------------------------------------------------------------------------
# Public API: readers
# ---------------------------------------------------------------------------------------------

$script:VmrIssueStoreListFields = @('slug', 'name', 'status', 'owner_team', 'tracker_url', 'first_seen', 'last_seen', 'occurrences', 'resolution')
$script:VmrIssueStoreAliasRowFields = @('alias', 'first_seen')
$script:VmrIssueStoreOccurrenceRowFields = @('report_date', 'name_used', 'bh_number', 'new_today', 'build_count', 'build_ids')
$script:VmrIssueStoreTrailFields = @('changed_on', 'status', 'note', 'source')

function ConvertTo-VmrIssueStoreRow {
    # Callers always get copies, never the cached records, so nothing they do can corrupt the store.
    param($Record, [string[]] $Fields)
    $row = [ordered]@{}
    foreach ($field in $Fields) { $row[$field] = $Record[$field] }
    return [pscustomobject]$row
}

function Get-VmrIssueStoreSortedKeys {
    param($Dictionary)
    $keys = [string[]]::new($Dictionary.Count)
    $Dictionary.Keys.CopyTo($keys, 0)
    [Array]::Sort($keys, [StringComparer]::Ordinal)
    return , $keys
}

function Get-VmrIssueStoreCounts {
    param($Model)
    $occurrences = 0
    foreach ($rows in $Model.Occurrences.Values) { $occurrences += $rows.Count }
    $entries = 0
    foreach ($list in $Model.History.Values) { $entries += $list.Count }
    return @{ Issues = $Model.Issues.Count; Aliases = $Model.Aliases.Count; Occurrences = $occurrences; StatusEntries = $entries }
}

function Get-VmrIssueHistory {
    param(
        [Parameter(Mandatory)][string] $Path,
        [string] $Name,
        [string] $Status,
        [int] $SinceDays = 0,
        [int] $Limit = 50
    )
    # A store that does not exist yet simply has no history.
    $model = Get-VmrIssueStoreModel -FullPath (Resolve-VmrIssueStorePath -Path $Path)
    if ($Name) {
        if ($null -eq $model) { return $null }
        $slug = Resolve-VmrIssueSlug -Model $model -Name $Name
        if (-not $model.Issues.ContainsKey($slug)) { return $null }
        $aliases = [Collections.Generic.List[object]]::new()
        foreach ($row in $model.Aliases.Values) {
            if ($row['slug'] -ceq $slug) { $aliases.Add((ConvertTo-VmrIssueStoreRow -Record $row -Fields $script:VmrIssueStoreAliasRowFields)) }
        }
        $occurrences = [Collections.Generic.List[object]]::new()
        $rows = $null
        if ($model.Occurrences.TryGetValue($slug, [ref]$rows)) {
            foreach ($row in $rows) { $occurrences.Add((ConvertTo-VmrIssueStoreRow -Record $row -Fields $script:VmrIssueStoreOccurrenceRowFields)) }
        }
        $trail = [Collections.Generic.List[object]]::new()
        $entries = $null
        if ($model.History.TryGetValue($slug, [ref]$entries)) {
            foreach ($entry in $entries) { $trail.Add((ConvertTo-VmrIssueStoreRow -Record $entry -Fields $script:VmrIssueStoreTrailFields)) }
        }
        return [pscustomobject][ordered]@{
            issue       = [pscustomobject]$model.Issues[$slug]
            aliases     = @($aliases | Sort-Object -Property first_seen, alias -Stable)
            occurrences = @($occurrences | Sort-Object -Property report_date -Descending -Stable)
            statusTrail = @($trail | Sort-Object -Property changed_on -Stable)
        }
    }

    if ($null -eq $model) { return @() }
    $since = $null
    if ($SinceDays -gt 0) { $since = (Get-Date).AddDays(-$SinceDays).ToString('yyyy-MM-dd') }
    $rows = [Collections.Generic.List[object]]::new()
    foreach ($issue in $model.Issues.Values) {
        if ($Status -and $issue['status'] -cne $Status) { continue }
        if ($null -ne $since -and [string]::CompareOrdinal([string]$issue['last_seen'], $since) -lt 0) { continue }
        $rows.Add((ConvertTo-VmrIssueStoreRow -Record $issue -Fields $script:VmrIssueStoreListFields))
    }
    # slug last makes the order total, so ties never depend on how the file happened to be arranged.
    $sorted = @($rows | Sort-Object -Property @{ Expression = 'last_seen'; Descending = $true }, @{ Expression = 'occurrences'; Descending = $true }, @{ Expression = 'slug'; Descending = $false })
    if ($Limit -ge 0) { $sorted = @($sorted | Select-Object -First $Limit) }
    return $sorted
}

function Get-VmrIssueStoreSchemaVersion {
    param([Parameter(Mandatory)][string] $Path)
    $full = Resolve-VmrIssueStorePath -Path $Path
    if (-not [IO.File]::Exists($full)) { return 0 }
    $bytes = Read-VmrIssueStoreBytes -FullPath $full
    return [int](ConvertFrom-VmrIssueStoreBytes -Bytes $bytes -Label "Issue history store $full" -Context 'store' -SchemaOnly)
}

function Read-VmrIssueStore {
    # A deep copy of the whole store for analysis tools: mutating it cannot reach the cache or the file.
    # A store that does not exist yet reads as empty with schemaVersion 0.
    param([Parameter(Mandatory)][string] $Path)
    $model = Get-VmrIssueStoreModel -FullPath (Resolve-VmrIssueStorePath -Path $Path)
    if ($null -eq $model) {
        return [pscustomobject][ordered]@{ schemaVersion = 0; updatedAtUtc = $null; issues = @(); aliases = @(); occurrences = @(); statusHistory = @() }
    }
    $view = Get-VmrIssueStoreSortedView -Model $model
    $copies = @{}
    foreach ($name in @('Issues', 'Aliases', 'Occurrences', 'History')) {
        $list = [Collections.Generic.List[object]]::new()
        foreach ($record in $view[$name]) { $list.Add([pscustomobject]$record) }
        $copies[$name] = $list.ToArray()
    }
    return [pscustomobject][ordered]@{
        schemaVersion = $model.SchemaVersion
        updatedAtUtc  = $model.UpdatedAtUtc
        issues        = $copies['Issues']
        aliases       = $copies['Aliases']
        occurrences   = $copies['Occurrences']
        statusHistory = $copies['History']
    }
}

function Export-VmrIssueHistory {
    param(
        [Parameter(Mandatory)][string] $Path,
        [Parameter(Mandatory)][string] $OutputPath
    )
    # A separate JSON file rather than a copy of the store keeps a handoff explicit and normalised (an old
    # schema is written out as the current one) and stamps when it was exported.
    $model = Get-VmrIssueStoreModel -FullPath (Resolve-VmrIssueStorePath -Path $Path)
    if ($null -eq $model) { throw "Issue history store not found: $Path" }
    $bytes = ConvertTo-VmrIssueStoreBytes -Model $model -ExportedAtUtc ([DateTime]::UtcNow.ToString('o'))
    $outputFull = Resolve-VmrIssueStorePath -Path $OutputPath
    $directory = [IO.Path]::GetDirectoryName($outputFull)
    if ($directory -and -not [IO.Directory]::Exists($directory)) { [void][IO.Directory]::CreateDirectory($directory) }
    [void](Write-VmrIssueStoreFile -FullPath $outputFull -Bytes $bytes)
    $counts = Get-VmrIssueStoreCounts -Model $model
    return [pscustomobject]@{
        path = $OutputPath; schemaVersion = $script:VmrIssueStoreSchema
        issues = $counts.Issues; aliases = $counts.Aliases
        occurrences = $counts.Occurrences; statusEntries = $counts.StatusEntries
    }
}

function Get-VmrIssueStoreHistoryKey {
    param($Entry)
    return [string]::Join([string][char]31, @([string]$Entry['slug'], [string]$Entry['changed_on'], [string]$Entry['status'], [string]$Entry['note'], [string]$Entry['source']))
}

function Merge-VmrIssueStoreModel {
    # Additive, idempotent and non-destructive, so a QB can merge a colleague's export without losing
    # their own recorded work. Conflict rules:
    #   issues       union; first_seen=min, last_seen=max; owner/tracker/resolution/notes: the local
    #                non-blank value wins, except that a local daily-run owner/tracker yields to an
    #                incoming operator-recorded one
    #   aliases      inserted if absent; then any issue living under an alias slug is folded into its
    #                canonical issue (otherwise it would be orphaned and unreachable by name)
    #   occurrences  keyed (slug, report_date): the EXISTING row wins. The original replaced it with
    #                the incoming one; an import must not overwrite what this QB recorded.
    #   history      deduplicated on (slug, changed_on, status, note, source), then appended
    #   status       an existing issue that received new history takes the status of its latest entry
    #                (by changed_on; ties go to the later one), so a colleague's more advanced status can
    #                arrive while the audit trail stays append-only. A newer local status is kept
    #                because it is the latest entry. New issues keep the status they arrived with.
    param($Local, $Incoming)
    $ordinal = [StringComparer]::Ordinal
    $added = [Collections.Generic.HashSet[string]]::new($ordinal)
    $arrived = [Collections.Generic.HashSet[string]]::new($ordinal)

    foreach ($slug in (Get-VmrIssueStoreSortedKeys -Dictionary $Incoming.Issues)) {
        $source = $Incoming.Issues[$slug]
        if ($Local.Issues.ContainsKey($slug)) {
            Merge-VmrIssueStoreRecord -Model $Local -Target $Local.Issues[$slug] -Other $source
        }
        else {
            $Local.Issues[$slug] = Copy-VmrIssueStoreRecord -Record $source
            [void]$added.Add($slug)
            $Local.Dirty = $true
        }
    }

    foreach ($slug in (Get-VmrIssueStoreSortedKeys -Dictionary $Incoming.Occurrences)) {
        $rows = $null
        if (-not $Local.Occurrences.TryGetValue($slug, [ref]$rows)) {
            $rows = [Collections.Generic.List[object]]::new()
            $Local.Occurrences[$slug] = $rows
        }
        foreach ($row in $Incoming.Occurrences[$slug]) {
            $exists = $false
            foreach ($candidate in $rows) { if ($candidate['report_date'] -ceq $row['report_date']) { $exists = $true; break } }
            if (-not $exists) {
                $rows.Add((Copy-VmrIssueStoreRecord -Record $row))
                $Local.Dirty = $true
            }
        }
    }

    $known = [Collections.Generic.HashSet[string]]::new($ordinal)
    foreach ($list in $Local.History.Values) { foreach ($entry in $list) { [void]$known.Add((Get-VmrIssueStoreHistoryKey -Entry $entry)) } }
    foreach ($slug in (Get-VmrIssueStoreSortedKeys -Dictionary $Incoming.History)) {
        $entries = $null
        if (-not $Local.History.TryGetValue($slug, [ref]$entries)) {
            $entries = [Collections.Generic.List[object]]::new()
            $Local.History[$slug] = $entries
        }
        foreach ($entry in $Incoming.History[$slug]) {
            if ($known.Add((Get-VmrIssueStoreHistoryKey -Entry $entry))) {
                $entries.Add((Copy-VmrIssueStoreRecord -Record $entry))
                [void]$arrived.Add($slug)
                $Local.Dirty = $true
            }
        }
    }

    foreach ($aliasName in (Get-VmrIssueStoreSortedKeys -Dictionary $Incoming.Aliases)) {
        if (-not $Local.Aliases.ContainsKey($aliasName)) {
            $Local.Aliases[$aliasName] = Copy-VmrIssueStoreRecord -Record $Incoming.Aliases[$aliasName]
            $Local.Dirty = $true
        }
    }
    foreach ($aliasName in (Get-VmrIssueStoreSortedKeys -Dictionary $Local.Aliases)) {
        $row = $Local.Aliases[$aliasName]
        $canonical = Resolve-VmrIssueSlugValue -Model $Local -Slug $row['slug']
        # Keep aliases flat: an alias never points at another alias.
        if ($canonical -cne $row['slug'] -and $canonical -cne $aliasName) {
            $row['slug'] = $canonical
            $Local.Dirty = $true
        }
        if ($canonical -cne $aliasName -and $Local.Issues.ContainsKey($aliasName)) {
            $mode = Merge-VmrIssueStoreIntoCanonical -Model $Local -AliasSlug $aliasName -Canonical $canonical -CanonicalName $null
            if ($arrived.Contains($aliasName)) { [void]$arrived.Add($canonical) }
            if ($mode -eq 'renamed' -and $added.Contains($aliasName)) { [void]$added.Add($canonical) }
        }
    }

    $arrivedSlugs = [string[]]::new($arrived.Count)
    $arrived.CopyTo($arrivedSlugs)
    [Array]::Sort($arrivedSlugs, $ordinal)
    foreach ($slug in $arrivedSlugs) {
        if ($added.Contains($slug) -or -not $Local.Issues.ContainsKey($slug)) { continue }
        $latest = $null
        foreach ($entry in $Local.History[$slug]) {
            if ($null -eq $latest -or [string]::CompareOrdinal([string]$entry['changed_on'], [string]$latest['changed_on']) -ge 0) { $latest = $entry }
        }
        if ($null -ne $latest) { [void](Set-VmrIssueStoreField -Model $Local -Record $Local.Issues[$slug] -Field 'status' -Value $latest['status']) }
    }

    foreach ($issue in $Local.Issues.Values) { Update-VmrIssueStoreDerivedFields -Model $Local -Issue $issue -CountOnly }
}

function Import-VmrIssueHistory {
    param(
        [Parameter(Mandatory)][string] $Path,
        [Parameter(Mandatory)][string] $InputPath
    )
    if (-not (Test-Path -LiteralPath $InputPath -PathType Leaf)) { throw "History export not found: $InputPath" }
    $bytes = Read-VmrIssueStoreBytes -FullPath (Resolve-VmrIssueStorePath -Path $InputPath)
    # Schema 1 (the SQLite era) and 2 are accepted as they are; a newer one is refused here. This is
    # where a forward migration would go when the schema moves again.
    $incoming = ConvertFrom-VmrIssueStoreBytes -Bytes $bytes -Label "History export $InputPath" -Context 'export'

    $merged = Invoke-VmrIssueStoreTransaction -StorePath $Path -Create -Action {
        param($m)
        $null = Merge-VmrIssueStoreModel -Local $m -Incoming $incoming
        return (Get-VmrIssueStoreCounts -Model $m)
    }
    return [pscustomobject]@{
        issues = $merged.Issues; occurrences = $merged.Occurrences
        statusEntries = $merged.StatusEntries; schemaVersion = $incoming.SchemaVersion
    }
}

function Get-VmrIssueStoreField {
    param([AllowNull()][object] $Object, [string] $Name, [AllowNull()][object] $Default = $null)
    if ($null -eq $Object) { return $Default }
    if ($Object -is [System.Collections.IDictionary]) {
        if ($Object.Contains($Name) -and $null -ne $Object[$Name]) { return $Object[$Name] }
        return $Default
    }
    $property = $Object.PSObject.Properties[$Name]
    if ($null -ne $property -and $null -ne $property.Value) { return $property.Value }
    return $Default
}

function Get-VmrKnownTrackerMap {
    param([Parameter(Mandatory)][string] $Path)
    # findingId / title -> tracker URL, for everything the store already knows is tracked or owned.
    $map = @{}
    if (-not (Test-Path -LiteralPath $Path)) { return $map }
    $rows = Get-VmrIssueHistory -Path $Path -Limit 5000
    foreach ($row in $rows) {
        $tracker = [string]$row.tracker_url
        if (-not $tracker -and [string]$row.status -in @('owned', 'resolved', 'noise', 'wontfix')) {
            $tracker = "an operator-recorded $([string]$row.status) status"
        }
        if (-not $tracker) { continue }
        foreach ($key in @([string]$row.slug, [string]$row.name)) {
            if ($key -and -not $map.ContainsKey($key)) { $map[$key] = $tracker }
        }
    }
    return $map
}

function Format-VmrIssueHistoryContext {
    param([Parameter(Mandatory)][string] $Path, [int] $SinceDays = 45, [int] $Limit = 40)
    if (-not (Test-Path -LiteralPath $Path)) { return $null }
    $rows = Get-VmrIssueHistory -Path $Path -SinceDays $SinceDays -Limit $Limit
    if (-not $rows) { return $null }
    $lines = foreach ($row in $rows) {
        $span = if ($row.first_seen -eq $row.last_seen) { "seen once on $($row.first_seen)" } else { "$($row.occurrences)x between $($row.first_seen) and $($row.last_seen)" }
        $tracker = if ($row.tracker_url) { $row.tracker_url } else { 'no tracker' }
        $owner = if ($row.owner_team) { $row.owner_team } else { 'unrouted' }
        $resolution = if ($row.resolution) { " Resolution: $($row.resolution)" } else { '' }
        "- $($row.name) [$($row.status)] $span; owner $owner; $tracker.$resolution"
    }
    return ($lines -join "`n")
}
