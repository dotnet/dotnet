// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

#nullable enable
namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Contains the result of validating that a <see cref="SecurityToken"/> has not been replayed.
    /// The <see cref="TokenValidationResult"/> contains a collection of <see cref="ValidationResult"/> for each step in the token validation.
    /// </summary>
    internal class ReplayValidationResult : ValidationResult
    {
        private Exception? _exception;

        /// <summary>
        /// Creates an instance of <see cref="ReplayValidationResult"/>.
        /// </summary>
        /// <paramref name="expirationTime"/> is the expiration date against which the token was validated.
        public ReplayValidationResult(DateTime? expirationTime) : base(ValidationFailureType.ValidationSucceeded)
        {
            IsValid = true;
            ExpirationTime = expirationTime;
        }

        /// <summary>
        /// Creates an instance of <see cref="ReplayValidationResult"/>
        /// </summary>
        /// <paramref name="expirationTime"/> is the expiration date against which the token was validated.
        /// <paramref name="validationFailure"/> is the <see cref="ValidationFailureType"/> that occurred during validation.
        /// <paramref name="exceptionDetail"/> is the <see cref="ExceptionDetail"/> that occurred during validation.
        public ReplayValidationResult(DateTime? expirationTime, ValidationFailureType validationFailure, ExceptionDetail exceptionDetail)
            : base(validationFailure, exceptionDetail)
        {
            IsValid = false;
            ExpirationTime = expirationTime;
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
                _exception.Source = "Microsoft.IdentityModel.Tokens";

                if (_exception is SecurityTokenReplayDetectedException securityTokenReplayDetectedException)
                {
                    securityTokenReplayDetectedException.ExceptionDetail = ExceptionDetail;
                }
                else if (_exception is SecurityTokenReplayAddFailedException securityTokenReplayAddFailedException)
                {
                    securityTokenReplayAddFailedException.ExceptionDetail = ExceptionDetail;
                }

                return _exception;
            }
        }

        /// <summary>
        /// Gets the expiration date against which the token was validated.
        /// </summary>
        public DateTime? ExpirationTime { get; }
    }
}
#nullable restore
