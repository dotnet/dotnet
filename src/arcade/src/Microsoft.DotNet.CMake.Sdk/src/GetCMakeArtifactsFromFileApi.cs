// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Microsoft.DotNet.CMake.Sdk
{
    /// <summary>
    /// Reads CMake File API response to find artifacts for a specific source directory.
    /// </summary>
    [MSBuildMultiThreadableTask]
    public class GetCMakeArtifactsFromFileApi : Microsoft.Build.Utilities.Task, IMultiThreadableTask
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
        [Required]
        public string SourceDirectory { get; set; }

        /// <summary>
        /// The configuration name (e.g., Debug, Release).
        /// </summary>
        [Required]
        public string Configuration { get; set; }

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
                
                if (!Directory.Exists(TaskEnvironment.GetAbsolutePath(replyDir)))
                {
                    Log.LogError("CMake File API reply directory does not exist: {0}", replyDir);
                    return false;
                }

                // Find the latest index file
                var indexFiles = Directory.GetFiles(TaskEnvironment.GetAbsolutePath(replyDir), "index-*.json");
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
                if (!File.Exists(TaskEnvironment.GetAbsolutePath(codeModelFile)))
                {
                    Log.LogError("Codemodel file not found: {0}", codeModelFile);
                    return false;
                }

                Log.LogMessage(MessageImportance.Low, "Reading codemodel: {0}", codeModelFile);
                
                string codeModelJson = File.ReadAllText(TaskEnvironment.GetAbsolutePath(codeModelFile));
                var codeModel = JsonSerializer.Deserialize<CMakeCodeModel>(codeModelJson, options);

                if (codeModel == null)
                {
                    Log.LogError("Failed to deserialize codemodel.");
                    return false;
                }

                // Get the source root from the codemodel
                string sourceRoot = codeModel.Paths?.Source?.Replace('\\', '/').TrimEnd('/') ?? "";

                // Normalize source directory for comparison
                string normalizedSourceDir = TaskEnvironment.GetAbsolutePath(SourceDirectory).Value.Replace('\\', '/').TrimEnd('/');

                // Find the configuration using LINQ
                var config = codeModel.Configurations?.FirstOrDefault(c => 
                    string.Equals(c.Name, Configuration, StringComparison.OrdinalIgnoreCase));

                if (config == null)
                {
                    Log.LogError("Configuration '{0}' not found in CMake File API response.", Configuration);
                    return false;
                }

                Log.LogMessage(MessageImportance.Low, "Found configuration: {0}", Configuration);

                if (config.Directories == null || config.Targets == null)
                {
                    Log.LogError("Configuration '{0}' has no directories or targets.", Configuration);
                    return false;
                }

                // Find the matching directory using LINQ
                var directory = config.Directories.FirstOrDefault(d =>
                {
                    string dirSource = d.Source?.Replace('\\', '/').TrimEnd('/') ?? "";
                    
                    // Make the directory source path absolute
                    if (!Path.IsPathRooted(dirSource))
                    {
                        dirSource = Path.Combine(sourceRoot, dirSource);
                        dirSource = TaskEnvironment.GetAbsolutePath(dirSource).Value.Replace('\\', '/').TrimEnd('/');
                    }
                    
                    return string.Equals(dirSource, normalizedSourceDir, StringComparison.OrdinalIgnoreCase);
                });

                if (directory == null)
                {
                    Log.LogError("Source directory '{0}' not found in CMake File API response.", SourceDirectory);
                    return false;
                }

                Log.LogMessage(MessageImportance.Low, "Found matching directory: {0}", SourceDirectory);

                // Get artifacts
                var artifacts = new List<ITaskItem>();

                if (directory.TargetIndexes != null)
                {
                    foreach (int targetIndex in directory.TargetIndexes)
                    {
                        if (targetIndex < 0 || targetIndex >= config.Targets.Count)
                        {
                            continue;
                        }

                        var target = config.Targets[targetIndex];
                        if (string.IsNullOrEmpty(target.JsonFile))
                        {
                            continue;
                        }

                        string targetFile = Path.Combine(replyDir, target.JsonFile);
                        if (!File.Exists(TaskEnvironment.GetAbsolutePath(targetFile)))
                        {
                            continue;
                        }

                        Log.LogMessage(MessageImportance.Low, "Reading target file: {0}", targetFile);

                        // Read target details
                        string targetJson = File.ReadAllText(TaskEnvironment.GetAbsolutePath(targetFile));
                        var targetDetails = JsonSerializer.Deserialize<CMakeTargetDetails>(targetJson, options);

                        // Get artifacts
                        if (targetDetails?.Artifacts != null)
                        {
                            foreach (var artifact in targetDetails.Artifacts)
                            {
                                if (!string.IsNullOrEmpty(artifact.Path))
                                {
                                    string fullPath = Path.Combine(CMakeOutputDir, artifact.Path);
                                    fullPath = TaskEnvironment.GetAbsolutePath(fullPath);
                                    
                                    var item = new TaskItem(fullPath);
                                    artifacts.Add(item);
                                    
                                    Log.LogMessage(MessageImportance.Low, "Found artifact: {0}", fullPath);
                                }
                            }
                        }
                    }
                }

                if (artifacts.Count == 0)
                {
                    Log.LogWarning("No artifacts found for source directory '{0}' in configuration '{1}'.", SourceDirectory, Configuration);
                }

                Artifacts = artifacts.ToArray();
                Log.LogMessage(MessageImportance.Normal, "Found {0} artifact(s) for source directory '{1}' in configuration '{2}'", Artifacts.Length, SourceDirectory, Configuration);
                
                return true;
            }
            catch (Exception ex)
            {
                Log.LogErrorFromException(ex, showStackTrace: false);
                return false;
            }
        }
    }
}
