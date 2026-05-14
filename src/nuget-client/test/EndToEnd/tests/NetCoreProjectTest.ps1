function Test-NetCoreVSandMSBuildNoOp {
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
