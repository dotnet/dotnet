// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// This exception is thrown when a security token contained a key identifier but the key was not found by the runtime 
    /// when decrypting a token.
    /// </summary>
    [Serializable]
    public class SecurityTokenEncryptionKeyNotFoundException : SecurityTokenDecryptionFailedException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenEncryptionKeyNotFoundException"/> class.
        /// </summary>
        public SecurityTokenEncryptionKeyNotFoundException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenEncryptionKeyNotFoundException"/> class.
        /// </summary>
        /// <param name="message">Addtional information to be included in the exception and displayed to user.</param>
        public SecurityTokenEncryptionKeyNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenEncryptionKeyNotFoundException"/> class.
        /// </summary>
        /// <param name="message">Addtional information to be included in the exception and displayed to user.</param>
        /// <param name="innerException">A <see cref="Exception"/> that represents the root cause of the exception.</param>
        public SecurityTokenEncryptionKeyNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenEncryptionKeyNotFoundException"/> class.
        /// </summary>
        /// <param name="info">the <see cref="SerializationInfo"/> that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected SecurityTokenEncryptionKeyNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
