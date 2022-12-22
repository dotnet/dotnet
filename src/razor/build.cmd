@echo off
setlocal

set _args=-restore -build %*
if "%~1"=="-?" set _args=-help

powershell -ExecutionPolicy ByPass -NoProfile -Command "& '%~dp0eng\common\build.ps1'" %_args%
exit /b %ERRORLEVEL%
