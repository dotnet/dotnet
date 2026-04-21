// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Protocols.SignedHttpRequest
{
    /// <summary>
    /// This exception is thrown when a SignedHttpRequest handler encounters an error during 'ts' claim validation.  
    /// </summary>
    [Serializable]
    public class SignedHttpRequestInvalidTsClaimException : SignedHttpRequestValidationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SignedHttpRequestInvalidTsClaimException"/> class.
        /// </summary>
        public SignedHttpRequestInvalidTsClaimException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignedHttpRequestInvalidTsClaimException"/> class.
        /// </summary>
        /// <param name="message">Additional information to be included in the exception and displayed to user.</param>
        public SignedHttpRequestInvalidTsClaimException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignedHttpRequestInvalidTsClaimException"/> class.
        /// </summary>
        /// <param name="message">Additional information to be included in the exception and displayed to user.</param>
        /// <param name="innerException">A <see cref="Exception"/> that represents the root cause of the exception.</param>
        public SignedHttpRequestInvalidTsClaimException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignedHttpRequestInvalidTsClaimException"/> class.
        /// </summary>
        /// <param name="info">the <see cref="SerializationInfo"/> that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected SignedHttpRequestInvalidTsClaimException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
