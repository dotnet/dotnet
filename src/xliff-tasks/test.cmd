@echo off
powershell -NoLogo -NoProfile -ExecutionPolicy ByPass %~dp0eng\common\build.ps1 -test %*
exit /b %ErrorLevel%
