// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Tests
{
    public class MultiThreadingTokenTests
    {
        [Theory, MemberData(nameof(MultiThreadingCreateAndVerifyTestCases))]
        public void MultiThreadingCreateAndVerify(MultiThreadingTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.MultiThreadingCreateAndVerify", theoryData);
            var numberOfErrors = 0;
            void action()
            {
                for (int loop = 0; loop < 5; loop++)
                {
                    var jwt = theoryData.JwtSecurityTokenHandler.CreateEncodedJwt(theoryData.TokenDescriptor);
                    var claimsPrincipal = theoryData.JwtSecurityTokenHandler.ValidateToken(theoryData.Jwt, theoryData.ValidationParameters, out SecurityToken _);
                    var tokenValidationResult = theoryData.JsonWebTokenHandler.ValidateTokenAsync(theoryData.Jwt, theoryData.ValidationParameters).Result;

                    if (tokenValidationResult.Exception != null && tokenValidationResult.IsValid)
                        context.Diffs.Add("tokenValidationResult.IsValid, tokenValidationResult.Exception != null");

                    if (!tokenValidationResult.IsValid)
                    {
                        numberOfErrors++;
                        if (tokenValidationResult.Exception != null)
                            throw tokenValidationResult.Exception;
                        else
                            throw new SecurityTokenException("something failed");
                    }
                }
            }

            var actions = new Action[100];
            for (int i = 0; i < actions.Length; i++)
                actions[i] = action;

            try
            {
                Parallel.Invoke(actions);
                theoryData.ExpectedException.ProcessNoException();
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            if (numberOfErrors > 0)
                context.AddDiff($"Number of errors: '{numberOfErrors}'.");

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<MultiThreadingTheoryData> MultiThreadingCreateAndVerifyTestCases
        {
            get
            {
                var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                var jsonWebTokenHandler = new JsonWebTokenHandler();

                // ECD
                var tokenValidationParametersEcd = new TokenValidationParameters
                {
                    IssuerSigningKey = KeyingMaterial.Ecdsa256Key,
                    ValidAudience = Default.Audience,
                    ValidIssuer = Default.Issuer
                };

                var securityTokenDescriptorEcd = new SecurityTokenDescriptor
                {
                    Claims = Default.PayloadDictionary,
                    SigningCredentials = new SigningCredentials(KeyingMaterial.Ecdsa256Key, SecurityAlgorithms.EcdsaSha256, SecurityAlgorithms.Sha256),
                };

                var jwtEcd = jwtSecurityTokenHandler.CreateEncodedJwt(securityTokenDescriptorEcd);

                // RSA
                var securityTokenDescriptorRsa = new SecurityTokenDescriptor
                {
                    Claims = Default.PayloadDictionary,
                    SigningCredentials = new SigningCredentials(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaSha256, SecurityAlgorithms.Sha256),
                };

                var tokenValidationParametersRsa = new TokenValidationParameters
                {
                    IssuerSigningKey = KeyingMaterial.RsaSecurityKey_2048,
                    ValidAudience = Default.Audience,
                    ValidIssuer = Default.Issuer
                };

                var jwtRsa = jwtSecurityTokenHandler.CreateEncodedJwt(securityTokenDescriptorRsa);

                // Symmetric
                var securityTokenDescriptorSymmetric = new SecurityTokenDescriptor
                {
                    Claims = Default.PayloadDictionary,
                    SigningCredentials = new SigningCredentials(KeyingMaterial.SymmetricSecurityKey2_256, SecurityAlgorithms.HmacSha256, SecurityAlgorithms.Sha256),
                };

                var tokenValidationParametersSymmetric = new TokenValidationParameters
                {
                    IssuerSigningKey = KeyingMaterial.SymmetricSecurityKey2_256,
                    ValidAudience = Default.Audience,
                    ValidIssuer = Default.Issuer
                };

                var jwtSymmetric = jwtSecurityTokenHandler.CreateEncodedJwt(securityTokenDescriptorSymmetric);

                // Encrypted "RSA keywrap"
                var securityTokenDescriptorEncryptedRsaKW = new SecurityTokenDescriptor
                {
                    Claims = Default.PayloadDictionary,
                    SigningCredentials = new SigningCredentials(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaSha256, SecurityAlgorithms.Sha256),
                    EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaOAEP, SecurityAlgorithms.Aes128CbcHmacSha256)
                };

                var tokenValidationParametersEncryptedRsaKW = new TokenValidationParameters
                {
                    TokenDecryptionKey = KeyingMaterial.RsaSecurityKey_2048,
                    IssuerSigningKey = KeyingMaterial.RsaSecurityKey_2048,
                    ValidAudience = Default.Audience,
                    ValidIssuer = Default.Issuer
                };

                var jwtEncryptedRsaKW = jwtSecurityTokenHandler.CreateEncodedJwt(securityTokenDescriptorEncryptedRsaKW);

                // Encrypted "dir"
                var securityTokenDescriptorEncryptedDir = new SecurityTokenDescriptor
                {
                    Claims = Default.PayloadDictionary,
                    SigningCredentials = new SigningCredentials(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaSha256, SecurityAlgorithms.Sha256),
                    EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.SymmetricSecurityKey2_256, "dir", SecurityAlgorithms.Aes128CbcHmacSha256)
                };

                var tokenValidationParametersEncryptedDir = new TokenValidationParameters
                {
                    TokenDecryptionKey = KeyingMaterial.SymmetricSecurityKey2_256,
                    IssuerSigningKey = KeyingMaterial.RsaSecurityKey_2048,
                    ValidAudience = Default.Audience,
                    ValidIssuer = Default.Issuer
                };

                var jwtEncryptedDir = jwtSecurityTokenHandler.CreateEncodedJwt(securityTokenDescriptorEncryptedDir);

#if NET462 || NET472
                // RSACng 
                var securityTokenDescriptorRsaCng = new SecurityTokenDescriptor
                {
                    Claims = Default.PayloadDictionary,
                    SigningCredentials = new SigningCredentials(KeyingMaterial.RsaSecurityKeyCng_2048, SecurityAlgorithms.RsaSha256, SecurityAlgorithms.Sha256),
                };

                var tokenValidationParametersRsaCng = new TokenValidationParameters
                {
                    IssuerSigningKey = KeyingMaterial.RsaSecurityKeyCng_2048,
                    ValidAudience = Default.Audience,
                    ValidIssuer = Default.Issuer
                };

                var jwtRsaCng = jwtSecurityTokenHandler.CreateEncodedJwt(securityTokenDescriptorRsaCng);
                // Encrypted "RSA keywrap"
                // RsaSecurityKeyRsaKWCng_2048
                var securityTokenDescriptorEncryptedRsaKWCng = new SecurityTokenDescriptor
                {
                    Claims = Default.PayloadDictionary,
                    SigningCredentials = new SigningCredentials(KeyingMaterial.RsaSecurityKeyCng_2048, SecurityAlgorithms.RsaSha256, SecurityAlgorithms.Sha256),
                    EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.RsaSecurityKeyCng_2048, SecurityAlgorithms.RsaOAEP, SecurityAlgorithms.Aes128CbcHmacSha256)
                };

                var tokenValidationParametersEncryptedRsaKWCng = new TokenValidationParameters
                {
                    TokenDecryptionKey = KeyingMaterial.RsaSecurityKeyCng_2048,
                    IssuerSigningKey = KeyingMaterial.RsaSecurityKeyCng_2048,
                    ValidAudience = Default.Audience,
                    ValidIssuer = Default.Issuer
                };

                var jwtEncryptedRsaKWCng = jwtSecurityTokenHandler.CreateEncodedJwt(securityTokenDescriptorEncryptedRsaKWCng);
#endif

                return new TheoryData<MultiThreadingTheoryData>()
                {
                    new MultiThreadingTheoryData
                    {
                        JwtSecurityTokenHandler = jwtSecurityTokenHandler,
                        JsonWebTokenHandler = jsonWebTokenHandler,
                        Jwt = jwtSymmetric,
                        TestId = "JwtSymmetric",
                        TokenDescriptor = securityTokenDescriptorSymmetric,
                        ValidationParameters = tokenValidationParametersSymmetric
                    },
                    new MultiThreadingTheoryData
                    {
                        JwtSecurityTokenHandler = jwtSecurityTokenHandler,
                        JsonWebTokenHandler = jsonWebTokenHandler,
                        Jwt = jwtRsa,
                        TestId = "JwtRsa",
                        TokenDescriptor = securityTokenDescriptorRsa,
                        ValidationParameters = tokenValidationParametersRsa
                    },
                    new MultiThreadingTheoryData
                    {
                        JwtSecurityTokenHandler = jwtSecurityTokenHandler,
                        JsonWebTokenHandler = jsonWebTokenHandler,
                        Jwt = jwtEcd,
                        TestId = "JwtEcd",
                        TokenDescriptor = securityTokenDescriptorEcd,
                        ValidationParameters = tokenValidationParametersEcd
                    },
                    new MultiThreadingTheoryData
                    {
                        JwtSecurityTokenHandler = jwtSecurityTokenHandler,
                        JsonWebTokenHandler = jsonWebTokenHandler,
                        Jwt = jwtEncryptedRsaKW,
                        TestId = "JwtRsaEncryptedRsaKW",
                        TokenDescriptor = securityTokenDescriptorEncryptedRsaKW,
                        ValidationParameters = tokenValidationParametersEncryptedRsaKW
                    },
                    new MultiThreadingTheoryData
                    {
                        JwtSecurityTokenHandler = jwtSecurityTokenHandler,
                        JsonWebTokenHandler = jsonWebTokenHandler,
                        Jwt = jwtEncryptedDir,
                        TestId = "JwtRsaEncryptedDir",
                        TokenDescriptor = securityTokenDescriptorEncryptedDir,
                        ValidationParameters = tokenValidationParametersEncryptedDir
                    },
#if NET462 || NET472
                    new MultiThreadingTheoryData
                    {
                        JwtSecurityTokenHandler = jwtSecurityTokenHandler,
                        JsonWebTokenHandler = jsonWebTokenHandler,
                        Jwt = jwtRsaCng,
                        TestId = "JwtRsaCng",
                        TokenDescriptor = securityTokenDescriptorRsaCng,
                        ValidationParameters = tokenValidationParametersRsaCng
                    },
                    new MultiThreadingTheoryData
                    {
                        JwtSecurityTokenHandler = jwtSecurityTokenHandler,
                        JsonWebTokenHandler = jsonWebTokenHandler,
                        Jwt = jwtEncryptedRsaKWCng,
                        TestId = "JwtRsaEncryptedRsaKWCng",
                        TokenDescriptor = securityTokenDescriptorEncryptedRsaKWCng,
                        ValidationParameters = tokenValidationParametersEncryptedRsaKWCng
                    },
#endif
                };
            }
        }
    }

    public class MultiThreadingTheoryData : TheoryDataBase
    {
        public string Jwt { get; set; }

        public SecurityTokenDescriptor TokenDescriptor { get; set; }

        public JsonWebTokenHandler JsonWebTokenHandler { get; set; }

        public JwtSecurityTokenHandler JwtSecurityTokenHandler { get; set; }

        public TokenValidationParameters ValidationParameters { get; set; }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
