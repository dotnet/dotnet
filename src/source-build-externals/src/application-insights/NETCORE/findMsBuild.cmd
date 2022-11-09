@echo off

IF NOT DEFINED VSVERSION SET VSVERSION=15.0

IF DEFINED MSBUILD (
  IF EXIST "%MSBUILD%" GOTO :eof
)

SET VSWHERE=..\packages\vswhere\tools\vswhere.exe
IF NOT EXIST "%VSWHERE%" nuget.exe install vswhere -NonInteractive -ExcludeVersion -Source https://www.nuget.org/api/v2 -OutputDirectory ..\packages> nul

FOR /f "usebackq tokens=*" %%i in (`"%VSWHERE%" -version %VSVERSION% -products * -requires Microsoft.Component.MSBuild -property installationPath`) DO (
  SET MSBUILD=%%i\MSBuild\%VSVERSION%\Bin\MSBuild.exe
)

IF NOT DEFINED MSBUILD (
  ECHO Could not find MSBuild %VSVERSION%. Please SET MSBUILD=^<path-to-MSBuild.exe^> and try again.
  GOTO :eof
)

IF NOT EXIST "%MSBUILD%" (
  ECHO vswhere.exe claims that MSBuild is at !MSBUILD! but it does not exist.
  ECHO Please SET MSBUILD=^<path-to-MSBuild.exe^> and try again.
  GOTO :eof
)

ECHO Using MSBuild from %MSBUILD%
