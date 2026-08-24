// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;

#nullable enable
namespace Microsoft.IdentityModel.TestUtils
{
    internal class CustomTokenTypeValidationDelegates
    {
        internal static ValidationResult<ValidatedTokenType, ValidationError> CustomTokenTypeValidationFailed(
            string? type,
            SecurityToken? securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            return new CustomTokenTypeValidationError(
                new MessageDetail(nameof(CustomTokenTypeValidationFailed)),
                CustomValidationFailure.TokenTypeValidationFailed,
                Default.GetStackFrame(),
                type,
                null);
        }

        internal static ValidationResult<ValidatedTokenType, ValidationError> TokenTypeValidationFailed(
            string? tokenType,
            SecurityToken? securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            return new CustomTokenTypeValidationError(
                new MessageDetail(nameof(TokenTypeValidationFailed)),
                TokenTypeValidationFailure.ValidationFailed,
                Default.GetStackFrame(),
                tokenType,
                null);
        }

        internal static ValidationResult<ValidatedTokenType, ValidationError> UnknownValidationFailure(
            string? tokenType,
            SecurityToken? securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            return new CustomTokenTypeValidationError(
                new MessageDetail(nameof(UnknownValidationFailure)),
                AlgorithmValidationFailure.AlgorithmIsNotSupported,
                Default.GetStackFrame(),
                tokenType);
        }

        internal static ValidationResult<ValidatedTokenType, ValidationError> TokenTypeValidatorDelegate(
            string? tokenType,
            SecurityToken? securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            return new TokenTypeValidationError(
                new MessageDetail(nameof(TokenTypeValidatorDelegate)),
                TokenTypeValidationFailure.ValidationFailed,
                Default.GetStackFrame(),
                tokenType,
                null);
        }

        internal static ValidationResult<ValidatedTokenType, ValidationError> TokenTypeValidatorThrows(
            string? type,
            SecurityToken? securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            throw new CustomSecurityTokenInvalidTypeException(
                nameof(TokenTypeValidatorThrows),
                new TokenTypeValidationError(
                    new MessageDetail(nameof(TokenTypeValidatorThrows)),
                    TokenTypeValidationFailure.ValidationFailed,
                    Default.GetStackFrame(),
                    type,
                    null),
                null);
        }
    }
}
#nullable restore
