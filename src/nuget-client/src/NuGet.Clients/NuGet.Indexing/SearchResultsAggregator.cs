// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Protocol.Core.Types;

namespace NuGet.Indexing
{
    /// <summary>
    /// Aggregator preserving relative order of search results in original collection.
    /// </summary>
    public class SearchResultsAggregator : ISearchResultsAggregator
    {
        private readonly ISearchResultsIndexer _indexer;
        private readonly IPackageSearchMetadataSplicer _splicer;

        public SearchResultsAggregator(ISearchResultsIndexer indexer, IPackageSearchMetadataSplicer splicer)
        {
            if (indexer == null)
            {
                throw new ArgumentNullException(nameof(indexer));
            }

            _indexer = indexer;

            if (splicer == null)
            {
                throw new ArgumentNullException(nameof(splicer));
            }

            _splicer = splicer;
        }

        public Task<IEnumerable<IPackageSearchMetadata>> AggregateAsync(string queryString, params IEnumerable<IPackageSearchMetadata>[] inputResults)
        {
            var mergedIndex = new MergedIndex(_splicer);
            foreach (var inputResult in inputResults)
            {
                mergedIndex.MergeResults(inputResult);
            }

            var ranking = _indexer.Rank(queryString, mergedIndex.Entries);

            var inputQueues = inputResults
                .Select(result => new Queue<string>(
                    _indexer.ProcessUnrankedEntries(result, ranking).Select(entry => entry.Identity.Id)))
                .ToArray();

            var enqueued = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var outputQueue = new Queue<IPackageSearchMetadata>(mergedIndex.Entries.Count());

            var queues = inputQueues.Where(q => q.Count > 0);
            while (queues.Count() > 1)
            {
                var candidates = queues.Select(q => q.Peek()).ToArray();

                var winnerRank = candidates.Max(x => ranking[x]);
                var winners = candidates.Where(x => ranking[x] == winnerRank);
                foreach (var winner in winners.Where(w => !enqueued.Contains(w)))
                {
                    enqueued.Add(winner);
                    outputQueue.Enqueue(mergedIndex[winner]);
                }

                foreach (var queue in queues)
                {
                    // remove elements with no rank and already enqueued ones
                    while ((queue.Count > 0) && (enqueued.Contains(queue.Peek()) || !ranking.ContainsKey(queue.Peek())))
                    {
                        queue.Dequeue();
                    }
                }
            }

            // append tail elements from the last queue (should be one or none)
            // in most cases input queues are unbalanced, i.e. contain different amount of search results.
            // in that scenario merging algorithm ends leaving a "tail" in a longer queue.
            // although in certain rare edge cases it might empty all queues at once.
            if (queues.Any())
            {
                foreach (var entry in queues.Single().Where(e => !enqueued.Contains(e)))
                {
                    // don't have to update enqueued as this is the last queue
                    outputQueue.Enqueue(mergedIndex[entry]);
                }
            }

            return Task.FromResult<IEnumerable<IPackageSearchMetadata>>(outputQueue);
        }

        private class MergedIndex
        {
            private readonly IDictionary<string, IPackageSearchMetadata> _index = new Dictionary<string, IPackageSearchMetadata>(StringComparer.OrdinalIgnoreCase);
            private readonly IPackageSearchMetadataSplicer _splicer;

            public IEnumerable<IPackageSearchMetadata> Entries => _index.Values;

            public IPackageSearchMetadata this[string key] => _index[key];

            public MergedIndex(IPackageSearchMetadataSplicer splicer)
            {
                if (splicer == null)
                {
                    throw new ArgumentNullException(nameof(splicer));
                }

                _splicer = splicer;
            }

            public void MergeResults(IEnumerable<IPackageSearchMetadata> result)
            {
                foreach (var entry in result)
                {
                    IPackageSearchMetadata value;
                    if (_index.TryGetValue(entry.Identity.Id, out value))
                    {
                        _index[entry.Identity.Id] = _splicer.MergeEntries(value, entry);
                    }
                    else
                    {
                        _index.Add(entry.Identity.Id, entry);
                    }
                }
            }
        }
    }
}
