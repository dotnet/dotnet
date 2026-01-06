[CmdletBinding(PositionalBinding=$false)]
Param(
  [string[]][Alias('p')]$package,
  [string][Alias('c')]$csv,
  [string][Alias('d')]$destination,
  [ValidateSet('ref','text')][string][Alias('t')]$type = 'ref',
  [switch][Alias('x')]$excludeDependencies,
  [switch][Alias('a')]$regenerateAll,
  [string][Alias('f')]$feeds,
  [switch][Alias('h')]$help,
  [Parameter(ValueFromRemainingArguments=$true)][String[]]$properties
)

function Get-Help() {
  Write-Host "usage: $(split-path $MyInvocation.PSCommandPath -Leaf) [options]"
  Write-Host ""
  Write-Host "Generates a reference package or a text-only package from the specified packages and versions. The"
  Write-Host "type of the generated package is controlled via the --type option."
  Write-Host ""
  Write-Host "Reference package generation will restore reference package(s) and dependencies and generate cs files"
  Write-Host "and with accompanying projects into the specified destination ('./src/referencePackages/' by default)."
  Write-Host "Text-only package generation will restore the specified package and copy the source-build-usable content"
  Write-Host "into the provided directory ('./src/textOnlyPackages/' by default)."
  Write-Host ""
  Write-Host "Either -package or -csv must be specified"
  Write-Host ""
  Write-Host "options:"
  Write-Host "  -p|-package <name,version[,tfms]>            A package and version, no spaces, separated by comma. If an optional TFM is"
  Write-Host "                                               specified, it will be used to filter the project's target frameworks."
  Write-Host "                                               Examples: System.Collections,4.3.0"
  Write-Host "                                                         System.Text.Json,4.7.0,netstandard2.0"
  Write-Host "  -c|-csv                                      A path to a csv file of packages to generate. Format is the same as the -package"
  Write-Host "                                               option above, one per line. If specified, the -package option is ignored."
  Write-Host "  -d|-destination                              A path to the root of the repo to copy source into."
  Write-Host "  -t|-type                                     Type of the package to generate. Accepted values: ref (default) | text."
  Write-Host "  -x|-excludeDependencies                      Determines if package dependencies should be excluded. Default is false."
  Write-Host "  -a|-regenerateAll                            Regenerate all packages of the specified type."
  Write-Host "  -f|-feeds                                    A semicolon-separated list of additional NuGet feeds to use during restore."
  Write-Host "  -h|-help                                     Print help and exit."
}

function Initialize-PackageRegeneration {
  Write-Host "Discovering packages for regeneration..."

  $script:tempCsv = [System.IO.Path]::GetTempFileName()
  $packages = @()

  if ($type -eq "ref") {
    $packagesDir = Join-Path $PSScriptRoot "..\src\referencePackages\src"
  } elseif ($type -eq "text") {
    $packagesDir = Join-Path $PSScriptRoot "..\src\textOnlyPackages\src"
  }

  if (Test-Path $packagesDir) {
    $packages = Get-ChildItem $packagesDir -Directory | ForEach-Object {
      $pkg = $_.Name
      Get-ChildItem $_.FullName -Directory | ForEach-Object { "$pkg,$($_.Name)" }
    }
  }

  [System.IO.File]::WriteAllLines($script:tempCsv, $packages)
  if ($packages.Count -eq 0) {
    Write-Error "No packages found to regenerate"
    exit -1
  }

  Write-Host "Found $($packages.Count) package(s) to regenerate"

  $script:arguments += " /p:PackageCSV=`"$script:tempCsv`" /p:ExcludePackageDependencies=true"
}

if ($help -or $($PSBoundParameters.Count) -eq 0) {
  Get-Help
  exit 0
}

foreach ($argument in $PSBoundParameters.Keys)
{
  switch($argument)
  {
    "package"
    { 
        if ($package.Length -lt 2 -or $package.Length -gt 3) {
            Write-Error "Cannot parse -package argument. Format: <name,version[,tfms]>"
            exit -1
        }

        $arguments += " /p:PackageName=$($package[0]) /p:PackageVersion=$($package[1])"
        if ($package.Length -eq 3) {
            $arguments += " /p:PackageTargetFrameworks=`"$($package[2])`""
        }
    }
    "csv"                        { $arguments += " /p:PackageCSV=`"$($PSBoundParameters[$argument])`"" }
    "destination"                { $arguments += " /p:PackagesSrcDirectory=`"$($PSBoundParameters[$argument])`"" }
    "type"                       { $arguments += " /p:PackageType=$($PSBoundParameters[$argument])" }
    "excludeDependencies"        { $arguments += " /p:ExcludePackageDependencies=true" }
    "regenerateAll"              { } # Handled separately below
    "feeds"                      { $arguments += " /p:RestoreAdditionalProjectSources=`"$($PSBoundParameters[$argument])`"" }
    default                      { $arguments += " $($PSBoundParameters[$argument])" }
  }
}

$tempCsv = $null

try {
  if ($regenerateAll) {
    Initialize-PackageRegeneration
  }

  Invoke-Expression "& `"$PSScriptRoot\common\build.ps1`" -restore -build -warnaserror 0 /p:GeneratePackageSource=true $arguments"
}
finally {
  if ($tempCsv) {
    Remove-Item $tempCsv -ErrorAction SilentlyContinue
  }
}

exit 0
