// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Benchmarks
{
    public class BenchmarkUtils
    {
        public const string Issuer = "http://www.contoso.com";

        public const string Audience = "http://www.contoso.com/protected";

        private static RSA _rsa;
        private static SymmetricSecurityKey _symmetricKey;

        public static RSA RSA
        {
            get
            {
                if (_rsa == null)
                {
                    _rsa = RSA.Create();
                    _rsa.KeySize = 2048;
                }

                return _rsa;
            }
        }

        public static RSAParameters RsaParameters => RSA.ExportParameters(true);

        public static RSAParameters RsaParametersPublic => RSA.ExportParameters(false);

        public static RsaSecurityKey RsaSecurityKey => new(RsaParameters) { KeyId = "RsaPrivate" };

        public static RsaSecurityKey RsaSecurityKeyPublic => new(RsaParametersPublic) { KeyId = "RsaPublic" };

        public static Dictionary<string, object> Claims
        {
            get
            {
                DateTime now = DateTime.UtcNow;
                return new Dictionary<string, object>()
                {
                    { "role", new List<string>() { "role1", "Developer", "Sales"} },
                    { JwtRegisteredClaimNames.Email, "Bob@contoso.com" },
                    { JwtRegisteredClaimNames.Exp, EpochTime.GetIntDate(now + TimeSpan.FromDays(1)) },
                    { JwtRegisteredClaimNames.Nbf, EpochTime.GetIntDate(now) },
                    { JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(now) },
                    { JwtRegisteredClaimNames.GivenName, "Bob" },
                    { JwtRegisteredClaimNames.Iss, Issuer },
                    { JwtRegisteredClaimNames.Aud, Audience }
                };
            }
        }

        public static Dictionary<string, object> ClaimsExtendedExample
        {
            get
            {
                DateTime now = DateTime.UtcNow;
                return new Dictionary<string, object>()
                {
                    { "acct", 0 },
                    { "aio", Guid.NewGuid().ToString() },
                    { "amr", new List<string>() { "pwd", "mfa" } },
                    { "app_displayname", "MyApp" },
                    { "appidacr", 0 },
                    { "azpacr", 0 },
                    { "azp", Guid.NewGuid().ToString() },
                    { "groups", new List<string>() { "group1", "group2" } },
                    { "name", "Bob" },
                    { "oid", Guid.NewGuid().ToString() },
                    { "rh", Guid.NewGuid().ToString() },
                    { "scp", "access_as_user" },
                    { JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString() },
                    { "tid", Guid.NewGuid().ToString() },
                    { "family_name", "Smith" },
                    { "ver", "2.0" },
                    { "wids", new List<string>() { Guid.NewGuid().ToString() } },
                    { "xms_cc", Guid.NewGuid().ToString() },
                    { "role", new List<string>() { "role1", "Developer", "Sales"} },
                    { JwtRegisteredClaimNames.Email, "Bob@contoso.com" },
                    { JwtRegisteredClaimNames.Exp, EpochTime.GetIntDate(now + TimeSpan.FromDays(1)) },
                    { JwtRegisteredClaimNames.Nbf, EpochTime.GetIntDate(now) },
                    { JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(now) },
                    { JwtRegisteredClaimNames.GivenName, "Bob" },
                    { JwtRegisteredClaimNames.Iss, Issuer },
                    { JwtRegisteredClaimNames.Aud, Audience }
                };
            }
        }

        public static SigningCredentials SigningCredentialsRsaSha256 => new(RsaSecurityKey, SecurityAlgorithms.RsaSha256, SecurityAlgorithms.Sha256);

        public static EncryptingCredentials EncryptingCredentialsAes256Sha512 => new(SymmetricEncryptionKey512, "dir", SecurityAlgorithms.Aes256CbcHmacSha512);

        public static SymmetricSecurityKey SymmetricEncryptionKey512
        {
            get
            {
                _symmetricKey ??= new SymmetricSecurityKey(SHA512.Create().ComputeHash(Guid.NewGuid().ToByteArray()));
                return _symmetricKey;
            }
        }

        public static string CreateCnfClaim(RsaSecurityKey key, string algorithm)
        {
            return "{\"jwk\":" + CreateJwkClaim(key, algorithm) + "}";
        }

        public static string CreateJwkClaim(RsaSecurityKey key, string algorithm)
        {
            RSAParameters rsaParameters = ((key.Rsa == null) ? key.Parameters : key.Rsa.ExportParameters(includePrivateParameters: false));
            return "{\"kty\":\"RSA\",\"n\":\"" +
                    Base64UrlEncoder.Encode(rsaParameters.Modulus) +
                    "\",\"e\":\"" +
                    Base64UrlEncoder.Encode(rsaParameters.Exponent) +
                    "\",\"alg\":\"" +
                    algorithm +
                    "\",\"kid\":\"" +
                    key.KeyId +
                    "\"}";
        }

        public static string CreateAccessTokenWithCnf()
        {
            Dictionary<string, object> claims = new Dictionary<string, object>(Claims);
            claims.Add("cnf", CreateCnfClaim(RsaSecurityKeyPublic, SecurityAlgorithms.RsaSha256));
            return new JsonWebTokenHandler().CreateToken(new SecurityTokenDescriptor
            {
                SigningCredentials = SigningCredentialsRsaSha256,
                Claims = claims,
                TokenType = JwtHeaderParameterNames.Jwk
            });
        }

        public static HttpRequestData HttpRequestData => new()
        {
            Method = "GET",
            Uri = new Uri("https://www.relyingparty.com")
        };
    }
}
