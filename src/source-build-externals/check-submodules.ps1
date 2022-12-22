$answeredAll = $false   # has the user answered "yes to all" to init'ing submodules?
$ProjectRoot = $PSScriptRoot
$CleanAllSentinel = "$ProjectRoot\.cleansourcebuildsubmodules"
# How long to wait between polling for input on ReadKey.
# This affects apparent responsiveness so setting it fairly low.
$InputPollingTime = 100 # ms
$InputCountTimeout = (30 * 1000) / $InputPollingTime # ms

function WaitForInput() {
  $counter = 0
  while (-Not [Console]::KeyAvailable -and ($counter++ -lt $InputCountTimeout)) {
    Start-Sleep -Milliseconds $InputPollingTime
  }
  if ([Console]::KeyAvailable) {
    $k = [Console]::ReadKey()
    return $k
  }
  return $null
}

# ask for confirmation and initialize a submodule if approved.
function init_submodule($Path) {
  $done = $false
  while (-Not $done) {
    Write-Warning "submodule $Path does not appear to be initialized."
    Write-Host "Should I initialize it for you [Y]es / [n]o / [a]ll / [q]uit"
    $answer = WaitForInput
    if ($answer -eq $null -or $answer.KeyChar -ieq "a") {
      $script:answeredAll = $true
      $done = $true
      git submodule update --init --recursive $Path
    }
    elseif ($answer.KeyChar -ieq "q") {
      exit 1
    }
    elseif ($answer.KeyChar -ieq "y" -or $answer.Key -eq [System.ConsoleKey]::Spacebar -or $answer.Key -eq [System.ConsoleKey]::Enter) {
      $done = $true
      git submodule update --init --recursive $Path
    }
    elseif ($answer.KeyChar -ieq "n") {
      $done = $true
    }
    else {
      Write-Host "Didn't understand that ($($answer.KeyChar))"
    }
  }
}

# update a submodule to an expected commit.  We give different messages
# for being ahead vs being behind or diverged from the expected commit,
# so this function is parameterized.
function fix_submodule($Path, $ExpectedSha, $ActualSha, $Message, $Prompt) {
  $done = $false
  while (-Not $done) {
    Write-Warning $Message
    Write-Host $Prompt
    $answer = WaitForInput
    if ($answer -eq $null -or $answer.KeyChar -ieq "n" -or $answer.Key -eq [System.ConsoleKey]::Spacebar -or $answer.Key -eq [System.ConsoleKey]::Enter) {
      $done = $true
    }
    elseif ($answer.KeyChar -ieq "q") {
      exit 1
    }
    elseif ($answer.KeyChar -ieq "y") {
      # check if we have this commit locally and can skip the fetch
      git cat-file -e $expectedSha^`{commit`} 2>&1 | Out-Null
      if ($LastExitCode -ne 0) {
        git fetch
      }
      # double-check, we should have the commit now unless something
      # weird is going on.
      git cat-file -e $expectedSha^`{commit`} 2>&1 | Out-Null
      if ($LastExitCode -ne 0) {
        Write-Error "commit $expectedSha was not found in $path"
        Write-Host "The remote may have changed in source-build; run 'git submodule sync' and retry."
        Write-Host "Are you using a custom remote for this submodule?  You may need to pick up changes from upstream."
        Write-Host "Canceling remainder of checks."
        exit 1
      }
      git checkout $expectedSha
      $done = $true
    }
    else {
      Write-Host "Didn't understand that ($($answer.KeyChar))"
    }
  }
}

# clean a submodule if the user approves.
function clean_submodule($Path, $Message, $Prompt) {
  $done = $false
  while (-Not $done) {
    Write-Warning $Message
    Write-Host $Prompt
    $answer = WaitForInput
    if ($answer -eq $null -or $answer.KeyChar -ieq "n" -or $answer.Key -eq [System.ConsoleKey]::Spacebar -or $answer.Key -eq [System.ConsoleKey]::Enter) {
      $done = $true
    }
    elseif ($answer.KeyChar -ieq "a") {
      git clean -fxd
      git reset --hard HEAD
      New-Item -ItemType File $CleanAllSentinel | Out-Null
      $done = $true
    }
    elseif ($answer.KeyChar -ieq "q") {
      exit 1
    }
    elseif ($answer.KeyChar -ieq "y") {
      git clean -fxd
      git reset --hard HEAD
      $done = $true
    }
    else {
      Write-Host "Didn't understand that ($($answer.KeyChar))"
    }
  }
}

# We use the same script for checking the super-repo and the submodules.
# Having the first argument be "in-submodule" triggers this submodule behavior.
if ($args[0] -ieq "in-submodule") {
  $path = $args[1]
  $expectedSha = $args[2]
  $subcommit = git rev-parse HEAD
  if ($subcommit -ne $expectedSha) {
    # merge-base fails if the commit is missing, so check for that first.
    git cat-file -e $expectedSha^`{commit`} 2>&1 | Out-Null
    if ($LastExitCode -ne 0) {
      $mergeBase = "missing commit"
    }
    else {
      $mergeBase = git merge-base HEAD $expectedSha
    }
    if ($mergeBase -ne $expectedSha) {
      fix_submodule -Path $path -ExpectedSha $expectedSha -ActualSha $subcommit -Message "submodule $path, currently at $subcommit, has diverged from checked-in version $expectedSha" -Prompt "If you are changing a submodule branch or moving a submodule backwards, this is expected.`r`nShould I checkout $path to the expected commit $expectedSha [N]o / [y]es / [q]uit"
    }
    else {
      fix_submodule -Path $path -ExpectedSha $expectedSha -ActualSha $subcommit -Message "submodule $path, currently at $subcommit, is ahead of checked-in version $expectedSha" -Prompt "If you are updating a submodule, this is expected.`r`nShould I checkout $path to the expected commit $expectedSha [N]o / [y]es / [q]uit"
    }
  }
  $dirty = $false
  # check for staged changes and unstaged modifications
  git diff-index --quiet HEAD --
  $dirty = $dirty -or $LastExitCode
  # check for untracked new files
  $untracked = git ls-files --others --exclude-standard
  $dirty = $dirty -or (-Not [string]::IsNullOrWhitespace($untracked))
  if ($dirty) {
    if (Test-Path $CleanAllSentinel) {
      git clean -fxd
      git reset --hard HEAD
    }
    else {
      clean_submodule -Path $path -Message "submodule $path has uncommitted changes" -Prompt "Should I clean and reset $path (this will lose ALL uncommitted changes)? [N]o / [y]es / [a]ll / [q]uit"
    }
  }
}
# Main branch for super-repo behavior
else {
  # submodule foreach doesn't work until submodules are init'd, read the modules manually
  $modules = git config --file $ProjectRoot\.gitmodules --get-regexp path
  foreach ($m in $modules) {
    $m = $m.Split(' ')[1]
    $mWin = $m.Replace('/', '\')
    if (-Not (Test-Path "$ProjectRoot\$mWin\.git")) {
      if ($script:answeredAll) {
        git submodule update --init --recursive $m
      }
      else {
        init_submodule -Path $m
      }
    }
  }
  $ProjectRoot = $ProjectRoot.Replace('\', '/')
  # kick off the submodule behavior for each repo
  Remove-Item -Force -ErrorAction SilentlyContinue $CleanAllSentinel
  git submodule foreach --quiet --recursive "powershell $ProjectRoot/check-submodules.ps1 in-submodule `$path `$sha1"
  Remove-Item -Force -ErrorAction SilentlyContinue $CleanAllSentinel
}
