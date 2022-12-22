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

using System.Collections.Generic;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Tokens.Saml
{
    /// <summary>
    /// Represents the Evidence element specified in [Saml, 2.4.5.2].
    /// </summary>
    /// <remarks>
    /// Contains one or more assertions or assertion references that the SAML
    /// authority relied on in issuing the authorization decision.
    /// </remarks>
    public class SamlEvidence
    {
        internal SamlEvidence()
        {
            AssertionIDReferences = new List<string>();
            Assertions = new List<SamlAssertion>();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SamlEvidence"/> class from a <see cref="SamlAssertion"/>.
        /// </summary>
        /// <param name="assertionIDReferences"><see cref="IEnumerable{String}"/>.</param>
        public SamlEvidence(IEnumerable<string> assertionIDReferences)
            : this(assertionIDReferences, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SamlEvidence"/> class from a <see cref="SamlAssertion"/>.
        /// </summary>
        /// <param name="assertions"><see cref="IEnumerable{SamlAssertion}"/>.</param>
        public SamlEvidence(IEnumerable<SamlAssertion> assertions)
            : this(null, assertions)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SamlEvidence"/> class from a <see cref="SamlAssertion"/>.
        /// </summary>
        /// <param name="assertionIDReferences"><see cref="IEnumerable{String}"/>.</param>
        /// <param name="assertions"><see cref="IEnumerable{SamlAssertion}"/>.</param>
        public SamlEvidence(IEnumerable<string> assertionIDReferences, IEnumerable<SamlAssertion> assertions)
        {
            if (assertionIDReferences == null && assertions == null)
                throw LogExceptionMessage(new SamlSecurityTokenException(LogMessages.IDX11509));

            AssertionIDReferences = (assertionIDReferences == null) ? new List<string>() : new List<string>(assertionIDReferences);
            Assertions = (assertions == null) ? new List<SamlAssertion>() : new List<SamlAssertion>(assertions);
        }

        /// <summary>
        /// Gets a collection of <see cref="ICollection{String}"/>.
        /// </summary>
        public ICollection<string> AssertionIDReferences
        {
            get;
        }

        /// <summary>
        /// Gets a collection of <see cref="ICollection{SamlAssertion}"/>  for use by the <see cref="SamlEvidence"/>.
        /// </summary>
        public ICollection<SamlAssertion> Assertions
        {
            get;
        }
    }
}
