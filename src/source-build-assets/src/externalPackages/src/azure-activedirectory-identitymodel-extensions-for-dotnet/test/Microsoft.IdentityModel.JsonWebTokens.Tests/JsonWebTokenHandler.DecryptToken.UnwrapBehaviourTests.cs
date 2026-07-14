// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace Microsoft.IdentityModel.JsonWebTokens.Tests
{
    /// <summary>
    /// Tests covering the unwrap-then-decrypt behaviour of <see cref="JsonWebTokenHandler"/>.
    /// When a key cannot produce a usable Content Encryption Key, the handler now uses a
    /// placeholder key and proceeds to decryption. As a result, both unwrap-stage and
    /// decryption-stage failures surface to callers as
    /// <see cref="SecurityTokenDecryptionFailedException"/> (IDX10603) rather than
    /// <see cref="SecurityTokenKeyWrapException"/> (IDX10618).
    /// </summary>
    public class JsonWebTokenHandlerDecryptTokenUnwrapBehaviourTests
    {
        /// <summary>
        /// Happy path: a JWE produced and consumed with matching keys decrypts cleanly.
        /// </summary>
        /// <remarks>
        /// AES-GCM content encryption is not exercised here because the handler only
        /// supports GCM with the direct-key alg, which bypasses the unwrap path
        /// entirely. The GCM branches of the size lookup are defensive only.
        /// </remarks>
        [Theory]
        [InlineData(SecurityAlgorithms.RsaOAEP, SecurityAlgorithms.Aes128CbcHmacSha256)]
        [InlineData(SecurityAlgorithms.RsaOAEP, SecurityAlgorithms.Aes256CbcHmacSha512)]
        [InlineData(SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes128CbcHmacSha256)]
        public async Task ValidJwe_DecryptsSuccessfully(string alg, string enc)
        {
            var handler = new JsonWebTokenHandler();

            var encryptingCredentials = new EncryptingCredentials(
                KeyingMaterial.RsaSecurityKey_2048, alg, enc);

            string jwe = handler.CreateToken(
                Default.PayloadString,
                Default.SymmetricSigningCredentials,
                encryptingCredentials);

            var tvp = Default.TokenValidationParameters(
                KeyingMaterial.RsaSecurityKey_2048,
                Default.SymmetricSigningKey256);
            tvp.ValidateLifetime = false;

            var result = await handler.ValidateTokenAsync(jwe, tvp);

            Assert.True(result.IsValid, $"Token validation should succeed. Exception: {result.Exception}");
        }

        /// <summary>
        /// A JWE whose encrypted-key segment has been altered must fail validation as a
        /// decryption failure, not as a key-wrap failure.
        /// </summary>
        [Fact]
        public async Task AlteredEncryptedKey_SurfacesAsDecryptionFailure()
        {
            var handler = new JsonWebTokenHandler();

            var encryptingCredentials = new EncryptingCredentials(
                KeyingMaterial.RsaSecurityKey_2048,
                SecurityAlgorithms.RsaOAEP,
                SecurityAlgorithms.Aes128CbcHmacSha256);

            string jwe = handler.CreateToken(
                Default.PayloadString,
                Default.SymmetricSigningCredentials,
                encryptingCredentials);

            string alteredJwe = AlterJweEncryptedKey(jwe, charactersToFlip: 2);

            var tvp = Default.TokenValidationParameters(
                KeyingMaterial.RsaSecurityKey_2048,
                Default.SymmetricSigningKey256);
            tvp.ValidateLifetime = false;

            var result = await handler.ValidateTokenAsync(alteredJwe, tvp);

            Assert.False(result.IsValid, "Token validation should fail when the encrypted key is altered.");
            Assert.NotNull(result.Exception);
            Assert.IsNotType<SecurityTokenKeyWrapException>(result.Exception);
            Assert.IsType<SecurityTokenDecryptionFailedException>(result.Exception);
        }

        /// <summary>
        /// A JWE decrypted with a different RSA key from the one used to encrypt it
        /// must fail validation as a decryption failure, not as a key-wrap failure.
        /// </summary>
        [Fact]
        public async Task MismatchedDecryptionKey_SurfacesAsDecryptionFailure()
        {
            var handler = new JsonWebTokenHandler();

            var encryptingCredentials = new EncryptingCredentials(
                KeyingMaterial.RsaSecurityKey_2048,
                SecurityAlgorithms.RsaOAEP,
                SecurityAlgorithms.Aes128CbcHmacSha256);

            string jwe = handler.CreateToken(
                Default.PayloadString,
                Default.SymmetricSigningCredentials,
                encryptingCredentials);

            // Different RSA key (also 2048-bit) so the failure is on key value, not size.
            var tvp = Default.TokenValidationParameters(
                KeyingMaterial.DefaultX509Key_2048,
                Default.SymmetricSigningKey256);
            tvp.ValidateLifetime = false;

            var result = await handler.ValidateTokenAsync(jwe, tvp);

            Assert.False(result.IsValid, "Token validation should fail when the decryption key does not match.");
            Assert.NotNull(result.Exception);
            Assert.IsNotType<SecurityTokenKeyWrapException>(result.Exception);
            Assert.IsType<SecurityTokenDecryptionFailedException>(result.Exception);
        }

        /// <summary>
        /// Two distinct invalid inputs (an altered encrypted key vs a non-matching
        /// decryption key) must surface to the caller through the same exception type.
        /// </summary>
        [Fact]
        public async Task DistinctInvalidInputs_ProduceSameExceptionType()
        {
            var handler = new JsonWebTokenHandler();

            var encryptingCredentials = new EncryptingCredentials(
                KeyingMaterial.RsaSecurityKey_2048,
                SecurityAlgorithms.RsaOAEP,
                SecurityAlgorithms.Aes256CbcHmacSha512);

            string jwe = handler.CreateToken(
                Default.PayloadString,
                Default.SymmetricSigningCredentials,
                encryptingCredentials);

            string alteredJwe = AlterJweEncryptedKey(jwe, charactersToFlip: 3);

            var tvpA = Default.TokenValidationParameters(
                KeyingMaterial.RsaSecurityKey_2048,
                Default.SymmetricSigningKey256);
            tvpA.ValidateLifetime = false;

            var tvpB = Default.TokenValidationParameters(
                KeyingMaterial.DefaultX509Key_2048,
                Default.SymmetricSigningKey256);
            tvpB.ValidateLifetime = false;

            var resultA = await handler.ValidateTokenAsync(alteredJwe, tvpA);
            var resultB = await handler.ValidateTokenAsync(jwe, tvpB);

            Assert.False(resultA.IsValid);
            Assert.False(resultB.IsValid);
            Assert.NotNull(resultA.Exception);
            Assert.NotNull(resultB.Exception);
            Assert.Equal(resultA.Exception.GetType(), resultB.Exception.GetType());
            Assert.IsType<SecurityTokenDecryptionFailedException>(resultA.Exception);
            Assert.IsType<SecurityTokenDecryptionFailedException>(resultB.Exception);
        }

        /// <summary>
        /// When several keys are tried and only one is correct, decryption must succeed.
        /// The placeholder key used for the non-matching attempt must not interfere.
        /// </summary>
        [Fact]
        public async Task MultipleKeys_OneMatches_DecryptsCorrectly()
        {
            var handler = new JsonWebTokenHandler();

            var encryptingCredentials = new EncryptingCredentials(
                KeyingMaterial.RsaSecurityKey_2048,
                SecurityAlgorithms.RsaOAEP,
                SecurityAlgorithms.Aes128CbcHmacSha256);

            string jwe = handler.CreateToken(
                Default.PayloadString,
                Default.SymmetricSigningCredentials,
                encryptingCredentials);

            var tvp = Default.TokenValidationParameters(
                KeyingMaterial.RsaSecurityKey_2048,
                Default.SymmetricSigningKey256);
            tvp.TokenDecryptionKeys = new SecurityKey[]
            {
                KeyingMaterial.DefaultX509Key_2048,
                KeyingMaterial.RsaSecurityKey_2048
            };
            tvp.ValidateLifetime = false;

            var result = await handler.ValidateTokenAsync(jwe, tvp);

            Assert.True(result.IsValid, $"Token validation should succeed when one of the supplied keys matches. Exception: {result.Exception}");
        }

        private static string AlterJweEncryptedKey(string jwe, int charactersToFlip)
        {
            string[] parts = jwe.Split('.');
            Assert.Equal(5, parts.Length);

            char[] altered = parts[1].ToCharArray();
            int count = Math.Min(charactersToFlip, altered.Length);
            for (int i = 0; i < count; i++)
                altered[i] = altered[i] == 'A' ? 'B' : 'A';

            parts[1] = new string(altered);
            return string.Join(".", parts);
        }
    }
}
