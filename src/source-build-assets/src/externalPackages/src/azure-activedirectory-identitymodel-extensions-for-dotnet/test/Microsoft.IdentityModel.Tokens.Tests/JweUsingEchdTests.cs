// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

#if NET472 || NET6_0_OR_GREATER

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.TestUtils;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Microsoft.IdentityModel.Tokens.Tests
{
    public class JweUsingEcdhEsTests
    {
        [Theory, MemberData(nameof(CreateEcdhEsTestcases), DisableDiscoveryEnumeration = true)]
        public async Task CreateJweEcdhEsTests(CreateEcdhEsTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateJweEcdhEsTests", theoryData);
            context.AddClaimTypesToIgnoreWhenComparing("exp", "iat", "nbf");
            context.AddDictionaryKeysToIgnoreWhenComparing("exp", "iat", "nbf");

            try
            {
                JsonWebTokenHandler jsonWebTokenHandler = new JsonWebTokenHandler();
                JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                jwtSecurityTokenHandler.MapInboundClaims = false;
                jwtSecurityTokenHandler.OutboundClaimTypeMap.Clear();
                SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = Default.ClaimsIdentity,
                    SigningCredentials = Default.AsymmetricSigningCredentials,
                    EncryptingCredentials = theoryData.EncryptingCredentials,
                    AdditionalHeaderClaims = theoryData.AdditionalHeaderParams,
                    Audience = Default.Audience,
                    Issuer = Default.Issuer,
                };

                string jsonJwe = jsonWebTokenHandler.CreateToken(securityTokenDescriptor);
                string jwtJwe = jwtSecurityTokenHandler.CreateEncodedJwt(securityTokenDescriptor);

                TokenValidationResult tokenValidationResult1 = await jsonWebTokenHandler.ValidateTokenAsync(jsonJwe, theoryData.TokenValidationParameters);
                TokenValidationResult tokenValidationResult2 = await jsonWebTokenHandler.ValidateTokenAsync(jwtJwe, theoryData.TokenValidationParameters);
                TokenValidationResult tokenValidationResult3 = await jwtSecurityTokenHandler.ValidateTokenAsync(jsonJwe, theoryData.TokenValidationParameters);
                TokenValidationResult tokenValidationResult4 = await jwtSecurityTokenHandler.ValidateTokenAsync(jwtJwe, theoryData.TokenValidationParameters);

                if (tokenValidationResult1.IsValid != theoryData.ExpectedIsValid)
                    context.AddDiff($"tokenValidationResult1.IsValid != theoryData.ExpectedIsValid");

                if (tokenValidationResult2.IsValid != theoryData.ExpectedIsValid)
                    context.AddDiff($"tokenValidationResult2.IsValid != theoryData.ExpectedIsValid");

                if (tokenValidationResult3.IsValid != theoryData.ExpectedIsValid)
                    context.AddDiff($"tokenValidationResult3.IsValid != theoryData.ExpectedIsValid");

                if (tokenValidationResult4.IsValid != theoryData.ExpectedIsValid)
                    context.AddDiff($"tokenValidationResult4.IsValid != theoryData.ExpectedIsValid");

                IdentityComparer.AreEqual(tokenValidationResult1.ClaimsIdentity, tokenValidationResult2.ClaimsIdentity, context);
                IdentityComparer.AreEqual(tokenValidationResult1.ClaimsIdentity, tokenValidationResult3.ClaimsIdentity, context);
                IdentityComparer.AreEqual(tokenValidationResult1.ClaimsIdentity, tokenValidationResult4.ClaimsIdentity, context);
                IdentityComparer.AreEqual(tokenValidationResult1.Claims, tokenValidationResult2.Claims, context);
                IdentityComparer.AreEqual(tokenValidationResult1.Claims, tokenValidationResult3.Claims, context);
                IdentityComparer.AreEqual(tokenValidationResult1.Claims, tokenValidationResult4.Claims, context);

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateEcdhEsTheoryData> CreateEcdhEsTestcases
        {
            get
            {
                TheoryData<CreateEcdhEsTheoryData> theoryData = new TheoryData<CreateEcdhEsTheoryData>();

                theoryData.Add(EcdhEsCurveP256AEnc256KW());
                theoryData.Add(EcdhEsCurveP256AEnc256KWNullApuApv());
                theoryData.Add(EcdhEsCurveP384EncA256KW());
                theoryData.Add(EcdhEsCurveP512EncA256KW());
                theoryData.Add(EcdhEsCurveP256EncA192KW());
                theoryData.Add(EcdhEsCurveP256EncA128KW());

                return theoryData;
            }
        }

        private static CreateEcdhEsTheoryData EcdhEsCurveP256AEnc256KW()
        {
            CreateEcdhEsTheoryData testData = new CreateEcdhEsTheoryData("EcdhEsCurveP256AEnc256KW")
            {
                EncryptingCredentials = new EncryptingCredentials(
                                    new ECDsaSecurityKey(KeyingMaterial.JsonWebKeyP256, true),
                                    SecurityAlgorithms.EcdhEsA256kw,
                                    SecurityAlgorithms.Aes128CbcHmacSha256)
                {
                    KeyExchangePublicKey = KeyingMaterial.JsonWebKeyP256_Public
                },
                PublicKeyReceiver = KeyingMaterial.JsonWebKeyP256_Public,
                PublicKeySender = KeyingMaterial.JsonWebKeyP256_Public,
                PrivateKeyReceiver = KeyingMaterial.JsonWebKeyP256,
                TokenValidationParameters = new TokenValidationParameters()
                {
                    TokenDecryptionKey = new ECDsaSecurityKey(KeyingMaterial.JsonWebKeyP256, true),
                    ValidAudience = Default.Audience,
                    ValidIssuer = Default.Issuer,
                    IssuerSigningKey = Default.AsymmetricSigningKey
                },
                ApuSender = Guid.NewGuid().ToString(),
                ApvSender = Guid.NewGuid().ToString()
            };

            return SetAdditionalHeaderParameters(testData);
        }

        private static CreateEcdhEsTheoryData EcdhEsCurveP256AEnc256KWNullApuApv()
        {
            CreateEcdhEsTheoryData testData = new CreateEcdhEsTheoryData("EcdhEsCurveP256AEnc256KW")
            {
                EncryptingCredentials = new EncryptingCredentials(
                                    new ECDsaSecurityKey(KeyingMaterial.JsonWebKeyP256, true),
                                    SecurityAlgorithms.EcdhEsA256kw,
                                    SecurityAlgorithms.Aes128CbcHmacSha256)
                {
                    KeyExchangePublicKey = KeyingMaterial.JsonWebKeyP256_Public
                },
                PublicKeyReceiver = KeyingMaterial.JsonWebKeyP256_Public,
                PublicKeySender = KeyingMaterial.JsonWebKeyP256_Public,
                PrivateKeyReceiver = KeyingMaterial.JsonWebKeyP256,
                TokenValidationParameters = new TokenValidationParameters()
                {
                    TokenDecryptionKey = new ECDsaSecurityKey(KeyingMaterial.JsonWebKeyP256, true),
                    ValidAudience = Default.Audience,
                    ValidIssuer = Default.Issuer,
                    IssuerSigningKey = Default.AsymmetricSigningKey
                },
                ApuSender = null,
                ApvSender = null
            };

            return SetAdditionalHeaderParameters(testData);
        }

        private static CreateEcdhEsTheoryData EcdhEsCurveP384EncA256KW()
        {
            CreateEcdhEsTheoryData testData = new CreateEcdhEsTheoryData("EcdhEsCurveP384EncA256KW")
            {
                EncryptingCredentials = new EncryptingCredentials(
                                    new ECDsaSecurityKey(KeyingMaterial.JsonWebKeyP384, true),
                                    SecurityAlgorithms.EcdhEsA256kw,
                                    SecurityAlgorithms.Aes128CbcHmacSha256)
                {
                    KeyExchangePublicKey = KeyingMaterial.JsonWebKeyP384_Public
                },
                PublicKeyReceiver = KeyingMaterial.JsonWebKeyP384_Public,
                PublicKeySender = KeyingMaterial.JsonWebKeyP384_Public,
                PrivateKeyReceiver = KeyingMaterial.JsonWebKeyP384,
                TokenValidationParameters = new TokenValidationParameters()
                {
                    TokenDecryptionKey = new ECDsaSecurityKey(KeyingMaterial.JsonWebKeyP384, true),
                    ValidAudience = Default.Audience,
                    ValidIssuer = Default.Issuer,
                    IssuerSigningKey = Default.AsymmetricSigningKey
                },
                ApuSender = Guid.NewGuid().ToString(),
                ApvSender = Guid.NewGuid().ToString()
            };

            return SetAdditionalHeaderParameters(testData);
        }

        private static CreateEcdhEsTheoryData EcdhEsCurveP512EncA256KW()
        {
            // use of 521 is actually 512
            CreateEcdhEsTheoryData testData = new CreateEcdhEsTheoryData("EcdhEsCurveP512EncA256KW")
            {
                EncryptingCredentials = new EncryptingCredentials(
                                    new ECDsaSecurityKey(KeyingMaterial.JsonWebKeyP521, true),
                                    SecurityAlgorithms.EcdhEsA256kw,
                                    SecurityAlgorithms.Aes128CbcHmacSha256)
                {
                    KeyExchangePublicKey = KeyingMaterial.JsonWebKeyP521_Public
                },
                PublicKeyReceiver = KeyingMaterial.JsonWebKeyP521_Public,
                PublicKeySender = KeyingMaterial.JsonWebKeyP521_Public,
                PrivateKeyReceiver = KeyingMaterial.JsonWebKeyP521,
                TokenValidationParameters = new TokenValidationParameters()
                {
                    TokenDecryptionKey = new ECDsaSecurityKey(KeyingMaterial.JsonWebKeyP521, true),
                    ValidAudience = Default.Audience,
                    ValidIssuer = Default.Issuer,
                    IssuerSigningKey = Default.AsymmetricSigningKey
                },
                ApuSender = Guid.NewGuid().ToString(),
                ApvSender = Guid.NewGuid().ToString()
            };

            return SetAdditionalHeaderParameters(testData);
        }

        private static CreateEcdhEsTheoryData EcdhEsCurveP256EncA192KW()
        {
            CreateEcdhEsTheoryData testData = new CreateEcdhEsTheoryData("EcdhEsCurveP256EncA192KW")
            {
                EncryptingCredentials = new EncryptingCredentials(
                                    new ECDsaSecurityKey(KeyingMaterial.JsonWebKeyP256, true),
                                    SecurityAlgorithms.EcdhEsA192kw,
                                    SecurityAlgorithms.Aes192CbcHmacSha384)
                {
                    KeyExchangePublicKey = KeyingMaterial.JsonWebKeyP256_Public
                },
                PublicKeyReceiver = KeyingMaterial.JsonWebKeyP256_Public,
                PublicKeySender = KeyingMaterial.JsonWebKeyP256_Public,
                PrivateKeyReceiver = KeyingMaterial.JsonWebKeyP256,
                TokenValidationParameters = new TokenValidationParameters()
                {
                    TokenDecryptionKey = new ECDsaSecurityKey(KeyingMaterial.JsonWebKeyP256, true),
                    ValidAudience = Default.Audience,
                    ValidIssuer = Default.Issuer,
                    IssuerSigningKey = Default.AsymmetricSigningKey
                },
                ApuSender = Guid.NewGuid().ToString(),
                ApvSender = Guid.NewGuid().ToString()
            };

            return SetAdditionalHeaderParameters(testData);
        }

        private static CreateEcdhEsTheoryData EcdhEsCurveP256EncA128KW()
        {
            CreateEcdhEsTheoryData testData = new CreateEcdhEsTheoryData("EcdhEsCurveP256EncA128KW")
            {
                EncryptingCredentials = new EncryptingCredentials(
                                    new ECDsaSecurityKey(KeyingMaterial.JsonWebKeyP256, true),
                                    SecurityAlgorithms.EcdhEsA128kw,
                                    SecurityAlgorithms.Aes128CbcHmacSha256)
                {
                    KeyExchangePublicKey = KeyingMaterial.JsonWebKeyP256_Public
                },
                PublicKeyReceiver = KeyingMaterial.JsonWebKeyP256_Public,
                PublicKeySender = KeyingMaterial.JsonWebKeyP256_Public,
                PrivateKeyReceiver = KeyingMaterial.JsonWebKeyP256,
                TokenValidationParameters = new TokenValidationParameters()
                {
                    TokenDecryptionKey = new ECDsaSecurityKey(KeyingMaterial.JsonWebKeyP256, true),
                    ValidAudience = Default.Audience,
                    ValidIssuer = Default.Issuer,
                    IssuerSigningKey = Default.AsymmetricSigningKey
                },
                ApuSender = Guid.NewGuid().ToString(),
                ApvSender = Guid.NewGuid().ToString()
            };

            return SetAdditionalHeaderParameters(testData);
        }

        private static CreateEcdhEsTheoryData SetAdditionalHeaderParameters(CreateEcdhEsTheoryData testData)
        {
            var epkJObject = new JObject();
            epkJObject.Add(JsonWebKeyParameterNames.Kty, testData.PublicKeySender.Kty);
            epkJObject.Add(JsonWebKeyParameterNames.Crv, testData.PublicKeySender.Crv);
            epkJObject.Add(JsonWebKeyParameterNames.X, testData.PublicKeySender.X);
            epkJObject.Add(JsonWebKeyParameterNames.Y, testData.PublicKeySender.Y);
            testData.AdditionalHeaderParams = new Dictionary<string, object>();
            testData.AdditionalHeaderParams.Add(JsonWebTokens.JwtHeaderParameterNames.Apu, testData.ApuSender);
            testData.AdditionalHeaderParams.Add(JsonWebTokens.JwtHeaderParameterNames.Apv, testData.ApvSender);
            testData.AdditionalHeaderParams.Add(JsonWebTokens.JwtHeaderParameterNames.Epk, epkJObject.ToString(Newtonsoft.Json.Formatting.None));

            return testData;
        }
    }

    public class CreateEcdhEsTheoryData : TheoryDataBase
    {
        public CreateEcdhEsTheoryData(string testId)
        {
            TestId = testId;
        }

        public string ApuReceiver { get; set; }
        public string ApvReceiver { get; set; }
        public string ApuSender { get; set; }
        public string ApvSender { get; set; }
        public EncryptingCredentials EncryptingCredentials { get; set; }
        public JsonWebKey PrivateKeyReceiver { get; set; }
        public JsonWebKey PublicKeyReceiver { get; set; }
        public JsonWebKey PublicKeySender { get; set; }
        public TokenValidationParameters TokenValidationParameters { get; set; }
        public bool ExpectedIsValid { get; set; } = true;
        public IDictionary<string, object> AdditionalHeaderParams { get; set; }
    }
}

#endif
