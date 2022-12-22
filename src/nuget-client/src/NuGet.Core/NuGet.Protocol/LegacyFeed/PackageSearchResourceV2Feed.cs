// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    public class PackageSearchResourceV2Feed : PackageSearchResource
    {
        private readonly HttpSource _httpSource;
        private readonly Configuration.PackageSource _packageSource;
        private readonly V2FeedParser _feedParser;

        public PackageSearchResourceV2Feed(HttpSourceResource httpSourceResource, string baseAddress, Configuration.PackageSource packageSource)
        {
            if (httpSourceResource == null)
            {
                throw new ArgumentNullException(nameof(httpSourceResource));
            }

            if (packageSource == null)
            {
                throw new ArgumentNullException(nameof(packageSource));
            }

            _httpSource = httpSourceResource.HttpSource;
            _packageSource = packageSource;
            _feedParser = new V2FeedParser(_httpSource, baseAddress, packageSource.Source);
        }

        public override async Task<IEnumerable<IPackageSearchMetadata>> SearchAsync(
            string searchTerm,
            SearchFilter filters,
            int skip,
            int take,
            Common.ILogger log,
            CancellationToken cancellationToken)
        {
            var query = await _feedParser.Search(
                searchTerm,
                filters,
                skip,
                take,
                log,
                cancellationToken);

            var metadataCache = new MetadataReferenceCache();
            // NuGet.Server does not group packages by id, this resource needs to handle it.
            var results = query.GroupBy(p => p.Id)
                .Select(group => group.OrderByDescending(p => p.Version).First())
                .Select(package => V2FeedUtilities.CreatePackageSearchResult(package, metadataCache, filters, _feedParser, log, cancellationToken));

            return results.ToList();
        }
    }
}
