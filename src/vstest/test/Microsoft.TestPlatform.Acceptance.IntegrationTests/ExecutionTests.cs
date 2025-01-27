// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Extensions;

using TestPlatform.TestUtilities;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.Common;
using FluentAssertions;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
public class ExecutionTests : AcceptanceTestBase
{
    //TODO: It looks like the first 3 tests would be useful to multiply by all 3 test frameworks, should we make the test even more generic, or duplicate them?
    [TestMethod]
    [TestCategory("Windows-Review")]
    [MSTestCompatibilityDataSource(InProcess = true)]
    public void RunMultipleTestAssemblies(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPaths = BuildMultipleAssemblyPath("MSTestProject1.dll", "MSTestProject2.dll");

        InvokeVsTestForExecution(assemblyPaths, testAdapterPath: null, FrameworkArgValue, string.Empty);

        ValidateSummaryStatus(2, 2, 2);
        ExitCodeEquals(1); // failing tests
        StdErrHasTestRunFailedMessageButNoOtherError();
        StdOutHasNoWarnings();
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    [TestPlatformCompatibilityDataSource]
    public void RunTestsFromMultipleMSTestAssemblies(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPaths = BuildMultipleAssemblyPath("MSTestProject1.dll", "MSTestProject2.dll");

        InvokeVsTestForExecution(assemblyPaths, testAdapterPath: null, FrameworkArgValue, string.Empty);

        ValidateSummaryStatus(passed: 2, failed: 2, skipped: 2);
        ExitCodeEquals(1); // failing tests
        StdErrHasTestRunFailedMessageButNoOtherError();
        StdOutHasNoWarnings();
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    [TestHostCompatibilityDataSource]
    public void RunMultipleMSTestAssembliesOnVstestConsoleAndTesthostCombinations(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPaths = BuildMultipleAssemblyPath("MSTestProject1.dll", "MSTestProject2.dll");

        InvokeVsTestForExecution(assemblyPaths, testAdapterPath: null, FrameworkArgValue, string.Empty);

        ValidateSummaryStatus(2, 2, 2);
        ExitCodeEquals(1); // failing tests
        StdErrHasTestRunFailedMessageButNoOtherError();
        StdOutHasNoWarnings();
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    [RunnerCompatibilityDataSource]
    public void RunMultipleMSTestAssembliesOnVstestConsoleAndTesthostCombinations2(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPaths = BuildMultipleAssemblyPath("MSTestProject1.dll", "MSTestProject2.dll");

        InvokeVsTestForExecution(assemblyPaths, testAdapterPath: null, FrameworkArgValue, string.Empty);

        InvokeVsTestForExecution(assemblyPaths, testAdapterPath: null, FrameworkArgValue, string.Empty);

        ValidateSummaryStatus(2, 2, 2);
        ExitCodeEquals(1); // failing tests
    }

    // TODO: This one mixes different frameworks, I can make it work, but it is worth it? We are going to test
    // the two respective versions together (e.g. latest xunit and latest mstest), but does using two different test
    // frameworks have any added value over using 2 mstest dlls?
    [TestMethod]
    [NetFullTargetFrameworkDataSource(inIsolation: true, inProcess: true)]
    [NetCoreTargetFrameworkDataSource]
    public void RunMultipleTestAssembliesWithoutTestAdapterPath(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPath = GetAssetFullPath("SimpleTestProject.dll");
        var xunitAssemblyPath = _testEnvironment.GetTestAsset("XUTestProject.dll");

        assemblyPath = string.Join(" ", assemblyPath.AddDoubleQuote(), xunitAssemblyPath.AddDoubleQuote());
        InvokeVsTestForExecution(assemblyPath, testAdapterPath: string.Empty, FrameworkArgValue, string.Empty);

        ValidateSummaryStatus(2, 2, 1);
        ExitCodeEquals(1); // failing tests
    }

    // We cannot run this test on Mac/Linux because we're trying to switch the arch between x64 and x86
    // and after --arch feature implementation we won't find correct muxer on CI.
    [TestCategory("Windows")]
    [TestMethod]
    [MSTestCompatibilityDataSource]
    public void RunMultipleTestAssembliesInParallel(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPaths = BuildMultipleAssemblyPath("MSTestProject1.dll", "MSTestProject2.dll");
        var arguments = PrepareArguments(assemblyPaths, testAdapterPath: null, runSettings: null, FrameworkArgValue, runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, " /Parallel");
        arguments = string.Concat(arguments, " /Platform:x86");
        arguments += GetDiagArg(TempDirectory.Path);

        // for the desktop we will run testhost.x86 in two copies, but for core
        // we will run a combination of testhost.x86 and dotnet, where the dotnet will be
        // the test console, and sometimes it will be the test host (e.g dotnet, dotnet, testhost.x86, or dotnet, testhost.x86, testhost.x86)
        // based on the target framework
        int expectedNumOfProcessCreated = 2;
        var testHostProcessNames = IsDesktopTargetFramework()
            ? new[] { "testhost.x86", }
            : new[] { "testhost.x86", "testhost", };

        InvokeVsTest(arguments);

        AssertExpectedNumberOfHostProcesses(expectedNumOfProcessCreated, TempDirectory.Path, testHostProcessNames);
        ValidateSummaryStatus(2, 2, 2);
        ExitCodeEquals(1); // failing tests
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource(inIsolation: true, inProcess: true)]
    [NetCoreTargetFrameworkDataSource]
    public void TestSessionTimeOutTests(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPaths =
            GetAssetFullPath("SimpleTestProject3.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, " /TestCaseFilter:TestSessionTimeoutTest");

        // set TestSessionTimeOut = 7 sec
        arguments = string.Concat(arguments, " -- RunConfiguration.TestSessionTimeout=7000");
        InvokeVsTest(arguments);

        ExitCodeEquals(1);
        StdErrorContains("Test Run Aborted.");
        StdErrorContains("Aborting test run: test run timeout of 7000 milliseconds exceeded.");
        StdOutputDoesNotContains("Total tests: 6");
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource(inIsolation: true, inProcess: true)]
    [NetCoreTargetFrameworkDataSource]
    public void WorkingDirectoryIsSourceDirectory(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPaths = GetAssetFullPath("SimpleTestProject3.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, " /tests:WorkingDirectoryTest");

        InvokeVsTest(arguments);

        ValidateSummaryStatus(1, 0, 0);
        ExitCodeEquals(0);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void StackOverflowExceptionShouldBeLoggedToConsoleAndDiagLogFile(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        if (IntegrationTestEnvironment.BuildConfiguration.Equals("release", StringComparison.OrdinalIgnoreCase))
        {
            // On release, x64 builds, recursive calls may be replaced with loops (tail call optimization)
            Assert.Inconclusive("On StackOverflowException testhost not exited in release configuration.");
            return;
        }

        var diagLogFilePath = Path.Combine(TempDirectory.Path, $"std_error_log_{Guid.NewGuid()}.txt");
        File.Delete(diagLogFilePath);

        var assemblyPaths = GetAssetFullPath("SimpleTestProject3.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, " /testcasefilter:ExitWithStackoverFlow");
        arguments = string.Concat(arguments, $" /diag:{diagLogFilePath}");

        InvokeVsTest(arguments);

        var errorMessage = "Process is terminated due to StackOverflowException.";
        if (!runnerInfo.TargetFramework.StartsWith("net4"))
        {
            errorMessage = "Test host process crashed : Stack overflow.";
        }

        ExitCodeEquals(1);
        FileAssert.Contains(diagLogFilePath, errorMessage);
        StdErrorContains(errorMessage);
        File.Delete(diagLogFilePath);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void UnhandleExceptionExceptionShouldBeLoggedToDiagLogFile(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var diagLogFilePath = Path.Combine(TempDirectory.Path, $"std_error_log_{Guid.NewGuid()}.txt");
        File.Delete(diagLogFilePath);

        var assemblyPaths =
            GetAssetFullPath("SimpleTestProject3.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, " /testcasefilter:ExitwithUnhandleException");
        arguments = string.Concat(arguments, $" /diag:{diagLogFilePath}");

        InvokeVsTest(arguments);

        var errorFirstLine =
            !runnerInfo.TargetFramework.StartsWith("net4")
            ? "Test host standard error line: Unhandled exception. System.InvalidOperationException: Operation is not valid due to the current state of the object."
            : "Test host standard error line: Unhandled Exception: System.InvalidOperationException: Operation is not valid due to the current state of the object.";
        FileAssert.Contains(diagLogFilePath, errorFirstLine);
        File.Delete(diagLogFilePath);
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    [NetFullTargetFrameworkDataSource]
    public void IncompatibleSourcesWarningShouldBeDisplayedInTheConsoleWhenGivenIncompatibleX86andX64Dll(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var expectedWarningContains = @"Following DLL(s) do not match current settings, which are .NETFramework,Version=v4.6.2 framework and X64 platform. SimpleTestProjectx86.dll would use Framework .NETFramework,Version=v4.6.2 and Platform X86";
        var assemblyPaths =
            BuildMultipleAssemblyPath("SimpleTestProject3.dll", "SimpleTestProjectx86.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, " /testcasefilter:PassingTestx86");

        InvokeVsTest(arguments);

        ValidateSummaryStatus(1, 0, 0);
        ExitCodeEquals(0);

        // When both x64 & x86 DLL is passed x64 dll will be ignored.
        StdOutputContains(expectedWarningContains);
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    [NetFullTargetFrameworkDataSource]
    public void NoIncompatibleSourcesWarningShouldBeDisplayedInTheConsoleWhenGivenSingleX86Dll(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var expectedWarningContains = @"Following DLL(s) do not match current settings, which are .NETFramework,Version=v4.6.2 framework and X86 platform. SimpleTestProjectx86 would use Framework .NETFramework,Version=v4.6.2 and Platform X86";
        var assemblyPaths =
            GetAssetFullPath("SimpleTestProjectx86.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);

        InvokeVsTest(arguments + " /diag:logs\\");

        ValidateSummaryStatus(1, 0, 0);
        ExitCodeEquals(0);

        StdOutputDoesNotContains(expectedWarningContains);
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    [NetFullTargetFrameworkDataSource]
    public void IncompatibleSourcesWarningShouldBeDisplayedInTheConsoleOnlyWhenRunningIn32BitOS(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var expectedWarningContains = @"Following DLL(s) do not match current settings, which are .NETFramework,Version=v4.6.2 framework and X86 platform. SimpleTestProject2.dll would use Framework .NETFramework,Version=v4.6.2 and Platform X64";
        var assemblyPaths =
            GetAssetFullPath("SimpleTestProject2.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);

        InvokeVsTest(arguments);

        ValidateSummaryStatus(1, 1, 1);
        ExitCodeEquals(1);

        // If we are running this test on 64 bit OS, it should not output any warning
        if (Environment.Is64BitOperatingSystem)
        {
            StdOutputDoesNotContains(expectedWarningContains);
        }

        // If we are running this test on 32 bit OS, it should output warning message
        else
        {
            StdOutputContains(expectedWarningContains);
        }
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    [NetFullTargetFrameworkDataSource]
    public void ExitCodeShouldReturnOneWhenTreatNoTestsAsErrorParameterSetToTrueAndNoTestMatchesFilter(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPaths = GetAssetFullPath("SimpleTestProject2.dll");

        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);

        // Setting /TestCaseFilter to the test name, which does not exists in the assembly, so we will have 0 tests executed
        arguments = string.Concat(arguments, " /TestCaseFilter:TestNameThatMatchesNoTestInTheAssembly");

        arguments = string.Concat(arguments, " -- RunConfiguration.TreatNoTestsAsError=true");
        InvokeVsTest(arguments);

        ExitCodeEquals(1);
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    [NetFullTargetFrameworkDataSource]
    public void ExitCodeShouldReturnZeroWhenTreatNoTestsAsErrorParameterSetToFalseAndNoTestMatchesFilter(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPaths = GetAssetFullPath("SimpleTestProject2.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);

        // Setting /TestCaseFilter to the test name, which does not exists in the assembly, so we will have 0 tests executed
        arguments = string.Concat(arguments, " /TestCaseFilter:TestNameThatMatchesNoTestInTheAssembly");

        arguments = string.Concat(arguments, " -- RunConfiguration.TreatNoTestsAsError=false");
        InvokeVsTest(arguments);

        ExitCodeEquals(0);
    }

    [TestMethod]
    [TestCategory("Windows")]
    [NetFullTargetFrameworkDataSource]
    public void ExitCodeShouldNotDependOnTreatNoTestsAsErrorTrueValueWhenThereAreAnyTestsToRun(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPaths = GetAssetFullPath("SimpleTestProject2.dll");

        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);

        arguments = string.Concat(arguments, " -- RunConfiguration.TreatNoTestsAsError=true");
        InvokeVsTest(arguments);

        // Returning 1 because of failing test in test assembly (SimpleTestProject2.dll)
        ExitCodeEquals(1);
    }

    [TestMethod]
    [TestCategory("Windows")]
    [NetFullTargetFrameworkDataSource]
    public void ExitCodeShouldNotDependOnFailTreatNoTestsAsErrorFalseValueWhenThereAreAnyTestsToRun(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPaths = GetAssetFullPath("SimpleTestProject2.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);

        arguments = string.Concat(arguments, " -- RunConfiguration.TreatNoTestsAsError=false");
        InvokeVsTest(arguments);

        // Returning 1 because of failing test in test assembly (SimpleTestProject2.dll)
        ExitCodeEquals(1);
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    [NetFullTargetFrameworkDataSource(inIsolation: true, inProcess: true)]
    public void ExecuteTestsShouldSucceedWhenAtLeastOneDllFindsRuntimeProvider(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var testDll = GetAssetFullPath("MSTestProject1.dll");
        var nonTestDll = GetTestDllForFramework("NetStandard2Library.dll", "netstandard2.0");

        var testAndNonTestDll = new[] { testDll, nonTestDll };
        var quotedDlls = string.Join(" ", testAndNonTestDll.Select(a => a.AddDoubleQuote()));

        var arguments = PrepareArguments(quotedDlls, GetTestAdapterPath(), string.Empty, framework: string.Empty, _testEnvironment.InIsolationValue, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, " /logger:\"console;prefix=true\"");
        InvokeVsTest(arguments);

        StringAssert.Contains(StdOut, $"Skipping source: {nonTestDll} (.NETStandard,Version=v2.0,");

        ExitCodeEquals(1);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource(inIsolation: true, inProcess: true)]
    [NetCoreTargetFrameworkDataSource]
    public void RunXunitTestsWhenProvidingAllDllsInBin(RunnerInfo runnerInfo)
    {
        // This is the default filter of AzDo VSTest task:
        //     testAssemblyVer2: |
        //       **\*test *.dll
        //       ! * *\*TestAdapter.dll
        //       ! * *\obj\**
        // Because of this in typical run we get a lot of dlls that we are sure don't have tests, like Microsoft.VisualStudio.TestPlatform.MSTestAdapter.PlatformServices.dll
        // or testhost.dll. Those dlls are built for net8.0 tfm, so theoretically they should be tests, but attempting to run them fails to find runtimeconfig.json
        // or deps.json, and fails the run.
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var testAssemblyPath = _testEnvironment.GetTestAsset("XUTestProject.dll");
        var allDllsMatchingTestPattern = Directory.GetFiles(Path.GetDirectoryName(testAssemblyPath)!, "*test*.dll");

        string assemblyPaths = string.Join(" ", allDllsMatchingTestPattern.Concat(new[] { testAssemblyPath }).Select(s => s.AddDoubleQuote()));
        InvokeVsTestForExecution(assemblyPaths, testAdapterPath: string.Empty, FrameworkArgValue, string.Empty);
        var fails = StdErrWithWhiteSpace.Split('\n').Where(s => !s.IsNullOrWhiteSpace()).Select(s => s.Trim()).ToList();
        fails.Should().HaveCount(2, "because there is 1 failed test, and one message that tests failed.");
        fails.Last().Should().Be("Test Run Failed.");
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource(inIsolation: true, inProcess: true)]
    [NetCoreTargetFrameworkDataSource()]
    public void RunMstestTestsWhenProvidingAllDllsInBin(RunnerInfo runnerInfo)
    {
        // This is the default filter of AzDo VSTest task:
        //     testAssemblyVer2: |
        //       **\*test *.dll
        //       ! * *\*TestAdapter.dll
        //       ! * *\obj\**
        // Because of this in typical run we get a lot of dlls that we are sure don't have tests, like Microsoft.VisualStudio.TestPlatform.MSTestAdapter.PlatformServices.dll
        // or testhost.dll. Those dlls are built for net8.0 tfm, so theoretically they should be tests, but attempting to run them fails to find runtimeconfig.json
        // or deps.json, and fails the run.
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var testAssemblyPath = _testEnvironment.GetTestAsset("SimpleTestProject.dll");
        var allDllsMatchingTestPattern = Directory.GetFiles(Path.GetDirectoryName(testAssemblyPath)!, "*test*.dll");

        string assemblyPaths = string.Join(" ", allDllsMatchingTestPattern.Concat(new[] { testAssemblyPath }).Select(s => s.AddDoubleQuote()));
        InvokeVsTestForExecution(assemblyPaths, testAdapterPath: string.Empty, FrameworkArgValue, string.Empty);

        StdErrHasTestRunFailedMessageButNoOtherError();
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource(inIsolation: true, inProcess: true)]
    [NetCoreTargetFrameworkDataSource()]
    public void RunNunitTestsWhenProvidingAllDllsInBin(RunnerInfo runnerInfo)
    {
        // This is the default filter of AzDo VSTest task:
        //     testAssemblyVer2: |
        //       **\*test *.dll
        //       ! * *\*TestAdapter.dll
        //       ! * *\obj\**
        // Because of this in typical run we get a lot of dlls that we are sure don't have tests, like Microsoft.VisualStudio.TestPlatform.MSTestAdapter.PlatformServices.dll
        // or testhost.dll. Those dlls are built for net8.0 tfm, so theoretically they should be tests, but attempting to run them fails to find runtimeconfig.json
        // or deps.json, and fails the run.
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var testAssemblyPath = _testEnvironment.GetTestAsset("NUTestProject.dll");
        var allDllsMatchingTestPattern = Directory.GetFiles(Path.GetDirectoryName(testAssemblyPath)!, "*test*.dll");

        string assemblyPaths = string.Join(" ", allDllsMatchingTestPattern.Concat(new[] { testAssemblyPath }).Select(s => s.AddDoubleQuote()));
        InvokeVsTestForExecution(assemblyPaths, testAdapterPath: string.Empty, FrameworkArgValue, string.Empty);

        StdErrHasTestRunFailedMessageButNoOtherError();
    }

    [TestMethod]
    [NetCoreTargetFrameworkDataSource(useDesktopRunner: false)]
    public void RunTestsWhenProvidingJustPlatformDllsFailsTheRun(RunnerInfo runnerInfo)
    {
        // This is the default filter of AzDo VSTest task:
        //     testAssemblyVer2: |
        //       **\*test *.dll
        //       ! * *\*TestAdapter.dll
        //       ! * *\obj\**
        // Because of this in typical run we get a lot of dlls that we are sure don't have tests, like Microsoft.VisualStudio.TestPlatform.MSTestAdapter.PlatformServices.dll
        // or testhost.dll. Those dlls are built for net8.0 tfm, so theoretically they should be tests, but attempting to run them fails to find runtimeconfig.json
        // or deps.json, and fails the run.
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var testAssemblyPath = _testEnvironment.GetTestAsset("SimpleTestProject.dll");
        var allDllsMatchingTestPattern = Directory.GetFiles(Path.GetDirectoryName(testAssemblyPath)!, "*test*.dll").Where(f => !f.EndsWith("SimpleTestProject.dll"));

        string assemblyPaths = string.Join(" ", allDllsMatchingTestPattern.Select(s => s.AddDoubleQuote()));
        InvokeVsTestForExecution(assemblyPaths, testAdapterPath: string.Empty, FrameworkArgValue, string.Empty);

        StdErr.Should().Be("No test source files were specified. ", "because all platform files we provided were filtered out");
    }
}
