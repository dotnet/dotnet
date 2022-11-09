// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Packaging.Core;

namespace NuGet.Protocol
{
    /// <summary>
    /// Unzipped package repository reader used for project templates.
    /// </summary>
    public class FindLocalPackagesResourceUnzipped : FindLocalPackagesResource
    {
        // Read the packages once
        private readonly Lazy<IReadOnlyList<LocalPackageInfo>> _packages;
        private readonly Lazy<Dictionary<PackageIdentity, LocalPackageInfo>> _index;
        private readonly Lazy<Dictionary<Uri, LocalPackageInfo>> _pathIndex;

        public FindLocalPackagesResourceUnzipped(string root)
        {
            Root = root;
            _packages = new Lazy<IReadOnlyList<LocalPackageInfo>>(() => GetPackagesCore(root));
            _index = new Lazy<Dictionary<PackageIdentity, LocalPackageInfo>>(() => GetIndex(_packages));
            _pathIndex = new Lazy<Dictionary<Uri, LocalPackageInfo>>(() => GetPathIndex(_packages));
        }

        public override IEnumerable<LocalPackageInfo> FindPackagesById(string id, ILogger logger, CancellationToken token)
        {
            return _packages.Value.Where(package => StringComparer.OrdinalIgnoreCase.Equals(id, package.Identity.Id)).ToArray();
        }

        public override LocalPackageInfo GetPackage(Uri path, ILogger logger, CancellationToken token)
        {
            LocalPackageInfo package;
            _pathIndex.Value.TryGetValue(path, out package);
            return package;
        }

        public override LocalPackageInfo GetPackage(PackageIdentity identity, ILogger logger, CancellationToken token)
        {
            LocalPackageInfo package;
            _index.Value.TryGetValue(identity, out package);
            return package;
        }

        public override IEnumerable<LocalPackageInfo> GetPackages(ILogger logger, CancellationToken token)
        {
            return _packages.Value;
        }

        public override bool Exists(PackageIdentity identity, ILogger logger, CancellationToken token)
        {
            return _index.Value.ContainsKey(identity);
        }

        /// <summary>
        /// Id + Version -> Package
        /// </summary>
        private static Dictionary<PackageIdentity, LocalPackageInfo> GetIndex(Lazy<IReadOnlyList<LocalPackageInfo>> packages)
        {
            var index = new Dictionary<PackageIdentity, LocalPackageInfo>();

            foreach (var package in packages.Value)
            {
                if (!index.ContainsKey(package.Identity))
                {
                    index.Add(package.Identity, package);
                }
            }

            return index;
        }

        /// <summary>
        /// Uri -> Package
        /// </summary>
        private static Dictionary<Uri, LocalPackageInfo> GetPathIndex(Lazy<IReadOnlyList<LocalPackageInfo>> packages)
        {
            var index = new Dictionary<Uri, LocalPackageInfo>();

            foreach (var package in packages.Value)
            {
                var path = UriUtility.CreateSourceUri(package.Path, UriKind.Absolute);

                if (!index.ContainsKey(path))
                {
                    index.Add(path, package);
                }
            }

            return index;
        }

        private static IReadOnlyList<LocalPackageInfo> GetPackagesCore(string root)
        {
            var rootDirInfo = LocalFolderUtility.GetAndVerifyRootDirectory(root);

            if (!rootDirInfo.Exists)
            {
                return new List<LocalPackageInfo>();
            }

            var files = rootDirInfo.GetFiles("*" + PackagingCoreConstants.NupkgExtension, SearchOption.TopDirectoryOnly);
            var directories = rootDirInfo.GetDirectories("*", SearchOption.TopDirectoryOnly);

            // Find all packages that have both a nupkg and a directory
            var validSet = new HashSet<string>(directories.Select(dir => dir.Name), StringComparer.OrdinalIgnoreCase);
            validSet.IntersectWith(files.Select(file => Path.GetFileNameWithoutExtension(file.Name)));

            var result = new List<LocalPackageInfo>(validSet.Count);

            foreach (var name in validSet)
            {
                var nuspec = GetNuspec(rootDirInfo, name);
                var nupkgPath = Path.Combine(rootDirInfo.FullName, $"{name}{PackagingCoreConstants.NupkgExtension}");

                var localPackage = new LocalPackageInfo(
                    nuspec.GetIdentity(),
                    nupkgPath,
                    DateTime.UtcNow,
                    new Lazy<NuspecReader>(() => nuspec),
                    useFolder: true
                );

                result.Add(localPackage);
            }

            return result;
        }

        private static PackageReaderBase GetPackage(DirectoryInfo root, string name)
        {
            var packageRoot = Path.Combine(root.FullName, name);
            return new PackageFolderReader(packageRoot);
        }

        private static NuspecReader GetNuspec(DirectoryInfo root, string name)
        {
            // nuspecs are stored as id.version.nuspec
            var nuspecPath = Path.Combine(root.FullName, name, $"{name}{PackagingCoreConstants.NuspecExtension}");

            if (File.Exists(nuspecPath))
            {
                return new NuspecReader(nuspecPath);
            }
            else
            {
                // If the nuspec did not exist try the nupkg, we know this exists
                var nupkgPath = Path.Combine(root.FullName, $"{name}{PackagingCoreConstants.NupkgExtension}");

                using (var packageReader = new PackageArchiveReader(nupkgPath))
                {
                    return packageReader.NuspecReader;
                }
            }
        }
    }
}
