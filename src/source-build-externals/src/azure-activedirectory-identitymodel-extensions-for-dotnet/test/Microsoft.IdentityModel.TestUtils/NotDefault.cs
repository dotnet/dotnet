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
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.TestUtils
{
    /// <summary>
    /// Returns NON default token creation / validation artifacts:
    /// Claim
    /// ClaimIdentity
    /// ClaimPrincipal
    /// SecurityTokenDescriptor
    /// TokenValidationParameters
    /// </summary>
    public static class NotDefault
    {
        public static SigningCredentials AsymmetricSigningCredentials
        {
            get { return new SigningCredentials(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaSha256); }
        }

        public static SignatureProvider AsymmetricSignatureProvider
        {
            get { return CryptoProviderFactory.Default.CreateForSigning(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaSha256); }
        }

        public static SecurityKey AsymmetricSigningKey
        {
            get { return KeyingMaterial.RsaSecurityKey_2048; }
        }

        public static string Audience
        {
            get { return Default.Audience.Replace("Default", "NotDefault"); }
        }

        public static string AuthenticationType
        {
            get { return Default.AuthenticationType.Replace("Default", "NotDefault"); }
        }

        public static List<Claim> Claims
        {
            get
            {
                return new List<Claim>()
                {
                    new Claim(ClaimTypes.Country, "USA", ClaimValueTypes.String, Issuer, OriginalIssuer),
                    new Claim(ClaimTypes.Email, "user@contoso.com", ClaimValueTypes.String, Issuer, OriginalIssuer),
                    new Claim(ClaimTypes.GivenName, "Tony", ClaimValueTypes.String, Issuer, OriginalIssuer),
                    new Claim(ClaimTypes.HomePhone, "555.1212", ClaimValueTypes.String, Issuer, OriginalIssuer),
                    new Claim(ClaimTypes.Role, "Sales", ClaimValueTypes.String, Issuer, OriginalIssuer),
                };
            }
        }

        public static string AuthorizedParty { get { return "http://relyingparty.notazp.com"; } }

        public static ClaimsIdentity CaimsIdentity
        {
            get
            {
                return new ClaimsIdentity(Claims, AuthenticationType, NameClaimType, RoleClaimType)
                {
                    Label = ClaimsIdentityLabel
                };
            }
        }

        public static string ClaimsIdentityLabel
        {
            get {  return Default.ClaimsIdentityLabel.Replace("Default", "NotDefault"); }
        }

        public static string Issuer
        {
            get => Guid.NewGuid().ToString();
        }

        public static IEnumerable<string> Issuers
        {
            get => new List<string> { Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString() };
        }

        public static string NameClaimType
        {
            get { return Default.NameClaimType.Replace("Default", "NotDefault"); }
        }

        public static string OriginalIssuer
        {
            get { return Default.OriginalIssuer.Replace("Default", "NotDefault"); }
        }

        public static string RoleClaimType
        {
            get { return Default.RoleClaimType.Replace("Default", "NotDefault"); }
        }
        
        public static EncryptingCredentials SymmetricEncryptionCredentials
        {
            get { return new EncryptingCredentials(new SymmetricSecurityKey(KeyingMaterial.SymmetricKeyBytes2_256), "dir", SecurityAlgorithms.Aes128CbcHmacSha256); }
        }

        public static SecurityKey SymmetricEncryptionKey
        {
            get { return new SymmetricSecurityKey(KeyingMaterial.SymmetricKeyBytes2_256); }
        }

        public static SigningCredentials SymmetricSigningCredentials
        {
            get { return new SigningCredentials(new SymmetricSecurityKey(KeyingMaterial.SymmetricKeyBytes2_256), SecurityAlgorithms.HmacSha256); }
        }

        public static SecurityKey SymmetricSigningKey128
        {
            get { return new SymmetricSecurityKey(KeyingMaterial.SymmetricKeyBytes2_128) { KeyId = KeyingMaterial.SymmetricSecurityKey2_128.KeyId }; }
        }

        public static SecurityKey SymmetricSigningKey256
        {
            get { return new SymmetricSecurityKey(KeyingMaterial.SymmetricKeyBytes2_256) { KeyId = KeyingMaterial.SymmetricSecurityKey2_256.KeyId }; }
        }

        public static SecurityKey SymmetricSigningKey384
        {
            get { return new SymmetricSecurityKey(KeyingMaterial.SymmetricKeyBytes2_384) { KeyId = KeyingMaterial.SymmetricSecurityKey2_384.KeyId }; }
        }

        public static SecurityKey SymmetricSigningKey512
        {
            get { return new SymmetricSecurityKey(KeyingMaterial.SymmetricKeyBytes2_512) { KeyId = KeyingMaterial.SymmetricSecurityKey2_512.KeyId }; }
        }

        public static SecurityKey SymmetricSigningKey768
        {
            get { return new SymmetricSecurityKey(KeyingMaterial.SymmetricKeyBytes2_768) { KeyId = KeyingMaterial.SymmetricSecurityKey2_768.KeyId }; }
        }

        public static SecurityKey SymmetricSigningKey1024
        {
            get { return new SymmetricSecurityKey(KeyingMaterial.SymmetricKeyBytes2_1024) { KeyId = KeyingMaterial.SymmetricSecurityKey2_1024.KeyId }; }
        }

        public static TokenValidationParameters SymmetricEncrytpSignTokenValidationParameters
        {
            get { return TokenValidationParameters(SymmetricEncryptionKey, SymmetricSigningKey256); }
        }

        public static TokenValidationParameters TokenValidationParameters(SecurityKey encryptionKey, SecurityKey signingKey)
        {
            return new TokenValidationParameters
            {
                AuthenticationType = AuthenticationType,
                TokenDecryptionKey = encryptionKey,
                IssuerSigningKey = signingKey,
                ValidAudience = Audience,
                ValidIssuer = Issuer,
            };
        }
    }
}
