// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;

namespace Microsoft.DotNet.StrongName
{
    internal static class Utils
    {
        // From StrongNameInternal.cpp
        // Checks to see if a public key is a valid instance of a PublicKeyBlob as
        // defined in StongName.h
        internal static bool IsValidPublicKey(ImmutableArray<byte> blob)
        {
            // The number of public key bytes must be at least large enough for the header and one byte of data.
            if (blob.IsDefault || blob.Length < Constants.SnPublicKeyHeaderSize + 1)
            {
                return false;
            }

            var blobStream = new MemoryStream(blob.ToArray());
            var blobReader = new BinaryReader(blobStream);

            // Signature algorithm ID
            var sigAlgId = blobReader.ReadUInt32();
            // Hash algorithm ID
            var hashAlgId = blobReader.ReadUInt32();
            // Size of public key data in bytes, not including the header
            var publicKeySize = blobReader.ReadUInt32();
            // publicKeySize bytes of public key data
            var publicKey = blobReader.ReadByte();

            // The number of public key bytes must be the same as the size of the header plus the size of the public key data.
            if (blob.Length != Constants.SnPublicKeyHeaderSize + publicKeySize)
            {
                return false;
            }

            // Check for the ECMA neutral public key, which does not obey the invariants checked below.
            if (blob.SequenceEqual(Constants.NeutralPublicKey))
            {
                return true;
            }

            // The public key must be in the wincrypto PUBLICKEYBLOB format
            if (publicKey != Constants.PublicKeyBlobId)
            {
                return false;
            }

            var signatureAlgorithmId = new Algorithm.AlgorithmId(sigAlgId);
            if (signatureAlgorithmId.IsSet && signatureAlgorithmId.Class != Algorithm.AlgorithmClass.Signature)
            {
                return false;
            }

            var hashAlgorithmId = new Algorithm.AlgorithmId(hashAlgId);
            if (hashAlgorithmId.IsSet && (hashAlgorithmId.Class != Algorithm.AlgorithmClass.Hash || hashAlgorithmId.SubId < Algorithm.AlgorithmSubId.Sha1Hash))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Prepare a PE buffer for hashing by zeroing out the checksum and authenticode signature, and
        /// potentially setting the strong name bit.
        /// </summary>
        /// <param name="peBuffer">PE buffer</param>
        /// <param name="peHeaders">Headers</param>
        /// <param name="setStrongNameBit">If true, strong name bit is set.</param>
        internal static void PreparePEForHashing(byte[] peBuffer, PEHeaders peHeaders, bool setStrongNameBit)
        {
            bool is32bit = peHeaders.PEHeader.Magic == PEMagic.PE32;

            // Zero the checksum
            peBuffer.SetBytes(peHeaders.PEHeaderStartOffset + Constants.ChecksumOffsetInPEHeader, Constants.CheckSumSize, 0);

            // Zero the authenticode signature
            int authenticodeOffset = GetAuthenticodeOffset(peHeaders, is32bit);
            var authenticodeDir = peHeaders.PEHeader.CertificateTableDirectory;
            peBuffer.SetBytes(authenticodeOffset, Constants.AuthenticodeDirectorySize, 0);

            if (setStrongNameBit)
            {
                var flagBytes = BitConverter.GetBytes((uint)(peHeaders.CorHeader.Flags | CorFlags.StrongNameSigned));
                peBuffer.SetBytes(peHeaders.CorHeaderStartOffset + Constants.FlagsOffsetInCorHeader, flagBytes);
            }
        }

        /// <summary>
        /// Sets the bytes in the buffer starting at <paramref name="index"/> to the bytes in <paramref name="value"/>
        /// </summary>
        /// <param name="buffer">Buffer to alter</param>
        /// <param name="index">Starting index</param>
        /// <param name="value">Value</param>
        internal static void SetBytes(this byte[] buffer, int index, byte[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                buffer[index + i] = value[i];
            }
        }

        internal static byte[] ReadPEToBuffer(Stream peStream)
        {
            byte[] peImage = new byte[checked((int)peStream.Length)];

            peStream.Position = 0;
            if (peStream.Read(peImage, 0, peImage.Length) != peImage.Length)
            {
                throw new InvalidOperationException("Failed to read the full PE file.");
            }

            return peImage;
        }

        internal static uint CalculateChecksum(byte[] peImage, PEHeaders peHeaders)
        {
            return CalculateChecksum(GetContentWithoutChecksum(peImage, peHeaders)) + (uint)peImage.Length;
        }

        internal static byte[] ComputeSigningHash(
                    byte[] peImage,
                    PEHeaders peHeaders,
                    int strongNameOffset,
                    int strongNameSize)
        {
            int peHeadersSize = peHeadersSize = peHeaders.PEHeaderStartOffset
                + (peHeaders.PEHeader.Magic == PEMagic.PE32 ? Constants.PEHeaderSize32Bit : Constants.PEHeaderSize64Bit)
                + Constants.PESectionHeaderSize * peHeaders.SectionHeaders.Length;

            // CodeQL [SM02196] ECMA-335 requires us to support SHA-1 and this is testing that support
            using (var hash = IncrementalHash.CreateHash(HashAlgorithmName.SHA1))
            {
                // First hash the DOS header and PE headers
                hash.AppendData(peImage, 0, peHeadersSize);

                // Now each section, skipping the strong name signature if present
                foreach (var sectionHeader in peHeaders.SectionHeaders)
                {
                    int sectionOffset = sectionHeader.PointerToRawData;
                    int sectionSize = sectionHeader.SizeOfRawData;

                    if ((strongNameOffset + strongNameSize) < sectionOffset ||
                        strongNameOffset >= (sectionOffset + sectionSize))
                    {
                        // No signature overlap, hash the whole section
                        hash.AppendData(peImage, sectionOffset, sectionSize);
                    }
                    else
                    {
                        // There is overlap. Hash either side of signature
                        hash.AppendData(peImage, sectionOffset, strongNameOffset - sectionOffset);
                        var strongNameEndOffset = strongNameOffset + strongNameSize;
                        hash.AppendData(peImage, strongNameEndOffset, sectionSize - (strongNameEndOffset - sectionOffset));
                    }
                }

                return hash.GetHashAndReset();
            }
        }

        /// <summary>
        /// Helper for RsaCryptoServiceProvider.ExportParameters()
        /// Adapted from https://github.com/dotnet/roslyn/blob/2f0aa43abd019143ae4662b5ccca11d4d666a61f/src/Compilers/Core/Portable/StrongName/CryptoBlobParser.cs#L257
        /// </summary>
        internal static RSAParameters ToRSAParameters(this ImmutableArray<byte> cspBlob, bool includePrivateParameters)
        {
            MemoryStream stream = new MemoryStream(cspBlob.ToArray());
            var br = new BinaryReader(stream);

            byte bType = br.ReadByte();    // BLOBHEADER.bType: Expected to be 0x6 (PUBLICKEYBLOB) or 0x7 (PRIVATEKEYBLOB), though there's no check for backward compat reasons. 
            byte bVersion = br.ReadByte(); // BLOBHEADER.bVersion: Expected to be 0x2, though there's no check for backward compat reasons.
            br.ReadUInt16();               // BLOBHEADER.wReserved
            int algId = br.ReadInt32();    // BLOBHEADER.aiKeyAlg

            int magic = br.ReadInt32();    // RSAPubKey.magic: Expected to be 0x31415352 ('RSA1') or 0x32415352 ('RSA2') 
            int bitLen = br.ReadInt32();   // RSAPubKey.bitLen

            int modulusLength = bitLen / 8;
            int halfModulusLength = (modulusLength + 1) / 2;

            uint expAsDword = br.ReadUInt32();

            RSAParameters rsaParameters = new RSAParameters();
            rsaParameters.Exponent = ExponentAsBytes(expAsDword);
            rsaParameters.Modulus = br.ReadReversed(modulusLength);

            if (includePrivateParameters)
            {
                rsaParameters.P = br.ReadReversed(halfModulusLength);
                rsaParameters.Q = br.ReadReversed(halfModulusLength);
                rsaParameters.DP = br.ReadReversed(halfModulusLength);
                rsaParameters.DQ = br.ReadReversed(halfModulusLength);
                rsaParameters.InverseQ = br.ReadReversed(halfModulusLength);
                rsaParameters.D = br.ReadReversed(modulusLength);
            }

            return rsaParameters;
        }

        /// <summary>
        /// Gets the public key blob from the assembly definition.
        /// </summary>
        /// <param name="metadataReader">Metadata reader</param>
        /// <returns>Public key blob</returns>
        internal static ImmutableArray<byte> GetPublicKeyBlob(this MetadataReader metadataReader)
        {
            var publicKey = metadataReader.GetAssemblyDefinition().PublicKey;
            if (publicKey.IsNil)
            {
                return ImmutableArray<byte>.Empty;
            }

            return metadataReader.GetBlobContent(publicKey);
        }

        /// <summary>
        /// Sets <paramref name="count"/> bytes starting at <paramref name="index"/> in buffer to value
        /// </summary>
        /// <param name="buffer">Buffer to alter</param>
        /// <param name="index">Start index</param>
        /// <param name="count">count</param>
        /// <param name="value">Value to set</param>
        private static void SetBytes(this byte[] buffer, int index, int count, byte value)
        {
            for (int i = 0; i < count; i++)
            {
                buffer[index + i] = value;
            }
        }

        private static int GetAuthenticodeOffset(PEHeaders peHeaders, bool is32bit)
        {
            return peHeaders.PEHeaderStartOffset
                + Constants.ChecksumOffsetInPEHeader
                + sizeof(int)                                  // Checksum
                + sizeof(short)                                // Subsystem
                + sizeof(short)                                // DllCharacteristics
                + 4 * (is32bit ? sizeof(int) : sizeof(long))   // SizeOfStackReserve, SizeOfStackCommit, SizeOfHeapReserve, SizeOfHeapCommit
                + sizeof(int)                                  // LoaderFlags
                + sizeof(int)                                  // NumberOfRvaAndSizes
                + 4 * sizeof(long);                            // directory entries before Authenticode
        }

        private static IEnumerable<Blob> GetContentWithoutChecksum(byte[] peImage, PEHeaders peHeaders)
        {
            BlobBuilder imageWithoutChecksum = new BlobBuilder();
            int checksumStart = peHeaders.PEHeaderStartOffset + Constants.ChecksumOffsetInPEHeader;
            int checksumEnd = checksumStart + Constants.CheckSumSize;
            // Content up to the checksum
            imageWithoutChecksum.WriteBytes(peImage, 0, checksumStart);
            // Content after the checksum
            imageWithoutChecksum.WriteBytes(peImage, checksumEnd, peImage.Length - checksumEnd);
            return imageWithoutChecksum.GetBlobs();
        }

        private static uint AggregateChecksum(uint checksum, ushort value)
        {
            uint sum = checksum + value;
            return (sum >> 16) + unchecked((ushort)sum);
        }

        private static uint CalculateChecksum(IEnumerable<Blob> blobs)
        {
            uint checksum = 0;
            int pendingByte = -1;

            // Iterates over the blobs in the PE image.
            // For each pair of bytes in the blob, compute an aggregate checksum
            // If the blob has an odd number of bytes, save the value of the last byte
            // and pair it with the first byte of the next blob. If there is no next blob, aggregate it.
            foreach (var blob in blobs)
            {
                var segment = blob.GetBytes().ToList();
                Debug.Assert(segment.Count > 0);

                int currIndex = 0;
                int count = segment.Count;
                if (pendingByte >= 0)
                {
                    checksum = AggregateChecksum(checksum, (ushort)(segment[currIndex] << 8 | pendingByte));
                    currIndex++;
                }

                if ((count - currIndex) % 2 != 0)
                {
                    // Save last byte for later
                    pendingByte = segment[count - 1];
                    count--;
                }
                else
                {
                    pendingByte = -1;
                }

                while (currIndex < count)
                {
                    checksum = AggregateChecksum(checksum, (ushort)(segment[currIndex + 1] << 8 | segment[currIndex]));
                    currIndex += 2;
                }
            }

            if (pendingByte >= 0)
            {
                checksum = AggregateChecksum(checksum, (ushort)pendingByte);
            }

            return checksum;
        }

        /// <summary>
        /// Convert a UInt32 into the minimal number of bytes (in big-endian order) to represent the value.
        /// Copied from https://github.com/dotnet/corefx/blob/5fe5f9aae7b2987adc7082f90712b265bee5eefc/src/System.Security.Cryptography.Csp/src/System/Security/Cryptography/CapiHelper.Shared.cs
        /// </summary>
        private static byte[] ExponentAsBytes(uint exponent)
        {
            if (exponent <= 0xFF)
            {
                return new[] { (byte)exponent };
            }
            else if (exponent <= 0xFFFF)
            {
                unchecked
                {
                    return new[]
                    {
                        (byte)(exponent >> 8),
                        (byte)(exponent)
                    };
                }
            }
            else if (exponent <= 0xFFFFFF)
            {
                unchecked
                {
                    return new[]
                    {
                        (byte)(exponent >> 16),
                        (byte)(exponent >> 8),
                        (byte)(exponent)
                    };
                }
            }
            else
            {
                return new[]
                {
                    (byte)(exponent >> 24),
                    (byte)(exponent >> 16),
                    (byte)(exponent >> 8),
                    (byte)(exponent)
                };
            }
        }

        private static byte[] ReadReversed(this BinaryReader reader, int count)
        {
            byte[] data = reader.ReadBytes(count);
            Array.Reverse(data);
            return data;
        }
    }
}