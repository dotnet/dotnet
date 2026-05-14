@echo off
powershell.exe -NoProfile -ExecutionPolicy RemoteSigned -Command "%~dpn0.ps1" %*
