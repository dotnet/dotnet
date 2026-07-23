// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
public class FrameworkTests : AcceptanceTestBase
{

    [TestMethod]
    [TestMatrix(testHost: Net)]
    public void FrameworkArgumentShouldWork(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var arguments = PrepareArguments(GetSampleTestAssembly(), string.Empty, string.Empty, string.Empty, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, " ", $"/Framework:{FrameworkArgValue}");

        InvokeVsTest(arguments);
        ValidateSummaryStatus(1, 1, 1);
    }

    [TestMethod]
    [TestMatrix(testHost: Net)]
    public void FrameworkShortNameArgumentShouldWork(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var arguments = PrepareArguments(GetSampleTestAssembly(), string.Empty, string.Empty, string.Empty, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, " ", $"/Framework:{_testEnvironment.TargetFramework}");

        InvokeVsTest(arguments);
        ValidateSummaryStatus(1, 1, 1);
    }

    [TestMethod]
    // framework runner not available on Linux
    [TestCategory("Windows-Review")]
    [TestMatrix(console: NetFx, testHost: NetFx)]
    //[TestMatrix(testHost: Net)]
    public void OnWrongFrameworkPassedTestRunShouldNotRun(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var arguments = PrepareArguments(GetSampleTestAssembly(), string.Empty, string.Empty, string.Empty, resultsDirectory: TempDirectory.Path);
        if (runnerInfo.IsNetTarget)
        {
            arguments = string.Concat(arguments, " ", "/Framework:Framework45");
        }
        else
        {
            arguments = string.Concat(arguments, " ", "/Framework:FrameworkCore10");
        }
        InvokeVsTest(arguments);

        if (runnerInfo.IsNetTarget)
        {
            StdOutputContains("No test is available");
        }
    }

    [TestMethod]
    [TestMatrix(testHost: NetFx)]
    [TestMatrix(testHost: Net)]
    // The .NET (Core) runner produces a different framework-incompatible warning on non-Windows, so keep this Windows-only.
    [TestCategory("Windows-Review")]
    public void RunSpecificTestsShouldWorkWithFrameworkInCompatibleWarning(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var arguments = PrepareArguments(GetSampleTestAssembly(), string.Empty, string.Empty, string.Empty, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, " ", "/tests:PassingTest");
        arguments = string.Concat(arguments, " ", "/Framework:Framework40");

        InvokeVsTest(arguments);

        // When this test runs it provides an incorrect desired framework for the run. E.g. the dll is actually net11.0
        // but we request to run as .NET Framework 4.0. On windows this has predictable results, for net11.0 dll we fail
        // to load it into .NET Framework testhost.exe, and fail with "No test is available". For .NET Framework dll, we
        // just log a warning saying that we provided .NET Framework 481 dlls (or whatever the current tfm for test dlls is),
        // but the settings requested .NET Framework 4.0. The test will still run because .NET Framework is compatible, and in reality
        // the system has .NET Framework 481 or newer installed, which runs even if we ask for .NET Framework 4.0 testhost.
        //
        // This test is Windows-Review only, so it does not run on Linux or Mac in CI. If it is run there manually,
        // forcing .NET Framework now fails fast, because the .NET Framework test host is no longer launched through Mono.
        var isWindows = Environment.OSVersion.Platform.ToString().StartsWith("Win");
        if (!isWindows)
        {
            StdErrorContains("Running .NET Framework tests is supported on Windows only");
        }
        else if (runnerInfo.TargetFramework.Contains("net11"))
        {
            StdOutputContains("No test is available");
        }
        else
        {
            StdOutputContains("Following DLL(s) do not match current settings, which are .NETFramework,Version=v4.0 framework and X64 platform.");
            ValidateSummaryStatus(1, 0, 0);
        }
    }

    [TestMethod]
    [NetCoreTargetFrameworkDataSource]
    public void RunningNetFrameworkTestsOnNonWindowsShouldFailWithClearError(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        // Force the run to use the .NET Framework test host (testhost.exe). That host exists only on
        // Windows. On other operating systems we used to fall back to Mono, which is no longer supported,
        // so the run should fail fast with a clear, actionable message instead of an opaque Mono error.
        var arguments = PrepareArguments(GetSampleTestAssembly(), string.Empty, string.Empty, string.Empty, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, " ", "/Framework:Framework40");

        InvokeVsTest(arguments);

        var isWindows = Environment.OSVersion.Platform.ToString().StartsWith("Win");
        if (isWindows)
        {
            // On Windows the .NET Framework test host is available, so the "Windows only" error must not appear.
            StdErrorDoesNotContains("Running .NET Framework tests is supported on Windows only");
        }
        else
        {
            // The run must fail fast with a clear message, not merely log a warning.
            StdErrorContains("Running .NET Framework tests is supported on Windows only");
            ExitCodeEquals(1);
        }
    }
}
