# SdkTests do not currently work with globally installed CLI as they use dotnet-install.ps1 to install more runtimes

$script:useInstalledDotNetCli = $false

# Add CMake to the path.
$env:PATH = "$PSScriptRoot\..\.tools\bin;$env:PATH"
