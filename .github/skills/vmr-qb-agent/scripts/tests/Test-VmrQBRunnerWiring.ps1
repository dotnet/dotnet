[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# Offline checks for email settings, read-only permissions and private-config protection.
$scripts = Split-Path -Parent $PSScriptRoot
$skill = Split-Path -Parent $scripts
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
function New-TempDirectory { $d = Join-Path ([IO.Path]::GetTempPath()) ('vmrqb-' + [guid]::NewGuid().ToString('N')); [void][IO.Directory]::CreateDirectory($d); return $d }
function Use-StateRoot([string] $Path) { if ($Path) { $env:VMR_QB_HOME = $Path } else { Remove-Item Env:\VMR_QB_HOME -ErrorAction SilentlyContinue } }

Check 'Email is off without a sender; empty recipients send to sender and explicit recipients are unchanged' {
    . (Join-Path $scripts 'common\core.ps1')
    $sandbox = New-TempDirectory
    $saved = $env:VMR_QB_HOME
    try {
        Use-StateRoot $sandbox
        $result = Get-VmrQBConfigLenient -Path (Get-VmrQBPaths).ConfigPath
        Assert ([string]::IsNullOrWhiteSpace([string]$result.Config.sender) -and @($result.Config.recipients).Count -eq 0 -and @($result.Problems).Count -eq 0) 'A missing config must mean no sender, no recipients and no problem.'
    }
    finally { Use-StateRoot $saved; Remove-Item -LiteralPath $sandbox -Recurse -Force -ErrorAction SilentlyContinue }
    $template = Get-Content -LiteralPath (Join-Path $skill 'qb-config.template.json') -Raw | ConvertFrom-Json
    Assert ([string]::IsNullOrWhiteSpace([string]$template.sender) -and @($template.recipients).Count -eq 0) 'The public template must be blank.'
    Assert ((Read-Script $daily).Text -match '(?s)IsNullOrWhiteSpace\(\[string\]\$config\.sender\).{0,900}\$SkipEmail = ') 'An empty sender must switch sending off before anything is sent.'
    Assert ((@(Get-VmrQBRecipientList -Value @() -Sender 'qb@example.com') -join ';') -eq 'qb@example.com') 'Empty recipients must send to sender.'
    Assert ((@(Get-VmrQBRecipientList -Value @('one@example.com', 'two@example.com') -Sender 'qb@example.com') -join ';') -eq 'one@example.com;two@example.com') 'Explicit recipients must not require or automatically include sender.'
}

Check 'The nested model can read but never write: no gh api, no blanket allow, writes denied, allow-all removed from its environment' {
    . $daily
    $config = [pscustomobject]@{ sender = 'qb@example.com'; recipients = @(); model = ''; pluginRoot = '' }
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

Check 'A config holding an email address stops every run until Git neither tracks it nor would stage it' {
    if (-not (Get-Command git -ErrorAction SilentlyContinue)) { Write-Host '  (git is not available; skipped)'; return }
    . (Join-Path $scripts 'daily\preflight.ps1')
    $repo = New-TempDirectory
    $state = Join-Path $repo 'state'
    [void][IO.Directory]::CreateDirectory($state)
    & git -C $repo init -q
    Set-Content -LiteralPath (Join-Path $state 'qb-config.local.json') -Value '{ "sender": "qb@example.com", "recipients": [] }'
    $saved = $env:VMR_QB_HOME
    try {
        Use-StateRoot $state
        $stops = { @(Test-VmrQBStateRoot | Where-Object { $_.id -eq 'state-in-repo' -and $_.status -eq 'fail' }).Count }
        Assert ((& $stops) -eq 1) 'A config with an address that is untracked and not ignored must stop the run.'
        Set-Content -LiteralPath (Join-Path $repo '.gitignore') -Value 'state/'
        Assert ((& $stops) -eq 0) 'An ignored config must not stop the run.'
        & git -C $repo add -f -- state/qb-config.local.json
        Assert ((& $stops) -eq 1) 'A tracked config must stop the run even when it is ignored.'
    }
    finally { Use-StateRoot $saved; Remove-Item -LiteralPath $repo -Recurse -Force -ErrorAction SilentlyContinue }
}

if ($failures) { Write-Host "$failures safety check(s) failed." -ForegroundColor Red; exit 1 }
Write-Host 'Offline email, read-only and private-config checks passed. No network or email.' -ForegroundColor Green
