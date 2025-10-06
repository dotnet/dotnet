// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
#nullable enable
namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Contains the result of validating the TokenType of a <see cref="SecurityToken"/>.
    /// The <see cref="TokenValidationResult"/> contains a collection of <see cref="ValidationResult"/> for each step in the token validation.
    /// </summary>
    internal class TokenTypeValidationResult : ValidationResult
    {
        private Exception? _exception;
        private const string TokenSource = "Microsoft.IdentityModel.Tokens";

        /// <summary>
        /// Creates an instance of <see cref="TokenTypeValidationResult"/>.
        /// </summary>
        /// <paramref name="type"/> is the type against which the token was validated.
        public TokenTypeValidationResult(string? type)
            : base(ValidationFailureType.ValidationSucceeded)
        {
            Type = type;
            IsValid = true;
        }

        /// <summary>
        /// Creates an instance of <see cref="TokenTypeValidationResult"/>
        /// </summary>
        /// <paramref name="type"/> is the type against which the token was validated.
        /// <paramref name="validationFailure"/> is the <see cref="ValidationFailureType"/> that occurred during validation.
        /// <paramref name="exceptionDetail"/> is the <see cref="ExceptionDetail"/> that occurred during validation.
        public TokenTypeValidationResult(string? type, ValidationFailureType validationFailure, ExceptionDetail exceptionDetail)
            : base(validationFailure, exceptionDetail)
        {
            Type = type;
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
                if (_exception is SecurityTokenInvalidTypeException securityTokenInvalidTypeException)
                {
                    securityTokenInvalidTypeException.InvalidType = Type;
                    securityTokenInvalidTypeException.Source = TokenSource;
                }

                return _exception;
            }
        }

        /// <summary>
        /// Gets the security token type.
        /// </summary>
        public string? Type { get; }

    }
}
#nullable restore
