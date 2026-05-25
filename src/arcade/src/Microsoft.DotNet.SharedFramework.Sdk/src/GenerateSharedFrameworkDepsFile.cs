// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.Build.Tasks;
using Microsoft.Extensions.DependencyModel;
using NuGet.RuntimeModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Microsoft.DotNet.SharedFramework.Sdk
{
    public class GenerateSharedFrameworkDepsFile : BuildTask
    {
        [Required]
        public string TargetFrameworkMoniker { get; set; }

        [Required]
        public string RuntimeIdentifier { get; set; }

        [Required]
        public string SharedFrameworkName { get; set; }

        [Required]
        public string SharedFrameworkPackName { get; set; }

        [Required]
        public string Version { get; set; }

        [Required]
        public ITaskItem[] Files { get; set; }

        [Required]
        public string IntermediateOutputPath { get; set; }

        public string SharedFrameworkDepsNameOverride { get; set; }

        public string RuntimeIdentifierGraph { get; set; }

        public bool IncludeFallbacksInDepsFile { get; set; }

        [Output]
        public ITaskItem GeneratedDepsFile { get; set; }

        public override bool Execute()
        {
            var target = new TargetInfo(TargetFrameworkMoniker, RuntimeIdentifier, string.Empty, isPortable: false);
            var runtimeFiles = new List<RuntimeFile>();
            var nativeFiles = new List<RuntimeFile>();
            var resourceAssemblies = new List<ResourceAssembly>();

            foreach (var file in Files)
            {
                if (!string.IsNullOrEmpty(file.GetMetadata("GeneratedBuildFile")))
                {
                    continue;
                }
                string filePath = file.ItemSpec;
                string fileName = Path.GetFileName(filePath);
                string fileVersion = FileUtilities.GetFileVersion(filePath)?.ToString() ?? string.Empty;
                Version assemblyVersion = FileUtilities.GetAssemblyName(filePath)?.Version;
                string cultureMaybe = file.GetMetadata("Culture");
                if (!string.IsNullOrEmpty(cultureMaybe))
                {
                    resourceAssemblies.Add(new ResourceAssembly(Path.Combine(cultureMaybe, fileName), cultureMaybe));
                }
                else if (assemblyVersion == null)
                {
                    var nativeFile = new RuntimeFile(fileName, null, fileVersion);
                    nativeFiles.Add(nativeFile);
                }
                else
                {
                    var runtimeFile = new RuntimeFile(fileName,
                        fileVersion: fileVersion,
                        assemblyVersion: assemblyVersion.ToString());
                    runtimeFiles.Add(runtimeFile);
                }
            }

            var runtimeLibrary = new RuntimeLibrary("package",
               SharedFrameworkPackName,
               Version,
               hash: string.Empty,
               runtimeAssemblyGroups: new[] { new RuntimeAssetGroup(string.Empty, runtimeFiles) },
               nativeLibraryGroups: new[] { new RuntimeAssetGroup(string.Empty, nativeFiles) },
               resourceAssemblies,
               Array.Empty<Dependency>(),
               hashPath: null,
               path: $"{SharedFrameworkPackName.ToLowerInvariant()}/{Version}",
               serviceable: true);

            IEnumerable<RuntimeFallbacks> runtimeFallbackGraph = Array.Empty<RuntimeFallbacks>();

            if (IncludeFallbacksInDepsFile)
            {
                RuntimeGraph runtimeGraph = JsonRuntimeFormat.ReadRuntimeGraph(RuntimeIdentifierGraph);
                runtimeFallbackGraph = runtimeGraph.Runtimes
                        .Select(runtimeDict => runtimeGraph.ExpandRuntime(runtimeDict.Key))
                        .Where(expansion => expansion.Contains(RuntimeIdentifier))
                        .Select(expansion => new RuntimeFallbacks(expansion.First(), expansion.Skip(1))); // ExpandRuntime return runtime itself as first item.
            }

            var context = new DependencyContext(target,
                CompilationOptions.Default,
                Enumerable.Empty<CompilationLibrary>(),
                new[] { runtimeLibrary },
                runtimeFallbackGraph);

            var depsFileName = string.IsNullOrEmpty(SharedFrameworkDepsNameOverride) ? $"{SharedFrameworkName}.deps.json" : $"{SharedFrameworkDepsNameOverride}.deps.json";

            var depsFilePath = Path.Combine(IntermediateOutputPath, depsFileName);

            // When multiple project configurations build in parallel (e.g. in -mt mode),
            // they may converge on the same output file via RemoveProperties in nested MSBuild calls.
            // All invocations produce identical content, so if another writer is active or has
            // already completed, we can safely retry or reuse the existing file.
            const int maxRetries = 5;
            const int retryDelayMs = 200;

            for (int attempt = 0; attempt < maxRetries; attempt++)
            {
                try
                {
                    // Write to a temporary file first, then move atomically to avoid
                    // partial-file reads by concurrent builds.
                    var tempFilePath = depsFilePath + ".tmp" + Path.GetRandomFileName();
                    using (var depsStream = File.Create(tempFilePath))
                    {
                        new DependencyContextWriter().Write(context, depsStream);
                    }

                    try
                    {
                        File.Move(tempFilePath, depsFilePath
#if !NETFRAMEWORK
                            , overwrite: true
#endif
                            );
                    }
                    catch (IOException)
                    {
                        // Another writer may have placed the file already — that's fine,
                        // the content is identical. Clean up our temp file.
                        try { File.Delete(tempFilePath); } catch { }

                        // If the target exists, we're done — another invocation wrote it.
                        if (File.Exists(depsFilePath) && new FileInfo(depsFilePath).Length > 0)
                        {
                            GeneratedDepsFile = new TaskItem(depsFilePath);
                            return true;
                        }

                        // Target doesn't exist or is empty — rethrow to trigger retry.
                        throw;
                    }

                    GeneratedDepsFile = new TaskItem(depsFilePath);
                    return true;
                }
                catch (IOException) when (attempt < maxRetries - 1)
                {
                    // File is locked by another concurrent build writing the same deps.json.
                    // Since all invocations produce identical output, wait and retry.
                    Log.LogMessage(MessageImportance.Low,
                        $"Deps file '{depsFilePath}' is locked (attempt {attempt + 1}/{maxRetries}), retrying...");
                    Thread.Sleep(retryDelayMs * (attempt + 1));

                    // If the file now exists and is non-empty, another writer completed successfully.
                    if (File.Exists(depsFilePath) && new FileInfo(depsFilePath).Length > 0)
                    {
                        GeneratedDepsFile = new TaskItem(depsFilePath);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    // If there is a problem, ensure we don't write a partially complete version to disk.
                    if (File.Exists(depsFilePath))
                    {
                        try { File.Delete(depsFilePath); } catch { }
                    }
                    Log.LogErrorFromException(ex, false);
                    return false;
                }
            }

            Log.LogError($"Failed to write deps file '{depsFilePath}' after {maxRetries} attempts.");
            return false;
        }
    }
}
