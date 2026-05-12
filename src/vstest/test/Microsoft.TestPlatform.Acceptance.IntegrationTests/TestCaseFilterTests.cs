// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
public class TestCaseFilterTests : AcceptanceTestBase
{
    [TestMethod]
    [NetFullTargetFrameworkDataSourceAttribute(inIsolation: true, inProcess: true)]
    [NetCoreTargetFrameworkDataSource]
    public void RunSelectedTestsWithAndOperatorTrait(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var arguments = PrepareArguments(
            GetSampleTestAssembly(),
            GetTestAdapterPath(),
            string.Empty, FrameworkArgValue,
            runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, " /TestCaseFilter:\"(TestCategory=CategoryA&Priority=3)\"");
        InvokeVsTest(arguments);
        ValidateSummaryStatus(0, 1, 0);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void RunSelectedTestsWithCategoryTraitInMixCase(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var arguments = PrepareArguments(
            GetSampleTestAssembly(),
            GetTestAdapterPath(),
            string.Empty, FrameworkArgValue,
            runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, " /TestCaseFilter:\"TestCategory=Categorya\"");
        InvokeVsTest(arguments);
        ValidateSummaryStatus(0, 1, 0);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void RunSelectedTestsWithClassNameTrait(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var arguments = PrepareArguments(
            GetSampleTestAssembly(),
            GetTestAdapterPath(),
            string.Empty, FrameworkArgValue,
            runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, " /TestCaseFilter:\"ClassName=SampleUnitTestProject.UnitTest1\"");
        InvokeVsTest(arguments);
        ValidateSummaryStatus(1, 1, 1);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void RunSelectedTestsWithFullyQualifiedNameTrait(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var arguments = PrepareArguments(
            GetSampleTestAssembly(),
            GetTestAdapterPath(),
            string.Empty, FrameworkArgValue,
            runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(
            arguments,
            " /TestCaseFilter:\"FullyQualifiedName=SampleUnitTestProject.UnitTest1.FailingTest\"");
        InvokeVsTest(arguments);
        ValidateSummaryStatus(0, 1, 0);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void RunSelectedTestsWithNameTrait(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var arguments = PrepareArguments(
            GetSampleTestAssembly(),
            GetTestAdapterPath(),
            string.Empty, FrameworkArgValue,
            runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, " /TestCaseFilter:\"Name=PassingTest\"");
        InvokeVsTest(arguments);
        ValidateSummaryStatus(1, 0, 0);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void RunSelectedTestsWithOrOperatorTrait(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var arguments = PrepareArguments(
            GetSampleTestAssembly(),
            GetTestAdapterPath(),
            string.Empty, FrameworkArgValue,
            runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, " /TestCaseFilter:\"(TestCategory=CategoryA|Priority=2)\"");
        InvokeVsTest(arguments);
        ValidateSummaryStatus(1, 1, 0);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void RunSelectedTestsWithPriorityTrait(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var arguments = PrepareArguments(
            GetSampleTestAssembly(),
            GetTestAdapterPath(),
            string.Empty, FrameworkArgValue,
            runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, " /TestCaseFilter:\"Priority=2\"");
        InvokeVsTest(arguments);
        ValidateSummaryStatus(1, 0, 0);
    }

    /// <summary>
    /// In case TestCaseFilter is provide without any property like Name or ClassName. ex. /TestCaseFilter:"UnitTest1"
    /// this command should provide same results as /TestCaseFilter:"FullyQualifiedName~UnitTest1".
    /// </summary>
    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void TestCaseFilterShouldWorkIfOnlyPropertyValueGivenInExpression(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var arguments = PrepareArguments(
            _testEnvironment.GetTestAsset("SimpleTestProject2.dll"),
            GetTestAdapterPath(),
            string.Empty, FrameworkArgValue,
            runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, " /TestCaseFilter:UnitTest1");
        InvokeVsTest(arguments);
        ValidateSummaryStatus(1, 1, 1);
    }

    /// <summary>
    /// Discover tests using mstest v1 adapter with test case filters.
    /// </summary>
    [TestMethod]
    [TestCategory("Windows-Review")]
    // MSTest v1 tests from dlls are only supported in .NET Framework runner, in and outside of VS
    // via Microsoft.VisualStudio.TestPlatform.Extensions.VSTestIntegration.dll
    [NetFullTargetFrameworkDataSource(useCoreRunner: false)]
    public void DiscoverMstestV1TestsWithAndOperatorTrait(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var arguments = PrepareArguments(
            _testEnvironment.GetTestAsset("MstestV1UnitTestProject.dll"),
            GetTestAdapterPath(),
            string.Empty, FrameworkArgValue,
            runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, " /listtests /TestCaseFilter:\"(TestCategory!=CategoryA&Priority!=3)\"");

        InvokeVsTest(arguments);
        var listOfTests = new string[] {"MstestV1UnitTestProject.UnitTest1.PassingTest1", "MstestV1UnitTestProject.UnitTest1.PassingTest2",
                "MstestV1UnitTestProject.UnitTest1.FailingTest2", "MstestV1UnitTestProject.UnitTest1.SkippingTest" };
        var listOfNotDiscoveredTests = new string[] { "MstestV1UnitTestProject.UnitTest1.FailingTest1" };
        ValidateDiscoveredTests(listOfTests);
        ValidateTestsNotDiscovered(listOfNotDiscoveredTests);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSourceAttribute(inIsolation: true, inProcess: true)]
    [NetCoreTargetFrameworkDataSource]
    public void RunSelectedTestsWithNoneTestCategoryFilterMatchesUncategorizedTests(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var arguments = PrepareArguments(
            GetSampleTestAssembly(),
            GetTestAdapterPath(),
            string.Empty, FrameworkArgValue,
            runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);
        // "None" is a reserved keyword that matches tests without any TestCategory attribute.
        // In SimpleTestProject: PassingTest (no category) and SkippingTest (no category, ignored).
        // FailingTest has TestCategory("CategoryA") and should NOT be matched.
        arguments = string.Concat(arguments, " /TestCaseFilter:\"TestCategory=None\"");
        InvokeVsTest(arguments);
        ValidateSummaryStatus(1, 0, 1);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSourceAttribute(inIsolation: true, inProcess: true)]
    [NetCoreTargetFrameworkDataSource]
    public void RunSelectedTestsWithNoneTestCategoryNotEqualFilterMatchesCategorizedTests(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var arguments = PrepareArguments(
            GetSampleTestAssembly(),
            GetTestAdapterPath(),
            string.Empty, FrameworkArgValue,
            runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);
        // NotEqual to "None" should match tests WITH categories.
        // In SimpleTestProject: only FailingTest has TestCategory("CategoryA").
        arguments = string.Concat(arguments, " /TestCaseFilter:\"TestCategory!=None\"");
        InvokeVsTest(arguments);
        ValidateSummaryStatus(0, 1, 0);
    }

}
