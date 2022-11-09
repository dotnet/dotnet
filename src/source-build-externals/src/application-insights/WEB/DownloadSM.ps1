param ([string] $msiPath)

function Download-AgentMsi
{
	param (
		[string] $msiPath
	)

	Write-Host "Start - Agent MSI download process."

	$downloadLink = "http://go.microsoft.com/fwlink/?LinkID=522371&clcid=0x409"
	
	Write-Host "Downloading from $downloadLink"
	$wc = New-Object System.Net.WebClient
	$wc.DownloadFile($downloadLink, $msiPath)
	
	Write-Host "End - Agent MSI download process."
}

Download-AgentMsi $msiPath