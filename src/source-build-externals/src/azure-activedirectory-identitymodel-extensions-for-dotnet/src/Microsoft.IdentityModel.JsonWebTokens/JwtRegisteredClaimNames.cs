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
    /// List of registered claims from different sources
    /// https://datatracker.ietf.org/doc/html/rfc7519#section-4
    /// http://openid.net/specs/openid-connect-core-1_0.html#IDToken
    /// </summary>
    public struct JwtRegisteredClaimNames
    {
        /// <summary>
        /// </summary>
        public const string Actort = "actort";

        /// <summary>
        /// http://openid.net/specs/openid-connect-core-1_0.html#IDToken
        /// </summary>
        public const string Acr = "acr";

        /// <summary>
        /// http://openid.net/specs/openid-connect-core-1_0.html#IDToken
        /// </summary>
        public const string Amr = "amr";

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/rfc7519#section-4
        /// </summary>
        public const string Aud = "aud";

        /// <summary>
        /// http://openid.net/specs/openid-connect-core-1_0.html#IDToken
        /// </summary>
        public const string AuthTime = "auth_time";

        /// <summary>
        /// http://openid.net/specs/openid-connect-core-1_0.html#IDToken
        /// </summary>
        public const string Azp = "azp";

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </summary>
        public const string Birthdate = "birthdate";

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#HybridIDToken
        /// </summary>
        public const string CHash = "c_hash";

        /// <summary>
        /// http://openid.net/specs/openid-connect-core-1_0.html#CodeIDToken
        /// </summary>
        public const string AtHash = "at_hash";

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </summary>
        public const string Email = "email";

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/rfc7519#section-4
        /// </summary>
        public const string Exp = "exp";

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </summary>
        public const string Gender = "gender";

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </summary>
        public const string FamilyName = "family_name";

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </summary>
        public const string GivenName = "given_name";

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/rfc7519#section-4
        /// </summary>
        public const string Iat = "iat";

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/rfc7519#section-4
        /// </summary>
        public const string Iss = "iss";

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/rfc7519#section-4
        /// </summary>
        public const string Jti = "jti";

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </summary>
        public const string Name = "name";

        /// <summary>
        /// </summary>
        public const string NameId = "nameid";

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#AuthRequest
        /// </summary>
        public const string Nonce = "nonce";

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/rfc7519#section-4
        /// </summary>
        public const string Nbf = "nbf";

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </summary>
        public const string PhoneNumber = "phone_number";

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </summary>
        public const string PhoneNumberVerified = "phone_number_verified";

        /// <summary>
        /// </summary>
        public const string Prn = "prn";

        /// <summary>
        /// http://openid.net/specs/openid-connect-frontchannel-1_0.html#OPLogout
        /// </summary>
        public const string Sid = "sid";

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/rfc7519#section-4
        /// </summary>
        public const string Sub = "sub";

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/rfc7519#section-5
        /// </summary>
        public const string Typ = "typ";

        /// <summary>
        /// </summary>
        public const string UniqueName = "unique_name";

        /// <summary>
        /// </summary>
        public const string Website = "website";
    }
}
