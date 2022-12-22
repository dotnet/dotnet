// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    public class AutoCompleteResourceV2FeedProvider : ResourceProvider
    {
        public AutoCompleteResourceV2FeedProvider()
            : base(
                  typeof(AutoCompleteResource),
                  nameof(AutoCompleteResourceV2FeedProvider),
                  "AutoCompleteResourceLocalProvider")
        {
        }

        public override async Task<Tuple<bool, INuGetResource>> TryCreate(SourceRepository source, CancellationToken token)
        {
            AutoCompleteResourceV2Feed resource = null;

            if (await source.GetFeedType(token) == FeedType.HttpV2)
            {
                var serviceDocument = await source.GetResourceAsync<ODataServiceDocumentResourceV2>(token);

                var httpSource = await source.GetResourceAsync<HttpSourceResource>(token);

                resource = new AutoCompleteResourceV2Feed(httpSource, serviceDocument.BaseAddress, source.PackageSource);
            }

            return new Tuple<bool, INuGetResource>(resource != null, resource);
        }
    }
}
