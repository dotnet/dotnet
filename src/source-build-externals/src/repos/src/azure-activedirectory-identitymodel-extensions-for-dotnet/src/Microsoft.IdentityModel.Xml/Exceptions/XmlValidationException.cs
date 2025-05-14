// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Xml
{
    /// <summary>
    /// This exception is thrown when a problem occurs when validating the XML &lt;Signature>.
    /// </summary>
    [Serializable]
    public class XmlValidationException : XmlException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlValidationException"/> class.
        /// </summary>
        public XmlValidationException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlValidationException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public XmlValidationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlValidationException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The <see cref="Exception"/> that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public XmlValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlValidationException"/> class.
        /// </summary>
        /// <param name="info">the <see cref="SerializationInfo"/> that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected XmlValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
