// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

# if NETFRAMEWORK
using System.Linq;

using FluentAssertions;
using FluentAssertions.Extensions;

using Microsoft.TestPlatform.AcceptanceTests.Performance.TranslationLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests.Performance;

/// <summary>
/// The performance tests.
/// </summary>
[TestClass]
[DoNotParallelize]
[Ignore("The timing can vary significantly based on the system running the test. Convert them to report the results and not fail.")]
public class PerformanceTests : TelemetryPerfTestBase
{
    [TestMethod]
    [DataRow("MSTest1Passing", 1, 500)]
    public void ExecutionPerformanceTest(string projectName, int expectedTestCount, int thresholdInMs)
    {
        RunExecutionPerformanceTests(GetPerfAssetFullPath(projectName).Single(), GetTestAdapterPath(), string.Empty);

        ValidateSummaryStatus(expectedTestCount, 0, 0);

        var actualExecutionTime = GetExecutionTime();

        actualExecutionTime.Should().BeLessThanOrEqualTo(thresholdInMs.Milliseconds());
    }

    [TestMethod]
    public void DiscoveryPerformanceTest()
    {
        RunDiscoveryPerformanceTests(GetSampleTestAssembly(), GetTestAdapterPath(), string.Empty);

        ValidateDiscoveredTests(
            "SampleUnitTestProject.UnitTest1.PassingTest",
            "SampleUnitTestProject.UnitTest1.FailingTest",
            "SampleUnitTestProject.UnitTest1.SkippingTest");

        var actualDiscoveryTime = GetDiscoveryTime();

        actualDiscoveryTime.Should().BeLessThanOrEqualTo(500.Milliseconds());
    }

    [TestMethod]
    public void VsTestConsolePerformanceTest()
    {
        RunExecutionPerformanceTests(GetSampleTestAssembly(), GetTestAdapterPath(), string.Empty);

        ValidateSummaryStatus(1, 1, 1);
        ValidatePassedTests("SampleUnitTestProject.UnitTest1.PassingTest");
        ValidateFailedTests("SampleUnitTestProject.UnitTest1.FailingTest");
        ValidateSkippedTests("SampleUnitTestProject.UnitTest1.SkippingTest");

        var actualVsTestTime = GetVsTestTime();

        actualVsTestTime.Should().BeLessThanOrEqualTo(2500.Milliseconds());
    }

    [TestMethod]
    public void TestHostPerformanceTest()
    {
        RunExecutionPerformanceTests(GetSampleTestAssembly(), GetTestAdapterPath(), string.Empty);

        ValidateSummaryStatus(1, 1, 1);
        ValidatePassedTests("SampleUnitTestProject.UnitTest1.PassingTest");
        ValidateFailedTests("SampleUnitTestProject.UnitTest1.FailingTest");
        ValidateSkippedTests("SampleUnitTestProject.UnitTest1.SkippingTest");

        var actualTestHostTime = GetTestHostTime();

        actualTestHostTime.Should().BeLessThanOrEqualTo(2000.Milliseconds());
    }

    [TestMethod]
    public void MsTestV2AdapterPerformanceTest()
    {
        RunExecutionPerformanceTests(GetSampleTestAssembly(), GetTestAdapterPath(), string.Empty);

        ValidateSummaryStatus(1, 1, 1);
        ValidatePassedTests("SampleUnitTestProject.UnitTest1.PassingTest");
        ValidateFailedTests("SampleUnitTestProject.UnitTest1.FailingTest");
        ValidateSkippedTests("SampleUnitTestProject.UnitTest1.SkippingTest");

        var actualAdapterTimeTaken = GetAdapterExecutionTime("executor://mstestadapter/v2");

        actualAdapterTimeTaken.Should().BeLessThanOrEqualTo(1500.Milliseconds());
    }
}
#endif
