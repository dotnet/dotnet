// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;

namespace Test.Utility
{
    public static class StaticHttpSource
    {
        /// <summary>
        /// Creates a handler to override url requests to static content
        /// </summary>
        public static TestHttpSourceProvider CreateHttpSource(Dictionary<string, string> responses, string errorContent = "", TestHttpSource httpSource = null, bool throwOperationCancelledException = false)
        {
            return new TestHttpSourceProvider(responses, httpSource, throwOperationCancelledException);
        }

        /// <summary>
        /// Creates a source and injects an http handler to override the normal http calls
        /// </summary>
        public static SourceRepository CreateSource(string sourceUrl, IEnumerable<Lazy<INuGetResourceProvider>> providers, Dictionary<string, string> responses)
        {
            var handler = new Lazy<INuGetResourceProvider>(() => CreateHttpSource(responses));

            return new SourceRepository(new PackageSource(sourceUrl), providers.Concat(new Lazy<INuGetResourceProvider>[] { handler }));
        }
    }
}
