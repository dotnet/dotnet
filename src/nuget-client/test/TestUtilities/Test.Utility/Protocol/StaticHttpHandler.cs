// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;

namespace Test.Utility
{
    public static class StaticHttpHandler
    {
        /// <summary>
        /// Creates a handler to override url requests to static content
        /// </summary>
        public static TestHttpHandlerProvider CreateHttpHandler(Dictionary<string, string> responses, string errorContent = "")
        {
            return new TestHttpHandlerProvider(() => new TestMessageHandler(responses, errorContent));
        }

        /// <summary>
        /// Creates a handler to override url requests to static content
        /// </summary>
        public static TestHttpHandlerProvider CreateHttpHandler(Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>> responses)
        {
            return new TestHttpHandlerProvider(() => new TestMessageHandler(responses));
        }

        /// <summary>
        /// Creates a source and injects an http handler to override the normal http calls
        /// </summary>
        public static SourceRepository CreateSource(string sourceUrl, IEnumerable<Lazy<INuGetResourceProvider>> providers, Dictionary<string, string> responses, string errorContent = "")
        {
            var handler = new Lazy<INuGetResourceProvider>(() => CreateHttpHandler(responses, errorContent));

            return new SourceRepository(new PackageSource(sourceUrl), providers.Concat(new Lazy<INuGetResourceProvider>[] { handler }));
        }

        /// <summary>
        /// Creates a source and injects an http handler to override the normal http calls
        /// </summary>
        public static SourceRepository CreateSource(
            string sourceUrl,
            IEnumerable<Lazy<INuGetResourceProvider>> providers,
            Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>> responses)
        {
            var handler = new Lazy<INuGetResourceProvider>(() => CreateHttpHandler(responses));

            return new SourceRepository(new PackageSource(sourceUrl), providers.Concat(new Lazy<INuGetResourceProvider>[] { handler }));
        }
    }
}
