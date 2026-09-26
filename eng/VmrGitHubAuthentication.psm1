$script:RepositoriesByOwner = [ordered]@{
  dotnet = @(
    'arcade'
    'deployment-tools'
    'diagnostics'
    'fsharp'
    'msbuild'
    'roslyn'
  )
  nuget = @(
    'NuGet.Client'
  )
  microsoft = @(
    'vstest'
  )
}

function Get-VmrGitHubRepositoriesByOwner {
  $copy = [ordered]@{}
  foreach ($entry in $script:RepositoriesByOwner.GetEnumerator()) {
    $copy[$entry.Key] = @($entry.Value)
  }
  $copy
}

function Get-VmrGitHubRepository {
  param([Parameter(Mandatory=$true)] [string] $RemoteUri)

  try {
    $uri = [Uri]$RemoteUri
  }
  catch {
    throw "Repository URI '$RemoteUri' is invalid."
  }

  if ($uri.Scheme -ne 'https' -or $uri.Host -ine 'github.com') {
    throw "Repository URI '$RemoteUri' is not an HTTPS GitHub repository."
  }

  $segments = @($uri.AbsolutePath.Trim('/') -split '/')
  if ($segments.Count -ne 2) {
    throw "Repository URI '$RemoteUri' must identify exactly one GitHub owner and repository."
  }

  $owner = $segments[0].ToLowerInvariant()
  $repository = $segments[1] -replace '\.git$', ''
  if (-not $script:RepositoriesByOwner.Contains($owner)) {
    throw "GitHub owner '$owner' is not configured for VMR artifact comparison."
  }
  if ($repository -notin $script:RepositoriesByOwner[$owner]) {
    throw "GitHub repository '$owner/$repository' is not configured for VMR artifact comparison."
  }

  [pscustomobject]@{
    Owner = $owner
    Repository = $repository
    FullName = "$owner/$repository"
  }
}

function Get-VmrGitHubToken {
  param(
    [Parameter(Mandatory=$true)] [string] $RemoteUri,
    [Parameter(Mandatory=$true)] [System.Collections.IDictionary] $TokensByOwner
  )

  $repository = Get-VmrGitHubRepository -RemoteUri $RemoteUri
  if (-not $TokensByOwner.Contains($repository.Owner) -or
      [string]::IsNullOrWhiteSpace([string]$TokensByOwner[$repository.Owner])) {
    throw "No GitHub App installation token was provided for owner '$($repository.Owner)'."
  }

  [string]$TokensByOwner[$repository.Owner]
}

Export-ModuleMember -Function Get-VmrGitHubRepositoriesByOwner, Get-VmrGitHubRepository, Get-VmrGitHubToken
