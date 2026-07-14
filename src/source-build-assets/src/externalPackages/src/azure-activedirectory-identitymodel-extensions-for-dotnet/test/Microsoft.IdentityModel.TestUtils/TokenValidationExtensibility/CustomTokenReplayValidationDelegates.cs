// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;

#nullable enable
namespace Microsoft.IdentityModel.TestUtils
{
    internal class CustomTokenReplayValidationDelegates
    {
        internal static ValidationResult<DateTime?, ValidationError> CustomTokenReplayValidationFailed(
            DateTime? expirationTime,
            string securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            return new CustomTokenReplayValidationError(
                new MessageDetail(nameof(CustomTokenReplayValidationFailed)),
                CustomValidationFailure.TokenReplayValidationFailed,
                Default.GetStackFrame(),
                expirationTime);
        }

        internal static ValidationResult<DateTime?, ValidationError> TokenReplayValidationFailed(
            DateTime? expirationTime,
            string securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            return new CustomTokenReplayValidationError(
                new MessageDetail(nameof(TokenReplayValidationFailed)),
                TokenReplayValidationFailure.ValidationFailed,
                Default.GetStackFrame(),
                expirationTime);
        }

        internal static ValidationResult<DateTime?, ValidationError> UnknownValidationFailure(
            DateTime? expirationTime,
            string securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            return new CustomTokenReplayValidationError(
                new MessageDetail(nameof(UnknownValidationFailure)),
                AlgorithmValidationFailure.AlgorithmIsNotSupported,
                Default.GetStackFrame(),
                expirationTime);
        }

        internal static ValidationResult<DateTime?, ValidationError> TokenReplayValidationDelegate(
            DateTime? expirationTime,
            string securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            return new TokenReplayValidationError(
                new MessageDetail(nameof(TokenReplayValidationDelegate)),
                TokenReplayValidationFailure.ValidationFailed,
                Default.GetStackFrame(),
                expirationTime);
        }

        internal static ValidationResult<DateTime?, ValidationError> TokenReplayValidatorThrows(
            DateTime? expirationTime,
            string securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            throw new CustomSecurityTokenReplayDetectedException(
                nameof(TokenReplayValidatorThrows),
                new TokenReplayValidationError(
                    new MessageDetail(nameof(TokenReplayValidatorThrows)),
                    TokenReplayValidationFailure.ValidationFailed,
                    Default.GetStackFrame(),
                    expirationTime),
                null);
        }

        internal static ValidationResult<DateTime?, ValidationError> TokenReplayValidatorCustomTokenReplayDetectedExceptionTypeDelegate(
            DateTime? expirationTime,
            string securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            return new TokenReplayValidationError(
                new MessageDetail(nameof(TokenReplayValidatorCustomTokenReplayDetectedExceptionTypeDelegate)),
                TokenReplayValidationFailure.ValidationFailed,
                Default.GetStackFrame(),
                expirationTime);
        }
        internal static ValidationResult<DateTime?, ValidationError> TokenReplayValidatorCustomExceptionTypeDelegate(
            DateTime? expirationTime,
            string securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            return new TokenReplayValidationError(
                new MessageDetail(nameof(TokenReplayValidatorCustomExceptionTypeDelegate)),
                TokenReplayValidationFailure.ValidationFailed,
                Default.GetStackFrame(),
                expirationTime);
        }
    }
}
#nullable restore
