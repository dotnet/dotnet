// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
//
// (C) 2002, 2003 Motus Technologies Inc. (http://www.motus.com)
// Copyright (C) 2004 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using Xunit;

namespace System.Security.Cryptography.Tests
{
    // PKCS1MaskGenerationMethod is annotated as RequiresUnreferencedCode
    [ConditionalClass(typeof(PlatformDetection), nameof(PlatformDetection.IsNotBuiltWithAggressiveTrimming))]
    [SkipOnPlatform(TestPlatforms.Browser, "Not supported on Browser")]
    public class PKCS1MaskGenerationMethodTest
    {
        [Fact]
        public static void PropertyTest()
        {
            PKCS1MaskGenerationMethod pkcs1 = new PKCS1MaskGenerationMethod();
            Assert.Equal("SHA1", pkcs1.HashName);

            pkcs1.HashName = "MD5";
            Assert.Equal("MD5", pkcs1.HashName);

            pkcs1.HashName = null;
            Assert.Equal("SHA1", pkcs1.HashName);
        }

        [Fact]
        public static void EmptyMaskTest()
        {
            PKCS1MaskGenerationMethod pkcs1 = new PKCS1MaskGenerationMethod();
            byte[] random = { 0x01 };
            byte[] mask = pkcs1.GenerateMask(random, 0);
            Assert.Equal(0, mask.Length);
        }

        [Fact]
        public static void NullSeedTest()
        {
            PKCS1MaskGenerationMethod pkcs1 = new PKCS1MaskGenerationMethod();
            Assert.Throws<NullReferenceException>(() => pkcs1.GenerateMask(null, 10));
        }

        [Fact]
        public static void NegativeReturnParameterTest()
        {
            PKCS1MaskGenerationMethod pkcs1 = new PKCS1MaskGenerationMethod();
            byte[] random = { 0x01 };
            Assert.Throws<OverflowException>(() => pkcs1.GenerateMask(random, -1));
        }

        [Theory]
        [InlineData("DoesntExist")]
        [InlineData("DSA")]
        public static void GenerateMask_InvalidHashName_Throws(string hashName)
        {
            PKCS1MaskGenerationMethod pkcs1 = new PKCS1MaskGenerationMethod();
            pkcs1.HashName = hashName;
            Assert.Throws<CryptographicException>(() => pkcs1.GenerateMask(new byte[] { 0 }, 1));
        }

        [Fact]
        public static void GenerateMaskTest_SHA1()
        {
            PKCS1MaskGenerationMethod pkcs1 = new PKCS1MaskGenerationMethod();

            byte[] seed = { 0xaa, 0xfd, 0x12, 0xf6, 0x59, 0xca, 0xe6, 0x34, 0x89, 0xb4, 0x79, 0xe5, 0x07, 0x6d, 0xde, 0xc2, 0xf0, 0x6c, 0xb5, 0x8f };
            int LengthDB = 107;

            byte[] expectedDBMask = { 0x06, 0xe1, 0xde, 0xb2, 0x36, 0x9a, 0xa5, 0xa5, 0xc7, 0x07, 0xd8, 0x2c, 0x8e, 0x4e, 0x93, 0x24,
                0x8a, 0xc7, 0x83, 0xde, 0xe0, 0xb2, 0xc0, 0x46, 0x26, 0xf5, 0xaf, 0xf9, 0x3e, 0xdc, 0xfb, 0x25,
                0xc9, 0xc2, 0xb3, 0xff, 0x8a, 0xe1, 0x0e, 0x83, 0x9a, 0x2d, 0xdb, 0x4c, 0xdc, 0xfe, 0x4f, 0xf4,
                0x77, 0x28, 0xb4, 0xa1, 0xb7, 0xc1, 0x36, 0x2b, 0xaa, 0xd2, 0x9a, 0xb4, 0x8d, 0x28, 0x69, 0xd5,
                0x02, 0x41, 0x21, 0x43, 0x58, 0x11, 0x59, 0x1b, 0xe3, 0x92, 0xf9, 0x82, 0xfb, 0x3e, 0x87, 0xd0,
                0x95, 0xae, 0xb4, 0x04, 0x48, 0xdb, 0x97, 0x2f, 0x3a, 0xc1, 0x4e, 0xaf, 0xf4, 0x9c, 0x8c, 0x3b,
                0x7c, 0xfc, 0x95, 0x1a, 0x51, 0xec, 0xd1, 0xdd, 0xe6, 0x12, 0x64 };
            byte[] dbMask = pkcs1.GenerateMask(seed, LengthDB);

            Assert.Equal(expectedDBMask, dbMask);

            byte[] DB = { 0xda, 0x39, 0xa3, 0xee, 0x5e, 0x6b, 0x4b, 0x0d, 0x32, 0x55, 0xbf, 0xef, 0x95, 0x60, 0x18, 0x90,
                0xaf, 0xd8, 0x07, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0xd4, 0x36, 0xe9, 0x95, 0x69,
                0xfd, 0x32, 0xa7, 0xc8, 0xa0, 0x5b, 0xbc, 0x90, 0xd3, 0x2c, 0x49 };
            byte[] maskedDB = new byte[dbMask.Length];
            for (int i = 0; i < dbMask.Length; i++)
            {
                maskedDB[i] = (byte)(DB[i] ^ dbMask[i]);
            }

            byte[] seedMask = pkcs1.GenerateMask(maskedDB, seed.Length);
            byte[] expectedSeedMask = { 0x41, 0x87, 0x0b, 0x5a, 0xb0, 0x29, 0xe6, 0x57, 0xd9, 0x57, 0x50, 0xb5, 0x4c, 0x28, 0x3c, 0x08, 0x72, 0x5d, 0xbe, 0xa9 };

            Assert.Equal(expectedSeedMask, seedMask);
        }

        [Fact]
        public static void GenerateMaskTest_MD5()
        {
            PKCS1MaskGenerationMethod pkcs1 = new PKCS1MaskGenerationMethod();
            pkcs1.HashName = "MD5";

            byte[] seed = { 0xaa, 0xfd, 0x12, 0xf6, 0x59, 0xca, 0xe6, 0x34, 0x89, 0xb4, 0x79, 0xe5, 0x07, 0x6d, 0xde, 0xc2, 0xf0, 0x6c, 0xb5, 0x8f };
            int LengthDB = 107;

            byte[] expectedDBMask = { 0xF2, 0x3F, 0x56, 0x5C, 0xE3, 0x7D, 0x61, 0x6E, 0x9E, 0x39, 0x22, 0x4F, 0x8C, 0x67,
                0xE8, 0xFE, 0xBD, 0xD0, 0x30, 0xC2, 0x3B, 0x81, 0x6A, 0x1B, 0x9B, 0xB1, 0xC1, 0xEE, 0x13, 0xBF,
                0x48, 0x72, 0x8, 0xD2, 0x5A, 0xE3, 0x76, 0xAD, 0x53, 0x6F, 0xED, 0x25, 0x24, 0x66, 0xB3, 0xBF,
                0xE9, 0x6B, 0x53, 0xFA, 0x66, 0x3, 0x3, 0xE4, 0x77, 0xD, 0xE3, 0xF0, 0x4B, 0xBB, 0xF3, 0x93,
                0x7E, 0x5C, 0x98, 0x35, 0x2A, 0x53, 0xCE, 0xEE, 0xDC, 0x6B, 0xDC, 0x9B, 0xEB, 0xD9, 0xC1, 0xA,
                0x87, 0x17, 0x83, 0x26, 0x4B, 0x87, 0x23, 0xD2, 0xA9, 0x23, 0xDD, 0x1E, 0x6B, 0x38, 0x67, 0x37,
                0xED, 0x9A, 0xD1, 0x1A, 0x20, 0xE6, 0x6A, 0xAB, 0xDE, 0xFD, 0x3E, 0x35, 0x42 };
            byte[] dbMask = pkcs1.GenerateMask(seed, LengthDB);

            Assert.Equal(expectedDBMask, dbMask);

            byte[] DB = { 0xda, 0x39, 0xa3, 0xee, 0x5e, 0x6b, 0x4b, 0x0d, 0x32, 0x55, 0xbf, 0xef, 0x95, 0x60, 0x18, 0x90,
                0xaf, 0xd8, 0x07, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0xd4, 0x36, 0xe9, 0x95, 0x69,
                0xfd, 0x32, 0xa7, 0xc8, 0xa0, 0x5b, 0xbc, 0x90, 0xd3, 0x2c, 0x49 };
            byte[] maskedDB = new byte[dbMask.Length];
            for (int i = 0; i < dbMask.Length; i++)
            {
                maskedDB[i] = (byte)(DB[i] ^ dbMask[i]);
            }

            byte[] seedMask = pkcs1.GenerateMask(maskedDB, seed.Length);
            byte[] expectedSeedMask = { 0x2F, 0xC1, 0x35, 0x69, 0x61, 0x71, 0x74, 0x10, 0x72, 0x5F, 0xE6, 0x7, 0x2B, 0xB3, 0x4A, 0x1D, 0xEF, 0xC6, 0x48, 0x20 };

            Assert.Equal(expectedSeedMask, seedMask);
        }
    }
}
