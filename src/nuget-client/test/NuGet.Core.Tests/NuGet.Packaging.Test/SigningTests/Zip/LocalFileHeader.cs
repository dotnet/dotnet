// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;

// ZIP specification: http://www.pkware.com/documents/casestudies/APPNOTE.TXT

namespace NuGet.Packaging.Test
{
    internal sealed class LocalFileHeader
    {
        internal const uint Signature = 0x04034b50;

        internal ushort VersionNeededToExtract { get; set; }
        internal ushort GeneralPurposeBitFlag { get; set; }
        internal ushort CompressionMethod => 0;
        internal ushort LastModFileTime { get; set; }
        internal ushort LastModFileDate { get; set; }
        internal uint Crc32 => Signing.Crc32.CalculateCrc(FileData);
        internal uint CompressedSize => (uint)FileData.Length;
        internal uint UncompressedSize => (uint)FileData.Length;
        internal ushort FileNameLength => (ushort)FileName.Length;
        internal ushort ExtraFieldLength => (ushort)ExtraField.Length;
        internal byte[] FileName { get; set; }
        internal byte[] ExtraField { get; set; }
        internal byte[] FileData { get; set; }

        internal uint OffsetFromStart { get; private set; }

        internal LocalFileHeader()
        {
            FileName = Array.Empty<byte>();
            ExtraField = Array.Empty<byte>();
            FileData = Array.Empty<byte>();
        }

        internal void Write(BinaryWriter writer)
        {
            OffsetFromStart = (uint)writer.BaseStream.Position;

            writer.Write(Signature);
            writer.Write(VersionNeededToExtract);
            writer.Write(GeneralPurposeBitFlag);
            writer.Write(CompressionMethod);
            writer.Write(LastModFileTime);
            writer.Write(LastModFileDate);
            writer.Write(Crc32);
            writer.Write(CompressedSize);
            writer.Write(UncompressedSize);
            writer.Write(FileNameLength);
            writer.Write(ExtraFieldLength);
            writer.Write(FileName);
            writer.Write(ExtraField);
            writer.Write(FileData);
        }
    }
}
