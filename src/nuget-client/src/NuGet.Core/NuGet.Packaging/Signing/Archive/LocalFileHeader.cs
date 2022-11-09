// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;

// ZIP specification: http://www.pkware.com/documents/casestudies/APPNOTE.TXT

namespace NuGet.Packaging.Signing
{
    internal sealed class LocalFileHeader
    {
        internal const uint SizeInBytesOfFixedLengthFields = 30;

        internal const uint Signature = 0x04034b50;

        internal ushort VersionNeededToExtract { get; private set; }
        internal ushort GeneralPurposeBitFlag { get; private set; }
        internal ushort CompressionMethod { get; private set; }
        internal ushort LastModFileTime { get; private set; }
        internal ushort LastModFileDate { get; private set; }
        internal uint Crc32 { get; private set; }
        internal uint CompressedSize { get; private set; }
        internal uint UncompressedSize { get; private set; }
        internal ushort FileNameLength { get; private set; }
        internal ushort ExtraFieldLength { get; private set; }
        internal byte[] FileName { get; private set; }
        internal byte[] ExtraField { get; private set; }

        internal static bool TryRead(BinaryReader reader, out LocalFileHeader header)
        {
            header = null;

            var signature = reader.ReadUInt32();

            if (signature != Signature)
            {
                reader.BaseStream.Seek(offset: -sizeof(uint), origin: SeekOrigin.Current);

                return false;
            }

            header = new LocalFileHeader();

            header.VersionNeededToExtract = reader.ReadUInt16();
            header.GeneralPurposeBitFlag = reader.ReadUInt16();
            header.CompressionMethod = reader.ReadUInt16();
            header.LastModFileTime = reader.ReadUInt16();
            header.LastModFileDate = reader.ReadUInt16();
            header.Crc32 = reader.ReadUInt32();
            header.CompressedSize = reader.ReadUInt32();
            header.UncompressedSize = reader.ReadUInt32();
            header.FileNameLength = reader.ReadUInt16();
            header.ExtraFieldLength = reader.ReadUInt16();
            header.FileName = reader.ReadBytes(header.FileNameLength);
            header.ExtraField = reader.ReadBytes(header.ExtraFieldLength);

            return true;
        }
    }
}
