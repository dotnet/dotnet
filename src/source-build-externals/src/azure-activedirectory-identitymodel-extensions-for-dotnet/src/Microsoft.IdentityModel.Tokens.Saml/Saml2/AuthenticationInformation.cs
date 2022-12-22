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
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
    /// <summary>
    /// The authentication information that an authority asserted when creating a token for a subject.
    /// </summary>
    public class AuthenticationInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationInformation"/> class.
        /// </summary>
        public AuthenticationInformation(Uri authenticationMethod, DateTime authenticationInstant)
        {
            AuthenticationMethod = authenticationMethod ?? throw LogHelper.LogArgumentNullException(nameof(authenticationMethod));
            AuthenticationInstant = authenticationInstant;
        }

        /// <summary>
        /// Gets or sets the address of the authority that created the token.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the AuthenticationMethod
        /// </summary>
        public Uri AuthenticationMethod
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the AuthenticationInstant. This value should be in UTC.
        /// </summary>
        public DateTime AuthenticationInstant { get; set; }

        /// <summary>
        /// Gets or sets the DNS name of the authority that created the token.
        /// </summary>
        public string DnsName { get; set; }

        /// <summary>
        /// Gets or sets the time that the session referred to in the session index MUST be considered ended. This value should be in UTC.
        /// </summary>
        public DateTime? NotOnOrAfter { get; set; }

        /// <summary>
        /// Gets or sets the session index that describes the session between the authority and the client.
        /// </summary>
        public string Session { get; set; }
    }
}
