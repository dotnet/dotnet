# Test that getting repository path from the VsSettings object returns
# the right value specified in $solutionDir\.nuget\nuget.config
function Test-GetRepositoryPathFromVsSettings {
    param($context)

	# Arrange
	$p1 = New-ClassLibrary

	$solutionFile = Get-SolutionFullName
	$solutionDir = Split-Path $solutionFile -Parent
	$nugetDir = Join-Path $solutionDir ".nuget"
	$repoPath = Join-Path $solutionDir "my_repo"

	New-Item $nugetDir -type directory
	$settingFile = Join-Path $nugetDir "nuget.config"
	$settingFileContent =@"
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <config>
    <add key="repositoryPath" value="{0}" />
  </config>
</configuration>
"@

	$settingFileContent -f $repoPath | Out-File -Encoding "UTF8" $settingFile

	# Act
	# close & open the solution so that the settings are reloaded.
	SaveAs-Solution($solutionFile)
	Close-Solution
	Write-Host 'Closed solution'
	Open-Solution $solutionFile
	Write-Host 'Open solution'
	$p2 = Get-Project
	$p2 | Install-Package elmah -Version 1.2.2

	$vsSetting = [NuGet.PackageManagement.VisualStudio.SettingsHelper]::GetVsSettings()
	$v = [NuGet.Configuration.SettingsUtility]::GetValueForAddItem($vsSetting, "config", "repositoryPath")

	# Assert
	Write-Host 'Expected:' $repoPath
	Write-Host 'Actual:' $v
	Assert-AreEqual $repoPath $v
        Write-Host 'Testing if $repoPath exists'
	Assert-True (Test-Path $repoPath)
	Assert-True (Test-Path (Join-Path $repoPath "elmah.1.2.2"))
}

function Test-GetMachineWideSettingBaseDirectoryInVSIX {
    param($context)

	# Arrange
	[string]$vsVersionString = Get-VSVersion
	[string]$machineWideSettingsBaseDirExpected

	$machineWideSettingsBaseDirExpected = Join-Path (Get-ChildItem Env:PROGRAMFILES`(X86`)).Value "\NuGet"

	# Act
	$machineWideSettingsBaseDirActual = [NuGet.Common.NuGetEnvironment]::GetFolderPath([NuGet.Common.NuGetFolderPath]::MachineWideSettingsBaseDirectory)

	# Assert
	Assert-StringEqual $machineWideSettingsBaseDirActual  $machineWideSettingsBaseDirExpected -CaseSensitive $True
}