cd /D "%~dp0..\"

dotnet restore .\ProjectsForSigning.sln || exit /b 1
