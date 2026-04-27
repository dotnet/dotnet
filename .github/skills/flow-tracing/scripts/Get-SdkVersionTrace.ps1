<#
.SYNOPSIS
    Traces which component commit SHA is included in a specific .NET SDK version.

.DESCRIPTION
    Given an SDK version string (e.g., 10.0.300-preview.26117.103), this script:
    1. Decodes the version to extract band, build date, and revision
    2. Maps to the correct VMR branch
    3. Finds the AzDO unified build that produced it
    4. Walks source-manifest.json and/or Version.Details.xml to find the component SHA

.PARAMETER SdkVersion
    Full SDK version string (e.g., "10.0.300-preview.26117.103").

.PARAMETER Component
    Component to trace. Default: "runtime". Options: runtime, aspnetcore, roslyn, sdk, fsharp, nuget.

.PARAMETER DecodeOnly
    Only decode the version string; don't trace the full dependency chain.

.PARAMETER CheckCommit
    One or more commit SHAs to check whether they are included (ancestors of) the
    resolved component SHA. Requires a local clone of the component repo.

.EXAMPLE
    ./Get-SdkVersionTrace.ps1 -SdkVersion "10.0.300-preview.26117.103"

.EXAMPLE
    ./Get-SdkVersionTrace.ps1 -SdkVersion "10.0.300-preview.26117.103" -Component "aspnetcore"

.EXAMPLE
    ./Get-SdkVersionTrace.ps1 -SdkVersion "10.0.300-preview.26117.103" -DecodeOnly
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string]$SdkVersion,

    [string]$Component = "runtime",

    [switch]$DecodeOnly,

    [string[]]$CheckCommit
)

$ErrorActionPreference = "Stop"

function Write-Section($title) {
    Write-Host ""
    Write-Host "=== $title ===" -ForegroundColor Cyan
}

function Write-Status($emoji, $message, $color) {
    Write-Host "  $emoji $message" -ForegroundColor $color
}

function Get-GitHubFileContent {
    param([string]$Repo, [string]$Path, [string]$Ref)
    # GitHub Contents API returns base64 content. For files >1MB it uses download_url instead.
    # gh api --jq '.content' can return the string with surrounding quotes or whitespace.
    $response = gh api "repos/$Repo/contents/${Path}?ref=$Ref" 2>$null
    if ($LASTEXITCODE -ne 0 -or -not $response) { return $null }
    $obj = $response | ConvertFrom-Json
    if ($obj.content) {
        $raw = $obj.content -replace '"', '' -replace '\s', ''
        try {
            return [System.Text.Encoding]::UTF8.GetString([System.Convert]::FromBase64String($raw))
        } catch {
            Write-Status "⚠️" "Base64 decode failed, trying download_url" Yellow
        }
    }
    # Fallback: download raw content via download_url
    if ($obj.download_url) {
        try {
            return (Invoke-RestMethod -Uri $obj.download_url)
        } catch {
            return $null
        }
    }
    return $null
}

# Component matching is data-driven from source-manifest.json — no hardcoded map needed.
# The user's -Component value is matched against the manifest's "path" field or repo name in "remoteUri".
function Find-ComponentInManifest {
    param($Manifest, [string]$Component)
    $lower = $Component.ToLower()
    # Try exact path match first
    $matches = @($manifest.repositories | Where-Object { $_.path -eq $lower })
    if ($matches.Count -eq 1) { return $matches[0] }
    # Try matching the repo name portion of remoteUri (e.g., "runtime" matches "https://github.com/dotnet/runtime")
    $matches = @($manifest.repositories | Where-Object { $_.remoteUri -match "/$lower(\.\w+)?$" })
    if ($matches.Count -eq 1) { return $matches[0] }
    # Try partial match on path (e.g., "nuget" matches "nuget-client")
    $matches = @($manifest.repositories | Where-Object { $_.path -like "*$lower*" })
    if ($matches.Count -eq 1) { return $matches[0] }
    if ($matches.Count -gt 1) {
        Write-Status "⚠️" "Ambiguous component '$Component' — matched $($matches.Count) entries: $(($matches | ForEach-Object { $_.path }) -join ', ')" Yellow
        return $null
    }
    return $null
}

# ─── Step 1: Decode SDK version ───

Write-Section "Step 1: Decode SDK Version"

# Parse: {major}.{minor}.{patch}-{prerelease}.{SHORT_DATE}.{revision}
# Prerelease can be multi-part: preview.3, rc.1, alpha.1, etc.
# or: {major}.{minor}.{patch}.{SHORT_DATE}.{revision} (GA)
# Use non-greedy (.+?) so SHORT_DATE and revision are captured from the end.
$versionRegex = '^(\d+)\.(\d+)\.(\d+)(?:-(.+?))?\.(\d{4,5})\.(\d+)$'
if ($SdkVersion -notmatch $versionRegex) {
    Write-Status "🔴" "Cannot parse SDK version: $SdkVersion" Red
    Write-Host "  Expected format: X.Y.ZZZ[-prerelease].SHORT_DATE.revision" -ForegroundColor DarkGray
    exit 1
}

$major = [int]$Matches[1]
$minor = [int]$Matches[2]
$patch = [int]$Matches[3]
$prerelease = $Matches[4]
$shortDate = [int]$Matches[5]
$revision = [int]$Matches[6]

$band = [int][math]::Floor($patch / 100)
$bandStr = "${band}xx"

# Decode SHORT_DATE using Arcade formula: SHORT_DATE = YY * 1000 + MM * 50 + DD
$yy = [int][math]::Floor($shortDate / 1000)
$remainder = $shortDate % 1000
$mm = [int][math]::Floor($remainder / 50)
$dd = $remainder % 50

if ($mm -lt 1 -or $mm -gt 12 -or $dd -lt 1 -or $dd -gt 31) {
    Write-Status "🔴" "Invalid SHORT_DATE $shortDate decodes to month=$mm day=$dd" Red
    Write-Host "  The SHORT_DATE formula is YY*1000 + MM*50 + DD. Check the version string." -ForegroundColor DarkGray
    exit 1
}

$year = 2000 + $yy
$buildDate = Get-Date -Year $year -Month $mm -Day $dd
$buildDateStr = $buildDate.ToString("yyyy-MM-dd")

# Determine VMR branch — 1xx preview/rc branches use a suffix, all others use just the band
# For 1xx GA: current dev major builds from 'main', released majors from 'release/X.Y.1xx'
if ($band -eq 1 -and $prerelease) {
    # VMR preview/rc branches use preview1/rc1 (no dot before N)
    $normalizedPrerelease = $prerelease -replace '\\.(\d+)$', '$1'
    $vmrBranch = "release/$major.$minor.${bandStr}-$normalizedPrerelease"
} elseif ($band -eq 1 -and -not $prerelease) {
    # GA 1xx — if the release branch doesn't exist, this is the current dev major (use main)
    $vmrBranch = "release/$major.$minor.${bandStr}"
    $encodedBranch = [uri]::EscapeDataString($vmrBranch)
    $branchCheck = gh api "repos/dotnet/dotnet/branches/$encodedBranch" 2>$null | ConvertFrom-Json -ErrorAction SilentlyContinue
    if (-not $branchCheck -or -not $branchCheck.name) {
        $vmrBranch = "main"
    }
} else {
    $vmrBranch = "release/$major.$minor.${bandStr}"
}

Write-Host "  SDK Version:  $SdkVersion"
Write-Host "  Major.Minor:  $major.$minor"
Write-Host "  Band:         ${bandStr} (patch=$patch)"
Write-Host "  Prerelease:   $(if ($prerelease) { $prerelease } else { '(none - GA)' })"
Write-Host "  SHORT_DATE:   $shortDate (YY=$yy MM=$mm DD=$dd)"
Write-Host "  Build Date:   $buildDateStr"
Write-Host "  Revision:     $revision"
Write-Host "  VMR Branch:   $vmrBranch"

if ($DecodeOnly) {
    Write-Host ""
    Write-Host "[SKILL_SUMMARY]"
    $summary = [ordered]@{
        sdkVersion = $SdkVersion
        major = $major; minor = $minor; band = $bandStr; patch = $patch
        prerelease = $prerelease
        shortDate = $shortDate; buildDate = $buildDateStr
        year = $year; month = $mm; day = $dd
        revision = $revision
        vmrBranch = $vmrBranch
    }
    Write-Host ($summary | ConvertTo-Json -Depth 4 -Compress)
    Write-Host "[/SKILL_SUMMARY]"
    exit 0
}

# ─── Step 2: Find the AzDO build ───

Write-Section "Step 2: Find AzDO Build"

Write-Host "  Searching internal builds on branch: $vmrBranch" -ForegroundColor DarkGray
Write-Host "  Looking for builds around: $buildDateStr" -ForegroundColor DarkGray

# Search ±2 days around the build date
$minDate = $buildDate.AddDays(-2).ToString("yyyy-MM-ddT00:00:00Z")
$maxDate = $buildDate.AddDays(2).ToString("yyyy-MM-ddT23:59:59Z")

# Use az CLI to query internal builds
# Pipeline 1330 = dotnet-unified-build in dnceng/internal
$buildsJson = az pipelines runs list `
    --org "https://dev.azure.com/dnceng" `
    --project "internal" `
    --pipeline-ids 1330 `
    --branch "refs/heads/$vmrBranch" `
    --top 20 `
    --query "[?finishTime >= '$minDate' && finishTime <= '$maxDate' && (result == 'succeeded' || result == 'partiallySucceeded')]" `
    --output json 2>$null

if ($LASTEXITCODE -ne 0 -or -not $buildsJson) {
    Write-Status "⚠️" "Could not query dnceng/internal builds (az CLI auth may be needed)." Yellow
    Write-Host ""
    Write-Host "[SKILL_SUMMARY]"
    $summary = [ordered]@{
        status = "azdo_query_failed"
        sdkVersion = $SdkVersion
        vmrBranch = $vmrBranch
        buildDate = $buildDateStr
        searchWindow = [ordered]@{ minDate = $minDate; maxDate = $maxDate }
        note = "az CLI auth to dnceng org required for internal build lookup. Try 'az login' if not authenticated."
    }
    Write-Host ($summary | ConvertTo-Json -Depth 4 -Compress)
    Write-Host "[/SKILL_SUMMARY]"
    exit 0
}

$builds = $buildsJson | ConvertFrom-Json
if ($builds.Count -eq 0) {
    Write-Status "⚠️" "No successful builds found in date range $minDate to $maxDate" Yellow

    Write-Host ""
    Write-Host "[SKILL_SUMMARY]"
    $summary = [ordered]@{
        status = "no_builds_found"
        sdkVersion = $SdkVersion
        vmrBranch = $vmrBranch
        buildDate = $buildDateStr
        band = $bandStr
        searchWindow = [ordered]@{ minDate = $minDate; maxDate = $maxDate }
        note = "Decoded version data above is valid. Try widening the search window or checking the AzDO pipeline directly."
    }
    Write-Host ($summary | ConvertTo-Json -Depth 4 -Compress)
    Write-Host "[/SKILL_SUMMARY]"
    exit 0
}

# Pick the build closest to the target date
$targetBuild = $builds | Sort-Object { [math]::Abs(([DateTime]$_.finishTime - $buildDate).TotalSeconds) } | Select-Object -First 1
$vmrCommit = $targetBuild.sourceVersion
$buildNumber = $targetBuild.buildNumber
$buildResult = $targetBuild.result

Write-Status "✅" "Found build: $buildNumber ($buildResult)" Green
Write-Host "  VMR Commit: $vmrCommit"
Write-Host "  Finished:   $($targetBuild.finishTime)"

# ─── Step 3: Check source-manifest.json ───

Write-Section "Step 3: Check source-manifest.json"

$manifestContent = Get-GitHubFileContent -Repo "dotnet/dotnet" -Path "src/source-manifest.json" -Ref $vmrCommit
if (-not $manifestContent) {
    Write-Status "⚠️" "Cannot fetch source-manifest.json from VMR at $vmrCommit" Yellow
    Write-Host "  The agent should fetch this file via gh api or raw.githubusercontent.com." -ForegroundColor DarkGray

    Write-Host ""
    Write-Host "[SKILL_SUMMARY]"
    $summary = [ordered]@{
        status = "manifest_fetch_failed"
        sdkVersion = $SdkVersion
        vmrBranch = $vmrBranch
        vmrCommit = $vmrCommit
        buildNumber = $buildNumber
        component = $Component
        fallback = "Fetch via: gh api repos/dotnet/dotnet/contents/src/source-manifest.json?ref=$vmrCommit"
    }
    Write-Host ($summary | ConvertTo-Json -Depth 4 -Compress)
    Write-Host "[/SKILL_SUMMARY]"
    exit 0
}

$manifest = $manifestContent | ConvertFrom-Json

# Search for the component using data-driven matching
$entry = Find-ComponentInManifest -Manifest $manifest -Component $Component

if (-not $entry) {
    # Show available components so the user/agent can pick the right one
    $available = ($manifest.repositories | ForEach-Object { $_.path }) -join ', '
    Write-Status "⚠️" "'$Component' not found in source-manifest.json at this VMR commit" Yellow
    Write-Host "  Available components: $available" -ForegroundColor DarkGray
}

if ($entry) {
    Write-Status "✅" "Found $Component in source-manifest.json" Green
    Write-Host "  Repository: $($entry.remoteUri)"
    Write-Host "  Commit SHA: $($entry.commitSha)"

    $resolvedSha = $entry.commitSha
    $resolvedRepo = $entry.remoteUri
    $traceType = "source-built (direct)"
} else {

# ─── Step 4: Follow Version.Details.xml (servicing branches) ───

Write-Section "Step 4: Follow Version.Details.xml (component is prebuilt)"

Write-Status "⚠️" "$Component is NOT source-built on $vmrBranch — following dependency chain" Yellow

$vdxContent = Get-GitHubFileContent -Repo "dotnet/dotnet" -Path "eng/Version.Details.xml" -Ref $vmrCommit
if (-not $vdxContent) {
    Write-Status "⚠️" "Cannot fetch Version.Details.xml. Agent should trace manually." Yellow

    Write-Host ""
    Write-Host "[SKILL_SUMMARY]"
    $summary = [ordered]@{
        status = "vdx_fetch_failed"
        sdkVersion = $SdkVersion
        vmrBranch = $vmrBranch
        vmrCommit = $vmrCommit
        buildNumber = $buildNumber
        component = $Component
        fallback = "Fetch eng/Version.Details.xml from dotnet/dotnet at $vmrCommit and inspect the <Dependency> entry for MicrosoftNETCoreAppRefPackageVersion, then use its <Sha> element as the component commit."
    }
    Write-Host ($summary | ConvertTo-Json -Depth 4 -Compress)
    Write-Host "[/SKILL_SUMMARY]"
    exit 0
}

[xml]$vdx = $vdxContent

# Find the dependency that points to the upstream VMR commit (dotnet-dotnet source).
# VDX uses <Uri> and <Sha> as child elements. Check both ProductDependencies and ToolsetDependencies.
$upstreamSha = $null
$allDeps = @()
if ($vdx.Dependencies.ProductDependencies.Dependency) {
    $allDeps += $vdx.Dependencies.ProductDependencies.Dependency
}
if ($vdx.Dependencies.ToolsetDependencies.Dependency) {
    $allDeps += $vdx.Dependencies.ToolsetDependencies.Dependency
}
foreach ($dep in $allDeps) {
    $uri = $dep.Uri
    if ($uri -and $uri -match 'dotnet-dotnet|dotnet/dotnet') {
        $upstreamSha = $dep.Sha
        Write-Host "  Upstream VMR SHA: $upstreamSha"
        Write-Host "  From package: $($dep.Name) v$($dep.Version)"
        break
    }
}

if (-not $upstreamSha) {
    Write-Status "🔴" "Could not find upstream VMR reference in Version.Details.xml" Red
    Write-Host "  The agent should manually inspect eng/Version.Details.xml at $vmrCommit" -ForegroundColor DarkGray

    Write-Host ""
    Write-Host "[SKILL_SUMMARY]"
    $summary = [ordered]@{
        status = "upstream_not_found"
        sdkVersion = $SdkVersion
        vmrBranch = $vmrBranch
        vmrCommit = $vmrCommit
        component = $Component
    }
    Write-Host ($summary | ConvertTo-Json -Depth 4 -Compress)
    Write-Host "[/SKILL_SUMMARY]"
    exit 0
}

# Now check source-manifest.json at the upstream SHA
Write-Section "Step 5: Check source-manifest.json at upstream VMR commit"

$upstreamManifestContent = Get-GitHubFileContent -Repo "dotnet/dotnet" -Path "src/source-manifest.json" -Ref $upstreamSha
if (-not $upstreamManifestContent) {
    Write-Status "⚠️" "Cannot fetch source-manifest.json at upstream SHA $upstreamSha" Yellow

    Write-Host ""
    Write-Host "[SKILL_SUMMARY]"
    $summary = [ordered]@{
        status = "upstream_manifest_failed"
        sdkVersion = $SdkVersion
        vmrBranch = $vmrBranch
        vmrCommit = $vmrCommit
        upstreamVmrSha = $upstreamSha
        component = $Component
        fallback = "Fetch source-manifest.json from dotnet/dotnet at $upstreamSha"
    }
    Write-Host ($summary | ConvertTo-Json -Depth 4 -Compress)
    Write-Host "[/SKILL_SUMMARY]"
    exit 0
}

$upstreamManifest = $upstreamManifestContent | ConvertFrom-Json

$upstreamEntry = Find-ComponentInManifest -Manifest $upstreamManifest -Component $Component

if ($upstreamEntry) {
    Write-Status "✅" "Found $Component in upstream source-manifest.json" Green
    Write-Host "  Repository: $($upstreamEntry.remoteUri)"
    Write-Host "  Commit SHA: $($upstreamEntry.commitSha)"
    Write-Host ""
    Write-Host "  Trace chain:"
    Write-Host "    SDK $SdkVersion"
    Write-Host "    → VMR branch: $vmrBranch (commit: $vmrCommit)"
    Write-Host "    → Version.Details.xml → upstream VMR: $upstreamSha"
    Write-Host "    → source-manifest.json → $($upstreamEntry.remoteUri): $($upstreamEntry.commitSha)"

    $resolvedSha = $upstreamEntry.commitSha
    $resolvedRepo = $upstreamEntry.remoteUri
    $traceType = "prebuilt (via Version.Details.xml → upstream 1xx branch)"
} else {
    Write-Status "🔴" "$Component not found in upstream source-manifest.json either" Red
    Write-Host "  Component may not be tracked in source-manifest.json for this branch configuration." -ForegroundColor DarkGray

    Write-Host ""
    Write-Host "[SKILL_SUMMARY]"
    $summary = [ordered]@{
        status = "not_found"
        sdkVersion = $SdkVersion
        vmrBranch = $vmrBranch
        vmrCommit = $vmrCommit
        upstreamVmrSha = $upstreamSha
        component = $Component
    }
    Write-Host ($summary | ConvertTo-Json -Depth 4 -Compress)
    Write-Host "[/SKILL_SUMMARY]"
    exit 0
}

} # end of else (component not in direct source-manifest)

# ─── Step 6 (optional): Check if specific commits are included ───

$commitResults = @()
if ($CheckCommit -and $resolvedSha) {
    Write-Section "Step 6: Check Commits Against Resolved SHA ($($resolvedSha.Substring(0, 12)))"

    foreach ($commit in $CheckCommit) {
        $shortCommit = $commit.Substring(0, [Math]::Min(12, $commit.Length))

        $commitMsg = git log --oneline -1 $commit 2>$null
        if ($LASTEXITCODE -ne 0) {
            Write-Status "⚠️" "$shortCommit — not found in local repo (fetch may be needed)" Yellow
            $commitResults += [ordered]@{ commit = $commit; included = "unknown"; reason = "not in local repo" }
            continue
        }

        # Check ancestry — distinguish "not ancestor" from "error"
        $mergeBaseOutput = git merge-base --is-ancestor $commit $resolvedSha 2>&1
        $mergeBaseExit = $LASTEXITCODE

        if ($mergeBaseExit -eq 0) {
            Write-Status "✅" "$shortCommit — INCLUDED ($commitMsg)" Green
            $commitResults += [ordered]@{ commit = $commit; included = $true; message = $commitMsg }
        } elseif ($mergeBaseExit -eq 1) {
            # Exit code 1 = not an ancestor (definitive answer)
            Write-Status "🔴" "$shortCommit — NOT included ($commitMsg)" Red
            $commitResults += [ordered]@{ commit = $commit; included = $false; message = $commitMsg }
        } else {
            # Exit code > 1 = error (disconnected history, bad object, etc.)
            Write-Status "⚠️" "$shortCommit — UNKNOWN (merge-base error: $mergeBaseOutput)" Yellow
            $commitResults += [ordered]@{ commit = $commit; included = "unknown"; reason = "merge-base error"; message = $commitMsg }
        }
    }
}

# ─── Final Summary ───

Write-Host ""
Write-Host "[SKILL_SUMMARY]"
$summary = [ordered]@{
    status = if ($resolvedSha) { if ($traceType -match 'direct') { "found_direct" } else { "found_via_upstream" } } else { "not_found" }
    sdkVersion = $SdkVersion
    vmrBranch = $vmrBranch
    vmrCommit = $vmrCommit
    buildNumber = $buildNumber
    component = $Component
    componentRepo = $resolvedRepo
    componentSha = $resolvedSha
    traceType = $traceType
}
if ($upstreamSha) { $summary.upstreamVmrSha = $upstreamSha }
if ($commitResults.Count -gt 0) { $summary.commitChecks = $commitResults }
Write-Host ($summary | ConvertTo-Json -Depth 4 -Compress)
Write-Host "[/SKILL_SUMMARY]"
