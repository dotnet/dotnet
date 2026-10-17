Set-StrictMode -Version Latest

# daily/preflight.ps1 - Preflight: will a daily run be able to see what it needs to see (sign-in, tools, identity)?
# Dot-sourced by Invoke-Preflight.ps1, Invoke-Daily.ps1 and the tests; it defines functions only and runs nothing.
. (Join-Path $PSScriptRoot '..\common\core.ps1')
. (Join-Path $PSScriptRoot 'evidence.ps1')

# ---------------------------------------------------------------------------------------------
# checks/Preflight.ps1
# ---------------------------------------------------------------------------------------------
# Preflight: will a daily run actually be able to see what it needs to see?
#
# The costliest failure mode of this whole process is a confident report built from partial evidence
# because something quietly could not sign in. This checks every dependency up front and says exactly
# what is wrong. Authentication problems are reported FIRST, verbatim, with the command that fixes them,
# because a report built without internal access is not "healthy", it is "unknown".
#
# Only four things stop a run: PowerShell older than 7.2, a state folder that cannot be written, a
# GitHub sign-in that does not work, and a config holding your email address that Git tracks or does not
# ignore (a privacy stop: the next commit could publish it). Everything else is a WARNING: the run goes on
# with what it has, the report says what is missing (internal authentication first, in red) and each
# warning is listed in the run notes at the top of the overall summary. A missing report is worse than a
# report with a note.
#
# The Azure DevOps probes go through the same transport the collector uses
# (VmrEvidenceTransport.ps1), so "preflight passes" means "the collector's own access works".
# External commands go through $Probes.Native so tests can simulate any failure without a network.


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

function Get-VmrQBAzureTokenFailure {
    <#
    .SYNOPSIS
    Classifies why `az account get-access-token` failed, from its stderr. Pure; the token is never involved.
    #>
    [CmdletBinding()]
    param([AllowEmptyString()][string] $StdErr = '', [int] $ExitCode = 1)
    $text = $StdErr
    $rules = @(
        @{ kind = 'az-missing';         auth = $false; pattern = "(?i)\baz\b.*(?:is not recognized|not found|cannot find)|command not found|CommandNotFoundException"; remedy = 'Install the Azure CLI (https://aka.ms/installazurecli), then run az login.' },
        @{ kind = 'mfa-required';       auth = $true;  pattern = '(?i)AADSTS50076|AADSTS50079|multi-?factor|\bMFA\b';                                                  remedy = "Complete multi-factor sign-in: $script:VmrQBSignIn" },
        @{ kind = 'conditional-access'; auth = $true;  pattern = '(?i)AADSTS53\d{3}|conditional access|compliant device|device is not';                          remedy = "Sign in from a compliant device/session: $script:VmrQBSignIn" },
        @{ kind = 'token-expired';      auth = $true;  pattern = '(?i)AADSTS70043|AADSTS700082|AADSTS50173|AADSTS70008|refresh token (?:has )?expired|sign-?in frequency|session (?:has )?expired|token (?:has )?expired'; remedy = "The sign-in expired: $script:VmrQBSignIn" },
        @{ kind = 'wrong-account';      auth = $true;  pattern = '(?i)AADSTS50020|AADSTS700016|AADSTS90072|not found in the directory|does not exist in tenant|guest user';       remedy = "Sign in with the Microsoft work account that has dnceng/internal access: $script:VmrQBSignIn" },
        @{ kind = 'consent-required';   auth = $true;  pattern = '(?i)AADSTS65001|AADSTS65004|consent';                                                          remedy = 'An administrator or you must grant consent for the Azure CLI to access Azure DevOps.' },
        @{ kind = 'not-logged-in';      auth = $true;  pattern = "(?i)az login|Please run 'az login'|not logged in|no subscription found|Interactive authentication is needed|AADSTS\d+"; remedy = "Not signed in: $script:VmrQBSignIn" },
        @{ kind = 'network';            auth = $false; pattern = '(?i)ConnectionError|Max retries|timed out|Name or service not known|proxy|SSL|certificate|connection (?:reset|refused)|No such host'; remedy = 'Check network/VPN/proxy access to login.microsoftonline.com, then retry.' }
    )
    foreach ($rule in $rules) {
        if ($text -match $rule.pattern) { return [pscustomobject]@{ kind = $rule.kind; authRelated = $rule.auth; remedy = $rule.remedy } }
    }
    return [pscustomobject]@{ kind = 'unknown'; authRelated = $false; remedy = "Run 'az account get-access-token --resource $script:VmrQBAzureDevOpsResource' yourself and read the error." }
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

function Get-VmrQBDefaultProbes {
    return @{
        Native = { param($command, $timeout) Invoke-VmrEvidenceNativeCapture -Command $command -TimeoutSeconds $timeout }
        PluginRoot = { param($configured) Get-VmrQBPluginSearchRoots -ConfiguredRoot $configured }
        Outlook = {
            param($addresses)
            $result = @{ available = $false; offline = $null; accountFound = $false; error = $null }
            $com = [Collections.Generic.List[object]]::new()
            try {
                $app = New-Object -ComObject Outlook.Application; $com.Add($app); $result.available = $true
                $session = $app.Session; $com.Add($session)
                $result.offline = [bool]$session.Offline
                $accounts = $session.Accounts; $com.Add($accounts)
                for ($i = 1; $i -le $accounts.Count; $i++) {
                    $account = $accounts.Item($i); $com.Add($account)
                    if (@($addresses) -contains $account.SmtpAddress) { $result.accountFound = $true; break }
                }
            }
            catch { $result.error = $_.Exception.Message }
            finally { foreach ($o in $com) { if ($null -ne $o) { [void][Runtime.InteropServices.Marshal]::ReleaseComObject($o) } } }
            return $result
        }
        Session = { New-VmrReadOnlySession -TimeoutSeconds 60 -MaxRequests 50 }
        Now = { [DateTimeOffset]::UtcNow }
    }
}

function Test-VmrQBAzureCliToken {
    [CmdletBinding()]
    param([Parameter(Mandatory)][hashtable] $Probes)
    $capture = $null
    try {
        $capture = & $Probes.Native "& az account get-access-token --resource $script:VmrQBAzureDevOpsResource --output json --only-show-errors" 60
    }
    catch {
        return New-VmrQBCheck 'az-token' 'Azure CLI token for Azure DevOps' 'warn' (Limit-VmrQBText $_.Exception.Message) "Run: $script:VmrQBSignIn" @('Collection') $true $true
    }
    if ($capture.exitCode -ne 0) {
        $failure = Get-VmrQBAzureTokenFailure -StdErr ([string]$capture.stderr) -ExitCode $capture.exitCode
        return New-VmrQBCheck 'az-token' 'Azure CLI token for Azure DevOps' 'warn' "[$($failure.kind)] $(Limit-VmrQBText ([string]$capture.stderr))" $failure.remedy @('Collection') $failure.authRelated $true
    }
    # Read only the expiry; the token itself is dropped here and never stored or printed.
    $expires = $null
    try { $expires = [DateTimeOffset]::Parse(([string](ConvertFrom-Json -InputObject $capture.stdout).expiresOn), [Globalization.CultureInfo]::InvariantCulture) } catch { }
    $capture = $null
    if ($null -ne $expires) {
        $remaining = $expires - (& $Probes.Now)
        if ($remaining.TotalMinutes -lt 20) {
            return New-VmrQBCheck 'az-token' 'Azure CLI token for Azure DevOps' 'warn' "Token expires in $([int][Math]::Max(0, $remaining.TotalMinutes)) minutes; a full run can take about an hour." "Refresh it first: $script:VmrQBSignIn" @('Collection') $true
        }
        return New-VmrQBCheck 'az-token' 'Azure CLI token for Azure DevOps' 'ok' "Valid for about $([int]$remaining.TotalMinutes) more minutes." '' @('Collection')
    }
    return New-VmrQBCheck 'az-token' 'Azure CLI token for Azure DevOps' 'ok' 'Token acquired.' '' @('Collection')
}

function Test-VmrQBAzureDevOpsAccess {
    <#
    .SYNOPSIS
    Reads the unified-build pipeline definition through the collector's own transport.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][hashtable] $Probes,
        [Parameter(Mandatory)][ValidateSet('dnceng-public', 'dnceng')][string] $Organization
    )
    $project = if ($Organization -eq 'dnceng') { 'internal' } else { 'public' }
    $label = "Azure DevOps $Organization/$project"
    $id = "azdo-$project"
    $session = & $Probes.Session
    try {
        $result = Invoke-VmrReadOnlyRequest -Session $session -Service AzureDevOps -ApiPath 'build/definitions?name=dotnet-unified-build' -Organization $Organization -Project $project
        $count = if ($null -ne $result.PSObject.Properties['value']) { @($result.value).Count } else { 0 }
        if ($count -lt 1) {
            return New-VmrQBCheck $id $label 'warn' 'Signed in, but the dotnet-unified-build pipeline definition was not returned.' 'Confirm your account can see the pipeline in the Azure DevOps UI; a missing definition means no permission or a renamed pipeline.' @('Collection') $false ($Organization -eq 'dnceng')
        }
        return New-VmrQBCheck $id $label 'ok' "Pipeline definition readable (HTTP $($session['lastHttpStatusCode']))." '' @('Collection')
    }
    catch {
        $error = Get-VmrCollectorError -Exception $_.Exception -Scope "$Organization/$project" -Stage 'preflight'
        $status = if ($session.ContainsKey('lastHttpStatusCode')) { " (HTTP $($session['lastHttpStatusCode']))" } else { '' }
        $remedy = if ($error.authenticationRequired) {
            if ($error.signInCommand) { "Run: $($error.signInCommand)" } else { "Run: $script:VmrQBSignIn" }
        } else { 'Retry; if it persists, check VPN/proxy and Azure DevOps service health.' }
        return New-VmrQBCheck $id $label 'warn' "$(Limit-VmrQBText $error.message)$status" $remedy @('Collection') ([bool]$error.authenticationRequired) ($Organization -eq 'dnceng')
    }
    finally { try { $session.tokens.Clear() } catch { } }
}

function Test-VmrQBGitHubAccess {
    [CmdletBinding()]
    param([Parameter(Mandatory)][hashtable] $Probes)
    $checks = [Collections.Generic.List[object]]::new()
    $status = & $Probes.Native '& gh auth status --hostname github.com' 30
    $statusText = ([string]$status.stdout + ' ' + [string]$status.stderr)
    if ($status.exitCode -ne 0) {
        $missing = $statusText -match '(?i)not recognized|command not found|CommandNotFoundException'
        $checks.Add((New-VmrQBCheck 'gh-auth' 'GitHub CLI sign-in' 'fail' (Limit-VmrQBText $statusText) $(if ($missing) { 'Install the GitHub CLI (https://cli.github.com), then run gh auth login.' } else { 'Run: gh auth login --hostname github.com' }) @('Collection') (-not $missing)))
        return $checks.ToArray()
    }
    $checks.Add((New-VmrQBCheck 'gh-auth' 'GitHub CLI sign-in' 'ok' 'Signed in to github.com.' '' @('Collection')))

    $limit = & $Probes.Native '& gh api rate_limit --jq .resources.core.remaining' 30
    $remaining = 0
    if ($limit.exitCode -eq 0 -and [int]::TryParse(([string]$limit.stdout).Trim(), [ref]$remaining)) {
        if ($remaining -lt 300) { $checks.Add((New-VmrQBCheck 'gh-rate' 'GitHub API rate limit' 'warn' "Only $remaining core requests remain; issue/owner evidence may come back partial." 'Wait for the reset, or run again later.' @('Collection'))) }
        else { $checks.Add((New-VmrQBCheck 'gh-rate' 'GitHub API rate limit' 'ok' "$remaining requests remain." '' @('Collection'))) }
    }

    $team = & $Probes.Native '& gh api orgs/dotnet/teams/dnceng --jq .slug' 30
    if ($team.exitCode -eq 0) { $checks.Add((New-VmrQBCheck 'gh-teams' 'GitHub org team read (handle validation)' 'ok' 'dotnet org teams are readable.' '' @('Collection'))) }
    else {
        $checks.Add((New-VmrQBCheck 'gh-teams' 'GitHub org team read (handle validation)' 'warn' "Cannot read dotnet org teams: $(Limit-VmrQBText ([string]$team.stderr) 160)" 'Authorize the GitHub CLI for the dotnet org (SSO) so ./verify/Invoke-Verify.ps1 -Online can verify handles.' @('Collection') $true))
    }
    return $checks.ToArray()
}

function Test-VmrQBStateRoot {
    [CmdletBinding()]
    param([Parameter(Mandatory)][hashtable] $Probes)
    $paths = Get-VmrQBPaths
    $checks = [Collections.Generic.List[object]]::new()
    try {
        [void][IO.Directory]::CreateDirectory($paths.StateRoot)
        $probe = Join-Path $paths.StateRoot ('.preflight-' + [guid]::NewGuid().ToString('N'))
        [IO.File]::WriteAllText($probe, 'x'); [IO.File]::Delete($probe)
        $checks.Add((New-VmrQBCheck 'state-root' 'State directory is writable' 'ok' $paths.StateRoot '' @('Collection', 'Synthesis', 'Sending')))
    }
    catch { $checks.Add((New-VmrQBCheck 'state-root' 'State directory is writable' 'fail' "$($paths.StateRoot): $(Limit-VmrQBText $_.Exception.Message)" 'Set VMR_QB_HOME to a writable directory.' @('Collection', 'Synthesis', 'Sending'))); return $checks.ToArray() }

    # A config that holds your email address inside a git work tree is a commit away from being public, so the run stops until it is
    # safe. Git tracking it (or having it staged) is the worse case: an ignore rule does not protect a tracked file, and
    # `git update-index --skip-worktree` neither removes staged content nor works on a file that is not tracked. Untracked and not
    # ignored, a `git add -A` stages it. Only a definite answer from git stops a run; a git that cannot answer (no repository, a
    # safe.directory refusal) is not a reason to stop. Git is asked from the config's own folder with its file name, so the answer
    # does not depend on how the path is spelled (8.3 short names, symlinks).
    if ((Get-Command git -ErrorAction SilentlyContinue) -and (Test-Path -LiteralPath $paths.ConfigPath)) {
        $holdsEmail = $false
        try { $holdsEmail = -not [string]::IsNullOrWhiteSpace([string](Get-Content -LiteralPath $paths.ConfigPath -Raw | ConvertFrom-Json).recipient) } catch { }
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
            $stopsEveryRun = @('Collection', 'Synthesis', 'Sending')
            if ($indexExit -eq 0 -and $indexed.Count) {
                $checks.Add((New-VmrQBCheck 'state-in-repo' 'Your email address is not at risk of being committed' 'fail' "The local config holds your email address and Git tracks it or has it staged in '$top'. An ignore rule does not protect a tracked file, so the next commit would publish it." "Stop tracking it and keep your copy: git -C `"$configDirectory`" rm --cached -- `"$configName`"  (add -f if Git refuses). Then add an ignore rule for it and run this check again. If it was already committed or pushed, treat the address as exposed." $stopsEveryRun))
            }
            elseif ($notIgnored) {
                $checks.Add((New-VmrQBCheck 'state-in-repo' 'Your email address is not at risk of being committed' 'fail' "The local config holds your email address and sits in the git work tree '$top' without an ignore rule, so a 'git add -A' would stage it." "Add an ignore rule that covers it, such as the line $configName in that repository's .gitignore (or your whole state folder, which also holds your history and reports), then run this check again." $stopsEveryRun))
            }
        }
    }
    try {
        $drive = [IO.DriveInfo]::new([IO.Path]::GetPathRoot($paths.StateRoot))
        $freeGb = [Math]::Round($drive.AvailableFreeSpace / 1GB, 1)
        if ($freeGb -lt 5) { $checks.Add((New-VmrQBCheck 'disk' 'Free disk space for evidence' 'warn' "$freeGb GB free; a run downloads hundreds of MB of logs." 'Free some space or move VMR_QB_HOME to a larger drive.' @('Collection'))) }
        else { $checks.Add((New-VmrQBCheck 'disk' 'Free disk space for evidence' 'ok' "$freeGb GB free." '' @('Collection'))) }
    }
    catch { }
    return $checks.ToArray()
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
        [hashtable] $Probes = @{},
        [switch] $SkipNetwork,
        [switch] $SkipOutlook
    )
    $defaults = Get-VmrQBDefaultProbes
    foreach ($key in $defaults.Keys) { if (-not $Probes.ContainsKey($key)) { $Probes[$key] = $defaults[$key] } }
    $checks = [Collections.Generic.List[object]]::new()
    $add = { param($c) foreach ($item in @($c)) { $checks.Add($item) } }

    # Tool versions
    if ($PSVersionTable.PSVersion -ge [version]'7.2') { & $add (New-VmrQBCheck 'pwsh' 'PowerShell 7.2 or newer' 'ok' "PowerShell $($PSVersionTable.PSVersion)" '' @('Collection', 'Synthesis', 'Sending')) }
    else { & $add (New-VmrQBCheck 'pwsh' 'PowerShell 7.2 or newer' 'fail' "PowerShell $($PSVersionTable.PSVersion)" 'Install PowerShell 7 (winget install Microsoft.PowerShell) and run with pwsh.' @('Collection', 'Synthesis', 'Sending')) }

    # Routing never stops a run: a local entry that does not fit the Guide is matched to the closest team, kept as written or
    # dropped, and listed. This only reports what the load found.
    try {
        $routing = Get-VmrQBRouting
        $routingNotes = @($routing.Warnings)
        if ($routingNotes.Count) {
            & $add (New-VmrQBCheck 'knowledge' 'Routing registry loads' 'warn' "$($routing.Teams.Count) teams; $($routingNotes.Count) owner note(s): $(Limit-VmrQBText ($routingNotes -join ' ') 300)" 'The run carries on and lists these in its run notes. ./common/Invoke-Routing.ps1 diff shows how your routing copy differs from the shipped file; ./common/Invoke-Routing.ps1 refresh-ownership renews the saved Guide ownership copy.' @('Synthesis', 'Sending'))
        }
        elseif (Test-VmrQBLocalRoutingBehind) {
            & $add (New-VmrQBCheck 'knowledge' 'Routing registry loads' 'warn' "$($routing.Teams.Count) teams. Your local routing copy was made from an older knowledge/routing.json." './common/Invoke-Routing.ps1 diff shows what changed and who changed it; adopt takes the shipped version of an entry, acknowledge records that you have reviewed it. Your local file is never overwritten.' @('Synthesis', 'Sending'))
        }
        else { & $add (New-VmrQBCheck 'knowledge' 'Routing registry loads' 'ok' "$($routing.Teams.Count) teams, verified $($routing.VerifiedOn)." '' @('Synthesis', 'Sending')) }
    }
    catch { & $add (New-VmrQBCheck 'knowledge' 'Routing registry loads' 'warn' (Limit-VmrQBText $_.Exception.Message) 'The run carries on without owner routing and shows owners as unidentified. ./common/Invoke-Routing.ps1 diff shows how your routing copy differs from the shipped file; moving local/knowledge/routing.local.json aside makes a fresh copy.' @('Synthesis', 'Sending')) }

    & $add (Test-VmrQBStateRoot -Probes $Probes)

    $paths = Get-VmrQBPaths
    $configResult = Get-VmrQBConfigLenient -Path $paths.ConfigPath
    $config = $configResult.Config
    $emailOn = -not [string]::IsNullOrWhiteSpace([string]$config.recipient)
    if (@($configResult.Problems).Count) {
        & $add (New-VmrQBCheck 'config' 'QB config is valid' 'warn' (Limit-VmrQBText (@($configResult.Problems) -join ' ') 400) "No email is sent until $($paths.ConfigPath) is fixed; the reports are still produced and saved." @('Sending'))
    }
    else {
        $detail = if ($emailOn) { "Reports are emailed to $($config.recipient)." } else { "No recipient configured: reports are saved to $($paths.ReportsDir) and not emailed. To get them by email, copy qb-config.template.json to $($paths.ConfigPath) and set `"recipient`" (see README.md)." }
        & $add (New-VmrQBCheck 'config' 'QB config is valid' 'ok' $detail '' @('Sending'))
    }
    $zoneResult = Resolve-VmrQBTimeZone -Id ([string](Get-VmrQBField $config 'timeZone' '')) -ConfigPath $paths.ConfigPath
    if ($zoneResult.Problem) { & $add (New-VmrQBCheck 'timezone' 'Report time zone resolves' 'warn' $zoneResult.Problem "Remove timeZone from qb-config.local.json to use this computer's time zone, or set it to a valid IANA or Windows time zone id." @('Collection')) }
    else { & $add (New-VmrQBCheck 'timezone' 'Report time zone resolves' 'ok' $zoneResult.Zone.Id '' @('Collection')) }

    # Model run prerequisites. Without the Copilot CLI no narrative can be written; the email then carries a digest of the evidence.
    $copilot = Get-Command copilot -ErrorAction SilentlyContinue
    if ($copilot) { & $add (New-VmrQBCheck 'copilot-cli' 'Copilot CLI is installed' 'ok' $copilot.Source '' @('Synthesis')) }
    else { & $add (New-VmrQBCheck 'copilot-cli' 'Copilot CLI is installed' 'warn' 'The copilot command is not on PATH, so no narrative reports can be written; the email carries a digest built from the collected evidence instead.' 'Install GitHub Copilot CLI and sign in once interactively.' @('Synthesis')) }
    # The skills each report uses are looked for by the skill's own name wherever a plugin holds it (the configured pluginRoot,
    # then COPILOT_HOME and ~/.copilot), so a renamed plugin or a skill that moved is still found. A missing one only affects its report.
    $pluginRoots = @(& $Probes.PluginRoot (Get-VmrQBField $config 'pluginRoot'))
    $searched = if ($pluginRoots.Count) { $pluginRoots -join '; ' } else { 'no plugin folder found under COPILOT_HOME or ~/.copilot/installed-plugins' }
    foreach ($skill in @(
            @{ n = 'pipeline-investigation'; why = 'build-health guidance'; effect = 'The build-health report is still written from the collected evidence, without that guidance.' },
            @{ n = 'flow-analysis'; why = 'code-flow report (the Maestro tools)'; effect = 'The code-flow report is skipped.' })) {
        $holder = Find-VmrQBSkillPlugin -SkillName $skill.n -SearchRoot $pluginRoots
        if ($holder) { & $add (New-VmrQBCheck "skill-$($skill.n)" "Skill $($skill.n)" 'ok' "$($skill.why): $holder" '' @('Synthesis')) }
        else { & $add (New-VmrQBCheck "skill-$($skill.n)" "Skill $($skill.n)" 'warn' "Not found ($($skill.why)). Looked in: $searched. $($skill.effect)" 'Install the dotnet-arcade-skills plugin from dotnet/arcade-skills in Copilot CLI (/plugin), or set pluginRoot in local/qb-config.local.json to the folder that holds it.' @('Synthesis')) }
    }
    $found = @{}
    foreach ($located in @(Find-VmrQBPlugin -SearchRoot $pluginRoots)) { $found[$located.Name] = $located.Path }
    foreach ($plugin in @(@{ n = 'dotnet-binlog'; why = 'binlog drill-down' }, @{ n = 'dotnet-arcade'; why = 'Arcade skills' })) {
        if ($found[$plugin.n]) { & $add (New-VmrQBCheck "plugin-$($plugin.n)" "Plugin $($plugin.n)" 'ok' "$($plugin.why): $($found[$plugin.n])" '' @('Synthesis')) }
        else { & $add (New-VmrQBCheck "plugin-$($plugin.n)" "Plugin $($plugin.n)" 'warn' "Not found ($($plugin.why))." 'Optional; install it from dotnet/arcade-skills for deeper investigations.' @('Synthesis')) }
    }

    if (-not $SkipNetwork) {
        & $add (Test-VmrQBAzureCliToken -Probes $Probes)
        & $add (Test-VmrQBAzureDevOpsAccess -Probes $Probes -Organization 'dnceng')
        & $add (Test-VmrQBAzureDevOpsAccess -Probes $Probes -Organization 'dnceng-public')
        & $add (Test-VmrQBGitHubAccess -Probes $Probes)
    }

    if ('Sending' -in $Scope -and -not $SkipOutlook -and $emailOn) {
        $addresses = @(Get-VmrQBRecipientList ([string]$config.recipient))
        $outlook = & $Probes.Outlook $addresses
        $tryAgain = 'The reports are still produced and saved; the send is tried again at the end of the run.'
        if (-not $outlook.available) { & $add (New-VmrQBCheck 'outlook' 'Outlook is available for sending' 'warn' (Limit-VmrQBText ([string]$outlook.error)) "Email uses classic Outlook through COM. Start classic Outlook (the `"New Outlook`" has no COM interface), or run with -SkipEmail. $tryAgain" @('Sending')) }
        elseif ($outlook.offline) { & $add (New-VmrQBCheck 'outlook' 'Outlook is available for sending' 'warn' 'Outlook is in offline mode.' "Switch Outlook to online mode. $tryAgain" @('Sending')) }
        elseif (-not $outlook.accountFound) { & $add (New-VmrQBCheck 'outlook' 'Outlook is available for sending' 'warn' "Outlook has no account matching $($addresses -join ', ')." "The reports are sent from the Outlook account that matches one of your recipients, so at least one recipient must be your own address. Set recipient in qb-config.local.json to include an address Outlook is signed in with. $tryAgain" @('Sending')) }
        else { & $add (New-VmrQBCheck 'outlook' 'Outlook is available for sending' 'ok' 'Online, account found.' '' @('Sending')) }
    }

    # Only checks that serve a requested scope can block.
    $relevant = @($checks | Where-Object { @($_.scopes | Where-Object { $_ -in $Scope }).Count -gt 0 })
    $blocking = @($relevant | Where-Object status -eq 'fail')
    $warnings = @($relevant | Where-Object status -eq 'warn')
    return [pscustomobject]@{
        ok         = ($blocking.Count -eq 0)
        scope      = $Scope
        checks     = $relevant
        blocking   = $blocking
        warnings   = $warnings
        authErrors = @($relevant | Where-Object { $_.authRelated -and $_.status -in 'fail', 'warn' })
    }
}
