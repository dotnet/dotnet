[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# Run isolation: a rehearsal never touches real state, two runs for one day cannot overlap, and a day is never mailed twice by
# accident. Outlook is replaced by a fake object graph and the "other process" is a real child process, so the claims are
# exercised against real file locking and nothing is ever sent.
$scripts = Split-Path -Parent $PSScriptRoot
. (Join-Path $scripts 'common\core.ps1')
. (Join-Path $scripts 'daily\workspace.ps1')

$failures = 0
function Check([string] $Name, [scriptblock] $Body) {
    try { & $Body; Write-Host "PASS: $Name" -ForegroundColor Green }
    catch { $script:failures++; Write-Host "FAIL: $Name -> $($_.Exception.Message)" -ForegroundColor Red }
}
function Assert($Condition, [string] $Message) { if (-not $Condition) { throw $Message } }
function Assert-Throws([scriptblock] $Body, [string] $Pattern) {
    try { & $Body | Out-Null } catch {
        if ($_.Exception.Message -notmatch $Pattern) { throw "wrong error: $($_.Exception.Message)" }
        return
    }
    throw "Expected an error matching: $Pattern"
}

$sandbox = Join-Path ([IO.Path]::GetTempPath()) ('vmrqb-isolation-' + [guid]::NewGuid().ToString('N'))
[void][IO.Directory]::CreateDirectory($sandbox)
$originalHome = $env:VMR_QB_HOME
$pwsh = (Get-Process -Id $PID).Path

function Invoke-Child([string] $Script) {
    # A real second process: the only honest way to test a lock or an atomic create.
    $encoded = [Convert]::ToBase64String([Text.Encoding]::Unicode.GetBytes($Script))
    return (& $pwsh -NoProfile -NonInteractive -EncodedCommand $encoded 2>&1 | Out-String).Trim()
}

try {
    Check 'A second run for the same state and date is refused while the first is alive, and a crashed or finished run never blocks' {
        $logs = Join-Path $sandbox 'claim-logs'
        $holder = $null
        $first = Enter-QBDailyClaim -LogDirectory $logs -Date '2026-10-08' -Holder ([ref]$holder)
        Assert ($null -ne $first) 'the first run must get the claim'
        $workspace = Join-Path $scripts 'daily\workspace.ps1'
        $probe = ". '$workspace'; `$h = `$null; `$s = Enter-QBDailyClaim -LogDirectory '$logs' -Date '2026-10-08' -Holder ([ref]`$h); if (`$null -eq `$s) { 'REFUSED ' + `$h } else { 'GRANTED'; `$s.Dispose() }"
        $refused = Invoke-Child $probe
        Assert ($refused -like 'REFUSED*' -and $refused -match "pid") "a live holder must refuse a second run and say who it is: $refused"
        Assert ((Invoke-Child ($probe.Replace('2026-10-08', '2026-10-09'))) -eq 'GRANTED') 'another date is a different claim'
        Assert ((Invoke-Child ($probe.Replace($logs, (Join-Path $sandbox 'rehearsal-logs')))) -eq 'GRANTED') 'a rehearsal has its own logs and so its own claim'
        $first.Dispose()
        Assert ((Invoke-Child $probe) -eq 'GRANTED') 'once the first run ends, the day can be run again'
        # A leftover file from a run that was killed is just a file: the lock died with the process.
        [IO.File]::WriteAllText((Join-Path $logs 'daily-2026-10-10.lock'), '{"pid":1,"stale":true}')
        Assert ((Invoke-Child ($probe.Replace('2026-10-08', '2026-10-10'))) -eq 'GRANTED') 'a stale lock file must not block anything'
    }

    Check 'Of several processes claiming one email, exactly one gets it; the rest are told it is taken' {
        $claim = Join-Path $sandbox 'race.claim'
        $workspace = Join-Path $scripts 'daily\workspace.ps1'
        $script = ". '$workspace'; try { Enter-QBEmailClaim -ClaimPath '$claim' -Subject 'race'; 'CLAIMED' } catch { 'REFUSED' }"
        $processes = foreach ($n in 1..6) {
            $info = [Diagnostics.ProcessStartInfo]::new($pwsh)
            foreach ($argument in @('-NoProfile', '-NonInteractive', '-EncodedCommand', [Convert]::ToBase64String([Text.Encoding]::Unicode.GetBytes($script)))) { $info.ArgumentList.Add($argument) }
            $info.RedirectStandardOutput = $true; $info.UseShellExecute = $false
            [Diagnostics.Process]::Start($info)
        }
        $results = @($processes | ForEach-Object { $text = $_.StandardOutput.ReadToEnd(); $_.WaitForExit(); $text.Trim() })
        Assert (@($results | Where-Object { $_ -eq 'CLAIMED' }).Count -eq 1) "exactly one claim must win: $($results -join ',')"
        Assert (@($results | Where-Object { $_ -eq 'REFUSED' }).Count -eq 5) "the others must be refused: $($results -join ',')"
    }

    Check 'An email claim says whether it was already sent or may have been delivered; -Force replaces it; a failed send gives it back' {
        $claim = Join-Path $sandbox 'policy.claim'; $receipt = Join-Path $sandbox 'policy.json'
        Enter-QBEmailClaim -ClaimPath $claim -ReceiptPath $receipt -Subject 'one'
        Assert-Throws { Enter-QBEmailClaim -ClaimPath $claim -ReceiptPath $receipt -Subject 'two' } 'left no receipt, so it may already have been delivered.*Sent folder.*-Force'
        [IO.File]::WriteAllText($receipt, '{"status":"sent"}')
        Assert-Throws { Enter-QBEmailClaim -ClaimPath $claim -ReceiptPath $receipt -Subject 'two' } 'already sent \(there is a receipt\).*-Force'
        Enter-QBEmailClaim -ClaimPath $claim -ReceiptPath $receipt -Subject 'forced' -Force
        Assert ((Get-Content -LiteralPath $claim -Raw) -match 'forced') '-Force must replace the claim'
        Clear-QBEmailClaim -ClaimPath $claim
        Assert (-not (Test-Path -LiteralPath $claim)) 'a released claim is gone'
        Enter-QBEmailClaim -ClaimPath $claim -ReceiptPath $receipt -Subject 'retry'
    }

    Check 'Send-QBEmail claims before it sends: one delivery, a refused repeat, -Force to resend, a retry after a refused send, and no retry after a possible delivery' {
        $runner = [Management.Automation.Language.Parser]::ParseFile((Join-Path $scripts 'daily\Invoke-Daily.ps1'), [ref]$null, [ref]$null)
        $function = $runner.Find({ param($node) $node -is [Management.Automation.Language.FunctionDefinitionAst] -and $node.Name -eq 'Send-QBEmail' }, $false)
        . ([scriptblock]::Create($function.Extent.Text))
        $policy = $runner.Find({ param($node) $node -is [Management.Automation.Language.FunctionDefinitionAst] -and $node.Name -eq 'Get-QBEmailRetryPolicy' }, $false)
        . ([scriptblock]::Create($policy.Extent.Text))
        # One attempt keeps the claim checks below about the claim; the retry checks at the end set their own policy.
        $script:QBEmailAttempts = 1; $script:QBEmailRetryDelaySeconds = 0
        $script:sendCalls = 0; $script:refuse = $false; $script:refuseTimes = 0
        $mail = [pscustomobject]@{ SendUsingAccount = $null; SaveSentMessageFolder = $null; To = ''; Subject = ''; HTMLBody = ''; Attachments = [pscustomobject]@{} }
        Add-Member -InputObject $mail.Attachments -MemberType ScriptMethod -Name Add -Value { param($path) 1 }
        Add-Member -InputObject $mail -MemberType ScriptMethod -Name Send -Value { $script:sendCalls++; if ($script:refuseTimes -gt 0) { $script:refuseTimes--; throw 'Outlook refused the message.' }; if ($script:refuse) { throw 'Outlook refused the message.' } }
        $store = [pscustomobject]@{}; Add-Member -InputObject $store -MemberType ScriptMethod -Name GetDefaultFolder -Value { param($n) [pscustomobject]@{ folder = $n } }
        $account = [pscustomobject]@{ SmtpAddress = 'qb@example.com'; DeliveryStore = $store }
        $accounts = [pscustomobject]@{ Count = 1 }; Add-Member -InputObject $accounts -MemberType ScriptMethod -Name Item -Value { param($i) $account }.GetNewClosure()
        $outlook = [pscustomobject]@{ Session = [pscustomobject]@{ Offline = $false; Accounts = $accounts } }
        Add-Member -InputObject $outlook -MemberType ScriptMethod -Name CreateItem -Value { param($kind) $mail }.GetNewClosure()
        function New-Object { param([string] $TypeName, [string] $ComObject) if ($ComObject -eq 'Outlook.Application') { return $outlook }; Microsoft.PowerShell.Utility\New-Object @PSBoundParameters }
        $config = [pscustomobject]@{ recipient = 'qb@example.com' }
        $Date = '2026-10-08'
        $attachment = Join-Path $sandbox 'report.md'; [IO.File]::WriteAllText($attachment, '# report')
        $receipt = Join-Path $sandbox 'email-send.json'
        $claim = [IO.Path]::ChangeExtension($receipt, 'claim')
        $send = { param([switch] $Force) Send-QBEmail -Attachments @($attachment) -HtmlBody '<p>x</p>' -ReceiptPath $receipt -RunStatus complete -Force:$Force 6>$null }

        Assert ((& $send) -eq $true -and $script:sendCalls -eq 1) 'the first send delivers once'
        Assert ((Get-Content -LiteralPath $receipt -Raw | ConvertFrom-Json).status -eq 'sent' -and (Test-Path -LiteralPath $claim)) 'a delivery leaves its receipt and keeps its claim'
        Assert-Throws { & $send } 'already has a claim.*already sent'
        Assert ($script:sendCalls -eq 1) 'a refused repeat must not call Outlook at all'
        Assert ((& $send -Force) -eq $true -and $script:sendCalls -eq 2) '-Force sends again'

        Remove-Item -LiteralPath $receipt, $claim -Force
        $script:refuse = $true
        Assert-Throws { & $send } 'Outlook refused the message'
        Assert (-not (Test-Path -LiteralPath $claim)) 'a send that failed before anything was submitted gives its claim back'
        $script:refuse = $false
        Assert ((& $send) -eq $true -and $script:sendCalls -eq 4) 'so it can simply be retried'

        # Delivered, but the receipt could not be written: the day may be in someone's inbox, so a repeat must be refused.
        Remove-Item -LiteralPath $receipt, $claim -Force
        [void][IO.Directory]::CreateDirectory($receipt)
        $before = $script:sendCalls
        Assert-Throws { & $send } '.'
        Assert ($script:sendCalls -eq $before + 1 -and (Test-Path -LiteralPath $claim)) 'a possible delivery keeps its claim'
        Remove-Item -LiteralPath $receipt -Recurse -Force
        Assert-Throws { & $send } 'left no receipt, so it may already have been delivered'
        Assert ($script:sendCalls -eq $before + 1) 'and the repeat is not sent'

        # A send that did not go out is tried again (Outlook is often just busy); after the last attempt the claim is given back.
        # A possible delivery is never repeated: that stays until a QB asks for a resend (-Force).
        $script:QBEmailAttempts = 3
        Remove-Item -LiteralPath $receipt, $claim -Force -ErrorAction SilentlyContinue
        $script:refuseTimes = 2; $before = $script:sendCalls
        Assert ((& $send 3>$null) -eq $true -and $script:sendCalls -eq $before + 3) 'a send Outlook refused twice is delivered on the third attempt'
        Remove-Item -LiteralPath $receipt, $claim -Force -ErrorAction SilentlyContinue
        $script:refuseTimes = 5; $before = $script:sendCalls
        Assert-Throws { & $send 3>$null } 'Outlook refused the message'
        Assert ($script:sendCalls -eq $before + 3 -and -not (Test-Path -LiteralPath $claim)) 'three attempts, then the claim is given back'
        $script:refuseTimes = 0

        # An attachment that is not there is left out and noted; it never costs the email or the reports that are there.
        . (Join-Path $scripts 'daily\notes.ps1')
        Clear-QBRunNotes
        Remove-Item -LiteralPath $receipt, $claim -Force -ErrorAction SilentlyContinue
        $before = $script:sendCalls
        $missingPath = Join-Path $sandbox 'not-there.md'
        Assert ((Send-QBEmail -Attachments @($attachment, $missingPath) -HtmlBody '<p>x</p>' -ReceiptPath $receipt -RunStatus complete 6>$null 3>$null) -eq $true -and $script:sendCalls -eq $before + 1) 'the email goes without the missing attachment'
        Assert (@((Get-Content -LiteralPath $receipt -Raw | ConvertFrom-Json).attachments).Count -eq 1) 'only the attachment that exists is in the receipt'
        Assert (@(Get-QBRunNotes | Where-Object { $_.source -eq 'email' -and $_.severity -eq 'first' -and $_.message -match 'not-there\.md' }).Count -eq 1) 'a highlighted note names the missing one'

        # Several recipients: one message to all of them, sent from the Outlook account that matches any one of them.
        Remove-Item -LiteralPath $receipt, $claim -Force -ErrorAction SilentlyContinue
        $config = [pscustomobject]@{ recipient = 'team@example.com; qb@example.com' }
        Assert ((& $send) -eq $true) 'a list that includes your own address is delivered'
        Assert ($mail.To -eq 'team@example.com; qb@example.com' -and $mail.SendUsingAccount.SmtpAddress -eq 'qb@example.com') 'it goes to every recipient, from the account that matches one of them'
        Remove-Item -LiteralPath $receipt, $claim -Force -ErrorAction SilentlyContinue
        $config = [pscustomobject]@{ recipient = 'team@example.com; other@example.com' }
        $before = $script:sendCalls
        Assert-Throws { & $send } 'no account matching any recipient.*at least one recipient must be your own'
        Assert ($script:sendCalls -eq $before -and -not (Test-Path -LiteralPath $claim)) 'with none of them yours nothing is sent and the claim is given back'
    }

    Check 'A rehearsal (-OutputRoot) writes only under its own root and replays only into its own copy of the history; real state is untouched' {
        $env:VMR_QB_HOME = Join-Path $sandbox 'real-state'
        $paths = Get-VmrQBPaths -Create
        $date = '2026-10-08'
        $header = '| Issue category | Builds / PRs (links) | New Issue (Y/N) | Existing fix PR (status) | Action as VMR QB (who / where to ping) | Links that QB reported |'
        function New-Overall([string] $Finding, [int] $BuildId) {
            $link = "[$BuildId](https://dev.azure.com/dnceng/internal/_build/results?buildId=$BuildId)"
            $where = 'Teams - Unified Build Technical and General Discussions'
            @("# VMR QB Daily Overall Report - $date", '', '## Top actions for the QB today', '',
              "1. $link - **BH #1:** $Finding failed. Where: $where.", '', '## Summary', '', $header, '|---|---|---|---|---|---|',
              "| BH #1 - $Finding | $link | Y | None found | Where: $where.<br>Copy/paste: `"@dotnet/dnceng, Please check the build failure. ${Finding} failed: $link`" | None found |") -join "`n"
        }
        function Write-Reports([string] $ReportsDir, [string] $Finding, [int] $BuildId) {
            [void][IO.Directory]::CreateDirectory($ReportsDir)
            [IO.File]::WriteAllText((Join-Path $ReportsDir "build-health-$date.md"), "# Build Health Monitoring - $date`n")
            [IO.File]::WriteAllText((Join-Path $ReportsDir "code-flow-$date.md"), "# Code Flow Monitoring - $date`n")
            [IO.File]::WriteAllText((Join-Path $ReportsDir "overall-summary-$date.md"), (New-Overall $Finding $BuildId) + "`n")
        }
        # Real state: a delivered report for the same day, a history that knows the delivered finding, and a receipt.
        Write-Reports $paths.ReportsDir 'Delivered finding' 111
        [IO.File]::WriteAllText((Join-Path $paths.LogsDir "email-$date.json"), '{"status":"sent"}')
        . (Join-Path $scripts 'history\history.ps1')
        Initialize-VmrIssueStore -Path $paths.HistoryPath | Out-Null
        Add-VmrIssueOccurrence -Path $paths.HistoryPath -Name 'Delivered finding' -ReportDate $date -BuildIds @('111') | Out-Null
        function Get-Tree([string] $Root) { @(Get-ChildItem -LiteralPath $Root -Recurse -File -Force | Where-Object { $_.Extension -ne '.lock' -and $_.Name -notlike 'schedule-audit.log' } | Sort-Object FullName | ForEach-Object { "$($_.FullName.Substring($Root.Length))=$((Get-FileHash -LiteralPath $_.FullName).Hash)" }) -join ';' }
        $reportsBefore = Get-Tree $paths.ReportsDir; $historyBefore = (Get-FileHash -LiteralPath $paths.HistoryPath).Hash; $logsReceiptBefore = (Get-FileHash -LiteralPath (Join-Path $paths.LogsDir "email-$date.json")).Hash

        $rehearsal = Join-Path $sandbox 'rehearsal'
        Write-Reports (Join-Path $rehearsal 'reports') 'Rehearsed finding' 222
        $output = & $pwsh -NoProfile -NonInteractive -File (Join-Path $scripts 'daily\Invoke-Daily.ps1') -Date $date -UseExistingReports -SkipEmail -OutputRoot $rehearsal 2>&1 | Out-String
        Assert ($output -notmatch 'Another VMR QB run|PREFLIGHT FAILED') "the rehearsal must start: $output"
        Assert ($output -match 'Issue history updated') "the rehearsal replays its own reports into its own history: $output"

        Assert ((Get-Tree $paths.ReportsDir) -ceq $reportsBefore) 'the delivered reports must be untouched'
        Assert ((Get-FileHash -LiteralPath $paths.HistoryPath).Hash -eq $historyBefore) 'the real history must be untouched'
        Assert ((Get-FileHash -LiteralPath (Join-Path $paths.LogsDir "email-$date.json")).Hash -eq $logsReceiptBefore) 'the real receipt must be untouched'
        Assert (-not (Test-Path -LiteralPath (Join-Path $paths.RunDir '*'))) 'the real run folder must be untouched'
        Assert (-not (Test-Path -LiteralPath (Join-Path $paths.LogsDir "preflight-$date-*.json"))) 'no real log of the rehearsal'

        $copy = Join-Path $rehearsal 'issue-history.json'
        Assert (Test-Path -LiteralPath $copy) 'the rehearsal keeps its own copy of the history'
        $names = @(Get-VmrIssueHistory -Path $copy -Limit 100 | ForEach-Object name)
        Assert ($names -contains 'Rehearsed finding' -and $names -contains 'Delivered finding') "the copy holds the real history plus the rehearsal's own finding: $($names -join ', ')"
        $realNames = @(Get-VmrIssueHistory -Path $paths.HistoryPath -Limit 100 | ForEach-Object name)
        Assert ($realNames -notcontains 'Rehearsed finding') 'the rehearsal must never reach the real history'
        Assert (@(Get-ChildItem -LiteralPath $rehearsal -Directory | ForEach-Object Name) -contains 'logs') 'the rehearsal logs under its own root'
    }
}
finally {
    if ($null -eq $originalHome) { Remove-Item Env:\VMR_QB_HOME -ErrorAction SilentlyContinue } else { $env:VMR_QB_HOME = $originalHome }
    Remove-Item -LiteralPath $sandbox -Recurse -Force -ErrorAction SilentlyContinue
}

if ($failures) { Write-Host "$failures run-isolation test(s) failed." -ForegroundColor Red; exit 1 }
Write-Host 'All run-isolation tests passed.' -ForegroundColor Green
