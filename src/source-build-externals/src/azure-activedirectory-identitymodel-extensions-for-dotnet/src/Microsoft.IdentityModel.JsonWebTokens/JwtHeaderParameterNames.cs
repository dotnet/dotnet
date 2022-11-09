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

namespace Microsoft.IdentityModel.JsonWebTokens
{
    /// <summary>
    /// List of header parameter names see: https://datatracker.ietf.org/doc/html/rfc7519#section-5.
    /// </summary>
    public struct JwtHeaderParameterNames
    {
        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7515#section-4.1.1
        /// </summary>
        public const string Alg = "alg";

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7515#section-4.1.10
        /// Also: https://datatracker.ietf.org/doc/html/rfc7519#section-5.2
        /// </summary>
        public const string Cty = "cty";

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7516#section-4.1.2
        /// </summary>
        public const string Enc = "enc";

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7518#section-4.7.1.1
        /// </summary>
        public const string IV = "iv";

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7515#section-4.1.2
        /// </summary>
        public const string Jku = "jku";

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7515#section-4.1.3
        /// </summary>
        public const string Jwk = "jwk";

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7515#section-4.1.4
        /// </summary>
        public const string Kid = "kid";

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7515#section-4.1.9
        /// Also: https://datatracker.ietf.org/doc/html/rfc7519#section-5.1
        /// </summary>
        public const string Typ = "typ";

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7515#section-4.1.6
        /// </summary>
        public const string X5c = "x5c";

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7515#page-12
        /// </summary>
        public const string X5t = "x5t";

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7515#section-4.1.5
        /// </summary>
        public const string X5u = "x5u";

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7516#section-4.1.3
        /// </summary>
        public const string Zip = "zip";

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7518#section-4.6.1.1
        /// </summary>
        public const string Epk = "epk";

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7518#section-4.6.1.2
        /// </summary>
        public const string Apu = "apu";

        /// <summary>
        /// See: https://datatracker.ietf.org/doc/html/rfc7518#section-4.6.1.3
        /// </summary>
        public const string Apv = "apv";
    }
}
