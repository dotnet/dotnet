// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// This exception is thrown when the cloud instance of the signing key was not matched with the cloud instance from configuration.
    /// </summary>
    [Serializable]
    public class SecurityTokenInvalidCloudInstanceException : SecurityTokenInvalidSigningKeyException
    {
        [NonSerialized]
        const string _Prefix = "Microsoft.IdentityModel." + nameof(SecurityTokenInvalidCloudInstanceException) + ".";

        [NonSerialized]
        const string _SigningKeyCloudInstanceNameKey = _Prefix + nameof(SigningKeyCloudInstanceName);

        [NonSerialized]
        const string _ConfigurationCloudInstanceNameKey = _Prefix + nameof(ConfigurationCloudInstanceName);

        /// <summary>
        /// Gets or sets the cloud instance name of the signing key that created the validation exception.
        /// </summary>
        public string SigningKeyCloudInstanceName { get; set; }

        /// <summary>
        /// Gets or sets the cloud instance name from the configuration that did not match the cloud instance name of the signing key.
        /// </summary>
        public string ConfigurationCloudInstanceName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenInvalidCloudInstanceException"/> class.
        /// </summary>
        public SecurityTokenInvalidCloudInstanceException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenInvalidCloudInstanceException"/> class.
        /// </summary>
        /// <param name="message">Additional information to be included in the exception and displayed to user.</param>
        public SecurityTokenInvalidCloudInstanceException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenInvalidCloudInstanceException"/> class.
        /// </summary>
        /// <param name="message">Additional information to be included in the exception and displayed to user.</param>
        /// <param name="innerException">A <see cref="Exception"/> that represents the root cause of the exception.</param>
        public SecurityTokenInvalidCloudInstanceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenInvalidCloudInstanceException"/> class.
        /// </summary>
        /// <param name="info">the <see cref="SerializationInfo"/> that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
#if NET8_0_OR_GREATER
        [Obsolete("Formatter-based serialization is obsolete", DiagnosticId = "SYSLIB0051")]
#endif
        protected SecurityTokenInvalidCloudInstanceException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            SerializationInfoEnumerator enumerator = info.GetEnumerator();
            while (enumerator.MoveNext())
            {
                switch (enumerator.Name)
                {
                    case _SigningKeyCloudInstanceNameKey:
                        SigningKeyCloudInstanceName = info.GetString(_SigningKeyCloudInstanceNameKey);
                        break;

                    case _ConfigurationCloudInstanceNameKey:
                        ConfigurationCloudInstanceName = info.GetString(_ConfigurationCloudInstanceNameKey);
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

            if (!string.IsNullOrEmpty(SigningKeyCloudInstanceName))
                info.AddValue(_SigningKeyCloudInstanceNameKey, SigningKeyCloudInstanceName);

            if (!string.IsNullOrEmpty(ConfigurationCloudInstanceName))
                info.AddValue(_ConfigurationCloudInstanceNameKey, ConfigurationCloudInstanceName);
        }
    }
}
