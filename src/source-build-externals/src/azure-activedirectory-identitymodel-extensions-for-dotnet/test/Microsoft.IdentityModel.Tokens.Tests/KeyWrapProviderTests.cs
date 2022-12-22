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
using Microsoft.IdentityModel.TestUtils;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Tests
{
    /// <summary>
    /// Tests for KeyWrapProvider
    /// Constructors
    ///     - validate parameters (null, empty)
    ///     - algorithms supported
    ///     - properties are set correctly (Algorithm, Context, Key)
    /// WrapKey/UnwrapKey
    ///     - positive tests for keys (128, 256) X Algorithms supported.
    ///     - parameter validation for WrapKey
    /// UnwrapKey
    ///     - parameter validation for UnwrapKey
    /// UnwrapKeyMismatch
    ///     - negative tests for switching (keys, algorithms)
    /// WrapKeyVirtual
    ///     - tests virtual method was called
    /// UnwrapKeyVirtual
    ///     - tests virtual method was called
    /// </summary>
    public class KeyWrapProviderTests
    {
        [Theory, MemberData(nameof(KeyWrapConstructorTestCases))]
        public void Constructors(SupportedAlgorithmTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.Constructors", theoryData);

            try
            {
                var providerContext = Guid.NewGuid().ToString();
                var provider = CryptoProviderFactory.Default.CreateKeyWrapProvider(theoryData.SecurityKey, theoryData.Algorithm);
                provider.Context = providerContext;

                // validation is defered until first use
                provider.WrapKey(Guid.NewGuid().ToByteArray());

                theoryData.ExpectedException.ProcessNoException(context);
                if (provider.Algorithm != theoryData.Algorithm)
                    context.AddDiff($"provider.Algorithm: '{provider.Algorithm}' != theoryData.Algorithm: '{theoryData.Algorithm}'.");

                if (provider.Context != providerContext)
                    context.AddDiff($"provider.Context: '{provider.Context}' != providerContext: '{providerContext}'.");

                if (!ReferenceEquals(provider.Key, theoryData.SecurityKey))
                    context.AddDiff("!ReferenceEquals(provider.Key, theoryData.SecurityKey))");
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SupportedAlgorithmTheoryData> KeyWrapConstructorTestCases
        {
            get
            {
                var theoryData = new TheoryData<SupportedAlgorithmTheoryData>();

                SupportedAlgorithmTheoryData.AddTestCase(SecurityAlgorithms.Aes128KW, null, "SecurityKeyNull", theoryData, ExpectedException.ArgumentNullException("key"));
                SupportedAlgorithmTheoryData.AddTestCase(null, Default.SymmetricEncryptionKey128, "AlgorithmNull", theoryData, ExpectedException.ArgumentNullException("algorithm"));
                SupportedAlgorithmTheoryData.AddTestCase(SecurityAlgorithms.Aes128Encryption, Default.SymmetricEncryptionKey128, "Aes128Encryption", theoryData, ExpectedException.NotSupportedException("IDX10661:"));
                SupportedAlgorithmTheoryData.AddTestCase(SecurityAlgorithms.Aes256KeyWrap, Default.SymmetricEncryptionKey128, "SymmetricKey_128_Aes256KeyWrap", theoryData, ExpectedException.SecurityTokenKeyWrapException("IDX10662:"));
                SupportedAlgorithmTheoryData.AddTestCase(SecurityAlgorithms.Aes128KeyWrap, Default.SymmetricEncryptionKey256, "SymmetricKey_256_Aes128KeyWrap", theoryData, ExpectedException.SecurityTokenKeyWrapException("IDX10662:"));
                SupportedAlgorithmTheoryData.AddTestCase(SecurityAlgorithms.Aes256KW, Default.SymmetricEncryptionKey128, "SymmetricKey_128_Aes256KW", theoryData, ExpectedException.SecurityTokenKeyWrapException("IDX10662:"));
                SupportedAlgorithmTheoryData.AddTestCase(SecurityAlgorithms.Aes128KW, Default.SymmetricEncryptionKey256, "SymmetricKey_256_Aes128KW", theoryData, ExpectedException.SecurityTokenKeyWrapException("IDX10662:"));
                SupportedAlgorithmTheoryData.AddTestCase(SecurityAlgorithms.Aes256KW, new JsonWebKey { Kty = JsonWebAlgorithmsKeyTypes.RSA, K = KeyingMaterial.JsonWebKeySymmetric128.K }, "JsonWebKey_RSA_Aes256KW", theoryData, ExpectedException.NotSupportedException("IDX10661:"));
                SupportedAlgorithmTheoryData.AddTestCase(SecurityAlgorithms.Aes256KW, KeyingMaterial.RsaSecurityKey_2048, "RsaSecurityKey_Aes256KW", theoryData, ExpectedException.NotSupportedException("IDX10661:"));
                SupportedAlgorithmTheoryData.AddTestCase(SecurityAlgorithms.Aes128KeyWrap, Default.SymmetricEncryptionKey128, "SymmetricKey_Aes128KeyWrap", theoryData);
                SupportedAlgorithmTheoryData.AddTestCase(SecurityAlgorithms.Aes256KeyWrap, Default.SymmetricEncryptionKey256, "SymmetricKey_Aes256KeyWrap", theoryData); ;
                SupportedAlgorithmTheoryData.AddTestCase(SecurityAlgorithms.Aes128KW, Default.SymmetricEncryptionKey128, "SymmetricKey_Aes128KW", theoryData);
                SupportedAlgorithmTheoryData.AddTestCase(SecurityAlgorithms.Aes256KW, Default.SymmetricEncryptionKey256, "SymmetricKey_Aes256KW", theoryData);
                SupportedAlgorithmTheoryData.AddTestCase(SecurityAlgorithms.Aes128KeyWrap, KeyingMaterial.JsonWebKeySymmetric128, "JsonWebKey_Aes128KeyWrap", theoryData);
                SupportedAlgorithmTheoryData.AddTestCase(SecurityAlgorithms.Aes256KeyWrap, KeyingMaterial.JsonWebKeySymmetric256, "JsonWebKey_Aes256KeyWrap", theoryData);
                SupportedAlgorithmTheoryData.AddTestCase(SecurityAlgorithms.Aes128KW, KeyingMaterial.JsonWebKeySymmetric128, "JsonWebKey_Aes128KW", theoryData);
                SupportedAlgorithmTheoryData.AddTestCase(SecurityAlgorithms.Aes256KW, KeyingMaterial.JsonWebKeySymmetric256, "JsonWebKey_Aes256KW", theoryData);

                return theoryData;
            }
        }

        [Fact]
        public void KeyWrapProviderDispose_Test()
        {
            SecurityKey key = Default.SymmetricEncryptionKey128;
            var provider = key.CryptoProviderFactory.CreateKeyWrapProvider(key, SecurityAlgorithms.Aes128KW);
            key.CryptoProviderFactory.ReleaseKeyWrapProvider(provider);
        }

        [Fact]
        public void UnwrapKey()
        {
            var provider = new DerivedKeyWrapProvider(Default.SymmetricEncryptionKey128, SecurityAlgorithms.Aes128KW);
            var wrappedKey = provider.WrapKey(Guid.NewGuid().ToByteArray());
            provider.UnwrapKey(wrappedKey);
            Assert.True(provider.UnwrapKeyCalled);
            Assert.True(provider.WrapKeyCalled);
            Assert.True(provider.IsSupportedAlgorithmCalled);
            Assert.True(provider.GetSymmetricAlgorithmCalled);
        }

        [Theory, MemberData(nameof(WrapUnwrapTheoryData))]
        public void WrapUnwrapKey(KeyWrapTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.WrapUnwrapKey", theoryData);
            try
            {
                var provider = CryptoProviderFactory.Default.CreateKeyWrapProvider(theoryData.WrapKey, theoryData.WrapAlgorithm);
                var wrappedKey = provider.WrapKey(theoryData.KeyToWrap);
                byte[] unwrappedKey = provider.UnwrapKey(wrappedKey);

                Assert.True(Utility.AreEqual(unwrappedKey, theoryData.KeyToWrap), "theoryParams.KeyToWrap != unwrappedKey");

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<KeyWrapTheoryData> WrapUnwrapTheoryData()
        {
            var theoryData = new TheoryData<KeyWrapTheoryData>();

            // round trip positive tests
            AddWrapUnwrapTheoryData("Test1", SecurityAlgorithms.Aes128KW, Default.SymmetricEncryptionKey128, theoryData);
            AddWrapUnwrapTheoryData("Test2", SecurityAlgorithms.Aes256KW, Default.SymmetricEncryptionKey256, theoryData);

            // Wrap parameter checking
            AddWrapParameterCheckTheoryData("Test3", SecurityAlgorithms.Aes128KW, Default.SymmetricEncryptionKey128, null, ExpectedException.ArgumentNullException(), theoryData);
            byte[] keyToWrap = new byte[9];
            Array.Copy(Guid.NewGuid().ToByteArray(), keyToWrap, keyToWrap.Length);
            AddWrapParameterCheckTheoryData("Test4", SecurityAlgorithms.Aes128KW, Default.SymmetricEncryptionKey128, keyToWrap, ExpectedException.ArgumentException("IDX10664:"), theoryData);

            return theoryData;
        }

        private static void AddWrapUnwrapTheoryData(string testId, string algorithm, SecurityKey key, TheoryData<KeyWrapTheoryData> theoryData)
        {
            theoryData.Add(new KeyWrapTheoryData
            {
                KeyToWrap = Guid.NewGuid().ToByteArray(),
                WrapAlgorithm = algorithm,
                WrapKey = key,
                TestId = "AddWrapUnwrapTheoryData" + testId
            });
        }

        private static void AddWrapParameterCheckTheoryData(string testId, string algorithm, SecurityKey key, byte[] keyToWrap, ExpectedException ee, TheoryData<KeyWrapTheoryData> theoryData)
        {
            theoryData.Add(new KeyWrapTheoryData
            {
                WrapAlgorithm = algorithm,
                WrapKey = key,
                KeyToWrap = keyToWrap,
                ExpectedException = ee,
                TestId = testId
            });
        }

        [Theory, MemberData(nameof(UnwrapTamperedTheoryData))]
        public void UnwrapTamperedData(KeyWrapTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.UnwrapTamperedData", theoryData);
            try
            {
                theoryData.Provider.UnwrapKey(theoryData.WrappedKey);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<KeyWrapTheoryData> UnwrapTamperedTheoryData()
        {
            var theoryData = new TheoryData<KeyWrapTheoryData>();

            // tampering: wrapped key
            AddUnwrapTamperedTheoryData("Test1", Default.SymmetricEncryptionKey128, SecurityAlgorithms.Aes128KW, theoryData);
            AddUnwrapTamperedTheoryData("Test2", Default.SymmetricEncryptionKey256, SecurityAlgorithms.Aes256KW, theoryData);

            return theoryData;
        }

        private static void AddUnwrapTamperedTheoryData(string testId, SecurityKey key, string algorithm, TheoryData<KeyWrapTheoryData> theoryData)
        {
            var keyToWrap = Guid.NewGuid().ToByteArray();
            var provider = CryptoProviderFactory.Default.CreateKeyWrapProvider(key, algorithm);
            var wrappedKey = provider.WrapKey(keyToWrap);

            TestUtilities.XORBytes(wrappedKey);
            theoryData.Add(new KeyWrapTheoryData
            {
                ExpectedException = ExpectedException.KeyWrapException("IDX10659:"),
                Provider = provider,
                TestId = testId,
                WrapAlgorithm = algorithm,
                WrapKey = key,
                WrappedKey = wrappedKey
            });
        }

        [Theory, MemberData(nameof(UnwrapMismatchTheoryData))]
        public void UnwrapMismatch(KeyWrapTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.UnwrapMismatch", theoryData);
            try
            {
                var encryptProvider = CryptoProviderFactory.Default.CreateKeyWrapProvider(theoryData.WrapKey, theoryData.WrapAlgorithm);
                byte[] keyToWrap = Guid.NewGuid().ToByteArray();
                var wrappedKey = encryptProvider.WrapKey(keyToWrap);
                var decryptProvider = CryptoProviderFactory.Default.CreateKeyWrapProvider(theoryData.UnwrapKey, theoryData.UnwrapAlgorithm);
                byte[] unwrappedKey = decryptProvider.UnwrapKey(wrappedKey);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<KeyWrapTheoryData> UnwrapMismatchTheoryData()
        {
            var theoryData = new TheoryData<KeyWrapTheoryData>();

            AddUnwrapMismatchTheoryData("Test1", Default.SymmetricEncryptionKey128, Default.SymmetricEncryptionKey128_2, SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128KW, ExpectedException.KeyWrapException("IDX10659:"), theoryData);
            AddUnwrapMismatchTheoryData("Test2", Default.SymmetricEncryptionKey256, Default.SymmetricEncryptionKey256_2, SecurityAlgorithms.Aes256KW, SecurityAlgorithms.Aes256KW, ExpectedException.KeyWrapException("IDX10659:"), theoryData);
            AddUnwrapMismatchTheoryData("Test3", Default.SymmetricEncryptionKey128, Default.SymmetricEncryptionKey256, SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes256KW, ExpectedException.KeyWrapException("IDX10659:"), theoryData);

            return theoryData;
        }

        private static void AddUnwrapMismatchTheoryData(string testId, SecurityKey encryptKey, SecurityKey decryptKey, string encryptAlg, string decryptAlg, ExpectedException ee, TheoryData<KeyWrapTheoryData> theoryData)
        {
            theoryData.Add(new KeyWrapTheoryData
            {
                ExpectedException = ee,
                TestId = testId,
                UnwrapAlgorithm = decryptAlg,
                UnwrapKey = decryptKey,
                WrapAlgorithm = encryptAlg,
                WrapKey = encryptKey
            });
        }

        [Theory, MemberData(nameof(UnwrapTheoryData))]
        public void UnwrapParameterCheck(KeyWrapTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.UnwrapParameterCheck", theoryData);
            try
            {
                var provider = CryptoProviderFactory.Default.CreateKeyWrapProvider(theoryData.WrapKey, theoryData.WrapAlgorithm);
                byte[] unwrappedKey = provider.UnwrapKey(theoryData.WrappedKey);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<KeyWrapTheoryData> UnwrapTheoryData()
        {
            var theoryData = new TheoryData<KeyWrapTheoryData>();

            // Unwrap parameter checking
            AddUnwrapParameterCheckTheoryData("Test1", SecurityAlgorithms.Aes128KW, Default.SymmetricEncryptionKey128, null, ExpectedException.ArgumentNullException(), theoryData);

            byte[] wrappedKey = new byte[12];
            Array.Copy(Guid.NewGuid().ToByteArray(), wrappedKey, wrappedKey.Length);
            AddUnwrapParameterCheckTheoryData("Test2", SecurityAlgorithms.Aes128KW, Default.SymmetricEncryptionKey128, wrappedKey, ExpectedException.ArgumentException("IDX10664:"), theoryData);

            return theoryData;
        }

        private static void AddUnwrapParameterCheckTheoryData(string testId, string algorithm, SecurityKey key, byte[] wrappedKey, ExpectedException ee, TheoryData<KeyWrapTheoryData> theoryData)
        {
            theoryData.Add(new KeyWrapTheoryData
            {
                ExpectedException = ee,
                TestId = testId,
                WrapAlgorithm = algorithm,
                WrapKey = key,
                WrappedKey = wrappedKey
            });
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
