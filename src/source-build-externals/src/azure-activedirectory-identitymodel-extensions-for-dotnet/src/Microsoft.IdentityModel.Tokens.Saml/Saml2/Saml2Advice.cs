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

namespace Microsoft.IdentityModel.Tokens.Saml2
{
    /// <summary>
    /// Represents the Advice element specified in [Saml2Core, 2.6.1].
    /// see: http://docs.oasis-open.org/security/saml/v2.0/saml-core-2.0-os.pdf
    /// </summary>
    /// <remarks>
    /// This information MAY be ignored by applications without affecting either
    /// the semantics or the validity of the assertion. [Saml2Core, 2.6.1]
    /// </remarks>
    public class Saml2Advice
    {
        /// <summary>
        /// Creates an instance of Saml2Advice.
        /// </summary>
        public Saml2Advice()
        {
            AssertionIdReferences = new Collection<Saml2Id>();
            Assertions = new Collection<Saml2Assertion>();
            AssertionUriReferences = new AbsoluteUriCollection();
        }

        /// <summary>
        /// Gets a collection of <see cref="Saml2Id"/> representing the assertions in the <see cref="Saml2Advice"/>.
        /// </summary>
        public ICollection<Saml2Id> AssertionIdReferences
        {
            get;
        }

        /// <summary>
        /// Gets a collection of <see cref="Saml2Assertion"/> representing the assertions in the <see cref="Saml2Advice"/>.
        /// </summary>
        public ICollection<Saml2Assertion> Assertions
        {
            get;
        }

        /// <summary>
        /// Gets a collection of <see cref="Uri"/> representing the assertions in the <see cref="Saml2Advice"/>.
        /// </summary>
        public ICollection<Uri> AssertionUriReferences
        {
            get;
        }
    }
}