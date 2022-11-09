// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    public class ReportAbuseResourceV3Provider : ResourceProvider
    {
        public ReportAbuseResourceV3Provider()
            : base(typeof(ReportAbuseResourceV3),
                  nameof(ReportAbuseResourceV3),
                  NuGetResourceProviderPositions.Last)
        {
        }

        public override async Task<Tuple<bool, INuGetResource>> TryCreate(SourceRepository source, CancellationToken token)
        {
            ReportAbuseResourceV3 resource = null;
            var serviceIndex = await source.GetResourceAsync<ServiceIndexResourceV3>(token);
            if (serviceIndex != null)
            {
                var uriTemplate = serviceIndex.GetServiceEntryUri(ServiceTypes.ReportAbuse)?.AbsoluteUri;

                // construct a new resource
                resource = new ReportAbuseResourceV3(uriTemplate);
            }

            return new Tuple<bool, INuGetResource>(resource != null, resource);
        }
    }
}
