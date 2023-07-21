// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Microsoft.DotNet.Build.Tasks.Workloads
{
    public class Utils
    {
        /// <summary>
        /// Generate a truncated hash (first 16 bytes) for a string value using a given hash algorithm.
        /// </summary>
        /// <param name="value">The value to hash.</param>
        /// <param name="hashName">The name of the <see cref="HashAlgorithm"/> to use.</param>
        /// <returns>A string containing the hash result.</returns>
        public static string GetTruncatedHash(string value, HashAlgorithmName hashName)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
#pragma warning disable SYSLIB0045 // Cryptographic factory methods accepting an algorithm name are obsolete. Use the parameterless Create factory method on the algorithm type instead.
            HashAlgorithm algorithm = HashAlgorithm.Create(hashName.Name);
#pragma warning restore SYSLIB0045
            StringBuilder sb = new();

            foreach (byte b in algorithm.ComputeHash(bytes))
            {
                sb.Append(b.ToString("x2"));
            }

            string result = sb.ToString();

            return result.Substring(0, 32);
        }

        /// <summary>
        /// Updates a string containing a path and ensures that it contains a single directory separator at the end.
        /// </summary>
        /// <param name="path">The original path value.</param>
        /// <returns>The modified path.</returns>
        internal static string EnsureTrailingSlash(string path)
        {
            return path.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
        }

        /// <summary>
        /// Generates a safe SWIX ID by replacing "-", " ", and "_" with ".".
        /// </summary>
        /// <param name="id">The identifier to convert to a safe identifier</param>
        /// <returns>The safe identifier.</returns>
        internal static string ToSafeId(string id, string suffix = null) =>
            id.Replace("-", ".").Replace(" ", ".").Replace("_", ".") +
            (string.IsNullOrWhiteSpace(suffix) ? null : $".{suffix}");

        /// <summary>
        /// Replaces all the tokens in a file using the provided dictionary. The dictionary keys define the tokens and
        /// their values the replacement strings.
        /// </summary>
        /// <param name="fileName">The file to modify.</param>
        /// <param name="tokenReplacements">A dictionary containing the replacement tokens and values.</param>
        /// <param name="encoding">The encoding to use when updating the file.</param>
        internal static void StringReplace(string fileName, Dictionary<string, string> tokenReplacements, Encoding encoding)
        {
            FileAttributes oldAttributes = File.GetAttributes(fileName);
            File.SetAttributes(fileName, oldAttributes & ~FileAttributes.ReadOnly);

            string content = File.ReadAllText(fileName);

            foreach (string token in tokenReplacements.Keys)
            {
                content = content.Replace(token, tokenReplacements[token]);
            }

            File.WriteAllText(fileName, content, encoding);
            File.SetAttributes(fileName, oldAttributes);
        }

        /// <summary>
        /// Checks whether a string parameter is neither <see langword="null"/> nor empty.
        /// </summary>
        /// <param name="name">The name of the parameter to check.</param>
        /// <param name="value">The value of the parameter.</param>
        internal static string CheckNullOrEmpty(string name, string value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(name);
            }

            if (value == string.Empty)
            {
                throw new ArgumentException($"Parameter cannot be empty: ${name}");
            }

            return value;
        }

        /// <summary>
        /// Converts a string containing wildcards (*, ?) into a regular expression pattern string.
        /// </summary>
        /// <param name="wildcardPattern">The string pattern.</param>
        /// <returns>A string containing regular expression pattern.</returns>
        public static string ConvertToRegexPattern(string wildcardPattern)
        {
            string escapedPattern = Regex.Escape(wildcardPattern).Replace(@"\*", ".*").Replace(@"\?", ".");

            if ((wildcardPattern.EndsWith("*")) || (wildcardPattern.EndsWith("?")))
            {
                return escapedPattern;
            }
            else
            {
                return string.Concat(escapedPattern, "$");
            }
        }

        /// <summary>
        /// Generates a version 3 UUID given a namespace UUID and name. This is based on the algorithm described in
        /// RFC 4122 (https://tools.ietf.org/html/rfc4122), section 4.3.
        /// </summary>
        /// <param name="namespaceUuid">The UUID representing the namespace.</param>
        /// <param name="name">The name for which to generate a UUID within the given namespace.</param>
        /// <returns>A UUID generated using the given namespace UUID and name.</returns>
        public static Guid CreateUuid(Guid namespaceUuid, string name)
        {
            // 1. Convert the name to a canonical sequence of octets (as defined by the standards or conventions of its name space); put the name space ID in network byte order. 
            byte[] namespaceBytes = namespaceUuid.ToByteArray();
            // Octet 0-3
            int timeLow = IPAddress.HostToNetworkOrder(BitConverter.ToInt32(namespaceBytes, 0));
            // Octet 4-5
            short timeMid = IPAddress.HostToNetworkOrder(BitConverter.ToInt16(namespaceBytes, 4));
            // Octet 6-7
            short timeHiVersion = IPAddress.HostToNetworkOrder(BitConverter.ToInt16(namespaceBytes, 6));

            // 2. Compute the hash of the namespace ID concatenated with the name
            byte[] nameBytes = Encoding.Unicode.GetBytes(name);
            byte[] hashBuffer = new byte[namespaceBytes.Length + nameBytes.Length];

            Buffer.BlockCopy(BitConverter.GetBytes(timeLow), 0, hashBuffer, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(timeMid), 0, hashBuffer, 4, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(timeHiVersion), 0, hashBuffer, 6, 2);
            Buffer.BlockCopy(namespaceBytes, 8, hashBuffer, 8, 8);
            Buffer.BlockCopy(nameBytes, 0, hashBuffer, 16, nameBytes.Length);
            byte[] hash;

            using (SHA256 sha256 = SHA256.Create())
            {
                hash = sha256.ComputeHash(hashBuffer);
            }

            Array.Resize(ref hash, 16);

            // 3. Set octets zero through 3 of the time_low field to octets zero through 3 of the hash. 
            timeLow = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(hash, 0));
            Buffer.BlockCopy(BitConverter.GetBytes(timeLow), 0, hash, 0, 4);

            // 4. Set octets zero and one of the time_mid field to octets 4 and 5 of the hash. 
            timeMid = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(hash, 4));
            Buffer.BlockCopy(BitConverter.GetBytes(timeMid), 0, hash, 4, 2);

            // 5. Set octets zero and one of the time_hi_and_version field to octets 6 and 7 of the hash. 
            timeHiVersion = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(hash, 6));

            // 6. Set the four most significant bits (bits 12 through 15) of the time_hi_and_version field to the appropriate 4-bit version number from Section 4.1.3. 
            timeHiVersion = (short)((timeHiVersion & 0x0fff) | 0x3000);
            Buffer.BlockCopy(BitConverter.GetBytes(timeHiVersion), 0, hash, 6, 2);

            // 7. Set the clock_seq_hi_and_reserved field to octet 8 of the hash. 
            // 8. Set the two most significant bits (bits 6 and 7) of the clock_seq_hi_and_reserved to zero and one, respectively.
            hash[8] = (byte)((hash[8] & 0x3f) | 0x80);

            // Steps 9-11 are essentially no-ops, but provided for completion sake
            // 9. Set the clock_seq_low field to octet 9 of the hash.
            // 10. Set octets zero through five of the node field to octets 10 through 15 of the hash.
            // 11. Convert the resulting UUID to local byte order. 

            return new Guid(hash);
        }

        /// <summary>
        /// Deletes the specified directory and all subdirectories if it exists.
        /// </summary>
        /// <param name="path">The directory to delete.</param>
        internal static void DeleteDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, recursive: true);
            }
        }

        internal static void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
