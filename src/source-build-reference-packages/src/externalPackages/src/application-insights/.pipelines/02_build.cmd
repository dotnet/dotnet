cd /D "%~dp0..\"

dotnet build .\ProjectsForSigning.sln %* --configuration Release --no-restore || exit /b 1
