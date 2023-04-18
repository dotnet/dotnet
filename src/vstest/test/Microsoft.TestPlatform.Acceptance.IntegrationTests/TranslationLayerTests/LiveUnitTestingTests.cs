// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Microsoft.TestPlatform.VsTestConsole.TranslationLayer.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests.TranslationLayerTests;

[TestClass]
public class LiveUnitTestingTests : AcceptanceTestBase
{
    private IVsTestConsoleWrapper? _vstestConsoleWrapper;
    private DiscoveryEventHandler? _discoveryEventHandler;
    private RunEventHandler? _runEventHandler;

    [MemberNotNull(nameof(_vstestConsoleWrapper), nameof(_discoveryEventHandler), nameof(_runEventHandler))]
    public void Setup()
    {
        _vstestConsoleWrapper = GetVsTestConsoleWrapper();
        _discoveryEventHandler = new DiscoveryEventHandler();
        _runEventHandler = new RunEventHandler();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _vstestConsoleWrapper?.EndSession();
    }


    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void DiscoverTestsUsingLiveUnitTesting(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        Setup();

        string runSettingsXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
                                    <RunSettings>
                                        <RunConfiguration>
                                        <DisableAppDomain>true</DisableAppDomain>
                                        <DisableParallelization>true</DisableParallelization>
                                        </RunConfiguration>
                                    </RunSettings>";

        _vstestConsoleWrapper.DiscoverTests(
            GetTestAssemblies(),
            runSettingsXml,
            _discoveryEventHandler);

        // Assert
        Assert.AreEqual(6, _discoveryEventHandler.DiscoveredTestCases.Count);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void RunTestsWithLiveUnitTesting(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        Setup();

        string runSettingsXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
                                    <RunSettings>
                                        <RunConfiguration>
                                        <DisableAppDomain>true</DisableAppDomain>
                                        <DisableParallelization>true</DisableParallelization>
                                        </RunConfiguration>
                                    </RunSettings>";

        _vstestConsoleWrapper.RunTests(
            GetTestAssemblies(),
            runSettingsXml,
            _runEventHandler);

        // Assert
        Assert.AreEqual(6, _runEventHandler.TestResults.Count);
        Assert.AreEqual(2, _runEventHandler.TestResults.Count(t => t.Outcome == TestOutcome.Passed));
        Assert.AreEqual(2, _runEventHandler.TestResults.Count(t => t.Outcome == TestOutcome.Failed));
        Assert.AreEqual(2, _runEventHandler.TestResults.Count(t => t.Outcome == TestOutcome.Skipped));
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
