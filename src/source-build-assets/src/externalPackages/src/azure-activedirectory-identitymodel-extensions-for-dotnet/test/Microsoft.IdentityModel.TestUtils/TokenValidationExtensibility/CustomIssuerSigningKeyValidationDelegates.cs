// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;

#nullable enable
namespace Microsoft.IdentityModel.TestUtils
{
    internal class CustomIssuerSigningKeyValidationDelegates
    {
        internal static ValidationResult<ValidatedSignatureKey, ValidationError> CustomValidationFailed(
            SecurityKey signingKey,
            SecurityToken securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            return new CustomIssuerSigningKeyValidationError(
                new MessageDetail(nameof(CustomValidationFailed)),
                CustomValidationFailure.IssuerSigningKeyValidationFailed,
                Default.GetStackFrame(),
                signingKey,
                null);
        }

        internal static ValidationResult<ValidatedSignatureKey, ValidationError> IssuerSigningKeyValidationFailed(
            SecurityKey signingKey,
            SecurityToken securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            return new CustomIssuerSigningKeyValidationError(
                new MessageDetail(nameof(IssuerSigningKeyValidationFailed)),
                SignatureKeyValidationFailure.ValidationFailed,
                Default.GetStackFrame(),
                signingKey,
                null);
        }

        internal static ValidationResult<ValidatedSignatureKey, ValidationError> UnknownValidationFailure(
            SecurityKey signingKey,
            SecurityToken securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            return new CustomIssuerSigningKeyValidationError(
                new MessageDetail(nameof(UnknownValidationFailure)),
                AlgorithmValidationFailure.AlgorithmIsNotSupported,
                Default.GetStackFrame(),
                signingKey);
        }

        internal static ValidationResult<ValidatedSignatureKey, ValidationError> IssuerSigningKeyDelegate(
            SecurityKey signingKey,
            SecurityToken securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            return new CustomIssuerSigningKeyValidationError(
                new MessageDetail(nameof(IssuerSigningKeyDelegate)),
                SignatureKeyValidationFailure.ValidationFailed,
                Default.GetStackFrame(),
                signingKey,
                null);
        }

        internal static ValidationResult<ValidatedSignatureKey, ValidationError> IssuerSigningKeyValidatorThrows(
            SecurityKey signingKey,
            SecurityToken securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            throw new CustomSecurityTokenInvalidSigningKeyException(
                nameof(IssuerSigningKeyValidatorThrows),
                new SignatureKeyValidationError(
                    new MessageDetail(nameof(IssuerSigningKeyValidatorThrows)),
                    SignatureKeyValidationFailure.ValidationFailed,
                    Default.GetStackFrame(),
                    signingKey,
                    null),
                null);
        }
    }
}
#nullable restore
