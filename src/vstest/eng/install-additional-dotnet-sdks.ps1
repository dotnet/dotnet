param ( 
    [Parameter(Mandatory)]
    [string] $DotnetInstallDir,
    [Parameter(Mandatory)]
    [string] $RepoRoot
)

Write-Host "Installing additional dotnet SDKs for integration tests."

$eng = $PSScriptRoot
. $eng/common/tools.ps1

$globalJson = Get-Content $RepoRoot/global.json | ConvertFrom-Json
$sdkVersions = @("x86")

foreach ($architecture in $sdkVersions) { 
    $version = $globalJson.sdk.version
    $dotnetRoot = "$DotnetInstallDir/dotnet-sdk-$architecture"

    InstallDotNetSdk -dotnetRoot $dotnetRoot -version $version -architecture $architecture -noPath

    $runtimeVersions = @($globalJson.tools.runtimes."dotnet/$($architecture)")
    foreach ($runtimeVersion in $runtimeVersions) { 
        if ($runtimeVersion -like "8.*") { 
            $stop= $true
        }
        InstallDotNet -runtime "dotnet" -dotnetRoot $dotnetRoot -version $runtimeVersion -architecture $architecture -noPath
    }
}