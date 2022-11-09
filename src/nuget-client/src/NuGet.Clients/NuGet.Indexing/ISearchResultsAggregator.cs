// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using NuGet.Protocol.Core.Types;

namespace NuGet.Indexing
{
    /// <summary>
    /// Provides a method of aggregating search results
    /// </summary>
    public interface ISearchResultsAggregator
    {
        /// <summary>
        /// Merges collections of package metadata using search string for re-ranking items while ordering result list by relevance.
        /// </summary>
        /// <param name="queryString">Relevance ordering criteria (plain search text)</param>
        /// <param name="results">Collections of search results</param>
        /// <returns>Aggregated collection of search results</returns>
        Task<IEnumerable<IPackageSearchMetadata>> AggregateAsync(string queryString, params IEnumerable<IPackageSearchMetadata>[] results);
    }
}
