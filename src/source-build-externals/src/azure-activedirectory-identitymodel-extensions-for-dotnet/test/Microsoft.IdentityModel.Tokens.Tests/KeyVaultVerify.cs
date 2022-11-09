//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

using System;
using System.Security.Cryptography;
using Microsoft.Azure.KeyVault.Cryptography.Algorithms;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Tests
{
    public class KeyVaultVerify
    {
        [Theory, MemberData(nameof(KeyWrapTheoryData))]
        public void DecryptValidate(KeyWrapTestParams testParams)
        {
            if (testParams.Algorithm.Equals(SecurityAlgorithms.Aes128KW, StringComparison.OrdinalIgnoreCase)
                || testParams.Algorithm.Equals(SecurityAlgorithms.Aes256KW, StringComparison.OrdinalIgnoreCase))
            {
                var wrappedKey = testParams.KeyVaultEncryptor.TransformFinalBlock(testParams.KeyToWrap, 0, testParams.KeyToWrap.Length);
                var keyWrapProvider = CryptoProviderFactory.Default.CreateKeyWrapProvider(testParams.Key, testParams.Algorithm);
                var unwrappedKey = keyWrapProvider.UnwrapKey(wrappedKey);

                Assert.True(Utility.AreEqual(unwrappedKey, testParams.KeyToWrap), "Utility.AreEqual(unwrappedKey, testParams.KeyToWrap)");

                CryptoProviderFactory.Default.ReleaseKeyWrapProvider(keyWrapProvider);
            }
            else if (testParams.Algorithm.Equals(SecurityAlgorithms.RsaOAEP, StringComparison.OrdinalIgnoreCase)
                    || testParams.Algorithm.Equals(SecurityAlgorithms.RsaPKCS1, StringComparison.OrdinalIgnoreCase))
            {
                var keyWrapProvider = CryptoProviderFactory.Default.CreateKeyWrapProvider(testParams.Key, testParams.Algorithm);
                var wrappedKey = testParams.KeyVaultEncryptor.TransformFinalBlock(testParams.KeyToWrap, 0, testParams.KeyToWrap.Length);
                var unwrappedKey = keyWrapProvider.UnwrapKey(wrappedKey);

                Assert.True(Utility.AreEqual(unwrappedKey, testParams.KeyToWrap), "Utility.AreEqual(unwrappedKey, testParams.KeyToWrap)");

                CryptoProviderFactory.Default.ReleaseKeyWrapProvider(keyWrapProvider);
            }
        }

        [Theory, MemberData(nameof(KeyWrapTheoryData))]
        public void EncryptValidate(KeyWrapTestParams testParams)
        {
            if (testParams.Algorithm.Equals(SecurityAlgorithms.Aes128KW, StringComparison.OrdinalIgnoreCase)
                || testParams.Algorithm.Equals(SecurityAlgorithms.Aes256KW, StringComparison.OrdinalIgnoreCase))
            {
                var keyWrapProvider = CryptoProviderFactory.Default.CreateKeyWrapProvider(testParams.Key, testParams.Algorithm);
                var wrappedKey = keyWrapProvider.WrapKey(testParams.KeyToWrap);
                byte[] unwrappedKey = testParams.KeyVaultDecryptor.TransformFinalBlock(wrappedKey, 0, wrappedKey.Length);

                Assert.True(Utility.AreEqual(unwrappedKey, testParams.KeyToWrap), "Utility.AreEqual(unwrappedKey, testParams.KeyToWrap)");

                CryptoProviderFactory.Default.ReleaseKeyWrapProvider(keyWrapProvider);
            }
            else if (testParams.Algorithm.Equals(SecurityAlgorithms.RsaOAEP, StringComparison.OrdinalIgnoreCase)
                    || testParams.Algorithm.Equals(SecurityAlgorithms.RsaPKCS1, StringComparison.OrdinalIgnoreCase))
            {
                var keyWrapProvider = CryptoProviderFactory.Default.CreateKeyWrapProvider(testParams.Key, testParams.Algorithm);
                var wrappedKey = keyWrapProvider.WrapKey(testParams.KeyToWrap);
                byte[] unwrappedKey = testParams.KeyVaultDecryptor.TransformFinalBlock(wrappedKey, 0, wrappedKey.Length);

                Assert.True(Utility.AreEqual(unwrappedKey, testParams.KeyToWrap), "Utility.AreEqual(unwrappedKey, testParams.KeyToWrap)");

                CryptoProviderFactory.Default.ReleaseKeyWrapProvider(keyWrapProvider);
            }
        }

        public static TheoryData<KeyWrapTestParams> KeyWrapTheoryData
        {
            get
            {
                var theoryData = new TheoryData<KeyWrapTestParams>();

                AesKw128 aesKw128 = new AesKw128();
                byte[] defaultIV = new byte[] { 0xA6, 0xA6, 0xA6, 0xA6, 0xA6, 0xA6, 0xA6, 0xA6 };
                theoryData.Add(new KeyWrapTestParams
                {
                    Algorithm = SecurityAlgorithms.Aes128KW,
                    KeyVaultEncryptor = aesKw128.CreateEncryptor(KeyingMaterial.DefaultSymmetricKeyBytes_128, defaultIV),
                    KeyVaultDecryptor = aesKw128.CreateDecryptor(KeyingMaterial.DefaultSymmetricKeyBytes_128, defaultIV),
                    Key = new SymmetricSecurityKey(KeyingMaterial.DefaultSymmetricKeyBytes_128),
                    KeyToWrap = AES128_KeyWrap.CEK,
                    TestId = "AES128_KeyWrap"
                });

                AesKw256 aesKw256 = new AesKw256();
                theoryData.Add(new KeyWrapTestParams
                {
                    Algorithm = SecurityAlgorithms.Aes256KW,
                    KeyVaultEncryptor = aesKw256.CreateEncryptor(KeyingMaterial.DefaultSymmetricKeyBytes_256, defaultIV),
                    KeyVaultDecryptor = aesKw256.CreateDecryptor(KeyingMaterial.DefaultSymmetricKeyBytes_256, defaultIV),
                    Key = new SymmetricSecurityKey(KeyingMaterial.DefaultSymmetricKeyBytes_256),
                    KeyToWrap = AES128_KeyWrap.CEK,
                    TestId = "AES256_KeyWrap"
                });

#if NET452
                Rsa15 rsa15 = new Rsa15();
                theoryData.Add(new KeyWrapTestParams
                {
                    Algorithm = SecurityAlgorithms.RsaPKCS1,
                    KeyVaultEncryptor = rsa15.CreateEncryptor(KeyingMaterial.RsaSecurityKeyWithCspProvider_2048_Public.Rsa),
                    KeyVaultDecryptor = rsa15.CreateDecryptor(KeyingMaterial.RsaSecurityKeyWithCspProvider_2048.Rsa),
                    Key = KeyingMaterial.RsaSecurityKeyWithCspProvider_2048,
                    KeyToWrap = RSAES_PKCS1_KeyWrap.CEK,
                    TestId = "RSAES-PKCS1-v1_5"
                });

                RsaOaep rsaOaep = new RsaOaep();
                theoryData.Add(new KeyWrapTestParams
                {
                    Algorithm = SecurityAlgorithms.RsaOAEP,
                    KeyVaultEncryptor = rsaOaep.CreateEncryptor(KeyingMaterial.RsaSecurityKeyWithCspProvider_2048_Public.Rsa),
                    KeyVaultDecryptor = rsaOaep.CreateDecryptor(KeyingMaterial.RsaSecurityKeyWithCspProvider_2048.Rsa),
                    Key = KeyingMaterial.RsaSecurityKeyWithCspProvider_2048,
                    KeyToWrap = RSAES_OAEP_KeyWrap.CEK,
                    TestId = "RSA_OAEP_KeyWrap"
                });

                rsaOaep = new RsaOaep();
                theoryData.Add(new KeyWrapTestParams
                {
                    Algorithm = SecurityAlgorithms.RsaOaepKeyWrap,
                    KeyVaultEncryptor = rsaOaep.CreateEncryptor(KeyingMaterial.RsaSecurityKeyWithCspProvider_2048_Public.Rsa),
                    KeyVaultDecryptor = rsaOaep.CreateDecryptor(KeyingMaterial.RsaSecurityKeyWithCspProvider_2048.Rsa),
                    Key = KeyingMaterial.RsaSecurityKeyWithCspProvider_2048,
                    KeyToWrap = RSAES_OAEP_KeyWrap.CEK,
                    TestId = "RsaOaepKeyWrap"
                });

#endif
                return theoryData;
            }
        }

        public class KeyWrapTestParams
        {
            public string Algorithm { get; set; }
            public ICryptoTransform KeyVaultEncryptor { get; set; }
            public ICryptoTransform KeyVaultDecryptor { get; set; }
            public SecurityKey Key { get; set; }
            public byte[] KeyToWrap { get; set; }
            public string TestId { get; set; }

            public override string ToString()
            {
                    return TestId + ", " + Algorithm + ", " + Key.KeyId;
            }
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
