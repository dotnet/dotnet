// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    /// <summary>
    /// Holds a shared <see cref="HttpSource"/>. 
    /// This is expected to be shared across the app and should not be disposed of.
    /// </summary>
    public class HttpSourceResource : INuGetResource
    {
        public HttpSourceResource(HttpSource httpSource)
        {
            HttpSource = httpSource;
        }

        public HttpSource HttpSource { get; }
    }
}
