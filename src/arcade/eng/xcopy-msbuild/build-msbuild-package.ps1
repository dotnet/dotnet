[CmdletBinding(PositionalBinding=$false)]
param (
    [Parameter(Mandatory=$true)][string]$buildToolsDir,
    [Parameter(Mandatory=$true)][string]$outputDirectory,
    [string]$packageName = "RoslynTools.MSBuild"
)

Set-StrictMode -version 2.0
$ErrorActionPreference="Stop"

function Print-Usage() {
    Write-Host "build-msbuild.ps1"
    Write-Host "`t-buildToolsDir path       Path to Build Tools Installation"
    Write-Host "`t-packageName              Name of the nuget package (RoslynTools.MSBuild)"
}

function Get-MSBuildFileInfo() {
    $fileInfo = New-Object IO.FileInfo $msbuildExe
    return $fileInfo
}
function Get-Description() {
    $fileInfo = Get-MSBuildFileInfo
    $sha = & git show-ref HEAD -s
    $text =
"
This is an xcopy version of MSBuild with the following version:

- Product Version: $($fileInfo.VersionInfo.ProductVersion)
- File Version: $($fileInfo.VersionInfo.FileVersion)

This is built using the following tool:

- Repo: https://github.com/dotnet/arcade/eng/xcopy-msbuild
- Source: https://github.com/dotnet/arcade/eng/xcopy-msbuild/commit/$($sha)
"
    return $text
}

function Create-ReadMe() {
    Write-Host "Creating README.md"
    $text = Get-Description
    $text | Out-File (Join-Path $outputDirectory "README.md")
}

function Create-Packages() {
    
    $text = Get-Description
    $nuget = Ensure-NuGet
    $fileInfo = Get-MSBuildFileInfo
    $packageVersion = $fileInfo.VersionInfo.FileVersion
    # Split the file version by '.' and take the first 3 components
    $buildNumber = $Env:BUILD_BUILDNUMBER
    if($buildNumber -ne $null)
    {
        $revision = $buildNumber.Substring($buildNumber.LastIndexOf('-') + 1)
    }
    else {
        $revision = '1'
    }
    $packageVersion = $packageVersion.Split('.')[0..2] -join '.'
    $packageVersion = "$packageVersion-$revision"
    Write-Host "Packing $packageName, version $packageVersion"
    & $nuget pack msbuild.nuspec -ExcludeEmptyDirectories -OutputDirectory $outputDirectory -Properties name=$packageName`;version=$packageVersion`;filePath=$outputBuildToolsDirectory`;description=$text
}

function Create-Directory([string]$dir) {
    New-Item $dir -ItemType Directory -ErrorAction SilentlyContinue | Out-Null
}

function Ensure-NuGet() {
    $nuget = Join-Path $outputDirectory "nuget.exe"
    if (-not (Test-Path $nuget)) {
        Create-Directory (Split-Path -parent $nuget)
        $version = "6.7.0"
        Write-Host "Downloading NuGet.exe $version"
        $webClient = New-Object -TypeName "System.Net.WebClient"
        $webClient.DownloadFile("https://dist.nuget.org/win-x86-commandline/v$version/NuGet.exe", $nuget)
    }

    return $nuget
}

function Get-PackagesDir() {
    $d = $null
    if ($env:NUGET_PACKAGES -ne $null) {
        $d = $env:NUGET_PACKAGES
    }
    else {
        $d = Join-Path $env:UserProfile ".nuget\packages\"
    }

    Create-Directory $d
    return $d
}

Push-Location $PSScriptRoot
try {
    $repoDir = $PSScriptRoot
    
    $msbuildDir = Join-Path $buildToolsDir "MSBuild\Current\Bin"
    $msbuildExe = Join-Path $msbuildDir "msbuild.exe"

    if (-not (Test-Path $msbuildExe)) { 
        Write-Host "Did not find msbuild at $msbuildExe"
        exit 1
    }

    $outputBuildToolsDirectory = Join-Path $outputDirectory "BuildTools"
    Create-Directory $outputBuildToolsDirectory -ErrorAction SilentlyContinue | Out-Null
    Remove-Item -re -fo "$outputBuildToolsDirectory\*"
    Write-Host "Copying Build Tools"
    Copy-Item -re "$buildToolsDir\*" $outputBuildToolsDirectory
    Create-ReadMe
    Create-Packages

    exit 0
}
catch [exception] {
    Write-Host $_
    Write-Host $_.Exception
    Write-Host $_.ScriptStackTrace
    exit 1
}
finally {
    Pop-Location
}
