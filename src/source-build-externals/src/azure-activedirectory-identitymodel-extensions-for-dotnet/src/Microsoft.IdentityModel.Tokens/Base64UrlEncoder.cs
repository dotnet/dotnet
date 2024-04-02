// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Encodes and Decodes strings as Base64Url encoding.
    /// </summary>
    public static class Base64UrlEncoder
    {
        private const char base64PadCharacter = '=';
        private const char base64Character62 = '+';
        private const char base64Character63 = '/';
        private const char base64UrlCharacter62 = '-';
        private const char base64UrlCharacter63 = '_';

        /// <summary>
        /// The following functions perform base64url encoding which differs from regular base64 encoding as follows
        /// * padding is skipped so the pad character '=' doesn't have to be percent encoded
        /// * the 62nd and 63rd regular base64 encoding characters ('+' and '/') are replace with ('-' and '_')
        /// The changes make the encoding alphabet file and URL safe.
        /// </summary>
        /// <param name="arg">string to encode.</param>
        /// <returns>Base64Url encoding of the UTF8 bytes.</returns>
        public static string Encode(string arg)
        {
            _ = arg ?? throw LogHelper.LogArgumentNullException(nameof(arg));

            return Encode(Encoding.UTF8.GetBytes(arg));
        }

        /// <summary>
        /// Converts a subset of an array of 8-bit unsigned integers to its equivalent string representation which is encoded with base-64-url digits.
        /// </summary>
        /// <param name="inArray">An array of 8-bit unsigned integers.</param>
        /// <returns>The string representation in base 64 url encoding of length elements of inArray, starting at position offset.</returns>
        /// <exception cref="ArgumentNullException">'inArray' is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">offset or length is negative OR offset plus length is greater than the length of inArray.</exception>
        public static string Encode(byte[] inArray)
        {
            _ = inArray ?? throw LogHelper.LogArgumentNullException(nameof(inArray));

            return Encode(inArray, 0, inArray.Length);
        }

        /// <summary>
        /// Converts a subset of an array of 8-bit unsigned integers to its equivalent string representation which is encoded with base-64-url digits. Parameters specify
        /// the subset as an offset in the input array, and the number of elements in the array to convert.
        /// </summary>
        /// <param name="inArray">An array of 8-bit unsigned integers.</param>
        /// <param name="length">An offset in inArray.</param>
        /// <param name="offset">The number of elements of inArray to convert.</param>
        /// <returns>The string representation in base 64 url encoding of length elements of inArray, starting at position offset.</returns>
        /// <exception cref="ArgumentNullException">'inArray' is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">offset or length is negative OR offset plus length is greater than the length of inArray.</exception>
        public static string Encode(byte[] inArray, int offset, int length)
        {
            _ = inArray ?? throw LogHelper.LogArgumentNullException(nameof(inArray));

            if (offset < 0)
                throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(
                    nameof(offset),
                    LogHelper.FormatInvariant(
                        LogMessages.IDX10716,
                        LogHelper.MarkAsNonPII(nameof(offset)),
                        LogHelper.MarkAsNonPII(offset))));

            if (length == 0)
                return string.Empty;

            if (length < 0)
                throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(
                    nameof(length),
                    LogHelper.FormatInvariant(
                        LogMessages.IDX10716,
                        LogHelper.MarkAsNonPII(nameof(length)),
                        LogHelper.MarkAsNonPII(length))));

            if (inArray.Length < offset + length)
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
                throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(
                    "offset + length",
                    LogHelper.FormatInvariant(
                        LogMessages.IDX10717,
                        LogHelper.MarkAsNonPII(nameof(offset)),
                        LogHelper.MarkAsNonPII(nameof(length)),
                        LogHelper.MarkAsNonPII(nameof(inArray)),
                        LogHelper.MarkAsNonPII(offset),
                        LogHelper.MarkAsNonPII(length),
                        LogHelper.MarkAsNonPII(inArray.Length))));
#pragma warning restore CA2208 // Instantiate argument exceptions correctly

            char[] destination = new char[(inArray.Length + 2) / 3 * 4];
            int j = Encode(inArray.AsSpan<byte>().Slice(offset, length), destination.AsSpan<char>());

            return new string(destination, 0, j);
        }

        /// <summary>
        /// Populates a <see cref="ReadOnlySpan{T}"/>Converts a <see cref="Span{T}"/> encoded with base-64-url digits. Parameters specify
        /// the subset as an offset in the input array, and the number of elements in the array to convert.
        /// </summary>
        /// <param name="inArray">A span of bytes.</param>
        /// <param name="output">output for encoding.</param>
        /// <returns>The number of chars written to the output.</returns>
        public static int Encode(ReadOnlySpan<byte> inArray, Span<char> output)
        {
            int lengthmod3 = inArray.Length % 3;
            int limit = (inArray.Length - lengthmod3);
            ReadOnlySpan<byte> table = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_"u8;

            int i, j = 0;

            // takes 3 bytes from inArray and insert 4 bytes into output
            for (i = 0; i < limit; i += 3)
            {
                byte d0 = inArray[i];
                byte d1 = inArray[i + 1];
                byte d2 = inArray[i + 2];

                output[j + 0] = (char)table[d0 >> 2];
                output[j + 1] = (char)table[((d0 & 0x03) << 4) | (d1 >> 4)];
                output[j + 2] = (char)table[((d1 & 0x0f) << 2) | (d2 >> 6)];
                output[j + 3] = (char)table[d2 & 0x3f];
                j += 4;
            }

            //Where we left off before
            i = limit;

            switch (lengthmod3)
            {
                case 2:
                    {
                        byte d0 = inArray[i];
                        byte d1 = inArray[i + 1];

                        output[j + 0] = (char)table[d0 >> 2];
                        output[j + 1] = (char)table[((d0 & 0x03) << 4) | (d1 >> 4)];
                        output[j + 2] = (char)table[(d1 & 0x0f) << 2];
                        j += 3;
                    }
                    break;

                case 1:
                    {
                        byte d0 = inArray[i];

                        output[j + 0] = (char)table[d0 >> 2];
                        output[j + 1] = (char)table[(d0 & 0x03) << 4];
                        j += 2;
                    }
                    break;

                //default or case 0: no further operations are needed.
            }

            return j;
        }

        /// <summary>
        /// Converts the specified string, base-64-url encoded to utf8 bytes.</summary>
        /// <param name="str">base64Url encoded string.</param>
        /// <returns>UTF8 bytes.</returns>
        public static byte[] DecodeBytes(string str)
        {
            _ = str ?? throw LogHelper.LogExceptionMessage(new ArgumentNullException(nameof(str)));
            return UnsafeDecode(str.AsSpan());
        }

#if NET6_0_OR_GREATER
        [SkipLocalsInit]
#endif
        internal static unsafe byte[] UnsafeDecode(ReadOnlySpan<char> strSpan)
        {
            int mod = strSpan.Length % 4;
            if (mod == 1)
                throw LogHelper.LogExceptionMessage(new FormatException(LogHelper.FormatInvariant(LogMessages.IDX10400, strSpan.ToString())));

            bool needReplace = strSpan.IndexOfAny(base64UrlCharacter62, base64UrlCharacter63) >= 0;
            int decodedLength = strSpan.Length + (4 - mod) % 4;

#if NET6_0_OR_GREATER
            // If the incoming chars don't contain any of the base64url characters that need to be replaced,
            // and if the incoming chars are of the exact right length, then we'll be able to just pass the
            // incoming chars directly to Convert.TryFromBase64Chars. Otherwise, rent an array, copy all the
            // data into it, and do whatever fixups are necessary on that copy, then pass that copy into
            // Convert.TryFromBase64Chars.

            const int StackAllocThreshold = 512;
            char[] arrayPoolChars = null;
            scoped Span<char> charsSpan = default;
            scoped ReadOnlySpan<char> source = strSpan;

            if (needReplace || decodedLength != source.Length)
            {
                charsSpan = decodedLength <= StackAllocThreshold ?
                    stackalloc char[StackAllocThreshold] :
                    arrayPoolChars = ArrayPool<char>.Shared.Rent(decodedLength);
                charsSpan = charsSpan.Slice(0, decodedLength);

                source.CopyTo(charsSpan);
                if (source.Length < charsSpan.Length)
                {
                    charsSpan[source.Length] = base64PadCharacter;
                    if (source.Length + 1 < charsSpan.Length)
                    {
                        charsSpan[source.Length + 1] = base64PadCharacter;
                    }
                }

                if (needReplace)
                {
                    Span<char> remaining = charsSpan;
                    int pos;
                    while ((pos = remaining.IndexOfAny(base64UrlCharacter62, base64UrlCharacter63)) >= 0)
                    {
                        remaining[pos] = (remaining[pos] == base64UrlCharacter62) ? base64Character62 : base64Character63;
                        remaining = remaining.Slice(pos + 1);
                    }
                }

                source = charsSpan;
            }

            byte[] arrayPoolBytes = null;
            Span<byte> bytesSpan = decodedLength <= StackAllocThreshold ?
                stackalloc byte[StackAllocThreshold] :
                arrayPoolBytes = ArrayPool<byte>.Shared.Rent(decodedLength);

            bool converted = Convert.TryFromBase64Chars(source, bytesSpan, out int bytesWritten);
            Debug.Assert(converted, "Expected TryFromBase64Chars to be successful");
            byte[] result = bytesSpan.Slice(0, bytesWritten).ToArray();

            if (arrayPoolBytes is not null)
            {
                bytesSpan.Clear();
                ArrayPool<byte>.Shared.Return(arrayPoolBytes);
            }

            if (arrayPoolChars is not null)
            {
                charsSpan.Clear();
                ArrayPool<char>.Shared.Return(arrayPoolChars);
            }

            return result;
#else
            if (needReplace)
            {
                string decodedString = new(char.MinValue, decodedLength);
                fixed (char* dest = decodedString)
                {
                    int i = 0;
                    for (; i < strSpan.Length; i++)
                    {
                        if (strSpan[i] == base64UrlCharacter62)
                            dest[i] = base64Character62;
                        else if (strSpan[i] == base64UrlCharacter63)
                            dest[i] = base64Character63;
                        else
                            dest[i] = strSpan[i];
                    }

                    for (; i < decodedLength; i++)
                        dest[i] = base64PadCharacter;
                }

                return Convert.FromBase64String(decodedString);
            }
            else
            {
                if (decodedLength == strSpan.Length)
                {
                    return Convert.FromBase64CharArray(strSpan.ToArray(), 0, strSpan.Length);
                }
                else
                {
                    string decodedString = new(char.MinValue, decodedLength);
                    fixed (char* src = strSpan)
                    fixed (char* dest = decodedString)
                    {
                        Buffer.MemoryCopy(src, dest, strSpan.Length * 2, strSpan.Length * 2);

                        dest[strSpan.Length] = base64PadCharacter;
                        if (strSpan.Length + 2 == decodedLength)
                            dest[strSpan.Length + 1] = base64PadCharacter;
                    }

                    return Convert.FromBase64String(decodedString);
                }
            }
#endif
        }

        /// <summary>
        /// Decodes the string from Base64UrlEncoded to UTF8.
        /// </summary>
        /// <param name="arg">string to decode.</param>
        /// <returns>UTF8 string.</returns>
        public static string Decode(string arg)
        {
            return Encoding.UTF8.GetString(DecodeBytes(arg));
        }
    }
}
