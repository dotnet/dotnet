# Ensure all packages are created before publishing

[CmdletBinding(SupportsShouldProcess=$True)]
Param (
    [string]$NupkgOutputPath,
    [switch]$BuildRTM
)

# The list of projects need to be packed.
[System.Collections.ArrayList]$PackageIDListShouldExist = @(
"NuGet.CommandLine",
"NuGet.Indexing",
"NuGet.VisualStudio.Contracts",
"NuGet.VisualStudio",
"Microsoft.Build.NuGetSdkResolver",
"NuGet.Build.Tasks.Console",
"NuGet.Build.Tasks.Pack",
"NuGet.Build.Tasks",
"NuGet.CommandLine.XPlat",
"NuGet.Commands",
"NuGet.Common",
"NuGet.Configuration",
"NuGet.Credentials",
"NuGet.DependencyResolver.Core",
"NuGet.Frameworks",
"NuGet.LibraryModel",
"NuGet.Localization",
"NuGet.PackageManagement",
"NuGet.Packaging.Core",
"NuGet.Packaging",
"NuGet.ProjectModel",
"NuGet.Protocol",
"NuGet.Resolver",
"NuGet.Versioning")

if (!$BuildRTM)
{
    $PackageIDListShouldExist.Add("Test.Utility")
    $PackageIDListShouldExist.Add("NuGet.Localization")
}

$ErrorMessage = ""
$MissingNupkgs = ""
$MissingSymbols = ""
$MissingNupkgCount = 0
$MissingSymbolsCount = 0

ForEach ($PackageIDShouldExist in $PackageIDListShouldExist)
{
    $PackageIDShouldExist = $PackageIDShouldExist.trim()

    $PackagesName = Get-ChildItem $NupkgOutputPath -Filter *.nupkg -Name

    $FoundNupkg = $false

    $FoundSymbols = $false

    Foreach ($PackageName in $PackagesName)
    {
        $FoundNupkg = ( ($PackageName -match "${PackageIDShouldExist}.[0-9][0-9a-zA-Z.-]*.nupkg") -and -not($PackageName -match "${ProjectShouldPack}.[0-9][0-9a-zA-Z.-]*.symbols.nupkg") )

        if ($FoundNupkg)
        {
            Write-Host "$PackageName is found." -ForegroundColor Cyan
            break
        }
    }

    Foreach ($PackageName in $PackagesName)
    {
        $FoundSymbols = $PackageName -match "${PackageIDShouldExist}.[0-9][0-9a-zA-Z.-]*.symbols.nupkg"

        if ($FoundSymbols)
        {
            Write-Host "$PackageName is found." -ForegroundColor Cyan
            break
        }
    }

    if (-not($FoundNupkg))
    {
        $MissingNupkg = $MissingNupkg + "$PackageIDShouldExist "
        $MissingNupkgCount = $MissingNupkgCount + 1
    }
     if (-not($FoundSymbols))
    {
        $MissingSymbols = $MissingSymbols + "$PackageIDShouldExist "
        $MissingSymbolsCount = $MissingSymbolsCount + 1
    }
}

if (($MissingNupkgCount -ne 0) -OR ($MissingSymbolsCount -ne 0))
{
    $ErrorMessage = "$MissingNupkgCount nupkgs and $MissingSymbolsCount symbols are missing in $NupkgOutputPath `n"
    if ($MissingNupkgCount -ne 0)
    {
        $ErrorMessage = $ErrorMessage + "Projects missing nupkgs: $MissingNupkg `n"
    }
    if ($MissingSymbolsCount -ne 0)
    {
        $ErrorMessage = $ErrorMessage + "Projects missing Symbols: $MissingSymbols `n"
    }

    Write-Error "[FATAL] $ErrorMessage"
    Exit 1
}
else
{
    Write-Host "All projects are packed in : $NupkgOutputPath" -ForegroundColor Cyan
    Exit 0
}
