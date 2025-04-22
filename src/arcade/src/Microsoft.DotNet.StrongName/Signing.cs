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

namespace Microsoft.DotNet.StrongName
{
    internal static class Signing
    {
        /// <summary>
        /// Unset the strong name signing bit from a file. This is required for sn
        /// </summary>
        /// <param name="file"></param>
        internal static void ClearStrongNameSignedBit(string file)
        {
            using (var stream = new FileStream(file, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
            using (var peReader = new PEReader(stream))
            using (var writer = new BinaryWriter(stream))
            {
                if (!IsPublicSigned(peReader))
                {
                    return;
                }

                stream.Position = peReader.PEHeaders.CorHeaderStartOffset + Constants.FlagsOffsetInCorHeader;
                writer.Write((UInt32)(peReader.PEHeaders.CorHeader.Flags & ~CorFlags.StrongNameSigned));
            }
        }

        /// <summary>
        /// Gets the public key token from a strong named file.
        /// </summary>
        /// <param name="file">Path to file</param>
        /// <returns>Public key token</returns>
        internal static int GetStrongNameTokenFromAssembly(string file, out string tokenStr)
        {
            tokenStr = null;

            try
            {
                using (var stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (var peReader = new PEReader(stream))
                {
                    if (!peReader.HasMetadata)
                    {
                        return -1;
                    }

                    var metadataReader = peReader.GetMetadataReader();
                    if (!metadataReader.IsAssembly)
                    {
                        return -1;
                    }

                    ImmutableArray<byte> publicKeyBlob = metadataReader.GetPublicKeyBlob();
                    if (!TryParseKey(publicKeyBlob, out ImmutableArray<byte> snKey, out _))
                    {
                        return -1;
                    }
                    
                    byte[] token = GetPublicKeyToken(snKey.ToArray());
                    tokenStr = BitConverter.ToString(token).Replace("-", "").ToLowerInvariant();
                    return 0; // S_OK
                }
            }
            catch(Exception)
            {
                return -1;
            }
        }

        /// <summary>
        /// Strong names an existing previously signed or delay-signed binary with keyfile.
        /// Fall back to legacy signing if available and new signing fails.
        /// </summary>
        /// <param name="file">Path to file to sign</param>
        /// <param name="keyFile">Path to key pair.</param>
        /// <param name="snPath">Optional path to sn.exe</param>
        /// <returns>True if the file was signed successfully, false otherwise</returns>
        internal static bool Sign(string file, string keyFile, string snPath = null)
        {
            try
            {
                using (var metadata = new FileStream(file, FileMode.Open))
                {
                    Sign(metadata, keyFile);
                }
                return true;
            }
            catch (Exception)
            {
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
                ImmutableArray<byte> publicKeyBlob = metadataReader.GetPublicKeyBlob();
                if (publicKeyBlob.SequenceEqual(Constants.NeutralPublicKey))
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
                peBuffer = Utils.ReadPEToBuffer(peStream);

                // Now prepare that buffer for hashing
                Utils.PreparePEForHashing(peBuffer, peHeaders, setStrongNameBit: true);

                byte[] hash = Utils.ComputeSigningHash(peBuffer, peHeaders, snSignatureOffset, snDirectory.Size);
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
                uint checksum = Utils.CalculateChecksum(peBuffer, peHeaders);
                var checksumBytes = BitConverter.GetBytes(checksum);
                peBuffer.SetBytes(peHeaders.PEHeaderStartOffset + Constants.ChecksumOffsetInPEHeader, checksumBytes);
            }

            // Write the PE stream back
            peStream.Position = 0;
            peStream.Write(peBuffer, 0, peBuffer.Length);
        }

        /// <summary>
        /// Returns true if the PE file meets all of the pre-conditions to be Open Source Signed.
        /// Returns false and logs msbuild errors otherwise.
        /// </summary>
        private static bool IsPublicSigned(PEReader peReader)
        {
            if (!peReader.HasMetadata)
            {
                return false;
            }

            var mdReader = peReader.GetMetadataReader();
            if (!mdReader.IsAssembly)
            {
                return false;
            }

            CorHeader header = peReader.PEHeaders.CorHeader;
            return (header.Flags & CorFlags.StrongNameSigned) == CorFlags.StrongNameSigned;
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
            var w = new BlobWriter(3 * sizeof(uint) + Constants.OffsetToKeyData + pubKeyData.Length);
            w.WriteUInt32(Algorithm.AlgorithmId.RsaSign);
            w.WriteUInt32(Algorithm.AlgorithmId.Sha);
            w.WriteUInt32((uint)(Constants.OffsetToKeyData + pubKeyData.Length));

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
        private static bool TryParseKey(ImmutableArray<byte> blob, out ImmutableArray<byte> snKey, out RSAParameters? privateKey)
        {
            privateKey = null;
            snKey = default;

            if (Utils.IsValidPublicKey(blob))
            {
                snKey = blob;
                return true;
            }

            if (blob.Length < Constants.BlobHeaderSize + Constants.RsaPubKeySize)
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

                if (blob.Length - Constants.OffsetToKeyData < modulusLength)
                {
                    return false;
                }

                var modulus = br.ReadBytes(modulusLength);

                if (!(bType == Constants.PrivateKeyBlobId && magic == Constants.RSA2) && !(bType == Constants.PublicKeyBlobId && magic == Constants.RSA1))
                {
                    return false;
                }

                if (bType == Constants.PrivateKeyBlobId)
                {
                    privateKey = blob.ToRSAParameters(true);
                    // For snKey, rewrite some of the parameters
                    algId = Algorithm.AlgorithmId.RsaSign;
                    magic = Constants.RSA1;
                }

                snKey = CreateSnPublicKeyBlob(Constants.PublicKeyBlobId, bVersion, algId, Constants.RSA1, bitLen, pubExp, modulus);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Computes the public key token from the public key.
        /// </summary>
        /// <param name="publicKey">The public key.</param>
        /// <returns>The public key token.</returns>
        private static byte[] GetPublicKeyToken(byte[] publicKey)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] hash = sha1.ComputeHash(publicKey);
                byte[] token = new byte[8];
                Array.Copy(hash, hash.Length - 8, token, 0, 8);
                Array.Reverse(token); // Reverse the bytes to match the expected format

                return token;
            }
        }
    }
}
