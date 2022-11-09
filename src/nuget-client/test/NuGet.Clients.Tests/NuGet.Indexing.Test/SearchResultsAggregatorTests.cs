// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Indexing.Test
{
    public class PackageSearchResponse
    {
        public int totalHits;
        public string index;
        public PackageSearchMetadata[] data;
    }

    public class SearchResultsAggregatorTests
    {
        private const string NuGetCore = "NuGet.Core";

        [Fact]
        public async Task AggregateAsync_MergesVersions()
        {
            var indexer = new RelevanceSearchResultsIndexer();
            var aggregator = new SearchResultsAggregator(indexer, new PackageSearchMetadataSplicer());

            var queryString = "nuget";

            var rawSearch1 = TestUtility.LoadTestResponse("mergeVersions1.json");
            var package1 = FindNuGetCorePackage(rawSearch1);
            var v1 = await GetPackageVersionsAsync(package1);
            Assert.NotEmpty(v1);

            var rawSearch2 = TestUtility.LoadTestResponse("mergeVersions2.json");
            var package2 = FindNuGetCorePackage(rawSearch2);
            var v2 = await GetPackageVersionsAsync(package2);
            Assert.NotEmpty(v2);

            var results = await aggregator.AggregateAsync(queryString, rawSearch1, rawSearch2);

            var mergedPackage = FindNuGetCorePackage(results);
            var vm = await GetPackageVersionsAsync(mergedPackage);
            Assert.Superset(v1, vm);
            Assert.Superset(v2, vm);
        }

        [Fact]
        public async Task AggregateAsync_MaintainsOrder()
        {
            var indexer = new DownloadCountResultsIndexer();
            var aggregator = new SearchResultsAggregator(indexer, new PackageSearchMetadataSplicer());

            var queryString = "nuget";

            var rawSearch1 = TestUtility.LoadTestResponse("relativeOrder1.json");
            var rawSearch2 = TestUtility.LoadTestResponse("relativeOrder2.json");
            var rawSearch3 = TestUtility.LoadTestResponse("relativeOrder3.json");

            var results = await aggregator.AggregateAsync(queryString, rawSearch1, rawSearch2, rawSearch3);

            AssertRelativeOrder(rawSearch1, results);
            AssertRelativeOrder(rawSearch2, results);
            AssertRelativeOrder(rawSearch3, results);
        }

        [Fact]
        public async Task AggregateAsync_IdenticalFeeds()
        {
            var indexer = new DownloadCountResultsIndexer();
            var aggregator = new SearchResultsAggregator(indexer, new PackageSearchMetadataSplicer());

            var queryString = "nuget";

            var rawSearch1 = TestUtility.LoadTestResponse("relativeOrder1.json");
            var rawSearch2 = TestUtility.LoadTestResponse("relativeOrder1.json");

            var results = await aggregator.AggregateAsync(queryString, rawSearch1, rawSearch2);

            Assert.Equal(rawSearch1.Select(r => r.Identity), results.Select(r => r.Identity));
        }

        private static void AssertRelativeOrder(IEnumerable<PackageSearchMetadata> rawSearchResults, IEnumerable<IPackageSearchMetadata> mergedSearchResults)
        {
            var packageIdsOrderedInInitialOrder = rawSearchResults
                .Select(r => r.Identity.Id)
                .ToArray();
            Assert.Equal(
                expected: packageIdsOrderedInInitialOrder,
                actual: mergedSearchResults.Select(r => r.Identity.Id).Where(id => packageIdsOrderedInInitialOrder.Contains(id)));
        }

        private static async Task<ISet<NuGetVersion>> GetPackageVersionsAsync(IPackageSearchMetadata package)
        {
            return new HashSet<NuGetVersion>((await package.GetVersionsAsync()).Select(v => v.Version));
        }

        private static IPackageSearchMetadata FindNuGetCorePackage(IEnumerable<IPackageSearchMetadata> searchResults)
        {
            return searchResults
                .First(p => string.Equals(p.Identity.Id, NuGetCore, StringComparison.OrdinalIgnoreCase));
        }
    }
}
