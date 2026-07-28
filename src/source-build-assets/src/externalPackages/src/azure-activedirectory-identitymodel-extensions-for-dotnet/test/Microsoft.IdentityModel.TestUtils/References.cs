// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.TestUtils
{
    // https://datatracker.ietf.org/doc/html/rfc7518#appendix-A.3
    // B.1.  Test Cases for AES_128_CBC_HMAC_SHA_256
    public static class AES_128_CBC_HMAC_SHA_256
    {
        public static string Algorithm
        {
            get { return SecurityAlgorithms.Aes128CbcHmacSha256; }
        }

        public static byte[] K
        {
            get
            {
                return new byte[]{ 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,
                                   0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f };
            }
        }

        public static byte[] MAC_KEY
        {
            get
            {
                return new byte[] { 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xa, 0xb, 0xc, 0xd, 0xe, 0xf };
            }
        }

        public static byte[] ENC_KEY
        {
            get
            {
                return new byte[] { 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f };
            }
        }

        public static byte[] P
        {
            get
            {
                return new byte[] { 0x41, 0x20, 0x63, 0x69, 0x70, 0x68, 0x65, 0x72, 0x20, 0x73, 0x79, 0x73, 0x74, 0x65, 0x6d, 0x20,
                                    0x6d, 0x75, 0x73, 0x74, 0x20, 0x6e, 0x6f, 0x74, 0x20, 0x62, 0x65, 0x20, 0x72, 0x65, 0x71, 0x75,
                                    0x69, 0x72, 0x65, 0x64, 0x20, 0x74, 0x6f, 0x20, 0x62, 0x65, 0x20, 0x73, 0x65, 0x63, 0x72, 0x65,
                                    0x74, 0x2c, 0x20, 0x61, 0x6e, 0x64, 0x20, 0x69, 0x74, 0x20, 0x6d, 0x75, 0x73, 0x74, 0x20, 0x62,
                                    0x65, 0x20, 0x61, 0x62, 0x6c, 0x65, 0x20, 0x74, 0x6f, 0x20, 0x66, 0x61, 0x6c, 0x6c, 0x20, 0x69,
                                    0x6e, 0x74, 0x6f, 0x20, 0x74, 0x68, 0x65, 0x20, 0x68, 0x61, 0x6e, 0x64, 0x73, 0x20, 0x6f, 0x66,
                                    0x20, 0x74, 0x68, 0x65, 0x20, 0x65, 0x6e, 0x65, 0x6d, 0x79, 0x20, 0x77, 0x69, 0x74, 0x68, 0x6f,
                                    0x75, 0x74, 0x20, 0x69, 0x6e, 0x63, 0x6f, 0x6e, 0x76, 0x65, 0x6e, 0x69, 0x65, 0x6e, 0x63, 0x65
                };
            }
        }

        public static byte[] IV
        {
            get
            {
                return new byte[] { 0x1a, 0xf3, 0x8c, 0x2d, 0xc2, 0xb9, 0x6f, 0xfd, 0xd8, 0x66, 0x94, 0x09, 0x23, 0x41, 0xbc, 0x04 };
            }
        }

        public static byte[] A
        {
            get
            {
                return new byte[] { 0x54, 0x68, 0x65, 0x20, 0x73, 0x65, 0x63, 0x6f, 0x6e, 0x64, 0x20, 0x70, 0x72, 0x69, 0x6e, 0x63,
                                    0x69, 0x70, 0x6c, 0x65, 0x20, 0x6f, 0x66, 0x20, 0x41, 0x75, 0x67, 0x75, 0x73, 0x74, 0x65, 0x20,
                                    0x4b, 0x65, 0x72, 0x63, 0x6b, 0x68, 0x6f, 0x66, 0x66, 0x73 };
            }
        }

        public static byte[] AL
        {
            get
            {
                return new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x50 };
            }
        }

        public static byte[] E
        {
            get
            {
                return new byte[] { 0xc8, 0x0e, 0xdf, 0xa3, 0x2d, 0xdf, 0x39, 0xd5, 0xef, 0x00, 0xc0, 0xb4, 0x68, 0x83, 0x42, 0x79,
                                    0xa2, 0xe4, 0x6a, 0x1b, 0x80, 0x49, 0xf7, 0x92, 0xf7, 0x6b, 0xfe, 0x54, 0xb9, 0x03, 0xa9, 0xc9,
                                    0xa9, 0x4a, 0xc9, 0xb4, 0x7a, 0xd2, 0x65, 0x5c, 0x5f, 0x10, 0xf9, 0xae, 0xf7, 0x14, 0x27, 0xe2,
                                    0xfc, 0x6f, 0x9b, 0x3f, 0x39, 0x9a, 0x22, 0x14, 0x89, 0xf1, 0x63, 0x62, 0xc7, 0x03, 0x23, 0x36,
                                    0x09, 0xd4, 0x5a, 0xc6, 0x98, 0x64, 0xe3, 0x32, 0x1c, 0xf8, 0x29, 0x35, 0xac, 0x40, 0x96, 0xc8,
                                    0x6e, 0x13, 0x33, 0x14, 0xc5, 0x40, 0x19, 0xe8, 0xca, 0x79, 0x80, 0xdf, 0xa4, 0xb9, 0xcf, 0x1b,
                                    0x38, 0x4c, 0x48, 0x6f, 0x3a, 0x54, 0xc5, 0x10, 0x78, 0x15, 0x8e, 0xe5, 0xd7, 0x9d, 0xe5, 0x9f,
                                    0xbd, 0x34, 0xd8, 0x48, 0xb3, 0xd6, 0x95, 0x50, 0xa6, 0x76, 0x46, 0x34, 0x44, 0x27, 0xad, 0xe5,
                                    0x4b, 0x88, 0x51, 0xff, 0xb5, 0x98, 0xf7, 0xf8, 0x00, 0x74, 0xb9, 0x47, 0x3c, 0x82, 0xe2, 0xdb };
            }
        }

        public static byte[] M
        {
            get
            {
                return new byte[] { 0x65, 0x2c, 0x3f, 0xa3, 0x6b, 0x0a, 0x7c, 0x5b, 0x32, 0x19, 0xfa, 0xb3, 0xa3, 0x0b, 0xc1, 0xc4,
                                    0xe6, 0xe5, 0x45, 0x82, 0x47, 0x65, 0x15, 0xf0, 0xad, 0x9f, 0x75, 0xa2, 0xb7, 0x1c, 0x73, 0xef };
            }
        }

        public static byte[] T
        {
            get
            {
                return new byte[] { 0x65, 0x2c, 0x3f, 0xa3, 0x6b, 0x0a, 0x7c, 0x5b, 0x32, 0x19, 0xfa, 0xb3, 0xa3, 0x0b, 0xc1, 0xc4 };
            }
        }
    }

    // https://datatracker.ietf.org/doc/html/rfc7518#appendix-A.3
    // B.2.  Test Cases for AES_192_CBC_HMAC_SHA_256
    public static class AES_192_CBC_HMAC_SHA_384
    {
        public static string Algorithm
        {
            get { return SecurityAlgorithms.Aes192CbcHmacSha384; }
        }

        public static byte[] K
        {
            get
            {
                return new byte[]{ 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,
                                   0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f,
                                   0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2a, 0x2b, 0x2c, 0x2d, 0x2e, 0x2f };
            }
        }

        public static byte[] MAC_KEY
        {
            get
            {
                return new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,
                                    0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17 };
            }
        }

        public static byte[] ENC_KEY
        {
            get
            {
                return new byte[] { 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f, 0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27,
                                    0x28, 0x29, 0x2a, 0x2b, 0x2c, 0x2d, 0x2e, 0x2f };
            }
        }

        public static byte[] P
        {
            get
            {
                return new byte[] { 0x41, 0x20, 0x63, 0x69, 0x70, 0x68, 0x65, 0x72, 0x20, 0x73, 0x79, 0x73, 0x74, 0x65, 0x6d, 0x20,
                                    0x6d, 0x75, 0x73, 0x74, 0x20, 0x6e, 0x6f, 0x74, 0x20, 0x62, 0x65, 0x20, 0x72, 0x65, 0x71, 0x75,
                                    0x69, 0x72, 0x65, 0x64, 0x20, 0x74, 0x6f, 0x20, 0x62, 0x65, 0x20, 0x73, 0x65, 0x63, 0x72, 0x65,
                                    0x74, 0x2c, 0x20, 0x61, 0x6e, 0x64, 0x20, 0x69, 0x74, 0x20, 0x6d, 0x75, 0x73, 0x74, 0x20, 0x62,
                                    0x65, 0x20, 0x61, 0x62, 0x6c, 0x65, 0x20, 0x74, 0x6f, 0x20, 0x66, 0x61, 0x6c, 0x6c, 0x20, 0x69,
                                    0x6e, 0x74, 0x6f, 0x20, 0x74, 0x68, 0x65, 0x20, 0x68, 0x61, 0x6e, 0x64, 0x73, 0x20, 0x6f, 0x66,
                                    0x20, 0x74, 0x68, 0x65, 0x20, 0x65, 0x6e, 0x65, 0x6d, 0x79, 0x20, 0x77, 0x69, 0x74, 0x68, 0x6f,
                                    0x75, 0x74, 0x20, 0x69, 0x6e, 0x63, 0x6f, 0x6e, 0x76, 0x65, 0x6e, 0x69, 0x65, 0x6e, 0x63, 0x65 };
            }
        }

        public static byte[] IV
        {
            get
            {
                return new byte[] { 0x1a, 0xf3, 0x8c, 0x2d, 0xc2, 0xb9, 0x6f, 0xfd, 0xd8, 0x66, 0x94, 0x09, 0x23, 0x41, 0xbc, 0x04 };
            }
        }

        public static byte[] A
        {
            get
            {
                return new byte[] { 0x54, 0x68, 0x65, 0x20, 0x73, 0x65, 0x63, 0x6f, 0x6e, 0x64, 0x20, 0x70, 0x72, 0x69, 0x6e, 0x63,
                                    0x69, 0x70, 0x6c, 0x65, 0x20, 0x6f, 0x66, 0x20, 0x41, 0x75, 0x67, 0x75, 0x73, 0x74, 0x65, 0x20,
                                    0x4b, 0x65, 0x72, 0x63, 0x6b, 0x68, 0x6f, 0x66, 0x66, 0x73 };
            }
        }

        public static byte[] AL
        {
            get
            {
                return new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x50 };
            }
        }

        public static byte[] E
        {
            get
            {
                return new byte[] { 0xea, 0x65, 0xda, 0x6b, 0x59, 0xe6, 0x1e, 0xdb, 0x41, 0x9b, 0xe6, 0x2d, 0x19, 0x71, 0x2a, 0xe5,
                                    0xd3, 0x03, 0xee, 0xb5, 0x00, 0x52, 0xd0, 0xdf, 0xd6, 0x69, 0x7f, 0x77, 0x22, 0x4c, 0x8e, 0xdb,
                                    0x00, 0x0d, 0x27, 0x9b, 0xdc, 0x14, 0xc1, 0x07, 0x26, 0x54, 0xbd, 0x30, 0x94, 0x42, 0x30, 0xc6,
                                    0x57, 0xbe, 0xd4, 0xca, 0x0c, 0x9f, 0x4a, 0x84, 0x66, 0xf2, 0x2b, 0x22, 0x6d, 0x17, 0x46, 0x21,
                                    0x4b, 0xf8, 0xcf, 0xc2, 0x40, 0x0a, 0xdd, 0x9f, 0x51, 0x26, 0xe4, 0x79, 0x66, 0x3f, 0xc9, 0x0b,
                                    0x3b, 0xed, 0x78, 0x7a, 0x2f, 0x0f, 0xfc, 0xbf, 0x39, 0x04, 0xbe, 0x2a, 0x64, 0x1d, 0x5c, 0x21,
                                    0x05, 0xbf, 0xe5, 0x91, 0xba, 0xe2, 0x3b, 0x1d, 0x74, 0x49, 0xe5, 0x32, 0xee, 0xf6, 0x0a, 0x9a,
                                    0xc8, 0xbb, 0x6c, 0x6b, 0x01, 0xd3, 0x5d, 0x49, 0x78, 0x7b, 0xcd, 0x57, 0xef, 0x48, 0x49, 0x27,
                                    0xf2, 0x80, 0xad, 0xc9, 0x1a, 0xc0, 0xc4, 0xe7, 0x9c, 0x7b, 0x11, 0xef, 0xc6, 0x00, 0x54, 0xe3 };
            }
        }

        public static byte[] M
        {
            get
            {
                return new byte[] { 0x84, 0x90, 0xac, 0x0e, 0x58, 0x94, 0x9b, 0xfe, 0x51, 0x87, 0x5d, 0x73, 0x3f, 0x93, 0xac, 0x20,
                                    0x75, 0x16, 0x80, 0x39, 0xcc, 0xc7, 0x33, 0xd7, 0x45, 0x94, 0xf8, 0x86, 0xb3, 0xfa, 0xaf, 0xd4,
                                    0x86, 0xf2, 0x5c, 0x71, 0x31, 0xe3, 0x28, 0x1e, 0x36, 0xc7, 0xa2, 0xd1, 0x30, 0xaf, 0xde, 0x57 };
            }
        }

        public static byte[] T
        {
            get
            {
                return new byte[] { 0x84, 0x90, 0xac, 0x0e, 0x58, 0x94, 0x9b, 0xfe, 0x51, 0x87, 0x5d, 0x73, 0x3f, 0x93, 0xac, 0x20,
                                    0x75, 0x16, 0x80, 0x39, 0xcc, 0xc7, 0x33, 0xd7 };
            }
        }
    }

    // https://datatracker.ietf.org/doc/html/rfc7518#appendix-A.3
    // B.3.  Test Cases for AES_256_CBC_HMAC_SHA_512
    public static class AES_256_CBC_HMAC_SHA_512
    {
        public static string Algorithm
        {
            get { return SecurityAlgorithms.Aes256CbcHmacSha512; }
        }

        public static byte[] K
        {
            get
            {
                return new byte[]{ 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,
                                   0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f,
                                   0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2a, 0x2b, 0x2c, 0x2d, 0x2e, 0x2f,
                                   0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f };
            }
        }

        public static byte[] MAC_KEY
        {
            get
            {
                return new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,
                                    0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f };
            }
        }

        public static byte[] ENC_KEY
        {
            get
            {
                return new byte[] { 0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2a, 0x2b, 0x2c, 0x2d, 0x2e, 0x2f,
                                    0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f };
            }
        }

        public static byte[] P
        {
            get
            {
                return new byte[] { 0x41, 0x20, 0x63, 0x69, 0x70, 0x68, 0x65, 0x72, 0x20, 0x73, 0x79, 0x73, 0x74, 0x65, 0x6d, 0x20,
                                    0x6d, 0x75, 0x73, 0x74, 0x20, 0x6e, 0x6f, 0x74, 0x20, 0x62, 0x65, 0x20, 0x72, 0x65, 0x71, 0x75,
                                    0x69, 0x72, 0x65, 0x64, 0x20, 0x74, 0x6f, 0x20, 0x62, 0x65, 0x20, 0x73, 0x65, 0x63, 0x72, 0x65,
                                    0x74, 0x2c, 0x20, 0x61, 0x6e, 0x64, 0x20, 0x69, 0x74, 0x20, 0x6d, 0x75, 0x73, 0x74, 0x20, 0x62,
                                    0x65, 0x20, 0x61, 0x62, 0x6c, 0x65, 0x20, 0x74, 0x6f, 0x20, 0x66, 0x61, 0x6c, 0x6c, 0x20, 0x69,
                                    0x6e, 0x74, 0x6f, 0x20, 0x74, 0x68, 0x65, 0x20, 0x68, 0x61, 0x6e, 0x64, 0x73, 0x20, 0x6f, 0x66,
                                    0x20, 0x74, 0x68, 0x65, 0x20, 0x65, 0x6e, 0x65, 0x6d, 0x79, 0x20, 0x77, 0x69, 0x74, 0x68, 0x6f,
                                    0x75, 0x74, 0x20, 0x69, 0x6e, 0x63, 0x6f, 0x6e, 0x76, 0x65, 0x6e, 0x69, 0x65, 0x6e, 0x63, 0x65 };
            }
        }

        public static byte[] IV
        {
            get
            {
                return new byte[] { 0x1a, 0xf3, 0x8c, 0x2d, 0xc2, 0xb9, 0x6f, 0xfd, 0xd8, 0x66, 0x94, 0x09, 0x23, 0x41, 0xbc, 0x04 };
            }
        }

        public static byte[] A
        {
            get
            {
                return new byte[] { 0x54, 0x68, 0x65, 0x20, 0x73, 0x65, 0x63, 0x6f, 0x6e, 0x64, 0x20, 0x70, 0x72, 0x69, 0x6e, 0x63,
                                    0x69, 0x70, 0x6c, 0x65, 0x20, 0x6f, 0x66, 0x20, 0x41, 0x75, 0x67, 0x75, 0x73, 0x74, 0x65, 0x20,
                                    0x4b, 0x65, 0x72, 0x63, 0x6b, 0x68, 0x6f, 0x66, 0x66, 0x73 };
            }
        }

        public static byte[] AL
        {
            get
            {
                return new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x50 };
            }
        }

        public static byte[] E
        {
            get
            {
                return new byte[] { 0x4a, 0xff, 0xaa, 0xad, 0xb7, 0x8c, 0x31, 0xc5, 0xda, 0x4b, 0x1b, 0x59, 0x0d, 0x10, 0xff, 0xbd,
                                    0x3d, 0xd8, 0xd5, 0xd3, 0x02, 0x42, 0x35, 0x26, 0x91, 0x2d, 0xa0, 0x37, 0xec, 0xbc, 0xc7, 0xbd,
                                    0x82, 0x2c, 0x30, 0x1d, 0xd6, 0x7c, 0x37, 0x3b, 0xcc, 0xb5, 0x84, 0xad, 0x3e, 0x92, 0x79, 0xc2,
                                    0xe6, 0xd1, 0x2a, 0x13, 0x74, 0xb7, 0x7f, 0x07, 0x75, 0x53, 0xdf, 0x82, 0x94, 0x10, 0x44, 0x6b,
                                    0x36, 0xeb, 0xd9, 0x70, 0x66, 0x29, 0x6a, 0xe6, 0x42, 0x7e, 0xa7, 0x5c, 0x2e, 0x08, 0x46, 0xa1,
                                    0x1a, 0x09, 0xcc, 0xf5, 0x37, 0x0d, 0xc8, 0x0b, 0xfe, 0xcb, 0xad, 0x28, 0xc7, 0x3f, 0x09, 0xb3,
                                    0xa3, 0xb7, 0x5e, 0x66, 0x2a, 0x25, 0x94, 0x41, 0x0a, 0xe4, 0x96, 0xb2, 0xe2, 0xe6, 0x60, 0x9e,
                                    0x31, 0xe6, 0xe0, 0x2c, 0xc8, 0x37, 0xf0, 0x53, 0xd2, 0x1f, 0x37, 0xff, 0x4f, 0x51, 0x95, 0x0b,
                                    0xbe, 0x26, 0x38, 0xd0, 0x9d, 0xd7, 0xa4, 0x93, 0x09, 0x30, 0x80, 0x6d, 0x07, 0x03, 0xb1, 0xf6 };
            }
        }

        public static byte[] M
        {
            get
            {
                return new byte[] { 0x4d, 0xd3, 0xb4, 0xc0, 0x88, 0xa7, 0xf4, 0x5c, 0x21, 0x68, 0x39, 0x64, 0x5b, 0x20, 0x12, 0xbf,
                                    0x2e, 0x62, 0x69, 0xa8, 0xc5, 0x6a, 0x81, 0x6d, 0xbc, 0x1b, 0x26, 0x77, 0x61, 0x95, 0x5b, 0xc5,
                                    0xfd, 0x30, 0xa5, 0x65, 0xc6, 0x16, 0xff, 0xb2, 0xf3, 0x64, 0xba, 0xec, 0xe6, 0x8f, 0xc4, 0x07,
                                    0x53, 0xbc, 0xfc, 0x02, 0x5d, 0xde, 0x36, 0x93, 0x75, 0x4a, 0xa1, 0xf5, 0xc3, 0x37, 0x3b, 0x9c };
            }
        }

        public static byte[] T
        {
            get
            {
                return new byte[] { 0x4d, 0xd3, 0xb4, 0xc0, 0x88, 0xa7, 0xf4, 0x5c, 0x21, 0x68, 0x39, 0x64, 0x5b, 0x20, 0x12, 0xbf,
                                    0x2e, 0x62, 0x69, 0xa8, 0xc5, 0x6a, 0x81, 0x6d, 0xbc, 0x1b, 0x26, 0x77, 0x61, 0x95, 0x5b, 0xc5 };
            }
        }
    }

    // https://datatracker.ietf.org/doc/html/rfc7516#appendix-A.1.4
    // A.1.4 Content encryption using AES-GCM 256
    public static class AES_256_GCM
    {
        public static string Algorithm
        {
            get { return SecurityAlgorithms.Aes256Gcm; }
        }

        public static byte[] IV
        {
            get
            {
                return new byte[] { 227, 197, 117, 252, 2, 219, 233, 68, 180, 225, 77, 219 };
            }
        }

        public static byte[] P
        {
            get
            {
                return new byte[] { 84, 104, 101, 32, 116, 114, 117, 101, 32, 115, 105, 103, 110, 32,
                                    111, 102, 32, 105, 110, 116, 101, 108, 108, 105, 103, 101, 110, 99,
                                    101, 32, 105, 115, 32, 110, 111, 116, 32, 107, 110, 111, 119, 108,
                                    101, 100, 103, 101, 32, 98, 117, 116, 32, 105, 109, 97, 103, 105,
                                    110, 97, 116, 105, 111, 110, 46 };
            }
        }

        public static byte[] A
        {
            get
            {
                return new byte[] { 101, 121, 74, 104, 98, 71, 99, 105, 79, 105, 74, 83, 85, 48, 69,
                                    116, 84, 48, 70, 70, 85, 67, 73, 115, 73, 109, 86, 117, 89, 121, 73,
                                    54, 73, 107, 69, 121, 78, 84, 90, 72, 81, 48, 48, 105, 102, 81 };
            }
        }

        public static byte[] E
        {
            get
            {
                return new byte[] { 229, 236, 166, 241, 53, 191, 115, 196, 174, 43, 73, 109, 39, 122,
                                    233, 96, 140, 206, 120, 52, 51, 237, 48, 11, 190, 219, 186, 80, 111,
                                    104, 50, 142, 47, 167, 59, 61, 181, 127, 196, 21, 40, 82, 242, 32,
                                    123, 143, 168, 226, 73, 216, 176, 144, 138, 247, 106, 60, 16, 205,
                                    160, 109, 64, 63, 192 };
            }
        }

        public static byte[] T
        {
            get
            {
                return new byte[] { 92, 80, 104, 49, 133, 25, 161, 215, 173, 101, 219, 211, 136, 91,
                                    210, 145 };
            }
        }

        public static string EncodedAuthenticationTag
        {
            get { return "XFBoMYUZodetZdvTiFvSkQ"; }
        }

        public static string EncodedCipherText
        {
            get { return "5eym8TW_c8SuK0ltJ3rpYIzOeDQz7TALvtu6UG9oMo4vpzs9tX_EFShS8iB7j6jiSdiwkIr3ajwQzaBtQD_A"; }
        }

        public static string EncodedIV
        {
            get { return "48V1_ALb6US04U3b"; }
        }

        public static string Result
        {
            get { return "eyJhbGciOiJSU0EtT0FFUCIsImVuYyI6IkEyNTZHQ00ifQ.OKOawDo13gRp2ojaHV7LFpZcgV7T6DVZKTyKOMTYUmKoTCVJRgckCL9kiMT03JGeipsEdY3mx_etLbbWSrFr05kLzcSr4qKAq7YN7e9jwQRb23nfa6c9d-StnImGyFDbSv04uVuxIp5Zms1gNxKKK2Da14B8S4rzVRltdYwam_lDp5XnZAYpQdb76FdIKLaVmqgfwX7XWRxv2322i-vDxRfqNzo_tETKzpVLzfiwQyeyPGLBIO56YJ7eObdv0je81860ppamavo35UgoRdbYaBcoh9QcfylQr66oc6vFWXRcZ_ZT2LawVCWTIy3brGPi6UklfCpIMfIjf7iGdXKHzg.48V1_ALb6US04U3b.5eym8TW_c8SuK0ltJ3rpYIzOeDQz7TALvtu6UG9oMo4vpzs9tX_EFShS8iB7j6jiSdiwkIr3ajwQzaBtQD_A.XFBoMYUZodetZdvTiFvSkQ"; }
        }
    }

    // https://datatracker.ietf.org/doc/html/rfc7516#appendix-A.3.3
    // A.3.3 Key Encryption: Aes128 Key Wrap
    public static class AES128_KeyWrap
    {
        public static string Algorithm
        {
            get { return SecurityAlgorithms.Aes128KW; }
        }

        public static string K
        {
            get { return "GawgguFyGrWKav7AX4VKUg"; }
        }

        public static byte[] CEK
        {
            get
            {
                return new byte[] { 4, 211, 31, 197, 84, 157, 252, 254, 11, 100, 157, 250, 63, 170, 106,
                                    206, 107, 124, 212, 45, 111, 107, 9, 219, 200, 177, 0, 240, 143, 156,
                                    44, 207 };
            }
        }

        public static byte[] EncryptedKey
        {
            get
            {
                return new byte[] { 232, 160, 123, 211, 183, 76, 245, 132, 200, 128, 123, 75, 190, 216,
                                    22, 67, 201, 138, 193, 186, 9, 91, 122, 31, 246, 90, 28, 139, 57, 3,
                                    76, 124, 193, 11, 98, 37, 173, 61, 104, 57 };
            }
        }

        public static string EncodedEncryptedKey
        {
            get { return "6KB707dM9YTIgHtLvtgWQ8mKwboJW3of9locizkDTHzBC2IlrT1oOQ"; }
        }
    }

    // https://datatracker.ietf.org/doc/html/rfc7518#appendix-C
    public static class ECDH_ES
    {
        public static byte[] AlgorithmID = new byte[] { 0, 0, 0, 7, 65, 49, 50, 56, 71, 67, 77 };

        public static JsonWebKey AliceEphereralPrivateKey =>
            new JsonWebKey
            {
                Crv = "P-256",
                D = "0_NxaRPUMQoAJt50Gz8YiTr8gRTwyEaCumd-MToTmIo",
                Kty = "EC",
                X = "gI0GAILBdu7T53akrFmMyGcsF3n5dO7MmwNBHKW5SV0",
                Y = "SLW_xSffzlPWrHEVI30DHM_4egVwt3NQqeUD7nMFpps"
            };

        public static string AliceEphereralPrivateKeyString =>
            @"{
                ""kty"":""EC"",
                ""crv"":""P-256"",
                ""x"":""gI0GAILBdu7T53akrFmMyGcsF3n5dO7MmwNBHKW5SV0"",
                ""y"":""SLW_xSffzlPWrHEVI30DHM_4egVwt3NQqeUD7nMFpps"",
                ""d"":""0_NxaRPUMQoAJt50Gz8YiTr8gRTwyEaCumd-MToTmIo""
             }";

        public static JsonWebKey AliceEphereralPublicKey =>
            new JsonWebKey
            {
                Crv = "P-256",
                Kty = "EC",
                X = "gI0GAILBdu7T53akrFmMyGcsF3n5dO7MmwNBHKW5SV0",
                Y = "SLW_xSffzlPWrHEVI30DHM_4egVwt3NQqeUD7nMFpps"
            };

        public static string AliceEphereralPublicKeyString =>
            @"{
                ""kty"":""EC"",
                ""crv"":""P-256"",
                ""x"":""gI0GAILBdu7T53akrFmMyGcsF3n5dO7MmwNBHKW5SV0"",
                ""y"":""SLW_xSffzlPWrHEVI30DHM_4egVwt3NQqeUD7nMFpps""
             }";

        public static JsonWebKey BobEphereralPrivateKey =>
            new JsonWebKey
            {
                Crv = "P-256",
                D = "VEmDZpDXXK8p8N0Cndsxs924q6nS1RXFASRl6BfUqdw",
                Kty = "EC",
                X = "weNJy2HscCSM6AEDTDg04biOvhFhyyWvOHQfeF_PxMQ",
                Y = "e8lnCO-AlStT-NJVX-crhB7QRYhiix03illJOVAOyck"
            };

        public static string BobEphereralPrivateKeyString =>
            @"{
                ""kty"":""EC"",
                ""crv"":""P-256"",
                ""x"":""weNJy2HscCSM6AEDTDg04biOvhFhyyWvOHQfeF_PxMQ"",
                ""y"":""e8lnCO-AlStT-NJVX-crhB7QRYhiix03illJOVAOyck"",
                ""d"":""VEmDZpDXXK8p8N0Cndsxs924q6nS1RXFASRl6BfUqdw""
            }";

        public static JsonWebKey BobEphereralPublicKey =>
            new JsonWebKey
            {
                Crv = "P-256",
                Kty = "EC",
                X = "weNJy2HscCSM6AEDTDg04biOvhFhyyWvOHQfeF_PxMQ",
                Y = "e8lnCO-AlStT-NJVX-crhB7QRYhiix03illJOVAOyck"
            };

        public static string BobEphereralPublicString =>
            @"{
                ""kty"":""EC"",
                ""crv"":""P-256"",
                ""x"":""weNJy2HscCSM6AEDTDg04biOvhFhyyWvOHQfeF_PxMQ"",
                ""y"":""e8lnCO-AlStT-NJVX-crhB7QRYhiix03illJOVAOyck""
            }";

        public static byte[] ConcatKDF =
            new byte[] { 0, 0, 0, 1,
                         158, 86, 217, 29, 129, 113, 53, 211, 114, 131, 66, 131, 191, 132, 38, 156, 251, 49, 110, 163, 218, 128, 106, 72, 246, 218, 167, 121, 140, 254, 144, 196,
                         0, 0, 0, 7, 65, 49, 50, 56, 71, 67, 77,
                         0, 0, 0, 5, 65, 108, 105, 99, 101,
                         0, 0, 0, 3, 66, 111, 98, 0, 0, 0, 128};

        public static byte[] DerivedKeyBytes = new byte[] { 86, 170, 141, 234, 248, 35, 109, 32, 92, 34, 40, 205, 113, 167, 16, 26 };

        public static string DerivedKeyEncoded = "VqqN6vgjbSBcIijNcacQGg";

        public static string EPKString =>
            @"{
                ""alg"":""ECDH-ES"",
                ""enc"":""A128GCM"",
                ""apu"":""QWxpY2U"",
                ""apv"":""Qm9i"",
                ""epk"":
                {
                    ""kty"":""EC"",
                    ""crv"":""P-256"",
                    ""x"":""gI0GAILBdu7T53akrFmMyGcsF3n5dO7MmwNBHKW5SV0"",
                    ""y"":""SLW_xSffzlPWrHEVI30DHM_4egVwt3NQqeUD7nMFpps""
                }
              }";

        public static string Alg = "ECDH-ES";

        public static string Enc = "A128GCM";

        public static string Apu = "QWxpY2U";

        public static string Apv = "Qm9i";

        public static int KeyDataLen = 128;

        public static byte[] PartyUInfo = new byte[] { 0, 0, 0, 5, 65, 108, 105, 99, 101 };

        public static byte[] PartyVInfo = new byte[] { 0, 0, 0, 3, 66, 11, 98 };

        public static byte[] OtherInfo = new byte[] { 0, 0, 0, 7, 65, 49, 50, 56, 71, 67, 77, 0, 0, 0, 5, 65, 108, 105, 99, 101, 0, 0, 0, 3, 66, 111, 98, 0, 0, 0, 128 };

        public static byte[] SuppPubInfo = new byte[] { 0, 0, 0, 128 };

        public static byte[] SuppPrivInfo = new byte[] { };

        public static byte[] Z => new byte[] { 158, 86, 217, 29, 129, 113, 53, 211, 114, 131, 66, 131, 191, 132, 38, 156, 251, 49, 110, 163, 218, 128, 106, 72, 246, 218, 167, 121, 140, 254, 144, 196 };
    }

    // https://datatracker.ietf.org/doc/html/rfc7516#appendix-A.1.3
    // A.1.3 Key wrap: RSAES-OAEP + JsonWebKey
    public static class RSAES_OAEP_KeyWrap
    {
        public static string Algorithm
        {
            get { return SecurityAlgorithms.RsaOAEP; }
        }

        public static byte[] CEK
        {
            get
            {
                return new byte[] { 177, 161, 244, 128, 84, 143, 225, 115, 63, 180, 3, 255, 107, 154,
                                   212, 246, 138, 7, 110, 91, 112, 46, 34, 105, 47, 130, 203, 46, 122,
                                   234, 64, 252 };
            }
        }

        public static byte[] EncryptedKey
        {
            get
            {
                return new byte[] { 56, 163, 154, 192, 58, 53, 222, 4, 105, 218, 136, 218, 29, 94, 203,
                                   22, 150, 92, 129, 94, 211, 232, 53, 89, 41, 60, 138, 56, 196, 216,
                                   82, 98, 168, 76, 37, 73, 70, 7, 36, 8, 191, 100, 136, 196, 244, 220,
                                   145, 158, 138, 155, 4, 117, 141, 230, 199, 247, 173, 45, 182, 214,
                                   74, 177, 107, 211, 153, 11, 205, 196, 171, 226, 162, 128, 171, 182,
                                   13, 237, 239, 99, 193, 4, 91, 219, 121, 223, 107, 167, 61, 119, 228,
                                   173, 156, 137, 134, 200, 80, 219, 74, 253, 56, 185, 91, 177, 34, 158,
                                   89, 154, 205, 96, 55, 18, 138, 43, 96, 218, 215, 128, 124, 75, 138,
                                   243, 85, 25, 109, 117, 140, 26, 155, 249, 67, 167, 149, 231, 100, 6,
                                   41, 65, 214, 251, 232, 87, 72, 40, 182, 149, 154, 168, 31, 193, 126,
                                   215, 89, 28, 111, 219, 125, 182, 139, 235, 195, 197, 23, 234, 55, 58,
                                   63, 180, 68, 202, 206, 149, 75, 205, 248, 176, 67, 39, 178, 60, 98,
                                   193, 32, 238, 122, 96, 158, 222, 57, 183, 111, 210, 55, 188, 215,
                                   206, 180, 166, 150, 166, 106, 250, 55, 229, 72, 40, 69, 214, 216,
                                   104, 23, 40, 135, 212, 28, 127, 41, 80, 175, 174, 168, 115, 171, 197,
                                   89, 116, 92, 103, 246, 83, 216, 182, 176, 84, 37, 147, 35, 45, 219,
                                   172, 99, 226, 233, 73, 37, 124, 42, 72, 49, 242, 35, 127, 184, 134,
                                   117, 114, 135, 206 };
            }
        }

        public static string EncodedEncryptedKey
        {
            get { return "OKOawDo13gRp2ojaHV7LFpZcgV7T6DVZKTyKOMTYUmKoTCVJRgckCL9kiMT03JGeipsEdY3mx_etLbbWSrFr05kLzcSr4qKAq7YN7e9jwQRb23nfa6c9d-StnImGyFDbSv04uVuxIp5Zms1gNxKKK2Da14B8S4rzVRltdYwam_lDp5XnZAYpQdb76FdIKLaVmqgfwX7XWRxv2322i-vDxRfqNzo_tETKzpVLzfiwQyeyPGLBIO56YJ7eObdv0je81860ppamavo35UgoRdbYaBcoh9QcfylQr66oc6vFWXRcZ_ZT2LawVCWTIy3brGPi6UklfCpIMfIjf7iGdXKHzg"; }
        }

        public static JsonWebKey Key
        {
            get
            {
                return new JsonWebKey
                {
                    Kty = "RSA",
                    N = "oahUIoWw0K0usKNuOR6H4wkf4oBUXHTxRvgb48E-BVvxkeDNjbC4he8rUWcJoZmds2h7M70imEVhRU5djINXtqllXI4DFqcI1DgjT9LewND8MW2Krf3Spsk_ZkoFnilakGygTwpZ3uesH-PFABNIUYpOiN15dsQRkgr0vEhxN92i2asbOenSZeyaxziK72UwxrrKoExv6kc5twXTq4h-QChLOln0_mtUZwfsRaMStPs6mS6XrgxnxbWhojf663tuEQueGC-FCMfra36C9knDFGzKsNa7LZK2djYgyD3JR_MB_4NUJW_TqOQtwHYbxevoJArm-L5StowjzGy-_bq6Gw",
                    E = "AQAB",
                    D = "kLdtIj6GbDks_ApCSTYQtelcNttlKiOyPzMrXHeI-yk1F7-kpDxY4-WY5NWV5KntaEeXS1j82E375xxhWMHXyvjYecPT9fpwR_M9gV8n9Hrh2anTpTD93Dt62ypW3yDsJzBnTnrYu1iwWRgBKrEYY46qAZIrA2xAwnm2X7uGR1hghkqDp0Vqj3kbSCz1XyfCs6_LehBwtxHIyh8Ripy40p24moOAbgxVw3rxT_vlt3UVe4WO3JkJOzlpUf-KTVI2Ptgm-dARxTEtE-id-4OJr0h-K-VFs3VSndVTIznSxfyrj8ILL6MG_Uv8YAu7VILSB3lOW085-4qE3DzgrTjgyQ",
                    P = "1r52Xk46c-LsfB5P442p7atdPUrxQSy4mti_tZI3Mgf2EuFVbUoDBvaRQ-SWxkbkmoEzL7JXroSBjSrK3YIQgYdMgyAEPTPjXv_hI2_1eTSPVZfzL0lffNn03IXqWF5MDFuoUYE0hzb2vhrlN_rKrbfDIwUbTrjjgieRbwC6Cl0",
                    Q = "wLb35x7hmQWZsWJmB_vle87ihgZ19S8lBEROLIsZG4ayZVe9Hi9gDVCOBmUDdaDYVTSNx_8Fyw1YYa9XGrGnDew00J28cRUoeBB_jKI1oma0Orv1T9aXIWxKwd4gvxFImOWr3QRL9KEBRzk2RatUBnmDZJTIAfwTs0g68UZHvtc",
                    DP = "ZK-YwE7diUh0qR1tR7w8WHtolDx3MZ_OTowiFvgfeQ3SiresXjm9gZ5KLhMXvo-uz-KUJWDxS5pFQ_M0evdo1dKiRTjVw_x4NyqyXPM5nULPkcpU827rnpZzAJKpdhWAgqrXGKAECQH0Xt4taznjnd_zVpAmZZq60WPMBMfKcuE",
                    DQ = "Dq0gfgJ1DdFGXiLvQEZnuKEN0UUmsJBxkjydc3j4ZYdBiMRAy86x0vHCjywcMlYYg4yoC4YZa9hNVcsjqA3FeiL19rk8g6Qn29Tt0cj8qqyFpz9vNDBUfCAiJVeESOjJDZPYHdHY8v1b-o-Z2X5tvLx-TCekf7oxyeKDUqKWjis",
                    QI = "VIMpMYbPf47dT1w_zDUXfPimsSegnMOA1zTaX7aGk_8urY6R8-ZW1FxU7AlWAyLWybqq6t16VFd7hQd0y6flUK4SlOydB61gwanOsXGOAOv82cHq0E3eL4HrtZkUuKvnPrMnsUUFlfUdybVzxyjz9JF_XyaY14ardLSjf4L_FNY"
                };
            }
        }
    }

    // https://datatracker.ietf.org/doc/html/rfc7516#appendix-A.2.3
    // A.2.3 Key wrap: RSAES-PKCS1-v1_5 + JsonWebKey
    public static class RSAES_PKCS1_KeyWrap
    {
        public static string Algorithm
        {
            get { return SecurityAlgorithms.RsaPKCS1; }
        }

        public static byte[] CEK
        {
            get
            {
                return new byte[] { 4, 211, 31, 197, 84, 157, 252, 254, 11, 100, 157, 250, 63, 170, 106,
                                   206, 107, 124, 212, 45, 111, 107, 9, 219, 200, 177, 0, 240, 143, 156,
                                   44, 207 };
            }
        }

        public static byte[] EncryptedKey
        {
            get
            {
                return new byte[] { 80, 104, 72, 58, 11, 130, 236, 139, 132, 189, 255, 205, 61, 86, 151,
                                   176, 99, 40, 44, 233, 176, 189, 205, 70, 202, 169, 72, 40, 226, 181,
                                   156, 223, 120, 156, 115, 232, 150, 209, 145, 133, 104, 112, 237, 156,
                                   116, 250, 65, 102, 212, 210, 103, 240, 177, 61, 93, 40, 71, 231, 223,
                                   226, 240, 157, 15, 31, 150, 89, 200, 215, 198, 203, 108, 70, 117, 66,
                                   212, 238, 193, 205, 23, 161, 169, 218, 243, 203, 128, 214, 127, 253,
                                   215, 139, 43, 17, 135, 103, 179, 220, 28, 2, 212, 206, 131, 158, 128,
                                   66, 62, 240, 78, 186, 141, 125, 132, 227, 60, 137, 43, 31, 152, 199,
                                   54, 72, 34, 212, 115, 11, 152, 101, 70, 42, 219, 233, 142, 66, 151,
                                   250, 126, 146, 141, 216, 190, 73, 50, 177, 146, 5, 52, 247, 28, 197,
                                   21, 59, 170, 247, 181, 89, 131, 241, 169, 182, 246, 99, 15, 36, 102,
                                   166, 182, 172, 197, 136, 230, 120, 60, 58, 219, 243, 149, 94, 222,
                                   150, 154, 194, 110, 227, 225, 112, 39, 89, 233, 112, 207, 211, 241,
                                   124, 174, 69, 221, 179, 107, 196, 225, 127, 167, 112, 226, 12, 242,
                                   16, 24, 28, 120, 182, 244, 213, 244, 153, 194, 162, 69, 160, 244,
                                   248, 63, 165, 141, 4, 207, 249, 193, 79, 131, 0, 169, 233, 127, 167,
                                   101, 151, 125, 56, 112, 111, 248, 29, 232, 90, 29, 147, 110, 169,
                                   146, 114, 165, 204, 71, 136, 41, 252 };
            }
        }

        public static string EncodedEncryptedKey
        {
            get { return "UGhIOguC7IuEvf_NPVaXsGMoLOmwvc1GyqlIKOK1nN94nHPoltGRhWhw7Zx0-kFm1NJn8LE9XShH59_i8J0PH5ZZyNfGy2xGdULU7sHNF6Gp2vPLgNZ__deLKxGHZ7PcHALUzoOegEI-8E66jX2E4zyJKx-YxzZIItRzC5hlRirb6Y5Cl_p-ko3YvkkysZIFNPccxRU7qve1WYPxqbb2Yw8kZqa2rMWI5ng8OtvzlV7elprCbuPhcCdZ6XDP0_F8rkXds2vE4X-ncOIM8hAYHHi29NX0mcKiRaD0-D-ljQTP-cFPgwCp6X-nZZd9OHBv-B3oWh2TbqmScqXMR4gp_A"; }
        }

        public static JsonWebKey Key
        {
            get
            {
                return new JsonWebKey
                {
                    Kty = "RSA",
                    N = "sXchDaQebHnPiGvyDOAT4saGEUetSyo9MKLOoWFsueri23bOdgWp4Dy1WlUzewbgBHod5pcM9H95GQRV3JDXboIRROSBigeC5yjU1hGzHHyXss8UDprecbAYxknTcQkhslANGRUZmdTOQ5qTRsLAt6BTYuyvVRdhS8exSZEy_c4gs_7svlJJQ4H9_NxsiIoLwAEk7-Q3UXERGYw_75IDrGA84-lA_-Ct4eTlXHBIY2EaV7t7LjJaynVJCpkv4LKjTTAumiGUIuQhrNhZLuF_RJLqHpM2kgWFLU7-VTdL1VbC2tejvcI2BlMkEpk1BzBZI0KQB0GaDWFLN-aEAw3vRw",
                    E = "AQAB",
                    D = "VFCWOqXr8nvZNyaaJLXdnNPXZKRaWCjkU5Q2egQQpTBMwhprMzWzpR8Sxq1OPThh_J6MUD8Z35wky9b8eEO0pwNS8xlh1lOFRRBoNqDIKVOku0aZb-rynq8cxjDTLZQ6Fz7jSjR1Klop-YKaUHc9GsEofQqYruPhzSA-QgajZGPbE_0ZaVDJHfyd7UUBUKunFMScbflYAAOYJqVIVwaYR5zWEEceUjNnTNo_CVSj-VvXLO5VZfCUAVLgW4dpf1SrtZjSt34YLsRarSb127reG_DUwg9Ch-KyvjT1SkHgUWRVGcyly7uvVGRSDwsXypdrNinPA4jlhoNdizK2zF2CWQ",
                    P = "9gY2w6I6S6L0juEKsbeDAwpd9WMfgqFoeA9vEyEUuk4kLwBKcoe1x4HG68ik918hdDSE9vDQSccA3xXHOAFOPJ8R9EeIAbTi1VwBYnbTp87X-xcPWlEPkrdoUKW60tgs1aNd_Nnc9LEVVPMS390zbFxt8TN_biaBgelNgbC95sM",
                    Q = "uKlCKvKv_ZJMVcdIs5vVSU_6cPtYI1ljWytExV_skstvRSNi9r66jdd9-yBhVfuG4shsp2j7rGnIio901RBeHo6TPKWVVykPu1iYhQXw1jIABfw-MVsN-3bQ76WLdt2SDxsHs7q7zPyUyHXmps7ycZ5c72wGkUwNOjYelmkiNS0",
                    DP = "w0kZbV63cVRvVX6yk3C8cMxo2qCM4Y8nsq1lmMSYhG4EcL6FWbX5h9yuvngs4iLEFk6eALoUS4vIWEwcL4txw9LsWH_zKI-hwoReoP77cOdSL4AVcraHawlkpyd2TWjE5evgbhWtOxnZee3cXJBkAi64Ik6jZxbvk-RR3pEhnCs",
                    DQ = "o_8V14SezckO6CNLKs_btPdFiO9_kC1DsuUTd2LAfIIVeMZ7jn1Gus_Ff7B7IVx3p5KuBGOVF8L-qifLb6nQnLysgHDh132NDioZkhH7mI7hPG-PYE_odApKdnqECHWw0J-F0JWnUd6D2B_1TvF9mXA2Qx-iGYn8OVV1Bsmp6qU",
                    QI = "eNho5yRBEBxhGBtQRww9QirZsB66TrfFReG_CcteI1aCneT0ELGhYlRlCtUkTRclIfuEPmNsNDPbLoLqqCVznFbvdB7x-Tl-m0l_eFTj2KiqwGqE9PZB9nNTwMVvH3VRRSLWACvPnSiwP8N5Usy-WRXS-V7TbpxIhvepTfE0NNo"
                };
            }
        }
    }
}
