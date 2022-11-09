// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging.Core;
using NuGet.Versioning;

namespace NuGet.ProjectModel
{
    public static class PackageSpecOperations
    {
        public static void AddOrUpdateDependency(PackageSpec spec, PackageDependency dependency)
        {
            var existing = GetExistingDependencies(spec, dependency.Id);

            var range = dependency.VersionRange;

            foreach (var existingDependency in existing)
            {
                existingDependency.LibraryRange.VersionRange = range;
            }

            if (!existing.Any())
            {
                AddDependency(spec.Dependencies, dependency.Id, range, spec.RestoreMetadata?.CentralPackageVersionsEnabled ?? false);
            }
        }

        public static void AddOrUpdateDependency(PackageSpec spec, PackageIdentity identity)
        {
            AddOrUpdateDependency(spec, new PackageDependency(identity.Id, new VersionRange(identity.Version)));
        }

        public static bool HasPackage(PackageSpec spec, string packageId)
        {
            return GetExistingDependencies(spec, packageId).Any();
        }

        public static void AddOrUpdateDependency(
            PackageSpec spec,
            PackageDependency dependency,
            IEnumerable<NuGetFramework> frameworksToAdd)
        {
            var lists = GetDependencyLists(
                spec,
                includeGenericDependencies: false,
                frameworksToConsider: frameworksToAdd);

            foreach (var list in lists)
            {
                AddOrUpdateDependencyInDependencyList(spec, list, dependency.Id, dependency.VersionRange);
            }

            foreach (IDictionary<string, CentralPackageVersion> centralPackageVersionList in GetCentralPackageVersionLists(spec, frameworksToAdd))
            {
                centralPackageVersionList[dependency.Id] = new CentralPackageVersion(dependency.Id, dependency.VersionRange);
            }
        }

        public static void AddOrUpdateDependency(
            PackageSpec spec,
            PackageIdentity identity,
            IEnumerable<NuGetFramework> frameworksToAdd)
        {
            AddOrUpdateDependency(spec, new PackageDependency(identity.Id, new VersionRange(identity.Version)), frameworksToAdd);
        }

        public static void RemoveDependency(
            PackageSpec spec,
            string packageId)
        {
            var lists = GetDependencyLists(
                spec,
                includeGenericDependencies: true,
                frameworksToConsider: null);

            foreach (var list in lists)
            {
                var matchingDependencies = list
                    .Where(e => StringComparer.OrdinalIgnoreCase.Equals(e.Name, packageId))
                    .ToList();

                foreach (var dependency in matchingDependencies)
                {
                    list.Remove(dependency);
                }
            }
        }

        /// <summary>
        /// Get the list of dependencies in the package spec. Unless null is provided, the
        /// <paramref name="frameworksToConsider"/> set can be used to get the dependency lists for only for the
        /// provided target frameworks. If null is provided, all framework dependency lists are returned.
        /// </summary>
        /// <param name="spec">The package spec.</param>
        /// <param name="includeGenericDependencies">
        /// Whether or not the generic dependency list should be returned (dependencies that apply to all target
        /// frameworks.
        /// </param>
        /// <param name="frameworksToConsider">The frameworks to consider.</param>
        /// <returns>The sequence of dependency lists.</returns>
        private static IEnumerable<IList<LibraryDependency>> GetDependencyLists(
            PackageSpec spec,
            IEnumerable<NuGetFramework> frameworksToConsider,
            bool includeGenericDependencies)
        {
            if (includeGenericDependencies)
            {
                yield return spec.Dependencies;
            }

            foreach (var targetFramework in spec.TargetFrameworks)
            {
                if (frameworksToConsider == null || frameworksToConsider.Contains(targetFramework.FrameworkName))
                {
                    yield return targetFramework.Dependencies;
                }
            }
        }

        private static IEnumerable<IDictionary<string, CentralPackageVersion>> GetCentralPackageVersionLists(
            PackageSpec spec,
            IEnumerable<NuGetFramework> frameworksToConsider)
        {
            if (spec.RestoreMetadata?.CentralPackageVersionsEnabled ?? false)
            {
                foreach (var targetFramework in spec.TargetFrameworks)
                {
                    if (frameworksToConsider == null || frameworksToConsider.Contains(targetFramework.FrameworkName))
                    {
                        yield return targetFramework.CentralPackageVersions;
                    }
                }
            }
        }

        private static List<LibraryDependency> GetExistingDependencies(PackageSpec spec, string packageId)
        {
            return GetDependencyLists(spec, frameworksToConsider: null, includeGenericDependencies: true)
                    .SelectMany(list => list)
                    .Where(library => StringComparer.OrdinalIgnoreCase.Equals(library.Name, packageId))
                    .ToList();
        }

        private static void AddOrUpdateDependencyInDependencyList(
            PackageSpec spec,
            IList<LibraryDependency> list,
            string packageId,
            VersionRange range)
        {

            var dependencies = list.Where(e => StringComparer.OrdinalIgnoreCase.Equals(e.Name, packageId)).ToList();

            if (dependencies.Count != 0)
            {
                foreach (var library in dependencies)
                {
                    library.LibraryRange.VersionRange = range;
                }
            }
            else
            {
                AddDependency(list, packageId, range, spec.RestoreMetadata?.CentralPackageVersionsEnabled ?? false);
            }

        }

        private static void AddDependency(
            IList<LibraryDependency> list,
            string packageId,
            VersionRange range,
            bool centralPackageVersionsEnabled)
        {
            var dependency = new LibraryDependency
            {
                LibraryRange = new LibraryRange(packageId, range, LibraryDependencyTarget.Package),
                VersionCentrallyManaged = centralPackageVersionsEnabled
            };

            list.Add(dependency);
        }
    }
}
