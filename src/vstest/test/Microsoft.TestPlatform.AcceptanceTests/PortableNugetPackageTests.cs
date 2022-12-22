// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.IO.Compression;
using System.Linq;

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
        var packageLocation = Path.Combine(IntegrationTestEnvironment.TestPlatformRootDirectory, "artifacts", IntegrationTestEnvironment.BuildConfiguration, "packages");
        var nugetPackage = Directory.EnumerateFiles(packageLocation, "Microsoft.TestPlatform.Portable.*.nupkg").ToList();
        s_portablePackageFolder = Path.Combine(packageLocation, Path.GetFileNameWithoutExtension(nugetPackage[0]));
        if (Directory.Exists(s_portablePackageFolder))
        {
            Directory.Delete(s_portablePackageFolder, recursive: true);
        }
        ZipFile.ExtractToDirectory(nugetPackage[0], s_portablePackageFolder);
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        Directory.Delete(s_portablePackageFolder, true);
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
