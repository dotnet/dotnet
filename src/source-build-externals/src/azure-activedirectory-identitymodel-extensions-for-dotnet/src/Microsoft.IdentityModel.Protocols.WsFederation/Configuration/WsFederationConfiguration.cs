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
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Xml;

namespace Microsoft.IdentityModel.Protocols.WsFederation
{
    /// <summary>
    /// Contains WsFederation metadata that can be populated from a XML string.
    /// </summary>
    public class WsFederationConfiguration : BaseConfiguration
    {
        /// <summary>
        /// Initializes an new instance of <see cref="WsFederationConfiguration"/>.
        /// </summary>
        public WsFederationConfiguration()
        {
        }

        /// <summary>
        /// The <see cref="Xml.Signature"/> element that was found when reading metadata.
        /// </summary>
        public Signature Signature
        {
            get;
            set;
        }

        /// <summary>
        /// The <see cref="Tokens.SigningCredentials"/> that was used to sign the metadata.
        /// </summary>
        public SigningCredentials SigningCredentials
        {
            get;
            set;
        }

        /// <summary>
        /// Get the <see cref="IList{KeyInfo}"/> that the IdentityProvider indicates are to be used signing keys.
        /// </summary>
        public ICollection<KeyInfo> KeyInfos
        {
            get;
        } = new List<KeyInfo>();

        /// <summary>
        /// Gets or sets token endpoint.
        /// </summary>
        public string TokenEndpoint
        {
            get;
            set;
        }
    }
}
