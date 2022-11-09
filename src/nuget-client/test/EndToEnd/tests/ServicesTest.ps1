function Test-PackageManagerServicesAreAvailableThroughMEF {
    # Arrange
    $cm = Get-VsComponentModel

    # Act
    $installer = $cm.GetService([NuGet.VisualStudio.IVsPackageInstaller])
    $installerServices = $cm.GetService([NuGet.VisualStudio.IVsPackageInstallerServices])
    $installerEvents = $cm.GetService([NuGet.VisualStudio.IVsPackageInstallerEvents])

    # Assert
    Assert-NotNull $installer
    Assert-NotNull $installerServices
    Assert-NotNull $installerEvents
}

# If this test fails with the cryptic error:
#
#    Exception calling "NewProject" with "5" argument(s): "The method or operation is not implemented." 
#
# ...it is because the specific Windows 10 SDK build indicated by the <TargetPlatformVersion /> element in the test's
# project file (test\EndToEnd\ProjectTemplates\UwpClassLibraryProjectJson.zip\ClassLibrary6.csproj) is not installed.
function Test-MigrateVanillaUwpProjectJsonToPackageReference {
    param(
        $context
    )

    # Arrange
    $p = New-UwpClassLibraryProjectJson UwpClassLibrary1
    $cm = Get-VsComponentModel
    $projectDir = Get-ProjectDir $p
    $result = [API.Test.InternalAPITestHook]::MigrateJsonProject($p.FullName)
    Start-Sleep -Seconds 3
    # Assert

    # Check if runtimes were migrated correctly
    $expectRuntimeIds = 'win10-arm;win10-arm-aot;win10-x86;win10-x86-aot;win10-x64;win10-x64-aot'
    Assert-True($result.IsSuccess)
    $actualRuntimes = Get-MsBuildPropertyValue $p 'RuntimeIdentifiers'
    Assert-AreEqual $expectRuntimeIds $actualRuntimes

    # Check if project.json file was deleted
    Assert-True !(Test-Path (Join-Path $projectDir project.json))

    # Check if backup was created
    $backupProjectJsonPath = [System.IO.Path]::Combine($projectDir, "Backup", "project.json")
    $backupCsprojPath = [System.IO.Path]::Combine($projectDir, "Backup", "UwpClassLibrary1.csproj")
    Write-Host "Project json backup path: $backupProjectJsonPath"
    Write-Host "Csproj backup path: $backupCsprojPath"
    Assert-True (Test-Path $backupProjectJsonPath)
    Assert-True (Test-Path $backupCsprojPath)

    # Check if package reference was added correctly
    $packageRefs = @(Get-MsBuildItems $p 'PackageReference')
    Assert-AreEqual 1 $packageRefs.Count
    Assert-AreEqual $packageRefs[0].GetMetadataValue("Identity") 'Microsoft.NETCore.UniversalWindowsPlatform'
    Assert-AreEqual $packageRefs[0].GetMetadataValue("Version") '5.2.2'
}

function Test-VsPackageInstallerServices {
    param(
        $context
    )

    # Arrange
    $p = New-WebApplication
    $cm = Get-VsComponentModel
    $installerServices = $cm.GetService([NuGet.VisualStudio.IVsPackageInstallerServices])

    # Act
    $p | Install-Package jquery -Version 1.5 -Source $context.RepositoryPath
    $packages = @($installerServices.GetInstalledPackages())
    $package = $packages |  where Id -eq jquery

    # Assert
    Assert-NotNull $packages
    Assert-AreEqual 1 $packages.Count
    Assert-AreEqual jQuery $packages[0].Id
    Assert-NotNull $package.InstallPath
}

function Test-GetInstalledPackagesWithCustomRestorePackagesPath {
    param(
        $context
    )

    # Arrange
    $p = New-NetCoreConsoleAppWithCustomRestorePackagesPath ConsoleApp
    $cm = Get-VsComponentModel
    $installerServices = $cm.GetService([NuGet.VisualStudio.IVsPackageInstallerServices])
    Assert-NetCoreProjectCreation $p

    # Act
    $packages = @($installerServices.GetInstalledPackages())

    # Assert    
    Assert-NotNull $packages
    $package = $packages | where Id -eq NuGet.Versioning
    Assert-NotNull $package.InstallPath
    $packagesPath = Get-MsBuildPropertyValue $p 'RestorePackagesPath'
    Assert-NotNull $packagesPath
    $expectedInstallPath = (Join-Path $packagesPath (Join-Path $package.Id $package.Version))
    Assert-AreEqual $expectedInstallPath $package.InstallPath
}

function Test-GetInstalledPackagesForProjectWithCustomRestorePackagesPath {
    param(
        $context
    )

    # Arrange
    $p = New-NetCoreConsoleAppWithCustomRestorePackagesPath ConsoleApp
    $cm = Get-VsComponentModel
    $installerServices = $cm.GetService([NuGet.VisualStudio.IVsPackageInstallerServices])
    Assert-NetCoreProjectCreation $p

    # Act
    $packages = @($installerServices.GetInstalledPackages($p))

    # Assert    
    Assert-NotNull $packages
    $package = $packages | where Id -eq NuGet.Versioning
    Assert-NotNull $package.InstallPath
    $packagesPath = Get-MsBuildPropertyValue $p 'RestorePackagesPath'
    Assert-NotNull $packagesPath
    $expectedInstallPath = (Join-Path $packagesPath (Join-Path $package.Id $package.Version))
    Assert-AreEqual $expectedInstallPath $package.InstallPath
}

function Test-GetInstalledPackagesMultipleProjectsSameVersion {
    param($context)

    # Arrange
    $p = New-WebApplication
    $p | Install-Package jquery -Version 1.5 -Source $context.RepositoryPath
    $p = New-WebApplication
    $p | Install-Package jquery -Version 1.5 -Source $context.RepositoryPath

    $cm = Get-VsComponentModel
    $installerServices = $cm.GetService([NuGet.VisualStudio.IVsPackageInstallerServices])

    # Act
    $packages = @($installerServices.GetInstalledPackages())

    # Assert
    Assert-NotNull $packages
    Assert-AreEqual 1 $packages.Count
    Assert-AreEqual jQuery $packages[0].Id
    Assert-AreEqual 1.5 $packages[0].VersionString
}

function Test-GetInstalledPackagesMultipleProjectsDifferentVersion {
    param($context)

    # Arrange
    $p = New-WebApplication
    $p | Install-Package jquery -Version 1.5 -Source $context.RepositoryPath
    $p = New-WebApplication
    $p | Install-Package jquery -Version 1.6 -Source $context.RepositoryPath

    $cm = Get-VsComponentModel
    $installerServices = $cm.GetService([NuGet.VisualStudio.IVsPackageInstallerServices])

    # Act
    $packages = @($installerServices.GetInstalledPackages())

    # Assert
    Assert-NotNull $packages
    Assert-AreEqual 2 $packages.Count
    Assert-AreEqual jQuery $packages[0].Id
    Assert-AreEqual 1.5 $packages[0].VersionString
    Assert-AreEqual jQuery $packages[1].Id
    Assert-AreEqual 1.6 $packages[1].VersionString
}
<#
function Test-GetInstalledPackagesMVCTemplate
{
    param($context)

    # Arrange
    $p = New-MvcWebSite

    $cm = Get-VsComponentModel
    $installerServices = $cm.GetService([NuGet.VisualStudio.IVsPackageInstallerServices])

    # Act
    $packages = @($installerServices.GetInstalledPackages())

    # Assert
    Write-Host $packages

    Assert-AreEqual 30 $packages.Count
    Assert-AreEqual Microsoft.Web.Infrastructure $packages[0].Id
    Assert-AreEqual 1.0.0.0 $packages[0].VersionString
    Assert-AreEqual jQuery $packages[1].Id
    Assert-AreEqual 1.10.2 $packages[1].VersionString
    Assert-AreEqual Respond $packages[2].Id
    Assert-AreEqual 1.2.0 $packages[2].VersionString
    Assert-AreEqual Modernizr $packages[3].Id
    Assert-AreEqual 2.6.2 $packages[3].VersionString
    Assert-AreEqual bootstrap $packages[4].Id
    Assert-AreEqual 3.0.0 $packages[4].VersionString
    Assert-AreEqual WebGrease $packages[5].Id
    Assert-AreEqual 1.5.2 $packages[5].VersionString
    Assert-AreEqual Antlr $packages[6].Id
    Assert-AreEqual 3.4.1.9004 $packages[6].VersionString
    Assert-AreEqual Newtonsoft.Json $packages[7].Id
    Assert-AreEqual 6.0.4 $packages[7].VersionString
    Assert-AreEqual Microsoft.AspNet.Web.Optimization $packages[8].Id
    Assert-AreEqual 1.1.3 $packages[8].VersionString
    Assert-AreEqual AspNet.ScriptManager.bootstrap $packages[9].Id
    Assert-AreEqual 3.0.0 $packages[9].VersionString
    Assert-AreEqual AspNet.ScriptManager.jQuery $packages[10].Id
    Assert-AreEqual 1.10.2 $packages[10].VersionString
    Assert-AreEqual Microsoft.AspNet.ScriptManager.MSAjax $packages[11].Id
    Assert-AreEqual 5.0.0 $packages[11].VersionString
    Assert-AreEqual Microsoft.AspNet.ScriptManager.WebForms $packages[12].Id
    Assert-AreEqual 5.0.0 $packages[12].VersionString
    Assert-AreEqual Microsoft.AspNet.Web.Optimization.WebForms $packages[13].Id
    Assert-AreEqual 1.1.3 $packages[13].VersionString
    Assert-AreEqual Microsoft.AspNet.FriendlyUrls.Core $packages[14].Id
    Assert-AreEqual 1.0.2 $packages[14].VersionString
    Assert-AreEqual EntityFramework $packages[15].Id
    Assert-AreEqual 6.1.2 $packages[15].VersionString
    Assert-AreEqual Microsoft.AspNet.Identity.Core $packages[16].Id
    Assert-AreEqual 2.2.0 $packages[16].VersionString
    Assert-AreEqual Microsoft.AspNet.Identity.EntityFramework $packages[17].Id
    Assert-AreEqual 2.2.0 $packages[17].VersionString
    Assert-AreEqual Microsoft.AspNet.Identity.Owin $packages[18].Id
    Assert-AreEqual 2.2.0 $packages[18].VersionString
    Assert-AreEqual Microsoft.Owin $packages[19].Id
    Assert-AreEqual 3.0.1 $packages[19].VersionString
    Assert-AreEqual Microsoft.Owin.Host.SystemWeb $packages[20].Id
    Assert-AreEqual 3.0.1 $packages[20].VersionString
    Assert-AreEqual Microsoft.Owin.Security $packages[21].Id
    Assert-AreEqual 3.0.1 $packages[21].VersionString
    Assert-AreEqual Microsoft.Owin.Security.Facebook $packages[22].Id
    Assert-AreEqual 3.0.1 $packages[22].VersionString
    Assert-AreEqual Microsoft.Owin.Security.Cookies $packages[23].Id
    Assert-AreEqual 3.0.1 $packages[23].VersionString
    Assert-AreEqual Microsoft.Owin.Security.Google $packages[24].Id
    Assert-AreEqual 3.0.1 $packages[24].VersionString
    Assert-AreEqual Microsoft.Owin.Security.Twitter $packages[25].Id
    Assert-AreEqual 3.0.1 $packages[25].VersionString
    Assert-AreEqual Microsoft.Owin.Security.MicrosoftAccount $packages[26].Id
    Assert-AreEqual 3.0.1 $packages[26].VersionString
    Assert-AreEqual Owin $packages[27].Id
    Assert-AreEqual 1.0 $packages[27].VersionString
    Assert-AreEqual Microsoft.AspNet.Providers.Core $packages[28].Id
    Assert-AreEqual 2.0.0 $packages[28].VersionString
    Assert-AreEqual Microsoft.Owin.Security.OAuth $packages[29].Id
    Assert-AreEqual 3.0.1 $packages[29].VersionString
}
#>

# Disable the test in NuGet V3, as the underlying threading has changed.
# Now VsPackageInstallerEvent and PackageManager is doing work using the worker thread, which does not have a PowerShell runspace associated with it.
# And runspace cannot be shared by the threads.
function VsPackageInstallerEvents {
    param(
        $context
    )

    try {
        # Arrange
        $p = New-WebApplication
        $cm = Get-VsComponentModel
        $installerEvents = $cm.GetService([NuGet.VisualStudio.IVsPackageInstallerEvents])

        $global:installing = 0
        $global:installed = 0
        $global:uninstalling = 0
        $global:uninstalled = 0

        $installingHandler = {
            $global:installing++
        }

        $installerEvents.add_PackageInstalling($installingHandler)

        $installedHandler = {
            $global:installed++
        }

        $installerEvents.add_PackageInstalled($installedHandler)

        $uninstallingHandler = {
            $global:uninstalling++
        }

        $installerEvents.add_PackageUninstalling($uninstallingHandler)

        $uninstalledHandler = {
            $global:uninstalled++
        }

        $installerEvents.add_PackageUninstalled($uninstalledHandler)

        # Act
        $p | Install-Package jquery -Version 1.5 -Source $context.RepositoryPath
        $p | Uninstall-Package jquery


        # Assert
        Assert-AreEqual 1 $global:installing
        Assert-AreEqual 1 $global:installed
        Assert-AreEqual 1 $global:uninstalling
        Assert-AreEqual 1 $global:uninstalled
    }
    finally {
        $installerEvents.remove_PackageInstalling($installingHandler)
        $installerEvents.remove_PackageInstalled($installedHandler)
        $installerEvents.remove_PackageUninstalling($uninstallingHandler)
        $installerEvents.remove_PackageUninstalled($uninstalledHandler)

        Remove-Variable "installing"   -scope global
        Remove-Variable "installed"    -scope global
        Remove-Variable "uninstalling" -scope global
        Remove-Variable "uninstalled"  -scope global
    }
}

function Test-InstallLatestStablePackageAPI
{
    param($context)

    # Arrange
    $p = New-ClassLibrary

    # Act
    [API.Test.InternalAPITestHook]::InstallLatestPackageApi("TestPackage.ListedStable", $false)

    # Assert
    Assert-Package $p TestPackage.ListedStable 2.0.6
}

function Test-InstallLatestStablePackageAPIForOnlyPrerelease
{
    param($context)

    # Arrange
    $p = New-ClassLibrary

    # Act & Assert
    Assert-Throws { [API.Test.InternalAPITestHook]::InstallLatestPackageApi("TestPackage.AlwaysPrerelease", $false)  } "Exception calling `"InstallLatestPackageApi`" with `"2`" argument(s): `"No latest version found for 'TestPackage.AlwaysPrerelease' for the given source repositories and resolution context`""
    Assert-NoPackage $p TestPackage.AlwaysPrerelease
}

function Test-InstallLatestPrereleasePackageAPI
{
    param($context)

    # Arrange
    $p = New-ClassLibrary

    # Act
    [API.Test.InternalAPITestHook]::InstallLatestPackageApi("TestPackage.AlwaysPrerelease", $true)

    # Assert
    Assert-Package $p TestPackage.AlwaysPrerelease 5.0.0-beta
}

function Test-InstallPackageAPI
{
    param($context)

    # Arrange
    $p = New-ClassLibrary

    # Act
    [API.Test.InternalAPITestHook]::InstallPackageApi("owin","1.0.0")

    # Assert
    Assert-Package $p owin 1.0.0
}

function Test-InstallPackageAPIEmptyVersion
{
    param($context)

    # Arrange
    $p = New-ClassLibrary

    # Act
    [API.Test.InternalAPITestHook]::InstallPackageApi("owin","")

    # Assert
    Assert-Package $p owin 1.0.0
}

function Test-InstallPackageAPIAllSource
{
    param($context)

    # Arrange
    $p = New-ClassLibrary

    # Act
    [API.Test.InternalAPITestHook]::InstallPackageApi("All", "owin", "1.0.0", $false)

    # Assert
    Assert-Package $p owin 1.0.0
}

function Test-InstallPackageAPIInvalidSource
{
    param($context)

    # Arrange
    $p = New-ClassLibrary

    # Act&Assert
    Assert-Throws { [API.Test.InternalAPITestHook]::InstallPackageApi("invalid", "owin", "1.0.0", $false) } "Exception calling `"InstallPackageApi`" with `"4`" argument(s): `"The specified source 'invalid' is invalid. Please provide a valid source.`r`nParameter name: source`""
    Assert-NoPackage $p "owin"
}

function Test-InstallPackageAPIUnreachableSource
{
    param($context)

    # Arrange
    $p = New-ClassLibrary

    # Act&Assert
    Assert-Throws {
        [API.Test.InternalAPITestHook]::InstallPackageApi("http://packagesource", "owin", "1.0.0", $false)
    } "Exception calling `"InstallPackageApi`" with `"4`" argument(s): `"Unable to load the service index for source http://packagesource.`""
    Assert-NoPackage $p "owin"
}

function InstallPackageAPI_FWLinkSource
{
    param($context)

    # Arrange
    $p = New-BuildIntegratedProj UAPApp

    # Act
    [API.Test.InternalAPITestHook]::InstallPackageApi("https://go.microsoft.com/fwlink/?LinkId=699556", "Microsoft.Xaml.Behaviors.Uwp.Managed", [NuGet.Versioning.NuGetVersion]$null, $true)

    # Assert
    Assert-ProjectJsonDependency $p Microsoft.Xaml.Behaviors.Uwp.Managed 1.0.0
}

function Test-UninstallPackageAPI
{
    param($context)

    # Arrange
    $p = New-ClassLibrary

    # Act
    [API.Test.InternalAPITestHook]::InstallPackageApi("microsoft.owin","4.2.2")
    [API.Test.InternalAPITestHook]::UninstallPackageApi("microsoft.owin","true")

    # Assert
    Assert-NoPackage $p owin 1.0.0
    Assert-NoPackage $p microsoft.owin 4.2.2
}

function Test-UninstallPackageAPINoDep
{
    param($context)

    # Arrange
    $p = New-ClassLibrary

    # Act
    [API.Test.InternalAPITestHook]::InstallPackageApi("microsoft.owin","4.2.2")
    [API.Test.InternalAPITestHook]::UninstallPackageApi("microsoft.owin",0)

    # Assert
    Assert-Package $p owin 1.0.0
    Assert-NoPackage $p microsoft.owin 4.2.2
}

function Test-UninstallPackageAPINoForce
{
    param($context)

    # Arrange
    $p = New-ClassLibrary

    # Act
    [API.Test.InternalAPITestHook]::InstallPackageApi("microsoft.owin","4.2.2")
    Assert-Throws { [API.Test.InternalAPITestHook]::UninstallPackageApi("owin","true") } "Exception calling `"UninstallPackageApi`" with `"2`" argument(s): `"Unable to uninstall 'Owin.1.0.0' because 'Microsoft.Owin.4.2.2' depends on it.`""
}

function Test-GetSourceAPI
{
    # Arrange
    $cm = Get-VsComponentModel
    $sourceProvider = $cm.GetService([NuGet.VisualStudio.IVsPackageSourceProvider])

    # Act
    $sources = $sourceProvider.GetSources("true","false")

    # Assert
    Assert-NotNull $sources
}

function Test-CompareSemanticVersions
{
    # Arrange
    $cm = Get-VsComponentModel
    $service = $cm.GetService([NuGet.VisualStudio.IVsSemanticVersionComparer])
    $versionA = "3.1.0-beta-001"
    $versionB = "2.9.0.0"

    # Act
    $actual = $service.Compare($versionA, $versionB)

    # Assert
    Assert-True ($actual > 0) "$actual should be greater than zero."
}

function Test-ParseFrameworkName
{
    # Arrange
    $cm = Get-VsComponentModel
    $service = $cm.GetService([NuGet.VisualStudio.IVsFrameworkParser])
    $framework = "net45"

    # Act
    $actual = $service.ParseFrameworkName($framework)

    # Assert
    Assert-AreEqual ".NETFramework,Version=v4.5" $actual.ToString()
}

function Test-GetShortFolderName
{
    # Arrange
    $cm = Get-VsComponentModel
    $service = $cm.GetService([NuGet.VisualStudio.IVsFrameworkParser])
    $framework = [System.Runtime.Versioning.FrameworkName](".NETStandard,Version=v1.3")

    # Act
    $actual = $service.GetShortFrameworkName($framework)

    # Assert
    Assert-AreEqual "netstandard1.3" $actual.ToString()
}

function Test-GetNearest
{
    # Arrange
    $cm = Get-VsComponentModel
    $service = $cm.GetService([NuGet.VisualStudio.IVsFrameworkCompatibility])
    $target = [System.Runtime.Versioning.FrameworkName](".NETFramework,Version=v4.5.1")
    [System.Runtime.Versioning.FrameworkName[]] $frameworks = @(
        [System.Runtime.Versioning.FrameworkName](".NETFramework,Version=v3.5"),
        [System.Runtime.Versioning.FrameworkName](".NETFramework,Version=v4.0"),
        [System.Runtime.Versioning.FrameworkName](".NETFramework,Version=v4.5"),
        [System.Runtime.Versioning.FrameworkName](".NETFramework,Version=v4.5.2")
    )

    # Act
    $actual = $service.GetNearest($target, $frameworks)

    # Assert
    Assert-AreEqual ".NETFramework,Version=v4.5" $actual.ToString()
}

function Test-GetNetStandardVersions
{
    # Arrange
    $cm = Get-VsComponentModel
    $service = $cm.GetService([NuGet.VisualStudio.IVsFrameworkCompatibility])

    # Act
    $actual = $service.GetNetStandardFrameworks()

    # Assert
    Assert-AreEqual ".NETStandard,Version=v1.0" ($actual | Select-Object -Index 0)
    Assert-AreEqual ".NETStandard,Version=v1.1" ($actual | Select-Object -Index 1)
    Assert-AreEqual ".NETStandard,Version=v1.2" ($actual | Select-Object -Index 2)
    Assert-AreEqual ".NETStandard,Version=v1.3" ($actual | Select-Object -Index 3)
    Assert-AreEqual ".NETStandard,Version=v1.4" ($actual | Select-Object -Index 4)
    Assert-AreEqual ".NETStandard,Version=v1.5" ($actual | Select-Object -Index 5)
    Assert-AreEqual ".NETStandard,Version=v1.6" ($actual | Select-Object -Index 6)
    Assert-AreEqual ".NETStandard,Version=v1.7" ($actual | Select-Object -Index 7)
    Assert-AreEqual ".NETStandard,Version=v2.0" ($actual | Select-Object -Index 8)
    Assert-AreEqual ".NETStandard,Version=v2.1" ($actual | Select-Object -Index 9)
}

function Test-GetFrameworksSupportingNetStandard
{
    # Arrange
    $cm = Get-VsComponentModel
    $service = $cm.GetService([NuGet.VisualStudio.IVsFrameworkCompatibility])

    # Act
    $actual = $service.GetFrameworksSupportingNetStandard(".NETStandard,Version=v1.2")

    # Assert
    Assert-AreEqual ".NETCore,Version=v5.0" ($actual | Select-Object -Index 0)
    Assert-AreEqual ".NETCoreApp,Version=v1.0" ($actual | Select-Object -Index 1)
    Assert-AreEqual ".NETFramework,Version=v4.5.1" ($actual | Select-Object -Index 2)
    Assert-AreEqual ".NETPortable,Version=v0.0,Profile=Profile151" ($actual | Select-Object -Index 3)
}

function Test-RestorePackageAPI
{
    param($context)

    # Arrange
    $p = New-ClassLibrary
    $p | Install-Package JQuery

    # delete the packages folder
    $packagesDir = Get-PackagesDir
    Remove-Item -Recurse -Force $packagesDir
    Assert-False (Test-Path $packagesDir)

    # Act
    [API.Test.InternalAPITestHook]::RestorePackageApi()

    # Assert
    Assert-True (Test-Path $packagesDir)
    Assert-Package $p JQuery
}

function Test-InstallPackageAPIPackageNotExist
{
    param($context)

    # Arrange
    $p = New-ClassLibrary

    # Act & Assert
    Assert-Throws { [API.Test.InternalAPITestHook]::InstallPackageApi("NotExistPackage","") } "Exception calling `"InstallPackageApi`" with `"2`" argument(s): `"No latest version found for 'NotExistPackage' for the given source repositories and resolution context`""
}

function Test-InstallPackageAPIInstalledPackage
{
    param($context)

    # Arrange
    $p = New-ClassLibrary

    # Act
    [API.Test.InternalAPITestHook]::InstallPackageApi("owin","1.0.0")
    [API.Test.InternalAPITestHook]::InstallPackageApi("owin","1.0.0")

    # Assert
    Assert-Package $p owin 1.0.0
}

function Test-InstallPackageAPIInstalledLowerVersionPackage
{
    param($context)

    # Arrange
    $p = New-ClassLibrary

    # Act
    [API.Test.InternalAPITestHook]::InstallPackageApi("nuget.librarymodel","6.2.0")
    [API.Test.InternalAPITestHook]::InstallPackageApi("nuget.librarymodel","6.3.0")

    # Assert
    Assert-Package $p nuget.librarymodel 6.3.0
    Assert-NoPackage $p nuget.librarymodel 6.2.0
}

function Test-InstallPackageAPIInstalledHigherVersionPackage
{
    param($context)

    # Arrange
    $p = New-ClassLibrary

    # Act
    [API.Test.InternalAPITestHook]::InstallPackageApi("nuget.librarymodel","6.3.0")
    [API.Test.InternalAPITestHook]::InstallPackageApi("nuget.librarymodel","6.2.0")

    # Assert
    Assert-Package $p nuget.librarymodel 6.2.0
    Assert-NoPackage $p nuget.librarymodel 6.3.0
}

function Test-UninstallPackageAPIPackageNotExist
{
    param($context)

    # Arrange
    $p = New-ClassLibrary
    $project = Get-Project
    $projectName = $project.ProjectName

    # Act & Assert
    Assert-Throws {[API.Test.InternalAPITestHook]::UninstallPackageApi("owin","true") } "Exception calling `"UninstallPackageApi`" with `"2`" argument(s): `"Package 'owin' to be uninstalled could not be found in project '$projectName'`""
}

function Test-RestorePackageAPINoPackage
{
    param($context)

    # Arrange
    $p = New-ClassLibrary

    # Act
    [API.Test.InternalAPITestHook]::RestorePackageApi()

    # Assert
    Assert-False (Join-Path (Get-ProjectDir $p) packages.config)
}

function Test-InstallPackageAPIBindingRedirect
{
    param($context)

    # Arrange
    $p = New-ClassLibrary

    # Act
    [API.Test.InternalAPITestHook]::InstallPackageApi("TestBindingRedirectA","1.0.0")

    # Assert
    Assert-BindingRedirect $p app.config B '0.0.0.0-2.0.0.0' '2.0.0.0'
}

function Test-ExecuteInitPS1OnClassLibrary
{
    param($context)

    # Arrange
    $global:PackageInitPS1Var = 0
    $p = New-ClassLibrary

    Install-Package PackageInitPS1 -Project $p.Name -Source $context.RepositoryRoot

    Assert-True ($global:PackageInitPS1Var -eq 1)

    # Act
    $result = [API.Test.InternalAPITestHook]::ExecuteInitScript("PackageInitPS1","1.0.2")

    Assert-True $result

    Assert-True ($global:PackageInitPS1Var -eq 1)
}

function Test-ExecuteInitPS1OnUAP
{
    param($context)

    # Arrange
    $global:PackageInitPS1Var = 0
    $p = New-BuildIntegratedProj UAPApp

    Install-Package PackageInitPS1 -Project $p.Name -Source $context.RepositoryRoot

    Assert-True ($global:PackageInitPS1Var -eq 1)

    # Act
    $result = [API.Test.InternalAPITestHook]::ExecuteInitScript("PackageInitPS1","1.0.2")

    Assert-True $result

    Assert-True ($global:PackageInitPS1Var -eq 1)
}

# NOTE: The following test does not work since ExecuteInitScript needs the powershell pipeline to be free
#       for it execute scripts. But, under Run-Test, the pipeline is already busy.
function ExecuteInitPS1OnAspNetCore
{
    param($context)

    # Set DNX packages folder to be NUGET global packages folder
    $env:DNX_PACKAGES = "$env:USERPROFILE\.nuget\packages"

    # Arrange
    $global:PackageInitPS1Var = 0
    $p = New-DNXClassLibrary

    Install-Package PackageInitPS1 -Project $p.Name -Source $context.RepositoryRoot

    Assert-True ($global:PackageInitPS1Var -eq 0)

    # Act
    $result = [API.Test.InternalAPITestHook]::ExecuteInitScript("PackageInitPS1","1.0.2")

    Assert-True $result

    Assert-True ($global:PackageInitPS1Var -eq 1)
}

function Test-BatchEventsApi
{
    param($context)

    # Arrange
    $p = New-ClassLibrary

    # Act
    $result = [API.Test.InternalAPITestHook]::BatchEventsApi("owin","1.0.0")

    # Assert
    Assert-True $result
}

function Test-ExecuteInitScriptsPerSolution
{
    param($context)

    # Arrange
    $global:PackageInitPS1Var = 0
    $p = New-ClassLibrary

    Install-Package PackageInitPS1 -Project $p.Name -Source $context.RepositoryRoot

    Assert-True ($global:PackageInitPS1Var -eq 1)

    $solutionFile1 = Get-SolutionFullName
    SaveAs-Solution($solutionFile1)
	Close-Solution

    $p = New-ClassLibrary
    $p | Install-Package jquery -Version 1.9

    $solutionFile2 = Get-SolutionFullName
    SaveAs-Solution($solutionFile2)
	Close-Solution

    # Act
    Open-Solution $solutionFile1
	$p = Get-Project
    $p | Install-Package jquery -Version 1.9

    # Assert
    Assert-True ($global:PackageInitPS1Var -eq 1)
}

function Test-ExecuteInitScriptsOnlyOnce
{
    param($context)

    # Arrange
    $global:PackageInitPS1Var = 0
    $p = New-ClassLibrary

    Install-Package PackageInitPS1 -Project $p.Name -Source $context.RepositoryRoot

    Assert-True ($global:PackageInitPS1Var -eq 1)

    # Act
    $p | Install-Package jquery -Version 1.9

    # Assert
    Assert-True ($global:PackageInitPS1Var -eq 1)
}

function Test-CreateVsPathContextWithConfiguration {
    param($context)

	# Arrange
	$p = New-ClassLibrary

	$solutionFile = Get-SolutionFullName
	$solutionDir = Split-Path $solutionFile -Parent

	$userPackageFolder = Join-Path $solutionDir "userPackageFolder"
	$fallbackPackageFolderA = Join-Path $solutionDir "fallbackPackageFolderA"
	$fallbackPackageFolderB = Join-Path $solutionDir "fallbackPackageFolderB"

	$settingFile = Join-Path $solutionDir "nuget.config"
	$settingFileContent =@"
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <config>
    <add key="globalPackagesFolder" value="{0}" />
  </config>
  <fallbackPackageFolders>
    <clear />
    <add key="a" value="{1}" />
    <add key="b" value="{2}" />
  </fallbackPackageFolders>
</configuration>
"@

	$settingFileContent -f $userPackageFolder, $fallbackPackageFolderA, $fallbackPackageFolderB `
	    | Out-File -Encoding "UTF8" $settingFile

	SaveAs-Solution($solutionFile)
	Close-Solution
	Open-Solution $solutionFile

	$p = Get-Project

	# Act
	$context = [API.Test.InternalAPITestHook]::GetVsPathContext($p.FullName)

	# Assert
	Assert-AreEqual $userPackageFolder $context.UserPackageFolder
	Assert-AreEqual 2 $context.FallbackPackageFolders.Count
	Assert-AreEqual $fallbackPackageFolderA $context.FallbackPackageFolders[0]
	Assert-AreEqual $fallbackPackageFolderB $context.FallbackPackageFolders[1]
}

function Test-CreateVsPathContextWithoutConfiguration {
    param($context)

	# Arrange
	$p = New-ClassLibrary

	# Act
	$context = [API.Test.InternalAPITestHook]::GetVsPathContext($p.FullName)

	# Assert
	Assert-NotNull $context.UserPackageFolder
}

function Test-CreateVsPathContextUsesAssetsFileIfAvailable {
    param($context)

	# Arrange
	$p = New-BuildIntegratedProj UAPApp

    Install-Package NuGet.Versioning -ProjectName $p.Name -version 1.0.7

	$solutionFile = Get-SolutionFullName
	$solutionDir = Split-Path $solutionFile -Parent

	$userPackageFolder = Join-Path $solutionDir "userPackageFolder"

	$settingFile = Join-Path $solutionDir "nuget.config"
	$settingFileContent =@"
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <config>
    <add key="globalPackagesFolder" value="{0}" />
  </config>
</configuration>
"@

	$settingFileContent -f $userPackageFolder | Out-File -Encoding "UTF8" $settingFile

	SaveAs-Solution($solutionFile)
	Close-Solution
	Open-Solution $solutionFile

	$p = Get-Project

	# Act
	$context = [API.Test.InternalAPITestHook]::GetVsPathContext($p.FullName)

	# Assert
	Assert-NotNull $context.UserPackageFolder
	Assert-NotEqual $userPackageFolder $context.UserPackageFolder
}

function Test-InstallPackageAPIToLatestVersion
{
    param($context)

    # Arrange
    $p = New-ClassLibrary
    Install-Package TestPackage.ListedStable -ProjectName $p.Name -version 1.0.0
    Assert-Package $p TestPackage.ListedStable 1.0.0

    # Act
    [API.Test.InternalAPITestHook]::InstallPackageApi("TestPackage.ListedStable","")

    # Assert
    Assert-Package $p TestPackage.ListedStable 2.0.6
}

function Test-InstallPackageAsyncWithPackageReferenceFormat {
    param($context)

	# Arrange
	$p = New-ClassLibrary

	$solutionFile = Get-SolutionFullName
	$solutionDir = Split-Path $solutionFile -Parent

	$userPackageFolder = Join-Path $solutionDir "userPackageFolder"

	$settingFile = Join-Path $solutionDir "nuget.config"
	$settingFileContent =@"
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <config>
    <add key="globalPackagesFolder" value="{0}" />
  </config>
  <packageManagement>
    <add key="format" value="1" />
    <add key="disabled" value="True" />
  </packageManagement>
</configuration>
"@

	$settingFileContent -f $userPackageFolder `
	    | Out-File -Encoding "UTF8" $settingFile

	SaveAs-Solution($solutionFile)
	Close-Solution
	Open-Solution $solutionFile

	$p = Get-Project

	# Act
	$context = [API.Test.InternalAPITestHook]::InstallPackageApi("owin","1.0.0")
    $p.Save($p.FullName)

	# Assert
    $packageRefs = @(Get-MsBuildItems $p 'PackageReference')
    Assert-AreEqual 1 $packageRefs.Count
    Assert-AreEqual $packageRefs[0].GetMetadataValue("Identity") 'owin' 
    Assert-AreEqual $packageRefs[0].GetMetadataValue("Version") '1.0.0'
}
