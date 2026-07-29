// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.Serialization;
using Microsoft.IdentityModel.Tokens.Experimental;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// This exception is thrown when a security token contained a key identifier but the key was not found by the runtime.
    /// </summary>
    [Serializable]
    public class SecurityTokenSignatureKeyNotFoundException : SecurityTokenInvalidSignatureException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenSignatureKeyNotFoundException"/> class.
        /// </summary>
        public SecurityTokenSignatureKeyNotFoundException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenSignatureKeyNotFoundException"/> class.
        /// </summary>
        /// <param name="message">Additional information to be included in the exception and displayed to user.</param>
        public SecurityTokenSignatureKeyNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenSignatureKeyNotFoundException"/> class.
        /// </summary>
        /// <param name="message">Additional information to be included in the exception and displayed to user.</param>
        /// <param name="innerException">A <see cref="Exception"/> that represents the root cause of the exception.</param>
        public SecurityTokenSignatureKeyNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenSignatureKeyNotFoundException"/> class.
        /// </summary>
        /// <param name="info">the <see cref="SerializationInfo"/> that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected SecurityTokenSignatureKeyNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #region Experimental
        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenSignatureKeyNotFoundException"/> class.
        /// </summary>
        /// <param name="message">Additional information to be included in the exception and displayed to user.</param>
        /// <param name="validationError">The <see cref="ValidationError"/> that is associated with the exception.</param>
        internal SecurityTokenSignatureKeyNotFoundException(string message, ValidationError validationError)
            : base(message, validationError)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenSignatureKeyNotFoundException"/> class.
        /// </summary>
        /// <param name="message">Additional information to be included in the exception and displayed to user.</param>
        /// <param name="validationError">The <see cref="ValidationError"/> that is associated with the exception.</param>
        /// <param name="innerException">An <see cref="Exception"/> that represents the root cause of the exception.</param>
        internal SecurityTokenSignatureKeyNotFoundException(string message, ValidationError validationError, Exception innerException)
            : base(message, validationError, innerException)
        {
        }
        #endregion
    }
}
