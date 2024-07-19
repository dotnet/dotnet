// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Saml2;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Validators.Tests
{
    // Serialize as one of the tests depends on static state (app context)
    [Collection(nameof(AadSigningKeyIssuerValidatorTests))]
    public class AadSigningKeyIssuerValidatorTests
    {
        [Theory, MemberData(nameof(EnableAadSigningKeyIssuerValidationTestCases))]
        public async Task EnableAadSigningKeyIssuerValidationTests(AadSigningKeyIssuerTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.EnableAadSigningKeyIssuerValidationTests", theoryData);
            try
            {
                // set delegates
                bool delegateSet = false;
                if (theoryData.SetDelegateUsingConfig)
                {
                    theoryData.TokenValidationParameters.IssuerSigningKeyValidatorUsingConfiguration = (securityKey, securityToken, tvp, config) => { delegateSet = true; return true; };
                }
                else if (theoryData.SetDelegateWithoutConfig)
                {
                    theoryData.TokenValidationParameters.IssuerSigningKeyValidatorUsingConfiguration = null;
                    theoryData.TokenValidationParameters.IssuerSigningKeyValidator = (securityKey, securityToken, tvp) => { delegateSet = true; return true; };
                }

                var handler = new JsonWebTokenHandler();
                var jwt = handler.ReadJsonWebToken(Default.AsymmetricJws);
                AadIssuerValidator.GetAadIssuerValidator(Default.AadV1Authority).ConfigurationManagerV1 = theoryData.TokenValidationParameters.ConfigurationManager;
                theoryData.TokenValidationParameters.EnableAadSigningKeyIssuerValidation();

                var validationResult = await handler.ValidateTokenAsync(jwt, theoryData.TokenValidationParameters).ConfigureAwait(false);
                theoryData.ExpectedException.ProcessNoException(context);
                Assert.NotNull(theoryData.TokenValidationParameters.IssuerSigningKeyValidatorUsingConfiguration);
                Assert.True(validationResult.IsValid);

                // verify delegates were executed
                if (theoryData.SetDelegateUsingConfig || theoryData.SetDelegateWithoutConfig)
                    Assert.True(delegateSet);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<AadSigningKeyIssuerTheoryData> EnableAadSigningKeyIssuerValidationTestCases()
        {
            var signingKeysConfig = new OpenIdConnectConfiguration() { TokenEndpoint = Default.Issuer + "oauth/token", Issuer = Default.Issuer };
            signingKeysConfig.SigningKeys.Add(KeyingMaterial.DefaultX509Key_2048);
            var validationParameters = new TokenValidationParameters()
            {
                ConfigurationManager = new StaticConfigurationManager<OpenIdConnectConfiguration>(signingKeysConfig),
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                ValidateLifetime = false
            };

            var theoryData = new TheoryData<AadSigningKeyIssuerTheoryData>
            {
                new AadSigningKeyIssuerTheoryData
                {
                    TestId = "IssuerSigningKeyValidatorUsingConfiguration_Delegate_IsSetByWilson",
                    TokenValidationParameters = validationParameters
                },
                new AadSigningKeyIssuerTheoryData
                {
                    TestId = "IssuerSigningKeyValidatorUsingConfiguration_Delegate_IsSetByDeveloper",
                    TokenValidationParameters = validationParameters,
                    SetDelegateUsingConfig = true,
                },
                new AadSigningKeyIssuerTheoryData
                {
                    TestId = "IssuerSigningKeyValidator_Delegate_IsSetByDeveloper",
                    TokenValidationParameters = validationParameters,
                    SetDelegateWithoutConfig = true,
                }
            };

            return theoryData;
        }

        [Theory, MemberData(nameof(ValidateIssuerSigningKeyCertificateTestCases))]
        public void ValidateIssuerSigningKeyCertificateTests(AadSigningKeyIssuerTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateIssuerSigningKeyCertificateTests", theoryData);

            try
            {
                var result = AadTokenValidationParametersExtension.ValidateIssuerSigningKeyCertificate(theoryData.SecurityKey, theoryData.TokenValidationParameters);
                theoryData.ExpectedException.ProcessNoException(context);
                Assert.True(result);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<AadSigningKeyIssuerTheoryData> ValidateIssuerSigningKeyCertificateTestCases()
        {
            var theoryData = new TheoryData<AadSigningKeyIssuerTheoryData>
            {
                new AadSigningKeyIssuerTheoryData
                {
                    TestId = "SecurityKeyIsNull",
                    SecurityKey = null,
                    TokenValidationParameters = new TokenValidationParameters() { RequireSignedTokens = true, ValidateIssuerSigningKey = true },
                    ExpectedException = ExpectedException.ArgumentNullException("IDX40007:")
                },
                new AadSigningKeyIssuerTheoryData
                {
                    TestId = "SecurityKeyIsNull_RequireSignedTokensFalse",
                    SecurityKey = null,
                    TokenValidationParameters = new TokenValidationParameters() { RequireSignedTokens = false, ValidateIssuerSigningKey = true },
                },
                new AadSigningKeyIssuerTheoryData
                {
                    TestId = "ServiceAcceptsUnsignedTokens",
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    TokenValidationParameters = new TokenValidationParameters() { RequireSignedTokens = false, ValidateIssuerSigningKey = true },
                },
                new AadSigningKeyIssuerTheoryData
                {
                    TestId = "SkipValidaingIssuerSigningKey",
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    TokenValidationParameters = new TokenValidationParameters() { RequireSignedTokens = true, ValidateIssuerSigningKey = false },
                },
                new AadSigningKeyIssuerTheoryData
                {
                    TestId = "SkipValidaingIssuerSigningKey",
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    TokenValidationParameters = new TokenValidationParameters() { RequireSignedTokens = false, ValidateIssuerSigningKey = false },
                },
                new AadSigningKeyIssuerTheoryData
                {
                    TestId = "CertificateLifeTimeValidated",
                    SecurityKey = KeyingMaterial.X509SecurityKeySelfSigned1024_SHA256,
                    TokenValidationParameters = new TokenValidationParameters() { RequireSignedTokens = true, ValidateIssuerSigningKey = true },
                }
            };

            return theoryData;
        }

        [Theory, MemberData(nameof(ValidateIssuerSigningKeyTestCases))]
        public void ValidateIssuerSigningKeyTests(AadSigningKeyIssuerTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateIssuerSigningKeyTests", theoryData);

            try
            {
                theoryData.SetupAction?.Invoke();
                var result = AadTokenValidationParametersExtension.ValidateIssuerSigningKey(theoryData.SecurityKey, theoryData.SecurityToken, theoryData.OpenIdConnectConfiguration);
                theoryData.ExpectedException.ProcessNoException(context);
                Assert.True(result);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }
            finally
            {
                theoryData.TearDownAction?.Invoke();
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<AadSigningKeyIssuerTheoryData> ValidateIssuerSigningKeyTestCases
        {
            get
            {
                var theoryData = new TheoryData<AadSigningKeyIssuerTheoryData>();

                var tidClaim = new Claim(ValidatorConstants.ClaimNameTid, ValidatorConstants.TenantIdAsGuid);
                var issClaim = new Claim(ValidatorConstants.ClaimNameIss, ValidatorConstants.AadIssuer);
                var jwtSecurityToken = new JwtSecurityToken(issuer: ValidatorConstants.AadIssuer, claims: new[] { issClaim, tidClaim });
                theoryData.Add(new AadSigningKeyIssuerTheoryData
                {
                    TestId = "NullSecurityKey",
                    SecurityKey = null,
                    SecurityToken = jwtSecurityToken,
                    OpenIdConnectConfiguration = GetConfigurationMock()
                });

                theoryData.Add(new AadSigningKeyIssuerTheoryData
                {
                    TestId = "NullSecurityToken",
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    SecurityToken = null,
                    OpenIdConnectConfiguration = GetConfigurationMock(),
                    ExpectedException = ExpectedException.ArgumentNullException("IDX10000")
                });

                theoryData.Add(new AadSigningKeyIssuerTheoryData
                {
                    TestId = "NullConfiguration",
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    SecurityToken = jwtSecurityToken,
                    OpenIdConnectConfiguration = null,
                });

                theoryData.Add(new AadSigningKeyIssuerTheoryData
                {
                    TestId = "NoSigningKeysInConfiguration",
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    SecurityToken = jwtSecurityToken,
                    OpenIdConnectConfiguration = new OpenIdConnectConfiguration()
                });

                var mockConfiguration = GetConfigurationMock();
                mockConfiguration.JsonWebKeySet.Keys.Add(KeyingMaterial.JsonWebKeyP384);
                theoryData.Add(new AadSigningKeyIssuerTheoryData
                {
                    TestId = "NoMatchingKeysInConfiguration",
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    SecurityToken = jwtSecurityToken,
                    OpenIdConnectConfiguration = new OpenIdConnectConfiguration()
                });

                theoryData.Add(new AadSigningKeyIssuerTheoryData
                {
                    TestId = "MissingIssuerInConfiguration",
                    SecurityKey = KeyingMaterial.JsonWebKeyP384,
                    SecurityToken = jwtSecurityToken,
                    OpenIdConnectConfiguration = mockConfiguration
                });

                var jwk = KeyingMaterial.JsonWebKeySymmetric128;
                jwk.AdditionalData.Add(OpenIdProviderMetadataNames.Issuer, " ");
                mockConfiguration.JsonWebKeySet.Keys.Add(jwk);
                theoryData.Add(new AadSigningKeyIssuerTheoryData
                {
                    TestId = "WhitespaceForIssuerInConfiguration",
                    SecurityKey = KeyingMaterial.JsonWebKeySymmetric128,
                    SecurityToken = jwtSecurityToken,
                    OpenIdConnectConfiguration = mockConfiguration,
                });

                jwk = KeyingMaterial.JsonWebKeyP521;
                jwk.AdditionalData.Add(OpenIdProviderMetadataNames.Issuer, ValidatorConstants.UsGovIssuer);
                mockConfiguration.JsonWebKeySet.Keys.Add(jwk);
                theoryData.Add(new AadSigningKeyIssuerTheoryData
                {
                    TestId = "JST_TokenIssuer_MismatchesWith_SigningKeyIssuer",
                    SecurityKey = KeyingMaterial.JsonWebKeyP521,
                    SecurityToken = jwtSecurityToken,
                    OpenIdConnectConfiguration = mockConfiguration,
                    ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException("IDX40005")
                });

                List<Claim> claims = new List<Claim>
                {
                    tidClaim,
                    issClaim
                };
                var jsonWebToken = new JsonWebToken(Default.Jwt(Default.SecurityTokenDescriptor(Default.SymmetricSigningCredentials, claims)));
                theoryData.Add(new AadSigningKeyIssuerTheoryData
                {
                    TestId = "JWT_TokenIssuer_MismatchesWith_SigningKeyIssuer",
                    SecurityKey = KeyingMaterial.JsonWebKeyP521,
                    SecurityToken = jsonWebToken,
                    OpenIdConnectConfiguration = mockConfiguration,
                    ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException("IDX40005")
                });

                jwk = KeyingMaterial.JsonWebKeyP256;
                jwk.AdditionalData.Add(OpenIdProviderMetadataNames.Issuer, ValidatorConstants.AadIssuerV2CommonAuthority);
                mockConfiguration.JsonWebKeySet.Keys.Add(jwk);
                mockConfiguration.Issuer = ValidatorConstants.AadIssuerV2CommonAuthority;
                theoryData.Add(new AadSigningKeyIssuerTheoryData
                {
                    TestId = "HappyPath_TokenIssuer_Matches_SigningKeyIssuer",
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    SecurityToken = jwtSecurityToken,
                    OpenIdConnectConfiguration = mockConfiguration
                });

                theoryData.Add(new AadSigningKeyIssuerTheoryData
                {
                    TestId = "MissingTenantIdClaimInToken",
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    SecurityToken = new JwtSecurityToken(),
                    OpenIdConnectConfiguration = mockConfiguration,
                    ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException("IDX40009")
                });

                theoryData.Add(new AadSigningKeyIssuerTheoryData
                {
                    TestId = "WrongSecurityKeyType",
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    SecurityToken = new Saml2SecurityToken(new Saml2Assertion(new Saml2NameIdentifier("nameIdentifier"))),
                    OpenIdConnectConfiguration = mockConfiguration,
                    ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException("IDX40010")
                });

                theoryData.Add(new AadSigningKeyIssuerTheoryData
                {
                    TestId = "JST_TokenIssuer_MismatchesWith_TenantIdInToken",
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    SecurityToken = new JwtSecurityToken(issuer: ValidatorConstants.AadIssuer, claims: new[] { issClaim, new Claim(ValidatorConstants.ClaimNameTid, ValidatorConstants.B2CTenantAsGuid) }),
                    OpenIdConnectConfiguration = mockConfiguration,
                    ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException("IDX40004")
                });

                claims = new List<Claim>
                {
                    new Claim(ValidatorConstants.ClaimNameTid, ValidatorConstants.B2CTenantAsGuid),
                    issClaim
                };
                jsonWebToken = new JsonWebToken(Default.Jwt(Default.SecurityTokenDescriptor(Default.SymmetricSigningCredentials, claims)));
                theoryData.Add(new AadSigningKeyIssuerTheoryData
                {
                    TestId = "JWT_TokenIssuer_MismatchesWith_TenantIdInToken",
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    SecurityToken = jsonWebToken,
                    OpenIdConnectConfiguration = mockConfiguration,
                    ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException("IDX40004")
                });

                theoryData.Add(new AadSigningKeyIssuerTheoryData
                {
                    TestId = "Doesnt_Fail_With_Switch",
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    SecurityToken = new JwtSecurityToken(),
                    OpenIdConnectConfiguration = mockConfiguration,
                    SetupAction = () => AppContext.SetSwitch(AadTokenValidationParametersExtension.DontFailOnMissingTidSwitch, true),
                    TearDownAction = () => AppContext.SetSwitch(AadTokenValidationParametersExtension.DontFailOnMissingTidSwitch, false)
                });

                theoryData.Add(new AadSigningKeyIssuerTheoryData
                {
                    TestId = "Fail_With_Switch_False",
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    SecurityToken = new JwtSecurityToken(),
                    OpenIdConnectConfiguration = mockConfiguration,
                    ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException("IDX40009"),
                    SetupAction = () => AppContext.SetSwitch(AadTokenValidationParametersExtension.DontFailOnMissingTidSwitch, false),
                    TearDownAction = () => AppContext.SetSwitch(AadTokenValidationParametersExtension.DontFailOnMissingTidSwitch, isEnabled: false)
                });

                theoryData.Add(new AadSigningKeyIssuerTheoryData
                {
                    TestId = "Doesnt_Fail_With_Switch",
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    SecurityToken = new JwtSecurityToken(),
                    OpenIdConnectConfiguration = mockConfiguration,
                    SetupAction = () => AppContext.SetSwitch(AadTokenValidationParametersExtension.DontFailOnMissingTidSwitch, true),
                    TearDownAction = () => AppContext.SetSwitch(AadTokenValidationParametersExtension.DontFailOnMissingTidSwitch, false)
                });

                theoryData.Add(new AadSigningKeyIssuerTheoryData
                {
                    TestId = "Fail_With_Switch_False_JsonWebToken",
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    SecurityToken = new JsonWebToken(Default.Jwt(Default.SecurityTokenDescriptor(Default.SymmetricSigningCredentials, [issClaim]))),
                    OpenIdConnectConfiguration = mockConfiguration,
                    ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException("IDX40009"),
                    SetupAction = () => AppContext.SetSwitch(AadTokenValidationParametersExtension.DontFailOnMissingTidSwitch, false),
                    TearDownAction = () => AppContext.SetSwitch(AadTokenValidationParametersExtension.DontFailOnMissingTidSwitch, isEnabled: false)
                });

                theoryData.Add(new AadSigningKeyIssuerTheoryData
                {
                    TestId = "Doesnt_Fail_With_Switch_JsonWebToken",
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    SecurityToken = new JsonWebToken(Default.Jwt(Default.SecurityTokenDescriptor(Default.SymmetricSigningCredentials, [issClaim]))),
                    OpenIdConnectConfiguration = mockConfiguration,
                    SetupAction = () => AppContext.SetSwitch(AadTokenValidationParametersExtension.DontFailOnMissingTidSwitch, true),
                    TearDownAction = () => AppContext.SetSwitch(AadTokenValidationParametersExtension.DontFailOnMissingTidSwitch, false)
                });

                theoryData.Add(new AadSigningKeyIssuerTheoryData
                {
                    TestId = "Fails_With_Multiple_tids",
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    SecurityToken = new JsonWebToken(
                        Default.Jwt(Default.SecurityTokenDescriptor(
                            Default.SymmetricSigningCredentials,
                            [tidClaim, issClaim, new Claim("TID", Guid.NewGuid().ToString())]))),
                    ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException("IDX40011"),
                    OpenIdConnectConfiguration = mockConfiguration
                });

                theoryData.Add(new AadSigningKeyIssuerTheoryData
                {
                    TestId = "Fails_With_Multiple_tids_alternate_order",
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    SecurityToken = new JsonWebToken(
                        Default.Jwt(Default.SecurityTokenDescriptor(
                            Default.SymmetricSigningCredentials,
                            [issClaim, new Claim("TID", Guid.NewGuid().ToString()), tidClaim]))),
                    ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException("IDX40011"),
                    OpenIdConnectConfiguration = mockConfiguration
                });

                theoryData.Add(new AadSigningKeyIssuerTheoryData
                {
                    TestId = "Fails_With_no standard_tid",
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    SecurityToken = new JsonWebToken(
                        Default.Jwt(Default.SecurityTokenDescriptor(
                            Default.SymmetricSigningCredentials,
                            [issClaim, new Claim("TID", Guid.NewGuid().ToString())]))),
                    ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException("IDX40009"),
                    OpenIdConnectConfiguration = mockConfiguration
                });

                return theoryData;
            }
        }

        private static OpenIdConnectConfiguration GetConfigurationMock()
        {
            var config = new OpenIdConnectConfiguration();
            config.JsonWebKeySet = new JsonWebKeySet();
            config.JsonWebKeySet.Keys.Add(KeyingMaterial.JsonWebKeyP384);
            return config;
        }

        public class AadSigningKeyIssuerTheoryData : TheoryDataBase
        {
            public SecurityKey SecurityKey { get; set; }

            public TokenValidationParameters TokenValidationParameters { get; set; }

            public SecurityToken SecurityToken { get; set; }

            public OpenIdConnectConfiguration OpenIdConnectConfiguration { get; set; }

            public bool SetDelegateUsingConfig { get; set; } = false;

            public bool SetDelegateWithoutConfig { get; set; } = false;

            public Action SetupAction { get; set; }

            public Action TearDownAction { get; set; }
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
