@echo off
setlocal

echo Cleaning up any existing build processes...
for /f "tokens=2" %%i in ('tasklist /fi "imagename eq dotnet.exe" /fo table /nh 2^>nul ^| findstr /i "WindowsDesktop"') do (
    echo Killing process %%i
    taskkill /f /pid %%i >nul 2>&1
)

echo Starting sequential build...
powershell -ExecutionPolicy ByPass -NoProfile -command "& """%~dp0eng\common\Build.ps1""" -nodeReuse:$false -maxCpuCount:1 -restore -build %*"

echo Build completed. Cleaning up any remaining processes...
for /f "tokens=2" %%i in ('tasklist /fi "imagename eq dotnet.exe" /fo table /nh 2^>nul ^| findstr /i "WindowsDesktop"') do (
    echo Killing remaining process %%i
    taskkill /f /pid %%i >nul 2>&1
)