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
using Microsoft.IdentityModel.Xml;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
    /// <summary>
    /// Represents the SubjectLocality element specified in [Saml2Core, 2.7.2.1].
    /// </summary>
    /// <remarks>
    /// This element is entirely advisory, since both of these fields are quite 
    /// easily "spoofed". [Saml2Core, 2.7.2.1]
    /// </remarks>
    public class Saml2SubjectLocality
    {
        private string _address;
        private string _dnsName;

        /// <summary>
        /// Initializes an instance of <see cref="Saml2SubjectLocality"/>.
        /// </summary>
        internal Saml2SubjectLocality()
        {
        }

        /// <summary>
        /// Initializes an instance of <see cref="Saml2SubjectLocality"/> from an address and DNS name.
        /// </summary>
        /// <param name="address">A <see cref="String"/> indicating the address.</param>
        /// <param name="dnsName">A <see cref="String"/> indicating the DNS name.</param>
        public Saml2SubjectLocality(string address, string dnsName)
        {
            Address = address;
            DnsName = dnsName;
        }

        /// <summary>
        /// Gets or sets the network address of the system from which the principal identified
        /// by the subject was authenticated. [Saml2Core, 2.7.2.1]
        /// </summary>
        public string Address
        {
            get { return _address; }
            set { _address = XmlUtil.NormalizeEmptyString(value); }
        }

        /// <summary>
        /// Gets or sets the DNS name of the system from which the principal identified by the 
        /// subject was authenticated. [Saml2Core, 2.7.2.1]
        /// </summary>
        public string DnsName
        {
            get { return _dnsName; }
            set { _dnsName = XmlUtil.NormalizeEmptyString(value); }
        }
    }
}
