// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.TestPlatform.TestUtilities;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.SmokeTests;

[TestClass]
public class ExecutionTests : IntegrationTestBase
{
    [TestMethod]
    public void RunAllTestExecution()
    {
        InvokeVsTestForExecution(GetSampleTestAssembly(), GetTestAdapterPath(), ".NETFramework,Version=v4.5.1");
        ValidateSummaryStatus(1, 1, 1);
        ValidatePassedTests("SampleUnitTestProject.UnitTest1.PassingTest");
        ValidateFailedTests("SampleUnitTestProject.UnitTest1.FailingTest");
        ValidateSkippedTests("SampleUnitTestProject.UnitTest1.SkippingTest");
    }

    [TestMethod]
    public void RunSelectedTests()
    {
        using var resultsDir = new TempDirectory();
        var arguments = PrepareArguments(GetSampleTestAssembly(), GetTestAdapterPath(), string.Empty, ".NETFramework,Version=v4.5.1", resultsDirectory: resultsDir.Path);
        arguments = string.Concat(arguments, " /Tests:PassingTest");
        InvokeVsTest(arguments);
        ValidateSummaryStatus(1, 0, 0);
        ValidatePassedTests("SampleUnitTestProject.UnitTest1.PassingTest");
    }
}
