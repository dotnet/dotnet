// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Concurrent;
using System.Collections.Generic;
using NuGet.Common;
using NuGet.DependencyResolver;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Repositories;

namespace NuGet.Commands
{
    /// <summary>
    /// Caches providers for the RestoreCommand. This helper ensures that no resources are duplicated.
    /// </summary>
    public class RestoreCommandProvidersCache
    {
        private readonly ConcurrentDictionary<SourceRepository, IRemoteDependencyProvider> _remoteProviders
            = new ConcurrentDictionary<SourceRepository, IRemoteDependencyProvider>();

        private readonly ConcurrentDictionary<string, IRemoteDependencyProvider> _localProvider
            = new ConcurrentDictionary<string, IRemoteDependencyProvider>(PathUtility.GetStringComparerBasedOnOS());

        private readonly ConcurrentDictionary<string, NuGetv3LocalRepository> _globalCache
            = new ConcurrentDictionary<string, NuGetv3LocalRepository>(PathUtility.GetStringComparerBasedOnOS());

        private readonly ConcurrentDictionary<SourceRepository, IVulnerabilityInformationProvider> _vulnerabilityInformationProviders
            = new ConcurrentDictionary<SourceRepository, IVulnerabilityInformationProvider>();

        private readonly LocalPackageFileCache _fileCache = new LocalPackageFileCache();

        public RestoreCommandProviders GetOrCreate(
            string globalPackagesPath,
            IReadOnlyList<string> fallbackPackagesPaths,
            IReadOnlyList<SourceRepository> sources,
            SourceCacheContext cacheContext,
            ILogger log)
        {
            return GetOrCreate(globalPackagesPath, fallbackPackagesPaths, sources, cacheContext, log, updateLastAccess: false);
        }

        public RestoreCommandProviders GetOrCreate(
            string globalPackagesPath,
            IReadOnlyList<string> fallbackPackagesPaths,
            IReadOnlyList<SourceRepository> sources,
            SourceCacheContext cacheContext,
            ILogger log,
            bool updateLastAccess)
        {
            var isFallbackFolder = false;

            NuGetv3LocalRepository globalCache = _globalCache.GetOrAdd(globalPackagesPath,
                                                    (path) => new NuGetv3LocalRepository(path, _fileCache, isFallbackFolder, updateLastAccess));

            var local = _localProvider.GetOrAdd(globalPackagesPath, (path) =>
            {
                // Create a v3 file system source
                var pathSource = Repository.Factory.GetCoreV3(path, FeedType.FileSystemV3);

                // Do not throw or warn for global cache 
                return new SourceRepositoryDependencyProvider(
                    pathSource,
                    log,
                    cacheContext,
                    ignoreFailedSources: true,
                    ignoreWarning: true,
                    fileCache: _fileCache,
                    isFallbackFolderSource: isFallbackFolder);
            });

            var localProviders = new List<IRemoteDependencyProvider>() { local };
            var fallbackFolders = new List<NuGetv3LocalRepository>();

            isFallbackFolder = true;
            updateLastAccess = false;

            foreach (var fallbackPath in fallbackPackagesPaths)
            {
                var cache = _globalCache.GetOrAdd(fallbackPath, (path) => new NuGetv3LocalRepository(path, _fileCache, isFallbackFolder, updateLastAccess));
                fallbackFolders.Add(cache);

                var localProvider = _localProvider.GetOrAdd(fallbackPath, (path) =>
                {
                    // Create a v3 file system source
                    var pathSource = Repository.Factory.GetCoreV3(path, FeedType.FileSystemV3);

                    // Throw for fallback package folders
                    return new SourceRepositoryDependencyProvider(
                        pathSource,
                        log,
                        cacheContext,
                        ignoreFailedSources: false,
                        ignoreWarning: false,
                        fileCache: _fileCache,
                        isFallbackFolderSource: isFallbackFolder);
                });

                localProviders.Add(localProvider);
            }

            var remoteProviders = new List<IRemoteDependencyProvider>(sources.Count);

            isFallbackFolder = false;

            foreach (var source in sources)
            {
                var remoteProvider = _remoteProviders.GetOrAdd(source, (sourceRepo) => new SourceRepositoryDependencyProvider(
                    sourceRepo,
                    log,
                    cacheContext,
                    cacheContext.IgnoreFailedSources,
                    ignoreWarning: false,
                    fileCache: _fileCache,
                    isFallbackFolderSource: isFallbackFolder));

                remoteProviders.Add(remoteProvider);
            }

            var vulnerabilityInformationProviders = new List<IVulnerabilityInformationProvider>(sources.Count);
            foreach (SourceRepository source in sources)
            {
                IVulnerabilityInformationProvider provider = _vulnerabilityInformationProviders.GetOrAdd(source, s => new VulnerabilityInformationProvider(s, log));
                vulnerabilityInformationProviders.Add(provider);
            }

            return new RestoreCommandProviders(globalCache, fallbackFolders, localProviders, remoteProviders, _fileCache, vulnerabilityInformationProviders);
        }
    }
}
