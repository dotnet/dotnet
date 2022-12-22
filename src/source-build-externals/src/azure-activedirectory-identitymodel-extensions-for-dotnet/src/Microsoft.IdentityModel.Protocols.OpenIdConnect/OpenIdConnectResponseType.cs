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

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect
{
    /// <summary>
    /// Response types for OpenIdConnect.
    /// </summary>
    /// <remarks>Can be used to determine the message type by consumers of an <see cref="OpenIdConnectMessage"/>.
    /// For example: OpenIdConnectMessageTests.Publics() sets <see cref="OpenIdConnectMessage.ResponseType"/>
    /// to <see cref="OpenIdConnectResponseType.CodeIdToken"/>.</remarks>
    public static class OpenIdConnectResponseType
    {
        /// <summary>
        /// Indicates 'code' type see: http://openid.net/specs/openid-connect-core-1_0.html#CodeFlowAuth.
        /// For Example: http://openid.net/specs/openid-connect-core-1_0.html#codeExample.
        /// </summary>
        public const string Code = "code";

        /// <summary>
        /// Indicates 'code id_token' type see: http://openid.net/specs/openid-connect-core-1_0.html#HybridAuthRequest.
        /// For Example: http://openid.net/specs/openid-connect-core-1_0.html#code-id_tokenExample.
        /// </summary>
        public const string CodeIdToken = "code id_token";

        /// <summary>
        /// Indicates 'code id_token token' type see: http://openid.net/specs/openid-connect-core-1_0.html#HybridAuthRequest.
        /// For Example: http://openid.net/specs/openid-connect-core-1_0.html#code-id_token-tokenExample.
        /// </summary>
        public const string CodeIdTokenToken = "code id_token token";

        /// <summary>
        /// Indicates 'code token' type see: http://openid.net/specs/openid-connect-core-1_0.html#HybridAuthRequest.
        /// For Example: http://openid.net/specs/openid-connect-core-1_0.html#code-tokenExample.
        /// </summary>
        public const string CodeToken = "code token";

        /// <summary>
        /// Indicates 'id_token' type see: http://openid.net/specs/openid-connect-core-1_0.html#HybridAuthRequest.
        /// For Example: http://openid.net/specs/openid-connect-core-1_0.html#id_tokenExample.
        /// </summary>
        public const string IdToken = "id_token";

        /// <summary>
        /// Indicates 'id_token token' type see: http://openid.net/specs/openid-connect-core-1_0.html#ImplicitFlowAuth.
        /// For Example: http://openid.net/specs/openid-connect-core-1_0.html#id_token-tokenExample.
        /// </summary>
        public const string IdTokenToken = "id_token token";

        /// <summary>
        /// Defined in OAuth v2 multiple response types 1.0 spec, included for completion.
        /// See: http://openid.net/specs/oauth-v2-multiple-response-types-1_0.html#OAuthResponseTypesReg.
        /// </summary>
        public const string None = "none";

        /// <summary>
        /// Defined in OAuth 2.0 spec, included for completion.
        /// See: https://datatracker.ietf.org/doc/html/rfc6749#section-11.3.2.
        /// </summary>
        public const string Token = "token";
    }
}
