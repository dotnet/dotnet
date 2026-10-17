[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# The internal-authentication path of the collector. A token is not kept for the whole collection, a 401 only means the token held
# may have expired (one fresh token, then it is final), and a request that failed for a reason that may pass is tried again.
$scripts = Split-Path -Parent $PSScriptRoot
. (Join-Path $scripts 'daily\evidence.ps1')

$failures = 0
function Check([string] $Name, [scriptblock] $Body) {
    try { & $Body; Write-Host "PASS: $Name" -ForegroundColor Green }
    catch { $script:failures++; Write-Host "FAIL: $Name -> $($_.Exception.Message)" -ForegroundColor Red }
}
function Assert($Condition, [string] $Message) { if (-not $Condition) { throw $Message } }
function Assert-Throws([scriptblock] $Body, [string] $Pattern) {
    try { & $Body | Out-Null } catch {
        if ($_.Exception.Message -notmatch $Pattern) { throw "threw the wrong message: $($_.Exception.Message)" }
        return
    }
    throw "expected an error matching '$Pattern'"
}

$script:ok = { @{ statusCode = 200; contentType = 'application/json'; headers = @{}; content = '{"count":1,"value":[{"id":1}]}' } }
function New-Response([int] $Status, [hashtable] $Headers = @{}) { @{ statusCode = $Status; contentType = 'application/json'; headers = $Headers; content = '{}' } }

# A session whose transport is scripted: the nth HTTP call gets the nth answer (the last answer repeats).
function New-Scripted([object[]] $Answers, [int] $Retries = 2) {
    $script:http = 0; $script:tokens = 0; $script:waits = [Collections.Generic.List[int]]::new(); $script:answers = $Answers
    $session = New-VmrReadOnlySession -MaxTransientRetries $Retries `
        -TokenProvider { param($t) $script:tokens++; "token-$($script:tokens)" } `
        -Sleep { param($seconds) $script:waits.Add([int]$seconds) } `
        -HttpGet {
            param($uri, $headers, $path, $timeout, $maxBytes)
            $answer = $script:answers[[Math]::Min($script:http, $script:answers.Count - 1)]
            $script:http++
            if ($answer -is [scriptblock]) { return & $answer }
            return $answer
        }
    return $session
}
function Invoke-Internal($Session) { Invoke-VmrReadOnlyRequest $Session AzureDevOps 'build/definitions?name=dotnet-unified-build' dnceng internal }

Check 'A token is kept for a while and asked for again when it is old, so a long collection never runs on an expired one' {
    $session = New-Scripted @($ok)
    [void](Invoke-Internal $session)
    [void](Invoke-Internal $session)
    Assert ($script:tokens -eq 1) 'a fresh token is reused'
    $session.tokenAcquiredUtc['dnceng/internal'] = [DateTime]::UtcNow.AddMinutes(-40)
    [void](Invoke-Internal $session)
    Assert ($script:tokens -eq 2) 'a token older than the limit is asked for again'
    Assert ((Invoke-Internal $session).count -eq 1 -and $script:tokens -eq 2) 'and the new one is reused'
}

Check 'A 401 is tried again once with a fresh token; if that works nothing was wrong with the sign-in' {
    $session = New-Scripted @((New-Response 401), $ok)
    $result = Invoke-Internal $session
    Assert ($result.count -eq 1) 'the second try returned the data'
    Assert ($script:http -eq 2 -and $script:tokens -eq 2) "two requests and two tokens: http=$($script:http) tokens=$($script:tokens)"
    Assert ($session.authenticationFailures.Count -eq 0) 'no sign-in failure is recorded'
    [void](Invoke-Internal $session)
    Assert ($script:tokens -eq 2) 'and the renewed token keeps being used'
}

Check 'A second refusal is a real sign-in problem: final for the run, with the sign-in command, and nothing more is asked' {
    $session = New-Scripted @((New-Response 401))
    Assert-Throws { Invoke-Internal $session } 'HTTP 401'
    Assert ($script:http -eq 2 -and $script:tokens -eq 2) 'one try and one retry with a fresh token'
    Assert ($session.authenticationFailures.ContainsKey('dnceng/internal')) 'the failure is final for the scope'
    Assert-Throws { Invoke-Internal $session } 'HTTP 401'
    Assert ($script:http -eq 2 -and $script:tokens -eq 2) 'later requests are answered at once, without HTTP or a token'
}

Check 'A permission denial (403) or a token that cannot be acquired is not helped by a new token, so it is not retried' {
    $session = New-Scripted @((New-Response 403))
    Assert-Throws { Invoke-Internal $session } 'HTTP 403'
    Assert ($script:http -eq 1 -and $script:tokens -eq 1) 'a 403 is final at once'
    $script:tokens = 0
    $broken = New-VmrReadOnlySession -TokenProvider { param($t) $script:tokens++; throw 'az failed' } -HttpGet { throw 'no request may be made without a token' }
    Assert-Throws { Invoke-Internal $broken } 'token acquisition failed'
    Assert ($script:tokens -eq 1) 'a token that could not be acquired is not asked for again'
}

Check 'The public pipeline needs no sign-in, so a refusal there is never retried with a token' {
    $script:http = 0; $script:tokens = 0
    $session = New-VmrReadOnlySession -TokenProvider { param($t) $script:tokens++; 'x' } -HttpGet { param($u, $h, $p, $t, $m) $script:http++; (New-Response 401) }
    Assert-Throws { Invoke-VmrReadOnlyRequest $session AzureDevOps 'build/builds' } 'HTTP 401'
    Assert ($script:http -eq 1 -and $script:tokens -eq 0) 'one request, no token'
}

Check 'A request that failed for a reason that may pass (HTTP 503, 429, a timeout) is tried again, 2 s then 4 s, then reported' {
    $session = New-Scripted @((New-Response 503), $ok)
    Assert ((Invoke-Internal $session).count -eq 1 -and $script:http -eq 2) 'a 503 followed by success succeeds'
    Assert (($script:waits -join ',') -eq '2') "it waited 2 seconds: $($script:waits -join ',')"
    Assert ($session.requests -eq 2) 'every try counts against the request budget'
    $session = New-Scripted @((New-Response 503))
    Assert-Throws { Invoke-Internal $session } 'HTTP 503'
    Assert ($script:http -eq 3 -and ($script:waits -join ',') -eq '2,4') "three tries with growing waits: http=$($script:http) waits=$($script:waits -join ',')"
    Assert ($script:tokens -eq 1) 'a service problem never costs a token'
    Assert ($session.authenticationFailures.Count -eq 0) 'and is never recorded as a sign-in failure'
}

Check 'The server''s Retry-After is honored (capped), and anything that is not transient is not retried' {
    $session = New-Scripted @((New-Response 429 @{ 'Retry-After' = '7' }), $ok)
    [void](Invoke-Internal $session)
    Assert (($script:waits -join ',') -eq '7') "Retry-After 7 was waited: $($script:waits -join ',')"
    $session = New-Scripted @((New-Response 429 @{ 'Retry-After' = '900' }), $ok)
    [void](Invoke-Internal $session)
    Assert (($script:waits -join ',') -eq '30') 'a long Retry-After is capped at 30 seconds'
    $session = New-Scripted @((New-Response 404))
    Assert-Throws { Invoke-Internal $session } 'HTTP 404'
    Assert ($script:http -eq 1 -and $script:waits.Count -eq 0) 'a 404 is not retried'
}

Check 'A dropped connection or a timeout is tried again; a cut-off download does not stop the next attempt' {
    $session = New-Scripted @({ throw (New-VmrCollectorException -Kind 'transport' -Message 'HTTPS GET failed or timed out') }, $ok)
    Assert ((Invoke-Internal $session).count -eq 1 -and $script:http -eq 2) 'a transport failure is tried again'
    $partial = Join-Path ([IO.Path]::GetTempPath()) ('vmrqb-partial-' + [guid]::NewGuid().ToString('N'))
    [IO.File]::WriteAllText($partial, 'cut off')
    try {
        $script:http = 0; $script:waits = [Collections.Generic.List[int]]::new()
        $script:answers = @({ throw (New-VmrCollectorException -Kind 'transport' -Message 'cut') }, $ok)
        $result = Invoke-VmrEvidenceHttpWithRetry -Session $session -Uri 'https://dev.azure.com/dnceng/internal/_apis/build/builds' -Headers @{} -DownloadPath $partial -ResponseLimit 1MB
        Assert ($result.statusCode -eq 200 -and -not [IO.File]::Exists($partial)) 'the partial file is removed before the next attempt'
    }
    finally { Remove-Item -LiteralPath $partial -Force -ErrorAction SilentlyContinue }
    $session = New-Scripted @({ throw (New-VmrCollectorException -Kind 'transport' -Message 'down') })
    Assert-Throws { Invoke-Internal $session } 'down'
    Assert ($script:http -eq 3) 'after the last try the failure is reported'
}

Check 'A GitHub read that failed for a transient reason is tried again; a sign-in problem or a missing resource is not' {
    $script:gh = 0; $script:waits = [Collections.Generic.List[int]]::new()
    $script:throwTransient = { $e = New-VmrCollectorException -Kind 'github' -Message 'GitHub read failed'; $e.Data['Transient'] = $true; throw $e }
    $session = New-VmrReadOnlySession -Sleep { param($s) $script:waits.Add([int]$s) } -GitHubGet { param($api, $t) $script:gh++; if ($script:gh -lt 3) { & $script:throwTransient }; '{"ok":true}' }
    $data = Invoke-VmrReadOnlyRequest $session GitHub 'repos/dotnet/dotnet/branches'
    Assert ($data.ok -and $script:gh -eq 3 -and ($script:waits -join ',') -eq '2,4') "retried twice then succeeded: calls=$($script:gh) waits=$($script:waits -join ',')"
    $script:gh = 0
    $session = New-VmrReadOnlySession -Sleep { param($s) } -GitHubGet { param($api, $t) $script:gh++; throw (New-VmrCollectorException -Kind 'github' -Message 'GitHub read failed') }
    Assert-Throws { Invoke-VmrReadOnlyRequest $session GitHub 'repos/dotnet/dotnet/branches' } 'GitHub read failed'
    Assert ($script:gh -eq 1) 'a failure that is not marked transient (a 404, a sign-in problem) is not retried'
}

Check 'A session built by hand, without the newer settings, still works with their defaults' {
    $script:http = 0; $script:tokens = 0
    $session = @{
        tokenProvider = { param($t) $script:tokens++; 'x' }; httpGet = { param($u, $h, $p, $t, $m) $script:http++; (& $script:ok) }; githubGet = { param($a, $t) '{}' }
        tokens = @{}; authenticationFailures = @{}; maxPages = 4; maxRequests = 50; requests = 0; timeoutSeconds = 30; maxResponseBytes = 1MB
    }
    Assert ((Invoke-Internal $session).count -eq 1 -and $script:http -eq 1 -and $script:tokens -eq 1) 'works with defaults'
}

Write-Host '' 
if ($failures) { Write-Host "$failures collector-retry test(s) failed." -ForegroundColor Red; exit 1 }
Write-Host 'All collector-retry tests passed.' -ForegroundColor Green
