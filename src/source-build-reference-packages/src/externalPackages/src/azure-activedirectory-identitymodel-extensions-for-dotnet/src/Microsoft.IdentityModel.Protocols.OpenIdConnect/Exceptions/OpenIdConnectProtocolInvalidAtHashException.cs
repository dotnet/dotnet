// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect
{
    /// <summary>
    /// This exception is thrown when an OpenIdConnect protocol handler encounters an invalid at_hash.
    /// </summary>
    [Serializable]
    public class OpenIdConnectProtocolInvalidAtHashException : OpenIdConnectProtocolException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenIdConnectProtocolInvalidAtHashException"/> class.
        /// </summary>
        public OpenIdConnectProtocolInvalidAtHashException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenIdConnectProtocolInvalidAtHashException"/> class.
        /// </summary>
        /// <param name="message">Addtional information to be included in the exception and displayed to user.</param>
        public OpenIdConnectProtocolInvalidAtHashException(String message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenIdConnectProtocolInvalidAtHashException"/> class.
        /// </summary>
        /// <param name="message">Addtional information to be included in the exception and displayed to user.</param>
        /// <param name="innerException">A <see cref="Exception"/> that represents the root cause of the exception.</param>
        public OpenIdConnectProtocolInvalidAtHashException(String message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenIdConnectProtocolInvalidAtHashException"/> class.
        /// </summary>
        /// <param name="info">the <see cref="SerializationInfo"/> that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected OpenIdConnectProtocolInvalidAtHashException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
