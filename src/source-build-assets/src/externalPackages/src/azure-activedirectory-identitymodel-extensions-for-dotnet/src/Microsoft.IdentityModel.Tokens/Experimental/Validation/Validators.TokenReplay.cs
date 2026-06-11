// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.IdentityModel.Tokens.Experimental;

#nullable enable
namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Partial class for Token Replay validation.
    /// </summary>
    public static partial class Validators
    {
        /// <summary>
        /// Validates if a token has been replayed.
        /// </summary>
        /// <param name="expires">The <see cref="DateTime"/> when the security token expires.</param>
        /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
        /// <param name="validationParameters">The <see cref="ValidationParameters"/> to be used for validating the token.</param>
        /// <param name="callContext">The <see cref="CallContext"/> that contains call information.</param>
        internal static ValidationResult<DateTime?, ValidationError> ValidateTokenReplayInternal(
            DateTime? expires,
            string securityToken,
            ValidationParameters validationParameters,
#pragma warning disable CA1801 // Review unused parameters
            CallContext callContext)
#pragma warning restore CA1801 // Review unused parameters
        {
            if (validationParameters == null)
                return ValidationError.NullParameter(
                    nameof(validationParameters),
                    ValidationError.GetCurrentStackFrame());

            try
            {
                ValidationResult<DateTime?, ValidationError> result =
                    validationParameters.TokenReplayValidator(
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
                return new TokenReplayValidationError(
                    new MessageDetail(Tokens.LogMessages.IDX10276),
                    TokenReplayValidationFailure.ValidatorThrew,
                    ValidationError.GetCurrentStackFrame(),
                    expires,
                    ex);
            }
        }

        /// <summary>
        /// Validates if a token has been replayed.
        /// </summary>
        /// <param name="expires">The <see cref="DateTime"/> when the security token expires.</param>
        /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
        /// <param name="validationParameters">The <see cref="ValidationParameters"/> to be used for validating the token.</param>
        /// <param name="callContext">The <see cref="CallContext"/> that contains call information.</param>
        public static ValidationResult<DateTime?, ValidationError> ValidateTokenReplay(
            DateTime? expires,
            string securityToken,
            ValidationParameters validationParameters,
#pragma warning disable CA1801 // Review unused parameters
            CallContext callContext)
#pragma warning restore CA1801 // Review unused parameters
        {
            if (string.IsNullOrWhiteSpace(securityToken))
                return ValidationError.NullParameter(
                    nameof(securityToken),
                    ValidationError.GetCurrentStackFrame());

            if (validationParameters == null)
                return ValidationError.NullParameter(
                    nameof(validationParameters),
                    ValidationError.GetCurrentStackFrame());

            // check if token if replay cache is set, then there must be an expiration time.
            if (validationParameters.TokenReplayCache != null)
            {
                if (expires == null)
                    return new TokenReplayValidationError(
                        new MessageDetail(
                            LogMessages.IDX10227,
                            securityToken),
                        TokenReplayValidationFailure.NoExpiration,
                        ValidationError.GetCurrentStackFrame(),
                        expires);

                if (validationParameters.TokenReplayCache.TryFind(securityToken))
                    return new TokenReplayValidationError(
                        new MessageDetail(
                            LogMessages.IDX10228,
                            securityToken),
                        TokenReplayValidationFailure.TokenFoundInCache,
                        ValidationError.GetCurrentStackFrame(),
                        expires);

                if (!validationParameters.TokenReplayCache.TryAdd(securityToken, expires.Value))
                    return new TokenReplayValidationError(
                        new MessageDetail(
                            LogMessages.IDX10229,
                            securityToken),
                        TokenReplayValidationFailure.AddToCacheFailed,
                        ValidationError.GetCurrentStackFrame(),
                        expires);
            }

            // if it reaches here, that means no token replay is detected.
            // TODO: Move to CallContext
            //LogHelper.LogInformation(LogMessages.IDX10240);
            return expires;
        }
    }
}
#nullable restore
