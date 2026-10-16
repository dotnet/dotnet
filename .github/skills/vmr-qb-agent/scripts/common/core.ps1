Set-StrictMode -Version Latest

# common/core.ps1 - Configuration, state-directory paths, the team routing registry and the contact roster.
# Dot-sourced by the entry scripts and the tests; it defines functions only and runs nothing.

# ---------------------------------------------------------------------------------------------
# core/Config.ps1
# ---------------------------------------------------------------------------------------------
# Where things live, and who the reports go to.
#
# Two kinds of files exist and they must never be mixed:
#   * SHIPPED   - scripts/, knowledge/routing.json, references/, SKILL.md. The skill, shared publicly.
#   * LOCAL     - everything one QB creates for their own sessions: qb-config.local.json (an email address),
#                 the issue history, reports, logs, evidence, the team roster, verified findings and
#                 handoff files. It all lives in ONE folder, <skill>/local/, which the skill's
#                 .gitignore excludes (/local/). Nothing local is ever published.
#
# Local root, first match wins:
#   1. $env:VMR_QB_HOME   explicit override (also what the tests use)
#   2. <skill>/local      the default

function Get-VmrQBSkillRoot {
    # scripts/common/ -> vmr-qb-agent/
    return (Resolve-Path (Join-Path $PSScriptRoot '..\..')).Path
}

function Get-VmrQBPackageRoot {
    # <repo>/.github/skills/vmr-qb-agent/scripts/common -> <repo>. Used to find docs/VMR-QB-Guide.md in the enclosing checkout.
    return (Resolve-Path (Join-Path $PSScriptRoot '..\..\..\..\..')).Path
}

function Get-VmrQBStateRoot {
    [CmdletBinding()]
    param([switch] $Create)
    $override = [Environment]::GetEnvironmentVariable('VMR_QB_HOME')
    $root = if (-not [string]::IsNullOrWhiteSpace($override)) { [IO.Path]::GetFullPath($override) }
            else { Join-Path (Get-VmrQBSkillRoot) 'local' }
    if ($Create) { [void][IO.Directory]::CreateDirectory($root) }
    return $root
}
function Get-VmrQBPaths {
    <#
    .SYNOPSIS
    The single answer to "where is X" for every script. Nothing else should build these paths.
    #>
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
        # Your own settings (an email address): local, git-ignored, and optional. The shipped qb-config.template.json is the blank copy source.
        ConfigPath         = Join-Path $StateRoot 'qb-config.local.json'
        TemplatePath       = Join-Path $skill 'qb-config.template.json'
        HistoryPath        = Join-Path $StateRoot 'issue-history.json'
        LegacyDatabasePath = Join-Path $StateRoot 'issue-history.db'
        ReportedLinksPath  = Join-Path $StateRoot 'reported-links.json'
        LocalKnowledgeDir  = Join-Path $StateRoot 'knowledge'
        # Your copy of routing.json (made once) and the pristine shipped text it was copied from, which lets a changed
        # shipped file be told apart from your own edits.
        LocalRoutingPath   = Join-Path $StateRoot 'knowledge\routing.local.json'
        RoutingBasePath    = Join-Path $StateRoot 'knowledge\routing.base.json'
        # What the Guide's Repo Ownership table said when it was first read (renewed only when the Guide file changes).
        GuideOwnershipPath = Join-Path $StateRoot 'knowledge\guide-ownership.json'
        # Everything below is local: what this QB learned. Never shipped.
        LocalContextPath   = Join-Path $StateRoot 'knowledge\triage-context.local.json'
        AliasesPath        = Join-Path $StateRoot 'knowledge\issue-aliases.json'
        HandoffDir         = Join-Path $StateRoot 'handoff'
        ReportsDir         = Join-Path $StateRoot 'reports'
        LogsDir            = Join-Path $StateRoot 'logs'
        ArtifactsDir       = Join-Path $StateRoot 'artifacts'
        RunDir             = Join-Path $StateRoot 'run'
    }
    if ($Create) {
        foreach ($name in 'StateRoot', 'LocalKnowledgeDir', 'ReportsDir', 'LogsDir', 'ArtifactsDir', 'RunDir') {
            [void][IO.Directory]::CreateDirectory($paths.$name)
        }
    }
    return $paths
}

function Get-VmrQBRehearsalPaths {
    <#
    .SYNOPSIS
    The paths a rehearsal (-OutputRoot) works in: its own reports, logs, artifacts, run folder and history file.
    .DESCRIPTION
    A rehearsal must not overwrite a delivered report or receipt, replay itself into the real history, or share a run
    folder with a real run, so everything it writes is rooted at -OutputRoot. Configuration, local knowledge and reported
    links are inputs and keep pointing at the real state. Nothing is created here; the caller copies the history.
    #>
    [CmdletBinding()]
    param([Parameter(Mandatory)][object] $Paths, [Parameter(Mandatory)][string] $OutputRoot)
    $root = [IO.Path]::GetFullPath($OutputRoot).TrimEnd('\', '/')
    # The rehearsal root may not be the real state folder or sit inside any folder that holds real data.
    foreach ($protected in @($Paths.StateRoot, $Paths.ReportsDir, $Paths.LogsDir, $Paths.ArtifactsDir, $Paths.RunDir, $Paths.LocalKnowledgeDir, $Paths.HandoffDir)) {
        $guarded = ([string]$protected).TrimEnd('\', '/')
        $isStateRoot = $guarded -ieq ([string]$Paths.StateRoot).TrimEnd('\', '/')
        $inside = if ($isStateRoot) { $root -ieq $guarded } else { $root -ieq $guarded -or $root.StartsWith($guarded + [IO.Path]::DirectorySeparatorChar, [StringComparison]::OrdinalIgnoreCase) }
        if ($inside) { throw "-OutputRoot '$OutputRoot' is, or is inside, '$guarded'. Choose a separate folder so a rehearsal cannot touch real reports, receipts or history." }
    }
    $rehearsal = [pscustomobject][ordered]@{}
    foreach ($property in $Paths.PSObject.Properties) { Add-Member -InputObject $rehearsal -NotePropertyName $property.Name -NotePropertyValue $property.Value }
    $rehearsal.StateRoot    = $root
    $rehearsal.HistoryPath  = Join-Path $root 'issue-history.json'
    $rehearsal.ReportsDir   = Join-Path $root 'reports'
    $rehearsal.LogsDir      = Join-Path $root 'logs'
    $rehearsal.ArtifactsDir = Join-Path $root 'artifacts'
    $rehearsal.RunDir       = Join-Path $root 'run'
    return $rehearsal
}

# The Copilot plugins the model stages use. dotnet-helix and dotnet-codeflow are required for the report stages.
$script:VmrQBPluginNames = @('dotnet-helix', 'dotnet-codeflow', 'dotnet-binlog', 'dotnet-arcade', 'dotnet-compliance')

function Get-VmrQBPluginSearchRoots {
    <#
    .SYNOPSIS
    The folders that may hold plugin folders, most specific first.
    .DESCRIPTION
    1. pluginRoot from local/qb-config.local.json (one folder or a list), when set.
    2. For COPILOT_HOME and ~/.copilot: the installed-plugins folder itself (a plugin installed on its own), then each marketplace
       folder under it, with dotnet-arcade-skills first and the rest in name order.
    Only folders that exist are returned. Nothing is assumed about one person's install tree.
    #>
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
    <#
    .SYNOPSIS
    Finds each named plugin folder under the search roots; the first root that has it wins. Path is $null when it is not installed.
    #>
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

function Find-VmrQBSkillPlugin {
    <#
    .SYNOPSIS
    The plugin folder that holds a skill, found by the skill's name rather than the plugin's, or $null. A plugin that was renamed
    or that a skill moved to is still found.
    #>
    [CmdletBinding()]
    param([Parameter(Mandatory)][string] $SkillName, [Parameter(Mandatory)][AllowEmptyCollection()][string[]] $SearchRoot)
    foreach ($root in $SearchRoot) {
        foreach ($plugin in @(Get-ChildItem -LiteralPath $root -Directory -ErrorAction SilentlyContinue)) {
            if (Test-Path -LiteralPath (Join-Path $plugin.FullName "skills\$SkillName\SKILL.md") -PathType Leaf) { return $plugin.FullName }
        }
    }
    return $null
}

function Get-VmrQBConfigPath {
    param([string] $PackageRoot)
    # -PackageRoot is accepted for older callers; state now has its own root.
    if ($PackageRoot) { return (Join-Path $PackageRoot 'qb-config.local.json') }
    return (Get-VmrQBPaths).ConfigPath
}

function Test-VmrQBEmailAddress {
    param([AllowEmptyString()][AllowNull()][string] $Address)
    if ([string]::IsNullOrWhiteSpace($Address)) { return $false }
    # Deliberately permissive: catch obvious typos and empty values, not police valid addresses.
    return [bool]([regex]::IsMatch($Address.Trim(), '^[^@\s]+@[^@\s]+\.[^@\s]+$'))
}

function Get-VmrQBRecipientList {
    # The addresses in the recipient setting: one, or several separated by ';' or ','.
    param([AllowEmptyString()][AllowNull()][string] $Value)
    return @(([string]$Value -split '[;,]') | ForEach-Object { $_.Trim() } | Where-Object { $_ })
}

function Test-VmrQBRecipient {
    # True when the recipient setting is one address, or several separated by ';' or ',', and every one of them looks like an address.
    param([AllowEmptyString()][AllowNull()][string] $Value)
    $list = @(Get-VmrQBRecipientList $Value)
    return ($list.Count -gt 0 -and @($list | Where-Object { -not (Test-VmrQBEmailAddress $_) }).Count -eq 0)
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

function Get-VmrQBDefaultConfig {
    # The built-in defaults, the same as the shipped qb-config.template.json. An empty recipient means "do not send email".
    # There is no time zone setting: the report date follows this computer's own time zone, detected from the system.
    return [pscustomobject][ordered]@{
        recipient  = ''
        model      = ''
        # Optional folder(s) holding the Copilot plugins (one folder per plugin). Blank: they are found under COPILOT_HOME and
        # ~/.copilot/installed-plugins, in any marketplace folder.
        pluginRoot = ''
    }
}

function Write-VmrQBConfigFile {
    param([Parameter(Mandatory)][string] $Path, [Parameter(Mandatory)][object] $Config)
    $directory = Split-Path -Parent $Path
    if ($directory -and -not (Test-Path -LiteralPath $directory)) { [void][IO.Directory]::CreateDirectory($directory) }
    $json = (ConvertTo-Json -InputObject $Config -Depth 10) -replace "`r`n", "`n"
    [IO.File]::WriteAllText($Path, $json + "`n", [Text.UTF8Encoding]::new($false))
}

function Get-VmrQBConfig {
    <#
    .SYNOPSIS
    Reads local/qb-config.local.json, your optional settings (the skill ships only the blank qb-config.template.json).
    .DESCRIPTION
    Nothing is created or required. An empty "recipient" means reports are saved but not emailed; to get them by
    email, copy qb-config.template.json to local/qb-config.local.json and set "recipient" to your own address: the reports are sent from the Outlook account that matches it. Several addresses may be separated by ';' to send to more people, and at least one of them must be your own. Every address that is present must look like an email address.
    A missing file, or a key missing from it, takes the built-in default.
    #>
    [CmdletBinding()]
    param([string] $Path = (Get-VmrQBConfigPath))
    $defaults = Get-VmrQBDefaultConfig
    if (-not (Test-Path -LiteralPath $Path)) { return $defaults }
    try { $config = Get-Content -LiteralPath $Path -Raw | ConvertFrom-Json }
    catch { throw "QB config at $Path is not valid JSON: $($_.Exception.Message)" }
    $recipient = [string](Get-VmrQBField $config 'recipient')
    if (-not [string]::IsNullOrWhiteSpace($recipient) -and -not (Test-VmrQBRecipient $recipient)) {
        throw "QB config at $Path has an invalid 'recipient' ('$recipient'). Use an email address (several may be separated by ';'), or leave 'recipient' empty to skip email."
    }
    foreach ($property in $defaults.PSObject.Properties) {
        if ($null -eq $config.PSObject.Properties[$property.Name]) { $config | Add-Member -NotePropertyName $property.Name -NotePropertyValue $property.Value }
    }
    return $config
}

function Get-VmrQBConfigLenient {
    <#
    .SYNOPSIS
    The config for a daily run, which never stops for it: a problem comes back in Problems and the default stands in for it.
    .DESCRIPTION
    The config only says where to send the email (and, optionally, which time zone to follow instead of this computer's). A file that is not valid JSON gives
    the defaults; a recipient that is not an email address is dropped (no recipient means the reports are saved and not
    emailed). Each problem says how to fix it. Get-VmrQBConfig stays strict for Set-VmrQBConfig and the setup script.
    #>
    [CmdletBinding()]
    param([string] $Path = (Get-VmrQBConfigPath))
    $problems = [Collections.Generic.List[string]]::new()
    $defaults = Get-VmrQBDefaultConfig
    if (-not (Test-Path -LiteralPath $Path)) { return [pscustomobject]@{ Config = $defaults; Problems = @() } }
    $config = $null
    try { $config = Get-Content -LiteralPath $Path -Raw | ConvertFrom-Json }
    catch { $problems.Add("The QB config $Path is not valid JSON ($($_.Exception.Message)), so the defaults are used and no email is sent. To get email, fix the file (qb-config.template.json shows the shape) and set recipient to your address.") }
    if ($null -eq $config -or $config -isnot [System.Management.Automation.PSCustomObject]) {
        if ($problems.Count -eq 0) { $problems.Add("The QB config $Path is not a JSON object, so the defaults are used and no email is sent. To get email, copy qb-config.template.json over it and set recipient to your address.") }
        return [pscustomobject]@{ Config = $defaults; Problems = @($problems) }
    }
    $recipient = [string](Get-VmrQBField $config 'recipient')
    if (-not [string]::IsNullOrWhiteSpace($recipient) -and -not (Test-VmrQBRecipient $recipient)) {
        $problems.Add("The 'recipient' in $Path ('$recipient') is not an email address (or a list of them separated by ';'), so no email is sent. To get email, set it to your email address.")
        $config.recipient = ''
    }
    foreach ($property in $defaults.PSObject.Properties) {
        if ($null -eq $config.PSObject.Properties[$property.Name]) { $config | Add-Member -NotePropertyName $property.Name -NotePropertyValue $property.Value }
    }
    return [pscustomobject]@{ Config = $config; Problems = @($problems) }
}

function Resolve-VmrQBTimeZone {
    <#
    .SYNOPSIS
    The time zone the report date follows: this computer's, detected from the system. Only an optional timeZone override in the
    config names another; an id this machine does not know gives the system time zone and a problem, never an error.
    #>
    [CmdletBinding()]
    param([string] $Id, [string] $ConfigPath)
    if ([string]::IsNullOrWhiteSpace($Id)) { return [pscustomobject]@{ Zone = [TimeZoneInfo]::Local; Problem = '' } }
    try { return [pscustomobject]@{ Zone = [TimeZoneInfo]::FindSystemTimeZoneById($Id); Problem = '' } }
    catch {
        $local = [TimeZoneInfo]::Local
        $where = if ($ConfigPath) { " in $ConfigPath" } else { '' }
        return [pscustomobject]@{ Zone = $local; Problem = "The time zone '$Id' is not known on this machine, so the system time zone ($($local.Id)) decides the report date. Set timeZone$where to a valid id." }
    }
}

function Set-VmrQBConfig {
    <#
    .SYNOPSIS
    Sets the email address in your local config (local/qb-config.local.json), keeping every other setting. Copying the template and editing it by hand does the same.
    #>
    param(
        [Parameter(Mandatory)][string] $Recipient,
        [string] $Path = (Get-VmrQBConfigPath),
        [hashtable] $Extra = @{}
    )
    if (-not (Test-VmrQBRecipient $Recipient)) { throw "'$Recipient' is not a usable email address for recipient." }
    $ordered = [ordered]@{}
    foreach ($property in (Get-VmrQBDefaultConfig).PSObject.Properties) { $ordered[$property.Name] = $property.Value }
    if (Test-Path -LiteralPath $Path) { foreach ($property in (Get-Content -LiteralPath $Path -Raw | ConvertFrom-Json).PSObject.Properties) { $ordered[$property.Name] = $property.Value } }
    $ordered['recipient'] = (Get-VmrQBRecipientList $Recipient) -join '; '
    foreach ($key in $Extra.Keys) { $ordered[$key] = $Extra[$key] }
    Write-VmrQBConfigFile -Path $Path -Config ([pscustomobject]$ordered)
    return $Path
}

function Initialize-VmrQBLocal {
    <#
    .SYNOPSIS
    Creates the (git-ignored) local/ folders on first use. It never writes a config file. Safe to call every time.
    #>
    [CmdletBinding()]
    param()
    $paths = Get-VmrQBPaths -Create
    return [pscustomobject]@{ Paths = $paths; Config = (Get-VmrQBConfig -Path $paths.ConfigPath) }
}
# ---------------------------------------------------------------------------------------------
# core/Teams.ps1
# ---------------------------------------------------------------------------------------------
# Routing registry access: who owns what, which handles exist, which handles are mistakes.
#
# knowledge/routing.json is the single source for everything the Guide does not say (handle aliases,
# channel rules, categories). Repo -> owner is read from the Guide's Repo Ownership table, never copied.
# This module loads both, validates them, merges the
# QB's local additions, and exposes small deterministic functions so that nothing downstream
# (prompts, validators, the history store, docs checks) has to restate or guess an owner.
#
# Local copy: <state>/knowledge/routing.local.json, same schema, created once as a copy of the shipped file
# (Initialize-VmrQBLocalRouting) and edited by the QB from then on. Arrays are unioned, objects are merged by key,
# categories by id, so it can also hold just the additions.

if (-not (Get-Command Get-VmrQBPaths -ErrorAction SilentlyContinue)) {
}

$script:VmrQBRoutingCache = @{}

function Get-VmrQBRoutingField {
    param([AllowNull()][object] $Object, [string] $Name, [AllowNull()][object] $Default = $null)
    if ($null -eq $Object) { return $Default }
    $property = $Object.PSObject.Properties[$Name]
    if ($null -ne $property -and $null -ne $property.Value) { return $property.Value }
    return $Default
}

function Read-VmrQBRoutingFile {
    param([Parameter(Mandatory)][string] $Path, [string] $Label)
    try { $json = Get-Content -LiteralPath $Path -Raw | ConvertFrom-Json -Depth 20 }
    catch { throw "$Label routing file is not valid JSON ($Path): $($_.Exception.Message)" }
    if ([int](Get-VmrQBRoutingField $json 'schemaVersion' 0) -ne 1) {
        throw "$Label routing file has an unsupported schemaVersion; expected 1 ($Path)."
    }
    return $json
}

function Get-VmrQBShippedRoutingHash {
    [CmdletBinding()]
    param([string] $Path = (Join-Path (Get-VmrQBPaths).KnowledgeRoot 'routing.json'))
    $sha = [Security.Cryptography.SHA256]::Create()
    try { return ([BitConverter]::ToString($sha.ComputeHash([IO.File]::ReadAllBytes($Path))) -replace '-', '').ToLowerInvariant() }
    finally { $sha.Dispose() }
}

function Initialize-VmrQBLocalRouting {
    <#
    .SYNOPSIS
    Makes your own, git-ignored copy of knowledge/routing.json the first time, and never again.
    .DESCRIPTION
    If <local>/knowledge/routing.local.json does not exist it is created as a copy of the shipped file, stamped with
    the hash of the shipped file it came from. Anything you add or change in routing goes there. An existing file is
    never touched, so what you edited stays; pulling a newer routing.json does not rewrite it (qb check says when the
    shipped file has changed since the copy). Returns $true only when it created the file.
    The one failure that is not an error is a concurrent first use: another process created the copy between the
    existence check and the move, so the file now exists and is theirs. Every other failure (a parent that is a file, no
    permission, a full disk) throws, because returning quietly would leave a QB without the local copy they were promised.
    #>
    [CmdletBinding()]
    param(
        [string] $LocalPath = (Get-VmrQBPaths).LocalRoutingPath,
        [string] $ShippedPath = (Join-Path (Get-VmrQBPaths).KnowledgeRoot 'routing.json'),
        [string] $BasePath = (Join-Path (Split-Path -Parent $LocalPath) 'routing.base.json')
    )
    if (Test-Path -LiteralPath $LocalPath) { return $false }
    if (-not (Test-Path -LiteralPath $ShippedPath -PathType Leaf)) { return $false }
    $hash = Get-VmrQBShippedRoutingHash -Path $ShippedPath
    $note = 'Your own copy of knowledge/routing.json (git-ignored), made once on first use. Edit it freely: it is merged over the shipped file, a category with the same id replaces the shipped one, and channelRules merge by key. Pulling does not update it; baseSha256 is the shipped file it was copied from, and ./daily/Invoke-Preflight.ps1 says when the shipped file has changed since.'
    $lines = foreach ($line in [IO.File]::ReadAllLines($ShippedPath)) {
        if ($line -match '^\s*"\$schema":') { '  "$schema": ' + (ConvertTo-Json -InputObject $note) + ','; continue }
        $line
        if ($line -match '^\s*"schemaVersion":') { "  `"baseSha256`": `"$hash`"," }
    }
    $temp = "$LocalPath.$([guid]::NewGuid().ToString('N')).tmp"
    try {
        [void][IO.Directory]::CreateDirectory((Split-Path -Parent $LocalPath))
        [IO.File]::WriteAllText($temp, (($lines -join "`n") + "`n"), [Text.UTF8Encoding]::new($false))
        # The pristine shipped text the copy came from. With it, a later change to the shipped file can be told apart from
        # your own edits (./common/Invoke-Routing.ps1 diff). Two first uses write identical content, so overwriting is safe.
        $baseTemp = "$BasePath.$([guid]::NewGuid().ToString('N')).tmp"
        try {
            [IO.File]::Copy($ShippedPath, $baseTemp, $true)
            [IO.File]::Move($baseTemp, $BasePath, $true)
        }
        finally { if (Test-Path -LiteralPath $baseTemp) { Remove-Item -LiteralPath $baseTemp -Force -ErrorAction SilentlyContinue } }
        # A move that refuses to overwrite: two processes starting together cannot clobber each other or an edited file.
        [IO.File]::Move($temp, $LocalPath)
        return $true
    }
    catch {
        if (Test-Path -LiteralPath $LocalPath -PathType Leaf) { return $false }
        throw "Could not create your local routing copy at ${LocalPath}: $($_.Exception.Message) Make that folder writable, or set VMR_QB_HOME to a folder that is."
    }
    finally { if (Test-Path -LiteralPath $temp) { Remove-Item -LiteralPath $temp -Force -ErrorAction SilentlyContinue } }
}

function Test-VmrQBLocalRoutingBehind {
    # True when your local routing copy was made from an older shipped routing.json. A file you wrote by hand (no baseSha256) is never "behind".
    [CmdletBinding()]
    param(
        [string] $LocalPath = (Get-VmrQBPaths).LocalRoutingPath,
        [string] $ShippedPath = (Join-Path (Get-VmrQBPaths).KnowledgeRoot 'routing.json')
    )
    if (-not (Test-Path -LiteralPath $LocalPath) -or -not (Test-Path -LiteralPath $ShippedPath)) { return $false }
    try { $base = [string](Get-VmrQBRoutingField (Get-Content -LiteralPath $LocalPath -Raw | ConvertFrom-Json) 'baseSha256') } catch { return $false }
    return ($base -and $base -ne (Get-VmrQBShippedRoutingHash -Path $ShippedPath))
}

# --- Closest match -------------------------------------------------------------------------------------------------
# A team or repo name that is not in the Guide (any more) is matched to the closest one instead of being rejected: a team that was
# renamed still contains the old name's words. The match has to be unique (the most shared words wins); with two equally close
# names, or none sharing a word, there is no match, and the caller keeps the name as written and says so. Every match is listed in
# the run notes, so a wrong guess is visible.
$script:VmrQBNameStopWords = @('dotnet', 'team', 'teams', 'the', 'and', 'of')

function Get-VmrQBNameTokens {
    param([string] $Name)
    $slug = ([string]$Name) -replace '^@[^/]*/', ''
    return @($slug.ToLowerInvariant() -split '[-._\s]+' | Where-Object { $_ -and $_ -notin $script:VmrQBNameStopWords })
}

function Test-VmrQBTokenMatch {
    param([string] $Left, [string] $Right)
    if ($Left -eq $Right) { return $true }
    # One is the start of the other (infra / infrastructure), for words of at least four letters.
    return ([Math]::Min($Left.Length, $Right.Length) -ge 4 -and ($Left.StartsWith($Right) -or $Right.StartsWith($Left)))
}

function Find-VmrQBClosestName {
    <#
    .SYNOPSIS
    The one candidate that shares the most words with Name, or $null when there is no unique close match.
    #>
    [CmdletBinding()]
    param([Parameter(Mandatory)][string] $Name, [Parameter(Mandatory)][AllowEmptyCollection()][string[]] $Candidate)
    $wanted = @(Get-VmrQBNameTokens $Name)
    if (-not $wanted.Count) { return $null }
    $scored = foreach ($option in $Candidate) {
        $tokens = @(Get-VmrQBNameTokens $option)
        if (-not $tokens.Count) { continue }
        $shared = 0
        foreach ($word in $wanted) { if (@($tokens | Where-Object { Test-VmrQBTokenMatch $word $_ }).Count) { $shared++ } }
        if ($shared -eq 0) { continue }
        [pscustomobject]@{ Name = $option; Score = ($shared * 100) - (($wanted.Count - $shared) * 20) - (($tokens.Count - $shared) * 2) }
    }
    $scored = @($scored)
    if (-not $scored.Count) { return $null }
    $best = ($scored | Measure-Object Score -Maximum).Maximum
    $top = @($scored | Where-Object Score -eq $best)
    if ($top.Count -ne 1) { return $null }
    return [string]$top[0].Name
}

function Find-VmrQBClosestTeam {
    [CmdletBinding()]
    param([Parameter(Mandatory)][string] $Handle, [Parameter(Mandatory)][AllowEmptyCollection()][string[]] $Team)
    return Find-VmrQBClosestName -Name $Handle -Candidate $Team
}

# --- The ownership copy ----------------------------------------------------------------------------------------------
# The Guide's Repo Ownership table is read from the checkout this skill lives in (it arrives with a pull). The first time it is
# read, a copy of what it says (repo -> team) is saved in the git-ignored local folder with the Guide's hash. The copy is renewed
# only when the Guide file changes (you pulled) or on ./common/Invoke-Routing.ps1 refresh-ownership, never on a schedule, and it
# is what the round falls back to when the Guide cannot be read. When there is neither a Guide in the checkout nor a copy, the
# Guide is fetched from GitHub main once, and the copy is made from that.
$script:VmrQBGuideUrl = 'https://raw.githubusercontent.com/dotnet/dotnet/main/docs/VMR-QB-Guide.md'
$script:VmrQBGuideFetcher = {
    param($Url)
    $response = Invoke-WebRequest -Uri $Url -UseBasicParsing -TimeoutSec 15 -ErrorAction Stop
    return [string]$response.Content
}

function Get-VmrQBTextSha256 {
    param([Parameter(Mandatory)][AllowEmptyString()][string] $Text)
    $sha = [Security.Cryptography.SHA256]::Create()
    try { return ([BitConverter]::ToString($sha.ComputeHash([Text.UTF8Encoding]::new($false).GetBytes($Text))) -replace '-', '').ToLowerInvariant() }
    finally { $sha.Dispose() }
}

function Read-VmrQBGuideOwnershipCopy {
    # The saved ownership copy, or $null when there is none or it cannot be read.
    [CmdletBinding()]
    param([Parameter(Mandatory)][string] $Path)
    if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) { return $null }
    try {
        $document = Get-Content -LiteralPath $Path -Raw | ConvertFrom-Json -Depth 20
        if ([int](Get-VmrQBRoutingField $document 'schemaVersion' 0) -ne 1) { return $null }
        $rows = @(@(Get-VmrQBRoutingField $document 'rows' @()) | ForEach-Object {
                $team = [string](Get-VmrQBRoutingField $_ 'team'); $repo = [string](Get-VmrQBRoutingField $_ 'repo')
                if ($repo -and $team -match '^@([A-Za-z0-9._-]+)/([A-Za-z0-9._-]+)$') {
                    [pscustomobject]@{ Repo = $repo; Team = $team; Org = $Matches[1]; Slug = $Matches[2]; Url = "https://github.com/orgs/$($Matches[1])/teams/$($Matches[2])" }
                }
            })
        if (-not $rows.Count) { return $null }
        return [pscustomobject]@{
            Rows = $rows; Sha256 = [string](Get-VmrQBRoutingField $document 'sha256'); Source = [string](Get-VmrQBRoutingField $document 'source')
            CapturedUtc = [string](Get-VmrQBRoutingField $document 'capturedAtUtc')
        }
    }
    catch { return $null }
}

function Write-VmrQBGuideOwnershipCopy {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][string] $Path, [Parameter(Mandatory)][object[]] $Row, [Parameter(Mandatory)][string] $Sha256,
        [Parameter(Mandatory)][string] $Source, [string] $SourcePath, [string] $CheckoutCommit
    )
    $document = [ordered]@{
        schemaVersion = 1
        note          = 'Made from the Repo Ownership table of docs/VMR-QB-Guide.md the first time it was read, and renewed only when that file changes (a pull) or on ./common/Invoke-Routing.ps1 refresh-ownership. The daily round falls back to it when the Guide cannot be read. Git-ignored; safe to delete.'
        source        = $Source; sourcePath = $SourcePath; sha256 = $Sha256; checkoutCommit = $CheckoutCommit
        capturedAtUtc = [DateTime]::UtcNow.ToString('yyyy-MM-dd HH:mm') + ' UTC'
        rows          = @($Row | ForEach-Object { [ordered]@{ repo = $_.Repo; team = $_.Team } })
    }
    [void][IO.Directory]::CreateDirectory((Split-Path -Parent $Path))
    $temp = "$Path.$([guid]::NewGuid().ToString('N')).tmp"
    try {
        [IO.File]::WriteAllText($temp, ((ConvertTo-Json -InputObject $document -Depth 6).Replace("`r`n", "`n") + "`n"), [Text.UTF8Encoding]::new($false))
        [IO.File]::Move($temp, $Path, $true)
    }
    finally { if (Test-Path -LiteralPath $temp) { Remove-Item -LiteralPath $temp -Force -ErrorAction SilentlyContinue } }
}

function Get-VmrQBCheckoutCommit {
    param([string] $Directory)
    try {
        $commit = (& git -C $Directory rev-parse HEAD 2>$null)
        if ($LASTEXITCODE -eq 0 -and "$commit" -match '^[0-9a-f]{40}$') { return "$commit" }
    }
    catch { Write-Verbose "No git commit recorded: $($_.Exception.Message)" }
    return ''
}

function Resolve-VmrQBGuideOwnership {
    <#
    .SYNOPSIS
    The Repo Ownership rows the round uses, and where they came from. It does not throw (except with -Strict).
    .DESCRIPTION
    -Strict: the Guide of the checkout or an error (./verify, the tests). -UseCopy: the daily round's order is the Guide of this
    checkout (the copy is made, or renewed when the Guide's hash changed), then the saved copy, then one fetch from GitHub main,
    then nothing, each step saying why in Warnings. Without -UseCopy a Guide named by -GuidePath is read and nothing else is touched.
    Source is checkout, copy, github, file or none.
    #>
    [CmdletBinding()]
    param([string] $GuidePath, [switch] $UseCopy, [switch] $Strict, [string] $CopyPath)
    $warnings = [Collections.Generic.List[string]]::new()
    if ($Strict) {
        return [pscustomobject]@{ Rows = @(Get-VmrQBGuideOwnershipRows -GuidePath $GuidePath); Source = 'checkout'; CapturedUtc = ''; Warnings = @() }
    }
    if (-not $UseCopy) {
        try { $rows = @(Get-VmrQBGuideOwnershipRows -GuidePath $GuidePath) }
        catch { $rows = @(); $warnings.Add("Guide ownership is unavailable: $($_.Exception.Message)") }
        return [pscustomobject]@{ Rows = $rows; Source = $(if ($rows.Count) { 'file' } else { 'none' }); CapturedUtc = ''; Warnings = @($warnings) }
    }
    if (-not $CopyPath) { $CopyPath = (Get-VmrQBPaths).GuideOwnershipPath }
    $guideFile = if ($GuidePath) { $GuidePath } else { Join-Path (Get-VmrQBPackageRoot) 'docs\VMR-QB-Guide.md' }
    $checkoutProblem = ''
    try {
        $guide = Get-VmrQBGuideText -GuidePath $guideFile
        $rows = @(ConvertFrom-VmrQBGuideOwnership -Text $guide.Text)
        $sha = Get-VmrQBTextSha256 -Text $guide.Text
        $saved = Read-VmrQBGuideOwnershipCopy -Path $CopyPath
        if ($null -eq $saved -or $saved.Sha256 -ne $sha) {
            try {
                Write-VmrQBGuideOwnershipCopy -Path $CopyPath -Row $rows -Sha256 $sha -Source 'checkout' -SourcePath $guideFile `
                    -CheckoutCommit (Get-VmrQBCheckoutCommit -Directory (Split-Path -Parent $guideFile))
            }
            catch { $warnings.Add("The ownership copy could not be saved ($($_.Exception.Message)); the Guide of this checkout is used directly.") }
        }
        return [pscustomobject]@{ Rows = $rows; Source = 'checkout'; CapturedUtc = ''; Warnings = @($warnings) }
    }
    catch { $checkoutProblem = $_.Exception.Message }

    $saved = Read-VmrQBGuideOwnershipCopy -Path $CopyPath
    if ($null -ne $saved) {
        $warnings.Add("The Guide could not be read in this checkout ($checkoutProblem) Owners come from the ownership copy made $($saved.CapturedUtc) from $($saved.Source); confirm an owner before pinging.")
        return [pscustomobject]@{ Rows = $saved.Rows; Source = 'copy'; CapturedUtc = $saved.CapturedUtc; Warnings = @($warnings) }
    }
    try {
        $text = [string](& $script:VmrQBGuideFetcher $script:VmrQBGuideUrl)
        $rows = @(ConvertFrom-VmrQBGuideOwnership -Text $text)
        try { Write-VmrQBGuideOwnershipCopy -Path $CopyPath -Row $rows -Sha256 (Get-VmrQBTextSha256 -Text $text) -Source 'github-main' -SourcePath $script:VmrQBGuideUrl }
        catch { $warnings.Add("The ownership copy could not be saved ($($_.Exception.Message)).") }
        $warnings.Add('The Guide was not in this checkout, so it was fetched once from GitHub main and saved as your ownership copy. It is reused from now on; ./common/Invoke-Routing.ps1 refresh-ownership renews it.')
        return [pscustomobject]@{ Rows = $rows; Source = 'github'; CapturedUtc = ''; Warnings = @($warnings) }
    }
    catch {
        $warnings.Add("Owners are unavailable: the Guide could not be read in this checkout ($checkoutProblem) and fetching it once from GitHub main failed ($($_.Exception.Message)). Every owner is shown as unidentified. Run the skill from a dotnet/dotnet checkout, or run ./common/Invoke-Routing.ps1 refresh-ownership when GitHub is reachable.")
        return [pscustomobject]@{ Rows = @(); Source = 'none'; CapturedUtc = ''; Warnings = @($warnings) }
    }
}

function Update-VmrQBGuideOwnershipCopy {
    <#
    .SYNOPSIS
    Renews the saved ownership copy on request: from the Guide of this checkout, or with -FromGitHub from GitHub main.
    #>
    [CmdletBinding()]
    param([switch] $FromGitHub, [string] $GuidePath, [string] $CopyPath = (Get-VmrQBPaths).GuideOwnershipPath)
    $guideFile = if ($GuidePath) { $GuidePath } else { Join-Path (Get-VmrQBPackageRoot) 'docs\VMR-QB-Guide.md' }
    if ($FromGitHub) {
        $text = [string](& $script:VmrQBGuideFetcher $script:VmrQBGuideUrl)
        $source = 'github-main'; $sourcePath = $script:VmrQBGuideUrl; $commit = ''
    }
    else {
        $guide = Get-VmrQBGuideText -GuidePath $guideFile
        $text = $guide.Text; $source = 'checkout'; $sourcePath = $guideFile; $commit = Get-VmrQBCheckoutCommit -Directory (Split-Path -Parent $guideFile)
    }
    $rows = @(ConvertFrom-VmrQBGuideOwnership -Text $text)
    Write-VmrQBGuideOwnershipCopy -Path $CopyPath -Row $rows -Sha256 (Get-VmrQBTextSha256 -Text $text) -Source $source -SourcePath $sourcePath -CheckoutCommit $commit
    return [pscustomobject]@{ Path = $CopyPath; Source = $source; Rows = $rows.Count }
}

function Get-VmrQBRouting {
    <#
    .SYNOPSIS
    Loads the routing registry (shipped + optional local additions) and the Guide's repo owners.
    .DESCRIPTION
    Repo -> owner and the real teams come from the Repo Ownership table of docs/VMR-QB-Guide.md in the checkout this skill lives
    in (read from disk; see Resolve-VmrQBGuideOwnership for the saved copy it falls back to). A 'repoOwners' or 'teams' entry in a
    routing file adds to or overrides them.

    The load never fails because of an owner. Anything that does not fit (a local category naming a team that left the Guide, a
    redirect whose target was renamed, a routing file that does not parse) is matched to the closest team, kept as written, or
    dropped, and each case is listed in Warnings. -Strict is for ./verify, the tests and every edit of your local routing copy:
    it throws at the first problem instead, and reads the Guide of the checkout only.
    .OUTPUTS
    Teams, HandleAliases, PreferredRedirects, RepoOwners, Categories, Channels, ChannelRules, SupportDestinations,
    VmrTestsOwner, UnmappedLabel, VerifiedOn, Warnings, GuideSource.
    #>
    [CmdletBinding()]
    param([string] $Path, [string] $LocalPath, [string] $GuidePath, [switch] $Strict)

    $defaultFiles = -not $Path -and -not $PSBoundParameters.ContainsKey('LocalPath')
    $defaultGuide = -not $PSBoundParameters.ContainsKey('GuidePath')
    if (-not $Path) { $Path = Join-Path (Get-VmrQBPaths).KnowledgeRoot 'routing.json' }
    if (-not $PSBoundParameters.ContainsKey('LocalPath')) { $LocalPath = (Get-VmrQBPaths).LocalRoutingPath }
    $problems = [Collections.Generic.List[string]]::new()
    # First use on a clone: make your own copy once. It lives in the git-ignored local folder.
    if ($defaultFiles) {
        try { [void](Initialize-VmrQBLocalRouting -LocalPath $LocalPath -ShippedPath $Path) }
        catch { if ($Strict) { throw }; $problems.Add($_.Exception.Message) }
    }
    $useGuideCopy = $defaultGuide -and -not $Strict
    $copyPath = if ($useGuideCopy) { (Get-VmrQBPaths).GuideOwnershipPath } else { $null }
    if (-not $GuidePath) { $GuidePath = Join-Path (Get-VmrQBPackageRoot) 'docs\VMR-QB-Guide.md' }
    $hasShipped = Test-Path -LiteralPath $Path -PathType Leaf
    if (-not $hasShipped -and $Strict) { throw "Routing registry not found: $Path" }
    $hasLocal = $LocalPath -and (Test-Path -LiteralPath $LocalPath -PathType Leaf)
    $stampOf = {
        $parts = [Collections.Generic.List[string]]::new()
        foreach ($file in @($Path, $LocalPath, $GuidePath, $copyPath)) {
            if ($file -and (Test-Path -LiteralPath $file -PathType Leaf)) { $parts.Add([string](Get-Item -LiteralPath $file).LastWriteTimeUtc.Ticks) } else { $parts.Add('-') }
        }
        return ($parts -join ':')
    }
    $key = "$Path|$LocalPath|$GuidePath|$Strict"
    $stamp = & $stampOf
    if ($script:VmrQBRoutingCache.ContainsKey($key) -and $script:VmrQBRoutingCache[$key].stamp -eq $stamp) {
        return $script:VmrQBRoutingCache[$key].value
    }
    # A problem is thrown in strict mode and recorded otherwise; the caller then applies its fallback.
    $fail = { param([string] $Message) if ($Strict) { throw $Message }; $problems.Add($Message) }

    $sources = [Collections.Generic.List[object]]::new()
    $sourceNames = [Collections.Generic.List[string]]::new()
    if ($hasShipped) {
        try { $sources.Add((Read-VmrQBRoutingFile -Path $Path -Label 'Shipped')); $sourceNames.Add('shipped') }
        catch { if ($Strict) { throw }; $problems.Add("The shipped routing file was not used: $($_.Exception.Message)") }
    }
    else { $problems.Add("The shipped routing file was not found ($Path); only the Guide's owners are in effect.") }
    if ($hasLocal) {
        try { $sources.Add((Read-VmrQBRoutingFile -Path $LocalPath -Label 'Local')); $sourceNames.Add('local') }
        catch { if ($Strict) { throw }; $problems.Add("Your local routing copy was not used: $($_.Exception.Message) Fix it, or move it aside and a fresh copy is made.") }
    }

    $teams = [Collections.Generic.List[string]]::new()
    $aliases = [ordered]@{}
    $redirects = [ordered]@{}
    $repoOwners = [ordered]@{}
    # The Guide is the source of truth for repo -> owner and for which teams exist, so none of that is
    # copied into the registry.
    $guide = Resolve-VmrQBGuideOwnership -GuidePath $GuidePath -UseCopy:$useGuideCopy -Strict:$Strict -CopyPath $copyPath
    foreach ($warning in @($guide.Warnings)) { $problems.Add([string]$warning) }
    $guideRows = @($guide.Rows)
    $guideOwners = ConvertTo-VmrQBRepoOwnerMap -Rows $guideRows
    foreach ($repo in $guideOwners.Keys) { $repoOwners[$repo] = $guideOwners[$repo] }
    $guideTeams = [Collections.Generic.List[string]]::new()
    foreach ($row in $guideRows) {
        if ($row.Team -match '^@dotnet/' -and -not ($guideTeams | Where-Object { $_ -ieq $row.Team })) { $guideTeams.Add($row.Team); $teams.Add($row.Team) }
    }
    $channels = [ordered]@{}
    $channelRules = [ordered]@{}
    $categories = [ordered]@{}
    $categorySource = @{}
    $support = [ordered]@{}
    $scalars = @{ vmrTestsOwner = $null; unmappedLabel = $null; verifiedOn = $null }
    for ($sourceIndex = 0; $sourceIndex -lt $sources.Count; $sourceIndex++) {
        $source = $sources[$sourceIndex]
        foreach ($team in @(Get-VmrQBRoutingField $source 'teams' @())) {
            if (([string]$team) -notmatch '^@dotnet/[A-Za-z0-9][A-Za-z0-9._-]*$') { & $fail "Routing registry team '$team' is not an @dotnet/<slug> handle."; continue }
            if (-not ($teams | Where-Object { $_ -ieq $team })) { $teams.Add([string]$team) }
        }
        foreach ($pair in @((Get-VmrQBRoutingField $source 'handleAliases' ([pscustomobject]@{})).PSObject.Properties)) { $aliases[$pair.Name] = [string]$pair.Value }
        foreach ($pair in @((Get-VmrQBRoutingField $source 'preferredRedirects' ([pscustomobject]@{})).PSObject.Properties)) { $redirects[$pair.Name] = [string]$pair.Value }
        foreach ($pair in @((Get-VmrQBRoutingField $source 'repoOwners' ([pscustomobject]@{})).PSObject.Properties)) { $repoOwners[$pair.Name] = [string]$pair.Value }
        foreach ($pair in @((Get-VmrQBRoutingField $source 'channels' ([pscustomobject]@{})).PSObject.Properties)) { $channels[$pair.Name] = [string]$pair.Value }
        foreach ($pair in @((Get-VmrQBRoutingField $source 'channelRules' ([pscustomobject]@{})).PSObject.Properties)) { $channelRules[$pair.Name] = [string]$pair.Value }
        foreach ($category in @(Get-VmrQBRoutingField $source 'categories' @())) {
            $id = [string](Get-VmrQBRoutingField $category 'id')
            if (-not $id) { & $fail 'Routing registry category has no id.'; continue }
            $categories[$id] = $category
            $categorySource[$id] = $sourceNames[$sourceIndex]
        }
        foreach ($destination in @(Get-VmrQBRoutingField $source 'supportDestinations' @())) {
            $id = [string](Get-VmrQBRoutingField $destination 'id')
            if (-not $id) { & $fail 'Routing registry support destination has no id.'; continue }
            $support[$id] = $destination
        }
        foreach ($name in @($scalars.Keys)) {
            $value = Get-VmrQBRoutingField $source $name
            if ($null -ne $value) { $scalars[$name] = [string]$value }
        }
    }

    # The Guide wins: a handle it lists as a team is real, so a stale 404 alias or redirect for it is dropped.
    foreach ($team in $guideTeams) {
        foreach ($stale in @($aliases.Keys | Where-Object { $_ -ieq $team })) { $aliases.Remove($stale) }
        foreach ($stale in @($redirects.Keys | Where-Object { $_ -ieq $team })) { $redirects.Remove($stale) }
    }

    # An alias or owner that points at a team that is not in the registry would silently route to nowhere. Strict mode refuses to
    # load; otherwise the closest team is used, and when nothing is close the name is kept as written (an owner) or dropped (an
    # alias or redirect), and the Warnings say which.
    $unmatched = [Collections.Generic.HashSet[string]]::new([StringComparer]::OrdinalIgnoreCase)
    $canonical = { param($handle) [bool]($teams | Where-Object { $_ -ieq $handle }) }
    $known = { param($handle) ((& $canonical $handle) -or $unmatched.Contains([string]$handle)) }
    $closest = { param($handle) Find-VmrQBClosestTeam -Handle ([string]$handle) -Team @($teams) }
    foreach ($alias in @($aliases.Keys)) {
        if (-not (& $canonical $aliases[$alias])) {
            & $fail "Routing registry alias $alias points at unknown team $($aliases[$alias])."
            $match = & $closest $aliases[$alias]
            if ($match) { $problems.Add("Alias $alias pointed at $($aliases[$alias]), which is not a team in the Guide; using the closest team $match."); $aliases[$alias] = $match }
            else { $problems.Add("Alias $alias was dropped: $($aliases[$alias]) is not a team in the Guide and nothing is close to it."); $aliases.Remove($alias); continue }
        }
        if (& $canonical $alias) {
            & $fail "Routing registry alias $alias is also listed as a real team."
            $problems.Add("Alias $alias was dropped: it is also a real team."); $aliases.Remove($alias)
        }
    }
    foreach ($redirect in @($redirects.Keys)) {
        if (-not (& $canonical $redirects[$redirect])) {
            & $fail "Routing registry redirect $redirect points at unknown team $($redirects[$redirect])."
            $match = & $closest $redirects[$redirect]
            if ($match) { $problems.Add("Redirect $redirect pointed at $($redirects[$redirect]), which is not a team in the Guide; using the closest team $match."); $redirects[$redirect] = $match }
            else { $problems.Add("Redirect $redirect was dropped: $($redirects[$redirect]) is not a team in the Guide and nothing is close to it, so $redirect stays as written."); $redirects.Remove($redirect); continue }
        }
        if (& $canonical $redirect) {
            & $fail "Routing registry redirect $redirect is also listed as a real team."
            $problems.Add("Redirect $redirect was dropped: it is a real team."); $redirects.Remove($redirect); continue
        }
        if ($aliases.Contains($redirect)) {
            & $fail "Routing registry handle $redirect is both an alias and a redirect."
            $problems.Add("$redirect was both an alias and a redirect; the redirect is used."); $aliases.Remove($redirect)
        }
    }
    foreach ($repo in @($repoOwners.Keys)) {
        if (-not (& $canonical $repoOwners[$repo])) {
            & $fail "Routing registry repo '$repo' is owned by unknown team $($repoOwners[$repo])."
            $match = & $closest $repoOwners[$repo]
            if ($match) { $problems.Add("Repo '$repo' was owned by $($repoOwners[$repo]), which is not a team in the Guide; using the closest team $match."); $repoOwners[$repo] = $match }
            else { $problems.Add("Repo '$repo' is owned by $($repoOwners[$repo]), which is not a team in the Guide and nothing is close to it; kept as written."); [void]$unmatched.Add([string]$repoOwners[$repo]) }
        }
    }
    # A support destination is an external place to take a failure that has no GitHub team (for example 1ES support).
    # Its name is written into report text and matched by the validator, so it must be plain text and its link https.
    foreach ($id in @($support.Keys)) {
        $supportName = [string](Get-VmrQBRoutingField $support[$id] 'name')
        $supportUrl = [string](Get-VmrQBRoutingField $support[$id] 'url')
        if ([string]::IsNullOrWhiteSpace($supportName) -or $supportName -match '[\[\]()"|<>\r\n,]') { & $fail "Routing registry support destination '$id' needs a plain-text name (no brackets, quotes, commas or pipes)."; $support.Remove($id); continue }
        if ($supportUrl -notmatch '^https://[^\s()<>"]+$') { & $fail "Routing registry support destination '$id' needs an https url."; $support.Remove($id); continue }
    }
    # A category's owners are the owners of the repos it names (ownerRepos, read from the Guide just like everything
    # else about repos, so a Guide change reaches the category) followed by any teams it names outright (owners).
    $effectiveCategories = [ordered]@{}
    foreach ($id in $categories.Keys) {
        $category = $categories[$id]
        $origin = $categorySource[$id]
        $fix = if ($origin -eq 'local') { ' Your local routing copy holds this category: compare it with the shipped file using ./common/Invoke-Routing.ps1 diff, then fix or adopt it.' } else { '' }
        $owners = [Collections.Generic.List[string]]::new()
        foreach ($repo in @(Get-VmrQBRoutingField $category 'ownerRepos' @())) {
            $repoKey = @($repoOwners.Keys | Where-Object { $_ -ieq [string]$repo }) | Select-Object -First 1
            if (-not $repoKey) {
                & $fail "Routing registry category '$id' ($origin routing file) names repo '$repo', which has no owner in the Guide's Repo Ownership table.$fix"
                $repoMatch = Find-VmrQBClosestName -Name ([string]$repo) -Candidate @($repoOwners.Keys)
                if ($repoMatch) { $problems.Add("Category '$id' ($origin routing file) names repo '$repo', which is not in the Guide's Repo Ownership table; using the closest repo '$repoMatch'."); $repoKey = $repoMatch }
                else { $problems.Add("Category '$id' ($origin routing file) names repo '$repo', which is not in the Guide's Repo Ownership table and nothing is close to it; that repo was skipped."); continue }
            }
            $handle = [string]$repoOwners[$repoKey]
            if (-not ($owners | Where-Object { $_ -ieq $handle })) { $owners.Add($handle) }
        }
        foreach ($owner in @(Get-VmrQBRoutingField $category 'owners' @())) {
            $ownerHandle = [string]$owner
            if (-not (& $known $ownerHandle)) {
                & $fail "Routing registry category '$id' ($origin routing file) names unknown team $owner.$fix"
                $match = & $closest $ownerHandle
                if ($match) { $problems.Add("Category '$id' ($origin routing file) names $ownerHandle, which is not a team in the Guide; using the closest team $match."); $ownerHandle = $match }
                else { $problems.Add("Category '$id' ($origin routing file) names $ownerHandle, which is not a team in the Guide and nothing is close to it; kept as written."); [void]$unmatched.Add($ownerHandle) }
            }
            if (-not ($owners | Where-Object { $_ -ieq $ownerHandle })) { $owners.Add($ownerHandle) }
        }
        $copy = [ordered]@{}
        foreach ($property in $category.PSObject.Properties) { $copy[$property.Name] = $property.Value }
        $copy['owners'] = @($owners)
        $supportRef = [string](Get-VmrQBRoutingField $categories[$id] 'support')
        if ($supportRef -and -not $support.Contains($supportRef)) {
            & $fail "Routing registry category '$id' names unknown support destination '$supportRef'."
            $problems.Add("Category '$id' names the support destination '$supportRef', which does not exist; the reference was dropped."); $copy.Remove('support')
        }
        $effectiveCategories[$id] = [pscustomobject]$copy
    }
    foreach ($name in 'vmrTestsOwner') {
        if ($scalars[$name] -and -not (& $known $scalars[$name])) {
            & $fail "Routing registry $name is not a known team: $($scalars[$name])."
            $match = & $closest $scalars[$name]
            if ($match) { $problems.Add("$name was $($scalars[$name]), which is not a team in the Guide; using the closest team $match."); $scalars[$name] = $match }
            else { $problems.Add("$name is $($scalars[$name]), which is not a team in the Guide and nothing is close to it; kept as written."); [void]$unmatched.Add([string]$scalars[$name]) }
        }
    }

    # A repo name used as a handle (@dotnet/runtime) is not a team, and the Guide's table already says who
    # owns that repo, so the handle resolves to that owner. Nothing is listed by hand; a handle written
    # explicitly in a routing file wins, and a real team or a redirect is never turned into an alias.
    foreach ($repo in $repoOwners.Keys) {
        $handle = "@dotnet/$repo"
        if ($teams | Where-Object { $_ -ieq $handle }) { continue }
        if (@($redirects.Keys) + @($aliases.Keys) | Where-Object { $_ -ieq $handle }) { continue }
        $aliases[$handle] = $repoOwners[$repo]
    }

    $routing = [pscustomobject]@{
        Teams              = [string[]]@($teams)
        HandleAliases      = $aliases
        PreferredRedirects = $redirects
        RepoOwners         = $repoOwners
        Categories         = @($effectiveCategories.Values)
        Channels           = $channels
        ChannelRules       = $channelRules
        SupportDestinations = @($support.Keys | ForEach-Object {
                [pscustomobject]@{ Id = [string]$_; Name = [string]$support[$_].name; Url = [string]$support[$_].url; Note = [string](Get-VmrQBRoutingField $support[$_] 'note') }
            })
        VmrTestsOwner      = $scalars.vmrTestsOwner
        UnmappedLabel      = if ($scalars.unmappedLabel) { $scalars.unmappedLabel } else { 'No identified owner' }
        VerifiedOn         = $scalars.verifiedOn
        Warnings           = [string[]]@($problems)
        GuideSource        = [string]$guide.Source
    }
    $script:VmrQBRoutingCache[$key] = @{ stamp = (& $stampOf); value = $routing }
    return $routing
}

function Get-VmrQBKnownTeams {
    [CmdletBinding()]
    param([string] $Path, [string] $LocalPath)
    $arguments = @{}
    if ($Path) { $arguments.Path = $Path }
    if ($PSBoundParameters.ContainsKey('LocalPath')) { $arguments.LocalPath = $LocalPath }
    return @((Get-VmrQBRouting @arguments).Teams)
}

function Resolve-VmrQBOwnerTeam {
    <#
    .SYNOPSIS
    Canonicalizes an owner handle.
    .DESCRIPTION
    Returns the canonical handle for a real team, the real team for a known mistaken handle
    (@dotnet/runtime -> @dotnet/dnr-codeflow), the handle unchanged for an individual
    (@login, no slash) or for text that is not a handle at all, and $null for an @dotnet/*
    handle that is neither a team nor a known mistake. $null means "do not ping this".
    #>
    [CmdletBinding()]
    param([Parameter(Mandatory)][AllowEmptyString()][string] $Handle, [string] $Path, [string] $LocalPath)
    $value = $Handle.Trim()
    if (-not $value.StartsWith('@') -or -not $value.Contains('/')) { return $value }
    $arguments = @{}
    if ($Path) { $arguments.Path = $Path }
    if ($PSBoundParameters.ContainsKey('LocalPath')) { $arguments.LocalPath = $LocalPath }
    $routing = Get-VmrQBRouting @arguments
    foreach ($team in $routing.Teams) { if ($team -ieq $value) { return $team } }
    foreach ($alias in $routing.HandleAliases.Keys) { if ($alias -ieq $value) { return $routing.HandleAliases[$alias] } }
    foreach ($redirect in $routing.PreferredRedirects.Keys) { if ($redirect -ieq $value) { return $routing.PreferredRedirects[$redirect] } }
    return $null
}

function Test-VmrQBTeamHandle {
    [CmdletBinding()]
    param([Parameter(Mandatory)][string] $Handle, [switch] $IncludeAliases)
    $routing = Get-VmrQBRouting
    foreach ($team in $routing.Teams) { if ($team -ieq $Handle.Trim()) { return $true } }
    if ($IncludeAliases) {
        foreach ($alias in $routing.HandleAliases.Keys) { if ($alias -ieq $Handle.Trim()) { return $true } }
        foreach ($redirect in $routing.PreferredRedirects.Keys) { if ($redirect -ieq $Handle.Trim()) { return $true } }
    }
    return $false
}

function Find-VmrQBUnknownHandles {
    <#
    .SYNOPSIS
    Lists every @dotnet/<team> mention in the text that is not a real team (mistaken aliases included).
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][AllowEmptyString()][string] $Text,
        # The registry to check against. A caller that already loaded the SHIPPED registry passes it, so a broken local copy
        # cannot stop a check that is about the shipped content.
        [object] $Routing
    )
    $found = [Collections.Generic.List[object]]::new()
    $routing = if ($Routing) { $Routing } else { Get-VmrQBRouting }
    foreach ($match in [regex]::Matches($Text, '@dotnet/[A-Za-z0-9][A-Za-z0-9._-]*')) {
        $handle = $match.Value.TrimEnd('.')
        if ($routing.Teams | Where-Object { $_ -ieq $handle }) { continue }
        $suggestion = $null; $kind = 'unknown'
        foreach ($alias in $routing.HandleAliases.Keys) { if ($alias -ieq $handle) { $suggestion = $routing.HandleAliases[$alias]; $kind = 'nonexistent' } }
        foreach ($redirect in $routing.PreferredRedirects.Keys) { if ($redirect -ieq $handle) { $suggestion = $routing.PreferredRedirects[$redirect]; $kind = 'redirect' } }
        if (-not $suggestion) {
            # Not a team, alias or redirect: the closest team, when exactly one is close, so the message names a real owner.
            $close = Find-VmrQBClosestTeam -Handle $handle -Team @($routing.Teams)
            if ($close) { $suggestion = $close }
        }
        $found.Add([pscustomobject]@{ handle = $handle; suggestion = $suggestion; kind = $kind; index = $match.Index })
    }
    return $found.ToArray()
}

function Get-VmrQBOwnershipBlock {
    <#
    .SYNOPSIS
    Renders the owner table the report prompts embed: repo owners from the Guide, the rest from the registry.
    #>
    [CmdletBinding()]
    param()
    $routing = Get-VmrQBRouting
    $repoOwners = $routing.RepoOwners
    # When the owners did not come from this checkout's Guide, or something did not match, the model is told to confirm before pinging.
    $ownerMapNote = if (@($routing.Warnings).Count) { 'Owner map notes (confirm an owner before pinging it): ' + ((@($routing.Warnings) | Select-Object -First 3) -join ' ') } else { '' }
    $repoLine = @($repoOwners.Keys | ForEach-Object { "$_ $($repoOwners[$_])" }) -join '; '
    $wrapped = [regex]::Replace($repoLine, '(.{1,95})(?:; |$)', "`$1;`n").TrimEnd(";`n".ToCharArray()) + '.'
    $nonexistent = @($routing.HandleAliases.Keys | ForEach-Object { "$_ (use $($routing.HandleAliases[$_]))" }) -join ', '
    $redirectText = @($routing.PreferredRedirects.Keys | ForEach-Object { "$_ -> $($routing.PreferredRedirects[$_])" }) -join ', '
    $supportText = (@($routing.SupportDestinations | ForEach-Object {
            "External support: for $($_.Note) write the owner as `"$($_.Name)`" and the destination as `"Support - [$($_.Name)]($($_.Url))`"; it is not a GitHub team and not a Teams channel."
        }) -join "`n")
    $channelLines = foreach ($name in $routing.Channels.Keys) {
        $channel = $routing.Channels[$name]
        $rule = if ($routing.ChannelRules.Contains($channel)) { $routing.ChannelRules[$channel] } else { '' }
        if ($rule) { "- `"$channel`" - $rule" } else { "- `"$channel`"" }
    }
    return @"
Repo -> owner (ping this GitHub team for repo-specific failures):
$wrapped
VMR-level tests and packaging under test/ (Microsoft.DotNet.Installer.Tests Linux deb/rpm
installer scenarios, Microsoft.DotNet.SourceBuild.Tests, Microsoft.DotNet.Tests) are owned in
this repo, not by a product repo: use $($routing.VmrTestsOwner).
Use ONLY the team handles listed above. Do not invent or infer a handle from a repository,
directory or component name. The handle is the token AFTER the repo name in the table, never
the repo name itself. These repo names are NOT teams: $nonexistent.
These handles are real but broad; always use the narrower owner instead: $redirectText.
If no listed owner clearly applies, name no team: write "$($routing.UnmappedLabel)" and say what evidence
is missing. Do not fall back to a convenient team because it sounds like infrastructure. VMR build-orchestration
config (timeouts, node reuse, repo-projects/Directory.Build.targets) has no team either: name the person who
authored the mechanism (git log / gh pr view on the file), or write "$($routing.UnmappedLabel)".
$supportText

Teams channels (route hot / broad issues here):
$($channelLines -join "`n")
$ownerMapNote
"@
}

function Get-VmrQBCategoryTable {
    <#
    .SYNOPSIS
    Renders the 'Mapped category / owner(s)' bullets for the build-health report instructions.
    #>
    [CmdletBinding()]
    param()
    $routing = Get-VmrQBRouting
    $lines = foreach ($category in $routing.Categories) {
        $owners = @(Get-VmrQBRoutingField $category 'owners' @())
        $ownerText = Get-VmrQBRoutingField $category 'ownerText'
        $supportDestination = @($routing.SupportDestinations | Where-Object { $_.Id -eq [string](Get-VmrQBRoutingField $category 'support') }) | Select-Object -First 1
        $label = if ($ownerText) { $ownerText }
                 elseif ($supportDestination) { "$($supportDestination.Name) ($($supportDestination.Url))" }
                 elseif ($owners.Count) { ($owners | ForEach-Object { "``$_``" }) -join ', ' }
                 else { "``$($routing.UnmappedLabel)``" }
        $extra = [Collections.Generic.List[string]]::new()
        $links = @(Get-VmrQBRoutingField $category 'links' @())
        if ($links.Count) { $extra.Add('link ' + ($links -join ' and ')) }
        $note = [string](Get-VmrQBRoutingField $category 'note')
        if ($note) { $extra.Add($note) }
        $suffix = if ($extra.Count) { '; ' + ($extra -join '; ') } else { '' }
        "- $($category.name): $label$suffix"
    }
    return (($lines + "- Unsupported matches: ``$($routing.UnmappedLabel)``") -join "`n")
}

function Get-VmrQBRoutingDocMarkdown {
    <#
    .SYNOPSIS
    The routing tables shown in the triage skill, rendered from the SHIPPED registry by default so one QB's local
    additions can never leak into a published doc. -Active renders the effective routing (shipped plus this QB's local
    copy) instead; only a private, per-run copy of the document may use it.
    #>
    [CmdletBinding()]
    param([switch] $Active)
    $routing = if ($Active) { Get-VmrQBRouting } else { Get-VmrQBRouting -LocalPath $null }
    $builder = [Text.StringBuilder]::new()
    [void]$builder.AppendLine('#### Owners the Guide does not cover (repo owners are in its Repo Ownership table)')
    [void]$builder.AppendLine()
    [void]$builder.AppendLine('| Area | Owner handle |')
    [void]$builder.AppendLine('|---|---|')
    [void]$builder.AppendLine("| VMR-level tests and packaging (``test/``) | ``$($routing.VmrTestsOwner)`` |")
    [void]$builder.AppendLine("| Anything else | ``$($routing.UnmappedLabel)`` - name no team. For VMR build-orchestration config (timeouts, node reuse, ``repo-projects/Directory.Build.targets``) name the person who authored the mechanism (``git log`` / ``gh pr view`` on the file). |")
    [void]$builder.AppendLine()
    if ($routing.PreferredRedirects.Count) {
        [void]$builder.AppendLine('#### Real but too broad. Use the narrower owner.')
        [void]$builder.AppendLine()
        [void]$builder.AppendLine('| Broad team | Use instead |')
        [void]$builder.AppendLine('|---|---|')
        foreach ($redirect in $routing.PreferredRedirects.Keys) { [void]$builder.AppendLine("| ``$redirect`` | ``$($routing.PreferredRedirects[$redirect])`` |") }
    }
    if ($routing.SupportDestinations.Count) {
        [void]$builder.AppendLine()
        [void]$builder.AppendLine('#### Support destinations (no GitHub team, not a Teams channel)')
        [void]$builder.AppendLine()
        [void]$builder.AppendLine('| Name | Link | When |')
        [void]$builder.AppendLine('|---|---|---|')
        foreach ($destination in $routing.SupportDestinations) { [void]$builder.AppendLine("| $($destination.Name) | $($destination.Url) | $($destination.Note) |") }
    }
    [void]$builder.AppendLine()
    [void]$builder.AppendLine('#### Where to post')
    [void]$builder.AppendLine()
    [void]$builder.AppendLine('| Channel | Rule |')
    [void]$builder.AppendLine('|---|---|')
    foreach ($key in $routing.Channels.Keys) {
        $channel = $routing.Channels[$key]
        $rule = if ($routing.ChannelRules.Contains($channel)) { $routing.ChannelRules[$channel] } else { '-' }
        [void]$builder.AppendLine("| $channel | $rule |")
    }
    return $builder.ToString().TrimEnd()
}

function Sync-VmrQBGeneratedBlock {
    <#
    .SYNOPSIS
    Replaces the text between <!-- BEGIN GENERATED: name --> and <!-- END GENERATED: name --> with fresh content,
    or (-Check) reports whether the file is already in sync.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][string] $Path,
        [Parameter(Mandatory)][string] $Name,
        [Parameter(Mandatory)][string] $Content,
        [switch] $Check
    )
    $text = [IO.File]::ReadAllText($Path)
    $newline = if ($text.Contains("`r`n")) { "`r`n" } else { "`n" }
    $pattern = "(?s)(?<begin><!-- BEGIN GENERATED: $([regex]::Escape($Name))[^>]*-->)(?<body>.*?)(?<end><!-- END GENERATED: $([regex]::Escape($Name)) -->)"
    $match = [regex]::Match($text, $pattern)
    if (-not $match.Success) { throw "$Path has no GENERATED block named '$Name' (expected BEGIN/END markers)." }
    $replacement = $match.Groups['begin'].Value + $newline + $newline + ($Content -replace "`r?`n", $newline) + $newline + $newline + $match.Groups['end'].Value
    $inSync = ($match.Value -replace "`r`n", "`n") -eq ($replacement -replace "`r`n", "`n")
    if ($Check) { return $inSync }
    if (-not $inSync) { [IO.File]::WriteAllText($Path, $text.Substring(0, $match.Index) + $replacement + $text.Substring($match.Index + $match.Length), [Text.UTF8Encoding]::new($false)) }
    return $true
}
function Expand-VmrQBInstructionTemplate {
    <#
    .SYNOPSIS
    Renders the model's instruction template (runner-instructions.md) from the routing registry.
    .DESCRIPTION
    {{CATEGORY_TABLE}} becomes the generated category/owner list, and {{REPORT_TIME_ZONE}} the time zone the report date follows
    (this computer's unless TimeZoneId says otherwise). Throws if the category placeholder is missing or any placeholder is left
    unexpanded, so a template edit can never reach the model half-rendered.
    #>
    [CmdletBinding()]
    param([Parameter(Mandatory)][string] $Template, [string] $TimeZoneId)
    if (-not $Template.Contains('{{CATEGORY_TABLE}}')) { throw 'The instruction template has no {{CATEGORY_TABLE}} placeholder.' }
    if ([string]::IsNullOrWhiteSpace($TimeZoneId)) { $TimeZoneId = [TimeZoneInfo]::Local.Id }
    $text = $Template.Replace('{{CATEGORY_TABLE}}', (Get-VmrQBCategoryTable)).Replace('{{REPORT_TIME_ZONE}}', $TimeZoneId)
    $left = [regex]::Match($text, '\{\{[A-Z_]+\}\}')
    if ($left.Success) { throw "The instruction template has an unexpanded placeholder: $($left.Value)" }
    return $text
}

# ---------------------------------------------------------------------------------------------
# core/Contacts.ps1
# ---------------------------------------------------------------------------------------------
# The team contact roster: who to name beside a team ping.
#
# A roster names people, so it is local data (<skill>/local, git-ignored), never shipped. It is built from two sources and
# cached so nobody has to curate it by hand:
#   1. the "Repo Ownership" table in docs/VMR-QB-Guide.md in this checkout (which teams matter; read from disk)
#   2. the GitHub team's maintainers (who to call), ranked by recent public activity
# The cache lives in the local folder (<skill>/local). It is built once. A later run asks GitHub only about a team
# that is new in the guide (you pulled a newer guide) or when you pass -Refresh; there is no timer. Entries a
# person wrote or edited (anything without source = "github-maintainers") are never overwritten unless -Force is given.


$script:VmrQBContactSource = 'github-maintainers'

function Test-VmrQBContactGenerated {
    # Generated entries may be rebuilt; anything else is a person's choice. The first roster (before
    # entries carried a source) recorded GitHub-derived contacts with the basis "team maintainer", so
    # those count as generated too; every other hand-written basis is kept.
    param($Contact)
    $source = [string](Get-VmrQBField $Contact 'source')
    if ($source) { return $source -eq $script:VmrQBContactSource }
    return ([string](Get-VmrQBField $Contact 'basis')).Trim() -eq 'team maintainer'
}

function Get-VmrQBContactsPath {
    <#
    .SYNOPSIS
    The team roster. It names people, so it lives only in the local folder (git-ignored).
    .PARAMETER ForWrite
    Accepted for older callers; the answer is always the local path.
    #>
    [CmdletBinding()]
    param([switch] $ForWrite)
    return Join-Path (Get-VmrQBPaths).LocalKnowledgeDir 'team-contacts.json'
}
function Read-VmrQBContacts {
    # team handle -> contact object. A missing or unreadable file is an empty roster, never an error.
    [CmdletBinding()]
    param([string] $Path)
    $result = [ordered]@{}
    if (-not $Path -or -not (Test-Path -LiteralPath $Path -PathType Leaf)) { return $result }
    try {
        $document = Get-Content -LiteralPath $Path -Raw | ConvertFrom-Json -Depth 20
        foreach ($property in $document.contacts.PSObject.Properties) { $result[$property.Name] = $property.Value }
    }
    catch { Write-Warning "Could not read $Path ($($_.Exception.Message)); treating the roster as empty." }
    return $result
}

function ConvertFrom-VmrQBGuideOwnership {
    <#
    .SYNOPSIS
    Reads the "Repo Ownership" table of VMR-QB-Guide.md: one row per repo, naming its GitHub team.
    .DESCRIPTION
    The team comes from the row's link (https://github.com/orgs/<org>/teams/<slug>), which is the page
    that lists the team's maintainers; a bare @org/slug is accepted when there is no link.
    #>
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
        $org = $match.Groups[1].Value; $slug = $match.Groups[2].Value
        $rows.Add([pscustomobject]@{
                Repo = ($cells[0] -replace '[*`_]', '').Trim(); Team = "@$org/$slug"; Org = $org; Slug = $slug
                Url  = "https://github.com/orgs/$org/teams/$slug"
            })
    }
    if ($rows.Count -eq 0) { throw "The 'Repo Ownership' section has no table rows naming a GitHub team. The guide's format may have changed." }
    return $rows.ToArray()
}

function ConvertTo-VmrQBRepoOwnerMap {
    <#
    .SYNOPSIS
    The guide's Repo Ownership rows as repo -> team. Only rows for a single repo count: the
    "General VMR infra" row is the default owner, not a repo.
    #>
    [CmdletBinding()]
    param([Parameter(Mandatory)][AllowEmptyCollection()][object[]] $Rows)
    $map = [ordered]@{}
    foreach ($row in $Rows) {
        if ($row.Repo -notmatch '^[A-Za-z0-9][A-Za-z0-9._-]*$' -or $row.Team -notmatch '^@dotnet/') { continue }
        $map[$row.Repo] = $row.Team
    }
    return $map
}

function Get-VmrQBGuideText {
    <#
    .SYNOPSIS
    The guide text and where it came from: -GuidePath, else docs/VMR-QB-Guide.md in the checkout this
    skill lives in. It is never fetched from the network: it changes when you git pull.
    #>
    [CmdletBinding()]
    param([string] $GuidePath)
    if (-not $GuidePath) { $GuidePath = Join-Path (Get-VmrQBPackageRoot) 'docs\VMR-QB-Guide.md' }
    if (-not (Test-Path -LiteralPath $GuidePath -PathType Leaf)) {
        throw "Guide not found: $GuidePath. The skill reads repo owners from docs/VMR-QB-Guide.md in the dotnet/dotnet checkout it lives in; run it from that checkout (git pull updates the guide)."
    }
    return [pscustomobject]@{ Text = [IO.File]::ReadAllText($GuidePath); Source = $GuidePath }
}

function Get-VmrQBGuideOwnershipRows {
    # The guide's Repo Ownership rows; throws with the file named when it cannot be read or parsed.
    [CmdletBinding()]
    param([string] $GuidePath)
    $guide = Get-VmrQBGuideText -GuidePath $GuidePath
    try { return @(ConvertFrom-VmrQBGuideOwnership -Text $guide.Text) }
    catch { throw "$($_.Exception.Message) (guide: $($guide.Source))" }
}

# --- GitHub lookups. Each returns a status object instead of throwing so the caller can tell a missing
# --- team from a sign-in problem, and so tests can substitute them.

function Get-VmrQBGhStatus {
    param([string] $Output, [int] $ExitCode)
    if ($ExitCode -eq 0) { return 'ok' }
    if ($Output -match 'HTTP 404') { return 'notfound' }
    if ($Output -match 'HTTP 40[13]|SAML|SSO|read:org|Resource not accessible|authentication|auth login|gh auth') { return 'auth' }
    return 'error'
}

function Get-VmrQBDefaultMaintainerLookup {
    # The same list the team page shows under "Maintainers" (https://github.com/orgs/<org>/teams/<slug>);
    # the page itself needs a browser session, the API does not.
    return {
        param($org, $slug, $role)
        $output = & gh api "orgs/$org/teams/$slug/members?role=$role&per_page=100" --paginate --jq '.[].login' 2>&1 | Out-String
        $code = $LASTEXITCODE
        $status = Get-VmrQBGhStatus -Output $output -ExitCode $code
        $logins = if ($status -eq 'ok') { @($output -split "`r?`n" | ForEach-Object { $_.Trim() } | Where-Object { $_ }) } else { @() }
        [pscustomobject]@{ Status = $status; Logins = $logins; Message = $(if ($status -eq 'ok') { '' } else { $output.Trim() }) }
    }
}

function Get-VmrQBDefaultProfileLookup {
    return {
        param($login)
        $name = & gh api "users/$login" --jq '.name // ""' 2>$null | Out-String
        if ($LASTEXITCODE -ne 0) { return $null }
        return $name.Trim()
    }
}

function Get-VmrQBDefaultActivityLookup {
    return {
        param($login)
        $stamp = & gh api "users/$login/events/public?per_page=1" --jq '.[0].created_at // ""' 2>$null | Out-String
        if ($LASTEXITCODE -ne 0 -or -not $stamp.Trim()) { return $null }
        $parsed = [datetime]::MinValue
        if ([datetime]::TryParse($stamp.Trim(), [Globalization.CultureInfo]::InvariantCulture, [Globalization.DateTimeStyles]::AdjustToUniversal -bor [Globalization.DateTimeStyles]::AssumeUniversal, [ref]$parsed)) { return $parsed }
        return $null
    }
}

function Test-VmrQBContactsStale {
    <#
    .SYNOPSIS
    True when there is no roster, a generated entry was never verified, or the guide beside the skill lists
    a team the roster does not cover (you pulled a newer guide). Reads local files only; never touches the
    network, and never expires by age.
    #>
    [CmdletBinding()]
    param([string] $Path = (Get-VmrQBContactsPath), [string] $GuidePath)
    $contacts = Read-VmrQBContacts -Path $Path
    if ($contacts.Count -eq 0) { return $true }
    foreach ($contact in $contacts.Values) {
        if (-not (Test-VmrQBContactGenerated $contact)) { continue }
        $verified = [datetime]::MinValue
        if (-not [datetime]::TryParse([string](Get-VmrQBField $contact 'verifiedAt'), [Globalization.CultureInfo]::InvariantCulture, [Globalization.DateTimeStyles]::None, [ref]$verified)) { return $true }
    }
    try { $rows = @(Get-VmrQBGuideOwnershipRows -GuidePath $GuidePath) } catch { return $false }
    foreach ($row in $rows) { if (-not $contacts.Contains($row.Team)) { return $true } }
    return $false
}
function Select-VmrQBMaintainer {
    # The most recently active human, ties broken by login so the choice is repeatable.
    [CmdletBinding()]
    param([string[]] $Logins, [scriptblock] $ActivityLookup, [int] $MaxChecked = 20)
    $people = @($Logins | Where-Object { $_ -and $_ -notmatch '(?i)\[bot\]$|-bot$' } | Sort-Object -Unique)
    $ranked = foreach ($login in ($people | Select-Object -First $MaxChecked)) {
        $active = & $ActivityLookup $login
        [pscustomobject]@{ Login = $login; Active = $(if ($active) { [datetime]$active } else { [datetime]::MinValue }) }
    }
    return @($ranked | Sort-Object @{ Expression = 'Active'; Descending = $true }, @{ Expression = 'Login'; Descending = $false })
}

function Update-VmrQBContacts {
    <#
    .SYNOPSIS
    Builds or refreshes the roster from the guide's team table and each team's GitHub maintainers.
    .OUTPUTS
    An object with Path, Entries, Built, Reused, Preserved, Missing, Problems, AuthProblem, Drift, Changed.
    #>
    [CmdletBinding()]
    param(
        [string] $GuidePath,
        [string] $OutputPath = (Get-VmrQBContactsPath -ForWrite),
        [string] $ExistingPath = (Get-VmrQBContactsPath),
        # Ask GitHub again about every generated entry, not only teams the roster does not have yet.
        [switch] $Refresh,
        # Also replace entries a person wrote.
        [switch] $Force,
        [switch] $Preview,
        [datetime] $Today = (Get-Date),
        [scriptblock] $MaintainerLookup = (Get-VmrQBDefaultMaintainerLookup),
        [scriptblock] $ProfileLookup = (Get-VmrQBDefaultProfileLookup),
        [scriptblock] $ActivityLookup = (Get-VmrQBDefaultActivityLookup)
    )
    $guide = Get-VmrQBGuideText -GuidePath $GuidePath
    $rows = @(Get-VmrQBGuideOwnershipRows -GuidePath $GuidePath)
    $teamRows = [ordered]@{}
    foreach ($row in $rows) { if (-not $teamRows.Contains($row.Team)) { $teamRows[$row.Team] = $row } }
    $teams = @($teamRows.Keys)
    $existing = Read-VmrQBContacts -Path $ExistingPath
    $verifiedDay = $Today.Date.ToString('yyyy-MM-dd')

    $entries = [ordered]@{}
    $built = [Collections.Generic.List[string]]::new(); $reused = [Collections.Generic.List[string]]::new()
    $preserved = [Collections.Generic.List[string]]::new(); $missing = [Collections.Generic.List[string]]::new()
    $problems = [Collections.Generic.List[string]]::new(); $authProblem = $null

    foreach ($team in $teams) {
        $current = if ($existing.Contains($team)) { $existing[$team] } else { $null }
        $isGenerated = $null -ne $current -and (Test-VmrQBContactGenerated $current)
        if ($null -ne $current -and -not $isGenerated -and -not $Force) { $entries[$team] = $current; $preserved.Add($team); continue }
        if ($isGenerated -and -not $Refresh) {
            $verified = [datetime]::MinValue
            if ([datetime]::TryParse([string](Get-VmrQBField $current 'verifiedAt'), [Globalization.CultureInfo]::InvariantCulture, [Globalization.DateTimeStyles]::None, [ref]$verified)) {
                $entries[$team] = $current; $reused.Add($team); continue
            }
        }

        $row = $teamRows[$team]
        $role = 'maintainer'
        $found = & $MaintainerLookup $row.Org $row.Slug 'maintainer'
        if ($found.Status -eq 'ok' -and @($found.Logins | Where-Object { $_ -notmatch '(?i)\[bot\]$|-bot$' }).Count -eq 0) {
            $role = 'member'
            $found = & $MaintainerLookup $row.Org $row.Slug 'member'
        }
        if ($found.Status -ne 'ok' -or @($found.Logins).Count -eq 0) {
            $why = if ($found.Status -eq 'ok') { 'GitHub lists no maintainers or members it can show you' } else { "GitHub said ($($found.Status)): $($found.Message)" }
            if ($found.Status -eq 'auth' -and -not $authProblem) { $authProblem = $found.Message }
            if ($null -ne $current) { $entries[$team] = $current; $problems.Add("$team kept as it was; $why.") }
            else { $missing.Add($team); $problems.Add("$team has no contact; $why.") }
            continue
        }
        $ranked = @(Select-VmrQBMaintainer -Logins $found.Logins -ActivityLookup $ActivityLookup)
        if ($ranked.Count -eq 0) { $missing.Add($team); $problems.Add("$team has only bot accounts listed; no contact."); continue }
        $chosen = $ranked[0].Login
        # Everyone the team page lists (up to five, most recently active first), with their profile names.
        $display = $null
        $candidates = @(foreach ($person in ($ranked | Select-Object -First 5)) {
                $name = & $ProfileLookup $person.Login
                if ($person.Login -eq $chosen) { $display = $name }
                [ordered]@{ github = $person.Login; name = $(if ($name) { $name } else { $person.Login }); lastActive = $(if ($person.Active -gt [datetime]::MinValue) { $person.Active.ToString('yyyy-MM-dd') } else { $null }) }
            })
        $basis = if ($role -eq 'maintainer') { 'team maintainer' } else { 'team member (GitHub lists no maintainer)' }
        if ($ranked.Count -gt 1) { $basis += ", most recently active of $($ranked.Count)" }
        $entries[$team] = [ordered]@{
            github     = $chosen
            teams      = $(if ($display) { $display } else { $chosen })
            basis      = $basis
            source     = $script:VmrQBContactSource
            verifiedAt = $verifiedDay
            teamUrl    = $row.Url
            candidates = $candidates
        }
        $built.Add($team)
    }
    # A contact for a team the guide does not list survives only if a person wrote it.
    foreach ($team in $existing.Keys) {
        if ($teams -contains $team) { continue }
        if (-not (Test-VmrQBContactGenerated $existing[$team])) { $entries[$team] = $existing[$team]; $preserved.Add($team) }
    }

    $drift = [Collections.Generic.List[string]]::new()
    try {
        $routed = @((Get-VmrQBRouting).Teams)
        foreach ($team in $teams) { if ($team -notin $routed) { $drift.Add("$team is in the guide but not in routing.json; add it there so reports can route to it.") } }
        foreach ($team in $routed) { if ($team -notin $teams) { $drift.Add("$team is in routing.json but not in the guide's Repo Ownership table.") } }
    }
    catch { $drift.Add("routing.json could not be compared: $($_.Exception.Message)") }

    $changed = ($built.Count -gt 0) -or -not (Test-Path -LiteralPath $OutputPath -PathType Leaf)
    $document = [ordered]@{
        '$schema'     = 'operator-maintained team contacts'
        schemaVersion = 2
        readme        = @(
            'Generated by ./common/Invoke-Contacts.ps1 from the Repo Ownership table in docs/VMR-QB-Guide.md and each GitHub team''s maintainers.',
            'This file names people, so it is local data and is never committed. Re-running reuses it; GitHub is asked again only for a team new in the guide, or with -Refresh.',
            'github = login for GitHub destinations; teams = display name for Teams destinations (the GitHub profile name; fix it here if Teams shows a different one).',
            'To pin a contact, edit the entry and delete its "source" line: entries without source = "github-maintainers" are never overwritten.'
        )
        generated     = [ordered]@{ at = $Today.ToString('s'); guide = $guide.Source }
        contacts      = $entries
    }
    if ($changed -and -not $Preview) {
        $directory = Split-Path -Parent $OutputPath
        [void][IO.Directory]::CreateDirectory($directory)
        $json = (($document | ConvertTo-Json -Depth 10) -replace "`r`n", "`n") + "`n"
        $temp = "$OutputPath.$([guid]::NewGuid().ToString('N')).tmp"
        [IO.File]::WriteAllText($temp, $json, [Text.UTF8Encoding]::new($false))
        Move-Item -LiteralPath $temp -Destination $OutputPath -Force
    }
    return [pscustomobject]@{
        Path = $OutputPath; Guide = $guide.Source; Entries = $entries; Built = $built.ToArray(); Reused = $reused.ToArray()
        Preserved = $preserved.ToArray(); Missing = $missing.ToArray(); Problems = $problems.ToArray(); AuthProblem = $authProblem
        Drift = $drift.ToArray(); Changed = $changed; Written = ($changed -and -not $Preview)
    }
}

# Console summary of a roster build, shared by Invoke-Setup.ps1 and Invoke-Contacts.ps1.
function Write-QBContactsOutcome {
    param($Result, [switch] $Brief)
    if ($Brief) {
        if ($Result.Written) { Write-Host "  team roster: $($Result.Built.Count) team(s) built -> $($Result.Path)" -ForegroundColor Green }
    }
    else {
        Write-Host "Team contacts from $($Result.Guide)" -ForegroundColor Cyan
        foreach ($team in $Result.Entries.Keys) {
            $entry = $Result.Entries[$team]
            $how = if ($team -in $Result.Built) { 'built' } elseif ($team -in $Result.Reused) { 'reused' } else { 'kept' }
            Write-Host ('  {0,-40} {1,-22} {2,-24} {3}' -f $team, (Get-VmrQBField $entry 'github'), (Get-VmrQBField $entry 'teams'), $how)
        }
        Write-Host ("`n{0} built, {1} reused, {2} kept as written by a person, {3} without a contact." -f $Result.Built.Count, $Result.Reused.Count, $Result.Preserved.Count, $Result.Missing.Count)
        if ($Result.Written) { Write-Host "Wrote $($Result.Path)" -ForegroundColor Green }
        elseif ($Result.Changed) { Write-Host "Preview only; would write $($Result.Path)" -ForegroundColor Yellow }
        else { Write-Host "Up to date: $($Result.Path)" -ForegroundColor Green }
    }
    foreach ($problem in $Result.Problems) { Write-Warning $problem }
    foreach ($drift in $Result.Drift) { if (-not $Brief) { Write-Host "  DRIFT: $drift" -ForegroundColor Yellow } }
    if ($Result.AuthProblem) {
        Write-Host "`nAUTHENTICATION PROBLEM reading GitHub team members:" -ForegroundColor Red
        Write-Host "  $($Result.AuthProblem)" -ForegroundColor Red
        Write-Host '  Fix: gh auth refresh -h github.com -s read:org   then authorize SSO for the dotnet organization.' -ForegroundColor Yellow
    }
}
