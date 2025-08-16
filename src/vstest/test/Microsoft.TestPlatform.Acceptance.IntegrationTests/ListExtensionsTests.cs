// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
// this is tested only on .NET Framework
[TestCategory("Windows-Review")]
public class ListExtensionsTests : AcceptanceTestBase
{
    [TestMethod]
    [NetFullTargetFrameworkDataSource(inIsolation: false, inProcess: true)]
    public void ListDiscoverersShouldShowInboxDiscoverers(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        InvokeVsTest("/listDiscoverers");

        if (IsDesktopRunner())
        {
            StdOutputContains("executor://mstestadapter/v1");
            StdOutputContains("executor://cppunittestexecutor/v1");
        }
        else
        {
            // There are no inbox adapters for dotnet core
            StdOutputDoesNotContains("executor://mstestadapter/v1");
            StdOutputDoesNotContains("executor://cppunittestexecutor/v1");
        }
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource(inIsolation: false, inProcess: true)]
    public void ListExecutorsShouldShowInboxExecutors(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        InvokeVsTest("/listExecutors");

        if (IsDesktopRunner())
        {
            StdOutputContains("executor://MSTestAdapter/v1");
            StdOutputContains("executor://CppUnitTestExecutor/v1");
            StdOutputContains("executor://UAPCppExecutorIdentifier");
        }
        else
        {
            // There are no inbox adapters for dotnet core
            StdOutputDoesNotContains("executor://MSTestAdapter/v1");
            StdOutputDoesNotContains("executor://CppUnitTestExecutor/v1");
            StdOutputDoesNotContains("executor://UAPCppExecutorIdentifier");
        }
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource(inIsolation: false, inProcess: true)]
    public void ListLoggersShouldShowInboxLoggers(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        InvokeVsTest("/listLoggers");

        StdOutputContains("logger://Microsoft/TestPlatform/Extensions/Blame/v1");
        StdOutputContains("logger://Microsoft/TestPlatform/TrxLogger/v1");
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource(inIsolation: false, inProcess: true)]
    public void ListSettingsProvidersShouldShowInboxSettingsProviders(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        InvokeVsTest("/listSettingsProviders");

        if (IsDesktopRunner())
        {
            StdOutputContains("MSTestSettingsProvider");
        }
        else
        {
            // There are no inbox adapters for dotnet core
            StdOutputDoesNotContains("MSTestSettingsProvider");
        }
    }
}
