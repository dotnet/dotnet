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

namespace Microsoft.IdentityModel.Tokens.Saml
{
    /// <summary>
    /// Represents the Advice element specified in [Saml, 2.3.2.2].
    /// </summary>
    /// <remarks>
    /// This information MAY be ignored by applications without affecting either
    /// the semantics or the validity of the assertion.
    /// </remarks>
    public class SamlAdvice
    {
        /// <summary>
        /// Creates an instance of <see cref="SamlAdvice"/>.
        /// </summary>
        public SamlAdvice()
            : this(null, null)
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="SamlAdvice"/>.
        /// </summary>
        /// <param name="references"><see cref="IEnumerable{String}"/>.</param>
        public SamlAdvice(IEnumerable<string> references)
            : this(references, null)
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="SamlAdvice"/>.
        /// </summary>
        /// <param name="assertions"><see cref="IEnumerable{SamlAssertion}"/></param>
        public SamlAdvice(IEnumerable<SamlAssertion> assertions)
            : this(null, assertions)
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="SamlAdvice"/>.
        /// </summary>
        /// <param name="references"><see cref="IEnumerable{String}"/>.</param>
        /// <param name="assertions"><see cref="IEnumerable{SamlAssertion}"/>.</param>
        public SamlAdvice(IEnumerable<string> references, IEnumerable<SamlAssertion> assertions)
        {
            AssertionIdReferences = (references != null) ? new List<string>(references) : new List<string>();
            Assertions = (assertions != null) ? new List<SamlAssertion>(assertions) : new List<SamlAssertion>();
        }

        /// <summary>
        /// Gets a collection of <see cref="ICollection{String}"/> representing the assertions in the <see cref="SamlAdvice"/>.
        /// </summary>
        public ICollection<string> AssertionIdReferences
        {
            get;
        }

        /// <summary>
        /// Gets a collection of <see cref="ICollection{SamlAssertion}"/> representating the assertions in the <see cref="SamlAdvice"/>.
        /// </summary>
        public ICollection<SamlAssertion> Assertions
        {
            get;
        }
    }
}

