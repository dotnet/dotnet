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

namespace Microsoft.IdentityModel.Tokens.Saml
{
    /// <summary>
    /// Represents the Conditions element specified in [Saml, 2.3.2.1].
    /// </summary>
    public class SamlConditions
    {
        internal SamlConditions()
        {
            Conditions = new List<SamlCondition>();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SamlConditions"/>.
        /// </summary>
        /// <param name="notBefore">The earliest time instant at which the assertion is valid</param>
        /// <param name="notOnOrAfter">The time instant at which the assertion has expired.</param>
        public SamlConditions(DateTime notBefore, DateTime notOnOrAfter)
            : this(notBefore, notOnOrAfter, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SamlConditions"/>.
        /// </summary>
        /// <param name="notBefore">The earliest time instant at which the assertion is valid</param>
        /// <param name="notOnOrAfter">The time instant at which the assertion has expired.</param>
        /// <param name="conditions"><see cref="IEnumerable{SamlCondition}"/>.</param>
        public SamlConditions(DateTime notBefore, DateTime notOnOrAfter,
            IEnumerable<SamlCondition> conditions
            )
        {
            NotBefore = notBefore.ToUniversalTime();
            NotOnOrAfter = notOnOrAfter.ToUniversalTime();

            Conditions = (conditions == null) ? new List<SamlCondition>() : new List<SamlCondition>(conditions);
        }

        /// <summary>
        /// Gets a collection of <see cref="ICollection{SamlCondition}"/> that the assertion is addressed to.
        /// </summary>
        public ICollection<SamlCondition> Conditions { get; }

        /// <summary>
        /// Gets or sets the earliest time instant at which the assertion is valid. This value should be in UTC.
        /// </summary>
        public DateTime NotBefore { get; set; } = DateTimeUtil.GetMinValue(DateTimeKind.Utc);

        /// <summary>
        /// Gets or sets the time instant at which the assertion has expired. This value should be in UTC.
        /// </summary>
        public DateTime NotOnOrAfter { get; set; } = DateTimeUtil.GetMaxValue(DateTimeKind.Utc);
    }
}
