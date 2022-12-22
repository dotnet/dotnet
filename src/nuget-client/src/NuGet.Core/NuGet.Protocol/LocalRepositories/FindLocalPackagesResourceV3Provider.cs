// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    public class FindLocalPackagesResourceV3Provider : ResourceProvider
    {
        public FindLocalPackagesResourceV3Provider()
            : base(typeof(FindLocalPackagesResource), nameof(FindLocalPackagesResourceV3Provider), nameof(FindLocalPackagesResourceV2Provider))
        {
        }

        public override async Task<Tuple<bool, INuGetResource>> TryCreate(SourceRepository source, CancellationToken token)
        {
            FindLocalPackagesResource curResource = null;

            if (await source.GetFeedType(token) == FeedType.FileSystemV3)
            {
                curResource = new FindLocalPackagesResourceV3(source.PackageSource.Source);
            }

            return new Tuple<bool, INuGetResource>(curResource != null, curResource);
        }
    }
}
