@echo off
@setlocal enabledelayedexpansion enableextensions

CALL buildDebug.cmd

set BuildRoot=%~dp0..\bin\debug\

set VSTestPath=%PROGRAMFILES(x86)%\Microsoft Visual Studio\2017\Enterprise\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe

CALL "%VSTestPath%" "%BuildRoot%Src\PerformanceCollector\Xdt.Tests\Xdt.Tests.dll" "%BuildRoot%Src\PerformanceCollector\Perf.NetFull.Tests\Microsoft.AI.PerformanceCollector.NetFull.Tests.dll" "%BuildRoot%Src\DependencyCollector\Net45.Tests\Microsoft.ApplicationInsights.DependencyCollector.Net45.Tests.dll" "%BuildRoot%Src\DependencyCollector\Nuget.Tests\Microsoft.ApplicationInsights.DependencyCollector.NuGet.Tests.dll" "%BuildRoot%Src\Web\Web.Net45.Tests\Microsoft.ApplicationInsights.Web.Net45.Tests.dll" "%BuildRoot%Src\Web\Web.Nuget.Tests\Microsoft.ApplicationInsights.Platform.Web.Nuget.Tests.dll" "%BuildRoot%Src\WindowsServer\WindowsServer.Net45.Tests\WindowsServer.Net45.Tests.dll" "%BuildRoot%Src\WindowsServer\WindowsServer.Nuget.Tests\WindowsServer.Nuget.Tests.dll" /logger:trx

set VSTestNetCorePath=%PROGRAMFILES(x86)%\Microsoft Visual Studio\2017\Enterprise\Common7\IDE\Extensions\TestPlatform\vstest.console.exe

CALL "%VSTestNetCorePath%" "%BuildRoot%Src\PerformanceCollector\NetCore.Tests\netcoreapp1.0\Microsoft.AI.PerformanceCollector.NetCore.Tests.dll" "%BuildRoot%Src\DependencyCollector\NetCore.Tests\netcoreapp1.0\Microsoft.AI.DependencyCollector.Tests.dll" "/Framework:FrameworkCore10" /logger:trx


PAUSE