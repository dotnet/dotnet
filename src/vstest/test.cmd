@echo off

REM Copyright (c) Microsoft. All rights reserved.

powershell -ExecutionPolicy Bypass -NoProfile -NoLogo -Command "%~dp0scripts\test.ps1 %*; exit $LastExitCode;" 
if %errorlevel% neq 0 exit /b %errorlevel% 
