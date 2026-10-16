[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot '..\common\core.ps1')
. (Join-Path $PSScriptRoot '..\history\history.ps1')
. (Join-Path $PSScriptRoot '..\daily\report.ps1')

$failures = 0
function Check([string] $Name, [scriptblock] $Body) {
    try { & $Body; Write-Host "PASS: $Name" -ForegroundColor Green }
    catch { $script:failures++; Write-Host "FAIL: $Name -> $($_.Exception.Message)" -ForegroundColor Red }
}
function New-TempDir {
    $path = Join-Path ([IO.Path]::GetTempPath()) ('vmrqb-pkg-' + [guid]::NewGuid().ToString('N'))
    [void][IO.Directory]::CreateDirectory($path)
    return $path
}

Check 'Email validation accepts real addresses and rejects blanks and typos' {
    foreach ($good in @('qb@example.com', 'first.last@example.co.uk')) {
        if (-not (Test-VmrQBEmailAddress $good)) { throw "rejected a valid address: $good" }
    }
    foreach ($bad in @('', '   ', 'not-an-email', 'missing@domain', 'two@@at.com', $null)) {
        if (Test-VmrQBEmailAddress $bad) { throw "accepted an invalid address: '$bad'" }
    }
}

Check 'A missing config is not an error: built-in defaults, no recipient, nothing written' {
    $dir = New-TempDir
    try {
        $missing = Join-Path $dir 'qb-config.local.json'
        $config = Get-VmrQBConfig -Path $missing
        if ($null -eq $config) { throw 'a missing config must return the defaults' }
        if ([string](Get-VmrQBField $config 'recipient' '')) { throw 'defaults must not invent a recipient: no recipient means no email' }
        if (Test-Path -LiteralPath $missing) { throw 'reading the config must never create it' }
    }
    finally { Remove-Item $dir -Recurse -Force -ErrorAction SilentlyContinue }
}

Check 'An empty recipient is valid (no email); a recipient that is present must be an address' {
    $dir = New-TempDir
    try {
        $path = Join-Path $dir 'qb-config.local.json'
        '{ "recipient": "" }' | Set-Content -LiteralPath $path -Encoding utf8
        $config = Get-VmrQBConfig -Path $path
        if ([string]$config.recipient) { throw 'an empty recipient must stay empty' }
        '{ "recipient": "next.qb@example.com" }' | Set-Content -LiteralPath $path -Encoding utf8
        $config = Get-VmrQBConfig -Path $path
        if ($config.recipient -ne 'next.qb@example.com') { throw "a valid recipient must be kept, got '$($config.recipient)'" }
        foreach ($bad in '{ "recipient": "not-an-address" }') {
            $bad | Set-Content -LiteralPath $path -Encoding utf8
            $caught = $null; try { Get-VmrQBConfig -Path $path } catch { $caught = $_.Exception.Message }
            if (-not $caught -or $caught -notmatch "invalid 'recipient'") { throw "an invalid address must be refused and name the field: $caught" }
        }
    }
    finally { Remove-Item $dir -Recurse -Force -ErrorAction SilentlyContinue }
}

Check 'A recipient may be several addresses separated by ; or ,: every one must be an address, and they are read as a list' {
    $dir = New-TempDir
    try {
        $path = Join-Path $dir 'qb-config.local.json'
        foreach ($good in 'a@example.com; b@example.com', 'a@example.com,b@example.com', ' a@example.com ;b@example.com; ') {
            "{ `"recipient`": `"$good`" }" | Set-Content -LiteralPath $path -Encoding utf8
            if (@((Get-VmrQBConfigLenient -Path $path).Problems).Count) { throw "a list of addresses must be accepted: '$good'" }
            [void](Get-VmrQBConfig -Path $path)
            if ((@(Get-VmrQBRecipientList $good) -join '|') -ne 'a@example.com|b@example.com') { throw "the list must be read as two addresses: '$good'" }
        }
        '{ "recipient": "a@example.com; nope" }' | Set-Content -LiteralPath $path -Encoding utf8
        $lenient = Get-VmrQBConfigLenient -Path $path
        if (@($lenient.Problems).Count -ne 1 -or $lenient.Config.recipient -ne '') { throw 'one bad address in the list must turn email off with a note' }
        $caught = $null; try { Get-VmrQBConfig -Path $path } catch { $caught = $_.Exception.Message }
        if (-not $caught -or $caught -notmatch "invalid 'recipient'") { throw "one bad address in the list must be refused by the strict reader: $caught" }
        Set-VmrQBConfig -Recipient ' me@example.com ,team@example.com ' -Path $path | Out-Null
        if ((Get-VmrQBConfig -Path $path).recipient -ne 'me@example.com; team@example.com') { throw 'a written list is stored tidy, separated by ;' }
    }
    finally { Remove-Item $dir -Recurse -Force -ErrorAction SilentlyContinue }
}

Check 'The public qb-config.template.json is blank and matches the built-in defaults' {
    $template = (Get-VmrQBPaths).TemplatePath
    if (-not (Test-Path -LiteralPath $template)) { throw "the skill must ship $template" }
    $shipped = Get-Content -LiteralPath $template -Raw | ConvertFrom-Json
    if ([string](Get-VmrQBField $shipped 'recipient' 'x')) { throw "the public template must have an empty 'recipient'" }
    foreach ($property in (Get-VmrQBDefaultConfig).PSObject.Properties) {
        if ($null -eq $shipped.PSObject.Properties[$property.Name]) { throw "the template is missing '$($property.Name)'" }
        if ([string]$shipped.($property.Name) -ne [string]$property.Value) { throw "the template's '$($property.Name)' ('$($shipped.($property.Name))') differs from the built-in default ('$($property.Value)')" }
    }
}
Check 'Local data defaults to <skill>/local, and VMR_QB_HOME overrides it' {
    $saved = $env:VMR_QB_HOME
    try {
        $env:VMR_QB_HOME = ''
        $paths = Get-VmrQBPaths
        $expected = Join-Path $paths.SkillRoot 'local'
        if ($paths.StateRoot -ne $expected) { throw "default state root should be $expected, got $($paths.StateRoot)" }
        foreach ($name in 'ConfigPath', 'HistoryPath', 'ReportedLinksPath', 'LocalContextPath', 'AliasesPath', 'HandoffDir', 'ReportsDir', 'LogsDir', 'ArtifactsDir', 'RunDir', 'LocalKnowledgeDir') {
            if (-not $paths.$name.StartsWith($expected)) { throw "$name ($($paths.$name)) is outside the local folder" }
        }
        $env:VMR_QB_HOME = Join-Path ([IO.Path]::GetTempPath()) 'vmrqb-override-check'
        if ((Get-VmrQBPaths).StateRoot -ne [IO.Path]::GetFullPath($env:VMR_QB_HOME)) { throw 'VMR_QB_HOME must override the default' }
    }
    finally { $env:VMR_QB_HOME = $saved }
}
Check 'Writing a config keeps the defaults and refuses a bad address' {
    $dir = New-TempDir
    try {
        $path = Join-Path $dir 'qb-config.local.json'
        Set-VmrQBConfig -Recipient 'next.qb@example.com' -Path $path | Out-Null
        $config = Get-VmrQBConfig -Path $path
        if ($config.recipient -ne 'next.qb@example.com') { throw "recipient not persisted: $($config.recipient)" }
        if (-not $config.PSObject.Properties['pluginRoot']) { throw 'template defaults were dropped' }
        if (-not $config.PSObject.Properties['model']) { throw 'the optional model key must be present' }

        $caught = $null
        try { Set-VmrQBConfig -Recipient 'nope' -Path $path } catch { $caught = $_.Exception.Message }
        if (-not $caught) { throw 'an invalid address must be refused' }
    }
    finally { Remove-Item $dir -Recurse -Force -ErrorAction SilentlyContinue }
}

Check 'Export and import round trip preserves counts, status and aliases' {
    $dir = New-TempDir
    try {
        $source = Join-Path $dir 'source.json'
        Initialize-VmrIssueStore -Path $source | Out-Null
        Add-VmrIssueOccurrence -Path $source -Name 'Widget break' -ReportDate '2026-10-01' -BuildIds @('1', '2') | Out-Null
        Add-VmrIssueOccurrence -Path $source -Name 'Widget break' -ReportDate '2026-10-02' -BuildIds @('3') | Out-Null
        Set-VmrIssueStatus -Path $source -Name 'Widget break' -Status routed -OwnerTeam '@dotnet/prodconsvcs' `
            -TrackerUrl 'https://github.com/dotnet/dotnet/issues/1' -On '2026-10-02' -Note 'Routed.' | Out-Null
        Register-VmrIssueAlias -Path $source -Alias 'Widget breakage' -CanonicalName 'Widget break' | Out-Null

        $export = Join-Path $dir 'export.json'
        $exported = Export-VmrIssueHistory -Path $source -OutputPath $export
        if ($exported.schemaVersion -lt 1) { throw 'export must record a schema version' }

        $target = Join-Path $dir 'target.json'
        Import-VmrIssueHistory -Path $target -InputPath $export | Out-Null
        $history = Get-VmrIssueHistory -Path $target -Name 'Widget break'
        if ($history.issue.occurrences -ne 2) { throw "occurrences=$($history.issue.occurrences), expected 2" }
        if ($history.issue.status -ne 'routed') { throw "status=$($history.issue.status)" }
        if ($history.issue.tracker_url -ne 'https://github.com/dotnet/dotnet/issues/1') { throw 'tracker lost' }
        if (@($history.statusTrail).Count -lt 2) { throw 'status trail lost' }
        # The alias must survive, so the renamed wording still resolves after a handoff.
        if ($null -eq (Get-VmrIssueHistory -Path $target -Name 'Widget breakage')) { throw 'alias did not travel' }

        # Re-importing must not duplicate anything.
        Import-VmrIssueHistory -Path $target -InputPath $export | Out-Null
        $again = Get-VmrIssueHistory -Path $target -Name 'Widget break'
        if ($again.issue.occurrences -ne 2) { throw "re-import duplicated to $($again.issue.occurrences)" }
        if (@($again.statusTrail).Count -ne @($history.statusTrail).Count) { throw 're-import duplicated status history' }
    }
    finally { Remove-Item $dir -Recurse -Force -ErrorAction SilentlyContinue }
}

Check 'Importing a colleague export never discards your own recorded work' {
    $dir = New-TempDir
    try {
        # Mine: I already routed this one and recorded an owner.
        $mine = Join-Path $dir 'mine.json'
        Initialize-VmrIssueStore -Path $mine | Out-Null
        Add-VmrIssueOccurrence -Path $mine -Name 'Shared failure' -ReportDate '2026-10-05' -BuildIds @('9') | Out-Null
        Set-VmrIssueStatus -Path $mine -Name 'Shared failure' -Status owned -OwnerTeam '@dotnet/dnceng' `
            -TrackerUrl 'https://github.com/dotnet/dotnet/issues/42' -On '2026-10-05' | Out-Null

        # Theirs: same failure, seen earlier, no owner established.
        $theirs = Join-Path $dir 'theirs-store.json'
        Initialize-VmrIssueStore -Path $theirs | Out-Null
        Add-VmrIssueOccurrence -Path $theirs -Name 'Shared failure' -ReportDate '2026-09-20' -BuildIds @('1') | Out-Null
        $export = Join-Path $dir 'theirs.json'
        Export-VmrIssueHistory -Path $theirs -OutputPath $export | Out-Null

        Import-VmrIssueHistory -Path $mine -InputPath $export | Out-Null
        $merged = Get-VmrIssueHistory -Path $mine -Name 'Shared failure'
        if ($merged.issue.occurrences -ne 2) { throw "merge lost an occurrence: $($merged.issue.occurrences)" }
        if ($merged.issue.first_seen -ne '2026-09-20') { throw "earlier first_seen not adopted: $($merged.issue.first_seen)" }
        if ($merged.issue.owner_team -ne '@dotnet/dnceng') { throw "my owner was overwritten: $($merged.issue.owner_team)" }
        if ($merged.issue.tracker_url -ne 'https://github.com/dotnet/dotnet/issues/42') { throw 'my tracker was overwritten' }
    }
    finally { Remove-Item $dir -Recurse -Force -ErrorAction SilentlyContinue }
}

Check 'An export from a newer schema is refused with a clear message' {
    $dir = New-TempDir
    try {
        $export = Join-Path $dir 'future.json'
        '{ "schemaVersion": 9999, "issues": [], "aliases": [], "occurrences": [], "statusHistory": [] }' |
            Set-Content -LiteralPath $export -Encoding utf8
        $caught = $null
        try { Import-VmrIssueHistory -Path (Join-Path $dir 'x.json') -InputPath $export } catch { $caught = $_.Exception.Message }
        if (-not $caught) { throw 'a newer-schema export must be refused, not silently mangled' }
        if ($caught -notmatch '9999') { throw "message should name the schema it saw: $caught" }
    }
    finally { Remove-Item $dir -Recurse -Force -ErrorAction SilentlyContinue }
}

Check 'Report email rendering produces real HTML tables, not raw markdown' {
    # A prior bug just HTML-encoded markdown and joined lines with <br>: pipe-delimited table
    # rows rendered as unreadable literal text instead of a table. Guard against that regressing.
    $markdown = @'
## Summary

| Issue category | Builds | Action |
|---|---|---|
| BH #1 - Something broke | [123](https://dev.azure.com/dnceng/internal/_build/results?buildId=123) | Where: Teams - Unified Build.<br>Copy/paste: "@dotnet/team, check this" |
'@
    $html = ConvertTo-VmrQBEmailBody -Markdown $markdown
    foreach ($tag in @('<table', '<th', '<td', '<a href=')) {
        if ($html -notmatch [regex]::Escape($tag)) { throw "Rendered HTML is missing $tag - markdown was not converted" }
    }
    if ($html -match '\|---\|' -or $html -match '\| BH #1') { throw 'Raw markdown table syntax leaked into the HTML body' }
    if ($html -notmatch 'dev\.azure\.com/dnceng/internal/_build/results\?buildId=123') { throw 'Build link was lost during conversion' }
}

Check 'Scripts are organised per workflow: SKILL.md leads to every entry script, and no catch-all lib or dispatcher remains' {
    $scripts = (Get-VmrQBPaths).ScriptsRoot
    $skill = (Get-VmrQBPaths).SkillRoot
    if (Test-Path -LiteralPath (Join-Path $scripts 'lib')) { throw 'scripts/lib must not exist: each workflow keeps its own libraries' }
    if (Test-Path -LiteralPath (Join-Path $scripts 'qb.ps1')) { throw 'there is no central dispatcher; each workflow has its own entry script' }
    $entries = @(Get-ChildItem -LiteralPath $scripts -Directory | Where-Object Name -ne 'tests' | ForEach-Object { Get-ChildItem -LiteralPath $_.FullName -Filter 'Invoke-*.ps1' -File })
    if ($entries.Count -lt 8) { throw "expected the workflow entry scripts, found $($entries.Count)" }
    $skillText = [IO.File]::ReadAllText((Join-Path $skill 'SKILL.md')) + (@(Get-ChildItem -LiteralPath (Join-Path $skill 'references') -Filter *.md | ForEach-Object { [IO.File]::ReadAllText($_.FullName) }) -join "`n")
    $missing = @($entries | Where-Object { -not $skillText.Contains("$($_.Directory.Name)/$($_.Name)") } | ForEach-Object { "$($_.Directory.Name)/$($_.Name)" })
    if ($missing.Count) { throw "entry scripts that no guide tells the agent to run: $($missing -join ', ')" }
    foreach ($file in Get-ChildItem -LiteralPath $scripts -Recurse -Filter *.ps1 -File | Where-Object { $_.DirectoryName -notmatch '\\tests' }) {
        foreach ($line in [IO.File]::ReadAllLines($file.FullName)) {
            if ($line -match "^\s*\. \(Join-Path \`$PSScriptRoot '\.\.\\(?<dir>[a-z]+)\\") {
                # A workflow may load common/ and the libraries of another workflow, but never an entry script.
                if ($line -match 'Invoke-') { throw "$($file.Name) dot-sources an entry script: $line" }
            }
        }
    }
}

if ($failures) { Write-Host "$failures package test(s) failed." -ForegroundColor Red; exit 1 }
Write-Host '11 offline package tests. Temporary files only; no email, reports, or external changes.' -ForegroundColor Green
