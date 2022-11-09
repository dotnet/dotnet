// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Microsoft.TestPlatform.VsTestConsole.TranslationLayer.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests.TranslationLayerTests;

/// <summary>
/// The Run Tests using VsTestConsoleWrapper API's
/// </summary>
[TestClass]
public class RunTestsWithFilterTests : AcceptanceTestBase
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
    [TestCategory("Windows-Review")]
    [RunnerCompatibilityDataSource]
    public void RunTestsWithTestCaseFilter(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        _runEventHandler = new RunEventHandler();

        var vstestConsoleWrapper = GetVsTestConsoleWrapper();
        var sources = new List<string> { GetAssetFullPath("MSTestProject1.dll") };

        vstestConsoleWrapper.RunTests(
            sources,
            GetDefaultRunSettings(),
            new TestPlatformOptions() { TestCaseFilter = "FullyQualifiedName=MSTestProject1.UnitTest1.PassingTest" },
            _runEventHandler);

        // Assert
        Assert.AreEqual(1, _runEventHandler.TestResults.Count);
        Assert.AreEqual(TestOutcome.Passed, _runEventHandler.TestResults.First().Outcome);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void RunTestsWithFastFilter(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        Setup();

        var sources = new List<string> { GetAssetFullPath("SimpleTestProject.dll") };

        _vstestConsoleWrapper.RunTests(
            sources,
            GetDefaultRunSettings(),
            new TestPlatformOptions() { TestCaseFilter = "FullyQualifiedName=SampleUnitTestProject.UnitTest1.PassingTest | FullyQualifiedName=SampleUnitTestProject.UnitTest1.FailingTest" },
            _runEventHandler);

        // Assert
        Assert.AreEqual(2, _runEventHandler.TestResults.Count);
        Assert.AreEqual(1, _runEventHandler.TestResults.Count(t => t.Outcome == TestOutcome.Passed));
        Assert.AreEqual(1, _runEventHandler.TestResults.Count(t => t.Outcome == TestOutcome.Failed));
    }
}
