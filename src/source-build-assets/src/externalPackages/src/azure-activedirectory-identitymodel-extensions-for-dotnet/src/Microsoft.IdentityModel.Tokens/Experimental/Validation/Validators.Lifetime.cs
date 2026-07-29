// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens.Experimental;

#nullable enable
namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Partial class for Lifetime Validation.
    /// </summary>
    public static partial class Validators
    {
        /// <summary>
        /// Validates the lifetime of a <see cref="SecurityToken"/>.
        /// </summary>
        /// <param name="notBefore">The <see cref="DateTime"/> 'notBefore' time found in the <see cref="SecurityToken"/>.</param>
        /// <param name="expires">The 'expiration' time found in the <see cref="SecurityToken"/>.</param>
        /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
        /// <param name="validationParameters">The <see cref="ValidationParameters"/> to be used for validating the token.</param>
        /// <param name="callContext">The <see cref="CallContext"/> that contains call information.</param>
        /// <returns>A <see cref="ValidationResult{ValidatedLifetime, LifetimeValidationError}"/> indicating whether validation was successful, and providing a <see cref="SecurityTokenInvalidLifetimeException"/> if it was not.</returns>
        /// <remarks>All time comparisons apply <see cref="ValidationParameters.ClockSkew"/>.</remarks>
        internal static ValidationResult<ValidatedLifetime, ValidationError> ValidateLifetimeInternal(
            DateTime? notBefore,
            DateTime? expires,
            SecurityToken? securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            if (validationParameters == null)
                return ValidationError.NullParameter(
                    nameof(validationParameters),
                    ValidationError.GetCurrentStackFrame());

            try
            {
                ValidationResult<ValidatedLifetime, ValidationError> result =
                    validationParameters.LifetimeValidator(
                        notBefore,
                        expires,
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
                return new LifetimeValidationError(
                    new MessageDetail(Tokens.LogMessages.IDX10271),
                    LifetimeValidationFailure.ValidatorThrew,
                    ValidationError.GetCurrentStackFrame(),
                    notBefore,
                    expires,
                    ex);
            }
        }

        /// <summary>
        /// Validates the lifetime of a <see cref="SecurityToken"/>.
        /// </summary>
        /// <param name="notBefore">The <see cref="DateTime"/> 'notBefore' time found in the <see cref="SecurityToken"/>.</param>
        /// <param name="expires">The 'expiration' time found in the <see cref="SecurityToken"/>.</param>
        /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
        /// <param name="validationParameters">The <see cref="ValidationParameters"/> to be used for validating the token.</param>
        /// <param name="callContext">The <see cref="CallContext"/> that contains call information.</param>
        /// <returns>A <see cref="ValidationResult{ValidatedLifetime, LifetimeValidationError}"/> indicating whether validation was successful, and providing a <see cref="SecurityTokenInvalidLifetimeException"/> if it was not.</returns>
        /// <remarks>All time comparisons apply <see cref="ValidationParameters.ClockSkew"/>.</remarks>
        public static ValidationResult<ValidatedLifetime, ValidationError> ValidateLifetime(
            DateTime? notBefore,
            DateTime? expires,
            SecurityToken? securityToken,
            ValidationParameters validationParameters,
#pragma warning disable CA1801
            CallContext callContext)
#pragma warning restore CA1801
        {
            if (validationParameters == null)
                return ValidationError.NullParameter(
                    nameof(validationParameters),
                    ValidationError.GetCurrentStackFrame());

            if (!expires.HasValue)
                return new LifetimeValidationError(
                    new MessageDetail(
                        LogMessages.IDX10225,
                        LogHelper.MarkAsNonPII(securityToken == null ? "null" : securityToken.GetType().ToString())),
                    LifetimeValidationFailure.NoExpirationTime,
                    ValidationError.GetCurrentStackFrame(),
                    notBefore,
                    expires);

            if (notBefore.HasValue && expires.HasValue && (notBefore.Value > expires.Value))
                return new LifetimeValidationError(
                    new MessageDetail(
                        LogMessages.IDX10224,
                        LogHelper.MarkAsNonPII(notBefore.Value),
                        LogHelper.MarkAsNonPII(expires.Value)),
                    LifetimeValidationFailure.NotbeforeGreaterThanExpirationTime,
                    ValidationError.GetCurrentStackFrame(),
                    notBefore,
                    expires);

            DateTime utcNow = validationParameters.TimeProvider.GetUtcNow().UtcDateTime;
            if (notBefore.HasValue && (notBefore.Value > DateTimeUtil.Add(utcNow, validationParameters.ClockSkew)))
                return new LifetimeValidationError(
                    new MessageDetail(
                        LogMessages.IDX10222,
                        LogHelper.MarkAsNonPII(notBefore.Value),
                        LogHelper.MarkAsNonPII(utcNow)),
                    LifetimeValidationFailure.NotYetValid,
                    ValidationError.GetCurrentStackFrame(),
                    notBefore,
                    expires);

            if (expires.HasValue && (expires.Value < DateTimeUtil.Add(utcNow, validationParameters.ClockSkew.Negate())))
                return new LifetimeValidationError(
                    new MessageDetail(
                        LogMessages.IDX10223,
                        LogHelper.MarkAsNonPII(expires.Value),
                        LogHelper.MarkAsNonPII(utcNow)),
                    LifetimeValidationFailure.Expired,
                    ValidationError.GetCurrentStackFrame(),
                    notBefore,
                    expires);

            // if it reaches here, that means lifetime of the token is valid
            return new ValidatedLifetime(notBefore, expires);
        }
    }
}
#nullable restore
