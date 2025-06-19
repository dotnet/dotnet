// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

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
    }
}
