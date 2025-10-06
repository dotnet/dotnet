<#
.SYNOPSIS
Runs the set of performance tests in the test cases directory. 

.PARAMETER resultsFolderPath
The results folder path

.PARAMETER nugetClientFilePaths
An array of NuGet clients to run the tests for. Supported are dotnet.exe and NuGet.exe clients.

.PARAMETER testRootFolderPath
The test root folder path. All temporary assets will be stored here. That includes the download of repos under a 'source' subfolder and the NuGet folders, under an 'np' subfolder.

.PARAMETER logsFolderPath
The logs folder path. 

.PARAMETER iterationCount
How many times to run each test. The default is 3

.PARAMETER skipRepoCleanup
Whether to delete the checked out repos from the test cases.

.PARAMETER additionalOptions
Additional options such as ones to skip certain runs like -skipWarmup or maybe run the local version of NuGet, -useLocallyBuiltNuGet. 
To get the list of all options check out RunPerformanceTests.ps1 in the same folder.

.EXAMPLE
.\PerformanceTestRunner.ps1 -resultsFolderPath resultsFolder -nugetClientFilePaths F:\NuGetExe\NuGet.exe,"C:\Program Files\dotnet\dotnet.exe" 

.EXAMPLE
.\PerformanceTestRunner.ps1 -resultsFolderPath resultsFolder -nugetClientFilePaths "C:\Program Files\Microsoft Visual Studio\2022\Preview\MSBuild\Current\Bin\amd64\MSBuild.exe" -skipRepoCleanup -additionalOptions "-useLocallyBuiltNuGet -staticGraphRestore"

.EXAMPLE
.\PerformanceTestRunner.ps1 -resultsFolderPath resultsFolder -nugetClientFilePaths "C:\Program Files\Microsoft Visual Studio\2022\Preview\MSBuild\Current\Bin\amd64\MSBuild.exe" -skipRepoCleanup -additionalOptions "-useLocallyBuiltNuGet"

#>
Param(
    [Parameter(Mandatory = $true)]
    [string] $resultsFolderPath,
    [Parameter(Mandatory = $true)]
    [string[]] $nugetClientFilePaths,
    [string] $testRootFolderPath,
    [string] $logsFolderPath,
    [int] $iterationCount = 3,
    [string] $testCaseDirectory,
    [switch] $skipRepoCleanup,
    [string] $additionalOptions
)

. "$PSScriptRoot\PerformanceTestUtilities.ps1"

if([string]::IsNullOrEmpty($testRootFolderPath))
{
    $testRootFolderPath = GetDefaultNuGetTestFolder
} 
else 
{
    $testRootFolderPath = GetAbsolutePath $testRootFolderPath
}


$nugetFoldersPath = GetNuGetFoldersPath $testRootFolderPath
$resultsFolderPath = GetAbsolutePath $resultsFolderPath

$sourceRootFolderPath = [System.IO.Path]::Combine($testRootFolderPath, "source")

if([string]::IsNullOrEmpty($logsFolderPath))
{
    $deleteLogs = $True
    $logsFolderPath = [System.IO.Path]::Combine($testRootFolderPath, "logs")
} 
else 
{
    $deleteLogs = $False
    $logsFolderPath = GetAbsolutePath $logsFolderPath
}

If ([System.IO.Path]::GetDirectoryName($resultsFolderPath).StartsWith($nugetFoldersPath))
{
    Log "$resultsFolderPath cannot be a subdirectory of $nugetFoldersPath" "red"

    Exit 1
}

Log "Clients: $nugetClientFilePaths" "green"

Try
{
    ForEach ($nugetClientFilePath In $nugetClientFilePaths)
    {
        Try
        {
            Log "Running tests for $nugetClientFilePath" "green"

            If ([string]::IsNullOrEmpty($nugetClientFilePath) -Or !$(Test-Path $nugetClientFilePath))
            {
                Log "The NuGet client at '$nugetClientFilePath' cannot be resolved." "yellow"

                Exit 1
            }

            $testCasesDirectory = "$PSScriptRoot\testCases"
            If (-not [string]::IsNullOrWhiteSpace($testCaseDirectory))
            {
                $testCasesDirectory = $testCaseDirectory
            }

            Log "Discovering the test cases in $testCasesDirectory"
            $testFiles = $(Get-ChildItem $testCasesDirectory "Test-*.ps1" ) | ForEach-Object { $_.FullName }
            Log "Discovered test cases: $testFiles" "green"
            
            $testFiles | ForEach-Object {
                $testCase = $_
                Try
                {
                    . $_ `
                        -nugetClientFilePath $nugetClientFilePath `
                        -sourceRootFolderPath $sourceRootFolderPath `
                        -resultsFolderPath $resultsFolderPath `
                        -logsFolderPath $logsFolderPath `
                        -nugetFoldersPath $nugetFoldersPath `
                        -iterationCount $iterationCount `
                        -additionalOptions $additionalOptions
                }
                Catch
                {
                    Log "Problem running the test case $testCase with error $_" "red"
                }
            }
        }
        Catch
        {
            Log "Problem running the tests with $nugetClientFilePath" "red"
        }
    }
}
Finally
{
    Remove-Item -Recurse -Force $nugetFoldersPath -ErrorAction Ignore > $Null
    if($deleteLogs)
    {
        Remove-Item -Recurse -Force $logsFolderPath -ErrorAction Ignore > $Null
    }

    If (!$skipRepoCleanup)
    {
        Remove-Item -Recurse -Force $sourceRootFolderPath -ErrorAction Ignore > $Null
    }
}
