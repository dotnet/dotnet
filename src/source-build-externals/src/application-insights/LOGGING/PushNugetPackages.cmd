setlocal

if "%NUGET_KEY%"=="" SET NUGET_KEY=%1

echo "key: '%NUGET_KEY%'"

set BINROOT=%TF_BUILD_BINARIESDIRECTORY%\Release\NuGet
if not exist %BINROOT% echo "Error: '%BINROOT%' does not exist."&goto :eof

set NUGET=%TF_BUILD_SOURCESDIRECTORY%\NuGet.exe
if not exist %NUGET% echo "Error: '%NUGET%' does not exist."&goto :eof

set NUGET_GALLERY=https://www.myget.org/F/applicationinsights/api/v2/package

for /r "%BINROOT%" %%P in (*.nupkg) do call :push %%P
goto :eof

:push 
set PACKAGE=%1
if %PACKAGE:.symbols.=% == %PACKAGE% (
    %NUGET% push "%PACKAGE%" %NUGET_KEY% -source %NUGET_GALLERY%
)
goto :eof

endlocal
