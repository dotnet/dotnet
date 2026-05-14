// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace Microsoft.SignCheck.Verification.Jar
{
    public static class JarUtils
    {
        // Newline characters supported by manifest files
        public static readonly char[] NewLine = new char[] { '\r', '\n' };

        /// <summary>
        /// Reads a file from the JAR archive into a byte array.
        /// </summary>
        /// <param name="archivePath">The path of the JAR archive.</param>
        /// <param name="path">The path of the file in the archive.</param>
        /// <returns>The contents of the file or null if the archive does not contain the file.</returns>
        public static byte[] ReadBytes(string archivePath, string path)
        {
            using (ZipArchive archive = ZipFile.Open(archivePath, ZipArchiveMode.Read))
            {
                ZipArchiveEntry entry = archive.GetEntry(path);

                if (entry != null)
                {
                    using (Stream stream = entry.Open())
                    using (var ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        return ms.ToArray();
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Creates a Base64 encoded digest for a given input string and hash algorithm.
        /// </summary>
        /// <param name="input">The input to hash.</param>
        /// <param name="algorithmName">The hash algorithm to use.</param>
        /// <returns></returns>
        public static string GetHashDigest(string input, string algorithmName)
        {
            using (HashAlgorithm hashAlgorithm = Utils.CreateHashAlgorithm(algorithmName))
            {
                byte[] hashValue = hashAlgorithm.ComputeHash(new UTF8Encoding().GetBytes(input.ToCharArray()));
                return Convert.ToBase64String(hashValue);
            }
        }

        /// <summary>
        /// Strip the suffix of an x-digest attribute to return the hash algorithm name.
        /// </summary>
        /// <param name="attribute">The digest attribute.</param>
        /// <param name="suffix">The attribute suffix to strip.</param>
        /// <returns>The hash algorithm name.</returns>
        public static string GetHashAlgorithmFromDigest(string attribute, string suffix)
        {
            int digestIndex = attribute.IndexOf(suffix, StringComparison.OrdinalIgnoreCase);
            return digestIndex > 0 ? attribute.Substring(0, digestIndex) : String.Empty;
        }

        /// <summary>
        /// Writes a string value into a stream and resets the stream position to 0.
        /// </summary>
        /// <param name="value">The value to write to the stream.</param>
        /// <returns>A stream containing the string value.</returns>
        public static Stream ToStream(string value)
        {
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);
            sw.Write(value);
            sw.Flush();
            ms.Position = 0;
            return ms;
        }
    }
}
