# Scenario Tests

This repository contains a basic test harness and set of scenario tests designed to execute on an installed .NET SDK. 'Installed' might mean an extracted .NET SDK archive, or a .NET SDK,installed from an msi, pkg, deb package, etc. The goal is to provide a set of tests which are independent of development repository assumptions or restrictions, and which exercise large swaths of product functionality.

## Design and Organization

The scenario tests should be testing an installed SDK. Because this SDK and associated runtime may have issues, it's important to be able to divorce the execution of the test harness from the SDK and runtime being tested. To achieve this, the harness is designed to be a self-discovering executable, which can optionally be deployed as a self-contained application where necessary. Furthermore, the tests themselves execute commands in separate processes from the harness itself.

Let's look at an example. Consider a test that should verify that "dotnet new console" works properly. The executable would contain a test method which would create a new process, running `dotnet` with parameters `new console --name myapp`. In the psuedocode example below, VerifyConsoleTemplate is running within the host runtime (either a self-contained or framework dependent deployment), launching processes against 

```
// This method runs on the host runtime
[Fact]
public void VerifyConsoleTemplate()
{
    // Execute the "dotnet" executable in TestTargetSdkDirectory with parameters "new console --name myapp".
    // Working directory is TestTempDirectory
    LaunchProcess(Path.Combine(Test.TestTargetSdkDirectory, "dotnet"), "new console --name myapp", Test.TestTempDirectory)

    VerifyOutput(Path.Combine(Test.TestTempDirectory, "myapp"));
}
```

## Organization

The repository is organized as a set of executable projects and a folder of [common utilities/files](src/Microsoft.DotNet.ScenarioTests.Common/), which also contains the test harness [entry point](src/Microsoft.DotNet.ScenarioTests.Common/Program.cs). The test harness entry point is automatically included in each test project.

Scenario tests for logically different target areas should exist in separate projects.

## Build and Executing the Tests

The tests can be built with `./build.sh` and `./build.cmd` from the root of the repo, or with `dotnet build`.

Once built, a test project can be executed in the following way:

`dotnet <test dll> --dotnet-root <root of SDK installation>`

Optionally, for SxS SDK installations, a `-sdk-version` option may be used to override the default SDK version targeted for testing.

For other options, see the help of the scenario test harness:

`dotnet <test dll> --help`