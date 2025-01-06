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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.SignTool
{
    public static class StrongName
    {
        // Checksum offset in the PE header.
        internal const int ChecksumOffsetInPEHeader = 0x40;
        internal const int CheckSumSize = sizeof(uint);

        // Internal constants obtained from runtime's
        // src/libraries/System.Reflection.Metadata/src/System/Reflection/PortableExecutable/PEHeader.cs
        private const int PEHeaderSize32Bit = 224;
        private const int PEHeaderSize64Bit = 240;
        private const int PESectionHeaderSize = 40;
        private const int AuthenticodeDirectorySize = 2 * sizeof(int);
        private const int SnPublicKeyHeaderSize = 12;

        private const int BlobHeaderSize = sizeof(byte) + sizeof(byte) + sizeof(ushort) + sizeof(uint);
        private const int RsaPubKeySize = sizeof(uint) + sizeof(uint) + sizeof(uint);

        private const UInt32 RSA1 = 0x31415352;
        private const UInt32 RSA2 = 0x32415352;

        // In wincrypt.h both public and private key blobs start with a
        // PUBLICKEYSTRUC and RSAPUBKEY and then start the key data
        private const int OffsetToKeyData = BlobHeaderSize + RsaPubKeySize;

        // From wincrypt.h
        private const byte PublicKeyBlobId = 0x06;
        private const byte PrivateKeyBlobId = 0x07;

        // from winnt.h
        private const int FlagsOffsetInCorHeader = sizeof(uint) + // cb
                                                   sizeof(ushort) + // MajorRuntimeVersion
                                                   sizeof(ushort) + // MinorRuntimeVersion
                                                   sizeof(uint) * 2; // MetaData
        internal const int CorFlagsSize = sizeof(uint);

        /// <summary>
        /// Neutral public key indicates that the ECMA key was used to strong name the binary.
        /// </summary>
        static readonly byte[] NeutralPublicKey = { 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0 };

        // ECMA key
        static readonly byte[] ECMAKey =
        {
            0x00,0x24,0x00,0x00,0x04,0x80,0x00,0x00,0x94,0x00,0x00,0x00,0x06,0x02,0x00,0x00,
            0x00,0x24,0x00,0x00,0x52,0x53,0x41,0x31,0x00,0x04,0x00,0x00,0x01,0x00,0x01,0x00,
            0x07,0xd1,0xfa,0x57,0xc4,0xae,0xd9,0xf0,0xa3,0x2e,0x84,0xaa,0x0f,0xae,0xfd,0x0d,
            0xe9,0xe8,0xfd,0x6a,0xec,0x8f,0x87,0xfb,0x03,0x76,0x6c,0x83,0x4c,0x99,0x92,0x1e,
            0xb2,0x3b,0xe7,0x9a,0xd9,0xd5,0xdc,0xc1,0xdd,0x9a,0xd2,0x36,0x13,0x21,0x02,0x90,
            0x0b,0x72,0x3c,0xf9,0x80,0x95,0x7f,0xc4,0xe1,0x77,0x10,0x8f,0xc6,0x07,0x77,0x4f,
            0x29,0xe8,0x32,0x0e,0x92,0xea,0x05,0xec,0xe4,0xe8,0x21,0xc0,0xa5,0xef,0xe8,0xf1,
            0x64,0x5c,0x4c,0x0c,0x93,0xc1,0xab,0x99,0x28,0x5d,0x62,0x2c,0xaa,0x65,0x2c,0x1d,
            0xfa,0xd6,0x3d,0x74,0x5d,0x6f,0x2d,0xe5,0xf1,0x7e,0x5e,0xaf,0x0f,0xc4,0x96,0x3d,
            0x26,0x1c,0x8a,0x12,0x43,0x65,0x18,0x20,0x6d,0xc0,0x93,0x34,0x4d,0x5a,0xd2,0x93
        };

        /// <summary>
        /// Returns true if the file has a valid strong name signature.
        /// </summary>
        /// <param name="file">Path to file</param>
        /// <param name="log">MSBuild logger if desired</param>
        /// <param name="snPath">Path to sn.exe, if available and desired.</param>
        /// <returns>True if the file has a valid strong name signature, false otherwise.</returns>
        public static bool IsSigned(string file, string snPath = null, TaskLoggingHelper log = null)
        {
            try
            {
                using (var metadata = new FileStream(file, FileMode.Open))
                {
                    return IsSigned(metadata);
                }
            }
            catch (Exception e)
            {
                if (log != null)
                {
                    log.LogMessage(MessageImportance.High, $"Failed to determine whether PE file {file} has a valid strong name signature. {e}");
                }

                if (!string.IsNullOrEmpty(snPath))
                {
                    // Fall back to the old method of checking for a strong name signature, but only on Windows.
                    // Otherwise, return false:
                    return IsSigned_Legacy(file, snPath);
                }
            }

            return false;
        }

        // Internal for testing to avoid having to write a file to disk.
        internal static bool IsSigned(Stream peStream)
        {
            var peHeaders = new PEHeaders(peStream);
            peStream.Position = 0;
            using (PEReader peReader = new PEReader(peStream, PEStreamOptions.LeaveOpen))
            {
                if (!peReader.HasMetadata)
                {
                    return false;
                }
                // If the binary doesn't have metadata (e.g. crossgenned) then it's not signed.
                MetadataReader metadataReader = peReader.GetMetadataReader();

                var flags = peHeaders.CorHeader.Flags;

                // If the strong name bit isn't set, then it's not signed.
                if (CorFlags.StrongNameSigned != (flags & CorFlags.StrongNameSigned))
                {
                    return false;
                }

                // Reset position before creating the blob builder.
                peStream.Position = 0;
                byte[] peBuffer = ReadPEToBuffer(peStream);

                // Verify the checksum before verifying the strong name signature
                // PreparePEForHashing will zero out the checksum and authenticode signature.

                if (peHeaders.PEHeader.CheckSum != CalculateChecksum(peBuffer, peHeaders))
                {
                    return false;
                }

                // If the strong name signature data isn't present, then it's also not signed.
                var snDirectory = peReader.PEHeaders.CorHeader.StrongNameSignatureDirectory;
                if (!peHeaders.TryGetDirectoryOffset(snDirectory, out int snOffset))
                {
                    return false;
                }

                // Prepare the buffer for hashing. No need to set the strong name bit.
                PreparePEForHashing(peBuffer, peHeaders, setStrongNameBit: false);

                int snSize = snDirectory.Size;
                byte[] hash = ComputeSigningHash(peBuffer, peHeaders, snOffset, snSize);

                ImmutableArray<byte> publicKeyBlob = metadataReader.GetBlobContent(metadataReader.GetAssemblyDefinition().PublicKey);

                if (!IsValidPublicKey(publicKeyBlob))
                {
                    return false;
                }

                // It's possible that the public key blob is a neutral public key blob,
                // meaning that it's actually the ECMA key that was used to sign the assembly.
                // Verify against that.
                if (publicKeyBlob.SequenceEqual(NeutralPublicKey))
                {
                    publicKeyBlob = ECMAKey.ToImmutableArray();
                }

                using (RSA rsa = RSA.Create())
                {
                    // Ensure we skip over the sn key header.
                    rsa.ImportParameters(publicKeyBlob.Slice(RsaPubKeySize, publicKeyBlob.Length - RsaPubKeySize).ToRSAParameters(false));

                    var reversedSignature = peReader.GetSectionData(snDirectory.RelativeVirtualAddress).GetContent(0, snSize).ToArray();

                    // The signature bytes are in host (little-endian) byte order.
                    // Reverse the bytes to put them back into network byte order to verify the signature.
                    Array.Reverse(reversedSignature);

                    // CodeQL [SM02196] ECMA-335 requires us to support SHA-1 and this is testing that support
                    if (!rsa.VerifyHash(hash, reversedSignature, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Determine whether the file is strong named, using sn.exe instead
        /// of the custom implementation
        /// </summary>
        /// <param name="file">Path to file</param>
        /// <param name="snPath">sn.exe path</param>
        /// <returns>True if the file is strong named, false otherwise.</returns>
        internal static bool IsSigned_Legacy(string file, string snPath)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return false;
            }

            var process = Process.Start(new ProcessStartInfo()
            {
                FileName = snPath,
                Arguments = $@"-vf ""{file}"" > nul",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardError = false,
                RedirectStandardOutput = false
            });

            process.WaitForExit();

            return process.ExitCode == 0;
        }

        /// <summary>
        /// Unset the strong name signing bit from a file. This is required for sn
        /// </summary>
        /// <param name="file"></param>
        public static void ClearStrongNameSignedBit(string file)
        {
            using (var stream = new FileStream(file, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
            using (var peReader = new PEReader(stream))
            using (var writer = new BinaryWriter(stream))
            {
                if (!ContentUtil.IsPublicSigned(peReader))
                {
                    return;
                }

                stream.Position = peReader.PEHeaders.CorHeaderStartOffset + FlagsOffsetInCorHeader;
                writer.Write((UInt32)(peReader.PEHeaders.CorHeader.Flags & ~CorFlags.StrongNameSigned));
            }
        }

        /// <summary>
        /// Strong names an existing previously signed or delay-signed binary with keyfile.
        /// Fall back to legacy signing if available and new signing fails.
        /// </summary>
        /// <param name="file">Path to file to sign</param>
        /// <param name="keyFile">Path to key pair.</param>
        /// <param name="log">Optional logger</param>
        /// <param name="snPath">Optional path to sn.exe</param>
        /// <returns>True if the file was signed successfully, false otherwise</returns>
        public static bool Sign(string file, string keyFile, string snPath = null, TaskLoggingHelper log = null)
        {
            try
            {
                using (var metadata = new FileStream(file, FileMode.Open))
                {
                    Sign(metadata, keyFile);
                }
                return true;
            }
            catch (Exception e)
            {
                if (log != null)
                {
                    log.LogMessage(MessageImportance.High, $"Failed to sign PE file {file} with strong name {keyFile}: {e}");
                }

                if (!string.IsNullOrEmpty(snPath))
                {
                    // Fall back to the old method of checking for a strong name signature, but only on Windows.
                    // Otherwise, return false:
                    return Sign_Legacy(file, keyFile, snPath);
                }
            }

            return false;
        }

        internal static bool Sign_Legacy(string file, string keyfile,  string snPath)
        {
            // sn -R <path_to_file> <path_to_snk>
            var process = Process.Start(new ProcessStartInfo()
            {
                FileName = snPath,
                Arguments = $@"-R ""{file}"" ""{keyfile}""",
                UseShellExecute = false
            });

            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Given a key file, sets the strong name in the managed binary
        /// </summary>
        /// <param name="peStream"></param>
        /// <param name="keyFile"></param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="Exception"></exception>
        internal static void Sign(Stream peStream, string keyFile)
        {
            // This process happens as follows:
            // 1. Open the PE and read into a file.
            // 2. Compute the signing hash of the binary (excluding the strong name and authenticode signatures)
            // 3. Read the key data from the provided file
            // 4. Attempt to sign the hash using the crypto service provider.
            // 5. Write the signature into the strong name signature directory.
            // 6. Compute the checksum of the binary and write it back into the PE header.
            // 7. Write the binary back to the file.

            byte[] signature;
            byte[] peBuffer;
            int snSignatureOffset;
            var peHeaders = new PEHeaders(peStream);
            
            // Reset the stream position before loading the PE
            peStream.Position = 0;
            using (PEReader peReader = new PEReader(peStream, PEStreamOptions.LeaveOpen))
            {
                if (!peReader.HasMetadata)
                {
                    throw new InvalidOperationException("Cannot strong name sign binary without metadata.");
                }
                // If the binary doesn't have metadata (e.g. crossgenned) then it's not signed.
                MetadataReader metadataReader = peReader.GetMetadataReader();

                // Parse the SNK
                if (!TryParseKey(File.ReadAllBytes(keyFile).ToImmutableArray(), out ImmutableArray<byte> snkPublicKey, out RSAParameters? privateKey) ||
                    privateKey == null)
                {
                    throw new InvalidOperationException($"Failed to parse strong name '{keyFile}'. Key must be a full public/private keypair");
                }

                // If the strong name signature data isn't present, then we can't sign it.
                var snDirectory = peReader.PEHeaders.CorHeader.StrongNameSignatureDirectory;
                if (!peHeaders.TryGetDirectoryOffset(snDirectory, out snSignatureOffset))
                {
                    throw new InvalidOperationException("Strong name directory is not present. Binary is not signed or delay-signed.");
                }

                // Verify the public key of the assembly matches the private key we're using to sign.
                // We do not support signing with the ECMA key
                ImmutableArray<byte> publicKeyBlob = metadataReader.GetBlobContent(metadataReader.GetAssemblyDefinition().PublicKey);
                if (publicKeyBlob.SequenceEqual(NeutralPublicKey))
                {
                    throw new NotImplementedException("Cannot sign with the ECMA key.");
                }

                // Verify that the public key of the assembly matches the public key of the provided key file.
                if (!TryParseKey(publicKeyBlob, out ImmutableArray<byte> assemblyPublicKey, out _))
                {
                    throw new InvalidOperationException("Failed to parse the public key of the assembly.");
                }

                if (!assemblyPublicKey.SequenceEqual(snkPublicKey))
                {
                    throw new InvalidOperationException("Public key of the assembly does not match the public key of the provided key file.");
                }

                // Copy the PE into a buffer
                peStream.Position = 0;
                peBuffer = ReadPEToBuffer(peStream);

                // Now prepare that buffer for hashing
                PreparePEForHashing(peBuffer, peHeaders, setStrongNameBit: true);

                byte[] hash = ComputeSigningHash(peBuffer, peHeaders, snSignatureOffset, snDirectory.Size);
                using (RSA snkRSA = RSA.Create())
                {
                    snkRSA.ImportParameters(privateKey.Value);

                    // CodeQL [SM02196] ECMA-335 requires us to support SHA-1 and this is testing that support
                    signature = snkRSA.SignHash(hash, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);

                    // The signature is written in reverse order
                    Array.Reverse(signature);
                }

                // Write the signature into the strong name signature directory
                peBuffer.SetBytes(snSignatureOffset, signature);

                // Compute a new checksum and write it out.
                uint checksum = CalculateChecksum(peBuffer, peHeaders);
                var checksumBytes = BitConverter.GetBytes(checksum);
                peBuffer.SetBytes(peHeaders.PEHeaderStartOffset + ChecksumOffsetInPEHeader, checksumBytes);
            }

            // Write the PE stream back
            peStream.Position = 0;
            peStream.Write(peBuffer, 0, peBuffer.Length);
        }

        private static uint AggregateChecksum(uint checksum, ushort value)
        {
            uint sum = checksum + value;
            return (sum >> 16) + unchecked((ushort)sum);
        }

        private static uint CalculateChecksum(byte[] peImage, PEHeaders peHeaders)
        {
            return CalculateChecksum(GetContentWithoutChecksum(peImage, peHeaders)) + (uint)peImage.Length;
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

        private static byte[] ComputeSigningHash(
                    byte[] peImage,
                    PEHeaders peHeaders,
                    int strongNameOffset,
                    int strongNameSize)
        {
            int peHeadersSize = peHeadersSize = peHeaders.PEHeaderStartOffset
                + (peHeaders.PEHeader.Magic == PEMagic.PE32 ? PEHeaderSize32Bit : PEHeaderSize64Bit)
                + PESectionHeaderSize * peHeaders.SectionHeaders.Length;

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
        /// Prepare a PE buffer for hashing by zeroing out the checksum and authenticode signature, and
        /// potentially setting the strong name bit.
        /// </summary>
        /// <param name="peBuffer">PE buffer</param>
        /// <param name="peHeaders">Headers</param>
        /// <param name="setStrongNameBit">If true, strong name bit is set.</param>
        private static void PreparePEForHashing(byte[] peBuffer, PEHeaders peHeaders, bool setStrongNameBit)
        {
            bool is32bit = peHeaders.PEHeader.Magic == PEMagic.PE32;

            // Zero the checksum
            peBuffer.SetBytes(peHeaders.PEHeaderStartOffset + ChecksumOffsetInPEHeader, CheckSumSize, 0);

            // Zero the authenticode signature
            int authenticodeOffset = GetAuthenticodeOffset(peHeaders, is32bit);
            var authenticodeDir = peHeaders.PEHeader.CertificateTableDirectory;
            peBuffer.SetBytes(authenticodeOffset, AuthenticodeDirectorySize, 0);

            if (setStrongNameBit)
            {
                var flagBytes = BitConverter.GetBytes((uint)(peHeaders.CorHeader.Flags | CorFlags.StrongNameSigned));
                peBuffer.SetBytes(peHeaders.CorHeaderStartOffset + FlagsOffsetInCorHeader, flagBytes);
            }
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

        /// <summary>
        /// Sets the bytes in the buffer starting at <paramref name="index"/> to the bytes in <paramref name="value"/>
        /// </summary>
        /// <param name="buffer">Buffer to alter</param>
        /// <param name="index">Starting index</param>
        /// <param name="value">Value</param>
        private static void SetBytes(this byte[] buffer, int index, byte[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                buffer[index + i] = value[i];
            }
        }

        private static byte[] ReadPEToBuffer(Stream peStream)
        {
            byte[] peImage = new byte[checked((int)peStream.Length)];

            peStream.Position = 0;
            if (peStream.Read(peImage, 0, peImage.Length) != peImage.Length)
            {
                throw new InvalidOperationException("Failed to read the full PE file.");
            }

            return peImage;
        }

        private static int GetAuthenticodeOffset(PEHeaders peHeaders, bool is32bit)
        {
            return peHeaders.PEHeaderStartOffset
                + ChecksumOffsetInPEHeader
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
            int checksumStart = peHeaders.PEHeaderStartOffset + ChecksumOffsetInPEHeader;
            int checksumEnd = checksumStart + CheckSumSize;
            // Content up to the checksum
            imageWithoutChecksum.WriteBytes(peImage, 0, checksumStart);
            // Content after the checksum
            imageWithoutChecksum.WriteBytes(peImage, checksumEnd, peImage.Length - checksumEnd);
            return imageWithoutChecksum.GetBlobs();
        }

        /// <summary>
        /// Adapted from roslyn's CryptoBlobParser
        /// </summary>
        private enum AlgorithmClass
        {
            Signature = 1,
            Hash = 4,
        }

        /// <summary>
        /// Adapted from roslyn's CryptoBlobParser
        /// </summary>
        private enum AlgorithmSubId
        {
            Sha1Hash = 4,
            // Other possible values ommitted
        }

        /// <summary>
        /// Adapted from roslyn's CryptoBlobParser
        /// </summary>
        private struct AlgorithmId
        {
            // From wincrypt.h
            private const int AlgorithmClassOffset = 13;
            private const int AlgorithmClassMask = 0x7;
            private const int AlgorithmSubIdOffset = 0;
            private const int AlgorithmSubIdMask = 0x1ff;

            private readonly uint _flags;

            public const int RsaSign = 0x00002400;
            public const int Sha = 0x00008004;

            public bool IsSet
            {
                get { return _flags != 0; }
            }

            public AlgorithmClass Class
            {
                get { return (AlgorithmClass)((_flags >> AlgorithmClassOffset) & AlgorithmClassMask); }
            }

            public AlgorithmSubId SubId
            {
                get { return (AlgorithmSubId)((_flags >> AlgorithmSubIdOffset) & AlgorithmSubIdMask); }
            }

            public AlgorithmId(uint flags)
            {
                _flags = flags;
            }
        }

        // From StrongNameInternal.cpp
        // Checks to see if a public key is a valid instance of a PublicKeyBlob as
        // defined in StongName.h
        internal static bool IsValidPublicKey(ImmutableArray<byte> blob)
        {
            // The number of public key bytes must be at least large enough for the header and one byte of data.
            if (blob.IsDefault || blob.Length < SnPublicKeyHeaderSize + 1)
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
            if (blob.Length != SnPublicKeyHeaderSize + publicKeySize)
            {
                return false;
            }

            // Check for the ECMA neutral public key, which does not obey the invariants checked below.
            if (blob.SequenceEqual(NeutralPublicKey))
            {
                return true;
            }

            // The public key must be in the wincrypto PUBLICKEYBLOB format
            if (publicKey != PublicKeyBlobId)
            {
                return false;
            }

            var signatureAlgorithmId = new AlgorithmId(sigAlgId);
            if (signatureAlgorithmId.IsSet && signatureAlgorithmId.Class != AlgorithmClass.Signature)
            {
                return false;
            }

            var hashAlgorithmId = new AlgorithmId(hashAlgId);
            if (hashAlgorithmId.IsSet && (hashAlgorithmId.Class != AlgorithmClass.Hash || hashAlgorithmId.SubId < AlgorithmSubId.Sha1Hash))
            {
                return false;
            }

            return true;
        }

        private static ImmutableArray<byte> CreateSnPublicKeyBlob(
            byte type,
            byte version,
            uint algId,
            uint magic,
            uint bitLen,
            uint pubExp,
            ReadOnlySpan<byte> pubKeyData)
        {
            var w = new BlobWriter(3 * sizeof(uint) + OffsetToKeyData + pubKeyData.Length);
            w.WriteUInt32(AlgorithmId.RsaSign);
            w.WriteUInt32(AlgorithmId.Sha);
            w.WriteUInt32((uint)(OffsetToKeyData + pubKeyData.Length));

            w.WriteByte(type);
            w.WriteByte(version);
            w.WriteUInt16(0 /* 16 bits of reserved space in the spec */);
            w.WriteUInt32(algId);

            w.WriteUInt32(magic);
            w.WriteUInt32(bitLen);

            // re-add padding for exponent
            w.WriteUInt32(pubExp);

            unsafe
            {
                fixed (byte* bytes = pubKeyData)
                {
                    w.WriteBytes(bytes, pubKeyData.Length);
                }
            }

            return w.ToImmutableArray();
        }

        /// <summary>
        /// Try to retrieve the public key from a crypto blob.
        /// </summary>
        /// <remarks>
        /// Can be either a PUBLICKEYBLOB or PRIVATEKEYBLOB. The BLOB must be unencrypted.
        /// </remarks>
        public static bool TryParseKey(ImmutableArray<byte> blob, out ImmutableArray<byte> snKey, out RSAParameters? privateKey)
        {
            privateKey = null;
            snKey = default;

            if (IsValidPublicKey(blob))
            {
                snKey = blob;
                return true;
            }

            if (blob.Length < BlobHeaderSize + RsaPubKeySize)
            {
                return false;
            }

            try
            {
                MemoryStream stream = new MemoryStream(blob.ToArray());
                var br = new BinaryReader(stream);

                byte bType = br.ReadByte();    // BLOBHEADER.bType: Expected to be 0x6 (PUBLICKEYBLOB) or 0x7 (PRIVATEKEYBLOB), though there's no check for backward compat reasons. 
                byte bVersion = br.ReadByte(); // BLOBHEADER.bVersion: Expected to be 0x2, though there's no check for backward compat reasons.
                br.ReadUInt16();               // BLOBHEADER.wReserved
                uint algId = br.ReadUInt32();  // BLOBHEADER.aiKeyAlg
                uint magic = br.ReadUInt32();  // RSAPubKey.magic: Expected to be 0x31415352 ('RSA1') or 0x32415352 ('RSA2') 
                var bitLen = br.ReadUInt32();  // Bit Length for Modulus
                var pubExp = br.ReadUInt32();  // Exponent 
                var modulusLength = (int)(bitLen / 8);

                if (blob.Length - OffsetToKeyData < modulusLength)
                {
                    return false;
                }

                var modulus = br.ReadBytes(modulusLength);

                if (!(bType == PrivateKeyBlobId && magic == RSA2) && !(bType == PublicKeyBlobId && magic == RSA1))
                {
                    return false;
                }

                if (bType == PrivateKeyBlobId)
                {
                    privateKey = blob.ToRSAParameters(true);
                    // For snKey, rewrite some of the parameters
                    algId = AlgorithmId.RsaSign;
                    magic = RSA1;
                }

                snKey = CreateSnPublicKeyBlob(PublicKeyBlobId, bVersion, algId, RSA1, bitLen, pubExp, modulus);
                return true;
            }
            catch (Exception)
            {
                return false;
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

        private static byte[] ReadReversed(this BinaryReader reader, int count)
        {
            byte[] data = reader.ReadBytes(count);
            Array.Reverse(data);
            return data;
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
    }
}
