#enable verbose mode
$VerbosePreference = "Continue";


$Projects = @(
    '.\src\Microsoft.ApplicationInsights.AspNetCore',
    '.\test\FunctionalTestUtils'
	'.\test\Microsoft.ApplicationInsights.AspNetCore.Tests',
	'.\test\MVCFramework45.FunctionalTests',
	'.\test\WebApiShimFw46.FunctionalTests',
    '.\test\EmptyApp.FunctionalTests'
)

$Commands = @(
    'restore',
    'build -c Release',
    'pack -c Release'
)

Function Execute-DotnetProcess {
	Param (
		[Parameter(Mandatory=$True)]
		[String]$RuntimePath,
		[Parameter(Mandatory=$True)]
		[String]$Arguments,
		[Parameter(Mandatory=$True)]
		[String]$WorkingDirectory
	)

	$p = Start-Process $RuntimePath $Arguments -PassThru -NoNewWindow -Wait;

    Write-Host "Process executed, ExitCode:$($p.ExitCode)";
	Write-Host "Output:";
	Write-Host $p.StandardOutput;
	If (($Arguments -eq 'build') -and ($p.ExitCode -ne 0)) {
      $global:failed += $executeResult;
	}
}

[PSObject[]]$global:failed = @();
$global:WorkingDirectory = (pwd).Path;

$dotnetPath = 'C:\Program Files\dotnet\dotnet.exe';

$Projects |% {
	[String]$currentWorkingDirectory = Join-Path $global:WorkingDirectory -ChildPath $_;
    $Commands |% {
        $command = $_;
        Write-Host "=========================================================";
	    Write-Host "== Restoring and building the projects";
	    Write-Host "== Working Folder: $currentWorkingDirectory";
	    Write-Host "== Runtime:$dotnetPath";
	    Write-Host "== Args:$command";
	    Write-Host "=========================================================";
        Set-Location -Path $currentWorkingDirectory;
	    $executeResult = Execute-DotnetProcess `
	    -RuntimePath $dotnetPath `
	    -Arguments $command `
	    -WorkingDirectory $currentWorkingDirectory;	    
    }
}

If ($global:failed.Count -gt 0) {
	Throw "Test execution failed";
}
