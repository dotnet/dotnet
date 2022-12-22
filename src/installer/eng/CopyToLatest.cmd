@echo off

REM Copyright (c) .NET Foundation and contributors. All rights reserved.
REM Licensed under the MIT license. See LICENSE file in the project root for full license information.

powershell -ExecutionPolicy Bypass -NoProfile -NoLogo -Command "& \"%~dp0common\build.ps1\" -restore -build /p:Projects=\"%~dp0..\src\CopyToLatest\CopyToLatest.csproj\" %*; exit $LastExitCode;"
if %errorlevel% neq 0 exit /b %errorlevel%
