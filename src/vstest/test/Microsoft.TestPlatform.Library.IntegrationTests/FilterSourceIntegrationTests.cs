// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.Library.IntegrationTests;

/// <summary>
/// Integration tests for the <c>Microsoft.TestPlatform.Filter.Source</c> NuGet package.
/// The test asset <c>FilterSourcePackageConsumerTests</c> references the locally-built package,
/// which embeds the filter source files as <c>contentFiles</c>.  Running those tests through
/// vstest.console end-to-end verifies that the package compiles and executes correctly.
/// </summary>
[TestClass]
public class FilterSourceIntegrationTests : AcceptanceTestBase
{
    [TestMethod]
    [NetFullTargetFrameworkDataSource(useDesktopRunner: false)]
    [NetCoreTargetFrameworkDataSource(useDesktopRunner: false)]
    public void FilterSourcePackage_AllTestsPass(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var testAssembly = GetAssetFullPath("FilterSourcePackageConsumerTests.dll");
        var arguments = PrepareArguments(testAssembly, null, string.Empty, FrameworkArgValue,
            runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);

        InvokeVsTest(arguments);

        // 7 FilterExpressionWrapper tests + 7 TestCaseFilterExpression tests = 14 total
        ValidateSummaryStatus(14, 0, 0);
        ExitCodeEquals(0);
    }
}

