// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

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
    ///     - positive tests for keys Algorithms supported.
    ///     - parameter validation for WrapKey
    /// UnwrapKey
    ///     - parameter validataion for UnwrapKey
    /// UnwrapKeyMismatch
    ///     - negative tests for switching (keys, algorithms)
    /// WrapKeyVirtual
    ///     - tests virtual method was called
    /// UnwrapKeyVirtual
    ///     - tests virtual method was called
    /// </summary>
    public class RsaKeyWrapProviderTests
    {
        [Theory, MemberData(nameof(RsaKeyWrapConstructorTheoryData), DisableDiscoveryEnumeration = true)]
        public void Constructors(KeyWrapTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.Constructors", theoryData);
            try
            {
                RsaKeyWrapProvider provider = null;
                var keyWrapContext = Guid.NewGuid().ToString();
                if (theoryData.WillUnwrap)
                {
                    provider = new RsaKeyWrapProvider(theoryData.UnwrapKey, theoryData.UnwrapAlgorithm, theoryData.WillUnwrap) { Context = keyWrapContext };
                    provider.CreateAsymmetricAdapter();

                    if (!provider.Algorithm.Equals(theoryData.UnwrapAlgorithm))
                        context.AddDiff($"provider.Algorithm != theoryData.UnwrapAlgorithm: {provider.Algorithm} : {theoryData.UnwrapAlgorithm}.");

                    if (!ReferenceEquals(provider.Key, theoryData.UnwrapKey))
                        context.AddDiff($"!ReferenceEquals(provider.key, theoryData.UnwrapKey)");
                }
                else
                {
                    provider = new RsaKeyWrapProvider(theoryData.WrapKey, theoryData.WrapAlgorithm, theoryData.WillUnwrap) { Context = keyWrapContext };

                    provider.WrapKey(Guid.NewGuid().ToByteArray());

                    if (!provider.Algorithm.Equals(theoryData.WrapAlgorithm))
                        context.AddDiff($"provider.Algorithm != theoryData.WrapAlgorithm: {provider.Algorithm} : {theoryData.WrapAlgorithm}.");

                    if (!ReferenceEquals(provider.Key, theoryData.WrapKey))
                        context.AddDiff($"!ReferenceEquals(provider.key, theoryData.WrapKey)");
                }

                theoryData.ExpectedException.ProcessNoException(context);
                if (!provider.Context.Equals(keyWrapContext))
                    context.AddDiff($"provider.Context != keyWrapContext: {provider.Context} : {keyWrapContext}.");
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<KeyWrapTheoryData> RsaKeyWrapConstructorTheoryData()
        {
            return new TheoryData<KeyWrapTheoryData>
            {
                new KeyWrapTheoryData
                {
                    ExpectedException = ExpectedException.ArgumentNullException("key"),
                    First = true,
                    TestId = "SecurityKeyNULL",
                    WillUnwrap = false,
                    WrapAlgorithm = SecurityAlgorithms.Aes128KeyWrap,
                    WrapKey = null
                },
                new KeyWrapTheoryData
                {
                    ExpectedException = ExpectedException.ArgumentNullException("algorithm"),
                    TestId = "AlgorithmNULL",
                    WillUnwrap = false,
                    WrapAlgorithm = null,
                    WrapKey = KeyingMaterial.RsaSecurityKey_2048
                },
                new KeyWrapTheoryData
                {
                    ExpectedException =  ExpectedException.SecurityTokenKeyWrapException("IDX10661:"),
                    TestId = "KeyTooSmall1024",
                    WillUnwrap = false,
                    WrapAlgorithm = SecurityAlgorithms.RsaOAEP,
                    WrapKey = KeyingMaterial.RsaSecurityKey_1024
                },
                new KeyWrapTheoryData
                {
                    ExpectedException = ExpectedException.SecurityTokenKeyWrapException("IDX10661:"),
                    TestId = "KeyDoesNotMatchAlgorithm",
                    WillUnwrap = false,
                    WrapAlgorithm = SecurityAlgorithms.Aes128KW,
                    WrapKey = KeyingMaterial.RsaSecurityKey_2048
                },
                new KeyWrapTheoryData
                {
                    TestId = "RsaAlgorithmMatch",
                    WillUnwrap = false,
                    WrapAlgorithm = SecurityAlgorithms.RsaPKCS1,
                    WrapKey = KeyingMaterial.RsaSecurityKey_2048
                },
                new KeyWrapTheoryData
                {
                    TestId = "X509AlgorithmMatch",
                    WillUnwrap = false,
                    WrapKey = KeyingMaterial.X509SecurityKey2,
                    WrapAlgorithm = SecurityAlgorithms.RsaPKCS1
                },
                new KeyWrapTheoryData
                {
                    TestId = "JwkRSA",
                    WillUnwrap = false,
                    WrapKey = KeyingMaterial.JsonWebKeyRsa_2048,
                    WrapAlgorithm = SecurityAlgorithms.RsaPKCS1,
                },
                new KeyWrapTheoryData
                {
                    TestId = "RsaPublicKey",
                    UnwrapKey = KeyingMaterial.JsonWebKeyRsa_2048_Public,
                    UnwrapAlgorithm = SecurityAlgorithms.RsaPKCS1,
                    WillUnwrap = true
                }
            };
        }

        [Fact]
        public void RsaKeyWrapProviderDispose()
        {
            SecurityKey key = KeyingMaterial.RsaSecurityKey_2048;
            var provider = new RsaKeyWrapProvider(key, SecurityAlgorithms.RsaPKCS1, false);
            key.CryptoProviderFactory.ReleaseRsaKeyWrapProvider(provider);
        }

        [Fact]
        public void UnwrapKey()
        {
            var provider = new DerivedRsaKeyWrapProvider(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaPKCS1, true);
            var keyBytes = provider.WrapKey(Guid.NewGuid().ToByteArray());
            provider.UnwrapKey(keyBytes);
            Assert.True(provider.UnwrapKeyCalled);
            Assert.True(provider.WrapKeyCalled);
        }

        [Theory, MemberData(nameof(RsaUnwrapMismatchTheoryData), DisableDiscoveryEnumeration = true)]
        public void RsaUnwrapMismatch(KeyWrapTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.RsaUnwrapParameterCheck", theoryData);
            try
            {
                var encryptProvider = new RsaKeyWrapProvider(theoryData.WrapKey, theoryData.WrapAlgorithm, false);
                byte[] keyToWrap = Guid.NewGuid().ToByteArray();
                var wrappedKey = encryptProvider.WrapKey(keyToWrap);
                var decryptProvider = new RsaKeyWrapProvider(theoryData.UnwrapKey, theoryData.UnwrapAlgorithm, true);
                byte[] unwrappedKey = decryptProvider.UnwrapKey(wrappedKey);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<KeyWrapTheoryData> RsaUnwrapMismatchTheoryData()
        {
            return new TheoryData<KeyWrapTheoryData>
            {
                new KeyWrapTheoryData
                {
                    ExpectedException = ExpectedException.KeyWrapException("IDX10659:"),
                    TestId = "AlgorithmMismatchRsaPKCS1RsaOAEP",
                    UnwrapAlgorithm = SecurityAlgorithms.RsaOAEP,
                    UnwrapKey = KeyingMaterial.RsaSecurityKey_2048,
                    WrapAlgorithm = SecurityAlgorithms.RsaPKCS1,
                    WrapKey = KeyingMaterial.RsaSecurityKey_2048_Public
                },
                new KeyWrapTheoryData
                {
                    ExpectedException = ExpectedException.KeyWrapException("IDX10659:"),
                    TestId = "KeyMismatchRsa4096Rsa2048",
                    UnwrapAlgorithm = SecurityAlgorithms.RsaOAEP,
                    UnwrapKey = KeyingMaterial.RsaSecurityKey_2048,
                    WrapAlgorithm = SecurityAlgorithms.RsaOAEP,
                    WrapKey = KeyingMaterial.RsaSecurityKey_4096_Public,
                },
                new KeyWrapTheoryData
                {
                    ExpectedException = ExpectedException.KeyWrapException("IDX10659:"),
                    TestId = "AlgorithmAndKeyMismatchRsaPKCS1Bits4096RsaOAEKey2048",
                    UnwrapAlgorithm = SecurityAlgorithms.RsaOAEP,
                    UnwrapKey = KeyingMaterial.RsaSecurityKey_2048,
                    WrapAlgorithm = SecurityAlgorithms.RsaPKCS1,
                    WrapKey = KeyingMaterial.RsaSecurityKey_4096_Public,
                }
            };
        }

        [Theory, MemberData(nameof(RsaUnwrapTamperedTheoryData), DisableDiscoveryEnumeration = true)]
        public void RsaUnwrapTamperedData(KeyWrapTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.RsaUnwrapParameterCheck", theoryData);
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

        public static TheoryData<KeyWrapTheoryData> RsaUnwrapTamperedTheoryData()
        {
            var theoryData = new TheoryData<KeyWrapTheoryData>();

            // tampering: wrapped key
            AddUnwrapTamperedTheoryData(
                "Test1",
                KeyingMaterial.RsaSecurityKey_2048_Public,
                KeyingMaterial.RsaSecurityKey_2048,
                SecurityAlgorithms.RsaPKCS1, theoryData);

            AddUnwrapTamperedTheoryData(
                "Test2",
                KeyingMaterial.RsaSecurityKey_2048_Public,
                KeyingMaterial.RsaSecurityKey_2048,
                SecurityAlgorithms.RsaOAEP, theoryData);

            return theoryData;
        }

        private static void AddUnwrapTamperedTheoryData(
            string testId,
            SecurityKey encrtyptKey,
            SecurityKey decryptKey,
            string algorithm,
            TheoryData<KeyWrapTheoryData> theoryData)
        {
            var keyToWrap = Guid.NewGuid().ToByteArray();
            var provider = new RsaKeyWrapProvider(encrtyptKey, algorithm, false);
            var wrappedKey = provider.WrapKey(keyToWrap);

            TestUtilities.XORBytes(wrappedKey);
            theoryData.Add(new KeyWrapTheoryData
            {
                UnwrapAlgorithm = algorithm,
                UnwrapKey = decryptKey,
                ExpectedException = ExpectedException.KeyWrapException("IDX10659:"),
                Provider = provider,
                WrappedKey = wrappedKey
            });
        }

        [Theory, MemberData(nameof(RsaUnwrapTheoryData), DisableDiscoveryEnumeration = true)]
        public void RsaUnwrapParameterCheck(KeyWrapTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.RsaUnwrapParameterCheck", theoryData);
            try
            {
                var provider = new RsaKeyWrapProvider(theoryData.UnwrapKey, theoryData.UnwrapAlgorithm, true);
                provider.UnwrapKey(theoryData.WrappedKey);

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<KeyWrapTheoryData> RsaUnwrapTheoryData()
        {
            return new TheoryData<KeyWrapTheoryData>
            {
                new KeyWrapTheoryData
                {
                    ExpectedException = ExpectedException.ArgumentNullException(),
                    TestId = "NullKey",
                    UnwrapAlgorithm = SecurityAlgorithms.RsaPKCS1,
                    UnwrapKey = KeyingMaterial.RsaSecurityKey_2048,
                    WrappedKey = null
                },
                new KeyWrapTheoryData
                {
                    ExpectedException = ExpectedException.ArgumentNullException(),
                    TestId = "ZeroByteLength",
                    UnwrapAlgorithm = SecurityAlgorithms.RsaPKCS1,
                    UnwrapKey = KeyingMaterial.RsaSecurityKey_2048,
                    WrappedKey = new byte[0]
                }
            };
        }

        [Theory, MemberData(nameof(RsaWrapUnwrapTheoryData), DisableDiscoveryEnumeration = true)]
        public void RsaWrapUnwrapKey(KeyWrapTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.RsaWrapUnwrapKey", theoryData);
            try
            {
                var encryptProvider = new RsaKeyWrapProvider(theoryData.WrapKey, theoryData.WrapAlgorithm, false);
                var wrappedKey = encryptProvider.WrapKey(theoryData.KeyToWrap);
                var decryptProvider = new DerivedRsaKeyWrapProvider(theoryData.UnwrapKey, theoryData.UnwrapAlgorithm, true);
                byte[] unwrappedKey = decryptProvider.UnwrapKey(wrappedKey);

                if (!Utility.AreEqual(unwrappedKey, theoryData.KeyToWrap))
                    context.AddDiff("theoryParams.KeyToWrap != unwrappedKey");

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<KeyWrapTheoryData> RsaWrapUnwrapTheoryData()
        {
            var theoryData = new TheoryData<KeyWrapTheoryData>();

            // round trip positive tests
            AddWrapUnwrapTheoryData(
                "Test1",
                SecurityAlgorithms.RsaPKCS1,
                KeyingMaterial.RsaSecurityKey_2048_Public,
                KeyingMaterial.RsaSecurityKey_2048,
                theoryData);

            AddWrapUnwrapTheoryData(
                "Test2",
                SecurityAlgorithms.RsaOAEP,
                KeyingMaterial.RsaSecurityKey_2048_Public,
                KeyingMaterial.RsaSecurityKey_2048,
                theoryData);

            // Wrap parameter checking
            AddWrapParameterCheckTheoryData(
                "Test3",
                SecurityAlgorithms.RsaPKCS1,
                KeyingMaterial.RsaSecurityKey_2048_Public,
                KeyingMaterial.RsaSecurityKey_2048,
                null,
                ExpectedException.ArgumentNullException(),
                theoryData);

            return theoryData;
        }

        private static void AddWrapUnwrapTheoryData(
            string testId,
            string algorithm,
            SecurityKey encryptKey,
            SecurityKey decryptKey, TheoryData<KeyWrapTheoryData> theoryData)
        {
            theoryData.Add(new KeyWrapTheoryData
            {
                KeyToWrap = Guid.NewGuid().ToByteArray(),
                TestId = "AddWrapUnwrapTheoryData" + testId,
                UnwrapAlgorithm = algorithm,
                UnwrapKey = decryptKey,
                WrapAlgorithm = algorithm,
                WrapKey = encryptKey
            });
        }

        private static void AddWrapParameterCheckTheoryData(
            string testId,
            string algorithm,
            SecurityKey encryptKey,
            SecurityKey decryptKey,
            byte[] keyToWrap,
            ExpectedException ee,
            TheoryData<KeyWrapTheoryData> theoryData)
        {
            theoryData.Add(new KeyWrapTheoryData
            {
                ExpectedException = ee,
                KeyToWrap = keyToWrap,
                TestId = testId,
                UnwrapAlgorithm = algorithm,
                UnwrapKey = decryptKey,
                WrapAlgorithm = algorithm,
                WrapKey = encryptKey
            });
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
