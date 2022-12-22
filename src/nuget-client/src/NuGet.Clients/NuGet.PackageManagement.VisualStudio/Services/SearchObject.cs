// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.VisualStudio.Threading;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.VisualStudio.Internal.Contracts;
using static NuGet.Protocol.Core.Types.PackageSearchMetadataBuilder;

namespace NuGet.PackageManagement.VisualStudio
{
    internal sealed class SearchObject
    {
        private readonly IPackageFeed _mainFeed;
        private readonly IPackageFeed? _recommenderFeed;
        private SearchResult<IPackageSearchMetadata>? _lastMainFeedSearchResult;
        private SearchFilter? _lastSearchFilter;
        private readonly IReadOnlyCollection<PackageSourceContextInfo> _packageSources;
        private readonly IPackageMetadataProvider _packageMetadataProvider;
        private readonly MemoryCache? _inMemoryObjectCache;

        private readonly CacheItemPolicy _cacheItemPolicy = new CacheItemPolicy
        {
            SlidingExpiration = ObjectCache.NoSlidingExpiration,
            AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration,
        };

        public SearchObject(
            IPackageFeed mainFeed,
            IPackageFeed? recommenderFeed,
            IPackageMetadataProvider packageMetadataProvider,
            IReadOnlyCollection<PackageSourceContextInfo> packageSources,
            MemoryCache? searchCache)
        {
            Assumes.NotNull(mainFeed);
            Assumes.NotNull(packageMetadataProvider);
            Assumes.NotNullOrEmpty(packageSources);

            _mainFeed = mainFeed;
            _recommenderFeed = recommenderFeed;
            _packageSources = packageSources;
            _packageMetadataProvider = packageMetadataProvider;
            _inMemoryObjectCache = searchCache;
        }

        public async ValueTask<SearchResultContextInfo> SearchAsync(string searchText, SearchFilter filter, bool useRecommender, CancellationToken cancellationToken)
        {
            SearchResult<IPackageSearchMetadata>? mainFeedResult = await _mainFeed.SearchAsync(searchText, filter, cancellationToken);
            SearchResult<IPackageSearchMetadata>? recommenderFeedResults = null;

            if (useRecommender && _recommenderFeed != null)
            {
                recommenderFeedResults = await _recommenderFeed.SearchAsync(searchText, filter, cancellationToken);
            }

            cancellationToken.ThrowIfCancellationRequested();

            _lastMainFeedSearchResult = mainFeedResult; // Store this so we can ContinueSearch, we don't store recommended as we only do that on the first search
            _lastSearchFilter = filter;

            if (recommenderFeedResults != null)
            {
                // remove duplicated recommended packages from the browse results
                List<string> recommendedIds = recommenderFeedResults.Items.Select(item => item.Identity.Id).ToList();
                var recommendedPackageSearchMetadataContextInfo = new List<PackageSearchMetadataContextInfo>();

                foreach (IPackageSearchMetadata recommendedFeedResultItem in recommenderFeedResults.Items)
                {
                    CacheBackgroundData(recommendedFeedResultItem, filter.IncludePrerelease);
                    recommendedPackageSearchMetadataContextInfo.Add(
                        PackageSearchMetadataContextInfo.Create(
                            recommendedFeedResultItem,
                            isRecommended: true,
                            recommenderVersion: (_recommenderFeed as RecommenderPackageFeed)?.VersionInfo));
                }

                foreach (IPackageSearchMetadata mainFeedResultItem in mainFeedResult.Items)
                {
                    if (!recommendedIds.Contains(mainFeedResultItem.Identity.Id))
                    {
                        CacheBackgroundData(mainFeedResultItem, filter.IncludePrerelease);
                        recommendedPackageSearchMetadataContextInfo.Add(PackageSearchMetadataContextInfo.Create(mainFeedResultItem));
                    }
                }

                return new SearchResultContextInfo(
                    recommendedPackageSearchMetadataContextInfo.ToList(),
                    mainFeedResult.SourceSearchStatus.ToImmutableDictionary(),
                    mainFeedResult.NextToken != null,
                    mainFeedResult.OperationId);
            }

            var packageSearchMetadataContextInfoCollection = new List<PackageSearchMetadataContextInfo>(mainFeedResult.Items.Count);
            foreach (IPackageSearchMetadata packageSearchMetadata in mainFeedResult.Items)
            {
                CacheBackgroundData(packageSearchMetadata, filter.IncludePrerelease);
                packageSearchMetadataContextInfoCollection.Add(PackageSearchMetadataContextInfo.Create(packageSearchMetadata));
            }

            return new SearchResultContextInfo(
                packageSearchMetadataContextInfoCollection,
                mainFeedResult.SourceSearchStatus.ToImmutableDictionary(),
                mainFeedResult.NextToken != null,
                mainFeedResult.OperationId);
        }

        public async ValueTask<SearchResultContextInfo> RefreshSearchAsync(CancellationToken cancellationToken)
        {
            Assumes.NotNull(_lastMainFeedSearchResult);
            Assumes.NotNull(_lastSearchFilter);

            SearchResult<IPackageSearchMetadata> refreshSearchResult = await _mainFeed.RefreshSearchAsync(
                _lastMainFeedSearchResult.RefreshToken,
                cancellationToken);
            _lastMainFeedSearchResult = refreshSearchResult;

            var packageItems = new List<PackageSearchMetadataContextInfo>(_lastMainFeedSearchResult.Items.Count);

            foreach (IPackageSearchMetadata packageSearchMetadata in _lastMainFeedSearchResult.Items)
            {
                CacheBackgroundData(packageSearchMetadata, _lastSearchFilter.IncludePrerelease);
                packageItems.Add(PackageSearchMetadataContextInfo.Create(packageSearchMetadata));
            }

            return new SearchResultContextInfo(
                packageItems,
                refreshSearchResult.SourceSearchStatus.ToImmutableDictionary(),
                refreshSearchResult.NextToken != null,
                _lastMainFeedSearchResult.OperationId);
        }

        public async ValueTask<IReadOnlyCollection<PackageSearchMetadataContextInfo>> GetAllPackagesAsync(SearchFilter searchFilter, CancellationToken cancellationToken)
        {
            var packages = new List<PackageSearchMetadataContextInfo>();
            do
            {
                SearchResultContextInfo searchResult = _lastMainFeedSearchResult?.NextToken != null
                    ? await ContinueSearchAsync(cancellationToken)
                    : await SearchAsync(string.Empty, searchFilter, useRecommender: false, cancellationToken);

                if (_lastMainFeedSearchResult?.RefreshToken != null)
                {
                    searchResult = await RefreshSearchAsync(cancellationToken);
                }

                packages.AddRange(searchResult.PackageSearchItems);
            } while (_lastMainFeedSearchResult?.NextToken != null);

            return packages;
        }

        public async ValueTask<SearchResultContextInfo> ContinueSearchAsync(CancellationToken cancellationToken)
        {
            Assumes.NotNull(_lastMainFeedSearchResult);
            Assumes.NotNull(_lastSearchFilter);

            if (_lastMainFeedSearchResult.NextToken == null)
            {
                return new SearchResultContextInfo(_lastMainFeedSearchResult.OperationId);
            }

            SearchResult<IPackageSearchMetadata> continueSearchResult = await _mainFeed.ContinueSearchAsync(
                _lastMainFeedSearchResult.NextToken,
                cancellationToken);
            _lastMainFeedSearchResult = continueSearchResult;

            var packageItems = new List<PackageSearchMetadataContextInfo>(_lastMainFeedSearchResult.Items.Count);

            foreach (IPackageSearchMetadata packageSearchMetadata in _lastMainFeedSearchResult.Items)
            {
                CacheBackgroundData(packageSearchMetadata, _lastSearchFilter.IncludePrerelease);
                packageItems.Add(PackageSearchMetadataContextInfo.Create(packageSearchMetadata));
            }

            return new SearchResultContextInfo(
                packageItems,
                continueSearchResult.SourceSearchStatus.ToImmutableDictionary(),
                continueSearchResult.NextToken != null,
                _lastMainFeedSearchResult.OperationId);
        }

        public async ValueTask<int> GetTotalCountAsync(int maxCount, SearchFilter filter, CancellationToken cancellationToken)
        {
            // Go off the UI thread to perform non-UI operations
            await TaskScheduler.Default;

            ActivityCorrelationId.StartNew();

            int totalCount = 0;
            ContinuationToken? nextToken = null;
            do
            {
                SearchResult<IPackageSearchMetadata> searchResult = nextToken == null
                        ? await _mainFeed.SearchAsync(string.Empty, filter, cancellationToken)
                        : await _mainFeed.ContinueSearchAsync(nextToken, cancellationToken);

                while (searchResult.RefreshToken != null)
                {
                    searchResult = await _mainFeed.RefreshSearchAsync(searchResult.RefreshToken, cancellationToken);
                }
                totalCount += searchResult.Items?.Count() ?? 0;
                nextToken = searchResult.NextToken;
            } while (nextToken != null && totalCount < maxCount);

            return totalCount;
        }

        private void CacheBackgroundData(IPackageSearchMetadata packageSearchMetadata, bool includesPrerelease)
        {
            if (_inMemoryObjectCache == null)
            {
                return;
            }

            Assumes.NotNull(packageSearchMetadata);

            // If nothing is in the cache this will return null
            object? cacheObject = _inMemoryObjectCache.AddOrGetExisting(
                    PackageSearchMetadataCacheItem.GetCacheId(packageSearchMetadata.Identity.Id, includesPrerelease, _packageSources),
                    new PackageSearchMetadataCacheItem(packageSearchMetadata, _packageMetadataProvider),
                    _cacheItemPolicy);

            var memoryCacheItem = cacheObject as PackageSearchMetadataCacheItem;
            if (memoryCacheItem != null)
            {
                memoryCacheItem.UpdateSearchMetadata(packageSearchMetadata);
            }

            NuGetPackageFileService.AddIconToCache(packageSearchMetadata.Identity, packageSearchMetadata.IconUrl);

            string? packagePath = (packageSearchMetadata as LocalPackageSearchMetadata)?.PackagePath ??
                    (packageSearchMetadata as ClonedPackageSearchMetadata)?.PackagePath;

            if (packagePath != null)
            {
                LicenseMetadata? licenseMetadata = (packageSearchMetadata as LocalPackageSearchMetadata)?.LicenseMetadata ??
                    (packageSearchMetadata as ClonedPackageSearchMetadata)?.LicenseMetadata;
                if (licenseMetadata != null)
                {
                    NuGetPackageFileService.AddLicenseToCache(
                        packageSearchMetadata.Identity,
                        CreateEmbeddedLicenseUri(packagePath, licenseMetadata));
                }
            }
        }

        private static Uri CreateEmbeddedLicenseUri(string packagePath, LicenseMetadata licenseMetadata)
        {
            Uri? baseUri = Convert(packagePath);

            var builder = new UriBuilder(baseUri)
            {
                Fragment = licenseMetadata.License
            };

            return builder.Uri;
        }

        /// <summary>
        /// Convert a string to a URI safely. This will return null if there are errors.
        /// </summary>
        private static Uri? Convert(string uri)
        {
            Uri? fullUri = null;

            if (!string.IsNullOrEmpty(uri))
            {
                Uri.TryCreate(uri, UriKind.Absolute, out fullUri);
            }

            return fullUri;
        }
    }
}
