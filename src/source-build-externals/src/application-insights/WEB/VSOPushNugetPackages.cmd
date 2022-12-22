setlocal

if "%NUGET_KEY%"=="" SET NUGET_KEY=%1

if "%NUGET_GALLERY%"=="" SET NUGET_GALLERY=%2

set BINROOT=%BUILD_STAGINGDIRECTORY%\Release
if not exist %BINROOT% echo "Error: '%BINROOT%' does not exist."&goto :eof

set NUGET=%BUILD_SOURCESDIRECTORY%\NuGet.exe
if not exist %NUGET% echo "Error: '%NUGET%' does not exist."&goto :eof

for /r "%BINROOT%" %%P in (*.nupkg) do call :push %%P
goto :eof

:push 
set PACKAGE=%1
if %PACKAGE:.symbols.=% == %PACKAGE% (
    %NUGET% push "%PACKAGE%" %NUGET_KEY% -source %NUGET_GALLERY%
)

goto :eof

endlocal
