// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;

// ZIP specification: http://www.pkware.com/documents/casestudies/APPNOTE.TXT

namespace NuGet.Packaging.Signing
{
    internal class ExtraField
    {
        internal ushort HeaderId { get; private set; }
        internal ushort DataSize { get; private set; }
        internal byte[] Data { get; private set; }

        protected ExtraField(ushort headerId, ushort dataSize, byte[] data)
        {
            HeaderId = headerId;
            DataSize = dataSize;
            Data = data;
        }

        internal static bool TryRead(CentralDirectoryHeader header, out IReadOnlyList<ExtraField> extraFields)
        {
            extraFields = null;

            if (header.ExtraFieldLength == 0)
            {
                return false;
            }

            var readUncompressedFileSize = header.UncompressedSize == ZipConstants.Mask32Bit;
            var readCompressedFileSize = header.CompressedSize == ZipConstants.Mask32Bit;
            var readRelativeOffsetOfLocalHeader = header.RelativeOffsetOfLocalHeader == ZipConstants.Mask32Bit;
            var readDiskNumberStart = header.DiskNumberStart == ZipConstants.Mask16Bit;

            return TryRead(
                header.ExtraField,
                readUncompressedFileSize,
                readCompressedFileSize,
                readRelativeOffsetOfLocalHeader,
                readDiskNumberStart,
                out extraFields);
        }

        internal static bool TryRead(LocalFileHeader header, out IReadOnlyList<ExtraField> extraFields)
        {
            extraFields = null;

            if (header.ExtraFieldLength == 0)
            {
                return false;
            }

            var readUncompressedFileSize = header.UncompressedSize == ZipConstants.Mask32Bit;
            var readCompressedFileSize = header.CompressedSize == ZipConstants.Mask32Bit;

            return TryRead(
                header.ExtraField,
                readUncompressedFileSize,
                readCompressedFileSize,
                readRelativeOffsetOfLocalHeader: false,
                readDiskNumberStart: false,
                extraFields: out extraFields);
        }

        private static bool TryRead(
            byte[] extraFieldBytes,
            bool readUncompressedFileSize,
            bool readCompressedFileSize,
            bool readRelativeOffsetOfLocalHeader,
            bool readDiskNumberStart, out IReadOnlyList<ExtraField> extraFields)
        {
            var fields = new List<ExtraField>();

            extraFields = fields;

            using (var stream = new MemoryStream(extraFieldBytes))
            using (var reader = new BinaryReader(stream))
            {
                while (stream.Position < stream.Length - 1)
                {
                    var headerId = reader.ReadUInt16();
                    var dataSize = reader.ReadUInt16();
                    var data = reader.ReadBytes(dataSize);
                    ExtraField extraField;

                    if (headerId == 0x0001)
                    {
                        extraField = Zip64ExtendedInformationExtraField.Read(
                            headerId,
                            dataSize,
                            data,
                            readUncompressedFileSize,
                            readCompressedFileSize,
                            readRelativeOffsetOfLocalHeader,
                            readDiskNumberStart);
                    }
                    else
                    {
                        extraField = new ExtraField(headerId, dataSize, data);
                    }

                    fields.Add(extraField);
                }
            }

            return true;
        }
    }
}
