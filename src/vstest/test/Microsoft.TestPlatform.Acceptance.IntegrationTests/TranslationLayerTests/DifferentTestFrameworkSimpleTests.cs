// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.TestPlatform.VsTestConsole.TranslationLayer.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
//using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests.TranslationLayerTests;

/// <summary>
/// The Run Tests using VsTestConsoleWrapper API's
/// </summary>
[TestClass]
public class DifferentTestFrameworkSimpleTests : AcceptanceTestBase
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
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void RunTestsWithNunitAdapter(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        Setup();

        var sources = new List<string>
        {
            GetAssetFullPath("NUTestProject.dll")
        };

        _vstestConsoleWrapper.RunTests(
            sources,
            GetDefaultRunSettings(),
            _runEventHandler);

        var testCase =
            _runEventHandler.TestResults.Where(tr => tr.TestCase.DisplayName.Equals("PassTestMethod1"));

        // Assert
        Assert.AreEqual(2, _runEventHandler.TestResults.Count);
        Assert.AreEqual(1, _runEventHandler.TestResults.Count(t => t.Outcome == TestOutcome.Passed));
        Assert.AreEqual(1, _runEventHandler.TestResults.Count(t => t.Outcome == TestOutcome.Failed));

        // Release builds optimize code, hence line numbers are different.
        if (IntegrationTestEnvironment.BuildConfiguration.StartsWith("release", StringComparison.OrdinalIgnoreCase))
        {
            Assert.AreEqual(14, testCase.First().TestCase.LineNumber);
        }
        else
        {
            Assert.AreEqual(13, testCase.First().TestCase.LineNumber);
        }
    }

    [TestMethod]
    // there are logs in the diagnostic log, it is failing with NullReferenceException because path is null
    [TestCategory("Windows-Review")]
    [NetFullTargetFrameworkDataSource(useCoreRunner: true, useDesktopRunner: false)]
    // [NetCoreTargetFrameworkDataSource]
    public void RunTestsWithXunitAdapter(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        Setup();

        string testAssemblyPath = _testEnvironment.GetTestAsset("XUTestProject.dll");
        var sources = new List<string> { testAssemblyPath };
        var testAdapterPath = Directory.EnumerateFiles(GetTestAdapterPath(UnitTestFramework.XUnit), "*.TestAdapter.dll").ToList();
        _vstestConsoleWrapper.InitializeExtensions(new List<string>() { testAdapterPath.First() });

        _vstestConsoleWrapper.RunTests(
            sources,
             $@"<?xml version=""1.0"" encoding=""utf-8""?>
            <RunSettings>
                <RunConfiguration>
                    <TargetFrameworkVersion>{runnerInfo.TargetFramework}</TargetFrameworkVersion>
                </RunConfiguration>
            </RunSettings>",
            _runEventHandler);

        var testCase =
            _runEventHandler.TestResults.Where(tr => tr.TestCase.DisplayName.Equals("xUnitTestProject.Class1.PassTestMethod1"));

        // Assert
        Assert.AreEqual(2, _runEventHandler.TestResults.Count);
        Assert.AreEqual(1, _runEventHandler.TestResults.Count(t => t.Outcome == TestOutcome.Passed));
        Assert.AreEqual(1, _runEventHandler.TestResults.Count(t => t.Outcome == TestOutcome.Failed));

        // Release builds optimize code, hence line numbers are different.
        if (IntegrationTestEnvironment.BuildConfiguration.StartsWith("release", StringComparison.OrdinalIgnoreCase))
        {
            Assert.AreEqual(15, testCase.First().TestCase.LineNumber);
        }
        else
        {
            Assert.AreEqual(14, testCase.First().TestCase.LineNumber);
        }
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    [NetFullTargetFrameworkDataSource]
    public void RunTestsWithNonDllAdapter(RunnerInfo runnerInfo)
    {
        // This used to be test for Chutzpah, but it has long running problem with updating dependencies,
        // so we test against our own test framework, to ensure that we can run test files that are not using
        // *.dll as extension.
        SetTestEnvironment(_testEnvironment, runnerInfo);
        Setup();

        var jsSource = Path.Combine(_testEnvironment.TestAssetsPath, "test.js");
        var jsInTemp = TempDirectory.CopyFile(jsSource);

        var testAdapterPath = Directory.EnumerateFiles(GetTestAdapterPath(UnitTestFramework.NonDll), "*.TestAdapter.dll").ToList();
        _vstestConsoleWrapper.InitializeExtensions(new List<string>() { testAdapterPath.First() });

        _vstestConsoleWrapper.RunTests(
            new[] { jsInTemp },
            GetDefaultRunSettings(),
            _runEventHandler);

        var testCase = _runEventHandler.TestResults.Where(tr => tr.TestCase.DisplayName.Equals("TestMethod1"));

        // Assert
        Assert.AreEqual(1, _runEventHandler.TestResults.Count);
        Assert.AreEqual(1, _runEventHandler.TestResults.Count(t => t.Outcome == TestOutcome.Passed));
        Assert.AreEqual(0, _runEventHandler.TestResults.Count(t => t.Outcome == TestOutcome.Failed));
    }
}
