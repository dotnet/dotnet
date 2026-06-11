// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Tokens.Saml
{
    /// <summary>
    /// Comparison class supporting multi-part keys for a dicitionary
    /// </summary>
    public class SamlAttributeKeyComparer : IEqualityComparer<SamlAttributeKeyComparer.AttributeKey>
    {
        /// <summary>
        /// A class contains Saml attribute key.
        /// </summary>
        public class AttributeKey
        {
            readonly int _hashCode;

            /// <summary>
            /// Represents the Saml Attribute Key.
            /// </summary>
            /// <param name="attribute"></param>
            public AttributeKey(SamlAttribute attribute)
            {
                if (attribute == null)
                    throw LogArgumentNullException(nameof(attribute));

                FriendlyName = string.Empty;
                Name = attribute.Name;
                NameFormat = string.Empty;
                Namespace = attribute.Namespace ?? string.Empty;
                ValueType = attribute.AttributeValueXsiType ?? string.Empty;
                OriginalIssuer = attribute.OriginalIssuer ?? string.Empty;

                _hashCode = ComputeHashCode();
            }

            internal string FriendlyName { get; }
            internal string Name { get; }
            internal string NameFormat { get; }
            internal string Namespace { get; }
            internal string OriginalIssuer { get; }
            internal string ValueType { get; }

            int ComputeHashCode()
            {
                int hashCode = Name.GetHashCode();
                hashCode ^= FriendlyName.GetHashCode();
                hashCode ^= NameFormat.GetHashCode();
                hashCode ^= Namespace.GetHashCode();
                hashCode ^= ValueType.GetHashCode();
                hashCode ^= OriginalIssuer.GetHashCode();
                return hashCode;
            }

            /// <inheritdoc/>
            public override int GetHashCode() => _hashCode;

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
                    Namespace.Equals(other.Namespace) &&
                    OriginalIssuer.Equals(other.OriginalIssuer) &&
                    ValueType.Equals(other.ValueType);
            }
        }

        #region IEqualityComparer<AttributeKey> Members

        /// <inheritdoc/>
        public bool Equals(AttributeKey x, AttributeKey y)
        {
            if (x == null && y == null)
                return true;
            else if (x == null || y == null)
                return false;

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
