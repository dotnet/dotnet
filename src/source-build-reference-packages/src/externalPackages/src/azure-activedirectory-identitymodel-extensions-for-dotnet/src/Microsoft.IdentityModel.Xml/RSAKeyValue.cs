// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Microsoft.IdentityModel.Xml
{
    /// <summary>
    /// The RSAKeyValue found inside of the KeyValue element.
    /// </summary>
    public class RSAKeyValue
    {
        /// <summary>
        /// The modulus of the RSAKeyValue.
        /// </summary>
        public string Modulus { get; }

        /// <summary>
        /// The exponent of the RSAKeyValue.
        /// </summary>
        public string Exponent { get; }

        /// <summary>
        /// Creates an RSAKeyValue using the specified modulus and exponent.
        /// </summary>
        public RSAKeyValue(string modulus, string exponent)
        {
            Modulus = modulus;
            Exponent = exponent;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is RSAKeyValue value &&
                string.Equals(Modulus, value.Modulus, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(Exponent, value.Exponent, StringComparison.OrdinalIgnoreCase);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = 936145456;
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Modulus);
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Exponent);
                return hashCode;
            }
        }
    }
}
