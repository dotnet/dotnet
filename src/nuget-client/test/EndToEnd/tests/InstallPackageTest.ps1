# Verify Xunit 2.1.0 can be installed into a net45 project.
# https://github.com/NuGet/Home/issues/1711

function Test-InstallPackageWithInvalidAbsoluteLocalSource {
    # Arrange
    $package = "Rules"
    $project = New-ConsoleApplication
    $source = "c:\temp\data"
    $message = "Unable to find package '$package' at source '$source'. Source not found."

    # Act & Assert
    Assert-Throws { Install-Package $package -ProjectName $project.Name -source $source } $message
}

function Test-InstallPackageWithValidAbsoluteLocalSource {
    # Arrange
    $package = "Rules"
    $project = New-ConsoleApplication
    $source = pwd
    $message = "Unable to find package '$package'"

    # Act & Assert
    Assert-Throws { Install-Package $package -ProjectName $project.Name -source $source } $message
}

function Test-InstallPackageWithInvalidRelativeLocalSource {
    # Arrange
    $package = "Rules"
    $project = New-ConsoleApplication
    $source = "..\invalid_folder"
    $message = "Unable to find package '$package' at source '$source'. Source not found."

    # Act & Assert
    Assert-Throws { Install-Package $package -ProjectName $project.Name -source $source } $message
}

function Test-InstallPackageWithValidRelativeLocalSource {
    # Arrange
    $package = "Rules"
    $project = New-ConsoleApplication
    $source = "..\"
    $message = "Unable to find package '$package'"

    # Act & Assert
    Assert-Throws { Install-Package $package -ProjectName $project.Name -source $source } $message
}

function Test-InstallPackageWithInvalidHttpSource {
    # Arrange
    $package = "Rules"
    $project = New-ConsoleApplication
    $source = "http://example.com"
    $message = "Unable to find package '$package' at source '$source'."

    # Act & Assert
    Assert-Throws { Install-Package $package -ProjectName $project.Name -source $source } $message
}

function Test-InstallPackageWithInvalidHttpSourceVerbose {
    # Arrange
    $package = "Rules"
    $project = New-ConsoleApplication
    $source = "http://example.com"
    $escapedSource = [regex]::Escape($source)
    $escapedUrl = [regex]::Escape($source+"/FindPackagesById()?id='$package'&semVerLevel=2.0.0")
    $message = "\ \ GET\ $escapedUrl\ \ \ NotFound\ $escapedUrl\ [\w]+\ An\ error\ occurred\ while\ retrieving\ package\ metadata\ for\ '$package'\ from\ source\ '$escapedSource'\."
    # Act
    $result = Install-Package $package -ProjectName $project.Name -source $source -Verbose *>&1
    $resultString = [string]::Join(" ", $result)
    $compare = $resultString -Match $message
    $messageToPrint = "Result string is `n$resultString but expected message was `n$message"
    # Assert
    Assert-True $compare $messageToPrint
}

function Test-InstallPackageWithIncompleteHttpSource {
    # Arrange
    $package = "Rules"
    $project = New-ConsoleApplication
    $source = "http://"
    $message = "Unable to find package 'Rules' at source '$source'. Source not found."

    # Act & Assert
    Assert-Throws { Install-Package $package -ProjectName $project.Name -source $source } $message
}

function Test-InstallPackageWithInvalidKnownSource {
    # Arrange
    $package = "Rules"
    $project = New-ConsoleApplication
    $source = "nuget.random"
    $message = "Unable to find package 'Rules' at source '$source'. Source not found."

    # Act & Assert
    Assert-Throws { Install-Package $package -ProjectName $project.Name -source $source } $message
}

function Test-InstallPackageWithValidKnownSource {
    # Arrange
    $package = "Rules"
    $project = New-ConsoleApplication
    $source = "nuget.org"
    $message = "Unable to find package '$package'"

    # Act & Assert
    Assert-Throws { Install-Package $package -ProjectName $project.Name -source $source } $message
}

function Test-InstallPackageWithFtpProtocolSource {
    # Arrange
    $package = "Rules"
    $project = New-ConsoleApplication
    $source = "ftp://Rules"
    $message = "Unsupported type of source '$source'. Please provide an http or local source."

    # Act & Assert
    Assert-Throws { Install-Package $package -ProjectName $project.Name -source $source } $message
}

function Test-InstallXunit210WithEmptyBuildFolderSucceeds
{
    param($context)

    # Arrange
    $p = New-ClassLibrary

    # Act
    $p | Install-Package xunit -version 2.1.0

    # Assert
    Assert-Package $p xunit 2.1.0
    Assert-Package $p xunit.core 2.1.0
    Assert-Package $p xunit.assert 2.1.0
    Assert-Package $p xunit.extensibility.core 2.1.0
    Assert-Package $p xunit.extensibility.execution 2.1.0
    Assert-Package $p xunit.abstractions 2.0.0
}

function Test-SinglePackageInstallIntoSingleProject {
    # Arrange
    $project = New-ConsoleApplication

    # Act
    Install-Package FakeItEasy -ProjectName $project.Name -version 1.8.0

    # Assert
    Assert-Reference $project Castle.Core
    Assert-Reference $project FakeItEasy
    Assert-Package $project FakeItEasy
    Assert-Package $project Castle.Core
    Assert-SolutionPackage FakeItEasy
    Assert-SolutionPackage Castle.Core
}

# Test install-package -WhatIf to downgrade an installed package.
function Test-PackageInstallWithFileUri {
    # Arrange
    $project = New-ConsoleApplication

    $uri = $context.RepositoryRoot
    $uri = $uri.replace("\", "/")
    $uri = "file:///" + $uri

    # Act
    Install-Package TestUpdatePackage -Version 2.0.0.0 -Source $uri

    # Assert
    # that the installed package is not touched.
    Assert-Package $project TestUpdatePackage '2.0.0.0'
}

function Test-PackageInstallWhatIf {
    # Arrange
    $project = New-ConsoleApplication

    # Act
    Install-Package FakeItEasy -Project $project.Name -version 1.8.0 -WhatIf

    # Assert: no packages are installed
    Assert-Null (Get-ProjectPackage $project FakeItEasy)
}

# Test install-package -WhatIf to downgrade an installed package.
function Test-PackageInstallDowngradeWhatIf {
    # Arrange
    $project = New-ConsoleApplication

    Install-Package TestUpdatePackage -Version 2.0.0.0 -Source $context.RepositoryRoot
    Assert-Package $project TestUpdatePackage '2.0.0.0'

    # Act
    Install-Package TestUpdatePackage -Version 1.0.0.0 -Source $context.RepositoryRoot -WhatIf

    # Assert
    # that the installed package is not touched.
    Assert-Package $project TestUpdatePackage '2.0.0.0'
}

function Test-WebsiteSimpleInstall {
    param(
        $context
    )
    # Arrange
    $p = New-WebSite

    # Act
    Install-Package -Source $context.RepositoryPath -Project $p.Name MyAwesomeLibrary

    # Assert
    Assert-Package $p MyAwesomeLibrary
    Assert-SolutionPackage MyAwesomeLibrary

    $refreshFilePath = Join-Path (Get-ProjectDir $p) "bin\MyAwesomeLibrary.dll.refresh"
    $content = Get-Content $refreshFilePath

    Assert-AreEqual "..\packages\MyAwesomeLibrary.1.0\lib\net40\MyAwesomeLibrary.dll" $content
}

function Test-DiamondDependencies {
    param(
        $context
    )

    # Scenario:
    # D 1.0 -> B 1.0, C 1.0
    # B 1.0 -> A 1.0
    # C 1.0 -> A 2.0
    #     D 1.0
    #      /  \
    #  B 1.0   C 1.0
    #     |    |
    #  A 1.0   A 2.0

    # Arrange
    $packages = @("A", "B", "C", "D")
    $project = New-ClassLibrary

    # Act
    Install-Package D -Project $project.Name -Source $context.RepositoryPath

    # Assert
    $packages | %{ Assert-SolutionPackage $_ }
    $packages | %{ Assert-Package $project $_ }
    $packages | %{ Assert-Reference $project $_ }
    Assert-Package $project A 2.0
    Assert-Reference $project A 2.0.0.0
    Assert-Null (Get-ProjectPackage $project A 1.0.0.0)
    Assert-Null (Get-SolutionPackage A 1.0.0.0)
}

function Test-WebsiteWillNotDuplicateConfigOnReInstall {
    # Arrange
    $p = New-WebSite

    # Act
    Install-Package elmah -Project $p.Name -Version 1.1
    $item = Get-ProjectItem $p packages.config
    $item.Delete()
    Install-Package elmah -Project $p.Name -Version 1.1

    # Assert
    $config = [xml](Get-Content (Get-ProjectItemPath $p web.config))
    Assert-AreEqual 4 $config.configuration.configSections.sectionGroup.section.count
}

function Test-WebsiteConfigElementsAreRemovedEvenIfReordered {
    # Arrange
    $p = New-WebSite

    # Act
    Install-Package elmah -Project $p.Name -Version 1.1
    $configPath = Get-ProjectItemPath $p web.config
    $config = [xml](Get-Content $configPath)
    $sectionGroup = $config.configuration.configSections.sectionGroup
    $security = $sectionGroup.section[0]
    $sectionGroup.RemoveChild($security) | Out-Null
    $sectionGroup.AppendChild($security) | Out-Null
    $config.Save($configPath)
    Uninstall-Package elmah
    $config = [xml](Get-Content $configPath)

    # Assert
    Assert-Null $config.configuration.configSections
}

function Test-FailedInstallRollsBackInstall {
    param(
        $context
    )
    # Arrange
    $p = New-ClassLibrary

    # Act
    Install-Package haack.metaweblog -Project $p.Name -Source $context.RepositoryPath

    # Assert
    Assert-NotNull (Get-ProjectPackage $p haack.metaweblog 0.1.0)
    Assert-NotNull (Get-SolutionPackage haack.metaweblog 0.1.0)
}

function Test-PackageWithIncompatibleAssembliesRollsInstallBack {
    param(
        $context
    )
    # Arrange
    $p = New-WebApplication

    # Act & Assert
    Assert-Throws { Install-Package BingMapAppSDK -Project $p.Name -Source $context.RepositoryPath } "Could not install package 'BingMapAppSDK 1.0.1011.1716'. You are trying to install this package into a project that targets '.NETFramework,Version=v4.7.2', but the package does not contain any assembly references or content files that are compatible with that framework. For more information, contact the package author."
    Assert-Null (Get-ProjectPackage $p BingMapAppSDK 1.0.1011.1716)
    Assert-Null (Get-SolutionPackage BingMapAppSDK 1.0.1011.1716)
}

function Test-InstallPackageInvokeInstallScriptAndInitScript {
    param(
        $context
    )

    # Arrange
    $p = New-ConsoleApplication

    # Act
    Install-Package PackageWithScripts -Source $context.RepositoryRoot

    # Assert

    # This asserts init.ps1 gets called
    Assert-True (Test-Path function:\Get-World)
}

# TODO: We need to modify our console host to allow creating nested pipeline
#       in order for this test to run successfully.
#
#function Test-OpeningExistingSolutionInvokeInitScriptIfAny {
#    param(
#        $context
#    )
#
#    # Arrange
#    $p = New-ConsoleApplication
#
#    # Act
#    Install-Package PackageWithScripts -Source $context.RepositoryRoot
#
#    # Now close the solution and reopen it
#    $solutionDir = Get-SolutionFullName
#    Close-Solution
#    Remove-Item function:\Get-World
#    Assert-False (Test-Path function:\Get-World)
#
#    Open-Solution $solutionDir
#
#    # This asserts init.ps1 gets called
#    Assert-True (Test-Path function:\Get-World)
#}

function Test-InstallPackageResolvesDependenciesAcrossSources {
    param(
        $context
    )

    # Arrange
    $p = New-ConsoleApplication

    # Act
    # Ensure Antlr is not avilable in local repo.
    Assert-Null (Get-Package -ListAvailable -Source $context.RepositoryRoot Antlr)
    Install-Package PackageWithExternalDependency -Source $context.RepositoryRoot

    # Assert
    Assert-Package $p PackageWithExternalDependency
    Assert-Package $p Antlr
}

function Test-VariablesPassedToInstallScriptsAreValidWithWebSite {
    param(
        $context
    )

    # Arrange
    $p = New-WebSite

    # Act
    Install-Package PackageWithScripts -Project $p.Name -Source $context.RepositoryRoot

    # Assert

    # This asserts install.ps1 gets called with the correct project reference and package
    Assert-Reference $p System.Windows.Forms
}

function Test-InstallComplexPackageStructure {
    param(
        $context
    )

    # Arrange
    $p = New-WebApplication

    # Act
    Install-Package MyFirstPackage -Project $p.Name -Source $context.RepositoryPath

    # Assert
    Assert-NotNull (Get-ProjectItem $p Pages\Blocks\Help\Security)
    Assert-NotNull (Get-ProjectItem $p Pages\Blocks\Security\App_LocalResources)
}

function Test-InstallPackageWithWebConfigDebugChanges {
    param(
        $context
    )

    # Arrange
    $p = New-WebApplication

    # Act
    Install-Package PackageWithWebDebugConfig -Project $p.Name -Source $context.RepositoryRoot

    # Assert
    $configItem = Get-ProjectItem $p web.config
    $configDebugItem = $configItem.ProjectItems.Item("web.debug.config")
    $configDebugPath = $configDebugItem.Properties.Item("FullPath").Value
    $configDebug = [xml](Get-Content $configDebugPath)
    Assert-NotNull $configDebug
    Assert-NotNull ($configDebug.configuration.connectionStrings.add)
    $addNode = $configDebug.configuration.connectionStrings.add
    Assert-AreEqual MyDB $addNode.name
    Assert-AreEqual "Data Source=ReleaseSQLServer;Initial Catalog=MyReleaseDB;Integrated Security=True" $addNode.connectionString
}

function Test-WebsiteInstallPackageWithRootNamespace {
    param(
        $context
    )

    # Arrange
    $p = New-WebSite

    # Act
    Install-Package PackageWithRootNamespaceFileTransform -Source $context.RepositoryRoot

    # Assert
    Assert-NotNull (Get-ProjectItem $p App_Code\foo.cs)
    $path = (Get-ProjectItemPath $p App_Code\foo.cs)
    $content = [System.IO.File]::ReadAllText($path)
    Assert-True ($content.Contains("namespace ASP"))
}

function Test-AddBindingRedirectToWebsiteWithNonExistingOutputPath {
    # Arrange
    $p = New-WebSite

    # Act
    Add-BindingRedirect -ProjectName $p.Name

    # Assert
    Assert-Null $redirects
}

function Test-InstallCanPipeToFSharpProjects {
    [SkipTest('https://github.com/NuGet/Home/issues/11982')]
    param($context)
    # Arrange
    $p = New-FSharpLibrary
    Build-Solution # wait for project nomination

    # Act
    $p | Install-Package elmah -Version 1.1 -source $context.RepositoryPath

    # Assert
    Assert-NetCorePackageInLockFile $p elmah 1.1
}

function Test-PipingMultipleProjectsToInstall {
    # Arrange
    $projects = @((New-WebSite), (New-ClassLibrary), (New-WebApplication))

Write-Host 'proejct creation successful'
    # Act
    $projects | Install-Package elmah

Write-Host 'Installation successful'
    # Assert
    $projects | %{ Assert-Package $_ elmah }

Write-Host 'Assertion successful'
}

function Test-InstallPackageWithNestedContentFile {
    [SkipTest('https://github.com/NuGet/Home/issues/8486')]
    param(
        $context
    )
    # Arrange
    $p = New-WpfApplication

    # Act
    Install-Package PackageWithNestedFile -Source $context.RepositoryRoot

    $item = Get-ProjectItem $p TestMainWindow.xaml
    Assert-NotNull $item
    Assert-NotNull $item.ProjectItems.Item("TestMainWindow.xaml.cs")
    Assert-Package $p PackageWithNestedFile 1.0
    Assert-SolutionPackage PackageWithNestedFile 1.0
}

function Test-InstallPackageWithNestedAspxContentFiles {
    param(
        $context
    )
    # Arrange
    $p = New-WebApplication

    $files = @('Global.asax', 'Site.master', 'About.aspx')

    # Act
    Install-Package PackageWithNestedAspxFiles -Source $context.RepositoryRoot

    # Assert
    $files | %{
        $item = Get-ProjectItem $p $_
        Assert-NotNull $item
        Assert-NotNull $item.ProjectItems.Item("$_.cs")
    }

    Assert-Package $p PackageWithNestedAspxFiles 1.0
    Assert-SolutionPackage PackageWithNestedAspxFiles 1.0
}

function Test-InstallPackageWithNestedReferences {
    param(
        $context
    )

    # Arrange
    $p = New-WebApplication

    # Act
    Install-Package PackageWithNestedReferenceFolders -Source $context.RepositoryRoot

    # Assert
    Assert-Reference $p Ninject
    Assert-Reference $p CommonServiceLocator.NinjectAdapter
}

# In NuGet 3.0, adding references from sub folders is supported.
# For example, lib\x86\assembly.dll needs to be added, and x86 is not a valid framework.
function Test-InstallPackageWithUnsupportedReference {
    param(
        $context
    )

    # Arrange
    $p = New-ClassLibrary

    # Act
    Install-Package PackageWithUnsupportedReferences -Source $context.RepositoryRoot

    # Assert
    Assert-NotNull (Get-ProjectPackage $p PackageWithUnsupportedReferences)
    Assert-NotNull (Get-SolutionPackage PackageWithUnsupportedReferences)
}

function Test-InstallPackageWithExeReference {
    param(
        $context
    )

    # Arrange
    $p = New-ClassLibrary

    # Act
    Install-Package PackageWithExeReference -Source $context.RepositoryRoot

    # Assert
    Assert-Reference $p NuGet
}

function Test-InstallPackageWithResourceAssemblies {
    param(
        $context
    )

    # Arrange
    $p = New-ClassLibrary

    # Act
    Install-Package FluentValidation -Source $context.RepositoryPath

    # Assert
    Assert-Reference $p FluentValidation
    Assert-Null (Get-AssemblyReference $p FluentValidation.resources)
}

function Test-InstallPackageWithGacReferencesIntoMultipleProjectTypes {
    param(
        $context
    )

    # Arrange
    $a = New-ClassLibrary
    $b = New-WebSite
    $projects = @($a, $b)

    # Act
    $a | Install-Package PackageWithGacReferences -Source $context.RepositoryRoot
    $b | Install-Package PackageWithGacReferences -Source $context.RepositoryRoot

    # Assert
    $projects | %{ Assert-Reference $_ System.Web }
}

<#
function Test-PackageWithClientProfileAndFullFrameworkPicksClient {
    param(
        $context
    )

    # Arrange
    $p = New-ConsoleApplication

    # Arrange
    $p | Install-Package MyAwesomeLibrary -Source $context.RepositoryPath

    # Assert
    Assert-Reference $p MyAwesomeLibrary
    $reference = Get-AssemblyReference $p MyAwesomeLibrary
    Assert-True ($reference.Path.Contains("net40-client") -or ($reference.Path.Contains("net4-client")))
}
#>

function Test-InstallPackageWithNonExistentFrameworkReferences {
    param(
        $context
    )

    # Arrange
    $p = New-ClassLibrary
    $expectedExceptionMessage = "Failed to add reference. The package 'PackageWithNonExistentGacReferences' tried to add a framework reference to 'System.Awesome' which was not found in the GAC. This is possibly a bug in the package. Please contact the package owners for assistance."
    $actualExceptionMessage = [string]::Empty

    # Arrange
    try {
        $p | Install-Package PackageWithNonExistentGacReferences -Source $context.RepositoryRoot
    }
    catch {
        $actualExceptionMessage = $_.Exception.Message
    }

    Assert-Contains $expectedExceptionMessage $actualExceptionMessage
}

function Test-InstallPackageWithFrameworkFacadeReference {
    [SkipTest("https://github.com/NuGet/Home/issues/11221")]
    param($context)

    # Arrange
    $p = New-ClassLibrary

    Install-Package PackageWithFrameworkFacadeReference -Source $context.RepositoryRoot

    Assert-Null (Get-AssemblyReference $p 'System.Runtime')
}

function Test-InstallPackageWorksWithProjectsHavingSameNames {

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
    $p2 = New-ClassLibrary 'ProjectB'  'Folder1'

    New-SolutionFolder 'Folder2'
    $p3 = New-ClassLibrary 'ProjectA' 'Folder2'
    $p4 = New-ConsoleApplication 'ProjectC' 'Folder2'

    $p5 = New-ConsoleApplication 'ProjectA'

    # Act
    Get-Project -All | Install-Package elmah -Version 1.1

    # Assert
    $all = @( $p1, $p2, $p3, $p4, $p5 )

    $all | % { Assert-Package $_ elmah }
}

function Test-SimpleBindingRedirects {
    param(
        $context
    )
    # Arrange
    $a = New-WebApplication
    $b = New-WebSite

    $projects = @($a, $b)

    # Act
    $a | Install-Package B -Version 2.0 -Source $context.RepositoryPath
    $b | Install-Package B -Version 2.0 -Source $context.RepositoryPath
    $a | Install-Package A -Version 1.0 -Source $context.RepositoryPath
    $b | Install-Package A -Version 1.0 -Source $context.RepositoryPath

    # Assert
    $projects | %{ Assert-Reference $_ A 1.0.0.0;
                   Assert-Reference $_ B 2.0.0.0; }

    Assert-BindingRedirect $a web.config B '0.0.0.0-2.0.0.0' '2.0.0.0'
    Assert-BindingRedirect $b web.config B '0.0.0.0-2.0.0.0' '2.0.0.0'
}

# Tests that when there are multip config files, the binding redirects will
# be added to the right file.
function Test-SimpleBindingRedirectsMultipleConfigs {
    param(
        $context
    )
    # Arrange
    $a = New-WebApplication

    # Add a another web.config under directory test
    $testDirectory = Join-Path (Get-ProjectDir $a) "test"
    $file = Join-Path $testDirectory "web.config"
    New-Item $testDirectory -ItemType Directory
    "<configuration></configuration>" > $file
    $a.ProjectItems.AddFromFile($file)

    # Act
    $a | Install-Package B -Version 2.0 -Source $context.RepositoryPath
    $a | Install-Package A -Version 1.0 -Source $context.RepositoryPath

    # Assert
    # the binding redirect should be added to web.config directly under the project directory,
    # not to file test\web.config.
    Assert-BindingRedirect $a web.config B '0.0.0.0-2.0.0.0' '2.0.0.0'
}

function Test-SimpleBindingRedirectsClassLibraryUpdatePackage {
    # Arrange
    $a = New-ClassLibrary

    # Act
    $a | Install-Package E -Source $context.RepositoryPath

    # Assert
    Assert-Package $a E
    Assert-Reference $a E 1.0.0.0
    Assert-Reference $a F 1.0.0.0
    Assert-Null (Get-ProjectItem $a app.config)

    $a | Update-Package F -Safe -Source $context.RepositoryPath

    Assert-NotNull (Get-ProjectItem $a app.config)
    Assert-Reference $a F 1.0.5.0
    Assert-BindingRedirect $a app.config F '0.0.0.0-1.0.5.0' '1.0.5.0'
}

function Test-SimpleBindingRedirectsClassLibraryReference {
    param(
        $context
    )
    # Arrange
    $a = New-WebApplication
    $b = New-WebSite
    $d = New-ClassLibraryNET46
    $e = New-ClassLibraryNET46

    Add-ProjectReference $a $d
    Add-ProjectReference $b $e

    # Act
    $d | Install-Package E -Source $context.RepositoryPath
    $e | Install-Package E -Source $context.RepositoryPath
    $d | Update-Package F -Safe -Source $context.RepositoryPath
    $e | Update-Package F -Safe -Source $context.RepositoryPath

    # Assert
    Assert-Package $d E
    Assert-Package $e E
    Assert-Reference $d E 1.0.0.0
    Assert-Reference $e E 1.0.0.0
    Assert-BindingRedirect $a web.config F '0.0.0.0-1.0.5.0' '1.0.5.0'
    Assert-BindingRedirect $b web.config F '0.0.0.0-1.0.5.0' '1.0.5.0'
    Assert-BindingRedirect $d app.config F '0.0.0.0-1.0.5.0' '1.0.5.0'
    Assert-BindingRedirect $e app.config F '0.0.0.0-1.0.5.0' '1.0.5.0'
    Assert-Null (Get-ProjectItem $d web.config)
    Assert-Null (Get-ProjectItem $e web.config)
}

function Test-SimpleBindingRedirectsIndirectReference {
    param($context)

    # Arrange
    $a = New-WebApplication
    $b = New-ClassLibraryNET46
    $c = New-ClassLibraryNET46

    Add-ProjectReference $a $b
    Add-ProjectReference $b $c

    # Act
    $c | Install-Package E -Source $context.RepositoryPath
    $c | Update-Package F -Safe -Source $context.RepositoryPath

    # Assert
    Assert-Null (Get-ProjectItem $b web.config)
    Assert-Null (Get-ProjectItem $c web.config)
    Assert-BindingRedirect $a web.config F '0.0.0.0-1.0.5.0' '1.0.5.0'
    Assert-BindingRedirect $b app.config F '0.0.0.0-1.0.5.0' '1.0.5.0'
    Assert-BindingRedirect $c app.config F '0.0.0.0-1.0.5.0' '1.0.5.0'
}

function Test-SimpleBindingRedirectsNonWeb {
    [SkipTest('https://github.com/NuGet/Home/issues/8402')]
    param($context)

    # Arrange
    $a = New-ConsoleApplication
    $b = New-WPFApplication
    $projects = @($a, $b)

    # Act
    #$projects | Install-Package E -Source $context.RepositoryPath
    $a | Install-Package E -Source $context.RepositoryPath
    $b | Install-Package E -Source $context.RepositoryPath
    $a | Update-Package F -Safe -Source $context.RepositoryPath
    $b | Update-Package F -Safe -Source $context.RepositoryPath

    # Assert
    $projects | %{ Assert-Package $_ E;
                   Assert-BindingRedirect $_ app.config F '0.0.0.0-1.0.5.0' '1.0.5.0' }
}

function Test-BindingRedirectComplex {
    param($context)

    # Arrange
    $a = New-WebApplication
    $b = New-ConsoleApplication
    $c = New-ClassLibraryNET46

    Add-ProjectReference $a $b
    Add-ProjectReference $b $c

    $projects = @($a, $b)

    # Act
    $c | Install-Package E -Source $context.RepositoryPath
    $c | Update-Package F -Safe -Source $context.RepositoryPath

    Assert-Package $c E;

    # Assert
    Assert-BindingRedirect $a web.config F '0.0.0.0-1.0.5.0' '1.0.5.0'
    Assert-BindingRedirect $b app.config F '0.0.0.0-1.0.5.0' '1.0.5.0'
}

function Test-SimpleBindingRedirectsWebsite {
    param(
        $context
    )
    # Arrange
    $a = New-WebSite

    # Act
    $a | Install-Package E -Source $context.RepositoryPath
    $a | Update-Package F -Safe -Source $context.RepositoryPath

    # Assert
    Assert-Package $a E;
    Assert-BindingRedirect $a web.config F '0.0.0.0-1.0.5.0' '1.0.5.0'
}


function Test-BindingRedirectInstallLargeProject {
    param($context)

    $numProjects = 25
    $projects = 0..$numProjects | %{ New-ClassLibraryNET46 $_ }
    $p = New-WebApplication

    for($i = 0; $i -lt $numProjects; $i++) {
        Add-ProjectReference $projects[$i] $projects[$i+1]
    }

    Add-ProjectReference $p $projects[0]

    $projects[$projects.Length - 1] | Install-Package E -Source $context.RepositoryPath
    $projects[$projects.Length - 1] | Update-Package F -Safe -Source $context.RepositoryPath
    Assert-BindingRedirect $p web.config F '0.0.0.0-1.0.5.0' '1.0.5.0'
}

function Test-BindingRedirectDuplicateReferences {
    param($context)

    # Arrange
    $a = New-WebApplication
    $b = New-ConsoleApplication
    $c = New-ClassLibraryNET46

    ($a, $b) | Install-Package A -Source $context.RepositoryPath -IgnoreDependencies

    Add-ProjectReference $a $b
    Add-ProjectReference $b $c

    # Act
    $c | Install-Package E -Source $context.RepositoryPath
    $c | Update-Package F -Safe -Source $context.RepositoryPath

    Assert-Package $c E

    # Assert
    Assert-BindingRedirect $a web.config F '0.0.0.0-1.0.5.0' '1.0.5.0'
    Assert-BindingRedirect $b app.config F '0.0.0.0-1.0.5.0' '1.0.5.0'
}

function Test-BindingRedirectClassLibraryWithDifferentDependents {
    param($context)

    # Arrange
    $a = New-WebApplication
    $b = New-ConsoleApplication
    $c = New-ClassLibraryNET46

    ($a, $b) | Install-Package A -Source $context.RepositoryPath -IgnoreDependencies

    Add-ProjectReference $a $c
    Add-ProjectReference $b $c

    # Act
    $c | Install-Package E -Source $context.RepositoryPath
    $c | Update-Package F -Safe -Source $context.RepositoryPath

    Assert-Package $c E

    # Assert
    Assert-BindingRedirect $a web.config F '0.0.0.0-1.0.5.0' '1.0.5.0'
    Assert-BindingRedirect $b app.config F '0.0.0.0-1.0.5.0' '1.0.5.0'
}

function Test-BindingRedirectProjectsThatReferenceSameAssemblyFromDifferentLocations {
    param($context)

    # Arrange
    $a = New-WebApplication
    $b = New-ConsoleApplication
    $c = New-ClassLibraryNET46

    $a | Install-Package A -Source $context.RepositoryPath -IgnoreDependencies
    $aPath = ls (Get-SolutionDir) -Recurse -Filter A.dll
    cp $aPath.FullName (Get-SolutionDir)
    $aNewLocation = Join-Path (Get-SolutionDir) A.dll

    $b.Object.References.Add($aNewLocation)

    Add-ProjectReference $a $b
    Add-ProjectReference $b $c

    # Act
    $c | Install-Package E -Source $context.RepositoryPath
    $c | Update-Package F -Safe -Source $context.RepositoryPath

    Assert-Package $c E

    # Assert
    Assert-BindingRedirect $a web.config F '0.0.0.0-1.0.5.0' '1.0.5.0'
    Assert-BindingRedirect $b app.config F '0.0.0.0-1.0.5.0' '1.0.5.0'
}

function Test-BindingRedirectsMixNonStrongNameAndStrongNameAssemblies {
    param(
        $context
    )
    # Arrange
    $a = New-ConsoleApplication

    # Act
    $a | Install-Package PackageWithNonStrongNamedLibA -Source $context.RepositoryRoot
    $a | Install-Package PackageWithNonStrongNamedLibB -Source $context.RepositoryRoot

    # Assert
    Assert-Package $a PackageWithNonStrongNamedLibA
    Assert-Package $a PackageWithNonStrongNamedLibA
    Assert-Package $a PackageWithStrongNamedLib 1.1
    Assert-Reference $a A 1.0.0.0
    Assert-Reference $a B 1.0.0.0
    Assert-Reference $a Core 1.1.0.0

    Assert-BindingRedirect $a app.config Core '0.0.0.0-1.1.0.0' '1.1.0.0'
}

function Test-BindingRedirectProjectsThatReferenceDifferentVersionsOfSameAssembly {
    param($context)

    # Arrange
    $a = New-WebApplication
    $b = New-ConsoleApplication
    $c = New-ClassLibraryNET46

    $a | Install-Package A -Source $context.RepositoryPath -IgnoreDependencies
    $b | Install-Package A -Version 1.0 -Source $context.RepositoryPath -IgnoreDependencies

    Add-ProjectReference $a $b
    Add-ProjectReference $b $c

    # Act
    $c | Install-Package E -Source $context.RepositoryPath
    $c | Update-Package F -Safe -Source $context.RepositoryPath

    Assert-Package $c E

    # Assert
    Assert-BindingRedirect $a web.config F '0.0.0.0-1.0.5.0' '1.0.5.0'
    Assert-BindingRedirect $b app.config F '0.0.0.0-1.0.5.0' '1.0.5.0'
}

# Tests the case when Skip is specified in nuget.config under bindingRedirects section
function Test-InstallPackageSkipsBindingRedirectWhenSetOnConfig
{
    param(
        $context
    )

    # Arrange
    Check-NuGetConfig

    $componentModel = Get-VSComponentModel
    $setting = $componentModel.GetService([NuGet.Configuration.ISettings])

    $a = New-WebSite

    try
    {
        # Act
        $o = [NuGet.Configuration.AddItem]::new('skip', 'true')
        $setting.AddOrUpdate('bindingRedirects', $o)

        $a | Install-Package E -Source $context.RepositoryPath
        $a | Update-Package F -Safe -Source $context.RepositoryPath

        # Assert
        Assert-Package $a E;
        Assert-NoBindingRedirect $a web.config F '0.0.0.0-1.0.5.0' '1.0.5.0'
    }
    finally {
        $section = $setting.GetSection('bindingRedirects')

        ForEach ($item in $section.Items) {
            $setting.Remove('bindingRedirects', $item)
        }
    }
}

# Tests the case when Skip is specified in nuget.config under bindingRedirects section
function InstallPackageThrowWithLockedConfigFileIfSuccessRequired
{
    param(
        $context
    )

    # Arrange
    Check-NuGetConfig

    $componentModel = Get-VSComponentModel
    $setting = $componentModel.GetService([NuGet.Configuration.ISettings])

    $a = New-WebSite
    $webConfigPath = (Get-ProjectItemPath $a web.config)
    $projectDir = split-path $webConfigPath

    try
    {
        # Act
        $o = [NuGet.Configuration.AddItem]::new('successRequired', 'true')
        $setting.AddOrUpdate('bindingRedirects', $o)

        $a | Install-Package E -Source $context.RepositoryPath
        $stream = [System.IO.File]::Open($webconfigPath, [System.IO.FileMode]::Open, [System.IO.FileAccess]::Read, [System.IO.FileShare]::None)
        $stream.Lock(0, 10)

        # Assert
        $expectedErrorMessage = "Failed to update binding redirects for $projectDir : Failed to load '$webConfigPath', while updating binding redirects. The process cannot access the file '$webConfigPath' because it is being used by another process."
        Assert-Throws { $a | Update-Package F -Safe -Source $context.RepositoryPath } $expectedErrorMessage
    }
    finally
    {
        $section = $setting.GetSection('bindingRedirects')

        ForEach ($item in $section.Items) {
            $setting.Remove('bindingRedirects', $item)
        }
    }
}

# Tests the case when Skip is specified in nuget.config under bindingRedirects section
function Test-InstallPackageSucceedsWithLockedFileAndSkipsBindingRedirectSet
{
    param(
        $context
    )

    # Arrange
    Check-NuGetConfig

    $componentModel = Get-VSComponentModel
    $setting = $componentModel.GetService([NuGet.Configuration.ISettings])

    $a = New-WebSite
    $webConfigPath = (Get-ProjectItemPath $a web.config)

    try
    {
        # Act
        $o = [NuGet.Configuration.AddItem]::new('skip', 'True')
        $setting.AddOrUpdate('bindingRedirects', $o)

        $o = [NuGet.Configuration.AddItem]::new('successRequired', 'False')
        $setting.AddOrUpdate('bindingRedirects', $o)


        $a | Install-Package E -Source $context.RepositoryPath
        $stream = [System.IO.File]::Open($webconfigPath, [System.IO.FileMode]::Open, [System.IO.FileAccess]::Read, [System.IO.FileShare]::None)
        $stream.Lock(0, 10)

        $a | Update-Package F -Safe -Source $context.RepositoryPath

        # Assert
        Assert-Package $a E;
        # Unlock and close the stream of Web.Config so that it can be examined.
        $stream.Unlock(0, 10)
        $stream.Close()
        Assert-NoBindingRedirect $a web.config F '0.0.0.0-1.0.5.0' '1.0.5.0'
    }
    finally {
        $section = $setting.GetSection('bindingRedirects')

        ForEach ($item in $section.Items) {
            $setting.Remove('bindingRedirects', $item)
        }
    }
}

function Test-InstallingPackageDoesNotOverwriteFileIfExistsOnDiskButNotInProject {
    param(
        $context
    )

    # Arrange
    $p = New-WebApplication
    $projectPath = Get-ProjectDir $p
    $fooPath = Join-Path $projectPath foo
    "file content" > $fooPath

    # Act
    $p | Install-Package PackageWithFooContentFile -Source $context.RepositoryRoot

    Assert-Null (Get-ProjectItem $p foo) "foo exists in the project!"
    Assert-AreEqual "file content" (Get-Content $fooPath)
}

function Test-InstallPackageWithXmlTransformAndTokenReplacement {
    param(
        $context
    )

    # Arrange
    $p = New-WebApplication

    # Act
    $p | Install-Package PackageWithXmlTransformAndTokenReplacement -Source $context.RepositoryRoot

    # Assert
    $ns = $p.Properties.Item("DefaultNamespace").Value
    $assemblyName = $p.Properties.Item("AssemblyName").Value
    $path = (Get-ProjectItemPath $p web.config)
    $content = [System.IO.File]::ReadAllText($path)
    $expectedContent = "type=`"$ns.MyModule, $assemblyName`""
    Assert-True ($content.Contains($expectedContent))
}

function Test-InstallPackageAfterRenaming {
    param(
        $context
    )
    # Arrange
    New-SolutionFolder 'Folder1'
    New-SolutionFolder 'Folder1\Folder2'
    $p1 = New-ClassLibrary 'ProjectA' 'Folder1\Folder2'
    $p2 = New-ClassLibrary 'ProjectB' 'Folder1\Folder2'

    # Act
    $p1.Name = "ProjectX"
    Install-Package jquery -Version 1.5 -Source $context.RepositoryPath -project "Folder1\Folder2\ProjectX"

    Rename-SolutionFolder "Folder1\Folder2" "Folder3"
    Install-Package jquery -Version 1.5 -Source $context.RepositoryPath -project "Folder1\Folder3\ProjectB"

    # Assert
    Assert-NotNull (Get-ProjectItem $p1 scripts\jquery-1.5.js)
    Assert-NotNull (Get-ProjectItem $p2 scripts\jquery-1.5.js)
}

function Test-InstallingPackageWithDependencyThatFailsShouldRollbackSuccessfully {
    param(
        $context
    )
    # Arrange
    $p = New-WebApplication

    # Act
    Assert-Throws { $p | Install-Package GoodPackageWithBadDependency -Source $context.RepositoryPath } "NOT #WINNING"

    Assert-Null (Get-ProjectPackage $p GoodPackageWithBadDependency)
    Assert-Null (Get-SolutionPackage GoodPackageWithBadDependency)
    Assert-Null (Get-ProjectPackage $p PackageWithBadDependency)
    Assert-Null (Get-SolutionPackage PackageWithBadDependency)
    Assert-Null (Get-ProjectPackage $p PackageWithBadInstallScript)
    Assert-Null (Get-SolutionPackage PackageWithBadInstallScript)
}

function Test-WebsiteInstallPackageWithPPCSSourceFiles {
    param(
        $context
    )
    # Arrange
    $p = New-WebSite

    # Act
    $p | Install-Package PackageWithPPCSSourceFiles -Source $context.RepositoryRoot

    # Assert
    Assert-Package $p PackageWithPPCSSourceFiles
    Assert-SolutionPackage PackageWithPPCSSourceFiles
    Assert-NotNull (Get-ProjectItem $p App_Code\Foo.cs)
    Assert-NotNull (Get-ProjectItem $p App_Code\Bar.cs)
}

function Test-WebsiteInstallPackageWithPPVBSourceFiles {
    param(
        $context
    )
    # Arrange
    $p = New-WebSite

    # Act
    $p | Install-Package PackageWithPPVBSourceFiles -Source $context.RepositoryRoot

    # Assert
    Assert-Package $p PackageWithPPVBSourceFiles
    Assert-SolutionPackage PackageWithPPVBSourceFiles
    Assert-NotNull (Get-ProjectItem $p App_Code\Foo.vb)
    Assert-NotNull (Get-ProjectItem $p App_Code\Bar.vb)
}

function Test-WebsiteInstallPackageWithCSSourceFiles {
    param(
        $context
    )
    # Arrange
    $p = New-WebSite

    # Act
    $p | Install-Package PackageWithCSSourceFiles -Source $context.RepositoryRoot

    # Assert
    Assert-Package $p PackageWithCSSourceFiles
    Assert-SolutionPackage PackageWithCSSourceFiles
    Assert-NotNull (Get-ProjectItem $p App_Code\Foo.cs)
    Assert-NotNull (Get-ProjectItem $p App_Code\Bar.cs)
}

function Test-WebsiteInstallPackageWithVBSourceFiles {
    param(
        $context
    )
    # Arrange
    $p = New-WebSite

    # Act
    $p | Install-Package PackageWithVBSourceFiles -Source $context.RepositoryRoot

    # Assert
    Assert-Package $p PackageWithVBSourceFiles
    Assert-SolutionPackage PackageWithVBSourceFiles
    Assert-NotNull (Get-ProjectItem $p App_Code\Foo.vb)
    Assert-NotNull (Get-ProjectItem $p App_Code\Bar.vb)
}

function Test-WebsiteInstallPackageWithSourceFileUnderAppCode {
    param(
        $context
    )
    # Arrange
    $p = New-WebSite

    # Act
    $p | Install-Package PackageWithSourceFileUnderAppCode -Source $context.RepositoryRoot

    # Assert
    Assert-Package $p PackageWithSourceFileUnderAppCode
    Assert-SolutionPackage PackageWithSourceFileUnderAppCode
    Assert-NotNull (Get-ProjectItem $p App_Code\Class1.cs)
}

function Test-WebSiteInstallPackageWithNestedSourceFiles {
    param(
        $context
    )
    # Arrange
    $p = New-WebSite

    # Act
    $p | Install-Package netfx-Guard -Source $context.RepositoryRoot

    # Assert
    Assert-Package $p netfx-Guard
    Assert-SolutionPackage netfx-Guard
    Assert-NotNull (Get-ProjectItem $p App_Code\netfx\System\Guard.cs)
}

function Test-WebSiteInstallPackageWithFileNamedAppCode {
    param(
        $context
    )
    # Arrange
    $p = New-WebSite

    # Act
    $p | Install-Package PackageWithFileNamedAppCode -Source $context.RepositoryRoot

    # Assert
    Assert-Package $p PackageWithFileNamedAppCode
    Assert-SolutionPackage PackageWithFileNamedAppCode
    Assert-NotNull (Get-ProjectItem $p App_Code\App_Code.cs)
}

function Test-PackageInstallAcceptsSourceName {
    # Arrange
    $project = New-ConsoleApplication

    # Act
    Install-Package FakeItEasy -Project $project.Name -Source $SourceNuGet -Version 1.8.0

    # Assert
    Assert-Reference $project Castle.Core
    Assert-Reference $project FakeItEasy
    Assert-Package $project FakeItEasy
    Assert-Package $project Castle.Core
    Assert-SolutionPackage FakeItEasy
    Assert-SolutionPackage Castle.Core
}

function PackageInstallAcceptsAllAsSourceName {
    # Arrange
    $project = New-ConsoleApplication

    # Act
    Install-Package FakeItEasy -Project $project.Name -Source 'All' -Version 1.8.0

    # Assert
    Assert-Reference $project Castle.Core
    Assert-Reference $project FakeItEasy
    Assert-Package $project FakeItEasy
    Assert-Package $project Castle.Core
    Assert-SolutionPackage FakeItEasy
    Assert-SolutionPackage Castle.Core
}

function Test-PackageWithNoVersionInFolderName {
    param(
        $context
    )
    # Arrange
    $p = New-ClassLibrary

    # Act
    $p | Install-Package PackageWithNoVersionInFolderName -Source $context.RepositoryRoot

    # Assert
    Assert-Package $p PackageWithNoVersionInFolderName
    Assert-SolutionPackage PackageWithNoVersionInFolderName
    Assert-Reference $p A
}

function Test-PackageInstallAcceptsRelativePathSource {
    param(
        $context
    )

    pushd

    # Arrange
    $project = New-ConsoleApplication

    # Act
    cd $context.TestRoot
    Assert-AreEqual $context.TestRoot $pwd

    Install-Package PackageWithExeReference -Project $project.Name -Source '..\'

    # Assert
    Assert-Reference $project NuGet
    Assert-Package $project PackageWithExeReference

    popd
}

function Test-PackageInstallAcceptsRelativePathSource2 {
    param(
        $context
    )

    pushd

    # Arrange
    $repositoryRoot = $context.RepositoryRoot
    $parentOfRoot = Split-Path $repositoryRoot
    $relativePath = Split-Path $repositoryRoot -Leaf

    $project = New-ConsoleApplication

    # Act
    cd $parentOfRoot
    Assert-AreEqual $parentOfRoot $pwd
    Install-Package PackageWithExeReference -Project $project.Name -Source $relativePath

    # Assert
    Assert-Reference $project NuGet

    Assert-Package $project PackageWithExeReference

    popd
}

function Test-InstallPackageTargetingNetClientAndNet {
    param(
        $context
    )
    # Arrange
    $p = New-WebApplication

    # Act
    $p | Install-Package PackageTargetingNetClientAndNet -Source $context.RepositoryRoot

    # Assert
    Assert-Package $p PackageTargetingNetClientAndNet
    Assert-SolutionPackage PackageTargetingNetClientAndNet
    $reference = Get-AssemblyReference $p ClassLibrary1
    Assert-NotNull $reference
    Assert-True ($reference.Path.Contains("net40-client"))
}

function Test-InstallWithFailingInitPs1RollsBack {
    param(
        $context
    )
    # Arrange
    $p = New-WebApplication

    # Act
    Assert-Throws { $p | Install-Package PackageWithFailingInitPs1 -Source $context.RepositoryRoot } "This is an exception"

    # Assert
    Assert-Null (Get-ProjectPackage $p PackageWithFailingInitPs1)
    Assert-Null (Get-SolutionPackage PackageWithFailingInitPs1)
}

<#
function Test-InstallPackageWithBadFileInMachineCache {
    # Arrange
    # Write a bad package file to the machine cache
    "foo" > "$($env:LocalAppData)\NuGet\Cache\Ninject.2.2.1.0.nupkg"

    # Act
    $p = New-WebApplication
    $p | Install-Package Ninject -Version 2.2.1.0

    # Assert
    Assert-Package $p Ninject
    Assert-SolutionPackage Ninject
}
#>

function Test-InstallPackageThrowsWhenSourceIsInvalid {
    # Arrange
    $p = New-WebApplication

    # Act & Assert
    Assert-Throws { Install-Package jQuery -source "d:package" } "Unsupported type of source 'd:package'. Please provide an HTTP or local source."
}

function Test-InstallPackageInvokeInstallScriptWhenProjectNameHasApostrophe {
    param(
        $context
    )

    # Arrange
    New-Solution "Gun 'n Roses"
    $p = New-ConsoleApplication

    $global:InstallPackageMessages = @()

    # Act
    Install-Package TestUpdatePackage -Version 2.0.0.0 -Source $context.RepositoryRoot

    # Assert
    Assert-AreEqual 1 $global:InstallPackageMessages.Count
    Assert-AreEqual $p.Name $global:InstallPackageMessages[0]

    # Clean up
    Remove-Variable InstallPackageMessages -Scope Global
}

function Test-InstallPackageInvokeInstallScriptWhenProjectNameHasBrakets {
    param(
        $context
    )

    # Arrange
    New-Solution "Gun [] Roses"
    $p = New-ConsoleApplication

    $global:InstallPackageMessages = @()

    # Act
    Install-Package TestUpdatePackage -Version 2.0.0.0 -Source $context.RepositoryRoot

    # Assert
    Assert-AreEqual 1 $global:InstallPackageMessages.Count
    Assert-AreEqual $p.Name $global:InstallPackageMessages[0]

    # Clean up
    Remove-Variable InstallPackageMessages -Scope Global
}

function SinglePackageInstallIntoSingleProjectWhenSolutionPathHasComma {
    # Arrange
    New-Solution "Tom , Jerry"
    $project = New-ConsoleApplication

    # Act
    Install-Package FakeItEasy -Project $project.Name -Version 1.8.0

    # Assert
    Assert-Reference $project Castle.Core
    Assert-Reference $project FakeItEasy
    Assert-Package $project FakeItEasy
    Assert-Package $project Castle.Core
    Assert-SolutionPackage FakeItEasy
    Assert-SolutionPackage Castle.Core
}

function Test-WebsiteInstallPackageWithNestedAspxFilesShouldNotGoUnderAppCode {
    param(
        $context
    )
    # Arrange
    $p = New-WebSite

    $files = @('Global.asax', 'Site.master', 'About.aspx')

    # Act
    $p | Install-Package PackageWithNestedAspxFiles -Source $context.RepositoryRoot

    # Assert
    $files | %{
        $item = Get-ProjectItem $p $_
        Assert-NotNull $item
        $codeItem = Get-ProjectItem $p "$_.cs"
        Assert-NotNull $codeItem
    }

    Assert-Package $p PackageWithNestedAspxFiles 1.0
    Assert-SolutionPackage PackageWithNestedAspxFiles 1.0
}

function Test-InstallPackageWithReferences {
    param(
        $context
    )

    # Arrange - 1
    $p1 = New-ConsoleApplication

    # Act - 1
    $p1 | Install-Package -Source $context.RepositoryRoot -Id PackageWithReferences

    # Assert - 1
    Assert-Reference $p1 B

    New-Solution "Test"
    # Arrange - 2
    $p2 = New-ClassLibrary

    # Act - 2
    $p2 | Install-Package -Source $context.RepositoryRoot -Id PackageWithReferences

    # Assert - 2
    Assert-Reference $p2 B
}

function Test-InstallPackageNormalizesVersionBeforeCompare {
    param(
        $context
    )

    # Arrange
    $p = New-ClassLibrary

    # Act
    $p | Install-Package PackageWithContentFileAndDependency -Source $context.RepositoryRoot -Version 1.0.0.0

    # Assert
    Assert-Package $p PackageWithContentFileAndDependency 1.0
    Assert-Package $p PackageWithContentFile 1.0
}

function Test-InstallPackageWithFrameworkRefsOnlyRequiredForSL {
    param(
        $context
    )

    # Arrange
    $p = New-ClassLibrary

    # Act
    $p | Install-Package PackageWithNet40AndSLLibButOnlySLGacRefs -Source $context.RepositoryRoot

    # Assert
    Assert-Package $p PackageWithNet40AndSLLibButOnlySLGacRefs
    Assert-SolutionPackage PackageWithNet40AndSLLibButOnlySLGacRefs
}

function Test-InstallPackageWithValuesFromPipe {
    [SkipTest('https://github.com/NuGet/Home/issues/8496')]
    param(
        $context
    )

    # Arrange
    $p = New-ClassLibrary

    # Act
    Get-Package -ListAvailable -Filter "Microsoft-web-helpers" | Install-Package

    # Assert
    Assert-Package $p Microsoft-web-helpers
}

function Test-InstallPackageInstallsHighestReleasedPackageIfPreReleaseFlagIsNotSet {
    # Arrange
    $a = New-ClassLibrary

    # Act
    $a | Install-Package -Source $context.RepositoryRoot PreReleaseTestPackage

    # Assert
    Assert-Package $a 'PreReleaseTestPackage' '1.0.0'
}

function Test-InstallPackageInstallsHighestPackageIfPreReleaseFlagIsSet {
    # Arrange
    $a = New-ClassLibrary

    # Act
    $a | Install-Package -Source $context.RepositoryRoot PreReleaseTestPackage -PreRelease

    # Assert
    Assert-Package $a 'PreReleaseTestPackage' '1.0.1-a'
}

function Test-InstallPackageInstallsHighestPackageIfItIsReleaseWhenPreReleaseFlagIsSet {
    # Arrange
    $a = New-ClassLibrary

    # Act
    $a | Install-Package -Source $context.RepositoryRoot PreReleaseTestPackage.A -PreRelease

    # Assert
    Assert-Package $a 'PreReleaseTestPackage.A' '1.0.0'
}

function Test-InstallingPackagesWorksInTurkishLocaleWhenPackageIdContainsLetterI
{
    # Arrange
    $p = New-ClassLibraryNet46

    $currentCulture = [System.Threading.Thread]::CurrentThread.CurrentCulture

    try
    {
        [System.Threading.Thread]::CurrentThread.CurrentCulture = New-Object 'System.Globalization.CultureInfo' 'tr-TR'

        # Act
        $p | Install-Package 'YUICompressor.NET'
    }
    finally
    {
         # Restore culture
        [System.Threading.Thread]::CurrentThread.CurrentCulture = $currentCulture
    }

    # Assert
    Assert-Package $p 'yuicompressor.NeT'
}

function Test-InstallPackageConsidersPrereleasePackagesWhenResolvingDependencyWhenPrereleaseFlagIsNotSpecified {
    # Arrange
    $a = New-ClassLibrary

    $a | Install-Package -Source $context.RepositoryRoot PrereleaseTestPackage -Prerelease
    Assert-Package $a 'PrereleaseTestPackage' '1.0.1-a'

    $a | Install-Package -Source $context.RepositoryRoot PackageWithDependencyOnPrereleaseTestPackage -FileConflictAction Overwrite
    Assert-Package $a 'PrereleaseTestPackage' '1.0.1-a'
    Assert-Package $a 'PackageWithDependencyOnPrereleaseTestPackage' '1.0.0'
}

<#
function Test-InstallPackageDontMakeExcessiveNetworkRequests
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

    try
    {
        Register-ObjectEvent $packageDownloader "SendingRequest" $eventId { $global:numberOfRequests++; }

        # Act
        $a | Install-Package "nugetpackageexplorer.types" -version 1.0 -source $nugetsource

        # Assert
        Assert-Package $a 'nugetpackageexplorer.types' '1.0'
        Assert-AreEqual 1 $global:numberOfRequests
    }
    finally
    {
        Unregister-Event $eventId -ea SilentlyContinue
        Remove-Variable 'numberOfRequests' -Scope 'Global' -ea SilentlyContinue
    }
}
#>

# Solution level package used
function Test-InstallingSolutionLevelPackagesAddsRecordToProjectLevelConfig
{
    param(
        $context
    )

    # Arrange
    $a = New-ClassLibrary

    # Act
    $a | Install-Package SolutionLevelPkg -version 1.0.0 -source $context.RepositoryRoot
    $a | Install-Package SkypePackage -version 1.0 -source $context.RepositoryRoot

    # Assert
    Assert-Package $a SolutionLevelPkg
}

<# Solution level package used
function Test-InstallingPackageaAfterNuGetDirectoryIsRenamedContinuesUsingDirectory
{
    param(
        $context
    )

    # Arrange
    $f = New-SolutionFolder '.nuget'
    $a = New-ClassLibrary
    $aName = $a.Name

    # Act
    $a | Install-Package SkypePackage -version 1.0 -source $context.RepositoryRoot
    $f.Name = "test"
    $a | Install-Package SolutionLevelPkg -version 1.0.0 -source $context.RepositoryRoot

    # Assert
    $solutionFile = Get-SolutionFullName
    $solutionDir = Split-Path $solutionFile -Parent

    $configFile = "$solutionDir\.nuget\packages.config"

    Assert-True (Test-Path $configFile)

    $content = Get-Content $configFile
    $expected = @"
<?xml version="1.0" encoding="utf-8"?> <packages>   <package id="SolutionLevelPkg" version="1.0.0" /> </packages>
"@

    Assert-AreEqual $expected $content
}
#>

function Test-InstallSatellitePackageCopiesFilesToRuntimeFolderWhenInstalledAsDependency
{
    param(
        $context
    )

    # Arrange
    $p = New-ClassLibrary
    $solutionDir = Get-SolutionDir

    # Act (PackageWithStrongNamedLib is version 1.1, even though the file name is 1.0)
    $p | Install-Package PackageWithStrongNamedLib.ja-jp -Source $context.RepositoryPath

    # Assert (the resources from the satellite package are copied into the runtime package's folder)
    Assert-PathExists (Join-Path $solutionDir packages\PackageWithStrongNamedLib.1.1\lib\ja-jp\Core.resources.dll)
    Assert-PathExists (Join-Path $solutionDir packages\PackageWithStrongNamedLib.1.1\lib\ja-jp\Core.xml)
}

function Test-InstallSatellitePackageCopiesFilesToExistingRuntimePackageFolder
{
    param(
        $context
    )

    # Arrange
    $p = New-ClassLibrary
    $solutionDir = Get-SolutionDir

    # Act (PackageWithStrongNamedLib is version 1.1, even though the file name is 1.0)
    $p | Install-Package PackageWithStrongNamedLib -Source $context.RepositoryPath
    $p | Install-Package PackageWithStrongNamedLib.ja-jp -Source $context.RepositoryPath

    # Assert (the resources from the satellite package are copied into the runtime package's folder)
    Assert-PathExists (Join-Path $solutionDir packages\PackageWithStrongNamedLib.1.1\lib\ja-jp\Core.resources.dll)
    Assert-PathExists (Join-Path $solutionDir packages\PackageWithStrongNamedLib.1.1\lib\ja-jp\Core.xml)
}

function Test-InstallingSatellitePackageOnlyCopiesCultureSpecificLibFolderContents
{
    param(
        $context
    )

    # Arrange
    $p = New-ClassLibrary
    $solutionDir = Get-SolutionDir

    # Act (PackageWithStrongNamedLib is version 1.1, even though the file name is 1.0)
    $p | Install-Package PackageWithStrongNamedLib.ja-jp -Source $context.RepositoryPath

    # Assert (the resources from the satellite package are copied into the runtime package's folder)
    Assert-PathNotExists (Join-Path $solutionDir packages\PackageWithStrongNamedLib.1.1\RootFile.txt)
    Assert-PathNotExists (Join-Path $solutionDir packages\PackageWithStrongNamedLib.1.1\content\ja-jp\file.txt)
    Assert-PathNotExists (Join-Path $solutionDir packages\PackageWithStrongNamedLib.1.1\content\ja-jp.txt)
    Assert-PathNotExists (Join-Path $solutionDir packages\PackageWithStrongNamedLib.1.1\lib\ja-jp.txt)
}

function Test-InstallWithConflictDoesNotUpdateToPrerelease {
    param(
        $context
    )

    Write-Host $context.RepositoryPath

    # Arrange
    $a = New-ClassLibrary

    # Act 1
    $a | Install-Package A -Version 1.0.0 -Source $context.RepositoryPath

    # Assert 1
    Assert-Package $a A 1.0.0

    # Act 2
    $a | Install-Package B -Version 1.0.0 -Source $context.RepositoryPath

    # Assert 2
    Assert-Package $a A 1.1.0
    Assert-Package $a B 1.0.0
}

function Test-ReinstallingAnUninstallPackageIsNotExcessivelyCached {
    param(
        $context
    )

    # Arrange
    $a = New-ClassLibrary

    # Act 1
    $a | Install-Package netfx-Guard -Version 1.2 -Source $context.RepositoryRoot

    # Assert 1
    Assert-Package $a netfx-Guard 1.2

    # Act 2
    $a | Uninstall-Package netfx-Guard

    # Assert 2
    Assert-Null (Get-Package netfx-Guard)

    # Act 3
    $a | Install-Package netfx-Guard -Version 1.2.0 -Source $context.RepositoryRoot

    # Assert 3
    Assert-Package $a netfx-Guard 1.2
}

function Test-InstallPackageInstallContentFilesAccordingToTargetFramework {
    param($context)

    # Arrange
    $project = New-ConsoleApplication

    # Act
    Install-Package TestTargetFxContentFiles -Project $project.Name -Source $context.RepositoryPath

    # Assert
    Assert-Package $project TestTargetFxContentFiles
    Assert-NotNull (Get-ProjectItem $project "Sub\one.txt")
    Assert-Null (Get-ProjectItem $project "two.txt")
}

function Test-InstallPackageInstallContentFilesAccordingToTargetFramework2 {
    param($context)

    # Arrange
    $project = New-ClassLibrary

    # Act
    Install-Package TestTargetFxContentFiles -Project $project.Name -Source $context.RepositoryPath

    # Assert
    Assert-Package $project TestTargetFxContentFiles
    Assert-NotNull (Get-ProjectItem $project "Sub\one.txt")
    Assert-Null (Get-ProjectItem $project "two.txt")
}

function Test-InstallPackageExecuteCorrectInstallScriptsAccordingToTargetFramework {
    param($context)

    # Arrange
    $project = New-ConsoleApplication

    $global:InstallVar = 0

    # Act
    Install-Package TestTargetFxPSScripts -Project $project.Name -Source $context.RepositoryPath

    # Assert
    Assert-Package $project TestTargetFxPSScripts
    Assert-True ($global:InstallVar -eq 1)

    # Clean up
    Remove-Variable InstallVar -Scope Global
}

function Test-InstallPackageExecuteCorrectInstallScriptsAccordingToTargetFramework2 {
    param($context)

    # Arrange
    $project = New-ConsoleApplication

    $global:InstallVar = 0

    # Act
    Install-Package TestTargetFxPSScripts -Project $project.Name -Source $context.RepositoryPath

    # Assert
    Assert-Package $project TestTargetFxPSScripts
    Write-Host 'net40 folder will be chosen over silverlight'
    Assert-True ($global:InstallVar -eq 1)

    # Clean up
    Remove-Variable InstallVar -Scope Global
}

function Test-InstallPackageIgnoreInitScriptIfItIsNotDirectlyUnderTools {
    param($context)

    # Arrange
    $project = New-ConsoleApplication

    $global:InitVar = 0

    # Act
    Install-Package TestTargetFxInvalidInitScript -Project $project.Name -Source $context.RepositoryPath

    # Assert
    Assert-Package $project TestTargetFxInvalidInitScript
    Assert-True ($global:InitVar -eq 0)

    # Clean up
    Remove-Variable InitVar -Scope Global
}

function Test-InstallPackageIgnoreInitScriptIfItIsNotDirectlyUnderTools2 {
    param($context)

    # Arrange
    $project = New-ConsoleApplication

    $global:InitVar = 0

    # Act
    Install-Package TestTargetFxInvalidInitScript -Project $project.Name -Source $context.RepositoryPath

    # Assert
    Assert-Package $project TestTargetFxInvalidInitScript
    Assert-True ($global:InitVar -eq 0)

    # Clean up
    Remove-Variable InitVar -Scope Global
}

function Test-InstallPackageWithEmptyContentFrameworkFolder
{
    param($context)

    # Arrange
    $project = New-ClassLibrary

    # Act
    Install-Package TestEmptyContentFolder -Project $project.Name -Source $context.RepositoryPath

    # Assert
    Assert-Package $project TestEmptyContentFolder
    Assert-Null (Get-ProjectItem $project NewFile.txt)
}

function Test-InstallPackageWithEmptyLibFrameworkFolder
{
    param($context)

    # Arrange
    $project = New-ClassLibrary

    # Act
    Install-Package TestEmptyLibFolder -Project $project.Name -Source $context.RepositoryPath

    # Assert
    Assert-Package $project TestEmptyLibFolder
    Assert-Null (Get-AssemblyReference $project one.dll)
}

function Test-InstallPackageWithEmptyToolsFrameworkFolder
{
    param($context)

    # Arrange
    $project = New-ClassLibrary

    $global:InstallVar = 0

    # Act
    Install-Package TestEmptyToolsFolder -Project $project.Name -Source $context.RepositoryPath

    # Assert
    Assert-Package $project TestEmptyToolsFolder

    Assert-AreEqual 0 $global:InstallVar

    Remove-Variable InstallVar -Scope Global
}

function Test-InstallPackageInstallCorrectDependencyPackageBasedOnTargetFramework
{
    param($context)

    # Arrange
    $project = New-ClassLibrary

    $global:InstallVar = 0

    # Act
    Install-Package TestDependencyTargetFramework -Project $project.Name -Source $context.RepositoryPath

    # Assert
    Assert-Package $project TestDependencyTargetFramework
    Assert-Package $project TestEmptyLibFolder
    Assert-NoPackage $project TestEmptyContentFolder
    Assert-NoPackage $project TestEmptyToolsFolder
}

function Test-InstallingSatellitePackageToWebsiteCopiesResourcesToBin
{
    param($context)

    # Arrange
    $p = New-Website

    # Act
    $p | Install-Package Test.fr-FR -Source $context.RepositoryPath
    Build-Solution

    # Assert
    Assert-Package $p Test.fr-FR
    Assert-Package $p Test

    $projectPath = Get-ProjectDir $p
    Assert-PathExists (Join-Path $projectPath "bin\Test.dll.refresh")
    Assert-PathExists (Join-Path $projectPath "bin\Test.dll")
    Assert-PathExists (Join-Path $projectPath "bin\fr-FR\Test.resources.dll")
}

function Test-InstallPackagePersistTargetFrameworkToPackagesConfig
{
    param($context)

    # Arrange
    $p = New-ClassLibrary

    # Act
    $p | Install-Package PackageA -Source $context.RepositoryPath

    # Assert
    Assert-Package $p 'packageA'
    Assert-Package $p 'packageB'

    $content = [xml](Get-Content (Get-ProjectItemPath $p 'packages.config'))

    $entryA = $content.packages.package[0]
    $entryB = $content.packages.package[1]

    Assert-True 'net40' -ne $entryA.targetFramework -or 'net4' -ne $entryA.targetFramework
    Assert-True 'net40' -ne $entryB.targetFramework -or 'net4' -ne $entryB.targetFramework
}

function Test-ToolsPathForInitAndInstallScriptPointToToolsFolder
{
    param($context)

    # Arrange
    $p = New-ClassLibrary

    # Act
    $p | Install-Package PackageA -Version 1.0.0 -Source $context.RepositoryPath

    # Assert
    Assert-Package $p 'packageA'
}

function Test-InstallPackageWithNestedPS1InsideTools
{
    param($context)

    # Arrange
    $p = New-ClassLibrary
    $solutionDir = Get-SolutionDir

    # Act
    $p | Install-Package TestPackage.SupportingMultipleFrameworksNestedPS1 -Version 1.2.0 -Source $context.RepositoryRoot

    # Assert
    Assert-Package $p 'TestPackage.SupportingMultipleFrameworksNestedPS1'
    Assert-PathExists (Join-Path $solutionDir packages\TestPackage.SupportingMultipleFrameworksNestedPS1.1.2.0\version.txt)
}

function Test-InstallPackageWithNonNestedPS1InsideTools
{
    param($context)

    # Arrange
    $p = New-ClassLibrary
    $solutionDir = Get-SolutionDir

    # Act
    $p | Install-Package TestPackage.SupportingMultipleFrameworksNonNestedPS1 -Version 1.2.0 -Source $context.RepositoryRoot

    # Assert
    Assert-Package $p 'TestPackage.SupportingMultipleFrameworksNonNestedPS1'
    Assert-PathExists (Join-Path $solutionDir packages\TestPackage.SupportingMultipleFrameworksNonNestedPS1.1.2.0\version.txt)
}

function Test-InstallFailCleansUpSatellitePackageFiles
{
    # Verification for work item 2311
    # This also verifies "Fresh Install Of Parent Package Throws When Dependency Package Already Has A Newer Version Installed"
    param ($context)

    # Arrange
    $p = New-ClassLibrary

    # Act
    $p | Install-Package A -Version 1.2.0 -Source $context.RepositoryPath
    try {
    $p | Install-Package A.fr -Source $context.RepositoryPath
    } catch {}

    # Assert
    Assert-Package $p A 1.2.0

    $solutionDir = Get-SolutionDir
    Assert-SolutionPackage A -Version 1.2.0
    Assert-NoSolutionPackage A -Version 1.0.0
    Assert-NoSolutionPackage A.fr -Version 1.0.0
}

function FileTransformWorksOnDependentFile
{
    param($context)

    # Arrange
    $p = New-WebApplication
    Install-Package TTFile -Source $context.RepositoryPath

    # Act
    Install-Package test -Source $context.RepositoryPath

    # Assert
	Assert-Package $p TTFile
	Assert-Package $p test

	$projectDir = Split-Path -parent -path $p.FullName
    $configFilePath = Join-Path -path $projectDir -childpath "one.config"
	$content = [xml](Get-Content $configFilePath)
    Assert-AreEqual "bar" $content.configuration["system.web"].compilation.foo
}

# Solution level package used
function Test-InstallMetaPackageWorksAsExpected
{
    param($context)

    # Arrange
    $p = New-ClassLibrary

    $p | Install-Package MetaPackage -Source $context.RepositoryPath

    # Assert
    Assert-Package $p MetaPackage
    Assert-Package $p Dependency
}

function Test-InstallPackageDoNotUninstallDependenciesWhenSafeUpdatingDependency
{
    # The InstallWalker used to compensate for packages that were already installed by attempting to remove
    # an uninstall operation. Consequently any uninstall operation that occurred later in the graph would cause
    # the package to be uninstalled. This test verifies that this behavior does not occur.

    param ($context)

    # Arrange
    $p = New-ClassLibrary

    # Act - 1
    $p | Install-Package Microsoft.AspNet.WebPages.Administration -Version 2.0.20710.0 -Source $context.RepositoryPath

    # Assert - 1
    Assert-Package $p Microsoft.AspNet.WebPages.Administration 2.0.20710.0
    Assert-Package $p Microsoft.Web.Infrastructure 1.0
    Assert-Package $p NuGet.Core 1.6.2
    Assert-Package $p Microsoft.AspNet.WebPages 2.0.20710.0
    Assert-Package $p Microsoft.AspNet.Razor 2.0.20710.0

    # Act - 2
    $p | Install-Package microsoft-web-helpers -Source $context.RepositoryPath -Verbose

    # Assert - 2
    Assert-Package $p microsoft-web-helpers 2.0.20710.0
    Assert-Package $p Microsoft.AspNet.WebPages.Administration 2.0.20710.0
    Assert-Package $p Microsoft.Web.Infrastructure 1.0
    Assert-Package $p NuGet.Core 1.6.2
    Assert-Package $p Microsoft.AspNet.WebPages 2.0.20710.0
    Assert-Package $p Microsoft.AspNet.Razor 2.0.20710.0
}

function Test-InstallPackageRespectAssemblyReferenceFilterOnDependencyPackages
{
    param ($context)

    # Arrange
    $p = New-ClassLibrary

    # Act
    $p | Install-Package A -Source $context.RepositoryPath

    # Assert
    Assert-Package $p 'A' '1.0.0'
    Assert-Package $p 'B' '1.0.0'

    Assert-Reference $p 'GrayscaleEffect'
    Assert-Null (Get-AssemblyReference $p 'Ookii.Dialogs.Wpf')
}

function Test-InstallPackageRespectAssemblyReferenceFilterOnSecondProject
{
    param ($context)

    # Arrange
    $p = New-ClassLibrary

    # Act
    $p | Install-Package B -Source $context.RepositoryPath

    # Assert
    Assert-Package $p 'B' '1.0.0'
    Assert-Reference $p 'GrayscaleEffect'
    Assert-Null (Get-AssemblyReference $p 'Ookii.Dialogs.Wpf')

    $q = New-ConsoleApplication

    # Act
    $q | Install-Package B -Source $context.RepositoryPath

    # Assert
    Assert-Package $q 'B' '1.0.0'
    Assert-Reference $q 'GrayscaleEffect'
    Assert-Null (Get-AssemblyReference $q 'Ookii.Dialogs.Wpf')
}

function Test-InstallPackageThrowsIfMinClientVersionIsNotSatisfied
{
    param ($context)

    # Arrange
    $p = New-ConsoleApplication

    $currentSemanticVersion = Get-HostSemanticVersion

    # Act & Assert
    Assert-Throws { $p | Install-Package Kitty -Source $context.RepositoryPath } "The 'kitty 1.0.0' package requires NuGet client version '100.0.0' or above, but the current NuGet version is '$currentSemanticVersion'. To upgrade NuGet, please go to https://docs.nuget.org/consume/installing-nuget"
    Assert-NoPackage $p "Kitty"
}

function Test-InstallPackageWithXdtTransformTransformsTheFile
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
}

# Solution-level package used
function Test-ReinstallSolutionLevelPackageWorks
{
    param($context)

    # Arrange
    $p = New-ClassLibrary
    $p | Install-Package SolutionLevelPkg -Source $context.RepositoryRoot

    Assert-SolutionPackage SolutionLevelPkg

    # Act
    Update-Package -Reinstall -Source $context.RepositoryRoot

    # Assert
    Assert-SolutionPackage SolutionLevelPkg
}

function Test-InstallSolutionLevelPackageAddPackagesConfigToProject
{
    param($context)

    # Arrange
    $p = new-ConsoleApplication
    $p | Install-Package SolutionLevelPkg -Source $context.RepositoryRoot

    # Assert
    Assert-SolutionPackage SolutionLevelPkg

    Assert-Package $p SolutionLevelPkg
}

function Test-InstallMetadataPackageAddPackageToProject
{
    param($context)

    # Arrange
    $p = new-ClassLibrary

    # Act
    $p | Install-Package MetadataPackage -Source $context.RepositoryPath

    # Assert
    Assert-Package $p MetadataPackage
    Assert-Package $p DependencyPackage
}

function Test-FrameworkAssemblyReferenceShouldNotHaveBindingRedirect
{
    # Arrange
    $p1 = New-ConsoleApplication -ProjectName Hello

    Assert-NotNull $p1

    # Act
    # NuGet.Protocol.dll 5.11.0 has a reference to System.Net.Http 4.2.0.0
    $p1 | Install-Package NuGet.Protocol -version 5.11.0
    # Microsoft.Extensions.Http.dll has a reference to System.Net.Http 4.0.0.0
    $p1 | Install-Package Microsoft.Extensions.Http -version 5.0.0

    # Assert
    Write-Host "Checking System.Runtime.CompilerServices.Unsafe"
    Assert-BindingRedirect $p1 app.config System.Runtime.CompilerServices.Unsafe '0.0.0.0-5.0.0.0' '5.0.0.0'
    Write-Host "Checking System.Net.Http"
    Assert-NoBindingRedirect $p1 app.config System.Net.Http '0.0.0.0-5.2.0.0' '5.2.0.0'
}

function Test-NonFrameworkAssemblyReferenceShouldHaveABindingRedirect
{
    # Arrange
    $p = New-ConsoleApplication -ProjectName Hello

    # Act
    $p | Install-Package NuGet.Protocol -Version 5.10.0
    $p | Update-Package Newtonsoft.Json -Version 13.0.1

    # Assert
    Assert-BindingRedirect $p app.config Newtonsoft.Json '0.0.0.0-13.0.0.0' '13.0.0.0'
}

function Test-SpecifyDifferentVersionThenServerVersion
{
    # In this test, we explicitly set the version as "2.0",
    # whereas the server version is "2.0.0"
    # this test is to make sure the DataServicePackageRepository
    # checks for all variations of "2.0" (2.0, 2.0.0 and 2.0.0.0)

    # Arrange
    $p = New-WebApplication

    # Act
    Install-Package jQuery -version 2.0

    # Assert
    Assert-Package $p jQuery
}

function Test-InstallLatestVersionWorksCorrectly
{
    # Arrange
    $p = New-WebApplication

    # Act
    Install-Package A -ProjectName $p.Name -Source $context.RepositoryPath

    # Assert
    Assert-Package $p A 0.5
}

function Test-InstallLatestVersionWorksCorrectlyWithPrerelease
{
    # Arrange
    $p = New-WebApplication

    # Act
    Install-Package A -IncludePrerelease -ProjectName $p.Name -Source $context.RepositoryPath

    # Assert
    Assert-Package $p A 0.6-beta
}

function Test-InstallPackageAddPackagesConfigFileToProject
{
    param($context)

    # Arrange
    $p = New-ConsoleApplication

    $projectPath = $p.Properties.Item("FullPath").Value

    $packagesConfigPath = Join-Path $projectPath 'packages.config'

    # Write a file to disk, but do not add it to project
    '<packages><package id="jquery" version="2.0" /></packages>' | out-file $packagesConfigPath

    # Act
    install-package SkypePackage -projectName $p.Name -source $context.RepositoryRoot

    # Assert
    Assert-Package $p SkypePackage

    $xmlFile = [xml](Get-Content $packagesConfigPath)
    Assert-AreEqual 2 $xmlFile.packages.package.Count
    Assert-AreEqual 'jquery' $xmlFile.packages.package[0].Id
    Assert-AreEqual 'SkypePackage' $xmlFile.packages.package[1].Id
}

function Test-InstallPackageWithLeadingZeroInVersion
{
    # Arrange
    $p = New-ClassLibrary

    # Act
    $p | Install-Package -IgnoreDependencies Moq -Version 4.1.1309.0919
    $p | Install-Package -IgnoreDependencies EyeSoft.Wpf.Facilities -Version 0.2.3.690
    $p | Install-Package -IgnoreDependencies CraigsUtilityLibrary-Reflection -Version 3.0.0001
    $p | Install-Package -IgnoreDependencies JSLess -Version 0.01

    # Assert
    Assert-Package $p Moq '4.1.1309.0919'
    Assert-Package $p EyeSoft.Wpf.Facilities 0.2.3.690
    Assert-Package $p CraigsUtilityLibrary-Reflection 3.0.0001
    Assert-Package $p JSLess 0.01
}

function Test-InstallPackagePreservesProjectConfigFile
{
    param($context)

    # Arrange
    $p = New-ClassLibrary "CoolProject"

    $projectPath = $p.Properties.Item("FullPath").Value
    $packagesConfigPath = Join-Path $projectPath 'packages.CoolProject.config'

    # create file and add to project
    $newFile = New-Item $packagesConfigPath -ItemType File
    '<packages></packages>' > $newFile
    $p.ProjectItems.AddFromFile($packagesConfigPath)

    # Act
    $p | Install-Package PackageWithFolder -source $context.RepositoryRoot

    # Assert
    Assert-Package $p PackageWithFolder
    Assert-NotNull (Get-ProjectItem $p 'packages.CoolProject.config')
    Assert-Null (Get-ProjectItem $p 'packages.config')
}

function Test-InstallPackageToWebsitePreservesProjectConfigFile
{
    param($context)

    # Arrange
    $p = New-Website "CoolProject"
    $packagesConfigFileName = "packages.CoolProject.config"
    if ((Get-VSVersion) -gt '10.0')
    {
        # on dev 11.0 etc, the project name could be something lkie
        # "CoolProject(12)". So we need to get the project name
        # to construct the packages config file name.
        $packagesConfigFileName = "packages.$($p.Name).config"
    }

    $projectPath = $p.Properties.Item("FullPath").Value
    $packagesConfigPath = Join-Path $projectPath $packagesConfigFileName

    # create file and add to project
    $newFile = New-Item $packagesConfigPath -ItemType File
    '<packages></packages>' > $newFile
    $p.ProjectItems.AddFromFile($packagesConfigPath)

    # Act
    $p | Install-Package PackageWithFolder -source $context.RepositoryRoot

    # Assert
    Assert-Package $p PackageWithFolder
    Assert-NotNull (Get-ProjectItem $p $packagesConfigFileName)
    Assert-Null (Get-ProjectItem $p 'packages.config')
}

function Test-InstallPackageAddMoreEntriesToProjectConfigFile
{
    param($context)

    # Arrange
    $p = New-ClassLibrary "CoolProject"

    $p | Install-Package PackageWithContentFile -source $context.RepositoryRoot

    $file = Get-ProjectItem $p 'packages.config'
    Assert-NotNull $file

    # rename it
    $file.Name = 'packages.CoolProject.config'

    # Act
    $p | Install-Package PackageWithFolder -source $context.RepositoryRoot

    # Assert
    Assert-Package $p PackageWithFolder
    Assert-Package $p PackageWithContentFile

    Assert-NotNull (Get-ProjectItem $p 'packages.CoolProject.config')
    Assert-Null (Get-ProjectItem $p 'packages.config')
}

# Tests that when -DependencyVersion HighestPatch is specified, the dependency with
# the largest patch number is installed
function Test-InstallPackageWithDependencyVersionHighestPatch
{
    param($context)

    # A depends on B >= 1.0.0
    # Available versions of B are: 1.0.0, 1.0.1, 1.2.0, 1.2.1, 2.0.0, 2.0.1

    # Arrange
    $p = New-ClassLibrary

    # Act
    $p | Install-Package A -Source $context.RepositoryPath -DependencyVersion HighestPatch

    # Assert
    Assert-Package $p A 1.0
    Assert-Package $p B 1.0.1
}

# Tests that when -DependencyVersion HighestPatch is specified, the dependency with
# the lowest major, highest minor, highest patch is installed
function Test-InstallPackageWithDependencyVersionHighestMinor
{
    param($context)

    # A depends on B >= 1.0.0
    # Available versions of B are: 1.0.0, 1.0.1, 1.2.0, 1.2.1, 2.0.0, 2.0.1

    # Arrange
    $p = New-ClassLibrary

    # Act
    $p | Install-Package A -Source $context.RepositoryPath -DependencyVersion HighestMinor

    # Assert
    Assert-Package $p A 1.0
    Assert-Package $p B 1.2.1
}

# Tests that when -DependencyVersion Highest is specified, the dependency with
# the highest version installed
function Test-InstallPackageWithDependencyVersionHighest
{
    param($context)

    # A depends on B >= 1.0.0
    # Available versions of B are: 1.0.0, 1.0.1, 1.2.0, 1.2.1, 2.0.0, 2.0.1

    # Arrange
    $p = New-ClassLibrary

    # Act
    $p | Install-Package A -Source $context.RepositoryPath -DependencyVersion Highest

    # Assert
    Assert-Package $p A 1.0
    Assert-Package $p B 2.0.1
}

# Tests that when -DependencyVersion is lowest, the dependency with
# the smallest patch number is installed
function Test-InstallPackageWithDependencyVersionLowest
{
    param($context)

    # A depends on B >= 1.0.0
    # Available versions of B are: 1.0.0, 1.0.1, 1.2.0, 1.2.1, 2.0.0, 2.0.1

    # Arrange
    $p = New-ClassLibrary

    # Act
    $p | Install-Package A -Source $context.RepositoryPath -DependencyVersion Lowest

    # Assert
    Assert-Package $p A 1.0
    Assert-Package $p B 1.0.0
}

# Tests the case when DependencyVersion is specified in nuget.config
function Test-InstallPackageWithDependencyVersionHighestInNuGetConfig
{
    param($context)

    # Arrange
    Check-NuGetConfig

    $componentModel = Get-VSComponentModel
    $setting = $componentModel.GetService([NuGet.Configuration.ISettings])

    try {
        # Arrange
        $p = New-ClassLibrary

        $setting.AddOrUpdate('config', [NuGet.Configuration.AddItem]::new('dependencyversion', 'HighestPatch'))

        # Act
        $p | Install-Package jquery.validation -version 1.10

        # Assert
        Assert-Package $p jquery.validation 1.10
        Assert-Package $p jquery 1.4.4
    }
    finally {
        $setting.AddOrUpdate('config', [NuGet.Configuration.AddItem]::new('dependencyversion', $null))
    }
}

# Tests that when -DependencyVersion is not specified, the dependency with
# the smallest patch number is installed
function Test-InstallPackageWithoutDependencyVersion
{
    param($context)

   # A depends on B >= 1.0.0
    # Available versions of B are: 1.0.0, 1.0.1, 1.2.0, 1.2.1, 2.0.0, 2.0.1

    # Arrange
    $p = New-ClassLibrary

    # Act
    $p | Install-Package A -Source $context.RepositoryPath

    # Assert
    Assert-Package $p A 1.0
    Assert-Package $p B 1.0.0
}

# Tests that passing in online path to a packages.config file to
# Install-Package works.
function Test-InstallPackagesConfigOnline
{
    [SkipTest('https://github.com/NuGet/Home/issues/12249')]
    param($context)

    # Arrange
    $p = New-ClassLibrary

    # Act
    $p | Install-Package Newtonsoft.Json
    $p | Install-Package https://raw.githubusercontent.com/NuGet/json-ld.net/7dc9becb263a7210ebcd2f571c2a7a07409c240a/src/JsonLD/packages.config

    # Assert
    Assert-Package $p Newtonsoft.Json 4.0.1
}

# Tests that passing in local path to a packages.config file to
# Install-Package works.
function Test-InstallPackagesConfigLocal
{
    param($context)

    # Arrange
    $p = New-ClassLibrary
    $pathToPackagesConfig = Join-Path $context.RepositoryRoot "InstallPackagesConfigLocal\packages.config"

    # Act
    $p | Install-Package $pathToPackagesConfig

    # Assert
    Assert-Package $p jQuery.validation 1.13.1
    Assert-Package $p jQuery 2.1.3
    Assert-Package $p EntityFramework 6.1.3-beta1
}

# Tests that passing in online path to a .nupkg file to
# Install-Package works.
function Test-InstallPackagesNupkgOnline
{
    param($context)

    # Arrange
    $p = New-ClassLibrary

    # Act
    $p | Install-package https://globalcdn.nuget.org/packages/microsoft.aspnet.mvc.4.0.20505.nupkg

    # Assert
    Assert-Package $p microsoft.aspnet.mvc 4.0.20505.0
    Assert-Package $p microsoft.aspnet.webpages 2.0.20505
    Assert-Package $p microsoft.aspnet.razor 2.0.20505
    Assert-Package $p microsoft.web.infrastructure 1.0.0
}

# Tests that passing in local path to a .nupkg file to
# Install-Package works.
function Test-InstallPackagesNupkgLocal
{
    param($context)

    # Arrange
    $p = New-ClassLibrary
    $pathToPackagesNupkg = Join-Path $context.RepositoryRoot "PackageWithFolder.1.0.nupkg"

    # Act
    $p | Install-Package $pathToPackagesNupkg

    # Assert
    Assert-Package $p PackageWithFolder 1.0
}

function Test-InstallPackageMissingPackage {
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
    Install-Package Castle.Core -Version 2.5.1

    # Assert
    Assert-Package $proj Castle.Core 2.5.1
}

function Test-InstallPackageMissingPackageNoConsent {
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
        Assert-Throws { Install-Package PackageWithContentFile } "Some NuGet packages are missing from the solution. The packages need to be restored in order to build the dependency graph. Restore the packages before performing any operations."
    }
    finally {
        [NuGet.PackageManagement.VisualStudio.SettingsHelper]::Set('PackageRestoreConsentGranted', 'true')
        [NuGet.PackageManagement.VisualStudio.SettingsHelper]::Set('PackageRestoreIsAutomatic', 'true')
    }
}

function Test-InstallPackageWithScriptAddImportFile
{
    param($context)

    # Arrange
    $p = New-ClassLibrary

    #Act
    $p | Install-Package Microsoft.Bcl.build -version 1.0.14
    Build-Solution

    # Assert
    $errorlist = Get-Errors
    Assert-AreEqual 0 $errorlist.Count
}

# Temporarily disable this test
function Disable-Test-InstallPackageInCpsApp
{
    param($context)

    # Arrange
    $p = New-CpsApp "CpsProject"

    #Act
    $p | Install-Package GoogleAnalyticsTracker.Core -version 3.2.0

    # Assert
    $item = Get-ProjectItem $p packages.config
    Assert-NotNull $item
}

function Test-InstallPackageWithEscapedSymbolInPath()
{
    param($context)

    # Arrange
    $p = New-ClassLibrary

    #Act
    $p | Install-Package Xam.Plugin.Connectivity -version 1.0.2

    # Assert
    Assert-Package $p Xam.Plugin.Connectivity
}

function Test-InstallPackageWithRootNamespaceInPPFile {
    param(
        $context
    )

    # Arrange
    $p = New-ClassLibrary "testProject"

    # Act
    Install-Package PackageWithRootNamespaceFileTransform -Source $context.RepositoryRoot

    # Assert
    Assert-NotNull (Get-ProjectItem $p foo.cs)
    $path = (Get-ProjectItemPath $p foo.cs)
    $content = [System.IO.File]::ReadAllText($path)
    Assert-True ($content.Contains("namespace testProject"))
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