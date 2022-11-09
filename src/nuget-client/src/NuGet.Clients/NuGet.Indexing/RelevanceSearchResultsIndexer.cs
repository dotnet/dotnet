// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using NuGet.Protocol.Core.Types;

namespace NuGet.Indexing
{
    /// <summary>
    /// Lucene-based search results indexer.
    /// </summary>
    public class RelevanceSearchResultsIndexer : ISearchResultsIndexer
    {
        // Default rank value will cause element to sink down to bottom of search results list
        private const long DefaultRankValue = -1;

        public IDictionary<string, long> Rank(string queryString, IEnumerable<IPackageSearchMetadata> entries)
        {
            using var directory = new RAMDirectory();

            AddToIndex(directory, entries);

            using var searcher = new IndexSearcher(directory);

            var query = NuGetQuery.MakeQuery(queryString);
            var topDocs = searcher.Search(query, entries.Count());

            var ranking = topDocs.ScoreDocs
                .Select(d => searcher.Doc(d.Doc))
                .Zip(Enumerable.Range(0, topDocs.ScoreDocs.Length).Reverse(), (doc, rank) => new { doc, rank })
                .ToDictionary(x => x.doc.Get("Id"), x => (long)x.rank);

            return ranking;
        }

        private static void AddToIndex(Directory directory, IEnumerable<IPackageSearchMetadata> entries)
        {
            using var packageAnalyzer = new PackageAnalyzer();
            using var writer = new IndexWriter(directory, packageAnalyzer, IndexWriter.MaxFieldLength.UNLIMITED);
            foreach (var document in entries.Select(CreateDocument))
            {
                writer.AddDocument(document);
            }
            writer.Commit();
        }

        private static Document CreateDocument(IPackageSearchMetadata item)
        {
            var doc = new Document();
            doc.Add(new Field("Id", item.Identity?.Id ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("Version", item.Identity?.Version?.ToString() ?? string.Empty, Field.Store.NO, Field.Index.ANALYZED));
            doc.Add(new Field("Summary", item.Summary ?? string.Empty, Field.Store.NO, Field.Index.ANALYZED));
            doc.Add(new Field("Description", item.Description ?? string.Empty, Field.Store.NO, Field.Index.ANALYZED));
            doc.Add(new Field("Title", item.Title ?? string.Empty, Field.Store.NO, Field.Index.ANALYZED));
            doc.Add(new Field("Tags", item.Tags ?? string.Empty, Field.Store.NO, Field.Index.ANALYZED));
            return doc;
        }

        // Fills gaps of missing ranks in a given sequence of search results.
        // Helps to keep unranked elements in merged list.
        // To illustrate the effect of this method consider sample sequence with ranks as following:
        // [ 9, 2, -, -, 3, -, -, - ] => [ 9, 2, *3*, *3*, 3, *-1*, *-1*, *-1* ]
        public IEnumerable<IPackageSearchMetadata> ProcessUnrankedEntries(IEnumerable<IPackageSearchMetadata> entries, IDictionary<string, long> ranking)
        {
            var defaultRank = DefaultRankValue;
            foreach (var v in entries.Reverse().Select(e => e.Identity.Id))
            {
                if (!ranking.ContainsKey(v))
                {
                    ranking.Add(v, defaultRank);
                }

                // assign rank of element behind current
                defaultRank = ranking[v];
            }

            // returns unmodified list for convenience
            return entries;
        }
    }
}
