// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
public class DebugAssertTests : AcceptanceTestBase
{
    [TestMethod]
    // this is core only, there is nothing we can do about TPDebug.Assert crashing the process on framework
    [NetCoreTargetFrameworkDataSource(useDesktopRunner: false)]
    public void RunningTestWithAFailingDebugAssertDoesNotCrashTheHostingProcess(RunnerInfo runnerInfo)
    {
        // when debugging this test in case it starts failing, be aware that the default behavior of TPDebug.Assert
        // is to not crash the process when we are running in debug, and debugger is attached
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPath = GetAssetFullPath("CrashingOnDebugAssertTestProject.dll");
        var arguments = PrepareArguments(assemblyPath, null, null, FrameworkArgValue, runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);
        InvokeVsTest(arguments);

        // this will have failed tests when our trace listener works and crash the testhost process when it does not
        // because crashing processes is what a failed TPDebug.Assert does by default, unless you have a debugger attached
        ValidateSummaryStatus(passed: 4, failed: 4, 0);
        StringAssert.Contains(StdOut, "threw exception: Microsoft.VisualStudio.TestPlatform.TestHost.DebugAssertException:");
    }
}
