// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectModel;
using NuGet.Versioning;

namespace NuGet.PackageManagement.VisualStudio.Utility
{
    internal static class GetPackageReferenceUtility
    {
        internal static readonly Comparer<PackageReference> PackageReferenceMergeComparer = Comparer<PackageReference>.Create(ComparePackageReferenceByIdentity);

        /// <summary>
        /// Compares the project and the assets files returning the installed package.
        /// Assets information can be null and returns the package from the project files.
        /// Checks if package already exists in the project and return it, otherwise update the project installed packages.
        /// </summary>
        /// <param name="projectLibrary">Library from the project file.</param>
        /// <param name="targetFramework">Target framework from the project file.</param>
        /// <param name="targets">Target assets file with the package information.</param>
        /// <param name="installedPackages">Installed packages information from the project.</param>
        internal static PackageIdentity UpdateResolvedVersion(LibraryDependency projectLibrary, NuGetFramework targetFramework, IReadOnlyList<LockFileTarget> targets, Dictionary<string, ProjectInstalledPackage> installedPackages)
        {
            NuGetVersion resolvedVersion = default;

            // Returns the installed version if the package:
            // 1. Already exists in the installedPackages
            // 2. The range is the same as the installed one
            // 3. There are no changes in the assets file
            if (installedPackages.TryGetValue(projectLibrary.Name, out ProjectInstalledPackage installedVersion) && installedVersion.AllowedVersions.Equals(projectLibrary.LibraryRange.VersionRange) && targets == null)
            {
                return installedVersion.InstalledPackage;
            }

            resolvedVersion = GetInstalledVersion(projectLibrary.Name, targetFramework, targets);

            if (resolvedVersion == null)
            {
                resolvedVersion = projectLibrary.LibraryRange?.VersionRange?.MinVersion ?? new NuGetVersion(0, 0, 0);
            }

            // Add or update the the version of the package in the project
            if (installedPackages.TryGetValue(projectLibrary.Name, out ProjectInstalledPackage installedPackage))
            {
                installedPackages[projectLibrary.Name] = new ProjectInstalledPackage(projectLibrary.LibraryRange.VersionRange, new PackageIdentity(projectLibrary.Name, resolvedVersion));
            }
            else
            {
                ProjectInstalledPackage newInstalledPackage = new ProjectInstalledPackage(projectLibrary.LibraryRange.VersionRange, new PackageIdentity(projectLibrary.Name, resolvedVersion));
                installedPackages.Add(projectLibrary.Name, newInstalledPackage);
            }

            return new PackageIdentity(projectLibrary.Name, resolvedVersion);
        }

        /// <summary>
        /// Gets the dependencies of a top level package and caches these if the assets file has changed
        /// </summary>
        /// <param name="library">Library from the project file.</param>
        /// <param name="targetFramework">Target framework from the project file.</param>
        /// <param name="targets">Target assets file with the package information.</param>
        /// <param name="installedPackages">Cached installed package information</param>
        /// <param name="transitivePackages">Cached transitive package information</param>
        internal static IReadOnlyList<PackageIdentity> UpdateTransitiveDependencies(LockFileTargetLibrary library, NuGetFramework targetFramework, IReadOnlyList<LockFileTarget> targets, Dictionary<string, ProjectInstalledPackage> installedPackages, Dictionary<string, ProjectInstalledPackage> transitivePackages)
        {
            NuGetVersion resolvedVersion = default;

            var packageIdentities = new List<PackageIdentity>();

            // get the dependencies for this target framework
            IReadOnlyList<PackageDependency> transitiveDependencies = GetTransitivePackagesForLibrary(library, targetFramework, targets);

            if (transitiveDependencies != null)
            {
                foreach (PackageDependency package in transitiveDependencies)
                {
                    // don't add transitive packages if they are also top level packages
                    if (!installedPackages.ContainsKey(package.Id))
                    {
                        resolvedVersion = GetInstalledVersion(package.Id, targetFramework, targets);

                        if (resolvedVersion == null)
                        {
                            resolvedVersion = package.VersionRange?.MinVersion ?? new NuGetVersion(0, 0, 0);
                        }

                        var packageIdentity = new PackageIdentity(package.Id, resolvedVersion);
                        transitivePackages[package.Id] = new ProjectInstalledPackage(package.VersionRange, packageIdentity);
                        packageIdentities.Add(packageIdentity);
                    }
                }
            }

            return packageIdentities;
        }

        private static NuGetVersion GetInstalledVersion(string libraryName, NuGetFramework targetFramework, IReadOnlyList<LockFileTarget> targets)
        {
            // PERF: Intentionally avoiding LINQ and foreach to avoid allocating capture classes and enumerators
            if (targets != null)
            {
                LockFileTarget target = default;
                for (int i = 0; i < targets.Count; ++i)
                {
                    LockFileTarget t = targets[i];
                    if (t.TargetFramework.Equals(targetFramework) && string.IsNullOrEmpty(t.RuntimeIdentifier))
                    {
                        target = t;
                        break;
                    }
                }

                if (target != null && target.Libraries != null)
                {
                    for (int i = 0; i < target.Libraries.Count; ++i)
                    {
                        LockFileTargetLibrary a = target.Libraries[i];
                        if (a != null && a.Name.Equals(libraryName, StringComparison.OrdinalIgnoreCase))
                        {
                            return a.Version;
                        }
                    }
                }
            }

            return null;
        }

        private static IReadOnlyList<PackageDependency> GetTransitivePackagesForLibrary(LockFileTargetLibrary library, NuGetFramework targetFramework, IReadOnlyList<LockFileTarget> targets)
        {
            // PERF: Intentionally avoiding LINQ and foreach to avoid allocating capture classes and enumerators
            if (targets != null)
            {
                LockFileTarget target = default;
                for (int i = 0; i < targets.Count; ++i)
                {
                    LockFileTarget t = targets[i];
                    if (t.TargetFramework.Equals(targetFramework) && string.IsNullOrEmpty(t.RuntimeIdentifier))
                    {
                        target = t;
                        break;
                    }
                }

                if (target != null && target.Libraries != null)
                {
                    var packageDependencies = new List<PackageDependency>();
                    for (int i = 0; i < target.Libraries.Count; ++i)
                    {
                        LockFileTargetLibrary lib = target.Libraries[i];
                        if (lib.Name.Equals(library.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            if (lib.Dependencies != null)
                            {
                                packageDependencies.AddRange(lib.Dependencies);
                            }
                        }
                    }

                    return packageDependencies;
                }
            }

            return null;
        }

        internal static int ComparePackageReferenceByIdentity(PackageReference a, PackageReference b)
        {
            if (a?.PackageIdentity == null && b?.PackageIdentity == null)
            {
                return 0;
            }

            if (a?.PackageIdentity != null && b?.PackageIdentity == null)
            {
                return 1;
            }

            if (a?.PackageIdentity == null && b?.PackageIdentity != null)
            {
                return -1;
            }

            return a.PackageIdentity.CompareTo(b.PackageIdentity);
        }
    }
}
