// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NuGet.ProjectModel;

namespace NuGet.CommandLine.XPlat.Commands.Why
{
    internal static class DependencyGraphFinder
    {
        /// <summary>
        /// Finds all dependency graphs for a given project.
        /// </summary>
        /// <param name="assetsFile">Assets file for the project.</param>
        /// <param name="targetPackage">The package we want the dependency paths for.</param>
        /// <param name="userInputFrameworks">List of target framework aliases.</param>
        /// <returns>
        /// Dictionary mapping target framework aliases to their respective dependency graphs.
        /// Returns null if the project does not have a dependency on the target package.
        /// </returns>
        public static Dictionary<string, List<DependencyNode>?>? GetAllDependencyGraphsForTarget(
            LockFile assetsFile,
            string targetPackage,
            List<string> userInputFrameworks)
        {
            var dependencyGraphPerFramework = new Dictionary<string, List<DependencyNode>?>(assetsFile.Targets.Count);
            bool doesProjectHaveDependencyOnPackage = false;

            // add null to the list of runtime identifiers to account for projects that do not have a runtime identifier
            var runtimeIdentifiers = assetsFile.PackageSpec.RuntimeGraph.Runtimes.Keys
                .Append(null)
                .ToList();
            // get all top-level package and project references for the project, categorized by target framework alias
            Dictionary<string, List<string>> topLevelReferencesByFramework = GetTopLevelPackageAndProjectReferences(assetsFile, userInputFrameworks);

            if (topLevelReferencesByFramework.Count > 0)
            {
                foreach (var (targetFrameworkAlias, topLevelReferences) in topLevelReferencesByFramework)
                {
                    foreach (var runtimeIdentifier in runtimeIdentifiers)
                    {
                        var targetFrameworkDisplayName = runtimeIdentifier == null ? targetFrameworkAlias : $"{targetFrameworkAlias}/{runtimeIdentifier}";

                        LockFileTarget target = assetsFile.GetTarget(targetFrameworkAlias, runtimeIdentifier: runtimeIdentifier);

                        // get all package libraries for the framework
                        IList<LockFileTargetLibrary>? packageLibraries = target.Libraries;

                        // if the project has a dependency on the target package, get the dependency graph
                        if (packageLibraries?.Any(l => l?.Name?.Equals(targetPackage, StringComparison.OrdinalIgnoreCase) == true) == true)
                        {
                            doesProjectHaveDependencyOnPackage = true;
                            dependencyGraphPerFramework.Add(targetFrameworkDisplayName,
                                GetDependencyGraphForTargetPerFramework(topLevelReferences, packageLibraries, targetPackage));
                        }
                        else
                        {
                            dependencyGraphPerFramework.Add(targetFrameworkDisplayName, null);
                        }
                    }
                }
            }

            return doesProjectHaveDependencyOnPackage
                ? dependencyGraphPerFramework
                : null;
        }

        /// <summary>
        /// Finds all dependency paths from the top-level packages to the target package for a given framework.
        /// </summary>
        /// <param name="topLevelReferences">All top-level package and project references for the framework.</param>
        /// <param name="packageLibraries">All package libraries for the framework.</param>
        /// <param name="targetPackage">The package we want the dependency paths for.</param>
        /// <returns>
        /// List of all top-level package nodes in the dependency graph.
        /// </returns>
        private static List<DependencyNode>? GetDependencyGraphForTargetPerFramework(
            List<string> topLevelReferences,
            IList<LockFileTargetLibrary> packageLibraries,
            string targetPackage)
        {
            List<DependencyNode>? dependencyGraph = null;

            // hashset tracking every package node that we've traversed
            var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            // dictionary tracking all package nodes that have been added to the graph, mapped to their DependencyNode objects
            var dependencyNodes = new Dictionary<string, DependencyNode>(StringComparer.OrdinalIgnoreCase);
            // dictionary mapping all packageIds to their resolved version
            Dictionary<string, string> versions = GetAllResolvedVersions(packageLibraries);

            foreach (var topLevelReference in topLevelReferences)
            {
                // use depth-first search to find dependency paths from the top-level package to the target package
                DependencyNode? topLevelNode = FindDependencyPathForTarget(topLevelReference, packageLibraries, visited, dependencyNodes, versions, targetPackage);

                if (topLevelNode != null)
                {
                    dependencyGraph ??= [];
                    dependencyGraph.Add(topLevelNode);
                }
            }

            return dependencyGraph;
        }

        /// <summary>
        /// Traverses the dependency graph for a given top-level package, looking for a path to the target package.
        /// </summary>
        /// <param name="topLevelPackage">Top-level package.</param>
        /// <param name="packageLibraries">All package libraries for a given framework.</param>
        /// <param name="visited">HashSet tracking every package node that we've traversed.</param>
        /// <param name="dependencyNodes">Dictionary tracking all packageIds that were added to the graph, mapped to their DependencyNode objects.</param>
        /// <param name="versions">Dictionary mapping packageIds to their resolved versions.</param>
        /// <param name="targetPackage">The package we want the dependency paths for.</param>
        /// <returns>
        /// The top-level package node in the dependency graph (if a path was found), or null (if no path was found).
        /// </returns>
        private static DependencyNode? FindDependencyPathForTarget(
            string topLevelPackage,
            IList<LockFileTargetLibrary> packageLibraries,
            HashSet<string> visited,
            Dictionary<string, DependencyNode> dependencyNodes,
            Dictionary<string, string> versions,
            string targetPackage)
        {
            var stack = new Stack<StackDependencyData>();
            stack.Push(new StackDependencyData(topLevelPackage, null));

            while (stack.Count > 0)
            {
                var currentPackageData = stack.Pop();
                var currentPackageId = currentPackageData.Id;

                // if we reach the target node, or if we've already traversed this node and found dependency paths, add it to the graph
                if (currentPackageId.Equals(targetPackage, StringComparison.OrdinalIgnoreCase)
                    || dependencyNodes.ContainsKey(currentPackageId))
                {
                    AddToGraph(currentPackageData, dependencyNodes, versions);
                    continue;
                }

                // if we have already traversed this node's children, continue
                if (visited.Contains(currentPackageId))
                {
                    continue;
                }

                visited.Add(currentPackageId);

                // get all dependencies for the current package
                var dependencies = packageLibraries?.FirstOrDefault(i => i?.Name?.Equals(currentPackageId, StringComparison.OrdinalIgnoreCase) == true)?.Dependencies;

                if (dependencies?.Count > 0)
                {
                    // push all the dependencies onto the stack
                    foreach (var dependency in dependencies)
                    {
                        stack.Push(new StackDependencyData(dependency.Id, currentPackageData));
                    }
                }
            }

            return dependencyNodes.GetValueOrDefault(topLevelPackage);
        }

        /// <summary>
        /// Adds a dependency path to the graph, starting from the target package and traversing up to the top-level package.
        /// </summary>
        /// <param name="targetPackageData">Target node data. This stores parent references, so it can be used to construct the dependency graph
        /// up to the top-level package.</param>
        /// <param name="dependencyNodes">Dictionary tracking all packageIds that were added to the graph, mapped to their DependencyNode objects.</param>
        /// <param name="versions">Dictionary mapping packageIds to their resolved versions.</param>
        private static void AddToGraph(
            StackDependencyData targetPackageData,
            Dictionary<string, DependencyNode> dependencyNodes,
            Dictionary<string, string> versions)
        {
            // first, we traverse the target's parents, listing the packages in the path from the target to the top-level package
            var dependencyPath = new List<string> { targetPackageData.Id };
            StackDependencyData? current = targetPackageData.Parent;

            while (current != null)
            {
                dependencyPath.Add(current.Id);
                current = current.Parent;
            }

            // then, we traverse this list from the target package to the top-level package, initializing/updating their dependency nodes as needed
            for (int i = 0; i < dependencyPath.Count; i++)
            {
                string currentPackageId = dependencyPath[i];

                if (!dependencyNodes.ContainsKey(currentPackageId))
                {
                    dependencyNodes.Add(currentPackageId, new DependencyNode(currentPackageId, versions[currentPackageId]));
                }

                if (i > 0)
                {
                    var childNode = dependencyNodes[dependencyPath[i - 1]];
                    dependencyNodes[currentPackageId].Children.Add(childNode);
                }
            }
        }

        /// <summary>
        /// Get all top-level package and project references for the given project.
        /// </summary>
        /// <param name="assetsFile">Assets file for the project.</param>
        /// <param name="userInputFrameworks">List of target framework aliases.</param>
        /// <returns>
        /// Dictionary mapping the project's target framework aliases to their respective top-level package and project references.
        /// </returns>
        private static Dictionary<string, List<string>> GetTopLevelPackageAndProjectReferences(
            LockFile assetsFile,
            List<string> userInputFrameworks)
        {
            var topLevelReferences = new Dictionary<string, List<string>>();

            var targetAliases = assetsFile.PackageSpec.RestoreMetadata.OriginalTargetFrameworks;

            // filter the targets to the set of targets that the user has specified
            if (userInputFrameworks?.Count > 0)
            {
                targetAliases = targetAliases.Where(f => userInputFrameworks.Contains(f)).ToList();
            }

            // we need to match top-level project references to their target library entries using their paths,
            // so we will store all project reference paths in a dictionary here
            var projectLibraries = assetsFile.Libraries.Where(l => l.Type == "project");
            var projectLibraryPathToName = new Dictionary<string, string>(projectLibraries.Count());
            var projectDirectoryPath = Path.GetDirectoryName(assetsFile.PackageSpec.FilePath);

            if (projectDirectoryPath != null)
            {
                foreach (var library in projectLibraries)
                {
                    projectLibraryPathToName.Add(Path.GetFullPath(library.Path, projectDirectoryPath), library.Name);
                }
            }

            // get all top-level references for each target alias
            foreach (string targetAlias in targetAliases)
            {
                topLevelReferences.Add(targetAlias, []);

                // top-level packages
                TargetFrameworkInformation? targetFrameworkInformation = assetsFile.PackageSpec.TargetFrameworks.FirstOrDefault(tfi => tfi.TargetAlias.Equals(targetAlias, StringComparison.OrdinalIgnoreCase));
                if (targetFrameworkInformation != default)
                {
                    var topLevelPackages = targetFrameworkInformation.Dependencies.Select(d => d.Name);
                    topLevelReferences[targetAlias].AddRange(topLevelPackages);
                }

                // top-level projects
                ProjectRestoreMetadataFrameworkInfo? restoreMetadataFrameworkInfo = assetsFile.PackageSpec.RestoreMetadata.TargetFrameworks.FirstOrDefault(tfi => tfi.TargetAlias.Equals(targetAlias, StringComparison.OrdinalIgnoreCase));
                if (restoreMetadataFrameworkInfo != default)
                {
                    var topLevelProjectPaths = restoreMetadataFrameworkInfo.ProjectReferences.Select(p => p.ProjectPath);
                    foreach (var projectPath in topLevelProjectPaths)
                    {
                        topLevelReferences[targetAlias].Add(projectLibraryPathToName[projectPath]);
                    }
                }
            }

            return topLevelReferences;
        }

        /// <summary>
        /// Adds all resolved versions of packages to a dictionary.
        /// </summary>
        /// <param name="packageLibraries">All package libraries for a given framework.</param>
        private static Dictionary<string, string> GetAllResolvedVersions(IList<LockFileTargetLibrary> packageLibraries)
        {
            var versions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var package in packageLibraries)
            {
                if (package?.Name != null && package?.Version != null)
                {
                    versions.Add(package.Name, package.Version.ToNormalizedString());
                }
            }

            return versions;
        }

        private class StackDependencyData
        {
            public string Id { get; set; }
            public StackDependencyData? Parent { get; set; }

            public StackDependencyData(string currentId, StackDependencyData? parentDependencyData)
            {
                Id = currentId;
                Parent = parentDependencyData;
            }
        }
    }
}
