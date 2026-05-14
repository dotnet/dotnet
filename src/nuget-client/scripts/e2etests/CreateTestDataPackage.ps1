<#
.SYNOPSIS
Creates a package containing NuGet client EndToEnd test data.

.PARAMETER configuration
The build configuration.  The default value is 'debug'.

.PARAMETER toolsetVersion
The toolset version.  The default value is 15.

.PARAMETER outputDirectoryPath
The output directory where the test data package will be created.  The default value is the current directory.

.EXAMPLE
.\CreateTestDataPackage.ps1 -configuration 'debug' -toolsetVersion 15 -outputDirectoryPath 'C:\git\NuGet.Client\artifacts\nupkgs'
#>

[CmdletBinding()]
param (
    [ValidateSet('debug', 'release')]
    [string] $configuration = 'debug',
    [ValidateSet(15, 16, 17)]
    [int] $toolsetVersion = 15,
    [string] $outputDirectoryPath = $PWD
)

. "$PSScriptRoot\..\common.ps1"

$packageId = 'NuGet.Client.EndToEnd.TestData'

Function Get-DirectoryPath([string[]] $pathParts)
{
    Return [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($pathParts))
}

Set-Variable repositoryRootDirectoryPath -Option Constant -Value $(Get-DirectoryPath($PSScriptRoot, '..', '..'))

Function Get-Directory([string[]] $pathParts)
{
    $directoryPath = Get-DirectoryPath($pathParts)

    Return [System.IO.DirectoryInfo]::new($directoryPath)
}

Function Get-File([string[]] $pathParts)
{
    $filePath = [System.IO.Path]::Combine($pathParts)

    $file = [System.IO.FileInfo]::new($filePath)

    If (-Not $file.Exists)
    {
        throw [System.IO.FileNotFoundException]::new("Could not find $($file.Name) at $($file.FullName).  Please build first.", $file.FullName)
    }

    Return $file
}

Function Get-GenerateTestPackagesFile()
{
    Return Get-File($repositoryRootDirectoryPath, 'artifacts', 'GenerateTestPackages', "$toolsetVersion.0", 'bin', $configuration, 'net472', 'GenerateTestPackages.exe')
}

Function Get-NuGetFile()
{
    Return Get-File($repositoryRootDirectoryPath, 'artifacts', 'VS15', 'nuget.exe')
}

Function Create-TestPackages(
    [Parameter(Mandatory = $True)]  [System.IO.DirectoryInfo] $sourceDirectory,
    [Parameter(Mandatory = $False)] [System.IO.DirectoryInfo] $destinationDirectory)
{
    $generateTestPackagesFile = Get-GenerateTestPackagesFile
    $testDirectories = $sourceDirectory.GetDirectories()

    $testDirectories | %{
        $testDirectory = $_
        $packagesDirectory = Get-Directory($testDirectory.FullName, 'Packages')
        $assembliesDirectory = Get-Directory($testDirectory.FullName, 'Assemblies')

        Remove-Item -Path $packagesDirectory.FullName -Recurse -Force -ErrorAction Ignore
        Remove-Item -Path $assembliesDirectory.FullName -Recurse -Force -ErrorAction Ignore

        Get-ChildItem "$($testDirectory.FullName)\*" -Include *.dgml,*.nuspec | %{
            $file = $_

            Write-Host "Running $($generateTestPackagesFile.Name) on $($file.FullName)...  " -NoNewLine

            $startInfo = [System.Diagnostics.ProcessStartInfo]::new()
            $startInfo.FileName = $generateTestPackagesFile.FullName
            $startInfo.WorkingDirectory = $testDirectory.FullName
            $startInfo.UseShellExecute = $False
            $startInfo.RedirectStandardError = $True
            $startInfo.RedirectStandardOutput = $True
            $startInfo.Arguments = $file.FullName

            $process = [System.Diagnostics.Process]::new()
            $process.StartInfo = $startInfo
            $process.Start() | Out-Null
            $process.WaitForExit()

            $stdout = $process.StandardOutput.ReadToEnd()
            $stderr = $process.StandardError.ReadToEnd()

            If ($process.ExitCode -eq 0)
            {
                Write-Host 'Success.'

                If ($destinationDirectory)
                {
                    $packagesDirectory.GetFiles('*.nupkg') | %{
                        $packageFile = $_

                        Move-Item -Path $packageFile.FullName -Destination $destinationDirectory.FullName -Force
                    }

                    Remove-Item -Path $packagesDirectory.FullName -Recurse -Force -ErrorAction Ignore
                }
            }
            else
            {
                Write-Error "Failed.  Exit code is $($process.ExitCode)."
                Write-Host "Output stream: $stdout"
                Write-Host "Error stream: $stderr"
            }
        }

        Remove-Item -Path $assembliesDirectory.FullName -Recurse -Force -ErrorAction Ignore
    }
}

Function Create-TestDataPackage([Parameter(Mandatory = $True)] [System.IO.FileInfo] $nuspecFile)
{
    $nugetFile = Get-NuGetFile

    $outputDirectory = [System.IO.DirectoryInfo]::new([System.IO.Path]::GetFullPath($outputDirectoryPath))

    $outputDirectory.Create()

    $process = Start-Process `
        -FilePath $nugetFile.FullName `
        -WorkingDirectory $nuspecFile.DirectoryName `
        -WindowStyle Hidden `
        -PassThru `
        -Wait `
        -ArgumentList 'pack', $nuspecFile.FullName, "-OutputDirectory `"$($outputDirectory.FullName)`"", '-NoDefaultExcludes', '-NonInteractive'

    If ($process.ExitCode -eq 0)
    {
        $packageFiles = $outputDirectory.GetFiles("$packageId`.*.nupkg", [System.IO.SearchOption]::TopDirectoryOnly)
        $packageFile = $packageFiles[0]

        Write-Host "Created test data package at $($packageFile.FullName)."
    }
    else
    {
        Write-Error "$($nugetFile.Name) failed.  Exit code is $($process.ExitCode)."
    }
}

$workingDirectoryPath = New-TempDir
$workingDirectory = [System.IO.DirectoryInfo]::new($workingDirectoryPath)

Try
{
    $sourceDirectory = Get-Directory($repositoryRootDirectoryPath, 'test', 'EndToEnd', 'Packages')
    $packagesDirectory = Get-Directory($WorkingDirectory.FullName, 'Packages')

    Write-Verbose "Copying all test data from '$TestSource' to '$($packagesDirectory.FullName)'"
    & robocopy $sourceDirectory.FullName $packagesDirectory.FullName /MIR

    # RoboCopy returns a variety of error codes.  This is the only one we care about and it means "copy completed successfully";
    # however, to PowerShell a non-zero exit code is a failure.
    If ($LASTEXITCODE -eq 1)
    {
        $LASTEXITCODE = 0
    }

    $nuspecFile = Get-File($repositoryRootDirectoryPath, 'test', 'EndToEnd', "$packageId.nuspec")

    $nuspecFile = Copy-Item -Path $nuspecFile.FullName -Destination $workingDirectory.FullName -Force -PassThru
    Write-Host $nuspecFile.FullName
    Create-TestPackages -sourceDirectory $packagesDirectory

    $sharedDirectory = Get-Directory($packagesDirectory.FullName, '_Shared')
    Create-TestPackages -sourceDirectory $sharedDirectory -destinationDirectory $workingDirectory

    Create-TestDataPackage $nuspecFile
}
Finally
{
    Remove-Item -Path $workingDirectory.FullName -Recurse -Force -ErrorAction Ignore
}