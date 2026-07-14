// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace Microsoft.IdentityModel.Dpop.Tests;

/// <summary>
/// End-to-end round-trip tests: mint access token with cnf.jkt → create proof → validate proof → verify binding.
/// </summary>
public class DpopE2ETests
{
    private static RSA CreateTestRsa()
    {
#if NET462
        return new RSACryptoServiceProvider(2048);
#else
        return RSA.Create(2048);
#endif
    }

    /// <summary>
    /// Creates a self-signed access token with a cnf.jkt claim bound to the given key.
    /// Uses a separate signing key for the AT (simulating the AS signing key).
    /// </summary>
    private static (string AccessToken, RSA ProofKey, RSA AtSigningKey) CreateAccessTokenWithCnfJkt(
        string audience = "api://test",
        string issuer = "https://test-issuer.example.com",
        RSA proofKeyOverride = null)
    {
        var proofKey = proofKeyOverride ?? CreateTestRsa();
        var atSigningKey = CreateTestRsa();

        // Compute the JWK thumbprint of the proof key
        var proofJwk = JsonWebKeyConverter.ConvertFromSecurityKey(new RsaSecurityKey(proofKey));
        var thumbprint = DpopProofValidator.ComputeJwkThumbprint(proofJwk);

        // Build the cnf claim
        var cnfJson = $"{{\"jkt\":\"{thumbprint}\"}}";

        // Parse cnf into JsonElement for proper embedding
        using var cnfDoc = System.Text.Json.JsonDocument.Parse(cnfJson);
        var cnfElement = cnfDoc.RootElement.Clone();

        var handler = new JsonWebTokenHandler();
        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = issuer,
            Audience = audience,
            Claims = new Dictionary<string, object>
            {
                { "sub", "test-user" },
                { "cnf", cnfElement },
            },
            SigningCredentials = new SigningCredentials(new RsaSecurityKey(atSigningKey), SecurityAlgorithms.RsaSha256),
        };

        var accessToken = handler.CreateToken(descriptor);
        return (accessToken, proofKey, atSigningKey);
    }

    [Fact]
    public async Task E2E_CreateProof_ValidateProof_ValidateBinding_Succeeds()
    {
        // 1. Mint an access token with cnf.jkt bound to our proof key
        var (accessToken, proofKey, _) = CreateAccessTokenWithCnfJkt();

        // 2. Create a DPoP proof using the same key
        var proofCreator = new DpopProofCreator(new DpopProofCreatorOptions
        {
            SigningCredentials = new SigningCredentials(new RsaSecurityKey(proofKey), SecurityAlgorithms.RsaSha256),
        });
        var method = "GET";
        var uri = new Uri("https://api.example.com/resource");
        var proofJwt = proofCreator.CreateProof(method, uri, accessToken);

        // 3. Extract cnf.jkt from AT
        var handler = new JsonWebTokenHandler();
        var at = handler.ReadJsonWebToken(accessToken);
        Assert.True(at.TryGetPayloadValue("cnf", out System.Text.Json.JsonElement cnf));
        var jkt = cnf.GetProperty("jkt").GetString();

        // 4. Validate the proof with binding
        var validator = new DpopProofValidator();
        var options = new DpopValidationOptions
        {
            AllowedSigningAlgorithms = new HashSet<string>(StringComparer.Ordinal) { "RS256" },
            MaxLifetimeInSeconds = 300,
            ClockSkewInSeconds = 300,
            ReplayProtectionHandledExternally = true,
        };

        var result = await validator.ValidateAsync(proofJwt, method, uri, accessToken, jkt, options);

        Assert.True(result.IsValid, $"Proof validation failed: {result.Error}");
    }

    [Fact]
    public async Task E2E_DifferentProofKey_BindingFails()
    {
        // AT bound to key A, proof signed by key B
        var (accessToken, _, _) = CreateAccessTokenWithCnfJkt();
        var differentKey = CreateTestRsa();

        var proofCreator = new DpopProofCreator(new DpopProofCreatorOptions
        {
            SigningCredentials = new SigningCredentials(new RsaSecurityKey(differentKey), SecurityAlgorithms.RsaSha256),
        });
        var method = "GET";
        var uri = new Uri("https://api.example.com/resource");
        var proofJwt = proofCreator.CreateProof(method, uri, accessToken);

        // Extract cnf.jkt from AT (bound to key A)
        var handler = new JsonWebTokenHandler();
        var at = handler.ReadJsonWebToken(accessToken);
        Assert.True(at.TryGetPayloadValue("cnf", out System.Text.Json.JsonElement cnf));
        var jkt = cnf.GetProperty("jkt").GetString();

        // Proof validates structurally but binding fails — different keys
        var validator = new DpopProofValidator();
        var options = new DpopValidationOptions
        {
            AllowedSigningAlgorithms = new HashSet<string>(StringComparer.Ordinal) { "RS256" },
            ReplayProtectionHandledExternally = true,
        };
        var result = await validator.ValidateAsync(proofJwt, method, uri, accessToken, jkt, options);

        Assert.False(result.IsValid);
        Assert.Contains("cnf.jkt", result.Error.Message);
    }

    [Fact]
    public async Task E2E_WrongAccessToken_AthMismatch()
    {
        // Create proof bound to token A, validate with token B
        var (tokenA, proofKey, _) = CreateAccessTokenWithCnfJkt();
        var (tokenB, _, _) = CreateAccessTokenWithCnfJkt(proofKeyOverride: proofKey);

        var proofCreator = new DpopProofCreator(new DpopProofCreatorOptions
        {
            SigningCredentials = new SigningCredentials(new RsaSecurityKey(proofKey), SecurityAlgorithms.RsaSha256),
        });
        var proofJwt = proofCreator.CreateProof("GET", new Uri("https://api.example.com/resource"), tokenA);

        // Extract cnf.jkt (same key, so jkt is the same for both tokens)
        var handler = new JsonWebTokenHandler();
        var at = handler.ReadJsonWebToken(tokenB);
        Assert.True(at.TryGetPayloadValue("cnf", out System.Text.Json.JsonElement cnf));
        var jkt = cnf.GetProperty("jkt").GetString();

        // Validate proof against token B — ath won't match
        var validator = new DpopProofValidator();
        var options = new DpopValidationOptions
        {
            AllowedSigningAlgorithms = new HashSet<string>(StringComparer.Ordinal) { "RS256" },
            ReplayProtectionHandledExternally = true,
        };
        var result = await validator.ValidateAsync(
            proofJwt, "GET", new Uri("https://api.example.com/resource"), tokenB, jkt, options);

        Assert.False(result.IsValid);
        Assert.Contains("ath", result.Error.Message);
    }

    [Fact]
    public async Task E2E_WithNonce_RoundTrip_Succeeds()
    {
        var (accessToken, proofKey, _) = CreateAccessTokenWithCnfJkt();
        var nonce = "server-provided-nonce-abc123";

        var proofCreator = new DpopProofCreator(new DpopProofCreatorOptions
        {
            SigningCredentials = new SigningCredentials(new RsaSecurityKey(proofKey), SecurityAlgorithms.RsaSha256),
            IncludeNonce = true,
            Nonce = nonce,
        });
        var proofJwt = proofCreator.CreateProof("POST", new Uri("https://api.example.com/data"), accessToken);

        // Extract cnf.jkt
        var handler = new JsonWebTokenHandler();
        var at = handler.ReadJsonWebToken(accessToken);
        Assert.True(at.TryGetPayloadValue("cnf", out System.Text.Json.JsonElement cnf));
        var jkt = cnf.GetProperty("jkt").GetString();

        var validator = new DpopProofValidator();
        var options = new DpopValidationOptions
        {
            AllowedSigningAlgorithms = new HashSet<string>(StringComparer.Ordinal) { "RS256" },
            ExpectedNonce = nonce,
        };
        var result = await validator.ValidateAsync(
            proofJwt, "POST", new Uri("https://api.example.com/data"), accessToken, jkt, options);

        Assert.True(result.IsValid, $"Proof validation with nonce failed: {result.Error}");
    }

    [Fact]
    public async Task E2E_NonceMismatch_ReturnsNonceRequired()
    {
        var (accessToken, proofKey, _) = CreateAccessTokenWithCnfJkt();

        var proofCreator = new DpopProofCreator(new DpopProofCreatorOptions
        {
            SigningCredentials = new SigningCredentials(new RsaSecurityKey(proofKey), SecurityAlgorithms.RsaSha256),
            IncludeNonce = true,
            Nonce = "old-nonce",
        });
        var proofJwt = proofCreator.CreateProof("GET", new Uri("https://api.example.com/resource"), accessToken);

        // Extract cnf.jkt
        var handler = new JsonWebTokenHandler();
        var at = handler.ReadJsonWebToken(accessToken);
        Assert.True(at.TryGetPayloadValue("cnf", out System.Text.Json.JsonElement cnf));
        var jkt = cnf.GetProperty("jkt").GetString();

        var validator = new DpopProofValidator();
        var options = new DpopValidationOptions
        {
            AllowedSigningAlgorithms = new HashSet<string>(StringComparer.Ordinal) { "RS256" },
            ExpectedNonce = "current-server-nonce",
        };
        var result = await validator.ValidateAsync(
            proofJwt, "GET", new Uri("https://api.example.com/resource"), accessToken, jkt, options);

        Assert.False(result.IsValid);
        Assert.True(result.IsNonceRequired);
    }

    [Fact]
    public async Task E2E_JwkWithMismatchedX5cAndBareKey_BindingFails()
    {
        var (accessToken, originalProofKey, _) = CreateAccessTokenWithCnfJkt();
        var nonce = "server-provided-nonce-xyz789";
        var method = "POST";
        var uri = new Uri("https://api.example.com/data");

        var handler = new JsonWebTokenHandler();
        var at = handler.ReadJsonWebToken(accessToken);
        Assert.True(at.TryGetPayloadValue("cnf", out System.Text.Json.JsonElement cnf));
        var jkt = cnf.GetProperty("jkt").GetString();

        var validator = new DpopProofValidator();
        var options = new DpopValidationOptions
        {
            AllowedSigningAlgorithms = new HashSet<string>(StringComparer.Ordinal) { "RS256" },
            ExpectedNonce = nonce,
        };

        var otherKey = CreateTestRsa();
        var x5cValue = Convert.ToBase64String(new byte[] { 0x30, 0x82, 0x01, 0x00 });

        byte[] athHash;
        using (var sha = SHA256.Create())
            athHash = sha.ComputeHash(Encoding.ASCII.GetBytes(accessToken));
        var athValue = Base64UrlEncoder.Encode(athHash);

        var originalJwk = JsonWebKeyConverter.ConvertFromSecurityKey(new RsaSecurityKey(originalProofKey));
        var htu = uri.GetLeftPart(UriPartial.Path);
        var iat = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var jti = Guid.NewGuid().ToString();

        var headerJson =
            "{\"typ\":\"dpop+jwt\",\"alg\":\"RS256\",\"jwk\":{" +
            "\"kty\":\"" + originalJwk.Kty + "\"," +
            "\"e\":\"" + originalJwk.E + "\"," +
            "\"n\":\"" + originalJwk.N + "\"," +
            "\"x5c\":[\"" + x5cValue + "\"]" +
            "}}";

        var payloadJson =
            "{\"htm\":\"" + method + "\"," +
            "\"htu\":\"" + htu + "\"," +
            "\"iat\":" + iat + "," +
            "\"jti\":\"" + jti + "\"," +
            "\"ath\":\"" + athValue + "\"," +
            "\"nonce\":\"" + nonce + "\"}";

        var encodedHeader = Base64UrlEncoder.Encode(Encoding.UTF8.GetBytes(headerJson));
        var encodedPayload = Base64UrlEncoder.Encode(Encoding.UTF8.GetBytes(payloadJson));
        var signingInput = encodedHeader + "." + encodedPayload;
        var signature = otherKey.SignData(
            Encoding.ASCII.GetBytes(signingInput),
            HashAlgorithmName.SHA256,
            RSASignaturePadding.Pkcs1);
        var mismatchedProofJwt = signingInput + "." + Base64UrlEncoder.Encode(signature);

        var mismatchedResult = await validator.ValidateAsync(
            mismatchedProofJwt, method, uri, accessToken, jkt, options);

        Assert.False(
            mismatchedResult.IsValid,
            $"Proof should be rejected. Error was: {mismatchedResult.Error?.Message ?? "<none>"}");

        var proofCreator = new DpopProofCreator(new DpopProofCreatorOptions
        {
            SigningCredentials = new SigningCredentials(new RsaSecurityKey(originalProofKey), SecurityAlgorithms.RsaSha256),
            IncludeNonce = true,
            Nonce = nonce,
        });
        var wellFormedProofJwt = proofCreator.CreateProof(method, uri, accessToken);
        var wellFormedResult = await validator.ValidateAsync(
            wellFormedProofJwt, method, uri, accessToken, jkt, options);
        Assert.True(wellFormedResult.IsValid, $"Well-formed proof should validate: {wellFormedResult.Error?.Message}");
    }

    [Fact]
    public async Task E2E_ValidateProof_DoesNotCacheSignatureProvider()
    {
        var (accessToken, proofKey, _) = CreateAccessTokenWithCnfJkt();
        var nonce = "nonce-cache-single";
        var method = "GET";
        var uri = new Uri("https://api.example.com/cache-single");

        var handler = new JsonWebTokenHandler();
        var at = handler.ReadJsonWebToken(accessToken);
        Assert.True(at.TryGetPayloadValue("cnf", out System.Text.Json.JsonElement cnf));
        var jkt = cnf.GetProperty("jkt").GetString();

        var options = new DpopValidationOptions
        {
            AllowedSigningAlgorithms = new HashSet<string>(StringComparer.Ordinal) { "RS256" },
            ExpectedNonce = nonce,
        };

        var proofCreator = new DpopProofCreator(new DpopProofCreatorOptions
        {
            SigningCredentials = new SigningCredentials(new RsaSecurityKey(proofKey), SecurityAlgorithms.RsaSha256),
            IncludeNonce = true,
            Nonce = nonce,
        });
        var proofJwt = proofCreator.CreateProof(method, uri, accessToken);

        var probeKey = new RsaSecurityKey(proofKey);
        var providerTypeName = typeof(AsymmetricSignatureProvider).ToString();
        Assert.False(
            CryptoProviderFactory.Default.CryptoProviderCache.TryGetSignatureProvider(
                probeKey, SecurityAlgorithms.RsaSha256, providerTypeName, willCreateSignatures: false, out _),
            "Pre-validation: the verifying SignatureProvider should not already be cached.");

        var validator = new DpopProofValidator();
        var result = await validator.ValidateAsync(proofJwt, method, uri, accessToken, jkt, options);
        Assert.True(result.IsValid, $"Proof should validate: {result.Error}");

        Assert.False(
            CryptoProviderFactory.Default.CryptoProviderCache.TryGetSignatureProvider(
                probeKey, SecurityAlgorithms.RsaSha256, providerTypeName, willCreateSignatures: false, out _),
            "Post-validation: validating a DPoP proof must not add the verifying SignatureProvider to the cache.");
    }

    [Fact]
    public async Task E2E_ValidateManyProofs_DoesNotPopulateCacheWithEphemeralKeys()
    {
        const int proofCount = 25;
        var validator = new DpopProofValidator();
        var providerTypeName = typeof(AsymmetricSignatureProvider).ToString();
        var proofKeys = new List<RSA>();

        try
        {
            for (int i = 0; i < proofCount; i++)
            {
                var (accessToken, proofKey, _) = CreateAccessTokenWithCnfJkt();
                proofKeys.Add(proofKey);

                var nonce = "nonce-cache-many-" + i;
                var method = "POST";
                var uri = new Uri("https://api.example.com/cache-many-" + i);

                var handler = new JsonWebTokenHandler();
                var at = handler.ReadJsonWebToken(accessToken);
                Assert.True(at.TryGetPayloadValue("cnf", out System.Text.Json.JsonElement cnf));
                var jkt = cnf.GetProperty("jkt").GetString();

                var options = new DpopValidationOptions
                {
                    AllowedSigningAlgorithms = new HashSet<string>(StringComparer.Ordinal) { "RS256" },
                    ExpectedNonce = nonce,
                };

                var proofCreator = new DpopProofCreator(new DpopProofCreatorOptions
                {
                    SigningCredentials = new SigningCredentials(new RsaSecurityKey(proofKey), SecurityAlgorithms.RsaSha256),
                    IncludeNonce = true,
                    Nonce = nonce,
                });
                var proofJwt = proofCreator.CreateProof(method, uri, accessToken);

                var result = await validator.ValidateAsync(proofJwt, method, uri, accessToken, jkt, options);
                Assert.True(result.IsValid, $"Proof #{i} should validate: {result.Error}");
            }

            int cachedCount = 0;
            foreach (var key in proofKeys)
            {
                var probeKey = new RsaSecurityKey(key);
                if (CryptoProviderFactory.Default.CryptoProviderCache.TryGetSignatureProvider(
                        probeKey, SecurityAlgorithms.RsaSha256, providerTypeName,
                        willCreateSignatures: false, out _))
                {
                    cachedCount++;
                }
            }

            Assert.Equal(0, cachedCount);
        }
        finally
        {
            foreach (var key in proofKeys)
                key.Dispose();
        }
    }
}
