// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    [Obsolete("Use PackageSearchResource instead (via SourceRepository.GetResourceAsync<PackageSearchResource>")]
    public class RawSearchResourceV3Provider : ResourceProvider
    {
        public RawSearchResourceV3Provider()
            : base(typeof(RawSearchResourceV3),
                  nameof(RawSearchResourceV3),
                  NuGetResourceProviderPositions.Last)
        {
        }

        public override async Task<Tuple<bool, INuGetResource>> TryCreate(SourceRepository source, CancellationToken token)
        {
            RawSearchResourceV3 curResource = null;
            var serviceIndex = await source.GetResourceAsync<ServiceIndexResourceV3>(token);

            if (serviceIndex != null)
            {
                var endpoints = serviceIndex.GetServiceEntryUris(ServiceTypes.SearchQueryService);

                if (endpoints.Count > 0)
                {
                    var httpSourceResource = await source.GetResourceAsync<HttpSourceResource>(token);

                    // construct a new resource
                    curResource = new RawSearchResourceV3(httpSourceResource.HttpSource, endpoints);
                }
            }

            return new Tuple<bool, INuGetResource>(curResource != null, curResource);
        }
    }
}
