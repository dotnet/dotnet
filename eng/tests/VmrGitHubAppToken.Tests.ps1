$ErrorActionPreference = 'Stop'

Import-Module "$PSScriptRoot/../GitHubAppToken.psm1" -Force
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

function ConvertFrom-Base64Url {
  param([Parameter(Mandatory=$true)] [string] $Value)

  $base64 = $Value.Replace('-', '+').Replace('_', '/')
  switch ($base64.Length % 4) {
    2 { $base64 += '==' }
    3 { $base64 += '=' }
  }
  [Convert]::FromBase64String($base64)
}

$rsa = [Security.Cryptography.RSA]::Create(2048)
try {
  $privateKey = $rsa.ExportRSAPrivateKeyPem()
  $now = [DateTimeOffset]::Parse('2026-09-22T20:00:00Z')
  $jwt = New-GitHubAppJwt -AppId '4992078' -PrivateKeyPem $privateKey -Now $now
  $segments = $jwt.Split('.')

  Assert-Equal 3 $segments.Count 'JWT must have three segments.'
  $header = [Text.Encoding]::UTF8.GetString((ConvertFrom-Base64Url $segments[0])) | ConvertFrom-Json
  $payload = [Text.Encoding]::UTF8.GetString((ConvertFrom-Base64Url $segments[1])) | ConvertFrom-Json
  Assert-Equal 'RS256' $header.alg 'JWT algorithm is incorrect.'
  Assert-Equal '4992078' $payload.iss 'JWT issuer is incorrect.'
  Assert-Equal $now.AddMinutes(-1).ToUnixTimeSeconds() $payload.iat 'JWT issued-at time is incorrect.'
  Assert-Equal $now.AddMinutes(5).ToUnixTimeSeconds() $payload.exp 'JWT expiration is incorrect.'

  $signatureValid = $rsa.VerifyData(
    [Text.Encoding]::UTF8.GetBytes("$($segments[0]).$($segments[1])"),
    (ConvertFrom-Base64Url $segments[2]),
    [Security.Cryptography.HashAlgorithmName]::SHA256,
    [Security.Cryptography.RSASignaturePadding]::Pkcs1)
  Assert-Equal $true $signatureValid 'JWT signature is invalid.'
}
finally {
  $privateKey = $null
  $rsa.Dispose()
}

$requests = [Collections.Generic.List[object]]::new()
$requestInvoker = {
  param($Method, $Uri, $Headers, $Body)

  $requests.Add([pscustomobject]@{
    Method = $Method
    Uri = $Uri
    Body = $Body
  })
  if ($Method -eq 'GET' -and $Uri -match '/app/installations\?') {
    return @(
      [pscustomobject]@{
        id = 40
        account = [pscustomobject]@{ login = 'NuGet' }
      }
      [pscustomobject]@{
        id = 41
        account = [pscustomobject]@{ login = 'microsoft' }
      }
      [pscustomobject]@{
        id = 42
        account = [pscustomobject]@{ login = 'dotnet' }
      }
    )
  }
  [pscustomobject]@{
    token = 'test-installation-token'
    expires_at = '2026-09-22T21:00:00Z'
  }
}.GetNewClosure()

$response = Get-GitHubAppInstallationToken `
  -Jwt 'test-jwt' `
  -InstallationOwner 'dotnet' `
  -RepositoryNames @('arcade', 'roslyn') `
  -GitHubApiUrl 'https://example.invalid' `
  -RequestInvoker $requestInvoker

Assert-Equal 'test-installation-token' $response.token 'Installation token was not returned.'
Assert-Equal 2 $requests.Count 'Unexpected number of GitHub API requests.'
Assert-Equal 'https://example.invalid/app/installations/42/access_tokens' $requests[1].Uri 'The wrong installation was selected.'
$tokenRequest = $requests[1].Body | ConvertFrom-Json
Assert-Equal 'read' $tokenRequest.permissions.contents 'Token permissions were not downscoped.'
Assert-Equal 'arcade' $tokenRequest.repositories[0] 'First repository restriction is incorrect.'
Assert-Equal 'roslyn' $tokenRequest.repositories[1] 'Second repository restriction is incorrect.'

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
    $gatherDrops -match 'githubPat|--continue-on-error') {
  throw 'The VMR comparison pipeline must not retain the classic PAT or continue after an individual gather failure.'
}

Write-Host 'VMR GitHub App token tests passed.'
