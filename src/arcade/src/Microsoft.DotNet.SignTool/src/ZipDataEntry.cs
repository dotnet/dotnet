// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using System.IO;
using System.IO.Compression;

namespace Microsoft.DotNet.SignTool
{
    internal sealed class ZipDataEntry : IDisposable
    {
        string _relativePath;
        Stream _stream;
        ImmutableArray<byte> _contentHash;

        public ZipDataEntry(string relativePath, Stream contentStream) : this(relativePath, contentStream, contentStream?.Length ?? 0)
        {
        }

        public ZipDataEntry(string relativePath, Stream contentStream, long contentSize)
        {
            _relativePath = relativePath;

            // this might be just a pointer to a folder. We skip those.
            if (contentStream == null)
                return;

            if (contentStream.CanSeek)
            {
                _stream = contentStream;
            }
            else
            {
                // if we can't seek we need to copy the stream into a (seekable) MemoryStream so we can compute the content hash
                _stream = new MemoryStream((int)contentSize);
                contentStream.CopyTo(_stream);
            }

            // compute content hash and reset position back
            _contentHash = ContentUtil.GetContentHash(_stream);
            _stream.Position = 0;
        }

        public ZipDataEntry(ZipArchiveEntry entry)
        {
            _relativePath = entry.FullName; // lgtm [cs/zipslip] Archive from trusted source

            // this might be just a pointer to a folder. We skip those.
            if (_relativePath.EndsWith("/") && entry.Name == "")
                return;

            // the stream returned by entry.Open() isn't seekable,
            // we can avoid creating a MemoryStream copy by just opening a separate instance for computing the content hash
            using var contentHashStream = entry.Open();
            _contentHash = ContentUtil.GetContentHash(contentHashStream);

            // this will already be at position 0
            _stream = entry.Open();
        }

        public string RelativePath => _relativePath;

        public ImmutableArray<byte> ContentHash => _contentHash;

        public void WriteToFile(string path)
        {
            using var fs = File.Create(path);
            _stream.CopyTo(fs);
        }

        public void Dispose()
        {
            _stream.Dispose();
            _stream = null;
        }
    }
}
