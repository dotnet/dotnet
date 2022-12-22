// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;

namespace NuGet.Indexing
{
    /// <summary>
    /// Combined analyzers of package metadata.
    /// </summary>
    public class PackageAnalyzer : PerFieldAnalyzerWrapper
    {
        static readonly IDictionary<string, Analyzer> _fieldAnalyzers;

        static PackageAnalyzer()
        {
            _fieldAnalyzers = new Dictionary<string, Analyzer>(StringComparer.OrdinalIgnoreCase)
            {
                { "Id", new IdentifierKeywordAnalyzer() },
                { "TokenizedId", new IdentifierAnalyzer() },
                { "Version", new VersionAnalyzer() },
                { "Title", new DescriptionAnalyzer() },
                { "Description", new DescriptionAnalyzer() },
                { "Summary", new DescriptionAnalyzer() },
                { "Authors", new DescriptionAnalyzer() },
                { "Owner", new OwnerAnalyzer() },
                { "Tags", new TagsAnalyzer() }
            };
        }

        public PackageAnalyzer()
            : base(new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30), _fieldAnalyzers)
        {
        }
    }
}
