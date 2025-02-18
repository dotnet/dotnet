// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Microsoft.SignCheck
{
    public class Utils
    {
        /// <summary>
        /// Generate a hash for a string value using a given hash algorithm.
        /// </summary>
        /// <param name="value">The value to hash.</param>
        /// <param name="hashName">The name of the <see cref="HashAlgorithm"/> to use.</param>
        /// <returns>A string containing the hash result.</returns>
        public static string GetHash(string value, string hashName)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            HashAlgorithm ha = CreateHashAlgorithm(hashName);
            byte[] hash = ha.ComputeHash(bytes);

            var sb = new StringBuilder();
            foreach (byte b in hash)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }

        public static HashAlgorithm CreateHashAlgorithm(string hashName)
        {
            switch (hashName.ToUpperInvariant())
            {
                case "SHA256":
                    return SHA256.Create();
                case "SHA1":
                    return SHA1.Create();
                case "MD5":
                    return MD5.Create();
                case "SHA384":
                    return SHA384.Create();
                case "SHA512":
                    return SHA512.Create();
                default:
                    throw new ArgumentException("Unsupported hash algorithm name", nameof(hashName));
            }
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
                return String.Concat(escapedPattern, "$");
            }            
        }
    }
}
