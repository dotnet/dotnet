// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;

#nullable enable
namespace Microsoft.IdentityModel.TestUtils
{
    internal class CustomIssuerValidationDelegates
    {
        internal async static Task<ValidationResult<ValidatedIssuer, ValidationError>> CustomValidationFailed(
            string issuer,
            SecurityToken securityToken,
            ValidationParameters validationParameters,
            CallContext callContext,
            CancellationToken cancellationToken)
        {
            return await Task.FromResult(new ValidationResult<ValidatedIssuer, ValidationError>(
                new CustomIssuerValidationError(
                    new MessageDetail(nameof(CustomValidationFailed)),
                    CustomValidationFailure.IssuerValidationFailed,
                    Default.GetStackFrame(),
                    issuer)));
        }

        internal async static Task<ValidationResult<ValidatedIssuer, ValidationError>> IssuerValidationFailed(
            string issuer,
            SecurityToken securityToken,
            ValidationParameters validationParameters,
            CallContext callContext,
            CancellationToken cancellationToken)
        {
            return await Task.FromResult(new ValidationResult<ValidatedIssuer, ValidationError>(
                new CustomIssuerValidationError(
                    new MessageDetail(nameof(IssuerValidationFailed)),
                    IssuerValidationFailure.ValidationFailed,
                    Default.GetStackFrame(),
                    issuer)));
        }

        internal async static Task<ValidationResult<ValidatedIssuer, ValidationError>> UnknownValidationFailure(
            string issuer,
            SecurityToken securityToken,
            ValidationParameters validationParameters,
            CallContext callContext,
            CancellationToken cancellationToken)
        {
            return await Task.FromResult(new ValidationResult<ValidatedIssuer, ValidationError>(
                new CustomIssuerValidationError(
                    new MessageDetail(nameof(UnknownValidationFailure)),
                    AlgorithmValidationFailure.AlgorithmIsNotSupported,
                    Default.GetStackFrame(),
                    issuer)));
        }

        internal async static Task<ValidationResult<ValidatedIssuer, ValidationError>> IssuerValidatorDelegateAsync(
            string issuer,
            SecurityToken securityToken,
            ValidationParameters validationParameters,
            CallContext callContext,
            CancellationToken cancellationToken)
        {
            return await Task.FromResult(new ValidationResult<ValidatedIssuer, ValidationError>(
                new IssuerValidationError(
                    new MessageDetail(nameof(IssuerValidatorDelegateAsync)),
                    IssuerValidationFailure.ValidationFailed,
                    Default.GetStackFrame(),
                    issuer)));
        }

        internal static Task<ValidationResult<ValidatedIssuer, ValidationError>> IssuerValidatorThrows(
            string issuer,
            SecurityToken securityToken,
            ValidationParameters validationParameters,
            CallContext callContext,
            CancellationToken cancellationToken)
        {
            throw new CustomSecurityTokenInvalidIssuerException(
                nameof(IssuerValidatorThrows),
                new IssuerValidationError(
                    new MessageDetail(nameof(IssuerValidatorDelegateAsync)),
                    IssuerValidationFailure.ValidationFailed,
                    Default.GetStackFrame(),
                    issuer),
                null);
        }
    }
}
#nullable restore
