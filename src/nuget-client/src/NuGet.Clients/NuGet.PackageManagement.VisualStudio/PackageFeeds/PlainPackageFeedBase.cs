// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol.Core.Types;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// Most commonly used continuation token for plain package feeds.
    /// </summary>
    internal class FeedSearchContinuationToken : ContinuationToken
    {
        public int StartIndex { get; set; }
        public string SearchString { get; set; }
        public SearchFilter SearchFilter { get; set; }
    }

    /// <summary>
    /// Shared base implementation of plain package feeds.
    /// </summary>
    public abstract class PlainPackageFeedBase : IPackageFeed
    {
        public int PageSize { get; protected set; } = 100;

        // No, it's not.
        public bool IsMultiSource => false;

        public Task<SearchResult<IPackageSearchMetadata>> SearchAsync(string searchText, SearchFilter searchFilter, CancellationToken cancellationToken)
        {
            var searchToken = new FeedSearchContinuationToken
            {
                SearchString = searchText,
                SearchFilter = searchFilter,
                StartIndex = 0
            };

            return ContinueSearchAsync(searchToken, cancellationToken);
        }

        public abstract Task<SearchResult<IPackageSearchMetadata>> ContinueSearchAsync(ContinuationToken continuationToken, CancellationToken cancellationToken);

        public Task<SearchResult<IPackageSearchMetadata>> RefreshSearchAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
            => Task.FromResult(SearchResult.Empty<IPackageSearchMetadata>());
    }
}
