function ConvertTo-Base64Url {
  param([Parameter(Mandatory=$true)] [byte[]] $Bytes)

  [Convert]::ToBase64String($Bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_')
}

function New-GitHubAppJwt {
  param(
    [Parameter(Mandatory=$true)] [string] $AppId,
    [Parameter(Mandatory=$true)] [string] $PrivateKeyPem,
    [DateTimeOffset] $Now = [DateTimeOffset]::UtcNow
  )

  $header = [ordered]@{
    alg = 'RS256'
    typ = 'JWT'
  }
  $payload = [ordered]@{
    iat = $Now.AddMinutes(-1).ToUnixTimeSeconds()
    exp = $Now.AddMinutes(5).ToUnixTimeSeconds()
    iss = $AppId
  }

  $headerEncoded = ConvertTo-Base64Url ([Text.Encoding]::UTF8.GetBytes(($header | ConvertTo-Json -Compress)))
  $payloadEncoded = ConvertTo-Base64Url ([Text.Encoding]::UTF8.GetBytes(($payload | ConvertTo-Json -Compress)))
  $signingInput = "$headerEncoded.$payloadEncoded"

  $rsa = [Security.Cryptography.RSA]::Create()
  try {
    $rsa.ImportFromPem($PrivateKeyPem)
    $signature = $rsa.SignData(
      [Text.Encoding]::UTF8.GetBytes($signingInput),
      [Security.Cryptography.HashAlgorithmName]::SHA256,
      [Security.Cryptography.RSASignaturePadding]::Pkcs1)
  }
  finally {
    $rsa.Dispose()
  }

  "$signingInput.$(ConvertTo-Base64Url $signature)"
}

function Get-GitHubAppInstallationToken {
  param(
    [Parameter(Mandatory=$true)] [string] $Jwt,
    [Parameter(Mandatory=$true)] [string] $InstallationOwner,
    [Parameter(Mandatory=$true)] [string[]] $RepositoryNames,
    [string] $GitHubApiUrl = 'https://api.github.com',
    [scriptblock] $RequestInvoker
  )

  if (-not $RequestInvoker) {
    $RequestInvoker = {
      param($Method, $Uri, $Headers, $Body)

      $parameters = @{
        Method = $Method
        Uri = $Uri
        Headers = $Headers
      }
      if ($Body) {
        $parameters.Body = $Body
        $parameters.ContentType = 'application/json'
      }
      Invoke-RestMethod @parameters
    }
  }

  $headers = @{
    Authorization = "Bearer $Jwt"
    Accept = 'application/vnd.github+json'
    'X-GitHub-Api-Version' = '2022-11-28'
    'User-Agent' = 'dotnet-vmr-artifact-comparison'
  }

  $installations = [Collections.Generic.List[object]]::new()
  $page = 1
  do {
    $pageResponse = @(& $RequestInvoker 'GET' "$GitHubApiUrl/app/installations?per_page=100&page=$page" $headers $null)
    foreach ($installation in $pageResponse) {
      $installations.Add($installation)
    }
    $page++
  } while ($pageResponse.Count -eq 100)

  $matches = @($installations | Where-Object { $_.account.login -ieq $InstallationOwner })
  if ($matches.Count -eq 0) {
    $found = ($installations | ForEach-Object { $_.account.login }) -join ', '
    throw "No GitHub App installation found for '$InstallationOwner'. Installations found: $found"
  }
  if ($matches.Count -ne 1) {
    $ids = ($matches | ForEach-Object { $_.id }) -join ', '
    throw "Multiple GitHub App installations found for '$InstallationOwner': $ids"
  }

  $requestBody = @{
    repositories = @($RepositoryNames)
    permissions = @{
      contents = 'read'
    }
  } | ConvertTo-Json -Depth 3 -Compress

  & $RequestInvoker 'POST' "$GitHubApiUrl/app/installations/$($matches[0].id)/access_tokens" $headers $requestBody
}

Export-ModuleMember -Function New-GitHubAppJwt, Get-GitHubAppInstallationToken
