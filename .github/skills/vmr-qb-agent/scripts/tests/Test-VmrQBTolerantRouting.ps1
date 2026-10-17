[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# A stale or inconsistent owner never stops a run. The routing loader matches a missing team to the closest one, keeps the name
# as written when nothing is close, and lists each case in Warnings; -Strict (verify, the tests, every edit) still throws. The
# ownership copy is made at first use from the Guide of the checkout, reused until that file changes, and is what the round falls
# back to when the Guide cannot be read.
$scripts = Split-Path -Parent $PSScriptRoot
. (Join-Path $scripts 'common\core.ps1')

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
$script:TempDirs = [System.Collections.Generic.List[string]]::new()
function New-TempDir {
    $path = Join-Path ([IO.Path]::GetTempPath()) ('vmrqb-tolerant-' + [guid]::NewGuid().ToString('N'))
    [void][IO.Directory]::CreateDirectory($path)
    $script:TempDirs.Add($path)
    return $path
}
function Write-Json([string] $Path, $Value) { [IO.File]::WriteAllText($Path, ((ConvertTo-Json -InputObject $Value -Depth 20) + "`n"), [Text.UTF8Encoding]::new($false)) }

function New-Guide([string] $Dir, [string[]] $Rows, [string] $Heading = '## Repo Ownership', [string] $Name = 'guide.md') {
    $path = Join-Path $Dir $Name
    (@('# Guide', '', $Heading, '', '| Repo | Owner |', '|---|---|') + $Rows + @('', '### Tips', '')) | Set-Content -LiteralPath $path -Encoding utf8
    return $path
}
function Row([string] $Repo, [string] $Slug) { "| $Repo | [@dotnet/$Slug](https://github.com/orgs/dotnet/teams/$Slug) |" }
function New-Shipped([string] $Dir) {
    $path = Join-Path $Dir 'routing.json'
    Write-Json $path ([ordered]@{
            schemaVersion = 1; verifiedOn = '2026-10-09'; vmrTestsOwner = '@dotnet/source-build'; unmappedLabel = 'No identified owner'
            teams = @('@dotnet/source-build')
            preferredRedirects = [ordered]@{ '@dotnet/roslyn' = '@dotnet/roslyn-infrastructure' }
            channels = [ordered]@{ x = 'Channel X' }; channelRules = [ordered]@{ 'Channel X' = 'a rule' }
            categories = @([ordered]@{ id = 'c1'; name = 'Category one'; ownerRepos = @('runtime'); owners = @('@dotnet/source-build') })
        })
    return $path
}
function New-Local([string] $Dir, $Categories) {
    $path = Join-Path $Dir 'routing.local.json'
    Write-Json $path ([ordered]@{ schemaVersion = 1; categories = @($Categories) })
    return $path
}

try {
    Check 'The closest team is the unique one that shares the most of the wanted name''s words; a tie, or no shared word, is no match' {
        $teams = @('@dotnet/roslyn-infrastructure', '@dotnet/msbuild', '@dotnet/dnr-codeflow')
        Assert ((Find-VmrQBClosestTeam -Handle '@dotnet/roslyn' -Team $teams) -eq '@dotnet/roslyn-infrastructure') 'roslyn must find the one roslyn team'
        Assert ((Find-VmrQBClosestTeam -Handle '@dotnet/roslyn-infra' -Team $teams) -eq '@dotnet/roslyn-infrastructure') 'a word that starts another (infra) must match'
        Assert ($null -eq (Find-VmrQBClosestTeam -Handle '@dotnet/roslyn' -Team @('@dotnet/roslyn-infrastructure', '@dotnet/roslyn-ide'))) 'two equally close teams are ambiguous: no match'
        Assert ((Find-VmrQBClosestTeam -Handle '@dotnet/roslyn-infrastructure' -Team @('@dotnet/roslyn-ide', '@dotnet/msbuild')) -eq '@dotnet/roslyn-ide') 'a renamed team that still has the main word matches'
        Assert ((Find-VmrQBClosestTeam -Handle '@dotnet/roslyn-infrastructure' -Team @('@dotnet/roslyn-ide', '@dotnet/roslyn-infra')) -eq '@dotnet/roslyn-infra') 'more shared words beat fewer'
        Assert ($null -eq (Find-VmrQBClosestTeam -Handle '@dotnet/nothing' -Team $teams)) 'nothing shared, nothing matched'
        Assert ($null -eq (Find-VmrQBClosestTeam -Handle '@dotnet/roslyn' -Team @())) 'no teams, no match'
        Assert ((Find-VmrQBClosestName -Name 'source-build-assets' -Candidate @('source-build', 'runtime')) -eq 'source-build') 'repo names match the same way'
    }

    Check 'A shipped redirect whose target left the Guide follows the closest team; with no unique match it is dropped and the handle stays as written; strict still throws' {
        $dir = New-TempDir; $shipped = New-Shipped $dir
        $one = New-Guide $dir @((Row 'runtime' 'dnr-codeflow'), (Row 'roslyn' 'roslyn-ide'))
        $routing = Get-VmrQBRouting -Path $shipped -LocalPath $null -GuidePath $one
        Assert ($routing.PreferredRedirects['@dotnet/roslyn'] -eq '@dotnet/roslyn-ide') 'the redirect must follow the one roslyn team'
        Assert ((@($routing.Warnings) -join ' ') -match 'closest team @dotnet/roslyn-ide') "the match must be said: $(@($routing.Warnings) -join ' | ')"
        $two = New-Guide $dir @((Row 'runtime' 'dnr-codeflow'), (Row 'roslyn' 'roslyn-ide'), (Row 'roslyn-tools' 'roslyn-tooling')) -Name 'two.md'
        $ambiguous = Get-VmrQBRouting -Path $shipped -LocalPath $null -GuidePath $two
        Assert (-not $ambiguous.PreferredRedirects.Contains('@dotnet/roslyn')) 'an ambiguous redirect must be dropped'
        Assert ((@($ambiguous.Warnings) -join ' ') -match 'stays as written') 'and the warning must say the handle stays as written'
        Assert-Throws { Get-VmrQBRouting -Path $shipped -LocalPath $null -GuidePath $one -Strict } 'redirect @dotnet/roslyn points at unknown team'
        $clean = New-Guide $dir @((Row 'runtime' 'dnr-codeflow'), (Row 'roslyn' 'roslyn-infrastructure')) -Name 'clean.md'
        Assert (@((Get-VmrQBRouting -Path $shipped -LocalPath $null -GuidePath $clean).Warnings).Count -eq 0) 'a consistent Guide must have no warnings'
    }

    Check 'A local category naming a team that left the Guide is kept as written or matched to the closest team, with a warning; strict throws' {
        $dir = New-TempDir; $shipped = New-Shipped $dir
        $guide = New-Guide $dir @((Row 'runtime' 'dnr-codeflow'), (Row 'roslyn' 'roslyn-infrastructure'), (Row 'nuget' 'nuget-client'))
        $local = New-Local $dir @([ordered]@{ id = 'gone'; name = 'Gone'; owners = @('@dotnet/gone-team') }, [ordered]@{ id = 'renamed'; name = 'Renamed'; owners = @('@dotnet/nuget') })
        $routing = Get-VmrQBRouting -Path $shipped -LocalPath $local -GuidePath $guide
        $gone = @($routing.Categories | Where-Object id -eq 'gone')[0]
        Assert (@($gone.owners) -contains '@dotnet/gone-team') 'with nothing close the owner is kept as written'
        $renamed = @($routing.Categories | Where-Object id -eq 'renamed')[0]
        Assert (@($renamed.owners) -contains '@dotnet/nuget-client' -and @($renamed.owners) -notcontains '@dotnet/nuget') 'a close owner is replaced by the closest team'
        $text = @($routing.Warnings) -join ' | '
        Assert ($text -match "Category 'gone'.*kept as written" -and $text -match "Category 'renamed'.*closest team @dotnet/nuget-client") "both must be listed: $text"
        Assert-Throws { Get-VmrQBRouting -Path $shipped -LocalPath $local -GuidePath $guide -Strict } "category 'gone' \(local routing file\) names unknown team"
    }

    Check 'A category naming a repo the Guide does not list skips that repo, a bad support reference is dropped, and the rest of the registry loads' {
        $dir = New-TempDir; $shipped = New-Shipped $dir
        $guide = New-Guide $dir @((Row 'runtime' 'dnr-codeflow'), (Row 'roslyn' 'roslyn-infrastructure'))
        $local = New-Local $dir @([ordered]@{ id = 'r'; name = 'R'; ownerRepos = @('no-such-repo', 'runtime'); support = 'nope' })
        $routing = Get-VmrQBRouting -Path $shipped -LocalPath $local -GuidePath $guide
        $category = @($routing.Categories | Where-Object id -eq 'r')[0]
        Assert ((@($category.owners) -join ',') -eq '@dotnet/dnr-codeflow') 'the known repo still gives its owner'
        Assert ($null -eq $category.PSObject.Properties['support']) 'the unknown support reference is dropped'
        Assert ((@($routing.Warnings) -join ' ') -match "no-such-repo.*skipped" -and (@($routing.Warnings) -join ' ') -match "support destination 'nope'") 'both are listed'
        Assert ($routing.Teams -contains '@dotnet/dnr-codeflow') 'the registry still loads'
    }

    Check 'A routing file that does not parse is ignored with a warning, and the Guide''s owners still work; strict throws' {
        $dir = New-TempDir; $shipped = New-Shipped $dir
        $guide = New-Guide $dir @((Row 'runtime' 'dnr-codeflow'), (Row 'roslyn' 'roslyn-infrastructure'))
        $badLocal = Join-Path $dir 'routing.local.json'; Set-Content -LiteralPath $badLocal -Value '{ not json'
        $routing = Get-VmrQBRouting -Path $shipped -LocalPath $badLocal -GuidePath $guide
        Assert ((@($routing.Warnings) -join ' ') -match 'local routing copy was not used' -and $routing.Categories.Count -eq 1) 'a bad local copy is ignored and the shipped categories stay'
        Assert-Throws { Get-VmrQBRouting -Path $shipped -LocalPath $badLocal -GuidePath $guide -Strict } 'not valid JSON'
        $badShipped = Join-Path $dir 'shipped-bad.json'; Set-Content -LiteralPath $badShipped -Value '{ not json'
        $routing = Get-VmrQBRouting -Path $badShipped -LocalPath $null -GuidePath $guide
        Assert ((@($routing.Warnings) -join ' ') -match 'shipped routing file was not used' -and $routing.Teams -contains '@dotnet/dnr-codeflow') 'a bad shipped file leaves the Guide''s owners in effect'
        $missing = Get-VmrQBRouting -Path (Join-Path $dir 'nope.json') -LocalPath $null -GuidePath $guide
        Assert ((@($missing.Warnings) -join ' ') -match 'was not found' -and $missing.Teams.Count -ge 2) 'a missing shipped file is a warning too'
        Assert-Throws { Get-VmrQBRouting -Path (Join-Path $dir 'nope.json') -LocalPath $null -GuidePath $guide -Strict } 'Routing registry not found'
    }

    Check 'A Guide that is missing or whose heading was renamed is a warning at the load; strict throws' {
        $dir = New-TempDir; $shipped = New-Shipped $dir
        $routing = Get-VmrQBRouting -Path $shipped -LocalPath $null -GuidePath (Join-Path $dir 'missing.md')
        Assert ($routing.GuideSource -eq 'none' -and (@($routing.Warnings) -join ' ') -match 'Guide ownership is unavailable') 'a missing Guide is reported, not thrown'
        Assert ($routing.Categories.Count -eq 1) 'the shipped categories still load'
        $renamed = New-Guide $dir @((Row 'runtime' 'dnr-codeflow')) -Heading '## Repository Ownership' -Name 'renamed.md'
        Assert ((@((Get-VmrQBRouting -Path $shipped -LocalPath $null -GuidePath $renamed).Warnings) -join ' ') -match 'Repo Ownership') 'a renamed heading is reported'
        Assert-Throws { Get-VmrQBRouting -Path $shipped -LocalPath $null -GuidePath (Join-Path $dir 'missing.md') -Strict } 'Guide not found'
        Assert-Throws { Get-VmrQBRouting -Path $shipped -LocalPath $null -GuidePath $renamed -Strict } 'Repo Ownership'
    }

    Check 'The ownership copy is made at first use, reused while the Guide is unchanged, renewed when it changes, and used when the Guide cannot be read' {
        $dir = New-TempDir; $copy = Join-Path $dir 'guide-ownership.json'
        $guide = New-Guide $dir @((Row 'runtime' 'dnr-codeflow'), (Row 'sdk' 'dotnet-cli'))
        $first = Resolve-VmrQBGuideOwnership -GuidePath $guide -UseCopy -CopyPath $copy
        Assert ($first.Source -eq 'checkout' -and $first.Rows.Count -eq 2 -and (Test-Path -LiteralPath $copy)) 'the first use reads the Guide and makes the copy'
        $saved = Get-Content -LiteralPath $copy -Raw | ConvertFrom-Json
        Assert ($saved.sha256 -eq (Get-VmrQBTextSha256 -Text ([IO.File]::ReadAllText($guide))) -and @($saved.rows).Count -eq 2 -and $saved.source -eq 'checkout') 'the copy records the Guide''s hash, source and rows'
        $stamp = (Get-Item -LiteralPath $copy).LastWriteTimeUtc.Ticks
        Start-Sleep -Milliseconds 50
        [void](Resolve-VmrQBGuideOwnership -GuidePath $guide -UseCopy -CopyPath $copy)
        Assert ((Get-Item -LiteralPath $copy).LastWriteTimeUtc.Ticks -eq $stamp) 'an unchanged Guide must not rewrite the copy'
        $guide = New-Guide $dir @((Row 'runtime' 'dnr-codeflow'), (Row 'sdk' 'dotnet-cli'), (Row 'msbuild' 'msbuild'))
        $renewed = Resolve-VmrQBGuideOwnership -GuidePath $guide -UseCopy -CopyPath $copy
        Assert ($renewed.Rows.Count -eq 3 -and @((Get-Content -LiteralPath $copy -Raw | ConvertFrom-Json).rows).Count -eq 3) 'a changed Guide (a pull) renews the copy'
        Set-Content -LiteralPath $guide -Value "# Guide`n`nno table here`n"
        $fallback = Resolve-VmrQBGuideOwnership -GuidePath $guide -UseCopy -CopyPath $copy
        Assert ($fallback.Source -eq 'copy' -and $fallback.Rows.Count -eq 3 -and (@($fallback.Warnings) -join ' ') -match 'ownership copy made') 'an unreadable Guide falls back to the copy and says so'
        Assert ((Get-Content -LiteralPath $copy -Raw | ConvertFrom-Json).rows.Count -eq 3) 'and the copy is kept as it was'
        Remove-Item -LiteralPath $guide -Force
        Assert ((Resolve-VmrQBGuideOwnership -GuidePath $guide -UseCopy -CopyPath $copy).Source -eq 'copy') 'a missing Guide falls back to the copy too'
    }

    Check 'With no Guide and no copy the Guide is fetched once, saved, and then reused without fetching again; a failed fetch is a warning, never an error' {
        $dir = New-TempDir; $copy = Join-Path $dir 'guide-ownership.json'
        $missing = Join-Path $dir 'no-guide.md'
        $text = ((@('# Guide', '', '## Repo Ownership', '', '| Repo | Owner |', '|---|---|', (Row 'runtime' 'dnr-codeflow'), '', '### Tips') ) -join "`n")
        $script:fetches = 0
        $original = $script:VmrQBGuideFetcher
        try {
            $script:VmrQBGuideFetcher = { param($Url) $script:fetches++; return $text }
            $first = Resolve-VmrQBGuideOwnership -GuidePath $missing -UseCopy -CopyPath $copy
            Assert ($first.Source -eq 'github' -and $first.Rows.Count -eq 1 -and $script:fetches -eq 1) 'the first use fetches once'
            Assert ((Get-Content -LiteralPath $copy -Raw | ConvertFrom-Json).source -eq 'github-main') 'and saves the copy with its source'
            $again = Resolve-VmrQBGuideOwnership -GuidePath $missing -UseCopy -CopyPath $copy
            Assert ($again.Source -eq 'copy' -and $script:fetches -eq 1) 'the next use reads the copy and does not fetch'
            Remove-Item -LiteralPath $copy -Force
            $script:VmrQBGuideFetcher = { param($Url) throw 'offline' }
            $failed = Resolve-VmrQBGuideOwnership -GuidePath $missing -UseCopy -CopyPath $copy
            Assert ($failed.Source -eq 'none' -and $failed.Rows.Count -eq 0 -and (@($failed.Warnings) -join ' ') -match 'offline') 'a failed fetch is reported, never thrown'
            Assert (-not (Test-Path -LiteralPath $copy)) 'and no empty copy is saved'
        }
        finally { $script:VmrQBGuideFetcher = $original }
    }

    Check 'With the default files the copy is made in the state folder; the Guide of the checkout is read, never fetched' {
        $original = $env:VMR_QB_HOME
        $originalFetcher = $script:VmrQBGuideFetcher
        try {
            $env:VMR_QB_HOME = New-TempDir
            $script:VmrQBGuideFetcher = { param($Url) throw 'the checkout has the Guide: nothing may be fetched' }
            $routing = Get-VmrQBRouting
            Assert ($routing.GuideSource -eq 'checkout') 'the checkout''s Guide is the source'
            Assert (Test-Path -LiteralPath (Get-VmrQBPaths).GuideOwnershipPath) 'the copy is made on first use'
            Assert (@($routing.Warnings).Count -eq 0) "the shipped registry and this Guide are consistent: $(@($routing.Warnings) -join ' | ')"
            Assert ((Get-VmrQBRouting -Strict).Teams.Count -eq $routing.Teams.Count) 'strict reads the same Guide'
        }
        finally { $script:VmrQBGuideFetcher = $originalFetcher; if ($null -eq $original) { Remove-Item Env:\VMR_QB_HOME -ErrorAction SilentlyContinue } else { $env:VMR_QB_HOME = $original } }
    }

    Check 'Update-VmrQBGuideOwnershipCopy renews the copy on request, from the checkout or from GitHub' {
        $dir = New-TempDir; $copy = Join-Path $dir 'guide-ownership.json'
        $guide = New-Guide $dir @((Row 'runtime' 'dnr-codeflow'))
        $result = Update-VmrQBGuideOwnershipCopy -GuidePath $guide -CopyPath $copy
        Assert ($result.Source -eq 'checkout' -and $result.Rows -eq 1) 'from the checkout'
        $original = $script:VmrQBGuideFetcher
        try {
            $script:VmrQBGuideFetcher = { param($Url) "# G`n`n## Repo Ownership`n`n| Repo | Owner |`n|---|---|`n" + (Row 'runtime' 'dnr-codeflow') + "`n" + (Row 'sdk' 'dotnet-cli') + "`n" }
            $fromWeb = Update-VmrQBGuideOwnershipCopy -FromGitHub -CopyPath $copy
            Assert ($fromWeb.Source -eq 'github-main' -and $fromWeb.Rows -eq 2) 'from GitHub on request'
            $script:VmrQBGuideFetcher = { param($Url) throw 'offline' }
            Assert-Throws { Update-VmrQBGuideOwnershipCopy -FromGitHub -CopyPath $copy } 'offline'
            Assert (@((Get-Content -LiteralPath $copy -Raw | ConvertFrom-Json).rows).Count -eq 2) 'a failed renewal leaves the copy as it was'
        }
        finally { $script:VmrQBGuideFetcher = $original }
    }

    Check 'A mention of a team that is not in the Guide gets the closest real team as its suggestion' {
        $dir = New-TempDir; $shipped = New-Shipped $dir
        $guide = New-Guide $dir @((Row 'runtime' 'dnr-codeflow'), (Row 'nuget' 'nuget-client'), (Row 'roslyn' 'roslyn-infrastructure'))
        $routing = Get-VmrQBRouting -Path $shipped -LocalPath $null -GuidePath $guide
        $found = @(Find-VmrQBUnknownHandles -Text 'Ping @dotnet/nuget and @dotnet/zzz-nothing.' -Routing $routing)
        $nuget = @($found | Where-Object handle -eq '@dotnet/nuget')[0]
        Assert ($nuget.suggestion -eq '@dotnet/nuget-client') 'the close team is suggested'
        Assert ($null -eq (@($found | Where-Object handle -eq '@dotnet/zzz-nothing')[0]).suggestion) 'a handle with nothing close has no suggestion'
    }

    Check 'The ownership block tells the model to confirm owners when the owner map has warnings' {
        $original = $env:VMR_QB_HOME
        try {
            $env:VMR_QB_HOME = New-TempDir
            $paths = Get-VmrQBPaths -Create
            $document = [ordered]@{ schemaVersion = 1; categories = @([ordered]@{ id = 'gone'; name = 'Gone'; owners = @('@dotnet/gone-team') }) }
            Write-Json $paths.LocalRoutingPath $document
            $block = Get-VmrQBOwnershipBlock
            Assert ($block -match 'Owner map notes \(confirm an owner before pinging it\):.*gone-team') 'the model is told which owner did not match'
        }
        finally { if ($null -eq $original) { Remove-Item Env:\VMR_QB_HOME -ErrorAction SilentlyContinue } else { $env:VMR_QB_HOME = $original } }
    }
}
finally {
    foreach ($created in $script:TempDirs) { Remove-Item -LiteralPath $created -Recurse -Force -ErrorAction SilentlyContinue }
}

if ($failures) { Write-Host "$failures tolerant-routing test(s) failed." -ForegroundColor Red; exit 1 }
Write-Host 'All tolerant-routing tests passed.' -ForegroundColor Green
