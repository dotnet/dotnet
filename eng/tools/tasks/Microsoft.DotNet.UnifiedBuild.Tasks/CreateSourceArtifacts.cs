// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.DotNet.UnifiedBuild.Tasks.Models;
using Microsoft.DotNet.UnifiedBuild.Tasks.Services;
using BuildTask = Microsoft.Build.Utilities.Task;

namespace Microsoft.DotNet.UnifiedBuild.Tasks;

public class CreateSourceArtifacts : BuildTask
{
    /// <summary>
    /// Commit SHA of the VMR source to download
    /// </summary>
    [Required]
    public required string SourceCommit { get; init; }

    /// <summary>
    /// Name of the artifact to generate
    /// </summary>
    [Required]
    public required string ArtifactName { get; init; }

    /// <summary>
    /// The stable sdk version
    /// Should match the sdk tag associated with a release
    /// Used for artifact prefixing
    /// </summary>
    [Required]
    public required string SdkStableVersion { get; init; }

    /// <summary>
    /// The build/non-stable sdk version
    /// Used for artifact versioning
    /// </summary>
    [Required]
    public required string SdkNonStableVersion { get; init; }

    /// <summary>
    /// Path to the root of the repo
    /// </summary>
    [Required]
    public required string RepoRoot { get; init; }

    /// <summary>
    /// Path to the directory to place the source artifacts in
    /// </summary>
    [Required]
    public required string OutputDirectory { get; init; }

    private const string GitHubRepoName = "dotnet";
    private const string GitHubTimezone = "America/Los_Angeles"; // GitHub uses this timezone for commit timestamps in zip metadata
    private const int GitArchiveTimeout = 5 * 60 * 1000; // 5 minutes

    public override bool Execute() => ExecuteAsync().GetAwaiter().GetResult();

    private async Task<bool> ExecuteAsync()
    {
        var processService = new ProcessService(Log, GitArchiveTimeout);
        var errors = new ConcurrentQueue<string>();

        await Parallel.ForEachAsync(Enum.GetValues<ArtifactType>(), async (artifactType, _) =>
        {
            await CreateArtifactAsync(artifactType, processService, errors);
        });

        if (errors.IsEmpty)
        {
            Log.LogMessage(MessageImportance.High, "Source artifact creation succeeded.");
            return true;
        }

        Log.LogError($"Source artifact creation failed with the following errors:");
        foreach (var error in errors)
        {
            Log.LogError($" - {error}");
        }
        return false;
    }

    private async Task CreateArtifactAsync(ArtifactType artifactType, ProcessService processService, ConcurrentQueue<string> errors)
    {
        string artifactFilePath = Path.Combine(OutputDirectory, $"{ArtifactName}-{SdkNonStableVersion}.{artifactType.GetArtifactExtension()}");

        Log.LogMessage(MessageImportance.High, $"Creating {artifactType} source artifact at: {artifactFilePath}");

        try
        {
            ProcessResult result = await processService.RunProcessAsync(
                "git",
                artifactType.GetGitArchiveArgs(artifactFilePath, GitHubRepoName, SdkStableVersion, SourceCommit),
                environmentVariables: new List<ProcessEnvironmentVariable> { new ProcessEnvironmentVariable("TZ", GitHubTimezone) },
                workingDirectory: RepoRoot
            );

            if (result.ExitCode != 0)
            {
                errors.Enqueue($"[{artifactType}] Git exited with code {result.ExitCode}: {result.Error}");
            }
        }
        catch (Exception ex)
        {
            errors.Enqueue($"[{artifactType}] Exception during artifact creation: {ex.Message}");
        }
    }
}
