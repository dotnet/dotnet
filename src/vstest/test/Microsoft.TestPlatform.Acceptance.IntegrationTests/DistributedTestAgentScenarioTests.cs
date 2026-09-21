// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

/// <summary>
/// Reproduces the binding-redirect scenario experienced by Azure DevOps' Distributed
/// Test Agent (DTAExecutionHost) and any Visual Studio host that picks up our DLLs
/// without the in-box <c>vstest.console.exe.config</c> binding redirects.
///
/// The test loads <c>Common.dll</c> and <c>ObjectModel.dll</c> inside a net472 host that has no
/// binding redirects in its app.config and exercises two dependencies:
///
///   1. <see cref="Microsoft.VisualStudio.TestPlatform.Common.Filtering.FilterExpressionWrapper"/>,
///      which triggers <c>FastFilter.Builder</c> and forces <c>System.Collections.Immutable</c> /
///      <c>System.Reflection.Metadata</c> to load.
///   2. <c>TestCase.Id</c> with <c>VSTEST_DISABLE_XXHASH128_TESTCASE_ID=0</c>, which goes through
///      the vendored xxHash128 implementation and forces <c>System.Memory</c> to load. xxHash128
///      has to be opted in to explicitly: with the SHA1 default the <c>Span&lt;T&gt;</c>
///      using code is never JIT-compiled, so the dependency is never resolved and a break in it
///      would go unnoticed.
///
/// It runs the scenario twice:
///   1. Against the <c>Microsoft.TestPlatform</c> nupkg's
///      <c>tools/net462/Common7/IDE/Extensions/TestPlatform/</c> layout (as DTA consumes it).
///   2. Against the flat layout of the <c>Microsoft.VisualStudio.TestTools.TestPlatform.V2.CLI</c>
///      VSIX (as Visual Studio consumes it).
///
/// Regression guard: if the compiled metadata references of <c>Common.dll</c> or
/// <c>ObjectModel.dll</c> drift away from the versions we ship next to them, the test fails with
/// the same <c>FileLoadException</c> customers see. Both layouts must stay self-consistent.
/// </summary>
[TestClass]
public class DistributedTestAgentScenarioTests : NoBindingRedirectHostTestBase
{
    private const string AssetName = "DtaLikeHost";

    private const string FailureExplanation =
        "That means the compiled metadata references of Common.dll (System.Collections.Immutable / " +
        "System.Reflection.Metadata) or of ObjectModel.dll (System.Memory) do not match the versions " +
        "shipped next to them. Hosts without binding redirects - DTA, Visual Studio - will fail with " +
        "FileLoadException or FileNotFoundException on FastFilter.Builder or on computing a test case id.";

    [TestMethod]
    [TestCategory("Windows-Review")]
    public void LoadingCommonDllFromMicrosoftTestPlatformPackageWithoutBindingRedirectsDoesNotThrow()
    {
        // Nupkg layout: DTA-style consumption of the Microsoft.TestPlatform nupkg.
        RunDtaLikeHost(toolsDirOverride: null);
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    public void LoadingCommonDllFromCliV2VsixLayoutWithoutBindingRedirectsDoesNotThrow()
    {
        // VSIX layout: flat folder with Common.dll + ObjectModel.dll + their dependencies at the
        // root, as shipped in Microsoft.VisualStudio.TestTools.TestPlatform.V2.CLI.vsix and consumed
        // by Visual Studio. The VSIX is unzipped into PublishDirectory by Build.cs.
        RunDtaLikeHost(toolsDirOverride: GetExtractedVsixDirectory("Microsoft.VisualStudio.TestPlatform.Common.dll"));
    }

    private void RunDtaLikeHost(string? toolsDirOverride)
    {
        var runOut = BuildAndRunHost(AssetName, toolsDirOverride, FailureExplanation);

        Assert.Contains("OK - no binding exception.", runOut);

        // Assert the xxHash128 scenario positively rather than settling for a zero exit code: the
        // host only prints this after it computed an id whose shape proves xxHash128 produced it
        // and confirmed System.Memory really loaded into the AppDomain. Without this, a host that
        // silently stopped exercising the Span-using code would still report success.
        Assert.Contains("OK - xxhash128 test id computed, System.Memory bound.", runOut);
    }
}
