[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# Your own routing copy: made once with a base snapshot, compared with the shipped file entry by entry, edited only through
# changes that still load, reconciled without losing your edits, and validated as the registry the daily round actually runs.
$scripts = Split-Path -Parent $PSScriptRoot
. (Join-Path $scripts 'common\core.ps1')
. (Join-Path $scripts 'common\routing-edit.ps1')
. (Join-Path $scripts 'verify\knowledge.ps1')

$failures = 0
function Check([string] $Name, [scriptblock] $Body) {
    try { & $Body; Write-Host "PASS: $Name" -ForegroundColor Green }
    catch { $script:failures++; Write-Host "FAIL: $Name -> $($_.Exception.Message)" -ForegroundColor Red }
}
function Assert($Condition, [string] $Message) { if (-not $Condition) { throw $Message } }
function Assert-Throws([scriptblock] $Body, [string] $Pattern) {
    try { & $Body | Out-Null } catch {
        if ($_.Exception.Message -notmatch $Pattern) { throw "wrong error: $($_.Exception.Message)" }
        return
    }
    throw "Expected an error matching: $Pattern"
}
$script:TempDirs = [System.Collections.Generic.List[string]]::new()
function New-TempDir {
    $path = Join-Path ([IO.Path]::GetTempPath()) ('vmrqb-localrouting-' + [guid]::NewGuid().ToString('N'))
    [void][IO.Directory]::CreateDirectory($path)
    $script:TempDirs.Add($path)
    return $path
}
function New-GuideFile([string] $Dir, [string[]] $ExtraRows = @()) {
    # The real guide's table plus extra rows, so every team the registry names still exists.
    $real = [IO.File]::ReadAllLines((Join-Path (Get-VmrQBPackageRoot) 'docs\VMR-QB-Guide.md'))
    $start = [array]::FindIndex($real, [Predicate[string]] { param($l) $l -match '^##\s+Repo Ownership\b' })
    $rows = @(for ($i = $start + 1; $i -lt $real.Count; $i++) { if ($real[$i] -match '^#{1,6}\s') { break }; if ($real[$i].StartsWith('|')) { $real[$i] } })
    $path = Join-Path $Dir 'VMR-QB-Guide.md'
    (@('# Guide', '', '## Repo Ownership', '') + $rows + $ExtraRows + @('', '### Tips', '', '| not | a row |')) | Set-Content -LiteralPath $path -Encoding utf8
    return $path
}
function Read-Json([string] $Path) { return (Get-Content -LiteralPath $Path -Raw | ConvertFrom-Json -Depth 20) }
function Write-Json([string] $Path, $Value) { [IO.File]::WriteAllText($Path, ((ConvertTo-Json -InputObject $Value -Depth 20) + "`n"), [Text.UTF8Encoding]::new($false)) }
function Get-Hash([string] $Path) { return (Get-FileHash -LiteralPath $Path).Hash }

$sandbox = New-TempDir
$originalHome = $env:VMR_QB_HOME
$shipped = Join-Path (Get-VmrQBPaths).KnowledgeRoot 'routing.json'
$entry = Join-Path $scripts 'common\Invoke-Routing.ps1'
function Invoke-Entry([string[]] $Arguments) {
    $output = & pwsh -NoProfile -NonInteractive -File $entry @Arguments 2>&1 | Out-String
    return [pscustomobject]@{ Output = $output; ExitCode = $LASTEXITCODE }
}

try {
    Check 'The first use makes the copy and a base snapshot once; a later use rewrites neither' {
        $env:VMR_QB_HOME = New-TempDir
        $paths = Get-VmrQBPaths
        [void](Get-VmrQBRouting)
        Assert ((Test-Path -LiteralPath $paths.LocalRoutingPath) -and (Test-Path -LiteralPath $paths.RoutingBasePath)) 'both the copy and its base snapshot must be made'
        Assert ((Get-Hash $paths.RoutingBasePath) -eq (Get-Hash $shipped)) 'the base snapshot must be the shipped file, byte for byte'
        Assert ((Read-Json $paths.LocalRoutingPath).baseSha256 -eq (Get-VmrQBShippedRoutingHash)) 'the copy must be stamped with the shipped hash'
        $document = Read-Json $paths.LocalRoutingPath
        $document.channelRules | Add-Member -NotePropertyName 'Servicing chat' -NotePropertyValue 'My own rule.' -Force
        Write-Json $paths.LocalRoutingPath $document
        $mine = Get-Hash $paths.LocalRoutingPath; $base = Get-Hash $paths.RoutingBasePath
        [void](Get-VmrQBRouting)
        Assert ((Get-Hash $paths.LocalRoutingPath) -eq $mine -and (Get-Hash $paths.RoutingBasePath) -eq $base) 'an existing copy and base must never be rewritten'
    }

    Check 'A difference is attributed: yours, upstream or both; without a base it is only "differs"; property order is not a difference' {
        $base = [pscustomobject]@{ channelRules = [pscustomobject]@{ A = 'a1'; B = 'b1'; C = 'c1' }; categories = @([pscustomobject]@{ id = 'same'; name = 'Same'; owners = @('@dotnet/x') }) }
        $local = [pscustomobject]@{ channelRules = [pscustomobject]@{ A = 'a1'; B = 'b2'; C = 'c2' }; categories = @([pscustomobject]@{ owners = @('@dotnet/x'); name = 'Same'; id = 'same' }, [pscustomobject]@{ id = 'mine'; name = 'Mine' }) }
        $shippedDoc = [pscustomobject]@{ channelRules = [pscustomobject]@{ A = 'a2'; B = 'b1'; C = 'c3' }; categories = @([pscustomobject]@{ id = 'same'; name = 'Same'; owners = @('@dotnet/x') }, [pscustomobject]@{ id = 'theirs'; name = 'Theirs' }) }
        $diff = @(Get-VmrQBRoutingDiff -Local $local -Shipped $shippedDoc -Base $base)
        $state = @{}; foreach ($d in $diff) { $state["$($d.Section)/$($d.Key)"] = $d.State }
        Assert ($state['channelRules/A'] -eq 'upstream') "A: only the shipped file changed -> upstream, got $($state['channelRules/A'])"
        Assert ($state['channelRules/B'] -eq 'yours') "B: only you changed it -> yours, got $($state['channelRules/B'])"
        Assert ($state['channelRules/C'] -eq 'both') "C: both changed it differently -> both, got $($state['channelRules/C'])"
        Assert ($state['categories/mine'] -eq 'yours' -and $state['categories/theirs'] -eq 'upstream') 'added entries are attributed to whoever added them'
        Assert (-not $state.ContainsKey('categories/same')) 'the same entry with its properties in another order is not a difference'
        $plain = @(Get-VmrQBRoutingDiff -Local $local -Shipped $shippedDoc -Base $null)
        $names = @{}; foreach ($d in $plain) { $names["$($d.Section)/$($d.Key)"] = $d.State }
        Assert ($names['channelRules/A'] -eq 'differs' -and $names['categories/mine'] -eq 'onlyLocal' -and $names['categories/theirs'] -eq 'onlyShipped') "without a base: $($names | ConvertTo-Json -Compress)"
    }

    Check 'An edit is written only if the merged result still loads; -WhatIf and a refused edit change nothing' {
        $env:VMR_QB_HOME = New-TempDir
        $paths = Get-VmrQBPaths
        [void](Get-VmrQBRouting)
        $before = Get-Hash $paths.LocalRoutingPath
        Assert-Throws { Set-VmrQBLocalRoutingEntry -Section categories -Key 'ghost' -Value ([pscustomobject]@{ id = 'ghost'; name = 'Ghost'; owners = @('@dotnet/not-a-team') }) } 'Not saved.*unknown team @dotnet/not-a-team'
        Assert ((Get-Hash $paths.LocalRoutingPath) -eq $before) 'a refused edit must leave the file as it was'
        Set-VmrQBLocalRoutingEntry -Section categories -Key 'preview' -Value ([pscustomobject]@{ id = 'preview'; name = 'Preview'; owners = @('@dotnet/dnceng') }) -WhatIf
        Assert ((Get-Hash $paths.LocalRoutingPath) -eq $before) '-WhatIf must not write'
        Set-VmrQBLocalRoutingEntry -Section categories -Key 'kept' -Value ([pscustomobject]@{ id = 'kept'; name = 'Kept'; ownerRepos = @('runtime'); owners = @('@dotnet/dnceng') })
        $routing = Get-VmrQBRouting
        $kept = @($routing.Categories | Where-Object id -eq 'kept')[0]
        Assert (@($kept.owners) -contains '@dotnet/dnr-codeflow' -and @($kept.owners) -contains '@dotnet/dnceng') 'the saved category must be active, with ownerRepos resolved'
        Assert ((Read-Json $paths.LocalRoutingPath).baseSha256 -eq (Get-VmrQBShippedRoutingHash)) 'an edit must keep the stamp'
        Set-VmrQBLocalRoutingEntry -Section categories -Key 'kept' -Remove
        Assert (@((Get-VmrQBRouting).Categories | Where-Object id -eq 'kept').Count -eq 0) 'a removed local entry is gone'
        Set-VmrQBLocalRoutingEntry -Section channelRules -Key 'Servicing chat' -Value 'A rule of my own.'
        Assert ((Get-VmrQBRouting).ChannelRules['Servicing chat'] -eq 'A rule of my own.') 'a map entry must be saved'
        Assert (@(Get-ChildItem -LiteralPath (Split-Path -Parent $paths.LocalRoutingPath) -Filter '*.tmp').Count -eq 0) 'no temp file may remain'
    }

    Check 'Incoming routing changes are applied only where safe: added, replaced if untouched, kept on conflict, refused if they would not load' {
        $env:VMR_QB_HOME = New-TempDir
        $paths = Get-VmrQBPaths
        [void](Get-VmrQBRouting)
        Set-VmrQBLocalRoutingEntry -Section channelRules -Key 'Servicing chat' -Value 'My rule.'
        $changes = @(
            [pscustomobject]@{ section = 'categories'; key = 'incoming'; value = [pscustomobject]@{ id = 'incoming'; name = 'Incoming'; owners = @('@dotnet/dnceng') } },
            [pscustomobject]@{ section = 'channelRules'; key = 'Servicing chat'; value = 'Their rule.' },
            [pscustomobject]@{ section = 'categories'; key = 'ghost'; value = [pscustomobject]@{ id = 'ghost'; name = 'Ghost'; owners = @('@dotnet/not-a-team') } },
            [pscustomobject]@{ section = 'nonsense'; key = 'x'; value = 'y' })
        $before = Get-Hash $paths.LocalRoutingPath
        $preview = @(Import-VmrQBRoutingChanges -Changes $changes -Preview)
        Assert ((Get-Hash $paths.LocalRoutingPath) -eq $before) '-Preview must write nothing'
        $result = @(Import-VmrQBRoutingChanges -Changes $changes)
        $by = @{}; foreach ($r in $result) { $by["$($r.Section)/$($r.Key)"] = $r }
        Assert ($by['categories/incoming'].Result -eq 'added') 'a new entry is added'
        Assert ($by['channelRules/Servicing chat'].Result -eq 'conflict') 'my changed entry is kept'
        Assert ($by['categories/ghost'].Result -eq 'invalid' -and $by['categories/ghost'].Detail -match 'unknown team') 'an entry that would not load here is refused with the reason'
        Assert ($by['nonsense/x'].Result -eq 'invalid') 'an unknown section is invalid'
        Assert (($preview | ForEach-Object { "$($_.Section)/$($_.Key)=$($_.Result)" }) -join ',' -eq (($result | ForEach-Object { "$($_.Section)/$($_.Key)=$($_.Result)" }) -join ','))  '-Preview must predict exactly what the import does'
        Assert ((Get-VmrQBRouting).ChannelRules['Servicing chat'] -eq 'My rule.') 'on conflict mine must be kept'
        Assert (@((Get-VmrQBRouting).Categories | Where-Object id -eq 'incoming').Count -eq 1) 'the added entry must be active'
        $again = @(Import-VmrQBRoutingChanges -Changes $changes)
        Assert (@($again | Where-Object { $_.Section -eq 'categories' -and $_.Key -eq 'incoming' -and $_.Result -eq 'unchanged' }).Count -eq 1) 'importing twice must not duplicate or change anything'
        Assert (@((Get-VmrQBRouting).Categories | Where-Object id -eq 'incoming').Count -eq 1) 'a repeated import must not duplicate the entry'
    }

    Check 'Without a base snapshot a copy still works: diff says "differs", adopt -All refuses, acknowledge creates one' {
        $env:VMR_QB_HOME = New-TempDir
        $paths = Get-VmrQBPaths
        [void](Get-VmrQBRouting)
        Remove-Item -LiteralPath $paths.RoutingBasePath -Force
        $document = Read-Json $paths.LocalRoutingPath
        $document.channelRules | Add-Member -NotePropertyName 'Servicing chat' -NotePropertyValue 'Edited before snapshots existed.' -Force
        Write-Json $paths.LocalRoutingPath $document
        $diff = @((Invoke-Entry @('diff', '-Json')).Output | ConvertFrom-Json)
        Assert ($diff.Count -eq 1 -and $diff[0].State -eq 'differs') 'an old copy has differences it cannot attribute'
        $all = Invoke-Entry @('adopt', '-All')
        Assert ($all.ExitCode -ne 0 -and $all.Output -match 'base snapshot') 'adopt -All must refuse without a base snapshot'
        Assert ((Invoke-Entry @('acknowledge')).ExitCode -eq 0 -and (Test-Path -LiteralPath $paths.RoutingBasePath)) 'acknowledge must create the snapshot'
        Assert ((Get-VmrQBRouting).ChannelRules['Servicing chat'] -eq 'Edited before snapshots existed.') 'and keep the edit'
    }

    Check 'The knowledge check validates the routing that runs as its own finding, never crashes on a broken local copy, and reads aliases from the local folder' {
        $env:VMR_QB_HOME = New-TempDir
        $paths = Get-VmrQBPaths
        [void](Get-VmrQBRouting)
        $docs = New-TempDir
        Set-Content -LiteralPath (Join-Path $docs 'note.md') -Value "Ping @dotnet/totally-fake about it.`n"
        $clean = @(Invoke-VmrQBKnowledgeCheck -DocumentRoots @($docs))
        Assert (@($clean | Where-Object check -eq 'local-routing').Count -eq 0) 'a valid local copy has no local-routing finding'
        $document = Read-Json $paths.LocalRoutingPath
        $document.categories = @($document.categories) + @([pscustomobject]@{ id = 'ghost'; name = 'Ghost'; owners = @('@dotnet/ghost-team') })
        Write-Json $paths.LocalRoutingPath $document
        [IO.File]::WriteAllText($paths.AliasesPath, '{ not json')
        $broken = @(Invoke-VmrQBKnowledgeCheck -DocumentRoots @($docs))
        $local = @($broken | Where-Object check -eq 'local-routing')
        Assert ($local.Count -eq 1 -and $local[0].severity -eq 'warn' -and $local[0].message -match 'ghost-team' -and $local[0].message -match 'Invoke-Routing\.ps1 diff') "a broken local copy must be one clear warning (the round carries on): $(($local | ForEach-Object message) -join ' | ')"
        Assert (@($broken | Where-Object { $_.check -eq 'handle' -and $_.message -match 'totally-fake' }).Count -eq 1) 'the document scan must still run, against the shipped registry'
        Assert (@($broken | Where-Object { $_.check -eq 'schema' -and $_.subject -eq 'issue-aliases.json' }).Count -eq 1) 'the alias file is read from the local folder, so a broken one is found'
        $shippedOnly = @(Invoke-VmrQBKnowledgeCheck -KnowledgeRoot (Get-VmrQBPaths).KnowledgeRoot -DocumentRoots @($docs))
        Assert (@($shippedOnly | Where-Object check -eq 'local-routing').Count -eq 0) 'an explicit -KnowledgeRoot checks that folder only'
    }
}
finally {
    if ($null -eq $originalHome) { Remove-Item Env:\VMR_QB_HOME -ErrorAction SilentlyContinue } else { $env:VMR_QB_HOME = $originalHome }
    foreach ($created in $script:TempDirs) { Remove-Item -LiteralPath $created -Recurse -Force -ErrorAction SilentlyContinue }
}

if ($failures) { Write-Host "$failures local-routing test(s) failed." -ForegroundColor Red; exit 1 }
Write-Host 'All local-routing tests passed.' -ForegroundColor Green
