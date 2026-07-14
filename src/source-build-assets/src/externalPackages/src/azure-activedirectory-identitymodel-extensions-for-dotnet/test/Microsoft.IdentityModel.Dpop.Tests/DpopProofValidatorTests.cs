// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace Microsoft.IdentityModel.Dpop.Tests
{
    public class DpopProofValidatorTests
    {
        private readonly DpopProofValidator _validator = new();

        private static RSA CreateTestRsa()
        {
#if NET462
            return new RSACryptoServiceProvider(2048);
#else
            return RSA.Create(2048);
#endif
        }

        #region Test Helpers

#if !NET462 // EC helpers for EC-specific algorithm tests only
        private static (string ProofJwt, ECDsa Key) CreateValidEcProof(
            string httpMethod = "GET",
            string uri = "https://resource.example.org/api",
            string accessToken = null,
            string nonce = DefaultTestNonce)
        {
            var ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256);
            var signingCredentials = new SigningCredentials(
                new ECDsaSecurityKey(ecdsa), SecurityAlgorithms.EcdsaSha256);

            var proofOptions = new DpopProofCreatorOptions
            {
                SigningCredentials = signingCredentials,
                IncludeNonce = !string.IsNullOrEmpty(nonce),
                Nonce = nonce,
            };
            var dpopProof = new DpopProofCreator(proofOptions);
            var jwt = dpopProof.CreateProof(httpMethod, new Uri(uri), accessToken);
            return (jwt, ecdsa);
        }
#endif

        /// <summary>
        /// Creates a valid DPoP proof using RSA (works on all TFMs including net462).
        /// For tests that don't need to tamper with individual claims.
        /// </summary>
        private static (string ProofJwt, RSA Key) CreateValidRsaProof(
            string httpMethod = "GET",
            string uri = "https://resource.example.org/api",
            string accessToken = null,
            string nonce = DefaultTestNonce,
            RSA proofKey = null)
        {
            var rsa = proofKey ?? CreateTestRsa();
            var signingCredentials = new SigningCredentials(
                new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);

            var proofOptions = new DpopProofCreatorOptions
            {
                SigningCredentials = signingCredentials,
                IncludeNonce = !string.IsNullOrEmpty(nonce),
                Nonce = nonce,
            };
            var dpopProof = new DpopProofCreator(proofOptions);
            var jwt = dpopProof.CreateProof(httpMethod, new Uri(uri), accessToken);
            return (jwt, rsa);
        }

        /// <summary>
        /// Creates a tampered DPoP proof using RSA with specific claims omitted.
        /// Works on all TFMs including net462.
        /// </summary>
        private static (string ProofJwt, RSA Key) CreateTamperedRsaProof(
            string httpMethod = "GET",
            string uri = "https://resource.example.org/api",
            string accessToken = null,
            string nonce = DefaultTestNonce,
            string typ = "dpop+jwt",
            long? iatOverride = null,
            bool omitJti = false,
            bool omitHtm = false,
            bool omitHtu = false,
            bool omitIat = false,
            bool omitAth = false,
            bool includePrivateKey = false,
            RSA proofKey = null)
        {
            var rsa = proofKey ?? CreateTestRsa();
            var signingCredentials = new SigningCredentials(
                new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);

            var now = DateTimeOffset.UtcNow;
            var claims = new Dictionary<string, object>();

            if (!omitHtm)
                claims["htm"] = httpMethod.ToUpperInvariant();
            if (!omitHtu)
                claims["htu"] = new Uri(uri).GetLeftPart(UriPartial.Path);
            if (!omitIat)
                claims["iat"] = iatOverride ?? now.ToUnixTimeSeconds();
            if (!omitJti)
                claims["jti"] = Guid.NewGuid().ToString();

            if (!string.IsNullOrEmpty(accessToken) && !omitAth)
            {
#if NET6_0_OR_GREATER
                var hash = SHA256.HashData(Encoding.ASCII.GetBytes(accessToken));
#else
                byte[] hash;
                using (var sha = SHA256.Create())
                {
                    hash = sha.ComputeHash(Encoding.ASCII.GetBytes(accessToken));
                }
#endif
                claims["ath"] = Base64UrlEncoder.Encode(hash);
            }

            if (!string.IsNullOrEmpty(nonce))
                claims["nonce"] = nonce;

            // Build RSA JWK for header
            var rsaParams = rsa.ExportParameters(includePrivateKey);
            var jwkForHeader = new System.Text.Json.Nodes.JsonObject
            {
                ["e"] = Base64UrlEncoder.Encode(rsaParams.Exponent),
                ["kty"] = "RSA",
                ["n"] = Base64UrlEncoder.Encode(rsaParams.Modulus),
            };
            if (includePrivateKey)
            {
                jwkForHeader["d"] = Base64UrlEncoder.Encode(rsaParams.D);
                jwkForHeader["p"] = Base64UrlEncoder.Encode(rsaParams.P);
                jwkForHeader["q"] = Base64UrlEncoder.Encode(rsaParams.Q);
            }

            var headerClaims = new Dictionary<string, object>
            {
                { "typ", typ },
                { "jwk", jwkForHeader }
            };

            var descriptor = new SecurityTokenDescriptor
            {
                IncludeKeyIdInHeader = false,
                Claims = claims,
                AdditionalHeaderClaims = headerClaims,
                SigningCredentials = signingCredentials,
            };

            var handler = new JsonWebTokenHandler { SetDefaultTimesOnTokenCreation = false };
            var jwt = handler.CreateToken(descriptor);
            return (jwt, rsa);
        }

        private const string DefaultTestNonce = "test-server-nonce";

        private static DpopValidationOptions DefaultOptions() => new()
        {
            AllowedSigningAlgorithms = new HashSet<string>(StringComparer.Ordinal) { "ES256", "RS256" },
            MaxLifetimeInSeconds = 300,
            ClockSkewInSeconds = 300,
            ExpectedNonce = DefaultTestNonce,
        };

        /// <summary>
        /// Creates a simple access token with a cnf.jkt claim bound to the given proof key.
        /// </summary>
        private static (string AccessToken, string CnfJkt) CreateSimpleAccessToken(RSA proofKey)
        {
            var atSigningKey = CreateTestRsa();
            var handler = new JsonWebTokenHandler();

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

        /// <summary>
        /// Creates a valid DPoP proof and a matching access token with cnf.jkt binding.
        /// </summary>
        private static (string ProofJwt, string AccessToken, string CnfJkt) CreateProofAndAccessToken(
            string httpMethod = "GET",
            string uri = "https://resource.example.org/api",
            string nonce = DefaultTestNonce)
        {
            var rsa = CreateTestRsa();
            var (at, cnfJkt) = CreateSimpleAccessToken(rsa);
            var (proofJwt, _) = CreateValidRsaProof(httpMethod, uri, accessToken: at, nonce: nonce, proofKey: rsa);
            return (proofJwt, at, cnfJkt);
        }

        /// <summary>
        /// Creates a tampered DPoP proof and a matching access token with cnf.jkt binding.
        /// </summary>
        private static (string ProofJwt, string AccessToken, string CnfJkt) CreateTamperedProofAndAccessToken(
            string httpMethod = "GET",
            string uri = "https://resource.example.org/api",
            string nonce = DefaultTestNonce,
            string typ = "dpop+jwt",
            long? iatOverride = null,
            bool omitJti = false,
            bool omitHtm = false,
            bool omitHtu = false,
            bool omitIat = false,
            bool omitAth = false,
            bool includePrivateKey = false)
        {
            var rsa = CreateTestRsa();
            var (at, cnfJkt) = CreateSimpleAccessToken(rsa);
            var (proofJwt, _) = CreateTamperedRsaProof(
                httpMethod, uri, accessToken: at, nonce: nonce, typ: typ,
                iatOverride: iatOverride, omitJti: omitJti, omitHtm: omitHtm,
                omitHtu: omitHtu, omitIat: omitIat, omitAth: omitAth,
                includePrivateKey: includePrivateKey, proofKey: rsa);
            return (proofJwt, at, cnfJkt);
        }

        #endregion

        #region Happy Path

        [Fact]
        public async Task ValidateAsync_ValidProof_Succeeds()
        {
            var (proof, accessToken, cnfJkt) = CreateProofAndAccessToken();
            var options = DefaultOptions();

            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);

            Assert.True(result.IsValid);
            Assert.Null(result.Error?.Exception);
            Assert.False(result.IsNonceRequired);
        }

        [Fact]
        public async Task ValidateAsync_ValidProofWithAccessToken_Succeeds()
        {
            var (proof, accessToken, cnfJkt) = CreateProofAndAccessToken();
            var options = DefaultOptions();

            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);

            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task ValidateAsync_ValidProofWithNonce_Succeeds()
        {
            var (proof, accessToken, cnfJkt) = CreateProofAndAccessToken(nonce: "server-nonce-42");
            var options = DefaultOptions();
            options.ExpectedNonce = "server-nonce-42";

            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);

            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task ValidateAsync_PostMethod_Succeeds()
        {
            var (proof, accessToken, cnfJkt) = CreateProofAndAccessToken(httpMethod: "POST");
            var options = DefaultOptions();

            var result = await _validator.ValidateAsync(
                proof, "POST", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);

            Assert.True(result.IsValid);
        }

        #endregion

        #region Typ Validation

        [Fact]
        public async Task ValidateAsync_WrongTyp_Fails()
        {
            var (proof, accessToken, cnfJkt) = CreateTamperedProofAndAccessToken(typ: "jwt");
            var options = DefaultOptions();

            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);

            Assert.False(result.IsValid);
            Assert.Contains("typ", result.Error.Message);
        }

        #endregion

        #region Algorithm Validation

        [Fact]
        public async Task ValidateAsync_AlgorithmNotInAllowedSet_Fails()
        {
            var (proof, accessToken, cnfJkt) = CreateProofAndAccessToken();
            var options = DefaultOptions();
            options.AllowedSigningAlgorithms = new HashSet<string>(StringComparer.Ordinal) { "PS256" };

            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);

            Assert.False(result.IsValid);
            Assert.Contains("not in the allowed set", result.Error.Message);
        }

        [Fact]
        public async Task ValidateAsync_AlgNone_Fails()
        {
            var header = Base64UrlEncoder.Encode(Encoding.UTF8.GetBytes("{\"typ\":\"dpop+jwt\",\"alg\":\"none\",\"jwk\":{\"kty\":\"RSA\",\"e\":\"AQAB\",\"n\":\"test\"}}"));
            var payload = Base64UrlEncoder.Encode(Encoding.UTF8.GetBytes("{\"htm\":\"GET\",\"htu\":\"https://example.com\",\"iat\":1704063600,\"jti\":\"test\"}"));
            var fakeProof = $"{header}.{payload}.";

            var rsa = CreateTestRsa();
            var (accessToken, cnfJkt) = CreateSimpleAccessToken(rsa);

            var result = await _validator.ValidateAsync(
                fakeProof, "GET", new Uri("https://example.com"), accessToken, cnfJkt, DefaultOptions());

            Assert.False(result.IsValid);
            Assert.Contains("none", result.Error.Message);
        }

        [Fact]
        public async Task ValidateAsync_SymmetricAlgorithm_Fails()
        {
            var header = Base64UrlEncoder.Encode(Encoding.UTF8.GetBytes("{\"typ\":\"dpop+jwt\",\"alg\":\"HS256\",\"jwk\":{\"kty\":\"RSA\",\"e\":\"AQAB\",\"n\":\"test\"}}"));
            var payload = Base64UrlEncoder.Encode(Encoding.UTF8.GetBytes("{\"htm\":\"GET\",\"htu\":\"https://example.com\",\"iat\":1704063600,\"jti\":\"test\"}"));
            var fakeProof = $"{header}.{payload}.fakesig";

            var rsa = CreateTestRsa();
            var (accessToken, cnfJkt) = CreateSimpleAccessToken(rsa);

            var result = await _validator.ValidateAsync(
                fakeProof, "GET", new Uri("https://example.com"), accessToken, cnfJkt, DefaultOptions());

            Assert.False(result.IsValid);
            Assert.Contains("asymmetric", result.Error.Message);
        }

        [Fact]
        public async Task ValidateAsync_EmptyAlg_Fails()
        {
            var header = Base64UrlEncoder.Encode(Encoding.UTF8.GetBytes("{\"typ\":\"dpop+jwt\",\"alg\":\"\",\"jwk\":{\"kty\":\"RSA\",\"e\":\"AQAB\",\"n\":\"test\"}}"));
            var payload = Base64UrlEncoder.Encode(Encoding.UTF8.GetBytes("{\"htm\":\"GET\",\"htu\":\"https://example.com\",\"iat\":" + DateTimeOffset.UtcNow.ToUnixTimeSeconds() + ",\"jti\":\"test\"}"));
            var fakeProof = $"{header}.{payload}.fakesig";

            var rsa = CreateTestRsa();
            var (accessToken, cnfJkt) = CreateSimpleAccessToken(rsa);

            var result = await _validator.ValidateAsync(
                fakeProof, "GET", new Uri("https://example.com"), accessToken, cnfJkt, DefaultOptions());

            Assert.False(result.IsValid);
            Assert.Contains("empty", result.Error.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task ValidateAsync_NullAllowedAlgorithms_Fails()
        {
            var (proof, accessToken, cnfJkt) = CreateProofAndAccessToken();
            var options = DefaultOptions();
            options.AllowedSigningAlgorithms = null;

            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);

            Assert.False(result.IsValid);
            Assert.Contains("allowed algorithm", result.Error.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task ValidateAsync_EmptyAllowedAlgorithms_Fails()
        {
            var (proof, accessToken, cnfJkt) = CreateProofAndAccessToken();
            var options = DefaultOptions();
            options.AllowedSigningAlgorithms = new HashSet<string>();

            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);

            Assert.False(result.IsValid);
            Assert.Contains("allowed algorithm", result.Error.Message, StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        #region JWK Validation

        [Fact]
        public async Task ValidateAsync_MissingJwk_Fails()
        {
            var rsa = CreateTestRsa();
            var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);

            var handler = new JsonWebTokenHandler { SetDefaultTimesOnTokenCreation = false };
            var jwt = handler.CreateToken(new SecurityTokenDescriptor
            {
                IncludeKeyIdInHeader = false,
                Claims = new Dictionary<string, object>
                {
                    { "htm", "GET" },
                    { "htu", "https://example.com" },
                    { "iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds() },
                    { "jti", Guid.NewGuid().ToString() },
                },
                AdditionalHeaderClaims = new Dictionary<string, object>
                {
                    { "typ", "dpop+jwt" },
                },
                SigningCredentials = signingCredentials,
            });

            var (accessToken, cnfJkt) = CreateSimpleAccessToken(rsa);

            var result = await _validator.ValidateAsync(
                jwt, "GET", new Uri("https://example.com"), accessToken, cnfJkt, DefaultOptions());

            Assert.False(result.IsValid);
            Assert.Contains("jwk", result.Error.Message);
        }

        #endregion

        #region Htm Validation

        [Fact]
        public async Task ValidateAsync_HtmMismatch_Fails()
        {
            var (proof, accessToken, cnfJkt) = CreateProofAndAccessToken(httpMethod: "POST");
            var options = DefaultOptions();

            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);

            Assert.False(result.IsValid);
            Assert.Contains("htm", result.Error.Message);
        }

        [Fact]
        public async Task ValidateAsync_MissingHtm_Fails()
        {
            var (proof, accessToken, cnfJkt) = CreateTamperedProofAndAccessToken(omitHtm: true);
            var options = DefaultOptions();

            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);

            Assert.False(result.IsValid);
            Assert.Contains("htm", result.Error.Message);
        }

        #endregion

        #region Htu Validation

        [Fact]
        public async Task ValidateAsync_HtuMismatch_Fails()
        {
            var (proof, accessToken, cnfJkt) = CreateProofAndAccessToken(uri: "https://resource.example.org/api");
            var options = DefaultOptions();

            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://other.example.org/api"), accessToken, cnfJkt, options);

            Assert.False(result.IsValid);
            Assert.Contains("htu", result.Error.Message);
        }

        [Fact]
        public async Task ValidateAsync_MissingHtu_Fails()
        {
            var (proof, accessToken, cnfJkt) = CreateTamperedProofAndAccessToken(omitHtu: true);
            var options = DefaultOptions();

            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);

            Assert.False(result.IsValid);
            Assert.Contains("htu", result.Error.Message);
        }

        [Fact]
        public async Task ValidateAsync_HtuIgnoresQueryString_Succeeds()
        {
            var (proof, accessToken, cnfJkt) = CreateProofAndAccessToken(uri: "https://resource.example.org/api?foo=bar");
            var options = DefaultOptions();

            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api?other=val"), accessToken, cnfJkt, options);

            Assert.True(result.IsValid);
        }

        // RFC 9449 §4.3 + RFC 3986 §6.2.2: htu comparison must normalize scheme/host
        // case-insensitively but keep path comparison case-sensitive. Each case below pins
        // a specific contract; see UriComparer for the full contract documentation.
        [Theory]
        [InlineData("https://resource.example.org/admin", "https://resource.example.org/admin", true,  "identity")]
        [InlineData("https://Resource.Example.ORG/admin", "https://resource.example.org/admin", true,  "host case-insensitive (RFC 3986 §6.2.2.1)")]
        [InlineData("HTTPS://resource.example.org/admin", "https://resource.example.org/admin", true,  "scheme case-insensitive (RFC 3986 §6.2.2.1)")]
        [InlineData("https://resource.example.org/admin", "https://resource.example.org/Admin", false, "path case-sensitive (RFC 3986 §6.2.2 / §3.3) — the vulnerability fix")]
        [InlineData("https://resource.example.org/Admin", "https://resource.example.org/admin", false, "path case-sensitive (reverse direction)")]
        [InlineData("https://resource.example.org/admin", "https://resource.example.org/admin/", false, "trailing slash is significant (different resource)")]
        [InlineData("https://resource.example.org:443/admin", "https://resource.example.org/admin", true,  "explicit default port equivalence (RFC 3986 §3.2.3)")]
        [InlineData("https://resource.example.org:8443/admin", "https://resource.example.org/admin", false, "explicit non-default port matters")]
        [InlineData("https://resource.example.org/a%41", "https://resource.example.org/aA", true,  "percent-encoding of unreserved chars normalizes (RFC 3986 §6.2.2.2)")]
        [InlineData("https://resource.example.org/admin#frag", "https://resource.example.org/admin", true, "fragment ignored (RFC 9449 §4.3)")]
        [InlineData("https://resource.example.org/admin", "https://resource.example.org/admin?q=1", true, "query string in actual URI ignored (RFC 9449 §4.3)")]
        [InlineData("http://resource.example.org:80/admin", "http://resource.example.org/admin", true, "HTTP default port equivalence (RFC 3986 §3.2.3)")]
        [InlineData("https://resource.example.org/a/../admin", "https://resource.example.org/admin", true, "dot-segment removal per RFC 3986 §6.2.2.3")]
        [InlineData("https://resource.example.org/%2F", "https://resource.example.org//", false, "%2F (reserved /) stays encoded — flip side of unreserved decoding")]
        [InlineData("https://resource.example.org", "https://resource.example.org/", true, "empty path equivalent to /")]
        public async Task ValidateAsync_HtuComparison_AppliesRfcStrictRules(
            string htuClaim,
            string actualRequestUri,
            bool expectSuccess,
            string scenario)
        {
            var (proof, accessToken, cnfJkt) = CreateProofAndAccessToken(uri: htuClaim);
            var options = DefaultOptions();

            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri(actualRequestUri), accessToken, cnfJkt, options);

            Assert.True(
                result.IsValid == expectSuccess,
                $"Scenario: {scenario}. Expected IsValid={expectSuccess}, got {result.IsValid}. Error: {result.Error?.Message}");

            if (!expectSuccess)
                Assert.Same(DpopValidationFailureType.HtuMismatch, result.Error.FailureType);
        }

        #endregion

        #region Iat Validation

        [Fact]
        public async Task ValidateAsync_ExpiredProof_Fails()
        {
            var old = DateTimeOffset.UtcNow.AddSeconds(-700).ToUnixTimeSeconds();
            var (proof, accessToken, cnfJkt) = CreateTamperedProofAndAccessToken(iatOverride: old);
            var options = DefaultOptions();
            options.MaxLifetimeInSeconds = 60;
            options.ClockSkewInSeconds = 30;

            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);

            Assert.False(result.IsValid);
            Assert.Contains("expired", result.Error.Message);
        }

        [Fact]
        public async Task ValidateAsync_FutureIat_Fails()
        {
            var future = DateTimeOffset.UtcNow.AddSeconds(700).ToUnixTimeSeconds();
            var (proof, accessToken, cnfJkt) = CreateTamperedProofAndAccessToken(iatOverride: future);
            var options = DefaultOptions();

            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);

            Assert.False(result.IsValid);
            Assert.Contains("future", result.Error.Message);
        }

        [Fact]
        public async Task ValidateAsync_MissingIat_Fails()
        {
            var (proof, accessToken, cnfJkt) = CreateTamperedProofAndAccessToken(omitIat: true);
            var options = DefaultOptions();

            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);

            Assert.False(result.IsValid);
            Assert.Contains("iat", result.Error.Message);
        }

        #endregion

        #region Jti Validation

        [Fact]
        public async Task ValidateAsync_MissingJti_Fails()
        {
            var (proof, accessToken, cnfJkt) = CreateTamperedProofAndAccessToken(omitJti: true);
            var options = DefaultOptions();

            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);

            Assert.False(result.IsValid);
            Assert.Contains("jti", result.Error.Message);
        }

        [Fact]
        public async Task ValidateAsync_JtiReplayDetected_Fails()
        {
            var (proof, accessToken, cnfJkt) = CreateProofAndAccessToken();
            var cache = new TestJtiReplayCache(replayDetected: true);
            var options = DefaultOptions();
            options.JtiReplayCache = cache;

            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);

            Assert.False(result.IsValid);
            Assert.Contains("replay", result.Error.Message);
            Assert.True(cache.WasCalled);
        }

        [Fact]
        public async Task ValidateAsync_JtiFirstUse_Succeeds()
        {
            var (proof, accessToken, cnfJkt) = CreateProofAndAccessToken();
            var cache = new TestJtiReplayCache(replayDetected: false);
            var options = DefaultOptions();
            options.JtiReplayCache = cache;

            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);

            Assert.True(result.IsValid);
            Assert.True(cache.WasCalled);
        }

        [Fact]
        public async Task ValidateAsync_NoCacheConfigured_SkipsJtiCheck()
        {
            // DefaultOptions() configures ExpectedNonce, so nulling the cache leaves nonce-based
            // replay protection in place; validation should still succeed.
            var (proof, accessToken, cnfJkt) = CreateProofAndAccessToken();
            var options = DefaultOptions();
            options.JtiReplayCache = null;

            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);

            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task ValidateAsync_NeitherJtiNorNonce_NorExternalFlag_FailsClosed()
        {
            // Both replay-protection mechanisms are null and the caller has not declared
            // that replay protection is handled externally — validator must fail closed
            // with ReplayProtectionNotConfigured to prevent silent loss of §4.3 protection.
            var (proof, accessToken, cnfJkt) = CreateProofAndAccessToken(nonce: null);
            var options = DefaultOptions();
            options.JtiReplayCache = null;
            options.ExpectedNonce = null;

            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);

            Assert.False(result.IsValid);
            Assert.Same(DpopValidationFailureType.ReplayProtectionNotConfigured, result.Error.FailureType);
            Assert.Contains("replay protection is not configured", result.Error.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task ValidateAsync_ReplayProtectionHandledExternally_Succeeds()
        {
            // Both replay-protection mechanisms are null, but the caller has declared that
            // a higher layer handles it. Validator must skip its built-in checks without
            // raising the configuration error.
            var (proof, accessToken, cnfJkt) = CreateProofAndAccessToken(nonce: null);
            var options = DefaultOptions();
            options.JtiReplayCache = null;
            options.ExpectedNonce = null;
            options.ReplayProtectionHandledExternally = true;

            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);

            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task ValidateAsync_ReplayProtectionHandledExternally_TrueWithNonceConfigured_NonceStillEnforced()
        {
            // The flag is purely an acknowledgement for the "both-null" case. When a nonce is
            // configured the validator must still enforce it; the flag does not disable checks.
            var (proof, accessToken, cnfJkt) = CreateProofAndAccessToken(nonce: "actual-nonce");
            var options = DefaultOptions();
            options.JtiReplayCache = null;
            options.ExpectedNonce = "different-nonce";
            options.ReplayProtectionHandledExternally = true;

            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);

            Assert.False(result.IsValid);
            Assert.True(result.IsNonceRequired);
        }

        [Fact]
        public async Task ValidateAsync_OnlyNonceConfigured_Succeeds()
        {
            // A single replay-protection mechanism (nonce) is sufficient per RFC 9449 §4.3.
            var (proof, accessToken, cnfJkt) = CreateProofAndAccessToken();
            var options = DefaultOptions();
            options.JtiReplayCache = null;
            // ExpectedNonce is set by DefaultOptions()

            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);

            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task ValidateAsync_OnlyJtiCacheConfigured_Succeeds()
        {
            // A single replay-protection mechanism (jti cache) is sufficient per RFC 9449 §4.3.
            var (proof, accessToken, cnfJkt) = CreateProofAndAccessToken(nonce: null);
            var cache = new TestJtiReplayCache(replayDetected: false);
            var options = DefaultOptions();
            options.ExpectedNonce = null;
            options.JtiReplayCache = cache;

            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);

            Assert.True(result.IsValid);
            Assert.True(cache.WasCalled);
        }

        #endregion

        #region Nonce Validation

        [Fact]
        public async Task ValidateAsync_NonceMissing_ReturnsNonceRequired()
        {
            var (proof, accessToken, cnfJkt) = CreateProofAndAccessToken(nonce: null);
            var options = DefaultOptions();
            options.ExpectedNonce = "required-nonce";

            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);

            Assert.False(result.IsValid);
            Assert.True(result.IsNonceRequired);
        }

        [Fact]
        public async Task ValidateAsync_NonceWrong_ReturnsNonceRequired()
        {
            var (proof, accessToken, cnfJkt) = CreateProofAndAccessToken(nonce: "wrong-nonce");
            var options = DefaultOptions();
            options.ExpectedNonce = "expected-nonce";

            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);

            Assert.False(result.IsValid);
            Assert.True(result.IsNonceRequired);
        }

        [Fact]
        public async Task ValidateAsync_NonceNotRequired_JtiUsedInstead_Succeeds()
        {
            var (proof, accessToken, cnfJkt) = CreateProofAndAccessToken(nonce: null);
            var cache = new TestJtiReplayCache(replayDetected: false);
            var options = DefaultOptions();
            options.ExpectedNonce = null;
            options.JtiReplayCache = cache;

            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);

            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task ValidateAsync_EmptyExpectedNonce_ReturnsInvalid()
        {
            var (proofJwt, accessToken, cnfJkt) = CreateProofAndAccessToken();
            var options = DefaultOptions();
            options.ExpectedNonce = "";

            var result = await _validator.ValidateAsync(
                proofJwt, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);

            Assert.False(result.IsValid);
            Assert.Contains("ExpectedNonce", result.Error.Message);
        }

        #endregion

        #region Ath (Access Token Hash) Validation

        [Fact]
        public async Task ValidateAsync_AthMismatch_Fails()
        {
            // Create proof bound to one AT, validate with a different AT
            var rsa = CreateTestRsa();
            var (atA, cnfJkt) = CreateSimpleAccessToken(rsa);
            var (atB, _) = CreateSimpleAccessToken(rsa);
            var (proof, _) = CreateValidRsaProof(accessToken: atA, proofKey: rsa);
            var options = DefaultOptions();

            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), atB, cnfJkt, options);

            Assert.False(result.IsValid);
            Assert.Contains("ath", result.Error.Message);
        }

        [Fact]
        public async Task ValidateAsync_AthMissing_Fails()
        {
            // Create proof without ath but with a valid access token
            var (proof, accessToken, cnfJkt) = CreateTamperedProofAndAccessToken(omitAth: true);
            var options = DefaultOptions();

            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);

            Assert.False(result.IsValid);
            Assert.Contains("ath", result.Error.Message);
        }

        #endregion

        #region Signature Validation

        [Fact]
        public async Task ValidateAsync_WrongSignatureKey_Fails()
        {
            var (proof, accessToken, cnfJkt) = CreateProofAndAccessToken();

            // Tamper: decode, modify a character, re-encode — signature no longer matches
            var parts = proof.Split('.');
            var payload = parts[1] + "X";
            var tampered = parts[0] + "." + payload + "." + parts[2];

            var options = DefaultOptions();

            var result = await _validator.ValidateAsync(
                tampered, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);

            Assert.False(result.IsValid);
        }

        #endregion

        #region Private Key Detection

        [Fact]
        public void ContainsPrivateKeyMaterial_RsaPrivate_ReturnsTrue()
        {
            var jwk = new JsonWebKey { D = "secret", Kty = "RSA" };
            Assert.True(DpopProofValidator.ContainsPrivateKeyMaterial(jwk));
        }

        [Fact]
        public void ContainsPrivateKeyMaterial_EcPublic_ReturnsFalse()
        {
            var jwk = new JsonWebKey { Kty = "EC", Crv = "P-256", X = "x", Y = "y" };
            Assert.False(DpopProofValidator.ContainsPrivateKeyMaterial(jwk));
        }

        [Fact]
        public void ContainsPrivateKeyMaterial_RsaPublic_ReturnsFalse()
        {
            var jwk = new JsonWebKey { Kty = "RSA", E = "AQAB", N = "n" };
            Assert.False(DpopProofValidator.ContainsPrivateKeyMaterial(jwk));
        }

        [Fact]
        public void ContainsPrivateKeyMaterial_WithDP_ReturnsTrue()
        {
            var jwk = new JsonWebKey { DP = "dp" };
            Assert.True(DpopProofValidator.ContainsPrivateKeyMaterial(jwk));
        }

        #endregion

        #region CnfJkt Binding

        [Fact]
        public async Task ValidateAsync_CnfJktMatch_Succeeds()
        {
            var (proof, accessToken, cnfJkt) = CreateProofAndAccessToken();
            var options = DefaultOptions();

            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);

            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task ValidateAsync_CnfJktMismatch_Fails()
        {
            var (proof, accessToken, _) = CreateProofAndAccessToken();
            var options = DefaultOptions();

            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, "wrong-thumbprint", options);

            Assert.False(result.IsValid);
            Assert.Contains("cnf.jkt", result.Error.Message);
        }

        [Fact]
        public async Task ValidateAsync_EmptyCnfJkt_ReturnsInvalid()
        {
            var (proof, accessToken, _) = CreateProofAndAccessToken();
            var options = DefaultOptions();

            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, " ", options);

            Assert.False(result.IsValid);
            Assert.Contains("cnf.jkt", result.Error.Message);
        }

        #endregion

        #region ComputeAccessTokenHash

        [Fact]
        public void ComputeAccessTokenHash_Deterministic()
        {
            var hash1 = DpopProofValidator.ComputeAccessTokenHash("my-token");
            var hash2 = DpopProofValidator.ComputeAccessTokenHash("my-token");
            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void ComputeAccessTokenHash_DifferentTokens_DifferentHash()
        {
            var hash1 = DpopProofValidator.ComputeAccessTokenHash("token-A");
            var hash2 = DpopProofValidator.ComputeAccessTokenHash("token-B");
            Assert.NotEqual(hash1, hash2);
        }

        #endregion

        #region JwkThumbprint

#if !NET462 // net462 lacks ECDsa.Create(ECCurve) and ECDsa.ExportParameters
        [Fact]
        public void ComputeJwkThumbprint_EC_Deterministic()
        {
            var ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256);
            var jwk = JsonWebKeyConverter.ConvertFromSecurityKey(new ECDsaSecurityKey(ecdsa));

            var t1 = DpopProofValidator.ComputeJwkThumbprint(jwk);
            var t2 = DpopProofValidator.ComputeJwkThumbprint(jwk);
            Assert.Equal(t1, t2);
            Assert.False(string.IsNullOrEmpty(t1));
        }
#endif

        [Fact]
        public void ComputeJwkThumbprint_RSA_Deterministic()
        {
            var rsa = CreateTestRsa();
            var jwk = JsonWebKeyConverter.ConvertFromSecurityKey(new RsaSecurityKey(rsa));

            var t1 = DpopProofValidator.ComputeJwkThumbprint(jwk);
            var t2 = DpopProofValidator.ComputeJwkThumbprint(jwk);
            Assert.Equal(t1, t2);
        }

        #endregion

        #region Input Validation

        [Fact]
        public async Task ValidateAsync_NullProof_Throws()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _validator.ValidateAsync(null, "GET", new Uri("https://example.com"), "at", "jkt", DefaultOptions()));
        }

        [Fact]
        public async Task ValidateAsync_EmptyProof_ReturnsInvalid()
        {
            var result = await _validator.ValidateAsync(
                "  ", "GET", new Uri("https://example.com"), "at", "jkt", DefaultOptions());

            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task ValidateAsync_NullOptions_Throws()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _validator.ValidateAsync("jwt", "GET", new Uri("https://example.com"), "at", "jkt", null));
        }

        [Fact]
        public async Task ValidateAsync_NullAccessToken_Throws()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _validator.ValidateAsync("jwt", "GET", new Uri("https://example.com"), null, "jkt", DefaultOptions()));
        }

        [Fact]
        public async Task ValidateAsync_NullCnfJkt_Throws()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _validator.ValidateAsync("jwt", "GET", new Uri("https://example.com"), "at", null, DefaultOptions()));
        }

        [Fact]
        public async Task ValidateAsync_EmptyAccessToken_ReturnsInvalid()
        {
            var result = await _validator.ValidateAsync(
                "jwt", "GET", new Uri("https://example.com"), " ", "jkt", DefaultOptions());

            Assert.False(result.IsValid);
            Assert.Contains("Access token", result.Error.Message);
        }

        [Fact]
        public async Task ValidateAsync_RelativeUri_Throws()
        {
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _validator.ValidateAsync("jwt", "GET", new Uri("/relative", UriKind.Relative), "at", "jkt", DefaultOptions()));
        }

        [Fact]
        public async Task ValidateAsync_UnparseableProof_ReturnsInvalid()
        {
            var rsa = CreateTestRsa();
            var (accessToken, cnfJkt) = CreateSimpleAccessToken(rsa);

            var result = await _validator.ValidateAsync(
                "not-a-jwt", "GET", new Uri("https://example.com/api"), accessToken, cnfJkt, DefaultOptions());

            Assert.False(result.IsValid);
            Assert.NotNull(result.Error?.Exception);
        }

        #endregion

        #region RSA Key Support

        [Fact]
        public async Task ValidateAsync_RsaKey_Succeeds()
        {
            var rsa = CreateTestRsa();
            var (accessToken, cnfJkt) = CreateSimpleAccessToken(rsa);
            var signingCredentials = new SigningCredentials(
                new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);

            var proofOptions = new DpopProofCreatorOptions
            {
                SigningCredentials = signingCredentials,
                IncludeNonce = true,
                Nonce = DefaultTestNonce,
            };
            var dpopProof = new DpopProofCreator(proofOptions);
            var proof = dpopProof.CreateProof("GET", new Uri("https://resource.example.org/api"), accessToken);

            var options = new DpopValidationOptions
            {
                AllowedSigningAlgorithms = new HashSet<string>(StringComparer.Ordinal) { "RS256" },
                ExpectedNonce = DefaultTestNonce,
            };

            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);

            Assert.True(result.IsValid);
        }

        #endregion

        #region Size Limits & Granular Errors

        [Fact]
        public async Task ValidateAsync_ProofExceedsMaxSize_ReturnsInvalid()
        {
            var (proof, accessToken, cnfJkt) = CreateProofAndAccessToken();
            var options = DefaultOptions();
            options.MaxProofTokenSizeInBytes = 16;

            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);

            Assert.False(result.IsValid);
            Assert.Contains("maximum allowed size", result.Error.Message);
        }

        [Fact]
        public async Task ValidateAsync_ProofUnderMaxSize_Succeeds()
        {
            var (proof, accessToken, cnfJkt) = CreateProofAndAccessToken();
            var options = DefaultOptions();
            options.MaxProofTokenSizeInBytes = proof.Length + 1;

            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);

            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task ValidateAsync_RsaModulusBelowMin_ReturnsInvalid()
        {
            // 2048-bit key + an artificially high minimum to trigger the floor.
            var (proof, accessToken, cnfJkt) = CreateProofAndAccessToken();
            var options = DefaultOptions();
            options.MinRsaKeySizeInBits = 4096;

            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);

            Assert.False(result.IsValid);
            Assert.Contains("below the minimum allowed size", result.Error.Message);
        }

        [Fact]
        public async Task ValidateAsync_RsaModulusAboveMax_ReturnsInvalid()
        {
            // 2048-bit key + an artificially low maximum to trigger the ceiling.
            var (proof, accessToken, cnfJkt) = CreateProofAndAccessToken();
            var options = DefaultOptions();
            options.MaxRsaKeySizeInBits = 1024;

            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);

            Assert.False(result.IsValid);
            Assert.Contains("exceeds the maximum allowed size", result.Error.Message);
        }

        [Fact]
        public async Task ValidateAsync_RsaModulusWithinBounds_Succeeds()
        {
            // 2048-bit key with defaults (Min=2048, Max=4096) should succeed.
            var (proof, accessToken, cnfJkt) = CreateProofAndAccessToken();
            var options = DefaultOptions();

            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void MaxLifetimeInSeconds_InvalidValue_Throws(int value)
        {
            var options = new DpopValidationOptions();
            Assert.Throws<ArgumentOutOfRangeException>(() => options.MaxLifetimeInSeconds = value);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-300)]
        public void ClockSkewInSeconds_NegativeValue_Throws(int value)
        {
            var options = new DpopValidationOptions();
            Assert.Throws<ArgumentOutOfRangeException>(() => options.ClockSkewInSeconds = value);
        }

        [Fact]
        public void Defaults_MatchConstants()
        {
            var options = new DpopValidationOptions();
            Assert.Equal(DpopConstants.DefaultMaxLifetimeInSeconds, options.MaxLifetimeInSeconds);
            Assert.Equal(DpopConstants.DefaultClockSkewInSeconds, options.ClockSkewInSeconds);
            Assert.Equal(DpopConstants.DefaultMaxProofTokenSizeInBytes, options.MaxProofTokenSizeInBytes);
            Assert.Equal(DpopConstants.DefaultMaxRsaKeySizeInBits, options.MaxRsaKeySizeInBits);
            Assert.Equal(DpopConstants.DefaultMinRsaKeySizeInBits, options.MinRsaKeySizeInBits);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void MaxProofTokenSizeInBytes_InvalidValue_Throws(int value)
        {
            var options = new DpopValidationOptions();
            Assert.Throws<ArgumentOutOfRangeException>(() => options.MaxProofTokenSizeInBytes = value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void MaxRsaKeySizeInBits_InvalidValue_Throws(int value)
        {
            var options = new DpopValidationOptions();
            Assert.Throws<ArgumentOutOfRangeException>(() => options.MaxRsaKeySizeInBits = value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void MinRsaKeySizeInBits_InvalidValue_Throws(int value)
        {
            var options = new DpopValidationOptions();
            Assert.Throws<ArgumentOutOfRangeException>(() => options.MinRsaKeySizeInBits = value);
        }

#if !NET462
        [Fact]
        public async Task ValidateAsync_EcAlgWithMismatchedCurve_ReturnsCurveSpecificError()
        {
            // Sign with ES256 (requires P-256) but advertise a P-384 JWK in the header.
            var p256 = ECDsa.Create(ECCurve.NamedCurves.nistP256);
            var p384 = ECDsa.Create(ECCurve.NamedCurves.nistP384);
            var (accessToken, cnfJkt) = CreateSimpleAccessToken(CreateTestRsa());

            var signingCredentials = new SigningCredentials(
                new ECDsaSecurityKey(p256), SecurityAlgorithms.EcdsaSha256);

            var p384Params = p384.ExportParameters(false);
            var jwkForHeader = new System.Text.Json.Nodes.JsonObject
            {
                ["kty"] = "EC",
                ["crv"] = "P-384",
                ["x"] = Base64UrlEncoder.Encode(p384Params.Q.X),
                ["y"] = Base64UrlEncoder.Encode(p384Params.Q.Y),
            };

            var now = DateTimeOffset.UtcNow;
            var claims = new Dictionary<string, object>
            {
                ["htm"] = "GET",
                ["htu"] = "https://resource.example.org/api",
                ["iat"] = now.ToUnixTimeSeconds(),
                ["jti"] = Guid.NewGuid().ToString(),
                ["nonce"] = DefaultTestNonce,
            };

            var descriptor = new SecurityTokenDescriptor
            {
                IncludeKeyIdInHeader = false,
                Claims = claims,
                AdditionalHeaderClaims = new Dictionary<string, object>
                {
                    { "typ", "dpop+jwt" },
                    { "jwk", jwkForHeader },
                },
                SigningCredentials = signingCredentials,
            };
            var handler = new JsonWebTokenHandler { SetDefaultTimesOnTokenCreation = false };
            var proof = handler.CreateToken(descriptor);

            var options = new DpopValidationOptions
            {
                AllowedSigningAlgorithms = new HashSet<string>(StringComparer.Ordinal) { "ES256" },
                ExpectedNonce = DefaultTestNonce,
            };

            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);

            Assert.False(result.IsValid);
            Assert.Contains("requires curve", result.Error.Message);
        }
#endif

        #endregion



        #region Test Doubles

        /// <summary>
        /// Simple test double for <see cref="IJtiReplayCache"/>.
        /// </summary>
        private sealed class TestJtiReplayCache : IJtiReplayCache
        {
            private readonly bool _replayDetected;

            public TestJtiReplayCache(bool replayDetected)
            {
                _replayDetected = replayDetected;
            }

            public bool WasCalled { get; private set; }

            public Task<bool> TryAddAsync(string jti, DateTimeOffset expiration, CancellationToken cancellationToken = default)
            {
                WasCalled = true;
                // TryAdd returns true when added (NOT replay), false when replay detected
                return Task.FromResult(!_replayDetected);
            }
        }

        #endregion

        #region FailureType Coverage

        // Every failure path in DpopProofValidator must return a result with a populated FailureType
        // on the inner Error object. These tests pin the contract so refactors don't drop the category.

        [Fact]
        public async Task FailureType_ProofMissing()
        {
            var (_, accessToken, cnfJkt) = CreateProofAndAccessToken();
            var result = await _validator.ValidateAsync(
                "", "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, DefaultOptions());
            Assert.False(result.IsValid);
            Assert.Same(DpopValidationFailureType.ProofMissing, result.Error.FailureType);
        }

        [Fact]
        public async Task FailureType_ProofExceedsMaxSize()
        {
            var (_, accessToken, cnfJkt) = CreateProofAndAccessToken();
            var options = DefaultOptions();
            options.MaxProofTokenSizeInBytes = 10;
            var result = await _validator.ValidateAsync(
                new string('a', 100), "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);
            Assert.False(result.IsValid);
            Assert.Same(DpopValidationFailureType.ProofExceedsMaxSize, result.Error.FailureType);
        }

        [Fact]
        public async Task FailureType_AccessTokenMissing()
        {
            var (proof, _, cnfJkt) = CreateProofAndAccessToken();
            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), "", cnfJkt, DefaultOptions());
            Assert.False(result.IsValid);
            Assert.Same(DpopValidationFailureType.AccessTokenMissing, result.Error.FailureType);
        }

        [Fact]
        public async Task FailureType_CnfJktMissing()
        {
            var (proof, accessToken, _) = CreateProofAndAccessToken();
            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, "", DefaultOptions());
            Assert.False(result.IsValid);
            Assert.Same(DpopValidationFailureType.CnfJktMissing, result.Error.FailureType);
        }

        [Fact]
        public async Task FailureType_ReplayProtectionNotConfigured()
        {
            var (proof, accessToken, cnfJkt) = CreateProofAndAccessToken();
            var options = DefaultOptions();
            options.ExpectedNonce = null;
            options.JtiReplayCache = null;
            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);
            Assert.False(result.IsValid);
            Assert.Same(DpopValidationFailureType.ReplayProtectionNotConfigured, result.Error.FailureType);
        }

        [Fact]
        public async Task FailureType_TokenTypeInvalid()
        {
            var (proof, accessToken, cnfJkt) = CreateTamperedProofAndAccessToken(typ: "jwt");
            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, DefaultOptions());
            Assert.False(result.IsValid);
            Assert.Same(DpopValidationFailureType.TokenTypeInvalid, result.Error.FailureType);
        }

        [Fact]
        public async Task FailureType_AlgorithmDisallowed_NotInSet()
        {
            var (proof, accessToken, cnfJkt) = CreateProofAndAccessToken();
            var options = DefaultOptions();
            options.AllowedSigningAlgorithms = new HashSet<string>(StringComparer.Ordinal) { "ES256" };
            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);
            Assert.False(result.IsValid);
            Assert.Same(DpopValidationFailureType.AlgorithmDisallowed, result.Error.FailureType);
        }

        [Fact]
        public async Task FailureType_InvalidConfiguration_EmptyAllowedSet()
        {
            var (proof, accessToken, cnfJkt) = CreateProofAndAccessToken();
            var options = DefaultOptions();
            options.AllowedSigningAlgorithms = new HashSet<string>(StringComparer.Ordinal);
            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);
            Assert.False(result.IsValid);
            Assert.Same(DpopValidationFailureType.InvalidConfiguration, result.Error.FailureType);
        }

        [Fact]
        public async Task FailureType_InvalidConfiguration_WhitespaceNonce()
        {
            var (proof, accessToken, cnfJkt) = CreateProofAndAccessToken();
            var options = DefaultOptions();
            options.ExpectedNonce = "   ";
            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);
            Assert.False(result.IsValid);
            Assert.Same(DpopValidationFailureType.InvalidConfiguration, result.Error.FailureType);
        }

        [Fact]
        public async Task FailureType_JwkInvalid_PrivateKeyMaterial()
        {
            var (proof, accessToken, cnfJkt) = CreateTamperedProofAndAccessToken(includePrivateKey: true);
            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, DefaultOptions());
            Assert.False(result.IsValid);
            Assert.Same(DpopValidationFailureType.JwkInvalid, result.Error.FailureType);
        }

        [Fact]
        public async Task FailureType_HtmMismatch()
        {
            var (proof, accessToken, cnfJkt) = CreateProofAndAccessToken(httpMethod: "GET");
            var result = await _validator.ValidateAsync(
                proof, "POST", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, DefaultOptions());
            Assert.False(result.IsValid);
            Assert.Same(DpopValidationFailureType.HtmMismatch, result.Error.FailureType);
        }

        [Fact]
        public async Task FailureType_HtmMissing()
        {
            var (proof, accessToken, cnfJkt) = CreateTamperedProofAndAccessToken(omitHtm: true);
            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, DefaultOptions());
            Assert.False(result.IsValid);
            Assert.Same(DpopValidationFailureType.HtmMissing, result.Error.FailureType);
        }

        [Fact]
        public async Task FailureType_HtuMismatch()
        {
            var (proof, accessToken, cnfJkt) = CreateProofAndAccessToken(uri: "https://a.example.org/x");
            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://b.example.org/y"), accessToken, cnfJkt, DefaultOptions());
            Assert.False(result.IsValid);
            Assert.Same(DpopValidationFailureType.HtuMismatch, result.Error.FailureType);
        }

        [Fact]
        public async Task FailureType_HtuMissing()
        {
            var (proof, accessToken, cnfJkt) = CreateTamperedProofAndAccessToken(omitHtu: true);
            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, DefaultOptions());
            Assert.False(result.IsValid);
            Assert.Same(DpopValidationFailureType.HtuMissing, result.Error.FailureType);
        }

        [Fact]
        public async Task FailureType_IatMissing()
        {
            var (proof, accessToken, cnfJkt) = CreateTamperedProofAndAccessToken(omitIat: true);
            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, DefaultOptions());
            Assert.False(result.IsValid);
            Assert.Same(DpopValidationFailureType.IatMissing, result.Error.FailureType);
        }

        [Fact]
        public async Task FailureType_ProofExpired()
        {
            var pastIat = DateTimeOffset.UtcNow.AddSeconds(-2000).ToUnixTimeSeconds();
            var (proof, accessToken, cnfJkt) = CreateTamperedProofAndAccessToken(iatOverride: pastIat);
            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, DefaultOptions());
            Assert.False(result.IsValid);
            Assert.Same(DpopValidationFailureType.ProofExpired, result.Error.FailureType);
        }

        [Fact]
        public async Task FailureType_ProofIssuedInFuture()
        {
            var futureIat = DateTimeOffset.UtcNow.AddSeconds(2000).ToUnixTimeSeconds();
            var (proof, accessToken, cnfJkt) = CreateTamperedProofAndAccessToken(iatOverride: futureIat);
            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, DefaultOptions());
            Assert.False(result.IsValid);
            Assert.Same(DpopValidationFailureType.ProofIssuedInFuture, result.Error.FailureType);
        }

        [Fact]
        public async Task FailureType_JtiMissing()
        {
            var (proof, accessToken, cnfJkt) = CreateTamperedProofAndAccessToken(omitJti: true);
            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, DefaultOptions());
            Assert.False(result.IsValid);
            Assert.Same(DpopValidationFailureType.JtiMissing, result.Error.FailureType);
        }

        [Fact]
        public async Task FailureType_AthMissing()
        {
            var (proof, accessToken, cnfJkt) = CreateTamperedProofAndAccessToken(omitAth: true);
            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, DefaultOptions());
            Assert.False(result.IsValid);
            Assert.Same(DpopValidationFailureType.AthMissing, result.Error.FailureType);
        }

        [Fact]
        public async Task FailureType_AthMismatch()
        {
            var (proofA, accessTokenA, cnfJkt) = CreateProofAndAccessToken();
            var (_, accessTokenB, _) = CreateProofAndAccessToken();
            var result = await _validator.ValidateAsync(
                proofA, "GET", new Uri("https://resource.example.org/api"), accessTokenB, cnfJkt, DefaultOptions());
            Assert.False(result.IsValid);
            Assert.Same(DpopValidationFailureType.AthMismatch, result.Error.FailureType);
        }

        [Fact]
        public async Task FailureType_CnfJktMismatch()
        {
            var (proof, accessToken, _) = CreateProofAndAccessToken();
            var wrongCnfJkt = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, wrongCnfJkt, DefaultOptions());
            Assert.False(result.IsValid);
            Assert.Same(DpopValidationFailureType.CnfJktMismatch, result.Error.FailureType);
        }

        [Fact]
        public async Task FailureType_JtiReplayDetected()
        {
            var (proof, accessToken, cnfJkt) = CreateProofAndAccessToken();
            var options = DefaultOptions();
            options.ExpectedNonce = null;
            options.JtiReplayCache = new TestJtiReplayCache(replayDetected: true);
            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);
            Assert.False(result.IsValid);
            Assert.Same(DpopValidationFailureType.JtiReplayDetected, result.Error.FailureType);
        }

        [Fact]
        public async Task FailureType_NonceRequired()
        {
            var (proof, accessToken, cnfJkt) = CreateProofAndAccessToken(nonce: null);
            var options = DefaultOptions();
            options.ExpectedNonce = "required-nonce";
            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);
            Assert.False(result.IsValid);
            Assert.True(result.IsNonceRequired);
            Assert.Same(DpopValidationFailureType.NonceRequired, result.Error.FailureType);
        }

        [Fact]
        public async Task FailureType_NonceMismatch()
        {
            var (proof, accessToken, cnfJkt) = CreateProofAndAccessToken(nonce: "actual");
            var options = DefaultOptions();
            options.ExpectedNonce = "expected";
            var result = await _validator.ValidateAsync(
                proof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, options);
            Assert.False(result.IsValid);
            Assert.True(result.IsNonceRequired);
            Assert.Same(DpopValidationFailureType.NonceMismatch, result.Error.FailureType);
        }

        [Fact]
        public async Task FailureType_JwkMissing()
        {
            // Build a proof JWT that omits the 'jwk' header entirely.
            var rsa = CreateTestRsa();
            var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);
            var handler = new JsonWebTokenHandler { SetDefaultTimesOnTokenCreation = false };
            var jwt = handler.CreateToken(new SecurityTokenDescriptor
            {
                IncludeKeyIdInHeader = false,
                Claims = new Dictionary<string, object>
                {
                    { "htm", "GET" },
                    { "htu", "https://resource.example.org/api" },
                    { "iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds() },
                    { "jti", Guid.NewGuid().ToString() },
                },
                AdditionalHeaderClaims = new Dictionary<string, object>
                {
                    { "typ", "dpop+jwt" },
                },
                SigningCredentials = signingCredentials,
            });
            var (accessToken, cnfJkt) = CreateSimpleAccessToken(rsa);

            var result = await _validator.ValidateAsync(
                jwt, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, DefaultOptions());
            Assert.False(result.IsValid);
            Assert.Same(DpopValidationFailureType.JwkMissing, result.Error.FailureType);
        }

        [Fact]
        public async Task FailureType_ProofParseFailure()
        {
            // Build a proof whose 'jwk' header is present but not a parseable JWK.
            var rsa = CreateTestRsa();
            var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);
            var handler = new JsonWebTokenHandler { SetDefaultTimesOnTokenCreation = false };
            var jwt = handler.CreateToken(new SecurityTokenDescriptor
            {
                IncludeKeyIdInHeader = false,
                Claims = new Dictionary<string, object>
                {
                    { "htm", "GET" },
                    { "htu", "https://resource.example.org/api" },
                    { "iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds() },
                    { "jti", Guid.NewGuid().ToString() },
                },
                AdditionalHeaderClaims = new Dictionary<string, object>
                {
                    { "typ", "dpop+jwt" },
                    { "jwk", "this-is-not-a-jwk" },
                },
                SigningCredentials = signingCredentials,
            });
            var (accessToken, cnfJkt) = CreateSimpleAccessToken(rsa);

            var result = await _validator.ValidateAsync(
                jwt, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, DefaultOptions());
            Assert.False(result.IsValid);
            Assert.Same(DpopValidationFailureType.ProofParseFailure, result.Error.FailureType);
        }

        [Fact]
        public async Task FailureType_SignatureInvalid()
        {
            // Take a valid proof and corrupt the signature segment. The header/payload still parse
            // and the JWK still validates, but the signature won't verify against the proof key.
            var (proof, accessToken, cnfJkt) = CreateProofAndAccessToken();
            var parts = proof.Split('.');
            Assert.Equal(3, parts.Length);
            // Flip a character in the signature segment that's guaranteed to be base64url-valid.
            var sigChars = parts[2].ToCharArray();
            sigChars[0] = sigChars[0] == 'A' ? 'B' : 'A';
            var tamperedProof = parts[0] + "." + parts[1] + "." + new string(sigChars);

            var result = await _validator.ValidateAsync(
                tamperedProof, "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, DefaultOptions());
            Assert.False(result.IsValid);
            Assert.Same(DpopValidationFailureType.SignatureInvalid, result.Error.FailureType);
        }

        [Fact]
        public async Task FailureType_UnexpectedError()
        {
            // A short non-JWT string passes the size + non-empty checks but throws inside
            // ReadJsonWebToken, hitting the top-level catch in ValidateAsync.
            var (_, accessToken, cnfJkt) = CreateProofAndAccessToken();
            var result = await _validator.ValidateAsync(
                "this-is-not-a-jwt", "GET", new Uri("https://resource.example.org/api"), accessToken, cnfJkt, DefaultOptions());
            Assert.False(result.IsValid);
            Assert.Same(DpopValidationFailureType.UnexpectedError, result.Error.FailureType);
            Assert.NotNull(result.Error.Exception);
        }

        [Fact]
        public void FailureType_NamesAreStable()
        {
            // Names are surfaced for telemetry / SIEM. Pin them so a rename can't ship silently.
            Assert.Equal("ReplayProtectionNotConfigured", DpopValidationFailureType.ReplayProtectionNotConfigured.Name);
            Assert.Equal("InvalidConfiguration", DpopValidationFailureType.InvalidConfiguration.Name);
            Assert.Equal("ProofMissing", DpopValidationFailureType.ProofMissing.Name);
            Assert.Equal("ProofExceedsMaxSize", DpopValidationFailureType.ProofExceedsMaxSize.Name);
            Assert.Equal("AccessTokenMissing", DpopValidationFailureType.AccessTokenMissing.Name);
            Assert.Equal("CnfJktMissing", DpopValidationFailureType.CnfJktMissing.Name);
            Assert.Equal("UnexpectedError", DpopValidationFailureType.UnexpectedError.Name);
            Assert.Equal("ProofParseFailure", DpopValidationFailureType.ProofParseFailure.Name);
            Assert.Equal("TokenTypeInvalid", DpopValidationFailureType.TokenTypeInvalid.Name);
            Assert.Equal("AlgorithmDisallowed", DpopValidationFailureType.AlgorithmDisallowed.Name);
            Assert.Equal("JwkMissing", DpopValidationFailureType.JwkMissing.Name);
            Assert.Equal("JwkInvalid", DpopValidationFailureType.JwkInvalid.Name);
            Assert.Equal("SignatureInvalid", DpopValidationFailureType.SignatureInvalid.Name);
            Assert.Equal("HtmMissing", DpopValidationFailureType.HtmMissing.Name);
            Assert.Equal("HtmMismatch", DpopValidationFailureType.HtmMismatch.Name);
            Assert.Equal("HtuMissing", DpopValidationFailureType.HtuMissing.Name);
            Assert.Equal("HtuMismatch", DpopValidationFailureType.HtuMismatch.Name);
            Assert.Equal("IatMissing", DpopValidationFailureType.IatMissing.Name);
            Assert.Equal("ProofExpired", DpopValidationFailureType.ProofExpired.Name);
            Assert.Equal("ProofIssuedInFuture", DpopValidationFailureType.ProofIssuedInFuture.Name);
            Assert.Equal("JtiMissing", DpopValidationFailureType.JtiMissing.Name);
            Assert.Equal("JtiReplayDetected", DpopValidationFailureType.JtiReplayDetected.Name);
            Assert.Equal("AthMissing", DpopValidationFailureType.AthMissing.Name);
            Assert.Equal("AthMismatch", DpopValidationFailureType.AthMismatch.Name);
            Assert.Equal("CnfJktMismatch", DpopValidationFailureType.CnfJktMismatch.Name);
            Assert.Equal("NonceRequired", DpopValidationFailureType.NonceRequired.Name);
            Assert.Equal("NonceMismatch", DpopValidationFailureType.NonceMismatch.Name);
        }

        #endregion
    }
}