@echo off
@setlocal enabledelayedexpansion enableextensions

CALL buildReleaseFull.cmd		

set BuildRoot=%~dp0..\bin\Release\
set VSTestPath=%PROGRAMFILES(x86)%\Microsoft Visual Studio\2017\Enterprise\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe

CALL "%VSTestPath%" "%BuildRoot%Test\Web\FunctionalTests\FunctionalTests\Functional.dll" /logger:trx

PAUSE
