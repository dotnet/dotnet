// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect
{
    /// <summary>
    /// Delegate for validating additional claims in 'id_token'.
    /// </summary>
    /// <param name="idToken">The <see cref="JwtSecurityToken"/> to validate.</param>
    /// <param name="context">The <see cref="OpenIdConnectProtocolValidationContext"/> used for validation.</param>
    public delegate void IdTokenValidator(JwtSecurityToken idToken, OpenIdConnectProtocolValidationContext context);

    /// <summary>
    /// <see cref="OpenIdConnectProtocolValidator"/> is used to ensure that an <see cref="OpenIdConnectMessage"/>
    /// obtained using OpenID Connect is compliant with <see href="https://openid.net/specs/openid-connect-core-1_0.html"/>.
    /// </summary>
    public class OpenIdConnectProtocolValidator
    {
        private readonly Dictionary<string, string> _hashAlgorithmMap =
            new Dictionary<string, string>
            {
                { SecurityAlgorithms.EcdsaSha256, "SHA256" },
                { SecurityAlgorithms.EcdsaSha256Signature, "SHA256" },
                { SecurityAlgorithms.HmacSha256, "SHA256" },
                { SecurityAlgorithms.RsaSha256, "SHA256" },
                { SecurityAlgorithms.RsaSha256Signature, "SHA256" },
                { SecurityAlgorithms.RsaSsaPssSha256, "SHA256" },
                { SecurityAlgorithms.EcdsaSha384, "SHA384" },
                { SecurityAlgorithms.EcdsaSha384Signature, "SHA384" },
                { SecurityAlgorithms.HmacSha384, "SHA384" },
                { SecurityAlgorithms.RsaSha384, "SHA384" },
                { SecurityAlgorithms.RsaSha384Signature, "SHA384" },
                { SecurityAlgorithms.RsaSsaPssSha384, "SHA384" },
                { SecurityAlgorithms.EcdsaSha512, "SHA512" },
                { SecurityAlgorithms.EcdsaSha512Signature, "SHA512" },
                { SecurityAlgorithms.HmacSha512, "SHA512" },
                { SecurityAlgorithms.RsaSha512, "SHA512" },
                { SecurityAlgorithms.RsaSha512Signature, "SHA512" },
                { SecurityAlgorithms.RsaSsaPssSha512, "SHA512" }
          };

        private TimeSpan _nonceLifetime = DefaultNonceLifetime;
        private CryptoProviderFactory _cryptoProviderFactory;

        /// <summary>
        /// Default for the how long the nonce is valid.
        /// </summary>
        /// <remarks>The default is 1 hour.</remarks>
        public static readonly TimeSpan DefaultNonceLifetime = TimeSpan.FromMinutes(60);

        /// <summary>
        /// Creates a new instance of <see cref="OpenIdConnectProtocolValidator"/>,
        /// </summary>
        public OpenIdConnectProtocolValidator()
        {
            RequireAcr = false;
            RequireAmr = false;
            RequireAuthTime = false;
            RequireAzp = false;
            RequireNonce = true;
            RequireState = true;
            RequireTimeStampInNonce = true;
            RequireStateValidation = true;

            _cryptoProviderFactory = new CryptoProviderFactory(CryptoProviderFactory.Default);
        }

        /// <summary>
        /// Generates a value suitable to use as a nonce.
        /// </summary>
        /// <returns>A nonce</returns>
        /// <remarks>If <see cref="RequireTimeStampInNonce"/> is true then the 'nonce' will contain the Epoch time as the prefix, seperated by a '.'.
        /// <para>For example: 635410359229176103.MjQxMzU0ODUtMTdiNi00NzAwLWE4MjYtNTE4NGExYmMxNTNlZmRkOGU4NjctZjQ5OS00MWIyLTljNTEtMjg3NmM0NzI4ZTc5</para></remarks>
        public virtual string GenerateNonce()
        {
            LogHelper.LogVerbose(LogMessages.IDX21328);
            string nonce = Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString() + Guid.NewGuid().ToString()));
            if (RequireTimeStampInNonce)
            {
                return DateTime.UtcNow.Ticks.ToString(CultureInfo.InvariantCulture) + "." + nonce;
            }

            return nonce;
        }

        /// <summary>
        /// Gets the algorithm mapping between OpenIdConnect and .Net for Hash algorithms.
        /// a <see cref="IDictionary{TKey, TValue}"/> that contains mappings from the JWT namespace <see href="https://datatracker.ietf.org/doc/html/rfc7518"/> to .NET.
        /// </summary>
        public IDictionary<string, string> HashAlgorithmMap
        {
            get
            {
                return _hashAlgorithmMap;
            }
        }

        /// <summary>
        /// Gets or set the <see cref="TimeSpan"/> defining how long a nonce is valid.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if 'value' is less than or equal to 'TimeSpan.Zero'.</exception>
        /// <remarks>If <see cref="RequireTimeStampInNonce"/> is true, then the nonce timestamp is bound by DateTime.UtcNow + NonceLifetime.</remarks>
        public TimeSpan NonceLifetime
        {
            get
            {
                return _nonceLifetime;
            }

            set
            {
                if (value <= TimeSpan.Zero)
                {
                    throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(nameof(value), LogHelper.FormatInvariant(LogMessages.IDX21105, LogHelper.MarkAsNonPII(value))));
                }

                _nonceLifetime = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating if an 'acr' claim is required.
        /// </summary>
        [DefaultValue(false)]
        public bool RequireAcr { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if an 'amr' claim is required.
        /// </summary>
        [DefaultValue(false)]
        public bool RequireAmr { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if an 'auth_time' claim is required.
        /// </summary>
        [DefaultValue(false)]
        public bool RequireAuthTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if an 'azp' claim is required.
        /// </summary>
        [DefaultValue(false)]
        public bool RequireAzp { get; set; }

        /// <summary>
        /// Get or sets if a nonce is required.
        /// </summary>
        [DefaultValue(true)]
        public bool RequireNonce { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if a 'state' is required.
        /// </summary>
        [DefaultValue(true)]
        public bool RequireState { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if validation of 'state' is turned on or off.
        /// </summary>
        [DefaultValue(true)]
        public bool RequireStateValidation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if a 'sub' claim is required.
        /// </summary>
        [DefaultValue(true)]
        public bool RequireSub { get; set; } = RequireSubByDefault;

        /// <summary>
        /// Gets or sets a value for default RequreSub.
        /// </summary>
        /// <remarks>default: true.</remarks>
        public static bool RequireSubByDefault { get; set; } = true;

        /// <summary>
        /// Gets or set logic to control if a nonce is prefixed with a timestamp.
        /// </summary>
        /// <remarks>if <see cref="RequireTimeStampInNonce"/> is true then:
        /// <para><see cref="GenerateNonce"/> will return a 'nonce' with the Epoch time as the prefix, delimited with a '.'.</para>
        /// <para><see cref="ValidateNonce"/> will require that the 'nonce' has a valid time as the prefix.</para>
        /// </remarks>
        [DefaultValue(true)]
        public bool RequireTimeStampInNonce { get; set; }

        /// <summary>
        /// Gets or sets the delegate for validating 'id_token'.
        /// </summary>
        public IdTokenValidator IdTokenValidator { get; set; }

        /// <summary>
        /// Validates that an OpenID Connect response from 'authorization_endpoint" is valid as per <see href="https://openid.net/specs/openid-connect-core-1_0.html"/>.
        /// </summary>
        /// <param name="validationContext">the <see cref="OpenIdConnectProtocolValidationContext"/> that contains expected values.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="validationContext"/> is null.</exception>
        /// <exception cref="OpenIdConnectProtocolException">Thrown if the response is not spec compliant.</exception>
        /// <remarks>It is assumed that the IdToken had ('aud', 'iss', 'signature', 'lifetime') validated.</remarks>
        public virtual void ValidateAuthenticationResponse(OpenIdConnectProtocolValidationContext validationContext)
        {
            if (validationContext == null)
                throw LogHelper.LogArgumentNullException("validationContext");

            // no 'response' is received or 'id_token' in the response is null
            if (validationContext.ProtocolMessage == null)
                throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogMessages.IDX21333));

            if (string.IsNullOrEmpty(validationContext.ProtocolMessage.IdToken))
            {
                // if 'code' is also not present, then throw.
                if (string.IsNullOrEmpty(validationContext.ProtocolMessage.Code))
                {
                    throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogMessages.IDX21334));
                }
                else
                {
                    ValidateState(validationContext);
                }
                return;
            }

            if (validationContext.ValidatedIdToken == null)
                throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogMessages.IDX21332));

            // 'refresh_token' should not be returned from 'authorization_endpoint'. https://datatracker.ietf.org/doc/html/rfc6749#section-4.2.2.
            if (!string.IsNullOrEmpty(validationContext.ProtocolMessage.RefreshToken))
                throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogMessages.IDX21335));

            ValidateState(validationContext);
            ValidateIdToken(validationContext);
            ValidateNonce(validationContext);
            ValidateCHash(validationContext);
            ValidateAtHash(validationContext);
        }

        /// <summary>
        /// Validates that an OpenID Connect response from "token_endpoint" is valid as per <see href="https://openid.net/specs/openid-connect-core-1_0.html"/>.
        /// </summary>
        /// <param name="validationContext">the <see cref="OpenIdConnectProtocolValidationContext"/> that contains expected values.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="validationContext"/> is null.</exception>
        /// <exception cref="OpenIdConnectProtocolException">Thrown if the response is not spec compliant.</exception>
        /// <remarks>It is assumed that the IdToken had ('aud', 'iss', 'signature', 'lifetime') validated.</remarks>
        public virtual void ValidateTokenResponse(OpenIdConnectProtocolValidationContext validationContext)
        {
            if (validationContext == null)
                throw LogHelper.LogArgumentNullException(nameof(validationContext));

            // no 'response' is recieved
            if (validationContext.ProtocolMessage == null)
                throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogMessages.IDX21333));

            // both 'id_token' and 'access_token' are required
            if (string.IsNullOrEmpty(validationContext.ProtocolMessage.IdToken) || string.IsNullOrEmpty(validationContext.ProtocolMessage.AccessToken))
                throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogMessages.IDX21336));

            if (validationContext.ValidatedIdToken == null)
                throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogMessages.IDX21332));

            ValidateIdToken(validationContext);
            ValidateNonce(validationContext);

            // only if 'at_hash' claim exist. 'at_hash' is not required in token response.
            object atHashClaim;
            if (validationContext.ValidatedIdToken.Payload.TryGetValue(JwtRegisteredClaimNames.AtHash, out atHashClaim))
            {
                ValidateAtHash(validationContext);
            }

        }

        /// <summary>
        /// Validates that an OpenIdConnect response from "useinfo_endpoint" is valid as per <see href="https://openid.net/specs/openid-connect-core-1_0.html"/>.
        /// </summary>
        /// <param name="validationContext">the <see cref="OpenIdConnectProtocolValidationContext"/> that contains expected values.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="validationContext"/> is null.</exception>
        /// <exception cref="OpenIdConnectProtocolException">Thrown if the response is not spec compliant.</exception>
        public virtual void ValidateUserInfoResponse(OpenIdConnectProtocolValidationContext validationContext)
        {
            if (validationContext == null)
                throw LogHelper.LogArgumentNullException(nameof(validationContext));

            if (string.IsNullOrEmpty(validationContext.UserInfoEndpointResponse))
                throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogMessages.IDX21337));

            if (validationContext.ValidatedIdToken == null)
                throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogMessages.IDX21332));

            string sub = string.Empty;
            try
            {
                // if user info response is a jwt token
                var handler = new JwtSecurityTokenHandler();
                if (handler.CanReadToken(validationContext.UserInfoEndpointResponse))
                {
                    var token = handler.ReadToken(validationContext.UserInfoEndpointResponse) as JwtSecurityToken;
                    sub = token.Payload.Sub;
                }
                else
                {
                    // if the response is not a jwt, it should be json
                    var payload = JwtPayload.Deserialize(validationContext.UserInfoEndpointResponse);
                    sub = payload.Sub;
                }
            }
            catch (Exception ex)
            {
                throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogHelper.FormatInvariant(LogMessages.IDX21343, validationContext.UserInfoEndpointResponse), ex));
            }

            if (string.IsNullOrEmpty(sub))
                throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogMessages.IDX21345));

            if (string.IsNullOrEmpty(validationContext.ValidatedIdToken.Payload.Sub))
                throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogMessages.IDX21346));

            if (!string.Equals(validationContext.ValidatedIdToken.Payload.Sub, sub))
                throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogHelper.FormatInvariant(LogMessages.IDX21338, validationContext.ValidatedIdToken.Payload.Sub, sub)));
        }

        /// <summary>
        /// Validates the claims in the 'id_token' as per <see href="https://openid.net/specs/openid-connect-core-1_0.html#IDTokenValidation"/>.
        /// </summary>
        /// <param name="validationContext">the <see cref="OpenIdConnectProtocolValidationContext"/> that contains expected values.</param>
        protected virtual void ValidateIdToken(OpenIdConnectProtocolValidationContext validationContext)
        {
            if (validationContext == null)
                throw LogHelper.LogArgumentNullException("validationContext");

            if (validationContext.ValidatedIdToken == null)
                throw LogHelper.LogArgumentNullException("validationContext.ValidatedIdToken");

            // if user sets the custom validator, we call the delegate. The default checks for multiple audiences and azp are not executed.
            if (this.IdTokenValidator != null)
            {
                try
                {
                    this.IdTokenValidator(validationContext.ValidatedIdToken, validationContext);
                }
                catch (Exception ex)
                {
                    throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogHelper.FormatInvariant(LogMessages.IDX21313, validationContext.ValidatedIdToken), ex));
                }
                return;
            }
            else
            {
                JwtSecurityToken idToken = validationContext.ValidatedIdToken;

                // required claims
                if (idToken.Payload.Aud.Count == 0)
                    throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogHelper.FormatInvariant(LogMessages.IDX21314, LogHelper.MarkAsNonPII(JwtRegisteredClaimNames.Aud.ToLowerInvariant()), idToken)));

                if (!idToken.Payload.Expiration.HasValue)
                    throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogHelper.FormatInvariant(LogMessages.IDX21314, LogHelper.MarkAsNonPII(JwtRegisteredClaimNames.Exp.ToLowerInvariant()), idToken)));

                if (idToken.Payload.IssuedAt.Equals(DateTime.MinValue))
                    throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogHelper.FormatInvariant(LogMessages.IDX21314, LogHelper.MarkAsNonPII(JwtRegisteredClaimNames.Iat.ToLowerInvariant()), idToken)));

                if (idToken.Payload.Iss == null)
                    throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogHelper.FormatInvariant(LogMessages.IDX21314, LogHelper.MarkAsNonPII(JwtRegisteredClaimNames.Iss.ToLowerInvariant()), idToken)));

                // sub is required in OpenID spec; but we don't want to block valid idTokens provided by some identity providers
                if (RequireSub && (string.IsNullOrWhiteSpace(idToken.Payload.Sub)))
                    throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogHelper.FormatInvariant(LogMessages.IDX21314, LogHelper.MarkAsNonPII(JwtRegisteredClaimNames.Sub.ToLowerInvariant()), idToken)));

                // optional claims
                if (RequireAcr && string.IsNullOrWhiteSpace(idToken.Payload.Acr))
                    throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogHelper.FormatInvariant(LogMessages.IDX21315, idToken)));

                if (RequireAmr && idToken.Payload.Amr.Count == 0)
                    throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogHelper.FormatInvariant(LogMessages.IDX21316, idToken)));

                if (RequireAuthTime && !(idToken.Payload.AuthTime.HasValue))
                    throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogHelper.FormatInvariant(LogMessages.IDX21317, idToken)));

                if (RequireAzp && string.IsNullOrWhiteSpace(idToken.Payload.Azp))
                    throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogHelper.FormatInvariant(LogMessages.IDX21318, idToken)));

                // if multiple audiences are present in the id_token, 'azp' claim should be present
                if (idToken.Payload.Aud.Count > 1 && string.IsNullOrEmpty(idToken.Payload.Azp))
                {
                    LogHelper.LogWarning(LogMessages.IDX21339);
                }

                // if 'azp' claim exist, it should be equal to 'client_id' of the application
                if (!string.IsNullOrEmpty(idToken.Payload.Azp))
                {
                    if (string.IsNullOrEmpty(validationContext.ClientId))
                    {
                        throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogMessages.IDX21308));
                    }
                    else if (!string.Equals(idToken.Payload.Azp, validationContext.ClientId))
                    {
                        throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogHelper.FormatInvariant(LogMessages.IDX21340, idToken.Payload.Azp, validationContext.ClientId)));
                    }
                }
            }
        }

        /// <summary>
        /// Returns a <see cref="HashAlgorithm"/> corresponding to string 'algorithm' after translation using <see cref="HashAlgorithmMap"/>.
        /// </summary>
        /// <param name="algorithm">string representing the hash algorithm</param>
        /// <returns>A <see cref="HashAlgorithm"/>.</returns>
        public virtual HashAlgorithm GetHashAlgorithm(string algorithm)
        {
            if (string.IsNullOrEmpty(algorithm))
                throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogMessages.IDX21350));

            try
            {
                if (!HashAlgorithmMap.TryGetValue(algorithm, out string hashAlgorithm))
                    hashAlgorithm = algorithm;

                return CryptoProviderFactory.CreateHashAlgorithm(hashAlgorithm);
            }
            catch (Exception ex)
            {
                throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogHelper.FormatInvariant(LogMessages.IDX21301, LogHelper.MarkAsNonPII(algorithm), LogHelper.MarkAsNonPII(typeof(HashAlgorithm))), ex));
            }

            throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogHelper.FormatInvariant(LogMessages.IDX21302, LogHelper.MarkAsNonPII(algorithm))));
        }

        /// <summary>
        /// Gets or sets the <see cref="CryptoProviderFactory"/> that will be used for crypto operations.
        /// </summary>
        public CryptoProviderFactory CryptoProviderFactory
        {
            get
            {
                return _cryptoProviderFactory;
            }
            set
            {
                if (value == null)
                    throw LogHelper.LogArgumentNullException("value");

                _cryptoProviderFactory = value;
            }
        }

        /// <summary>
        /// Validates the 'token' or 'code'. See: <see href="https://openid.net/specs/openid-connect-core-1_0.html"/>.
        /// </summary>
        /// <param name="expectedValue">The expected value of the hash. normally the c_hash or at_hash claim.</param>
        /// <param name="hashItem">Item to be hashed per oidc spec.</param>
        /// <param name="algorithm">Algorithm for computing hash over hashItem.</param>
        /// <exception cref="OpenIdConnectProtocolException">Thrown if the expected value does not equal the hashed value.</exception>
        private void ValidateHash(string expectedValue, string hashItem, string algorithm)
        {
            if (LogHelper.IsEnabled(EventLogLevel.Informational))
                LogHelper.LogInformation(LogMessages.IDX21303, expectedValue);

            HashAlgorithm hashAlgorithm = null;
            try
            {
                hashAlgorithm = GetHashAlgorithm(algorithm);
                CheckHash(hashAlgorithm, expectedValue, hashItem, algorithm);
            }
            finally
            {
                CryptoProviderFactory.ReleaseHashAlgorithm(hashAlgorithm);
            }
        }

        private static void CheckHash(HashAlgorithm hashAlgorithm, string expectedValue, string hashItem, string algorithm)
        {
            var hashBytes = hashAlgorithm.ComputeHash(Encoding.ASCII.GetBytes(hashItem));
            var hashString = Base64UrlEncoder.Encode(hashBytes, 0, hashBytes.Length / 2);
            if (!string.Equals(expectedValue, hashString))
            {
                throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogHelper.FormatInvariant(LogMessages.IDX21300, expectedValue, hashItem, LogHelper.MarkAsNonPII(algorithm))));
            }
        }

        /// <summary>
        /// Validates the 'code' according to <see href="https://openid.net/specs/openid-connect-core-1_0.html"/>.
        /// </summary>
        /// <param name="validationContext">A <see cref="OpenIdConnectProtocolValidationContext"/> that contains the protocol message to validate.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="validationContext"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <see cref="OpenIdConnectProtocolValidationContext.ValidatedIdToken"/> is null.</exception>
        /// <exception cref="OpenIdConnectProtocolInvalidCHashException">Thrown if <paramref name="validationContext"/> contains a 'code' and there is no 'c_hash' claim in the 'id_token'.</exception>
        /// <exception cref="OpenIdConnectProtocolInvalidCHashException">Thrown if <paramref name="validationContext"/> contains a 'code' and the 'c_hash' claim is not a string in the 'id_token'.</exception>
        /// <exception cref="OpenIdConnectProtocolInvalidCHashException">Thrown if the 'c_hash' claim in the 'id_token' does not correspond to the 'code' in the <see cref="OpenIdConnectMessage"/> response.</exception>
        protected virtual void ValidateCHash(OpenIdConnectProtocolValidationContext validationContext)
        {
            LogHelper.LogVerbose(LogMessages.IDX21304);

            if (validationContext == null)
                throw LogHelper.LogArgumentNullException(nameof(validationContext));

            if (validationContext.ValidatedIdToken == null)
                throw LogHelper.LogArgumentNullException(nameof(validationContext.ValidatedIdToken));

            if (validationContext.ProtocolMessage == null)
                throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogMessages.IDX21333));

            if (string.IsNullOrEmpty(validationContext.ProtocolMessage.Code))
            {
                LogHelper.LogInformation(LogMessages.IDX21305);
                return;
            }

            object cHashClaim;
            if (!validationContext.ValidatedIdToken.Payload.TryGetValue(JwtRegisteredClaimNames.CHash, out cHashClaim))
            {
                throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolInvalidCHashException(LogHelper.FormatInvariant(LogMessages.IDX21307, validationContext.ValidatedIdToken)));
            }

            var chash = cHashClaim as string;
            if (chash == null)
            {
                throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolInvalidCHashException(LogHelper.FormatInvariant(LogMessages.IDX21306, validationContext.ValidatedIdToken)));
            }

            var idToken = validationContext.ValidatedIdToken;

            var alg = idToken.InnerToken != null ? idToken.InnerToken.Header.Alg : idToken.Header.Alg;

            try
            {
                ValidateHash(chash, validationContext.ProtocolMessage.Code, alg);
            }
            catch (OpenIdConnectProtocolException ex)
            {
                throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolInvalidCHashException(LogMessages.IDX21347, ex));
            }
        }

        /// <summary>
        /// Validates the 'token' according to <see href="https://openid.net/specs/openid-connect-core-1_0.html"/>.
        /// </summary>
        /// <param name="validationContext">A <see cref="OpenIdConnectProtocolValidationContext"/> that contains the protocol message to validate.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="validationContext"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <see cref="OpenIdConnectProtocolValidationContext.ValidatedIdToken"/> is null.</exception>
        /// <exception cref="OpenIdConnectProtocolInvalidAtHashException">Thrown if the <paramref name="validationContext"/> contains a 'token' and there is no 'at_hash' claim in the id_token.</exception>
        /// <exception cref="OpenIdConnectProtocolInvalidAtHashException">Thrown if the <paramref name="validationContext"/> contains a 'token' and the 'at_hash' claim is not a string in the 'id_token'.</exception>
        /// <exception cref="OpenIdConnectProtocolInvalidAtHashException">Thrown if the 'at_hash' claim in the 'id_token' does not correspond to the 'access_token' in the <see cref="OpenIdConnectMessage"/> response.</exception>
        protected virtual void ValidateAtHash(OpenIdConnectProtocolValidationContext validationContext)
        {
            LogHelper.LogVerbose(LogMessages.IDX21309);

            if (validationContext == null)
                throw LogHelper.LogArgumentNullException("validationContext");

            if (validationContext.ValidatedIdToken == null)
                throw LogHelper.LogArgumentNullException("validationContext.ValidatedIdToken");

            if (validationContext.ProtocolMessage == null)
                throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogMessages.IDX21333));

            if (string.IsNullOrEmpty(validationContext.ProtocolMessage.AccessToken))
            {
                LogHelper.LogInformation(LogMessages.IDX21310);
                return;
            }

            object atHashClaim;
            if (!validationContext.ValidatedIdToken.Payload.TryGetValue(JwtRegisteredClaimNames.AtHash, out atHashClaim))
                throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolInvalidAtHashException(LogHelper.FormatInvariant(LogMessages.IDX21312, validationContext.ValidatedIdToken)));

            var atHash = atHashClaim as string;
            if (atHash == null)
                throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolInvalidAtHashException(LogHelper.FormatInvariant(LogMessages.IDX21311, validationContext.ValidatedIdToken)));

            var idToken = validationContext.ValidatedIdToken;

            var alg = idToken.InnerToken != null ? idToken.InnerToken.Header.Alg : idToken.Header.Alg;

            try
            {
                ValidateHash(atHash, validationContext.ProtocolMessage.AccessToken, alg);
            }
            catch (OpenIdConnectProtocolException ex)
            {
                throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolInvalidAtHashException(LogMessages.IDX21348, ex));
            }
        }

        /// <summary>
        /// Validates that the <see cref="JwtSecurityToken"/> contains the nonce.
        /// </summary>
        /// <param name="validationContext">A <see cref="OpenIdConnectProtocolValidationContext"/> that contains the 'nonce' to validate.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="validationContext"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <see cref="OpenIdConnectProtocolValidationContext.ValidatedIdToken"/> is null.</exception>
        /// <exception cref="OpenIdConnectProtocolInvalidNonceException">Thrown if <see cref="OpenIdConnectProtocolValidationContext.Nonce"/> is null and RequireNonce is true.</exception>
        /// <exception cref="OpenIdConnectProtocolInvalidNonceException">Thrown if the 'nonce' found in the 'id_token' does not match <see cref="OpenIdConnectProtocolValidationContext.Nonce"/>.</exception>
        /// <exception cref="OpenIdConnectProtocolInvalidNonceException">Thrown if <see cref="RequireTimeStampInNonce"/> is true and a timestamp is not: found, well formed, negatire or expired.</exception>
        /// <remarks>The timestamp is only validated if <see cref="RequireTimeStampInNonce"/> is true.
        /// <para>If <see cref="OpenIdConnectProtocolValidationContext.Nonce"/> is not-null, then a matching 'nonce' must exist in the 'id_token'.</para></remarks>
        protected virtual void ValidateNonce(OpenIdConnectProtocolValidationContext validationContext)
        {
            LogHelper.LogVerbose(LogMessages.IDX21319);

            if (validationContext == null)
                throw LogHelper.LogArgumentNullException(nameof(validationContext));

            if (validationContext.ValidatedIdToken == null)
                throw LogHelper.LogArgumentNullException(nameof(validationContext.ValidatedIdToken));

            string nonceFoundInJwt = validationContext.ValidatedIdToken.Payload.Nonce;

            // if a nonce is not required AND there is no nonce in the context (which represents what was returned from the IDP) and the token log and return
            if (!RequireNonce && string.IsNullOrEmpty(validationContext.Nonce) && string.IsNullOrEmpty(nonceFoundInJwt))
            {
                LogHelper.LogInformation(LogMessages.IDX21322);
                return;
            }

            // if we get here then RequireNonce == true || validationContext.None != null || nonceFoundInJwt != null
            if (string.IsNullOrEmpty(validationContext.Nonce) && string.IsNullOrEmpty(nonceFoundInJwt))
                throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolInvalidNonceException(LogHelper.FormatInvariant(LogMessages.IDX21320, LogHelper.MarkAsNonPII(RequireNonce))));
            else if (string.IsNullOrEmpty(validationContext.Nonce))
                throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolInvalidNonceException(LogHelper.FormatInvariant(LogMessages.IDX21323, LogHelper.MarkAsNonPII(RequireNonce))));
            else if (string.IsNullOrEmpty(nonceFoundInJwt))
                throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolInvalidNonceException(LogHelper.FormatInvariant(LogMessages.IDX21349, LogHelper.MarkAsNonPII(RequireNonce))));

            if (!string.Equals(nonceFoundInJwt, validationContext.Nonce))
                throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolInvalidNonceException(LogHelper.FormatInvariant(LogMessages.IDX21321, validationContext.Nonce, nonceFoundInJwt, validationContext.ValidatedIdToken)));

            if (RequireTimeStampInNonce)
            {
                int endOfTimestamp = nonceFoundInJwt.IndexOf('.');
                if (endOfTimestamp == -1)
                    throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolInvalidNonceException(LogHelper.FormatInvariant(LogMessages.IDX21325, nonceFoundInJwt)));

                string timestamp = nonceFoundInJwt.Substring(0, endOfTimestamp);
                DateTime nonceTime = new DateTime(1979, 1, 1); // initializing to some value otherwise it gives an error
                long ticks = -1;
                try
                {
                    ticks = Convert.ToInt64(timestamp, CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolInvalidNonceException(LogHelper.FormatInvariant(LogMessages.IDX21326, LogHelper.MarkAsNonPII(timestamp), nonceFoundInJwt), ex));
                }

                if (ticks <= 0)
                    throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolInvalidNonceException(LogHelper.FormatInvariant(LogMessages.IDX21326, LogHelper.MarkAsNonPII(timestamp), nonceFoundInJwt)));

                try
                {
                    nonceTime = DateTime.FromBinary(ticks);
                }
                catch (Exception ex)
                {
                    throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolInvalidNonceException(LogHelper.FormatInvariant(LogMessages.IDX21327, LogHelper.MarkAsNonPII(timestamp), LogHelper.MarkAsNonPII(DateTime.MinValue.Ticks.ToString(CultureInfo.InvariantCulture)), LogHelper.MarkAsNonPII(DateTime.MaxValue.Ticks.ToString(CultureInfo.InvariantCulture))), ex));
                }

                DateTime utcNow = DateTime.UtcNow;
                if (nonceTime + NonceLifetime < utcNow)
                    throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolInvalidNonceException(LogHelper.FormatInvariant(LogMessages.IDX21324, nonceFoundInJwt, LogHelper.MarkAsNonPII(nonceTime.ToString(CultureInfo.InvariantCulture)), LogHelper.MarkAsNonPII(utcNow.ToString(CultureInfo.InvariantCulture)), LogHelper.MarkAsNonPII(NonceLifetime.ToString("c", CultureInfo.InvariantCulture)))));
            }
        }

        /// <summary>
        /// Validates that the 'state' in message is valid.
        /// </summary>
        /// <param name="validationContext">A <see cref="OpenIdConnectProtocolValidationContext"/> that contains the 'state' to validate.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="validationContext"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <see cref="OpenIdConnectProtocolValidationContext.ProtocolMessage"/> is null.</exception>
        /// <exception cref="OpenIdConnectProtocolInvalidStateException">Thrown if <see cref="OpenIdConnectProtocolValidationContext.State"/> is present in <see cref="OpenIdConnectProtocolValidationContext.State"/> but either <see cref="OpenIdConnectProtocolValidationContext.ProtocolMessage"/> or its state property is null.</exception>
        /// <exception cref="OpenIdConnectProtocolInvalidStateException">Thrown if 'state' in the context does not match the state in the message.</exception>
        protected virtual void ValidateState(OpenIdConnectProtocolValidationContext validationContext)
        {
            if (!RequireStateValidation)
            {
                LogHelper.LogVerbose(LogMessages.IDX21342);
                return;
            }

            if (validationContext == null)
                throw LogHelper.LogArgumentNullException("validationContext");

            if (validationContext.ProtocolMessage == null)
                throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogMessages.IDX21333));

            // if state is missing, but not required just return. Otherwise process it.
            if (!RequireState && string.IsNullOrEmpty(validationContext.State) && string.IsNullOrEmpty(validationContext.ProtocolMessage.State))
            {
                LogHelper.LogInformation(LogMessages.IDX21341);
                return;
            }
            else if (string.IsNullOrEmpty(validationContext.State))
            {
                throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolInvalidStateException(LogHelper.FormatInvariant(LogMessages.IDX21329, LogHelper.MarkAsNonPII(RequireState))));
            }
            else if (string.IsNullOrEmpty(validationContext.ProtocolMessage.State))
            {
                // 'state' was sent, but message does not contain 'state'
                throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolInvalidStateException(LogHelper.FormatInvariant(LogMessages.IDX21330, LogHelper.MarkAsNonPII(RequireState))));
            }

            if (!string.Equals(validationContext.State, validationContext.ProtocolMessage.State))
            {
                throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolInvalidStateException(LogHelper.FormatInvariant(LogMessages.IDX21331, validationContext.State, validationContext.ProtocolMessage.State)));
            }
        }
    }
}
