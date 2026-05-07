// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

/// <summary>
/// Reproduces the binding-redirect scenario experienced by Azure DevOps' Distributed
/// Test Agent (DTAExecutionHost) and any Visual Studio host that picks up
/// <c>Microsoft.VisualStudio.TestPlatform.Common.dll</c> without the in-box
/// <c>vstest.console.exe.config</c> binding redirects.
///
/// The test loads <c>Common.dll</c> inside a net472 host that has no binding
/// redirects in its app.config and calls
/// <see cref="Microsoft.VisualStudio.TestPlatform.Common.Filtering.FilterExpressionWrapper"/>,
/// which triggers <c>FastFilter.Builder</c> and forces
/// <c>System.Collections.Immutable</c> / <c>System.Reflection.Metadata</c> to load.
///
/// It runs the scenario twice:
///   1. Against the <c>Microsoft.TestPlatform</c> nupkg's
///      <c>tools/net462/Common7/IDE/Extensions/TestPlatform/</c> layout (as DTA consumes it).
///   2. Against the flat layout of the <c>Microsoft.VisualStudio.TestTools.TestPlatform.V2.CLI</c>
///      VSIX (as Visual Studio consumes it).
///
/// Regression guard: if <c>Common.dll</c>'s compiled metadata references for SCI/SRM drift
/// away from the versions we ship next to it, the test fails with the same
/// <c>FileLoadException</c> customers see. Both layouts must stay self-consistent.
/// </summary>
[TestClass]
public class DistributedTestAgentScenarioTests : AcceptanceTestBase
{
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
        // VSIX layout: flat folder with Common.dll + SCI + SRM at the root, as shipped
        // in Microsoft.VisualStudio.TestTools.TestPlatform.V2.CLI.vsix and consumed by
        // Visual Studio. The VSIX is unzipped into PublishDirectory by Build.cs.
        var extractedVsixDir = Path.Combine(
            IntegrationTestEnvironment.PublishDirectory,
            Path.GetFileName(IntegrationTestEnvironment.LocalVsixInsertion));

        Assert.IsTrue(
            Directory.Exists(extractedVsixDir),
            $"Extracted VSIX directory not found at '{extractedVsixDir}'. " +
            "Build.cs is expected to unzip the V2.CLI VSIX before acceptance tests run.");

        Assert.IsTrue(
            File.Exists(Path.Combine(extractedVsixDir, "Microsoft.VisualStudio.TestPlatform.Common.dll")),
            $"Expected Common.dll at the root of the extracted VSIX ('{extractedVsixDir}').");

        RunDtaLikeHost(toolsDirOverride: extractedVsixDir);
    }

    private void RunDtaLikeHost(string? toolsDirOverride)
    {
        var projectPath = GetIsolatedTestAsset("DtaLikeHost.csproj", "net472");
        var workingDir = Path.GetDirectoryName(projectPath)!;

        var dotnetPath = GetPatchedDotnetPath();

        var buildArgs =
            $@"build ""{projectPath}"" -c {IntegrationTestEnvironment.BuildConfiguration} " +
            $@"/p:PackageVersion={IntegrationTestEnvironment.LatestLocallyBuiltNugetVersion} " +
            @"/nodeReuse:false";

        if (toolsDirOverride is not null)
        {
            buildArgs += $@" /p:TestPlatformToolsDirOverride=""{toolsDirOverride}""";
        }

        ExecuteApplication(dotnetPath, buildArgs, out var buildOut, out var buildErr, out var buildExit, workingDirectory: workingDir);

        Assert.AreEqual(
            0,
            buildExit,
            $"dotnet build of DtaLikeHost failed (exit {buildExit}).\nSTDOUT:\n{buildOut}\nSTDERR:\n{buildErr}");

        var exePath = Path.Combine(
            workingDir,
            "artifacts", "bin", "TestAssets", "DtaLikeHost",
            IntegrationTestEnvironment.BuildConfiguration,
            "net472",
            "DtaLikeHost.exe");

        Assert.IsTrue(File.Exists(exePath), $"Expected DtaLikeHost.exe at '{exePath}'.");

        // With the fix in place, Common.dll's compiled metadata references for
        // System.Collections.Immutable and System.Reflection.Metadata match the DLLs
        // shipped next to it, so the host exe completes normally even without any
        // binding redirects in its app.config.
        ExecuteApplication(exePath, args: null, out var runOut, out var runErr, out var runExit);

        Assert.AreEqual(
            0,
            runExit,
            "DtaLikeHost.exe exited non-zero, which means Common.dll's compiled metadata " +
            "references for System.Collections.Immutable / System.Reflection.Metadata do " +
            "not match the versions shipped next to it. DTA-style hosts (no binding " +
            "redirects) will FileLoadException on FastFilter.Builder.\n" +
            $"Tools dir: {toolsDirOverride ?? "<nupkg default>"}\n" +
            $"STDOUT:\n{runOut}\nSTDERR:\n{runErr}");

        Assert.Contains("OK - no binding exception.", runOut);
    }

    private static string GetPatchedDotnetPath()
    {
        var executable = OSUtils.IsWindows ? "dotnet.exe" : "dotnet";
        return Path.GetFullPath(Path.Combine(IntegrationTestEnvironment.RepoRootDirectory, "artifacts", "tmp", ".dotnet", executable));
    }
}