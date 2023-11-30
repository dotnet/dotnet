[CmdletBinding(PositionalBinding=$false)]
Param(
  [Parameter(Mandatory=$true)][String][Alias('c')]$channel,
  [Parameter(Mandatory=$true)][String][Alias('r')]$release,
  [Parameter(Mandatory=$true)][String][Alias('o')]$outputDirectory
)
# sku: BuildTools, Enterprise, Professional, Community
# Setting sku explicitly to BuildTools for xcopy MSBuild generation
$sku = "BuildTools"
# channel: pre, etc..
# release: 16, 17, etc..
$downloadUrl = "https://visualstudio.microsoft.com/thank-you-downloading-visual-studio/?sku=$sku&ch=$channel&rel=$release"

$folderName = "$sku-$channel-$release"
$destinationDir = [System.IO.Path]::Combine("$PSScriptRoot", '.download', "$folderName")
$installerFilename = "vs_installer.exe"
$installerPath = "$destinationDir\$InstallerFilename"

if(-Not (Test-Path $destinationDir))
{
    New-Item -ItemType 'Directory' -Path "$destinationDir" -Force | Out-Null
}
# Query the page to get the download link
$response = Invoke-WebRequest $downloadUrl

$regex = "downloadUrl: '(?<downloadUrl>[^']+)'"
$response.Content -Match $regex | Out-Null
$downloadLink = $Matches['downloadUrl']

Write-Host "download link: $downloadLink"
$response = Invoke-WebRequest $downloadLink  -OutFile "$installerPath"

if(-Not (Test-Path $outputDirectory))
{
    New-Item -ItemType 'Directory' -Path "$outputDirectory" -Force | Out-Null
}

# Install
Write-Host "Installing..."
Write-Host "Start-Process -FilePath $installerPath -ArgumentList install, --installPath, $outputDirectory, --quiet, --norestart, --force, --add, 'Microsoft.VisualStudio.Workload.ManagedDesktopBuildTools;includeRecommended' -Verb RunAs -Wait"
Start-Process  -FilePath $installerPath -ArgumentList install, --installPath, $outputDirectory, --quiet, --norestart, --force, --add, 'Microsoft.VisualStudio.Workload.ManagedDesktopBuildTools;includeRecommended' -Verb RunAs -Wait

Write-Host "Installation complete."