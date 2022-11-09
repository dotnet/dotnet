<#
.SYNOPSIS
Creates end-to-end test package for test pass

.PARAMETER Configuration
API.Test build configuration to place in test package. Debug by default.

.PARAMETER OutputDirectory
Output directory where EndToEnd.zip package file will be created.
Will use current directory if not provided.

.PARAMETER NuGetRoot
Optional. NuGet.Client repository root
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory=$False)]
    [Alias('c')]
    [string]$Configuration = 'Debug',
    [Parameter(Mandatory=$False)]
    [Alias('out')]
    [string]$OutputDirectory = $PWD,
    [Parameter(Mandatory=$False)]
    [string]$NuGetRoot
)

. "$PSScriptRoot\..\common.ps1"

if (-not $NuGetRoot -and (Test-Path Env:\NuGetRoot)) {
    $NuGetRoot = $env:NuGetRoot
}

if (-not $NuGetRoot) {
    $NuGetRoot = Join-Path $PSScriptRoot '..\..\' -Resolve
}

$WorkingDirectory = New-TempDir

$opts = '/s', '/z', '/r:3', '/w:30', '/np', '/nfl'

if ($VerbosePreference) {
    $opts += '/v'
}
else {
    $opts += '/ndl', '/njs'
}

Function Get-TestDataPackageDirectory()
{
    $packagesConfigFilePath = [System.IO.Path]::Combine($NuGetRoot, 'build', 'bootstrap.proj')

    [System.Xml.XmlDocument] $xml = Get-Content $packagesConfigFilePath

    $package = $xml.SelectSingleNode('//Project/ItemGroup/PackageDownload[@Include="NuGet.Client.EndToEnd.TestData"]')
    $pkgId = $package.Include
    $pkgVersion = $package.Version.Trim('[', ']')

    $path = [System.IO.Path]::Combine($NuGetRoot, 'packages', $pkgId, $pkgVersion)

    Return [System.IO.DirectoryInfo]::new($path)
}

Function Run-RoboCopy(
    [Parameter(Mandatory = $True)]  [string] $sourceDirectoryPath,
    [Parameter(Mandatory = $True)]  [string] $destinationDirectoryPath,
    [Parameter(Mandatory = $False)] [string[]] $options)
{
    & robocopy $sourceDirectoryPath $destinationDirectoryPath $options

    # RoboCopy returns a variety of error codes.  0-3 are success; however, to PowerShell a non-zero exit code is a failure.
    If ($LastExitCode -lt 4)
    {
        $LastExitCode = 0
    }
    Else
    {
        Write-Error "Task failed while attempting to copy test files from $sourceDirectoryPath to $destinationDirectoryPath.  LastExitCode:  $LastExitCode"

        Exit 1
    }
}

try {
    $TestSource = Join-Path $NuGetRoot test\EndToEnd -Resolve
    Write-Verbose "Copying all test files from '$TestSource' to '$WorkingDirectory'"

    # Copy everything except the /Packages directory.
    # Instead, the /Packages directory will be copied from the NuGet.Client.EndToEnd.TestData package.
    Run-RoboCopy $TestSource $WorkingDirectory $($opts + '/XD' + 'Packages')

    $testDataPackageDirectory = Get-TestDataPackageDirectory

    $TestSource = [System.IO.Path]::Combine($testDataPackageDirectory.FullName, 'content', 'Packages')
    $packagesDirectory = Join-Path $WorkingDirectory 'Packages'
    Write-Verbose "Copying all test data from '$TestSource' to '$packagesDirectory'"
    Run-RoboCopy $TestSource $packagesDirectory $opts

    $TestExtensionDirectoryPath = Join-Path $NuGetRoot "artifacts\API.Test\bin\${Configuration}\net472"
    if (!(Test-Path "$TestExtensionDirectoryPath\API.Test.dll"))
    {
        $errorMessage = "API.Test binaries not found at $TestExtensionDirectoryPath\API.Test.dll. Make sure the project has been built."
        Write-Output $errorMessage
        throw $errorMessage
    }
    Write-Verbose "Copying test extension from '$TestExtensionDirectoryPath' to '$WorkingDirectory'"
    Run-RoboCopy $TestExtensionDirectoryPath $WorkingDirectory $(@('API.Test.*') + $opts)

    $GeneratePackagesUtil = Join-Path $NuGetRoot "artifacts\GenerateTestPackages\bin\${Configuration}\net472"
    if (!(Test-Path "$GeneratePackagesUtil\GenerateTestPackages.exe"))
    {
        $errorMessage = "GenerateTestPackages binaries not found at $GeneratePackagesUtil\GenerateTestPackages.exe. Make sure the project has been built."
        Write-Output $errorMessage
        throw $errorMessage
    }
    Write-Verbose "Copying utility binaries from `"$GeneratePackagesUtil`" to `"$WorkingDirectory`""
    Run-RoboCopy $GeneratePackagesUtil $WorkingDirectory $(@('*.exe', '*.dll', '*.pdb') + $opts)

    $ScriptsDirectory = Join-Path $WorkingDirectory scripts
    New-Item -ItemType Directory -Force -Path $ScriptsDirectory | Out-Null

    $ScriptsSource = Join-Path $NuGetRoot Scripts\e2etests -Resolve
    Write-Verbose "Copying test scripts from '$ScriptsSource' to '$ScriptsDirectory'"
    Run-RoboCopy $ScriptsSource $ScriptsDirectory $(@('*.ps1') + $opts)
    Copy-Item -Path (Join-Path $NuGetRoot scripts\utils\PostGitCommitStatus.ps1) -Destination $ScriptsDirectory

    if (-not (Test-Path $OutputDirectory)) {
        mkdir $OutputDirectory | Out-Null
    }

    $TestPackage = Join-Path $OutputDirectory EndToEnd.zip
    Write-Verbose "Creating test package '$TestPackage'"
    Remove-Item $TestPackage -Force -ea Ignore | Out-Null
    Compress-Archive -Path "$WorkingDirectory\*" -DestinationPath $TestPackage -CompressionLevel Optimal

    Write-Output "Created end-to-end test package at '$TestPackage'"
}
finally {
    Remove-Item $workingDirectory -r -Force -WhatIf:$false
    exit 0
}
