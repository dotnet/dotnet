// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    public class DependencyInfoResourceV2FeedProvider : ResourceProvider
    {
        public DependencyInfoResourceV2FeedProvider()
            : base(typeof(DependencyInfoResource), "DependencyInfoResourceV2FeedProvider", "DependencyInfoResourceV2Provider")
        {
        }

        public override async Task<Tuple<bool, INuGetResource>> TryCreate(SourceRepository source, CancellationToken token)
        {
            DependencyInfoResource resource = null;

            if (await source.GetFeedType(token) == FeedType.HttpV2)
            {
                var serviceDocument = await source.GetResourceAsync<ODataServiceDocumentResourceV2>(token);

                var httpSource = await source.GetResourceAsync<HttpSourceResource>(token);
                var parser = new V2FeedParser(httpSource.HttpSource, serviceDocument.BaseAddress, source.PackageSource.Source);

                resource = new DependencyInfoResourceV2Feed(parser, source);
            }

            return new Tuple<bool, INuGetResource>(resource != null, resource);
        }
    }
}
