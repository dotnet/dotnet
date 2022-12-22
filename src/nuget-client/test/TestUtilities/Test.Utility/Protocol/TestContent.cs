// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Test.Utility
{
    public class TestContent : HttpContent
    {
#pragma warning disable CA2213
        // Need to disable this rule because the analyzer thinks we are not disposing _stream
        // when in reality, it is.
        // See https://github.com/dotnet/roslyn-analyzers/issues/6172
        //     https://github.com/dotnet/roslyn-analyzers/issues/6202
        private readonly MemoryStream _stream;
#pragma warning restore CA2213
        private bool _isDisposed = false;

        public TestContent(string s)
        {
            _stream = new MemoryStream(Encoding.UTF8.GetBytes(s));
            _stream.Seek(0, SeekOrigin.Begin);
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext? context)
        {
            return _stream.CopyToAsync(stream);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = _stream.Length;
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (_isDisposed)
            {
                return;
            }

            if (disposing)
            {
                // free managed resources
                _stream.Dispose();
            }

            _isDisposed = true;
        }
    }
}
