<#
.SYNOPSIS
    Updates a non-1xx branch of the VMR with the latest from a 1xx branch.

.DESCRIPTION
    This script pulls in updates from the target remote and branch, ignoring paths to repos that are excluded from non-1xx branches.

.PARAMETER Remote
    Git remote to pull from (e.g., upstream)

.PARAMETER Branch1xx
    1xx branch name to merge in (e.g., main)

.PARAMETER ContinueMerge
    Continue after manually fixing merge conflicts

#>

[CmdletBinding(PositionalBinding=$false)]
Param(
    [Parameter(Mandatory = $false)]
    [string]$Remote,

    [Parameter(Mandatory = $false)]
    [string]$Branch1xx,

    [switch]$ContinueMerge = $false
)

function Attempt-Merge {
    $conflicts = git diff --check | Out-String
    $unmergedFiles = git ls-files -u

    if ($conflicts.Trim() -ne "" -or $unmergedFiles.Count -gt 0) {
        Write-Error "There are unresolved conflicts. Please resolve them before continuing."
        exit 1
    } else {
        Write-Output "Continuing with merge..."
        $mergeMsg = Get-Content ".git/MERGE_MSG" -TotalCount 1
        git commit -m "$mergeMsg"
    }
}

# List of deleted repos
$DeletedRepos = @(
    "aspire", "aspnetcore", "cecil", "command-line-api", "deployment-tools",
    "diagnostics", "efcore", "emsdk", "runtime", "source-build-externals",
    "sourcelink", "symreader", "windowsdesktop", "winforms", "wpf", "xdt"
)

if (-not $ContinueMerge) {
    if (-not $Remote) {
        Write-Error "Error: Remote is required."
        Show-Usage
        exit 1
    }
    if (-not $Branch1xx) {
        Write-Error "Error: Branch1xx is required."
        Show-Usage
        exit 1
    }

    git diff --quiet
    $diffExit = $LASTEXITCODE

    git diff --cached --quiet
    $cachedExit = $LASTEXITCODE

    if ($diffExit -ne 0 -or $cachedExit -ne 0) {
        Write-Error "You have uncommitted changes. Please commit or stash them before continuing."
        exit 1
    }

    # Attempt merge
    $mergeTarget = "$Remote/$Branch1xx"
    $mergeResult = & git merge --no-commit --no-ff "$mergeTarget" 2>&1
    $mergeExitCode = $LASTEXITCODE

    if ($mergeExitCode -ne 0) {
        Write-Output "Cleaning excluded paths..."

        foreach ($repo in $DeletedRepos) {
            $repoPath = Join-Path -Path "src" -ChildPath $repo
            if (Test-Path $repoPath -PathType Container) {
                git reset HEAD -- "$repoPath" 2>$null -ErrorAction SilentlyContinue
                git rm -rf --cached "$repoPath" 2>$null -ErrorAction SilentlyContinue
                Remove-Item -Recurse -Force "$repoPath" -ErrorAction SilentlyContinue
            }
        }
    }
}

Attempt-Merge

Write-Output "Completed merge"
