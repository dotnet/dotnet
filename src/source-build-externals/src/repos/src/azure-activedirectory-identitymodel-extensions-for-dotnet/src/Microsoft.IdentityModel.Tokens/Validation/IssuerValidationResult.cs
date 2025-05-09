// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Contains the result of validating a <see cref="SecurityToken"/> issuer.
    /// The <see cref="TokenValidationResult"/> contains a collection of <see cref="ValidationResult"/> for each step in the token validation.
    /// </summary>
    internal class IssuerValidationResult : ValidationResult
    {
        internal enum ValidationSource
        {
            NotValidated = 0,
            IssuerIsConfigurationIssuer,
            IssuerIsValidIssuer,
            IssuerIsAmongValidIssuers
        }

        private Exception _exception;

        /// <summary>
        /// Creates an instance of <see cref="IssuerValidationResult"/>
        /// </summary>
        /// <paramref name="issuer"/> is the issuer that was validated successfully.
        /// <paramref name="source"/> is the <see cref="ValidationSource"/> indicating how this issuer was validated.
        public IssuerValidationResult(string issuer, ValidationSource source = ValidationSource.NotValidated)
            : base(ValidationFailureType.ValidationSucceeded)
        {
            Issuer = issuer;
            IsValid = true;
            Source = source;
        }

        /// <summary>
        /// Creates an instance of <see cref="IssuerValidationResult"/>
        /// </summary>
        /// <paramref name="issuer"/> is the issuer that was intended to be validated.
        /// <paramref name="validationFailure"/> is the <see cref="ValidationFailureType"/> that occurred during validation.
        /// <paramref name="exceptionDetail"/> is the <see cref="ExceptionDetail"/> that occurred during validation.
        /// <paramref name="source"/> is the <see cref="ValidationSource"/> indicating how this issuer was validated.
        public IssuerValidationResult(string issuer, ValidationFailureType validationFailure, ExceptionDetail exceptionDetail, ValidationSource source = ValidationSource.NotValidated)
            : base(validationFailure, exceptionDetail)
        {
            Issuer = issuer;
            IsValid = false;
            Source = source;
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
                SecurityTokenInvalidIssuerException securityTokenInvalidIssuerException = _exception as SecurityTokenInvalidIssuerException;
                if (securityTokenInvalidIssuerException != null)
                {
                    securityTokenInvalidIssuerException.InvalidIssuer = Issuer;
                    securityTokenInvalidIssuerException.ExceptionDetail = ExceptionDetail;
                    securityTokenInvalidIssuerException.Source = "Microsoft.IdentityModel.Tokens";
                }

                return _exception;
            }
        }

        /// <summary>
        /// Gets the issuer that was validated or intended to be validated.
        /// </summary>
        public string Issuer { get; }

        public ValidationSource Source { get; }
    }
}
