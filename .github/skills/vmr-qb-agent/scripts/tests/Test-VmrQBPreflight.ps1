[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot '..\common\core.ps1')
. (Join-Path $PSScriptRoot '..\daily\evidence.ps1')
. (Join-Path $PSScriptRoot '..\daily\preflight.ps1')

$failures = 0
function Check([string] $Name, [scriptblock] $Body) {
    try { & $Body; Write-Host "PASS: $Name" -ForegroundColor Green }
    catch { $script:failures++; Write-Host "FAIL: $Name -> $($_.Exception.Message)" -ForegroundColor Red }
}
function Assert($Condition, [string] $Message) { if (-not $Condition) { throw $Message } }

$sandbox = Join-Path ([IO.Path]::GetTempPath()) ('vmrqb-preflight-' + [guid]::NewGuid().ToString('N'))
[void][IO.Directory]::CreateDirectory($sandbox)
$env:VMR_QB_HOME = Join-Path $sandbox 'state'
$secret = 'ey' + 'JhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIn0.c2lnbmF0dXJlLXNlY3JldC12YWx1ZQ'

function New-Response([int] $Status, [string] $Type = 'application/json', [string] $Content = '', [hashtable] $Headers = @{}) {
    [pscustomobject]@{ statusCode = $Status; contentType = $Type; headers = $Headers; content = $Content }
}
function New-FakeSession([scriptblock] $Responder) {
    New-VmrReadOnlySession -TokenProvider { param($t) 'fake-bearer-value' } -HttpGet {
        param($uri, $headers, $path, $timeout, $maxBytes) & $script:responder $uri
    }
}
function New-Probes {
    param([scriptblock] $Native, [scriptblock] $Responder, [scriptblock] $Outlook, [string] $PluginRoot)
    $good = { param($uri) New-Response 200 'application/json' '{"count":1,"value":[{"id":1,"name":"dotnet-unified-build"}]}' }
    $script:responder = if ($Responder) { $Responder } else { $good }
    $pluginRootValue = if ($PluginRoot) { $PluginRoot } else { $allPlugins }
    @{
        Native     = if ($Native) { $Native } else { { param($c, $t) @{ exitCode = 0; stdout = ($(if ($c -match 'get-access-token') { '{"accessToken":"' + $script:secret + '","expiresOn":"' + [DateTimeOffset]::UtcNow.AddHours(1).ToString('o') + '"}' } elseif ($c -match 'rate_limit') { '4999' } else { 'ok' })); stderr = '' } } }
        Session    = { New-FakeSession $null }
        Outlook    = if ($Outlook) { $Outlook } else { { param($s) @{ available = $true; offline = $false; accountFound = $true; error = $null } } }
        PluginRoot = { $pluginRootValue }.GetNewClosure()
        Now        = { [DateTimeOffset]::UtcNow }
    }
}
$allPlugins = Join-Path $sandbox 'plugins-all'
foreach ($p in 'dotnet-helix', 'dotnet-codeflow', 'dotnet-binlog', 'dotnet-arcade') { [void][IO.Directory]::CreateDirectory((Join-Path $allPlugins $p)) }
# The skills the reports use, in the plugins that hold them (they are looked up by the skill's name).
function New-Skill([string] $PluginRoot, [string] $Plugin, [string] $Skill) {
    $dir = Join-Path $PluginRoot "$Plugin\skills\$Skill"; [void][IO.Directory]::CreateDirectory($dir); Set-Content -LiteralPath (Join-Path $dir 'SKILL.md') -Value '# skill'
}
New-Skill $allPlugins 'dotnet-helix' 'pipeline-investigation'
New-Skill $allPlugins 'dotnet-codeflow' 'flow-analysis'
Set-VmrQBConfig -Recipient 'qb@example.com' | Out-Null

Check 'Azure CLI failures are classified by cause, with the right fix and whether they are authentication' {
    $cases = @(
        @{ err = "ERROR: Please run 'az login' to setup account.";                                   kind = 'not-logged-in';      auth = $true },
        @{ err = 'AADSTS70043: The refresh token has expired due to sign-in frequency checks';        kind = 'token-expired';      auth = $true },
        @{ err = 'AADSTS50076: Due to a configuration change, you must use multi-factor authentication'; kind = 'mfa-required';   auth = $true },
        @{ err = 'AADSTS53003: Access has been blocked by Conditional Access policies';              kind = 'conditional-access'; auth = $true },
        @{ err = 'AADSTS50020: User account does not exist in tenant';                                kind = 'wrong-account';      auth = $true },
        @{ err = 'AADSTS65001: The user has not consented';                                           kind = 'consent-required';   auth = $true },
        @{ err = "az : The term 'az' is not recognized as the name of a cmdlet";                      kind = 'az-missing';         auth = $false },
        @{ err = 'ConnectionError: Max retries exceeded with url: /oauth2/token';                     kind = 'network';            auth = $false },
        @{ err = 'something nobody has seen before';                                                  kind = 'unknown';            auth = $false }
    )
    foreach ($case in $cases) {
        $result = Get-VmrQBAzureTokenFailure -StdErr $case.err
        Assert ($result.kind -eq $case.kind) "'$($case.err)' -> $($result.kind), expected $($case.kind)"
        Assert ($result.authRelated -eq $case.auth) "$($case.kind): authRelated should be $($case.auth)"
        Assert ($result.remedy.Length -gt 10) "$($case.kind): needs a remedy"
    }
    Assert ((Get-VmrQBAzureTokenFailure -StdErr 'AADSTS70043 refresh token expired').remedy -match 'az login --allow-no-subscriptions') 'the remedy must be the exact sign-in command'
}

Check 'A token never reaches a check result, even when the CLI error echoes one' {
    $probes = New-Probes -Native { param($c, $t) @{ exitCode = 1; stdout = ''; stderr = "AADSTS70043 expired. debug: Bearer $script:secret and $script:secret" } }
    $check = Test-VmrQBAzureCliToken -Probes $probes
    $json = $check | ConvertTo-Json -Depth 5
    Assert ($check.status -eq 'warn' -and $check.authRelated -and $check.showFirst) 'an expired sign-in is an auth warning, shown first: the run goes on and the internal builds are reported as unknown'
    Assert ($json -notmatch [regex]::Escape($secret)) 'the token leaked into the check result'
    Assert ($json -match 'redacted') 'the redaction marker should show something was removed'
    $ok = Test-VmrQBAzureCliToken -Probes (New-Probes)
    Assert (($ok | ConvertTo-Json) -notmatch [regex]::Escape($secret)) 'a successful probe must not retain the token either'
}

Check 'A token about to expire is a warning, a healthy one is ok' {
    $soon = New-Probes -Native { param($c, $t) @{ exitCode = 0; stdout = '{"accessToken":"x","expiresOn":"' + [DateTimeOffset]::UtcNow.AddMinutes(8).ToString('o') + '"}'; stderr = '' } }
    $check = Test-VmrQBAzureCliToken -Probes $soon
    Assert ($check.status -eq 'warn' -and $check.detail -match 'expires in') "expected an expiry warning: $($check.status) $($check.detail)"
    Assert ((Test-VmrQBAzureCliToken -Probes (New-Probes)).status -eq 'ok') 'a one-hour token is fine'
}

Check 'Azure DevOps access distinguishes sign-in, permission, service and shape failures' {
    $cases = @(
        @{ name = '401';          r = { param($u) New-Response 401 'text/plain' '' };                                                      auth = $true;  match = '401' },
        @{ name = '403';          r = { param($u) New-Response 403 'application/json' '{}' };                                              auth = $true;  match = '403' },
        @{ name = 'sign-in page'; r = { param($u) New-Response 200 'text/html' '<html><title>Sign In</title>login.microsoftonline.com</html>' }; auth = $true;  match = 'sign-in' },
        @{ name = 'redirect';     r = { param($u) New-Response 302 'text/html' '' @{ Location = 'https://login.microsoftonline.com/x' } }; auth = $true;  match = 'sign-in' },
        @{ name = '503';          r = { param($u) New-Response 503 'text/plain' '' };                                                      auth = $false; match = '503' },
        @{ name = 'no definition'; r = { param($u) New-Response 200 'application/json' '{"count":0,"value":[]}' };                          auth = $false; match = 'not returned' }
    )
    foreach ($case in $cases) {
        $probes = New-Probes -Responder $case.r
        $check = Test-VmrQBAzureDevOpsAccess -Probes $probes -Organization dnceng
        Assert ($check.status -eq 'warn' -and $check.showFirst) "$($case.name): an internal access problem warns and is shown first (the run goes on), got $($check.status)"
        Assert ($check.authRelated -eq $case.auth) "$($case.name): authRelated=$($check.authRelated), expected $($case.auth)"
        Assert ($check.detail -match $case.match) "$($case.name): detail should mention '$($case.match)': $($check.detail)"
        if ($case.auth) { Assert ($check.remedy -match 'az login --allow-no-subscriptions') "$($case.name): the sign-in command must be given" }
    }
    $healthy = Test-VmrQBAzureDevOpsAccess -Probes (New-Probes) -Organization dnceng
    Assert ($healthy.status -eq 'ok' -and $healthy.detail -match 'HTTP 200') 'a readable definition is ok'
}

Check 'GitHub: not signed in is auth, missing CLI is not, low rate limit and team-read denial warn' {
    $signedOut = New-Probes -Native { param($c, $t) @{ exitCode = 1; stdout = ''; stderr = 'You are not logged into any GitHub hosts. To log in, run: gh auth login' } }
    $c = @(Test-VmrQBGitHubAccess -Probes $signedOut)
    Assert ($c[0].status -eq 'fail' -and $c[0].authRelated -and $c[0].remedy -match 'gh auth login') 'signed out must be an auth failure with the fix'
    $missing = New-Probes -Native { param($c, $t) @{ exitCode = 1; stdout = ''; stderr = "gh : The term 'gh' is not recognized" } }
    $c = @(Test-VmrQBGitHubAccess -Probes $missing)
    Assert ($c[0].status -eq 'fail' -and -not $c[0].authRelated -and $c[0].remedy -match 'cli\.github\.com') 'a missing CLI is an install problem, not an auth problem'
    $limited = New-Probes -Native { param($c, $t) if ($c -match 'rate_limit') { @{ exitCode = 0; stdout = '42'; stderr = '' } } elseif ($c -match 'teams') { @{ exitCode = 1; stdout = ''; stderr = 'HTTP 403: Resource protected by organization SAML enforcement' } } else { @{ exitCode = 0; stdout = 'ok'; stderr = '' } } }
    $c = @(Test-VmrQBGitHubAccess -Probes $limited)
    Assert (@($c | Where-Object { $_.id -eq 'gh-rate' -and $_.status -eq 'warn' }).Count -eq 1) 'a nearly exhausted limit must warn'
    $team = @($c | Where-Object id -eq 'gh-teams')[0]
    Assert ($team.status -eq 'warn' -and $team.authRelated -and $team.remedy -match 'SSO') 'SAML denial on team read must warn as an auth issue with the SSO hint'
}

Check 'The aggregate separates auth errors, and only checks for the requested scope can block' {
    $deny = { param($u) if ("$u" -match 'dnceng/internal') { New-Response 401 'text/plain' '' } else { New-Response 200 'application/json' '{"count":1,"value":[{"id":1}]}' } }
    $probes = New-Probes -Responder $deny
    $all = Invoke-VmrQBPreflight -Probes $probes -SkipOutlook
    Assert ($all.ok) 'an internal 401 must not stop the run: the report says the internal builds are unknown and the public side carries on'
    Assert (@($all.warnings | Where-Object { $_.id -eq 'azdo-internal' -and $_.showFirst }).Count -eq 1) 'the internal failure is a warning that is listed first'
    Assert (@($all.authErrors | Where-Object id -eq 'azdo-internal').Count -eq 1) 'the internal failure must be listed as an auth error'
    Assert (@($all.blocking | Where-Object id -eq 'azdo-public').Count -eq 0) 'public access still works and must not be blamed'
    $sendOnly = Invoke-VmrQBPreflight -Scope Sending -Probes (New-Probes -Responder $deny) -SkipOutlook
    Assert ($sendOnly.ok) 'when only sending, an internal AzDO failure is irrelevant and must not block'
    # A GitHub sign-in that does not work is the one access problem that stops a run.
    $noGitHub = New-Probes -Native { param($c, $t) if ($c -match 'gh auth status') { @{ exitCode = 1; stdout = ''; stderr = 'You are not logged into any GitHub hosts. To log in, run: gh auth login' } } elseif ($c -match 'get-access-token') { @{ exitCode = 0; stdout = '{"accessToken":"x","expiresOn":"' + [DateTimeOffset]::UtcNow.AddHours(1).ToString('o') + '"}'; stderr = '' } } else { @{ exitCode = 0; stdout = 'ok'; stderr = '' } } }
    $stopped = Invoke-VmrQBPreflight -Probes $noGitHub -SkipOutlook
    Assert (-not $stopped.ok -and @($stopped.blocking | Where-Object id -eq 'gh-auth').Count -eq 1) 'a GitHub sign-in failure stops the run'
    Assert (@($stopped.blocking).Count -eq 1) 'and it is the only thing that stops it'
    Assert (@($sendOnly.checks | Where-Object id -eq 'azdo-internal').Count -eq 0) 'irrelevant checks must be omitted from a scoped result'
}

Check 'Only PowerShell, the state folder, the GitHub sign-in and an unprotected config holding an address can stop a run: a missing skill, Outlook, a bad config or time zone only warn' {
    $partial = Join-Path $sandbox 'plugins-partial'; [void][IO.Directory]::CreateDirectory((Join-Path $partial 'dotnet-helix'))
    New-Skill $partial 'dotnet-helix' 'pipeline-investigation'
    $noOutlook = { param($s) @{ available = $false; offline = $null; accountFound = $false; error = 'Retrieving the COM class factory failed (80040154)' } }
    $r = Invoke-VmrQBPreflight -Probes (New-Probes -PluginRoot $partial -Outlook $noOutlook) -SkipNetwork
    Assert ($r.ok -and @($r.blocking).Count -eq 0) "nothing here may stop the run: $(@($r.blocking | ForEach-Object detail) -join ' | ')"
    $flow = @($r.warnings | Where-Object id -eq 'skill-flow-analysis')
    Assert ($flow.Count -eq 1 -and $flow[0].detail -match 'skipped') 'a missing code-flow skill warns and says its report is skipped'
    Assert (@($r.checks | Where-Object { $_.id -eq 'skill-pipeline-investigation' -and $_.status -eq 'ok' }).Count -eq 1) 'a skill that is installed is ok'
    Assert (@($r.warnings | Where-Object id -eq 'plugin-dotnet-binlog').Count -eq 1) 'an optional plugin only warns'
    $outlook = @($r.warnings | Where-Object id -eq 'outlook')[0]
    Assert ($null -ne $outlook -and $outlook.remedy -match 'New Outlook' -and $outlook.remedy -match 'produced and saved') 'the Outlook warning explains the New Outlook COM limit and that the reports are still produced'
    $copilot = @($r.checks | Where-Object id -eq 'copilot-cli')
    Assert ($copilot.Count -eq 1 -and $copilot[0].status -in 'ok', 'warn') 'a missing Copilot CLI is a warning (the email then carries a digest), never a stop'
    $offline = Invoke-VmrQBPreflight -Scope Sending -Probes (New-Probes -Outlook { param($s) @{ available = $true; offline = $true; accountFound = $true; error = $null } }) -SkipNetwork
    Assert ($offline.ok -and @($offline.warnings | Where-Object { $_.id -eq 'outlook' -and $_.detail -match 'offline' }).Count -eq 1) 'offline Outlook warns'
    $wrongAccount = Invoke-VmrQBPreflight -Scope Sending -Probes (New-Probes -Outlook { param($s) @{ available = $true; offline = $false; accountFound = $false; error = $null } }) -SkipNetwork
    Assert ($wrongAccount.ok -and @($wrongAccount.warnings | Where-Object { $_.id -eq 'outlook' -and $_.detail -match 'no account matching' }).Count -eq 1) 'a missing Outlook account warns'
    # Several recipients: the Outlook account only has to match one of them.
    $matchesQb = { param($addresses) @{ available = $true; offline = $false; accountFound = (@($addresses) -contains 'qb@example.com'); error = $null } }
    '{ "recipient": "team@example.com; qb@example.com" }' | Set-Content -LiteralPath (Get-VmrQBPaths).ConfigPath -Encoding utf8
    $listOk = Invoke-VmrQBPreflight -Scope Sending -Probes (New-Probes -Outlook $matchesQb) -SkipNetwork
    Assert (@($listOk.checks | Where-Object { $_.id -eq 'outlook' -and $_.status -eq 'ok' }).Count -eq 1) 'an Outlook account that matches any one of several recipients is enough'
    '{ "recipient": "team@example.com; other@example.com" }' | Set-Content -LiteralPath (Get-VmrQBPaths).ConfigPath -Encoding utf8
    $listNone = Invoke-VmrQBPreflight -Scope Sending -Probes (New-Probes -Outlook $matchesQb) -SkipNetwork
    Assert (@($listNone.warnings | Where-Object { $_.id -eq 'outlook' -and $_.detail -match 'no account matching team@example\.com, other@example\.com' -and $_.remedy -match 'at least one recipient must be your own' }).Count -eq 1) 'none of them matching warns, names them all and says one must be your own'
    Set-VmrQBConfig -Recipient 'qb@example.com' | Out-Null
    # No config file means no recipient: email is off, which is a valid choice and warns about nothing.
    Remove-Item (Get-VmrQBPaths).ConfigPath -Force
    $noConfig = Invoke-VmrQBPreflight -Scope Sending -Probes (New-Probes -Outlook { param($s) throw 'Outlook must not be probed when no email is wanted' }) -SkipNetwork
    Assert (@($noConfig.blocking).Count -eq 0 -and @($noConfig.warnings).Count -eq 0) "an absent config must not even warn: $(@($noConfig.warnings | ForEach-Object detail) -join ' | ')"
    $configCheck = @($noConfig.checks | Where-Object id -eq 'config')[0]
    Assert ($configCheck.status -eq 'ok' -and $configCheck.detail -match 'not emailed' -and $configCheck.detail -match 'qb-config\.template\.json') 'the check must say email is off and how to turn it on'
    Assert (@($noConfig.checks | Where-Object id -eq 'outlook').Count -eq 0) 'Outlook is irrelevant without a recipient'
    # A config that is wrong only costs the email: it is a warning that says how to fix it, and the reports are still produced.
    '{ "recipient": "nope" }' | Set-Content -LiteralPath (Get-VmrQBPaths).ConfigPath -Encoding utf8
    $badConfig = Invoke-VmrQBPreflight -Scope Sending -Probes (New-Probes) -SkipNetwork -SkipOutlook
    $configWarn = @($badConfig.warnings | Where-Object id -eq 'config')
    Assert ($badConfig.ok -and $configWarn.Count -eq 1 -and $configWarn[0].detail -match 'not an email address' -and $configWarn[0].remedy -match 'still produced') 'an address that is present but invalid warns and says the reports are still produced'
    '{ nope' | Set-Content -LiteralPath (Get-VmrQBPaths).ConfigPath -Encoding utf8
    $brokenConfig = Invoke-VmrQBPreflight -Scope Sending -Probes (New-Probes) -SkipNetwork -SkipOutlook
    Assert ($brokenConfig.ok -and @($brokenConfig.warnings | Where-Object { $_.id -eq 'config' -and $_.detail -match 'not valid JSON' }).Count -eq 1) 'a config that is not JSON warns'
    '{ "recipient": "qb@example.com", "timeZone": "Mars/Phobos" }' | Set-Content -LiteralPath (Get-VmrQBPaths).ConfigPath -Encoding utf8
    $badZone = Invoke-VmrQBPreflight -Probes (New-Probes) -SkipNetwork -SkipOutlook
    Assert ($badZone.ok -and @($badZone.warnings | Where-Object { $_.id -eq 'timezone' -and $_.detail -match 'system time zone' }).Count -eq 1) 'an unknown time zone warns and the system time zone is used'
    Set-VmrQBConfig -Recipient 'qb@example.com' | Out-Null
}

Check 'A routing problem never stops a run: an entry that does not fit the Guide is a warning that names it' {
    $paths = Get-VmrQBPaths -Create
    [void](Get-VmrQBRouting)
    $document = Get-Content -LiteralPath $paths.LocalRoutingPath -Raw | ConvertFrom-Json -Depth 30
    $document.categories = @($document.categories) + @([pscustomobject]@{ id = 'gone'; name = 'Gone'; owners = @('@dotnet/gone-team') })
    [IO.File]::WriteAllText($paths.LocalRoutingPath, ((ConvertTo-Json -InputObject $document -Depth 30) + "`n"), [Text.UTF8Encoding]::new($false))
    $r = Invoke-VmrQBPreflight -Probes (New-Probes) -SkipNetwork -SkipOutlook
    $knowledge = @($r.checks | Where-Object id -eq 'knowledge')
    Assert ($r.ok -and $knowledge.Count -eq 1 -and $knowledge[0].status -eq 'warn' -and $knowledge[0].detail -match 'gone-team') "the problem is a warning that names the team: $($knowledge[0].detail)"
    Set-Content -LiteralPath $paths.LocalRoutingPath -Value '{ not json'
    $broken = Invoke-VmrQBPreflight -Probes (New-Probes) -SkipNetwork -SkipOutlook
    Assert ($broken.ok -and @($broken.checks | Where-Object { $_.id -eq 'knowledge' -and $_.status -eq 'warn' }).Count -eq 1) 'a local routing file that does not parse is a warning too'
    Remove-Item -LiteralPath $paths.LocalRoutingPath -Force
}

Check 'A config holding an email address inside a git work tree stops every run until Git neither tracks it nor would stage it' {
    if (-not (Get-Command git -ErrorAction SilentlyContinue)) { Write-Host '  (git not installed; skipped)'; return }
    $repo = Join-Path $sandbox 'repo'; [void][IO.Directory]::CreateDirectory($repo)
    & git -C $repo init -q 2>$null
    $saved = $env:VMR_QB_HOME
    try {
        $env:VMR_QB_HOME = $repo
        Set-VmrQBConfig -Recipient 'qb@example.com' | Out-Null
        $config = (Get-VmrQBPaths).ConfigPath
        $risk = { @(Test-VmrQBStateRoot -Probes (New-Probes) | Where-Object id -eq 'state-in-repo') }

        # Untracked and not ignored: a 'git add -A' would stage it. That is a failure, not a warning.
        $untracked = @(& $risk)
        Assert ($untracked.Count -eq 1 -and $untracked[0].status -eq 'fail') 'an unprotected config holding an address must be a failure'
        Assert ($untracked[0].remedy -match 'ignore rule' -and $untracked[0].remedy -notmatch 'skip-worktree|VMR_QB_HOME') 'the fix is an ignore rule; skip-worktree cannot protect an untracked file'
        foreach ($scope in 'Collection', 'Synthesis', 'Sending') { Assert ($scope -in $untracked[0].scopes) "it must be able to stop a $scope run" }
        $stopped = Invoke-VmrQBPreflight -Scope Collection -Probes (New-Probes) -SkipNetwork -SkipOutlook
        Assert (-not $stopped.ok -and @($stopped.blocking | Where-Object id -eq 'state-in-repo').Count -eq 1) 'the run must not start, even one that sends no email'

        # Ignored and untracked: safe.
        'qb-config.local.json' | Set-Content (Join-Path $repo '.gitignore') -Encoding utf8
        Assert (@(& $risk).Count -eq 0) 'once ignored, it must not stop the run'
        Assert ((Invoke-VmrQBPreflight -Probes (New-Probes) -SkipNetwork -SkipOutlook).ok) 'and the run may start'

        # An ignore rule does not protect a file Git already holds (force-added here), and skip-worktree would not remove it.
        & git -C $repo add -f -- qb-config.local.json 2>$null
        $tracked = @(& $risk)
        Assert ($tracked.Count -eq 1 -and $tracked[0].status -eq 'fail' -and $tracked[0].detail -match 'tracks it or has it staged') 'a tracked or staged config must stop the run even though an ignore rule exists'
        Assert ($tracked[0].remedy -match 'rm --cached' -and $tracked[0].remedy -match 'exposed' -and $tracked[0].remedy -notmatch 'skip-worktree|VMR_QB_HOME') 'the fix is to stop tracking it, and a committed address counts as exposed'

        # Doing what the fix says clears it, and the file stays on disk.
        & git -C $repo rm --cached -q -- qb-config.local.json 2>$null
        Assert (Test-Path -LiteralPath $config) 'git rm --cached keeps the file'
        Assert (@(& $risk).Count -eq 0) 'untracked and ignored again, it passes'

        # No address, nothing to protect.
        Remove-Item -LiteralPath (Join-Path $repo '.gitignore') -Force
        '{ "recipient": "" }' | Set-Content -LiteralPath $config -Encoding utf8
        Assert (@(& $risk).Count -eq 0) 'a config without an address is not a risk'
    }
    finally { $env:VMR_QB_HOME = $saved }
}

Check 'The CLI prints auth errors first, emits JSON, and exits 2 (the daily run''s preflight code) when blocked' {
    $cli = Join-Path $PSScriptRoot '..\daily\Invoke-Preflight.ps1'
    $out = & pwsh -NoProfile -File $cli -SkipNetwork -SkipOutlook -Scope Collection -Json 2>&1 | Out-String
    $parsed = $out | ConvertFrom-Json
    Assert ($null -ne $parsed.ok -and $null -ne $parsed.checks) 'JSON output must contain ok and checks'
    $text = & pwsh -NoProfile -File $cli -SkipNetwork -SkipOutlook -Scope Collection 2>&1 | Out-String
    Assert ($text -match 'VMR QB preflight') 'the banner is missing'
    Assert ($LASTEXITCODE -in 0, 2) "unexpected exit code $LASTEXITCODE"
}

Check 'The manual preflight survives a config that is not valid JSON: it reads it leniently and carries on' {
    $cli = Join-Path $PSScriptRoot '..\daily\Invoke-Preflight.ps1'
    $stateDir = Join-Path $sandbox 'cli-bad-config'; [void][IO.Directory]::CreateDirectory($stateDir)
    Set-Content -LiteralPath (Join-Path $stateDir 'qb-config.local.json') -Value '{ nope'
    $saved = $env:VMR_QB_HOME
    try {
        $env:VMR_QB_HOME = $stateDir
        $out = & pwsh -NoProfile -File $cli -SkipNetwork -SkipOutlook -Json 2>&1 | Out-String
        $code = $LASTEXITCODE
    }
    finally { if ($null -eq $saved) { Remove-Item Env:\VMR_QB_HOME -ErrorAction SilentlyContinue } else { $env:VMR_QB_HOME = $saved } }
    Assert ($code -eq 0) "exit code $code, not a crash: $out"
    Assert ($null -ne ($out | ConvertFrom-Json).ok) 'the JSON is still produced'
}

Check 'Plugins are found wherever they are installed: configured root first, then any marketplace, dotnet-arcade-skills preferred, and absent ones are reported' {
    $copilotHome = Join-Path $sandbox 'discovery-home\.copilot'
    $installed = Join-Path $copilotHome 'installed-plugins'
    foreach ($relative in @('zeta-market\dotnet-helix', 'dotnet-arcade-skills\dotnet-helix', 'dotnet-arcade-skills\dotnet-codeflow', 'alpha-market\dotnet-binlog')) { [void][IO.Directory]::CreateDirectory((Join-Path $installed $relative)) }
    $configured = Join-Path $sandbox 'configured-plugins'
    [void][IO.Directory]::CreateDirectory((Join-Path $configured 'dotnet-arcade'))
    $roots = @(Get-VmrQBPluginSearchRoots -ConfiguredRoot $configured -CopilotHome $copilotHome -UserProfile (Join-Path $sandbox 'no-such-profile'))
    Assert ($roots[0] -eq $configured) 'a configured pluginRoot is searched first'
    Assert ($roots[1] -eq $installed) 'then the installed-plugins folder itself'
    $marketplaces = @($roots | Select-Object -Skip 2 | ForEach-Object { Split-Path -Leaf $_ })
    Assert (($marketplaces -join ',') -eq 'dotnet-arcade-skills,alpha-market,zeta-market') "marketplaces: dotnet-arcade-skills first, the rest in name order, got $($marketplaces -join ',')"
    $found = @{}; foreach ($plugin in @(Find-VmrQBPlugin -SearchRoot $roots)) { $found[$plugin.Name] = $plugin.Path }
    Assert ($found['dotnet-helix'] -eq (Join-Path $installed 'dotnet-arcade-skills\dotnet-helix')) 'the preferred marketplace wins when two have the same plugin'
    Assert ($found['dotnet-binlog'] -eq (Join-Path $installed 'alpha-market\dotnet-binlog')) 'a plugin in another marketplace is found, not assumed missing'
    Assert ($found['dotnet-arcade'] -eq (Join-Path $configured 'dotnet-arcade')) 'the configured root wins'
    Assert ($null -eq $found['dotnet-compliance']) 'a plugin that is not installed has no path'
    Assert (@(Get-VmrQBPluginSearchRoots -ConfiguredRoot @($configured, (Join-Path $sandbox 'missing-root')) -CopilotHome (Join-Path $sandbox 'nope') -UserProfile (Join-Path $sandbox 'nope')) -notcontains (Join-Path $sandbox 'missing-root')) 'a folder that does not exist is not a search root'
}

Check 'The preflight message names where it looked and how to point at the plugins' {
    $empty = Join-Path $sandbox 'plugins-none'; [void][IO.Directory]::CreateDirectory($empty)
    $r = Invoke-VmrQBPreflight -Probes (New-Probes -PluginRoot $empty) -SkipNetwork
    $missing = @($r.warnings | Where-Object id -eq 'skill-flow-analysis')
    Assert ($missing.Count -eq 1 -and $missing[0].detail -match 'Looked in' -and $missing[0].remedy -match 'pluginRoot') 'a missing skill must say where it looked and name pluginRoot'
    $ok = Invoke-VmrQBPreflight -Probes (New-Probes) -SkipNetwork
    $present = @($ok.checks | Where-Object id -eq 'skill-pipeline-investigation')
    Assert ($present.Count -eq 1 -and $present[0].status -eq 'ok' -and $present[0].detail -match [regex]::Escape($allPlugins)) 'a found skill names the plugin folder it was found in'
}

Check 'A skill is found by its own name, so a renamed plugin, or a skill that moved to another plugin, is still found' {
    $renamed = Join-Path $sandbox 'plugins-renamed'
    New-Skill $renamed 'a-plugin-nobody-named-before' 'flow-analysis'
    New-Skill $renamed 'another-new-plugin' 'pipeline-investigation'
    $r = Invoke-VmrQBPreflight -Probes (New-Probes -PluginRoot $renamed) -SkipNetwork
    Assert (@($r.checks | Where-Object { $_.id -eq 'skill-flow-analysis' -and $_.status -eq 'ok' -and $_.detail -match 'a-plugin-nobody-named-before' }).Count -eq 1) 'the code-flow skill is found in the renamed plugin'
    Assert (@($r.checks | Where-Object { $_.id -eq 'skill-pipeline-investigation' -and $_.status -eq 'ok' }).Count -eq 1) 'and so is the build-health skill'
    Assert ($r.ok) 'and nothing stops the run'
}

Remove-Item $sandbox -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item Env:\VMR_QB_HOME -ErrorAction SilentlyContinue
if ($failures) { Write-Host "$failures preflight test(s) failed." -ForegroundColor Red; exit 1 }
Write-Host '14 offline preflight tests. Injected probes and temporary files only; no network, email, or external changes.' -ForegroundColor Green
