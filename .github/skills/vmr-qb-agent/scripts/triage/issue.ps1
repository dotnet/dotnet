Set-StrictMode -Version Latest

# triage/issue.ps1 - Issue triage: fatal vs non-fatal classification, issue/PR evidence and the triage-context registry.
# Dot-sourced by the entry scripts and the tests; it defines functions only and runs nothing.

# ---------------------------------------------------------------------------------------------
# triage/Triage.ps1
# ---------------------------------------------------------------------------------------------
function Protect-VmrIssueTriageText {
    param([AllowNull()][AllowEmptyString()][string] $Text)

    if ($null -eq $Text) { return '' }
    $text = $Text
    foreach ($name in @('GH_TOKEN', 'GITHUB_TOKEN', 'GH_ENTERPRISE_TOKEN', 'GITHUB_ENTERPRISE_TOKEN')) {
        $credential = [Environment]::GetEnvironmentVariable($name)
        if (-not [string]::IsNullOrEmpty($credential)) { $text = $text.Replace($credential, '[REDACTED]') }
    }
    $text = [regex]::Replace($text, '(?i)\b(?:github_pat_|gh[pousr]_)[A-Za-z0-9_]+', '[REDACTED]')
    $text = [regex]::Replace($text, '\beyJ[A-Za-z0-9_-]+\.[A-Za-z0-9_-]+\.[A-Za-z0-9_-]+\b', '[REDACTED]')
    $text = [regex]::Replace($text, '(?im)(\b(?:Authorization|Proxy-Authorization)\s*[:=]\s*)[^\r\n]+', '$1[REDACTED]')
    $text = [regex]::Replace($text, '(?i)\b(Bearer|Basic)\s+[A-Za-z0-9+/=_-]+', '$1 [REDACTED]')
    $text = [regex]::Replace($text,
        '(?i)((?:[?&]|&amp;)(?:access_token|token|sig|client_secret|password|key|api_key)=)[^&\s"''<>]+', '$1[REDACTED]')
    $text = [regex]::Replace($text,
        '(?i)(["'']?(?:GH_TOKEN|GITHUB_TOKEN|access_token|refresh_token|client_secret|password|token|api_key|apikey)["'']?\s*[:=]\s*["'']?)[^\s,"''}]+',
        '$1[REDACTED]')
    return [regex]::Replace($text, '(?i)(https?://)[^/\s@]+:[^/\s@]+@', '$1[REDACTED]@')
}

function Protect-VmrIssueTriageValue {
    param([AllowNull()][object] $Value, [string] $Name)

    if ($null -eq $Value) { return $null }
    if ($Name -match '^(Authorization|Proxy-Authorization|(?:GH|GITHUB)(?:_ENTERPRISE)?_TOKEN|access_token|refresh_token|client_secret|password|token|api_key|apikey|sig)$') {
        return '[REDACTED]'
    }
    if ($Value -is [string]) { return Protect-VmrIssueTriageText $Value }
    if ($Value -is [Collections.IDictionary]) {
        $result = [ordered]@{}
        foreach ($key in $Value.Keys) {
            $result[$key] = Protect-VmrIssueTriageValue -Value $Value[$key] -Name ([string]$key)
        }
        return $result
    }
    if ($Value -is [Collections.IEnumerable]) {
        $items = [Collections.Generic.List[object]]::new()
        foreach ($item in $Value) { $items.Add((Protect-VmrIssueTriageValue -Value $item)) }
        return ,$items.ToArray()
    }
    if ($Value -is [pscustomobject]) {
        $result = [ordered]@{}
        foreach ($property in $Value.PSObject.Properties) {
            $result[$property.Name] = Protect-VmrIssueTriageValue -Value $property.Value -Name $property.Name
        }
        return [pscustomobject]$result
    }
    return $Value
}

function Invoke-VmrIssueTriageGhGet {
    param([Parameter(Mandatory)][string] $Resource)

    if ($Resource -match '[\\\r\n#]' -or $Resource.Contains('..') -or
        $Resource -notmatch '^(repos/[A-Za-z0-9_.-]+/[A-Za-z0-9_.-]+/(issues|pulls|assignees)(/|\?|$)|orgs/dotnet/teams/source-build/members\?|search/issues\?)') {
        throw 'Unsupported read-only GitHub resource.'
    }

    if ($Resource.StartsWith('search/', [StringComparison]::Ordinal)) {
        $previous = Get-Variable -Name VmrIssueLastSearchUtc -Scope Script -ErrorAction SilentlyContinue
        if ($null -ne $previous) {
            $wait = 2100 - ([DateTimeOffset]::UtcNow - $previous.Value).TotalMilliseconds
            if ($wait -gt 0) { Start-Sleep -Milliseconds ([int][Math]::Ceiling($wait)) }
        }
        $script:VmrIssueLastSearchUtc = [DateTimeOffset]::UtcNow
    }

    $reply = [pscustomobject]@{
        success = $false; statusCode = 0; data = $null; hasNext = $false; message = ''
        observedAtUtc = [DateTimeOffset]::UtcNow.ToString('o')
    }
    try {
        # Capture native failures ourselves so HTTP 401/403/404 remain distinguishable.
        $ErrorActionPreference = 'Continue'
        $PSNativeCommandUseErrorActionPreference = $false
        $raw = @(& gh api --hostname github.com --method GET --include `
            -H 'Accept: application/vnd.github+json' -H 'X-GitHub-Api-Version: 2022-11-28' $Resource 2>&1)
        $exitCode = $LASTEXITCODE
        $ErrorActionPreference = 'Stop'
        $text = ($raw | ForEach-Object { $_.ToString() }) -join "`n"
        $header = [regex]::Match($text, '(?s)^HTTP/\S+\s+(?<status>\d{3})[^\r\n]*\r?\n(?<headers>.*?)\r?\n\r?\n(?<body>.*)$')
        if (-not $header.Success) {
            $header = [regex]::Match($text, '(?s)^HTTP/\S+\s+(?<status>\d{3})[^\r\n]*(?:\r?\n(?<headers>.*))?$')
        }
        if ($header.Success) { $reply.statusCode = [int]$header.Groups['status'].Value }
        if ($exitCode -ne 0 -or $reply.statusCode -notin @(200, 204)) {
            $failureText = if ($header.Groups['body'].Success) { $header.Groups['body'].Value } else { $text }
            $detail = Protect-VmrIssueTriageText $failureText
            if ($detail.Length -gt 800) { $detail = $detail.Substring(0, 800) }
            $reply.message = "GitHub GET failed (HTTP $($reply.statusCode), exit $exitCode). $detail"
            return $reply
        }
        $reply.hasNext = $header.Groups['headers'].Value -match '(?im)^link:.*;\s*rel="next"'
        if ($reply.statusCode -eq 200) {
            if (-not $header.Groups['body'].Success) { throw 'GitHub response has no JSON body.' }
            $reply.data = ConvertFrom-Json -InputObject $header.Groups['body'].Value -Depth 100 -NoEnumerate -ErrorAction Stop
            if ($null -eq $reply.data) { throw 'GitHub response is JSON null rather than an inventory or resource.' }
            $reply.data = Protect-VmrIssueTriageValue -Value $reply.data
        }
        $reply.success = $true
    }
    catch {
        $reply.message = 'GitHub transport or response failure: ' + (Protect-VmrIssueTriageText $_.Exception.Message)
    }
    return $reply
}

function New-VmrIssueTriageEvidence {
    <#
    .SYNOPSIS
    Collects bounded, read-only GitHub issue and potential-owner evidence.
    .DESCRIPTION
    Returns only the absolute issue-evidence.json path. Requires PowerShell 7 and gh.
    Dot-sourcing this file performs no collection. All GitHub requests are explicit GETs.
    The supplied directory may contain other collectors' output, but issue-evidence.json
    and issue-triage must not already exist. The compact index is published atomically;
    sidecars use the existing monitor JSON writer.

    schemaVersion 1 includes status, observedAtUtc, coverage, issues, fixPullRequests,
    assigneeCandidates and errors. Body, comment, reference and metadata paths are absolute.
    Comments files contain {complete, reportedCount, fetchedCount, pages, items}; items
    retain full API comment IDs, html_url, author and timestamps. Compact index excerpts
    are not a replacement for these files. Reference files retain every extracted reference.
    Credential-shaped text and credential fields are redacted in memory before persistence,
    including successful API responses; raw HTTP headers and credentials are never written.

    complete means this finite collection succeeded, not that no other tracker exists.
    Collection failures, issue/comment truncation and omitted required references produce
    partial plus resource/message errors. Historical reference enrichment uses only the
    remaining lookup budget; unselected optional references remain explicitly unresolved
    in coverage and the reference catalog without making the index partial.
    Required references are direct registry PR links, references in registry issue bodies
    or explicitly pinned comments, and references in the same body/comment as a supplied
    exact build ID. A search hit or current comment does not promote an entire old thread.
    Every actual reference fetch failure, including optional enrichment, still means partial.
    Index-only preview limits do not discard the corresponding local evidence.
    GitHub state is observed at collection time, NOT reconstructed at ObservationCutoffUtc.
    Exact numeric mentions, historical issues, referenced PRs and ownership discussions
    are evidence for the caller, not classifications of cause, fix rollout or defect ownership.
    .EXAMPLE
    New-VmrIssueTriageEvidence -OutputDirectory 'C:\runs\unique-run' `
        -TriageContextPath 'C:\runs\triage-context.json' `
        -BuildIds @(9000001, 9000002) -ObservationCutoffUtc ([DateTimeOffset]::UtcNow)
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][string] $OutputDirectory,
        [Parameter(Mandatory)][string] $TriageContextPath,
        [Parameter(Mandatory)][AllowEmptyCollection()][int[]] $BuildIds,
        [Parameter(Mandatory)][DateTimeOffset] $ObservationCutoffUtc,
        # Standing team context from the local roster (team handle -> contact, built once by ./common/Invoke-Contacts.ps1).
        # When it has @dotnet/source-build its contact is used and the team's maintainers are not queried again every run.
        [hashtable] $TeamRoster = @{}
    )

    $ErrorActionPreference = 'Stop'
    Set-StrictMode -Version Latest

    if (-not [IO.Path]::IsPathFullyQualified($OutputDirectory) -or
        -not [IO.Path]::IsPathFullyQualified($TriageContextPath)) {
        throw 'OutputDirectory and TriageContextPath must be absolute filesystem paths.'
    }
    if (@($BuildIds | Where-Object { $_ -le 0 }).Count -ne 0) { throw 'BuildIds must be positive integers.' }
    $output = [IO.Path]::GetFullPath($OutputDirectory)
    $indexPath = Join-Path $output 'issue-evidence.json'
    $dataDirectory = Join-Path $output 'issue-triage'
    if ((Test-Path -LiteralPath $indexPath) -or (Test-Path -LiteralPath "$indexPath.tmp") -or
        (Test-Path -LiteralPath $dataDirectory)) {
        throw 'Issue evidence already exists. Supply a unique run directory; existing evidence is never overwritten.'
    }
    [void][IO.Directory]::CreateDirectory($dataDirectory)

    $limits = [ordered]@{
        issuePageSize = 100; issuePagesPerState = 2; registryIssues = 40
        searchBuildIds = 64; searchPageSize = 25; searchPagesPerBuild = 1
        issuesWithComments = 120; commentPageSize = 100; commentPagesPerIssue = 3
        pinnedComments = 20; referenceLookups = 40; pullRequests = 30
        assigneeChecks = 24; teamPages = 2; totalRequests = 450
        previewCharacters = 240; commentSummariesPerIssue = 4; referencesPerIssue = 8
    }
    $state = [pscustomobject]@{
        errors = [Collections.Generic.List[object]]::new()
        errorKeys = [Collections.Generic.HashSet[string]]::new()
        requests = 0; authBlocked = $false
    }
    $issueMap = @{}
    $prReferences = @{}
    $ownerMap = @{}
    $filteredBots = [Collections.Generic.HashSet[string]]::new([StringComparer]::OrdinalIgnoreCase)
    $requestedIds = @($BuildIds | Select-Object -Unique)
    $registryReferences = [Collections.Generic.List[object]]::new()
    $issueIndex = [Collections.Generic.List[object]]::new()
    $prIndex = [Collections.Generic.List[object]]::new()
    $assigneeIndex = [Collections.Generic.List[object]]::new()
    $coverage = [ordered]@{
        startedAtUtc = [DateTimeOffset]::UtcNow.ToString('o')
        observationCutoffUtc = $ObservationCutoffUtc.ToUniversalTime().ToString('o')
        triageContextPath = [IO.Path]::GetFullPath($TriageContextPath)
        scope = 'Open AND closed dotnet/dotnet operational-label issues, registry links, and exact build-ID searches in dotnet/dotnet and registry issue repositories.'
        redaction = 'Known credential values, token-shaped text and credential fields are redacted in memory before any evidence persistence. Evidence otherwise retains the collected content.'
        limits = $limits
        interpretation = @(
            'Complete describes only the recorded finite scope, never exhaustive GitHub coverage.'
            'Unselected historical reference enrichment is optional and unresolved, not checked; required omissions and every actual fetch failure remain partial.'
            'No exact build-ID search hit is not evidence that no tracker or recurring signature exists.'
            'Numeric mentions and search hits do not establish an identical failure; historical closed issues remain recurrence candidates.'
            'Closed does not mean fixed on every branch. Referenced or merged PRs do not prove applicability, deployment or recovery.'
            'GitHub state and comments are observed during collection, potentially after the build cutoff; timestamps are retained.'
            'Individual candidates are selectable triage suggestions with a stated basis, not confirmed defect owners or on-call assignments.'
        )
        enumeration = [Collections.Generic.List[object]]::new()
        registry = [ordered]@{ status = 'pending'; issueLinks = 0; fetched = 0; skipped = 0 }
        buildSearches = [Collections.Generic.List[object]]::new()
        requestedBuildIds = $requestedIds
        unsearchedBuildIds = @()
        comments = [ordered]@{ issuesFetched = 0; issuesSkipped = 0; pinnedFetched = 0 }
        references = [ordered]@{
            status = 'pending'; observed = 0; lookups = 0; skipped = 0
            required = [ordered]@{ status = 'pending'; count = 0; selected = 0; resolved = 0; unresolved = 0; skipped = 0; failed = 0 }
            optional = [ordered]@{ status = 'pending'; count = 0; selected = 0; resolved = 0; unresolved = 0; skipped = 0; failed = 0 }
            path = (Join-Path $dataDirectory 'pull-request-references.json')
            scope = 'Required: registry PR links, registry issue bodies, pinned registry comments, and body/comment references co-located with a supplied exact build ID. Other observed references are optional enrichment within remaining limits. No recursive traversal, PR reviews, checks or deployment verification.'
            priority = 'All required references precede optional enrichment; explicit PR URLs precede shorthand type lookups within each group. Optional enrichment reserves the remaining team/assignee request budget.'
            interpretation = 'Per-issue references are observed links, not lookup results. The catalog records required reasons, selected/checked flags, lookup status and omission reasons. Optional unresolved coverage does not mean these links were checked.'
        }
        ownership = [ordered]@{
            team = '@dotnet/source-build'
            teamEvidenceUrl = 'https://api.github.com/orgs/dotnet/teams/source-build/members?role=maintainer'
            teamStatus = 'pending'; teamPath = (Join-Path $dataDirectory 'source-build-maintainers.json')
            evidencePath = (Join-Path $dataDirectory 'owner-evidence.json')
            filteredBots = @(); skippedCandidates = 0
        }
        requests = 0
    }

    function Add-TriageError {
        param([string] $Resource, [string] $Message)
        $safeResource = Protect-VmrIssueTriageText $Resource
        $safeMessage = Protect-VmrIssueTriageText $Message
        if ($safeMessage.Length -gt 1000) { $safeMessage = $safeMessage.Substring(0, 1000) }
        if ($state.errorKeys.Add("$safeResource`n$safeMessage")) {
            $state.errors.Add([pscustomobject]@{ resource = $safeResource; message = $safeMessage })
        }
    }

    function Get-TriageResource {
        param([string] $Resource, [switch] $AllowMissing)
        if ($state.authBlocked) {
            Add-TriageError $Resource 'Not fetched after GitHub HTTP 401. Authentication is unavailable; this resource is unknown.'
            return $null
        }
        if ($state.requests -ge $limits.totalRequests) {
            Add-TriageError $Resource "Not fetched: the $($limits.totalRequests)-request collection limit was reached."
            return $null
        }
        $state.requests++
        $response = Invoke-VmrIssueTriageGhGet -Resource $Resource
        if (-not $response.success) {
            # A definitive 404 on an OPTIONAL shorthand reference is a resolved fact - that
            # "#N" is not a GitHub issue in this repository - not an inability to collect.
            # Bodies of aggregate trackers routinely contain non-GitHub "#<number>" tokens.
            # Required resources still record a real error and keep the run partial.
            if ($AllowMissing -and $response.statusCode -eq 404) {
                return [pscustomobject]@{ success = $false; notFound = $true; statusCode = 404 }
            }
            Add-TriageError $Resource $response.message
            if ($response.statusCode -eq 401) { $state.authBlocked = $true }
            return $null
        }
        return $response
    }

    function Get-TriageOptional {
        param([AllowNull()][object] $Value, [string] $Name)
        if ($null -ne $Value -and $null -ne $Value.PSObject.Properties[$Name]) {
            return $Value.PSObject.Properties[$Name].Value
        }
        return $null
    }

    function Assert-TriageFields {
        param([object] $Value, [string[]] $Names)
        foreach ($name in $Names) {
            if ($null -eq $Value -or $null -eq $Value.PSObject.Properties[$name]) {
                throw "GitHub response is missing required field '$name'."
            }
        }
    }

    function Get-TriagePreview {
        param([AllowNull()][AllowEmptyString()][string] $Text)
        $preview = [regex]::Replace((Protect-VmrIssueTriageText $Text), '\s+', ' ').Trim()
        if ($preview.Length -gt $limits.previewCharacters) {
            return $preview.Substring(0, $limits.previewCharacters) + '...'
        }
        return $preview
    }

    function Get-TriageGithubLinks {
        param([AllowEmptyString()][string] $Text, [string] $Repository, [string] $SourceUrl, [switch] $IncludeShort)
        $links = @{}
        foreach ($match in [regex]::Matches($Text,
                '(?i)https://github\.com/(?<repo>[a-z0-9-]+/[a-z0-9_.-]+)/(?<kind>issues|pull)/(?<number>[1-9]\d*)(?:#issuecomment-(?<comment>\d+))?')) {
            $repo = $match.Groups['repo'].Value
            $number = [long]$match.Groups['number'].Value
            $kind = if ($match.Groups['kind'].Value -eq 'pull') { 'pull' } else { 'issue' }
            $commentId = if ($match.Groups['comment'].Success) { [long]$match.Groups['comment'].Value } else { $null }
            $key = "$repo/$kind/$number/$commentId"
            $links[$key] = [pscustomobject]@{
                repository = $repo; number = $number; kind = $kind
                url = $match.Value; commentId = $commentId; sourceUrl = $SourceUrl
            }
        }
        if ($IncludeShort) {
            # A cross-repository link's "#123" label is not a second, local issue reference.
            $withoutLinks = [regex]::Replace($Text, '!?\[[^\]\r\n]*\]\(\s*<?https?://[^)\r\n]+\)', ' ')
            $withoutUrls = [regex]::Replace($withoutLinks, 'https?://[^\s<>]+', ' ')
            foreach ($match in [regex]::Matches($withoutUrls,
                    '(?<![\w/#])(?:(?<repo>[A-Za-z0-9-]+/[A-Za-z0-9_.-]+))?#(?<number>[1-9]\d*)\b')) {
                $repo = if ($match.Groups['repo'].Success) { $match.Groups['repo'].Value } else { $Repository }
                if (-not $repo) { continue }
                $number = [long]$match.Groups['number'].Value
                $key = "$repo/reference/$number"
                $links[$key] = [pscustomobject]@{
                    repository = $repo; number = $number; kind = 'reference'
                    url = "https://github.com/$repo/issues/$number"; commentId = $null; sourceUrl = $SourceUrl
                }
            }
        }
        return @($links.Values | Sort-Object repository, number, kind)
    }

    function Add-TriagePrReference {
        param([object] $Reference, [int] $Priority = 3, [string[]] $RequiredReasons = @())
        if ($Reference.kind -eq 'issue') { return }
        $key = "$($Reference.repository)#$($Reference.number)"
        if (-not $prReferences.ContainsKey($key)) {
            $prReferences[$key] = [pscustomobject]@{
                repository = $Reference.repository; number = $Reference.number; kind = $Reference.kind; priority = $Priority
                sources = [Collections.Generic.HashSet[string]]::new([StringComparer]::OrdinalIgnoreCase)
                required = $false
                requiredReasons = [Collections.Generic.HashSet[string]]::new()
                requiredEvidenceUrls = [Collections.Generic.HashSet[string]]::new([StringComparer]::OrdinalIgnoreCase)
                selected = $false; checked = $false; lookupStatus = 'pending'; notSelectedReason = $null; metadataPath = $null
            }
        }
        if ($Reference.kind -eq 'pull') { $prReferences[$key].kind = 'pull' }
        $prReferences[$key].priority = [math]::Min($prReferences[$key].priority, $Priority)
        [void]$prReferences[$key].sources.Add($Reference.sourceUrl)
        if ($RequiredReasons.Count -gt 0) {
            $prReferences[$key].required = $true
            foreach ($reason in $RequiredReasons) { [void]$prReferences[$key].requiredReasons.Add($reason) }
            [void]$prReferences[$key].requiredEvidenceUrls.Add($Reference.sourceUrl)
        }
    }

    function Get-TriageBuildReferences {
        param([AllowEmptyString()][string] $Text, [string] $SourceUrl)
        $references = [Collections.Generic.List[object]]::new()
        $seen = [Collections.Generic.HashSet[int]]::new()
        foreach ($match in [regex]::Matches($Text,
                '(?i)https://(?:dev\.azure\.com|[a-z0-9.-]+\.visualstudio\.com)/[^\s<>"''\]\)]+')) {
            $url = $match.Value.Replace('&amp;', '&')
            $idMatch = [regex]::Match($url, '(?i)[?&]buildId=(?<id>[1-9]\d*)(?:&|$)')
            $id = 0
            if ($idMatch.Success -and [int]::TryParse($idMatch.Groups['id'].Value, [ref]$id) -and $seen.Add($id)) {
                $references.Add([pscustomobject]@{
                    buildId = $id; requested = $id -in $requestedIds; kind = 'build-url'
                    sourceUrl = $SourceUrl; buildUrl = (Protect-VmrIssueTriageText $url)
                })
            }
        }
        foreach ($id in $requestedIds) {
            if (-not $seen.Contains($id) -and $Text -match "(?<!\d)$id(?!\d)") {
                [void]$seen.Add($id)
                $references.Add([pscustomobject]@{
                    buildId = $id; requested = $true; kind = 'exact-id-text'
                    sourceUrl = $SourceUrl; buildUrl = $null
                })
            }
        }
        return $references.ToArray()
    }

    function Add-TriageOwner {
        param([AllowNull()][object] $User, [string] $Basis, [string] $Url, [string] $Context, [int] $Rank)
        if ($null -eq $User) { return }
        try { Assert-TriageFields $User @('login', 'type') }
        catch {
            Add-TriageError $Url $_.Exception.Message
            return
        }
        $login = [string]$User.login
        if ($User.type -eq 'Bot' -or $login -match '(?i)(\[bot\]$|(?:^|[-_])bot(?:$|[-_])|^dependabot$|^github-actions$)') {
            [void]$filteredBots.Add($login)
            return
        }
        if ($User.type -ne 'User' -or $login -notmatch '^[A-Za-z0-9][A-Za-z0-9-]*$') {
            Add-TriageError $Url 'Potential owner has an unsupported account type or login; no individual was confirmed.'
            return
        }
        if (-not $ownerMap.ContainsKey($login)) {
            $ownerMap[$login] = [pscustomobject]@{
                login = $login; rank = $Rank; evidence = [Collections.Generic.List[object]]::new()
            }
        }
        $ownerMap[$login].rank = [math]::Min($ownerMap[$login].rank, $Rank)
        $ownerMap[$login].evidence.Add([pscustomobject]@{
            basis = $Basis; evidenceUrl = $Url; context = (Protect-VmrIssueTriageText $Context); rank = $Rank
        })
    }

    function Add-TriageIssue {
        param([object] $Data, [object] $Source, [string] $ObservedAtUtc, [switch] $SearchOnly)
        Assert-TriageFields $Data @('number', 'title', 'state', 'html_url', 'body', 'assignees', 'comments', 'updated_at')
        if ($null -ne (Get-TriageOptional $Data 'pull_request')) { return }
        $identity = [regex]::Match([string]$Data.html_url,
            '^https://github\.com/(?<repo>[A-Za-z0-9-]+/[A-Za-z0-9_.-]+)/issues/(?<number>[1-9]\d*)$')
        if (-not $identity.Success -or [long]$identity.Groups['number'].Value -ne $Data.number -or
            $Data.state -notin @('open', 'closed') -or $Data.comments -lt 0 -or $Data.assignees -isnot [array]) {
            throw 'Malformed GitHub issue identity, state, comment count or assignees.'
        }
        $key = "$($identity.Groups['repo'].Value)#$($Data.number)"
        if (-not $issueMap.ContainsKey($key)) {
            $issueMap[$key] = [pscustomobject]@{
                repository = $identity.Groups['repo'].Value; data = $Data; observedAtUtc = $ObservedAtUtc
                searchOnly = [bool]$SearchOnly; sources = [Collections.Generic.List[object]]::new()
            }
        }
        elseif (-not $SearchOnly) {
            $issueMap[$key].data = $Data
            $issueMap[$key].observedAtUtc = $ObservedAtUtc
            $issueMap[$key].searchOnly = $false
        }
        $issueMap[$key].sources.Add($Source)
    }

    try {
        try {
            $registry = Get-Content -LiteralPath $TriageContextPath -Raw -ErrorAction Stop |
                ConvertFrom-Json -Depth 50 -ErrorAction Stop
            $registry = Protect-VmrIssueTriageValue -Value $registry
            Assert-TriageFields $registry @('schemaVersion', 'entries')
            if ($registry.schemaVersion -ne 1 -or $registry.entries -isnot [array]) {
                throw 'Unsupported triage registry schema; expected schemaVersion 1 and an entries array.'
            }
            foreach ($entry in $registry.entries) {
                Assert-TriageFields $entry @('id')
                foreach ($reference in @(Get-TriageGithubLinks -Text ($entry | ConvertTo-Json -Depth 50) -SourceUrl "registry:$($entry.id)")) {
                    $registryReferences.Add([pscustomobject]@{ entryId = $entry.id; reference = $reference })
                    Add-TriagePrReference $reference -Priority 0 -RequiredReasons @('registry-link')
                }
            }
            $coverage.registry.status = 'complete'
        }
        catch {
            $coverage.registry.status = 'unavailable'
            Add-TriageError $TriageContextPath $_.Exception.Message
        }

        foreach ($issueState in @('open', 'closed')) {
            $inventory = [ordered]@{
                state = $issueState; label = 'area-unified-build-OperationalIssue'
                sort = 'updated desc'; pages = 0; issueCount = 0; status = 'complete'; truncated = $false
                path = (Join-Path $dataDirectory "operational-$issueState.json")
            }
            $items = [Collections.Generic.List[object]]::new()
            for ($page = 1; $page -le $limits.issuePagesPerState; $page++) {
                $resource = "repos/dotnet/dotnet/issues?state=$issueState&labels=area-unified-build-OperationalIssue&sort=updated&direction=desc&per_page=$($limits.issuePageSize)&page=$page"
                $response = Get-TriageResource $resource
                if ($null -eq $response) { $inventory.status = 'partial'; break }
                try {
                    if ($response.data -isnot [array]) { throw 'Issue enumeration returned a non-array response.' }
                    foreach ($item in $response.data) {
                        $items.Add($item)
                        Add-TriageIssue $item ([pscustomobject]@{ kind = 'operational-label'; state = $issueState }) $response.observedAtUtc
                        if ($null -eq (Get-TriageOptional $item 'pull_request')) { $inventory.issueCount++ }
                    }
                    $inventory.pages++
                }
                catch {
                    Add-TriageError $resource $_.Exception.Message
                    $inventory.status = 'partial'
                    break
                }
                if (-not $response.hasNext) { break }
                if ($page -eq $limits.issuePagesPerState) {
                    $inventory.status = 'partial'; $inventory.truncated = $true
                    Add-TriageError $resource "Operational $issueState enumeration truncated at $($limits.issuePagesPerState) pages; older issues may exist."
                }
            }
            Write-VmrJson -Path $inventory.path -Value @{ coverage = $inventory; items = $items.ToArray() }
            $coverage.enumeration.Add([pscustomobject]$inventory)
        }

        $knownIssues = @($registryReferences | Where-Object { $_.reference.kind -eq 'issue' } |
            Group-Object { "$($_.reference.repository)#$($_.reference.number)" })
        $coverage.registry.issueLinks = $knownIssues.Count
        foreach ($group in @($knownIssues | Select-Object -First $limits.registryIssues)) {
            $link = $group.Group[0].reference
            $resource = "repos/$($link.repository)/issues/$($link.number)"
            $response = Get-TriageResource $resource
            if ($null -eq $response) { $coverage.registry.status = 'partial'; continue }
            try {
                foreach ($source in $group.Group) {
                    Add-TriageIssue $response.data ([pscustomobject]@{
                        kind = 'registry'; entryId = $source.entryId; url = $source.reference.url
                    }) $response.observedAtUtc
                }
                $coverage.registry.fetched++
            }
            catch {
                $coverage.registry.status = 'partial'
                Add-TriageError $resource $_.Exception.Message
            }
        }
        if ($knownIssues.Count -gt $limits.registryIssues) {
            $coverage.registry.skipped = $knownIssues.Count - $limits.registryIssues
            $coverage.registry.status = 'partial'
            Add-TriageError 'registry-issues' "$($coverage.registry.skipped) registry issues were not fetched because of the registry limit."
        }

        $searchRepos = @(@('dotnet/dotnet') + @($knownIssues | ForEach-Object { $_.Group[0].reference.repository }) |
            Sort-Object -Unique)
        $searchScope = ($searchRepos | ForEach-Object { "repo:$_" }) -join ' '
        $coverage.unsearchedBuildIds = @($requestedIds | Select-Object -Skip $limits.searchBuildIds)
        if ($coverage.unsearchedBuildIds.Count -gt 0) {
            Add-TriageError 'build-id-search' "$($coverage.unsearchedBuildIds.Count) supplied build IDs exceeded the exact-search limit. Local matching still uses every supplied ID."
        }
        foreach ($buildId in @($requestedIds | Select-Object -First $limits.searchBuildIds)) {
            $query = "$searchScope is:issue in:title,body,comments $buildId"
            $search = [ordered]@{
                buildId = $buildId; query = $query; status = 'complete'; totalCount = $null
                resultCount = 0; truncated = $false; incompleteResults = $false
                path = (Join-Path $dataDirectory "build-search-$buildId.json")
            }
            $resource = "search/issues?q=$([Uri]::EscapeDataString($query))&sort=updated&order=desc&per_page=$($limits.searchPageSize)&page=1"
            $response = Get-TriageResource $resource
            if ($null -eq $response) { $search.status = 'unavailable' }
            else {
                try {
                    Assert-TriageFields $response.data @('total_count', 'incomplete_results', 'items')
                    if ($response.data.items -isnot [array]) { throw 'Search items is not an array.' }
                    $search.totalCount = $response.data.total_count
                    $search.resultCount = $response.data.items.Count
                    $search.incompleteResults = [bool]$response.data.incomplete_results
                    $search.truncated = $response.hasNext -or $search.totalCount -gt $search.resultCount
                    foreach ($item in $response.data.items) {
                        Add-TriageIssue $item ([pscustomobject]@{ kind = 'build-id-search'; buildId = $buildId }) $response.observedAtUtc -SearchOnly
                    }
                    if ($search.truncated -or $search.incompleteResults) {
                        $search.status = 'partial'
                        Add-TriageError $resource 'Exact build-ID search was truncated or GitHub reported incomplete_results; a missing hit is inconclusive.'
                    }
                    Write-VmrJson -Path $search.path -Value $response.data
                }
                catch {
                    $search.status = 'partial'
                    Add-TriageError $resource $_.Exception.Message
                }
            }
            if (-not (Test-Path -LiteralPath $search.path)) { $search.path = $null }
            $coverage.buildSearches.Add([pscustomobject]$search)
        }

        # Registry and search hits get comments first; alternate open/closed historical candidates.
        $prioritized = @($issueMap.Values | Where-Object {
            @($_.sources | Where-Object { $_.kind -in @('registry', 'build-id-search') }).Count -gt 0
        } | Sort-Object repository, { $_.data.number })
        $remaining = @($issueMap.Values | Where-Object { $_ -notin $prioritized })
        $openIssues = @($remaining | Where-Object { $_.data.state -eq 'open' } | Sort-Object { $_.data.updated_at } -Descending)
        $closedIssues = @($remaining | Where-Object { $_.data.state -eq 'closed' } | Sort-Object { $_.data.updated_at } -Descending)
        $orderedIssues = [Collections.Generic.List[object]]::new()
        foreach ($candidate in $prioritized) { $orderedIssues.Add($candidate) }
        for ($i = 0; $i -lt [math]::Max($openIssues.Count, $closedIssues.Count); $i++) {
            if ($i -lt $openIssues.Count) { $orderedIssues.Add($openIssues[$i]) }
            if ($i -lt $closedIssues.Count) { $orderedIssues.Add($closedIssues[$i]) }
        }

        foreach ($candidate in $orderedIssues) {
            $repo = $candidate.repository
            $issue = $candidate.data
            $resource = "repos/$repo/issues/$($issue.number)"
            try {
                if ($candidate.searchOnly) {
                    $fresh = Get-TriageResource $resource
                    if ($null -ne $fresh) {
                        Add-TriageIssue $fresh.data ([pscustomobject]@{ kind = 'search-hit-hydration' }) $fresh.observedAtUtc
                        $issue = $candidate.data
                    }
                }
                $stem = $repo.Replace('/', '-') + "-$($issue.number)"
                $bodyPath = Join-Path $dataDirectory "$stem.body.md"
                $commentsPath = Join-Path $dataDirectory "$stem.comments.json"
                $metadataPath = Join-Path $dataDirectory "$stem.issue.json"
                $referencesPath = Join-Path $dataDirectory "$stem.references.json"
                Set-Content -LiteralPath $bodyPath -Value ([string]$issue.body) -Encoding utf8 -NoNewline
                Write-VmrJson -Path $metadataPath -Value $issue
                $commentMap = @{}
                $commentCoverage = [ordered]@{
                    reportedCount = [int]$issue.comments; fetchedCount = 0
                    status = 'complete'; complete = -not $candidate.searchOnly; truncated = $false
                    pages = @(); pinnedIds = @(); selection = 'Newest pages first, plus explicitly linked registry comments.'
                }
                if ($candidate.searchOnly) { $commentCoverage.status = 'partial' }
                $pageCount = [int][math]::Ceiling($issue.comments / [double]$limits.commentPageSize)
                if ($pageCount -gt 0) {
                    if ($coverage.comments.issuesFetched -ge $limits.issuesWithComments) {
                        $coverage.comments.issuesSkipped++
                        $commentCoverage.status = 'skipped'; $commentCoverage.complete = $false; $commentCoverage.truncated = $true
                        Add-TriageError "$resource/comments" 'Comments were not fetched because the per-run issue-comment budget was reached.'
                    }
                    else {
                        $coverage.comments.issuesFetched++
                        $firstPage = [math]::Max(1, $pageCount - $limits.commentPagesPerIssue + 1)
                        if ($firstPage -gt 1) {
                            $commentCoverage.complete = $false; $commentCoverage.truncated = $true; $commentCoverage.status = 'partial'
                            Add-TriageError "$resource/comments" "Comments truncated: only the newest $($limits.commentPagesPerIssue) pages of $pageCount were selected."
                        }
                        for ($page = $pageCount; $page -ge $firstPage; $page--) {
                            $commentResource = "$resource/comments?per_page=$($limits.commentPageSize)&page=$page"
                            $response = Get-TriageResource $commentResource
                            if ($null -eq $response) { $commentCoverage.complete = $false; $commentCoverage.status = 'partial'; continue }
                            try {
                                if ($response.data -isnot [array]) { throw 'Comments response is not an array.' }
                                foreach ($comment in $response.data) {
                                    Assert-TriageFields $comment @('id', 'html_url', 'body', 'user', 'created_at', 'updated_at')
                                    if ($comment.html_url -ne "$($issue.html_url)#issuecomment-$($comment.id)") {
                                        throw 'Comment identity does not belong to the requested issue.'
                                    }
                                    $commentMap[[string]$comment.id] = $comment
                                }
                                $commentCoverage.pages += $page
                                if ($page -eq $pageCount -and $response.hasNext) {
                                    throw 'Comments changed during collection: the reported last page now has a next page.'
                                }
                            }
                            catch {
                                $commentCoverage.complete = $false; $commentCoverage.status = 'partial'
                                Add-TriageError $commentResource $_.Exception.Message
                            }
                        }
                    }
                }
                $pinned = @($registryReferences | Where-Object {
                    $_.reference.repository -eq $repo -and $_.reference.number -eq $issue.number -and $null -ne $_.reference.commentId
                } | ForEach-Object { $_.reference.commentId } | Select-Object -Unique)
                foreach ($commentId in $pinned) {
                    if ($commentMap.ContainsKey([string]$commentId)) { continue }
                    $commentResource = "repos/$repo/issues/comments/$commentId"
                    if ($coverage.comments.pinnedFetched -ge $limits.pinnedComments) {
                        Add-TriageError $commentResource 'Explicitly linked comment was not fetched because the pinned-comment limit was reached.'
                        $commentCoverage.complete = $false; $commentCoverage.status = 'partial'
                        continue
                    }
                    $coverage.comments.pinnedFetched++
                    $response = Get-TriageResource $commentResource
                    if ($null -eq $response) { $commentCoverage.complete = $false; $commentCoverage.status = 'partial'; continue }
                    try {
                        Assert-TriageFields $response.data @('id', 'html_url', 'body', 'user', 'created_at', 'updated_at')
                        if ($response.data.id -ne $commentId -or $response.data.html_url -ne "$($issue.html_url)#issuecomment-$commentId") {
                            throw 'Pinned comment does not belong to the registry issue.'
                        }
                        $commentMap[[string]$commentId] = $response.data
                        $commentCoverage.pinnedIds += $commentId
                    }
                    catch {
                        $commentCoverage.complete = $false; $commentCoverage.status = 'partial'
                        Add-TriageError $commentResource $_.Exception.Message
                    }
                }
                $comments = @($commentMap.Values | Sort-Object id)
                $commentCoverage.fetchedCount = $comments.Count
                if ($comments.Count -ne $issue.comments -and $commentCoverage.complete) {
                    $commentCoverage.complete = $false; $commentCoverage.status = 'partial'
                    Add-TriageError "$resource/comments" "Comment count changed or a page was incomplete: expected $($issue.comments), retained $($comments.Count)."
                }
                Write-VmrJson -Path $commentsPath -Value @{
                    repository = $repo; number = $issue.number; observedAtUtc = [DateTimeOffset]::UtcNow.ToString('o')
                    complete = $commentCoverage.complete; reportedCount = $issue.comments; fetchedCount = $comments.Count
                    pages = $commentCoverage.pages; coverage = $commentCoverage; items = $comments
                }

                $buildReferences = [Collections.Generic.List[object]]::new()
                $pullReferences = [Collections.Generic.List[object]]::new()
                $summaries = [Collections.Generic.List[object]]::new()
                $referencePriority = if (@($candidate.sources | Where-Object { $_.kind -in @('registry', 'build-id-search') }).Count -gt 0) { 1 }
                    elseif ($issue.state -eq 'open') { 2 } else { 3 }
                $bodyBuilds = @(Get-TriageBuildReferences "$($issue.title)`n$($issue.body)" $issue.html_url)
                $bodyReasons = @(
                    if (@($candidate.sources | Where-Object kind -eq 'registry').Count -gt 0) { 'registry-issue-body' }
                    if (@($bodyBuilds | Where-Object requested).Count -gt 0) { 'current-build-body' }
                )
                foreach ($reference in $bodyBuilds) { $buildReferences.Add($reference) }
                foreach ($reference in @(Get-TriageGithubLinks -Text ([string]$issue.body) -Repository $repo -SourceUrl $issue.html_url -IncludeShort)) {
                    if ($reference.kind -ne 'issue') {
                        $pullReferences.Add($reference)
                        Add-TriagePrReference $reference -Priority $referencePriority -RequiredReasons $bodyReasons
                    }
                }
                foreach ($assignee in $issue.assignees) {
                    Add-TriageOwner $assignee 'current-issue-assignee' $issue.html_url `
                        "Current assignee of $repo#$($issue.number) (state: $($issue.state)). Candidate for that tracker's triage; not a verified owner of today's failure." 0
                }
                foreach ($comment in $comments) {
                    $commentBuilds = @(Get-TriageBuildReferences ([string]$comment.body) $comment.html_url)
                    $commentReasons = @(
                        if ($comment.id -in $pinned) { 'registry-pinned-comment' }
                        if (@($commentBuilds | Where-Object requested).Count -gt 0) { 'current-build-comment' }
                    )
                    foreach ($reference in $commentBuilds) { $buildReferences.Add($reference) }
                    foreach ($reference in @(Get-TriageGithubLinks -Text ([string]$comment.body) -Repository $repo -SourceUrl $comment.html_url -IncludeShort)) {
                        if ($reference.kind -ne 'issue') {
                            $pullReferences.Add($reference)
                            Add-TriagePrReference $reference -Priority $referencePriority -RequiredReasons $commentReasons
                        }
                    }
                    $discussion = [regex]::Match([string]$comment.body,
                        '(?im)^(?![ \t]*>)[^\r\n]*(?:\bI(?:[''\u2019]m| am|[''\u2019]ll| will|[''\u2019]ve| have)?\s+(?:working on|porting|backporting|fixing|investigating|have a fix)|(?:^|[,.;]\s*)\s*(?:porting|backporting)\s+(?:the|this|that|a)\s+(?:PR|fix|patch))\b[^\r\n]*')
                    if ($discussion.Success) {
                        Add-TriageOwner $comment.user 'prior-ownership-discussion' $comment.html_url `
                            "Author discussed fix/triage work in $repo#$($issue.number): $(Get-TriagePreview $discussion.Value) Historical or related work only; current defect ownership and applicability are unverified." 1
                    }
                    $summaries.Add([pscustomobject]@{
                        id = $comment.id; url = $comment.html_url
                        author = (Get-TriageOptional $comment.user 'login')
                        createdAtUtc = $comment.created_at; updatedAtUtc = $comment.updated_at
                        excerpt = (Get-TriagePreview $comment.body)
                        matchedBuildIds = @($commentBuilds | Where-Object requested | Select-Object -ExpandProperty buildId -Unique)
                        ownershipDiscussion = $discussion.Success
                    })
                }
                Write-VmrJson -Path $referencesPath -Value @{
                    buildReferences = $buildReferences.ToArray(); pullRequestReferences = $pullReferences.ToArray()
                    commentSummaries = $summaries.ToArray()
                }
                $shortBuilds = @($buildReferences | Sort-Object @{ Expression = 'requested'; Descending = $true }, buildId |
                    Select-Object -First $limits.referencesPerIssue)
                $shortComments = @($summaries | Sort-Object `
                    @{ Expression = { $_.matchedBuildIds.Count -gt 0 }; Descending = $true },
                    @{ Expression = 'ownershipDiscussion'; Descending = $true },
                    @{ Expression = 'id'; Descending = $true } | Select-Object -First $limits.commentSummariesPerIssue)
                $issueIndex.Add([pscustomobject]@{
                    repository = $repo; number = $issue.number; title = (Protect-VmrIssueTriageText $issue.title)
                    state = $issue.state; url = $issue.html_url
                    author = (Get-TriageOptional (Get-TriageOptional $issue 'user') 'login')
                    assignees = @($issue.assignees | ForEach-Object { $_.login })
                    observedAtUtc = $candidate.observedAtUtc
                    metadataStatus = if ($candidate.searchOnly) { 'search-only' } else { 'available' }
                    bodyPath = $bodyPath; commentsPath = $commentsPath; commentsComplete = $commentCoverage.complete
                    metadataPath = $metadataPath; referencesPath = $referencesPath
                    bodyPreview = (Get-TriagePreview $issue.body); sources = $candidate.sources.ToArray()
                    matchedBuildIds = @($buildReferences | Where-Object requested | Select-Object -ExpandProperty buildId -Unique)
                    searchBuildIds = @($candidate.sources | Where-Object kind -eq 'build-id-search' | Select-Object -ExpandProperty buildId -Unique)
                    buildReferences = $shortBuilds
                    pullRequestReferences = @($pullReferences | Select-Object -First $limits.referencesPerIssue)
                    commentSummaries = $shortComments
                    commentsCoverage = [pscustomobject]@{
                        status = $commentCoverage.status; reportedCount = $commentCoverage.reportedCount
                        fetchedCount = $commentCoverage.fetchedCount; truncated = $commentCoverage.truncated
                        pages = $commentCoverage.pages; pinnedIds = $commentCoverage.pinnedIds
                    }
                    indexOnlyOmissions = @{
                        buildReferences = $buildReferences.Count - $shortBuilds.Count
                        pullRequestReferences = [math]::Max(0, $pullReferences.Count - $limits.referencesPerIssue)
                        commentSummaries = $summaries.Count - $shortComments.Count
                    }
                })
            }
            catch {
                Add-TriageError $resource $_.Exception.Message
            }
        }

        $coverage.references.observed = $prReferences.Count
        foreach ($reference in @($prReferences.Values | Sort-Object @{ Expression = 'required'; Descending = $true },
                @{ Expression = { $_.kind -eq 'pull' }; Descending = $true }, priority, repository, number)) {
            $key = "$($reference.repository)#$($reference.number)"
            if ($reference.kind -eq 'reference' -and $issueMap.ContainsKey($key) -and -not $issueMap[$key].searchOnly) {
                $reference.checked = $true; $reference.lookupStatus = 'resolved-issue'
                continue
            }
            $resource = "repos/$($reference.repository)/pulls/$($reference.number)"
            $lookupResource = if ($reference.kind -eq 'reference') { "repos/$($reference.repository)/issues/$($reference.number)" } else { $resource }
            $notSelectedReason = $null
            if ($coverage.references.lookups -ge $limits.referenceLookups -or $prIndex.Count -ge $limits.pullRequests) {
                $notSelectedReason = 'reference-or-pr-limit'
            }
            elseif (-not $reference.required) {
                $requestCost = if ($reference.kind -eq 'reference') { 2 } else { 1 }
                $remainingOwnerBudget = $limits.teamPages + $limits.assigneeChecks
                if ($state.authBlocked) { $notSelectedReason = 'authentication-unavailable' }
                elseif ($state.requests + $requestCost + $remainingOwnerBudget -gt $limits.totalRequests) {
                    $notSelectedReason = 'remaining-request-budget-reserved-for-ownership'
                }
            }
            if ($notSelectedReason) {
                $coverage.references.skipped++
                $reference.lookupStatus = 'not-selected'; $reference.notSelectedReason = $notSelectedReason
                if ($reference.required) {
                    Add-TriageError $lookupResource "Required reference metadata was not resolved ($notSelectedReason). The link is unchecked and applicability/deployment remain unknown."
                }
                continue
            }
            $reference.selected = $true
            $coverage.references.lookups++
            if ($reference.kind -eq 'reference') {
                # Shorthand "#<number>" is a guess: commit-message suffixes and AzDO record
                # numbers look identical to issue references. A definitive 404 proves this
                # number is not a GitHub issue/PR here - resolved evidence, not a fetch
                # failure. Explicit issue/pull URLs keep failing loudly below.
                $probeResource = "repos/$($reference.repository)/issues/$($reference.number)"
                $probe = Get-TriageResource $probeResource -AllowMissing
                if ($null -eq $probe) { $reference.lookupStatus = 'failed'; continue }
                if ($null -ne $probe.PSObject.Properties['notFound']) {
                    $reference.checked = $true; $reference.lookupStatus = 'not-found'
                    continue
                }
                try {
                    Assert-TriageFields $probe.data @('number', 'html_url', 'state')
                    if ($probe.data.number -ne $reference.number) { throw 'Reference lookup returned a different issue number.' }
                    if ($null -eq (Get-TriageOptional $probe.data 'pull_request')) {
                        if ($probe.data.html_url -ne "https://github.com/$($reference.repository)/issues/$($reference.number)") {
                            throw 'Reference lookup returned a different issue identity.'
                        }
                        $reference.checked = $true; $reference.lookupStatus = 'resolved-issue'
                        continue
                    }
                }
                catch {
                    $reference.lookupStatus = 'failed'
                    Add-TriageError $probeResource $_.Exception.Message
                    continue
                }
            }
            $response = Get-TriageResource $resource
            if ($null -eq $response) { $reference.lookupStatus = 'failed'; continue }
            try {
                $pr = $response.data
                Assert-TriageFields $pr @('number', 'title', 'state', 'html_url', 'body', 'merged', 'merged_at', 'base', 'head', 'user')
                Assert-TriageFields $pr.base @('ref')
                Assert-TriageFields $pr.head @('ref')
                if ($pr.number -ne $reference.number -or $pr.html_url -ne "https://github.com/$($reference.repository)/pull/$($reference.number)") {
                    throw 'Referenced PR identity does not match the requested repository and number.'
                }
                $stem = $reference.repository.Replace('/', '-') + "-pr-$($reference.number)"
                $metadataPath = Join-Path $dataDirectory "$stem.json"
                $bodyPath = Join-Path $dataDirectory "$stem.body.md"
                Write-VmrJson -Path $metadataPath -Value $pr
                Set-Content -LiteralPath $bodyPath -Value ([string]$pr.body) -Encoding utf8 -NoNewline
                $prIndex.Add([pscustomobject]@{
                    repository = $reference.repository; number = $pr.number; title = (Protect-VmrIssueTriageText $pr.title)
                    state = $pr.state; url = $pr.html_url; merged = $pr.merged; mergedAtUtc = $pr.merged_at
                    baseRefName = $pr.base.ref; headRefName = $pr.head.ref; author = (Get-TriageOptional $pr.user 'login')
                    observedAtUtc = $response.observedAtUtc; metadataPath = $metadataPath; bodyPath = $bodyPath
                    evidenceUrls = @($reference.sources | Sort-Object)
                    required = $reference.required; requiredReasons = @($reference.requiredReasons | Sort-Object)
                    relationship = 'Referenced candidate fix PR only; applicability, branch inclusion and deployment are unverified.'
                })
                $reference.checked = $true; $reference.lookupStatus = 'resolved-pull-request'; $reference.metadataPath = $metadataPath
            }
            catch {
                $reference.lookupStatus = 'failed'
                Add-TriageError $resource $_.Exception.Message
            }
        }
        foreach ($tier in @('required', 'optional')) {
            $references = @($prReferences.Values | Where-Object { $_.required -eq ($tier -eq 'required') })
            $summary = $coverage.references[$tier]
            $summary.count = $references.Count
            $summary.selected = @($references | Where-Object selected).Count
            $summary.resolved = @($references | Where-Object checked).Count
            $summary.unresolved = $summary.count - $summary.resolved
            $summary.skipped = @($references | Where-Object lookupStatus -eq 'not-selected').Count
            $summary.failed = @($references | Where-Object lookupStatus -eq 'failed').Count
            $summary.status = if ($summary.failed -gt 0 -or ($tier -eq 'required' -and $summary.unresolved -gt 0)) { 'partial' }
                elseif ($summary.unresolved -gt 0) { 'unresolved' } else { 'complete' }
        }
        $coverage.references.status = if ($coverage.references.required.status -eq 'partial' -or $coverage.references.optional.failed -gt 0) {
            'partial'
        } else { 'complete' }
        Write-VmrJson -Path $coverage.references.path -Value @{
            references = @($prReferences.Values | Sort-Object repository, number | ForEach-Object {
                @{
                    repository = $_.repository; number = $_.number; kind = $_.kind; priority = $_.priority
                    evidenceUrls = @($_.sources | Sort-Object)
                    required = $_.required; requiredReasons = @($_.requiredReasons | Sort-Object)
                    requiredEvidenceUrls = @($_.requiredEvidenceUrls | Sort-Object)
                    selected = $_.selected; checked = $_.checked; lookupStatus = $_.lookupStatus
                    notSelectedReason = $_.notSelectedReason; metadataPath = $_.metadataPath
                }
            })
        }

        $maintainers = [Collections.Generic.List[object]]::new()
        $coverage.ownership.teamStatus = 'complete'
        $rosterEntry = if ($TeamRoster.ContainsKey('@dotnet/source-build')) { $TeamRoster['@dotnet/source-build'] } else { $null }
        $rosterLogin = if ($null -ne $rosterEntry -and $null -ne $rosterEntry.PSObject.Properties['github']) { [string]$rosterEntry.github } else { '' }
        if ($rosterLogin) {
            # Standing team context is read from the local roster instead of asking GitHub for the team's maintainers every round.
            $rosterBasis = if ($null -ne $rosterEntry.PSObject.Properties['basis']) { [string]$rosterEntry.basis } else { 'roster entry' }
            $coverage.ownership.teamStatus = 'roster'
            Add-TriageOwner ([pscustomobject]@{ login = $rosterLogin; type = 'User' }) 'team-maintainer' 'https://github.com/orgs/dotnet/teams/source-build' `
                "Local team roster entry for @dotnet/source-build (basis: $rosterBasis). Initial triage suggestion only, not a proven code-defect owner or on-call assignment." 2
            $maintainers.Add([pscustomobject]@{ login = $rosterLogin; source = 'team-roster' })
        }
        else {
        for ($page = 1; $page -le $limits.teamPages; $page++) {
            $resource = "orgs/dotnet/teams/source-build/members?role=maintainer&per_page=100&page=$page"
            $response = Get-TriageResource $resource
            if ($null -eq $response) { $coverage.ownership.teamStatus = 'unavailable'; break }
            try {
                if ($response.data -isnot [array]) { throw 'Team maintainer response is not an array.' }
                foreach ($user in $response.data) {
                    $maintainers.Add($user)
                    Add-TriageOwner $user 'team-maintainer' $coverage.ownership.teamEvidenceUrl `
                        'Public source-build team maintainer metadata (role=maintainer). Initial @dotnet/source-build triage suggestion only, not a proven code-defect owner or on-call assignment.' 2
                }
            }
            catch {
                $coverage.ownership.teamStatus = 'partial'
                Add-TriageError $resource $_.Exception.Message
                break
            }
            if (-not $response.hasNext) { break }
            if ($page -eq $limits.teamPages) {
                $coverage.ownership.teamStatus = 'partial'
                Add-TriageError $resource 'Team maintainer enumeration was truncated at the recorded page limit.'
            }
        }
        }
        Write-VmrJson -Path $coverage.ownership.teamPath -Value @{
            team = '@dotnet/source-build'; status = $coverage.ownership.teamStatus; items = $maintainers.ToArray()
        }
        Write-VmrJson -Path $coverage.ownership.evidencePath -Value @{ candidates = @($ownerMap.Values | Sort-Object rank, login) }
        $owners = @($ownerMap.Values | Sort-Object rank, login)
        $coverage.ownership.skippedCandidates = [math]::Max(0, $owners.Count - $limits.assigneeChecks)
        if ($coverage.ownership.skippedCandidates -gt 0) {
            Add-TriageError 'assignee-checks' "$($coverage.ownership.skippedCandidates) potential individuals were not checked because of the assignee limit; their evidence remains local."
        }
        foreach ($owner in @($owners | Select-Object -First $limits.assigneeChecks)) {
            $resource = "repos/dotnet/dotnet/assignees/$($owner.login)"
            $response = Get-TriageResource $resource
            $assignable = $null -ne $response -and $response.statusCode -eq 204
            if ($null -ne $response -and -not $assignable) {
                Add-TriageError $resource 'Assignee endpoint did not return HTTP 204; selectability is not confirmed.'
            }
            $basis = @($owner.evidence | Sort-Object rank)[0]
            $assigneeIndex.Add([pscustomobject]@{
                login = $owner.login; assignable = [bool]$assignable
                evidenceUrl = $basis.evidenceUrl; context = $basis.context; basis = $basis.basis
                status = if ($assignable) { 'selectable-triage-candidate' } else { 'not-confirmed-selectable' }
                evidencePath = $coverage.ownership.evidencePath
                supportingEvidence = @($owner.evidence | Group-Object basis | ForEach-Object { $_.Group[0] } |
                    Sort-Object rank | Select-Object -First 3 basis, evidenceUrl, context)
            })
        }
    }
    catch {
        Add-TriageError 'issue-evidence-collection' $_.Exception.Message
    }

    $coverage.requests = $state.requests
    $coverage.ownership.filteredBots = @($filteredBots | Sort-Object)
    $index = [ordered]@{
        schemaVersion = 1
        status = if ($state.errors.Count -eq 0) { 'complete' } else { 'partial' }
        observedAtUtc = [DateTimeOffset]::UtcNow.ToString('o')
        coverage = $coverage
        issues = @($issueIndex | Sort-Object repository, number)
        fixPullRequests = $prIndex.ToArray()
        assigneeCandidates = $assigneeIndex.ToArray()
        errors = $state.errors.ToArray()
    }
    try {
        ConvertTo-Json -InputObject $index -Depth 20 -Compress |
            Set-Content -LiteralPath "$indexPath.tmp" -Encoding utf8 -NoNewline
        Move-Item -LiteralPath "$indexPath.tmp" -Destination $indexPath
    }
    catch {
        throw ('Could not persist issue-evidence.json: ' + (Protect-VmrIssueTriageText $_.Exception.Message))
    }
    return $indexPath
}

# Shared helper (was a separate file).
function Write-VmrJson {
    param(
        [Parameter(Mandatory)][string] $Path,
        [Parameter(Mandatory)][object] $Value
    )

    $temporaryPath = "$Path.tmp"
    $Value | ConvertTo-Json -Depth 10 | Set-Content -LiteralPath $temporaryPath -Encoding utf8
    Move-Item -LiteralPath $temporaryPath -Destination $Path -Force
}
