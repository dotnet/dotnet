@echo off
powershell -NoLogo -ExecutionPolicy ByPass -NoProfile -command "& """%~dp0build.ps1"""%*"
exit /b %ErrorLevel%
