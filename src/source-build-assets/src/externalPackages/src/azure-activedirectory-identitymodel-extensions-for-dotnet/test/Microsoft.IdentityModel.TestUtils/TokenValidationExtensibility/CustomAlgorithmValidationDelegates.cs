// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;

#nullable enable
namespace Microsoft.IdentityModel.TestUtils
{
    internal class CustomAlgorithmValidationDelegates
    {
        internal static ValidationResult<string, ValidationError> CustomAlgorithmValidationFailed(
            string? algorithm,
            SecurityToken securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            // Returns a CustomAlgorithmValidationError : AlgorithmValidationError
            return new CustomAlgorithmValidationError(
                new MessageDetail(nameof(CustomAlgorithmValidationFailed)),
                CustomValidationFailure.AlgorithmValidationFailed,
                Default.GetStackFrame(),
                algorithm);
        }

        internal static ValidationResult<string, ValidationError> UnknownValidationFailure(
            string? algorithm,
            SecurityToken securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            return new CustomAlgorithmValidationError(
                new MessageDetail(nameof(UnknownValidationFailure)),
                AudienceValidationFailure.AudienceDidNotMatch,
                Default.GetStackFrame(),
                algorithm);
        }

        internal static ValidationResult<string, ValidationError> AlgorithmValidationFailed(
            string? algorithm,
            SecurityToken securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            return new CustomAlgorithmValidationError(
                new MessageDetail(nameof(AlgorithmValidationFailed)),
                AlgorithmValidationFailure.ValidationFailed,
                Default.GetStackFrame(),
                algorithm);
        }

        internal static ValidationResult<string, ValidationError> CustomWithoutGetExceptionOverride(
            string? algorithm,
            SecurityToken securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            return new CustomAlgorithmWithoutGetExceptionValidationOverrideError(
                new MessageDetail(nameof(CustomWithoutGetExceptionOverride), null),
                AlgorithmValidationFailure.ValidationFailed,
                Default.GetStackFrame(),
                algorithm);
        }

        internal static ValidationResult<string, ValidationError> AlgorithmValidatorDelegate(
            string? algorithm,
            SecurityToken securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            return new AlgorithmValidationError(
                new MessageDetail(nameof(AlgorithmValidatorDelegate)),
                AlgorithmValidationFailure.ValidationFailed,
                Default.GetStackFrame(),
                algorithm);
        }

        internal static ValidationResult<string, ValidationError> AlgorithmValidatorThrows(
            string? algorithm,
            SecurityToken securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            throw new CustomSecurityTokenInvalidAlgorithmException(
                nameof(AlgorithmValidatorThrows),
                new AlgorithmValidationError(
                    new MessageDetail(nameof(AlgorithmValidatorThrows), null),
                    AlgorithmValidationFailure.ValidationFailed,
                    Default.GetStackFrame(),
                    algorithm,
                    null),
                null);
        }

        internal static ValidationResult<string, ValidationError> AlgorithmValidatorCustomAlgorithmExceptionTypeDelegate(
            string? algorithm,
            SecurityToken securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            return new AlgorithmValidationError(
                new MessageDetail(nameof(AlgorithmValidatorCustomAlgorithmExceptionTypeDelegate)),
                AlgorithmValidationFailure.ValidationFailed,
                Default.GetStackFrame(),
                algorithm);
        }

        internal static ValidationResult<string, ValidationError> AlgorithmValidatorNotSupportedFailureDelegate(
            string? algorithm,
            SecurityToken securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            return new AlgorithmValidationError(
                new MessageDetail(nameof(AlgorithmValidatorNotSupportedFailureDelegate)),
                AlgorithmValidationFailure.AlgorithmIsNotSupported,
                Default.GetStackFrame(),
                algorithm);
        }
    }
}
#nullable restore
