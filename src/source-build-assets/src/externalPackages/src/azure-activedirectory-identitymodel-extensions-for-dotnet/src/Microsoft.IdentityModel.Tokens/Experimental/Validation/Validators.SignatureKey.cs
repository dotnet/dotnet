// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens.Experimental;

#nullable enable
namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Partial class for Signature Key Validation.
    /// </summary>
    public static partial class Validators
    {
        /// <summary>
        /// Validates the <see cref="SecurityKey"/> that signed a <see cref="SecurityToken"/> is valie.
        /// </summary>
        /// <param name="securityKey">The <see cref="SecurityKey"/> that signed the <see cref="SecurityToken"/>.</param>
        /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
        /// <param name="validationParameters">The <see cref="ValidationParameters"/> to be used for validating the token.</param>
        /// <param name="callContext">The <see cref="CallContext"/> that contains call information.</param>
        /// <returns>A <see cref="ValidationResult{ValidatedSignatureKey, ValidationError}"/> indicating whether validation was successful, and providing a <see cref="SecurityTokenInvalidLifetimeException"/> if it was not.</returns>
        internal static ValidationResult<ValidatedSignatureKey, ValidationError> ValidateSignatureKeyInternal(
            SecurityKey securityKey,
            SecurityToken securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            if (validationParameters == null)
                return ValidationError.NullParameter(
                    nameof(validationParameters),
                    ValidationError.GetCurrentStackFrame());

            try
            {
                ValidationResult<ValidatedSignatureKey, ValidationError> result =
                    validationParameters.SignatureKeyValidator(
                        securityKey,
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
                return new SignatureKeyValidationError(
                    new MessageDetail(Tokens.LogMessages.IDX10274),
                    SignatureKeyValidationFailure.ValidatorThrew,
                    ValidationError.GetCurrentStackFrame(),
                    securityKey,
                    ex);
            }
        }

        /// <summary>
        /// Validates the <see cref="SecurityKey"/> that signed a <see cref="SecurityToken"/> is valid.
        /// </summary>
        /// <param name="securityKey">The <see cref="SecurityKey"/> that signed the <see cref="SecurityToken"/>.</param>
        /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
        /// <param name="validationParameters">The <see cref="ValidationParameters"/> to be used for validating the token.</param>
        /// <param name="callContext">The <see cref="CallContext"/> that contains call information.</param>
        /// <returns>A <see cref="ValidationResult{ValidatedSignatureKey, IssuerSigningKeyValidationError}"/> indicating whether validation was successful, and providing a <see cref="SecurityTokenInvalidLifetimeException"/> if it was not.</returns>
        public static ValidationResult<ValidatedSignatureKey, ValidationError> ValidateSignatureKey(
            SecurityKey securityKey,
            SecurityToken securityToken,
            ValidationParameters validationParameters,
            CallContext callContext)
        {
            if (validationParameters == null)
                return ValidationError.NullParameter(
                    nameof(validationParameters),
                    ValidationError.GetCurrentStackFrame());

            // TODO error message IDX10253 is not correct and needs adjusting.
            if (securityKey == null)
                return new SignatureKeyValidationError(
                    new MessageDetail(LogMessages.IDX10253, nameof(securityKey)),
                    SignatureKeyValidationFailure.KeyIsNull,
                    ValidationError.GetCurrentStackFrame(),
                    securityKey);

            if (securityToken == null)
                return ValidationError.NullParameter(
                    nameof(securityToken),
                    ValidationError.GetCurrentStackFrame());

            return ValidateSignatureKey(securityKey, validationParameters, callContext);
        }

        /// <summary>
        /// Given a signing key, when it's derived from a certificate, validates that the certificate is already active and non-expired
        /// </summary>
        /// <param name="securityKey">The <see cref="SecurityKey"/> that signed the <see cref="SecurityToken"/>.</param>
        /// <param name="validationParameters">The <see cref="ValidationParameters"/> to be used for validating the token.</param>
        /// <param name="callContext">The <see cref="CallContext"/> that contains call information.</param>
#pragma warning disable CA1801 // Review unused parameters
        internal static ValidationResult<ValidatedSignatureKey, ValidationError> ValidateSignatureKey(
            SecurityKey securityKey,
            ValidationParameters validationParameters,
            CallContext callContext)
#pragma warning restore CA1801 // Review unused parameters
        {
            DateTime utcNow = validationParameters.TimeProvider.GetUtcNow().UtcDateTime;
            DateTime? notBeforeUtc = null;
            DateTime? notAfterUtc = null;
            X509SecurityKey? x509SecurityKey = securityKey as X509SecurityKey;

            if (x509SecurityKey?.Certificate is X509Certificate2 cert)
            {
                notBeforeUtc = cert.NotBefore.ToUniversalTime();
                notAfterUtc = cert.NotAfter.ToUniversalTime();

                if (notBeforeUtc > DateTimeUtil.Add(utcNow, validationParameters.ClockSkew))
                    return new SignatureKeyValidationError(
                        new MessageDetail(
                            LogMessages.IDX10248,
                            LogHelper.MarkAsNonPII(notBeforeUtc),
                            LogHelper.MarkAsNonPII(utcNow)),
                        SignatureKeyValidationFailure.NotYetValid,
                        ValidationError.GetCurrentStackFrame(),
                        securityKey);

                if (notAfterUtc < DateTimeUtil.Add(utcNow, validationParameters.ClockSkew.Negate()))
                    return new SignatureKeyValidationError(
                        new MessageDetail(
                            LogMessages.IDX10249,
                            LogHelper.MarkAsNonPII(notAfterUtc),
                            LogHelper.MarkAsNonPII(utcNow)),
                        SignatureKeyValidationFailure.KeyExpired,
                        ValidationError.GetCurrentStackFrame(),
                        securityKey);
            }

            return new ValidatedSignatureKey(notBeforeUtc, notAfterUtc, utcNow);
        }
    }
}
#nullable restore
