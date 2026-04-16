// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// This exception is thrown when a security token contained a key identifier but the key was not found by the runtime
    /// and when validation errors exist over the security token. This exception is not intended to be used as a signal
    /// to refresh keys.
    /// </summary>
    /// <remarks>
    /// This exception type is now considered obsolete and will be removed in the next major version (7.0.0).
    /// </remarks>
    [Serializable]
    [Obsolete(
        "This expception is no longer being thrown by Microsoft.IdentityModel and will be removed in the next major " +
        "version see: https://aka.ms/SecurityTokenUnableToValidateException",
        false)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public class SecurityTokenUnableToValidateException : SecurityTokenInvalidSignatureException
    {
        [NonSerialized]
        const string _Prefix = "Microsoft.IdentityModel." + nameof(SecurityTokenUnableToValidateException) + ".";

        [NonSerialized]
        const string _ValidationFailureKey = _Prefix + nameof(ValidationFailure);

        /// <summary>
        /// Indicates the type of the validation failure.
        /// </summary>
        public ValidationFailure ValidationFailure { get; set; } = ValidationFailure.None;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenSignatureKeyNotFoundException"/> class.
        /// </summary>
        public SecurityTokenUnableToValidateException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenSignatureKeyNotFoundException"/> class.
        /// </summary>
        /// <param name="validationFailure">The validation failures.</param>
        /// <param name="message">Addtional information to be included in the exception and displayed to user.</param>
        public SecurityTokenUnableToValidateException(ValidationFailure validationFailure, string message)
            : base(message)
        {
            ValidationFailure = validationFailure;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenSignatureKeyNotFoundException"/> class.
        /// </summary>
        /// <param name="message">Addtional information to be included in the exception and displayed to user.</param>
        public SecurityTokenUnableToValidateException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenSignatureKeyNotFoundException"/> class.
        /// </summary>
        /// <param name="message">Addtional information to be included in the exception and displayed to user.</param>
        /// <param name="innerException">A <see cref="Exception"/> that represents the root cause of the exception.</param>
        public SecurityTokenUnableToValidateException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenSignatureKeyNotFoundException"/> class.
        /// </summary>
        /// <param name="info">the <see cref="SerializationInfo"/> that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected SecurityTokenUnableToValidateException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            SerializationInfoEnumerator enumerator = info.GetEnumerator();
            while (enumerator.MoveNext())
            {
                switch (enumerator.Name)
                {
                    case _ValidationFailureKey:
                        ValidationFailure = (ValidationFailure)info.GetValue(_ValidationFailureKey, typeof(ValidationFailure));
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

            info.AddValue(_ValidationFailureKey, ValidationFailure);
        }
    }
}
