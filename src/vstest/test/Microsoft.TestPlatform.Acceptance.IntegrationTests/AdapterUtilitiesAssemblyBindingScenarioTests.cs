// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

/// <summary>
/// Guards the <c>System.Memory</c> dependency that <c>Microsoft.TestPlatform.AdapterUtilities</c>
/// acquired when the vendored xxHash128 implementation was compiled into it.
///
/// AdapterUtilities had no NuGet dependencies at all before that change, and it is loaded by test
/// adapters and by Visual Studio in hosts that have no binding redirects. On .NET Framework,
/// strong-named binding is exact-version, so if <c>System.Memory.dll</c> is missing or is a
/// different assembly version than the one baked into AdapterUtilities' metadata, computing a test
/// id throws <c>FileNotFoundException</c> or <c>FileLoadException</c> at runtime.
///
/// The failure is JIT-triggered, so the host has to run <c>TestIdProviderXxHash128</c> specifically:
/// the SHA1 <c>TestIdProvider</c> never touches <c>Span&lt;T&gt;</c> and would pass with the
/// dependency completely broken.
///
/// It runs the scenario twice, matching how AdapterUtilities is really consumed:
///   1. As a <c>PackageReference</c> on the <c>Microsoft.TestPlatform.AdapterUtilities</c> nupkg,
///      as a test adapter consumes it. This also guards that the nuspec declares the
///      <c>System.Memory</c> dependency - suppressing it would give consumers a
///      <c>FileNotFoundException</c> with nothing in our own build to notice.
///   2. Against the flat layout of the <c>Microsoft.VisualStudio.TestTools.TestPlatform.V2.CLI</c>
///      VSIX, as Visual Studio consumes it, where <c>System.Memory.dll</c> has to ship next to
///      <c>Microsoft.TestPlatform.AdapterUtilities.dll</c>.
///
/// Precedent: issue #15718, where <c>Common.dll</c> picked up a <c>System.Collections.Immutable</c>
/// dependency that no binding redirect covered, and the break was reported by a user rather than by
/// CI. See <see cref="DistributedTestAgentScenarioTests"/>.
/// </summary>
[TestClass]
public class AdapterUtilitiesAssemblyBindingScenarioTests : NoBindingRedirectHostTestBase
{
    private const string AssetName = "AdapterUtilitiesBindingHost";

    private const string FailureExplanation =
        "That means Microsoft.TestPlatform.AdapterUtilities cannot resolve System.Memory in a host " +
        "without binding redirects: either it is not shipped/declared next to AdapterUtilities.dll, " +
        "or its assembly version does not match the one baked into AdapterUtilities' metadata. Every " +
        "adapter computing a test id with TestIdProviderXxHash128 on .NET Framework will fail the same way.";

    [TestMethod]
    [TestCategory("Windows-Review")]
    public void ComputingTestIdFromAdapterUtilitiesPackageWithoutBindingRedirectsDoesNotThrow()
    {
        // Package consumption: whatever the nuspec declares is what lands next to us.
        RunAdapterUtilitiesHost(toolsDirOverride: null);
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    public void ComputingTestIdFromCliV2VsixLayoutWithoutBindingRedirectsDoesNotThrow()
    {
        // VSIX layout: flat folder with AdapterUtilities.dll + System.Memory.dll at the root, as
        // consumed by Visual Studio. The VSIX is unzipped into PublishDirectory by Build.cs.
        RunAdapterUtilitiesHost(toolsDirOverride: GetExtractedVsixDirectory("Microsoft.TestPlatform.AdapterUtilities.dll"));
    }

    private void RunAdapterUtilitiesHost(string? toolsDirOverride)
    {
        var runOut = BuildAndRunHost(AssetName, toolsDirOverride, FailureExplanation);

        // Assert positively rather than settling for a zero exit code: the host only prints this
        // after computing an id whose shape proves xxHash128 produced it and confirming
        // System.Memory really loaded into the AppDomain. Without this, a host that silently
        // stopped exercising the Span-using code would still report success.
        Assert.Contains("OK - xxhash128 test id computed, System.Memory bound.", runOut);
    }
}
