//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// This exception is thrown when 'lifetime' of a token was not valid.
    /// </summary>
    [Serializable]
    public class SecurityTokenInvalidLifetimeException : SecurityTokenValidationException
    {
        [NonSerialized]
        const string _Prefix = "Microsoft.IdentityModel." + nameof(SecurityTokenInvalidLifetimeException) + ".";

        [NonSerialized]
        const string _NotBeforeKey = _Prefix + nameof(NotBefore);

        [NonSerialized]
        const string _ExpiresKey = _Prefix + nameof(Expires);

        /// <summary>
        /// Gets or sets the NotBefore value that created the validation exception. This value is always in UTC.
        /// </summary>
        public DateTime? NotBefore { get; set; }

        /// <summary>
        /// Gets or sets the Expires value that created the validation exception. This value is always in UTC.
        /// </summary>
        public DateTime? Expires { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenInvalidLifetimeException"/> class.
        /// </summary>
        public SecurityTokenInvalidLifetimeException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenInvalidLifetimeException"/> class.
        /// </summary>
        /// <param name="message">Addtional information to be included in the exception and displayed to user.</param>
        public SecurityTokenInvalidLifetimeException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenInvalidLifetimeException"/> class.
        /// </summary>
        /// <param name="message">Addtional information to be included in the exception and displayed to user.</param>
        /// <param name="innerException">A <see cref="Exception"/> that represents the root cause of the exception.</param>
        public SecurityTokenInvalidLifetimeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenInvalidLifetimeException"/> class.
        /// </summary>
        /// <param name="info">the <see cref="SerializationInfo"/> that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected SecurityTokenInvalidLifetimeException(SerializationInfo info, StreamingContext context)
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
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            if (NotBefore.HasValue)
                info.AddValue(_NotBeforeKey, NotBefore.Value);

            if (Expires.HasValue)
                info.AddValue(_ExpiresKey, Expires.Value);
        }
    }
}
