// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

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
        [Theory, MemberData(nameof(KeyWrapTheoryData), DisableDiscoveryEnumeration = true)]
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

        [Theory, MemberData(nameof(KeyWrapTheoryData), DisableDiscoveryEnumeration = true)]
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
