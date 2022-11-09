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
using System.Collections.ObjectModel;
using Microsoft.IdentityModel.Xml;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
    /// <summary>
    /// Represents the SubjectConfirmationData element and the associated 
    /// KeyInfoConfirmationDataType defined in [Saml2Core, 2.4.1.2-2.4.1.3].
    /// </summary>
    public class Saml2SubjectConfirmationData
    {
        private string _address;
        private Collection<KeyInfo> _keyInfos = new Collection<KeyInfo>();
        private DateTime? _notBefore;
        private DateTime? _notOnOrAfter;
        private Uri _recipient;

        /// <summary>
        /// Initializes an instance of <see cref="Saml2SubjectConfirmationData"/>.
        /// </summary>
        public Saml2SubjectConfirmationData()
        {
        }

        /// <summary>
        /// Gets or sets the network address/location from which an attesting entity can present the 
        /// assertion. [Saml2Core, 2.4.1.2]
        /// </summary>
        public string Address
        {
            get { return _address; }
            set { _address = XmlUtil.NormalizeEmptyString(value); }
        }

        /// <summary>
        /// Gets or sets the <see cref="Saml2Id"/> of a SAML protocol message in response to which an attesting entity can 
        /// present the assertion. [Saml2Core, 2.4.1.2]
        /// </summary>
        public Saml2Id InResponseTo { get; set; }

        /// <summary>
        /// Gets a collection of <see cref="SecurityKey"/> which can be used to authenticate an attesting entity. [Saml2Core, 2.4.1.3]
        /// </summary>
        public ICollection<KeyInfo> KeyInfos
        {
            get { return _keyInfos; }
        }

        /// <summary>
        /// Gets or sets a time instant before which the subject cannot be confirmed. If the provided DateTime is not in UTC, it will
        /// be converted to UTC.[Saml2Core, 2.4.1.2]
        /// </summary>
        public DateTime? NotBefore
        {
            get { return _notBefore; }
            set { _notBefore = DateTimeUtil.ToUniversalTime(value); }
        }

        /// <summary>
        /// Gets or sets a time instant at which the subject can no longer be confirmed. If the provided DateTime is not in UTC, it will
        /// be converted to UTC. [Saml2Core, 2.4.1.2]
        /// </summary>
        public DateTime? NotOnOrAfter
        {
            get { return _notOnOrAfter; }
            set { _notOnOrAfter = value?.ToUniversalTime(); }
        }

        /// <summary>
        /// Gets or sets a URI specifying the entity or location to which an attesting entity can present 
        /// the assertion. [Saml2Core, 2.4.1.2]
        /// </summary>
        public Uri Recipient
        {
            get { return _recipient;  }
            set
            {
                if (value == null)
                    throw LogArgumentNullException(nameof(value));

                if (!value.IsAbsoluteUri)
                    throw LogExceptionMessage(new ArgumentException(FormatInvariant(LogMessages.IDX13300, MarkAsNonPII(nameof(Recipient)), value)));

                _recipient = value;
            }
        }
    }
}
