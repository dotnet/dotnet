// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics;

#nullable enable
namespace Microsoft.IdentityModel.Tokens.Experimental
{
    /// <summary>
    /// Represents a validation error when the <see cref="SecurityToken"/> algorithm is not valid.
    /// If available, the invalid algorithm is stored in <see cref="InvalidAlgorithm"/>.
    /// </summary>
    public class AlgorithmValidationError : ValidationError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AlgorithmValidationError"/> class.
        /// </summary>
        /// <param name="messageDetail" />Information about the error. Can be used to provide details for error messages.
        /// <param name="validationFailure"/>The <see cref="ValidationFailureType"/> that occurred.
        /// <param name="stackFrame"/>The stack frame where the exception occurred.
        /// <param name="invalidAlgorithm"/>The algorithm that could not be validated. Can be null if the algorithm is missing from the token.
        public AlgorithmValidationError(
            MessageDetail messageDetail,
            ValidationFailureType validationFailure,
            StackFrame stackFrame,
            string? invalidAlgorithm) :
            this(messageDetail,
                validationFailure,
                stackFrame,
                invalidAlgorithm,
                null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlgorithmValidationError"/> class.
        /// </summary>
        /// <param name="messageDetail" />Information about the error. Can be used to provide details for error messages.
        /// <param name="validationFailure"/>The <see cref="ValidationFailureType"/> that occurred.
        /// <param name="stackFrame"/>The stack frame where the exception occurred.
        /// <param name="invalidAlgorithm"/>The algorithm that could not be validated. Can be null if the algorithm is missing from the token.
        /// <param name="innerException"/>If present, represents the exception that occurred during validation.
        public AlgorithmValidationError(
            MessageDetail messageDetail,
            ValidationFailureType validationFailure,
            StackFrame stackFrame,
            string? invalidAlgorithm,
            Exception? innerException) :
            base(messageDetail,
                validationFailure,
                stackFrame,
                innerException)
        {
            InvalidAlgorithm = invalidAlgorithm;
        }

        /// <summary>
        /// Creates an instance of a derived <see cref="Exception"/> using <see cref="ValidationFailureType"/>
        /// </summary>
        /// <returns>An instance of an Exception.</returns>
        public override Exception GetException()
        {
            if (Exception != null)
                return Exception;

            if (FailureType == AlgorithmValidationFailure.ValidationFailed
             || FailureType == AlgorithmValidationFailure.AlgorithmIsNotSupported
             || FailureType == AlgorithmValidationFailure.ValidatorThrew)
            {
                Exception = new SecurityTokenInvalidAlgorithmException(
                    MessageDetail.Message,
                    this,
                    InnerException)
                {
                    InvalidAlgorithm = InvalidAlgorithm
                };

                return Exception;
            }

            return base.GetException();
        }

        /// <summary>
        /// The algorithm that could not be validated.
        /// </summary>
        public string? InvalidAlgorithm { get; }
    }
}
#nullable restore
