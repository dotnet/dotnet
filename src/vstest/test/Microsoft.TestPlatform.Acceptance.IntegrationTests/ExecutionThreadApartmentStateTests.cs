// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
[TestCategory("Windows-Review")]
public class ExecutionThreadApartmentStateTests : AcceptanceTestBase
{
    [TestMethod]
    [NetFullTargetFrameworkDataSource(inIsolation: true, inProcess: true)]
    public void UITestShouldPassIfApartmentStateIsSTA(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPaths = GetAssetFullPath("SimpleTestProject3.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, " /testcasefilter:UITestMethod");
        InvokeVsTest(arguments);
        ValidateSummaryStatus(1, 0, 0);
    }

    [TestMethod]
    [NetCoreTargetFrameworkDataSource]
    public void WarningShouldBeShownWhenValueIsSTAForNetCore(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPaths =
            GetAssetFullPath("SimpleTestProject2.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, " /testcasefilter:PassingTest2 -- RunConfiguration.ExecutionThreadApartmentState=STA");
        InvokeVsTest(arguments);
        StdOutputContains("ExecutionThreadApartmentState option not supported for framework:");
        ValidateSummaryStatus(1, 0, 0);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource(inIsolation: true, inProcess: true)]
    public void UITestShouldFailWhenDefaultApartmentStateIsMTA(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPaths =
            GetAssetFullPath("SimpleTestProject3.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, " /testcasefilter:UITestMethod -- RunConfiguration.ExecutionThreadApartmentState=MTA");
        InvokeVsTest(arguments);
        ValidateSummaryStatus(0, 1, 0);
    }

    [Ignore(@"Issue with TestSessionTimeout:  https://github.com/Microsoft/vstest/issues/980")]
    [TestMethod]
    [NetFullTargetFrameworkDataSource(inIsolation: true, inProcess: true)]
    public void CancelTestExectionShouldWorkWhenApartmentStateIsSTA(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPaths =
            GetAssetFullPath("SimpleTestProject3.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, " /tests:UITestWithSleep1,UITestMethod -- RunConfiguration.ExecutionThreadApartmentState=STA RunConfiguration.TestSessionTimeout=2000");
        InvokeVsTest(arguments);
        StdOutputContains("Canceling test run: test run timeout of");
        ValidateSummaryStatus(1, 0, 0);
    }
}
