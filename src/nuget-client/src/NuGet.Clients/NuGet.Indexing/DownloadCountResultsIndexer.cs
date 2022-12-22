// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using NuGet.Protocol.Core.Types;

namespace NuGet.Indexing
{
    /// <summary>
    /// Arranges search results by download count.
    /// </summary>
    public class DownloadCountResultsIndexer : ISearchResultsIndexer
    {
        private const int DefaultRankValue = -1;

        public IEnumerable<IPackageSearchMetadata> ProcessUnrankedEntries(IEnumerable<IPackageSearchMetadata> entries, IDictionary<string, long> ranking)
        {
            foreach (var v in entries.Select(e => e.Identity.Id).Where(id => !ranking.ContainsKey(id)))
            {
                ranking[v] = DefaultRankValue;
            }

            return entries;
        }

        public IDictionary<string, long> Rank(string queryString, IEnumerable<IPackageSearchMetadata> entries)
        {
            return entries
                .Where(e => e.DownloadCount.HasValue)
                .ToDictionary(e => e.Identity.Id, e => e.DownloadCount.Value);
        }
    }
}
