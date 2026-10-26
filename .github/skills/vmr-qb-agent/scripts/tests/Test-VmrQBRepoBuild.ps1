[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot '..\common\core.ps1')
. (Join-Path $PSScriptRoot '..\triage\timeout.ps1')

$failures = 0
function Check([string] $Name, [scriptblock] $Body) {
    try { & $Body; Write-Host "PASS: $Name" -ForegroundColor Green }
    catch { $script:failures++; Write-Host "FAIL: $Name -> $($_.Exception.Message)" -ForegroundColor Red }
}
function Assert([bool] $Condition, [string] $Message) { if (-not $Condition) { throw $Message } }

# The synthetic logs live in a throwaway folder under the system temp folder.
$scratch = Join-Path ([IO.Path]::GetTempPath()) ('repobuild-test-' + [guid]::NewGuid().ToString('N'))
[void][IO.Directory]::CreateDirectory($scratch)
$script:logCount = 0
function Log([string[]] $Lines) { $script:logCount++; $path = Join-Path $scratch "leg$($script:logCount).log"; [IO.File]::WriteAllLines($path, $Lines); $path }
# One AzDO log line: the UTC prefix, the agent's bracketed clock and the text.
function L([string] $Utc, [string] $Text) { "2026-10-05T${Utc}0000Z   [$Utc] $Text" }
function Judge([string] $Path, [hashtable] $Arguments = @{}) { Get-VmrQBTimeoutAttribution -State (Get-VmrQBRepoBuildState -Path $Path) @Arguments }
# A leg of the given length: runtime and emsdk start and never finish.
function New-Leg([int] $Minutes) {
    $end = [datetime]::new(2026, 10, 5, 10, 0, 0, [DateTimeKind]::Utc).AddMinutes($Minutes)
    Log @((L '10:00:00.0' 'Building runtime'), (L '10:00:01.0' 'Building emsdk'), "$($end.ToString('yyyy-MM-ddTHH:mm:ss.fffffff'))Z ##[section]Finishing: Linux_x64")
}

try {
    Check 'A leg cut off mid-build names the repos that were running, not the one that only logged an error' {
        # aspnetcore logs an unrelated error but finishes; runtime and emsdk are still building when the log ends 150 minutes in.
        $leg = Log @((L '10:00:00.0' 'Building arcade'), (L '10:01:00.0' 'Building arcade...done'), (L '10:01:01.0' 'Building aspnetcore'),
            (L '10:01:02.0' 'Building runtime'), (L '10:01:03.0' 'Building emsdk'),
            '2026-10-05T10:20:00.0000000Z D:\a\_work\1\s\src\aspnetcore\eng\build.cmd(12,3): error MSB3073: command exited with code 1',
            (L '10:30:00.0' 'Building aspnetcore...done'), '2026-10-05T12:30:00.0000000Z ##[section]Finishing: Linux_x64')
        $result = Judge $leg @{ JobTimedOut = $true }
        Assert (($result.repoAtTimeout -join ',') -eq 'runtime,emsdk') "suspects=$($result.repoAtTimeout -join ',')"
        Assert ($result.repoAtTimeout -notcontains 'aspnetcore') 'aspnetcore must never be named for a timeout it did not cause'
        Assert ($result.errorOnlyRepos -contains 'aspnetcore') 'the error-only repo must be reported separately'
        Assert ($result.statement -match 'do not attribute the timeout to them') 'the statement must warn against attributing to error-only repos'
        Assert ($result.statement -match 'runtime \(running 149 min\)') "running time missing or wrong: $($result.statement)"
    }

    Check 'Late starters are bystanders; task-log length does not establish a job timeout' {
        $ran = Judge (Log @((L '10:00:00.0' 'Building runtime'), (L '10:01:00.0' 'Building runtime...done'), (L '12:27:00.0' 'Building sdk'), '2026-10-05T12:30:00.0000000Z ##[section]Finishing: Linux_x64')) @{ JobTimedOut = $true }
        Assert ($ran.timedOut -and $ran.repoAtTimeout.Count -eq 0 -and $ran.bystanders -contains 'sdk') "a repo that ran 3 minutes was named: $($ran.repoAtTimeout -join ',')"
        Assert ($ran.statement -match 'none of them is the cause') $ran.statement
        # sdk had been running 55 minutes in a leg that FAILED; the log stopped far short of the 150-minute limit.
        $failed = Judge (Log @((L '10:00:00.0' 'Building sdk'), '2026-10-05T10:55:00.0000000Z ##[error]Build FAILED.', '2026-10-05T10:55:01.0000000Z ##[section]Finishing: Build'))
        Assert (-not $failed.timedOut -and $failed.repoAtTimeout.Count -eq 0) 'no repo may be blamed for a timeout that did not happen'
        Assert ($failed.uncertain -and $failed.statement -match 'spans 55 minutes' -and $failed.statement -match 'did not establish a timeout') $failed.statement
    }

    Check 'A second pass of the same repo is tracked as still running' {
        $state = Get-VmrQBRepoBuildState -Path (Log @((L '10:00:00.0' 'Building runtime'), (L '10:10:00.0' 'Building runtime...done'), (L '10:20:00.0' 'Building runtime'), '2026-10-05T11:20:00.0000000Z ##[section]Finishing: Windows_x86'))
        $runtime = @($state.repos | Where-Object repo -eq 'runtime')[0]
        Assert ($runtime.open -eq 1 -and $runtime.running -and [int]$runtime.runningSeconds -eq 3600) "open=$($runtime.open) running=$($runtime.runningSeconds)s"
    }

    Check 'A log with no repo markers says so instead of inventing an answer' {
        foreach ($leg in @((Log @('2026-10-05T10:00:00.0000000Z hello', '2026-10-05T10:00:01.0000000Z world')), (Log @('plain text', 'no timestamps')), (Log @()))) {
            $state = Get-VmrQBRepoBuildState -Path $leg
            Assert (-not $state.hasMarkers -and -not $state.reliable) 'no markers means nothing reliable can be said'
            $result = Get-VmrQBTimeoutAttribution -State $state
            Assert ($result.repoAtTimeout.Count -eq 0 -and $result.statement -match 'not a repo-build log') $result.statement
        }
    }

    Check 'Job evidence confirms a timeout; an assumed limit and log duration do not' {
        $short = Get-VmrQBRepoBuildState -Path (New-Leg 58)
        $at60 = Get-VmrQBTimeoutAttribution -State $short -TimeLimitMinutes 60 -JobTimedOut
        Assert ($at60.timedOut -and ($at60.repoAtTimeout -join ',') -eq 'runtime,emsdk' -and -not $at60.limitAssumed -and $at60.timeLimitMinutes -eq 60) "60-minute limit: $($at60 | ConvertTo-Json -Compress)"
        foreach ($limit in 150, 240) {
            $other = Get-VmrQBTimeoutAttribution -State $short -TimeLimitMinutes $limit
            Assert (-not $other.timedOut -and $other.uncertain) 'A configured limit alone does not prove a job timeout.'
        }
        $assumed = Get-VmrQBTimeoutAttribution -State $short -LimitIsAssumed
        Assert (-not $assumed.timedOut -and $assumed.uncertain -and $assumed.limitAssumed -and $assumed.repoAtTimeout.Count -eq 0) 'an assumed limit with a short log is uncertain and names no repo'
        Assert ($assumed.statement -match 'uncertain' -and $assumed.statement -notmatch 'ended by failure or cancellation') "an assumed limit must not turn into a different cause: $($assumed.statement)"
        $long = Get-VmrQBRepoBuildState -Path (New-Leg 149)
        $stated = Get-VmrQBTimeoutAttribution -State $long -TimeLimitMinutes 150 -JobTimedOut
        Assert ($stated.timedOut -and -not $stated.uncertain -and ($stated.repoAtTimeout -join ',') -eq 'runtime,emsdk') 'a 149-minute log is a timeout at a stated 150 and is never uncertain'
        $assumedLong = Get-VmrQBTimeoutAttribution -State $long -LimitIsAssumed
        Assert (-not $assumedLong.timedOut -and $assumedLong.uncertain -and $assumedLong.repoAtTimeout.Count -eq 0) 'a long log alone must not establish a timeout'
    }

    Check 'The daily packet uses the limit the job itself reported, per job id, and says when it had to assume one' {
        . (Join-Path $PSScriptRoot '..\daily\report.ps1')
        $tasks = @(
            [ordered]@{ task = 'Linux_x64'; jobId = 'job-short'; recordType = 'Job'; issues = @('The job running on agent A ran longer than the maximum time of 60 minutes.') },
            [ordered]@{ task = 'Linux_arm64'; jobId = 'job-other'; recordType = 'Job'; issues = @('The job running on agent B ran longer than the maximum time of 240 minutes.') })
        Assert ((Get-QBJobTimeLimitMinutes -JobId 'job-short' -Task $tasks -Log $null) -eq 60) 'the job''s own limit comes from its own record'
        Assert ((Get-QBJobTimeLimitMinutes -JobId 'job-other' -Task $tasks -Log $null) -eq 240) 'another job keeps its own limit'
        Assert ($null -eq (Get-QBJobTimeLimitMinutes -JobId 'job-unknown' -Task $tasks -Log $null)) 'a job that never reported a limit has none, not 150'
        $fromLog = [pscustomobject]@{ recordType = 'Job'; diagnosticExcerpt = 'The job running on agent C ran longer than the maximum time of 90 minutes.' }
        Assert ((Get-QBJobTimeLimitMinutes -JobId 'job-unknown' -Task $tasks -Log $fromLog) -eq 90) 'the log''s own diagnostic is the fallback'
        $leg = New-Leg 40
        $known = Get-QBRepoBuildPacket -Path $leg -TimeLimitMinutes 60
        Assert ($known.timedOut -eq $true -and $known.timeLimitMinutes -eq 60 -and $known.limitSource -match 'job''s own limit' -and ($known.repoAtTimeout -join ',') -eq 'runtime,emsdk') "a confirmed 60-minute job timeout also applies to its 40-minute task log: $($known | ConvertTo-Json -Compress)"
        $unknown = Get-QBRepoBuildPacket -Path $leg -TimeLimitMinutes $null
        Assert ($unknown.timedOut -eq $false -and $unknown.uncertain -eq $true -and $unknown.limitSource -match 'assumed' -and $unknown.statement -match 'uncertain') "an unknown limit is uncertain, not a failure: $($unknown | ConvertTo-Json -Compress)"
    }
}
finally { Remove-Item -LiteralPath $scratch -Recurse -Force -ErrorAction SilentlyContinue }

if ($failures) { Write-Host "$failures repo-build test(s) failed." -ForegroundColor Red; exit 1 }
Write-Host '6 offline repo-build tests. Synthetic logs only; no network, email, or external changes.' -ForegroundColor Green
