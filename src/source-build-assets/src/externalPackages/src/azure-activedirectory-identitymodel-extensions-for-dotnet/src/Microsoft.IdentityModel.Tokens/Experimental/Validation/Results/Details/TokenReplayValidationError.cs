// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics;

#nullable enable
namespace Microsoft.IdentityModel.Tokens.Experimental
{
    /// <summary>
    /// Represents a validation error when a <see cref="SecurityToken"/> replay is detected.
    /// If available, the expiration time of the <see cref="SecurityToken"/> that failed the validation is included.
    /// </summary>
    public class TokenReplayValidationError : ValidationError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TokenReplayValidationError"/> class.
        /// </summary>
        /// <param name="messageDetail" />Information about the error. Can be used to provide details for error messages.
        /// <param name="validationFailure"/>The <see cref="ValidationFailureType"/> that occurred.
        /// <param name="stackFrame"/>The stack frame where the exception occurred.
        /// <param name="expirationTime"/>The <see cref="DateTime"/> of the <see cref="SecurityToken"/> that failed the validation. Can be null if the token does not have an expiration time.
        public TokenReplayValidationError(
            MessageDetail messageDetail,
            ValidationFailureType validationFailure,
            StackFrame stackFrame,
            DateTime? expirationTime)
            : this(messageDetail,
                  validationFailure,
                  stackFrame,
                  expirationTime,
                  null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenReplayValidationError"/> class.
        /// </summary>
        /// <param name="messageDetail" />Information about the error. Can be used to provide details for error messages.
        /// <param name="validationFailure"/>The <see cref="ValidationFailureType"/> that occurred.
        /// <param name="stackFrame"/>The stack frame where the exception occurred.
        /// <param name="expirationTime"/>The <see cref="DateTime"/> of the <see cref="SecurityToken"/> that failed the validation. Can be null if the token does not have an expiration time.
        /// <param name="innerException"/>If present, represents the exception that occurred during validation.
        public TokenReplayValidationError(
            MessageDetail messageDetail,
            ValidationFailureType validationFailure,
            StackFrame stackFrame,
            DateTime? expirationTime,
            Exception? innerException)
            : base(messageDetail,
                  validationFailure,
                  stackFrame,
                  innerException)
        {
            ExpirationTime = expirationTime;
        }

        /// <summary>
        /// Creates an instance of an <see cref="SecurityTokenReplayDetectedException"/>.
        /// </summary>
        /// <returns>An instance of an exception.</returns>
        public override Exception GetException()
        {
            if (Exception != null)
                return Exception;

            if (FailureType == TokenReplayValidationFailure.ValidationFailed
                || FailureType == TokenReplayValidationFailure.ValidatorThrew
                || FailureType == TokenReplayValidationFailure.NoExpiration
                || FailureType == TokenReplayValidationFailure.TokenFoundInCache
                || FailureType == TokenReplayValidationFailure.AddToCacheFailed)
            {
                Exception = new SecurityTokenReplayDetectedException(MessageDetail.Message, this, InnerException);
                return Exception;
            }

            return base.GetException();
        }

        /// <summary>
        /// The expiration time of the token that failed the validation.
        /// </summary>
        public DateTime? ExpirationTime { get; }
    }
}
#nullable restore
