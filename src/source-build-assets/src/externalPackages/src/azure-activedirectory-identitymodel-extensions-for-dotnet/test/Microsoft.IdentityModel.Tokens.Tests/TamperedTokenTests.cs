// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Microsoft.IdentityModel.Tokens.Tests
{
    public class TamperedTokenTests
    {
        [Theory, MemberData(nameof(JwtSignatureTruncationTheoryData), DisableDiscoveryEnumeration = true)]
        public async Task JwtSignatureTruncation(ValidateTokenTheoryData theoryData)
        {
            CompareContext compareContext = TestUtilities.WriteHeader($"{this}.JwtSignatureTruncation", theoryData);
            JsonWebToken jsonWebToken = new JsonWebToken(theoryData.JsonWebToken);
            JsonWebTokenHandler jsonWebTokenHandler = new JsonWebTokenHandler();
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            TokenValidationResult tokenValidationResult;

            try
            {
                tokenValidationResult = await jsonWebTokenHandler.ValidateTokenAsync(theoryData.JsonWebToken, theoryData.ValidationParameters);
                if (!tokenValidationResult.IsValid)
                    compareContext.AddDiff($"JsonWebTokenHandler.ValidateTokenAsync IS NOT VALID with untampered token. The token should have validated");

                tokenValidationResult = await jwtSecurityTokenHandler.ValidateTokenAsync(theoryData.JsonWebToken, theoryData.ValidationParameters);
                if (!tokenValidationResult.IsValid)
                    compareContext.AddDiff($"JwtSecurityTokenHandler.ValidateTokenAsync IS NOT VALID with untampered token. The token should have validated");

                theoryData.ExpectedException.ProcessNoException(compareContext);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, compareContext);
            }

            for (int i = 1; i < jsonWebToken.EncodedSignature.Length; i++)
            {
                try
                {
                    string token = theoryData.JsonWebToken.Substring(0, theoryData.JsonWebToken.Length - i);
                    tokenValidationResult = await jsonWebTokenHandler.ValidateTokenAsync(token, theoryData.ValidationParameters);
                    if (tokenValidationResult.IsValid)
                        compareContext.AddDiff($"jsonWebTokenHandler.ValidateTokenAsync, tokenValidationResult.IsValid, index:'{i}'.");

                    tokenValidationResult = await jwtSecurityTokenHandler.ValidateTokenAsync(token, theoryData.ValidationParameters);
                    if (tokenValidationResult.IsValid)
                        compareContext.AddDiff($"jwtSecurityTokenHandler.ValidateTokenAsync, tokenValidationResult.IsValid, index:'{i}'.");

                    theoryData.ExpectedException.ProcessNoException(compareContext);
                }
                catch (Exception ex)
                {
                    theoryData.ExpectedException.ProcessException(ex, compareContext);
                }
            }

            TestUtilities.AssertFailIfErrors(compareContext);
        }

        public static TheoryData<ValidateTokenTheoryData> JwtSignatureTruncationTheoryData()
        {
            var theoryData = new TheoryData<ValidateTokenTheoryData>();

            JsonWebTokenHandler jsonWebTokenHandler = new JsonWebTokenHandler();
            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "https://idp.com",
                Claims = new Dictionary<string, object>
                {
                    { JwtRegisteredClaimNames.Aud, "https://relyingparty.com" },
                    { JwtRegisteredClaimNames.Email, "bob@contoso.com" },
                    { JwtRegisteredClaimNames.GivenName, "bob" },
                    { JwtRegisteredClaimNames.Sub, "123456789" }
                }
            };

            // ECD - Key - 256
            theoryData.Add(BuildTestCase("ES256_Key256", KeyingMaterial.Ecdsa256Key, SecurityAlgorithms.EcdsaSha256, securityTokenDescriptor, jsonWebTokenHandler));
            theoryData.Add(BuildTestCase("ES384_Key256", KeyingMaterial.Ecdsa256Key, SecurityAlgorithms.EcdsaSha384, securityTokenDescriptor, jsonWebTokenHandler));
            theoryData.Add(BuildTestCase("ES512_Key256", KeyingMaterial.Ecdsa256Key, SecurityAlgorithms.EcdsaSha512, securityTokenDescriptor, jsonWebTokenHandler));

            // ECD - Key - 384
            theoryData.Add(BuildTestCase("ES256_Key384", KeyingMaterial.Ecdsa384Key, SecurityAlgorithms.EcdsaSha256, securityTokenDescriptor, jsonWebTokenHandler));
            theoryData.Add(BuildTestCase("ES384_Key384", KeyingMaterial.Ecdsa384Key, SecurityAlgorithms.EcdsaSha384, securityTokenDescriptor, jsonWebTokenHandler));
            theoryData.Add(BuildTestCase("ES512_Key384", KeyingMaterial.Ecdsa384Key, SecurityAlgorithms.EcdsaSha512, securityTokenDescriptor, jsonWebTokenHandler));

            // ECD - Key - 521
            theoryData.Add(BuildTestCase("ES256_Key521", KeyingMaterial.Ecdsa521Key, SecurityAlgorithms.EcdsaSha256, securityTokenDescriptor, jsonWebTokenHandler));
            theoryData.Add(BuildTestCase("ES384_Key521", KeyingMaterial.Ecdsa521Key, SecurityAlgorithms.EcdsaSha384, securityTokenDescriptor, jsonWebTokenHandler));
            theoryData.Add(BuildTestCase("ES512_Key521", KeyingMaterial.Ecdsa521Key, SecurityAlgorithms.EcdsaSha512, securityTokenDescriptor, jsonWebTokenHandler));

            // RSA - Key - 2048
            theoryData.Add(BuildTestCase("RS256_Key2048", KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaSha256, securityTokenDescriptor, jsonWebTokenHandler));
            theoryData.Add(BuildTestCase("RS384_Key2048", KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaSha384, securityTokenDescriptor, jsonWebTokenHandler));
            theoryData.Add(BuildTestCase("RS512_Key2048", KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaSha512, securityTokenDescriptor, jsonWebTokenHandler));

            // RSA - Key - 4096
            theoryData.Add(BuildTestCase("RS256_Key4096", KeyingMaterial.RsaSecurityKey_4096, SecurityAlgorithms.RsaSha256, securityTokenDescriptor, jsonWebTokenHandler));
            theoryData.Add(BuildTestCase("RS384_Key4096", KeyingMaterial.RsaSecurityKey_4096, SecurityAlgorithms.RsaSha384, securityTokenDescriptor, jsonWebTokenHandler));
            theoryData.Add(BuildTestCase("RS512_Key4096", KeyingMaterial.RsaSecurityKey_4096, SecurityAlgorithms.RsaSha512, securityTokenDescriptor, jsonWebTokenHandler));

            // Symmetric - Key - 256
            theoryData.Add(BuildTestCase("Hmac256_Key256", KeyingMaterial.SymmetricSecurityKey2_256, SecurityAlgorithms.HmacSha256, securityTokenDescriptor, jsonWebTokenHandler));
            theoryData.Add(BuildTestCase("Hmac384_Key256", KeyingMaterial.SymmetricSecurityKey2_384, SecurityAlgorithms.HmacSha384, securityTokenDescriptor, jsonWebTokenHandler));
            theoryData.Add(BuildTestCase("Hmac512_Key256", KeyingMaterial.SymmetricSecurityKey2_512, SecurityAlgorithms.HmacSha512, securityTokenDescriptor, jsonWebTokenHandler));

            // Symmetric - Key - 384
            theoryData.Add(BuildTestCase("Hmac256_Key384", KeyingMaterial.SymmetricSecurityKey2_256, SecurityAlgorithms.HmacSha256, securityTokenDescriptor, jsonWebTokenHandler));
            theoryData.Add(BuildTestCase("Hmac384_Key384", KeyingMaterial.SymmetricSecurityKey2_384, SecurityAlgorithms.HmacSha384, securityTokenDescriptor, jsonWebTokenHandler));
            theoryData.Add(BuildTestCase("Hmac512_Key384", KeyingMaterial.SymmetricSecurityKey2_512, SecurityAlgorithms.HmacSha512, securityTokenDescriptor, jsonWebTokenHandler));

            // Symmetric - Key - 512
            theoryData.Add(BuildTestCase("Hmac256_Key512", KeyingMaterial.SymmetricSecurityKey2_256, SecurityAlgorithms.HmacSha256, securityTokenDescriptor, jsonWebTokenHandler));
            theoryData.Add(BuildTestCase("Hmac384_Key512", KeyingMaterial.SymmetricSecurityKey2_384, SecurityAlgorithms.HmacSha384, securityTokenDescriptor, jsonWebTokenHandler));
            theoryData.Add(BuildTestCase("Hmac512_Key512", KeyingMaterial.SymmetricSecurityKey2_512, SecurityAlgorithms.HmacSha512, securityTokenDescriptor, jsonWebTokenHandler));

            return theoryData;
        }

        private static ValidateTokenTheoryData BuildTestCase(string testId, SecurityKey securityKey, string securityAlgorithm, SecurityTokenDescriptor securityTokenDescriptor, JsonWebTokenHandler jsonWebTokenHandler)
        {
            securityTokenDescriptor.SigningCredentials = new SigningCredentials(securityKey, securityAlgorithm);
            return new ValidateTokenTheoryData(testId)
            {
                JsonWebToken = jsonWebTokenHandler.CreateToken(securityTokenDescriptor),
                ValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = securityKey,
                    ValidateIssuer = false,
                    ValidateAudience = false
                }
            };
        }
    }
}
