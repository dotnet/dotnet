Param(
    [Parameter(Mandatory=$true)][string] $ManifestDirPath    # Manifest directory where sbom will be placed
)

# create directory for sbom manifest to be placed
if (!(Test-Path -path $ManifestDirPath))
{
  Write-Host "Creating dir $ManifestDirPath"
  New-Item -ItemType Directory -path $ManifestDirPath
  Write-Host "Successfully created directory $ManifestDirPath"
}

Write-Host "Updating artifact name"
$artifact_name = "${env:SYSTEM_STAGENAME}_${env:AGENT_JOBNAME}_SBOM" -replace '["/:<>\\|?@*"() ]', '_'
Write-Host "Artifact name $artifact_name"
Write-Host "##vso[task.setvariable variable=ARTIFACT_NAME]$artifact_name"
