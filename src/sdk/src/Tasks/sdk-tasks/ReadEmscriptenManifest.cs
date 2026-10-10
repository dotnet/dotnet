// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.Build.Framework;
using Microsoft.NET.Sdk.WorkloadManifestReader;

namespace Microsoft.DotNet.Build.Tasks
{
    [MSBuildMultiThreadableTask]
    public sealed class ReadEmscriptenManifest : Task
    {
        [Required]
        public string ManifestPath { get; set; }

        [Required]
        public string ManifestVersion { get; set; }

        [Required]
        public string NetVersion { get; set; }

        [Required]
        public string EmscriptenVersion { get; set; }

        [Output]
        public string PackageVersion { get; set; }

        public override bool Execute()
        {
            try
            {
                using (Stream stream = File.OpenRead(ManifestPath))
                {
                    WorkloadManifest manifest = WorkloadManifestReader.ReadWorkloadManifest("microsoft.net.workload.emscripten.current", stream, ManifestPath);
                    if (!string.Equals(manifest.Version, ManifestVersion, StringComparison.OrdinalIgnoreCase))
                    {
                        Log.LogError("Emscripten manifest '{0}' does not match selected 1xx version '{1}'.", ManifestPath, ManifestVersion);
                        return false;
                    }

                    foreach (string kind in new[] { "Sdk", "Cache", "Node", "Python" })
                    {
                        string packId = $"Microsoft.NET.Runtime.Emscripten.{kind}.{NetVersion}";
                        if (!manifest.Packs.TryGetValue(new WorkloadPackId(packId), out WorkloadPack pack) ||
                            string.IsNullOrWhiteSpace(pack.Version) || !pack.IsAlias)
                        {
                            Log.LogError("Emscripten manifest '{0}' is missing the version or aliases for '{1}'.", ManifestPath, packId);
                            return false;
                        }

                        if (PackageVersion != null && !string.Equals(PackageVersion, pack.Version, StringComparison.OrdinalIgnoreCase))
                        {
                            Log.LogError("Emscripten manifest '{0}' has inconsistent pack versions: '{1}' uses '{2}', expected '{3}'.", ManifestPath, packId, pack.Version, PackageVersion);
                            return false;
                        }

                        foreach (string rid in new[] { "win-x64", "win-arm64" })
                        {
                            if (!pack.AliasTo.ContainsKey(rid))
                            {
                                Log.LogError("Emscripten manifest '{0}' is missing the '{1}' alias for '{2}'.", ManifestPath, rid, packId);
                                return false;
                            }
                        }

                        foreach (KeyValuePair<string, WorkloadPackId> alias in pack.AliasTo)
                        {
                            string expectedId = $"Microsoft.NET.Runtime.Emscripten.{EmscriptenVersion}.{kind}.{alias.Key}";
                            if (!string.Equals(alias.Value.ToString(), expectedId, StringComparison.OrdinalIgnoreCase))
                            {
                                Log.LogError("Emscripten manifest '{0}' has an unexpected alias for '{1}' on '{2}': expected '{3}'.", ManifestPath, packId, alias.Key, expectedId);
                                return false;
                            }
                        }

                        PackageVersion = pack.Version;
                    }
                }
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is WorkloadManifestException || ex is JsonException)
            {
                Log.LogError("Unable to read Emscripten manifest '{0}': {1}", ManifestPath, ex.Message);
                return false;
            }

            return true;
        }
    }
}
