// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Throw this exception when a received Security token has an effective time 
    /// in the future.
    /// </summary>
    [Serializable]
    public class SecurityTokenNotYetValidException : SecurityTokenValidationException
    {
        [NonSerialized]
        const string _Prefix = "Microsoft.IdentityModel." + nameof(SecurityTokenNotYetValidException) + ".";

        [NonSerialized]
        const string _NotBeforeKey = _Prefix + nameof(NotBefore);

        /// <summary>
        /// Gets or sets the NotBefore value that created the validation exception. This value is always in UTC.
        /// </summary>
        public DateTime NotBefore { get; set; }

        /// <summary>
        /// Initializes a new instance of  <see cref="SecurityTokenNotYetValidException"/>
        /// </summary>
        public SecurityTokenNotYetValidException()
            : base("SecurityToken is not yet valid")
        {
        }

        /// <summary>
        /// Initializes a new instance of  <see cref="SecurityTokenNotYetValidException"/>
        /// </summary>
        public SecurityTokenNotYetValidException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of  <see cref="SecurityTokenNotYetValidException"/>
        /// </summary>
        public SecurityTokenNotYetValidException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenNotYetValidException"/> class.
        /// </summary>
        /// <param name="info">the <see cref="SerializationInfo"/> that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
#if NET8_0_OR_GREATER
        [Obsolete("Formatter-based serialization is obsolete", DiagnosticId = "SYSLIB0051")]
#endif
        protected SecurityTokenNotYetValidException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            SerializationInfoEnumerator enumerator = info.GetEnumerator();
            while (enumerator.MoveNext())
            {
                switch (enumerator.Name)
                {
                    case _NotBeforeKey:
                        NotBefore = (DateTime)info.GetValue(_NotBeforeKey, typeof(DateTime));
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

            info.AddValue(_NotBeforeKey, NotBefore);
        }
    }
}
