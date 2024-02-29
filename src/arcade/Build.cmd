@echo off
pwsh -ExecutionPolicy ByPass -NoProfile -command "& """%~dp0eng\common\Build.ps1""" -restore -build %*"
