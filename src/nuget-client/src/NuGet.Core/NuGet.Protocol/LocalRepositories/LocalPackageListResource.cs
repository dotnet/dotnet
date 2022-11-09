// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    public class LocalPackageListResource : ListResource
    {
        private readonly PackageSearchResource _localPackageSearchResource;
        private readonly string _baseAddress;
        public LocalPackageListResource(PackageSearchResource localPackageSearchResource, string baseAddress)
        {
            _localPackageSearchResource = localPackageSearchResource;
            _baseAddress = baseAddress;
        }
        public override string Source => _baseAddress;

        public override Task<IEnumerableAsync<IPackageSearchMetadata>> ListAsync(string searchTerm, bool prerelease, bool allVersions, bool includeDelisted, ILogger logger,
            CancellationToken token)
        {
            SearchFilter filter;

            if (allVersions)
            {
                filter = new SearchFilter(includePrerelease: prerelease, filter: null)
                {
                    OrderBy = SearchOrderBy.Id,
                    IncludeDelisted = includeDelisted
                };
            }
            else if (prerelease)
            {
                filter = new SearchFilter(includePrerelease: true, filter: SearchFilterType.IsAbsoluteLatestVersion)
                {
                    OrderBy = SearchOrderBy.Id,
                    IncludeDelisted = includeDelisted
                };
            }
            else
            {
                filter = new SearchFilter(includePrerelease: false, filter: SearchFilterType.IsLatestVersion)
                {
                    OrderBy = SearchOrderBy.Id,
                    IncludeDelisted = includeDelisted
                };
            }
            IEnumerableAsync<IPackageSearchMetadata> enumerable = new EnumerableAsync<IPackageSearchMetadata>(_localPackageSearchResource, searchTerm, filter,
                logger, token);
            return Task.FromResult(enumerable);

        }

        internal class EnumerableAsync<T> : IEnumerableAsync<T>
        {
            private readonly SearchFilter _filter;
            private readonly ILogger _logger;
            private readonly string _searchTerm;
            private readonly CancellationToken _token;
            private readonly PackageSearchResource _packageSearchResource;


            public EnumerableAsync(PackageSearchResource feedParser, string searchTerm, SearchFilter filter, ILogger logger, CancellationToken token)
            {
                _packageSearchResource = feedParser;
                _searchTerm = searchTerm;
                _filter = filter;
                _logger = logger;
                _token = token;
            }

            public IEnumeratorAsync<T> GetEnumeratorAsync()
            {
                return (IEnumeratorAsync<T>)new EnumeratorAsync(_packageSearchResource, _searchTerm, _filter, _logger, _token);
            }
        }

        internal class EnumeratorAsync : IEnumeratorAsync<IPackageSearchMetadata>
        {
            private readonly SearchFilter _filter;
            private readonly ILogger _logger;
            private readonly string _searchTerm;
            private readonly CancellationToken _token;
            private readonly PackageSearchResource _packageSearchResource;


            private IEnumerator<IPackageSearchMetadata> _currentEnumerator;

            public EnumeratorAsync(PackageSearchResource feedParser, string searchTerm, SearchFilter filter, ILogger logger, CancellationToken token)
            {
                _packageSearchResource = feedParser;
                _searchTerm = searchTerm;
                _filter = filter;
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
                if (_currentEnumerator == null)
                { // NOTE: We need to sort the values so this is very innefficient by design. 
                  // The FS search resource would return the results ordered in FS nat ordering.
                    var results = await _packageSearchResource.SearchAsync(
                        _searchTerm, _filter, 0, int.MaxValue, _logger, _token);
                    switch (_filter.OrderBy)
                    {
                        case SearchOrderBy.Id:
                            _currentEnumerator = results.OrderBy(p => p.Identity).GetEnumerator();
                            break;
                        default:
                            _currentEnumerator = results.GetEnumerator();
                            break;
                    }
                }
                return _currentEnumerator.MoveNext();
            }
        }
    }
}
