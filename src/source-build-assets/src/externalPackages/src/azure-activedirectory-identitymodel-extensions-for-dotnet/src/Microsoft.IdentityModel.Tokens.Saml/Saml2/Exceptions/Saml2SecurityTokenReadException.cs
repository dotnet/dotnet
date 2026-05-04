// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
    /// <summary>
    /// This exception is thrown when reading a <see cref="Saml2SecurityToken"/>.
    /// </summary>
    [Serializable]
    public class Saml2SecurityTokenReadException : Saml2SecurityTokenException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2SecurityTokenReadException"/> class.
        /// </summary>
        public Saml2SecurityTokenReadException()
            : base()
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2SecurityTokenReadException"/> class.
        /// </summary>
        /// <param name="message">Addtional information to be included in the exception and displayed to user.</param>
        public Saml2SecurityTokenReadException(string message)
            : base(message)
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2SecurityTokenReadException"/> class.
        /// </summary>
        /// <param name="message">Addtional information to be included in the exception and displayed to user.</param>
        /// <param name="innerException">A <see cref="Exception"/> that represents the root cause of the exception.</param>
        public Saml2SecurityTokenReadException(string message, Exception innerException)
            : base(message, innerException)
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2SecurityTokenReadException"/> class.
        /// </summary>
        /// <param name="info">the <see cref="SerializationInfo"/> that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected Saml2SecurityTokenReadException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {}
    }
}
