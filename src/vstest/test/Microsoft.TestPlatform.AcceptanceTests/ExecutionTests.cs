// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Extensions;

using TestPlatform.TestUtilities;

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

        var assemblyPaths = BuildMultipleAssemblyPath("SimpleTestProject.dll");
        var xunitAssemblyPath = _testEnvironment.GetTestAsset("XUTestProject.dll");

        assemblyPaths = string.Join(" ", assemblyPaths, xunitAssemblyPath.AddDoubleQuote());
        InvokeVsTestForExecution(assemblyPaths, testAdapterPath: string.Empty, FrameworkArgValue, string.Empty);

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
            BuildMultipleAssemblyPath("SimpleTestProject3.dll");
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
    [NetCoreTargetFrameworkDataSource]
    public void TestPlatformShouldBeCompatibleWithOldTestHost(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPaths = BuildMultipleAssemblyPath("SampleProjectWithOldTestHost.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);

        InvokeVsTest(arguments);

        ValidateSummaryStatus(1, 0, 0);
        ExitCodeEquals(0);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource(inIsolation: true, inProcess: true)]
    [NetCoreTargetFrameworkDataSource]
    public void WorkingDirectoryIsSourceDirectory(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPaths = BuildMultipleAssemblyPath("SimpleTestProject3.dll");
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

        var assemblyPaths = BuildMultipleAssemblyPath("SimpleTestProject3.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, " /testcasefilter:ExitWithStackoverFlow");
        arguments = string.Concat(arguments, $" /diag:{diagLogFilePath}");

        InvokeVsTest(arguments);

        var errorMessage = "Process is terminated due to StackOverflowException.";
        if (runnerInfo.TargetFramework.StartsWith("netcoreapp"))
        {
            errorMessage = "Process is terminating due to StackOverflowException.";
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
            BuildMultipleAssemblyPath("SimpleTestProject3.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, " /testcasefilter:ExitwithUnhandleException");
        arguments = string.Concat(arguments, $" /diag:{diagLogFilePath}");

        InvokeVsTest(arguments);

        var errorFirstLine =
            runnerInfo.TargetFramework.StartsWith("netcoreapp")
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
            BuildMultipleAssemblyPath("SimpleTestProjectx86.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);

        InvokeVsTest(arguments);

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
            BuildMultipleAssemblyPath("SimpleTestProject2.dll");
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

        var assemblyPaths = BuildMultipleAssemblyPath("SimpleTestProject2.dll");

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

        var assemblyPaths = BuildMultipleAssemblyPath("SimpleTestProject2.dll");
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

        var assemblyPaths = BuildMultipleAssemblyPath("SimpleTestProject2.dll");

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

        var assemblyPaths = BuildMultipleAssemblyPath("SimpleTestProject2.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);

        arguments = string.Concat(arguments, " -- RunConfiguration.TreatNoTestsAsError=false");
        InvokeVsTest(arguments);

        // Returning 1 because of failing test in test assembly (SimpleTestProject2.dll)
        ExitCodeEquals(1);
    }
}
