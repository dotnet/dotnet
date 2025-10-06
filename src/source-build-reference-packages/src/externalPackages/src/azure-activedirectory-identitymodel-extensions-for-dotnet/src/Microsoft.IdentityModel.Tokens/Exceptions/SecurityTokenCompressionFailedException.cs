// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Thrown when JWE compression fails.
    /// </summary>
    [Serializable]
    public class SecurityTokenCompressionFailedException : SecurityTokenException
    {
        /// <summary>
        /// Initializes a new instance of  <see cref="SecurityTokenCompressionFailedException"/>
        /// </summary>
        public SecurityTokenCompressionFailedException()
            : base("SecurityToken compression failed.")
        {
        }

        /// <summary>
        /// Initializes a new instance of  <see cref="SecurityTokenCompressionFailedException"/>
        /// </summary>
        public SecurityTokenCompressionFailedException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of  <see cref="SecurityTokenCompressionFailedException"/>
        /// </summary>
        public SecurityTokenCompressionFailedException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenCompressionFailedException"/> class.
        /// </summary>
        /// <param name="info">the <see cref="SerializationInfo"/> that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected SecurityTokenCompressionFailedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}