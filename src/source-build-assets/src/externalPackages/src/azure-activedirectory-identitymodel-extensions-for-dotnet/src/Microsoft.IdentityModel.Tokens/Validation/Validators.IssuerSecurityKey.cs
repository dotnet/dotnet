// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.IdentityModel.Logging;

#nullable enable
namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Definition for delegate that will validate the <see cref="SecurityKey"/> that signed a <see cref="SecurityToken"/>.
    /// </summary>
    /// <param name="signingKey">The security key to validate.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> that is being validated.</param>
    /// <param name="validationParameters">The <see cref="TokenValidationParameters"/> to be used for validating the token.</param>
    /// <param name="callContext"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A <see cref="SigningKeyValidationResult"/>that contains the results of validating the issuer.</returns>
    /// <remarks>This delegate is not expected to throw.</remarks>
    internal delegate Task<SigningKeyValidationResult> IssuerSecurityKeyValidationDelegate(
        SecurityKey signingKey,
        SecurityToken securityToken,
        TokenValidationParameters validationParameters,
        CallContext callContext,
        CancellationToken cancellationToken);

    /// <summary>
    /// SigningKeyValidation
    /// </summary>

    public static partial class Validators
    {
        /// <summary>
        /// Validates the <see cref="SecurityKey"/> that signed a <see cref="SecurityToken"/>.
        /// </summary>
        /// <param name="securityKey">The <see cref="SecurityKey"/> that signed the <see cref="SecurityToken"/>.</param>
        /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
        /// <param name="validationParameters">The <see cref="TokenValidationParameters"/> to be used for validating the token.</param>
        /// <param name="callContext"></param>
        /// <exception cref="ArgumentNullException"> if 'securityKey' is null and ValidateIssuerSigningKey is true.</exception>
        /// <exception cref="ArgumentNullException"> if 'securityToken' is null and ValidateIssuerSigningKey is true.</exception>
        /// <exception cref="ArgumentNullException"> if 'validationParameters' is null.</exception>
        internal static SigningKeyValidationResult ValidateIssuerSecurityKey(SecurityKey securityKey, SecurityToken securityToken, TokenValidationParameters validationParameters, CallContext callContext)
        {
            return ValidateIssuerSecurityKey(securityKey, securityToken, validationParameters, null, callContext);
        }

        /// <summary>
        /// Validates the <see cref="SecurityKey"/> that signed a <see cref="SecurityToken"/>.
        /// </summary>
        /// <param name="securityKey">The <see cref="SecurityKey"/> that signed the <see cref="SecurityToken"/>.</param>
        /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
        /// <param name="validationParameters">The <see cref="TokenValidationParameters"/> to be used for validating the token.</param>
        /// <param name="configuration">The <see cref="BaseConfiguration"/> required for issuer and signing key validation.</param>
        /// <param name="callContext"></param>
        /// <exception cref="ArgumentNullException"> if 'securityKey' is null and ValidateIssuerSigningKey is true.</exception>
        /// <exception cref="ArgumentNullException"> if 'securityToken' is null and ValidateIssuerSigningKey is true.</exception>
        /// <exception cref="ArgumentNullException"> if 'validationParameters' is null.</exception>
        internal static SigningKeyValidationResult ValidateIssuerSecurityKey(SecurityKey securityKey, SecurityToken securityToken, TokenValidationParameters validationParameters, BaseConfiguration? configuration, CallContext callContext)
        {
            if (validationParameters == null)
                return new SigningKeyValidationResult(
                    securityKey,
                    ValidationFailureType.NullArgument,
                    new ExceptionDetail(
                        new MessageDetail(
                            LogMessages.IDX10000,
                            LogHelper.MarkAsNonPII(nameof(validationParameters))),
                        typeof(ArgumentNullException),
                        new StackFrame(true)));

            if (validationParameters.IssuerSigningKeyValidatorUsingConfiguration != null)
            {
                return ValidateSigningKeyUsingDelegateAndConfiguration(securityKey, securityToken, validationParameters, configuration);
            }

            if (validationParameters.IssuerSigningKeyValidator != null)
            {
                return ValidateSigningKeyUsingDelegateAndConfiguration(securityKey, securityToken, validationParameters, null);
            }

            if (!validationParameters.ValidateIssuerSigningKey)
            {
                LogHelper.LogVerbose(LogMessages.IDX10237);
                return new SigningKeyValidationResult(securityKey);
            }

            if (!validationParameters.RequireSignedTokens && securityKey == null)
            {
                LogHelper.LogInformation(LogMessages.IDX10252);
                return new SigningKeyValidationResult(securityKey);
            }
            else if (securityKey == null)
            {
                return new SigningKeyValidationResult(
                    securityKey,
                    ValidationFailureType.NullArgument,
                    new ExceptionDetail(
                        new MessageDetail(
                            LogMessages.IDX10253,
                            LogHelper.MarkAsNonPII(nameof(securityKey))),
                        typeof(ArgumentNullException),
                        new StackFrame(true)));
            }

            if (securityToken == null)
                return new SigningKeyValidationResult(
                    securityKey,
                    ValidationFailureType.NullArgument,
                    new ExceptionDetail(
                        new MessageDetail(
                            LogMessages.IDX10000,
                            LogHelper.MarkAsNonPII(nameof(securityToken))),
                        typeof(ArgumentNullException),
                        new StackFrame(true)));

            return ValidateIssuerSigningKeyLifeTime(securityKey, validationParameters, callContext);
        }

        /// <summary>
        /// Given a signing key, when it's derived from a certificate, validates that the certificate is already active and non-expired
        /// </summary>
        /// <param name="securityKey">The <see cref="SecurityKey"/> that signed the <see cref="SecurityToken"/>.</param>
        /// <param name="validationParameters">The <see cref="TokenValidationParameters"/> to be used for validating the token.</param>
        /// <param name="callContext"></param>
#pragma warning disable CA1801 // Review unused parameters
        internal static SigningKeyValidationResult ValidateIssuerSigningKeyLifeTime(SecurityKey securityKey, TokenValidationParameters validationParameters, CallContext callContext)
#pragma warning restore CA1801 // Review unused parameters
        {
            X509SecurityKey? x509SecurityKey = securityKey as X509SecurityKey;
            if (x509SecurityKey?.Certificate is X509Certificate2 cert)
            {
                DateTime utcNow = DateTime.UtcNow;
                var notBeforeUtc = cert.NotBefore.ToUniversalTime();
                var notAfterUtc = cert.NotAfter.ToUniversalTime();

                if (notBeforeUtc > DateTimeUtil.Add(utcNow, validationParameters.ClockSkew))
                    return new SigningKeyValidationResult(
                        securityKey,
                        ValidationFailureType.SigningKeyValidationFailed,
                        new ExceptionDetail(
                            new MessageDetail(
                                LogHelper.FormatInvariant(
                                    LogMessages.IDX10248,
                                    LogHelper.MarkAsNonPII(notBeforeUtc),
                                    LogHelper.MarkAsNonPII(utcNow))),
                            typeof(SecurityTokenInvalidSigningKeyException),
                            new StackFrame(true)));

                if (LogHelper.IsEnabled(EventLogLevel.Informational))
                    LogHelper.LogInformation(LogMessages.IDX10250, LogHelper.MarkAsNonPII(notBeforeUtc), LogHelper.MarkAsNonPII(utcNow));

                if (notAfterUtc < DateTimeUtil.Add(utcNow, validationParameters.ClockSkew.Negate()))
                    return new SigningKeyValidationResult(
                        securityKey,
                        ValidationFailureType.SigningKeyValidationFailed,
                        new ExceptionDetail(
                            new MessageDetail(
                                LogHelper.FormatInvariant(
                                    LogMessages.IDX10249,
                                    LogHelper.MarkAsNonPII(notAfterUtc),
                                    LogHelper.MarkAsNonPII(utcNow))),
                            typeof(SecurityTokenInvalidSigningKeyException),
                            new StackFrame(true)));

                if (LogHelper.IsEnabled(EventLogLevel.Informational))
                    LogHelper.LogInformation(LogMessages.IDX10251, LogHelper.MarkAsNonPII(notAfterUtc), LogHelper.MarkAsNonPII(utcNow));
            }

            return new SigningKeyValidationResult(securityKey);
        }

        private static SigningKeyValidationResult ValidateSigningKeyUsingDelegateAndConfiguration(SecurityKey securityKey, SecurityToken securityToken, TokenValidationParameters validationParameters, BaseConfiguration? configuration)
        {
            try
            {
                bool success;
                if (configuration != null)
                    success = validationParameters.IssuerSigningKeyValidatorUsingConfiguration(securityKey, securityToken, validationParameters, configuration);
                else
                    success = validationParameters.IssuerSigningKeyValidator(securityKey, securityToken, validationParameters);

                if (!success)
                    return new SigningKeyValidationResult(
                        securityKey,
                        ValidationFailureType.SigningKeyValidationFailed,
                        new ExceptionDetail(
                            new MessageDetail(
                                LogMessages.IDX10232,
                                securityKey),
                            typeof(SecurityTokenInvalidSigningKeyException),
                            new StackFrame(true)));

                return new SigningKeyValidationResult(securityKey);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception exception)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                return new SigningKeyValidationResult(
                    securityKey,
                    ValidationFailureType.SigningKeyValidationFailed,
                    new ExceptionDetail(
                        new MessageDetail(
                            LogMessages.IDX10232,
                            securityKey),
                        exception.GetType(),
                        new StackFrame(true),
                        exception));
            }
        }
    }
}
#nullable restore
