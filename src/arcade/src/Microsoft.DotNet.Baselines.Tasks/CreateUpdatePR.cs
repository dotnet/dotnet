// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using BuildTask = Microsoft.Build.Utilities.Task;

namespace Microsoft.DotNet.Baselines.Tasks;

public class CreateUpdatePR : BuildTask
{
    /// <summary>
    /// The GitHub organization to create the PR in.
    /// </summary>
    [Required]
    public string GitHubOrg { get; set; } = string.Empty;

    /// <summary>
    /// The GitHub repository to create the PR in.
    /// </summary>
    [Required]
    public string GitHubRepo { get; set; } = string.Empty;

    /// <summary>
    /// The directory to place newly created baselines and locate baselines to update.
    /// Must be relative to the target repository root.
    /// </summary>
    [Required]
    public string TargetDirectory { get; set; } = string.Empty;

    /// <summary>
    /// The updated files to include in the PR.
    /// </summary>
    [Required]
    public ITaskItem[] UpdatedFiles { get; set; } = Array.Empty<ITaskItem>();

    /// <summary>
    /// The id of the build that published the updated test files.
    /// </summary>
    [Required]
    public int BuildId { get; set; }

    /// <summary>
    /// The title of the PR to create.
    /// </summary>
    public string Title { get; set; } = "Update Test Baselines and Exclusions";

    /// <summary>
    /// The target branch of the PR.
    /// </summary>
    public string TargetBranch { get; set; } = "main";

    /// <summary>
    /// If baseline files are created with a default content and should not be updated if they contain that content,
    /// this property can be set to provide a default content for those files.
    /// </summary>
    public string DefaultBaselineContent { get; set; } = string.Empty;

    /// <summary>
    /// Whether to combine exclusions baselines via union or intersection.
    /// If true, the exclusions baselines will be combined using a union operation.
    /// If false, the exclusions baselines will be combined using an intersection operation.
    /// </summary>
    public bool UnionExclusionsBaselines { get; set; } = false;

    /// <summary>
    /// The GitHub token to use to create the PR. If not provided, it will
    /// default to the environment variable GH_TOKEN.
    /// </summary>
    public string? GitHubToken { get; set; } = Environment.GetEnvironmentVariable("GH_TOKEN");

    public override bool Execute()
    {
        return ExecuteAsync().GetAwaiter().GetResult();
    }

    private async Task<bool> ExecuteAsync()
    {
        try
        {
            if (string.IsNullOrEmpty(GitHubOrg) || string.IsNullOrEmpty(GitHubRepo))
            {
                throw new ArgumentException("GitHubOrg and GitHubRepo must be specified.");
            }

            if (string.IsNullOrEmpty(TargetDirectory) || Path.IsPathRooted(TargetDirectory))
            {
                throw new ArgumentException("TargetDirectory must be specified and be a relative path.");
            }

            if (string.IsNullOrEmpty(GitHubToken))
            {
                throw new ArgumentException("GitHubToken must be specified or set in the GH_TOKEN environment variable.");
            }

            if (UpdatedFiles.Length == 0)
            {
                throw new ArgumentException("UpdatedFiles must contain at least one file.");
            }

            var creator = new PRCreator(Log, GitHubOrg, GitHubRepo, GitHubToken);
            return await creator.ExecuteAsync(
                TargetDirectory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar),
                UpdatedFiles.Select(file => file.ItemSpec).ToList(),
                BuildId,
                Title,
                TargetBranch,
                DefaultBaselineContent,
                UnionExclusionsBaselines);
        }
        catch (Exception ex)
        {
            Log.LogErrorFromException(ex, showStackTrace: true);
        }

        return !Log.HasLoggedErrors;
    }
}
