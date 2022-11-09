// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using NuGet.Common;
using NuGet.Packaging.Core;
using NuGet.Versioning;

namespace NuGet.Packaging
{
    // HACK: TODO: This entire class is a hack. This is in place only for back-compat reasons
    // If the version was always normalized for package directory and package file name, there would be no issue :(
    public static class PackagePathHelper
    {
        internal static IEnumerable<string> GetFiles(string root, string path, string filter, bool recursive)
        {
            path = PathUtility.EnsureTrailingSlash(Path.Combine(root, path));
            if (string.IsNullOrEmpty(filter))
            {
                filter = "*.*";
            }
            try
            {
                if (!Directory.Exists(path))
                {
                    return Enumerable.Empty<string>();
                }
                return Directory.EnumerateFiles(path, filter, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            }
            catch (UnauthorizedAccessException)
            {
            }
            catch (DirectoryNotFoundException)
            {
            }

            return Enumerable.Empty<string>();
        }

        internal static IEnumerable<string> GetDirectories(string root, string path)
        {
            try
            {
                path = PathUtility.EnsureTrailingSlash(Path.Combine(root, path));
                if (!Directory.Exists(path))
                {
                    return Enumerable.Empty<string>();
                }
                return Directory.EnumerateDirectories(path);
            }
            catch (UnauthorizedAccessException)
            {
            }
            catch (DirectoryNotFoundException)
            {
            }

            return Enumerable.Empty<string>();
        }

        private static IEnumerable<string> GetPackageFiles(string root, string filter)
        {
            filter = filter ?? "*" + PackagingCoreConstants.NupkgExtension;
            Debug.Assert(
                filter.EndsWith(PackagingCoreConstants.NupkgExtension, StringComparison.OrdinalIgnoreCase) ||
                filter.EndsWith(PackagingCoreConstants.NuspecExtension, StringComparison.OrdinalIgnoreCase));

            // Check for package files one level deep. We use this at package install time
            // to determine the set of installed packages. Installed packages are copied to 
            // {id}.{version}\{packagefile}.{extension}.
            foreach (var dir in GetDirectories(root, string.Empty))
            {
                foreach (var path in GetFiles(root, dir, filter, recursive: false))
                {
                    yield return path;
                }
            }

            // Check top level directory
            foreach (var path in GetFiles(root, string.Empty, filter, recursive: false))
            {
                yield return path;
            }
        }

        private static bool FileNameMatchesPattern(PackageIdentity packageIdentity, string path)
        {
            var packageId = packageIdentity.Id;
            var name = Path.GetFileNameWithoutExtension(path);
            NuGetVersion parsedVersion;

            // When matching by pattern, we will always have a version token. Packages without versions would be matched early on by the version-less path resolver 
            // when doing an exact match.
            return name.Length > packageId.Length &&
                   NuGetVersion.TryParse(name.Substring(packageId.Length + 1), out parsedVersion) &&
                   parsedVersion.Equals(packageIdentity.Version);
        }

        public static IEnumerable<string> GetPackageLookupPaths(PackageIdentity packageIdentity, PackagePathResolver packagePathResolver)
        {
            if (packageIdentity == null)
            {
                throw new ArgumentNullException(nameof(packageIdentity));
            }

            if (packageIdentity.Version == null)
            {
                throw new ArgumentException(
                    string.Format(CultureInfo.CurrentCulture, Strings.PropertyCannotBeNull, nameof(packageIdentity.Version)),
                    nameof(packageIdentity));
            }

            if (packagePathResolver == null)
            {
                throw new ArgumentNullException(nameof(packagePathResolver));
            }

            var packageId = packageIdentity.Id.ToLowerInvariant();
            var version = packageIdentity.Version;

            var root = packagePathResolver.Root;
            // Files created by the path resolver. This would take into account the non-side-by-side scenario 
            // and we do not need to match this for id and version.
            var packageFileName = packagePathResolver.GetPackageFileName(packageIdentity);
            var manifestFileName = Path.ChangeExtension(packageFileName, PackagingCoreConstants.NuspecExtension);
            var filesMatchingFullName = Enumerable.Concat(
                GetPackageFiles(root, packageFileName),
                GetPackageFiles(root, manifestFileName));

            if (version != null
                && version.Version.Revision < 1)
            {
                // If the build or revision number is not set, we need to look for combinations of the format
                // * Foo.1.2.nupkg
                // * Foo.1.2.3.nupkg
                // * Foo.1.2.0.nupkg
                // * Foo.1.2.0.0.nupkg
                // To achieve this, we would look for files named 1.2*.nupkg if both build and revision are 0 and
                // 1.2.3*.nupkg if only the revision is set to 0.
                var partialName = version.Version.Build < 1 ?
                    string.Join(".", packageId, version.Version.Major, version.Version.Minor) :
                    string.Join(".", packageId, version.Version.Major, version.Version.Minor, version.Version.Build);
                var partialManifestName = partialName + "*" + PackagingCoreConstants.NuspecExtension;
                partialName += "*" + PackagingCoreConstants.NupkgExtension;

                // Partial names would result is gathering package with matching major and minor but different build and revision. 
                // Attempt to match the version in the path to the version we're interested in.
                var partialNameMatches = GetPackageFiles(root, partialName).Where(path => FileNameMatchesPattern(packageIdentity, path));
                var partialManifestNameMatches = GetPackageFiles(root, partialManifestName).Where(
                    path => FileNameMatchesPattern(packageIdentity, path));
                return Enumerable.Concat(filesMatchingFullName, partialNameMatches).Concat(partialManifestNameMatches);
            }
            return filesMatchingFullName;
        }

        public static string GetInstalledPackageFilePath(PackageIdentity packageIdentity, PackagePathResolver packagePathResolver)
        {
            var packageLookupPaths = GetPackageLookupPaths(packageIdentity, packagePathResolver);
            // TODO: Not handling nuspec-only scenarios
            foreach (var packageLookupPath in packageLookupPaths)
            {
                if (packageLookupPath.EndsWith(PackagingCoreConstants.NupkgExtension, StringComparison.OrdinalIgnoreCase) &&
                    File.Exists(packageLookupPath))
                {
                    // This is an installed package lookup path which matches the packageIdentity for the given packagePathResolver
                    return packageLookupPath;
                }
            }

            return null;
        }
    }
}
