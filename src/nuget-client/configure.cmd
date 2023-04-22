@echo off
powershell.exe -NoProfile -ExecutionPolicy RemoteSigned -Command "%~dpn0.ps1" -SkipDotnetInfo %*
IF ERRORLEVEL 1 (
  EXIT /B %ERRORLEVEL%
)

SET "DOTNET_ROOT=%~dp0cli"
SET DOTNET_MULTILEVEL_LOOKUP=0
SET "PATH=%~dp0cli;%PATH%"
dotnet --info
IF ERRORLEVEL 1 (
    ECHO "dotnet --info exited with non-zero exit code" 1>&2
    EXIT /B 1
)