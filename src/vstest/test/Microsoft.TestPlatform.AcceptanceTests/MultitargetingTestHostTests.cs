// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
public class MultitargetingTestHostTests : AcceptanceTestBase
{
    [TestMethod]
    [TestCategory("Windows-Review")]
    // the underlying test is using xUnit to avoid AppDomain enhancements in MSTest that make this pass even without multitargetting
    // xUnit supports net452 onwards, so that is why this starts at net452, I also don't test all framework versions
    [NetCoreRunner(NETFX462_48)]
    [NetFrameworkRunner(NETFX462_48)]
    public void TestRunInATesthostThatTargetsTheirChosenNETFramework(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPath = BuildMultipleAssemblyPath("MultitargetedNetFrameworkProject.dll");
        var arguments = PrepareArguments(assemblyPath, null, null, FrameworkArgValue, runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);

        // Tell the test project which target framework we are expecting it to run as.
        // It has this value conditionally compiled, so it can compare it.
        var env = new Dictionary<string, string?>
        {
            ["EXPECTED_TARGET_FRAMEWORK"] = runnerInfo.TargetFramework
        };

        InvokeVsTest(arguments, env);

        ValidateSummaryStatus(passed: 1, failed: 0, 0);
    }
}
