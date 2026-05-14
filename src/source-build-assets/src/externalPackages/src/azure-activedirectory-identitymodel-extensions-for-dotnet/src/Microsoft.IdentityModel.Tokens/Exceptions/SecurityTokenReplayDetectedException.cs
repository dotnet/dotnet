// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Throw this exception when a received Security Token has been replayed.
    /// </summary>
    [Serializable]
    public class SecurityTokenReplayDetectedException : SecurityTokenValidationException
    {
        /// <summary>
        /// Initializes a new instance of  <see cref="SecurityTokenReplayDetectedException"/>
        /// </summary>
        public SecurityTokenReplayDetectedException()
            : base("SecurityToken replay detected")
        {
        }

        /// <summary>
        /// Initializes a new instance of  <see cref="SecurityTokenReplayDetectedException"/>
        /// </summary>
        public SecurityTokenReplayDetectedException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of  <see cref="SecurityTokenReplayDetectedException"/>
        /// </summary>
        public SecurityTokenReplayDetectedException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenReplayDetectedException"/> class.
        /// </summary>
        /// <param name="info">the <see cref="SerializationInfo"/> that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected SecurityTokenReplayDetectedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
