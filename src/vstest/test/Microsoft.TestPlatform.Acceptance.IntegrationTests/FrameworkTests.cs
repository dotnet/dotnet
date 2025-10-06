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
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void FrameworkArgumentShouldWork(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var arguments = PrepareArguments(GetSampleTestAssembly(), string.Empty, string.Empty, string.Empty, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, " ", $"/Framework:{FrameworkArgValue}");

        InvokeVsTest(arguments);
        ValidateSummaryStatus(1, 1, 1);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
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
    [NetFullTargetFrameworkDataSource(useCoreRunner: false)]
    //[NetCoreTargetFrameworkDataSource]
    public void OnWrongFrameworkPassedTestRunShouldNotRun(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var arguments = PrepareArguments(GetSampleTestAssembly(), string.Empty, string.Empty, string.Empty, resultsDirectory: TempDirectory.Path);
        if (runnerInfo.TargetFramework.Contains("netcore"))
        {
            arguments = string.Concat(arguments, " ", "/Framework:Framework45");
        }
        else
        {
            arguments = string.Concat(arguments, " ", "/Framework:FrameworkCore10");
        }
        InvokeVsTest(arguments);

        if (runnerInfo.TargetFramework.Contains("netcore"))
        {
            StdOutputContains("No test is available");
        }
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    [TestCategory("Windows-Review")]
    public void RunSpecificTestsShouldWorkWithFrameworkInCompatibleWarning(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var arguments = PrepareArguments(GetSampleTestAssembly(), string.Empty, string.Empty, string.Empty, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, " ", "/tests:PassingTest");
        arguments = string.Concat(arguments, " ", "/Framework:Framework40");

        InvokeVsTest(arguments);

        // When this test runs it provides an incorrect desired framework for the run. E.g. the dll is actually net8.0
        // but we request to run as .NET Framework 4.0. On windows this has predictable results, for net8.0 dll we fail
        // to load it into .NET Framework testhost.exe, and fail with "No test is available". For .NET Framework dll, we
        // just log a warning saying that we provided .NET Framework 472 dlls (or whatever the current tfm for test dlls is),
        // but the settings requested .NET Framework 4.0. The test will still run because .NET Framework is compatible, and in reality
        // the system has .NET Framework 472 or newer installed, which runs even if we ask for .NET Framework 4.0 testhost.
        //
        // On Linux and Mac we execute only net8.0 tests, and even though we force .NET Framework, we end up running on mono
        // which is suprisingly able to run the .NET CoreApp 3.1 dll, so we still just see a warning and 1 completed test.
        var isWindows = Environment.OSVersion.Platform.ToString().StartsWith("Win");
        if (runnerInfo.TargetFramework.Contains("net8") && isWindows)
        {
            StdOutputContains("No test is available");
        }
        else
        {
            StdOutputContains("Following DLL(s) do not match current settings, which are .NETFramework,Version=v4.0 framework and X64 platform.");
            ValidateSummaryStatus(1, 0, 0);
        }
    }
}
