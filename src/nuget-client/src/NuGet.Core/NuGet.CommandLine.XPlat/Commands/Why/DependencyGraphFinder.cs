// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using NuGet.LibraryModel;
using NuGet.Packaging.Core;
using NuGet.ProjectModel;
using NuGet.Versioning;

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
            var result = new Dictionary<string, List<DependencyNode>?>(StringComparer.OrdinalIgnoreCase);
            bool foundPackage = false;
            bool useTargetAlias = assetsFile.PackageSpec.TargetFrameworks.All(tf => !string.IsNullOrEmpty(tf.TargetAlias));
            if (!useTargetAlias
                && (assetsFile.PackageSpec.RestoreMetadata.OriginalTargetFrameworks.Count != 1
                    || assetsFile.PackageSpec.TargetFrameworks.Count != 1
                    || assetsFile.PackageSpec.RestoreMetadata.TargetFrameworks.Count != 1
                ))
            {
                throw new FileFormatException(Strings.WhyCommand_Error_InconsistentAssetsFile);
            }

            foreach (var target in assetsFile.Targets)
            {
                (string targetAlias,
                    ImmutableArray<LibraryDependency> directPackages,
                    IList<ProjectRestoreReference> directProjectReferences)
                    = GetTargetFrameworkData(useTargetAlias, target, assetsFile.PackageSpec);

                if (userInputFrameworks.Count > 0
                    && !userInputFrameworks.Any(f => string.Equals(targetAlias, f, StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }

                LockFileTargetLibrary projectAsLibrary = ConvertToLibrary(directPackages, directProjectReferences, assetsFile, target);

                DependencyNode? projectNode = CreateNode(target, targetPackage, projectAsLibrary, VersionRange.All);

                string displayName = string.IsNullOrEmpty(target.RuntimeIdentifier)
                    ? targetAlias
                    : $"{targetAlias}/{target.RuntimeIdentifier}";
                result[displayName] = projectNode?.Children.ToList();

                foundPackage |= projectNode != null;
            }

            return foundPackage ? result : null;

            static (string targetAlias, ImmutableArray<LibraryDependency> directPackages, IList<ProjectRestoreReference> directProjectReferences)
                GetTargetFrameworkData(bool useTargetAlias, LockFileTarget target, PackageSpec packageSpec)
            {
                string targetAlias;
                ImmutableArray<LibraryDependency> directPackages;
                IList<ProjectRestoreReference> directProjectReferences;
                if (useTargetAlias)
                {
                    targetAlias = target.TargetAlias;
                    directPackages = packageSpec.GetTargetFramework(targetAlias).Dependencies;
                    directProjectReferences = packageSpec.GetRestoreMetadataFramework(targetAlias).ProjectReferences;
                }
                else
                {
                    targetAlias = packageSpec.RestoreMetadata.OriginalTargetFrameworks[0];
                    directPackages = packageSpec.TargetFrameworks[0].Dependencies;
                    directProjectReferences = packageSpec.RestoreMetadata.TargetFrameworks[0].ProjectReferences;
                }
                return (targetAlias, directPackages, directProjectReferences);
            }
        }

        private static LockFileTargetLibrary ConvertToLibrary(
            ImmutableArray<LibraryDependency> directPackages,
            IList<ProjectRestoreReference> directProjectReferences,
            LockFile assetsFile,
            LockFileTarget target)
        {
            List<PackageDependency> dependencies = new List<PackageDependency>(directPackages.Length + directProjectReferences.Count);

            dependencies.AddRange(directPackages.Select(p => new PackageDependency(p.Name, p.LibraryRange.VersionRange ?? VersionRange.All)));

            string projectDirectory = Path.GetDirectoryName(assetsFile.PackageSpec.FilePath)!;
            var projectsByPath = assetsFile
                .Libraries
                .Where(l => string.Equals(l.Type, "project", StringComparison.OrdinalIgnoreCase))
                .ToDictionary(l => Path.GetFullPath(Path.Combine(projectDirectory, l.Path)), l => l, StringComparer.OrdinalIgnoreCase);
            dependencies.AddRange(directProjectReferences
                .Select(p =>
                {
                    LockFileLibrary projectLibrary = projectsByPath[p.ProjectPath];
                    LibraryRange libraryRange = new LibraryRange(
                        projectLibrary.Name,
                        VersionRange.Parse(projectLibrary.Version.ToString()),
                        LibraryDependencyTarget.Project);
                    var dependency = new PackageDependency(
                        libraryRange.Name,
                        VersionRange.Parse(projectLibrary.Version.OriginalVersion ?? projectLibrary.Version.ToString()));
                    return dependency;
                }));

            LockFileTargetLibrary project = new LockFileTargetLibrary
            {
                Name = assetsFile.PackageSpec.Name,
                Type = LibraryType.Project,
                Dependencies = dependencies
            };

            return project;
        }

        /// <summary>
        /// Convert the flat list of LockFileTargetLibrary into a display graph.
        /// </summary>
        /// <param name="target">The assets file target for the current target framework, used to look up the resolved version and dependencies.</param>
        /// <param name="filterPackage">The package that needs to be displayed</param>
        /// <param name="library"> The current node in the package graph to convert</param>
        /// <param name="requestedVersion">The requested version of the current node.</param>
        /// <returns>If the current node's package id matches the filter package id, or the filter package is a dependency of the current node, then
        /// a graph node instance is returned. Otherwise null is returned to signal that this part of the package graph does not contribute
        /// to the output display.</returns>
        public static DependencyNode? CreateNode(LockFileTarget target, string filterPackage, LockFileTargetLibrary library, VersionRange requestedVersion)
        {
            if (filterPackage.Equals(library.Name, StringComparison.OrdinalIgnoreCase))
            {
                // NuGet doesn't allow circular dependencies, and why only shows the graph up to the filter package,
                // so there's no point checking dependencies.
                return new PackageNode(
                    library.Name!,
                    library.Version!,
                    requestedVersion,
                    []);
            }

            HashSet<DependencyNode>? children = null;

            foreach (var dependency in library.Dependencies)
            {
                LockFileTargetLibrary? dependencyLibrary = target.Libraries.FirstOrDefault(l => l.Name!.Equals(dependency.Id, StringComparison.OrdinalIgnoreCase));
                if (dependencyLibrary is null)
                {
                    // When a package reference suppresses a package with PrivateAssets, but the same package is also a dependency of another
                    // package, the assets file will list the suppressed package as a dependency of a library node, but will not create a library node
                    // for the package itself.  See https://github.com/NuGet/Home/issues/14698
                    continue;
                }
                DependencyNode? childNode = CreateNode(target, filterPackage, dependencyLibrary, dependency.VersionRange);
                if (childNode is not null)
                {
                    if (children is null)
                    {
                        children = new HashSet<DependencyNode>();
                    }
                    children.Add(childNode);
                }
            }

            // Why only show the parts of the graph that lead to the target package.
            if (children is null)
            {
                return null;
            }

            if (library.Type!.Equals(LibraryType.Package, StringComparison.OrdinalIgnoreCase))
            {
                NuGetVersion resolvedVersion = library.Version!;
                var newNode = new PackageNode(
                    library.Name!,
                    resolvedVersion,
                    requestedVersion,
                    children ?? []);
                return newNode;
            }
            else
            {
                var newNode = new ProjectNode(library.Name!, children ?? []);
                return newNode;
            }
        }
    }
}
