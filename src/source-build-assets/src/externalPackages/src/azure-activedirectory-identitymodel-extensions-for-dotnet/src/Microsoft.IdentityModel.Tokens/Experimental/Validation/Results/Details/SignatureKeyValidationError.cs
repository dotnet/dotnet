// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Diagnostics;
using System;

#nullable enable
namespace Microsoft.IdentityModel.Tokens.Experimental
{
    /// <summary>
    /// Represents a validation error when the <see cref="SecurityToken.SecurityKey"/> that signed the <see cref="SecurityToken"/>is not valid.
    /// If available, the invalid signing key is stored in <see cref="InvalidSigningKey"/>.
    /// </summary>
    public class SignatureKeyValidationError : ValidationError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SignatureKeyValidationError"/> class.
        /// </summary>
        /// <param name="messageDetail" />Information about the error. Can be used to provide details for error messages.
        /// <param name="validationFailure"/>The <see cref="ValidationFailureType"/> that occurred.
        /// <param name="stackFrame"/>The stack frame where the exception occurred.
        /// <param name="invalidSigningKey"/>The <see cref="SecurityKey"/> that was not valid. Can be null if <see cref="SecurityToken.SigningKey"/> is missing.
        public SignatureKeyValidationError(
            MessageDetail messageDetail,
            ValidationFailureType validationFailure,
            StackFrame stackFrame,
            SecurityKey? invalidSigningKey)
            : this(messageDetail,
                  validationFailure,
                  stackFrame,
                  invalidSigningKey,
                  null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignatureKeyValidationError"/> class.
        /// </summary>
        /// <param name="messageDetail" />Information about the error. Can be used to provide details for error messages.
        /// <param name="validationFailure"/>The <see cref="ValidationFailureType"/> that occurred.
        /// <param name="stackFrame"/>The stack frame where the exception occurred.
        /// <param name="invalidSigningKey"/>The <see cref="SecurityKey"/> that was not valid. Can be null if <see cref="SecurityToken.SigningKey"/> is missing.
        /// <param name="innerException"/>If present, represents the exception that occurred during validation.
        public SignatureKeyValidationError(
            MessageDetail messageDetail,
            ValidationFailureType validationFailure,
            StackFrame stackFrame,
            SecurityKey? invalidSigningKey,
            Exception? innerException)
            : base(messageDetail,
                  validationFailure,
                  stackFrame,
                  innerException)
        {
            InvalidSigningKey = invalidSigningKey;
        }

        /// <summary>
        /// Creates an instance of an <see cref="SecurityTokenInvalidSigningKeyException"/>.
        /// </summary>
        /// <returns>An instance of an Exception.</returns>
        public override Exception GetException()
        {
            if (Exception != null)
                return Exception;

            if (FailureType == SignatureKeyValidationFailure.ValidationFailed
                || FailureType == SignatureKeyValidationFailure.ValidatorThrew
                || FailureType == SignatureKeyValidationFailure.NotYetValid
                || FailureType == SignatureKeyValidationFailure.KeyExpired
                || FailureType == SignatureKeyValidationFailure.KeyIsNull)
            {
                Exception = new SecurityTokenInvalidSigningKeyException(MessageDetail.Message, this, InnerException)
                {
                    SigningKey = InvalidSigningKey
                };
            }

            return base.GetException();
        }

        /// <summary>
        /// The signing key that was found invalid.
        /// </summary>
        protected SecurityKey? InvalidSigningKey { get; }
    }
}
#nullable restore
