// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Contains results of a single step in validating a <see cref="SecurityToken"/>.
    /// A <see cref="TokenValidationResult"/> maintains a list of <see cref="ValidationResult"/> for each step in the token validation.
    /// </summary>
    internal abstract class ValidationResult
    {
        private bool _isValid = false;

        /// <summary>
        /// Creates an instance of <see cref="ValidationResult"/>
        /// </summary>
        protected ValidationResult()
        {
            ValidationFailureType = ValidationFailureType.ValidationNotEvaluated;
        }

        /// <summary>
        /// Creates an instance of <see cref="ValidationResult"/>
        /// </summary>
        /// <param name="validationFailureType">The <see cref="ValidationFailureType"/> that occurred during validation.</param>
        protected ValidationResult(ValidationFailureType validationFailureType)
        {
            ValidationFailureType = validationFailureType;
        }

        /// <summary>
        /// Creates an instance of <see cref="ValidationResult"/>
        /// </summary>
        /// <param name="validationFailureType">The <see cref="ValidationFailureType"/> that occurred during validation.</param>
        /// <param name="exceptionDetail"> The <see cref="ExceptionDetail"/> representing the <see cref="Exception"/> that occurred during validation.</param>
        protected ValidationResult(ValidationFailureType validationFailureType, ExceptionDetail exceptionDetail)
        {
            ValidationFailureType = validationFailureType;
            ExceptionDetail = exceptionDetail;
        }

        /// <summary>
        /// Adds a new stack frame to the exception details.
        /// </summary>
        /// <param name="stackFrame"></param>
        public void AddStackFrame(StackFrame stackFrame)
        {
            ExceptionDetail.StackFrames.Add(stackFrame);
        }

        /// <summary>
        /// Gets the <see cref="Exception"/> that occurred during validation.
        /// </summary>
        public abstract Exception Exception { get; }

        /// <summary>
        /// Gets the <see cref="ExceptionDetail"/> that occurred during validation.
        /// </summary>
        public ExceptionDetail ExceptionDetail { get; }

        /// <summary>
        /// True if the token was successfully validated, false otherwise.
        /// </summary>
        public bool IsValid
        {
            get
            {
                HasValidOrExceptionWasRead = true;
                return _isValid;
            }
            set
            {
                _isValid = value;
            }
        }

        // TODO - HasValidOrExceptionWasRead, IsValid, Exception are temporary and will be removed when TokenValidationResult derives from ValidationResult.
        /// <summary>
        /// Gets or sets a boolean recording if IsValid or Exception was called.
        /// </summary>
        protected bool HasValidOrExceptionWasRead { get; set; }

        /// <summary>
        /// Logs the validation result.
        /// </summary>
#pragma warning disable CA1822 // Mark members as static
        public void Log()
#pragma warning restore CA1822 // Mark members as static
        {
            // TODO - Do we need this, how will it work?
        }

        /// <summary>
        /// Contains any logs that would have been written.
        /// </summary>
        public IList<LogDetail> LogDetails { get; } = new List<LogDetail>();

        /// <summary>
        /// Gets the <see cref="ValidationFailureType"/> indicating why the validation was not satisfied.
        /// </summary>
        public ValidationFailureType ValidationFailureType
        {
            get;
        } = ValidationFailureType.ValidationNotEvaluated;
    }
}
