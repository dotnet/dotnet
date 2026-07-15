// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics;

#nullable enable
namespace Microsoft.IdentityModel.Tokens.Experimental
{
    /// <summary>
    /// Represents a validation error when the <see cref="SecurityToken"/> signature is not valid.
    /// </summary>
    public class SignatureValidationError : ValidationError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SignatureValidationError"/> class.
        /// </summary>
        /// <param name="messageDetail" />Information about the error. Can be used to provide details for error messages.
        /// <param name="validationFailure"/>The <see cref="ValidationFailureType"/> that occurred.
        /// <param name="stackFrame"/>The stack frame where the exception occurred.
        public SignatureValidationError(
            MessageDetail messageDetail,
            ValidationFailureType validationFailure,
            StackFrame stackFrame)
            : this(messageDetail, validationFailure, stackFrame, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignatureValidationError"/> class.
        /// </summary>
        /// <param name="messageDetail" />Information about the error. Can be used to provide details for error messages.
        /// <param name="validationFailure"/>The <see cref="ValidationFailureType"/> that occurred.
        /// <param name="stackFrame"/>The stack frame where the exception occurred.
        /// <param name="innerException"/>If present, represents the exception that occurred during validation.
        public SignatureValidationError(
            MessageDetail messageDetail,
            ValidationFailureType validationFailure,
            StackFrame stackFrame,
            Exception? innerException) :
            base(messageDetail,
                validationFailure,
                stackFrame,
                innerException)
        {
        }

        /// <summary>
        /// <see cref="SecurityTokenInvalidSignatureException"/> or <see cref="SecurityTokenSignatureKeyNotFoundException"/>.
        /// </summary>
        /// <returns>An instance of an Exception.</returns>
        public override Exception GetException()
        {
            if (Exception is not null)
                return Exception;

            if (FailureType == SignatureValidationFailure.ValidationFailed
                || FailureType == SignatureValidationFailure.TokenIsNotSigned
                || FailureType == SignatureValidationFailure.ReferenceDigestValidationFailed
                || FailureType == SignatureValidationFailure.ValidatorThrew)
            {
                Exception = new SecurityTokenInvalidSignatureException(MessageDetail.Message, this, InnerException);
                return Exception;
            }
            else if (FailureType == SignatureValidationFailure.SigningKeyNotFound)
            {
                Exception = new SecurityTokenSignatureKeyNotFoundException(MessageDetail.Message, this, InnerException);
                return Exception;
            }

            return base.GetException();
        }
    }
}
#nullable restore
