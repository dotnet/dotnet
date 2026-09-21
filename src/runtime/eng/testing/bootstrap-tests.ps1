[CmdletBinding()]
param(
  [string]$DotNet = 'dotnet',
  [string]$RepositoryRoot = (Join-Path $PSScriptRoot '..\..')
)

$ErrorActionPreference = 'Stop'
$repoRoot = [IO.Path]::GetFullPath($RepositoryRoot)
$buildProject = Join-Path $repoRoot 'Build.proj'
$testCount = 0

function Assert-True([bool]$Condition, [string]$Message) {
  if (-not $Condition) {
    throw $Message
  }
  $script:testCount++
}

function Invoke-Evaluation([string]$Project, [string[]]$Properties, [string[]]$Options = @()) {
  $arguments = @('msbuild', $Project, '-nologo', '-p:Configuration=Release', '-p:TargetArchitecture=x64')
  $arguments += $Properties | ForEach-Object { "-p:$_" }
  $output = & $DotNet @arguments @Options
  if ($LASTEXITCODE -ne 0) {
    throw "MSBuild failed for $Project ($LASTEXITCODE): $($output -join [Environment]::NewLine)"
  }
  return ($output -join [Environment]::NewLine) | ConvertFrom-Json
}

function Get-Graph([string[]]$Properties, [string]$Project = $buildProject) {
  Invoke-Evaluation $Project $Properties @(
    '-getProperty:EnableBootstrap,SwapNativeForIL,BootstrapReadyFile,BootstrapLayoutDir,UseNativeAotForComponents'
    '-getItem:ProjectReference,BootstrapConsumer,BootstrapPackageProject,BootstrapCdacPackageProject,BootstrapTestProject'
  )
}

$normal = Get-Graph @('Subset=clr+libs+host', 'EnableBootstrap=false')
Assert-True (@($normal.Items.ProjectReference | Where-Object { $_.Identity -like '*ILCompiler_publish.csproj' }).Count -eq 1) 'Ordinary builds must still publish ILC.'
Assert-True (@($normal.Items.ProjectReference | Where-Object { [IO.Path]::GetFileName($_.Identity) -like 'bootstrap*.proj' }).Count -eq 0) 'Ordinary builds must not add bootstrap traversal projects.'
$normalProjects = @($normal.Items.ProjectReference | ForEach-Object { [IO.Path]::GetFileName($_.Identity) })
$hostCompilerIndex = [Array]::IndexOf($normalProjects, 'ILCompiler_inbuild.csproj')
Assert-True ($hostCompilerIndex -ge 0 -and $hostCompilerIndex -lt [Array]::IndexOf($normalProjects, 'ILCompiler_publish.csproj')) 'The host compiler must precede publishing for explicit --use-bootstrap builds.'

$integrated = Get-Graph @('Subset=clr+libs+host', 'EnableBootstrap=true')
Assert-True (@($integrated.Items.ProjectReference).Count -eq 2) 'Integrated builds must have two post-producer traversals.'
Assert-True (@($integrated.Items.BootstrapConsumer).Count -eq 3) 'The clr.tools subset must defer ILC, Crossgen2, and ILAsm.'
Assert-True (@($integrated.Items.BootstrapPackageProject).Count -eq 0) 'A build without packs must not add bootstrap packages.'
Assert-True ($integrated.Properties.BootstrapLayoutDir -eq [IO.Path]::Combine($repoRoot, 'artifacts', 'bootstrap', 'win-x64') + [IO.Path]::DirectorySeparatorChar -or
             $integrated.Properties.BootstrapLayoutDir -match '[/\\]artifacts[/\\]bootstrap[/\\][^/\\]+[/\\]$') 'The integrated snapshot must use the canonical bootstrap layout.'

$consumerLane = Get-Graph @('Subset=clr+libs+host', 'EnableBootstrap=true', 'BootstrapBuildLane=true') (Join-Path $repoRoot 'eng\bootstrap.proj')
$sharedProducer = @($consumerLane.Items.ProjectReference | Where-Object { [IO.Path]::GetFileName($_.Identity) -eq 'bootstrap-producers.proj' })
Assert-True ($sharedProducer[0].UndefineProperties -match 'BootstrapBuildLane' -and
             $sharedProducer[0].GlobalPropertiesToRemove -match 'BootstrapBuildLane') 'Both traversal build and static-graph restore must remove lane-specific producer properties.'

$narrow = Get-Graph @('Subset=host.native', 'EnableBootstrap=true')
Assert-True (@($narrow.Items.BootstrapConsumer).Count -eq 0) 'A host-only subset must not publish SDK tools.'
Assert-True (@($narrow.Items.BootstrapPackageProject).Count -eq 0) 'A host-only subset must not build SDK packages.'

$libraryTests = Get-Graph @('Subset=libs.tests', 'EnableBootstrap=true')
Assert-True (@($libraryTests.Items.BootstrapTestProject).Count -eq 1) 'Library test restore must be deferred until the bootstrap snapshot exists.'
$ordinaryTests = Get-Graph @('Subset=libs.tests', 'EnableBootstrap=false')
Assert-True (@($ordinaryTests.Items.ProjectReference | Where-Object { [IO.Path]::GetFileName($_.Identity) -eq 'tests.proj' }).Count -eq 1) 'Ordinary library-test builds must retain their traversal.'

$producerProject = Join-Path $repoRoot 'eng\bootstrap-producers.proj'
$producers = Get-Graph @('Subset=host.native', 'EnableBootstrap=false', 'BootstrapMainBuild=true') $producerProject
foreach ($name in @('sfx.proj', 'pretest.proj', 'corehost.proj', 'Microsoft.NETCore.Platforms.csproj')) {
  Assert-True (@($producers.Items.ProjectReference | Where-Object { [IO.Path]::GetFileName($_.Identity) -eq $name }).Count -eq 1) "Missing or duplicated bootstrap producer: $name"
}
if ($producers.Properties.UseNativeAotForComponents -eq 'true') {
  Assert-True (@($producers.Items.ProjectReference | Where-Object { $_.Identity -like '*ILCompiler_inbuild.csproj' }).Count -eq 1) 'NativeAOT consumers need the host compiler before the snapshot boundary.'
}

$singleFile = Get-Graph @('Subset=clr.tools', 'EnableBootstrap=false', 'BootstrapMainBuild=true', 'UseNativeAotForComponents=false') $producerProject
Assert-True (@($singleFile.Items.ProjectReference | Where-Object { $_.Identity -like '*System.Private.CoreLib.csproj' -and $_.Identity -notlike '*nativeaot*' }).Count -eq 1) 'Single-file bootstrap requires CoreCLR CoreLib.'
Assert-True (@($singleFile.Items.ProjectReference | Where-Object { $_.Identity -like '*runtime.proj' }).Count -ge 1) 'Single-file bootstrap requires the CoreCLR runtime.'

$cross = Get-Graph @('Subset=clr+libs+host', 'EnableBootstrap=false', 'BootstrapMainBuild=true', 'HostOS=linux', 'BuildArchitecture=x64', 'TargetOS=freebsd', 'TargetArchitecture=arm64', 'CrossBuild=true', 'UseNativeAotForComponents=false') $producerProject
Assert-True (@($cross.Items.ProjectReference | Where-Object { $_.Identity -like '*runtime.proj' -and $_.AdditionalProperties -match 'HostArchitecture=x64' -and $_.AdditionalProperties -match 'HostCrossOS=linux' }).Count -eq 1) 'Cross-OS/architecture bootstrap must retain its host JIT producer.'

$legacy = Get-Graph @('Subset=bootstrap', 'EnableBootstrap=false')
Assert-True ($legacy.Properties.SwapNativeForIL -eq 'true') 'Explicit bootstrap must retain IL-swapped pack composition.'
Assert-True (-not [string]::IsNullOrEmpty($legacy.Properties.BootstrapReadyFile)) 'Explicit bootstrap must create a layout.'
Assert-True (@($legacy.Items.BootstrapConsumer).Count -eq 0) 'Explicit bootstrap must not publish shipping consumers.'

$packs = Get-Graph @('Subset=clr+libs+tools+host+packs', 'EnableBootstrap=true', 'BuildHostTools=true')
Assert-True (@($packs.Items.BootstrapPackageProject).Count -ge 2) 'Requested tool packages must be deferred, not dropped.'
Assert-True (@($packs.Items.BootstrapCdacPackageProject).Count -eq 1) 'Requested cDAC packaging must remain after the traversal join.'

foreach ($hostPackProperty in @('BuildHostTools=true', 'BuildCrossgen2HostPackForWorkloadTesting=true')) {
  $browser = Get-Graph @('Subset=packs', 'EnableBootstrap=false', 'TargetOS=browser', 'TargetArchitecture=wasm', 'RuntimeFlavor=CoreCLR', $hostPackProperty)
  Assert-True (@($browser.Items.ProjectReference | Where-Object { $_.Identity -like '*Microsoft.NETCore.App.Crossgen2.Host.sfxproj' }).Count -eq 1) 'Browser host-tool packaging must retain both opt-in paths.'
}

$crossgenProject = Join-Path $repoRoot 'src\installer\pkg\sfx\Microsoft.NETCore.App\Microsoft.NETCore.App.Crossgen2.sfxproj'
$crossgen = Invoke-Evaluation $crossgenProject @('EnableBootstrap=false') @('-getItem:ProjectReference', '-getProperty:ArtifactsDir')
$publishReference = @($crossgen.Items.ProjectReference | Where-Object { $_.Identity -like '*crossgen2_publish.csproj' })
Assert-True ($publishReference.Count -eq 1) 'Crossgen2 packaging must retain its publish reference.'
Assert-True ($publishReference[0].AdditionalProperties -notmatch '(^|;)ArtifactsDir=') 'Ordinary packaging must not override the artifacts directory.'

$fixtureRoot = Join-Path ([IO.Path]::GetTempPath()) ('runtime-bootstrap-tests-' + [Guid]::NewGuid().ToString('N'))
try {
  $inputRoot = Join-Path $fixtureRoot 'inputs'
  foreach ($directory in @('aotsdk', 'host', 'ref', 'runtime\runtimes\test-rid\native', 'bin\Microsoft.NETCore.Platforms')) {
    New-Item -ItemType Directory -Path (Join-Path $inputRoot $directory) -Force | Out-Null
  }
  $sources = @{
    'aotsdk\sdk.obj' = 'sdk'
    'host\apphost' = 'host'
    'ref\reference.dll' = 'reference'
    'runtime\runtimes\test-rid\native\runtime.dll' = 'runtime'
    'bin\Microsoft.NETCore.Platforms\runtime.json' = '{}'
  }
  foreach ($source in $sources.GetEnumerator()) {
    Set-Content -LiteralPath (Join-Path $inputRoot $source.Key) -Value $source.Value
  }
  $layout = Join-Path $fixtureRoot 'layout'
  $escapedRoot = [Security.SecurityElement]::Escape($fixtureRoot)
  $targets = [Security.SecurityElement]::Escape((Join-Path $repoRoot 'eng\bootstrap-layout.targets'))
  $fixtureProject = Join-Path $fixtureRoot 'layout.proj'
  @"
<Project>
  <PropertyGroup>
    <ArtifactsBinDir>$escapedRoot/inputs/bin/</ArtifactsBinDir>
    <ArtifactsObjDir>$escapedRoot/obj/</ArtifactsObjDir>
    <BootstrapLayoutDir>$escapedRoot/layout/</BootstrapLayoutDir>
    <BootstrapRefPackDir>$escapedRoot/layout/ref</BootstrapRefPackDir>
    <BootstrapRuntimePackDir>$escapedRoot/layout/runtime</BootstrapRuntimePackDir>
    <BootstrapAotSdkDir>$escapedRoot/layout/aotsdk</BootstrapAotSdkDir>
    <BootstrapHostDir>$escapedRoot/layout/host</BootstrapHostDir>
    <BootstrapRidGraphDir>$escapedRoot/layout/ridgraph</BootstrapRidGraphDir>
    <CoreCLRAotSdkDir>$escapedRoot/inputs/aotsdk</CoreCLRAotSdkDir>
    <DotNetHostBinDir>$escapedRoot/inputs/host/</DotNetHostBinDir>
    <MicrosoftNetCoreAppRefPackDir>$escapedRoot/inputs/ref</MicrosoftNetCoreAppRefPackDir>
    <MicrosoftNetCoreAppRuntimePackDir>$escapedRoot/inputs/runtime</MicrosoftNetCoreAppRuntimePackDir>
    <UseNativeAotForComponents>true</UseNativeAotForComponents>
    <DotNetBuildSourceOnly>true</DotNetBuildSourceOnly>
    <TargetRid>test-rid</TargetRid>
    <PortableTargetRid>portable-rid</PortableTargetRid>
  </PropertyGroup>
  <Import Project="$targets" />
</Project>
"@ | Set-Content -LiteralPath $fixtureProject

  function Invoke-Layout {
    Invoke-Evaluation $fixtureProject @() @('-t:CreateBootstrapLayout', '-getProperty:BootstrapReadyFile,BootstrapLayoutDir') | Out-Null
  }

  Invoke-Layout
  $stamp = Join-Path $layout '.complete'
  Assert-True (Test-Path -LiteralPath $stamp) 'A successful snapshot must create its readiness stamp.'
  Assert-True (Test-Path -LiteralPath (Join-Path $layout 'runtime\runtimes\portable-rid\native\runtime.dll')) 'Distro-specific runtime packs must have a portable RID alias.'
  $stampTime = (Get-Item -LiteralPath $stamp).LastWriteTimeUtc
  Invoke-Layout
  Assert-True ((Get-Item -LiteralPath $stamp).LastWriteTimeUtc -eq $stampTime) 'An unchanged snapshot must not be rewritten.'

  $reference = Join-Path $inputRoot 'ref\reference.dll'
  $sourceTime = (Get-Item -LiteralPath $reference).LastWriteTimeUtc
  Set-Content -LiteralPath $reference -Value 'changed'
  (Get-Item -LiteralPath $reference).LastWriteTimeUtc = $sourceTime
  Invoke-Layout
  Assert-True ((Get-Content -LiteralPath (Join-Path $layout 'ref\reference.dll') -Raw).Trim() -eq 'changed') 'Content changes with preserved timestamps must invalidate the snapshot.'

  $extraSource = Join-Path $inputRoot 'ref\extra.dll'
  Set-Content -LiteralPath $extraSource -Value 'extra'
  Invoke-Layout
  Assert-True (Test-Path -LiteralPath (Join-Path $layout 'ref\extra.dll')) 'Added inputs must be copied.'
  Remove-Item -LiteralPath $extraSource
  Invoke-Layout
  Assert-True (-not (Test-Path -LiteralPath (Join-Path $layout 'ref\extra.dll'))) 'Removed inputs must not remain in the snapshot.'

  $debugRef = Join-Path $inputRoot 'debug-ref'
  New-Item -ItemType Directory -Path $debugRef | Out-Null
  Set-Content -LiteralPath (Join-Path $debugRef 'reference.dll') -Value 'debug'
  Invoke-Evaluation $fixtureProject @('Configuration=Debug', "MicrosoftNetCoreAppRefPackDir=$debugRef") @('-t:CreateBootstrapLayout', '-getProperty:BootstrapReadyFile,BootstrapLayoutDir') | Out-Null
  Assert-True ((Get-Content -LiteralPath (Join-Path $layout 'ref\reference.dll') -Raw).Trim() -eq 'debug') 'Switching configurations must update the snapshot.'
  Invoke-Layout
  Assert-True ((Get-Content -LiteralPath (Join-Path $layout 'ref\reference.dll') -Raw).Trim() -eq 'changed') 'Switching back to a previous configuration must not reuse the other configuration snapshot.'

  Remove-Item -LiteralPath (Join-Path $layout 'host\apphost')
  Invoke-Layout
  Assert-True (Test-Path -LiteralPath (Join-Path $layout 'host\apphost')) 'Missing snapshot files must be restored.'

  Move-Item -LiteralPath (Join-Path $inputRoot 'runtime\runtimes\test-rid') -Destination (Join-Path $inputRoot 'runtime\runtimes\portable-rid')
  Invoke-Layout
  Assert-True (Test-Path -LiteralPath (Join-Path $layout 'runtime\runtimes\test-rid\native\runtime.dll')) 'Portable runtime packs must also have the target RID alias.'

  $platformSources = Join-Path $inputRoot 'Microsoft.NETCore.Platforms\src'
  New-Item -ItemType Directory -Path $platformSources -Force | Out-Null
  Set-Content -LiteralPath (Join-Path $platformSources 'runtime.json') -Value '{"frozen":true}'
  Set-Content -LiteralPath (Join-Path $platformSources 'PortableRuntimeIdentifierGraph.json') -Value '{"portable":true}'
  Invoke-Evaluation $fixtureProject @('DotNetBuildSourceOnly=false', "LibrariesProjectRoot=$inputRoot$([IO.Path]::DirectorySeparatorChar)") @('-t:CreateBootstrapLayout', '-getProperty:BootstrapReadyFile,BootstrapLayoutDir') | Out-Null
  Assert-True ((Get-Content -LiteralPath (Join-Path $layout 'ridgraph\runtime.json') -Raw).Trim() -eq '{"frozen":true}') 'Portable builds must use checked-in RID graphs rather than stale generated graphs.'
  Assert-True (Test-Path -LiteralPath (Join-Path $layout 'ridgraph\PortableRuntimeIdentifierGraph.json')) 'Portable builds must snapshot both checked-in RID graphs.'
} finally {
  if (Test-Path -LiteralPath $fixtureRoot) {
    Remove-Item -LiteralPath $fixtureRoot -Recurse -Force
  }
}

Write-Host "Passed $testCount bootstrap graph and layout assertions."
