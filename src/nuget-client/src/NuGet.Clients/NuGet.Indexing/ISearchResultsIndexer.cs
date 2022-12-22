// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Protocol.Core.Types;

namespace NuGet.Indexing
{
    /// <summary>
    /// A contract of package metadata ranking provider
    /// </summary>
    public interface ISearchResultsIndexer
    {
        /// <summary>
        /// Associates relevance rank with every element in search results collection
        /// </summary>
        /// <param name="queryString">Relevance ranking criteria</param>
        /// <param name="entries">Search results</param>
        /// <returns>Dictionary of package to rank associations</returns>
        IDictionary<string, long> Rank(string queryString, IEnumerable<IPackageSearchMetadata> entries);

        /// <summary>
        /// Represents a strategy of dealing with unranked elements. Specific to an indexer.
        /// </summary>
        /// <param name="entries">Subset of search results sequence. Generally, elements from a single feed.</param>
        /// <param name="ranking">Ranking as computed in <see cref="Rank(string, IEnumerable{IPackageSearchMetadata})"/></param>
        /// <returns>Altered sequence of search results.</returns>
        IEnumerable<IPackageSearchMetadata> ProcessUnrankedEntries(IEnumerable<IPackageSearchMetadata> entries, IDictionary<string, long> ranking);
    }
}
