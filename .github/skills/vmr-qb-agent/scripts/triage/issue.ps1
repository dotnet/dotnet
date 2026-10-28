Set-StrictMode -Version Latest

# triage/issue.ps1 - Which dotnet/dotnet issues already track the failing builds. Read-only: every request is an explicit GET.
# Dot-sourced by the entry scripts and the tests; it defines functions only and runs nothing.

function Invoke-VmrIssueGhGet {
    # One read-only GitHub GET through gh. Data is the parsed JSON and HasNext says another page exists. Error is empty on success;
    # Stop is true for a sign-in or rate-limit problem, which another request would only repeat.
    param([Parameter(Mandatory)][string] $Resource)
    if ($Resource -notmatch '^(repos/dotnet/dotnet/issues|search/issues)\?') { throw 'Unsupported read-only GitHub resource.' }
    $ErrorActionPreference = 'Continue'
    $PSNativeCommandUseErrorActionPreference = $false
    try {
        $raw = @(& gh api --hostname github.com --method GET --include $Resource 2>&1)
        $exitCode = $LASTEXITCODE
    }
    catch { return [pscustomobject]@{ Data = $null; HasNext = $false; Stop = $true; Error = "The GitHub CLI could not be run ($($_.Exception.Message)). Install gh and sign in with 'gh auth login'." } }
    $text = ($raw | ForEach-Object { "$_" }) -join "`n"
    $status = if ($text -match '(?m)^HTTP/\S+\s+(\d{3})') { [int]$Matches[1] } else { 0 }
    $response = [regex]::Match($text, '(?s)^HTTP/\S+[^\r\n]*\r?\n(?<headers>.*?)\r?\n\r?\n(?<body>.*)$')
    if ($exitCode -eq 0 -and $status -eq 200 -and $response.Success) {
        return [pscustomobject]@{
            Data    = ConvertFrom-Json -InputObject $response.Groups['body'].Value -Depth 20 -NoEnumerate
            HasNext = $response.Groups['headers'].Value -match '(?im)^link:.*;\s*rel="next"'
            Stop    = $false; Error = ''
        }
    }
    $detail = if ($text -match '(?m)^gh:\s*(?<message>.+)$') { $Matches['message'].Trim() } else { ($text -replace '\s+', ' ').Trim() }
    if ($detail.Length -gt 300) { $detail = $detail.Substring(0, 300) }
    $detail = $detail.TrimEnd('.')
    $hint = if ($detail -match 'rate limit') { ' GitHub is limiting this account; wait a few minutes and run the round again.' }
            elseif (($status -in 401, 403) -or $detail -match 'gh auth login') { " Sign in with 'gh auth login' (authorize SSO for the dotnet organization when asked) and run the round again." }
            else { '' }
    return [pscustomobject]@{ Data = $null; HasNext = $false; Stop = [bool]$hint; Error = "GitHub GET failed (HTTP $status, gh exit $exitCode): $detail.$hint" }
}

function New-VmrIssueTriageEvidence {
    <#
    .SYNOPSIS
    Reads which dotnet/dotnet issues already track the failing builds and writes <OutputDirectory>\issue-evidence.json.
    .DESCRIPTION
    Two read-only lookups: the open and closed issues labeled area-unified-build-OperationalIssue (newest update first, at most 300 of
    each), and one exact search per build id (title, body or comments), at least 2 seconds apart to stay under GitHub's search rate
    limit. matchedBuildIds lists the supplied build ids an issue mentions; kind says where it was first found (the label list, else a
    build-id search). status is 'complete' only when every request succeeded and nothing was cut off; otherwise it is 'partial' and
    errors says what was not read. A search with no hit is not proof that no tracker exists. assigneeCandidates and fixPullRequests
    are always empty: the model looks those up itself. Returns the path of the file.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][string] $OutputDirectory,
        [Parameter(Mandatory)][AllowEmptyCollection()][long[]] $BuildIds,
        # Not used: GitHub is read as it is now, not reconstructed at the cutoff. Accepted so the caller can pass the collector's cutoff.
        [DateTimeOffset] $ObservationCutoffUtc
    )
    $ErrorActionPreference = 'Stop'
    if (-not [IO.Path]::IsPathFullyQualified($OutputDirectory)) { throw 'OutputDirectory must be an absolute path.' }
    if (@($BuildIds | Where-Object { $_ -le 0 }).Count) { throw 'BuildIds must be positive integers.' }
    $ids = @($BuildIds | Select-Object -Unique)
    $repository = 'dotnet/dotnet'
    $maxPages = 3
    $idPattern = if ($ids.Count) { '(?<!\d)(?:' + ($ids -join '|') + ')(?!\d)' } else { '' }
    $issues = @{}
    $errors = [Collections.Generic.List[object]]::new()
    # Adds an issue, or the build ids it mentions, to the result. Pull requests are not issues.
    $record = {
        param($item, [string] $kind, [long[]] $searched)
        if ($null -ne $item.PSObject.Properties['pull_request']) { return }
        $mentioned = if ($idPattern) { @([regex]::Matches("$($item.title)`n$($item.body)", $idPattern) | ForEach-Object { [long]$_.Value }) } else { @() }
        $key = "$($item.number)"
        if (-not $issues.ContainsKey($key)) {
            $issues[$key] = [pscustomobject]@{
                number = [long]$item.number; repository = $repository; url = [string]$item.html_url; title = [string]$item.title
                state = [string]$item.state; kind = $kind; matchedBuildIds = @()
            }
        }
        $issues[$key].matchedBuildIds = @(@($issues[$key].matchedBuildIds) + $mentioned + $searched | Sort-Object -Unique)
    }
    $stop = $false
    try {
        foreach ($issueState in 'open', 'closed') {
            for ($page = 1; $page -le $maxPages -and -not $stop; $page++) {
                $resource = "repos/$repository/issues?state=$issueState&labels=area-unified-build-OperationalIssue&sort=updated&direction=desc&per_page=100&page=$page"
                $reply = Invoke-VmrIssueGhGet $resource
                if ($reply.Error) { $errors.Add([pscustomobject]@{ resource = $resource; message = $reply.Error }); $stop = $reply.Stop; break }
                foreach ($item in @($reply.Data)) { & $record $item 'operational-label' @() }
                if (-not $reply.HasNext) { break }
                if ($page -eq $maxPages) { $errors.Add([pscustomobject]@{ resource = $resource; message = "Only the newest $($maxPages * 100) $issueState operational issues were read; older ones may exist." }) }
            }
        }
        foreach ($id in $ids) {
            if ($stop) { break }
            if ($id -ne $ids[0]) { Start-Sleep -Milliseconds 2100 }
            $resource = "search/issues?q=$([Uri]::EscapeDataString("repo:$repository is:issue in:title,body,comments $id"))&sort=updated&order=desc&per_page=25&page=1"
            $reply = Invoke-VmrIssueGhGet $resource
            if ($reply.Error) { $errors.Add([pscustomobject]@{ resource = $resource; message = $reply.Error }); $stop = $reply.Stop; continue }
            $hits = @($reply.Data.items)
            foreach ($item in $hits) { & $record $item 'build-id-search' $id }
            if ($reply.Data.incomplete_results -or $reply.Data.total_count -gt $hits.Count) {
                $errors.Add([pscustomobject]@{ resource = $resource; message = "The search for build $id was cut off (GitHub marked it incomplete, or it has more hits than the 25 read); a missing hit is inconclusive." })
            }
        }
    }
    catch { $errors.Add([pscustomobject]@{ resource = 'issue-evidence-collection'; message = $_.Exception.Message }) }

    $evidence = [ordered]@{
        schemaVersion      = 1
        status             = if ($errors.Count) { 'partial' } else { 'complete' }
        observedAtUtc      = [DateTimeOffset]::UtcNow.ToString('o')
        issues             = @($issues.Values | Sort-Object number)
        assigneeCandidates = @()
        fixPullRequests    = @()
        errors             = $errors.ToArray()
    }
    [void][IO.Directory]::CreateDirectory($OutputDirectory)
    $path = Join-Path $OutputDirectory 'issue-evidence.json'
    [IO.File]::WriteAllText($path, (ConvertTo-Json -InputObject $evidence -Depth 6), [Text.UTF8Encoding]::new($false))
    return $path
}
