// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;

using FluentAssertions;

using Microsoft.TestPlatform.Library.IntegrationTests.TranslationLayerTests.EventHandler;
using Microsoft.TestPlatform.TestUtilities;
using Microsoft.TestPlatform.VsTestConsole.TranslationLayer.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Common.Telemetry;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.Library.IntegrationTests.TranslationLayerTests;

/// <summary>
/// The Run Tests using VsTestConsoleWrapper API's
/// </summary>
[TestClass]
public class RunTests : AcceptanceTestBase
{
    private IVsTestConsoleWrapper? _vstestConsoleWrapper;
    private RunEventHandler? _runEventHandler;

    [MemberNotNull(nameof(_vstestConsoleWrapper), nameof(_runEventHandler))]
    private void Setup()
    {
        _vstestConsoleWrapper = GetVsTestConsoleWrapper();
        _runEventHandler = new RunEventHandler();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _vstestConsoleWrapper?.EndSession();
    }

    [TestMethod]
    // WrapperCompatibilityDataSource includes the .NET Framework runner, which is not available on Linux/macOS.
    [TestCategory("Windows-Review")]
    [CompatibilityMatrix(CompatScenario.Wrapper)]
    public void RunAllTests(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        _vstestConsoleWrapper = GetVsTestConsoleWrapper();
        var runEventHandler = new RunEventHandler();
        _vstestConsoleWrapper.RunTests(GetTestDlls("MSTestProject1.dll", "MSTestProject2.dll"), GetDefaultRunSettings(), runEventHandler);

        // Assert
        Assert.HasCount(6, runEventHandler.TestResults);
        Assert.AreEqual(2, runEventHandler.TestResults.Count(t => t.Outcome == TestOutcome.Passed));
        Assert.AreEqual(2, runEventHandler.TestResults.Count(t => t.Outcome == TestOutcome.Failed));
        Assert.AreEqual(2, runEventHandler.TestResults.Count(t => t.Outcome == TestOutcome.Skipped));
    }

    [TestMethod]
    [TestMatrix(testHost: Net)]
    [TestCategory("Smoke")]
    public void RunAllTestsFromDlls(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        _vstestConsoleWrapper = GetVsTestConsoleWrapper();
        var runEventHandler = new RunEventHandler();
        _vstestConsoleWrapper.RunTests([GetAssetFullPath("SimpleTestProject.dll"), GetAssetFullPath("SimpleTestProject2.dll")], GetDefaultRunSettings(), runEventHandler);

        // Assert
        Assert.HasCount(6, runEventHandler.TestResults);
        Assert.AreEqual(2, runEventHandler.TestResults.Count(t => t.Outcome == TestOutcome.Passed));
        Assert.AreEqual(2, runEventHandler.TestResults.Count(t => t.Outcome == TestOutcome.Failed));
        Assert.AreEqual(2, runEventHandler.TestResults.Count(t => t.Outcome == TestOutcome.Skipped));
    }

    [TestMethod]
    // WrapperCompatibilityDataSource includes the .NET Framework runner, which is not available on Linux/macOS.
    [TestCategory("Windows-Review")]
    [CompatibilityMatrix(CompatScenario.Wrapper)]
    public void RunAllTestsWithMixedTFMsWillRunTestsFromAllProvidedDllEvenWhenTheyMixTFMs(RunnerInfo runnerInfo)
    {
        // Arrange
        SetTestEnvironment(_testEnvironment, runnerInfo);

        _vstestConsoleWrapper = GetVsTestConsoleWrapper();
        var runEventHandler = new RunEventHandler();
        // Use SimpleTestProject4 which has a minimal test adapter. This helps us test that the console / testhost are compatible.
        // Rather than testing that the console & mstest (or other adapter) and compatible. If we use mstest directly it fails on
        // very old versions of vstest.console.
        var netFrameworkDll = GetTestDllForFramework("SimpleTestProject4.dll", HOST_NETFX, automaticallyResolveCompatibilityTestAsset: false);
        var netDll = GetTestDllForFramework("SimpleTestProject4.dll", HOST_NET, automaticallyResolveCompatibilityTestAsset: false);

        // Act
        // We have no preference around what TFM is used. It will be autodetected.
        var runsettingsXml = "<RunSettings><RunConfiguration></RunConfiguration></RunSettings>";
        _vstestConsoleWrapper.RunTests(new[] { netFrameworkDll, netDll }, runsettingsXml, runEventHandler);

        // Assert
        runEventHandler.Errors.Should().BeEmpty();
        runEventHandler.TestResults.Should().HaveCount(6, "we run all tests from both assemblies");
    }

    [TestMethod]
    [TestMatrix(testHost: Net)]
    public void EndSessionShouldEnsureVstestConsoleProcessDies(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        Setup();

        _vstestConsoleWrapper.RunTests(GetTestAssemblies(), GetDefaultRunSettings(), _runEventHandler);
        // TODO: this is ugly and it could be useful for the consumer of wrapper to actually know what process they are using, so publishing this would be better
        var processManager = (_vstestConsoleWrapper).GetType().GetField("_vstestConsoleProcessManager", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!.GetValue(_vstestConsoleWrapper)!;
        var processId = (int)processManager.GetType().GetProperty("ProcessId", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)!.GetValue(processManager)!;
        var consoleProcess = Process.GetProcessById(processId);
        Assert.IsFalse(consoleProcess.HasExited, "vstest.console process did not exit");

        _vstestConsoleWrapper!.EndSession();
        _vstestConsoleWrapper = null;

        Assert.IsTrue(consoleProcess.HasExited, "vstest.console did not start");
    }

    [TestMethod]
    [TestMatrix(testHost: Net)]
    public void RunTestsWithTelemetryOptedIn(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        Setup();

        _vstestConsoleWrapper.RunTests(
            GetTestAssemblies(),
            GetDefaultRunSettings(),
            new TestPlatformOptions() { CollectMetrics = true },
            _runEventHandler);

        // Assert
        Assert.HasCount(6, _runEventHandler.TestResults, _runEventHandler.ToString());
        Assert.IsTrue(_runEventHandler.Metrics!.ContainsKey(TelemetryDataConstants.TargetDevice));
        Assert.IsTrue(_runEventHandler.Metrics.ContainsKey(TelemetryDataConstants.TargetFramework));
        Assert.IsTrue(_runEventHandler.Metrics.ContainsKey(TelemetryDataConstants.TargetOS));
        Assert.IsTrue(_runEventHandler.Metrics.ContainsKey(TelemetryDataConstants.TimeTakenInSecForRun));
        Assert.IsTrue(_runEventHandler.Metrics.ContainsKey(TelemetryDataConstants.NumberOfAdapterDiscoveredDuringExecution));
        Assert.IsTrue(_runEventHandler.Metrics.ContainsKey(TelemetryDataConstants.RunState));
    }

    [TestMethod]
    [TestMatrix(testHost: Net)]
    public void RunTestsWithTelemetryOptedOut(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        Setup();

        _vstestConsoleWrapper.RunTests(
            GetTestAssemblies(),
            GetDefaultRunSettings(),
            new TestPlatformOptions() { CollectMetrics = false },
            _runEventHandler);

        // Assert
        Assert.HasCount(6, _runEventHandler.TestResults, _runEventHandler.ToString());
        Assert.HasCount(0, _runEventHandler.Metrics!, _runEventHandler.ToString());
    }

    [TestMethod]
    // This is testing the behavior of crash in testhost, run on different testhost, and just .NET runner.
    // The assertion below branches on the .NET Framework-specific stack overflow message, and the
    // .NET Framework testhost is only available on Windows, so this runs as Windows-Review.
    [TestCategory("Windows-Review")]
    [TestMatrix(console: Net, testHost: NetFx)]
    [TestMatrix(console: Net, testHost: Net)]
    public void RunTestsShouldThrowOnStackOverflowException(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        Setup();

        var source = new[] { GetAssetFullPath("SimpleTestProject3.dll") };

        _vstestConsoleWrapper.RunTests(
            source,
            GetDefaultRunSettings(),
            new TestPlatformOptions() { TestCaseFilter = "ExitWithStackoverFlow" },
            _runEventHandler);

        var errorMessagePattern = runnerInfo.IsNetFrameworkTarget
            ? $"The active test run was aborted. Reason: Test host process crashed : Process path: *{Environment.NewLine}Process is terminated due to StackOverflowException.*"
            : $"The active test run was aborted. Reason: Test host process crashed : Process path: *{Environment.NewLine}Stack overflow.*";

        _runEventHandler.Errors.Should().ContainSingle()
            .Which.Should().Match(errorMessagePattern);
    }

    [TestMethod]
    [TestMatrix(console: Net, testHost: Net)]
    public void RunTestsShouldShowProperWarningOnNoTestsForTestCaseFilter(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        Setup();

        var testAssemblyName = "SimpleTestProject2.dll";
        var source = new List<string>() { GetAssetFullPath(testAssemblyName) };

        var veryLongTestCaseFilter =
            "FullyQualifiedName=VeryLongTestCaseNameeeeeeeeeeeeee" +
            "eeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee" +
            "eeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee" +
            "eeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee" +
            "eeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee";

        _vstestConsoleWrapper.RunTests(
            source,
            GetDefaultRunSettings(),
            new TestPlatformOptions() { TestCaseFilter = veryLongTestCaseFilter },
            _runEventHandler);

        var expectedFilter = veryLongTestCaseFilter.Substring(0, 256) + "...";

        // Assert
        Assert.StartsWith($"No test matches the given testcase filter `{expectedFilter}` in", _runEventHandler.LogMessage);
        Assert.EndsWith(testAssemblyName, _runEventHandler.LogMessage);

        Assert.AreEqual(TestMessageLevel.Warning, _runEventHandler.TestMessageLevel);
    }

    private IList<string> GetTestAssemblies()
    {
        return new List<string>
        {
            GetAssetFullPath("SimpleTestProject.dll"),
            GetAssetFullPath("SimpleTestProject2.dll")
        };
    }

    private class TestHostLauncher : ITestHostLauncher2
    {
        public bool IsDebug => true;

        public bool AttachDebuggerToProcess(int pid)
        {
            return true;
        }

        public bool AttachDebuggerToProcess(int pid, CancellationToken cancellationToken)
        {
            return true;
        }

        public int LaunchTestHost(TestProcessStartInfo defaultTestHostStartInfo)
        {
            return -1;
        }

        public int LaunchTestHost(TestProcessStartInfo defaultTestHostStartInfo, CancellationToken cancellationToken)
        {
            return -1;
        }
    }
}
