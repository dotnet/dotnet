// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IdentityModel.Tokens.Jwt.Tests;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using Xunit;

#if NET472_OR_GREATER || NET6_0_OR_GREATER
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
#endif
using TokenLogMessages = Microsoft.IdentityModel.Tokens.LogMessages;

namespace Microsoft.IdentityModel.JsonWebTokens.Tests
{
    public class JsonWebTokenHandlerDecryptTokenTests
    {
        [Theory, MemberData(nameof(JsonWebTokenHandlerDecryptTokenTestCases), DisableDiscoveryEnumeration = false)]
        public void DecryptToken(TokenDecryptingTheoryData theoryData)
        {
            JsonWebTokenHandler jsonWebTokenHandler = new JsonWebTokenHandler();
            if (theoryData.Token == null)
            {
                string tokenString = null;
                if (theoryData.SecurityTokenDescriptor != null)
                    tokenString = jsonWebTokenHandler.CreateToken(theoryData.SecurityTokenDescriptor);
                else
                    tokenString = theoryData.TokenString;

                if (tokenString != null)
                    theoryData.Token = new JsonWebToken(tokenString);
            }

            CompareContext context = TestUtilities.WriteHeader($"{this}.JsonWebTokenHandlerDecryptTokenTests", theoryData);
            ValidationResult<string, ValidationError> validationResult = jsonWebTokenHandler.DecryptToken(
                theoryData.Token,
                theoryData.ValidationParameters,
                theoryData.Configuration,
                theoryData.CallContext);

            if (validationResult.Succeeded)
            {
                IdentityComparer.AreStringsEqual(
                    validationResult.Result,
                    theoryData.OperationResult.Result,
                    context);

                theoryData.ExpectedException.ProcessNoException(context);
            }
            else
            {
                ValidationError validationError = validationResult.Error;
                IdentityComparer.AreStringsEqual(
                    validationError.FailureType.Name,
                    theoryData.OperationResult.Error.FailureType.Name,
                    context);

                Exception exception = validationError.GetException();
                theoryData.ExpectedException.ProcessException(exception, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void DecryptToken_ThrowsIfAccessingSecurityTokenOnFailedRead()
        {
            JsonWebTokenHandler jsonWebTokenHandler = new JsonWebTokenHandler();
            ValidationResult<string, ValidationError> tokenDecryptionResult = jsonWebTokenHandler.DecryptToken(
                null,
                null,
                null,
                new CallContext());

            // TODO what did this test do?
        }

        public static TheoryData<TokenDecryptingTheoryData> JsonWebTokenHandlerDecryptTokenTestCases
        {
            get
            {
                var validToken = EncodedJwts.LiveJwt;
                var token = new JsonWebToken(validToken);
#if NET472 || NET6_0_OR_GREATER
                var ecdsaEncryptingCredentials = new EncryptingCredentials(
                                new ECDsaSecurityKey(KeyingMaterial.JsonWebKeyP256, true),
                                SecurityAlgorithms.EcdhEsA256kw,
                                SecurityAlgorithms.Aes128CbcHmacSha256)
                {
                    KeyExchangePublicKey = KeyingMaterial.JsonWebKeyP256_Public
                };
                var ecdsaTokenDescriptor = new SecurityTokenDescriptor
                {
                    EncryptingCredentials = ecdsaEncryptingCredentials,
                    Expires = DateTime.MaxValue,
                    NotBefore = DateTime.MinValue,
                    IssuedAt = DateTime.MinValue,
                    AdditionalHeaderClaims = AdditionalEcdhEsHeaderParameters(KeyingMaterial.JsonWebKeyP256_Public),
                };

                var jsonWebTokenHandler = new JsonWebTokenHandler();
                var ecdsaToken = new JsonWebToken(jsonWebTokenHandler.CreateToken(ecdsaTokenDescriptor));

                static Dictionary<string, object> AdditionalEcdhEsHeaderParameters(JsonWebKey publicKeySender)
                {
                    var epkJObject = new JObject();
                    epkJObject.Add(JsonWebKeyParameterNames.Kty, publicKeySender.Kty);
                    epkJObject.Add(JsonWebKeyParameterNames.Crv, publicKeySender.Crv);
                    epkJObject.Add(JsonWebKeyParameterNames.X, publicKeySender.X);
                    epkJObject.Add(JsonWebKeyParameterNames.Y, publicKeySender.Y);

                    Dictionary<string, object> additionalHeaderParams = new Dictionary<string, object>()
                    {
                        { JsonWebTokens.JwtHeaderParameterNames.Apu, Guid.NewGuid().ToString() },
                        { JsonWebTokens.JwtHeaderParameterNames.Apv, Guid.NewGuid().ToString() },
                        { JsonWebTokens.JwtHeaderParameterNames.Epk, epkJObject.ToString(Newtonsoft.Json.Formatting.None) }
                    };

                    return additionalHeaderParams;
                }
#endif
                var rsaKey = new RsaSecurityKey(KeyingMaterial.RsaParameters_2048) { KeyId = "CustomRsaSecurityKey_2048" };
                var configurationThatThrows = CreateCustomConfigurationThatThrows(rsaKey);

                var configurationWithMismatchedKeys = new CustomConfiguration(rsaKey);

                return new TheoryData<TokenDecryptingTheoryData>
                {
                    new TokenDecryptingTheoryData("Invalid_TokenIsNotEncrypted")
                    {
                        Token = token,
                        ValidationParameters = new ValidationParameters(),
                        ExpectedException = ExpectedException.SecurityTokenDecryptionFailedException("IDX10612:"),
                        OperationResult = new ValidationError(
                            new MessageDetail(TokenLogMessages.IDX10612),
                            ValidationFailureType.TokenDecryptionFailed,
                            null),
                    },
                    new TokenDecryptingTheoryData("Invalid_SecurityTokenIsNull")
                    {
                        Token = null,
                        ValidationParameters = new ValidationParameters(),
                        ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                        OperationResult = new ValidationError(
                            new MessageDetail(TokenLogMessages.IDX10000, "jwtToken"),
                            ValidationFailureType.NullArgument,
                            null),
                    },
                    new TokenDecryptingTheoryData("Invalid_ValidationParametersIsNull")
                    {
                        Token = token,
                        ValidationParameters = null,
                        ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                        OperationResult = new ValidationError(
                            new MessageDetail(TokenLogMessages.IDX10000, "validationParameters"),
                            ValidationFailureType.NullArgument,
                            null),
                    },
                    new TokenDecryptingTheoryData("Valid_Aes128_FromValidationParameters")
                    {
                        TokenString = ReferenceTokens.JWEDirectEncryptionUnsignedInnerJWTWithAdditionalHeaderClaims,
                        ValidationParameters = ValidationUtils.CreateValidationParameters(
                            decryptionKeys: [Default.SymmetricEncryptingCredentials.Key]),
                        OperationResult = "eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0.eyJlbWFpbCI6IkJvYkBjb250b3NvLmNvbSIsImdpdmVuX25hbWUiOiJCb2IiLCJpc3MiOiJodHRwOi8vRGVmYXVsdC5Jc3N1ZXIuY29tIiwiYXVkIjoiaHR0cDovL0RlZmF1bHQuQXVkaWVuY2UuY29tIiwiaWF0IjoiMTQ4OTc3NTYxNyIsIm5iZiI6IjE0ODk3NzU2MTciLCJleHAiOiIyNTM0MDIzMDA3OTkifQ.",
                    },
                    new TokenDecryptingTheoryData("Valid_Aes128_FromKeyResolver")
                    {
                        TokenString = ReferenceTokens.JWEDirectEncryptionUnsignedInnerJWTWithAdditionalHeaderClaims,
                        ValidationParameters = new ValidationParameters
                        {
                            DecryptionKeyResolver = (tokenString, token, kid, validationParameters, callContext) => [Default.SymmetricEncryptingCredentials.Key]
                        },
                        OperationResult = "eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0.eyJlbWFpbCI6IkJvYkBjb250b3NvLmNvbSIsImdpdmVuX25hbWUiOiJCb2IiLCJpc3MiOiJodHRwOi8vRGVmYXVsdC5Jc3N1ZXIuY29tIiwiYXVkIjoiaHR0cDovL0RlZmF1bHQuQXVkaWVuY2UuY29tIiwiaWF0IjoiMTQ4OTc3NTYxNyIsIm5iZiI6IjE0ODk3NzU2MTciLCJleHAiOiIyNTM0MDIzMDA3OTkifQ.",
                    },
                    new TokenDecryptingTheoryData("Valid_Aes128_FromConfiguration")
                    {
                        TokenString = ReferenceTokens.JWEDirectEncryptionUnsignedInnerJWTWithAdditionalHeaderClaims,
                        ValidationParameters = new ValidationParameters
                        {
                            ConfigurationManager = new StaticConfigurationManager<BaseConfiguration>(new CustomConfiguration(Default.SymmetricEncryptingCredentials.Key))
                        },
                        Configuration = new CustomConfiguration(Default.SymmetricEncryptingCredentials.Key),
                        OperationResult = "eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0.eyJlbWFpbCI6IkJvYkBjb250b3NvLmNvbSIsImdpdmVuX25hbWUiOiJCb2IiLCJpc3MiOiJodHRwOi8vRGVmYXVsdC5Jc3N1ZXIuY29tIiwiYXVkIjoiaHR0cDovL0RlZmF1bHQuQXVkaWVuY2UuY29tIiwiaWF0IjoiMTQ4OTc3NTYxNyIsIm5iZiI6IjE0ODk3NzU2MTciLCJleHAiOiIyNTM0MDIzMDA3OTkifQ.",
                    },
#if NET472 || NET6_0_OR_GREATER
                    new TokenDecryptingTheoryData("Valid_Ecdsa256_FromValidationParameters")
                    {
                        Token = ecdsaToken,
                        ValidationParameters = ValidationUtils.CreateValidationParameters(
                            decryptionKeys: [new ECDsaSecurityKey(KeyingMaterial.JsonWebKeyP256, true)]
                        ),
                        OperationResult = "eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0.eyJleHAiOjI1MzQwMjMwMDgwMCwiaWF0IjowLCJuYmYiOjB9."
                    },
#endif
                    new TokenDecryptingTheoryData("Invalid_NoKeysProvided")
                    {
                        TokenString = ReferenceTokens.JWEDirectEncryptionUnsignedInnerJWTWithAdditionalHeaderClaims,
                        ValidationParameters = new ValidationParameters(),
                        ExpectedException = ExpectedException.SecurityTokenDecryptionFailedException("IDX10609:"),
                        OperationResult = new ValidationError(
                            new MessageDetail(
                                TokenLogMessages.IDX10609,
                                LogHelper.MarkAsSecurityArtifact(
                                    new JsonWebToken(ReferenceTokens.JWEDirectEncryptionUnsignedInnerJWTWithAdditionalHeaderClaims),
                                    JwtTokenUtilities.SafeLogJwtToken)),
                            ValidationFailureType.TokenDecryptionFailed,
                            null),
                    },
                    new TokenDecryptingTheoryData("Valid_OneKeyThrowsOnUnwrap_DecryptionSucceeds")
                    {
                        SecurityTokenDescriptor = new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes128CbcHmacSha256),
                            Claims = Default.PayloadDictionary
                        },
                        ValidationParameters = new ValidationParameters
                        {
                            ConfigurationManager = new StaticConfigurationManager<BaseConfiguration>(configurationThatThrows)
                        },
                        Configuration = configurationThatThrows,
                        OperationResult = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikpzb25XZWJLZXlSc2FfMjA0OCIsInR5cCI6IkpXVCJ9.eyJhdWQiOiJodHRwOi8vRGVmYXVsdC5BdWRpZW5jZS5jb20iLCJhenAiOiJodHRwOi8vRGVmYXVsdC5BenAuY29tIiwiZW1haWwiOiJCb2JAY29udG9zby5jb20iLCJleHAiOiIyNTM0MDIzMDA3OTkiLCJnaXZlbl9uYW1lIjoiQm9iIiwiaXNzIjoiaHR0cDovL0RlZmF1bHQuSXNzdWVyLmNvbSIsImlhdCI6IjE0ODk3NzU2MTciLCJqdGkiOiJKdGkiLCJuYmYiOiIxNDg5Nzc1NjE3In0.Et69LAC4sn6nNm_HNz_AnJ8siLT6LRTjDSb1aY8APcwJmPn-TxU-8GG5_bmNkoVukR7hkYG2JuWPxJKbjDd73BlmelaiyZBoPUyU0S-GX3XgyC2v_CkOq4yYbtD-kq5s7kNNj5QJjZDq0oJeqcUMrq4xRWATPtUMkIZ0GpEhO_C5MFxT8jAWe_a2gyUA4KoibalKtkYgFvgLcvyZJhUx7AERbli6b7OkUksFp9zIwmc_jZZCXJ_F_wASyj9KgHQKN9VHER3bB2zQeWHR0q32ODYC4ggsan-Nkm-jIsATi2tgkKzROzK55dy8ZdFArXUYJRpI_raYkTUHRK_wP3GqtQ",
                    },
                    new TokenDecryptingTheoryData("Invalid_AlgorithmMismatch_DecryptionFails")
                    {
                        // Unwrap failures now surface as decryption failures (IDX10603)
                        // rather than key-wrap exceptions (IDX10618).
                        ExpectedException = ExpectedException.SecurityTokenDecryptionFailedException("IDX10603:"),
                        SecurityTokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.DefaultX509Key_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes128CbcHmacSha256),
                            Claims = Default.PayloadDictionary
                        },
                        ValidationParameters = ValidationUtils.CreateValidationParameters(
                            decryptionKeys: [KeyingMaterial.RsaSecurityKey_2048]),
                        OperationResult = new ValidationError(
                            new MessageDetail(
                                TokenLogMessages.IDX10603,
                                LogHelper.MarkAsSecurityArtifact(
                                    new JsonWebToken(ReferenceTokens.JWEDirectEncryptionUnsignedInnerJWTWithAdditionalHeaderClaims),
                                    JwtTokenUtilities.SafeLogJwtToken)),
                            ValidationFailureType.TokenDecryptionFailed,
                            null),
                    },
                    new TokenDecryptingTheoryData("KeyIdMismatch_TryAllDecryptionKeysTrue_DecryptionSucceeds")
                    {
                        SecurityTokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes128CbcHmacSha256),
                            Claims = Default.PayloadDictionary
                        },
                        ValidationParameters = new ValidationParameters  // TryAllDecryptionKeys is true by default
                        {
                            ConfigurationManager = new StaticConfigurationManager<BaseConfiguration>(configurationWithMismatchedKeys)
                        },
                        Configuration = configurationWithMismatchedKeys,
                        OperationResult = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikpzb25XZWJLZXlSc2FfMjA0OCIsInR5cCI6IkpXVCJ9.eyJhdWQiOiJodHRwOi8vRGVmYXVsdC5BdWRpZW5jZS5jb20iLCJhenAiOiJodHRwOi8vRGVmYXVsdC5BenAuY29tIiwiZW1haWwiOiJCb2JAY29udG9zby5jb20iLCJleHAiOiIyNTM0MDIzMDA3OTkiLCJnaXZlbl9uYW1lIjoiQm9iIiwiaXNzIjoiaHR0cDovL0RlZmF1bHQuSXNzdWVyLmNvbSIsImlhdCI6IjE0ODk3NzU2MTciLCJqdGkiOiJKdGkiLCJuYmYiOiIxNDg5Nzc1NjE3In0.Et69LAC4sn6nNm_HNz_AnJ8siLT6LRTjDSb1aY8APcwJmPn-TxU-8GG5_bmNkoVukR7hkYG2JuWPxJKbjDd73BlmelaiyZBoPUyU0S-GX3XgyC2v_CkOq4yYbtD-kq5s7kNNj5QJjZDq0oJeqcUMrq4xRWATPtUMkIZ0GpEhO_C5MFxT8jAWe_a2gyUA4KoibalKtkYgFvgLcvyZJhUx7AERbli6b7OkUksFp9zIwmc_jZZCXJ_F_wASyj9KgHQKN9VHER3bB2zQeWHR0q32ODYC4ggsan-Nkm-jIsATi2tgkKzROzK55dy8ZdFArXUYJRpI_raYkTUHRK_wP3GqtQ",
                    },
                    new TokenDecryptingTheoryData("KeyIdMismatch_TryAllDecryptionKeysFalse_DecryptionFails")
                    {
                        ExpectedException = ExpectedException.SecurityTokenDecryptionFailedException("IDX10609:"),
                        SecurityTokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes128CbcHmacSha256),
                            Claims = Default.PayloadDictionary
                        },
                        ValidationParameters = new ValidationParameters
                        {
                            TryAllDecryptionKeys = false,
                        },
                        Configuration = configurationWithMismatchedKeys,
                        OperationResult = new ValidationError(
                            new MessageDetail(
                                TokenLogMessages.IDX10609,
                                LogHelper.MarkAsSecurityArtifact(
                                    new JsonWebToken(ReferenceTokens.JWEDirectEncryptionUnsignedInnerJWTWithAdditionalHeaderClaims),
                                    JwtTokenUtilities.SafeLogJwtToken)),
                            ValidationFailureType.TokenDecryptionFailed,
                            null),
                    },
                };
            }
        }

        private static CustomConfiguration CreateCustomConfigurationThatThrows(SecurityKey rsaKey)
        {
            var customCryptoProviderFactory = new DerivedCryptoProviderFactory
            {
                IsSupportedAlgImpl = (alg, key) => true,
                CreateKeyWrapProviderForUnwrapImpl = (key, alg) => throw new InvalidOperationException("Test exception")
            };

            var sym512Hey = new SymmetricSecurityKey(KeyingMaterial.DefaultSymmetricKeyBytes_512) { KeyId = "CustomSymmetricSecurityKey_512" };
            sym512Hey.CryptoProviderFactory = customCryptoProviderFactory;

            var configurationWithCustomCryptoProviderFactory = new CustomConfiguration(rsaKey);
            configurationWithCustomCryptoProviderFactory.TokenDecryptionKeys.Add(sym512Hey);

            return configurationWithCustomCryptoProviderFactory;
        }
    }

    public class TokenDecryptingTheoryData : TheoryDataBase
    {
        public TokenDecryptingTheoryData(string testId) : base(testId) { }
        public JsonWebToken Token { get; set; }
        internal ValidationResult<string, ValidationError> OperationResult { get; set; }
        public BaseConfiguration Configuration { get; internal set; }
        public SecurityTokenDescriptor SecurityTokenDescriptor { get; internal set; }
        public string TokenString { get; internal set; }
        internal ValidationParameters ValidationParameters { get; set; }
    }

    public class CustomConfiguration : BaseConfiguration
    {
        public CustomConfiguration(SecurityKey tokenDecryptionKey) : base()
        {
            TokenDecryptionKeys.Add(tokenDecryptionKey);
        }
    }
}
