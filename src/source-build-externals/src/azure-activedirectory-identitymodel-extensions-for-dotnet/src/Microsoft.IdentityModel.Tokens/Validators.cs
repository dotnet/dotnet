// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// AudienceValidator
    /// </summary>
    public static partial class Validators
    {
        /// <summary>
        /// Validates if a given algorithm for a <see cref="SecurityKey"/> is valid.
        /// </summary>
        /// <param name="algorithm">The algorithm to be validated.</param>
        /// <param name="securityKey">The <see cref="SecurityKey"/> that signed the <see cref="SecurityToken"/>.</param>
        /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
        /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
        public static void ValidateAlgorithm(string algorithm, SecurityKey securityKey, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (validationParameters == null)
                throw LogHelper.LogArgumentNullException(nameof(validationParameters));

            if (validationParameters.AlgorithmValidator != null)
            {
                if (!validationParameters.AlgorithmValidator(algorithm, securityKey, securityToken, validationParameters))
                {
                    throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidAlgorithmException(LogHelper.FormatInvariant(LogMessages.IDX10697, LogHelper.MarkAsNonPII(algorithm), securityKey))
                    {
                        InvalidAlgorithm = algorithm,
                    });
                }

                return;
            }

            if (validationParameters.ValidAlgorithms != null && validationParameters.ValidAlgorithms.Any() && !validationParameters.ValidAlgorithms.Contains(algorithm, StringComparer.Ordinal))
            {
                throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidAlgorithmException(LogHelper.FormatInvariant(LogMessages.IDX10696, LogHelper.MarkAsNonPII(algorithm)))
                {
                    InvalidAlgorithm = algorithm,
                });
            }
        }

        /// <summary>
        /// Validates the <see cref="SecurityKey"/> that signed a <see cref="SecurityToken"/>.
        /// </summary>
        /// <param name="securityKey">The <see cref="SecurityKey"/> that signed the <see cref="SecurityToken"/>.</param>
        /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
        /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
        /// <exception cref="ArgumentNullException"> if 'securityKey' is null and ValidateIssuerSigningKey is true.</exception>
        /// <exception cref="ArgumentNullException"> if 'securityToken' is null and ValidateIssuerSigningKey is true.</exception>
        /// <exception cref="ArgumentNullException"> if 'validationParameters' is null.</exception>
        public static void ValidateIssuerSecurityKey(SecurityKey securityKey, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            ValidateIssuerSecurityKey(securityKey, securityToken, validationParameters, null);
        }

        /// <summary>
        /// Validates the <see cref="SecurityKey"/> that signed a <see cref="SecurityToken"/>.
        /// </summary>
        /// <param name="securityKey">The <see cref="SecurityKey"/> that signed the <see cref="SecurityToken"/>.</param>
        /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
        /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
        /// <param name="configuration">The <see cref="BaseConfiguration"/> required for issuer and signing key validation.</param>
        /// <exception cref="ArgumentNullException"> if 'securityKey' is null and ValidateIssuerSigningKey is true.</exception>
        /// <exception cref="ArgumentNullException"> if 'securityToken' is null and ValidateIssuerSigningKey is true.</exception>
        /// <exception cref="ArgumentNullException"> if 'validationParameters' is null.</exception>
        internal static void ValidateIssuerSecurityKey(SecurityKey securityKey, SecurityToken securityToken, TokenValidationParameters validationParameters, BaseConfiguration configuration)
        {
            if (validationParameters == null)
                throw LogHelper.LogArgumentNullException(nameof(validationParameters));

            if (validationParameters.IssuerSigningKeyValidatorUsingConfiguration != null)
            {
                if (!validationParameters.IssuerSigningKeyValidatorUsingConfiguration(securityKey, securityToken, validationParameters, configuration))
                    throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidSigningKeyException(LogHelper.FormatInvariant(LogMessages.IDX10232, securityKey)) { SigningKey = securityKey });

                return;
            }

            if (validationParameters.IssuerSigningKeyValidator != null)
            {
                if (!validationParameters.IssuerSigningKeyValidator(securityKey, securityToken, validationParameters))
                    throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidSigningKeyException(LogHelper.FormatInvariant(LogMessages.IDX10232, securityKey)) { SigningKey = securityKey });

                return;
            }

            if (!validationParameters.ValidateIssuerSigningKey)
            {
                LogHelper.LogVerbose(LogMessages.IDX10237);
                return;
            }

            if (!validationParameters.RequireSignedTokens && securityKey == null)
            {
                LogHelper.LogInformation(LogMessages.IDX10252);
                return;
            }
            else if (securityKey == null)
            {
                throw LogHelper.LogExceptionMessage(new ArgumentNullException(nameof(securityKey), LogMessages.IDX10253));
            }

            if (securityToken == null)
                throw LogHelper.LogArgumentNullException(nameof(securityToken));

            ValidateIssuerSigningKeyLifeTime(securityKey, validationParameters);
        }

        /// <summary>
        /// Given a signing key, when it's derived from a certificate, validates that the certificate is already active and non-expired
        /// </summary>
        /// <param name="securityKey">The <see cref="SecurityKey"/> that signed the <see cref="SecurityToken"/>.</param>
        /// <param name="validationParameters">The <see cref="TokenValidationParameters"/> that are used to validate the token.</param>
        internal static void ValidateIssuerSigningKeyLifeTime(SecurityKey securityKey, TokenValidationParameters validationParameters)
        {
            X509SecurityKey x509SecurityKey = securityKey as X509SecurityKey;
            if (x509SecurityKey?.Certificate is X509Certificate2 cert)
            {
                DateTime utcNow = DateTime.UtcNow;
                var notBeforeUtc = cert.NotBefore.ToUniversalTime();
                var notAfterUtc = cert.NotAfter.ToUniversalTime();

                if (notBeforeUtc > DateTimeUtil.Add(utcNow, validationParameters.ClockSkew))
                    throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidSigningKeyException(LogHelper.FormatInvariant(LogMessages.IDX10248, LogHelper.MarkAsNonPII(notBeforeUtc), LogHelper.MarkAsNonPII(utcNow))));

                if (LogHelper.IsEnabled(EventLogLevel.Informational))
                    LogHelper.LogInformation(LogMessages.IDX10250, LogHelper.MarkAsNonPII(notBeforeUtc), LogHelper.MarkAsNonPII(utcNow));

                if (notAfterUtc < DateTimeUtil.Add(utcNow, validationParameters.ClockSkew.Negate()))
                    throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidSigningKeyException(LogHelper.FormatInvariant(LogMessages.IDX10249, LogHelper.MarkAsNonPII(notAfterUtc), LogHelper.MarkAsNonPII(utcNow))));

                if (LogHelper.IsEnabled(EventLogLevel.Informational))
                    LogHelper.LogInformation(LogMessages.IDX10251, LogHelper.MarkAsNonPII(notAfterUtc), LogHelper.MarkAsNonPII(utcNow));
            }
        }

        /// <summary>
        /// Validates if a token has been replayed.
        /// </summary>
        /// <param name="expirationTime">When does the security token expire.</param>
        /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
        /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
        /// <exception cref="ArgumentNullException">If 'securityToken' is null or whitespace.</exception>
        /// <exception cref="ArgumentNullException">If 'validationParameters' is null or whitespace.</exception>
        /// <exception cref="SecurityTokenNoExpirationException">If <see cref="TokenValidationParameters.TokenReplayCache"/> is not null and expirationTime.HasValue is false. When a TokenReplayCache is set, tokens require an expiration time.</exception>
        /// <exception cref="SecurityTokenReplayDetectedException">If the 'securityToken' is found in the cache.</exception>
        /// <exception cref="SecurityTokenReplayAddFailedException">If the 'securityToken' could not be added to the <see cref="TokenValidationParameters.TokenReplayCache"/>.</exception>
        public static void ValidateTokenReplay(DateTime? expirationTime, string securityToken, TokenValidationParameters validationParameters)
        {
            if (string.IsNullOrWhiteSpace(securityToken))
                throw LogHelper.LogArgumentNullException(nameof(securityToken));

            if (validationParameters == null)
                throw LogHelper.LogArgumentNullException(nameof(validationParameters));

            if (validationParameters.TokenReplayValidator != null)
            {
                if (!validationParameters.TokenReplayValidator(expirationTime, securityToken, validationParameters))
                    throw LogHelper.LogExceptionMessage(new SecurityTokenReplayDetectedException(
                        LogHelper.FormatInvariant(
                            LogMessages.IDX10228,
                            LogHelper.MarkAsUnsafeSecurityArtifact(securityToken, t => t.ToString()))));
                return;
            }

            if (!validationParameters.ValidateTokenReplay)
            {
                LogHelper.LogVerbose(LogMessages.IDX10246);
                return;
            }

            // check if token if replay cache is set, then there must be an expiration time.
            if (validationParameters.TokenReplayCache != null)
            {
                if (!expirationTime.HasValue)
                    throw LogHelper.LogExceptionMessage(new SecurityTokenNoExpirationException(LogHelper.FormatInvariant(LogMessages.IDX10227, securityToken)));

                if (validationParameters.TokenReplayCache.TryFind(securityToken))
                    throw LogHelper.LogExceptionMessage(new SecurityTokenReplayDetectedException(LogHelper.FormatInvariant(LogMessages.IDX10228, securityToken)));

                if (!validationParameters.TokenReplayCache.TryAdd(securityToken, expirationTime.Value))
                    throw LogHelper.LogExceptionMessage(new SecurityTokenReplayAddFailedException(LogHelper.FormatInvariant(LogMessages.IDX10229, securityToken)));
            }

            // if it reaches here, that means no token replay is detected.
            LogHelper.LogInformation(LogMessages.IDX10240);
        }

        /// <summary>
        /// Validates if a token has been replayed.
        /// </summary>
        /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
        /// <param name="expirationTime">When does the security token expire.</param>
        /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
        /// <exception cref="ArgumentNullException">If 'securityToken' is null or whitespace.</exception>
        /// <exception cref="ArgumentNullException">If 'validationParameters' is null or whitespace.</exception>
        /// <exception cref="SecurityTokenNoExpirationException">If <see cref="TokenValidationParameters.TokenReplayCache"/> is not null and expirationTime.HasValue is false. When a TokenReplayCache is set, tokens require an expiration time.</exception>
        /// <exception cref="SecurityTokenReplayDetectedException">If the 'securityToken' is found in the cache.</exception>
        /// <exception cref="SecurityTokenReplayAddFailedException">If the 'securityToken' could not be added to the <see cref="TokenValidationParameters.TokenReplayCache"/>.</exception>
        public static void ValidateTokenReplay(string securityToken, DateTime? expirationTime, TokenValidationParameters validationParameters)
        {
            ValidateTokenReplay(expirationTime, securityToken, validationParameters);
        }

        /// <summary>
        /// Validates the type of the token.
        /// </summary>
        /// <param name="type">The token type or <c>null</c> if it couldn't be resolved (e.g from the 'typ' header for a JWT).</param>
        /// <param name="securityToken">The <see cref="SecurityToken"/> that is being validated.</param>
        /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="validationParameters"/> is null.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="securityToken"/> is null.</exception>
        /// <exception cref="SecurityTokenInvalidTypeException">If <paramref name="type"/> is null or whitespace and <see cref="TokenValidationParameters.ValidTypes"/> is not null.</exception>
        /// <exception cref="SecurityTokenInvalidTypeException">If <paramref name="type"/> failed to match <see cref="TokenValidationParameters.ValidTypes"/>.</exception>
        /// <remarks>An EXACT match is required. <see cref="StringComparison.Ordinal"/> (case sensitive) is used for comparing <paramref name="type"/> against <see cref="TokenValidationParameters.ValidTypes"/>.</remarks>
        /// <returns>The actual token type, that may be the same as <paramref name="type"/> or a different value if the token type was resolved from a different location.</returns>
        public static string ValidateTokenType(string type, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (securityToken == null)
                throw new ArgumentNullException(nameof(securityToken));

            if (validationParameters == null)
                throw LogHelper.LogArgumentNullException(nameof(validationParameters));

            if (validationParameters.TypeValidator == null && (validationParameters.ValidTypes == null || !validationParameters.ValidTypes.Any()))
            {
                LogHelper.LogVerbose(LogMessages.IDX10255);
                return type;
            }

            if (validationParameters.TypeValidator != null)
                return validationParameters.TypeValidator(type, securityToken, validationParameters);

            // Note: don't throw an exception for a null or empty token type when a user-defined delegate is set
            // to allow it to extract the actual token type from a different location (e.g from the claims).
            if (string.IsNullOrEmpty(type))
                throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidTypeException(LogMessages.IDX10256) { InvalidType = null });

            if (!validationParameters.ValidTypes.Contains(type, StringComparer.Ordinal))
            {
                throw LogHelper.LogExceptionMessage(
                    new SecurityTokenInvalidTypeException(LogHelper.FormatInvariant(LogMessages.IDX10257, LogHelper.MarkAsNonPII(type), Utility.SerializeAsSingleCommaDelimitedString(validationParameters.ValidTypes)))
                    { InvalidType = type });
            }

            // if it reaches here, token type was succcessfully validated.
            if (LogHelper.IsEnabled(EventLogLevel.Informational))
                LogHelper.LogInformation(LogMessages.IDX10258, LogHelper.MarkAsNonPII(type));

            return type;
        }
    }
}
