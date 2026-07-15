// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Linq;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens.Experimental;

#nullable enable
namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Partial class for Token Type Validation.
    /// </summary>
    public static partial class Validators
    {
        /// <summary>
        /// Validates the type of the token.
        /// </summary>
        /// <param name="type">The token type.</param>
        /// <param name="securityToken">The <see cref="SecurityToken"/> that is being validated.</param>
        /// <param name="validationParameters"><see cref="ValidationParameters"/> required for validation.</param>
        /// <param name="callContext">The <see cref="CallContext"/> that contains call information.</param>
        /// <returns> A <see cref="ValidationResult{TResult, TError}"/>that contains the results of validating the token type.</returns>
        /// <remarks>An EXACT match is required. <see cref="StringComparison.Ordinal"/> (case sensitive) is used for comparing <paramref name="type"/> against <see cref="ValidationParameters.ValidTypes"/>.</remarks>
        internal static ValidationResult<ValidatedTokenType, ValidationError> ValidateTokenTypeInternal(
            string? type,
            SecurityToken? securityToken,
            ValidationParameters validationParameters,
#pragma warning disable CA1801
            CallContext callContext)
#pragma warning restore CA1801
        {
            if (securityToken == null)
                return ValidationError.NullParameter(
                    nameof(securityToken),
                    ValidationError.GetCurrentStackFrame());

            if (validationParameters == null)
                return ValidationError.NullParameter(
                    nameof(validationParameters),
                    ValidationError.GetCurrentStackFrame());

            try
            {
                ValidationResult<ValidatedTokenType, ValidationError> result =
                    validationParameters.TokenTypeValidator(
                        type,
                        securityToken,
                        validationParameters,
                        callContext);

                if (!result.Succeeded)
                    return result.Error!.AddCurrentStackFrame();

                return result;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                return new TokenTypeValidationError(
                    new MessageDetail(LogMessages.IDX10275),
                    TokenTypeValidationFailure.ValidatorThrew,
                    ValidationError.GetCurrentStackFrame(),
                    type,
                    ex);
            }
        }

        /// <summary>
        /// Validates the type of the token.
        /// </summary>
        /// <param name="type">The token type.</param>
        /// <param name="securityToken">The <see cref="SecurityToken"/> that is being validated.</param>
        /// <param name="validationParameters"><see cref="ValidationParameters"/> required for validation.</param>
        /// <param name="callContext">The <see cref="CallContext"/> that contains call information.</param>
        /// <returns> A <see cref="ValidationResult{TResult, TError}"/>that contains the results of validating the token type.</returns>
        /// <remarks>An EXACT match is required. <see cref="StringComparison.Ordinal"/> (case sensitive) is used for comparing <paramref name="type"/> against <see cref="ValidationParameters.ValidTypes"/>.</remarks>
        public static ValidationResult<ValidatedTokenType, ValidationError> ValidateTokenType(
            string? type,
            SecurityToken? securityToken,
            ValidationParameters validationParameters,
#pragma warning disable CA1801
            CallContext callContext)
#pragma warning restore CA1801
        {
            if (securityToken == null)
                return ValidationError.NullParameter(
                    nameof(securityToken),
                    ValidationError.GetCurrentStackFrame());

            if (validationParameters == null)
                return ValidationError.NullParameter(
                    nameof(validationParameters),
                    ValidationError.GetCurrentStackFrame());

            if (validationParameters.ValidTypes.Count == 0)
                return new ValidatedTokenType(type ?? "null", validationParameters.ValidTypes.Count);

            if (string.IsNullOrEmpty(type))
                return new TokenTypeValidationError(
                    new MessageDetail(LogMessages.IDX10256),
                    TokenTypeValidationFailure.ValidationFailed,
                    ValidationError.GetCurrentStackFrame(),
                    null);

            if (!validationParameters.ValidTypes.Contains(type, StringComparer.Ordinal))
            {
                return new TokenTypeValidationError(
                    new MessageDetail(
                        LogMessages.IDX10257,
                        LogHelper.MarkAsNonPII(type),
                        LogHelper.MarkAsNonPII(Utility.SerializeAsSingleCommaDelimitedString(validationParameters.ValidTypes))),
                    TokenTypeValidationFailure.ValidationFailed,
                    ValidationError.GetCurrentStackFrame(),
                    type);
            }

            return new ValidatedTokenType(type!, validationParameters.ValidTypes.Count);
        }
    }
}
#nullable restore
