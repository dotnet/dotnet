[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$script:GitHubCalls = [Collections.Generic.List[object]]::new()
$passed = [Collections.Generic.List[string]]::new()

function Assert-IssueTriage {
    param([bool] $Condition, [string] $Message)
    if (-not $Condition) { throw $Message }
}

# Any GitHub call while the collector is only being loaded is a failure.
function gh {
    $script:GitHubCalls.Add(@($args))
    throw 'The collector called GitHub while it was only being loaded.'
}

$collector = Join-Path $PSScriptRoot '..\triage\issue.ps1'
$loadOutput = @(. $collector)
Assert-IssueTriage ($script:GitHubCalls.Count -eq 0 -and $loadOutput.Count -eq 0) 'Dot-sourcing performed collection or emitted output.'
$passed.Add('No collection at dot-source time')

$tokens = $null; $parseErrors = $null
$ast = [Management.Automation.Language.Parser]::ParseFile($collector, [ref]$tokens, [ref]$parseErrors)
Assert-IssueTriage ($parseErrors.Count -eq 0) 'Collector has PowerShell parse errors.'
$ghCommands = @($ast.FindAll({
    param($node)
    $node -is [Management.Automation.Language.CommandAst] -and $node.GetCommandName() -eq 'gh'
}, $true))
Assert-IssueTriage ($ghCommands.Count -eq 1 -and $ghCommands[0].Extent.Text -match 'api --hostname github.com --method GET --include') `
    'Production transport must have one explicit GET-only gh entry point.'
$passed.Add('No GitHub writes: the sole production gh command is a GET')

"PASS: $($passed.Count) offline issue-triage checks."
$passed | ForEach-Object { "  $_" }
