// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

#nullable enable
namespace Microsoft.IdentityModel.Tokens.Experimental
{
    /// <summary>
    /// Represents an error that occurred during a <see cref="SecurityToken"/> validation.
    /// </summary>
    public class ValidationError
    {
        /// <summary>
        /// Creates an instance of <see cref="ValidationError"/>.
        /// </summary>
        /// <param name="messageDetail" />Information about the error. Can be used to provide details for error messages.
        /// <param name="validationFailure"/>The <see cref="ValidationFailureType"/> that occurred.
        /// <param name="stackFrame"/>The stack frame where the error occurred.
        protected internal ValidationError(
            MessageDetail messageDetail,
            ValidationFailureType validationFailure,
            StackFrame stackFrame)
            : this(messageDetail,
                  validationFailure,
                  stackFrame,
                  null)
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="ValidationError"/>.
        /// </summary>
        /// <param name="messageDetail" />Information about the error. Can be used to provide details for error messages.
        /// <param name="validationFailure"/>The <see cref="ValidationFailureType"/> that occurred.
        /// <param name="stackFrame"/>The stack frame where the error occurred.
        /// <param name="innerException"/>An exception that occurred during validation.
        protected internal ValidationError(
            MessageDetail messageDetail,
            ValidationFailureType validationFailure,
            StackFrame stackFrame,
            Exception? innerException)
        {
            InnerException = innerException;
            MessageDetail = messageDetail;
            FailureType = validationFailure;
            StackFrames = new List<StackFrame>(4)
            {
                stackFrame
            };
        }

        /// <summary>
        /// Creates and returns instance of an <see cref="Exception"/> using <see cref="FailureType"/>
        /// </summary>
        /// <returns>An instance of an Exception.</returns>
        public virtual Exception GetException()
        {
            if (Exception is not null)
                return Exception;

            if (FailureType == ValidationFailureType.NullArgument)
                Exception = new ArgumentNullException(MessageDetail.Message, InnerException);
            else if (FailureType == ValidationFailureType.TokenDecryptionFailed)
                Exception = new SecurityTokenDecryptionFailedException(MessageDetail.Message, this, InnerException);
            else if (FailureType == ValidationFailureType.KeyWrapFailed)
                Exception = new SecurityTokenKeyWrapException(MessageDetail.Message, this, InnerException);
            else
                Exception = new SecurityTokenValidationException(MessageDetail.Message, this, InnerException);

            return Exception;
        }

        /// <summary>
        /// Gets or sets the exception associated with the <see cref="ValidationError"/>.
        /// </summary>
#pragma warning disable CA1721 // Property names should not match get methods
        protected Exception? Exception { get; set; }
#pragma warning restore CA1721 // Property names should not match get methods

        /// <summary>
        /// Creates a new instance of <see cref="ValidationError"/> representing a null parameter.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="stackFrame">The stack frame where the error occurred.</param>
        /// <returns>A new <see cref="ValidationError"/>.</returns>
        public static ValidationError NullParameter(string parameterName, StackFrame stackFrame) => new(
            MessageDetail.NullParameter(parameterName),
            ValidationFailureType.NullArgument,
            stackFrame,
            null);

        /// <summary>
        /// Gets the type of validation failure that occurred.
        /// </summary>
        public ValidationFailureType FailureType { get; }

        /// <summary>
        /// Gets the inner exception that occurred.
        /// </summary>
        public Exception? InnerException { get; }

        /// <summary>
        /// Gets the message that contains details of the error.
        /// </summary>
        public string Message => MessageDetail.Message;

        /// <summary>
        /// Gets the message which contains information about the error. Can be used to provide details for error messages.
        /// </summary>
        internal MessageDetail MessageDetail { get; }

        /// <summary>
        /// Gets the stack frames where the exception occurred.
        /// </summary>
        public IList<StackFrame> StackFrames { get; }

        /// <summary>
        /// Adds a stack frame to the list of stack frames and returns the updated object.
        /// </summary>
        /// <param name="stackFrame">The <see cref="StackFrame"/> to be added.</param>
        /// <returns>The updated <see cref="ValidationError"/> instance.</returns>
        public ValidationError AddStackFrame(StackFrame stackFrame)
        {
            StackFrames.Add(stackFrame);
            return this;
        }

        /// <summary>
        /// Adds the current stack frame to the list of stack frames and returns the updated object.
        /// If there is no cache entry for the given file path and line number, a new stack frame is created and added to the cache.
        /// </summary>
        /// <param name="filePath">The path to the file from which this method is called. Captured automatically by default.</param>
        /// <param name="lineNumber">The line number from which this method is called. CAptured automatically by default.</param>
        /// <param name="skipFrames">The number of stack frames to skip when capturing. Used to avoid capturing this method and get the caller instead.</param>
        /// <returns>The updated object.</returns>
        public ValidationError AddCurrentStackFrame([CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0, int skipFrames = 1)
        {
            // We add 1 to the skipped frames to skip the current method
            StackFrames.Add(GetCurrentStackFrame(filePath, lineNumber, skipFrames + 1));
            return this;
        }

        /// <summary>
        /// Returns the stack frame corresponding to the file path and line number from which this method is called.
        /// If there is no cache entry for the given file path and line number, a new stack frame is created and added to the cache.
        /// </summary>
        /// <param name="filePath">The path to the file from which this method is called. Captured automatically by default.</param>
        /// <param name="lineNumber">The line number from which this method is called. CAptured automatically by default.</param>
        /// <param name="skipFrames">The number of stack frames to skip when capturing. Used to avoid capturing this method and get the caller instead.</param>
        /// <returns>The captured stack frame.</returns>
        /// <remarks>If this is called from a helper method, consider adding an extra skip frame to avoid capturing the helper instead.</remarks>
        public static StackFrame GetCurrentStackFrame(
            [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0, int skipFrames = 1)
        {
            // String is allocated, but it goes out of scope immediately after the call
            string key = filePath + lineNumber;

            return CachedStackFrames.GetOrAdd(
                key,
                // Need to skip the call to the delegate + GetOrAdd when creating the frame
                _ => new StackFrame(skipFrames + 2, true));
        }

        // ConcurrentDictionary is thread-safe and only locks when adding a new item.
        private static ConcurrentDictionary<string, StackFrame> CachedStackFrames { get; } = new();
    }
}
#nullable restore
