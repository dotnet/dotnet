// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.UnifiedBuild.Tasks
{
    // Takes a path to a source-manifest.json file and
    // reads the information for a specific repo from it.
    [MSBuildMultiThreadableTask]
    public class ReadRepoInfoFromSourceManifest : Task, IMultiThreadableTask
    {
        /// <summary>Injected by MSBuild so paths resolve against the project directory in multithreaded builds.</summary>
        public TaskEnvironment TaskEnvironment { get; set; } = TaskEnvironment.Fallback;

        [Required]
        public string SourceManifest { get; set; } = "";

        [Required]
        public string RepositoryName { get; set; } = "";

        /// <summary>
        /// Returns metadata about the repository from source-manifest.json.
        /// </summary>
        [Output]
        public ITaskItem? RepoInfo { get; set; }

        public override bool Execute()
        {
            if (string.IsNullOrEmpty(SourceManifest) || !File.Exists(TaskEnvironment.GetAbsolutePath(SourceManifest)))
            {
                Log.LogError($"Source manifest file not found: {SourceManifest}");
                return false;
            }

            JsonArray? repositories = JsonNode.Parse(File.OpenRead(TaskEnvironment.GetAbsolutePath(SourceManifest)))?["repositories"]?.AsArray();

            JsonObject? repo = repositories
                ?.Where(p => p?["path"]?.ToString() == RepositoryName)
                .FirstOrDefault()
                ?.AsObject();

            if (repo == null)
            {
                Log.LogError($"Repository {RepositoryName} not found in source manifest.");
                return false;
            }

            RepoInfo = new TaskItem(RepositoryName, repo.ToDictionary(p => p.Key, p => p.Value?.ToString()));

            return true;
        }
    }
}
