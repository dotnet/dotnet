cd /D "%~dp0..\"

dotnet pack .\ProjectsForSigning.sln %* --configuration Release --no-restore --no-build || exit /b 1


setlocal enabledelayedexpansion
powershell.exe -ExecutionPolicy Unrestricted -NoProfile -WindowStyle Hidden -File "./.scripts/release_GenerateReleaseMetadata.ps1" -artifactsPath "./bin/Release/NuGet" -sourcePath "./" -outPath "./bin/Release/NuGet"
endlocal
exit /B %ERRORLEVEL%
