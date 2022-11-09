#enable verbose mode
$VerbosePreference = "Continue";

$TestProjects = @(
	'.\test\Microsoft.ApplicationInsights.AspNetCore.Tests',
	'.\test\MVCFramework45.FunctionalTests',
	'.\test\WebApiShimFw46.FunctionalTests'
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
	If ($p.ExitCode -ne 0) {
      $global:failed += $executeResult;
	}
}

Push-Location

[PSObject[]]$global:failed = @();
$global:WorkingDirectory = (pwd).Path;

$dotnetPath = "C:\Program Files\dotnet\dotnet.exe";

$TestProjects |% {
	[String]$arguments = "test";
	[String]$currentWorkingDirectory = Join-Path $global:WorkingDirectory -ChildPath $_;
	Write-Host "=========================================================";
	Write-Host "== Executing tests";
	Write-Host "== Working Folder: $currentWorkingDirectory";
	Write-Host "== Runtime:$dotnetPath";
	Write-Host "== Args:$arguments";
	Write-Host "=========================================================";
    Set-Location -Path $currentWorkingDirectory;
	$executeResult = Execute-DotnetProcess `
	-RuntimePath $dotnetPath `
	-Arguments $arguments `
	-WorkingDirectory $currentWorkingDirectory;
}

Pop-Location

If ($global:failed.Count -gt 0) {
	Throw "Test execution failed";
}