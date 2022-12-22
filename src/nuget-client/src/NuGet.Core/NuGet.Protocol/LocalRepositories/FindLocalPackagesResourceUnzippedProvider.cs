// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    public class FindLocalPackagesResourceUnzippedProvider : ResourceProvider
    {
        // Cache unzipped resources across the repository
        private readonly ConcurrentDictionary<PackageSource, FindLocalPackagesResourceUnzipped> _cache =
            new ConcurrentDictionary<PackageSource, FindLocalPackagesResourceUnzipped>();

        public FindLocalPackagesResourceUnzippedProvider()
            : base(typeof(FindLocalPackagesResource), nameof(FindLocalPackagesResourceUnzippedProvider), NuGetResourceProviderPositions.Last)
        {
        }

        public override async Task<Tuple<bool, INuGetResource>> TryCreate(SourceRepository source, CancellationToken token)
        {
            FindLocalPackagesResource curResource = null;

            if (await source.GetFeedType(token) == FeedType.FileSystemUnzipped)
            {
                curResource = _cache.GetOrAdd(source.PackageSource,
                    (packageSource) => new FindLocalPackagesResourceUnzipped(packageSource.Source));
            }

            return new Tuple<bool, INuGetResource>(curResource != null, curResource);
        }
    }
}
