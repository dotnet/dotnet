// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;

#if IS_DESKTOP
using System.Net;
#endif

namespace NuGet.Protocol
{
    public class HttpSourceResourceProvider : ResourceProvider
    {
#if IS_DESKTOP
        private const int DefaultMaxHttpRequestsPerSource = 64;
#endif
        // Only one HttpSource per source should exist. This is to reduce the number of TCP connections.
        private readonly ConcurrentDictionary<PackageSource, HttpSourceResource> _cache
            = new ConcurrentDictionary<PackageSource, HttpSourceResource>();

        /// <summary>
        /// The throttle to apply to all <see cref="HttpSource"/> HTTP requests.
        /// </summary>
        public static IThrottle Throttle { get; set; }

        public HttpSourceResourceProvider()
            : base(typeof(HttpSourceResource),
                  nameof(HttpSourceResource),
                  NuGetResourceProviderPositions.Last)
        {
        }

        public override Task<Tuple<bool, INuGetResource>> TryCreate(SourceRepository source, CancellationToken token)
        {
            Debug.Assert(source.PackageSource.IsHttp, "HTTP source requested for a non-http source.");

            HttpSourceResource curResource = null;

            if (source.PackageSource.IsHttp)
            {
                IThrottle throttle = NullThrottle.Instance;

                if (Throttle != null)
                {
                    throttle = Throttle;
                }
                else if (source.PackageSource.MaxHttpRequestsPerSource > 0)
                {
                    throttle = SemaphoreSlimThrottle.CreateSemaphoreThrottle(source.PackageSource.MaxHttpRequestsPerSource);
                }
#if IS_DESKTOP
                else if (ServicePointManager.DefaultConnectionLimit == ServicePointManager.DefaultPersistentConnectionLimit)
                {
                    source.PackageSource.MaxHttpRequestsPerSource = DefaultMaxHttpRequestsPerSource;
                }
#endif

                curResource = _cache.GetOrAdd(
                    source.PackageSource,
                    packageSource => new HttpSourceResource(HttpSource.Create(source, throttle)));
            }

            return Task.FromResult(new Tuple<bool, INuGetResource>(curResource != null, curResource));
        }
    }
}
