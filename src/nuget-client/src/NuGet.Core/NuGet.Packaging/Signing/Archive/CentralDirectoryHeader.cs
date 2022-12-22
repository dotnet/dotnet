// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;

// ZIP specification: http://www.pkware.com/documents/casestudies/APPNOTE.TXT

namespace NuGet.Packaging.Signing
{
    internal sealed class CentralDirectoryHeader
    {
        internal const uint SizeInBytesOfFixedLengthFields = 46;

        internal const uint Signature = 0x02014b50;

        internal ushort VersionMadeBy { get; private set; }
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
        internal ushort FileCommentLength { get; private set; }
        internal ushort DiskNumberStart { get; private set; }
        internal ushort InternalFileAttributes { get; private set; }
        internal uint ExternalFileAttributes { get; private set; }
        internal uint RelativeOffsetOfLocalHeader { get; private set; }
        internal byte[] FileName { get; private set; }
        internal byte[] ExtraField { get; private set; }
        internal byte[] FileComment { get; private set; }

        // This property is not part of the ZIP specification.
        internal long OffsetFromStart { get; private set; }

        internal uint GetSizeInBytes()
        {
            return SizeInBytesOfFixedLengthFields +
                FileNameLength +
                ExtraFieldLength +
                FileCommentLength;
        }

        internal static bool TryRead(BinaryReader reader, out CentralDirectoryHeader header)
        {
            header = null;

            var signature = reader.ReadUInt32();

            if (signature != Signature)
            {
                reader.BaseStream.Seek(offset: -sizeof(uint), origin: SeekOrigin.Current);

                return false;
            }

            header = new CentralDirectoryHeader();

            header.OffsetFromStart = reader.BaseStream.Position - sizeof(uint);

            header.VersionMadeBy = reader.ReadUInt16();
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
            header.FileCommentLength = reader.ReadUInt16();
            header.DiskNumberStart = reader.ReadUInt16();
            header.InternalFileAttributes = reader.ReadUInt16();
            header.ExternalFileAttributes = reader.ReadUInt32();
            header.RelativeOffsetOfLocalHeader = reader.ReadUInt32();
            header.FileName = reader.ReadBytes(header.FileNameLength);
            header.ExtraField = reader.ReadBytes(header.ExtraFieldLength);
            header.FileComment = reader.ReadBytes(header.FileCommentLength);

            return true;
        }
    }
}
