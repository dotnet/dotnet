// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol.Core.Types;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.VisualStudio
{
    public sealed class PackageFeedEnumerator : IEnumerator<IPackageSearchMetadata>, IDisposable, IEnumerator
    {
        private readonly IPackageFeed _packageFeed;
        private readonly Task<SearchResult<IPackageSearchMetadata>> _startFromTask;
        private readonly Action<string, Exception> _handleException;
        private readonly CancellationToken _cancellationToken;

        private Task<SearchResult<IPackageSearchMetadata>> _searchTask;
        private IEnumerator<IPackageSearchMetadata> _current;

        private bool _isDisposed;

        private PackageFeedEnumerator(
            IPackageFeed packageFeed,
            Task<SearchResult<IPackageSearchMetadata>> searchTask,
            Action<string, Exception> handleException,
            CancellationToken cancellationToken)
        {
            if (packageFeed == null)
            {
                throw new ArgumentNullException(nameof(packageFeed));
            }

            if (searchTask == null)
            {
                throw new ArgumentNullException(nameof(searchTask));
            }

            if (handleException == null)
            {
                throw new ArgumentNullException(nameof(handleException));
            }

            _packageFeed = packageFeed;
            _startFromTask = searchTask;
            _handleException = handleException;
            _cancellationToken = cancellationToken;

            Reset();
        }

        private PackageFeedEnumerator(PackageFeedEnumerator other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            _packageFeed = other._packageFeed;
            _startFromTask = other._startFromTask;
            _handleException = other._handleException;
            _cancellationToken = other._cancellationToken;

            Reset();
        }

        public IPackageSearchMetadata Current => _current.Current;

        object IEnumerator.Current => _current.Current;

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _current?.Dispose();
            GC.SuppressFinalize(this);

            _isDisposed = true;
        }

        public bool MoveNext()
        {
            if (_current.MoveNext())
            {
                return true;
            }

            NuGetUIThreadHelper.JoinableTaskFactory.Run(LoadNextAsync);
            return _current.MoveNext();
        }

        private async Task LoadNextAsync()
        {
            var searchResult = await _searchTask;

            while (searchResult.RefreshToken != null)
            {
                searchResult = await _packageFeed.RefreshSearchAsync(searchResult.RefreshToken, _cancellationToken);
            }

            _current = searchResult.GetEnumerator();

            foreach (var pair in searchResult.SourceSearchException)
            {
                _handleException(pair.Key, pair.Value);
            }

            if (searchResult.NextToken != null)
            {
                _searchTask = _packageFeed.ContinueSearchAsync(searchResult.NextToken, _cancellationToken);
            }
            else
            {
                _searchTask = Task.FromResult(SearchResult.Empty<IPackageSearchMetadata>());
            }
        }

        public void Reset()
        {
            _searchTask = _startFromTask;
            _current = Enumerable.Empty<IPackageSearchMetadata>().GetEnumerator();
        }

        /// <summary>
        /// Wrap the provided search result task via a lazy enumerable. The search is continued
        /// until the <see cref="SearchResult{T}.NextToken"/> is <code>null</code>, indicating that
        /// there are no more results.
        /// </summary>
        /// <param name="packageFeed">The package feed to perform the search on.</param>
        /// <param name="searchTask">The initial search result task to operate on.</param>
        /// <param name="handleException">
        /// A callback for handling exceptions during enumeration. The first parameter is the
        /// source that encountered the exception. The second parameter is the exception itself.
        /// This callback is never called from multiple threads at once.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The lazy enumerable of package search metadata.</returns>
        public static IEnumerable<IPackageSearchMetadata> Enumerate(
            IPackageFeed packageFeed,
            Task<SearchResult<IPackageSearchMetadata>> searchTask,
            Action<string, Exception> handleException,
            CancellationToken cancellationToken)
        {
            var enumerator = new PackageFeedEnumerator(packageFeed, searchTask, handleException, cancellationToken);
            return new PackageFeedEnumerable(enumerator);
        }

        private sealed class PackageFeedEnumerable : IEnumerable<IPackageSearchMetadata>
        {
            private readonly PackageFeedEnumerator _enumerator;

            public PackageFeedEnumerable(PackageFeedEnumerator enumerator)
            {
                _enumerator = enumerator;
            }

            public IEnumerator<IPackageSearchMetadata> GetEnumerator() => new PackageFeedEnumerator(_enumerator);

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        }
    }
}
