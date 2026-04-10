// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Represents a security token exception when decryption failed.
    /// </summary>
    [Serializable]
    public class SecurityTokenDecryptionFailedException : SecurityTokenException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenDecryptionFailedException"/> class.
        /// </summary>
        public SecurityTokenDecryptionFailedException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenDecryptionFailedException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public SecurityTokenDecryptionFailedException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenDecryptionFailedException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The <see cref="Exception"/> that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public SecurityTokenDecryptionFailedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenDecryptionFailedException"/> class.
        /// </summary>
        /// <param name="info">the <see cref="SerializationInfo"/> that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected SecurityTokenDecryptionFailedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
