// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    public class FindLocalPackagesResourceV2Provider : ResourceProvider
    {
        public FindLocalPackagesResourceV2Provider()
            : base(typeof(FindLocalPackagesResource), nameof(FindLocalPackagesResourceV2Provider), nameof(FindLocalPackagesResourceUnzippedProvider))
        {
        }

        public override async Task<Tuple<bool, INuGetResource>> TryCreate(SourceRepository source, CancellationToken token)
        {
            FindLocalPackagesResource curResource = null;
            var feedType = await source.GetFeedType(token);

            if (feedType == FeedType.FileSystemV2
                || feedType == FeedType.FileSystemUnknown)
            {
                curResource = new FindLocalPackagesResourceV2(source.PackageSource.Source);
            }

            return new Tuple<bool, INuGetResource>(curResource != null, curResource);
        }
    }
}
