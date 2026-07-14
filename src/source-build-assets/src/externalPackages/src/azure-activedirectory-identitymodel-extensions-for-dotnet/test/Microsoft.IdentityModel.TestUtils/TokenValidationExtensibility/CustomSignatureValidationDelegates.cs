// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;

#nullable enable
namespace Microsoft.IdentityModel.TestUtils
{
    internal class CustomSignatureValidationDelegates
    {
        internal static ValidationResult<SecurityKey, ValidationError> CustomSignatureValidationFailed
            (
            SecurityToken? securityToken,
            ValidationParameters validationParameters,
            BaseConfiguration? configuration,
            CallContext callContext)
        {
            return new CustomSignatureValidationError(
                new MessageDetail(nameof(CustomSignatureValidationFailed)),
                CustomValidationFailure.SignatureValidationFailed,
                Default.GetStackFrame());
        }

        internal static ValidationResult<SecurityKey, ValidationError> CustomSignatureValidatorCustomExceptionDelegate(
            SecurityToken? securityToken,
            ValidationParameters validationParameters,
            BaseConfiguration? configuration,
            CallContext callContext)
        {
            return new CustomSignatureValidationError(
                new MessageDetail(nameof(CustomSignatureValidatorCustomExceptionDelegate)),
                SignatureValidationFailure.ValidationFailed,
                Default.GetStackFrame());
        }

        internal static ValidationResult<SecurityKey, ValidationError> UnknownValidationFailure(
            SecurityToken? securityToken,
            ValidationParameters validationParameters,
            BaseConfiguration? configuration,
            CallContext callContext)
        {
            return new CustomSignatureValidationError(
                new MessageDetail(nameof(UnknownValidationFailure)),
                AlgorithmValidationFailure.AlgorithmIsNotSupported,
                Default.GetStackFrame());
        }

        internal static ValidationResult<SecurityKey, ValidationError> SignatureValidationFailed(
            SecurityToken? securityToken,
            ValidationParameters validationParameters,
            BaseConfiguration? configuration,
            CallContext callContext)
        {
            return new CustomSignatureValidationError(
                new MessageDetail(nameof(SignatureValidationFailed)),
                SignatureValidationFailure.ValidationFailed,
                Default.GetStackFrame());
        }

        internal static ValidationResult<SecurityKey, ValidationError> SignatureValidatorDelegate(
            SecurityToken? securityToken,
            ValidationParameters validationParameters,
            BaseConfiguration? configuration,
            CallContext callContext)
        {
            return new SignatureValidationError(
                new MessageDetail(nameof(SignatureValidatorDelegate)),
                SignatureValidationFailure.ValidationFailed,
                Default.GetStackFrame());
        }

        internal static ValidationResult<SecurityKey, ValidationError> SignatureValidatorThrows(
            SecurityToken? securityToken,
            ValidationParameters validationParameters,
            BaseConfiguration? configuration,
            CallContext callContext)
        {
            throw new CustomSecurityTokenInvalidSignatureException(
                nameof(SignatureValidatorThrows),
                new SignatureValidationError(
                    new MessageDetail(nameof(SignatureValidatorDelegate)),
                    SignatureValidationFailure.ValidationFailed,
                    Default.GetStackFrame()),
                null);
        }
    }
}
#nullable restore
