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
    /// Represents the AuthnContext element specified in [Saml2Core, 2.7.2.2].
    /// see: http://docs.oasis-open.org/security/saml/v2.0/saml-core-2.0-os.pdf
    /// </summary>
    /// <remarks>
    /// <para>
    /// This base class does not directly support any by-value authentication 
    /// context declarations (represented in XML by the AuthnContextDecl element). 
    /// To support by-value declarations, extend this class to support the data 
    /// model and extend Saml2AssertionSerializer, overriding ReadAuthnContext 
    /// and WriteAuthnContext to read and write the by-value declaration.
    /// </para>
    /// </remarks>
    public class Saml2AuthenticationContext
    {
        private Uri _classReference;
        private Uri _declarationReference;

        /// <summary>
        /// Creates an instance of Saml2AuthenticationContext.
        /// </summary>
        public Saml2AuthenticationContext()
        {
        }

        /// <summary>
        /// Creates an instance of Saml2AuthenticationContext.
        /// </summary>
        /// <param name="classReference">The class reference of the authentication context.</param>
        public Saml2AuthenticationContext(Uri classReference)
        {
            ClassReference = classReference;
        }

        /// <summary>
        /// Creates an instance of Saml2AuthenticationContext.
        /// </summary>
        /// <param name="classReference">The class reference of the authentication context.</param>
        /// <param name="declarationReference">The declaration reference of the authentication context.</param>
        public Saml2AuthenticationContext(Uri classReference, Uri declarationReference)
        {
            ClassReference = classReference;
            DeclarationReference = declarationReference;
        }

        /// <summary>
        /// Gets Zero or more unique identifiers of authentication authorities that 
        /// were involved in the authentication of the principal (not including
        /// the assertion issuer, who is presumed to have been involved without
        /// being explicitly named here). [Saml2Core, 2.7.2.2]
        /// </summary>
        public ICollection<Uri> AuthenticatingAuthorities
        {
            get;
        } = new List<Uri>();

        /// <summary>
        /// Gets or sets a URI reference identifying an authentication context class that 
        /// describes the authentication context declaration that follows.
        /// [Saml2Core, 2.7.2.2]
        /// </summary>
        /// <exception cref="ArgumentNullException">if 'value' is null.</exception>
        /// <exception cref="ArgumentException">if 'value' is not an absolute Uri.</exception>
        public Uri ClassReference
        {
            get { return _classReference; }
            set
            {
                if (value == null)
                    throw LogArgumentNullException(nameof(value));

                if (!value.IsAbsoluteUri)
                    throw LogExceptionMessage(new ArgumentException(FormatInvariant(LogMessages.IDX13300, MarkAsNonPII(nameof(ClassReference)), value)));

                _classReference = value;
            }
        }

        /// <summary>
        /// Gets or sets a URI reference that identifies an authentication context 
        /// declaration. [Saml2Core, 2.7.2.2]
        /// </summary>
        /// <exception cref="ArgumentException">if 'value' is not null and is not an absolute Uri.</exception>
        public Uri DeclarationReference
        {
            get { return _declarationReference; }
            set
            {
                if (value != null && !value.IsAbsoluteUri)
                    throw LogExceptionMessage(new ArgumentException(FormatInvariant(LogMessages.IDX13300, MarkAsNonPII(nameof(DeclarationReference)), value)));

                _declarationReference = value;
            }
        }
    }
}
