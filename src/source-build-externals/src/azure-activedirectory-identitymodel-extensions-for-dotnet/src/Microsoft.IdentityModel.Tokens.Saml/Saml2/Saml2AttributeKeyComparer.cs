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
using System.Collections.Generic;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
    /// <summary>
    /// Comparison class supporting multi-part keys for a dictionary
    /// </summary>
    internal sealed class Saml2AttributeKeyComparer : IEqualityComparer<Saml2AttributeKeyComparer.AttributeKey>
    {
        public static readonly Saml2AttributeKeyComparer Instance = new Saml2AttributeKeyComparer();

        private Saml2AttributeKeyComparer()
        {
        }

        public class AttributeKey
        {
            readonly int _hashCode;

            internal string FriendlyName { get; }
            internal string Name { get; }
            internal string NameFormat { get; }
            internal string ValueType { get; }
            internal string OriginalIssuer { get; }

            public AttributeKey(Saml2Attribute attribute)
            {
                if (attribute == null)
                    throw LogArgumentNullException(nameof(attribute));

                FriendlyName = attribute.FriendlyName ?? string.Empty;
                Name = attribute.Name;
                NameFormat = attribute.NameFormat == null ? string.Empty : attribute.NameFormat.OriginalString;
                ValueType = attribute.AttributeValueXsiType ?? string.Empty;
                OriginalIssuer = attribute.OriginalIssuer ?? string.Empty;

                _hashCode = ComputeHashCode();
            }

            /// <inheritdoc/>
            public override int GetHashCode() => _hashCode;

            int ComputeHashCode()
            {
                int hashCode = Name.GetHashCode();
                hashCode ^= FriendlyName.GetHashCode();
                hashCode ^= NameFormat.GetHashCode();
                hashCode ^= ValueType.GetHashCode();
                hashCode ^= OriginalIssuer.GetHashCode();
                return hashCode;
            }

            /// <inheritdoc/>
            public override bool Equals(object obj) => Equals(obj as AttributeKey);

            /// <summary>
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <param name="other">An object to compare with this object.</param>
            /// <returns>
            /// <c>true</c> if the current object is equal to the other parameter; otherwise, <c>false</c>.
            /// </returns>
            public bool Equals(AttributeKey other)
            {
                return other != null &&
                    FriendlyName.Equals(other.FriendlyName) &&
                    Name.Equals(other.Name) &&
                    NameFormat.Equals(other.NameFormat) &&
                    ValueType.Equals(other.ValueType) &&
                    OriginalIssuer.Equals(other.OriginalIssuer);
            }
        }

        #region IEqualityComparer<AttributeKey> Members

        /// <inheritdoc/>
        public bool Equals(AttributeKey x, AttributeKey y)
        {
            if (x == null)
                return y == null;

            return x.Equals(y);
        }

        /// <inheritdoc/>
        public int GetHashCode(AttributeKey obj)
        {
            if (obj == null)
                throw LogArgumentNullException(nameof(obj));

            return obj.GetHashCode();
        }

        #endregion
    };
}
