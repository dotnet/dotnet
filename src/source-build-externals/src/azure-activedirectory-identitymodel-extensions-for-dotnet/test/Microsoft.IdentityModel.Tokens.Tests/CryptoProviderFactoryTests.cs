// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

using ALG = Microsoft.IdentityModel.Tokens.SecurityAlgorithms;
using EE = Microsoft.IdentityModel.TestUtils.ExpectedException;
using KM = Microsoft.IdentityModel.TestUtils.KeyingMaterial;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Tests
{
    /// <summary>
    /// Tests for CryptoProviderFactory
    /// </summary>
    public class CryptoProviderFactoryTests
    {
        /// <summary>
        /// This test checks that SignatureProviders are properly created and released when CryptoProviderFactory.CacheSignatureProviders = false.
        /// </summary>
        [Theory, MemberData(nameof(CreateAndReleaseSignatureProvidersTheoryData))]
        public void CreateAndReleaseSignatureProviders(SignatureProviderTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateAndReleaseSignatureProvidersTheoryData", theoryData);
            var cryptoProviderFactory = new CryptoProviderFactory(CryptoProviderCacheTests.CreateCacheForTesting()) { CacheSignatureProviders = false };
            try
            {
                var signatureProvider = cryptoProviderFactory.CreateForSigning(theoryData.SigningKey, theoryData.SigningAlgorithm);
                if (cryptoProviderFactory.CryptoProviderCache.TryGetSignatureProvider(theoryData.SigningKey, theoryData.SigningAlgorithm, theoryData.SignatureProviderType, true, out var _))
                    context.Diffs.Add("A SignatureProvider was added to CryptoProviderFactory.CryptoProviderCache, but CryptoProviderFactory.CacheSignatureProviders is false.");

                cryptoProviderFactory.ReleaseSignatureProvider(signatureProvider);

                // If the signatureProvider is cached Dispose() will not be called on it.
                if (signatureProvider.GetType().Equals(typeof(AsymmetricSignatureProvider)))
                {
                    var disposeCalled = GetSignatureProviderIsDisposedByReflect(signatureProvider);
                    if (!disposeCalled)
                        context.Diffs.Add("Dispose wasn't called on the AsymmetricSignatureProvider.");
                }
                else // signatureProvider.GetType().Equals(typeof(SymmetricSignatureProvider))
                {
                    var disposeCalled = GetSignatureProviderIsDisposedByReflect(signatureProvider);
                    if (!disposeCalled)
                        context.Diffs.Add("Dispose wasn't called on the SymmetricSignatureProvider.");
                }
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            theoryData.ExpectedException.ProcessNoException(context);
            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SignatureProviderTheoryData> CreateAndReleaseSignatureProvidersTheoryData
        {
            get
            {
                return new TheoryData<SignatureProviderTheoryData>
                {
                    new SignatureProviderTheoryData
                    {
                        First = true,
                        SigningKey = Default.AsymmetricSigningKey,
                        SigningAlgorithm = Default.AsymmetricSigningAlgorithm,
                        SignatureProviderType = typeof(AsymmetricSignatureProvider).ToString(),
                        TestId = "Asymmetric"
                    },
                    new SignatureProviderTheoryData
                    {
                        SigningKey = Default.SymmetricSigningKey,
                        SigningAlgorithm = ALG.HmacSha256,
                        SignatureProviderType = typeof(SymmetricSignatureProvider).ToString(),
                        TestId = "Symmetric"
                    },
                };
            }
        }

        /// <summary>
        /// Tests that defaults haven't changed.
        /// </summary>
        [Fact]
        public void Defaults()
        {
            TestUtilities.WriteHeader($"{this}.Defaults");
            var context = new CompareContext($"{this}.Defaults");

            var cryptoFactory1 = CryptoProviderFactory.Default;
            var cryptoFactory2 = CryptoProviderFactory.Default;
            if (!object.ReferenceEquals(cryptoFactory1, cryptoFactory2))
                context.Diffs.Add("!object.ReferenceEquals(cryptoFactory1, cryptoFactory2)");

            if (!cryptoFactory2.CacheSignatureProviders)
                context.Diffs.Add("cryptoFactory2.CacheSignatureProviders should be true");

            if (cryptoFactory1.CustomCryptoProvider != null)
                context.Diffs.Add("cryptoFactory2.CustomCryptoProvider should be NULL");

            if (typeof(InMemoryCryptoProviderCache) != cryptoFactory1.CryptoProviderCache.GetType())
                context.Diffs.Add("typeof(InMemoryCryptoProviderCache) != cryptoFactory1.CryptoProviderCache.GetType()");

            if (cryptoFactory1.SignatureProviderObjectPoolCacheSize != CryptoProviderFactory.DefaultSignatureProviderObjectPoolCacheSize)
                context.Diffs.Add("cryptoFactory1.SignatureProviderObjectPoolCacheSize != CryptoProviderFactory.DefaultSignatureProviderObjectPoolCacheSize");

            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void GetSets()
        {
            TestUtilities.WriteHeader($"{this}.GetSets");
            var context = new CompareContext($"{this}.GetSets");

            var cryptoProviderFactory = new CryptoProviderFactory(CryptoProviderCacheTests.CreateCacheForTesting());

            Type type = typeof(CryptoProviderFactory);
            PropertyInfo[] properties = type.GetProperties();
            if (properties.Length != 7)
                Assert.True(false, "Number of public fields has changed from 7 to: " + properties.Length + ", adjust tests");

            CustomCryptoProvider customCryptoProvider = new CustomCryptoProvider();
            GetSetContext getSetContext =
                new GetSetContext
                {
                    PropertyNamesAndSetGetValue = new List<KeyValuePair<string, List<object>>>
                    {
                        new KeyValuePair<string, List<object>>("SignatureProviderObjectPoolCacheSize", new List<object>{CryptoProviderFactory.DefaultSignatureProviderObjectPoolCacheSize, 20, 10}),
                        new KeyValuePair<string, List<object>>("CacheSignatureProviders", new List<object>{CryptoProviderFactory.DefaultCacheSignatureProviders, false, true}),
                        new KeyValuePair<string, List<object>>("CustomCryptoProvider", new List<object>{(ICryptoProvider)null, customCryptoProvider, null}),
                    },
                    Object = cryptoProviderFactory,
                };

            TestUtilities.GetSet(getSetContext);

            cryptoProviderFactory.SignatureProviderObjectPoolCacheSize = 42;
            cryptoProviderFactory.CacheSignatureProviders = false;
            cryptoProviderFactory.CustomCryptoProvider = customCryptoProvider;
            CryptoProviderFactory clone = new CryptoProviderFactory(cryptoProviderFactory);
            IdentityComparer.CompareAllPublicProperties(clone, cryptoProviderFactory, context);

            try
            {
                cryptoProviderFactory.SignatureProviderObjectPoolCacheSize = 0;
                context.AddDiff("cryptoProviderFactory.SignatureProviderObjectPoolCacheSize = 0; - Succeeded");
            }
            catch
            {
            }

            try
            {
                cryptoProviderFactory.SignatureProviderObjectPoolCacheSize = -1;
                context.AddDiff("cryptoProviderFactory.SignatureProviderObjectPoolCacheSize = -1; - Succeeded");
            }
            catch
            {
            }

            if (cryptoProviderFactory.CryptoProviderCache is IDisposable disposable)
                disposable?.Dispose();

            if (clone.CryptoProviderCache is IDisposable disposableClone)
                disposableClone?.Dispose();

            context.Diffs.AddRange(getSetContext.Errors);
            TestUtilities.AssertFailIfErrors(context);
        }

        /// <summary>
        /// Tests that SymmetricSignatureProviders that fault will be removed from cache
        /// </summary>
        /// <param name="theoryData"></param>
        [Theory, MemberData(nameof(FaultingAsymmetricSignatureProvidersTheoryData))]
        public void FaultingAsymmetricSignatureProviders(SignatureProviderTheoryData theoryData)
        {
            IdentityModelEventSource.ShowPII = true;
            var context = TestUtilities.WriteHeader($"{this}.FaultingAsymmetricSignatureProviders", theoryData);

            try
            {
                var bytes = new byte[256];
                var signingSignatureProvider = theoryData.CryptoProviderFactory.CreateForSigning(theoryData.SigningKey, theoryData.SigningAlgorithm) as AsymmetricSignatureProvider;
                var signedBytes = signingSignatureProvider.Sign(bytes);
                var verifyingSignatureProvider = theoryData.CryptoProviderFactory.CreateForVerifying(theoryData.VerifyKey, theoryData.VerifyAlgorithm) as AsymmetricSignatureProvider;
                verifyingSignatureProvider.Verify(bytes, signedBytes);

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            var signingProviderFound = theoryData.CryptoProviderFactory.CryptoProviderCache.TryGetSignatureProvider(theoryData.SigningKey, theoryData.SigningAlgorithm, theoryData.SigningSignatureProviderType, true, out SignatureProvider signingProvider);
            if (signingProviderFound != theoryData.ShouldFindSignSignatureProvider)
                context.Diffs.Add($"(signingProviderFound '{signingProviderFound}' != theoryData.ShouldFindSigningSignatureProvider: '{theoryData.ShouldFindSignSignatureProvider}'");

            var verifyingProviderFound = theoryData.CryptoProviderFactory.CryptoProviderCache.TryGetSignatureProvider(theoryData.VerifyKey, theoryData.VerifyAlgorithm, theoryData.VerifySignatureProviderType, false, out SignatureProvider verifyingProvider);
            if (verifyingProviderFound != theoryData.ShouldFindVerifySignatureProvider)
                context.Diffs.Add($"(verifyingSignatureProviderFound '{verifyingProviderFound}' != theoryData.ShouldFindVerifyingSignatureProvider: '{theoryData.ShouldFindSignSignatureProvider}'");

            TestUtilities.AssertFailIfErrors(context);
        }

        /// <summary>
        /// When a SignatureProvider faults, we want to remove it from the cache.
        /// Otherwise the fault will continue and there is no opportunity for recovery.
        /// </summary>
        public static TheoryData<SignatureProviderTheoryData> FaultingAsymmetricSignatureProvidersTheoryData
        {
            get
            {
                var theoryData = new TheoryData<SignatureProviderTheoryData>();

                // signing dispose fault
                var signingSignatureProvider = new CustomAsymmetricSignatureProvider(Default.AsymmetricSigningKey, ALG.RsaSha256, true);
                signingSignatureProvider.Dispose();
                theoryData.Add(new SignatureProviderTheoryData
                {
                    First = true,
                    ExpectedException = EE.ObjectDisposedException,
                    CryptoProviderFactory = new CustomCryptoProviderFactory(new string[] { ALG.RsaSha256 })
                    {
                        SigningSignatureProvider = signingSignatureProvider
                    },
                    ShouldFindSignSignatureProvider = false,
                    ShouldFindVerifySignatureProvider = false,
                    SigningAlgorithm = ALG.RsaSha256,
                    SigningKey = Default.AsymmetricSigningKey,
                    SigningSignatureProviderType = typeof(CustomAsymmetricSignatureProvider).ToString(),
                    VerifyAlgorithm = ALG.RsaSha256,
                    VerifyKey = Default.AsymmetricSigningKeyPublic,
                    VerifySignatureProviderType = typeof(CustomAsymmetricSignatureProvider).ToString(),
                    TestId = "SignDisposeFault"
                });

                // verify dispose fault
                signingSignatureProvider = new CustomAsymmetricSignatureProvider(Default.AsymmetricSigningKey, ALG.RsaSha256, true);
                var verifyingSignatureProvider = new CustomAsymmetricSignatureProvider(Default.AsymmetricSigningKeyPublic, ALG.RsaSha256, false);
                verifyingSignatureProvider.Dispose();
                theoryData.Add(new SignatureProviderTheoryData
                {
                    First = true,
                    ExpectedException = EE.ObjectDisposedException,
                    CryptoProviderFactory = new CustomCryptoProviderFactory(new string[] { ALG.RsaSha256 })
                    {
                        SigningSignatureProvider = signingSignatureProvider,
                        VerifyingSignatureProvider = verifyingSignatureProvider
                    },
                    ShouldFindSignSignatureProvider = true,
                    ShouldFindVerifySignatureProvider = false,
                    SigningAlgorithm = ALG.RsaSha256,
                    SigningKey = Default.AsymmetricSigningKey,
                    SigningSignatureProviderType = typeof(CustomAsymmetricSignatureProvider).ToString(),
                    VerifyAlgorithm = ALG.RsaSha256,
                    VerifyKey = Default.AsymmetricSigningKeyPublic,
                    VerifySignatureProviderType = typeof(CustomAsymmetricSignatureProvider).ToString(),
                    TestId = "VerifyDisposeFault"
                });

                // signing public key fault
                var signingKey = new CustomRsaSecurityKey(2048, PrivateKeyStatus.Exists, KM.RsaParameters_2048_Public);
                signingSignatureProvider = new CustomAsymmetricSignatureProvider(signingKey, ALG.RsaSha256, true);
                theoryData.Add(new SignatureProviderTheoryData
                {
                    First = true,
                    ExpectedException = new EE(typeof(Exception)){IgnoreExceptionType = true},
                    CryptoProviderFactory = new CustomCryptoProviderFactory(new string[] { ALG.RsaSha256 })
                    {
                        SigningSignatureProvider = signingSignatureProvider
                    },
                    ShouldFindSignSignatureProvider = false,
                    ShouldFindVerifySignatureProvider = false,
                    SigningAlgorithm = ALG.RsaSha256,
                    SigningKey = signingKey,
                    SigningSignatureProviderType = typeof(CustomAsymmetricSignatureProvider).ToString(),
                    VerifyAlgorithm = ALG.RsaSha256,
                    VerifyKey = Default.AsymmetricSigningKeyPublic,
                    VerifySignatureProviderType = typeof(CustomAsymmetricSignatureProvider).ToString(),
                    TestId = "SignPublicKeyFault"
                });

                return theoryData;
            }
        }

        /// <summary>
        /// Tests that SymmetricSignatureProviders that fault will be removed from cache
        /// </summary>
        /// <param name="theoryData"></param>
        [Theory, MemberData(nameof(FaultingSymmetricSignatureProvidersTheoryData))]
        public void FaultingSymmetricSignatureProviders(SignatureProviderTheoryData theoryData)
        {
            IdentityModelEventSource.ShowPII = true;
            var context = TestUtilities.WriteHeader($"{this}.FaultingSymmetricSignatureProviders", theoryData);

            try
            {
                var bytes = new byte[256];
                var signingSignatureProvider = theoryData.CryptoProviderFactory.CreateForSigning(theoryData.SigningKey, theoryData.SigningAlgorithm) as SymmetricSignatureProvider;
                var signedBytes = signingSignatureProvider.Sign(bytes);
                var verifyingSignatureProvider = theoryData.CryptoProviderFactory.CreateForVerifying(theoryData.VerifyKey, theoryData.VerifyAlgorithm) as SymmetricSignatureProvider;
                if (theoryData.VerifySpecifyingLength)
                    verifyingSignatureProvider.Verify(bytes, signedBytes);
                else
                    verifyingSignatureProvider.Verify(bytes, signedBytes, signedBytes.Length);

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            var signProviderFound = theoryData.CryptoProviderFactory.CryptoProviderCache.TryGetSignatureProvider(theoryData.SigningKey, theoryData.SigningAlgorithm, theoryData.SigningSignatureProviderType, true, out SignatureProvider signingProvider);
            if (signProviderFound != theoryData.ShouldFindSignSignatureProvider)
                context.Diffs.Add($"signingProviderFound '{signProviderFound}' != theoryData.ShouldFindSignSignatureProvider: '{theoryData.ShouldFindSignSignatureProvider}'");

            var verifyProviderFound = theoryData.CryptoProviderFactory.CryptoProviderCache.TryGetSignatureProvider(theoryData.VerifyKey, theoryData.VerifyAlgorithm, theoryData.VerifySignatureProviderType, false, out SignatureProvider verifyingProvider);
            if (verifyProviderFound != theoryData.ShouldFindVerifySignatureProvider)
                context.Diffs.Add($"verifySignatureProviderFound '{verifyProviderFound}' != theoryData.ShouldFindVerifySignatureProvider: '{theoryData.ShouldFindVerifySignatureProvider}'");

            TestUtilities.AssertFailIfErrors(context);
        }

        /// <summary>
        /// When a SignatureProvider faults, we want to remove it from the cache.
        /// Otherwise the fault will continue and the next usage will result in a new provider
        /// </summary>
        public static TheoryData<SignatureProviderTheoryData> FaultingSymmetricSignatureProvidersTheoryData
        {
            get
            {
                var theoryData = new TheoryData<SignatureProviderTheoryData>();

                // signing will fault, signingSignatureProvider should be removed from cache,no need for verifying signature provider
                theoryData.Add(new SignatureProviderTheoryData
                {
                    First = true,
                    ExpectedException = EE.CryptographicException("KeyedHashAlgorithmThrowOnHashFinal"),
                    CryptoProviderFactory = new CustomCryptoProviderFactory(new string[] { ALG.HmacSha256 })
                    {
                        SigningSignatureProvider = new CustomSymmetricSignatureProvider(Default.SymmetricSigningKey256, ALG.HmacSha256, true)
                        {
                            KeyedHashAlgorithmPublic = new CustomKeyedHashAlgorithm(Default.SymmetricSigningKey256.Key)
                            {
                                ThrowOnHashFinal = new CryptographicException("KeyedHashAlgorithmThrowOnHashFinal")
                            }
                        },
                    },
                    ShouldFindSignSignatureProvider = false,
                    ShouldFindVerifySignatureProvider = false,
                    SigningAlgorithm = ALG.HmacSha256,
                    SigningKey = Default.SymmetricSigningKey256,
                    SigningSignatureProviderType = typeof(CustomSymmetricSignatureProvider).ToString(),
                    TestId = "SignKeyedHashFault",
                    VerifyAlgorithm = ALG.HmacSha256,
                    VerifyKey = Default.SymmetricSigningKey256,
                    VerifySignatureProviderType = typeof(CustomSymmetricSignatureProvider).ToString()
                });

                // verifying will fault, verifying and signingSignatureProvider should be removed from cache since in symmetric case
                // they are the same.
                theoryData.Add(new SignatureProviderTheoryData
                {
                    ExpectedException = EE.CryptographicException("KeyedHashAlgorithmThrowOnHashFinal"),
                    CryptoProviderFactory = new CustomCryptoProviderFactory(new string[] { ALG.HmacSha256 })
                    {
                        SigningSignatureProvider = new CustomSymmetricSignatureProvider(Default.SymmetricSigningKey256, ALG.HmacSha256, true),
                        VerifyingSignatureProvider = new CustomSymmetricSignatureProvider(Default.SymmetricSigningKey256, ALG.HmacSha256, false)
                        {
                            KeyedHashAlgorithmPublic = new CustomKeyedHashAlgorithm(Default.SymmetricSigningKey256.Key)
                            {
                                ThrowOnHashFinal = new CryptographicException("KeyedHashAlgorithmThrowOnHashFinal")
                            }
                        },
                    },
                    ShouldFindSignSignatureProvider = true,
                    ShouldFindVerifySignatureProvider = false,
                    SigningAlgorithm = ALG.HmacSha256,
                    SigningKey = Default.SymmetricSigningKey256,
                    SigningSignatureProviderType = typeof(CustomSymmetricSignatureProvider).ToString(),
                    TestId = "VerifyKeyedHashFault",
                    VerifyAlgorithm = ALG.HmacSha256,
                    VerifyKey = Default.SymmetricSigningKey256,
                    VerifySignatureProviderType = typeof(CustomSymmetricSignatureProvider).ToString()
                });

                // verifying will fault, verifying and signingSignatureProvider should be removed from cache since in symmetric case
                // they are the same.
                theoryData.Add(new SignatureProviderTheoryData
                {
                    ExpectedException = EE.CryptographicException("KeyedHashAlgorithmThrowOnHashFinal"),
                    CryptoProviderFactory = new CustomCryptoProviderFactory(new string[] { ALG.HmacSha256 })
                    {
                        SigningSignatureProvider = new CustomSymmetricSignatureProvider(Default.SymmetricSigningKey256, ALG.HmacSha256, true),
                        VerifyingSignatureProvider = new CustomSymmetricSignatureProvider(Default.SymmetricSigningKey256, ALG.HmacSha256, false)
                        {
                            KeyedHashAlgorithmPublic = new CustomKeyedHashAlgorithm(Default.SymmetricSigningKey256.Key)
                            {
                                ThrowOnHashFinal = new CryptographicException("KeyedHashAlgorithmThrowOnHashFinal")
                            }
                        },
                    },
                    ShouldFindSignSignatureProvider = true,
                    ShouldFindVerifySignatureProvider = false,
                    SigningAlgorithm = ALG.HmacSha256,
                    SigningKey = Default.SymmetricSigningKey256,
                    SigningSignatureProviderType = typeof(CustomSymmetricSignatureProvider).ToString(),
                    TestId = "VerifySpecifyingLengthKeyedHashFault",
                    VerifyAlgorithm = ALG.HmacSha256,
                    VerifyKey = Default.SymmetricSigningKey256,
                    VerifySignatureProviderType = typeof(CustomSymmetricSignatureProvider).ToString(),
                    VerifySpecifyingLength = true
                });

                // Symmetric disposed signing
                var signingSignatureProvider = new CustomSymmetricSignatureProvider(Default.SymmetricSigningKey256, ALG.HmacSha256, true);
                signingSignatureProvider.Dispose();
                var verifyingSignatureProvider = new CustomSymmetricSignatureProvider(Default.SymmetricSigningKey256, ALG.HmacSha256, false);
                theoryData.Add(new SignatureProviderTheoryData
                {
                    ExpectedException = EE.ObjectDisposedException,
                    CryptoProviderFactory = new CustomCryptoProviderFactory(new string[] { ALG.HmacSha256 })
                    {
                        SigningSignatureProvider = signingSignatureProvider,
                        VerifyingSignatureProvider = verifyingSignatureProvider,
                    },
                    ShouldFindSignSignatureProvider = false,
                    ShouldFindVerifySignatureProvider = false,
                    SigningAlgorithm = ALG.HmacSha256,
                    SigningKey = Default.SymmetricSigningKey256,
                    SigningSignatureProviderType = typeof(CustomSymmetricSignatureProvider).ToString(),
                    TestId = "SignDisposedFault",
                    VerifyAlgorithm = ALG.HmacSha256,
                    VerifyKey = Default.SymmetricSigningKey256,
                    VerifySignatureProviderType = typeof(CustomSymmetricSignatureProvider).ToString()
                });

                // Symmetric disposed verifying
                signingSignatureProvider = new CustomSymmetricSignatureProvider(Default.SymmetricSigningKey256, ALG.HmacSha256, true);
                verifyingSignatureProvider = new CustomSymmetricSignatureProvider(Default.SymmetricSigningKey256, ALG.HmacSha256, false);
                verifyingSignatureProvider.Dispose();
                theoryData.Add(new SignatureProviderTheoryData
                {
                    ExpectedException = EE.ObjectDisposedException,
                    CryptoProviderFactory = new CustomCryptoProviderFactory(new string[] { ALG.HmacSha256 })
                    {
                        SigningSignatureProvider = signingSignatureProvider,
                        VerifyingSignatureProvider = verifyingSignatureProvider,
                    },
                    ShouldFindSignSignatureProvider = true,
                    ShouldFindVerifySignatureProvider = false,
                    SigningAlgorithm = ALG.HmacSha256,
                    SigningKey = Default.SymmetricSigningKey256,
                    SigningSignatureProviderType = typeof(CustomSymmetricSignatureProvider).ToString(),
                    TestId = "VerifyDisposeFault",
                    VerifyAlgorithm = ALG.HmacSha256,
                    VerifyKey = Default.SymmetricSigningKey256,
                    VerifySignatureProviderType = typeof(CustomSymmetricSignatureProvider).ToString()
                });

                // Symmetric disposed verifying (specifying length)
                signingSignatureProvider = new CustomSymmetricSignatureProvider(Default.SymmetricSigningKey256, ALG.HmacSha256, true);
                verifyingSignatureProvider = new CustomSymmetricSignatureProvider(Default.SymmetricSigningKey256, ALG.HmacSha256, false);
                verifyingSignatureProvider.Dispose();
                theoryData.Add(new SignatureProviderTheoryData
                {
                    ExpectedException = EE.ObjectDisposedException,
                    CryptoProviderFactory = new CustomCryptoProviderFactory(new string[] { ALG.HmacSha256 })
                    {
                        SigningSignatureProvider = signingSignatureProvider,
                        VerifyingSignatureProvider = verifyingSignatureProvider,
                    },
                    ShouldFindSignSignatureProvider = true,
                    ShouldFindVerifySignatureProvider = false,
                    SigningAlgorithm = ALG.HmacSha256,
                    SigningKey = Default.SymmetricSigningKey256,
                    SigningSignatureProviderType = typeof(CustomSymmetricSignatureProvider).ToString(),
                    TestId = "VerifySpecifyingLengthDisposedFault",
                    VerifyAlgorithm = ALG.HmacSha256,
                    VerifyKey = Default.SymmetricSigningKey256,
                    VerifySignatureProviderType = typeof(CustomSymmetricSignatureProvider).ToString(),
                    VerifySpecifyingLength = true
                });

                // Symmetric signing verifying succeed
                signingSignatureProvider = new CustomSymmetricSignatureProvider(Default.SymmetricSigningKey256, ALG.HmacSha256, true);
                verifyingSignatureProvider = new CustomSymmetricSignatureProvider(Default.SymmetricSigningKey256, ALG.HmacSha256, false);
                theoryData.Add(new SignatureProviderTheoryData
                {
                    CryptoProviderFactory = new CustomCryptoProviderFactory(new string[] { ALG.HmacSha256 })
                    {
                        SigningSignatureProvider = signingSignatureProvider,
                        VerifyingSignatureProvider = verifyingSignatureProvider,
                    },
                    ShouldFindSignSignatureProvider = true,
                    ShouldFindVerifySignatureProvider = true,
                    SigningAlgorithm = ALG.HmacSha256,
                    SigningKey = Default.SymmetricSigningKey256,
                    SigningSignatureProviderType = typeof(CustomSymmetricSignatureProvider).ToString(),
                    TestId = "SignVerifySucceed",
                    VerifyAlgorithm = ALG.HmacSha256,
                    VerifyKey = Default.SymmetricSigningKey256,
                    VerifySignatureProviderType = typeof(CustomSymmetricSignatureProvider).ToString(),
                    VerifySpecifyingLength = false
                });

                // Symmetric signing verifying (specifying length) succeed
                signingSignatureProvider = new CustomSymmetricSignatureProvider(Default.SymmetricSigningKey256, ALG.HmacSha256, true);
                verifyingSignatureProvider = new CustomSymmetricSignatureProvider(Default.SymmetricSigningKey256, ALG.HmacSha256, false);
                theoryData.Add(new SignatureProviderTheoryData
                {
                    CryptoProviderFactory = new CustomCryptoProviderFactory(new string[] { ALG.HmacSha256 })
                    {
                        SigningSignatureProvider = signingSignatureProvider,
                        VerifyingSignatureProvider = verifyingSignatureProvider,
                    },
                    ShouldFindSignSignatureProvider = true,
                    ShouldFindVerifySignatureProvider = true,
                    SigningAlgorithm = ALG.HmacSha256,
                    SigningKey = Default.SymmetricSigningKey256,
                    SigningSignatureProviderType = typeof(CustomSymmetricSignatureProvider).ToString(),
                    TestId = "SignVerifySpecifyingLengthSucceed",
                    VerifyAlgorithm = ALG.HmacSha256,
                    VerifyKey = Default.SymmetricSigningKey256,
                    VerifySignatureProviderType = typeof(CustomSymmetricSignatureProvider).ToString(),
                    VerifySpecifyingLength = true
                });

                return theoryData;
            }
        }

        [Theory, MemberData(nameof(ReleaseSignatureProvidersTheoryData))]
        public void ReleaseSignatureProviders(SignatureProviderTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ReleaseSignatureProviders", theoryData);

            var cryptoProviderFactory = new CryptoProviderFactory(CryptoProviderCacheTests.CreateCacheForTesting());
            // turning off caching also turns off ref counting considerations in the dipose algorithm.
            cryptoProviderFactory.CacheSignatureProviders = false;

            try
            {
                if (theoryData.CustomCryptoProvider != null)
                    cryptoProviderFactory.CustomCryptoProvider = theoryData.CustomCryptoProvider;
                cryptoProviderFactory.ReleaseSignatureProvider(theoryData.SigningSignatureProvider);
                if (theoryData.CustomCryptoProvider != null && theoryData.SigningSignatureProvider != null && !((CustomCryptoProvider)theoryData.CustomCryptoProvider).ReleaseCalled)
                    context.Diffs.Add("Release wasn't called on the CustomCryptoProvider.");
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SignatureProviderTheoryData> ReleaseSignatureProvidersTheoryData
        {
            get
            {
                var cache = CryptoProviderCacheTests.CreateCacheForTesting();
                var asymmetricSignatureProvider = new CustomAsymmetricSignatureProvider(Default.AsymmetricSigningKey, Default.AsymmetricSigningAlgorithm, true) { ThrowOnDispose = new InvalidOperationException() };
                var asymmetricSignatureProviderToRelease = new CustomAsymmetricSignatureProvider(Default.AsymmetricSigningKey, Default.AsymmetricSigningAlgorithm, true);
                var symmetricSignatureProvider = new CustomSymmetricSignatureProvider(Default.SymmetricSigningKey256, ALG.HmacSha256, true) { ThrowOnDispose = new InvalidOperationException() };
                var asymmetricSignatureProviderCached = new CustomAsymmetricSignatureProvider(Default.AsymmetricSigningKey, Default.AsymmetricSigningAlgorithm, true) { ThrowOnDispose = new InvalidOperationException() };
                var symmetricSignatureProviderCached = new CustomSymmetricSignatureProvider(Default.SymmetricSigningKey256, ALG.HmacSha256, true) { ThrowOnDispose = new InvalidOperationException() };
                cache.TryAdd(asymmetricSignatureProviderCached);
                cache.TryAdd(symmetricSignatureProviderCached);

                var theoryData = new TheoryData<SignatureProviderTheoryData>
                {
                    new SignatureProviderTheoryData
                    {
                        ExpectedException = EE.InvalidOperationException(),
                        First = true,
                        SigningSignatureProvider = asymmetricSignatureProvider,
                        TestId = "Release1"
                    },
                    new SignatureProviderTheoryData
                    {
                        SigningSignatureProvider = asymmetricSignatureProviderCached,
                        TestId = "Release2"
                    },
                    new SignatureProviderTheoryData
                    {
                        ExpectedException = EE.InvalidOperationException(),
                        SigningSignatureProvider = symmetricSignatureProvider,
                        TestId = "Release3"
                    },
                    new SignatureProviderTheoryData
                    {
                        SigningSignatureProvider = symmetricSignatureProviderCached,
                        TestId = "Release4"
                    },
                    new SignatureProviderTheoryData
                    {
                       CustomCryptoProvider = new CustomCryptoProvider(new string[] {"RS256"})
                       {
                           SignatureProvider = asymmetricSignatureProviderToRelease
                       },
                       SigningSignatureProvider = asymmetricSignatureProviderToRelease,
                       TestId = "CustomCryptoProviderRelease"
                    },
                    new SignatureProviderTheoryData
                    {
                       ExpectedException = EE.ArgumentNullException(),
                       CustomCryptoProvider = new CustomCryptoProvider(new string[] {"RS256"})
                       {
                           SignatureProvider = asymmetricSignatureProviderToRelease
                       },
                       SigningSignatureProvider = null,
                       TestId = "CustomCryptoProviderRelease - SignatureProvider null"
                    }
                };

                return theoryData;
            }
        }

        [Theory, MemberData(nameof(ReleaseHashAlgorithmsTheoryData))]
        public void ReleaseHashAlgorithms(CryptoProviderFactoryTheoryData theoryData)
        {
            IdentityModelEventSource.ShowPII = true;
            var context = TestUtilities.WriteHeader($"{this}.ReleaseHashAlgorithms", theoryData);
            var cryptoProviderFactory = theoryData.CryptoProviderFactory;
            try
            {
                cryptoProviderFactory.ReleaseHashAlgorithm(theoryData.HashAlgorithm);
                if (theoryData.CustomCryptoProvider != null && theoryData.HashAlgorithm != null && !((CustomCryptoProvider)theoryData.CustomCryptoProvider).ReleaseCalled)
                    context.Diffs.Add("Release wasn't called on the CustomCryptoProvider.");
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CryptoProviderFactoryTheoryData> ReleaseHashAlgorithmsTheoryData
        {
            get
            {
                var customCryptoProvider = new CustomCryptoProvider(new string[] { SecurityAlgorithms.Sha256 })
                {
                    HashAlgorithm = Default.HashAlgorithm
                };
                var cryptoProviderFactory = new CryptoProviderFactory() { CustomCryptoProvider = customCryptoProvider };

                var theoryData = new TheoryData<CryptoProviderFactoryTheoryData>
                {
                    new CryptoProviderFactoryTheoryData
                    {
                       First = true,
                       CustomCryptoProvider = customCryptoProvider,
                       CryptoProviderFactory = cryptoProviderFactory,
                       HashAlgorithm = (HashAlgorithm) cryptoProviderFactory.CreateHashAlgorithm(SecurityAlgorithms.Sha256),
                       TestId = "CustomCryptoProviderRelease"
                    },
                    new CryptoProviderFactoryTheoryData
                    {
                       ExpectedException = EE.ArgumentNullException(),
                       CustomCryptoProvider = customCryptoProvider,
                       CryptoProviderFactory = cryptoProviderFactory,
                       HashAlgorithm = null,
                       TestId = "CustomCryptoProviderRelease - HashAlgorithm null"
                    }
                };

                return theoryData;
            }
        }

        [Theory, MemberData(nameof(ReleaseKeyWrapProvidersTheoryData))]
        public void ReleaseKeyWrapProviders(CryptoProviderFactoryTheoryData theoryData)
        {
            IdentityModelEventSource.ShowPII = true;
            var context = TestUtilities.WriteHeader($"{this}.ReleaseKeyWrapProviders", theoryData);
            var cryptoProviderFactory = theoryData.CryptoProviderFactory;
            try
            {
                cryptoProviderFactory.ReleaseKeyWrapProvider(theoryData.KeyWrapProvider);
                if (theoryData.CustomCryptoProvider != null && theoryData.KeyWrapProvider != null && !((CustomCryptoProvider)theoryData.CustomCryptoProvider).ReleaseCalled)
                    context.Diffs.Add("Release wasn't called on the CustomCryptoProvider.");
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CryptoProviderFactoryTheoryData> ReleaseKeyWrapProvidersTheoryData
        {
            get
            {
                SecurityKey key = Default.SymmetricEncryptionKey128;
                var provider = key.CryptoProviderFactory.CreateKeyWrapProvider(key, SecurityAlgorithms.Aes128KW);
                var customCryptoProvider = new CustomCryptoProvider(new string[] { SecurityAlgorithms.Aes128KW })
                {
                    KeyWrapProvider = provider
                };
                var cryptoProviderFactory = new CryptoProviderFactory() { CustomCryptoProvider = customCryptoProvider };

                var theoryData = new TheoryData<CryptoProviderFactoryTheoryData>
                {
                    new CryptoProviderFactoryTheoryData
                    {
                       First = true,
                       CustomCryptoProvider = customCryptoProvider,
                       CryptoProviderFactory = cryptoProviderFactory,
                       KeyWrapProvider = provider,
                       TestId = "CustomCryptoProviderRelease"
                    },
                    new CryptoProviderFactoryTheoryData
                    {
                       ExpectedException = EE.ArgumentNullException(),
                       CustomCryptoProvider = customCryptoProvider,
                       CryptoProviderFactory = cryptoProviderFactory,
                       KeyWrapProvider = null,
                       TestId = "CustomCryptoProviderRelease - KeyWrapProvider null"
                    }
                };

                return theoryData;
            }
        }

        [Theory, MemberData(nameof(ReleaseRsaKeyWrapProvidersTheoryData))]
        public void ReleaseRsaKeyWrapProviders(CryptoProviderFactoryTheoryData theoryData)
        {
            IdentityModelEventSource.ShowPII = true;
            var context = TestUtilities.WriteHeader($"{this}.ReleaseRsaKeyWrapProviders", theoryData);
            var cryptoProviderFactory = theoryData.CryptoProviderFactory;
            try
            {
                cryptoProviderFactory.ReleaseKeyWrapProvider(theoryData.RsaKeyWrapProvider);
                if (theoryData.CustomCryptoProvider != null && theoryData.RsaKeyWrapProvider != null && !((CustomCryptoProvider)theoryData.CustomCryptoProvider).ReleaseCalled)
                    context.Diffs.Add("Release wasn't called on the CustomCryptoProvider.");
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CryptoProviderFactoryTheoryData> ReleaseRsaKeyWrapProvidersTheoryData
        {
            get
            {
                SecurityKey key = Default.SymmetricEncryptionKey128;
                var provider = (RsaKeyWrapProvider)key.CryptoProviderFactory.CreateKeyWrapProvider(KeyingMaterial.RsaSecurityKey1, SecurityAlgorithms.RsaPKCS1);
                var customCryptoProvider = new CustomCryptoProvider(new string[] { SecurityAlgorithms.RsaPKCS1 })
                {
                    RsaKeyWrapProvider = provider
                };
                var cryptoProviderFactory = new CryptoProviderFactory() { CustomCryptoProvider = customCryptoProvider };

                var theoryData = new TheoryData<CryptoProviderFactoryTheoryData>
                {
                    new CryptoProviderFactoryTheoryData
                    {
                       First = true,
                       CustomCryptoProvider = customCryptoProvider,
                       CryptoProviderFactory = cryptoProviderFactory,
                       RsaKeyWrapProvider = provider,
                       TestId = "CustomCryptoProviderRelease"
                    },
                    new CryptoProviderFactoryTheoryData
                    {
                       ExpectedException = EE.ArgumentNullException(),
                       CustomCryptoProvider = customCryptoProvider,
                       CryptoProviderFactory = cryptoProviderFactory,
                       RsaKeyWrapProvider = null,
                       TestId = "CustomCryptoProviderRelease - RsaKeyWrapProvider null"
                    }
                };

                return theoryData;
            }
        }

        [Fact]
        public void ShouldCacheSignatureProvider()
        {
            TestUtilities.WriteHeader($"{this}.ShouldCacheSignatureProvider");
            var context = new CompareContext($"{this}.ShouldCacheSignatureProvider");
            var signingKeyWithEmptyKid = new CustomRsaSecurityKey(1024, PrivateKeyStatus.Exists, KM.RsaParameters_1024);
            var signatureProvider = CryptoProviderFactory.Default.CreateForSigning(signingKeyWithEmptyKid, ALG.RsaSha256Signature);
            if (CryptoProviderFactory.Default.CryptoProviderCache.TryGetSignatureProvider(signingKeyWithEmptyKid, ALG.RsaSha256Signature, typeof(AsymmetricSignatureProvider).ToString(), true, out var _))
                context.Diffs.Add("A SignatureProvider was added to CryptoProviderFactory.CryptoProviderCache, but ShouldCacheSignatureProvider() should return false as the key has an empty key id.");

            CryptoProviderFactory.Default.ReleaseSignatureProvider(signatureProvider);

            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void ReferenceCountingTest()
        {
            var context = new CompareContext($"{this}.ReferenceCountingTest");
            var cryptoProviderFactory = new CryptoProviderFactory(CryptoProviderCacheTests.CreateCacheForTesting());

            var signing = cryptoProviderFactory.CreateForSigning(Default.AsymmetricSigningKey, Default.AsymmetricSigningAlgorithm);

            if (signing.RefCount != 1)
                context.AddDiff($"{nameof(signing)} reference count should have been 1");

            var signing2 = cryptoProviderFactory.CreateForSigning(Default.AsymmetricSigningKey, Default.AsymmetricSigningAlgorithm);

            if (signing.RefCount != 2)
                context.AddDiff($"{nameof(signing)} reference count should have been 2");

            if (signing2.RefCount != 2)
                context.AddDiff($"{nameof(signing2)} reference count should have been 2");

            cryptoProviderFactory.ReleaseSignatureProvider(signing2);

            if (signing2.RefCount != 1)
                context.AddDiff($"{nameof(signing2)} reference count should have been 1");

            if (GetSignatureProviderIsDisposedByReflect(signing2))
                context.AddDiff($"{nameof(signing2)} should NOT have been disposed");

            cryptoProviderFactory.ReleaseSignatureProvider(signing);

            if (signing.RefCount != 0)
                context.AddDiff($"{nameof(signing)} reference count should have been 0");

            if (GetSignatureProviderIsDisposedByReflect(signing))
                context.AddDiff($"{nameof(signing)} should NOT have been disposed");

            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void ReferenceCountingTest_NoCaching()
        {
            var context = new CompareContext($"{this}.ReferenceCountingTest_NoCaching");
            var cryptoProviderFactory = new CryptoProviderFactory(CryptoProviderCacheTests.CreateCacheForTesting());
            cryptoProviderFactory.CacheSignatureProviders = false;

            var signing = cryptoProviderFactory.CreateForSigning(Default.AsymmetricSigningKey, Default.AsymmetricSigningAlgorithm);
            var signing2 = cryptoProviderFactory.CreateForSigning(Default.AsymmetricSigningKey, Default.AsymmetricSigningAlgorithm);

            if (signing.RefCount != 1)
                context.AddDiff($"{nameof(signing)} reference count should have been 1");

            if (signing2.RefCount != 1)
                context.AddDiff($"{nameof(signing2)} reference count should have been 1");

            cryptoProviderFactory.ReleaseSignatureProvider(signing2);

            if (signing2.RefCount != 0)
                context.AddDiff($"{nameof(signing2)} reference count should have been 0");

            if (!GetSignatureProviderIsDisposedByReflect(signing2))
                context.AddDiff($"{nameof(signing2)} should not have been disposed");

            cryptoProviderFactory.ReleaseSignatureProvider(signing);

            if (!GetSignatureProviderIsDisposedByReflect(signing))
                context.AddDiff($"{nameof(signing)} should have been disposed");

            if (signing.RefCount != 0)
                context.AddDiff($"{nameof(signing)} reference count should have been 0");

            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void ReferenceCountingTest_Caching()
        {
            var context = new CompareContext($"{this}.ReferenceCountingTest");
            var cryptoProviderFactory = new CryptoProviderFactory(CryptoProviderCacheTests.CreateCacheForTesting());

            var signing = cryptoProviderFactory.CreateForSigning(Default.AsymmetricSigningKey, Default.AsymmetricSigningAlgorithm);
            var signing2 = cryptoProviderFactory.CreateForSigning(Default.AsymmetricSigningKey, Default.AsymmetricSigningAlgorithm);

            cryptoProviderFactory.CryptoProviderCache.TryRemove(signing2);

            if (signing.CryptoProviderCache != null)
                context.AddDiff($"{nameof(signing)} cache should be null");

            if (signing2.CryptoProviderCache != null)
                context.AddDiff($"{nameof(signing2)} cache should be null");

            cryptoProviderFactory.ReleaseSignatureProvider(signing2);

            if (GetSignatureProviderIsDisposedByReflect(signing2))
                context.AddDiff($"{nameof(signing2)} should not have been disposed");

            cryptoProviderFactory.ReleaseSignatureProvider(signing);

            if (!GetSignatureProviderIsDisposedByReflect(signing))
                context.AddDiff($"{nameof(signing2)} should have been disposed");

            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact (Skip = "too long")]
        public void ReferenceCountingTest_MultiThreaded()
        {
            var context = new CompareContext($"{this}.ReferenceCountingTest_MultiThreaded");
            var cryptoProviderFactory = new CryptoProviderFactory(CryptoProviderCacheTests.CreateCacheForTesting());

            Task[] tasks = new Task[100];

            for (int i = 0; i < 100; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    var rsaSha256 = cryptoProviderFactory.CreateForSigning(Default.AsymmetricSigningKey, Default.AsymmetricSigningAlgorithm);
                    var hmacSha256 = cryptoProviderFactory.CreateForSigning(Default.SymmetricSigningKey, SecurityAlgorithms.HmacSha256Signature);
                    var hmacSha512 = cryptoProviderFactory.CreateForSigning(Default.SymmetricSigningKey512, ALG.HmacSha512);
                    var hmacSha384 = cryptoProviderFactory.CreateForSigning(Default.SymmetricSigningKey384, ALG.HmacSha384);

                    var rsaSha256Verifying = cryptoProviderFactory.CreateForVerifying(Default.AsymmetricSigningKey, Default.AsymmetricSigningAlgorithm);
                    var hmacSha256Verifying = cryptoProviderFactory.CreateForVerifying(Default.SymmetricSigningKey, SecurityAlgorithms.HmacSha256Signature);
                    var hmacSha512Verifying = cryptoProviderFactory.CreateForVerifying(Default.SymmetricSigningKey512, ALG.HmacSha512);
                    var hmacSha384Verifying = cryptoProviderFactory.CreateForVerifying(Default.SymmetricSigningKey384, ALG.HmacSha384);

                    cryptoProviderFactory.ReleaseSignatureProvider(rsaSha256);
                    cryptoProviderFactory.ReleaseSignatureProvider(hmacSha256);
                    cryptoProviderFactory.ReleaseSignatureProvider(hmacSha512);
                    cryptoProviderFactory.ReleaseSignatureProvider(hmacSha384);

                    cryptoProviderFactory.ReleaseSignatureProvider(rsaSha256Verifying);
                    cryptoProviderFactory.ReleaseSignatureProvider(hmacSha256Verifying);
                    cryptoProviderFactory.ReleaseSignatureProvider(hmacSha512Verifying);
                    cryptoProviderFactory.ReleaseSignatureProvider(hmacSha384Verifying);
                });
            }

            Task.WaitAll(tasks);

            cryptoProviderFactory.CacheSignatureProviders = false;

            var rsaSha256Final = cryptoProviderFactory.CreateForSigning(Default.AsymmetricSigningKey, Default.AsymmetricSigningAlgorithm);
            var hmacSha256Final = cryptoProviderFactory.CreateForSigning(Default.SymmetricSigningKey, SecurityAlgorithms.HmacSha256Signature);
            var hmacSha512Final = cryptoProviderFactory.CreateForSigning(Default.SymmetricSigningKey512, ALG.HmacSha512);
            var hmacSha384Final = cryptoProviderFactory.CreateForSigning(Default.SymmetricSigningKey384, ALG.HmacSha384);

            var rsaSha256VerifyingFinal = cryptoProviderFactory.CreateForVerifying(Default.AsymmetricSigningKey, Default.AsymmetricSigningAlgorithm);
            var hmacSha256VerifyingFinal = cryptoProviderFactory.CreateForVerifying(Default.SymmetricSigningKey, SecurityAlgorithms.HmacSha256Signature);
            var hmacSha512VerifyingFinal = cryptoProviderFactory.CreateForVerifying(Default.SymmetricSigningKey512, ALG.HmacSha512);
            var hmacSha384VerifyingFinal = cryptoProviderFactory.CreateForVerifying(Default.SymmetricSigningKey384, ALG.HmacSha384);

            if (rsaSha256Final.RefCount != 1)
                context.AddDiff($"{nameof(rsaSha256Final)} reference count should have been 1");

            if (hmacSha256Final.RefCount != 1)
                context.AddDiff($"{nameof(hmacSha256Final)} reference count should have been 1");

            if (hmacSha512Final.RefCount != 1)
                context.AddDiff($"{nameof(hmacSha512Final)} reference count should have been 1");

            if (hmacSha384Final.RefCount != 1)
                context.AddDiff($"{nameof(hmacSha384Final)} reference count should have been 1");

            if (rsaSha256VerifyingFinal.RefCount != 1)
                context.AddDiff($"{nameof(rsaSha256VerifyingFinal)} reference count should have been 1");

            if (hmacSha256VerifyingFinal.RefCount != 1)
                context.AddDiff($"{nameof(hmacSha256VerifyingFinal)} reference count should have been 1");

            if (hmacSha512VerifyingFinal.RefCount != 1)
                context.AddDiff($"{nameof(hmacSha512VerifyingFinal)} reference count should have been 1");

            if (hmacSha384VerifyingFinal.RefCount != 1)
                context.AddDiff($"{nameof(hmacSha384VerifyingFinal)} reference count should have been 1");

            TestUtilities.AssertFailIfErrors(context);
        }

        /// <summary>
        /// Testing adding/removing providers to the Default cache w/o leaking task at the end of test.
        /// </summary>
        [Fact]
        public void ProviderCache_EnsureNoHangingTasks()
        {
            long taskIdleTimeoutInSeconds = 1;
            var cache = new InMemoryCryptoProviderCache();
            var factory = new CryptoProviderFactory(cache);

            // create signing providers
            var signingProviders = CreateSigningProviders(factory);

            // create verifying providers
            var verifyingProviders = CreateVerifyingProviders(factory);

            WaitTillTasksStarted(cache, taskIdleTimeoutInSeconds); // wait for the event queue task to start

            // make sure providers can be retrieved from the cache
            if (cache.TryGetSignatureProvider(Default.AsymmetricSigningKey, Default.AsymmetricSigningAlgorithm, typeof(AsymmetricSignatureProvider).ToString(), true, out var tmpProvider))
            {
                Assert.True(tmpProvider != null);
            }

            // remove all signing providers
            foreach (var provider in signingProviders)
                cache.TryRemove(provider);

            foreach (var provider in verifyingProviders)
                cache.TryRemove(provider);

            //=============================================================================================
            // repeat the steps and verify tasks will be restarted again and stopped when cache is empty...
            //=============================================================================================
            signingProviders = CreateSigningProviders(factory); // create signing providers

            WaitTillTasksStarted(cache, taskIdleTimeoutInSeconds); // wait for the event queue task to start

            // remove all signing providers
            foreach (var provider in signingProviders)
                cache.TryRemove(provider);

            // Dispose() should stop the event queue task if it is running.
            cache.Dispose();

            AssertNoHangingingTasks(cache, "ProviderCache_EnsureNoHangingTasks");
        }

        /// <summary>
        /// Test adding and removing providers by multiple threads w/o exception.
        /// </summary>
        [Fact]
        public void ProviderCache_EnsureNoException_MultipleThreads()
        {
            var cache = new InMemoryCryptoProviderCache();
            var factory = new CryptoProviderFactory(cache);

            int count = 5;
            List<Thread> signingThreads = new List<Thread>(count);
            for (int i = 0; i < count; i++)
            {
                var thread = new Thread(() => ThreadStartProcAddAndRemoveProviders(factory, CreateSigningProviders));
                thread.Start();
                signingThreads.Add(thread);
            }

            List<Thread> verifyingThreads = new List<Thread>(count);
            for (int i = 0; i < count; i++)
            {
                var thread = new Thread(() => ThreadStartProcAddAndRemoveProviders(factory, CreateVerifyingProviders));
                thread.Start();
                verifyingThreads.Add(thread);
            }

            // wait for all threads to finish
            foreach (Thread thread in signingThreads)
                thread.Join();

            foreach (Thread thread in verifyingThreads)
                thread.Join();

            // Dispose() should stop the event queue task if it is running.
            cache.Dispose();

            AssertNoHangingingTasks(cache, "ProviderCache_EnsureNoException_MultipleThreads");
        }

        /// <summary>
        /// Test to ensure no hanging task at the end when calling the JwtSecurityTokenHandler.WriteToken() method.
        /// The JwtHeader is created with SymmetricEncryptingCredentials.
        /// </summary>
        [Fact]
        public void ProviderCache_EnsureNoLeakingTasks_SecurityTokenHandler_SymmetricEncryptingCredentials()
        {
            var cache = new InMemoryCryptoProviderCache();
            CryptoProviderFactory cryptoProviderFactory = new CryptoProviderFactory(cache);

            var testClaims = new List<Claim>
            {
                new Claim(ClaimTypes.AuthenticationMethod, Default.AuthenticationMethod, ClaimValueTypes.String, Default.Issuer),
                new Claim(ClaimTypes.AuthenticationInstant, Default.AuthenticationInstant, ClaimValueTypes.DateTime, Default.Issuer)
            };

            var header = new JwtHeader(new EncryptingCredentials(
                    KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2.Key,
                    KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2.Alg,
                    KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2.Enc)
                    { CryptoProviderFactory = cryptoProviderFactory });

            JwtPayload payload = new JwtPayload("IssuerName", "Audience", testClaims, DateTime.Now.AddHours(-1), DateTime.Now.AddHours(1), DateTime.Now.AddHours(-1));
            var token = new JwtSecurityToken(header, payload);

            string certHash = "Test Cert Hash";
            token.Header[JwtHeaderParameterNames.X5t] = certHash;
            token.Header[JwtHeaderParameterNames.Kid] = certHash;

            var handler = new JwtSecurityTokenHandler();
            _ = handler.WriteToken(token);

            // Dispose() should stop the event queue task if it is running.
            cache.Dispose();

            // when JwtHeader is created with SymmetricEncryptingCredentials, the provider will not be added to cache (an error in logic???)
            AssertNoHangingingTasks(cache, "ProviderCache_EnsureNoLeakingTasks_SecurityTokenHandler_SymmetricEncryptingCredentials");
        }

        /// <summary>
        /// Test to ensure no hanging task at the end when calling the JwtSecurityTokenHandler.WriteToken() method.
        /// The JwtHeader is created with SigningCredentials.
        /// </summary>
        [Fact]
        public void ProviderCache_EnsureNoLeakingTasks_SecurityTokenHandler_SigningCredentials()
        {
            var cache = new InMemoryCryptoProviderCache();
            CryptoProviderFactory cryptoProviderFactory = new CryptoProviderFactory(cache);

            var testClaims = new List<Claim>
            {
                new Claim(ClaimTypes.AuthenticationMethod, Default.AuthenticationMethod, ClaimValueTypes.String, Default.Issuer),
                new Claim(ClaimTypes.AuthenticationInstant, Default.AuthenticationInstant, ClaimValueTypes.DateTime, Default.Issuer)
            };

            // create new key, set the newly created crypto provider factory on it
            var signingCredentials = new SigningCredentials(new X509SecurityKey(KeyingMaterial.DefaultCert_2048), SecurityAlgorithms.RsaSha256) { CryptoProviderFactory = cryptoProviderFactory };

            var token = new JwtSecurityToken(
                issuer: "IssuerName",
                audience: "Audience",
                claims: testClaims,
                notBefore: DateTime.Now.AddHours(-1),
                expires: DateTime.Now.AddHours(1),
                signingCredentials: signingCredentials);

            string certHash = "Test Cert Hash";

            token.Header[JwtHeaderParameterNames.X5t] = certHash;
            token.Header[JwtHeaderParameterNames.Kid] = certHash;

            var handler = new JwtSecurityTokenHandler();
            _ = handler.WriteToken(token);

            // Dispose() should stop the event queue task if it is running.
            cache.Dispose();

            AssertNoHangingingTasks(cache, "ProviderCache_EnsureNoLeakingTasks_SecurityTokenHandler_SigningCredentials");
        }

        private void AssertNoHangingingTasks(InMemoryCryptoProviderCache cache, string callName)
        {
            WaitTillTaskComplete(cache, MaxEventQueueTaskWaitTimeInSeconds); // wait for the event queue task to complete
            Assert.True(cache.TaskCount == 0, $"{callName}: unexpected task count: {cache.TaskCount}, expected: 0");
        }

        /// <summary>
        /// The max wait time (in seconds) for the event queue task to exit.
        /// </summary>
        private int MaxEventQueueTaskWaitTimeInSeconds => 5;

        /// <summary>
        /// Helper method to wait for the event queue tasks to start, up to the specified time in seconds.
        /// </summary>
        /// <param name="cache">the cache to check</param>
        /// <param name="secondsTimeout">the timeout in seconds</param>
        private void WaitTillTasksStarted(InMemoryCryptoProviderCache cache, long secondsTimeout)
        {
            int i = 0;
            for (; i < secondsTimeout; i++)
            {
                if (cache.TaskCount > 0)
                    break;

                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Helper method to wait for tasks to complete, up to the specified time in seconds.
        /// </summary>
        /// <param name="cache">the cache to check</param>
        /// <param name="secondsTimeout">the timeout in seconds</param>
        private void WaitTillTaskComplete(InMemoryCryptoProviderCache cache, long secondsTimeout)
        {
            int i = 0;
            for (; i < secondsTimeout; i++)
            {
                if (cache.TaskCount == 0)
                    break;

                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Thread proc that creates and removes providers.
        /// </summary>
        /// <param name="obj">func creating providers (signing and verifying)</param>
        private static void ThreadStartProcAddAndRemoveProviders(CryptoProviderFactory factory, CreateProvidersFunc func)
        {
            var cache = factory.CryptoProviderCache as InMemoryCryptoProviderCache;

            // create signing providers
            var providers = func(factory);
            foreach (var provider in providers)
            {
                provider.AddRef();
                Thread.Sleep(100);
                provider.Release();
            }

            Thread.Sleep(500);
            foreach (var provider in providers)
                cache.TryRemove(provider);
        }

        public delegate IList<SignatureProvider> CreateProvidersFunc(CryptoProviderFactory factory);

        /// <summary>
        /// Helper method to create some signing providers.
        /// </summary>
        /// <param name="factory"><see cref="CryptoProviderFactory"/>the factory to create providers</param>
        /// <returns>a list of signing providers</returns>
        private static IList<SignatureProvider> CreateSigningProviders(CryptoProviderFactory factory)
        {
            var providers = new List<SignatureProvider>();

            providers.Add(factory.CreateForSigning(Default.AsymmetricSigningKey, Default.AsymmetricSigningAlgorithm));
            providers.Add(factory.CreateForSigning(Default.SymmetricSigningKey, SecurityAlgorithms.HmacSha256Signature));
            providers.Add(factory.CreateForSigning(Default.SymmetricSigningKey512, ALG.HmacSha512));
            providers.Add(factory.CreateForSigning(Default.SymmetricSigningKey384, ALG.HmacSha384));

            return providers;
        }

        /// <summary>
        /// Helper method to create some verifying providers.
        /// </summary>
        /// <param name="factory"><see cref="CryptoProviderFactory"/>the factory to create providers</param>
        /// <returns>a list of verifying providers</returns>
        private static IList<SignatureProvider> CreateVerifyingProviders(CryptoProviderFactory factory)
        {
            var providers = new List<SignatureProvider>();

            providers.Add(factory.CreateForVerifying(Default.AsymmetricSigningKey, Default.AsymmetricSigningAlgorithm));
            providers.Add(factory.CreateForVerifying(Default.SymmetricSigningKey, SecurityAlgorithms.HmacSha256Signature));
            providers.Add(factory.CreateForVerifying(Default.SymmetricSigningKey512, ALG.HmacSha512));
            providers.Add(factory.CreateForVerifying(Default.SymmetricSigningKey384, ALG.HmacSha384));

            return providers;
        }

        private static bool GetSignatureProviderIsDisposedByReflect(SignatureProvider signatureProvider) =>
            (bool)signatureProvider.GetType().GetField("_disposed", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(signatureProvider);
    }
}
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
