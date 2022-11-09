// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using System.Threading;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace NuGet.Protocol
{
    public static class LocalFolderUtility
    {
        // *.nupkg
        private static readonly string NupkgFilter = $"*{NuGetConstants.PackageExtension}";

        /// <summary>
        /// Retrieve a nupkg using the path.
        /// </summary>
        /// <param name="path">Nupkg path in uri form.</param>
        public static LocalPackageInfo GetPackage(Uri path, ILogger log)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }

            var file = GetAndVerifyFileInfo(path);

            return GetPackageFromNupkg(file);
        }

        /// <summary>
        /// Retrieve all packages from a folder and one level deep.
        /// </summary>
        /// <param name="root">Nupkg folder directory path.</param>
        public static IEnumerable<LocalPackageInfo> GetPackagesV2(string root, ILogger log)
        {
            return GetPackagesV2(root, log, CancellationToken.None);
        }

        /// <summary>
        /// Retrieve all packages from a folder and one level deep.
        /// </summary>
        /// <param name="root">Folder path</param>
        /// <param name="log">Logger</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public static IEnumerable<LocalPackageInfo> GetPackagesV2(string root, ILogger log, CancellationToken cancellationToken)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }

            return GetPackagesFromNupkgs(GetNupkgsFromFlatFolder(root, log, cancellationToken), cancellationToken);
        }

        /// <summary>
        /// Retrieve all packages of an id from a v2 folder.
        /// </summary>
        /// <param name="root">Nupkg folder directory path.</param>
        /// <param name="id">Package id.</param>
        public static IEnumerable<LocalPackageInfo> GetPackagesV2(string root, string id, ILogger log)
        {
            return GetPackagesV2(root, id, log, CancellationToken.None);
        }

        /// <summary>
        /// Retrieve all packages of an id from a v2 folder.
        /// </summary>
        /// <param name="root">Nupkg folder directory path.</param>
        /// <param name="id">Package id.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public static IEnumerable<LocalPackageInfo> GetPackagesV2(string root, string id, ILogger log, CancellationToken cancellationToken)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }

            foreach (var package in GetPackagesFromNupkgs(GetNupkgsFromFlatFolder(root, id, log, cancellationToken), cancellationToken))
            {
                // Filter out any packages that were incorrectly identified
                // Ex: id: packageA.1 version: 1.0 -> packageA.1.1.0 -> packageA 1.1.0
                if (StringComparer.OrdinalIgnoreCase.Equals(id, package.Identity.Id))
                {
                    yield return package;
                }
            }

            yield break;
        }

        /// <summary>
        /// Retrieve all packages of an id from a v2 folder.
        /// </summary>
        /// <param name="root">Nupkg folder directory path.</param>
        /// <param name="id">Package id.</param>
        /// <param name="version">Package version.</param>
        public static LocalPackageInfo GetPackageV2(string root, string id, NuGetVersion version, ILogger log)
        {
            return GetPackageV2(root, id, version, log, CancellationToken.None);
        }

        /// <summary>
        /// Retrieve all packages of an id from a v2 folder.
        /// </summary>
        /// <param name="root">Nupkg folder directory path.</param>
        /// <param name="id">Package id.</param>
        /// <param name="version">Package version.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public static LocalPackageInfo GetPackageV2(string root, string id, NuGetVersion version, ILogger log, CancellationToken cancellationToken)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (version == null)
            {
                throw new ArgumentNullException(nameof(version));
            }

            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }

            return GetPackageV2(root, new PackageIdentity(id, version), log, cancellationToken);
        }

        /// <summary>
        /// Retrieve all packages of an id from a v2 folder.
        /// </summary>
        /// <param name="root">Nupkg folder directory path.</param>
        /// <param name="identity">Package id and version.</param>
        public static LocalPackageInfo GetPackageV2(string root, PackageIdentity identity, ILogger log)
        {
            return GetPackageV2(root, identity, log, CancellationToken.None);
        }

        /// <summary>
        /// Retrieve all packages of an id from a v2 folder.
        /// </summary>
        /// <param name="root">Nupkg folder directory path.</param>
        /// <param name="identity">Package id and version.</param>
        /// /// <param name="cancellationToken">Cancellation token</param>
        public static LocalPackageInfo GetPackageV2(string root, PackageIdentity identity, ILogger log, CancellationToken cancellationToken)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }

            // Verify the root path is a valid path.
            var rootDirInfo = GetAndVerifyRootDirectory(root);

            // Search directories starting with the top directory for any package matching the identity
            // If multiple packages are found in the same directory that match (ex: 1.0, 1.0.0.0)
            // then favor the exact non-normalized match. If no exact match is found take the first
            // using the file system sort order. This is to match the legacy nuget 2.8.x behavior.
            foreach (var directoryList in GetNupkgsFromFlatFolderChunked(rootDirInfo, log, cancellationToken))
            {
                LocalPackageInfo fallbackMatch = null;

                // Check for any files that are in the form packageId.version.nupkg
                foreach (var file in directoryList.Where(file => IsPossiblePackageMatch(file, identity)))
                {
                    var package = GetPackageFromNupkg(file);

                    if (identity.Equals(package.Identity))
                    {
                        if (StringComparer.OrdinalIgnoreCase.Equals(
                            identity.Version.ToString(),
                            package.Identity.Version.ToString()))
                        {
                            // Take an exact match immediately
                            return package;
                        }
                        else if (fallbackMatch == null)
                        {
                            // This matches the identity, but there may be an exact match still
                            fallbackMatch = package;
                        }
                    }
                }

                if (fallbackMatch != null)
                {
                    // Use the fallback match if an exact match was not found
                    return fallbackMatch;
                }
            }

            // Not found
            return null;
        }

        /// <summary>
        /// Retrieve a package with an id and version from a packages.config packages folder.
        /// </summary>
        /// <param name="root">Nupkg folder directory path.</param>
        public static IEnumerable<LocalPackageInfo> GetPackagesConfigFolderPackages(
            string root,
            ILogger log)
        {
            return GetPackagesConfigFolderPackages(root, log, CancellationToken.None);
        }

        /// <summary>
        /// Retrieve a package with an id and version from a packages.config packages folder.
        /// </summary>
        /// <param name="root">Nupkg folder directory path.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public static IEnumerable<LocalPackageInfo> GetPackagesConfigFolderPackages(
            string root,
            ILogger log,
            CancellationToken cancellationToken)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }

            var rootDirInfo = GetAndVerifyRootDirectory(root);

            // Find the matching nupkg for each sub directory.
            if (rootDirInfo.Exists)
            {
                foreach (var dir in GetDirectoriesSafe(rootDirInfo, log, cancellationToken))
                {
                    var package = GetPackagesConfigFolderPackage(dir, log);

                    // Ensure that the nupkg file exists
                    if (package != null)
                    {
                        yield return package;
                    }
                }
            }

            yield break;
        }

        /// <summary>
        /// Retrieve a package with an id and version from a packages.config packages folder.
        /// </summary>
        /// <param name="root">Nupkg folder directory path.</param>
        public static IEnumerable<LocalPackageInfo> GetPackagesConfigFolderPackages(
            string root,
            string id,
            ILogger log)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }

            var rootDirInfo = GetAndVerifyRootDirectory(root);

            if (rootDirInfo.Exists)
            {
                var searchPattern = GetPackagesConfigFolderSearchPattern(id);

                foreach (var dir in GetDirectoriesSafe(rootDirInfo, searchPattern, SearchOption.TopDirectoryOnly, log, CancellationToken.None))
                {
                    // Check the id and version of the path, if the id matches and the version
                    // is valid this will be non-null;
                    var dirVersion = GetVersionFromIdVersionString(dir.Name, id);

                    if (dirVersion != null)
                    {
                        var package = GetPackagesConfigFolderPackage(dir, log);

                        if (package != null)
                        {
                            yield return package;
                        }
                    }
                }
            }

            yield break;
        }

        /// <summary>
        /// Retrieve a package with an id and version from a packages.config packages folder.
        /// </summary>
        /// <param name="root">Nupkg folder directory path.</param>
        /// <param name="identity">Package id and version.</param>
        public static LocalPackageInfo GetPackagesConfigFolderPackage(string root, PackageIdentity identity, ILogger log)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }

            var rootDirInfo = GetAndVerifyRootDirectory(root);

            // Try matching the exact version format
            var idVersion = $"{identity.Id}.{identity.Version.ToString()}";
            var expectedPath = Path.Combine(rootDirInfo.FullName, idVersion, $"{idVersion}{PackagingCoreConstants.NupkgExtension}");
            var expectedFile = CreateFileInfoIfValidOrNull(expectedPath, log);

            if (expectedFile != null && expectedFile.Exists)
            {
                var localPackage = GetPackageFromNupkg(expectedFile);

                // Verify that the nuspec matches the expected id/version.
                if (localPackage != null && identity.Equals(localPackage.Identity))
                {
                    return localPackage;
                }
            }

            // Search all sub folders

            if (rootDirInfo.Exists)
            {
                var searchPattern = GetPackagesConfigFolderSearchPattern(identity.Id);

                foreach (var dir in GetDirectoriesSafe(rootDirInfo, searchPattern, SearchOption.TopDirectoryOnly, log, CancellationToken.None))
                {
                    // Check the id and version of the path, if the id matches and the version
                    // is valid this will be non-null;
                    var dirVersion = GetVersionFromIdVersionString(dir.Name, identity.Id);

                    if (identity.Version == dirVersion)
                    {
                        var localPackage = GetPackagesConfigFolderPackage(dir, log);

                        // Verify that the nuspec matches the expected id/version.
                        if (localPackage != null && identity.Equals(localPackage.Identity))
                        {
                            return localPackage;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Returns either id.* or * depending on the OS.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static string GetPackagesConfigFolderSearchPattern(string id)
        {
            // Case insensitive searches on windows may use the id prefix.
            // The majority of packages.config scenarios will be on windows.
            if (!string.IsNullOrEmpty(id) && RuntimeEnvironmentHelper.IsWindows)
            {
                return $"{id}.*";
            }

            // For non-windows systems which may be case-sensitive search all directories
            return $"*";
        }

        /// <summary>
        /// Retrieve a package with an id and version from a packages.config packages folder.
        /// </summary>
        /// <param name="root">Nupkg folder directory path.</param>
        public static LocalPackageInfo GetPackagesConfigFolderPackage(
            string root,
            string id,
            NuGetVersion version,
            ILogger log)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (version == null)
            {
                throw new ArgumentNullException(nameof(version));
            }

            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }

            var identity = new PackageIdentity(id, version);
            return GetPackagesConfigFolderPackage(root, identity, log);
        }

        /// <summary>
        /// Return the package nupkg from a packages.config folder sub directory.
        /// </summary>
        /// <param name="dir">Package directory in the format id.version</param>
        private static LocalPackageInfo GetPackagesConfigFolderPackage(DirectoryInfo dir, ILogger log)
        {
            LocalPackageInfo result = null;

            var nupkgPath = Path.Combine(
                dir.FullName,
                $"{dir.Name}{PackagingCoreConstants.NupkgExtension}");

            var nupkgFile = CreateFileInfoIfValidOrNull(nupkgPath, log);

            if (nupkgFile != null && nupkgFile.Exists)
            {
                result = GetPackageFromNupkg(nupkgFile);
            }

            return result;
        }

        /// <summary>
        /// True if the file name matches the identity. This is could be incorrect if
        /// the package name ends with numbers. The result should be checked against the nuspec.
        /// </summary>
        public static bool IsPossiblePackageMatch(FileInfo file, PackageIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            return identity.Equals(GetIdentityFromNupkgPath(file, identity.Id));
        }

        /// <summary>
        /// True if the file name matches the id and is followed by a version. This is could be incorrect if
        /// the package name ends with numbers. The result should be checked against the nuspec.
        /// </summary>
        public static bool IsPossiblePackageMatch(FileInfo file, string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            return GetIdentityFromNupkgPath(file, id) != null;
        }

        /// <summary>
        /// An imperfect attempt at finding the identity of a package from the file name.
        /// This can fail if the package name ends with something such as .1
        /// </summary>
        public static PackageIdentity GetIdentityFromNupkgPath(FileInfo file, string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            var version = GetVersionFromFileName(file.Name, id, PackagingCoreConstants.NupkgExtension);

            if (version != null)
            {
                return new PackageIdentity(id, version);
            }

            return null;
        }

        /// <summary>
        /// An imperfect attempt at finding the version of a package from the file name.
        /// This can fail if the package name ends with something such as .1
        /// </summary>
        public static NuGetVersion GetVersionFromFileName(string fileName, string id, string extension)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (fileName == null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            if (extension == null)
            {
                throw new ArgumentNullException(nameof(extension));
            }

            NuGetVersion result = null;

            if (fileName.EndsWith(extension, StringComparison.OrdinalIgnoreCase))
            {
                fileName = Path.GetFileNameWithoutExtension(fileName);

                // Skip symbol packages
                if (!fileName.EndsWith(".symbols", StringComparison.OrdinalIgnoreCase))
                {
                    result = GetVersionFromIdVersionString(fileName, id);
                }
            }

            return result;
        }

        /// <summary>
        /// Parse a possible version from a string in the format Id.Version
        /// Returns null if the version is invalid or the id did not match.
        /// </summary>
        /// <param name="idVersionString">Id.Version</param>
        /// <param name="id">Expected id</param>
        private static NuGetVersion GetVersionFromIdVersionString(string idVersionString, string id)
        {
            var prefix = $"{id}.";

            if (idVersionString.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                var versionString = idVersionString.Substring(prefix.Length);

                NuGetVersion version;
                if (NuGetVersion.TryParse(versionString, out version))
                {
                    return version;
                }
            }

            return null;
        }

        /// <summary>
        /// Retrieve a single package from a v3 version folder.
        /// </summary>
        public static LocalPackageInfo GetPackageV3(string root, string id, NuGetVersion version, ILogger log)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (version == null)
            {
                throw new ArgumentNullException(nameof(version));
            }

            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }

            return GetPackageV3(root, new PackageIdentity(id, version), log);
        }

        /// <summary>
        /// Retrieve a package from a v3 feed.
        /// </summary>
        public static LocalPackageInfo GetPackageV3(string root, PackageIdentity identity, ILogger log)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }

            // Verify the root path is a valid path.
            var rootDirInfo = GetAndVerifyRootDirectory(root);

            var pathResolver = new VersionFolderPathResolver(rootDirInfo.FullName);

            // Verify the neccessary files exist
            var nupkgPath = pathResolver.GetPackageFilePath(identity.Id, identity.Version);
            var nuspecPath = pathResolver.GetManifestFilePath(identity.Id, identity.Version);
            var hashPath = pathResolver.GetHashPath(identity.Id, identity.Version);

            if (!File.Exists(nupkgPath))
            {
                log.LogDebug($"Missing {nupkgPath}");
                return null;
            }

            if (!File.Exists(nuspecPath))
            {
                log.LogDebug($"Missing {nuspecPath}");
                return null;
            }

            if (!File.Exists(hashPath))
            {
                log.LogDebug($"Missing {hashPath}");
                return null;
            }

            var nuspecHelper = new Lazy<NuspecReader>(() => new NuspecReader(nuspecPath));

            return new LocalPackageInfo(
                new PackageIdentity(identity.Id, identity.Version),
                nupkgPath,
                File.GetLastWriteTimeUtc(nupkgPath),
                nuspecHelper,
                useFolder: false
            );
        }

        /// <summary>
        /// Discover all nupkgs from a v2 local folder.
        /// </summary>
        /// <param name="root">Folder root.</param>
        public static IEnumerable<FileInfo> GetNupkgsFromFlatFolder(string root, ILogger log)
        {
            return GetNupkgsFromFlatFolder(root, log, CancellationToken.None);
        }

        /// <summary>
        /// Discover all nupkgs from a v2 local folder.
        /// </summary>
        /// <param name="root">Folder root.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public static IEnumerable<FileInfo> GetNupkgsFromFlatFolder(string root, ILogger log, CancellationToken cancellationToken)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }

            // Check for package files one level deep.
            DirectoryInfo rootDirectoryInfo = GetAndVerifyRootDirectory(root);

            // Return all directory file list chunks in a flat list
            foreach (var directoryList in GetNupkgsFromFlatFolderChunked(rootDirectoryInfo, log, cancellationToken))
            {
                foreach (var file in directoryList)
                {
                    yield return file;
                }
            }

            yield break;
        }

        public static FeedType GetLocalFeedType(string root, ILogger log)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }

            // Check for package files one level deep.
            DirectoryInfo rootDirectoryInfo = GetAndVerifyRootDirectory(root);

            try
            {
                // Search root directory for *.Nupkg, return V2 if there is any Nupkg file.
                if (rootDirectoryInfo.EnumerateFiles(NupkgFilter, SearchOption.TopDirectoryOnly).Any())
                {
                    return FeedType.FileSystemV2;
                }

                foreach (var idDir in rootDirectoryInfo.EnumerateDirectories())
                {
                    // Search first sub directory for *.Nupkg, return V2 if there is any Nupkg file.
                    if (idDir.EnumerateFiles(NupkgFilter, SearchOption.TopDirectoryOnly).Any())
                    {
                        return FeedType.FileSystemV2;
                    }

                    foreach (var versionDir in idDir.EnumerateDirectories())
                    {
                        // If we have files in the format {packageId}/{version}/{packageId}.{version}.nupkg, return V3. 
                        var package = GetPackageV3(root, idDir.Name, versionDir.Name, log);

                        if (package != null)
                        {
                            return FeedType.FileSystemV3;
                        }
                    }
                }
            }

            catch (UnauthorizedAccessException)
            {

            }
            catch (DirectoryNotFoundException)
            {

            }

            return FeedType.FileSystemUnknown;
        }

        /// <summary>
        /// Verify that a path could be a valid directory. Throw a FatalProtocolException otherwise.
        /// </summary>
        public static DirectoryInfo GetAndVerifyRootDirectory(string root)
        {
            // Check for package files one level deep.
            DirectoryInfo rootDirectoryInfo = null;

            try
            {
                // Convert file:// to a local path if needed
                var localPath = UriUtility.GetLocalPath(root);

                // Verify that the directory is a valid path.
                rootDirectoryInfo = new DirectoryInfo(localPath);

                // The root must also be parsable as a URI (relative or absolute). This rejects
                // sources that have the weird "C:Source" format. For more information about this 
                // format, see:
                // https://msdn.microsoft.com/en-us/library/windows/desktop/aa365247(v=vs.85).aspx#paths
                var uriResult = new Uri(root, UriKind.RelativeOrAbsolute);

                // Allow only local paths
                if (uriResult?.IsAbsoluteUri == true && !uriResult.IsFile)
                {
                    throw new NotSupportedException(uriResult.AbsoluteUri);
                }
            }
            catch (Exception ex) when (ex is ArgumentException ||
                                       ex is IOException ||
                                       ex is SecurityException ||
                                       ex is UriFormatException ||
                                       ex is NotSupportedException)
            {
                var message = string.Format(CultureInfo.CurrentCulture, Strings.Log_FailedToVerifyRootDirectory, root);

                throw new FatalProtocolException(message, ex);
            }

            return rootDirectoryInfo;
        }

        /// <summary>
        /// Verify that a path could be a valid file. Throw a FatalProtocolException otherwise.
        /// </summary>
        private static FileInfo GetAndVerifyFileInfo(Uri fileUri)
        {
            FileInfo fileInfo = null;

            try
            {
                // Verify that the file is a valid path
                fileInfo = new FileInfo(fileUri.LocalPath);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is IOException || ex is SecurityException)
            {
                var message = string.Format(CultureInfo.CurrentCulture, Strings.Log_FailedToVerifyValidFile, fileUri.AbsoluteUri);

                throw new FatalProtocolException(message, ex);
            }

            return fileInfo;
        }

        /// <summary>
        /// Retrieve files in chunks, this helps maintain the legacy behavior of searching for
        /// certain non-normalized file names.
        /// </summary>
        private static IEnumerable<List<FileInfo>> GetNupkgsFromFlatFolderChunked(DirectoryInfo root, ILogger log, CancellationToken cancellationToken)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }

            // Ignore missing directories for v2
            if (!root.Exists)
            {
                yield break;
            }

            cancellationToken.ThrowIfCancellationRequested();

            // Search the top level directory
            var topLevel = GetNupkgsFromDirectory(root, log, cancellationToken);

            if (topLevel.Count > 0)
            {
                yield return topLevel;
            }

            // Search all sub directories
            foreach (var subDirectory in GetDirectoriesSafe(root, log, cancellationToken))
            {
                var files = GetNupkgsFromDirectory(subDirectory, log, cancellationToken);

                if (files.Count > 0)
                {
                    yield return files;
                }
            }

            yield break;
        }

        /// <summary>
        /// Discover nupkgs from a v2 local folder.
        /// </summary>
        /// <param name="root">Folder root.</param>
        /// <param name="id">Package id file name prefix.</param>
        public static IEnumerable<FileInfo> GetNupkgsFromFlatFolder(string root, string id, ILogger log)
        {
            return GetNupkgsFromFlatFolder(root, id, log, CancellationToken.None);
        }

        /// <summary>
        /// Discover nupkgs from a v2 local folder.
        /// </summary>
        /// <param name="root">Folder root.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public static IEnumerable<FileInfo> GetNupkgsFromFlatFolder(string root, string id, ILogger log, CancellationToken cancellationToken)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }

            foreach (var path in GetNupkgsFromFlatFolder(root, log, cancellationToken))
            {
                if (IsPossiblePackageMatch(path, id))
                {
                    yield return path;
                }
            }

            yield break;
        }

        /// <summary>
        /// Discover all nupkgs from a v3 folder.
        /// </summary>
        /// <param name="root">Folder root.</param>
        public static IEnumerable<LocalPackageInfo> GetPackagesV3(string root, ILogger log)
        {
            return GetPackagesV3(root, log, CancellationToken.None);
        }

        /// <summary>
        /// Discover all nupkgs from a v3 folder.
        /// </summary>
        /// <param name="root">Folder root.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public static IEnumerable<LocalPackageInfo> GetPackagesV3(string root, ILogger log, CancellationToken cancellationToken)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }

            cancellationToken.ThrowIfCancellationRequested();
            // Validate teh root path
            DirectoryInfo rootDirectoryInfo = GetAndVerifyRootDirectory(root);

            if (!rootDirectoryInfo.Exists)
            {
                // Directory is missing
                yield break;
            }

            // Match all nupkgs in the folder
            foreach (var idPath in GetDirectoriesSafe(rootDirectoryInfo, log, cancellationToken))
            {
                foreach (var nupkg in GetPackagesV3(root, id: idPath.Name, log: log, cancellationToken: cancellationToken))
                {
                    yield return nupkg;
                }
            }

            yield break;
        }

        /// <summary>
        /// Discover nupkgs from a v3 local folder.
        /// </summary>
        /// <param name="root">Folder root.</param>
        /// <param name="id">Package id or package id prefix.</param>
        public static IEnumerable<LocalPackageInfo> GetPackagesV3(string root, string id, ILogger log)
        {
            return GetPackagesV3(root, id, log, CancellationToken.None);
        }

        /// <summary>
        /// Discover nupkgs from a v3 local folder.
        /// </summary>
        /// <param name="root">Folder root.</param>
        /// <param name="id">Package id or package id prefix.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public static IEnumerable<LocalPackageInfo> GetPackagesV3(string root, string id, ILogger log, CancellationToken cancellationToken)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }

            cancellationToken.ThrowIfCancellationRequested();
            // Check for package files one level deep.
            DirectoryInfo rootDirectoryInfo = GetAndVerifyRootDirectory(root);

            var pathResolver = new VersionFolderPathResolver(rootDirectoryInfo.FullName);
            var idRoot = new DirectoryInfo(pathResolver.GetVersionListPath(id));
            if (!idRoot.Exists)
            {
                // Directory is missing
                yield break;
            }

            foreach (var versionDir in GetDirectoriesSafe(idRoot, log, cancellationToken))
            {
                var package = GetPackageV3(root, id, versionDir.Name, log);

                if (package != null)
                {
                    yield return package;
                }
            }

            yield break;
        }

        /// <summary>
        /// Resolves a package path into a list of paths.
        /// If the path contains wildcards then the path is expanded to all matching entries.
        /// </summary>
        /// <param name="packagePath">Package path</param>
        /// <returns>A list of package paths that match the input path.</returns>
        public static IEnumerable<string> ResolvePackageFromPath(string packagePath, bool isSnupkg = false)
        {
            packagePath = EnsurePackageExtension(packagePath, isSnupkg);
            return PathResolver.PerformWildcardSearch(Directory.GetCurrentDirectory(), packagePath);
        }

        /// <summary>
        /// Ensure any wildcards in packagePath end with *.nupkg or *.snupkg.
        /// </summary>
        /// <param name="packagePath"></param>
        /// <param name="isSnupkg"></param>
        /// <returns>The absolute path, or the normalized wildcard path.</returns>
        private static string EnsurePackageExtension(string packagePath, bool isSnupkg)
        {
#if NETCOREAPP
            if (packagePath.IndexOf('*', StringComparison.Ordinal) == -1)
#else
            if (packagePath.IndexOf('*') == -1)
#endif
            {
                // If there's no wildcard in the path to begin with, assume that it's an absolute path.
                return packagePath;
            }
            // If the path does not contain wildcards, we need to add *.nupkg to it.
            if (!packagePath.EndsWith(NuGetConstants.PackageExtension, StringComparison.OrdinalIgnoreCase)
                && !packagePath.EndsWith(NuGetConstants.SnupkgExtension, StringComparison.OrdinalIgnoreCase))
            {
                if (packagePath.EndsWith("**", StringComparison.OrdinalIgnoreCase))
                {
                    packagePath = packagePath + Path.DirectorySeparatorChar + '*';
                }
                else if (!packagePath.EndsWith("*", StringComparison.OrdinalIgnoreCase))
                {
                    packagePath = packagePath + '*';
                }
                packagePath = packagePath + (isSnupkg ? NuGetConstants.SnupkgExtension : NuGetConstants.PackageExtension);
            }
            return packagePath;
        }

        /// <summary>
        /// If there isn't at least one Path specified, throw that no file paths were resolved for this Package.
        /// </summary>
        /// <param name="packagePath">The package path the user originally provided.</param>
        /// <param name="matchingPackagePaths">A list of matching package paths that were previously resolved.</param>
        public static void EnsurePackageFileExists(string packagePath, IEnumerable<string> matchingPackagePaths)
        {
            if (!(matchingPackagePaths != null && matchingPackagePaths.Any()))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                    Strings.UnableToFindFile,
                    packagePath));
            }
        }

        /// <summary>
        /// Remove duplicate packages which can occur in directories.
        /// In V2 packages may exist under multiple sub folders. 
        /// Non-normalized versions also lead to duplicates: ex: 1.0, 1.0.0.0
        /// </summary>
        public static IEnumerable<LocalPackageInfo> GetDistinctPackages(IEnumerable<LocalPackageInfo> packages)
        {
            if (packages == null)
            {
                throw new ArgumentNullException(nameof(packages));
            }

            var seen = new HashSet<PackageIdentity>();

            foreach (var package in packages)
            {
                if (seen.Add(package.Identity))
                {
                    yield return package;
                }
            }

            yield break;
        }

        /// <summary>
        /// Retrieve directories and log exceptions that occur.
        /// </summary>
        private static List<DirectoryInfo> GetDirectoriesSafe(DirectoryInfo root, ILogger log, CancellationToken cancellationToken)
        {
            try
            {
                var enumerable = root.EnumerateDirectories();
                // .ToList necessary for perf concern.
                // If enumaration happen several times then same I/O calls repeatedly called on same input, I/O calls are more expensive then memory.
                return CancellableYieldEnumeration(enumerable, cancellationToken).ToList();
            }
            catch (Exception e) when (e is not OperationCanceledException)
            {
                // On cancellation we bubble up exception to call stack.
                // Otherwise return all or nothing. If no exception return all.
                // Break on first exception with logging in order to keep previous experience, return empty List, but don't throw.
                log.LogWarning(e.Message);
            }

            return new List<DirectoryInfo>();
        }

        private static List<DirectoryInfo> GetDirectoriesSafe(DirectoryInfo root, string filter, SearchOption searchOption, ILogger log, CancellationToken cancellationToken)
        {
            try
            {
                var enumerable = root.EnumerateDirectories(filter, searchOption);
                // .ToList necessary for perf concern.
                // If enumaration happen several times then same I/O calls repeatedly called on same input, I/O calls are more expensive then memory.
                return CancellableYieldEnumeration(enumerable, cancellationToken).ToList();
            }
            catch (Exception e) when (e is not OperationCanceledException)
            {
                // On cancellation we bubble up exception to call stack.
                // Otherwise return all or nothing. If no exception return all.
                // Break on first exception with logging in order to keep previous experience, return empty List, but don't throw.
                log.LogWarning(e.Message);
            }

            return new List<DirectoryInfo>();
        }

        /// <summary>
        /// Retrieve files and log exceptions that occur.
        /// </summary>
        internal static List<FileInfo> GetFilesSafe(DirectoryInfo root, string filter, ILogger log, CancellationToken cancellationToken)
        {
            try
            {
                var enumerable = root.EnumerateFiles(filter);
                // .ToList necessary for perf concern.
                // If enumaration happen several times then same I/O calls repeatedly called on same input, I/O calls are more expensive then memory.
                return CancellableYieldEnumeration(enumerable, cancellationToken).ToList();
            }
            catch (Exception e) when (e is not OperationCanceledException)
            {
                // On cancellation we bubble up exception to call stack.
                // Otherwise return all or nothing. If no exception return all.
                // Break on first exception with logging in order to keep previous experience, return empty List, but don't throw.
                log.LogWarning(e.Message);
            }

            return new List<FileInfo>();
        }

        /// <summary>
        /// Path -> LocalPackageInfo
        /// </summary>
        private static IEnumerable<LocalPackageInfo> GetPackagesFromNupkgs(IEnumerable<FileInfo> files, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return files.Select(file => GetPackageFromNupkg(file));
        }

        /// <summary>
        /// Path -> LocalPackageInfo
        /// </summary>
        private static LocalPackageInfo GetPackageFromNupkg(FileInfo nupkgFile)
        {
            try
            {
                using (var package = new PackageArchiveReader(nupkgFile.FullName))
                {
                    var nuspec = package.NuspecReader;

                    var nuspecHelper = new Lazy<NuspecReader>(() => nuspec);

                    return new LocalPackageInfo(
                        nuspec.GetIdentity(),
                        nupkgFile.FullName,
                        nupkgFile.LastWriteTimeUtc,
                        nuspecHelper,
                        useFolder: false
                    );
                }
            }
            catch (Exception ex)
            {
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.NupkgPath_InvalidEx,
                    nupkgFile.FullName,
                    ex.Message);

                throw new FatalProtocolException(message, ex);
            }
        }

        /// <summary>
        /// Find all nupkgs in the top level of a directory.
        /// </summary>
        private static List<FileInfo> GetNupkgsFromDirectory(DirectoryInfo root, ILogger log, CancellationToken cancellationToken)
        {
            return GetFilesSafe(root, NupkgFilter, log, cancellationToken);
        }

        private static LocalPackageInfo GetPackageV3(string root, string id, string version, ILogger log)
        {
            NuGetVersion nugetVersion;
            if (NuGetVersion.TryParse(version, out nugetVersion))
            {
                var identity = new PackageIdentity(id, nugetVersion);

                // Read the package, this may be null if files are missing
                return GetPackageV3(root, identity, log);
            }
            else
            {
                log.LogWarning(string.Format(CultureInfo.CurrentCulture, Strings.UnableToParseFolderV3Version, version));
            }

            return null;
        }

        private static FileInfo CreateFileInfoIfValidOrNull(string localPath, ILogger log)
        {
            FileInfo fileInfo = null;
            try
            {
                fileInfo = new FileInfo(localPath);
            }
            catch (PathTooLongException e)
            {
                log.LogDebug(e.Message);
            }
            return fileInfo;
        }

        public static void GenerateNupkgMetadataFile(string nupkgPath, string installPath, string hashPath, string nupkgMetadataPath)
        {
            ConcurrencyUtilities.ExecuteWithFileLocked(nupkgPath,
                action: () =>
                {
                    // make sure new hash file doesn't exists within File lock before actually creating it.
                    if (!File.Exists(nupkgMetadataPath))
                    {
                        var tempNupkgMetadataFilePath = Path.Combine(installPath, Path.GetRandomFileName());
                        using (var stream = File.Open(nupkgPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                        using (var packageReader = new PackageArchiveReader(stream))
                        {
                            // get hash of unsigned content of signed package
                            var packageHash = packageReader.GetContentHash(
                                CancellationToken.None,
                                GetUnsignedPackageHash:
                                () =>
                                {
                                    if (!string.IsNullOrEmpty(hashPath) && File.Exists(hashPath))
                                    {
                                        return File.ReadAllText(hashPath);
                                    }

                                    return null;
                                });

                            // write the new hash file
                            var hashFile = new NupkgMetadataFile()
                            {
                                ContentHash = packageHash
                            };

                            NupkgMetadataFileFormat.Write(tempNupkgMetadataFilePath, hashFile);
                            File.Move(tempNupkgMetadataFilePath, nupkgMetadataPath);
                        }
                    }
                });
        }

        static IEnumerable<T> CancellableYieldEnumeration<T>(IEnumerable<T> enumerable, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            foreach (T item in enumerable)
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return item;
            }
        }
    }
}
