@echo off
.dotnet\dotnet test test\Microsoft.TestTemplates.Acceptance.Tests.sln --logger:trx
exit /b %ErrorLevel%
