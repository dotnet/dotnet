// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using Xunit;

namespace Microsoft.IdentityModel.JsonWebTokens.Tests
{
    /// <summary>
    /// Tests covering how <see cref="JsonWebTokenHandler"/> handles the
    /// <see cref="ValidationParameters.DecryptionKeys"/> collection during token decryption.
    /// </summary>
    public class JsonWebTokenHandlerDecryptTokenDecryptionKeysTests
    {
        /// <summary>
        /// Verifies that repeated calls to GetContentEncryptionKeys with non-matching
        /// kid values do not grow the caller-supplied DecryptionKeys collection.
        /// </summary>
        [Fact]
        public void GetContentEncryptionKeys_RepeatedCalls_LeaveDecryptionKeysUnchanged()
        {
            var handler = new JsonWebTokenHandler();

            var encryptingCredentials = new EncryptingCredentials(
                KeyingMaterial.RsaSecurityKey_2048,
                SecurityAlgorithms.RsaPKCS1,
                SecurityAlgorithms.Aes128CbcHmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                EncryptingCredentials = encryptingCredentials,
                Claims = Default.PayloadDictionary
            };

            string jweString = handler.CreateToken(tokenDescriptor);
            var jwtToken = new JsonWebToken(jweString);

            var validationParameters = new ValidationParameters();
            var unmatchedKey = new SymmetricSecurityKey(KeyingMaterial.DefaultSymmetricKeyBytes_256)
            {
                KeyId = "UnmatchedKeyId"
            };
            validationParameters.DecryptionKeys.Add(unmatchedKey);

            var configuration = new CustomConfiguration(
                new SymmetricSecurityKey(KeyingMaterial.DefaultSymmetricKeyBytes_512)
                {
                    KeyId = "ConfigKey"
                });

            int originalDecryptionKeysCount = validationParameters.DecryptionKeys.Count;

            for (int i = 0; i < 10; i++)
            {
                handler.GetContentEncryptionKeys(jwtToken, validationParameters, configuration, null);
            }

            Assert.Equal(originalDecryptionKeysCount, validationParameters.DecryptionKeys.Count);
        }

        /// <summary>
        /// Exercises GetContentEncryptionKeys from multiple callers in parallel using a shared
        /// <see cref="ValidationParameters"/> instance and asserts that no enumeration or indexing
        /// failures surface and the collection is left unchanged.
        /// </summary>
        [Fact]
        public async Task GetContentEncryptionKeys_ParallelCallers_LeaveDecryptionKeysUnchanged()
        {
            var handler = new JsonWebTokenHandler();

            var encryptingCredentials = new EncryptingCredentials(
                KeyingMaterial.RsaSecurityKey_2048,
                SecurityAlgorithms.RsaPKCS1,
                SecurityAlgorithms.Aes128CbcHmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                EncryptingCredentials = encryptingCredentials,
                Claims = Default.PayloadDictionary
            };

            string jweString = handler.CreateToken(tokenDescriptor);
            var jwtToken = new JsonWebToken(jweString);

            var validationParameters = new ValidationParameters();
            var unmatchedKey = new SymmetricSecurityKey(KeyingMaterial.DefaultSymmetricKeyBytes_256)
            {
                KeyId = "UnmatchedKeyId"
            };
            validationParameters.DecryptionKeys.Add(unmatchedKey);

            var configuration = new CustomConfiguration(
                new SymmetricSecurityKey(KeyingMaterial.DefaultSymmetricKeyBytes_512)
                {
                    KeyId = "ConfigKey"
                });

            int originalCount = validationParameters.DecryptionKeys.Count;
            int callerCount = 20;
            int iterationsPerCaller = 50;
            var exceptions = new ConcurrentBag<Exception>();

            using var barrier = new ManualResetEventSlim(false);

            var tasks = Enumerable.Range(0, callerCount).Select(_ => Task.Run(() =>
            {
                barrier.Wait();
                for (int i = 0; i < iterationsPerCaller; i++)
                {
                    try
                    {
                        handler.GetContentEncryptionKeys(jwtToken, validationParameters, configuration, null);
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }
            })).ToArray();

            barrier.Set();
            await Task.WhenAll(tasks);

            Assert.Empty(exceptions);
            Assert.Equal(originalCount, validationParameters.DecryptionKeys.Count);
        }

        /// <summary>
        /// Verifies that JWE decryption still succeeds when the matching key is supplied
        /// via the configuration rather than directly on the ValidationParameters.
        /// </summary>
        [Fact]
        public void DecryptToken_WithConfigurationKey_StillDecrypts()
        {
            var handler = new JsonWebTokenHandler();

            var encryptingCredentials = new EncryptingCredentials(
                KeyingMaterial.RsaSecurityKey_2048,
                SecurityAlgorithms.RsaPKCS1,
                SecurityAlgorithms.Aes128CbcHmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                EncryptingCredentials = encryptingCredentials,
                Claims = Default.PayloadDictionary
            };

            string jweString = handler.CreateToken(tokenDescriptor);
            var jwtToken = new JsonWebToken(jweString);

            var validationParameters = new ValidationParameters();
            var configuration = new CustomConfiguration(KeyingMaterial.RsaSecurityKey_2048);

            var result = handler.DecryptToken(jwtToken, validationParameters, configuration, null);

            Assert.True(result.Succeeded, "Decryption should succeed when the matching key is available via configuration.");
            Assert.NotNull(result.Result);
            Assert.NotEmpty(result.Result);
        }

        /// <summary>
        /// Verifies that the <see cref="ValidationParameters"/> copy constructor produces
        /// collections that are independent of the source instance.
        /// </summary>
        [Fact]
        public void ValidationParameters_CopyConstructor_CollectionsAreIndependent()
        {
            var original = new ValidationParameters();
            var key1 = new SymmetricSecurityKey(KeyingMaterial.DefaultSymmetricKeyBytes_256) { KeyId = "Key1" };
            original.DecryptionKeys.Add(key1);
            original.SigningKeys.Add(key1);
            original.ValidIssuers.Add("issuer1");
            original.ValidAudiences.Add("audience1");
            original.ValidAlgorithms.Add("RS256");
            original.ValidTypes.Add("JWT");

            var copy = new TestableValidationParameters(original);
            var key2 = new SymmetricSecurityKey(KeyingMaterial.DefaultSymmetricKeyBytes_512) { KeyId = "Key2" };
            copy.DecryptionKeys.Add(key2);
            copy.SigningKeys.Add(key2);
            copy.ValidIssuers.Add("issuer2");
            copy.ValidAudiences.Add("audience2");
            copy.ValidAlgorithms.Add("ES256");
            copy.ValidTypes.Add("at+jwt");

            Assert.Single(original.DecryptionKeys);
            Assert.Single(original.SigningKeys);
            Assert.Single(original.ValidIssuers);
            Assert.Single(original.ValidAudiences);
            Assert.Single(original.ValidAlgorithms);
            Assert.Single(original.ValidTypes);

            Assert.Equal(2, copy.DecryptionKeys.Count);
            Assert.Equal(2, copy.SigningKeys.Count);
            Assert.Equal(2, copy.ValidIssuers.Count);
            Assert.Equal(2, copy.ValidAudiences.Count);
            Assert.Equal(2, copy.ValidAlgorithms.Count);
            Assert.Equal(2, copy.ValidTypes.Count);
        }

        private class TestableValidationParameters : ValidationParameters
        {
            public TestableValidationParameters(ValidationParameters other) : base(other) { }
        }
    }
}
