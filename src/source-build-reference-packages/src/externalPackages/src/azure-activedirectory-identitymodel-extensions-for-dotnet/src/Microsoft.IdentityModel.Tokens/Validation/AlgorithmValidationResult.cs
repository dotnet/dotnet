// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
#nullable enable
namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Contains the result of validating the Algorithm of a <see cref="SecurityToken"/>.
    /// The <see cref="TokenValidationResult"/> contains a collection of <see cref="ValidationResult"/> for each step in the token validation.
    /// </summary>
    internal class AlgorithmValidationResult : ValidationResult
    {
        private Exception? _exception;
        private const string TokenSource = "Microsoft.IdentityModel.Tokens";

        /// <summary>
        /// Creates an instance of <see cref="AlgorithmValidationResult"/>.
        /// </summary>
        /// <paramref name="algorithm"/>The algorithm to be validated.
        public AlgorithmValidationResult(string? algorithm)
            : base(ValidationFailureType.ValidationSucceeded)
        {
            Algorithm = algorithm;
            IsValid = true;
        }

        /// <summary>
        /// Creates an instance of <see cref=" AlgorithmValidationResult"/>
        /// </summary>
        /// <paramref name="algorithm"/>The algorithm to be validated.
        /// <paramref name="validationFailure"/> is the <see cref="ValidationFailureType"/> that occurred during validation.
        /// <paramref name="exceptionDetail"/> is the <see cref="ExceptionDetail"/> that occurred during validation.
        public AlgorithmValidationResult(string? algorithm, ValidationFailureType validationFailure, ExceptionDetail exceptionDetail)
            : base(validationFailure, exceptionDetail)
        {
            Algorithm = algorithm;
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
                if (_exception is SecurityTokenInvalidAlgorithmException securityTokenInvalidAlgorithmException)
                {
                    securityTokenInvalidAlgorithmException.InvalidAlgorithm = Algorithm;
                    securityTokenInvalidAlgorithmException.Source = TokenSource;
                }

                return _exception;
            }
        }

        /// <summary>
        /// Gets the algorithm used to sign the token.
        /// </summary>
        public string? Algorithm { get; }

    }
}
#nullable restore
