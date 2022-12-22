# SdkTests do not currently work with globally installed CLI as they use dotnet-install.ps1 to install more runtimes

useInstalledDotNetCli="false"

# Working around issue https://github.com/dotnet/arcade/issues/7327
DisableNativeToolsetInstalls=true