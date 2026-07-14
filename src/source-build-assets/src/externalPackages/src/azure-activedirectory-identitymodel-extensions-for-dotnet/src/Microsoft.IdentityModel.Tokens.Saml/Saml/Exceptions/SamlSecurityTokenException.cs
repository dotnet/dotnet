// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens.Saml
{
    /// <summary>
    /// This exception is thrown when a security is missing an ExpirationTime.
    /// </summary>
    [Serializable]
    public class SamlSecurityTokenException : SecurityTokenException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SamlSecurityTokenException"/> class.
        /// </summary>
        public SamlSecurityTokenException()
            : base()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SamlSecurityTokenException"/> class.
        /// </summary>
        /// <param name="message">Addtional information to be included in the exception and displayed to user.</param>
        public SamlSecurityTokenException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SamlSecurityTokenException"/> class.
        /// </summary>
        /// <param name="message">Addtional information to be included in the exception and displayed to user.</param>
        /// <param name="innerException">A <see cref="Exception"/> that represents the root cause of the exception.</param>
        public SamlSecurityTokenException(string message, Exception innerException)
            : base(message, innerException)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SamlSecurityTokenException"/> class.
        /// </summary>
        /// <param name="info">the <see cref="SerializationInfo"/> that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected SamlSecurityTokenException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
