// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable enable
namespace Microsoft.IdentityModel.Tokens.Experimental
{
    /// <summary>
    /// Represents a validation error when the <see cref="SecurityToken"/> audience is not valid.
    /// If available, the audiences from the <see cref="SecurityToken"/> are stored in <see cref="TokenAudiences"/>.
    /// Valid audiences are stored in <see cref="ValidAudiences"/>.
    /// </summary>
    public class AudienceValidationError : ValidationError
    {
        /// <summary>
        /// Initializes a new instance of <see cref="AudienceValidationError"/>.
        /// </summary>
        /// <param name="messageDetail" />Information about the error. Can be used to provide details for error messages.
        /// <param name="validationFailure"/>The <see cref="ValidationFailureType"/> that occurred.
        /// <param name="stackFrame"/>The stack frame where the exception occurred.
        /// <param name="tokenAudiences"/>The audiences that were in the <see cref="SecurityToken"/>. Can be null if no audiences were found in the token.
        /// <param name="validAudiences"/>The audiences that were expected. Can be null if no valid audiences were provided in the validation parameters.
        public AudienceValidationError(
            MessageDetail messageDetail,
            ValidationFailureType validationFailure,
            StackFrame stackFrame,
            IList<string>? tokenAudiences,
            IList<string>? validAudiences)
            : this(messageDetail,
                  validationFailure,
                  stackFrame,
                  tokenAudiences,
                  validAudiences,
                  null)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="AudienceValidationError"/>.
        /// </summary>
        /// <param name="messageDetail" />Information about the error. Can be used to provide details for error messages.
        /// <param name="validationFailure"/>The <see cref="ValidationFailureType"/> that occurred.
        /// <param name="stackFrame"/>The stack frame where the exception occurred.
        /// <param name="tokenAudiences"/>The audiences that were in the <see cref="SecurityToken"/>. Can be null if no audiences were found in the token.
        /// <param name="validAudiences"/>The audiences that were expected. Can be null if no valid audiences were provided in the validation parameters.
        /// <param name="innerException"/>If present, represents the exception that occurred during validation.
        public AudienceValidationError(
            MessageDetail messageDetail,
            ValidationFailureType validationFailure,
            StackFrame stackFrame,
            IList<string>? tokenAudiences,
            IList<string>? validAudiences,
            Exception? innerException)
            : base(messageDetail, validationFailure, stackFrame, innerException)
        {
            TokenAudiences = tokenAudiences;
            ValidAudiences = validAudiences;
        }

        /// <summary>
        /// Creates an instance of an <see cref="SecurityTokenInvalidAudienceException"/>.
        /// </summary>
        /// <returns>An instance of an Exception.</returns>
        public override Exception GetException()
        {
            if (Exception != null)
                return Exception;

            if (FailureType == AudienceValidationFailure.NoAudienceInToken
                || FailureType == AudienceValidationFailure.NoValidationParameterAudiencesProvided
                || FailureType == AudienceValidationFailure.AudienceDidNotMatch
                || FailureType == AudienceValidationFailure.ValidatorThrew)
            {
                if (TokenAudiences == null)
                    Exception = new SecurityTokenInvalidAudienceException(MessageDetail.Message, this, InnerException);
                else
                    Exception = new SecurityTokenInvalidAudienceException(
                        MessageDetail.Message,
                        this,
                        InnerException)
                    {
                        InvalidAudience = Utility.SerializeAsSingleCommaDelimitedString(TokenAudiences)
                    };

                return Exception;
            }

            return base.GetException();
        }

        /// <summary>
        /// The audiences that were in the token.
        /// </summary>
        public IList<string>? TokenAudiences { get; }

        /// <summary>
        /// The audiences that were expected.
        /// </summary>
        public IList<string>? ValidAudiences { get; }
    }
}
#nullable restore
