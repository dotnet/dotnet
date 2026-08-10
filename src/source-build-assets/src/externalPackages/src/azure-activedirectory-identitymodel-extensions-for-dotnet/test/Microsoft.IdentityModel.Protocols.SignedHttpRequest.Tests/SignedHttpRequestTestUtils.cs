// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.IdentityModel.Protocols.SignedHttpRequest.Tests
{
    public static class SignedHttpRequestTestUtils
    {
        internal static string DefaultEncodedAccessToken = CreateAt(DefaultCnfJwk, false);

        internal static string DefaultEncodedAccessTokenWithCnfThumprint = CreateAt(DefaultCnfJwkThumprint, false);

        internal static SigningCredentials DefaultSigningCredentials => new SigningCredentials(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaSha256, SecurityAlgorithms.Sha256) { CryptoProviderFactory = new CryptoProviderFactory() };

        internal static EncryptingCredentials DefaultEncryptingCredentials => KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2;

        internal static TokenValidationParameters DefaultTokenValidationParameters => new TokenValidationParameters()
        {
            IssuerSigningKey = KeyingMaterial.RsaSigningCreds_2048_Public.Key,
            ValidIssuer = Default.Issuer,
            ValidAudience = Default.Audience,
            TokenDecryptionKey = DefaultEncryptingCredentials.Key,
            ValidateLifetime = false,
        };

        internal static JObject DefaultAccessTokenPayload => new JObject
        {
            { JwtRegisteredClaimNames.Email, "Bob@contoso.com" },
            { JwtRegisteredClaimNames.GivenName, "Bob" },
            { JwtRegisteredClaimNames.Iss, Default.Issuer },
            { JwtRegisteredClaimNames.Aud, Default.Audience },
            { JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(Default.IssueInstant).ToString() },
            { JwtRegisteredClaimNames.Nbf, EpochTime.GetIntDate(Default.NotBefore).ToString()},
            { JwtRegisteredClaimNames.Exp, EpochTime.GetIntDate(Default.Expires).ToString() },
        };

        internal static JObject DefaultJwk => new JObject
        {
            { "kty", "RSA" },
            { "n",   Base64UrlEncoder.Encode(KeyingMaterial.RsaParameters_2048.Modulus)},
            { "e",  Base64UrlEncoder.Encode(KeyingMaterial.RsaParameters_2048.Exponent) },
            { JwtHeaderParameterNames.Alg, SecurityAlgorithms.RsaSha256 },
            { JwtHeaderParameterNames.Kid, KeyingMaterial.RsaSecurityKey_2048.InternalId }
        };

        internal static JObject InvalidJwk => new JObject
        {
            { "kty", "RSA" },
            { "e", "bad_data" },
            { JwtHeaderParameterNames.Alg, SecurityAlgorithms.RsaSha256 },
            { JwtHeaderParameterNames.Kid, KeyingMaterial.RsaSecurityKey_2048.InternalId }
        };

        internal static Cnf CnfJwk => new Cnf
        {
            JsonWebKey = new JsonWebKey(DefaultJwk.ToString(Formatting.None))
        };

        internal static JObject DefaultCnfJwk => new JObject
        {
            { JwtHeaderParameterNames.Jwk, DefaultJwk },
        };

        internal static JObject DefaultCnfJwkThumprint => new JObject
        {
            { JwtHeaderParameterNames.Kid, Base64UrlEncoder.Encode(new JsonWebKey(DefaultJwk.ToString(Formatting.None)).ComputeJwkThumbprint()) },
        };

        internal static Cnf CnfJwkThumprint => new Cnf
        {
            Kid = Base64UrlEncoder.Encode(new JsonWebKey(DefaultJwk.ToString(Formatting.None)).ComputeJwkThumbprint())
        };

        internal static JObject DefaultCnfJwkEcdsa => new JObject
        {
            { JwtHeaderParameterNames.Jwk, DefaultJwkEcdsa },
        };

        internal static Cnf CnfJwkEcdsa => new Cnf
        {
            JsonWebKey = new JsonWebKey(DefaultJwkEcdsa.ToString(Formatting.None))
        };

        internal static JObject DefaultCnfJwkEcdsaThumbprint => new JObject
        {
            { JwtHeaderParameterNames.Kid, Base64UrlEncoder.Encode(new JsonWebKey(DefaultJwkEcdsa.ToString(Formatting.None)).ComputeJwkThumbprint()) },
        };

        internal static Cnf CnfJwkEcdsaThumbprint => new Cnf
        {
            Kid = Base64UrlEncoder.Encode(new JsonWebKey(DefaultJwkEcdsa.ToString(Formatting.None)).ComputeJwkThumbprint())
        };

#if NET462
        internal static JObject DefaultJwkEcdsa => new JObject
        {
            { "kty", "EC" },
            { JwtHeaderParameterNames.Alg, SecurityAlgorithms.EcdsaSha256 },
            { JsonWebKeyParameterNames.Use, "sig" },
            { JsonWebKeyParameterNames.Crv, "P-256" },
            { JwtHeaderParameterNames.Kid, KeyingMaterial.Ecdsa256Key.KeyId },
            { JsonWebKeyParameterNames.X, "luR290c8sXxbOGhNquQ3J3rh763Os4D609cHK-L_5fA" },
            { JsonWebKeyParameterNames.Y, "tUqUwtaVHwc7_CXnuBrCpMQTF5BJKdFnw9_JkSIXWpQ" }
        };
#else
        internal static JObject DefaultJwkEcdsa => new JObject
        {
            { "kty", "EC" },
            { JwtHeaderParameterNames.Alg, SecurityAlgorithms.EcdsaSha256 },
            { JsonWebKeyParameterNames.Use, "sig" },
            { JsonWebKeyParameterNames.Crv, "P-256" },
            { JwtHeaderParameterNames.Kid, KeyingMaterial.Ecdsa256Key.KeyId },
            { JsonWebKeyParameterNames.X, Base64UrlEncoder.Encode(KeyingMaterial.Ecdsa256Parameters_Public.Q.X) },
            { JsonWebKeyParameterNames.Y, Base64UrlEncoder.Encode(KeyingMaterial.Ecdsa256Parameters_Public.Q.Y) }
        };
#endif
        internal static JObject DefaultJwe => new JObject
        {
            { JsonWebKeyParameterNames.Kty, JsonWebAlgorithmsKeyTypes.Octet },
            { JwtHeaderParameterNames.Alg, SecurityAlgorithms.Aes128CbcHmacSha256 },
            { JsonWebKeyParameterNames.K, KeyingMaterial.DefaultSymmetricKeyEncoded_256 }
        };

        internal static JObject DefaultCnfJwe => new JObject
        {
            { ConfirmationClaimTypes.Jwe, EncryptToken(DefaultJwe.ToString(Formatting.None)) },
        };

        internal static JObject DefaultJku => new JObject
        {
            { JwtHeaderParameterNames.Jku, "jku.json" },
        };

        internal static JObject DefaultJkuKid => new JObject
        {
            { JwtHeaderParameterNames.Jku, "jku.json" },
            { JwtHeaderParameterNames.Kid, KeyingMaterial.RsaSecurityKey_2048.KeyId }
        };

        internal static JObject DefaultKid => new JObject
        {
            { JwtHeaderParameterNames.Kid, KeyingMaterial.RsaSecurityKey_2048.KeyId }
        };

        internal static JObject DefaultSignedHttpRequestHeader => new JObject
        {
            { JwtHeaderParameterNames.Alg, SecurityAlgorithms.RsaSha256 },
            { JwtHeaderParameterNames.Typ, SignedHttpRequestConstants.TokenType },
            { JwtHeaderParameterNames.Kid, KeyingMaterial.RsaSecurityKey_2048.KeyId }
        };

        internal static JObject DefaultSignedHttpRequestPayload => new JObject
        {
            { SignedHttpRequestClaimTypes.At, DefaultEncodedAccessToken},
            { SignedHttpRequestClaimTypes.Ts, (long)(DateTime.UtcNow - EpochTime.UnixEpoch).TotalSeconds},
            { SignedHttpRequestClaimTypes.M, "GET"},
            { SignedHttpRequestClaimTypes.U, "www.contoso.com"},
            { SignedHttpRequestClaimTypes.P, "/path1"},
            { SignedHttpRequestClaimTypes.Q, "[[\"b\",\"a\",\"c\"],\"u4LgkGUWhP9MsKrEjA4dizIllDXluDku6ZqCeyuR-JY\"]" },
            { SignedHttpRequestClaimTypes.H, "[[\"content-type\",\"etag\"],\"P6z5XN4tTzHkfwe3XO1YvVUIurSuhvh_UG10N_j-aGs\"]" },
            { SignedHttpRequestClaimTypes.B, "ZK-O2gzHjpsCGped6sUL2EM20Z9T-uF07LCGMA88UFw" },
            { SignedHttpRequestClaimTypes.Nonce, "81da490f46c3494eba8c6e25a45a4d0f" },
            { ConfirmationClaimTypes.Cnf, JObject.Parse(SignedHttpRequestUtilities.CreateJwkClaim(JsonWebKeyConverter.ConvertFromRSASecurityKey(DefaultSigningCredentials.Key as RsaSecurityKey))) }
        };

        internal static string CreateAt(JObject cnf, bool encrypt, bool addCnf = true, bool addCnfAsString = false)
        {
            var accessToken = DefaultAccessTokenPayload;

            if (addCnf && !addCnfAsString)
                accessToken.Add(ConfirmationClaimTypes.Cnf, cnf);

            if (addCnf && addCnfAsString)
                accessToken.Add(ConfirmationClaimTypes.Cnf, cnf.ToString());

            if (encrypt)
                return new JsonWebTokenHandler().CreateToken(accessToken.ToString(Formatting.None), DefaultSigningCredentials, KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2, new Dictionary<string, object>() { { System.IdentityModel.Tokens.Jwt.JwtHeaderParameterNames.Typ, SignedHttpRequestConstants.TokenType } });

            return new JsonWebTokenHandler().CreateToken(accessToken.ToString(Formatting.None), DefaultSigningCredentials, new Dictionary<string, object>() { { System.IdentityModel.Tokens.Jwt.JwtHeaderParameterNames.Typ, SignedHttpRequestConstants.TokenType } });
        }

        internal static string EncryptToken(string innerJwt)
        {
            return new JsonWebTokenHandler().EncryptToken(innerJwt, DefaultEncryptingCredentials);
        }

        internal static JsonWebToken ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(JProperty newProperty)
        {
            JObject token = DefaultSignedHttpRequestPayload;

            if (token.ContainsKey(newProperty.Name))
                token.Property(newProperty.Name).Remove();

            if (newProperty.Value.Type != JTokenType.Null)
                token.Add(newProperty);

            return CreateDefaultSignedHttpRequestToken(token.ToString(Formatting.None));
        }

        internal static JsonWebToken CreateDefaultSignedHttpRequestToken(string signedHttpRequestTokenPayload)
        {
            var signingCredentials = SignedHttpRequestTestUtils.DefaultSigningCredentials;
            var popHeaderTyp = new Dictionary<string, object>() { { System.IdentityModel.Tokens.Jwt.JwtHeaderParameterNames.Typ, SignedHttpRequestConstants.TokenType } };
            return new JsonWebToken(new JsonWebTokenHandler().CreateToken(signedHttpRequestTokenPayload, signingCredentials, popHeaderTyp));
        }

        internal static string CalculateBase64UrlEncodedHash(string data)
        {
            return CalculateBase64UrlEncodedHash(Encoding.UTF8.GetBytes(data));
        }

        internal static string CalculateBase64UrlEncodedHash(byte[] bytes)
        {
            using (var hash = SHA256.Create())
            {
                var hashedBytes = hash.ComputeHash(bytes);
                return Base64UrlEncoder.Encode(hashedBytes);
            }
        }

        internal static HttpHeaders CreateHttpHeaders(List<KeyValuePair<string, string>> headerKeyValuePairs)
        {
            using (var client = new HttpClient())
            {
                var headers = client.DefaultRequestHeaders;
                foreach (var headerKeyValuePair in headerKeyValuePairs)
                    headers.Add(headerKeyValuePair.Key, headerKeyValuePair.Value);

                return headers;
            }
        }

        internal static HttpRequestMessage CreateHttpRequestMessage(HttpMethod method, Uri uri, List<KeyValuePair<string, string>> headers, byte[] content, List<KeyValuePair<string, string>> contentHeaders = null)
        {
            var message = new HttpRequestMessage()
            {
                RequestUri = uri,
                Method = method,
            };

            if (content != null)
            {
                message.Content = new ByteArrayContent(content);
                message.Content.Headers.ContentLength = content.Length;
            }

            foreach (var header in headers)
            {
                message.Headers.Add(header.Key, header.Value);
            }

            if (contentHeaders != null)
                foreach (var contentHeader in contentHeaders)
                    message.Content.Headers.Add(contentHeader.Key, contentHeader.Value);

            return message;
        }
    }
}

