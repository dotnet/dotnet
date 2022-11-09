# execute this first: Set-ExecutionPolicy -ExecutionPolicy Unrestricted -Scope Process -Confirm 

function GetUrlStatusCode($url)
{
	$req = [system.Net.WebRequest]::Create($url)
	try {
		$res = $req.GetResponse()
	} catch [System.Net.WebException] {
		$res = $_.Exception.Response
	}
	return [int]$res.StatusCode
}

$totalTime = 0
$attempts = 10

function StartUnderDebugger()
{
	$script:sw = [system.diagnostics.stopwatch]::StartNew()

	$dte.ExecuteCommand("Debug.Start");
	#$dte.ExecuteCommand("Debug.StartWithoutDebugging");

	$status = GetUrlStatusCode("http://localhost:54056/")
	while ($status -ne 200)
	{
		$status = GetUrlStatusCode("http://localhost:54056/")
	}

	$script:sw.Stop()
	$elapsed = $script:sw.Elapsed.TotalMilliseconds
	Write-Host $elapsed 

	$global:totalTime = $global:totalTime + $elapsed

	$dte.ExecuteCommand("Debug.StopDebugging");
}

$i = 0
while ($i -lt $attempts)
{
	StartUnderDebugger
	$i = $i + 1
}

Write-Host "Total $attempts attempts"

