Set-StrictMode -Version Latest

# common/routing-edit.ps1 - Reading, comparing and editing YOUR copy of the routing registry (local/knowledge/routing.local.json).
# The copy is made once from knowledge/routing.json and then belongs to you. This file is how you change it safely and how you
# take in a later change to the shipped file without losing your edits:
#   Get-VmrQBRoutingDiff            your copy against the shipped file (and, when a base snapshot exists, against what you copied)
#   Set-VmrQBLocalRoutingEntry      add, replace or remove one entry; the result must load before anything is written
#   Confirm-VmrQBLocalRoutingCurrent  "I have reviewed the differences": re-stamp the copy against today's shipped file
# Every write is validated by loading the merged result, then replaced atomically under a lock. Defines functions only.
. (Join-Path $PSScriptRoot 'core.ps1')

# How each section of the registry is keyed, so a difference can be reported and applied one entry at a time.
$script:VmrQBRoutingKeyedLists = @{ categories = 'id'; supportDestinations = 'id' }
$script:VmrQBRoutingMapSections = @('channels', 'channelRules', 'preferredRedirects', 'handleAliases', 'repoOwners')
$script:VmrQBRoutingScalarSections = @('vmrTestsOwner', 'unmappedLabel', 'verifiedOn')
$script:VmrQBRoutingStringLists = @('teams')
$script:VmrQBRoutingSections = @('categories', 'supportDestinations', 'channels', 'channelRules', 'preferredRedirects', 'handleAliases', 'repoOwners', 'teams', 'vmrTestsOwner', 'unmappedLabel', 'verifiedOn')

function ConvertTo-VmrQBCanonicalJson {
    <#
    .SYNOPSIS
    Compact JSON with object properties in name order, so two entries that differ only in property order compare equal.
    #>
    param([AllowNull()][object] $Value)
    $normalize = {
        param($item)
        if ($null -eq $item) { return $null }
        if ($item -is [string] -or $item -is [ValueType]) { return $item }
        if ($item -is [System.Collections.IDictionary]) {
            $ordered = [ordered]@{}
            foreach ($key in @($item.Keys | Sort-Object)) { $ordered[[string]$key] = & $normalize $item[$key] }
            return $ordered
        }
        if ($item -is [System.Collections.IEnumerable]) { return , @(foreach ($element in $item) { & $normalize $element }) }
        $ordered = [ordered]@{}
        foreach ($property in @($item.PSObject.Properties | Sort-Object Name)) { $ordered[$property.Name] = & $normalize $property.Value }
        return $ordered
    }
    return (ConvertTo-Json -InputObject (& $normalize $Value) -Depth 20 -Compress)
}

function Get-VmrQBRoutingEntries {
    <#
    .SYNOPSIS
    A routing document flattened to one entry per (section, key), each holding its canonical JSON.
    #>
    param([AllowNull()][object] $Document)
    $entries = [ordered]@{}
    if ($null -eq $Document) { return $entries }
    foreach ($section in $script:VmrQBRoutingSections) {
        $value = Get-VmrQBRoutingField $Document $section
        if ($null -eq $value) { continue }
        if ($script:VmrQBRoutingKeyedLists.ContainsKey($section)) {
            $keyName = $script:VmrQBRoutingKeyedLists[$section]
            foreach ($element in @($value)) { $entries["$section|$([string](Get-VmrQBRoutingField $element $keyName))"] = ConvertTo-VmrQBCanonicalJson $element }
        }
        elseif ($section -in $script:VmrQBRoutingMapSections) {
            foreach ($property in @($value.PSObject.Properties)) { $entries["$section|$($property.Name)"] = ConvertTo-VmrQBCanonicalJson $property.Value }
        }
        elseif ($section -in $script:VmrQBRoutingStringLists) {
            foreach ($element in @($value)) { $entries["$section|$([string]$element)"] = ConvertTo-VmrQBCanonicalJson ([string]$element) }
        }
        else { $entries["$section|$section"] = ConvertTo-VmrQBCanonicalJson ([string]$value) }
    }
    return $entries
}

function Get-VmrQBRoutingDiff {
    <#
    .SYNOPSIS
    Compares your routing copy with the shipped file, entry by entry.
    .DESCRIPTION
    With a base snapshot (the shipped text your copy was made from) each difference says who changed it:
      yours     only you changed it; keep it
      upstream  only the shipped file changed it and you did not; safe to adopt
      both      you and the shipped file changed it differently; review it
    Without a base snapshot (a copy made before snapshots existed) the difference cannot be attributed:
      differs, onlyLocal, onlyShipped
    Entries that are identical are not listed. Each result has Section, Key, State and the Local/Shipped/Base values as JSON.
    #>
    [CmdletBinding()]
    param([Parameter(Mandatory)][AllowNull()][object] $Local, [Parameter(Mandatory)][object] $Shipped, [AllowNull()][object] $Base)
    $localEntries = Get-VmrQBRoutingEntries $Local
    $shippedEntries = Get-VmrQBRoutingEntries $Shipped
    $baseEntries = Get-VmrQBRoutingEntries $Base
    $hasBase = $null -ne $Base
    $keys = [Collections.Generic.List[string]]::new()
    foreach ($source in @($shippedEntries, $localEntries, $baseEntries)) { foreach ($key in $source.Keys) { if (-not $keys.Contains($key)) { $keys.Add($key) } } }
    $result = [Collections.Generic.List[object]]::new()
    foreach ($key in $keys) {
        $l = if ($localEntries.Contains($key)) { $localEntries[$key] } else { $null }
        $s = if ($shippedEntries.Contains($key)) { $shippedEntries[$key] } else { $null }
        $b = if ($baseEntries.Contains($key)) { $baseEntries[$key] } else { $null }
        if ($l -ceq $s) { continue }
        $state = if ($hasBase) {
            if ($s -ceq $b) { 'yours' } elseif ($l -ceq $b) { 'upstream' } else { 'both' }
        }
        elseif ($null -eq $l) { 'onlyShipped' } elseif ($null -eq $s) { 'onlyLocal' } else { 'differs' }
        $section, $name = $key -split '\|', 2
        $result.Add([pscustomobject][ordered]@{ Section = $section; Key = $name; State = $state; Local = $l; Shipped = $s; Base = $b })
    }
    return $result.ToArray()
}

function Use-VmrQBRoutingLock {
    <#
    .SYNOPSIS
    Runs a script block while holding an exclusive lock on the local routing file, so two edits cannot overwrite each other.
    #>
    param([Parameter(Mandatory)][string] $Path, [Parameter(Mandatory)][scriptblock] $Body, [int] $TimeoutSeconds = 15)
    [void][IO.Directory]::CreateDirectory((Split-Path -Parent $Path))
    $deadline = [DateTime]::UtcNow.AddSeconds($TimeoutSeconds)
    $stream = $null
    while ($null -eq $stream) {
        try { $stream = [IO.FileStream]::new("$Path.lock", [IO.FileMode]::OpenOrCreate, [IO.FileAccess]::ReadWrite, [IO.FileShare]::None) }
        catch {
            # A constructor failure arrives wrapped; exactly an IOException is "someone else holds it".
            $inner = if ($null -ne $_.Exception.InnerException) { $_.Exception.InnerException } else { $_.Exception }
            if ($inner.GetType() -ne [IO.IOException]) { throw }
            if ([DateTime]::UtcNow -ge $deadline) { throw "Timed out after $TimeoutSeconds seconds waiting for another process to finish editing $Path." }
            Start-Sleep -Milliseconds 25
        }
    }
    try { & $Body }
    finally { $stream.Dispose() }
}

function Test-VmrQBRoutingDocument {
    <#
    .SYNOPSIS
    Returns $null when the merged result (shipped + this document) loads as routing, otherwise the loader's message.
    #>
    param(
        [Parameter(Mandatory)][object] $Document,
        [Parameter(Mandatory)][string] $LocalPath,
        [Parameter(Mandatory)][string] $ShippedPath,
        [string] $GuidePath
    )
    # In the system temp folder, so checking a document never needs (or creates) the local folder.
    $candidate = Join-Path ([IO.Path]::GetTempPath()) ("vmrqb-routing-check-$([guid]::NewGuid().ToString('N')).json")
    try {
        [IO.File]::WriteAllText($candidate, ((ConvertTo-Json -InputObject $Document -Depth 20) -replace "`r`n", "`n") + "`n", [Text.UTF8Encoding]::new($false))
        $arguments = @{ Path = $ShippedPath; LocalPath = $candidate }
        if ($GuidePath) { $arguments.GuidePath = $GuidePath }
        [void](Get-VmrQBRouting @arguments -Strict)
        return $null
    }
    catch { return $_.Exception.Message }
    finally { if ([IO.File]::Exists($candidate)) { try { [IO.File]::Delete($candidate) } catch { Write-Verbose "Could not remove $candidate" } } }
}

function Write-VmrQBLocalRoutingDocument {
    <#
    .SYNOPSIS
    Saves a modified local routing document only if the merged result still loads; the replace is atomic.
    #>
    param(
        [Parameter(Mandatory)][object] $Document,
        [Parameter(Mandatory)][string] $LocalPath,
        [Parameter(Mandatory)][string] $ShippedPath,
        [string] $GuidePath
    )
    $problem = Test-VmrQBRoutingDocument -Document $Document -LocalPath $LocalPath -ShippedPath $ShippedPath -GuidePath $GuidePath
    if ($problem) { throw "Not saved. The result would not load as routing: $problem" }
    $temporary = "$LocalPath.$([guid]::NewGuid().ToString('N')).tmp"
    try {
        [IO.File]::WriteAllText($temporary, ((ConvertTo-Json -InputObject $Document -Depth 20) -replace "`r`n", "`n") + "`n", [Text.UTF8Encoding]::new($false))
        [IO.File]::Move($temporary, $LocalPath, $true)
    }
    finally { if ([IO.File]::Exists($temporary)) { try { [IO.File]::Delete($temporary) } catch { Write-Verbose "Could not remove $temporary" } } }
}

function Update-VmrQBRoutingDocument {
    <#
    .SYNOPSIS
    Applies one entry to a routing document in memory (add, replace or remove). Nothing is validated or written here.
    #>
    param(
        [Parameter(Mandatory)][object] $Document,
        [Parameter(Mandatory)][string] $Section,
        [Parameter(Mandatory)][string] $Key,
        [AllowNull()][object] $Value,
        [switch] $Remove
    )
    $existing = $Document.PSObject.Properties[$Section]
    if ($script:VmrQBRoutingKeyedLists.ContainsKey($Section)) {
        $keyName = $script:VmrQBRoutingKeyedLists[$Section]
        $kept = @(@(if ($existing) { $existing.Value }) | Where-Object { [string](Get-VmrQBRoutingField $_ $keyName) -ne $Key })
        if (-not $Remove) {
            $entry = if ($Value -is [System.Collections.IDictionary]) { [pscustomobject]$Value } else { $Value }
            if ([string](Get-VmrQBRoutingField $entry $keyName) -ne $Key) { Add-Member -InputObject $entry -NotePropertyName $keyName -NotePropertyValue $Key -Force }
            $kept += $entry
        }
        $newValue = @($kept)
    }
    elseif ($Section -in $script:VmrQBRoutingMapSections) {
        $map = [ordered]@{}
        if ($existing -and $existing.Value) { foreach ($property in $existing.Value.PSObject.Properties) { if ($property.Name -ne $Key) { $map[$property.Name] = $property.Value } } }
        if (-not $Remove) { $map[$Key] = [string]$Value }
        $newValue = [pscustomobject]$map
    }
    elseif ($Section -in $script:VmrQBRoutingStringLists) {
        $kept = @(@(if ($existing) { $existing.Value }) | Where-Object { [string]$_ -ine $Key })
        if (-not $Remove) { $kept += [string]$Value }
        $newValue = @($kept)
    }
    else { $newValue = if ($Remove) { $null } else { [string]$Value } }
    if ($Remove -and $script:VmrQBRoutingScalarSections -contains $Section) { if ($existing) { $Document.PSObject.Properties.Remove($Section) }; return }
    if ($existing) { $Document.$Section = $newValue } else { Add-Member -InputObject $Document -NotePropertyName $Section -NotePropertyValue $newValue }
}

function Set-VmrQBLocalRoutingEntry {
    <#
    .SYNOPSIS
    Adds, replaces or removes one entry in your local routing copy.
    .DESCRIPTION
    Section is one of categories, supportDestinations, channels, channelRules, preferredRedirects, handleAliases, repoOwners,
    teams, vmrTestsOwner, unmappedLabel, verifiedOn. Key is the id (categories, supportDestinations), the property name
    (the maps), the handle (teams) or the section name (the scalars). Value is the entry: an object for a category or a
    support destination, a string otherwise. The merged result must load; if it would not, nothing is written. The copy
    is created first (once) if it does not exist yet.
    #>
    [CmdletBinding(SupportsShouldProcess)]
    param(
        [Parameter(Mandatory)][ValidateSet('categories', 'supportDestinations', 'channels', 'channelRules', 'preferredRedirects', 'handleAliases', 'repoOwners', 'teams', 'vmrTestsOwner', 'unmappedLabel', 'verifiedOn')][string] $Section,
        [Parameter(Mandatory)][string] $Key,
        [AllowNull()][object] $Value,
        [switch] $Remove,
        [string] $LocalPath = (Get-VmrQBPaths).LocalRoutingPath,
        [string] $ShippedPath = (Join-Path (Get-VmrQBPaths).KnowledgeRoot 'routing.json'),
        [string] $GuidePath
    )
    if (-not $Remove -and $null -eq $Value) { throw 'A value is required unless -Remove is given.' }
    if (-not $PSCmdlet.ShouldProcess($LocalPath, "$(if ($Remove) { 'Remove' } else { 'Set' }) $Section '$Key'")) { return }
    [void](Initialize-VmrQBLocalRouting -LocalPath $LocalPath -ShippedPath $ShippedPath)
    Use-VmrQBRoutingLock -Path $LocalPath -Body {
        $document = Read-VmrQBRoutingFile -Path $LocalPath -Label 'Local'
        Update-VmrQBRoutingDocument -Document $document -Section $Section -Key $Key -Value $Value -Remove:$Remove
        Write-VmrQBLocalRoutingDocument -Document $document -LocalPath $LocalPath -ShippedPath $ShippedPath -GuidePath $GuidePath
    }
}

function Confirm-VmrQBLocalRoutingCurrent {
    <#
    .SYNOPSIS
    Declares that you have reviewed the differences from the shipped file: restamps your copy and refreshes the base snapshot.
    .DESCRIPTION
    Your edits are kept exactly as they are. Afterwards ./daily/Invoke-Preflight.ps1 stops warning that the shipped file
    changed, and the next ./common/Invoke-Routing.ps1 diff compares against today's shipped file as the new base.
    #>
    [CmdletBinding(SupportsShouldProcess)]
    param(
        [string] $LocalPath = (Get-VmrQBPaths).LocalRoutingPath,
        [string] $ShippedPath = (Join-Path (Get-VmrQBPaths).KnowledgeRoot 'routing.json'),
        [string] $BasePath = (Join-Path (Split-Path -Parent $LocalPath) 'routing.base.json'),
        [string] $GuidePath
    )
    if (-not $PSCmdlet.ShouldProcess($LocalPath, 'Restamp against the current shipped routing.json')) { return }
    [void](Initialize-VmrQBLocalRouting -LocalPath $LocalPath -ShippedPath $ShippedPath -BasePath $BasePath)
    Use-VmrQBRoutingLock -Path $LocalPath -Body {
        $document = Read-VmrQBRoutingFile -Path $LocalPath -Label 'Local'
        $hash = Get-VmrQBShippedRoutingHash -Path $ShippedPath
        if ($document.PSObject.Properties['baseSha256']) { $document.baseSha256 = $hash } else { Add-Member -InputObject $document -NotePropertyName baseSha256 -NotePropertyValue $hash }
        Write-VmrQBLocalRoutingDocument -Document $document -LocalPath $LocalPath -ShippedPath $ShippedPath -GuidePath $GuidePath
        $baseTemp = "$BasePath.$([guid]::NewGuid().ToString('N')).tmp"
        try { [IO.File]::Copy($ShippedPath, $baseTemp, $true); [IO.File]::Move($baseTemp, $BasePath, $true) }
        finally { if ([IO.File]::Exists($baseTemp)) { try { [IO.File]::Delete($baseTemp) } catch { Write-Verbose "Could not remove $baseTemp" } } }
    }
}

function Get-VmrQBRoutingExport {
    <#
    .SYNOPSIS
    The routing a QB added or changed, as a versioned section for a handoff file. Only this QB's own input, never the whole file.
    .DESCRIPTION
    Each change is { section, key, value, base }: value is the entry as the QB has it, base is the shipped entry it was changed from
    (null for an addition), so an importer can tell whether it is starting from the same place. With a base snapshot the changes are
    the entries the QB changed or added. Without one, only entries that do not exist in the shipped file are exported, because an
    entry that merely differs could be an older shipped value, and exporting that would push stale routing onto the next QB.
    Nothing machine-specific is exported: no stamp, no path, no settings. Returns $null when there is no local copy.
    #>
    [CmdletBinding()]
    param(
        [string] $LocalPath = (Get-VmrQBPaths).LocalRoutingPath,
        [string] $ShippedPath = (Join-Path (Get-VmrQBPaths).KnowledgeRoot 'routing.json'),
        [string] $BasePath = (Join-Path (Split-Path -Parent $LocalPath) 'routing.base.json')
    )
    if (-not (Test-Path -LiteralPath $LocalPath -PathType Leaf)) { return $null }
    $local = Read-VmrQBRoutingFile -Path $LocalPath -Label 'Local'
    $shipped = Read-VmrQBRoutingFile -Path $ShippedPath -Label 'Shipped'
    $base = if (Test-Path -LiteralPath $BasePath -PathType Leaf) { Read-VmrQBRoutingFile -Path $BasePath -Label 'Base' } else { $null }
    $diff = @(Get-VmrQBRoutingDiff -Local $local -Shipped $shipped -Base $base | Where-Object { $_.Section -ne 'verifiedOn' -and $null -ne $_.Local })
    $own = @($diff | Where-Object { if ($null -ne $base) { $_.State -in @('yours', 'both') } else { $_.State -eq 'onlyLocal' } })
    $changes = @(foreach ($difference in $own) {
            [ordered]@{
                section = $difference.Section; key = $difference.Key
                value = ($difference.Local | ConvertFrom-Json -Depth 20)
                base = $(if ($null -ne $difference.Base) { $difference.Base | ConvertFrom-Json -Depth 20 } else { $null })
            }
        })
    return [ordered]@{
        schemaVersion = 1
        changes = $changes
        notExported = @($diff | Where-Object { $own -notcontains $_ }).Count
    }
}

function Import-VmrQBRoutingChanges {
    <#
    .SYNOPSIS
    Takes routing input from elsewhere (a handoff) into your local routing, keeping yours on conflict.
    .DESCRIPTION
    Each incoming change is { section, key, value }. It is applied only when that is safe:
      - you have no entry for that key                  -> added
      - your entry is identical                          -> unchanged
      - your entry is still exactly the shipped entry    -> replaced (you never changed it)
      - you changed it, differently                      -> kept, reported as a conflict
      - the sender started from a different shipped entry than yours (a change carries the entry it was made from) -> kept, conflict
      - it carries an email address or a file path                      -> invalid (one QB's personal data is not imported)
      - the result would not load here (for example it names a team this checkout's Guide does not have) -> invalid, with the reason
    Returns one result per change: Section, Key, Result, Detail. -Preview reports the same results and writes nothing.
    All applied changes are saved together, or none are.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][AllowEmptyCollection()][object[]] $Changes,
        [switch] $Preview,
        [string] $LocalPath = (Get-VmrQBPaths).LocalRoutingPath,
        [string] $ShippedPath = (Join-Path (Get-VmrQBPaths).KnowledgeRoot 'routing.json'),
        [string] $GuidePath
    )
    $results = [Collections.Generic.List[object]]::new()
    if ($Changes.Count -eq 0) { return @() }
    if (-not $Preview) { [void](Initialize-VmrQBLocalRouting -LocalPath $LocalPath -ShippedPath $ShippedPath) }
    $apply = {
        param($document)
        $shipped = Read-VmrQBRoutingFile -Path $ShippedPath -Label 'Shipped'
        $localEntries = Get-VmrQBRoutingEntries $document
        $shippedEntries = Get-VmrQBRoutingEntries $shipped
        $changed = $false
        foreach ($change in $Changes) {
            $section = [string](Get-VmrQBRoutingField $change 'section')
            $key = [string](Get-VmrQBRoutingField $change 'key')
            $value = Get-VmrQBRoutingField $change 'value'
            if ($section -notin $script:VmrQBRoutingSections -or -not $key -or $null -eq $value) {
                $results.Add([pscustomobject]@{ Section = $section; Key = $key; Result = 'invalid'; Detail = 'An entry needs a known section, a key and a value.' }); continue
            }
            $flat = "$section|$key"
            $mine = if ($localEntries.Contains($flat)) { $localEntries[$flat] } else { $null }
            $upstream = if ($shippedEntries.Contains($flat)) { $shippedEntries[$flat] } else { $null }
            if ($mine -ceq (ConvertTo-VmrQBCanonicalJson $value)) { $results.Add([pscustomobject]@{ Section = $section; Key = $key; Result = 'unchanged'; Detail = '' }); continue }
            $incomingJson = ConvertTo-Json -InputObject $value -Depth 20 -Compress
            if ($incomingJson -match '[A-Za-z0-9._%+-]+@[A-Za-z0-9-]+\.[A-Za-z]{2,}|\b[A-Za-z]:\\|\\Users\\') {
                $results.Add([pscustomobject]@{ Section = $section; Key = $key; Result = 'invalid'; Detail = 'It carries an email address or a file path, which is one QB''s personal data and is not imported.' }); continue
            }
            if ($null -ne $mine -and $mine -cne $upstream) { $results.Add([pscustomobject]@{ Section = $section; Key = $key; Result = 'conflict'; Detail = 'Your entry differs from the shipped one, so yours is kept.' }); continue }
            $hasBase = $null -ne $change.PSObject.Properties['base']
            if ($null -ne $mine -and $hasBase) {
                $incomingBase = $change.base
                $baseJson = if ($null -eq $incomingBase) { $null } else { ConvertTo-VmrQBCanonicalJson $incomingBase }
                if ($baseJson -cne $upstream) {
                    $results.Add([pscustomobject]@{ Section = $section; Key = $key; Result = 'conflict'; Detail = 'The sender changed a different version of the shipped entry than the one you have, so yours is kept; compare them before replacing.' }); continue
                }
            }
            $trial = (ConvertTo-Json -InputObject $document -Depth 20) | ConvertFrom-Json -Depth 20
            Update-VmrQBRoutingDocument -Document $trial -Section $section -Key $key -Value (($value | ConvertTo-Json -Depth 20) | ConvertFrom-Json -Depth 20)
            $problem = Test-VmrQBRoutingDocument -Document $trial -LocalPath $LocalPath -ShippedPath $ShippedPath -GuidePath $GuidePath
            if ($problem) { $results.Add([pscustomobject]@{ Section = $section; Key = $key; Result = 'invalid'; Detail = $problem }); continue }
            $results.Add([pscustomobject]@{ Section = $section; Key = $key; Result = $(if ($null -eq $mine) { 'added' } else { 'replaced' }); Detail = '' })
            $document = $trial
            $localEntries = Get-VmrQBRoutingEntries $document
            $changed = $true
        }
        return [pscustomobject]@{ Document = $document; Changed = $changed }
    }
    if ($Preview) {
        $current = if (Test-Path -LiteralPath $LocalPath -PathType Leaf) { Read-VmrQBRoutingFile -Path $LocalPath -Label 'Local' } else { Read-VmrQBRoutingFile -Path $ShippedPath -Label 'Shipped' }
        [void](& $apply $current)
    }
    else {
        Use-VmrQBRoutingLock -Path $LocalPath -Body {
            $outcome = & $apply (Read-VmrQBRoutingFile -Path $LocalPath -Label 'Local')
            if ($outcome.Changed) { Write-VmrQBLocalRoutingDocument -Document $outcome.Document -LocalPath $LocalPath -ShippedPath $ShippedPath -GuidePath $GuidePath }
        }
    }
    return $results.ToArray()
}
