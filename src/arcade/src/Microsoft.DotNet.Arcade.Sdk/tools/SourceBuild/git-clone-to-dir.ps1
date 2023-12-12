<#
.SYNOPSIS
Creates a git clone of <source> into <dest> directory, optionally saving WIP changes.

.PARAMETER Source
The source Git directory

.PARAMETER Dest
The destination Git directory. Created if doesn't exist.

.PARAMETER Clean
If dest directory already exists, delete it.

.PARAMETER CopyWip
Transfer most types of uncommitted change into the destination directory. Useful for dev workflows.

#>

param (
  [Parameter(Mandatory=$true)]
  [string]$Source,
  [Parameter(Mandatory=$true)]
  [string]$Dest,
  [switch]$Clean,
  [switch]$CopyWip
)

if (-not (Test-Path $Source -PathType Container)) {
  Write-Host "-Source not a directory: $Source"
  exit 1
}

Write-Host "Cloning repository at: $Source -> $Dest ..."

if (Test-Path $Dest) {
  Write-Host "Destination already exists!"
  if (Test-Path $Dest -PathType Leaf) {
    Write-Host "Existing destination is a regular file, not a directory. This is unusual: aborting."
    exit 1
  }
  elseif ($Clean) {
    Write-Host "Clean is enabled: removing $dest and continuing..."
    Remove-Item -Path $Dest -Recurse -Force
  }
  else {
    Write-Host "Clean is not enabled: aborting."
    exit 1
  }
}

Push-Location -Path $Source

if (-not (Test-Path $Dest)) {
  New-Item -Path $Dest -ItemType Directory | Out-Null

  if ($CopyWip) {
    # Copy over changes that haven't been committed, for dev inner loop.
    # This gets changes (whether staged or not) but misses untracked files.
    $stashCommit = (git stash create)

    if ($stashCommit) {
      Write-Host "WIP changes detected: created temporary stash $stashCommit to transfer to inner repository..."
    }
    else {
      Write-Host "No WIP changes detected..."
    }
  }

  Write-Host "Creating empty clone at: $Dest"

  # Combine the results of git rev-parse --git-dir with the "shallow" subdirectory
  # to get the full path to the shallow file.

  $shallowFile = Join-Path (git rev-parse --git-dir) "shallow"

  if (Test-Path $shallowFile) {
    Write-Host "Source repository is shallow..."
    if ($stashCommit) {
      Write-Host "WIP stash is not supported in a shallow repository: aborting."
      exit 1
    }

    # If source repo is shallow, old versions of Git refuse to clone it to another directory. First,
    # remove the 'shallow' file to trick Git into allowing the clone.
    $shallowContent = Get-Content -Path $shallowFile
    Remove-Item -Path $shallowFile

    # Then, run the clone:
      # * 'depth=1' avoids encountering the leaf commit in the shallow repo that points to a parent
      #   that doesn't exist. (The commit marked "grafted".) Git would fail here, otherwise.
      # * '--no-local' allows a shallow clone from a git dir on the same filesystem. This means the
      #   clone will not use hard links and takes up more space. However, since we're doing a shallow
      #   clone anyway, the difference is probably not significant. (This has not been measured.)
      # * '-c protocol.file.allow=alwyas allows cloning from the local filesystem even on newer versions
      #   of git which have disabled this for security reasons.
    git clone -c protocol.file.allow=always --depth=1 --no-local --no-checkout "$Source" "$Dest"

    # Restore the 'shallow' file
    Set-Content -Path $shallowFile -Value $shallowContent
  } else {
    git clone -c protocol.file.allow=always --no-checkout "$Source" "$Dest"
  }

  echo "Checking out sources..."
  # If no changes were stashed, stashCommit is empty string, and this is a simple checkout.
  git -C $Dest checkout $stashCommit
  echo "Clone complete: $Source -> $Dest"

}

Pop-Location
