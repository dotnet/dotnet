@echo off
powershell -ExecutionPolicy ByPass -NoProfile -File "%~dp0eng\Build.ps1" -test %*
exit /b %ErrorLevel%