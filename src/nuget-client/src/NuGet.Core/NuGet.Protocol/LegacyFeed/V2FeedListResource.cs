// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    public class V2FeedListResource : ListResource
    {
        private readonly ILegacyFeedCapabilityResource _feedCapabilities;
        private readonly IV2FeedParser _feedParser;
        private readonly string _baseAddress;
        private const int Take = 30;

        public V2FeedListResource(IV2FeedParser feedParser, ILegacyFeedCapabilityResource feedCapabilities, string baseAddress)
        {
            _feedParser = feedParser;
            _feedCapabilities = feedCapabilities;
            _baseAddress = baseAddress;
        }

        public override string Source => _baseAddress;

        public async override Task<IEnumerableAsync<IPackageSearchMetadata>> ListAsync(
            string searchTerm,
            bool prerelease,
            bool allVersions,
            bool includeDelisted,
            ILogger logger,
            CancellationToken token)
        {
            var isSearchSupported = await _feedCapabilities.SupportsSearchAsync(logger, token);
            SearchFilter filter = null;
            if (isSearchSupported)
            {
                if (allVersions)
                {
                    filter = new SearchFilter(includePrerelease: prerelease, filter: null)
                    {
                        OrderBy = SearchOrderBy.Id,
                        IncludeDelisted = includeDelisted
                    };
                }
                else
                {
                    var supportsIsAbsoluteLatestVersion =
                        await _feedCapabilities.SupportsIsAbsoluteLatestVersionAsync(logger, token);
                    if (prerelease && supportsIsAbsoluteLatestVersion)
                    {
                        filter = new SearchFilter(includePrerelease: true, filter: SearchFilterType.IsAbsoluteLatestVersion)
                        {
                            OrderBy = SearchOrderBy.Id,
                            IncludeDelisted = includeDelisted
                        };
                    }
                    else
                    {
                        filter = new SearchFilter(includePrerelease: false,
                            filter: SearchFilterType.IsLatestVersion)
                        {
                            OrderBy = SearchOrderBy.Id,
                            IncludeDelisted = includeDelisted
                        };
                    }
                }
            }
            else
            {
                if (allVersions)
                {
                    filter = new SearchFilter(includePrerelease: prerelease, filter: null)
                    {
                        IncludeDelisted = includeDelisted,
                        OrderBy = SearchOrderBy.Id
                    };
                }
                else
                {
                    var supportsIsAbsoluteLatestVersion =
                        await _feedCapabilities.SupportsIsAbsoluteLatestVersionAsync(logger, token);
                    if (prerelease && supportsIsAbsoluteLatestVersion)
                    {
                        filter = new SearchFilter(includePrerelease: true,
                            filter: SearchFilterType.IsAbsoluteLatestVersion)
                        {
                            IncludeDelisted = includeDelisted,
                            OrderBy = SearchOrderBy.Id
                        };
                    }
                    else
                    {
                        filter = new SearchFilter(includePrerelease: false,
                            filter: SearchFilterType.IsLatestVersion)
                        {
                            OrderBy = SearchOrderBy.Id,
                            IncludeDelisted = includeDelisted
                        };
                    }
                }

            }
            return new EnumerableAsync<IPackageSearchMetadata>(_feedParser, searchTerm, filter, 0, Take, isSearchSupported, allVersions,
                        logger, token);
        }
    }
}

internal class EnumerableAsync<T> : IEnumerableAsync<T>
{
    private readonly SearchFilter _filter;
    private readonly ILogger _logger;
    private readonly string _searchTerm;
    private readonly int _skip;
    private readonly int _take;
    private readonly CancellationToken _token;
    private readonly IV2FeedParser _feedParser;
    private readonly bool _isSearchAvailable;
    private readonly bool _allVersions;


    public EnumerableAsync(IV2FeedParser feedParser, string searchTerm, SearchFilter filter, int skip, int take, bool isSearchAvailable, bool allVersions, ILogger logger, CancellationToken token)
    {
        _feedParser = feedParser;
        _searchTerm = searchTerm;
        _filter = filter;
        _skip = skip;
        _take = take;
        _isSearchAvailable = isSearchAvailable;
        _allVersions = allVersions;
        _logger = logger;
        _token = token;
    }

    public IEnumeratorAsync<T> GetEnumeratorAsync()
    {
        return (IEnumeratorAsync<T>)new EnumeratorAsync(_feedParser, _searchTerm, _filter, _skip, _take, _isSearchAvailable, _allVersions, _logger, _token);
    }
}

internal class EnumeratorAsync : IEnumeratorAsync<IPackageSearchMetadata>
{
    private readonly SearchFilter _filter;
    private readonly ILogger _logger;
    private readonly string _searchTerm;
    private int _skip;
    private readonly int _take;
    private readonly CancellationToken _token;
    private readonly IV2FeedParser _feedParser;
    private readonly bool _isSearchAvailable;
    private readonly bool _allVersions;


    private IEnumerator<IPackageSearchMetadata> _currentEnumerator;
    private V2FeedPage _currentPage;

    public EnumeratorAsync(IV2FeedParser feedParser, string searchTerm, SearchFilter filter, int skip, int take, bool isSearchAvailable, bool allVersions,
        ILogger logger, CancellationToken token)
    {
        _feedParser = feedParser;
        _searchTerm = searchTerm;
        _filter = filter;
        _skip = skip;
        _take = take;
        _isSearchAvailable = isSearchAvailable;
        _allVersions = allVersions;
        _logger = logger;
        _token = token;
    }

    public IPackageSearchMetadata Current
    {
        get
        {
            return _currentEnumerator?.Current;
        }
    }

    public async Task<bool> MoveNextAsync()
    {
        var metadataCache = new MetadataReferenceCache();

        if (_currentPage == null)
        {


            _currentPage = _isSearchAvailable
                ? await _feedParser.GetSearchPageAsync(_searchTerm, _filter, _skip, _take, _logger, _token)
                : await _feedParser.GetPackagesPageAsync(_searchTerm, _filter, _skip, _take, _logger, _token);


            var results = _allVersions ?
                _currentPage.Items.GroupBy(p => p.Id)
                 .Select(group => group.OrderByDescending(p => p.Version)).SelectMany(pg => pg)
                 .Select(
                     package =>
                         V2FeedUtilities.CreatePackageSearchResult(package, metadataCache, _filter,
                             (V2FeedParser)_feedParser, _logger, _token)).Where(p => _filter.IncludeDelisted || p.IsListed)
            :
            _currentPage.Items.GroupBy(p => p.Id)
                 .Select(group => group.OrderByDescending(p => p.Version).First())
                 .Select(
                     package =>
                         V2FeedUtilities.CreatePackageSearchResult(package, metadataCache, _filter,
                             (V2FeedParser)_feedParser, _logger, _token)).Where(p => _filter.IncludeDelisted || p.IsListed);


            var enumerator = results.GetEnumerator();
            _currentEnumerator = enumerator;
            return _currentEnumerator.MoveNext();
        }
        else
        {
            if (!_currentEnumerator.MoveNext())
            {
                if (_currentPage.Items.Count != _take) // Last page not filled completely, no more pages left
                {
                    return false;
                }
                _skip += _take;
                _currentPage = _isSearchAvailable
                                    ? await _feedParser.GetSearchPageAsync(_searchTerm, _filter, _skip, _take, _logger, _token)
                                    : await _feedParser.GetPackagesPageAsync(_searchTerm, _filter, _skip, _take, _logger, _token);

                var results = _allVersions ?
               _currentPage.Items.GroupBy(p => p.Id)
                .Select(group => group.OrderByDescending(p => p.Version)).SelectMany(pg => pg)
                .Select(
                    package =>
                        V2FeedUtilities.CreatePackageSearchResult(package, metadataCache, _filter,
                            (V2FeedParser)_feedParser, _logger, _token)).Where(p => _filter.IncludeDelisted || p.IsListed)
                :
                _currentPage.Items.GroupBy(p => p.Id)
                 .Select(group => group.OrderByDescending(p => p.Version).First())
                 .Select(
                     package =>
                         V2FeedUtilities.CreatePackageSearchResult(package, metadataCache, _filter,
                             (V2FeedParser)_feedParser, _logger, _token)).Where(p => _filter.IncludeDelisted || p.IsListed);

                var enumerator = results.GetEnumerator();
                _currentEnumerator = enumerator;
                return _currentEnumerator.MoveNext();
            }
            else
            {
                return true;
            }

        }
    }
}
