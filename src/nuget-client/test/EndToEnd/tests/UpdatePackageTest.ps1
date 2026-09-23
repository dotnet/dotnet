function Test-UpdatingPackageWithPackageSaveModeNuspec {
    # Arrange
	Check-NuGetConfig

    $componentModel = Get-VSComponentModel
	$setting = $componentModel.GetService([NuGet.Configuration.ISettings])

    try {
        $p = New-ClassLibrary

        $setting.AddOrUpdate('config', [NuGet.Configuration.AddItem]::new('PackageSaveMode', 'nuspec'))

        Install-Package Castle.Core -Version 1.2.0 -Project $p.Name
        Assert-Package $p Castle.Core 1.2.0

        # Act
        Update-Package Castle.Core

        # Assert
        # Assert-Package $p Castle.Core 2.5.1
    }
    finally {
        $setting.AddOrUpdate('config', [NuGet.Configuration.AddItem]::new('PackageSaveMode', $null))
    }
}

# Tests that when a dependency package is updated, its dependent packages
# will be updated to the right version
#function Test-UpdateSolutionOnlyPackage {
function UpdateSolutionOnlyPackage {
    param(
        $context
    )

    # Arrange
    $p = New-WebApplication
    $solutionDir = Get-SolutionDir

    # Act
    $p | Install-Package SolutionOnlyPackage -Source $context.RepositoryRoot -Version 1.0
    Assert-SolutionPackage SolutionOnlyPackage 1.0
    Assert-PathExists (Join-Path $solutionDir packages\SolutionOnlyPackage.1.0\file1.txt)

    $p | Update-Package SolutionOnlyPackage -Source $context.RepositoryRoot
    Assert-Null (Get-SolutionPackage SolutionOnlyPackage 1.0)
    Assert-SolutionPackage SolutionOnlyPackage 2.0
    Assert-PathExists (Join-Path $solutionDir packages\SolutionOnlyPackage.2.0\file2.txt)
}

#function Test-UpdateSolutionOnlyPackageWhenAmbiguous {
function UpdateSolutionOnlyPackageWhenAmbiguous {
    param(
        $context
    )

    # Arrange
    $p = New-MvcApplication
    Install-Package SolutionOnlyPackage -Version 1.0 -Source $context.RepositoryRoot
    Install-Package SolutionOnlyPackage -Version 2.0 -Source $context.RepositoryRoot

    Assert-SolutionPackage SolutionOnlyPackage 1.0
    Assert-SolutionPackage SolutionOnlyPackage 2.0

    Assert-Throws { Update-Package SolutionOnlyPackage } "Unable to update 'SolutionOnlyPackage'. Found multiple versions installed."
}

function Test-UpdatePackageResolvesDependenciesAcrossSources {
    param(
        $context
    )

    # Arrange
    $p = New-ConsoleApplication

    # Act
    # Ensure Antlr is not avilable in local repo.
    Assert-Null (Get-Package -ListAvailable -Source $context.RepositoryRoot Antlr)
    # Install a package with no external dependency
    Install-Package PackageWithExternalDependency -Source $context.RepositoryRoot -Version 0.5
    # Upgrade to a version that has an external dependency
    Update-Package PackageWithExternalDependency -Source $context.RepositoryRoot

    # Assert
    Assert-Package $p PackageWithExternalDependency
    Assert-Package $p Antlr
}

#function Test-UpdateAmbiguousProjectLevelPackageNoInstalledInProjectThrows {
function UpdateAmbiguousProjectLevelPackageNoInstalledInProjectThrows {
    # Arrange
    $p1 = New-ClassLibrary
    $p2 = New-FSharpLibrary
    $p1 | Install-Package Antlr -Version 3.1.1 -source $context.RepositoryPath
    $p2 | Install-Package Antlr -Version 3.1.3.42154 -source $context.RepositoryPath
    Remove-ProjectItem $p1 packages.config
    Remove-ProjectItem $p2 packages.config

    Assert-SolutionPackage Antlr 3.1.1
    Assert-SolutionPackage Antlr 3.1.3.42154
    @($p1, $p2) | %{ Assert-Null (Get-ProjectPackage $_ Antlr) }

    # Act
    Assert-Throws { $p1 | Update-Package Antlr } "Unable to find package 'Antlr' in '$($p1.Name)'."
}

function Test-AddingBindingRedirectAfterUpdate {
    [SkipTest('https://github.com/NuGet/Home/issues/12292')]
    param(
        $context
    )

    # Arrange
    $p = New-WebApplication

    # Act
    $p | Install-Package A -Source $context.RepositoryPath
    Assert-Package $p A 1.0
    Assert-Package $p B 1.0
    $p | Install-Package C -Source $context.RepositoryPath
    Assert-Package $p C 1.0
    Assert-Package $p B 2.0
    Assert-Null (Get-SolutionPackage B 1.0)

    Build-Solution

    Add-BindingRedirect

    Assert-BindingRedirect $p web.config B '0.0.0.0-2.0.0.0' '2.0.0.0'
}


function UpdatePackageAcceptsAllAsSourceName {
    # Arrange
    $p = New-ConsoleApplication
    Install-Package Antlr -Version 3.1.1 -Project $p.Name -Source 'All'

    Assert-Package $p Antlr 3.1.1

    # Act
    Update-Package Antlr -Version 3.1.3.42154 -Project $p.Name -Source 'All'

    # Assert
    Assert-Package $p Antlr 3.1.3.42154
}

# https://github.com/NuGet/Home/issues/9283
#function Test-UpdatePackageMissingPackage {
#    # Arrange
#    # create project and install package
#    $proj = New-ClassLibrary
#    $proj | Install-Package Castle.Core -Version 1.2.0
#    Assert-Package $proj Castle.Core 1.2.0
#
#    # delete the packages folder
#    $packagesDir = Get-PackagesDir
#    RemoveDirectory $packagesDir
#    Assert-False (Test-Path $packagesDir)
#
#    # Act
#	Update-Package Castle.Core -Version 2.5.1
#
#    # Assert
#	Assert-Package $proj Castle.Core 2.5.1
#}

function Test-UpdatePackageMissingPackageNoConsent {
	try {
        [NuGet.PackageManagement.VisualStudio.SettingsHelper]::Set('PackageRestoreConsentGranted', 'false')
        [NuGet.PackageManagement.VisualStudio.SettingsHelper]::Set('PackageRestoreIsAutomatic', 'false')

		# Arrange
		# create project and install package
		$proj = New-ClassLibrary
		$proj | Install-Package TestUpdatePackage -Source $context.RepositoryRoot -Version 1.0.0.0
		Assert-Package $proj TestUpdatePackage 1.0.0.0

		# delete the packages folder
		$packagesDir = Get-PackagesDir
		RemoveDirectory $packagesDir
		Assert-False (Test-Path $packagesDir)

		# Act
		# Assert
		Assert-Throws { Update-Package TestUpdatePackage -Source $context.RepositoryRoot } "Some NuGet packages are missing from the solution. The packages need to be restored in order to build the dependency graph. Restore the packages before performing any operations."
    }
    finally {
        [NuGet.PackageManagement.VisualStudio.SettingsHelper]::Set('PackageRestoreConsentGranted', 'true')
        [NuGet.PackageManagement.VisualStudio.SettingsHelper]::Set('PackageRestoreIsAutomatic', 'true')
    }
}

function Test-UpdatePackageOnAnFSharpProjectWithMultiplePackages {
    [SkipTest('https://github.com/NuGet/Home/issues/11982')]
    param($context)

    # Arrange
    $p = New-FSharpLibrary
    Build-Solution # wait for project nomination

    # Act
    $p | Install-Package SkypePackage -version 1.0 -source $context.RepositoryRoot
    $p | Install-Package netfx-Guard -Source $context.RepositoryRoot

    Update-Package -Source $context.RepositoryRoot -ProjectName $p.Name

    # Assert
    Assert-NetCorePackageInLockFile $p SkypePackage 3.0
}

function Test-UpdateScenariosWithConstraints {
    param(
        $context
    )

    # Arrange
    $p1 = New-ConsoleApplication
    $p2 = New-ClassLibrary
    $p3 = New-WebSite

    $p1 | Install-Package A -Version 1.0 -Source $context.RepositoryPath
    $p2 | Install-Package C -Version 1.0 -Source $context.RepositoryPath
    $p3 | Install-Package E -Version 1.0 -Source $context.RepositoryPath

    Add-PackageConstraint $p1 A "[1.0, 2.0)"
    Add-PackageConstraint $p2 D "[1.0]"
    Add-PackageConstraint $p3 E "[1.0]"

    # Act
    Assert-Throws { Update-Package -Version 2.0 A -Source $context.RepositoryPath } "Unable to resolve 'A'. An additional constraint '(>= 1.0.0 && < 2.0.0)' defined in packages.config prevents this operation."
    Assert-Throws { Update-Package C -Source $context.RepositoryPath } "Unable to find a version of 'D' that is compatible with 'C 2.0.0 constraint: D (>= 2.0.0)'. 'D' has an additional constraint (= 1.0.0) defined in packages.config."
    Assert-Throws { Update-Package F -Source $context.RepositoryPath } "Unable to resolve dependencies. 'F 2.0.0' is not compatible with 'E 1.0.0 constraint: F (= 1.0.0)'."

    # Assert
    Assert-Package $p1 A 1.0
    Assert-Package $p1 B 1.0
    Assert-SolutionPackage A 1.0
    Assert-SolutionPackage B 1.0

    Assert-Package $p2 C 1.0
    Assert-Package $p2 D 1.0
    Assert-SolutionPackage C 1.0
    Assert-SolutionPackage D 1.0

    Assert-Package $p3 E 1.0
    Assert-Package $p3 F 1.0
    Assert-SolutionPackage E 1.0
    Assert-SolutionPackage F 1.0
}

function Test-BindingRedirectShouldNotBeAddedForNonStrongNamedAssemblies {
    param(
        $context
    )
    # Arrange
    $p = New-ConsoleApplication
    $p | Install-Package NonStrongNameB -Version 1.0.0.0 -Source $context.RepositoryRoot
    $p | Install-Package NonStrongNameA -Source $context.RepositoryRoot

    # Act
    $p | Update-Package NonStrongNameB -Source $context.RepositoryRoot

    # Assert
    Assert-Null (Get-ProjectItem $p app.config)
}

function Test-UpdatePackageDoesNotConsiderPrereleasePackagesForSafeUpdateIfFlagIsNotSpecified {
     param(
        $context
    )

    # Arrange
    $p = New-ClassLibrary

    # Act
    $p | Install-Package -Source $context.RepositoryRoot -Id PreReleaseTestPackage -Version 1.0.0-a -Prerelease
    Assert-Package $p 'PreReleaseTestPackage'
    $p | Update-Package -Source $context.RepositoryRoot -Id PreReleaseTestPackage  -Safe

    # Assert
    Assert-Package $p PreReleaseTestPackage 1.0.0
}

function Test-UpdatePackageDoesNotConsiderPrereleasePackagesForSafeUpdateIfFlagIsNotSpecified {
     param(
        $context
    )

    # Arrange
    $p = New-ClassLibrary

    # Act
    $p | Install-Package -Source $context.RepositoryRoot -Id PreReleaseTestPackage -Version 1.0.0-a -Prerelease
    Assert-Package $p 'PreReleaseTestPackage'
    $p | Update-Package -Source $context.RepositoryRoot -Id PreReleaseTestPackage  -Safe -Prerelease

    # Assert
    Assert-Package $p PreReleaseTestPackage 1.0.1-a
}

# Currently PackageDownloader is a static class without eventHandlers and does not implement IHttpClientEvents
# TODO: Re-enable the test after IHttpClientEvents feature is implemented
function UpdatePackageDontMakeExcessiveNetworkRequests
{
    # Arrange
    $a = New-ClassLibrary

    $nugetsource = "https://www.nuget.org/api/v2/"

    $repository = Get-PackageRepository $nugetsource
    Assert-NotNull $repository

    $packageDownloader = $repository.PackageDownloader
    Assert-NotNull $packageDownloader

    $global:numberOfRequests = 0
    $eventId = "__DataServiceSendingRequest"

    $a | Install-Package "nugetpackageexplorer.types" -version 1.0 -source $nugetsource
    Assert-Package $a 'nugetpackageexplorer.types' '1.0'

    try
    {
        Register-ObjectEvent $packageDownloader "SendingRequest" $eventId { $global:numberOfRequests++; }

        # Act
        $a | Update-Package "nugetpackageexplorer.types" -version 2.0 -source $nugetsource

        # Assert
        Assert-Package $a 'nugetpackageexplorer.types' '2.0'
        Assert-AreEqual 1 $global:numberOfRequests
    }
    finally
    {
        Unregister-Event $eventId -ea SilentlyContinue
        Remove-Variable 'numberOfRequests' -Scope 'Global' -ea SilentlyContinue
    }
}

function Test-UpdatePackageDoesNotUninstallDependencyOfPreviousVersion
{
    param($context)

    # Arrange
    $project = New-ClassLibrary

    $global:InstallVar = 0

    Install-Package TestDependencyTargetFramework -Version 1.0 -Project $project.Name -Source $context.RepositoryPath

    Assert-Package $project TestDependencyTargetFramework -Version 1.0
    Assert-Package $project TestEmptyLibFolder
    Assert-NoPackage $project TestEmptyContentFolder
    Assert-NoPackage $project TestEmptyToolsFolder

    # Act
    Update-Package TestDependencyTargetFramework -Version 2.0 -Project $project.Name -Source $context.RepositoryPath -FileConflictAction OverwriteAll

    # Assert
    Assert-Package $project TestDependencyTargetFramework -Version 2.0
    Assert-Package $project TestEmptyToolsFolder
    Assert-Package $project TestEmptyLibFolder
    Assert-NoPackage $project TestEmptyContentFolder
}

function Test-UpdatingSatellitePackageUpdatesReferences
{
    param($context)

    # Arrange
    $p = New-ClassLibrary

    # Act - 1
    $p | Install-Package Localized.fr-FR -Version 1.0 -Source $context.RepositoryPath

    # Assert - 1
    Assert-Package $p Localized 1.0
    Assert-Package $p Localized.fr-FR 1.0

    $solutionDir = Get-SolutionDir
    $packageDir = (Join-Path $solutionDir packages\Localized.1.0)
    Assert-PathExists (Join-Path $packageDir 'lib\net40\fr-FR\Main.1.0.resources.dll')


    # Act
    $p | Update-Package Localized.fr-FR -Source $context.RepositoryPath

    # Assert
    Assert-Package $p Localized 2.0
    Assert-Package $p Localized.fr-FR 2.0
    $packageDir = (Join-Path $solutionDir packages\Localized.2.0)
    Assert-PathNotExists (Join-Path $packageDir 'lib\net40\fr-FR\Main.1.0.resources.dll')
    Assert-PathExists (Join-Path $packageDir 'lib\net40\fr-FR\Main.2.0.resources.dll')
}

function Test-UpdatingSatellitePackageWhenMultipleVersionsInstalled
{
    param($context)

    # Arrange
    $p1 = New-ClassLibrary
    $p2 = New-ClassLibrary

    # Act - 1
    $p1 | Install-Package Localized.fr-FR -Version 1.0 -Source $context.RepositoryPath
    $p2 | Install-Package Localized.fr-FR -Version 2.0 -Source $context.RepositoryPath
    $p2 | Install-Package DependsOnLocalized -Version 1.0 -Source $context.RepositoryPath

    # Assert - 1
    Assert-Package $p1 Localized 1.0
    Assert-Package $p1 Localized.fr-FR 1.0
    Assert-Package $p2 Localized 2.0
    Assert-Package $p2 Localized.fr-FR 2.0
    Assert-Package $p2 DependsOnLocalized 1.0

    $solutionDir = Get-SolutionDir
    Assert-PathExists (Join-Path $solutionDir 'packages\Localized.1.0\lib\net40\fr-FR\Main.1.0.resources.dll')
    Assert-PathExists (Join-Path $solutionDir 'packages\Localized.2.0\lib\net40\fr-FR\Main.2.0.resources.dll')

    # Act - 2
    $p2 | Update-Package DependsOnLocalized -Source $context.RepositoryPath

    # Assert - 2
    Assert-Package $p2 Localized 3.0
    Assert-Package $p2 Localized.fr-FR 3.0
    Assert-Package $p2 DependsOnLocalized 2.0

    Assert-PathNotExists (Join-Path $solutionDir 'packages\Localized.2.0\')
    Assert-PathExists (Join-Path $solutionDir 'packages\Localized.3.0\lib\net40\fr-FR\Main.3.0.resources.dll')

}

function Test-UpdatingPackagesWithDependenciesOnSatellitePackages
{
    param($context)

    # Arrange
    $p = New-ClassLibrary

    # Act - 1
    $p | Install-Package Localized.LangPack -Version 1.0 -Source $context.RepositoryPath

    # Assert - 1
    Assert-Package $p Localized 1.0
    Assert-Package $p Localized.fr-FR 1.0
    Assert-Package $p Localized.ja-JP 1.0
    Assert-Package $p Localized.LangPack 1.0

    $solutionDir = Get-SolutionDir
    Assert-PathExists (Join-Path $solutionDir 'packages\Localized.1.0\lib\net40\ja-JP\Main.1.0.resources.dll')
    Assert-PathExists (Join-Path $solutionDir 'packages\Localized.1.0\lib\net40\fr-FR\Main.1.0.resources.dll')

    # Act - 2
    $p | Update-Package Localized.LangPack -Source $context.RepositoryPath

    # Assert - 2
    Assert-Package $p Localized 2.0
    Assert-Package $p Localized.fr-FR 2.0
    Assert-Package $p Localized.ja-JP 2.0
    Assert-Package $p Localized.LangPack 2.0

    Assert-PathExists (Join-Path $solutionDir 'packages\Localized.2.0\lib\net40\ja-JP\Main.2.0.resources.dll')
    Assert-PathExists (Join-Path $solutionDir 'packages\Localized.2.0\lib\net40\fr-FR\Main.2.0.resources.dll')

}

function Test-UpdatingMetaPackageRemovesSatelliteReferences
{
    # Verification for work item 2313
    param ($context)

    # Arrange
    $p = New-ClassLibrary

    # Act - 1
    $p | Install-Package A.Localized -Version 1.0.0 -Source $context.RepositoryPath

    # Assert - 1
    Assert-Package $p A 1.0.0
    Assert-Package $p A.localized 1.0.0
    Assert-Package $p A.fr 1.0.0
    Assert-Package $p A.es 1.0.0

    # Act - 2
    $p | Update-Package A.Localized -Source $context.RepositoryPath

    # Assert - 1
    Assert-Package $p A 2.0.0
    Assert-Package $p A.localized 2.0.0
    Assert-Package $p A.fr 2.0.0
    Assert-Package $p A.es 2.0.0

    $solutionDir = Get-SolutionDir
    Assert-PathExists (Join-Path $solutionDir 'packages\A.Localized.2.0.0\')
    Assert-PathNotExists (Join-Path $solutionDir 'packages\A.Localized.1.0.0\')
    Assert-PathNotExists (Join-Path $solutionDir 'packages\A.fr.1.0.0\')
    Assert-PathNotExists (Join-Path $solutionDir 'packages\A.es.1.0.0\')
}

#function Test-ReinstallPackageReinstallPrereleaseDependencyPackages
function ReinstallPackageReinstallPrereleaseDependencyPackages
{
    param($context)

    # Arrange
    $sol = New-Solution

    $p1 = $sol | New-ClassLibrary
    $p2 = $sol | New-ConsoleApplication

    ($p1, $p2) | Install-Package A -Source $context.RepositoryPath -Pre

    Assert-Package $p1 "A" "1.0.0-alpha"
    Assert-Package $p1 "B" "2.0.0-beta"

    Assert-Package $p2 "A" "1.0.0-alpha"
    Assert-Package $p2 "B" "2.0.0-beta"

    # Act
    Update-Package A -Reinstall -Source $context.RepositoryPath

    # Assert
    Assert-Package $p1 "A" "1.0.0-alpha"
    Assert-Package $p1 "B" "2.0.0-beta"

    Assert-Package $p2 "A" "1.0.0-alpha"
    Assert-Package $p2 "B" "2.0.0-beta"
}

function Test-FinishFailedUpdateOnSolutionOpen
{
    param($context)

    # Arrange
    $p = New-ConsoleApplication

    $componentService = Get-VSComponentModel
	$solutionManager = $componentService.GetService([NuGet.PackageManagement.ISolutionManager])
	$setting = $componentService.GetService([NuGet.Configuration.ISettings])
	$packageFolderPath = [NuGet.PackageManagement.PackagesFolderPathUtility]::GetPackagesFolderPath($solutionManager, $setting)

	$p | Install-Package TestUpdatePackage -Version 1.0 -Source $context.RepositoryRoot

    # We will open a file handle preventing the deletion packages\SolutionOnlyPackage.1.0\file1.txt
    # causing the uninstall to fail to complete thereby forcing it to finish the next time the solution is opened
    $filePath = Join-Path $packageFolderPath "TestUpdatePackage.1.0.0.0\content\readme.txt"
    $fileStream = [System.IO.File]::Open($filePath, [System.IO.FileMode]::Open, [System.IO.FileAccess]::Read, [System.IO.FileShare]::Read)

    try {
        # Act
        $p | Update-Package TestUpdatePackage -Source $context.RepositoryRoot

        # Assert
        Assert-True [NuGet.ProjectManagement.FileSystemUtility]::DirectoryExists($packageFolderPath,"TestUpdatePackage.1.0.0.0")
        Assert-True [NuGet.ProjectManagement.FileSystemUtility]::FileExists($packageFolderPath,"TestUpdatePackage.1.0.0.0.deleteme")
        Assert-True [NuGet.ProjectManagement.FileSystemUtility]::DirectoryExists($packageFolderPath, "TestUpdatePackage.2.0.0.0")
    } finally {
        $fileStream.Close()
    }

    # Act
    # After closing the file handle, we close the solution and reopen it
    $solutionDir = Get-SolutionFullName
    Close-Solution
    Open-Solution $solutionDir

    # Assert
    Assert-False [NuGet.ProjectManagement.FileSystemUtility]::DirectoryExists($packageFolderPath,"TestUpdatePackage.1.0.0.0")
    Assert-False [NuGet.ProjectManagement.FileSystemUtility]::FileExists($packageFolderPath,"TestUpdatePackage.1.0.0.0.deleteme")
    Assert-True [NuGet.ProjectManagement.FileSystemUtility]::DirectoryExists($packageFolderPath, "TestUpdatePackage.2.0.0.0")
}

#function Test-UpdatePackageWhenAnUnusedVersionOfPackageIsPresentInPackagesFolder
function UpdatePackageWhenAnUnusedVersionOfPackageIsPresentInPackagesFolder
{
    param($context)

    # Arrange
    $p = New-ClassLibrary

    # Install TestUpdatePackage 1.0.0 at solution level only
    $p | Install-Package TestUpdatePackage -version 1.0.0 -Source $context.RepositoryRoot
    Remove-ProjectItem $p packages.config

    # Install TestUpdatePackage 1.0.1 in project
    $p | Install-Package TestUpdatePackage -version 1.0.1 -Source $context.RepositoryRoot

    # Check that TestUpdatePackage 1.0.0 is solution level only and 1.0.1 is in project level
    Assert-NoPackage $p TestUpdatePackage -Version 1.0.0
    Assert-Package $p TestUpdatePackage 1.0.1
    Assert-SolutionPackage TestUpdatePackage 1.0.0
    Assert-SolutionPackage TestUpdatePackage 1.0.1

    # Act
    Update-Package -Source $context.RepositoryRoot

    # Assert
    Assert-NoPackage $p TestUpdatePackage -Version 1.0.1
    Assert-NoPackage $p TestUpdatePackage -Version 1.0.0
    Assert-Package $p TestUpdatePackage 2.0.0
    Assert-NoSolutionPackage TestUpdatePackage 1.0.1
    Assert-SolutionPackage TestUpdatePackage 1.0.0
    Assert-SolutionPackage TestUpdatePackage 2.0.0
}

#function Test-UpdatePackageThrowsWhenOnlyUnusedVersionsOfAPackageIsPresentInPackagesFolder
function UpdatePackageThrowsWhenOnlyUnusedVersionsOfAPackageIsPresentInPackagesFolder
{
    param($context)

    # Arrange
    $p = New-ClassLibrary

    # Install TestUpdatePackage 1.0.0 at solution level only
    $p | Install-Package TestUpdatePackage -version 1.0.0 -Source $context.RepositoryRoot
    Remove-ProjectItem $p packages.config

    # Install TestUpdatePackage 1.0.1 in project
    $p | Install-Package TestUpdatePackage -version 1.0.1 -Source $context.RepositoryRoot
    Remove-ProjectItem $p packages.config

    # Check that both the versions of TestUpdatePackage is solution level only
    Assert-NoPackage $p TestUpdatePackage -Version 1.0.0
    Assert-NoPackage $p TestUpdatePackage 1.0.1
    Assert-SolutionPackage TestUpdatePackage 1.0.0
    Assert-SolutionPackage TestUpdatePackage 1.0.1

    # Act & Assert
    # Update specific package here. Because, when all packages are updated, the PackageNotInstalledException gets caught. We want it to be thrown
    Assert-Throws { Update-Package TestUpdatePackage -Source $context.RepositoryRoot } "'TestUpdatePackage' was not installed in any project. Update failed."
}

function Test-UpdatePackageWithContentInLicenseBlocks
{
    param($context)

    # Arrange
    $p = New-ClassLibrary

    $name = 'PackageWithTextFile'

    Install-Package $name -Version 1.0 -Source $context.RepositoryRoot

    $packages = Get-PackagesDir
    $fooFilePath = Join-Path $packages "$name.1.0\content\text"

    Assert-True (Test-Path $fooFilePath)

    '***************NUget: Begin License Text ---------dsafdsafdas
sdaflkjdsal;fj;ldsafdsa
dsaflkjdsa;lkfj;ldsafas
dsafdsafdsafsdaNuGet: End License Text-------------
This is a text file 1.0' > $fooFilePath

    # Act
    Update-Package $name -Source $context.RepositoryRoot

    # Assert
    Assert-Package $p $name '2.0'

    $textFilePathInProject = Join-Path (Get-ProjectDir $p) 'text'
    Assert-True (Test-Path $textFilePathInProject)

    Assert-AreEqual 'This is a text file 2.0' (Get-Content $textFilePathInProject)
}

function Test-UpdatePackagePreservesProjectConfigFile
{
    param($context)

    # Arrange
    $p = New-ClassLibrary "CoolProject"

    $p | Install-Package TestUpdatePackage -version 1.0 -source $context.RepositoryRoot

    $file = Get-ProjectItem $p 'packages.config'
    Assert-NotNull $file

    # rename it
    $file.Name = 'packages.CoolProject.config'

    # Act
    $p | Update-Package TestUpdatePackage -source $context.RepositoryRoot

    # Assert
    Assert-Package $p TestUpdatePackage '2.0'

    Assert-NotNull (Get-ProjectItem $p 'packages.CoolProject.config')
    Assert-Null (Get-ProjectItem $p 'packages.config')
}

# Test update-package ordering
function Test-UpdatingPackageInstallOrdering {
    param(
        $context
    )

    # Arrange
    $p = New-ConsoleApplication

	# A.1 depends on B.1 depends on C.1 however A.2 depends on C.2 depends on B.2 (B and C are swapped)

    # Act
    Install-Package A -Version 1.0 -Source $context.RepositoryPath
    Assert-Package $p A 1.0
    Assert-Package $p B 1.0
    Assert-Package $p C 1.0

    Update-Package A -Source $context.RepositoryPath
    # Make sure the new package is installed
    Assert-Package $p A 2.0
    Assert-Package $p B 2.0
    Assert-Package $p C 2.0

    # Make sure the old package is removed
    Assert-Null (Get-ProjectPackage $p A 1.0)
    Assert-Null (Get-ProjectPackage $p B 1.0)
    Assert-Null (Get-ProjectPackage $p C 1.0)
}

function Test-UpdatingBindingRedirectAfterUpdate {
    [SkipTest('https://github.com/NuGet/Home/issues/12292')]
    param(
        $context
    )

    # Arrange
    $p = New-WebApplication

    # Act
    $p | Install-Package B -Version 2.0 -Source $context.RepositoryPath
    $p | Install-Package A -Version 1.0 -Source $context.RepositoryPath

    # Assert
    Assert-BindingRedirect $p web.config B '0.0.0.0-2.0.0.0' '2.0.0.0'

    # ACT
    $p | Update-Package B -Version 3.0 -Source $context.RepositoryPath

    # Assert
    Assert-Package $p B 3.0
    Assert-BindingRedirect $p web.config B '0.0.0.0-3.0.0.0' '3.0.0.0'
}

function Test-CanReinstallDelistedPackage
{
    # Arrange
    # using a delisted package
    $nugetsource = "https://www.nuget.org/api/v2/"
    $p = New-ClassLibrary
    $p | Install-Package Rx-Core -Version 2.2.5 -Source $nugetsource

    # Act
    Update-Package Rx-Core -Reinstall -ProjectName $p.Name -Source $nugetsource

    # Assert
    # we get here as act errored prior to fix
    Assert-Package $p Rx-Core 2.2.5
}

function RemoveDirectory {
    param($dir)

    $iteration = 0
    while ($iteration++ -lt 10)
    {
        if (Test-Path $dir)
        {
            # because -Recurse parameter in Remove-Item has a known issue so using Get-ChildItem to
            # first delete all the children and then delete the folder.
            Get-ChildItem $dir -Recurse | Remove-Item -Force -Recurse -ErrorAction SilentlyContinue
            Remove-Item -Recurse -Force $dir -ErrorAction SilentlyContinue
        }
        else
        {
            break;
        }
    }
}
