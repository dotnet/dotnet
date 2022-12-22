// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Reflection.PortableExecutable;
using System.Reflection.Metadata;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace Microsoft.DotNet.SignTool
{
    internal static class ContentUtil
    {
        public static ImmutableArray<byte> GetContentHash(string fullPath)
        {
            using (var stream = File.OpenRead(fullPath))
            {
                return GetContentHash(stream);
            }
        }

        public static ImmutableArray<byte> GetContentHash(Stream stream)
        {
            using (var sha2 = SHA256.Create())
            {
                return ImmutableArray.Create(sha2.ComputeHash(stream));
            }
        }

        public static string HashToString(ImmutableArray<byte> hash)
            => BitConverter.ToString(hash.ToArray()).Replace("-", "");

        public static ImmutableArray<byte> StringToHash(string hash)
        {
            int NumberChars = hash.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hash.Substring(i, 2), 16);
            return bytes.ToImmutableArray<byte>();

        }

        /// <summary>
        /// Returns true if the PE file meets all of the pre-conditions to be Open Source Signed.
        /// Returns false and logs msbuild errors otherwise.
        /// </summary>
        public static bool IsPublicSigned(PEReader peReader)
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

        public static bool IsManaged(string filePath)
        {
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Open))
                using (var peReader = new PEReader(stream))
                {
                    return peReader.PEHeaders.CorHeader != null;
                }
            }
            catch (BadImageFormatException)
            {
                return false;
            }
        }

        public static bool IsCrossgened(string filePath)
        {
            const int CROSSGEN_FLAG = 4;

            using (var stream = new FileStream(filePath, FileMode.Open))
            using (var peReader = new PEReader(stream))
            {
                return ((int)peReader.PEHeaders.CorHeader.Flags & CROSSGEN_FLAG) == CROSSGEN_FLAG;
            }
        }

        public static bool IsAuthenticodeSigned(Stream assemblyStream)
        {
            using (var peReader = new PEReader(assemblyStream))
            {
                var headers = peReader.PEHeaders;
                var entry = headers.PEHeader.CertificateTableDirectory;

                return entry.Size > 0;
            }
        }

        public static string GetPublicKeyToken(string fullPath)
        {
            try
            {
                AssemblyName assemblyName = AssemblyName.GetAssemblyName(fullPath);
                byte[] pktBytes = assemblyName.GetPublicKeyToken();

                return (pktBytes == null || pktBytes.Length == 0) ? 
                    string.Empty : 
                    string.Join("", pktBytes.Select(b => b.ToString("x2")));
            }
            catch (BadImageFormatException)
            {
                return string.Empty;
            }
        }
    }
}
