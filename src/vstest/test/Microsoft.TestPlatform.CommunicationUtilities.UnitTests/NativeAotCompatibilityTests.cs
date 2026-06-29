// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETCOREAPP

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests;

/// <summary>
/// Verifies that the TranslationLayer and CommunicationUtilities assemblies can be consumed
/// by a NativeAOT application without producing IL2026/IL3050 trimming or AOT warnings from
/// the serialization code.
///
/// This test publishes a minimal console app that references the TranslationLayer with
/// PublishAot=true and checks the publish output for linker warnings originating from the
/// CommunicationUtilities.Serialization namespace. Pre-existing warnings from ObjectModel
/// and Jsonite are expected and excluded from the assertion.
/// </summary>
[TestClass]
[TestCategory("Compatibility")]
public class NativeAotCompatibilityTests
{
    private static readonly string TestAssetPath = Path.GetFullPath(
        Path.Combine(
            // Navigate from the test output directory (artifacts/bin/<project>/Debug/net11.0)
            // to the repo root, then into the test asset.
            AppContext.BaseDirectory,
            "..", "..", "..", "..", "..",
            "test", "TestAssets", "NativeAotTranslationLayerConsumer"));

    [TestMethod]
    public void TranslationLayer_NativeAotPublish_ShouldNotProduceSerializationWarnings()
    {
        // If the test asset project isn't found (e.g., running from a different layout),
        // skip rather than fail — the CI build will run from the repo root.
        if (!Directory.Exists(TestAssetPath))
        {
            Assert.Inconclusive($"Test asset not found at: {TestAssetPath}");
        }

        var rid = RuntimeInformation.RuntimeIdentifier;

        var psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"publish -r {rid} -v q --nologo",
            WorkingDirectory = TestAssetPath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        var outputBuilder = new StringBuilder();

        using var process = Process.Start(psi)!;
        process.OutputDataReceived += (_, e) => { if (e.Data is not null) outputBuilder.AppendLine(e.Data); };
        process.ErrorDataReceived += (_, e) => { if (e.Data is not null) outputBuilder.AppendLine(e.Data); };
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        // NativeAOT publish can take several minutes.
        var exited = process.WaitForExit(TimeSpan.FromMinutes(10));
        Assert.IsTrue(exited, "dotnet publish timed out after 10 minutes.");

        // Ensure all async output has been drained before reading the buffer.
        process.WaitForExit();

        var output = outputBuilder.ToString();

        // Publish must succeed.
        Assert.AreEqual(0, process.ExitCode,
            $"dotnet publish failed with exit code {process.ExitCode}.\n\nOutput:\n{output}");

        // Extract all IL warning lines from publish output.
        var warningLines = output
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Where(line => line.Contains("warning IL"))
            .ToArray();

        // Filter to only warnings from our serialization code.
        // Pre-existing warnings from Jsonite, ObjectModel, TestPropertyConverter are excluded.
        var serializationWarnings = warningLines
            .Where(line =>
                line.Contains("CommunicationUtilities", StringComparison.OrdinalIgnoreCase)
                && !line.Contains("Jsonite", StringComparison.OrdinalIgnoreCase)
                && !line.Contains("TestPropertyConverter", StringComparison.OrdinalIgnoreCase)
                && !line.Contains("DefaultJsonTypeInfoResolver", StringComparison.OrdinalIgnoreCase))
            .ToArray();

        Assert.IsEmpty(serializationWarnings,
            $"Expected zero serialization warnings from CommunicationUtilities, but found {serializationWarnings.Length}:\n"
            + string.Join("\n", serializationWarnings));
    }
}

#endif
