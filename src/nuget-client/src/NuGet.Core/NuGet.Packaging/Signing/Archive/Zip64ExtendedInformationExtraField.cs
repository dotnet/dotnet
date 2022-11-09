// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;

// ZIP specification: http://www.pkware.com/documents/casestudies/APPNOTE.TXT

namespace NuGet.Packaging.Signing
{
    internal sealed class Zip64ExtendedInformationExtraField : ExtraField
    {
        internal ulong? OriginalUncompressedFileSize { get; private set; }
        internal ulong? SizeOfCompressedData { get; private set; }
        internal ulong? OffsetOfLocalHeaderRecord { get; private set; }
        internal uint? NumberOfDiskOnWhichThisFileStarts { get; private set; }

        private Zip64ExtendedInformationExtraField(
            ushort headerId,
            ushort dataSize,
            byte[] data,
            ulong? originalUncompressedFileSize,
            ulong? sizeOfCompressedData,
            ulong? offsetOfLocalHeaderRecord,
            uint? numberOfDiskOnWhichThisFileStarts)
            : base(headerId, dataSize, data)
        {
            OriginalUncompressedFileSize = originalUncompressedFileSize;
            SizeOfCompressedData = sizeOfCompressedData;
            OffsetOfLocalHeaderRecord = offsetOfLocalHeaderRecord;
            NumberOfDiskOnWhichThisFileStarts = numberOfDiskOnWhichThisFileStarts;
        }

        internal static Zip64ExtendedInformationExtraField Read(
            ushort headerId,
            ushort dataSize,
            byte[] data,
            bool readUncompressedFileSize,
            bool readCompressedFileSize,
            bool readRelativeOffsetOfLocalHeader,
            bool readDiskNumberStart)
        {
            var remainingDataSize = dataSize;

            ushort expectedDataSize = 0;

            if (readUncompressedFileSize)
            {
                expectedDataSize += sizeof(ulong);
            }

            if (readCompressedFileSize)
            {
                expectedDataSize += sizeof(ulong);
            }

            if (readRelativeOffsetOfLocalHeader)
            {
                expectedDataSize += sizeof(ulong);
            }

            if (readDiskNumberStart)
            {
                expectedDataSize += sizeof(uint);
            }

            using (var stream = new MemoryStream(data))
            using (var reader = new BinaryReader(stream))
            {
                ulong? originalUncompressedFileSize = null;
                ulong? sizeOfCompressedData = null;
                ulong? offsetOfLocalHeaderRecord = null;
                uint? numberOfDiskOnWhichThisFileStarts = null;

                if (readUncompressedFileSize && remainingDataSize >= sizeof(ulong))
                {
                    originalUncompressedFileSize = reader.ReadUInt64();
                    remainingDataSize -= sizeof(ulong);

                    if (readCompressedFileSize && remainingDataSize >= sizeof(ulong))
                    {
                        sizeOfCompressedData = reader.ReadUInt64();
                        remainingDataSize -= sizeof(ulong);

                        if (readRelativeOffsetOfLocalHeader && remainingDataSize >= sizeof(ulong))
                        {
                            offsetOfLocalHeaderRecord = reader.ReadUInt64();
                            remainingDataSize -= sizeof(ulong);

                            if (readDiskNumberStart && remainingDataSize >= sizeof(uint))
                            {
                                numberOfDiskOnWhichThisFileStarts = reader.ReadUInt32();
                            }
                        }
                    }
                }

                return new Zip64ExtendedInformationExtraField(
                    headerId,
                    dataSize,
                    data,
                    originalUncompressedFileSize,
                    sizeOfCompressedData,
                    offsetOfLocalHeaderRecord,
                    numberOfDiskOnWhichThisFileStarts);
            }
        }
    }
}
