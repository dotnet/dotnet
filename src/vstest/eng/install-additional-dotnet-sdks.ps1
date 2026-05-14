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
$sdkArchitectures = @("x86")

foreach ($sdkArchitecture in $sdkArchitectures) { 
    $version = $globalJson.sdk.version
    $dotnetRoot = "$DotnetInstallDir/dotnet-sdk-$sdkArchitecture"

    InstallDotNetSdk -dotnetRoot $dotnetRoot -version $version -architecture $sdkArchitecture -noPath
}