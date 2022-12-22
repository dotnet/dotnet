// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Threading;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;

namespace NuGet.PackageManagement
{
    internal class PackagesConfigContentHashProvider : IPackagesConfigContentHashProvider
    {
        private readonly FolderNuGetProject _packagesFolder;

        internal PackagesConfigContentHashProvider(FolderNuGetProject packagesFolder)
        {
            _packagesFolder = packagesFolder;
        }

        public string GetContentHash(PackageIdentity packageIdentity, CancellationToken token)
        {
            var nupkgPath = GetNupkgPath(packageIdentity, token);
            var result = TryGetNupkgMetadata(nupkgPath);

            if (!result.Found)
            {
                var contentHash = GetContentHashFromNupkg(nupkgPath, token);
                result = new Result(found: true, contentHash: contentHash);

                WriteNupkgMetadata(nupkgPath, result.ContentHash);
            }

            return result.ContentHash;
        }

        private string GetNupkgPath(PackageIdentity packageIdentity, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var packagePath = _packagesFolder.GetInstalledPackageFilePath(packageIdentity);
            return packagePath;
        }

        private Result TryGetNupkgMetadata(string nupkgPath)
        {
            var metadataPath = Path.Combine(Path.GetDirectoryName(nupkgPath), PackagingCoreConstants.NupkgMetadataFileExtension);
            if (File.Exists(metadataPath))
            {
                var metadata = NupkgMetadataFileFormat.Read(metadataPath);
                return new Result(found: true, contentHash: metadata.ContentHash);
            }

            return Result.NotFound;
        }

        private string GetNupkgMetadataPath(string nupkgPath)
        {
            return Path.Combine(Path.GetDirectoryName(nupkgPath), PackagingCoreConstants.NupkgMetadataFileExtension);
        }

        private string GetContentHashFromNupkg(string filePath, CancellationToken token)
        {
            using (var reader = new PackageArchiveReader(filePath))
            {
                var hash = reader.GetContentHash(token);
                return hash;
            }
        }

        private void WriteNupkgMetadata(string nupkgPath, string contentHash)
        {
            var metadataPath = GetNupkgMetadataPath(nupkgPath);

            var metadata = new NupkgMetadataFile()
            {
                ContentHash = contentHash
            };

            NupkgMetadataFileFormat.Write(metadataPath, metadata);
        }

        private struct Result
        {
            public static readonly Result NotFound = new Result(found: false, contentHash: null);

            public Result(bool found, string contentHash)
            {
                Found = found;
                ContentHash = contentHash;
            }

            public bool Found { get; }
            public string ContentHash { get; }
        }
    }
}
