@ECHO OFF
SETLOCAL

IF NOT [%1] == [] (set from=%1)
IF [%from%] == [] (
  echo The 'from' command line parameter is not set, aborting.
  exit /b 1
)

IF NOT [%2] == [] (set to=%2)
IF [%to%] == [] (
  echo The 'to' command line parameter is not set, aborting.
  exit /b 1
)

echo Copying from '%from%' to '%to'.

REM https://superuser.com/questions/280425/getting-robocopy-to-return-a-proper-exit-code
(robocopy %from% %to% /MIR) ^& IF %ERRORLEVEL% LSS 8 SET ERRORLEVEL = 0
