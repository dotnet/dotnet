// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NuGet.Common;
using NuGet.LibraryModel;
using NuGet.Packaging.Core;
using NuGet.ProjectModel;
using NuGet.Shared;

namespace NuGet.Commands
{
    public class PackagesLockFileBuilder
    {
        public PackagesLockFile CreateNuGetLockFile(LockFile assetsFile)
        {
            var lockFile = new PackagesLockFile(GetPackagesLockFileVersion(assetsFile));

            var libraryLookup = assetsFile.Libraries.Where(e => e.Type == LibraryType.Package)
                .ToDictionary(e => new PackageIdentity(e.Name, e.Version));

            foreach (var target in assetsFile.Targets)
            {
                var nuGettarget = new PackagesLockFileTarget()
                {
                    TargetFramework = target.TargetFramework,
                    RuntimeIdentifier = target.RuntimeIdentifier
                };

                var framework = assetsFile.PackageSpec.TargetFrameworks.FirstOrDefault(
                    f => EqualityUtility.EqualsWithNullCheck(f.FrameworkName, target.TargetFramework));

                IEnumerable<LockFileTargetLibrary> libraries = target.Libraries;

                // check if this is RID-based graph then only add those libraries which differ from original TFM.
                if (!string.IsNullOrEmpty(target.RuntimeIdentifier))
                {
                    var onlyTFM = assetsFile.Targets.First(t => EqualityUtility.EqualsWithNullCheck(t.TargetFramework, target.TargetFramework));

                    libraries = target.Libraries.Where(lib => !onlyTFM.Libraries.Any(tfmLib => tfmLib.Equals(lib)));
                }

                foreach (var library in libraries.Where(e => e.Type == LibraryType.Package))
                {
                    var identity = new PackageIdentity(library.Name, library.Version);

                    var dependency = new LockFileDependency()
                    {
                        Id = library.Name,
                        ResolvedVersion = library.Version,
                        ContentHash = libraryLookup[identity].Sha512,
                        Dependencies = library.Dependencies
                    };

                    var framework_dep = framework?.Dependencies.FirstOrDefault(
                        dep => StringComparer.OrdinalIgnoreCase.Equals(dep.Name, library.Name));

                    CentralPackageVersion centralPackageVersion = null;
                    framework?.CentralPackageVersions.TryGetValue(library.Name, out centralPackageVersion);

                    if (framework_dep != null)
                    {
                        dependency.Type = PackageDependencyType.Direct;
                        dependency.RequestedVersion = framework_dep.LibraryRange.VersionRange;
                    }

                    // The dgspec has a list of the direct dependencies and changes in the direct dependencies will invalidate the lock file
                    // A dgspec does not have information about transitive dependencies
                    // At the restore time the transitive dependencies could be pinned from central package version management file
                    // By marking them will allow to evaluate when to invalidate the packages.lock.json
                    // in cases that a central transitive version is updated, removed or added the lock file will be invalidated
                    else if (centralPackageVersion != null)
                    {
                        // This is a transitive dependency that is in the list of central dependencies.
                        dependency.Type = PackageDependencyType.CentralTransitive;
                        dependency.RequestedVersion = centralPackageVersion.VersionRange;
                    }
                    else
                    {
                        dependency.Type = PackageDependencyType.Transitive;
                    }

                    nuGettarget.Dependencies.Add(dependency);
                }

                var projectFullPaths = assetsFile.Libraries
                    .Where(l => l.Type == LibraryType.Project || l.Type == LibraryType.ExternalProject)
                    .ToDictionary(l => new PackageIdentity(l.Name, l.Version), l => l.MSBuildProject);

                foreach (var projectReference in libraries.Where(e => e.Type == LibraryType.Project || e.Type == LibraryType.ExternalProject))
                {
                    var projectIdentity = new PackageIdentity(projectReference.Name, projectReference.Version);
                    var projectFullPath = projectFullPaths[projectIdentity];
                    var id = PathUtility.GetStringComparerBasedOnOS().Equals(Path.GetFileNameWithoutExtension(projectFullPath), projectReference.Name)
                        ? projectReference.Name.ToLowerInvariant()
                        : projectReference.Name;

                    var dependency = new LockFileDependency()
                    {
                        Id = id,
                        Dependencies = projectReference.Dependencies,
                        Type = PackageDependencyType.Project
                    };

                    nuGettarget.Dependencies.Add(dependency);
                }

                nuGettarget.Dependencies = nuGettarget.Dependencies.OrderBy(d => d.Type).ToList();

                lockFile.Targets.Add(nuGettarget);
            }

            return lockFile;
        }

        private int GetPackagesLockFileVersion(LockFile assetsFile)
        {
            // Increase the version only for the projects opted-in central version management
            if (assetsFile.PackageSpec.RestoreMetadata.CentralPackageVersionsEnabled)
            {
                return PackagesLockFileFormat.PackagesLockFileVersion;
            }

            return PackagesLockFileFormat.Version;
        }
    }
}
