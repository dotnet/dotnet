<#
.SYNOPSIS
    Create release notes for current project using either last commit to last tag or between last 2 tags.

.EXAMPLE
    Assuming you are on branch rel/17.4 and you want to create the release notes, run
    .\write-release-notes -PackageVersion 17.4.0
    it will create release notes between last commit of current branch and last release

    If you want to generate the release notes between 2 tags, use simply
    .\write-release-notes
#>

[CmdletBinding()]
param
(
    [string] $Path = ".",
    [ValidatePattern("^\d+\.\d+\.\d+(-(preview|release)-\d{8}-\d{2})?$")][string] $PackageVersion
)

$repoUrl = $(if ((git -C $Path remote -v) -match "upstream") {
        git -C $Path remote get-url --push upstream
    }
    else {
        git -C $Path remote get-url --push origin
    }) -replace "\.git$"

# list all tags on this branch ordered by creator date to get the latest, stable or pre-release tag.
# For stable release we choose only tags without any dash, for pre-release we choose all tags.
$tags = git -C $Path tag -l --sort=refname | Where-Object { $_ -match "v\d+\.\d+\.\d+.*" -and (-not $Stable -or $_ -notlike '*-*') }

if ([string]::IsNullOrWhiteSpace($PackageVersion)) {
    # normally we show changes between the latest two tags
    $start, $end = $tags | Select-Object -Last 2
    Write-Host "$start -- $end"
    $tag = $end
}
else {
    # in CI we don't have the tag yet, so we show changes between the most recent tag, and this commit
    # we figure out the tag from the package version that is set by vsts-prebuild
    $start = $tags | Select-Object -Last 1
    $end = git -C $Path rev-parse HEAD
    $tag = "v$PackageVersion"
}

# # override the tags to use if you need
# $start = "v16.8.0-preview-20200812-03"
# $end = $tag = "v16.8.0-preview-20200921-01"

Write-Host "Generating release notes for $start..$end$(if ($HasPackageVersion) { " (expected tag: $tag)" })"

$sourceBranch = $branch = git -C $Path rev-parse --abbrev-ref HEAD
if ($sourceBranch -eq "HEAD") {
    # when CI checks out just the single commit, https://docs.microsoft.com/en-us/azure/devops/pipelines/build/variables?view=azure-devops&tabs=yaml
    $sourceBranch = $env:BUILD_SOURCEBRANCH -replace "^refs/heads/"
}

if ([string]::IsNullOrWhiteSpace($branch)) {
    throw "Branch is null or empty!"
}

if ([string]::IsNullOrWhiteSpace($sourceBranch)) {
    throw "SourceBranch is null or empty!"
}

Write-Host "Branch is $branch"
Write-Host "SourceBranch is $sourceBranch"

$discard = @(
    "^Update dependencies from https:\/\/",
    "^\[.+\] Update dependencies from",
    "^LEGO: Pull request from lego",
    "^Localized file check-in by OneLocBuild Task:",
    "^Juno: check in to lego"
) -join "|"

$prUrl = "$repoUrl/pull/"
$tagVersionNumber = $tag -replace '^v'
$b = if ($HasPackageVersion) { $tagVersionNumber } else { $tag -replace '.*?(\d+-\d+)$', '$1' }
# using .. because I want to know the changes that are on this branch, but don't care about the changes that I don't have https://stackoverflow.com/a/24186641/3065397
$log = (git -C $Path log "$start..$end" --oneline --pretty="format:%s" --first-parent)
$issues = $log | ForEach-Object {
    if ($_ -notmatch $discard) {
        if ($_ -match '^(?<message>.+)\s\(#(?<pr>\d+)\)?$') {
            $message = "* $($matches.message)"
            if ($matches.pr) {
                $pr = $matches.pr
                $message += " [#$pr]($prUrl$pr)"
            }

            $message
        }
        else {
            "* $_"
        }
    }
}

$tagVersionNumbersixDropBranch = $sourceBranch -replace "rel/", ""

$output = @"

See the release notes [here](https://github.com/microsoft/vstest/blob/main/docs/releases.md#$($tagVersionNumber -replace '\.')).

-------------------------------

## $tagVersionNumber

### $(if ($issues.Length -eq 1) { 'Issue' } else { 'Issues' }) Fixed

$($issues -join "`n")

See full log [here]($repoUrl/compare/$start...$tag)

### Artifacts

* TestPlatform vsix: [$tagVersionNumber](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/$tagVersionNumbersixDropBranch/$b;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel : [$tagVersionNumber](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/$tagVersionNumber)
"@


$output
$output | clip
