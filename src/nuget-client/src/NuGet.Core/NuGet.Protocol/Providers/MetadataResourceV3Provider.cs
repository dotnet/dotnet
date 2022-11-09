// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    public class MetadataResourceV3Provider : ResourceProvider
    {
        public MetadataResourceV3Provider()
            : base(typeof(MetadataResource),
                  nameof(MetadataResourceV3Provider),
                  "MetadataResourceV2FeedProvider")
        {
        }

        public override async Task<Tuple<bool, INuGetResource>> TryCreate(SourceRepository source, CancellationToken token)
        {
            MetadataResourceV3 curResource = null;
            var regResource = await source.GetResourceAsync<RegistrationResourceV3>(token);

            if (regResource != null)
            {
                curResource = new MetadataResourceV3(regResource);
            }

            return new Tuple<bool, INuGetResource>(curResource != null, curResource);
        }
    }
}
