// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
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
            return GetOrCreate(
                globalPackagesPath,
                fallbackPackagesPaths,
                sources,
                auditSources: Array.Empty<SourceRepository>(),
                cacheContext,
                log,
                updateLastAccess: false);
        }

        public RestoreCommandProviders GetOrCreate(
            string globalPackagesPath,
            IReadOnlyList<string> fallbackPackagesPaths,
            IReadOnlyList<SourceRepository> sources,
            SourceCacheContext cacheContext,
            ILogger log,
            bool updateLastAccess)
        {
            return GetOrCreate(
                globalPackagesPath,
                fallbackPackagesPaths,
                sources,
                auditSources: Array.Empty<SourceRepository>(),
                cacheContext,
                log,
                updateLastAccess);
        }

        public RestoreCommandProviders GetOrCreate(
            string globalPackagesPath,
            IReadOnlyList<string> fallbackPackagesPaths,
            IReadOnlyList<SourceRepository> packageSources,
            IReadOnlyList<SourceRepository> auditSources,
            SourceCacheContext cacheContext,
            ILogger log,
            bool updateLastAccess)
        {
            NuGetv3LocalRepository globalPackages = CreateGlobalPackagedRepository(globalPackagesPath, updateLastAccess);
            List<NuGetv3LocalRepository> fallbackFolders = GetFallbackFolderRepositories(fallbackPackagesPaths);
            List<IRemoteDependencyProvider> localProviders = CreateLocalProviders(globalPackagesPath, fallbackPackagesPaths, cacheContext, log);
            List<IRemoteDependencyProvider> remoteProviders = CreateRemoveProviders(packageSources, cacheContext, log);
            IReadOnlyList<IVulnerabilityInformationProvider> vulnerabilityInformationProviders = CreateVulnerabilityProviders(packageSources, auditSources, log);

            return new RestoreCommandProviders(globalPackages, fallbackFolders, localProviders, remoteProviders, _fileCache, vulnerabilityInformationProviders);
        }

        private NuGetv3LocalRepository CreateGlobalPackagedRepository(string globalPackagesPath, bool updateLastAccess)
        {
            NuGetv3LocalRepository globalCache = _globalCache.GetOrAdd(globalPackagesPath,
                (path) => new NuGetv3LocalRepository(path, _fileCache, isFallbackFolder: false, updateLastAccess));
            return globalCache;
        }

        private List<NuGetv3LocalRepository> GetFallbackFolderRepositories(IReadOnlyList<string> fallbackPackagesPaths)
        {
            var fallbackFolders = new List<NuGetv3LocalRepository>();
            for (int i = 0; i < fallbackPackagesPaths.Count; i++)
            {
                var fallbackPath = fallbackPackagesPaths[i];
                var cache = _globalCache.GetOrAdd(fallbackPath, (path) => new NuGetv3LocalRepository(path, _fileCache, isFallbackFolder: true, updateLastAccessTime: false));
                fallbackFolders.Add(cache);
            }

            return fallbackFolders;
        }

        private List<IRemoteDependencyProvider> CreateLocalProviders(string globalPackagesPath, IReadOnlyList<string> fallbackPackagesPaths, SourceCacheContext cacheContext, ILogger log)
        {
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
                    isFallbackFolderSource: false);
            });

            var localProviders = new List<IRemoteDependencyProvider>(fallbackPackagesPaths.Count + 1) { local };

            for (int i = 0; i < fallbackPackagesPaths.Count; i++)
            {
                var fallbackPath = fallbackPackagesPaths[i];
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
                        isFallbackFolderSource: true);
                });

                localProviders.Add(localProvider);
            }

            return localProviders;
        }

        private List<IRemoteDependencyProvider> CreateRemoveProviders(IReadOnlyList<SourceRepository> sources, SourceCacheContext cacheContext, ILogger log)
        {
            var remoteProviders = new List<IRemoteDependencyProvider>(sources.Count);

            foreach (var source in sources)
            {
                var remoteProvider = _remoteProviders.GetOrAdd(source, (sourceRepo) => new SourceRepositoryDependencyProvider(
                    sourceRepo,
                    log,
                    cacheContext,
                    cacheContext.IgnoreFailedSources,
                    ignoreWarning: false,
                    fileCache: _fileCache,
                    isFallbackFolderSource: false));

                remoteProviders.Add(remoteProvider);
            }

            return remoteProviders;
        }

        private IReadOnlyList<IVulnerabilityInformationProvider> CreateVulnerabilityProviders(
            IReadOnlyList<SourceRepository> packageSources,
            IReadOnlyList<SourceRepository> auditSources,
            ILogger log)
        {
            IReadOnlyList<IVulnerabilityInformationProvider> result = auditSources.Count > 0
                ? CreateVulnerabilityProviders(auditSources, log, isAuditSource: true)
                : CreateVulnerabilityProviders(packageSources, log, isAuditSource: false);
            return result;

            IReadOnlyList<IVulnerabilityInformationProvider> CreateVulnerabilityProviders(IReadOnlyList<SourceRepository> sources, ILogger log, bool isAuditSource)
            {
                var vulnerabilityInformationProviders = new List<IVulnerabilityInformationProvider>(sources.Count);
                Func<SourceRepository, IVulnerabilityInformationProvider> factory = s => new VulnerabilityInformationProvider(s, log, isAuditSource: isAuditSource);

                for (int i = 0; i < sources.Count; i++)
                {
                    SourceRepository source = sources[i];
                    IVulnerabilityInformationProvider provider = _vulnerabilityInformationProviders.GetOrAdd(source, factory);
                    vulnerabilityInformationProviders.Add(provider);
                }

                return vulnerabilityInformationProviders;
            }
        }
    }
}
