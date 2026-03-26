// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace NuGet.Packaging.Signing
{
    /// <summary>
    /// Helper to calculate CRC-32 for data.
    /// Derivative of a .NET core implementation - https://source.dot.net/#System.IO.Compression.Tests/Common/System/IO/Compression/CRC.cs
    /// This is public to allow testing.
    /// </summary>
    public static class Crc32
    {
        // Table of CRCs of all 8-bit messages.
        private static uint[] CrcLookUpTable = new uint[256];

        // Flag: has the table been computed? Initially false.
        private static bool CrcLookUptableComputed = false;

        // 0xedb88320 is the reversed form (least significant bit first) of the
        // CRC-32 polynomial 0x04c11db7 (most significant bit first).
        private const uint Crc32Polynomial = 0xedb88320;

        /// <summary>
        /// Calculates a 32 bit cyclic redundancy code for the input data.
        /// </summary>
        /// <param name="data">Byte[] of the data.</param>
        /// <returns>32 bit cyclic redundancy code for the input data in uint.</returns>
        [CLSCompliant(false)]
        public static uint CalculateCrc(byte[] data)
        {
            var crc = UpdateCrc(0xffffffff, data, data.Length);

            // post-invert the crc
            return crc ^ 0xffffffff;
        }

        // Update a running CRC with the bytes buf[0..len-1].
        // CRC should be initialized to all 1's.
        // The transmitted value should the 1's complement of the final running CRC.
        private static uint UpdateCrc(uint crc, byte[] buf, int len)
        {
            var c = crc;
            uint n;

            if (!CrcLookUptableComputed)
            {
                ComputeCrcLookUpTable();
            }
            for (n = 0; n < len; n++)
            {
                c = CrcLookUpTable[(c ^ buf[n]) & 0xff] ^ (c >> 8);
            }

            return c;
        }

        // Make the table for a fast CRC.
        // Derivative work of zlib -- https://github.com/madler/zlib/blob/master/crc32.c (hint: L108)
        private static void ComputeCrcLookUpTable()
        {
            uint c;
            uint n, k;

            for (n = 0; n < 256; n++)
            {
                c = n;
                for (k = 0; k < 8; k++)
                {
                    if ((c & 1) > 0)
                        c = Crc32Polynomial ^ (c >> 1);
                    else
                        c = c >> 1;
                }

                CrcLookUpTable[n] = c;
            }

            CrcLookUptableComputed = true;
        }
    }
}
