Set-StrictMode -Version Latest

# common/core.ps1 - Paths, optional config, plugin discovery and local routing/ownership data initialization.
# Dot-sourced by the entry scripts and the tests; it defines functions only and runs nothing.
#
# SHIPPED files (scripts/, knowledge/routing.json, references/, SKILL.md) are public. LOCAL files (settings, reports, logs,
# evidence, notes, the team roster) belong to one QB and live in ONE git-ignored folder, <skill>/local/. $env:VMR_QB_HOME points
# the skill at a different folder.

function Get-VmrQBSkillRoot {
    # scripts/common/ -> vmr-qb-agent/
    return (Resolve-Path (Join-Path $PSScriptRoot '..\..')).Path
}

function Get-VmrQBPackageRoot {
    # <repo>/.github/skills/vmr-qb-agent/scripts/common -> <repo>. docs/VMR-QB-Guide.md is read from this checkout.
    return (Resolve-Path (Join-Path $PSScriptRoot '..\..\..\..\..')).Path
}

function Get-VmrQBStateRoot {
    $override = [Environment]::GetEnvironmentVariable('VMR_QB_HOME')
    if (-not [string]::IsNullOrWhiteSpace($override)) { return [IO.Path]::GetFullPath($override) }
    return Join-Path (Get-VmrQBSkillRoot) 'local'
}

function Get-VmrQBPaths {
    # The single answer to "where is X" for every script. Nothing else should build these paths.
    [CmdletBinding()]
    param([string] $StateRoot, [switch] $Create)
    $skill = Get-VmrQBSkillRoot
    if (-not $StateRoot) { $StateRoot = Get-VmrQBStateRoot }
    $StateRoot = [IO.Path]::GetFullPath($StateRoot)
    $paths = [pscustomobject][ordered]@{
        SkillRoot          = $skill
        ScriptsRoot        = Join-Path $skill 'scripts'
        KnowledgeRoot      = Join-Path $skill 'knowledge'
        ReferencesRoot     = Join-Path $skill 'references'
        StateRoot          = $StateRoot
        # Your own settings: local, git-ignored and optional. The shipped template is the blank copy source.
        ConfigPath         = Join-Path $StateRoot 'qb-config.local.json'
        TemplatePath       = Join-Path $skill 'qb-config.template.json'
        # What people told you about findings, written by hand. It is handed to the model.
        OwnerNotesPath     = Join-Path $StateRoot 'owner-notes.md'
        LocalKnowledgeDir  = Join-Path $StateRoot 'knowledge'
        # Your copy of routing.json, made once on first use and used from then on.
        LocalRoutingPath   = Join-Path $StateRoot 'knowledge\routing.local.json'
        # What the Guide's Repo Ownership table said when it was first read, used from then on.
        GuideOwnershipPath = Join-Path $StateRoot 'knowledge\guide-ownership.json'
        ContactsPath       = Join-Path $StateRoot 'knowledge\team-contacts.json'
        ReportsDir         = Join-Path $StateRoot 'reports'
        LogsDir            = Join-Path $StateRoot 'logs'
        ArtifactsDir       = Join-Path $StateRoot 'artifacts'
    }
    if ($Create) {
        foreach ($name in 'StateRoot', 'LocalKnowledgeDir', 'ReportsDir', 'LogsDir', 'ArtifactsDir') {
            [void][IO.Directory]::CreateDirectory($paths.$name)
        }
    }
    return $paths
}

function Get-VmrQBField {
    param([AllowNull()][object] $Object, [string] $Name, [AllowNull()][object] $Default = $null)
    if ($null -eq $Object) { return $Default }
    if ($Object -is [System.Collections.IDictionary]) {
        if ($Object.Contains($Name)) { return $Object[$Name] }
        return $Default
    }
    $property = $Object.PSObject.Properties[$Name]
    if ($null -ne $property) { return $property.Value }
    return $Default
}

# ---------------------------------------------------------------------------------------------
# Copilot plugins
# ---------------------------------------------------------------------------------------------
# The plugins the model stages use. dotnet-helix and dotnet-codeflow are required for the report stages.
$script:VmrQBPluginNames = @('dotnet-helix', 'dotnet-codeflow', 'dotnet-binlog', 'dotnet-arcade', 'dotnet-compliance')

function Get-VmrQBPluginSearchRoots {
    # The folders that may hold plugin folders, most specific first: pluginRoot from the config (one folder or a list), then for
    # COPILOT_HOME and ~/.copilot the installed-plugins folder itself and each marketplace folder under it (dotnet-arcade-skills first).
    # Only folders that exist are returned.
    [CmdletBinding()]
    param([AllowNull()][object] $ConfiguredRoot, [string] $CopilotHome = $env:COPILOT_HOME, [string] $UserProfile = [Environment]::GetFolderPath('UserProfile'))
    $roots = [Collections.Generic.List[string]]::new()
    foreach ($configured in @($ConfiguredRoot | Where-Object { -not [string]::IsNullOrWhiteSpace([string]$_) })) { $roots.Add(([string]$configured).Trim()) }
    $copilotDirectories = @($CopilotHome, $(if ($UserProfile) { Join-Path $UserProfile '.copilot' })) | Where-Object { -not [string]::IsNullOrWhiteSpace($_) }
    foreach ($copilotDirectory in $copilotDirectories) {
        $installed = Join-Path $copilotDirectory 'installed-plugins'
        if (-not (Test-Path -LiteralPath $installed -PathType Container)) { continue }
        $roots.Add($installed)
        $marketplaces = @(Get-ChildItem -LiteralPath $installed -Directory -ErrorAction SilentlyContinue | Sort-Object { if ($_.Name -eq 'dotnet-arcade-skills') { '' } else { $_.Name } })
        foreach ($marketplace in $marketplaces) { $roots.Add($marketplace.FullName) }
    }
    return @($roots | Where-Object { Test-Path -LiteralPath $_ -PathType Container } | Select-Object -Unique)
}

function Find-VmrQBPlugin {
    # Finds each named plugin folder under the search roots; the first root that has it wins. Path is $null when it is not installed.
    [CmdletBinding()]
    param([string[]] $Name = $script:VmrQBPluginNames, [Parameter(Mandatory)][AllowEmptyCollection()][string[]] $SearchRoot)
    foreach ($plugin in $Name) {
        $found = $null
        foreach ($root in $SearchRoot) {
            $candidate = Join-Path $root $plugin
            if (Test-Path -LiteralPath $candidate -PathType Container) { $found = $candidate; break }
        }
        [pscustomobject]@{ Name = $plugin; Path = $found }
    }
}

# ---------------------------------------------------------------------------------------------
# Config
# ---------------------------------------------------------------------------------------------
function Get-VmrQBDefaultConfig {
    # The built-in defaults, the same as the shipped qb-config.template.json.
    return [pscustomobject][ordered]@{
        model      = ''
        # Optional folder(s) holding the Copilot plugins (one folder per plugin). Blank: they are found under COPILOT_HOME and
        # ~/.copilot/installed-plugins, in any marketplace folder.
        pluginRoot = ''
    }
}

function Get-VmrQBConfigLenient {
    <#
    .SYNOPSIS
    The config for a run, which never stops for it: a problem comes back in Problems and the default stands in for it.
    .DESCRIPTION
    local/qb-config.local.json optionally names the model and plugins. An unreadable or invalid config uses the defaults
    and returns a problem explaining the fix.
    #>
    [CmdletBinding()]
    param([string] $Path = (Get-VmrQBPaths).ConfigPath)
    $problems = [Collections.Generic.List[string]]::new()
    $defaults = Get-VmrQBDefaultConfig
    if (-not (Test-Path -LiteralPath $Path)) { return [pscustomobject]@{ Config = $defaults; Problems = @() } }
    $config = $null
    try { $config = Get-Content -LiteralPath $Path -Raw | ConvertFrom-Json }
    catch { $problems.Add("The QB config $Path is not valid JSON ($($_.Exception.Message)), so the defaults are used. Fix the file; qb-config.template.json shows the shape.") }
    if ($null -eq $config -or $config -isnot [System.Management.Automation.PSCustomObject]) {
        if ($problems.Count -eq 0) { $problems.Add("The QB config $Path is not a JSON object, so the defaults are used. Fix the file; qb-config.template.json shows the shape.") }
        return [pscustomobject]@{ Config = $defaults; Problems = @($problems) }
    }
    foreach ($property in $defaults.PSObject.Properties) {
        if ($null -eq $config.PSObject.Properties[$property.Name]) { $config | Add-Member -NotePropertyName $property.Name -NotePropertyValue $property.Value }
    }
    return [pscustomobject]@{ Config = $config; Problems = @($problems) }
}

# ---------------------------------------------------------------------------------------------
# Routing: who owns what, which handles are real
# ---------------------------------------------------------------------------------------------
# Repo -> owner and the real teams come from the Repo Ownership table of docs/VMR-QB-Guide.md in the checkout this skill lives in.
# knowledge/routing.json holds what the Guide does not say: the test/ owner, redirects, channels, support destinations, categories.
# Both are copied once into the local folder and used from then on: a pull changes nothing until the copy is deleted, and a new
# one is then made from the shipped file or the Guide.

function ConvertFrom-VmrQBGuideOwnership {
    # The "Repo Ownership" table of VMR-QB-Guide.md: one row per repo, naming its GitHub team (from the row's link, or a bare @org/slug).
    [CmdletBinding()]
    param([Parameter(Mandatory)][string] $Text)
    $lines = $Text -split "`r?`n"
    $start = -1
    for ($i = 0; $i -lt $lines.Count; $i++) { if ($lines[$i] -match '^##\s+Repo Ownership\b') { $start = $i; break } }
    if ($start -lt 0) { throw "The guide has no '## Repo Ownership' section. If it was renamed, update ConvertFrom-VmrQBGuideOwnership." }
    $rows = [Collections.Generic.List[object]]::new()
    for ($i = $start + 1; $i -lt $lines.Count; $i++) {
        $line = $lines[$i].Trim()
        if ($line -match '^#{1,6}\s') { break }
        if (-not $line.StartsWith('|') -or $line -match '^\|[\s:|-]+\|?$') { continue }
        $cells = @($line.Trim('|').Split('|') | ForEach-Object { $_.Trim() })
        if ($cells.Count -lt 2) { continue }
        $link = [regex]::Match($cells[1], 'github\.com/orgs/([A-Za-z0-9._-]+)/teams/([A-Za-z0-9._-]+)')
        $handle = [regex]::Match($cells[1], '@([A-Za-z0-9._-]+)/([A-Za-z0-9._-]+)')
        $match = if ($link.Success) { $link } elseif ($handle.Success) { $handle } else { continue }
        $rows.Add([pscustomobject]@{ Repo = ($cells[0] -replace '[*`_]', '').Trim(); Team = "@$($match.Groups[1].Value)/$($match.Groups[2].Value)" })
    }
    if ($rows.Count -eq 0) { throw "The 'Repo Ownership' section has no table rows naming a GitHub team. The guide's format may have changed." }
    return $rows.ToArray()
}

function Get-VmrQBGuideOwnershipRows {
    # The Repo Ownership rows of the Guide in this checkout. It is never fetched: it changes when you git pull.
    # Throws, naming the file, when it cannot be read or parsed.
    [CmdletBinding()]
    param()
    $guide = Join-Path (Get-VmrQBPackageRoot) 'docs\VMR-QB-Guide.md'
    if (-not (Test-Path -LiteralPath $guide -PathType Leaf)) {
        throw "Guide not found: $guide. The skill reads repo owners from docs/VMR-QB-Guide.md in the dotnet/dotnet checkout it lives in; run it from that checkout (git pull updates the guide)."
    }
    try { return @(ConvertFrom-VmrQBGuideOwnership -Text ([IO.File]::ReadAllText($guide))) }
    catch { throw "$($_.Exception.Message) (guide: $guide)" }
}

function Initialize-VmrQBGuideOwnership {
    # Make the local ownership data once; report stages read the saved JSON directly.
    [CmdletBinding()]
    param()
    $copyPath = (Get-VmrQBPaths).GuideOwnershipPath
    if (Test-Path -LiteralPath $copyPath) { return $false }
    $rows = @(Get-VmrQBGuideOwnershipRows)
    $document = [ordered]@{
        schemaVersion = 1
        capturedAtUtc = [DateTimeOffset]::UtcNow.ToString('o')
        rows = @($rows | ForEach-Object { [ordered]@{ repo = $_.Repo; team = $_.Team } })
    }
    [void][IO.Directory]::CreateDirectory((Split-Path -Parent $copyPath))
    [IO.File]::WriteAllText($copyPath, (ConvertTo-Json -InputObject $document -Depth 6) + "`n", [Text.UTF8Encoding]::new($false))
    return $true
}

function Initialize-VmrQBLocalRouting {
    # Your own copy of knowledge/routing.json, made once and never touched again. Returns $true only when it made the copy.
    $paths = Get-VmrQBPaths
    $shipped = Join-Path $paths.KnowledgeRoot 'routing.json'
    if ((Test-Path -LiteralPath $paths.LocalRoutingPath) -or -not (Test-Path -LiteralPath $shipped -PathType Leaf)) { return $false }
    [void][IO.Directory]::CreateDirectory($paths.LocalKnowledgeDir)
    Copy-Item -LiteralPath $shipped -Destination $paths.LocalRoutingPath -ErrorAction Stop
    return $true
}
