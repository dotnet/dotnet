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
$arcadePkg = Get-ChildItem -Path $PackagesSource -Recurse -Filter 'Microsoft.DotNet.Arcade.Sdk.*.nupkg' -ErrorAction SilentlyContinue |
  Where-Object { $_.Name -notlike '*.symbols.nupkg' } | Select-Object -First 1
if (-not $arcadePkg) {
  throw "Could not find Microsoft.DotNet.Arcade.Sdk.*.nupkg under '$PackagesSource'."
}
if ($arcadePkg.Name -notmatch '^Microsoft\.DotNet\.Arcade\.Sdk\.(.+)\.nupkg$') {
  throw "Unexpected Arcade SDK package name '$($arcadePkg.Name)'."
}
$version = $Matches[1]
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

# If the config uses packageSourceMapping, a bare <add> source is never consulted for a package
# whose ID is claimed by a more/equally specific pattern on another source. The newly built SDK
# packages ('Microsoft.DotNet.Arcade.Sdk' / 'Microsoft.DotNet.Helix.Sdk') match the remote feeds'
# 'microsoft.*' mapping, so without a matching mapping entry NuGet would never look at our local feed.
# Mirror just the 'microsoft.*' pattern on each local feed so it *ties* with the remotes for
# microsoft.* IDs: the local feed becomes eligible for the new SDK (which only it has) but, because it
# only ties (never exceeds) the remote patterns and is scoped to microsoft.*, it neither blocks
# resolution of microsoft.* packages it lacks nor gets consulted for unrelated (non-microsoft) IDs.
$packageSourceMapping = $nugetConfig.SelectSingleNode("//packageSourceMapping")

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

  if ($null -ne $packageSourceMapping) {
    $mappingSource = $nugetConfig.CreateElement("packageSource")
    $mappingKey = $nugetConfig.CreateAttribute("key")
    $mappingKey.Value = $key
    $mappingSource.Attributes.Append($mappingKey) | Out-Null
    $pkg = $nugetConfig.CreateElement("package")
    $patternAttribute = $nugetConfig.CreateAttribute("pattern")
    $patternAttribute.Value = 'microsoft.*'
    $pkg.Attributes.Append($patternAttribute) | Out-Null
    $mappingSource.AppendChild($pkg) | Out-Null
    $packageSourceMapping.AppendChild($mappingSource) | Out-Null
  }

  $index++
}
$nugetConfig.Save($nugetConfigPath)

Write-Host "done."
