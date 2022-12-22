// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.LibraryModel;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement.Projects;
using NuGet.ProjectModel;

namespace NuGet.ProjectManagement
{
    /// <summary>
    /// Utilities for project.json
    /// </summary>
    public static class BuildIntegratedProjectUtility
    {
        /// <summary>
        /// Orders all package dependencies in a project.
        /// Project must be restored.
        /// </summary>
        public static async Task<IReadOnlyList<PackageIdentity>> GetOrderedProjectPackageDependencies(
            BuildIntegratedNuGetProject buildIntegratedProject)
        {
            var lockFile = await GetLockFileOrNull(buildIntegratedProject);

            if (lockFile != null)
            {
                return GetOrderedLockFilePackageDependencies(lockFile);
            }

            return new List<PackageIdentity>();
        }

        /// <summary>
        /// Read lock file
        /// </summary>
        public static async Task<LockFile> GetLockFileOrNull(BuildIntegratedNuGetProject buildIntegratedProject)
        {
            var lockFilePath = await buildIntegratedProject.GetAssetsFilePathOrNullAsync();

            if (lockFilePath == null)
            {
                return null;
            }

            return GetLockFileOrNull(lockFilePath);
        }

        /// <summary>
        /// Read lock file
        /// </summary>
        public static LockFile GetLockFileOrNull(string lockFilePath)
        {
            LockFile lockFile = null;
            var lockFileFormat = new LockFileFormat();

            // Read the lock file to find the full closure of dependencies
            if (File.Exists(lockFilePath))
            {
                lockFile = lockFileFormat.Read(lockFilePath);
            }

            return lockFile;
        }

        /// <summary>
        /// Lock file dependencies - packages only
        /// </summary>
        public static IReadOnlyList<PackageIdentity> GetOrderedLockFilePackageDependencies(LockFile lockFile)
        {
            return GetOrderedLockFileDependencies(lockFile)
                .Where(library => library.Type == LibraryType.Package)
                .Select(library => new PackageIdentity(library.Name, library.Version))
                .ToList();
        }

        /// <summary>
        /// Get ordered dependencies from the lock file
        /// </summary>
        /// <param name="lockFile"></param>
        /// <returns></returns>
        public static IReadOnlyList<LibraryIdentity> GetOrderedLockFileDependencies(LockFile lockFile)
        {
            var results = new List<LibraryIdentity>();

            var dependencies = new HashSet<PackageDependencyInfo>(PackageIdentity.Comparer);
            var typeMappings = new Dictionary<PackageDependencyInfo, LibraryIdentity>(PackageIdentity.Comparer);

            foreach (var target in lockFile.Targets)
            {
                foreach (var targetLibrary in target.Libraries)
                {
                    var identity = new PackageIdentity(targetLibrary.Name, targetLibrary.Version);
                    var dependency = new PackageDependencyInfo(identity, targetLibrary.Dependencies);
                    dependencies.Add(dependency);

                    if (!typeMappings.ContainsKey(dependency))
                    {
                        var libraryIdentity = new LibraryIdentity(
                            targetLibrary.Name,
                            targetLibrary.Version,
                            LibraryType.Parse(targetLibrary.Type));

                        typeMappings.Add(dependency, libraryIdentity);
                    }
                }
            }

            // Sort dependencies
            var sortedDependencies = TopologicalSortUtility.SortPackagesByDependencyOrder(dependencies);

            foreach (var dependency in sortedDependencies)
            {
                // Convert back
                // PackageDependencyInfo -> LibraryIdentity
                results.Add(typeMappings[dependency]);
            }

            return results;
        }
    }
}
