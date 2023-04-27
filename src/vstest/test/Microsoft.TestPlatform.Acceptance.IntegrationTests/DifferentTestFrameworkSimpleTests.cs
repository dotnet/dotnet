// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.IO;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
[TestCategory("Windows-Review")]
public class DifferentTestFrameworkSimpleTests : AcceptanceTestBase
{
    [TestMethod]
    [NetFullTargetFrameworkDataSource(inIsolation: true, inProcess: true)]
    public void NonDllRunAllTestExecution(RunnerInfo runnerInfo)
    {
        // This used to test Chutzpah, to prove that we can run tests that are not shipped in dlls.
        // But that framework is not fixing vulnerable dependencies for a long time, so we use our custom, test adapter
        // that simply returns 1 discovered test on discovery, and 1 passed test on execution.
        // We do not really test that we can run JavaScript tests, but we test that we can trigger tests that are not shipped
        // in a dll, and pick up the provided test adapter.
        SetTestEnvironment(_testEnvironment, runnerInfo);
        string fileName = "test.js";
        var testJSFileAbsolutePath = Path.Combine(_testEnvironment.TestAssetsPath, fileName);
        string tempPath = Path.Combine(TempDirectory.Path, fileName);
        File.Copy(testJSFileAbsolutePath, tempPath);
        var arguments = PrepareArguments(tempPath, GetTestAdapterPath(UnitTestFramework.NonDll), string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);
        InvokeVsTest(arguments);
        ValidateSummaryStatus(1, 0, 0);
    }

    [TestMethod]
    // vstest.console is x64 now, but x86 run "in process" run should still succeed by being run in x86 testhost
    // Skip .NET (Core) tests because we test them below.
    [NetFullTargetFrameworkDataSource(inIsolation: true, inProcess: true, useCoreRunner: false)]
    public void CPPRunAllTestExecutionNetFramework(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        CppRunAllTests("x86");
    }


    [TestMethod]
    [TestCategory("Windows-Review")]
    // vstest.console is 64-bit now, run in process to test the 64-bit native dll
    // Skip .NET (Core) tests because we test them below.
    [NetFullTargetFrameworkDataSource(inIsolation: true, inProcess: true, useCoreRunner: false)]
    public void CPPRunAllTestExecutionPlatformx64NetFramework(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        CppRunAllTests("x64");
    }

    [TestMethod]
    [Ignore("TODO: this does not work in already shipped dotnet test either, investigate")]
    // C++ tests cannot run in .NET Framework host under .NET Core, because we only ship .NET Standard CPP adapter in .NET Core
    // We also don't test x86 for .NET Core, because the resolver there does not switch between x86 and x64 correctly, it just uses the parent process bitness.
    // We run this on netcore31 and not the default netcore21 because netcore31 is the minimum tfm that has the runtime features we need, such as additionaldeps.
    [NetCoreTargetFrameworkDataSource(useDesktopRunner: false, useCoreRunner: true)]
    public void CPPRunAllTestExecutionPlatformx64Net(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        CppRunAllTests("x64");
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    [NetFullTargetFrameworkDataSource]
    [Ignore("After the bump of TestPlatformRemoteExternalsVersion to 17.6 it doesn't work anymore, it's a test for the legacy platform and fails for wrong Microsoft.VisualStudio.Telemetry version.")]
    public void WebTestRunAllTestsWithRunSettings(RunnerInfo runnerInfo)
    {
        if (!IsCI)
        {
            Assert.Inconclusive("This works on server but not locally, because locally it grabs old dll from GAC, but has version 10.0.0 as the one in our package.");
        }

        SetTestEnvironment(_testEnvironment, runnerInfo);
        var runSettingsFilePath = Path.Combine(TempDirectory.Path, Guid.NewGuid() + ".runsettings");

        //test the iterationCount setting for WebTestRunConfiguration in run settings
        var runSettingsXml = $@"<?xml version='1.0' encoding='utf-8'?>
                                <RunSettings>
                                    <WebTestRunConfiguration iterationCount='5' />
                                    <RunConfiguration>
                                        <TargetFrameworkVersion>{FrameworkArgValue}</TargetFrameworkVersion>
                                    </RunConfiguration>
                                </RunSettings>";

        CreateRunSettingsFile(runSettingsFilePath, runSettingsXml);

        //therefore, the test will run for 5 iterations resulting in web test result file size of at least 150 KB
        var minWebTestResultFileSizeInKB = 150;
        if (runnerInfo.IsNetRunner)
        {
            Assert.Inconclusive("WebTests tests not supported with .NET Core runner.");
            return;
        }

        string assemblyRelativePath =
            @"microsoft.testplatform.qtools.assets\2.0.0\contentFiles\any\any\WebTestAssets\WebTest1.webtest";

        var assemblyAbsolutePath = Path.Combine(_testEnvironment.PackageDirectory, assemblyRelativePath);
        using var resultsDirectory = TempDirectory;
        var arguments = PrepareArguments(
            assemblyAbsolutePath,
            string.Empty,
            runSettingsFilePath, FrameworkArgValue, string.Empty, resultsDirectory.Path);

        InvokeVsTest(arguments);
        ValidateSummaryStatus(1, 0, 0);

        if (minWebTestResultFileSizeInKB > 0)
        {
            var dirInfo = new DirectoryInfo(resultsDirectory.Path);
            var webtestResultFile = "WebTest1.webtestResult";
            var files = dirInfo.GetFiles(webtestResultFile, SearchOption.AllDirectories);
            Assert.IsTrue(files.Length > 0, $"File {webtestResultFile} not found under results directory {resultsDirectory}");

            var fileSizeInKB = files[0].Length / 1024;
            Assert.IsTrue(fileSizeInKB > minWebTestResultFileSizeInKB, $"Size of the file {webtestResultFile} is {fileSizeInKB} KB. It is not greater than {minWebTestResultFileSizeInKB} KB indicating iterationCount in run settings not honored.");
        }
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    [NetFullTargetFrameworkDataSource]
    [Ignore("Test expects .NETFramework,Version=v4.5.2 support but the minimum one is .NETFramework,Version=v4.6.2")]
    public void CodedWebTestRunAllTests(RunnerInfo runnerInfo)
    {
        if (!IsCI)
        {
            Assert.Inconclusive("This works on server but not locally, because locally it grabs old dll from GAC, but has version 10.0.0 as the one in our package.");
        }

        SetTestEnvironment(_testEnvironment, runnerInfo);
        if (runnerInfo.IsNetRunner)
        {
            Assert.Inconclusive("WebTests tests not supported with .NET Core runner.");
            return;
        }

        string assemblyRelativePath = @"microsoft.testplatform.qtools.assets\2.0.0\contentFiles\any\any\WebTestAssets\BingWebTest.dll";
        var assemblyAbsolutePath = Path.Combine(_testEnvironment.PackageDirectory, assemblyRelativePath);
        var arguments = PrepareArguments(
            assemblyAbsolutePath,
            string.Empty,
            string.Empty, FrameworkArgValue, resultsDirectory: TempDirectory.Path);

        InvokeVsTest(arguments);
        ValidateSummaryStatus(1, 0, 0);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource(inIsolation: true, inProcess: true)]
    [NetCoreTargetFrameworkDataSource]
    public void NUnitRunAllTestExecution(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var arguments = PrepareArguments(
            GetAssetFullPath("NUTestProject.dll"),
            GetTestAdapterPath(UnitTestFramework.NUnit),
            string.Empty, FrameworkArgValue,
            runnerInfo.InIsolationValue, TempDirectory.Path);
        InvokeVsTest(arguments);
        ValidateSummaryStatus(1, 1, 0);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource(inIsolation: true, inProcess: true)]
    [NetCoreTargetFrameworkDataSource]
    public void XUnitRunAllTestExecution(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        string testAssemblyPath = _testEnvironment.GetTestAsset("XUTestProject.dll");
        var arguments = PrepareArguments(
            testAssemblyPath,
            GetTestAdapterPath(UnitTestFramework.XUnit),
            string.Empty, FrameworkArgValue,
            runnerInfo.InIsolationValue, TempDirectory.Path);
        InvokeVsTest(arguments);
        ValidateSummaryStatus(1, 1, 0);
    }

    private void CppRunAllTests(string platform)
    {
        string assemblyRelativePathFormat = @"microsoft.testplatform.testasset.nativecpp\2.0.0\contentFiles\any\any\{0}\Microsoft.TestPlatform.TestAsset.NativeCPP.dll";
        var assemblyRelativePath = platform.Equals("x64", StringComparison.OrdinalIgnoreCase)
            ? string.Format(CultureInfo.CurrentCulture, assemblyRelativePathFormat, platform)
            : string.Format(CultureInfo.CurrentCulture, assemblyRelativePathFormat, "");
        var assemblyAbsolutePath = Path.Combine(_testEnvironment.PackageDirectory, assemblyRelativePath);
        var arguments = PrepareArguments(assemblyAbsolutePath, string.Empty, string.Empty, FrameworkArgValue, _testEnvironment.InIsolationValue, resultsDirectory: TempDirectory.Path);

        InvokeVsTest(arguments);
        ValidateSummaryStatus(1, 1, 0);
    }
}
