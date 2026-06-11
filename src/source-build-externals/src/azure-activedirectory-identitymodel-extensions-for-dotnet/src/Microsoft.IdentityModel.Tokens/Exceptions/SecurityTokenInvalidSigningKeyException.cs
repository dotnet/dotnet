// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.Serialization;
using Microsoft.IdentityModel.Tokens.Experimental;

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

        #region Experimental
        /// <summary>
        /// Initializes a new instance of  <see cref="SecurityTokenInvalidSigningKeyException"/>
        /// <param name="message">Additional information to be included in the exception and displayed to user.</param>
        /// <param name="validationError">The <see cref="ValidationError"/> that is associated with the exception.</param>
        /// </summary>
        internal SecurityTokenInvalidSigningKeyException(string message, ValidationError validationError)
            : base(message, validationError)
        {
        }

        /// <summary>
        /// Initializes a new instance of  <see cref="SecurityTokenInvalidSigningKeyException"/>
        /// <param name="message">Additional information to be included in the exception and displayed to user.</param>
        /// <param name="validationError">The <see cref="ValidationError"/> that is associated with the exception.</param>
        /// <param name="innerException">An <see cref="Exception"/> that represents the root cause of the exception.</param>
        /// </summary>
        internal SecurityTokenInvalidSigningKeyException(string message, ValidationError validationError, Exception innerException)
            : base(message, validationError, innerException)
        {
        }
        #endregion
    }
}
