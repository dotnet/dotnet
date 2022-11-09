// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;

// ZIP specification: http://www.pkware.com/documents/casestudies/APPNOTE.TXT

namespace NuGet.Packaging.Test
{
    internal sealed class CentralDirectoryHeader
    {
        private const uint Signature = 0x02014b50;

        internal ushort VersionMadeBy { get; set; }
        internal ushort VersionNeededToExtract { get; set; }
        internal ushort GeneralPurposeBitFlag { get; set; }
        internal ushort CompressionMethod => LocalFileHeader.CompressionMethod;
        internal ushort LastModFileTime => LocalFileHeader.LastModFileTime;
        internal ushort LastModFileDate => LocalFileHeader.LastModFileDate;
        internal uint Crc32 => LocalFileHeader.Crc32;
        internal uint CompressedSize => LocalFileHeader.CompressedSize;
        internal uint UncompressedSize => LocalFileHeader.UncompressedSize;
        internal ushort FileNameLength => (ushort)FileName.Length;
        internal ushort ExtraFieldLength => (ushort)ExtraField.Length;
        internal ushort FileCommentLength => (ushort)FileComment.Length;
        internal ushort DiskNumberStart => 0;
        internal ushort InternalFileAttributes { get; set; }
        internal uint ExternalFileAttributes { get; set; }
        internal uint RelativeOffsetOfLocalHeader => LocalFileHeader.OffsetFromStart;
        internal byte[] FileName { get; set; }
        internal byte[] ExtraField { get; set; }
        internal byte[] FileComment { get; set; }

        internal uint OffsetFromStart { get; private set; }

        internal LocalFileHeader LocalFileHeader { get; set; }

        internal CentralDirectoryHeader()
        {
            FileName = Array.Empty<byte>();
            ExtraField = Array.Empty<byte>();
            FileComment = Array.Empty<byte>();
        }

        internal void Write(BinaryWriter writer)
        {
            OffsetFromStart = (uint)writer.BaseStream.Position;

            writer.Write(Signature);
            writer.Write(VersionMadeBy);
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
            writer.Write(FileCommentLength);
            writer.Write(DiskNumberStart);
            writer.Write(InternalFileAttributes);
            writer.Write(ExternalFileAttributes);
            writer.Write(RelativeOffsetOfLocalHeader);
            writer.Write(FileName);
            writer.Write(ExtraField);
            writer.Write(FileComment);
        }
    }
}
