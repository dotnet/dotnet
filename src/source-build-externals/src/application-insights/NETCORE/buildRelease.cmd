@echo off

IF NOT DEFINED MSBUILD CALL findMsBuild.cmd

SET ToolsVersion=15.0
SET ProjectName=Msbuild.All
SET Configuration=Release
SET Platform="Any CPU"

"%MSBUILD%" dirs.proj /nologo /m:1  /fl /toolsversion:%ToolsVersion% /flp:logfile=%ProjectName%.%Platform%.log;v=d /flp1:logfile=%ProjectName%.%Platform%.wrn;warningsonly /flp2:logfile=%ProjectName%.%Platform%.err;errorsonly /p:Configuration=%Configuration% /p:Platform=%Platform% /p:RunCodeAnalysis="False" /flp3:logfile=%ProjectName%.%Platform%.prf;performancesummary /flp4:logfile=%ProjectName%.%Platform%.exec.log;showcommandline /p:BuildSingleFilePackage=true /p:IsOfficialBuild=%IsOfficialBuild% /p:RunTests=False

pause