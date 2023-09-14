// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect.Tests
{
    /// <summary>
    /// Tests for OpenIdConnectProtocolValidator
    /// </summary>
    public class OpenIdConnectProtocolValidatorTests
    {
        private static JwtSecurityToken CreateValidatedIdToken()
        {
            return CreateValidatedIdToken(null, null);
        }

        private static JwtSecurityToken CreateValidatedIdToken(string claimType, object claimValue)
        {
            return CreateValidatedIdToken(claimType, claimValue, SecurityAlgorithms.RsaSha256);
        }

        private static JwtSecurityToken CreateValidatedIdToken(string claimType, object claimValue, string alg)
        {
            var payload = new JwtPayload
            {
                [JwtRegisteredClaimNames.Aud] = Default.Audience,
                [JwtRegisteredClaimNames.Exp] = EpochTime.GetIntDate(DateTime.UtcNow),
                [JwtRegisteredClaimNames.Iat] = EpochTime.GetIntDate(DateTime.UtcNow),
                [JwtRegisteredClaimNames.Iss] = Default.Issuer,
                [JwtRegisteredClaimNames.Nonce] = Default.Nonce,
                [JwtRegisteredClaimNames.Sub] = Default.Subject
            };

            if (!string.IsNullOrEmpty(claimType))
                payload[claimType] = claimValue;

            var header = new JwtHeader();
            if (alg != null)
                header[JwtHeaderParameterNames.Alg] = alg;

            return new JwtSecurityToken(header, payload);
        }

        private static JwtSecurityToken CreateJWEValidatedIdToken(string claimType, object claimValue, string alg)
        {
            var innerToken = CreateValidatedIdToken(claimType, claimValue, alg);

            var header = new JwtHeader(Default.SymmetricEncryptingCredentials);
            var token = new JwtSecurityToken(
                       header,
                       innerToken,
                       "ey",
                       "ey",
                       "ey",
                       "ey",
                       "ey");
            return token;
        }

        [Fact]
        public void GenerateNonce()
        {
            List<string> errors = new List<string>();
            OpenIdConnectProtocolValidator protocolValidator = new OpenIdConnectProtocolValidator();
            string nonce = protocolValidator.GenerateNonce();
            int endOfTimestamp = nonce.IndexOf('.');
            if (endOfTimestamp == -1)
            {
                errors.Add("nonce does not have '.' seperator");
            }

            TestUtilities.AssertFailIfErrors("GenerateNonce", errors);
        }

        [Fact]
        public void GetSets()
        {
            OpenIdConnectProtocolValidator validationParameters = new OpenIdConnectProtocolValidator();
            Type type = typeof(OpenIdConnectProtocolValidator);
            PropertyInfo[] properties = type.GetProperties();
            if (properties.Length != 10)
                Assert.True(true, "Number of properties has changed from 10 to: " + properties.Length + ", adjust tests");

            GetSetContext context =
                new GetSetContext
                {
                    PropertyNamesAndSetGetValue = new List<KeyValuePair<string, List<object>>>
                    {
                        new KeyValuePair<string, List<object>>("NonceLifetime", new List<object>{TimeSpan.FromMinutes(60), TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(100)}),
                        new KeyValuePair<string, List<object>>("RequireAcr", new List<object>{false, true, false}),
                        new KeyValuePair<string, List<object>>("RequireAmr", new List<object>{false, true, false}),
                        new KeyValuePair<string, List<object>>("RequireAuthTime", new List<object>{false, true, false}),
                        new KeyValuePair<string, List<object>>("RequireAzp", new List<object>{false, true, false}),
                        new KeyValuePair<string, List<object>>("RequireNonce", new List<object>{true, false, true}),
                        new KeyValuePair<string, List<object>>("RequireSub", new List<object>{true, false, true}),
                        new KeyValuePair<string, List<object>>("RequireTimeStampInNonce", new List<object>{true, false, true}),
                        new KeyValuePair<string, List<object>>("RequireState", new List<object>{true, false, true}),
                        new KeyValuePair<string, List<object>>("RequireStateValidation", new List<object>{true, false, true}),
                    },
                    Object = validationParameters,
                };

            TestUtilities.GetSet(context);
            TestUtilities.AssertFailIfErrors("OpenIdConnectProtocolValidator_GetSets", context.Errors);

            ExpectedException ee = ExpectedException.ArgumentNullException();
            Assert.NotNull(validationParameters.HashAlgorithmMap);
            Assert.Equal(18, validationParameters.HashAlgorithmMap.Count);

            ee = ExpectedException.ArgumentOutOfRangeException();
            try
            {
                validationParameters.NonceLifetime = TimeSpan.Zero;
                ee.ProcessNoException();
            }
            catch (Exception ex)
            {
                ee.ProcessException(ex);
            }

            ee = ExpectedException.ArgumentNullException();
            try
            {
                validationParameters.CryptoProviderFactory = null;
                ee.ProcessNoException();
            }
            catch (Exception ex)
            {
                ee.ProcessException(ex);
            }
        }


        [Theory, MemberData(nameof(ValidateAuthenticationResponseTheoryData))]
        public void ValidateAuthenticationResponse(OidcProtocolValidatorTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ValidateAuthenticationResponse", theoryData);
            try
            {
                theoryData.ProtocolValidator.ValidateAuthenticationResponse(theoryData.ValidationContext);
                theoryData.ExpectedException.ProcessNoException();
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex);
            }
        }

        public static TheoryData<OidcProtocolValidatorTheoryData> ValidateAuthenticationResponseTheoryData
        {
            get
            {
                var theoryData = new TheoryData<OidcProtocolValidatorTheoryData>
                {
                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        First = true,
                        ProtocolValidator = new PublicOpenIdConnectProtocolValidator(),
                        TestId = "validationContext == null"
                    },

                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX21333:"),
                        ProtocolValidator = new PublicOpenIdConnectProtocolValidator(),
                        TestId = "validationContext.ProtocolMessage == null",
                        ValidationContext = new OpenIdConnectProtocolValidationContext()
                    },

                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX21334:"),
                        ProtocolValidator = new PublicOpenIdConnectProtocolValidator(),
                        TestId = "'id_token' == null, 'code' == null",
                        ValidationContext = new OpenIdConnectProtocolValidationContext { ProtocolMessage = new OpenIdConnectMessage() }
                    },

                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX21334:"),
                        ProtocolValidator = new PublicOpenIdConnectProtocolValidator(),
                        TestId = "'id_token' == string.Empty, 'code' == string.Empty",
                        ValidationContext = new OpenIdConnectProtocolValidationContext
                        {
                            ProtocolMessage = new OpenIdConnectMessage
                            {
                                Code = string.Empty,
                                IdToken = string.Empty,
                            }
                        }
                    },

                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX21332:"),
                        ProtocolValidator = new PublicOpenIdConnectProtocolValidator(),
                        TestId = "'id_token' != null, validationContext.validatedIdToken == null",
                        ValidationContext = new OpenIdConnectProtocolValidationContext
                        {
                            ProtocolMessage = new OpenIdConnectMessage
                            {
                                IdToken = Guid.NewGuid().ToString()
                            }
                        }
                    },

                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX21335:"),
                        ProtocolValidator = new PublicOpenIdConnectProtocolValidator(),
                        TestId = "'refresh_token' should not be returned from AuthorizationEndpoint",
                        ValidationContext = new OpenIdConnectProtocolValidationContext
                        {
                            ProtocolMessage = new OpenIdConnectMessage
                            {
                                IdToken = Guid.NewGuid().ToString(),
                                RefreshToken = Guid.NewGuid().ToString()
                            },
                            ValidatedIdToken = new JwtSecurityToken()
                        }
                    },

                    new OidcProtocolValidatorTheoryData
                    {
                        ProtocolValidator = new PublicOpenIdConnectProtocolValidator
                        {
                            RequireState = false
                        },
                        TestId = "'id_token' == string.Empty, 'code' != null, RequireState == false",
                        ValidationContext = new OpenIdConnectProtocolValidationContext
                        {
                            ProtocolMessage = new OpenIdConnectMessage
                            {
                                Code = Guid.NewGuid().ToString(),
                                IdToken = string.Empty,
                            }
                        }
                    },

                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX21334:"),
                        ProtocolValidator = new PublicOpenIdConnectProtocolValidator
                        {
                            RequireState = false
                        },
                        TestId = "'id_token' == null, 'code' == null, 'access_token' != null, RequireState == false",
                        ValidationContext = new OpenIdConnectProtocolValidationContext
                        {
                            ProtocolMessage = new OpenIdConnectMessage
                            {
                                AccessToken = Guid.NewGuid().ToString(),
                                Code = string.Empty,
                                IdToken = string.Empty,
                            }
                        }
                    }
                };

                return theoryData;
            }
        }

        [Theory, MemberData(nameof(ValidateTokenResponseTheoryData))]
        public void ValidateTokenResponse(OidcProtocolValidatorTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ValidateTokenResponse", theoryData);
            try
            {
                theoryData.ProtocolValidator.ValidateTokenResponse(theoryData.ValidationContext);
                theoryData.ExpectedException.ProcessNoException();
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex);
            }
        }

        public static TheoryData<OidcProtocolValidatorTheoryData> ValidateTokenResponseTheoryData
        {
            get
            {
                var theoryData = new TheoryData<OidcProtocolValidatorTheoryData>
                {
                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        First = true,
                        ProtocolValidator = new PublicOpenIdConnectProtocolValidator(),
                        TestId = "validationContext == null"
                    },

                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX21333:"),
                        ProtocolValidator = new PublicOpenIdConnectProtocolValidator(),
                        TestId = "validationContext.ProtocolMessage == null",
                        ValidationContext = new OpenIdConnectProtocolValidationContext()
                    },

                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX21336:"),
                        ProtocolValidator = new PublicOpenIdConnectProtocolValidator(),
                        TestId = "validationContext.ProtocolMessage.IdToken == null",
                        ValidationContext = new OpenIdConnectProtocolValidationContext()
                        {
                            ProtocolMessage = new OpenIdConnectMessage { AccessToken = Guid.NewGuid().ToString() }
                        }
                    },

                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX21336:"),
                        ProtocolValidator = new PublicOpenIdConnectProtocolValidator(),
                        TestId = "validationContext.ProtocolMessage.AccessToken == null",
                        ValidationContext = new OpenIdConnectProtocolValidationContext()
                        {
                            ProtocolMessage = new OpenIdConnectMessage { IdToken = Guid.NewGuid().ToString() }
                        }
                    },

                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX21332:"),
                        ProtocolValidator = new PublicOpenIdConnectProtocolValidator(),
                        TestId = "validationContext.ValidatedIdToken == null",
                        ValidationContext = new OpenIdConnectProtocolValidationContext()
                        {
                            ProtocolMessage = new OpenIdConnectMessage
                            {
                                AccessToken = Guid.NewGuid().ToString(),
                                IdToken = Guid.NewGuid().ToString()
                            }
                        }
                    },

                    new OidcProtocolValidatorTheoryData
                    {
                        ProtocolValidator = new PublicOpenIdConnectProtocolValidator
                        {
                            RequireNonce = false,
                            RequireTimeStampInNonce = false
                        },
                        TestId = "validationContext.ValidatedIdToken.AtHash == null (Optional)",
                        ValidationContext = new OpenIdConnectProtocolValidationContext()
                        {
                            Nonce = Default.Nonce,
                            ProtocolMessage = new OpenIdConnectMessage
                            {
                                AccessToken = Guid.NewGuid().ToString(),
                                IdToken = Guid.NewGuid().ToString()
                            },
                            ValidatedIdToken = CreateValidatedIdToken()
                        }
                    }
                };

                return theoryData;
            }
        }

        [Theory, MemberData(nameof(ValidateUserInfoResponseTheoryData))]
        public void ValidateUserInfoResponse(OidcProtocolValidatorTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ValidateUserInfoResponse", theoryData);
            try
            {
                theoryData.ProtocolValidator.ValidateUserInfoResponse(theoryData.ValidationContext);
                theoryData.ExpectedException.ProcessNoException();
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex);
            }
        }

        public static TheoryData<OidcProtocolValidatorTheoryData> ValidateUserInfoResponseTheoryData
        {
            get
            {
                var theoryData = new TheoryData<OidcProtocolValidatorTheoryData>
                {
                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        First = true,
                        ProtocolValidator = new PublicOpenIdConnectProtocolValidator(),
                        TestId = "validationContext == null"
                    },

                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX21337:"),
                        ProtocolValidator = new PublicOpenIdConnectProtocolValidator(),
                        TestId = "validationContext.UserInfoEndpointResponse == null",
                        ValidationContext = new OpenIdConnectProtocolValidationContext()
                    },

                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX21332:"),
                        ProtocolValidator = new PublicOpenIdConnectProtocolValidator(),
                        TestId = "validationContext.validatedIdToken == null",
                        ValidationContext = new OpenIdConnectProtocolValidationContext { UserInfoEndpointResponse = "response" }
                    },

                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX21343:", typeof(System.Text.Json.JsonException), true),
                        ProtocolValidator = new PublicOpenIdConnectProtocolValidator(),
                        TestId = "UserInfoEndpointResponse is not valid JSON",
                        ValidationContext = new OpenIdConnectProtocolValidationContext
                        {
                            UserInfoEndpointResponse = "response",
                            ValidatedIdToken = CreateValidatedIdToken(),
                        }
                    },

                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX21345:"),
                        ProtocolValidator = new PublicOpenIdConnectProtocolValidator(),
                        TestId = "UserInfoEndpointResponse.sub == null",
                        ValidationContext = new OpenIdConnectProtocolValidationContext
                        {
                            UserInfoEndpointResponse = @"{ ""tid"":""42"",""name"":""bob""}",
                            ValidatedIdToken = CreateValidatedIdToken(),
                        }
                    }
                };

                var jwtWithoutSub = CreateValidatedIdToken();
                jwtWithoutSub.Payload.Remove(JwtRegisteredClaimNames.Sub);
                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX21346:"),
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator(),
                    TestId = "ValidatedIdToken.sub == null",
                    ValidationContext = new OpenIdConnectProtocolValidationContext
                    {
                        UserInfoEndpointResponse = @"{ ""sub"": ""sub1""}",
                        ValidatedIdToken = jwtWithoutSub
                    }
                });

                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX21338:"),
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator(),
                    TestId = "UserInfoEndpointResponse.sub != ValidatedIdToken.sub",
                    ValidationContext = new OpenIdConnectProtocolValidationContext
                    {
                        UserInfoEndpointResponse = @"{ ""sub"": ""sub1""}",
                        ValidatedIdToken = CreateValidatedIdToken()
                    }
                });

                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator(),
                    TestId = "(JSON) UserInfoResponse.sub == ValidatedIdToken.sub",
                    ValidationContext = new OpenIdConnectProtocolValidationContext
                    {
                        UserInfoEndpointResponse = @"{ ""sub"": ""sub""}",
                        ValidatedIdToken = CreateValidatedIdToken("sub", "sub")
                    }
                });

                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator(),
                    TestId = "(JWT) UserInfoResponse.sub == ValidatedIdToken.sub",
                    ValidationContext = new OpenIdConnectProtocolValidationContext
                    {
                        UserInfoEndpointResponse = (new JwtSecurityTokenHandler()).WriteToken(CreateValidatedIdToken("sub", "sub")),
                        ValidatedIdToken = CreateValidatedIdToken("sub", "sub")
                    }
                });

                return theoryData;
            }
        }

        [Theory, MemberData(nameof(ValidateIdTokenTheoryData))]
        public void ValidateIdToken(OidcProtocolValidatorTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ValidateIdToken", theoryData);

            // should put this in ValidationContext
            theoryData.ValidationContext.ValidatedIdToken = theoryData.JwtSecurityToken;

            try
            {
                theoryData.ProtocolValidator.PublicValidateIdToken(theoryData.ValidationContext);
                theoryData.ExpectedException.ProcessNoException();
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex);
            }

            return;
        }

        public static TheoryData<OidcProtocolValidatorTheoryData> ValidateIdTokenTheoryData
        {
            get
            {
                var theoryData = new TheoryData<OidcProtocolValidatorTheoryData>();

                var validationContext = new OpenIdConnectProtocolValidationContext
                {
                    ProtocolMessage = new OpenIdConnectMessage()
                };

                var jwt = new JwtSecurityToken();
                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX21314:"),
                    First = true,
                    JwtSecurityToken = new JwtSecurityToken(),
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator(),
                    TestId = "aud == null",
                    ValidationContext = validationContext,
                });

                jwt = new JwtSecurityToken();
                jwt.Payload[JwtRegisteredClaimNames.Aud] = Default.Audience;
                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX21314:"),
                    JwtSecurityToken = jwt,
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator(),
                    TestId = "exp == null",
                    ValidationContext = validationContext
                });

                jwt = new JwtSecurityToken();
                jwt.Payload[JwtRegisteredClaimNames.Aud] = Default.Audience;
                jwt.Payload[JwtRegisteredClaimNames.Exp] = EpochTime.GetIntDate(DateTime.UtcNow).ToString();
                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX21314:"),
                    JwtSecurityToken = jwt,
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator(),
                    TestId = "iat == null",
                    ValidationContext = validationContext
                });

                jwt = new JwtSecurityToken();
                jwt.Payload[JwtRegisteredClaimNames.Aud] = Default.Audience;
                jwt.Payload[JwtRegisteredClaimNames.Exp] = EpochTime.GetIntDate(DateTime.UtcNow).ToString();
                jwt.Payload[JwtRegisteredClaimNames.Iat] = EpochTime.GetIntDate(DateTime.UtcNow).ToString();
                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX21314:"),
                    JwtSecurityToken = jwt,
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator(),
                    TestId = "iss == null",
                    ValidationContext = validationContext,
                });

                jwt = new JwtSecurityToken();
                jwt.Payload[JwtRegisteredClaimNames.Aud] = Default.Audience;
                jwt.Payload[JwtRegisteredClaimNames.Exp] = EpochTime.GetIntDate(DateTime.UtcNow).ToString();
                jwt.Payload[JwtRegisteredClaimNames.Iat] = EpochTime.GetIntDate(DateTime.UtcNow).ToString();
                jwt.Payload[JwtRegisteredClaimNames.Iss] = Default.Issuer;
                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    JwtSecurityToken = jwt,
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator
                    {
                        RequireSub = false
                    },
                    TestId = "sub == null, RequireSub == false",
                    ValidationContext = validationContext
                });

                jwt = new JwtSecurityToken();
                jwt.Payload[JwtRegisteredClaimNames.Aud] = Default.Audience;
                jwt.Payload[JwtRegisteredClaimNames.Exp] = EpochTime.GetIntDate(DateTime.UtcNow).ToString();
                jwt.Payload[JwtRegisteredClaimNames.Iat] = EpochTime.GetIntDate(DateTime.UtcNow).ToString();
                jwt.Payload[JwtRegisteredClaimNames.Iss] = Default.Issuer;
                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX21314:"),
                    JwtSecurityToken = jwt,
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator(),
                    TestId = "sub == null, RequireSub == true",
                    ValidationContext = validationContext
                });

                new PublicOpenIdConnectProtocolValidator
                {
                    RequireAcr = true,
                };

                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX21315:"),
                    JwtSecurityToken = CreateValidatedIdToken(),
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator { RequireAcr = true },
                    TestId = "'acr' == null, RequireAcr == true",
                    ValidationContext = validationContext
                });

                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX21316:"),
                    JwtSecurityToken = CreateValidatedIdToken(),
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator { RequireAmr = true },
                    TestId = "amr == null, RequireAmr == true",
                    ValidationContext = validationContext
                });

                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX21317:"),
                    JwtSecurityToken = CreateValidatedIdToken(),
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator { RequireAuthTime = true },
                    TestId = "auth_time == null, RequireAuthTime == true",
                    ValidationContext = validationContext
                });

                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    JwtSecurityToken = CreateValidatedIdToken(JwtRegisteredClaimNames.Aud, Default.Audiences),
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator(),
                    TestId = "multiple 'aud' no 'azp' warning only",
                    ValidationContext = validationContext
                });

                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX21318:"),
                    JwtSecurityToken = CreateValidatedIdToken(),
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator { RequireAzp = true },
                    TestId = "azp == null",
                    ValidationContext = validationContext
                });

                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX21308:"),
                    JwtSecurityToken = CreateValidatedIdToken(JwtRegisteredClaimNames.Azp, Default.Azp),
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator { RequireAzp = true },
                    TestId = "'azp' != null, validationContext.ClientId == null",
                    ValidationContext = validationContext
                });

                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX21340:"),
                    JwtSecurityToken = CreateValidatedIdToken(JwtRegisteredClaimNames.Azp, Default.Azp),
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator { RequireAzp = true },
                    TestId = "azp claim != validationContext.ClientId",
                    ValidationContext = new OpenIdConnectProtocolValidationContext { ClientId = Default.ClientId }
                });

                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    JwtSecurityToken = CreateValidatedIdToken(JwtRegisteredClaimNames.Azp, Default.Azp),
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator { RequireAzp = true },
                    TestId = "azp claim == validationContext.ClientId",
                    ValidationContext = new OpenIdConnectProtocolValidationContext { ClientId = Default.Azp },
                });

                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX21313:", typeof(InvalidOperationException)),
                    JwtSecurityToken = CreateValidatedIdToken(),
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator
                    {
                        IdTokenValidator = ((jwtToken, context) => { throw new InvalidOperationException("Validator"); })
                    },
                    TestId = "IdTokenValidator throws InvalidOperation",
                    ValidationContext = new OpenIdConnectProtocolValidationContext()
                });

                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    JwtSecurityToken = new JwtSecurityToken(),
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator
                    {
                        IdTokenValidator = ((jwtToken, context) => { return; })
                    },
                    TestId = "IdTokenValidator returns",
                    ValidationContext = new OpenIdConnectProtocolValidationContext()
                });

                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX21313:", typeof(InvalidOperationException)),
                    JwtSecurityToken = CreateValidatedIdToken(JwtRegisteredClaimNames.Acr, Default.Acr),
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator
                    {
                        IdTokenValidator = ((jwtToken, context) =>
                       {
                           var jwtSecurityToken = jwtToken;
                           if (jwtSecurityToken.Payload.Acr != "acr")
                               throw new InvalidOperationException();
                       })
                    },
                    TestId = "IdTokenValidator throws if no acr",
                    ValidationContext = new OpenIdConnectProtocolValidationContext()
                });

                return theoryData;
            }
        }

        [Theory, MemberData(nameof(ValidateCHashTheoryData))]
        private void ValidateCHash(OidcProtocolValidatorTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateCHash", theoryData);
            try
            {
                theoryData.ProtocolValidator.PublicValidateCHash(theoryData.ValidationContext);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);

            return;
        }

        public static TheoryData<OidcProtocolValidatorTheoryData> ValidateCHashTheoryData
        {
            get
            {
                string code = Guid.NewGuid().ToString();
                string chash256 = IdentityUtilities.CreateHashClaim(code, "SHA256");
                string chash384 = IdentityUtilities.CreateHashClaim(code, "SHA384");
                string chash512 = IdentityUtilities.CreateHashClaim(code, "SHA512");

                return new TheoryData<OidcProtocolValidatorTheoryData>
                {
                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        First = true,
                        TestId = "validationContext == null"
                    },
                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        TestId = "validationContext.ValidatedIdToken == null",
                        ValidationContext = new OpenIdConnectProtocolValidationContext
                        {
                            ProtocolMessage = new OpenIdConnectMessage()
                        }
                    },
                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX21333:"),
                        TestId = "validationContext.ProtocolMessage == null",
                        ValidationContext = new OpenIdConnectProtocolValidationContext
                        {
                            ValidatedIdToken = CreateValidatedIdToken()
                        }
                    },
                    new OidcProtocolValidatorTheoryData
                    {
                        TestId = "ProtocolMessage.Code == null",
                        ValidationContext = new OpenIdConnectProtocolValidationContext
                        {
                            ProtocolMessage = new OpenIdConnectMessage(),
                            ValidatedIdToken = CreateValidatedIdToken()
                        }
                    },
                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidCHashException), "IDX21307:"),
                        TestId = "ValidatedIdToken.chash == null",
                        ValidationContext = new OpenIdConnectProtocolValidationContext
                        {
                            ProtocolMessage = new OpenIdConnectMessage { Code = Guid.NewGuid().ToString() },
                            ValidatedIdToken = CreateValidatedIdToken()
                        }
                    },
                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidCHashException), "IDX21347:", typeof(OpenIdConnectProtocolException)),
                        TestId = "ValidatedIdToken.chash == string.Empty",
                        ValidationContext = new OpenIdConnectProtocolValidationContext
                        {
                            ProtocolMessage = new OpenIdConnectMessage { Code = Guid.NewGuid().ToString() },
                            ValidatedIdToken = CreateValidatedIdToken(JwtRegisteredClaimNames.CHash, string.Empty)
                        }
                    },
                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(ArgumentNullException), "IDX10000:"),
                        TestId = "ValidatedIdToken.Header.alg == 'None'",
                        ValidationContext = new OpenIdConnectProtocolValidationContext
                        {
                            ProtocolMessage = new OpenIdConnectMessage { Code = code },
                            ValidatedIdToken = CreateValidatedIdToken(JwtRegisteredClaimNames.CHash, chash256, SecurityAlgorithms.None)
                        }
                    },
                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(ArgumentNullException), "IDX10000:"),
                        TestId = "ValidatedIdToken.Header.alg == null",
                        ValidationContext = new OpenIdConnectProtocolValidationContext
                        {
                            ProtocolMessage = new OpenIdConnectMessage { Code = code },
                            ValidatedIdToken = CreateValidatedIdToken(JwtRegisteredClaimNames.CHash, chash256, null)
                        }
                    },
                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(ArgumentNullException), "IDX10000:"),
                        TestId = "ValidatedIdToken.Header.alg == string.empty",
                        ValidationContext = new OpenIdConnectProtocolValidationContext
                        {
                            ProtocolMessage = new OpenIdConnectMessage { Code = code },
                            ValidatedIdToken = CreateValidatedIdToken(JwtRegisteredClaimNames.CHash, chash256, "")
                        }
                    },
                    new OidcProtocolValidatorTheoryData
                    {
                        TestId = "alg==256, hash(code)==256",
                        ValidationContext = new OpenIdConnectProtocolValidationContext
                        {
                            ProtocolMessage = new OpenIdConnectMessage { Code = code },
                            ValidatedIdToken = CreateValidatedIdToken(JwtRegisteredClaimNames.CHash, chash256, SecurityAlgorithms.RsaSha256)
                        }
                    },
                    new OidcProtocolValidatorTheoryData
                    {
                        TestId = "alg==384, hash(code)==384",
                        ValidationContext = new OpenIdConnectProtocolValidationContext
                        {
                            ProtocolMessage = new OpenIdConnectMessage { Code = code },
                            ValidatedIdToken = CreateValidatedIdToken(JwtRegisteredClaimNames.CHash, chash384, SecurityAlgorithms.RsaSha384)
                        }
                    },
                    new OidcProtocolValidatorTheoryData
                    {
                        TestId = "alg==512, hash(code)==512",
                        ValidationContext = new OpenIdConnectProtocolValidationContext
                        {
                            ProtocolMessage = new OpenIdConnectMessage { Code = code },
                            ValidatedIdToken = CreateValidatedIdToken(JwtRegisteredClaimNames.CHash, chash512, SecurityAlgorithms.RsaSha512)
                        }
                    },
                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidCHashException), "IDX21347:", typeof(OpenIdConnectProtocolException)),
                        TestId = "ValidatedIdToken.chash != ProtocolMessage.Code",
                        ValidationContext = new OpenIdConnectProtocolValidationContext
                        {
                            ProtocolMessage = new OpenIdConnectMessage { Code = Guid.NewGuid().ToString() },
                            ValidatedIdToken = CreateValidatedIdToken(JwtRegisteredClaimNames.CHash, chash256, SecurityAlgorithms.RsaSha256)
                        }
                    },
                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidCHashException), "IDX21347:", typeof(OpenIdConnectProtocolException)),
                        TestId = "alg==256, hash(code)==384",
                        ValidationContext = new OpenIdConnectProtocolValidationContext
                        {
                            ProtocolMessage = new OpenIdConnectMessage { Code = code },
                            ValidatedIdToken = CreateValidatedIdToken(JwtRegisteredClaimNames.CHash, chash384, SecurityAlgorithms.RsaSha256)
                        }
                    },
                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidCHashException), "IDX21347:", typeof(OpenIdConnectProtocolException)),
                        TestId = "alg==256, hash(code)==384",
                        ValidationContext = new OpenIdConnectProtocolValidationContext
                        {
                            ProtocolMessage = new OpenIdConnectMessage { Code = code },
                            ValidatedIdToken = CreateValidatedIdToken(JwtRegisteredClaimNames.CHash, chash384, SecurityAlgorithms.RsaSha256)
                        }
                    },
                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidCHashException), "IDX21347:", typeof(OpenIdConnectProtocolException)),
                        TestId = "alg==256, hash(code)==384",
                        ValidationContext = new OpenIdConnectProtocolValidationContext
                        {
                            ProtocolMessage = new OpenIdConnectMessage { Code = code },
                            ValidatedIdToken = CreateValidatedIdToken(JwtRegisteredClaimNames.CHash, chash384, SecurityAlgorithms.RsaSha256)
                        }
                    },
                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidCHashException), "IDX21347:", typeof(OpenIdConnectProtocolException)),
                        TestId = "alg==256, hash(code)==512",
                        ValidationContext = new OpenIdConnectProtocolValidationContext
                        {
                            ProtocolMessage = new OpenIdConnectMessage { Code = code },
                            ValidatedIdToken = CreateValidatedIdToken(JwtRegisteredClaimNames.CHash, chash512, SecurityAlgorithms.RsaSha256)
                        }
                    },
                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidCHashException), "IDX21347:", typeof(OpenIdConnectProtocolException)),
                        TestId = "alg==384, hash(code)==512",
                        ValidationContext = new OpenIdConnectProtocolValidationContext
                        {
                            ProtocolMessage = new OpenIdConnectMessage { Code = code },
                            ValidatedIdToken = CreateValidatedIdToken(JwtRegisteredClaimNames.CHash, chash512, SecurityAlgorithms.RsaSha384)
                        }
                    },
                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidCHashException), "IDX21347:", typeof(OpenIdConnectProtocolException)),
                        TestId = "alg==384, hash(code)==256",
                        ValidationContext = new OpenIdConnectProtocolValidationContext
                        {
                            ProtocolMessage = new OpenIdConnectMessage { Code = code },
                            ValidatedIdToken = CreateValidatedIdToken(JwtRegisteredClaimNames.CHash, chash256, SecurityAlgorithms.RsaSha384)
                        }
                    },
                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidCHashException), "IDX21306:"),
                        TestId = "ValidatedIdToken.chash is not a string, but array",
                        ValidationContext = new OpenIdConnectProtocolValidationContext
                        {
                            ProtocolMessage = new OpenIdConnectMessage { Code = Guid.NewGuid().ToString() },
                            ValidatedIdToken = CreateValidatedIdToken(JwtRegisteredClaimNames.CHash, new List<string> { "chash1", "chash2" })
                        }
                    },
                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidCHashException), "IDX21306:"),
                        TestId = "Multiple chashes",
                        ValidationContext = new OpenIdConnectProtocolValidationContext
                        {
                            ProtocolMessage = new OpenIdConnectMessage { Code = code },
                            ValidatedIdToken = new JwtSecurityToken(claims: new List<Claim> { new Claim(JwtRegisteredClaimNames.CHash, chash256), new Claim(JwtRegisteredClaimNames.CHash, chash512) }, signingCredentials: Default.AsymmetricSigningCredentials)
                        }
                    }
                };
            }
        }

        [Theory, MemberData(nameof(ValidateJWEPayloadCHashTheoryData))]
        private void ValidateJWEPayloadCHash(OidcProtocolValidatorTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateJWEPayloadCHash", theoryData);
            try
            {
                theoryData.ProtocolValidator.PublicValidateCHash(theoryData.ValidationContext);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);

            return;
        }

        public static TheoryData<OidcProtocolValidatorTheoryData> ValidateJWEPayloadCHashTheoryData
        {
            get
            {
                string code = Guid.NewGuid().ToString();
                string chash256 = IdentityUtilities.CreateHashClaim(code, "SHA256");
                string chash384 = IdentityUtilities.CreateHashClaim(code, "SHA384");
                string chash512 = IdentityUtilities.CreateHashClaim(code, "SHA512");

                return new TheoryData<OidcProtocolValidatorTheoryData>
                {
                    new OidcProtocolValidatorTheoryData
                    {
                        TestId = "alg==256, hash(code)==256",
                        ValidationContext = new OpenIdConnectProtocolValidationContext
                        {
                            ProtocolMessage = new OpenIdConnectMessage { Code = code },
                            ValidatedIdToken = CreateJWEValidatedIdToken(JwtRegisteredClaimNames.CHash, chash256, SecurityAlgorithms.RsaSha256)
                        }
                    },
                    new OidcProtocolValidatorTheoryData
                    {
                        TestId = "alg==384, hash(code)==384",
                        ValidationContext = new OpenIdConnectProtocolValidationContext
                        {
                            ProtocolMessage = new OpenIdConnectMessage { Code = code },
                            ValidatedIdToken = CreateJWEValidatedIdToken(JwtRegisteredClaimNames.CHash, chash384, SecurityAlgorithms.RsaSha384)
                        }
                    },
                    new OidcProtocolValidatorTheoryData
                    {
                        TestId = "alg==512, hash(code)==512",
                        ValidationContext = new OpenIdConnectProtocolValidationContext
                        {
                            ProtocolMessage = new OpenIdConnectMessage { Code = code },
                            ValidatedIdToken = CreateJWEValidatedIdToken(JwtRegisteredClaimNames.CHash, chash512, SecurityAlgorithms.RsaSha512)
                        }
                    },
                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidCHashException), "IDX21347:", typeof(OpenIdConnectProtocolException)),
                        TestId = "ValidatedIdToken.chash != ProtocolMessage.Code",
                        ValidationContext = new OpenIdConnectProtocolValidationContext
                        {
                            ProtocolMessage = new OpenIdConnectMessage { Code = Guid.NewGuid().ToString() },
                            ValidatedIdToken = CreateJWEValidatedIdToken(JwtRegisteredClaimNames.CHash, chash256, SecurityAlgorithms.RsaSha256)
                        }
                    },
                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidCHashException), "IDX21347:", typeof(OpenIdConnectProtocolException)),
                        TestId = "alg==256, hash(code)==384",
                        ValidationContext = new OpenIdConnectProtocolValidationContext
                        {
                            ProtocolMessage = new OpenIdConnectMessage { Code = code },
                            ValidatedIdToken = CreateJWEValidatedIdToken(JwtRegisteredClaimNames.CHash, chash384, SecurityAlgorithms.RsaSha256)
                        }
                    },
                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidCHashException), "IDX21347:", typeof(OpenIdConnectProtocolException)),
                        TestId = "alg==256, hash(code)==384",
                        ValidationContext = new OpenIdConnectProtocolValidationContext
                        {
                            ProtocolMessage = new OpenIdConnectMessage { Code = code },
                            ValidatedIdToken = CreateJWEValidatedIdToken(JwtRegisteredClaimNames.CHash, chash384, SecurityAlgorithms.RsaSha256)
                        }
                    },
                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidCHashException), "IDX21347:", typeof(OpenIdConnectProtocolException)),
                        TestId = "alg==256, hash(code)==384",
                        ValidationContext = new OpenIdConnectProtocolValidationContext
                        {
                            ProtocolMessage = new OpenIdConnectMessage { Code = code },
                            ValidatedIdToken = CreateJWEValidatedIdToken(JwtRegisteredClaimNames.CHash, chash384, SecurityAlgorithms.RsaSha256)
                        }
                    },
                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidCHashException), "IDX21347:", typeof(OpenIdConnectProtocolException)),
                        TestId = "alg==256, hash(code)==512",
                        ValidationContext = new OpenIdConnectProtocolValidationContext
                        {
                            ProtocolMessage = new OpenIdConnectMessage { Code = code },
                            ValidatedIdToken = CreateJWEValidatedIdToken(JwtRegisteredClaimNames.CHash, chash512, SecurityAlgorithms.RsaSha256)
                        }
                    },
                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidCHashException), "IDX21347:", typeof(OpenIdConnectProtocolException)),
                        TestId = "alg==384, hash(code)==512",
                        ValidationContext = new OpenIdConnectProtocolValidationContext
                        {
                            ProtocolMessage = new OpenIdConnectMessage { Code = code },
                            ValidatedIdToken = CreateJWEValidatedIdToken(JwtRegisteredClaimNames.CHash, chash512, SecurityAlgorithms.RsaSha384)
                        }
                    },
                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidCHashException), "IDX21347:", typeof(OpenIdConnectProtocolException)),
                        TestId = "alg==384, hash(code)==256",
                        ValidationContext = new OpenIdConnectProtocolValidationContext
                        {
                            ProtocolMessage = new OpenIdConnectMessage { Code = code },
                            ValidatedIdToken = CreateJWEValidatedIdToken(JwtRegisteredClaimNames.CHash, chash256, SecurityAlgorithms.RsaSha384)
                        }
                    }
                };
            }
        }

        [Theory, MemberData(nameof(ValidateNonceTheoryData))]
        private void ValidateNonce(OidcProtocolValidatorTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ValidateNonce", theoryData);
            try
            {
                theoryData.ProtocolValidator.PublicValidateNonce(theoryData.ValidationContext);
                theoryData.ExpectedException.ProcessNoException();
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex);
            }
        }

        public static TheoryData<OidcProtocolValidatorTheoryData> ValidateNonceTheoryData
        {
            get
            {
                var theoryData = new TheoryData<OidcProtocolValidatorTheoryData>
                {
                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        First = true,
                        ProtocolValidator = new PublicOpenIdConnectProtocolValidator(),
                        TestId = "validationContext == null",
                    },

                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        ProtocolValidator = new PublicOpenIdConnectProtocolValidator(),
                        TestId = "validationContext.ValidatedToken == null",
                    }
                };

                var jwtWithoutNonce = CreateValidatedIdToken();
                jwtWithoutNonce.Payload.Remove(JwtRegisteredClaimNames.Nonce);
                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidNonceException), "IDX21320:"),
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator { RequireNonce = true },
                    TestId = "validationContext.Nonce == null, jwt.Nonce == null, RequireNonce == true",
                    ValidationContext = new OpenIdConnectProtocolValidationContext
                    {
                        ValidatedIdToken = jwtWithoutNonce
                    }
                });

                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidNonceException), "IDX21323:"),
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator { RequireNonce = true },
                    TestId = "validationContext.Nonce == null, jwt.Nonce != null, RequireNonce == true",
                    ValidationContext = new OpenIdConnectProtocolValidationContext
                    {
                        ValidatedIdToken = CreateValidatedIdToken()
                    }
                });

                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidNonceException), "IDX21349:"),
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator { RequireNonce = true },
                    TestId = "validationContext.Nonce != null, jwt.Nonce == null, RequireNonce == true",
                    ValidationContext = new OpenIdConnectProtocolValidationContext
                    {
                        Nonce = Default.Nonce,
                        ValidatedIdToken = jwtWithoutNonce
                    }
                });

                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator { RequireNonce = false },
                    TestId = "validationContext.Nonce == null, jwt.Nonce == null, RequireNonce == false",
                    ValidationContext = new OpenIdConnectProtocolValidationContext
                    {
                        ValidatedIdToken = jwtWithoutNonce
                    }
                });

                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidNonceException), "IDX21323:"),
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator { RequireNonce = false },
                    TestId = "validationContext.Nonce == null, jwt.Nonce != null, RequireNonce == false",
                    ValidationContext = new OpenIdConnectProtocolValidationContext
                    {
                        ValidatedIdToken = CreateValidatedIdToken()
                    }
                });

                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidNonceException), "IDX21349:"),
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator { RequireNonce = false },
                    TestId = "validationContext.Nonce != null, jwt.Nonce == null, RequireNonce == false",
                    ValidationContext = new OpenIdConnectProtocolValidationContext
                    {
                        Nonce = Default.Nonce,
                        ValidatedIdToken = jwtWithoutNonce
                    }
                });

                var protocolValidatorRequiresTimeStamp = new PublicOpenIdConnectProtocolValidator { RequireTimeStampInNonce = true };
                var nonce = protocolValidatorRequiresTimeStamp.GenerateNonce();
                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator { RequireNonce = false },
                    TestId = "nonce.timestamp == true, RequireTimeStampInNonce == true",
                    ValidationContext = new OpenIdConnectProtocolValidationContext
                    {
                        Nonce = nonce,
                        ValidatedIdToken = CreateValidatedIdToken(JwtRegisteredClaimNames.Nonce, nonce)
                    }
                });

                var protocolValidatorDoesNotRequireTimeStamp = new PublicOpenIdConnectProtocolValidator { RequireTimeStampInNonce = false };
                var nonceWithoutTimestamp = protocolValidatorDoesNotRequireTimeStamp.GenerateNonce();
                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidNonceException), "IDX21325:"),
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator { RequireNonce = false, RequireTimeStampInNonce = true },
                    TestId = "nonce.timestamp == false, RequireTimeStampInNonce == true",
                    ValidationContext = new OpenIdConnectProtocolValidationContext
                    {
                        Nonce = nonceWithoutTimestamp,
                        ValidatedIdToken = CreateValidatedIdToken(JwtRegisteredClaimNames.Nonce, nonceWithoutTimestamp)
                    }
                });

                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidNonceException), "IDX21321:"),
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator { RequireNonce = false, RequireTimeStampInNonce = true },
                    TestId = "ValidationContext.Nonce != Jwt.Nonce",
                    ValidationContext = new OpenIdConnectProtocolValidationContext
                    {
                        Nonce = protocolValidatorRequiresTimeStamp.GenerateNonce(),
                        ValidatedIdToken = CreateValidatedIdToken(JwtRegisteredClaimNames.Nonce, protocolValidatorRequiresTimeStamp.GenerateNonce())
                    }
                });

                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidNonceException), "IDX21326:", typeof(FormatException)),
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator { RequireNonce = false, RequireTimeStampInNonce = true },
                    TestId = "Nonce timestamp is not formated as a int",
                    ValidationContext = new OpenIdConnectProtocolValidationContext
                    {
                        Nonce = "abc.abc",
                        ValidatedIdToken = CreateValidatedIdToken(JwtRegisteredClaimNames.Nonce, "abc.abc")
                    }
                });

                string nonceExpired = (DateTime.UtcNow - TimeSpan.FromDays(20)).Ticks.ToString(CultureInfo.InvariantCulture) + "." + nonceWithoutTimestamp;
                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidNonceException), "IDX21324:"),
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator { RequireTimeStampInNonce = true },
                    TestId = "Nonce is expired",
                    ValidationContext = new OpenIdConnectProtocolValidationContext
                    {
                        Nonce = nonceExpired,
                        ValidatedIdToken = CreateValidatedIdToken(JwtRegisteredClaimNames.Nonce, nonceExpired)
                    }
                });

                string nonceMaxTicks = Int64.MaxValue.ToString() + "." + nonceWithoutTimestamp;
                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidNonceException), "IDX21327:", typeof(ArgumentException)),
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator { RequireNonce = false, RequireTimeStampInNonce = true },
                    TestId = "Nonce ticks == Int64.MaxValue",
                    ValidationContext = new OpenIdConnectProtocolValidationContext
                    {
                        Nonce = nonceMaxTicks,
                        ValidatedIdToken = CreateValidatedIdToken(JwtRegisteredClaimNames.Nonce, nonceMaxTicks)
                    }
                });

                string nonceMinTicks = Int64.MinValue.ToString() + "." + nonceWithoutTimestamp;
                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidNonceException), "IDX21326:"),
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator { RequireNonce = false, RequireTimeStampInNonce = true },
                    TestId = "Nonce ticks == Int64.MinValue",
                    ValidationContext = new OpenIdConnectProtocolValidationContext
                    {
                        Nonce = nonceMinTicks,
                        ValidatedIdToken = CreateValidatedIdToken(JwtRegisteredClaimNames.Nonce, nonceMinTicks)
                    }
                });

                string nonceTicksNegative = ((Int64)(-1)).ToString() + "." + nonceWithoutTimestamp;
                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidNonceException), "IDX21326:"),
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator { RequireNonce = false, RequireTimeStampInNonce = true },
                    TestId = "Nonce ticks == ((Int64)(-1))",
                    ValidationContext = new OpenIdConnectProtocolValidationContext
                    {
                        Nonce = nonceTicksNegative,
                        ValidatedIdToken = CreateValidatedIdToken(JwtRegisteredClaimNames.Nonce, nonceTicksNegative)
                    }
                });

                string nonceTicksZero = ((Int64)(0)).ToString() + "." + nonceWithoutTimestamp;
                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidNonceException), "IDX21326:"),
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator { RequireNonce = false, RequireTimeStampInNonce = true },
                    TestId = "Nonce ticks ==  ((Int64)(0))",
                    ValidationContext = new OpenIdConnectProtocolValidationContext
                    {
                        Nonce = nonceTicksZero,
                        ValidatedIdToken = CreateValidatedIdToken(JwtRegisteredClaimNames.Nonce, nonceTicksZero)
                    }
                });

                return theoryData;
            }
        }

        [Theory, MemberData(nameof(ValidateAtHashTheoryData))]
        public void ValidateAtHash(OidcProtocolValidatorTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ValidateAtHash", theoryData);
            try
            {
                theoryData.ProtocolValidator.PublicValidateAtHash(theoryData.ValidationContext);
                theoryData.ExpectedException.ProcessNoException();
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex);
            }
        }

        public static TheoryData<OidcProtocolValidatorTheoryData> ValidateAtHashTheoryData
        {
            get
            {
                var theoryData = new TheoryData<OidcProtocolValidatorTheoryData>
                {
                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        First = true,
                        ProtocolValidator = new PublicOpenIdConnectProtocolValidator(),
                        TestId = "validationContext == null"
                    },

                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        ProtocolValidator = new PublicOpenIdConnectProtocolValidator(),
                        TestId = "validationContext.ValidatedIdToken == null",
                        ValidationContext = new OpenIdConnectProtocolValidationContext { ProtocolMessage = new OpenIdConnectMessage() }
                    },

                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX21333:"),
                        ProtocolValidator = new PublicOpenIdConnectProtocolValidator(),
                        TestId = "validationContext.ProtocolMessage == null",
                        ValidationContext = new OpenIdConnectProtocolValidationContext { ValidatedIdToken = CreateValidatedIdToken() }
                    }
                };

                var token = Guid.NewGuid().ToString();
                var hashClaimValue256 = IdentityUtilities.CreateHashClaim(token, "SHA256");
                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator(),
                    TestId = "Jwt.at_hash == hash(access_token)",
                    ValidationContext = new OpenIdConnectProtocolValidationContext()
                    {
                        ProtocolMessage = new OpenIdConnectMessage { AccessToken = token },
                        ValidatedIdToken = new JwtSecurityToken(claims: new List<Claim> { new Claim("at_hash", hashClaimValue256) }, signingCredentials: Default.AsymmetricSigningCredentials)
                    }
                });

                var hashClaimValue512 = IdentityUtilities.CreateHashClaim(token, "SHA512");
                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidAtHashException), "IDX21348:", typeof(OpenIdConnectProtocolException)),
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator(),
                    TestId = "Jwt.at_hash != hash(access_token) - 256 - 512",
                    ValidationContext = new OpenIdConnectProtocolValidationContext()
                    {
                        ProtocolMessage = new OpenIdConnectMessage { AccessToken = token },
                        ValidatedIdToken = new JwtSecurityToken(claims: new List<Claim> { new Claim("at_hash", hashClaimValue512) }, signingCredentials: Default.AsymmetricSigningCredentials)
                    }
                });

                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidAtHashException), "IDX21348:", typeof(OpenIdConnectProtocolException)),
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator(),
                    TestId = "Jwt.at_hash != hash(access_token)",
                    ValidationContext = new OpenIdConnectProtocolValidationContext
                    {
                        ProtocolMessage = new OpenIdConnectMessage { AccessToken = Guid.NewGuid().ToString() },
                        ValidatedIdToken = new JwtSecurityToken(claims: new List<Claim> { new Claim("at_hash", hashClaimValue256) }, signingCredentials: Default.AsymmetricSigningCredentials)
                    }
                });

                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidAtHashException), "IDX21311:"),
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator(),
                    TestId = "multiple at_hash claims",
                    ValidationContext = new OpenIdConnectProtocolValidationContext()
                    {
                        ProtocolMessage = new OpenIdConnectMessage { AccessToken = Guid.NewGuid().ToString() },
                        ValidatedIdToken = new JwtSecurityToken(claims: new List<Claim> { new Claim("at_hash", hashClaimValue256), new Claim("at_hash", hashClaimValue256) }, signingCredentials: Default.AsymmetricSigningCredentials)
                    }
                });

                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidAtHashException), "IDX21312:"),
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator(),
                    TestId = "Jwt.at_hash == null",
                    ValidationContext = new OpenIdConnectProtocolValidationContext
                    {
                        ProtocolMessage = new OpenIdConnectMessage { AccessToken = Guid.NewGuid().ToString() },
                        ValidatedIdToken = CreateValidatedIdToken()
                    }
                });

                return theoryData;
            }
        }

        [Theory, MemberData(nameof(ValidateJWEPayloadAtHashTheoryData))]
        public void ValidateJWEPayloadAtHash(OidcProtocolValidatorTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateJWEPayloadAtHash", theoryData);
            try
            {
                theoryData.ProtocolValidator.PublicValidateAtHash(theoryData.ValidationContext);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }
        }

        public static TheoryData<OidcProtocolValidatorTheoryData> ValidateJWEPayloadAtHashTheoryData
        {
            get
            {
                var token = Guid.NewGuid().ToString();
                var hashClaimValue256 = IdentityUtilities.CreateHashClaim(token, "SHA256");

                var theoryData = new TheoryData<OidcProtocolValidatorTheoryData>
                {
                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidAtHashException), "IDX21312:"),
                        ProtocolValidator = new PublicOpenIdConnectProtocolValidator(),
                        TestId = "Jwt.at_hash == hash(access_token)",
                        ValidationContext = new OpenIdConnectProtocolValidationContext
                        {
                            ProtocolMessage = new OpenIdConnectMessage { AccessToken = token },
                            ValidatedIdToken = CreateJWEValidatedIdToken("at_hash", hashClaimValue256, SecurityAlgorithms.RsaSha256)
                        }
                    }
                };

                return theoryData;
            }
        }

        [Theory, MemberData(nameof(ValidateStateTheoryData))]
        public void ValidateState(OidcProtocolValidatorTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ValidateState", theoryData);
            try
            {
                theoryData.ProtocolValidator.PublicValidateState(theoryData.ValidationContext);
                theoryData.ExpectedException.ProcessNoException();
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex);
            }
        }

        public static TheoryData<OidcProtocolValidatorTheoryData> ValidateStateTheoryData
        {
            get
            {
                var theoryData = new TheoryData<OidcProtocolValidatorTheoryData>
                {
                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                        First = true,
                        ProtocolValidator = new PublicOpenIdConnectProtocolValidator(),
                        TestId = "validationContext == null"
                    },

                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidStateException), "IDX21329:"),
                        ProtocolValidator = new PublicOpenIdConnectProtocolValidator { RequireState = true },
                        TestId = "validationContext.State == null",
                        ValidationContext = new OpenIdConnectProtocolValidationContext { ProtocolMessage = new OpenIdConnectMessage() }
                    },

                    new OidcProtocolValidatorTheoryData
                    {
                        ProtocolValidator = new PublicOpenIdConnectProtocolValidator { RequireState = false },
                        TestId = "validationContext.State == null, RequireState == false",
                        ValidationContext = new OpenIdConnectProtocolValidationContext { ProtocolMessage = new OpenIdConnectMessage() }
                    },

                    new OidcProtocolValidatorTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidStateException), "IDX21330:"),
                        ProtocolValidator = new PublicOpenIdConnectProtocolValidator { RequireState = true },
                        TestId = "validationContext.state != null, protocolMessage.state == null",
                        ValidationContext = new OpenIdConnectProtocolValidationContext
                        {
                            ProtocolMessage = new OpenIdConnectMessage(),
                            State = Guid.NewGuid().ToString()
                        }
                    }
                };

                var state = Guid.NewGuid().ToString();
                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator { RequireState = true },
                    TestId = "validationContext.state == protocolMessage.state",
                    ValidationContext = new OpenIdConnectProtocolValidationContext()
                    {
                        State = state,
                        ProtocolMessage = new OpenIdConnectMessage { State = state }
                    }
                });

                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidStateException), "IDX21331:"),
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator { RequireState = true },
                    TestId = "validationContext.state != protocolMessage.state, RequireState = true",
                    ValidationContext = new OpenIdConnectProtocolValidationContext()
                    {
                        State = Guid.NewGuid().ToString(),
                        ProtocolMessage = new OpenIdConnectMessage { State = Guid.NewGuid().ToString() }
                    },
                });

                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator { RequireStateValidation = false },
                    TestId = "validationContext.state != protocolMessage.state, RequireStateValidation = false",
                    ValidationContext = new OpenIdConnectProtocolValidationContext()
                    {
                        State = Guid.NewGuid().ToString(),
                        ProtocolMessage = new OpenIdConnectMessage { State = Guid.NewGuid().ToString() },
                    },
                });

                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator { RequireState = false },
                    TestId = "validationContext.state == null, protocolMessage.state == null, RequireState = false",
                    ValidationContext = new OpenIdConnectProtocolValidationContext()
                    {
                        ProtocolMessage = new OpenIdConnectMessage(),
                    },
                });

                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidStateException), "IDX21330:"),
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator { RequireState = false },
                    TestId = "validationContext.state != null, protocolMessage.state == null, RequireState = false",
                    ValidationContext = new OpenIdConnectProtocolValidationContext()
                    {
                        ProtocolMessage = new OpenIdConnectMessage(),
                        State = Guid.NewGuid().ToString()
                    },
                });

                theoryData.Add(new OidcProtocolValidatorTheoryData
                {
                    ExpectedException = new ExpectedException(typeof(OpenIdConnectProtocolInvalidStateException), "IDX21329:"),
                    ProtocolValidator = new PublicOpenIdConnectProtocolValidator { RequireState = false },
                    TestId = "validationContext.state == null, protocolMessage.state != null, RequireState = false",
                    ValidationContext = new OpenIdConnectProtocolValidationContext()
                    {
                        ProtocolMessage = new OpenIdConnectMessage { State = Guid.NewGuid().ToString() }
                    },
                });

                return theoryData;
            }
        }

        [Theory]
        [InlineData(SecurityAlgorithms.EcdsaSha256, "SHA256", true)]
        [InlineData(SecurityAlgorithms.EcdsaSha256Signature, "SHA256", true)]
        [InlineData(SecurityAlgorithms.HmacSha256, "SHA256", true)]
        [InlineData(SecurityAlgorithms.RsaSha256, "SHA256", true)]
        [InlineData(SecurityAlgorithms.RsaSha256Signature, "SHA256", true)]
        [InlineData(SecurityAlgorithms.RsaSsaPssSha256, "SHA256", true)]
        [InlineData(SecurityAlgorithms.EcdsaSha384, "SHA384", true)]
        [InlineData(SecurityAlgorithms.EcdsaSha384Signature, "SHA384", true)]
        [InlineData(SecurityAlgorithms.HmacSha384, "SHA384", true)]
        [InlineData(SecurityAlgorithms.RsaSha384, "SHA384", true)]
        [InlineData(SecurityAlgorithms.RsaSha384Signature, "SHA384", true)]
        [InlineData(SecurityAlgorithms.RsaSsaPssSha384, "SHA384", true)]
        [InlineData(SecurityAlgorithms.EcdsaSha512, "SHA512", true)]
        [InlineData(SecurityAlgorithms.EcdsaSha512Signature, "SHA512", true)]
        [InlineData(SecurityAlgorithms.HmacSha512, "SHA512", true)]
        [InlineData(SecurityAlgorithms.RsaSha512, "SHA512", true)]
        [InlineData(SecurityAlgorithms.RsaSha512Signature, "SHA512", true)]
        [InlineData(SecurityAlgorithms.RsaSsaPssSha512, "SHA512", true)]
        [InlineData(SecurityAlgorithms.ExclusiveC14nWithComments, "SHA512", false)]
        [InlineData(SecurityAlgorithms.Aes128KeyWrap, "SHA512", false)]
        public void DefaultAlgorithmMapTest(string algorithm, string expectedHash, bool shouldFind)
        {
            var protocolValidator = new OpenIdConnectProtocolValidator();
            string hashFound;
            Assert.True(protocolValidator.HashAlgorithmMap.TryGetValue(algorithm, out hashFound) == shouldFind);
            if (shouldFind)
                Assert.Equal(hashFound, expectedHash);
        }

        [Theory, MemberData(nameof(HashAlgorithmExtensibilityTheoryData))]
        public void HashAlgorithmExtensibility(OpenIdConnectProtocolValidator protocolValidator, string alg, Type algorithmType, ExpectedException ee)
        {
            ee.Verbose = false;
            try
            {
                var hash = protocolValidator.GetHashAlgorithm(alg);
                ee.ProcessNoException();
                Assert.True(hash.GetType() == algorithmType, string.Format(CultureInfo.InvariantCulture, "hash.GetType() != algorithmType: '{0}' : '{1}'", hash.GetType(), algorithmType));
            }
            catch (Exception ex)
            {
                ee.ProcessException(ex);
            }
        }

        public static TheoryData<OpenIdConnectProtocolValidator, string, Type, ExpectedException> HashAlgorithmExtensibilityTheoryData
        {
            get
            {
                var theoryData = new TheoryData<OpenIdConnectProtocolValidator, string, Type, ExpectedException>();

                // CustomCryptoProviderFactory understands this 'hash' algorithm
                var customHashAlgorithm = new CustomHashAlgorithm();
                var customCryptoProviderFactory = new CustomCryptoProviderFactory()
                {
                    HashAlgorithm = customHashAlgorithm
                };

                var validator = new OpenIdConnectProtocolValidator()
                {
                    CryptoProviderFactory = customCryptoProviderFactory
                };

                theoryData.Add(validator, SecurityAlgorithms.ExclusiveC14nWithComments, customHashAlgorithm.GetType(), ExpectedException.NoExceptionExpected);

                var cryptoProviderFactory = new CryptoProviderFactory(new InMemoryCryptoProviderCache(new CryptoProviderCacheOptions(), TaskCreationOptions.None, 50));

                // Default CryptoProviderFactory faults on this 'hash' algorithm
                validator = new OpenIdConnectProtocolValidator()
                {
                    CryptoProviderFactory = cryptoProviderFactory
                };

                theoryData.Add(validator, SecurityAlgorithms.ExclusiveC14nWithComments, customHashAlgorithm.GetType(), new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX21301:", typeof(NotSupportedException)));

                // Adjust mapping table, and Default CryptoProviderFactory will find 'hash' algorithm
                var sha2 = SHA256.Create();
                validator = new OpenIdConnectProtocolValidator();
                validator.HashAlgorithmMap[SecurityAlgorithms.ExclusiveC14nWithComments] = SecurityAlgorithms.Sha256;
                theoryData.Add(validator, SecurityAlgorithms.ExclusiveC14nWithComments, sha2.GetType(), ExpectedException.NoExceptionExpected);

                // Support a single hash algorithm, add CryptoProvider that supports hash algorithm
                var cryptoProvider = new CustomCryptoProvider()
                {
                    HashAlgorithm = customHashAlgorithm,
                    IsSupportedResult = true
                };

                cryptoProvider.AdditionalHashAlgorithms.Add(SecurityAlgorithms.ExclusiveC14nWithComments);

                validator = new OpenIdConnectProtocolValidator()
                {
                    CryptoProviderFactory = new CryptoProviderFactory()
                };

                validator.CryptoProviderFactory.CustomCryptoProvider = cryptoProvider;
                theoryData.Add(validator, SecurityAlgorithms.ExclusiveC14nWithComments, customHashAlgorithm.GetType(), ExpectedException.NoExceptionExpected);

                return theoryData;
            }
        }

        [Theory, MemberData(nameof(GetHashAlgorithmTheoryData))]
        public void GetHashAlgorithm(OpenIdConnectProtocolValidator protocolValidator, string alg, Type algorithmType, ExpectedException ee)
        {
            ee.Verbose = false;
            try
            {
                var hash = protocolValidator.GetHashAlgorithm(alg);
                ee.ProcessNoException();
                Assert.True(hash.GetType() == algorithmType);
            }
            catch (Exception ex)
            {
                ee.ProcessException(ex);
            }
        }

        public static TheoryData<OpenIdConnectProtocolValidator, string, Type, ExpectedException> GetHashAlgorithmTheoryData
        {
            get
            {
                var validator = new OpenIdConnectProtocolValidator();
                var sha2 = SHA256.Create();
                var sha3 = SHA384.Create();
                var sha5 = SHA512.Create();

                return new TheoryData<OpenIdConnectProtocolValidator, string, Type, ExpectedException>
                {
                    {validator, SecurityAlgorithms.EcdsaSha256, sha2.GetType(), ExpectedException.NoExceptionExpected },
                    {validator, SecurityAlgorithms.EcdsaSha256Signature, sha2.GetType(), ExpectedException.NoExceptionExpected},
                    {validator, SecurityAlgorithms.HmacSha256, sha2.GetType(), ExpectedException.NoExceptionExpected},
                    {validator, SecurityAlgorithms.RsaSha256, sha2.GetType(), ExpectedException.NoExceptionExpected},
                    {validator, SecurityAlgorithms.RsaSha256Signature, sha2.GetType(), ExpectedException.NoExceptionExpected},
                    {validator, SecurityAlgorithms.RsaSsaPssSha256, sha2.GetType(), ExpectedException.NoExceptionExpected},

                    {validator, SecurityAlgorithms.EcdsaSha384, sha3.GetType(), ExpectedException.NoExceptionExpected },
                    {validator, SecurityAlgorithms.HmacSha384, sha3.GetType(), ExpectedException.NoExceptionExpected },
                    {validator, SecurityAlgorithms.RsaSha384, sha3.GetType(), ExpectedException.NoExceptionExpected},
                    {validator, SecurityAlgorithms.RsaSsaPssSha384, sha3.GetType(), ExpectedException.NoExceptionExpected},
                    {validator, SecurityAlgorithms.RsaSha384Signature, sha3.GetType(), ExpectedException.NoExceptionExpected},
                    {validator, SecurityAlgorithms.EcdsaSha384Signature, sha3.GetType(), ExpectedException.NoExceptionExpected},

                    {validator, SecurityAlgorithms.RsaSha512Signature, sha5.GetType(), ExpectedException.NoExceptionExpected },
                    {validator, SecurityAlgorithms.RsaSsaPssSha512, sha5.GetType(), ExpectedException.NoExceptionExpected },
                    {validator, SecurityAlgorithms.EcdsaSha512, sha5.GetType(), ExpectedException.NoExceptionExpected},
                    {validator, SecurityAlgorithms.EcdsaSha512Signature, sha5.GetType(), ExpectedException.NoExceptionExpected},
                    {validator, SecurityAlgorithms.HmacSha512, sha5.GetType(), ExpectedException.NoExceptionExpected},
                    {validator, SecurityAlgorithms.RsaSha512, sha5.GetType(), ExpectedException.NoExceptionExpected},

                    {validator, SecurityAlgorithms.ExclusiveC14nWithComments, sha5.GetType(), new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX21301:", typeof(NotSupportedException))}
                };
            }
        }
    }

    public class OidcProtocolValidatorTheoryData : TheoryDataBase
    {
        public JwtSecurityToken JwtSecurityToken { get; set; }

        public OpenIdConnectProtocolValidationContext ValidationContext { get; set; }

        public PublicOpenIdConnectProtocolValidator ProtocolValidator { get; set; } = new PublicOpenIdConnectProtocolValidator();
    }

    public class PublicOpenIdConnectProtocolValidator : OpenIdConnectProtocolValidator
    {
        public void PublicValidateIdToken(OpenIdConnectProtocolValidationContext context)
        {
            base.ValidateIdToken(context);
        }

        public void PublicValidateCHash(OpenIdConnectProtocolValidationContext context)
        {
            base.ValidateCHash(context);
        }

        public void PublicValidateAtHash(OpenIdConnectProtocolValidationContext context)
        {
            base.ValidateAtHash(context);
        }

        public void PublicValidateNonce(OpenIdConnectProtocolValidationContext context)
        {
            base.ValidateNonce(context);
        }

        public void PublicValidateState(OpenIdConnectProtocolValidationContext context)
        {
            base.ValidateState(context);
        }

        public void SetHashAlgorithmMap(Dictionary<string, string> hashAlgorithmMap)
        {
            HashAlgorithmMap.Clear();
            foreach (var key in hashAlgorithmMap.Keys)
                HashAlgorithmMap.Add(key, hashAlgorithmMap[key]);
        }
    }

    internal class SampleListener : EventListener
    {
        public string TraceBuffer { get; set; }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            if (eventData != null && eventData.Payload.Count > 0)
            {
                TraceBuffer += eventData.Payload[0] + "\n";
            }
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
