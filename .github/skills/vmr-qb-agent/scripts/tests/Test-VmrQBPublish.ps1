[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot '..\verify\publish.ps1')

$failures = 0
function Check([string] $Name, [scriptblock] $Body) {
    try { & $Body; Write-Host "PASS: $Name" -ForegroundColor Green }
    catch { $script:failures++; Write-Host "FAIL: $Name -> $($_.Exception.Message)" -ForegroundColor Red }
}
function Assert($Condition, [string] $Message) { if (-not $Condition) { throw $Message } }

function New-Tree {
    $root = Join-Path ([IO.Path]::GetTempPath()) ('vmrqb-publish-' + [guid]::NewGuid().ToString('N'))
    $skill = Join-Path $root 'vmr-qb-demo'
    [void][IO.Directory]::CreateDirectory((Join-Path $skill 'scripts'))
    [void][IO.Directory]::CreateDirectory((Join-Path $skill 'references'))
    @'
---
name: vmr-qb-demo
description: A demonstration skill used only by the publish-readiness tests; long enough to be meaningful.
---
# Demo
Run `Do-Thing.ps1` then read [the contract](references/contract.md).
'@ | Set-Content -LiteralPath (Join-Path $skill 'SKILL.md') -Encoding utf8
    # Set-Content ends the here-string above with CRLF; rewrite it as pure LF so the fixture is clean for the format check.
    $skillMd = Join-Path $skill 'SKILL.md'
    [IO.File]::WriteAllText($skillMd, ([IO.File]::ReadAllText($skillMd) -replace "`r`n", "`n"), [Text.UTF8Encoding]::new($false))
    'contract' | Set-Content -LiteralPath (Join-Path $skill 'references\contract.md') -Encoding utf8
    'Write-Output "hi"' | Set-Content -LiteralPath (Join-Path $skill 'scripts\Do-Thing.ps1') -Encoding utf8
    return $root
}
# Violations are built from parts so this file does not itself contain what the scanner rejects.
$at = '@'
$realish = 'a.person' + $at + 'contoso.com'
$realish2 = 'b.person' + $at + 'contoso.com'
$profile = 'C:\Users\' + 'jsmith'
$machine = 'C:\proj' + 'ects\SecretRepo\y'
$teams = 'https://teams.' + 'microsoft.com/l/message/19:abc123def4567890abc123def4567890' + $at + 'thread.' + 'skype/1790360326156'
$key = '-----BEGIN ' + 'RSA PRIVATE KEY-----'
$session = '%USERPROFILE%\.cop' + 'ilot\session-' + 'state\00000000-0000-0000-0000-000000000000\files'
$session2 = '~/.cop' + 'ilot/session-' + 'state/00000000-0000-0000-0000-000000000000/x'
function Get-Findings([string] $Root, [string] $Severity) { @(Invoke-VmrQBPublishReadiness -Root $Root | Where-Object { -not $Severity -or $_.severity -eq $Severity }) }

Check 'A clean tree has no findings' {
    $root = New-Tree
    try { $f = @(Get-Findings $root); Assert ($f.Count -eq 0) "unexpected findings: $(($f | ForEach-Object { "$($_.check):$($_.message)" }) -join ' | ')" }
    finally { Remove-Item $root -Recurse -Force -ErrorAction SilentlyContinue }
}

Check 'A path into a private Copilot session folder blocks publishing' {
    $root = New-Tree
    try {
        "Fixture: $session\snapshot.json" | Set-Content -LiteralPath (Join-Path $root 'vmr-qb-demo\references\a.md') -Encoding utf8
        "Fixture: $session2" | Set-Content -LiteralPath (Join-Path $root 'vmr-qb-demo\references\b.md') -Encoding utf8
        $f = @(Get-Findings $root fail | Where-Object { $_.message -match 'Copilot session' })
        Assert ($f.Count -ge 2) "expected a finding in each file, got $($f.Count)"
    }
    finally { Remove-Item $root -Recurse -Force -ErrorAction SilentlyContinue }
}
Check 'Mixed line endings and a missing final newline are flagged as warnings, not blocks' {
    $root = New-Tree
    try {
        [IO.File]::WriteAllText((Join-Path $root 'vmr-qb-demo\references\mixed.md'), "one`r`ntwo`nthree`n", [Text.UTF8Encoding]::new($false))
        [IO.File]::WriteAllText((Join-Path $root 'vmr-qb-demo\references\nonewline.md'), 'no newline at the end', [Text.UTF8Encoding]::new($false))
        [IO.File]::WriteAllText((Join-Path $root 'vmr-qb-demo\references\crlf-only.md'), "one`r`ntwo`r`n", [Text.UTF8Encoding]::new($false))
        $f = @(Get-Findings $root | Where-Object check -eq 'format')
        Assert ($f.Count -eq 2) "expected the mixed and the no-newline file only, got $($f.Count): $($f.path -join ', ')"
        Assert (@($f | Where-Object severity -ne 'warn').Count -eq 0) 'format findings must be warnings'
        Assert (@($f.path) -match 'mixed\.md') 'mixed endings must be reported'
        Assert (@($f.path) -match 'nonewline\.md') 'a missing final newline must be reported'
        Assert (@(Get-Findings $root fail).Count -eq 0) 'format findings must not block publishing'
    }
    finally { Remove-Item $root -Recurse -Force -ErrorAction SilentlyContinue }
}

Check 'Binaries are blocked by extension and by content, because the VMR removes them' {
    $root = New-Tree
    try {
        [IO.File]::WriteAllBytes((Join-Path $root 'vmr-qb-demo\scripts\native.dll'), [byte[]](0x4D, 0x5A, 0, 0))
        [IO.File]::WriteAllBytes((Join-Path $root 'vmr-qb-demo\references\renamed.txt'), [byte[]](1, 2, 0, 3))
        $f = @(Get-Findings $root fail | Where-Object check -eq 'binary')
        Assert ($f.Count -eq 2) "expected 2 binary findings, got $($f.Count)"
        Assert (@($f.path) -match 'native\.dll') 'the .dll must be reported'
        Assert (@($f.path) -match 'renamed\.txt') 'a renamed binary (NUL bytes) must be reported too'
    }
    finally { Remove-Item $root -Recurse -Force -ErrorAction SilentlyContinue }
}

Check 'A real email address blocks publishing, example addresses do not' {
    $root = New-Tree
    try {
        "Reports go to $realish and $realish2" | Set-Content -LiteralPath (Join-Path $root 'vmr-qb-demo\references\a.md') -Encoding utf8
        ("Use you$($at)microsoft.com or qb$($at)microsoft.com or first.last$($at)example.co.uk") | Set-Content -LiteralPath (Join-Path $root 'vmr-qb-demo\references\b.md') -Encoding utf8
        $f = @(Get-Findings $root fail | Where-Object check -eq 'identity')
        Assert ($f.Count -eq 2) "expected 2 identity findings (both real addresses), got $($f.Count): $($f.message -join ' | ')"
        Assert (@($f.path | Select-Object -Unique) -eq 'vmr-qb-demo\references\a.md') 'only a.md has real addresses'
    }
    finally { Remove-Item $root -Recurse -Force -ErrorAction SilentlyContinue }
}

Check 'A personal profile path and a machine path are caught; neutral examples are not' {
    $root = New-Tree
    try {
        "Saved to $profile\AppData\x and $machine" | Set-Content -LiteralPath (Join-Path $root 'vmr-qb-demo\references\a.md') -Encoding utf8
        'Use C:\runs\unique-run or C:\Users\<you>\x or /home/runner/work' | Set-Content -LiteralPath (Join-Path $root 'vmr-qb-demo\references\b.md') -Encoding utf8
        $all = Get-Findings $root
        Assert (@($all | Where-Object { $_.check -eq 'identity' -and $_.path -match 'a\.md' }).Count -eq 1) 'the profile path must be a blocking identity finding'
        Assert (@($all | Where-Object { $_.check -eq 'path' -and $_.path -match 'a\.md' }).Count -eq 1) 'the machine path must be a warning'
        Assert (@($all | Where-Object { $_.path -match 'b\.md' }).Count -eq 0) "neutral examples were flagged: $(($all | Where-Object { $_.path -match 'b\.md' } | ForEach-Object message) -join ' | ')"
    }
    finally { Remove-Item $root -Recurse -Force -ErrorAction SilentlyContinue }
}

Check 'Teams message permalinks and conversation ids block publishing' {
    $root = New-Tree
    try {
        "See $teams" | Set-Content -LiteralPath (Join-Path $root 'vmr-qb-demo\references\a.md') -Encoding utf8
        $f = @(Get-Findings $root fail)
        Assert (@($f | Where-Object check -eq 'internal-link').Count -eq 1) 'the permalink must be a blocking internal-link finding'
        Assert (@($f | Where-Object check -eq 'identity').Count -eq 0) 'a conversation id must not also be reported as an email address'
    }
    finally { Remove-Item $root -Recurse -Force -ErrorAction SilentlyContinue }
}

Check 'Secrets are caught' {
    $root = New-Tree
    try {
        $fake = 'ghp_' + ('a1B2c3D4e5' * 4)
        "token = $fake" | Set-Content -LiteralPath (Join-Path $root 'vmr-qb-demo\references\a.md') -Encoding utf8
        $key | Set-Content -LiteralPath (Join-Path $root 'vmr-qb-demo\references\b.md') -Encoding utf8
        $f = @(Get-Findings $root fail | Where-Object check -eq 'secret')
        Assert ($f.Count -eq 2) "expected 2 secret findings, got $($f.Count)"
    }
    finally { Remove-Item $root -Recurse -Force -ErrorAction SilentlyContinue }
}

Check 'Per-QB state beside the scripts is blocked' {
    $root = New-Tree
    try {
        '{}' | Set-Content -LiteralPath (Join-Path $root 'vmr-qb-demo\qb-config.json') -Encoding utf8
        '{}' | Set-Content -LiteralPath (Join-Path $root 'vmr-qb-demo\issue-history.json') -Encoding utf8
        [void][IO.Directory]::CreateDirectory((Join-Path $root 'vmr-qb-demo\reports'))
        'x' | Set-Content -LiteralPath (Join-Path $root 'vmr-qb-demo\reports\today.md') -Encoding utf8
        $f = @(Get-Findings $root fail | Where-Object check -eq 'state')
        Assert ($f.Count -eq 3) "expected 3 state findings, got $($f.Count): $($f.path -join ', ')"
    }
    finally { Remove-Item $root -Recurse -Force -ErrorAction SilentlyContinue }
}

Check 'A document that names a missing script or a missing link target is blocked' {
    $root = New-Tree
    try {
        "Run Export-Things.ps1 now`nand see [missing](references/nope.md) and [web](https://example.com/x.md)`nA documented placeholder such as ``Support - [<name>](<url>)`` is not a file link." | Set-Content -LiteralPath (Join-Path $root 'vmr-qb-demo\references\a.md') -Encoding utf8
        $f = @(Get-Findings $root fail | Where-Object check -eq 'dangling-reference')
        Assert ($f.Count -eq 2) "expected 2 dangling references, got $($f.Count): $($f.message -join ' | ')"
        Assert (@($f.message) -match 'Export-Things\.ps1') 'the missing script must be named'
        Assert (@($f.message) -match 'nope\.md') 'the missing link target must be named'
    }
    finally { Remove-Item $root -Recurse -Force -ErrorAction SilentlyContinue }
}

Check 'A script named after ./ or with folders is checked too: a removed command, and a path that does not lead to the script, are blocked' {
    $root = New-Tree
    try {
        $lines = @(
            'Run ./Do-Thing.ps1 from the scripts folder.',                    # exists directly under scripts/
            'Or ./scripts/Do-Thing.ps1 from the skill root.',                 # exists relative to the skill root
            'The removed entry ./qb.ps1 help lists nothing any more.',        # the command that was removed: no such name
            'A wrong folder: ./history/Do-Thing.ps1 does not exist there.',   # the name exists, that path does not
            'A missing script in a real-looking folder: ./daily/Gone.ps1.')   # no such name
        ($lines -join "`n") | Set-Content -LiteralPath (Join-Path $root 'vmr-qb-demo\references\b.md') -Encoding utf8
        $f = @(Get-Findings $root fail | Where-Object { $_.check -eq 'dangling-reference' -and $_.path -match 'b\.md' })
        $messages = @($f | ForEach-Object message)
        Assert ($f.Count -eq 3) "expected 3 dangling references, got $($f.Count): $($messages -join ' | ')"
        Assert (@($messages) -match 'qb\.ps1') 'the removed command directly after ./ must be caught'
        Assert (@($messages) -match 'history[\\/]Do-Thing\.ps1.*no such path') 'a path that does not lead to an existing script must be caught'
        Assert (@($messages) -match 'daily[\\/]Gone\.ps1') 'a missing script in a folder must be caught'
        Assert (@($messages) -notmatch '\./Do-Thing\.ps1|scripts[\\/]Do-Thing') 'a correct ./ or folder reference must pass'
    }
    finally { Remove-Item $root -Recurse -Force -ErrorAction SilentlyContinue }
}
Check 'A script that does not parse and a SKILL.md with bad front matter are blocked' {
    $root = New-Tree
    try {
        'function Broken { if ( }' | Set-Content -LiteralPath (Join-Path $root 'vmr-qb-demo\scripts\Broken.ps1') -Encoding utf8
        $other = Join-Path $root 'vmr-qb-other'; [void][IO.Directory]::CreateDirectory($other)
        "---`nname: wrong-name`ndescription: short`n---`n# x" | Set-Content -LiteralPath (Join-Path $other 'SKILL.md') -Encoding utf8
        $nofront = Join-Path $root 'vmr-qb-third'; [void][IO.Directory]::CreateDirectory($nofront)
        '# no front matter' | Set-Content -LiteralPath (Join-Path $nofront 'SKILL.md') -Encoding utf8
        $f = @(Get-Findings $root fail)
        Assert (@($f | Where-Object check -eq 'parse').Count -ge 1) 'parse error not reported'
        $front = @($f | Where-Object check -eq 'frontmatter')
        Assert ($front.Count -ge 3) "expected name mismatch + short description + missing block, got $($front.Count): $($front.message -join ' | ')"
    }
    finally { Remove-Item $root -Recurse -Force -ErrorAction SilentlyContinue }
}

Check 'A description written as a YAML block (>- or |) is read as its text, so a long one passes and a short one is still blocked' {
    $root = New-Tree
    try {
        $skillMd = Join-Path $root 'vmr-qb-demo\SKILL.md'
        $folded = "---`nname: vmr-qb-demo`ndescription: >-`n  A demonstration skill that wraps its description over`n  several lines, as YAML allows, and is long enough to count.`n---`n# Demo`n"
        [IO.File]::WriteAllText($skillMd, $folded, [Text.UTF8Encoding]::new($false))
        Assert (@(Get-Findings $root | Where-Object check -eq 'frontmatter').Count -eq 0) 'a folded description must be read as its text'
        [IO.File]::WriteAllText($skillMd, $folded.Replace('>-', '|'), [Text.UTF8Encoding]::new($false))
        Assert (@(Get-Findings $root | Where-Object check -eq 'frontmatter').Count -eq 0) 'a literal block must be read as its text'
        [IO.File]::WriteAllText($skillMd, "---`nname: vmr-qb-demo`ndescription: >-`n  Too short.`n---`n# Demo`n", [Text.UTF8Encoding]::new($false))
        Assert (@(Get-Findings $root fail | Where-Object { $_.check -eq 'frontmatter' -and $_.message -match 'meaningful' }).Count -eq 1) 'a short block description is still blocked'
    }
    finally { Remove-Item $root -Recurse -Force -ErrorAction SilentlyContinue }
}

Check 'Size is a warning and internal links are an inventory, not a block' {
    $root = New-Tree
    try {
        ('x' * 300KB) | Set-Content -LiteralPath (Join-Path $root 'vmr-qb-demo\references\big.md') -Encoding utf8
        'https://dev.azure.com/dnceng/internal/_build/results?buildId=1 and https://dev.azure.com/mseng/1ES/_workitems/edit/2' | Set-Content -LiteralPath (Join-Path $root 'vmr-qb-demo\references\links.md') -Encoding utf8
        $all = Get-Findings $root
        Assert (@($all | Where-Object { $_.severity -eq 'fail' }).Count -eq 0) 'size and internal links must not block'
        Assert (@($all | Where-Object { $_.check -eq 'size' -and $_.severity -eq 'warn' }).Count -eq 1) 'oversize file must warn'
        $info = @($all | Where-Object { $_.check -eq 'internal-links' })
        Assert ($info.Count -eq 1 -and $info[0].message -match '^2 reference') "internal-link inventory wrong: $($info.message -join ' | ')"
    }
    finally { Remove-Item $root -Recurse -Force -ErrorAction SilentlyContinue }
}

Check 'Only the named skill folder is scanned, so sibling skills in a shared tree are left alone' {
    $root = New-Tree
    try {
        $sibling = Join-Path $root 'other-team-skill'; [void][IO.Directory]::CreateDirectory($sibling)
        "---`nname: wrong-name`ndescription: short`n---`n# x" | Set-Content -LiteralPath (Join-Path $sibling 'SKILL.md') -Encoding utf8
        [IO.File]::WriteAllBytes((Join-Path $sibling 'native.dll'), [byte[]](0x4D, 0x5A, 0, 0))
        $all = @(Invoke-VmrQBPublishReadiness -Root $root)
        Assert (@($all | Where-Object { $_.severity -eq 'fail' }).Count -ge 2) 'the unscoped scan must see the sibling skill problems'
        $scoped = @(Invoke-VmrQBPublishReadiness -Root $root -SkillFolder 'vmr-qb-demo')
        Assert ($scoped.Count -eq 0) "the scoped scan must ignore the sibling skill: $(($scoped | ForEach-Object { "$($_.check):$($_.path)" }) -join ' | ')"
    }
    finally { Remove-Item $root -Recurse -Force -ErrorAction SilentlyContinue }
}

Check 'local/ must be git-ignored; an ignored local/ is not scanned or counted, an unignored one blocks' {
    $root = New-Tree
    try {
        $local = Join-Path $root 'vmr-qb-demo\local'
        [void][IO.Directory]::CreateDirectory((Join-Path $local 'knowledge')); [void][IO.Directory]::CreateDirectory((Join-Path $local 'reports'))
        ('x' * 300KB) | Set-Content -LiteralPath (Join-Path $local 'issue-history.json') -Encoding utf8
        '{"contacts":{}}' | Set-Content -LiteralPath (Join-Path $local 'knowledge\team-contacts.json') -Encoding utf8
        '{}' | Set-Content -LiteralPath (Join-Path $local 'qb-config.json') -Encoding utf8
        "$realish notes" | Set-Content -LiteralPath (Join-Path $local 'reports\today.md') -Encoding utf8
        $f = @(Get-Findings $root fail)
        Assert (@($f | Where-Object { $_.check -eq 'state' -and $_.path -eq 'vmr-qb-demo/local' }).Count -eq 1) 'an unignored local/ must block publishing, named once'
        [IO.File]::WriteAllText((Join-Path $root 'vmr-qb-demo\.gitignore'), "# one QB's data`n/local/`n", [Text.UTF8Encoding]::new($false))
        $all = @(Get-Findings $root)
        Assert ($all.Count -eq 0) "an ignored local/ must produce no findings (no identity, no size warning): $(($all | ForEach-Object { "$($_.check):$($_.path)" }) -join ' | ')"
    }
    finally { Remove-Item $root -Recurse -Force -ErrorAction SilentlyContinue }
}

Check 'Private state that Git already tracks or has staged blocks publishing even though local/ is ignored; an ignored, untracked local/ passes' {
    if (-not (Get-Command git -ErrorAction SilentlyContinue)) { Write-Host '  (git not installed; skipped)' -ForegroundColor DarkGray; return }
    $root = New-Tree
    try {
        & git -C $root init -q 2>$null
        $skill = Join-Path $root 'vmr-qb-demo'
        $local = Join-Path $skill 'local'
        [void][IO.Directory]::CreateDirectory($local)
        [IO.File]::WriteAllText((Join-Path $skill '.gitignore'), "/local/`n", [Text.UTF8Encoding]::new($false))
        'Synthetic private note.' | Set-Content -LiteralPath (Join-Path $local 'note.txt') -Encoding utf8
        Assert (@(Get-Findings $root fail | Where-Object check -eq 'state').Count -eq 0) 'an ignored, untracked local/ must pass'

        # A force-added file is in the index although the rule ignores it: the gate must not trust the rule alone.
        & git -C $root add -f -- 'vmr-qb-demo/local/note.txt' 2>$null
        $f = @(Get-Findings $root fail | Where-Object { $_.check -eq 'state' -and $_.path -eq 'vmr-qb-demo/local' })
        Assert ($f.Count -eq 1 -and $f[0].message -match 'tracks or has staged 1 file' -and $f[0].message -match 'git rm -r --cached -- vmr-qb-demo/local') "a staged file under an ignored local/ must block publishing, naming the fix: $($f.message -join ' | ')"
        Assert ($f[0].message -notmatch 'skip-worktree') 'the fix is to stop tracking the files, not to hide them'

        # Doing what the finding says clears it, and the working copy stays.
        & git -C $root rm -r --cached -q -- 'vmr-qb-demo/local' 2>$null
        Assert (Test-Path -LiteralPath (Join-Path $local 'note.txt')) 'git rm --cached keeps the working copy'
        Assert (@(Get-Findings $root fail | Where-Object check -eq 'state').Count -eq 0) 'once untracked, local/ passes again'

        # A sibling skill is only checked when it is in scope.
        $other = Join-Path $root 'other-team-skill\local'
        [void][IO.Directory]::CreateDirectory($other)
        'x' | Set-Content -LiteralPath (Join-Path $other 'kept.txt') -Encoding utf8
        & git -C $root add -f -- 'other-team-skill/local/kept.txt' 2>$null
        Assert (@(Invoke-VmrQBPublishReadiness -Root $root -SkillFolder 'vmr-qb-demo' | Where-Object { $_.check -eq 'state' }).Count -eq 0) 'a sibling skill that is not requested must be left alone'
    }
    finally { Remove-Item $root -Recurse -Force -ErrorAction SilentlyContinue }
}

Check 'Files that belong in local/ but sit in knowledge/ or references/ block publishing even when ignored' {
    $root = New-Tree
    try {
        $skill = Join-Path $root 'vmr-qb-demo'
        [void][IO.Directory]::CreateDirectory((Join-Path $skill 'knowledge'))
        [IO.File]::WriteAllText((Join-Path $skill '.gitignore'), "/local/`n/knowledge/team-contacts.json`n", [Text.UTF8Encoding]::new($false))
        foreach ($rel in 'knowledge\issue-history.seed.json', 'knowledge\team-contacts.json', 'knowledge\triage-context.json', 'knowledge\issue-aliases.json', 'references\guide-update-draft.md') {
            '{}' | Set-Content -LiteralPath (Join-Path $skill $rel) -Encoding utf8
        }
        $f = @(Get-Findings $root fail | Where-Object { $_.check -eq 'state' })
        Assert ($f.Count -eq 5) "all five misplaced files must be reported, got $($f.Count): $($f.path -join ', ')"
        Assert (@($f | Where-Object { $_.message -match 'Move it into vmr-qb-demo/local/' }).Count -eq 5) 'each finding must say where it belongs'
    }
    finally { Remove-Item $root -Recurse -Force -ErrorAction SilentlyContinue }
}
Check 'The public qb-config.template.json must stay blank; a local qb-config.local.json outside local/ is state' {
    $root = New-Tree
    try {
        $skill = Join-Path $root 'vmr-qb-demo'
        '{ "recipient": "", "model": "" }' | Set-Content -LiteralPath (Join-Path $skill 'qb-config.template.json') -Encoding utf8
        Assert (@(Get-Findings $root fail).Count -eq 0) 'a blank template is fine'
        "{ `"recipient`": `"$realish`" }" | Set-Content -LiteralPath (Join-Path $skill 'qb-config.template.json') -Encoding utf8
        $f = @(Get-Findings $root fail | Where-Object { $_.path -match 'qb-config\.template\.json' })
        Assert (@($f | Where-Object { $_.check -eq 'identity' -and $_.message -match "non-empty 'recipient'" }).Count -ge 1) 'a template carrying an address must block publishing'
        '{ "recipient": "" }' | Set-Content -LiteralPath (Join-Path $skill 'qb-config.template.json') -Encoding utf8
        '{ "recipient": "" }' | Set-Content -LiteralPath (Join-Path $skill 'qb-config.local.json') -Encoding utf8
        $f = @(Get-Findings $root fail | Where-Object { $_.check -eq 'state' -and $_.path -match 'qb-config\.local\.json' })
        Assert ($f.Count -eq 1) 'a local config beside the shipped files is state and must be reported'
    }
    finally { Remove-Item $root -Recurse -Force -ErrorAction SilentlyContinue }
}
Check 'Your local routing copy is fine inside local/ and blocks publishing anywhere else' {
    $root = New-Tree
    try {
        $skill = Join-Path $root 'vmr-qb-demo'
        [void][IO.Directory]::CreateDirectory((Join-Path $skill 'local\knowledge'))
        [void][IO.Directory]::CreateDirectory((Join-Path $skill 'knowledge'))
        [IO.File]::WriteAllText((Join-Path $skill '.gitignore'), "/local/`n", [Text.UTF8Encoding]::new($false))
        '{ "schemaVersion": 1 }' | Set-Content -LiteralPath (Join-Path $skill 'local\knowledge\routing.local.json') -Encoding utf8
        Assert (@(Get-Findings $root fail | Where-Object { $_.path -match 'routing\.local\.json' }).Count -eq 0) 'the copy inside local/ is yours and must not block publishing'
        '{ "schemaVersion": 1 }' | Set-Content -LiteralPath (Join-Path $skill 'knowledge\routing.local.json') -Encoding utf8
        '{ "schemaVersion": 1 }' | Set-Content -LiteralPath (Join-Path $skill 'routing.local.json') -Encoding utf8
        $f = @(Get-Findings $root fail | Where-Object { $_.check -eq 'state' -and $_.path -match 'routing\.local\.json' })
        Assert ($f.Count -ge 2) "a routing copy outside local/ must be reported in both places, got $($f.path -join ', ')"
    }
    finally { Remove-Item $root -Recurse -Force -ErrorAction SilentlyContinue }
}
Check 'A requested skill folder that is absent or empty fails closed instead of reading as all clear' {
    $root = New-Tree
    try {
        $f = @(Invoke-VmrQBPublishReadiness -Root $root -SkillFolder 'no-such-skill' | Where-Object { $_.severity -eq 'fail' -and $_.check -eq 'scope' })
        Assert ($f.Count -eq 1 -and $f[0].message -match 'No files were scanned') 'a missing requested folder must be a blocking scope finding'
        [void][IO.Directory]::CreateDirectory((Join-Path $root 'empty-skill'))
        $f = @(Invoke-VmrQBPublishReadiness -Root $root -SkillFolder 'empty-skill' | Where-Object { $_.severity -eq 'fail' -and $_.check -eq 'scope' })
        Assert ($f.Count -eq 1) 'an empty requested folder must be a blocking scope finding'
        $ok = @(Invoke-VmrQBPublishReadiness -Root $root -SkillFolder 'vmr-qb-demo' | Where-Object { $_.severity -eq 'fail' })
        Assert ($ok.Count -eq 0) 'a real requested folder must still scan clean'
    }
    finally { Remove-Item $root -Recurse -Force -ErrorAction SilentlyContinue }
}

Check 'Invoke-Verify.ps1 with its default arguments scans this skill and catches state placed beside the public files' {
    $guide = Join-Path (Get-VmrQBPackageRoot) 'docs\VMR-QB-Guide.md'
    if (-not (Test-Path -LiteralPath $guide)) { Write-Host '  (no docs/VMR-QB-Guide.md beside this skill; skipped)' -ForegroundColor DarkGray; return }
    $workspace = Join-Path ([IO.Path]::GetTempPath()) ('vmrqb-verify-clone-' + [guid]::NewGuid().ToString('N'))
    $savedHome = $env:VMR_QB_HOME
    try {
        # A clone-shaped tree: <repo>/.github/skills/vmr-qb-agent plus the Guide, with none of this QB's local data.
        $cloneSkill = Join-Path $workspace 'repo\.github\skills\vmr-qb-agent'
        [void][IO.Directory]::CreateDirectory($cloneSkill)
        [void][IO.Directory]::CreateDirectory((Join-Path $workspace 'repo\docs'))
        Copy-Item -LiteralPath $guide -Destination (Join-Path $workspace 'repo\docs\VMR-QB-Guide.md')
        foreach ($entry in Get-ChildItem -LiteralPath (Get-VmrQBPaths).SkillRoot -Force | Where-Object Name -ne 'local') { Copy-Item -LiteralPath $entry.FullName -Destination $cloneSkill -Recurse }
        $env:VMR_QB_HOME = Join-Path $workspace 'state'
        $verify = Join-Path $cloneSkill 'scripts\verify\Invoke-Verify.ps1'

        $clean = (& pwsh -NoProfile -NonInteractive -File $verify -Json 2>&1 | Out-String)
        Assert ($LASTEXITCODE -eq 0) "a clean clone must verify: $clean"

        [IO.File]::WriteAllText((Join-Path $cloneSkill 'issue-history.json'), '{}')
        $dirty = (& pwsh -NoProfile -NonInteractive -File $verify -Json 2>&1 | Out-String)
        Assert ($LASTEXITCODE -eq 1) 'state placed beside the public files must block publishing through the default entry'
        $findings = @($dirty | ConvertFrom-Json)
        Assert (@($findings | Where-Object { $_.area -eq 'publish' -and $_.check -eq 'state' -and $_.path -match 'issue-history\.json$' }).Count -eq 1) 'the finding must name the misplaced file'
    }
    finally {
        $env:VMR_QB_HOME = $savedHome
        Remove-Item $workspace -Recurse -Force -ErrorAction SilentlyContinue
    }
}

if ($failures) { Write-Host "$failures publish-readiness test(s) failed." -ForegroundColor Red; exit 1 }
Write-Host '22 offline publish-readiness tests. Temporary trees only; no network, email, or external changes.' -ForegroundColor Green
