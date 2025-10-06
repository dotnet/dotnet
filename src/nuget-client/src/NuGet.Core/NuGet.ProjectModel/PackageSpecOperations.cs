// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging.Core;
using NuGet.Versioning;

namespace NuGet.ProjectModel
{
    public static class PackageSpecOperations
    {
        /// <summary>
        /// Add or Update the dependencies in the spec. If the package exists in any of the dependencies list, only those will be updated.
        /// If the package does not exist in any of dependencies lists,
        /// if the <see cref="ProjectRestoreMetadata.ProjectStyle" /> is <see cref="ProjectStyle.PackageReference"/>
        /// then the <see cref="TargetFrameworkInformation"/> will be updated,
        /// otherwise, the generic dependencies will be updated.
        /// </summary>
        /// <param name="spec">PackageSpec to update. Cannot be <see langword="null"/></param>
        /// <param name="dependency">Dependency to add. Cannot be <see langword="null"/> </param>
        /// <exception cref="ArgumentNullException"> If <paramref name="spec"/> or <paramref name="dependency"/> is <see langword="null"/> </exception>
        public static void AddOrUpdateDependency(PackageSpec spec, PackageDependency dependency)
        {
            if (spec == null) throw new ArgumentNullException(nameof(spec));
            if (dependency == null) throw new ArgumentNullException(nameof(dependency));

            var foundExistingDependency = false;
            var range = dependency.VersionRange;
            var dependencyId = dependency.Id;

            for (var i = 0; i < spec.Dependencies.Count; i++)
            {
                var existingDependency = spec.Dependencies[i];

                bool updateVersionOverride = spec.RestoreMetadata?.CentralPackageVersionsEnabled == true && !existingDependency.VersionCentrallyManaged && existingDependency.VersionOverride is not null;

                if (IsMatchingDependencyName(existingDependency, dependencyId))
                {
                    var libraryRange = new LibraryRange(existingDependency.LibraryRange) { VersionRange = range };
                    spec.Dependencies[i] = new LibraryDependency(existingDependency) { LibraryRange = libraryRange, VersionOverride = updateVersionOverride ? range : null };

                    foundExistingDependency = true;
                }
            }

            for (var i = 0; i < spec.TargetFrameworks.Count; i++)
            {
                var targetFramework = spec.TargetFrameworks[i];

                // Don't allocate a new dependencies array if there aren't any matching dependencies
                if (!targetFramework.Dependencies.Any(dep => IsMatchingDependencyName(dep, dependencyId)))
                {
                    continue;
                }

                foundExistingDependency = true;
                var newDependencies = new LibraryDependency[targetFramework.Dependencies.Length];
                for (var j = 0; j < targetFramework.Dependencies.Length; j++)
                {
                    var existingDependency = targetFramework.Dependencies[j];
                    var libraryRange = existingDependency.LibraryRange;

                    if (IsMatchingDependencyName(existingDependency, dependencyId))
                    {
                        libraryRange = new LibraryRange(libraryRange) { VersionRange = range };
                        existingDependency = new LibraryDependency(existingDependency) { LibraryRange = libraryRange };
                    }

                    newDependencies[j] = existingDependency;
                }

                var newDependenciesImmutable = ImmutableCollectionsMarshal.AsImmutableArray(newDependencies);
                spec.TargetFrameworks[i] = new TargetFrameworkInformation(targetFramework) { Dependencies = newDependenciesImmutable };
            }

            if (!foundExistingDependency)
            {
                if (spec.RestoreMetadata?.ProjectStyle == ProjectStyle.PackageReference) // PackageReference does not use the `Dependencies` list in the PackageSpec.
                {
                    for (var i = 0; i < spec.TargetFrameworks.Count; i++)
                    {
                        var framework = spec.TargetFrameworks[i];
                        var newDependency = CreateDependency(dependencyId, range, spec.RestoreMetadata?.CentralPackageVersionsEnabled ?? false);

                        var newDependencies = framework.Dependencies.Add(newDependency);
                        spec.TargetFrameworks[i] = new TargetFrameworkInformation(framework) { Dependencies = newDependencies };
                    }
                }
                else
                {
                    var newDependency = CreateDependency(dependencyId, range, spec.RestoreMetadata?.CentralPackageVersionsEnabled ?? false);

                    spec.Dependencies.Add(newDependency);
                }
            }
        }

        /// <summary>
        /// Add or Update the dependencies in the spec. If the package exists in any of the dependencies list, only those will be updated.
        /// If the package does not exist in any of dependencies lists,
        /// if the <see cref="ProjectRestoreMetadata.ProjectStyle" /> is <see cref="ProjectStyle.PackageReference"/>
        /// then the <see cref="TargetFrameworkInformation"/> will be updated,
        /// otherwise, the generic dependencies will be updated.
        /// </summary>
        /// <param name="spec">PackageSpec to update. Cannot be <see langword="null"/></param>
        /// <param name="identity">Dependency to add. Cannot be <see langword="null"/> </param>
        /// <exception cref="ArgumentNullException"> If <paramref name="spec"/> or <paramref name="identity"/> is <see langword="null"/> </exception>
        public static void AddOrUpdateDependency(PackageSpec spec, PackageIdentity identity)
        {
            if (spec == null) throw new ArgumentNullException(nameof(spec));
            if (identity == null) throw new ArgumentNullException(nameof(identity));

            AddOrUpdateDependency(spec, new PackageDependency(identity.Id, new VersionRange(identity.Version)));
        }

        public static bool HasPackage(PackageSpec spec, string packageId)
        {
            if (spec.Dependencies.Any(library => IsMatchingDependencyName(library, packageId)))
            {
                return true;
            }

            if (spec.TargetFrameworks.Any(tf => tf.Dependencies.Any(library => IsMatchingDependencyName(library, packageId))))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Add or Update the dependencies in the spec. Only the frameworks specified will be considered. 
        /// </summary>
        /// <param name="spec">PackageSpec to update. Cannot be <see langword="null"/></param>
        /// <param name="dependency">Dependency to add. Cannot be <see langword="null"/> </param>
        /// <param name="frameworksToAdd">The frameworks to be considered. If <see langword="null"/>, then all frameworks will be considered. </param>
        /// <exception cref="ArgumentNullException"> If <paramref name="spec"/> or <paramref name="dependency"/> is <see langword="null"/> </exception>
        public static void AddOrUpdateDependency(
            PackageSpec spec,
            PackageDependency dependency,
            IEnumerable<NuGetFramework> frameworksToAdd)
        {
            if (spec == null) throw new ArgumentNullException(nameof(spec));
            if (dependency == null) throw new ArgumentNullException(nameof(dependency));

            for (var i = 0; i < spec.TargetFrameworks.Count; i++)
            {
                var targetFramework = spec.TargetFrameworks[i];
                if (frameworksToAdd == null || frameworksToAdd.Contains(targetFramework.FrameworkName))
                {
                    var newDependencies = AddOrUpdateDependencyInDependencyList(spec, targetFramework.Dependencies, dependency.Id, dependency.VersionRange);
                    spec.TargetFrameworks[i] = new TargetFrameworkInformation(targetFramework) { Dependencies = newDependencies };
                }
            }

            if (spec.RestoreMetadata?.CentralPackageVersionsEnabled ?? false)
            {
                for (var i = 0; i < spec.TargetFrameworks.Count; i++)
                {
                    var targetFramework = spec.TargetFrameworks[i];
                    if (frameworksToAdd == null || frameworksToAdd.Contains(targetFramework.FrameworkName))
                    {
                        var newCentralPackageVersion = new KeyValuePair<string, CentralPackageVersion>(dependency.Id, new CentralPackageVersion(dependency.Id, dependency.VersionRange));
                        var newCentralPackageVersionsEnum = targetFramework.CentralPackageVersions
                            .Where(kvp => !string.Equals(kvp.Key, dependency.Id, StringComparison.OrdinalIgnoreCase))
                            .Append(newCentralPackageVersion);
                        var newCentralPackageVersions = CreateCentralPackageVersions(newCentralPackageVersionsEnum);

                        spec.TargetFrameworks[i] = new TargetFrameworkInformation(targetFramework) { CentralPackageVersions = newCentralPackageVersions };
                    }
                }
            }
        }

        static IReadOnlyDictionary<string, CentralPackageVersion> CreateCentralPackageVersions(IEnumerable<KeyValuePair<string, CentralPackageVersion>> versions)
        {
            Dictionary<string, CentralPackageVersion> result = new Dictionary<string, CentralPackageVersion>(StringComparer.OrdinalIgnoreCase);
            foreach (var kvp in versions)
            {
                result.Add(kvp.Key, kvp.Value);
            }

            return result;
        }

        /// <summary>
        /// Add or Update the dependencies in the spec. Only the frameworks specified will be considered. 
        /// </summary>
        /// <param name="spec">PackageSpec to update. Cannot be <see langword="null"/></param>
        /// <param name="identity">Dependency to add. Cannot be <see langword="null"/> </param>
        /// <param name="frameworksToAdd">The frameworks to be considered. If <see langword="null"/>, then all frameworks will be considered. </param>
        /// <exception cref="ArgumentNullException"> If <paramref name="spec"/> or <paramref name="identity"/> is <see langword="null"/> </exception>
        public static void AddOrUpdateDependency(
            PackageSpec spec,
            PackageIdentity identity,
            IEnumerable<NuGetFramework> frameworksToAdd)
        {
            if (spec == null) throw new ArgumentNullException(nameof(spec));
            if (identity == null) throw new ArgumentNullException(nameof(identity));

            AddOrUpdateDependency(spec, new PackageDependency(identity.Id, new VersionRange(identity.Version)), frameworksToAdd);
        }

        public static void RemoveDependency(
            PackageSpec spec,
            string packageId)
        {
            if (spec == null) throw new ArgumentNullException(nameof(spec));
            if (packageId == null) throw new ArgumentNullException(nameof(packageId));

            for (var i = spec.Dependencies.Count - 1; i >= 0; i--)
            {
                var dependency = spec.Dependencies[i];
                if (IsMatchingDependencyName(dependency, packageId))
                {
                    spec.Dependencies.RemoveAt(i);
                }
            }

            for (var i = 0; i < spec.TargetFrameworks.Count; i++)
            {
                var framework = spec.TargetFrameworks[i];
                var matchingDependencyCount = framework.Dependencies.Count(dep => IsMatchingDependencyName(dep, packageId));
                if (matchingDependencyCount == 0)
                {
                    continue;
                }

                var remainingDependencies = new LibraryDependency[framework.Dependencies.Length - matchingDependencyCount];
                var dependencyIndex = 0;
                foreach (var dep in framework.Dependencies)
                {
                    if (!IsMatchingDependencyName(dep, packageId))
                    {
                        remainingDependencies[dependencyIndex++] = dep;
                    }
                }

                var remainingDependenciesImmutable = ImmutableCollectionsMarshal.AsImmutableArray(remainingDependencies);
                spec.TargetFrameworks[i] = new TargetFrameworkInformation(framework) { Dependencies = remainingDependenciesImmutable };
            }
        }

        private static bool IsMatchingDependencyName(LibraryDependency dependency, string dependencyName)
        {
            return StringComparer.OrdinalIgnoreCase.Equals(dependency.Name, dependencyName);
        }

        private static ImmutableArray<LibraryDependency> AddOrUpdateDependencyInDependencyList(
            PackageSpec spec,
            ImmutableArray<LibraryDependency> list,
            string packageId,
            VersionRange range)
        {
            var existingDependency = list.Any(dep => IsMatchingDependencyName(dep, packageId));

            if (existingDependency)
            {
                var result = new LibraryDependency[list.Length];
                for (var i = 0; i < list.Length; i++)
                {
                    var libraryDependency = list[i];
                    if (IsMatchingDependencyName(libraryDependency, packageId))
                    {
                        var libraryRange = new LibraryRange(libraryDependency.LibraryRange) { VersionRange = range };
                        libraryDependency = new LibraryDependency(libraryDependency) { LibraryRange = libraryRange };
                    }

                    result[i] = libraryDependency;
                }

                return ImmutableCollectionsMarshal.AsImmutableArray(result);
            }
            else
            {
                var newDependency = CreateDependency(packageId, range, spec.RestoreMetadata?.CentralPackageVersionsEnabled ?? false);

                return list.Add(newDependency);
            }
        }

        private static LibraryDependency CreateDependency(
            string packageId,
            VersionRange range,
            bool centralPackageVersionsEnabled)
        {
            return new LibraryDependency()
            {
                LibraryRange = new LibraryRange(packageId, range, LibraryDependencyTarget.Package),
                VersionCentrallyManaged = centralPackageVersionsEnabled
            };
        }
    }
}
