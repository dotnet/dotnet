// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
public class DiscoveryTests : AcceptanceTestBase
{
    [TestMethod]
    [NetFullTargetFrameworkDataSource(inIsolation: true, inProcess: true)]
    [NetCoreTargetFrameworkDataSource]
    public void DiscoverAllTests(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        InvokeVsTestForDiscovery(GetSampleTestAssembly(), GetTestAdapterPath(), string.Empty, FrameworkArgValue);

        var listOfTests = new[] { "SampleUnitTestProject.UnitTest1.PassingTest", "SampleUnitTestProject.UnitTest1.FailingTest", "SampleUnitTestProject.UnitTest1.SkippingTest" };
        ValidateDiscoveredTests(listOfTests);
        ExitCodeEquals(0);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource(inIsolation: true, inProcess: true)]
    [NetCoreTargetFrameworkDataSource]
    public void MultipleSourcesDiscoverAllTests(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        var assemblyPaths = BuildMultipleAssemblyPath("SimpleTestProject.dll", "SimpleTestProject2.dll");
        var listOfTests = new[] {
            "SampleUnitTestProject.UnitTest1.PassingTest",
            "SampleUnitTestProject.UnitTest1.FailingTest",
            "SampleUnitTestProject.UnitTest1.SkippingTest",
            "SampleUnitTestProject.UnitTest1.PassingTest2",
            "SampleUnitTestProject.UnitTest1.FailingTest2",
            "SampleUnitTestProject.UnitTest1.SkippingTest2"
        };

        InvokeVsTestForDiscovery(assemblyPaths, GetTestAdapterPath(), string.Empty, FrameworkArgValue);

        ValidateDiscoveredTests(listOfTests);
        ExitCodeEquals(0);
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    [NetFullTargetFrameworkDataSource(inIsolation: true, inProcess: true)]
    public void DiscoverFullyQualifiedTests(RunnerInfo runnerInfo)
    {
        var dummyFilePath = Path.Combine(TempDirectory.Path, $"{Guid.NewGuid()}.txt");

        SetTestEnvironment(_testEnvironment, runnerInfo);

        var listOfTests = new[] { "SampleUnitTestProject.UnitTest1.PassingTest", "SampleUnitTestProject.UnitTest1.FailingTest", "SampleUnitTestProject.UnitTest1.SkippingTest" };

        var arguments = PrepareArguments(GetSampleTestAssembly(), GetTestAdapterPath(), string.Empty, FrameworkArgValue, _testEnvironment.InIsolationValue, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, " /ListFullyQualifiedTests", " /ListTestsTargetPath:\"" + dummyFilePath + "\"");
        InvokeVsTest(arguments);

        ValidateFullyQualifiedDiscoveredTests(dummyFilePath, listOfTests);
        ExitCodeEquals(0);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void DiscoverTestsShouldShowProperWarningIfNoTestsOnTestCaseFilter(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assetFullPath = GetAssetFullPath("SimpleTestProject2.dll");
        var arguments = PrepareArguments(assetFullPath, GetTestAdapterPath(), string.Empty, FrameworkArgValue, _testEnvironment.InIsolationValue, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, " /listtests");
        arguments = string.Concat(arguments, " /testcasefilter:NonExistTestCaseName");
        arguments = string.Concat(arguments, " /logger:\"console;prefix=true\"");
        InvokeVsTest(arguments);

        StringAssert.Contains(StdOut, "Warning: No test matches the given testcase filter `NonExistTestCaseName` in");
        StringAssert.Contains(StdOut, "SimpleTestProject2.dll");
        ExitCodeEquals(0);
    }

    [TestMethod]
    public void TypesToLoadAttributeTests()
    {
        var extensionsDirectory = IntegrationTestEnvironment.ExtensionsDirectory;
        var extensionsToVerify = new Dictionary<string, string[]>
        {
            {"Microsoft.TestPlatform.Extensions.EventLogCollector.dll", new[] { "Microsoft.TestPlatform.Extensions.EventLogCollector.EventLogDataCollector"} },
            {"Microsoft.TestPlatform.Extensions.BlameDataCollector.dll", new[] { "Microsoft.TestPlatform.Extensions.BlameDataCollector.BlameLogger", "Microsoft.TestPlatform.Extensions.BlameDataCollector.BlameCollector" } },
            {"Microsoft.VisualStudio.TestPlatform.Extensions.Html.TestLogger.dll", new[] { "Microsoft.VisualStudio.TestPlatform.Extensions.HtmlLogger.HtmlLogger" } },
            {"Microsoft.VisualStudio.TestPlatform.Extensions.Trx.TestLogger.dll", new[] { "Microsoft.VisualStudio.TestPlatform.Extensions.TrxLogger.TrxLogger" } },
            {"Microsoft.TestPlatform.TestHostRuntimeProvider.dll", new[] { "Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Hosting.DefaultTestHostManager", "Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Hosting.DotnetTestHostManager" } }
        };

        foreach (var extension in extensionsToVerify.Keys)
        {
            var assemblyFile = Path.Combine(extensionsDirectory, extension);
            var assembly = Assembly.LoadFrom(assemblyFile);

            var expected = extensionsToVerify[extension];
            var actual = TypesToLoadUtilities.GetTypesToLoad(assembly).Select(i => i.FullName).ToArray();

            CollectionAssert.AreEquivalent(expected, actual, $"Specified types using TypesToLoadAttribute in \"{extension}\" assembly doesn't match the expected.");
        }
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    [NetFullTargetFrameworkDataSource(inIsolation: true, inProcess: true)]
    public void DiscoverTestsShouldSucceedWhenAtLeastOneDllFindsRuntimeProvider(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var testDll = GetAssetFullPath("MSTestProject1.dll");
        var nonTestDll = GetTestDllForFramework("NetStandard2Library.dll", "netstandard2.0");

        var testAndNonTestDll = new[] { testDll, nonTestDll };
        var quotedDlls = string.Join(" ", testAndNonTestDll.Select(a => a.AddDoubleQuote()));

        var arguments = PrepareArguments(quotedDlls, GetTestAdapterPath(), string.Empty, framework: string.Empty, _testEnvironment.InIsolationValue, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, " /listtests");
        arguments = string.Concat(arguments, " /logger:\"console;prefix=true\"");
        InvokeVsTest(arguments);

        StringAssert.Contains(StdOut, $"Skipping source: {nonTestDll} (.NETStandard,Version=v2.0,");

        ExitCodeEquals(0);
    }
}
