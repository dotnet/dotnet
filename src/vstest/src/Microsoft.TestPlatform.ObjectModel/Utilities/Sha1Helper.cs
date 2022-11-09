// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Security.Cryptography;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;

/// <summary>
/// Used to calculate SHA1 hash.
///
/// https://tools.ietf.org/html/rfc3174
/// </summary>
internal static class Sha1Helper
{
    public static byte[] ComputeSha1(byte[] message)
    {
        using HashAlgorithm provider = SHA1.Create();
        byte[] hash = provider.ComputeHash(message);

        return hash;
    }

    /// <summary>
    /// SHA-1 Implementation as in https://tools.ietf.org/html/rfc3174
    /// </summary>
    /// <remarks>
    /// This implementation only works with messages with a length
    /// that is a multiple of the size of 8-bits.
    /// </remarks>
    internal class Sha1Implementation
    {
        /*
         * Many of the variable, function and parameter names in this code
         * were used because those were the names used in the publication.
         *
         * For more information please refer to https://tools.ietf.org/html/rfc3174.
         */

        private const int BlockBits = 512;
        private const int DigestBits = 160;
        private const int BlockBytes = BlockBits / 8;
        private const int DigestBytes = DigestBits / 8;

        /// <summary>
        /// A sequence of logical functions to be used in SHA-1.
        /// Each f(t), 0 <= t <= 79, operates on three 32-bit words B, C, D and produces a 32-bit word as output.
        /// </summary>
        /// <param name="t">Function index. 0 <= t <= 79</param>
        /// <param name="b">Word B</param>
        /// <param name="c">Word C</param>
        /// <param name="d">Word D</param>
        /// <returns>
        /// f(t;B,C,D) = (B AND C) OR ((NOT B) AND D)         ( 0 <= t <= 19)
        /// f(t;B,C,D) = B XOR C XOR D                        (20 <= t <= 39)
        /// f(t;B,C,D) = (B AND C) OR (B AND D) OR (C AND D)  (40 <= t <= 59)
        /// f(t;B,C,D) = B XOR C XOR D                        (60 <= t <= 79)
        /// </returns>
        private static uint F(int t, uint b, uint c, uint d)
        {
            return t switch
            {
                >= 0 and <= 19 => b & c | ~b & d,
                >= 20 and <= 39 or >= 60 and <= 79 => b ^ c ^ d,
                _ => t is >= 40 and <= 59
                    ? b & c | b & d | c & d
                    : throw new ArgumentException("Argument out of bounds! 0 <= t < 80", nameof(t))
            };
        }

        /// <summary>
        /// Returns a constant word K(t) which is used in the SHA-1.
        /// </summary>
        /// <param name="t">Word index.</param>
        /// <returns>
        /// K(t) = 0x5A827999 ( 0 <= t <= 19)
        /// K(t) = 0x6ED9EBA1 (20 <= t <= 39)
        /// K(t) = 0x8F1BBCDC (40 <= t <= 59)
        /// K(t) = 0xCA62C1D6 (60 <= t <= 79)
        /// </returns>
        private static uint K(int t)
        {
            return t switch
            {
                >= 0 and <= 19 => 0x5A827999u,
                >= 20 and <= 39 => 0x6ED9EBA1u,
                >= 40 and <= 59 => 0x8F1BBCDCu,
                _ => t is >= 60 and <= 79
                    ? 0xCA62C1D6u
                    : throw new ArgumentException("Argument out of bounds! 0 <= t < 80", nameof(t))
            };
        }

        /// <summary>
        /// The circular left shift operation.
        /// </summary>
        /// <param name="x">An uint word.</param>
        /// <param name="n">0 <= n < 32</param>
        /// <returns>S^n(X)  =  (X << n) OR (X >> 32-n)</returns>
        private static uint S(uint x, byte n)
        {
            return n > 32 ? throw new ArgumentOutOfRangeException(nameof(n)) : (x << n) | (x >> (32 - n));
        }

        /// <summary>
        /// Ensures that given bytes are in big endian notation.
        /// </summary>
        /// <param name="array">An array of bytes</param>
        private static void EnsureBigEndian(ref byte[] array)
        {
            ValidateArg.NotNull(array, nameof(array));

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(array);
            }
        }

        private readonly uint[] _h = new uint[5];

        private void Reset()
        {
            // as defined in https://tools.ietf.org/html/rfc3174#section-6.1
            _h[0] = 0x67452301u;
            _h[1] = 0xEFCDAB89u;
            _h[2] = 0x98BADCFEu;
            _h[3] = 0x10325476u;
            _h[4] = 0xC3D2E1F0u;
        }

        public byte[] ComputeHash(byte[] message)
        {
            ValidateArg.NotNull(message, nameof(message));

            Reset();
            PadMessage(ref message);

            var messageCount = message.Length / BlockBytes;
            for (var i = 0; i < messageCount; ++i)
            {
                ProcessBlock(message, i * BlockBytes, BlockBytes);
            }

            var digest = new byte[DigestBytes];
            for (int t = 0; t < _h.Length; t++)
            {
                var hi = BitConverter.GetBytes(_h[t]);
                EnsureBigEndian(ref hi);

                Buffer.BlockCopy(hi, 0, digest, t * hi.Length, hi.Length);
            }

            return digest;
        }

        private static void PadMessage(ref byte[] message)
        {
            var length = message.Length;
            var paddingBytes = BlockBytes - (length % BlockBytes);

            // 64bit uint message size will be appended to end of the padding, making sure we have space for it.
            if (paddingBytes <= 8)
                paddingBytes += BlockBytes;

            var padding = new byte[paddingBytes];
            padding[0] = 0b10000000;

            var messageBits = (ulong)message.Length << 3;
            var messageSize = BitConverter.GetBytes(messageBits);
            EnsureBigEndian(ref messageSize);

            Buffer.BlockCopy(messageSize, 0, padding, padding.Length - messageSize.Length, messageSize.Length);

            Array.Resize(ref message, message.Length + padding.Length);
            Buffer.BlockCopy(padding, 0, message, length, padding.Length);
        }

        private void ProcessBlock(byte[] message, int start, int length)
        {
            if (start + length > message.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }
            if (length != BlockBytes)
            {
                throw new ArgumentException($"Invalid block size. Actual: {length}, Expected: {BlockBytes}", nameof(length));
            }

            var w = new uint[80];

            // Get W(0) .. W(15)
            for (int t = 0; t <= 15; t++)
            {
                var wordBytes = new byte[sizeof(uint)];
                Buffer.BlockCopy(message, start + (t * sizeof(uint)), wordBytes, 0, sizeof(uint));
                EnsureBigEndian(ref wordBytes);

                w[t] = BitConverter.ToUInt32(wordBytes, 0);
            }

            // Calculate W(16) .. W(79)
            for (int t = 16; t <= 79; t++)
            {
                w[t] = S(w[t - 3] ^ w[t - 8] ^ w[t - 14] ^ w[t - 16], 1);
            }

            uint a = _h[0],
                b = _h[1],
                c = _h[2],
                d = _h[3],
                e = _h[4];

            for (int t = 0; t < 80; t++)
            {
                var temp = S(a, 5) + F(t, b, c, d) + e + w[t] + K(t);
                e = d;
                d = c;
                c = S(b, 30);
                b = a;
                a = temp;
            }

            _h[0] += a;
            _h[1] += b;
            _h[2] += c;
            _h[3] += d;
            _h[4] += e;
        }
    }
}
