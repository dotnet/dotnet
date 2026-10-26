[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# Offline checks for read-only permissions.
$scripts = Split-Path -Parent $PSScriptRoot
$daily = Join-Path $scripts 'daily\Invoke-Daily.ps1'

$failures = 0
function Check([string] $Name, [scriptblock] $Body) {
    try { & $Body; Write-Host "PASS: $Name" -ForegroundColor Green }
    catch { $script:failures++; Write-Host "FAIL: $Name -> $($_.Exception.Message)" -ForegroundColor Red }
}
function Assert([bool] $Condition, [string] $Message) { if (-not $Condition) { throw $Message } }
function Read-Script([string] $Path) {
    $ast = [Management.Automation.Language.Parser]::ParseFile($Path, [ref]$null, [ref]$null)
    return [pscustomobject]@{ Ast = $ast; Text = [IO.File]::ReadAllText($Path) }
}

Check 'The nested model can read but never write: no gh api, no blanket allow, writes denied, allow-all removed from its environment' {
    . $daily
    $config = [pscustomobject]@{ model = ''; pluginRoot = '' }
    $readOnlyShell = @('gh pr list', 'gh pr view', 'gh pr checks', 'gh pr diff', 'gh issue list', 'gh issue view', 'gh search issues', 'gh search prs')
    foreach ($variant in @(@{}, @{ EvidenceOnly = $true })) {
        $arguments = @(Get-CopilotArgs -Prompt 'fixture' -WorkDirectory 'C:\runs\fixture-run' @variant)
        Assert ('--deny-tool=write' -in $arguments) 'Writes must be denied in every profile.'
        foreach ($blanket in @('--allow-all', '--allow-all-tools', '--yolo', '--allow-all-paths', '--allow-all-urls')) { Assert ($blanket -notin $arguments) "A profile grants $blanket." }
        foreach ($argument in @($arguments | Where-Object { $_ -like '--allow-tool=shell(*' })) {
            $command = $argument.Substring('--allow-tool=shell('.Length).TrimEnd(')')
            Assert ($command -in $readOnlyShell) "shell($command) is not on the read-only list; it could change something with the QB's credentials."
        }
        Assert (($arguments -join ' ') -notmatch 'shell\(gh api') 'gh api matches by subcommand, so it would also allow -X POST.'
    }
    Assert ((Read-Script $daily).Text.Contains("Remove('COPILOT_ALLOW_ALL')")) 'The launcher must remove COPILOT_ALLOW_ALL from the nested process.'
}

Check 'The issue search only reads: every gh call is a GET' {
    $ast = (Read-Script (Join-Path $scripts 'triage\issue.ps1')).Ast
    $ghCommands = @($ast.FindAll({ param($node) $node -is [Management.Automation.Language.CommandAst] -and $node.GetCommandName() -eq 'gh' }, $true))
    Assert ($ghCommands.Count -ge 1) 'The issue search must reach GitHub through gh.'
    foreach ($command in $ghCommands) { Assert ($command.Extent.Text -match 'api\b.*--method GET') "A gh call is not an explicit GET (line $($command.Extent.StartLineNumber))." }
}

if ($failures) { Write-Host "$failures safety check(s) failed." -ForegroundColor Red; exit 1 }
Write-Host 'Offline read-only checks passed. No network or external changes.' -ForegroundColor Green
