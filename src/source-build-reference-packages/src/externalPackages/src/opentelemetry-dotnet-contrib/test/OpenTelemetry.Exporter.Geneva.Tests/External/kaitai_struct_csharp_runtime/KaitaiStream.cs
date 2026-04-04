using System.IO.Compression;
using System.Globalization;

namespace Kaitai
{
    /// <summary>
    /// The base Kaitai stream which exposes an API for the Kaitai Struct framework.
    /// It's based off a <code>BinaryReader</code>, which is a little-endian reader.
    /// </summary>
    public partial class KaitaiStream : BinaryReader
    {
        #region Constructors

        public KaitaiStream(Stream stream) : base(stream)
        {
        }

        ///<summary>
        /// Creates a KaitaiStream backed by a file (RO)
        ///</summary>
        public KaitaiStream(string file) : base(File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
        }

        ///<summary>
        ///Creates a KaitaiStream backed by a byte buffer
        ///</summary>
        public KaitaiStream(byte[] bytes) : base(new MemoryStream(bytes))
        {
        }

        private int BitsLeft = 0;
        private ulong Bits = 0;

        static readonly bool IsLittleEndian = BitConverter.IsLittleEndian;

        #endregion

        #region Stream positioning

        /// <summary>
        /// Check if the stream position is at the end of the stream
        /// </summary>
        public bool IsEof
        {
            get { return BaseStream.Position >= BaseStream.Length && BitsLeft == 0; }
        }

        /// <summary>
        /// Seek to a specific position from the beginning of the stream
        /// </summary>
        /// <param name="position">The position to seek to</param>
        public void Seek(long position)
        {
            BaseStream.Seek(position, SeekOrigin.Begin);
        }

        /// <summary>
        /// Get the current position in the stream
        /// </summary>
        public long Pos
        {
            get { return BaseStream.Position; }
        }

        /// <summary>
        /// Get the total length of the stream (ie. file size)
        /// </summary>
        public long Size
        {
            get { return BaseStream.Length; }
        }

        #endregion

        #region Integer types

        #region Signed

        /// <summary>
        /// Read a signed byte from the stream
        /// </summary>
        /// <returns></returns>
        public sbyte ReadS1()
        {
            return ReadSByte();
        }

        #region Big-endian

        /// <summary>
        /// Read a signed short from the stream (big endian)
        /// </summary>
        /// <returns></returns>
        public short ReadS2be()
        {
            return BitConverter.ToInt16(ReadBytesNormalisedBigEndian(2), 0);
        }

        /// <summary>
        /// Read a signed int from the stream (big endian)
        /// </summary>
        /// <returns></returns>
        public int ReadS4be()
        {
            return BitConverter.ToInt32(ReadBytesNormalisedBigEndian(4), 0);
        }

        /// <summary>
        /// Read a signed long from the stream (big endian)
        /// </summary>
        /// <returns></returns>
        public long ReadS8be()
        {
            return BitConverter.ToInt64(ReadBytesNormalisedBigEndian(8), 0);
        }

        #endregion

        #region Little-endian

        /// <summary>
        /// Read a signed short from the stream (little endian)
        /// </summary>
        /// <returns></returns>
        public short ReadS2le()
        {
            return BitConverter.ToInt16(ReadBytesNormalisedLittleEndian(2), 0);
        }

        /// <summary>
        /// Read a signed int from the stream (little endian)
        /// </summary>
        /// <returns></returns>
        public int ReadS4le()
        {
            return BitConverter.ToInt32(ReadBytesNormalisedLittleEndian(4), 0);
        }

        /// <summary>
        /// Read a signed long from the stream (little endian)
        /// </summary>
        /// <returns></returns>
        public long ReadS8le()
        {
            return BitConverter.ToInt64(ReadBytesNormalisedLittleEndian(8), 0);
        }

        #endregion

        #endregion

        #region Unsigned

        /// <summary>
        /// Read an unsigned byte from the stream
        /// </summary>
        /// <returns></returns>
        public byte ReadU1()
        {
            return ReadByte();
        }

        #region Big-endian

        /// <summary>
        /// Read an unsigned short from the stream (big endian)
        /// </summary>
        /// <returns></returns>
        public ushort ReadU2be()
        {
            return BitConverter.ToUInt16(ReadBytesNormalisedBigEndian(2), 0);
        }

        /// <summary>
        /// Read an unsigned int from the stream (big endian)
        /// </summary>
        /// <returns></returns>
        public uint ReadU4be()
        {
            return BitConverter.ToUInt32(ReadBytesNormalisedBigEndian(4), 0);
        }

        /// <summary>
        /// Read an unsigned long from the stream (big endian)
        /// </summary>
        /// <returns></returns>
        public ulong ReadU8be()
        {
            return BitConverter.ToUInt64(ReadBytesNormalisedBigEndian(8), 0);
        }

        #endregion

        #region Little-endian

        /// <summary>
        /// Read an unsigned short from the stream (little endian)
        /// </summary>
        /// <returns></returns>
        public ushort ReadU2le()
        {
            return BitConverter.ToUInt16(ReadBytesNormalisedLittleEndian(2), 0);
        }

        /// <summary>
        /// Read an unsigned int from the stream (little endian)
        /// </summary>
        /// <returns></returns>
        public uint ReadU4le()
        {
            return BitConverter.ToUInt32(ReadBytesNormalisedLittleEndian(4), 0);
        }

        /// <summary>
        /// Read an unsigned long from the stream (little endian)
        /// </summary>
        /// <returns></returns>
        public ulong ReadU8le()
        {
            return BitConverter.ToUInt64(ReadBytesNormalisedLittleEndian(8), 0);
        }

        #endregion

        #endregion

        #endregion

        #region Floating point types

        #region Big-endian

        /// <summary>
        /// Read a single-precision floating point value from the stream (big endian)
        /// </summary>
        /// <returns></returns>
        public float ReadF4be()
        {
            return BitConverter.ToSingle(ReadBytesNormalisedBigEndian(4), 0);
        }

        /// <summary>
        /// Read a double-precision floating point value from the stream (big endian)
        /// </summary>
        /// <returns></returns>
        public double ReadF8be()
        {
            return BitConverter.ToDouble(ReadBytesNormalisedBigEndian(8), 0);
        }

        #endregion

        #region Little-endian

        /// <summary>
        /// Read a single-precision floating point value from the stream (little endian)
        /// </summary>
        /// <returns></returns>
        public float ReadF4le()
        {
            return BitConverter.ToSingle(ReadBytesNormalisedLittleEndian(4), 0);
        }

        /// <summary>
        /// Read a double-precision floating point value from the stream (little endian)
        /// </summary>
        /// <returns></returns>
        public double ReadF8le()
        {
            return BitConverter.ToDouble(ReadBytesNormalisedLittleEndian(8), 0);
        }

        #endregion

        #endregion

        #region Unaligned bit values

        public void AlignToByte()
        {
            BitsLeft = 0;
            Bits = 0;
        }

        /// <summary>
        /// Read a n-bit integer in a big-endian manner from the stream
        /// </summary>
        /// <returns></returns>
        public ulong ReadBitsIntBe(int n)
        {
            ulong res = 0;

            int bitsNeeded = n - BitsLeft;
            BitsLeft = -bitsNeeded & 7; // `-bitsNeeded mod 8`

            if (bitsNeeded > 0)
            {
                // 1 bit  => 1 byte
                // 8 bits => 1 byte
                // 9 bits => 2 bytes
                int bytesNeeded = ((bitsNeeded - 1) / 8) + 1; // `ceil(bitsNeeded / 8)`
                byte[] buf = ReadBytes(bytesNeeded);
                for (int i = 0; i < bytesNeeded; i++)
                {
                    res = res << 8 | buf[i];
                }

                ulong newBits = res;
                res = res >> BitsLeft | Bits << bitsNeeded;
                Bits = newBits; // will be masked at the end of the function
            }
            else
            {
                res = Bits >> -bitsNeeded; // shift unneeded bits out
            }

            ulong mask = (1UL << BitsLeft) - 1; // `BitsLeft` is in range 0..7, so `(1UL << 64)` does not have to be considered
            Bits &= mask;

            return res;
        }

        [Obsolete("use ReadBitsIntBe instead")]
        public ulong ReadBitsInt(int n)
        {
            return ReadBitsIntBe(n);
        }

        /// <summary>
        /// Read a n-bit integer in a little-endian manner from the stream
        /// </summary>
        /// <returns></returns>
        public ulong ReadBitsIntLe(int n)
        {
            ulong res = 0;
            int bitsNeeded = n - BitsLeft;

            if (bitsNeeded > 0)
            {
                // 1 bit  => 1 byte
                // 8 bits => 1 byte
                // 9 bits => 2 bytes
                int bytesNeeded = ((bitsNeeded - 1) / 8) + 1; // `ceil(bitsNeeded / 8)`
                byte[] buf = ReadBytes(bytesNeeded);
                for (int i = 0; i < bytesNeeded; i++)
                {
                    res |= ((ulong)buf[i]) << (i * 8);
                }

                // NB: in C#, bit shift operators on left-hand operand of type `ulong` work
                // as if the right-hand operand were subjected to `& 63` (`& 0b11_1111`) (see
                // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/bitwise-and-shift-operators#shift-count-of-the-shift-operators),
                // so `res >> 64` is equivalent to `res >> 0` (but we don't want that)
                ulong newBits = bitsNeeded < 64 ? res >> bitsNeeded : 0;
                res = res << BitsLeft | Bits;
                Bits = newBits;
            }
            else
            {
                res = Bits;
                Bits >>= n;
            }

            BitsLeft = -bitsNeeded & 7; // `-bitsNeeded mod 8`

            if (n < 64)
            {
                ulong mask = (1UL << n) - 1;
                res &= mask;
            }
            // if `n == 64`, do nothing
            return res;
        }

        #endregion

        #region Byte arrays

        /// <summary>
        /// Read a fixed number of bytes from the stream
        /// </summary>
        /// <param name="count">The number of bytes to read</param>
        /// <returns></returns>
        public byte[] ReadBytes(long count)
        {
            if (count < 0 || count > Int32.MaxValue)
                throw new ArgumentOutOfRangeException("requested " + count + " bytes, while only non-negative int32 amount of bytes possible");
            byte[] bytes = base.ReadBytes((int)count);
            if (bytes.Length < count)
                throw new EndOfStreamException("requested " + count + " bytes, but got only " + bytes.Length + " bytes");
            return bytes;
        }

        /// <summary>
        /// Read a fixed number of bytes from the stream
        /// </summary>
        /// <param name="count">The number of bytes to read</param>
        /// <returns></returns>
        public byte[] ReadBytes(ulong count)
        {
            if (count > Int32.MaxValue)
                throw new ArgumentOutOfRangeException("requested " + count + " bytes, while only non-negative int32 amount of bytes possible");
            byte[] bytes = base.ReadBytes((int)count);
            if (bytes.Length < (int)count)
                throw new EndOfStreamException("requested " + count + " bytes, but got only " + bytes.Length + " bytes");
            return bytes;
        }

        /// <summary>
        /// Read bytes from the stream in little endian format and convert them to the endianness of the current platform
        /// </summary>
        /// <param name="count">The number of bytes to read</param>
        /// <returns>An array of bytes that matches the endianness of the current platform</returns>
        protected byte[] ReadBytesNormalisedLittleEndian(int count)
        {
            byte[] bytes = ReadBytes(count);
            if (!IsLittleEndian) Array.Reverse(bytes);
            return bytes;
        }

        /// <summary>
        /// Read bytes from the stream in big endian format and convert them to the endianness of the current platform
        /// </summary>
        /// <param name="count">The number of bytes to read</param>
        /// <returns>An array of bytes that matches the endianness of the current platform</returns>
        protected byte[] ReadBytesNormalisedBigEndian(int count)
        {
            byte[] bytes = ReadBytes(count);
            if (IsLittleEndian) Array.Reverse(bytes);
            return bytes;
        }

        /// <summary>
        /// Read all the remaining bytes from the stream until the end is reached
        /// </summary>
        /// <returns></returns>
        public byte[] ReadBytesFull()
        {
            return ReadBytes(BaseStream.Length - BaseStream.Position);
        }

        /// <summary>
        /// Read a terminated string from the stream
        /// </summary>
        /// <param name="term">The string terminator value</param>
        /// <param name="includeTerm">True to include the terminator in the returned string</param>
        /// <param name="consumeTerm">True to consume the terminator byte before returning</param>
        /// <param name="eosError">True to throw an error when the EOS was reached before the terminator</param>
        /// <returns></returns>
        public byte[] ReadBytesTerm(byte term, bool includeTerm, bool consumeTerm, bool eosError)
        {
            // TODO: check if `System.IO.MemoryStream` would be a better choice than `List<byte>`
            List<byte> bytes = new List<byte>();
            while (true)
            {
                if (IsEof)
                {
                    if (eosError) throw new EndOfStreamException(string.Format("End of stream reached, but no terminator `{0}` found", term));
                    break;
                }

                byte b = ReadByte();
                if (b == term)
                {
                    if (includeTerm) bytes.Add(b);
                    if (!consumeTerm) Seek(Pos - 1);
                    break;
                }
                bytes.Add(b);
            }
            return bytes.ToArray();
        }

        public byte[] ReadBytesTermMulti(byte[] term, bool includeTerm, bool consumeTerm, bool eosError)
        {
            int unitSize = term.Length;
            // TODO: check if `System.IO.MemoryStream` would be a better choice than `List<byte>`
            List<byte> bytes = new List<byte>();
            while (true)
            {
                byte[] c = base.ReadBytes(unitSize);
                if (c.Length < unitSize)
                {
                    if (eosError) throw new EndOfStreamException(string.Format("End of stream reached, but no terminator `{0}` found", term));

                    bytes.AddRange(c);
                    break;
                }
                if (ByteArrayCompare(c, term) == 0)
                {
                    if (includeTerm) bytes.AddRange(c);
                    if (!consumeTerm) Seek(Pos - unitSize);
                    break;
                }
                bytes.AddRange(c);
            }
            return bytes.ToArray();
        }

        /// <summary>
        /// Read a specific set of bytes and assert that they are the same as an expected result
        /// </summary>
        /// <param name="expected">The expected result</param>
        /// <returns></returns>
        [Obsolete("use explicit \"if\" using ByteArrayCompare method instead")]
        public byte[] EnsureFixedContents(byte[] expected)
        {
            byte[] bytes = ReadBytes(expected.Length);

            if (bytes.Length != expected.Length)
            {
                throw new Exception(string.Format("Expected bytes: {0} ({1} bytes), Instead got: {2} ({3} bytes)", Convert.ToBase64String(expected), expected.Length, Convert.ToBase64String(bytes), bytes.Length));
            }
            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] != expected[i])
                {
                    throw new Exception(string.Format("Expected bytes: {0} ({1} bytes), Instead got: {2} ({3} bytes)", Convert.ToBase64String(expected), expected.Length, Convert.ToBase64String(bytes), bytes.Length));
                }
            }

            return bytes;
        }

        public static byte[] BytesStripRight(byte[] src, byte padByte)
        {
            int newLen = src.Length;
            while (newLen > 0 && src[newLen - 1] == padByte)
                newLen--;

            byte[] dst = new byte[newLen];
            Array.Copy(src, dst, newLen);
            return dst;
        }

        public static byte[] BytesTerminate(byte[] src, byte term, bool includeTerm)
        {
            int newLen = 0;
            int maxLen = src.Length;

            while (newLen < maxLen && src[newLen] != term)
                newLen++;

            if (includeTerm && newLen < maxLen)
                newLen++;

            byte[] dst = new byte[newLen];
            Array.Copy(src, dst, newLen);
            return dst;
        }

        public static byte[] BytesTerminateMulti(byte[] src, byte[] term, bool includeTerm)
        {
            int unitSize = term.Length;
            if (unitSize == 0)
            {
                return new byte[0];
            }
            int newLen = src.Length;
            int iTerm = 0;
            for (int iSrc = 0; iSrc < src.Length;)
            {
                if (src[iSrc] != term[iTerm])
                {
                    iSrc += unitSize - iTerm;
                    iTerm = 0;
                    continue;
                }
                iSrc++;
                iTerm++;
                if (iTerm == unitSize)
                {
                    newLen = iSrc - (includeTerm ? 0 : unitSize);
                    break;
                }
            }
            byte[] dst = new byte[newLen];
            Array.Copy(src, dst, newLen);
            return dst;
        }

        #endregion

        #region Byte array processing

        /// <summary>
        /// Performs XOR processing with given data, XORing every byte of the input with a single value.
        /// </summary>
        /// <param name="value">The data toe process</param>
        /// <param name="key">The key value to XOR with</param>
        /// <returns>Processed data</returns>
        public byte[] ProcessXor(byte[] value, int key)
        {
            byte[] result = new byte[value.Length];
            for (int i = 0; i < value.Length; i++)
            {
                result[i] = (byte)(value[i] ^ key);
            }
            return result;
        }

        /// <summary>
        /// Performs XOR processing with given data, XORing every byte of the input with a key
        /// array, repeating from the beginning of the key array if necessary
        /// </summary>
        /// <param name="value">The data toe process</param>
        /// <param name="key">The key array to XOR with</param>
        /// <returns>Processed data</returns>
        public byte[] ProcessXor(byte[] value, byte[] key)
        {
            int keyLen = key.Length;
            byte[] result = new byte[value.Length];
            for (int i = 0, j = 0; i < value.Length; i++, j = (j + 1) % keyLen)
            {
                result[i] = (byte)(value[i] ^ key[j]);
            }
            return result;
        }

        /// <summary>
        /// Performs a circular left rotation shift for a given buffer by a given amount of bits.
        /// Pass a negative amount to rotate right.
        /// </summary>
        /// <param name="data">The data to rotate</param>
        /// <param name="amount">The number of bytes to rotate by</param>
        /// <param name="groupSize"></param>
        /// <returns></returns>
        public byte[] ProcessRotateLeft(byte[] data, int amount, int groupSize)
        {
            if (amount > 7 || amount < -7) throw new ArgumentException("Rotation of more than 7 cannot be performed.", "amount");
            if (amount < 0) amount += 8; // Rotation of -2 is the same as rotation of +6

            byte[] r = new byte[data.Length];
            switch (groupSize)
            {
                case 1:
                    for (int i = 0; i < data.Length; i++)
                    {
                        byte bits = data[i];
                        // http://stackoverflow.com/a/812039
                        r[i] = (byte)((bits << amount) | (bits >> (8 - amount)));
                    }
                    break;
                default:
                    throw new NotImplementedException(string.Format("Unable to rotate a group of {0} bytes yet", groupSize));
            }
            return r;
        }

        /// <summary>
        /// Inflates a deflated zlib byte stream
        /// </summary>
        /// <param name="data">The data to deflate</param>
        /// <returns>The deflated result</returns>
        public byte[] ProcessZlib(byte[] data)
        {
            // See RFC 1950 (https://tools.ietf.org/html/rfc1950)
            // zlib adds a header to DEFLATE streams - usually 2 bytes,
            // but can be 6 bytes if FDICT is set.
            // There's also 4 checksum bytes at the end of the stream.

            byte zlibCmf = data[0];
            if ((zlibCmf & 0x0F) != 0x08) throw new NotSupportedException("Only the DEFLATE algorithm is supported for zlib data.");

            const int zlibFooter = 4;
            int zlibHeader = 2;

            // If the FDICT bit (0x20) is 1, then the 4-byte dictionary is included in the header, we need to skip it
            byte zlibFlg = data[1];
            if ((zlibFlg & 0x20) == 0x20) zlibHeader += 4;

            using (MemoryStream ms = new MemoryStream(data, zlibHeader, data.Length - (zlibHeader + zlibFooter)))
            {
                using (DeflateStream ds = new DeflateStream(ms, CompressionMode.Decompress))
                {
                    using (MemoryStream target = new MemoryStream())
                    {
                        ds.CopyTo(target);
                        return target.ToArray();
                    }
                }
            }
        }

        #endregion

        #region Misc utility methods

        /// <summary>
        /// Performs modulo operation between two integers.
        /// </summary>
        /// <remarks>
        /// This method is required because C# lacks a "true" modulo
        /// operator, the % operator rather being the "remainder"
        /// operator. We want mod operations to always be positive.
        /// </remarks>
        /// <param name="a">The value to be divided</param>
        /// <param name="b">The value to divide by. Must be greater than zero.</param>
        /// <returns>The result of the modulo opertion. Will always be positive.</returns>
        public static int Mod(int a, int b)
        {
            if (b <= 0) throw new ArgumentException("Divisor of mod operation must be greater than zero.", "b");
            int r = a % b;
            if (r < 0) r += b;
            return r;
        }

        /// <summary>
        /// Performs modulo operation between two integers.
        /// </summary>
        /// <remarks>
        /// This method is required because C# lacks a "true" modulo
        /// operator, the % operator rather being the "remainder"
        /// operator. We want mod operations to always be positive.
        /// </remarks>
        /// <param name="a">The value to be divided</param>
        /// <param name="b">The value to divide by. Must be greater than zero.</param>
        /// <returns>The result of the modulo opertion. Will always be positive.</returns>
        public static long Mod(long a, long b)
        {
            if (b <= 0) throw new ArgumentException("Divisor of mod operation must be greater than zero.", "b");
            long r = a % b;
            if (r < 0) r += b;
            return r;
        }

        /// <summary>
        /// Compares two byte arrays in lexicographical order.
        /// </summary>
        /// <returns>negative number if a is less than b, <c>0</c> if a is equal to b, positive number if a is greater than b.</returns>
        /// <param name="a">First byte array to compare</param>
        /// <param name="b">Second byte array to compare.</param>
        public static int ByteArrayCompare(byte[] a, byte[] b)
        {
            if (a == b)
                return 0;
            int al = a.Length;
            int bl = b.Length;
            int minLen = al < bl ? al : bl;
            for (int i = 0; i < minLen; i++)
            {
                int cmp = a[i] - b[i];
                if (cmp != 0)
                    return cmp;
            }

            // Reached the end of at least one of the arrays
            if (al == bl)
            {
                return 0;
            }
            else
            {
                return al - bl;
            }
        }

        /// <summary>
        /// Reverses the string, Unicode-aware.
        /// </summary>
        /// <a href="https://stackoverflow.com/a/15029493">taken from here</a>
        public static string StringReverse(string s)
        {
            TextElementEnumerator enumerator = StringInfo.GetTextElementEnumerator(s);

            List<string> elements = new List<string>();
            while (enumerator.MoveNext())
                elements.Add(enumerator.GetTextElement());

            elements.Reverse();
            return string.Concat(elements);
        }

        #endregion
    }
}
