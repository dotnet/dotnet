// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

using Microsoft.TestPlatform.VsTestConsole.TranslationLayer.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests.TranslationLayerTests;

[TestClass]
// We need to dogfood the package built in this repo *-dev and we pack tha tp only on windows
[TestCategory("Windows-Review")]
public class SerialTestRunDecoratorTests : AcceptanceTestBase
{
    private IVsTestConsoleWrapper? _vstestConsoleWrapper;
    private RunEventHandler? _runEventHandler;
    private DiscoveryEventHandler? _discoveryEventHandler;
    private DiscoveryEventHandler2? _discoveryEventHandler2;
    private readonly string _runsettings = $$"""
<RunSettings>
    <RunConfiguration>
        <ForceOneTestAtTimePerTestHost>true</ForceOneTestAtTimePerTestHost>
    </RunConfiguration>
</RunSettings>
""";

    [MemberNotNull(nameof(_vstestConsoleWrapper), nameof(_runEventHandler), nameof(_discoveryEventHandler), nameof(_discoveryEventHandler2))]
    private void Setup(Dictionary<string, string?>? environmentVariables = null)
    {
        _vstestConsoleWrapper = GetVsTestConsoleWrapper(environmentVariables);
        _discoveryEventHandler = new DiscoveryEventHandler();
        _discoveryEventHandler2 = new DiscoveryEventHandler2();
        _runEventHandler = new RunEventHandler();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _vstestConsoleWrapper?.EndSession();
    }

    [TestMethod]
    [NetCoreTargetFrameworkDataSource]
    [NetFullTargetFrameworkDataSource]
    public void DiscoverTestsAndRunTestsSequentially(RunnerInfo runnerInfo)
    {
        // Arrange
        SetTestEnvironment(_testEnvironment, runnerInfo);
        Setup();

        // Act
        var testDll = GetAssetFullPath("SerializeTestRunTestProject.dll");
        _vstestConsoleWrapper.DiscoverTests(new string[] { testDll }, GetDefaultRunSettings(), _discoveryEventHandler);
        _vstestConsoleWrapper.RunTests(_discoveryEventHandler.DiscoveredTestCases, _runsettings, _runEventHandler);
        _runEventHandler.EnsureSuccess();

        // Assert
        Assert.AreEqual(10, _discoveryEventHandler.DiscoveredTestCases.Count);
        int failedTests = _runEventHandler.TestResults.Count(x => x.Outcome == TestOutcome.Failed);
        Assert.IsFalse(failedTests > 0, $"Number of failed tests {failedTests}");
    }

    [TestMethod]
    [NetCoreTargetFrameworkDataSource]
    [NetFullTargetFrameworkDataSource]
    public void DiscoverTestsAndRunTestsSequentially_DisabledByFeatureFlag(RunnerInfo runnerInfo)
    {
        // Arrange
        SetTestEnvironment(_testEnvironment, runnerInfo);
        Dictionary<string, string?>? environmentVariables = new() { { "VSTEST_DISABLE_SERIALTESTRUN_DECORATOR", "1" } };
        Setup(environmentVariables);

        // Act
        var testDll = GetAssetFullPath("SerializeTestRunTestProject.dll");
        _vstestConsoleWrapper.DiscoverTests(new string[] { testDll }, GetDefaultRunSettings(), _discoveryEventHandler);
        _vstestConsoleWrapper.RunTests(_discoveryEventHandler.DiscoveredTestCases, _runsettings, _runEventHandler);
        _runEventHandler.EnsureSuccess();

        // Assert
        Assert.AreEqual(10, _discoveryEventHandler.DiscoveredTestCases.Count);
        int failedTests = _runEventHandler.TestResults.Count(x => x.Outcome == TestOutcome.Failed);
        Assert.IsTrue(failedTests > 0, $"Number of failed tests {failedTests}");
    }

    [TestMethod]
    [NetCoreTargetFrameworkDataSource]
    [NetFullTargetFrameworkDataSource]
    public void DiscoverTestsAndRunTestsSequentially_IsNotSupportedForSources(RunnerInfo runnerInfo)
    {
        // Arrange
        SetTestEnvironment(_testEnvironment, runnerInfo);
        Setup();

        // Act
        var testDll = GetAssetFullPath("SerializeTestRunTestProject.dll");
        _vstestConsoleWrapper.RunTests(new string[] { testDll }, _runsettings, _runEventHandler);
        _ = Assert.ThrowsException<InvalidOperationException>(_runEventHandler.EnsureSuccess);

        StringBuilder builder = new();
        foreach (string? error in _runEventHandler.Errors)
        {
            builder.AppendLine(error);
        }

        Assert.IsTrue(_runEventHandler.Errors.Count > 0);
        Assert.IsTrue(_runEventHandler.Errors.Contains(VisualStudio.TestPlatform.Common.Resources.Resources.SerialTestRunInvalidScenario), $"Error messages\n:{builder}");
    }
}
