// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Throw this exception when a received Security Token has an invalid issuer signing key.
    /// </summary>
    [Serializable]
    public class SecurityTokenInvalidSigningKeyException : SecurityTokenValidationException
    {
        /// <summary>
        /// Gets or sets the SigningKey that was found invalid.
        /// </summary>
        public SecurityKey SigningKey { get; set; }

        /// <summary>
        /// Initializes a new instance of  <see cref="SecurityTokenInvalidSigningKeyException"/>
        /// </summary>
        public SecurityTokenInvalidSigningKeyException()
            : base("SecurityToken has invalid issuer signing key.")
        {
        }

        /// <summary>
        /// Initializes a new instance of  <see cref="SecurityTokenInvalidSigningKeyException"/>
        /// </summary>
        public SecurityTokenInvalidSigningKeyException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of  <see cref="SecurityTokenInvalidSigningKeyException"/>
        /// </summary>
        public SecurityTokenInvalidSigningKeyException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenInvalidSigningKeyException"/> class.
        /// </summary>
        /// <param name="info">the <see cref="SerializationInfo"/> that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected SecurityTokenInvalidSigningKeyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
