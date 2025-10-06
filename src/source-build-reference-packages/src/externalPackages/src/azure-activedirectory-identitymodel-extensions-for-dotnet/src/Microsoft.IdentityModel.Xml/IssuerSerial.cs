// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Microsoft.IdentityModel.Xml
{
    /// <summary>
    /// Represents the IssuerSerial property of X509Data as per:  https://www.w3.org/TR/2001/PR-xmldsig-core-20010820/#sec-X509Data
    /// </summary>
    public class IssuerSerial
    {
        /// <summary>
        /// Gets the IssuerName of the IssuerSerial.
        /// </summary>
        public string IssuerName { get; }

        /// <summary>
        /// Gets the SerialNumber of the IssuerSerial.
        /// </summary>
        public string SerialNumber { get; }

        /// <summary>
        /// Creates an IssuerSerial using the specified IssuerName and SerialNumber.
        /// </summary>
        public IssuerSerial(string issuerName, string serialNumber)
        {
            IssuerName = issuerName;
            SerialNumber = serialNumber;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is IssuerSerial serial &&
                string.Equals(IssuerName, serial.IssuerName, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(SerialNumber, serial.SerialNumber, StringComparison.OrdinalIgnoreCase);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = -1073543679;
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(IssuerName);
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SerialNumber);
                return hashCode;
            }
        }
    }
}
