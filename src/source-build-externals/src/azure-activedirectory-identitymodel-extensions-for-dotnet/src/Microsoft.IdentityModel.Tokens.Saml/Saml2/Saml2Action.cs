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
    /// Represents the Action element specified in [Saml2Core, 2.7.4.2].
    /// see: http://docs.oasis-open.org/security/saml/v2.0/saml-core-2.0-os.pdf
    /// </summary>
    public class Saml2Action
    {
        private Uri _namespace;
        private string _value;

        /// <summary>
        /// Constructs an instance of Saml2Action class.
        /// </summary>
        /// <param name="value">Value represented by this class.</param>
        /// <param name="namespace">Namespace in which the action is interpreted.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="value"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="namespace"/> is null.</exception>
        /// <exception cref="ArgumentException">if <paramref name="namespace"/> is not an absolute Uri.</exception>
        public Saml2Action(string value, Uri @namespace)
        {
            // ==
            // There is a discrepancy between the schema and the text of the
            // specification as to whether the Namespace attribute is optional
            // or required. The schema specifies required.
            // ==
            // Per the SAML 2.0 errata the schema takes precedence over the text, 
            // and the namespace attribute is required. This is errata item E36.
            // ==
            // SAML 2.0 errata at the time of this implementation:
            // http://docs.oasis-open.org/security/saml/v2.0/sstc-saml-approved-errata-2.0-cd-02.pdf
            // ==

            Namespace = @namespace;
            Value = value;
        }

        /// <summary>
        /// Gets or sets a URI reference representing the namespace in which the name of the
        /// specified action is to be interpreted. [Saml2Core, 2.7.4.2]
        /// </summary>
        /// <exception cref="ArgumentNullException">if 'value' is null.</exception>
        /// <exception cref="ArgumentException">if 'value' is not an absolute Uri.</exception>
        public Uri Namespace
        {
            get => _namespace;
            set
            {
                // See note in constructor about why this is required.
                if (value == null)
                    throw LogArgumentNullException(nameof(value));

                if (!value.IsAbsoluteUri)
                    throw LogExceptionMessage(new ArgumentException(FormatInvariant(LogMessages.IDX13300, MarkAsNonPII(nameof(Namespace)), value), nameof(value)));

                _namespace = value;
            }
        }

        /// <summary>
        /// Gets or sets the label for an action sought to be performed on the 
        /// specified resource. [Saml2Core, 2.7.4.2]
        /// </summary>
        /// <exception cref="ArgumentNullException">if 'value' is null or empty.</exception>
        public string Value
        {
            get => _value;
            set => _value = (string.IsNullOrEmpty(value)) ? throw LogArgumentNullException(nameof(value)) : value;
        }
    }
}
