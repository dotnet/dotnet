// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
public class PortableNugetPackageTests : AcceptanceTestBase
{
    private static string s_portablePackageFolder = string.Empty;

    [ClassInitialize]
    public static void ClassInit(TestContext _)
    {
        s_portablePackageFolder = Path.Combine(IntegrationTestEnvironment.PublishDirectory, $"Microsoft.TestPlatform.Portable.{IntegrationTestEnvironment.LatestLocallyBuiltNugetVersion}.nupkg");
        if (!Directory.Exists(s_portablePackageFolder))
        {
            throw new DirectoryNotFoundException($"Cannot find '{s_portablePackageFolder}'.");
        }
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource(inIsolation: true, inProcess: true)]
    [NetCoreTargetFrameworkDataSource]
    public void RunMultipleTestAssemblies(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPaths = BuildMultipleAssemblyPath("SimpleTestProject.dll", "SimpleTestProject2.dll");

        InvokeVsTestForExecution(assemblyPaths, GetTestAdapterPath(), FrameworkArgValue, string.Empty);

        ValidateSummaryStatus(2, 2, 2);
        ExitCodeEquals(1); // failing tests
    }

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
}
