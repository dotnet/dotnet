[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot '..\triage\timeout.ps1')

$failures = 0
function Check([string] $Name, [scriptblock] $Body) {
    try { & $Body; Write-Host "PASS: $Name" -ForegroundColor Green }
    catch { $script:failures++; Write-Host "FAIL: $Name -> $($_.Exception.Message)" -ForegroundColor Red }
}
function Assert([bool] $Condition, [string] $Message) { if (-not $Condition) { throw $Message } }

# One AzDO log line: UTC prefix + the agent's bracketed clock + the marker text.
function L([string] $Utc, [string] $Clock, [string] $Text) { "2026-10-05T${Utc}0000Z   [$Clock] $Text" }

# A leg that ran long: arcade finishes, then runtime and emsdk start and never finish, and the log
# is cut when the job hits its time limit. aspnetcore logs an unrelated error but finished.
$timedOutLeg = @(
    (L '10:00:00.0' '10:00:00.00' 'Building arcade'),
    (L '10:01:00.0' '10:01:00.00' 'Building arcade...done'),
    (L '10:01:01.0' '10:01:01.00' 'Building aspnetcore'),
    (L '10:01:02.0' '10:01:02.00' 'Building runtime'),
    (L '10:01:03.0' '10:01:03.00' 'Building emsdk'),
    "2026-10-05T10:20:00.0000000Z D:\a\_work\1\s\src\aspnetcore\eng\build.cmd(12,3): error MSB3073: command exited with code 1",
    (L '10:30:00.0' '10:30:00.00' 'Building aspnetcore...done'),
    '2026-10-05T12:30:00.0000000Z ##[section]Finishing: Linux_x64'
)

Check 'A leg cut off mid-build names the repos that were running, not the one that only logged an error' {
    $state = Get-VmrQBRepoBuildState -Line $timedOutLeg
    Assert $state.reliable 'a complete log with markers must be reliable'
    Assert (($state.unfinished -join ',') -eq 'runtime,emsdk') "unfinished=$($state.unfinished -join ',')"
    $result = Get-VmrQBTimeoutAttribution -State $state
    Assert (($result.repoAtTimeout -join ',') -eq 'runtime,emsdk') "suspects=$($result.repoAtTimeout -join ',')"
    # The 2026-10 misattribution: aspnetcore logged an error in the same file but had finished.
    Assert ($result.repoAtTimeout -notcontains 'aspnetcore') 'aspnetcore must never be named for a timeout it did not cause'
    Assert ($result.errorOnlyRepos -contains 'aspnetcore') 'the error-only repo must be reported separately'
    Assert ($result.statement -match 'do not attribute the timeout to them') 'the statement must warn against attributing to error-only repos'
    Assert ($result.statement -match 'runtime \(running 149 min\)') "running time missing or wrong: $($result.statement)"
}

Check 'A repo that merely started last when an upstream failure ended the build is a bystander' {
    $leg = @(
        (L '10:00:00.0' '10:00:00.00' 'Building runtime'),
        (L '10:00:30.0' '10:00:30.00' 'Building runtime...done'),
        (L '10:00:31.0' '10:00:31.00' 'Building sdk'),
        "2026-10-05T10:00:35.0000000Z ##[error]Bash exited with code '1'.",
        '2026-10-05T10:00:36.0000000Z ##[section]Finishing: SB_Fedora_Offline'
    )
    $result = Get-VmrQBTimeoutAttribution -State (Get-VmrQBRepoBuildState -Line $leg)
    Assert ($result.repoAtTimeout.Count -eq 0) "a 5-second repo was named as a timeout suspect: $($result.repoAtTimeout -join ',')"
    Assert (-not $result.timedOut) 'a 36-second log did not run to a 150-minute limit'
    Assert ($result.statement -match 'ended by failure or cancellation, not by timing out') $result.statement
    Assert ($result.statement -match 'Do not report it as a timeout') $result.statement
}

Check 'A repo that started just before the end of a log that DID run to the limit is a bystander' {
    $leg = @(
        (L '10:00:00.0' '10:00:00.00' 'Building runtime'),
        (L '10:01:00.0' '10:01:00.00' 'Building runtime...done'),
        (L '12:27:00.0' '12:27:00.00' 'Building sdk'),
        '2026-10-05T12:30:00.0000000Z ##[section]Finishing: Linux_x64'
    )
    $result = Get-VmrQBTimeoutAttribution -State (Get-VmrQBRepoBuildState -Line $leg)
    Assert $result.timedOut 'a 150-minute log is a timeout'
    Assert ($result.repoAtTimeout.Count -eq 0) "a repo that ran 3 minutes was named: $($result.repoAtTimeout -join ',')"
    Assert ($result.bystanders -contains 'sdk') 'sdk should be a bystander'
    Assert ($result.statement -match 'none of them is the cause') $result.statement
}

Check 'A repo that ran long in a log that stopped well short of the limit is still not a timeout' {
    # The real trap: sdk had been running 10-17 minutes in legs that FAILED, not timed out.
    $leg = @(
        (L '10:00:00.0' '10:00:00.00' 'Building sdk'),
        '2026-10-05T10:55:00.0000000Z ##[error]Build FAILED.',
        '2026-10-05T10:55:01.0000000Z ##[section]Finishing: Build'
    )
    $result = Get-VmrQBTimeoutAttribution -State (Get-VmrQBRepoBuildState -Line $leg)
    Assert (-not $result.timedOut) 'a 55-minute log must not count as a 150-minute timeout'
    Assert ($result.repoAtTimeout.Count -eq 0) 'no repo may be blamed for a timeout that did not happen'
    Assert ($result.statement -match 'spans 55 minutes') $result.statement
}

Check 'The time limit can be taken from the job instead of assuming 150' {
    $state = Get-VmrQBRepoBuildState -Line $timedOutLeg
    Assert ((Get-VmrQBTimeoutAttribution -State $state -TimeLimitMinutes 150).timedOut) '150 minutes of log meets a 150-minute limit'
    Assert (-not (Get-VmrQBTimeoutAttribution -State $state -TimeLimitMinutes 240).timedOut) '150 minutes of log is not a 240-minute timeout'
}

Check 'Parallel starts are counted per repo, and a second pass of the same repo is tracked' {
    $leg = @(
        (L '10:00:00.0' '10:00:00.00' 'Building runtime'),
        (L '10:10:00.0' '10:10:00.00' 'Building runtime...done'),
        (L '10:20:00.0' '10:20:00.00' 'Building runtime'),
        '2026-10-05T11:20:00.0000000Z ##[section]Finishing: Windows_x86'
    )
    $state = Get-VmrQBRepoBuildState -Line $leg
    $runtime = @($state.repos | Where-Object repo -eq 'runtime')[0]
    Assert ($runtime.starts -eq 2 -and $runtime.completions -eq 1 -and $runtime.open -eq 1) "starts=$($runtime.starts) completions=$($runtime.completions) open=$($runtime.open)"
    Assert ($state.unfinished -contains 'runtime') 'the open second pass must count as unfinished'
    Assert ([int]$runtime.runningSeconds -eq 3600) "second pass should have run 3600s, got $($runtime.runningSeconds)"
}

Check 'A truncated log is never used to name a repo' {
    $state = Get-VmrQBRepoBuildState -Line $timedOutLeg -Truncated
    Assert (-not $state.reliable) 'a truncated log must not be reliable'
    $result = Get-VmrQBTimeoutAttribution -State $state
    Assert ($result.repoAtTimeout.Count -eq 0) 'no repo may be named from a truncated log'
    Assert ($result.statement -match 'truncated') $result.statement
    Assert ($result.statement -match 'tail') 'the statement must say the tail is what is needed'
}

Check 'A .partial file is treated as truncated without being told' {
    $path = Join-Path ([IO.Path]::GetTempPath()) ('vmrqb-repobuild-' + [guid]::NewGuid().ToString('N') + '.log.partial')
    try {
        $timedOutLeg | Set-Content -LiteralPath $path -Encoding utf8
        $state = Get-VmrQBRepoBuildState -Path $path
        Assert $state.truncated 'the collector marks a capped download with .partial'
        Assert (-not $state.reliable) 'so it cannot be reliable'
    }
    finally { Remove-Item $path -Force -ErrorAction SilentlyContinue }
}

Check 'A log with no repo markers says so instead of inventing an answer' {
    $state = Get-VmrQBRepoBuildState -Line @('2026-10-05T10:00:00.0000000Z hello', '2026-10-05T10:00:01.0000000Z world')
    Assert (-not $state.hasMarkers) 'no markers expected'
    Assert (-not $state.reliable) 'no markers means nothing reliable can be said'
    Assert ($state.summary -match 'not a repo-build log') $state.summary
    Assert ((Get-VmrQBTimeoutAttribution -State $state).repoAtTimeout.Count -eq 0) 'nothing may be attributed'
}

Check 'Lines without a UTC prefix still work, including across midnight' {
    $leg = @(
        '[23:50:00.00] Building runtime',
        '[23:55:00.00] Building emsdk',
        '[00:20:00.00] Building emsdk...done'
    )
    $state = Get-VmrQBRepoBuildState -Line $leg
    $runtime = @($state.repos | Where-Object repo -eq 'runtime')[0]
    Assert ($runtime.open -eq 1) 'runtime never finished'
    Assert ([int]$runtime.runningSeconds -eq 1800) "midnight rollover broke the duration: $($runtime.runningSeconds)"
}

Check 'A nested src folder inside a repo is not mistaken for a repo' {
    $leg = @(
        (L '10:00:00.0' '10:00:00.00' 'Building sdk'),
        "2026-10-05T10:00:02.0000000Z D:\a\_work\1\s\src\sdk\src\Layout\redist\targets\Foo.targets(10,5): error MSB4018: boom",
        '2026-10-05T10:00:03.0000000Z ##[section]Finishing: Build'
    )
    $state = Get-VmrQBRepoBuildState -Line $leg
    Assert ($state.errorRepos -contains 'sdk') 'the error belongs to sdk'
    Assert ($state.errorRepos -notcontains 'layout') "'layout' is a folder inside sdk, not a repo: $($state.errorRepos -join ',')"
}

Check 'Pipeline input from Get-Content works and the last lines are retained for context' {
    $path = Join-Path ([IO.Path]::GetTempPath()) ('vmrqb-repobuild-' + [guid]::NewGuid().ToString('N') + '.log')
    try {
        $timedOutLeg | Set-Content -LiteralPath $path -Encoding utf8
        $state = Get-Content -LiteralPath $path | Get-VmrQBRepoBuildState
        Assert (($state.unfinished -join ',') -eq 'runtime,emsdk') 'pipeline input lost lines'
        Assert ($state.lastLines[-1] -match 'Finishing: Linux_x64') 'the final line should be kept so the report can show how the log ended'
    }
    finally { Remove-Item $path -Force -ErrorAction SilentlyContinue }
}

Check 'Plain, empty and untimestamped logs give the documented "not a repo-build log" answer instead of an index error' {
    $entry = Join-Path $PSScriptRoot '..\triage\Invoke-Triage.ps1'
    $directory = Join-Path ([IO.Path]::GetTempPath()) ('vmrqb-plainlog-' + [guid]::NewGuid().ToString('N'))
    [void][IO.Directory]::CreateDirectory($directory)
    try {
        $plain = Join-Path $directory 'plain.log'
        [IO.File]::WriteAllText($plain, "Restoring packages`nBuild succeeded.`n")
        $empty = Join-Path $directory 'empty.log'
        [IO.File]::WriteAllText($empty, '')
        foreach ($path in @($plain, $empty)) {
            $text = & pwsh -NoProfile -File $entry timeout -LogPath $path -Json 2>&1 | Out-String
            Assert ($LASTEXITCODE -eq 0) "the entry script failed for $(Split-Path -Leaf $path): $text"
            $result = $text | ConvertFrom-Json
            Assert ($result.reliable -eq $false -and $result.statement -match 'not a repo-build log') "the documented answer is expected for $(Split-Path -Leaf $path): $($result.statement)"
        }
        $state = Get-VmrQBRepoBuildState -Line @('plain text', 'no timestamps', 'no markers')
        Assert (-not $state.hasMarkers -and $null -eq $state.firstTimestamp -and $null -eq $state.elapsedMinutes) 'a plain log has no first timestamp and no elapsed time'
        $none = Get-VmrQBRepoBuildState -Line @()
        Assert ($none.summary -match 'not a repo-build log') 'zero lines is also a plain log'
        # Markers but no UTC prefixes at all (only the agent clock): still analysable.
        $clockOnly = Get-VmrQBRepoBuildState -Line @('[10:00:00.00] Building runtime', '[10:00:30.00] Building runtime...done', '[10:00:31.00] Building sdk')
        Assert ($clockOnly.hasMarkers -and ($clockOnly.unfinished -join ',') -eq 'sdk') 'clock-only markers are still read'
    }
    finally { Remove-Item -LiteralPath $directory -Recurse -Force -ErrorAction SilentlyContinue }
}

Check 'The job''s own limit decides: the same 58-minute log is a timeout at 60 minutes, not at 150 or 240, and an unknown limit is uncertain rather than a failure' {
    function New-Leg([int] $Minutes) {
        $end = [datetime]::new(2026, 10, 5, 10, 0, 0, [DateTimeKind]::Utc).AddMinutes($Minutes)
        @((L '10:00:00.0' '10:00:00.00' 'Building runtime'), (L '10:00:01.0' '10:00:01.00' 'Building emsdk'),
          "$($end.ToString('yyyy-MM-ddTHH:mm:ss.fffffff'))Z ##[section]Finishing: Linux_x64")
    }
    $short = Get-VmrQBRepoBuildState -Line (New-Leg 58)
    $at60 = Get-VmrQBTimeoutAttribution -State $short -TimeLimitMinutes 60
    Assert ($at60.timedOut -and ($at60.repoAtTimeout -join ',') -eq 'runtime,emsdk' -and -not $at60.limitAssumed -and $at60.timeLimitMinutes -eq 60) "60-minute limit: $($at60 | ConvertTo-Json -Compress)"
    $at150 = Get-VmrQBTimeoutAttribution -State $short -TimeLimitMinutes 150
    Assert (-not $at150.timedOut -and $at150.statement -match 'ended by failure or cancellation') 'against a stated 150-minute limit a 58-minute log did not time out'
    $at240 = Get-VmrQBTimeoutAttribution -State $short -TimeLimitMinutes 240
    Assert (-not $at240.timedOut) 'against 240 minutes it is far short'
    $assumed = Get-VmrQBTimeoutAttribution -State $short -LimitIsAssumed
    Assert (-not $assumed.timedOut -and $assumed.uncertain -and $assumed.limitAssumed -and $assumed.repoAtTimeout.Count -eq 0) 'an assumed limit with a short log is uncertain and names no repo'
    Assert ($assumed.statement -match 'uncertain' -and $assumed.statement -match 'not available' -and $assumed.statement -notmatch 'ended by failure or cancellation') "an assumed limit must not turn into a different cause: $($assumed.statement)"
    $long150 = Get-VmrQBRepoBuildState -Line (New-Leg 149)
    $a150 = Get-VmrQBTimeoutAttribution -State $long150 -TimeLimitMinutes 150
    Assert ($a150.timedOut -and ($a150.repoAtTimeout -join ',') -eq 'runtime,emsdk') 'a 149-minute log is a timeout at 150'
    $assumedLong = Get-VmrQBTimeoutAttribution -State $long150 -LimitIsAssumed
    Assert ($assumedLong.timedOut -and -not $assumedLong.uncertain -and $assumedLong.statement -match 'VMR default') 'a log that reached the assumed limit is analysed, and says the limit was assumed'
    $long240 = Get-VmrQBRepoBuildState -Line (New-Leg 238)
    Assert ((Get-VmrQBTimeoutAttribution -State $long240 -TimeLimitMinutes 240).timedOut) 'a 238-minute log is a timeout at 240'
    Assert (-not (Get-VmrQBTimeoutAttribution -State $long240 -TimeLimitMinutes 150).uncertain) 'a stated limit is never uncertain'
    $truncated = Get-VmrQBRepoBuildState -Line (New-Leg 58) -Truncated
    $cut = Get-VmrQBTimeoutAttribution -State $truncated -TimeLimitMinutes 60
    Assert (-not $cut.reliable -and -not $cut.timedOut -and $cut.repoAtTimeout.Count -eq 0) 'a truncated log never names a repo, whatever the limit'
}

Check 'The daily packet uses the limit the job itself reported, per job id, and says when it had to assume one' {
    . (Join-Path $PSScriptRoot '..\daily\report.ps1')
    $directory = Join-Path ([IO.Path]::GetTempPath()) ('vmrqb-limit-' + [guid]::NewGuid().ToString('N'))
    [void][IO.Directory]::CreateDirectory($directory)
    try {
        $log = Join-Path $directory 'leg.log'
        [IO.File]::WriteAllLines($log, @((L '10:00:00.0' '10:00:00.00' 'Building runtime'), (L '10:00:01.0' '10:00:01.00' 'Building emsdk'),
                '2026-10-05T10:58:00.0000000Z ##[section]Finishing: Linux_x64'))
        $tasks = @(
            [ordered]@{ task = 'Linux_x64'; job = 'Linux_x64'; jobId = 'job-short'; recordType = 'Job'; result = 'canceled'; issues = @('The job running on agent A ran longer than the maximum time of 60 minutes.') },
            [ordered]@{ task = 'Linux_arm64'; job = 'Linux_arm64'; jobId = 'job-other'; recordType = 'Job'; result = 'canceled'; issues = @('The job running on agent B ran longer than the maximum time of 240 minutes.') })
        Assert ((Get-QBJobTimeLimitMinutes -JobId 'job-short' -Task $tasks -Log $null) -eq 60) 'the job''s own limit comes from its own record'
        Assert ((Get-QBJobTimeLimitMinutes -JobId 'job-other' -Task $tasks -Log $null) -eq 240) 'another job keeps its own limit'
        Assert ($null -eq (Get-QBJobTimeLimitMinutes -JobId 'job-unknown' -Task $tasks -Log $null)) 'a job that never reported a limit has none, not 150'
        $fromLog = [pscustomobject]@{ diagnosticExcerpt = 'The job running on agent C ran longer than the maximum time of 90 minutes.' }
        Assert ((Get-QBJobTimeLimitMinutes -JobId 'job-unknown' -Task $tasks -Log $fromLog) -eq 90) 'the log''s own diagnostic is the fallback'
        $known = Get-QBRepoBuildPacket -Path $log -TimeLimitMinutes 60
        Assert ($known.timedOut -eq $true -and $known.timeLimitMinutes -eq 60 -and $known.limitSource -match 'job''s own limit' -and ($known.repoAtTimeout -join ',') -eq 'runtime,emsdk') "a stated 60-minute limit makes the 58-minute leg a timeout: $($known | ConvertTo-Json -Compress)"
        $unknown = Get-QBRepoBuildPacket -Path $log -TimeLimitMinutes $null
        Assert ($unknown.timedOut -eq $false -and $unknown.uncertain -eq $true -and $unknown.limitSource -match 'assumed' -and $unknown.statement -match 'uncertain') "an unknown limit is uncertain, not a failure: $($unknown | ConvertTo-Json -Compress)"
    }
    finally { Remove-Item -LiteralPath $directory -Recurse -Force -ErrorAction SilentlyContinue }
}
Check 'The triage entry script gives the same answer as the functions, as text and as JSON' {
    $path = Join-Path ([IO.Path]::GetTempPath()) ('vmrqb-timeout-' + [guid]::NewGuid().ToString('N') + '.log')
    try {
        [IO.File]::WriteAllLines($path, $timedOutLeg)
        $entry = Join-Path $PSScriptRoot '..\triage\Invoke-Triage.ps1'
        $json = (& pwsh -NoProfile -File $entry timeout -LogPath $path -Json 2>&1 | Out-String) | ConvertFrom-Json
        Assert ($LASTEXITCODE -eq 0) 'the entry exits 0'
        Assert ((@($json.repoAtTimeout) -join ',') -eq 'runtime,emsdk') "entry named: $(@($json.repoAtTimeout) -join ',')"
        $text = & pwsh -NoProfile -File $entry timeout -LogPath $path 2>&1 | Out-String
        Assert ($text -match 'repoAtTimeout' -and $text -match 'runtime') 'the text view lists the suspects'
        $null = & pwsh -NoProfile -File $entry timeout -LogPath (Join-Path ([IO.Path]::GetTempPath()) 'no-such-vmrqb.log') 2>&1
        Assert ($LASTEXITCODE -ne 0) 'a missing log fails loudly'
    }
    finally { Remove-Item $path -Force -ErrorAction SilentlyContinue }
}

if ($failures) { Write-Host "$failures repo-build test(s) failed." -ForegroundColor Red; exit 1 }
Write-Host '13 offline repo-build tests. Synthetic logs only; no network, email, or external changes.' -ForegroundColor Green
