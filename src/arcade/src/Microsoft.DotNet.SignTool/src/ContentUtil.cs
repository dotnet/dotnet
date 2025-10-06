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
using System.Diagnostics;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.Reflection.Metadata.Ecma335;

namespace Microsoft.DotNet.SignTool
{
    internal static class ContentUtil
    {
        /// <summary>
        /// Returns the hash of the content of the file at the given path.
        /// If the file is empty, returns the hash of an empty stream.
        /// </summary>
        /// <param name="fullPath">Path of file to hash</param>
        /// <returns>Hash of content.</returns>
        public static ImmutableArray<byte> GetContentHash(string fullPath)
        {
            using (var stream = File.OpenRead(fullPath))
            {
                if (stream.Length == 0)
                {
                    return EmptyFileContentHash;
                }
                return GetContentHash(stream);
            }
        }

        public static readonly ImmutableArray<byte> EmptyFileContentHash = GetContentHash(new MemoryStream()).ToImmutableArray();

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

        /// <summary>
        /// Determines the executable type of a file by examining its binary format.
        /// Returns PE, MachO, ELF, or None if the format is not recognized.
        /// </summary>
        /// <param name="filePath">Path to the file to examine</param>
        /// <returns>The executable type or None if not recognized</returns>
        public static ExecutableType GetExecutableType(string filePath)
        {
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    if (stream.Length < 4)
                        return ExecutableType.None;

                    var buffer = new byte[4]; // Read enough for MachO and ELF magic numbers
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead < 4)
                        return ExecutableType.None;

                    // Check for ELF magic: 7F 45 4C 46
                    if (buffer[0] == 0x7F && buffer[1] == 0x45 && buffer[2] == 0x4C && buffer[3] == 0x46)
                        return ExecutableType.ELF;

                    // Check for Mach-O magic numbers
                    uint magic = BitConverter.ToUInt32(buffer, 0);
                    if (magic == 0xFEEDFACE || magic == 0xFEEDFACF || 
                        magic == 0xCEFAEDFE || magic == 0xCFFAEDFE)
                        return ExecutableType.MachO;

                    stream.Seek(0, SeekOrigin.Begin);
                    using (var peReader = new PEReader(stream))
                    {
                        // This will throw if it's not a PE.
                        return (peReader.PEHeaders.PEHeaderStartOffset != 0) ? ExecutableType.PE : ExecutableType.None;
                    }
                }
            }
            catch
            {
                return ExecutableType.None;
            }
        }
    }
}
