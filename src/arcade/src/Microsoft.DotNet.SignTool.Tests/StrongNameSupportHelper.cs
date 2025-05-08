// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Security.Cryptography;

namespace Microsoft.DotNet.SignTool.Tests
{
    internal static class StrongNameSupportHelper
    {
        internal static bool GetPlatformSupportsRSASHA1()
        {
            using (RSA rsa = RSA.Create(2048))
            {
                try
                {
                    rsa.SignData(Array.Empty<byte>(), HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
                    return true;
                }
                catch (CryptographicException)
                {
                    return false;
                }
            }
        }
    }
}
