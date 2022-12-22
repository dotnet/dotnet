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
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Saml2;

#if USING_SAML1
using Microsoft.IdentityModel.Tokens.Saml;
#endif

namespace Microsoft.IdentityModel.TestUtils
{
    /// <summary>
    /// Main purpose of this code is to serve up Identities
    /// ClaimPrincipal
    /// ClaimIdentiy
    /// Claim
    /// SamlTokens
    /// JwtTokens
    /// </summary>
    public static class IdentityUtilities
    {
        /// <summary>
        /// Computes the OIDC hash for a claim. Used for creating c_hash and at_hash claims
        /// </summary>
        /// <param name="item"></param>
        /// <param name="algorithm"></param>
        /// <returns></returns>
        public static string CreateHashClaim(string item, string algorithm)
        {
            HashAlgorithm hashAlgorithm = null;
            switch (algorithm)
            {
                case "SHA256":
                    hashAlgorithm = SHA256.Create();
                    break;
                case "SHA384":
                    hashAlgorithm = SHA384.Create();
                    break;
                case "SHA512":
                    hashAlgorithm = SHA512.Create();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Hash algorithm not known: " + algorithm);
            }

            byte[] hashBytes = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(item));
            return Base64UrlEncoder.Encode(hashBytes, 0, hashBytes.Length / 2);
        }

        public static JwtSecurityToken CreateJwtSecurityToken(string issuer = null, string originalIssuer = null)
        {
            string iss = issuer ?? Default.Issuer;
            string originalIss = originalIssuer ?? Default.OriginalIssuer;

            return new JwtSecurityToken(issuer, "http://www.contoso.com", ClaimSets.Simple(iss, originalIss));
        }

        public static JwtSecurityToken CreateJwtSecurityToken(string issuer, string audience, IEnumerable<Claim> claims, DateTime? nbf, DateTime? exp, DateTime? iat, SigningCredentials signingCredentials)
        {
            JwtPayload payload = new JwtPayload(issuer, audience, claims, nbf, exp, iat);
            JwtHeader header = (signingCredentials != null) ? new JwtHeader(signingCredentials) : new JwtHeader();
            return new JwtSecurityToken(header, payload, header.Base64UrlEncode(), payload.Base64UrlEncode(), "" );
        }

        public static string CreateEncodedSaml(SecurityTokenDescriptor tokenDescriptor, SecurityTokenHandler tokenHandler)
        {
            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }

        public static string CreateEncodedSaml2(SecurityTokenDescriptor tokenDescriptor, SecurityTokenHandler tokenHandler)
        {
             return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }

        public static string CreateEncodedJwt(SecurityTokenDescriptor tokenDescriptor, SecurityTokenHandler tokenHandler)
        {
            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }

#if NET452
        public static Saml2SecurityToken CreateSaml2Token(string issuer, string audience, IEnumerable<Claim> claims, DateTime? nbf, DateTime? exp, DateTime? iat, SigningCredentials signingCredentials)
        {
            return null;
        }

        public static Saml2SecurityToken CreateSaml2Token(SecurityTokenDescriptor securityTokenDescriptor, Saml2SecurityTokenHandler tokenHandler)
        {
            return tokenHandler.CreateToken(securityTokenDescriptor) as Saml2SecurityToken;
        }

        #if USING_SAML1
        public static SamlSecurityToken CreateSamlSecurityToken(string issuer, string audience, IEnumerable<Claim> claims, DateTime? nbf, DateTime? exp, DateTime? iat, SigningCredentials signingCredentials)
        {
            return null;
        }

        public static SamlSecurityToken CreateSamlSecurityToken(SecurityTokenDescriptor securityTokenDescriptor, SecurityTokenHandler tokenHandler)
        {
            return null;
        }
        #endif

#endif
    }
}
