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
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
    /// <summary>
    /// Represents the NameID element as specified in [Saml2Core, 2.2.3].
    /// see: http://docs.oasis-open.org/security/saml/v2.0/saml-core-2.0-os.pdf
    /// </summary>
    public class Saml2NameIdentifier
    {
        private Uri _format;
        private string _nameQualifier;
        private string _serviceProviderPointNameQualifier;
        private string _serviceProviderdId;
        private string _value;

        /// <summary>
        /// Initializes an instance of <see cref="Saml2NameIdentifier"/> from a name.
        /// </summary>
        /// <param name="name">Name string to initialize with.</param>
        public Saml2NameIdentifier(string name)
            : this(name, null)
        { }

        /// <summary>
        /// Initializes an instance of <see cref="Saml2NameIdentifier"/> from a name and format.
        /// </summary>
        /// <param name="name">Name string to initialize with.</param>
        /// <param name="format"><see cref="Uri"/> specifying the identifier format.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="name"/> is null of empty.</exception>
        /// <exception cref="ArgumentException">if <paramref name="format"/> is not an absolute Uri.</exception>
        public Saml2NameIdentifier(string name, Uri format)
        {
            Value = name;
            Format = format;
        }

        /// <summary>
        /// Gets or sets the <see cref="EncryptingCredentials"/> used for encrypting. 
        /// </summary>
        public EncryptingCredentials EncryptingCredentials
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a URI reference representing the classification of string-based identifier 
        /// information. [Saml2Core, 2.2.2]
        /// </summary>
        /// <exception cref="ArgumentException">if 'value' is not an absolute Uri.</exception>
        public Uri Format
        {
            get { return _format; }
            set
            {
                if (null != value && !value.IsAbsoluteUri)
                    throw LogExceptionMessage(new ArgumentException(FormatInvariant(LogMessages.IDX13300, MarkAsNonPII(nameof(Format)), value), nameof(value)));

                _format = value;
            }
        }

        /// <summary>
        ///  Gets or sets the security or administrative domain that qualifies the name. [Saml2Core, 2.2.2]
        /// </summary>
        public string NameQualifier
        {
            get { return _nameQualifier; }
            set { _nameQualifier = XmlUtil.NormalizeEmptyString(value); }
        }

        /// <summary>
        /// Gets or sets a name that further qualifies the name of a service provider or affiliation 
        /// of providers. [Saml2Core, 2.2.2]
        /// </summary>
        public string SPNameQualifier
        {
            get { return _serviceProviderPointNameQualifier; }
            set { _serviceProviderPointNameQualifier = XmlUtil.NormalizeEmptyString(value); }
        }

        /// <summary>
        /// Gets or sets a name identifier established by a service provider or affiliation of providers 
        /// for the entity, if different from the primary name identifier. [Saml2Core, 2.2.2]
        /// </summary>
        public string SPProvidedId
        {
            get { return _serviceProviderdId; }
            set { _serviceProviderdId = XmlUtil.NormalizeEmptyString(value); }
        }

        /// <summary>
        /// Gets or sets the value of the name identifier.
        /// </summary>
        /// <exception cref="ArgumentNullException">if 'value' is null or empty.</exception>
        public string Value
        {
            get { return _value; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw LogArgumentNullException(nameof(value));

                _value = value;
            }
        }
    }
}
