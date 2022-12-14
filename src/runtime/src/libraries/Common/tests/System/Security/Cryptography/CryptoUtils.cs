// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;

namespace Test.Cryptography
{
    internal static class CryptoUtils
    {
        internal static byte[] Encrypt(this SymmetricAlgorithm alg, byte[] plainText, int blockSizeMultiplier = 1)
        {
            using (ICryptoTransform encryptor = alg.CreateEncryptor())
            {
                return encryptor.Transform(plainText, blockSizeMultiplier);
            }
        }

        internal static byte[] Decrypt(this SymmetricAlgorithm alg, byte[] cipher, int blockSizeMultiplier = 1)
        {
            using (ICryptoTransform decryptor = alg.CreateDecryptor())
            {
                return decryptor.Transform(cipher, blockSizeMultiplier);
            }
        }

        internal static byte[] Transform(this ICryptoTransform transform, byte[] input, int blockSizeMultiplier = 1)
        {
            List<byte> output = new List<byte>(input.Length);
            int blockSize = transform.InputBlockSize * blockSizeMultiplier;
            for (int i = 0; i <= input.Length; i += blockSize)
            {
                int count = Math.Min(blockSize, input.Length - i);
                if (count >= blockSize)
                {
                    byte[] buffer = new byte[blockSize];
                    int numBytesWritten = transform.TransformBlock(input, i, count, buffer, 0);
                    Array.Resize(ref buffer, numBytesWritten);
                    output.AddRange(buffer);
                }
                else
                {
                    byte[] finalBlock = transform.TransformFinalBlock(input, i, count);
                    output.AddRange(finalBlock);
                    break;
                }
            }

            return output.ToArray();
        }
    }
}
