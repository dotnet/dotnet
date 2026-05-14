param (
    [Parameter(Mandatory = $True)]
    [string] $NuGetPackageManagementPowerShellCmdletsFilePath,
    [Parameter(Mandatory = $True)]
    [string] $ManifestModuleSourceFilePath,
    [Parameter(Mandatory = $True)]
    [string] $ManifestModuleDestinationFilePath)

trap
{
    Write-Error ($_.Exception | Format-List -Force | Out-String) -ErrorAction Continue
    Write-Error ($_.InvocationInfo | Format-List -Force | Out-String) -ErrorAction Continue
    exit 1
}

$assembly = [System.Reflection.Assembly]::LoadFile($NuGetPackageManagementPowerShellCmdletsFilePath)

[string] $manifestModuleFileContents = [System.IO.File]::ReadAllText($ManifestModuleSourceFilePath)

$from = 'NuGet.PackageManagement.PowerShellCmdlets.dll'
$to = $assembly.FullName

Write-Host "Source file:  $ManifestModuleSourceFilePath"
Write-Host "Destination file:  $ManifestModuleSourceFilePath"
Write-Host "Replacing `'$from`' with `'$to`'"

$startIndex = $manifestModuleFileContents.IndexOf($from)

If ($startIndex -eq -1)
{
    Throw [System.Exception]::new("No occurrences of `'$from`' found in $ManifestModuleSourceFilePath")
}

$manifestModuleFileContents = $manifestModuleFileContents.Replace($from, $to)

$startIndex = $manifestModuleFileContents.IndexOf($from, $startIndex + 1)

If ($startIndex -ne -1)
{
    Throw [System.Exception]::new("Extra occurrences of `'$from`' found in $ManifestModuleSourceFilePath")
}

$ManifestModuleDestinationFile = [System.IO.FileInfo]::new($ManifestModuleDestinationFilePath)

[System.IO.Directory]::CreateDirectory($ManifestModuleDestinationFile.DirectoryName) | Out-Null

[System.IO.File]::WriteAllText($ManifestModuleDestinationFilePath, $manifestModuleFileContents)
