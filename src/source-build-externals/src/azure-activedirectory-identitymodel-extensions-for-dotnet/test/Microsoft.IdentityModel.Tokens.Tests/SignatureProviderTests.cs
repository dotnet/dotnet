// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Azure.KeyVault.Cryptography;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

using ALG = Microsoft.IdentityModel.Tokens.SecurityAlgorithms;
using EE = Microsoft.IdentityModel.TestUtils.ExpectedException;
using KEY = Microsoft.IdentityModel.TestUtils.KeyingMaterial;

namespace Microsoft.IdentityModel.Tokens.Tests
{
    /// <summary>
    /// This class tests:
    /// CryptoProviderFactory
    /// SignatureProvider
    /// SymmetricSignatureProvider
    /// AsymmetricSignatureProvider
    /// </summary>
    public class SignatureProviderTests
    {
        [Theory, MemberData(nameof(SignatureProviderConstructorParamsTheoryData))]
        public void CryptoProviderFactoryConstructorParams(CryptoProviderFactoryTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CryptoProviderFactoryConstructorParams", theoryData);

            try
            {
                if (theoryData.WillCreateSignatures)
                    theoryData.CryptoProviderFactory.CreateForSigning(theoryData.SigningKey, theoryData.SigningAlgorithm);
                else
                    theoryData.CryptoProviderFactory.CreateForVerifying(theoryData.SigningKey, theoryData.SigningAlgorithm);

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory, MemberData(nameof(SignatureProviderConstructorParamsTheoryData))]
        public void AsymmetricSignatureProviderConstructorParams(SignatureProviderTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.AsymmetricSignatureProviderConstructorParams", theoryData);

            try
            {
                new AsymmetricSignatureProvider(theoryData.SigningKey, theoryData.SigningAlgorithm, theoryData.WillCreateSignatures);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory, MemberData(nameof(SignatureProviderConstructorParamsTheoryData))]
        public void SymmetricSignatureProviderConstructorParams(SignatureProviderTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.SymmetricSignatureProviderConstructorParams", theoryData);

            try
            {
                new SymmetricSignatureProvider(theoryData.SigningKey, theoryData.SigningAlgorithm);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SignatureProviderTheoryData> SignatureProviderConstructorParamsTheoryData
        {
            get => new TheoryData<SignatureProviderTheoryData>
            {
                new SignatureProviderTheoryData
                {
                    SigningAlgorithm = string.Empty,
                    ExpectedException = EE.ArgumentNullException(),
                    First = true,
                    SigningKey = KEY.X509SecurityKey_1024,
                    TestId = "AlgorithmString.Empty",
                    WillCreateSignatures = true
                },
                new SignatureProviderTheoryData
                {
                    SigningAlgorithm = null,
                    ExpectedException = EE.ArgumentNullException(),
                    SigningKey = KEY.X509SecurityKey_1024,
                    TestId = "AlgorithmNULL"
                },
                new SignatureProviderTheoryData
                {
                    SigningAlgorithm = ALG.RsaSha256,
                    ExpectedException = EE.ArgumentNullException(),
                    SigningKey = null,
                    TestId = "SigningKeyNULL",
                },
            };
        }

        /// <summary>
        /// Tests Asymmetric SecurityKeys
        /// </summary>
        /// <param name="theoryData"></param>
        [Theory, MemberData(nameof(AsymmetricSignAndVerifyTheoryData))]
        public void AsymmetricSignAndVerify(SignatureProviderTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.AsymmetricSignAndVerify", theoryData);
            try
            {
                theoryData.VerifyKey.CryptoProviderFactory = theoryData.CryptoProviderFactory;
                var signatureProviderVerify = theoryData.CryptoProviderFactory.CreateForVerifying(theoryData.VerifyKey, theoryData.VerifyAlgorithm);
                var signatureProviderSign = theoryData.CryptoProviderFactory.CreateForSigning(theoryData.SigningKey, theoryData.SigningAlgorithm);
                var bytes = Encoding.UTF8.GetBytes("GenerateASignature");
                var signature = signatureProviderSign.Sign(bytes);
                var isValid = signatureProviderVerify.Verify(bytes, signature);
                if (isValid != theoryData.IsValid)
                    context.AddDiff($"isValid != theoryData.IsValid. '{isValid}', '{theoryData.IsValid}'.");

                if (signatureProviderVerify.ObjectPoolSize != theoryData.CryptoProviderFactory.SignatureProviderObjectPoolCacheSize)
                    context.AddDiff($"signatureProviderVerify.ObjectPoolSize != theoryData.CryptoProviderFactory.SignatureProviderObjectPoolCacheSize. '{signatureProviderVerify.ObjectPoolSize}, {theoryData.CryptoProviderFactory.SignatureProviderObjectPoolCacheSize}'.");

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SignatureProviderTheoryData> AsymmetricSignAndVerifyTheoryData
        {
            get => new TheoryData<SignatureProviderTheoryData>
            {
                new SignatureProviderTheoryData("ECDsa1", ALG.EcdsaSha256, ALG.EcdsaSha256, KEY.Ecdsa256Key, KEY.Ecdsa256Key_Public),
                new SignatureProviderTheoryData("ECDsa2", ALG.EcdsaSha384, ALG.EcdsaSha384, KEY.Ecdsa256Key, KEY.Ecdsa256Key_Public),
                new SignatureProviderTheoryData("ECDsa3", ALG.EcdsaSha512, ALG.EcdsaSha512, KEY.Ecdsa256Key, KEY.Ecdsa256Key_Public),
                new SignatureProviderTheoryData("ECDsa4", ALG.EcdsaSha256Signature, ALG.EcdsaSha256Signature, KEY.Ecdsa256Key, KEY.Ecdsa256Key_Public),
                new SignatureProviderTheoryData("ECDsa5", ALG.EcdsaSha384Signature, ALG.EcdsaSha384Signature, KEY.Ecdsa256Key, KEY.Ecdsa256Key_Public),
                new SignatureProviderTheoryData("ECDsa6", ALG.EcdsaSha512Signature, ALG.EcdsaSha512Signature, KEY.Ecdsa256Key, KEY.Ecdsa256Key_Public),
                new SignatureProviderTheoryData("ECDsa7", ALG.Aes128Encryption, ALG.EcdsaSha256Signature, KEY.Ecdsa256Key, KEY.Ecdsa256Key_Public, EE.NotSupportedException("IDX10634:")),
                new SignatureProviderTheoryData("ECDsa8", ALG.EcdsaSha384, ALG.EcdsaSha384, KEY.Ecdsa384Key, KEY.Ecdsa384Key_Public),
                new SignatureProviderTheoryData("ECDsa9", ALG.EcdsaSha512, ALG.EcdsaSha512, KEY.Ecdsa521Key, KEY.Ecdsa521Key_Public),

                // JsonWebKey
                new SignatureProviderTheoryData("JsonWebKeyEcdsa1", ALG.EcdsaSha256, ALG.EcdsaSha256, KEY.JsonWebKeyP256, KEY.JsonWebKeyP256_Public){ CryptoProviderFactory = new CryptoProviderFactory(CryptoProviderCacheTests.CreateCacheForTesting()){ CacheSignatureProviders = false, SignatureProviderObjectPoolCacheSize = 10 } },
                new SignatureProviderTheoryData("JsonWebKeyEcdsa2", ALG.EcdsaSha256Signature, ALG.EcdsaSha256Signature, KEY.JsonWebKeyP256, KEY.JsonWebKeyP256_Public),
                new SignatureProviderTheoryData("JsonWebKeyEcdsa3", ALG.Aes256KeyWrap, ALG.EcdsaSha256Signature, KEY.JsonWebKeyP256, KEY.JsonWebKeyP256_Public, EE.NotSupportedException("IDX10634:")),
                new SignatureProviderTheoryData("JsonWebKeyEcdsa4", ALG.EcdsaSha256, ALG.EcdsaSha256, KEY.JsonWebKeyP256, KEY.JsonWebKeyP256_Public),
                new SignatureProviderTheoryData("JsonWebKeyEcdsa5", ALG.EcdsaSha384, ALG.EcdsaSha384, KEY.JsonWebKeyP384, KEY.JsonWebKeyP384_Public),
                new SignatureProviderTheoryData("JsonWebKeyEcdsa6", ALG.EcdsaSha512, ALG.EcdsaSha512, KEY.JsonWebKeyP521, KEY.JsonWebKeyP521_Public),
                new SignatureProviderTheoryData("JsonWebKeyP256_Invalid_D", ALG.EcdsaSha256, ALG.EcdsaSha256, KEY.JsonWebKeyP256_Invalid_D, KEY.JsonWebKeyP256_Public, EE.CryptographicException(ignoreInnerException: true)),
                new SignatureProviderTheoryData("JsonWebKeyRsa1", ALG.RsaSha256, ALG.RsaSha256, KEY.JsonWebKeyRsa_2048, KEY.JsonWebKeyRsa_2048_Public),
                new SignatureProviderTheoryData("JsonWebKeyRsa2", ALG.RsaSha256Signature, ALG.RsaSha256Signature, KEY.JsonWebKeyRsa_2048, KEY.JsonWebKeyRsa_2048_Public),
                new SignatureProviderTheoryData("JsonWebKeyRsa3", ALG.Aes192KeyWrap, ALG.RsaSha256Signature, KEY.JsonWebKeyRsa_2048, KEY.JsonWebKeyRsa_2048_Public, EE.NotSupportedException("IDX10634:")),

                new SignatureProviderTheoryData("RsaSecurityKey1", ALG.RsaSha256, ALG.RsaSha256, KEY.RsaSecurityKey_2048, KEY.RsaSecurityKey_2048_Public){ CryptoProviderFactory = new CryptoProviderFactory(CryptoProviderCacheTests.CreateCacheForTesting()){ CacheSignatureProviders = false, SignatureProviderObjectPoolCacheSize = 100 } },
                new SignatureProviderTheoryData("RsaSecurityKey2", ALG.RsaSha256Signature, ALG.RsaSha256Signature, KEY.RsaSecurityKey_2048, KEY.RsaSecurityKey_2048_Public),
                new SignatureProviderTheoryData("RsaSecurityKey3", ALG.RsaSha384, ALG.RsaSha384, KEY.RsaSecurityKey_2048, KEY.RsaSecurityKey_2048_Public),
                new SignatureProviderTheoryData("RsaSecurityKey4", ALG.RsaSha384Signature, ALG.RsaSha384Signature, KEY.RsaSecurityKey_2048, KEY.RsaSecurityKey_2048_Public),
                new SignatureProviderTheoryData("RsaSecurityKey5", ALG.RsaSha512, ALG.RsaSha512, KEY.RsaSecurityKey_2048, KEY.RsaSecurityKey_2048_Public),
                new SignatureProviderTheoryData("RsaSecurityKey6", ALG.RsaSha512Signature, ALG.RsaSha512Signature, KEY.RsaSecurityKey_2048, KEY.RsaSecurityKey_2048_Public),
                new SignatureProviderTheoryData("RsaSecurityKey7", ALG.Aes128Encryption, ALG.RsaSha512, KEY.RsaSecurityKey_2048, KEY.RsaSecurityKey_2048_Public, EE.NotSupportedException("IDX10634:")),
                new SignatureProviderTheoryData("RsaSecurityKey8", ALG.RsaSha256Signature, ALG.RsaSha256Signature, KEY.RsaSecurityKey_4096, KEY.RsaSecurityKey_4096_Public),
                new SignatureProviderTheoryData("RsaSecurityKey9", ALG.RsaSha384Signature, ALG.RsaSha384Signature, KEY.RsaSecurityKey_4096, KEY.RsaSecurityKey_4096_Public),
                new SignatureProviderTheoryData("RsaSecurityKey10", ALG.RsaSha512Signature, ALG.RsaSha512Signature, KEY.RsaSecurityKey_4096, KEY.RsaSecurityKey_4096_Public),

                new SignatureProviderTheoryData("X509SecurityKey1", ALG.RsaSha256, ALG.RsaSha256, KEY.X509SecurityKeySelfSigned2048_SHA256, KEY.X509SecurityKeySelfSigned2048_SHA256_Public),
                new SignatureProviderTheoryData("X509SecurityKey2", ALG.RsaSha256Signature, ALG.RsaSha256Signature, KEY.X509SecurityKeySelfSigned2048_SHA256, KEY.X509SecurityKeySelfSigned2048_SHA256_Public),
                new SignatureProviderTheoryData("X509SecurityKey3", ALG.RsaSha384, ALG.RsaSha384, KEY.X509SecurityKeySelfSigned2048_SHA256, KEY.X509SecurityKeySelfSigned2048_SHA256_Public),
                new SignatureProviderTheoryData("X509SecurityKey4", ALG.RsaSha384Signature, ALG.RsaSha384Signature, KEY.X509SecurityKeySelfSigned2048_SHA256, KEY.X509SecurityKeySelfSigned2048_SHA256_Public),
                new SignatureProviderTheoryData("X509SecurityKey5", ALG.RsaSha512, ALG.RsaSha512, KEY.X509SecurityKeySelfSigned2048_SHA256, KEY.X509SecurityKeySelfSigned2048_SHA256_Public),
                new SignatureProviderTheoryData("X509SecurityKey6", ALG.RsaSha512Signature, ALG.RsaSha512Signature, KEY.X509SecurityKeySelfSigned2048_SHA256, KEY.X509SecurityKeySelfSigned2048_SHA256_Public),
                new SignatureProviderTheoryData("X509SecurityKey7", ALG.Aes128Encryption, ALG.RsaSha512Signature, KEY.X509SecurityKeySelfSigned2048_SHA256, KEY.X509SecurityKeySelfSigned2048_SHA256_Public, EE.NotSupportedException("IDX10634:")),
                new SignatureProviderTheoryData("X509SecurityKey8", ALG.RsaSha256Signature, ALG.RsaSha512Signature, KEY.DefaultX509Key_2048, KEY.DefaultX509Key_2048_Public, null, false),
                new SignatureProviderTheoryData("UnknownKeyType1", ALG.RsaSha256Signature, ALG.RsaSha256Signature, KEY.RsaSecurityKey_2048, NotAsymmetricOrSymmetricSecurityKey.New, EE.NotSupportedException("IDX10621:")),
                new SignatureProviderTheoryData("UnKnownKeyType2", ALG.RsaSha256Signature, ALG.RsaSha256Signature, NotAsymmetricOrSymmetricSecurityKey.New, KEY.RsaSecurityKey_2048, EE.NotSupportedException("IDX10621:")),

                // Private keys missing
                new SignatureProviderTheoryData("PrivateKeyMissing1", ALG.EcdsaSha256, ALG.EcdsaSha256, KEY.JsonWebKeyP256_Public, KEY.JsonWebKeyP256_Public, EE.InvalidOperationException("IDX10638:")),
                new SignatureProviderTheoryData("PrivateKeyMissing2", ALG.RsaSha256, ALG.RsaSha256, KEY.JsonWebKeyRsa_2048_Public, KEY.JsonWebKeyRsa_2048_Public, EE.InvalidOperationException("IDX10638:")),
                new SignatureProviderTheoryData("PrivateKeyMissing3", ALG.RsaSha256Signature, ALG.RsaSha256Signature, KEY.RsaSecurityKey_2048_Public,KEY.RsaSecurityKey_2048_Public, EE.InvalidOperationException("IDX10638:")),
                new SignatureProviderTheoryData("PrivateKeyMissing4", ALG.RsaSha256, ALG.RsaSha256, KEY.X509SecurityKeySelfSigned2048_SHA256_Public, KEY.X509SecurityKeySelfSigned2048_SHA256_Public, EE.InvalidOperationException("IDX10638:")),

                // .Net Core throws some funky inner exception that GetType() reports as: Internal.Cryptography.CryptoThrowHelper+WindowsCryptographicException
                new SignatureProviderTheoryData("PrivateKeyMissing5", ALG.EcdsaSha512, ALG.EcdsaSha512, KEY.Ecdsa521Key_Public, KEY.Ecdsa521Key_Public, new EE(typeof(Exception)){IgnoreExceptionType = true}),

                // Invalid JsonWebKeyComponents
                new SignatureProviderTheoryData("JsonWebKeyP521_Public_Invalid_X", ALG.EcdsaSha512, ALG.EcdsaSha512, KEY.JsonWebKeyP521_Public_Invalid_X, KEY.JsonWebKeyP521, EE.InvalidOperationException()),
                new SignatureProviderTheoryData("JsonWebKeyP521_Public_Invalid_Y", ALG.EcdsaSha512, ALG.EcdsaSha512, KEY.JsonWebKeyP521_Public_Invalid_Y, KEY.JsonWebKeyP521, EE.InvalidOperationException()),
                new SignatureProviderTheoryData("JsonWebKeyP521_Invalid_D", ALG.EcdsaSha512, ALG.EcdsaSha512, KEY.JsonWebKeyP521_Invalid_D, KEY.JsonWebKeyP521_Public, EE.CryptographicException()),
            };
        }

        [Theory, MemberData(nameof(SymmetricSignAndVerifyTheoryData))]
        public void SymmetricSignAndVerify(SignatureProviderTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.SignAndVerify", theoryData);
            try
            {
                theoryData.VerifyKey.CryptoProviderFactory = theoryData.CryptoProviderFactory;
                var signatureProviderVerify = theoryData.CryptoProviderFactory.CreateForVerifying(theoryData.VerifyKey, theoryData.VerifyAlgorithm);
                var signatureProviderSign = theoryData.CryptoProviderFactory.CreateForSigning(theoryData.SigningKey, theoryData.SigningAlgorithm);
                var bytes = Encoding.UTF8.GetBytes("GenerateASignature");
                var signature = signatureProviderSign.Sign(bytes);
                if (!signatureProviderVerify.Verify(bytes, signature))
                    throw new SecurityTokenInvalidSignatureException("SignatureFailed");

                if (signatureProviderVerify.ObjectPoolSize != theoryData.CryptoProviderFactory.SignatureProviderObjectPoolCacheSize)
                    context.AddDiff($"signatureProviderVerify.ObjectPoolSize != theoryData.CryptoProviderFactory.SignatureProviderObjectPoolCacheSize. '{signatureProviderVerify.ObjectPoolSize}, {theoryData.CryptoProviderFactory.SignatureProviderObjectPoolCacheSize}'.");

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SignatureProviderTheoryData> SymmetricSignAndVerifyTheoryData
        {
            get => new TheoryData<SignatureProviderTheoryData>
            {
                // JsonWebKey
                new SignatureProviderTheoryData("JsonWebKeySymmetric1", ALG.HmacSha256, ALG.HmacSha256, KEY.JsonWebKeySymmetric256, KEY.JsonWebKeySymmetric256){ CryptoProviderFactory = new CryptoProviderFactory(CryptoProviderCacheTests.CreateCacheForTesting()){ CacheSignatureProviders = false, SignatureProviderObjectPoolCacheSize = 10 } },
                new SignatureProviderTheoryData("JsonWebKeySymmetric2", ALG.HmacSha256Signature, ALG.HmacSha256Signature, KEY.JsonWebKeySymmetric256, KEY.JsonWebKeySymmetric256),
                new SignatureProviderTheoryData("JsonWebKeySymmetric3", ALG.RsaSha256Signature, ALG.RsaSha256Signature, KEY.JsonWebKeySymmetric256, KEY.JsonWebKeyRsa_2048_Public, EE.NotSupportedException("IDX10634:")),
                new SignatureProviderTheoryData("JsonWebKeySymmetric4", ALG.EcdsaSha512Signature, ALG.EcdsaSha512Signature, KEY.JsonWebKeySymmetric256, KEY.JsonWebKeyRsa_2048_Public, EE.NotSupportedException("IDX10634:")),

                new SignatureProviderTheoryData("SymmetricSecurityKey1", ALG.HmacSha256, ALG.HmacSha256, KEY.SymmetricSecurityKey2_256, KEY.SymmetricSecurityKey2_256){ CryptoProviderFactory = new CryptoProviderFactory(CryptoProviderCacheTests.CreateCacheForTesting()){ CacheSignatureProviders = false, SignatureProviderObjectPoolCacheSize = 42 } },
                new SignatureProviderTheoryData("SymmetricSecurityKey2", ALG.HmacSha256, ALG.HmacSha256, Default.SymmetricSigningKey256,  Default.SymmetricSigningKey256),
                
                // HmacSha256 <-> HmacSha256Signature
                new SignatureProviderTheoryData("SymmetricSecurityKey3", ALG.HmacSha256Signature, ALG.HmacSha256, Default.SymmetricSigningKey256,  Default.SymmetricSigningKey256),
                new SignatureProviderTheoryData("SymmetricSecurityKey4", ALG.HmacSha256, ALG.HmacSha256Signature, Default.SymmetricSigningKey256,  Default.SymmetricSigningKey256),

                // HmacSha384 <-> HmacSha384Signature
                new SignatureProviderTheoryData("SymmetricSecurityKey5", ALG.HmacSha384, ALG.HmacSha384Signature, Default.SymmetricSigningKey384,  Default.SymmetricSigningKey384),
                new SignatureProviderTheoryData("SymmetricSecurityKey6", ALG.HmacSha384Signature, ALG.HmacSha384, Default.SymmetricSigningKey384,  Default.SymmetricSigningKey384),
                
                // HmacSha512 <-> HmacSha512Signature
                new SignatureProviderTheoryData("SymmetricSecurityKey7", ALG.HmacSha512, ALG.HmacSha512Signature, Default.SymmetricSigningKey512,  Default.SymmetricSigningKey512),
                new SignatureProviderTheoryData("SymmetricSecurityKey8", ALG.HmacSha512Signature, ALG.HmacSha512, Default.SymmetricSigningKey512,  Default.SymmetricSigningKey512),

                new SignatureProviderTheoryData("SymmetricSecurityKey9", ALG.HmacSha256Signature, ALG.HmacSha256Signature, KEY.SymmetricSecurityKey2_256, KEY.SymmetricSecurityKey2_256),
                new SignatureProviderTheoryData("SymmetricSecurityKey10", ALG.RsaSha256Signature, ALG.RsaSha512Signature, KEY.SymmetricSecurityKey2_256, KEY.SymmetricSecurityKey2_256, EE.NotSupportedException("IDX10634:")),
                new SignatureProviderTheoryData("SymmetricSecurityKey11", ALG.HmacSha256Signature, ALG.HmacSha256Signature, KEY.DefaultSymmetricSecurityKey_256, KEY.DefaultSymmetricSecurityKey_256),
                new SignatureProviderTheoryData("SymmetricSecurityKey12",
                                                ALG.HmacSha256Signature, 
                                                ALG.HmacSha256Signature,
                                                new FaultingSymmetricSecurityKey(Default.SymmetricSigningKey256, new CryptographicException("Inner CryptographicException"), null, null, Default.SymmetricSigningKey256.Key),
                                                KEY.SymmetricSecurityKey2_256,
                                                EE.CryptographicException("Inner CryptographicException")),

                new SignatureProviderTheoryData("SymmetricSecurityKey13",
                                                ALG.HmacSha256Signature,
                                                ALG.HmacSha256Signature,
                                                KEY.SymmetricSecurityKey2_256,
                                                new FaultingSymmetricSecurityKey(Default.SymmetricSigningKey256, new CryptographicException("Inner CryptographicException"), null, null, Default.SymmetricSigningKey256.Key),
                                                EE.CryptographicException("Inner CryptographicException")),

                new SignatureProviderTheoryData("UnknownKeyType1", ALG.HmacSha256Signature, ALG.HmacSha256Signature, NotAsymmetricOrSymmetricSecurityKey.New, KEY.SymmetricSecurityKey2_256, EE.NotSupportedException("IDX10621:")),
                new SignatureProviderTheoryData("UnknownKeyType2", ALG.HmacSha256Signature, ALG.HmacSha256Signature, KEY.SymmetricSecurityKey2_256, NotAsymmetricOrSymmetricSecurityKey.New, EE.NotSupportedException("IDX10621:")),

                // Key size checks
                new SignatureProviderTheoryData("KeySize1", ALG.HmacSha256Signature, ALG.HmacSha256Signature, KEY.DefaultSymmetricSecurityKey_56, KEY.DefaultSymmetricSecurityKey_56, EE.ArgumentOutOfRangeException("IDX10653:")),
                new SignatureProviderTheoryData("KeySize2", ALG.HmacSha256Signature, ALG.HmacSha256Signature, Default.SymmetricSigningKey56, Default.SymmetricSigningKey56, EE.ArgumentOutOfRangeException("IDX10653:")),
                new SignatureProviderTheoryData("KeySize3", ALG.HmacSha256Signature, ALG.HmacSha256Signature, Default.SymmetricSigningKey64, Default.SymmetricSigningKey64, EE.ArgumentOutOfRangeException("IDX10653:")),

                // signing and verifying with different keys
                new SignatureProviderTheoryData("DifferentKey1", ALG.HmacSha256, ALG.HmacSha256, Default.SymmetricSigningKey256, NotDefault.SymmetricSigningKey256, EE.SecurityTokenInvalidSignatureException()),
                new SignatureProviderTheoryData("DifferentKey2", ALG.HmacSha256, ALG.HmacSha256, Default.SymmetricSigningKey384, NotDefault.SymmetricSigningKey256, EE.SecurityTokenInvalidSignatureException()),
                new SignatureProviderTheoryData("DifferentKey3", ALG.HmacSha384, ALG.HmacSha384, Default.SymmetricSigningKey384, NotDefault.SymmetricSigningKey384, EE.SecurityTokenInvalidSignatureException()),
                new SignatureProviderTheoryData("DifferentKey4", ALG.HmacSha512, ALG.HmacSha512, Default.SymmetricSigningKey512, NotDefault.SymmetricSigningKey512, EE.SecurityTokenInvalidSignatureException()),
                new SignatureProviderTheoryData("DifferentKey5", ALG.HmacSha512, ALG.HmacSha512, Default.SymmetricSigningKey1024, NotDefault.SymmetricSigningKey1024, EE.SecurityTokenInvalidSignatureException()),
                new SignatureProviderTheoryData("DifferentKey6", ALG.HmacSha256, ALG.HmacSha256, KEY.JsonWebKeySymmetric256, KEY.JsonWebKeySymmetric256_2, EE.SecurityTokenInvalidSignatureException()),
                new SignatureProviderTheoryData("DifferentKey7", ALG.HmacSha384, ALG.HmacSha384, Default.SymmetricSigningKey384, NotDefault.SymmetricSigningKey384, EE.SecurityTokenInvalidSignatureException()),

                // KeyAlgorithmMismatch
                new SignatureProviderTheoryData("KeyAlgorithmMismatch1", ALG.HmacSha256, ALG.HmacSha384, KEY.JsonWebKeyP256, KEY.JsonWebKeyP256_Public, EE.NotSupportedException()),
                new SignatureProviderTheoryData("KeyAlgorithmMismatch2", ALG.HmacSha256, ALG.HmacSha512, KEY.JsonWebKeyP256, KEY.JsonWebKeyP256_Public, EE.NotSupportedException()),
                new SignatureProviderTheoryData("KeyAlgorithmMismatch3", ALG.HmacSha384, ALG.HmacSha512, KEY.JsonWebKeyP256, KEY.JsonWebKeyP256_Public, EE.NotSupportedException()),

                // NotSupported
                // TODO - add scenarios

                // BadKeys
                // Create some bad symmetric keys
            };
        }

        [Fact]
        public void SignatureProvider_Dispose()
        {
            AsymmetricSignatureProvider asymmetricSignatureProvider = new AsymmetricSignatureProvider(KEY.DefaultX509Key_2048_Public, ALG.RsaSha256Signature);
            asymmetricSignatureProvider.Dispose();

            var expectedException = EE.ObjectDisposedException;
            SignatureProvider_DisposeVariation("Sign", asymmetricSignatureProvider, expectedException);
            SignatureProvider_DisposeVariation("Verify", asymmetricSignatureProvider, expectedException);
            SignatureProvider_DisposeVariation("Dispose", asymmetricSignatureProvider, EE.NoExceptionExpected);

            SymmetricSignatureProvider symmetricProvider = new SymmetricSignatureProvider(KEY.DefaultSymmetricSecurityKey_256, KEY.DefaultSymmetricSigningCreds_256_Sha2.Algorithm);
            symmetricProvider.Dispose();
            SignatureProvider_DisposeVariation("Sign", symmetricProvider, expectedException);
            SignatureProvider_DisposeVariation("Verify", symmetricProvider, expectedException);
            SignatureProvider_DisposeVariation("Dispose", symmetricProvider, EE.NoExceptionExpected);
        }

        private void SignatureProvider_DisposeVariation(string testCase, SignatureProvider provider, EE expectedException)
        {
            try
            {
                if (testCase.StartsWith("Sign"))
                    provider.Sign(new byte[256]);
                else if (testCase.StartsWith("Verify"))
                    provider.Verify(new byte[256], new byte[256]);
                else if (testCase.StartsWith("Dispose"))
                    provider.Dispose();
                else
                    Assert.True(false, "Test case does not match any scenario");

                expectedException.ProcessNoException();
            }
            catch(Exception ex)
            {
                expectedException.ProcessException(ex);
            }
        }

        [Fact]
        public void AsymmetricSignatureProvider_SupportedAlgorithms()
        {
            var errors = new List<string>();

            foreach (var algorithm in
                new string[] {
                    ALG.RsaSha256,
                    ALG.RsaSha384,
                    ALG.RsaSha512,
                    ALG.RsaSha256Signature,
                    ALG.RsaSha384Signature,
                    ALG.RsaSha512Signature })
            {
                try
                {
                    var provider = new AsymmetricSignatureProvider(KEY.DefaultX509Key_2048, algorithm);
                }
                catch (Exception ex)
                {
                    errors.Add("Creation of AsymmetricSignatureProvider with algorithm: " + algorithm + ", threw: " + ex.Message);
                }

            }

            foreach (var algorithm in
                new string[] {
                    ALG.EcdsaSha256,
                    ALG.EcdsaSha384,
                    ALG.EcdsaSha512 })
            {
                try
                {
                    SecurityKey key = null;
                    if (algorithm.Equals(ALG.EcdsaSha256))
                    {
                        key = KEY.Ecdsa256Key;
                    }
                    else if (algorithm.Equals(ALG.EcdsaSha384))
                    {
                        key = KEY.Ecdsa384Key;
                    }
                    else
                    {
                        key = KEY.Ecdsa521Key;
                    }

                    var provider = new AsymmetricSignatureProvider(key, algorithm);
                }
                catch (Exception ex)
                {
                    errors.Add("Creation of AsymmetricSignatureProvider with algorithm: " + algorithm + ", threw: " + ex.Message);
                }

            }
            TestUtilities.AssertFailIfErrors("AsymmetricSignatureProvider_SupportedAlgorithms", errors);

        }

        [Theory, MemberData(nameof(AsymmetricSignatureProviderVerifyParameterChecksTheoryData))]
        public void AsymmetricSignatureProviderVerifyParameterChecks(SignatureProviderTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.AsymmetricSignatureProviderVerifyParameterChecks", theoryData);
            try
            {
                theoryData.VerifySignatureProvider.Verify(theoryData.RawBytes, theoryData.Signature);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SignatureProviderTheoryData> AsymmetricSignatureProviderVerifyParameterChecksTheoryData
        {
            get
            {
                var signatureProvider = new AsymmetricSignatureProvider(KEY.RsaSecurityKey_2048, ALG.RsaSha256);
                return new TheoryData<SignatureProviderTheoryData>
                {
                    new SignatureProviderTheoryData
                    {
                        ExpectedException = EE.ArgumentNullException(),
                        RawBytes = null,
                        Signature = new byte[1],
                        VerifySignatureProvider = signatureProvider,
                        TestId = "RawBytes-NULL"
                    },
                    new SignatureProviderTheoryData
                    {
                        ExpectedException = EE.ArgumentNullException(),
                        RawBytes = new byte[1],
                        Signature = null,
                        VerifySignatureProvider = signatureProvider,
                        TestId = "Signature-NULL"
                    },
                    new SignatureProviderTheoryData
                    {
                        ExpectedException = EE.ArgumentNullException(),
                        RawBytes = new byte[0],
                        Signature = new byte[1],
                        VerifySignatureProvider = signatureProvider,
                        TestId = "RawBytes-Size:0"
                    },
                    new SignatureProviderTheoryData
                    {
                        ExpectedException = EE.ArgumentNullException(),
                        RawBytes = new byte[1],
                        Signature = new byte[0],
                        VerifySignatureProvider = signatureProvider,
                        TestId = "Signature-Size:0"
                    }
                };
            }
        }

        [Theory, MemberData(nameof(SymmetricSignatureProviderVerifyParameterChecksTheoryData))]
        public void SymmetricSignatureProviderVerifyParameterChecks(SignatureProviderTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.SymmetricSignatureProviderVerifyParameterChecks", theoryData);
            try
            {
                theoryData.SigningSignatureProvider.Verify(theoryData.RawBytes, theoryData.Signature);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SignatureProviderTheoryData> SymmetricSignatureProviderVerifyParameterChecksTheoryData
        {
            get
            {
                var signatureProvider = new SymmetricSignatureProvider(KEY.SymmetricSecurityKey2_256, ALG.HmacSha256);
                return new TheoryData<SignatureProviderTheoryData>
                {
                    new SignatureProviderTheoryData
                    {
                        ExpectedException = EE.ArgumentNullException(),
                        RawBytes = null,
                        Signature = new byte[1],
                        SigningSignatureProvider = signatureProvider,
                        TestId = "RawBytes-NULL"
                    },
                    new SignatureProviderTheoryData
                    {
                        ExpectedException = EE.ArgumentNullException(),
                        RawBytes = new byte[1],
                        Signature = null,
                        SigningSignatureProvider = signatureProvider,
                        TestId = "Signature-NULL"
                    },
                    new SignatureProviderTheoryData
                    {
                        ExpectedException = EE.ArgumentNullException(),
                        RawBytes = new byte[0],
                        Signature = new byte[1],
                        SigningSignatureProvider = signatureProvider,
                        TestId = "RawBytes-Size:0"
                    },
                    new SignatureProviderTheoryData
                    {
                        ExpectedException = EE.ArgumentNullException(),
                        RawBytes = new byte[1],
                        Signature = new byte[0],
                        SigningSignatureProvider = signatureProvider,
                        TestId = "Signature-Size:0"
                    }
                };
            }
        }

        [Theory, MemberData(nameof(SymmetricVerifySignatureSizeTheoryData))]
        public void SymmetricVerify1Tests(SignatureProviderTheoryData theoryData)
        {
            // verifies: public bool Verify(byte[] input, byte[] signature)
            var context = TestUtilities.WriteHeader($"{this}.SymmetricVerify1Tests", theoryData);
            try
            {
                if (theoryData.SigningSignatureProvider.Verify(theoryData.RawBytes, theoryData.Signature))
                    context.Diffs.Add("SigningSignatureProvider.Verify should not have succeeded");
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory, MemberData(nameof(SymmetricVerifySignatureSizeTheoryData))]
        public void SymmetricVerify2Tests(SignatureProviderTheoryData theoryData)
        {
            // verifies: public bool Verify(byte[] input, byte[] signature, int length)
            var context = TestUtilities.WriteHeader($"{this}.SymmetricVerify2Tests", theoryData);
            try
            {
                ((SymmetricSignatureProvider)theoryData.SigningSignatureProvider).Verify(theoryData.RawBytes, theoryData.Signature, theoryData.Signature.Length);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory, MemberData(nameof(SymmetricVerifySignatureSizeTheoryData))]
        public void SymmetricVerify3Tests(SignatureProviderTheoryData theoryData)
        {
            // verifies: public override bool Verify(byte[] input, int inputOffset, int inputLength, byte[] signature, int signatureOffset, int signatureLength)
            var context = TestUtilities.WriteHeader($"{this}.SymmetricVerify3Tests", theoryData);
            try
            {
                theoryData.SigningSignatureProvider.Verify(theoryData.RawBytes, 0, theoryData.RawBytes.Length, theoryData.Signature, 0, theoryData.Signature.Length);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SignatureProviderTheoryData> SymmetricVerifySignatureSizeTheoryData
        {
            get
            {
                return new TheoryData<SignatureProviderTheoryData>
                {
                    new SignatureProviderTheoryData("HmacSha256")
                    {
                        ExpectedException = EE.ArgumentException("IDX10719:"),
                        RawBytes= new byte[16],
                        Signature = new byte[16],
                        SigningSignatureProvider = new SymmetricSignatureProvider(KEY.SymmetricSecurityKey2_256, ALG.HmacSha256),
                    },
                    new SignatureProviderTheoryData("HmacSha384")
                    {
                        ExpectedException = EE.ArgumentException("IDX10719:"),
                        RawBytes= new byte[32],
                        Signature = new byte[32],
                        SigningSignatureProvider = new SymmetricSignatureProvider(KEY.SymmetricSecurityKey2_384, ALG.HmacSha384),
                    },
                    new SignatureProviderTheoryData("HmacSha512")
                    {
                        ExpectedException = EE.ArgumentException("IDX10719:"),
                        RawBytes= new byte[48],
                        Signature = new byte[48],
                        SigningSignatureProvider = new SymmetricSignatureProvider(KEY.SymmetricSecurityKey2_512, ALG.HmacSha512),
                    }
                };
            }
        }

        [Theory, MemberData(nameof(SymmetricVerifySignatureSizeInternalTheoryData))]
        public void SymmetricVerify4Tests(SignatureProviderTheoryData theoryData)
        {
            // verifies: internal bool Verify(byte[] input, int inputOffset, int inputLength, byte[] signature, int signatureOffset, int signatureLength, string algorithm)
            var context = TestUtilities.WriteHeader($"{this}.SymmetricVerify4Tests", theoryData);
            try
            {
                ((SymmetricSignatureProvider)theoryData.SigningSignatureProvider).Verify(theoryData.RawBytes, 0, theoryData.RawBytes.Length, theoryData.Signature, 0, theoryData.Signature.Length, theoryData.VerifyAlgorithm);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }
        
        public static TheoryData<SignatureProviderTheoryData> SymmetricVerifySignatureSizeInternalTheoryData
        {
            get
            {
                return new TheoryData<SignatureProviderTheoryData>
                {
                    new SignatureProviderTheoryData("UnknownAlgorithm")
                    {
                        ExpectedException = EE.ArgumentException("IDX10718:"),
                        RawBytes= new byte[10],
                        Signature = new byte[10],
                        SigningSignatureProvider = new SymmetricSignatureProvider(KEY.SymmetricSecurityKey2_256, ALG.HmacSha256),
                        VerifyAlgorithm = "ALG.Aes128CbcHmacSha256"
                    },
                    new SignatureProviderTheoryData("HmacSha256_HmacSha384")
                    {
                        ExpectedException = EE.ArgumentException("IDX10719:"),
                        RawBytes= new byte[32],
                        Signature = new byte[32],
                        SigningSignatureProvider = new SymmetricSignatureProvider(KEY.SymmetricSecurityKey2_256, ALG.HmacSha256),
                        VerifyAlgorithm = ALG.HmacSha384
                    },
                    new SignatureProviderTheoryData("HmacSha384_HmacSha256")
                    {
                        ExpectedException = EE.ArgumentException("IDX10719:"),
                        RawBytes= new byte[384],
                        Signature = new byte[384],
                        SigningSignatureProvider = new SymmetricSignatureProvider(KEY.SymmetricSecurityKey2_256, ALG.HmacSha384),
                        VerifyAlgorithm = ALG.HmacSha256
                    },
                    new SignatureProviderTheoryData("HmacSha512_HmacSha384")
                    {
                        ExpectedException = EE.ArgumentException("IDX10719:"),
                        RawBytes= new byte[512],
                        Signature = new byte[512],
                        SigningSignatureProvider = new SymmetricSignatureProvider(KEY.SymmetricSecurityKey2_256, ALG.HmacSha512),
                        VerifyAlgorithm = ALG.HmacSha384
                    }
                };
            }
        }

        [Fact]
        public void SymmetricSignatureProvider_SupportedAlgorithms()
        {
            var errors = new List<string>();

            foreach (var algorithm in
                new string[] {
                    ALG.HmacSha256Signature,
                    ALG.HmacSha384Signature,
                    ALG.HmacSha512Signature,
                    ALG.HmacSha256,
                    ALG.HmacSha384,
                    ALG.HmacSha512 })
            {
                try
                {
                    var provider = new SymmetricSignatureProvider(KEY.DefaultSymmetricSecurityKey_256, algorithm);
                }
                catch (Exception ex)
                {
                    errors.Add("Creation of AsymmetricSignatureProvider with algorithm: " + algorithm + ", threw: " + ex.Message);
                }

                TestUtilities.AssertFailIfErrors("AsymmetricSignatureProvider_SupportedAlgorithms", errors);
            }
        }

        [Theory, MemberData(nameof(SymmetricSecurityKeySizesTheoryData))]
        public void SymmetricSecurityKeySizesSign(SymmetricSignatureProviderTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.SymmetricSecurityKeySizes", theoryData);
            try
            {
                var provider = new SymmetricSignatureProvider(theoryData.SecurityKey, theoryData.Algorithm);
                provider.Sign(new byte[32]);

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory, MemberData(nameof(SymmetricSecurityKeySizesTheoryData))]
        public void SymmetricSecurityKeySizesVerify(SymmetricSignatureProviderTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.SymmetricSecurityKeySizes", theoryData);
            try
            {
                var provider = new SymmetricSignatureProvider(theoryData.SecurityKey, theoryData.Algorithm);
                provider.Verify(new byte[32], new byte[32]);

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SymmetricSignatureProviderTheoryData> SymmetricSecurityKeySizesTheoryData()
        {
            var theoryData = new TheoryData<SymmetricSignatureProviderTheoryData>();

            theoryData.Add(new SymmetricSignatureProviderTheoryData("HmacSha256Signature")
            {
                SecurityKey = new SymmetricSecurityKey(new byte[16]),
                Algorithm = ALG.HmacSha256Signature,
                ExpectedException = EE.ArgumentOutOfRangeException("IDX10720:")
            });

            theoryData.Add(new SymmetricSignatureProviderTheoryData("HmacSha256")
            {
                SecurityKey = new SymmetricSecurityKey(new byte[16]),
                Algorithm = ALG.HmacSha256,
                ExpectedException = EE.ArgumentOutOfRangeException("IDX10720:")
            });

            theoryData.Add(new SymmetricSignatureProviderTheoryData("HmacSha256_32")
            {
                SecurityKey = new SymmetricSecurityKey(new byte[32]),
                Algorithm = ALG.HmacSha256
            });

            theoryData.Add(new SymmetricSignatureProviderTheoryData("HmacSha384Signature")
            {
                SecurityKey = new SymmetricSecurityKey(new byte[32]),
                Algorithm = ALG.HmacSha384Signature,
                ExpectedException = EE.ArgumentOutOfRangeException("IDX10720:")
            });

            theoryData.Add(new SymmetricSignatureProviderTheoryData("HmacSha384")
            {
                SecurityKey = new SymmetricSecurityKey(new byte[32]),
                Algorithm = ALG.HmacSha384,
                ExpectedException = EE.ArgumentOutOfRangeException("IDX10720:")
            });

            theoryData.Add(new SymmetricSignatureProviderTheoryData("HmacSha384_48")
            {
                SecurityKey = new SymmetricSecurityKey(new byte[48]),
                Algorithm = ALG.HmacSha384
            });

            theoryData.Add(new SymmetricSignatureProviderTheoryData("HmacSha512Signature")
            {
                SecurityKey = new SymmetricSecurityKey(new byte[48]),
                Algorithm = ALG.HmacSha512Signature,
                ExpectedException = EE.ArgumentOutOfRangeException("IDX10720:")
            });

            theoryData.Add(new SymmetricSignatureProviderTheoryData("HmacSha512")
            {
                SecurityKey = new SymmetricSecurityKey(new byte[48]),
                Algorithm = ALG.HmacSha512,
                ExpectedException = EE.ArgumentOutOfRangeException("IDX10720:")
            });

            theoryData.Add(new SymmetricSignatureProviderTheoryData("HmacSha512_64")
            {
                SecurityKey = new SymmetricSecurityKey(new byte[64]),
                Algorithm = ALG.HmacSha512
            });

            return theoryData;
        }

        [Fact]
        public void SymmetricSignatureProvider_Publics()
        {
            var provider = new SymmetricSignatureProvider(KEY.DefaultSymmetricSecurityKey_256, KEY.DefaultSymmetricSigningCreds_256_Sha2.Algorithm);
            EE expectedException = EE.ArgumentOutOfRangeException("IDX10628:");
            try
            {
                provider.MinimumSymmetricKeySizeInBits = SymmetricSignatureProvider.DefaultMinimumSymmetricKeySizeInBits - 10;
                expectedException.ProcessNoException();
            }
            catch (Exception ex)
            {
                expectedException.ProcessException(ex);
            }
        }

        [Theory, MemberData(nameof(KeyDisposeData))]
        public void SignatureProviderDispose_Test(string testId, SecurityKey securityKey, string algorithm, ExpectedException ee)
        {
            try
            {
                var jsonWebKey = securityKey as JsonWebKey;

                if (securityKey is SymmetricSecurityKey symmetricSecurityKey || jsonWebKey?.Kty == JsonWebAlgorithmsKeyTypes.Octet)
                    SymmetricProviderDispose(testId, securityKey, algorithm, ee);
                else
                    AsymmetricProviderDispose(testId, securityKey, algorithm, ee);

                var bytes = new byte[1024];
                var provider = securityKey.CryptoProviderFactory.CreateForSigning(securityKey, algorithm);
                var signature = provider.Sign(bytes);
                securityKey.CryptoProviderFactory.ReleaseSignatureProvider(provider);

                provider = securityKey.CryptoProviderFactory.CreateForSigning(securityKey, algorithm);
                signature = provider.Sign(bytes);
                securityKey.CryptoProviderFactory.ReleaseSignatureProvider(provider);

                provider = securityKey.CryptoProviderFactory.CreateForVerifying(securityKey, algorithm);
                provider.Verify(bytes, signature);

                ee.ProcessNoException();
            }
            catch (Exception ex)
            {
                ee.ProcessException(ex);
            }
        }

        private void AsymmetricProviderDispose(string testId, SecurityKey securityKey, string algorithm, ExpectedException ee)
        {
            try
            {
                var bytes = new byte[256];
                var asymmetricProvider = new AsymmetricSignatureProvider(securityKey, algorithm, true);
                var signature = asymmetricProvider.Sign(bytes);
                asymmetricProvider.Dispose();

                asymmetricProvider = new AsymmetricSignatureProvider(securityKey, algorithm, true);
                signature = asymmetricProvider.Sign(bytes);
                asymmetricProvider.Dispose();

                asymmetricProvider = new AsymmetricSignatureProvider(securityKey, algorithm, false);
                asymmetricProvider.Verify(bytes, signature);

                ee.ProcessNoException();
            }
            catch(Exception ex)
            {
                ee.ProcessException(ex);
            }
        }

        private void SymmetricProviderDispose(string testId, SecurityKey securityKey, string algorithm, EE ee)
        {
            try
            {
                var bytes = new byte[256];
                var symmetricProvider = new SymmetricSignatureProvider(securityKey, algorithm);
                var signature = symmetricProvider.Sign(bytes);
                symmetricProvider.Dispose();

                symmetricProvider = new SymmetricSignatureProvider(securityKey, algorithm);
                signature = symmetricProvider.Sign(bytes);
                symmetricProvider.Dispose();

                symmetricProvider = new SymmetricSignatureProvider(securityKey, algorithm);
                symmetricProvider.Verify(bytes, signature);

                ee.ProcessNoException();
            }
            catch (Exception ex)
            {
                ee.ProcessException(ex);
            }
        }

        public static TheoryData<string, SecurityKey, string, EE> KeyDisposeData()
        {
            var theoryData = new TheoryData<string, SecurityKey, string, EE>
            {
                {
                    "Test2",
                    new RsaSecurityKey(KEY.RsaParameters_2048),
                    ALG.RsaSha256,
                    EE.NoExceptionExpected
                },
                {
                    "Test3",
                    KEY.JsonWebKeyRsa_2048,
                    ALG.RsaSha256,
                    EE.NoExceptionExpected
                },
                {
                    "Test4",
                    KEY.JsonWebKeyP256,
                    ALG.EcdsaSha256,
                    EE.NoExceptionExpected
                },
                {
                    "Test5",
                    KEY.Ecdsa256Key,
                    ALG.EcdsaSha256,
                    EE.NoExceptionExpected
                },
                {
                    "Test6",
                    KEY.SymmetricSecurityKey2_256,
                    ALG.HmacSha256,
                    EE.NoExceptionExpected
                }
            };

            return theoryData;
        }

        [Theory, MemberData(nameof(SignatureTheoryData))]
        public void SignatureTampering(SignatureProviderTheoryData theoryData)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Console.WriteLine("OSX is excluded as the SignatureTampering test is slow (~6 minutes).") ;
            }
            else
            { 
                TestUtilities.WriteHeader($"{this}.SignatureTampering", theoryData);
                var copiedSignature = theoryData.Signature.CloneByteArray();
                for (int i = 0; i < theoryData.Signature.Length; i++)
                {
                    var originalB = theoryData.Signature[i];
                    for (byte b = 0; b < byte.MaxValue; b++)
                    {
                        // skip here as this will succeed
                        if (b == theoryData.Signature[i])
                            continue;

                        copiedSignature[i] = b;
                        Assert.False(theoryData.VerifySignatureProvider.Verify(theoryData.RawBytes, copiedSignature), $"signature should not have verified: {theoryData.TestId} : {i} : {b} : {copiedSignature[i]}");

                        // reset so we move to next byte
                        copiedSignature[i] = originalB;
                    }
                }

                Assert.True(theoryData.VerifySignatureProvider.Verify(theoryData.RawBytes, copiedSignature), "Final check should have verified");
            }
        }

        [Theory, MemberData(nameof(SignatureTheoryData))]
        public void SignatureTruncation(SignatureProviderTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.SignatureTruncation", theoryData);
            for (int i = 0; i < theoryData.Signature.Length - 1; i++)
            {
                var truncatedSignature = new byte[i + 1];
                Array.Copy(theoryData.Signature, truncatedSignature, i + 1);
                Assert.False(theoryData.VerifySignatureProvider.Verify(theoryData.RawBytes, truncatedSignature), $"signature should not have verified: {theoryData.TestId} : {i}");
            }

            Assert.True(theoryData.VerifySignatureProvider.Verify(theoryData.RawBytes, theoryData.Signature), "Final check should have verified");
        }

        public static TheoryData<SignatureProviderTheoryData> SignatureTheoryData()
        {
            var theoryData = new TheoryData<SignatureProviderTheoryData>();

            var rawBytes = Guid.NewGuid().ToByteArray();
            var asymmetricProvider = new AsymmetricSignatureProvider(KEY.DefaultX509Key_2048, ALG.RsaSha256, true);
            theoryData.Add(new SignatureProviderTheoryData
            {
                First = true,
                RawBytes = rawBytes,
                Signature = asymmetricProvider.Sign(rawBytes),
                TestId = ALG.RsaSha256,
                VerifyKey = KEY.DefaultX509Key_2048,
                VerifyAlgorithm = ALG.RsaSha256,
                VerifySignatureProvider = asymmetricProvider
            });

            var asymmetricProvider2 = new AsymmetricSignatureProvider(KEY.Ecdsa256Key, ALG.EcdsaSha256, true);
            theoryData.Add(new SignatureProviderTheoryData
            {
                RawBytes = rawBytes,
                Signature = asymmetricProvider2.Sign(rawBytes),
                TestId = ALG.EcdsaSha256,
                VerifyKey = KEY.Ecdsa256Key,
                VerifyAlgorithm = ALG.EcdsaSha256,
                VerifySignatureProvider = asymmetricProvider2
            });

            var symmetricProvider = new SymmetricSignatureProvider(KEY.SymmetricSecurityKey2_256, ALG.HmacSha256);
            theoryData.Add(new SignatureProviderTheoryData
            {
                RawBytes = rawBytes,
                Signature = symmetricProvider.Sign(rawBytes),
                TestId = ALG.HmacSha256,
                VerifyKey = KEY.SymmetricSecurityKey2_256,
                VerifyAlgorithm = ALG.HmacSha256,
                VerifySignatureProvider = symmetricProvider,
            });

            var symmetricProvider2 = new SymmetricSignatureProvider(KEY.SymmetricSecurityKey2_512, ALG.HmacSha512);
            theoryData.Add(new SignatureProviderTheoryData
            {
                RawBytes = rawBytes,
                Signature = symmetricProvider2.Sign(rawBytes),
                TestId = ALG.HmacSha512,
                VerifyKey = KEY.SymmetricSecurityKey2_512,
                VerifyAlgorithm = ALG.HmacSha512,
                VerifySignatureProvider = symmetricProvider2
            });

            return theoryData;
        }

        /// <summary>
        /// Tests that the signature size returned from TokenUtilities.GetSignatureSize(string algorithm) ia not too small.
        /// Each supported signature is tried, 2k is the default.
        /// </summary>
        /// <param name="theoryData"></param>
        [Theory, MemberData(nameof(MaximumSignatureSizeTestCases), DisableDiscoveryEnumeration = true)]
        public void MaximumSignatureSizeTests(SignTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"MaximumSignatureSizeTests", theoryData);

            try
            {
                byte[] signature = theoryData.SignatureProvider.Sign(theoryData.Bytes);
                int maximumSignatureSize = SupportedAlgorithms.GetMaxByteCount(theoryData.SignatureProvider.Algorithm);
                if (signature.Length > maximumSignatureSize)
                    context.AddDiff($"signature.Length: '{signature.Length}' > maximumSignatureSize: '{maximumSignatureSize}'.");

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SignTheoryData> MaximumSignatureSizeTestCases
        {
            get
            {
                var theoryData = new TheoryData<SignTheoryData>();

                AddSymmetricKeySizes(KeyingMaterial.DefaultSymmetricSecurityKey_256, theoryData);
                AddSymmetricKeySizes(KeyingMaterial.DefaultSymmetricSecurityKey_384, theoryData);
                AddSymmetricKeySizes(KeyingMaterial.DefaultSymmetricSecurityKey_512, theoryData);

                AddECDSAKeySizes(KeyingMaterial.Ecdsa256Key, theoryData);
                AddECDSAKeySizes(KeyingMaterial.Ecdsa384Key, theoryData);
                AddECDSAKeySizes(KeyingMaterial.Ecdsa521Key, theoryData);

                AddRSAKeySize(KeyingMaterial.RsaSecurityKey_1024, theoryData);
                AddRSAKeySize(KeyingMaterial.RsaSecurityKey_2048, theoryData);
                AddRSAKeySize(KeyingMaterial.RsaSecurityKey_4096, theoryData);

                theoryData.Add(new SignTheoryData("Custom2K")
                {
                    SignatureProvider = new SignatureProvider2K(KeyingMaterial.RsaSecurityKey_2048, "CustomAlgorithm")
                });

                return theoryData;
            }
        }
        private static void AddECDSAKeySizes(SecurityKey securityKey, TheoryData<SignTheoryData> theoryData)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());

            foreach (string algorithm in SupportedAlgorithms.EcdsaSigningAlgorithms)
            {
                if (securityKey.KeySize >= AsymmetricSignatureProvider.DefaultMinimumAsymmetricKeySizeInBitsForSigningMap[algorithm])
                    theoryData.Add(new SignTheoryData($"{algorithm}_Key{securityKey.KeySize}")
                    {
                        Bytes = bytes,
                        SignatureProvider = CreateProvider(securityKey, algorithm)
                    });
            }
        }

        private static void AddSymmetricKeySizes(SecurityKey securityKey, TheoryData<SignTheoryData> theoryData)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());

            foreach (string algorithm in SupportedAlgorithms.SymmetricSigningAlgorithms)
            {
                if (securityKey.KeySize / 8 >= SymmetricSignatureProvider.ExpectedSignatureSizeInBytes[algorithm])
                    theoryData.Add(new SignTheoryData($"{algorithm}_Key{securityKey.KeySize}")
                    {
                        Bytes = bytes,
                        SignatureProvider = CreateProvider(securityKey, algorithm)
                    });
            }
        }

        private static void AddRSAKeySize(SecurityKey securityKey, TheoryData<SignTheoryData> theoryData)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());

            foreach (string algorithm in SupportedAlgorithms.RsaSigningAlgorithms)
            {
                if (securityKey.KeySize >= AsymmetricSignatureProvider.DefaultMinimumAsymmetricKeySizeInBitsForSigningMap[algorithm])
                    theoryData.Add(new SignTheoryData($"{algorithm}_Key{securityKey.KeySize}")
                    {
                        Bytes = bytes,
                        SignatureProvider = CreateProvider(securityKey, algorithm)
                    });
            }

            foreach (string algorithm in SupportedAlgorithms.RsaPssSigningAlgorithms)
            {
                if (securityKey.KeySize >= AsymmetricSignatureProvider.DefaultMinimumAsymmetricKeySizeInBitsForSigningMap[algorithm])
                    theoryData.Add(new SignTheoryData($"{algorithm}_Key{securityKey.KeySize}")
                    {
                        Bytes = bytes,
                        SignatureProvider = CreateProvider(securityKey, algorithm)
                    });
            }
        }

#if NET6_0_OR_GREATER
        [Theory, MemberData(nameof(SignUsingSpanTestCases), DisableDiscoveryEnumeration = true)]
        public void SignUsingSpanTests(SignTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader("SignUsingSpanTests", theoryData);

            try
            {
                bool success = theoryData.SignatureProvider.Sign(theoryData.Bytes.AsSpan(), theoryData.Buffer.AsSpan<byte>(), out int bytesWritten);

                IdentityComparer.AreBoolsEqual(success, theoryData.Success, context);
                if (theoryData.Success)
                    IdentityComparer.AreBoolsEqual(theoryData.SignatureProvider.Verify(theoryData.Bytes, theoryData.Buffer.AsSpan<byte>().Slice(0, bytesWritten).ToArray()), true, $"{theoryData.SignatureProvider}", "true", context);

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SignTheoryData> SignUsingSpanTestCases
        {
            get
            {
                TheoryData<SignTheoryData> theoryData = new TheoryData<SignTheoryData>();
                byte[] bytes = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());

                AddSignUsingSpans(bytes, KeyingMaterial.Ecdsa256Key, SecurityAlgorithms.EcdsaSha256, "ECDSA", theoryData);
                AddSignUsingSpans(bytes, KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaSha256, "RSA", theoryData);
                AddSignUsingSpans(bytes, new SymmetricSecurityKey(KeyingMaterial.SymmetricKeyBytes2_256), SecurityAlgorithms.HmacSha256, "HMAC256", theoryData);

                theoryData.Add(new SignTheoryData("NotImplementedException")
                {
                    Buffer = new byte[2048],
                    Bytes = new byte[2048],
                    Count = 2048,
                    ExpectedException = new ExpectedException(typeof(NotImplementedException)),
                    Offset = 0,
                    SignatureProvider = new SignatureProvider2K(KeyingMaterial.Ecdsa256Key, SecurityAlgorithms.EcdsaSha256)
                });

                return theoryData;
            }
        }

        internal static void AddSignUsingSpans(byte[] bytes, SecurityKey securityKey, string algorithm, string prefix, TheoryData<SignTheoryData> theoryData)
        {
            theoryData.Add(new SignTheoryData($"{prefix}_BufferNull")
            {
                Buffer = null,
                Bytes = bytes,
                SignatureProvider = CreateProvider(securityKey, algorithm),
                Success = false
            });

            theoryData.Add(new SignTheoryData($"{prefix}_BufferOneByte")
            {
                Buffer = new byte[1],
                Bytes = bytes,
                SignatureProvider = CreateProvider(securityKey, algorithm),
                Success = false
            });

            theoryData.Add(new SignTheoryData($"{prefix}_BufferTooSmall")
            {
                Buffer = new byte[10],
                Bytes = bytes,
                SignatureProvider = CreateProvider(securityKey, algorithm),
                Success = false
            });

            theoryData.Add(new SignTheoryData($"{prefix}")
            {
                Buffer = new byte[512],
                Bytes = bytes,
                SignatureProvider = CreateProvider(securityKey, algorithm),
                Success = true
            });
        }
#endif

        [Theory, MemberData(nameof(SignUsingOffsetTestCases), DisableDiscoveryEnumeration = true)]
        public void SignUsingOffsetTests(SignTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader("SignUsingOffsetTests", theoryData);
            try
            {
                byte[] signature = theoryData.SignatureProvider.Sign(theoryData.Bytes, theoryData.Offset, theoryData.Count);
                if (theoryData.Success)
                    IdentityComparer.AreBoolsEqual(
                        theoryData.SignatureProvider.Verify(
                            theoryData.Bytes.AsSpan<byte>().Slice(theoryData.Offset, theoryData.Count).ToArray(),
                            signature),
                        true,
                        $"{theoryData.SignatureProvider}",
                        "true",
                        context);

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SignTheoryData> SignUsingOffsetTestCases
        {
            get
            {
                TheoryData<SignTheoryData> theoryData = new TheoryData<SignTheoryData>();

                byte[] bytes = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());
                AddSignUsingOffsets(bytes, KeyingMaterial.Ecdsa256Key, SecurityAlgorithms.EcdsaSha256, "ECDSA", theoryData);
                AddSignUsingOffsets(bytes, KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaSha256, "RSA", theoryData);
                AddSignUsingOffsets(bytes, new SymmetricSecurityKey(KeyingMaterial.SymmetricKeyBytes2_256), SecurityAlgorithms.HmacSha256, "HMAC256", theoryData);

                theoryData.Add(new SignTheoryData("NotImplementedException")
                {
                    Bytes = new byte[1024],
                    Count = 1024,
                    ExpectedException = new ExpectedException(typeof(NotImplementedException)),
                    Offset = 0,
                    SignatureProvider = new SignatureProvider2K(KeyingMaterial.Ecdsa256Key, SecurityAlgorithms.EcdsaSha256)
                });

                return theoryData;
            }
        }

        internal static void AddSignUsingOffsets(byte[] bytes, SecurityKey securityKey, string algorithm, string prefix, TheoryData<SignTheoryData> theoryData)
        {
            theoryData.Add(new SignTheoryData($"{prefix}_BytesNull")
            {
                Bytes = null,
                Count = bytes.Length,
                ExpectedException = ExpectedException.ArgumentNullException(),
                Offset = 0,
                SignatureProvider = CreateProvider(securityKey, algorithm)
            });

            theoryData.Add(new SignTheoryData($"{prefix}_BytesEmpty")
            {
                Bytes = Array.Empty<byte>(),
                Count = bytes.Length,
                ExpectedException = ExpectedException.ArgumentNullException(),
                Offset = 0,
                SignatureProvider = CreateProvider(securityKey, algorithm)
            });

#if NET461 || NET462
            // RSA throws a different exception in the following three cases than HMAC or ECDSA 472+
            theoryData.Add(new SignTheoryData($"{prefix}_CountNegative")
            {
                Bytes = bytes,
                Count = -1,
                ExpectedException = ExpectedException.ArgumentException(),
                Offset = 0,
                SignatureProvider = CreateProvider(securityKey, algorithm)
            });

            theoryData.Add(new SignTheoryData($"{prefix}_CountGreaterThanBytes")
            {
                Bytes = bytes,
                Count = bytes.Length + 1,
                ExpectedException = ExpectedException.ArgumentException(),
                Offset = 0,
                SignatureProvider = CreateProvider(securityKey, algorithm)
            });

            theoryData.Add(new SignTheoryData($"{prefix}_CountPlusOffsetGreaterThanBytes")
            {
                Bytes = bytes,
                Count = 10,
                ExpectedException = ExpectedException.ArgumentException(),
                Offset = bytes.Length - 1,
                SignatureProvider = CreateProvider(securityKey, algorithm)
            });
#else
            // RSA throws a different exception in the following three cases than HMAC or ECDSA 472+
            theoryData.Add(new SignTheoryData($"{prefix}_CountNegative")
            {
                Bytes = bytes,
                Count = -1,
                ExpectedException = prefix == "RSA" ? ExpectedException.ArgumentOutOfRangeException() : ExpectedException.ArgumentException(),
                Offset = 0,
                SignatureProvider = CreateProvider(securityKey, algorithm)
            });

            theoryData.Add(new SignTheoryData($"{prefix}_CountGreaterThanBytes")
            {
                Bytes = bytes,
                Count = bytes.Length + 1,
                ExpectedException = prefix == "RSA" ? ExpectedException.ArgumentOutOfRangeException() : ExpectedException.ArgumentException(),
                Offset = 0,
                SignatureProvider = CreateProvider(securityKey, algorithm)
            });

            theoryData.Add(new SignTheoryData($"{prefix}_CountPlusOffsetGreaterThanBytes")
            {
                Bytes = bytes,
                Count = 10,
                ExpectedException = prefix == "RSA" ? ExpectedException.ArgumentOutOfRangeException() : ExpectedException.ArgumentException(),
                Offset = bytes.Length - 1,
                SignatureProvider = CreateProvider(securityKey, algorithm)
            });
#endif
            theoryData.Add(new SignTheoryData($"{prefix}_OffsetNegative")
            {
                Bytes = bytes,
                Count = bytes.Length,
                ExpectedException = ExpectedException.ArgumentOutOfRangeException(),
                Offset = -1,
                SignatureProvider = CreateProvider(securityKey, algorithm)
            });

            theoryData.Add(new SignTheoryData($"{prefix}")
            {
                Bytes = bytes,
                Count = bytes.Length,
                Offset = 0,
                SignatureProvider = CreateProvider(securityKey, algorithm),
                Success = true
            });

            byte[] bytesOffset = new byte[bytes.Length + 10];
            Array.Copy(bytes, 0, bytesOffset, 5, bytes.Length);
            theoryData.Add(new SignTheoryData($"{prefix}_Offset")
            {
                Bytes = bytesOffset,
                Count = bytes.Length,
                Offset = 5,
                SignatureProvider = CreateProvider(securityKey, algorithm),
                Success = true
            });
        }

        public static SignatureProvider CreateProvider(SecurityKey securityKey, string algorithm)
        {
            if (securityKey is AsymmetricSecurityKey)
                return new AsymmetricSignatureProvider(securityKey, algorithm);

            if (securityKey is SymmetricSecurityKey)
                return new SymmetricSignatureProvider(securityKey, algorithm);

            throw new NotSupportedException($"Unknown securityKey type: '{securityKey}'");
        }
    }

    public class CryptoProviderFactoryTheoryData : TheoryDataBase, IDisposable
    {
        public CryptoProviderFactoryTheoryData() { }
        public CryptoProviderFactoryTheoryData(string testId) : base(testId) { }

        public CryptoProviderFactoryTheoryData(string testId, string algorithm, SecurityKey signingKey, SecurityKey verifyKey, EE expectedException = null)
            : base(testId)
        {
            SigningAlgorithm = algorithm;
            SigningKey = signingKey;
            VerifyKey = verifyKey;
            ExpectedException = expectedException ?? EE.NoExceptionExpected;
        }

        public CryptoProviderFactory CryptoProviderFactory { get; set; } = new CryptoProviderFactory(CryptoProviderCacheTests.CreateCacheForTesting())
        {
            CacheSignatureProviders = false
        };

        public ICryptoProvider CustomCryptoProvider { get; set; }

        public HashAlgorithm HashAlgorithm { get; set; }

        public KeyWrapProvider KeyWrapProvider { get; set; }

        public RsaKeyWrapProvider RsaKeyWrapProvider { get; set; }

        public bool ShouldFindSignSignatureProvider { get; set; }

        public bool ShouldFindVerifySignatureProvider { get; set; }

        public string SigningAlgorithm { get; set; }

        public SecurityKey SigningKey { get; set; }

        public SignatureProvider SigningSignatureProvider { get; set; }

        public string SigningSignatureProviderType { get; set; }

        public override string ToString()
        {
            return TestId + ", " + SigningAlgorithm + ", " + SigningKey;
        }

        public void Dispose()
        {
            if (CryptoProviderFactory?.CryptoProviderCache is IDisposable disposableCache)
                disposableCache.Dispose();
        }

        public string VerifyAlgorithm { get; set; }

        public SecurityKey VerifyKey { get; set; }

        public SignatureProvider VerifySignatureProvider { get; set; }

        public string VerifySignatureProviderType { get; set; }

        public bool WillCreateSignatures { get; set; } = false;
    }

    public class SignatureProviderTheoryData : CryptoProviderFactoryTheoryData
    {
        public SignatureProviderTheoryData() { }

        public SignatureProviderTheoryData(string testId) : base(testId) { }

        public SignatureProviderTheoryData(string testId, string signingAlgorithm, string verifyAlgorithm, SecurityKey signingKey, SecurityKey verifyKey, EE expectedException = null, bool isValid = true)
        {
            SigningAlgorithm = signingAlgorithm;
            VerifyAlgorithm = verifyAlgorithm;
            SigningKey = signingKey;
            VerifyKey = verifyKey;
            ExpectedException = expectedException ?? EE.NoExceptionExpected;
            IsValid = isValid;
            TestId = testId;
        }

        public bool IsValid { get; set; }

        public byte[] RawBytes { get; set; }

        public byte[] Signature { get; set; }

        public string SignatureProviderType { get; set; }

        public bool VerifyUsingLength { get; set; }
    }

    public class SymmetricSignatureProviderTheoryData : TheoryDataBase
    {
        public SymmetricSignatureProviderTheoryData(string testId) : base(testId) { }

        public string Algorithm { get; set; }

        public SecurityKey SecurityKey { get; set; }
    }

    public class SignTheoryData : TheoryDataBase
    {
        public SignTheoryData() { }

        public SignTheoryData(string testId) : base(testId) { }

        public string Algorithm { get; set; }

        public byte[] Buffer { get; set; }

        public byte[] Bytes { get; set; }

        public int Count { get; set; }

        public string HashAlgorithmString { get; set; }

        public int Offset { get; set; } = 0;

        public SecurityKey SecurityKey { get; set; }

        public byte[] Signature { get; set; }

        public SignatureProvider SignatureProvider { get; set; }

        public bool Success { get; set; }
    }

    public class SignatureProvider2K : SignatureProvider
    {
        public SignatureProvider2K(SecurityKey key, string algorithm):base(key, algorithm){}

        public override byte[] Sign(byte[] input) => new byte[2048];

        public override bool Verify(byte[] input, byte[] signature) => throw new NotImplementedException();

        protected override void Dispose(bool disposing) => throw new NotImplementedException();

        public override bool Verify(byte[] input, int inputOffset, int inputLength, byte[] signature, int signatureOffset, int signatureLength) => throw new NotImplementedException();
    }
}
