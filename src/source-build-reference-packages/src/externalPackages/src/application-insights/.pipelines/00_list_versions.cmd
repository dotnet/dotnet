cd /D "%~dp0..\"

dotnet --version && dotnet --list-sdks && dotnet --list-runtimes || exit /b 1
