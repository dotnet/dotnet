// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.SampleTests
{
    public class SampleTestTokenValidationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="SampleTestTokenValidationException"/>.
        /// </summary>
        public SampleTestTokenValidationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SampleTestTokenValidationException"/>.
        /// </summary>
        /// <param name="message">Message of the exception.</param>
        public SampleTestTokenValidationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SampleTestTokenValidationException"/>.
        /// </summary>
        /// <param name="message">Message of the exception.</param>
        /// <param name="innerException">Inner exception of the exception.</param>
        public SampleTestTokenValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SampleTestTokenValidationException"/>.
        /// </summary>
        /// <param name="info">SerializationInfo for the exception.</param>
        /// <param name="context">StreamingContext for the exception.</param>
        protected SampleTestTokenValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
