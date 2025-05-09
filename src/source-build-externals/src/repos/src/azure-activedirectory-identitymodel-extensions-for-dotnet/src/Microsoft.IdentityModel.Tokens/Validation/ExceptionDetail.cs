// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Contains information so that Exceptions can be logged or thrown written as required.
    /// </summary>
    internal class ExceptionDetail
    {
        /// <summary>
        /// Creates an instance of <see cref="ExceptionDetail"/>
        /// </summary>
        /// <paramref name="messageDetail"/> contains information about the exception that is used to generate the exception message.
        /// <paramref name="exceptionType"/> is the type of exception that occurred.
        /// <paramref name="stackFrame"/> contains information about the stack frame where the exception occurred.
        public ExceptionDetail(MessageDetail messageDetail, Type exceptionType, StackFrame stackFrame)
            : this(messageDetail, exceptionType, stackFrame, null)
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="ExceptionDetail"/>
        /// </summary>
        /// <paramref name="messageDetail"/> contains information about the exception that is used to generate the exception message.
        /// <paramref name="exceptionType"/> is the type of exception that occurred.
        /// <paramref name="stackFrame"/> contains information about the stack frame where the exception occurred.
        /// <paramref name="innerException"/> is the inner exception that occurred.
        public ExceptionDetail(MessageDetail messageDetail, Type exceptionType, StackFrame stackFrame, Exception innerException)
        {
            ExceptionType = exceptionType;
            InnerException = innerException;
            MessageDetail = messageDetail;
            StackFrames.Add(stackFrame);
        }

        /// <summary>
        /// Creates an instance of an <see cref="Exception"/> using <see cref="ExceptionDetail"/>
        /// </summary>
        /// <returns>An instantance of an Exception.</returns>
        public Exception GetException()
        {
            if (InnerException != null)
                return Activator.CreateInstance(ExceptionType, MessageDetail.Message, InnerException) as Exception;

            return Activator.CreateInstance(ExceptionType, MessageDetail.Message) as Exception;
        }

        /// <summary>
        /// Gets the type of exception that occurred.
        /// </summary>
        public Type ExceptionType { get; }

        /// <summary>
        /// Gets the inner exception that occurred.
        /// </summary>
        public Exception InnerException { get; }

        /// <summary>
        /// Gets the message details that are used to generate the exception message.
        /// </summary>
        public MessageDetail MessageDetail { get; }

        /// <summary>
        /// Gets the stack frames where the exception occurred.
        /// </summary>
        public IList<StackFrame> StackFrames { get; } = [];
    }
}
