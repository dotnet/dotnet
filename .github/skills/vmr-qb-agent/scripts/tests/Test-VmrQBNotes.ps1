[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# Run notes: everything that is stale, missing or failed but did not stop the run is recorded with its fix, listed first when it is
# internal authentication, and shown at the top of the overall summary and the email. A damaged optional input is skipped with a
# note, and a bad config only costs the email.
$scripts = Split-Path -Parent $PSScriptRoot
. (Join-Path $scripts 'common\core.ps1')
. (Join-Path $scripts 'history\history.ps1')
. (Join-Path $scripts 'daily\contract.ps1')

$failures = 0
function Check([string] $Name, [scriptblock] $Body) {
    try { & $Body; Write-Host "PASS: $Name" -ForegroundColor Green }
    catch { $script:failures++; Write-Host "FAIL: $Name -> $($_.Exception.Message)" -ForegroundColor Red }
}
function Assert($Condition, [string] $Message) { if (-not $Condition) { throw $Message } }
function Assert-Throws-Strict([string] $Path) {
    # The strict reader still refuses: Set-VmrQBConfig and the setup script rely on it.
    Set-Content -LiteralPath $Path -Value '{ "recipient": "nope" }'
    try { [void](Get-VmrQBConfig -Path $Path) } catch { return }
    throw 'Get-VmrQBConfig must still refuse an invalid address'
}

$sandbox = Join-Path ([IO.Path]::GetTempPath()) ('vmrqb-notes-' + [guid]::NewGuid().ToString('N'))
[void][IO.Directory]::CreateDirectory($sandbox)

try {
    Check 'A note is recorded once per key, on one line without angle brackets, and internal authentication is listed first' {
        Clear-QBRunNotes
        Add-QBRunNote -Source 'a' -Message "Line one`nline two <script>x</script>" -Fix 'Do <this>'
        Add-QBRunNote -Source 'a' -Message "Line one`nline two <script>x</script>" -Fix 'Do <this>'
        Add-QBRunNote -Source 'b' -Message 'Second note'
        Add-QBRunNote -Source 'internal-auth' -Key 'internal-auth' -Severity first -Message 'The internal builds are UNKNOWN' -Fix 'az login --allow-no-subscriptions'
        Add-QBRunNote -Source 'internal-auth' -Key 'internal-auth' -Severity first -Message 'A second sentence about the same sign-in problem'
        $notes = @(Get-QBRunNotes)
        Assert ($notes.Count -eq 3) "same key or same text is recorded once: $($notes.Count)"
        Assert ($notes[0].source -eq 'internal-auth' -and $notes[0].fix -eq 'az login --allow-no-subscriptions') 'the first-severity note leads and keeps the first message'
        Assert ($notes[1].message -notmatch "[`r`n<>]" -and $notes[1].message -match 'line two') 'text is one line without angle brackets'
        Add-QBRunNote -Source 'long' -Message ('x' * 900)
        Assert ((@(Get-QBRunNotes | Where-Object source -eq 'long')[0].message.Length) -le 400) 'a note is bounded'
        Clear-QBRunNotes
        Assert (@(Get-QBRunNotes).Count -eq 0) 'Clear-QBRunNotes empties the list'
    }

    Check 'An optional step that fails is skipped with a note and a default; one that works returns its value untouched' {
        Clear-QBRunNotes
        $value = Invoke-QBOptional -Name 'Fine input' -Default 'fallback' -Script { 'real value' }
        Assert ($value -eq 'real value' -and @(Get-QBRunNotes).Count -eq 0) 'a working step is untouched'
        $value = Invoke-QBOptional -Name 'Broken input' -Default 'fallback' -Fix 'Repair the file.' -Script { throw 'it is not valid JSON' }
        $notes = @(Get-QBRunNotes)
        Assert ($value -eq 'fallback') 'the default stands in'
        Assert ($notes.Count -eq 1 -and $notes[0].message -match 'Broken input could not be used' -and $notes[0].message -match 'not valid JSON' -and $notes[0].fix -eq 'Repair the file.') 'the note names the input, the reason and the fix'
        $array = @(Invoke-QBOptional -Name 'List' -Default @() -Script { 'a'; 'b' })
        Assert ($array.Count -eq 2) 'a list result keeps its items'
        Clear-QBRunNotes
    }

    Check 'The notes block goes under the title, is replaced rather than repeated, comes out again exactly, and is absent when there are no notes' {
        Clear-QBRunNotes
        $report = "# VMR QB Daily Overall Report - 2026-10-09`n`n## Top actions for the QB today`n`nNo QB action required.`n"
        Assert ((Set-QBRunNotesBlock -Content $report) -ceq $report) 'no notes, no block'
        Add-QBRunNote -Source 'config' -Message 'The QB config is not valid JSON.' -Fix 'Fix the file.'
        $with = Set-QBRunNotesBlock -Content $report
        $lines = @($with -split "`n")
        Assert ($lines[0] -like '# VMR QB Daily Overall Report*' -and $lines[2] -eq '<!-- VMR-QB-RUN-NOTES -->') 'the block is right under the title'
        Assert ($with -match '> - The QB config is not valid JSON\. \*Fix:\* Fix the file\.' -and $with -match '> \*\*Run notes \(1\)\*\*') 'each note and its fix are listed'
        Add-QBRunNote -Source 'owners' -Message 'A second note.'
        $again = Set-QBRunNotesBlock -Content $with
        Assert (([regex]::Matches($again, 'VMR-QB-RUN-NOTES -->')).Count -eq 2) 'the block is replaced, not repeated (one begin and one end marker)'
        Assert ($again -match 'Run notes \(2\)') 'and shows both notes'
        Assert ((Remove-QBRunNotesBlock -Content $again) -ceq $report) 'removing the block gives back the model''s own text exactly'
        $untitled = Set-QBRunNotesBlock -Content "no title here`n"
        Assert ($untitled.StartsWith('<!-- VMR-QB-RUN-NOTES -->') -and (Remove-QBRunNotesBlock -Content $untitled) -ceq "no title here`n") 'without a title the block leads, and still comes out cleanly'
        Clear-QBRunNotes
    }

    Check 'Failed report checks become notes; no findings at all (null or empty) is no note and no error' {
        Clear-QBRunNotes
        Add-QBReportCheckNotes -Findings $null
        Add-QBReportCheckNotes -Findings @()
        Add-QBReportCheckNotes
        Assert (@(Get-QBRunNotes).Count -eq 0) 'nothing to note'
        Add-QBReportCheckNotes -Findings @([pscustomobject]@{ severity = 'warn'; message = 'advisory' }, [pscustomobject]@{ severity = 'fail'; message = 'a link is missing' })
        $notes = @(Get-QBRunNotes)
        Assert ($notes.Count -eq 1 -and $notes[0].source -eq 'report-check' -and $notes[0].message -eq 'Report check: a link is missing') 'only a failed check is noted'
        Clear-QBRunNotes
    }

    Check 'More than twelve notes are capped with a pointer to the run log' {
        Clear-QBRunNotes
        1..15 | ForEach-Object { Add-QBRunNote -Source 'many' -Message "Note number $_" }
        $block = Format-QBRunNotesMarkdown
        Assert (([regex]::Matches($block, '(?m)^> - Note number')).Count -eq 12 -and $block -match '3 more in the run log') 'twelve shown, the rest counted'
        Assert ((Format-QBRunNotesHtml) -match '3 more in the run log') 'and the same in the email'
        Clear-QBRunNotes
    }

    Check 'The email shows the notes in a box (red when internal authentication is among them), without the markdown block, and says when only a digest exists' {
        Clear-QBRunNotes
        Add-QBRunNote -Source 'owners' -Message 'Owner <b>x</b> is not a team & more.'
        $amber = Format-QBRunNotesHtml
        Assert ($amber -match '#c19c00' -and $amber -notmatch '#d13438' -and $amber -match 'not a team &amp; more') 'ordinary notes are amber and HTML-encoded'
        Add-QBRunNote -Source 'internal-auth' -Key 'internal-auth' -Severity first -Message 'The internal builds are UNKNOWN.' -Fix 'az login --allow-no-subscriptions'
        $red = Format-QBRunNotesHtml
        Assert ($red -match '#d13438' -and $red -match 'az login --allow-no-subscriptions') 'internal authentication is red and carries its fix'
        $report = Set-QBRunNotesBlock -Content "# VMR QB Daily Overall Report - 2026-10-09`n`n## Top actions for the QB today`n`nNo QB action required.`n"
        $result = [pscustomobject]@{ name = 'overall-summary'; status = 'succeeded'; path = 'x'; error = $null }
        $html = ConvertTo-QBHtmlBody -Date '2026-10-09' -RunStatus partial -Results @($result) -Attachments @('x') -OverallMarkdown $report -Notes @(Get-QBRunNotes)
        Assert ($html -match 'Run notes \(2\)' -and ([regex]::Matches($html, 'UNKNOWN')).Count -eq 1) 'the notes are in the email once (the box), not again from the markdown block'
        $digestHtml = ConvertTo-QBHtmlBody -Date '2026-10-09' -RunStatus failed -Results @() -Attachments @() -OverallMarkdown $null -Notes @() -DigestMarkdown "# Digest`n`n| a | b |`n|---|---|`n| 1 | 2 |`n"
        Assert ($digestHtml -match 'without the model' -and $digestHtml -match '<table') 'a digest takes the place of the missing overall report and says how it was made'
        $bare = ConvertTo-QBHtmlBody -Date '2026-10-09' -RunStatus failed -Results @() -Attachments @() -OverallMarkdown $null
        Assert ($bare -match 'was not produced') 'with neither, the email says so'
        Clear-QBRunNotes
    }

    Check 'A config that cannot be used only costs the email: the problem says how to fix it and the defaults stand in' {
        $path = Join-Path $sandbox 'config.json'
        $missing = Get-VmrQBConfigLenient -Path (Join-Path $sandbox 'nope.json')
        Assert (@($missing.Problems).Count -eq 0 -and $missing.Config.recipient -eq '') 'no file is a valid choice: email off, no problem'
        Set-Content -LiteralPath $path -Value '{ nope'
        $broken = Get-VmrQBConfigLenient -Path $path
        Assert (@($broken.Problems).Count -eq 1 -and $broken.Problems[0] -match 'not valid JSON' -and $broken.Problems[0] -match 'To get email' -and $broken.Config.recipient -eq '') 'bad JSON: defaults, no email, a fix'
        Set-Content -LiteralPath $path -Value '[1, 2]'
        Assert (@((Get-VmrQBConfigLenient -Path $path).Problems).Count -eq 1) 'JSON that is not an object is a problem too'
        Set-Content -LiteralPath $path -Value '{ "recipient": "nope", "timeZone": "Pacific Standard Time" }'
        $badRecipient = Get-VmrQBConfigLenient -Path $path
        Assert ($badRecipient.Config.recipient -eq '' -and $badRecipient.Problems[0] -match "'recipient'.*not an email address.*no email is sent" -and $badRecipient.Config.timeZone -eq 'Pacific Standard Time') 'a bad recipient turns email off and keeps the other settings'
        Set-Content -LiteralPath $path -Value '{ "recipient": "me@example.com" }'
        $good = Get-VmrQBConfigLenient -Path $path
        Assert (@($good.Problems).Count -eq 0 -and $good.Config.recipient -eq 'me@example.com') 'a good config has no problem'
        Assert-Throws-Strict $path
    }

    Check 'A time zone this machine does not know gives the system time zone and a problem, never an error' {
        $unknown = Resolve-VmrQBTimeZone -Id 'Mars/Phobos' -ConfigPath 'C:\x\qb-config.local.json'
        Assert ($unknown.Zone.Id -eq [TimeZoneInfo]::Local.Id -and $unknown.Problem -match "'Mars/Phobos'" -and $unknown.Problem -match 'qb-config\.local\.json') 'the system zone, named, with where to fix it'
        $known = Resolve-VmrQBTimeZone -Id 'America/Los_Angeles'
        Assert ($known.Problem -eq '' -and $null -ne $known.Zone) 'a known zone has no problem'
    }

    Check 'An optional local file that does not parse is skipped and left untouched: the triage context merge' {
        $local = Join-Path $sandbox 'triage-context.local.json'; Set-Content -LiteralPath $local -Value '{ nope'
        $output = Join-Path $sandbox 'merged.json'
        Clear-QBRunNotes
        $merged = Invoke-QBOptional -Name 'Your triage context' -Default $null -Script { Merge-VmrQBTriageContext -LocalPath $local -OutputPath $output }
        Assert ($null -eq $merged -and @(Get-QBRunNotes).Count -eq 1) 'the broken file is noted and skipped'
        Assert ((Get-Content -LiteralPath $local -Raw).Trim() -eq '{ nope') 'the file is left exactly as it was'
        $empty = Merge-VmrQBTriageContext -LocalPath $null -OutputPath $output
        Assert ($empty -eq $output -and @((Get-Content -LiteralPath $output -Raw | ConvertFrom-Json).entries).Count -eq 0) 'without it the merge is simply empty'
        Clear-QBRunNotes
    }
}
finally { Remove-Item -LiteralPath $sandbox -Recurse -Force -ErrorAction SilentlyContinue }

if ($failures) { Write-Host "$failures run-notes test(s) failed." -ForegroundColor Red; exit 1 }
Write-Host 'All run-notes tests passed.' -ForegroundColor Green
