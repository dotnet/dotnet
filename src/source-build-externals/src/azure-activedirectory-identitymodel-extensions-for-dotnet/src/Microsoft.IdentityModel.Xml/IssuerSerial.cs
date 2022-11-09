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
