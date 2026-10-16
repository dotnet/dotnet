[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# The evidence digest is what the QB gets when the overall report could not be produced: the scripts list the problem builds, the
# coverage and what each report did, without the model. It must never throw on missing or damaged evidence, never present an
# unreadable pipeline as healthy, and keep every table row a single, valid row.
$scripts = Split-Path -Parent $PSScriptRoot
. (Join-Path $scripts 'common\core.ps1')
. (Join-Path $scripts 'daily\digest.ps1')

$failures = 0
function Check([string] $Name, [scriptblock] $Body) {
    try { & $Body; Write-Host "PASS: $Name" -ForegroundColor Green }
    catch { $script:failures++; Write-Host "FAIL: $Name -> $($_.Exception.Message)" -ForegroundColor Red }
}
function Assert($Condition, [string] $Message) { if (-not $Condition) { throw $Message } }

$sandbox = Join-Path ([IO.Path]::GetTempPath()) ('vmrqb-digest-' + [guid]::NewGuid().ToString('N'))
[void][IO.Directory]::CreateDirectory($sandbox)

function New-Build([string] $Org, [string] $Project, [int] $Id, [string] $Result, [string] $Finish, [string] $Branch = 'refs/heads/main', [object[]] $Tasks = @()) {
    return [ordered]@{ organization = $Org; project = $Project; buildId = $Id; sourceBranch = $Branch; result = $Result; finishTime = $Finish; tasks = @($Tasks) }
}
function New-Task([string] $Job, [string] $Task, [string] $Result, [string[]] $Errors = @()) {
    return [ordered]@{ job = $Job; task = $Task; result = $Result; issues = @($Errors | ForEach-Object { [ordered]@{ type = 'error'; message = $_ } }) }
}
function New-Evidence([string] $Name, [object[]] $Builds, [object[]] $Logs = @(), [object[]] $Pipelines = $null, [object[]] $Errors = @()) {
    $dir = Join-Path $sandbox $Name
    [void][IO.Directory]::CreateDirectory($dir)
    $failurePath = Join-Path $dir 'failure-tasks.json'; $logsPath = Join-Path $dir 'root-logs.json'
    [IO.File]::WriteAllText($failurePath, (ConvertTo-Json @{ builds = @($Builds) } -Depth 20))
    [IO.File]::WriteAllText($logsPath, (ConvertTo-Json @{ logs = @($Logs) } -Depth 20))
    if ($null -eq $Pipelines) {
        $counts = [ordered]@{ completed = 9; failed = 1; canceled = 0; partiallySucceeded = 0 }
        $Pipelines = @(
            [ordered]@{ name = 'internal'; status = 'available'; counts = $counts; error = $null; loginCommand = $null }
            [ordered]@{ name = 'public'; status = 'available'; counts = $counts; error = $null; loginCommand = $null })
    }
    return [pscustomobject]@{ pipelines = @($Pipelines); failureTasksPath = $failurePath; rootLogsPath = $logsPath; requiredErrors = @($Errors) }
}
function Get-Section([string] $Markdown, [string] $Heading) {
    $match = [regex]::Match($Markdown, '(?ms)^## ' + [regex]::Escape($Heading) + '[ \t]*\r?\n(.*?)(?=^## |\z)')
    if (-not $match.Success) { throw "no section '$Heading'" }
    return $match.Groups[1].Value
}
function Get-Rows([string] $Section) { return @($Section -split "`n" | Where-Object { $_ -match '^\| \[' }) }

try {
    Check 'Problem builds are listed per pipeline, newest first, with a link, the failed job and task, and the first error; builds that succeeded are left out' {
        $builds = @(
            (New-Build 'dnceng' 'internal' 3001 'failed' '2026-10-09T10:00:00Z' 'refs/heads/main' @((New-Task 'Build_Linux' 'Build' 'failed' @('error CS1002: ; expected')), (New-Task 'Build_Linux' 'Cleanup' 'succeeded')))
            (New-Build 'dnceng' 'internal' 3002 'succeeded' '2026-10-09T11:00:00Z')
            (New-Build 'dnceng-public' 'public' 4001 'canceled' '2026-10-09T08:00:00Z' 'refs/heads/release/10.0' @((New-Task 'Run Tests' 'Run Tests' 'canceled')))
            (New-Build 'dnceng-public' 'public' 4002 'partiallySucceeded' '2026-10-09T12:00:00Z'))
        $evidence = New-Evidence 'complete' $builds
        $results = @(
            [pscustomobject]@{ name = 'overall-summary'; status = 'failed'; path = $null; error = 'The model timed out after 60 minutes.' }
            [pscustomobject]@{ name = 'build-health'; status = 'succeeded'; path = 'C:\x\build-health.md'; error = $null })
        $digest = New-QBEvidenceDigest -Date '2026-10-09' -Evidence $evidence -IssueEvidence $null -Results $results
        Assert ($digest.StartsWith("# VMR QB evidence digest - 2026-10-09`n") -and $digest.EndsWith("`n") -and $digest -match 'Generated without the model') 'title and the how-it-was-made line'
        $internal = Get-Section $digest 'Internal builds (dnceng/internal)'
        $public = Get-Section $digest 'Public builds (dnceng-public/public)'
        Assert ($digest.IndexOf('## Internal builds') -lt $digest.IndexOf('## Public builds')) 'internal comes first'
        $row = @(Get-Rows $internal)
        Assert ($row.Count -eq 1 -and $row[0] -match '\[3001\]\(https://dev\.azure\.com/dnceng/internal/_build/results\?buildId=3001\)') 'one internal row, linked'
        Assert ($row[0] -match '\| main \| failed \| Build_Linux / Build \| error CS1002: ; expected \|$') 'branch without refs/heads, result, job / task, first error'
        Assert ($internal -notmatch '3002' -and $internal -notmatch 'Cleanup') 'a build that succeeded and a task that succeeded are not listed'
        $publicRows = @(Get-Rows $public)
        Assert ($publicRows.Count -eq 2 -and $publicRows[0] -match '\[4002\]' -and $publicRows[1] -match '\[4001\]') 'public builds are newest first'
        Assert ($publicRows[0] -match 'partiallySucceeded' -and $publicRows[0] -match 'no failed task in the collected timeline') 'a build with no failed task says so'
        Assert ($publicRows[1] -match 'release/10\.0' -and $publicRows[1] -match 'Run Tests \| ' -and $publicRows[1] -notmatch 'Run Tests / Run Tests') 'a job named like its task is not doubled'
        $reports = Get-Section $digest 'Reports'
        Assert ($reports -match '\| overall-summary \| not generated \| The model timed out after 60 minutes\. \|' -and $reports -match '\| build-health \| generated \| build-health\.md \|') 'what each report did'
        $coverage = Get-Section $digest 'Coverage'
        Assert ($coverage -match '\| internal \| complete \| 9 \| 1 \| 0 \| 0 \|' -and $coverage -match '\| public \| complete \|') 'coverage per pipeline'
        Assert ($digest -notmatch 'Evidence gaps') 'no gaps, no gap section'
    }

    Check 'Rows stay single rows: pipes, angle brackets, line breaks and long text in evidence cannot break the tables' {
        $nasty = New-Task 'Job | with pipe' 'Task <b>x</b>' 'failed' @("first line | pipe <script>alert(1)</script>`nsecond line " + ('y' * 400))
        $evidence = New-Evidence 'nasty' @((New-Build 'dnceng' 'internal' 3101 'failed' '2026-10-09T10:00:00Z' 'refs/heads/feature|x' @($nasty)))
        $digest = New-QBEvidenceDigest -Date '2026-10-09' -Evidence $evidence -IssueEvidence $null -Results @()
        $rows = @(Get-Rows (Get-Section $digest 'Internal builds (dnceng/internal)'))
        Assert ($rows.Count -eq 1) 'one row'
        $cells = @($rows[0].Trim('|') -split '\|')
        Assert ($cells.Count -eq 5) "five cells, got $($cells.Count): $($rows[0])"
        Assert ($rows[0] -notmatch '[<>]' -and $rows[0] -match 'first line / pipe \(script\)' -and $rows[0] -match 'second line') 'pipes and angle brackets are replaced, the line break is one space'
        Assert ($rows[0].Length -lt 900 -and $rows[0] -match '\.\.\. \|$') 'long text is cut with an ellipsis'
    }

    Check 'The first error comes from the collected log excerpt when the task has no error issue of its own' {
        $logs = @([ordered]@{ buildId = 4201; actualBuildId = 4201; diagnosticExcerpt = "[warning] ignore me`n##[error]Process completed with exit code 137.`n##[error]second" })
        $evidence = New-Evidence 'logfallback' @((New-Build 'dnceng-public' 'public' 4201 'failed' '2026-10-09T10:00:00Z' 'refs/heads/main' @((New-Task 'Build' 'Build' 'failed')))) $logs
        $digest = New-QBEvidenceDigest -Date '2026-10-09' -Evidence $evidence -IssueEvidence $null -Results @()
        $row = @(Get-Rows (Get-Section $digest 'Public builds (dnceng-public/public)'))
        Assert ($row.Count -eq 1 -and $row[0] -match 'Process completed with exit code 137\. \|$' -and $row[0] -notmatch 'second') 'the first ##[error] line of the excerpt'
    }

    Check 'A pipeline that could not be read is UNKNOWN with its sign-in fix, never described as healthy' {
        $pipelines = @(
            [ordered]@{ name = 'internal'; status = 'authentication_required'; counts = $null; error = 'Azure DevOps sign-in is required. Azure DevOps sign-in is required.'; loginCommand = 'az login --allow-no-subscriptions' }
            [ordered]@{ name = 'public'; status = 'available'; counts = [ordered]@{ completed = 3; failed = 0; canceled = 0; partiallySucceeded = 0 }; error = $null; loginCommand = $null })
        $evidence = New-Evidence 'auth' @() @() $pipelines
        $digest = New-QBEvidenceDigest -Date '2026-10-09' -Evidence $evidence -IssueEvidence $null -Results @()
        $coverage = Get-Section $digest 'Coverage'
        $internalRow = @($coverage -split "`n" | Where-Object { $_ -match '^\| internal ' })[0]
        Assert ($internalRow -match 'UNKNOWN, not healthy' -and $internalRow -match 'Fix: az login --allow-no-subscriptions' -and $internalRow -match '\| unknown \| unknown \|') 'UNKNOWN, the fix, unknown counts'
        Assert (([regex]::Matches($internalRow, 'sign-in is required\.')).Count -eq 1) 'the repeated sentence is said once'
        $internal = Get-Section $digest 'Internal builds (dnceng/internal)'
        Assert ($internal -match 'not fully collected' -and $internal -notmatch 'No failed, canceled') 'the internal section does not claim there were no failures'
        $public = Get-Section $digest 'Public builds (dnceng-public/public)'
        Assert ($public -match 'No failed, canceled or partially succeeded builds in the window\.') 'a fully collected pipeline with no problem builds says so'
    }

    Check 'At most 25 problem builds are listed per pipeline, newest first, with the rest counted' {
        $builds = @(1..30 | ForEach-Object { New-Build 'dnceng' 'internal' (5000 + $_) 'failed' ([DateTime]::Parse('2026-10-08T00:00:00Z').ToUniversalTime().AddHours($_).ToString('o')) })
        $evidence = New-Evidence 'cap' $builds
        $digest = New-QBEvidenceDigest -Date '2026-10-09' -Evidence $evidence -IssueEvidence $null -Results @()
        $internal = Get-Section $digest 'Internal builds (dnceng/internal)'
        $rows = @(Get-Rows $internal)
        Assert ($rows.Count -eq 25) "25 rows, got $($rows.Count)"
        Assert ($rows[0] -match '\[5030\]' -and $rows[24] -match '\[5006\]' -and $internal -notmatch '\[5005\]') 'the newest 25 are kept'
        Assert ($internal -match '\.\.\. and 5 more problem builds in the collected evidence\.') 'the rest is counted'
    }

    Check 'Missing, unreadable or corrupt evidence never throws and is said, not hidden' {
        $none = New-QBEvidenceDigest -Date '2026-10-09' -Evidence $null -IssueEvidence $null -Results @()
        Assert ($none -match '\| internal \| no evidence \|' -and $none -match '\| public \| no evidence \|' -and $none -match '\| \(none\) \| - \| - \|') 'no evidence at all'
        Assert (([regex]::Matches($none, 'The failure evidence could not be read')).Count -eq 2) 'both build sections say the failure evidence is unreadable'
        $corrupt = New-Evidence 'corrupt' @()
        [IO.File]::WriteAllText($corrupt.failureTasksPath, '{ nope')
        $digest = New-QBEvidenceDigest -Date '2026-10-09' -Evidence $corrupt -IssueEvidence $null -Results @()
        Assert (([regex]::Matches($digest, 'The failure evidence could not be read')).Count -eq 2) 'a corrupt failure file is said'
        $missingFile = New-Evidence 'missingfile' @()
        Remove-Item -LiteralPath $missingFile.failureTasksPath -Force
        Assert ((New-QBEvidenceDigest -Date '2026-10-09' -Evidence $missingFile -IssueEvidence $null -Results @()) -match 'could not be read') 'a missing failure file is said'
        $logsGone = New-Evidence 'logsgone' @((New-Build 'dnceng' 'internal' 3301 'failed' '2026-10-09T10:00:00Z' 'refs/heads/main' @((New-Task 'Build' 'Build' 'failed'))))
        [IO.File]::WriteAllText($logsGone.rootLogsPath, '{ nope')
        $partial = New-QBEvidenceDigest -Date '2026-10-09' -Evidence $logsGone -IssueEvidence $null -Results @()
        Assert (@(Get-Rows (Get-Section $partial 'Internal builds (dnceng/internal)')).Count -eq 1) 'corrupt log excerpts only cost the first-error column'
        $odd = [pscustomobject]@{ pipelines = 'not a list'; failureTasksPath = 5; rootLogsPath = $null; requiredErrors = 7 }
        [void](New-QBEvidenceDigest -Date '2026-10-09' -Evidence $odd -IssueEvidence ([pscustomobject]@{ errors = 3 }) -Results @($null))
    }

    Check 'Collector gaps and issue-evidence errors are listed, with the first ten and a count of the rest' {
        $errors = @(1..12 | ForEach-Object { [ordered]@{ scope = 'internal'; message = "Timeline for build $_ was unavailable." } })
        $evidence = New-Evidence 'gaps' @() @() $null $errors
        $issues = [pscustomobject]@{ errors = @([pscustomobject]@{ message = 'GitHub issue search was rate limited.' }) }
        $digest = New-QBEvidenceDigest -Date '2026-10-09' -Evidence $evidence -IssueEvidence $issues -Results @()
        $gaps = Get-Section $digest 'Evidence gaps'
        Assert (([regex]::Matches($gaps, '(?m)^- internal: Timeline for build')).Count -eq 10 -and $gaps -match '\.\.\. and 2 more') 'ten gaps and a count'
        Assert ($digest -match 'Issue and owner evidence was incomplete: GitHub issue search was rate limited\.') 'issue evidence errors'
    }

    Check 'The run notes go under the digest title and come out again exactly' {
        $digest = New-QBEvidenceDigest -Date '2026-10-09' -Evidence $null -IssueEvidence $null -Results @()
        Clear-QBRunNotes
        Add-QBRunNote -Source 'overall-report' -Message 'The overall report was not produced.' -Fix 'Re-run later.'
        $with = Set-QBRunNotesBlock -Content $digest
        $lines = @($with -split "`n")
        Assert ($lines[0] -like '# VMR QB evidence digest*' -and $lines[2] -eq '<!-- VMR-QB-RUN-NOTES -->') 'the block sits under the title'
        Assert ((Remove-QBRunNotesBlock -Content $with) -ceq $digest) 'and is removable'
        Clear-QBRunNotes
    }
}
finally { Remove-Item -LiteralPath $sandbox -Recurse -Force -ErrorAction SilentlyContinue }

if ($failures) { Write-Host "$failures digest test(s) failed." -ForegroundColor Red; exit 1 }
Write-Host 'All digest tests passed.' -ForegroundColor Green
