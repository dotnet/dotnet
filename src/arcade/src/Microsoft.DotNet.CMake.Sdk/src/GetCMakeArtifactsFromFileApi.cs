// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Microsoft.DotNet.CMake.Sdk;

/// <summary>
/// Reads CMake File API response to find artifacts for a specific source directory or target.
/// </summary>
[MSBuildMultiThreadableTask]
public class GetCMakeArtifactsFromFileApi : Task, IMultiThreadableTask
{
    /// <summary>Injected by MSBuild so paths resolve against the project directory in multithreaded builds.</summary>
    public TaskEnvironment TaskEnvironment { get; set; } = TaskEnvironment.Fallback;

    /// <summary>
    /// The CMake build output directory containing the File API response.
    /// </summary>
    [Required]
    public string CMakeOutputDir { get; set; }

    /// <summary>
    /// The source directory of the CMakeLists.txt to find artifacts for.
    /// </summary>
    public string SourceDirectory { get; set; }

    /// <summary>
    /// Semicolon-separated CMake target names to find artifacts for.
    /// </summary>
    public string CMakeTargets { get; set; }

    /// <summary>
    /// The configuration name (e.g., Debug, Release).
    /// </summary>
    [Required]
    public string Configuration { get; set; }

    /// <summary>
    /// Synchronizes the File API read with native builds using the same output directory.
    /// </summary>
    public bool SynchronizeWithCMakeBuild { get; set; }

    /// <summary>
    /// Output: The list of artifact file paths.
    /// </summary>
    [Output]
    public ITaskItem[] Artifacts { get; set; }

    public override bool Execute()
    {
        try
        {
            string replyDir = Path.Combine(CMakeOutputDir, ".cmake", "api", "v1", "reply");

            if (SynchronizeWithCMakeBuild && !Directory.Exists(TaskEnvironment.GetAbsolutePath(CMakeOutputDir)))
            {
                Log.LogError("CMake File API reply directory does not exist: {0}", replyDir);
                return false;
            }

            using var mutex = SynchronizeWithCMakeBuild ? CMakeBuildMutex.Acquire(TaskEnvironment, CMakeOutputDir) : null;
            
            AbsolutePath replyDirPath = TaskEnvironment.GetAbsolutePath(replyDir);
            if (!Directory.Exists(replyDirPath))
            {
                Log.LogError("CMake File API reply directory does not exist: {0}", replyDir);
                return false;
            }

            // Find the latest index file
            var indexFiles = Directory.GetFiles(replyDirPath, "index-*.json");
            if (indexFiles.Length == 0)
            {
                Log.LogError("No CMake File API index files found.");
                return false;
            }

            string indexFile = indexFiles.OrderByDescending(f => f).First();
            Log.LogMessage(MessageImportance.Low, "Reading CMake File API index: {0}", indexFile);

            string indexJson = File.ReadAllText(TaskEnvironment.GetAbsolutePath(indexFile));
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var index = JsonSerializer.Deserialize<CMakeFileApiIndex>(indexJson, options);

            if (index?.Reply?.ClientReply?.CodemodelV2?.JsonFile == null)
            {
                Log.LogError("No 'codemodel-v2' found in File API index reply.");
                return false;
            }

            string codeModelFile = Path.Combine(replyDir, index.Reply.ClientReply.CodemodelV2.JsonFile);
            AbsolutePath codeModelFilePath = TaskEnvironment.GetAbsolutePath(codeModelFile);
            if (!File.Exists(codeModelFilePath))
            {
                Log.LogError("Codemodel file not found: {0}", codeModelFile);
                return false;
            }

            Log.LogMessage(MessageImportance.Low, "Reading codemodel: {0}", codeModelFile);
            
            string codeModelJson = File.ReadAllText(codeModelFilePath);
            var codeModel = JsonSerializer.Deserialize<CMakeCodeModel>(codeModelJson, options);

            if (codeModel == null)
            {
                Log.LogError("Failed to deserialize codemodel.");
                return false;
            }

            // Get the source root from the codemodel
            string sourceRoot = codeModel.Paths?.Source?.Replace('\\', '/').TrimEnd('/') ?? "";

            // Find the configuration using LINQ
            var config = codeModel.Configurations?.FirstOrDefault(c => 
                string.Equals(c.Name, Configuration, StringComparison.OrdinalIgnoreCase));

            if (config == null)
            {
                Log.LogError("Configuration '{0}' not found in CMake File API response.", Configuration);
                return false;
            }

            Log.LogMessage(MessageImportance.Low, "Found configuration: {0}", Configuration);

            if (config.Targets == null || (string.IsNullOrEmpty(CMakeTargets) && config.Directories == null))
            {
                Log.LogError("Configuration '{0}' has no directories or targets.", Configuration);
                return false;
            }

            // Get artifacts
            var artifacts = new List<ITaskItem>();
            IEnumerable<CMakeTarget> targets;
            if (!string.IsNullOrEmpty(CMakeTargets))
            {
                var requestedTargets = CMakeTargets.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                // Generator-provided targets such as "install" are not present in the codemodel.
                targets = config.Targets.Where(t => requestedTargets.Contains(t.Name, StringComparer.Ordinal));
                Log.LogMessage(MessageImportance.Low, "Found {0} requested CMake target(s) for configuration '{1}'.", targets.Count(), Configuration);
            }
            else
            {
                if (string.IsNullOrEmpty(SourceDirectory))
                {
                    Log.LogError("Either SourceDirectory or CMakeTargets must be specified.");
                    return false;
                }

                // GetAbsolutePath does not canonicalize, but this value is string-compared against
                // dirSource below, and Path.GetFullPath used to resolve the "." and ".." segments that
                // CMake's file API routinely emits.
                string normalizedSourceDir = TaskEnvironment.GetAbsolutePath(SourceDirectory).GetCanonicalForm().Value.Replace('\\', '/').TrimEnd('/');
                var directory = config.Directories.FirstOrDefault(d =>
                {
                    string dirSource = d.Source?.Replace('\\', '/').TrimEnd('/') ?? "";
                    if (!Path.IsPathRooted(dirSource))
                    {
                        dirSource = Path.Combine(sourceRoot, dirSource);
                    }
                    dirSource = TaskEnvironment.GetAbsolutePath(dirSource).GetCanonicalForm().Value.Replace('\\', '/').TrimEnd('/');

                    return string.Equals(dirSource, normalizedSourceDir, StringComparison.OrdinalIgnoreCase);
                });

                if (directory == null)
                {
                    Log.LogError("Source directory '{0}' not found in CMake File API response.", SourceDirectory);
                    return false;
                }

                Log.LogMessage(MessageImportance.Low, "Found matching directory: {0}", SourceDirectory);
                targets = directory.TargetIndexes?
                    .Where(targetIndex => targetIndex >= 0 && targetIndex < config.Targets.Count)
                    .Select(targetIndex => config.Targets[targetIndex])
                    ?? Enumerable.Empty<CMakeTarget>();
            }

            foreach (var target in targets)
            {
                if (string.IsNullOrEmpty(target.JsonFile))
                {
                    continue;
                }

                string targetFile = Path.Combine(replyDir, target.JsonFile);
                AbsolutePath targetFilePath = TaskEnvironment.GetAbsolutePath(targetFile);
                if (!File.Exists(targetFilePath))
                {
                    continue;
                }

                Log.LogMessage(MessageImportance.Low, "Reading target file: {0}", targetFile);

                // Read target details
                string targetJson = File.ReadAllText(targetFilePath);
                var targetDetails = JsonSerializer.Deserialize<CMakeTargetDetails>(targetJson, options);

                // Get artifacts
                if (targetDetails?.Artifacts != null)
                {
                    foreach (var artifact in targetDetails.Artifacts)
                    {
                        if (!string.IsNullOrEmpty(artifact.Path))
                        {
                            string fullPath = Path.Combine(CMakeOutputDir, artifact.Path);
                            // Emitted as an item spec, and combining the output dir with a
                            // CMake-relative artifact path routinely produces ".." segments
                            // that Path.GetFullPath used to resolve.
                            fullPath = TaskEnvironment.GetAbsolutePath(fullPath).GetCanonicalForm();

                            var item = new TaskItem(fullPath);
                            artifacts.Add(item);

                            Log.LogMessage(MessageImportance.Low, "Found artifact: {0}", fullPath);
                        }
                    }
                }
            }

            if (artifacts.Count == 0)
            {
                Log.LogWarning(
                    string.IsNullOrEmpty(CMakeTargets)
                        ? "No artifacts found for source directory '{0}' in configuration '{1}'."
                        : "No artifacts found for CMake target(s) '{0}' in configuration '{1}'.",
                    string.IsNullOrEmpty(CMakeTargets) ? SourceDirectory : CMakeTargets,
                    Configuration);
            }

            Artifacts = artifacts.ToArray();
            Log.LogMessage(
                MessageImportance.Normal,
                string.IsNullOrEmpty(CMakeTargets)
                    ? "Found {0} artifact(s) for source directory '{1}' in configuration '{2}'"
                    : "Found {0} artifact(s) for CMake target(s) '{1}' in configuration '{2}'",
                Artifacts.Length,
                string.IsNullOrEmpty(CMakeTargets) ? SourceDirectory : CMakeTargets,
                Configuration);
            
            return true;
        }
        catch (Exception ex)
        {
            Log.LogErrorFromException(ex, showStackTrace: false);
            return false;
        }
    }
}
