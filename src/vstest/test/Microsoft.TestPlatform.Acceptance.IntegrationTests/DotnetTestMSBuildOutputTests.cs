// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

/// <summary>
/// Running dotnet test + csproj and using MSBuild for the output.
/// </summary>
[TestClass]
public class DotnetTestMSBuildOutputTests : AcceptanceTestBase
{
    [TestMethod]
    // patched dotnet is not published on non-windows systems
    [TestCategory("Windows-Review")]
    [NetCoreTargetFrameworkDataSource(useDesktopRunner: false)]
    public void MSBuildLoggerCanBeEnabledByBuildPropertyAndDoesNotEatSpecialChars(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var projectPath = GetIsolatedTestAsset("TerminalLoggerTestProject.csproj");
        // Forcing terminal logger so we can see the output when it is redirected
        InvokeDotnetTest($@"{projectPath} -tl:on -nodereuse:false /p:PackageVersion={IntegrationTestEnvironment.LatestLocallyBuiltNugetVersion}");

        // The output:
        // Determining projects to restore...
        //   Restored C:\Users\nohwnd\AppData\Local\Temp\vstest\xvoVt\SimpleTestProject.csproj (in 441 ms).
        //   SimpleTestProject -> C:\Users\nohwnd\AppData\Local\Temp\vstest\xvoVt\artifacts\bin\TestAssets\SimpleTestProject\Debug\net462\SimpleTestProject.dll
        //   SimpleTestProject -> C:\Users\nohwnd\AppData\Local\Temp\vstest\xvoVt\artifacts\bin\TestAssets\SimpleTestProject\Debug\net8.0\SimpleTestProject.dll
        // C:\Users\nohwnd\AppData\Local\Temp\vstest\xvoVt\UnitTest1.cs(41): error VSTEST1: (FailingTest) SampleUnitTestProject.UnitTest1.FailingTest() Assert.AreEqual failed. Expected:<2>. Actual:<3>.  [C:\Users\nohwnd\AppData\Local\Temp\vstest\xvoVt\SimpleTestProject.csproj::TargetFramework=net462]
        // C:\Users\nohwnd\AppData\Local\Temp\vstest\xvoVt\UnitTest1.cs(41): error VSTEST1: (FailingTest) SampleUnitTestProject.UnitTest1.FailingTest() Assert.AreEqual failed. Expected:<2>. Actual:<3>.  [C:\Users\nohwnd\AppData\Local\Temp\vstest\xvoVt\SimpleTestProject.csproj::TargetFramework=net8.0]

        StdOutputContains("TESTERROR");
        StdOutputContains("FailingTest (");
        StdOutputContains("): Error Message: Assert.AreEqual failed. Expected:<ğğğ𦮙我們剛才從𓋴𓅓𓏏𓇏𓇌𓀀>. Actual:<not the same>.");
        StdOutputContains("at TerminalLoggerUnitTests.UnitTest1.FailingTest() in");
        // We are sending those as low prio messages, they won't show up on screen but will be in binlog.
        //StdOutputContains("passed PassingTest");
        //StdOutputContains("skipped SkippingTest");
        ExitCodeEquals(1);
    }

    [TestMethod]
    // patched dotnet is not published on non-windows systems
    [TestCategory("Windows-Review")]
    [NetCoreTargetFrameworkDataSource(useDesktopRunner: false)]
    public void MSBuildLoggerCanBeDisabledByBuildProperty(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var projectPath = GetIsolatedTestAsset("TerminalLoggerTestProject.csproj");
        InvokeDotnetTest($@"{projectPath} -nodereuse:false /p:VsTestUseMSBuildOutput=false /p:PackageVersion={IntegrationTestEnvironment.LatestLocallyBuiltNugetVersion}");

        // Check that we see the summary that is printed from the console logger, meaning the new output is disabled.
        StdOutputContains("Failed! - Failed: 1, Passed: 1, Skipped: 1, Total: 3, Duration:");
        // We are sending those as low prio messages, they won't show up on screen but will be in binlog.
        //StdOutputContains("passed PassingTest");
        //StdOutputContains("skipped SkippingTest");
        ExitCodeEquals(1);
    }


    [TestMethod]
    // patched dotnet is not published on non-windows systems
    [TestCategory("Windows-Review")]
    [NetCoreTargetFrameworkDataSource(useDesktopRunner: false)]
    public void MSBuildLoggerCanBeDisabledByEnvironmentVariableProperty(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var projectPath = GetIsolatedTestAsset("TerminalLoggerTestProject.csproj");
        InvokeDotnetTest($@"{projectPath} -nodereuse:false /p:PackageVersion={IntegrationTestEnvironment.LatestLocallyBuiltNugetVersion}", environmentVariables: new Dictionary<string, string?> { ["MSBUILDENSURESTDOUTFORTASKPROCESSES"] = "1" });

        // Check that we see the summary that is printed from the console logger, meaning the new output is disabled.
        StdOutputContains("Failed! - Failed: 1, Passed: 1, Skipped: 1, Total: 3, Duration:");

        ExitCodeEquals(1);
    }
}
