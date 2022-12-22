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
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
    /// <summary>
    /// A security token backed by a SAML2 assertion.
    /// </summary>
    public class Saml2SecurityToken : SecurityToken
    {
        /// <summary>
        /// Initializes an instance of <see cref="Saml2SecurityToken"/> from a <see cref="Saml2Assertion"/>.
        /// </summary>
        /// <param name="assertion">A <see cref="Saml2Assertion"/> to initialize from.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="assertion"/> is null.</exception>
        public Saml2SecurityToken(Saml2Assertion assertion)
        {
            Assertion = assertion ?? throw LogArgumentNullException(nameof(assertion));
        }

        /// <summary>
        /// Gets the <see cref="Saml2Assertion"/> for this token.
        /// </summary>
        public Saml2Assertion Assertion
        {
            get;
        }

        /// <summary>
        /// Gets the SecurityToken id.
        /// </summary>
        public override string Id
        {
            get { return Assertion.Id.Value; }
        }

        /// <summary>
        /// Gets the issuer of this token
        /// </summary>
        public override string Issuer
        {
            get { return Assertion.Issuer.Value; }
        }

        /// <summary>
        /// Gets the <see cref="SecurityKey"/> for this instance.
        /// </summary>
        public override SecurityKey SecurityKey
        {
            get { return null; }
        }

        /// <summary>
        /// Gets or sets the <see cref="SecurityKey"/> that was used to Sign this assertion.
        /// </summary>
        public override SecurityKey SigningKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the time the token is valid from. This value is always in UTC.
        /// </summary>
        public override DateTime ValidFrom
        {
            get
            {
                if (Assertion.Conditions != null && Assertion.Conditions.NotBefore != null)
                    return Assertion.Conditions.NotBefore.Value;
                else
                    return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Gets the time the token is valid to. This value is always in UTC.
        /// </summary>
        public override DateTime ValidTo
        {
            get
            {
                if (Assertion.Conditions != null && Assertion.Conditions.NotOnOrAfter != null)
                    return Assertion.Conditions.NotOnOrAfter.Value;
                else
                    return DateTime.MaxValue;
            }
        }
    }
}
