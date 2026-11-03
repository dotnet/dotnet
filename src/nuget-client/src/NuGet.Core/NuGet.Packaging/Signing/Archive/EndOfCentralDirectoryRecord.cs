// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

// ZIP specification: http://www.pkware.com/documents/casestudies/APPNOTE.TXT

namespace NuGet.Packaging.Signing
{
    internal sealed class EndOfCentralDirectoryRecord
    {
        private static readonly IReadOnlyList<byte> Signature = new byte[4] { 0x50, 0x4b, 0x05, 0x06 };

        internal ushort NumberOfThisDisk { get; private set; }
        internal ushort NumberOfTheDiskWithTheStartOfTheCentralDirectory { get; private set; }
        internal ushort CountOfEntriesInCentralDirectoryOnThisDisk { get; private set; }
        internal ushort CountOfEntriesInCentralDirectory { get; private set; }
        internal uint SizeOfCentralDirectory { get; private set; }
        internal uint OffsetOfStartOfCentralDirectory { get; private set; }
        internal ushort FileCommentLength { get; private set; }
        internal byte[] FileComment { get; private set; } = null!; // Set to non-null in the static Read method.

        // This property is not part of the ZIP specification.
        internal long OffsetFromStart { get; private set; }

        internal static EndOfCentralDirectoryRecord Read(BinaryReader reader)
        {
            SeekToEndOfCentralDirectoryRecord(reader);

            var header = new EndOfCentralDirectoryRecord();

            header.OffsetFromStart = reader.BaseStream.Position;

            reader.ReadUInt32(); // Read and discard the byte signature.

            header.NumberOfThisDisk = reader.ReadUInt16();
            header.NumberOfTheDiskWithTheStartOfTheCentralDirectory = reader.ReadUInt16();
            header.CountOfEntriesInCentralDirectoryOnThisDisk = reader.ReadUInt16();
            header.CountOfEntriesInCentralDirectory = reader.ReadUInt16();
            header.SizeOfCentralDirectory = reader.ReadUInt32();
            header.OffsetOfStartOfCentralDirectory = reader.ReadUInt32();
            header.FileCommentLength = reader.ReadUInt16();
            header.FileComment = reader.ReadBytes(header.FileCommentLength);

            return header;
        }

        private static void SeekToEndOfCentralDirectoryRecord(BinaryReader reader)
        {
            // Match ZipArchive: skip the 18 fixed bytes following the signature and search at most
            // the maximum ZIP comment length plus the signature length backwards.
            const int SizeOfRecordWithoutSignature = 18;
            var searchEnd = reader.BaseStream.Length - SizeOfRecordWithoutSignature;

            if (searchEnd < Signature.Count)
            {
                ThrowByteSignatureNotFoundException(Signature);
            }

            const int DefaultBufferSize = 4096;

            var searchStart = Math.Max(0, searchEnd - ushort.MaxValue - Signature.Count);
            var bufferSize = (int)Math.Min(searchEnd - searchStart, DefaultBufferSize);
            var matchingByteCount = 0;

            // Search backwards in adjacent, non-overlapping chunks, retaining partial matches between chunks.
            for (var position = searchEnd - bufferSize; position >= searchStart; position -= bufferSize)
            {
                reader.BaseStream.Position = position;

                var buffer = reader.ReadBytes(bufferSize);

                for (var i = buffer.Length - 1; i >= 0; --i)
                {
                    if (buffer[i] == Signature[Signature.Count - matchingByteCount - 1])
                    {
                        ++matchingByteCount;

                        if (matchingByteCount == Signature.Count)
                        {
                            reader.BaseStream.Position = position + i;

                            return;
                        }
                    }
                    else
                    {
                        matchingByteCount = buffer[i] == Signature[Signature.Count - 1] ? 1 : 0;
                    }
                }

                bufferSize = (int)Math.Min(position - searchStart, bufferSize);

                if (bufferSize == 0)
                {
                    break;
                }
            }

            ThrowByteSignatureNotFoundException(Signature);
        }

        private static void ThrowByteSignatureNotFoundException(IReadOnlyList<byte> signature)
        {
            throw new InvalidDataException(
                string.Format(CultureInfo.CurrentCulture,
                Strings.ErrorByteSignatureNotFound,
#if NETCOREAPP
                BitConverter.ToString(signature.ToArray()).Replace("-", "", StringComparison.Ordinal)));
#else
                BitConverter.ToString(signature.ToArray()).Replace("-", "")));
#endif
        }
    }
}
