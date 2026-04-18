// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

#nullable enable
namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Contains the result of reading a <see cref="SecurityToken"/>.
    /// The <see cref="TokenValidationResult"/> contains a collection of <see cref="ValidationResult"/> for each step in the token validation.
    /// </summary>
    internal class TokenReadingResult : ValidationResult
    {
        private Exception? _exception;
        private SecurityToken? _securityToken;

        /// <summary>
        /// Creates an instance of <see cref="TokenReadingResult"/>.
        /// </summary>
        /// <paramref name="tokenInput"/> is the string from which the <see cref="SecurityToken"/> was created.
        /// <paramref name="securityToken"/> is the <see cref="SecurityToken"/> that was created.
        public TokenReadingResult(SecurityToken securityToken, string tokenInput)
            : base(ValidationFailureType.ValidationSucceeded)
        {
            IsValid = true;
            TokenInput = tokenInput;
            _securityToken = securityToken;
        }

        /// <summary>
        /// Creates an instance of <see cref="TokenReadingResult"/>
        /// </summary>
        /// <paramref name="tokenInput"/> is the string that failed to create a <see cref="SecurityToken"/>.
        /// <paramref name="validationFailure"/> is the <see cref="ValidationFailureType"/> that occurred during reading.
        /// <paramref name="exceptionDetail"/> is the <see cref="ExceptionDetail"/> that occurred during reading.
        public TokenReadingResult(string? tokenInput, ValidationFailureType validationFailure, ExceptionDetail exceptionDetail)
            : base(validationFailure, exceptionDetail)
        {
            TokenInput = tokenInput;
            IsValid = false;
        }

        /// <summary>
        /// Gets the <see cref="SecurityToken"/> that was read.
        /// </summary>
        /// <exception cref="InvalidOperationException"/> if the <see cref="SecurityToken"/> is null.
        /// <remarks>It is expected that the caller would check <see cref="ValidationResult.IsValid"/> returns true before accessing this.</remarks>
        public SecurityToken SecurityToken()
        {
            if (_securityToken is null)
                throw new InvalidOperationException("Attempted to retrieve the SecurityToken from a failed TokenReading result.");

            return _securityToken;
        }

        /// <summary>
        /// Gets the <see cref="Exception"/> that occurred during reading.
        /// </summary>
        public override Exception? Exception
        {
            get
            {
                if (_exception != null || ExceptionDetail == null)
                    return _exception;

                HasValidOrExceptionWasRead = true;
                _exception = ExceptionDetail.GetException();

                if (_exception is SecurityTokenException securityTokenException)
                {
                    securityTokenException.Source = "Microsoft.IdentityModel.Tokens";
                    securityTokenException.ExceptionDetail = ExceptionDetail;
                }

                return _exception;
            }
        }

        /// <summary>
        /// Gets the string from which the <see cref="SecurityToken"/> was read.
        /// </summary>
        public string? TokenInput { get; }
    }
}
#nullable restore
