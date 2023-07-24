// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using Microsoft.IdentityModel.TestUtils;
using Xunit;
#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Tests
{
    public class AuthenticatedEncryptionTheoryData : TheoryDataBase
    {
        public byte[] AuthenticatedData { get; set; }

        public byte[] Bytes { get; set; }

        public string DecryptAlgorithm { get; set; }

        public SecurityKey DecryptKey { get; set; }

        public AuthenticatedEncryptionProvider DecryptionProvider { get; set; }

        public string EncryptAlgorithm { get; set; }

        public AuthenticatedEncryptionProvider EncryptionProvider { get; set; }

        public AuthenticatedEncryptionResult EncryptionResults { get; set; }

        public SecurityKey EncryptKey { get; set; }

        public bool IsSupportedAlgorithm { get; set; }

        public byte[] Plaintext { get; set; }

        public AuthenticatedEncryptionProvider Provider { get; set; }

        public override string ToString()
        {
            return $"{TestId}, {DecryptAlgorithm}, {EncryptAlgorithm}";
        }
    }

    /// <summary>
    /// Tests for AuthenticatedEncryptionProvider
    /// Constructors
    ///     - validate parameters (null, empty)
    ///     - algorithms supported
    ///     - key size
    ///     - properties are set correctly (Algorithm, Context, Key)
    /// EncryptDecrypt
    ///     - positive tests for keys (256, 384, 512, 768, 1024) X Algorithms supported.
    ///     - parameter validation for Encrypt
    /// Decrypt
    ///     - negative tests for tampering of (ciphertest, iv, authenticationtag, authenticateddata)
    ///     - parameter validation for Decrypt
    /// DecryptMismatch
    ///     - negative tests for switching (keys, algorithms)
    /// EncryptVirtual
    ///     - tests virtual method was called
    /// DecryptVirtual
    ///     - tests virtual method was called
    /// </summary>
    public class AuthenticatedEncryptionProviderTests
    {
#if NET_CORE
        [PlatformSpecific(TestPlatforms.Linux | TestPlatforms.OSX)]
        [Fact(Skip = "Adjustment needed")]
        public void AesGcmEncryptionOnLinuxAndMac()
        {
            Assert.Throws<PlatformNotSupportedException>(() => new AuthenticatedEncryptionProvider(Default.SymmetricEncryptionKey256, SecurityAlgorithms.Aes256Gcm));
        }
#endif

#if NET_CORE
        [PlatformSpecific(TestPlatforms.Windows)]
#endif
        [Fact]
        public void AesGcmEncryptionOnWindows()
        {
            var context = new CompareContext();
            try
            {
                var provider = new AuthenticatedEncryptionProvider(Default.SymmetricEncryptionKey256, SecurityAlgorithms.Aes256Gcm);
            }
            catch (Exception ex)
            {
                context.AddDiff($"AuthenticatedEncryptionProvider is not supposed to throw an exception, Exception:{ ex.ToString()}");
            }
            TestUtilities.AssertFailIfErrors(context);
        }

#if NET_CORE
        [PlatformSpecific(TestPlatforms.Windows)]
        [Fact]
        public void AesGcm_Dispose()
        {
            AuthenticatedEncryptionProvider encryptionProvider = new AuthenticatedEncryptionProvider(Default.SymmetricEncryptionKey256, SecurityAlgorithms.Aes256Gcm);
            encryptionProvider.Dispose();
            var expectedException = ExpectedException.ObjectDisposedException;

            try
            {
                encryptionProvider.Decrypt(AES_256_GCM.E, AES_256_GCM.A, AES_256_GCM.IV, AES_256_GCM.T);
            }
            catch (Exception ex)
            {
                expectedException.ProcessException(ex);
            }
        }
#endif

        [Theory, MemberData(nameof(AEPConstructorTheoryData))]
        public void Constructors(string testId, SymmetricSecurityKey key, string algorithm, ExpectedException ee)
        {
            TestUtilities.WriteHeader("Constructors - " + testId, true);
            try
            {
                var context = Guid.NewGuid().ToString();
                var provider = new AuthenticatedEncryptionProvider(key, algorithm) { Context = context };
                provider.CreateSymmetricSignatureProvider();

                ee.ProcessNoException();

                Assert.Equal(provider.Algorithm, algorithm);
                Assert.Equal(provider.Context, context);
                Assert.True(ReferenceEquals(provider.Key, key));
            }
            catch (Exception ex)
            {
                ee.ProcessException(ex);
            }
        }

        public static TheoryData<string, SecurityKey, string, ExpectedException> AEPConstructorTheoryData()
        {
            var theoryData = new TheoryData<string, SecurityKey, string, ExpectedException>();

            theoryData.Add("Test1", null, null, ExpectedException.ArgumentNullException());
            theoryData.Add("Test2", Default.SymmetricEncryptionKey256, null, ExpectedException.ArgumentNullException());
            theoryData.Add("Test3", Default.SymmetricEncryptionKey256, SecurityAlgorithms.Aes128CbcHmacSha256, ExpectedException.NoExceptionExpected);
            theoryData.Add("Test4", Default.SymmetricEncryptionKey512, SecurityAlgorithms.Aes128CbcHmacSha256, ExpectedException.NoExceptionExpected);
            theoryData.Add("Test5", Default.SymmetricEncryptionKey512, SecurityAlgorithms.Aes256CbcHmacSha512, ExpectedException.NoExceptionExpected);
            theoryData.Add("Test6", Default.SymmetricEncryptionKey256, SecurityAlgorithms.Aes128Encryption, ExpectedException.ArgumentException("IDX10668:"));
            theoryData.Add("Test7", Default.SymmetricEncryptionKey128, SecurityAlgorithms.Aes128CbcHmacSha256, ExpectedException.ArgumentOutOfRangeException("IDX10653:"));
            theoryData.Add("Test8", Default.SymmetricEncryptionKey256, SecurityAlgorithms.Aes256CbcHmacSha512, ExpectedException.ArgumentOutOfRangeException("IDX10653:"));

            // set key.CryptoProviderFactory to return null when creating SignatureProvider
            var key = Default.SymmetricEncryptionKey256;
            key.CryptoProviderFactory = new AuthenticatedEncryptionCryptoProviderFactory
            {
                SymmetricSignatureProviderForSigning = null
            };
            theoryData.Add("Test9", key, SecurityAlgorithms.Aes128CbcHmacSha256, ExpectedException.ArgumentException("IDX10649:"));

            key = Default.SymmetricEncryptionKey256;
            key.CryptoProviderFactory = new AuthenticatedEncryptionCryptoProviderFactory
            {
                SymmetricSignatureProviderForSigning = new SymmetricSignatureProvider(key, SecurityAlgorithms.HmacSha256),
            };
            theoryData.Add("Test10", key, SecurityAlgorithms.Aes128CbcHmacSha256, ExpectedException.NoExceptionExpected);

            return theoryData;
        }

        [Theory, MemberData(nameof(DecryptTheoryData))]
        public void Decrypt(AuthenticatedEncryptionTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.Decrypt", theoryData);
            try
            {
                theoryData.Provider.Decrypt(theoryData.EncryptionResults.Ciphertext, theoryData.AuthenticatedData, theoryData.EncryptionResults.IV, theoryData.EncryptionResults.AuthenticationTag);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<AuthenticatedEncryptionTheoryData> DecryptTheoryData()
        {
            var theoryData = new TheoryData<AuthenticatedEncryptionTheoryData>();

            // tampering: AuthenticatedData, AuthenticationTag, Ciphertext, InitializationVector
            AddDecryptTamperedTheoryData("Test1", Default.SymmetricEncryptionKey256, SecurityAlgorithms.Aes128CbcHmacSha256, theoryData);
            AddDecryptTamperedTheoryData("Test2", Default.SymmetricEncryptionKey384, SecurityAlgorithms.Aes128CbcHmacSha256, theoryData);
            AddDecryptTamperedTheoryData("Test3", Default.SymmetricEncryptionKey512, SecurityAlgorithms.Aes128CbcHmacSha256, theoryData);
            AddDecryptTamperedTheoryData("Test4", Default.SymmetricEncryptionKey768, SecurityAlgorithms.Aes128CbcHmacSha256, theoryData);
            AddDecryptTamperedTheoryData("Test5", Default.SymmetricEncryptionKey1024, SecurityAlgorithms.Aes128CbcHmacSha256, theoryData);
            AddDecryptTamperedTheoryData("Test6", Default.SymmetricEncryptionKey512, SecurityAlgorithms.Aes256CbcHmacSha512, theoryData);
            AddDecryptTamperedTheoryData("Test7", Default.SymmetricEncryptionKey768, SecurityAlgorithms.Aes256CbcHmacSha512, theoryData);
            AddDecryptTamperedTheoryData("Test8", Default.SymmetricEncryptionKey1024, SecurityAlgorithms.Aes256CbcHmacSha512, theoryData);

            // parameter check: AuthenticatedData, AuthenticationTag, Ciphertext, InitializationVector - null / size 0
            AddDecryptParameterCheckTheoryData("Test9", null, new byte[1], new byte[1], new byte[1], theoryData);
            AddDecryptParameterCheckTheoryData("Test10", new byte[0], new byte[1], new byte[1], new byte[1], theoryData);
            AddDecryptParameterCheckTheoryData("Test11", new byte[1], null, new byte[1], new byte[1], theoryData);
            AddDecryptParameterCheckTheoryData("Test12", new byte[1], new byte[0], new byte[1], new byte[1], theoryData);
            AddDecryptParameterCheckTheoryData("Test13", new byte[1], new byte[1], null, new byte[1], theoryData);
            AddDecryptParameterCheckTheoryData("Test14", new byte[1], new byte[1], new byte[0], new byte[1], theoryData);
            AddDecryptParameterCheckTheoryData("Test15", new byte[1], new byte[1], new byte[1], null, theoryData);
            AddDecryptParameterCheckTheoryData("Test16", new byte[1], new byte[1], new byte[1], new byte[0], theoryData);

            return theoryData;
        }

        private static void AddDecryptTamperedTheoryData(string testId, SymmetricSecurityKey key, string algorithm, TheoryData<AuthenticatedEncryptionTheoryData> theoryData)
        {
            var authenticatedData = Guid.NewGuid().ToByteArray();
            var plainText = Guid.NewGuid().ToByteArray();
            var provider = new AuthenticatedEncryptionProvider(key, algorithm);
            var results = provider.Encrypt(plainText, authenticatedData);

            theoryData.Add(new AuthenticatedEncryptionTheoryData
            {
                AuthenticatedData = Guid.NewGuid().ToByteArray(),
                DecryptAlgorithm = algorithm,
                DecryptKey = key,
                ExpectedException = ExpectedException.SecurityTokenDecryptionFailedException("IDX10650:"),
                EncryptAlgorithm = algorithm,
                EncryptKey = key,
                EncryptionResults = results,
                Provider = provider,
                TestId = "AddDecryptTheoryData1_" + testId
            });

            results = provider.Encrypt(plainText, authenticatedData);
            TestUtilities.XORBytes(results.IV);
            theoryData.Add(new AuthenticatedEncryptionTheoryData
            {
                AuthenticatedData = authenticatedData,
                DecryptAlgorithm = algorithm,
                DecryptKey = key,
                ExpectedException = ExpectedException.SecurityTokenDecryptionFailedException("IDX10650:"),
                EncryptAlgorithm = algorithm,
                EncryptKey = key,
                EncryptionResults = results,
                Provider = provider,
                TestId = "AddDecryptTheoryData2_" + testId
            });

            results = provider.Encrypt(plainText, authenticatedData);
            TestUtilities.XORBytes(results.AuthenticationTag);
            theoryData.Add(new AuthenticatedEncryptionTheoryData
            {
                AuthenticatedData = authenticatedData,
                DecryptAlgorithm = algorithm,
                DecryptKey = key,
                ExpectedException = ExpectedException.SecurityTokenDecryptionFailedException("IDX10650:"),
                EncryptAlgorithm = algorithm,
                EncryptKey = key,
                EncryptionResults = results,
                Provider = provider,
                TestId = "AddDecryptTheoryData3_" + testId
            });

            results = provider.Encrypt(plainText, authenticatedData);
            TestUtilities.XORBytes(results.Ciphertext);
            theoryData.Add(new AuthenticatedEncryptionTheoryData
            {
                AuthenticatedData = authenticatedData,
                DecryptAlgorithm = algorithm,
                DecryptKey = key,
                ExpectedException = ExpectedException.SecurityTokenDecryptionFailedException("IDX10650:"),
                EncryptAlgorithm = algorithm,
                EncryptKey = key,
                EncryptionResults = results,
                Provider = provider,
                TestId = "AddDecryptTheoryData4_" + testId
            });
        }

        private static void AddDecryptParameterCheckTheoryData(string testId, byte[] authenticatedData, byte[] authenticationTag, byte[] cipherText, byte[] iv, TheoryData<AuthenticatedEncryptionTheoryData> theoryData)
        {
            var provider = new AuthenticatedEncryptionProvider(Default.SymmetricEncryptionKey256, SecurityAlgorithms.Aes128CbcHmacSha256);
            theoryData.Add(new AuthenticatedEncryptionTheoryData
            {
                AuthenticatedData = authenticatedData,
                ExpectedException = ExpectedException.ArgumentNullException(),
                EncryptionResults = new AuthenticatedEncryptionResult(Default.SymmetricEncryptionKey256, cipherText, iv, authenticationTag),
                Provider = provider,
                TestId = testId
            });
        }

        [Theory, MemberData(nameof(DecryptMismatchTheoryData))]
        public void DecryptMismatch(AuthenticatedEncryptionTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.DecryptMismatch", theoryData);
            try
            {
                theoryData.Provider.Decrypt(theoryData.EncryptionResults.Ciphertext, theoryData.AuthenticatedData, theoryData.EncryptionResults.IV, theoryData.EncryptionResults.AuthenticationTag);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<AuthenticatedEncryptionTheoryData> DecryptMismatchTheoryData()
        {
            var theoryData = new TheoryData<AuthenticatedEncryptionTheoryData>();
            var keys128 = new List<SymmetricSecurityKey> { Default.SymmetricEncryptionKey256, Default.SymmetricEncryptionKey384, Default.SymmetricEncryptionKey512, Default.SymmetricEncryptionKey768, Default.SymmetricEncryptionKey1024 };
            var keys256 = new List<SymmetricSecurityKey> { Default.SymmetricEncryptionKey512, Default.SymmetricEncryptionKey768, Default.SymmetricEncryptionKey1024 };
            var keys128_256 = new List<SymmetricSecurityKey> { Default.SymmetricEncryptionKey512, Default.SymmetricEncryptionKey768, Default.SymmetricEncryptionKey1024, Default.SymmetricEncryptionKey256, Default.SymmetricEncryptionKey384 };

            for (int i = 0; i < keys128.Count - 1; i++)
                for(int j = i + 1; j < keys128.Count; j++)
                    AddDecryptMismatchTheoryData(
                        "Test1-" + i.ToString() + "-" + j.ToString(),
                        keys128[i],
                        keys128[j],
                        SecurityAlgorithms.Aes128CbcHmacSha256,
                        SecurityAlgorithms.Aes128CbcHmacSha256,
                        ExpectedException.SecurityTokenDecryptionFailedException(),
                        theoryData);

            for (int i = keys128.Count - 1; i > 0; i--)
                for (int j = i - 1; j > -1; j--)
                    AddDecryptMismatchTheoryData(
                        "Test2-" + i.ToString() + "-" + j.ToString(),
                        keys128[i],
                        keys128[j],
                        SecurityAlgorithms.Aes128CbcHmacSha256,
                        SecurityAlgorithms.Aes128CbcHmacSha256,
                        ExpectedException.SecurityTokenDecryptionFailedException(),
                        theoryData);

            for (int i = 0; i < keys256.Count - 1; i++)
                for (int j = i + 1; j < keys256.Count; j++)
                    AddDecryptMismatchTheoryData(
                        "Test3-" + i.ToString() + "-" + j.ToString(),
                        keys256[i],
                        keys256[j],
                        SecurityAlgorithms.Aes256CbcHmacSha512,
                        SecurityAlgorithms.Aes256CbcHmacSha512,
                        ExpectedException.SecurityTokenDecryptionFailedException(),
                        theoryData);

            for (int i = keys256.Count - 1; i > 0; i--)
                for (int j = i - 1; j > -1; j--)
                    AddDecryptMismatchTheoryData(
                        "Test4-" + i.ToString() + "-" + j.ToString(),
                        keys256[i],
                        keys256[j],
                        SecurityAlgorithms.Aes256CbcHmacSha512,
                        SecurityAlgorithms.Aes256CbcHmacSha512,
                        ExpectedException.SecurityTokenDecryptionFailedException(),
                        theoryData);

            for (int i = 0; i < keys256.Count - 1; i++)
                for (int j = 0; j < keys128.Count; j++)
                    AddDecryptMismatchTheoryData(
                        "Test5-" + i.ToString() + "-" + j.ToString(),
                        keys128[j],
                        keys256[i],
                        SecurityAlgorithms.Aes128CbcHmacSha256,
                        SecurityAlgorithms.Aes256CbcHmacSha512,
                        ExpectedException.SecurityTokenDecryptionFailedException(),
                        theoryData);

            return theoryData;
        }

        private static void AddDecryptMismatchTheoryData(
            string testId,
            SymmetricSecurityKey decryptKey,
            SymmetricSecurityKey encryptkey,
            string decryptAlgorithm,
            string encryptAlgorithm,
            ExpectedException ee,
            TheoryData<AuthenticatedEncryptionTheoryData> theoryData)
        {
            var authenticatedData = Guid.NewGuid().ToByteArray();
            var plainText = Guid.NewGuid().ToByteArray();
            var provider = new AuthenticatedEncryptionProvider(encryptkey, encryptAlgorithm);
            var results = provider.Encrypt(plainText, authenticatedData);
            theoryData.Add(new AuthenticatedEncryptionTheoryData
            {
                AuthenticatedData = authenticatedData,
                DecryptAlgorithm = decryptAlgorithm,
                DecryptKey = decryptKey,
                ExpectedException = ee,
                EncryptionResults = results,
                Provider = new AuthenticatedEncryptionProvider(decryptKey, decryptAlgorithm),
                TestId = testId
            });
        }

        [Theory, MemberData(nameof(DisposeTheoryData))]
        public void Dispose(AuthenticatedEncryptionTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.Dispose", theoryData);

            try
            {
                var results = theoryData.EncryptionProvider.Encrypt(theoryData.Plaintext, theoryData.AuthenticatedData);
                var cleartext = theoryData.DecryptionProvider.Decrypt(results.Ciphertext, theoryData.AuthenticatedData, results.IV, results.AuthenticationTag);

                if (!Utility.AreEqual(theoryData.Plaintext, cleartext))
                    context.AddDiff($"theoryParams.PlainText != clearText. plaintext: '{theoryData.Plaintext}', clearText: '{cleartext}'.");

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

#region DisposeTests
        public static TheoryData<AuthenticatedEncryptionTheoryData> DisposeTheoryData()
        {
            var theoryData = new TheoryData<AuthenticatedEncryptionTheoryData>();

            // these first tests show that dispose is working as expected
            var decryptionKey = Default.SymmetricEncryptionKey256;
            var encryptionKey = Default.SymmetricEncryptionKey256;

            var decryptSignatureProvider = new DecryptSymmetricSignatureProvider(encryptionKey, SecurityAlgorithms.HmacSha256);
            var decryptSignatureProviderDisposed = new DecryptSymmetricSignatureProvider(encryptionKey, SecurityAlgorithms.HmacSha256);
            decryptSignatureProviderDisposed.Dispose();
            var encryptSignatureProvider = new EncryptSymmetricSignatureProvider(encryptionKey, SecurityAlgorithms.HmacSha256);
            var encryptSignatureProviderDisposed = new EncryptSymmetricSignatureProvider(encryptionKey, SecurityAlgorithms.HmacSha256);
            encryptSignatureProviderDisposed.Dispose();

            decryptionKey.CryptoProviderFactory = new DecryptAuthenticatedEncryptionCryptoProviderFactory
            {
                SymmetricSignatureProviderForSigning = decryptSignatureProviderDisposed,
            };
            decryptionKey.CryptoProviderFactory.CacheSignatureProviders = false;

            encryptionKey.CryptoProviderFactory = new EncryptAuthenticatedEncryptionCryptoProviderFactory
            {
                SymmetricSignatureProviderForSigning = encryptSignatureProvider,
            };
            encryptionKey.CryptoProviderFactory.CacheSignatureProviders = false;

            ExpectedException expectedException = ExpectedException.ObjectDisposedException;
            expectedException.SubstringExpected = encryptSignatureProvider.GetType().ToString();
            theoryData.Add(new AuthenticatedEncryptionTheoryData
            {
                AuthenticatedData = Guid.NewGuid().ToByteArray(),
                DecryptAlgorithm = SecurityAlgorithms.Aes128CbcHmacSha256,
                DecryptionProvider = new DecryptAuthenticatedEncryptionProvider(decryptionKey, SecurityAlgorithms.Aes128CbcHmacSha256),
                EncryptAlgorithm = SecurityAlgorithms.Aes128CbcHmacSha256,
                EncryptionProvider = new EncryptAuthenticatedEncryptionProvider(encryptionKey, SecurityAlgorithms.Aes128CbcHmacSha256),
                ExpectedException = expectedException,
                Plaintext = Guid.NewGuid().ToByteArray(),
                TestId = "DecryptUsingExtensibility"
            });

            decryptionKey.CryptoProviderFactory = new DecryptAuthenticatedEncryptionCryptoProviderFactory
            {
                SymmetricSignatureProviderForSigning = decryptSignatureProvider
            };
            decryptionKey.CryptoProviderFactory.CacheSignatureProviders = false;

            encryptionKey.CryptoProviderFactory = new EncryptAuthenticatedEncryptionCryptoProviderFactory
            {
                SymmetricSignatureProviderForSigning = encryptSignatureProviderDisposed
            };
            encryptionKey.CryptoProviderFactory.CacheSignatureProviders = false;

            expectedException = ExpectedException.ObjectDisposedException;
            expectedException.SubstringExpected = encryptSignatureProviderDisposed.GetType().ToString();
            theoryData.Add(new AuthenticatedEncryptionTheoryData
            {
                AuthenticatedData = Guid.NewGuid().ToByteArray(),
                DecryptAlgorithm = SecurityAlgorithms.Aes128CbcHmacSha256,
                DecryptionProvider = new DecryptAuthenticatedEncryptionProvider(decryptionKey, SecurityAlgorithms.Aes128CbcHmacSha256),
                EncryptAlgorithm = SecurityAlgorithms.Aes128CbcHmacSha256,
                EncryptionProvider = new EncryptAuthenticatedEncryptionProvider(encryptionKey, SecurityAlgorithms.Aes128CbcHmacSha256),
                ExpectedException = expectedException,
                Plaintext = Guid.NewGuid().ToByteArray(),
                TestId = "EncryptUsingExtensibility"
            });

            var decryptionProvider = new DecryptAuthenticatedEncryptionProvider(Default.SymmetricEncryptionKey256, SecurityAlgorithms.Aes128CbcHmacSha256);
            var encryptionProvider = new EncryptAuthenticatedEncryptionProvider(Default.SymmetricEncryptionKey256, SecurityAlgorithms.Aes128CbcHmacSha256);
            decryptionProvider.Dispose();
            expectedException = ExpectedException.ObjectDisposedException;
            expectedException.SubstringExpected = decryptionProvider.GetType().ToString();
            theoryData.Add(new AuthenticatedEncryptionTheoryData
            {
                AuthenticatedData = Guid.NewGuid().ToByteArray(),
                DecryptAlgorithm = SecurityAlgorithms.Aes128CbcHmacSha256,
                DecryptionProvider = decryptionProvider,
                EncryptAlgorithm = SecurityAlgorithms.Aes128CbcHmacSha256,
                EncryptionProvider = encryptionProvider,
                ExpectedException = expectedException,
                Plaintext = Guid.NewGuid().ToByteArray(),
                TestId = "Decrypt"
            });

            decryptionProvider = new DecryptAuthenticatedEncryptionProvider(Default.SymmetricEncryptionKey256, SecurityAlgorithms.Aes128CbcHmacSha256);
            encryptionProvider = new EncryptAuthenticatedEncryptionProvider(Default.SymmetricEncryptionKey256, SecurityAlgorithms.Aes128CbcHmacSha256);
            encryptionProvider.Dispose();
            expectedException = ExpectedException.ObjectDisposedException;
            expectedException.SubstringExpected = encryptionProvider.GetType().ToString();
            theoryData.Add(new AuthenticatedEncryptionTheoryData
            {
                AuthenticatedData = Guid.NewGuid().ToByteArray(),
                DecryptAlgorithm = SecurityAlgorithms.Aes128CbcHmacSha256,
                DecryptionProvider = decryptionProvider,
                EncryptAlgorithm = SecurityAlgorithms.Aes128CbcHmacSha256,
                EncryptionProvider = encryptionProvider,
                ExpectedException = expectedException,
                Plaintext = Guid.NewGuid().ToByteArray(),
                TestId = "Encrypt"
            });

            // extensibility tests to show that if a users wants to cache signature providers there is a way to do so.
            decryptSignatureProvider = new DecryptSymmetricSignatureProvider(decryptionKey, SecurityAlgorithms.HmacSha256);
            encryptSignatureProvider = new EncryptSymmetricSignatureProvider(encryptionKey, SecurityAlgorithms.HmacSha256);

            decryptionKey = Default.SymmetricEncryptionKey256;
            decryptionKey.CryptoProviderFactory = new DecryptAuthenticatedEncryptionCryptoProviderFactory
            {
                DisposeSignatureProvider = false,
                SymmetricSignatureProviderForSigning = decryptSignatureProvider
            };
            decryptionKey.CryptoProviderFactory.CacheSignatureProviders = false;

            encryptionKey = Default.SymmetricEncryptionKey256;
            encryptionKey.CryptoProviderFactory = new EncryptAuthenticatedEncryptionCryptoProviderFactory
            {
                DisposeSignatureProvider = false,
                SymmetricSignatureProviderForSigning = encryptSignatureProvider
            };
            encryptionKey.CryptoProviderFactory.CacheSignatureProviders = false;

            // dispose, the crypto provider from the key will not dispose SignatureProvider so it can be reused
            // CryptoProvider.ReleaseSignatureProvider is overloaded.
            decryptionProvider = new DecryptAuthenticatedEncryptionProvider(decryptionKey, SecurityAlgorithms.Aes128CbcHmacSha256);
            decryptionProvider.Dispose();
            decryptionProvider = new DecryptAuthenticatedEncryptionProvider(decryptionKey, SecurityAlgorithms.Aes128CbcHmacSha256);

            encryptionProvider = new EncryptAuthenticatedEncryptionProvider(encryptionKey, SecurityAlgorithms.Aes128CbcHmacSha256);
            encryptionProvider.Dispose();
            encryptionProvider = new EncryptAuthenticatedEncryptionProvider(encryptionKey, SecurityAlgorithms.Aes128CbcHmacSha256);

            theoryData.Add(new AuthenticatedEncryptionTheoryData
            {
                AuthenticatedData = Guid.NewGuid().ToByteArray(),
                DecryptAlgorithm = SecurityAlgorithms.Aes128CbcHmacSha256,
                DecryptionProvider = decryptionProvider,
                EncryptAlgorithm = SecurityAlgorithms.Aes128CbcHmacSha256,
                EncryptionProvider = encryptionProvider,
                Plaintext = Guid.NewGuid().ToByteArray(),
                TestId = "ExtensibilityCache"
            });

            // in this case, dispose will be called, expect ObjectDisposedException
            decryptSignatureProvider = new DecryptSymmetricSignatureProvider(decryptionKey, SecurityAlgorithms.HmacSha256);
            encryptSignatureProvider = new EncryptSymmetricSignatureProvider(encryptionKey, SecurityAlgorithms.HmacSha256);
            decryptionKey = Default.SymmetricEncryptionKey256;
            decryptionKey.CryptoProviderFactory = new DecryptAuthenticatedEncryptionCryptoProviderFactory
            {
                DisposeSignatureProvider = true,
                SymmetricSignatureProviderForSigning = decryptSignatureProvider
            };
            decryptionKey.CryptoProviderFactory.CacheSignatureProviders = false;

            encryptionKey = Default.SymmetricEncryptionKey256;
            encryptionKey.CryptoProviderFactory = new EncryptAuthenticatedEncryptionCryptoProviderFactory
            {
                DisposeSignatureProvider = true,
                SymmetricSignatureProviderForSigning = encryptSignatureProvider
            };
            encryptionKey.CryptoProviderFactory.CacheSignatureProviders = false;

            // dispose the DecryptionProvider
            decryptionProvider = new DecryptAuthenticatedEncryptionProvider(decryptionKey, SecurityAlgorithms.Aes128CbcHmacSha256);
            decryptionProvider.Dispose();
            decryptionProvider = new DecryptAuthenticatedEncryptionProvider(decryptionKey, SecurityAlgorithms.Aes128CbcHmacSha256);
            encryptionProvider = new EncryptAuthenticatedEncryptionProvider(encryptionKey, SecurityAlgorithms.Aes128CbcHmacSha256);
            expectedException = ExpectedException.ObjectDisposedException;
            expectedException.SubstringExpected = decryptSignatureProvider.GetType().ToString();

            theoryData.Add(new AuthenticatedEncryptionTheoryData
            {
                AuthenticatedData = Guid.NewGuid().ToByteArray(),
                DecryptAlgorithm = SecurityAlgorithms.Aes128CbcHmacSha256,
                DecryptionProvider = decryptionProvider,
                EncryptAlgorithm = SecurityAlgorithms.Aes128CbcHmacSha256,
                EncryptionProvider = encryptionProvider,
                ExpectedException = expectedException,
                Plaintext = Guid.NewGuid().ToByteArray(),
                TestId = "DecryptExtensibilityCache"
            });

            // in this case, dispose will be called, expect ObjectDisposedException
            decryptionKey = Default.SymmetricEncryptionKey256;
            encryptionKey = Default.SymmetricEncryptionKey256;
            decryptSignatureProvider = new DecryptSymmetricSignatureProvider(decryptionKey, SecurityAlgorithms.HmacSha256);
            encryptSignatureProvider = new EncryptSymmetricSignatureProvider(encryptionKey, SecurityAlgorithms.HmacSha256);
            decryptionKey.CryptoProviderFactory = new DecryptAuthenticatedEncryptionCryptoProviderFactory
            {
                DisposeSignatureProvider = true,
                SymmetricSignatureProviderForSigning = decryptSignatureProvider
            };
            decryptionKey.CryptoProviderFactory.CacheSignatureProviders = false;

            encryptionKey.CryptoProviderFactory = new EncryptAuthenticatedEncryptionCryptoProviderFactory
            {
                DisposeSignatureProvider = true,
                SymmetricSignatureProviderForSigning = encryptSignatureProvider
            };
            encryptionKey.CryptoProviderFactory.CacheSignatureProviders = false;

            // dispose the EncryptionProvider
            decryptionProvider = new DecryptAuthenticatedEncryptionProvider(decryptionKey, SecurityAlgorithms.Aes128CbcHmacSha256);
            encryptionProvider = new EncryptAuthenticatedEncryptionProvider(encryptionKey, SecurityAlgorithms.Aes128CbcHmacSha256);
            encryptionProvider.Dispose();
            encryptionProvider = new EncryptAuthenticatedEncryptionProvider(encryptionKey, SecurityAlgorithms.Aes128CbcHmacSha256);
            expectedException = ExpectedException.ObjectDisposedException;
            expectedException.SubstringExpected = encryptSignatureProvider.GetType().ToString();

            theoryData.Add(new AuthenticatedEncryptionTheoryData
            {
                AuthenticatedData = Guid.NewGuid().ToByteArray(),
                DecryptAlgorithm = SecurityAlgorithms.Aes128CbcHmacSha256,
                DecryptionProvider = decryptionProvider,
                EncryptAlgorithm = SecurityAlgorithms.Aes128CbcHmacSha256,
                EncryptionProvider = encryptionProvider,
                ExpectedException = expectedException,
                Plaintext = Guid.NewGuid().ToByteArray(),
                TestId = "EcryptExtensibilityCache"
            });

            return theoryData;
        }
#endregion

        [Theory, MemberData(nameof(EncryptDecryptTheoryData))]
        public void EncryptDecrypt(AuthenticatedEncryptionTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.EncryptDecrypt", theoryData);

            try
            {
                // use a different provider for encrypting and decrypting to ensure key creation / privated vars are set correctly
                var encryptionProvider = new AuthenticatedEncryptionProvider(theoryData.EncryptKey, theoryData.DecryptAlgorithm);
                var decryptionProvider = new AuthenticatedEncryptionProvider(theoryData.DecryptKey, theoryData.EncryptAlgorithm);
                var results = encryptionProvider.Encrypt(theoryData.Plaintext, theoryData.AuthenticatedData);
                var cleartext = decryptionProvider.Decrypt(results.Ciphertext, theoryData.AuthenticatedData, results.IV, results.AuthenticationTag);

                Assert.True(Utility.AreEqual(theoryData.Plaintext, cleartext), "theoryParams.PlainText != clearText");

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<AuthenticatedEncryptionTheoryData> EncryptDecryptTheoryData()
        {
            var theoryData = new TheoryData<AuthenticatedEncryptionTheoryData>();

            // round trip positive tests
            AddEncryptDecryptTheoryData("Test1", SecurityAlgorithms.Aes128CbcHmacSha256, Default.SymmetricEncryptionKey256, theoryData);
            AddEncryptDecryptTheoryData("Test2", SecurityAlgorithms.Aes128CbcHmacSha256, Default.SymmetricEncryptionKey384, theoryData);
            AddEncryptDecryptTheoryData("Test3", SecurityAlgorithms.Aes128CbcHmacSha256, Default.SymmetricEncryptionKey512, theoryData);
            AddEncryptDecryptTheoryData("Test4", SecurityAlgorithms.Aes128CbcHmacSha256, Default.SymmetricEncryptionKey768, theoryData);
            AddEncryptDecryptTheoryData("Test5", SecurityAlgorithms.Aes128CbcHmacSha256, Default.SymmetricEncryptionKey1024, theoryData);
            AddEncryptDecryptTheoryData("Test6", SecurityAlgorithms.Aes256CbcHmacSha512, Default.SymmetricEncryptionKey512, theoryData);
            AddEncryptDecryptTheoryData("Test7", SecurityAlgorithms.Aes256CbcHmacSha512, Default.SymmetricEncryptionKey768, theoryData);
            AddEncryptDecryptTheoryData("Test8", SecurityAlgorithms.Aes256CbcHmacSha512, Default.SymmetricEncryptionKey1024, theoryData);

            // Encrypt parameter checking
            AddEncryptParameterCheckTheoryData("Test9",  null,        new byte[1], theoryData);
            AddEncryptParameterCheckTheoryData("Test10", new byte[0], new byte[1], theoryData);
            AddEncryptParameterCheckTheoryData("Test11", new byte[1], null,        theoryData);
            AddEncryptParameterCheckTheoryData("Test12", new byte[1], new byte[0], theoryData);

            return theoryData;
        }

        private static void AddEncryptDecryptTheoryData(string testId, string algorithm, SymmetricSecurityKey key, TheoryData<AuthenticatedEncryptionTheoryData> theoryData)
        {
            theoryData.Add(new AuthenticatedEncryptionTheoryData
            {
                AuthenticatedData = Guid.NewGuid().ToByteArray(),
                DecryptAlgorithm = algorithm,
                DecryptKey = key,
                ExpectedException = ExpectedException.NoExceptionExpected,
                EncryptAlgorithm = algorithm,
                EncryptKey = key,
                Plaintext = Guid.NewGuid().ToByteArray(),
                TestId = "AddEncryptDecryptTheoryData_" + testId
            });
        }

        private static void AddEncryptParameterCheckTheoryData(string testId, byte[] authenticatedData, byte[] plainText, TheoryData<AuthenticatedEncryptionTheoryData> theoryData)
        {
            var provider = new AuthenticatedEncryptionProvider(Default.SymmetricEncryptionKey256, SecurityAlgorithms.Aes128CbcHmacSha256);
            theoryData.Add(new AuthenticatedEncryptionTheoryData
            {
                AuthenticatedData = authenticatedData,
                ExpectedException = ExpectedException.ArgumentNullException(),
                EncryptionResults = new AuthenticatedEncryptionResult(Default.SymmetricEncryptionKey256, new byte[1], new byte[1], new byte[1]),
                Plaintext = plainText,
                Provider = provider,
                TestId = testId
            });
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
