Set-StrictMode -Version Latest

# daily/evidence.ps1 - Stage one of the daily round: the read-only Azure DevOps / GitHub transport and the evidence collector.
# Dot-sourced by Invoke-Daily.ps1, preflight.ps1 and the tests; it defines functions only and runs nothing.
#
# Kept true throughout: GET only and no redirect is followed; a credential is never written, printed or put in an error; and what
# could not be read is never "healthy" - an error makes the pipeline partial, unavailable or authentication_required, and its counts
# stay null unless the whole completed inventory was read.

$script:VmrEvidenceHttpClient = $null

# ---------------------------------------------------------------------------------------------
# Helpers and errors
# ---------------------------------------------------------------------------------------------
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
        [string] $Kind = 'collection',
        [bool] $Transient = $false
    )

    $exception = [InvalidOperationException]::new($Message)
    $exception.Data['VmrCollectorError'] = $true
    $exception.Data['AuthenticationRequired'] = $AuthenticationRequired
    $exception.Data['SignInCommand'] = $SignInCommand
    $exception.Data['Kind'] = $Kind
    $exception.Data['Transient'] = $Transient
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
    param([string] $Path, [AllowNull()][object] $Value)

    [IO.File]::WriteAllText($Path, (ConvertTo-Json -InputObject $Value -Depth 100), [Text.UTF8Encoding]::new($false))
}

# ---------------------------------------------------------------------------------------------
# Native tools, token, request addresses
# ---------------------------------------------------------------------------------------------
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

    $decoded = [Uri]::UnescapeDataString($ApiPath)
    if ($decoded -match '[\\\r\n#]' -or $decoded.Contains('..')) {
        throw (New-VmrCollectorException -Message 'ApiPath must be relative and contain no traversal, fragments, or newlines.')
    }
    if ($Service -eq 'GitHub') {
        if ($decoded -notmatch '^repos/dotnet/dotnet/pulls/[1-9]\d*$') {
            throw (New-VmrCollectorException -Message 'GitHub GET is limited to dotnet/dotnet pull request metadata.')
        }
        return
    }
    $projects = @{ 'dnceng-public' = 'public'; dnceng = 'internal' }
    if (-not $projects.ContainsKey($Organization) -or $Project -ne $projects[$Organization]) {
        throw (New-VmrCollectorException -Message 'The Azure DevOps organization/project pair is not allowlisted.')
    }
    if ($decoded -notmatch '^build/(builds|definitions)(/|\?|$)' -or $decoded -match '(^|[?&])continuationToken=') {
        throw (New-VmrCollectorException -Message 'Only build and definition GET resources are supported; pagination is automatic.')
    }
    if ($Text -and $decoded -notmatch '^build/builds/[1-9]\d*/logs/[1-9]\d*(?:\?|$)') {
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

# ---------------------------------------------------------------------------------------------
# HTTP transport and session
# ---------------------------------------------------------------------------------------------
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

function Get-VmrEvidenceHttpClient {
    if ($null -eq $script:VmrEvidenceHttpClient) {
        $handler = [Net.Http.SocketsHttpHandler]::new()
        $handler.AllowAutoRedirect = $false
        $handler.AutomaticDecompression = [Net.DecompressionMethods]::GZip -bor [Net.DecompressionMethods]::Deflate
        $client = [Net.Http.HttpClient]::new($handler, $true)
        $client.Timeout = [Threading.Timeout]::InfiniteTimeSpan
        $script:VmrEvidenceHttpClient = $client
    }
    return $script:VmrEvidenceHttpClient
}

function Invoke-VmrEvidenceHttpGet {
    # One GET. A body is written to OutputPath, or returned in content; an HTML page is always returned in content so that a
    # sign-in page can be recognized.
    param([uri] $Uri, [hashtable] $Headers, [string] $OutputPath, [int] $TimeoutSeconds = 90, [long] $MaxBytes = 64MB)

    if ($Uri.Scheme -ne 'https' -or $Uri.Host -ne 'dev.azure.com' -or $Uri.Port -ne 443 -or
        $Uri.UserInfo -or $Uri.Fragment -or
        $Uri.AbsolutePath -notmatch '^/(dnceng-public/public|dnceng/internal)/_apis/') {
        throw (New-VmrCollectorException -Message 'HTTP GET destination is not an allowlisted HTTPS Azure DevOps API.')
    }
    $request = [Net.Http.HttpRequestMessage]::new([Net.Http.HttpMethod]::Get, $Uri)
    $cancellation = [Threading.CancellationTokenSource]::new([TimeSpan]::FromSeconds($TimeoutSeconds))
    $response = $null
    $body = $null
    $output = $null
    try {
        foreach ($name in $Headers.Keys) { [void]$request.Headers.TryAddWithoutValidation($name, [string]$Headers[$name]) }
        $response = (Get-VmrEvidenceHttpClient).SendAsync($request, [Net.Http.HttpCompletionOption]::ResponseHeadersRead,
            $cancellation.Token).GetAwaiter().GetResult()
        $result = @{ statusCode = [int]$response.StatusCode; headers = @{}; contentType = [string]$response.Content.Headers.ContentType; content = $null }
        foreach ($header in $response.Headers) { $result.headers[$header.Key] = $header.Value -join ',' }
        if (-not $response.IsSuccessStatusCode) { return $result }
        $body = $response.Content.ReadAsStreamAsync($cancellation.Token).GetAwaiter().GetResult()
        $toFile = $OutputPath -and $result.contentType -notmatch 'text/html'
        $output = if ($toFile) {
            [IO.File]::Open($OutputPath, [IO.FileMode]::Create, [IO.FileAccess]::Write, [IO.FileShare]::None)
        } else { [IO.MemoryStream]::new() }
        Copy-VmrEvidenceStream -InputStream $body -OutputStream $output -MaxBytes $MaxBytes -CancellationToken $cancellation.Token
        if (-not $toFile) { $result.content = [Text.Encoding]::UTF8.GetString($output.ToArray()) }
        return $result
    }
    catch {
        if ($_.Exception.Data.Contains('VmrCollectorError')) { throw }
        throw (New-VmrCollectorException -Kind 'transport' -Transient $true -Message 'HTTPS GET failed or timed out; no response body or request credentials were included in this error.')
    }
    finally {
        if ($null -ne $output) { $output.Dispose() }
        if ($null -ne $body) { $body.Dispose() }
        if ($null -ne $response) { $response.Dispose() }
        $request.Dispose()
        $cancellation.Dispose()
    }
}

function New-VmrReadOnlySession {
    # The three transports can be replaced, so that the tests run without a network. -MaxRequests is the one ceiling on the requests
    # of a session; the page limit and the response size limit are fixed.
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
                # Only the fact that it is worth another try is kept, never the CLI's text.
                $transient = (-not $auth) -and ([string]$capture.stderr -match '(?i)\bHTTP (429|5\d\d)\b|timed? ?out|connection (reset|refused|closed)|EOF|TLS|temporar|rate limit')
                throw (New-VmrCollectorException -AuthenticationRequired $auth -Kind 'github' -Transient $transient `
                    -SignInCommand $(if ($auth) { 'gh auth login --hostname github.com' } else { $null }) `
                    -Message "GitHub read-only GET failed (exit $($capture.exitCode)); CLI diagnostic output was not retained.")
            }
            return $capture.stdout
        },
        [ValidateRange(1, 100000)][int] $MaxRequests = 1600,
        [ValidateRange(1, 600)][int] $TimeoutSeconds = 90
    )

    return @{
        tokenProvider = $TokenProvider; httpGet = $HttpGet; githubGet = $GitHubGet
        tokens = @{}; authenticationFailures = @{}
        maxRequests = $MaxRequests; requests = 0; timeoutSeconds = $TimeoutSeconds
        maxPages = 64; maxResponseBytes = 64MB
    }
}

function Invoke-VmrEvidenceAttempt {
    # One GET through the session, tried again twice (after 2 s, then 4 s) when it failed for a reason that may pass: a timeout or
    # dropped connection, HTTP 429 or a 5xx, a transient GitHub error. Anything else is returned or thrown at once.
    param([hashtable] $Session, [string] $Service, [object] $Target, [hashtable] $Headers = @{}, [AllowNull()][string] $DownloadPath = $null)

    for ($attempt = 1; ; $attempt++) {
        if ($Session.requests -ge $Session.maxRequests) {
            throw (New-VmrCollectorException -Kind 'limit' -Message "The explicit $($Session.maxRequests) request limit was exhausted; required evidence remains incomplete.")
        }
        $Session.requests++
        $failure = $null
        $response = $null
        try {
            $response = if ($Service -eq 'GitHub') { & $Session.githubGet $Target $Session.timeoutSeconds }
                else { & $Session.httpGet $Target $Headers $DownloadPath $Session.timeoutSeconds $Session.maxResponseBytes }
        }
        catch { $failure = $_.Exception }
        $transient = if ($failure) { [bool]$failure.Data['Transient'] }
            else { $Service -eq 'AzureDevOps' -and [int](Get-VmrEvidenceProperty $response 'statusCode' 0) -in @(429, 500, 502, 503, 504) }
        if (-not $transient -or $attempt -ge 3) {
            if ($failure) { throw $failure }
            return $response
        }
        # Drop what a cut-off download left behind before the next attempt.
        if ($DownloadPath -and [IO.File]::Exists($DownloadPath)) { [IO.File]::Delete($DownloadPath) }
        Start-Sleep -Seconds ([int][Math]::Pow(2, $attempt))
    }
}

function Get-VmrAzureDevOpsResource {
    # One Azure DevOps resource. A list is followed page by page into one { count, value } object; any other object is returned as
    # it is. A log (-Text) is streamed to a temporary file and only moved to OutputPath when it arrived whole.
    param([hashtable] $Session, [string] $Organization, [string] $Project, [string] $ApiPath, [switch] $Text, [string] $OutputPath)

    $scope = "$Organization/$Project"
    $headers = @{ Accept = if ($Text) { 'text/plain' } else { 'application/json' } }
    try {
        if ($Organization -ne 'dnceng-public') {
            if (-not $Session.tokens.ContainsKey($scope)) {
                try { $Session.tokens[$scope] = & $Session.tokenProvider $Session.timeoutSeconds }
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
        for ($pageNumber = 1; $pageNumber -le $Session.maxPages; $pageNumber++) {
            $download = if ($Text -and $OutputPath) { "$OutputPath.download" } else { $null }
            try {
                $response = Invoke-VmrEvidenceAttempt -Session $Session -Service AzureDevOps -Target $uri -Headers $headers -DownloadPath $download
                $Session['lastHttpStatusCode'] = [int]$response.statusCode
                Assert-VmrEvidenceHttpResponse -Response $response -Scope $scope
                if ($Text) {
                    if (-not $OutputPath) { return [string]$response.content }
                    [IO.File]::Move($download, $OutputPath)
                    return @{ path = $OutputPath; length = ([IO.FileInfo]$OutputPath).Length }
                }
            }
            finally { if ($download -and [IO.File]::Exists($download)) { [IO.File]::Delete($download) } }
            $continuation = [string]((Get-VmrEvidenceProperty $response.headers 'x-ms-continuationtoken') -join '')
            # HTTP 204 or an empty body is a definitive "this resource has no content" (a build rejected before any job ran has no
            # timeline). That is collected evidence, not a malformed response.
            if ([int]$response.statusCode -eq 204 -or [string]::IsNullOrWhiteSpace([string]$response.content)) {
                if ($pageNumber -ne 1) {
                    throw (New-VmrCollectorException -Message 'Azure DevOps returned an empty page mid-pagination; refusing an incomplete success.')
                }
                return [pscustomobject]@{ noContent = $true; httpStatusCode = [int]$response.statusCode }
            }
            try { $page = ConvertFrom-Json -InputObject $response.content -Depth 100 -NoEnumerate }
            catch { throw (New-VmrCollectorException -Message 'Azure DevOps returned invalid JSON; evidence is incomplete.') }
            if ($null -eq $page.PSObject.Properties['value']) {
                if ($pageNumber -ne 1 -or $continuation) {
                    throw (New-VmrCollectorException -Message 'Azure DevOps changed pagination shape; refusing an incomplete success.')
                }
                if ($OutputPath) { [IO.File]::WriteAllText($OutputPath, [string]$response.content, [Text.UTF8Encoding]::new($false)) }
                return $page
            }
            if ($page.value -isnot [array]) {
                throw (New-VmrCollectorException -Message 'Azure DevOps list response has no value array.')
            }
            foreach ($item in $page.value) { $values.Add($item) }
            if (-not $continuation) {
                $list = [pscustomobject]@{ count = $values.Count; value = $values.ToArray() }
                if ($OutputPath) { Write-VmrEvidenceJson -Path $OutputPath -Value $list }
                return $list
            }
            $separator = if ($baseUri.Contains('?')) { '&' } else { '?' }
            $uri = "${baseUri}${separator}continuationToken=$([Uri]::EscapeDataString($continuation))"
        }
        throw (New-VmrCollectorException -Kind 'limit' -Message "Pagination exceeded the explicit $($Session.maxPages) page limit; no complete inventory was produced.")
    }
    finally {
        $headers.Clear()
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
        [string] $OutputPath
    )

    Assert-VmrReadOnlyApiPath -Service $Service -ApiPath $ApiPath -Organization $Organization -Project $Project -Text:$Text
    if ($OutputPath -and (-not [IO.Path]::IsPathFullyQualified($OutputPath) -or [IO.File]::Exists($OutputPath))) {
        throw (New-VmrCollectorException -Message 'OutputPath must be an absolute, unused file path.')
    }
    if ($Service -eq 'GitHub') {
        $raw = Invoke-VmrEvidenceAttempt -Session $Session -Service GitHub -Target $ApiPath
        try { $data = ConvertFrom-Json -InputObject $raw -Depth 100 -NoEnumerate }
        catch { throw (New-VmrCollectorException -Message 'GitHub returned invalid JSON; no evidence was accepted.') }
        if ($OutputPath) { Write-VmrEvidenceJson -Path $OutputPath -Value $data }
        return ,$data
    }

    $scope = "$Organization/$Project"
    # A sign-in failure that is real is final for the run: every later request for the scope gets the same answer at once.
    if ($Session.authenticationFailures.ContainsKey($scope)) { throw $Session.authenticationFailures[$scope] }
    for ($renewed = $false; ; $renewed = $true) {
        try {
            return Get-VmrAzureDevOpsResource -Session $Session -Organization $Organization -Project $Project `
                -ApiPath $ApiPath -Text:$Text -OutputPath $OutputPath
        }
        catch {
            $authentication = [bool]$_.Exception.Data['AuthenticationRequired']
            # A 401 or a sign-in page may only mean that the token this session held has expired: drop it and try once more with a
            # fresh one. A 403 (permission) or a token that could not be acquired at all is not helped by a new token.
            if ($authentication -and -not $renewed -and $Organization -ne 'dnceng-public' -and
                [string]$_.Exception.Data['Kind'] -eq 'authentication' -and $_.Exception.Message -match 'HTTP 401|sign-in redirect') {
                [void]$Session.tokens.Remove($scope)
                continue
            }
            if ($authentication) {
                $Session.authenticationFailures[$scope] = $_.Exception
                [void]$Session.tokens.Remove($scope)
            }
            throw
        }
    }
}

# ---------------------------------------------------------------------------------------------
# Collector
# ---------------------------------------------------------------------------------------------
function Add-VmrEvidenceError {
    param([hashtable] $State, [string] $Scope, [string] $Stage, [string] $Message, [Exception] $Exception)

    if ($null -eq $Exception) { $Exception = New-VmrCollectorException -Message $Message }
    $errorInfo = Get-VmrCollectorError -Exception $Exception -Scope $Scope -Stage $Stage
    $State.errors.Add($errorInfo)
    return $errorInfo
}

function Save-VmrEvidenceSource {
    # Reads one resource into <directory>\<Name>.json (or .log with -Text) and records it in sources.json. A failure is an error of
    # the scope, never an exception.
    param(
        [hashtable] $State, [string] $Scope, [string] $Stage, [string] $Name,
        [string] $Service, [string] $ApiPath,
        [string] $Organization = 'dnceng-public', [string] $Project = 'public', [switch] $Text
    )

    if ($Name -notmatch '^[a-zA-Z0-9_-]+$') { throw 'Invalid collector artifact name.' }
    $path = Join-Path $State.directory ("$Name." + $(if ($Text) { 'log' } else { 'json' }))
    $source = [pscustomobject]@{
        name = $Name; path = $path; service = $Service; apiPath = $ApiPath
        organization = $Organization; project = $Project
        observedAtUtc = $null; status = 'unavailable'; error = $null; authenticationRequired = $false; signInCommand = $null
    }
    try {
        $data = Invoke-VmrReadOnlyRequest -Session $State.session -Service $Service -ApiPath $ApiPath `
            -Organization $Organization -Project $Project -Text:$Text -OutputPath $path
        if ($null -ne $data -and $null -ne $data.PSObject.Properties['noContent']) {
            $source.status = 'no-content'
            return [pscustomobject]@{ available = $false; noContent = $true; data = $null; source = $source }
        }
        $source.status = 'available'
        return [pscustomobject]@{ available = $true; noContent = $false; data = $data; source = $source }
    }
    catch {
        $errorInfo = Add-VmrEvidenceError -State $State -Scope $Scope -Stage $Stage -Exception $_.Exception
        $source.error = $errorInfo.message
        $source.authenticationRequired = $errorInfo.authenticationRequired
        $source.signInCommand = $errorInfo.signInCommand
        return [pscustomobject]@{ available = $false; noContent = $false; data = $null; source = $source }
    }
    finally {
        $source.observedAtUtc = [DateTimeOffset]::UtcNow.ToString('o')
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
    # Which builds are in scope: rolling builds of main and release branches (internal: also internal/release), and public pull
    # request builds whose target branch is one of those. Everything else is an experiment and is left out.
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
    if ($State.prCache.ContainsKey($number)) { return $State.prCache[$number] }
    $metadata = Save-VmrEvidenceSource -State $State -Scope $Scope -Stage 'pr-classification' `
        -Name "public-pr-dotnet-dotnet-$number" -Service GitHub -ApiPath "repos/dotnet/dotnet/pulls/$number"
    $classification = @{ include = $true; complete = $false; kind = 'unclassified-pr'; pullRequest = @{ number = $number } }
    if ($metadata.available) {
        $items = @($metadata.data)
        $pr = if ($items.Count -eq 1) { $items[0] } else { $null }
        $title = [string](Get-VmrEvidenceProperty $pr 'title')
        $author = [string](Get-VmrEvidenceProperty (Get-VmrEvidenceProperty $pr 'user') 'login')
        $base = [string](Get-VmrEvidenceProperty (Get-VmrEvidenceProperty $pr 'base') 'ref')
        # author_association tells a team member's own PR (they own its CI, so the QB only monitors it) from codeflow bots and
        # outside contributors that need routing.
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
            [void](Add-VmrEvidenceError $State $Scope 'pr-classification' "PR $number metadata is missing its actual title, author, or base branch.")
        }
    }
    $State.prCache[$number] = $classification
    return $classification
}

function Get-VmrScopedBuilds {
    # The builds of an inventory that are in scope, each with its classification. complete is false when any build could not be
    # placed (no valid id, time, branch or result), because counts built from such an inventory would be a guess.
    param(
        [hashtable] $State, [object[]] $Builds, [string] $Scope,
        [ValidateSet('completed', 'active')][string] $Kind,
        [DateTimeOffset] $WindowStart, [DateTimeOffset] $WindowEnd
    )

    $timeField = if ($Kind -eq 'completed') { 'finishTime' } else { 'queueTime' }
    $included = [Collections.Generic.List[object]]::new()
    $complete = $true
    foreach ($build in $Builds) {
        $id = [string](Get-VmrEvidenceProperty $build 'id')
        if ($id -notmatch '^[1-9]\d*$') {
            [void](Add-VmrEvidenceError $State $Scope "$Kind-inventory" 'Inventory contains a build without a valid ID.')
            $complete = $false
            continue
        }
        try { $time = ConvertTo-VmrEvidenceTime (Get-VmrEvidenceProperty $build $timeField) }
        catch {
            [void](Add-VmrEvidenceError -State $State -Scope $Scope -Stage "$Kind-inventory-build-$id" -Exception $_.Exception)
            $complete = $false
            continue
        }
        # Completed: finished inside the inclusive window. Active: queued no later than the cutoff and still running or waiting.
        if ($Kind -eq 'completed' -and ($time -lt $WindowStart -or $time -gt $WindowEnd)) { continue }
        if ($Kind -eq 'active' -and ($time -gt $WindowEnd -or
            (Get-VmrEvidenceProperty $build 'status') -notin @('inProgress', 'notStarted', 'postponed', 'cancelling'))) { continue }
        $classification = Get-VmrBuildClassification -State $State -Build $build -Scope $Scope
        if (-not $classification.include) { continue }
        if (-not $classification.complete) { $complete = $false }
        if ($Kind -eq 'completed' -and
            (Get-VmrEvidenceProperty $build 'result') -notin @('succeeded', 'failed', 'canceled', 'partiallySucceeded')) {
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
        value = @($included.ToArray() | Sort-Object @{ Expression = { Get-VmrEvidenceProperty $_ $timeField } }, id)
    }
}

function Get-VmrEvidenceProblemTasks {
    # The records of a timeline worth reading: failed, canceled or succeeded with issues, or carrying any issue or counter. Each
    # one is tied to its Job (name, id and result) by walking up its parents, so a failed task under a Job that succeeded stays
    # identifiable as non-fatal even though that Job record is not itself kept.
    param([object[]] $Records)

    $byId = @{}
    foreach ($record in $Records) { $byId[[string]$record.id] = $record }
    foreach ($record in $Records) {
        $result = Get-VmrEvidenceProperty $record 'result'
        $issues = @(Get-VmrEvidenceProperty $record 'issues' @())
        $errorCount = Get-VmrEvidenceProperty $record 'errorCount' 0
        $warningCount = Get-VmrEvidenceProperty $record 'warningCount' 0
        if ($result -notin @('failed', 'canceled', 'succeededWithIssues') -and
            $issues.Count -eq 0 -and $errorCount -eq 0 -and $warningCount -eq 0) { continue }
        $parentId = [string](Get-VmrEvidenceProperty $record 'parentId')
        $ancestor = $parentId
        $job = if ($record.type -eq 'Job') { $record.name } else { $null }
        $jobId = if ($record.type -eq 'Job') { [string]$record.id } else { $null }
        $jobResult = if ($record.type -eq 'Job') { $result } else { $null }
        $visited = [Collections.Generic.HashSet[string]]::new()
        while (-not $jobId -and $ancestor -and $byId.ContainsKey($ancestor) -and $visited.Add($ancestor)) {
            $parent = $byId[$ancestor]
            if ($parent.type -eq 'Job') { $job = $parent.name; $jobId = [string]$parent.id; $jobResult = Get-VmrEvidenceProperty $parent 'result'; break }
            $ancestor = [string](Get-VmrEvidenceProperty $parent 'parentId')
        }
        [pscustomobject]@{
            id = $record.id; recordType = $record.type; task = $record.name; job = $job; jobId = $jobId; jobResult = $jobResult; parentId = $parentId
            result = $result; issues = $issues; errorCount = $errorCount; warningCount = $warningCount
            startTime = (Get-VmrEvidenceProperty $record 'startTime')
            finishTime = (Get-VmrEvidenceProperty $record 'finishTime')
            log = (Get-VmrEvidenceProperty $record 'log')
            workerName = (Get-VmrEvidenceProperty $record 'workerName')
        }
    }
}

function Test-VmrFailedRecord {
    # Failing by the record itself: failed or canceled, or carrying an error (a warning alone is not a failure).
    param([object] $Record)

    return (Get-VmrEvidenceProperty $Record 'result') -in @('failed', 'canceled') -or
        [int](Get-VmrEvidenceProperty $Record 'errorCount' 0) -gt 0 -or
        @(Get-VmrEvidenceProperty $Record 'issues' @() | Where-Object { (Get-VmrEvidenceProperty $_ 'type') -eq 'error' }).Count -gt 0
}

function Get-VmrBuildTimeline {
    # The root timeline of one build. Child timelines (record details) are not followed.
    param([hashtable] $State, [string] $Scope, [string] $Organization, [string] $Project, [long] $BuildId, [string] $Prefix)

    $source = Save-VmrEvidenceSource -State $State -Scope $Scope -Stage "timeline-$BuildId" -Name "$Prefix-timeline" `
        -Service AzureDevOps -Organization $Organization -Project $Project -ApiPath "build/builds/$BuildId/timeline"
    $timeline = @{ status = 'unavailable'; noTimeline = $false; path = $source.source.path; records = @() }
    # HTTP 204: the build never produced a timeline (the pipeline was rejected before any job ran). A complete answer, not a gap.
    if ($source.noContent) { $timeline.status = 'none'; $timeline.noTimeline = $true; return $timeline }
    if (-not $source.available) { return $timeline }
    $property = $source.data.PSObject.Properties['records']
    if ($null -eq $property -or $property.Value -isnot [array] -or $property.Value.Count -eq 0) {
        [void](Add-VmrEvidenceError $State $Scope "timeline-$BuildId" "Build $BuildId returned an empty or malformed timeline.")
        return $timeline
    }
    $records = @($property.Value | Where-Object {
        (Get-VmrEvidenceProperty $_ 'id') -and (Get-VmrEvidenceProperty $_ 'type') -and (Get-VmrEvidenceProperty $_ 'name')
    })
    if ($records.Count -ne $property.Value.Count) {
        [void](Add-VmrEvidenceError $State $Scope "timeline-$BuildId" "Build $BuildId contains a malformed timeline record.")
    }
    $timeline.status = 'available'
    $timeline.records = $records
    return $timeline
}

function Protect-VmrEvidenceDiagnostic {
    param([string] $Text)

    $redacted = [regex]::Replace($Text, '(?i)(Bearer\s+)[a-z0-9._~+/-]+=*', '$1[redacted]')
    return [regex]::Replace($redacted, '(?i)((?:access_token|accessToken|client_secret|password)\s*["=:]+\s*)[^\s",;]+', '$1[redacted]')
}

function Get-VmrExcerptText {
    # A short, credential-free excerpt: the first two and the last two non-empty lines, cut to a bounded length.
    param([AllowEmptyCollection()][string[]] $Lines, [ValidateRange(1, 65536)][int] $MaxCharacters = 1536)

    $kept = @($Lines | Where-Object { -not [string]::IsNullOrWhiteSpace($_) } | ForEach-Object { Protect-VmrEvidenceDiagnostic -Text $_ })
    if (-not $kept.Count) { return $null }
    if ($kept.Count -gt 4) { $kept = @($kept[0..1]) + @($kept[-2..-1]) }
    $text = $kept -join "`n"
    if ($text.Length -gt $MaxCharacters) { $text = $text.Substring(0, $MaxCharacters) }
    return $text
}

function Get-VmrLogDiagnostics {
    # The lines of a raw log that look like diagnostics (the first and the last few of them), or its last lines when none does.
    # Credentials are removed and every kept line is cut to a bounded length.
    param([string] $Path, [ValidateRange(2, 200)][int] $MaxLines = 24, [int] $MaxLineLength = 512)

    $pattern = [regex]::new('(?i)##\[(error|warning)\]|\berror\b|exception|fatal|timed?\s*out|cancel[le]+d|agent.*(lost|disconnect)|maximum.*minutes|throttl|Text file busy|exceeding usage|Unauthorized|publishing assets:')
    $half = [int]($MaxLines / 2)
    $first = [Collections.Generic.List[object]]::new()
    $last = [Collections.Generic.Queue[object]]::new()
    $ring = [string[]]::new($MaxLines)
    $matching = 0
    $lineNumber = 0
    foreach ($line in [IO.File]::ReadLines($Path)) {
        $lineNumber++
        $ring[$lineNumber % $MaxLines] = $line
        if (-not $pattern.IsMatch($line)) { continue }
        $matching++
        if ($first.Count -lt $half) { $first.Add(@{ line = $lineNumber; text = $line }) }
        else {
            $last.Enqueue(@{ line = $lineNumber; text = $line })
            if ($last.Count -gt $half) { [void]$last.Dequeue() }
        }
    }
    $picked = @(if ($matching) { $first; $last }
        elseif ($lineNumber) { ([math]::Max(1, $lineNumber - $MaxLines + 1))..$lineNumber | ForEach-Object { @{ line = $_; text = $ring[$_ % $MaxLines] } } })
    $excerpts = @($picked | ForEach-Object {
        $text = Protect-VmrEvidenceDiagnostic -Text $_.text
        @{ line = $_.line; text = $(if ($text.Length -gt $MaxLineLength) { $text.Substring(0, $MaxLineLength) } else { $text }) }
    })
    return @{ excerpts = $excerpts; matchingLines = $matching }
}

function Get-VmrPromotionBuildIds {
    # The internal builds a "Publish Using Darc" log links to (the promotion builds).
    param([string] $Path)

    $ids = [Collections.Generic.HashSet[long]]::new()
    $links = Select-String -LiteralPath $Path -AllMatches `
        -Pattern 'https://dev\.azure\.com/dnceng/internal/_build/results\?(?:[^\s<>"'')]*&)?buildId=(?<id>[1-9]\d*)'
    foreach ($hit in $links) { foreach ($match in $hit.Matches) { [void]$ids.Add([long]$match.Groups['id'].Value) } }
    return @($ids | Sort-Object)
}

function Get-VmrFailureSignature {
    # A wrapper alone cannot establish that two builds failed for the same reason.
    param([Parameter(Mandatory)][object] $Task, [string] $Scope)

    $messages = @($Task.issues | ForEach-Object {
        [string](Get-VmrEvidenceProperty $_ 'message')
    } | Where-Object {
        -not [string]::IsNullOrWhiteSpace($_) -and
        $_ -notmatch '(?i)MSB3073|Bash exited|Script failed|Cmd\.exe exited|PowerShell exited|Process completed with exit code|The operation was canceled|Encountered an error in generation workflow|^\W*The command .* exited with code'
    })
    if (-not $messages.Count) { return $null }
    $text = (($messages | Sort-Object -Unique) -join ' | ').ToLowerInvariant()
    $text = [regex]::Replace($text, '\b\d+(?:\.\d+){2,}[\w.+-]*', '<ver>')
    $text = [regex]::Replace($text, '[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}', '<guid>')
    $text = [regex]::Replace($text, '\b[0-9a-f]{7,}\b', '<hex>')
    $text = [regex]::Replace($text, '(?<![a-z0-9])\d{4,}(?![a-z0-9])', '#')
    $text = [regex]::Replace($text, '\s+', ' ').Trim()
    return "$Scope|$($Task.recordType)|$($Task.task)|$text"
}

function Get-VmrBuildEvidence {
    # Every problem build keeps its timeline. Matching failures share one representative raw log; wrappers without a diagnostic
    # remain independent. Promotion builds are read separately, one level deep, and filed under their parent build's id.
    param(
        [hashtable] $State, [string] $Scope, [string] $Organization, [string] $Project, [object] $Build,
        [long] $RootBuildId = 0, [long] $ParentBuildId = 0
    )

    $id = [long]$Build.id
    $isPromotion = $RootBuildId -gt 0
    if (-not $isPromotion) { $RootBuildId = $id }
    $prefix = if ($isPromotion) { "internal-promotion-$RootBuildId-$id" } else { "$Scope-build-$id" }
    $timeline = Get-VmrBuildTimeline -State $State -Scope $Scope -Organization $Organization -Project $Project -BuildId $id -Prefix $prefix
    $tasks = @(Get-VmrEvidenceProblemTasks -Records $timeline.records)
    if ($timeline.noTimeline) {
        # The build's own pre-flight validationResults are the diagnosis of a pipeline rejected before any job ran.
        $validation = @(Get-VmrEvidenceProperty $Build 'validationResults' @() |
            Where-Object { (Get-VmrEvidenceProperty $_ 'result') -in @('error', 'warning') })
        if ($validation.Count) {
            $tasks = @([pscustomobject]@{
                id = "preflight-$id"; recordType = 'PipelineValidation'; task = 'Pipeline validation'
                job = 'Pipeline pre-flight'; jobId = $null; jobResult = 'failed'; parentId = $null; result = 'failed'
                issues = @($validation | ForEach-Object {
                    [pscustomobject]@{ type = (Get-VmrEvidenceProperty $_ 'result'); message = (Get-VmrEvidenceProperty $_ 'message') }
                })
                errorCount = @($validation | Where-Object { (Get-VmrEvidenceProperty $_ 'result') -eq 'error' }).Count
                warningCount = 0
                startTime = (Get-VmrEvidenceProperty $Build 'startTime'); finishTime = (Get-VmrEvidenceProperty $Build 'finishTime')
                log = $null; workerName = $null
            })
        }
        else {
            [void](Add-VmrEvidenceError $State $Scope "timeline-$id" "Build $id produced no timeline and reported no pipeline validation results; its failure cannot be diagnosed.")
        }
    }
    $entry = [ordered]@{
        organization = $Organization; project = $Project; buildId = $RootBuildId
        definitionId = (Get-VmrEvidenceProperty (Get-VmrEvidenceProperty $Build 'definition') 'id')
        sourceBranch = (Get-VmrEvidenceProperty $Build 'sourceBranch'); result = (Get-VmrEvidenceProperty $Build 'result')
        finishTime = (Get-VmrEvidenceProperty $Build 'finishTime')
        timelineStatus = $timeline.status; timelinePath = $timeline.path; tasks = $tasks
    }
    if ($isPromotion) { $entry['promotionBuildId'] = $id; $entry['parentBuildId'] = $ParentBuildId; $entry['rootScope'] = $Scope }
    elseif ($timeline.status -eq 'available' -and $tasks.Count -eq 0) {
        [void](Add-VmrEvidenceError $State $Scope "root-logs-$id" "Problem build $id has no problem records; a root cause is not established.")
    }

    # A Job's own log is read only when none of its tasks has a failing log of its own.
    $candidates = @($tasks | Where-Object {
        $_.recordType -in @('Task', 'Job') -and (Test-VmrFailedRecord $_) -and [string](Get-VmrEvidenceProperty $_.log 'id') -match '^[1-9]\d*$'
    })
    $taskJobs = @($candidates | Where-Object recordType -eq 'Task' | ForEach-Object jobId)
    $candidates = @($candidates | Where-Object { $_.recordType -eq 'Task' -or $_.jobId -notin $taskJobs } |
        Sort-Object @{ Expression = { if ($_.task -match '\b(Build|Run Tests?|Validate installers?|Publish)\b') { 0 } else { 1 } } }, task, job, id)
    $fetched = @{}
    $promotionIds = [Collections.Generic.HashSet[long]]::new()
    $signatureBranch = ([string](Get-VmrEvidenceProperty $Build 'sourceBranch')) -replace '^refs/pull/\d+/', 'refs/pull/*/'
    foreach ($task in $candidates) {
        $logKey = [string](Get-VmrEvidenceProperty $task.log 'id')
        $signature = if (-not $isPromotion) { Get-VmrFailureSignature -Task $task -Scope "$Scope|$signatureBranch" }
        $sameFailure = if ($signature -and -not $fetched.ContainsKey($logKey) -and $State.failureSignatures.ContainsKey($signature)) {
            $State.failureSignatures[$signature]
        } else { $null }
        $cached = $null
        if ($null -eq $sameFailure) {
            if (-not $fetched.ContainsKey($logKey)) {
                if ($State.logsFetched[$Scope] -ge $State.maxLogsPerPipeline) { $State.logsSkipped[$Scope]++; continue }
                $State.logsFetched[$Scope]++
                $source = Save-VmrEvidenceSource -State $State -Scope $Scope -Stage "root-log-$id-$logKey" -Name "$prefix-log-$logKey" `
                    -Service AzureDevOps -Organization $Organization -Project $Project -ApiPath "build/builds/$id/logs/$logKey" -Text
                $diagnostics = $null
                if ($source.available) {
                    try { $diagnostics = Get-VmrLogDiagnostics -Path $source.source.path }
                    catch { [void](Add-VmrEvidenceError -State $State -Scope $Scope -Stage "log-excerpts-$id-$logKey" -Exception $_.Exception) }
                    if ($task.task -match '\bPublish Using Darc\b') {
                        foreach ($promotionId in (Get-VmrPromotionBuildIds -Path $source.source.path)) { [void]$promotionIds.Add($promotionId) }
                    }
                }
                $fetched[$logKey] = @{ source = $source; diagnostics = $diagnostics }
            }
            $cached = $fetched[$logKey]
            if ($signature -and $cached.source.available) {
                $State.failureSignatures[$signature] = [pscustomobject]@{
                    buildId = $id; logId = $task.log.id; task = $task.task
                    organization = $Organization; project = $Project; path = $cached.source.source.path
                }
            }
        }
        $diagnostics = if ($null -ne $cached) { $cached.diagnostics } else { $null }
        $issueText = Get-VmrExcerptText -Lines @($task.issues | ForEach-Object {
            $message = [string](Get-VmrEvidenceProperty $_ 'message')
            if ($message) { '[{0}] {1}' -f (Get-VmrEvidenceProperty $_ 'type'), $message }
        })
        $logText = if ($null -ne $diagnostics) {
            Get-VmrExcerptText -Lines @($diagnostics.excerpts | ForEach-Object { 'L{0}: {1}' -f $_.line, $_.text })
        }
        $useLog = $null -ne $diagnostics -and ($diagnostics.matchingLines -gt 0 -or -not $issueText)
        $excerpts = @(if ($null -ne $diagnostics) { $diagnostics.excerpts })
        $logEntry = [ordered]@{
            buildId = $RootBuildId; task = $task.task; job = $task.job; jobId = $task.jobId; jobResult = $task.jobResult
            logId = (Get-VmrEvidenceProperty $task.log 'id')
            path = $(if ($null -ne $cached -and $cached.source.available) { $cached.source.source.path } else { $null })
            recordId = $task.id; recordType = $task.recordType; result = $task.result
            evidenceKind = $(if ($null -ne $sameFailure) { 'sameFailureSummary' } else { 'failure' })
            available = ($null -ne $cached -and $cached.source.available)
            error = $(if ($null -ne $cached) { $cached.source.source.error } else { $null })
            diagnosticExcerpts = $excerpts; diagnosticExcerpt = $(if ($useLog) { $logText } else { $issueText })
            organization = $Organization; project = $Project; actualBuildId = $id
        }
        if ($null -ne $sameFailure) { $logEntry['sameFailureAs'] = $sameFailure }
        if ($isPromotion) { $logEntry['promotionBuildId'] = $id; $logEntry['parentBuildId'] = $ParentBuildId }
        $State.logs.Add([pscustomobject]$logEntry)
    }

    $followed = 0
    foreach ($promotionId in @($promotionIds | Sort-Object)) {
        if ($promotionId -eq $id -or $promotionId -eq $RootBuildId) { continue }
        if ($isPromotion -or $followed -ge $State.maxPromotionsPerBuild) {
            [void](Add-VmrEvidenceError $State $Scope "promotion-$RootBuildId-$promotionId" (
                "Promotion $promotionId referenced by build $id was not followed because the explicit promotion limit (one level, $($State.maxPromotionsPerBuild) builds per parent) was reached."))
            $State.promotions.Add([pscustomobject]@{
                buildId = $RootBuildId; parentBuildId = $id; promotionBuildId = $promotionId; rootScope = $Scope
                timelineStatus = 'unavailable'; timelinePath = $null; tasks = @()
            })
            continue
        }
        $followed++
        $State.promotions.Add((Get-VmrBuildEvidence -State $State -Scope $Scope -Organization dnceng -Project internal `
            -Build ([pscustomobject]@{ id = $promotionId; result = 'unknown' }) -RootBuildId $RootBuildId -ParentBuildId $id))
    }
    return [pscustomobject]$entry
}

function New-VmrPipelineEvidenceRecord {
    param([string] $Scope)

    return [ordered]@{
        name = $Scope; status = 'unavailable'; inventoryPath = $null; partialInventoryPath = $null; activeInventoryPaths = @()
        requiredProblemBuildIds = @(); error = $null; loginCommand = $null
        counts = [ordered]@{ completed = $null; succeeded = $null; failed = $null; canceled = $null; partiallySucceeded = $null }
    }
}

function Invoke-VmrPipelineEvidence {
    # One pipeline: its definition, the completed builds of the window (the counts), the active builds, and the evidence of each
    # failed, canceled or partiallySucceeded build. Repeated failures share a representative raw log.
    param([hashtable] $State, [string] $Scope, [DateTimeOffset] $WindowStart, [DateTimeOffset] $WindowEnd)

    $organization = if ($Scope -eq 'internal') { 'dnceng' } else { 'dnceng-public' }
    $project = $Scope
    $pipeline = New-VmrPipelineEvidenceRecord -Scope $Scope
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
    $activePaths = [Collections.Generic.List[string]]::new()
    $inventoryComplete = $true
    $start = [Uri]::EscapeDataString($WindowStart.ToUniversalTime().ToString('o'))
    $end = [Uri]::EscapeDataString($WindowEnd.ToUniversalTime().ToString('o'))
    foreach ($definition in $exact) {
        $completed = Save-VmrEvidenceSource -State $State -Scope $Scope -Stage 'completed-inventory' `
            -Name "$Scope-builds-$($definition.id)-completed" -Service AzureDevOps -Organization $organization -Project $project `
            -ApiPath "build/builds?definitions=$($definition.id)&statusFilter=completed&queryOrder=finishTimeAscending&minTime=$start&maxTime=$end&`$top=200"
        if ($completed.available -and $null -ne $completed.data.PSObject.Properties['value'] -and $completed.data.value -is [array]) {
            foreach ($build in $completed.data.value) { $allBuilds.Add($build) }
        }
        else {
            $inventoryComplete = $false
            if ($completed.available -or $completed.noContent) {
                [void](Add-VmrEvidenceError $State $Scope 'completed-inventory' 'Completed inventory response has no complete value array.')
            }
        }
        $active = Save-VmrEvidenceSource -State $State -Scope $Scope -Stage 'active-inventory' `
            -Name "$Scope-builds-$($definition.id)-active" -Service AzureDevOps -Organization $organization -Project $project `
            -ApiPath "build/builds?definitions=$($definition.id)&statusFilter=inProgress,notStarted,postponed,cancelling&queryOrder=queueTimeAscending&`$top=200"
        if (-not $active.available) { continue }
        if ($null -eq $active.data.PSObject.Properties['value'] -or $active.data.value -isnot [array]) {
            [void](Add-VmrEvidenceError $State $Scope 'active-inventory' 'Active inventory response has no complete value array.')
            continue
        }
        $scopedActive = Get-VmrScopedBuilds -State $State -Builds @($active.data.value) -Scope $Scope -Kind active `
            -WindowStart $WindowStart -WindowEnd $WindowEnd
        $path = Join-Path $State.directory "$Scope-builds-$($definition.id)-active-scoped.json"
        # Active status is observed later than the cutoff; it is not a reconstruction of "active at the cutoff".
        Write-VmrEvidenceJson -Path $path -Value @{
            status = if ($scopedActive.complete) { 'complete' } else { 'partial' }
            count = $scopedActive.value.Count; value = $scopedActive.value
            observedAtUtc = $active.source.observedAtUtc; observationCutoffUtc = $WindowEnd.ToUniversalTime().ToString('o')
        }
        $activePaths.Add($path)
    }
    $pipeline.activeInventoryPaths = $activePaths.ToArray()
    $scoped = Get-VmrScopedBuilds -State $State -Builds $allBuilds.ToArray() -Scope $Scope -Kind completed `
        -WindowStart $WindowStart -WindowEnd $WindowEnd
    $complete = $inventoryComplete -and $scoped.complete
    $inventoryPath = Join-Path $State.directory "$Scope-completed-builds.json"
    Write-VmrEvidenceJson -Path $inventoryPath -Value @{
        status = if ($complete) { 'complete' } else { 'partial' }; count = $scoped.value.Count; value = $scoped.value
        windowStartUtc = $WindowStart.ToUniversalTime().ToString('o'); observationCutoffUtc = $WindowEnd.ToUniversalTime().ToString('o')
    }
    if ($complete) {
        $pipeline.inventoryPath = $inventoryPath
        $pipeline.counts = [ordered]@{
            completed = $scoped.value.Count
            succeeded = @($scoped.value | Where-Object result -eq 'succeeded').Count
            failed = @($scoped.value | Where-Object result -eq 'failed').Count
            canceled = @($scoped.value | Where-Object result -eq 'canceled').Count
            partiallySucceeded = @($scoped.value | Where-Object result -eq 'partiallySucceeded').Count
        }
    }
    else { $pipeline.partialInventoryPath = $inventoryPath }
    $problems = @($scoped.value | Where-Object result -in @('failed', 'canceled', 'partiallySucceeded'))
    $pipeline.requiredProblemBuildIds = @($problems | ForEach-Object { [long]$_.id } | Sort-Object)
    foreach ($build in $problems) {
        try { $entry = Get-VmrBuildEvidence -State $State -Scope $Scope -Organization $organization -Project $project -Build $build }
        catch {
            # One build that cannot be read is a gap for that build (and an error that says so), not a reason to lose the rest.
            [void](Add-VmrEvidenceError -State $State -Scope $Scope -Stage "build-$($build.id)" -Exception $_.Exception)
            $entry = [pscustomobject]@{
                organization = $organization; project = $project; buildId = $build.id
                result = $build.result; sourceBranch = $build.sourceBranch; finishTime = $build.finishTime
                timelineStatus = 'unavailable'; timelinePath = $null; tasks = @()
            }
        }
        $State.builds.Add($entry)
    }
    if ($State.logsSkipped[$Scope]) {
        [void](Add-VmrEvidenceError $State $Scope 'root-logs' (
            "The explicit per-pipeline log limit ($($State.maxLogsPerPipeline)) was reached: the raw logs of $($State.logsSkipped[$Scope]) failing tasks were not downloaded (their timeline messages are retained)."))
    }
    return $pipeline
}

function Invoke-VmrDailyEvidence {
    <#
    .SYNOPSIS
    Collects the evidence of one daily round into OutputDirectory and returns the path of its index, sources.json (schema 2).
    .DESCRIPTION
    Both unified-build pipelines (internal first) are read over one window: the pipeline definition, the completed builds, the
    active builds and every problem build's root timeline. Matching failures share one representative raw log per scope/branch;
    an unclassifiable failure is read separately. failure-tasks.json and root-logs.json are written beside the index. An unread
    representative is an error, but a repeated failure linked to its representative is not a coverage gap. The pipeline is
    then partial, unavailable or authentication_required, and its counts and inventoryPath stay null unless the whole completed
    inventory was read. No sign-in fallback is attempted, and no credential is written anywhere.
    #>
    param(
        [Parameter(Mandatory)][ValidatePattern('^\d{4}-\d{2}-\d{2}$')][string] $ReportDate,
        [Parameter(Mandatory)][DateTimeOffset] $WindowStart,
        [Parameter(Mandatory)][DateTimeOffset] $WindowEnd,
        [Parameter(Mandatory)][string] $OutputDirectory
    )

    if ($WindowStart -ge $WindowEnd) { throw 'The monitoring window start must precede its end.' }
    if (-not [IO.Path]::IsPathFullyQualified($OutputDirectory)) { throw 'OutputDirectory must be an absolute path.' }
    $directory = [IO.Path]::GetFullPath($OutputDirectory)
    [void][IO.Directory]::CreateDirectory($directory)
    $maxLogsPerPipeline = 1000
    # Cover both log budgets plus inventory/timeline requests and retries.
    $session = New-VmrReadOnlySession -MaxRequests (2 * $maxLogsPerPipeline + 1000)
    $state = @{
        directory = $directory; session = $session
        sources = [Collections.Generic.List[object]]::new(); errors = [Collections.Generic.List[object]]::new()
        builds = [Collections.Generic.List[object]]::new(); logs = [Collections.Generic.List[object]]::new()
        promotions = [Collections.Generic.List[object]]::new(); prCache = @{}; failureSignatures = @{}
        maxLogsPerPipeline = $maxLogsPerPipeline; maxPromotionsPerBuild = 4
        logsFetched = @{ internal = 0; public = 0 }; logsSkipped = @{ internal = 0; public = 0 }
    }
    try {
        $pipelines = @{}
        foreach ($scope in @('internal', 'public')) {
            # The two pipelines are collected independently: one that fails unexpectedly is an error of that pipeline only.
            try { $pipelines[$scope] = Invoke-VmrPipelineEvidence -State $state -Scope $scope -WindowStart $WindowStart -WindowEnd $WindowEnd }
            catch {
                [void](Add-VmrEvidenceError -State $state -Scope $scope -Stage 'pipeline-collection' -Exception $_.Exception)
                $pipelines[$scope] = New-VmrPipelineEvidenceRecord -Scope $scope
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
        $failureTasksPath = Join-Path $directory 'failure-tasks.json'
        $rootLogsPath = Join-Path $directory 'root-logs.json'
        Write-VmrEvidenceJson -Path $failureTasksPath -Value @{ builds = $state.builds.ToArray(); promotions = $state.promotions.ToArray() }
        Write-VmrEvidenceJson -Path $rootLogsPath -Value @{ logs = $state.logs.ToArray() }
        $indexPath = Join-Path $directory 'sources.json'
        Write-VmrEvidenceJson -Path $indexPath -Value ([ordered]@{
            schemaVersion = 2; reportDate = $ReportDate
            windowStartUtc = $WindowStart.ToUniversalTime().ToString('o'); observationCutoffUtc = $WindowEnd.ToUniversalTime().ToString('o')
            collectedAtUtc = [DateTimeOffset]::UtcNow.ToString('o')
            status = if ($state.errors.Count) { 'partial' } else { 'complete' }
            failureTasksPath = $failureTasksPath; rootLogsPath = $rootLogsPath
            pipelines = @($pipelines.public, $pipelines.internal)
            requiredErrors = $state.errors.ToArray(); sources = $state.sources.ToArray()
        })
        return $indexPath
    }
    finally {
        $session.tokens.Clear()
        $session.authenticationFailures.Clear()
    }
}
