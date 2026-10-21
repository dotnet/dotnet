Set-StrictMode -Version Latest

# daily/preflight.ps1 - Preflight: will a daily run be able to see what it needs to see (sign-in, tools, identity)?
# Dot-sourced by Invoke-Preflight.ps1 and Invoke-Daily.ps1; it defines functions only and runs nothing.
#
# Only four things stop a run (exit 2): PowerShell older than 7.3, a state folder that cannot be written, no GitHub sign-in, and a
# config holding your email address that Git tracks or does not ignore (the next commit could publish it). Everything else is a
# warning that the run turns into a note at the top of its report. A report built without internal access is "unknown", never
# "healthy", so authentication problems are listed first with the command that fixes them. The Copilot CLI and its plugins are
# prerequisites (README.md) and are not checked: without them the first model call fails the run.
. (Join-Path $PSScriptRoot '..\common\core.ps1')
. (Join-Path $PSScriptRoot 'evidence.ps1')

$script:VmrQBAzureDevOpsResource = '499b84ac-1321-427f-aa17-267ca6975798'
$script:VmrQBSignIn = 'az login --allow-no-subscriptions'

function New-VmrQBCheck {
    param(
        [Parameter(Mandatory)][string] $Id,
        [Parameter(Mandatory)][string] $Name,
        [Parameter(Mandatory)][ValidateSet('ok', 'warn', 'fail', 'skip')][string] $Status,
        [string] $Detail = '',
        [string] $Remedy = '',
        [string[]] $Scopes = @('Collection'),
        [bool] $AuthRelated = $false,
        # Listed before every other note (the internal builds cannot be seen).
        [bool] $ShowFirst = $false
    )
    [pscustomobject]@{ id = $Id; name = $Name; status = $Status; detail = $Detail; remedy = $Remedy; scopes = $Scopes; authRelated = $AuthRelated; showFirst = $ShowFirst }
}

function Limit-VmrQBText {
    param([AllowEmptyString()][string] $Text, [int] $Max = 400)
    # Short, single-line, and with anything token-shaped removed before it can reach a screen or a report.
    $clean = [regex]::Replace($Text, '\beyJ[A-Za-z0-9_-]{10,}\.[A-Za-z0-9_-]{10,}\.[A-Za-z0-9_-]{5,}', '[redacted-token]')
    $clean = [regex]::Replace($clean, '(?i)(Bearer|access_?token["''\s:=]+)\s*[A-Za-z0-9._~+/=-]{20,}', '$1 [redacted]')
    $clean = ($clean -replace '\s+', ' ').Trim()
    if ($clean.Length -gt $Max) { $clean = $clean.Substring(0, $Max) + '...' }
    return $clean
}

function Get-VmrQBAzureTokenFailure {
    # Why `az account get-access-token` failed, from its stderr.
    param([AllowEmptyString()][string] $StdErr = '')
    if ($StdErr -match '(?i)\baz\b.*(?:is not recognized|not found|cannot find)|command not found|CommandNotFoundException') {
        return [pscustomobject]@{ kind = 'az-missing'; authRelated = $false; remedy = 'Install the Azure CLI (https://aka.ms/installazurecli), then run az login.' }
    }
    if ($StdErr -match '(?i)AADSTS|az login|not logged in|no subscription found|Interactive authentication is needed|expired|conditional access|multi-?factor') {
        return [pscustomobject]@{ kind = 'sign-in'; authRelated = $true; remedy = "Sign in with the Microsoft work account that has dnceng/internal access: $script:VmrQBSignIn" }
    }
    return [pscustomobject]@{ kind = 'unknown'; authRelated = $false; remedy = "Run 'az account get-access-token --resource $script:VmrQBAzureDevOpsResource' yourself and read the error." }
}

function Test-VmrQBAzureCliToken {
    $name = 'Azure CLI token for Azure DevOps'
    try { $capture = Invoke-VmrEvidenceNativeCapture -Command "& az account get-access-token --resource $script:VmrQBAzureDevOpsResource --output json --only-show-errors" -TimeoutSeconds 60 }
    catch { return New-VmrQBCheck 'az-token' $name 'warn' (Limit-VmrQBText $_.Exception.Message) "Run: $script:VmrQBSignIn" @('Collection') $true $true }
    if ($capture.exitCode -ne 0) {
        $failure = Get-VmrQBAzureTokenFailure -StdErr ([string]$capture.stderr)
        return New-VmrQBCheck 'az-token' $name 'warn' "[$($failure.kind)] $(Limit-VmrQBText ([string]$capture.stderr))" $failure.remedy @('Collection') $failure.authRelated $true
    }
    # Only the expiry is read; the token itself is dropped here and never stored or printed.
    $expires = $null
    try { $expires = [DateTimeOffset]::Parse(([string](ConvertFrom-Json -InputObject $capture.stdout).expiresOn), [Globalization.CultureInfo]::InvariantCulture) } catch { }
    $capture = $null
    if ($null -ne $expires) {
        $remaining = $expires - [DateTimeOffset]::UtcNow
        if ($remaining.TotalMinutes -lt 20) {
            return New-VmrQBCheck 'az-token' $name 'warn' "Token expires in $([int][Math]::Max(0, $remaining.TotalMinutes)) minutes; a full run can take about an hour." "Refresh it first: $script:VmrQBSignIn" @('Collection') $true
        }
        return New-VmrQBCheck 'az-token' $name 'ok' "Valid for about $([int]$remaining.TotalMinutes) more minutes." '' @('Collection')
    }
    return New-VmrQBCheck 'az-token' $name 'ok' 'Token acquired.' '' @('Collection')
}

function Test-VmrQBAzureDevOpsAccess {
    # Reads the unified-build pipeline definition through the collector's own transport.
    param([Parameter(Mandatory)][ValidateSet('dnceng-public', 'dnceng')][string] $Organization)
    $project = if ($Organization -eq 'dnceng') { 'internal' } else { 'public' }
    $label = "Azure DevOps $Organization/$project"
    $id = "azdo-$project"
    $session = New-VmrReadOnlySession -TimeoutSeconds 60 -MaxRequests 50
    try {
        $result = Invoke-VmrReadOnlyRequest -Session $session -Service AzureDevOps -ApiPath 'build/definitions?name=dotnet-unified-build' -Organization $Organization -Project $project
        $count = if ($null -ne $result.PSObject.Properties['value']) { @($result.value).Count } else { 0 }
        if ($count -lt 1) {
            return New-VmrQBCheck $id $label 'warn' 'Signed in, but the dotnet-unified-build pipeline definition was not returned.' 'Confirm your account can see the pipeline in the Azure DevOps UI; a missing definition means no permission or a renamed pipeline.' @('Collection') $false ($Organization -eq 'dnceng')
        }
        return New-VmrQBCheck $id $label 'ok' "Pipeline definition readable (HTTP $($session['lastHttpStatusCode']))." '' @('Collection')
    }
    catch {
        $failure = Get-VmrCollectorError -Exception $_.Exception -Scope "$Organization/$project" -Stage 'preflight'
        $status = if ($session.ContainsKey('lastHttpStatusCode')) { " (HTTP $($session['lastHttpStatusCode']))" } else { '' }
        $remedy = if ($failure.authenticationRequired) {
            if ($failure.signInCommand) { "Run: $($failure.signInCommand)" } else { "Run: $script:VmrQBSignIn" }
        } else { 'Retry; if it persists, check VPN/proxy and Azure DevOps service health.' }
        return New-VmrQBCheck $id $label 'warn' "$(Limit-VmrQBText $failure.message)$status" $remedy @('Collection') ([bool]$failure.authenticationRequired) ($Organization -eq 'dnceng')
    }
    finally { try { $session.tokens.Clear() } catch { } }
}

function Test-VmrQBGitHubAccess {
    $checks = [Collections.Generic.List[object]]::new()
    $status = Invoke-VmrEvidenceNativeCapture -Command '& gh auth status --hostname github.com' -TimeoutSeconds 30
    $statusText = ([string]$status.stdout + ' ' + [string]$status.stderr)
    if ($status.exitCode -ne 0) {
        $missing = $statusText -match '(?i)not recognized|command not found|CommandNotFoundException'
        $checks.Add((New-VmrQBCheck 'gh-auth' 'GitHub CLI sign-in' 'fail' (Limit-VmrQBText $statusText) $(if ($missing) { 'Install the GitHub CLI (https://cli.github.com), then run gh auth login.' } else { 'Run: gh auth login --hostname github.com' }) @('Collection') (-not $missing)))
        return $checks.ToArray()
    }
    $checks.Add((New-VmrQBCheck 'gh-auth' 'GitHub CLI sign-in' 'ok' 'Signed in to github.com.' '' @('Collection')))
    $limit = Invoke-VmrEvidenceNativeCapture -Command '& gh api rate_limit --jq .resources.core.remaining' -TimeoutSeconds 30
    $remaining = 0
    if ($limit.exitCode -eq 0 -and [int]::TryParse(([string]$limit.stdout).Trim(), [ref]$remaining) -and $remaining -lt 300) {
        $checks.Add((New-VmrQBCheck 'gh-rate' 'GitHub API rate limit' 'warn' "Only $remaining core requests remain; issue evidence may come back partial." 'Wait for the reset, or run again later.' @('Collection')))
    }
    return $checks.ToArray()
}

function Test-VmrQBStateRoot {
    $paths = Get-VmrQBPaths
    $checks = [Collections.Generic.List[object]]::new()
    $everyScope = @('Collection', 'Synthesis', 'Sending')
    try {
        [void][IO.Directory]::CreateDirectory($paths.StateRoot)
        $probe = Join-Path $paths.StateRoot ('.preflight-' + [guid]::NewGuid().ToString('N'))
        [IO.File]::WriteAllText($probe, 'x'); [IO.File]::Delete($probe)
        $checks.Add((New-VmrQBCheck 'state-root' 'State directory is writable' 'ok' $paths.StateRoot '' $everyScope))
    }
    catch { $checks.Add((New-VmrQBCheck 'state-root' 'State directory is writable' 'fail' "$($paths.StateRoot): $(Limit-VmrQBText $_.Exception.Message)" 'Set VMR_QB_HOME to a writable directory.' $everyScope)); return $checks.ToArray() }

    # A config that holds your email address inside a git work tree is a commit away from being public, so the run stops until it is
    # safe. Git tracking it (or having it staged) is the worse case: an ignore rule does not protect a tracked file. Untracked and not
    # ignored, a `git add -A` stages it. Only a definite answer from git stops a run; a git that cannot answer (no repository, a
    # safe.directory refusal) is not a reason to stop. Git is asked from the config's own folder with its file name, so the answer
    # does not depend on how the path is spelled (8.3 short names, symlinks).
    if ((Get-Command git -ErrorAction SilentlyContinue) -and (Test-Path -LiteralPath $paths.ConfigPath)) {
        $holdsEmail = $false
        try {
            $document = Get-Content -LiteralPath $paths.ConfigPath -Raw | ConvertFrom-Json
            # Protect raw addresses even when validation disables sending.
            $addresses = @(
                Get-VmrQBField $document 'sender'
                Get-VmrQBField $document 'recipients'
            )
            $holdsEmail = @(Get-VmrQBRecipientList $addresses).Count -gt 0
        } catch { }
        $configDirectory = Split-Path -Parent $paths.ConfigPath
        $configName = Split-Path -Leaf $paths.ConfigPath
        $top = (& git -C $configDirectory rev-parse --show-toplevel 2>$null)
        if ($holdsEmail -and $LASTEXITCODE -eq 0 -and $top) {
            $indexed = @(& git -C $configDirectory ls-files --cached -- $configName 2>$null)
            $indexExit = $LASTEXITCODE
            $notIgnored = $false
            if ($indexExit -eq 0 -and -not $indexed.Count) {
                & git -C $configDirectory check-ignore -q -- $configName 2>$null
                $notIgnored = ($LASTEXITCODE -eq 1)
            }
            if ($indexExit -eq 0 -and $indexed.Count) {
                $checks.Add((New-VmrQBCheck 'state-in-repo' 'Your email address is not at risk of being committed' 'fail' "The local config holds your email address and Git tracks it or has it staged in '$top'. An ignore rule does not protect a tracked file, so the next commit would publish it." "Stop tracking it and keep your copy: git -C `"$configDirectory`" rm --cached -- `"$configName`"  (add -f if Git refuses). Then add an ignore rule for it and run this check again. If it was already committed or pushed, treat the address as exposed." $everyScope))
            }
            elseif ($notIgnored) {
                $checks.Add((New-VmrQBCheck 'state-in-repo' 'Your email address is not at risk of being committed' 'fail' "The local config holds your email address and sits in the git work tree '$top' without an ignore rule, so a 'git add -A' would stage it." "Add an ignore rule that covers it, such as the line $configName in that repository's .gitignore (or your whole state folder, which also holds your reports and logs), then run this check again." $everyScope))
            }
        }
    }
    return $checks.ToArray()
}

function Test-VmrQBOutlook {
    param([Parameter(Mandatory)][string] $Sender)
    $name = 'Outlook is available for sending'
    $com = [Collections.Generic.List[object]]::new()
    $offline = $false; $found = $false
    try {
        $app = New-Object -ComObject Outlook.Application; $com.Add($app)
        $session = $app.Session; $com.Add($session)
        $offline = [bool]$session.Offline
        $accounts = $session.Accounts; $com.Add($accounts)
        for ($i = 1; $i -le $accounts.Count; $i++) {
            $account = $accounts.Item($i); $com.Add($account)
            if ($account.SmtpAddress -ieq $Sender) { $found = $true; break }
        }
    }
    catch { return New-VmrQBCheck 'outlook' $name 'warn' (Limit-VmrQBText $_.Exception.Message) 'Email uses classic Outlook through COM. Start classic Outlook (the "New Outlook" has no COM interface), or run with -SkipEmail.' @('Sending') }
    finally { foreach ($o in $com) { if ($null -ne $o) { [void][Runtime.InteropServices.Marshal]::ReleaseComObject($o) } } }
    if ($offline) { return New-VmrQBCheck 'outlook' $name 'warn' 'Outlook is in offline mode.' 'Switch Outlook to online mode, or run with -SkipEmail.' @('Sending') }
    if (-not $found) { return New-VmrQBCheck 'outlook' $name 'warn' "Outlook has no account matching sender '$Sender'." 'Set sender to the email address of an account available in classic Outlook; it does not have to be in recipients.' @('Sending') }
    return New-VmrQBCheck 'outlook' $name 'ok' 'Online, account found.' '' @('Sending')
}

function Invoke-VmrQBPreflight {
    <#
    .SYNOPSIS
    Runs every check relevant to the requested scope and returns structured results.
    .PARAMETER Scope
    Collection (evidence), Synthesis (the model run), Sending (email). Default: all three.
    .OUTPUTS
    ok (nothing blocking), checks, blocking, warnings, authErrors.
    #>
    [CmdletBinding()]
    param(
        [ValidateSet('Collection', 'Synthesis', 'Sending')][string[]] $Scope = @('Collection', 'Synthesis', 'Sending'),
        [switch] $SkipNetwork
    )
    $everyScope = @('Collection', 'Synthesis', 'Sending')
    $checks = [Collections.Generic.List[object]]::new()
    $add = { param($c) foreach ($item in @($c)) { $checks.Add($item) } }

    if ($PSVersionTable.PSVersion -ge [version]'7.3') { & $add (New-VmrQBCheck 'pwsh' 'PowerShell 7.3 or newer' 'ok' "PowerShell $($PSVersionTable.PSVersion)" '' $everyScope) }
    else { & $add (New-VmrQBCheck 'pwsh' 'PowerShell 7.3 or newer' 'fail' "PowerShell $($PSVersionTable.PSVersion)" 'Install PowerShell 7 (winget install Microsoft.PowerShell) and run with pwsh.' $everyScope) }

    & $add (Test-VmrQBStateRoot)

    $paths = Get-VmrQBPaths
    $configResult = Get-VmrQBConfigLenient -Path $paths.ConfigPath
    $config = $configResult.Config
    $emailOn = -not [string]::IsNullOrWhiteSpace([string]$config.sender)
    if (@($configResult.Problems).Count) {
        & $add (New-VmrQBCheck 'config' 'QB config is valid' 'warn' (Limit-VmrQBText (@($configResult.Problems) -join ' ') 400) "No email is sent until $($paths.ConfigPath) is fixed; the reports are still produced and saved." @('Sending'))
    }
    else {
        $detail = if ($emailOn) {
            "Reports are emailed from $($config.sender) to $(@(Get-VmrQBRecipientList -Value $config.recipients -Sender $config.sender) -join '; ')."
        } else {
            "No sender configured: reports are saved to $($paths.ReportsDir) and not emailed. To get them by email, copy qb-config.template.json to $($paths.ConfigPath) and set `"sender`" (see README.md)."
        }
        & $add (New-VmrQBCheck 'config' 'QB config is valid' 'ok' $detail '' @('Sending'))
    }

    # Every network check serves Collection, so a run that only writes reports or sends them makes no network call.
    if (-not $SkipNetwork -and 'Collection' -in $Scope) {
        & $add (Test-VmrQBAzureCliToken)
        & $add (Test-VmrQBAzureDevOpsAccess -Organization 'dnceng')
        & $add (Test-VmrQBAzureDevOpsAccess -Organization 'dnceng-public')
        & $add (Test-VmrQBGitHubAccess)
    }
    if ('Sending' -in $Scope -and $emailOn) { & $add (Test-VmrQBOutlook -Sender $config.sender) }

    # Only checks that serve a requested scope can block.
    $relevant = @($checks | Where-Object { @($_.scopes | Where-Object { $_ -in $Scope }).Count -gt 0 })
    $blocking = @($relevant | Where-Object status -eq 'fail')
    return [pscustomobject]@{
        ok         = ($blocking.Count -eq 0)
        scope      = $Scope
        checks     = $relevant
        blocking   = $blocking
        warnings   = @($relevant | Where-Object status -eq 'warn')
        authErrors = @($relevant | Where-Object { $_.authRelated -and $_.status -in 'fail', 'warn' })
    }
}
