// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Protocol;
using NuGet.Shared;
using NuGet.Versioning;

namespace NuGet.Repositories
{
    /// <summary>
    /// Caches package info from the global packages folder in memory.
    /// Packages not yet in the cache will be retrieved from disk.
    /// </summary>
    public class NuGetv3LocalRepository
    {
        // Folder path -> Package
        private readonly ConcurrentDictionary<string, LocalPackageInfo> _packageCache
            = new ConcurrentDictionary<string, LocalPackageInfo>(PathUtility.GetStringComparerBasedOnOS());

        // Id -> Packages
        private readonly ConcurrentDictionary<string, List<LocalPackageInfo>> _cache
            = new ConcurrentDictionary<string, List<LocalPackageInfo>>(StringComparer.OrdinalIgnoreCase);

        // Per package id locks
        private readonly ConcurrentDictionary<string, object> _idLocks
            = new ConcurrentDictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        // Cache nuspecs lazily
        private readonly LocalPackageFileCache _packageFileCache = null;

        private readonly bool _isFallbackFolder;

        private readonly bool _updateLastAccessTime;

        public VersionFolderPathResolver PathResolver { get; }

        public string RepositoryRoot { get; }

        public NuGetv3LocalRepository(string path)
            : this(path, packageFileCache: null, isFallbackFolder: false, updateLastAccessTime: false)
        {
        }

        public NuGetv3LocalRepository(string path, LocalPackageFileCache packageFileCache, bool isFallbackFolder)
            : this(path, packageFileCache, isFallbackFolder, updateLastAccessTime: false)
        {
        }

        public NuGetv3LocalRepository(string path, LocalPackageFileCache packageFileCache, bool isFallbackFolder, bool updateLastAccessTime)
        {
            RepositoryRoot = path;
            PathResolver = new VersionFolderPathResolver(path);
            _packageFileCache = packageFileCache ?? new LocalPackageFileCache();
            _isFallbackFolder = isFallbackFolder;
            _updateLastAccessTime = updateLastAccessTime;
        }

        /// <summary>
        /// True if the package exists.
        /// </summary>
        public bool Exists(string packageId, NuGetVersion version)
        {
            return FindPackageImpl(packageId, version) != null;
        }

        public LocalPackageInfo FindPackage(string packageId, NuGetVersion version)
        {
            var package = FindPackageImpl(packageId, version);

            if (package == null)
            {
                return null;
            }

            // Check for an exact match on casing
            if (StringComparer.Ordinal.Equals(packageId, package.Id)
                && EqualityUtility.SequenceEqualWithNullCheck(version.ReleaseLabels, package.Version.ReleaseLabels, StringComparer.Ordinal))
            {
                return package;
            }

            // nuspec
            var nuspec = _packageFileCache.GetOrAddNuspec(package.ManifestPath, package.ExpandedPath);

            // files
            var files = _packageFileCache.GetOrAddFiles(package.ExpandedPath);

            // sha512
            var sha512 = _packageFileCache.GetOrAddSha512(package.Sha512Path);

            // runtime.json
            var runtimeGraph = _packageFileCache.GetOrAddRuntimeGraph(package.ExpandedPath);

            // Create a new info to match the given id/version
            return new LocalPackageInfo(
                packageId,
                version,
                package.ExpandedPath,
                package.ManifestPath,
                package.ZipPath,
                package.Sha512Path,
                nuspec,
                files,
                sha512,
                runtimeGraph);
        }

        public IEnumerable<LocalPackageInfo> FindPackagesById(string packageId)
            => FindPackagesByIdImpl(packageId);

        private List<LocalPackageInfo> FindPackagesByIdImpl(string packageId)
        {
            if (string.IsNullOrEmpty(packageId))
            {
                throw new ArgumentNullException(nameof(packageId));
            }

            // Callers must wait until all clears have finished
            lock (GetLockObj(packageId))
            {
                return _cache.GetOrAdd(packageId, id =>
                {
                    return GetPackages(id);
                });
            }
        }

        private LocalPackageInfo FindPackageImpl(string packageId, NuGetVersion version)
        {
            var installPath = PathResolver.GetInstallPath(packageId, version);
            lock (GetLockObj(installPath))
            {
                return GetPackage(packageId, version, installPath);
            }
        }

        private List<LocalPackageInfo> GetPackages(string id)
        {
            var packages = new List<LocalPackageInfo>();

            var packageIdRoot = PathResolver.GetVersionListPath(id);

            if (!Directory.Exists(packageIdRoot))
            {
                return packages;
            }

            foreach (var fullVersionDir in Directory.EnumerateDirectories(packageIdRoot))
            {
                LocalPackageInfo package;
                if (!_packageCache.TryGetValue(fullVersionDir, out package))
                {
                    var versionPart = fullVersionDir.Substring(packageIdRoot.Length).TrimStart(Path.DirectorySeparatorChar);

                    // Get the version part and parse it
                    NuGetVersion version;
                    if (!NuGetVersion.TryParse(versionPart, out version))
                    {
                        continue;
                    }

                    package = GetPackage(id, version, fullVersionDir);
                }

                // Add the package if it is valid
                if (package != null)
                {
                    packages.Add(package);
                }
            }

            return packages;
        }

        private LocalPackageInfo GetPackage(string packageId, NuGetVersion version, string path)
        {
            if (!_packageCache.TryGetValue(path, out var package))
            {
                var nupkgMetadataPath = PathResolver.GetNupkgMetadataPath(packageId, version);
                var hashPath = PathResolver.GetHashPath(packageId, version);
                var zipPath = PathResolver.GetPackageFilePath(packageId, version);

                // The nupkg metadata file is written last. If this file does not exist then the package is
                // incomplete and should not be used.
                if (_packageFileCache.Sha512Exists(nupkgMetadataPath))
                {
                    package = CreateLocalPackageInfo(packageId, version, path, nupkgMetadataPath, zipPath);
                }
                // if hash file exists and it's not a fallback folder then we generate nupkg metadata file
                else if (!_isFallbackFolder && _packageFileCache.Sha512Exists(hashPath))
                {
                    LocalFolderUtility.GenerateNupkgMetadataFile(zipPath, path, hashPath, nupkgMetadataPath);

                    package = CreateLocalPackageInfo(packageId, version, path, nupkgMetadataPath, zipPath);
                }

                if (package != null)
                {
                    // Cache the package, if it is valid it will not change
                    // for the life of this restore.
                    // Locking is done at a higher level around the id
                    _packageCache.TryAdd(path, package);

                    if (!_isFallbackFolder && _updateLastAccessTime)
                    {
                        _packageFileCache.UpdateLastAccessTime(nupkgMetadataPath);
                    }
                }
            }

            return package;
        }

        private LocalPackageInfo CreateLocalPackageInfo(string id, NuGetVersion version, string fullVersionDir, string newHashPath, string zipPath)
        {
            var manifestPath = PathResolver.GetManifestFilePath(id, version);
            var nuspec = _packageFileCache.GetOrAddNuspec(manifestPath, fullVersionDir);
            var files = _packageFileCache.GetOrAddFiles(fullVersionDir);
            var sha512 = _packageFileCache.GetOrAddSha512(newHashPath);
            var runtimeGraph = _packageFileCache.GetOrAddRuntimeGraph(fullVersionDir);

            return new LocalPackageInfo(id, version, fullVersionDir, manifestPath, zipPath, newHashPath, nuspec, files, sha512, runtimeGraph);
        }

        /// <summary>
        /// Remove cached results for the given ids. This is needed
        /// after installing a new package.
        /// </summary>
        public void ClearCacheForIds(IEnumerable<string> packageIds)
        {
            foreach (var packageId in packageIds)
            {
                // Clearers must wait for all requests to complete
                lock (GetLockObj(packageId))
                {
                    _cache.TryRemove(packageId, out _);
                }
            }
        }

        private object GetLockObj(string privateId)
        {
            return _idLocks.GetOrAdd(privateId, new object());
        }
    }
}
