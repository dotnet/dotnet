// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
#nullable enable
namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Contains the result of validating the lifetime of a <see cref="SecurityToken"/>.
    /// The <see cref="TokenValidationResult"/> contains a collection of <see cref="ValidationResult"/> for each step in the token validation.
    /// </summary>
    internal class LifetimeValidationResult : ValidationResult
    {
        private Exception? _exception;

        /// <summary>
        /// Creates an instance of <see cref="LifetimeValidationResult"/>
        /// </summary>
        /// <paramref name="notBefore"/> is the date from which the token that was validated successfully is valid.
        /// <paramref name="expires"/> is the expiration date for the token that was validated successfully.
        public LifetimeValidationResult(DateTime? notBefore, DateTime? expires)
            : base(ValidationFailureType.ValidationSucceeded)
        {
            NotBefore = notBefore;
            Expires = expires;
            IsValid = true;
        }

        /// <summary>
        /// Creates an instance of <see cref="LifetimeValidationResult"/>
        /// </summary>
        /// <paramref name="notBefore"/> is the date from which the token is valid.
        /// <paramref name="expires"/> is the expiration date for the token.
        /// <paramref name="validationFailure"/> is the <see cref="ValidationFailureType"/> that occurred during validation.
        /// <paramref name="exceptionDetail"/> is the <see cref="ExceptionDetail"/> that occurred during validation.
        public LifetimeValidationResult(DateTime? notBefore, DateTime? expires, ValidationFailureType validationFailure, ExceptionDetail exceptionDetail)
            : base(validationFailure, exceptionDetail)
        {
            NotBefore = notBefore;
            Expires = expires;
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
                if (_exception is SecurityTokenInvalidLifetimeException securityTokenInvalidLifetimeException)
                {
                    securityTokenInvalidLifetimeException.NotBefore = NotBefore;
                    securityTokenInvalidLifetimeException.Expires = Expires;
                    securityTokenInvalidLifetimeException.Source = "Microsoft.IdentityModel.Tokens";
                }

                return _exception;
            }
        }

        /// <summary>
        /// Gets the date from which the token is valid.
        /// </summary>
        public DateTime? NotBefore { get; }

        /// <summary>
        /// Gets the expiration date for the token.
        /// </summary>
        public DateTime? Expires { get; }
    }
}
#nullable restore
