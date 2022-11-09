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
    public class FeedTypeResourceProvider : ResourceProvider
    {
        // TODO: should these timeout?
        // Cache feed types for repositories, these should not be changing and for v2 vs v3 folders this can be
        // an expensive call.
        private readonly ConcurrentDictionary<PackageSource, FeedTypeResource> _feedTypeCache
            = new ConcurrentDictionary<PackageSource, FeedTypeResource>();

        private object _accessLock = new object();

        public FeedTypeResourceProvider()
            : base(typeof(FeedTypeResource), nameof(FeedTypeResourceProvider))
        {
        }

        public override Task<Tuple<bool, INuGetResource>> TryCreate(SourceRepository source, CancellationToken token)
        {
            FeedTypeResource curResource = null;

            if (source.FeedTypeOverride == FeedType.Undefined)
            {
                if (!_feedTypeCache.TryGetValue(source.PackageSource, out curResource))
                {
                    lock (_accessLock)
                    {
                        if (!_feedTypeCache.TryGetValue(source.PackageSource, out curResource))
                        {
                            // Check the feed type
                            var feedType = FeedTypeUtility.GetFeedType(source.PackageSource);

                            if (feedType == FeedType.FileSystemUnknown)
                            {
                                // Do not cache unknown folder types
                                curResource = new FeedTypeResource(FeedType.FileSystemUnknown);
                            }
                            else
                            {
                                curResource = new FeedTypeResource(feedType);
                                _feedTypeCache.TryAdd(source.PackageSource, curResource);
                            }
                        }
                    }
                }
            }
            else
            {
                // Use the feed type defined on the source
                curResource = new FeedTypeResource(source.FeedTypeOverride);
            }

            return Task.FromResult(new Tuple<bool, INuGetResource>(curResource != null, curResource));
        }
    }
}
