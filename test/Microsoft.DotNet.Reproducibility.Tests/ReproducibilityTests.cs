// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using TestUtilities;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Reproducibility.Tests;

[Trait("Category", "Reproducibility")]
public class ReproducibilityTests
{
    private ITestOutputHelper OutputHelper { get; }

    public ReproducibilityTests(ITestOutputHelper outputHelper) => OutputHelper = outputHelper;

    public static bool IncludeMsftReproducibilityTests =>
        !string.IsNullOrWhiteSpace(Config.MsftSdkTarballPath1) && !string.IsNullOrWhiteSpace(Config.MsftSdkTarballPath2);

    public static bool IncludeSourceBuiltReproducibilityTests =>
        !string.IsNullOrWhiteSpace(Config.SourceBuiltSdkTarballPath1) && !string.IsNullOrWhiteSpace(Config.SourceBuiltSdkTarballPath2);

    [ConditionalFact(typeof(ReproducibilityTests), nameof(IncludeMsftReproducibilityTests))]
    public void MsftSdkTarballIsReproducible()
    {
        CompareTarballs(
            Config.MsftSdkTarballPath1!,
            Config.MsftSdkTarballPath2!,
            exclusionsFileName: "MsftReproducibilityExclusions.txt",
            baselineFileName: "MsftReproducibilityBaseline.diff",
            updatedBaselineFileName: "UpdatedMsftReproducibilityBaseline.diff");
    }

    [ConditionalFact(typeof(ReproducibilityTests), nameof(IncludeSourceBuiltReproducibilityTests))]
    public void SourceBuiltSdkTarballIsReproducible()
    {
        CompareTarballs(
            Config.SourceBuiltSdkTarballPath1!,
            Config.SourceBuiltSdkTarballPath2!,
            exclusionsFileName: "SourceBuiltReproducibilityExclusions.txt",
            baselineFileName: "SourceBuiltReproducibilityBaseline.diff",
            updatedBaselineFileName: "UpdatedSourceBuiltReproducibilityBaseline.diff");
    }

    /// <summary>
    /// Compares the regular-file contents of two SDK tarballs and asserts that they
    /// match, modulo files listed in the exclusions file. Non-regular entries
    /// (symlinks, hardlinks, directories) and tar metadata (file modes, uid/gid,
    /// mtime, entry order, gzip header) are not compared. Any new non-reproducible
    /// file causes the test to fail; any file that becomes reproducible causes the
    /// baseline to be stale.
    /// </summary>
    private void CompareTarballs(
        string tarballPath1,
        string tarballPath2,
        string exclusionsFileName,
        string baselineFileName,
        string updatedBaselineFileName)
    {
        Assert.True(File.Exists(tarballPath1), $"First SDK tarball not found: {tarballPath1}");
        Assert.True(File.Exists(tarballPath2), $"Second SDK tarball not found: {tarballPath2}");

        OutputHelper.WriteLine($"Comparing tarballs:");
        OutputHelper.WriteLine($"  Tarball 1: {tarballPath1}");
        OutputHelper.WriteLine($"  Tarball 2: {tarballPath2}");

        Dictionary<string, string> hashes1 = null!;
        Dictionary<string, string> hashes2 = null!;

        Parallel.Invoke(
            () => hashes1 = ComputeTarballFileHashes(tarballPath1),
            () => hashes2 = ComputeTarballFileHashes(tarballPath2));

        OutputHelper.WriteLine($"  Tarball 1 files: {hashes1.Count}");
        OutputHelper.WriteLine($"  Tarball 2 files: {hashes2.Count}");

        // ExclusionsHelper.GenerateNewBaselineFile and the updated baseline write below
        // both target Config.LogsDirectory; ensure it exists for environments (e.g. local
        // runs) where the harness hasn't already created it.
        Directory.CreateDirectory(Config.LogsDirectory);

        ExclusionsHelper exclusionsHelper = new(exclusionsFileName, Config.LogsDirectory);

        // First, compute the raw diff lists. Do not consult exclusions yet — IsFileExcluded
        // has the side effect of marking matched exclusions as "used", and we only want an
        // exclusion to count as used when it actually suppresses a real diff. Otherwise an
        // exclusion for a file that has become byte-identical between the two builds would
        // never be detected as stale.
        List<string> differingFiles = new();
        List<string> onlyInFirst = new();
        List<string> onlyInSecond = new();

        foreach ((string path, string hash) in hashes1)
        {
            if (!hashes2.TryGetValue(path, out string? hash2))
            {
                onlyInFirst.Add(path);
            }
            else if (hash != hash2)
            {
                differingFiles.Add(path);
            }
        }

        foreach (string path in hashes2.Keys)
        {
            if (!hashes1.ContainsKey(path))
            {
                onlyInSecond.Add(path);
            }
        }

        // Now apply exclusions — this is the only place IsFileExcluded gets called, so each
        // matched exclusion is recorded as "used" exactly when it suppresses an actual diff.
        differingFiles.RemoveAll(p => exclusionsHelper.IsFileExcluded(p));
        onlyInFirst.RemoveAll(p => exclusionsHelper.IsFileExcluded(p));
        onlyInSecond.RemoveAll(p => exclusionsHelper.IsFileExcluded(p));

        differingFiles.Sort(StringComparer.Ordinal);
        onlyInFirst.Sort(StringComparer.Ordinal);
        onlyInSecond.Sort(StringComparer.Ordinal);

        exclusionsHelper.GenerateNewBaselineFile();

        // Build the actual diff content
        List<string> allDiffs = new();
        foreach (string file in onlyInFirst)
        {
            allDiffs.Add($"[only in first] {file}");
        }
        foreach (string file in onlyInSecond)
        {
            allDiffs.Add($"[only in second] {file}");
        }
        foreach (string file in differingFiles)
        {
            allDiffs.Add($"[diff] {file}");
        }

        string actualContent = string.Join(Environment.NewLine, allDiffs);
        if (allDiffs.Count > 0)
        {
            actualContent += Environment.NewLine;
        }

        // Write updated baseline to logs directory for CI to publish
        string updatedBaselinePath = Path.Combine(Config.LogsDirectory, updatedBaselineFileName);
        File.WriteAllText(updatedBaselinePath, actualContent);
        OutputHelper.WriteLine($"Updated baseline written to: {updatedBaselinePath}");

        // Compare against baseline
        string baselinePath = GetAssetsPath(baselineFileName);
        string baselineContent = File.ReadAllText(baselinePath).Trim();
        string actualTrimmed = actualContent.Trim();

        if (baselineContent != actualTrimmed)
        {
            string message = $"Reproducibility baseline mismatch.{Environment.NewLine}" +
                $"Expected baseline: {baselinePath}{Environment.NewLine}" +
                $"Actual results:    {updatedBaselinePath}{Environment.NewLine}{Environment.NewLine}";

            // Show what changed
            string[] baselineLines = baselineContent.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            string[] actualLines = actualTrimmed.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            string[] newEntries = actualLines.Except(baselineLines).ToArray();
            string[] removedEntries = baselineLines.Except(actualLines).ToArray();

            if (newEntries.Length > 0)
            {
                message += $"New non-reproducible files (add to baseline or fix):{Environment.NewLine}" +
                    string.Join(Environment.NewLine, newEntries) + Environment.NewLine + Environment.NewLine;
            }

            if (removedEntries.Length > 0)
            {
                message += $"Files that are now reproducible (update baseline):{Environment.NewLine}" +
                    string.Join(Environment.NewLine, removedEntries) + Environment.NewLine;
            }

            Assert.Fail(message);
        }

        OutputHelper.WriteLine(allDiffs.Count == 0
            ? "All files are reproducible!"
            : $"{allDiffs.Count} known non-reproducible file(s) match the baseline.");
    }

    /// <summary>
    /// Computes SHA256 hashes for all files in a tar.gz archive without extracting to disk.
    /// Returns a dictionary mapping relative file paths to their hex-encoded hash.
    /// </summary>
    private Dictionary<string, string> ComputeTarballFileHashes(string tarballPath)
    {
        Dictionary<string, string> fileHashes = new(StringComparer.Ordinal);

        using FileStream fileStream = File.OpenRead(tarballPath);
        using GZipStream gzipStream = new(fileStream, CompressionMode.Decompress);
        using TarReader tarReader = new(gzipStream);

        while (tarReader.GetNextEntry() is TarEntry entry)
        {
            if (entry.EntryType is not TarEntryType.RegularFile and not TarEntryType.V7RegularFile)
                continue;

            // Strip the leading directory component (the tarball root folder)
            string relativePath = StripLeadingDirectory(entry.Name);
            if (string.IsNullOrEmpty(relativePath))
                continue;

            string hash = ComputeHash(entry.DataStream);
            fileHashes[relativePath] = hash;
        }

        return fileHashes;
    }

    private static string ComputeHash(Stream? dataStream)
    {
        if (dataStream is null)
            return string.Empty;

        byte[] hashBytes = SHA256.HashData(dataStream);
        return Convert.ToHexString(hashBytes).ToLowerInvariant();
    }

    /// <summary>
    /// Strips the first path component from a tar entry name (e.g., "dotnet-sdk-10.0.100/sdk/..." → "sdk/...").
    /// </summary>
    private static string StripLeadingDirectory(string entryName)
    {
        // Normalize path separators
        entryName = entryName.Replace('\\', '/');

        int slashIndex = entryName.IndexOf('/');
        if (slashIndex < 0 || slashIndex == entryName.Length - 1)
            return string.Empty;

        return entryName[(slashIndex + 1)..];
    }

    private static string GetAssetsPath(string fileName)
    {
        return Path.Combine(Directory.GetCurrentDirectory(), "assets", fileName);
    }
}
