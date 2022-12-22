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
using System.IdentityModel.Tokens.Jwt;

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect
{
    /// <summary>
    /// A context that is used by a <see cref="OpenIdConnectProtocolValidator"/> when validating an OpenIdConnect Response
    /// to ensure it compliant with http://openid.net/specs/openid-connect-core-1_0.html.
    /// </summary>
    public class OpenIdConnectProtocolValidationContext
    {
        /// <summary>
        /// Creates an instance of <see cref="OpenIdConnectProtocolValidationContext"/>
        /// </summary>
        public OpenIdConnectProtocolValidationContext() {}

        /// <summary>
        /// Gets or sets the 'client_id'.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the 'nonce' that was sent with the 'Request'.
        /// </summary>
        public string Nonce { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="OpenIdConnectMessage"/> that represents the 'Response'.
        /// </summary>
        public OpenIdConnectMessage ProtocolMessage { get; set; }

        /// <summary>
        /// Gets or sets the state that was sent with the 'Request'.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the response received from userinfo_endpoint.
        /// </summary>
        public string UserInfoEndpointResponse { get; set; }

        /// <summary>
        /// This id_token is assumed to have audience, issuer, lifetime and signature validated.
        /// </summary>
        public JwtSecurityToken ValidatedIdToken { get; set; }
    }
}
