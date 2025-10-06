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
        private readonly IPackageFeed _packageFeed;
        private SearchResult<IPackageSearchMetadata>? _lastMainFeedSearchResult;
        private SearchFilter? _lastSearchFilter;
        private readonly IReadOnlyCollection<PackageSourceContextInfo> _packageSources;
        private readonly IPackageMetadataProvider _packageMetadataProvider;
        private readonly IOwnerDetailsUriService? _ownerDetailsUriService;
        private readonly MemoryCache? _inMemoryObjectCache;

        private readonly CacheItemPolicy _cacheItemPolicy = new CacheItemPolicy
        {
            SlidingExpiration = ObjectCache.NoSlidingExpiration,
            AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration,
        };

        public SearchObject(
            IPackageFeed mainFeed,
            IPackageMetadataProvider packageMetadataProvider,
            IReadOnlyCollection<PackageSourceContextInfo> packageSources,
            MemoryCache? searchCache)
        {
            Assumes.NotNull(mainFeed);
            Assumes.NotNull(packageMetadataProvider);
            Assumes.NotNullOrEmpty(packageSources);

            _packageFeed = mainFeed;
            _packageSources = packageSources;
            _packageMetadataProvider = packageMetadataProvider;
            _ownerDetailsUriService = _packageMetadataProvider as IOwnerDetailsUriService;
            _inMemoryObjectCache = searchCache;
        }

        public async ValueTask<SearchResultContextInfo> SearchAsync(string searchText, SearchFilter filter, bool useRecommender, CancellationToken cancellationToken)
        {
            SearchResult<IPackageSearchMetadata>? feedResults = await _packageFeed.SearchAsync(searchText, filter, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();

            _lastMainFeedSearchResult = feedResults; // Store this so we can ContinueSearch, we don't store recommended as we only do that on the first search
            _lastSearchFilter = filter;

            var packageSearchMetadataContextInfoCollection = new List<PackageSearchMetadataContextInfo>(feedResults.Items.Count);
            foreach (IPackageSearchMetadata packageSearchMetadata in feedResults.Items)
            {
                IPackageSearchMetadata? localPackageSearchMetadata = null;

                // Attach local metadata in case we do not have an icon remotely, can try local metadata.
                localPackageSearchMetadata = await _packageMetadataProvider.GetOnlyLocalPackageMetadataAsync(packageSearchMetadata.Identity, cancellationToken);

                CacheBackgroundData(packageSearchMetadata, localPackageSearchMetadata, filter.IncludePrerelease);
                var knownOwners = CreateKnownOwners(packageSearchMetadata);
                packageSearchMetadataContextInfoCollection.Add(PackageSearchMetadataContextInfo.Create(packageSearchMetadata, knownOwners));
            }

            return new SearchResultContextInfo(
                packageSearchMetadataContextInfoCollection,
                feedResults.SourceSearchStatus.ToImmutableDictionary(),
                feedResults.NextToken != null,
                feedResults.OperationId);
        }

        public async ValueTask<SearchResultContextInfo> RefreshSearchAsync(CancellationToken cancellationToken)
        {
            Assumes.NotNull(_lastMainFeedSearchResult);
            Assumes.NotNull(_lastSearchFilter);

            SearchResult<IPackageSearchMetadata> refreshSearchResult = await _packageFeed.RefreshSearchAsync(
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

            SearchResult<IPackageSearchMetadata> continueSearchResult = await _packageFeed.ContinueSearchAsync(
                _lastMainFeedSearchResult.NextToken,
                cancellationToken);
            _lastMainFeedSearchResult = continueSearchResult;

            var packageItems = new List<PackageSearchMetadataContextInfo>(_lastMainFeedSearchResult.Items.Count);

            foreach (IPackageSearchMetadata packageSearchMetadata in _lastMainFeedSearchResult.Items)
            {
                CacheBackgroundData(packageSearchMetadata, _lastSearchFilter.IncludePrerelease);
                var knownOwners = CreateKnownOwners(packageSearchMetadata);
                packageItems.Add(PackageSearchMetadataContextInfo.Create(packageSearchMetadata, knownOwners));
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
                        ? await _packageFeed.SearchAsync(string.Empty, filter, cancellationToken)
                        : await _packageFeed.ContinueSearchAsync(nextToken, cancellationToken);

                while (searchResult.RefreshToken != null)
                {
                    searchResult = await _packageFeed.RefreshSearchAsync(searchResult.RefreshToken, cancellationToken);
                }
                totalCount += searchResult.Items?.Count() ?? 0;
                nextToken = searchResult.NextToken;
            } while (nextToken != null && totalCount < maxCount);

            return totalCount;
        }

        private void CacheBackgroundData(IPackageSearchMetadata packageSearchMetadata, bool includesPrerelease)
        {
            CacheBackgroundData(packageSearchMetadata, localPackageSearchMetadata: null, includesPrerelease);
        }

        private void CacheBackgroundData(IPackageSearchMetadata packageSearchMetadata, IPackageSearchMetadata? localPackageSearchMetadata, bool includesPrerelease)
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
            if (localPackageSearchMetadata?.IconUrl != null)
            {
                NuGetPackageFileService.AddLocalIconToCache(packageSearchMetadata.Identity, localPackageSearchMetadata.IconUrl);
            }

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

        private IReadOnlyList<KnownOwner>? CreateKnownOwners(IPackageSearchMetadata packageSearchMetadata)
        {
            if (_ownerDetailsUriService is null
                || !_ownerDetailsUriService.SupportsKnownOwners)
            {
                return null;
            }

            IReadOnlyList<string> ownersList = packageSearchMetadata.OwnersList;

            if (ownersList is null || ownersList.Count == 0)
            {
                return Array.Empty<KnownOwner>();
            }

            List<KnownOwner> knownOwners = new(capacity: ownersList.Count);

            foreach (string owner in ownersList)
            {
                Uri ownerDetailsUrl = _ownerDetailsUriService.GetOwnerDetailsUri(owner);
                KnownOwner knownOwner = new(owner, ownerDetailsUrl);
                knownOwners.Add(knownOwner);
            }

            return knownOwners;
        }
    }
}
