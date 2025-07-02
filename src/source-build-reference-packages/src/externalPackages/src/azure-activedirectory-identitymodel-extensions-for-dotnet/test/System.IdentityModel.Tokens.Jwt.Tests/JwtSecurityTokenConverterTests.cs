// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace System.IdentityModel.Tokens.Jwt.Tests
{
    public class JwtSecurityTokenConverterTests
    {
        [Fact]
        public void JwtSecurityTokenConverter_ThrowsOnNull()
        {
            Assert.Throws<ArgumentNullException>(() => JwtSecurityTokenConverter.Convert(null));
        }

        [Theory, MemberData(nameof(ConverterTheoryData))]
        public void JsonWebTokenToJwtSecurityTokenConversions(JwtSecurityTokenConverterTheoryData theoryData)
        {
            var output = JwtSecurityTokenConverter.Convert(theoryData.InputToken);
            Assert.NotNull(output);
            theoryData.Validator(output);
        }

        public static TheoryData<JwtSecurityTokenConverterTheoryData> ConverterTheoryData()
        {
            var tokenDescriptorJwe = new SecurityTokenDescriptor
            {
                SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                EncryptingCredentials = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes256_Sha512_512,
                Claims = Default.PayloadDictionary
            };

            var tokenDescriptorJws = new SecurityTokenDescriptor
            {
                SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                Claims = Default.PayloadDictionary
            };

            var handler = new JsonWebTokenHandler();
            var jweTokenString = handler.CreateToken(tokenDescriptorJwe);
            var jwsTokenString = handler.CreateToken(tokenDescriptorJws);

            var validationParameters = new TokenValidationParameters
            {
                RequireExpirationTime = false,
                ValidateAudience = false,
                ValidateIssuer = false,
                TokenDecryptionKey = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes256_Sha512_512.Key,
                IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key
            };

            var result = handler.ValidateTokenAsync(jweTokenString, validationParameters).Result;
            var jweToken = result.SecurityToken as JsonWebToken;

            result = handler.ValidateTokenAsync(jwsTokenString, validationParameters).Result;
            var jwsTokenFromString = result.SecurityToken as JsonWebToken;

            var jwsTokenFromHeaderAndPayload = new JsonWebToken(
                Default.PayloadString,
                new JObject
                {
                    { JwtHeaderParameterNames.Alg, SecurityAlgorithms.Sha512  },
                    { JwtHeaderParameterNames.Kid, Default.AsymmetricSigningKey.KeyId },
                    { JwtHeaderParameterNames.Typ, JwtConstants.HeaderType }
                }.ToString(Formatting.None));

            return new TheoryData<JwtSecurityTokenConverterTheoryData>
            {
                new JwtSecurityTokenConverterTheoryData
                {
                    First = true,
                    TestId = "JweToJwe",
                    InputToken = jweToken,
                    Validator = (token) =>
                    {
                        Assert.NotNull(token.InnerToken);

                        foreach (var header in jweToken.Header.Claims((string)Default.PayloadDictionary[JwtRegisteredClaimNames.Iss]))
                        {
                            Assert.True(token.Header.ContainsKey(header.Type));
                            var otherHeader = token.Header[header.Type];
                            Assert.Equal(header.Value, otherHeader);
	                    }

                        foreach (var header in jweToken.InnerToken.Header.Claims((string)Default.PayloadDictionary[JwtRegisteredClaimNames.Iss]))
                        {
                            Assert.True(token.InnerToken.Header.ContainsKey(header.Type));
                            var otherHeader = token.InnerToken.Header[header.Type];
                            Assert.Equal(header.Value, otherHeader);
                        }

                        var jweTokenClaims = new List<Security.Claims.Claim>(jweToken.Claims).ToDictionary(c => c.Type, c => c.Value);
                        var otherTokenClaims = new List<Security.Claims.Claim>(token.Claims).ToDictionary(c => c.Type, c => c.Value);;

                        Assert.Equal(jweTokenClaims.Count, otherTokenClaims.Count);

                        foreach (var claim in jweTokenClaims)
                        {
                            Assert.True(otherTokenClaims.ContainsKey(claim.Key));
                            Assert.Equal(otherTokenClaims[claim.Key], claim.Value);
                        }

                    }
                },
                new JwtSecurityTokenConverterTheoryData
                {
                    TestId = "JwsCreatedFromString",
                    InputToken = jwsTokenFromString,
                    Validator = (token) =>
                    {
                        Assert.Null(token.InnerToken);

                        foreach (var header in jwsTokenFromString.Header.Claims((string)Default.PayloadDictionary[JwtRegisteredClaimNames.Iss]))
                        {
                            Assert.True(token.Header.ContainsKey(header.Type));
                            var otherHeader = token.Header[header.Type];
                            Assert.Equal(header.Value, otherHeader);
                        }

                        var jwsTokenFromStringClaims = new List<Security.Claims.Claim>(jwsTokenFromString.Claims).ToDictionary(c => c.Type, c => c.Value);
                        var otherTokenClaims = new List<Security.Claims.Claim>(token.Claims).ToDictionary(c => c.Type, c => c.Value);;

                        Assert.Equal(jwsTokenFromStringClaims.Count, otherTokenClaims.Count);

                        foreach (var claim in jwsTokenFromStringClaims)
                        {
                            Assert.True(otherTokenClaims.ContainsKey(claim.Key));
                            Assert.Equal(otherTokenClaims[claim.Key], claim.Value);
                        }
                    }
                },
                new JwtSecurityTokenConverterTheoryData
                {
                    TestId = "JwsCreatedFromHeaderAndPayload",
                    InputToken = jwsTokenFromHeaderAndPayload,
                    Validator = (token) =>
                    {
                        Assert.Null(token.InnerToken);

                        foreach (var header in jwsTokenFromHeaderAndPayload.Header.Claims((string)Default.PayloadDictionary[JwtRegisteredClaimNames.Iss]))
                        {
                            Assert.True(token.Header.ContainsKey(header.Type));
                            var otherHeader = token.Header[header.Type];
                            Assert.Equal(header.Value, otherHeader);
                        }

                        var jwsTokenFromHeaderAndPayloadClaims = new List<Security.Claims.Claim>(jwsTokenFromHeaderAndPayload.Claims).ToDictionary(c => c.Type, c => c.Value);
                        var otherTokenClaims = new List<Security.Claims.Claim>(token.Claims).ToDictionary(c => c.Type, c => c.Value);;

                        Assert.Equal(jwsTokenFromHeaderAndPayloadClaims.Count, otherTokenClaims.Count);

                        foreach (var claim in jwsTokenFromHeaderAndPayloadClaims)
                        {
                            Assert.True(otherTokenClaims.ContainsKey(claim.Key));
                            Assert.Equal(otherTokenClaims[claim.Key], claim.Value);
                        }
                    }
                }
            };
        }


        public class JwtSecurityTokenConverterTheoryData : TheoryDataBase
        {
            public JsonWebToken InputToken { get; set; }

            public Action<JwtSecurityToken> Validator { get; set; }
        }
    }
}
