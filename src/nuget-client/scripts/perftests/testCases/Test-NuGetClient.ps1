Param(
    [Parameter(Mandatory = $True)]
    [string] $nugetClientFilePath,
    [Parameter(Mandatory = $True)]
    [string] $sourceRootFolderPath,
    [Parameter(Mandatory = $True)]
    [string] $resultsFolderPath,
    [Parameter(Mandatory = $True)]
    [string] $logsFolderPath,
    [string] $nugetFoldersPath,
    [int] $iterationCount,
    [string] $additionalOptions
)

. "$PSScriptRoot\..\PerformanceTestUtilities.ps1"

if(![string]::IsNullOrEmpty($additionalOptions))
{
RunPerformanceTestsOnGitRepository `
    -nugetClientFilePath $nugetClientFilePath `
    -sourceRootFolderPath $sourceRootFolderPath `
    -testCaseName $testCaseName `
    -repoUrl "https://github.com/NuGet/NuGet.Client.git" `
    -commitHash "4b906a9bd9dde24da0caaecbaf43c747b17f2668" `
    -resultsFolderPath $resultsFolderPath `
    -logsFolderPath $logsFolderPath `
    -nugetFoldersPath $nugetFoldersPath `
    -iterationCount $iterationCount `
    -additionalOptions $additionalOptions
} 
Else 
{
RunPerformanceTestsOnGitRepository `
    -nugetClientFilePath $nugetClientFilePath `
    -sourceRootFolderPath $sourceRootFolderPath `
    -testCaseName $testCaseName `
    -repoUrl "https://github.com/NuGet/NuGet.Client.git" `
    -commitHash "f6279fb833960d9128d16c4e911705167e0bb754" `
    -resultsFolderPath $resultsFolderPath `
    -logsFolderPath $logsFolderPath `
    -nugetFoldersPath $nugetFoldersPath `
    -iterationCount $iterationCount 
}