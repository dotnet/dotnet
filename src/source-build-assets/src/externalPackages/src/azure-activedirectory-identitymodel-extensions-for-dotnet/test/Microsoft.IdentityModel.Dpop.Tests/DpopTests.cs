// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace Microsoft.IdentityModel.Dpop.Tests
{
    public class DpopTests
    {
        private static RSA CreateTestRsa()
        {
#if NET462
            return new RSACryptoServiceProvider(2048);
#else
            return RSA.Create(2048);
#endif
        }

        private static (string AccessToken, string CnfJkt) CreateTestAccessToken(RSA proofKey)
        {
            var atSigningKey = CreateTestRsa();
            var handler = new Microsoft.IdentityModel.JsonWebTokens.JsonWebTokenHandler();

            var jwk = JsonWebKeyConverter.ConvertFromSecurityKey(new RsaSecurityKey(proofKey));
            var thumbprint = DpopProofValidator.ComputeJwkThumbprint(jwk);
            var cnfJson = $"{{\"jkt\":\"{thumbprint}\"}}";

            using var doc = System.Text.Json.JsonDocument.Parse(cnfJson);
            var claims = new Dictionary<string, object>
            {
                { "sub", "test" },
                { "cnf", doc.RootElement.Clone() },
            };

            var accessToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = "https://test-issuer.example.com",
                Audience = "api://test",
                Claims = claims,
                SigningCredentials = new SigningCredentials(new RsaSecurityKey(atSigningKey), SecurityAlgorithms.RsaSha256),
            });

            return (accessToken, thumbprint);
        }

        [Fact]
        public async Task CreateAndValidateProof_Success()
        {
            // Arrange
            var rsa = CreateTestRsa();
            var rsaKey = new RsaSecurityKey(rsa);
            var signingCredentials = new SigningCredentials(rsaKey, SecurityAlgorithms.RsaSha256);
            var (accessToken, cnfJkt) = CreateTestAccessToken(rsa);

            var dpopProof = new DpopProofCreator(new DpopProofCreatorOptions { SigningCredentials = signingCredentials });
            var httpMethod = "POST";
            var uri = new Uri("https://example.com/token");

            // Act
            var proof = dpopProof.CreateProof(httpMethod, uri, accessToken);
            var validator = new DpopProofValidator();
            var result = await validator.ValidateAsync(proof, httpMethod, uri, accessToken, cnfJkt,
                new DpopValidationOptions
                {
                    AllowedSigningAlgorithms = new HashSet<string> { "RS256" },
                    ReplayProtectionHandledExternally = true,
                });

            // Assert
            Assert.NotNull(proof);
            Assert.True(result.IsValid);
            Assert.Null(result.Error);
        }

        [Fact]
        public async Task ValidateProof_InvalidHttpMethod_Fails()
        {
            // Arrange
            var rsa = CreateTestRsa();
            var rsaKey = new RsaSecurityKey(rsa);
            var signingCredentials = new SigningCredentials(rsaKey, SecurityAlgorithms.RsaSha256);
            var (accessToken, cnfJkt) = CreateTestAccessToken(rsa);

            var dpopProof = new DpopProofCreator(new DpopProofCreatorOptions { SigningCredentials = signingCredentials });
            var uri = new Uri("https://example.com/token");

            // Act
            var proof = dpopProof.CreateProof("POST", uri, accessToken);
            var validator = new DpopProofValidator();
            var result = await validator.ValidateAsync(proof, "GET", uri, accessToken, cnfJkt,
                new DpopValidationOptions
                {
                    AllowedSigningAlgorithms = new HashSet<string> { "RS256" },
                    ReplayProtectionHandledExternally = true,
                });

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("htm", result.Error.Message);
        }

        [Fact]
        public async Task ValidateProof_InvalidUri_Fails()
        {
            // Arrange
            var rsa = CreateTestRsa();
            var rsaKey = new RsaSecurityKey(rsa);
            var signingCredentials = new SigningCredentials(rsaKey, SecurityAlgorithms.RsaSha256);
            var (accessToken, cnfJkt) = CreateTestAccessToken(rsa);

            var dpopProof = new DpopProofCreator(new DpopProofCreatorOptions { SigningCredentials = signingCredentials });
            var uri = new Uri("https://example.com/token");

            // Act
            var proof = dpopProof.CreateProof("POST", uri, accessToken);
            var validator = new DpopProofValidator();
            var result = await validator.ValidateAsync(proof, "POST", new Uri("https://example.com/resource"), accessToken, cnfJkt,
                new DpopValidationOptions
                {
                    AllowedSigningAlgorithms = new HashSet<string> { "RS256" },
                    ReplayProtectionHandledExternally = true,
                });

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("htu", result.Error.Message);
        }

        [Fact]
        public async Task ValidateProof_WithNonce_Success()
        {
            // Arrange
            var rsa = CreateTestRsa();
            var rsaKey = new RsaSecurityKey(rsa);
            var signingCredentials = new SigningCredentials(rsaKey, SecurityAlgorithms.RsaSha256);
            var (accessToken, cnfJkt) = CreateTestAccessToken(rsa);

            var dpopProof = new DpopProofCreator(new DpopProofCreatorOptions
            {
                SigningCredentials = signingCredentials,
                IncludeNonce = true,
                Nonce = "test-nonce-123"
            });
            var uri = new Uri("https://example.com/token");

            // Act
            var proof = dpopProof.CreateProof("POST", uri, accessToken);
            var validator = new DpopProofValidator();
            var result = await validator.ValidateAsync(proof, "POST", uri, accessToken, cnfJkt,
                new DpopValidationOptions
                {
                    AllowedSigningAlgorithms = new HashSet<string> { "RS256" },
                    ExpectedNonce = "test-nonce-123",
                });

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void CreateProofAndSetHeaders_Success()
        {
            // Arrange
            var rsaKey = new RsaSecurityKey(CreateTestRsa());
            var signingCredentials = new SigningCredentials(rsaKey, SecurityAlgorithms.RsaSha256);

            var options = new DpopProofCreatorOptions
            {
                SigningCredentials = signingCredentials
            };

            var dpopProof = new DpopProofCreator(options);
            var request = new HttpRequestMessage(HttpMethod.Post, "https://example.com/token");

            // Act — use CreateProof directly, set headers manually
            string proof = dpopProof.CreateProof(request.Method.Method, request.RequestUri, "access_token");
            request.Headers.Add(DpopConstants.DpopTokenType, proof);

            // Assert
            Assert.True(request.Headers.Contains(DpopConstants.DpopTokenType));
            var dpopHeader = request.Headers.GetValues(DpopConstants.DpopTokenType).FirstOrDefault();
            Assert.NotNull(dpopHeader);
        }

        [Fact]
        public void CreateProofWithBoundAccessToken_Success()
        {
            // Arrange
            var rsaKey = new RsaSecurityKey(CreateTestRsa());
            var signingCredentials = new SigningCredentials(rsaKey, SecurityAlgorithms.RsaSha256);

            var options = new DpopProofCreatorOptions
            {
                SigningCredentials = signingCredentials
            };

            var dpopProof = new DpopProofCreator(options);
            var request = new HttpRequestMessage(HttpMethod.Post, "https://example.com/resource");
            var accessToken = "test-access-token-123";

            // Act — use CreateProof directly, set headers manually
            string proof = dpopProof.CreateProof(request.Method.Method, request.RequestUri, accessToken);
            request.Headers.Add(DpopConstants.DpopTokenType, proof);
            request.Headers.Authorization = new AuthenticationHeaderValue(DpopConstants.DpopTokenType, accessToken);

            // Assert
            Assert.True(request.Headers.Contains(DpopConstants.DpopTokenType));
            Assert.NotNull(request.Headers.Authorization);
            Assert.Equal(DpopConstants.DpopTokenType, request.Headers.Authorization.Scheme);
            Assert.Equal(accessToken, request.Headers.Authorization.Parameter);
        }
    }
}
