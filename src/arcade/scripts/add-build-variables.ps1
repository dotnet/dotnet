<#
.SYNOPSIS
Adds/removes variables to a pipeline or all pipelines in a project.

.DESCRIPTION
This script is generally useful for setting or removing variables that have an effect on pipelines. This has generally been used for:
- Setting variables to control monitoring on internal builds.
- Adding and removing opt-outs for NuGet feed analysis.

.PARAMETER PipelineId
Optional ID of a pipeline. If no ID is specified, all pipelines get the variable

.PARAMETER AzDoPAT
PAT with edit access to pipelines

.PARAMETER VariableName
Name of variable

.PARAMETER VariableValue
Value of variable, if not removing

.PARAMETER RemoveVariable
Remove the variable, rather than adding it.

.PARAMETER Org
AzDO Organization

.PARAMETER Project
AzDO Project in the organization.

#>

param (
    [string]$PipelineId,
    [Parameter(Mandatory=$true)]
    [string]$AzDoPAT,
    [Parameter(Mandatory=$true)]
    [string]$VariableName,
    [string]$VariableValue,
    [switch]$RemoveVariable,
    [Parameter(Mandatory=$true)]
    [string]$Org,
    [Parameter(Mandatory=$true)]
    [string]$Project
)

function UpdatePipeline($id, $authHeaders)
{
    $pipelineUri = "https://dev.azure.com/$Org/$Project/_apis/build/definitions/$($id)?api-version=6.1-preview.7"
    $existingPipeline = Invoke-WebRequest -Headers $authHeaders -Method Get -Uri $pipelineUri -ContentType 'application/json' | ConvertFrom-Json
    Write-Host "Updating pipeline $Org/$Project/$($existingPipeline.name) (pipeline id: $id)"
    
    # Update the variables with the new one if not already done. If variable value is null, remove
    if ($existingPipeline.variables -eq $null -and -not $RemoveVariable) {
        Write-Host "  Adding new variable '$VariableName' with value '$VariableValue'"
        $newVariablesNode = @{ $VariableName = @{ value = $VariableValue; allowOverride = $false } }
        $existingPipeline | Add-Member -MemberType NoteProperty -Name 'variables' -Value $newVariablesNode
    } else {
        $existingVariableObject = $existingPipeline.variables | Get-Member $VariableName
        if ($RemoveVariable) {
            Write-Host "  Removing '$VariableName' from build pipeline"
            $existingPipeline.variables.PSObject.Properties.Remove($VariableName)
        } elseif ($existingVariableObject -eq $null) {
            Write-Host "  Adding new variable '$VariableName' with value '$VariableValue'"
            $existingPipeline.variables | Add-Member -MemberType NoteProperty -Name $VariableName -Value @{ value = $VariableValue; allowOverride = $false }
        } else {
            Write-Host "  Updating '$VariableName' with new value '$VariableValue'"
            $existingPipeline.variables.PSObject.Properties.Remove($VariableName)
            $existingPipeline.variables | Add-Member -MemberType NoteProperty -Name $VariableName -Value @{ value = $VariableValue; allowOverride = $false }
        }
    }
    
    # Attempt the update
    $bodyJson = $existingPipeline | ConvertTo-Json -Depth 10
    $updatedPipelineJson = Invoke-WebRequest -Headers $azdoAuthHeader -Method Put $pipelineUri -Body $bodyJson -ContentType 'application/json' | ConvertFrom-Json
}

$base64authinfo = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes(":$AzDoPAT"))
$azdoAuthHeader = @{"Authorization"="Basic $base64authinfo"}

if ($PipelineId) {
    UpdatePipeline $PipelineId $azdoAuthHeader
} else {
    $allPipelines = Invoke-WebRequest -ContentType 'application/json' -Method Get -Headers $azdoAuthHeader -Uri "https://dev.azure.com/$Org/$Project/_apis/build/definitions?api-version=6.1-preview.7" | ConvertFrom-Json
    Write-Host "Updating $($allPipelines.count) pipelines"
    foreach ($pipeline in $allPipelines.value) {
        UpdatePipeline $pipeline.Id $azdoAuthHeader
    }
}