// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.DotNet.Build.Tasks.Installers
{
    internal sealed class RpmPackage(RpmLead lead, RpmHeader<RpmSignatureTag> signature, RpmHeader<RpmHeaderTag> header, MemoryStream archiveStream) : IDisposable
    {
        public RpmLead Lead { get; set; } = lead;
        public RpmHeader<RpmSignatureTag> Signature { get; set; } = signature;
        public RpmHeader<RpmHeaderTag> Header { get; set; } = header;
        public MemoryStream ArchiveStream { get; set; } = archiveStream;

        public static unsafe RpmPackage Read(Stream stream)
        {
            RpmLead lead = RpmLead.Read(stream);

            RpmHeader<RpmSignatureTag> signature = RpmHeader<RpmSignatureTag>.Read(stream, RpmSignatureTag.HeaderSignatures);
            stream.AlignReadTo(8);
            RpmHeader<RpmHeaderTag> header = RpmHeader<RpmHeaderTag>.Read(stream, RpmHeaderTag.Immutable);

            if (header.Entries.First(e => e.Tag == RpmHeaderTag.PayloadCompressor).Value is not "gzip")
            {
                throw new InvalidDataException("Unsupported payload compressor");
            }

            using GZipStream gzipStream = new(stream, CompressionMode.Decompress, leaveOpen: true);
            MemoryStream archiveStream = new();
            gzipStream.CopyTo(archiveStream);
            archiveStream.Position = 0;
            return new RpmPackage(lead, signature, header, archiveStream);
        }

        public static unsafe MemoryStream GetSignableContent(Stream stream)
        {
            // We don't care about the lead and signature header
            RpmLead.Read(stream);
            RpmHeader<RpmSignatureTag>.Read(stream, RpmSignatureTag.HeaderSignatures);
            stream.AlignReadTo(8);

            // Remaining stream content is the signable content
            // This includes all the magic and alignment bytes in both header and archive sections.
            MemoryStream signableContentStream = new();
            stream.CopyTo(signableContentStream);
            signableContentStream.Position = 0;
            return signableContentStream;
        }

        public void WriteTo(Stream stream)
        {
            Lead.WriteTo(stream);
            Signature.WriteTo(stream, RpmSignatureTag.HeaderSignatures);
            stream.AlignWriteTo(8);
            Header.WriteTo(stream, RpmHeaderTag.Immutable);

            using GZipStream gzipStream = new(stream, CompressionLevel.Optimal, leaveOpen: true);
            ArchiveStream.CopyTo(gzipStream);
        }

        public override string ToString()
        {
            return $"""
            Lead:
            {Lead}
            Signature:
            {Signature}
            Header:
            {Header}
            """;
        }

        public void Dispose() => ArchiveStream.Dispose();
    }
}
