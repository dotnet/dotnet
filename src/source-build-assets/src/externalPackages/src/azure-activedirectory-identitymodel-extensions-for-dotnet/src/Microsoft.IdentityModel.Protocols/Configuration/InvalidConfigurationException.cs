// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Protocols.Configuration
{
    /// <summary>
    /// This exception is thrown when retrieved configuration is not valid.
    /// </summary>
    [Serializable]
    public class InvalidConfigurationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidConfigurationException"/> class.
        /// </summary>
        public InvalidConfigurationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidConfigurationException"/> class.
        /// </summary>
        /// <param name="message">Additional information to be included in the exception and displayed to user.</param>
        public InvalidConfigurationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidConfigurationException"/> class.
        /// </summary>
        /// <param name="message">Additional information to be included in the exception and displayed to user.</param>
        /// <param name="innerException">A <see cref="Exception"/> that represents the root cause of the exception.</param>
        public InvalidConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidConfigurationException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected InvalidConfigurationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
