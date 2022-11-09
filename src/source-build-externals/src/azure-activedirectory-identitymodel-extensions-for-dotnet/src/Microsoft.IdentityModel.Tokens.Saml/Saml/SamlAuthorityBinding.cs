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

using System.Xml;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Tokens.Saml
{
    /// <summary>
    /// Represents the SamlAuthorityBinding specified in [Saml, 2.4.3.2].
    /// </summary>
    public class SamlAuthorityBinding
    {
        private XmlQualifiedName _authorityKind;
        private string _binding;
        private string _location;

        internal SamlAuthorityBinding()
        {
        }

        /// <summary>
        /// Create an instance of <see cref="SamlAuthorityBinding"/>.
        /// </summary>
        /// <param name="authorityKind">The type of SAML protocol queries to which the authority described by this element will respond.</param>
        /// <param name="binding">The URI identifying the SAML protocol binding to use in communicating with the authority.</param>
        /// <param name="location">The URI describing how to locate and communicate with the authority.</param>
        public SamlAuthorityBinding(XmlQualifiedName authorityKind, string binding, string location)
        {
            AuthorityKind = authorityKind;
            Binding = binding;
            Location = location;
        }

        /// <summary>
        /// Gets or sets the AuthorityKind of the binding.
        /// </summary>
        public XmlQualifiedName AuthorityKind
        {
            get { return _authorityKind; }
            set
            {
                if (value == null)
                    throw LogArgumentNullException(nameof(value));

                if (string.IsNullOrEmpty(value.Name))
                    throw LogExceptionMessage(new SamlSecurityTokenException(LogMessages.IDX11507));

                _authorityKind = value;
            }
        }

        /// <summary>
        /// Gets or sets the binding.
        /// </summary>
        public string Binding
        {
            get { return _binding; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw LogArgumentNullException(nameof(value));

                _binding = value;
            }
        }

        /// <summary>
        /// Gets or sets the location of the binding.
        /// </summary>
        public string Location
        {
            get { return _location; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw LogArgumentNullException(nameof(value));

                _location = value;
            }
        }
    }
}
