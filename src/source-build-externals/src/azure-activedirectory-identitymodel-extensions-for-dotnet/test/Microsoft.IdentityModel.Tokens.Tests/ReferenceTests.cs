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
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Tests
{
    /// <summary>
    /// Tests for references in specs
    /// https://datatracker.ietf.org/doc/html/rfc7518#appendix-A.3
    /// </summary>
    public class ReferenceTests
    {

#if NET472 || NET6_0
        [Fact]
        public void ECDH_ESReferenceTest()
        {
            var context = new CompareContext();
            // arrange
            string alg = ECDH_ES.Alg;
            string enc = ECDH_ES.Enc;
            string apu = ECDH_ES.Apu;
            string apv = ECDH_ES.Apv;

            var aliceEcdsaSecurityKey = new ECDsaSecurityKey(ECDH_ES.AliceEphereralPrivateKey, true);
            var aliceKeyExchangeProvider = new EcdhKeyExchangeProvider(aliceEcdsaSecurityKey, ECDH_ES.BobEphereralPublicKey, alg, enc);

            var bobEcdsaSecurityKey = new ECDsaSecurityKey(ECDH_ES.BobEphereralPrivateKey, true);
            var bobKeyExchangeProvider = new EcdhKeyExchangeProvider(bobEcdsaSecurityKey, ECDH_ES.AliceEphereralPublicKey, alg, enc);

            // act
            SecurityKey aliceCek = aliceKeyExchangeProvider.GenerateKdf(apu, apv);
            SecurityKey bobCek = bobKeyExchangeProvider.GenerateKdf(apu, apv);

            // assert
            // compare KDFs are the same and they're matching with expected
            if (!Utility.AreEqual(((SymmetricSecurityKey)aliceCek).Key, ((SymmetricSecurityKey)bobCek).Key)) 
                context.AddDiff($"!Utility.AreEqual(aliceCek, bobCek)");
            if (!Utility.AreEqual(((SymmetricSecurityKey)aliceCek).Key, ECDH_ES.DerivedKeyBytes))
                context.AddDiff($"!Utility.AreEqual(aliceCek, ECDH_ES.DerivedKeyBytes)");

            TestUtilities.AssertFailIfErrors(context);
        }
#endif

#if NET_CORE
        [PlatformSpecific(TestPlatforms.Windows)]
#endif
        [Fact]
        public void AesGcmReferenceTest()
        {
            var context = new CompareContext();
            var providerForDecryption = CryptoProviderFactory.Default.CreateAuthenticatedEncryptionProvider(new SymmetricSecurityKey(RSAES_OAEP_KeyWrap.CEK), AES_256_GCM.Algorithm);
            var plaintext = providerForDecryption.Decrypt(AES_256_GCM.E, AES_256_GCM.A, AES_256_GCM.IV, AES_256_GCM.T);

            if (!Utility.AreEqual(plaintext, AES_256_GCM.P))
                context.AddDiff($"!Utility.AreEqual(plaintext, testParams.Plaintext)");

            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory, MemberData(nameof(AuthenticatedEncryptionTheoryData))]
        public void AuthenticatedEncryptionReferenceTest(AuthenticationEncryptionTestParams testParams)
        {
            var context = new CompareContext();
            var providerForEncryption = CryptoProviderFactory.Default.CreateAuthenticatedEncryptionProvider(testParams.EncryptionKey, testParams.Algorithm);
            var providerForDecryption = CryptoProviderFactory.Default.CreateAuthenticatedEncryptionProvider(testParams.DecryptionKey, testParams.Algorithm);
            var plaintext = providerForDecryption.Decrypt(testParams.Ciphertext, testParams.AuthenticationData, testParams.IV, testParams.AuthenticationTag);
            var encryptionResult = providerForEncryption.Encrypt(testParams.Plaintext, testParams.AuthenticationData, testParams.IV);

            if (!Utility.AreEqual(encryptionResult.IV, testParams.IV))
                context.AddDiff($"!Utility.AreEqual(encryptionResult.IV, testParams.IV)");

            if (!Utility.AreEqual(encryptionResult.AuthenticationTag, testParams.AuthenticationTag))
                context.AddDiff($"!Utility.AreEqual(encryptionResult.AuthenticationTag, testParams.AuthenticationTag)");

            if (!Utility.AreEqual(encryptionResult.Ciphertext, testParams.Ciphertext))
                context.AddDiff($"!Utility.AreEqual(encryptionResult.Ciphertext, testParams.Ciphertext)");

            if (!Utility.AreEqual(plaintext, testParams.Plaintext))
                context.AddDiff($"!Utility.AreEqual(plaintext, testParams.Plaintext)");

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<AuthenticationEncryptionTestParams> AuthenticatedEncryptionTheoryData
        {
            get
            {
                var theoryData = new TheoryData<AuthenticationEncryptionTestParams>();

                theoryData.Add(new AuthenticationEncryptionTestParams
                {
                    Algorithm = AES_128_CBC_HMAC_SHA_256.Algorithm,
                    AuthenticationData = AES_128_CBC_HMAC_SHA_256.A,
                    AuthenticationTag = AES_128_CBC_HMAC_SHA_256.T,
                    Ciphertext = AES_128_CBC_HMAC_SHA_256.E,
                    DecryptionKey = new SymmetricSecurityKey(AES_128_CBC_HMAC_SHA_256.K) { KeyId = "DecryptionKey.AES_128_CBC_HMAC_SHA_256.K" },
                    EncryptionKey = new SymmetricSecurityKey(AES_128_CBC_HMAC_SHA_256.K) { KeyId = "EncryptionKey.AES_128_CBC_HMAC_SHA_256.K" },
                    IV = AES_128_CBC_HMAC_SHA_256.IV,
                    Plaintext = AES_128_CBC_HMAC_SHA_256.P,
                    TestId = "AES_128_CBC_HMAC_SHA_256"
                });

                theoryData.Add(new AuthenticationEncryptionTestParams
                {
                    Algorithm = AES_192_CBC_HMAC_SHA_384.Algorithm,
                    AuthenticationData = AES_192_CBC_HMAC_SHA_384.A,
                    AuthenticationTag = AES_192_CBC_HMAC_SHA_384.T,
                    Ciphertext = AES_192_CBC_HMAC_SHA_384.E,
                    DecryptionKey = new SymmetricSecurityKey(AES_192_CBC_HMAC_SHA_384.K) { KeyId = "DecryptionKey.AES_192_CBC_HMAC_SHA_384.K" },
                    EncryptionKey = new SymmetricSecurityKey(AES_192_CBC_HMAC_SHA_384.K) { KeyId = "EncryptionKey.AES_192_CBC_HMAC_SHA_384.K" },
                    IV = AES_192_CBC_HMAC_SHA_384.IV,
                    Plaintext = AES_192_CBC_HMAC_SHA_384.P,
                    TestId = "AES_192_CBC_HMAC_SHA_384"
                });

                theoryData.Add(new AuthenticationEncryptionTestParams
                {
                    Algorithm = AES_256_CBC_HMAC_SHA_512.Algorithm,
                    AuthenticationData = AES_256_CBC_HMAC_SHA_512.A,
                    AuthenticationTag = AES_256_CBC_HMAC_SHA_512.T,
                    Ciphertext = AES_256_CBC_HMAC_SHA_512.E,
                    DecryptionKey = new SymmetricSecurityKey(AES_256_CBC_HMAC_SHA_512.K) { KeyId = "DecryptionKey.AES_256_CBC_HMAC_SHA_512.K" },
                    EncryptionKey = new SymmetricSecurityKey(AES_256_CBC_HMAC_SHA_512.K) { KeyId = "EncryptionKey.AES_256_CBC_HMAC_SHA_512.K" },
                    IV = AES_256_CBC_HMAC_SHA_512.IV,
                    Plaintext = AES_256_CBC_HMAC_SHA_512.P,
                    TestId = "AES_256_CBC_HMAC_SHA_512"
                });

                return theoryData;
            }
        }

        public class AuthenticationEncryptionTestParams
        {
            public string Algorithm { get; set; }
            public byte[] AuthenticationData { get; set; }
            public byte[] AuthenticationTag { get; set; }
            public byte[] Ciphertext { get; set; }
            public SecurityKey DecryptionKey { get; set; }
            public SecurityKey EncryptionKey { get; set; }
            public byte[] IV { get; set; }
            public byte[] Plaintext { get; set; }
            public string TestId { get; set; }

            public override string ToString()
            {
                return TestId + ", " + Algorithm + ", " + EncryptionKey.KeyId + ", " + DecryptionKey.KeyId;
            }
        }

        [Theory, MemberData(nameof(KeyWrapTheoryData))]
        public void KeyWrapReferenceTest(KeyWrapTestParams testParams)
        {
            if (testParams.Algorithm.Equals(SecurityAlgorithms.Aes128KW, StringComparison.OrdinalIgnoreCase)
                || testParams.Algorithm.Equals(SecurityAlgorithms.Aes256KW, StringComparison.OrdinalIgnoreCase))
            {
                var keyWrapProvider = CryptoProviderFactory.Default.CreateKeyWrapProvider(testParams.Key, testParams.Algorithm);
                var wrappedKey = keyWrapProvider.WrapKey(testParams.KeyToWrap);
                Assert.True(Utility.AreEqual(wrappedKey, testParams.EncryptedKey), "Utility.AreEqual(wrappedKey, testParams.EncryptedKey)");
                Assert.Equal(Base64UrlEncoder.Encode(wrappedKey), testParams.EncodedEncryptedKey);

                byte[] unwrappedKey = keyWrapProvider.UnwrapKey(wrappedKey);
                Assert.True(Utility.AreEqual(unwrappedKey, testParams.KeyToWrap), "Utility.AreEqual(unwrappedKey, testParams.KeyToWrap)");
            }
            else if (testParams.Algorithm.Equals(SecurityAlgorithms.RsaOAEP, StringComparison.OrdinalIgnoreCase)
                    || testParams.Algorithm.Equals(SecurityAlgorithms.RsaPKCS1, StringComparison.OrdinalIgnoreCase))
            {
                var rsaKeyWrapProvider = CryptoProviderFactory.Default.CreateKeyWrapProvider(testParams.Key, testParams.Algorithm);
                byte[] unwrappedKey = rsaKeyWrapProvider.UnwrapKey(testParams.EncryptedKey);
                Assert.True(Utility.AreEqual(unwrappedKey, testParams.KeyToWrap), "Utility.AreEqual(unwrappedKey, testParams.KeyToWrap)");
            }
        }

        public static TheoryData<KeyWrapTestParams> KeyWrapTheoryData
        {
            get
            {
                var theoryData = new TheoryData<KeyWrapTestParams>();

                theoryData.Add(new KeyWrapTestParams
                {
                    Algorithm = AES128_KeyWrap.Algorithm,
                    Key = new SymmetricSecurityKey(Base64UrlEncoder.DecodeBytes(AES128_KeyWrap.K)),
                    KeyToWrap = AES128_KeyWrap.CEK,
                    EncryptedKey = AES128_KeyWrap.EncryptedKey,
                    EncodedEncryptedKey = AES128_KeyWrap.EncodedEncryptedKey,
                    TestId = "AES128_KeyWrap"
                });

                theoryData.Add(new KeyWrapTestParams
                {
                    Algorithm = RSAES_OAEP_KeyWrap.Algorithm,
                    Key = RSAES_OAEP_KeyWrap.Key,
                    KeyToWrap = RSAES_OAEP_KeyWrap.CEK,
                    EncryptedKey = RSAES_OAEP_KeyWrap.EncryptedKey,
                    EncodedEncryptedKey = RSAES_OAEP_KeyWrap.EncodedEncryptedKey,
                    TestId = "RSA_OAEP_KeyWrap"
                });

                theoryData.Add(new KeyWrapTestParams
                {
                    Algorithm = RSAES_PKCS1_KeyWrap.Algorithm,
                    Key = RSAES_PKCS1_KeyWrap.Key,
                    KeyToWrap = RSAES_PKCS1_KeyWrap.CEK,
                    EncryptedKey = RSAES_PKCS1_KeyWrap.EncryptedKey,
                    EncodedEncryptedKey = RSAES_PKCS1_KeyWrap.EncodedEncryptedKey,
                    TestId = "RSAES-PKCS1-v1_5"
                });

                return theoryData;
            }
        }

        public class KeyWrapTestParams
        {
            public string Algorithm { get; set; }
            public SecurityKey Key { get; set; }
            public byte[] KeyToWrap { get; set; }
            public byte[] EncryptedKey { get; set; }
            public string EncodedEncryptedKey { get; set; }
            public string TestId { get; set; }

            public override string ToString()
            {
                return TestId + ", " + Algorithm + ", " + Key.KeyId;
            }
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
