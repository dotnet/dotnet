<#
.SYNOPSIS
Runs the Helix Job Monitor for an Azure DevOps build.

.PARAMETER BuildUrl
The Azure DevOps build results URL. Quote URLs containing '&' so PowerShell
does not interpret it as a command separator.

.PARAMETER StageName
The stage containing the Helix Job Monitor job. When omitted and more than one
stage contains the job, the script prompts for the stage name.

.PARAMETER StageAttempt
The stage attempt to monitor. Defaults to the attempt of the latest Helix Job
Monitor job in the selected stage.

.PARAMETER ArgsOnly
Prints only the resolved JobMonitor arguments for use in a Visual Studio
debugging session. The JobMonitor is not run and HELIX_ACCESSTOKEN is not required.

.EXAMPLE
./scripts/run-helix-job-monitor.ps1 'https://dev.azure.com/dnceng-public/public/_build/results?buildId=1549709&view=results'

.EXAMPLE
./scripts/run-helix-job-monitor.ps1 'https://dev.azure.com/dnceng-public/public/_build/results?buildId=1549709&view=results' -ArgsOnly
#>

[CmdletBinding(SupportsShouldProcess = $true)]
param(
    [Parameter(Mandatory = $true, Position = 0)]
    [uri] $BuildUrl,

    [Parameter(Mandatory = $false)]
    [string] $StageName,

    [Parameter(Mandatory = $false)]
    [ValidateRange(1, [int]::MaxValue)]
    [int] $StageAttempt,

    [Parameter(Mandatory = $false)]
    [switch] $ArgsOnly
)

$ErrorActionPreference = 'Stop'

function Get-QueryValue {
    param(
        [uri] $Uri,
        [string] $Name
    )

    foreach ($part in $Uri.Query.TrimStart('?') -split '&') {
        $pair = $part -split '=', 2
        if ([uri]::UnescapeDataString($pair[0]) -eq $Name) {
            if ($pair.Count -eq 2) {
                return [uri]::UnescapeDataString($pair[1])
            }

            return ''
        }
    }

    return $null
}

function Get-RepositoryCoordinates {
    param([object] $Repository)

    $repositoryUrl = [string] $Repository.url
    if (-not [string]::IsNullOrWhiteSpace($repositoryUrl)) {
        $parsedRepositoryUrl = [uri] $repositoryUrl
        if ($parsedRepositoryUrl.Host -eq 'github.com') {
            $segments = @($parsedRepositoryUrl.AbsolutePath.Trim('/') -split '/')
            if ($segments.Count -ge 2) {
                return [pscustomobject]@{
                    Organization = $segments[-2]
                    Repository = $segments[-1] -replace '\.git$', ''
                }
            }
        }
    }

    $repositoryIdentifier = [string] $Repository.name
    if ([string]::IsNullOrWhiteSpace($repositoryIdentifier)) {
        $repositoryIdentifier = [string] $Repository.id
    }

    if ($repositoryIdentifier -match '^([^/]+)/(.+)$') {
        return [pscustomobject]@{
            Organization = $Matches[1]
            Repository = $Matches[2]
        }
    }

    if ($repositoryIdentifier -match '^([^-]+)-(.+)$') {
        return [pscustomobject]@{
            Organization = $Matches[1]
            Repository = $Matches[2]
        }
    }

    throw "Could not derive the Helix organization and repository from build repository '$repositoryIdentifier' ($repositoryUrl)."
}

function Get-OwningStage {
    param(
        [object] $Job,
        [hashtable] $RecordsById
    )

    $record = $Job
    while ($null -ne $record -and -not [string]::IsNullOrWhiteSpace([string] $record.parentId)) {
        $record = $RecordsById[[string] $record.parentId]
        if ($null -ne $record -and $record.type -eq 'Stage') {
            return $record
        }
    }

    return $null
}

if (-not $ArgsOnly -and [string]::IsNullOrWhiteSpace($env:HELIX_ACCESSTOKEN)) {
    throw 'HELIX_ACCESSTOKEN must be set before running this script.'
}

if ($BuildUrl.Host -ne 'dev.azure.com') {
    throw "Expected a dev.azure.com build URL, but received '$BuildUrl'."
}

$pathSegments = @($BuildUrl.AbsolutePath.Trim('/') -split '/' | ForEach-Object { [uri]::UnescapeDataString($_) })
if ($pathSegments.Count -lt 2) {
    throw "Could not derive the Azure DevOps organization and project from '$BuildUrl'."
}

$azureDevOpsOrganization = $pathSegments[0]
$teamProject = $pathSegments[1]
$buildId = Get-QueryValue -Uri $BuildUrl -Name 'buildId'
if ($buildId -notmatch '^\d+$') {
    throw "The build URL must contain a numeric buildId query parameter: '$BuildUrl'."
}

$collectionUri = "https://dev.azure.com/$azureDevOpsOrganization/"
$escapedTeamProject = [uri]::EscapeDataString($teamProject)
$buildApiUri = "${collectionUri}${escapedTeamProject}/_apis/build/builds/${buildId}?api-version=7.1"
$timelineApiUri = "${collectionUri}${escapedTeamProject}/_apis/build/builds/${buildId}/timeline?api-version=7.1"

try {
    $build = Invoke-RestMethod -Method Get -Uri $buildApiUri
    $timeline = Invoke-RestMethod -Method Get -Uri $timelineApiUri
}
catch {
    throw "Failed to read Azure DevOps metadata for build $buildId. Ensure the build is publicly accessible. $($_.Exception.Message)"
}

$repository = Get-RepositoryCoordinates -Repository $build.repository
$records = @($timeline.records)
$recordsById = @{}
foreach ($record in $records) {
    $recordsById[[string] $record.id] = $record
}

$monitorJobs = @($records | Where-Object {
    $_.type -eq 'Job' -and (
        $_.identifier -match '(^|\.)HelixJobMonitor(\.|$)' -or
        $_.name -eq 'HelixJobMonitor' -or
        $_.name -eq 'Monitor Helix Jobs')
})

$monitorContexts = @($monitorJobs | ForEach-Object {
    $stage = Get-OwningStage -Job $_ -RecordsById $recordsById
    if ($null -ne $stage) {
        [pscustomobject]@{
            StageName = [string] $stage.name
            StageIdentifier = [string] $stage.identifier
            StageAttempt = [int] $stage.attempt
            JobAttempt = [int] $_.attempt
        }
    }
})

if ($monitorContexts.Count -eq 0) {
    throw "Build $buildId has no Helix Job Monitor job."
}

$stageGroups = @($monitorContexts | Group-Object StageIdentifier)
if ([string]::IsNullOrWhiteSpace($StageName)) {
    if ($stageGroups.Count -gt 1) {
        Write-Host 'The build has Helix Job Monitor jobs in these stages:'
        foreach ($stageGroup in $stageGroups) {
            $stage = $stageGroup.Group[0]
            $attempts = @($stageGroup.Group | Select-Object -ExpandProperty StageAttempt -Unique | Sort-Object) -join ', '
            Write-Host "  $($stage.StageName) (attempts: $attempts)"
        }

        $StageName = Read-Host 'Enter the stage name'
    }
    else {
        $StageName = $stageGroups[0].Group[0].StageName
    }
}

$selectedContexts = @($monitorContexts | Where-Object {
    $_.StageName -eq $StageName -or $_.StageIdentifier -eq $StageName
})
if ($selectedContexts.Count -eq 0) {
    $availableStages = @($stageGroups | ForEach-Object { $_.Group[0].StageName }) -join ', '
    throw "Stage '$StageName' does not contain a Helix Job Monitor job. Available stages: $availableStages."
}

$resolvedStageName = $selectedContexts[0].StageName
if ($PSBoundParameters.ContainsKey('StageAttempt')) {
    if (-not ($selectedContexts | Where-Object { $_.StageAttempt -eq $StageAttempt })) {
        $availableAttempts = @($selectedContexts | Select-Object -ExpandProperty StageAttempt -Unique | Sort-Object) -join ', '
        throw "Stage '$resolvedStageName' has no Helix Job Monitor job in attempt $StageAttempt. Available attempts: $availableAttempts."
    }
}
else {
    $latestJob = $selectedContexts | Sort-Object StageAttempt, JobAttempt -Descending | Select-Object -First 1
    $StageAttempt = $latestJob.StageAttempt
}

$buildReason = [string] $build.reason
if ($buildReason -eq 'pullRequest') {
    $buildReason = 'PullRequest'
}

$projectPath = Join-Path $PSScriptRoot '..\src\Microsoft.DotNet.Helix\JobMonitor\Microsoft.DotNet.Helix.JobMonitor.csproj'
$dotnetScript = Join-Path $PSScriptRoot '..\eng\common\dotnet.ps1'
$jobMonitorArguments = @(
    '--organization', $repository.Organization,
    '--repository', $repository.Repository,
    '--collection-uri', $collectionUri,
    '--team-project', $teamProject,
    '--build-id', [string] $buildId,
    '--helix-base-uri', 'https://helix.dot.net/',
    '--stage-name', $resolvedStageName,
    '--stage-attempt', [string] $StageAttempt,
    '--build-reason', $buildReason,
    '--source-branch', [string] $build.sourceBranch
)

if ($ArgsOnly) {
    $formattedArguments = for ($index = 0; $index -lt $jobMonitorArguments.Count; $index += 2) {
        $escapedValue = ([string] $jobMonitorArguments[$index + 1]).Replace('"', '\"')
        "$($jobMonitorArguments[$index]) `"$escapedValue`""
    }

    Write-Output ($formattedArguments -join ' ')
    return
}

Write-Host "Resolved $($repository.Organization)/$($repository.Repository), build $buildId, stage '$resolvedStageName', attempt $StageAttempt."

if ($PSCmdlet.ShouldProcess($projectPath, 'Run Helix Job Monitor')) {
    & $dotnetScript run --project $projectPath --configuration Debug -- @jobMonitorArguments
    exit $LASTEXITCODE
}