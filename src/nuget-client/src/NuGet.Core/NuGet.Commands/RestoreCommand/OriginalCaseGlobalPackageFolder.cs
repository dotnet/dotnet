// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.DependencyResolver;
using NuGet.LibraryModel;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectModel;
using NuGet.Repositories;

namespace NuGet.Commands
{
    using LocalPackageArchiveDownloader = NuGet.Protocol.LocalPackageArchiveDownloader;

    public class OriginalCaseGlobalPackageFolder
    {
        private readonly List<NuGetv3LocalRepository> _localRepositories;
        private readonly RestoreRequest _request;
        private readonly ToolPathResolver _toolPathResolver;
        private readonly VersionFolderPathResolver _pathResolver;

        public Guid ParentId { get; }

        public OriginalCaseGlobalPackageFolder(RestoreRequest request) :
            this(request, Guid.Empty)
        {
        }

        public OriginalCaseGlobalPackageFolder(RestoreRequest request, Guid parentId)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            _request = request;
            ParentId = parentId;

            _localRepositories = new List<NuGetv3LocalRepository>();
            _localRepositories.Add(request.DependencyProviders.GlobalPackages);
            _localRepositories.AddRange(request.DependencyProviders.FallbackPackageFolders);

            _pathResolver = new VersionFolderPathResolver(
                _request.PackagesDirectory,
                _request.IsLowercasePackagesDirectory);

            _toolPathResolver = new ToolPathResolver(
                _request.PackagesDirectory,
                _request.IsLowercasePackagesDirectory);
        }

        public async Task CopyPackagesToOriginalCaseAsync(IEnumerable<RestoreTargetGraph> graphs, CancellationToken token)
        {
            // Keep track of the packages we've already converted to original case.
            var converted = new HashSet<PackageIdentity>();

            var originalCaseContext = GetPathContext();
            var versionFolderPathResolver = new VersionFolderPathResolver(_request.PackagesDirectory, _request.IsLowercasePackagesDirectory);

            // Iterate over every package node.
            foreach (var graph in graphs)
            {
                var packages = graph
                    .Flattened
                    .Select(graphItem => graphItem.Data.Match)
                    .Where(remoteMatch => remoteMatch.Library.Type == LibraryType.Package);

                foreach (var remoteMatch in packages)
                {
                    var identity = GetPackageIdentity(remoteMatch);
                    var hashPath = _pathResolver.GetNupkgMetadataPath(identity.Id, identity.Version);

                    // No need to re-install the same package identity more than once or if it is
                    // already installed.
                    if (!converted.Add(identity) || File.Exists(hashPath))
                    {
                        continue;
                    }

                    var localPackageSourceInfo = GetLocalPackageSourceInfo(remoteMatch);
                    var packageIdentity = new PackageIdentity(remoteMatch.Library.Name, remoteMatch.Library.Version);
                    IPackageDownloader packageDependency = null;

                    if (string.IsNullOrEmpty(localPackageSourceInfo?.Package.ZipPath))
                    {
                        packageDependency = await remoteMatch.Provider.GetPackageDownloaderAsync(
                            packageIdentity,
                            _request.CacheContext,
                            _request.Log,
                            token);
                    }
                    else
                    {
                        packageDependency = new LocalPackageArchiveDownloader(
                            localPackageSourceInfo.Repository.RepositoryRoot,
                            localPackageSourceInfo.Package.ZipPath,
                            packageIdentity,
                            _request.Log);
                    }

                    // Install the package.
                    using (packageDependency)
                    {
                        var result = await PackageExtractor.InstallFromSourceAsync(
                            identity,
                            packageDependency,
                            versionFolderPathResolver,
                            originalCaseContext,
                            token,
                            ParentId);

                        if (result)
                        {
                            _request.Log.LogInformation(string.Format(
                                CultureInfo.CurrentCulture,
                                Strings.Log_ConvertedPackageToOriginalCase,
                                identity));
                        }
                    }
                }
            }
        }

        public void ConvertLockFileToOriginalCase(LockFile lockFile)
        {
            var packageLibraries = lockFile
                .Libraries
                .Where(library => library.Type == LibraryType.Package);

            foreach (var library in packageLibraries)
            {
                var path = _pathResolver.GetPackageDirectory(library.Name, library.Version);
                library.Path = PathUtility.GetPathWithForwardSlashes(path);
            }
        }

        private PackageExtractionContext GetPathContext()
        {
            return new PackageExtractionContext(
                _request.PackageSaveMode,
                _request.XmlDocFileSaveMode,
                _request.ClientPolicyContext,
                _request.Log)
            {
                SignedPackageVerifier = _request.SignedPackageVerifier
            };
        }

        private static PackageIdentity GetPackageIdentity(RemoteMatch remoteMatch)
        {
            return new PackageIdentity(
                remoteMatch.Library.Name,
                remoteMatch.Library.Version);
        }

        private LocalPackageSourceInfo GetLocalPackageSourceInfo(RemoteMatch remoteMatch)
        {
            var library = remoteMatch.Library;

            // Try to get the package from the local repositories first.
            var localPackage = NuGetv3LocalRepositoryUtility.GetPackage(
                _localRepositories,
                library.Name,
                library.Version);

            if (localPackage != null && File.Exists(localPackage.Package.ZipPath))
            {
                return localPackage;
            }

            return null;
        }
    }
}
