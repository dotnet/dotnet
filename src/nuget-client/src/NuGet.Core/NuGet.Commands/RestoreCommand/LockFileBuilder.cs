// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NuGet.Common;
using NuGet.DependencyResolver;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging;
using NuGet.ProjectModel;
using NuGet.Repositories;
using NuGet.Shared;
using NuGetVersion = NuGet.Versioning.NuGetVersion;

namespace NuGet.Commands
{
    public class LockFileBuilder
    {
        private readonly int _lockFileVersion;
        private readonly ILogger _logger;
        private readonly Dictionary<RestoreTargetGraph, Dictionary<string, LibraryIncludeFlags>> _includeFlagGraphs;

        public LockFileBuilder(int lockFileVersion,
            ILogger logger,
            Dictionary<RestoreTargetGraph,
            Dictionary<string, LibraryIncludeFlags>> includeFlagGraphs)
        {
            _lockFileVersion = lockFileVersion;
            _logger = logger;
            _includeFlagGraphs = includeFlagGraphs;
        }

        [Obsolete("Use method with LockFileBuilderCache parameter")]
        public LockFile CreateLockFile(LockFile previousLockFile,
            PackageSpec project,
            IEnumerable<RestoreTargetGraph> targetGraphs,
            IReadOnlyList<NuGetv3LocalRepository> localRepositories,
            RemoteWalkContext context)
        {
            return CreateLockFile(previousLockFile,
                project,
                targetGraphs,
                localRepositories,
                context,
                new LockFileBuilderCache());
        }

        public LockFile CreateLockFile(LockFile previousLockFile,
            PackageSpec project,
            IEnumerable<RestoreTargetGraph> targetGraphs,
            IReadOnlyList<NuGetv3LocalRepository> localRepositories,
            RemoteWalkContext context,
            LockFileBuilderCache lockFileBuilderCache)
        {
            var lockFile = new LockFile()
            {
                Version = _lockFileVersion
            };

            var previousLibraries = previousLockFile?.Libraries.ToDictionary(l => Tuple.Create(l.Name, l.Version));

            if (project.RestoreMetadata?.ProjectStyle == ProjectStyle.PackageReference ||
                project.RestoreMetadata?.ProjectStyle == ProjectStyle.DotnetToolReference)
            {
                AddProjectFileDependenciesForPackageReference(project, lockFile, targetGraphs);
            }
            else
            {
                AddProjectFileDependenciesForSpec(project, lockFile);
            }

            // Record all libraries used
            var libraryItems = targetGraphs
                .SelectMany(g => g.Flattened) // All GraphItem<RemoteResolveResult> resolved in the graph.
                .Distinct(GraphItemKeyComparer<RemoteResolveResult>.Instance) // Distinct list of GraphItems. Two items are equal only if the itmes' Keys are equal.
                .OrderBy(x => x.Data.Match.Library);
            foreach (var item in libraryItems)
            {
                var library = item.Data.Match.Library;

                if (project.Name.Equals(library.Name, StringComparison.OrdinalIgnoreCase))
                {
                    // Do not include the project itself as a library.
                    continue;
                }

                if (library.Type == LibraryType.Project || library.Type == LibraryType.ExternalProject)
                {
                    // Project
                    var localMatch = (LocalMatch)item.Data.Match;

                    var projectLib = new LockFileLibrary()
                    {
                        Name = library.Name,
                        Version = library.Version,
                        Type = LibraryType.Project,
                    };

                    // Set the relative path if a path exists
                    // For projects without project.json this will be empty
                    if (!string.IsNullOrEmpty(localMatch.LocalLibrary.Path))
                    {
                        projectLib.Path = PathUtility.GetRelativePath(
                            project.FilePath,
                            localMatch.LocalLibrary.Path,
                            '/');
                    }

                    // The msbuild project path if it exists
                    object msbuildPath;
                    if (localMatch.LocalLibrary.Items.TryGetValue(KnownLibraryProperties.MSBuildProjectPath, out msbuildPath))
                    {
                        var msbuildRelativePath = PathUtility.GetRelativePath(
                            project.FilePath,
                            (string)msbuildPath,
                            '/');

                        projectLib.MSBuildProject = msbuildRelativePath;
                    }

                    lockFile.Libraries.Add(projectLib);
                }
                else if (library.Type == LibraryType.Package)
                {
                    // Packages
                    var packageInfo = NuGetv3LocalRepositoryUtility.GetPackage(localRepositories, library.Name, library.Version);

                    // Add the library if it was resolved, unresolved packages are not added to the assets file.
                    if (packageInfo != null)
                    {
                        var package = packageInfo.Package;
                        var resolver = packageInfo.Repository.PathResolver;

                        var sha512 = package.Sha512;
                        var path = PathUtility.GetPathWithForwardSlashes(resolver.GetPackageDirectory(package.Id, package.Version));
                        LockFileLibrary lockFileLib = null;
                        LockFileLibrary previousLibrary = null;

                        if (previousLibraries?.TryGetValue(Tuple.Create(package.Id, package.Version), out previousLibrary) == true)
                        {
                            // Check that the previous library is still valid
                            if (previousLibrary != null
                                && StringComparer.Ordinal.Equals(path, previousLibrary.Path)
                                && StringComparer.Ordinal.Equals(sha512, previousLibrary.Sha512))
                            {
                                // We mutate this previous library so we must take a clone of it. This is
                                // important because later, when deciding whether the lock file has changed,
                                // we compare the new lock file to the previous (in-memory) lock file.
                                lockFileLib = previousLibrary.Clone();
                            }
                        }

                        // Create a new lock file library if one doesn't exist already.
                        if (lockFileLib == null)
                        {
                            lockFileLib = CreateLockFileLibrary(package, sha512, path);
                        }

                        // Create a new lock file library
                        lockFile.Libraries.Add(lockFileLib);
                    }
                }
            }

            Dictionary<Tuple<string, NuGetVersion>, LockFileLibrary> libraries = EnsureUniqueLockFileLibraries(lockFile);

            var librariesWithWarnings = new HashSet<LibraryIdentity>();

            var rootProjectStyle = project.RestoreMetadata?.ProjectStyle ?? ProjectStyle.Unknown;

            // Add the targets
            foreach (var targetGraph in targetGraphs
                .OrderBy(graph => graph.Framework.ToString(), StringComparer.Ordinal)
                .ThenBy(graph => graph.RuntimeIdentifier, StringComparer.Ordinal))
            {
                var target = new LockFileTarget
                {
                    TargetFramework = targetGraph.Framework,
                    RuntimeIdentifier = targetGraph.RuntimeIdentifier
                };

                var flattenedFlags = IncludeFlagUtils.FlattenDependencyTypes(_includeFlagGraphs, project, targetGraph);

                // Check if warnings should be displayed for the current framework.
                var tfi = project.GetTargetFramework(targetGraph.Framework);

                bool warnForImportsOnGraph = tfi.Warn
                    && (target.TargetFramework is FallbackFramework
                        || target.TargetFramework is AssetTargetFallbackFramework);

                foreach (var graphItem in targetGraph.Flattened.OrderBy(x => x.Key))
                {
                    var library = graphItem.Key;

                    // include flags
                    LibraryIncludeFlags includeFlags;
                    if (!flattenedFlags.TryGetValue(library.Name, out includeFlags))
                    {
                        includeFlags = ~LibraryIncludeFlags.ContentFiles;
                    }

                    if (library.Type == LibraryType.Project || library.Type == LibraryType.ExternalProject)
                    {
                        if (project.Name.Equals(library.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            // Do not include the project itself as a library.
                            continue;
                        }

                        var projectLib = LockFileUtils.CreateLockFileTargetProject(
                            graphItem,
                            library,
                            includeFlags,
                            targetGraph,
                            rootProjectStyle);

                        target.Libraries.Add(projectLib);
                    }
                    else if (library.Type == LibraryType.Package)
                    {
                        var packageInfo = NuGetv3LocalRepositoryUtility.GetPackage(localRepositories, library.Name, library.Version);

                        if (packageInfo == null)
                        {
                            continue;
                        }

                        var package = packageInfo.Package;
                        var libraryDependency = tfi.Dependencies.FirstOrDefault(e => e.Name.Equals(library.Name, StringComparison.OrdinalIgnoreCase));

                        (LockFileTargetLibrary targetLibrary, bool usedFallbackFramework) = LockFileUtils.CreateLockFileTargetLibrary(
                            libraryDependency?.Aliases,
                            libraries[Tuple.Create(library.Name, library.Version)],
                            package,
                            targetGraph,
                            dependencyType: includeFlags,
                            targetFrameworkOverride: null,
                            dependencies: graphItem.Data.Dependencies,
                            cache: lockFileBuilderCache);

                        target.Libraries.Add(targetLibrary);

                        // Log warnings if the target library used the fallback framework
                        if (warnForImportsOnGraph && !librariesWithWarnings.Contains(library))
                        {
                            if (target.TargetFramework is FallbackFramework)
                            {
                                // PackageTargetFallback works different from AssetTargetFallback so the warning logic for PTF cannot be optimized.
                                var nonFallbackFramework = new NuGetFramework(target.TargetFramework);

                                var targetLibraryWithoutFallback = LockFileUtils.CreateLockFileTargetLibrary(
                                    libraryDependency?.Aliases,
                                    libraries[Tuple.Create(library.Name, library.Version)],
                                    package,
                                    targetGraph,
                                    targetFrameworkOverride: nonFallbackFramework,
                                    dependencyType: includeFlags,
                                    dependencies: graphItem.Data.Dependencies,
                                    cache: lockFileBuilderCache);
                                usedFallbackFramework = !targetLibrary.Equals(targetLibraryWithoutFallback);
                            }

                            if (usedFallbackFramework)
                            {
                                var libraryName = DiagnosticUtility.FormatIdentity(library);

                                var message = string.Format(CultureInfo.CurrentCulture,
                                    Strings.Log_ImportsFallbackWarning,
                                    libraryName,
                                    GetFallbackFrameworkString(target.TargetFramework),
                                    new NuGetFramework(target.TargetFramework));

                                var logMessage = RestoreLogMessage.CreateWarning(
                                    NuGetLogCode.NU1701,
                                    message,
                                    library.Name,
                                    targetGraph.TargetGraphName);

                                _logger.Log(logMessage);

                                // only log the warning once per library
                                librariesWithWarnings.Add(library);
                            }
                        }
                    }
                }

                EnsureUniqueLockFileTargetLibraries(target);
                lockFile.Targets.Add(target);
            }

            PopulatePackageFolders(localRepositories.Select(repo => repo.RepositoryRoot).Distinct(), lockFile);

            AddCentralTransitiveDependencyGroupsForPackageReference(project, lockFile, targetGraphs);

            // Add the original package spec to the lock file.
            lockFile.PackageSpec = project;

            return lockFile;
        }

        private Dictionary<Tuple<string, NuGetVersion>, LockFileLibrary> EnsureUniqueLockFileLibraries(LockFile lockFile)
        {
            IList<LockFileLibrary> libraries = lockFile.Libraries;
            var libraryReferences = new Dictionary<Tuple<string, NuGetVersion>, LockFileLibrary>();

            foreach (LockFileLibrary lib in libraries)
            {
                var libraryKey = Tuple.Create(lib.Name, lib.Version);

                if (libraryReferences.TryGetValue(libraryKey, out LockFileLibrary existingLibrary))
                {
                    if (RankReferences(existingLibrary.Type) > RankReferences(lib.Type))
                    {
                        // Prefer project reference over package reference, so replace the the package reference.
                        libraryReferences[libraryKey] = lib;
                    }
                }
                else
                {
                    libraryReferences[libraryKey] = lib;
                }
            }

            if (lockFile.Libraries.Count != libraryReferences.Count)
            {
                lockFile.Libraries = new List<LockFileLibrary>(libraryReferences.Count);
                foreach (KeyValuePair<Tuple<string, NuGetVersion>, LockFileLibrary> pair in libraryReferences)
                {
                    lockFile.Libraries.Add(pair.Value);
                }
            }

            return libraryReferences;
        }

        private static void EnsureUniqueLockFileTargetLibraries(LockFileTarget lockFileTarget)
        {
            IList<LockFileTargetLibrary> libraries = lockFileTarget.Libraries;
            var libraryReferences = new Dictionary<LockFileTargetLibrary, LockFileTargetLibrary>(comparer: LockFileTargetLibraryNameAndVersionEqualityComparer.Instance);

            foreach (LockFileTargetLibrary library in libraries)
            {
                if (libraryReferences.TryGetValue(library, out LockFileTargetLibrary existingLibrary))
                {
                    if (RankReferences(existingLibrary.Type) > RankReferences(library.Type))
                    {
                        // Prefer project reference over package reference, so replace the the package reference.
                        libraryReferences[library] = library;
                    }
                }
                else
                {
                    libraryReferences[library] = library;
                }
            }

            if (lockFileTarget.Libraries.Count == libraryReferences.Count)
            {
                return;
            }

            lockFileTarget.Libraries = new List<LockFileTargetLibrary>(libraryReferences.Count);
            foreach (KeyValuePair<LockFileTargetLibrary, LockFileTargetLibrary> pair in libraryReferences)
            {
                lockFileTarget.Libraries.Add(pair.Value);
            }
        }

        /// <summary>
        /// Prefer projects over packages
        /// </summary>
        /// <param name="referenceType"></param>
        /// <returns></returns>
        private static int RankReferences(string referenceType)
        {
            if (referenceType == "project")
            {
                return 0;
            }
            else if (referenceType == "externalProject")
            {
                return 1;
            }
            else if (referenceType == "package")
            {
                return 2;
            }

            return 5;
        }

        private static string GetFallbackFrameworkString(NuGetFramework framework)
        {
            var frameworks = (framework as AssetTargetFallbackFramework)?.Fallback
                ?? (framework as FallbackFramework)?.Fallback
                ?? new List<NuGetFramework>();

            return string.Join(", ", frameworks);
        }

        private static void AddProjectFileDependenciesForSpec(PackageSpec project, LockFile lockFile)
        {
            // Use empty string as the key of dependencies shared by all frameworks
            lockFile.ProjectFileDependencyGroups.Add(new ProjectFileDependencyGroup(
                string.Empty,
                project.Dependencies
                    .Select(group => group.LibraryRange.ToLockFileDependencyGroupString())
                    .OrderBy(group => group, StringComparer.Ordinal)));

            foreach (var frameworkInfo in project.TargetFrameworks
                .OrderBy(framework => framework.FrameworkName.ToString(),
                    StringComparer.Ordinal))
            {
                lockFile.ProjectFileDependencyGroups.Add(new ProjectFileDependencyGroup(
                    frameworkInfo.FrameworkName.ToString(),
                    frameworkInfo.Dependencies
                        .Select(x => x.LibraryRange.ToLockFileDependencyGroupString())
                        .OrderBy(dependency => dependency, StringComparer.Ordinal)));
            }
        }

        private static void AddProjectFileDependenciesForPackageReference(PackageSpec project, LockFile lockFile, IEnumerable<RestoreTargetGraph> targetGraphs)
        {
            // For NETCore put everything under a TFM section
            // Projects are included for NETCore
            foreach (var frameworkInfo in project.TargetFrameworks
                .OrderBy(framework => framework.FrameworkName.ToString(),
                    StringComparer.Ordinal))
            {
                var dependencies = new List<LibraryRange>();
                dependencies.AddRange(project.Dependencies.Select(e => e.LibraryRange));
                dependencies.AddRange(frameworkInfo.Dependencies.Select(e => e.LibraryRange));

                var targetGraph = targetGraphs.SingleOrDefault(graph =>
                    graph.Framework.Equals(frameworkInfo.FrameworkName)
                    && string.IsNullOrEmpty(graph.RuntimeIdentifier));

                var resolvedEntry = targetGraph?
                    .Flattened
                    .SingleOrDefault(library => library.Key.Name.Equals(project.Name, StringComparison.OrdinalIgnoreCase));

                // In some failure cases where there is a conflict the root level project cannot be resolved, this should be handled gracefully
                if (resolvedEntry != null)
                {
                    dependencies.AddRange(resolvedEntry.Data.Dependencies.Where(lib =>
                        lib.LibraryRange.TypeConstraint == LibraryDependencyTarget.ExternalProject)
                        .Select(lib => lib.LibraryRange));
                }

                var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                var uniqueDependencies = new List<LibraryRange>();

                foreach (var dependency in dependencies)
                {
                    if (seen.Add(dependency.Name))
                    {
                        uniqueDependencies.Add(dependency);
                    }
                }

                // Add entry
                var dependencyGroup = new ProjectFileDependencyGroup(
                    frameworkInfo.FrameworkName.ToString(),
                    uniqueDependencies.Select(x => x.ToLockFileDependencyGroupString())
                        .OrderBy(dependency => dependency, StringComparer.Ordinal));

                lockFile.ProjectFileDependencyGroups.Add(dependencyGroup);
            }
        }

        private void AddCentralTransitiveDependencyGroupsForPackageReference(PackageSpec project, LockFile lockFile, IEnumerable<RestoreTargetGraph> targetGraphs)
        {
            if (project.RestoreMetadata == null || !project.RestoreMetadata.CentralPackageVersionsEnabled)
            {
                return;
            }

            // Do not pack anything from the runtime graphs
            // The runtime graphs are added in addition to the graphs without a runtime
            foreach (RestoreTargetGraph targetGraph in targetGraphs.Where(targetGraph => string.IsNullOrEmpty(targetGraph.RuntimeIdentifier)))
            {
                TargetFrameworkInformation targetFrameworkInformation = project.TargetFrameworks.FirstOrDefault(i => i.FrameworkName.Equals(targetGraph.Framework));

                if (targetFrameworkInformation == null)
                {
                    continue;
                }

                // The transitive dependencies enforced by the central package version management file are written to the assets to be used by the pack task.
                List<LibraryDependency> centralEnforcedTransitiveDependencies = GetLibraryDependenciesForCentralTransitiveDependencies(targetGraph, targetFrameworkInformation, project.RestoreMetadata.CentralPackageTransitivePinningEnabled).ToList();

                if (centralEnforcedTransitiveDependencies.Any())
                {
                    var centralEnforcedTransitiveDependencyGroup = new CentralTransitiveDependencyGroup
                            (
                                targetGraph.Framework,
                                centralEnforcedTransitiveDependencies
                            );

                    lockFile.CentralTransitiveDependencyGroups.Add(centralEnforcedTransitiveDependencyGroup);
                }
            }
        }

        /// <summary>
        /// Determines the <see cref="LibraryDependency" /> objects for the specified <see cref="RestoreTargetGraph" /> that represent the centrally defined transitive dependencies.
        /// </summary>
        /// <param name="targetGraph">The <see cref="RestoreTargetGraph" /> to get centrally defined transitive dependencies for.</param>
        /// <param name="targetFrameworkInformation">The <see cref="TargetFrameworkInformation" /> for the target framework to get centrally defined transitive dependencies for.</param>
        /// <param name="centralPackageTransitivePinningEnabled">A value indicating whether or not central transitive dependency version pinning is enabled.</param>
        /// <returns>An <see cref="IEnumerable{LibraryDependency}" /> representing the centrally defined transitive dependencies for the specified <see cref="RestoreTargetGraph" />.</returns>
        private IEnumerable<LibraryDependency> GetLibraryDependenciesForCentralTransitiveDependencies(RestoreTargetGraph targetGraph, TargetFrameworkInformation targetFrameworkInformation, bool centralPackageTransitivePinningEnabled)
        {
            foreach (GraphNode<RemoteResolveResult> rootNode in targetGraph.Graphs)
            {
                foreach (GraphNode<RemoteResolveResult> node in rootNode.InnerNodes)
                {
                    // Only consider nodes that are Accepted, IsCentralTransitive, and have a centrally defined package version
                    if (node?.Item == null || node.Disposition != Disposition.Accepted || !node.Item.IsCentralTransitive || !targetFrameworkInformation.CentralPackageVersions?.ContainsKey(node.Item.Key.Name) == true)
                    {
                        continue;
                    }

                    CentralPackageVersion centralPackageVersion = targetFrameworkInformation.CentralPackageVersions[node.Item.Key.Name];
                    Dictionary<string, LibraryIncludeFlags> dependenciesIncludeFlags = _includeFlagGraphs[targetGraph];

                    LibraryIncludeFlags suppressParent = LibraryIncludeFlags.None;

                    if (centralPackageTransitivePinningEnabled)
                    {
                        // Centrally pinned dependencies are not directly declared but the PrivateAssets from the top-level dependency that pulled it in should apply to it also
                        foreach (GraphNode<RemoteResolveResult> parentNode in EnumerateParentNodes(node))
                        {
                            LibraryDependency parentDependency = rootNode.Item.Data.Dependencies.FirstOrDefault(i => i.Name.Equals(parentNode.Item.Key.Name, StringComparison.OrdinalIgnoreCase));

                            // A transitive dependency that is a few levels deep won't be a top-level dependency so skip it
                            if (parentDependency == null || parentDependency.ReferenceType != LibraryDependencyReferenceType.Direct)
                            {
                                continue;
                            }

                            suppressParent |= parentDependency.SuppressParent;
                        }

                        // If all assets are suppressed then the dependency should not be added
                        if (suppressParent == LibraryIncludeFlags.All)
                        {
                            continue;
                        }
                    }

                    yield return new LibraryDependency()
                    {
                        LibraryRange = new LibraryRange(centralPackageVersion.Name, centralPackageVersion.VersionRange, LibraryDependencyTarget.Package),
                        ReferenceType = LibraryDependencyReferenceType.Transitive,
                        VersionCentrallyManaged = true,
                        IncludeType = dependenciesIncludeFlags[centralPackageVersion.Name],
                        SuppressParent = suppressParent,
                    };
                }
            }
        }

        /// <summary>
        /// Enumerates all parent nodes of the specified node.
        /// </summary>
        /// <typeparam name="T">The type of the node.</typeparam>
        /// <param name="graphNode">The <see cref="GraphNode{TItem}" /> to enumerate the parent nodes of.</param>
        /// <returns>An <see cref="IEnumerable{T}" /> containing a top down list of parent nodes of the specied node.</returns>
        private static IEnumerable<GraphNode<T>> EnumerateParentNodes<T>(GraphNode<T> graphNode)
        {
            foreach (GraphNode<T> item in graphNode.ParentNodes)
            {
                if (item.ParentNodes.Any())
                {
                    // Transitive pinned nodes have ParentNodes set
                    foreach (GraphNode<T> parentNode in EnumerateParentNodes(item))
                    {
                        yield return parentNode;
                    }
                }
                else if (item.OuterNode != null)
                {
                    // Normal transitive nodes use OuterNode to track their parent
                    foreach (GraphNode<T> outerNode in EnumerateParentNodes(item.OuterNode))
                    {
                        yield return outerNode;
                    }

                    yield return item.OuterNode;
                }

                yield return item;
            }
        }

        private static void PopulatePackageFolders(IEnumerable<string> packageFolders, LockFile lockFile)
        {
            lockFile.PackageFolders.AddRange(packageFolders.Select(path => new LockFileItem(path)));
        }

        private static LockFileLibrary CreateLockFileLibrary(LocalPackageInfo package, string sha512, string path)
        {
            var lockFileLib = new LockFileLibrary
            {
                Name = package.Id,
                Version = package.Version,
                Type = LibraryType.Package,
                Sha512 = sha512,

                // This is the relative path, appended to the global packages folder path. All
                // of the paths in the in the Files property should be appended to this path along
                // with the global packages folder path to get the absolute path to each file in the
                // package.
                Path = path
            };

            foreach (var file in package.Files)
            {
                if (!lockFileLib.HasTools && HasTools(file))
                {
                    lockFileLib.HasTools = true;
                }
                lockFileLib.Files.Add(file);
            }

            return lockFileLib;
        }

        private static bool HasTools(string file)
        {
            return file.StartsWith("tools/", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// An <see cref="IEqualityComparer{T}" /> that compares <see cref="LockFileTargetLibrary" /> objects by the value of the <see cref="LockFileTargetLibrary.Name" /> and <see cref="LockFileTargetLibrary.Version" /> properties.
        /// </summary>
        private class LockFileTargetLibraryNameAndVersionEqualityComparer : IEqualityComparer<LockFileTargetLibrary>
        {
            /// <summary>
            /// Gets a static singleton for the <see cref="LockFileTargetLibraryNameAndVersionEqualityComparer" /> class.
            /// </summary>
            public static LockFileTargetLibraryNameAndVersionEqualityComparer Instance = new();

            /// <summary>
            /// Initializes a new instance of the <see cref="LockFileTargetLibraryNameAndVersionEqualityComparer" /> class.
            /// </summary>
            private LockFileTargetLibraryNameAndVersionEqualityComparer()
            {
            }

            /// <summary>
            /// Determines whether the specified <see cref="LockFileTargetLibrary" /> objects are equal by comparing their <see cref="LockFileTargetLibrary.Name" /> and <see cref="LockFileTargetLibrary.Version" /> properties.
            /// </summary>
            /// <param name="x">The first <see cref="LockFileTargetLibrary" /> to compare.</param>
            /// <param name="y">The second <see cref="LockFileTargetLibrary" /> to compare.</param>
            /// <returns><c>true</c> if the specified <see cref="LockFileTargetLibrary" /> objects' <see cref="LockFileTargetLibrary.Name" /> and <see cref="LockFileTargetLibrary.Version" /> properties are equal, otherwise <c>false</c>.</returns>
            public bool Equals(LockFileTargetLibrary x, LockFileTargetLibrary y)
            {
                return string.Equals(x.Name, y.Name, StringComparison.Ordinal) && x.Version.Equals(y.Version);
            }

            /// <summary>
            /// Returns a hash code for the specified <see cref="LockFileTargetLibrary" /> object's <see cref="LockFileTargetLibrary.Name" /> property.
            /// </summary>
            /// <param name="obj">The <see cref="LockFileTargetLibrary" /> for which a hash code is to be returned.</param>
            /// <returns>A hash code for the specified <see cref="LockFileTargetLibrary" /> object's <see cref="LockFileTargetLibrary.Name" /> and and <see cref="LockFileTargetLibrary.Version" /> properties.</returns>
            public int GetHashCode(LockFileTargetLibrary obj)
            {
                var combiner = new HashCodeCombiner();

                combiner.AddObject(obj.Name);
                combiner.AddObject(obj.Version);

                return combiner.CombinedHash;
            }
        }
    }
}
