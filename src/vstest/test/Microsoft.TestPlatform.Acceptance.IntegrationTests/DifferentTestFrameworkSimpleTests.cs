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
    [NetFullTargetFrameworkDataSource(inIsolation: true, inProcess: true)]
    [NetCoreTargetFrameworkDataSource]
    public void NUnitRunAllTestExecution(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var arguments = PrepareArguments(
            GetAssetFullPath("NUTestProject.dll"),
            null, // GetTestAdapterPath(UnitTestFramework.NUnit),
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
        var assemblyAbsolutePath = Path.Combine(_testEnvironment.GlobalPackageDirectory, assemblyRelativePath);
        var arguments = PrepareArguments(assemblyAbsolutePath, string.Empty, string.Empty, FrameworkArgValue, _testEnvironment.InIsolationValue, resultsDirectory: TempDirectory.Path);

        InvokeVsTest(arguments);
        ValidateSummaryStatus(1, 1, 0);
    }
}
