// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Represents a security token exception.
    /// </summary>
    [Serializable]
    public class SecurityTokenException : Exception
    {
        [NonSerialized]
        private string _stackTrace;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenException"/> class.
        /// </summary>
        public SecurityTokenException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public SecurityTokenException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The <see cref="Exception"/> that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public SecurityTokenException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenException"/> class.
        /// </summary>
        /// <param name="info">the <see cref="SerializationInfo"/> that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
#if NET8_0_OR_GREATER
        [Obsolete("Formatter-based serialization is obsolete", DiagnosticId = "SYSLIB0051")] 
#endif
        protected SecurityTokenException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets the stack trace that is captured when the exception is created.
        /// </summary>
        public override string StackTrace
        {
            get
            {
                if (_stackTrace == null)
                {
                    if (ExceptionDetail == null)
                        return base.StackTrace;
#if NET8_0_OR_GREATER
                    _stackTrace = new StackTrace(ExceptionDetail.StackFrames).ToString();
#else
                    StringBuilder sb = new();
                    foreach (StackFrame frame in ExceptionDetail.StackFrames)
                    {
                        sb.Append(frame.ToString());
                        sb.Append(Environment.NewLine);
                    }

                    _stackTrace = sb.ToString();
#endif
                }

                return _stackTrace;
            }
        }

        /// <summary>
        /// Gets or sets the source of the exception.
        /// </summary>
        public override string Source
        {
            get => base.Source;
            set => base.Source = value;
        }

        internal ExceptionDetail ExceptionDetail
        {
            get; set;
        }

#if NET472 || NETSTANDARD2_0 || NET6_0_OR_GREATER
        /// <summary>
        /// When overridden in a derived class, sets the System.Runtime.Serialization.SerializationInfo
        /// with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="ArgumentNullException">thrown if <paramref name="info"/> is null.</exception>
#if NET8_0_OR_GREATER
        [Obsolete("Formatter-based serialization is obsolete", DiagnosticId = "SYSLIB0051")]
#endif
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw LogHelper.LogArgumentNullException(nameof(info));

            base.GetObjectData(info, context);
        }
#endif
    }
}
