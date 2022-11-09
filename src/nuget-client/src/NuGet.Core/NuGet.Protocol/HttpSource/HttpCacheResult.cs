// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;

namespace NuGet.Protocol
{
    public class HttpCacheResult
    {
        public HttpCacheResult(TimeSpan maxAge, string newFile, string cacheFule)
        {
            MaxAge = maxAge;
            NewFile = newFile;
            CacheFile = cacheFule;
        }

        public TimeSpan MaxAge { get; }
        public string NewFile { get; }
        public string CacheFile { get; }
        public Stream Stream { get; set; }
    }
}
