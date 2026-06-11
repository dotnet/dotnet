// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;

#nullable enable
namespace Microsoft.IdentityModel.TestUtils
{
    internal class CustomLifetimeValidationDelegates
    {
        internal static ValidationResult<ValidatedLifetime, ValidationError> CustomLifetimeValidationFailed(
            DateTime? notBefore,
            DateTime? expires,
            SecurityToken? securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            return new CustomLifetimeValidationError(
                new MessageDetail(nameof(CustomLifetimeValidationFailed)),
                CustomValidationFailure.LifetimeValidationFailed,
                Default.GetStackFrame(),
                notBefore,
                expires,
                null);
        }

        internal static ValidationResult<ValidatedLifetime, ValidationError> LifetimeValidationFailed(
            DateTime? notBefore,
            DateTime? expires,
            SecurityToken? securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            return new LifetimeValidationError(
                new MessageDetail(nameof(LifetimeValidationFailure.ValidationFailed)),
                LifetimeValidationFailure.ValidationFailed,
                Default.GetStackFrame(),
                notBefore,
                expires,
                null);
        }

        internal static ValidationResult<ValidatedLifetime, ValidationError> LifetimeValidator(
            DateTime? notBefore,
            DateTime? expires,
            SecurityToken? securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            return new LifetimeValidationError(
                new MessageDetail(nameof(LifetimeValidator)),
                LifetimeValidationFailure.ValidationFailed,
                Default.GetStackFrame(),
                notBefore,
                expires);
        }

        internal static ValidationResult<ValidatedLifetime, ValidationError> CustomUnknownValidationFailure(
            DateTime? notBefore,
            DateTime? expires,
            SecurityToken? securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            return new CustomLifetimeValidationError(
                new MessageDetail(nameof(CustomUnknownValidationFailure)),
                AlgorithmValidationFailure.AlgorithmIsNotSupported,
                Default.GetStackFrame(),
                notBefore,
                expires,
                null);
        }

        internal static ValidationResult<ValidatedLifetime, ValidationError> ValidatorDelegate(
            DateTime? notBefore,
            DateTime? expires,
            SecurityToken? securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            return new LifetimeValidationError(
                new MessageDetail(nameof(ValidatorDelegate)),
                LifetimeValidationFailure.ValidationFailed,
                Default.GetStackFrame(),
                notBefore,
                expires,
                null);
        }

        internal static ValidationResult<ValidatedLifetime, ValidationError> ValidatorThrows(
            DateTime? notBefore,
            DateTime? expires,
            SecurityToken? securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            throw new CustomSecurityTokenInvalidLifetimeException(
                nameof(ValidatorThrows),
                new LifetimeValidationError(
                    new MessageDetail(nameof(ValidatorThrows)),
                    LifetimeValidationFailure.ValidatorThrew,
                    Default.GetStackFrame(),
                    notBefore,
                    expires,
                    null),
                null);
        }
    }
}
#nullable restore
