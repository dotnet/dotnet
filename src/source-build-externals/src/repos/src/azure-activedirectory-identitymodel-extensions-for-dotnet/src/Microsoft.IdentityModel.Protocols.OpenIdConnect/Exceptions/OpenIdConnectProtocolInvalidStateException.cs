// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect
{
    /// <summary>
    /// This exception is thrown when an OpenIdConnect protocol handler encounters an invalid state.
    /// </summary>
    [Serializable]
    public class OpenIdConnectProtocolInvalidStateException : OpenIdConnectProtocolException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenIdConnectProtocolInvalidStateException"/> class.
        /// </summary>
        public OpenIdConnectProtocolInvalidStateException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenIdConnectProtocolInvalidStateException"/> class.
        /// </summary>
        /// <param name="message">Addtional information to be included in the exception and displayed to user.</param>
        public OpenIdConnectProtocolInvalidStateException(String message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenIdConnectProtocolInvalidStateException"/> class.
        /// </summary>
        /// <param name="message">Addtional information to be included in the exception and displayed to user.</param>
        /// <param name="innerException">A <see cref="Exception"/> that represents the root cause of the exception.</param>
        public OpenIdConnectProtocolInvalidStateException(String message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenIdConnectProtocolInvalidStateException"/> class.
        /// </summary>
        /// <param name="info">the <see cref="SerializationInfo"/> that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected OpenIdConnectProtocolInvalidStateException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
