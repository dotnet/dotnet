// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Microsoft.TestPlatform.VsTestConsole.TranslationLayer.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Common.Telemetry;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests.TranslationLayerTests;

[TestClass]
public class RunSelectedTests : AcceptanceTestBase
{
    private IVsTestConsoleWrapper? _vstestConsoleWrapper;
    private RunEventHandler? _runEventHandler;
    private DiscoveryEventHandler? _discoveryEventHandler;

    [MemberNotNull(nameof(_vstestConsoleWrapper), nameof(_runEventHandler), nameof(_discoveryEventHandler))]
    private void Setup()
    {
        _vstestConsoleWrapper = GetVsTestConsoleWrapper();
        _runEventHandler = new RunEventHandler();
        _discoveryEventHandler = new DiscoveryEventHandler();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _vstestConsoleWrapper?.EndSession();
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void RunSelectedTestsWithoutTestPlatformOptions(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        Setup();

        _vstestConsoleWrapper.DiscoverTests(GetTestAssemblies(), GetDefaultRunSettings(), _discoveryEventHandler);
        var testCases = _discoveryEventHandler.DiscoveredTestCases;

        _vstestConsoleWrapper.RunTests(testCases, GetDefaultRunSettings(), _runEventHandler);

        // Assert
        Assert.AreEqual(6, _runEventHandler.TestResults.Count);
        Assert.AreEqual(2, _runEventHandler.TestResults.Count(t => t.Outcome == TestOutcome.Passed));
        Assert.AreEqual(2, _runEventHandler.TestResults.Count(t => t.Outcome == TestOutcome.Failed));
        Assert.AreEqual(2, _runEventHandler.TestResults.Count(t => t.Outcome == TestOutcome.Skipped));
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void RunSelectedTestsWithTestPlatformOptions(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        Setup();

        _vstestConsoleWrapper.DiscoverTests(GetTestAssemblies(), GetDefaultRunSettings(), _discoveryEventHandler);
        var testCases = _discoveryEventHandler.DiscoveredTestCases;

        _vstestConsoleWrapper.RunTests(
            testCases,
            GetDefaultRunSettings(),
            new TestPlatformOptions() { CollectMetrics = true },
            _runEventHandler);

        // Assert
        Assert.AreEqual(6, _runEventHandler.TestResults.Count);
        Assert.IsTrue(_runEventHandler.Metrics!.ContainsKey(TelemetryDataConstants.TargetDevice));
        Assert.IsTrue(_runEventHandler.Metrics.ContainsKey(TelemetryDataConstants.TargetFramework));
        Assert.IsTrue(_runEventHandler.Metrics.ContainsKey(TelemetryDataConstants.TargetOS));
        Assert.IsTrue(_runEventHandler.Metrics.ContainsKey(TelemetryDataConstants.TimeTakenInSecForRun));
        Assert.IsTrue(_runEventHandler.Metrics.ContainsKey(TelemetryDataConstants.NumberOfAdapterDiscoveredDuringExecution));
        Assert.IsTrue(_runEventHandler.Metrics.ContainsKey(TelemetryDataConstants.RunState));
    }

    private IList<string> GetTestAssemblies()
    {
        var testAssemblies = new List<string>
        {
            GetAssetFullPath("SimpleTestProject.dll"),
            GetAssetFullPath("SimpleTestProject2.dll")
        };

        return testAssemblies;
    }
}
