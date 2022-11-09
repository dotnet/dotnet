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
    /// Represents the AudienceRestriction element specified in [Saml2Core, 2.5.1.4].
    /// see: http://docs.oasis-open.org/security/saml/v2.0/saml-core-2.0-os.pdf
    /// </summary>
    public class Saml2AudienceRestriction
    {
        /// <summary>
        /// Creates an instance of Saml2AudienceRestriction.
        /// </summary>
        /// <param name="audience">The audience element contained in this restriction.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="audience"/> is null or empty.</exception>
        public Saml2AudienceRestriction(string audience)
        {
            if (string.IsNullOrEmpty(audience))
                throw LogArgumentNullException(nameof(audience));

            Audiences = new List<string> { audience };
        }

        /// <summary>
        /// Creates an instance of Saml2AudienceRestriction.
        /// </summary>
        /// <param name="audiences">The collection of audience elements contained in this restriction.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="audiences"/> is null.</exception>
        public Saml2AudienceRestriction(IEnumerable<string> audiences)
        {
            if (audiences == null)
                throw LogArgumentNullException(nameof(audiences));

            Audiences = new List<string>(audiences);
        }

        /// <summary>
        /// Gets the audiences for which the assertion is addressed.
        /// </summary>
        public ICollection<string> Audiences
        {
            get;
        }
    }
}
