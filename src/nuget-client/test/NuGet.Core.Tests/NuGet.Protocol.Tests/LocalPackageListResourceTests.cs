// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Protocol.Core.Types;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class LocalPackageListResourceTests
    {

        [Fact]
        public async Task TestTTT()
        {
            var searchTerm = "bla";
            var prerelease = true;
            var allVersions = true;
            var includeDelisted = true;
            var mock = new PackageSearchResourceMock() { _searchTerm = searchTerm, _prerelease = prerelease, _allVersions = allVersions, _includeDelisted = includeDelisted };
            var resource = new LocalPackageListResource(mock, "");
            var enumerable = await resource.ListAsync(searchTerm, prerelease, allVersions, includeDelisted, NullLogger.Instance, CancellationToken.None);
            var enumerator = enumerable.GetEnumeratorAsync();
            await enumerator.MoveNextAsync();
            AssertAll(mock);
        }

        [Fact]
        public async Task TestTTF()
        {
            var searchTerm = "bla";
            var prerelease = true;
            var allVersions = true;
            var includeDelisted = false;
            var mock = new PackageSearchResourceMock() { _searchTerm = searchTerm, _prerelease = prerelease, _allVersions = allVersions, _includeDelisted = includeDelisted };
            var resource = new LocalPackageListResource(mock, "");
            var enumerable = await resource.ListAsync(searchTerm, prerelease, allVersions, includeDelisted, NullLogger.Instance, CancellationToken.None);
            var enumerator = enumerable.GetEnumeratorAsync();
            await enumerator.MoveNextAsync();
            AssertAll(mock);
        }

        [Fact]
        public async Task TestTFT()
        {
            var searchTerm = "bla";
            var prerelease = true;
            var allVersions = false;
            var includeDelisted = true;
            var mock = new PackageSearchResourceMock() { _searchTerm = searchTerm, _prerelease = prerelease, _allVersions = allVersions, _includeDelisted = includeDelisted };
            var resource = new LocalPackageListResource(mock, "");
            var enumerable = await resource.ListAsync(searchTerm, prerelease, allVersions, includeDelisted, NullLogger.Instance, CancellationToken.None);
            var enumerator = enumerable.GetEnumeratorAsync();
            await enumerator.MoveNextAsync();
            AssertAll(mock);
        }

        [Fact]
        public async Task TestTFF()
        {
            var searchTerm = "bla";
            var prerelease = true;
            var allVersions = false;
            var includeDelisted = false;
            var mock = new PackageSearchResourceMock() { _searchTerm = searchTerm, _prerelease = prerelease, _allVersions = allVersions, _includeDelisted = includeDelisted };
            var resource = new LocalPackageListResource(mock, "");
            var enumerable = await resource.ListAsync(searchTerm, prerelease, allVersions, includeDelisted, NullLogger.Instance, CancellationToken.None);
            var enumerator = enumerable.GetEnumeratorAsync();
            await enumerator.MoveNextAsync();
            AssertAll(mock);
        }

        [Fact]
        public async Task TestFTT()
        {
            var searchTerm = "bla";
            var prerelease = false;
            var allVersions = true;
            var includeDelisted = true;
            var mock = new PackageSearchResourceMock() { _searchTerm = searchTerm, _prerelease = prerelease, _allVersions = allVersions, _includeDelisted = includeDelisted };
            var resource = new LocalPackageListResource(mock, "");
            var enumerable = await resource.ListAsync(searchTerm, prerelease, allVersions, includeDelisted, NullLogger.Instance, CancellationToken.None);
            var enumerator = enumerable.GetEnumeratorAsync();
            await enumerator.MoveNextAsync();
            AssertAll(mock);
        }

        [Fact]
        public async Task TestFTF()
        {
            var searchTerm = "bla";
            var prerelease = false;
            var allVersions = true;
            var includeDelisted = false;
            var mock = new PackageSearchResourceMock() { _searchTerm = searchTerm, _prerelease = prerelease, _allVersions = allVersions, _includeDelisted = includeDelisted };
            var resource = new LocalPackageListResource(mock, "");
            var enumerable = await resource.ListAsync(searchTerm, prerelease, allVersions, includeDelisted, NullLogger.Instance, CancellationToken.None);
            var enumerator = enumerable.GetEnumeratorAsync();
            await enumerator.MoveNextAsync();
            AssertAll(mock);
        }

        [Fact]
        public async Task TestFFT()
        {
            var searchTerm = "bla";
            var prerelease = false;
            var allVersions = false;
            var includeDelisted = true;
            var mock = new PackageSearchResourceMock() { _searchTerm = searchTerm, _prerelease = prerelease, _allVersions = allVersions, _includeDelisted = includeDelisted };
            var resource = new LocalPackageListResource(mock, "");
            var enumerable = await resource.ListAsync(searchTerm, prerelease, allVersions, includeDelisted, NullLogger.Instance, CancellationToken.None);
            var enumerator = enumerable.GetEnumeratorAsync();
            await enumerator.MoveNextAsync();
            AssertAll(mock);
        }

        [Fact]
        public async Task TestFFF()
        {
            var searchTerm = "bla";
            var prerelease = false;
            var allVersions = false;
            var includeDelisted = false;
            var mock = new PackageSearchResourceMock() { _searchTerm = searchTerm, _prerelease = prerelease, _allVersions = allVersions, _includeDelisted = includeDelisted };
            var resource = new LocalPackageListResource(mock, "");
            var enumerable = await resource.ListAsync(searchTerm, prerelease, allVersions, includeDelisted, NullLogger.Instance, CancellationToken.None);
            var enumerator = enumerable.GetEnumeratorAsync();
            await enumerator.MoveNextAsync();
            AssertAll(mock);
        }


        private void AssertAll(PackageSearchResourceMock mock)
        {
            Assert.Equal(mock._searchTerm, mock._actualSearchTerm);
            Assert.True(0 == mock._actualSkip);
            Assert.True(int.MaxValue == mock._actualTake);
            Assert.True(mock._searchFilter.OrderBy == SearchOrderBy.Id);
            if (mock._allVersions)
            {
                Assert.True(mock._searchFilter.Filter == null);
            }
            if (!mock._allVersions && mock._prerelease)
            {
                Assert.True(mock._searchFilter.Filter == SearchFilterType.IsAbsoluteLatestVersion);
            }

            if (!mock._allVersions && !mock._prerelease)
            {
                Assert.True(mock._searchFilter.Filter == SearchFilterType.IsLatestVersion);
            }

            Assert.True(mock._searchFilter.IncludeDelisted == mock._includeDelisted);
            Assert.True(mock._searchFilter.IncludePrerelease == mock._prerelease);

        }

        private class PackageSearchResourceMock : PackageSearchResource
        {
            public string _searchTerm { get; set; }
            public bool _prerelease { get; set; }
            public bool _includeDelisted { get; set; }
            public bool _allVersions { get; set; }

            public string _actualSearchTerm { get; set; }
            public SearchFilter _searchFilter { get; set; }
            public int _actualSkip { get; set; }
            public int _actualTake { get; set; }

            public override Task<IEnumerable<IPackageSearchMetadata>> SearchAsync(string searchTerm, SearchFilter filters, int skip, int take, ILogger log, CancellationToken cancellationToken)
            {
                _actualSearchTerm = searchTerm;
                _searchFilter = filters;
                _actualSkip = skip;
                _actualTake = take;
                return Task.FromResult(new List<IPackageSearchMetadata>().AsEnumerable());
            }
        }
    }


}
