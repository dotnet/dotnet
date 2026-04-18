// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Contains some utility methods.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// A string with "empty" value.
        /// </summary>
        public const string Empty = "empty";

        /// <summary>
        /// A string with "null" value.
        /// </summary>
        public const string Null = "null";

        /// <summary>
        /// Creates a copy of the byte array.
        /// </summary>
        /// <param name="src">The resource array.</param>
        /// <returns>A copy of the byte array.</returns>
        public static byte[] CloneByteArray(this byte[] src)
        {
            if (src == null)
                throw LogHelper.LogArgumentNullException(nameof(src));

            return (byte[])src.Clone();
        }

        /// <summary>
        /// Serializes the list of strings into string as follows:
        /// 'str1','str2','str3' ...
        /// </summary>
        /// <param name="strings">
        /// The strings used to build a comma delimited string.
        /// </param>
        /// <returns>
        /// The single <see cref="string"/>.
        /// </returns>
        internal static string SerializeAsSingleCommaDelimitedString(IEnumerable<string> strings)
        {
            if (strings == null)
            {
                return Utility.Null;
            }

            StringBuilder sb = new StringBuilder();
            bool first = true;
            foreach (string str in strings)
            {
                if (first)
                {
                    sb.AppendFormat(CultureInfo.InvariantCulture, "{0}", str ?? Utility.Null);
                    first = false;
                }
                else
                {
                    sb.AppendFormat(CultureInfo.InvariantCulture, ", {0}", str ?? Utility.Null);
                }
            }

            if (first)
            {
                return Utility.Empty;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns whether the input string is https.
        /// </summary>
        /// <param name="address">The input string.</param>
        /// <remarks>true if the input string is https; otherwise, false.</remarks>
        public static bool IsHttps(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                return false;
            }

            try
            {
                Uri uri = new Uri(address);
                return IsHttps(uri);
            }
            catch (UriFormatException)
            {
                return false;
            }
        }

        /// <summary>
        /// Returns whether the input uri is https.
        /// </summary>
        /// <param name="uri"><see cref="Uri"/>.</param>
        /// <returns>true if the input uri is https; otherwise, false.</returns>
        public static bool IsHttps(Uri uri)
        {
            if (uri == null)
            {
                return false;
            }
#if NET462
            return uri.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase); //Uri.UriSchemeHttps is internal in dnxcore
#else
            return uri.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase);
#endif
        }

        /// <summary>
        /// Compares two byte arrays for equality. Hash size is fixed normally it is 32 bytes.
        /// The attempt here is to take the same time if an attacker shortens the signature OR changes some of the signed contents.
        /// </summary>
        /// <param name="a">
        /// One set of bytes to compare.
        /// </param>
        /// <param name="b">
        /// The other set of bytes to compare with.
        /// </param>
        /// <returns>
        /// true if the bytes are equal, false otherwise.
        /// </returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static bool AreEqual(byte[] a, byte[] b)
        {
            ReadOnlySpan<byte> a1, a2;

            if (((a == null) || (b == null))
            || (a.Length != b.Length))
            {
                // Non-allocating. The direct assignment into a ReadOnlySpan<byte> causes the C# compiler to emit these as pointers into the assembly's data section.
                a1 = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };
                a2 = new byte[] { 31, 30, 29, 28, 27, 26, 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 };
            }
            else
            {
                a1 = a.AsSpan();
                a2 = b.AsSpan();
            }

            int result = 0;
            for (int i = 0; i < a1.Length; i++)
            {
                result |= a1[i] ^ a2[i];
            }

            return result == 0;
        }

        /// <summary>
        /// Compares two byte spans for equality. Hash size is fixed normally it is 32 bytes.
        /// The attempt here is to take the same time if an attacker shortens the signature OR changes some of the signed contents.
        /// </summary>
        /// <param name="a">
        /// One set of bytes to compare.
        /// </param>
        /// <param name="b">
        /// The other set of bytes to compare with.
        /// </param>
        /// <param name="length">length of spans to check</param>
        /// <returns>
        /// true if the bytes are equal, false otherwise.
        /// </returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static bool AreEqual(ReadOnlySpan<byte> a, ReadOnlySpan<byte> b, int length)
        {
            if ((a.Length < length || b.Length < length))
            {
                // Non-allocating. The direct assignment into a ReadOnlySpan<byte> causes the C# compiler to emit these as pointers into the assembly's data section.
                a = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };
                b = new byte[] { 31, 30, 29, 28, 27, 26, 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 };
            }
            else
            {
                a = a.Slice(0, length);
                b = b.Slice(0, length);
            }

            int result = 0;
            for (int i = 0; i < a.Length; i++)
            {
                result |= a[i] ^ b[i];
            }

            return result == 0;
        }

        internal static byte[] ConvertToBigEndian(long i)
        {
            byte[] temp = BitConverter.GetBytes(i);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(temp);

            return temp;
        }

        internal static byte[] Xor(byte[] a, byte[] b, int offset, bool inPlace)
        {
            if (inPlace)
            {
                for (var i = 0; i < a.Length; i++)
                {
                    a[i] = (byte)(a[i] ^ b[offset + i]);
                }

                return a;
            }
            else
            {
                var result = new byte[a.Length];

                for (var i = 0; i < a.Length; i++)
                {
                    result[i] = (byte)(a[i] ^ b[offset + i]);
                }

                return result;
            }
        }

        internal static void Zero(byte[] byteArray)
        {
            for (var i = 0; i < byteArray.Length; i++)
            {
                byteArray[i] = 0;
            }
        }

        internal static byte[] GenerateSha256Hash(string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);

#if NET6_0_OR_GREATER
            return SHA256.HashData(bytes);
#else
            using (var hash = SHA256.Create())
            {
                return hash.ComputeHash(bytes);
            }
#endif
        }
    }
}
