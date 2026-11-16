@echo off
powershell -ExecutionPolicy ByPass -NoProfile -command "& """%~dp0..\common\build.ps1""" -restore -build -projects """%~dp0dotnet-build.proj""" %*"
