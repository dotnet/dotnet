// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.Serialization;
using Microsoft.IdentityModel.Tokens.Experimental;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// This exception is thrown when the token type ('typ' header claim) of a JWT token is invalid.
    /// </summary>
    [Serializable]
    public class SecurityTokenInvalidTypeException : SecurityTokenValidationException
    {
        [NonSerialized]
        const string _Prefix = "Microsoft.IdentityModel." + nameof(SecurityTokenInvalidTypeException) + ".";

        [NonSerialized]
        const string _InvalidTypeKey = _Prefix + nameof(InvalidType);

        /// <summary>
        /// Gets or sets the invalid type that created the validation exception.
        /// </summary>
        public string InvalidType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenInvalidTypeException"/> class.
        /// </summary>
        public SecurityTokenInvalidTypeException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenInvalidTypeException"/> class.
        /// </summary>
        /// <param name="message">Additional information to be included in the exception and displayed to user.</param>
        public SecurityTokenInvalidTypeException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenInvalidTypeException"/> class.
        /// </summary>
        /// <param name="message">Additional information to be included in the exception and displayed to user.</param>
        /// <param name="innerException">A <see cref="Exception"/> that represents the root cause of the exception.</param>
        public SecurityTokenInvalidTypeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenInvalidTypeException"/> class.
        /// </summary>
        /// <param name="info">the <see cref="SerializationInfo"/> that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected SecurityTokenInvalidTypeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            SerializationInfoEnumerator enumerator = info.GetEnumerator();
            while (enumerator.MoveNext())
            {
                switch (enumerator.Name)
                {
                    case _InvalidTypeKey:
                        InvalidType = info.GetString(_InvalidTypeKey);
                        break;

                    default:
                        // Ignore other fields.
                        break;
                }
            }
        }

        /// <inheritdoc/>
#if NET8_0_OR_GREATER
        [Obsolete("Formatter-based serialization is obsolete", DiagnosticId = "SYSLIB0051")]
#endif
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            if (!string.IsNullOrEmpty(InvalidType))
                info.AddValue(_InvalidTypeKey, InvalidType);
        }

        #region Experimental
        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenInvalidTypeException"/> class.
        /// </summary>
        /// <param name="message">Additional information to be included in the exception and displayed to user.</param>
        /// <param name="validationError">The <see cref="ValidationError"/> that is associated with the exception.</param>
        internal SecurityTokenInvalidTypeException(string message, ValidationError validationError)
            : base(message, validationError)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenInvalidTypeException"/> class.
        /// </summary>
        /// <param name="message">Additional information to be included in the exception and displayed to user.</param>
        /// <param name="validationError">The <see cref="ValidationError"/> that is associated with the exception.</param>
        /// <param name="innerException">An <see cref="Exception"/> that represents the root cause of the exception.</param>
        internal SecurityTokenInvalidTypeException(string message, ValidationError validationError, Exception innerException)
            : base(message, validationError, innerException)
        {
        }
        #endregion
    }
}
