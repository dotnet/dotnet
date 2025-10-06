// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Protocols.SignedHttpRequest
{
    /// <summary>
    /// A class which contains useful methods related to processing of SignedHttpRequest protocol.
    /// </summary>
    public static class SignedHttpRequestUtilities
    {
        /// <summary>
        /// Creates a "jwk" claim from a JsonWebKey representation of an asymmetric public key.
        /// </summary>
        /// <param name="jsonWebKey">JsonWebKey representation of an asymmetric public key.</param>
        /// <returns>A "jwk" claim as a JSON string.</returns>
        /// <remarks>https://datatracker.ietf.org/doc/html/rfc7800#section-3.2</remarks>
        public static string CreateJwkClaim(JsonWebKey jsonWebKey)
        {
            if (jsonWebKey == null)
                throw LogHelper.LogArgumentNullException(nameof(jsonWebKey));

            return $@"{{""{ConfirmationClaimTypes.Jwk}"":{jsonWebKey.RepresentAsAsymmetricPublicJwk()}}}";
        }

        /// <summary>
        /// Creates an authorization header using the SignedHttpRequest.
        /// </summary>
        /// <param name="signedHttpRequest">A signed http request.</param>
        /// <returns>A SignedHttpRequest value prefixed with the word "PoP".</returns>
        /// <remarks>https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-4.1</remarks>
        public static string CreateSignedHttpRequestHeader(string signedHttpRequest)
        {
            if (string.IsNullOrEmpty(signedHttpRequest))
                throw LogHelper.LogArgumentNullException(nameof(signedHttpRequest));

            return $"{SignedHttpRequestConstants.AuthorizationHeaderSchemeName} {signedHttpRequest}";
        }

        /// <summary>
        /// A helper method that converts <see cref="HttpRequestMessage"/> into <see cref="HttpRequestData"/> object.
        /// </summary>
        /// <param name="httpRequestMessage"><see cref="HttpRequestMessage"/> object that represents incoming or outgoing http request.</param>
        /// <returns><see cref="HttpRequestData"/> object</returns>
        public static async Task<HttpRequestData> ToHttpRequestDataAsync(this HttpRequestMessage httpRequestMessage)
        {
            if (httpRequestMessage == null)
                throw LogHelper.LogArgumentNullException(nameof(httpRequestMessage));

            var httpRequestData = new HttpRequestData()
            {
                Method = httpRequestMessage.Method?.ToString(),
                Uri = httpRequestMessage.RequestUri
            };

            httpRequestData.AppendHeaders(httpRequestMessage.Headers);

            if (httpRequestMessage.Content != null)
            {
                httpRequestData.Body = await httpRequestMessage.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                httpRequestData.AppendHeaders(httpRequestMessage.Content.Headers);
            }

            return httpRequestData;
        }

        internal static async Task<JsonWebKey> DecryptSymmetricPopKeyAsync(JsonWebTokenHandler jwtTokenHandler, string jwe, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(jwe))
                throw LogHelper.LogArgumentNullException(nameof(jwe));

            var jweJwt = jwtTokenHandler.ReadJsonWebToken(jwe);

            IEnumerable<SecurityKey> decryptionKeys;
            if (signedHttpRequestValidationContext.SignedHttpRequestValidationParameters.CnfDecryptionKeysResolverAsync != null)
                decryptionKeys = await signedHttpRequestValidationContext.SignedHttpRequestValidationParameters.CnfDecryptionKeysResolverAsync(jweJwt, cancellationToken).ConfigureAwait(false);
            else
                decryptionKeys = signedHttpRequestValidationContext.SignedHttpRequestValidationParameters.CnfDecryptionKeys;

            if (decryptionKeys == null || !decryptionKeys.Any())
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidPopKeyException(LogHelper.FormatInvariant(LogMessages.IDX23017)));

            var tokenDecryptionParameters = new TokenValidationParameters()
            {
                TokenDecryptionKeys = decryptionKeys,
                RequireSignedTokens = false,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = false,
            };

            try
            {
                var decryptedJson = jwtTokenHandler.DecryptToken(jweJwt, tokenDecryptionParameters);
                return new JsonWebKey(decryptedJson);
            }
            catch (Exception e)
            {
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidPopKeyException(LogHelper.FormatInvariant(LogMessages.IDX23018, string.Join(", ", decryptionKeys.Select(x => x?.KeyId ?? "Null")), e), e));
            }
        }
    }
}
