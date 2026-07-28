// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Tests
{
    /// <summary>
    /// Tests for AuthenticatedEncryptionProvider Extensibility
    /// </summary>
    public class AuthenticatedEncryptionProviderExtensibilityTests
    {
        [Fact]
        public void Constructor()
        {
            var provider = new DerivedAuthenticatedEncryptionProvider(Default.SymmetricEncryptionKey256, SecurityAlgorithms.Aes128CbcHmacSha256);
            provider.CreateSymmetricSignatureProvider();
            Assert.True(provider.GetKeyBytesCalled);
            Assert.True(provider.IsSupportedAlgorithmCalled);
            Assert.True(provider.ValidateKeySizeCalled);
        }

        [Fact]
        public void DecryptVirtual()
        {
            var provider = new AuthenticatedEncryptionProvider(Default.SymmetricEncryptionKey256, SecurityAlgorithms.Aes128CbcHmacSha256);
            var authenticatedData = Guid.NewGuid().ToByteArray();
            var results = provider.Encrypt(Guid.NewGuid().ToByteArray(), authenticatedData);
            var derivedProvider = new DerivedAuthenticatedEncryptionProvider(Default.SymmetricEncryptionKey256, SecurityAlgorithms.Aes128CbcHmacSha256);
            derivedProvider.Decrypt(results.Ciphertext, authenticatedData, results.IV, results.AuthenticationTag);
            Assert.True(derivedProvider.DecryptCalled);
        }

        [Fact]
        public void EncryptVirtual()
        {
            var provider = new DerivedAuthenticatedEncryptionProvider(Default.SymmetricEncryptionKey256, SecurityAlgorithms.Aes128CbcHmacSha256);
            provider.Encrypt(Guid.NewGuid().ToByteArray(), Guid.NewGuid().ToByteArray());
            Assert.True(provider.EncryptCalled);
        }

        [Theory, MemberData(nameof(GetKeyBytesTheoryData), DisableDiscoveryEnumeration = true)]
        public void GetKeyBytes(AuthenticatedEncryptionTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.GetKeyBytes", theoryData);
            try
            {
                var provider = theoryData.Provider as DerivedAuthenticatedEncryptionProvider;
                var result = provider.GetKeyBytesPublic(theoryData.DecryptKey);
                Assert.True(Utility.AreEqual(result, theoryData.Bytes));
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<AuthenticatedEncryptionTheoryData> GetKeyBytesTheoryData()
        {
            return new TheoryData<AuthenticatedEncryptionTheoryData>
            {
                new AuthenticatedEncryptionTheoryData
                {
                    Bytes = Default.SymmetricEncryptionKey256.Key,
                    DecryptKey = Default.SymmetricEncryptionKey256,
                    Provider = new DerivedAuthenticatedEncryptionProvider(),
                    TestId = "Test1"
                },
                new AuthenticatedEncryptionTheoryData
                {
                    DecryptKey = null,
                    ExpectedException = ExpectedException.ArgumentNullException(),
                    Provider = new DerivedAuthenticatedEncryptionProvider(),
                    TestId = "Test2"
                },
                new AuthenticatedEncryptionTheoryData
                {
                    DecryptKey = Default.AsymmetricSigningKey,
                    ExpectedException = ExpectedException.ArgumentException("IDX10667:"),
                    Provider = new DerivedAuthenticatedEncryptionProvider(),
                    TestId = "Test3"
                },
                new AuthenticatedEncryptionTheoryData
                {
                    Bytes = KeyingMaterial.JsonWebKeySymmetricBytes256,
                    DecryptKey = KeyingMaterial.JsonWebKeySymmetric256,
                    Provider = new DerivedAuthenticatedEncryptionProvider(),
                    TestId = "Test4"
                }
            };
        }

        [Theory, MemberData(nameof(IsSupportedAlgorithmTheoryData), DisableDiscoveryEnumeration = true)]
        public void IsSupportedAlgorithm(AuthenticatedEncryptionTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.IsSupportedAlgorithm", theoryData);
            try
            {
                var provider = theoryData.Provider as DerivedAuthenticatedEncryptionProvider;
                var result = provider.IsSupportedAlgorithmPublic(theoryData.DecryptKey, theoryData.DecryptAlgorithm);

                Assert.True(result == theoryData.IsSupportedAlgorithm);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<AuthenticatedEncryptionTheoryData> IsSupportedAlgorithmTheoryData()
        {
            return new TheoryData<AuthenticatedEncryptionTheoryData>
            {
                new AuthenticatedEncryptionTheoryData
                {
                    DecryptAlgorithm = SecurityAlgorithms.Aes128CbcHmacSha256,
                    DecryptKey = null,
                    IsSupportedAlgorithm = false,
                    Provider = new DerivedAuthenticatedEncryptionProvider(Default.SymmetricEncryptionKey256, SecurityAlgorithms.Aes128CbcHmacSha256),
                    TestId = "Test1"
                },
                new AuthenticatedEncryptionTheoryData
                {
                    DecryptAlgorithm = SecurityAlgorithms.Aes128CbcHmacSha256,
                    DecryptKey = null,
                    IsSupportedAlgorithm = false,
                    Provider = new DerivedAuthenticatedEncryptionProvider(Default.SymmetricEncryptionKey256, SecurityAlgorithms.Aes128CbcHmacSha256),
                    TestId = "Test2"
                },
                new AuthenticatedEncryptionTheoryData
                {
                    DecryptAlgorithm = SecurityAlgorithms.Aes128Encryption,
                    DecryptKey = Default.SymmetricEncryptionKey256,
                    IsSupportedAlgorithm = false,
                    Provider = new DerivedAuthenticatedEncryptionProvider(Default.SymmetricEncryptionKey256, SecurityAlgorithms.Aes128CbcHmacSha256),
                    TestId = "Test3"
                },
                new AuthenticatedEncryptionTheoryData
                {
                    DecryptAlgorithm = SecurityAlgorithms.Aes128CbcHmacSha256,
                    DecryptKey = Default.AsymmetricSigningKey,
                    IsSupportedAlgorithm = false,
                    Provider = new DerivedAuthenticatedEncryptionProvider(Default.SymmetricEncryptionKey256, SecurityAlgorithms.Aes128CbcHmacSha256),
                    TestId = "Test4"
                }
            };
        }

        [Theory, MemberData(nameof(ValidateKeySizeTheoryData), DisableDiscoveryEnumeration = true)]
        public void ValidateKeySize(AuthenticatedEncryptionTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateKeySize", theoryData);
            try
            {
                var provider = theoryData.Provider as DerivedAuthenticatedEncryptionProvider;
                provider.ValidateKeySizePublic(theoryData.DecryptKey, theoryData.DecryptAlgorithm);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<AuthenticatedEncryptionTheoryData> ValidateKeySizeTheoryData()
        {
            return new TheoryData<AuthenticatedEncryptionTheoryData>
            {
                new AuthenticatedEncryptionTheoryData
                {
                    DecryptAlgorithm = SecurityAlgorithms.Aes128CbcHmacSha256,
                    DecryptKey = null,
                    ExpectedException = ExpectedException.ArgumentNullException(),
                    Provider = new DerivedAuthenticatedEncryptionProvider(),
                    TestId = "Test1"
                },
                new AuthenticatedEncryptionTheoryData
                {
                    DecryptAlgorithm = null,
                    DecryptKey = Default.SymmetricEncryptionKey256,
                    ExpectedException = ExpectedException.ArgumentNullException(),
                    Provider = new DerivedAuthenticatedEncryptionProvider(),
                    TestId = "Test2"
                },
                new AuthenticatedEncryptionTheoryData
                {
                    DecryptAlgorithm = string.Empty,
                    DecryptKey = Default.SymmetricEncryptionKey256,
                    ExpectedException = ExpectedException.ArgumentNullException(),
                    Provider = new DerivedAuthenticatedEncryptionProvider(),
                    TestId = "Test3"
                },
                new AuthenticatedEncryptionTheoryData
                {
                    DecryptAlgorithm = SecurityAlgorithms.Aes192KeyWrap,
                    DecryptKey = Default.SymmetricEncryptionKey256,
                    ExpectedException = ExpectedException.ArgumentException("IDX10652:"),
                    Provider = new DerivedAuthenticatedEncryptionProvider(),
                    TestId = "Test4"
                }
            };
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
