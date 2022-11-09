// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NuGet.Indexing.Test
{
    public class RelevanceSearchResultsIndexerTests
    {
        [Fact]
        public void ProcessUnrankedEntries_FillsWithDefaultRank()
        {
            var entries = TestUtility.LoadTestResponse("unrankedEntries.json");
            var ranking = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase)
            {
                ["Package1"] = 9,
                ["Package2"] = 2,
                ["Package5"] = 3
            };
            var expected = new Dictionary<string, long>
            {
                ["Package1"] = 9,
                ["Package2"] = 2,
                ["Package3"] = 3,
                ["Package4"] = 3,
                ["Package5"] = 3,
                ["Package6"] = -1,
                ["Package7"] = -1,
                ["Package8"] = -1,
                ["Package9"] = -1
            };

            var indexer = new RelevanceSearchResultsIndexer();

            var processed = indexer.ProcessUnrankedEntries(entries, ranking);

            Assert.Equal(expected.OrderBy(x => x.Key), ranking.OrderBy(y => y.Key));
        }
    }
}
