[CmdletBinding(PositionalBinding=$false)]
Param(
  [string][Alias('c')]$configuration = "Debug",
  [string]$platform = $null,
  [string] $projects,
  [string][Alias('v')]$verbosity = "minimal",
  [string] $msbuildEngine = $null,
  [bool] $warnAsError = $true,
  [bool] $nodeReuse = $true,
  [switch][Alias('r')]$restore,
  [switch] $deployDeps,
  [switch][Alias('b')]$build,
  [switch] $rebuild,
  [switch] $deploy,
  [switch][Alias('t')]$test,
  [switch] $integrationTest,
  [switch] $performanceTest,
  [switch] $sign,
  [switch] $pack,
  [switch] $publish,
  [switch] $clean,
  [switch][Alias('bl')]$binaryLog,
  [switch][Alias('nobl')]$excludeCIBinarylog,
  [switch] $ci,
  [switch] $prepareMachine,
  [string] $runtimeSourceFeed = '',
  [string] $runtimeSourceFeedKey = '',
  [switch] $excludePrereleaseVS,
  [switch] $nativeToolsOnMachine,
  [switch] $help,
  [Parameter(ValueFromRemainingArguments=$true)][String[]]$properties
)

$count = 0
$err = $null
$path = "$PSScriptRoot/common/build.ps1"
# CIBuild.cmd script specifies restore -build -test -sign -pack -publish -ci $args
$PSBoundParameters["restore"] = $true
$PSBoundParameters["build"] = $true
$PSBoundParameters["test"] = $true
$PSBoundParameters["sign"] = $true
$PSBoundParameters["pack"] = $true
$PSBoundParameters["publish"] = $true
$PSBoundParameters["ci"] = $true

$cmd = "$path $(foreach($pair in $PSBoundParameters.GetEnumerator()) { "-$($pair.Key)=$($pair.Value)"})"
while (3 -gt $count) { 
    $err = $null
    $count++
    try {
        & $path @PSBoundParameters
        if (0 -ne $LASTEXITCODE) {
            Write-Host ">>> Try ${count}: Command '$cmd' failed, with exit code $LASTEXITCODE."
            continue
        }
    }
    catch {
        $err = $_
        Write-Host ">>> Try ${count}: Command '$cmd' failed, with $_."
        continue
    }

    Write-Host ">>> Try ${count}: Command '$cmd' succeeded."
    break
}

if ($null -ne $err) { 
    throw $err
}

