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

namespace System.IdentityModel.Tokens.Jwt
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
        public const string Actort = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Actort;

        /// <summary>
        /// http://openid.net/specs/openid-connect-core-1_0.html#IDToken
        /// </summary>
        public const string Acr = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Acr;

        /// <summary>
        /// http://openid.net/specs/openid-connect-core-1_0.html#IDToken
        /// </summary>
        public const string Amr = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Amr;

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/rfc7519#section-4
        /// </summary>
        public const string Aud = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Aud;

        /// <summary>
        /// http://openid.net/specs/openid-connect-core-1_0.html#IDToken
        /// </summary>
        public const string AuthTime = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.AuthTime;

        /// <summary>
        /// http://openid.net/specs/openid-connect-core-1_0.html#IDToken
        /// </summary>
        public const string Azp = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Azp;

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </summary>
        public const string Birthdate = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Birthdate;

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#HybridIDToken
        /// </summary>
        public const string CHash = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.CHash;

        /// <summary>
        /// http://openid.net/specs/openid-connect-core-1_0.html#CodeIDToken
        /// </summary>
        public const string AtHash = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.AtHash;

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </summary>
        public const string Email = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Email;

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/rfc7519#section-4
        /// </summary>
        public const string Exp = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Exp;

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </summary>
        public const string Gender = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Gender;

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </summary>
        public const string FamilyName = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.FamilyName;

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </summary>
        public const string GivenName = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.GivenName;

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/rfc7519#section-4
        /// </summary>
        public const string Iat = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Iat;

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/rfc7519#section-4
        /// </summary>
        public const string Iss = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Iss;

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/rfc7519#section-4
        /// </summary>
        public const string Jti = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti;

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </summary>
        public const string Name = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Name;

        /// <summary>
        /// </summary>
        public const string NameId = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.NameId;

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#AuthRequest
        /// </summary>
        public const string Nonce = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Nonce;

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/rfc7519#section-4
        /// </summary>
        public const string Nbf = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Nbf;

        /// <summary>
        /// </summary>
        public const string Prn = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Prn;

        /// <summary>
        /// http://openid.net/specs/openid-connect-frontchannel-1_0.html#OPLogout
        /// </summary>
        public const string Sid = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sid;

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/rfc7519#section-4
        /// </summary>
        public const string Sub = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub;

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/rfc7519#section-5
        /// </summary>
        public const string Typ = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Typ;

        /// <summary>
        /// </summary>
        public const string UniqueName = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.UniqueName;

        /// <summary>
        /// </summary>
        public const string Website = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Website;
    }
}
