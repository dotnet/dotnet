//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Json;
using Microsoft.IdentityModel.Json.Linq;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Protocols.SignedHttpRequest
{
    /// <summary>
    /// A handler designed for creating and validating signed http requests. 
    /// </summary>
    /// <remarks>The handler implementation is based on 'A Method for Signing HTTP Requests for OAuth' specification.</remarks>
    public class SignedHttpRequestHandler
    {
        // (https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-3.2)
        // "Encodes the name and value of the header as "name: value" and appends it to the string buffer separated by a newline "\n" character."
        private readonly string _newlineSeparator = "\n";

        private readonly JsonWebTokenHandler _jwtTokenHandler = new JsonWebTokenHandler()
        {
            SetDefaultTimesOnTokenCreation = false
        };
        private readonly Uri _baseUriHelper = new Uri("http://localhost", UriKind.Absolute);
        private readonly HttpClient _defaultHttpClient = new HttpClient();

        #region SignedHttpRequest creation
        /// <summary>
        /// Creates a signed http request using the <paramref name="signedHttpRequestDescriptor"/>.
        /// </summary>
        /// <param name="signedHttpRequestDescriptor">A structure that wraps parameters needed for SignedHttpRequest creation.</param>
        /// <returns>A signed http request as a JWS in Compact Serialization Format.</returns>
        /// <remarks>Default <see cref="CallContext"/> will be created.</remarks>
        public string CreateSignedHttpRequest(SignedHttpRequestDescriptor signedHttpRequestDescriptor)
        {
            return CreateSignedHttpRequest(signedHttpRequestDescriptor, new CallContext());
        }

        /// <summary>
        /// Creates a signed http request using the <paramref name="signedHttpRequestDescriptor"/>.
        /// </summary>
        /// <param name="signedHttpRequestDescriptor">A structure that wraps parameters needed for SignedHttpRequest creation.</param>
        /// <param name="callContext" >An opaque context used to store work and logs when working with authentication artifacts.</param>
        /// <returns>A signed http request as a JWS in Compact Serialization Format.</returns>
        public string CreateSignedHttpRequest(SignedHttpRequestDescriptor signedHttpRequestDescriptor, CallContext callContext)
        {
            if (signedHttpRequestDescriptor == null)
                throw LogHelper.LogArgumentNullException(nameof(signedHttpRequestDescriptor));

            if (signedHttpRequestDescriptor.SigningCredentials == null)
                throw LogHelper.LogArgumentNullException(nameof(signedHttpRequestDescriptor.SigningCredentials));

            var payload = CreateHttpRequestPayload(signedHttpRequestDescriptor, callContext);
            var header = new JObject();
            if (signedHttpRequestDescriptor.AdditionalHeaderClaims != null && signedHttpRequestDescriptor.AdditionalHeaderClaims.Count != 0)
            {
                if (signedHttpRequestDescriptor.AdditionalHeaderClaims.Keys.Intersect(JwtTokenUtilities.DefaultHeaderParameters, StringComparer.OrdinalIgnoreCase).Any())
                    throw LogHelper.LogExceptionMessage(new SecurityTokenException(LogHelper.FormatInvariant(JsonWebTokens.LogMessages.IDX14116, LogHelper.MarkAsNonPII(nameof(signedHttpRequestDescriptor.AdditionalHeaderClaims)), LogHelper.MarkAsNonPII(string.Join(", ", JwtTokenUtilities.DefaultHeaderParameters)))));

                header.Merge(JObject.FromObject(signedHttpRequestDescriptor.AdditionalHeaderClaims));
            }

            // set the "typ" header claim to "pop"
            // https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-6.2
            header[JwtHeaderParameterNames.Alg] = signedHttpRequestDescriptor.SigningCredentials.Algorithm;
            header[JwtHeaderParameterNames.Typ] = SignedHttpRequestConstants.TokenType;

            var message = Base64UrlEncoder.Encode(Encoding.UTF8.GetBytes(header.ToString(Formatting.None))) + "." + Base64UrlEncoder.Encode(Encoding.UTF8.GetBytes(payload));

            return message + "." + JwtTokenUtilities.CreateEncodedSignature(message, signedHttpRequestDescriptor.SigningCredentials, false);
        }

        /// <summary>
        /// Creates a JSON representation of a HttpRequest payload.
        /// </summary>
        /// <param name="signedHttpRequestDescriptor">A structure that wraps parameters needed for SignedHttpRequest creation.</param>
        /// <param name="callContext" >An opaque context used to store work and logs when working with authentication artifacts.</param> 
        /// <returns>A JSON representation of an HttpRequest payload.</returns>
        /// <remarks>
        /// Users can utilize <see cref="SignedHttpRequestDescriptor.AdditionalPayloadClaims"/> to create additional claim(s) and add them to the signed http request.
        /// </remarks>
        protected virtual string CreateHttpRequestPayload(SignedHttpRequestDescriptor signedHttpRequestDescriptor, CallContext callContext)
        {
            if (signedHttpRequestDescriptor == null)
                throw LogHelper.LogArgumentNullException(nameof(signedHttpRequestDescriptor));

            Dictionary<string, object> payload = new Dictionary<string, object>();

            AddAtClaim(payload, signedHttpRequestDescriptor);

            if (signedHttpRequestDescriptor.SignedHttpRequestCreationParameters.CreateTs)
                AddTsClaim(payload, signedHttpRequestDescriptor);

            if (signedHttpRequestDescriptor.SignedHttpRequestCreationParameters.CreateM)
                AddMClaim(payload, signedHttpRequestDescriptor);

            if (signedHttpRequestDescriptor.SignedHttpRequestCreationParameters.CreateU)
                AddUClaim(payload, signedHttpRequestDescriptor);

            if (signedHttpRequestDescriptor.SignedHttpRequestCreationParameters.CreateP)
                AddPClaim(payload, signedHttpRequestDescriptor);

            if (signedHttpRequestDescriptor.SignedHttpRequestCreationParameters.CreateQ)
                AddQClaim(payload, signedHttpRequestDescriptor);

            if (signedHttpRequestDescriptor.SignedHttpRequestCreationParameters.CreateH)
                AddHClaim(payload, signedHttpRequestDescriptor);

            if (signedHttpRequestDescriptor.SignedHttpRequestCreationParameters.CreateB)
                AddBClaim(payload, signedHttpRequestDescriptor);

            if (signedHttpRequestDescriptor.SignedHttpRequestCreationParameters.CreateNonce)
                AddNonceClaim(payload, signedHttpRequestDescriptor);

            if (signedHttpRequestDescriptor.SignedHttpRequestCreationParameters.CreateCnf)
                AddCnfClaim(payload, signedHttpRequestDescriptor);

            if (signedHttpRequestDescriptor.AdditionalPayloadClaims != null && signedHttpRequestDescriptor.AdditionalPayloadClaims.Any())
            {
                foreach (var additionalPayloadClaim in signedHttpRequestDescriptor.AdditionalPayloadClaims)
                {
                    if (!payload.ContainsKey(additionalPayloadClaim.Key))
                        payload.Add(additionalPayloadClaim.Key, additionalPayloadClaim.Value);
                }
            }

            return JObject.FromObject(payload).ToString(Formatting.None);
        }

        /// <summary>
        /// Adds the 'at' claim to the <paramref name="payload"/>.
        /// </summary>
        /// <param name="payload">HttpRequest payload represented as a <see cref="Dictionary{TKey, TValue}"/>.</param>
        /// <param name="signedHttpRequestDescriptor">A structure that wraps parameters needed for SignedHttpRequest creation.</param>
        internal virtual void AddAtClaim(Dictionary<string, object> payload, SignedHttpRequestDescriptor signedHttpRequestDescriptor)
        {
            if (payload == null)
                throw LogHelper.LogArgumentNullException(nameof(payload));

            payload.Add(SignedHttpRequestClaimTypes.At, signedHttpRequestDescriptor.AccessToken);
        }

        /// <summary>
        /// Adds the 'ts' claim to the <paramref name="payload"/>.
        /// </summary>
        /// <param name="payload">HttpRequest payload represented as a <see cref="Dictionary{TKey, TValue}"/>.</param>
        /// <param name="signedHttpRequestDescriptor">A structure that wraps parameters needed for SignedHttpRequest creation.</param>
        /// <remarks>
        /// This method will be executed only if <see cref="SignedHttpRequestCreationParameters.CreateTs"/> is set to <c>true</c>.
        /// </remarks>    
        internal virtual void AddTsClaim(Dictionary<string, object> payload, SignedHttpRequestDescriptor signedHttpRequestDescriptor)
        {
            if (payload == null)
                throw LogHelper.LogArgumentNullException(nameof(payload));

            var signedHttpRequestCreationTime = DateTime.UtcNow.Add(signedHttpRequestDescriptor.SignedHttpRequestCreationParameters.TimeAdjustment);
            payload.Add(SignedHttpRequestClaimTypes.Ts, EpochTime.GetIntDate(signedHttpRequestCreationTime));
        }

        /// <summary>
        /// Adds the 'm' claim to the <paramref name="payload"/>.
        /// </summary>
        /// <param name="payload">HttpRequest payload represented as a <see cref="Dictionary{TKey, TValue}"/>.</param>
        /// <param name="signedHttpRequestDescriptor">A structure that wraps parameters needed for SignedHttpRequest creation.</param>
        /// <remarks>
        /// This method will be executed only if <see cref="SignedHttpRequestCreationParameters.CreateM"/> is set to <c>true</c>.
        /// </remarks>   
        internal virtual void AddMClaim(Dictionary<string, object> payload, SignedHttpRequestDescriptor signedHttpRequestDescriptor)
        {
            if (payload == null)
                throw LogHelper.LogArgumentNullException(nameof(payload));

            var httpMethod = signedHttpRequestDescriptor.HttpRequestData.Method;

            if (string.IsNullOrEmpty(httpMethod))
                throw LogHelper.LogArgumentNullException(nameof(signedHttpRequestDescriptor.HttpRequestData.Method));

            if (!httpMethod.ToUpperInvariant().Equals(httpMethod))
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestCreationException(LogHelper.FormatInvariant(LogMessages.IDX23002, LogHelper.MarkAsNonPII(httpMethod))));

            payload.Add(SignedHttpRequestClaimTypes.M, httpMethod);
        }

        /// <summary>
        /// Adds the 'u' claim to the <paramref name="payload"/>.
        /// </summary>
        /// <param name="payload">HttpRequest payload represented as a <see cref="Dictionary{TKey, TValue}"/>.</param>
        /// <param name="signedHttpRequestDescriptor">A structure that wraps parameters needed for SignedHttpRequest creation.</param>
        /// <remarks>
        /// This method will be executed only if <see cref="SignedHttpRequestCreationParameters.CreateU"/> is set to <c>true</c>.
        /// </remarks>  
        internal virtual void AddUClaim(Dictionary<string, object> payload, SignedHttpRequestDescriptor signedHttpRequestDescriptor)
        {
            if (payload == null)
                throw LogHelper.LogArgumentNullException(nameof(payload));

            var httpRequestUri = signedHttpRequestDescriptor.HttpRequestData.Uri;

            if (httpRequestUri == null)
                throw LogHelper.LogArgumentNullException(nameof(signedHttpRequestDescriptor.HttpRequestData.Uri));

            if (!httpRequestUri.IsAbsoluteUri)
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestCreationException(LogHelper.FormatInvariant(LogMessages.IDX23001, httpRequestUri.OriginalString)));

            // https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-3
            // u claim: The HTTP URL host component as a JSON string. This MAY include the port separated from the host by a colon in host:port format.
            // Including the port if it not the default port for the httpRequestUri scheme.
            var httpUrlHostComponent = httpRequestUri.Host;
            if (!httpRequestUri.IsDefaultPort)
                httpUrlHostComponent = $"{httpUrlHostComponent}:{httpRequestUri.Port}";

            payload.Add(SignedHttpRequestClaimTypes.U, httpUrlHostComponent);
        }

        /// <summary>
        /// Adds the 'm' claim to the <paramref name="payload"/>.
        /// </summary>
        /// <param name="payload">HttpRequest payload represented as a <see cref="Dictionary{TKey, TValue}"/>.</param>
        /// <param name="signedHttpRequestDescriptor">A structure that wraps parameters needed for SignedHttpRequest creation.</param>
        /// <remarks>
        /// This method will be executed only if <see cref="SignedHttpRequestCreationParameters.CreateP"/> is set to <c>true</c>.
        /// </remarks>  
        internal virtual void AddPClaim(Dictionary<string, object> payload, SignedHttpRequestDescriptor signedHttpRequestDescriptor)
        {
            if (payload == null)
                throw LogHelper.LogArgumentNullException(nameof(payload));

            var httpRequestUri = signedHttpRequestDescriptor.HttpRequestData.Uri;

            if (httpRequestUri == null)
                throw LogHelper.LogArgumentNullException(nameof(signedHttpRequestDescriptor.HttpRequestData.Uri));

            httpRequestUri = EnsureAbsoluteUri(httpRequestUri);

            payload.Add(SignedHttpRequestClaimTypes.P, httpRequestUri.AbsolutePath);
        }

        /// <summary>
        /// Adds the 'q' claim to the <paramref name="payload"/>.
        /// </summary>
        /// <param name="payload">HttpRequest payload represented as a <see cref="Dictionary{TKey, TValue}"/>.</param>
        /// <param name="signedHttpRequestDescriptor">A structure that wraps parameters needed for SignedHttpRequest creation.</param>
        /// <remarks>
        /// This method will be executed only if <see cref="SignedHttpRequestCreationParameters.CreateQ"/> is set to <c>true</c>.
        /// </remarks>  
        internal virtual void AddQClaim(Dictionary<string, object> payload, SignedHttpRequestDescriptor signedHttpRequestDescriptor)
        {
            if (payload == null)
                throw LogHelper.LogArgumentNullException(nameof(payload));

            var httpRequestUri = signedHttpRequestDescriptor.HttpRequestData.Uri;

            if (httpRequestUri == null)
                throw LogHelper.LogArgumentNullException(nameof(signedHttpRequestDescriptor.HttpRequestData.Uri));

            httpRequestUri = EnsureAbsoluteUri(httpRequestUri);
            var sanitizedQueryParams = SanitizeQueryParams(httpRequestUri);

            StringBuilder stringBuffer = new StringBuilder();
            List<string> queryParamNameList = new List<string>();
            try
            {
                var firstQueryParam = true;
                foreach (var queryParam in sanitizedQueryParams)
                {
                    if (!firstQueryParam)
                        stringBuffer.Append("&");

                    stringBuffer.Append($"{queryParam.Key}={queryParam.Value}");

                    queryParamNameList.Add(queryParam.Key);
                    firstQueryParam = false;
                }

                var base64UrlEncodedHash = CalculateBase64UrlEncodedHash(stringBuffer.ToString());
                payload.Add(SignedHttpRequestClaimTypes.Q, new List<object>() { queryParamNameList, base64UrlEncodedHash });
            }
            catch (Exception e)
            {
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestCreationException(LogHelper.FormatInvariant(LogMessages.IDX23008, LogHelper.MarkAsNonPII(SignedHttpRequestClaimTypes.Q), e), e));
            }
        }

        /// <summary>
        /// Adds the 'h' claim to the <paramref name="payload"/>.
        /// </summary>
        /// <param name="payload">HttpRequest payload represented as a <see cref="Dictionary{TKey, TValue}"/>.</param>
        /// <param name="signedHttpRequestDescriptor">A structure that wraps parameters needed for SignedHttpRequest creation.</param>
        /// <remarks>
        /// This method will be executed only if <see cref="SignedHttpRequestCreationParameters.CreateH"/> is set to <c>true</c>.
        /// </remarks>  
        internal virtual void AddHClaim(Dictionary<string, object> payload, SignedHttpRequestDescriptor signedHttpRequestDescriptor)
        {
            if (payload == null)
                throw LogHelper.LogArgumentNullException(nameof(payload));

            var sanitizedHeaders = SanitizeHeaders(signedHttpRequestDescriptor.HttpRequestData.Headers);
            StringBuilder stringBuffer = new StringBuilder();
            List<string> headerNameList = new List<string>();
            try
            {
                var firstHeader = true;
                foreach (var header in sanitizedHeaders)
                {
                    var headerName = header.Key.ToLowerInvariant();
                    headerNameList.Add(headerName);

                    if (!firstHeader)
                        stringBuffer.Append(_newlineSeparator);

                    stringBuffer.Append($"{headerName}: {header.Value}");
                    firstHeader = false;
                }

                var base64UrlEncodedHash = CalculateBase64UrlEncodedHash(stringBuffer.ToString());
                payload.Add(SignedHttpRequestClaimTypes.H, new List<object>() { headerNameList, base64UrlEncodedHash });
            }
            catch (Exception e)
            {
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestCreationException(LogHelper.FormatInvariant(LogMessages.IDX23008, LogHelper.MarkAsNonPII(SignedHttpRequestClaimTypes.H), e), e));
            }
        }

        /// <summary>
        /// Adds the 'b' claim to the <paramref name="payload"/>.
        /// </summary>
        /// <param name="payload">HttpRequest payload represented as a <see cref="Dictionary{TKey, TValue}"/>.</param>
        /// <param name="signedHttpRequestDescriptor">A structure that wraps parameters needed for SignedHttpRequest creation.</param>
        /// <remarks>
        /// This method will be executed only if <see cref="SignedHttpRequestCreationParameters.CreateB"/> is set to <c>true</c>.
        /// </remarks> 
        internal virtual void AddBClaim(Dictionary<string, object> payload, SignedHttpRequestDescriptor signedHttpRequestDescriptor)
        {
            if (payload == null)
                throw LogHelper.LogArgumentNullException(nameof(payload));

            var httpRequestBody = signedHttpRequestDescriptor.HttpRequestData.Body;

            if (httpRequestBody == null)
                httpRequestBody = new byte[0];

            try
            {
                var base64UrlEncodedHash = CalculateBase64UrlEncodedHash(httpRequestBody);
                payload.Add(SignedHttpRequestClaimTypes.B, base64UrlEncodedHash);
            }
            catch (Exception e)
            {
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestCreationException(LogHelper.FormatInvariant(LogMessages.IDX23008, LogHelper.MarkAsNonPII(SignedHttpRequestClaimTypes.B), e), e));
            }
        }

        /// <summary>
        /// Adds the 'nonce' claim to the <paramref name="payload"/>.
        /// </summary>
        /// <param name="payload">HttpRequest payload represented as a <see cref="Dictionary{TKey, TValue}"/>.</param>
        /// <param name="signedHttpRequestDescriptor">A structure that wraps parameters needed for SignedHttpRequest creation.</param>
        /// <remarks>
        /// This method will be executed only if <see cref="SignedHttpRequestCreationParameters.CreateNonce"/> is set to <c>true</c>.
        /// Users can utilize <see cref="SignedHttpRequestDescriptor.CustomNonceValue"/> to provide a custom nonce value.
        /// </remarks>
        internal virtual void AddNonceClaim(Dictionary<string, object> payload, SignedHttpRequestDescriptor signedHttpRequestDescriptor)
        {
            if (payload == null)
                throw LogHelper.LogArgumentNullException(nameof(payload));

            if (!string.IsNullOrEmpty(signedHttpRequestDescriptor.CustomNonceValue))
                payload.Add(SignedHttpRequestClaimTypes.Nonce, signedHttpRequestDescriptor.CustomNonceValue);
            else
                payload.Add(SignedHttpRequestClaimTypes.Nonce, Guid.NewGuid().ToString("N"));
        }

        /// <summary>
        /// Adds the 'cnf' claim to the <paramref name="payload"/>.
        /// </summary>
        /// <param name="payload">HttpRequest payload represented as a <see cref="Dictionary{TKey, TValue}"/>.</param>
        /// <param name="signedHttpRequestDescriptor">A structure that wraps parameters needed for SignedHttpRequest creation.</param>
        /// <remarks>
        /// If <see cref="SignedHttpRequestDescriptor.CnfClaimValue"/> is not null or empty, its value will be used as a "cnf" claim value.
        /// Otherwise, a "cnf" claim value will be derived from the <see cref="SigningCredentials"/>.<see cref="SecurityKey"/> member of <paramref name="signedHttpRequestDescriptor"/>.
        /// </remarks>
        internal virtual void AddCnfClaim(Dictionary<string, object> payload, SignedHttpRequestDescriptor signedHttpRequestDescriptor)
        {
            if (payload == null)
                throw LogHelper.LogArgumentNullException(nameof(payload));
            try
            {
                if (!string.IsNullOrEmpty(signedHttpRequestDescriptor.CnfClaimValue))
                {
                    payload.Add(ConfirmationClaimTypes.Cnf, JObject.Parse(signedHttpRequestDescriptor.CnfClaimValue));
                }
                else
                {
                    var signedHttpRequestSigningKey = signedHttpRequestDescriptor.SigningCredentials.Key;
                    JsonWebKey jsonWebKey;
                    if (signedHttpRequestSigningKey is JsonWebKey jwk)
                        jsonWebKey = jwk;
                    // create a JsonWebKey from an X509SecurityKey, represented as an RsaSecurityKey. 
                    else if (signedHttpRequestSigningKey is X509SecurityKey x509SecurityKey)
                        jsonWebKey = JsonWebKeyConverter.ConvertFromX509SecurityKey(x509SecurityKey, true);
                    else if (signedHttpRequestSigningKey is AsymmetricSecurityKey asymmetricSecurityKey)
                        jsonWebKey = JsonWebKeyConverter.ConvertFromSecurityKey(asymmetricSecurityKey);
                    else
                        throw LogHelper.LogExceptionMessage(new SignedHttpRequestCreationException(LogHelper.FormatInvariant(LogMessages.IDX23032, LogHelper.MarkAsNonPII(signedHttpRequestSigningKey != null ? signedHttpRequestSigningKey.GetType().ToString() : "null"))));

                    // set the jwk thumbprint as the Kid
                    jsonWebKey.Kid = Base64UrlEncoder.Encode(jsonWebKey.ComputeJwkThumbprint());
                    payload.Add(ConfirmationClaimTypes.Cnf, JObject.Parse(SignedHttpRequestUtilities.CreateJwkClaim(jsonWebKey)));
                }
            }
            catch (Exception e)
            {
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestCreationException(LogHelper.FormatInvariant(LogMessages.IDX23008, LogHelper.MarkAsNonPII(ConfirmationClaimTypes.Cnf), e), e));
            }
        }
        #endregion

        #region SignedHttpRequest validation
        /// <summary>
        /// Validates a signed http request using the <paramref name="signedHttpRequestValidationContext"/>.
        /// </summary>
        /// <param name="signedHttpRequestValidationContext">A structure that wraps parameters needed for SignedHttpRequest validation.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>A <see cref="SignedHttpRequestValidationResult"/>. 
        /// <see cref="TokenValidationResult.IsValid"/> will be <c>true</c> if the signed http request was successfully validated, <c>false</c> otherwise.
        /// </returns>
        public async Task<SignedHttpRequestValidationResult> ValidateSignedHttpRequestAsync(SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken)
        {
            try
            {
                if (signedHttpRequestValidationContext == null)
                    throw LogHelper.LogArgumentNullException(nameof(signedHttpRequestValidationContext));

                // read signed http request as JWT
                var signedHttpRequest = ReadSignedHttpRequest(signedHttpRequestValidationContext);

                // read access token ("at")
                if (!signedHttpRequest.TryGetPayloadValue(SignedHttpRequestClaimTypes.At, out string accessToken) || string.IsNullOrEmpty(accessToken))
                    throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidAtClaimException(LogHelper.FormatInvariant(LogMessages.IDX23003, LogHelper.MarkAsNonPII(SignedHttpRequestClaimTypes.At))));

                // validate access token ("at")
                var tokenValidationResult = await ValidateAccessTokenAsync(accessToken, signedHttpRequestValidationContext, cancellationToken).ConfigureAwait(false);
                if (!tokenValidationResult.IsValid)
                    throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidAtClaimException(LogHelper.FormatInvariant(LogMessages.IDX23013, tokenValidationResult.Exception), tokenValidationResult.Exception));

                // resolve PoP key (confirmation key)
                var popKey = await ResolvePopKeyAsync(signedHttpRequest, tokenValidationResult.SecurityToken as JsonWebToken, signedHttpRequestValidationContext, cancellationToken).ConfigureAwait(false);

                // validate signed http request signature
                signedHttpRequest.SigningKey = await ValidateSignatureAsync(signedHttpRequest, popKey, signedHttpRequestValidationContext, cancellationToken).ConfigureAwait(false);

                // validate nonce claim
                ValidateNonceAsync(signedHttpRequest, popKey, signedHttpRequestValidationContext, cancellationToken);

                // validate signed http request payload
                var validatedSignedHttpRequest = await ValidateSignedHttpRequestPayloadAsync(signedHttpRequest, signedHttpRequestValidationContext, cancellationToken).ConfigureAwait(false);

                return new SignedHttpRequestValidationResult()
                {
                    IsValid = true,
                    AccessTokenValidationResult = tokenValidationResult,
                    SignedHttpRequest = signedHttpRequest.EncodedToken,
                    ValidatedSignedHttpRequest = validatedSignedHttpRequest
                };
            }
            catch (Exception ex)
            {
                return new SignedHttpRequestValidationResult()
                {
                    IsValid = false,
                    Exception = ex,
                };
            }
        }

        /// <summary>
        /// Reads a SignedHttpRequest as a <see cref="JsonWebToken"/>.
        /// </summary>
        /// <param name="signedHttpRequestValidationContext">A structure that wraps parameters needed for SignedHttpRequest validation.</param>
        /// <returns>A SignedHttpRequest as a <see cref="JsonWebToken"/>.</returns>
        internal virtual JsonWebToken ReadSignedHttpRequest(SignedHttpRequestValidationContext signedHttpRequestValidationContext)
        {
            return _jwtTokenHandler.ReadJsonWebToken(signedHttpRequestValidationContext.SignedHttpRequest);
        }

        /// <summary>
        /// Validates an access token ("at").
        /// </summary>
        /// <param name="accessToken">An access token ("at") as a JWT.</param>
        /// <param name="signedHttpRequestValidationContext">A structure that wraps parameters needed for SignedHttpRequest validation.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>A <see cref="TokenValidationResult"/>.</returns>
        internal virtual Task<TokenValidationResult> ValidateAccessTokenAsync(string accessToken, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(accessToken))
                throw LogHelper.LogArgumentNullException(nameof(accessToken));
            var tokenValidationResult = _jwtTokenHandler.ValidateToken(accessToken, signedHttpRequestValidationContext.AccessTokenValidationParameters);
            return Task.FromResult(tokenValidationResult);
        }

        /// <summary>
        /// Validates signed http request payload.
        /// </summary>
        /// <param name="signedHttpRequest">A SignedHttpRequest.</param>
        /// <param name="signedHttpRequestValidationContext">A structure that wraps parameters needed for SignedHttpRequest validation.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        /// <remarks>
        /// The library doesn't provide any caching logic for replay validation purposes.
        /// <see cref="SignedHttpRequestValidationParameters.ReplayValidatorAsync"/> delegate can be utilized for replay validation
        /// </remarks>
        protected virtual async Task<SecurityToken> ValidateSignedHttpRequestPayloadAsync(SecurityToken signedHttpRequest, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken)
        {
            if (signedHttpRequest == null)
                throw LogHelper.LogArgumentNullException(nameof(signedHttpRequest));

            if (signedHttpRequestValidationContext == null)
                throw LogHelper.LogArgumentNullException(nameof(signedHttpRequestValidationContext));

            if (!(signedHttpRequest is JsonWebToken jwtSignedHttpRequest))
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestValidationException(LogHelper.FormatInvariant(LogMessages.IDX23030, LogHelper.MarkAsNonPII(signedHttpRequest.GetType()), LogHelper.MarkAsNonPII(typeof(JsonWebToken)), signedHttpRequest)));

            var validationParameters = signedHttpRequestValidationContext.SignedHttpRequestValidationParameters;

            if (validationParameters.ReplayValidatorAsync != null)
                await signedHttpRequestValidationContext.SignedHttpRequestValidationParameters.ReplayValidatorAsync(jwtSignedHttpRequest, signedHttpRequestValidationContext, cancellationToken).ConfigureAwait(false);

            if (ShouldValidate(jwtSignedHttpRequest, validationParameters.ValidateTs, ValidateIfPresent(validationParameters, SignedHttpRequestClaimTypes.Ts), SignedHttpRequestClaimTypes.Ts))
                ValidateTsClaim(jwtSignedHttpRequest, signedHttpRequestValidationContext);

            if (ShouldValidate(jwtSignedHttpRequest, validationParameters.ValidateM, ValidateIfPresent(validationParameters, SignedHttpRequestClaimTypes.M), SignedHttpRequestClaimTypes.M))
                ValidateMClaim(jwtSignedHttpRequest, signedHttpRequestValidationContext);

            if (ShouldValidate(jwtSignedHttpRequest, validationParameters.ValidateU, ValidateIfPresent(validationParameters, SignedHttpRequestClaimTypes.U), SignedHttpRequestClaimTypes.U))
                ValidateUClaim(jwtSignedHttpRequest, signedHttpRequestValidationContext);

            if (ShouldValidate(jwtSignedHttpRequest, validationParameters.ValidateP, ValidateIfPresent(validationParameters, SignedHttpRequestClaimTypes.P), SignedHttpRequestClaimTypes.P))
                ValidatePClaim(jwtSignedHttpRequest, signedHttpRequestValidationContext);

            if (ShouldValidate(jwtSignedHttpRequest, validationParameters.ValidateQ, ValidateIfPresent(validationParameters, SignedHttpRequestClaimTypes.Q), SignedHttpRequestClaimTypes.Q))
                ValidateQClaim(jwtSignedHttpRequest, signedHttpRequestValidationContext);

            if (ShouldValidate(jwtSignedHttpRequest, validationParameters.ValidateH, ValidateIfPresent(validationParameters, SignedHttpRequestClaimTypes.H), SignedHttpRequestClaimTypes.H))
                ValidateHClaim(jwtSignedHttpRequest, signedHttpRequestValidationContext);

            if (ShouldValidate(jwtSignedHttpRequest, validationParameters.ValidateB, ValidateIfPresent(validationParameters, SignedHttpRequestClaimTypes.B), SignedHttpRequestClaimTypes.B))
                ValidateBClaim(jwtSignedHttpRequest, signedHttpRequestValidationContext);

            return jwtSignedHttpRequest;
        }

        /// <summary>
        /// Determine if validation of a claim should happen.
        /// </summary>
        /// <param name="jwtSignedHttpRequest">The request being considered to validate the claim on.</param>
        /// <param name="validateClaim">Force validation to always occur.</param>
        /// <param name="validateIfPresent">Validate if the claim is present.</param>
        /// <param name="claimName">The name of the claim to validate.</param>
        /// <returns>Whether the given claim should be validated.</returns>
        internal virtual bool ShouldValidate(JsonWebToken jwtSignedHttpRequest, bool validateClaim, bool validateIfPresent, string claimName)
        {
            return validateClaim || (validateIfPresent && jwtSignedHttpRequest.TryGetClaim(claimName, out var claimValue) && claimValue != null);
        }

        private static bool ValidateIfPresent(SignedHttpRequestValidationParameters validationParameters, string claim)
        {
            return validationParameters.ValidatePresentClaims &&
                validationParameters.ClaimsToValidateWhenPresent != null &&
                validationParameters.ClaimsToValidateWhenPresent.Contains(claim);
        }

        /// <summary>
        /// Validates the signature of the signed http request using <paramref name="popKey"/>.
        /// </summary>
        /// <param name="signedHttpRequest">A SignedHttpRequest.</param>
        /// <param name="popKey">A Pop key used to validate the signed http request signature.</param>
        /// <param name="signedHttpRequestValidationContext">A structure that wraps parameters needed for SignedHttpRequest validation.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>A PoP <see cref="SecurityKey"/> that validates signature of the <paramref name="signedHttpRequest"/>.</returns>
        internal virtual async Task<SecurityKey> ValidateSignatureAsync(JsonWebToken signedHttpRequest, SecurityKey popKey, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken)
        {
            if (signedHttpRequestValidationContext.SignedHttpRequestValidationParameters.SignatureValidatorAsync != null)
                return await signedHttpRequestValidationContext.SignedHttpRequestValidationParameters.SignatureValidatorAsync(popKey, signedHttpRequest, signedHttpRequestValidationContext, cancellationToken).ConfigureAwait(false);

            if (popKey == null)
                throw LogHelper.LogArgumentNullException(nameof(popKey));

            if (popKey.CryptoProviderFactory == null)
                throw LogHelper.LogArgumentNullException(nameof(popKey.CryptoProviderFactory));

            try
            {
                if (popKey.CryptoProviderFactory.IsSupportedAlgorithm(signedHttpRequest.Alg, popKey))
                {
                    SignatureProvider signatureProvider = null;
                    try
                    {
                        signatureProvider = popKey.CryptoProviderFactory.CreateForVerifying(popKey, signedHttpRequest.Alg, false);
                        if (signatureProvider == null)
                            throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant(Tokens.LogMessages.IDX10636, popKey.ToString(), LogHelper.MarkAsNonPII(signedHttpRequest.Alg))));

                        if (signatureProvider.Verify(Encoding.UTF8.GetBytes(signedHttpRequest.EncodedHeader + "." + signedHttpRequest.EncodedPayload), Base64UrlEncoder.DecodeBytes(signedHttpRequest.EncodedSignature)))
                            return popKey;
                    }
                    finally
                    {
                        if (signatureProvider != null)
                            popKey.CryptoProviderFactory.ReleaseSignatureProvider(signatureProvider);
                    }
                }
            }
            catch (Exception ex)
            {
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidSignatureException(LogHelper.FormatInvariant(LogMessages.IDX23009, ex.ToString()), ex));
            }

            throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidSignatureException(LogHelper.FormatInvariant(LogMessages.IDX23034, signedHttpRequest.EncodedToken)));
        }

        /// <summary>
        /// Validates the nonce claim of the signed http request.
        /// </summary>
        /// <param name="signedHttpRequest">A SignedHttpRequest.</param>
        /// <param name="popKey">A Pop key used to validate the signed http request nonce signature.</param>
        /// <param name="signedHttpRequestValidationContext">A structure that wraps parameters needed for SignedHttpRequest validation.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        internal virtual void ValidateNonceAsync(JsonWebToken signedHttpRequest, SecurityKey popKey, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken)
        {
            try
            {
                if (signedHttpRequestValidationContext.SignedHttpRequestValidationParameters.NonceValidatorAsync != null)
                    if (!signedHttpRequestValidationContext.SignedHttpRequestValidationParameters.NonceValidatorAsync(popKey, signedHttpRequest, signedHttpRequestValidationContext, cancellationToken))
                        throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidNonceClaimException("SignedHttpRequest nonce validation failed."));
            }
            catch (Exception ex)
            {
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidNonceClaimException(LogHelper.FormatInvariant(LogMessages.IDX23036, ex.ToString()), ex));
            }
        }

        /// <summary>
        /// Validates the signed http request lifetime ("ts").
        /// </summary>
        /// <param name="signedHttpRequest">A SignedHttpRequest.</param>
        /// <param name="signedHttpRequestValidationContext">A structure that wraps parameters needed for SignedHttpRequest validation.</param>
        /// <remarks>
        /// This method will be executed only if <see cref="SignedHttpRequestValidationParameters.ValidateTs"/> is set to <c>true</c> or a ts claim is present and
        /// <see cref="SignedHttpRequestValidationParameters.ValidatePresentClaims"/> is set to true and <see cref="SignedHttpRequestClaimTypes.Ts"/> is in
        /// <see cref="SignedHttpRequestValidationParameters.ClaimsToValidateWhenPresent"/>.
        /// </remarks>
        internal virtual void ValidateTsClaim(JsonWebToken signedHttpRequest, SignedHttpRequestValidationContext signedHttpRequestValidationContext)
        {
            if (!signedHttpRequest.TryGetPayloadValue(SignedHttpRequestClaimTypes.Ts, out long tsClaimValue))
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidTsClaimException(LogHelper.FormatInvariant(LogMessages.IDX23003, LogHelper.MarkAsNonPII(SignedHttpRequestClaimTypes.Ts))));

            DateTime utcNow = DateTime.UtcNow;
            DateTime signedHttpRequestCreationTime = EpochTime.DateTime(tsClaimValue);
            DateTime signedHttpRequestExpirationTime = signedHttpRequestCreationTime.Add(signedHttpRequestValidationContext.SignedHttpRequestValidationParameters.SignedHttpRequestLifetime);

            if (utcNow > signedHttpRequestExpirationTime)
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidTsClaimException(LogHelper.FormatInvariant(LogMessages.IDX23010, LogHelper.MarkAsNonPII(utcNow), LogHelper.MarkAsNonPII(signedHttpRequestExpirationTime))));
        }

        /// <summary>
        /// Validates the signed http request "m" claim.
        /// </summary>
        /// <param name="signedHttpRequest">A SignedHttpRequest.</param>
        /// <param name="signedHttpRequestValidationContext">A structure that wraps parameters needed for SignedHttpRequest validation.</param>
        /// <remarks>
        /// This method will be executed only if <see cref="SignedHttpRequestValidationParameters.ValidateM"/> is set to <c>true</c> or a m claim is present and
        /// <see cref="SignedHttpRequestValidationParameters.ValidatePresentClaims"/> is set to true and <see cref="SignedHttpRequestClaimTypes.M"/> is in
        /// <see cref="SignedHttpRequestValidationParameters.ClaimsToValidateWhenPresent"/>.
        /// </remarks>
        internal virtual void ValidateMClaim(JsonWebToken signedHttpRequest, SignedHttpRequestValidationContext signedHttpRequestValidationContext)
        {
            var expectedHttpMethod = signedHttpRequestValidationContext.HttpRequestData.Method;

            if (string.IsNullOrEmpty(expectedHttpMethod))
                throw LogHelper.LogArgumentNullException(nameof(expectedHttpMethod));

            if (!signedHttpRequest.TryGetPayloadValue(SignedHttpRequestClaimTypes.M, out string httpMethod) || httpMethod == null)
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidMClaimException(LogHelper.FormatInvariant(LogMessages.IDX23003, LogHelper.MarkAsNonPII(SignedHttpRequestClaimTypes.M))));

            // "get " is functionally the same as "GET".
            // different implementations might use differently formatted http verbs and we shouldn't fault.
            httpMethod = httpMethod.Trim();
            expectedHttpMethod = expectedHttpMethod.Trim();
            if (!string.Equals(expectedHttpMethod, httpMethod, StringComparison.OrdinalIgnoreCase))
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidMClaimException(LogHelper.FormatInvariant(LogMessages.IDX23011, LogHelper.MarkAsNonPII(SignedHttpRequestClaimTypes.M), LogHelper.MarkAsNonPII(expectedHttpMethod), LogHelper.MarkAsNonPII(httpMethod))));
        }

        /// <summary>
        /// Validates the signed http request "u" claim. 
        /// </summary>
        /// <param name="signedHttpRequest">A SignedHttpRequest.</param>
        /// <param name="signedHttpRequestValidationContext">A structure that wraps parameters needed for SignedHttpRequest validation.</param>
        /// <remarks>
        /// This method will be executed only if <see cref="SignedHttpRequestValidationParameters.ValidateU"/> is set to <c>true</c> or a u claim is present and
        /// <see cref="SignedHttpRequestValidationParameters.ValidatePresentClaims"/> is set to true and <see cref="SignedHttpRequestClaimTypes.U"/> is in
        /// <see cref="SignedHttpRequestValidationParameters.ClaimsToValidateWhenPresent"/>.
        /// </remarks>
        internal virtual void ValidateUClaim(JsonWebToken signedHttpRequest, SignedHttpRequestValidationContext signedHttpRequestValidationContext)
        {
            var httpRequestUri = signedHttpRequestValidationContext.HttpRequestData.Uri;

            if (httpRequestUri == null)
                throw LogHelper.LogArgumentNullException(nameof(signedHttpRequestValidationContext.HttpRequestData.Uri));

            if (!httpRequestUri.IsAbsoluteUri)
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidUClaimException(LogHelper.FormatInvariant(LogMessages.IDX23001, httpRequestUri.OriginalString)));

            if (!signedHttpRequest.TryGetPayloadValue(SignedHttpRequestClaimTypes.U, out string uClaimValue) || uClaimValue == null)
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidUClaimException(LogHelper.FormatInvariant(LogMessages.IDX23003, LogHelper.MarkAsNonPII(SignedHttpRequestClaimTypes.U))));

            // https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-3.2
            // u: The HTTP URL host component as a JSON string.
            // This MAY include the port separated from the host by a colon in host:port format.
            var expectedUClaimValue = httpRequestUri.Host;
            var expectedUClaimValueIncludingPort = $"{expectedUClaimValue}:{httpRequestUri.Port}";

            if (!string.Equals(expectedUClaimValue, uClaimValue, StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(expectedUClaimValueIncludingPort, uClaimValue, StringComparison.OrdinalIgnoreCase))
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidUClaimException(LogHelper.FormatInvariant(LogMessages.IDX23012, LogHelper.MarkAsNonPII(SignedHttpRequestClaimTypes.U), expectedUClaimValue, expectedUClaimValueIncludingPort, uClaimValue)));
        }

        /// <summary>
        /// Validates the signed http request "p" claim. 
        /// </summary>
        /// <param name="signedHttpRequest">A SignedHttpRequest.</param>
        /// <param name="signedHttpRequestValidationContext">A structure that wraps parameters needed for SignedHttpRequest validation.</param>
        /// <remarks>
        /// This method will be executed only if <see cref="SignedHttpRequestValidationParameters.ValidateP"/> is set to <c>true</c> or a p claim is present and
        /// <see cref="SignedHttpRequestValidationParameters.ValidatePresentClaims"/> is set to true and <see cref="SignedHttpRequestClaimTypes.P"/> is in
        /// <see cref="SignedHttpRequestValidationParameters.ClaimsToValidateWhenPresent"/>.
        /// </remarks>
        internal virtual void ValidatePClaim(JsonWebToken signedHttpRequest, SignedHttpRequestValidationContext signedHttpRequestValidationContext)
        {
            var httpRequestUri = signedHttpRequestValidationContext.HttpRequestData.Uri;

            if (httpRequestUri == null)
                throw LogHelper.LogArgumentNullException(nameof(signedHttpRequestValidationContext.HttpRequestData.Uri));

            httpRequestUri = EnsureAbsoluteUri(httpRequestUri);
            if (!signedHttpRequest.TryGetPayloadValue(SignedHttpRequestClaimTypes.P, out string pClaimValue) || pClaimValue == null)
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidPClaimException(LogHelper.FormatInvariant(LogMessages.IDX23003, LogHelper.MarkAsNonPII(SignedHttpRequestClaimTypes.P))));

            // relax comparison by trimming start and ending forward slashes
            pClaimValue = pClaimValue.Trim('/');
            var expectedPClaimValue = httpRequestUri.AbsolutePath.Trim('/');

            if (!string.Equals(expectedPClaimValue, pClaimValue, StringComparison.OrdinalIgnoreCase))
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidPClaimException(LogHelper.FormatInvariant(LogMessages.IDX23011, LogHelper.MarkAsNonPII(SignedHttpRequestClaimTypes.P), expectedPClaimValue, pClaimValue)));
        }

        /// <summary>
        /// Validates the signed http request "q" claim. 
        /// </summary>
        /// <param name="signedHttpRequest">A SignedHttpRequest.</param>
        /// <param name="signedHttpRequestValidationContext">A structure that wraps parameters needed for SignedHttpRequest validation.</param>
        /// <remarks>
        /// This method will be executed only if <see cref="SignedHttpRequestValidationParameters.ValidateQ"/> is set to <c>true</c> or a q claim is present and
        /// <see cref="SignedHttpRequestValidationParameters.ValidatePresentClaims"/> is set to true and <see cref="SignedHttpRequestClaimTypes.Q"/> is in
        /// <see cref="SignedHttpRequestValidationParameters.ClaimsToValidateWhenPresent"/>.
        /// </remarks>
        internal virtual void ValidateQClaim(JsonWebToken signedHttpRequest, SignedHttpRequestValidationContext signedHttpRequestValidationContext)
        {
            var httpRequestUri = signedHttpRequestValidationContext.HttpRequestData.Uri;

            if (httpRequestUri == null)
                throw LogHelper.LogArgumentNullException(nameof(httpRequestUri));

            if (!signedHttpRequest.TryGetPayloadValue(SignedHttpRequestClaimTypes.Q, out JArray qClaim) || qClaim == null)
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidQClaimException(LogHelper.FormatInvariant(LogMessages.IDX23003, LogHelper.MarkAsNonPII(SignedHttpRequestClaimTypes.Q))));

            httpRequestUri = EnsureAbsoluteUri(httpRequestUri);
            var sanitizedQueryParams = SanitizeQueryParams(httpRequestUri);

            string qClaimBase64UrlEncodedHash = string.Empty;
            string expectedBase64UrlEncodedHash = string.Empty;
            List<string> qClaimQueryParamNames;
            try
            {
                // "q": [["queryParamName1", "queryParamName2",... "queryParamNameN"], "base64UrlEncodedHashValue"]]
                qClaimQueryParamNames = qClaim[0].ToObject<List<string>>();
                qClaimBase64UrlEncodedHash = qClaim[1].ToString();
            }
            catch (Exception e)
            {
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidQClaimException(LogHelper.FormatInvariant(LogMessages.IDX23024, LogHelper.MarkAsNonPII(SignedHttpRequestClaimTypes.Q), qClaim.ToString(), e), e));
            }

            try
            {
                StringBuilder stringBuffer = new StringBuilder();
                var firstQueryParam = true;
                foreach (var queryParamName in qClaimQueryParamNames)
                {
                    if (!sanitizedQueryParams.TryGetValue(queryParamName, out var queryParamsValue))
                    {
                        throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidQClaimException(LogHelper.FormatInvariant(LogMessages.IDX23028, LogHelper.MarkAsNonPII(queryParamName), LogHelper.MarkAsNonPII(string.Join(", ", sanitizedQueryParams.Select(x => x.Key))))));
                    }
                    else
                    {
                        if (!firstQueryParam)
                            stringBuffer.Append("&");

                        stringBuffer.Append($"{queryParamName}={queryParamsValue}");
                        firstQueryParam = false;

                        // remove the query param from the dictionary to mark it as covered.
                        sanitizedQueryParams.Remove(queryParamName);
                    }
                }

                expectedBase64UrlEncodedHash = CalculateBase64UrlEncodedHash(stringBuffer.ToString());
            }
            catch (Exception e)
            {
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidQClaimException(LogHelper.FormatInvariant(LogMessages.IDX23025, LogHelper.MarkAsNonPII(SignedHttpRequestClaimTypes.Q), e), e));
            }

            if (!signedHttpRequestValidationContext.SignedHttpRequestValidationParameters.AcceptUnsignedQueryParameters && sanitizedQueryParams.Any())
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidQClaimException(LogHelper.FormatInvariant(LogMessages.IDX23029, LogHelper.MarkAsNonPII(string.Join(", ", sanitizedQueryParams.Select(x => x.Key))))));

            if (!string.Equals(expectedBase64UrlEncodedHash, qClaimBase64UrlEncodedHash))
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidQClaimException(LogHelper.FormatInvariant(LogMessages.IDX23011, LogHelper.MarkAsNonPII(SignedHttpRequestClaimTypes.Q), expectedBase64UrlEncodedHash, qClaimBase64UrlEncodedHash)));
        }

        /// <summary>
        /// Validates the signed http request "h" claim. 
        /// </summary>
        /// <param name="signedHttpRequest">A SignedHttpRequest.</param>
        /// <param name="signedHttpRequestValidationContext">A structure that wraps parameters needed for SignedHttpRequest validation.</param>
        /// <remarks>
        /// This method will be executed only if <see cref="SignedHttpRequestValidationParameters.ValidateH"/> is set to <c>true</c> or a h claim is present and
        /// <see cref="SignedHttpRequestValidationParameters.ValidatePresentClaims"/> is set to true and <see cref="SignedHttpRequestClaimTypes.H"/> is in
        /// <see cref="SignedHttpRequestValidationParameters.ClaimsToValidateWhenPresent"/>.
        /// </remarks>
        internal virtual void ValidateHClaim(JsonWebToken signedHttpRequest, SignedHttpRequestValidationContext signedHttpRequestValidationContext)
        {
            if (!signedHttpRequest.TryGetPayloadValue(SignedHttpRequestClaimTypes.H, out JArray hClaim) || hClaim == null)
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidHClaimException(LogHelper.FormatInvariant(LogMessages.IDX23003, LogHelper.MarkAsNonPII(SignedHttpRequestClaimTypes.H))));

            var sanitizedHeaders = SanitizeHeaders(signedHttpRequestValidationContext.HttpRequestData.Headers);

            string hClaimBase64UrlEncodedHash = string.Empty;
            string expectedBase64UrlEncodedHash = string.Empty;
            List<string> hClaimHeaderNames;
            try
            {
                // "h": [["headerName1", "headerName2",... "headerNameN"], "base64UrlEncodedHashValue"]]
                hClaimHeaderNames = hClaim[0].ToObject<List<string>>();
                hClaimBase64UrlEncodedHash = hClaim[1].ToString();
            }
            catch (Exception e)
            {
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidHClaimException(LogHelper.FormatInvariant(LogMessages.IDX23024, LogHelper.MarkAsNonPII(SignedHttpRequestClaimTypes.H), hClaim.ToString(), e), e));
            }

            try
            {
                StringBuilder stringBuffer = new StringBuilder();
                var firstHeader = true;
                foreach (var headerName in hClaimHeaderNames)
                {
                    if (!sanitizedHeaders.TryGetValue(headerName, out var headerValue))
                    {
                        throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidHClaimException(LogHelper.FormatInvariant(LogMessages.IDX23027, LogHelper.MarkAsNonPII(headerName), LogHelper.MarkAsNonPII(string.Join(", ", sanitizedHeaders.Select(x => x.Key))))));
                    }
                    else
                    {
                        if (!firstHeader)
                            stringBuffer.Append(_newlineSeparator);

                        stringBuffer.Append($"{headerName}: {headerValue}");
                        firstHeader = false;

                        // remove the header from the dictionary to mark it as covered.
                        sanitizedHeaders.Remove(headerName);
                    }
                }

                expectedBase64UrlEncodedHash = CalculateBase64UrlEncodedHash(stringBuffer.ToString());
            }
            catch (Exception e)
            {
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidHClaimException(LogHelper.FormatInvariant(LogMessages.IDX23025, LogHelper.MarkAsNonPII(SignedHttpRequestClaimTypes.H), e), e));
            }

            if (!signedHttpRequestValidationContext.SignedHttpRequestValidationParameters.AcceptUnsignedHeaders && sanitizedHeaders.Any())
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidHClaimException(LogHelper.FormatInvariant(LogMessages.IDX23026, LogHelper.MarkAsNonPII(string.Join(", ", sanitizedHeaders.Select(x => x.Key))))));

            if (!string.Equals(expectedBase64UrlEncodedHash, hClaimBase64UrlEncodedHash))
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidHClaimException(LogHelper.FormatInvariant(LogMessages.IDX23011, LogHelper.MarkAsNonPII(SignedHttpRequestClaimTypes.H), expectedBase64UrlEncodedHash, hClaimBase64UrlEncodedHash)));
        }

        /// <summary>
        /// Validates the signed http request "b" claim. 
        /// </summary>
        /// <param name="signedHttpRequest">A SignedHttpRequest.</param>
        /// <param name="signedHttpRequestValidationContext">A structure that wraps parameters needed for SignedHttpRequest validation.</param>
        /// <remarks>
        /// This method will be executed only if <see cref="SignedHttpRequestValidationParameters.ValidateB"/> is set to <c>true</c> or a b claim is present and
        /// <see cref="SignedHttpRequestValidationParameters.ValidatePresentClaims"/> is set to true and <see cref="SignedHttpRequestClaimTypes.B"/> is in
        /// <see cref="SignedHttpRequestValidationParameters.ClaimsToValidateWhenPresent"/>.
        /// </remarks>
        internal virtual void ValidateBClaim(JsonWebToken signedHttpRequest, SignedHttpRequestValidationContext signedHttpRequestValidationContext)
        {
            var httpRequestBody = signedHttpRequestValidationContext.HttpRequestData.Body;

            if (httpRequestBody == null)
                httpRequestBody = new byte[0];

            if (!signedHttpRequest.TryGetPayloadValue(SignedHttpRequestClaimTypes.B, out string bClaim) || bClaim == null)
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidBClaimException(LogHelper.FormatInvariant(LogMessages.IDX23003, LogHelper.MarkAsNonPII(SignedHttpRequestClaimTypes.B))));

            string expectedBase64UrlEncodedHash;
            try
            {
                expectedBase64UrlEncodedHash = CalculateBase64UrlEncodedHash(httpRequestBody);
            }
            catch (Exception e)
            {
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestCreationException(LogHelper.FormatInvariant(LogMessages.IDX23008, LogHelper.MarkAsNonPII(SignedHttpRequestClaimTypes.B), e), e));
            }

            if (!string.Equals(expectedBase64UrlEncodedHash, bClaim))
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidBClaimException(LogHelper.FormatInvariant(LogMessages.IDX23011, LogHelper.MarkAsNonPII(SignedHttpRequestClaimTypes.B), expectedBase64UrlEncodedHash, bClaim)));
        }
        #endregion

        #region Resolving PoP key
        /// <summary>
        /// Resolves a PoP <see cref="SecurityKey"/>.
        /// </summary>
        /// <param name="signedHttpRequest">A signed http request as a JWT.</param>
        /// <param name="validatedAccessToken">An access token ("at") that was already validated during the SignedHttpRequest validation process.</param>
        /// <param name="signedHttpRequestValidationContext">A structure that wraps parameters needed for SignedHttpRequest validation.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>A resolved PoP <see cref="SecurityKey"/>.</returns>
        internal virtual async Task<SecurityKey> ResolvePopKeyAsync(JsonWebToken signedHttpRequest, JsonWebToken validatedAccessToken, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken)
        {
            if (signedHttpRequestValidationContext == null)
                throw LogHelper.LogArgumentNullException(nameof(signedHttpRequestValidationContext));

            if (signedHttpRequestValidationContext.SignedHttpRequestValidationParameters.PopKeyResolverAsync != null)
                return await signedHttpRequestValidationContext.SignedHttpRequestValidationParameters.PopKeyResolverAsync(validatedAccessToken, signedHttpRequest, signedHttpRequestValidationContext, cancellationToken).ConfigureAwait(false);

            var cnf = GetCnfClaimValue(signedHttpRequest, validatedAccessToken, signedHttpRequestValidationContext);
            return await ResolvePopKeyFromCnfClaimAsync(cnf, signedHttpRequest, validatedAccessToken, signedHttpRequestValidationContext, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the JSON representation of the 'cnf' claim.
        /// This method expects a "cnf" claim to be present as a claim of the <paramref name="validatedAccessToken"/> ("at").
        /// </summary>
        /// <param name="signedHttpRequest">A signed http request as a JWT.</param>
        /// <param name="validatedAccessToken">An access token ("at") that was already validated during the SignedHttpRequest validation process.</param>
        /// <param name="signedHttpRequestValidationContext">A structure that wraps parameters needed for SignedHttpRequest validation.</param>
        /// <returns>JSON representation of the 'cnf' claim.</returns>
        internal virtual JObject GetCnfClaimValue(JsonWebToken signedHttpRequest, JsonWebToken validatedAccessToken, SignedHttpRequestValidationContext signedHttpRequestValidationContext)
        {
            if (validatedAccessToken == null)
                throw LogHelper.LogArgumentNullException(nameof(validatedAccessToken));

            // use the decrypted jwt if the jwtValidatedAccessToken is encrypted.
            if (validatedAccessToken.InnerToken != null)
                validatedAccessToken = validatedAccessToken.InnerToken;
            
            if (validatedAccessToken.TryGetPayloadValue(ConfirmationClaimTypes.Cnf, out JObject cnf) && cnf != null)
                return cnf;
            else if (validatedAccessToken.TryGetPayloadValue(ConfirmationClaimTypes.Cnf, out string cnfString) && !string.IsNullOrEmpty(cnfString))
                return JObject.Parse(cnfString);
            else
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidCnfClaimException(LogHelper.FormatInvariant(LogMessages.IDX23003, LogHelper.MarkAsNonPII(ConfirmationClaimTypes.Cnf))));
        }

        /// <summary>
        /// Resolves a PoP <see cref="SecurityKey"/> from a confirmation ("cnf") claim.
        /// </summary>
        /// <param name="cnf">A confirmation ("cnf") claim.</param>
        /// <param name="signedHttpRequest">A signed http request as a JWT.</param>
        /// <param name="validatedAccessToken">An access token ("at") that was already validated during the SignedHttpRequest validation process.</param>
        /// <param name="signedHttpRequestValidationContext">A structure that wraps parameters needed for SignedHttpRequest validation.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>A resolved PoP <see cref="SecurityKey"/>.</returns>
        /// <remarks>https://datatracker.ietf.org/doc/html/rfc7800#section-3.1</remarks>
        internal virtual async Task<SecurityKey> ResolvePopKeyFromCnfClaimAsync(JObject cnf, JsonWebToken signedHttpRequest, JsonWebToken validatedAccessToken, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken)
        {
            if (cnf == null)
                throw LogHelper.LogArgumentNullException(nameof(cnf));

            if (cnf.TryGetValue(ConfirmationClaimTypes.Jwk, StringComparison.Ordinal, out var jwk))
                return ResolvePopKeyFromJwk(jwk.ToString(), signedHttpRequestValidationContext);
            else if (cnf.TryGetValue(ConfirmationClaimTypes.Jwe, StringComparison.Ordinal, out var jwe))
                return await ResolvePopKeyFromJweAsync(jwe.ToString(), signedHttpRequestValidationContext, cancellationToken).ConfigureAwait(false);
            else if (cnf.TryGetValue(ConfirmationClaimTypes.Jku, StringComparison.Ordinal, out var jku))
                return await ResolvePopKeyFromJkuAsync(jku.ToString(), cnf, signedHttpRequestValidationContext, cancellationToken).ConfigureAwait(false);
            else if (cnf.TryGetValue(ConfirmationClaimTypes.Kid, StringComparison.Ordinal, out var kid))
                return await ResolvePopKeyFromKeyIdentifierAsync(kid.ToString(), signedHttpRequest, validatedAccessToken, signedHttpRequestValidationContext, cancellationToken).ConfigureAwait(false);
            else
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidCnfClaimException(LogHelper.FormatInvariant(LogMessages.IDX23014, cnf.ToString())));
        }

        /// <summary>
        /// Resolves a PoP <see cref="SecurityKey"/> from the asymmetric representation of a PoP key. 
        /// </summary>
        /// <param name="jwk">An asymmetric representation of a PoP key (JSON).</param>
        /// <param name="signedHttpRequestValidationContext">A structure that wraps parameters needed for SignedHttpRequest validation.</param>
        /// <returns>A resolved PoP <see cref="SecurityKey"/>.</returns>
        internal virtual SecurityKey ResolvePopKeyFromJwk(string jwk, SignedHttpRequestValidationContext signedHttpRequestValidationContext)
        {
            if (string.IsNullOrEmpty(jwk))
                throw LogHelper.LogArgumentNullException(nameof(jwk));

            var jsonWebKey = new JsonWebKey(jwk);

            if (JsonWebKeyConverter.TryConvertToSecurityKey(jsonWebKey, out var key))
            {
                if (key is AsymmetricSecurityKey)
                    return jsonWebKey;
                else
                    throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidPopKeyException(LogHelper.FormatInvariant(LogMessages.IDX23015, LogHelper.MarkAsNonPII(key.GetType().ToString()))));
            }
            else
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidPopKeyException(LogHelper.FormatInvariant(LogMessages.IDX23016, jsonWebKey.ToString())));
        }

        /// <summary>
        /// Resolves a PoP <see cref="SecurityKey"/> from the encrypted symmetric representation of a PoP key. 
        /// </summary>
        /// <param name="jwe">An encrypted symmetric representation of a PoP key (JSON).</param>
        /// <param name="signedHttpRequestValidationContext">A structure that wraps parameters needed for SignedHttpRequest validation.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>A resolved PoP <see cref="SecurityKey"/>.</returns>
        internal virtual async Task<SecurityKey> ResolvePopKeyFromJweAsync(string jwe, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken)
        {
            var jwk = await SignedHttpRequestUtilities.DecryptSymmetricPopKeyAsync(_jwtTokenHandler, jwe, signedHttpRequestValidationContext, cancellationToken).ConfigureAwait(false);
            if (JsonWebKeyConverter.TryConvertToSymmetricSecurityKey(jwk, out _))
                return jwk;
            else
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidPopKeyException(LogHelper.FormatInvariant(LogMessages.IDX23019, LogHelper.MarkAsNonPII(jwk.GetType().ToString()))));
        }

        /// <summary>
        /// Resolves a PoP <see cref="SecurityKey"/> from the URL reference to a PoP key.  
        /// </summary>
        /// <param name="jkuSetUrl">A URL reference to a PoP JWK set.</param>
        /// <param name="cnf">A confirmation ("cnf") claim as a JObject.</param>
        /// <param name="signedHttpRequestValidationContext">A structure that wraps parameters needed for SignedHttpRequest validation.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>A resolved PoP <see cref="SecurityKey"/>.</returns>
        internal virtual async Task<SecurityKey> ResolvePopKeyFromJkuAsync(string jkuSetUrl, JObject cnf, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken)
        {
            var popKeys = await GetPopKeysFromJkuAsync(jkuSetUrl, signedHttpRequestValidationContext, cancellationToken).ConfigureAwait(false);

            if (popKeys == null || popKeys.Count == 0)
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidPopKeyException(LogHelper.FormatInvariant(LogMessages.IDX23031)));

            if (popKeys.Count == 1)
            {
                return popKeys[0];
            }
            // If there are multiple keys in the referenced JWK Set document, a "kid" member MUST also be included
            // with the referenced key's JWK also containing the same "kid" value.
            // https://datatracker.ietf.org/doc/html/rfc7800#section-3.5
            else if (cnf.TryGetValue(ConfirmationClaimTypes.Kid, StringComparison.Ordinal, out var kid))
            {
                foreach (var key in popKeys)
                {
                    if (string.Equals(key.KeyId, kid.ToString()))
                        return key;
                }

                throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidPopKeyException(LogHelper.FormatInvariant(LogMessages.IDX23021, kid, string.Join(", ", popKeys.Select(x => x.KeyId ?? "Null")))));
            }
            else
            {
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidPopKeyException(LogHelper.FormatInvariant(LogMessages.IDX23035)));
            }
        }

        /// <summary>
        /// Gets a JWK set of PoP <see cref="SecurityKey"/> from the <paramref name="jkuSetUrl"/>.
        /// </summary>
        /// <param name="jkuSetUrl">A URL reference to a PoP JWK set.</param>
        /// <param name="signedHttpRequestValidationContext">A structure that wraps parameters needed for SignedHttpRequest validation.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>A collection of PoP <see cref="SecurityKey"/>.</returns>
        internal virtual async Task<IList<SecurityKey>> GetPopKeysFromJkuAsync(string jkuSetUrl, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(jkuSetUrl))
                throw LogHelper.LogArgumentNullException(nameof(jkuSetUrl));

            if (!Utility.IsHttps(jkuSetUrl) && signedHttpRequestValidationContext.SignedHttpRequestValidationParameters.RequireHttpsForJkuResourceRetrieval)
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidPopKeyException(LogHelper.FormatInvariant(LogMessages.IDX23006, jkuSetUrl)));

            try
            {
                var httpClient = signedHttpRequestValidationContext.SignedHttpRequestValidationParameters.HttpClientProvider?.Invoke() ?? _defaultHttpClient;
                var uri = new Uri(jkuSetUrl, UriKind.RelativeOrAbsolute);
                var response = await httpClient.GetAsync(uri, cancellationToken).ConfigureAwait(false);
                var jsonWebKey = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var jsonWebKeySet = new JsonWebKeySet(jsonWebKey);
                return jsonWebKeySet.Keys.Cast<SecurityKey>().ToList();
            }
            catch (Exception e)
            {
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidPopKeyException(LogHelper.FormatInvariant(LogMessages.IDX23022, jkuSetUrl, e), e));
            }
        }

        /// <summary>
        /// Resolves a PoP <see cref="SecurityKey"/> using a key identifier of a PoP key. 
        /// </summary>
        /// <param name="kid">A <see cref="ConfirmationClaimTypes.Kid"/> claim value.</param>
        /// <param name="signedHttpRequest">A signed http request as a JWT.</param>
        /// <param name="validatedAccessToken">An access token ("at") that was already validated during the SignedHttpRequest validation process.</param>
        /// <param name="signedHttpRequestValidationContext">A structure that wraps parameters needed for SignedHttpRequest validation.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>A resolved PoP <see cref="SecurityKey"/>.</returns>
        /// <remarks>
        /// To resolve a PoP <see cref="SecurityKey"/> using only the 'kid' claim, set the <see cref="SignedHttpRequestValidationParameters.PopKeyResolverFromKeyIdAsync"/> delegate.
        /// </remarks>
        internal virtual async Task<SecurityKey> ResolvePopKeyFromKeyIdentifierAsync(string kid, JsonWebToken signedHttpRequest, JsonWebToken validatedAccessToken, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken)
        {
            if (signedHttpRequestValidationContext.SignedHttpRequestValidationParameters.PopKeyResolverFromKeyIdAsync != null)
                return await signedHttpRequestValidationContext.SignedHttpRequestValidationParameters.PopKeyResolverFromKeyIdAsync(kid, validatedAccessToken, signedHttpRequest, signedHttpRequestValidationContext, cancellationToken).ConfigureAwait(false);
            else if (signedHttpRequest != null && signedHttpRequest.TryGetPayloadValue(ConfirmationClaimTypes.Cnf, out JObject signedHttpRequestCnf) && signedHttpRequestCnf != null)
                return await ResolvePopKeyFromCnfReferenceAsync(kid, signedHttpRequestCnf, validatedAccessToken, signedHttpRequestValidationContext, cancellationToken).ConfigureAwait(false);
            else
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidPopKeyException(LogHelper.FormatInvariant(LogMessages.IDX23023)));
        }

        /// <summary>
        /// Resolves a PoP key from a "cnf" reference and validates the reference.
        /// </summary>
        /// <param name="cnfReferenceId">A reference to the root "cnf" claim, as base64url-encoded JWK thumbprint.</param>
        /// <param name="confirmationClaim">A confirmation ("cnf") claim.</param>
        /// <param name="validatedAccessToken">An access token ("at") that was already validated during the SignedHttpRequest validation process.</param>
        /// <param name="signedHttpRequestValidationContext">A structure that wraps parameters needed for SignedHttpRequest validation.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>A resolved PoP <see cref="SecurityKey"/>.</returns>
        /// <remarks><paramref name="cnfReferenceId"/> MUST match the base64url-encoded thumbprint of a JWK resolved from the <paramref name="confirmationClaim"/>.</remarks>
        internal virtual async Task<SecurityKey> ResolvePopKeyFromCnfReferenceAsync(string cnfReferenceId, JObject confirmationClaim, JsonWebToken validatedAccessToken, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken)
        {
            // resolve PoP key from the confirmation claim, but set signedHttpRequest to null to prevent recursion.
            var popKey = await ResolvePopKeyFromCnfClaimAsync(confirmationClaim, null, validatedAccessToken, signedHttpRequestValidationContext, cancellationToken).ConfigureAwait(false);

            string jwkPopKeyThumprint;
            // if the cnf key is an X509SecurityKey ('x5c'), JWK thumbprint has to be calculated on its underlying RSA key.
            if (popKey is JsonWebKey jwtPopKey && jwtPopKey.ConvertedSecurityKey is X509SecurityKey)
                jwkPopKeyThumprint = Base64UrlEncoder.Encode(jwtPopKey.ConvertedSecurityKey.ComputeJwkThumbprint());
            else
                jwkPopKeyThumprint = Base64UrlEncoder.Encode(popKey.ComputeJwkThumbprint());

            // validate reference
            if (!string.Equals(cnfReferenceId, jwkPopKeyThumprint))
                throw LogHelper.LogExceptionMessage(new SignedHttpRequestInvalidPopKeyException(LogHelper.FormatInvariant(LogMessages.IDX23033, cnfReferenceId, jwkPopKeyThumprint, confirmationClaim)));

            return popKey;
        }
        #endregion

        #region Private utility methods
        private static string CalculateBase64UrlEncodedHash(string data)
        {
            return CalculateBase64UrlEncodedHash(Encoding.UTF8.GetBytes(data));
        }

        private static string CalculateBase64UrlEncodedHash(byte[] bytes)
        {
            using (var hash = SHA256.Create())
            {
                var hashedBytes = hash.ComputeHash(bytes);
                return Base64UrlEncoder.Encode(hashedBytes);
            }
        }

        /// <summary>
        /// Ensures that the <paramref name="uri"/> is <see cref="UriKind.Absolute"/>.
        /// If <paramref name="uri"/> is <see cref="UriKind.Absolute"/>, the method returns it as-is.
        /// If <paramref name="uri"/> is <see cref="UriKind.Relative"/>, new helper <see cref="UriKind.Absolute"/> URI is created and returned.
        /// Throws in case that an <see cref="UriKind.Absolute"/> URI can't be created.
        /// </summary>
        private Uri EnsureAbsoluteUri(Uri uri)
        {
            if (uri.IsAbsoluteUri)
            {
                return uri;
            }
            else
            {
                if (!Uri.TryCreate(_baseUriHelper, uri, out Uri absoluteUri))
                    throw LogHelper.LogExceptionMessage(new SignedHttpRequestCreationException(LogHelper.FormatInvariant(LogMessages.IDX23007, uri.OriginalString)));

                return absoluteUri;
            }
        }

        /// <summary>
        /// Sanitizes the query params to comply with the specification.
        /// </summary>
        /// <remarks>https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-7.5.</remarks>
        private static Dictionary<string, string> SanitizeQueryParams(Uri httpRequestUri)
        {
            // Remove repeated query params according to the spec: https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-7.5.
            // "If a header or query parameter is repeated on either the outgoing request from the client or the
            // incoming request to the protected resource, that query parameter or header name MUST NOT be covered by the hash and signature."
            var sanitizedQueryParams = new Dictionary<string, string>(StringComparer.Ordinal);
            var repeatedQueryParams = new List<string>();

            var queryString = httpRequestUri.Query.TrimStart('?');
            if (string.IsNullOrEmpty(queryString))
                return sanitizedQueryParams;

            var queryParamKeyValuePairs = queryString.Split('&');
            foreach (var queryParamValuePair in queryParamKeyValuePairs)
            {
                var queryParamKeyValuePairArray = queryParamValuePair.Split('=');
                if (queryParamKeyValuePairArray.Length == 2)
                {
                    var queryParamName = queryParamKeyValuePairArray[0];
                    var queryParamValue = queryParamKeyValuePairArray[1];
                    if (!string.IsNullOrEmpty(queryParamName))
                    {
                        // if sanitizedQueryParams already contains the query parameter name it means that the queryParamName is repeated.
                        // in that case queryParamName should not be added, and the existing entry in sanitizedQueryParams should be removed.
                        if (sanitizedQueryParams.ContainsKey(queryParamName))
                            repeatedQueryParams.Add(queryParamName);
                        else if (!string.IsNullOrEmpty(queryParamValue))
                            sanitizedQueryParams.Add(queryParamName, queryParamValue);
                    }
                }
            }

            if (repeatedQueryParams.Any())
            {
                LogHelper.LogWarning(LogHelper.FormatInvariant(LogMessages.IDX23004, LogHelper.MarkAsNonPII(string.Join(", ", repeatedQueryParams))));

                foreach (var repeatedQueryParam in repeatedQueryParams)
                {
                    if (sanitizedQueryParams.ContainsKey(repeatedQueryParam))
                        sanitizedQueryParams.Remove(repeatedQueryParam);
                }
            }

            return sanitizedQueryParams;
        }

        /// <summary>
        /// Sanitizes the headers to comply with the specification.
        /// </summary>
        /// <remarks>
        /// https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-4.1
        /// https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-7.5
        /// </remarks>
        private static Dictionary<string, string> SanitizeHeaders(IDictionary<string, IEnumerable<string>> headers)
        {
            // Remove repeated headers according to the spec: https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-7.5.
            // "If a header or query parameter is repeated on either the outgoing request from the client or the
            // incoming request to the protected resource, that query parameter or header name MUST NOT be covered by the hash and signature."
            var sanitizedHeaders = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var repeatedHeaders = new List<string>();
            foreach (var header in headers)
            {
                var headerName = header.Key;

                if (string.IsNullOrEmpty(headerName))
                    continue;

                // Don't include the authorization header (https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-4.1).
                if (string.Equals(headerName, SignedHttpRequestConstants.AuthorizationHeader, StringComparison.OrdinalIgnoreCase))
                    continue;

                // if sanitizedHeaders already contains the header name it means that the headerName is repeated.
                // in that case headerName should not be added, and the existing entry in sanitizedHeaders should be removed.
                if (sanitizedHeaders.ContainsKey(headerName))
                {
                    repeatedHeaders.Add(headerName.ToLowerInvariant());
                }
                // if header has more than one value don't add it to the sanitizedHeaders as it's repeated.
                else if (header.Value.Count() > 1)
                {
                    repeatedHeaders.Add(headerName.ToLowerInvariant());
                }
                else if (header.Value.Count() == 1 && !string.IsNullOrEmpty(header.Value.First()))
                    sanitizedHeaders.Add(headerName, header.Value.First());
            }

            if (repeatedHeaders.Any())
            {
                LogHelper.LogWarning(LogHelper.FormatInvariant(LogMessages.IDX23005, LogHelper.MarkAsNonPII(string.Join(", ", repeatedHeaders))));

                foreach (var repeatedHeaderName in repeatedHeaders)
                {
                    if (sanitizedHeaders.ContainsKey(repeatedHeaderName))
                        sanitizedHeaders.Remove(repeatedHeaderName);
                }
            }

            return sanitizedHeaders;
        }
        #endregion
    }
}
