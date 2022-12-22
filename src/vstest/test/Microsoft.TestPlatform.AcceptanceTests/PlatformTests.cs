// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
// monitoring the processes does not work correctly
[TestCategory("Windows-Review")]
public class PlatformTests : AcceptanceTestBase
{
    /// <summary>
    /// The run test execution with platform x64.
    /// </summary>
    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void RunTestExecutionWithPlatformx64(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var platformArg = " /Platform:x64";
        RunTestExecutionWithPlatform(platformArg, "testhost", 1);
    }

    /// <summary>
    /// The run test execution with platform x86.
    /// </summary>
    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void RunTestExecutionWithPlatformx86(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var platformArg = " /Platform:x86";
        RunTestExecutionWithPlatform(platformArg, "testhost.x86", 1);
    }

    private void RunTestExecutionWithPlatform(string platformArg, string testhostProcessName, int expectedNumOfProcessCreated)
    {

        var arguments = PrepareArguments(
            GetSampleTestAssembly(),
            GetTestAdapterPath(),
            string.Empty, FrameworkArgValue,
            _testEnvironment.InIsolationValue, resultsDirectory: TempDirectory.Path);

        arguments = string.Concat(arguments, platformArg, GetDiagArg(TempDirectory.Path));
        InvokeVsTest(arguments);

        AssertExpectedNumberOfHostProcesses(expectedNumOfProcessCreated, TempDirectory.Path, new[] { testhostProcessName }, arguments, GetConsoleRunnerPath());
        ValidateSummaryStatus(1, 1, 1);
    }
}
