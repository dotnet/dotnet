<#
.SYNOPSIS
Pauses all pipelines in an org/project

.PARAMETER Organization
Organization to pause

.PARAMETER Project
Project to pause

.PARAMETER AzDOPat
PAT used to make AzDO modifications. Requires build read/execute.

.PARAMETER Resume
Resume rather than pause

#>

param (
    [Parameter(Mandatory=$true)][string]$Organization,
    [Parameter(Mandatory=$true)][string]$Project,
    [Parameter(Mandatory=$true)][string]$AzDOPat,
    [switch]$Resume
)

$base64authinfo = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes(":$AzDOPat"))
$AzDOAuthHeader = @{"Authorization"="Basic $base64authinfo"}

$allPipelines = Invoke-WebRequest -Uri "https://dev.azure.com/$Organization/$Project/_apis/build/definitions?api-version=6.0" -Headers $AzDOAuthHeader | ConvertFrom-Json

$queueStatusString = $null
$uxString = $null
if ($Resume) {
    $queueStatusString = "enabled"
    $uxString = "Resuming"
} else {
    $queueStatusString = "paused"
    $uxString = "Pausing"
}

Write-Host "$uxString $($allPipelines.count) pipelines..."

foreach ($pipeline in $allPipelines.value) {
    
    try {
        $pipelineId = $pipeline.id
        
        Write-Host -NoNewLine "  $uxString '$($pipeline.name)' (id: $pipelineId)..."
        
        $pipelineUri = "https://dev.azure.com/$Organization/$Project/_apis/build/definitions/$($pipelineId)?api-version=6.0"
        $pipelineInfo = Invoke-WebRequest -Uri $pipelineUri -Headers $AzDOAuthHeader | ConvertFrom-Json
        
        # Update the pipeline
        $pipelineInfo.queueStatus = $queueStatusString
        
        #update the definition
        $result = Invoke-WebRequest -Uri $pipelineUri -Headers $AzDOAuthHeader -Method Put -Body (ConvertTo-Json $pipelineInfo -Depth 100) -ContentType "application/json"
        
        if ($result.StatusCode -eq 200) {
            Write-Host "done"
        }
    
    } catch {
        Write-Host "`nFailed to pause pipeline $pipelineId"
        Write-Host $_
    }
}