// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.ServiceHub.Framework;
using Microsoft.ServiceHub.Framework.Services;
using Microsoft.VisualStudio.Services.Common;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.VisualStudio
{
    public sealed class NuGetPackageSearchService : INuGetSearchService
    {
        private readonly ServiceActivationOptions _options;
        private readonly IServiceBroker _serviceBroker;
        private readonly AuthorizationServiceClient _authorizationServiceClient;
        private SearchObject? _searchObject;
        private readonly ISharedServiceState _sharedServiceState;
        private readonly Microsoft.VisualStudio.Threading.AsyncLazy<SourceRepository> _packagesFolderLocalRepositoryLazy;
        private readonly Microsoft.VisualStudio.Threading.AsyncLazy<IReadOnlyList<SourceRepository>> _globalPackageFolderRepositoriesLazy;
        // internal for testing purposes only
        internal readonly static MemoryCache PackageSearchMetadataMemoryCache = new MemoryCache("PackageSearchMetadata",
            new NameValueCollection
            {
                { "cacheMemoryLimitMegabytes", "4" },
                { "physicalMemoryLimitPercentage", "0" },
                { "pollingInterval", "00:02:00" }
            });

        private readonly static CacheItemPolicy CacheItemPolicy = new CacheItemPolicy
        {
            SlidingExpiration = ObjectCache.NoSlidingExpiration,
            AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration,
        };

        public NuGetPackageSearchService(ServiceActivationOptions options, IServiceBroker sb, AuthorizationServiceClient ac, ISharedServiceState state)
        {
            _options = options;
            _serviceBroker = sb;
            _authorizationServiceClient = ac;
            _sharedServiceState = state;

            _packagesFolderLocalRepositoryLazy = new Microsoft.VisualStudio.Threading.AsyncLazy<SourceRepository>(
                GetPackagesFolderSourceRepositoryAsync,
                NuGetUIThreadHelper.JoinableTaskFactory);
            _globalPackageFolderRepositoriesLazy = new Microsoft.VisualStudio.Threading.AsyncLazy<IReadOnlyList<SourceRepository>>(
                GetGlobalPackageFolderRepositoriesAsync,
                NuGetUIThreadHelper.JoinableTaskFactory);
        }

        public async ValueTask<IReadOnlyCollection<PackageSearchMetadataContextInfo>> GetAllPackagesAsync(
            IReadOnlyCollection<IProjectContextInfo> projectContextInfos,
            IReadOnlyCollection<PackageSourceContextInfo> packageSources,
            IReadOnlyCollection<string> targetFrameworks,
            SearchFilter searchFilter,
            ItemFilter itemFilter,
            bool isSolution,
            CancellationToken cancellationToken)
        {
            Assumes.NotNullOrEmpty(projectContextInfos);
            Assumes.NotNullOrEmpty(packageSources);
            Assumes.NotNull(searchFilter);

            bool recommendPackages = false;
            IReadOnlyCollection<SourceRepository> sourceRepositories = await _sharedServiceState.GetRepositoriesAsync(packageSources, cancellationToken);
            (IPackageFeed? mainFeed, IPackageFeed? recommenderFeed) packageFeeds = await CreatePackageFeedAsync(
                projectContextInfos,
                targetFrameworks,
                itemFilter,
                isSolution,
                recommendPackages,
                sourceRepositories,
                cancellationToken);

            Assumes.NotNull(packageFeeds.mainFeed);

            SourceRepository packagesFolderSourceRepository = await _packagesFolderLocalRepositoryLazy.GetValueAsync(cancellationToken);
            IEnumerable<SourceRepository> globalPackageFolderRepositories = await GetAllPackageFoldersAsync(projectContextInfos, cancellationToken);
            var metadataProvider = new MultiSourcePackageMetadataProvider(
                sourceRepositories,
                packagesFolderSourceRepository,
                globalPackageFolderRepositories,
                new VisualStudioActivityLogger());

            var searchObject = new SearchObject(packageFeeds.mainFeed, packageFeeds.recommenderFeed, metadataProvider, packageSources, PackageSearchMetadataMemoryCache);
            return await searchObject.GetAllPackagesAsync(searchFilter, cancellationToken);
        }

        public async ValueTask<(PackageSearchMetadataContextInfo, PackageDeprecationMetadataContextInfo?)> GetPackageMetadataAsync(
            PackageIdentity identity,
            IReadOnlyCollection<PackageSourceContextInfo> packageSources,
            bool includePrerelease,
            CancellationToken cancellationToken)
        {
            Assumes.NotNull(identity);
            Assumes.NotNullOrEmpty(packageSources);

            string cacheId = PackageSearchMetadataCacheItem.GetCacheId(identity.Id, includePrerelease, packageSources);
            if (PackageSearchMetadataMemoryCache.Get(cacheId) is PackageSearchMetadataCacheItem backgroundDataCache)
            {
                PackageSearchMetadataCacheItemEntry cacheItem = await backgroundDataCache.GetPackageSearchMetadataCacheVersionedItemAsync(identity, cancellationToken);
                PackageSearchMetadataContextInfo packageSearchData = await cacheItem.DetailedPackageSearchMetadataContextInfo;
                PackageDeprecationMetadataContextInfo? deprecatedData = await cacheItem.PackageDeprecationMetadataContextInfo;
                return (packageSearchData, deprecatedData);
            }

            IPackageMetadataProvider packageMetadataProvider = await GetPackageMetadataProviderAsync(packageSources, cancellationToken);
            IPackageSearchMetadata packageMetadata = await packageMetadataProvider.GetPackageMetadataAsync(
                identity,
                includePrerelease,
                cancellationToken);

            PackageSearchMetadataContextInfo packageSearchMetadataContextInfo = PackageSearchMetadataContextInfo.Create(packageMetadata);
            PackageDeprecationMetadataContextInfo? deprecationMetadataContextInfo = null;

            PackageDeprecationMetadata? deprecationMetadata = await packageMetadata.GetDeprecationMetadataAsync();
            if (deprecationMetadata != null)
            {
                deprecationMetadataContextInfo = PackageDeprecationMetadataContextInfo.Create(deprecationMetadata);
            }

            return (packageSearchMetadataContextInfo, deprecationMetadataContextInfo);
        }

        public async ValueTask<IReadOnlyCollection<PackageSearchMetadataContextInfo>> GetPackageMetadataListAsync(
            string id,
            IReadOnlyCollection<PackageSourceContextInfo> packageSources,
            bool includePrerelease,
            bool includeUnlisted,
            CancellationToken cancellationToken)
        {
            Assumes.NotNullOrEmpty(id);
            Assumes.NotNullOrEmpty(packageSources);

            IPackageMetadataProvider packageMetadataProvider = await GetPackageMetadataProviderAsync(packageSources, cancellationToken);
            IEnumerable<IPackageSearchMetadata> packageMetadata = await packageMetadataProvider.GetPackageMetadataListAsync(
                id,
                includePrerelease,
                includeUnlisted,
                cancellationToken);

            return packageMetadata.Select(package => PackageSearchMetadataContextInfo.Create(package)).ToList();
        }

        public async ValueTask<IReadOnlyCollection<VersionInfoContextInfo>> GetPackageVersionsAsync(
            PackageIdentity identity,
            IReadOnlyCollection<PackageSourceContextInfo> packageSources,
            bool includePrerelease,
            CancellationToken cancellationToken)
        {
            Assumes.NotNull(identity);
            Assumes.NotNullOrEmpty(packageSources);

            string cacheId = PackageSearchMetadataCacheItem.GetCacheId(identity.Id, includePrerelease, packageSources);
            PackageSearchMetadataCacheItem? backgroundDataCache = PackageSearchMetadataMemoryCache.Get(cacheId) as PackageSearchMetadataCacheItem;
            if (backgroundDataCache != null)
            {
                return await backgroundDataCache.AllVersionsContextInfo;
            }

            IPackageMetadataProvider packageMetadataProvider = await GetPackageMetadataProviderAsync(packageSources, cancellationToken);
            IPackageSearchMetadata packageMetadata = await packageMetadataProvider.GetPackageMetadataAsync(identity, includePrerelease, cancellationToken);

            // Update the cache
            var cacheEntry = new PackageSearchMetadataCacheItem(packageMetadata, packageMetadataProvider);
            cacheEntry.UpdateSearchMetadata(packageMetadata);
            PackageSearchMetadataMemoryCache.AddOrGetExisting(cacheId, cacheEntry, CacheItemPolicy);

            return await cacheEntry.AllVersionsContextInfo;
        }

        public async ValueTask<IReadOnlyCollection<VersionInfoContextInfo>> GetPackageVersionsAsync(
            PackageIdentity identity,
            IReadOnlyCollection<PackageSourceContextInfo> packageSources,
            bool includePrerelease,
            bool isTransitive,
            CancellationToken cancellationToken)
        {
            return await GetPackageVersionsAsync(identity, packageSources, includePrerelease, isTransitive, projects: null, cancellationToken);
        }

        public async ValueTask<IReadOnlyCollection<VersionInfoContextInfo>> GetPackageVersionsAsync(
            PackageIdentity identity,
            IReadOnlyCollection<PackageSourceContextInfo> packageSources,
            bool includePrerelease,
            bool isTransitive,
            IEnumerable<IProjectContextInfo>? projects,
            CancellationToken cancellationToken)
        {
            Assumes.NotNull(identity);
            Assumes.NotNullOrEmpty(packageSources);

            string cacheId = PackageSearchMetadataCacheItem.GetCacheId(identity.Id, includePrerelease, packageSources);
            PackageSearchMetadataCacheItem? backgroundDataCache = PackageSearchMetadataMemoryCache.Get(cacheId) as PackageSearchMetadataCacheItem;

            // Transitive packages will have only one version the first time they are loaded, when the package is selected we update the cache with all the versions
            if (backgroundDataCache != null)
            {
                // If the item was cached with search API, PackageSearchMetadata could be null. If so, update it with registration api information
                if (isTransitive
                    && !(backgroundDataCache.AllVersionsContextInfo.Result?.Count > 1)
                    || backgroundDataCache.AllVersionsContextInfo.Result?.First().PackageSearchMetadata == null)
                {
                    IPackageMetadataProvider newPackageMetadataProvider = await GetPackageMetadataProviderAsync(packageSources, projects?.ToList().AsReadOnly(), cancellationToken);
                    IPackageSearchMetadata newPackageMetadata = await newPackageMetadataProvider.GetPackageMetadataAsync(identity, includePrerelease, cancellationToken);
                    backgroundDataCache.UpdateSearchMetadata(newPackageMetadata);
                }
                return await backgroundDataCache.AllVersionsContextInfo;
            }

            IPackageMetadataProvider packageMetadataProvider = await GetPackageMetadataProviderAsync(packageSources, projects?.ToList().AsReadOnly(), cancellationToken);
            IPackageSearchMetadata packageMetadata = await packageMetadataProvider.GetPackageMetadataAsync(identity, includePrerelease, cancellationToken);

            // Update the cache
            var cacheEntry = new PackageSearchMetadataCacheItem(packageMetadata, packageMetadataProvider);
            cacheEntry.UpdateSearchMetadata(packageMetadata);
            PackageSearchMetadataMemoryCache.AddOrGetExisting(cacheId, cacheEntry, CacheItemPolicy);

            return await cacheEntry.AllVersionsContextInfo;
        }


        public async ValueTask<PackageDeprecationMetadataContextInfo?> GetDeprecationMetadataAsync(
            PackageIdentity identity,
            IReadOnlyCollection<PackageSourceContextInfo> packageSources,
            bool includePrerelease,
            CancellationToken cancellationToken)
        {
            Assumes.NotNull(identity);
            Assumes.NotNullOrEmpty(packageSources);

            string cacheId = PackageSearchMetadataCacheItem.GetCacheId(identity.Id, includePrerelease, packageSources);
            PackageSearchMetadataCacheItem? backgroundDataCache = PackageSearchMetadataMemoryCache.Get(cacheId) as PackageSearchMetadataCacheItem;
            if (backgroundDataCache != null)
            {
                PackageSearchMetadataCacheItemEntry cacheItem = await backgroundDataCache.GetPackageSearchMetadataCacheVersionedItemAsync(identity, cancellationToken);
                return await cacheItem.PackageDeprecationMetadataContextInfo;
            }

            IPackageMetadataProvider packageMetadataProvider = await GetPackageMetadataProviderAsync(packageSources, cancellationToken);
            IPackageSearchMetadata packageMetadata = await packageMetadataProvider.GetPackageMetadataAsync(identity, includePrerelease, cancellationToken);
            PackageDeprecationMetadata deprecationMetadata = await packageMetadata.GetDeprecationMetadataAsync();
            if (deprecationMetadata == null)
            {
                return null;
            }
            return PackageDeprecationMetadataContextInfo.Create(deprecationMetadata);
        }

        public async ValueTask<SearchResultContextInfo> RefreshSearchAsync(CancellationToken cancellationToken)
        {
            Assumes.NotNull(_searchObject);
            return await _searchObject.RefreshSearchAsync(cancellationToken);
        }

        public async ValueTask<SearchResultContextInfo> ContinueSearchAsync(CancellationToken cancellationToken)
        {
            Assumes.NotNull(_searchObject);
            return await _searchObject.ContinueSearchAsync(cancellationToken);
        }

        public async ValueTask<SearchResultContextInfo> SearchAsync(
            IReadOnlyCollection<IProjectContextInfo> projectContextInfos,
            IReadOnlyCollection<PackageSourceContextInfo> packageSources,
            IReadOnlyCollection<string> targetFrameworks,
            string searchText,
            SearchFilter searchFilter,
            ItemFilter itemFilter,
            bool isSolution,
            bool useRecommender,
            CancellationToken cancellationToken)
        {
            Assumes.NotNullOrEmpty(projectContextInfos);
            Assumes.NotNullOrEmpty(packageSources);
            Assumes.NotNull(searchFilter);

            IReadOnlyCollection<SourceRepository>? sourceRepositories = await _sharedServiceState.GetRepositoriesAsync(packageSources, cancellationToken);
            (IPackageFeed? mainFeed, IPackageFeed? recommenderFeed) = await CreatePackageFeedAsync(
                projectContextInfos,
                targetFrameworks,
                itemFilter,
                isSolution,
                useRecommender,
                sourceRepositories,
                cancellationToken);
            Assumes.NotNull(mainFeed);

            SourceRepository packagesFolderSourceRepository = await _packagesFolderLocalRepositoryLazy.GetValueAsync(cancellationToken);
            IEnumerable<SourceRepository> globalPackageFolderRepositories = await GetAllPackageFoldersAsync(projectContextInfos, cancellationToken);
            var metadataProvider = new MultiSourcePackageMetadataProvider(
                sourceRepositories,
                packagesFolderSourceRepository,
                globalPackageFolderRepositories,
                new VisualStudioActivityLogger());

            _searchObject = new SearchObject(mainFeed, recommenderFeed, metadataProvider, packageSources, PackageSearchMetadataMemoryCache);
            return await _searchObject.SearchAsync(searchText, searchFilter, useRecommender, cancellationToken);
        }

        public async ValueTask<int> GetTotalCountAsync(
            int maxCount,
            IReadOnlyCollection<IProjectContextInfo> projectContextInfos,
            IReadOnlyCollection<PackageSourceContextInfo> packageSources,
            IReadOnlyCollection<string> targetFrameworks,
            SearchFilter searchFilter,
            ItemFilter itemFilter,
            bool isSolution,
            CancellationToken cancellationToken)
        {
            Assumes.NotNullOrEmpty(projectContextInfos);
            Assumes.NotNullOrEmpty(packageSources);
            Assumes.NotNull(searchFilter);

            IReadOnlyCollection<SourceRepository>? sourceRepositories = await _sharedServiceState.GetRepositoriesAsync(packageSources, cancellationToken);
            (IPackageFeed? mainFeed, IPackageFeed? recommenderFeed) = await CreatePackageFeedAsync(projectContextInfos, targetFrameworks, itemFilter, isSolution, recommendPackages: false, sourceRepositories, cancellationToken);
            Assumes.NotNull(mainFeed);

            SourceRepository packagesFolderSourceRepository = await _packagesFolderLocalRepositoryLazy.GetValueAsync(cancellationToken);
            IEnumerable<SourceRepository> globalPackageFolderRepositories = await GetAllPackageFoldersAsync(projectContextInfos, cancellationToken);
            var metadataProvider = new MultiSourcePackageMetadataProvider(
                sourceRepositories,
                packagesFolderSourceRepository,
                globalPackageFolderRepositories,
                new VisualStudioActivityLogger());

            var searchObject = new SearchObject(mainFeed, recommenderFeed, metadataProvider, packageSources, searchCache: null);
            return await searchObject.GetTotalCountAsync(maxCount, searchFilter, cancellationToken);
        }

        public void Dispose()
        {
            _authorizationServiceClient.Dispose();
        }

        private async ValueTask<IPackageMetadataProvider> GetPackageMetadataProviderAsync(
            IReadOnlyCollection<PackageSourceContextInfo> packageSources,
            CancellationToken cancellationToken)
        {
            return await GetPackageMetadataProviderAsync(packageSources, projects: null, cancellationToken);
        }

        private async ValueTask<IPackageMetadataProvider> GetPackageMetadataProviderAsync(
            IReadOnlyCollection<PackageSourceContextInfo> packageSources,
            IReadOnlyCollection<IProjectContextInfo>? projects,
            CancellationToken cancellationToken)
        {
            IReadOnlyCollection<SourceRepository> sourceRepositories = await _sharedServiceState.GetRepositoriesAsync(packageSources, cancellationToken);
            SourceRepository localRepo = await _packagesFolderLocalRepositoryLazy.GetValueAsync(cancellationToken);
            IEnumerable<SourceRepository> globalRepo;
            if (projects != null)
            {
                globalRepo = await GetAllPackageFoldersAsync(projects, cancellationToken);
            }
            else
            {
                globalRepo = await _globalPackageFolderRepositoriesLazy.GetValueAsync(cancellationToken);
            }

            return new MultiSourcePackageMetadataProvider(sourceRepositories, localRepo, globalRepo, new VisualStudioActivityLogger());
        }

        private async ValueTask<IReadOnlyCollection<IPackageReferenceContextInfo>> GetAllInstalledPackagesAsync(IReadOnlyCollection<IProjectContextInfo> projectContextInfos, CancellationToken cancellationToken)
        {
            IEnumerable<Task<IReadOnlyCollection<IPackageReferenceContextInfo>>> tasks = projectContextInfos
                .Select(project => project.GetInstalledPackagesAsync(_serviceBroker, cancellationToken).AsTask());
            IReadOnlyCollection<IPackageReferenceContextInfo>[] packageReferences = await Task.WhenAll(tasks);
            return packageReferences.SelectMany(e => e).ToList();
        }

        /// <summary>
        /// Combines package folders from PackageReferenceProject with global package folders
        /// </summary>
        /// <param name="projectContextInfos">A collection of projects</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A collection of Global package folder repositories combined with repositories found in packageFolders from PackageReference projects</returns>
        public async Task<IReadOnlyList<SourceRepository>> GetAllPackageFoldersAsync(
            IReadOnlyCollection<IProjectContextInfo> projectContextInfos,
            CancellationToken cancellationToken)
        {
            Task<IReadOnlyCollection<string>>[] tasks = projectContextInfos.Select(pctxi => pctxi.GetPackageFoldersAsync(_serviceBroker, cancellationToken).AsTask()).ToArray();
            IReadOnlyCollection<string>[] packageFolders = await Task.WhenAll(tasks);

            HashSet<string> pkgFoldersUnique = new HashSet<string>();
            packageFolders.ForEach(folders => pkgFoldersUnique.AddRange(folders));

            IEnumerable<SourceRepository> assetsPackageFolders = pkgFoldersUnique.Select(folder => _sharedServiceState.SourceRepositoryProvider.CreateRepository(new PackageSource(folder)));
            IEnumerable<SourceRepository> globalPackageFolderRepositories = await _globalPackageFolderRepositoriesLazy.GetValueAsync(cancellationToken);
            List<SourceRepository> allLocalFolders = globalPackageFolderRepositories
                .Concat(assetsPackageFolders)
                .GroupBy(source => source?.PackageSource?.Source) // remove duplicates
                .Select(group => group.First())
                .ToList();

            return allLocalFolders;
        }

        internal async Task<(IPackageFeed? mainFeed, IPackageFeed? recommenderFeed)> CreatePackageFeedAsync(
            IReadOnlyCollection<IProjectContextInfo> projectContextInfos,
            IReadOnlyCollection<string> targetFrameworks,
            ItemFilter itemFilter,
            bool isSolution,
            bool recommendPackages,
            IEnumerable<SourceRepository> sourceRepositories,
            CancellationToken cancellationToken)
        {
            var logger = new VisualStudioActivityLogger();
            var uiLogger = await ServiceLocator.GetComponentModelServiceAsync<INuGetUILogger>();
            var packageFeeds = (mainFeed: (IPackageFeed?)null, recommenderFeed: (IPackageFeed?)null);

            if (itemFilter == ItemFilter.All && recommendPackages == false)
            {
                packageFeeds.mainFeed = new MultiSourcePackageFeed(sourceRepositories, uiLogger, TelemetryActivity.NuGetTelemetryService);
                return packageFeeds;
            }

            IEnumerable<SourceRepository> globalPackageFolderRepositories = await GetAllPackageFoldersAsync(projectContextInfos, cancellationToken);
            SourceRepository packagesFolderSourceRepository = await _packagesFolderLocalRepositoryLazy.GetValueAsync(cancellationToken);
            var metadataProvider = new MultiSourcePackageMetadataProvider(
                sourceRepositories,
                packagesFolderSourceRepository,
                globalPackageFolderRepositories,
                logger);

            if (itemFilter == ItemFilter.All)
            {
                // Browse Tab, Project or Solution View: no need of transitive origins data.
                IInstalledAndTransitivePackages browseTabPackages = await PackageCollection.GetInstalledAndTransitivePackagesAsync(_serviceBroker, projectContextInfos, includeTransitiveOrigins: false, cancellationToken);
                PackageCollection installedPackageCollection = PackageCollection.FromPackageReferences(browseTabPackages.InstalledPackages);
                PackageCollection transitivePackageCollection = PackageCollection.FromPackageReferences(browseTabPackages.TransitivePackages);

                // if we get here, recommendPackages == true
                packageFeeds.mainFeed = new MultiSourcePackageFeed(sourceRepositories, uiLogger, TelemetryActivity.NuGetTelemetryService);
                try
                {
                    // Recommender needs installed and transitive package lists, but it does not need transitive origins data.
                    packageFeeds.recommenderFeed = new RecommenderPackageFeed(
                        sourceRepositories,
                        installedPackageCollection,
                        transitivePackageCollection,
                        targetFrameworks,
                        metadataProvider,
                        logger);
                }
                catch (System.IO.FileNotFoundException)
                {
                    // This could happen if the user disables the recommender extension. Catching this
                    // exception allows the package manager to continue without recommendations.
                }
                return packageFeeds;
            }

            if (itemFilter == ItemFilter.Installed)
            {
                if (isSolution)
                {
                    // Installed Tab, Solution View: only needs installed packages.
                    IReadOnlyCollection<IPackageReferenceContextInfo> installedSolutionTabPackages = await GetAllInstalledPackagesAsync(projectContextInfos, cancellationToken);
                    PackageCollection installedPackageCollection = PackageCollection.FromPackageReferences(installedSolutionTabPackages);
                    packageFeeds.mainFeed = new InstalledPackageFeed(installedPackageCollection, metadataProvider);
                }
                else // is Project
                {
                    // Installed Tab, Project View, Experiment On: needs installed, transitive packages and transitive origins data
                    IInstalledAndTransitivePackages installedTabWithTransitiveOrigins = await PackageCollection.GetInstalledAndTransitivePackagesAsync(_serviceBroker, projectContextInfos, includeTransitiveOrigins: true, cancellationToken);
                    PackageCollection installedPackageCollection = PackageCollection.FromPackageReferences(installedTabWithTransitiveOrigins.InstalledPackages);
                    PackageCollection transitivePackageCollection = PackageCollection.FromPackageReferences(installedTabWithTransitiveOrigins.TransitivePackages);

                    packageFeeds.mainFeed = new InstalledAndTransitivePackageFeed(installedPackageCollection, transitivePackageCollection, metadataProvider);
                }

                return packageFeeds;
            }

            if (itemFilter == ItemFilter.Consolidate)
            {
                // Consolidate tab, Solution View only: only needs installed packages
                IReadOnlyCollection<IPackageReferenceContextInfo> installedTabPackages = await GetAllInstalledPackagesAsync(projectContextInfos, cancellationToken);
                PackageCollection installedPackageCollection = PackageCollection.FromPackageReferences(installedTabPackages);

                packageFeeds.mainFeed = new ConsolidatePackageFeed(installedPackageCollection, metadataProvider, logger);
                return packageFeeds;
            }

            // Search all / updates available cannot work without a source repo
            if (sourceRepositories == null)
            {
                return packageFeeds;
            }

            if (itemFilter == ItemFilter.UpdatesAvailable)
            {
                // Updates tab, Project or Solution View: only needs installed packages
                IReadOnlyCollection<IPackageReferenceContextInfo> updatedTabPackages = await GetAllInstalledPackagesAsync(projectContextInfos, cancellationToken);
                PackageCollection installedPackageCollection = PackageCollection.FromPackageReferences(updatedTabPackages);

                packageFeeds.mainFeed = new UpdatePackageFeed(
                    _serviceBroker,
                    installedPackageCollection,
                    metadataProvider,
                    projectContextInfos.ToArray());

                return packageFeeds;
            }

            throw new InvalidOperationException(
                string.Format(CultureInfo.CurrentCulture, Strings.UnsupportedFeedType, itemFilter));
        }

        private async Task<IReadOnlyList<SourceRepository>> GetGlobalPackageFolderRepositoriesAsync()
        {
            NuGetPackageManager packageManager = await _sharedServiceState.GetPackageManagerAsync(CancellationToken.None);

            return packageManager.GlobalPackageFolderRepositories;
        }

        private async Task<SourceRepository> GetPackagesFolderSourceRepositoryAsync()
        {
            IVsSolutionManager solutionManager = await _sharedServiceState.SolutionManager.GetValueAsync();
            ISettings settings = await ServiceLocator.GetComponentModelServiceAsync<ISettings>();

            return _sharedServiceState.SourceRepositoryProvider.CreateRepository(
                new PackageSource(PackagesFolderPathUtility.GetPackagesFolderPath(solutionManager, settings)),
                FeedType.FileSystemPackagesConfig);
        }
    }
}
