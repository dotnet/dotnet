// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
public class RecursiveResourcesLookupTests : AcceptanceTestBase
{
    [TestMethod]
    // This only fails on .NET Framework, and it fails in teshtost, so no need to double check with
    // two different runners.
    [NetFullTargetFrameworkDataSource(useCoreRunner: false)]
    public void RunsToCompletionWhenJapaneseResourcesAreLookedUpForMSCorLib(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPath = GetAssetFullPath("RecursiveResourceLookupCrash.dll");
        var arguments = PrepareArguments(assemblyPath, null, null, FrameworkArgValue, runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);
        InvokeVsTest(arguments);

        // If we don't short-circuit the recursion correctly testhost will crash with StackOverflow.
        ValidateSummaryStatus(passed: 1, failed: 0, 0);
    }
}
