// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics;

#nullable enable
namespace Microsoft.IdentityModel.Tokens.Experimental
{
    /// <summary>
    /// Represents a validation error when the <see cref="SecurityToken.Issuer"/> is not valid.
    /// If available, the invalid issuer is stored in <see cref="InvalidIssuer"/>.
    /// </summary>
    public class IssuerValidationError : ValidationError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IssuerValidationError"/> class.
        /// </summary>
        /// <param name="messageDetail" />Information about the error. Can be used to provide details for error messages.
        /// <param name="validationFailure"/>The <see cref="ValidationFailureType"/> that occurred.
        /// <param name="stackFrame"/>The stack frame where the exception occurred.
        /// <param name="invalidIssuer"/>The issuer that was not valid. Can be null if the issuer is missing from the token.
        public IssuerValidationError(
            MessageDetail messageDetail,
            ValidationFailureType validationFailure,
            StackFrame stackFrame,
            string? invalidIssuer)
            : this(messageDetail, validationFailure, stackFrame, invalidIssuer, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="IssuerValidationError"/> class.
        /// </summary>
        /// <param name="messageDetail" />Information about the error. Can be used to provide details for error messages.
        /// <param name="validationFailure"/>The <see cref="ValidationFailureType"/> that occurred.
        /// <param name="stackFrame"/>The stack frame where the exception occurred.
        /// <param name="invalidIssuer"/>The issuer that was not valid. Can be null if the issuer is missing from the token.
        /// <param name="innerException"/>If present, represents the exception that occurred during validation.
        public IssuerValidationError(
            MessageDetail messageDetail,
            ValidationFailureType validationFailure,
            StackFrame stackFrame,
            string? invalidIssuer,
            Exception? innerException)
            : base(messageDetail,
                  validationFailure,
                  stackFrame,
                  innerException)
        {
            InvalidIssuer = invalidIssuer;
        }

        /// <summary>
        /// The issuer that could not be validated.
        /// </summary>
        public string? InvalidIssuer { get; }

        /// <summary>
        /// Creates an instance of an <see cref="SecurityTokenInvalidIssuerException"/> using <see cref="ValidationError"/>
        /// </summary>
        /// <returns>An instance of an exception.</returns>
        public override Exception GetException()
        {
            if (Exception != null)
                return Exception;

            if (FailureType == IssuerValidationFailure.NoIssuerInToken
                || FailureType == IssuerValidationFailure.ValidationFailed
                || FailureType == IssuerValidationFailure.ValidatorThrew
                || FailureType == IssuerValidationFailure.NoValidationParameterIssuersProvided)
            {
                Exception = new SecurityTokenInvalidIssuerException(MessageDetail.Message, this, InnerException);
                return Exception;
            }

            return base.GetException();
        }
    }
}
#nullable restore
