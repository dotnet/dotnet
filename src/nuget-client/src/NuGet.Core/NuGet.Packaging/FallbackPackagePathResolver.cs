// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using NuGet.Common;
using NuGet.Packaging.Core;
using NuGet.Versioning;

namespace NuGet.Packaging
{
    public class FallbackPackagePathResolver
    {
        private readonly List<VersionFolderPathResolver> _pathResolvers;

        /// <summary>
        /// Creates a package folder path resolver that scans multiple folders to find a package.
        /// </summary>
        /// <param name="pathContext">NuGet paths loaded from NuGet.Config settings.</param>
        public FallbackPackagePathResolver(INuGetPathContext pathContext)
            : this(pathContext?.UserPackageFolder, pathContext?.FallbackPackageFolders)
        {

        }

        public FallbackPackagePathResolver(string userPackageFolder, IEnumerable<string> fallbackPackageFolders)
        {
            if (fallbackPackageFolders == null)
            {
                throw new ArgumentNullException(nameof(fallbackPackageFolders));
            }

            if (userPackageFolder == null)
            {
                throw new ArgumentNullException(nameof(userPackageFolder));
            }

            var packageFolders = new List<string>();

            // The user's packages folder may not exist, this is expected if the fallback
            // folders contain all packages.
            if (Directory.Exists(userPackageFolder))
            {
                packageFolders.Add(userPackageFolder);
            }

            // All fallback folders must exist
            foreach (var path in fallbackPackageFolders)
            {
                if (!Directory.Exists(path))
                {
                    var message = string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.FallbackFolderNotFound,
                        path);

                    throw new PackagingException(message);
                }

                packageFolders.Add(path);
            }

            // Create path resolvers for each source.
            _pathResolvers = PathUtility.GetUniquePathsBasedOnOS(packageFolders)
                .Select(path => new VersionFolderPathResolver(path))
                .ToList();
        }

        /// <summary>
        /// Returns the root directory of an installed package.
        /// </summary>
        /// <param name="packageId">Package id.</param>
        /// <param name="version">Package version.</param>
        /// <returns>Returns the path if the package exists in any of the folders. Null if the package does not exist.</returns>
        public string GetPackageDirectory(string packageId, string version)
        {
            return GetPackageDirectory(packageId, NuGetVersion.Parse(version));
        }

        /// <summary>
        /// Returns the root directory of an installed package.
        /// </summary>
        /// <param name="packageId">Package id.</param>
        /// <param name="version">Package version.</param>
        /// <returns>Returns the path if the package exists in any of the folders. Null if the package does not exist.</returns>
        public string GetPackageDirectory(string packageId, NuGetVersion version)
        {
            // Find the package
            var info = GetPackageInfo(packageId, version);

            // Find the folder using the resolver for the folder where it was found.
            return info?.PathResolver.GetInstallPath(packageId, version);
        }

        /// <summary>
        /// Returns the package info along with a path resolver specific to the folder where the package exists.
        /// </summary>
        /// <param name="packageId">Package id.</param>
        /// <param name="version">Package version.</param>
        /// <returns>Returns the package info if the package exists in any of the folders. Null if the package does not exist.</returns>
        public FallbackPackagePathInfo GetPackageInfo(string packageId, NuGetVersion version)
        {
            if (string.IsNullOrEmpty(packageId))
            {
                throw new ArgumentException(
                    string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.StringCannotBeNullOrEmpty,
                    nameof(packageId)));
            }

            if (version == null)
            {
                throw new ArgumentNullException(nameof(version));
            }

            // Check each folder for the package.
            foreach (var resolver in _pathResolvers)
            {
                var hashPath = resolver.GetHashPath(packageId, version);

                if (File.Exists(hashPath))
                {
                    // If the hash exists we can use this path
                    return new FallbackPackagePathInfo(packageId, version, resolver);
                }

                // Perf: As hashPath is commonly found and the GetNupkgMetadataPath call is relatively
                // expensive, only request nupkgMetadataFilePath if hashPath isn't found
                var nupkgMetadataFilePath = resolver.GetNupkgMetadataPath(packageId, version);

                if (File.Exists(nupkgMetadataFilePath))
                {
                    // If the hash exists we can use this path
                    return new FallbackPackagePathInfo(packageId, version, resolver);
                }
            }

            // Not found
            return null;
        }
    }
}
