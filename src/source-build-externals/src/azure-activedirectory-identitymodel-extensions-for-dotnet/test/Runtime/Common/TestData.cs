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
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace RuntimeCommon
{
    public static class TestData
    {
        public static string Audience { get => "http://Audience"; }

        public static string AuthenticationType { get => "LocalUser"; }

        public static IDictionary<string, object> ClaimsDictionary
        {
            get => new Dictionary<string, object>
            {
                { ClaimTypes.Country, "USA" },
                { ClaimTypes.NameIdentifier, "Bob" },
                { ClaimTypes.Email, "Bob@contoso.com" },
                { ClaimTypes.GivenName, "Bob" },
                { ClaimTypes.HomePhone, "555.1212" },
                { ClaimTypes.Role, "Developer" },
                { ClaimTypes.StreetAddress, "123AnyWhereStreet/r/nSomeTown/r/nUSA" },
                { ClaimsIdentity.DefaultNameClaimType, "Jean-Sébastien" }
            };
        }

        public static List<Claim> Claims
        {
            get => new List<Claim>
            {
                new Claim(ClaimTypes.Country, "USA", ClaimValueTypes.String, Issuer, OriginalIssuer),
                new Claim(ClaimTypes.NameIdentifier, "Bob", ClaimValueTypes.String, Issuer, OriginalIssuer),
                new Claim(ClaimTypes.Email, "Bob@contoso.com", ClaimValueTypes.String, Issuer, OriginalIssuer),
                new Claim(ClaimTypes.GivenName, "Bob", ClaimValueTypes.String, Issuer, OriginalIssuer),
                new Claim(ClaimTypes.HomePhone, "555.1212", ClaimValueTypes.String, Issuer, OriginalIssuer),
                new Claim(ClaimTypes.Role, "Developer", ClaimValueTypes.String, Issuer, OriginalIssuer),
                new Claim(ClaimTypes.StreetAddress, "123AnyWhereStreet/r/nSomeTown/r/nUSA", ClaimValueTypes.String, Issuer, OriginalIssuer),
                new Claim(ClaimsIdentity.DefaultNameClaimType, "Jean-Sébastien", ClaimValueTypes.String, Issuer, OriginalIssuer),
            };
        }

        public static string Issuer { get => "http://issuer.com"; }

        public static string OriginalIssuer { get => "http://originalIssuer.com"; }

        // SecurityTokenDescriptor does not have SigningCredentials set
        public static SecurityTokenDescriptor SecurityTokenDescriptor(SigningCredentials signingCredentials)
        {
            return new SecurityTokenDescriptor
            {
                Audience = Audience,
                Claims = ClaimsDictionary,
                Issuer = Issuer,
                SigningCredentials = signingCredentials,
                Subject = Subject
            };
        }

        public static RSAParameters RsaParameters_2048 => new RSAParameters
        {
            D = Base64UrlEncoder.DecodeBytes("C6EGZYf9U6RI5Z0BBoSlwy_gKumVqRx-dBMuAfPM6KVbwIUuSJKT3ExeL5P0Ky1b4p-j2S3u7Afnvrrj4HgVLnC1ks6rEOc2ne5DYQq8szST9FMutyulcsNUKLOM5cVromALPz3PAqE2OCLChTiQZ5XZ0AiH-KcG-3hKMa-g1MVnGW-SSmm27XQwRtUtFQFfxDuL0E0fyA9O9ZFBV5201ledBaLdDcPBF8cHC53Gm5G6FRX3QVpoewm3yGk28Wze_YvNl8U3hvbxei2Koc_b9wMbFxvHseLQrxvFg_2byE2em8FrxJstxgN7qhMsYcAyw1qGJY-cYX-Ab_1bBCpdcQ"),
            DP = Base64UrlEncoder.DecodeBytes("ErP3OpudePAY3uGFSoF16Sde69PnOra62jDEZGnPx_v3nPNpA5sr-tNc8bQP074yQl5kzSFRjRlstyW0TpBVMP0ocbD8RsN4EKsgJ1jvaSIEoP87OxduGkim49wFA0Qxf_NyrcYUnz6XSidY3lC_pF4JDJXg5bP_x0MUkQCTtQE"),
            DQ = Base64UrlEncoder.DecodeBytes("YbBsthPt15Pshb8rN8omyfy9D7-m4AGcKzqPERWuX8bORNyhQ5M8JtdXcu8UmTez0j188cNMJgkiN07nYLIzNT3Wg822nhtJaoKVwZWnS2ipoFlgrBgmQiKcGU43lfB5e3qVVYUebYY0zRGBM1Fzetd6Yertl5Ae2g2CakQAcPs"),
            Exponent = Base64UrlEncoder.DecodeBytes("AQAB"),
            InverseQ = Base64UrlEncoder.DecodeBytes("lbljWyVY-DD_Zuii2ifAz0jrHTMvN-YS9l_zyYyA_Scnalw23fQf5WIcZibxJJll5H0kNTIk8SCxyPzNShKGKjgpyZHsJBKgL3iAgmnwk6k8zrb_lqa0sd1QWSB-Rqiw7AqVqvNUdnIqhm-v3R8tYrxzAqkUsGcFbQYj4M5_F_4"),
            Modulus = Base64UrlEncoder.DecodeBytes("6-FrFkt_TByQ_L5d7or-9PVAowpswxUe3dJeYFTY0Lgq7zKI5OQ5RnSrI0T9yrfnRzE9oOdd4zmVj9txVLI-yySvinAu3yQDQou2Ga42ML_-K4Jrd5clMUPRGMbXdV5Rl9zzB0s2JoZJedua5dwoQw0GkS5Z8YAXBEzULrup06fnB5n6x5r2y1C_8Ebp5cyE4Bjs7W68rUlyIlx1lzYvakxSnhUxSsjx7u_mIdywyGfgiT3tw0FsWvki_KYurAPR1BSMXhCzzZTkMWKE8IaLkhauw5MdxojxyBVuNY-J_elq-HgJ_dZK6g7vMNvXz2_vT-SykIkzwiD9eSI9UWfsjw"),
            P = Base64UrlEncoder.DecodeBytes("_avCCyuo7hHlqu9Ec6R47ub_Ul_zNiS-xvkkuYwW-4lNnI66A5zMm_BOQVMnaCkBua1OmOgx7e63-jHFvG5lyrhyYEmkA2CS3kMCrI-dx0fvNMLEXInPxd4np_7GUd1_XzPZEkPxBhqf09kqryHMj_uf7UtPcrJNvFY-GNrzlJk"),
            Q = Base64UrlEncoder.DecodeBytes("7gvYRkpqM-SC883KImmy66eLiUrGE6G6_7Y8BS9oD4HhXcZ4rW6JJKuBzm7FlnsVhVGro9M-QQ_GSLaDoxOPQfHQq62ERt-y_lCzSsMeWHbqOMci_pbtvJknpMv4ifsQXKJ4Lnk_AlGr-5r5JR5rUHgPFzCk9dJt69ff3QhzG2c"),
        };

        public static RSAParameters RsaParameters_2048_Public => new RSAParameters
        {
            Exponent = Base64UrlEncoder.DecodeBytes("AQAB"),
            Modulus = Base64UrlEncoder.DecodeBytes("6-FrFkt_TByQ_L5d7or-9PVAowpswxUe3dJeYFTY0Lgq7zKI5OQ5RnSrI0T9yrfnRzE9oOdd4zmVj9txVLI-yySvinAu3yQDQou2Ga42ML_-K4Jrd5clMUPRGMbXdV5Rl9zzB0s2JoZJedua5dwoQw0GkS5Z8YAXBEzULrup06fnB5n6x5r2y1C_8Ebp5cyE4Bjs7W68rUlyIlx1lzYvakxSnhUxSsjx7u_mIdywyGfgiT3tw0FsWvki_KYurAPR1BSMXhCzzZTkMWKE8IaLkhauw5MdxojxyBVuNY-J_elq-HgJ_dZK6g7vMNvXz2_vT-SykIkzwiD9eSI9UWfsjw"),
        };


        public static RsaSecurityKey RsaSecurityKey_2048_Public => new RsaSecurityKey(RsaParameters_2048_Public) { KeyId = "RsaSecurityKey_2048_Public" };

        public static RsaSecurityKey RsaSecurityKey_2048 => new RsaSecurityKey(RsaParameters_2048) { KeyId = "RsaSecurityKey_2048" };

        public static SigningCredentials RsaSigningCredentials_2048Sha256  => new SigningCredentials(RsaSecurityKey_2048, SecurityAlgorithms.RsaSha256, SecurityAlgorithms.Sha256);
        public static SigningCredentials RsaSigningCredentials_2048Sha512 => new SigningCredentials(RsaSecurityKey_2048, SecurityAlgorithms.RsaSha512, SecurityAlgorithms.Sha512);


        // Symmetric
        public static string DefaultSymmetricKeyEncoded_256 = "Vbxq2mlbGJw8XH+ZoYBnUHmHga8/o/IduvU/Tht70iE=";
        public static byte[] DefaultSymmetricKeyBytes_256 = Convert.FromBase64String(DefaultSymmetricKeyEncoded_256);
        public static SymmetricSecurityKey DefaultSymmetricSecurityKey_256 = new SymmetricSecurityKey(DefaultSymmetricKeyBytes_256) { KeyId = "DefaultSymmetricSecurityKey_256" };
        public static SigningCredentials SymmetricSigningCreds_256Sha256 = new SigningCredentials(DefaultSymmetricSecurityKey_256, SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256);
        public static EncryptingCredentials SymmetricEncryptingCreds_Aes128Sha2 = new EncryptingCredentials(DefaultSymmetricSecurityKey_256, "dir", SecurityAlgorithms.Aes128CbcHmacSha256);

        // ECDsa
        public static ECDsa Ecdsa256 = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        public static ECDsa Ecdsa384 = ECDsa.Create(ECCurve.NamedCurves.nistP384);
        public static ECDsa Ecdsa512 = ECDsa.Create(ECCurve.NamedCurves.nistP521);

        public static SigningCredentials EcdSigningCredentials_2048Sha256 => new SigningCredentials(new ECDsaSecurityKey(Ecdsa256), SecurityAlgorithms.EcdsaSha256, SecurityAlgorithms.Sha256);

        public static SigningCredentials EcdSigningCredentials_2048Sha512 => new SigningCredentials(new ECDsaSecurityKey(Ecdsa512), SecurityAlgorithms.EcdsaSha512, SecurityAlgorithms.Sha256);

        public static TokenValidationParameters TokenValidationParameters(SecurityKey issuerSecurityKey)
        {
            return new TokenValidationParameters
            {
                IssuerSigningKey = issuerSecurityKey,
                ValidAudience = Audience,
                ValidIssuer = Issuer
            };
        }

        public static TokenValidationParameters RsaTokenValidationParameters_2048_Public => new TokenValidationParameters
        {
            IssuerSigningKey = RsaSecurityKey_2048_Public,
            ValidAudience = Audience,
            ValidIssuer = Issuer
        };

        public static ClaimsIdentity Subject { get => new ClaimsIdentity(Claims, AuthenticationType); }
    }
}
