// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
#nullable enable
namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Contains the result of validating the <see cref="SecurityKey"/> used to sign a <see cref="SecurityToken"/>.
    /// The <see cref="TokenValidationResult"/> contains a collection of <see cref="ValidationResult"/> for each step in the token validation.
    /// </summary>
    internal class SigningKeyValidationResult : ValidationResult
    {
        private Exception? _exception;

        /// <summary>
        /// Creates an instance of <see cref="SigningKeyValidationResult"/>
        /// </summary>
        /// <paramref name="signingKey"/> is the security key that was validated successfully.
        public SigningKeyValidationResult(SecurityKey? signingKey)
            : base(ValidationFailureType.ValidationSucceeded)
        {
            SigningKey = signingKey;
            IsValid = true;
        }

        /// <summary>
        /// Creates an instance of <see cref="SigningKeyValidationResult"/>
        /// </summary>
        /// <paramref name="signingKey"/> is the security key that was intended to be validated.
        /// <paramref name="validationFailure"/> is the <see cref="ValidationFailureType"/> that occurred during validation.
        /// <paramref name="exceptionDetail"/> is the <see cref="ExceptionDetail"/> that occurred during validation.
        public SigningKeyValidationResult(SecurityKey? signingKey, ValidationFailureType validationFailure, ExceptionDetail exceptionDetail)
            : base(validationFailure, exceptionDetail)
        {
            SigningKey = signingKey;
            IsValid = false;
        }

        /// <summary>
        /// Gets the <see cref="Exception"/> that occurred during validation.
        /// </summary>
        public override Exception? Exception
        {
            get
            {
                if (_exception != null || ExceptionDetail == null)
                    return _exception;

                HasValidOrExceptionWasRead = true;
                _exception = ExceptionDetail.GetException();
                if (_exception is SecurityTokenInvalidSigningKeyException securityTokenInvalidSigningKeyException)
                {
                    securityTokenInvalidSigningKeyException.SigningKey = SigningKey;
                    securityTokenInvalidSigningKeyException.ExceptionDetail = ExceptionDetail;
                    securityTokenInvalidSigningKeyException.Source = "Microsoft.IdentityModel.Tokens";
                }

                return _exception;
            }
        }

        /// <summary>
        /// Gets the security key that was validated or intended to be validated.
        /// </summary>
        public SecurityKey? SigningKey { get; }
    }
}
#nullable restore
