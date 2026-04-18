// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Xml
{
    /// <summary>
    /// This exception is thrown when a problem occurs reading XML.
    /// </summary>
    [Serializable]
    public class XmlReadException : XmlException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlReadException"/> class.
        /// </summary>
        public XmlReadException()
            : base()
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlReadException"/> class.
        /// </summary>
        /// <param name="message">Addtional information to be included in the exception and displayed to user.</param>
        public XmlReadException(string message)
            : base(message)
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlReadException"/> class.
        /// </summary>
        /// <param name="message">Addtional information to be included in the exception and displayed to user.</param>
        /// <param name="innerException">A <see cref="Exception"/> that represents the root cause of the exception.</param>
        public XmlReadException(string message, Exception innerException)
            : base(message, innerException)
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlReadException"/> class.
        /// </summary>
        /// <param name="info">the <see cref="SerializationInfo"/> that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected XmlReadException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {}
    }
}
