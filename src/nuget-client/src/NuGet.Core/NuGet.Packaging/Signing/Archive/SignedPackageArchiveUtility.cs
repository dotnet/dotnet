// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

// Zip Spec here: http://www.pkware.com/documents/casestudies/APPNOTE.TXT

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
#if IS_SIGNING_SUPPORTED
using System.Security.Cryptography.Pkcs;
#endif
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;

namespace NuGet.Packaging.Signing
{
    public static class SignedPackageArchiveUtility
    {
        private static readonly SigningSpecifications _signingSpecification = SigningSpecifications.V1;

        /// <summary>
        /// Utility method to know if a zip archive is signed.
        /// </summary>
        /// <param name="reader">Binary reader pointing to a zip archive.</param>
        /// <returns>true if the given archive is signed</returns>
        public static bool IsSigned(BinaryReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            try
            {
                var endOfCentralDirectoryRecord = EndOfCentralDirectoryRecord.Read(reader);

                // Look for signature central directory record
                reader.BaseStream.Seek(endOfCentralDirectoryRecord.OffsetOfStartOfCentralDirectory, SeekOrigin.Begin);
                CentralDirectoryHeader centralDirectoryHeader;

                while (CentralDirectoryHeader.TryRead(reader, out centralDirectoryHeader))
                {
                    if (IsPackageSignatureFileEntry(
                        centralDirectoryHeader.FileName,
                        centralDirectoryHeader.GeneralPurposeBitFlag))
                    {
                        // Go to local file header
                        reader.BaseStream.Seek(centralDirectoryHeader.RelativeOffsetOfLocalHeader, SeekOrigin.Begin);

                        // Make sure local file header exists
                        LocalFileHeader localFileHeader;
                        if (!LocalFileHeader.TryRead(reader, out localFileHeader))
                        {
                            throw new InvalidDataException(Strings.ErrorInvalidPackageArchive);
                        }

                        return IsPackageSignatureFileEntry(
                            localFileHeader.FileName,
                            localFileHeader.GeneralPurposeBitFlag);
                    }
                }
            }
            // Ignore any exception. If something is thrown it means the archive is either not valid or not signed
            catch { }

            return false;
        }

        /// <summary>
        /// Opens a read-only stream for the package signature file.
        /// </summary>
        /// <remarks>Callers should first verify that a package is signed before calling this method.</remarks>
        /// <param name="reader">A binary reader for a signed package.</param>
        /// <returns>A readable stream.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="reader" /> is <see langword="null" />.</exception>
        /// <exception cref="SignatureException">Thrown if a package signature file is invalid or missing.</exception>
        public static Stream OpenPackageSignatureFileStream(BinaryReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            var metadata = SignedPackageArchiveIOUtility.ReadSignedArchiveMetadata(reader);
            var signatureCentralDirectoryHeader = metadata.GetPackageSignatureFileCentralDirectoryHeaderMetadata();

            return GetPackageSignatureFile(reader, signatureCentralDirectoryHeader);
        }

        private static Stream GetPackageSignatureFile(
            BinaryReader reader,
            CentralDirectoryHeaderMetadata signatureCentralDirectoryHeader)
        {
            var localFileHeader = ReadPackageSignatureFileLocalFileHeader(reader, signatureCentralDirectoryHeader);
            var offsetToData = signatureCentralDirectoryHeader.OffsetToLocalFileHeader +
                LocalFileHeader.SizeInBytesOfFixedLengthFields +
                localFileHeader.FileNameLength +
                localFileHeader.ExtraFieldLength;

            var buffer = new byte[localFileHeader.UncompressedSize];

            reader.BaseStream.Seek(offsetToData, SeekOrigin.Begin);
            reader.BaseStream.Read(buffer, offset: 0, count: buffer.Length);

            return new MemoryStream(buffer, writable: false);
        }

        private static LocalFileHeader ReadPackageSignatureFileLocalFileHeader(
            BinaryReader reader,
            CentralDirectoryHeaderMetadata signatureCentralDirectoryHeader)
        {
            reader.BaseStream.Seek(signatureCentralDirectoryHeader.OffsetToLocalFileHeader, SeekOrigin.Begin);

            LocalFileHeader header;

            if (!LocalFileHeader.TryRead(reader, out header))
            {
                throw new SignatureException(NuGetLogCode.NU3005, Strings.InvalidPackageSignatureFile);
            }

            return header;
        }

        internal static bool IsPackageSignatureFileEntry(byte[] fileName, ushort generalPurposeBitFlag)
        {
            if (fileName == null || IsUtf8(generalPurposeBitFlag))
            {
                return false;
            }

            var expectedFileName = Encoding.ASCII.GetBytes(_signingSpecification.SignaturePath);

            // The ZIP format specification says the code page should be IBM code page 437 (CP437)
            // if bit 11 of the general purpose bit flag field is not set.  CP437 is not the same
            // as ASCII, but there is overlap.
            // All characters in the package signature file name are in that overlap, so we can
            // use the ASCII decoder instead of pulling in a new package for full CP437 support.
            return fileName.SequenceEqual(expectedFileName);
        }

        public static bool IsZip64(BinaryReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            var endOfCentralDirectoryRecord = EndOfCentralDirectoryRecord.Read(reader);

            if (endOfCentralDirectoryRecord.NumberOfThisDisk != endOfCentralDirectoryRecord.NumberOfTheDiskWithTheStartOfTheCentralDirectory)
            {
                return false;
            }

            var offset = endOfCentralDirectoryRecord.OffsetFromStart - Zip64EndOfCentralDirectoryLocator.SizeInBytes;

            if (offset >= 0)
            {
                reader.BaseStream.Seek(offset, SeekOrigin.Begin);

                if (Zip64EndOfCentralDirectoryLocator.Exists(reader))
                {
                    return true;
                }
            }

            reader.BaseStream.Seek(endOfCentralDirectoryRecord.OffsetOfStartOfCentralDirectory, SeekOrigin.Begin);

            CentralDirectoryHeader centralDirectoryHeader;

            while (CentralDirectoryHeader.TryRead(reader, out centralDirectoryHeader))
            {
                if (HasZip64ExtendedInformationExtraField(centralDirectoryHeader))
                {
                    return true;
                }

                if (centralDirectoryHeader.DiskNumberStart != endOfCentralDirectoryRecord.NumberOfThisDisk)
                {
                    continue;
                }

                var savedPosition = reader.BaseStream.Position;

                reader.BaseStream.Position = centralDirectoryHeader.RelativeOffsetOfLocalHeader;

                LocalFileHeader localFileHeader;

                if (LocalFileHeader.TryRead(reader, out localFileHeader) &&
                    HasZip64ExtendedInformationExtraField(localFileHeader))
                {
                    return true;
                }

                reader.BaseStream.Position = savedPosition;
            }

            return false;
        }

        private static bool HasZip64ExtendedInformationExtraField(CentralDirectoryHeader header)
        {
            IReadOnlyList<ExtraField> extraFields;

            if (ExtraField.TryRead(header, out extraFields))
            {
                return extraFields.Any(extraField => extraField is Zip64ExtendedInformationExtraField);
            }

            return false;
        }

        private static bool HasZip64ExtendedInformationExtraField(LocalFileHeader header)
        {
            IReadOnlyList<ExtraField> extraFields;

            if (ExtraField.TryRead(header, out extraFields))
            {
                return extraFields.Any(extraField => extraField is Zip64ExtendedInformationExtraField);
            }

            return false;
        }

#if IS_SIGNING_SUPPORTED
        /// <summary>
        /// Removes repository primary signature (if it exists) or any repository countersignature (if it exists).
        /// </summary>
        /// <param name="input">A readable stream for a signed package.</param>
        /// <param name="output">A read/write stream for receiving an updated package.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A flag indicating whether or not a signature was removed.</returns>
        public static async Task<bool> RemoveRepositorySignaturesAsync(
            Stream input,
            Stream output,
            CancellationToken cancellationToken)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            cancellationToken.ThrowIfCancellationRequested();

            PrimarySignature primarySignature;

            using (var packageReader = new PackageArchiveReader(input, leaveStreamOpen: true))
            {
                primarySignature = await packageReader.GetPrimarySignatureAsync(cancellationToken);
            }

            if (primarySignature == null)
            {
                return false;
            }

            switch (primarySignature.Type)
            {
                case SignatureType.Repository:
                    await RemoveRepositoryPrimarySignatureAsync(input, output, cancellationToken);

                    return true;

                default:
                    return await RemoveRepositoryCountersignaturesAsync(
                        input,
                        output,
                        primarySignature.SignedCms,
                        cancellationToken);
            }
        }

        private static Task RemoveRepositoryPrimarySignatureAsync(
            Stream input,
            Stream output,
            CancellationToken cancellationToken)
        {
            using (var package = new SignedPackageArchive(input, output))
            {
                return package.RemoveSignatureAsync(cancellationToken);
            }
        }

        private static async Task<bool> RemoveRepositoryCountersignaturesAsync(
            Stream input,
            Stream output,
            SignedCms signedCms,
            CancellationToken cancellationToken)
        {
            if (TryRemoveRepositoryCountersignatures(signedCms, out var updatedSignedCms))
            {
                var primarySignature = PrimarySignature.Load(updatedSignedCms.Encode());

                using (var unsignedPackage = new MemoryStream())
                {
                    using (var package = new SignedPackageArchive(input, unsignedPackage))
                    {
                        await package.RemoveSignatureAsync(cancellationToken);
                    }

                    using (var package = new SignedPackageArchive(unsignedPackage, output))
                    using (var signatureStream = new MemoryStream(primarySignature.GetBytes()))
                    {
                        await package.AddSignatureAsync(signatureStream, cancellationToken);
                    }
                }

                return true;
            }

            return false;
        }

        private static bool TryRemoveRepositoryCountersignatures(SignedCms signedCms, out SignedCms updatedSignedCms)
        {
            updatedSignedCms = null;

            // SignerInfo.CouterSignerInfos returns a mutable copy of countersigners.  This copy does not reflect
            // the removal of countersigners via SignerInfo.RemoveCounterSignature(...).
            // Also, SignerInfo.UnsignedAttributes is defined as an ASN.1 SET, which is an unordered collection.
            // The underlying platform may reorder elements when modifying the collection, so obtaining updated
            // indicies is necessary after removing an attribute.
            var tempSignedCms = new SignedCms();

            tempSignedCms = Reencode(signedCms);

            while (true)
            {
                var repositoryCountersignatureFound = false;
                var primarySigner = tempSignedCms.SignerInfos[0];
                var countersigners = primarySigner.CounterSignerInfos;

                for (var i = 0; i < countersigners.Count; ++i)
                {
                    var countersigner = countersigners[i];

                    if (AttributeUtility.GetSignatureType(countersigner.SignedAttributes) == SignatureType.Repository)
                    {
                        repositoryCountersignatureFound = true;

                        primarySigner.RemoveCounterSignature(i);

                        // This is a workaround to SignerInfo.CounterSignerInfos not reflecting changes in the signed CMS.
                        tempSignedCms = Reencode(tempSignedCms);
                        updatedSignedCms = tempSignedCms;

                        // Indices of other countersignatures may have changed unexpectedly as a result
                        // of removing a countersignature.
                        break;
                    }
                }

                if (!repositoryCountersignatureFound)
                {
                    break;
                }
            }

            return updatedSignedCms != null;
        }

        private static SignedCms Reencode(SignedCms signedCms)
        {
            var newSignedCms = new SignedCms();

            newSignedCms.Decode(signedCms.Encode());

            return newSignedCms;
        }

        /// <summary>
        /// Signs a Zip with the contents in the SignatureStream using the writer.
        /// The reader is used to read the exisiting contents for the Zip.
        /// </summary>
        /// <param name="signatureStream">MemoryStream of the signature to be inserted into the zip.</param>
        /// <param name="reader">BinaryReader to be used to read the existing zip data.</param>
        /// <param name="writer">BinaryWriter to be used to write the signature into the zip.</param>
        internal static void SignZip(MemoryStream signatureStream, BinaryReader reader, BinaryWriter writer)
        {
            SignedPackageArchiveIOUtility.WriteSignatureIntoZip(signatureStream, reader, writer);
        }

        internal static void UnsignZip(BinaryReader reader, BinaryWriter writer)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            SignedPackageArchiveIOUtility.RemoveSignature(reader, writer);
        }

        internal static void HashUInt16(HashAlgorithm hashAlgorithm, ushort value)
        {
            byte[] array = BitConverter.GetBytes(value);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(array);
            }
            SignedPackageArchiveIOUtility.HashBytes(hashAlgorithm, array);
        }

        internal static void HashUInt32(HashAlgorithm hashAlgorithm, uint value)
        {
            byte[] array = BitConverter.GetBytes(value);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(array);
            }
            SignedPackageArchiveIOUtility.HashBytes(hashAlgorithm, array);
        }

        /// <summary>
        /// Verifies that a signed package archive's signature is valid and it has not been tampered with.
        ///
        /// </summary>
        /// <param name="reader">Signed package to verify</param>
        /// <param name="hashAlgorithm">Hash algorithm to be used to hash data.</param>
        /// <param name="expectedHash">Hash value of the original data.</param>
        /// <returns>True if package archive's hash matches the expected hash</returns>
        internal static bool VerifySignedPackageIntegrity(BinaryReader reader, HashAlgorithm hashAlgorithm, byte[] expectedHash)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (hashAlgorithm == null)
            {
                throw new ArgumentNullException(nameof(hashAlgorithm));
            }

            if (expectedHash == null || expectedHash.Length == 0)
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(reader));
            }

            // Make sure it is signed with a valid signature file
            if (!IsSigned(reader))
            {
                throw new SignatureException(NuGetLogCode.NU3003, Strings.SignedPackageNotSignedOnVerify);
            }

            var metadata = SignedPackageArchiveIOUtility.ReadSignedArchiveMetadata(reader);
            var signatureCentralDirectoryHeader = metadata.GetPackageSignatureFileCentralDirectoryHeaderMetadata();
            var centralDirectoryRecordsWithoutSignature = RemoveSignatureAndOrderByOffset(metadata);

            try
            {
                // Read and hash from the start of the archive to the start of the file headers
                reader.BaseStream.Seek(offset: 0, origin: SeekOrigin.Begin);
                SignedPackageArchiveIOUtility.ReadAndHashUntilPosition(reader, hashAlgorithm, metadata.StartOfLocalFileHeaders);

                // Read and hash file headers
                foreach (var record in centralDirectoryRecordsWithoutSignature)
                {
                    reader.BaseStream.Seek(offset: record.OffsetToLocalFileHeader, origin: SeekOrigin.Begin);
                    SignedPackageArchiveIOUtility.ReadAndHashUntilPosition(reader, hashAlgorithm, record.OffsetToLocalFileHeader + record.FileEntryTotalSize);
                }

                // Order central directory records by their position
                centralDirectoryRecordsWithoutSignature.Sort((x, y) => x.Position.CompareTo(y.Position));

                // Update offset of any central directory record that has a file entry after signature
                foreach (var record in centralDirectoryRecordsWithoutSignature)
                {
                    reader.BaseStream.Seek(offset: record.Position, origin: SeekOrigin.Begin);
                    // Hash from the start of the central directory record until the relative offset of local file header (42 from the start of central directory record, including signature length)
                    SignedPackageArchiveIOUtility.ReadAndHashUntilPosition(reader, hashAlgorithm, reader.BaseStream.Position + 42);

                    var relativeOffsetOfLocalFileHeader = (uint)(reader.ReadUInt32() + record.ChangeInOffset);
                    HashUInt32(hashAlgorithm, relativeOffsetOfLocalFileHeader);

                    // Continue hashing file name, extra field, and file comment fields.
                    SignedPackageArchiveIOUtility.ReadAndHashUntilPosition(reader, hashAlgorithm, reader.BaseStream.Position + record.HeaderSize - CentralDirectoryHeader.SizeInBytesOfFixedLengthFields);
                }

                reader.BaseStream.Seek(offset: metadata.EndOfCentralDirectory, origin: SeekOrigin.Begin);

                // Hash until total entries in end of central directory record (8 bytes from the start of EOCDR)
                SignedPackageArchiveIOUtility.ReadAndHashUntilPosition(reader, hashAlgorithm, metadata.EndOfCentralDirectory + 8);

                var eocdrTotalEntries = (ushort)(reader.ReadUInt16() - 1);
                var eocdrTotalEntriesOnDisk = (ushort)(reader.ReadUInt16() - 1);

                HashUInt16(hashAlgorithm, eocdrTotalEntries);
                HashUInt16(hashAlgorithm, eocdrTotalEntriesOnDisk);

                // update the central directory size by substracting the size of the package signature file's central directory header
                var eocdrSizeOfCentralDirectory = (uint)(reader.ReadUInt32() - signatureCentralDirectoryHeader.HeaderSize);
                HashUInt32(hashAlgorithm, eocdrSizeOfCentralDirectory);

                var eocdrOffsetOfCentralDirectory = reader.ReadUInt32() - (uint)signatureCentralDirectoryHeader.FileEntryTotalSize;
                HashUInt32(hashAlgorithm, eocdrOffsetOfCentralDirectory);

                // Hash until the end of the reader
                SignedPackageArchiveIOUtility.ReadAndHashUntilPosition(reader, hashAlgorithm, reader.BaseStream.Length);

                hashAlgorithm.TransformFinalBlock(Array.Empty<byte>(), inputOffset: 0, inputCount: 0);

                return CompareHash(expectedHash, hashAlgorithm.Hash);
            }
            // If exception is throw in means the archive was not a valid package. It has been tampered, return false.
            catch { }

            return false;
        }
#else

        public static Task<bool> RemoveRepositorySignaturesAsync(
            Stream input,
            Stream output,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        internal static void SignZip(MemoryStream signatureStream, BinaryReader reader, BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        internal static void UnsignZip(BinaryReader reader, BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        internal static void VerifySignedZipIntegrity(BinaryReader reader, HashAlgorithm hashAlgorithm, byte[] expectedHash)
        {
            throw new NotImplementedException();
        }

#endif

        private static List<CentralDirectoryHeaderMetadata> RemoveSignatureAndOrderByOffset(SignedPackageArchiveMetadata metadata)
        {
            // Remove signature cdr
            var centralDirectoryRecordsList = metadata.CentralDirectoryHeaders.Where((v, i) => i != metadata.SignatureCentralDirectoryHeaderIndex).ToList();

            // Sort by order of file entries
            centralDirectoryRecordsList.Sort((x, y) => x.OffsetToLocalFileHeader.CompareTo(y.OffsetToLocalFileHeader));

            // Update offsets with removed signature
            var previousRecordFileEntryEnd = 0L;
            foreach (var centralDirectoryRecord in centralDirectoryRecordsList)
            {
                centralDirectoryRecord.ChangeInOffset = previousRecordFileEntryEnd - centralDirectoryRecord.OffsetToLocalFileHeader;

                previousRecordFileEntryEnd = centralDirectoryRecord.OffsetToLocalFileHeader + centralDirectoryRecord.FileEntryTotalSize + centralDirectoryRecord.ChangeInOffset;
            }

            return centralDirectoryRecordsList;
        }

        internal static void HashUInt16(Sha512HashFunction hashFunc, ushort value)
        {
            byte[] array = BitConverter.GetBytes(value);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(array);
            }
            SignedPackageArchiveIOUtility.HashBytes(hashFunc, array);
        }

        internal static void HashUInt32(Sha512HashFunction hashFunc, uint value)
        {
            byte[] array = BitConverter.GetBytes(value);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(array);
            }
            SignedPackageArchiveIOUtility.HashBytes(hashFunc, array);
        }

        internal static string GetPackageContentHash(BinaryReader reader)
        {
            using (var hashFunc = new Sha512HashFunction())
            {
                // skip validating signature entry since we're just trying to get the content hash here instead of
                // verifying signature entry.
                var metadata = SignedPackageArchiveIOUtility.ReadSignedArchiveMetadata(reader, validateSignatureEntry: false);
                var signatureCentralDirectoryHeader = metadata.GetPackageSignatureFileCentralDirectoryHeaderMetadata();
                var centralDirectoryRecordsWithoutSignature = RemoveSignatureAndOrderByOffset(metadata);

                // Read and hash from the start of the archive to the start of the file headers
                reader.BaseStream.Seek(offset: 0, origin: SeekOrigin.Begin);
                SignedPackageArchiveIOUtility.ReadAndHashUntilPosition(reader, hashFunc, metadata.StartOfLocalFileHeaders);

                // Read and hash file headers
                foreach (var record in centralDirectoryRecordsWithoutSignature)
                {
                    reader.BaseStream.Seek(offset: record.OffsetToLocalFileHeader, origin: SeekOrigin.Begin);
                    SignedPackageArchiveIOUtility.ReadAndHashUntilPosition(reader, hashFunc, record.OffsetToLocalFileHeader + record.FileEntryTotalSize);
                }

                // Order central directory records by their position
                centralDirectoryRecordsWithoutSignature.Sort((x, y) => x.Position.CompareTo(y.Position));

                // Update offset of any central directory record that has a file entry after signature
                foreach (var record in centralDirectoryRecordsWithoutSignature)
                {
                    reader.BaseStream.Seek(offset: record.Position, origin: SeekOrigin.Begin);
                    // Hash from the start of the central directory record until the relative offset of local file header (42 from the start of central directory record, including signature length)
                    SignedPackageArchiveIOUtility.ReadAndHashUntilPosition(reader, hashFunc, reader.BaseStream.Position + 42);

                    var relativeOffsetOfLocalFileHeader = (uint)(reader.ReadUInt32() + record.ChangeInOffset);
                    HashUInt32(hashFunc, relativeOffsetOfLocalFileHeader);

                    // Continue hashing file name, extra field, and file comment fields.
                    SignedPackageArchiveIOUtility.ReadAndHashUntilPosition(reader, hashFunc, reader.BaseStream.Position + record.HeaderSize - CentralDirectoryHeader.SizeInBytesOfFixedLengthFields);
                }

                reader.BaseStream.Seek(offset: metadata.EndOfCentralDirectory, origin: SeekOrigin.Begin);

                // Hash until total entries in end of central directory record (8 bytes from the start of EOCDR)
                SignedPackageArchiveIOUtility.ReadAndHashUntilPosition(reader, hashFunc, metadata.EndOfCentralDirectory + 8);

                var eocdrTotalEntries = (ushort)(reader.ReadUInt16() - 1);
                var eocdrTotalEntriesOnDisk = (ushort)(reader.ReadUInt16() - 1);

                HashUInt16(hashFunc, eocdrTotalEntries);
                HashUInt16(hashFunc, eocdrTotalEntriesOnDisk);

                // update the central directory size by substracting the size of the package signature file's central directory header
                var eocdrSizeOfCentralDirectory = (uint)(reader.ReadUInt32() - signatureCentralDirectoryHeader.HeaderSize);
                HashUInt32(hashFunc, eocdrSizeOfCentralDirectory);

                var eocdrOffsetOfCentralDirectory = reader.ReadUInt32() - (uint)signatureCentralDirectoryHeader.FileEntryTotalSize;
                HashUInt32(hashFunc, eocdrOffsetOfCentralDirectory);

                // Hash until the end of the reader
                SignedPackageArchiveIOUtility.ReadAndHashUntilPosition(reader, hashFunc, reader.BaseStream.Length);

                hashFunc.Update(Array.Empty<byte>(), offset: 0, count: 0);

                return hashFunc.GetHash();
            }
        }

        internal static bool IsUtf8(ushort generalPurposeBitFlags)
        {
            return (generalPurposeBitFlags & (1 << 11)) != 0;
        }

        private static bool CompareHash(byte[] expectedHash, byte[] actualHash)
        {
            if (expectedHash.Length != actualHash.Length)
            {
                return false;
            }

            for (var i = 0; i < expectedHash.Length; i++)
            {
                if (expectedHash[i] != actualHash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
