// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using TokenLogMessages = Microsoft.IdentityModel.Tokens.LogMessages;

namespace Microsoft.IdentityModel.JsonWebTokens
{
    /// <remarks>This partial class contains methods and logic related to the validation of tokens.</remarks>
    public partial class JsonWebTokenHandler : TokenHandler
    {
        /// <summary>
        /// Returns a value that indicates if this handler can validate a <see cref="SecurityToken"/>.
        /// </summary>
        /// <returns><see langword="true"/> if this instance can validate a <see cref="JsonWebToken"/>.</returns>
        public virtual bool CanValidateToken
        {
            get { return true; }
        }

        internal async ValueTask<TokenValidationResult> ValidateJWEAsync(
            JsonWebToken jwtToken,
            TokenValidationParameters validationParameters,
            BaseConfiguration configuration)
        {
            try
            {
                TokenValidationResult tokenValidationResult = ReadToken(DecryptToken(jwtToken, validationParameters, configuration), validationParameters);
                if (!tokenValidationResult.IsValid)
                    return tokenValidationResult;

                tokenValidationResult = await ValidateJWSAsync(
                    tokenValidationResult.SecurityToken as JsonWebToken,
                    validationParameters,
                    configuration).ConfigureAwait(false);

                if (!tokenValidationResult.IsValid)
                    return tokenValidationResult;

                jwtToken.InnerToken = tokenValidationResult.SecurityToken as JsonWebToken;
                jwtToken.Payload = (tokenValidationResult.SecurityToken as JsonWebToken).Payload;
                return new TokenValidationResult
                {
                    SecurityToken = jwtToken,
                    ClaimsIdentityNoLocking = tokenValidationResult.ClaimsIdentityNoLocking,
                    IsValid = true,
                    TokenType = tokenValidationResult.TokenType
                };
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                return new TokenValidationResult
                {
                    Exception = ex,
                    IsValid = false,
                    TokenOnFailedValidation = validationParameters.IncludeTokenOnFailedValidation ? jwtToken : null
                };
            }
        }

        internal async ValueTask<TokenValidationResult> ValidateJWEAsync(
            JsonWebToken jwtToken,
            TokenValidationParameters validationParameters,
            CallContext callContext,
            CancellationToken cancellationToken)
        {
            try
            {
                TokenValidationResult tokenValidationResult = ReadToken(DecryptToken(jwtToken, validationParameters), validationParameters);
                if (!tokenValidationResult.IsValid)
                    return tokenValidationResult;

                tokenValidationResult = await ValidateJWSAsync(
                    tokenValidationResult.SecurityToken as JsonWebToken,
                    validationParameters,
                    callContext,
                    cancellationToken).ConfigureAwait(false);

                if (!tokenValidationResult.IsValid)
                    return tokenValidationResult;

                jwtToken.InnerToken = tokenValidationResult.SecurityToken as JsonWebToken;
                jwtToken.Payload = (tokenValidationResult.SecurityToken as JsonWebToken).Payload;
                return new TokenValidationResult
                {
                    SecurityToken = jwtToken,
                    ClaimsIdentityNoLocking = tokenValidationResult.ClaimsIdentityNoLocking,
                    IsValid = true,
                    TokenType = tokenValidationResult.TokenType
                };
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                return new TokenValidationResult
                {
                    Exception = ex,
                    IsValid = false,
                    TokenOnFailedValidation = validationParameters.IncludeTokenOnFailedValidation ? jwtToken : null
                };
            }
        }

        internal async ValueTask<TokenValidationResult> ValidateJWSAsync(
            JsonWebToken jsonWebToken,
            TokenValidationParameters validationParameters,
            BaseConfiguration configuration)
        {
            try
            {
                TokenValidationResult tokenValidationResult;
                if (validationParameters.TransformBeforeSignatureValidation != null)
                    jsonWebToken = validationParameters.TransformBeforeSignatureValidation(jsonWebToken, validationParameters) as JsonWebToken;

                if (validationParameters.SignatureValidator != null || validationParameters.SignatureValidatorUsingConfiguration != null)
                {
                    var validatedToken = ValidateSignatureUsingDelegates(jsonWebToken, validationParameters);
                    tokenValidationResult = await ValidateTokenPayloadAsync(
                        validatedToken,
                        validationParameters,
                        configuration).ConfigureAwait(false);

                    Validators.ValidateIssuerSecurityKey(validatedToken.SigningKey, validatedToken, validationParameters);
                }
                else
                {
                    if (validationParameters.ValidateSignatureLast)
                    {
                        tokenValidationResult = await ValidateTokenPayloadAsync(
                            jsonWebToken,
                            validationParameters,
                            configuration).ConfigureAwait(false);

                        if (tokenValidationResult.IsValid)
                            tokenValidationResult.SecurityToken = ValidateSignatureAndIssuerSecurityKey(jsonWebToken, validationParameters, configuration);
                    }
                    else
                    {
                        var validatedToken = ValidateSignatureAndIssuerSecurityKey(jsonWebToken, validationParameters, configuration);
                        tokenValidationResult = await ValidateTokenPayloadAsync(
                            validatedToken,
                            validationParameters,
                            configuration).ConfigureAwait(false);
                    }
                }

                return tokenValidationResult;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                return new TokenValidationResult
                {
                    Exception = ex,
                    IsValid = false,
                    TokenOnFailedValidation = validationParameters.IncludeTokenOnFailedValidation ? jsonWebToken : null
                };
            }
        }

        internal async ValueTask<TokenValidationResult> ValidateJWSAsync(
            JsonWebToken jsonWebToken,
            TokenValidationParameters validationParameters,
            CallContext callContext,
            CancellationToken cancellationToken)
        {
            try
            {
                BaseConfiguration currentConfiguration = null;
                if (validationParameters.ConfigurationManager != null)
                {
                    try
                    {
                        currentConfiguration = await validationParameters.ConfigurationManager.GetBaseConfigurationAsync(CancellationToken.None).ConfigureAwait(false);
                    }
#pragma warning disable CA1031 // Do not catch general exception types
                    catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
                    {
                        // The exception is not re-thrown as the TokenValidationParameters may have the issuer and signing key set
                        // directly on them, allowing the library to continue with token validation.
                        if (LogHelper.IsEnabled(EventLogLevel.Warning))
                            LogHelper.LogWarning(LogHelper.FormatInvariant(TokenLogMessages.IDX10261, validationParameters.ConfigurationManager.MetadataAddress, ex.ToString()));
                    }
                }

                TokenValidationResult tokenValidationResult;
                if (validationParameters.TransformBeforeSignatureValidation != null)
                    jsonWebToken = validationParameters.TransformBeforeSignatureValidation(jsonWebToken, validationParameters) as JsonWebToken;

                if (validationParameters.SignatureValidator != null || validationParameters.SignatureValidatorUsingConfiguration != null)
                {
                    var validatedToken = ValidateSignatureUsingDelegates(jsonWebToken, validationParameters);
                    tokenValidationResult = await ValidateTokenPayloadAsync(
                        validatedToken,
                        validationParameters,
                        callContext,
                        cancellationToken).ConfigureAwait(false);

                    Validators.ValidateIssuerSecurityKey(validatedToken.SigningKey, validatedToken, validationParameters);
                }
                else
                {
                    if (validationParameters.ValidateSignatureLast)
                    {
                        tokenValidationResult = await ValidateTokenPayloadAsync(
                            jsonWebToken,
                            validationParameters,
                            callContext,
                            cancellationToken).ConfigureAwait(false);

                        if (tokenValidationResult.IsValid)
                            tokenValidationResult.SecurityToken = ValidateSignatureAndIssuerSecurityKey(jsonWebToken, validationParameters, currentConfiguration);
                    }
                    else
                    {
                        var validatedToken = ValidateSignatureAndIssuerSecurityKey(jsonWebToken, validationParameters, currentConfiguration);
                        tokenValidationResult = await ValidateTokenPayloadAsync(
                            validatedToken,
                            validationParameters,
                            callContext,
                            cancellationToken).ConfigureAwait(false);
                    }
                }

                return tokenValidationResult;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                return new TokenValidationResult
                {
                    Exception = ex,
                    IsValid = false,
                    TokenOnFailedValidation = validationParameters.IncludeTokenOnFailedValidation ? jsonWebToken : null
                };
            }
        }

        private static JsonWebToken ValidateSignatureAndIssuerSecurityKey(JsonWebToken jsonWebToken, TokenValidationParameters validationParameters, BaseConfiguration configuration)
        {
            JsonWebToken validatedToken = ValidateSignature(jsonWebToken, validationParameters, configuration);
            Validators.ValidateIssuerSecurityKey(validatedToken.SigningKey, jsonWebToken, validationParameters, configuration);
            return validatedToken;
        }

        /// <summary>
        /// Validates the JWT signature.
        /// </summary>
        private static JsonWebToken ValidateSignature(JsonWebToken jwtToken, TokenValidationParameters validationParameters, BaseConfiguration configuration)
        {
            bool kidMatched = false;
            IEnumerable<SecurityKey> keys = null;

            if (!jwtToken.IsSigned)
            {
                if (validationParameters.RequireSignedTokens)
                    throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidSignatureException(LogHelper.FormatInvariant(TokenLogMessages.IDX10504, jwtToken)));
                else
                    return jwtToken;
            }

            if (validationParameters.IssuerSigningKeyResolverUsingConfiguration != null)
            {
                keys = validationParameters.IssuerSigningKeyResolverUsingConfiguration(jwtToken.EncodedToken, jwtToken, jwtToken.Kid, validationParameters, configuration);
            }
            else if (validationParameters.IssuerSigningKeyResolver != null)
            {
                keys = validationParameters.IssuerSigningKeyResolver(jwtToken.EncodedToken, jwtToken, jwtToken.Kid, validationParameters);
            }
            else
            {
                var key = JwtTokenUtilities.ResolveTokenSigningKey(jwtToken.Kid, jwtToken.X5t, validationParameters, configuration);
                if (key != null)
                {
                    kidMatched = true;
                    keys = [key];
                }
            }

            if (validationParameters.TryAllIssuerSigningKeys && keys.IsNullOrEmpty())
            {
                // control gets here if:
                // 1. User specified delegate: IssuerSigningKeyResolver returned null
                // 2. ResolveIssuerSigningKey returned null
                // Try all the keys. This is the degenerate case, not concerned about perf.
                keys = TokenUtilities.GetAllSigningKeys(configuration, validationParameters);
            }

            // keep track of exceptions thrown, keys that were tried
            StringBuilder exceptionStrings = null;
            StringBuilder keysAttempted = null;
            var kidExists = !string.IsNullOrEmpty(jwtToken.Kid);

            if (keys != null)
            {
                foreach (var key in keys)
                {
#pragma warning disable CA1031 // Do not catch general exception types
                    try
                    {
                        if (ValidateSignature(jwtToken, key, validationParameters))
                        {
                            if (LogHelper.IsEnabled(EventLogLevel.Informational))
                                LogHelper.LogInformation(TokenLogMessages.IDX10242, jwtToken);

                            jwtToken.SigningKey = key;
                            return jwtToken;
                        }
                    }
                    catch (Exception ex)
                    {
                        (exceptionStrings ??= new StringBuilder()).AppendLine(ex.ToString());
                    }
#pragma warning restore CA1031 // Do not catch general exception types

                    if (key != null)
                    {
                        (keysAttempted ??= new StringBuilder()).Append(key.ToString()).Append(" , KeyId: ").AppendLine(key.KeyId);
                        if (kidExists && !kidMatched && key.KeyId != null)
                            kidMatched = jwtToken.Kid.Equals(key.KeyId, key is X509SecurityKey ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
                    }
                }
            }

            // Get information on where keys used during token validation came from for debugging purposes.
            var keysInTokenValidationParameters = TokenUtilities.GetAllSigningKeys(validationParameters: validationParameters);

            var keysInConfiguration = TokenUtilities.GetAllSigningKeys(configuration);
            var numKeysInTokenValidationParameters = keysInTokenValidationParameters.Count();
            var numKeysInConfiguration = keysInConfiguration.Count();

            if (kidExists)
            {
                if (kidMatched)
                {
                    JsonWebToken localJwtToken = jwtToken; // avoid closure on non-exceptional path
                    var isKidInTVP = keysInTokenValidationParameters.Any(x => x.KeyId.Equals(localJwtToken.Kid));
                    var keyLocation = isKidInTVP ? "TokenValidationParameters" : "Configuration";
                    throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidSignatureException(LogHelper.FormatInvariant(TokenLogMessages.IDX10511,
                        LogHelper.MarkAsNonPII((object)keysAttempted ?? ""),
                        LogHelper.MarkAsNonPII(numKeysInTokenValidationParameters),
                        LogHelper.MarkAsNonPII(numKeysInConfiguration),
                        LogHelper.MarkAsNonPII(keyLocation),
                        LogHelper.MarkAsNonPII(jwtToken.Kid),
                        (object)exceptionStrings ?? "",
                        jwtToken)));
                }

                if (!validationParameters.ValidateSignatureLast)
                {
                    InternalValidators.ValidateAfterSignatureFailed(
                        jwtToken,
                        jwtToken.ValidFromNullable,
                        jwtToken.ValidToNullable,
                        jwtToken.Audiences,
                        validationParameters,
                        configuration);
                }
            }

            if (keysAttempted is not null)
            {
                if (kidExists)
                {
                    throw LogHelper.LogExceptionMessage(new SecurityTokenSignatureKeyNotFoundException(LogHelper.FormatInvariant(TokenLogMessages.IDX10503,
                        LogHelper.MarkAsNonPII(jwtToken.Kid),
                        LogHelper.MarkAsNonPII((object)keysAttempted ?? ""),
                        LogHelper.MarkAsNonPII(numKeysInTokenValidationParameters),
                        LogHelper.MarkAsNonPII(numKeysInConfiguration),
                        (object)exceptionStrings ?? "",
                        jwtToken)));
                }
                else
                {
                    throw LogHelper.LogExceptionMessage(new SecurityTokenSignatureKeyNotFoundException(LogHelper.FormatInvariant(TokenLogMessages.IDX10517,
                        LogHelper.MarkAsNonPII((object)keysAttempted ?? ""),
                        LogHelper.MarkAsNonPII(numKeysInTokenValidationParameters),
                        LogHelper.MarkAsNonPII(numKeysInConfiguration),
                        (object)exceptionStrings ?? "",
                        jwtToken)));
                }
            }

            throw LogHelper.LogExceptionMessage(new SecurityTokenSignatureKeyNotFoundException(TokenLogMessages.IDX10500));
        }

        internal static bool IsSignatureValid(byte[] signatureBytes, int signatureBytesLength, SignatureProvider signatureProvider, byte[] dataToVerify, int dataToVerifyLength)
        {
            if (signatureProvider is SymmetricSignatureProvider)
            {
                return signatureProvider.Verify(dataToVerify, 0, dataToVerifyLength, signatureBytes, 0, signatureBytesLength);
            }
            else
            {
                if (signatureBytes.Length == signatureBytesLength)
                {
                    return signatureProvider.Verify(dataToVerify, 0, dataToVerifyLength, signatureBytes, 0, signatureBytesLength);
                }
                else
                {
                    byte[] sigBytes = new byte[signatureBytesLength];
                    Array.Copy(signatureBytes, 0, sigBytes, 0, signatureBytesLength);
                    return signatureProvider.Verify(dataToVerify, 0, dataToVerifyLength, sigBytes, 0, signatureBytesLength);
                }
            }
        }

        internal static bool ValidateSignature(byte[] bytes, int len, string stringWithSignature, int signatureStartIndex, SignatureProvider signatureProvider)
        {
            return Base64UrlEncoding.Decode<bool, SignatureProvider, byte[], int>(
                    stringWithSignature,
                    signatureStartIndex + 1,
                    stringWithSignature.Length - signatureStartIndex - 1,
                    signatureProvider,
                    bytes,
                    len,
                    IsSignatureValid);
        }

        internal static bool ValidateSignature(JsonWebToken jsonWebToken, SecurityKey key, TokenValidationParameters validationParameters)
        {
            var cryptoProviderFactory = validationParameters.CryptoProviderFactory ?? key.CryptoProviderFactory;
            if (!cryptoProviderFactory.IsSupportedAlgorithm(jsonWebToken.Alg, key))
            {
                if (LogHelper.IsEnabled(EventLogLevel.Informational))
                    LogHelper.LogInformation(LogMessages.IDX14000, LogHelper.MarkAsNonPII(jsonWebToken.Alg), key);

                return false;
            }

            Validators.ValidateAlgorithm(jsonWebToken.Alg, key, jsonWebToken, validationParameters);
            var signatureProvider = cryptoProviderFactory.CreateForVerifying(key, jsonWebToken.Alg);
            try
            {
                if (signatureProvider == null)
                    throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant(TokenLogMessages.IDX10636, key == null ? "Null" : key.ToString(), LogHelper.MarkAsNonPII(jsonWebToken.Alg))));

                return EncodingUtils.PerformEncodingDependentOperation<bool, string, int, SignatureProvider>(
                    jsonWebToken.EncodedToken,
                    0,
                    jsonWebToken.Dot2,
                    Encoding.UTF8,
                    jsonWebToken.EncodedToken,
                    jsonWebToken.Dot2,
                    signatureProvider,
                    ValidateSignature);
            }
            finally
            {
                cryptoProviderFactory.ReleaseSignatureProvider(signatureProvider);
            }
        }

        private static JsonWebToken ValidateSignatureUsingDelegates(JsonWebToken jsonWebToken, TokenValidationParameters validationParameters)
        {
            if (validationParameters.SignatureValidatorUsingConfiguration != null)
            {
                // TODO - get configuration from validationParameters
                BaseConfiguration configuration = null;
                var validatedToken = validationParameters.SignatureValidatorUsingConfiguration(jsonWebToken.EncodedToken, validationParameters, configuration);
                if (validatedToken == null)
                    throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidSignatureException(LogHelper.FormatInvariant(TokenLogMessages.IDX10505, jsonWebToken)));

                if (!(validatedToken is JsonWebToken validatedJsonWebToken))
                    throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidSignatureException(LogHelper.FormatInvariant(TokenLogMessages.IDX10506, LogHelper.MarkAsNonPII(typeof(JsonWebToken)), LogHelper.MarkAsNonPII(validatedToken.GetType()), jsonWebToken)));

                return validatedJsonWebToken;
            }
            else if (validationParameters.SignatureValidator != null)
            {
                var validatedToken = validationParameters.SignatureValidator(jsonWebToken.EncodedToken, validationParameters);
                if (validatedToken == null)
                    throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidSignatureException(LogHelper.FormatInvariant(TokenLogMessages.IDX10505, jsonWebToken)));

                if (!(validatedToken is JsonWebToken validatedJsonWebToken))
                    throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidSignatureException(LogHelper.FormatInvariant(TokenLogMessages.IDX10506, LogHelper.MarkAsNonPII(typeof(JsonWebToken)), LogHelper.MarkAsNonPII(validatedToken.GetType()), jsonWebToken)));

                return validatedJsonWebToken;
            }

            throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidSignatureException(LogHelper.FormatInvariant(TokenLogMessages.IDX10505, jsonWebToken)));
        }

        /// <summary>
        /// Validates a JWS or a JWE.
        /// </summary>
        /// <param name="token">A JSON Web Token (JWT) in JWS or JWE Compact Serialization format.</param>
        /// <param name="validationParameters">The <see cref="TokenValidationParameters"/> to be used for validating the token.</param>
        /// <returns>A <see cref="TokenValidationResult"/>.</returns>
        [Obsolete("`JsonWebTokens.ValidateToken(string, TokenValidationParameters)` has been deprecated and will be removed in a future release. Use `JsonWebTokens.ValidateTokenAsync(string, TokenValidationParameters)` instead. For more information, see https://aka.ms/IdentityModel/7-breaking-changes", false)]
        public virtual TokenValidationResult ValidateToken(string token, TokenValidationParameters validationParameters)
        {
            return ValidateTokenAsync(token, validationParameters).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Validates a token.
        /// On a validation failure, no exception will be thrown; instead, the exception will be set in the returned TokenValidationResult.Exception property.
        /// Callers should always check the TokenValidationResult.IsValid property to verify the validity of the result.
        /// </summary>
        /// <param name="token">The token to be validated.</param>
        /// <param name="validationParameters">The <see cref="TokenValidationParameters"/> to be used for validating the token.</param>
        /// <returns>A <see cref="TokenValidationResult"/>.</returns>
        /// <remarks>
        /// <para>TokenValidationResult.Exception will be set to one of the following exceptions if the <paramref name="token"/> is invalid.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="token"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="validationParameters"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if 'token.Length' is greater than <see cref="TokenHandler.MaximumTokenSizeInBytes"/>.</exception>
        /// <exception cref="SecurityTokenMalformedException">Thrown if <paramref name="token"/> is not a valid <see cref="JsonWebToken"/>, <see cref="ReadToken(string, TokenValidationParameters)"/></exception>
        /// <exception cref="SecurityTokenMalformedException">Thrown if the validationParameters.TokenReader delegate is not able to parse/read the token as a valid <see cref="JsonWebToken"/>, <see cref="ReadToken(string, TokenValidationParameters)"/></exception>
        public override async Task<TokenValidationResult> ValidateTokenAsync(string token, TokenValidationParameters validationParameters)
        {
            if (string.IsNullOrEmpty(token))
                return new TokenValidationResult { Exception = LogHelper.LogArgumentNullException(nameof(token)), IsValid = false };

            if (validationParameters == null)
                return new TokenValidationResult { Exception = LogHelper.LogArgumentNullException(nameof(validationParameters)), IsValid = false };

            if (token.Length > MaximumTokenSizeInBytes)
                return new TokenValidationResult { Exception = LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(TokenLogMessages.IDX10209, LogHelper.MarkAsNonPII(token.Length), LogHelper.MarkAsNonPII(MaximumTokenSizeInBytes)))), IsValid = false };

            try
            {
                TokenValidationResult result = ReadToken(token, validationParameters);
                if (result.IsValid)
                    return await ValidateTokenAsync(result.SecurityToken, validationParameters).ConfigureAwait(false);

                return result;
            }
            catch (Exception ex)
            {
                return new TokenValidationResult
                {
                    Exception = ex,
                    IsValid = false
                };
            }
        }

        /// <inheritdoc/>
        public override async Task<TokenValidationResult> ValidateTokenAsync(SecurityToken token, TokenValidationParameters validationParameters)
        {
            if (token == null)
                throw LogHelper.LogArgumentNullException(nameof(token));

            if (validationParameters == null)
                return new TokenValidationResult { Exception = LogHelper.LogArgumentNullException(nameof(validationParameters)), IsValid = false };

            var jwt = token as JsonWebToken;
            if (jwt == null)
                return new TokenValidationResult { Exception = LogHelper.LogArgumentException<ArgumentException>(nameof(token), $"{nameof(token)} must be a {nameof(JsonWebToken)}."), IsValid = false };

            try
            {
                return await ValidateTokenAsync(jwt, validationParameters).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return new TokenValidationResult
                {
                    Exception = ex,
                    IsValid = false
                };
            }
        }

        /// <summary>
        ///  Internal method for token validation, responsible for:
        ///  (1) Obtaining a configuration from the <see cref="TokenValidationParameters.ConfigurationManager"/>.
        ///  (2) Revalidating using the Last Known Good Configuration (if present), and obtaining a refreshed configuration (if necessary) and revalidating using it.
        /// </summary>
        /// <param name="jsonWebToken">The JWT token.</param>
        /// <param name="validationParameters">The <see cref="TokenValidationParameters"/> to be used for validating the token.</param>
        /// <returns></returns>
        internal async ValueTask<TokenValidationResult> ValidateTokenAsync(
            JsonWebToken jsonWebToken,
            TokenValidationParameters validationParameters)
        {
            BaseConfiguration currentConfiguration = null;
            if (validationParameters.ConfigurationManager != null)
            {
                try
                {
                    currentConfiguration = await validationParameters.ConfigurationManager.GetBaseConfigurationAsync(CancellationToken.None).ConfigureAwait(false);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    // The exception is not re-thrown as the TokenValidationParameters may have the issuer and signing key set
                    // directly on them, allowing the library to continue with token validation.
                    if (LogHelper.IsEnabled(EventLogLevel.Warning))
                        LogHelper.LogWarning(LogHelper.FormatInvariant(TokenLogMessages.IDX10261, validationParameters.ConfigurationManager.MetadataAddress, ex.ToString()));
                }
            }

            TokenValidationResult tokenValidationResult =  jsonWebToken.IsEncrypted ?
                await ValidateJWEAsync(jsonWebToken, validationParameters, currentConfiguration).ConfigureAwait(false) :
                await ValidateJWSAsync(jsonWebToken, validationParameters, currentConfiguration).ConfigureAwait(false);

            if (validationParameters.ConfigurationManager != null)
            {
                if (tokenValidationResult.IsValid)
                {
                    // Set current configuration as LKG if it exists.
                    if (currentConfiguration != null)
                        validationParameters.ConfigurationManager.LastKnownGoodConfiguration = currentConfiguration;

                    return tokenValidationResult;
                }
                else if (TokenUtilities.IsRecoverableException(tokenValidationResult.Exception))
                {
                    // If we were still unable to validate, attempt to refresh the configuration and validate using it
                    // but ONLY if the currentConfiguration is not null. We want to avoid refreshing the configuration on
                    // retrieval error as this case should have already been hit before. This refresh handles the case
                    // where a new valid configuration was somehow published during validation time.
                    if (currentConfiguration != null)
                    {
                        validationParameters.ConfigurationManager.RequestRefresh();
                        validationParameters.RefreshBeforeValidation = true;
                        var lastConfig = currentConfiguration;
                        currentConfiguration = await validationParameters.ConfigurationManager.GetBaseConfigurationAsync(CancellationToken.None).ConfigureAwait(false);

                        // Only try to re-validate using the newly obtained config if it doesn't reference equal the previously used configuration.
                        if (lastConfig != currentConfiguration)
                        {
                            tokenValidationResult = jsonWebToken.IsEncrypted ?
                                await ValidateJWEAsync(jsonWebToken, validationParameters, currentConfiguration).ConfigureAwait(false) :
                                await ValidateJWSAsync(jsonWebToken, validationParameters, currentConfiguration).ConfigureAwait(false);

                            if (tokenValidationResult.IsValid)
                            {
                                validationParameters.ConfigurationManager.LastKnownGoodConfiguration = currentConfiguration;
                                return tokenValidationResult;
                            }
                        }
                    }

                    if (validationParameters.ConfigurationManager.UseLastKnownGoodConfiguration)
                    {
                        validationParameters.RefreshBeforeValidation = false;
                        validationParameters.ValidateWithLKG = true;
                        var recoverableException = tokenValidationResult.Exception;

                        foreach (BaseConfiguration lkgConfiguration in validationParameters.ConfigurationManager.GetValidLkgConfigurations())
                        {
                            if (!lkgConfiguration.Equals(currentConfiguration) && TokenUtilities.IsRecoverableConfiguration(jsonWebToken.Kid, currentConfiguration, lkgConfiguration, recoverableException))
                            {
                                tokenValidationResult = jsonWebToken.IsEncrypted ?
                                    await ValidateJWEAsync(jsonWebToken, validationParameters, lkgConfiguration).ConfigureAwait(false) :
                                    await ValidateJWSAsync(jsonWebToken, validationParameters, lkgConfiguration).ConfigureAwait(false);

                                if (tokenValidationResult.IsValid)
                                    return tokenValidationResult;
                            }
                        }
                    }
                }
            }

            return tokenValidationResult;
        }

        internal async ValueTask<TokenValidationResult> ValidateTokenPayloadAsync(
            JsonWebToken jsonWebToken,
            TokenValidationParameters validationParameters,
            BaseConfiguration configuration)
        {
            var expires = jsonWebToken.HasPayloadClaim(JwtRegisteredClaimNames.Exp) ? (DateTime?)jsonWebToken.ValidTo : null;
            var notBefore = jsonWebToken.HasPayloadClaim(JwtRegisteredClaimNames.Nbf) ? (DateTime?)jsonWebToken.ValidFrom : null;

            Validators.ValidateLifetime(notBefore, expires, jsonWebToken, validationParameters);
            Validators.ValidateAudience(jsonWebToken.Audiences, jsonWebToken, validationParameters);
            string issuer = await Validators.ValidateIssuerAsync(jsonWebToken.Issuer, jsonWebToken, validationParameters, configuration).ConfigureAwait(false);

            Validators.ValidateTokenReplay(expires, jsonWebToken.EncodedToken, validationParameters);
            if (validationParameters.ValidateActor && !string.IsNullOrWhiteSpace(jsonWebToken.Actor))
            {
                // Infinite recursion should not occur here, as the JsonWebToken passed into this method is (1) constructed from a string
                // AND (2) the signature is successfully validated on it. (1) implies that even if there are nested actor tokens,
                // they must end at some point since they cannot reference one another. (2) means that the token has a valid signature
                // and (since issuer validation occurs first) came from a trusted authority.
                // NOTE: More than one nested actor token should not be considered a valid token, but if we somehow encounter one,
                // this code will still work properly.
                TokenValidationResult tokenValidationResult =
                    await ValidateTokenAsync(jsonWebToken.Actor, validationParameters.ActorValidationParameters ?? validationParameters).ConfigureAwait(false);

                if (!tokenValidationResult.IsValid)
                    return tokenValidationResult;
            }

            string tokenType = Validators.ValidateTokenType(jsonWebToken.Typ, jsonWebToken, validationParameters);
            return new TokenValidationResult(jsonWebToken, this, validationParameters.Clone(), issuer)
            {
                IsValid = true,
                TokenType = tokenType
            };
        }

        internal async ValueTask<TokenValidationResult> ValidateTokenPayloadAsync(
            JsonWebToken jsonWebToken,
            TokenValidationParameters validationParameters,
            CallContext callContext,
            CancellationToken cancellationToken)
        {
            var expires = jsonWebToken.HasPayloadClaim(JwtRegisteredClaimNames.Exp) ? (DateTime?)jsonWebToken.ValidTo : null;
            var notBefore = jsonWebToken.HasPayloadClaim(JwtRegisteredClaimNames.Nbf) ? (DateTime?)jsonWebToken.ValidFrom : null;

            Validators.ValidateLifetime(notBefore, expires, jsonWebToken, validationParameters);
            Validators.ValidateAudience(jsonWebToken.Audiences, jsonWebToken, validationParameters);

            IssuerValidationResult issuerValidationResult = await Validators.ValidateIssuerAsync(
                jsonWebToken.Issuer,
                jsonWebToken,
                validationParameters,
                callContext,
                cancellationToken).ConfigureAwait(false);

            if (!issuerValidationResult.IsValid)
            {
                return new TokenValidationResult(jsonWebToken, this, validationParameters, issuerValidationResult.Issuer)
                {
                    IsValid = false,
                    Exception = issuerValidationResult.Exception
                };
            }

            Validators.ValidateTokenReplay(expires, jsonWebToken.EncodedToken, validationParameters);
            if (validationParameters.ValidateActor && !string.IsNullOrWhiteSpace(jsonWebToken.Actor))
            {
                // Infinite recursion should not occur here, as the JsonWebToken passed into this method is (1) constructed from a string
                // AND (2) the signature is successfully validated on it. (1) implies that even if there are nested actor tokens,
                // they must end at some point since they cannot reference one another. (2) means that the token has a valid signature
                // and (since issuer validation occurs first) came from a trusted authority.
                // NOTE: More than one nested actor token should not be considered a valid token, but if we somehow encounter one,
                // this code will still work properly.
                TokenValidationResult tokenValidationResult =
                    await ValidateTokenAsync(jsonWebToken.Actor, validationParameters.ActorValidationParameters ?? validationParameters).ConfigureAwait(false);

                if (!tokenValidationResult.IsValid)
                    return tokenValidationResult;
            }

            string tokenType = Validators.ValidateTokenType(jsonWebToken.Typ, jsonWebToken, validationParameters);
            return new TokenValidationResult(jsonWebToken, this, validationParameters.Clone(), issuerValidationResult.Issuer)
            {
                IsValid = true,
                TokenType = tokenType
            };
        }
    }
}
