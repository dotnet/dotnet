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

using System.Collections.Generic;
using System.IO;
using System.Net;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Validators;
using Xunit;

namespace System.IdentityModel.Tokens.Jwt.Tests
{

    /// <summary>
    /// JWT test datasets intended to be shared by the <see cref="JwtSecurityTokenHandler"/> and the <see cref="JsonWebTokenHandler"/>.
    /// </summary>
    public static class JwtTestDatasets
    {
        public static List<JwtTheoryData> ValidateJwsWithConfigTheoryData
        {
            get
            {
                var validConfig = new OpenIdConnectConfiguration() { TokenEndpoint = Default.Issuer + "oauth/token", Issuer = Default.Issuer };
                validConfig.SigningKeys.Add(KeyingMaterial.DefaultX509Key_2048);

                var invalidIssuerConfig = new OpenIdConnectConfiguration() { TokenEndpoint = Default.Issuer + "oauth/token", Issuer = Default.Issuer + "2" };
                invalidIssuerConfig.SigningKeys.Add(KeyingMaterial.DefaultX509Key_2048);

                var incorrectSigningKeysConfig = new OpenIdConnectConfiguration() { TokenEndpoint = Default.Issuer + "oauth/token", Issuer = Default.Issuer };
                incorrectSigningKeysConfig.SigningKeys.Add(KeyingMaterial.X509SecurityKey2);

                var requestTimedOutException = new IOException();
                requestTimedOutException.Data.Add(HttpDocumentRetriever.StatusCode, HttpStatusCode.RequestTimeout);
                requestTimedOutException.Data.Add(HttpDocumentRetriever.ResponseContent, "requestTimedOutException");

                var requestServiceUnavailableException = new IOException();
                requestServiceUnavailableException.Data.Add(HttpDocumentRetriever.StatusCode, HttpStatusCode.RequestTimeout);
                requestServiceUnavailableException.Data.Add(HttpDocumentRetriever.ResponseContent, "requestServiceUnavailableException");

                var requestNotFoundException = new IOException();
                requestNotFoundException.Data.Add(HttpDocumentRetriever.StatusCode, HttpStatusCode.NotFound);
                requestNotFoundException.Data.Add(HttpDocumentRetriever.ResponseContent, "requestNotFoundException");

                return new List<JwtTheoryData>
                {
                    new JwtTheoryData
                    {
                        TestId = nameof(Default.AsymmetricJws) + "_TVPInvalid_ConfigValid",
                        Token = Default.AsymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new StaticConfigurationManager<OpenIdConnectConfiguration>(validConfig),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        },
                        ShouldSetLastKnownConfiguration = true
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(Default.AsymmetricJws) + "_TVPInvalid_ConfigValid_ValidateSignatureLast",
                        Token = Default.AsymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new StaticConfigurationManager<OpenIdConnectConfiguration>(validConfig),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            ValidateSignatureLast = true,
                        },
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(Default.AsymmetricJws) + "_TVPInvalid_ConfigIssuerInvalid",
                        Token = Default.AsymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new StaticConfigurationManager<OpenIdConnectConfiguration>(invalidIssuerConfig),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        },
                        ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException("IDX10205: "),
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(Default.AsymmetricJws) + "_TVPInvalid_ConfigIssuerInvalid_IssuerValidatorReturnsTrue",
                        Token = Default.AsymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new StaticConfigurationManager<OpenIdConnectConfiguration>(invalidIssuerConfig),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            IssuerValidatorUsingConfiguration = (issuer, securityToken, validationParameters, configuration) => { return issuer; },
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        },
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(Default.AsymmetricJws) + "_TVPInvalid_ConfigSigningKeysInvalid",
                        Token = Default.AsymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new StaticConfigurationManager<OpenIdConnectConfiguration>(incorrectSigningKeysConfig),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        },
                        ExpectedException = ExpectedException.SecurityTokenSignatureKeyNotFoundException("IDX10503: "),
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(Default.AsymmetricJws) + "_TVPInvalid_ConfigSigningKeysInvalid_SigningKeyResolverValid",
                        Token = Default.AsymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new StaticConfigurationManager<OpenIdConnectConfiguration>(incorrectSigningKeysConfig),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            IssuerSigningKeyResolverUsingConfiguration =  (token, securityToken, kid, validationParameters, configuration) => { return new List<SecurityKey>() { KeyingMaterial.DefaultX509Key_2048 }; }
                        },
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(Default.AsymmetricJws) + "_TVPInvalid_ConfigValid_IssuerSigningKeyValidatorReturnsFalse",
                        Token = Default.AsymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new StaticConfigurationManager<OpenIdConnectConfiguration>(validConfig),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            IssuerSigningKeyValidatorUsingConfiguration = (securityKey, securityToken, validationParameters, configuration) => { return false; }
                        },
                        ExpectedException = ExpectedException.SecurityTokenInvalidSigningKeyException("IDX10232: ")
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(Default.AsymmetricJws) + "_TVPInvalid_ConfigValid_SignatureValidatorReturnsNull",
                        Token = Default.AsymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new StaticConfigurationManager<OpenIdConnectConfiguration>(validConfig),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            SignatureValidatorUsingConfiguration = (token, validationParameters, configuration) => { return null; },
                        },
                        ShouldSetLastKnownConfiguration = true,
                        ExpectedException = ExpectedException.SecurityTokenInvalidSignatureException("IDX10505: ")
                    },
                    new JwtTheoryData {
                        TestId = nameof(Default.AsymmetricJws) + "_TVPInvalid_CannotObtainConfig",
                        Token = Default.AsymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>("DoesNotExist.json", new OpenIdConnectConfigurationRetriever(), new FileDocumentRetriever()),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        },
                        ExpectedException = new ExpectedException(typeof(SecurityTokenInvalidIssuerException), "IDX10204: ")
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(Default.AsymmetricJws) + "_TVPValid_CannotObtainConfig",
                        Token = Default.AsymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>("DoesNotExist.json", new OpenIdConnectConfigurationRetriever(), new FileDocumentRetriever()),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            IssuerSigningKey = KeyingMaterial.DefaultX509Key_2048,
                            ValidIssuer = Default.Issuer
                        },
                        ShouldSetLastKnownConfiguration = false
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(Default.AsymmetricJws) + "_TVPInvalid_ConfigIssuerValid_AadIssuerValidatorReturnsTrue",
                        Token = Default.AadAsymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new StaticConfigurationManager<OpenIdConnectConfiguration>(validConfig),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            IssuerValidator = AadIssuerValidator.GetAadIssuerValidator(Default.AadV1Authority).Validate,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        },
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(Default.AsymmetricJws) + "_TVPInvalid_ConfigIssuerInvalid_AadIssuerValidatorThrow",
                        Token = Default.AadAsymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new StaticConfigurationManager<OpenIdConnectConfiguration>(invalidIssuerConfig),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            IssuerValidator = AadIssuerValidator.GetAadIssuerValidator(Default.AadV1Authority).Validate,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        },
                        ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException("IDX40001: "),
                    },
                    new JwtTheoryData {
                        TestId = nameof(Default.AsymmetricJws) + "_TVPValid_ConfigNotSet_TryAllIssuerSigningKeysFalse",
                        Token = Default.AsymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = null,
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            IssuerSigningKey = KeyingMaterial.DefaultX509Key_2048,
                            ValidIssuer = Default.Issuer,
                            TryAllIssuerSigningKeys = false
                        }
                    },
                };
            }
        }

        public static TheoryData<JwtTheoryData> ValidateJwsWithLastKnownGoodTheoryData
        {
            get
            {
                var validConfig = new OpenIdConnectConfiguration() { TokenEndpoint = Default.Issuer + "/oauth/token", Issuer = Default.Issuer };
                validConfig.SigningKeys.Add(KeyingMaterial.DefaultX509Key_2048);

                // a special IssuerSigningKeyValidator in the tests below is set to fail if this configuration is used in order
                // to mock issuer signing key validation failure
                var validConfigKeyValidationFails = new OpenIdConnectConfiguration() { TokenEndpoint = Default.Issuer + "/oauth/token", Issuer = Default.Issuer };
                validConfigKeyValidationFails.SigningKeys.Add(KeyingMaterial.DefaultX509Key_2048);

                var invalidIssuerConfig = new OpenIdConnectConfiguration() { TokenEndpoint = Default.Issuer + "/oauth/token", Issuer = Default.Issuer + "1" };
                invalidIssuerConfig.SigningKeys.Add(KeyingMaterial.DefaultX509Key_2048);

                var incorrectSigningKeysConfig = new OpenIdConnectConfiguration() { TokenEndpoint = Default.Issuer + "oauth/token", Issuer = Default.Issuer };
                incorrectSigningKeysConfig.SigningKeys.Add(KeyingMaterial.X509SecurityKey2);

                var incorrectIssuerAndSigningKeysConfig = new OpenIdConnectConfiguration() { TokenEndpoint = Default.Issuer + "/oauth/token", Issuer = Default.Issuer + "1" };
                incorrectIssuerAndSigningKeysConfig.SigningKeys.Add(KeyingMaterial.X509SecurityKey2);

                var incorrectIssuerAndSigningKeysConfig2 = new OpenIdConnectConfiguration() { TokenEndpoint = Default.Issuer + "/oauth/token", Issuer = Default.Issuer + "2" };
                incorrectIssuerAndSigningKeysConfig.SigningKeys.Add(KeyingMaterial.X509SecurityKey2);

                var incorrectSigningKeysConfigWithMatchingKid = new OpenIdConnectConfiguration() { TokenEndpoint = Default.Issuer + "/oauth/token", Issuer = Default.Issuer };
                incorrectSigningKeysConfigWithMatchingKid.SigningKeys.Add(KeyingMaterial.CreateJsonWebKeyEC(JsonWebKeyECTypes.P256, Default.X509AsymmetricSigningCredentials.Key.KeyId, KeyingMaterial.P256_D, KeyingMaterial.P256_X, KeyingMaterial.P256_Y));

                var incorrectIssuerAndIncorrectSigningKeysConfigWithMatchingKid = new OpenIdConnectConfiguration() { TokenEndpoint = Default.Issuer + "/oauth/token", Issuer = Default.Issuer + "1" };
                incorrectSigningKeysConfigWithMatchingKid.SigningKeys.Add(KeyingMaterial.CreateJsonWebKeyEC(JsonWebKeyECTypes.P521, Default.X509AsymmetricSigningCredentials.Key.KeyId, KeyingMaterial.P521_D, KeyingMaterial.P521_X, KeyingMaterial.P521_Y));

                var expiredSecurityTokenDescriptor = Default.X509SecurityTokenDescriptor(Default.X509AsymmetricSigningCredentials);
                expiredSecurityTokenDescriptor.NotBefore = DateTime.UtcNow + TimeSpan.FromDays(1);
                expiredSecurityTokenDescriptor.Expires = DateTime.UtcNow + System.TimeSpan.FromDays(2);
                var expiredJws = Default.Jwt(expiredSecurityTokenDescriptor);

                AadIssuerValidator.GetAadIssuerValidator(Default.AadV1Authority).ConfigurationManagerV1 =
                    new MockConfigurationManager<OpenIdConnectConfiguration>(invalidIssuerConfig, validConfig, validConfig) as BaseConfigurationManager;

                return new TheoryData<JwtTheoryData>
                {
                    new JwtTheoryData
                    {
                        First = true,
                        TestId = nameof(Default.AsymmetricJws) + "_ConfigKeyInvalid_LKGValid",
                        Token = Default.AsymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(incorrectSigningKeysConfig, validConfig),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        }
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(Default.AsymmetricJws) + "_ConfigKeyInvalidKidMatches_LKGValid",
                        Token = Default.AsymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(incorrectSigningKeysConfigWithMatchingKid, validConfig),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        }
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(Default.AsymmetricJws) + "_ConfigKeyInvalidKeyAndIssuer_LKGValid",
                        Token = Default.AsymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(incorrectIssuerAndSigningKeysConfig, validConfig),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        }
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(Default.AsymmetricJws) + "_ConfigIssuerInvalid_LKGValid",
                        Token = Default.AsymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(invalidIssuerConfig, validConfig),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        }
                    },
                    new JwtTheoryData
                    {
                        // SecurityTokenInvalidSigningKeyException is no longer a recoverable exception
                        TestId = nameof(Default.AsymmetricJws) + "_ConfigInvalid_IssuerSigningKeyValidationFails_LKGValid",
                        Token = Default.AsymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(validConfigKeyValidationFails, validConfig),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            IssuerSigningKeyValidatorUsingConfiguration = (securityKey, securityToken, validationParameters, configuration) =>
                            {
                                // mock failing on issuer validation the first time
                                if (configuration == validConfigKeyValidationFails)
                                    return false;
                                else
                                    return true;
                            },
                        },
                        ExpectedException = new ExpectedException(typeof(SecurityTokenInvalidSigningKeyException))
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(Default.AsymmetricJws) + "_ConfigInvalid_ConfigKeyInvalid_LKGExpired",
                        Token = Default.AsymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(incorrectSigningKeysConfig, validConfig, TimeSpan.FromMilliseconds(.000001)),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        },
                        ExpectedException = new ExpectedException(typeof(SecurityTokenSignatureKeyNotFoundException))
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(Default.AsymmetricJws) + "_ConfigInvalid_ConfigIssuerInvalid_LKGExpired",
                        Token = Default.AsymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(invalidIssuerConfig, validConfig, TimeSpan.FromMilliseconds(.000001)),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        },
                        ExpectedException = new ExpectedException(typeof(SecurityTokenInvalidIssuerException))
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(Default.AsymmetricJws) + "_ConfigInvalid_ConfigKeyInvalidKidMatches_LKGExpired",
                        Token = Default.AsymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(incorrectSigningKeysConfigWithMatchingKid, validConfig, TimeSpan.FromMilliseconds(.000001)),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        },
                        ExpectedException = new ExpectedException(typeof(SecurityTokenInvalidSignatureException))
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(Default.AsymmetricJws) + "_ConfigInvalid_ConfigKeyInvalidKeyAndIssuer_LKGExpired",
                        Token = Default.AsymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(incorrectIssuerAndSigningKeysConfig, validConfig, TimeSpan.FromMilliseconds(.000001)),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        },
                        ExpectedException = new ExpectedException(typeof(SecurityTokenInvalidIssuerException))
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(Default.AsymmetricJws) + "_ConfigInvalid_IssuerSigningKeyValidationFails_LKGExpired",
                        Token = Default.AsymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(validConfigKeyValidationFails, validConfig, TimeSpan.FromMilliseconds(.000001)),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            IssuerSigningKeyValidatorUsingConfiguration = (securityKey, securityToken, validationParameters, configuration) =>
                            {
                                // mock failing on issuer validation the first time
                                if (configuration == validConfigKeyValidationFails)
                                    return false;
                                else
                                    return true;
                            },
                        },
                        ExpectedException = new ExpectedException(typeof(SecurityTokenInvalidSigningKeyException))
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(Default.AsymmetricJws) + "_ConfigInvalid_ConfigKeyInvalid_LKGIssuerInvalid_RefreshedConfigValid",
                        Token = Default.AsymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(incorrectSigningKeysConfig, invalidIssuerConfig, validConfig),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        }
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(Default.AsymmetricJws) + "_ConfigInvalid_ConfigKeyInvalid_LKGConfigKeyInvalid_RefreshedIssuerInvalid",
                        Token = Default.AsymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(incorrectSigningKeysConfig, incorrectSigningKeysConfig, invalidIssuerConfig),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        },
                        ExpectedException = new ExpectedException(typeof(SecurityTokenSignatureKeyNotFoundException))
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(Default.AsymmetricJws) + "_ConfigInvalid_ConfigKeyInvalid_LKGFeatureOff",
                        Token = Default.AsymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(incorrectSigningKeysConfig, validConfig) {UseLastKnownGoodConfiguration = false },
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        },
                        ExpectedException = new ExpectedException(typeof(SecurityTokenSignatureKeyNotFoundException))
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(Default.AsymmetricJws) + "_ConfigInvalid_ConfigKeyInvalid_LKGFeatureOff_RequestRefreshSucceeds",
                        Token = Default.AsymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(incorrectSigningKeysConfig, validConfig, validConfig) { UseLastKnownGoodConfiguration = false },
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        },
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(expiredJws) + "_ConfigKeyInvalid_LKGValid_TokenNotYetValid",
                        Token = expiredJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(incorrectSigningKeysConfig, validConfig, validConfig) { UseLastKnownGoodConfiguration = true },
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = true,
                        },
                        ExpectedException = new ExpectedException(typeof(SecurityTokenNotYetValidException))
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(Default.AsymmetricJws) + "_ConfigIssuerInvalid_AadIssuerValidatorThrow_LKGValid",
                        Token = Default.AadAsymmetricJws,
                        SetupIssuerLkg = true,
                        SetupIssuerLkgConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(validConfig),
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(invalidIssuerConfig, validConfig),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            IssuerValidator = AadIssuerValidator.GetAadIssuerValidator(Default.AadV1Authority).Validate,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        }
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(Default.AsymmetricJws) + "_ConfigIssuerInvalid_AadIssuerValidatorThrow_LKGSameInvalidIssuer",
                        Token = Default.AadAsymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(invalidIssuerConfig, incorrectIssuerAndSigningKeysConfig),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            IssuerValidator = AadIssuerValidator.GetAadIssuerValidator(Default.AadV1Authority).Validate,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        },
                        ExpectedException = new ExpectedException(typeof(SecurityTokenInvalidIssuerException))
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(Default.AsymmetricJws) + "_ConfigIssuerInvalid_AadIssuerValidatorThrow_LKGDiffInvalidIssuer",
                        Token = Default.AadAsymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(invalidIssuerConfig, incorrectIssuerAndSigningKeysConfig2),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            IssuerValidator = AadIssuerValidator.GetAadIssuerValidator(Default.AadV1Authority).Validate,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        },
                        ExpectedException = new ExpectedException(typeof(SecurityTokenInvalidIssuerException))
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(Default.AsymmetricJws) + "_ConfigInvalidSigningKeyMatchingKid_AadIssuerValidatorThrow_LKGDiffInvalidSigningKeyMatchingKidAndInvalidIssuer",
                        Token = Default.AadAsymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(incorrectSigningKeysConfigWithMatchingKid, incorrectIssuerAndSigningKeysConfig),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            IssuerValidator = AadIssuerValidator.GetAadIssuerValidator(Default.AadV1Authority).Validate,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        },
                        ExpectedException = new ExpectedException(typeof(SecurityTokenInvalidSignatureException))
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(Default.AsymmetricJws) + "_ConfigInvalid_AadIssuerValidatorThrow_LKGIssuerInvalid_RefreshedConfigKeyInvalid",
                        Token = Default.AadAsymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(invalidIssuerConfig, incorrectIssuerAndSigningKeysConfig, incorrectSigningKeysConfig),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            IssuerValidator = AadIssuerValidator.GetAadIssuerValidator(Default.AadV1Authority).Validate,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        },
                        ExpectedException = new ExpectedException(typeof(SecurityTokenSignatureKeyNotFoundException))
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(Default.AsymmetricJws) + "_ConfigInvalid_AadIssuerValidatorThrow_LKGIssuerInvalid_RequestRefreshSucceeds",
                        Token = Default.AadAsymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(invalidIssuerConfig, incorrectSigningKeysConfig, validConfig),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            IssuerValidator = AadIssuerValidator.GetAadIssuerValidator(Default.AadV1Authority).Validate,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        },
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(Default.AsymmetricJws) + "_ConfigInvalid_AadIssuerValidatorThrow_LKGSucceeds_RequestRefreshIssuerInvalid",
                        Token = Default.AadAsymmetricJws,
                        SetupIssuerLkg = true,
                        SetupIssuerLkgConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(validConfig),
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(invalidIssuerConfig, validConfig, invalidIssuerConfig),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            IssuerValidator = AadIssuerValidator.GetAadIssuerValidator(Default.AadV1Authority).Validate,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        },
                    },
                };
            }
        }

        public static TheoryData<JwtTheoryData> ValidateJWEWithLastKnownGoodTheoryData
        {
            get
            {
                var jwe = new JsonWebTokenHandler().CreateToken(Default.PayloadString, Default.SymmetricSigningCredentials, Default.SymmetricEncryptingCredentials);
                var aadJwe = new JsonWebTokenHandler().CreateToken(Default.AadPayloadString, Default.SymmetricSigningCredentials, Default.SymmetricEncryptingCredentials);
                var validConfig = new OpenIdConnectConfiguration() { TokenEndpoint = Default.Issuer + "oauth/token", Issuer = Default.Issuer };
                validConfig.SigningKeys.Add(Default.SymmetricSigningKey256);

                // a special IssuerSigningKeyValidator in the tests below is set to fail if this configuration is used in order
                // to mock issuer signing key validation failure
                var validConfigKeyValidationFails = new OpenIdConnectConfiguration() { TokenEndpoint = Default.Issuer + "/oauth/token", Issuer = Default.Issuer };
                validConfigKeyValidationFails.SigningKeys.Add(Default.SymmetricSigningKey256);

                var invalidIssuerConfig = new OpenIdConnectConfiguration() { TokenEndpoint = Default.Issuer + "/oauth/token", Issuer = Default.Issuer + "1" };
                invalidIssuerConfig.SigningKeys.Add(Default.SymmetricSigningKey256);

                var incorrectSigningKeysConfig = new OpenIdConnectConfiguration() { TokenEndpoint = Default.Issuer + "/oauth/token", Issuer = Default.Issuer };
                incorrectSigningKeysConfig.SigningKeys.Add(KeyingMaterial.X509SecurityKey2);

                var incorrectIssuerAndSigningKeysConfig = new OpenIdConnectConfiguration() { TokenEndpoint = Default.Issuer + "/oauth/token", Issuer = Default.Issuer + "1" };
                incorrectIssuerAndSigningKeysConfig.SigningKeys.Add(KeyingMaterial.X509SecurityKey2);

                var incorrectIssuerAndSigningKeysConfig2 = new OpenIdConnectConfiguration() { TokenEndpoint = Default.Issuer + "/oauth/token", Issuer = Default.Issuer + "2" };
                incorrectIssuerAndSigningKeysConfig.SigningKeys.Add(KeyingMaterial.X509SecurityKey2);

                var incorrectSigningKeysConfigWithMatchingKid = new OpenIdConnectConfiguration() { TokenEndpoint = Default.Issuer + "/oauth/token", Issuer = Default.Issuer };
                incorrectSigningKeysConfigWithMatchingKid.SigningKeys.Add(new SymmetricSecurityKey(KeyingMaterial.DefaultSymmetricSecurityKey_128.Key) { KeyId = KeyingMaterial.DefaultSymmetricSecurityKey_256.KeyId });

                var incorrectIssuerAndIncorrectSigningKeysConfigWithMatchingKid = new OpenIdConnectConfiguration() { TokenEndpoint = Default.Issuer + "/oauth/token", Issuer = Default.Issuer + "1" };
                incorrectSigningKeysConfigWithMatchingKid.SigningKeys.Add(new SymmetricSecurityKey(KeyingMaterial.DefaultSymmetricSecurityKey_64.Key) { KeyId = KeyingMaterial.DefaultSymmetricSecurityKey_256.KeyId });

                var notYetValidSecurityTokenDescriptor = Default.X509SecurityTokenDescriptor(Default.SymmetricEncryptingCredentials, Default.X509AsymmetricSigningCredentials, null);
                notYetValidSecurityTokenDescriptor.NotBefore = DateTime.UtcNow + TimeSpan.FromDays(1);
                notYetValidSecurityTokenDescriptor.Expires = DateTime.UtcNow + TimeSpan.FromDays(2);
                var notYetValidJwe = Default.Jwt(notYetValidSecurityTokenDescriptor);
                var notYetValidJweConfig = new OpenIdConnectConfiguration() { TokenEndpoint = Default.Issuer + "/oauth/token", Issuer = Default.Issuer };
                notYetValidJweConfig.SigningKeys.Add(Default.X509AsymmetricSigningCredentials.Key);

                return new TheoryData<JwtTheoryData>
                {
                    new JwtTheoryData
                    {
                        TestId = nameof(jwe) + "_ConfigKeyInvalid_LKGValid",
                        Token = jwe,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(incorrectSigningKeysConfig, validConfig),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2.Key
                        }
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(jwe) + "_ConfigKeyInvalidKidMatches_LKGValid",
                        Token = jwe,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(incorrectSigningKeysConfigWithMatchingKid, validConfig),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2.Key
                        }
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(jwe) + "_ConfigKeyInvalidKeyAndIssuer_LKGValid",
                        Token = jwe,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(incorrectIssuerAndSigningKeysConfig, validConfig),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2.Key
                        }
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(jwe) + "_ConfigIssuerInvalid_LKGValid",
                        Token = jwe,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(invalidIssuerConfig, validConfig),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2.Key
                        }
                    },
                    new JwtTheoryData
                    {
                        // SecurityTokenInvalidSigningKeyException is no longer a recoverable exception
                        TestId = nameof(jwe) + "_ConfigIssuerSigningKeyValidationFails_LKGValid",
                        Token = jwe,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(validConfigKeyValidationFails, validConfig),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            IssuerSigningKeyValidatorUsingConfiguration = (securityKey, securityToken, validationParameters, configuration) =>
                            {
                                // mock failing on issuer validation the first time
                                if (configuration == validConfigKeyValidationFails)
                                    return false;
                                else
                                    return true;
                            },
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2.Key
                        },
                        ExpectedException = new ExpectedException(typeof(SecurityTokenInvalidSigningKeyException))

                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(jwe) + "_ConfigKeyInvalid_LKGExpired",
                        Token = jwe,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(incorrectSigningKeysConfig, validConfig, TimeSpan.FromMilliseconds(.000001)),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2.Key
                        },
                        ExpectedException = new ExpectedException(typeof(SecurityTokenSignatureKeyNotFoundException))
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(jwe) + "_ConfigIssuerInvalid_LKGExpired",
                        Token = jwe,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(invalidIssuerConfig, validConfig, TimeSpan.FromMilliseconds(.000001)),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2.Key
                        },
                        ExpectedException = new ExpectedException(typeof(SecurityTokenInvalidIssuerException))
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(jwe) + "_ConfigKeyInvalidKidMatches_LKGExpired",
                        Token = jwe,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(incorrectSigningKeysConfigWithMatchingKid, validConfig, TimeSpan.FromMilliseconds(.000001)),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2.Key
                        },
                        ExpectedException = new ExpectedException(typeof(SecurityTokenInvalidSignatureException))
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(jwe) + "_ConfigKeyInvalidKeyAndIssuer_LKGExpired",
                        Token = jwe,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(incorrectIssuerAndSigningKeysConfig, validConfig, TimeSpan.FromMilliseconds(.000001)),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2.Key
                        },
                        ExpectedException = new ExpectedException(typeof(SecurityTokenInvalidIssuerException))
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(jwe) + "_ConfigIssuerSigningKeyValidationFails_LKGExpired",
                        Token = jwe,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(validConfigKeyValidationFails, validConfig, TimeSpan.FromMilliseconds(.000001)),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            IssuerSigningKeyValidatorUsingConfiguration = (securityKey, securityToken, validationParameters, configuration) =>
                            {
                                // mock failing on issuer validation the first time
                                if (configuration == validConfigKeyValidationFails)
                                    return false;
                                else
                                    return true;
                            },
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2.Key
                        },
                        ExpectedException = new ExpectedException(typeof(SecurityTokenInvalidSigningKeyException))
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(jwe) + "_ConfigKeyInvalid_LKGIssuerInvalid_RefreshedConfigValid",
                        Token = jwe,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(incorrectSigningKeysConfig, invalidIssuerConfig, validConfig),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2.Key
                        }
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(jwe) + "_ConfigKeyInvalid_LKGConfigKeyInvalid_RefreshedIssuerInvalid",
                        Token = jwe,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(incorrectSigningKeysConfig, incorrectSigningKeysConfig, invalidIssuerConfig),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2.Key
                        },
                        ExpectedException = new ExpectedException(typeof(SecurityTokenSignatureKeyNotFoundException))
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(jwe) + "_ConfigKeyInvalid_LKGFeatureOff",
                        Token = jwe,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(incorrectSigningKeysConfig, validConfig) {UseLastKnownGoodConfiguration = false },
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2.Key
                        },
                        ExpectedException = new ExpectedException(typeof(SecurityTokenSignatureKeyNotFoundException))
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(jwe) + "_ConfigKeyInvalid_LKGFeatureOff_RequestRefreshSucceeds",
                        Token = jwe,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(incorrectSigningKeysConfig, validConfig, validConfig) {UseLastKnownGoodConfiguration = false },
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2.Key
                        },
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(notYetValidJwe) + "_ConfigKeyInvalid_LKGValid_TokenNotYetValid",
                        Token = notYetValidJwe,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(incorrectSigningKeysConfig, notYetValidJweConfig, notYetValidJweConfig) { UseLastKnownGoodConfiguration = true },
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = true,
                            TokenDecryptionKey = Default.SymmetricEncryptingCredentials.Key
                        },
                        ExpectedException = new ExpectedException(typeof(SecurityTokenNotYetValidException))
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(aadJwe) + "_ConfigIssuerInvalid_AadIssuerValidatorThrow_LKGValid",
                        Token = aadJwe,
                        SetupIssuerLkg = true,
                        SetupIssuerLkgConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(validConfig),
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(invalidIssuerConfig, validConfig),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            IssuerValidator = AadIssuerValidator.GetAadIssuerValidator(Default.AadV1Authority).Validate,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2.Key
                        }
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(aadJwe) + "_ConfigIssuerInvalid_AadIssuerValidatorThrow_LKGSameInvalidIssuer",
                        Token = aadJwe,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(invalidIssuerConfig, incorrectIssuerAndSigningKeysConfig),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            IssuerValidator = AadIssuerValidator.GetAadIssuerValidator(Default.AadV1Authority).Validate,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2.Key
                        },
                        ExpectedException = new ExpectedException(typeof(SecurityTokenInvalidIssuerException))
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(aadJwe) + "_ConfigIssuerInvalid_AadIssuerValidatorThrow_LKGDiffInvalidIssuer",
                        Token = aadJwe,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(invalidIssuerConfig, incorrectIssuerAndSigningKeysConfig2),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            IssuerValidator = AadIssuerValidator.GetAadIssuerValidator(Default.AadV1Authority).Validate,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2.Key
                        },
                        ExpectedException = new ExpectedException(typeof(SecurityTokenInvalidIssuerException))
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(aadJwe) + "_ConfigInvalidSigningKeyMatchingKid_AadIssuerValidatorThrow_LKGDiffInvalidSigningKeyMatchingKidAndInvalidIssuer",
                        Token = aadJwe,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(incorrectSigningKeysConfigWithMatchingKid, incorrectIssuerAndIncorrectSigningKeysConfigWithMatchingKid),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            IssuerValidator = AadIssuerValidator.GetAadIssuerValidator(Default.AadV1Authority).Validate,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2.Key
                        },
                        ExpectedException = new ExpectedException(typeof(SecurityTokenInvalidSignatureException))
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(aadJwe) + "_ConfigInvalid_AadIssuerValidatorThrow_LKGIssuerInvalid_RefreshedConfigKeyInvalid",
                        Token = aadJwe,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(invalidIssuerConfig, incorrectIssuerAndSigningKeysConfig, incorrectSigningKeysConfig),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            IssuerValidator = AadIssuerValidator.GetAadIssuerValidator(Default.AadV1Authority).Validate,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2.Key

                        },
                        ExpectedException = new ExpectedException(typeof(SecurityTokenSignatureKeyNotFoundException))
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(aadJwe) + "_ConfigInvalid_AadIssuerValidatorThrow_LKGSucceeds_RequestRefreshIssuerInvalid",
                        Token = aadJwe,
                        SetupIssuerLkg = true,
                        SetupIssuerLkgConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(validConfig),
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(invalidIssuerConfig, validConfig, invalidIssuerConfig),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            IssuerValidator = AadIssuerValidator.GetAadIssuerValidator(Default.AadV1Authority).Validate,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2.Key
                        },
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(aadJwe) + "_ConfigInvalid_AadIssuerValidatorThrow_LKGIssuerInvalid_RequestRefreshSucceeds",
                        Token = aadJwe,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(invalidIssuerConfig, incorrectSigningKeysConfig, validConfig),
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            IssuerValidator = AadIssuerValidator.GetAadIssuerValidator(Default.AadV1Authority).Validate,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2.Key
                        },
                    },
                };
            }
        }
    }
}
