#Requires -Version 7.0
<#
.SYNOPSIS
Offline tests for the pooled/prefetch transport fast paths. No network access.
.DESCRIPTION
Guards the three latency fixes: verbatim single-page persistence, prefetch cache
consumption, and the rule that injected (mock) transports never prefetch.
#>
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# Offline guard. The over-limit cache case below deliberately falls through to a real request once the
# cached body is discarded. Pin the default proxy to a closed loopback port so that request cannot leave
# this machine, and refuse to run at all if the pin did not take effect.
foreach ($name in @('HTTP_PROXY', 'HTTPS_PROXY', 'ALL_PROXY')) { [Environment]::SetEnvironmentVariable($name, 'http://127.0.0.1:9') }
[Environment]::SetEnvironmentVariable('NO_PROXY', $null)
$defaultProxy = [Net.Http.HttpClient]::DefaultProxy
$pinned = if ($defaultProxy.GetType().Name -eq 'HttpEnvironmentProxy') { $defaultProxy.GetProxy([uri]'https://dev.azure.com/') } else { $null }
if ($null -eq $pinned -or $pinned.Host -ne '127.0.0.1' -or $pinned.Port -ne 9) {
    throw 'Offline guard failed: the HTTP proxy could not be pinned to loopback, so this test would reach the network.'
}

. (Join-Path $PSScriptRoot '..\daily\evidence.ps1')

$script:Failures = 0
function Test-Case {
    param([string] $Name, [scriptblock] $Body)
    try { & $Body; Write-Host "PASS: $Name" -ForegroundColor Green }
    catch { $script:Failures++; Write-Host "FAIL: $Name -> $($_.Exception.Message)" -ForegroundColor Red }
}
function Assert-True { param([bool] $Condition, [string] $Message) if (-not $Condition) { throw $Message } }

$work = Join-Path ([IO.Path]::GetTempPath()) ("transport-test-" + [Guid]::NewGuid().ToString('N'))
[void][IO.Directory]::CreateDirectory($work)
$counter = 0
function New-WorkPath { $script:counter++; return (Join-Path $work "file-$script:counter.json") }

try {
    Test-Case 'Single-page responses are persisted verbatim, not re-serialized' {
        # Deliberately unusual spacing/ordering/precision that ConvertTo-Json would rewrite.
        $raw = '{  "id" : "abc" ,' + "`n" + '"records":[{"id":"r1","type":"Task","name":"Build","order":1.50}],"deep":{"a":{"b":{"c":[1,2,3]}}}}'
        $session = New-VmrReadOnlySession -HttpGet {
            param($uri, $headers, $path, $timeout, $maxBytes)
            @{ statusCode = 200; headers = @{}; contentType = 'application/json'; content = $raw }
        }.GetNewClosure()
        $path = New-WorkPath
        $data = Invoke-VmrReadOnlyRequest -Session $session -Service AzureDevOps -ApiPath 'build/builds/1/timeline' -OutputPath $path
        $written = [IO.File]::ReadAllText($path)
        Assert-True ($written -ceq $raw) 'Retained timeline text is not byte-faithful to the response.'
        Assert-True ($data.records.Count -eq 1) 'Parsed records were not returned to the caller.'
        Assert-True (-not $written.Contains('1.5,')) 'Response body was re-serialized.'
    }

    Test-Case 'Prefetched bodies are served once and drain the cache (OutputPath form)' {
        $body = '{"records":[{"id":"r1","type":"Task","name":"Build"}]}'
        $temp = New-WorkPath
        [IO.File]::WriteAllText($temp, $body)
        $uri = [uri]'https://dev.azure.com/dnceng-public/public/_apis/build/builds/7/timeline?api-version=7.1'
        $script:VmrPrefetchCache[$uri.AbsoluteUri] = @{ path = $temp; contentType = 'application/json' }
        $out = New-WorkPath
        $result = Invoke-VmrEvidenceHttpGet -Uri $uri -Headers @{ Accept = 'application/json' } -OutputPath $out -TimeoutSeconds 30 -MaxBytes 8MB
        Assert-True ($result.statusCode -eq 200) 'Cached serve did not report success.'
        Assert-True ([IO.File]::ReadAllText($out) -ceq $body) 'Cached body was not delivered verbatim.'
        Assert-True (-not [IO.File]::Exists($temp)) 'Cached temp file was not consumed.'
        Assert-True ($script:VmrPrefetchCache.Count -eq 0) 'Cache entry was not removed after serving.'
    }

    Test-Case 'Prefetched bodies are served once and drain the cache (in-memory form)' {
        $body = '{"count":1,"value":[{"id":9}]}'
        $temp = New-WorkPath
        [IO.File]::WriteAllText($temp, $body)
        $uri = [uri]'https://dev.azure.com/dnceng/internal/_apis/build/builds/8/timeline?api-version=7.1'
        $script:VmrPrefetchCache[$uri.AbsoluteUri] = @{ path = $temp; contentType = 'application/json' }
        $result = Invoke-VmrEvidenceHttpGet -Uri $uri -Headers @{ Accept = 'application/json' } -OutputPath $null -TimeoutSeconds 30 -MaxBytes 8MB
        Assert-True ($result.content -ceq $body) 'Cached in-memory body was altered.'
        Assert-True (-not [IO.File]::Exists($temp)) 'Cached temp file was not deleted.'
        Assert-True ($script:VmrPrefetchCache.Count -eq 0) 'Cache entry was not removed.'
    }

    Test-Case 'Oversized cached bodies are discarded rather than served past the limit' {
        $oversized = 'y' * 4096
        $temp = New-WorkPath
        [IO.File]::WriteAllText($temp, $oversized)
        $uri = [uri]'https://dev.azure.com/dnceng-public/public/_apis/build/builds/11/timeline?api-version=7.1'
        $script:VmrPrefetchCache[$uri.AbsoluteUri] = @{ path = $temp; contentType = 'application/json' }
        # The fall-through re-request is bounded to 1s and its outcome is irrelevant here.
        $served = $null
        try {
            $served = Invoke-VmrEvidenceHttpGet -Uri $uri -Headers @{ Accept = 'application/json' } -OutputPath $null -TimeoutSeconds 1 -MaxBytes 16
        }
        catch { }
        Assert-True (-not [IO.File]::Exists($temp)) 'Discarded cache file was not cleaned up.'
        Assert-True ($script:VmrPrefetchCache.Count -eq 0) 'Discarded cache entry was not removed.'
        if ($null -ne $served -and $served.content) {
            Assert-True ($served.content -cne $oversized) 'An over-limit cached body was served anyway.'
        }
    }

    Test-Case 'Injected transports never prefetch; the default pooled transport does' {
        $mock = New-VmrReadOnlySession -HttpGet { param($uri, $headers, $path, $timeout, $maxBytes) @{} }
        Assert-True (-not $mock.prefetchEnabled) 'A mocked transport must stay strictly serial.'
        $real = New-VmrReadOnlySession
        Assert-True ($real.prefetchEnabled) 'The default pooled transport should warm requests.'
        $warmed = Invoke-VmrEvidencePrefetch -Session $mock -ApiPaths @('build/builds/1/timeline', 'build/builds/2/timeline')
        Assert-True ($warmed -eq 0) 'A mocked transport performed a prefetch.'
        Assert-True ($script:VmrPrefetchCache.Count -eq 0) 'A mocked transport populated the cache.'
    }

    Test-Case 'Prefetch is skipped without a cached token or a batch worth parallelising' {
        $real = New-VmrReadOnlySession
        Assert-True ((Invoke-VmrEvidencePrefetch -Session $real -ApiPaths @('build/builds/1/timeline')) -eq 0) 'Single-path batches should not prefetch.'
        Assert-True ((Invoke-VmrEvidencePrefetch -Session $real -ApiPaths @()) -eq 0) 'Empty batches should not prefetch.'
        $blocked = Invoke-VmrEvidencePrefetch -Session $real -Organization 'dnceng' -Project 'internal' `
            -ApiPaths @('build/builds/1/timeline', 'build/builds/2/timeline')
        Assert-True ($blocked -eq 0) 'Internal prefetch must not acquire a token on its own.'
        Assert-True ($script:VmrPrefetchCache.Count -eq 0) 'Skipped prefetch populated the cache.'
    }

    Test-Case 'Raw persistence is atomic and leaves no temporary files' {
        $path = New-WorkPath
        Write-VmrEvidenceRawJson -Path $path -Text '{"ok":true}'
        Assert-True ([IO.File]::ReadAllText($path) -ceq '{"ok":true}') 'Raw text was not persisted verbatim.'
        Assert-True (@(Get-ChildItem $work -Filter '*.tmp').Count -eq 0) 'A temporary file was left behind.'
        $bytes = [IO.File]::ReadAllBytes($path)
        Assert-True ($bytes[0] -ne 0xEF) 'Raw JSON must not be written with a UTF-8 BOM.'
    }

    Test-Case 'Clearing the prefetch cache removes every warmed file and its directory' {
        $directory = Get-VmrPrefetchDirectory
        $temp = Join-Path $directory ([Guid]::NewGuid().ToString('N'))
        [IO.File]::WriteAllText($temp, 'x')
        $script:VmrPrefetchCache['https://dev.azure.com/x'] = @{ path = $temp; contentType = 'application/json' }
        Clear-VmrEvidencePrefetch
        Assert-True (-not [IO.File]::Exists($temp)) 'Warmed file survived the cache clear.'
        Assert-True (-not [IO.Directory]::Exists($directory)) 'Prefetch directory survived the cache clear.'
        Assert-True ($script:VmrPrefetchCache.Count -eq 0) 'Cache was not emptied.'
    }
}
finally {
    Clear-VmrEvidencePrefetch
    if ([IO.Directory]::Exists($work)) { [IO.Directory]::Delete($work, $true) }
}

if ($script:Failures) { Write-Host "$script:Failures transport test(s) failed." -ForegroundColor Red; exit 1 }
Write-Host '8 offline transport tests (one fall-through request, pinned to a closed loopback port). No reports, email, or external changes.' -ForegroundColor Green
