// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

#nullable enable
using System.Threading.Tasks;
using System.Threading;

namespace Microsoft.IdentityModel.Tokens.Experimental
{
    /// <summary>
    /// Experimental interface that provides result based token validation instead of throwing exceptions.
    /// </summary>
    public interface IResultBasedValidation
    {
        /// <summary>
        /// Validates a token.
        /// On a validation failure, no exception will be thrown; instead, the <see cref="ValidationError"/> will contain the information about the error that occurred.
        /// Callers should always check the ValidationResult.IsValid property to verify the validity of the result.
        /// </summary>
        /// <param name="token">The token to be validated.</param>
        /// <param name="validationParameters">The <see cref="ValidationParameters"/> to be used for validating the token.</param>
        /// <param name="callContext">A <see cref="CallContext"/> that contains call information.</param>
        /// <returns>A <see cref="ValidationResult{TResult, TError}"/> with either a <see cref="ValidatedToken"/> if the token was validated or an <see cref="ValidationError"/> with the failure information and exception otherwise.</returns>
        public Task<ValidationResult<ValidatedToken, ValidationError>> ValidateTokenAsync(
            string token,
            ValidationParameters validationParameters,
            CallContext callContext);

        /// <summary>
        /// Validates a token.
        /// On a validation failure, no exception will be thrown; instead, the <see cref="ValidationError"/> will contain the information about the error that occurred.
        /// Callers should always check the ValidationResult.IsValid property to verify the validity of the result.
        /// </summary>
        /// <param name="token">The token to be validated.</param>
        /// <param name="validationParameters">The <see cref="ValidationParameters"/> to be used for validating the token.</param>
        /// <param name="callContext">A <see cref="CallContext"/> that contains call information.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to request cancellation of the asynchronous operation.</param>
        /// <returns>A <see cref="ValidationResult{TResult, TError}"/> with either a <see cref="ValidatedToken"/> if the token was validated or an <see cref="ValidationError"/> with the failure information and exception otherwise.</returns>
        public Task<ValidationResult<ValidatedToken, ValidationError>> ValidateTokenAsync(
            string token,
            ValidationParameters validationParameters,
            CallContext callContext,
            CancellationToken cancellationToken);

        /// <summary>
        /// Validates a token.
        /// On a validation failure, no exception will be thrown; instead, the <see cref="ValidationError"/> will contain the information about the error that occurred.
        /// Callers should always check the ValidationResult.IsValid property to verify the validity of the result.
        /// </summary>
        /// <param name="securityToken">The <see cref="SecurityToken"/> to be validated.</param>
        /// <param name="validationParameters">The <see cref="ValidationParameters"/> to be used for validating the token.</param>
        /// <param name="callContext">A <see cref="CallContext"/> that contains call information.</param>
        /// <returns>A <see cref="ValidationResult{TResult, TError}"/> with either a <see cref="ValidatedToken"/> if the token was validated or an <see cref="ValidationError"/> with the failure information and exception otherwise.</returns>
        public Task<ValidationResult<ValidatedToken, ValidationError>> ValidateTokenAsync(
            SecurityToken securityToken,
            ValidationParameters validationParameters,
            CallContext callContext);

        /// <summary>
        /// Validates a token.
        /// On a validation failure, no exception will be thrown; instead, the <see cref="ValidationError"/> will contain the information about the error that occurred.
        /// Callers should always check the ValidationResult.IsValid property to verify the validity of the result.
        /// </summary>
        /// <param name="securityToken">The <see cref="SecurityToken"/> to be validated.</param>
        /// <param name="validationParameters">The <see cref="ValidationParameters"/> to be used for validating the token.</param>
        /// <param name="callContext">A <see cref="CallContext"/> that contains call information.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to request cancellation of the asynchronous operation.</param>
        /// <returns>A <see cref="ValidationResult{TResult, TError}"/> with either a <see cref="ValidatedToken"/> if the token was validated or an <see cref="ValidationError"/> with the failure information and exception otherwise.</returns>
        public Task<ValidationResult<ValidatedToken, ValidationError>> ValidateTokenAsync(
            SecurityToken securityToken,
            ValidationParameters validationParameters,
            CallContext callContext,
            CancellationToken cancellationToken);
    }
}
#nullable restore
