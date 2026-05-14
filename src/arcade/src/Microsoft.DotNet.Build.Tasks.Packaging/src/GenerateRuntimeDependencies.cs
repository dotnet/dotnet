// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Newtonsoft.Json;
using NuGet.RuntimeModel;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.DotNet.Build.Tasks.Packaging
{
    public class GenerateRuntimeDependencies : BuildTask
    {
        private const string c_emptyDependency = "none";

        [Required]
        public ITaskItem[] Dependencies
        {
            get;
            set;
        }

        [Required]
        public string PackageId
        {
            get;
            set;
        }

        public ITaskItem RuntimeJsonTemplate
        {
            get;
            set;
        }

        [Required]
        public ITaskItem RuntimeJson
        {
            get;
            set;
        }


        public override bool Execute()
        {
            if (Dependencies == null || Dependencies.Length == 0)
            {
                Log.LogError("Dependencies argument must be specified");
                return false;
            }

            if (String.IsNullOrEmpty(PackageId))
            {
                Log.LogError("PackageID argument must be specified");
                return false;
            }

            if (RuntimeJson == null)
            {
                Log.LogError("RuntimeJson argument must be specified");
                return false;
            }

            string sourceRuntimeFilePath = null;
            if (RuntimeJsonTemplate != null)
            {
                sourceRuntimeFilePath = RuntimeJsonTemplate.GetMetadata("FullPath");
            }
            string destRuntimeFilePath = RuntimeJson.GetMetadata("FullPath");


            Dictionary<string, string> packageAliases = new Dictionary<string, string>();
            foreach (var dependency in Dependencies)
            {
                string alias = dependency.GetMetadata("PackageAlias");

                if (String.IsNullOrEmpty(alias))
                {
                    continue;
                }

                Log.LogMessage(LogImportance.Low, "Aliasing {0} -> {1}", alias, dependency.ItemSpec);
                packageAliases[alias] = dependency.ItemSpec;
            }

            var runtimeGroups = Dependencies.GroupBy(d => d.GetMetadata("TargetRuntime"));

            List<RuntimeDescription> runtimes = new List<RuntimeDescription>();
            foreach (var runtimeGroup in runtimeGroups)
            {
                string targetRuntimeId = runtimeGroup.Key;

                if (String.IsNullOrEmpty(targetRuntimeId))
                {
                    Log.LogMessage(LogImportance.Low, "Skipping dependencies {0} since they don't have a TargetRuntime.", String.Join(", ", runtimeGroup.Select(d => d.ItemSpec)));
                    continue;
                }

                if (runtimeGroup.Any(d => d.ItemSpec == c_emptyDependency))
                {
                    runtimes.Add(new RuntimeDescription(targetRuntimeId));
                    continue;
                }

                List<RuntimeDependencySet> runtimeDependencySets = new List<RuntimeDependencySet>();
                var targetPackageGroups = runtimeGroup.GroupBy(d => GetTargetPackageId(d, packageAliases));
                foreach (var targetPackageGroup in targetPackageGroups)
                {
                    string targetPackageId = targetPackageGroup.Key;

                    List<RuntimePackageDependency> runtimePackageDependencies = new List<RuntimePackageDependency>();
                    var dependencyGroups = targetPackageGroup.GroupBy(d => d.ItemSpec);
                    foreach (var dependencyGroup in dependencyGroups)
                    {
                        string dependencyId = dependencyGroup.Key;
                        var dependencyVersions = dependencyGroup.Select(d => GetDependencyVersion(d));
                        var maxDependencyVersion = dependencyVersions.Max();
                        runtimePackageDependencies.Add(new RuntimePackageDependency(dependencyId, new VersionRange(maxDependencyVersion)));
                    }
                    runtimeDependencySets.Add(new RuntimeDependencySet(targetPackageId, runtimePackageDependencies));
                }
                runtimes.Add(new RuntimeDescription(targetRuntimeId, runtimeDependencySets));
            }

            RuntimeGraph runtimeGraph = new RuntimeGraph(runtimes);

            // read in existing JSON, if it was provided so that we preserve any 
            // hand authored #imports or dependencies
            if (!String.IsNullOrEmpty(sourceRuntimeFilePath))
            {
                RuntimeGraph existingGraph  = JsonRuntimeFormat.ReadRuntimeGraph(sourceRuntimeFilePath);
                runtimeGraph = RuntimeGraph.Merge(existingGraph, runtimeGraph);
            }

            string destRuntimeFileDir = Path.GetDirectoryName(destRuntimeFilePath);
            if (!String.IsNullOrEmpty(destRuntimeFileDir) && !Directory.Exists(destRuntimeFileDir))
            {
                Directory.CreateDirectory(destRuntimeFileDir);
            }

            NuGetUtility.WriteRuntimeGraph(destRuntimeFilePath, runtimeGraph);

            return true;
        }

        private string GetTargetPackageId(ITaskItem dependency, IDictionary<string, string> packageAliases)
        {
            string targetPackageId = dependency.GetMetadata("TargetPackage");
            string targetPackageAlias = dependency.GetMetadata("TargetPackageAlias");

            if (!String.IsNullOrEmpty(targetPackageAlias) && !packageAliases.TryGetValue(targetPackageAlias, out targetPackageId))
            {
                Log.LogWarning("Dependency {0} specified TargetPackageAlias {1} but no package was found defining this alias.", dependency.ItemSpec, targetPackageAlias);
            }
            else
            {
                Log.LogMessage(LogImportance.Low, "Using {0} for TargetPackageAlias {1}", targetPackageId, targetPackageAlias);
            }

            if (String.IsNullOrEmpty(targetPackageId))
            {
                Log.LogMessage(LogImportance.Low, "Dependency {0} has no parent so will assume {1}.", dependency.ItemSpec, PackageId);
                targetPackageId = PackageId;
            }

            return targetPackageId;
        }

        private NuGetVersion GetDependencyVersion(ITaskItem dependency)
        {
            NuGetVersion dependencyVersion;
            string dependencyVersionString = dependency.GetMetadata("version");

            if (String.IsNullOrEmpty(dependencyVersionString))
            {
                Log.LogWarning("Dependency {0} has no version", dependency.ItemSpec);
                dependencyVersion = new NuGetVersion(0, 0, 0);
            }
            else if (!NuGetVersion.TryParse(dependencyVersionString, out dependencyVersion))
            {
                Log.LogError("Dependency {0} has invalid version {1}", dependency.ItemSpec, dependencyVersionString);
                dependencyVersion = new NuGetVersion(0, 0, 0);
            }

            return dependencyVersion;
        }
    }
}
