// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.Serialization;
using Microsoft.IdentityModel.Tokens.Experimental;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// This exception is thrown when 'signature' of a token was not valid.
    /// </summary>
    [Serializable]
    public class SecurityTokenInvalidSignatureException : SecurityTokenValidationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenInvalidSignatureException"/> class.
        /// </summary>
        public SecurityTokenInvalidSignatureException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenInvalidSignatureException"/> class.
        /// </summary>
        /// <param name="message">Additional information to be included in the exception and displayed to user.</param>
        public SecurityTokenInvalidSignatureException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenInvalidSignatureException"/> class.
        /// </summary>
        /// <param name="message">Additional information to be included in the exception and displayed to user.</param>
        /// <param name="innerException">An <see cref="Exception"/> that represents the root cause of the exception.</param>
        public SecurityTokenInvalidSignatureException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenInvalidSignatureException"/> class.
        /// </summary>
        /// <param name="info">the <see cref="SerializationInfo"/> that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected SecurityTokenInvalidSignatureException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #region Experimental
        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenInvalidSignatureException"/> class.
        /// </summary>
        /// <param name="message">Additional information to be included in the exception and displayed to user.</param>
        /// <param name="validationError">The <see cref="ValidationError"/> that is associated with the exception.</param>
        internal SecurityTokenInvalidSignatureException(string message, ValidationError validationError)
            : base(message, validationError)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenInvalidSignatureException"/> class.
        /// </summary>
        /// <param name="message">Additional information to be included in the exception and displayed to user.</param>
        /// <param name="validationError">The <see cref="ValidationError"/> that is associated with the exception.</param>
        /// <param name="innerException">An <see cref="Exception"/> that represents the root cause of the exception.</param>
        internal SecurityTokenInvalidSignatureException(string message, ValidationError validationError, Exception innerException)
            : base(message, validationError, innerException)
        {
        }
        #endregion
    }
}
