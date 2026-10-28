Set-StrictMode -Version Latest

# daily/notes.ps1 - Run notes: everything that was stale, missing, wrong or failed during a run but did not stop it.
# A run never stops for these. They are collected as they happen, written to the run log, shown at the top of the overall
# summary, and each one says how to fix it. A note with severity 'first' (internal authentication,
# missing evidence) is listed before the others and drawn in red.
# Defines functions only; loading it runs nothing.

$script:QBRunNotes = [Collections.Generic.List[object]]::new()
$script:QBRunNotesBegin = '<!-- VMR-QB-RUN-NOTES -->'
$script:QBRunNotesEnd = '<!-- /VMR-QB-RUN-NOTES -->'
$script:QBRunNotesShown = 12

function Clear-QBRunNotes { $script:QBRunNotes.Clear() }

function Format-QBNoteText {
    # One line, no angle brackets (they would be read as HTML in the report), bounded.
    param([AllowNull()][string] $Text, [int] $Limit = 400)
    $line = ([regex]::Replace([string]$Text, '\s+', ' ')).Trim().Replace('<', '(').Replace('>', ')')
    if ($line.Length -gt $Limit) { $line = $line.Substring(0, $Limit - 3) + '...' }
    return $line
}

function Add-QBRunNote {
    <#
    .SYNOPSIS
    Records something that was wrong or missing and did not stop the run.
    .PARAMETER Key
    Notes with the same key are recorded once (for example every report about the same internal sign-in failure).
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][string] $Message,
        [string] $Source = 'run',
        [string] $Fix = '',
        [ValidateSet('first', 'note')][string] $Severity = 'note',
        [string] $Key
    )
    $text = Format-QBNoteText $Message
    if (-not $text) { return }
    if (-not $Key) { $Key = "$Source|$text" }
    foreach ($existing in $script:QBRunNotes) { if ($existing.key -eq $Key) { return } }
    $script:QBRunNotes.Add([pscustomobject]@{
            key = $Key; severity = $Severity; source = $Source; message = $text; fix = (Format-QBNoteText $Fix 300)
        })
}

function Get-QBRunNotes {
    # Severity 'first' notes lead; within a severity the order they happened in is kept.
    $all = @($script:QBRunNotes)
    return @(@($all | Where-Object severity -eq 'first') + @($all | Where-Object severity -ne 'first'))
}

function Invoke-QBOptional {
    <#
    .SYNOPSIS
    Runs an optional step. If it throws, the run goes on with -Default and a note says what was not used and how to fix it.
    .DESCRIPTION
    For inputs the round can do without (the routing registry, the owner table): a damaged or unreadable one is
    reported, never a reason to lose the report. The file itself is left untouched.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][string] $Name,
        [Parameter(Mandatory)][scriptblock] $Script,
        [AllowNull()][object] $Default = $null,
        [string] $Fix = '',
        [ValidateSet('first', 'note')][string] $Severity = 'note'
    )
    try { return (& $Script) }
    catch {
        Add-QBRunNote -Source $Name -Message "$Name could not be used and was skipped for this run: $($_.Exception.Message)" -Fix $Fix -Severity $Severity
        return $Default
    }
}

function Format-QBRunNotesMarkdown {
    [CmdletBinding()]
    param([AllowEmptyCollection()][object[]] $Notes = (Get-QBRunNotes))
    # An empty default arrives as $null, which @() would count as one note.
    $notes = @($Notes | Where-Object { $null -ne $_ })
    if (-not $notes.Count) { return '' }
    $lines = [Collections.Generic.List[string]]::new()
    $lines.Add($script:QBRunNotesBegin)
    $lines.Add("> **Run notes ($($notes.Count))** - the report was still produced. Something was stale, missing or failed:")
    foreach ($note in @($notes | Select-Object -First $script:QBRunNotesShown)) {
        $fix = if ($note.fix) { " *Fix:* $($note.fix)" } else { '' }
        $lines.Add("> - $($note.message)$fix")
    }
    if ($notes.Count -gt $script:QBRunNotesShown) { $lines.Add("> - ... and $($notes.Count - $script:QBRunNotesShown) more in the run log.") }
    $lines.Add($script:QBRunNotesEnd)
    return ($lines -join "`n")
}

function Remove-QBRunNotesBlock {
    # Removes runner-owned notes from the report Markdown.
    [CmdletBinding()]
    param([AllowEmptyString()][string] $Content)
    $begin = [regex]::Escape($script:QBRunNotesBegin); $end = [regex]::Escape($script:QBRunNotesEnd)
    $text = [regex]::Replace($Content, "(?s)(?:\r?\n){1,2}$begin.*?$end", '')
    return [regex]::Replace($text, "(?s)^$begin.*?$end(?:\r?\n){0,2}", '')
}

function Set-QBRunNotesBlock {
    <#
    .SYNOPSIS
    Puts the run notes at the top of a report, directly under its title (replacing an earlier block). No notes, no block.
    #>
    [CmdletBinding()]
    param([Parameter(Mandatory)][AllowEmptyString()][string] $Content, [AllowEmptyCollection()][object[]] $Notes = (Get-QBRunNotes))
    $clean = Remove-QBRunNotesBlock -Content $Content
    $block = Format-QBRunNotesMarkdown -Notes $Notes
    if (-not $block) { return $clean }
    $title = [regex]::Match($clean, '(?m)^#[ \t]+\S.*$')
    if ($title.Success) { return $clean.Insert($title.Index + $title.Length, "`n`n$block") }
    return $block + "`n`n" + $clean
}
