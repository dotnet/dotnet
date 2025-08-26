// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Concurrent;
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
    /// Version of the artifact to generate
    /// </summary>
    [Required]
    public required string ArtifactName { get; init; }

    /// <summary>
    /// Version of the artifact to generate
    /// </summary>
    [Required]
    public required string ArtifactVersion { get; init; }

    /// <summary>
    /// Path to the directory to place the source artifacts in
    /// </summary>
    [Required]
    public required string OutputDirectory { get; init; }

    public override bool Execute()
    {
        try
        {
            return ExecuteAsync().GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            Log.LogError($"Source artifact creation failed: {ex.Message}");
            return false;
        }
    }

    private async Task<bool> ExecuteAsync()
    {
        var processService = new ProcessService(Log);
        var errors = new ConcurrentQueue<string>();

        await Parallel.ForEachAsync(Enum.GetValues<ArtifactType>(), async (artifactType, _) =>
        {
            try
            {
                await CreateArtifactAsync(artifactType, processService);
            }
            catch (Exception ex)
            {
                errors.Enqueue(ex.Message);
            }
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

    private async Task CreateArtifactAsync(ArtifactType artifactType, ProcessService processService)
    {
        string artifactFileName = $"{ArtifactName}-{ArtifactVersion}{artifactType.GetArtifactExtension()}";
        string artifactFilePath = Path.Combine(OutputDirectory, artifactFileName);

        Log.LogMessage(MessageImportance.High, $"Creating {artifactType} source artifact at: {artifactFilePath}");

        ProcessResult result = await processService.RunProcessAsync("git", $"archive -o {artifactFilePath} {SourceCommit}");

        if (result.ExitCode != 0)
        {
            throw new InvalidOperationException($"Failed to create {artifactType} source artifact: {result.Error}");
        }
    }
}
