// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Throw this exception when a received Security Token has expiration time in the past.
    /// </summary>
    [Serializable]
    public class SecurityTokenExpiredException : SecurityTokenValidationException
    {
        [NonSerialized]
        const string _Prefix = "Microsoft.IdentityModel." + nameof(SecurityTokenExpiredException) + ".";

        [NonSerialized]
        const string _ExpiresKey = _Prefix + nameof(Expires);

        /// <summary>
        /// Gets or sets the Expires value that created the validation exception. This value is always in UTC.
        /// </summary>
        public DateTime Expires { get; set; }

        /// <summary>
        /// Initializes a new instance of  <see cref="SecurityTokenExpiredException"/>
        /// </summary>
        public SecurityTokenExpiredException()
            : base("SecurityToken has Expired")
        {
        }

        /// <summary>
        /// Initializes a new instance of  <see cref="SecurityTokenExpiredException"/>
        /// </summary>
        public SecurityTokenExpiredException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of  <see cref="SecurityTokenExpiredException"/>
        /// </summary>
        public SecurityTokenExpiredException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenExpiredException"/> class.
        /// </summary>
        /// <param name="info">the <see cref="SerializationInfo"/> that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
#if NET8_0_OR_GREATER
        [Obsolete("Formatter-based serialization is obsolete", DiagnosticId = "SYSLIB0051")]
#endif
        protected SecurityTokenExpiredException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            SerializationInfoEnumerator enumerator = info.GetEnumerator();
            while (enumerator.MoveNext())
            {
                switch (enumerator.Name)
                {
                    case _ExpiresKey:
                        Expires = (DateTime)info.GetValue(_ExpiresKey, typeof(DateTime));
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

            info.AddValue(_ExpiresKey, Expires);
        }
    }
}
