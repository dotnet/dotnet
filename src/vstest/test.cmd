@echo off
if not defined MSBUILDTERMINALLOGGER set MSBUILDTERMINALLOGGER=off
powershell -ExecutionPolicy ByPass -NoProfile -File "%~dp0eng\build.ps1" -test %*
exit /b %ErrorLevel%
