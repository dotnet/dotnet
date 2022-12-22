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
using System.Collections.ObjectModel;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
    /// <summary>
    /// Represents the Subject element specified in [Saml2Core, 2.4.1].
    /// see: http://docs.oasis-open.org/security/saml/v2.0/saml-core-2.0-os.pdf
    /// </summary>
    /// <remarks>
    /// If the NameId is null and the SubjectConfirmations collection is empty,
    /// an InvalidOperationException will be thrown during serialization.
    /// </remarks>
    public class Saml2Subject
    {
        private Collection<Saml2SubjectConfirmation> _subjectConfirmations = new Collection<Saml2SubjectConfirmation>();

        /// <summary>
        /// Initialize an instance of <see cref="Saml2Subject"/>.
        /// </summary>
        internal Saml2Subject() { }

        /// <summary>
        /// Initializes an instance of <see cref="Saml2Subject"/> from a <see cref="Saml2NameIdentifier"/>.
        /// </summary>
        /// <param name="nameId">The <see cref="Saml2NameIdentifier"/> to use for initialization.</param>
        public Saml2Subject(Saml2NameIdentifier nameId)
        {
            NameId = nameId;
        }

        /// <summary>
        /// Initializes an instance of <see cref="Saml2Subject"/> from a <see cref="Saml2SubjectConfirmation"/>.
        /// </summary>
        /// <param name="subjectConfirmation">The <see cref="Saml2SubjectConfirmation"/> to use for initialization.</param>
        public Saml2Subject(Saml2SubjectConfirmation subjectConfirmation)
        {
            if (subjectConfirmation == null)
                throw LogArgumentNullException(nameof(subjectConfirmation));

            _subjectConfirmations.Add(subjectConfirmation);
        }

        /// <summary>
        /// Gets or sets the <see cref="Saml2NameIdentifier"/>. [Saml2Core, 2.4.1]
        /// </summary>
        public Saml2NameIdentifier NameId { get; set; }

        /// <summary>
        /// Gets a collection of <see cref="Saml2SubjectConfirmation"/> which can be used to validate and confirm the <see cref="Saml2Subject"/>. [Saml2Core, 2.4.1]
        /// </summary>
        /// <remarks>
        /// If more than one subject confirmation is provied, then satisfying any one of 
        /// them is sufficient to confirm the subject for the purpose of applying the 
        /// assertion.
        /// </remarks>
        public ICollection<Saml2SubjectConfirmation> SubjectConfirmations
        {
            get { return _subjectConfirmations; }
        }
    }
}
