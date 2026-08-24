// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

#if IS_DESKTOP
using System.Runtime.Serialization;
#endif

namespace NuGet.Credentials
{
    /// <summary>
    /// The exception thrown when a credential provider returns an invalid response.
    /// </summary>
    [Serializable]
    public class ProviderException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderException"/> class.
        /// </summary>
        public ProviderException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderException"/> class with an error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ProviderException(string? message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderException"/> class with an error message and inner exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The exception that caused the current exception.</param>
        public ProviderException(string? message, Exception? inner) : base(message, inner)
        {
        }
#if IS_DESKTOP
        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The object that contains the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected ProviderException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
#endif
    }
}
