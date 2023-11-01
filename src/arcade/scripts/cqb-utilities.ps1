# Set of utility functionality for coherency QBs

<#
.SYNOPSIS
    Generate a public->internal merge PR

.DESCRIPTION
    Given the name of a release branch without the 'release/' prefix,
    an AzureDevOps PAT, and an AzDO a repository name, generate a merge PR
    from the release and internal/release branches.

    This functionality is useful for quickly creating PRs to resolve internal/public merge
    conflicts.

.PARAMETER RepoName
    AzDO repo name. E.g. dotnet-runtime
.PARAMETER Org
    AzDO org where the repo resides.
.PARAMETER Project
    AzDO project where the repo resides.
.PARAMETER BranchName
    Branch name, without 'release/' prefix of the branch to generate a internal merge PR for. E.g. '6.0'
.PARAMETER AzDOPAT
    PAT with Code R/W perms.
#>
Function Gen-Internal-Merge-PR
{
  param (
      [string]$RepoName,
      [string]$Org = "dnceng",
      [string]$Project = "internal",
      [string]$BranchName,
      [string]$AzDOPAT
  )

  $header = Get-AzDO-Auth-Header $AzDOPAT
  
  if ($BranchName -contains "/") {
    Write-Error "Branch name should just be the last segment of the branch name path"
    return
  }
  
  $prBody = @{
    sourceRefName = "refs/heads/release/$BranchName";
    targetRefName = "refs/heads/internal/release/$BranchName";
    title = "[internal/release/$BranchName] Merge from public";
    description = "Merge from public release/$BranchName to internal/release/$BranchName and resolve conflicts if necessary"
  }
  
  try {
    $resp = Invoke-WebRequest -Method Post -Body $($prBody | ConvertTo-Json) -Headers $header -Uri "https://dev.azure.com/$Org/$Project/_apis/git/repositories/$RepoName/pullrequests?api-version=7.0" -ContentType application/json
    $result = $resp | ConvertFrom-Json
    Write-Host https://dev.azure.com/$Org/$Project/_git/$RepoName/pullrequest/$($result.pullRequestId)
  } catch {
    Write-Error $_
  }
}

<#
.SYNOPSIS
    Generate a REST API auth header for AzDO

.PARAMETER AzDOPAT
    PAT to generate a header for.
#>
Function Get-AzDO-Auth-Header
{
  param (
      [string]$AzDOPAT
  )
  
  $base64authinfo = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes(":$AzDOPat"))
  return @{"Authorization"="Basic $base64authinfo"}
}

<#
.SYNOPSIS
    Given a manifest.json with builds, mark all of these as released.

.PARAMETER Manifest
    manifest.json file (from staging pipeline) with builds to release.
#>
Function Mark-All-In-Manifest-As-Released
{
  param (
      [string]$Manifest
  )

  $manifestContent = Get-Content $Manifest | ConvertFrom-Json
  foreach ($build in $manifestContent.builds) {
    Write-Host "Marking $($build.repo) @ $($build.commit) (BAR ID: $($build.barBuildId)) as released"
    darc update-build --released --id $build.barBuildId
  }
}