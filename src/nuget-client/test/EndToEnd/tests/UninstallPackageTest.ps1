function Test-RemovingPackageFromProjectDoesNotRemoveIfInUse {
    # Arrange
    $p1 = New-ClassLibrary
    $p2 = New-ClassLibrary

    Install-Package Ninject -ProjectName $p1.Name
    Assert-Reference $p1 Ninject

    Install-Package Ninject -ProjectName $p2.Name
    Assert-Reference $p2 Ninject

    Uninstall-Package Ninject -ProjectName $p1.Name

    Assert-Null (Get-ProjectPackage $p1 Ninject)
    Assert-Null (Get-AssemblyReference $p1 Ninject)
    Assert-SolutionPackage Ninject
}

function Test-UninstallPackageWhatIf {
    # Arrange
    $p1 = New-ClassLibrary

    Install-Package Ninject -ProjectName $p1.Name
    Assert-Reference $p1 Ninject

	# Act
    Uninstall-Package Ninject -ProjectName $p1.Name -What

	# Assert: packages are not uninstalled
	Assert-Reference $p1 Ninject
	Assert-Package $p1 Ninject
}

function Test-RemovingPackageWithDependencyFromProjectDoesNotRemoveIfInUse {
    # Arrange
    $p1 = New-WebApplication
    $p2 = New-WebApplication

    $p1 | Install-Package jquery.Validation
    Assert-Package $p1 jquery.Validation
    Assert-Package $p1 jquery

    $p2 | Install-Package jquery.Validation
    Assert-Package $p1 jquery.Validation
    Assert-Package $p1 jquery

    $p1 | Uninstall-Package jquery.Validation
    $p1 | Uninstall-Package jquery

    Assert-Null (Get-ProjectPackage $p1 jquery.Validation)
    Assert-Null (Get-ProjectPackage $p1 jquery)
    Assert-SolutionPackage jquery.Validation
    Assert-SolutionPackage jquery
}

function Test-RemovePackageRemovesPackageFromSolutionIfNotInUse {
    # Arrange
    $p1 = New-WebApplication

    Install-Package elmah -ProjectName $p1.Name -Version 1.1
    Assert-Reference $p1 elmah
    Assert-SolutionPackage elmah

    Uninstall-Package elmah -ProjectName $p1.Name
    Assert-Null (Get-AssemblyReference $p1 elmah)
    Assert-Null (Get-ProjectPackage $p1 elmah)
    Assert-Null (Get-SolutionPackage elmah)
}

function Test-UninstallingPackageWithConfigTransformWhenConfigReadOnly {
    # Arrange
    $p1 = New-WebApplication

    Install-Package elmah -ProjectName $p1.Name -Version 1.1
    Assert-Reference $p1 elmah
    Assert-SolutionPackage elmah
    attrib +R (Get-ProjectItemPath $p1 web.config)

    Uninstall-Package elmah -ProjectName $p1.Name
    Assert-Null (Get-AssemblyReference $p1 elmah)
    Assert-Null (Get-ProjectPackage $p1 elmah)
    Assert-Null (Get-SolutionPackage elmah)
}

function Test-VariablesPassedToUninstallScriptsAreValidWithWebSite {
    param(
        $context
    )

    # Arrange
    $p = New-WebSite

    Install-Package PackageWithScripts -ProjectName $p.Name -Source $context.RepositoryRoot

    # This asserts install.ps1 gets called with the correct project reference and package
    Assert-Reference $p System.Windows.Forms

     # Act
    Uninstall-Package PackageWithScripts -ProjectName $p.Name
    Assert-Null (Get-AssemblyReference $p System.Windows.Forms)
}

function Test-UninstallPackageWithNestedContentFiles {
    param(
        $context
    )

    # Arrange
    $p = New-WebApplication
    Install-Package NestedFolders -ProjectName $p.Name -Source $context.RepositoryPath

    # Act
    Uninstall-Package NestedFolders -ProjectName $p.Name

    # Assert
    Assert-Null (Get-ProjectItem $p a)
    Assert-Null (Get-ProjectItem $p a\b)
    Assert-Null (Get-ProjectItem $p a\b\c)
    Assert-Null (Get-ProjectItem $p a\b\c\test.txt)
}

function Test-SimpleFSharpUninstall {
    [SkipTest('https://github.com/NuGet/Home/issues/11982')]
    param($context)

    # Arrange
    $p = New-FSharpLibrary
    Build-Solution # wait for project nomination

    # Act
    Install-Package Ninject -ProjectName $p.Name -Source $context.RepositoryPath -version 2.0.1
    Build-Solution # wait for assets file to be updated
    Assert-NetCorePackageInLockFile $p Ninject 2.0.1
    Uninstall-Package Ninject -ProjectName $p.Name
    Build-Solution # wait for assets file to be updated

    # Assert
    Assert-NetCorePackageNotInLockFile $p Ninject
}

function Test-UninstallPackageThatIsNotInstalledThrows {
    # Arrange
    $p = New-ClassLibrary

    # Act & Assert
    Assert-Throws { $p | Uninstall-Package elmah } ("Package 'elmah' to be uninstalled could not be found in project '" + $p.Name + "'")
}

function Test-UninstallPackageThatIsInstalledInAnotherProjectThrows {
    # Arrange
    $p1 = New-ClassLibrary
    $p2 = New-ClassLibrary
    $p1 | Install-Package elmah -Version 1.1

    # Act & Assert
    Assert-Throws { $p2 | Uninstall-Package elmah } ("Package 'elmah' to be uninstalled could not be found in project '" + $p2.Name + "'")
}

#function Test-UninstallSolutionOnlyPackage {
function UninstallSolutionOnlyPackage {
    param(
        $context
    )

    # Arrange
    $p = New-MvcApplication
    $p | Install-Package SolutionOnlyPackage -Source $context.RepositoryRoot

    Assert-SolutionPackage SolutionOnlyPackage 2.0

    Uninstall-Package SolutionOnlyPackage

    Assert-Null (Get-SolutionPackage SolutionOnlyPackage 2.0)
}

function Test-UninstallPackageMissingPackage {
	# Arrange
	# create project and install package
	$proj = New-ClassLibrary
    $proj | Install-Package Castle.Core -Version 1.2.0
    Assert-Package $proj Castle.Core 1.2.0

	# delete the packages folder
	$packagesDir = Get-PackagesDir
	RemoveDirectory $packagesDir
	Assert-False (Test-Path $packagesDir)

	# Act
	Uninstall-Package Castle.Core

	# Assert
    Assert-Null (Get-ProjectPackage $proj Castle.Core)
}

function Test-UninstallPackageMissingPackageNoConsent {
    try {
        [NuGet.PackageManagement.VisualStudio.SettingsHelper]::Set('PackageRestoreConsentGranted', 'false')
        [NuGet.PackageManagement.VisualStudio.SettingsHelper]::Set('PackageRestoreIsAutomatic', 'false')

		# Arrange
		# create project and install package
		$proj = New-ClassLibrary
		$proj | Install-Package PackageWithContentFile -Source $context.RepositoryRoot -Version 1.0.0.0
		Assert-Package $proj PackageWithContentFile 1.0

		# delete the packages folder
		$packagesDir = Get-PackagesDir
		RemoveDirectory $packagesDir
		Assert-False (Test-Path $packagesDir)

		# Act
		# Assert
		Assert-Throws { Uninstall-Package PackageWithContentFile } "Some NuGet packages are missing from the solution. The packages need to be restored in order to build the dependency graph. Restore the packages before performing any operations."
    }
    finally {
        [NuGet.PackageManagement.VisualStudio.SettingsHelper]::Set('PackageRestoreConsentGranted', 'true')
        [NuGet.PackageManagement.VisualStudio.SettingsHelper]::Set('PackageRestoreIsAutomatic', 'true')
    }
}

#function Test-UninstallSpecificPackageThrowsIfNotInstalledInProject {
function UninstallSpecificPackageThrowsIfNotInstalledInProject {
    # Arrange
    $p1 = New-ClassLibrary
    $p2 = New-FSharpLibrary
    $p1 | Install-Package Antlr -Version 3.1.1 -Source $context.RepositoryPath
    $p2 | Install-Package Antlr -Version 3.1.3.42154 -Source $context.RepositoryPath

    # Act
    Assert-Throws { $p2 | Uninstall-Package Antlr -Version 3.1.1 } "Unable to find package 'Antlr 3.1.1' in '$($p2.Name)'."
}

function Test-UninstallSpecificVersionOfPackage {
    # Arrange
    $p1 = New-ClassLibrary
    $p2 = New-ClassLibrary
    $p1 | Install-Package Antlr -Version 3.1.1 -Source $context.RepositoryPath
    $p2 | Install-Package Antlr -Version 3.1.3.42154 -Source $context.RepositoryPath

    # Act
    $p1 | Uninstall-Package Antlr -Version 3.1.1

    # Assert
    Assert-Null (Get-ProjectPackage $p1 Antlr 3.1.1)
    Assert-Null (Get-SolutionPackage Antlr 3.1.1)
    Assert-SolutionPackage Antlr 3.1.3.42154
}

#function Test-UninstallSolutionOnlyPackageWhenAmbiguous {
function UninstallSolutionOnlyPackageWhenAmbiguous {
    param(
        $context
    )

    # Arrange
    $p = New-MvcApplication
    Install-Package SolutionOnlyPackage -Version 1.0 -Source $context.RepositoryRoot
    Install-Package SolutionOnlyPackage -Version 2.0 -Source $context.RepositoryRoot

    Assert-SolutionPackage SolutionOnlyPackage 1.0
    Assert-SolutionPackage SolutionOnlyPackage 2.0

    Assert-Throws { Uninstall-Package SolutionOnlyPackage } "Found multiple versions of 'SolutionOnlyPackage' installed. Please specify a version."
}

function Test-UninstallPackageWorksWithPackagesHavingSameNames {
    #
    #  Folder1
    #     + ProjectA
    #     + ProjectB
    #  Folder2
    #     + ProjectA
    #     + ProjectC
    #  ProjectA
    #

    # Arrange
    New-SolutionFolder 'Folder1'
    $p1 = New-ClassLibrary 'ProjectA' 'Folder1'
    $p2 = New-ClassLibrary 'ProjectB' 'Folder1'

    New-SolutionFolder 'Folder2'
    $p3 = New-ClassLibrary 'ProjectA' 'Folder2'
    $p4 = New-ConsoleApplication 'ProjectC' 'Folder2'

    $p5 = New-ConsoleApplication 'ProjectA'

    # Act
    Get-Project -All | Install-Package elmah -Version 1.1
    $all = @( $p1, $p2, $p3, $p4, $p5 )
    $all | % { Assert-Package $_ elmah }

    Get-Project -All | Uninstall-Package elmah

    # Assert
    $all | % { Assert-Null (Get-ProjectPackage $_ elmah) }
}

function Test-UninstallPackageWithXmlTransformAndTokenReplacement {
    param(
        $context
    )

    # Arrange
    $p = New-WebApplication
    $p | Install-Package PackageWithXmlTransformAndTokenReplacement -Source $context.RepositoryRoot

    # Assert
    $ns = $p.Properties.Item("DefaultNamespace").Value
    $assemblyName = $p.Properties.Item("AssemblyName").Value
    $path = (Get-ProjectItemPath $p web.config)
    $content = [System.IO.File]::ReadAllText($path)
    $expectedContent = "type=`"$ns.MyModule, $assemblyName`""
    Assert-True ($content.Contains($expectedContent))

    # Act
    $p | Uninstall-Package PackageWithXmlTransformAndTokenReplacement
    $content = [System.IO.File]::ReadAllText($path)
    Assert-False ($content.Contains($expectedContent))
}

function Test-UninstallPackageAfterRenaming {
    param(
        $context
    )
    # Arrange
    New-SolutionFolder 'Folder1'
    New-SolutionFolder 'Folder1\Folder2'
    $p0 = New-ClassLibrary 'ProjectX'
    $p1 = New-ClassLibrary 'ProjectA' 'Folder1\Folder2'
    $p2 = New-ClassLibrary 'ProjectB' 'Folder1\Folder2'

    # Act
    $p1 | Install-Package NestedFolders -Source $context.RepositoryPath
    $p1.Name = "ProjectX"
    Uninstall-Package NestedFolders -ProjectName Folder1\Folder2\ProjectX

    $p2 | Install-Package NestedFolders -Source $context.RepositoryPath
    Rename-SolutionFolder "Folder1\Folder2" "Folder3"
    Uninstall-Package NestedFolders -ProjectName Folder1\Folder3\ProjectB

    Assert-Null (Get-ProjectItem $p1 scripts\jquery-1.5.js)
    Assert-Null (Get-ProjectItem $p2 scripts\jquery-1.5.js)
}

function Test-UninstallDoesNotRemoveFolderIfNotEmpty {
    param(
        $context
    )
    # Arrange
    $p = New-WebApplication
    $p | Install-Package PackageWithFolder -Source $context.RepositoryRoot

    # Get the path to the foo folder
    $fooPath = (Join-Path (Split-Path $p.FullName) Foo)

    # Add 5 files to that folder (on disk but not in the project)
    0..5 | %{ "foo" | Out-File (Join-Path $fooPath "file$_.out") }

    Uninstall-Package PackageWithFolder

    Assert-Null (Get-ProjectPackage $p PackageWithFolder)
    Assert-Null (Get-SolutionPackage PackageWithFolder)
    Assert-PathExists $fooPath
}

function Test-WebSiteUninstallPackageWithPPCSSourceFiles {
    param(
        $context
    )
    # Arrange
    $p = New-WebSite

    # Act
    $p | Install-Package PackageWithPPCSSourceFiles -Source $context.RepositoryRoot
    Assert-Package $p PackageWithPPCSSourceFiles
    Assert-SolutionPackage PackageWithPPCSSourceFiles
    Assert-NotNull (Get-ProjectItem $p App_Code\Foo.cs)
    Assert-NotNull (Get-ProjectItem $p App_Code\Bar.cs)

    # Assert
    $p | Uninstall-Package PackageWithPPCSSourceFiles
    Assert-Null (Get-ProjectItem $p App_Code\Foo.cs)
    Assert-Null (Get-ProjectItem $p App_Code\Bar.cs)
    Assert-Null (Get-ProjectItem $p App_Code)
}

function Test-WebSiteUninstallPackageWithPPVBSourceFiles {
    param(
        $context
    )
    # Arrange
    $p = New-WebSite

    # Act
    $p | Install-Package PackageWithPPVBSourceFiles -Source $context.RepositoryRoot
    Assert-Package $p PackageWithPPVBSourceFiles
    Assert-SolutionPackage PackageWithPPVBSourceFiles
    Assert-NotNull (Get-ProjectItem $p App_Code\Foo.vb)
    Assert-NotNull (Get-ProjectItem $p App_Code\Bar.vb)

    # Assert
    $p | Uninstall-Package PackageWithPPVBSourceFiles
    Assert-Null (Get-ProjectItem $p App_Code\Foo.vb)
    Assert-Null (Get-ProjectItem $p App_Code\Bar.vb)
    Assert-Null (Get-ProjectItem $p App_Code)
}

function Test-WebSiteUninstallPackageWithNestedSourceFiles {
    param(
        $context
    )
    # Arrange
    $p = New-WebSite

    # Act
    $p | Install-Package netfx-Guard -Source $context.RepositoryRoot
    Assert-Package $p netfx-Guard
    Assert-SolutionPackage netfx-Guard
    Assert-NotNull (Get-ProjectItem $p App_Code\netfx\System\Guard.cs)

    # Assert
    $p | Uninstall-Package netfx-Guard
    Assert-Null (Get-ProjectPackage $p netfx-Guard)
    Assert-Null (Get-SolutionPackage netfx-Guard)
    Assert-Null (Get-ProjectItem $p App_Code\netfx\System\Guard.cs)
    Assert-Null (Get-ProjectItem $p App_Code\netfx\System)
    Assert-Null (Get-ProjectItem $p App_Code\netfx)
    Assert-Null (Get-ProjectItem $p App_Code)
}

function Test-WebSiteUninstallWithNestedAspxPPFiles {
    param(
        $context
    )

    # Arrange
    $p = New-WebSite
    $files = @('About.aspx')
    $p | Install-Package PackageWithNestedAspxPPFiles -Source $context.RepositoryRoot

    $files | %{
        $item = Get-ProjectItem $p $_
        Assert-NotNull $item
        $codeItem = Get-ProjectItem $p "$_.cs"
        Assert-NotNull $codeItem
    }

    Assert-Package $p PackageWithNestedAspxPPFiles 1.0
    Assert-SolutionPackage PackageWithNestedAspxPPFiles 1.0

    # Act
    $p | Uninstall-Package PackageWithNestedAspxPPFiles

    # Assert
    $files | %{
        $item = Get-ProjectItem $p $_
        Assert-Null $item
        $codeItem = Get-ProjectItem $p "$_.cs"
        Assert-Null $codeItem
    }

    Assert-Null (Get-ProjectPackage $p PackageWithNestedAspxPPFiles 1.0)
    Assert-Null (Get-SolutionPackage PackageWithNestedAspxPPFiles 1.0)
}

function Test-WebsiteUninstallPackageWithNestedAspxFiles {
    param(
        $context
    )

    # Arrange
    $p = New-WebSite
    $files = @('Global.asax', 'Site.master', 'About.aspx')
    $p | Install-Package PackageWithNestedAspxFiles -Source $context.RepositoryRoot

    $files | %{
        $item = Get-ProjectItem $p $_
        Assert-NotNull $item
        $codeItem = Get-ProjectItem $p "$_.cs"
        Assert-NotNull $codeItem
    }

    Assert-Package $p PackageWithNestedAspxFiles 1.0
    Assert-SolutionPackage PackageWithNestedAspxFiles 1.0

    # Act
    $p | Uninstall-Package PackageWithNestedAspxFiles

    # Assert
    $files | %{
        $item = Get-ProjectItem $p $_
        Assert-Null $item
        $codeItem = Get-ProjectItem $p "$_.cs"
        Assert-Null $codeItem
    }
    Assert-Null (Get-ProjectPackage $p PackageWithNestedAspxFiles 1.0)
    Assert-Null (Get-SolutionPackage PackageWithNestedAspxFiles 1.0)
}

function Test-WebSiteUninstallPackageWithNestedSourceFilesAndAnotherProject {
    param(
        $context
    )
    # Arrange
    $p1 = New-WebSite
    $p2 = New-WebApplication

    # Act
    $p1 | Install-Package netfx-Guard -Source $context.RepositoryRoot
    Assert-Package $p1 netfx-Guard
    Assert-SolutionPackage netfx-Guard
    Assert-NotNull (Get-ProjectItem $p1 App_Code\netfx\System\Guard.cs)

    $p2 | Install-Package netfx-Guard -Source $context.RepositoryRoot
    Assert-Package $p2 netfx-Guard
    Assert-SolutionPackage netfx-Guard
    Assert-NotNull (Get-ProjectItem $p2 netfx\System\Guard.cs)

    # Assert
    $p1 | Uninstall-Package netfx-Guard
    Assert-NotNull (Get-SolutionPackage netfx-Guard)
    Assert-Null (Get-ProjectPackage $p1 netfx-Guard)
    Assert-Null (Get-ProjectItem $p1 App_Code\netfx\System\Guard.cs)
    Assert-Null (Get-ProjectItem $p1 App_Code\netfx\System)
    Assert-Null (Get-ProjectItem $p1 App_Code\netfx)
    Assert-Null (Get-ProjectItem $p1 App_Code)
}

function Test-UninstallPackageSwallowExceptionThrownByUninstallScript {
   param(
       $context
   )

   # Arrange
   $p = New-ConsoleApplication
   $p | Install-Package TestUninstallThrowPackage -Source $context.RepositoryRoot
   Assert-Package $p TestUninstallThrowPackage

   # Act
   $p | Uninstall-Package TestUninstallThrowPackage

   # Assert
   Assert-Null (Get-ProjectPackage $p TestUninstallThrowPackage)

}

function Test-UninstallPackageInvokeInstallScriptWhenProjectNameHasApostrophe {
    param(
        $context
    )

    # Arrange
    New-Solution "Gun 'n Roses"
    $p = New-ConsoleApplication

    Install-Package TestUpdatePackage -Version 1.0.0.0 -Source $context.RepositoryRoot

    $global:UninstallPackageMessages = @()

    $expectedMessage = "uninstall" + $p.Name

    # Act
    Uninstall-Package TestUpdatePackage -Version 1.0.0.0

    # Assert
    Assert-AreEqual 1 $global:UninstallPackageMessages.Count
    Assert-AreEqual $expectedMessage $global:UninstallPackageMessages[0]

    # Clean up
    Remove-Variable UninstallPackageMessages -Scope Global
}

function Test-UninstallPackageInvokeInstallScriptWhenProjectNameHasBrackets {
    param(
        $context
    )

    # Arrange
    New-Solution "Gun [] Roses 2"
    $p = New-ConsoleApplication

    Install-Package TestUpdatePackage -Version 1.0.0.0 -Source $context.RepositoryRoot

    $global:UninstallPackageMessages = @()

    $expectedMessage = "uninstall" + $p.Name

    # Act
    Uninstall-Package TestUpdatePackage -Version 1.0.0.0

    # Assert
    Assert-AreEqual 1 $global:UninstallPackageMessages.Count
    Assert-AreEqual $expectedMessage $global:UninstallPackageMessages[0]

    # Clean up
    Remove-Variable UninstallPackageMessages -Scope Global
}

#function Test-UninstallPackageRemoveSolutionPackagesConfig
function UninstallPackageRemoveSolutionPackagesConfig
{
    param(
        $context
    )

    # Arrange
    $a = New-ClassLibrary

    $a | Install-Package SolutionOnlyPackage -version 1.0 -source $context.RepositoryRoot

    $solutionFile = Get-SolutionFullName
    $solutionDir = Split-Path $solutionFile -Parent

    $configFile = "$solutionDir\.nuget\packages.config"

    Assert-True (Test-Path $configFile)

    $content = Get-Content $configFile
    Assert-AreEqual 4 $content.Length
    Assert-AreEqual '<?xml version="1.0" encoding="utf-8"?>' $content[0]
    Assert-AreEqual '<packages>' $content[1]
    Assert-AreEqual '  <package id="SolutionOnlyPackage" version="1.0" />' $content[2]
    Assert-AreEqual '</packages>' $content[3]

    # Act
    $a | Uninstall-Package SolutionOnlyPackage

    # Assert
    Assert-False (Test-Path $configFile)
}

function Test-UninstallSolutionPackageRemoveEntryFromProjectPackagesConfig
{
    param(
        $context
    )

    # Arrange
    $a = New-ClassLibrary

    $a | Install-Package SolutionLevelPkg -version 1.0.0 -source $context.RepositoryRoot
    $a | Install-Package RazorGenerator.MsBuild -version 1.3.2

    $solutionFile = Get-SolutionFullName
    $solutionDir = Split-Path $solutionFile -Parent

    $configFile = "$solutionDir\" + $a.Name + "\packages.config"

    Assert-True (Test-Path $configFile)

    $content = Get-Content $configFile
    Assert-AreEqual 5 $content.Length
    Assert-AreEqual '<?xml version="1.0" encoding="utf-8"?>' $content[0]
    Assert-AreEqual '<packages>' $content[1]
    # Currently, when running NuGet api V2, we write the non-normalized version. So, replace 1.3.2.0 with 1.3.2. Related to bug: https://github.com/NuGet/Home/issues/577
    Assert-AreEqual '  <package id="RazorGenerator.MsBuild" version="1.3.2" targetFramework="net48" />' $content[2].Replace("1.3.2.0", "1.3.2")
    Assert-AreEqual '  <package id="SolutionLevelPkg" version="1.0.0" targetFramework="net48" />' $content[3]
    Assert-AreEqual '</packages>' $content[4]

    # Act
    $a | Uninstall-Package RazorGenerator.MsBuild

    # Assert
    $content = Get-Content $configFile
    Assert-AreEqual 4 $content.Length
    Assert-AreEqual '<?xml version="1.0" encoding="utf-8"?>' $content[0]
    Assert-AreEqual '<packages>' $content[1]
    Assert-AreEqual '  <package id="SolutionLevelPkg" version="1.0.0" targetFramework="net48" />' $content[2]
    Assert-AreEqual '</packages>' $content[3]
}

function Test-UninstallingSatellitePackageRemovesFilesFromRuntimePackageFolder
{
    param(
        $context
    )

    # Arrange
    $p = New-ClassLibrary
    $solutionDir = Get-SolutionDir

    # Act
    $p | Install-Package PackageWithStrongNamedLib -Source $context.RepositoryPath
    $p | Install-Package PackageWithStrongNamedLib.ja-jp -Source $context.RepositoryPath

    $p | Uninstall-Package PackageWithStrongNamedLib.ja-jp

    # Assert (the resources from the satellite package are copied into the runtime package's folder)
    Assert-PathNotExists (Join-Path $solutionDir packages\PackageWithStrongNamedLib.1.1\lib\ja-jp\Core.resources.dll)
    Assert-PathNotExists (Join-Path $solutionDir packages\PackageWithStrongNamedLib.1.1\lib\ja-jp\Core.xml)
}

function Test-UninstallSatellitePackageDoNotRemoveCollidingRuntimeFilesWhenContentsDiffer
{
    param(
        $context
    )

    # Arrange
    $p = New-ClassLibrary
    $solutionDir = Get-SolutionDir

    # Act
    $p | Install-Package PackageWithStrongNamedLib -Source $context.RepositoryPath
    $p | Install-Package PackageWithStrongNamedLib.ja-jp -Source $context.RepositoryPath

    $p | Uninstall-Package PackageWithStrongNamedLib.ja-jp

    # Assert (the resources from the satellite package are copied into the runtime package's folder)
    Assert-PathExists (Join-Path $solutionDir packages\PackageWithStrongNamedLib.1.1\lib\ja-jp\collision-differences.txt)
}

function Test-UninstallSatellitePackageDoRemoveCollidingRuntimeFilesWhenContentsMatch
{
    param(
        $context
    )

    # Arrange
    $p = New-ClassLibrary
    $solutionDir = Get-SolutionDir

    # Act
    $p | Install-Package PackageWithStrongNamedLib -Source $context.RepositoryPath
    $p | Install-Package PackageWithStrongNamedLib.ja-jp -Source $context.RepositoryPath

    $p | Uninstall-Package PackageWithStrongNamedLib.ja-jp

    # Assert (the resources from the satellite package are copied into the runtime package's folder)
    Assert-PathNotExists (Join-Path $solutionDir packages\PackageWithStrongNamedLib.1.1\lib\ja-jp\collision-match.txt)
}

function Test-UninstallSatelliteThenRuntimeRemoveCollidingRuntimeFilesWhenContentsDiffer
{
    param(
        $context
    )

    # Arrange
    $p = New-ClassLibrary
    $solutionDir = Get-SolutionDir

    # Act
    $p | Install-Package PackageWithStrongNamedLib -Source $context.RepositoryPath
    $p | Install-Package PackageWithStrongNamedLib.ja-jp -Source $context.RepositoryPath

    $p | Uninstall-Package PackageWithStrongNamedLib.ja-jp
    $p | Uninstall-Package PackageWithStrongNamedLib

    # Assert (the resources from the satellite package are copied into the runtime package's folder)
    Assert-PathNotExists (Join-Path $solutionDir packages\PackageWithStrongNamedLib.1.1\lib\ja-jp\collision-differences.txt)
}

function Test-WebSiteSimpleUninstall
{
    param(
        $context
    )

    # Arrange
    $p = New-Website

    # Act
    $p | Install-Package MyAwesomeLibrary -Source $context.RepositoryPath
    $p | Uninstall-Package MyAwesomeLibrary

    # Assert
    Assert-PathNotExists (Join-Path (Get-ProjectDir $p) "bin\AwesomeLibrary.dll.refresh")
}

function Test-UninstallPackageUseTargetFxPersistedInPackagesConfigToRemoveContentFiles
{
    [SkipTest('https://github.com/NuGet/Home/issues/11221')]
    param($context)

    # Arrange
    $p = New-ClassLibrary

    $p | Install-Package PackageA -Source $context.RepositoryPath

    Assert-Package $p 'packageA'
    Assert-Package $p 'packageB'

    Assert-NotNull (Get-ProjectItem $p testA4.txt)
    Assert-NotNull (Get-ProjectItem $p testB4.txt)

    # Act (change the target framework of the project to 3.5 and verifies that it still removes the content files correctly )

    $projectName = $p.Name
    $p.Properties.Item("TargetFrameworkMoniker").Value = '.NETFramework,Version=3.5'

    $p = Get-Project $projectName

    Uninstall-Package 'PackageA' -ProjectName $projectName -RemoveDependencies

    # Assert
    Assert-NoPackage $p 'PackageA'
    Assert-NoPackage $p 'PackageB'

    Assert-Null (Get-ProjectItem $p testA4.txt)
    Assert-Null (Get-ProjectItem $p testB4.txt)
}

function Test-UninstallPackageUseTargetFxPersistedInPackagesConfigToRemoveAssemblies
{
    [SkipTest('https://github.com/NuGet/Home/issues/11221')]
    param($context)

    # Arrange
    $p = New-ClassLibrary

    $p | Install-Package PackageA -Source $context.RepositoryPath

    Assert-Package $p 'packageA'
    Assert-Package $p 'packageB'

    Assert-Reference $p testA4
    Assert-Reference $p testB4

    # Act (change the target framework of the project to 3.5 and verifies that it still removes the assembly references correctly )

    $projectName = $p.Name
    $p.Properties.Item("TargetFrameworkMoniker").Value = '.NETFramework,Version=3.5'

    $p = Get-Project $projectName

    Uninstall-Package 'PackageA' -ProjectName $projectName -RemoveDependencies

    # Assert
    Assert-NoPackage $p 'PackageA'
    Assert-NoPackage $p 'PackageB'

    Assert-Null (Get-AssemblyReference $p testA4.dll)
    Assert-Null (Get-AssemblyReference $p testB4.dll)
}

function Test-UninstallPackageUseTargetFxPersistedInPackagesConfigToInvokeUninstallScript
{
    [SkipTest('https://github.com/NuGet/Home/issues/11221')]
    param($context)

    # Arrange
    $p = New-ClassLibrary

    $p | Install-Package PackageA -Source $context.RepositoryPath

    Assert-Package $p 'packageA'

    # Act (change the target framework of the project to 3.5 and verifies that it invokes the correct uninstall.ps1 file in 'net40' folder )

    $projectName = $p.Name
    $p.Properties.Item("TargetFrameworkMoniker").Value = '.NETFramework,Version=3.5'

    $global:UninstallVar = 0

    $p = Get-Project $projectName
    Uninstall-Package 'PackageA' -ProjectName $projectName

    # Assert
    Assert-NoPackage $p 'PackageA'

    Assert-AreEqual 1 $global:UninstallVar

    Remove-Variable UninstallVar -Scope Global
}


function Test-ToolsPathForUninstallScriptPointToToolsFolder
{
    param($context)

    # Arrange
    $p = New-ConsoleApplication

    $p | Install-Package PackageA -Version 1.0.0 -Source $context.RepositoryPath
    Assert-Package $p 'packageA'

    # Act

    $p | Uninstall-Package PackageA
}

function Test-FinishFailedUninstallOnSolutionOpenOfProjectLevelPackage
{
    param($context)

    # Arrange
    $p = New-ConsoleApplication

    $componentService = Get-VSComponentModel
	$solutionManager = $componentService.GetService([NuGet.PackageManagement.ISolutionManager])
	$setting = $componentService.GetService([NuGet.Configuration.ISettings])
	$packageFolderPath = [NuGet.PackageManagement.PackagesFolderPathUtility]::GetPackagesFolderPath($solutionManager, $setting)

    $p | Install-Package PackageWithTextFile -Version 1.0 -Source $context.RepositoryRoot

    # We will open a file handle preventing the deletion packages\PackageWithTextFile.1.0\content\text
    # causing the uninstall to fail to complete thereby forcing it to finish the next time the solution is opened
    $filePath = Join-Path $packageFolderPath "PackageWithTextFile.1.0\content\text"
    $fileStream = [System.IO.File]::Open($filePath, [System.IO.FileMode]::Open, [System.IO.FileAccess]::Read, [System.IO.FileShare]::Read)

    try {
        # Act
        $p | Uninstall-Package PackageWithTextFile

        # Assert
        Assert-True [NuGet.ProjectManagement.FileSystemUtility]::DirectoryExists("PackageWithTextFile.1.0.0.0")
        Assert-True [NuGet.ProjectManagement.FileSystemUtility]::FileExists("PackageWithTextFile.1.0.0.0.deleteme")

    } finally {
        $fileStream.Close()
    }

    # Act
    # After closing the file handle, we close the solution and reopen it
    $solutionDir = Get-SolutionFullName
    Close-Solution
    Open-Solution $solutionDir

    # Assert
    Assert-False [NuGet.ProjectManagement.FileSystemUtility]::DirectoryExists("PackageWithTextFile.1.0.0.0")
    Assert-False [NuGet.ProjectManagement.FileSystemUtility]::FileExists("PackageWithTextFile.1.0.0.0.deleteme")
}


function Test-UnInstallPackageWithXdtTransformUnTransformsTheFile
{
    # Arrange
    $p = New-WebApplication

    # Act
    $p | Install-Package XdtPackage -Source $context.RepositoryPath

    # Assert
    Assert-Package $p 'XdtPackage' '1.0.0'

    $content = [xml](Get-Content (Get-ProjectItemPath $p web.config))

    Assert-AreEqual "false" $content.configuration["system.web"].compilation.debug
    Assert-NotNull $content.configuration["system.web"].customErrors

    # Act 2
    $p | UnInstall-Package XdtPackage

    # Assert 2
    Assert-NoPackage $p 'XdtPackage' '1.0.0'

    $content = [xml](Get-Content (Get-ProjectItemPath $p web.config))

    Assert-AreEqual "true" $content.configuration["system.web"].compilation.debug
    Assert-Null $content.configuration["system.web"].customErrors
}

#function Test-UninstallPackageHonorPackageReferencesAccordingToProjectFramework
function UninstallPackageHonorPackageReferencesAccordingToProjectFramework
{
    param ($context)

    # Arrange
    $p = New-ClassLibrary

    $p | Install-Package mars -Source $repositoryPath
    $p | Install-Package natal -Source $repositoryPath

    Assert-Package $p mars
    Assert-Package $p natal

    # Act
    $p | Uninstall-Package natal

    # Assert
    Assert-Reference $p one
    Assert-Null (Get-AssemblyReference $p two)
    Assert-Null (Get-AssemblyReference $p three)
}

# This test has been disabled and is tracked by an issue
# function Test-UninstallPackageRemoveImportStatement
function UninstallPackageRemoveImportStatement
{
    param ($context)

    # Arrange
    $p = New-ConsoleApplication

    $p | Install-Package PackageWithImport -Source $context.RepositoryPath

    Assert-Package $p PackageWithImport
    Assert-ProjectImport $p "..\packages\PackageWithImport.2.0.0\build\PackageWithImport.targets"
    Assert-ProjectImport $p "..\packages\PackageWithImport.2.0.0\build\PackageWithImport.props"

    # Act
    $p | Uninstall-Package PackageWithImport

    Assert-NoPackage $p PackageWithImport
    Assert-NoProjectImport $p "..\packages\PackageWithImport.2.0.0\build\PackageWithImport.targets"
    Assert-NoProjectImport $p "..\packages\PackageWithImport.2.0.0\build\PackageWithImport.props"
}

function Test-UninstallPackageWithContentInLicenseBlocks
{
	param($context)

	# Arrange
	$p = New-ClassLibrary

	$name = 'PackageWithFooContentFile'

	Install-Package $name -Version 1.0 -Source $context.RepositoryRoot

	$packages = Get-PackagesDir
	$fooFilePath = Join-Path $packages "$name.1.0\content\foo"

	Assert-True (Test-Path $fooFilePath)

	'***************NUget: Begin License Text ---------dsafdsafdas
sdaflkjdsal;fj;ldsafdsa
dsaflkjdsa;lkfj;ldsafas
dsafdsafdsafsdaNuGet: End License Text-------------
From the package' > $fooFilePath

	# Act
	Uninstall-Package $name

	# Assert
	Assert-NoPackage $p $name
	Assert-Null (Get-ProjectItem $p 'foo')
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