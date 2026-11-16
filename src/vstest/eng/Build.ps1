[CmdletBinding(PositionalBinding = $false)]
Param(
  [string][Alias('c')]$configuration = "Debug",
  [string]$platform = $null,
  [string] $projects,
  [string][Alias('v')]$verbosity = "minimal",
  [string] $msbuildEngine = $null,
  [bool] $warnAsError = $true,
  [bool] $nodeReuse = $true,
  [switch] $buildCheck = $false,
  [switch][Alias('r')]$restore,
  [switch] $deployDeps,
  [switch][Alias('b')]$build,
  [switch] $rebuild,
  [switch] $deploy,
  [switch][Alias('t')]$test,
  [string] $filter,
  [switch] $smokeTest,
  [switch] $integrationTest,
  [switch] $performanceTest,
  [switch] $compatibilityTest,
  # Skip the build when running multiple categories from the integration tests. This is useful mostly in CI where we want to split the runs to different jobs, but
  # they all fall back to the same project and initialization.
  [switch] $skipIntegrationTestBuild,
  [switch] $compatibilityTestBuild,
  [switch] $sign,
  [switch] $pack,
  [switch] $publish,
  [switch] $clean,
  [switch][Alias('pb')]$productBuild,
  [switch]$fromVMR,
  [switch][Alias('bl')]$binaryLog,
  [switch][Alias('nobl')]$excludeCIBinarylog,
  [switch] $ci,
  [switch] $prepareMachine,
  [string] $runtimeSourceFeed = '',
  [string] $runtimeSourceFeedKey = '',
  [switch] $excludePrereleaseVS,
  [switch] $nativeToolsOnMachine,
  [switch] $help,
  [Parameter(ValueFromRemainingArguments = $true)][String[]]$properties
)

# Add steps that need to happen before build here

if ($properties -like "*TestRunnerAdditionalArguments*--filter*") {
  throw "Use --filter instead of passing filter as an additional argument to TestRunnerAdditionalArguments."
}

if ($smokeTest -and $integrationTest) {
  throw "Cannot specify both smoke and integration tests. Smoke tests are a subset of integration tests, so specifying both is redundant and will run all integration tests."
}

$filters = @()
# This translates to properties on test context, the only way MSTest allows us to pass info dynamically to AssemblyInitialize
$testParameters = @{}
if ($skipIntegrationTestBuild) {
  $testParameters['SkipIntegrationTestBuild'] = $true
}

if ($filter) {
  $filters += $filter
}

if ([System.Environment]::OSVersion.Platform -notlike "Win*") {
  $filters += "TestCategory!=Windows&TestCategory!=Windows-Review"
}

if ($smokeTest) {
  $filters += "TestCategory=Smoke"
}
else {
  # Don't exclude smoke tests if not specified explicitly, those are integration tests that should
  # run by default when running integration tests. We want to make sure we can run just Smoke tests,
  # but should not skip them when not specified.
  # $filters += "TestCategory!=Smoke"
}

if ($compatibilityTestBuild) {
  $testParameters['CompatibilityTestBuild'] = $true
}

if ($compatibilityTest) {
  $testParameters['BuildCompatibility'] = $true
  if (-not $integrationTest) {
    # We specified just compatibility so run just compatibility tests. If there are both
    # we need to run all, but we don't have a filter for uncategorized tests, https://github.com/microsoft/testfx/issues/5136
    # so we simply don't provide an include filter.
    $filters += "TestCategory=Compatibility"
  }
}
else {
  $filters += "TestCategory!=Compatibility"
}

if ($performanceTest) {
  # We don't have any perf tests in the library.integrationtest.csproj, so providing this alone will fail
  # but we will use it together with compatibility tests in a nightly run, so good enough for now.
  if (-not $integrationTest) {
    # We specified just perf tests so run just perf tests. If there are both
    # we need to run all, but we don't have a filter for uncategorized tests, https://github.com/microsoft/testfx/issues/5136
    # so we simply don't provide an include filter.
    $filters += "TestCategory=TelemetryPerf"
  }
  
}
else {
  $filters += "TestCategory!=TelemetryPerf"
}

$null = $PSBoundParameters.Remove("filter")
$null = $PSBoundParameters.Remove("smokeTest")
$null = $PSBoundParameters.Remove("compatibilityTest")
$null = $PSBoundParameters.Remove("performanceTest")
$null = $PSBoundParameters.Remove("skipIntegrationTestBuild")
$null = $PSBoundParameters.Remove("compatibilityTestBuild")

if ($integrationTest -or $performanceTest -or $compatibilityTest -or $smokeTest) {
  # Rest of the infra knows nothing about or additional categories for tests. They simply consider them
  # integration tests, so mark that.
  $PSBoundParameters['integrationTest'] = $true
  # This is also non-default, normally we would run also unit tests, but if we filter anything that matches 0 tests in a project
  # the project will fail.
  $PSBoundParameters['test'] = $false
}

if ($filters.Count -gt 0 -or $testParameters.Count -gt 0) {
  if ($filters.Count -gt 0) {
    
    # We have to double escape by '\"', otherwise the filter is passed as string with & in it and interpreted directly as a separate command to run.
    # Ignoring exit code 8 which means no tests found, otherwise we will fail the build when we run with a filter that doesn't match any test in a project, which is common when we have multiple projects and some of them don't have certain categories of tests. https://github.com/microsoft/testfx/issues/7457
    $filterString = "--filter \`"$($filters -join '&')\`""
    $filterParameters = "$filterString --ignore-exit-code 8"
  }

  $testParameterString = ($testParameters.GetEnumerator() | ForEach-Object { "--test-parameter $($_.Key)=$($_.Value)" }) -join ' '
  
  if (-not $PSBoundParameters.ContainsKey('properties')) {
    $PSBoundParameters['properties'] = @()
  }
  $PSBoundParameters['properties'] += "/p:TestRunnerExternalArguments=$filterParameters $testParameterString"
}

# Call the build script provided by Arcade
& $PSScriptRoot/common/build.ps1 @PSBoundParameters

# Forward exit code of the parent script
exit $LastExitCode
