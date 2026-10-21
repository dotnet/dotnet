Set-StrictMode -Version Latest

# daily/email.ps1 - The HTML body of the round's email: run status, the status of each report, the run notes and the overall
# report. Outlook strips <style> blocks, so every table is styled inline. Defines functions only; loading it runs nothing.
. (Join-Path $PSScriptRoot 'notes.ps1')

function ConvertTo-QBHtmlBody {
    param(
        [Parameter(Mandatory)][ValidatePattern('^\d{4}-\d{2}-\d{2}$')][string] $Date,
        [Parameter(Mandatory)][string] $RunStatus,
        [Parameter(Mandatory)][AllowEmptyCollection()][object[]] $Results,
        [Parameter(Mandatory)][AllowEmptyCollection()][string[]] $Attachments,
        [AllowNull()][string] $OverallMarkdown,
        # The run's notes (Add-QBRunNote), drawn in a box at the top of the email.
        [AllowEmptyCollection()][object[]] $Notes = @(),
        # Saved reports being sent again keep the notes block they were saved with; it is not this run's, so it is not drawn twice.
        [switch] $KeepSavedNotes
    )
    $esc = { param($s) if ($null -eq $s) { '' } else { [System.Net.WebUtility]::HtmlEncode([string]$s) } }

    $statusRows = foreach ($r in $Results) {
        $ok = $r.status -eq 'succeeded'
        $color = if ($ok) { '#107c10' } else { '#d13438' }
        $label = if ($ok) { 'OK' } else { 'FAILED' }
        $detail = if ($r.PSObject.Properties['error'] -and $r.error) { ' &mdash; ' + (& $esc $r.error) } else { '' }
        $modelNote = if ($r.PSObject.Properties['model'] -and $r.model) { " <span style=""color:#777;"">(model: $(& $esc $r.model))</span>" } else { '' }
        "<li><strong style=""color:$color;"">[$label]</strong> $(& $esc $r.name)$modelNote$detail</li>"
    }

    # Markdown -> HTML, with inline styles on the tables for Outlook. A document the converter cannot read is shown as plain text.
    $overallHtml = if ($OverallMarkdown) {
        # This run's notes are drawn as their own box above, so the block that carries them in the saved report is left out here.
        # A saved report that is only being sent again keeps its own block: those notes are not this run's.
        $markdown = if ($KeepSavedNotes) { $OverallMarkdown } else { Remove-QBRunNotesBlock -Content $OverallMarkdown }
        try {
            (ConvertFrom-Markdown -InputObject $markdown).Html `
                -replace '<table>', '<table style="border-collapse:collapse;width:100%;margin:10px 0;font-size:13px;">' `
                -replace '<th>', '<th style="border:1px solid #c8c8c8;padding:6px 9px;background:#f3f3f3;text-align:left;vertical-align:top;">' `
                -replace '<td>', '<td style="border:1px solid #c8c8c8;padding:6px 9px;text-align:left;vertical-align:top;">'
        }
        catch {
            '<p style="color:#d13438;"><strong>This report could not be formatted for email, so it is shown as plain text.</strong> It is attached in full.</p><pre style="white-space:pre-wrap;font-size:12px;">' + (& $esc $markdown) + '</pre>'
        }
    }
    else { '<p style="color:#d13438;"><strong>The overall summary report was not produced this run.</strong> See the per-report status above and any attached reports.</p>' }

    $attachLine = if ($Attachments.Count) {
        'Attached (full detail): ' + (($Attachments | ForEach-Object { & $esc (Split-Path $_ -Leaf) }) -join ', ')
    } else { 'No reports were successfully generated to attach.' }
    $statusColor = switch ($RunStatus) { 'complete' { '#107c10' } 'partial' { '#c19c00' } 're-sent from saved reports' { '#605e5c' } default { '#d13438' } }
    $notesHtml = Format-QBRunNotesHtml -Notes $Notes

    @"
<html><head><meta charset="utf-8"></head>
<body style="font-family:'Segoe UI',Arial,sans-serif;font-size:14px;color:#1b1b1b;line-height:1.45;">
<h2 style="margin:0 0 4px 0;">VMR QB daily monitoring &mdash; $Date</h2>
<p style="margin:0 0 10px 0;">Run status: <strong style="color:$statusColor;">$RunStatus</strong></p>
$notesHtml
<ul style="margin:0 0 14px 18px;padding:0;">
$($statusRows -join "`n")
</ul>
<hr style="border:none;border-top:1px solid #e1e1e1;margin:12px 0;">
$overallHtml
<hr style="border:none;border-top:1px solid #e1e1e1;margin:12px 0;">
<p style="margin:6px 0;color:#444;">$(& $esc $attachLine)</p>
<p style="margin:6px 0;color:#777;font-size:12px;">Monitoring is read-only. No PRs were approved, merged, or closed; no Teams messages, subscription changes, or retries were issued.</p>
</body></html>
"@
}
