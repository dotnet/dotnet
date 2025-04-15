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
    internal static class Verification
    {
        /// <summary>
        /// Returns true if the file has a valid strong name signature.
        /// </summary>
        /// <param name="file">Path to file</param>
        /// <param name="snPath">Path to sn.exe, if available and desired.</param>
        /// <returns>True if the file has a valid strong name signature, false otherwise.</returns>
        internal static bool IsSigned(string file, string snPath = null)
        {
            try
            {
                using (var metadata = new FileStream(file, FileMode.Open))
                {
                    return IsSigned(metadata);
                }
            }
            catch (Exception)
            {
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
                byte[] peBuffer = Utils.ReadPEToBuffer(peStream);

                // Verify the checksum before verifying the strong name signature
                // PreparePEForHashing will zero out the checksum and authenticode signature.

                if (peHeaders.PEHeader.CheckSum != Utils.CalculateChecksum(peBuffer, peHeaders))
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
                Utils.PreparePEForHashing(peBuffer, peHeaders, setStrongNameBit: false);

                int snSize = snDirectory.Size;
                byte[] hash = Utils.ComputeSigningHash(peBuffer, peHeaders, snOffset, snSize);

                ImmutableArray<byte> publicKeyBlob = metadataReader.GetBlobContent(metadataReader.GetAssemblyDefinition().PublicKey);

                if (!Utils.IsValidPublicKey(publicKeyBlob))
                {
                    return false;
                }

                // It's possible that the public key blob is a neutral public key blob,
                // meaning that it's actually the ECMA key that was used to sign the assembly.
                // Verify against that.
                if (publicKeyBlob.SequenceEqual(Constants.NeutralPublicKey))
                {
                    publicKeyBlob = Constants.ECMAKey.ToImmutableArray();
                }

                using (RSA rsa = RSA.Create())
                {
                    // Ensure we skip over the sn key header.
                    rsa.ImportParameters(publicKeyBlob.Slice(Constants.RsaPubKeySize, publicKeyBlob.Length - Constants.RsaPubKeySize).ToRSAParameters(false));

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
    }
}
