// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.DependencyResolver;
using NuGet.LibraryModel;
using NuGet.ProjectModel;

namespace NuGet.Commands
{
    internal sealed partial class DependencyGraphResolver
    {
        /// <summary>
        /// A class representing a declared dependency graph item to consider for the resolved graph.
        /// </summary>
        [DebuggerDisplay("{LibraryDependency}, DependencyIndex={LibraryDependencyIndex}, RangeIndex={LibraryRangeIndex}")]
        public class DependencyGraphItem
        {
            /// <summary>
            /// Gets or initializes a <see cref="Task{TResult}" /> that returns a <see cref="GraphItem{TItem}" /> containing a <see cref="RemoteResolveResult" /> that represents the resolved graph item after looking it up in the configured feeds.
            /// </summary>
            public required Task<GraphItem<RemoteResolveResult>> FindLibraryTask { get; init; }

            /// <summary>
            /// Gets or initializes a value indicating whether or not this dependency graph item is a transitively pinned dependency.
            /// </summary>
            public bool IsCentrallyPinnedTransitivePackage { get; init; }

            /// <summary>
            /// Gets or initializes a value indicating whether or not this dependency graph item is a direct package reference from the root project.
            /// </summary>
            public bool IsRootPackageReference { get; init; }

            /// <summary>
            /// Gets or initializes the <see cref="LibraryDependency" /> used to declare this dependency graph item.
            /// </summary>
            public required LibraryDependency LibraryDependency { get; init; }

            /// <summary>
            /// Gets or initializes the <see cref="LibraryDependencyIndex" /> associated with this dependency graph item.
            /// </summary>
            public LibraryDependencyIndex LibraryDependencyIndex { get; init; }

            /// <summary>
            /// Gets or initializes the <see cref="LibraryRangeIndex" /> associated with this dependency graph item.
            /// </summary>
            public LibraryRangeIndex LibraryRangeIndex { get; init; }

            /// <summary>
            /// Gets or initializes the <see cref="LibraryRangeIndex" /> of this dependency graph item's parent.
            /// </summary>
            public LibraryRangeIndex Parent { get; init; }

            /// <summary>
            /// Gets or initializes an array representing all of the parent <see cref="LibraryRangeIndex" /> values of this dependency graph item.
            /// </summary>
            public LibraryRangeIndex[] Path { get; init; } = Array.Empty<LibraryRangeIndex>();

            /// <summary>
            /// Gets or initializes a <see cref="HashSet{T}" /> containing <see cref="LibraryDependency" /> objects representing any runtime specific dependencies.
            /// </summary>
            public HashSet<LibraryDependency>? RuntimeDependencies { get; init; }

            /// <summary>
            /// Gets or initializes a <see cref="HashSet{T}" /> containing <see cref="LibraryDependencyIndex" /> values representing any libraries that should be suppressed under this dependency graph item.
            /// </summary>
            public HashSet<LibraryDependencyIndex>? Suppressions { get; init; }

            /// <summary>
            /// Gets the <see cref="GraphItem{TItem}" /> with the <see cref="RemoteResolveResult" /> of this dependency graph item from calling the delegate specified in <see cref="FindLibraryTask" />.
            /// </summary>
            /// <param name="projectRestoreMetadata">The <see cref="ProjectRestoreMetadata" /> of the current restore.</param>
            /// <param name="packagesToPrune">An optional <see cref="IReadOnlyDictionary{TKey, TValue}" /> containing any packages to prune from the defined dependencies.</param>
            /// <param name="isRootProject">Indicates whether or not the dependency graph item is the root project.</param>
            /// <param name="logger">An <see cref="ILogger" /> used for logging.</param>
            /// <returns>A <see cref="GraphItem{TItem}" /> with the <see cref="RemoteResolveResult" /> of the result of finding the library.</returns>
            public async Task<GraphItem<RemoteResolveResult>> GetGraphItemAsync(
                ProjectRestoreMetadata projectRestoreMetadata,
                IReadOnlyDictionary<string, PrunePackageReference>? packagesToPrune,
                bool isRootProject,
                ILogger logger)
            {
                // Call the task to get the library, this may returned a cached result
                GraphItem<RemoteResolveResult> item = await FindLibraryTask;

                // If there are no runtime dependencies or packages to prune, just return the original result
                if ((RuntimeDependencies == null || RuntimeDependencies.Count == 0) && (packagesToPrune == null || packagesToPrune.Count == 0))
                {
                    return item;
                }

                // Create a new list that is big enough for the existing items plus any runtime dependencies
                List<LibraryDependency> dependencies = new(capacity: RuntimeDependencies?.Count ?? 0 + item.Data.Dependencies.Count);

                // Loop through the defined dependencies, leaving out any pruned packages or runtime packages that replace them
                for (int i = 0; i < item.Data.Dependencies.Count; i++)
                {
                    LibraryDependency dependency = item.Data.Dependencies[i];

                    // Skip any packages that should be pruned or will be replaced with a runtime dependency
                    if (ShouldPrunePackage(projectRestoreMetadata, packagesToPrune, dependency, item.Key, isRootProject, logger)
                        || RuntimeDependencies?.Contains(dependency) == true)
                    {
                        continue;
                    }

                    dependencies.Add(dependency);
                }

                if (RuntimeDependencies?.Count > 0)
                {
                    // Add any runtime dependencies unless they should be pruned
                    foreach (LibraryDependency runtimeDependency in RuntimeDependencies)
                    {
                        if (ShouldPrunePackage(projectRestoreMetadata, packagesToPrune, runtimeDependency, item.Key, isRootProject, logger) == true)
                        {
                            continue;
                        }

                        dependencies.Add(runtimeDependency);
                    }
                }

                // Return a new graph item containing the mutated list of dependencies
                return new GraphItem<RemoteResolveResult>(item.Key)
                {
                    Data = new RemoteResolveResult()
                    {
                        Dependencies = dependencies,
                        Match = item.Data.Match
                    }
                };
            }

            /// <summary>
            /// Determines whether or not a package should be pruned from the list of defined dependencies.
            /// </summary>
            /// <remarks>
            /// A package is not pruned if it is a project reference or a direct package reference from the root project.
            /// </remarks>
            /// <param name="projectRestoreMetadata">The <see cref="ProjectRestoreMetadata" /> of the current restore.</param>
            /// <param name="packagesToPrune">An optional <see cref="IReadOnlyDictionary{TKey, TValue}" /> containing any packages to prune from the defined dependencies.</param>
            /// <param name="dependency">The <see cref="LibraryDependency" /> of the defined dependency.</param>
            /// <param name="parentLibrary">The <see cref="LibraryIdentity" /> of the parent library that defined this dependency.</param>
            /// <param name="isRootProject">Indicates if the parent library is the root project.</param>
            /// <param name="logger">An <see cref="ILogger" /> used for logging.</param>
            /// <returns><see langword="true" /> if the package should be pruned, otherwise <see langword="false" />.</returns>
            private static bool ShouldPrunePackage(
                ProjectRestoreMetadata projectRestoreMetadata,
                IReadOnlyDictionary<string, PrunePackageReference>? packagesToPrune,
                LibraryDependency dependency,
                LibraryIdentity parentLibrary,
                bool isRootProject,
                ILogger logger)
            {
                if (packagesToPrune?.TryGetValue(dependency.Name, out PrunePackageReference? packageToPrune) != true
                    || !dependency.LibraryRange!.VersionRange!.Satisfies(packageToPrune!.VersionRange!.MaxVersion!))
                {
                    // There is either no packages to prune or the package to prune does not match the version range of the dependency
                    return false;
                }

                bool isPackage = dependency.LibraryRange?.TypeConstraintAllows(LibraryDependencyTarget.Package) == true;

                if (!isPackage)
                {
                    if (SdkAnalysisLevelMinimums.IsEnabled(
                        projectRestoreMetadata.SdkAnalysisLevel,
                        projectRestoreMetadata.UsingMicrosoftNETSdk,
                        SdkAnalysisLevelMinimums.PruningWarnings))
                    {
                        logger.Log(RestoreLogMessage.CreateWarning(NuGetLogCode.NU1511, string.Format(CultureInfo.CurrentCulture, Strings.Error_RestorePruningProjectReference, dependency.Name)));
                    }

                    return false;
                }

                if (isRootProject)
                {
                    if (SdkAnalysisLevelMinimums.IsEnabled(
                        projectRestoreMetadata.SdkAnalysisLevel,
                        projectRestoreMetadata.UsingMicrosoftNETSdk,
                        SdkAnalysisLevelMinimums.PruningWarnings))
                    {
                        logger.Log(RestoreLogMessage.CreateWarning(NuGetLogCode.NU1510, string.Format(CultureInfo.CurrentCulture, Strings.Error_RestorePruningDirectPackageReference, dependency.Name)));
                    }

                    return false;
                }

                logger.LogDebug(string.Format(CultureInfo.CurrentCulture, Strings.RestoreDebugPruningPackageReference, $"{dependency.Name} {dependency.LibraryRange!.VersionRange.OriginalString}", parentLibrary, packageToPrune.VersionRange.MaxVersion));

                return true;
            }
        }
    }
}
