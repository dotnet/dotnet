# Contribution Guide

This article will help you build, test and try out local builds of the VS test
platform.

## Prerequisites

Clone the repository to a local directory. Rest of this article assumes
`C:\source\vstest` as the location where you cloned the repo.

```shell
> cd C:\source
> git clone https://github.com/Microsoft/vstest.git
```

### Windows requirements

You can use any editor paired with dotnet SDK to develop VSTest.

If you're planning to use **Visual Studio** as development environment, install `VS 2026` with .NET Desktop development workload.

### Linux / macOS requirements

You can use any editor paired with dotnet SDK to develop VSTest.

## Build

On Windows, run `build.cmd` (or `.\build.cmd` in PowerShell); on Linux/macOS, run `./build.sh` to install the required .NET SDK into the `.dotnet` directory and to build and restore dependencies. After that you can use Visual Studio and build normally.

### Building with Visual Studio

Open `C:\source\vstest\TestPlatform.slnx` in VS.

Use `Build Solution` to build the source code.

Binaries for each assembly are produced in the
`artifacts/src/<Assembly>/bin/Debug` directory.

### Building with CLI

To build the repository, run the following command:

```shell
> cd C:\source\vstest
> build.cmd
```

Or `build.cmd -pack` to also produce nuget packages.


## Test

There are following sets of tests that you would run locally to validate your changes:

* Unit tests - run test.cmd
* Smoke tests - run test.cmd -smokeTest

Additional tests that can be run locally, but typically you would run just the ones related to changes, and rely on PR build to validate the complete change:

* Integration tests - run test.cmd -integrationTest
* Compatibility tests - run test.cmd -compatibilityTest
* Performance tests - run test.cmd -performanceTest

> ⚠️Smoke, Integration, Compatibility and Performance tests do use the build packages that are produced by running `build.cmd -pack`, if you touch the production code (in src, e.g. in vstest.console) you should re-build before running these tests.
> If you however touched just the integration test code, or test assets (in test/TestAssets) you can do `./build.cmd` for faster build (without -pack), or run the tests from IDE directly. The test assets will automatically re-build, but since you did not re-pack, there is no reason to fully clean and restore the dev packages, making it much faster to startup the integration tests.

### Running a specific test

With integration tests you typically want to run all integration tests that affect a particular component, for example when changing blame data collector you want to run all tests for blame. This can be done by providing a parameter `-filter` to the test run, providing part of the test name, or providing a more complete filter using [MSTest filter syntax](https://learn.microsoft.com/en-us/dotnet/core/testing/selective-unit-tests?pivots=mstest)

#### Windows

On Windows, `test.cmd` wraps `eng/Build.ps1` which has a dedicated `-filter` parameter:

```powershell
test.cmd -integrationTest -filter blame
test.cmd -integrationTest -filter "FullyQualifiedName~StackOverflow"
```

#### Linux / macOS

On Linux/macOS, `test.sh` wraps arcade's `eng/common/build.sh` which does **not** have a dedicated `-filter` parameter. Instead, you pass the filter as an MSBuild property. This requires careful quoting to prevent bash from interpreting special characters:

```bash
# Run a specific test by name (use single quotes to protect the value from shell expansion):
./test.sh --integrationTest --property:'TestRunnerAdditionalArguments=--filter "FullyQualifiedName~StackOverflow"'

# Run tests matching a simple keyword:
./test.sh --integrationTest --property:'TestRunnerAdditionalArguments=--filter "blame"'

# Combine with -tl:off to disable terminal logger and see raw output:
./test.sh --integrationTest --property:'TestRunnerAdditionalArguments=--filter "FullyQualifiedName~StackOverflow"' -tl:off
```

> ⚠️ The single quotes around `--property:'...'` are critical — without them, bash will interpret `&`, `|`, spaces, and quotes inside the filter expression. If your filter contains `&` (AND), wrap the whole `--property:` argument in single quotes.

> ⚠️ On non-Windows platforms, tests marked with `TestCategory=Windows` or `TestCategory=Windows-Review` are automatically excluded.

## Using the development packages

The nuget packages produced by `./build.cmd -pack` are stored in `C:\source\vstest\artifacts\packages\<configuration>\Shipping`, and the VSIX in `C:\source\vstest\artifacts\VSSetup\Debug\Insertion\Microsoft.VisualStudio.TestTools.TestPlatform.V2.CLI.vsix`.

You can use those packages in your own test projects to test your changes, by importing them via a local nuget source defined in nuget.config.

- Microsoft.TestPlatform - ships vstest.console.exe to nuget, and is used in CI runs
- Microsoft.VisualStudio.TestTools.TestPlatform.V2.CLI.vsix - ships vstest.console.exe into Visual Studio, is used by Test Explorer, and in CI runs from hosted AzDO images
- Microsoft.TestPlatform.CLI - ships vstest.console.dll to dotnet/sdk and is used by dotnet test
- Microsoft.NET.Test.Sdk - ships targets and brings testhost as dependency

Before using these packages directly, consider writing an Integration test instead, integration tests already do lot of the work for you automatically, and you can debug there easily as well.

## Debugging Integration Tests

Integration tests (in `test/Microsoft.TestPlatform.Acceptance.IntegrationTests`) run vstest.console and testhost as separate processes. You can attach Visual Studio to those processes automatically by using the `DebugInfo` properties on the test data source attributes.

### Using DebugInfo properties on test attributes

The `NetCoreRunnerAttribute` and `NetFrameworkRunnerAttribute` attributes (and other data source attributes like `CompatibilityRowsBuilder`) expose the following boolean properties:

| Property | What it debugs | Environment variable set |
|---|---|---|
| `DebugVSTestConsole = true` | vstest.console (the runner) | `VSTEST_RUNNER_DEBUG_ATTACHVS=1` |
| `DebugTestHost = true` | testhost (the test process) | `VSTEST_HOST_DEBUG_ATTACHVS=1` |
| `DebugDataCollector = true` | data collector process | `VSTEST_DATACOLLECTOR_DEBUG_ATTACHVS=1` |
| `DebugStopAtEntrypoint = true` | keeps entry point breakpoints | (when `false`, sets `VSTEST_DEBUG_NOBP=1`) |

### Step-by-step: attaching VS to an integration test

1. Open the solution in Visual Studio.
2. Find the integration test method you want to debug, for example:

    ```csharp
    [TestMethod]
    [NetCoreRunner(AcceptanceTestBase.NET9, DebugVSTestConsole = true)]
    public void MyTest(RunnerInfo runnerInfo)
    {
        // ...
    }
    ```

3. Set breakpoints in the vstest source code corresponding to what you are debugging (e.g. inside `vstest.console`, `testhost`, or the `datacollector` project).
4. Run that specific test case from the Test Explorer in Visual Studio.
5. The test infrastructure automatically builds `AttachVS.exe` (from `src/AttachVS`) and sets `VSTEST_DEBUG_ATTACHVS_PATH` to point to it. When vstest.console starts, it will invoke `AttachVS.exe`, which attaches the running Visual Studio instance to the launched process.
6. Your breakpoints in the vstest source code will be hit.

> **Note:** `DebugStopAtEntrypoint = false` (the default) sets `VSTEST_DEBUG_NOBP=1`, which skips the entry-point breakpoint to go directly to your breakpoints. Set `DebugStopAtEntrypoint = true` if you want to explore and are not sure where to put your breakpoint.

> **Note:** `AttachVS` looks for a running Visual Studio instance. Make sure you are running the integration test from within Visual Studio (not from the CLI) for the automatic attach to work. If you do run from the command line it will try to find VS instance using the AttachVS heuristic (look for parent process that is VS, look for the instance of VS that was started first).

## Debugging in general

There are several other environment variables that allow you to wait for debugger in a specific component, see [docs/environment-variables.md](docs/environment-variables.md#debug-variables)
