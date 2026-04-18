// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Buffers;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Base64 encode/decode implementation for as per https://tools.ietf.org/html/rfc4648#section-5.
    /// Uses ArrayPool[T] to minimize memory usage.
    /// </summary>
    internal static class Base64UrlEncoding
    {
        private const uint IntA = 'A';
        private const uint IntZ = 'Z';
        private const uint Inta = 'a';
        private const uint Intz = 'z';
        private const uint Int0 = '0';
        private const uint Int9 = '9';
        private const uint IntEq = '=';
        private const uint IntPlus = '+';
        private const uint IntMinus = '-';
        private const uint IntSlash = '/';
        private const uint IntUnderscore = '_';

        private static readonly char[] Base64Table =
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O',
            'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd',
            'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's',
            't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7',
            '8', '9', '-', '_',
        };

        /// <summary>
        /// Decodes a base64url encoded string into a byte array.
        /// </summary>
        /// <param name="inputString">The base64url encoded string to decode.</param>
        /// <returns>The decoded bytes.</returns>
        public static byte[] Decode(string inputString)
        {
            _ = inputString ?? throw LogHelper.LogArgumentNullException(nameof(inputString));

            return Decode(inputString, 0, inputString.Length);
        }

        /// <summary>
        /// Decodes a base64url encoded substring of a string into a byte array.
        /// </summary>
        /// <param name="input">The base64url encoded string to decode.</param>
        /// <param name="offset">The index of the character in <paramref name="input"/> to start decoding.</param>
        /// <param name="length">The number of characters in <paramref name="input"/> to decode.</param>
        /// <returns>The decoded bytes.</returns>
        public static byte[] Decode(string input, int offset, int length)
        {
            _ = input ?? throw LogHelper.LogArgumentNullException(nameof(input));

            ReadOnlySpan<char> inputSpan = input.AsSpan();
            int outputsize = ValidateAndGetOutputSize(inputSpan, offset, length);
            byte[] output = new byte[outputsize];
            Decode(inputSpan, offset, length, output);
            return output;
        }

        /// <summary>
        /// Decodes a base64url encoded substring of a string and then performs an action on the decoded bytes.
        /// </summary>
        /// <typeparam name="T">The output type of the decoding action.</typeparam>
        /// <typeparam name="TX">The type of the input parameter to the action.</typeparam>
        /// <param name="input">The base64url encoded string to decode.</param>
        /// <param name="offset">The index of the character in <paramref name="input"/> to start decoding.</param>
        /// <param name="length">The number of characters in <paramref name="input"/> to decode from <paramref name="offset"/>.</param>
        /// <param name="argx">The input parameter to the action.</param>
        /// <param name="action">The action to perform on the decoded bytes.</param>
        /// <returns>An instance of {T}.</returns>
        /// <remarks>
        /// The buffer for the decode operation uses a shared memory pool to avoid allocations.
        /// The length of the rented array of bytes may be larger than the decoded bytes; therefore, the action needs to know the actual length to use.
        /// The result of <see cref="ValidateAndGetOutputSize(ReadOnlySpan{char}, int, int)"/> is passed to the action.
        /// </remarks>
        public static T Decode<T, TX>(string input, int offset, int length, TX argx, Func<byte[], int, TX, T> action)
        {
            _ = action ?? throw new ArgumentNullException(nameof(action));

            ReadOnlySpan<char> inputSpan = input.AsSpan();
            int outputsize = ValidateAndGetOutputSize(inputSpan, offset, length);
            byte[] output = ArrayPool<byte>.Shared.Rent(outputsize);
            try
            {
                Decode(inputSpan, offset, length, output);
                return action(output, outputsize, argx);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(output);
            }
        }

        /// <summary>
        /// Decodes a base64url encoded substring of a string and then performs an action on the decoded bytes.
        /// </summary>
        /// <typeparam name="T">The return type of the operation.</typeparam>
        /// <param name="input">The base64url encoded string to decode.</param>
        /// <param name="offset">The index of the character in <paramref name="input"/> to start decoding from.</param>
        /// <param name="length">The number of characters in <paramref name="input"/> to decode from <paramref name="offset"/>.</param>
        /// <param name="action">The action to perform on the decoded bytes.</param>
        /// <returns>An instance of {T}.</returns>
        /// <remarks>
        /// The buffer for the decode operation uses a shared memory pool to avoid allocations.
        /// The length of the rented array of bytes may be larger than the decoded bytes; therefore, the action needs to know the actual length to use.
        /// The result of <see cref="ValidateAndGetOutputSize(ReadOnlySpan{char}, int, int)"/> is passed to the action.
        /// </remarks>
        public static T Decode<T>(string input, int offset, int length, Func<byte[], int, T> action)
        {
            _ = input ?? throw LogHelper.LogArgumentNullException(nameof(input));
            _ = action ?? throw new ArgumentNullException(nameof(action));

            ReadOnlySpan<char> inputSpan = input.AsSpan();
            int outputsize = ValidateAndGetOutputSize(inputSpan, offset, length);
            byte[] output = ArrayPool<byte>.Shared.Rent(outputsize);
            try
            {
                Decode(inputSpan, offset, length, output);
                return action(output, outputsize);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(output);
            }
        }

        /// <summary>
        /// Decodes a base64url encoded substring of a string and then performs an action on the decoded bytes.
        /// </summary>
        /// <typeparam name="T">The return type of the operation.</typeparam>
        /// <typeparam name="TX">The type of input parameter 1 to the action.</typeparam>
        /// <typeparam name="TY">The type of input parameter 2 to the action.</typeparam>
        /// <typeparam name="TZ">The type of input parameter 3 to the action.</typeparam>
        /// <param name="input">The base64url encoded string to decode.</param>
        /// <param name="offset">The index of the character in <paramref name="input"/> to start decoding from.</param>
        /// <param name="length">The number of characters in <paramref name="input"/> to decode from <paramref name="offset"/>.</param>
        /// <param name="argx">Input parameter 1 to the action.</param>
        /// <param name="argy">Input parameter 2 to the action.</param>
        /// <param name="argz">Input parameter 3 to the action.</param>
        /// <param name="action">The action to perform on the decoded bytes.</param>
        /// <returns>An instance of {T}.</returns>
        /// <remarks>
        /// The buffer for the decode operation uses a shared memory pool to avoid allocations.
        /// The length of the rented array of bytes may be larger than the decoded bytes; therefore, the action needs to know the actual length to use.
        /// The result of <see cref="ValidateAndGetOutputSize(ReadOnlySpan{char}, int, int)"/> is passed to the action.
        /// </remarks>
        public static T Decode<T, TX, TY, TZ>(
            string input,
            int offset,
            int length,
            TX argx,
            TY argy,
            TZ argz,
            Func<byte[], int, TX, TY, TZ, T> action)
        {
            _ = action ?? throw LogHelper.LogArgumentNullException(nameof(action));

            ReadOnlySpan<char> inputSpan = input.AsSpan();
            int outputsize = ValidateAndGetOutputSize(inputSpan, offset, length);
            byte[] output = ArrayPool<byte>.Shared.Rent(outputsize);
            try
            {
                Decode(inputSpan, offset, length, output);
                return action(output, outputsize, argx, argy, argz);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(output);
            }
        }

        /// <summary>
        /// Decodes a Base64Url encoded substring of a string into a byte array.
        /// </summary>
        /// <param name="input">The string represented as a span to decode.</param>
        /// <param name="offset">The index of the character in <paramref name="input"/> to start decoding from.</param>
        /// <param name="length">The number of characters beginning from <paramref name="offset"/> to decode.</param>
        /// <param name="output">The byte array to place the decoded results into.</param>
        /// <remarks>
        /// Changes from Base64UrlEncoder implementation:
        /// 1. Padding is optional.
        /// 2. '+' and '-' are treated the same.
        /// 3. '/' and '_' are treated the same.
        /// </remarks>
        internal static void Decode(ReadOnlySpan<char> input, int offset, int length, byte[] output)
        {
            int outputpos = 0;
            uint curblock = 0x000000FFu;
            for (int i = offset; i < (offset + length); i++)
            {
                uint cur = input[i];
                if (cur >= IntA && cur <= IntZ)
                {
                    cur -= IntA;
                }
                else if (cur >= Inta && cur <= Intz)
                {
                    cur = (cur - Inta) + 26u;
                }
                else if (cur >= Int0 && cur <= Int9)
                {
                    cur = (cur - Int0) + 52u;
                }
                else if (cur == IntPlus || cur == IntMinus)
                {
                    cur = 62u;
                }
                else if (cur == IntSlash || cur == IntUnderscore)
                {
                    cur = 63u;
                }
                else if (cur == IntEq)
                {
                    continue;
                }
                else
                {
                    throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(
                        LogHelper.FormatInvariant(
                            LogMessages.IDX10820,
                            LogHelper.MarkAsNonPII(cur),
                            input.ToString())));
                }

                curblock = (curblock << 6) | cur;

                // check if 4 characters have been read, based on number of shifts.
                if ((0xFF000000u & curblock) == 0xFF000000u)
                {
                    output[outputpos++] = (byte)(curblock >> 16);
                    output[outputpos++] = (byte)(curblock >> 8);
                    output[outputpos++] = (byte)curblock;
                    curblock = 0x000000FFu;
                }
            }

            // Handle spill over characters. This accounts for case where padding character is not present.
            if (curblock != 0x000000FFu)
            {
                if ((0x03FC0000u & curblock) == 0x03FC0000u)
                {
                    // shifted 3 times, 1 padding character, 2 output characters
                    curblock <<= 6;
                    output[outputpos++] = (byte)(curblock >> 16);
                    output[outputpos++] = (byte)(curblock >> 8);
                }
                else if ((0x000FF000u & curblock) == 0x000FF000u)
                {
                    // shifted 2 times, 2 padding character, 1 output character
                    curblock <<= 12;
                    output[outputpos++] = (byte)(curblock >> 16);
                }
                else
                {
                    throw LogHelper.LogExceptionMessage(new ArgumentException(
                        LogHelper.FormatInvariant(LogMessages.IDX10821, input.ToString())));
                }
            }
        }

        /// <summary>
        /// Encodes a byte array into a base64url encoded string.
        /// </summary>
        /// <param name="bytes">The bytes to encode.</param>
        /// <returns>base64url encoded string.</returns>
        public static string Encode(byte[] bytes)
        {
            _ = bytes ?? throw LogHelper.LogArgumentNullException(nameof(bytes));
            return Encode(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Encodes a subset of a byte array into a Base64Url encoded string.
        /// </summary>
        /// <param name="input">The byte array to encode.</param>
        /// <param name="offset">The index into <paramref name="input"/> to start the encode operation.</param>
        /// <param name="length">The number of bytes in <paramref name="input"/> to encode, starting from <paramref name="offset"/>.</param>
        /// <returns>Base64Url encoded string.</returns>
        public static string Encode(byte[] input, int offset, int length)
        {
            _ = input ?? throw LogHelper.LogArgumentNullException(nameof(input));

            if (length == 0)
                return string.Empty;

            if (length < 0)
                throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(
                    nameof(length),
                    LogHelper.FormatInvariant(
                    LogMessages.IDX10716,
                    LogHelper.MarkAsNonPII(nameof(length)),
                    LogHelper.MarkAsNonPII(length))));

            if (offset < 0)
                throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(
                    nameof(offset),
                    LogHelper.FormatInvariant(
                        LogMessages.IDX10716,
                        LogHelper.MarkAsNonPII(nameof(offset)),
                        LogHelper.MarkAsNonPII(offset))));

            if (input.Length < offset + length)
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
                throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(
                    "offset + length",
                    LogHelper.FormatInvariant(
                        LogMessages.IDX10717,
                        LogHelper.MarkAsNonPII(nameof(offset)),
                        LogHelper.MarkAsNonPII(nameof(length)),
                        LogHelper.MarkAsNonPII(nameof(input)),
                        LogHelper.MarkAsNonPII(offset),
                        LogHelper.MarkAsNonPII(length),
                        LogHelper.MarkAsNonPII(input.Length))));
#pragma warning restore CA2208 // Instantiate argument exceptions correctly

            int outputsize = length % 3;
            if (outputsize > 0)
                outputsize++;

            outputsize += (length / 3) * 4;

            char[] output = new char[outputsize];
            WriteEncodedOutput(input, offset, length, output);
            return new string(output);
        }

        /// <summary>
        /// Validates the input span for a decode operation.
        /// </summary>
        /// <param name="strSpan">The string represented by a span to validate.</param>
        /// <param name="offset">The index of the character in <paramref name="strSpan"/> to start the decode operation.</param>
        /// <param name="length">The number of characters in <paramref name="strSpan"/> to decode, starting from <paramref name="offset"/>.</param>
        /// <returns>The size of the decoded bytes array.</returns>
        internal static int ValidateAndGetOutputSize(ReadOnlySpan<char> strSpan, int offset, int length)
        {
            if (strSpan.IsEmpty)
                throw LogHelper.LogArgumentNullException(nameof(strSpan));

            if (length == 0)
                return 0;

            if (offset < 0)
                throw LogHelper.LogExceptionMessage(new ArgumentException(
                    LogHelper.FormatInvariant(
                        LogMessages.IDX10716,
                        LogHelper.MarkAsNonPII(nameof(offset)),
                        LogHelper.MarkAsNonPII(offset))));

            if (length < 0)
                throw LogHelper.LogExceptionMessage(new ArgumentException(
                    LogHelper.FormatInvariant(
                        LogMessages.IDX10716,
                        LogHelper.MarkAsNonPII(nameof(length)),
                        LogHelper.MarkAsNonPII(length))));

            if (length + offset > strSpan.Length)
                throw LogHelper.LogExceptionMessage(new ArgumentException(
                    LogHelper.FormatInvariant(
                        LogMessages.IDX10717,
                        LogHelper.MarkAsNonPII(nameof(length)),
                        LogHelper.MarkAsNonPII(nameof(offset)),
                        LogHelper.MarkAsNonPII(nameof(strSpan)),
                        LogHelper.MarkAsNonPII(length),
                        LogHelper.MarkAsNonPII(offset),
                        LogHelper.MarkAsNonPII(strSpan.Length))));

            if (length % 4 == 1)
                throw LogHelper.LogExceptionMessage(new FormatException(LogHelper.FormatInvariant(LogMessages.IDX10400, strSpan.ToString())));

            int lastCharPosition = offset + length - 1;

            // Compute useful length (i.e. ignore padding characters)
            if (strSpan[lastCharPosition] == '=')
            {
                lastCharPosition--;
                if (strSpan[lastCharPosition] == '=')
                    lastCharPosition--;
            }

            int effectiveLength = 1 + (lastCharPosition - offset);
            int outputSize = effectiveLength % 4;
            if (outputSize > 0)
                outputSize--;

            outputSize += (effectiveLength / 4) * 3;
            return outputSize;
        }

        private static void WriteEncodedOutput(byte[] inputBytes, int offset, int length, Span<char> output)
        {
            uint curBlock = 0x000000FFu;
            int outputPointer = 0;

            for (int i = offset; i < offset + length; i++)
            {
                curBlock = (curBlock << 8) | inputBytes[i];

                if ((curBlock & 0xFF000000u) == 0xFF000000u)
                {
                    output[outputPointer++] = Base64Table[(curBlock & 0x00FC0000u) >> 18];
                    output[outputPointer++] = Base64Table[(curBlock & 0x00030000u | curBlock & 0x0000F000u) >> 12];
                    output[outputPointer++] = Base64Table[(curBlock & 0x00000F00u | curBlock & 0x000000C0u) >> 6];
                    output[outputPointer++] = Base64Table[curBlock & 0x0000003Fu];

                    curBlock = 0x000000FFu;
                }
            }

            if ((curBlock & 0x00FF0000u) == 0x00FF0000u)
            {
                // 2 shifts, 3 output characters.
                output[outputPointer++] = Base64Table[(curBlock & 0x0000FC00u) >> 10];
                output[outputPointer++] = Base64Table[(curBlock & 0x000003F0u) >> 4];
                output[outputPointer++] = Base64Table[(curBlock & 0x0000000Fu) << 2];
            }
            else if ((curBlock & 0x0000FF00u) == 0x0000FF00u)
            {
                // 1 shift, 2 output characters.
                output[outputPointer++] = Base64Table[(curBlock & 0x000000FCu) >> 2];
                output[outputPointer++] = Base64Table[(curBlock & 0x00000003u) << 4];
            }
        }
    }
}
