// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Protocols.SignedHttpRequest
{
    /// <summary>
    /// This exception is thrown when a SignedHttpRequest handler encounters an error during 'Nonce' claim resolution.   
    /// </summary>
    [Serializable]
    public class SignedHttpRequestInvalidNonceClaimException : SignedHttpRequestValidationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SignedHttpRequestInvalidNonceClaimException"/> class.
        /// </summary>
        public SignedHttpRequestInvalidNonceClaimException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignedHttpRequestInvalidNonceClaimException"/> class.
        /// </summary>
        /// <param name="message">Additional information to be included in the exception and displayed to user.</param>
        public SignedHttpRequestInvalidNonceClaimException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignedHttpRequestInvalidNonceClaimException"/> class.
        /// </summary>
        /// <param name="message">Additional information to be included in the exception and displayed to user.</param>
        /// <param name="innerException">A <see cref="Exception"/> that represents the root cause of the exception.</param>
        public SignedHttpRequestInvalidNonceClaimException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignedHttpRequestInvalidNonceClaimException"/> class.
        /// </summary>
        /// <param name="info">the <see cref="SerializationInfo"/> that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected SignedHttpRequestInvalidNonceClaimException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets or sets an <see cref="IDictionary{String, Object}"/> that enables custom extensibility scenarios.
        /// </summary>
        public IDictionary<string, object> PropertyBag { get; set; }
    }
}
