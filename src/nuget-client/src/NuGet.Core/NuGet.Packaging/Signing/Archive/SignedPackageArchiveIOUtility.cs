// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using NuGet.Common;

namespace NuGet.Packaging.Signing
{
    public static class SignedPackageArchiveIOUtility
    {
        private const int _bufferSize = 4096;

        private static readonly SigningSpecifications _signingSpecification = SigningSpecifications.V1;

        // used while converting DateTime to MS-DOS date time format
        private const int ValidZipDate_YearMin = 1980;
        private const int ValidZipDate_YearMax = 2107;

        /// <summary>
        /// Read bytes from a BinaryReader and write them to the BinaryWriter and stop when the provided position
        /// is the current position of the BinaryReader's base stream. It does not read the byte in the provided position.
        /// </summary>
        /// <param name="reader">Read bytes from this stream.</param>
        /// <param name="writer">Write bytes to this stream.</param>
        /// <param name="position">Position to stop copying data.</param>
        public static void ReadAndWriteUntilPosition(BinaryReader reader, BinaryWriter writer, long position)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (position > reader.BaseStream.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(position), Strings.SignedPackageArchiveIOExtraRead);
            }

            if (position < reader.BaseStream.Position)
            {
                throw new ArgumentOutOfRangeException(nameof(position), Strings.SignedPackageArchiveIOInvalidRead);
            }

            byte[] buffer = ArrayPool<byte>.Shared.Rent(_bufferSize);

            Stream stream = reader.BaseStream;
            long currentPosition;
            while ((currentPosition = stream.Position) != position)
            {
                var bytesToRead = (int)Math.Min(position - currentPosition, buffer.Length);

                int bytesRead = stream.Read(buffer, offset: 0, bytesToRead);
                writer.Write(buffer, index: 0, bytesRead);
            }

            ArrayPool<byte>.Shared.Return(buffer);
        }

        /// <summary>
        /// Read bytes from a BinaryReader and hash them with a given HashAlgorithm and stop when the provided position
        /// is the current position of the BinaryReader's base stream. It does not hash the byte in the provided position.
        ///
        /// </summary>
        /// <param name="reader">Read bytes from this stream</param>
        /// <param name="hashAlgorithm">HashAlgorithm used to hash contents</param>
        /// <param name="position">Position to stop copying data</param>
        public static void ReadAndHashUntilPosition(BinaryReader reader, HashAlgorithm hashAlgorithm, long position)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (position > reader.BaseStream.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(position), Strings.SignedPackageArchiveIOExtraRead);
            }

            if (position < reader.BaseStream.Position)
            {
                throw new ArgumentOutOfRangeException(nameof(position), Strings.SignedPackageArchiveIOInvalidRead);
            }

            byte[] buffer = ArrayPool<byte>.Shared.Rent(_bufferSize);

            Stream stream = reader.BaseStream;
            long currentPosition;
            while ((currentPosition = stream.Position) != position)
            {
                var bytesToRead = (int)Math.Min(position - currentPosition, buffer.Length);

                int bytesRead = stream.Read(buffer, offset: 0, bytesToRead);
                HashBytes(hashAlgorithm, buffer, bytesRead);
            }

            ArrayPool<byte>.Shared.Return(buffer);
        }

        /// <summary>
        /// Hashes given byte array with a specified HashAlgorithm
        ///
        /// </summary>
        /// <param name="hashAlgorithm">HashAlgorithm used to hash contents</param>
        /// <param name="bytes">Content to hash</param>
        public static void HashBytes(HashAlgorithm hashAlgorithm, byte[] bytes)
        {
            HashBytes(hashAlgorithm, bytes, bytes?.Length ?? 0);
        }

        internal static void HashBytes(HashAlgorithm hashAlgorithm, byte[] bytes, int count)
        {
            if (hashAlgorithm == null)
            {
                throw new ArgumentNullException(nameof(hashAlgorithm));
            }

            if (bytes == null || bytes.Length == 0)
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(bytes));
            }

            if (count <= 0 || bytes.Length < count)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            hashAlgorithm.TransformBlock(bytes, inputOffset: 0, count, outputBuffer: null, outputOffset: 0);
        }


        /// <summary>
        /// Read bytes from a BinaryReader and hash them with a given HashAlgorithm wrapper and stop when the provided position
        /// is the current position of the BinaryReader's base stream. It does not hash the byte in the provided position.
        /// </summary>
        /// <param name="reader">Read bytes from this stream</param>
        /// <param name="hashFunc">HashAlgorithm wrapper used to hash contents cross platform</param>
        /// <param name="position">Position to stop copying data</param>
        internal static void ReadAndHashUntilPosition(BinaryReader reader, Sha512HashFunction hashFunc, long position)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (position > reader.BaseStream.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(position), Strings.SignedPackageArchiveIOExtraRead);
            }

            if (position < reader.BaseStream.Position)
            {
                throw new ArgumentOutOfRangeException(nameof(position), Strings.SignedPackageArchiveIOInvalidRead);
            }

            byte[] buffer = ArrayPool<byte>.Shared.Rent(_bufferSize);

            Stream stream = reader.BaseStream;
            long currentPosition;
            while ((currentPosition = stream.Position) != position)
            {
                var bytesToRead = (int)Math.Min(position - currentPosition, buffer.Length);

                int bytesRead = stream.Read(buffer, offset: 0, bytesToRead);
                HashBytes(hashFunc, buffer, bytesRead);
            }

            ArrayPool<byte>.Shared.Return(buffer);
        }

        /// <summary>
        /// Hashes given byte array with a specified HashAlgorithm wrapper which works cross platform.
        /// </summary>
        /// <param name="hashFunc">HashAlgorithm wrapper used to hash contents cross platform</param>
        /// <param name="bytes">Content to hash</param>
        internal static void HashBytes(Sha512HashFunction hashFunc, byte[] bytes)
        {
            HashBytes(hashFunc, bytes, bytes?.Length ?? 0);
        }

        /// <summary>
        /// Hashes given byte array with a specified HashAlgorithm wrapper which works cross platform.
        /// </summary>
        /// <param name="hashFunc">HashAlgorithm wrapper used to hash contents cross platform</param>
        /// <param name="bytes">Content to hash</param>
        /// <param name="count">The number of bytes in the input byte array to use as data.</param>
        internal static void HashBytes(Sha512HashFunction hashFunc, byte[] bytes, int count)
        {
            if (hashFunc == null)
            {
                throw new ArgumentNullException(nameof(hashFunc));
            }

            if (bytes == null || bytes.Length == 0)
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(bytes));
            }

            if (count <= 0 || bytes.Length < count)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            hashFunc.Update(bytes, offset: 0, count);
        }

        /// <summary>
        /// Read ZIP's offsets and positions of offsets.
        /// </summary>
        /// <param name="reader">binary reader to zip archive</param>
        /// <param name="validateSignatureEntry">boolean to skip validate signature entry</param>
        /// <returns>metadata with offsets and positions for entries</returns>
        public static SignedPackageArchiveMetadata ReadSignedArchiveMetadata(BinaryReader reader, bool validateSignatureEntry = true)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            var metadata = new SignedPackageArchiveMetadata()
            {
                StartOfLocalFileHeaders = reader.BaseStream.Length
            };

            var endOfCentralDirectoryRecord = EndOfCentralDirectoryRecord.Read(reader);
            var endOfCentralDirectoryRecordPosition = endOfCentralDirectoryRecord.OffsetFromStart;

            reader.BaseStream.Seek(endOfCentralDirectoryRecord.OffsetOfStartOfCentralDirectory, SeekOrigin.Begin);

            var centralDirectoryRecords = new List<CentralDirectoryHeaderMetadata>();
            var packageSignatureFileMetadataIndex = -1;
            var index = 0;

            while (CentralDirectoryHeader.TryRead(reader, out var header))
            {
                metadata.StartOfLocalFileHeaders = Math.Min(metadata.StartOfLocalFileHeaders, header.RelativeOffsetOfLocalHeader);

                var isPackageSignatureFile = SignedPackageArchiveUtility.IsPackageSignatureFileEntry(
                    header.FileName,
                    header.GeneralPurposeBitFlag);

                if (isPackageSignatureFile)
                {
                    if (packageSignatureFileMetadataIndex != -1)
                    {
                        throw new SignatureException(NuGetLogCode.NU3005, Strings.MultiplePackageSignatureFiles);
                    }

                    packageSignatureFileMetadataIndex = index;
                }

                var centralDirectoryMetadata = new CentralDirectoryHeaderMetadata()
                {
                    Position = header.OffsetFromStart,
                    OffsetToLocalFileHeader = header.RelativeOffsetOfLocalHeader,
                    IsPackageSignatureFile = isPackageSignatureFile,
                    HeaderSize = header.GetSizeInBytes(),
                    IndexInHeaders = index
                };

                centralDirectoryRecords.Add(centralDirectoryMetadata);

                ++index;
            }

            if (centralDirectoryRecords.Count == 0)
            {
                throw new InvalidDataException(Strings.ErrorInvalidPackageArchive);
            }

            if (packageSignatureFileMetadataIndex == -1)
            {
                throw new SignatureException(NuGetLogCode.NU3005, Strings.NoPackageSignatureFile);
            }

            var lastCentralDirectoryRecord = centralDirectoryRecords.Last();
            var endOfCentralDirectoryPosition = lastCentralDirectoryRecord.Position + lastCentralDirectoryRecord.HeaderSize;
            var endOfLocalFileHeadersPosition = centralDirectoryRecords.Min(record => record.Position);

            UpdateLocalFileHeadersTotalSize(centralDirectoryRecords, endOfLocalFileHeadersPosition);

            metadata.EndOfCentralDirectory = lastCentralDirectoryRecord.Position + lastCentralDirectoryRecord.HeaderSize;
            metadata.CentralDirectoryHeaders = centralDirectoryRecords;
            metadata.SignatureCentralDirectoryHeaderIndex = packageSignatureFileMetadataIndex;

            if (validateSignatureEntry)
            {
                AssertSignatureEntryMetadata(reader, metadata);
            }

            return metadata;
        }

        internal static void RemoveSignature(BinaryReader reader, BinaryWriter writer)
        {
            var metadata = ReadSignedArchiveMetadata(reader);
            var signatureFileMetadata = metadata.GetPackageSignatureFileCentralDirectoryHeaderMetadata();

            reader.BaseStream.Seek(offset: 0, origin: SeekOrigin.Begin);
            writer.BaseStream.Seek(offset: 0, origin: SeekOrigin.Begin);

            // Write local file headers up until the package signature file local file header.
            ReadAndWriteUntilPosition(reader, writer, signatureFileMetadata.OffsetToLocalFileHeader);

            // Skip over package signature file local file header.
            reader.BaseStream.Seek(offset: signatureFileMetadata.FileEntryTotalSize, origin: SeekOrigin.Current);

            // Write any remaining local file headers, then central directory headers up until
            // the package signature central directory header.
            ReadAndWriteUntilPosition(reader, writer, signatureFileMetadata.Position);

            // Skip over package signature file central directory header.
            reader.BaseStream.Seek(offset: signatureFileMetadata.HeaderSize, origin: SeekOrigin.Current);

            // Write any remaining central directory headers.
            ReadAndWriteUntilPosition(reader, writer, metadata.EndOfCentralDirectory);

            ReadAndWriteUpdatedEndOfCentralDirectoryRecordIntoZip(
                reader,
                writer,
                entryCountChange: -1,
                sizeOfSignatureCentralDirectoryRecord: -signatureFileMetadata.HeaderSize,
                sizeOfSignatureFileHeaderAndData: -signatureFileMetadata.FileEntryTotalSize);
        }

        private static UnsignedPackageArchiveMetadata ReadUnsignedArchiveMetadata(BinaryReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            var endOfCentralDirectoryRecord = EndOfCentralDirectoryRecord.Read(reader);
            var endOfCentralDirectoryRecordPosition = endOfCentralDirectoryRecord.OffsetFromStart;

            reader.BaseStream.Seek(endOfCentralDirectoryRecord.OffsetOfStartOfCentralDirectory, SeekOrigin.Begin);

            var centralDirectoryRecords = new List<CentralDirectoryHeaderMetadata>();
            CentralDirectoryHeader header;

            while (CentralDirectoryHeader.TryRead(reader, out header))
            {
                var centralDirectoryMetadata = new CentralDirectoryHeaderMetadata()
                {
                    Position = header.OffsetFromStart,
                    OffsetToLocalFileHeader = header.RelativeOffsetOfLocalHeader,
                    HeaderSize = header.GetSizeInBytes(),
                };

                centralDirectoryRecords.Add(centralDirectoryMetadata);
            }

            if (centralDirectoryRecords.Count == 0)
            {
                throw new InvalidDataException(Strings.ErrorInvalidPackageArchive);
            }

            var lastCentralDirectoryRecord = centralDirectoryRecords.Last();
            var endOfCentralDirectoryPosition = lastCentralDirectoryRecord.Position + lastCentralDirectoryRecord.HeaderSize;
            var endOfLocalFileHeadersPosition = centralDirectoryRecords.Min(record => record.Position);

            UpdateLocalFileHeadersTotalSize(centralDirectoryRecords, endOfLocalFileHeadersPosition);

            return new UnsignedPackageArchiveMetadata(endOfLocalFileHeadersPosition, endOfCentralDirectoryPosition);
        }

        private static void UpdateLocalFileHeadersTotalSize(
            IReadOnlyList<CentralDirectoryHeaderMetadata> records,
            long startOfCentralDirectory)
        {
            var orderedRecords = records.OrderBy(record => record.OffsetToLocalFileHeader).ToArray();

            for (var i = 0; i < orderedRecords.Length - 1; ++i)
            {
                var current = orderedRecords[i];
                var next = orderedRecords[i + 1];

                current.FileEntryTotalSize = next.OffsetToLocalFileHeader - current.OffsetToLocalFileHeader;
            }

            if (orderedRecords.Length > 0)
            {
                var last = orderedRecords.Last();

                last.FileEntryTotalSize = startOfCentralDirectory - last.OffsetToLocalFileHeader;
            }
        }

        /// <summary>
        /// Asserts the validity of central directory header and local file header for the package signature file entry.
        /// </summary>
        /// <param name="reader">BinaryReader on the package.</param>
        /// <param name="metadata">Metadata for the package signature file's central directory header.</param>
        /// <exception cref="SignatureException">Thrown if either header is invalid.</exception>
        private static void AssertSignatureEntryMetadata(BinaryReader reader, SignedPackageArchiveMetadata metadata)
        {
            var signatureCentralDirectoryHeader = metadata.GetPackageSignatureFileCentralDirectoryHeaderMetadata();

            // Move to central directory header and skip header signature (4 bytes) and version fields (2 entries of 2 bytes each)
            reader.BaseStream.Seek(offset: signatureCentralDirectoryHeader.Position + 8L, origin: SeekOrigin.Begin);

            // check central directory file header
            AssertSignatureEntryCommonHeaderFields(
                reader,
                Strings.InvalidPackageSignatureFileEntry,
                Strings.InvalidPackageSignatureFileEntryCentralDirectoryHeader);

            // Skip file name length (2 bytes), extra field length (2 bytes), file comment length (2 bytes),
            // disk number start (2 bytes), and internal file attributes (2 bytes)
            reader.BaseStream.Seek(offset: 10, origin: SeekOrigin.Current);

            var externalFileAttributes = reader.ReadUInt32();
            AssertValue(
                expectedValue: 0U,
                actualValue: externalFileAttributes,
                errorCode: NuGetLogCode.NU3005,
                errorMessagePrefix: Strings.InvalidPackageSignatureFileEntry,
                errorMessageSuffix: Strings.InvalidPackageSignatureFileEntryCentralDirectoryHeader,
                fieldName: "external file attributes");

            // Move to local file header and skip header signature (4 bytes) and version field (2 bytes)
            reader.BaseStream.Seek(offset: signatureCentralDirectoryHeader.OffsetToLocalFileHeader + 6L, origin: SeekOrigin.Begin);

            // check local file header
            AssertSignatureEntryCommonHeaderFields(
                reader,
                Strings.InvalidPackageSignatureFileEntry,
                Strings.InvalidPackageSignatureFileEntryLocalFileHeader);
        }

        private static void AssertSignatureEntryCommonHeaderFields(
            BinaryReader reader,
            string errorPrefix,
            string errorSuffix)
        {
            var signatureEntryErrorCode = NuGetLogCode.NU3005;

            // Assert general purpose bits to 0
            uint actualValue = reader.ReadUInt16();
            AssertValue(
                expectedValue: 0U,
                actualValue: actualValue,
                errorCode: signatureEntryErrorCode,
                errorMessagePrefix: errorPrefix,
                errorMessageSuffix: errorSuffix,
                fieldName: "general purpose bit");

            // Assert compression method to 0
            actualValue = reader.ReadUInt16();
            AssertValue(
                expectedValue: 0U,
                actualValue: actualValue,
                errorCode: signatureEntryErrorCode,
                errorMessagePrefix: errorPrefix,
                errorMessageSuffix: errorSuffix,
                fieldName: "compression method");

            // skip date (2 bytes), time (2 bytes) and crc32 (4 bytes)
            reader.BaseStream.Seek(offset: 8L, origin: SeekOrigin.Current);

            // assert that file compressed and uncompressed sizes are the same
            var compressedSize = reader.ReadUInt32();
            var uncompressedSize = reader.ReadUInt32();
            if (compressedSize != uncompressedSize)
            {
                var failureCause = string.Format(
                    CultureInfo.CurrentCulture,
                    errorSuffix,
                    "compressed size",
                    compressedSize);

                throw new SignatureException(
                    signatureEntryErrorCode,
                    string.Format(CultureInfo.CurrentCulture, errorPrefix, failureCause));
            }
        }

        private static void AssertValue(
            uint expectedValue,
            uint actualValue,
            NuGetLogCode errorCode,
            string errorMessagePrefix,
            string errorMessageSuffix,
            string fieldName)
        {
            if (!actualValue.Equals(expectedValue))
            {
                var failureCause = string.Format(
                    CultureInfo.CurrentCulture,
                    errorMessageSuffix,
                    fieldName,
                    actualValue);

                throw new SignatureException(
                    errorCode,
                    string.Format(CultureInfo.CurrentCulture, errorMessagePrefix, failureCause));
            }
        }

        /// <summary>
        /// Writes the signature data into the zip using the writer.
        /// The reader is used to read the exisiting zip. 
        /// </summary>
        /// <param name="signatureStream">MemoryStream of the signature to be inserted into the zip.</param>
        /// <param name="reader">BinaryReader to be used to read the existing zip data.</param>
        /// <param name="writer">BinaryWriter to be used to write the signature into the zip.</param>
        internal static void WriteSignatureIntoZip(
            MemoryStream signatureStream,
            BinaryReader reader,
            BinaryWriter writer)
        {
            if (signatureStream == null)
            {
                throw new ArgumentNullException(nameof(signatureStream));
            }

            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            var packageMetadata = ReadUnsignedArchiveMetadata(reader);
            var signatureBytes = signatureStream.ToArray();
            var signatureCrc32 = Crc32.CalculateCrc(signatureBytes);
            var signatureDosTime = DateTimeToDosTime(DateTime.Now);

            // ensure both streams are reset
            reader.BaseStream.Seek(offset: 0, origin: SeekOrigin.Begin);
            writer.BaseStream.Seek(offset: 0, origin: SeekOrigin.Begin);

            // copy all data till previous end of local file headers
            ReadAndWriteUntilPosition(reader, writer, packageMetadata.EndOfLocalFileHeadersPosition);

            // write the signature local file header
            var signatureFileHeaderLength = WriteLocalFileHeaderIntoZip(writer, signatureBytes, signatureCrc32, signatureDosTime);

            // write the signature file
            var signatureFileLength = WriteFileIntoZip(writer, signatureBytes);

            // copy all data that was after previous end of local file headers till previous end of central directory headers
            ReadAndWriteUntilPosition(reader, writer, packageMetadata.EndOfCentralDirectoryHeadersPosition);

            // write the central directory header for signature file
            var signatureCentralDirectoryHeaderLength = WriteCentralDirectoryHeaderIntoZip(writer, signatureBytes, signatureCrc32, signatureDosTime, packageMetadata.EndOfLocalFileHeadersPosition);

            // copy all data that was after previous end of central directory headers till previous start of end of central directory record
            ReadAndWriteUntilPosition(reader, writer, packageMetadata.EndOfCentralDirectoryHeadersPosition);

            var totalSignatureSize = signatureFileHeaderLength + signatureFileLength;

            // update and write the end of central directory record
            ReadAndWriteUpdatedEndOfCentralDirectoryRecordIntoZip(
                reader,
                writer,
                entryCountChange: 1,
                sizeOfSignatureCentralDirectoryRecord: signatureCentralDirectoryHeaderLength,
                sizeOfSignatureFileHeaderAndData: totalSignatureSize);
        }

        /// <summary>
        /// Writes a local file header into a zip using the writer starting at the writer.BaseStream.Position.
        /// </summary>
        /// <param name="writer">BinaryWriter to be used to write file.</param>
        /// <param name="fileData">Byte[] of the corresponding file to be written into the zip.</param>
        /// <param name="crc32">CRC-32 for the file.</param>
        /// <param name="dosDateTime">Last modified DateTime for the file data.</param>
        /// <returns>Number of total bytes written into the zip.</returns>
        private static long WriteLocalFileHeaderIntoZip(
            BinaryWriter writer,
            byte[] fileData,
            uint crc32,
            uint dosDateTime)
        {
            // Write the file header signature
            writer.Write(LocalFileHeader.Signature);

            // Version needed to extract:  2.0
            writer.Write((ushort)20);

            // General purpose bit flags
            writer.Write((ushort)0);

            // The file is stored (no compression)
            writer.Write((ushort)0);

            // write date and time
            writer.Write(dosDateTime);

            // write file CRC32
            writer.Write(crc32);

            // write uncompressed size
            writer.Write((uint)fileData.Length);

            // write compressed size - same as uncompressed since file should have no compression
            writer.Write((uint)fileData.Length);

            // write file name length
            var fileNameBytes = Encoding.ASCII.GetBytes(_signingSpecification.SignaturePath);
            var fileNameLength = fileNameBytes.Length;
            writer.Write((ushort)fileNameLength);

            // write extra field length
            writer.Write((ushort)0);

            // write file name
            writer.Write(fileNameBytes);

            // calculate the total length of data written
            var writtenDataLength = LocalFileHeader.SizeInBytesOfFixedLengthFields + fileNameLength;

            return writtenDataLength;
        }

        /// <summary>
        /// Writes a file into a zip using the writer starting at the writer.BaseStream.Position.
        /// </summary>
        /// <param name="writer">BinaryWriter to be used to write file.</param>
        /// <param name="fileData">Byte[] of the file to be written into the zip.</param>
        /// <returns>Number of total bytes written into the zip.</returns>
        private static long WriteFileIntoZip(BinaryWriter writer, byte[] fileData)
        {
            // write file
            writer.Write(fileData);

            // calculate the total length of data written
            var writtenDataLength = (long)fileData.Length;

            return writtenDataLength;
        }

        /// <summary>
        /// Writes a central directory header into a zip using the writer starting at the writer.BaseStream.Position.
        /// </summary>
        /// <param name="writer">BinaryWriter to be used to write file.</param>
        /// <param name="fileData">Byte[] of the file to be written into the zip.</param>
        /// <param name="crc32">CRC-32 checksum for the file.</param>
        /// <param name="dosDateTime">Last modified DateTime for the file data.</param>
        /// <param name="fileOffset">Offset, in bytes, for the local file header of the corresponding file from the start of the archive.</param>
        /// <returns>Number of total bytes written into the zip.</returns>
        private static long WriteCentralDirectoryHeaderIntoZip(
            BinaryWriter writer,
            byte[] fileData,
            uint crc32,
            uint dosDateTime,
            long fileOffset)
        {
            // Write the file header signature
            writer.Write(CentralDirectoryHeader.Signature);

            // Version made by:  2.0
            writer.Write((ushort)20);

            // Version needed to extract:  2.0
            writer.Write((ushort)20);

            // General purpose bit flags
            writer.Write((ushort)0);

            // The file is stored (no compression)
            writer.Write((ushort)0);

            // write date and time
            writer.Write(dosDateTime);

            // write file CRC32
            writer.Write(crc32);

            // write uncompressed size
            writer.Write((uint)fileData.Length);

            // write compressed size - same as uncompressed since file should have no compression
            writer.Write((uint)fileData.Length);

            // write file name length
            var fileNameBytes = Encoding.ASCII.GetBytes(_signingSpecification.SignaturePath);
            var fileNameLength = fileNameBytes.Length;
            writer.Write((ushort)fileNameLength);

            // write extra field length
            writer.Write((ushort)0);

            // write file comment length
            writer.Write((ushort)0);

            // write disk number start
            writer.Write((ushort)0);

            // write internal file attributes
            writer.Write((ushort)0);

            // write external file attributes
            writer.Write((uint)0);

            // write relative offset of local header
            writer.Write((uint)fileOffset);

            // write file name
            writer.Write(fileNameBytes);

            // calculate the total length of data written
            var writtenDataLength = CentralDirectoryHeader.SizeInBytesOfFixedLengthFields + fileNameLength;

            return writtenDataLength;
        }

        /// <summary>
        /// Writes the end of central directory header into a zip using the writer starting at the writer.BaseStream.Position.
        /// The new end of central directory record will be based on the one at reader.BaseStream.Position.
        /// </summary>
        /// <param name="reader">BinaryReader to be used to read the existing end of central directory record.</param>
        /// <param name="writer">BinaryWriter to be used to write the updated end of central directory record.</param>
        /// <param name="entryCountChange">The change to central directory header counts.</param>
        /// <param name="sizeOfSignatureCentralDirectoryRecord">Size of the central directory header for the signature file.</param>
        /// <param name="sizeOfSignatureFileHeaderAndData">Size of the signature file and the corresponding local file header.</param>
        private static void ReadAndWriteUpdatedEndOfCentralDirectoryRecordIntoZip(
            BinaryReader reader,
            BinaryWriter writer,
            sbyte entryCountChange,
            long sizeOfSignatureCentralDirectoryRecord,
            long sizeOfSignatureFileHeaderAndData)
        {
            // 4 bytes for disk numbers. same as before.
            ReadAndWriteUntilPosition(reader, writer, reader.BaseStream.Position + 8L);

            // Update central directory header counts by adding 1 for the signature entry
            var centralDirectoryCountOnThisDisk = reader.ReadUInt16();
            writer.Write((ushort)(centralDirectoryCountOnThisDisk + entryCountChange));

            var centralDirectoryCountTotal = reader.ReadUInt16();
            writer.Write((ushort)(centralDirectoryCountTotal + entryCountChange));

            // Update size of central directory by adding size of signature central directory size
            var sizeOfCentralDirectory = reader.ReadUInt32();
            writer.Write((uint)(sizeOfCentralDirectory + sizeOfSignatureCentralDirectoryRecord));

            // update the offset of central directory by adding the size of the signature local file header and data
            var offsetOfCentralDirectory = reader.ReadUInt32();
            writer.Write((uint)(offsetOfCentralDirectory + sizeOfSignatureFileHeaderAndData));

            // read and write the rest of the data
            ReadAndWriteUntilPosition(reader, writer, reader.BaseStream.Length);
        }

        private static bool CurrentStreamPositionMatchesByteSignature(BinaryReader reader, byte[] byteSignature)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (byteSignature == null || byteSignature.Length == 0)
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(byteSignature));
            }

            var stream = reader.BaseStream;

            if (stream.Length < byteSignature.Length)
            {
                return false;
            }

            var startingOffset = stream.Position;

            for (var i = 0; i < byteSignature.Length; ++i)
            {
                var b = stream.ReadByte();

                if (b != byteSignature[i])
                {
                    stream.Seek(offset: startingOffset, origin: SeekOrigin.Begin);
                    return false;
                }
            }

            stream.Seek(offset: startingOffset, origin: SeekOrigin.Begin);

            return true;
        }

        /// <summary>
        /// Converts a DateTime value into a unit in the MS-DOS date time format.
        /// Reference - https://docs.microsoft.com/en-us/cpp/c-runtime-library/32-bit-windows-time-date-formats
        /// Reference - https://source.dot.net/#System.IO.Compression/System/IO/Compression/ZipHelper.cs,91
        /// </summary>
        /// <param name="dateTime">DateTime value to be converted.</param>
        /// <returns>uint representing the MS-DOS equivalent date time.</returns>
        private static uint DateTimeToDosTime(DateTime dateTime)
        {
            // DateTime must be Convertible to DosTime
            Debug.Assert(ValidZipDate_YearMin <= dateTime.Year && dateTime.Year <= ValidZipDate_YearMax);

            var ret = ((dateTime.Year - ValidZipDate_YearMin) & 0x7F);
            ret = (ret << 4) + dateTime.Month;
            ret = (ret << 5) + dateTime.Day;
            ret = (ret << 5) + dateTime.Hour;
            ret = (ret << 6) + dateTime.Minute;
            ret = (ret << 5) + (dateTime.Second / 2); // only 5 bits for second, so we only have a granularity of 2 sec.
            return (uint)ret;
        }
    }
}
