// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// Represents a package feed enumerating installed packages.
    /// </summary>
    public class InstalledPackageFeed : PlainPackageFeedBase
    {
        internal readonly IEnumerable<PackageCollectionItem> _installedPackages;
        internal readonly IPackageMetadataProvider _metadataProvider;

        public InstalledPackageFeed(
            IEnumerable<PackageCollectionItem> installedPackages,
            IPackageMetadataProvider metadataProvider)
        {
            if (installedPackages == null)
            {
                throw new ArgumentNullException(nameof(installedPackages));
            }
            _installedPackages = installedPackages;

            if (metadataProvider == null)
            {
                throw new ArgumentNullException(nameof(metadataProvider));
            }
            _metadataProvider = metadataProvider;
        }

        public override async Task<SearchResult<IPackageSearchMetadata>> ContinueSearchAsync(ContinuationToken continuationToken, CancellationToken cancellationToken)
        {
            var searchToken = continuationToken as FeedSearchContinuationToken ?? throw new InvalidOperationException(Strings.Exception_InvalidContinuationToken);
            cancellationToken.ThrowIfCancellationRequested();

            IPackageSearchMetadata[] searchItems = await GetMetadataForPackagesAndSortAsync(PerformLookup(_installedPackages.GetLatest(), searchToken), searchToken.SearchFilter.IncludePrerelease, cancellationToken);

            return CreateResult(searchItems);
        }

        internal async Task<IPackageSearchMetadata[]> GetMetadataForPackagesAndSortAsync<T>(T[] packages, bool includePrerelease, CancellationToken cancellationToken) where T : PackageIdentity
        {
            cancellationToken.ThrowIfCancellationRequested();

            IEnumerable<IPackageSearchMetadata> items = await TaskCombinators.ThrottledAsync(
                packages,
                (p, t) => GetPackageMetadataAsync(p, includePrerelease, t),
                cancellationToken);

            // The packages were originally sorted which is important because we skip based on that sort
            // however, the asynchronous execution has randomly reordered the set. So, we need to resort.
            return items.OrderBy(p => p.Identity.Id).ToArray();
        }

        internal static T[] PerformLookup<T>(IEnumerable<T> items, FeedSearchContinuationToken token) where T : PackageIdentity
        {
            return items
                .Where(p => p.Id.IndexOf(token.SearchString, StringComparison.OrdinalIgnoreCase) != -1)
                .OrderBy(p => p.Id)
                .Skip(token.StartIndex)
                .ToArray();
        }

        internal static SearchResult<IPackageSearchMetadata> CreateResult(IPackageSearchMetadata[] items)
        {
            SearchResult<IPackageSearchMetadata> result = SearchResult.FromItems(items);

            var loadingStatus = result.Any() ? LoadingStatus.NoMoreItems : LoadingStatus.NoItemsFound; // No pagination on installed-based feeds
            result.SourceSearchStatus = new Dictionary<string, LoadingStatus>
            {
                { "Installed", loadingStatus }
            };

            return result;
        }

        internal virtual async Task<IPackageSearchMetadata> GetPackageMetadataAsync<T>(T identity, bool includePrerelease, CancellationToken cancellationToken) where T : PackageIdentity
        {
            cancellationToken.ThrowIfCancellationRequested();

            // first we try and load the metadata from a local package
            var packageMetadata = await _metadataProvider.GetLocalPackageMetadataAsync(identity, includePrerelease, cancellationToken);
            if (packageMetadata == null)
            {
                // and failing that we go to the network
                packageMetadata = await _metadataProvider.GetPackageMetadataAsync(identity, includePrerelease, cancellationToken);
            }
            return packageMetadata;
        }
    }
}
