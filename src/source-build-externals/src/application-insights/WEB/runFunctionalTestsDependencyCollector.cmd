@echo off
@setlocal enabledelayedexpansion enableextensions


CALL powershell -NoProfile -ExecutionPolicy Bypass -File  .\dockercleanup.ps1

CALL buildReleaseFull.cmd

set BuildRoot=%~dp0..\bin\Release\
set VSTestPath=%PROGRAMFILES(x86)%\Microsoft Visual Studio\2017\Enterprise\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe

CALL "%VSTestPath%" "%BuildRoot%Test\E2ETests\E2ETests\DependencyCollectionTests.dll" /TestCaseFilter:"TestCategory=Core20" /logger:trx

# CALL "%VSTestPath%" "%BuildRoot%Test\E2ETests\E2ETests\DependencyCollectionTests.dll" /TestCaseFilter:"TestCategory=Core30" /logger:trx

CALL "%VSTestPath%" "%BuildRoot%Test\E2ETests\E2ETests\DependencyCollectionTests.dll" /TestCaseFilter:"TestCategory=Net452OnNet462" /logger:trx

CALL "%VSTestPath%" "%BuildRoot%Test\E2ETests\E2ETests\DependencyCollectionTests.dll" /TestCaseFilter:"TestCategory=Net452OnNet462SM" /logger:trx

CALL "%VSTestPath%" "%BuildRoot%Test\E2ETests\E2ETests\DependencyCollectionTests.dll" /TestCaseFilter:"TestCategory=Net452OnNet471" /logger:trx

PAUSE
