// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens.Experimental;

#nullable enable
namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Partial class for Issuer Validation.
    /// </summary>
    public static partial class Validators
    {
        /// <summary>
        /// Determines if an issuer found in a <see cref="SecurityToken"/> is valid.
        /// </summary>
        /// <param name="issuer">The issuer to validate</param>
        /// <param name="securityToken">The <see cref="SecurityToken"/> that is being validated.</param>
        /// <param name="validationParameters">The <see cref="ValidationParameters"/> to be used for validating the token.</param>
        /// <param name="callContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>A <see cref="ValidationResult{ValidatedIssuer, IssuerValidationError}"/> that contains either the issuer that was validated or an error.</returns>
        /// <remarks>An EXACT match is required.</remarks>
        internal static async Task<ValidationResult<ValidatedIssuer, ValidationError>> ValidateIssuerInternalAsync(
            string issuer,
            SecurityToken securityToken,
            ValidationParameters validationParameters,
            CallContext callContext,
            CancellationToken cancellationToken)
        {
            if (validationParameters == null)
                return new IssuerValidationError(
                    MessageDetail.NullParameter(nameof(validationParameters)),
                    ValidationFailureType.NullArgument,
                    ValidationError.GetCurrentStackFrame(),
                    issuer,
                    null);

            try
            {
                ValidationResult<ValidatedIssuer, ValidationError> result =
                    await validationParameters.IssuerValidatorAsync(
                        issuer,
                        securityToken,
                        validationParameters,
                        callContext,
                        cancellationToken).ConfigureAwait(false);

                if (!result.Succeeded)
                    return result.Error!.AddCurrentStackFrame();

                return result;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                return new IssuerValidationError(
                    new MessageDetail(LogMessages.IDX10269),
                    IssuerValidationFailure.ValidatorThrew,
                    ValidationError.GetCurrentStackFrame(),
                    issuer,
                    ex);
            }
        }

        /// <summary>
        /// Determines if an issuer found in a <see cref="SecurityToken"/> is valid.
        /// </summary>
        /// <param name="issuer">The issuer to found in the <see cref="SecurityToken"/>.</param>
        /// <param name="securityToken">The <see cref="SecurityToken"/> that is being validated.</param>
        /// <param name="validationParameters">The <see cref="ValidationParameters"/> to be used for validating the token.</param>
        /// <param name="callContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>A <see cref="ValidationResult{ValidatedIssuer, IssuerValidationError}"/> that contains either the issuer that was validated or an error.</returns>
        /// <remarks>An EXACT match is required.</remarks>
        public static async Task<ValidationResult<ValidatedIssuer, ValidationError>> ValidateIssuerAsync(
            string? issuer,
#pragma warning disable CA1801 // Review unused parameters
            SecurityToken? securityToken,
            ValidationParameters validationParameters,
            CallContext? callContext,
#pragma warning restore CA1801 // Review unused parameters
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(issuer))
            {
                return new IssuerValidationError(
                    new MessageDetail(LogMessages.IDX10211),
                    IssuerValidationFailure.NoIssuerInToken,
                    ValidationError.GetCurrentStackFrame(),
                    issuer,
                    null);
            }

            if (validationParameters == null)
                return new IssuerValidationError(
                    MessageDetail.NullParameter(nameof(validationParameters)),
                    ValidationFailureType.NullArgument,
                    ValidationError.GetCurrentStackFrame(),
                    issuer,
                    null);

            BaseConfiguration? configuration = null;
            if (validationParameters.ConfigurationManager != null)
                configuration = await validationParameters.ConfigurationManager.GetBaseConfigurationAsync(cancellationToken).ConfigureAwait(false);

            // Return failed IssuerValidationResult if all possible places to validate against are null or empty.
            if (validationParameters.ValidIssuers.Count == 0 && string.IsNullOrWhiteSpace(configuration?.Issuer))
                return new IssuerValidationError(
                    new MessageDetail(
                        LogMessages.IDX10212,
                        LogHelper.MarkAsNonPII(issuer),
                        "ValdIssuers is empty",
                        LogHelper.MarkAsNonPII(configuration?.Issuer)
                        ),
                    IssuerValidationFailure.NoValidationParameterIssuersProvided,
                    ValidationError.GetCurrentStackFrame(),
                    issuer);

            if (configuration != null)
            {
                if (string.Equals(configuration.Issuer, issuer))
                {
                    // TODO - how and when to log
                    // Logs will have to be passed back to Wilson
                    // so that they can be written to the correct place and in the correct format respecting PII.
                    // Add to CallContext
                    //if (LogHelper.IsEnabled(EventLogLevel.Informational))
                    //    LogHelper.LogInformation(LogMessages.IDX10236, LogHelper.MarkAsNonPII(issuer), callContext);
                    return new ValidatedIssuer(
                            issuer!,
                            IssuerValidationSource.IssuerMatchedConfiguration);
                }
            }

            if (validationParameters.ValidIssuers.Count != 0)
            {
                for (int i = 0; i < validationParameters.ValidIssuers.Count; i++)
                {
                    if (string.IsNullOrEmpty(validationParameters.ValidIssuers[i]))
                    {
                        // TODO: Add to CallContext
                        //if (LogHelper.IsEnabled(EventLogLevel.Informational))
                        //    LogHelper.LogInformation(LogMessages.IDX10262);

                        continue;
                    }

                    if (string.Equals(validationParameters.ValidIssuers[i], issuer))
                        return new ValidatedIssuer(
                            issuer!,
                            IssuerValidationSource.IssuerMatchedValidationParameters);
                }
            }

            return new IssuerValidationError(
                new MessageDetail(
                    LogMessages.IDX10212,
                    LogHelper.MarkAsNonPII(issuer),
                    LogHelper.MarkAsNonPII(Utility.SerializeAsSingleCommaDelimitedString(validationParameters.ValidIssuers)),
                    LogHelper.MarkAsNonPII(configuration?.Issuer)),
                IssuerValidationFailure.ValidationFailed,
                ValidationError.GetCurrentStackFrame(),
                issuer);
        }
    }
}
#nullable restore
