# XHarness support in Microsoft.DotNet.Helix.Sdk

> Note: This document presumes you are familiar with the usage of Microsoft.DotNet.Helix.Sdk. If not, please [start here](https://github.com/dotnet/arcade/blob/master/src/Microsoft.DotNet.Helix/Sdk/Readme.md).

The Helix SDK has extended support for execution of **Android/iOS/tvOS/WASM workloads** where you only need to point the SDK to:
  - Android .apks,
  - iOS/tvOS .app bundles,
  - WASM-ready test DLLs

and the SDK will create a Helix job with the specified payload and send it to Helix where it will be run using a tool called [XHarness](https://github.com/dotnet/xharness). A suitable test target will be found - an emulator, a real device or a specified JS engine for WASM scenarios - the app will be installed, run and logs extracted and uploaded with other job results.

**Please note that we require all jobs targeting Android and Apple platforms to use Helix SDK as described below. The SDK makes sure the environment stays clean and the jobs run more reliably. Using the SDK will for instance re-run your work item on a different machine when we detect infrastructure issues such as problematic mobile device. Furthermore, it collects additional telemtry that helps us maintain these platforms in Helix.**

XHarness is a [.NET tool](https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools) and requires .NET 6 ASP.NET runtime to execute on the Helix agent.
The SDK will automatically pre-install the needed runtime and the XHarness tool and you can then call XHarness directly.

Furthermore, in case your application contains the [XHarness TestRunner](https://github.com/dotnet/xharness#test-runners), Helix SDK can handle everything for you - you just need to point it to the application.
XHarness will be called automatically and test results will be published to Azure DevOps.

## Table of contents

- [How to use](#how-to-use)
  - [iOS/tvOS .app bundle payloads](#iostvos-app-bundle-payloads)
    - [Targeting real iOS/tvOS devices](#targeting-real-iostvos-devices)
  - [Android .apk payloads](#android-apk-payloads)
  - [WASM payloads](#wasm-payloads)
  - [Calling the XHarness tool directly via custom commands](#calling-the-xharness-tool-directly-via-custom-commands)
    - [Variables defined for Apple scenarios](#variables-defined-for-apple-scenarios)
    - [Variables defined for Android scenarios](#variables-defined-for-android-scenarios)
  - [Reusing app bundles / apks](#reusing-app-bundles--apks)
  - [Supplying arbitrary .zip archive](#supplying-arbitrary-zip-archive)
  - [Detecting infrastructural issues](#detecting-infrastructural-issues)

## How to use

There are few ways how to run XHarness using the Helix SDK:
- Specifying the apks/app bundles using the `XHarnessApkToTest` and `XHarnessAppBundleToTest` items as described below; rest (running the app and collecting results) will be taken care of from there.
  You no longer specify the `HelixCommand` to be executed even though you can specify your own custom commands to be executed.
  Each apk/app bundle will be processed as a separate Helix work item.
- Specifying the `XHarnessAndroidProject` or `XHarnessAppleProject` task items which will point to projects that produce apks/app bundles from their `Build` target.
  - Examples - [iOS](https://github.com/dotnet/arcade/blob/master/tests/XHarness/XHarness.TestAppBundle.proj) and [Android](https://github.com/dotnet/arcade/blob/master/tests/XHarness/XHarness.TestApk.proj)
- Specifying the apks/app bundles using the `XHarnessApkToTest` and `XHarnessAppBundleToTest` and providing custom commands to be executed via the `CustomCommands` property [(see below)](#calling-the-xharness-tool-directly-via-custom-commands).
- Specifying the `XHarnessAndroidProject` or `XHarnessAppleProject` task items by pointing them to a zip archive [(see details below)](#supplying-arbitrary-zip-archive) including any number of apps (or no apps at all if these are for example built as part of the job) and providing custom commands to be executed via the `CustomCommands` property [(see below)](#calling-the-xharness-tool-directly-via-custom-commands).

There are some required configuration properties that need to be set for the SDK to enable XHarness. You can also provide some optional properties to customize the run further:

```xml
<PropertyGroup>
  <!-- Required: Makes sure XHarness is pre-installed on the Helix agent before the job starts - this effectively marks the job as one using XHarness -->
  <IncludeXHarnessCli>true</IncludeXHarnessCli>

  <!-- Required: Version of XHarness CLI to use. Check the NuGet feed for current version: https://dev.azure.com/dnceng/public/_packaging?_a=package&feed=dotnet-eng&package=Microsoft.DotNet.XHarness.CLI&protocolType=NuGet -->
  <MicrosoftDotNetXHarnessCLIVersion>1.0.0-prerelease.21511.3</MicrosoftDotNetXHarnessCLIVersion>

  <!-- Optional: Properties that are also valid for regular jobs created by Helix SDK (some might be needed for CI runs only) -->
  <HelixType>test/product/</HelixType>
  <HelixBaseUri>https://helix.int-dot.net</HelixBaseUri>
  <Creator>$(BUILD_SOURCEVERSIONAUTHOR)</Creator>
  <EnableAzurePipelinesReporter>true</EnableAzurePipelinesReporter>
</PropertyGroup>

<!-- Required: Configuration that is already needed for any job created by Helix SDK -->
<ItemGroup>
  <HelixTargetQueue Include="osx.1015.amd64.open"/>
</ItemGroup>
```

### iOS/tvOS .app bundle payloads

To execute .app bundles, declare one or more `XHarnessAppBundleToTest` items:

```xml
<ItemGroup>
  <!-- Example that finds all directories named *.app -->
  <XHarnessAppBundleToTest Include="$([System.IO.Directory]::GetDirectories('$(TestArchiveTestsRoot)', '*.app', System.IO.SearchOption.AllDirectories))">
    <!-- Specify target platform () -->
    <TestTarget>ios-simulator-64_13.5</TestTarget>
  </XHarnessAppBundleToTest>
</ItemGroup>
```

The `<TestTarget>` metadata is a required configuration that tells XHarness which kind of device/Simulator to target.
You can omit the iOS version too by specifying ios-simulator-64 only.
For more information, use the `help apple test` command of the XHarness CLI, see the `--target` option.

You can also specify some additional metadata that will help you configure the run better:

```xml
<ItemGroup>
  <XHarnessAppBundleToTest Include=".\appbundles\Contoso.Example.Tests.app">
    <!-- Optional: Timeout for the overall run of the whole Helix work item (including Simulator booting, app installation..) -->
    <WorkItemTimeout>00:20:00</WorkItemTimeout>

    <!-- Optional: Timeout for the actual test run (when TestRunner starts execution of tests) -->
    <!-- Should be smaller than WorkItemTimeout by several minutes -->
    <TestTimeout>00:12:00</TestTimeout>

    <!-- Optional: Timeout for how long it takes to install and boot the app and start running the first test -->
    <LaunchTimeout>00:10:00</LaunchTimeout>
    
    <!-- Optional: Apps that don't contain the TestRunner can be run using the `apple run` command instead of `apple test` -->
    <!-- Default is true -->
    <IncludesTestRunner>false</IncludesTestRunner>

    <!-- Optional (`apple run` command only): Expected exit code of the iOS/tvOS application. XHarness exits with 0 when the app exits with this code -->
    <!-- Please note that exit code detection may not be reliable across iOS/tvOS versions -->
    <ExpectedExitCode>3</ExpectedExitCode>

    <!-- Optional: Before and after the run, erases all simulator data and resets it for a clean state -->
    <!-- Default is false -->
    <ResetSimulator>true</ResetSimulator>
  </XHarnessAppBundleToTest>
</ItemGroup>
```

You can configure the execution further via MSBuild properties:

```xml
<PropertyGroup>
  <!-- Optional: Specific version of Xcode to use. If omitted, xcode-select is used to determine the version -->
  <XHarnessXcodeVersion>11.4</XHarnessXcodeVersion>
</PropertyGroup>
```

#### Targeting real iOS/tvOS devices

When you are not using `CustomCommands` and you point the SDK to an app bundle, signing will be taken care of for you and you can skip this section.

Otherwise, to deploy an app bundle to a real device, the app bundle needs to be signed before it is installed.
To sign an app bundle you need to make sure that:

1. The bundle contains a *provisioning profile*. This is a file called **embedded.mobileprovision** that needs to be in the root of the app bundle folder (`[PATH TO BUNDLE]/embedded.mobileprovision`).
2. The `sign [PATH TO BUNDLE]` bash command is called. This method is available for you when using `CustomCommands`.

The provisioning profile will be injected into your Helix payload as part of the job preparation and you need to copy it into the app bundle's root:
- If you point the SDK to an app bundle directory, the profile will be injected in that folder automatically (in this case you don't control the payload creation).
- If you give the SDK a custom already zipped payload (you control the payload more), the profile is:
  - Either placed into any `.app` ending folder from the root of the zip archive (assumed app bundle),
  - or placed in the root of the zip archive otherwise.

> Take away: If you provide your own zipped payload that doesn't contain an `.app` ending directory, make sure you copy `embedded.mobileprovision` from `$HELIX_WORKITEM_ROOT` into your app bundle.

Couple more notes:
- Only the basic set of app permissions is supported at the moment (e.g. no GPS control...)
- We cannot re-sign an app that was already signed with a different set of permissions
- App bundle identifier has to start with `net.dot.` since we only support those application IDs at the moment (restrictions by Apple)
- When signing succeeds, you should see something like this in the logs:  
    `/tmp/helix/working/A74A0921/w/A5C2095E/e/System.Buffers.Tests.app: signed app bundle with Mach-O thin (arm64) [net.dot.System.Buffers.Tests]`

### Android .apk payloads

To execute .apks, declare one or more `XHarnessApkToTest` items:

```xml
<ItemGroup>
  <XHarnessApkToTest Include="$(TestArchiveTestsRoot)apk\x64\System.Numerics.Vectors.Tests.apk">
    <!-- Package name: this comes from metadata inside the apk itself -->
    <AndroidPackageName>net.dot.System.Numerics.Vectors.Tests</AndroidPackageName>

    <!-- If there are > 1 instrumentation classes inside the package, we need to know the name of which to use -->
    <AndroidInstrumentationName>net.dot.MonoRunner</AndroidInstrumentationName>
  </XHarnessApkToTest>
</ItemGroup>
```

You can also specify some additional metadata that will help you configure the run better:

```xml
<ItemGroup>
  <XHarnessApkToTest Include="$(TestArchiveTestsRoot)**\*.apk">
    <!-- Optional: Timeout for the overall run of the whole Helix work item (including Simulator booting, app installation..) -->
    <WorkItemTimeout>00:20:00</WorkItemTimeout>

    <!-- Optional: Timeout for the actual test run (when TestRunner starts execution of tests) -->
    <!-- Should be smaller than WorkItemTimeout by several minutes -->
    <TestTimeout>00:12:00</TestTimeout>
  
    <!-- Optional: Expected exit code of the instrumentation run. XHarness exits with 0 when the app exits with this code -->
    <ExpectedExitCode>3</ExpectedExitCode>
  </XHarnessApkToTest>
</ItemGroup>
```

### WASM payloads

We currently do not support execution of WASM workloads directly, please call the `xharness wasm *` commands manually via `CustomCommands`.

### Calling the XHarness tool directly via custom commands

In case you want to run your own custom set of commands, you can specify the `CustomCommands` property.
However, be mindful that you need to perform a clean up (read "uninstall the apps").
The SDK will try to clean up the device/simulator state at the end of the job too but it is better to be handled by user's payload too.

Example:

```xml
<ItemGroup>
  <XHarnessAppBundleToTest Include="path\to\Some.iOS.app">
    <TestTarget>ios-simulator-64</TestTarget>
    <WorkItemTimeout>00:12:00</WorkItemTimeout>
    <CustomCommands>
      <![CDATA[
      set -e
      deviceId=`xharness apple device $target`
      xharness apple install -t $target --device "$deviceId" -o "$output_directory" --app=$app
      set +e
      result=0
      xharness apple just-test -t $target --device "$deviceId" -o "$output_directory" --app net.dot.Some.iOS --timeout 00:08:00
      ((result|=$?))
      xharness apple uninstall -t $target --device "$deviceId" -o "$output_directory" --app net.dot.Some.iOS
      ((result|=$?))
      exit $result
      ]]>
    </CustomCommands>
  </XHarnessAppBundleToTest>
</ItemGroup>
```

Please note that Android can run on both Windows and Linux based on the target queue.
For that reason, make sure the `<CustomCommands>` script you supply is either **bash** for Linux queues or **PowerShell** for Windows.

When using `CustomCommands`, several variables will be defined for you for easier run.

#### Variables defined for Apple scenarios
- `$app` - path to the application
- `$output_directory` - path under which all files will be uploaded to Helix at the end of the job
  - If a file named `testResults.xml` is found containing xUnit results, it will be uploaded back to Azure DevOps
- `$target`, `$timeout`, `$launch_timeout`, `$expected_exit_code`, `$includes_test_runner` - parsed metadata defined on the original `XHarnessAppBundleToTest` MSBuild item

#### Variables defined for Android scenarios
- `$app` - path to the application
- `$package_name` - name of the Android package
- `$output_directory` - path under which all files will be uploaded to Helix at the end of the job
  - If a file named `testResults.xml` is found containing xUnit results, it will be uploaded back to Azure DevOps
- `$timeout`, `$expected_exit_code`, `$device_output_path`, `$instrumentation` - parsed metadata defined on the original `XHarnessApkToTest` MSBuild item

### Reusing app bundles / apks

In some scenarios, you might need to re-use one application for multiple work items, i.e. to supply each with a different custom command to run the application with different parameters or to run the application on different test targets (e.g. different versions of iOS).

You can then name the item however you like and supply the path to the app as metadata:

```xml
<XHarnessApkToTest Include="System.Text.Json"> <!-- Include can be any string -->
  <ApkPath>path/to/System.Text.Json.Tests.apk</ApkPath> <!-- Set the path in here -->
  <AndroidPackageName>net.dot.System.Buffers.Tests</AndroidPackageName>
  <AndroidInstrumentationName>net.dot.MonoRunner</AndroidInstrumentationName>
</XHarnessApkToTest>

<XHarnessApkToTest Include="System.Text.Json-with-custom-commands"> <!-- Include should differ for different work items -->
  <ApkPath>path/to/System.Text.Json.Tests.apk</ApkPath> <!-- The path stays the same -->
  <AndroidPackageName>net.dot.System.Buffers.Tests</AndroidPackageName>
  <AndroidInstrumentationName>net.dot.MonoRunner</AndroidInstrumentationName>
  <CustomCommands>
    <![CDATA[
    xharness android test --app "$app" --package-name "net.dot.System.Buffers.Tests" --output-directory "$output_directory" --instrumentation=net.dot.MonoRunner
    ]]>
  </CustomCommands>
</XHarnessApkToTest>
```

For Apple it is the same, just the metadata property name is `<AppBundlePath>`.

### Supplying arbitrary .zip archive

In some scenarios, you might not have the app/apk available because you will build it in Helix. Alternatively, you might need to send multiple apps and run XHarness commands over them.
In these cases, you can point the SDK to a .zip archive with your payloads.
The SDK will add some scripts needed for clean execution inside of this .zip archive and send it to Helix.
The .zip archive will be extracted for you in the working directory.

**Note that in case you supply a .zip you also need to supply the `CustomCommands` property since the SDK won't know the specifics of the .zip contents.**

Example:

```xml
<ItemGroup>
  <XHarnessAppBundleToTest Include="path\to\an-archive.zip">
    <TestTarget>ios-device_14.4</TestTarget>
    <WorkItemTimeout>00:12:00</WorkItemTimeout>
    <CustomCommands>
      <![CDATA[
      # Sign applications since we are targeting real devices in this example
      sign first.app
      sign second.app
      result=0
      xharness apple test --target $target --output-directory "$output_directory" -app first.app
      ((result|=$?))
      xharness apple test --target $target --output-directory "$output_directory" -app second.app
      ((result|=$?))
      exit $result
      ]]>
    </CustomCommands>
  </XHarnessAppBundleToTest>
</ItemGroup>
```

### Detecting infrastructural issues

The mobile platforms can sometimes be unreliable and the devices and emulators can get into bad states which can fail the job.
Examples can be:
- Device rebooted and has not started properly
- Device is locked
- Device memory is full and app cannot be installed
- Android emulator is not started
- Apple Simulator is freezing up (caused by Apple's CPU/RAM leaks)

The SDK can detect most of these problems and will try to run your work on a different Helix agent with a different device.
This usually resolves the issue transparently for the end user.

However, when supplying own commands via the [`CustomCommand` property](#calling-the-xharness-tool-directly-via-custom-commands), the SDK doesn't have visibility into the job and in those cases, it is up to the user to handle some of the issues.
Usually the issues can be recognized from the [exit code returned by XHarness](https://github.com/dotnet/xharness/blob/main/src/Microsoft.DotNet.XHarness.Common/CLI/ExitCode.cs), e.g.:
- 78 - `PACKAGE_INSTALLATION_FAILURE`
- 81 - `DEVICE_NOT_FOUND`
- 85 - `ADB_DEVICE_ENUMERATION_FAILURE`
- 86 - `PACKAGE_INSTALLATION_TIMEOUT`

In these cases, you can ask for the work item to be processed again (usually by a different Helix agent, but not 100% granted).
You have two options how to achieve this:

- Calling a bash/PowerShell function named `report_infrastructure_failure` which is available in your main script.
  - The function accepts a string parameter - a reason message that will be reported to Helix, e.g. "Failed to install app X.Y (XHarness returned 86)".
- Creating a file in the working directory called `.retry` with the reason message set as its content.
  - You can also create a `.reboot` file which will reboot the machine after the job is over. This is also recommended as it might resolve some of the issues. It will also increase the chance some other Helix agent will pick up the re-tried job.

