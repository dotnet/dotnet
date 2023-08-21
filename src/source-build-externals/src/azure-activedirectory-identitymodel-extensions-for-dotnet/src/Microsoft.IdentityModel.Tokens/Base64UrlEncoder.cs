// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
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
        /// Encoding table
        /// </summary>
        internal static readonly char[] s_base64Table =
        {
            'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
            'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
            '0','1','2','3','4','5','6','7','8','9',
            base64UrlCharacter62,
            base64UrlCharacter63
        };

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
                    LogHelper.FormatInvariant(
                        LogMessages.IDX10716,
                        LogHelper.MarkAsNonPII(nameof(offset)),
                        LogHelper.MarkAsNonPII(offset))));

            if (length == 0)
                return string.Empty;

            if (length < 0)
                throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(
                    LogHelper.FormatInvariant(
                        LogMessages.IDX10716,
                        LogHelper.MarkAsNonPII(nameof(length)),
                        LogHelper.MarkAsNonPII(length))));

            if (inArray.Length < offset + length)
                throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(
                    LogHelper.FormatInvariant(
                        LogMessages.IDX10717,
                        LogHelper.MarkAsNonPII(nameof(offset)),
                        LogHelper.MarkAsNonPII(nameof(length)),
                        LogHelper.MarkAsNonPII(nameof(inArray)),
                        LogHelper.MarkAsNonPII(offset),
                        LogHelper.MarkAsNonPII(length),
                        LogHelper.MarkAsNonPII(inArray.Length))));

            int lengthmod3 = length % 3;
            int limit = offset + (length - lengthmod3);
            char[] output = new char[(length + 2) / 3 * 4];
            char[] table = s_base64Table;
            int i, j = 0;

            // takes 3 bytes from inArray and insert 4 bytes into output
            for (i = offset; i < limit; i += 3)
            {
                byte d0 = inArray[i];
                byte d1 = inArray[i + 1];
                byte d2 = inArray[i + 2];

                output[j + 0] = table[d0 >> 2];
                output[j + 1] = table[((d0 & 0x03) << 4) | (d1 >> 4)];
                output[j + 2] = table[((d1 & 0x0f) << 2) | (d2 >> 6)];
                output[j + 3] = table[d2 & 0x3f];
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

                        output[j + 0] = table[d0 >> 2];
                        output[j + 1] = table[((d0 & 0x03) << 4) | (d1 >> 4)];
                        output[j + 2] = table[(d1 & 0x0f) << 2];
                        j += 3;
                    }
                    break;

                case 1:
                    {
                        byte d0 = inArray[i];

                        output[j + 0] = table[d0 >> 2];
                        output[j + 1] = table[(d0 & 0x03) << 4];
                        j += 2;
                    }
                    break;

                //default or case 0: no further operations are needed.
            }

            return new string(output, 0, j);
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

        internal static string EncodeString(string str)
        {
            _ = str ?? throw LogHelper.LogArgumentNullException(nameof(str));

            return Encode(Encoding.UTF8.GetBytes(str));
        }

        /// <summary>
        /// Converts the specified string, base-64-url encoded to utf8 bytes.</summary>
        /// <param name="str">base64Url encoded string.</param>
        /// <returns>UTF8 bytes.</returns>
        public static byte[] DecodeBytes(string str)
        {
            _ = str ?? throw LogHelper.LogExceptionMessage(new ArgumentNullException(nameof(str)));
            return UnsafeDecode(str);
        }

        internal static unsafe byte[] UnsafeDecode(string str)
        {
            int mod = str.Length % 4;
            if (mod == 1)
                throw LogHelper.LogExceptionMessage(new FormatException(LogHelper.FormatInvariant(LogMessages.IDX10400, str)));

            bool needReplace = false;
            int decodedLength = str.Length + (4 - mod) % 4;

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == base64UrlCharacter62 || str[i] == base64UrlCharacter63)
                {
                    needReplace = true;
                    break;
                }
            }

            if (needReplace)
            {
                string decodedString = new(char.MinValue, decodedLength);
                fixed (char* dest = decodedString)
                {
                    int i = 0;
                    for (; i < str.Length; i++)
                    {
                        if (str[i] == base64UrlCharacter62)
                            dest[i] = base64Character62;
                        else if (str[i] == base64UrlCharacter63)
                            dest[i] = base64Character63;
                        else
                            dest[i] = str[i];
                    }

                    for (; i < decodedLength; i++)
                        dest[i] = base64PadCharacter;
                }

                return Convert.FromBase64String(decodedString);
            }
            else
            {
                if (decodedLength == str.Length)
                {
                    return Convert.FromBase64String(str);
                }
                else
                {
                    string decodedString = new(char.MinValue, decodedLength);
                    fixed (char* src = str)
                    fixed (char* dest = decodedString)
                    {
                        Buffer.MemoryCopy(src, dest, str.Length * 2, str.Length * 2);

                        dest[str.Length] = base64PadCharacter;
                        if (str.Length + 2 == decodedLength)
                            dest[str.Length + 1] = base64PadCharacter;
                    }

                    return Convert.FromBase64String(decodedString);
                }
            }
        }

        internal static unsafe byte[] UnsafeDecode(char[] str)
        {
            int mod = str.Length % 4;
            if (mod == 1)
                throw LogHelper.LogExceptionMessage(new FormatException(LogHelper.FormatInvariant(LogMessages.IDX10400, str)));

            bool needReplace = false;
            // the decoded length 
            int decodedLength = str.Length + (4 - mod) % 4;

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == base64UrlCharacter62 || str[i] == base64UrlCharacter63)
                {
                    needReplace = true;
                    break;
                }
            }

            if (needReplace)
            {
                string decodedString = new(char.MinValue, decodedLength);
                fixed (char* dest = decodedString)
                {
                    int i = 0;
                    for (; i < str.Length; i++)
                    {
                        if (str[i] == base64UrlCharacter62)
                            dest[i] = base64Character62;
                        else if (str[i] == base64UrlCharacter63)
                            dest[i] = base64Character63;
                        else
                            dest[i] = str[i];
                    }

                    for (; i < decodedLength; i++)
                        dest[i] = base64PadCharacter;
                }

                return Convert.FromBase64String(decodedString);
            }
            else
            {
                if (decodedLength == str.Length)
                {
                    return Convert.FromBase64CharArray(str, 0, str.Length);
                }
                else
                {
                    string decodedString = new(char.MinValue, decodedLength);
                    fixed (char* src = str)
                    fixed (char* dest = decodedString)
                    {
                        Buffer.MemoryCopy(src, dest, str.Length * 2, str.Length * 2);

                        dest[str.Length] = base64PadCharacter;
                        if (str.Length + 2 == decodedLength)
                            dest[str.Length + 1] = base64PadCharacter;
                    }

                    return Convert.FromBase64String(decodedString);
                }
            }
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
