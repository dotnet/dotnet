// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Packaging.Core;
using NuGet.Versioning;

namespace NuGet.Packaging
{
    public static class PackageHelper
    {
        private static readonly string[] ExcludePaths = new[]
        {
            "_rels/",
            "package/",
            @"_rels\",
            @"package\",
            "[Content_Types].xml"
        };

        private static readonly char[] Slashes = new char[] { '/', '\\' };

        public static bool IsAssembly(string path)
        {
            return path.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) ||
                   path.EndsWith(".winmd", StringComparison.OrdinalIgnoreCase) ||
                   path.EndsWith(".exe", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsNuspec(string path)
        {
            return path.EndsWith(PackagingCoreConstants.NuspecExtension, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsManifest(string path)
        {
            return IsRoot(path) && IsNuspec(path);
        }

        public static bool IsRoot(string path)
        {
            // True if the path contains no directory slashes.
            return path.IndexOfAny(Slashes) == -1;
        }

        public static bool IsPackageFile(string packageFileName, PackageSaveMode packageSaveMode)
        {
            if (string.IsNullOrEmpty(packageFileName)
                || string.IsNullOrEmpty(Path.GetFileName(packageFileName)))
            {
                // This is to ignore archive entries that are not really files
                return false;
            }

            if (IsManifest(packageFileName))
            {
                return (packageSaveMode & PackageSaveMode.Nuspec) == PackageSaveMode.Nuspec;
            }

            if ((packageSaveMode & PackageSaveMode.Files) == PackageSaveMode.Files)
            {
                return !ExcludePaths.Any(p =>
                    packageFileName.StartsWith(p, StringComparison.OrdinalIgnoreCase)) &&
                    !IsNuGetGeneratedFile(packageFileName);
            }

            return false;
        }

        private static bool IsNuGetGeneratedFile(string path)
        {
            return path.EndsWith(PackagingCoreConstants.HashFileExtension, StringComparison.OrdinalIgnoreCase) ||
                path.EndsWith(PackagingCoreConstants.NupkgMetadataFileExtension, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// A package is deemed to be a satellite package if it has a language property set, the id of the package is
        /// of the format [.*].[Language]
        /// and it has at least one dependency with an id that maps to the runtime package .
        /// </summary>
        private static async Task<SatellitePackageInfo> GetSatellitePackageInfoAsync(
            IAsyncPackageCoreReader packageReader,
            CancellationToken cancellationToken)
        {
            // A satellite package has the following properties:
            //     1) A package suffix that matches the package's language, with a dot preceding it
            //     2) A dependency on the package with the same Id minus the language suffix
            //     3) The dependency can be found by Id in the repository (as its path is needed for installation)
            // Example: foo.ja-jp, with a dependency on foo

            var nuspec = await packageReader.GetNuspecAsync(cancellationToken);
            var nuspecReader = new NuspecReader(nuspec);
            var packageId = nuspecReader.GetId();
            var packageLanguage = nuspecReader.GetLanguage();
            string localRuntimePackageId = null;
            PackageIdentity runtimePackageIdentity = null;

            if (!string.IsNullOrEmpty(packageLanguage)
                && packageId.EndsWith('.' + packageLanguage, StringComparison.OrdinalIgnoreCase))
            {
                // The satellite pack's Id is of the format <Core-Package-Id>.<Language>. Extract the core package id using this.
                // Additionally satellite packages have a strict dependency on the core package
                localRuntimePackageId = packageId.Substring(0, packageId.Length - packageLanguage.Length - 1);

                foreach (var group in nuspecReader.GetDependencyGroups())
                {
                    foreach (var dependencyPackage in group.Packages)
                    {
                        if (dependencyPackage.Id.Equals(localRuntimePackageId, StringComparison.OrdinalIgnoreCase)
                            && dependencyPackage.VersionRange != null
                            && dependencyPackage.VersionRange.MaxVersion == dependencyPackage.VersionRange.MinVersion
                            && dependencyPackage.VersionRange.IsMaxInclusive
                            && dependencyPackage.VersionRange.IsMinInclusive)
                        {
                            var runtimePackageVersion = new NuGetVersion(dependencyPackage.VersionRange.MinVersion.ToNormalizedString());
                            runtimePackageIdentity = new PackageIdentity(dependencyPackage.Id, runtimePackageVersion);
                        }
                    }
                }
            }

            return new SatellitePackageInfo(runtimePackageIdentity != null, packageLanguage, runtimePackageIdentity);
        }

        public static async Task<Tuple<string, IEnumerable<string>>> GetSatelliteFilesAsync(
            PackageReaderBase packageReader,
            PackagePathResolver packagePathResolver,
            CancellationToken cancellationToken)
        {
            var satelliteFileEntries = new List<string>();
            string runtimePackageDirectory = null;

            var result = await GetSatellitePackageInfoAsync(packageReader, cancellationToken);

            if (result.IsSatellitePackage)
            {
                // Now, we know that the package is a satellite package and that the runtime package is 'runtimePackageId'
                // Check, if the runtimePackage is installed and get the folder to copy over files

                var runtimePackageFilePath = packagePathResolver.GetInstalledPackageFilePath(result.RuntimePackageIdentity);
                if (File.Exists(runtimePackageFilePath))
                {
                    // Existence of the package file is the validation that the package exists
                    runtimePackageDirectory = Path.GetDirectoryName(runtimePackageFilePath);
                    satelliteFileEntries.AddRange(await packageReader.GetSatelliteFilesAsync(result.PackageLanguage, cancellationToken));
                }
            }

            return new Tuple<string, IEnumerable<string>>(runtimePackageDirectory, satelliteFileEntries);
        }

        /// <summary>
        /// This returns all the installed package files (does not include satellite files)
        /// </summary>
        public static async Task<IEnumerable<ZipFilePair>> GetInstalledPackageFilesAsync(
            PackageArchiveReader packageReader,
            PackageIdentity packageIdentity,
            PackagePathResolver packagePathResolver,
            PackageSaveMode packageSaveMode,
            CancellationToken cancellationToken)
        {
            var installedPackageFiles = Enumerable.Empty<ZipFilePair>();

            var packageDirectory = packagePathResolver.GetInstalledPath(packageIdentity);
            if (!string.IsNullOrEmpty(packageDirectory))
            {
                var packageFiles = await packageReader.GetPackageFilesAsync(packageSaveMode, cancellationToken);
                var entries = packageReader.EnumeratePackageEntries(packageFiles, packageDirectory);
                installedPackageFiles = entries.Where(e => e.IsInstalled());
            }

            return installedPackageFiles.ToList();
        }

        public static async Task<Tuple<string, IEnumerable<ZipFilePair>>> GetInstalledSatelliteFilesAsync(
            PackageArchiveReader packageReader,
            PackagePathResolver packagePathResolver,
            PackageSaveMode packageSaveMode,
            CancellationToken cancellationToken)
        {
            var installedSatelliteFiles = Enumerable.Empty<ZipFilePair>();

            var result = await GetSatelliteFilesAsync(packageReader, packagePathResolver, cancellationToken);
            var runtimePackageDirectory = result.Item1;
            var satelliteFiles = result.Item2;

            if (satelliteFiles.Any())
            {
                var satelliteFileEntries = packageReader.EnumeratePackageEntries(
                    satelliteFiles.Where(f => IsPackageFile(f, packageSaveMode)),
                    runtimePackageDirectory);
                installedSatelliteFiles = satelliteFileEntries.Where(e => e.IsInstalled());
            }

            return new Tuple<string, IEnumerable<ZipFilePair>>(runtimePackageDirectory, installedSatelliteFiles.ToList());
        }

        private sealed class SatellitePackageInfo
        {
            public bool IsSatellitePackage { get; }
            public string PackageLanguage { get; }
            public PackageIdentity RuntimePackageIdentity { get; }

            internal SatellitePackageInfo(
                bool isSatellitePackage,
                string packageLanguage,
                PackageIdentity runtimePackageIdentity)
            {
                IsSatellitePackage = isSatellitePackage;
                PackageLanguage = packageLanguage;
                RuntimePackageIdentity = runtimePackageIdentity;
            }
        }
    }
}
