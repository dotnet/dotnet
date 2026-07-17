// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics;

#nullable enable
namespace Microsoft.IdentityModel.Tokens.Experimental
{
    /// <summary>
    /// Represents a validation error when the <see cref="SecurityToken"/> type is not valid.
    /// If available, the invalid token type is stored in <see cref="InvalidTokenType"/>.
    /// </summary>
    public class TokenTypeValidationError : ValidationError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TokenTypeValidationError"/> class.
        /// </summary>
        /// <param name="messageDetail" />Information about the error. Can be used to provide details for error messages.
        /// <param name="validationFailure"/>The <see cref="ValidationFailureType"/> that occurred.
        /// <param name="stackFrame"/>The stack frame where the exception occurred.
        /// <param name="invalidTokenType"/>The token type that was not valid. Can be null if the token type is missing from the token.
        public TokenTypeValidationError(
            MessageDetail messageDetail,
            ValidationFailureType validationFailure,
            StackFrame stackFrame,
            string? invalidTokenType)
            : this(messageDetail,
                  validationFailure,
                  stackFrame,
                  invalidTokenType,
                  null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenTypeValidationError"/> class.
        /// </summary>
        /// <param name="messageDetail" />Information about the error. Can be used to provide details for error messages.
        /// <param name="validationFailure"/>The <see cref="ValidationFailureType"/> that occurred.
        /// <param name="stackFrame"/>The stack frame where the exception occurred.
        /// <param name="invalidTokenType"/>The token type that was not valid. Can be null if the token type is missing from the token.
        /// <param name="innerException"/>If present, represents the exception that occurred during validation.
        public TokenTypeValidationError(
            MessageDetail messageDetail,
            ValidationFailureType validationFailure,
            StackFrame stackFrame,
            string? invalidTokenType,
            Exception? innerException)
            : base(messageDetail, validationFailure, stackFrame, innerException)
        {
            InvalidTokenType = invalidTokenType;
        }

        /// <summary>
        /// Creates an instance of a <see cref="SecurityTokenInvalidTypeException"/>.
        /// </summary>
        /// <returns>An instance of an exception.</returns>
        public override Exception GetException()
        {
            if (Exception != null)
                return Exception;

            if (FailureType == TokenTypeValidationFailure.ValidationFailed
                || FailureType == TokenTypeValidationFailure.ValidatorThrew)
            {
                Exception = new SecurityTokenInvalidTypeException(MessageDetail.Message, this, InnerException)
                {
                    InvalidType = InvalidTokenType
                };

                return Exception;
            }

            return base.GetException();
        }

        /// <summary>
        /// The token type that could not be validated.
        /// </summary>
        public string? InvalidTokenType { get; }
    }
}
#nullable restore
