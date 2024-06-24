// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Definition for delegate that will validate the issuer value in a token.
    /// </summary>
    /// <param name="issuer">The issuer to validate.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> that is being validated.</param>
    /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
    /// <param name="callContext"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A <see cref="IssuerValidationResult"/>that contains the results of validating the issuer.</returns>
    /// <remarks>This delegate is not expected to throw.</remarks>
    internal delegate Task<IssuerValidationResult> IssuerValidationDelegateAsync(
        string issuer,
        SecurityToken securityToken,
        TokenValidationParameters validationParameters,
        CallContext callContext,
        CancellationToken cancellationToken);

    /// <summary>
    /// IssuerValidation
    /// </summary>
    public static partial class Validators
    {
        /// <summary>
        /// Determines if an issuer found in a <see cref="SecurityToken"/> is valid.
        /// </summary>
        /// <param name="issuer">The issuer to validate</param>
        /// <param name="securityToken">The <see cref="SecurityToken"/> that is being validated.</param>
        /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
        /// <returns>The issuer to use when creating the "Claim"(s) in a "ClaimsIdentity".</returns>
        /// <exception cref="ArgumentNullException">If 'validationParameters' is null.</exception>
        /// <exception cref="ArgumentNullException">If 'issuer' is null or whitespace and <see cref="TokenValidationParameters.ValidateIssuer"/> is true.</exception>
        /// <exception cref="SecurityTokenInvalidIssuerException">If <see cref="TokenValidationParameters.ValidIssuer"/> is null or whitespace and <see cref="TokenValidationParameters.ValidIssuers"/> is null.</exception>
        /// <exception cref="SecurityTokenInvalidIssuerException">If 'issuer' failed to matched either <see cref="TokenValidationParameters.ValidIssuer"/> or one of <see cref="TokenValidationParameters.ValidIssuers"/>.</exception>
        /// <remarks>An EXACT match is required.</remarks>
        public static string ValidateIssuer(string issuer, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            return ValidateIssuer(issuer, securityToken, validationParameters, null);
        }

        /// <summary>
        /// Determines if an issuer found in a <see cref="SecurityToken"/> is valid.
        /// </summary>
        /// <param name="issuer">The issuer to validate</param>
        /// <param name="securityToken">The <see cref="SecurityToken"/> that is being validated.</param>
        /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
        /// <param name="configuration">The <see cref="BaseConfiguration"/> required for issuer and signing key validation.</param>
        /// <returns>The issuer to use when creating the "Claim"(s) in a "ClaimsIdentity".</returns>
        /// <exception cref="ArgumentNullException">If 'validationParameters' is null.</exception>
        /// <exception cref="ArgumentNullException">If 'issuer' is null or whitespace and <see cref="TokenValidationParameters.ValidateIssuer"/> is true.</exception>
        /// <exception cref="ArgumentNullException">If ' configuration' is null.</exception>
        /// <exception cref="SecurityTokenInvalidIssuerException">If <see cref="TokenValidationParameters.ValidIssuer"/> is null or whitespace and <see cref="TokenValidationParameters.ValidIssuers"/> is null and <see cref="BaseConfiguration.Issuer"/> is null.</exception>
        /// <exception cref="SecurityTokenInvalidIssuerException">If 'issuer' failed to matched either <see cref="TokenValidationParameters.ValidIssuer"/> or one of <see cref="TokenValidationParameters.ValidIssuers"/> or <see cref="BaseConfiguration.Issuer"/>.</exception>
        /// <remarks>An EXACT match is required.</remarks>
        internal static string ValidateIssuer(string issuer, SecurityToken securityToken, TokenValidationParameters validationParameters, BaseConfiguration configuration)
        {
            ValueTask<string> vt = ValidateIssuerAsync(issuer, securityToken, validationParameters, configuration);
            return vt.IsCompletedSuccessfully ?
                vt.Result :
                vt.AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Determines if an issuer found in a <see cref="SecurityToken"/> is valid.
        /// </summary>
        /// <param name="issuer">The issuer to validate</param>
        /// <param name="securityToken">The <see cref="SecurityToken"/> that is being validated.</param>
        /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
        /// <param name="configuration">The <see cref="BaseConfiguration"/> required for issuer and signing key validation.</param>
        /// <returns>The issuer to use when creating the "Claim"(s) in a "ClaimsIdentity".</returns>
        /// <exception cref="ArgumentNullException">If 'validationParameters' is null.</exception>
        /// <exception cref="ArgumentNullException">If 'issuer' is null or whitespace and <see cref="TokenValidationParameters.ValidateIssuer"/> is true.</exception>
        /// <exception cref="ArgumentNullException">If ' configuration' is null.</exception>
        /// <exception cref="SecurityTokenInvalidIssuerException">If <see cref="TokenValidationParameters.ValidIssuer"/> is null or whitespace and <see cref="TokenValidationParameters.ValidIssuers"/> is null and <see cref="BaseConfiguration.Issuer"/> is null.</exception>
        /// <exception cref="SecurityTokenInvalidIssuerException">If 'issuer' failed to matched either <see cref="TokenValidationParameters.ValidIssuer"/> or one of <see cref="TokenValidationParameters.ValidIssuers"/> or <see cref="BaseConfiguration.Issuer"/>.</exception>
        /// <remarks>An EXACT match is required.</remarks>
        internal static async ValueTask<string> ValidateIssuerAsync(
            string issuer,
            SecurityToken securityToken,
            TokenValidationParameters validationParameters,
            BaseConfiguration configuration)
        {
            if (validationParameters == null)
                throw LogHelper.LogArgumentNullException(nameof(validationParameters));

            if (validationParameters.IssuerValidatorAsync != null)
                return await validationParameters.IssuerValidatorAsync(issuer, securityToken, validationParameters).ConfigureAwait(false);

            if (validationParameters.IssuerValidatorUsingConfiguration != null)
                return validationParameters.IssuerValidatorUsingConfiguration(issuer, securityToken, validationParameters, configuration);

            if (validationParameters.IssuerValidator != null)
                return validationParameters.IssuerValidator(issuer, securityToken, validationParameters);

            if (!validationParameters.ValidateIssuer)
            {
                LogHelper.LogWarning(LogMessages.IDX10235);
                return issuer;
            }

            if (string.IsNullOrWhiteSpace(issuer))
                throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidIssuerException(LogMessages.IDX10211)
                { InvalidIssuer = issuer });

            // Throw if all possible places to validate against are null or empty
            if (string.IsNullOrWhiteSpace(validationParameters.ValidIssuer)
                && validationParameters.ValidIssuers.IsNullOrEmpty()
                && string.IsNullOrWhiteSpace(configuration?.Issuer))
                throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidIssuerException(LogMessages.IDX10204)
                { InvalidIssuer = issuer });

            if (configuration != null)
            {
                if (string.Equals(configuration.Issuer, issuer))
                {
                    if (LogHelper.IsEnabled(EventLogLevel.Informational))
                        LogHelper.LogInformation(LogMessages.IDX10236, LogHelper.MarkAsNonPII(issuer));

                    return issuer;
                }
            }

            if (string.Equals(validationParameters.ValidIssuer, issuer))
            {
                if (LogHelper.IsEnabled(EventLogLevel.Informational))
                    LogHelper.LogInformation(LogMessages.IDX10236, LogHelper.MarkAsNonPII(issuer));

                return issuer;
            }

            if (validationParameters.ValidIssuers != null)
            {
                foreach (string str in validationParameters.ValidIssuers)
                {
                    if (string.IsNullOrEmpty(str))
                    {
                        LogHelper.LogInformation(LogMessages.IDX10262);
                        continue;
                    }

                    if (string.Equals(str, issuer))
                    {
                        if (LogHelper.IsEnabled(EventLogLevel.Informational))
                            LogHelper.LogInformation(LogMessages.IDX10236, LogHelper.MarkAsNonPII(issuer));

                        return issuer;
                    }
                }
            }

            SecurityTokenInvalidIssuerException ex = new SecurityTokenInvalidIssuerException(
                LogHelper.FormatInvariant(LogMessages.IDX10205,
                    LogHelper.MarkAsNonPII(issuer),
                    LogHelper.MarkAsNonPII(validationParameters.ValidIssuer ?? "null"),
                    LogHelper.MarkAsNonPII(Utility.SerializeAsSingleCommaDelimitedString(validationParameters.ValidIssuers)),
                    LogHelper.MarkAsNonPII(configuration?.Issuer)))
            { InvalidIssuer = issuer };

            if (!validationParameters.LogValidationExceptions)
                throw ex;

            throw LogHelper.LogExceptionMessage(ex);
        }

        /// <summary>
        /// Determines if an issuer found in a <see cref="SecurityToken"/> is valid.
        /// </summary>
        /// <param name="issuer">The issuer to validate</param>
        /// <param name="securityToken">The <see cref="SecurityToken"/> that is being validated.</param>
        /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
        /// <param name="callContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>The issuer to use when creating the "Claim"(s) in a "ClaimsIdentity".</returns>
        /// <exception cref="ArgumentNullException">If 'validationParameters' is null.</exception>
        /// <exception cref="ArgumentNullException">If 'issuer' is null or whitespace and <see cref="TokenValidationParameters.ValidateIssuer"/> is true.</exception>
        /// <exception cref="SecurityTokenInvalidIssuerException">If <see cref="TokenValidationParameters.ValidIssuer"/> is null or whitespace and <see cref="TokenValidationParameters.ValidIssuers"/> is null.</exception>
        /// <exception cref="SecurityTokenInvalidIssuerException">If 'issuer' failed to matched either <see cref="TokenValidationParameters.ValidIssuer"/> or one of <see cref="TokenValidationParameters.ValidIssuers"/>.</exception>
        /// <remarks>An EXACT match is required.</remarks>
        internal static async Task<IssuerValidationResult> ValidateIssuerAsync(
            string issuer,
            SecurityToken securityToken,
            TokenValidationParameters validationParameters,
            CallContext callContext,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(issuer))
            {
                return new IssuerValidationResult(
                    issuer,
                    ValidationFailureType.NullArgument,
                    new ExceptionDetail(
                        new MessageDetail(
                            LogMessages.IDX10211,
                            null),
                        typeof(SecurityTokenInvalidIssuerException),
                        new StackFrame(true),
                        null));
            }

            if (validationParameters == null)
                return new IssuerValidationResult(
                    issuer,
                    ValidationFailureType.NullArgument,
                    new ExceptionDetail(
                        new MessageDetail(
                            LogMessages.IDX10000,
                            LogHelper.MarkAsNonPII(nameof(validationParameters))),
                        typeof(ArgumentNullException),
                        new StackFrame(true),
                        null));

            if (securityToken == null)
                return new IssuerValidationResult(
                    issuer,
                    ValidationFailureType.NullArgument,
                    new ExceptionDetail(
                        new MessageDetail(
                            LogMessages.IDX10000,
                            LogHelper.MarkAsNonPII(nameof(securityToken))),
                        typeof(ArgumentNullException),
                        new StackFrame(true),
                        null));

            BaseConfiguration configuration = null;
            if (validationParameters.ConfigurationManager != null)
                configuration = await validationParameters.ConfigurationManager.GetBaseConfigurationAsync(cancellationToken).ConfigureAwait(false);

            // Throw if all possible places to validate against are null or empty
            if (string.IsNullOrWhiteSpace(validationParameters.ValidIssuer)
                && validationParameters.ValidIssuers.IsNullOrEmpty()
                && string.IsNullOrWhiteSpace(configuration?.Issuer))
            {
                return new IssuerValidationResult(
                    issuer,
                    ValidationFailureType.IssuerValidationFailed,
                        new ExceptionDetail(
                            new MessageDetail(
                                LogMessages.IDX10211,
                                null),
                            typeof(SecurityTokenInvalidIssuerException),
                            new StackFrame(true)));
            }

            if (configuration != null)
            {
                if (string.Equals(configuration.Issuer, issuer))
                {
                    // TODO - how and when to log
                    // Logs will have to be passed back to Wilson
                    // so that they can be written to the correct place and in the correct format respecting PII.
                    if (LogHelper.IsEnabled(EventLogLevel.Informational))
                        LogHelper.LogInformation(LogMessages.IDX10236, LogHelper.MarkAsNonPII(issuer), callContext);

                    return new IssuerValidationResult(issuer,
                        IssuerValidationResult.ValidationSource.IssuerIsConfigurationIssuer);
                }
            }

            if (string.Equals(validationParameters.ValidIssuer, issuer))
            {
                return new IssuerValidationResult(issuer,
                    IssuerValidationResult.ValidationSource.IssuerIsValidIssuer);
            }

            if (validationParameters.ValidIssuers != null)
            {
                foreach (string str in validationParameters.ValidIssuers)
                {
                    if (string.IsNullOrEmpty(str))
                    {
                        if (LogHelper.IsEnabled(EventLogLevel.Informational))
                            LogHelper.LogInformation(LogMessages.IDX10262);

                        continue;
                    }

                    if (string.Equals(str, issuer))
                    {
                        if (LogHelper.IsEnabled(EventLogLevel.Informational))
                            LogHelper.LogInformation(LogMessages.IDX10236, LogHelper.MarkAsNonPII(issuer));

                        return new IssuerValidationResult(issuer,
                            IssuerValidationResult.ValidationSource.IssuerIsAmongValidIssuers);
                    }
                }
            }

            return new IssuerValidationResult(
                issuer,
                ValidationFailureType.IssuerValidationFailed,
                new ExceptionDetail(
                    new MessageDetail(
                        LogMessages.IDX10205,
                        LogHelper.MarkAsNonPII(issuer),
                        LogHelper.MarkAsNonPII(validationParameters.ValidIssuer ?? "null"),
                        LogHelper.MarkAsNonPII(Utility.SerializeAsSingleCommaDelimitedString(validationParameters.ValidIssuers)),
                        LogHelper.MarkAsNonPII(configuration?.Issuer)),
                    typeof(SecurityTokenInvalidIssuerException),
                    new StackFrame(true)));
        }
    }
}
