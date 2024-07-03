param(
  [Parameter(Mandatory = $true)][int] $BuildId,
  [Parameter(Mandatory = $true)][string] $PromoteToChannelIds
)

try {
  . $PSScriptRoot\..\..\common\tools.ps1
  $darc = Get-Darc

  $buildInfo = & $darc get-build `
    --id $BuildId `
    --extended `
    --output-format json `
    --ci `
  | convertFrom-Json

  if ((-not $?) -or !$buildInfo) {
    Write-Host "Build with BAR ID $BuildId was not found in BAR!"
    exit 1
  }

  $channels = $PromoteToChannelIds -split "-"
  $channelNames = @()
  foreach ($channelId in $channels) {
    $channelInfo = & $darc get-channel `
      --id $channelId `
      --output-format json `
      --ci `
    | ConvertFrom-Json

    if ((-not $?) -or !$channelInfo) {
      Write-PipelineTelemetryError -Category 'PromoteBuild' -Message "Channel with BAR ID $channelId was not found in BAR!"
      ExitWithExitCode 1
    }

    $channelNames += "'$($channelInfo.name)'"
  }

  $azureDevOpsBuildNumber = $buildInfo.azureDevOpsBuildNumber
  $azureDevOpsRepository = "Unknown"
  $lastIndexOfSlash = $buildInfo.azureDevOpsRepository.LastIndexOf('/')
  if ($lastIndexOfSlash -ne -1) {
    $azureDevOpsRepository = $buildInfo.azureDevOpsRepository.Substring($lastIndexOfSlash + 1)
    # Invalid chars in Azdo build number: '"', '/', ':', '<', '>', '\', '|', '?', '@', and '*'
    $azureDevOpsRepository = $azureDevOpsRepository -replace '["/:<>\\|?@*"]', '_'
  }

  $channelNames = $channelNames -join ", "
  $buildNumberName = "Promoting $azureDevOpsRepository build $azureDevOpsBuildNumber ($BuildId) to channel(s) $channelNames #"

  # Maximum buildnumber length is 255 chars
  if ($buildNumberName.Length -GT 255) {
    $buildNumberName = $buildNumberName.Substring(0, 255)
  }

  # Set tags on publishing for visibility
  Write-Host "##vso[build.updatebuildnumber]$buildNumberName"
  Write-Host "##vso[build.addbuildtag]Channel(s) - $channelNames"
  Write-Host "##vso[build.addbuildtag]BAR ID - $BuildId"

  # Set variables used in publishing
  Write-Host "##vso[task.setvariable variable=AzDOProject]$($buildInfo.azureDevOpsProject)"
  Write-Host "##vso[task.setvariable variable=AzDOPipelineId]$($buildInfo.azureDevOpsBuildDefinitionId)"
  Write-Host "##vso[task.setvariable variable=AzDOBuildId]$($buildInfo.azureDevOpsBuildId)"
  Write-Host "##vso[task.setvariable variable=AzDOAccount]$($buildInfo.azureDevOpsAccount)"
  Write-Host "##vso[task.setvariable variable=AzDOBranch]$($buildInfo.azureDevOpsBranch)"
}
catch {
  Write-Host $_
  Write-Host $_.Exception
  Write-Host $_.ScriptStackTrace
  exit 1
}