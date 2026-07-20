// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
public class MissingTestSdkTests : AcceptanceTestBase
{
    // A substring of the CouldNotFindTesthost guidance. We don't match the whole (localizable) message, just enough to
    // make clear the failure is telling the user to reference the Microsoft.NET.Test.Sdk package.
    private const string TestSdkGuidance = "references the 'Microsoft.NET.Test.Sdk' NuGet package";

    // A managed test project that does not reference Microsoft.NET.Test.Sdk brings no testhost of its own. It must not
    // silently run on the built-in testhost shipped next to the runner (that fallback is for native C++ runners) - the
    // run should fail and tell the user to reference Microsoft.NET.Test.Sdk.
    [TestMethod]
    [TestMatrix(console: Net, testHost: Net)]
    public void RunningManagedProjectWithoutTestSdkShouldFailAndSuggestTestSdk(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPath = GetAssetFullPath("ProjectWithoutTestSdk.dll");
        var arguments = PrepareArguments(assemblyPath, string.Empty, string.Empty, string.Empty, resultsDirectory: TempDirectory.Path);

        InvokeVsTest(arguments);

        // The run fails (the managed source is rejected, not run on the built-in testhost) and tells the user to
        // reference Microsoft.NET.Test.Sdk.
        ExitCodeEquals(1);
        Assert.Contains(TestSdkGuidance, StdOut + StdErr);
    }

    // A native (C++) source has no managed testhost of its own, and that is exactly what the built-in testhost fallback
    // is for. It keeps using the fallback (the testhost launches and discovery runs - here it finds no C++ adapter under
    // .NET Core, like the [Ignore]'d CPPRunAllTestExecutionPlatformx64Net), and must NOT get the managed
    // Microsoft.NET.Test.Sdk guidance. Uses the prebuilt Microsoft.TestPlatform.TestAsset.NativeCPP package so we don't
    // build C++ locally. Windows-only (the asset is a Windows native dll).
    [TestMethod]
    [TestCategory("Windows-Review")]
    [TestMatrix(console: Net, testHost: Net)]
    public void RunningNativeCppProjectWithoutTestSdkShouldUseTheBuiltInTesthostFallback(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPath = Path.Combine(
            _testEnvironment.GlobalPackageDirectory,
            "microsoft.testplatform.testasset.nativecpp", "2.0.0", "contentFiles", "any", "any", "x64",
            "Microsoft.TestPlatform.TestAsset.NativeCPP.dll");
        var arguments = PrepareArguments(assemblyPath, string.Empty, string.Empty, FrameworkArgValue, _testEnvironment.InIsolationValue, resultsDirectory: TempDirectory.Path);

        InvokeVsTest(arguments);

        // The fallback was used: the testhost launched and discovery ran (it just finds no C++ adapter under .NET Core).
        // This also guards against the test passing vacuously if the asset path failed to resolve.
        StdOutputContains("No test is available");
        // ...and the native source was NOT rejected with the managed Microsoft.NET.Test.Sdk guidance.
        Assert.DoesNotContain(TestSdkGuidance, StdOut + StdErr);
    }
}
