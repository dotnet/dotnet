// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    public class PackageUpdateResourceV2Provider : ResourceProvider
    {
        public PackageUpdateResourceV2Provider()
            : base(
                  typeof(PackageUpdateResource),
                  nameof(PackageUpdateResourceV2Provider),
                  NuGetResourceProviderPositions.Last)
        { }

        public async override Task<Tuple<bool, INuGetResource>> TryCreate(
            SourceRepository source,
            CancellationToken token)
        {
            HttpSource httpSource = null;
            PackageUpdateResource packageUpdateResource = null;
            var sourceUri = source.PackageSource?.Source;
            if (!string.IsNullOrEmpty(sourceUri))
            {
                if (source.PackageSource.IsHttp)
                {
                    var httpSourceResource = await source.GetResourceAsync<HttpSourceResource>(token);
                    httpSource = httpSourceResource.HttpSource;
                }
                packageUpdateResource = new PackageUpdateResource(sourceUri, httpSource);
            }

            var result = new Tuple<bool, INuGetResource>(packageUpdateResource != null, packageUpdateResource);
            return result;
        }
    }
}
