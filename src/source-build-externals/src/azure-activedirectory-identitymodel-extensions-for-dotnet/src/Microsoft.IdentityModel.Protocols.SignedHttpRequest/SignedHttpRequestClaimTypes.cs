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

using Microsoft.IdentityModel.JsonWebTokens;

namespace Microsoft.IdentityModel.Protocols.SignedHttpRequest
{
    /// <summary>
    /// Claim types used with SignedHttpRequest.
    /// </summary>
    public static class SignedHttpRequestClaimTypes
    {
        /// <summary>
        /// https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-3
        /// </summary>
        public const string At = "at";

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-3
        /// </summary>
        public const string Ts = "ts";

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-3
        /// </summary>
        public const string M = "m";

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-3
        /// </summary>
        public const string U = "u";

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-3
        /// </summary>
        public const string P = "p";

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-3
        /// </summary>
        public const string Q = "q";

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-3
        /// </summary>
        public const string H = "h";

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-3
        /// </summary>
        public const string B = "b";

        /// <summary>
        /// Default "nonce" claim.
        /// </summary>
        public const string Nonce = JwtRegisteredClaimNames.Nonce;
    }
}
