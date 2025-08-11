// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtilities;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.SourceBuild.Tests;

public class SourceBuiltArtifactsTests : SdkTests
{
    public static bool IncludeSourceBuiltArtifactsTests => !string.IsNullOrWhiteSpace(Config.SourceBuiltArtifactsPath);

    public SourceBuiltArtifactsTests(ITestOutputHelper outputHelper) : base(outputHelper) { }

    [ConditionalFact(typeof(SourceBuiltArtifactsTests), nameof(IncludeSourceBuiltArtifactsTests))]
    public void VerifyVersionFile()
    {
        Assert.NotNull(Config.SourceBuiltArtifactsPath);

        string outputDir = Path.Combine(Directory.GetCurrentDirectory(), Path.GetRandomFileName());
        Directory.CreateDirectory(outputDir);
        try
        {
            // Extract the .version file
            Utilities.ExtractTarball(Config.SourceBuiltArtifactsPath, outputDir, ".version");

            string[] versionLines = File.ReadAllLines(Path.Combine(outputDir, ".version"));
            Assert.Equal(2, versionLines.Length);

            // Verify the commit SHA, it will likely be either a valid commit SHA or an unknown commit SHA, depending on
            // the state of the repository and git installation.
            // Therefore, we only verify the commit SHA is not an unknown commit SHA.

            string commitSha = versionLines[0];
            OutputHelper.WriteLine($"Commit SHA: {commitSha}");
            if (Config.RunningInCI)
            {
                Assert.False(commitSha.Equals("unknown commit SHA", StringComparison.OrdinalIgnoreCase));
            }

            // Verify the SDK version

            string sdkVersion = versionLines[1];

            // Find the expected SDK version by getting it from the source built SDK
            DirectoryInfo sdkDir = new DirectoryInfo(Path.Combine(Config.DotNetDirectory, "sdk"));
            string sdkVersionPath = sdkDir.GetFiles(".version", SearchOption.AllDirectories).Single().FullName;
            string[] sdkVersionLines = File.ReadAllLines(Path.Combine(outputDir, sdkVersionPath));
            string expectedSdkVersion = sdkVersionLines[3];  // Get the unique, non-stable, SDK version

            Assert.Equal(expectedSdkVersion, sdkVersion);
        }
        finally
        {
            Directory.Delete(outputDir, recursive: true);
        }
    }

    [ConditionalFact(typeof(SourceBuiltArtifactsTests), nameof(IncludeSourceBuiltArtifactsTests))]
    public void EnsureNoSymbolsNupkgs()
    {
        Assert.NotNull(Config.SourceBuiltArtifactsPath);

        string outputDir = Path.Combine(Directory.GetCurrentDirectory(), Path.GetRandomFileName());
        Directory.CreateDirectory(outputDir);
        try
        {
            // Extract the .version file
            Utilities.ExtractTarball(Config.SourceBuiltArtifactsPath, outputDir, OutputHelper);

            string[] symbolsNupkgs = Directory.GetFiles(outputDir, "*.nupkg", SearchOption.AllDirectories)
                .Where(file => file.ToLowerInvariant().Contains(".symbols."))
                .ToArray();

            if (symbolsNupkgs.Length > 0)
            {
                string message = $"Found symbols nupkgs in the source built artifacts: {Environment.NewLine}{string.Join(Environment.NewLine, symbolsNupkgs)}";
                OutputHelper.WriteLine(message);
            }

            Assert.Empty(symbolsNupkgs);
        }
        finally
        {
            Directory.Delete(outputDir, recursive: true);
        }
    }
}
