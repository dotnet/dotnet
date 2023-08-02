// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.PackageManagement.VisualStudio.Exceptions;
using NuGet.PackageManagement.VisualStudio.Utility;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.ProjectManagement.Projects;
using NuGet.ProjectModel;
using NuGet.VisualStudio.Internal.Contracts;
using TransitiveEntry = System.Collections.Generic.IDictionary<NuGet.Frameworks.FrameworkRuntimePair, System.Collections.Generic.IList<NuGet.Packaging.PackageReference>>;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// An implementation of <see cref="NuGetProject"/> that interfaces with VS project APIs to coordinate
    /// packages in a package reference style project.
    /// </summary>
    /// <remarks>Each concrete implementation is responsible of initializing <see cref="InstalledPackages"/> and <see cref="TransitivePackages"/> collections</remarks>
    /// <typeparam name="T">A collection type for Installed and Transtive packages</typeparam>
    /// <typeparam name="U">Type of the collection elements for Installed and Transitive packages</typeparam>
    public abstract class PackageReferenceProject<T, U> : BuildIntegratedNuGetProject, IPackageReferenceProject where T : ICollection<U>, new()
    {
        private static readonly NuGetFrameworkSorter FrameworkSorter = NuGetFrameworkSorter.Instance;

        private static readonly ProjectPackages EmptyProjectPackages = new(Array.Empty<PackageReference>(), Array.Empty<TransitivePackageReference>());

        private readonly protected string _projectName;
        private readonly protected string _projectUniqueName;
        private readonly protected string _projectFullPath;

        // Cache
        private protected Dictionary<string, TransitiveEntry> TransitiveOriginsCache { get; set; }
        protected T InstalledPackages { get; set; }
        protected T TransitivePackages { get; set; }

        private readonly object _installedAndTransitivePackagesLock = new object();
        private readonly object _transitiveOriginsLock = new object();

        private protected DateTime _lastTimeAssetsModified;
        private protected WeakReference<PackageSpec> _lastPackageSpec;
        private protected IList<LockFileItem> _packageFolders;

        protected bool IsInstalledAndTransitiveComputationNeeded { get; set; } = true;

        protected PackageReferenceProject(
            string projectName,
            string projectUniqueName,
            string projectFullPath)
        {
            ProjectName = projectName;
            ProjectUniqueName = projectUniqueName;
            ProjectFullPath = projectFullPath;
        }

        public override async Task<string> GetAssetsFilePathAsync()
        {
            return await GetAssetsFilePathAsync(shouldThrow: true);
        }

        public override async Task<string> GetAssetsFilePathOrNullAsync()
        {
            return await GetAssetsFilePathAsync(shouldThrow: false);
        }

        protected abstract Task<string> GetAssetsFilePathAsync(bool shouldThrow);

        public override string ProjectName { get; }
        protected string ProjectUniqueName { get; }
        protected string ProjectFullPath { get; }

        public override async Task<IReadOnlyList<PackageSpec>> GetPackageSpecsAsync(DependencyGraphCacheContext context)
        {
            (IReadOnlyList<PackageSpec> dgSpec, IReadOnlyList<IAssetsLogMessage> _) = await GetPackageSpecsAndAdditionalMessagesAsync(context);
            return dgSpec;
        }

        /// <summary>
        /// Gets the installed (top level) package references for this project. 
        /// </summary>
        public override async Task<IEnumerable<PackageReference>> GetInstalledPackagesAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            ProjectPackages packages = await GetInstalledAndTransitivePackagesAsync(includeTransitivePackages: false, includeTransitiveOrigins: false, token);
            return packages.InstalledPackages;
        }

        private async Task<(PackageSpec, string)> GetCurrentPackageSpecAndAssetsFilePathSafeAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            PackageSpec packageSpec = null;
            string assetsPath = null;
            try
            {
                (packageSpec, assetsPath) = await GetCurrentPackageSpecAndAssetsFilePathAsync(token);
            }
            catch (ProjectNotNominatedException)
            {
            }

            return (packageSpec, assetsPath);
        }

        public virtual async Task<ProjectPackages> GetInstalledAndTransitivePackagesAsync(bool includeTransitiveOrigins, CancellationToken token) => await GetInstalledAndTransitivePackagesAsync(includeTransitivePackages: true, includeTransitiveOrigins, token);

        internal async Task<ProjectPackages> GetInstalledAndTransitivePackagesAsync(bool includeTransitivePackages, bool includeTransitiveOrigins, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            (PackageSpec packageSpec, string assetsFilePath) = await GetCurrentPackageSpecAndAssetsFilePathSafeAsync(token);

            if (packageSpec == null) // null means project is not nominated
            {
                IsInstalledAndTransitiveComputationNeeded = true;

                return EmptyProjectPackages;
            }

            IList<LockFileTarget> targetsList = null;
            T installedPackages;
            T transitivePackages = default;
            Dictionary<string, TransitiveEntry> transitiveOrigins = default;
            if (IsInstalledAndTransitiveComputationNeeded)
            {
                // clear the transitive packages cache, since we don't know when a dependency has been removed
                installedPackages = new T();
                transitivePackages = new T();
                transitiveOrigins = new Dictionary<string, TransitiveEntry>();
                targetsList = await GetTargetsListAsync(assetsFilePath, token);
            }
            else
            {
                if (InstalledPackages == null)
                {
                    installedPackages = new T();
                }
                else
                {
                    // Make a copy of the caches to prevent concurrency issues.
                    lock (_installedAndTransitivePackagesLock)
                    {
                        installedPackages = GetCollectionCopy(InstalledPackages);
                    }
                }

                if (includeTransitivePackages)
                {
                    if (TransitivePackages == null)
                    {
                        transitivePackages = new T();
                        transitiveOrigins = new Dictionary<string, TransitiveEntry>();
                    }
                    else
                    {
                        // Make a copy of the caches to prevent concurrency issues.
                        lock (_installedAndTransitivePackagesLock)
                        {
                            transitivePackages = GetCollectionCopy(TransitivePackages);
                        }
                    }
                }
            }

            // get installed packages
            List<PackageReference> calculatedInstalledPackages = packageSpec
                .TargetFrameworks
                .SelectMany(f => ResolvedInstalledPackagesList(f.Dependencies, f.FrameworkName, targetsList, installedPackages))
                .GroupBy(p => p.PackageIdentity)
                .Select(g => g.OrderBy(p => p.TargetFramework, FrameworkSorter).First())
                .ToList();

            // get transitive packages
            IEnumerable<PackageReference> calculatedTransitivePackages = Enumerable.Empty<PackageReference>();
            if (includeTransitivePackages)
            {
                if (targetsList == null)
                {
                    targetsList = await GetTargetsListAsync(assetsFilePath, token);
                }

                calculatedTransitivePackages = packageSpec
                    .TargetFrameworks
                    .SelectMany(f => ResolvedTransitivePackagesList(f.FrameworkName, targetsList, installedPackages, transitivePackages))
                    .GroupBy(p => p.PackageIdentity)
                    .Select(g => g.OrderBy(p => p.TargetFramework, FrameworkSorter).First());
            }

            IEnumerable<TransitivePackageReference> transitivePackagesWithOrigins = Enumerable.Empty<TransitivePackageReference>();
            if (includeTransitivePackages)
            {
                if (includeTransitiveOrigins)
                {
                    // Compute Transitive Origins
                    if (IsInstalledAndTransitiveComputationNeeded // Cache invalidation
                        || TransitiveOriginsCache == null // If any data race left the cache as null
                        || (!TransitiveOriginsCache.Any() && calculatedTransitivePackages.Any())) // We have transitive packages, but no transitive origins and the call is requesting transitive origins
                    {
                        // Special case: Installed and Transitive lists (<see cref="InstalledPackages" />, <see cref="TransitivePackages" /> respectively) are populated,
                        // but Transitive Origins Cache <see cref="TransitiveOriginsCache" /> is not populated.
                        // Then, we need targets section from project.assets.json file on disk to populate Transitive Origins cache
                        if (targetsList == null)
                        {
                            targetsList = await GetTargetsListAsync(assetsFilePath, token);
                        }

                        transitiveOrigins = calculatedTransitivePackages.Any() ? ComputeTransitivePackageOrigins(calculatedInstalledPackages, targetsList, token) : new Dictionary<string, TransitiveEntry>();
                    }
                    else
                    {
                        lock (_transitiveOriginsLock)
                        {
                            // Make a copy of the cache to prevent concurrency issues.
                            transitiveOrigins = new Dictionary<string, TransitiveEntry>(TransitiveOriginsCache);
                        }
                    }

                    // 4. Return cached result for specific transitive dependency
                    transitivePackagesWithOrigins = calculatedTransitivePackages
                        .Select(packageRef =>
                        {
                            transitiveOrigins.TryGetValue(packageRef.PackageIdentity.Id, out TransitiveEntry cacheEntry);
                            return MergeTransitiveOrigin(packageRef, cacheEntry);
                        });

                    lock (_transitiveOriginsLock)
                    {
                        TransitiveOriginsCache = transitiveOrigins;
                    }
                }
                else
                {
                    // Get Transitive packages without Transitive Origins
                    transitivePackagesWithOrigins = calculatedTransitivePackages
                        .Select(packageRef => new TransitivePackageReference(packageRef));
                }
            }

            List<TransitivePackageReference> transitivePkgsResult = transitivePackagesWithOrigins.ToList(); // Materialize results before setting IsInstalledAndTransitiveComputationNeeded flag to false

            // Refresh cache
            lock (_installedAndTransitivePackagesLock)
            {
                InstalledPackages = installedPackages;
            }
            // if includeTransitivePackages, update the cache with the new transitive packages information
            // or if IsInstalledAndTransitiveComputationNeeded, clear the transitive packages cache, since we don't know when a dependency has been removed
            if (includeTransitivePackages || IsInstalledAndTransitiveComputationNeeded)
            {
                lock (_transitiveOriginsLock)
                {
                    TransitiveOriginsCache = transitiveOrigins;
                }
                lock (_installedAndTransitivePackagesLock)
                {
                    TransitivePackages = transitivePackages;
                }
            }

            IsInstalledAndTransitiveComputationNeeded = false;

            return new ProjectPackages(calculatedInstalledPackages, transitivePkgsResult);
        }

        protected abstract IEnumerable<PackageReference> ResolvedInstalledPackagesList(IEnumerable<LibraryDependency> libraries, NuGetFramework targetFramework, IList<LockFileTarget> targets, T installedPackages);

        protected abstract IReadOnlyList<PackageReference> ResolvedTransitivePackagesList(NuGetFramework targetFramework, IList<LockFileTarget> targets, T installedPackages, T transitivePackages);

        /// <summary>
        /// To avoid race condition, we work on copy of cache InstalledPackages and TransitivePackages.
        /// </summary>
        /// <param name="collection">Collection to copy, can be <see cref="InstalledPackages"/> or <see cref="TransitivePackages"/></param>
        /// <returns>A shallow copy of the collection</returns>
        protected abstract T GetCollectionCopy(T collection);

        /// <summary>
        /// Obtains <see cref="PackageSpec"/> object from assets file from disk
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>A <see cref="PackageSpec"/> filled from assets file on disk</returns>
        /// <remarks>Each project implementation is responsible of gathering <see cref="PackageSpec"/> info</remarks>
        protected abstract Task<PackageSpec> GetPackageSpecAsync(CancellationToken ct);

        private protected IEnumerable<PackageReference> GetPackageReferences(
            IEnumerable<LibraryDependency> libraries,
            NuGetFramework targetFramework,
            Dictionary<string, ProjectInstalledPackage> installedPackages,
            IList<LockFileTarget> targets)
        {
            return libraries
                .Where(library => (library.LibraryRange.TypeConstraint & LibraryDependencyTarget.Package) != 0)
                .Select(library => new BuildIntegratedPackageReference(library, targetFramework, GetPackageReferenceUtility.UpdateResolvedVersion(library, targetFramework, targets, installedPackages)));
        }

        private protected IReadOnlyList<PackageReference> GetTransitivePackageReferences(
            NuGetFramework targetFramework,
            Dictionary<string, ProjectInstalledPackage> installedPackages,
            Dictionary<string, ProjectInstalledPackage> transitivePackages,
            IList<LockFileTarget> targets)
        {
            // If the assets files has not been updated, return the cached transitive packages
            if (targets == null)
            {
                return transitivePackages
                    .Select(package => new PackageReference(package.Value.InstalledPackage, targetFramework))
                    .ToList();
            }
            else
            {
                return targets
                    .SelectMany(target => target.Libraries)
                    .Where(library => library.Type == LibraryType.Package)
                    .SelectMany(library => GetPackageReferenceUtility.UpdateTransitiveDependencies(library, targetFramework, targets, installedPackages, transitivePackages))
                    .Select(packageIdentity => new PackageReference(packageIdentity, targetFramework))
                    .ToList();
            }
        }

        /// <summary>
        /// Get All Installed packages that transitively install a given transitive package in this project
        /// </summary>
        /// <param name="installedPackages">The list of installed packages</param>
        /// <param name="targetsList">The list of targets</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns>A dictionary, indexed by Framework/Runtime-ID with all top (installed)
        /// packages that depends on given transitive package</returns>
        /// <remarks>Computes all transitive origins for each Framework/Runtime-ID combiation. Runtime-ID can be <c>null</c>.
        /// Transitive origins are calculated using a Depth First Search algorithm on all direct dependencies exhaustively</remarks>
        internal static Dictionary<string, TransitiveEntry> ComputeTransitivePackageOrigins(List<PackageReference> installedPackages, IList<LockFileTarget> targetsList, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            Dictionary<string, TransitiveEntry> transitiveOriginsCache = new();

            // Find all Transitive origins and update cache
            var memoryVisited = new HashSet<PackageIdentity>();

            // For each target framework graph (Framework, RID)-pair:
            foreach (LockFileTarget targetFxGraph in targetsList)
            {
                var key = new FrameworkRuntimePair(targetFxGraph.TargetFramework, targetFxGraph.RuntimeIdentifier);

                foreach (var directPkg in installedPackages) // 3.1 For each direct dependency
                {
                    memoryVisited.Clear();
                    // Do DFS to mark directPkg as a transitive origin over all transitive dependencies found
                    MarkTransitiveOrigin(transitiveOriginsCache, directPkg, directPkg, targetFxGraph, memoryVisited, key, ct);
                }
            }

            return transitiveOriginsCache;
        }

        /// <summary>
        /// Returns a <see cref="PackageSpec"/> object, either from cache or from project-system
        /// </summary>
        /// <param name="token">Cancellation token</param>
        /// <returns>An cached <see cref="PackageSpec"/> object if current object has not changed</returns>
        /// <remarks>Projects need to be NuGet-restored before calling this function</remarks>
        internal async Task<(PackageSpec, string)> GetCurrentPackageSpecAndAssetsFilePathAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            PackageSpec currentPackageSpec = await GetPackageSpecAsync(token);
            PackageSpec cachedPackageSpec = null;

            if (_lastPackageSpec != null)
            {
                _lastPackageSpec.TryGetTarget(out cachedPackageSpec);
            }

            string assetsFilePath = await GetAssetsFilePathAsync();
            var assets = new FileInfo(assetsFilePath);

            bool cacheMissAssets = (assets.Exists && assets.LastWriteTimeUtc > _lastTimeAssetsModified);

            if (cacheMissAssets || (this is CpsPackageReferenceProject && !ReferenceEquals(currentPackageSpec, cachedPackageSpec)))
            {
                _lastTimeAssetsModified = assets.LastWriteTimeUtc;
                _lastPackageSpec = new WeakReference<PackageSpec>(currentPackageSpec);
                IsInstalledAndTransitiveComputationNeeded = true;
            }

            return (currentPackageSpec, assetsFilePath);
        }

        /// <summary>
        /// Obtains targets (and packageFolders) section from project assets file (project.assets.json)
        /// </summary>
        /// <param name="ct">Cancellation token for async operation</param>
        /// <returns>A list of dependencies, indexed by framework/RID</returns>
        /// <remarks>Assets file reading occurs in a background thread</remarks>
        /// <seealso cref="GetAssetsFilePathAsync"/>
        private async ValueTask<IList<LockFileTarget>> GetTargetsListAsync(string assetsFilePath, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            await TaskScheduler.Default;

            LockFile lockFile = LockFileUtilities.GetLockFile(assetsFilePath, NullLogger.Instance);
            _packageFolders = lockFile?.PackageFolders ?? Array.Empty<LockFileItem>();

            return lockFile?.Targets;
        }

        /// <summary>
        /// Runs Depth First Search recursively to mark current and dependend nodes with top dependency
        /// </summary>
        /// <param name="top">Top, Direct dependency</param>
        /// <param name="current">Current package/node to visit</param>
        /// <param name="graph">Package dependency graph, from assets file</param>
        /// <param name="visited">Dictionary to remember visited nodes</param>
        /// <param name="fxRidEntry">Framework/Runtime-ID associated with current <paramref name="graph"/></param>
        private static void MarkTransitiveOrigin(Dictionary<string, TransitiveEntry> transitiveOriginsCache, PackageReference top, PackageReference current, LockFileTarget graph, HashSet<PackageIdentity> visited, FrameworkRuntimePair fxRidEntry, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            LockFileTargetLibrary node = default;

            // Find first target node that matches current
            foreach (LockFileTargetLibrary lib in graph.Libraries)
            {
                if (lib.Type == LibraryType.Package.Value
                    && string.Equals(lib.Name, current.PackageIdentity.Id, StringComparison.OrdinalIgnoreCase)
                    && ((current.HasAllowedVersions && current.AllowedVersions.Satisfies(lib.Version)) ||
                        (current.PackageIdentity.HasVersion && current.PackageIdentity.Version.Equals(lib.Version))))
                {
                    node = lib;
                    break;
                }
            }

            if (node != default)
            {
                visited.Add(current.PackageIdentity); // visited

                // Lookup Transitive Origins Cache
                TransitiveEntry cachedEntry;
                if (!transitiveOriginsCache.TryGetValue(current.PackageIdentity.Id, out cachedEntry))
                {
                    cachedEntry = new Dictionary<FrameworkRuntimePair, IList<PackageReference>>
                    {
                        [fxRidEntry] = new List<PackageReference>()
                    };
                }

                if (!cachedEntry.ContainsKey(fxRidEntry))
                {
                    cachedEntry[fxRidEntry] = new List<PackageReference>();
                }

                if (!cachedEntry[fxRidEntry].Contains(top)) // Dictionary value is a List. If perf. is bad, change to HashSet.
                {
                    cachedEntry[fxRidEntry].Add(top);
                }

                // Upsert Transitive Origins Cache
                transitiveOriginsCache[current.PackageIdentity.Id] = cachedEntry;

                foreach (PackageDependency dep in node.Dependencies.ToList()) // Casting to list to prevent backing allocations
                {
                    // Create PackageReference object as a data-model based on dependency
                    var pkgChild = new PackageReference(
                        identity: new PackageIdentity(dep.Id, dep.VersionRange.MinVersion),
                        targetFramework: fxRidEntry.Framework,
                        userInstalled: false,
                        developmentDependency: false,
                        requireReinstallation: false,
                        allowedVersions: dep.VersionRange);

                    if (!visited.Contains(pkgChild.PackageIdentity))
                    {
                        MarkTransitiveOrigin(transitiveOriginsCache, top, pkgChild, graph, visited, fxRidEntry, token);
                    }
                }
            }
        }

        internal static TransitivePackageReference MergeTransitiveOrigin(PackageReference currentPackage, TransitiveEntry transitiveEntry)
        {
            var transitiveOrigins = new SortedSet<PackageReference>(GetPackageReferenceUtility.PackageReferenceMergeComparer);
            transitiveEntry?.Keys.ForEach(key =>
            {
                if (currentPackage.TargetFramework == null || key.Framework == currentPackage.TargetFramework)
                {
                    transitiveOrigins.AddRange(transitiveEntry[key]);
                }
            });

            List<PackageReference> merged = transitiveOrigins
                .GroupBy(tr => tr.PackageIdentity.Id)
                .Select(g => g.OrderByDescending(pr => pr.PackageIdentity.Version).First())
                .ToList();

            var transitivePR = new TransitivePackageReference(currentPackage)
            {
                TransitiveOrigins = merged,
            };

            return transitivePR;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyCollection<string>> GetPackageFoldersAsync(CancellationToken ct)
        {
            (PackageSpec packageSpec, string assetsFilePath) = await GetCurrentPackageSpecAndAssetsFilePathSafeAsync(ct);

            if (packageSpec == null)
            {
                return Array.Empty<string>();
            }

            if (IsInstalledAndTransitiveComputationNeeded)
            {
                await GetTargetsListAsync(assetsFilePath, ct);
            }

            return _packageFolders.Select(pf => pf.Path).ToList();
        }
    }
}
