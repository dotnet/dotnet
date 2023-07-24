// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
    /// <summary>
    /// This exception is thrown when a security is missing an ExpirationTime.
    /// </summary>
    [Serializable]
    public class Saml2SecurityTokenException : SecurityTokenException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2SecurityTokenException"/> class.
        /// </summary>
        public Saml2SecurityTokenException()
            : base()
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2SecurityTokenException"/> class.
        /// </summary>
        /// <param name="message">Addtional information to be included in the exception and displayed to user.</param>
        public Saml2SecurityTokenException(string message)
            : base(message)
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2SecurityTokenException"/> class.
        /// </summary>
        /// <param name="message">Addtional information to be included in the exception and displayed to user.</param>
        /// <param name="innerException">A <see cref="Exception"/> that represents the root cause of the exception.</param>
        public Saml2SecurityTokenException(string message, Exception innerException)
            : base(message, innerException)
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2SecurityTokenException"/> class.
        /// </summary>
        /// <param name="info">the <see cref="SerializationInfo"/> that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected Saml2SecurityTokenException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {}
    }
}
