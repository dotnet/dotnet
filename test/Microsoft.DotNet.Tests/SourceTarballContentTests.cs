// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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
        !string.IsNullOrWhiteSpace(Config.RepoRoot);

    public SourceTarballContentTests(ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;

        if (!Directory.Exists(Config.LogsDirectory))
        {
            Directory.CreateDirectory(Config.LogsDirectory);
        }
    }

    /// <summary>
    /// Validates that all files tracked in the git repository are included in the git archive output.
    /// Detects files excluded by export-ignore directives in .gitattributes files.
    /// See https://github.com/dotnet/source-build/issues/5472
    /// </summary>
    [ConditionalFact(typeof(SourceTarballContentTests), nameof(IncludeSourceTarballContentTests))]
    public void CompareSourceTarballToGitRepository()
    {
        string repoRoot = Config.RepoRoot!;

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

        HashSet<string> archiveFiles = GetGitArchiveFileList(repoRoot);

        OutputHelper.WriteLine($"Found {archiveFiles.Count} files in git archive");

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
    /// Runs git archive and reads the tar stream directly from stdout to enumerate file entries
    /// without writing the full archive to disk.
    /// </summary>
    private HashSet<string> GetGitArchiveFileList(string repoRoot)
    {
        using Process process = new()
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = $"-C \"{repoRoot}\" archive --format=tar HEAD",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
            }
        };

        process.Start();

        // Read stderr asynchronously to prevent deadlock
        StringBuilder stderr = new();
        process.ErrorDataReceived += (_, e) => { if (e.Data is not null) stderr.AppendLine(e.Data); };
        process.BeginErrorReadLine();

        HashSet<string> files = TarHelper.GetEntryNames(process.StandardOutput.BaseStream)
            .Where(name => !name.EndsWith('/'))
            .ToHashSet();

        process.WaitForExit(600000);

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException(
                $"git archive failed with exit code {process.ExitCode}: {stderr}");
        }

        return files;
    }
}
