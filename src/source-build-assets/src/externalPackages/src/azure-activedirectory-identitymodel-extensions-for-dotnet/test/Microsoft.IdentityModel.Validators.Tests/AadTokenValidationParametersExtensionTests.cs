// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using Microsoft.IdentityModel.Tokens.Saml2;
using Xunit;

namespace Microsoft.IdentityModel.Validators.Tests
{
    // Serialize as one of the tests depends on static state (app context)
    [Collection(nameof(AadTokenValidationParametersExtensionTests))]
    public class AadTokenValidationParametersExtensionTests
    {
        [Theory, MemberData(nameof(EnableEntraIdSigningKeyCloudInstanceValidationTestCases), DisableDiscoveryEnumeration = true)]
        public async Task EnableEntraIdSigningKeyCloudInstanceValidationTests(EnableEntraIdSigningKeyValidationTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.EnableAadSigningKeyValidationTests", theoryData);
            try
            {

                ValidationParameters validationParameters = TestUtilities.CreateFromTokenValidationParameters(theoryData.TokenValidationParameters);
                validationParameters.AudienceValidator = SkipValidationDelegates.SkipAudienceValidation;
                validationParameters.IssuerValidatorAsync = SkipValidationDelegates.SkipIssuerValidation;

                // set delegates
                bool validationParametersDelegateSet = false;
                bool delegateSet = false;

                if (theoryData.SetDelegateUsingConfig)
                {
                    theoryData.TokenValidationParameters.IssuerSigningKeyValidatorUsingConfiguration = (securityKey, securityToken, tvp, config) => { delegateSet = true; return true; };
                    validationParameters.SignatureKeyValidator = (securityKey, securityToken, validationParameters, callContext) =>
                    {
                        validationParametersDelegateSet = true;
                        return new ValidatedSignatureKey(DateTime.UtcNow, DateTime.UtcNow, DateTime.UtcNow);
                    };
                }
                else if (theoryData.SetDelegateWithoutConfig)
                {
                    theoryData.TokenValidationParameters.IssuerSigningKeyValidatorUsingConfiguration = null;
                    theoryData.TokenValidationParameters.IssuerSigningKeyValidator = (securityKey, securityToken, tvp) => { delegateSet = true; return true; };
                    validationParameters.SignatureKeyValidator = (securityKey, securityToken, validationParameters, callContext) =>
                    {
                        validationParametersDelegateSet = true;
                        return new ValidatedSignatureKey(DateTime.UtcNow, DateTime.UtcNow, DateTime.UtcNow);
                    };
                }

                var handler = new JsonWebTokenHandler();
                AadIssuerValidator.GetAadIssuerValidator(Default.AadV1Authority).ConfigurationManagerV1 = theoryData.TokenValidationParameters.ConfigurationManager;
                theoryData.TokenValidationParameters.EnableEntraIdSigningKeyCloudInstanceValidation();

                var validationResult = await handler.ValidateTokenAsync(theoryData.Token, theoryData.TokenValidationParameters);
                ValidationResult<ValidatedToken, ValidationError> validatedToken = await handler.ValidateTokenAsync(theoryData.Token, validationParameters, new CallContext(), CancellationToken.None);

                theoryData.ExpectedException.ProcessNoException(context);
                Assert.NotNull(theoryData.TokenValidationParameters.IssuerSigningKeyValidatorUsingConfiguration);
                Assert.Equal(theoryData.ExpectedValidationResult, validationResult.IsValid);

                // verify delegates were executed
                if (theoryData.ExpectedValidationResult && (theoryData.SetDelegateUsingConfig || theoryData.SetDelegateWithoutConfig))
                {
                    Assert.True(delegateSet);
                    Assert.True(validationParametersDelegateSet);
                }
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<EnableEntraIdSigningKeyValidationTheoryData> EnableEntraIdSigningKeyCloudInstanceValidationTestCases()
        {
            var signingKeysConfig = new OpenIdConnectConfiguration() { TokenEndpoint = Default.Issuer + "oauth/token", Issuer = Default.Issuer };
            signingKeysConfig.SigningKeys.Add(KeyingMaterial.DefaultX509Key_2048);

            var theoryData = new TheoryData<EnableEntraIdSigningKeyValidationTheoryData>();
            theoryData.Add(new EnableEntraIdSigningKeyValidationTheoryData("IssuerSigningKeyValidatorUsingConfiguration_Delegate_IsSetByWilson")
            {
                Token = Default.AsymmetricJws,
                TokenValidationParameters = SetupTokenValidationParametersMock(signingKeysConfig),
                ExpectedValidationResult = true,
            });

            theoryData.Add(new EnableEntraIdSigningKeyValidationTheoryData("IssuerSigningKeyValidatorUsingConfiguration_Delegate_IsSetByDeveloper")
            {
                Token = Default.AsymmetricJws,
                TokenValidationParameters = SetupTokenValidationParametersMock(signingKeysConfig),
                SetDelegateUsingConfig = true,
                ExpectedValidationResult = true,
            });

            theoryData.Add(new EnableEntraIdSigningKeyValidationTheoryData("IssuerSigningKeyValidator_Delegate_IsSetByDeveloper")
            {
                Token = Default.AsymmetricJws,
                TokenValidationParameters = SetupTokenValidationParametersMock(signingKeysConfig),
                SetDelegateWithoutConfig = true,
                ExpectedValidationResult = true,
            });

            signingKeysConfig = SetupOpenIdConnectConfigurationMock(configurationCloudInstanceName: Default.CloudInstanceName, siginingKeyCloudInstanceName: Default.CloudInstanceName);
            theoryData.Add(new EnableEntraIdSigningKeyValidationTheoryData("IssuerSigningKeyValidatorUsingConfiguration_CloudInstanceNamesIsMatched_ValidationSuccess")
            {
                Token = Default.AsymmetricJws,
                TokenValidationParameters = SetupTokenValidationParametersMock(signingKeysConfig),
                ExpectedValidationResult = true,
            });

            signingKeysConfig = SetupOpenIdConnectConfigurationMock(configurationCloudInstanceName: "microsoftonline.com", siginingKeyCloudInstanceName: "microsoftonline.us");
            theoryData.Add(new EnableEntraIdSigningKeyValidationTheoryData("IssuerSigningKeyValidatorUsingConfiguration_CloudInstanceNameIsNotMatched_ValidationFailed")
            {
                Token = Default.AsymmetricJws,
                TokenValidationParameters = SetupTokenValidationParametersMock(signingKeysConfig),
                ExpectedValidationResult = false,
            });

            signingKeysConfig = SetupOpenIdConnectConfigurationMock(configurationCloudInstanceName: "microsoftonline.com", siginingKeyCloudInstanceName: "microsoftonline.us");
            theoryData.Add(new EnableEntraIdSigningKeyValidationTheoryData("IssuerSigningKeyValidatorUsingConfiguration_CloudInstanceNameIsNotMatched_CustomDelegateIsSet_ValidationFailed")
            {
                Token = Default.AsymmetricJws,
                TokenValidationParameters = SetupTokenValidationParametersMock(signingKeysConfig),
                ExpectedValidationResult = false,
                SetDelegateUsingConfig = true,
            });

            signingKeysConfig = SetupOpenIdConnectConfigurationMock(configurationCloudInstanceName: Default.CloudInstanceName, siginingKeyCloudInstanceName: null);
            theoryData.Add(new EnableEntraIdSigningKeyValidationTheoryData("IssuerSigningKeyValidatorUsingConfiguration_CloudInstanceNamePassed_SecurityKeyCloudInstanceAbsent_ValidationSuccess")
            {
                Token = Default.AsymmetricJws,
                TokenValidationParameters = SetupTokenValidationParametersMock(signingKeysConfig),
                ExpectedValidationResult = true,
            });

            signingKeysConfig = SetupOpenIdConnectConfigurationMock(configurationCloudInstanceName: null, siginingKeyCloudInstanceName: Default.CloudInstanceName);
            theoryData.Add(new EnableEntraIdSigningKeyValidationTheoryData("IssuerSigningKeyValidatorUsingConfiguration_CloudInstanceNamePassed_ConfigurationCloudInstanceAbsent_ValidationSuccess")
            {
                Token = Default.AsymmetricJws,
                TokenValidationParameters = SetupTokenValidationParametersMock(signingKeysConfig),
                ExpectedValidationResult = true,
            });

            OpenIdConnectConfiguration SetupOpenIdConnectConfigurationMock(string configurationCloudInstanceName = null, string siginingKeyCloudInstanceName = null)
            {
                OpenIdConnectConfiguration config = new OpenIdConnectConfiguration() { TokenEndpoint = Default.Issuer + "oauth/token", Issuer = Default.Issuer };
                if (configurationCloudInstanceName != null)
                {
                    config.AdditionalData.Add("cloud_instance_name", configurationCloudInstanceName);
                }
                config.SigningKeys.Add(KeyingMaterial.DefaultX509Key_2048);
                config.JsonWebKeySet = new JsonWebKeySet();
                var jwk = KeyingMaterial.JsonWebKeyX509_2048;
                config.JsonWebKeySet.Keys.Add(jwk);
                if (siginingKeyCloudInstanceName != null)
                {
                    jwk.AdditionalData.Add("cloud_instance_name", siginingKeyCloudInstanceName);
                }

                return config;
            }

            return theoryData;
        }

        [Theory, MemberData(nameof(EnableAadSigningKeyIssuerValidationTestCases), DisableDiscoveryEnumeration = true)]
        public async Task EnableAadSigningKeyIssuerValidationTests(EnableEntraIdSigningKeyValidationTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.EnableAadSigningKeyIssuerValidationTests", theoryData);
            try
            {
                ValidationParameters validationParameters = TestUtilities.CreateFromTokenValidationParameters(theoryData.TokenValidationParameters);
                validationParameters.AudienceValidator = SkipValidationDelegates.SkipAudienceValidation;
                validationParameters.IssuerValidatorAsync = SkipValidationDelegates.SkipIssuerValidation;

                // set delegates
                bool delegateSet = false;
                bool validationParametersDelegateSet = false;
                if (theoryData.SetDelegateUsingConfig)
                {
                    theoryData.TokenValidationParameters.IssuerSigningKeyValidatorUsingConfiguration = (securityKey, securityToken, tvp, config) => { delegateSet = true; return true; };
                    validationParameters.SignatureKeyValidator = (securityKey, securityToken, validationParameters, callContext) =>
                    {
                        validationParametersDelegateSet = true;
                        return new ValidatedSignatureKey(DateTime.UtcNow, DateTime.UtcNow, DateTime.UtcNow);
                    };
                }
                else if (theoryData.SetDelegateWithoutConfig)
                {
                    theoryData.TokenValidationParameters.IssuerSigningKeyValidatorUsingConfiguration = null;
                    theoryData.TokenValidationParameters.IssuerSigningKeyValidator = (securityKey, securityToken, tvp) => { delegateSet = true; return true; };
                    validationParameters.SignatureKeyValidator = (securityKey, securityToken, validationParameters, callContext) =>
                    {
                        validationParametersDelegateSet = true;
                        return new ValidatedSignatureKey(DateTime.UtcNow, DateTime.UtcNow, DateTime.UtcNow);
                    };

                }

                var handler = new JsonWebTokenHandler();
                var jwt = handler.ReadJsonWebToken(Default.AsymmetricJws);
                AadIssuerValidator.GetAadIssuerValidator(Default.AadV1Authority).ConfigurationManagerV1 = theoryData.TokenValidationParameters.ConfigurationManager;
                theoryData.TokenValidationParameters.EnableAadSigningKeyIssuerValidation();

                var validationResult = await handler.ValidateTokenAsync(jwt, theoryData.TokenValidationParameters);
                ValidationResult<ValidatedToken, ValidationError> validatedToken = await handler.ValidateTokenAsync(jwt, validationParameters, new CallContext(), CancellationToken.None);
                theoryData.ExpectedException.ProcessNoException(context);

                Assert.NotNull(theoryData.TokenValidationParameters.IssuerSigningKeyValidatorUsingConfiguration);
                Assert.True(validationResult.IsValid);

                // verify delegates were executed
                if (theoryData.SetDelegateUsingConfig || theoryData.SetDelegateWithoutConfig)
                {
                    Assert.True(delegateSet);
                    Assert.True(validationParametersDelegateSet);
                }
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<EnableEntraIdSigningKeyValidationTheoryData> EnableAadSigningKeyIssuerValidationTestCases()
        {
            var signingKeysConfig = new OpenIdConnectConfiguration() { TokenEndpoint = Default.Issuer + "oauth/token", Issuer = Default.Issuer };
            signingKeysConfig.SigningKeys.Add(KeyingMaterial.DefaultX509Key_2048);

            var theoryData = new TheoryData<EnableEntraIdSigningKeyValidationTheoryData>
            {
                new EnableEntraIdSigningKeyValidationTheoryData("IssuerSigningKeyValidatorUsingConfiguration_Delegate_IsSetByWilson")
                {
                    TokenValidationParameters = SetupTokenValidationParametersMock(signingKeysConfig),
                },
                new EnableEntraIdSigningKeyValidationTheoryData("IssuerSigningKeyValidatorUsingConfiguration_Delegate_IsSetByDeveloper")
                {
                    TokenValidationParameters = SetupTokenValidationParametersMock(signingKeysConfig),
                    SetDelegateUsingConfig = true,
                },
                new EnableEntraIdSigningKeyValidationTheoryData("IssuerSigningKeyValidator_Delegate_IsSetByDeveloper")
                {
                    TokenValidationParameters = SetupTokenValidationParametersMock(signingKeysConfig),
                    SetDelegateWithoutConfig = true,
                }
            };

            return theoryData;
        }

        [Theory, MemberData(nameof(ValidateSigningKeyCloudInstanceNameTestCases), DisableDiscoveryEnumeration = true)]
        public void ValidateSigningKeyCloudInstanceNameTests(AadSigningKeyTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateSigningKeyCloudInstanceNameTests", theoryData);

            try
            {
                AadTokenValidationParametersExtension.ValidateSigningKeyCloudInstance(theoryData.SecurityKey, theoryData.OpenIdConnectConfiguration);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }
            finally
            {
                AppContextSwitches.ResetAllSwitches();
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<AadSigningKeyTheoryData> ValidateSigningKeyCloudInstanceNameTestCases
        {
            get
            {
                var theoryData = new TheoryData<AadSigningKeyTheoryData>();
                theoryData.Add(new AadSigningKeyTheoryData("NullSecurityKey")
                {
                    SecurityKey = null,
                    OpenIdConnectConfiguration = GetConfigurationMock(configurationCloudInstanceName: Default.CloudInstanceName, siginingKeyCloudInstanceName: Default.CloudInstanceName),
                });

                theoryData.Add(new AadSigningKeyTheoryData("NullConfiguration")
                {
                    SecurityKey = KeyingMaterial.JsonWebKeyP384,
                    OpenIdConnectConfiguration = null,
                });

                theoryData.Add(new AadSigningKeyTheoryData("NoMatchingKeysInConfiguration")
                {
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    OpenIdConnectConfiguration = GetConfigurationMock(configurationCloudInstanceName: Default.CloudInstanceName, siginingKeyCloudInstanceName: "different.com")
                });

                theoryData.Add(new AadSigningKeyTheoryData("MissingConfigurationCloudInstanceName")
                {
                    SecurityKey = KeyingMaterial.JsonWebKeyP384,
                    OpenIdConnectConfiguration = GetConfigurationMock(configurationCloudInstanceName: null, siginingKeyCloudInstanceName: Default.CloudInstanceName),
                });

                theoryData.Add(new AadSigningKeyTheoryData("MissingSigningKeyCloudInstanceName")
                {
                    SecurityKey = KeyingMaterial.JsonWebKeyP384,
                    OpenIdConnectConfiguration = GetConfigurationMock(configurationCloudInstanceName: Default.CloudInstanceName, siginingKeyCloudInstanceName: null),
                });

                theoryData.Add(new AadSigningKeyTheoryData("CloudInstanceNameMatched")
                {
                    SecurityKey = KeyingMaterial.JsonWebKeyP384,
                    OpenIdConnectConfiguration = GetConfigurationMock(configurationCloudInstanceName: Default.CloudInstanceName, siginingKeyCloudInstanceName: Default.CloudInstanceName),
                });

                theoryData.Add(new AadSigningKeyTheoryData("CloudInstanceNameNotMatched")
                {
                    SecurityKey = KeyingMaterial.JsonWebKeyP384,
                    ExpectedException = ExpectedException.SecurityTokenInvalidCloudInstanceException("IDX40012"),
                    OpenIdConnectConfiguration = GetConfigurationMock(configurationCloudInstanceName: "microsoftonline.com", siginingKeyCloudInstanceName: "microsoftonline.us"),
                });

                OpenIdConnectConfiguration GetConfigurationMock(string configurationCloudInstanceName = null, string siginingKeyCloudInstanceName = null)
                {
                    var config = new OpenIdConnectConfiguration();
                    if (configurationCloudInstanceName != null)
                    {
                        config.AdditionalData.Add("cloud_instance_name", configurationCloudInstanceName);
                    }
                    config.JsonWebKeySet = new JsonWebKeySet();
                    var jwt = KeyingMaterial.JsonWebKeyP384;
                    if (!string.IsNullOrEmpty(siginingKeyCloudInstanceName))
                    {
                        jwt.AdditionalData.Add("cloud_instance_name", siginingKeyCloudInstanceName);
                    }

                    config.JsonWebKeySet.Keys.Add(jwt);
                    return config;
                }

                return theoryData;
            }
        }

        [Theory, MemberData(nameof(ValidateIssuerSigningKeyCertificateTestCases), DisableDiscoveryEnumeration = true)]
        public void ValidateIssuerSigningKeyCertificateTests(AadSigningKeyTheoryData theoryData)
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

        public static TheoryData<AadSigningKeyTheoryData> ValidateIssuerSigningKeyCertificateTestCases()
        {
            var theoryData = new TheoryData<AadSigningKeyTheoryData>
            {
                new AadSigningKeyTheoryData("SecurityKeyIsNull")
                {
                    SecurityKey = null,
                    TokenValidationParameters = new TokenValidationParameters() { RequireSignedTokens = true, ValidateIssuerSigningKey = true },
                    ExpectedException = ExpectedException.ArgumentNullException("IDX40007:")
                },
                new AadSigningKeyTheoryData("SecurityKeyIsNull_RequireSignedTokensFalse")
                {
                    SecurityKey = null,
                    TokenValidationParameters = new TokenValidationParameters() { RequireSignedTokens = false, ValidateIssuerSigningKey = true },
                },
                new AadSigningKeyTheoryData("ServiceAcceptsUnsignedTokens")
                {
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    TokenValidationParameters = new TokenValidationParameters() { RequireSignedTokens = false, ValidateIssuerSigningKey = true },
                },
                new AadSigningKeyTheoryData("SkipValidaingIssuerSigningKey")
                {
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    TokenValidationParameters = new TokenValidationParameters() { RequireSignedTokens = true, ValidateIssuerSigningKey = false },
                },
                new AadSigningKeyTheoryData("SkipValidaingIssuerSigningKey")
                {
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    TokenValidationParameters = new TokenValidationParameters() { RequireSignedTokens = false, ValidateIssuerSigningKey = false },
                },
                new AadSigningKeyTheoryData("CertificateLifeTimeValidated")
                {
                    SecurityKey = KeyingMaterial.X509SecurityKeySelfSigned1024_SHA256,
                    TokenValidationParameters = new TokenValidationParameters() { RequireSignedTokens = true, ValidateIssuerSigningKey = true },
                }
            };

            return theoryData;
        }

        [Theory, MemberData(nameof(ValidateIssuerSigningKeyTestCases), DisableDiscoveryEnumeration = true)]
        public void ValidateIssuerSigningKeyTests(AadSigningKeyTheoryData theoryData)
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
                AppContextSwitches.ResetAllSwitches();
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<AadSigningKeyTheoryData> ValidateIssuerSigningKeyTestCases
        {
            get
            {
                var theoryData = new TheoryData<AadSigningKeyTheoryData>();

                var tidClaim = new Claim(ValidatorConstants.ClaimNameTid, ValidatorConstants.TenantIdAsGuid);
                var issClaim = new Claim(ValidatorConstants.ClaimNameIss, ValidatorConstants.AadIssuer);
                var jwtSecurityToken = new JwtSecurityToken(issuer: ValidatorConstants.AadIssuer, claims: new[] { issClaim, tidClaim });
                theoryData.Add(new AadSigningKeyTheoryData("NullSecurityKey")
                {
                    SecurityKey = null,
                    SecurityToken = jwtSecurityToken,
                    OpenIdConnectConfiguration = GetConfigurationMock()
                });

                theoryData.Add(new AadSigningKeyTheoryData("NullSecurityToken")
                {
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    SecurityToken = null,
                    OpenIdConnectConfiguration = GetConfigurationMock(),
                    ExpectedException = ExpectedException.ArgumentNullException("IDX10000")
                });

                theoryData.Add(new AadSigningKeyTheoryData("NullConfiguration")
                {
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    SecurityToken = jwtSecurityToken,
                    OpenIdConnectConfiguration = null,
                });

                theoryData.Add(new AadSigningKeyTheoryData("NoSigningKeysInConfiguration")
                {
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    SecurityToken = jwtSecurityToken,
                    OpenIdConnectConfiguration = new OpenIdConnectConfiguration()
                });

                var mockConfiguration = GetConfigurationMock();
                mockConfiguration.JsonWebKeySet.Keys.Add(KeyingMaterial.JsonWebKeyP384);
                theoryData.Add(new AadSigningKeyTheoryData("NoMatchingKeysInConfiguration")
                {
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    SecurityToken = jwtSecurityToken,
                    OpenIdConnectConfiguration = new OpenIdConnectConfiguration()
                });

                theoryData.Add(new AadSigningKeyTheoryData("MissingIssuerInConfiguration")
                {
                    SecurityKey = KeyingMaterial.JsonWebKeyP384,
                    SecurityToken = jwtSecurityToken,
                    OpenIdConnectConfiguration = mockConfiguration
                });

                var jwk = KeyingMaterial.JsonWebKeySymmetric128;
                jwk.AdditionalData.Add(OpenIdProviderMetadataNames.Issuer, " ");
                mockConfiguration.JsonWebKeySet.Keys.Add(jwk);
                theoryData.Add(new AadSigningKeyTheoryData("WhitespaceForIssuerInConfiguration")
                {
                    SecurityKey = KeyingMaterial.JsonWebKeySymmetric128,
                    SecurityToken = jwtSecurityToken,
                    OpenIdConnectConfiguration = mockConfiguration,
                });

                jwk = KeyingMaterial.JsonWebKeyP521;
                jwk.AdditionalData.Add(OpenIdProviderMetadataNames.Issuer, ValidatorConstants.UsGovIssuer);
                mockConfiguration.JsonWebKeySet.Keys.Add(jwk);
                theoryData.Add(new AadSigningKeyTheoryData("JST_TokenIssuer_MismatchesWith_SigningKeyIssuer")
                {
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
                theoryData.Add(new AadSigningKeyTheoryData("JWT_TokenIssuer_MismatchesWith_SigningKeyIssuer")
                {
                    SecurityKey = KeyingMaterial.JsonWebKeyP521,
                    SecurityToken = jsonWebToken,
                    OpenIdConnectConfiguration = mockConfiguration,
                    ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException("IDX40005")
                });

                jwk = KeyingMaterial.JsonWebKeyP256;
                jwk.AdditionalData.Add(OpenIdProviderMetadataNames.Issuer, ValidatorConstants.AadIssuerV2CommonAuthority);
                mockConfiguration.JsonWebKeySet.Keys.Add(jwk);
                mockConfiguration.Issuer = ValidatorConstants.AadIssuerV2CommonAuthority;
                theoryData.Add(new AadSigningKeyTheoryData("HappyPath_TokenIssuer_Matches_SigningKeyIssuer")
                {
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    SecurityToken = jwtSecurityToken,
                    OpenIdConnectConfiguration = mockConfiguration
                });

                theoryData.Add(new AadSigningKeyTheoryData("MissingTenantIdClaimInToken")
                {
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    SecurityToken = new JwtSecurityToken(),
                    OpenIdConnectConfiguration = mockConfiguration,
                    ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException("IDX40009")
                });

                theoryData.Add(new AadSigningKeyTheoryData("WrongSecurityKeyType")
                {
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    SecurityToken = new Saml2SecurityToken(new Saml2Assertion(new Saml2NameIdentifier("nameIdentifier"))),
                    OpenIdConnectConfiguration = mockConfiguration,
                    ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException("IDX40010")
                });

                theoryData.Add(new AadSigningKeyTheoryData("JST_TokenIssuer_MismatchesWith_TenantIdInToken")
                {
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
                theoryData.Add(new AadSigningKeyTheoryData("JWT_TokenIssuer_MismatchesWith_TenantIdInToken")
                {
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    SecurityToken = jsonWebToken,
                    OpenIdConnectConfiguration = mockConfiguration,
                    ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException("IDX40004")
                });

                theoryData.Add(new AadSigningKeyTheoryData("Doesnt_Fail_With_Switch")
                {
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    SecurityToken = new JwtSecurityToken(),
                    OpenIdConnectConfiguration = mockConfiguration,
                    SetupAction = () => AppContext.SetSwitch(AppContextSwitches.DoNotFailOnMissingTidSwitch, true),
                });

                theoryData.Add(new AadSigningKeyTheoryData("Fail_With_Switch_False")
                {
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    SecurityToken = new JwtSecurityToken(),
                    OpenIdConnectConfiguration = mockConfiguration,
                    ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException("IDX40009"),
                    SetupAction = () => AppContext.SetSwitch(AppContextSwitches.DoNotFailOnMissingTidSwitch, false),
                });

                theoryData.Add(new AadSigningKeyTheoryData("Doesnt_Fail_With_Switch")
                {
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    SecurityToken = new JwtSecurityToken(),
                    OpenIdConnectConfiguration = mockConfiguration,
                    SetupAction = () => AppContext.SetSwitch(AppContextSwitches.DoNotFailOnMissingTidSwitch, true),
                });

                theoryData.Add(new AadSigningKeyTheoryData("Fail_With_Switch_False_JsonWebToken")
                {
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    SecurityToken = new JsonWebToken(Default.Jwt(Default.SecurityTokenDescriptor(Default.SymmetricSigningCredentials, [issClaim]))),
                    OpenIdConnectConfiguration = mockConfiguration,
                    ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException("IDX40009"),
                    SetupAction = () => AppContext.SetSwitch(AppContextSwitches.DoNotFailOnMissingTidSwitch, false),
                });

                theoryData.Add(new AadSigningKeyTheoryData("Doesnt_Fail_With_Switch_JsonWebToken")
                {
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    SecurityToken = new JsonWebToken(Default.Jwt(Default.SecurityTokenDescriptor(Default.SymmetricSigningCredentials, [issClaim]))),
                    OpenIdConnectConfiguration = mockConfiguration,
                    SetupAction = () => AppContext.SetSwitch(AppContextSwitches.DoNotFailOnMissingTidSwitch, true),
                });

                theoryData.Add(new AadSigningKeyTheoryData("Fails_With_Multiple_tids")
                {
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    SecurityToken = new JsonWebToken(
                        Default.Jwt(Default.SecurityTokenDescriptor(
                            Default.SymmetricSigningCredentials,
                            [tidClaim, issClaim, new Claim("TID", Guid.NewGuid().ToString())]))),
                    ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException("IDX40011"),
                    OpenIdConnectConfiguration = mockConfiguration
                });

                theoryData.Add(new AadSigningKeyTheoryData("Fails_With_Multiple_tids_alternate_order")
                {
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    SecurityToken = new JsonWebToken(
                        Default.Jwt(Default.SecurityTokenDescriptor(
                            Default.SymmetricSigningCredentials,
                            [issClaim, new Claim("TID", Guid.NewGuid().ToString()), tidClaim]))),
                    ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException("IDX40011"),
                    OpenIdConnectConfiguration = mockConfiguration
                });

                theoryData.Add(new AadSigningKeyTheoryData("Fails_With_no_standard_tid")
                {
                    SecurityKey = KeyingMaterial.JsonWebKeyP256,
                    SecurityToken = new JsonWebToken(
                        Default.Jwt(Default.SecurityTokenDescriptor(
                            Default.SymmetricSigningCredentials,
                            [issClaim, new Claim("TID", Guid.NewGuid().ToString())]))),
                    ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException("IDX40009"),
                    OpenIdConnectConfiguration = mockConfiguration
                });

                OpenIdConnectConfiguration GetConfigurationMock()
                {
                    var config = new OpenIdConnectConfiguration();
                    config.JsonWebKeySet = new JsonWebKeySet();
                    config.JsonWebKeySet.Keys.Add(KeyingMaterial.JsonWebKeyP384);
                    return config;
                }

                return theoryData;
            }
        }

        private static TokenValidationParameters SetupTokenValidationParametersMock(OpenIdConnectConfiguration openIdConnectConfiguration)
        {
            return new TokenValidationParameters()
            {
                ConfigurationManager = new StaticConfigurationManager<OpenIdConnectConfiguration>(openIdConnectConfiguration),
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                ValidateLifetime = false
            };
        }

        public class EnableEntraIdSigningKeyValidationTheoryData : TheoryDataBase
        {
            public EnableEntraIdSigningKeyValidationTheoryData(string testId) : base(testId) { }

            public TokenValidationParameters TokenValidationParameters { get; set; }

            public string Token { get; set; }

            public bool SetDelegateUsingConfig { get; set; }

            public bool SetDelegateWithoutConfig { get; set; }

            public bool ExpectedValidationResult { get; set; }
        }

        public class AadSigningKeyTheoryData : TheoryDataBase
        {
            public AadSigningKeyTheoryData(string testId) : base(testId) { }

            public SecurityKey SecurityKey { get; set; }

            public SecurityToken SecurityToken { get; set; }

            public TokenValidationParameters TokenValidationParameters { get; set; }

            public OpenIdConnectConfiguration OpenIdConnectConfiguration { get; set; }

            public Action SetupAction { get; set; }
        }
    }
}
