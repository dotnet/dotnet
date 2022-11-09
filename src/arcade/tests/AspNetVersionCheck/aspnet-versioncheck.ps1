Param(
  [string] $aspNetCoreVersion
)

$versionRegex = "(?<version>\d+\.\d+)\.\d+(?:-(?<preview>\w+\.\d+)\.\d+\.\d+)?"

$dotnetVersion = & dotnet --version
$dotNetSdkMatch = $dotnetVersion -Match $versionRegex
if ($dotNetSdkMatch -eq $false) {
  Write-Host ".NET SDK version '$dotnetVersion' did not match regex!"
  exit 2
}
$dotnetMajorVersion = $Matches.version
$dotnetPreviewVersion = $Matches.preview
Write-Host ".NET SDK '$dotnetVersion' -- major version: '$dotnetMajorVersion'; preview: '$dotnetPreviewVersion'"

$aspNetCoreMatch = $aspNetCoreVersion -Match $versionRegex
if ($aspNetCoreMatch -eq $false) {
  Write-Host "ASP.NET Core Runtime version '$aspNetCoreVersion' did not match regex!"
  exit 3
}
$aspNetCoreMajorVersion = $Matches.version
$aspNetCorePreviewVersion = $Matches.preview
Write-Host "ASP.NET SDK '$aspNetCoreVersion' -- major version: '$aspNetCoreMajorVersion'; preview: '$aspNetCorePreviewVersion'"

if ($dotnetMajorVersion -ne $aspNetCoreMajorVersion -or $dotnetPreviewVersion -ne $aspNetCorePreviewVersion) {
    Write-Host "ASP.NET Core Runtime version and .NET Core Runtime version do not match -- update DotNetCli.props!"
    exit 1
}