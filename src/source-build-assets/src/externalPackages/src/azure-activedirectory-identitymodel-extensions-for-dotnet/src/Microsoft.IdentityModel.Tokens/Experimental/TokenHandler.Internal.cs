// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens.Experimental;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Defines properties shared across all security token handlers.
    /// </summary>
    public abstract partial class TokenHandler
    {
        /// <summary>
        /// Validates a token.
        /// On validation failure no exception will be thrown. <see cref="ValidationError"/> will contain information pertaining to the error.
        /// </summary>
        /// <param name="token">The token to be validated.</param>
        /// <param name="validationParameters">The <see cref="ValidationParameters"/> to be used for validating the token.</param>
        /// <param name="callContext">A <see cref="CallContext"/> that contains call information.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to request cancellation of the asynchronous operation.</param>
        /// <returns>An <see cref="ValidationResult{ValidatedToken, ValidationError}"/> with either a <see cref="ValidatedToken"/>
        /// if the token was validated or a <see cref="ValidationError"/> containing the failure information.</returns>
        internal virtual Task<ValidationResult<ValidatedToken, ValidationError>> ValidateTokenAsync(
            string token,
            ValidationParameters validationParameters,
            CallContext callContext,
            CancellationToken cancellationToken)
        {
            throw LogExceptionMessage(
                new NotImplementedException(
                    FormatInvariant(
                        LogMessages.IDX10267,
                        MarkAsNonPII("internal virtual Task<OperationResult<ValidatedToken, ValidationError>> " +
                        "ValidateTokenAsync(string token, ValidationParameters validationParameters, CallContext callContext, CancellationToken cancellationToken)"),
                        MarkAsNonPII(GetType().FullName))));
        }

        /// <summary>
        /// Validates a token.
        /// On validation failure no exception will be thrown. <see cref="ValidationError"/> will contain information pertaining to the error.
        /// </summary>
        /// <param name="token">The token to be validated.</param>
        /// <param name="validationParameters">The <see cref="ValidationParameters"/> to be used for validating the token.</param>
        /// <param name="callContext">A <see cref="CallContext"/> that contains call information.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to request cancellation of the asynchronous operation.</param>
        /// <returns>An <see cref="ValidationResult{ValidatedToken, ValidationError}"/> with either a <see cref="ValidatedToken"/>
        /// if the token was validated or a <see cref="ValidationError"/> containing the failure information.</returns>
        internal virtual Task<ValidationResult<ValidatedToken, ValidationError>> ValidateTokenAsync(
            SecurityToken token,
            ValidationParameters validationParameters,
            CallContext callContext,
            CancellationToken cancellationToken)
        {
            throw LogExceptionMessage(
                new NotImplementedException(
                    FormatInvariant(
                        LogMessages.IDX10267,
                        MarkAsNonPII("internal virtual Task<OperationResult<ValidatedToken, ValidationError>> " +
                        "ValidateTokenAsync(SecurityToken token, ValidationParameters validationParameters, CallContext callContext, CancellationToken cancellationToken)"),
                        MarkAsNonPII(GetType().FullName))));
        }

        /// <summary>
        /// Called by base class to create a <see cref="ClaimsIdentity"/>.
        /// Currently only used by the JsonWebTokenHandler when called with ValidationParameters to allow for a Lazy creation.
        /// </summary>
        /// <param name="securityToken">the <see cref="SecurityToken"/> that has the Claims.</param>
        /// <param name="validationParameters">the <see cref="ValidationParameters"/> that was used to validate the token.</param>
        /// <param name="issuer">the 'issuer' to use by default when creating a Claim.</param>
        /// <returns>A <see cref="ClaimsIdentity"/>.</returns>
        /// <exception cref="NotImplementedException"></exception>
        internal virtual ClaimsIdentity CreateClaimsIdentityInternal(
            SecurityToken securityToken,
            ValidationParameters validationParameters,
            string issuer)
        {
            throw LogExceptionMessage(
                new NotImplementedException(
                    FormatInvariant(
                        LogMessages.IDX10267,
                        MarkAsNonPII("internal virtual ClaimsIdentity CreateClaimsIdentityInternal(SecurityToken securityToken, ValidationParameters validationParameters, string issuer)"),
                        MarkAsNonPII(GetType().FullName))));
        }
    }
}
