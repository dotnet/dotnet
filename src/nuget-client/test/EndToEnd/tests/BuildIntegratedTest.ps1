# install a package with dependencies
function Test-BuildIntegratedInstallAndVerifyLockFileContainsChildDependency {
    # Arrange
    $project = New-BuildIntegratedProj UAPApp

    # Act
    Install-Package AppSupport.Win81 -ProjectName $project.Name -version 0.0.3-alpha

    # Assert
    Assert-NetCorePackageInLockFile $project WindowsAzure.MobileServices 1.0.2
    Assert-NetCoreNoPackageReference $project WindowsAzure.MobileServices
} 

function Test-BuildIntegratedLockFileIsCreatedOnBuild {
    # Arrange
    $project = New-BuildIntegratedProj UAPApp
    Install-Package NuGet.Versioning -ProjectName $project.Name -version 1.0.7
    Remove-AssetsFile $project

    # Act
    Build-Solution

    # Assert
    Assert-NetCorePackageInLockFile $project NuGet.Versioning 1.0.7
}

function Test-BuildIntegratedPackageFailsIfDowngradeWasDetected {
    # Arrange
    $project = New-BuildIntegratedProj UAPApp

    # Act
    Install-Package Newtonsoft.Json -ProjectName $project.Name -version 6.0.4

	# Assert
    # DotNetRDF requires json.net >= 6.0.8, but the direct dependency attempts to downgrade it.
	Install-Package DotNetRDF  -ProjectName $project.Name -version 1.0.8.3533
    Assert-NetCorePackageInLockFile $project Newtonsoft.Json 6.0.4
}

function Test-BuildIntegratedDependencyUpdatedByInstall {
    # Arrange
    $project = New-BuildIntegratedProj UAPApp

    # Act
    Install-Package DotNetRDF  -ProjectName $project.Name -version 1.0.8.3533
    Install-Package Newtonsoft.Json -ProjectName $project.Name -version 7.0.1

    # Assert
    # DotNetRDF requires json.net 6.0.8
    Assert-NetCorePackageInLockFile $project Newtonsoft.Json 7.0.1
}

function Test-BuildIntegratedInstallPackageInvokeInitScript {
    param(
        $context
    )
    
    # Arrange
    $p = New-BuildIntegratedProj

    # Act
    Install-Package PackageWithScriptsB -Source $context.RepositoryRoot

    # Assert

    # This asserts init.ps1 gets called
    Assert-True (Test-Path function:\Get-WorldB)
}

function Test-BuildIntegratedInstallPackageJsonNet701Beta3 {
    # Arrange
    $project = New-BuildIntegratedProj UAPApp

    # Act
    Install-Package newtonsoft.json -ProjectName $project.Name -version 7.0.1-beta3

    # Assert
    Assert-PackageReferenceAssetsFileRuntimeAssembly $project "lib/net45/Newtonsoft.Json.dll"
}

function Test-BuildIntegratedProjectClosure {
    # Arrange
    $project1 = New-Project BuildIntegratedClassLibrary Project1
    $project2 = New-Project BuildIntegratedClassLibrary Project2
    Add-ProjectReference $project1 $project2

    Install-Package NuGet.Versioning -ProjectName $project2.Name -version 1.0.7
    Remove-AssetsFile $project2

    # Act
    Build-Solution

    # Assert
    Assert-NetCorePackageInLockFile $project1 NuGet.Versioning 1.0.7
    Assert-NetCorePackageInLockFile $project2 NuGet.Versioning 1.0.7
}

function Test-BuildIntegratedProjectClosureWithLegacyProjects {
    # Arrange
    $project1 = New-Project BuildIntegratedClassLibrary Project1
    $project2 = New-ClassLibrary Project2
    $project3 = New-ClassLibrary Project3

    Add-ProjectReference $project1 $project2
    Add-ProjectReference $project2 $project3

    Install-Package Comparers -ProjectName $project2.Name -version 4.0.0

    # Act
    Build-Solution

    # Assert
    Assert-NotNull Get-NetCoreLockFile $project1
}

# Tests that packages are restored on build
function Test-BuildIntegratedMixedLegacyProjects {
    # Arrange
    $project1 = New-ClassLibrary
    $project1 | Install-Package Newtonsoft.Json -Version 13.0.1

    $project2 = New-Project BuildIntegratedClassLibrary
    $project2 | Install-Package NuGet.Versioning -Version 1.0.7

    # delete the packages folder
    $packagesDir = Get-PackagesDir
    Remove-Item -Recurse -Force $packagesDir
    Assert-False (Test-Path $packagesDir)

    # delete the lock file
    $lockFile = Get-NetCoreLockFilePath $project2
    Remove-Item $lockFile
    Assert-False (Test-Path $lockFile)

    # Act
    Build-Solution

    # Assert
    Assert-True (Test-Path $packagesDir)
    Assert-Package $project1 Newtonsoft.Json
    Assert-True (Test-Path $lockFile)
    Assert-NetCorePackageInLockFile $project2 NuGet.Versioning 1.0.7
}

function Test-BuildIntegratedMixedLegacyProjectsPackageReferenceOnly {
    # Arrange
    $project1 = New-ClassLibrary
    $project1 | Install-Package Newtonsoft.Json -Version 13.0.1

    $project2 = New-Project BuildIntegratedClassLibrary
    $project2 | Install-Package NuGet.Versioning -Version 1.0.7

    # delete the lock file
    $lockFile = Get-NetCoreLockFilePath $project2
    Remove-Item $lockFile
    Assert-False (Test-Path $lockFile)

    # Act
    Build-Solution

    # Assert
    Assert-True (Test-Path $lockFile)
    Assert-NetCorePackageInLockFile $project2 NuGet.Versioning 1.0.7
}

function Test-BuildIntegratedMixedLegacyProjectsPackagesFolderOnly {
    # Arrange
    $project1 = New-ClassLibrary
    $project1 | Install-Package Newtonsoft.Json -Version 13.0.1

    $project2 = New-Project BuildIntegratedClassLibrary
    $project2 | Install-Package NuGet.Versioning -Version 1.0.7

    # delete the packages folder
    $packagesDir = Get-PackagesDir
    Remove-Item -Recurse -Force $packagesDir
    Assert-False (Test-Path $packagesDir)

    # Act
    Build-Solution

    # Assert
    Assert-True (Test-Path $packagesDir)
    Assert-Package $project1 Newtonsoft.Json
}

# Verifies that project.json that specified in project.json referenced transitively through a non-project.json project
# are correctly pulled in.
function Test-BuildIntegratedTransitivePackageReferenceRestores {
    # Arrange
    $project1 = New-Project BuildIntegratedClassLibrary
    $project2 = New-ClassLibraryNET46
    $project3 = New-Project BuildIntegratedClassLibrary

    Add-ProjectReference $project2 $project1
    Add-ProjectReference $project3 $project2

    # Act
    $project1 | Install-Package NuGet.Versioning -Version 1.0.7
    Build-Solution

    # Assert
    Assert-NoPackage $project2 NuGet.Versioning 1.0.7
    Assert-NetCorePackageInLockFile $project1 NuGet.Versioning 1.0.7
    Assert-NetCorePackageInLockFile $project3 NuGet.Versioning 1.0.7
}

# Verifies that parent projects are restored after an install
function Test-BuildIntegratedParentProjectIsRestoredAfterInstall {
    # Arrange
    $project1 = New-BuildIntegratedProj UAPApp1
    $project2 = New-BuildIntegratedProj UAPApp2
    $project3 = New-BuildIntegratedProj UAPApp3
    $project4 = New-BuildIntegratedProj UAPApp4

    Add-ProjectReference $project1 $project2
    Add-ProjectReference $project2 $project3
    Add-ProjectReference $project3 $project4

    # Act
    $project3 | Install-Package NuGet.Versioning -Version 1.0.7

    # Assert
    Assert-NetCorePackageInLockFile $project1 NuGet.Versioning 1.0.7
    Assert-NetCorePackageInLockFile $project2 NuGet.Versioning 1.0.7
    Assert-NetCorePackageInLockFile $project3 NuGet.Versioning 1.0.7

    # the child project should not be restored
    Assert-AssetsFileDoesNotExist $project4
}

# Verifies that parent projects are restored after an uninstall
function Test-BuildIntegratedParentProjectIsRestoredAfterUnInstall {
    # Arrange
    $project1 = New-BuildIntegratedProj UAPApp1
    $project2 = New-BuildIntegratedProj UAPApp2
    $project3 = New-BuildIntegratedProj UAPApp3
    $project4 = New-BuildIntegratedProj UAPApp4

    Add-ProjectReference $project1 $project2
    Add-ProjectReference $project2 $project3
    Add-ProjectReference $project3 $project4

    $project3 | Install-Package NuGet.Versioning -Version 1.0.7
    Remove-AssetsFile $project1
    Remove-AssetsFile $project2
    Remove-AssetsFile $project3

    # Act
    $project3 | Uninstall-Package NuGet.Versioning -Version 1.0.7

    # Assert
    Assert-NetCorePackageNotInLockFile $project1 NuGet.Versioning
    Assert-NetCorePackageNotInLockFile $project2 NuGet.Versioning
    Assert-NetCorePackageNotInLockFile $project3 NuGet.Versioning

    # the child project should not be restored
    Assert-AssetsFileDoesNotExist $project4
}

# Verifies that parent projects are restored after an update
function Test-BuildIntegratedParentProjectIsRestoredAfterUpdate {
    # Arrange
    $project1 = New-BuildIntegratedProj UAPApp1
    $project2 = New-BuildIntegratedProj UAPApp2
    $project3 = New-BuildIntegratedProj UAPApp3
    $project4 = New-BuildIntegratedProj UAPApp4

    Add-ProjectReference $project1 $project2
    Add-ProjectReference $project2 $project3
    Add-ProjectReference $project3 $project4

    $project3 | Install-Package NuGet.Versioning -Version 1.0.5
    Remove-AssetsFile $project1
    Remove-AssetsFile $project2
    Remove-AssetsFile $project3

    # Act    
    $project3 | Update-Package NuGet.Versioning -Version 1.0.7

    # Assert
    Assert-NetCorePackageInLockFile $project1 NuGet.Versioning 1.0.7    
    Assert-NetCorePackageInLockFile $project2 NuGet.Versioning 1.0.7
    Assert-NetCorePackageInLockFile $project3 NuGet.Versioning 1.0.7

    # the child project should not be restored
    Assert-AssetsFileDoesNotExist $project4
}

# Verify that all build integrated projects are included in the closure, even when a 
# non-build integrated project exists in between them
function Test-BuildIntegratedParentProjectIsRestoredAfterInstallWithClassLibInTree {
    # Arrange
    $project1 = New-Project BuildIntegratedClassLibrary
    $project2 = New-ClassLibraryNET46 ClassLib2
    $project3 = New-Project BuildIntegratedClassLibrary

    Add-ProjectReference $project1 $project2
    Add-ProjectReference $project2 $project3

    # Act
    $project3 | Install-Package NuGet.Versioning -Version 1.0.7

    # Assert
    Assert-NetCorePackageInLockFile $project1 NuGet.Versioning 1.0.7
}

function Test-InconsistencyBetweenAssetsAndProjectFile{
    param()

    $projectT = New-Project PackageReferenceClassLibrary
    $projectT | Install-Package Newtonsoft.Json -Version 13.0.1
    $solutionFile = Get-SolutionFullName
    $projectFullName = $projectT.FullName
    $projectT.Save();
    
    #Pre-condition
    Assert-True ($projectT | Test-InstalledPackage -Id Newtonsoft.Json -Version 13.0.1) -Message 'Test package should be installed'
    
    SaveAs-Solution($solutionFile)
    Close-Solution
    Remove-PackageReference $projectFullName Newtonsoft.Json
    Open-Solution $solutionFile    
    $project = Get-Project

    #Pre-condition
    Assert-False ($project | Test-InstalledPackage -Id Newtonsoft.Json -Version 13.0.1) -Message 'Test package should not be installed'

    #Act
    $project | Install-Package Newtonsoft.Json -Version 13.0.1

    #Assert
    Assert-True ($project | Test-InstalledPackage -Id Newtonsoft.Json -Version 13.0.1) -Message 'Test package should be installed'
}

function Remove-PackageReference {
    param(
        [parameter(Mandatory = $true)]
        $projectPath,
        [parameter(Mandatory = $true)]
        $packageReference
    )
    $doc = [xml](Get-Content $projectPath)
    $ns = New-Object System.Xml.XmlNamespaceManager($doc.NameTable)
    $ns.AddNamespace("ns", $doc.DocumentElement.NamespaceURI)
    $node = $doc.SelectSingleNode("//ns:PackageReference[@Include='$packageReference']",$ns)
    $node.ParentNode.RemoveChild($node)

    $doc.Save($projectPath)
}

function Test-BuildIntegratedRestoreAfterInstall {
    # Arrange
    $project = New-Project PackageReferenceClassLibrary
    $project | Install-Package Newtonsoft.Json -Version 13.0.1
    Assert-ProjectCacheFileExists $project
    $cacheFile = Get-ProjectCacheFilePath $project
    $installTimeStamp = ([datetime](Get-ItemProperty -Path $cacheFile -Name LastWriteTime).lastwritetime).Ticks

    #Act
    Build-Solution
    $restoreTimeStamp =( [datetime](Get-ItemProperty -Path $cacheFile -Name LastWriteTime).lastwritetime).Ticks
    
    #Assert
    Assert-True ($installTimeStamp -eq $restoreTimeStamp)
}

function Test-BuildIntegratedRestoreAfterUninstall {
    # Arrange
    $project = New-Project PackageReferenceClassLibrary
    $project | Install-Package Newtonsoft.Json -Version 13.0.1
    Assert-ProjectCacheFileExists $project
    $cacheFile = Get-ProjectCacheFilePath $project

    #Act
    $project | Uninstall-Package Newtonsoft.Json -Version 13.0.1

    $uninstallTimeStamp =( [datetime](Get-ItemProperty -Path $cacheFile -Name LastWriteTime).lastwritetime).Ticks
    
    Build-Solution

    $restoreTimeStamp =( [datetime](Get-ItemProperty -Path $cacheFile -Name LastWriteTime).lastwritetime).Ticks
    
    #Assert
    Assert-True ($uninstallTimeStamp -eq $restoreTimeStamp)
}
function Test-BuildIntegratedProjectGetPackageTransitive {
    param($Context, $TestCase)

    $projectR = New-Project $TestCase.ProjectTemplate
    $projectT = New-Project BuildIntegratedClassLibrary

    $projectT | Add-ProjectReference -ProjectTo $projectR
    $projectR | Install-Package NuGet.Versioning -Version 1.0.7
    Clean-Solution

    $projectT = $projectT | Select-Object UniqueName, ProjectName, FullName

    # Act (Restore)
    Build-Solution

    Assert-NetCorePackageInLockFile $projectT NuGet.Versioning 1.0.7
}

function TestCases-BuildIntegratedProjectGetPackageTransitive{
    BuildProjectTemplateTestCases 'PackageReferenceClassLibrary', 'BuildIntegratedClassLibrary'
}

function Test-PackageReferenceProjectGetPackageTransitive {
    param($Context, $TestCase)

    $projectR = New-Project $TestCase.ProjectTemplate
    $projectT = New-Project PackageReferenceClassLibrary

    $projectT | Add-ProjectReference -ProjectTo $projectR
    $projectR | Install-Package NuGet.Versioning -Version 1.0.7
    Clean-Solution

    $projectT = $projectT | Select-Object UniqueName, ProjectName, FullName

    # Act (Restore)
    Build-Solution

    Assert-NetCorePackageInLockFile $projectT NuGet.Versioning 1.0.7
}

function TestCases-PackageReferenceProjectGetPackageTransitive{
    BuildProjectTemplateTestCases 'ClassLibrary' , 'PackageReferenceClassLibrary', 'BuildIntegratedClassLibrary'
}

function Test-BuildIntegratedVSandMSBuildNoOp {
    # Arrange
    $project = New-Project PackageReferenceClassLibrary
    $project | Install-Package Newtonsoft.Json -Version 13.0.1
    Assert-ProjectCacheFileExists $project
    $cacheFile = Get-ProjectCacheFilePath $project
    
    Build-Solution

    $VSRestoreTimestamp =( [datetime](Get-ItemProperty -Path $cacheFile -Name LastWriteTime).lastwritetime).Ticks
    
    $MSBuildExe = Get-MSBuildExe

    & "$MSBuildExe" /t:restore $project.FullName
    Assert-True ($LASTEXITCODE -eq 0)

    $MsBuildRestoreTimestamp =( [datetime](Get-ItemProperty -Path $cacheFile -Name LastWriteTime).lastwritetime).Ticks

    #Assert
    Assert-True ($MsBuildRestoreTimestamp -eq $VSRestoreTimestamp)
}

function Test-PackageReferenceProjectWithLockFile{

    $projectT = New-Project PackageReferenceClassLibraryWithLockFile
    $projectT | Install-Package Newtonsoft.Json -Version 13.0.1
    $projectT.Save();
    
    #Assert
    Assert-PackagesLockFile $projectT
}

function Test-PackageReferenceToPackagesConfigProjectWithLockFile {
    $project1 = New-Project PackageReferenceClassLibraryWithLockFile
    $project2 = New-ClassLibraryNET46
    Add-ProjectReference $project1 $project2

    $project1.Save();
    Build-Solution

    $assetsFile = Get-NetCoreLockFilePath $project1
    Remove-Item -Force $assetsFile
    $project1 | Install-Package Newtonsoft.Json -Version 13.0.1

    # Act
    Build-Solution

    # Assert
    Assert-PathExists $assetsFile
}

function BuildProjectTemplateTestCases([string[]]$ProjectTemplates) {		
    $ProjectTemplates | ForEach-Object{		
        $testCase = New-Object System.Object		
        $testCase | Add-Member -Type NoteProperty -Name ProjectTemplate -Value $_		
        $testCase		
    }		
}
