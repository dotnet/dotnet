// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;

namespace NuGet.Protocol
{
    public class HttpSourceResult : IDisposable
    {
        private bool _disposed;

        public Stream Stream { get; private set; }
        public HttpSourceResultStatus Status { get; }
        public string CacheFile { get; }

        public HttpSourceResult(HttpSourceResultStatus status)
        {
            Status = status;
            Stream = null;
            CacheFile = null;
        }

        public HttpSourceResult(HttpSourceResultStatus status, string cacheFileName, Stream stream)
        {
            Status = status;
            Stream = stream;
            CacheFile = cacheFileName;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                if (Stream != null)
                {
                    Stream.Dispose();
                    Stream = null;
                }
            }

            _disposed = true;
        }
    }
}
