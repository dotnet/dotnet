# NuGet Apex test instructions

## Select the Visual Studio instance

Use the Visual Studio instance from the intended channel. Do not assume the latest instance returned
by `vswhere` is correct; internal channels may be omitted. Read the installed instance metadata:

```powershell
Get-ChildItem 'C:\ProgramData\Microsoft\VisualStudio\Packages\_Instances' -Directory |
    ForEach-Object {
        $state = Join-Path $_.FullName 'state.json'
        if (Test-Path $state) {
            $instance = Get-Content $state -Raw | ConvertFrom-Json
            [pscustomobject]@{
                InstanceId = $_.Name
                Path = $instance.installationPath
                ChannelId = $instance.channelId
                Version = $instance.installationVersion
            }
        }
    }
```

For IntCanary, select the instance whose channel is `VisualStudio.18.IntPreview`. Use that
instance's `Common7\IDE` directory in the commands below.

## Prepare the experimental instance

Do not reset the experimental instance before routine test runs. Resetting is a troubleshooting step
for failures that indicate a corrupted or stale experimental profile, such as MEF composition errors
or NuGet assembly load errors. Try it when rebuilding, redeploying, and refreshing the extension
configuration do not resolve those failures.

When a reset is necessary, use the selected Visual Studio instance's qualified version name. Using
only the major version, such as `/VSInstance=18.0`, can reset a different channel's profile when
multiple Visual Studio 18 channels are installed:

```powershell
$instanceId = '<instance ID from state.json>'
$visualStudioDirectory = '<installation path from state.json>'

& "$visualStudioDirectory\VSSDK\VisualStudioIntegration\Tools\Bin\CreateExpInstance.exe" `
    /Reset /VSInstance="18.0_$instanceId" /RootSuffix=Exp
```

Build the project references without deployment first. Then build and deploy only the top-level
VSIX project. `DeployExtension=true` must not propagate to project references because the referenced
VSSDK projects do not create standalone VSIX containers:

```powershell
$msbuild = "$visualStudioDirectory\MSBuild\Current\Bin\amd64\MSBuild.exe"
$project = '.\src\NuGet.Clients\NuGet.VisualStudio.Client\NuGet.VisualStudio.Client.csproj'

& $msbuild $project /t:Build /restore /p:Configuration=Debug

& $msbuild $project '/t:Build;DeployVsixExtensionFiles' `
    /p:Configuration=Debug `
    /p:BuildProjectReferences=false `
    /p:DeployExtension=true `
    /p:DeployTargetInstanceId=$instanceId `
    /p:VSSDKTargetPlatformRegRootSuffix=Exp

& "$visualStudioDirectory\Common7\IDE\devenv.exe" `
    /RootSuffix Exp /UpdateConfiguration /Log
```

The explicit `DeployVsixExtensionFiles` target is required because a command-line `Build` only
creates the VSIX. The `/UpdateConfiguration` step makes Visual Studio select the deployed extension
instead of loading both it and the built-in NuGet product extension. If that step is omitted,
`ActivityLog.xml` can report invalid casts between NuGet types loaded from LocalAppData and
`CommonExtensions\Microsoft\NuGet`.

Do not install the generated VSIX into the normal Visual Studio instance. NuGet is a system
component, and removing that installation can require repairing Visual Studio.

Before testing, verify the selected instance's `Exp` profile contains a `NuGet.Console.dll` whose
version matches the Apex test output:

```powershell
Get-ChildItem "$env:LOCALAPPDATA\Microsoft\VisualStudio\18.0_${instanceId}Exp\Extensions" `
    -Recurse -Filter NuGet.Console.dll |
    ForEach-Object { $_.FullName; $_.VersionInfo.FileVersion }
```

## Run tests against the experimental instance

Use the same environment as the selected instance's Developer PowerShell. In particular, set
`VSAPPIDDIR` and leave the Apex-specific variables unset. `VisualStudioOperationsFixture` then sets
both the installation path and `RootSuffix = Exp`; setting
`VisualStudio.InstallationUnderTest.Path` directly bypasses that setup and can launch the normal
hive.

```powershell
$ideDirectory = 'C:\Program Files\Microsoft Visual Studio\18\IntPreview\Common7\IDE\'

Remove-Item 'Env:VisualStudio.InstallationUnderTest.Path' -ErrorAction SilentlyContinue
Remove-Item 'Env:VisualStudio.InstallationUnderTest.RootSuffix' -ErrorAction SilentlyContinue
Remove-Item 'Env:DevEnvDir' -ErrorAction SilentlyContinue
$env:VSAPPIDDIR = $ideDirectory

.\cli\dotnet.exe test `
    .\test\NuGet.Tests.Apex\NuGet.Tests.Apex\bin\Debug\NuGet.Tests.Apex.dll `
    --filter 'FullyQualifiedName~NuGet.Tests.Apex.NuGetConsoleTestCase.MyTestCase'
```

Run one known-good Apex test first to validate the local deployment:

```powershell
--filter 'FullyQualifiedName~NuGet.Tests.Apex.NuGetConsoleTestCase.InstallPackageFromPMCWithInvalidAbsoluteLocalSource_Fails'
```

An immediate `InvalidCastException` mentioning two versions of
`NuGetConsole.Implementation.PowerConsoleToolWindow` means the test launched the wrong hive or the
experimental instance does not contain NuGet binaries from the same build.
