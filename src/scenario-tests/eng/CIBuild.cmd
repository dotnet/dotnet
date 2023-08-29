@echo off
powershell -ExecutionPolicy ByPass -command "& """%~dp0common\Build.ps1""" -restore -build -test -pack -ci -publish %*"
exit /b %ErrorLevel%
