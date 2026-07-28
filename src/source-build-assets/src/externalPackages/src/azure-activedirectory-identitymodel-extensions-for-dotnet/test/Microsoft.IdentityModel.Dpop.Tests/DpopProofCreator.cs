// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// NOTE: This is a test-only proof creator for testing and demonstration purposes.
// In a production application, use MSAL's built-in DPoP support instead.
// See: https://learn.microsoft.com/entra/msal/dotnet/advanced/proof-of-possession

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Dpop;

/// <summary>
/// This class creates DPoP proof JWTs according to RFC 9449.
/// It is intended for test and sample use only.
/// </summary>
internal class DpopProofCreator
{
    private readonly DpopProofCreatorOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="DpopProofCreator"/> class.
    /// </summary>
    /// <param name="options">The options for creating DPoP proofs.</param>
    public DpopProofCreator(DpopProofCreatorOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));

        if (_options.SigningCredentials == null)
            throw new ArgumentException("SigningCredentials must be provided", nameof(options));
    }

    /// <summary>
    /// Creates a DPoP proof JWT for the specified HTTP request.
    /// </summary>
    /// <param name="httpMethod">The HTTP method of the request.</param>
    /// <param name="uri">The URI of the request.</param>
    /// <param name="accessToken">The access token to bind this proof to.</param>
    /// <param name="serverProvidedNonce">Optional server provided nonce to include in the proof.</param>
    /// <returns>A JWT string representing the DPoP proof.</returns>
    public string CreateProof(string httpMethod, Uri uri, string accessToken = null, string serverProvidedNonce = null)
    {
        if (string.IsNullOrWhiteSpace(httpMethod))
            throw new ArgumentException("HTTP method cannot be null, empty, or whitespace.", nameof(httpMethod));

        if (uri is null)
            throw new ArgumentNullException(nameof(uri));

        if (!uri.IsAbsoluteUri)
            throw new ArgumentException("URI must be absolute.", nameof(uri));

        var now =
#if NET8_0_OR_GREATER
            _options.TimeProvider.GetUtcNow();
#else
            DateTimeOffset.UtcNow;
#endif

        var claims = new Dictionary<string, object>
        {
            // DPoP RFC 9449 required claims
            { DpopClaimTypes.Htm, httpMethod.ToUpperInvariant() },
            { DpopClaimTypes.Htu, uri.GetLeftPart(UriPartial.Path) },
            { DpopClaimTypes.Iat, now.ToUnixTimeSeconds() },
            { DpopClaimTypes.Jti, Guid.NewGuid().ToString() }
        };

        if (!string.IsNullOrEmpty(accessToken))
        {
            // RFC 9449 §4.2: ath = base64url(SHA-256(ASCII(access_token)))
            // Use a strict ASCII encoder that throws on non-ASCII characters rather than
            // silently replacing them with '?' (which would produce an incorrect hash).
            byte[] tokenBytes;
            try
            {
                tokenBytes = Encoding.GetEncoding(
                    "us-ascii",
                    EncoderFallback.ExceptionFallback,
                    DecoderFallback.ExceptionFallback).GetBytes(accessToken);
            }
            catch (EncoderFallbackException ex)
            {
                throw new ArgumentException(
                    "Access token contains non-ASCII characters. " +
                    "RFC 9449 §4.2 requires the ASCII encoding of the access token for the 'ath' claim.",
                    nameof(accessToken),
                    ex);
            }

#if NET6_0_OR_GREATER
            var tokenHash = SHA256.HashData(tokenBytes);
#else
            byte[] tokenHash;
            using (var sha256 = SHA256.Create())
            {
                tokenHash = sha256.ComputeHash(tokenBytes);
            }
#endif
            var tokenHashBase64 = Base64UrlEncoder.Encode(tokenHash);
            claims.Add(DpopClaimTypes.Ath, tokenHashBase64);
        }

        // Include a nonce if specified in options or provided by server
        if (_options.IncludeNonce || !string.IsNullOrEmpty(serverProvidedNonce))
        {
            string nonceValue = serverProvidedNonce ??
                _options.Nonce ?? GenerateRandomNonce();
            claims.Add(DpopClaimTypes.Nonce, nonceValue);
        }

        JsonObject jwk = GetAsymmetricPublicJwk(
            JsonWebKeyConverter.ConvertFromSecurityKey(_options.SigningCredentials.Key));

        var headerClaims = new Dictionary<string, object>
        {
            { "typ", "dpop+jwt" },
            { "jwk", jwk }
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            IncludeKeyIdInHeader = false,
            Claims = claims,
            AdditionalHeaderClaims = headerClaims,
            SigningCredentials = _options.SigningCredentials,
        };

        var handler = new JsonWebTokenHandler()
        {
            SetDefaultTimesOnTokenCreation = false,
        };

        var token = handler.CreateToken(tokenDescriptor);

        return token;
    }

    /// <summary>
    /// Builds a <see cref="JsonObject"/> containing only the public components of
    /// an asymmetric JWK (EC or RSA), suitable for the DPoP proof header.
    /// </summary>
    private static JsonObject GetAsymmetricPublicJwk(JsonWebKey key)
    {
        if (string.Equals(key.Kty, JsonWebAlgorithmsKeyTypes.EllipticCurve))
        {
            return new JsonObject
            {
                [JsonWebKeyParameterNames.Crv] = key.Crv,
                [JsonWebKeyParameterNames.Kty] = key.Kty,
                [JsonWebKeyParameterNames.X] = key.X,
                [JsonWebKeyParameterNames.Y] = key.Y,
            };
        }
        else if (string.Equals(key.Kty, JsonWebAlgorithmsKeyTypes.RSA))
        {
            return new JsonObject
            {
                [JsonWebKeyParameterNames.E] = key.E,
                [JsonWebKeyParameterNames.Kty] = key.Kty,
                [JsonWebKeyParameterNames.N] = key.N,
            };
        }
        else
        {
            throw new ArgumentException(
                $"Unsupported key type '{key.Kty}'. Only EC and RSA keys are supported for DPoP.");
        }
    }

    private static string GenerateRandomNonce()
    {
        var bytes = new byte[32];
#if NET6_0_OR_GREATER
        RandomNumberGenerator.Fill(bytes);
#else
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(bytes);
        }
#endif
        return Convert.ToBase64String(bytes);
    }
}
