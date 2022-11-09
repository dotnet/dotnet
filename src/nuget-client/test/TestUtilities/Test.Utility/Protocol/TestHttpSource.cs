// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace Test.Utility
{
    /// <summary>
    /// HttpSource with file caching disabled
    /// </summary>
    public class TestHttpSource : HttpSource
    {
        public TestHttpSource(PackageSource source, Dictionary<string, string> responses, string errorContent = "", bool throwOperationCancelledException = false) : base(
            source,
            () => Task.FromResult<HttpHandlerResource>(
                    new TestHttpHandler(
                        new TestMessageHandler(responses, errorContent))),
            NullThrottle.Instance)
        {
            _throwOperationCancelledException = throwOperationCancelledException;
        }

        public TestHttpSource(PackageSource source, Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>> responses, bool throwOperationCancelledException = false) : base(
            source,
            () => Task.FromResult<HttpHandlerResource>(
                    new TestHttpHandler(new TestMessageHandler(responses))),
            NullThrottle.Instance)
        {
            _throwOperationCancelledException = throwOperationCancelledException;
        }

        /// <summary>
        /// Modify or wrap the returned stream.
        /// By default do nothing.
        /// </summary>
        public Func<Stream, Stream> StreamWrapper { get; set; } = (stream) => stream;
        public Action<HttpSourceCachedRequest> HttpSourceCachedRequestInspector { get; set; }

        public bool DisableCaching { get; set; } = true;
        public int CacheHits { get; private set; }
        public int CacheMisses { get; private set; }
        private readonly bool _throwOperationCancelledException;

        public override Task<T> GetAsync<T>(HttpSourceCachedRequest request, Func<HttpSourceResult, Task<T>> processAsync, ILogger log, CancellationToken token)
        {
            if (_throwOperationCancelledException)
            {
                throw new OperationCanceledException(token);
            }

            HttpSourceCachedRequestInspector?.Invoke(request);

            return base.GetAsync(request, processAsync, log, token);
        }

        protected override Stream TryReadCacheFile(string uri, TimeSpan maxAge, string cacheFile)
        {
            if (DisableCaching)
            {
                return null;
            }

            var result = base.TryReadCacheFile(uri, maxAge, cacheFile);

            if (result == null)
            {
                CacheMisses++;
            }
            else
            {
                CacheHits++;
            }

            if (result != null)
            {
                result = StreamWrapper(result);
            }

            return result;
        }
    }
}
