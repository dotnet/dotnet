// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using TestUtilities;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Tests;

[Trait("Category", "SourceTarballContent")]
public class SourceTarballContentTests
{
    private const string BaselineSubDir = nameof(SourceTarballContentTests);
    private const string BaselineFileName = "MissingFromArchive.txt";
    private const string ExclusionsFileName = "MissingFromArchiveExclusions.txt";

    private ITestOutputHelper OutputHelper { get; }

    public static bool IncludeSourceTarballContentTests =>
        !string.IsNullOrWhiteSpace(Config.RepoRoot) &&
        !string.IsNullOrWhiteSpace(Config.SourceTarballPath);

    public SourceTarballContentTests(ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;

        if (!Directory.Exists(Config.LogsDirectory))
        {
            Directory.CreateDirectory(Config.LogsDirectory);
        }
    }

    /// <summary>
    /// Validates that all files tracked in the git repository are included in the source tarball.
    /// Detects files excluded by export-ignore directives in .gitattributes files.
    /// See https://github.com/dotnet/source-build/issues/5472
    /// </summary>
    [ConditionalFact(typeof(SourceTarballContentTests), nameof(IncludeSourceTarballContentTests))]
    public void CompareSourceTarballToGitRepository()
    {
        string repoRoot = Config.RepoRoot!;
        string sourceTarballPath = Config.SourceTarballPath!;

        Assert.True(File.Exists(sourceTarballPath), $"Source tarball not found: {sourceTarballPath}");

        (Process lsTreeProcess, string lsTreeStdOut, string lsTreeStdErr) = ExecuteHelper.ExecuteProcess(
            "git", $"-c core.quotePath=false -C \"{repoRoot}\" ls-tree -r HEAD --name-only",
            OutputHelper,
            millisecondTimeout: 120000);
        Assert.True(lsTreeProcess.ExitCode == 0,
            $"git ls-tree failed with exit code {lsTreeProcess.ExitCode}: {lsTreeStdErr}");

        HashSet<string> repoFiles = lsTreeStdOut
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToHashSet();

        OutputHelper.WriteLine($"Found {repoFiles.Count} files in git repository");

        HashSet<string> archiveFiles = GetSourceTarballFileList(sourceTarballPath);

        OutputHelper.WriteLine($"Found {archiveFiles.Count} files in source tarball");

        List<string> missingFromArchive = repoFiles.Except(archiveFiles).Order().ToList();

        ExclusionsHelper exclusionsHelper = new(ExclusionsFileName, Config.LogsDirectory, BaselineSubDir);
        List<string> filteredMissing = missingFromArchive
            .Where(file => !exclusionsHelper.IsFileExcluded(file))
            .ToList();

        OutputHelper.WriteLine($"Found {missingFromArchive.Count} file(s) missing from archive ({filteredMissing.Count} after exclusions)");

        exclusionsHelper.GenerateNewBaselineFile();

        string actualContent = string.Join(Environment.NewLine, filteredMissing);
        string? message = BaselineHelper.CompareBaselineContents(BaselineFileName, actualContent, OutputHelper, Config.LogsDirectory, BaselineSubDir);
        Assert.True(message == null, message);
    }

    /// <summary>
    /// Reads the source tarball produced by CreateSourceArtifact and enumerates file entries,
    /// stripping the archive prefix directory (e.g. "dotnet-9.0.100/") from each entry name.
    /// </summary>
    private HashSet<string> GetSourceTarballFileList(string tarballPath)
    {
        using FileStream fileStream = File.OpenRead(tarballPath);
        using GZipStream gzipStream = new(fileStream, CompressionMode.Decompress);

        return TarHelper.GetEntryNames(gzipStream)
            .Where(name => !name.EndsWith('/'))
            .Select(StripArchivePrefix)
            .ToHashSet();
    }

    /// <summary>
    /// Strips the leading prefix directory from a tar entry name.
    /// CreateSourceArtifact uses --prefix "dotnet-{version}/" so entries are like "dotnet-9.0.100/path/to/file".
    /// </summary>
    private static string StripArchivePrefix(string entryName)
    {
        int slashIndex = entryName.IndexOf('/');
        return slashIndex >= 0 ? entryName[(slashIndex + 1)..] : entryName;
    }
}
