[CmdletBinding(PositionalBinding=$false)]
Param(
  [switch][Alias('h')]$help,
  [switch][Alias('t')]$test,
  [ValidateSet("Debug","Release")][string][Alias('c')]$configuration,
  [string][Alias('v')]$verbosity = "minimal",
  [ValidateSet("windows")][string]$os,
  [ValidateSet("x86","x64","arm","arm64")][string][Alias('a')]$arch,
  [Parameter(ValueFromRemainingArguments=$true)][String[]]$properties
)

function Get-Help() {
  Write-Host "Common settings:"
  Write-Host "  -arch (-a)                     Target platform: x86, x64, arm, arm64, or wasm."
  Write-Host "                                 Pass a comma-separated list to build for multiple architectures."
  Write-Host "                                 [Default: Your machine's architecture.]"
  Write-Host "  -binaryLog (-bl)               Output binary log."
  Write-Host "  -configuration (-c)            Build configuration: Debug or Release."
  Write-Host "                                 Pass a comma-separated list to build for multiple configurations."
  Write-Host "                                 [Default: Debug]"
  Write-Host "  -help (-h)                     Print help and exit."
  Write-Host "  -os                            Target operating system: windows."
  Write-Host "                                 [Default: Your machine's OS.]"
  Write-Host "  -verbosity (-v)                MSBuild verbosity: q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic]."
  Write-Host "                                 [Default: Minimal]"
  Write-Host ""

  Write-Host "Actions (defaults to -restore -build):"
  Write-Host "  -build (-b)             Build all source projects."
  Write-Host "                          This assumes -restore has been run already."
  Write-Host "  -clean                  Clean the solution."
  Write-Host "  -pack                   Package build outputs into NuGet packages."
  Write-Host "  -publish                Publish artifacts (e.g. symbols)."
  Write-Host "                          This assumes -build has been run already."
  Write-Host "  -rebuild                Rebuild all source projects."
  Write-Host "  -restore                Restore dependencies."
  Write-Host "  -sign                   Sign build outputs."
  Write-Host "  -test (-t)              Incrementally builds and runs tests."
  Write-Host ""

  Write-Host "Command-line arguments not listed above are passed through to MSBuild."
  Write-Host "The above arguments can be shortened as much as to be unambiguous."
  Write-Host "(Example: -con for configuration, -t for test, etc.)."
  Write-Host ""
}

if ($help -or (($null -ne $properties) -and ($properties.Contains('/help') -or $properties.Contains('/?')))) {
  Get-Help
  exit 0
}

# Check if an action is passed in
$actions = "b","build","r","restore","rebuild","sign","publish","clean","pack"
$actionPassedIn = @(Compare-Object -ReferenceObject @($PSBoundParameters.Keys) -DifferenceObject $actions -ExcludeDifferent -IncludeEqual).Length -ne 0
if ($null -ne $properties -and $actionPassedIn -ne $true) {
  $actionPassedIn = @(Compare-Object -ReferenceObject $properties -DifferenceObject $actions.ForEach({ "-" + $_ }) -ExcludeDifferent -IncludeEqual).Length -ne 0
}

if (!$actionPassedIn) {
  $arguments = "-restore -build"
}

foreach ($argument in $PSBoundParameters.Keys)
{
  switch($argument)
  {
    "os"                     { $arguments += " /p:TargetOS=$($PSBoundParameters[$argument])" }
    "properties"             { $arguments += " " + $properties }
    "verbosity"              { $arguments += " -$argument " + $($PSBoundParameters[$argument]) }
    "configuration"          { $configuration = (Get-Culture).TextInfo.ToTitleCase($($PSBoundParameters[$argument])); $arguments += " -configuration $configuration" }
    "arch"                   { $arguments += " /p:TargetArchitecture=$($PSBoundParameters[$argument])" }
    default                  { $arguments += " /p:$argument=$($PSBoundParameters[$argument])" }
  }
}

Invoke-Expression "& `"$PSScriptRoot/common/build.ps1`" $arguments"