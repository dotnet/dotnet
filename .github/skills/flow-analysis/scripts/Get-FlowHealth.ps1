<#
.SYNOPSIS
    Batch-scan codeflow PR health across branches for a repository.

.DESCRIPTION
    Scans all backflow and forward flow PRs for a repository, gathering
    health signals in parallel. Designed to complement the flow-analysis
    skill which uses maestro MCP tools for subscription/build data.

    This script handles the GitHub-side batch operations that require
    many parallel API calls - something an agent cannot do efficiently
    with sequential MCP tool calls.

    Maestro-side data (subscriptions, builds, channels, freshness) should
    be gathered separately using maestro MCP tools.

.PARAMETER Repository
    Target repository in owner/repo format (e.g., dotnet/sdk).

.PARAMETER Branch
    Optional. Only scan a specific branch instead of all discovered branches.

.EXAMPLE
    ./Get-FlowHealth.ps1 -Repository "dotnet/sdk"

.EXAMPLE
    ./Get-FlowHealth.ps1 -Repository "dotnet/sdk" -Branch "main"
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory=$true)]
    [string]$Repository,

    [string]$Branch
)

$ErrorActionPreference = "Continue"

# Threshold for considering a closed preview PR as "released" rather than "missing"
$ReleasedPreviewThresholdDays = 14

# --- Parallel support detection ---
$script:canParallel = $null -ne (Get-Command Start-ThreadJob -ErrorAction SilentlyContinue)

function Start-AsyncGh {
    param([string[]]$GhArgs)
    if (-not $script:canParallel) { return $null }
    return Start-ThreadJob -ScriptBlock {
        param($args_)
        $result = gh @args_ 2>$null
        if ($LASTEXITCODE -ne 0) { return $null }
        return ($result -join "`n")
    } -ArgumentList (,@($GhArgs))
}

function Complete-AsyncJob {
    param($Job, [scriptblock]$FallbackBlock)
    if ($Job) { return Receive-Job -Job $Job -Wait -AutoRemoveJob }
    return & $FallbackBlock
}

function Get-ShortSha {
    param([string]$Sha, [int]$Length = 12)
    if (-not $Sha) { return "(unknown)" }
    return $Sha.Substring(0, [Math]::Min($Length, $Sha.Length))
}

# --- Validate ---
if ($Repository -notmatch '^[A-Za-z0-9_.-]+/[A-Za-z0-9_.-]+$') {
    Write-Error "Repository must be in format 'owner/repo'"
    return
}
if (-not (Get-Command gh -ErrorAction SilentlyContinue)) {
    Write-Error "GitHub CLI (gh) is required. Install from https://cli.github.com/"
    return
}

# --- Search for codeflow PRs ---
Write-Host "🔍 Searching for codeflow PRs in $Repository..." -ForegroundColor Cyan

# Use gh pr list (REST API, reliable) instead of gh search prs (search index, can lag)
# Then filter client-side for codeflow PRs (title contains "Source code updates from dotnet/dotnet")
# Use --search instead of --author to avoid PS 5.1 bracket-mangling with [bot]
$allOpenJson = gh pr list --repo $Repository --search "author:app/dotnet-maestro" --state open --json number,title --limit 100 2>$null
$openPRs = @()
if ($LASTEXITCODE -eq 0 -and $allOpenJson) {
    try {
        $allOpen = ($allOpenJson -join "`n") | ConvertFrom-Json
        $openPRs = @($allOpen | Where-Object { $_.title -match 'Source code updates from dotnet/dotnet' })
    } catch { $openPRs = @() }
}

# For merged PRs, gh pr list doesn't support --merged directly, so use gh search prs
# (search lag is less critical for merged PRs since they're historical context)
# Use app/ author syntax to avoid PS 5.1 bracket-mangling with [bot]
$mergedPRsJson = gh search prs --repo $Repository --author "app/dotnet-maestro" --state closed --merged "Source code updates from dotnet/dotnet" --limit 30 --sort updated --json number,title,closedAt 2>$null
$mergedPRs = @()
if ($LASTEXITCODE -eq 0 -and $mergedPRsJson) {
    try { $mergedPRs = ($mergedPRsJson -join "`n") | ConvertFrom-Json } catch { $mergedPRs = @() }
}

Write-Host "  ✅ Found $($openPRs.Count) open, $($mergedPRs.Count) merged codeflow PRs" -ForegroundColor Green

# --- Map open PRs by branch ---
$openBranches = @{}
foreach ($opr in $openPRs) {
    if ($opr.title -match '^\[([^\]]+)\]') {
        $openBranches[$Matches[1]] = $opr.number
    }
}

# --- Group merged PRs by branch (most recent per branch) ---
$branchLastMerged = @{}
foreach ($mpr in $mergedPRs) {
    if ($mpr.title -match '^\[([^\]]+)\]') {
        $branchName = $Matches[1]
        if ($Branch -and $branchName -ne $Branch) { continue }
        if (-not $branchLastMerged.ContainsKey($branchName) -or
            ($mpr.closedAt -and $branchLastMerged[$branchName].closedAt -and $mpr.closedAt -gt $branchLastMerged[$branchName].closedAt)) {
            $branchLastMerged[$branchName] = $mpr
        }
    }
}

# --- Parallel fetch: PR bodies for merged PRs (extract VMR metadata) ---
Write-Host "📦 Fetching PR metadata and VMR branch data..." -ForegroundColor Cyan
$prBodyJobs = @{}
foreach ($branchName in ($branchLastMerged.Keys | Sort-Object)) {
    if ($openBranches.ContainsKey($branchName)) { continue }
    $lastPR = $branchLastMerged[$branchName]
    $prBodyJobs[$branchName] = Start-AsyncGh @('pr', 'view', $lastPR.number.ToString(), '-R', $Repository, '--json', 'body')
}

# --- Parallel fetch: open PR health data ---
$healthJobs = @{}
foreach ($branchName in ($openBranches.Keys | Sort-Object)) {
    if ($Branch -and $branchName -ne $Branch) { continue }
    $healthJobs[$branchName] = Start-AsyncGh @('pr', 'view', $openBranches[$branchName].ToString(), '-R', $Repository, '--json', 'body,comments,updatedAt,mergeable,statusCheckRollup')
}

# --- Collect PR body results and extract VMR metadata ---
$vmrBranches = @{}
$vmrCommits = @{}
foreach ($branchName in ($branchLastMerged.Keys | Sort-Object)) {
    if ($openBranches.ContainsKey($branchName)) { continue }
    $result = Complete-AsyncJob $prBodyJobs[$branchName] {
        $json = gh pr view $branchLastMerged[$branchName].number -R $Repository --json body 2>$null
        if ($LASTEXITCODE -ne 0) { return $null }
        return ($json -join "`n")
    }
    if (-not $result) { continue }
    try { $prDetail = $result | ConvertFrom-Json } catch { continue }
    if ($prDetail.body -match '\*\*Branch\*\*:\s*\[([^\]]+)\]') {
        $vmrBranches[$branchName] = $Matches[1]
    }
    if ($prDetail.body -match '\*\*Commit\*\*:\s*\[([a-fA-F0-9]+)\]') {
        $vmrCommits[$branchName] = $Matches[1]
    }
}

# --- Parallel fetch: VMR branch HEADs ---
Write-Host "🔗 Comparing VMR branch HEADs..." -ForegroundColor Cyan
$vmrHeadJobs = @{}
foreach ($branchName in $vmrBranches.Keys) {
    if (-not $vmrCommits.ContainsKey($branchName)) { continue }
    $encodedBranch = [uri]::EscapeDataString($vmrBranches[$branchName])
    $vmrHeadJobs[$branchName] = Start-AsyncGh @('api', "/repos/dotnet/dotnet/commits/$encodedBranch")
}

$vmrHeads = @{}
foreach ($branchName in $vmrHeadJobs.Keys) {
    $result = Complete-AsyncJob $vmrHeadJobs[$branchName] {
        $encoded = [uri]::EscapeDataString($vmrBranches[$branchName])
        $json = gh api "/repos/dotnet/dotnet/commits/$encoded" 2>$null
        if ($LASTEXITCODE -ne 0) { return $null }
        return ($json -join "`n")
    }
    if ($result) {
        try { $vmrHeads[$branchName] = ($result | ConvertFrom-Json) } catch { }
    }
}

# --- Collect open PR health ---
$openPRHealth = @{}
foreach ($branchName in $healthJobs.Keys) {
    $result = Complete-AsyncJob $healthJobs[$branchName] {
        $json = gh pr view $openBranches[$branchName] -R $Repository --json body,comments,updatedAt,mergeable,statusCheckRollup 2>$null
        if ($LASTEXITCODE -ne 0) { return $null }
        return ($json -join "`n")
    }
    if (-not $result) { continue }
    try {
        $prDetail = $result | ConvertFrom-Json
        $health = @{ status = "healthy"; hasConflict = $false; hasStaleness = $false }
        if ($prDetail.mergeable -eq 'CONFLICTING') { $health.hasConflict = $true }
        if ($prDetail.comments) {
            foreach ($comment in $prDetail.comments) {
                if ($comment.author.login -match '^dotnet-maestro') {
                    if ($comment.body -match 'codeflow cannot continue|the source repository has received code changes') { $health.hasStaleness = $true }
                    if ($comment.body -match 'Conflict detected') { $health.hasConflict = $true }
                }
            }
        }
        # CI status from statusCheckRollup
        # gh pr view returns a flat array; CheckRun items have conclusion+status, StatusContext items have state
        $rollup = $prDetail.statusCheckRollup
        if ($rollup -and $rollup.Count -gt 0) {
            $contexts = @($rollup)
            $failed = @($contexts | Where-Object { $_.conclusion -eq 'FAILURE' -or $_.conclusion -eq 'ERROR' -or $_.state -eq 'FAILURE' -or $_.state -eq 'ERROR' })
            $pending = @($contexts | Where-Object { ($_.status -eq 'IN_PROGRESS' -or $_.status -eq 'QUEUED' -or $_.status -eq 'PENDING') -or $_.state -eq 'PENDING' })
            $total = $contexts.Count
            if ($failed.Count -gt 0) {
                $health.ciStatus = "red"
                $health.ciFailedCount = $failed.Count
                $health.ciTotalCount = $total
            } elseif ($pending.Count -gt 0) {
                $health.ciStatus = "pending"
            } elseif ($total -gt 0) {
                $health.ciStatus = "green"
            } else {
                $health.ciStatus = "none"
            }
        } else {
            $health.ciStatus = "none"
        }

        if ($health.hasConflict) { $health.status = "conflict" }
        elseif ($health.hasStaleness) { $health.status = "stale" }
        elseif ($health.ciStatus -eq "red") { $health.status = "ci-red" }

        if ($prDetail.body -match '\(Begin:([a-f0-9-]+)\)'){
            $health.subscriptionId = $Matches[1]
        }
        if ($prDetail.body -match '\*\*Branch\*\*:\s*\[([^\]]+)\]') {
            $health.vmrBranch = $Matches[1]
        }

        $openPRHealth[$branchName] = $health
    } catch { }
}

# --- Parallel fetch: commit comparisons for branches missing PRs ---
$compareJobs = @{}
foreach ($branchName in $vmrCommits.Keys) {
    if ($openBranches.ContainsKey($branchName)) { continue }
    if (-not $vmrHeads.ContainsKey($branchName)) { continue }
    $vmrCommit = $vmrCommits[$branchName]
    $vmrHeadSha = $vmrHeads[$branchName].sha
    if ($vmrCommit -eq $vmrHeadSha -or $vmrHeadSha.StartsWith($vmrCommit) -or $vmrCommit.StartsWith($vmrHeadSha)) { continue }
    $compareJobs[$branchName] = Start-AsyncGh @('api', "/repos/dotnet/dotnet/compare/$vmrCommit...$vmrHeadSha")
}

$compareResults = @{}
foreach ($branchName in $compareJobs.Keys) {
    $vmrCommit = $vmrCommits[$branchName]
    $vmrHeadSha = $vmrHeads[$branchName].sha
    $result = Complete-AsyncJob $compareJobs[$branchName] {
        $json = gh api "/repos/dotnet/dotnet/compare/$vmrCommit...$vmrHeadSha" 2>$null
        if ($LASTEXITCODE -ne 0) { return $null }
        return ($json -join "`n")
    }
    if ($result) {
        try { $compareResults[$branchName] = ($result | ConvertFrom-Json) } catch { }
    }
}

# --- Forward flow scan ---
Write-Host "↔️ Scanning forward flow PRs..." -ForegroundColor Cyan
$repoShortName = $Repository -replace '^dotnet/', ''
$fwdPRsJson = gh search prs --repo dotnet/dotnet --author "app/dotnet-maestro" --state open "Source code updates from dotnet/$repoShortName" --json number,title --limit 10 2>$null
$fwdPRs = @()
if ($LASTEXITCODE -eq 0 -and $fwdPRsJson) {
    try {
        $all = ($fwdPRsJson -join "`n") | ConvertFrom-Json
        $fwdPRs = @($all | Where-Object { $_.title -match "from dotnet/$([regex]::Escape($repoShortName))$" })
    } catch { }
}

# Forward flow health (parallel)
$fwdHealthJobs = @{}
foreach ($fpr in $fwdPRs) {
    $fwdHealthJobs[$fpr.number] = Start-AsyncGh @('pr', 'view', $fpr.number.ToString(), '-R', 'dotnet/dotnet', '--json', 'comments,mergeable,statusCheckRollup')
}
$fwdHealth = @{}
foreach ($fpr in $fwdPRs) {
    $result = Complete-AsyncJob $fwdHealthJobs[$fpr.number] {
        $json = gh pr view $fpr.number -R dotnet/dotnet --json comments,mergeable,statusCheckRollup 2>$null
        if ($LASTEXITCODE -ne 0) { return $null }
        return ($json -join "`n")
    }
    if (-not $result) { continue }
    try {
        $detail = $result | ConvertFrom-Json
        $h = @{ status = "healthy"; hasConflict = $false; hasStaleness = $false }
        if ($detail.mergeable -eq 'CONFLICTING') { $h.hasConflict = $true }
        if ($detail.comments) {
            foreach ($c in $detail.comments) {
                if ($c.author.login -match '^dotnet-maestro') {
                    if ($c.body -match 'codeflow cannot continue|the source repository has received code changes') { $h.hasStaleness = $true }
                    if ($c.body -match 'Conflict detected') { $h.hasConflict = $true }
                }
            }
        }
        if ($h.hasConflict) { $h.status = "conflict" }
        elseif ($h.hasStaleness) { $h.status = "stale" }

        # CI status from statusCheckRollup (flat array with conclusion/status fields)
        $h.ciStatus = "none"
        $rollup = $detail.statusCheckRollup
        if ($rollup -and $rollup.Count -gt 0) {
            $ctx = @($rollup)
            $fail = @($ctx | Where-Object { $_.conclusion -eq 'FAILURE' -or $_.conclusion -eq 'ERROR' -or $_.state -eq 'FAILURE' -or $_.state -eq 'ERROR' })
            $pend = @($ctx | Where-Object { ($_.status -eq 'IN_PROGRESS' -or $_.status -eq 'QUEUED' -or $_.status -eq 'PENDING') -or $_.state -eq 'PENDING' })
            if ($fail.Count -gt 0) {
                $h.ciStatus = "red"; $h.ciFailedCount = $fail.Count; $h.ciTotalCount = $ctx.Count
            } elseif ($pend.Count -gt 0) { $h.ciStatus = "pending" }
            elseif ($ctx.Count -gt 0) { $h.ciStatus = "green" }
        }
        if ($h.status -eq "healthy" -and $h.ciStatus -eq "red") { $h.status = "ci-red" }

        $fwdHealth[$fpr.number] = $h
    } catch { }
}

# --- Build structured JSON output ---
$backflowBranches = @()

# Open PR branches
foreach ($branchName in ($openBranches.Keys | Sort-Object)) {
    if ($Branch -and $branchName -ne $Branch) { continue }
    $entry = [ordered]@{
        branch       = $branchName
        prNumber     = $openBranches[$branchName]
        prState      = "open"
    }
    $health = $openPRHealth[$branchName]
    if ($health) {
        $entry.status = $health.status
        $entry.hasConflict = $health.hasConflict
        $entry.hasStaleness = $health.hasStaleness
        $entry.ciStatus = $health.ciStatus
        if ($health.ciFailedCount) { $entry.ciFailedCount = $health.ciFailedCount; $entry.ciTotalCount = $health.ciTotalCount }
        if ($health.subscriptionId) { $entry.subscriptionId = $health.subscriptionId }
        if ($health.vmrBranch) { $entry.vmrBranch = $health.vmrBranch }
    } else {
        $entry.status = "unknown"
    }
    $backflowBranches += $entry
}

# Branches without open PRs (from merged PR history)
foreach ($branchName in ($branchLastMerged.Keys | Sort-Object)) {
    if ($Branch -and $branchName -ne $Branch) { continue }
    if ($openBranches.ContainsKey($branchName)) { continue }
    $lastPR = $branchLastMerged[$branchName]
    $entry = [ordered]@{
        branch         = $branchName
        lastMergedPR   = $lastPR.number
        lastMergedAt   = $lastPR.closedAt
    }
    if ($vmrBranches.ContainsKey($branchName)) { $entry.vmrBranch = $vmrBranches[$branchName] }
    if ($vmrCommits.ContainsKey($branchName)) { $entry.lastVmrCommit = Get-ShortSha $vmrCommits[$branchName] }

    $vmrHead = $vmrHeads[$branchName]
    if ($vmrHead) {
        $entry.vmrHeadSha = Get-ShortSha $vmrHead.sha
        $entry.vmrHeadDate = $vmrHead.commit.committer.date
    }

    $vmrCommit = $vmrCommits[$branchName]
    if ($vmrHead -and $vmrCommit) {
        if ($vmrCommit -eq $vmrHead.sha -or $vmrHead.sha.StartsWith($vmrCommit) -or $vmrCommit.StartsWith($vmrHead.sha)) {
            $entry.status = "up-to-date"
            $entry.aheadBy = 0
        } else {
            $compare = $compareResults[$branchName]
            $entry.aheadBy = if ($compare) { $compare.ahead_by } else { -1 }
            $isReleasedPreview = ($vmrBranches[$branchName] -match 'preview') -and
                $lastPR.closedAt -and
                (([DateTime]::UtcNow - [DateTimeOffset]::Parse($lastPR.closedAt).UtcDateTime).TotalDays -gt $ReleasedPreviewThresholdDays)
            if ($isReleasedPreview) {
                $entry.status = "released-preview"
            } else {
                $entry.status = "missing"
                $elapsed = if ($lastPR.closedAt) { [DateTime]::UtcNow - [DateTimeOffset]::Parse($lastPR.closedAt).UtcDateTime } else { $null }
                $entry.hoursSinceLastMerge = if ($elapsed) { [math]::Round($elapsed.TotalHours, 1) } else { $null }
            }
        }
    } else {
        $entry.status = "unknown"
    }
    $backflowBranches += $entry
}

$forwardFlowPRs = @()
foreach ($fpr in $fwdPRs) {
    $fprBranch = if ($fpr.title -match '^\[([^\]]+)\]') { $Matches[1] } else { "unknown" }
    if ($Branch -and $fprBranch -ne $Branch) { continue }
    $fEntry = [ordered]@{
        prNumber = $fpr.number
        branch   = $fprBranch
        title    = $fpr.title
    }
    $fh = $fwdHealth[$fpr.number]
    if ($fh) {
        $fEntry.status = $fh.status
        $fEntry.hasConflict = $fh.hasConflict
        $fEntry.hasStaleness = $fh.hasStaleness
        $fEntry.ciStatus = $fh.ciStatus
        if ($fh.ciFailedCount) { $fEntry.ciFailedCount = $fh.ciFailedCount; $fEntry.ciTotalCount = $fh.ciTotalCount }
    } else {
        $fEntry.status = "unknown"
    }
    $forwardFlowPRs += $fEntry
}

# --- Compute summary counts ---
$healthy = @($backflowBranches | Where-Object { $_.status -eq "healthy" }).Count
$upToDate = @($backflowBranches | Where-Object { $_.status -eq "up-to-date" -or $_.status -eq "released-preview" }).Count
$blocked = @($backflowBranches | Where-Object { $_.status -in @("conflict", "stale", "ci-red") }).Count
$missing = @($backflowBranches | Where-Object { $_.status -eq "missing" }).Count

$fwdHealthy = @($forwardFlowPRs | Where-Object { $_.status -eq "healthy" }).Count
$fwdStale = @($forwardFlowPRs | Where-Object { $_.status -eq "stale" }).Count
$fwdConflict = @($forwardFlowPRs | Where-Object { $_.status -eq "conflict" }).Count
$fwdCiRed = @($forwardFlowPRs | Where-Object { $_.status -eq "ci-red" }).Count

$output = [ordered]@{
    repository  = $Repository
    backflow    = [ordered]@{
        branches  = $backflowBranches
        summary   = [ordered]@{
            healthy   = $healthy
            upToDate  = $upToDate
            blocked   = $blocked
            missing   = $missing
        }
    }
    forwardFlow = [ordered]@{
        prs     = $forwardFlowPRs
        summary = [ordered]@{
            healthy    = $fwdHealthy
            stale      = $fwdStale
            conflicted = $fwdConflict
            ciRed      = $fwdCiRed
        }
    }
}

# --- Summary ---
$totalBranches = $backflowBranches.Count
$totalFwd = $forwardFlowPRs.Count
$problemCount = $blocked + $missing
if ($problemCount -eq 0 -and $fwdStale -eq 0 -and $fwdConflict -eq 0 -and $fwdCiRed -eq 0) {
    Write-Host "✅ ${Repository}: $totalBranches branches healthy, $totalFwd forward flow PRs" -ForegroundColor Green
} else {
    Write-Host "⚠️ ${Repository}: $problemCount backflow issues ($blocked blocked, $missing missing), $($fwdStale + $fwdConflict) forward flow issues" -ForegroundColor Yellow
}

# Output as JSON for the agent to consume
$output | ConvertTo-Json -Depth 5
