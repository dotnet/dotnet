#!/usr/bin/env pwsh

#Requires -PSEdition Core
#Requires -Version 7

param(
  [Parameter()][string]$repoRoot = (Get-Item $PSScriptRoot).Parent.Parent.FullName
)

$ErrorActionPreference = "Stop"

function GetUnreleasedChanges {
  param([string]$changelogPath)

  $content = (Get-Content $changelogPath -Raw) -replace "`r`n", "`n"
  $match = [regex]::Match($content, '## Unreleased[ \t]*\n([\s\S]*?)(?=\n## |\z)')
  if (-not $match.Success) {
    return $null
  }

  $section = $match.Groups[1].Value.Trim()
  if ([string]::IsNullOrWhiteSpace($section)) {
    return $null
  }

  return $section
}

function GetLastReleasedVersion {
  param([string]$changelogPath)

  $content = Get-Content $changelogPath -Raw
  $match = [regex]::Match($content, '## (\d+\.\d+\.\d+(?:-(?:alpha|beta|rc)\.\d+)?)')
  if (-not $match.Success) {
    return $null
  }

  return $match.Groups[1].Value
}

function HasUnshippedPublicApiChanges {
  param([string]$packageDir)

  $searchPath = Join-Path $packageDir ".publicApi"
  $unshippedFiles = Get-ChildItem -Path $searchPath -Filter "PublicAPI.Unshipped.txt" -Recurse -ErrorAction SilentlyContinue
  foreach ($file in $unshippedFiles) {
    $nonEmpty = Get-Content $file.FullName | Where-Object { $_ -ne '#nullable enable' -and -not [string]::IsNullOrWhiteSpace($_) }
    if ($nonEmpty.Count -gt 0) {
      return $true
    }
  }

  return $false
}

function GetSuggestedVersion {
  param(
    [string]$lastVersion,
    [string]$unreleasedChanges,
    [string]$repoOtelMajorMinor,
    [string]$packageDir
  )

  if ([string]::IsNullOrEmpty($lastVersion)) {
    if ([string]::IsNullOrEmpty($repoOtelMajorMinor)) {
      return $null
    }
    return "$repoOtelMajorMinor.0-alpha.1"
  }

  $lastVersionMatch = [regex]::Match($lastVersion, '^(\d+)\.(\d+)\.(\d+)(?:-(alpha|beta|rc)\.(\d+))?$')
  if (-not $lastVersionMatch.Success) {
    return $null
  }

  [int]$lastMajor = $lastVersionMatch.Groups[1].Value
  [int]$lastMinor = $lastVersionMatch.Groups[2].Value
  [int]$lastPatch = $lastVersionMatch.Groups[3].Value
  [string]$preReleaseType = $lastVersionMatch.Groups[4].Value
  [int]$preReleaseNum = if ($lastVersionMatch.Groups[5].Success) { $lastVersionMatch.Groups[5].Value } else { 0 }

  [int]$newMajor = $lastMajor
  [int]$newMinor = $lastMinor
  [bool]$majorMinorChanged = $false

  # A package is considered OTel-version-based if its major version matches OTel's
  # and its minor version is within 3 of the current OTel minor (i.e. it tracks OTel releases).
  # Packages with an independent versioning scheme (e.g. major=0 or minor far behind OTel) are not.
  [bool]$isOtelVersionBased = $false
  if (-not [string]::IsNullOrEmpty($repoOtelMajorMinor)) {
    $otelParts = $repoOtelMajorMinor -split '\.'
    [int]$otelMajor = $otelParts[0]
    [int]$otelMinor = $otelParts[1]
    $isOtelVersionBased = ($lastMajor -eq $otelMajor) -and (($otelMinor - $lastMinor) -ge 0) -and (($otelMinor - $lastMinor) -le 3)
  }

  if ($isOtelVersionBased) {
    # Use the last (most recent) OTel core version update found in the unreleased section
    $coreVersionMatches = [regex]::Matches($unreleasedChanges, 'Updated OpenTelemetry core component version\(s\) to `(\d+)\.(\d+)\.\d+`')
    if ($coreVersionMatches.Count -gt 0) {
      $latestCoreMatch = $coreVersionMatches[$coreVersionMatches.Count - 1]
      [int]$coreMajor = $latestCoreMatch.Groups[1].Value
      [int]$coreMinor = $latestCoreMatch.Groups[2].Value
      if ($coreMajor -ne $lastMajor -or $coreMinor -ne $lastMinor) {
        $newMajor = $coreMajor
        $newMinor = $coreMinor
        $majorMinorChanged = $true
      }
    }
  }
  else {
    # Non-OTel-based: keep the same major.minor unless there are unshipped public API changes,
    # in which case the minor version is incremented.
    if (-not [string]::IsNullOrEmpty($packageDir) -and (HasUnshippedPublicApiChanges -packageDir $packageDir)) {
      $newMinor = $lastMinor + 1
      $majorMinorChanged = $true
    }
  }

  if ([string]::IsNullOrEmpty($preReleaseType)) {
    [int]$newPatch = if ($majorMinorChanged) { 0 } else { $lastPatch + 1 }
    return "$newMajor.$newMinor.$newPatch"
  }
  else {
    [int]$newPatch = if ($majorMinorChanged) { 0 } else { $lastPatch }
    [int]$newNum = if ($majorMinorChanged) { 1 } else { $preReleaseNum + 1 }
    return "$newMajor.$newMinor.$newPatch-$preReleaseType.$newNum"
  }
}

$changelogs = Get-ChildItem -Path (Join-Path $repoRoot "src") -Filter "CHANGELOG.md" -Recurse

$propsContent = Get-Content (Join-Path $repoRoot "Directory.Packages.props") -Raw
$otelCoreMatch = [regex]::Match($propsContent, '<OpenTelemetryCoreLatestVersion>(\d+)\.(\d+)\.\d+</OpenTelemetryCoreLatestVersion>')
$repoOtelMajorMinor = if ($otelCoreMatch.Success) { "$($otelCoreMatch.Groups[1].Value).$($otelCoreMatch.Groups[2].Value)" } else { $null }

$packages = @()

foreach ($changelog in $changelogs) {
  $packageName = $changelog.Directory.Name
  $unreleased = GetUnreleasedChanges -changelogPath $changelog.FullName
  if ($null -eq $unreleased) {
    continue
  }

  $lastVersion = GetLastReleasedVersion -changelogPath $changelog.FullName
  $suggestedVersion = GetSuggestedVersion -lastVersion $lastVersion -unreleasedChanges $unreleased -repoOtelMajorMinor $repoOtelMajorMinor -packageDir $changelog.Directory.FullName

  $packages += [PSCustomObject]@{
    Name              = $packageName
    UnreleasedChanges = $unreleased
    LastVersion       = $lastVersion
    SuggestedVersion  = $suggestedVersion
  }
}

$packages = @($packages | Sort-Object Name)

if ($packages.Count -eq 0) {
  @("> [!TIP]", "> No packages have any unreleased changes.") -join "`n"
  return
}

$lines = [System.Collections.Generic.List[string]]::new()

$lines.Add("# Unreleased Changes")
$lines.Add("")
$lines.Add("| Package | Last Version | Suggested Next Version | Create Release Request |")
$lines.Add("| :--- | :--- | :--- | :--- |")

$issueBaseUrl = "https://github.com/open-telemetry/opentelemetry-dotnet-contrib/issues/new"

foreach ($pkg in $packages) {
  $lastVerCell = if ($pkg.LastVersion) { "[``$($pkg.LastVersion)``](https://www.nuget.org/packages/$($pkg.Name)/$($pkg.LastVersion))" } else { 'N/A' }
  $suggestedVer = if ($pkg.SuggestedVersion) { $pkg.SuggestedVersion } else { 'N/A' }

  $createLinkCell = 'N/A'
  if ($pkg.SuggestedVersion) {
    $issueTitle = [uri]::EscapeDataString("[release request] $($pkg.Name) $($pkg.SuggestedVersion)")
    $issueComponent = [uri]::EscapeDataString($pkg.Name)
    $issueUrl = "$issueBaseUrl`?template=release_request.yml&title=$issueTitle&labels=release&component=$issueComponent"
    $createLinkCell = ":spiral_notepad: [Create issue]($issueUrl)"
  }

  $lines.Add("| :package: $($pkg.Name -replace '^OpenTelemetry\.', '') | $lastVerCell | ``$suggestedVer`` | $createLinkCell |")
}

foreach ($pkg in $packages) {
  $lines.Add("")
  $lines.Add("## $($pkg.Name)")
  $lines.Add("")
  $lastVer = if ($pkg.LastVersion) { $pkg.LastVersion } else { 'N/A' }
  $suggestedVer = if ($pkg.SuggestedVersion) { $pkg.SuggestedVersion } else { 'N/A' }
  $lines.Add("**Last version:** ``$lastVer`` | **Suggested next version:** ``$suggestedVer``")
  $lines.Add("")
  if ($pkg.LastVersion) {
    $lines.Add($pkg.UnreleasedChanges)
  }
  else {
    $lines.Add("_All changes - this package has not yet been released._")
  }
}

$lines -join "`n"
