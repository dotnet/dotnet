// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
[TestCategory("Windows-Review")]
public class CuitTest : AcceptanceTestBase
{
    [TestMethod]
    [TestCategory("Windows-Review")]
    [NetFullTargetFrameworkDataSource]
    public void CuitRunAllTests(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        CuitRunAll(runnerInfo);
    }

    private void CuitRunAll(RunnerInfo runnerInfo)
    {
        if (runnerInfo.IsNetRunner)
        {
            Assert.Inconclusive("CUIT tests are not supported with .NET Core runner.");
            return;
        }

        var assemblyAbsolutePath = _testEnvironment.GetTestAsset("CUITTestProject.dll", "net462");
        var arguments = PrepareArguments(assemblyAbsolutePath, string.Empty, string.Empty, FrameworkArgValue, resultsDirectory: TempDirectory.Path);
        arguments += " -- RunConfiguration.TargetPlatform=x86";

        InvokeVsTest(arguments);
        ValidateSummaryStatus(1, 0, 0);
    }
}
