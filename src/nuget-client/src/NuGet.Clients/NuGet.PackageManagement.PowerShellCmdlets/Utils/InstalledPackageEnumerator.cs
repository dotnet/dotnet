// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.VisualStudio.Threading;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.PackageManagement;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.ProjectManagement.Projects;
using NuGet.ProjectModel;
using NuGet.Protocol.Core.Types;
using NuGet.Resolver;

namespace NuGetConsole.Host.PowerShell
{
    /// <summary>
    /// Helper class collecting installed packages in all supported projects in topological order.
    /// </summary>
    internal class InstalledPackageEnumerator
    {
        private readonly ISolutionManager _solutionManager;
        private readonly ISettings _settings;
        private readonly Func<BuildIntegratedNuGetProject, Task<LockFile>> _getLockFileOrNullAsync;

        /// <summary>
        /// Represents an installed package item.
        /// </summary>
        public class PackageItem : IEquatable<PackageItem>
        {
            public PackageIdentity Identity { get; }
            public string InstallPath { get; }

            public PackageItem(PackageIdentity identity, string installPath)
            {
                Assumes.NotNull(identity);
                Assumes.NotNullOrEmpty(installPath);

                Identity = identity;
                InstallPath = installPath;
            }

            public bool Equals(PackageItem other)
            {
                if (other == null)
                {
                    return false;
                }

                if (object.ReferenceEquals(this, other))
                {
                    return true;
                }

                if (!PackageIdentityComparer.Default.Equals(Identity, other.Identity))
                {
                    return false;
                }

                if (!StringComparer.OrdinalIgnoreCase.Equals(InstallPath, other.InstallPath))
                {
                    return false;
                }

                return true;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as PackageItem);
            }

            public override int GetHashCode()
            {
                var hash = 0x11;
                hash = hash * 0x1F + PackageIdentityComparer.Default.GetHashCode(Identity);
                hash = hash * 0x1F + StringComparer.OrdinalIgnoreCase.GetHashCode(InstallPath);

                return hash;
            }
        }

        public InstalledPackageEnumerator(
            ISolutionManager solutionManager,
            ISettings settings)
        {
            Assumes.Present(solutionManager);
            Assumes.Present(settings);

            _solutionManager = solutionManager;
            _settings = settings;
            _getLockFileOrNullAsync = BuildIntegratedProjectUtility.GetLockFileOrNull;
        }

        /// <summary>
        /// This constructor is used for creating a test instance
        /// </summary>
        internal InstalledPackageEnumerator(
            ISolutionManager solutionManager,
            ISettings settings,
            Func<BuildIntegratedNuGetProject, Task<LockFile>> getLockFileOrNullAsync)
        {
            Assumes.Present(solutionManager);
            Assumes.Present(settings);

            _solutionManager = solutionManager;
            _settings = settings;
            _getLockFileOrNullAsync = getLockFileOrNullAsync ?? BuildIntegratedProjectUtility.GetLockFileOrNull;
        }

        public async Task<IEnumerable<PackageItem>> EnumeratePackagesAsync(
            NuGetPackageManager packageManager,
            CancellationToken token)
        {
            Assumes.Present(packageManager);

            // invoke init.ps1 files in the order of package dependency.
            // if A -> B, we invoke B's init.ps1 before A's.
            var installedPackages = new List<PackageItem>();

            // Sort projects by type
            var projectLookup = (await _solutionManager.GetNuGetProjectsAsync())
                .ToLookup(p => p is BuildIntegratedNuGetProject);

            // Each id/version should only be executed once
            var finishedPackages = new HashSet<PackageIdentity>();

            // Packages.config projects
            await ProcessPackagesConfigProjectsAsync(
                projectLookup[false],
                packageManager,
                finishedPackages,
                installedPackages,
                token);

            // build integrated projects
            foreach (var project in projectLookup[true].Cast<BuildIntegratedNuGetProject>())
            {
                await CollectPackagesForBuildIntegratedProjectAsync(
                    project,
                    finishedPackages,
                    installedPackages,
                    token);
            }

            return installedPackages;
        }

        private async Task ProcessPackagesConfigProjectsAsync(
            IEnumerable<NuGetProject> projects,
            NuGetPackageManager packageManager,
            ISet<PackageIdentity> finishedPackages,
            List<PackageItem> installedPackages,
            CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var packagesByFramework = new Dictionary<NuGetFramework, ISet<PackageIdentity>>();

            foreach (var project in projects)
            {
                var result = await CollectPackagesForPackagesConfigAsync(project, token);

                ISet<PackageIdentity> frameworkPackages;
                if (!packagesByFramework.TryGetValue(result.Item1, out frameworkPackages))
                {
                    frameworkPackages = new HashSet<PackageIdentity>();
                    packagesByFramework.Add(result.Item1, frameworkPackages);
                }

                frameworkPackages.UnionWith(result.Item2);
            }

            if (packagesByFramework.Count > 0)
            {
                await OrderPackagesForPackagesConfigAsync(
                    packageManager,
                    packagesByFramework,
                    finishedPackages,
                    installedPackages,
                    token);
            }
        }

        private async Task<Tuple<NuGetFramework, IEnumerable<PackageIdentity>>> CollectPackagesForPackagesConfigAsync(
            NuGetProject project,
            CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            // Read packages.config
            var installedRefs = await project.GetInstalledPackagesAsync(token);

            if (installedRefs?.Any() == true)
            {
                // Index packages.config references by target framework since this affects dependencies
                NuGetFramework targetFramework;
                if (!project.TryGetMetadata(NuGetProjectMetadataKeys.TargetFramework, out targetFramework))
                {
                    targetFramework = NuGetFramework.AnyFramework;
                }

                return Tuple.Create(targetFramework, installedRefs.Select(reference => reference.PackageIdentity));
            }

            return Tuple.Create(NuGetFramework.AnyFramework, Enumerable.Empty<PackageIdentity>());
        }

        private async Task OrderPackagesForPackagesConfigAsync(
            NuGetPackageManager packageManager,
            IDictionary<NuGetFramework, ISet<PackageIdentity>> packagesConfigInstalled,
            ISet<PackageIdentity> finishedPackages,
            IList<PackageItem> installedPackages,
            CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            // Get the path to the Packages folder.
            var packagesFolderPath = packageManager.PackagesFolderSourceRepository.PackageSource.Source;
            var packagePathResolver = new PackagePathResolver(Path.GetFullPath(packagesFolderPath));

            var packagesToSort = new HashSet<ResolverPackage>();
            var resolvedPackages = new HashSet<PackageIdentity>();

            var dependencyInfoResource = await packageManager
                .PackagesFolderSourceRepository
                .GetResourceAsync<DependencyInfoResource>();

            using (var sourceCacheContext = new SourceCacheContext())
            {
                // Order by the highest framework first to make this deterministic
                // Process each framework/id/version once to avoid duplicate work
                // Packages may have different dependency orders depending on the framework, but there is 
                // no way to fully solve this across an entire solution so we make a best effort here.
                foreach ((var framework, var packageIdentities) in packagesConfigInstalled.OrderByDescending(fw => fw.Key, NuGetFrameworkSorter.Instance))
                {
                    foreach (var package in packageIdentities)
                    {
                        if (resolvedPackages.Add(package))
                        {
                            var dependencyInfo = await dependencyInfoResource.ResolvePackage(
                                package,
                                framework,
                                sourceCacheContext,
                                NullLogger.Instance,
                                token);

                            // This will be null for unrestored packages
                            if (dependencyInfo != null)
                            {
                                packagesToSort.Add(new ResolverPackage(dependencyInfo, listed: true, absent: false));
                            }
                        }
                    }
                }
            }

            token.ThrowIfCancellationRequested();

            // Order packages by dependency order
            var sortedPackages = ResolverUtility.TopologicalSort(packagesToSort);

            foreach (var package in sortedPackages)
            {
                if (!finishedPackages.Contains(package))
                {
                    var installPath = packagePathResolver.GetInstalledPath(package);
                    if (!string.IsNullOrEmpty(installPath))
                    {
                        installedPackages.Add(new PackageItem(package, installPath));
                        finishedPackages.Add(package);
                    }
                }
            }
        }

        private async Task CollectPackagesForBuildIntegratedProjectAsync(
            BuildIntegratedNuGetProject project,
            ISet<PackageIdentity> finishedPackages,
            IList<PackageItem> installedPackages,
            CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var lockFile = await _getLockFileOrNullAsync(project);

            if (lockFile == null)
            {
                return;
            }

            if (lockFile.Libraries == null ||
                lockFile.Libraries.Count == 0)
            {
                return;
            }

            FallbackPackagePathResolver fppr;

            if ((lockFile?.PackageFolders?.Count ?? 0) != 0)
            {
                // The user packages folder is always the first package folder. Subsequent package folders are always
                // fallback package folders.
                var packageFolders = lockFile
                    .PackageFolders
                    .Select(lockFileItem => lockFileItem.Path);

                var userPackageFolder = packageFolders.First();
                var fallbackPackageFolders = packageFolders.Skip(1);

                fppr = new FallbackPackagePathResolver(userPackageFolder, fallbackPackageFolders);
            }
            else
            {
                var pathContext = NuGetPathContext.Create(_settings);
                fppr = new FallbackPackagePathResolver(pathContext);
            }

            foreach (var package in BuildIntegratedProjectUtility.GetOrderedLockFilePackageDependencies(lockFile))
            {
                if (!finishedPackages.Contains(package))
                {
                    var installPath = fppr.GetPackageDirectory(package.Id, package.Version);
                    if (!string.IsNullOrEmpty(installPath))
                    {
                        installedPackages.Add(new PackageItem(package, installPath));
                        finishedPackages.Add(package);
                    }
                }
            }
        }
    }
}
