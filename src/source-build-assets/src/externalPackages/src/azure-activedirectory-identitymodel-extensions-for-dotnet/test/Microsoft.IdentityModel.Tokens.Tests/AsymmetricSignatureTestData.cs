// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

namespace Microsoft.IdentityModel.Tokens.Tests
{
    public class AsymmetricSignatureTestData
    {
        public static readonly List<Tuple<X509Certificate2, X509Certificate2, string>> Certificates = new List<Tuple<X509Certificate2, X509Certificate2, string>>
        {
            { KeyingMaterial.CertSelfSigned2048_SHA256, KeyingMaterial.CertSelfSigned2048_SHA256_Public, "Cert1" },
            { KeyingMaterial.CertSelfSigned2048_SHA384, KeyingMaterial.CertSelfSigned2048_SHA384_Public, "Cert2" },
            { KeyingMaterial.CertSelfSigned2048_SHA512, KeyingMaterial.CertSelfSigned2048_SHA512_Public, "Cert3" }
        };

        public static readonly List<Tuple<ECDsaSecurityKey, ECDsaSecurityKey, string>> ECDsaSecurityKeys = new List<Tuple<ECDsaSecurityKey, ECDsaSecurityKey, string>>
        {
            { KeyingMaterial.Ecdsa256Key, KeyingMaterial.Ecdsa256Key_Public, "ECDsaKey1" },
            { KeyingMaterial.Ecdsa384Key, KeyingMaterial.Ecdsa384Key_Public, "ECDsaKey2" },
            { KeyingMaterial.Ecdsa521Key, KeyingMaterial.Ecdsa521Key_Public, "ECDsaKey3" }
        };

        public static readonly List<Tuple<JsonWebKey, JsonWebKey, string>> JsonECDsaSecurityKeys = new List<Tuple<JsonWebKey, JsonWebKey, string>>
        {
            { KeyingMaterial.JsonWebKeyP256, KeyingMaterial.JsonWebKeyP256_Public, "JsonKey1" },
        };

        public static readonly List<Tuple<JsonWebKey, JsonWebKey, string>> JsonRsaSecurityKeys = new List<Tuple<JsonWebKey, JsonWebKey, string>>
        {
            { KeyingMaterial.JsonWebKeyRsa_2048, KeyingMaterial.JsonWebKeyRsa_2048_Public, "JsonKey1" },
        };

        public static readonly List<Tuple<JsonWebKey, JsonWebKey, string>> JsonX509SecurityKeys = new List<Tuple<JsonWebKey, JsonWebKey, string>>
        {
            { KeyingMaterial.JsonWebKeyX509_2048, KeyingMaterial.JsonWebKeyX509_2048, "JsonKey1" }
        };

        public static readonly List<Tuple<string, string>> ECDsaSigningAlgorithms = new List<Tuple<string, string>>
        {
            { SecurityAlgorithms.EcdsaSha256, SecurityAlgorithms.EcdsaSha256 },
            { SecurityAlgorithms.EcdsaSha256, SecurityAlgorithms.EcdsaSha256Signature },
            { SecurityAlgorithms.EcdsaSha256Signature, SecurityAlgorithms.EcdsaSha256 },
            { SecurityAlgorithms.EcdsaSha256Signature, SecurityAlgorithms.EcdsaSha256Signature },
            { SecurityAlgorithms.EcdsaSha384, SecurityAlgorithms.EcdsaSha384 },
            { SecurityAlgorithms.EcdsaSha384,SecurityAlgorithms.EcdsaSha384Signature },
            { SecurityAlgorithms.EcdsaSha384Signature, SecurityAlgorithms.EcdsaSha384 },
            { SecurityAlgorithms.EcdsaSha384Signature, SecurityAlgorithms.EcdsaSha384Signature },
            { SecurityAlgorithms.EcdsaSha512, SecurityAlgorithms.EcdsaSha512 },
            { SecurityAlgorithms.EcdsaSha512, SecurityAlgorithms.EcdsaSha512Signature },
            { SecurityAlgorithms.EcdsaSha512Signature, SecurityAlgorithms.EcdsaSha512 },
            { SecurityAlgorithms.EcdsaSha512Signature,SecurityAlgorithms.EcdsaSha512Signature }
        };

        public static readonly List<Tuple<RsaSecurityKey, RsaSecurityKey, string>> RsaSecurityKeys = new List<Tuple<RsaSecurityKey, RsaSecurityKey, string>>
        {
            { KeyingMaterial.RsaSecurityKey_2048, KeyingMaterial.RsaSecurityKey_2048_Public, "RSAKey1" },
            { KeyingMaterial.RsaSecurityKey_4096, KeyingMaterial.RsaSecurityKey_4096_Public, "RSAKey2" }
        };

        public static List<Tuple<string, string>> RsaSigningAlgorithms = new List<Tuple<string, string>>
        {
            { SecurityAlgorithms.RsaSha256, SecurityAlgorithms.RsaSha256 },
            { SecurityAlgorithms.RsaSha256, SecurityAlgorithms.RsaSha256Signature },
            { SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.RsaSha256 },
            { SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.RsaSha256Signature },
            { SecurityAlgorithms.RsaSha384, SecurityAlgorithms.RsaSha384 },
            { SecurityAlgorithms.RsaSha384, SecurityAlgorithms.RsaSha384Signature },
            { SecurityAlgorithms.RsaSha384Signature, SecurityAlgorithms.RsaSha384 },
            { SecurityAlgorithms.RsaSha384Signature, SecurityAlgorithms.RsaSha384Signature },
            { SecurityAlgorithms.RsaSha512, SecurityAlgorithms.RsaSha512 },
            { SecurityAlgorithms.RsaSha512, SecurityAlgorithms.RsaSha512Signature },
            { SecurityAlgorithms.RsaSha512Signature, SecurityAlgorithms.RsaSha512 },
            { SecurityAlgorithms.RsaSha512Signature, SecurityAlgorithms.RsaSha512Signature },
        };

        public static List<Tuple<string, string>> RsaPssSigningAlgorithms = new List<Tuple<string, string>>
        {
            { SecurityAlgorithms.RsaSsaPssSha256, SecurityAlgorithms.RsaSsaPssSha256 },
            { SecurityAlgorithms.RsaSsaPssSha256, SecurityAlgorithms.RsaSsaPssSha256Signature },
            { SecurityAlgorithms.RsaSsaPssSha256Signature, SecurityAlgorithms.RsaSsaPssSha256 },
            { SecurityAlgorithms.RsaSsaPssSha256Signature, SecurityAlgorithms.RsaSsaPssSha256Signature },
            { SecurityAlgorithms.RsaSsaPssSha384, SecurityAlgorithms.RsaSsaPssSha384 },
            { SecurityAlgorithms.RsaSsaPssSha384, SecurityAlgorithms.RsaSsaPssSha384Signature },
            { SecurityAlgorithms.RsaSsaPssSha384Signature, SecurityAlgorithms.RsaSsaPssSha384 },
            { SecurityAlgorithms.RsaSsaPssSha384Signature, SecurityAlgorithms.RsaSsaPssSha384Signature },
            { SecurityAlgorithms.RsaSsaPssSha512, SecurityAlgorithms.RsaSsaPssSha512 },
            { SecurityAlgorithms.RsaSsaPssSha512, SecurityAlgorithms.RsaSsaPssSha512Signature },
            { SecurityAlgorithms.RsaSsaPssSha512Signature, SecurityAlgorithms.RsaSsaPssSha512 },
            { SecurityAlgorithms.RsaSsaPssSha512Signature, SecurityAlgorithms.RsaSsaPssSha512Signature },
        };

        public static readonly List<Tuple<X509SecurityKey, X509SecurityKey, string>> X509SecurityKeys = new List<Tuple<X509SecurityKey, X509SecurityKey, string>>
        {
            { KeyingMaterial.X509SecurityKeySelfSigned2048_SHA256, KeyingMaterial.X509SecurityKeySelfSigned2048_SHA256_Public, "X509Key1" },
            { KeyingMaterial.X509SecurityKeySelfSigned2048_SHA384, KeyingMaterial.X509SecurityKeySelfSigned2048_SHA384_Public, "X509Key2" },
            { KeyingMaterial.X509SecurityKeySelfSigned2048_SHA512, KeyingMaterial.X509SecurityKeySelfSigned2048_SHA512_Public, "X509Key3" }
        };

        public static void AddECDsaAlgorithmVariations(SignatureProviderTheoryData theoryData, TheoryData<SignatureProviderTheoryData> variations)
        {
            foreach (var algorithm in ECDsaSigningAlgorithms)
                variations.Add(new SignatureProviderTheoryData
                {
                    SigningAlgorithm = algorithm.Item1,
                    SigningKey = theoryData.SigningKey,
                    TestId = theoryData.TestId + algorithm.Item1 + algorithm.Item2,
                    VerifyAlgorithm = algorithm.Item2,
                    VerifyKey = theoryData.VerifyKey
                });
        }

        public static void AddRsaAlgorithmVariations(SignatureProviderTheoryData theoryData, TheoryData<SignatureProviderTheoryData> variations)
        {
            foreach (var algorithm in RsaSigningAlgorithms)
                variations.Add(new SignatureProviderTheoryData
                {
                    SigningAlgorithm = algorithm.Item1,
                    SigningKey = theoryData.SigningKey,
                    TestId = theoryData.TestId + algorithm.Item1 + algorithm.Item2,
                    VerifyAlgorithm = algorithm.Item2,
                    VerifyKey = theoryData.VerifyKey
                });
        }

        public static void AddRsaPssAlgorithmVariations(SignatureProviderTheoryData theoryData, TheoryData<SignatureProviderTheoryData> variations)
        {
            foreach (var algorithm in RsaPssSigningAlgorithms)
                variations.Add(new SignatureProviderTheoryData
                {
                    SigningAlgorithm = algorithm.Item1,
                    SigningKey = theoryData.SigningKey,
                    TestId = theoryData.TestId + algorithm.Item1 + algorithm.Item2,
                    VerifyAlgorithm = algorithm.Item2,
                    VerifyKey = theoryData.VerifyKey,
                    ExpectedException = theoryData.ExpectedException,
                });
        }
    }
}
