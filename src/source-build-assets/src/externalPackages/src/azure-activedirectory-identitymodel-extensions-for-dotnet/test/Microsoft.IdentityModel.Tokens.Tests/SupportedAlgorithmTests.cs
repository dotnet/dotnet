// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Tests
{
    public class SupportedAlgorithmTests
    {
        /// <summary>
        /// This test ensures that:
        /// 1. CryptoProviderFactory.IsSupportedAlgorithm && SecurityKey.IsSupportedAlgorithm have same logic.
        /// 2. Our default algorithms are supported.
        /// </summary>
        /// <param name="theoryData"></param>
        [Theory, MemberData(nameof(IsSupportedAlgorithmAndKeyTestCases))]
        public void IsSupportedAlgorithmAndKey(SupportedAlgorithmTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.IsSupportedAlgorithm", theoryData);

            try
            {
                if (theoryData.SecurityKey.CryptoProviderFactory.IsSupportedAlgorithm(theoryData.Algorithm, theoryData.SecurityKey) != theoryData.IsSupportedAlgorithm)
                    context.AddDiff($"SecurityKey.CryptoProviderFactory.IsSupportedAlgorithm != theoryData.IsSupportedAlgorithm. Algorithm: '{theoryData.Algorithm}', theoryData.SecurityKey: '{theoryData.SecurityKey}', theoryData.IsSupportedAlgorithm: '{theoryData.IsSupportedAlgorithm}'.");

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch(Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            try
            {
                if (theoryData.SecurityKey.IsSupportedAlgorithm(theoryData.Algorithm) != theoryData.IsSupportedAlgorithm)
                    context.AddDiff($"SecurityKey.IsSupportedAlgorithm != theoryData.IsSupportedAlgorithm. Algorithm: '{theoryData.Algorithm}', theoryData.SecurityKey: '{theoryData.SecurityKey}', theoryData.IsSupportedAlgorithm: '{theoryData.IsSupportedAlgorithm}'.");

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SupportedAlgorithmTheoryData> IsSupportedAlgorithmAndKeyTestCases
        {
            get
            {
                var theoryData = new TheoryData<SupportedAlgorithmTheoryData>();

                // ECDsaSecurityKey
                foreach (var alg in SupportedAlgorithms.EcdsaSigningAlgorithms)
                    SupportedAlgorithmTheoryData.AddTestCase(alg, KeyingMaterial.Ecdsa256Key, true, $"Ecdsa_{alg}", theoryData);

                SupportedAlgorithmTheoryData.AddTestCase(null, KeyingMaterial.Ecdsa384Key, false, "Ecdsa_NULL_Aes128Encryption", theoryData);
                SupportedAlgorithmTheoryData.AddTestCase(SecurityAlgorithms.Aes128Encryption, KeyingMaterial.Ecdsa384Key, false, "Ecdsa_Aes128Encryption", theoryData);
                SupportedAlgorithmTheoryData.AddTestCase(SecurityAlgorithms.RsaSsaPssSha256Signature,
                    new ECDsaSecurityKey(KeyingMaterial.Ecdsa256Key.ECDsa)
                    {
                        CryptoProviderFactory = new CustomCryptoProviderFactory(new string[] { SecurityAlgorithms.RsaSsaPssSha256Signature })
                    },
                    true,
                    "Ecdsa_CustomCryptoProviderFactory",
                    theoryData);

                // JsonWebKey - could have combined with other loops, but decided to keep things seperate
                // ECD
                foreach (var alg in SupportedAlgorithms.EcdsaSigningAlgorithms)
                    SupportedAlgorithmTheoryData.AddTestCase(alg, KeyingMaterial.JsonWebKeyP256, true, $"JsonWebKey_Ecdsa_{alg}", theoryData);

                // RSA
                foreach (var alg in SupportedAlgorithms.RsaEncryptionAlgorithms)
                    SupportedAlgorithmTheoryData.AddTestCase(alg, KeyingMaterial.JsonWebKeyRsa_2048, true, $"JsonWebKey_Rsa_{alg}", theoryData);

                foreach (var alg in SupportedAlgorithms.RsaSigningAlgorithms)
                    SupportedAlgorithmTheoryData.AddTestCase(alg, KeyingMaterial.JsonWebKeyRsa_2048, true, $"JsonWebKey_Rsa_{alg}", theoryData);

                foreach (var alg in SupportedAlgorithms.RsaPssSigningAlgorithms)
                    SupportedAlgorithmTheoryData.AddTestCase(alg, KeyingMaterial.JsonWebKeyRsa_2048, true, $"JsonWebKeyRsa_2048_{alg}", theoryData);

                // Symmetric
                foreach (var alg in SupportedAlgorithms.SymmetricEncryptionAlgorithms)
                    SupportedAlgorithmTheoryData.AddTestCase(alg, KeyingMaterial.JsonWebKeySymmetric256, true, $"JsonWebKey_Symmetric_{alg}", theoryData);

                foreach (var alg in SupportedAlgorithms.SymmetricKeyWrapAlgorithms)
                    SupportedAlgorithmTheoryData.AddTestCase(alg, KeyingMaterial.JsonWebKeySymmetric256, true, $"JsonWebKey_Symmetric_{alg}", theoryData);

                foreach (var alg in SupportedAlgorithms.SymmetricSigningAlgorithms)
                    SupportedAlgorithmTheoryData.AddTestCase(alg, KeyingMaterial.JsonWebKeySymmetric256, true, $"JsonWebKey_Symmetric_{alg}", theoryData);

                SupportedAlgorithmTheoryData.AddTestCase(SecurityAlgorithms.RsaSha256Signature, KeyingMaterial.JsonWebKeyP256, false, "JsonWebKey_Escsa_RsaSha256Signature", theoryData);
                SupportedAlgorithmTheoryData.AddTestCase(SecurityAlgorithms.EcdsaSha256, KeyingMaterial.JsonWebKeyRsa_2048, false, "JsonWebKey_Rsa_EcdsaSha256", theoryData);
                SupportedAlgorithmTheoryData.AddTestCase(SecurityAlgorithms.HmacSha256, KeyingMaterial.JsonWebKeyRsa_2048, false, "JsonWebKey_Rsa_HmacSha256", theoryData);
                SupportedAlgorithmTheoryData.AddTestCase(SecurityAlgorithms.RsaSha256Signature,
                    new JsonWebKey
                    {
                        CryptoProviderFactory = new CustomCryptoProviderFactory(new string[] { SecurityAlgorithms.RsaSha256Signature }),
                        Kty = JsonWebAlgorithmsKeyTypes.Octet,
                        K = KeyingMaterial.DefaultSymmetricKeyEncoded_256
                    },
                    true,
                    "JsonWebKey_Symmetric_CustomCryptoProviderFactory",
                    theoryData);

                // RsaSecurityKey
                foreach (var alg in SupportedAlgorithms.RsaEncryptionAlgorithms)
                    SupportedAlgorithmTheoryData.AddTestCase(alg, KeyingMaterial.RsaSecurityKey_2048, true, $"Rsa_{alg}", theoryData);

                foreach (var alg in SupportedAlgorithms.RsaSigningAlgorithms)
                    SupportedAlgorithmTheoryData.AddTestCase(alg, KeyingMaterial.RsaSecurityKey_2048, true, $"Rsa_{alg}", theoryData);

                foreach (var alg in SupportedAlgorithms.RsaPssSigningAlgorithms)
                {
                    SupportedAlgorithmTheoryData.AddTestCase(alg, KeyingMaterial.RsaSecurityKey_2048, true, $"Rsa_{alg}", theoryData);
                }

                SupportedAlgorithmTheoryData.AddTestCase(SecurityAlgorithms.EcdsaSha256,
                    new RsaSecurityKey(KeyingMaterial.RsaParameters1)
                    {
                        CryptoProviderFactory = new CustomCryptoProviderFactory(new string[] { SecurityAlgorithms.EcdsaSha256 })
                    },
                    true,
                    "Rsa_CustomCryptoProviderFactory",
                    theoryData);

                // SymmetricSecurityKey
                foreach(var alg in SupportedAlgorithms.SymmetricEncryptionAlgorithms)
                    SupportedAlgorithmTheoryData.AddTestCase(alg, KeyingMaterial.DefaultSymmetricSecurityKey_256, true, $"Symmetric_{alg}", theoryData);

                foreach (var alg in SupportedAlgorithms.SymmetricKeyWrapAlgorithms)
                    SupportedAlgorithmTheoryData.AddTestCase(alg, KeyingMaterial.DefaultSymmetricSecurityKey_256, true, $"Symmetric_{alg}", theoryData);

                foreach (var alg in SupportedAlgorithms.SymmetricSigningAlgorithms)
                    SupportedAlgorithmTheoryData.AddTestCase(alg, KeyingMaterial.DefaultSymmetricSecurityKey_256, true, $"Symmetric_{alg}", theoryData);

                SupportedAlgorithmTheoryData.AddTestCase(SecurityAlgorithms.Aes128Encryption, KeyingMaterial.DefaultSymmetricSecurityKey_256, false, "Symmetric_Aes128Encryption", theoryData);
                SupportedAlgorithmTheoryData.AddTestCase(SecurityAlgorithms.Aes128Encryption,
                    new SymmetricSecurityKey(KeyingMaterial.DefaultSymmetricKeyBytes_256)
                    {
                        CryptoProviderFactory = new CustomCryptoProviderFactory(new string[] { SecurityAlgorithms.Aes128Encryption })
                    },
                    true,
                    "Symmetric_CustomCryptoProviderFactory",
                    theoryData);

                // X509SecurityKey
                foreach (var alg in SupportedAlgorithms.RsaEncryptionAlgorithms)
                    SupportedAlgorithmTheoryData.AddTestCase(alg, KeyingMaterial.X509SecurityKeySelfSigned2048_SHA256, true, $"X509_{alg}", theoryData);

                foreach (var alg in SupportedAlgorithms.RsaSigningAlgorithms)
                    SupportedAlgorithmTheoryData.AddTestCase(alg, KeyingMaterial.X509SecurityKeySelfSigned2048_SHA256, true, $"X509_{alg}", theoryData);

                foreach (var alg in SupportedAlgorithms.RsaPssSigningAlgorithms)
                    SupportedAlgorithmTheoryData.AddTestCase(alg, KeyingMaterial.X509SecurityKeySelfSigned2048_SHA256, true, $"X509_{alg}", theoryData);

                SupportedAlgorithmTheoryData.AddTestCase(SecurityAlgorithms.Aes128Encryption, KeyingMaterial.X509SecurityKeySelfSigned2048_SHA512, false, "X509_Aes128Encryption", theoryData);
                SupportedAlgorithmTheoryData.AddTestCase(SecurityAlgorithms.RsaSsaPssSha256Signature,
                    new X509SecurityKey(KeyingMaterial.CertSelfSigned2048_SHA256)
                    {
                        CryptoProviderFactory = new CustomCryptoProviderFactory(new string[] { SecurityAlgorithms.RsaSsaPssSha256Signature })
                    },
                    true,
                    "X509_CustomCryptoProviderFactory",
                    theoryData);

                return theoryData;
            }
        }

        [Theory, MemberData(nameof(IsSymmetricKeyWrapSupportedTests))]
        public void IsSymmetricKeyWrapSupported(SupportedAlgorithmTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.IsSymmetricKeyWrapSupported", theoryData);

            try
            {
                if (SupportedAlgorithms.IsSupportedSymmetricKeyWrap(theoryData.Algorithm, theoryData.SecurityKey) != theoryData.IsSupportedAlgorithm)
                    context.AddDiff($"SupportedAlgorithms.IsSymmetricKeyWrapSupported != theoryData.IsSupportedAlgorithm. Algorithm: '{theoryData.Algorithm}', theoryData.SecurityKey: '{theoryData.SecurityKey}', theoryData.IsSupportedAlgorithm: '{theoryData.IsSupportedAlgorithm}'.");

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SupportedAlgorithmTheoryData> IsSymmetricKeyWrapSupportedTests
        {
            get
            {
                var theoryData = new TheoryData<SupportedAlgorithmTheoryData>();

                foreach (var alg in SupportedAlgorithms.SymmetricKeyWrapAlgorithms)
                    SupportedAlgorithmTheoryData.AddTestCase(alg, KeyingMaterial.JsonWebKeySymmetric256, true, $"JsonWebKey_Symmetric_{alg}", theoryData);

                foreach (var alg in SupportedAlgorithms.SymmetricKeyWrapAlgorithms)
                    SupportedAlgorithmTheoryData.AddTestCase(alg, KeyingMaterial.DefaultSymmetricSecurityKey_256, true, $"Symmetric_{alg}", theoryData);

                SupportedAlgorithmTheoryData.AddTestCase(SecurityAlgorithms.Aes128KW, KeyingMaterial.Ecdsa384Key, false, "Ecdsa", theoryData);
                SupportedAlgorithmTheoryData.AddTestCase(SecurityAlgorithms.Aes128KW, KeyingMaterial.JsonWebKeyP256, false, "JsonWebKey_Ecdsa", theoryData);
                SupportedAlgorithmTheoryData.AddTestCase(SecurityAlgorithms.Aes128KW, KeyingMaterial.JsonWebKeyRsa_2048, false, "JsonWebKey_Rsa", theoryData);
                SupportedAlgorithmTheoryData.AddTestCase(SecurityAlgorithms.Aes128KW, KeyingMaterial.X509SecurityKey1, false, "X509", theoryData);

                return theoryData;
            }
        }

        [Theory, MemberData(nameof(GetDigestFromSignatureAlgorithmTests))]
        public void GetDigestFromSignatureAlgorithm(SupportedAlgorithmTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.GetDigestFromSignatureAlgorithm", theoryData);

            try
            {
                if (!theoryData.Digest.Equals(SupportedAlgorithms.GetDigestFromSignatureAlgorithm(theoryData.Algorithm)))
                    context.AddDiff($"(!theoryData.Digest.Equals(SupportedAlgorithms.GetDigestFromSignatureAlgorithm(theoryData.Algorithm)). '{theoryData.Digest}' != Expected result from: '{theoryData.Algorithm}'.");

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SupportedAlgorithmTheoryData> GetDigestFromSignatureAlgorithmTests
        {
            get
            {
                return new TheoryData<SupportedAlgorithmTheoryData>
                {
                    new SupportedAlgorithmTheoryData{ TestId = SecurityAlgorithms.EcdsaSha256, Algorithm = SecurityAlgorithms.EcdsaSha256, Digest = SecurityAlgorithms.Sha256 },
                    new SupportedAlgorithmTheoryData{ TestId = SecurityAlgorithms.EcdsaSha256Signature, Algorithm = SecurityAlgorithms.EcdsaSha256Signature, Digest = SecurityAlgorithms.Sha256Digest },
                    new SupportedAlgorithmTheoryData{ TestId = SecurityAlgorithms.EcdsaSha384, Algorithm = SecurityAlgorithms.EcdsaSha384, Digest = SecurityAlgorithms.Sha384 },
                    new SupportedAlgorithmTheoryData{ TestId = SecurityAlgorithms.EcdsaSha384Signature, Algorithm = SecurityAlgorithms.EcdsaSha384Signature , Digest = SecurityAlgorithms.Sha384Digest },
                    new SupportedAlgorithmTheoryData{ TestId = SecurityAlgorithms.EcdsaSha512, Algorithm = SecurityAlgorithms.EcdsaSha512, Digest = SecurityAlgorithms.Sha512 },
                    new SupportedAlgorithmTheoryData{ TestId = SecurityAlgorithms.EcdsaSha512Signature, Algorithm = SecurityAlgorithms.EcdsaSha512Signature, Digest = SecurityAlgorithms.Sha512Digest },
                    new SupportedAlgorithmTheoryData{ TestId = SecurityAlgorithms.HmacSha256, Algorithm = SecurityAlgorithms.HmacSha256, Digest = SecurityAlgorithms.Sha256 },
                    new SupportedAlgorithmTheoryData{ TestId = SecurityAlgorithms.HmacSha256Signature, Algorithm = SecurityAlgorithms.HmacSha256Signature, Digest = SecurityAlgorithms.Sha256Digest },
                    new SupportedAlgorithmTheoryData{ TestId = SecurityAlgorithms.HmacSha384, Algorithm = SecurityAlgorithms.HmacSha384, Digest = SecurityAlgorithms.Sha384 },
                    new SupportedAlgorithmTheoryData{ TestId = SecurityAlgorithms.HmacSha384Signature, Algorithm = SecurityAlgorithms.HmacSha384Signature, Digest = SecurityAlgorithms.Sha384Digest },
                    new SupportedAlgorithmTheoryData{ TestId = SecurityAlgorithms.HmacSha512, Algorithm = SecurityAlgorithms.HmacSha512, Digest = SecurityAlgorithms.Sha512 },
                    new SupportedAlgorithmTheoryData{ TestId = SecurityAlgorithms.HmacSha512Signature, Algorithm = SecurityAlgorithms.HmacSha512Signature, Digest = SecurityAlgorithms.Sha512Digest },
                    new SupportedAlgorithmTheoryData{ TestId = SecurityAlgorithms.RsaSha256, Algorithm = SecurityAlgorithms.RsaSha256, Digest = SecurityAlgorithms.Sha256 },
                    new SupportedAlgorithmTheoryData{ TestId = SecurityAlgorithms.RsaSha256Signature, Algorithm = SecurityAlgorithms.RsaSha256Signature, Digest = SecurityAlgorithms.Sha256Digest },
                    new SupportedAlgorithmTheoryData{ TestId = SecurityAlgorithms.RsaSha384, Algorithm = SecurityAlgorithms.RsaSha384, Digest = SecurityAlgorithms.Sha384 },
                    new SupportedAlgorithmTheoryData{ TestId = SecurityAlgorithms.RsaSha384Signature, Algorithm = SecurityAlgorithms.RsaSha384Signature, Digest = SecurityAlgorithms.Sha384Digest },
                    new SupportedAlgorithmTheoryData{ TestId = SecurityAlgorithms.RsaSha512, Algorithm = SecurityAlgorithms.RsaSha512, Digest = SecurityAlgorithms.Sha512 },
                    new SupportedAlgorithmTheoryData{ TestId = SecurityAlgorithms.RsaSha512Signature, Algorithm = SecurityAlgorithms.RsaSha512Signature, Digest = SecurityAlgorithms.Sha512Digest },
                    new SupportedAlgorithmTheoryData{ TestId = SecurityAlgorithms.RsaSsaPssSha256, Algorithm = SecurityAlgorithms.RsaSha256, Digest = SecurityAlgorithms.Sha256 },
                    new SupportedAlgorithmTheoryData{ TestId = SecurityAlgorithms.RsaSsaPssSha256Signature, Algorithm = SecurityAlgorithms.RsaSsaPssSha256Signature, Digest = SecurityAlgorithms.Sha256Digest },
                    new SupportedAlgorithmTheoryData{ TestId = SecurityAlgorithms.RsaSsaPssSha384, Algorithm = SecurityAlgorithms.RsaSha384, Digest = SecurityAlgorithms.Sha384 },
                    new SupportedAlgorithmTheoryData{ TestId = SecurityAlgorithms.RsaSsaPssSha384Signature, Algorithm = SecurityAlgorithms.RsaSsaPssSha384Signature, Digest = SecurityAlgorithms.Sha384Digest },
                    new SupportedAlgorithmTheoryData{ TestId = SecurityAlgorithms.RsaSsaPssSha512, Algorithm = SecurityAlgorithms.RsaSha512, Digest = SecurityAlgorithms.Sha512 },
                    new SupportedAlgorithmTheoryData{ TestId = SecurityAlgorithms.RsaSsaPssSha512Signature, Algorithm = SecurityAlgorithms.RsaSsaPssSha512Signature, Digest = SecurityAlgorithms.Sha512Digest }
                };
            }
        }
    }
}
