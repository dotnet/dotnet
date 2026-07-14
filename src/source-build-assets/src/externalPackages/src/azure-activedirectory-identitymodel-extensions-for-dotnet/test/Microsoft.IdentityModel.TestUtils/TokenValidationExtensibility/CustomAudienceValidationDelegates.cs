// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;

#nullable enable
namespace Microsoft.IdentityModel.TestUtils
{
    internal class CustomAudienceValidationDelegates
    {
        internal static ValidationResult<string, ValidationError> CustomValidationFailed(
            IList<string> tokenAudiences,
            SecurityToken? securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            return new CustomAudienceValidationError(
                new MessageDetail(nameof(CustomValidationFailed)),
                CustomValidationFailure.AudienceValidationFailed,
                Default.GetStackFrame(),
                tokenAudiences,
                null);
        }

        internal static ValidationResult<string, ValidationError> AudienceDidNotMatch(
            IList<string> tokenAudiences,
            SecurityToken? securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            return new CustomAudienceValidationError(
                new MessageDetail(nameof(AudienceDidNotMatch)),
                AudienceValidationFailure.AudienceDidNotMatch,
                Default.GetStackFrame(),
                tokenAudiences,
                null);
        }

        internal static ValidationResult<string, ValidationError> UnknownValidationFailure(
            IList<string> tokenAudiences,
            SecurityToken? securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            return new CustomAudienceValidationError(
                new MessageDetail(nameof(UnknownValidationFailure)),
                AlgorithmValidationFailure.AlgorithmIsNotSupported,
                Default.GetStackFrame(),
                tokenAudiences,
                null);
        }

        internal static ValidationResult<string, ValidationError> WithoutGetExceptionOverrideDelegate(
            IList<string> tokenAudiences,
            SecurityToken? securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            return new CustomAudienceWithoutGetExceptionValidationOverrideError(
                new MessageDetail(nameof(WithoutGetExceptionOverrideDelegate)),
                Default.GetStackFrame(),
                tokenAudiences,
                null)
                ;
        }

        internal static ValidationResult<string, ValidationError> AudienceValidatorDelegate(
            IList<string> tokenAudiences,
            SecurityToken? securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            return new AudienceValidationError(
                new MessageDetail(nameof(AudienceValidatorDelegate)),
                AudienceValidationFailure.AudienceDidNotMatch,
                Default.GetStackFrame(),
                tokenAudiences,
                null);
        }

        internal static ValidationResult<string, ValidationError> AudienceValidatorThrows(
            IList<string> tokenAudiences,
            SecurityToken? securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            throw new CustomSecurityTokenInvalidAudienceException(
                nameof(AudienceValidatorThrows),
                new AudienceValidationError(
                    new MessageDetail(nameof(AudienceValidatorDelegate)),
                    AudienceValidationFailure.AudienceDidNotMatch,
                    Default.GetStackFrame(),
                    tokenAudiences,
                    null),
                null);
        }
    }
}
#nullable restore
