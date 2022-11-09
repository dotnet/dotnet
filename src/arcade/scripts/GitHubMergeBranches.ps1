#!/usr/bin/env pwsh -c

<#
.DESCRIPTION
Creates a GitHub pull request to merge a head branch into a base branch
.PARAMETER RepoOwner
The GitHub repository owner.
.PARAMETER RepoName
The GitHub repository name.
.PARAMETER BaseBranch
The base branch -- the target branch for the PR
.PARAMETER HeadBranch
The current branch
.PARAMETER Username
The GitHub username
.PARAMETER AuthToken
A personal access token
.PARAMETER Fork
Make PR from a fork
.PARAMETER AllowAutomatedCommits
Create a PR even if the only commits are from dotnet-maestro[bot]
.PARAMETER QuietComments
Do not tag commiters, do not comment on PR updates. Reduces GitHub notifications
#>
[CmdletBinding(SupportsShouldProcess = $true)]
param(
    [Alias('o')]
    [Parameter(Mandatory = $true)]
    $RepoOwner,

    [Alias('n')]
    [Parameter(Mandatory = $true)]
    $RepoName,

    [Alias('b')]
    [Parameter(Mandatory = $true)]
    $BaseBranch,

    [Alias('h')]
    [Parameter(Mandatory = $true)]
    $HeadBranch,

    [Parameter(Mandatory = $true)]
    [Alias('a')]
    $AuthToken,

    [Obsolete('Unused parameter. AuthToken is used to find the username.')]
    [Alias('u')]
    $Username,

    [switch]$Fork,

    [switch]$AllowAutomatedCommits,

    [switch]$QuietComments
)

$ErrorActionPreference = 'stop'
Set-StrictMode -Version 1
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

# Workaround for quirk in how dotnet-maestro-bot triggers this script
if ($RepoName -like "$RepoOwner/*") {
    $RepoName = $RepoName.Substring("$RepoOwner/".Length)
}

$headers = @{
    Authorization = "bearer $AuthToken"
}

[hashtable] $script:emails = @{}

function Invoke-Block([scriptblock]$cmd) {
    $cmd | Out-String | Write-Verbose
    & $cmd

    # Need to check both of these cases for errors as they represent different items
    # - $?: did the powershell script block throw an error
    # - $lastexitcode: did a windows command executed by the script block end in error
    if ((-not $?) -or ($lastexitcode -ne 0)) {
        if ($error -ne $null) {
            Write-Warning $error[0]
        }
        throw "Command failed to execute: $cmd"
    }
}

function GetCommitterGitHubName($sha) {
    $email = & git show -s --format='%ce' $sha
    $key = 'committer'

    if ($email -eq '@dotnet-maestro') {
        return 'dotnet-maestro'
    }

    # Exclude noreply@github.com - these map to https://github.com/web-flow, which is the user account
    # added as the 'committer' when users commit via the GitHub web UI on their own PRs
    if ((-not $email) -or ($email -eq 'noreply@github.com')) {
        $key = 'author'
        $email = & git show -s --format='%ae' $sha
    }

    if ($email -like '*@users.noreply.github.com') {
        [string[]] $userNames = ($email -replace '@users.noreply.github.com', '') -split '\+'
        return $userNames | select -last 1
    }
    elseif ($script:emails[$email]) {
        return $script:emails[$email]
    }
    else {
        Write-Verbose "Attempting to find GitHub username for $email"
        try {
            $resp = Invoke-RestMethod -Method GET -Headers $headers `
                "https://api.github.com/repos/$RepoOwner/$RepoName/commits/$sha"
            $resp | Write-Verbose

            $script:emails[$email] = $resp.$key.login
            return $resp.$key.login
        }
        catch {
            Write-Warning "Failed to find github user for $email. $_"
        }
    }
    return $null
}

function RepoExists($owner, $name) {
    try {
        $resp = Invoke-RestMethod -Headers $headers "https://api.github.com/repos/$owner/$name"
        $resp | Write-Verbose
        return ($resp -ne $null)
    }
    catch {
        return $false
    }
}

function GetOrCreateFork() {
    $resp = Invoke-RestMethod -Method Post -Headers $Headers `
        "https://api.github.com/repos/$RepoOwner/$RepoName/forks"
    $resp | Write-Verbose

    # This effectively gets the owner based on the PAT.
    $owner = $resp.owner.login
    # If there are repos in different orgs with the same name, like dotnet/buildtools and
    # aspnet/BuildTools, the name of the fork isn't predictable.
    $name = $resp.name

    $retries = 10
    $repoCreated = $false
    do {
        $retries -= 1
        if (RepoExists $owner $name) {
            # Fork creation is an async operation. Wait a minute to give GitHub more time to finish fork creation
            Start-Sleep -Seconds 90
            $repoCreated = $true
            break
        }
        Write-Host "Repo ${owner}/${name} does not exist yet. Waiting to check again..."
        Start-Sleep -Seconds 30
    } while ($retries -gt 0)

    if (-not $repoCreated) {
        throw "Could not create a fork ${owner}/${name} for ${RepoOwner}/${RepoName}"
    }

    return @{
        Name = $name
        Owner = $owner
    }
}

$workDir = "$PSScriptRoot/obj/$RepoOwner/$RepoName"
New-Item "$PSScriptRoot/obj/" -ItemType Directory -ErrorAction Ignore | Out-Null
            
Invoke-Block { & git config --system core.longpaths true }

$fetch = $true
if (-not (Test-Path $workDir)) {
    $fetch = $false
    Invoke-Block { & git clone "https://github.com/$RepoOwner/$RepoName.git" $workDir `
            --quiet `
            --no-tags `
            --branch $BaseBranch
    }
}

# see https://git-scm.com/docs/pretty-formats
$formatString = '%h %cn <%ce>: %s (%cr)'

Push-Location $workDir
try {
    if ($fetch) {
        Invoke-Block { & git fetch --quiet origin }
        Invoke-Block { & git checkout --quiet $BaseBranch }
        Invoke-Block { & git reset --hard origin/$BaseBranch }
    }

    Write-Host -f Magenta "${BaseBranch}:`t`t$(& git log --format=$formatString -1 HEAD)"

    Invoke-Block { & git checkout --quiet $HeadBranch }
    Invoke-Block { & git reset --quiet --hard origin/$HeadBranch }

    Write-Host -f Magenta "${HeadBranch}:`t$(& git log --format=$formatString -1 HEAD)"

    [string[]] $commitsToMerge = & git rev-list "$BaseBranch..$HeadBranch" # find all commits which will be merged

    if (-not $commitsToMerge) {
        Write-Warning "There were no commits to be merged from $HeadBranch into $BaseBranch"
        exit 0
    }

    $authors = $commitsToMerge `
        | % { Write-Host -f Cyan "Merging:`t$(git log --format=$formatString -1 $_)"; $_ } `
        | % { GetCommitterGitHubName $_ } `
        | ? { $_ -ne $null } `
        | select -Unique

    if (-not $AllowAutomatedCommits -and (($authors | measure).Count -eq 1) -and ($authors | select -first 1) -eq 'dotnet-maestro[bot]') {
        Write-Host -ForegroundColor Yellow 'Skipping PR generation because it appears this PR would only contain automated commits by @dotnet-maestro[bot]'
        exit 0
    }

    if (-not $QuietComments) {
        $authors = $authors | % { "* @$_" }
    } else {
        $authors = $authors | % { "* $_" }
    }
    

    $committersList = "This PR merges commits made on $HeadBranch by the following committers:`n`n$($authors -join "`n")"

    Write-Host $committersList

    $mergeBranchName = "merge/$HeadBranch-to-$BaseBranch"
    Invoke-Block { & git checkout -B $mergeBranchName  }

    $remoteName = 'origin'
    $prOwnerName = $RepoOwner
    $prRepoName = $RepoName

    if ($Fork) {
        $remoteName = 'fork'

        try {
            # remove remote if it already exists and re-configure
            Invoke-Block { & git remote remove fork }
        }
        catch { }

        if ($PSCmdlet.ShouldProcess("Finding or creating fork for ${RepoName}")) {

            Write-Host -ForegroundColor Yellow "Finding or creating fork for ${RepoName}"

            $forkData = GetOrCreateFork

            Invoke-Block { & git remote add fork "https://placeholderUser:${AuthToken}@github.com/$($forkData.Owner)/$($forkData.Name).git" }
            $prOwnerName = $forkData.Owner
            $prRepoName = $forkData.Name
        }
    }

    $query = 'query ($repoOwner: String!, $repoName: String!, $baseRefName: String!) {
        repository(owner: $repoOwner, name: $repoName) {
          pullRequests(baseRefName: $baseRefName, states: OPEN, first: 100) {
            totalCount
            nodes {
              number
              headRef {
                name
                repository {
                  name
                  owner {
                    login
                  }
                }
              }
            }
          }
        }
      }'

    $data = @{
        query     = $query
        variables = @{
            repoOwner   = $RepoOwner
            repoName    = $RepoName
            baseRefName = $BaseBranch
        }
    }

    $resp = Invoke-RestMethod -Method Post `
        -Headers $headers `
        https://api.github.com/graphql `
        -Body ($data | ConvertTo-Json)
    $resp | Write-Verbose

    $matchingPr = $resp.data.repository.pullRequests.nodes `
        | ? { $_.headRef.name -eq $mergeBranchName -and $_.headRef.repository.owner.login -eq $prOwnerName } `
        | select -First 1

    if ($matchingPr) {
        $prUpdatedSuccess = $false

        try {
            if ($PSCmdlet.ShouldProcess("Update remote branch $mergeBranchName on $remoteName")) {
                Invoke-Block { & git push $remoteName "${mergeBranchName}:${mergeBranchName}" }
            }
            $prUpdatedSuccess = $true
        }
        catch {
            Write-Warning "Failed to update existing PR"
        }

        $prMessage = if ($prUpdatedSuccess) {
            "This pull request has been updated.`n`n$committersList"
        } else {
            @"
:x: Uh oh, this pull request could not be updated automatically. New commits were pushed to $HeadBranch, but I could not automatically push those to $mergeBranchName to update this PR.
You may need to fix this problem by merging branches with this PR. Contact .NET Core Engineering if you are not sure what to do about this.
"@
        }

        $data = @{
            body = $prMessage
        }

        $prNumber = $matchingPr.number
        $prUrl = "https://github.com/$RepoOwner/$RepoName/pull/$prNumber"

        if ($PSCmdlet.ShouldProcess("Update $prUrl") -and -not $QuietComments) {
            $resp = Invoke-RestMethod -Method Post -Headers $headers `
                "https://api.github.com/repos/$RepoOwner/$RepoName/issues/$prNumber/comments" `
                -Body ($data | ConvertTo-Json)
            $resp | Write-Verbose
            Write-Host -f green "Updated pull request $url"
        }
    }
    else {
        # Use --force because the merge branch may have been used for a previous PR.
        # This should only happen if there is no existing PR for the merge

        if ($PSCmdlet.ShouldProcess("Force updating remote branch $mergeBranchName on $remoteName")) {
            Invoke-Block { & git push --force $remoteName "${mergeBranchName}:${mergeBranchName}" }
        }

        $previewHeaders = @{
            #  Required while this api is in preview: https://developer.github.com/v3/pulls/#create-a-pull-request
            Accept        = 'application/vnd.github.symmetra-preview+json'
            Authorization = "bearer $AuthToken"
        }

        $prBody = @"
I detected changes in the $HeadBranch branch which have not been merged yet to $BaseBranch. I'm a robot and am [configured](https://github.com/dotnet/versions/blob/main/Maestro/subscriptions.json) to help you automatically keep $BaseBranch up to date, so I've opened this PR.

$committersList

## Instructions for merging from UI

This PR will not be auto-merged. When pull request checks pass, complete this PR by creating a merge commit, *not* a squash or rebase commit.

<img alt="merge button instructions" src="https://i.imgur.com/GepcNJV.png" width="300" />

If this repo does not allow creating merge commits from the GitHub UI, use command line instructions.

## Instructions for merging via command line

Run these commands to merge this pull request from the command line.

`````` sh
git fetch
git checkout ${HeadBranch}
git pull --ff-only
git checkout ${baseBranch}
git pull --ff-only
git merge --no-ff ${HeadBranch}

# If there are merge conflicts, resolve them and then run `git merge --continue` to complete the merge
# Pushing the changes to the PR branch will re-trigger PR validation.
git push https://github.com/$prOwnerName/$prRepoName HEAD:${mergeBranchName}
``````

<details>
<summary>or if you are using SSH</summary>

``````
git push git@github.com:$prOwnerName/$prRepoName HEAD:${mergeBranchName}
``````

</details>


After PR checks are complete push the branch
``````
git push
``````

## Instructions for resolving conflicts

:warning: If there are merge conflicts, you will need to resolve them manually before merging. You can do this [using GitHub][resolve-github] or using the [command line][resolve-cli].

[resolve-github]: https://help.github.com/articles/resolving-a-merge-conflict-on-github/
[resolve-cli]: https://help.github.com/articles/resolving-a-merge-conflict-using-the-command-line/

## Instructions for updating this pull request

Contributors to this repo have permission update this pull request by pushing to the branch '$mergeBranchName'. This can be done to resolve conflicts or make other changes to this pull request before it is merged.

``````
git checkout -b ${mergeBranchName} $BaseBranch
git pull https://github.com/$prOwnerName/$prRepoName ${mergeBranchName}
(make changes)
git commit -m "Updated PR with my changes"
git push https://github.com/$prOwnerName/$prRepoName HEAD:${mergeBranchName}
``````

<details>
    <summary>or if you are using SSH</summary>

``````
git checkout -b ${mergeBranchName} $BaseBranch
git pull git@github.com:$prOwnerName/$prRepoName ${mergeBranchName}
(make changes)
git commit -m "Updated PR with my changes"
git push git@github.com:$prOwnerName/$prRepoName HEAD:${mergeBranchName}
``````

</details>

Contact .NET Core Engineering if you have questions or issues.
Also, if this PR was generated incorrectly, help us fix it. See https://github.com/dotnet/arcade/blob/master/scripts/GitHubMergeBranches.ps1.

"@;

        $data = @{
            title                 = "[automated] Merge branch '$HeadBranch' => '$BaseBranch'"
            head                  = "${prOwnerName}:${mergeBranchName}"
            base                  = $BaseBranch
            body                  = $prBody
            maintainer_can_modify = $true
        }

        if ($PSCmdlet.ShouldProcess("Create PR from ${prOwnerName}:${mergeBranchName} to $BaseBranch on $Reponame")) {
            $resp = Invoke-RestMethod -Method POST -Headers $previewHeaders `
                https://api.github.com/repos/$RepoOwner/$RepoName/pulls `
                -Body ($data | ConvertTo-Json)
            $resp | Write-Verbose
            Write-Host -f green "Created pull request https://github.com/$RepoOwner/$RepoName/pull/$($resp.number)"
        }
    }
}
finally {
    Pop-Location
}
