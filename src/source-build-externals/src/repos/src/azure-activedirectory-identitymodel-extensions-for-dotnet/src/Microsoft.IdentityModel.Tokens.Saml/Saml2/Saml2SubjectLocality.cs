// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

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
