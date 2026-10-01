# Points the repo at a locally built Arcade/Helix SDK so a subsequent `build` uses the newly built SDK -
# both the signing validation (`build -sign`, exercising the new SignTool) and the self-build validation.
# It bumps the msbuild-sdks versions in global.json via a targeted regex and adds the produced packages
# as local NuGet feed(s) (with a packageSourceMapping entry so the new SDK resolves).
#
# Pure file edits (no dotnet/darc), so it is cross-platform (Windows/Linux/macOS). NOTE: darc
# update-dependencies --packages-folder can also do the version bump (arcade tracks Arcade.Sdk/Helix.Sdk
# in Version.Details.xml), but it additionally re-syncs eng/common from the remote arcade repo at the
# built commit - which needs a GitHub PAT and the commit to exist on github.com/dotnet/arcade, so it
# cannot run on dev/PR builds. This file-based bump works everywhere and needs no auth or network.

Param(
  [Parameter(Mandatory=$true)][string] $PackagesSource  # Folder containing the freshly built *.nupkg (e.g. the downloaded build artifacts).
)

$ErrorActionPreference = 'Stop'

$repoRoot = (Resolve-Path "$PSScriptRoot/../..").Path

# Discover the produced Arcade SDK version from the built package. Exclude *.symbols.nupkg so we don't
# pick a symbols package and extract a bogus version (e.g. '11.0.0-beta.xxx.symbols').
$arcadePackages = @(Get-ChildItem -Path $PackagesSource -Recurse -Filter 'Microsoft.DotNet.Arcade.Sdk.*.nupkg' -ErrorAction SilentlyContinue |
  Where-Object { $_.Name -notlike '*.symbols.nupkg' })
if ($arcadePackages.Count -eq 0) {
  throw "Could not find Microsoft.DotNet.Arcade.Sdk.*.nupkg under '$PackagesSource'."
}

$arcadeVersions = @($arcadePackages | ForEach-Object {
  if ($_.Name -notmatch '^Microsoft\.DotNet\.Arcade\.Sdk\.(.+)\.nupkg$') {
    throw "Unexpected Arcade SDK package name '$($_.Name)'."
  }
  $Matches[1]
} | Sort-Object -Unique)
if ($arcadeVersions.Count -ne 1) {
  throw "Expected one Arcade SDK version under '$PackagesSource', but found: $($arcadeVersions -join ', ')."
}
$version = $arcadeVersions[0]

# Both global.json SDK entries use the same build version. Require the matching Helix package before
# changing either entry so validation cannot restore Helix remotely or fail later with a missing SDK.
$helixPackages = @(Get-ChildItem -Path $PackagesSource -Recurse -Filter "Microsoft.DotNet.Helix.Sdk.$version.nupkg" -ErrorAction SilentlyContinue)
if ($helixPackages.Count -eq 0) {
  throw "Could not find Microsoft.DotNet.Helix.Sdk.$version.nupkg under '$PackagesSource'."
}

$sdkPackages = [ordered]@{
  'Microsoft.DotNet.Arcade.Sdk' = $arcadePackages
  'Microsoft.DotNet.Helix.Sdk' = $helixPackages
}
Write-Host "Using locally built Arcade/Helix SDK version '$version'."

# Bump the msbuild-sdks versions in global.json (targeted replace to preserve formatting).
$globalJsonPath = Join-Path $repoRoot 'global.json'
$globalJson = Get-Content -Path $globalJsonPath -Raw
$globalJson = $globalJson -replace '("Microsoft\.DotNet\.Arcade\.Sdk"\s*:\s*")[^"]*(")', "`${1}$version`${2}"
$globalJson = $globalJson -replace '("Microsoft\.DotNet\.Helix\.Sdk"\s*:\s*")[^"]*(")', "`${1}$version`${2}"
# Verify the replacements actually took effect. If global.json's formatting/keys ever change (or the
# regex stops matching), fail loudly instead of silently proceeding to build with the bootstrap SDK.
foreach ($sdk in @('Microsoft.DotNet.Arcade.Sdk', 'Microsoft.DotNet.Helix.Sdk')) {
  if ($globalJson -notmatch ([regex]::Escape("`"$sdk`"") + '\s*:\s*"' + [regex]::Escape($version) + '"')) {
    throw "Failed to update '$sdk' to '$version' in '$globalJsonPath' (entry not found after replacement)."
  }
}
# Write UTF-8 without a BOM explicitly: Set-Content defaults to UTF-16 in Windows PowerShell 5.1,
# which the .NET SDK JSON reader can choke on.
[System.IO.File]::WriteAllText($globalJsonPath, $globalJson, (New-Object System.Text.UTF8Encoding $false))
Write-Host "Updated Arcade/Helix SDK versions in '$globalJsonPath'."

# Add the local package feed(s) to NuGet.config so the new SDK resolves. Add each distinct directory
# that actually contains *.nupkg (e.g. packages/<config>/Shipping and .../NonShipping) as a flat
# feed - a NuGet folder source pointing at the artifacts root does not reliably resolve the nested
# packages (and the MSBuild SDK resolver needs the SDK package findable at a source root).
$feedDirs = @(Get-ChildItem -Path $PackagesSource -Recurse -Filter '*.nupkg' -ErrorAction SilentlyContinue |
  Select-Object -ExpandProperty DirectoryName -Unique)
if ($feedDirs.Count -eq 0) {
  throw "No *.nupkg found under '$PackagesSource'."
}

$nugetConfigPath = Join-Path $repoRoot 'NuGet.config'
$nugetConfig = New-Object System.Xml.XmlDocument
$nugetConfig.PreserveWhitespace = $true
$nugetConfig.Load($nugetConfigPath)
$packageSources = $nugetConfig.SelectSingleNode("//packageSources")
if ($null -eq $packageSources) {
  throw "'$nugetConfigPath' has no <packageSources> element; cannot add the local feed."
}

# Exact SDK mappings take precedence over the remote feeds' wildcard mappings. Combined with an
# isolated global-packages directory in the validation job, this guarantees both SDKs resolve from
# the downloaded build output instead of a remote feed or a package cached by an earlier job.
$packageSourceMapping = $nugetConfig.SelectSingleNode("//packageSourceMapping")
if ($null -eq $packageSourceMapping) {
  throw "'$nugetConfigPath' has no <packageSourceMapping> element; cannot restrict the built SDKs to local feeds."
}

# Idempotency: remove any 'arcade-local-*' entries a previous run may have added, so re-running in the
# same workspace doesn't create duplicate keys (which NuGet rejects).
foreach ($node in @($packageSources.SelectNodes("add[starts-with(@key,'arcade-local-')]"))) {
  $packageSources.RemoveChild($node) | Out-Null
}
if ($null -ne $packageSourceMapping) {
  foreach ($node in @($packageSourceMapping.SelectNodes("packageSource[starts-with(@key,'arcade-local-')]"))) {
    $packageSourceMapping.RemoveChild($node) | Out-Null
  }
}

$index = 0
foreach ($dir in $feedDirs) {
  $key = "arcade-local-$index"
  Write-Host "Adding local feed '$dir' (key '$key') to '$nugetConfigPath'."
  $newSource = $nugetConfig.CreateElement("add")
  $keyAttribute = $nugetConfig.CreateAttribute("key")
  $keyAttribute.Value = $key
  $valueAttribute = $nugetConfig.CreateAttribute("value")
  $valueAttribute.Value = $dir
  $newSource.Attributes.Append($keyAttribute) | Out-Null
  $newSource.Attributes.Append($valueAttribute) | Out-Null
  $packageSources.AppendChild($newSource) | Out-Null

  $mappingSource = $nugetConfig.CreateElement("packageSource")
  $mappingKey = $nugetConfig.CreateAttribute("key")
  $mappingKey.Value = $key
  $mappingSource.Attributes.Append($mappingKey) | Out-Null

  foreach ($sdkId in $sdkPackages.Keys) {
    $feedContainsSdk = @($sdkPackages[$sdkId] | Where-Object {
      [StringComparer]::OrdinalIgnoreCase.Equals($_.DirectoryName, $dir)
    }).Count -gt 0
    if ($feedContainsSdk) {
      $pkg = $nugetConfig.CreateElement("package")
      $patternAttribute = $nugetConfig.CreateAttribute("pattern")
      $patternAttribute.Value = $sdkId
      $pkg.Attributes.Append($patternAttribute) | Out-Null
      $mappingSource.AppendChild($pkg) | Out-Null
    }
  }

  # Keep other built Microsoft packages eligible from local feeds. The exact SDK mappings above are
  # more specific than every remote microsoft.* pattern, so only the SDKs are forced to local output.
  $pkg = $nugetConfig.CreateElement("package")
  $patternAttribute = $nugetConfig.CreateAttribute("pattern")
  $patternAttribute.Value = 'microsoft.*'
  $pkg.Attributes.Append($patternAttribute) | Out-Null
  $mappingSource.AppendChild($pkg) | Out-Null
  $packageSourceMapping.AppendChild($mappingSource) | Out-Null

  $index++
}
$nugetConfig.Save($nugetConfigPath)

Write-Host "done."
