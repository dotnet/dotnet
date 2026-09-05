// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

/// <summary>
/// Base class for scenarios that build and run a net472 test asset which deliberately has no
/// binding redirects, so that a .NET Framework assembly binding failure in one of our shipped DLLs
/// surfaces as a non-zero exit code instead of reaching users.
/// </summary>
/// <remarks>
/// Shared by every scenario that consumes one of our layouts the way a host without
/// <c>vstest.console.exe.config</c> would - Azure DevOps' Distributed Test Agent, Visual Studio
/// loading the V2.CLI VSIX, or a test adapter taking a plain PackageReference.
/// </remarks>
public abstract class NoBindingRedirectHostTestBase : AcceptanceTestBase
{
    /// <summary>
    /// Builds the named test asset for net472 and runs the resulting exe.
    /// </summary>
    /// <param name="assetName">Asset name, which is also the project, exe and output folder name.</param>
    /// <param name="toolsDirOverride">
    /// Flat directory to resolve our DLLs from, or <see langword="null"/> to let the asset decide
    /// (a nupkg layout or a PackageReference, depending on the asset).
    /// </param>
    /// <param name="failureExplanation">
    /// What a non-zero exit code means for this scenario, prepended to the assertion message so a
    /// CI log alone explains the failure.
    /// </param>
    /// <returns>The standard output of the host process.</returns>
    protected string BuildAndRunHost(string assetName, string? toolsDirOverride, string failureExplanation)
    {
        var projectPath = GetIsolatedTestAsset($"{assetName}.csproj", "net472");
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
            $"dotnet build of {assetName} failed (exit {buildExit}).\nSTDOUT:\n{buildOut}\nSTDERR:\n{buildErr}");

        var exePath = Path.Combine(
            workingDir,
            "artifacts", "bin", "TestAssets", assetName,
            IntegrationTestEnvironment.BuildConfiguration,
            "net472",
            $"{assetName}.exe");

        Assert.IsTrue(File.Exists(exePath), $"Expected {assetName}.exe at '{exePath}'.");

        ExecuteApplication(exePath, args: null, out var runOut, out var runErr, out var runExit);

        Assert.AreEqual(
            0,
            runExit,
            $"{assetName}.exe exited non-zero. {failureExplanation}\n" +
            $"Tools dir: {toolsDirOverride ?? "<asset default>"}\n" +
            $"STDOUT:\n{runOut}\nSTDERR:\n{runErr}");

        return runOut;
    }

    /// <summary>
    /// Resolves the directory the V2.CLI VSIX is unzipped into, asserting that the given file is
    /// present at its root so a layout change is reported as a missing file rather than as an
    /// unexplained binding failure.
    /// </summary>
    protected static string GetExtractedVsixDirectory(string expectedFileName)
    {
        var extractedVsixDir = Path.Combine(
            IntegrationTestEnvironment.PublishDirectory,
            Path.GetFileName(IntegrationTestEnvironment.LocalVsixInsertion));

        Assert.IsTrue(
            Directory.Exists(extractedVsixDir),
            $"Extracted VSIX directory not found at '{extractedVsixDir}'. " +
            "Build.cs is expected to unzip the V2.CLI VSIX before acceptance tests run.");

        Assert.IsTrue(
            File.Exists(Path.Combine(extractedVsixDir, expectedFileName)),
            $"Expected '{expectedFileName}' at the root of the extracted VSIX ('{extractedVsixDir}').");

        return extractedVsixDir;
    }

    private static string GetPatchedDotnetPath()
    {
        var executable = OSUtils.IsWindows ? "dotnet.exe" : "dotnet";
        return Path.GetFullPath(Path.Combine(IntegrationTestEnvironment.RepoRootDirectory, "artifacts", "tmp", ".dotnet", executable));
    }
}
