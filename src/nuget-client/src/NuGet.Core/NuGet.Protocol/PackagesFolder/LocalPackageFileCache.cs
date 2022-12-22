// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.RuntimeModel;

namespace NuGet.Protocol
{
    /// <summary>
    /// Allow .nuspec files on disk to be cached across v3 folder readers.
    /// Allow the list of files in a package to be cached across all projects.
    /// </summary>
    /// <remarks>It is expected that the caller has already verified the that folder and paths are valid.</remarks>
    public class LocalPackageFileCache
    {
        // Expanded path -> NuspecReader
        private readonly ConcurrentDictionary<string, Lazy<NuspecReader>> _nuspecCache
            = new ConcurrentDictionary<string, Lazy<NuspecReader>>(PathUtility.GetStringComparerBasedOnOS());

        // Expanded path -> Package file list
        private readonly ConcurrentDictionary<string, Lazy<IReadOnlyList<string>>> _filesCache
            = new ConcurrentDictionary<string, Lazy<IReadOnlyList<string>>>(PathUtility.GetStringComparerBasedOnOS());

        // SHA512 path -> SHA512
        private readonly ConcurrentDictionary<string, Lazy<string>> _sha512Cache
            = new ConcurrentDictionary<string, Lazy<string>>(PathUtility.GetStringComparerBasedOnOS());

        // File exists cache, values are only added if they exist, missing files are not cached.
        private readonly ConcurrentDictionary<string, bool> _fileExistsCache
            = new ConcurrentDictionary<string, bool>(PathUtility.GetStringComparerBasedOnOS());

        // Cache runtime.json files
        private readonly ConcurrentDictionary<string, Lazy<RuntimeGraph>> _runtimeCache
            = new ConcurrentDictionary<string, Lazy<RuntimeGraph>>(PathUtility.GetStringComparerBasedOnOS());

        // Metadata file
        private readonly ConcurrentDictionary<string, bool> _metadataFileCache
            = new ConcurrentDictionary<string, bool>(PathUtility.GetStringComparerBasedOnOS());

        public LocalPackageFileCache()
        {
        }

        /// <summary>
        /// Read a nuspec file from disk. The nuspec is expected to exist.
        /// </summary>
        public virtual Lazy<NuspecReader> GetOrAddNuspec(string manifestPath, string expandedPath)
        {
            return _nuspecCache.GetOrAdd(expandedPath,
                e => new Lazy<NuspecReader>(() => GetNuspec(manifestPath, e)));
        }

        /// <summary>
        /// Read a the package files from disk.
        /// </summary>
        public virtual Lazy<IReadOnlyList<string>> GetOrAddFiles(string expandedPath)
        {
            return _filesCache.GetOrAdd(expandedPath,
                e => new Lazy<IReadOnlyList<string>>(() => GetFiles(e)));
        }

        /// <summary>
        /// Read the .metadata.json file from disk.
        /// </summary>
        /// <remarks>Throws if the file is not found or corrupted.</remarks>
        public virtual Lazy<string> GetOrAddSha512(string sha512Path)
        {
            return _sha512Cache.GetOrAdd(sha512Path,
                e => new Lazy<string>(() =>
                {
                    var metadataFile = NupkgMetadataFileFormat.Read(e);
                    return metadataFile.ContentHash;
                }));
        }

        /// <summary>
        /// True if the path exists on disk. This also uses
        /// the SHA512 cache for already read files.
        /// </summary>
        public virtual bool Sha512Exists(string sha512Path)
        {
            // Avoid checking the desk if we have already read the file.
            var exists = _fileExistsCache.ContainsKey(sha512Path);

            // Check the file directly if it was not in the cache.
            if (!exists && File.Exists(sha512Path))
            {
                // The file exists, add it to the cache
                _fileExistsCache.TryAdd(sha512Path, true);
                exists = true;
            }

            return exists;
        }

        /// <summary>
        /// Update the last access time of the metadata package file. This also uses
        /// the metadata file cache for already accessed files.
        /// </summary>
        /// <param name="nupkgMetadataPath">metadata file path to update</param>
        public void UpdateLastAccessTime(string nupkgMetadataPath)
        {
            var exists = _metadataFileCache.ContainsKey(nupkgMetadataPath);
            if (exists)
            {
                return;
            }

            try
            {
                File.SetLastAccessTimeUtc(nupkgMetadataPath, DateTime.UtcNow);
                _metadataFileCache.TryAdd(nupkgMetadataPath, true);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Read runtime.json from a package.
        /// Returns null if runtime.json does not exist.
        /// </summary>
        public virtual Lazy<RuntimeGraph> GetOrAddRuntimeGraph(string expandedPath)
        {
            return _runtimeCache.GetOrAdd(expandedPath, p => new Lazy<RuntimeGraph>(() => GetRuntimeGraph(p)));
        }

        /// <summary>
        /// Read files from a package folder.
        /// </summary>
        private static IReadOnlyList<string> GetFiles(string expandedPath)
        {
            using (var packageReader = new PackageFolderReader(expandedPath))
            {
                // Get package files, excluding directory entries and OPC files
                // This is sorted before it is written out
                return packageReader.GetFiles()
                    .Where(file => IsAllowedLibraryFile(file))
                    .ToList()
                    .AsReadOnly();
            }
        }

        /// <summary>
        /// True if the file should be added to the lock file library
        /// Fale if it is an OPC file or empty directory
        /// </summary>
        private static bool IsAllowedLibraryFile(string path)
        {
            switch (path)
            {
                case "_rels/.rels":
                case "[Content_Types].xml":
                    return false;
            }

            if (path.EndsWith("/", StringComparison.Ordinal)
                || path.EndsWith(".psmdcp", StringComparison.Ordinal))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Search for a nuspec using the given path, or by the expanded folder path.
        /// The manifest path here is a shortcut to use the already constructed well
        /// known location, if this doesn't exist the folder reader will find the nuspec
        /// if it exists.
        /// </summary>
        private static NuspecReader GetNuspec(string manifestPath, string expandedPath)
        {
            NuspecReader nuspec = null;

            // Verify that the nuspec has the correct name before opening it
            if (File.Exists(manifestPath))
            {
                nuspec = new NuspecReader(File.OpenRead(manifestPath));
            }
            else
            {
                // Scan the folder for the nuspec
                using (var folderReader = new PackageFolderReader(expandedPath))
                {
                    // This will throw if the nuspec is not found
                    nuspec = new NuspecReader(folderReader.GetNuspec());
                }
            }

            return nuspec;
        }

        /// <summary>
        /// Return runtime.json from a package.
        /// </summary>
        private RuntimeGraph GetRuntimeGraph(string expandedPath)
        {
            var runtimeGraphFile = Path.Combine(expandedPath, RuntimeGraph.RuntimeGraphFileName);
            if (File.Exists(runtimeGraphFile))
            {
                using (var stream = File.OpenRead(runtimeGraphFile))
                {
                    return JsonRuntimeFormat.ReadRuntimeGraph(stream);
                }
            }

            return null;
        }
    }
}
