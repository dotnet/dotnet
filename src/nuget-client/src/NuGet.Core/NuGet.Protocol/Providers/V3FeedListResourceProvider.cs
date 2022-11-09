// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    public class V3FeedListResourceProvider : ResourceProvider
    {
        public V3FeedListResourceProvider()
            : base(
                  typeof(ListResource),
                  nameof(V3FeedListResourceProvider),
                  nameof(V2FeedListResourceProvider))
        {
        }

        public override async Task<Tuple<bool, INuGetResource>> TryCreate(
            SourceRepository source,
            CancellationToken token)
        {
            ListResource resource = null;

            var serviceIndex = await source.GetResourceAsync<ServiceIndexResourceV3>(token);

            if (serviceIndex != null)
            {
                var baseUrl = serviceIndex.GetServiceEntryUri(ServiceTypes.LegacyGallery);
                if (baseUrl != null)
                {
                    var httpSource = await source.GetResourceAsync<HttpSourceResource>(token);
                    var serviceDocument =
                        await ODataServiceDocumentUtils.CreateODataServiceDocumentResourceV2(
                            baseUrl.AbsoluteUri, httpSource.HttpSource, DateTime.UtcNow, NullLogger.Instance, token);
                    var parser = new V2FeedParser(httpSource.HttpSource, serviceDocument.BaseAddress, source.PackageSource.Source);
                    var feedCapabilityResource = new LegacyFeedCapabilityResourceV2Feed(parser, serviceDocument.BaseAddress);
                    resource = new V2FeedListResource(parser, feedCapabilityResource, serviceDocument.BaseAddress);
                }
            }

            var result = new Tuple<bool, INuGetResource>(resource != null, resource);
            return result;
        }
    }
}
