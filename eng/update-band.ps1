[CmdletBinding(PositionalBinding=$false)]
Param(
    [Parameter(Mandatory = $true)]
    [string]$Remote,

    [Parameter(Mandatory = $true)]
    [Alias("branch-1xx")]
    [string]$Branch1xx,

    [switch][Alias('h')]$help
)

function Get-Usage {
    Write-Host "  -Remote <name>           Git remote to pull from (e.g., upstream)"
    Write-Host "  -1xx-branch <branch>     1xx branch name to merge in (e.g., main)"
    Write-Host ""
    Write-Host "  -help                    Print help and exit (short: -h)"
}

if ($help) {
  Get-Usage
  exit 0
}

# List of deleted repos
$DeletedRepos = @(
    "aspire", "aspnetcore", "cecil", "command-line-api", "deployment-tools",
    "diagnostics", "efcore", "emsdk", "runtime", "source-build-externals",
    "sourcelink", "symreader", "windowsdesktop", "winforms", "wpf", "xdt"
)

# Attempt merge
$mergeSuccess = git merge --no-commit --no-ff "$Remote/$Branch1xx" 2>$null

if (-not $?) {
    Write-Output "Cleaning excluded paths..."

    foreach ($repo in $DeletedRepos) {
        if (Test-Path "src/$repo") {
            git reset HEAD -- "$path" 2>$null
            git rm -rf --cached "$path" 2>$null
            Remove-Item -Recurse -Force "$path" -ErrorAction SilentlyContinue
        }
    }

    $mergeMsg = Get-Content ".git/MERGE_MSG" -TotalCount 1
    git commit -m "$mergeMsg"
}

Write-Output "Completed merge"
