@echo off
powershell -ExecutionPolicy ByPass -NoProfile -File "%~dp0common\build.ps1" -build -restore %*
