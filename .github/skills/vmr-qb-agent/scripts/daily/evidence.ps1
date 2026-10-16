Set-StrictMode -Version Latest

# daily/evidence.ps1 - Read-only AzDO/GitHub transport and the evidence collector (the first stage of the daily round).
# Dot-sourced by Invoke-Daily.ps1, preflight.ps1, report.ps1 and the tests; it defines functions only and runs nothing.
. (Join-Path $PSScriptRoot '..\common\core.ps1')

# ---------------------------------------------------------------------------------------------
# evidence/Transport.ps1
# ---------------------------------------------------------------------------------------------
# --------------------------------------------------------------------------- #
# Shared pooled HTTP engine.                                                   #
#                                                                              #
# Every evidence GET previously built and disposed its own HttpClient, forcing #
# a fresh TCP + TLS handshake per request. A single pooled client keeps        #
# connections warm, and Prefetch warms read-only GETs concurrently so the      #
# serial collector loop can consume them without further network latency.      #
# Prefetch never paginates: paged or non-success responses are discarded so    #
# the ordinary serial path re-requests them with unchanged semantics.          #
# --------------------------------------------------------------------------- #
if (-not ('VmrEvidenceHttp' -as [type])) {
    Add-Type -TypeDefinition @'
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

public static class VmrEvidenceHttp
{
    private static readonly HttpClient SharedClient = CreateClient();

    public static HttpClient Client { get { return SharedClient; } }

    private static HttpClient CreateClient()
    {
        var handler = new SocketsHttpHandler();
        handler.AllowAutoRedirect = false;
        handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
        handler.MaxConnectionsPerServer = 32;
        handler.PooledConnectionLifetime = TimeSpan.FromMinutes(10);
        handler.PooledConnectionIdleTimeout = TimeSpan.FromMinutes(2);
        var client = new HttpClient(handler, true);
        client.Timeout = Timeout.InfiniteTimeSpan;
        return client;
    }

    // Returns one status string per request: "ok\t<contentType>" or a reason the
    // response was not cached. Never throws; callers fall back to a serial GET.
    public static string[] Prefetch(
        string[] uris, string[] authorizations, string[] accepts, string[] paths,
        int maxConcurrency, int timeoutSeconds, long maxBytes)
    {
        var results = new string[uris.Length];
        using (var gate = new SemaphoreSlim(Math.Max(1, maxConcurrency)))
        {
            var tasks = new List<Task>(uris.Length);
            for (int i = 0; i < uris.Length; i++)
            {
                int index = i;
                tasks.Add(Task.Run(async delegate
                {
                    await gate.WaitAsync().ConfigureAwait(false);
                    try
                    {
                        results[index] = await FetchOne(
                            uris[index], authorizations[index], accepts[index], paths[index],
                            timeoutSeconds, maxBytes).ConfigureAwait(false);
                    }
                    catch (Exception ex) { results[index] = "error:" + ex.GetType().Name; }
                    finally { gate.Release(); }
                }));
            }
            try { Task.WaitAll(tasks.ToArray()); }
            catch (AggregateException) { }
        }
        return results;
    }

    private static async Task<string> FetchOne(
        string uri, string authorization, string accept, string path,
        int timeoutSeconds, long maxBytes)
    {
        string outcome = "error:Unknown";
        using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(timeoutSeconds)))
        using (var request = new HttpRequestMessage(HttpMethod.Get, uri))
        {
            if (!string.IsNullOrEmpty(authorization))
                request.Headers.TryAddWithoutValidation("Authorization", authorization);
            if (!string.IsNullOrEmpty(accept))
                request.Headers.TryAddWithoutValidation("Accept", accept);
            try
            {
                using (var response = await SharedClient.SendAsync(
                    request, HttpCompletionOption.ResponseHeadersRead, cts.Token).ConfigureAwait(false))
                {
                    if (!response.IsSuccessStatusCode) return "status:" + (int)response.StatusCode;
                    foreach (var header in response.Headers)
                    {
                        if (string.Equals(header.Key, "x-ms-continuationtoken", StringComparison.OrdinalIgnoreCase))
                            return "paged";
                    }
                    string contentType = response.Content.Headers.ContentType == null
                        ? string.Empty : response.Content.Headers.ContentType.ToString();
                    if (contentType.IndexOf("text/html", StringComparison.OrdinalIgnoreCase) >= 0) return "html";
                    using (var input = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                    using (var output = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None, 65536, true))
                    {
                        var buffer = new byte[65536];
                        long total = 0;
                        int read;
                        while ((read = await input.ReadAsync(buffer, 0, buffer.Length, cts.Token).ConfigureAwait(false)) > 0)
                        {
                            total += read;
                            if (total > maxBytes) { outcome = "limit"; break; }
                            await output.WriteAsync(buffer, 0, read, cts.Token).ConfigureAwait(false);
                        }
                        if (outcome == "limit") { }
                        else outcome = "ok\t" + contentType;
                    }
                }
            }
            catch (Exception ex) { outcome = "error:" + ex.GetType().Name; }
        }
        if (!outcome.StartsWith("ok", StringComparison.Ordinal))
        {
            try { if (File.Exists(path)) File.Delete(path); } catch { }
        }
        return outcome;
    }
}
'@
}

# Prefetched bodies, keyed by absolute request URI. Populated only after all
# parallel downloads finish, then consumed single-threaded by the collector.
$script:VmrPrefetchCache = @{}
$script:VmrPrefetchDirectory = $null

function Get-VmrPrefetchDirectory {
    if ($null -eq $script:VmrPrefetchDirectory -or -not [IO.Directory]::Exists($script:VmrPrefetchDirectory)) {
        $script:VmrPrefetchDirectory = Join-Path ([IO.Path]::GetTempPath()) ("vmr-prefetch-" + [Guid]::NewGuid().ToString('N'))
        [void][IO.Directory]::CreateDirectory($script:VmrPrefetchDirectory)
    }
    return $script:VmrPrefetchDirectory
}

function Clear-VmrEvidencePrefetch {
    foreach ($entry in @($script:VmrPrefetchCache.Values)) {
        try { if ([IO.File]::Exists($entry.path)) { [IO.File]::Delete($entry.path) } } catch { }
    }
    $script:VmrPrefetchCache = @{}
    if ($script:VmrPrefetchDirectory -and [IO.Directory]::Exists($script:VmrPrefetchDirectory)) {
        try { [IO.Directory]::Delete($script:VmrPrefetchDirectory, $true) } catch { }
    }
    $script:VmrPrefetchDirectory = $null
}

function Get-VmrEvidenceProperty {
    param([AllowNull()][object] $Object, [string] $Name, [AllowNull()][object] $Default = $null)

    if ($null -eq $Object) { return $Default }
    if ($Object -is [Collections.IDictionary]) {
        if ($Object.Contains($Name) -and $null -ne $Object[$Name]) { return $Object[$Name] }
    }
    elseif ($null -ne $Object.PSObject.Properties[$Name] -and $null -ne $Object.PSObject.Properties[$Name].Value) {
        return $Object.PSObject.Properties[$Name].Value
    }
    return $Default
}

function New-VmrCollectorException {
    param(
        [string] $Message,
        [bool] $AuthenticationRequired = $false,
        [AllowNull()][string] $SignInCommand = $null,
        [string] $Kind = 'collection'
    )

    $exception = [InvalidOperationException]::new($Message)
    $exception.Data['VmrCollectorError'] = $true
    $exception.Data['AuthenticationRequired'] = $AuthenticationRequired
    $exception.Data['SignInCommand'] = $SignInCommand
    $exception.Data['Kind'] = $Kind
    return $exception
}

function Get-VmrCollectorError {
    param([Exception] $Exception, [string] $Scope, [string] $Stage)

    $known = $Exception.Data.Contains('VmrCollectorError')
    [pscustomobject]@{
        scope = $Scope
        stage = $Stage
        message = if ($known) { $Exception.Message } else {
            "Collector operation failed ($($Exception.GetType().Name)); untrusted exception text was not retained."
        }
        authenticationRequired = $known -and [bool]$Exception.Data['AuthenticationRequired']
        signInCommand = if ($known -and $Exception.Data['SignInCommand']) {
            [string]$Exception.Data['SignInCommand']
        } else { $null }
    }
}

function Write-VmrEvidenceJson {
    param([string] $Path, [AllowNull()][object] $Value, [switch] $Replace)

    $temporaryPath = "$Path.$([Guid]::NewGuid().ToString('N')).tmp"
    try {
        $json = ConvertTo-Json -InputObject $Value -Depth 100
        [IO.File]::WriteAllText($temporaryPath, $json, [Text.UTF8Encoding]::new($false))
        [IO.File]::Move($temporaryPath, $Path, $Replace.IsPresent)
    }
    finally {
        if ([IO.File]::Exists($temporaryPath)) { [IO.File]::Delete($temporaryPath) }
    }
}

function Write-VmrEvidenceRawJson {
    <#
    .SYNOPSIS
    Atomically persists an already-validated JSON response body verbatim.
    .DESCRIPTION
    The caller parsed this exact text, so re-serializing the parsed object only
    costs time and loses fidelity (ConvertTo-Json re-orders nothing but truncates
    beyond -Depth and rewrites numeric/escape forms). Writing the original text
    keeps the retained evidence byte-faithful to what Azure DevOps returned.
    #>
    param([string] $Path, [string] $Text)

    $temporaryPath = "$Path.$([Guid]::NewGuid().ToString('N')).tmp"
    try {
        [IO.File]::WriteAllText($temporaryPath, $Text, [Text.UTF8Encoding]::new($false))
        [IO.File]::Move($temporaryPath, $Path)
    }
    finally {
        if ([IO.File]::Exists($temporaryPath)) { [IO.File]::Delete($temporaryPath) }
    }
}

function Invoke-VmrEvidenceNativeCapture {
    param([string] $Command, [int] $TimeoutSeconds = 90)

    $process = [Diagnostics.Process]::new()
    $start = [Diagnostics.ProcessStartInfo]::new()
    $start.FileName = (Join-Path $PSHOME 'pwsh.exe')
    $start.UseShellExecute = $false
    $start.CreateNoWindow = $true
    $start.RedirectStandardOutput = $true
    $start.RedirectStandardError = $true
    foreach ($argument in @('-NoProfile', '-NonInteractive', '-EncodedCommand',
        [Convert]::ToBase64String([Text.Encoding]::Unicode.GetBytes(
            "`$ErrorActionPreference = 'Stop'; $Command; exit `$LASTEXITCODE")))) {
        $start.ArgumentList.Add($argument)
    }
    $process.StartInfo = $start
    try {
        [void]$process.Start()
        $stdout = $process.StandardOutput.ReadToEndAsync()
        $stderr = $process.StandardError.ReadToEndAsync()
        if (-not $process.WaitForExit($TimeoutSeconds * 1000)) {
            $process.Kill($true)
            $process.WaitForExit()
            throw (New-VmrCollectorException -Message 'Read-only CLI request exceeded its time limit.' -Kind 'timeout')
        }
        $output = $stdout.GetAwaiter().GetResult()
        $errorOutput = $stderr.GetAwaiter().GetResult()
        if ($output.Length -gt 64MB -or $errorOutput.Length -gt 1MB) {
            throw (New-VmrCollectorException -Message 'Read-only CLI response exceeded its size limit.' -Kind 'limit')
        }
        return @{ exitCode = $process.ExitCode; stdout = $output; stderr = $errorOutput }
    }
    finally {
        $process.Dispose()
    }
}

function Get-VmrAzureCliAccessToken {
    param([int] $TimeoutSeconds = 90)

    $capture = $null
    $token = $null
    try {
        $capture = Invoke-VmrEvidenceNativeCapture -TimeoutSeconds $TimeoutSeconds -Command (
            '& az account get-access-token --resource 499b84ac-1321-427f-aa17-267ca6975798 --output json --only-show-errors')
        if ($capture.exitCode -ne 0) { throw [InvalidOperationException]::new('Token command failed.') }
        $token = ConvertFrom-Json -InputObject $capture.stdout -Depth 10
        $value = [string](Get-VmrEvidenceProperty $token 'accessToken')
        if ([string]::IsNullOrWhiteSpace($value) -or $value -match '\s' -or $value -eq '******') {
            throw [InvalidOperationException]::new('No usable access token.')
        }
        return $value
    }
    catch {
        throw (New-VmrCollectorException -AuthenticationRequired $true -Kind 'token' `
            -SignInCommand 'az login --allow-no-subscriptions' -Message (
                'Azure CLI token acquisition failed. Run az login --allow-no-subscriptions with a Microsoft work account with dnceng/internal access. No alternate identity or provider was attempted.'))
    }
    finally {
        $capture = $null
        $token = $null
        $value = $null
    }
}

function New-VmrAzureHeaders {
    param([AllowEmptyString()][string] $AccessToken, [switch] $Text)

    if ([string]::IsNullOrWhiteSpace($AccessToken) -or $AccessToken -match '\s' -or $AccessToken -eq '******') {
        throw (New-VmrCollectorException -AuthenticationRequired $true -Kind 'token' `
            -SignInCommand 'az login --allow-no-subscriptions' -Message 'Azure CLI returned no usable bearer token.')
    }
    return @{
        Accept = if ($Text) { 'text/plain' } else { 'application/json' }
        Authorization = 'Bearer ' + $AccessToken
    }
}

function Get-VmrEvidenceRequestUri {
    param([string] $Organization, [string] $Project, [string] $ApiPath)

    $baseUri = "https://dev.azure.com/$Organization/$Project/_apis/$ApiPath"
    if ($baseUri -notmatch '[?&]api-version=') {
        $separator = if ($baseUri.Contains('?')) { '&' } else { '?' }
        $baseUri += "${separator}api-version=7.1"
    }
    return $baseUri
}

function Assert-VmrReadOnlyApiPath {
    param([string] $Service, [string] $ApiPath, [string] $Organization, [string] $Project, [switch] $Text)

    $decoded = $ApiPath
    for ($i = 0; $i -lt 4; $i++) {
        if ($decoded -match '[\\\r\n#]' -or $decoded.Contains('..')) {
            throw (New-VmrCollectorException -Message 'ApiPath must be relative and contain no traversal, fragments, or newlines.')
        }
        $next = [Uri]::UnescapeDataString($decoded)
        if ($next -eq $decoded) { break }
        $decoded = $next
    }
    if ($decoded -match '%[0-9a-f]{2}') {
        throw (New-VmrCollectorException -Message 'ApiPath contains excessive URL encoding.')
    }
    if ($Service -eq 'GitHub') {
        if ($decoded -notmatch '^(repos/(dotnet/[a-z0-9_.-]+|microsoft/vstest|NuGet/NuGet\.Client)(/|$)|orgs/dotnet/teams/|search/(issues|code)\?)') {
            throw (New-VmrCollectorException -Message 'GitHub GET is limited to the approved .NET repositories, ownership, and issue-search resources.')
        }
        return
    }
    $projects = @{ 'dnceng-public' = 'public'; dnceng = 'internal'; devdiv = 'DevDiv' }
    if (-not $projects.ContainsKey($Organization) -or $Project -ne $projects[$Organization]) {
        throw (New-VmrCollectorException -Message 'The Azure DevOps organization/project pair is not allowlisted.')
    }
    if ($decoded -notmatch '^(build/(builds|definitions)(/|\?|$)|test/(runs|results)(/|\?|$)|wiki/wikis/)' -or
        $decoded -match '(^|[?&])continuationToken=') {
        throw (New-VmrCollectorException -Message 'Only build, test, and wiki GET resources are supported; pagination is automatic.')
    }
    if ($Text -and $decoded -notmatch '^build/builds/\d+/logs/\d+(?:\?|$)') {
        throw (New-VmrCollectorException -Message 'Text responses are supported only for Azure DevOps build task logs.')
    }
}

function Assert-VmrEvidenceHttpResponse {
    param([object] $Response, [string] $Scope)

    $status = [int]$Response.statusCode
    $contentType = [string]$Response.contentType
    $location = [string](Get-VmrEvidenceProperty $Response.headers 'Location')
    $html = $contentType -match 'text/html'
    $signIn = $status -in @(401, 403) -or
        ($status -ge 200 -and $status -lt 300 -and $html -and
            (Get-VmrEvidenceProperty $Response 'content' '') -match '(?i)\bsign[\s-]*in\b|\blog[\s-]*in\b|login\.microsoftonline\.com|/_signin\b') -or
        ($status -ge 300 -and $status -lt 400 -and
            $location -match '(login\.microsoftonline\.com|login\.windows\.net|[/?]_?signin\b|[/?]login\b)')
    if ($signIn) {
        $reason = if ($status -eq 403) {
            'HTTP 403 permission denied. Signing in again does not grant missing project permissions.'
        } elseif ($status -eq 401) { 'HTTP 401 authentication rejected.' }
        else { 'HTTP sign-in redirect/page returned instead of API evidence.' }
        throw (New-VmrCollectorException -AuthenticationRequired $true -Kind 'authentication' `
            -SignInCommand 'az login --allow-no-subscriptions' -Message (
                "${Scope}: $reason Run az login --allow-no-subscriptions with a Microsoft work account with $Scope access. No fallback was attempted."))
    }
    if ($status -lt 200 -or $status -ge 300) {
        throw (New-VmrCollectorException -Kind 'http' -Message "$Scope GET returned HTTP $status; redirects are not followed.")
    }
    if ($html) {
        throw (New-VmrCollectorException -Kind 'http' -Message "$Scope returned unexpected HTML instead of API evidence; authentication failure is not established.")
    }
}

function Copy-VmrEvidenceStream {
    param([IO.Stream] $InputStream, [IO.Stream] $OutputStream, [long] $MaxBytes,
        [Threading.CancellationToken] $CancellationToken = [Threading.CancellationToken]::None)

    $buffer = [byte[]]::new(65536)
    [long]$total = 0
    while (($read = $InputStream.ReadAsync($buffer, 0, $buffer.Length, $CancellationToken).GetAwaiter().GetResult()) -gt 0) {
        $total += $read
        if ($total -gt $MaxBytes) {
            throw (New-VmrCollectorException -Kind 'limit' -Message "HTTP response exceeded the explicit $MaxBytes byte limit; evidence is incomplete.")
        }
        $OutputStream.Write($buffer, 0, $read)
    }
}

function Invoke-VmrEvidenceHttpGet {
    param([uri] $Uri, [hashtable] $Headers, [string] $OutputPath, [int] $TimeoutSeconds, [long] $MaxBytes)

    if ($Uri.Scheme -ne 'https' -or $Uri.Host -ne 'dev.azure.com' -or $Uri.Port -ne 443 -or
        $Uri.UserInfo -or $Uri.Fragment -or
        $Uri.AbsolutePath -notmatch '^/(dnceng-public/public|dnceng/internal|devdiv/DevDiv)/_apis/') {
        throw (New-VmrCollectorException -Message 'HTTP GET destination is not an allowlisted HTTPS Azure DevOps API.')
    }
    # A warmed body is byte-identical to the serial response for this URI; consume it once.
    if ($script:VmrPrefetchCache.ContainsKey($Uri.AbsoluteUri)) {
        $warmed = $script:VmrPrefetchCache[$Uri.AbsoluteUri]
        $script:VmrPrefetchCache.Remove($Uri.AbsoluteUri)
        try {
            if ([IO.File]::Exists($warmed.path) -and ([IO.FileInfo]::new($warmed.path)).Length -le $MaxBytes) {
                $result = @{ statusCode = 200; headers = @{}; contentType = $warmed.contentType; content = $null }
                if ($OutputPath) { [IO.File]::Move($warmed.path, $OutputPath) }
                else {
                    $result.content = [IO.File]::ReadAllText($warmed.path, [Text.UTF8Encoding]::new($false))
                    [IO.File]::Delete($warmed.path)
                }
                return $result
            }
        }
        catch { }
        try { if ([IO.File]::Exists($warmed.path)) { [IO.File]::Delete($warmed.path) } } catch { }
    }
    $client = [VmrEvidenceHttp]::Client
    $request = [Net.Http.HttpRequestMessage]::new([Net.Http.HttpMethod]::Get, $Uri)
    $cancellation = [Threading.CancellationTokenSource]::new([TimeSpan]::FromSeconds($TimeoutSeconds))
    $response = $null
    $inputStream = $null
    $outputStream = $null
    try {
        foreach ($name in $Headers.Keys) { [void]$request.Headers.TryAddWithoutValidation($name, [string]$Headers[$name]) }
        $response = $client.SendAsync($request, [Net.Http.HttpCompletionOption]::ResponseHeadersRead,
            $cancellation.Token).GetAwaiter().GetResult()
        $responseHeaders = @{}
        foreach ($header in $response.Headers) { $responseHeaders[$header.Key] = $header.Value -join ',' }
        $contentType = [string]$response.Content.Headers.ContentType
        $result = @{ statusCode = [int]$response.StatusCode; headers = $responseHeaders; contentType = $contentType; content = $null }
        if (-not $response.IsSuccessStatusCode) { return $result }
        $inputStream = $response.Content.ReadAsStreamAsync($cancellation.Token).GetAwaiter().GetResult()
        if ($contentType -match 'text/html') {
            $prefix = [byte[]]::new([int][math]::Min(16384, $MaxBytes))
            $length = 0
            while ($length -lt $prefix.Length) {
                $read = $inputStream.ReadAsync($prefix, $length, $prefix.Length - $length, $cancellation.Token).GetAwaiter().GetResult()
                if ($read -eq 0) { break }
                $length += $read
            }
            $result.content = [Text.Encoding]::UTF8.GetString($prefix, 0, $length)
            return $result
        }
        $outputStream = if ($OutputPath) {
            [IO.File]::Open($OutputPath, [IO.FileMode]::CreateNew, [IO.FileAccess]::Write, [IO.FileShare]::None)
        } else { [IO.MemoryStream]::new() }
        Copy-VmrEvidenceStream -InputStream $inputStream -OutputStream $outputStream -MaxBytes $MaxBytes `
            -CancellationToken $cancellation.Token
        if (-not $OutputPath) { $result.content = [Text.Encoding]::UTF8.GetString($outputStream.ToArray()) }
        return $result
    }
    catch {
        if ($_.Exception.Data.Contains('VmrCollectorError')) { throw }
        throw (New-VmrCollectorException -Kind 'transport' -Message 'HTTPS GET failed or timed out; no response body or request credentials were included in this error.')
    }
    finally {
        if ($null -ne $outputStream) { $outputStream.Dispose() }
        if ($null -ne $inputStream) { $inputStream.Dispose() }
        if ($null -ne $response) { $response.Dispose() }
        $request.Dispose()
        $cancellation.Dispose()
    }
}

function New-VmrReadOnlySession {
    param(
        [scriptblock] $TokenProvider = { param($timeout) Get-VmrAzureCliAccessToken -TimeoutSeconds $timeout },
        [scriptblock] $HttpGet = {
            param($uri, $headers, $path, $timeout, $maxBytes)
            Invoke-VmrEvidenceHttpGet -Uri $uri -Headers $headers -OutputPath $path -TimeoutSeconds $timeout -MaxBytes $maxBytes
        },
        [scriptblock] $GitHubGet = {
            param($apiPath, $timeout)
            $quotedPath = "'" + $apiPath.Replace("'", "''") + "'"
            $capture = Invoke-VmrEvidenceNativeCapture -TimeoutSeconds $timeout `
                -Command "& gh api --hostname github.com --method GET --paginate --slurp $quotedPath"
            if ($capture.exitCode -ne 0) {
                $auth = $capture.stderr -match '\bHTTP (401|403)\b|gh auth login|not logged'
                $failure = New-VmrCollectorException -AuthenticationRequired $auth -Kind 'github' `
                    -SignInCommand $(if ($auth) { 'gh auth login --hostname github.com' } else { $null }) `
                    -Message "GitHub read-only GET failed (exit $($capture.exitCode)); CLI diagnostic output was not retained."
                # Only the fact that it is worth another try is kept, never the CLI's text.
                $failure.Data['Transient'] = (-not $auth) -and ([string]$capture.stderr -match '(?i)\bHTTP (429|5\d\d)\b|timed? ?out|connection (reset|refused|closed)|EOF|TLS|temporar|rate limit')
                throw $failure
            }
            return $capture.stdout
        },
        [ValidateRange(1, 1000)][int] $MaxPages = 64,
        [ValidateRange(1, 100000)][int] $MaxRequests = 1600,
        [ValidateRange(1, 600)][int] $TimeoutSeconds = 90,
        [ValidateRange(1, 1073741824)][long] $MaxResponseBytes = 64MB,
        [ValidateRange(1, 64)][int] $PrefetchConcurrency = 8,
        # A token is asked for again when it is older than this (az hands out a fresh one when the cached one is near its end), so a
        # long collection never runs on an expired token.
        [ValidateRange(1, 600)][int] $TokenMaxAgeMinutes = 30,
        # A request that failed for a reason that may pass (a timeout, HTTP 429 or 5xx) is tried again this many times.
        [ValidateRange(0, 5)][int] $MaxTransientRetries = 2,
        [scriptblock] $Sleep = { param($seconds) Start-Sleep -Seconds $seconds }
    )

    return @{
        tokenProvider = $TokenProvider; httpGet = $HttpGet; githubGet = $GitHubGet
        tokens = @{}; tokenAcquiredUtc = @{}; authenticationFailures = @{}
        tokenMaxAgeMinutes = $TokenMaxAgeMinutes; maxTransientRetries = $MaxTransientRetries; sleep = $Sleep
        maxPages = $MaxPages; maxRequests = $MaxRequests; requests = 0
        timeoutSeconds = $TimeoutSeconds; maxResponseBytes = $MaxResponseBytes
        # Warming only applies to the real pooled transport; injected transports stay strictly serial.
        prefetchEnabled = -not $PSBoundParameters.ContainsKey('HttpGet')
        prefetchConcurrency = $PrefetchConcurrency
    }
}

function Use-VmrEvidenceRequestBudget {
    param([hashtable] $Session)

    if ($Session.requests -ge $Session.maxRequests) {
        throw (New-VmrCollectorException -Kind 'limit' -Message "The explicit $($Session.maxRequests) request budget was exhausted; required evidence remains incomplete.")
    }
    $Session.requests++
}

function Get-VmrSessionSetting {
    # A session built by hand (the tests) may lack the newer settings; they then take their defaults.
    param([hashtable] $Session, [string] $Name, [object] $Default)
    if ($Session.ContainsKey($Name) -and $null -ne $Session[$Name]) { return $Session[$Name] }
    return $Default
}

function Wait-VmrEvidenceRetry {
    param([hashtable] $Session, [int] $Seconds)
    $sleep = Get-VmrSessionSetting $Session 'sleep' ({ param($s) Start-Sleep -Seconds $s })
    if ($Seconds -gt 0) { & $sleep $Seconds }
}

function Invoke-VmrEvidenceHttpWithRetry {
    <#
    .SYNOPSIS
    One HTTP GET, tried again (twice by default, 2 s then 4 s, or the server's Retry-After) when it failed for a reason that may pass:
    a timeout or dropped connection, HTTP 429 or a 5xx. Anything else is returned or thrown at once.
    #>
    param([hashtable] $Session, [object] $Uri, [hashtable] $Headers, [AllowNull()][string] $DownloadPath, [long] $ResponseLimit)
    $retries = [int](Get-VmrSessionSetting $Session 'maxTransientRetries' 2)
    for ($attempt = 0; ; $attempt++) {
        $wait = 0
        $response = $null
        try {
            $response = & $Session.httpGet $Uri $Headers $DownloadPath $Session.timeoutSeconds $ResponseLimit
            $status = [int]$response.statusCode
            if ($attempt -ge $retries -or $status -notin @(429, 500, 502, 503, 504)) { return $response }
            $retryAfter = [string](Get-VmrEvidenceProperty $response.headers 'Retry-After')
            if ($retryAfter -match '^\d+$') { $wait = [Math]::Min([int]$retryAfter, 30) }
        }
        catch {
            $kind = if ($_.Exception.Data.Contains('Kind')) { [string]$_.Exception.Data['Kind'] } else { '' }
            if ($attempt -ge $retries -or $kind -ne 'transport') { throw }
        }
        if ($wait -eq 0) { $wait = [int]([Math]::Pow(2, $attempt + 1)) }
        # A download that was cut off leaves a partial file that would stop the next attempt from creating it.
        if ($DownloadPath -and [IO.File]::Exists($DownloadPath)) { try { [IO.File]::Delete($DownloadPath) } catch { Write-Verbose "Could not remove $DownloadPath" } }
        Wait-VmrEvidenceRetry -Session $Session -Seconds $wait
        Use-VmrEvidenceRequestBudget $Session
    }
}

function Invoke-VmrReadOnlyRequest {
    param(
        [hashtable] $Session,
        [ValidateSet('GitHub', 'AzureDevOps')][string] $Service,
        [string] $ApiPath,
        [string] $Organization = 'dnceng-public',
        [string] $Project = 'public',
        [switch] $Text,
        [string] $OutputPath,
        [ValidateRange(0, 1073741824)][long] $MaxResponseBytes = 0
    )

    Assert-VmrReadOnlyApiPath -Service $Service -ApiPath $ApiPath -Organization $Organization -Project $Project -Text:$Text
    $responseLimit = if ($MaxResponseBytes) { [math]::Min($MaxResponseBytes, $Session.maxResponseBytes) } else { $Session.maxResponseBytes }
    if ($OutputPath -and (-not [IO.Path]::IsPathFullyQualified($OutputPath) -or [IO.File]::Exists($OutputPath))) {
        throw (New-VmrCollectorException -Message 'OutputPath must be an absolute, unused file path.')
    }
    if ($Service -eq 'GitHub') {
        Use-VmrEvidenceRequestBudget $Session
        $gitHubRetries = [int](Get-VmrSessionSetting $Session 'maxTransientRetries' 2)
        for ($gitHubAttempt = 0; ; $gitHubAttempt++) {
            try { $raw = & $Session.githubGet $ApiPath $Session.timeoutSeconds; break }
            catch {
                $transient = $_.Exception.Data.Contains('Transient') -and [bool]$_.Exception.Data['Transient']
                if (-not $transient -or $gitHubAttempt -ge $gitHubRetries) { throw }
                Wait-VmrEvidenceRetry -Session $Session -Seconds ([int]([Math]::Pow(2, $gitHubAttempt + 1)))
                Use-VmrEvidenceRequestBudget $Session
            }
        }
        try { $data = ConvertFrom-Json -InputObject $raw -Depth 100 -NoEnumerate }
        catch { throw (New-VmrCollectorException -Message 'GitHub returned invalid JSON; no evidence was accepted.') }
        if ($OutputPath) { Write-VmrEvidenceJson -Path $OutputPath -Value $data }
        return ,$data
    }

    $scope = "$Organization/$Project"
    $headers = @{ Accept = if ($Text) { 'text/plain' } else { 'application/json' } }
    # A sign-in failure that is real is final for the run: every later request for the scope gets the same answer at once.
    if ($Session.authenticationFailures.ContainsKey($scope)) { throw $Session.authenticationFailures[$scope] }
    $tokenRefreshed = $false
    while ($true) {
        try {
            if ($Organization -ne 'dnceng-public') {
                # A token is only kept for a while. An old one is dropped so az is asked again; it hands out a fresh token when the cached
                # one is near its end, so a long collection never runs on an expired one.
                if (-not $Session.ContainsKey('tokenAcquiredUtc')) { $Session['tokenAcquiredUtc'] = @{} }
                $maxAgeMinutes = [int](Get-VmrSessionSetting $Session 'tokenMaxAgeMinutes' 30)
                if ($Session.tokens.ContainsKey($scope) -and $Session.tokenAcquiredUtc.ContainsKey($scope) -and
                    ([DateTime]::UtcNow - $Session.tokenAcquiredUtc[$scope]).TotalMinutes -ge $maxAgeMinutes) { [void]$Session.tokens.Remove($scope) }
                if (-not $Session.tokens.ContainsKey($scope)) {
                    try { $Session.tokens[$scope] = & $Session.tokenProvider $Session.timeoutSeconds; $Session.tokenAcquiredUtc[$scope] = [DateTime]::UtcNow }
                    catch {
                        throw (New-VmrCollectorException -AuthenticationRequired $true -Kind 'token' `
                            -SignInCommand 'az login --allow-no-subscriptions' -Message (
                                "Azure CLI token acquisition failed for $scope. Run az login --allow-no-subscriptions with a Microsoft work account with $scope access. No fallback was attempted."))
                    }
                }
                $headers = New-VmrAzureHeaders -AccessToken $Session.tokens[$scope] -Text:$Text
            }
            $baseUri = Get-VmrEvidenceRequestUri -Organization $Organization -Project $Project -ApiPath $ApiPath
            $uri = $baseUri
            $values = [Collections.Generic.List[object]]::new()
            $seenTokens = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
            for ($pageNumber = 1; $pageNumber -le $Session.maxPages; $pageNumber++) {
                Use-VmrEvidenceRequestBudget $Session
                $downloadPath = if ($Text -and $OutputPath) { "$OutputPath.partial" } else { $null }
                $response = Invoke-VmrEvidenceHttpWithRetry -Session $Session -Uri $uri -Headers $headers -DownloadPath $downloadPath -ResponseLimit $responseLimit
                $Session['lastHttpStatusCode'] = [int]$response.statusCode
                Assert-VmrEvidenceHttpResponse -Response $response -Scope $scope
                if ($Text) {
                    if ($OutputPath) {
                        [IO.File]::Move($downloadPath, $OutputPath)
                        return @{ path = $OutputPath; length = ([IO.FileInfo]$OutputPath).Length }
                    }
                    return [string]$response.content
                }
                # HTTP 204 / an empty body is a definitive "this resource has no content"
                # answer - for example a build rejected at YAML pre-flight never produces a
                # timeline. That is collected evidence, not a malformed or non-JSON response.
                if ([int]$response.statusCode -eq 204 -or [string]::IsNullOrWhiteSpace([string]$response.content)) {
                    if ($pageNumber -ne 1 -or $seenTokens.Count -gt 0) {
                        throw (New-VmrCollectorException -Message 'Azure DevOps returned an empty page mid-pagination; refusing an incomplete success.')
                    }
                    return [pscustomobject]@{ noContent = $true; httpStatusCode = [int]$response.statusCode }
                }
                if ($response.contentType -notmatch 'json') {
                    throw (New-VmrCollectorException -Message 'Azure DevOps returned a non-JSON API response; evidence is incomplete.')
                }
                try { $page = ConvertFrom-Json -InputObject $response.content -Depth 100 -NoEnumerate }
                catch { throw (New-VmrCollectorException -Message 'Azure DevOps returned invalid JSON; evidence is incomplete.') }
                if ($null -eq $page -or $page -is [array] -or $page -isnot [pscustomobject]) {
                    throw (New-VmrCollectorException -Message 'Azure DevOps returned an unexpected JSON shape.')
                }
                if ($OutputPath) {
                    $pageDirectory = "$OutputPath.pages"
                    [void][IO.Directory]::CreateDirectory($pageDirectory)
                    [IO.File]::WriteAllText((Join-Path $pageDirectory ('page-{0:D4}.json' -f $pageNumber)),
                        $response.content, [Text.UTF8Encoding]::new($false))
                }
                $continuation = [string]((Get-VmrEvidenceProperty $response.headers 'x-ms-continuationtoken') -join '')
                if ($null -eq $page.PSObject.Properties['value']) {
                    if ($pageNumber -ne 1 -or $continuation) {
                        throw (New-VmrCollectorException -Message 'Azure DevOps changed pagination shape; refusing an incomplete success.')
                    }
                    if ($OutputPath) { Write-VmrEvidenceRawJson -Path $OutputPath -Text $response.content }
                    return $page
                }
                if ($page.value -isnot [array]) {
                    throw (New-VmrCollectorException -Message 'Azure DevOps list response has no value array.')
                }
                if ($null -ne $page.PSObject.Properties['count'] -and $page.count -ne $page.value.Count) {
                    throw (New-VmrCollectorException -Message 'Azure DevOps page count does not match its value array; evidence is incomplete.')
                }
                foreach ($item in $page.value) { $values.Add($item) }
                if (-not $continuation) {
                    $data = [pscustomobject]@{ count = $values.Count; value = $values.ToArray() }
                    if ($OutputPath) { Write-VmrEvidenceJson -Path $OutputPath -Value $data }
                    return $data
                }
                if (-not $seenTokens.Add($continuation)) {
                    throw (New-VmrCollectorException -Message 'Azure DevOps repeated a continuation token; refusing an incomplete success.')
                }
                $separator = if ($baseUri.Contains('?')) { '&' } else { '?' }
                $uri = "${baseUri}${separator}continuationToken=$([Uri]::EscapeDataString($continuation))"
            }
            throw (New-VmrCollectorException -Kind 'limit' -Message "Pagination exceeded the explicit $($Session.maxPages) page limit; no complete inventory was produced.")
        }
        catch {
            $isAuthentication = $_.Exception.Data.Contains('AuthenticationRequired') -and $_.Exception.Data['AuthenticationRequired']
            # A 401 or a sign-in page may only mean that the token this session held has expired: drop it, ask for a fresh one and try this
            # request again, once. Only when the fresh token is refused too is it a sign-in problem, and then it is final for the run.
            # A 403 (permission) or a token that could not be acquired at all is not helped by a new token.
            if ($isAuthentication -and -not $tokenRefreshed -and $Organization -ne 'dnceng-public' -and
                ([string]$_.Exception.Data['Kind']) -eq 'authentication' -and $_.Exception.Message -match 'HTTP 401|sign-in redirect') {
                $tokenRefreshed = $true
                [void]$Session.tokens.Remove($scope)
                continue
            }
            if ($isAuthentication) {
                $Session.authenticationFailures[$scope] = $_.Exception
                [void]$Session.tokens.Remove($scope)
            }
            throw
        }
        finally {
            $headers.Clear()
        }

    }
}

function Invoke-VmrEvidencePrefetch {
    <#
    .SYNOPSIS
    Best-effort concurrent warm-up of read-only Azure DevOps GETs.
    .DESCRIPTION
    Warms bodies for URIs the serial collector is about to request. It consumes no
    request budget, records no sources, and never throws: anything not warmed is
    fetched normally with unchanged validation, pagination and error semantics.
    Paged, redirected, oversized and non-success responses are deliberately not cached.
    #>
    param(
        [hashtable] $Session,
        [string] $Organization = 'dnceng-public',
        [string] $Project = 'public',
        [string[]] $ApiPaths,
        [switch] $Text,
        [ValidateRange(0, 1073741824)][long] $MaxResponseBytes = 0
    )

    if ($null -eq $Session -or -not $Session.prefetchEnabled) { return 0 }
    if ($null -eq $ApiPaths -or $ApiPaths.Count -lt 2) { return 0 }

    $scope = "$Organization/$Project"
    if ($Session.authenticationFailures.ContainsKey($scope)) { return 0 }

    $headers = @{ Accept = if ($Text) { 'text/plain' } else { 'application/json' } }
    if ($Organization -ne 'dnceng-public') {
        if (-not $Session.tokens.ContainsKey($scope)) { return 0 }
        try { $headers = New-VmrAzureHeaders -AccessToken $Session.tokens[$scope] -Text:$Text }
        catch { return 0 }
    }
    $authorization = if ($headers.ContainsKey('Authorization')) { [string]$headers['Authorization'] } else { '' }
    $accept = [string]$headers['Accept']

    $uris = [Collections.Generic.List[string]]::new()
    $paths = [Collections.Generic.List[string]]::new()
    try {
        $directory = Get-VmrPrefetchDirectory
        foreach ($apiPath in $ApiPaths) {
            try { Assert-VmrReadOnlyApiPath -Service AzureDevOps -ApiPath $apiPath -Organization $Organization -Project $Project -Text:$Text }
            catch { continue }
            $uri = Get-VmrEvidenceRequestUri -Organization $Organization -Project $Project -ApiPath $apiPath
            $absolute = ([uri]$uri).AbsoluteUri
            if ($script:VmrPrefetchCache.ContainsKey($absolute) -or $uris.Contains($absolute)) { continue }
            $uris.Add($absolute)
            $paths.Add((Join-Path $directory ([Guid]::NewGuid().ToString('N'))))
        }
        if ($uris.Count -lt 2) { return 0 }
        $limit = if ($MaxResponseBytes) { [math]::Min($MaxResponseBytes, $Session.maxResponseBytes) } else { $Session.maxResponseBytes }
        $blanks = [string[]]::new($uris.Count)
        $authorizations = [string[]]::new($uris.Count)
        $accepts = [string[]]::new($uris.Count)
        for ($i = 0; $i -lt $uris.Count; $i++) {
            $authorizations[$i] = $authorization
            $accepts[$i] = $accept
            $blanks[$i] = ''
        }
        $outcomes = [VmrEvidenceHttp]::Prefetch($uris.ToArray(), $authorizations, $accepts, $paths.ToArray(),
            $Session.prefetchConcurrency, $Session.timeoutSeconds, $limit)
        $warmed = 0
        for ($i = 0; $i -lt $outcomes.Length; $i++) {
            $outcome = [string]$outcomes[$i]
            if (-not $outcome.StartsWith('ok', [StringComparison]::Ordinal)) { continue }
            $contentType = if ($outcome.Contains("`t")) { $outcome.Split("`t")[1] } else { '' }
            $script:VmrPrefetchCache[$uris[$i]] = @{ path = $paths[$i]; contentType = $contentType }
            $warmed++
        }
        return $warmed
    }
    catch { return 0 }
    finally {
        $authorization = $null
        $authorizations = $null
        $headers.Clear()
    }
}

# ---------------------------------------------------------------------------------------------
# evidence/Collector.ps1
# ---------------------------------------------------------------------------------------------
function Initialize-VmrEvidenceDirectory {
    param([string] $OutputDirectory, [string] $LegacyDirectory)

    if ($OutputDirectory) {
        if (-not [IO.Path]::IsPathFullyQualified($OutputDirectory)) {
            throw 'OutputDirectory must be an absolute, unique run directory.'
        }
        $directory = [IO.Path]::GetFullPath($OutputDirectory)
        if ([IO.Directory]::Exists($directory) -and @([IO.Directory]::EnumerateFileSystemEntries($directory)).Count) {
            throw 'OutputDirectory must be new or empty; evidence from another run must never be reused.'
        }
    }
    else {
        $directory = Join-Path $LegacyDirectory (
            'evidence-{0}-{1}' -f [DateTimeOffset]::UtcNow.ToString('yyyyMMddTHHmmssfffffffZ'), [Guid]::NewGuid().ToString('N'))
    }
    [void][IO.Directory]::CreateDirectory($directory)
    $claim = [IO.File]::Open((Join-Path $directory '.collector-run'), [IO.FileMode]::CreateNew,
        [IO.FileAccess]::Write, [IO.FileShare]::None)
    $claim.Dispose()
    return $directory
}

function Add-VmrEvidenceError {
    param([hashtable] $State, [string] $Scope, [string] $Stage, [string] $Message, [Exception] $Exception, [switch] $Optional)

    if ($null -eq $Exception) { $Exception = New-VmrCollectorException -Message $Message }
    $errorInfo = Get-VmrCollectorError -Exception $Exception -Scope $Scope -Stage $Stage
    if ($Optional -and -not $errorInfo.authenticationRequired) {
        if (-not $State.ContainsKey('optionalErrors')) { $State.optionalErrors = [Collections.Generic.List[object]]::new() }
        $State.optionalErrors.Add($errorInfo)
    }
    else { $State.errors.Add($errorInfo) }
    return $errorInfo
}

function Save-VmrEvidenceSource {
    param(
        [hashtable] $State, [string] $Scope, [string] $Stage, [string] $Name,
        [string] $Service, [string] $ApiPath,
        [string] $Organization = 'dnceng-public', [string] $Project = 'public', [switch] $Text,
        [switch] $Optional, [long] $MaxResponseBytes = 0
    )

    if ($Name -notmatch '^[a-zA-Z0-9_-]+$') { throw 'Invalid collector artifact name.' }
    $extension = if ($Text) { 'log' } else { 'json' }
    $path = Join-Path $State.directory "$Name.$extension"
    $source = [pscustomobject]@{
        name = $Name; path = $path; service = $Service; apiPath = $ApiPath
        organization = $Organization; project = $Project
        requestedAtUtc = [DateTimeOffset]::UtcNow.ToString('o')
        observedAt = $null; observedAtUtc = $null
        status = 'unavailable'; error = $null; authenticationRequired = $false; signInCommand = $null
        rawPagesPath = $null; partialPath = $null; required = -not $Optional
    }
    try {
        $data = Invoke-VmrReadOnlyRequest -Session $State.session -Service $Service -ApiPath $ApiPath `
            -Organization $Organization -Project $Project -Text:$Text -OutputPath $path -MaxResponseBytes $MaxResponseBytes
        if ($null -ne $data -and $null -ne $data.PSObject.Properties['noContent']) {
            # Definitive empty resource. Collected successfully; there is simply nothing there.
            $source.status = 'no-content'
            return [pscustomobject]@{ available = $false; noContent = $true; data = $null; source = $source }
        }
        $source.status = 'available'
        return [pscustomobject]@{ available = $true; noContent = $false; data = $data; source = $source }
    }
    catch {
        $errorInfo = Add-VmrEvidenceError -State $State -Scope $Scope -Stage $Stage -Exception $_.Exception -Optional:$Optional
        $source.error = $errorInfo.message
        $source.authenticationRequired = $errorInfo.authenticationRequired
        $source.signInCommand = $errorInfo.signInCommand
        return [pscustomobject]@{ available = $false; noContent = $false; data = $null; source = $source }
    }
    finally {
        $source.observedAtUtc = [DateTimeOffset]::UtcNow.ToString('o')
        $source.observedAt = $source.observedAtUtc
        if ([IO.Directory]::Exists("$path.pages")) { $source.rawPagesPath = "$path.pages" }
        if ([IO.File]::Exists("$path.partial")) { $source.partialPath = "$path.partial" }
        $State.sources.Add($source)
    }
}

function ConvertTo-VmrEvidenceTime {
    param([AllowNull()][object] $Value)

    if ($Value -is [DateTimeOffset]) { return $Value.ToUniversalTime() }
    if ($Value -is [datetime]) { return ([DateTimeOffset]$Value).ToUniversalTime() }
    $parsed = [DateTimeOffset]::MinValue
    if ($Value -isnot [string] -or
        -not [DateTimeOffset]::TryParse($Value, [Globalization.CultureInfo]::InvariantCulture,
            [Globalization.DateTimeStyles]::AssumeUniversal, [ref]$parsed)) {
        throw (New-VmrCollectorException -Message 'Inventory contains a missing or invalid timestamp.')
    }
    return $parsed.ToUniversalTime()
}

function Test-VmrShippingBranch {
    param([string] $Branch, [string] $Scope)

    return $Branch -cmatch '^refs/heads/(main|release/.+)$' -or
        ($Scope -eq 'internal' -and $Branch -cmatch '^refs/heads/internal/release/.+$')
}

function Get-VmrBuildClassification {
    param([hashtable] $State, [object] $Build, [string] $Scope)

    $branch = [string](Get-VmrEvidenceProperty $Build 'sourceBranch')
    if (-not $branch) {
        [void](Add-VmrEvidenceError $State $Scope 'branch-classification' "Build $($Build.id) has no source branch; its scope is unknown.")
        return @{ include = $true; complete = $false; kind = 'unclassified-branch'; pullRequest = $null }
    }
    if (Test-VmrShippingBranch -Branch $branch -Scope $Scope) {
        return @{ include = $true; complete = $true; kind = 'rolling'; pullRequest = $null }
    }
    if ($Scope -ne 'public' -or $branch -notmatch '^refs/pull/(?<number>[1-9]\d*)/') {
        return @{ include = $false; complete = $true; kind = 'experimental'; pullRequest = $null }
    }
    $number = $Matches.number
    $repository = Get-VmrEvidenceProperty $Build 'repository'
    $identity = @('name', 'id', 'url' | ForEach-Object { [string](Get-VmrEvidenceProperty $repository $_) })
    if (@($identity | Where-Object { $_ -match '^(https://github\.com/)?dotnet/dotnet(?:\.git)?/?$' }).Count -eq 0) {
        [void](Add-VmrEvidenceError $State $Scope 'pr-classification' `
            "Build $($Build.id) has no verified dotnet/dotnet repository identity for PR $number.")
        return @{ include = $true; complete = $false; kind = 'unclassified-pr'; pullRequest = @{ number = $number } }
    }
    $key = "dotnet/dotnet#$number"
    if (-not $State.prCache.ContainsKey($key)) {
        $metadata = Save-VmrEvidenceSource -State $State -Scope $Scope -Stage 'pr-classification' `
            -Name "public-pr-dotnet-dotnet-$number" -Service GitHub -ApiPath "repos/dotnet/dotnet/pulls/$number"
        $classification = @{ include = $true; complete = $false; kind = 'unclassified-pr'; pullRequest = @{ number = $number } }
        if ($metadata.available) {
            $items = @($metadata.data)
            $pr = if ($items.Count -eq 1) { $items[0] } else { $null }
            $title = [string](Get-VmrEvidenceProperty $pr 'title')
            $author = [string](Get-VmrEvidenceProperty (Get-VmrEvidenceProperty $pr 'user') 'login')
            $base = [string](Get-VmrEvidenceProperty (Get-VmrEvidenceProperty $pr 'base') 'ref')
            # author_association distinguishes a team member's own PR (they own its CI, so the QB
            # only monitors it) from codeflow bots and outside contributors that need routing.
            $association = [string](Get-VmrEvidenceProperty $pr 'author_association')
            if ($title -and $author -and $base) {
                $classification = @{
                    include = (Test-VmrShippingBranch -Branch "refs/heads/$base" -Scope 'public')
                    complete = $true; kind = 'pull-request'
                    pullRequest = @{
                        number = [long]$number; title = $title; author = $author; baseRefName = $base
                        authorAssociation = $association
                        isTeamMemberAuthored = ($association -in @('MEMBER', 'OWNER', 'COLLABORATOR'))
                        repository = 'dotnet/dotnet'; metadataPath = $metadata.source.path
                        url = "https://github.com/dotnet/dotnet/pull/$number"
                    }
                }
            }
            else {
                [void](Add-VmrEvidenceError $State $Scope 'pr-classification' `
                    "PR $number metadata is missing its actual title, author, or base branch.")
            }
        }
        $State.prCache[$key] = $classification
    }
    return $State.prCache[$key]
}

function Get-VmrScopedBuilds {
    param(
        [hashtable] $State, [object[]] $Builds, [string] $Scope,
        [ValidateSet('completed', 'active')][string] $Kind,
        [DateTimeOffset] $WindowStart, [DateTimeOffset] $WindowEnd
    )

    $included = [Collections.Generic.List[object]]::new()
    $excluded = [Collections.Generic.List[object]]::new()
    $seen = [Collections.Generic.HashSet[long]]::new()
    $complete = $true
    foreach ($build in $Builds) {
        $id = [string](Get-VmrEvidenceProperty $build 'id')
        if ($id -notmatch '^[1-9]\d*$') {
            [void](Add-VmrEvidenceError $State $Scope "$Kind-inventory" 'Inventory contains a build without a valid ID.')
            $complete = $false
            continue
        }
        if (-not $seen.Add([long]$id)) { continue }
        try {
            $time = ConvertTo-VmrEvidenceTime (Get-VmrEvidenceProperty $build $(if ($Kind -eq 'completed') { 'finishTime' } else { 'queueTime' }))
        }
        catch {
            [void](Add-VmrEvidenceError -State $State -Scope $Scope -Stage "$Kind-inventory-build-$id" -Exception $_.Exception)
            $complete = $false
            continue
        }
        $status = [string](Get-VmrEvidenceProperty $build 'status')
        $reason = $null
        if ($Kind -eq 'completed' -and ($time -lt $WindowStart -or $time -gt $WindowEnd)) { $reason = 'finishTime outside the inclusive window' }
        elseif ($Kind -eq 'completed' -and $status -ne 'completed') {
            $reason = 'not completed'
            $complete = $false
            [void](Add-VmrEvidenceError $State $Scope 'completed-inventory' "Build $id was not completed in the completed response.")
        }
        elseif ($Kind -eq 'active' -and $time -gt $WindowEnd) { $reason = 'queued after observation cutoff' }
        elseif ($Kind -eq 'active' -and $status -notin @('inProgress', 'notStarted', 'postponed', 'cancelling')) {
            $reason = 'not active when observed'
        }
        if ($null -ne $reason) {
            $excluded.Add(@{ buildId = [long]$id; reason = $reason })
            continue
        }
        $classification = Get-VmrBuildClassification -State $State -Build $build -Scope $Scope
        if (-not $classification.include) {
            $excluded.Add(@{ buildId = [long]$id; reason = 'experimental branch or experimental PR base'; classification = $classification })
            continue
        }
        if (-not $classification.complete) { $complete = $false }
        if ($Kind -eq 'completed' -and (Get-VmrEvidenceProperty $build 'result') -notin @('succeeded', 'failed', 'canceled', 'partiallySucceeded')) {
            $complete = $false
            [void](Add-VmrEvidenceError $State $Scope 'completed-inventory' "Completed build $id has an unknown result.")
        }
        $copy = [ordered]@{}
        foreach ($property in $build.PSObject.Properties) { $copy[$property.Name] = $property.Value }
        $copy['result'] = Get-VmrEvidenceProperty $build 'result'
        $copy['sourceBranch'] = Get-VmrEvidenceProperty $build 'sourceBranch'
        $copy['vmrClassification'] = $classification
        $included.Add([pscustomobject]$copy)
    }
    return @{
        complete = $complete
        value = @($included.ToArray() | Sort-Object @{ Expression = {
            Get-VmrEvidenceProperty $_ $(if ($Kind -eq 'completed') { 'finishTime' } else { 'queueTime' })
        } }, id)
        excluded = $excluded.ToArray()
    }
}

function Get-VmrEvidenceProblemTasks {
    param([object[]] $Records, [switch] $IncludeBuildSteps)

    $byId = @{}
    foreach ($record in $Records) { $byId[[string]$record.id] = $record }
    foreach ($record in $Records) {
        $result = Get-VmrEvidenceProperty $record 'result'
        $issues = @(Get-VmrEvidenceProperty $record 'issues' @())
        $errorCount = Get-VmrEvidenceProperty $record 'errorCount' 0
        $warningCount = Get-VmrEvidenceProperty $record 'warningCount' 0
        if ($result -notin @('failed', 'canceled', 'succeededWithIssues') -and
            $issues.Count -eq 0 -and $errorCount -eq 0 -and $warningCount -eq 0 -and
            -not ($IncludeBuildSteps -and $record.type -eq 'Task' -and $record.name -match '^Build(?:\s+\(.+\))?$')) { continue }
        $parentId = [string](Get-VmrEvidenceProperty $record 'parentId')
        $ancestor = $parentId
        $job = if ($record.type -eq 'Job') { $record.name } else { $null }
        $jobId = if ($record.type -eq 'Job') { [string]$record.id } else { $null }
        $visited = [Collections.Generic.HashSet[string]]::new()
        while (-not $jobId -and $ancestor -and $byId.ContainsKey($ancestor) -and $visited.Add($ancestor)) {
            $parent = $byId[$ancestor]
            if ($parent.type -eq 'Job') { $job = $parent.name; $jobId = [string]$parent.id; break }
            $ancestor = [string](Get-VmrEvidenceProperty $parent 'parentId')
        }
        [pscustomobject]@{
            id = $record.id; recordType = $record.type; task = $record.name; job = $job; jobId = $jobId; parentId = $parentId
            result = $result; issues = $issues; errorCount = $errorCount; warningCount = $warningCount
            startTime = (Get-VmrEvidenceProperty $record 'startTime')
            finishTime = (Get-VmrEvidenceProperty $record 'finishTime')
            log = (Get-VmrEvidenceProperty $record 'log')
            workerName = (Get-VmrEvidenceProperty $record 'workerName')
        }
    }
}

function Get-VmrRecordDiagnostics {
    param([object] $Record)

    $issues = @(Get-VmrEvidenceProperty $Record 'issues' @())
    $errors = [math]::Max([int](Get-VmrEvidenceProperty $Record 'errorCount' 0),
        @($issues | Where-Object { (Get-VmrEvidenceProperty $_ 'type') -eq 'error' }).Count)
    $warnings = [math]::Max([int](Get-VmrEvidenceProperty $Record 'warningCount' 0),
        @($issues | Where-Object { (Get-VmrEvidenceProperty $_ 'type') -eq 'warning' }).Count)
    $result = Get-VmrEvidenceProperty $Record 'result'
    $failure = $result -in @('failed', 'canceled') -or $errors -gt 0 -or
        ($result -eq 'succeededWithIssues' -and $warnings -eq 0)
    return @{
        failure = $failure; errors = $errors; warnings = $warnings
        nonblocking = -not $failure -and $result -in @('succeeded', 'succeededWithIssues')
    }
}

function Get-VmrFullEvidenceTimeline {
    param([hashtable] $State, [string] $Scope, [string] $Organization, [string] $Project, [long] $BuildId, [string] $Prefix)

    $key = "$Organization/$Project/$BuildId"
    if ($State.timelineCache.ContainsKey($key)) {
        $cached = $State.timelineCache[$key]
        foreach ($errorInfo in $cached.errors) {
            [void](Add-VmrEvidenceError -State $State -Scope $Scope -Stage "cached-timeline-$BuildId" -Exception (
                New-VmrCollectorException -Message $errorInfo.message -AuthenticationRequired $errorInfo.authenticationRequired `
                    -SignInCommand $errorInfo.signInCommand))
        }
        return $cached
    }
    $errorStart = $State.errors.Count
    $noTimeline = $false
    $records = [Collections.Generic.List[object]]::new()
    $recordsById = @{}
    $paths = [Collections.Generic.List[string]]::new()
    $queue = [Collections.Generic.Queue[string]]::new()
    $scheduled = [Collections.Generic.HashSet[string]]::new([StringComparer]::OrdinalIgnoreCase)
    $resolved = [Collections.Generic.HashSet[string]]::new([StringComparer]::OrdinalIgnoreCase)
    $requiredDetails = [Collections.Generic.HashSet[string]]::new([StringComparer]::OrdinalIgnoreCase)
    $optionalDetails = @{}
    $references = [Collections.Generic.HashSet[string]]::new([StringComparer]::OrdinalIgnoreCase)
    $duplicateRecords = 0
    $repeatedReferences = 0
    $primaryRecordCount = 0
    $queue.Enqueue('')
    [void]$scheduled.Add('')
    $complete = $true
    $rootPath = $null
    while ($queue.Count) {
        $part = $queue.Dequeue()
        if ($resolved.Contains($part)) { continue }
        if ($paths.Count -ge $State.limits.maxTimelinePartsPerBuild) {
            [void](Add-VmrEvidenceError $State $Scope "timeline-$BuildId" 'Timeline-part limit reached; full timeline coverage is incomplete.')
            $complete = $false
            break
        }
        $suffix = if ($part) { "-$part" } else { '' }
        $apiSuffix = if ($part) { "/$part" } else { '' }
        $source = Save-VmrEvidenceSource -State $State -Scope $Scope -Stage "timeline-$BuildId" `
            -Name "$Prefix-timeline$suffix" -Service AzureDevOps -Organization $Organization -Project $Project `
            -ApiPath "build/builds/$BuildId/timeline$apiSuffix"
        if (-not $part) { $rootPath = $source.source.path }
        $paths.Add($source.source.path)
        # A root timeline of HTTP 204 means the build never produced one - the pipeline was
        # rejected before any job ran. That is a complete answer, not missing evidence.
        if ($source.noContent -and -not $part) { $noTimeline = $true; break }
        if (-not $source.available) { $complete = $false; continue }
        $partRecords = @(Get-VmrEvidenceProperty $source.data 'records' @())
        if ($partRecords.Count -eq 0 -or $source.data.PSObject.Properties['records'].Value -isnot [array]) {
            [void](Add-VmrEvidenceError $State $Scope "timeline-$BuildId" "Build $BuildId returned an empty or malformed timeline.")
            $complete = $false
            continue
        }
        if (-not $part) { $primaryRecordCount = $partRecords.Count }
        $timelineId = [string](Get-VmrEvidenceProperty $source.data 'id')
        [void]$resolved.Add($part)
        if ($timelineId) { [void]$resolved.Add($timelineId) }
        foreach ($record in $partRecords) {
            if (-not (Get-VmrEvidenceProperty $record 'id') -or -not (Get-VmrEvidenceProperty $record 'type') -or
                -not (Get-VmrEvidenceProperty $record 'name')) {
                [void](Add-VmrEvidenceError $State $Scope "timeline-$BuildId" "Build $BuildId contains a malformed timeline record.")
                $complete = $false
                continue
            }
            $recordId = [string]$record.id
            if ($recordsById.ContainsKey($recordId)) {
                $duplicateRecords++
                $previous = $recordsById[$recordId]
                if ($previous.type -ne $record.type -or $previous.name -ne $record.name -or
                    (Get-VmrEvidenceProperty $previous 'result') -ne (Get-VmrEvidenceProperty $record 'result') -or
                    (Get-VmrEvidenceProperty $previous 'errorCount' 0) -ne (Get-VmrEvidenceProperty $record 'errorCount' 0) -or
                    (ConvertTo-Json -InputObject @(Get-VmrEvidenceProperty $previous 'issues' @()) -Depth 100 -Compress) -cne
                        (ConvertTo-Json -InputObject @(Get-VmrEvidenceProperty $record 'issues' @()) -Depth 100 -Compress)) {
                    [void](Add-VmrEvidenceError $State $Scope "timeline-$BuildId" "Conflicting repeated timeline record $recordId; both raw snapshots are retained.")
                    $complete = $false
                    $records.Add($record)
                }
            }
            else {
                $recordsById[$recordId] = $record
                $records.Add($record)
            }
            $details = [string](Get-VmrEvidenceProperty (Get-VmrEvidenceProperty $record 'details') 'id')
            if (-not $details) { continue }
            if (-not $references.Add($details)) { $repeatedReferences++ }
            if ($details -notmatch '^[0-9a-f]{8}-(?:[0-9a-f]{4}-){3}[0-9a-f]{12}$') {
                [void](Add-VmrEvidenceError $State $Scope "timeline-$BuildId" 'Timeline contains an invalid child timeline ID.')
                $complete = $false
                continue
            }
            $diagnostics = Get-VmrRecordDiagnostics $record
            # Completed nonblocking Task details are command diagnostics, not missing pipeline Jobs/Tasks.
            if ($record.type -eq 'Task' -and (Get-VmrEvidenceProperty $record 'state') -eq 'completed' -and $diagnostics.nonblocking) {
                $optionalDetails[$details] = @{ timelineId = $details; recordId = $recordId
                    reason = 'Completed nonblocking Task detail; primary pipeline record and warning counts are retained.' }
                continue
            }
            [void]$requiredDetails.Add($details)
            if (-not $resolved.Contains($details) -and $scheduled.Add($details)) {
                $queue.Enqueue($details)
            }
        }
    }
    $missingParents = @($records | ForEach-Object {
        $parent = [string](Get-VmrEvidenceProperty $_ 'parentId')
        if ($parent -and -not $recordsById.ContainsKey($parent)) { $parent }
    } | Sort-Object -Unique)
    if ($missingParents.Count) {
        [void](Add-VmrEvidenceError $State $Scope "timeline-$BuildId" "$($missingParents.Count) parent records are missing from the full pipeline timeline.")
        $complete = $false
    }
    $unresolved = @($requiredDetails | Where-Object { -not $resolved.Contains($_) } | Sort-Object)
    $deferred = @($optionalDetails.Values | Where-Object {
        -not $requiredDetails.Contains($_.timelineId) -and -not $resolved.Contains($_.timelineId)
    } | Sort-Object timelineId)
    $timeline = @{
        status = if ($noTimeline) { 'none' } elseif ($complete) { 'available' } elseif ($records.Count) { 'partial' } else { 'unavailable' }
        noTimeline = $noTimeline
        path = $rootPath; paths = $paths.ToArray(); records = $records.ToArray()
        errors = @($State.errors | Select-Object -Skip $errorStart)
        coverage = @{
            primaryRecordCount = $primaryRecordCount; retainedRecordCount = $records.Count
            retainedJobCount = @($records | Where-Object type -eq Job).Count
            retainedTaskCount = @($records | Where-Object type -eq Task).Count
            requiredDetailIds = @($requiredDetails | Sort-Object); unresolvedDetailIds = $unresolved
            optionalTaskDetails = $deferred; duplicateRecordCount = $duplicateRecords
            repeatedReferenceCount = $repeatedReferences; missingParentIds = $missingParents
            policy = 'Full primary pipeline records plus structural/problem detail timelines. Completed nonblocking Task command details are optional, not additional pipeline coverage.'
        }
    }
    $State.timelineCache[$key] = $timeline
    return $timeline
}

function Get-VmrCascadeTimestamp {
    param([AllowNull()][object] $Value)

    if ($Value -is [DateTimeOffset]) { return $Value.ToUniversalTime() }
    if ($Value -is [datetime]) { return ([DateTimeOffset]$Value).ToUniversalTime() }
    $parsed = [DateTimeOffset]::MinValue
    if ($Value -is [string] -and [DateTimeOffset]::TryParse($Value, [Globalization.CultureInfo]::InvariantCulture,
        [Globalization.DateTimeStyles]::AssumeUniversal, [ref]$parsed)) { return $parsed.ToUniversalTime() }
    return $null
}

function Get-VmrMissingInputCascade {
    param([object] $Task, [object[]] $BuildSteps)

    $sbom = $Task.task -match '\bGenerate\s+SBoM\s+Manifest\b'
    $onFailurePublish = $Task.task -match '\bPublish\b.*\bArtifacts\b.*\(On Failure\)'
    $jobId = [string](Get-VmrEvidenceProperty $Task 'jobId')
    if ($Task.recordType -ne 'Task' -or (-not $sbom -and -not $onFailurePublish) -or -not $jobId) { return $null }
    $messages = @($Task.issues | ForEach-Object { [string](Get-VmrEvidenceProperty $_ 'message') })
    if (($messages -join "`n") -match '\b(ESRP|signing|scanner|segmentation|crash|Unauthorized|Forbidden)\b') { return $null }
    $paths = [Collections.Generic.List[string]]::new()
    $diagnostics = [Collections.Generic.List[string]]::new()
    foreach ($issue in $Task.issues | Where-Object { (Get-VmrEvidenceProperty $_ 'type') -eq 'error' }) {
        $message = ([string](Get-VmrEvidenceProperty $issue 'message')).Trim()
        $match = [regex]::Match($message, '(?i)^Path does not exist:\s*(?<path>[^\r\n]+)$')
        if (-not $match.Success) {
            $match = [regex]::Match($message, '(?i)^(?:Error details:\s*)?Could not find a part of the path [''"](?<path>.+)[''"]\.\s*$')
        }
        if ($match.Success) {
            $path = $match.Groups['path'].Value.Trim().Trim([char[]]@("'", '"'))
            if ($path -notmatch '^(?:[a-zA-Z]:[\\/]|/|\\\\)' -or
                ($sbom -and $path -notmatch '[\\/](prepublish|Shipping)[\\/]?$')) { return $null }
            $paths.Add($path)
            $diagnostics.Add((Protect-VmrEvidenceDiagnostic $message))
        }
        elseif ($message -notmatch '^(?i:Encountered an error in generation workflow\.?|SBOM generation failed with exit code 1\.?|The process [''"].+[''"] failed with exit code 1\.?)$') {
            return $null
        }
    }
    if ($paths.Count -eq 0) { return $null }
    $start = Get-VmrCascadeTimestamp $Task.startTime
    if ($null -eq $start) { return $null }
    $preceding = [Collections.Generic.List[object]]::new()
    foreach ($step in $BuildSteps | Where-Object { (Get-VmrEvidenceProperty $_ 'jobId') -eq $jobId }) {
        $stepStart = Get-VmrCascadeTimestamp $step.startTime
        $stepEnd = Get-VmrCascadeTimestamp $step.finishTime
        if ($null -eq $stepStart -or $null -eq $stepEnd -or $stepEnd -lt $stepStart) { return $null }
        if ($stepStart -ge $start) { continue }
        if ($stepEnd -gt $start) { return $null }
        $preceding.Add(@{ step = $step; finishTime = $stepEnd })
    }
    $latest = @($preceding | Sort-Object finishTime -Descending | Select-Object -First 2)
    if ($latest.Count -eq 0 -or $latest[0].step.result -notin @('failed', 'canceled') -or
        ($latest.Count -gt 1 -and $latest[0].finishTime -eq $latest[1].finishTime)) { return $null }
    $root = $latest[0].step
    return @{
        classification = 'missingInputAfterBuildFailure'; jobId = $jobId
        rootRecordId = $root.id; rootTask = $root.task; rootResult = $root.result
        rootLogId = (Get-VmrEvidenceProperty $root.log 'id')
        rootFinishTimeUtc = $latest[0].finishTime.ToString('o'); downstreamStartTimeUtc = $start.ToString('o')
        missingInputPaths = @($paths | Sort-Object -Unique); diagnosticMessages = $diagnostics.ToArray()
        basis = 'Specific missing-input diagnostic and preceding failed/canceled Build in the same Job; no intervening successful Build.'
    }
}

function Get-VmrFailureSignature {
    <#
    .SYNOPSIS
    What a failed record says, with everything that differs from build to build taken out, so the same failure in many builds has one key.
    .DESCRIPTION
    Built only from what the timeline already holds (the task name and its issue messages), so it costs no download. Version numbers,
    GUIDs, hashes and long numbers (build ids, ports, times) are replaced; the words are kept, so two different errors never share a key.
    $null (no message to go on) means the failure cannot be grouped, and its log is fetched as before.
    #>
    param([Parameter(Mandatory)][object] $Task, [string] $Scope = '')

    # A wrapper message ("Bash exited with code 1", MSB3073) names no cause: it is left out, and a record with nothing else to say cannot be
    # grouped (its log is fetched), because two builds can fail the same wrapper for different reasons.
    $messages = @($Task.issues | ForEach-Object { [string](Get-VmrEvidenceProperty $_ 'message') } | Where-Object {
        -not [string]::IsNullOrWhiteSpace($_) -and
        $_ -notmatch '(?i)MSB3073|Bash exited|Script failed|Cmd\.exe exited|PowerShell exited|Process completed with exit code|The operation was canceled|Encountered an error in generation workflow|^\W*The command .* exited with code'
    })
    if (-not $messages.Count) { return $null }
    $text = (($messages | Select-Object -First 3) -join ' | ').ToLowerInvariant()
    $text = [regex]::Replace($text, '\b\d+(?:\.\d+){2,}[\w.+-]*', '<ver>')
    $text = [regex]::Replace($text, '[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}', '<guid>')
    $text = [regex]::Replace($text, '\b[0-9a-f]{7,}\b', '<hex>')
    $text = [regex]::Replace($text, '(?<![a-z])\d{4,}', '#')
    $text = [regex]::Replace($text, '\s+', ' ').Trim()
    if ($text.Length -gt 400) { $text = $text.Substring(0, 400) }
    return "$Scope|$($Task.task)|$text"
}

function Get-VmrRootLogSelection {
    param(
        [object[]] $Tasks, [int] $MaxLogs, [bool] $Required, [ValidateRange(0, 100)][int] $MaxWarningLogs = 1,
        [object[]] $BuildSteps = @(), [switch] $FullTimelineAvailable,
        # The failures already seen in this run (signature -> the build and log that stand for it), the scope and branch that make two
        # builds comparable, and this build. With them, a failure another build already has a log for is not downloaded again: it keeps its
        # timeline message and says which build stands for it, and it takes no slot of the per-build limit. Without them every failure
        # gets its own log, as before.
        [hashtable] $Signatures, [string] $SignatureScope = '', [long] $BuildId = 0
    )

    $candidates = [Collections.Generic.List[object]]::new()
    $cascades = [Collections.Generic.List[object]]::new()
    $warnings = [Collections.Generic.List[object]]::new()
    $diagnosticsById = @{}
    foreach ($task in $Tasks) { $diagnosticsById[[string]$task.id] = Get-VmrRecordDiagnostics $task }
    $hasTasks = @($Tasks | Where-Object { $_.recordType -eq 'Task' -and $diagnosticsById[[string]$_.id].failure }).Count -gt 0
    foreach ($task in $Tasks) {
        if ($task.recordType -notin @('Task', 'Job')) { continue }
        $diagnostics = $diagnosticsById[[string]$task.id]
        if (-not $diagnostics.failure) {
            $warnings.Add($task)
            continue
        }
        if ($FullTimelineAvailable) {
            $cascade = Get-VmrMissingInputCascade -Task $task -BuildSteps $BuildSteps
            if ($null -ne $cascade) {
                $cascades.Add(@{ task = $task; cascade = $cascade; priority = 3; required = $false; kind = 'cascadeSummary'
                    reason = 'Proven same-job missing-input cascade; full timeline diagnostic retained, separate raw log optional.' })
                continue
            }
        }
        $messages = @($task.issues | ForEach-Object { Get-VmrEvidenceProperty $_ 'message' }) -join "`n"
        $support = $task.task -match 'Copy logs|Copy unmerged|On Failure|Publish (BuildLogs|.*logs)' -or
            ($task.task -match 'SBoM|artifact|publish' -and
                $messages -match '(missing|not found|does not exist|could not find).*(Shipping|output|artifact|director)|(?:Shipping|output|artifact|director).*(missing|not found|does not exist)')
        $priority = if ($support) { 3 } elseif ($task.recordType -eq 'Job') { 2 }
        elseif ($task.task -match '\b(Build|Run Tests?|Validate installers?|Publish)\b') { 0 } else { 1 }
        $candidates.Add(@{
            task = $task; priority = $priority
            kind = 'failure'
            required = $task.recordType -eq 'Task' -or
                ($task.recordType -eq 'Job' -and (
                    [string](Get-VmrEvidenceProperty $task.log 'id') -match '^[1-9]\d*$' -or
                    (-not $hasTasks -and [string]::IsNullOrWhiteSpace($messages))))
            reason = if ($support) { 'Supporting/missing-output candidate; dependency causality is not inferred.' }
                else { 'Distinct root task or job diagnostic.' }
        })
    }
    $selected = [Collections.Generic.List[object]]::new()
    $omitted = [Collections.Generic.List[object]]::new()
    $seenLogs = [Collections.Generic.HashSet[string]]::new()
    $rounds = @{}
    foreach ($candidate in @($candidates.ToArray() | Sort-Object @{ Expression = { $_.task.task } }, @{ Expression = {
            switch ($_.task.result) { 'failed' { 0 }; 'canceled' { 1 }; 'succeededWithIssues' { 2 }; default { 3 } }
        } }, priority, @{ Expression = { $_.task.job } }, @{ Expression = { $_.task.id } })) {
        $group = "$($candidate.task.recordType)|$($candidate.task.task)"
        if (-not $rounds.ContainsKey($group)) { $rounds[$group] = 0 }
        $candidate['representativeRound'] = $rounds[$group]
        if ([string](Get-VmrEvidenceProperty $candidate.task.log 'id') -match '^[1-9]\d*$') { $rounds[$group]++ }
    }
    foreach ($candidate in @($candidates.ToArray() | Sort-Object representativeRound, priority,
        @{ Expression = { $_.task.task } }, @{ Expression = { $_.task.job } }, @{ Expression = { $_.task.id } })) {
        $logId = [string](Get-VmrEvidenceProperty $candidate.task.log 'id')
        if ($logId -notmatch '^[1-9]\d*$') {
            $candidate['omission'] = 'Timeline contains no downloadable log ID.'
            $omitted.Add($candidate)
            continue
        }
        # The same failure in another build (or another leg of this one) is already covered by one raw log.
        $signature = if ($null -ne $Signatures -and $candidate.kind -eq 'failure' -and -not $seenLogs.Contains($logId)) {
            Get-VmrFailureSignature -Task $candidate.task -Scope $SignatureScope
        } else { $null }
        if ($signature -and $Signatures.ContainsKey($signature)) {
            $candidate['sameFailureAs'] = $Signatures[$signature]
            $selected.Add($candidate)
        }
        elseif ($seenLogs.Contains($logId)) { $selected.Add($candidate) }
        elseif ($seenLogs.Count -lt $MaxLogs) {
            [void]$seenLogs.Add($logId)
            if ($signature) { $Signatures[$signature] = @{ buildId = $BuildId; logId = $logId; task = [string]$candidate.task.task } }
            $selected.Add($candidate)
        }
        else {
            $candidate['omission'] = "Explicit per-build root log limit ($MaxLogs) reached."
            $omitted.Add($candidate)
        }
    }
    $warningSelected = [Collections.Generic.List[object]]::new()
    $warningLogs = [Collections.Generic.HashSet[string]]::new()
    foreach ($task in @($warnings.ToArray() | Sort-Object task, job, id)) {
        $logId = [string](Get-VmrEvidenceProperty $task.log 'id')
        if ($warningLogs.Count -ge $MaxWarningLogs) { break }
        if ($logId -match '^[1-9]\d*$' -and -not $seenLogs.Contains($logId) -and $warningLogs.Add($logId)) {
            $warningSelected.Add(@{ task = $task; priority = 4; required = $false; kind = 'warningSample'
                reason = 'Optional nonblocking-warning sample; unsampled warnings remain in the full timeline and summary.' })
        }
    }
    $warningGroups = @($warnings | Group-Object recordType, task, result | ForEach-Object {
        $group = $_.Group
        @{ recordType = $group[0].recordType; task = $group[0].task; result = $group[0].result; recordCount = $group.Count
            warningCount = [int](($group | ForEach-Object { $diagnosticsById[[string]$_.id].warnings } | Measure-Object -Sum).Sum) }
    } | Sort-Object recordType, task, result)
    $warningSummary = @{
        recordCount = $warnings.Count
        taskCount = @($warnings | Where-Object recordType -eq Task).Count
        taskWarningCount = [int](($warnings | Where-Object recordType -eq Task |
            ForEach-Object { $diagnosticsById[[string]$_.id].warnings } | Measure-Object -Sum).Sum)
        groups = $warningGroups
        logSampling = @{ limit = $MaxWarningLogs; selected = $warningSelected.Count; available = 0; unavailable = 0
            notSampled = $warnings.Count - $warningSelected.Count; policy = 'Expected limited sampling, not missing required failure evidence.' }
    }
    return @{ selected = $selected.ToArray(); omitted = $omitted.ToArray()
        warningSelected = $warningSelected.ToArray(); warningSummary = $warningSummary; cascades = $cascades.ToArray() }
}

function Protect-VmrEvidenceDiagnostic {
    param([string] $Text)

    $redacted = [regex]::Replace($Text, '(?i)(Bearer\s+)[a-z0-9._~+/-]+=*', '$1[redacted]')
    return [regex]::Replace($redacted, '(?i)((?:access_token|accessToken|client_secret|password)\s*["=:]+\s*)[^\s",;]+', '$1[redacted]')
}

function Get-VmrCompactDiagnosticExcerpt {
    param([AllowEmptyCollection()][string[]] $Lines, [ValidateRange(1, 65536)][int] $MaxCharacters = 1536)

    $nonempty = @($Lines | Where-Object { -not [string]::IsNullOrWhiteSpace($_) })
    if (-not $nonempty.Count) { return @{ text = $null; truncated = $false } }
    $lineLimit = [math]::Min(4, [math]::Max(1, [math]::Floor(($MaxCharacters + 1) / 2)))
    $selected = if ($nonempty.Count -le $lineLimit) { $nonempty } else {
        @($nonempty | Select-Object -First ([int][math]::Ceiling($lineLimit / 2))) +
            @($nonempty | Select-Object -Last ([int][math]::Floor($lineLimit / 2)))
    }
    $selected = @($selected)
    $perLine = [int][math]::Floor(($MaxCharacters - ($selected.Count - 1)) / $selected.Count)
    $truncated = $nonempty.Count -gt $selected.Count
    $text = @(
        foreach ($line in $selected) {
            $redacted = Protect-VmrEvidenceDiagnostic -Text $line
            if ($redacted.Length -gt $perLine) {
                $truncated = $true
                $redacted.Substring(0, $perLine)
            }
            else { $redacted }
        }
    ) -join "`n"
    return @{ text = $text; truncated = $truncated }
}

function Get-VmrLogDiagnostics {
    param([string] $Path, [int] $MaxLines = 24, [int] $MaxLineLength = 512)

    $first = [Collections.Generic.List[object]]::new()
    $last = [Collections.Generic.Queue[object]]::new()
    $tail = [Collections.Generic.Queue[object]]::new()
    $promotions = [Collections.Generic.HashSet[long]]::new()
    $matchCount = 0
    $lineNumber = 0
    $truncatedLine = $false
    $reader = [IO.File]::OpenText($Path)
    try {
        while ($null -ne ($line = $reader.ReadLine())) {
            $lineNumber++
            $text = Protect-VmrEvidenceDiagnostic -Text $line
            if ($text.Length -gt $MaxLineLength) { $text = $text.Substring(0, $MaxLineLength); $truncatedLine = $true }
            $item = @{ line = $lineNumber; text = $text }
            $tail.Enqueue($item)
            if ($tail.Count -gt $MaxLines) { [void]$tail.Dequeue() }
            if ($line -match '(?i)##\[(error|warning)\]|\berror\b|exception|fatal|timed?\s*out|cancel[le]+d|agent.*(lost|disconnect)|maximum.*minutes|throttl|Text file busy|exceeding usage|Unauthorized|publishing assets:') {
                $matchCount++
                if ($first.Count -lt [math]::Ceiling($MaxLines / 2)) { $first.Add($item) }
                else {
                    $last.Enqueue($item)
                    if ($last.Count -gt [math]::Floor($MaxLines / 2)) { [void]$last.Dequeue() }
                }
            }
            foreach ($link in [regex]::Matches($line, 'https://dev\.azure\.com/dnceng/internal/_build/results\?[^\s<>"'')]+')) {
                $id = [regex]::Match($link.Value, '[?&]buildId=(?<id>[1-9]\d*)')
                if ($id.Success) { [void]$promotions.Add([long]$id.Groups['id'].Value) }
            }
        }
    }
    finally {
        $reader.Dispose()
    }
    return @{
        excerpts = if ($matchCount) { @($first.ToArray()) + @($last.ToArray()) } else { @($tail.ToArray()) }
        excerptsTruncated = $truncatedLine -or $matchCount -gt $MaxLines -or ($matchCount -eq 0 -and $lineNumber -gt $MaxLines)
        scannedLines = $lineNumber; matchingLines = $matchCount; promotionBuildIds = @($promotions | Sort-Object)
    }
}

function Get-VmrBuildEvidence {
    param(
        [hashtable] $State, [string] $Scope, [string] $Organization, [string] $Project,
        [object] $Build, [long] $RootBuildId, [long] $ParentBuildId, [int] $Depth = 0,
        [hashtable] $PromotionState
    )

    $id = [long]$Build.id
    if (-not $RootBuildId) { $RootBuildId = $id }
    if ($null -eq $PromotionState) {
        $PromotionState = @{ count = 0; seen = [Collections.Generic.HashSet[string]]::new() }
        [void]$PromotionState.seen.Add("$Organization/$Project/$id")
    }
    $prefix = if ($Depth) { "internal-promotion-$id" } else { "$Scope-build-$id" }
    $timeline = Get-VmrFullEvidenceTimeline -State $State -Scope $Scope -Organization $Organization `
        -Project $Project -BuildId $id -Prefix $prefix
    $tasks = @(Get-VmrEvidenceProblemTasks -Records $timeline.records)
    if ($timeline.noTimeline) {
        # The pipeline was rejected before any job ran, so there are no timeline records.
        # The build's own pre-flight validationResults are the initiating diagnosis.
        $validation = @(Get-VmrEvidenceProperty $Build 'validationResults' @() |
            Where-Object { (Get-VmrEvidenceProperty $_ 'result') -in @('error', 'warning') })
        if ($validation.Count) {
            $tasks = @([pscustomobject]@{
                id = "preflight-$id"; recordType = 'PipelineValidation'; task = 'Pipeline validation'
                job = 'Pipeline pre-flight'; jobId = $null; parentId = $null; result = 'failed'
                issues = @($validation | ForEach-Object {
                    [pscustomobject]@{
                        type = (Get-VmrEvidenceProperty $_ 'result')
                        message = (Get-VmrEvidenceProperty $_ 'message')
                    }
                })
                errorCount = @($validation | Where-Object { (Get-VmrEvidenceProperty $_ 'result') -eq 'error' }).Count
                warningCount = 0
                startTime = (Get-VmrEvidenceProperty $Build 'startTime')
                finishTime = (Get-VmrEvidenceProperty $Build 'finishTime')
                log = $null; workerName = $null
            })
        }
        else {
            [void](Add-VmrEvidenceError $State $Scope "timeline-$id" (
                "Build $id produced no timeline and reported no pipeline validation results; its failure cannot be diagnosed."))
        }
    }
    $buildSteps = @(Get-VmrEvidenceProblemTasks -Records $timeline.records -IncludeBuildSteps |
        Where-Object { $_.recordType -eq 'Task' -and $_.task -match '^Build(?:\s+\(.+\))?$' })
    $entry = [ordered]@{
        organization = $Organization; project = $Project; buildId = $id
        definitionId = (Get-VmrEvidenceProperty (Get-VmrEvidenceProperty $Build 'definition') 'id')
        sourceBranch = (Get-VmrEvidenceProperty $Build 'sourceBranch'); result = (Get-VmrEvidenceProperty $Build 'result')
        finishTime = (Get-VmrEvidenceProperty $Build 'finishTime')
        timelineStatus = $timeline.status; timelinePath = $timeline.path
        supplementalTimelinePaths = @($timeline.paths | Where-Object { $_ -ne $timeline.path })
        timelineCoverage = $timeline.coverage
        tasks = $tasks; warningSample = ($Scope -eq 'public' -and (Get-VmrEvidenceProperty $Build 'result') -eq 'succeeded')
    }
    if ($Depth) {
        $entry['buildId'] = $RootBuildId
        $entry['promotionBuildId'] = $id
        $entry['parentBuildId'] = $ParentBuildId
        $entry['rootScope'] = $Scope
    }
    $required = if ($Depth) { $tasks.Count -gt 0 } else {
        (Get-VmrEvidenceProperty $Build 'result') -in @('failed', 'canceled', 'partiallySucceeded')
    }
    # One raw log per distinct failure: a failure another build already has a log for keeps its timeline message and points at that build.
    # Promotion builds (followed from a publish log) are never grouped; each is read in full.
    if (-not $State.ContainsKey('failureSignatures')) { $State['failureSignatures'] = @{} }
    $signatureArguments = if ($Depth) { @{} } else {
        # Every pull request has its own branch name; the same failure across pull requests is one failure.
        $signatureBranch = ([string](Get-VmrEvidenceProperty $Build 'sourceBranch')) -replace '^refs/pull/\d+/', 'refs/pull/*/'
        @{ Signatures = $State.failureSignatures; SignatureScope = "$Scope|$signatureBranch"; BuildId = $id }
    }
    $selection = Get-VmrRootLogSelection -Tasks $tasks -MaxLogs $State.limits.maxRootLogsPerBuild -Required $required `
        -MaxWarningLogs $State.limits.maxWarningLogsPerBuild -BuildSteps $buildSteps `
        -FullTimelineAvailable:($timeline.status -eq 'available') @signatureArguments
    $entry['warningSummary'] = $selection.warningSummary
    $entry['cascadeSummaries'] = @($selection.cascades | ForEach-Object {
        @{ recordId = $_.task.id; task = $_.task.task; jobId = $_.task.jobId; cascade = $_.cascade }
    })
    if ($Depth) { $State.promotions.Add([pscustomobject]$entry) }
    if ($required -and $timeline.status -ne 'unavailable' -and $tasks.Count -eq 0) {
        [void](Add-VmrEvidenceError $State $Scope "root-logs-$id" "Problem build $id has no problem records; a root cause is not established.")
    }
    # Warm this build's required root logs concurrently before the serial loop reads them.
    [void](Invoke-VmrEvidencePrefetch -Session $State.session -Organization $Organization -Project $Project -Text `
        -ApiPaths @($selection.selected | Where-Object {
                $_.kind -ne 'cascadeSummary' -and -not $_.ContainsKey('omission') -and -not $_.ContainsKey('sameFailureAs') -and
                (Get-VmrEvidenceProperty $_.task.log 'id')
            } | ForEach-Object { "build/builds/$id/logs/$(Get-VmrEvidenceProperty $_.task.log 'id')" } |
            Where-Object { -not $State.logCache.ContainsKey("$Organization/$Project/$id/$($_ -replace '^.*/', '')") }))
    foreach ($candidate in @($selection.omitted) + @($selection.selected) + @($selection.warningSelected) + @($selection.cascades)) {
        $task = $candidate.task
        $logId = Get-VmrEvidenceProperty $task.log 'id'
        $optional = $candidate.kind -eq 'warningSample'
        $issueExcerpt = Get-VmrCompactDiagnosticExcerpt -MaxCharacters $State.limits.compactExcerptCharacters `
            -Lines @($task.issues | ForEach-Object {
                $message = [string](Get-VmrEvidenceProperty $_ 'message')
                if ($message) { '[{0}] {1}' -f (Get-VmrEvidenceProperty $_ 'type'), $message }
            })
        $logEntry = [ordered]@{
            buildId = $RootBuildId; task = $task.task; job = $task.job; jobId = $task.jobId; logId = $logId; path = $null
            recordId = $task.id; recordType = $task.recordType; result = $task.result
            evidenceKind = $candidate.kind
            available = $false; required = $candidate.required; selectionPriority = $candidate.priority
            selectionReason = $candidate.reason; error = $null; diagnosticExcerpts = @(); excerptsTruncated = $false
            diagnosticExcerpt = $issueExcerpt.text; diagnosticExcerptTruncated = $issueExcerpt.truncated
            diagnosticExcerptSource = if ($null -ne $issueExcerpt.text) { 'timelineIssues' } else { $null }
            organization = $Organization; project = $Project; actualBuildId = $id; scope = $Scope
            partialPath = $null
        }
        if ($Depth) { $logEntry['promotionBuildId'] = $id; $logEntry['parentBuildId'] = $ParentBuildId }
        if ($candidate.kind -eq 'cascadeSummary') {
            $logEntry['cascade'] = $candidate.cascade
            $logEntry['rawLogStatus'] = 'not_requested_optional'
            $cacheKey = "$Organization/$Project/$id/$logId"
            if ($State.logCache.ContainsKey($cacheKey)) {
                $retained = $State.logCache[$cacheKey].source
                $logEntry.available = $retained.available
                $logEntry.path = $retained.source.path
                $logEntry.partialPath = $retained.source.partialPath
                $logEntry.rawLogStatus = if ($retained.available) { 'available' } else { 'unavailable_optional' }
            }
            $State.logs.Add([pscustomobject]$logEntry)
            continue
        }
        if ($candidate.ContainsKey('sameFailureAs')) {
            # Not downloaded: this failure has a raw log in another build. The timeline message above is what this build keeps, and
            # the build and log that stand for it are named so the report can say so.
            $same = $candidate.sameFailureAs
            $logEntry.evidenceKind = 'sameFailureSummary'
            $logEntry.required = $false
            $logEntry.selectionReason = "Same failure as build $($same.buildId), task '$($same.task)', log $($same.logId): the raw log is not fetched again; the timeline message of this build is retained."
            $logEntry['rawLogStatus'] = 'not_requested_same_failure'
            $logEntry['sameFailureAs'] = [pscustomobject]@{ buildId = $same.buildId; logId = $same.logId; task = $same.task }
            $State.logs.Add([pscustomobject]$logEntry)
            continue
        }
        if ($candidate.ContainsKey('omission')) {
            $logEntry.error = $candidate.omission
            if ($candidate.required) {
                [void](Add-VmrEvidenceError $State $Scope "root-logs-$id" "Build $id task '$($task.task)': $($candidate.omission)")
            }
            $State.logs.Add([pscustomobject]$logEntry)
            continue
        }
        $key = "$Organization/$Project/$id/$logId"
        if (-not $State.logCache.ContainsKey($key)) {
            $source = Save-VmrEvidenceSource -State $State -Scope $Scope -Stage "root-log-$id-$logId" `
                -Name "$prefix-log-$logId" -Service AzureDevOps -Organization $Organization -Project $Project `
                -ApiPath "build/builds/$id/logs/$logId" -Text -Optional:$optional `
                -MaxResponseBytes $(if ($optional) { $State.limits.maxWarningLogBytes } else { 0 })
            $diagnostics = $null
            if ($source.available) {
                try {
                    $diagnostics = Get-VmrLogDiagnostics -Path $source.source.path `
                        -MaxLines $State.limits.excerptLines -MaxLineLength $State.limits.excerptLineLength
                }
                catch {
                    [void](Add-VmrEvidenceError -State $State -Scope $Scope -Stage "log-excerpts-$id-$logId" -Exception $_.Exception -Optional:$optional)
                }
            }
            $State.logCache[$key] = @{ source = $source; diagnostics = $diagnostics }
        }
        else {
            $prior = $State.logCache[$key]
            if (-not $prior.source.available) {
                [void](Add-VmrEvidenceError -State $State -Scope $Scope -Stage "cached-root-log-$id-$logId" -Optional:$optional -Exception (
                    New-VmrCollectorException -Message $prior.source.source.error `
                        -AuthenticationRequired $prior.source.source.authenticationRequired `
                        -SignInCommand $prior.source.source.signInCommand))
            }
            elseif ($null -eq $prior.diagnostics) {
                [void](Add-VmrEvidenceError $State $Scope "cached-log-excerpts-$id-$logId" 'Compact log diagnostics are unavailable.' -Optional:$optional)
            }
        }
        $cached = $State.logCache[$key]
        $logEntry.path = $cached.source.source.path
        $logEntry.available = $cached.source.available
        $logEntry.error = $cached.source.source.error
        $logEntry.partialPath = $cached.source.source.partialPath
        if ($optional) {
            $counter = if ($logEntry.available) { 'available' } else { 'unavailable' }
            $selection.warningSummary.logSampling[$counter]++
        }
        if ($null -ne $cached.diagnostics) {
            $logEntry.diagnosticExcerpts = $cached.diagnostics.excerpts
            $logEntry.excerptsTruncated = $cached.diagnostics.excerptsTruncated
            $logEntry['scannedLines'] = $cached.diagnostics.scannedLines
            if ($cached.diagnostics.matchingLines -gt 0 -or $null -eq $logEntry.diagnosticExcerpt) {
                $compact = Get-VmrCompactDiagnosticExcerpt -MaxCharacters $State.limits.compactExcerptCharacters `
                    -Lines @($cached.diagnostics.excerpts | ForEach-Object { 'L{0}: {1}' -f $_.line, $_.text })
                $logEntry.diagnosticExcerpt = $compact.text
                $logEntry.diagnosticExcerptTruncated = $compact.truncated -or $cached.diagnostics.excerptsTruncated
                $logEntry.diagnosticExcerptSource = if ($null -ne $compact.text) { 'log' } else { $null }
            }
        }
        $State.logs.Add([pscustomobject]$logEntry)
        if ($optional -or $task.task -notmatch '\bPublish Using Darc\b' -or $null -eq $cached.diagnostics) { continue }
        foreach ($promotionId in $cached.diagnostics.promotionBuildIds) {
            $promotionKey = "dnceng/internal/$promotionId"
            if ($PromotionState.seen.Contains($promotionKey)) { continue }
            [void]$PromotionState.seen.Add($promotionKey)
            if ($Depth -ge $State.limits.maxPromotionDepth -or $PromotionState.count -ge $State.limits.maxPromotionBuildsPerParent) {
                [void](Add-VmrEvidenceError $State $Scope "promotion-$RootBuildId-$promotionId" `
                    "Promotion $promotionId referenced by build $id was not followed because the explicit promotion depth/build limit was reached.")
                $State.promotions.Add(@{ buildId = $RootBuildId; parentBuildId = $id; promotionBuildId = $promotionId
                    rootScope = $Scope; timelineStatus = 'unavailable'; timelinePath = $null; tasks = @() })
                continue
            }
            $PromotionState.count++
            [void](Get-VmrBuildEvidence -State $State -Scope $Scope -Organization dnceng -Project internal `
                -Build ([pscustomobject]@{ id = $promotionId; result = 'unknown' }) -RootBuildId $RootBuildId `
                -ParentBuildId $id -Depth ($Depth + 1) -PromotionState $PromotionState)
        }
    }
    return [pscustomobject]$entry
}

function New-VmrUnknownCounts {
    return @{ completed = $null; succeeded = $null; failed = $null; canceled = $null; partiallySucceeded = $null }
}

function New-VmrPipelineEvidenceRecord {
    param([string] $Scope, [hashtable] $Limits)

    return [ordered]@{
        name = $Scope; status = 'unavailable'; inventoryPath = $null; activeInventoryPaths = @()
        requiredProblemBuildIds = @(); counts = (New-VmrUnknownCounts); error = $null; loginCommand = $null
        rawInventoryPaths = @(); partialInventoryPath = $null
        timelineCoverage = @{
            policy = if ($Scope -eq 'internal') { 'Every scoped completed build, including successes and warnings.' }
                else { "Every problem build; last $($Limits.successfulPublicTimelineSample) successful builds sampled for warnings." }
            requiredBuildIds = @(); availableBuildIds = @(); partialBuildIds = @(); unavailableBuildIds = @()
            noTimelineBuildIds = @()
        }
    }
}

function Invoke-VmrPipelineEvidence {
    param([hashtable] $State, [string] $Scope, [DateTimeOffset] $WindowStart, [DateTimeOffset] $WindowEnd)

    $organization = if ($Scope -eq 'internal') { 'dnceng' } else { 'dnceng-public' }
    $project = $Scope
    $pipeline = New-VmrPipelineEvidenceRecord -Scope $Scope -Limits $State.limits
    $definitions = Save-VmrEvidenceSource -State $State -Scope $Scope -Stage 'definitions' `
        -Name "$Scope-pipeline-definitions" -Service AzureDevOps -Organization $organization -Project $project `
        -ApiPath 'build/definitions?name=dotnet-unified-build'
    if (-not $definitions.available) { return $pipeline }
    $exact = @((Get-VmrEvidenceProperty $definitions.data 'value') | Where-Object {
        (Get-VmrEvidenceProperty $_ 'name') -eq 'dotnet-unified-build' -and
        [string](Get-VmrEvidenceProperty $_ 'id') -match '^[1-9]\d*$'
    } | Sort-Object id -Unique)
    if ($exact.Count -eq 0) {
        [void](Add-VmrEvidenceError $State $Scope 'definitions' 'No exact dotnet-unified-build definition with a valid ID was returned.')
        return $pipeline
    }
    $allBuilds = [Collections.Generic.List[object]]::new()
    $rawPaths = [Collections.Generic.List[string]]::new()
    $activePaths = [Collections.Generic.List[string]]::new()
    $inventoryComplete = $true
    $start = [Uri]::EscapeDataString($WindowStart.ToUniversalTime().ToString('o'))
    $end = [Uri]::EscapeDataString($WindowEnd.ToUniversalTime().ToString('o'))
    foreach ($definition in $exact) {
        $source = Save-VmrEvidenceSource -State $State -Scope $Scope -Stage 'completed-inventory' `
            -Name "$Scope-builds-$($definition.id)-completed" -Service AzureDevOps -Organization $organization -Project $project `
            -ApiPath "build/builds?definitions=$($definition.id)&statusFilter=completed&queryOrder=finishTimeAscending&minTime=$start&maxTime=$end&`$top=200"
        if ($source.available -and $null -ne $source.data.PSObject.Properties['value'] -and $source.data.value -is [array]) {
            $rawPaths.Add($source.source.path)
            foreach ($build in @(Get-VmrEvidenceProperty $source.data 'value')) { $allBuilds.Add($build) }
        }
        else {
            $inventoryComplete = $false
            if ($source.available) {
                [void](Add-VmrEvidenceError $State $Scope 'completed-inventory' 'Completed inventory response has no complete value array.')
            }
        }
        foreach ($status in @('inProgress', 'notStarted', 'postponed', 'cancelling')) {
            $active = Save-VmrEvidenceSource -State $State -Scope $Scope -Stage "active-inventory-$status" `
                -Name "$Scope-builds-$($definition.id)-$status" -Service AzureDevOps -Organization $organization -Project $project `
                -ApiPath "build/builds?definitions=$($definition.id)&statusFilter=$status&queryOrder=queueTimeAscending&`$top=200"
            if (-not $active.available) { continue }
            if ($null -eq $active.data.PSObject.Properties['value'] -or $active.data.value -isnot [array]) {
                [void](Add-VmrEvidenceError $State $Scope "active-inventory-$status" 'Active inventory response has no complete value array.')
                continue
            }
            $scopedActive = Get-VmrScopedBuilds -State $State -Builds @(Get-VmrEvidenceProperty $active.data 'value') `
                -Scope $Scope -Kind active -WindowStart $WindowStart -WindowEnd $WindowEnd
            $path = Join-Path $State.directory "$Scope-builds-$($definition.id)-$status-scoped.json"
            Write-VmrEvidenceJson -Path $path -Value @{
                status = if ($scopedActive.complete) { 'complete' } else { 'partial' }
                count = $scopedActive.value.Count; value = $scopedActive.value; excluded = $scopedActive.excluded
                rawInventoryPath = $active.source.path; observedAtUtc = $active.source.observedAtUtc
                observationCutoffUtc = $WindowEnd.ToUniversalTime().ToString('o')
                observationNote = 'Active status observed at observedAtUtc, not a reconstruction of active-at-cutoff. Queue times after cutoff are excluded.'
            }
            $activePaths.Add($path)
        }
    }
    $pipeline.activeInventoryPaths = $activePaths.ToArray()
    $pipeline.rawInventoryPaths = $rawPaths.ToArray()
    $scoped = Get-VmrScopedBuilds -State $State -Builds $allBuilds.ToArray() -Scope $Scope `
        -Kind completed -WindowStart $WindowStart -WindowEnd $WindowEnd
    $complete = $inventoryComplete -and $scoped.complete
    $inventoryPath = Join-Path $State.directory "$Scope-completed-builds.json"
    Write-VmrEvidenceJson -Path $inventoryPath -Value @{
        status = if ($complete) { 'complete' } else { 'partial' }; collectionComplete = $inventoryComplete
        classificationComplete = $scoped.complete; count = $scoped.value.Count; value = $scoped.value
        excluded = $scoped.excluded; rawInventoryPaths = $rawPaths.ToArray()
        windowStartUtc = $WindowStart.ToUniversalTime().ToString('o'); observationCutoffUtc = $WindowEnd.ToUniversalTime().ToString('o')
        windowSemantics = 'Inclusive [windowStartUtc, observationCutoffUtc], using finishTime only.'
    }
    if ($complete) {
        $pipeline.inventoryPath = $inventoryPath
        $pipeline.counts = @{
            completed = $scoped.value.Count
            succeeded = @($scoped.value | Where-Object result -eq 'succeeded').Count
            failed = @($scoped.value | Where-Object result -eq 'failed').Count
            canceled = @($scoped.value | Where-Object result -eq 'canceled').Count
            partiallySucceeded = @($scoped.value | Where-Object result -eq 'partiallySucceeded').Count
        }
    }
    else { $pipeline.partialInventoryPath = $inventoryPath }
    $pipeline.requiredProblemBuildIds = @($scoped.value | Where-Object result -in @('failed', 'canceled', 'partiallySucceeded') |
        ForEach-Object { [long]$_.id } | Sort-Object)
    $successSample = @($scoped.value | Where-Object result -eq 'succeeded' |
        Select-Object -Last $State.limits.successfulPublicTimelineSample | ForEach-Object id)
    $requiredBuilds = @($scoped.value | Where-Object {
        $Scope -eq 'internal' -or $_.result -ne 'succeeded' -or $_.id -in $successSample
    } | Sort-Object @{ Expression = { if ($_.result -eq 'succeeded') { 1 } else { 0 } } }, finishTime, id)
    $pipeline.timelineCoverage.requiredBuildIds = @($requiredBuilds | ForEach-Object { [long]$_.id })
    # Warm the root timelines concurrently; the serial loop below consumes them unchanged.
    [void](Invoke-VmrEvidencePrefetch -Session $State.session -Organization $organization -Project $project `
        -ApiPaths @($requiredBuilds |
            Select-Object -First $State.limits.maxTimelineBuildsPerPipeline |
            Where-Object { -not $State.timelineCache.ContainsKey("$organization/$project/$($_.id)") } |
            ForEach-Object { "build/builds/$($_.id)/timeline" }))
    $timelineCount = 0
    foreach ($build in $requiredBuilds) {
        if ($timelineCount -ge $State.limits.maxTimelineBuildsPerPipeline) {
            $entry = [pscustomobject]@{
                organization = $organization; project = $project; buildId = $build.id
                result = $build.result; sourceBranch = $build.sourceBranch; finishTime = $build.finishTime
                timelineStatus = 'unavailable'; timelinePath = $null; tasks = @(); warningSample = $false
            }
            if ($timelineCount -eq $State.limits.maxTimelineBuildsPerPipeline) {
                [void](Add-VmrEvidenceError $State $Scope 'timeline-coverage' 'Explicit per-pipeline timeline build limit reached; required timelines are missing.')
            }
        }
        else {
            try { $entry = Get-VmrBuildEvidence -State $State -Scope $Scope -Organization $organization -Project $project -Build $build }
            catch {
                # One build that cannot be read is a gap for that build (and an error that says so), not a reason to lose the rest.
                [void](Add-VmrEvidenceError -State $State -Scope $Scope -Stage "build-$($build.id)" -Exception $_.Exception)
                $entry = [pscustomobject]@{
                    organization = $organization; project = $project; buildId = $build.id
                    result = $build.result; sourceBranch = $build.sourceBranch; finishTime = $build.finishTime
                    timelineStatus = 'unavailable'; timelinePath = $null; tasks = @(); warningSample = $false
                }
            }
        }
        $timelineCount++
        $State.builds.Add($entry)
        $field = switch ($entry.timelineStatus) {
            'available' { 'availableBuildIds' }; 'partial' { 'partialBuildIds' }
            'none' { 'noTimelineBuildIds' }; default { 'unavailableBuildIds' }
        }
        $pipeline.timelineCoverage[$field] += [long]$build.id
    }
    return $pipeline
}

function Save-VmrCheckoutDocument {
    <#
    .SYNOPSIS
    Keeps a copy of a static document from the checkout this skill lives in, with where it came from.
    .DESCRIPTION
    The Guide and the ownership policy change when the user pulls, not while a run is in progress, and the skill already reads
    its routing from them on disk. Fetching them again from GitHub main would give the run two sources of truth, so the
    evidence holds a copy of the checkout's own file: its path, SHA-256, last write time and (when git can say) the checkout's
    commit. Returns the text, or $null with an evidence error when the file is not there.
    #>
    param([hashtable] $State, [string] $Name, [string] $RelativePath, [string] $CheckoutRoot)

    if ($Name -notmatch '^[a-zA-Z0-9_-]+$') { throw 'Invalid collector artifact name.' }
    $sourcePath = Join-Path $CheckoutRoot $RelativePath
    $copyPath = Join-Path $State.directory "$Name.txt"
    $record = [pscustomobject]@{
        name = $Name; path = $copyPath; service = 'Checkout'; apiPath = ($RelativePath -replace '\\', '/')
        organization = $null; project = $null
        requestedAtUtc = [DateTimeOffset]::UtcNow.ToString('o'); observedAt = $null; observedAtUtc = $null
        status = 'unavailable'; error = $null; authenticationRequired = $false; signInCommand = $null
        rawPagesPath = $null; partialPath = $null; required = $true
        checkoutPath = $sourcePath; sha256 = $null; lastWriteUtc = $null; checkoutCommit = $null
    }
    $content = $null
    try {
        if (-not [IO.File]::Exists($sourcePath)) {
            throw (New-VmrCollectorException -Message "$RelativePath is not in this checkout ($CheckoutRoot). The skill reads it from disk and never fetches it; run it from a dotnet/dotnet checkout.")
        }
        $bytes = [IO.File]::ReadAllBytes($sourcePath)
        $content = [Text.UTF8Encoding]::new($false).GetString($bytes)
        [IO.File]::WriteAllText($copyPath, $content, [Text.UTF8Encoding]::new($false))
        $sha = [Security.Cryptography.SHA256]::Create()
        try { $record.sha256 = ([BitConverter]::ToString($sha.ComputeHash($bytes)) -replace '-', '').ToLowerInvariant() } finally { $sha.Dispose() }
        $record.lastWriteUtc = ([IO.File]::GetLastWriteTimeUtc($sourcePath)).ToString('o')
        try {
            $commit = (& git -C $CheckoutRoot rev-parse HEAD 2>$null)
            if ($LASTEXITCODE -eq 0 -and "$commit" -match '^[0-9a-f]{40}$') { $record.checkoutCommit = "$commit" }
        }
        catch { Write-Verbose "No git commit recorded for the checkout: $($_.Exception.Message)" }
        $record.status = 'available'
    }
    catch {
        $errorInfo = Add-VmrEvidenceError -State $State -Scope context -Stage "$Name-checkout" -Exception $_.Exception
        $record.error = $errorInfo.message
    }
    finally {
        $record.observedAtUtc = [DateTimeOffset]::UtcNow.ToString('o')
        $record.observedAt = $record.observedAtUtc
        $State.sources.Add($record)
    }
    return $content
}

function Save-VmrGuideAndOwnership {
    param([hashtable] $State, [switch] $CoreOnly, [string] $CheckoutRoot = (Get-VmrQBPackageRoot))

    # The Guide and the ownership policy are static documents of this checkout: copied with provenance, never fetched.
    # Everything that really is live (branches, and with a full collection the release wiki and each team) stays live and
    # carries its own observation time.
    foreach ($item in @(
        @{ name = 'guide'; path = 'docs\VMR-QB-Guide.md' },
        @{ name = 'ownership'; path = '.github\policies\assign_ownership.yml' }
    )) {
        $content = Save-VmrCheckoutDocument -State $State -Name $item.name -RelativePath $item.path -CheckoutRoot $CheckoutRoot
        if ($null -eq $content -or $CoreOnly) { continue }
        if ($item.name -eq 'guide') {
            $wiki = [regex]::Match($content, 'https://dev\.azure\.com/devdiv/DevDiv/_wiki/wikis/DevDiv\.wiki/(?<id>\d+)/')
            if ($wiki.Success) {
                [void](Save-VmrEvidenceSource -State $State -Scope context -Stage release-schedule -Name release-schedule `
                    -Service AzureDevOps -Organization devdiv -Project DevDiv `
                    -ApiPath "wiki/wikis/DevDiv.wiki/pages/$($wiki.Groups['id'].Value)?includeContent=true")
            }
            else {
                [void](Add-VmrEvidenceError $State context release-schedule 'The current guide has no recognized DevDiv wiki schedule URL.')
                $State.sources.Add(@{ name = 'release-schedule'; status = 'unavailable'; error = 'No recognized release schedule URL was found in the current guide.' })
            }
        }
        else {
            $teams = @([regex]::Matches($content, '(?m)^\s*teamReviewer:\s*(?<slug>[a-zA-Z0-9-]+)\s*$') |
                ForEach-Object { $_.Groups['slug'].Value }) + @('prodconsvcs')
            foreach ($team in @($teams | Sort-Object -Unique)) {
                [void](Save-VmrEvidenceSource -State $State -Scope context -Stage "team-$team" `
                    -Name "team-$team" -Service GitHub -ApiPath "orgs/dotnet/teams/$team")
            }
        }
    }
    [void](Save-VmrEvidenceSource -State $State -Scope context -Stage branches -Name branches `
        -Service GitHub -ApiPath 'repos/dotnet/dotnet/branches?per_page=100')
}

function Write-VmrDailyEvidenceSnapshot {
    param([hashtable] $Index, [hashtable] $Failures, [hashtable] $Logs, [string] $Directory, [string] $LegacyDirectory)

    Write-VmrEvidenceJson -Path $Index.failureTasksPath -Value $Failures -Replace
    Write-VmrEvidenceJson -Path $Index.rootLogsPath -Value $Logs -Replace
    $path = Join-Path $Directory 'sources.json'
    Write-VmrEvidenceJson -Path $path -Value $Index -Replace
    if ($LegacyDirectory) {
        Write-VmrEvidenceJson -Path (Join-Path $LegacyDirectory 'failure-tasks.json') -Value $Failures -Replace
        Write-VmrEvidenceJson -Path (Join-Path $LegacyDirectory 'root-logs.json') -Value $Logs -Replace
        $path = Join-Path $LegacyDirectory 'sources.json'
        Write-VmrEvidenceJson -Path $path -Value $Index -Replace
    }
    return $path
}

function Invoke-VmrDailyEvidence {
    <#
    .SYNOPSIS
    Writes schema-v2 sources.json plus legacy sources[], failure-tasks.json and root-logs.json.
    .DESCRIPTION
    OutputDirectory must be absolute and new/empty. Legacy callers receive artifacts\<date>\sources.json,
    pointing exclusively into a fresh evidence subdirectory. Missing/incomplete inventories have null counts
    and inventoryPath; partialInventoryPath retains provisional evidence. No authentication fallback occurs.
    The Guide and the ownership policy are copied from the checkout (CheckoutRoot, default: the checkout this skill lives in), with
    path, SHA-256, last write time and commit recorded; they are never fetched, because they change when the user pulls. Branches,
    builds, issues and the rest stay live and carry their observation times. CoreOnly omits the release wiki and individual team lookups.
    Root-log entries retain timeline recordId/recordType/result and organization/project. diagnosticExcerpt
    is a bounded string; diagnosticExcerptSource distinguishes log text from timelineIssues fallback.
    Full primary timelines include all warning records. Only structural/problem detail references are
    required; completed nonblocking Task command details are explicitly deferred. Warning log samples
    have independent count/byte limits and optionalErrors; they do not consume the failure-log budget.
    Proven same-job missing-input cascades retain cascade/root record linkage and timeline diagnostics
    with optional rawLogStatus; ambiguous or standalone signing/scanning errors remain required.
    The per-run sources.json is the evidence pointer; no current-run.json lookup is performed.
    #>
    param(
        [string] $ReportDate, [DateTimeOffset] $WindowStart, [DateTimeOffset] $WindowEnd,
        [string] $OutputDirectory, [string] $LegacyDirectory, [switch] $CoreOnly,
        [hashtable] $Session, [hashtable] $Limits = @{},
        [string] $CheckoutRoot = (Get-VmrQBPackageRoot)
    )

    if ($WindowStart -ge $WindowEnd) { throw 'The monitoring window start must precede its end.' }
    $directory = Initialize-VmrEvidenceDirectory -OutputDirectory $OutputDirectory -LegacyDirectory $LegacyDirectory
    if ($null -eq $Session) { $Session = New-VmrReadOnlySession }
    $effectiveLimits = @{
        maxRootLogsPerBuild = 32; maxPromotionBuildsPerParent = 4; maxPromotionDepth = 2
        maxWarningLogsPerBuild = 1; maxWarningLogBytes = 1MB
        maxTimelinePartsPerBuild = 16; maxTimelineBuildsPerPipeline = 500; successfulPublicTimelineSample = 6
        excerptLines = 24; excerptLineLength = 512; compactExcerptCharacters = 1536
    }
    foreach ($name in $Limits.Keys) {
        $minimum = if ($name -eq 'maxWarningLogsPerBuild') { 0 } else { 1 }
        if (-not $effectiveLimits.ContainsKey($name) -or $Limits[$name] -isnot [int] -or $Limits[$name] -lt $minimum) {
            throw "Invalid collector limit: $name."
        }
        $effectiveLimits[$name] = $Limits[$name]
    }
    $state = @{
        directory = $directory; session = $Session; limits = $effectiveLimits
        sources = [Collections.Generic.List[object]]::new(); errors = [Collections.Generic.List[object]]::new()
        optionalErrors = [Collections.Generic.List[object]]::new()
        builds = [Collections.Generic.List[object]]::new(); logs = [Collections.Generic.List[object]]::new()
        promotions = [Collections.Generic.List[object]]::new(); prCache = @{}; timelineCache = @{}; logCache = @{}
    }
    $failureIndex = @{
        windowStart = $WindowStart.ToUniversalTime().ToString('o'); windowEnd = $WindowEnd.ToUniversalTime().ToString('o')
        builds = @(); promotions = @()
    }
    $logIndex = @{
        logs = @(); limits = $effectiveLimits
        sampling = "Failures are grouped first, by task and timeline message with build ids, versions, GUIDs and hashes taken out, within one pipeline and branch: one raw log is fetched for each distinct failure (from the first build that has it), and every other build with the same failure keeps its timeline message and names the build whose log stands for it (evidenceKind sameFailureSummary, not required). At most $($effectiveLimits.maxRootLogsPerBuild) distinct failure root logs per build; same-failure records take no slot. Distinct failed Task names receive representatives before duplicate legs. Proven missing-input On Failure publish/SBOM generation cascades after a failed/canceled Build in the same Job retain timeline diagnostics and explicit root linkage; separate raw logs are optional and consume no required slots. Ambiguous and standalone signing/scanning errors remain required. Nonblocking warning records/counters remain in full timelines and warningSummary; at most $($effectiveLimits.maxWarningLogsPerBuild) optional warning logs per build, each limited to $($effectiveLimits.maxWarningLogBytes) bytes, use a separate sampling budget. Missing required initiating logs/timelines remain partial. Promotion children are never counted as parent pipeline failures and are never grouped."
    }
    $pendingMessage = 'Collection has not finished or was interrupted. No current evidence can be claimed complete.'
    $pendingPipelines = @('public', 'internal' | ForEach-Object {
        $record = New-VmrPipelineEvidenceRecord -Scope $_ -Limits $effectiveLimits
        $record.error = $pendingMessage
        $record
    })
    $started = [DateTimeOffset]::UtcNow.ToString('o')
    $index = @{
        schemaVersion = 2; reportDate = $ReportDate
        windowStartUtc = $WindowStart.ToUniversalTime().ToString('o'); observationCutoffUtc = $WindowEnd.ToUniversalTime().ToString('o')
        collectedAtUtc = $started; collectedAt = $started; status = 'partial'
        failureTasksPath = (Join-Path $directory 'failure-tasks.json'); rootLogsPath = (Join-Path $directory 'root-logs.json')
        pipelines = $pendingPipelines
        requiredErrors = @(@{ scope = 'collector'; stage = 'collection'; message = $pendingMessage
            authenticationRequired = $false; signInCommand = $null })
        sources = @(); optionalErrors = @()
        limits = $effectiveLimits + @{
            maxPagesPerRequest = $Session.maxPages; maxRequests = $Session.maxRequests
            requestTimeoutSeconds = $Session.timeoutSeconds; maxResponseBytes = $Session.maxResponseBytes
        }
        requestsUsed = 0; coreOnly = $CoreOnly.IsPresent
    }
    $legacyAlias = if (-not $OutputDirectory) { $LegacyDirectory } else { $null }
    try {
        # Publish an explicitly incomplete snapshot before any I/O can fail, including legacy date aliases.
        [void](Write-VmrDailyEvidenceSnapshot -Index $index -Failures $failureIndex -Logs $logIndex `
            -Directory $directory -LegacyDirectory $legacyAlias)
        Save-VmrGuideAndOwnership -State $state -CoreOnly:$CoreOnly -CheckoutRoot $CheckoutRoot
        $pipelines = @{}
        # Internal evidence has first claim on the finite request budget.
        foreach ($scope in @('internal', 'public')) {
            # The two pipelines are collected independently: one that fails unexpectedly is recorded as an error for that pipeline
            # and the other keeps what it collected.
            try { $pipelines[$scope] = Invoke-VmrPipelineEvidence -State $state -Scope $scope -WindowStart $WindowStart -WindowEnd $WindowEnd }
            catch {
                [void](Add-VmrEvidenceError -State $state -Scope $scope -Stage 'pipeline-collection' -Exception $_.Exception)
                $pipelines[$scope] = New-VmrPipelineEvidenceRecord -Scope $scope -Limits $effectiveLimits
            }
        }
        foreach ($scope in @('public', 'internal')) {
            $pipeline = $pipelines[$scope]
            $errors = @($state.errors | Where-Object scope -eq $scope)
            $authErrors = @($errors | Where-Object authenticationRequired)
            $pipeline.status = if ($authErrors.Count) { 'authentication_required' }
                elseif ($errors.Count -and ($pipeline.inventoryPath -or $pipeline.partialInventoryPath)) { 'partial' }
                elseif ($errors.Count) { 'unavailable' } else { 'available' }
            if ($errors.Count) { $pipeline.error = ($errors | ForEach-Object message) -join ' ' }
            if ($authErrors.Count) { $pipeline.loginCommand = $authErrors[0].signInCommand }
        }
        $failureIndex.builds = $state.builds.ToArray()
        $failureIndex.promotions = $state.promotions.ToArray()
        $logIndex.logs = $state.logs.ToArray()
        $collected = [DateTimeOffset]::UtcNow.ToString('o')
        $index.collectedAtUtc = $collected
        $index.collectedAt = $collected
        $index.status = if ($state.errors.Count) { 'partial' } else { 'complete' }
        $index.pipelines = @($pipelines.public, $pipelines.internal)
        $index.requiredErrors = $state.errors.ToArray()
        $index.optionalErrors = $state.optionalErrors.ToArray()
        $index.sources = $state.sources.ToArray()
        $index.requestsUsed = $Session.requests
        return Write-VmrDailyEvidenceSnapshot -Index $index -Failures $failureIndex -Logs $logIndex `
            -Directory $directory -LegacyDirectory $legacyAlias
    }
    finally {
        $Session.tokens.Clear()
        $Session.authenticationFailures.Clear()
        Clear-VmrEvidencePrefetch
    }
}
