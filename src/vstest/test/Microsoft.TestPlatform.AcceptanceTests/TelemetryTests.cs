// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestPlatform.Common.Telemetry;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
public class TelemetryTests : AcceptanceTestBase
{
    private const string TELEMETRY_OPTEDIN = "VSTEST_TELEMETRY_OPTEDIN";
    private const string LOG_TELEMETRY = "VSTEST_LOGTELEMETRY";
    private const string LOG_TELEMETRY_PATH = "VSTEST_LOGTELEMETRY_PATH";

    [TestMethod]
    [NetFullTargetFrameworkDataSourceAttribute(inIsolation: true, inProcess: true)]
    [NetCoreTargetFrameworkDataSource]
    public void RunTestsShouldPublishMetrics(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        RunTests(runnerInfo);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSourceAttribute(inIsolation: true, inProcess: true)]
    [NetCoreTargetFrameworkDataSource]
    public void DiscoverTestsShouldPublishMetrics(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        DiscoverTests(runnerInfo);
    }

    private void RunTests(RunnerInfo runnerInfo)
    {
        if (runnerInfo.IsNetRunner)
        {
            Assert.Inconclusive("Telemetry API is not supported for .NetCore runner");
            return;
        }

        var assemblyPaths = GetAssetFullPath("SimpleTestProject2.dll");

        var env = new Dictionary<string, string?>
        {
            [LOG_TELEMETRY_PATH] = TempDirectory.Path,
            [TELEMETRY_OPTEDIN] = "1",
            [LOG_TELEMETRY] = "1",
        };

        InvokeVsTestForExecution(assemblyPaths, GetTestAdapterPath(), FrameworkArgValue, string.Empty, env);
        ValidateOutput("Execution", TempDirectory);
    }

    private void DiscoverTests(RunnerInfo runnerInfo)
    {
        if (runnerInfo.IsNetRunner)
        {
            Assert.Inconclusive("Telemetry API is not supported for .NetCore runner");
            return;
        }

        var assemblyPaths = GetAssetFullPath("SimpleTestProject2.dll");

        var env = new Dictionary<string, string?>
        {
            [LOG_TELEMETRY_PATH] = TempDirectory.Path,
            [TELEMETRY_OPTEDIN] = "1",
            [LOG_TELEMETRY] = "1",
        };

        InvokeVsTestForDiscovery(assemblyPaths, GetTestAdapterPath(), string.Empty, FrameworkArgValue, env);
        ValidateOutput("Discovery", TempDirectory);
    }

    private static void ValidateOutput(string command, TempDirectory tempDirectory)
    {
        if (!Directory.Exists(tempDirectory.Path))
        {
            Assert.Fail("Could not find the telemetry logs folder at {0}", tempDirectory.Path);
        }

        bool isValid = false;
        var directory = new DirectoryInfo(tempDirectory.Path);
        var file = directory.GetFiles().OrderByDescending(f => f.CreationTime).First();

        string[] lines = File.ReadAllLines(file.FullName);

        foreach (var line in lines)
        {
            if (line.Contains(TelemetryDataConstants.TestExecutionCompleteEvent) && command.Equals("Execution", StringComparison.Ordinal))
            {
                var isPresent = line.Contains(
                                    TelemetryDataConstants.NumberOfAdapterDiscoveredDuringExecution)
                                && line.Contains(TelemetryDataConstants.NumberOfAdapterUsedToRunTests)
                                && line.Contains(TelemetryDataConstants.ParallelEnabledDuringExecution + '=' + "False")
                                && line.Contains(TelemetryDataConstants.NumberOfSourcesSentForRun + '=' + "1")
                                && line.Contains(TelemetryDataConstants.RunState + '=' + "Completed")
                                && line.Contains(TelemetryDataConstants.TimeTakenByAllAdaptersInSec)
                                && line.Contains(TelemetryDataConstants.TotalTestsRun + '=' + "3")
                                && line.Contains(TelemetryDataConstants.TotalTestsRanByAdapter)
                                && line.Contains(TelemetryDataConstants.TimeTakenToRunTestsByAnAdapter);

                isValid = isPresent;
                break;
            }
            else if (line.Contains(TelemetryDataConstants.TestDiscoveryCompleteEvent) && command.Equals("Discovery", StringComparison.Ordinal))
            {
                var isPresent = line.Contains(TelemetryDataConstants.TotalTestsDiscovered + '=' + "3")
                                && line.Contains(TelemetryDataConstants.ParallelEnabledDuringDiscovery + '=' + "False")
                                && line.Contains(TelemetryDataConstants.TimeTakenInSecForDiscovery)
                                && line.Contains(TelemetryDataConstants.TimeTakenToLoadAdaptersInSec)
                                && line.Contains(TelemetryDataConstants.TimeTakenInSecByAllAdapters)
                                && line.Contains(TelemetryDataConstants.TotalTestsByAdapter)
                                && line.Contains(TelemetryDataConstants.TimeTakenToDiscoverTestsByAnAdapter)
                                && line.Contains(TelemetryDataConstants.DiscoveryState + "=Completed")
                                && line.Contains(TelemetryDataConstants.NumberOfSourcesSentForDiscovery + '=' + "1")
                                && line.Contains(
                                    TelemetryDataConstants.NumberOfAdapterDiscoveredDuringDiscovery)
                                && line.Contains(TelemetryDataConstants.NumberOfAdapterUsedToDiscoverTests);

                isValid = isPresent;
                break;
            }
        }

        Assert.IsTrue(isValid);
    }
}
