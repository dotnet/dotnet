Set-StrictMode -Version Latest

# daily/workspace.ps1 - Run isolation: the per-run directory the nested model works in, and the claims that keep two runs apart.
# The nested copilot starts in <state>\run\<stamp>, which holds everything it may read without any other grant: its
# instructions (AGENTS.md), the reference documents each stage is told to read, and the task documents it is handed.
# Runs never share a directory, so a second run cannot rewrite the instructions a first one is reading.
# Two claims protect the files that are named by date. A run holds the DAILY claim for its state and date, so a second run for
# the same day stops instead of writing the same reports and context at the same time. A send holds the EMAIL claim, created
# atomically before Outlook is asked to send, so a day is never mailed twice by accident. Defines functions only.
. (Join-Path $PSScriptRoot '..\common\core.ps1')

# The reference documents each stage reads. A stage that needs none is absent.
$script:QBStageReferences = [ordered]@{
    'build-health'    = @('triage.md')
    'overall-summary' = @('report-contract.md', 'triage.md')
}
$script:QBRunDirectoryPattern = '^\d{4}-\d{2}-\d{2}-\d{6}-\d{7}$'

function New-QBRunWorkspace {
    <#
    .SYNOPSIS
    Creates <RunDir>\<Stamp> with AGENTS.md, the staged reference documents and a prompts folder, and prunes old ones.
    .DESCRIPTION
    triage.md is staged with its generated routing block rendered from the ACTIVE routing, so the model reads the same
    owners and channels the prompts and validators use, including this QB's local additions. The public copy is not touched.
    Returns the directory, the prompts folder and a name -> staged path map of the references.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][string] $RunDir,
        [Parameter(Mandatory)][ValidatePattern('^\d{4}-\d{2}-\d{2}-\d{6}-\d{7}$')][string] $Stamp,
        [Parameter(Mandatory)][string] $ReferencesRoot,
        # The time zone the report date follows, shown in the instructions' "Generated:" header. Blank: this computer's.
        [string] $TimeZoneId,
        [ValidateRange(2, 1000)][int] $Keep = 10
    )
    $path = Join-Path $RunDir $Stamp
    $promptsDirectory = Join-Path $path 'prompts'
    $referenceDirectory = Join-Path $path 'references'
    foreach ($directory in @($path, $promptsDirectory, $referenceDirectory)) { [void][IO.Directory]::CreateDirectory($directory) }

    $template = [IO.File]::ReadAllText((Join-Path $ReferencesRoot 'runner-instructions.md'))
    [IO.File]::WriteAllText((Join-Path $path 'AGENTS.md'), (Expand-VmrQBInstructionTemplate -Template $template -TimeZoneId $TimeZoneId), [Text.UTF8Encoding]::new($false))

    $staged = [ordered]@{}
    foreach ($name in @($script:QBStageReferences.Values | ForEach-Object { $_ } | Sort-Object -Unique)) {
        $source = Join-Path $ReferencesRoot $name
        if (-not (Test-Path -LiteralPath $source -PathType Leaf)) { throw "The reference document '$name' is missing from $ReferencesRoot; a stage is told to read it." }
        $copy = Join-Path $referenceDirectory $name
        Copy-Item -LiteralPath $source -Destination $copy -Force
        if ($name -eq 'triage.md') { [void](Sync-VmrQBGeneratedBlock -Path $copy -Name routing -Content (Get-VmrQBRoutingDocMarkdown -Active)) }
        $staged[$name] = $copy
    }

    # Keep the newest few for diagnosis (each holds the task documents a run was given); never the one just made.
    $older = @(Get-ChildItem -LiteralPath $RunDir -Directory -ErrorAction SilentlyContinue |
        Where-Object { $_.Name -match $script:QBRunDirectoryPattern -and $_.Name -ne $Stamp } | Sort-Object Name -Descending | Select-Object -Skip ($Keep - 1))
    foreach ($directory in $older) { Remove-Item -LiteralPath $directory.FullName -Recurse -Force -ErrorAction SilentlyContinue }

    return [pscustomobject]@{ Path = $path; PromptsDirectory = $promptsDirectory; References = $staged }
}

function Get-QBStageReferencePath {
    <#
    .SYNOPSIS
    The staged reference documents a stage is told to read (empty for a stage that needs none).
    #>
    [CmdletBinding()]
    param([Parameter(Mandatory)][object] $Workspace, [Parameter(Mandatory)][string] $Stage)
    if (-not $script:QBStageReferences.Contains($Stage)) { return @() }
    return @($script:QBStageReferences[$Stage] | ForEach-Object { [string]$Workspace.References[$_] })
}

function Enter-QBDailyClaim {
    <#
    .SYNOPSIS
    Takes the exclusive claim on one state's daily run for one date, or says who holds it.
    .DESCRIPTION
    The claim is an operating-system lock on <logs>\daily-<date>.lock, held by the returned stream until it is disposed or the
    process ends. A crashed run releases it, so a stale file never blocks anything. Returns the stream, or $null with Holder set
    to what the lock file says when another live run holds it.
    #>
    [CmdletBinding()]
    param([Parameter(Mandatory)][string] $LogDirectory, [Parameter(Mandatory)][ValidatePattern('^\d{4}-\d{2}-\d{2}$')][string] $Date, [Parameter(Mandatory)][ref] $Holder)
    [void][IO.Directory]::CreateDirectory($LogDirectory)
    $path = Join-Path $LogDirectory "daily-$Date.lock"
    try { $stream = [IO.FileStream]::new($path, [IO.FileMode]::OpenOrCreate, [IO.FileAccess]::ReadWrite, [IO.FileShare]::Read) }
    catch {
        # A constructor failure arrives wrapped; exactly an IOException is "someone else holds it".
        $inner = if ($null -ne $_.Exception.InnerException) { $_.Exception.InnerException } else { $_.Exception }
        if ($inner.GetType() -ne [IO.IOException]) { throw }
        $Holder.Value = try {
            $reader = [IO.FileStream]::new($path, [IO.FileMode]::Open, [IO.FileAccess]::Read, [IO.FileShare]::ReadWrite)
            try { [IO.StreamReader]::new($reader).ReadToEnd().Trim() } finally { $reader.Dispose() }
        } catch { 'another run' }
        return $null
    }
    $stream.SetLength(0)
    $note = [Text.Encoding]::UTF8.GetBytes((ConvertTo-Json -InputObject ([ordered]@{ date = $Date; pid = $PID; startedAtUtc = [DateTimeOffset]::UtcNow.ToString('o') }) -Compress))
    $stream.Write($note, 0, $note.Length)
    $stream.Flush()
    return $stream
}

function Enter-QBEmailClaim {
    <#
    .SYNOPSIS
    Claims the right to send one email, atomically, before anything is sent. Throws when an earlier attempt already holds it.
    .DESCRIPTION
    The claim is a file created with CreateNew beside the receipt, so of any number of processes exactly one gets it. A claim that
    is still there means an earlier send began and either succeeded (there is a receipt) or was interrupted after it may have been
    delivered (there is none); in both cases sending again could mail the day twice, so it is refused unless -Force. A send that
    fails before anything was submitted releases its claim (Clear-QBEmailClaim) so it can simply be retried.
    #>
    [CmdletBinding()]
    param([Parameter(Mandatory)][string] $ClaimPath, [string] $ReceiptPath, [string] $Subject = '', [switch] $Force)
    if ($Force -and (Test-Path -LiteralPath $ClaimPath)) { Remove-Item -LiteralPath $ClaimPath -Force }
    $note = [Text.Encoding]::UTF8.GetBytes((ConvertTo-Json -InputObject ([ordered]@{ claimedAtUtc = [DateTimeOffset]::UtcNow.ToString('o'); pid = $PID; subject = $Subject }) -Compress))
    try {
        $stream = [IO.FileStream]::new($ClaimPath, [IO.FileMode]::CreateNew, [IO.FileAccess]::Write, [IO.FileShare]::None)
        try { $stream.Write($note, 0, $note.Length) } finally { $stream.Dispose() }
    }
    catch {
        $inner = if ($null -ne $_.Exception.InnerException) { $_.Exception.InnerException } else { $_.Exception }
        if ($inner.GetType() -ne [IO.IOException] -or -not (Test-Path -LiteralPath $ClaimPath -PathType Leaf)) { throw }
        $status = ''
        if ($ReceiptPath -and (Test-Path -LiteralPath $ReceiptPath -PathType Leaf)) {
            try { $status = [string](Get-Content -LiteralPath $ReceiptPath -Raw | ConvertFrom-Json).status } catch { $status = '' }
        }
        $sent = $status -eq 'sent'
        $detail = if ($sent) { 'It was already sent (there is a receipt).' } else { 'An earlier send began and left no receipt, so it may already have been delivered: check your Sent folder.' }
        throw "Not sending: this email already has a claim ($ClaimPath). $detail Run with -Force to send again."
    }
}

function Clear-QBEmailClaim {
    <# .SYNOPSIS Releases an email claim after a send that failed before anything was submitted, so it can be retried. #>
    [CmdletBinding()]
    param([Parameter(Mandatory)][string] $ClaimPath)
    if (Test-Path -LiteralPath $ClaimPath -PathType Leaf) { Remove-Item -LiteralPath $ClaimPath -Force -ErrorAction SilentlyContinue }
}
