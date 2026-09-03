# basic create for uwp package ref based project
function Test-UwpPackageRefClassLibraryCreate {

    # Arrange & Act
    $project = New-UwpPackageRefClassLibrary UwpLibrary1

    # Assert
    Assert-NetCoreProjectCreation $project
}


function Test-WPFPackageVersionNoInclusiveLowerBoundNU1604 {

    # Arrange
    param(
        $context
    )    

    $sol = New-Solution
    $solutionDir = Get-SolutionDir

    # Create Directory.Build.props file with PackageReference with invalid version.
    $directoryPropPath = Join-Path $solutionDir "Directory.Build.props"
	$directoryPropContent =@"
<Project>
    <ItemGroup>
      <PackageReference Include="TestUpdatePackage"/>
    </ItemGroup>
  
  </Project>  
"@   

    $directoryPropContent | Out-File  $directoryPropPath   

    $project = $sol | New-WpfApplication

    # Act
    Build-Solution

    # Assert   
    # Make sure VS UI warning has NU1604 warning       
    $warnings = Get-Warnings   
    Assert-True $warnings.Contains("Project dependency TestUpdatePackage does not contain an inclusive lower bound. Include a lower bound in the dependency version to ensure consistent restore results.")
   
    # Make sure asset file has NU1604 warning   
    $NetCoreLockFilePath = Get-NetCoreLockFilePath $project
    $assetJson = Get-Content -Raw -Path $NetCoreLockFilePath | ConvertFrom-Json
    $warningCodes = $assetJson.logs | Select-Object Code
    Assert-True $warningCodes.Code.Contains("NU1604")

    # Make sure msbuild creates asset file with NU1604 warning too
    $MSBuildExe = Get-MSBuildExe    
    & "$MSBuildExe" /t:restore  $project.FullName   
    $NetCoreLockFilePath = Get-NetCoreLockFilePath $project
    $assetJson = Get-Content -Raw -Path $NetCoreLockFilePath | ConvertFrom-Json
    $warningCodes = $assetJson.logs | Select-Object Code
    Assert-True $warningCodes.Code.Contains("NU1604")
}