
& $PSScriptRoot\eng\common\Build.ps1 -restore -build -test -pack $args
function ExitWithExitCode([int] $exitCode) {
    if ($ci -and $prepareMachine) {
      Stop-Processes
    }
    exit $exitCode
}

ExitWithExitCode $LASTEXITCODE