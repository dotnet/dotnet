// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.Serialization;
using Microsoft.IdentityModel.Tokens.Experimental;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Represents a key wrap exception when encryption failed.
    /// </summary>
    [Serializable]
    public class SecurityTokenKeyWrapException : SecurityTokenException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenKeyWrapException"/> class.
        /// </summary>
        public SecurityTokenKeyWrapException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenKeyWrapException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public SecurityTokenKeyWrapException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenKeyWrapException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The <see cref="Exception"/> that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public SecurityTokenKeyWrapException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenKeyWrapException"/> class.
        /// </summary>
        /// <param name="info">the <see cref="SerializationInfo"/> that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected SecurityTokenKeyWrapException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #region Experimental
        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenKeyWrapException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">Additional information to be included in the exception and displayed to user.</param>
        /// <param name="validationError">The <see cref="ValidationError"/> that is associated with the exception.</param>
        internal SecurityTokenKeyWrapException(string message, ValidationError validationError)
            : base(message, validationError)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenKeyWrapException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">Additional information to be included in the exception and displayed to user.</param>
        /// <param name="validationError">The <see cref="ValidationError"/> that is associated with the exception.</param>
        /// <param name="innerException">An <see cref="Exception"/> that represents the root cause of the exception.</param>
        internal SecurityTokenKeyWrapException(string message, ValidationError validationError, Exception innerException)
            : base(message, validationError, innerException)
        {
        }
        #endregion
    }
}
