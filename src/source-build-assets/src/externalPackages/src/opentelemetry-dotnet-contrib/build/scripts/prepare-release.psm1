Import-Module $PSScriptRoot\build.psm1

function CreatePullRequestToUpdateChangelogsAndPublicApis {
  param(
    [Parameter(Mandatory=$true)][string]$gitRepository,
    [Parameter(Mandatory=$true)][string]$component,
    [Parameter(Mandatory=$true)][string]$version,
    [Parameter(Mandatory=$true)][string]$requestedByUserName,
    [Parameter()][string]$releaseIssue,
    [Parameter()][string]$targetBranch="main",
    [Parameter()][string]$gitUserName,
    [Parameter()][string]$gitUserEmail
  )

  $match = [regex]::Match($version, '^(\d+\.\d+\.\d+)(?:-((?:alpha)|(?:beta)|(?:rc))\.(\d+))?$')
  if ($match.Success -eq $false)
  {
      throw 'Input version did not match expected format'
  }

  $projectContent = Get-Content -Path src/$component/$component.csproj

  $match = [regex]::Match($projectContent, '<MinVerTagPrefix>(.*)<\/MinVerTagPrefix>')
  if ($match.Success -eq $false)
  {
      throw 'Could not parse MinVerTagPrefix from project file'
  }

  $minVerTagPrefix = $match.Groups[1].Value
  $tag="${minVerTagPrefix}${version}"
  $branch="otelbot/prepare-${tag}-release"

  if ([string]::IsNullOrEmpty($gitUserName) -eq $false)
  {
    git config user.name $gitUserName
  }
  if ([string]::IsNullOrEmpty($gitUserEmail) -eq $false)
  {
    git config user.email $gitUserEmail
  }

  git switch --create $branch 2>&1 | % ToString
  if ($LASTEXITCODE -gt 0)
  {
      throw 'git switch failure'
  }

  if ([string]::IsNullOrEmpty($releaseIssue) -eq $false)
  {
    $issueText =
@"

Release request: #$releaseIssue
"@
  }

  $body =
@"
Note: This PR was opened automatically by the [prepare release workflow](https://github.com/$gitRepository/actions/workflows/prepare-release.yml).
$issueText
Requested by: @$requestedByUserName

## Changes

* CHANGELOG files updated for projects being released.
"@

  # Update CHANGELOGs
  & ./build/scripts/update-changelogs.ps1 -minVerTagPrefix $minVerTagPrefix -version $version

  # Update publicApi files for stable releases
  if ($version -notlike "*-alpha*" -and $version -notlike "*-beta*" -and $version -notlike "*-rc*")
  {
    & ./build/scripts/finalize-publicapi.ps1 -minVerTagPrefix $minVerTagPrefix

    $body += "`r`n* Public API files updated for projects being released (only performed for stable releases)."
  }

  $body +=
@"

## Commands

``/UpdateReleaseDates``: Use to update release dates in CHANGELOGs before merging [``approvers``, ``maintainers``]
``/CreateReleaseTag``: Use after merging to push the release tag and trigger the job to create packages and push to NuGet [``approvers``, ``maintainers``]
"@

  git commit -a -m "Prepare repo to release $tag." -s 2>&1 | % ToString
  if ($LASTEXITCODE -gt 0)
  {
      throw 'git commit failure'
  }

  git push -u origin $branch 2>&1 | % ToString
  if ($LASTEXITCODE -gt 0)
  {
      throw 'git push failure'
  }

  gh pr create `
    --title "[release] Prepare release $tag" `
    --body $body `
    --base $targetBranch `
    --head $branch `
    --label release
}

Export-ModuleMember -Function CreatePullRequestToUpdateChangelogsAndPublicApis

function LockPullRequestAndPostNoticeToCreateReleaseTag {
  param(
    [Parameter(Mandatory=$true)][string]$gitRepository,
    [Parameter(Mandatory=$true)][string]$pullRequestNumber,
    [Parameter(Mandatory=$true)][string]$expectedPrAuthorUserName
  )

  $prViewResponse = gh pr view $pullRequestNumber --json mergeCommit,author,title | ConvertFrom-Json

  if ($prViewResponse.author.login -ne $expectedPrAuthorUserName)
  {
      throw 'PR author was unexpected'
  }

  $match = [regex]::Match($prViewResponse.title, '^\[release\] Prepare release (.*)$')
  if ($match.Success -eq $false)
  {
      throw 'Could not parse tag from PR title'
  }

  $tag = $match.Groups[1].Value

  $commit = $prViewResponse.mergeCommit.oid
  if ([string]::IsNullOrEmpty($commit) -eq $true)
  {
      throw 'Could not find merge commit'
  }

  $body =
@"
I noticed this PR was merged.

Post a comment with "/CreateReleaseTag" in the body if you would like me to create the release tag ``$tag`` for [the merge commit](https://github.com/$gitRepository/commit/$commit) which will trigger the package workflow.
"@

  gh pr comment $pullRequestNumber --body $body

  gh pr lock $pullRequestNumber
}

Export-ModuleMember -Function LockPullRequestAndPostNoticeToCreateReleaseTag

function CreateReleaseTagAndPostNoticeOnPullRequest {
  param(
    [Parameter(Mandatory=$true)][string]$gitRepository,
    [Parameter(Mandatory=$true)][string]$pullRequestNumber,
    [Parameter(Mandatory=$true)][string]$expectedPrAuthorUserName,
    [Parameter()][string]$gitUserName,
    [Parameter()][string]$gitUserEmail
  )

  $prViewResponse = gh pr view $pullRequestNumber --json mergeCommit,author,title | ConvertFrom-Json

  if ($prViewResponse.author.login -ne $expectedPrAuthorUserName)
  {
      throw 'PR author was unexpected'
  }

  $match = [regex]::Match($prViewResponse.title, '^\[release\] Prepare release (.*)$')
  if ($match.Success -eq $false)
  {
      throw 'Could not parse tag from PR title'
  }

  $tag = $match.Groups[1].Value

  $commit = $prViewResponse.mergeCommit.oid
  if ([string]::IsNullOrEmpty($commit) -eq $true)
  {
      throw 'Could not find merge commit'
  }

  if ([string]::IsNullOrEmpty($gitUserName) -eq $false)
  {
    git config user.name $gitUserName
  }
  if ([string]::IsNullOrEmpty($gitUserEmail) -eq $false)
  {
    git config user.email $gitUserEmail
  }

  git tag -a $tag -m "$tag" $commit 2>&1 | % ToString
  if ($LASTEXITCODE -gt 0)
  {
      throw 'git tag failure'
  }

  git push origin $tag 2>&1 | % ToString
  if ($LASTEXITCODE -gt 0)
  {
      throw 'git push failure'
  }

  gh pr unlock $pullRequestNumber

  $body =
@"
I just pushed the [$tag](https://github.com/$gitRepository/releases/tag/$tag) tag.

The [package workflow](https://github.com/$gitRepository/actions/workflows/publish-packages.yml) should begin momentarily.
"@

  gh pr comment $pullRequestNumber --body $body
}

Export-ModuleMember -Function CreateReleaseTagAndPostNoticeOnPullRequest

function UpdateChangelogReleaseDatesAndPostNoticeOnPullRequest {
  param(
    [Parameter(Mandatory=$true)][string]$gitRepository,
    [Parameter(Mandatory=$true)][string]$pullRequestNumber,
    [Parameter(Mandatory=$true)][string]$expectedPrAuthorUserName,
    [Parameter(Mandatory=$true)][string]$commentUserName,
    [Parameter()][string]$gitUserName,
    [Parameter()][string]$gitUserEmail
  )

  $prViewResponse = gh pr view $pullRequestNumber --json headRefName,author,title | ConvertFrom-Json

  if ($prViewResponse.author.login -ne $expectedPrAuthorUserName)
  {
      throw 'PR author was unexpected'
  }

  $match = [regex]::Match($prViewResponse.title, '^\[release\] Prepare release (.*)$')
  if ($match.Success -eq $false)
  {
      throw 'Could not parse tag from PR title'
  }

  $tag = $match.Groups[1].Value

  $match = [regex]::Match($tag, '^(.*?-)(.*)$')
  if ($match.Success -eq $false)
  {
      throw 'Could not parse prefix or version from tag'
  }

  $tagPrefix = $match.Groups[1].Value
  $version = $match.Groups[2].Value

  $commentUserPermission = gh api "repos/$gitRepository/collaborators/$commentUserName/permission" | ConvertFrom-Json
  if ($commentUserPermission.permission -ne 'admin' -and $commentUserPermission.permission -ne 'write')
  {
    gh pr comment $pullRequestNumber `
      --body "I'm sorry @$commentUserName but you don't have permission to update this PR. Only maintainers and approvers can update this PR."
    return
  }

  if ([string]::IsNullOrEmpty($gitUserName) -eq $false)
  {
    git config user.name $gitUserName
  }
  if ([string]::IsNullOrEmpty($gitUserEmail) -eq $false)
  {
    git config user.email $gitUserEmail
  }

  git switch $prViewResponse.headRefName 2>&1 | % ToString
  if ($LASTEXITCODE -gt 0)
  {
      throw 'git switch failure'
  }

  $updatedFiles = 0
  $newHeader =
@"
## $version

Released $(Get-Date -UFormat '%Y-%b-%d')
"@

  $projectDirs = Get-ChildItem -Path src/**/*.csproj | Select-String "<MinVerTagPrefix>$tagPrefix</MinVerTagPrefix>" -List | Select Path | Split-Path -Parent

  foreach ($projectDir in $projectDirs)
  {
    $content = (Get-Content "$projectDir/CHANGELOG.md" -Raw)

    $newContent = $content -replace "## $version\s*Released .*", $newHeader

    if ($content -ne $newContent)
    {
      $updatedFiles++
      Set-Content -Path "$projectDir/CHANGELOG.md" $newContent.Trim()
    }
  }

  if ($updatedFiles -eq 0)
  {
    gh pr comment $pullRequestNumber --body "All of the CHANGELOG files have valid release dates."
    return
  }

  git commit -a -m "Update CHANGELOG release dates for $tag." -s 2>&1 | % ToString
  if ($LASTEXITCODE -gt 0)
  {
      throw 'git commit failure'
  }

  git push -u origin $prViewResponse.headRefName 2>&1 | % ToString
  if ($LASTEXITCODE -gt 0)
  {
      throw 'git push failure'
  }

  gh pr comment $pullRequestNumber --body "I updated the CHANGELOG release dates."
}

Export-ModuleMember -Function UpdateChangelogReleaseDatesAndPostNoticeOnPullRequest

function TagCodeOwnersOnOrRunWorkflowForRequestReleaseIssue {
  param(
    [Parameter(Mandatory=$true)][string]$gitRepository,
    [Parameter(Mandatory=$true)][string]$triggeringEventName,
    [Parameter(Mandatory=$true)][string]$approvingGroups,
    [Parameter(Mandatory=$true)][string]$requestedByUserName,
    [Parameter(Mandatory=$true)][string]$issueNumber,
    [Parameter(Mandatory=$true)][string]$issueBody,
    [Parameter()][string]$targetBranch="main",
    [Parameter()][string]$gitUserName,
    [Parameter()][string]$gitUserEmail
  )

  $match = [regex]::Match($issueBody, '^[#]+ Component\s*(OpenTelemetry\.(?:.|\w+)+)$', [Text.RegularExpressions.RegexOptions]::Multiline)
  if ($match.Success -eq $false)
  {
      Write-Host 'Component could not be parsed from body'
      Return
  }

  $component = $match.Groups[1].Value.Trim()

  $match = [regex]::Match($issueBody, '^[#]+ Version\s*(.*)$', [Text.RegularExpressions.RegexOptions]::Multiline)
  if ($match.Success -eq $false)
  {
      Write-Host 'Version could not be parsed from body'
      Return
  }

  $version = $match.Groups[1].Value.Trim()

  $match = [regex]::Match($version, '^(\d+\.\d+\.\d+)(?:-((?:alpha)|(?:beta)|(?:rc))\.(\d+))?$')
  if ($match.Success -eq $false)
  {
      gh issue comment $issueNumber `
        --body "The version specified on the release request is invalid. Please create a new release request with a valid version or edit the description and set a valid version."
      Return
  }

  $componentOwners = $null

  FindComponentOwners `
      -component $component `
      -issueNumber $issueNumber `
      -componentOwners ([ref]$componentOwners)

  $requestedByUserPermission = gh api "repos/$gitRepository/collaborators/$requestedByUserName/permission" | ConvertFrom-Json

  $kickOffWorkflow = $false
  $kickOffWorkflowReason = ''

  if ($requestedByUserPermission.permission -eq 'admin' -or $requestedByUserPermission.permission -eq 'write')
  {
    $kickOffWorkflow = $true
    $kickOffWorkflowReason = "@$requestedByUserName has collaborator or greater permission"
  }
  elseif ($componentOwners.Contains($requestedByUserName) -eq $true)
  {
    $kickOffWorkflow = $true
    $kickOffWorkflowReason = "@$requestedByUserName is a component owner"
  }

  if ($kickOffWorkflow -eq $true)
  {
    CreatePullRequestToUpdateChangelogsAndPublicApis `
      -gitRepository $gitRepository `
      -component $component `
      -version $version `
      -requestedByUserName $requestedByUserName `
      -releaseIssue $issueNumber `
      -targetBranch $targetBranch `
      -gitUserName $gitUserName `
      -gitUserEmail $gitUserEmail

    gh issue close $issueNumber `
      --comment "I executed the prepare release script for ``$component`` version ``$version``` because $kickOffWorkflowReason."

    return
  }

  if ($triggeringEventName -eq 'issues')
  {
    # Executed when issues are created or edited
    $componentOwnerApprovers = ''
    if ($componentOwners.Count -gt 0)
    {
      foreach ($componentOwner in $componentOwners)
      {
        $componentOwnerApprovers += "@$componentOwner "
      }
    }

    $body =
@"
$componentOwnerApprovers$approvingGroups

Post a comment with "/PrepareRelease" in the body if you would like me to execute the prepare release script for the component and version listed in the description.
"@

    gh issue comment $issueNumber --body $body
  }
  else {
    # Executed when issues are commented with the /PrepareRelease command
    if ($kickOffWorkflow -eq $false)
    {
      gh issue comment $issueNumber `
        --body "I'm sorry @$requestedByUserName but you don't have permission to execute the prepare release script. Only maintainers, approvers, and/or owners of the component may use the `"/PrepareRelease`" command."
    }
  }
}

Export-ModuleMember -Function TagCodeOwnersOnOrRunWorkflowForRequestReleaseIssue
