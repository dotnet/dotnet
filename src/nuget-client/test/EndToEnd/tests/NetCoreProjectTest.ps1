# VSSolutionManager and ProjectSystemCache event test for .net core
function Test-NetCoreProjectSystemCacheUpdateEvent {

    # Arrange
    $projectA = New-NetCoreConsoleApp
    Build-Solution
    Assert-NetCoreProjectCreation $projectA

    $componentModel = Get-VSComponentModel
    $solutionManager = $componentModel.GetService([NuGet.PackageManagement.ISolutionManager])

    $cacheEvent = $null

    Get-Event | Remove-Event
    Register-ObjectEvent -InputObject $solutionManager -EventName AfterNuGetCacheUpdated -SourceIdentifier SolutionManagerCacheUpdated

    Try
    {
        # Act
        $projectA | Install-Package Newtonsoft.Json -Version '13.0.1'

        $cacheEvent = Wait-Event -SourceIdentifier SolutionManagerCacheUpdated -TimeoutSec 10
    }
    Finally
    {
        Unregister-Event -SourceIdentifier SolutionManagerCacheUpdated
    }

    # Assert
    Assert-NotNull $cacheEvent -Message "Cache update event should've been raised"
}

function Test-NetCoreConsoleAppClean {

    # Arrange & Act
    $project = New-NetCoreConsoleApp ConsoleApp

    Build-Solution

    Assert-ProjectCacheFileExists $project

    #Act
    Clean-Solution

    #Assert
    Assert-ProjectCacheFileNotExists $project
}

function Test-NetCoreConsoleAppRebuildDoesNotDeleteCacheFile {
    # Arrange & Act
    $project = New-NetCoreConsoleApp ConsoleApp
    Build-Solution

    Assert-ProjectCacheFileExists $project

    AdviseSolutionEvents

    #Act
    Rebuild-Solution

    WaitUntilRebuildCompleted
    UnadviseSolutionEvents

    #Assert
    Assert-ProjectCacheFileExists $project
}

function Test-NetCoreVSandMSBuildNoOp {
    [SkipTest('https://github.com/NuGet/Home/issues/13003')]
    param ()

    # Arrange
    $project = New-NetCoreConsoleApp ConsoleApp
    Build-Solution

    Assert-ProjectCacheFileExists $project
    $cacheFile = Get-ProjectCacheFilePath $project

    #Act

    $VSRestoreTimestamp =( [datetime](Get-ItemProperty -Path $cacheFile -Name LastWriteTime).lastwritetime).Ticks

    $MSBuildExe = Get-MSBuildExe

    & "$MSBuildExe" /t:restore  $project.FullName
    Assert-True ($LASTEXITCODE -eq 0)

    $MsBuildRestoreTimestamp =( [datetime](Get-ItemProperty -Path $cacheFile -Name LastWriteTime).lastwritetime).Ticks

    #Assert
    Assert-True ($MsBuildRestoreTimestamp -eq $VSRestoreTimestamp)
}

function Test-NetCoreTargetFrameworksVSandMSBuildNoOp {
    [SkipTest('https://github.com/NuGet/Home/issues/13003')]
    param ()

    # Arrange
    $project = New-NetCoreConsoleTargetFrameworksApp ConsoleApp
    Build-Solution

    Assert-ProjectCacheFileExists $project
    $cacheFile = Get-ProjectCacheFilePath $project

    #Act

    $VSRestoreTimestamp =( [datetime](Get-ItemProperty -Path $cacheFile -Name LastWriteTime).lastwritetime).Ticks

    $MSBuildExe = Get-MSBuildExe

    & "$MSBuildExe" /t:restore  $project.FullName
    Assert-True ($LASTEXITCODE -eq 0)

    $MsBuildRestoreTimestamp =( [datetime](Get-ItemProperty -Path $cacheFile -Name LastWriteTime).lastwritetime).Ticks

    #Assert
    Assert-True ($MsBuildRestoreTimestamp -eq $VSRestoreTimestamp)
}

function Test-NetCoreMultipleTargetFrameworksVSandMSBuildNoOp {
    [SkipTest('https://github.com/NuGet/Home/issues/11231')]
    param ()

    # Arrange
    $project = New-NetCoreConsoleMultipleTargetFrameworksApp ConsoleApp
    Build-Solution

    Assert-ProjectCacheFileExists $project
    $cacheFile = Get-ProjectCacheFilePath $project

    #Act

    $VSRestoreTimestamp =( [datetime](Get-ItemProperty -Path $cacheFile -Name LastWriteTime).lastwritetime).Ticks

    $MSBuildExe = Get-MSBuildExe

    & "$MSBuildExe" /t:restore  $project.FullName
    Assert-True ($LASTEXITCODE -eq 0)

    $MsBuildRestoreTimestamp =( [datetime](Get-ItemProperty -Path $cacheFile -Name LastWriteTime).lastwritetime).Ticks

    #Assert
    Assert-True ($MsBuildRestoreTimestamp -eq $VSRestoreTimestamp)
}

function Test-NetCoreToolsVSandMSBuildNoOp {
    [SkipTest('https://github.com/NuGet/Home/issues/11231')]
    param ()

    # Arrange
    $project = New-NetCoreWebApp10 ConsoleApp
    Assert-NetCoreProjectCreation $project

    $ToolsCacheFile = Get-ProjectToolsCacheFilePath $project

    #Act
    $VSRestoreTimestamp =( [datetime](Get-ItemProperty -Path $ToolsCacheFile -Name LastWriteTime).lastwritetime).Ticks

    $MSBuildExe = Get-MSBuildExe

    & "$MSBuildExe" /t:restore  $project.FullName
    Assert-True ($LASTEXITCODE -eq 0)

    $MsBuildRestoreTimestamp =( [datetime](Get-ItemProperty -Path $ToolsCacheFile -Name LastWriteTime).lastwritetime).Ticks

    #Assert
    Assert-True ($MsBuildRestoreTimestamp -eq $VSRestoreTimestamp)
}
