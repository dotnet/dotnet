// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    public class LocalAutoCompleteResourceProvider : ResourceProvider
    {
        public LocalAutoCompleteResourceProvider()
            : base(typeof(AutoCompleteResource), nameof(LocalAutoCompleteResourceProvider), NuGetResourceProviderPositions.Last)
        {
        }

        public override async Task<Tuple<bool, INuGetResource>> TryCreate(SourceRepository source, CancellationToken token)
        {
            INuGetResource resource = null;

            var localResource = await source.GetResourceAsync<FindLocalPackagesResource>(token);

            if (localResource != null)
            {
                resource = new LocalAutoCompleteResource(localResource);
            }

            return new Tuple<bool, INuGetResource>(resource != null, resource);
        }
    }
}
