// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.DotNet.UnifiedBuild.Tasks.Services;
using BuildTask = Microsoft.Build.Utilities.Task;

namespace Microsoft.DotNet.UnifiedBuild.Tasks;

public class CreateSourceArtifact : BuildTask
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

    /// <summary>
    /// Create an empty detached signature file alongside the source artifact
    /// Ensures the signature is published via darc when signing occurs post-build
    /// </summary>
    public bool CreateEmptyDetachedSignature { get; init; } = false;

    private const string GitHubRepoName = "dotnet";
    private const string GitHubTimezone = "America/Los_Angeles"; // GitHub uses this timezone for commit timestamps in zip metadata
    private const int GitArchiveTimeout = 5 * 60 * 1000; // 5 minutes
    private const string TarExtension = "tar.gz";
    private const string SignatureExtension = "sig";

    public override bool Execute() => ExecuteAsync().GetAwaiter().GetResult();

    private async Task<bool> ExecuteAsync()
    {
        var processService = new ProcessService(Log, GitArchiveTimeout);

        string artifactFilePath = Path.Combine(OutputDirectory, $"{ArtifactName}-{SdkNonStableVersion}.{TarExtension}");

        Log.LogMessage(MessageImportance.High, $"Creating {TarExtension} source artifact at: {artifactFilePath}");

        try
        {
            ProcessResult result = await processService.RunProcessAsync(
                "git",
                $"-c \"tar.tar.gz.command=gzip -cn\" archive --format={TarExtension} --output \"{artifactFilePath}\" --prefix \"{GitHubRepoName}-{SdkStableVersion}/\" {SourceCommit}",
                environmentVariables: new List<ProcessEnvironmentVariable> { new ProcessEnvironmentVariable("TZ", GitHubTimezone) },
                workingDirectory: RepoRoot
            );

            if (result.ExitCode != 0)
            {
                Log.LogError($"[{TarExtension}] Git exited with code {result.ExitCode}: {result.Error}");
                return false;
            }
        }
        catch (Exception ex)
        {
            Log.LogError($"[{TarExtension}] Exception during artifact creation: {ex.Message}");
            return false;
        }

        if (CreateEmptyDetachedSignature)
        {
            string signatureFilePath = $"{artifactFilePath}.{SignatureExtension}";
            Log.LogMessage(MessageImportance.High, $"Creating empty detached signature at: {signatureFilePath}");

            try
            {
                using (FileStream fs = File.Create(signatureFilePath))
                {
                    // Create an empty file
                }
            }
            catch (Exception ex)
            {
                Log.LogError($"[{SignatureExtension}] Exception during signature file creation: {ex.Message}");
                return false;
            }
        }

        return true;
    }
}
