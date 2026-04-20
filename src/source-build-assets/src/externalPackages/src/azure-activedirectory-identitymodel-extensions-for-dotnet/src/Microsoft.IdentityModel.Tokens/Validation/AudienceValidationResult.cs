// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Contains the result of validating the audiences from a <see cref="SecurityToken"/>.
    /// The <see cref="TokenValidationResult"/> contains a collection of <see cref="ValidationResult"/> for each step in the token validation.
    /// </summary>
    internal class AudienceValidationResult : ValidationResult
    {
        private Exception _exception;

        /// <summary>
        /// Creates an instance of <see cref="AudienceValidationResult"/>.
        /// </summary>
        /// <paramref name="audience"/> is the audience that was validated successfully.
        public AudienceValidationResult(string audience) : base(ValidationFailureType.ValidationSucceeded)
        {
            IsValid = true;
            Audience = audience;
        }

        /// <summary>
        /// Creates an instance of <see cref="IssuerValidationResult"/>
        /// </summary>
        /// <paramref name="audience"/> is the audience that was intended to be validated.
        /// <paramref name="validationFailure"/> is the <see cref="ValidationFailureType"/> that occurred during validation.
        /// <paramref name="exceptionDetail"/> is the <see cref="ExceptionDetail"/> that occurred during validation.
        public AudienceValidationResult(string audience, ValidationFailureType validationFailure, ExceptionDetail exceptionDetail)
            : base(validationFailure, exceptionDetail)
        {
            IsValid = false;
            Audience = audience;
        }

        /// <summary>
        /// Gets the <see cref="Exception"/> that occurred during validation.
        /// </summary>
        public override Exception Exception
        {
            get
            {
                if (_exception != null || ExceptionDetail == null)
                    return _exception;

                HasValidOrExceptionWasRead = true;
                _exception = ExceptionDetail.GetException();
                if (_exception is SecurityTokenInvalidAudienceException securityTokenInvalidAudienceException)
                {
                    securityTokenInvalidAudienceException.InvalidAudience = Audience;
                    securityTokenInvalidAudienceException.ExceptionDetail = ExceptionDetail;
                    securityTokenInvalidAudienceException.Source = "Microsoft.IdentityModel.Tokens";
                }

                return _exception;
            }
        }

        /// <summary>
        /// Gets the audience that was validated or intended to be validated.
        /// </summary>
        public string Audience { get; }
    }
}
