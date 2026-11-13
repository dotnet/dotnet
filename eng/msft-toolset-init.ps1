### Initializes a Microsoft build from an externally supplied SDK and package set.
### This must be loaded before eng/common/tools.ps1 so the latter sees the updated global.json.

function Update-GlobalJsonSdkVersion([string]$globalJsonFile, [string]$sdkVersion) {
  $content = Get-Content -Raw -Path $globalJsonFile

  $content = [regex]::Replace(
    $content,
    '("sdk"\s*:\s*\{[^}]*?"version"\s*:\s*")[^"]*(")',
    { param($m) $m.Groups[1].Value + $sdkVersion + $m.Groups[2].Value })

  $content = [regex]::Replace(
    $content,
    '("tools"\s*:\s*\{[^}]*?"dotnet"\s*:\s*")[^"]*(")',
    { param($m) $m.Groups[1].Value + $sdkVersion + $m.Groups[2].Value })

  Set-Content -Path $globalJsonFile -Value $content -NoNewline
}

function Update-GlobalJsonMSBuildSdkVersion([string]$globalJsonFile, [string]$sdkId, [string]$sdkVersion) {
  $content = Get-Content -Raw -Path $globalJsonFile
  $pattern = '("' + [regex]::Escape($sdkId) + '"\s*:\s*")[^"]*(")'

  $content = [regex]::Replace(
    $content,
    $pattern,
    { param($m) $m.Groups[1].Value + $sdkVersion + $m.Groups[2].Value })

  Set-Content -Path $globalJsonFile -Value $content -NoNewline
}

function Add-NuGetSourceToConfig([string]$nuGetConfigFile, [string]$sourceName, [string]$sourcePath) {
  if (-not (Test-Path $nuGetConfigFile)) {
    throw "NuGet.config '$nuGetConfigFile' does not exist"
  }

  $content = Get-Content -Raw -Path $nuGetConfigFile
  $content = [regex]::Replace($content, '[ \t]*<add\s+key="' + [regex]::Escape($sourceName) + '"[^>]*/>\r?\n', '')
  $entry = '    <add key="' + $sourceName + '" value="' + $sourcePath + '" />'
  $content = [regex]::Replace(
    $content,
    '(<packageSources>\s*<clear\s*/>|<packageSources>)',
    { param($m) $m.Groups[1].Value + "`r`n" + $entry },
    1)

  Set-Content -Path $nuGetConfigFile -Value $content -NoNewline
}

function Find-PackageVersion([string]$packagesDir, [string]$packageId) {
  $package = Get-ChildItem -Path $packagesDir -Recurse -File -Filter "$packageId.*.nupkg" -ErrorAction SilentlyContinue |
    Where-Object { $_.BaseName -match ('^' + [regex]::Escape($packageId) + '\.\d') } |
    Sort-Object Name |
    Select-Object -First 1

  if ($null -eq $package) {
    return $null
  }

  return $package.BaseName.Substring($packageId.Length + 1)
}

function Initialize-MsftToolset([string]$repoRoot, [string]$customSdkDir, [string]$customPackagesDir) {
  $extraProperties = @()

  if ($customSdkDir) {
    if (-not (Test-Path $customSdkDir -PathType Container)) {
      throw "Custom SDK directory '$customSdkDir' does not exist"
    }

    $customSdkDir = (Resolve-Path $customSdkDir).Path
    $dotnetExe = Join-Path $customSdkDir 'dotnet.exe'
    if (-not (Test-Path $dotnetExe)) {
      throw "Custom SDK '$dotnetExe' does not exist"
    }

    $sdkLines = @(& $dotnetExe --list-sdks)
    if ($LASTEXITCODE -ne 0 -or $sdkLines.Count -eq 0) {
      throw "Could not determine the SDK version of the custom SDK at '$customSdkDir'"
    }

    $sdkVersion = ($sdkLines[-1] -replace '\s*\[.*$', '').Trim()
    if (-not (Test-Path (Join-Path $customSdkDir "sdk\$sdkVersion") -PathType Container)) {
      throw "Custom SDK '$customSdkDir' does not contain 'sdk\$sdkVersion'"
    }

    Write-Host "Using custom bootstrap SDK from '$customSdkDir', version '$sdkVersion'"
    Update-GlobalJsonSdkVersion (Join-Path $repoRoot 'global.json') $sdkVersion
    $env:DOTNET_INSTALL_DIR = $customSdkDir
    $env:DOTNET_ROOT = $customSdkDir
  }

  if ($customPackagesDir) {
    if (-not (Test-Path $customPackagesDir -PathType Container)) {
      throw "Custom packages directory '$customPackagesDir' does not exist"
    }

    $customPackagesDir = (Resolve-Path $customPackagesDir).Path
    Write-Host "Using custom bootstrap packages from '$customPackagesDir'"
    Add-NuGetSourceToConfig (Join-Path $repoRoot 'NuGet.config') 'bootstrap-packages' $customPackagesDir

    $arcadeVersion = Find-PackageVersion $customPackagesDir 'Microsoft.DotNet.Arcade.Sdk'
    if (-not $arcadeVersion) {
      throw "Microsoft.DotNet.Arcade.Sdk package not found under '$customPackagesDir'"
    }

    Write-Host "Using bootstrap Arcade SDK version '$arcadeVersion'"
    Update-GlobalJsonMSBuildSdkVersion (Join-Path $repoRoot 'global.json') 'Microsoft.DotNet.Arcade.Sdk' $arcadeVersion
    $extraProperties += "/p:CustomPreviouslySourceBuiltPackagesPath=$customPackagesDir"
    $extraProperties += "/p:RestoreAdditionalProjectSources=$customPackagesDir"
  }

  return $extraProperties
}
