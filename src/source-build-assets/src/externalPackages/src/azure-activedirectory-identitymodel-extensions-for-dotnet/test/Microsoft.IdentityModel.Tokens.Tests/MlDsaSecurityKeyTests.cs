// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens.Experimental;
using Microsoft.IdentityModel.Tokens.Json;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Tests
{
    public class MlDsaSecurityKeyTests
    {
        [MlDsaFact]
        public void Constructor_NullMlDsa_ThrowsArgumentNullException()
        {
            var ee = ExpectedException.ArgumentNullException("mlDsa");
            try
            {
                var key = new MlDsaSecurityKey(null);
                ee.ProcessNoException();
            }
            catch (Exception ex)
            {
                ee.ProcessException(ex);
            }
        }

        [MlDsaFact]
        public void Constructor_ValidMlDsa()
        {
            using var mlDsa = MLDsa.GenerateKey(MLDsaAlgorithm.MLDsa44);
            var key = new MlDsaSecurityKey(mlDsa);

            Assert.NotNull(key.MLDsa);
            Assert.True(key.KeySize > 0, "KeySize should be positive");
        }

        [MlDsaTheory]
        [InlineData("ML-DSA-44")]
        [InlineData("ML-DSA-65")]
        [InlineData("ML-DSA-87")]
        public void KeySize_MatchesExpectedBits(string algorithm)
        {
            var mlDsaAlg = MlDsaAdapter.GetMLDsaAlgorithm(algorithm);
            using var mlDsa = MLDsa.GenerateKey(mlDsaAlg);
            var key = new MlDsaSecurityKey(mlDsa);

            int expectedBits = mlDsaAlg.PublicKeySizeInBytes * 8;
            Assert.Equal(expectedBits, key.KeySize);
        }

        [MlDsaFact]
        public void HasPrivateKey_WithPrivateKey_ReturnsTrue()
        {
            using var mlDsa = MLDsa.GenerateKey(MLDsaAlgorithm.MLDsa44);
            var key = new MlDsaSecurityKey(mlDsa);

#pragma warning disable CS0618 // Type or member is obsolete
            Assert.True(key.HasPrivateKey);
#pragma warning restore CS0618
            Assert.Equal(PrivateKeyStatus.Exists, key.PrivateKeyStatus);
        }

        [MlDsaFact]
        public void HasPrivateKey_WithPublicKeyOnly_ReturnsFalse()
        {
            using var privateKey = MLDsa.GenerateKey(MLDsaAlgorithm.MLDsa44);
            byte[] publicKeyBytes = privateKey.ExportMLDsaPublicKey();
            using var publicOnly = MLDsa.ImportMLDsaPublicKey(MLDsaAlgorithm.MLDsa44, publicKeyBytes);
            var key = new MlDsaSecurityKey(publicOnly);

#pragma warning disable CS0618 // Type or member is obsolete
            Assert.False(key.HasPrivateKey);
#pragma warning restore CS0618
            Assert.Equal(PrivateKeyStatus.DoesNotExist, key.PrivateKeyStatus);
        }

        [MlDsaFact]
        public void CanComputeJwkThumbprint_ReturnsTrue()
        {
            Assert.True(MlDsaKeyingMaterial.MlDsa44Key.CanComputeJwkThumbprint());
            Assert.True(MlDsaKeyingMaterial.MlDsa65Key.CanComputeJwkThumbprint());
            Assert.True(MlDsaKeyingMaterial.MlDsa87Key.CanComputeJwkThumbprint());
        }

        [MlDsaFact]
        public void ComputeJwkThumbprint_IsDeterministic()
        {
            byte[] thumbprint1 = MlDsaKeyingMaterial.MlDsa44Key.ComputeJwkThumbprint();
            byte[] thumbprint2 = MlDsaKeyingMaterial.MlDsa44Key.ComputeJwkThumbprint();

            Assert.Equal(thumbprint1, thumbprint2);
        }

        [MlDsaFact]
        public void ComputeJwkThumbprint_PublicAndPrivateKeysMatch()
        {
            // A key's thumbprint should be the same whether computed from the private or public key
            // since thumbprint only uses public key material.
            byte[] privateThumbprint = MlDsaKeyingMaterial.MlDsa44Key.ComputeJwkThumbprint();
            byte[] publicThumbprint = MlDsaKeyingMaterial.MlDsa44Key_Public.ComputeJwkThumbprint();

            Assert.Equal(privateThumbprint, publicThumbprint);
        }

        [MlDsaFact]
        public void ComputeJwkThumbprint_DifferentKeysProduceDifferentThumbprints()
        {
            byte[] thumbprint44 = MlDsaKeyingMaterial.MlDsa44Key.ComputeJwkThumbprint();
            byte[] thumbprint65 = MlDsaKeyingMaterial.MlDsa65Key.ComputeJwkThumbprint();
            byte[] thumbprint87 = MlDsaKeyingMaterial.MlDsa87Key.ComputeJwkThumbprint();

            Assert.NotEqual(thumbprint44, thumbprint65);
            Assert.NotEqual(thumbprint44, thumbprint87);
            Assert.NotEqual(thumbprint65, thumbprint87);
        }

        [MlDsaTheory]
        [InlineData("ML-DSA-44")]
        [InlineData("ML-DSA-65")]
        [InlineData("ML-DSA-87")]
        public void ConstructFromJsonWebKey_RoundTrips(string algorithm)
        {
            // Create a key, convert to JWK, then create a new key from the JWK
            var mlDsaAlg = MlDsaAdapter.GetMLDsaAlgorithm(algorithm);
            using var originalMlDsa = MLDsa.GenerateKey(mlDsaAlg);
            var originalKey = new MlDsaSecurityKey(originalMlDsa);

            var jwk = JsonWebKeyConverter.ConvertFromMlDsaSecurityKey(originalKey);
            Assert.True(JsonWebKeyConverter.TryConvertToSecurityKey(jwk, out SecurityKey roundTrippedKey));

            var mlDsaKey = Assert.IsType<MlDsaSecurityKey>(roundTrippedKey);
            Assert.Equal(originalKey.KeySize, mlDsaKey.KeySize);
            Assert.Equal(PrivateKeyStatus.Exists, mlDsaKey.PrivateKeyStatus);

            // Verify the public keys match
            byte[] originalPub = originalMlDsa.ExportMLDsaPublicKey();
            byte[] roundTrippedPub = mlDsaKey.MLDsa.ExportMLDsaPublicKey();
            Assert.Equal(originalPub, roundTrippedPub);
        }

        [MlDsaTheory]
        [InlineData("ML-DSA-44")]
        [InlineData("ML-DSA-65")]
        [InlineData("ML-DSA-87")]
        public void X509SecurityKey_MlDsa_KeySize(string algorithm)
        {
            var (x509Key, expectedAlg) = GetX509MlDsaKey(algorithm);
            Assert.Equal(expectedAlg.PublicKeySizeInBytes * 8, x509Key.KeySize);
        }

        [MlDsaTheory]
        [InlineData("ML-DSA-44")]
        [InlineData("ML-DSA-65")]
        [InlineData("ML-DSA-87")]
        public void X509SecurityKey_MlDsa_CanComputeJwkThumbprint(string algorithm)
        {
            var (x509Key, _) = GetX509MlDsaKey(algorithm);

            Assert.True(x509Key.CanComputeJwkThumbprint());
            byte[] thumbprint = x509Key.ComputeJwkThumbprint();
            Assert.NotNull(thumbprint);
            Assert.True(thumbprint.Length > 0);
        }

        [MlDsaTheory]
        [InlineData("ML-DSA-44")]
        [InlineData("ML-DSA-65")]
        [InlineData("ML-DSA-87")]
        public void X509SecurityKey_MlDsa_JwkThumbprint_IsDeterministic(string algorithm)
        {
            var (x509Key, _) = GetX509MlDsaKey(algorithm);

            byte[] thumbprint1 = x509Key.ComputeJwkThumbprint();
            byte[] thumbprint2 = x509Key.ComputeJwkThumbprint();
            Assert.Equal(thumbprint1, thumbprint2);
        }

        private static (X509SecurityKey key, MLDsaAlgorithm alg) GetX509MlDsaKey(string algorithm)
        {
            return algorithm switch
            {
                "ML-DSA-44" => (MlDsaKeyingMaterial.X509MlDsa44Key, MLDsaAlgorithm.MLDsa44),
                "ML-DSA-65" => (MlDsaKeyingMaterial.X509MlDsa65Key, MLDsaAlgorithm.MLDsa65),
                "ML-DSA-87" => (MlDsaKeyingMaterial.X509MlDsa87Key, MLDsaAlgorithm.MLDsa87),
                _ => throw new ArgumentException(algorithm)
            };
        }

        #region X509-to-JWK Conversion Tests

        [MlDsaTheory]
        [InlineData("ML-DSA-44")]
        [InlineData("ML-DSA-65")]
        [InlineData("ML-DSA-87")]
        public void ConvertFromX509SecurityKey_X5cMode(string algorithm)
        {
            var (x509Key, _) = GetX509MlDsaKey(algorithm);

            var jwk = JsonWebKeyConverter.ConvertFromX509SecurityKey(x509Key);

            Assert.Equal(JsonWebAlgorithmsKeyTypes.Akp, jwk.Kty);
            Assert.Equal(algorithm, jwk.Alg);
            Assert.Equal(x509Key.KeyId, jwk.Kid);
            Assert.NotNull(jwk.X5t);
            Assert.Single(jwk.X5c);
            // x5c mode should not include key material
            Assert.Null(jwk.Pub);
            Assert.Null(jwk.Priv);

            // Verify the x5c JWK can be converted back to an X509SecurityKey
            // (requires ML-DSA public key extraction from a cert loaded via x5c)
            if (CanExtractMlDsaPublicKeyFromX509PublicOnlyCert())
            {
                Assert.True(JsonWebKeyConverter.TryConvertToSecurityKey(jwk, out var roundTripped));
                Assert.IsType<X509SecurityKey>(roundTripped);
            }
        }

        [MlDsaTheory]
        [InlineData("ML-DSA-44")]
        [InlineData("ML-DSA-65")]
        [InlineData("ML-DSA-87")]
        public void ConvertFromX509SecurityKey_ExtractKeyMaterial(string algorithm)
        {
            if (!MlDsaKeyingMaterial.CanExtractMlDsaPrivateKeyFromX509())
                return; // skip on platforms that can't extract ML-DSA private keys

            var (x509Key, _) = GetX509MlDsaKey(algorithm);

            var jwk = JsonWebKeyConverter.ConvertFromX509SecurityKey(x509Key, representAsRsaKey: true);

            Assert.Equal(JsonWebAlgorithmsKeyTypes.Akp, jwk.Kty);
            Assert.Equal(algorithm, jwk.Alg);
            Assert.NotNull(jwk.Pub);
            Assert.NotNull(jwk.Priv);
            Assert.True(jwk.HasPrivateKey);

            // Verify round-trip: the extracted JWK should produce a working key
            Assert.True(JsonWebKeyConverter.TryConvertToSecurityKey(jwk, out SecurityKey roundTrippedKey));
            var mlDsaKey = Assert.IsType<MlDsaSecurityKey>(roundTrippedKey);
            Assert.Equal(x509Key.KeySize, mlDsaKey.KeySize);
        }

        [MlDsaTheory]
        [InlineData("ML-DSA-44")]
        [InlineData("ML-DSA-65")]
        [InlineData("ML-DSA-87")]
        public void ConvertFromX509SecurityKey_ExtractKeyMaterial_PublicOnly(string algorithm)
        {
            if (!CanExtractMlDsaPublicKeyFromX509PublicOnlyCert())
                return; // GetMLDsaPublicKey() on public-only certs is not supported on all platforms

            var (x509Key, _) = GetX509MlDsaKey(algorithm);

            // Create a public-only X509SecurityKey by loading only the certificate (no private key)
#if NET9_0_OR_GREATER
            using var publicOnlyCert = X509CertificateLoader.LoadCertificate(x509Key.Certificate.RawData);
#else
#pragma warning disable SYSLIB0057
            using var publicOnlyCert = new X509Certificate2(x509Key.Certificate.RawData);
#pragma warning restore SYSLIB0057
#endif
            var publicOnlyKey = new X509SecurityKey(publicOnlyCert);

            var jwk = JsonWebKeyConverter.ConvertFromX509SecurityKey(publicOnlyKey, representAsRsaKey: true);

            Assert.Equal(JsonWebAlgorithmsKeyTypes.Akp, jwk.Kty);
            Assert.Equal(algorithm, jwk.Alg);
            Assert.NotNull(jwk.Pub);
            Assert.Null(jwk.Priv);
            Assert.False(jwk.HasPrivateKey);
        }

        #endregion

        #region JWK Negative Tests

        [MlDsaFact]
        public void JwkMissingAlg_FailsConversion()
        {
            var jwk = new JsonWebKey
            {
                Kty = JsonWebAlgorithmsKeyTypes.Akp,
                Pub = Base64UrlEncoder.Encode(new byte[1312]) // dummy public key
            };

            Assert.False(JsonWebKeyConverter.TryConvertToSecurityKey(jwk, out _));
        }

        [MlDsaFact]
        public void JwkMissingPub_ThrowsOnConstruction()
        {
            var jwk = new JsonWebKey
            {
                Kty = JsonWebAlgorithmsKeyTypes.Akp,
                Alg = SecurityAlgorithms.MlDsa44
            };

            Assert.Throws<ArgumentException>(() => new MlDsaSecurityKey(jwk, false));
        }

        [MlDsaFact]
        public void JwkInvalidAlg_FailsConversion()
        {
            var jwk = new JsonWebKey
            {
                Kty = JsonWebAlgorithmsKeyTypes.Akp,
                Alg = "UNSUPPORTED-ALG",
                Pub = Base64UrlEncoder.Encode(new byte[1312])
            };

            Assert.False(JsonWebKeyConverter.TryConvertToSecurityKey(jwk, out _));
        }

        #endregion

        #region Pub/Priv Mismatch Tests

        [MlDsaFact]
        public void JwkWithMismatchedPubPriv_FailsConversion()
        {
            // Create two different ML-DSA-44 keys
            using var keyA = MLDsa.GenerateKey(MLDsaAlgorithm.MLDsa44);
            using var keyB = MLDsa.GenerateKey(MLDsaAlgorithm.MLDsa44);

            // Build a JWK with priv from keyA but pub from keyB
            var jwk = new JsonWebKey
            {
                Kty = JsonWebAlgorithmsKeyTypes.Akp,
                Alg = SecurityAlgorithms.MlDsa44,
                Pub = Base64UrlEncoder.Encode(keyB.ExportMLDsaPublicKey()),
                Priv = Base64UrlEncoder.Encode(keyA.ExportMLDsaPrivateSeed())
            };

            // TryConvert should fail because pub does not match the key derived from priv
            Assert.False(JsonWebKeyConverter.TryConvertToSecurityKey(jwk, out _));
        }

        #endregion

        #region Algorithm Mismatch Tests

        [MlDsaTheory]
        [InlineData("ML-DSA-44", "ML-DSA-65")]
        [InlineData("ML-DSA-44", "ML-DSA-87")]
        [InlineData("ML-DSA-65", "ML-DSA-44")]
        [InlineData("ML-DSA-65", "ML-DSA-87")]
        [InlineData("ML-DSA-87", "ML-DSA-44")]
        [InlineData("ML-DSA-87", "ML-DSA-65")]
        public void IsSupportedAlgorithm_RejectsKeyAlgorithmMismatch(string keyAlgorithm, string requestedAlgorithm)
        {
            var key = GetMlDsaKey(keyAlgorithm);
            Assert.False(
                SupportedAlgorithms.IsSupportedAlgorithm(requestedAlgorithm, key),
                $"Expected {requestedAlgorithm} to be rejected for {keyAlgorithm} key");
        }

        [MlDsaTheory]
        [InlineData("ML-DSA-44")]
        [InlineData("ML-DSA-65")]
        [InlineData("ML-DSA-87")]
        public void IsSupportedAlgorithm_AcceptsMatchingKeyAlgorithm(string algorithm)
        {
            var key = GetMlDsaKey(algorithm);
            Assert.True(
                SupportedAlgorithms.IsSupportedAlgorithm(algorithm, key),
                $"Expected {algorithm} to be accepted for matching key");
        }

        [MlDsaTheory]
        [InlineData("ML-DSA-44", "ML-DSA-65")]
        [InlineData("ML-DSA-65", "ML-DSA-87")]
        [InlineData("ML-DSA-87", "ML-DSA-44")]
        public void IsSupportedAlgorithm_RejectsJwkAlgorithmMismatch(string jwkAlgorithm, string requestedAlgorithm)
        {
            var jwk = new JsonWebKey
            {
                Kty = JsonWebAlgorithmsKeyTypes.Akp,
                Alg = jwkAlgorithm,
                Pub = Base64UrlEncoder.Encode(new byte[32]) // dummy
            };
            Assert.False(
                SupportedAlgorithms.IsSupportedAlgorithm(requestedAlgorithm, jwk),
                $"Expected {requestedAlgorithm} to be rejected for JWK with alg={jwkAlgorithm}");
        }

        #endregion

        #region Public-Key-Only Signing Tests

        [MlDsaFact]
        public void SignWithPublicKeyOnly_Throws()
        {
            // Creating a signing provider with a public-only key should fail at construction
            using var privateKey = MLDsa.GenerateKey(MLDsaAlgorithm.MLDsa44);
            byte[] publicKeyBytes = privateKey.ExportMLDsaPublicKey();
            using var publicOnly = MLDsa.ImportMLDsaPublicKey(MLDsaAlgorithm.MLDsa44, publicKeyBytes);
            var publicOnlyKey = new MlDsaSecurityKey(publicOnly);

            Assert.Throws<InvalidOperationException>(() =>
                new AsymmetricSignatureProvider(publicOnlyKey, SecurityAlgorithms.MlDsa44, true));
        }

        [MlDsaFact]
        public void VerifyWithPublicKeyOnly_Succeeds()
        {
            // Verifying should work with a public-only key
            byte[] data = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            var signingProvider = new AsymmetricSignatureProvider(MlDsaKeyingMaterial.MlDsa44Key, SecurityAlgorithms.MlDsa44, true);
            byte[] signature = signingProvider.Sign(data);

            var verifyProvider = new AsymmetricSignatureProvider(MlDsaKeyingMaterial.MlDsa44Key_Public, SecurityAlgorithms.MlDsa44, false);
            Assert.True(verifyProvider.Verify(data, signature));
        }

        #endregion

        #region Signature Correctness Tests

        [MlDsaTheory]
        [InlineData("ML-DSA-44")]
        [InlineData("ML-DSA-65")]
        [InlineData("ML-DSA-87")]
        public void TamperedSignature_FailsVerification(string algorithm)
        {
            byte[] data = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            var key = GetMlDsaKey(algorithm);
            var signingProvider = new AsymmetricSignatureProvider(key, algorithm, true);
            byte[] signature = signingProvider.Sign(data);

            // Tamper with one byte of the signature
            signature[0] ^= 0xFF;

            var verifyProvider = new AsymmetricSignatureProvider(GetMlDsaPublicKey(algorithm), algorithm, false);
            Assert.False(verifyProvider.Verify(data, signature));
        }

        [MlDsaFact]
        public void CrossKeyVerification_Fails()
        {
            // Signature from MlDsa44 key should not verify with a different MlDsa44 key
            byte[] data = new byte[] { 10, 20, 30, 40 };
            var signingProvider = new AsymmetricSignatureProvider(MlDsaKeyingMaterial.MlDsa44Key, SecurityAlgorithms.MlDsa44, true);
            byte[] signature = signingProvider.Sign(data);

            // Create a completely different key pair
            using var differentMlDsa = MLDsa.GenerateKey(MLDsaAlgorithm.MLDsa44);
            var differentKey = new MlDsaSecurityKey(differentMlDsa);
            var verifyProvider = new AsymmetricSignatureProvider(differentKey, SecurityAlgorithms.MlDsa44, false);
            Assert.False(verifyProvider.Verify(data, signature));
        }

        #endregion

        #region Sign/Verify Round-Trip Tests

        [MlDsaTheory]
        [InlineData("ML-DSA-44")]
        [InlineData("ML-DSA-65")]
        [InlineData("ML-DSA-87")]
        public void SignVerify_RoundTrip(string algorithm)
        {
            byte[] data = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var signingKey = GetMlDsaKey(algorithm);
            var verifyKey = GetMlDsaPublicKey(algorithm);

            var signingProvider = new AsymmetricSignatureProvider(signingKey, algorithm, true);
            byte[] signature = signingProvider.Sign(data);

            Assert.NotNull(signature);
            Assert.True(signature.Length > 0);

            var verifyProvider = new AsymmetricSignatureProvider(verifyKey, algorithm, false);
            Assert.True(verifyProvider.Verify(data, signature));
        }

        #endregion

        #region Thumbprint Consistency Tests

        [MlDsaTheory]
        [InlineData("ML-DSA-44")]
        [InlineData("ML-DSA-65")]
        [InlineData("ML-DSA-87")]
        public void JwkThumbprint_MatchesBetweenSecurityKeyAndJsonWebKey(string algorithm)
        {
            // MlDsaSecurityKey and its JWK representation should produce identical thumbprints.
            var key = GetMlDsaKey(algorithm);
            var jwk = JsonWebKeyConverter.ConvertFromMlDsaSecurityKey(key);

            byte[] keyThumbprint = key.ComputeJwkThumbprint();
            byte[] jwkThumbprint = jwk.ComputeJwkThumbprint();

            Assert.Equal(keyThumbprint, jwkThumbprint);
        }

        [MlDsaTheory]
        [InlineData("ML-DSA-44")]
        [InlineData("ML-DSA-65")]
        [InlineData("ML-DSA-87")]
        public void JwkThumbprint_X509MatchesMlDsaSecurityKey(string algorithm)
        {
            var (x509Key, _) = GetX509MlDsaKey(algorithm);

            // Extract the public key from the X509 cert and create a standalone MlDsaSecurityKey.
#pragma warning disable SYSLIB5006
            using var mlDsaPub = x509Key.Certificate.GetMLDsaPublicKey();
#pragma warning restore SYSLIB5006
            var standaloneKey = new MlDsaSecurityKey(mlDsaPub);

            byte[] x509Thumbprint = x509Key.ComputeJwkThumbprint();
            byte[] standaloneThumbprint = standaloneKey.ComputeJwkThumbprint();

            Assert.Equal(x509Thumbprint, standaloneThumbprint);
        }

        #endregion

        #region X509 Algorithm Enforcement Tests

        [MlDsaTheory]
        [InlineData("ML-DSA-44", "ML-DSA-65")]
        [InlineData("ML-DSA-44", "ML-DSA-87")]
        [InlineData("ML-DSA-65", "ML-DSA-44")]
        public void IsSupportedAlgorithm_RejectsX509KeyAlgorithmMismatch(string keyAlgorithm, string requestedAlgorithm)
        {
            var (x509Key, _) = GetX509MlDsaKey(keyAlgorithm);
            Assert.False(
                SupportedAlgorithms.IsSupportedAlgorithm(requestedAlgorithm, x509Key),
                $"Expected {requestedAlgorithm} to be rejected for X509 {keyAlgorithm} key");
        }

        [MlDsaTheory]
        [InlineData("ML-DSA-44")]
        [InlineData("ML-DSA-65")]
        [InlineData("ML-DSA-87")]
        public void IsSupportedAlgorithm_AcceptsMatchingX509KeyAlgorithm(string algorithm)
        {
            var (x509Key, _) = GetX509MlDsaKey(algorithm);
            Assert.True(
                SupportedAlgorithms.IsSupportedAlgorithm(algorithm, x509Key),
                $"Expected {algorithm} to be accepted for matching X509 key");
        }

        #endregion

        #region JWK JSON Serialization Round-Trip

        [MlDsaTheory]
        [InlineData("ML-DSA-44")]
        [InlineData("ML-DSA-65")]
        [InlineData("ML-DSA-87")]
        public void JwkJsonSerialization_RoundTrips(string algorithm)
        {
            var mlDsaAlg = MlDsaAdapter.GetMLDsaAlgorithm(algorithm);
            using var mlDsa = MLDsa.GenerateKey(mlDsaAlg);
            var key = new MlDsaSecurityKey(mlDsa);

            // Convert to JWK
            var originalJwk = JsonWebKeyConverter.ConvertFromMlDsaSecurityKey(key);

            // Serialize to JSON using the custom serializer
            string json = JsonWebKeySerializer.Write(originalJwk);

            // Deserialize back
            var parsedJwk = new JsonWebKey(json);

            // Verify all key properties survived
            Assert.Equal(JsonWebAlgorithmsKeyTypes.Akp, parsedJwk.Kty);
            Assert.Equal(algorithm, parsedJwk.Alg);
            Assert.Equal(originalJwk.Pub, parsedJwk.Pub);
            Assert.Equal(originalJwk.Priv, parsedJwk.Priv);
            Assert.True(parsedJwk.HasPrivateKey);

            // Verify the parsed JWK can create a working key
            Assert.True(JsonWebKeyConverter.TryConvertToSecurityKey(parsedJwk, out SecurityKey roundTrippedKey));
            var mlDsaKey = Assert.IsType<MlDsaSecurityKey>(roundTrippedKey);
            Assert.Equal(key.KeySize, mlDsaKey.KeySize);
        }

        [MlDsaTheory]
        [InlineData("ML-DSA-44")]
        [InlineData("ML-DSA-65")]
        [InlineData("ML-DSA-87")]
        public void JwkJsonSerialization_PublicKeyOnly_RoundTrips(string algorithm)
        {
            var mlDsaAlg = MlDsaAdapter.GetMLDsaAlgorithm(algorithm);
            using var mlDsa = MLDsa.GenerateKey(mlDsaAlg);
            byte[] publicKeyBytes = mlDsa.ExportMLDsaPublicKey();
            using var publicOnly = MLDsa.ImportMLDsaPublicKey(mlDsaAlg, publicKeyBytes);
            var key = new MlDsaSecurityKey(publicOnly);

            var originalJwk = JsonWebKeyConverter.ConvertFromMlDsaSecurityKey(key);
            string json = JsonWebKeySerializer.Write(originalJwk);
            var parsedJwk = new JsonWebKey(json);

            Assert.Equal(JsonWebAlgorithmsKeyTypes.Akp, parsedJwk.Kty);
            Assert.Equal(algorithm, parsedJwk.Alg);
            Assert.Equal(originalJwk.Pub, parsedJwk.Pub);
            Assert.Null(parsedJwk.Priv);
            Assert.False(parsedJwk.HasPrivateKey);
        }

        #endregion

        #region End-to-End JWT Tests

        [MlDsaTheory]
        [InlineData("ML-DSA-44")]
        [InlineData("ML-DSA-65")]
        [InlineData("ML-DSA-87")]
        public async Task JwtCreateAndValidate_EndToEnd(string algorithm)
        {
            var signingKey = GetMlDsaKey(algorithm);
            var verifyKey = GetMlDsaPublicKey(algorithm);

            var handler = new JsonWebTokenHandler();
            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = "https://test-issuer.example.com",
                Audience = "https://test-audience.example.com",
                SigningCredentials = new SigningCredentials(signingKey, algorithm),
                Claims = new System.Collections.Generic.Dictionary<string, object>
                {
                    { "sub", "test-user" },
                    { "name", "Test User" }
                }
            };

            string token = handler.CreateToken(descriptor);
            Assert.False(string.IsNullOrEmpty(token));

            // Validate
            var validationParams = new TokenValidationParameters
            {
                ValidIssuer = "https://test-issuer.example.com",
                ValidAudience = "https://test-audience.example.com",
                IssuerSigningKey = verifyKey,
                ValidateLifetime = false
            };

            var result = await handler.ValidateTokenAsync(token, validationParams);
            Assert.True(result.IsValid, $"Token validation failed: {result.Exception?.Message}");
            Assert.Equal("test-user", result.Claims["sub"]);
        }

        [MlDsaTheory]
        [InlineData("ML-DSA-44")]
        [InlineData("ML-DSA-65")]
        [InlineData("ML-DSA-87")]
        public async Task JwtCreateAndValidate_WithJsonWebKey(string algorithm)
        {
            var signingJwk = GetMlDsaJsonWebKey(algorithm);
            var verifyJwk = GetMlDsaJsonWebKeyPublic(algorithm);

            var handler = new JsonWebTokenHandler();
            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = "https://test-issuer.example.com",
                Audience = "https://test-audience.example.com",
                SigningCredentials = new SigningCredentials(signingJwk, algorithm),
                Claims = new System.Collections.Generic.Dictionary<string, object>
                {
                    { "sub", "jwk-test-user" }
                }
            };

            string token = handler.CreateToken(descriptor);
            Assert.False(string.IsNullOrEmpty(token));

            var validationParams = new TokenValidationParameters
            {
                ValidIssuer = "https://test-issuer.example.com",
                ValidAudience = "https://test-audience.example.com",
                IssuerSigningKey = verifyJwk,
                ValidateLifetime = false
            };

            var result = await handler.ValidateTokenAsync(token, validationParams);
            Assert.True(result.IsValid, $"Token validation failed: {result.Exception?.Message}");
        }

        [MlDsaTheory]
        [InlineData("ML-DSA-44")]
        [InlineData("ML-DSA-65")]
        [InlineData("ML-DSA-87")]
        public void JwtCreateAndValidate_WithJwtSecurityTokenHandler(string algorithm)
        {
            var signingKey = GetMlDsaKey(algorithm);
            var verifyKey = GetMlDsaPublicKey(algorithm);

            var handler = new JwtSecurityTokenHandler();
            handler.InboundClaimTypeMap.Clear();
            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = "https://test-issuer.example.com",
                Audience = "https://test-audience.example.com",
                SigningCredentials = new SigningCredentials(signingKey, algorithm),
                Subject = new CaseSensitiveClaimsIdentity(new[]
                {
                    new Claim("sub", "jwt-handler-user")
                })
            };

            var securityToken = handler.CreateToken(descriptor);
            string token = handler.WriteToken(securityToken);
            Assert.False(string.IsNullOrEmpty(token));

            var validationParams = new TokenValidationParameters
            {
                ValidIssuer = "https://test-issuer.example.com",
                ValidAudience = "https://test-audience.example.com",
                IssuerSigningKey = verifyKey,
                ValidateLifetime = false
            };

            var principal = handler.ValidateToken(token, validationParams, out SecurityToken validatedToken);
            Assert.NotNull(principal);
            Assert.NotNull(validatedToken);
            Assert.Equal("jwt-handler-user", principal.FindFirst("sub")?.Value);
        }

        [MlDsaTheory]
        [InlineData("ML-DSA-44")]
        [InlineData("ML-DSA-65")]
        [InlineData("ML-DSA-87")]
        public async Task JwtCreateAndValidate_WithExperimentalValidationParameters(string algorithm)
        {
            var signingKey = GetMlDsaKey(algorithm);
            var verifyKey = GetMlDsaPublicKey(algorithm);

            var handler = new JsonWebTokenHandler();
            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = "https://test-issuer.example.com",
                Audience = "https://test-audience.example.com",
                SigningCredentials = new SigningCredentials(signingKey, algorithm),
                Claims = new System.Collections.Generic.Dictionary<string, object>
                {
                    { "sub", "experimental-user" }
                }
            };

            string token = handler.CreateToken(descriptor);
            Assert.False(string.IsNullOrEmpty(token));

            var validationParameters = new ValidationParameters();
            validationParameters.ValidIssuers.Add("https://test-issuer.example.com");
            validationParameters.ValidAudiences.Add("https://test-audience.example.com");
            validationParameters.SigningKeys.Add(verifyKey);
            validationParameters.TryAllSigningKeys = true;
            validationParameters.LifetimeValidator = SkipValidationDelegates.SkipLifetimeValidation;
            validationParameters.TokenTypeValidator = SkipValidationDelegates.SkipTokenTypeValidation;

            var result = await ((IResultBasedValidation)handler).ValidateTokenAsync(
                token, validationParameters, new CallContext());

            Assert.True(result.Succeeded, $"Validation failed: {result.Error?.Message}");
            Assert.NotNull(result.Result);
        }

        #endregion

        #region X509 ML-DSA End-to-End JWT Tests

        // On .NET 6, loading an ML-DSA certificate from RawData (without private key) and then
        // calling GetMLDsaPublicKey() throws PlatformNotSupportedException, even though the same
        // call works on a PFX-loaded certificate. This guards tests that create public-only certs.
        private static bool CanExtractMlDsaPublicKeyFromX509PublicOnlyCert()
        {
            try
            {
                // Simulate exactly what the test does: load cert from RawData (public-only)
                // and attempt to extract ML-DSA public key.
#if NET9_0_OR_GREATER
                using var cert = X509CertificateLoader.LoadCertificate(MlDsaKeyingMaterial.MlDsa44Cert.RawData);
#else
#pragma warning disable SYSLIB0057
                using var cert = new X509Certificate2(MlDsaKeyingMaterial.MlDsa44Cert.RawData);
#pragma warning restore SYSLIB0057
#endif
                var x509Key = new X509SecurityKey(cert);
                return x509Key.MlDsaPublicKey != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [MlDsaTheory]
        [InlineData("ML-DSA-44")]
        [InlineData("ML-DSA-65")]
        [InlineData("ML-DSA-87")]
        public async Task JwtCreateAndValidate_WithX509SecurityKey(string algorithm)
        {
            if (!MlDsaKeyingMaterial.CanExtractMlDsaPrivateKeyFromX509())
                return; // skip on platforms that can't extract ML-DSA private keys from X509

            var (x509Key, _) = GetX509MlDsaKey(algorithm);

            var handler = new JsonWebTokenHandler();
            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = "https://test-issuer.example.com",
                Audience = "https://test-audience.example.com",
                SigningCredentials = new SigningCredentials(x509Key, algorithm),
                Claims = new System.Collections.Generic.Dictionary<string, object>
                {
                    { "sub", "x509-test-user" }
                }
            };

            string token = handler.CreateToken(descriptor);
            Assert.False(string.IsNullOrEmpty(token));

            // Validate using the same X509 key (public key only path)
            var validationParams = new TokenValidationParameters
            {
                ValidIssuer = "https://test-issuer.example.com",
                ValidAudience = "https://test-audience.example.com",
                IssuerSigningKey = x509Key,
                ValidateLifetime = false
            };

            var result = await handler.ValidateTokenAsync(token, validationParams);
            Assert.True(result.IsValid, $"Token validation failed: {result.Exception?.Message}");
            Assert.Equal("x509-test-user", result.Claims["sub"]);
        }

        [MlDsaTheory]
        [InlineData("ML-DSA-44")]
        [InlineData("ML-DSA-65")]
        [InlineData("ML-DSA-87")]
        public async Task JwtCreateWithX509_ValidateWithMlDsaKey(string algorithm)
        {
            if (!MlDsaKeyingMaterial.CanExtractMlDsaPrivateKeyFromX509())
                return; // skip on platforms that can't extract ML-DSA private keys from X509

            var (x509Key, _) = GetX509MlDsaKey(algorithm);

            var handler = new JsonWebTokenHandler();
            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = "https://test-issuer.example.com",
                Audience = "https://test-audience.example.com",
                SigningCredentials = new SigningCredentials(x509Key, algorithm),
                Claims = new System.Collections.Generic.Dictionary<string, object>
                {
                    { "sub", "cross-key-user" }
                }
            };

            string token = handler.CreateToken(descriptor);

            // Validate using an MlDsaSecurityKey created from the X509 certificate's public key
#pragma warning disable SYSLIB5006
            using var mlDsaPub = x509Key.Certificate.GetMLDsaPublicKey();
#pragma warning restore SYSLIB5006
            var mlDsaKey = new MlDsaSecurityKey(mlDsaPub);

            var validationParams = new TokenValidationParameters
            {
                ValidIssuer = "https://test-issuer.example.com",
                ValidAudience = "https://test-audience.example.com",
                IssuerSigningKey = mlDsaKey,
                ValidateLifetime = false
            };

            var result = await handler.ValidateTokenAsync(token, validationParams);
            Assert.True(result.IsValid, $"Token validation failed: {result.Exception?.Message}");
            Assert.Equal("cross-key-user", result.Claims["sub"]);
        }

        #endregion

        #region Clone and Dispose Tests

        [MlDsaFact]
        public void CloneMlDsa_PublicKeyOnly_ProducesIndependentInstance()
        {
            // Arrange
            using var original = MLDsa.GenerateKey(MLDsaAlgorithm.MLDsa44);
            byte[] data = new byte[] { 1, 2, 3, 4, 5 };

            // Act
            using var clone = MlDsaAdapter.CloneMlDsa(original, includePrivateKey: false);

            // Assert — clone can verify signatures produced by original
            byte[] signature = original.SignData(data, context: null);
            Assert.True(clone.VerifyData(data, signature, context: null));

            // Clone should NOT be able to sign (public-key only)
            Assert.Throws<CryptographicException>(() => clone.SignData(data, context: null));
        }

        [MlDsaFact]
        public void CloneMlDsa_WithPrivateKey_ProducesSigningCapableInstance()
        {
            // Arrange
            using var original = MLDsa.GenerateKey(MLDsaAlgorithm.MLDsa65);
            byte[] data = new byte[] { 10, 20, 30, 40, 50 };

            // Act
            using var clone = MlDsaAdapter.CloneMlDsa(original, includePrivateKey: true);

            // Assert — clone can sign and original can verify
            byte[] signature = clone.SignData(data, context: null);
            Assert.True(original.VerifyData(data, signature, context: null));
        }

        [MlDsaFact]
        public void CloneMlDsa_IsIndependent_DisposingCloneDoesNotAffectOriginal()
        {
            // Arrange
            using var original = MLDsa.GenerateKey(MLDsaAlgorithm.MLDsa44);
            byte[] data = new byte[] { 1, 2, 3 };

            // Act — create and immediately dispose the clone
            var clone = MlDsaAdapter.CloneMlDsa(original, includePrivateKey: true);
            clone.Dispose();

            // Assert — original still works
            byte[] signature = original.SignData(data, context: null);
            Assert.NotNull(signature);
            Assert.True(original.VerifyData(data, signature, context: null));
        }

        [MlDsaFact]
        public void Dispose_OwnedKey_DisposesMLDsa()
        {
            // Arrange — internal constructor owns the MLDsa
            using var mlDsa = MLDsa.GenerateKey(MLDsaAlgorithm.MLDsa44);
            var jwk = JsonWebKeyConverter.ConvertFromMlDsaSecurityKey(new MlDsaSecurityKey(mlDsa));
            var key = new MlDsaSecurityKey(jwk, usePrivateKey: false);

            // Act
            key.Dispose();

            // Assert — the owned MLDsa should be disposed (accessing it should throw)
            Assert.Throws<ObjectDisposedException>(() => key.MLDsa.ExportMLDsaPublicKey());
        }

        [MlDsaFact]
        public void Dispose_BorrowedKey_DoesNotDisposeMLDsa()
        {
            // Arrange — public constructor does not own the MLDsa
            using var mlDsa = MLDsa.GenerateKey(MLDsaAlgorithm.MLDsa44);
            var key = new MlDsaSecurityKey(mlDsa);

            // Act
            key.Dispose();

            // Assert — the borrowed MLDsa should still be usable
            byte[] exported = mlDsa.ExportMLDsaPublicKey();
            Assert.NotNull(exported);
        }

        #endregion

        #region Concurrency Tests

        [MlDsaTheory]
        [InlineData("ML-DSA-44")]
        [InlineData("ML-DSA-65")]
        [InlineData("ML-DSA-87")]
        public async Task ConcurrentSign_WithSharedProvider_ProducesValidSignatures(string algorithm)
        {
            // Arrange
            var signingKey = GetMlDsaKey(algorithm);
            var verifyKey = GetMlDsaPublicKey(algorithm);
            var signingProvider = new AsymmetricSignatureProvider(signingKey, algorithm, true);
            var verifyProvider = new AsymmetricSignatureProvider(verifyKey, algorithm, false);
            int taskCount = 100;
            using var barrier = new CountdownEvent(taskCount);

            // Act — sign 100 different payloads concurrently on the same provider
            var tasks = new Task<(byte[] data, byte[] signature)>[taskCount];
            for (int i = 0; i < taskCount; i++)
            {
                int index = i;
                tasks[i] = Task.Run(() =>
                {
                    byte[] data = new byte[32];
                    byte[] indexBytes = BitConverter.GetBytes(index);
                    Buffer.BlockCopy(indexBytes, 0, data, 0, indexBytes.Length);

                    // Wait until all tasks are ready before signing concurrently.
                    barrier.Signal();
                    barrier.Wait();

                    byte[] signature = signingProvider.Sign(data);
                    return (data, signature);
                });
            }

            var results = await Task.WhenAll(tasks);

            // Assert — every signature must be valid
            foreach (var (data, signature) in results)
            {
                Assert.NotNull(signature);
                Assert.True(signature.Length > 0, "Signature should not be empty");
                Assert.True(verifyProvider.Verify(data, signature),
                    "Concurrent signature failed verification");
            }
        }

        [MlDsaTheory]
        [InlineData("ML-DSA-44")]
        [InlineData("ML-DSA-65")]
        [InlineData("ML-DSA-87")]
        public async Task ConcurrentVerify_WithSharedProvider_AllSucceed(string algorithm)
        {
            // Arrange — create a signature to verify
            byte[] data = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            var signingKey = GetMlDsaKey(algorithm);
            var verifyKey = GetMlDsaPublicKey(algorithm);
            var signingProvider = new AsymmetricSignatureProvider(signingKey, algorithm, true);
            byte[] signature = signingProvider.Sign(data);
            var verifyProvider = new AsymmetricSignatureProvider(verifyKey, algorithm, false);
            int taskCount = 100;
            using var barrier = new CountdownEvent(taskCount);

            // Act — verify the same signature concurrently on the same provider
            var tasks = new Task<bool>[taskCount];
            for (int i = 0; i < taskCount; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    barrier.Signal();
                    barrier.Wait();
                    return verifyProvider.Verify(data, signature);
                });
            }

            var results = await Task.WhenAll(tasks);

            // Assert
            Assert.All(results, result => Assert.True(result,
                "Concurrent verification returned false"));
        }

        [MlDsaTheory]
        [InlineData("ML-DSA-44")]
        [InlineData("ML-DSA-65")]
        [InlineData("ML-DSA-87")]
        public async Task ConcurrentSignAndVerify_WithSharedKey_ProducesCorrectResults(string algorithm)
        {
            // Arrange — use the same key for both sign and verify concurrently
            var signingKey = GetMlDsaKey(algorithm);
            var verifyKey = GetMlDsaPublicKey(algorithm);
            var signingProvider = new AsymmetricSignatureProvider(signingKey, algorithm, true);
            var verifyProvider = new AsymmetricSignatureProvider(verifyKey, algorithm, false);
            int taskCount = 50;
            using var signBarrier = new CountdownEvent(taskCount);

            // Act — sign all payloads concurrently
            var signTasks = new Task<(byte[] data, byte[] signature)>[taskCount];
            for (int i = 0; i < taskCount; i++)
            {
                int index = i;
                signTasks[i] = Task.Run(() =>
                {
                    byte[] data = new byte[64];
                    byte[] indexBytes = BitConverter.GetBytes(index);
                    Buffer.BlockCopy(indexBytes, 0, data, 0, indexBytes.Length);

                    signBarrier.Signal();
                    signBarrier.Wait();

                    byte[] signature = signingProvider.Sign(data);
                    return (data, signature);
                });
            }

            var signResults = await Task.WhenAll(signTasks);

            // Now verify all signatures concurrently
            using var verifyBarrier = new CountdownEvent(taskCount);
            var verifyTasks = new Task<bool>[taskCount];
            for (int i = 0; i < taskCount; i++)
            {
                var (data, sig) = signResults[i];
                verifyTasks[i] = Task.Run(() =>
                {
                    verifyBarrier.Signal();
                    verifyBarrier.Wait();
                    return verifyProvider.Verify(data, sig);
                });
            }

            var verifyResults = await Task.WhenAll(verifyTasks);

            // Assert
            Assert.All(verifyResults, result => Assert.True(result,
                "Signature produced under concurrency failed verification"));
        }

        #endregion

        #region Helpers

        private static MlDsaSecurityKey GetMlDsaKey(string algorithm) => algorithm switch
        {
            "ML-DSA-44" => MlDsaKeyingMaterial.MlDsa44Key,
            "ML-DSA-65" => MlDsaKeyingMaterial.MlDsa65Key,
            "ML-DSA-87" => MlDsaKeyingMaterial.MlDsa87Key,
            _ => throw new ArgumentException(algorithm)
        };

        private static MlDsaSecurityKey GetMlDsaPublicKey(string algorithm) => algorithm switch
        {
            "ML-DSA-44" => MlDsaKeyingMaterial.MlDsa44Key_Public,
            "ML-DSA-65" => MlDsaKeyingMaterial.MlDsa65Key_Public,
            "ML-DSA-87" => MlDsaKeyingMaterial.MlDsa87Key_Public,
            _ => throw new ArgumentException(algorithm)
        };

        private static JsonWebKey GetMlDsaJsonWebKey(string algorithm) => algorithm switch
        {
            "ML-DSA-44" => MlDsaKeyingMaterial.JsonWebKeyMlDsa44,
            "ML-DSA-65" => MlDsaKeyingMaterial.JsonWebKeyMlDsa65,
            "ML-DSA-87" => MlDsaKeyingMaterial.JsonWebKeyMlDsa87,
            _ => throw new ArgumentException(algorithm)
        };

        private static JsonWebKey GetMlDsaJsonWebKeyPublic(string algorithm) => algorithm switch
        {
            "ML-DSA-44" => MlDsaKeyingMaterial.JsonWebKeyMlDsa44_Public,
            "ML-DSA-65" => MlDsaKeyingMaterial.JsonWebKeyMlDsa65_Public,
            "ML-DSA-87" => MlDsaKeyingMaterial.JsonWebKeyMlDsa87_Public,
            _ => throw new ArgumentException(algorithm)
        };

        #endregion
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
