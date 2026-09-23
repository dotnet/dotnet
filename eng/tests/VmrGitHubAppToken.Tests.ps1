$ErrorActionPreference = 'Stop'

Import-Module "$PSScriptRoot/../VmrGitHubAuthentication.psm1" -Force

function Assert-Equal {
  param($Expected, $Actual, [string] $Message)
  if ($Expected -ne $Actual) {
    throw "$Message Expected '$Expected', got '$Actual'."
  }
}

function Assert-Throws {
  param([scriptblock] $Action, [string] $Message)
  try {
    & $Action
  }
  catch {
    return
  }
  throw $Message
}

$repositoriesByOwner = Get-VmrGitHubRepositoriesByOwner
Assert-Equal 6 $repositoriesByOwner.dotnet.Count 'dotnet repository count is incorrect.'
Assert-Equal 'NuGet.Client' $repositoriesByOwner.nuget[0] 'NuGet repository is incorrect.'
Assert-Equal 'vstest' $repositoriesByOwner.microsoft[0] 'microsoft repository is incorrect.'

$tokens = @{
  dotnet = 'dotnet-token'
  nuget = 'nuget-token'
  microsoft = 'microsoft-token'
}
Assert-Equal 'dotnet-token' (Get-VmrGitHubToken -RemoteUri 'https://github.com/dotnet/arcade' -TokensByOwner $tokens) 'dotnet token selection failed.'
Assert-Equal 'nuget-token' (Get-VmrGitHubToken -RemoteUri 'https://github.com/NuGet/NuGet.Client' -TokensByOwner $tokens) 'NuGet token selection failed.'
Assert-Equal 'microsoft-token' (Get-VmrGitHubToken -RemoteUri 'https://github.com/microsoft/vstest' -TokensByOwner $tokens) 'microsoft token selection failed.'

Assert-Throws { Get-VmrGitHubToken -RemoteUri 'https://github.com/dotnet/runtime' -TokensByOwner $tokens } 'An unconfigured repository must fail.'
Assert-Throws { Get-VmrGitHubToken -RemoteUri 'https://github.com/unknown/repo' -TokensByOwner $tokens } 'An unknown owner must fail.'
Assert-Throws { Get-VmrGitHubToken -RemoteUri 'https://dev.azure.com/dnceng/internal/_git/dotnet-dotnet' -TokensByOwner $tokens } 'A non-GitHub repository must fail.'
Assert-Throws { Get-VmrGitHubToken -RemoteUri 'https://github.com/NuGet/NuGet.Client' -TokensByOwner @{ dotnet = 'token' } } 'A missing owner token must fail.'

$comparisonStage = Get-Content "$PSScriptRoot/../pipelines/templates/stages/vmr-compare.yml" -Raw
$comparisonSteps = Get-Content "$PSScriptRoot/../pipelines/templates/steps/vmr-compare.yml" -Raw
$gatherDrops = Get-Content "$PSScriptRoot/../GatherDrops.ps1" -Raw
if ($comparisonStage -match 'DotNetBot-GitHub-AllBranches' -or
    $comparisonSteps -match 'BotAccount-dotnet-bot-repo-PAT' -or
    $comparisonSteps -notmatch 'eng/common/Get-GitHubAppToken.ps1' -or
    $gatherDrops -match 'githubPat|--continue-on-error') {
  throw 'The VMR comparison pipeline must use the Arcade GitHub App helper without retaining the classic PAT or continuing after an individual gather failure.'
}

Write-Host 'VMR GitHub App token tests passed.'
