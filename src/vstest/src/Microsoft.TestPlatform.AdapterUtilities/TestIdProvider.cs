// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;

namespace Microsoft.TestPlatform.AdapterUtilities;

/// <summary>
/// Used to generate id for tests.
/// </summary>
public class TestIdProvider
{
    internal const int BlockBits = 512;
    internal const int DigestBits = 160;
    internal const int BlockBytes = BlockBits / 8;
    internal const int DigestBytes = DigestBits / 8;

    private Guid _id = Guid.Empty;
    private byte[]? _hash;
    private byte[] _lastBlock = new byte[BlockBytes];
    private int _position;

    private readonly Sha1Implementation _hasher;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestIdProvider"/> class.
    /// </summary>
    public TestIdProvider()
    {
        _hasher = new Sha1Implementation();
    }

    /// <summary>
    /// Appends a string to id generation seed.
    /// </summary>
    /// <param name="str">String to append to the id seed.</param>
    /// <exception cref="InvalidOperationException">Thrown if <see cref="GetHash"/> or <see cref="GetId"/> is called already.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="str"/> is <c>null</c>.</exception>
    public void AppendString(string str)
    {
        if (_hash != null)
        {
            throw new InvalidOperationException(Resources.Resources.ErrorCannotAppendAfterHashCalculation);
        }
        _ = str ?? throw new ArgumentNullException(nameof(str));

        var bytes = Encoding.Unicode.GetBytes(str);

        AppendBytes(bytes);
    }

    /// <summary>
    /// Appends an array of bytes to id generation seed.
    /// </summary>
    /// <param name="bytes">Array to append to the id seed.</param>
    /// <exception cref="InvalidOperationException">Thrown if <see cref="GetHash"/> or <see cref="GetId"/> is called already.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="bytes"/> is <c>null</c>.</exception>
    public void AppendBytes(byte[] bytes)
    {
        if (_hash != null)
        {
            throw new InvalidOperationException(Resources.Resources.ErrorCannotAppendAfterHashCalculation);
        }
        _ = bytes ?? throw new ArgumentNullException(nameof(bytes));

        if (bytes.Length == 0)
        {
            return;
        }

        var end = Math.Min(BlockBytes - _position, bytes.Length);

        Buffer.BlockCopy(bytes, 0, _lastBlock, _position, end);

        // Block length is not reached yet.
        if (end + _position < BlockBytes)
        {
            _position += end;
            return;
        }

        _hasher.ProcessBlock(_lastBlock, 0, _lastBlock.Length);
        _position = 0;

        // We processed the entire string already
        if (end == bytes.Length)
        {
            return;
        }

        int start = 0;
        while (end < bytes.Length)
        {
            start = end;
            end += BlockBytes;
            if (end > bytes.Length)
            {
                break;
            }

            _hasher.ProcessBlock(bytes, start, end - start);
        }

        if (end > bytes.Length)
        {
            _position = bytes.Length - start;
            Buffer.BlockCopy(bytes, start, _lastBlock, 0, _position);
        }
    }

    /// <summary>
    /// Calculates the Id seed.
    /// </summary>
    /// <returns>An array containing the seed.</returns>
    /// <remarks>
    /// <see cref="AppendBytes(byte[])"/> and <see cref="AppendString(string)"/> cannot be called
    /// on instance after this method is called.
    /// </remarks>
    public byte[] GetHash()
    {
        if (_hash != null)
        {
            return _hash;
        }

        if (_position != 0)
        {
            _hasher.PadMessage(ref _lastBlock, _position);
            _hasher.ProcessBlock(_lastBlock, 0, _lastBlock.Length);
        }

        _hash = _hasher.ProcessFinalBlock();

        return _hash;
    }

    /// <summary>
    /// Calculates the Id from the seed.
    /// </summary>
    /// <returns>Id</returns>
    /// <remarks>
    /// <see cref="AppendBytes(byte[])"/> and <see cref="AppendString(string)"/> cannot be called
    /// on instance after this method is called.
    /// </remarks>
    public Guid GetId()
    {
        if (_id != Guid.Empty)
        {
            return _id;
        }

        var toGuid = new byte[16];
        Array.Copy(GetHash(), toGuid, 16);
        _id = new Guid(toGuid);

        return _id;
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

        private int _streamSize;
        private bool _messagePadded;

        public Sha1Implementation()
        {
            Reset();
        }

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
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(array);
            }
        }

        private readonly uint[] _h = new uint[5];

        private void Reset()
        {
            _streamSize = 0;
            _messagePadded = false;

            // as defined in https://tools.ietf.org/html/rfc3174#section-6.1
            _h[0] = 0x67452301u;
            _h[1] = 0xEFCDAB89u;
            _h[2] = 0x98BADCFEu;
            _h[3] = 0x10325476u;
            _h[4] = 0xC3D2E1F0u;
        }

        public byte[] ComputeHash(byte[] message)
        {
            Reset();
            _streamSize = 0;
            PadMessage(ref message);

            ProcessBlock(message, 0, message.Length);

            return ProcessFinalBlock();
        }

        private void ProcessMultipleBlocks(byte[] message)
        {
            var messageCount = message.Length / BlockBytes;
            for (var i = 0; i < messageCount; i += 1)
            {
                ProcessBlock(message, i * BlockBytes, BlockBytes);
            }
        }

        public byte[] ProcessFinalBlock()
        {
            if (!_messagePadded)
            {
                var pad = new byte[0];
                PadMessage(ref pad, 0);
                ProcessBlock(pad, 0, pad.Length);
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

        public void PadMessage(ref byte[] message, int length = 0)
        {
            if (_messagePadded)
            {
                throw new InvalidOperationException();
            }

            if (length == 0)
            {
                length = message.Length;
            }
            else
            {
                Array.Resize(ref message, length);
            }

            _streamSize += length;

            var paddingBytes = BlockBytes - (length % BlockBytes);

            // 64bit uint message size will be appended to end of the padding, making sure we have space for it.
            if (paddingBytes <= 8)
                paddingBytes += BlockBytes;

            var padding = new byte[paddingBytes];
            padding[0] = 0b10000000;

            var messageBits = (ulong)_streamSize << 3;
            var messageSize = BitConverter.GetBytes(messageBits);
            EnsureBigEndian(ref messageSize);

            Buffer.BlockCopy(messageSize, 0, padding, padding.Length - messageSize.Length, messageSize.Length);

            Array.Resize(ref message, message.Length + padding.Length);
            Buffer.BlockCopy(padding, 0, message, length, padding.Length);

            _messagePadded = true;
        }

        public void ProcessBlock(byte[] message, int start, int length)
        {
            if (start + length > message.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }
            if (length % BlockBytes != 0)
            {
                throw new ArgumentException($"Invalid block size. Actual: {length}, Expected: Multiples of {BlockBytes}", nameof(length));
            }
            if (length != BlockBytes)
            {
                ProcessMultipleBlocks(message);
                return;
            }

            _streamSize += BlockBytes;
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
