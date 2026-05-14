<#
.SYNOPSIS
Checks a set of repositories involved in "normal" flow for .NET 6 previews and servicing for their mirror states. Each repo is checked for having an up-to-date internal fork, as well as an internal/ merge branch, if applicable.

.PARAMETER RepoRoot
Directory containing repos.

.PARAMETER RuntimeBranch
Branch for runtime repos

.PARAMETER SdkBranch
Branch for sdk repos

.PARAMETER NoFetch
Don't automatically fetch repos after they are found (assume they are up to date)

#>

param
(
    [Parameter(Mandatory=$true)][string]$RepoRoot,
    [Parameter(Mandatory=$true)][string]$RuntimeBranch,
    [Parameter(Mandatory=$true)][string[]]$SdkBranches,
    [Parameter(Mandatory=$true)][ValidateSet('5.0','6.0','7.0')][string]$ReleaseBand,
    [switch]$NoFetch = $false
)

$repos = @(
    @{
        org="dotnet";
        repo="runtime";
        branches=@($RuntimeBranch);
        hasInternal=$true;
        appliesTo=@("5.0","6.0","7.0");
    },
    @{
        org="dotnet";
        repo="emsdk";
        branches=@($RuntimeBranch);
        hasInternal=$false;
        appliesTo=@("6.0","7.0");
    },
    @{
        org="dotnet";
        repo="aspnetcore";
        branches=@($RuntimeBranch);
        hasInternal=$true;
        appliesTo=@("5.0","6.0","7.0");
    },
    @{
        org="dotnet";
        repo="efcore";
        branches=@($RuntimeBranch);
        hasInternal=$true;
        appliesTo=@("5.0","6.0","7.0");
    },
    @{
        org="dotnet";
        repo="windowsdesktop";
        branches=@($RuntimeBranch);
        hasInternal=$true;
        appliesTo=@("5.0","6.0","7.0");
    },
    @{
        org="dotnet";
        repo="winforms";
        branches=@($RuntimeBranch);
        hasInternal=$true;
        appliesTo=@("5.0","6.0","7.0");
    },
    @{
        org="dotnet";
        repo="wpf";
        branches=@($RuntimeBranch);
        hasInternal=$true;
        appliesTo=@("5.0","6.0","7.0");
    },
    @{
        org="dotnet";
        repo="templating";
        branches=$SdkBranches;
        hasInternal=$true;
        appliesTo=@("5.0","6.0","7.0");
    },
    @{
        org="dotnet";
        repo="sdk";
        branches=$SdkBranches;
        hasInternal=$true;
        appliesTo=@("5.0","6.0","7.0");
    },
    @{
        org="dotnet";
        repo="installer";
        branches=$SdkBranches;
        hasInternal=$true;
        appliesTo=@("5.0","6.0","7.0");
    }
)

function Fetch($dir, $remoteName)
{
    try {
        pushd
        $fetchOutput = & git fetch $remoteName 2>&1
        if ($LASTEXITCODE -ne 0) {
            Write-Error "Failed to fetch in $dir"
            WRite-Host $fetchOutput
        }
    } finally {
        popd
    }
}

function CheckDirectMirror($dir, $publicRemoteName, $internalRemoteName, $publicRepo, $internalRepo, $branch)
{
    try {
        pushd
        Write-Host "  Checking $publicRepo ($publicRemoteName) @ $branch == $internalRepo ($internalRemoteName) @ $branch"
        $publicRev = & git rev-parse "$publicRemoteName/$branch"
        if (!$?) {
            Write-Error "Could not get rev for $publicRemoteName/$branch"
        }
        $internalRev = & git rev-parse "$internalRemoteName/$branch"
        if (!$?) {
            Write-Error "Could not get rev for $internalRemoteName/$branch"
        }
        
        if ($publicRev -ne $internalRev) {
            Write-Error "$publicRepo @ $branch != $internalRepo @ $branch"
        } else {
            Write-Host "    $publicRev == $internalRev"
        }
    } finally {
        popd
    }
}

function CheckMergeMirror($dir, $publicRemoteName, $internalRemoteName, $publicRepo, $internalRepo, $branch)
{
    try {
        pushd
        Write-Host "  Checking $internalRepo ($internalRemoteName) @ internal/$branch contains all of $publicRepo ($publicRemoteName) @ $branch"
        $branchesThatContain = & git branch --all --contains "$publicRemoteName/$branch"
        foreach ($cBranch in $branchesThatContain) {
            if ($cBranch.IndexOf("remotes/$internalRemoteName/internal/$branch") -ne -1) {
                $cBranch = $cBranch.Trim()
                Write-Host "    $cBranch contains $publicRemoteName/$branch"
                return
            }
        }
        Write-Error "$internalRepo @ internal/$branch is missing commits from $publicRepo @ $branch"
    } finally {
        popd
    }
}


$availableDirs = Get-ChildItem -Directory $RepoRoot

foreach ($repo in $repos) {
    
    Write-Host "Finding & checking $($repo.org)/$($repo.repo)"
    
    if ($repo.appliesTo.indexOf($ReleaseBand) -eq -1) {
        Write-Host "  $($repo.org)/$($repo.repo) does not apply to $ReleaseBand, skipping"
        continue
    }

    $publicRepo = "https://github.com/$($repo.org)/$($repo.repo)"
    $internalRepo = "https://dnceng@dev.azure.com/dnceng/internal/_git/$($repo.org)-$($repo.repo)"
   
    $repoDir = $null
    
    # Walk dir and find a git repo with both of those remotes
    foreach ($dir in $availableDirs) {
        try {
            pushd $dir
            $remotes = & git remote -v 2>&1
            if ($?) {
                $publicRemoteName = $null
                $internalRemoteName = $null

                foreach ($remote in $remotes) {
                    if ($remote.IndexOf("$publicRepo (fetch)") -ne -1) {
                        $publicRemoteName = $remote.Substring(0, $remote.IndexOf("`t"))
                    } elseif ($remote.IndexOf("$internalRepo (fetch)") -ne -1) {
                        $internalRemoteName = $remote.Substring(0, $remote.IndexOf("`t"))
                    }

                    if ($publicRemoteName -ne $null -and $internalRemoteName -ne $null) {
                        $repoDir = $dir
                        break
                    }
                }

                if ($repoDir) {
                    if (!$NoFetch) {
                        Fetch $repoDir $publicRemoteName
                        Fetch $repoDir $internalRemoteName
                    }
                    foreach ($branchToCheck in $repo.branches) {
                        CheckDirectMirror $repoDir $publicRemoteName $internalRemoteName $publicRepo $internalRepo $branchToCheck
                        if ($repo.hasInternal) {
                            CheckMergeMirror $repoDir $publicRemoteName $internalRemoteName $publicRepo $internalRepo $branchToCheck
                        }
                    }
                    break
                }
            }
        }
        finally {
            popd
        }
    }
    
    if ($repoDir -eq $null) {
        Write-Error "Repo $publicRepo/$internalRepo was not found in $RepoDir, please ensure that a git repo is contained within that has these remotes"
    }
}

