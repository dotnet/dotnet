// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.WsFederation;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Xml;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Saml.Tests
{
    /// <summary>
    /// 
    /// </summary>
    public class SamlSecurityTokenHandlerTests
    {
        [Fact]
        public void Constructors()
        {
            SamlSecurityTokenHandler samlSecurityTokenHandler = new SamlSecurityTokenHandler();
        }

        [Fact]
        public void Defaults()
        {
            SamlSecurityTokenHandler samlSecurityTokenHandler = new SamlSecurityTokenHandler();
            Assert.True(samlSecurityTokenHandler.MaximumTokenSizeInBytes == TokenValidationParameters.DefaultMaximumTokenSizeInBytes, "MaximumTokenSizeInBytes");
        }

        [Fact]
        public void GetSets()
        {
            var samlSecurityTokenHandler = new SamlSecurityTokenHandler();
            var context = new GetSetContext
            {
                PropertyNamesAndSetGetValue = new List<KeyValuePair<string, List<object>>>
                {
                    new KeyValuePair<string, List<object>>("MaximumTokenSizeInBytes", new List<object>{(object)TokenValidationParameters.DefaultMaximumTokenSizeInBytes, (object)1000, (object)10}),
                    new KeyValuePair<string, List<object>>("SetDefaultTimesOnTokenCreation", new List<object>{true, false, true}),
                    new KeyValuePair<string, List<object>>("TokenLifetimeInMinutes", new List<object>{(object)60, (object)1000, (object)10}),
                },
                Object = samlSecurityTokenHandler
            };

            TestUtilities.GetSet(context);

            samlSecurityTokenHandler = new SamlSecurityTokenHandler();
            TestUtilities.SetGet(samlSecurityTokenHandler, "MaximumTokenSizeInBytes", (object)0, ExpectedException.ArgumentOutOfRangeException(substringExpected: "IDX10101"), context);
            TestUtilities.SetGet(samlSecurityTokenHandler, "MaximumTokenSizeInBytes", (object)1, ExpectedException.NoExceptionExpected, context);
            TestUtilities.SetGet(samlSecurityTokenHandler, "Serializer", null, ExpectedException.ArgumentNullException(), context);

            TestUtilities.AssertFailIfErrors("Saml2SecurityTokenHandlerTests_GetSets", context.Errors);

        }

        [Theory, MemberData(nameof(CanReadTokenTheoryData))]
        public void CanReadToken(SamlTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CanReadToken", theoryData);
            try
            {
                if (theoryData.CanRead != theoryData.Handler.CanReadToken(theoryData.Token))
                    Assert.False(true, $"Expected CanRead != CanRead, token: {theoryData.Token}");

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SamlTheoryData> CanReadTokenTheoryData
        {
            get
            {
                return new TheoryData<SamlTheoryData>
                {
                    new SamlTheoryData
                    {
                        First = true,
                        CanRead = false,
                        Handler = new SamlSecurityTokenHandler(),
                        TestId = "Null Token",
                        Token = null
                    },

                    new SamlTheoryData
                    {
                        CanRead = false,
                        Handler = new SamlSecurityTokenHandler(),
                        TestId = "DefaultMaximumTokenSizeInBytes + 1",
                        Token = new string('S', TokenValidationParameters.DefaultMaximumTokenSizeInBytes + 2)
                    },

                    new SamlTheoryData
                    {
                        CanRead = true,
                        Handler = new SamlSecurityTokenHandler(),
                        TestId = nameof(ReferenceTokens.SamlToken_Valid),
                        Token = ReferenceTokens.SamlToken_Valid
                    }
                };
            }
        }

        [Theory, MemberData(nameof(CreateClaimsIdentitiesTheoryData))]
        public void CreateClaimsIdentities(SamlTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.CreateClaimsIdentities", theoryData);
            var context = new CompareContext($"{this}.CreateClaimsIdentities, {theoryData.TestId}");

            try
            {
                var identities = ((theoryData.Handler) as SamlSecurityTokenHandlerPublic).CreateClaimsIdentitiesPublic(theoryData.TokenTestSet.SecurityToken as SamlSecurityToken, theoryData.Issuer, theoryData.ValidationParameters);
                theoryData.ExpectedException.ProcessNoException(context);

                IdentityComparer.AreEqual(identities, theoryData.TokenTestSet.Identities, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SamlTheoryData> CreateClaimsIdentitiesTheoryData
        {
            get
            {
                return new TheoryData<SamlTheoryData>
                {
                    new SamlTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenException), "IDX11313:"),
                        First = true,
                        Handler = new SamlSecurityTokenHandlerPublic(),
                        Issuer = Default.Issuer,
                        TestId = nameof(ReferenceSaml.TokenClaimsIdentitiesSubjectEmptyString),
                        TokenTestSet = ReferenceSaml.TokenClaimsIdentitiesSubjectEmptyString,
                        ValidationParameters = new TokenValidationParameters()
                    },
                    new SamlTheoryData
                    {
                        Handler = new SamlSecurityTokenHandlerPublic(),
                        Issuer = Default.Issuer,
                        TestId = nameof(ReferenceSaml.TokenClaimsIdentitiesSameSubject),
                        TokenTestSet = ReferenceSaml.TokenClaimsIdentitiesSameSubject,
                        ValidationParameters = new TokenValidationParameters()
                    },
                    new SamlTheoryData
                    {
                        Handler = new SamlSecurityTokenHandlerPublic(),
                        Issuer = Default.Issuer,
                        TestId = nameof(ReferenceSaml.TokenClaimsIdentitiesDifferentSubjects),
                        TokenTestSet = ReferenceSaml.TokenClaimsIdentitiesDifferentSubjects,
                        ValidationParameters = new TokenValidationParameters()
                    }
                };
            }
        }

        [Theory, MemberData(nameof(ReadTokenTheoryData))]
        public void ReadToken(SamlTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ReadToken", theoryData);
            SecurityToken samlTokenFromString = null;
            SecurityToken samlTokenFromXmlReader = null;
            try
            {
                samlTokenFromString = theoryData.Handler.ReadToken(theoryData.Token);
                samlTokenFromXmlReader = theoryData.Handler.ReadToken(theoryData.XmlReader);
                theoryData.ExpectedException.ProcessNoException(context);
                IdentityComparer.AreEqual(samlTokenFromString, samlTokenFromXmlReader, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SamlTheoryData> ReadTokenTheoryData
        {
            get
            {
                return new TheoryData<SamlTheoryData>
                {
                    new SamlTheoryData
                    {
                        ExpectedException = ExpectedException.NoExceptionExpected,
                        First = true,
                        Handler = new SamlSecurityTokenHandler(),
                        TestId = nameof(ReferenceTokens.SamlToken_Valid),
                        Token = ReferenceTokens.SamlToken_Valid,
                        XmlReader = XmlReader.Create(new StringReader(ReferenceTokens.SamlToken_Valid), new XmlReaderSettings() { XmlResolver = null })
                    }
                };
            }
        }

        // Test checks to make sure that default times are correctly added to the token
        // upon token creation.
        [Fact]
        public void SetDefaultTimesOnTokenCreation()
        {
            TestUtilities.WriteHeader($"{this}.SetDefaultTimesOnTokenCreation");
            var context = new CompareContext();

            var tokenHandler = new SamlSecurityTokenHandler();
            var descriptorNoTimeValues = new SecurityTokenDescriptor()
            {
                Issuer = Default.Issuer,
                Audience = Default.Audience,
                SigningCredentials = Default.AsymmetricSigningCredentials
            };

            var token = tokenHandler.CreateToken(descriptorNoTimeValues);
            var samlSecurityToken = token as SamlSecurityToken;

            Assert.NotEqual(DateTime.MinValue, samlSecurityToken.ValidFrom);
            Assert.NotEqual(DateTime.MinValue, samlSecurityToken.ValidTo);
        }

        [Theory, MemberData(nameof(ValidateAudienceTheoryData))]
        public void ValidateAudience(SamlTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateAudience", theoryData);
            try
            {
                ((theoryData.Handler) as SamlSecurityTokenHandlerPublic).ValidateAudiencePublic(theoryData.Audiences, null, theoryData.ValidationParameters);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SamlTheoryData> ValidateAudienceTheoryData
        {
            get
            {
                var tokenTheoryData = new List<TokenTheoryData>();
                ValidateTheoryData.AddValidateAudienceTheoryData(tokenTheoryData);

                var theoryData = new TheoryData<SamlTheoryData>();
                foreach (var item in tokenTheoryData)
                    theoryData.Add(new SamlTheoryData(item)
                    {
                        Handler = new SamlSecurityTokenHandlerPublic()
                    });

                return theoryData;
            }
        }

        [Theory, MemberData(nameof(ValidateIssuerTheoryData))]
        public void ValidateIssuer(SamlTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateIssuer", theoryData);
            try
            {
                ((theoryData.Handler) as SamlSecurityTokenHandlerPublic).ValidateIssuerPublic(theoryData.Issuer, null, theoryData.ValidationParameters);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SamlTheoryData> ValidateIssuerTheoryData
        {
            get
            {
                var tokenTheoryData = new List<TokenTheoryData>();
                ValidateTheoryData.AddValidateIssuerTheoryData(tokenTheoryData);

                var theoryData = new TheoryData<SamlTheoryData>();
                foreach (var item in tokenTheoryData)
                    theoryData.Add(new SamlTheoryData(item)
                    {
                        Handler = new SamlSecurityTokenHandlerPublic()
                    });

                return theoryData;
            }
        }

        [Theory, MemberData(nameof(ValidateTokenTheoryData))]
        public void ValidateToken(SamlTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateToken", theoryData);
            try
            {
                (theoryData.Handler as SamlSecurityTokenHandler).ValidateToken(theoryData.Token, theoryData.ValidationParameters, out SecurityToken validatedToken);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory, MemberData(nameof(ValidateTokenTheoryData))]
        public async Task ValidateTokenAsync(SamlTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateToken", theoryData);
            var validationResult = await (theoryData.Handler as SamlSecurityTokenHandler).ValidateTokenAsync(theoryData.Token, theoryData.ValidationParameters);
            if (validationResult.Exception != null)
            {
                theoryData.ExpectedException.ProcessException(validationResult.Exception, context);
            }
            else
            {
                theoryData.ExpectedException.ProcessNoException(context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SamlTheoryData> ValidateTokenTheoryData
        {
            get
            {
                // uncomment to view exception displayed to user
                // ExpectedException.DefaultVerbose = true;

                return new TheoryData<SamlTheoryData>
                {
                    new SamlTheoryData("Null_SecurityToken")
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                        First = true,
                        Token = null,
                        ValidationParameters = new TokenValidationParameters()
                    },
                    new SamlTheoryData("NULL_TokenValidationParameters")
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                        Handler = new SamlSecurityTokenHandler { MaximumTokenSizeInBytes = 1 },
                        Token = "s",
                        ValidationParameters = null,
                    },
                    new SamlTheoryData("SecurityTokenTooLarge")
                    {
                        ExpectedException = ExpectedException.ArgumentException("IDX10209:"),
                        Handler = new SamlSecurityTokenHandler { MaximumTokenSizeInBytes = 1 },
                        Token = "ss",
                        ValidationParameters = new TokenValidationParameters(),
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_MissingMajorVersion")
                    {
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11115:"),
                        Token = ReferenceTokens.SamlToken_MissingMajorVersion,
                        ValidationParameters = new TokenValidationParameters(),
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_MissingMinorVersion")
                    {
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11115:"),
                        Token = ReferenceTokens.SamlToken_MissingMinorVersion,
                        ValidationParameters = new TokenValidationParameters(),
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_MajorVersionNotV1")
                    {
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11116:"),
                        Token = ReferenceTokens.SamlToken_MajorVersionNotV1,
                        ValidationParameters = new TokenValidationParameters(),
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_MinorVersionNotV1")
                    {
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11117:"),
                        Token = ReferenceTokens.SamlToken_MinorVersionNotV1,
                        ValidationParameters = new TokenValidationParameters(),
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_IdMissing")
                    {
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11115:"),
                        Token = ReferenceTokens.SamlToken_IdMissing,
                        ValidationParameters = new TokenValidationParameters(),
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_IdFormatError")
                    {
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11121:"),
                        Token = ReferenceTokens.SamlToken_IdFormatError,
                        ValidationParameters = new TokenValidationParameters(),
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_IssuerMissing")
                    {
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11115:"),
                        Token = ReferenceTokens.SamlToken_IssuerMissing,
                        ValidationParameters = new TokenValidationParameters(),
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_IssueInstantMissing")
                    {
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11115:"),
                        Token = ReferenceTokens.SamlToken_IssueInstantMissing,
                        ValidationParameters = new TokenValidationParameters(),
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_IssueInstantFormatError")
                    {
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11122:", typeof(FormatException)),
                        Token = ReferenceTokens.SamlToken_IssueInstantFormatError,
                        ValidationParameters = new TokenValidationParameters(),
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_AudienceMissing")
                    {
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11120:"),
                        Token = ReferenceTokens.SamlToken_AudienceMissing,
                        ValidationParameters = new TokenValidationParameters(),
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_NoStatements")
                    {
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11130:"),
                        Token = ReferenceTokens.SamlToken_NoStatements,
                        ValidationParameters = new TokenValidationParameters(),
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_NoSubject")
                    {
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11112:", typeof(XmlReadException)),
                        Token = ReferenceTokens.SamlToken_NoSubject,
                        ValidationParameters = new TokenValidationParameters(),
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_NoAttributes")
                    {
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11131:"),
                        Token = ReferenceTokens.SamlToken_NoAttributes,
                        ValidationParameters = new TokenValidationParameters(),
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_Valid_Issuer_ConfigurationManager")
                    {
                        ExpectedException = ExpectedException.SecurityTokenSignatureKeyNotFoundException("IDX10500:"),
                        Token = ReferenceTokens.SamlToken_Valid,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new StaticConfigurationManager<BaseConfiguration>(new WsFederationConfiguration()
                            {
                                Issuer = "http://Default.Issuer.com",
                            }),
                            ValidateIssuerSigningKey = false,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        }
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_Valid_IssuerSigningKey_ConfigurationManager")
                    {
                        Token = ReferenceTokens.SamlToken_Valid,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new StaticConfigurationManager<BaseConfiguration>(new WsFederationConfiguration()
                            {
                                SigningKeys = { KeyingMaterial.DefaultX509SigningCreds_2048_RsaSha2_Sha2.Key },
                            }),
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        }
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_Valid_IssuerSigningKey_and_Issuer_ConfigurationManager")
                    {
                        Token = ReferenceTokens.SamlToken_Valid,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new StaticConfigurationManager<BaseConfiguration>(new WsFederationConfiguration()
                            {
                                Issuer = "http://Default.Issuer.com",
                                SigningKeys = { KeyingMaterial.DefaultX509SigningCreds_2048_RsaSha2_Sha2.Key },
                            }),
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        }
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_Valid_NoSigningKey_ConfigurationManager")
                    {
                        ExpectedException = ExpectedException.SecurityTokenSignatureKeyNotFoundException("IDX10500:"),
                        Token = ReferenceTokens.SamlToken_Valid,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ConfigurationManager = new StaticConfigurationManager<BaseConfiguration>(new WsFederationConfiguration()),
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        }
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_Valid_IssuerSigningKey_Set")
                    {
                        Token = ReferenceTokens.SamlToken_Valid,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultX509SigningCreds_2048_RsaSha2_Sha2.Key,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        }
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_Valid_IssuerSigningKey_Rsa")
                    {
                        Token = ReferenceTokens.SamlToken_Valid_WithRsaKeyValue,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultRsaSecurityKey1,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        }
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_Valid_IssuerSigningKey_JsonWithCertificate")
                    {
                        Token = ReferenceTokens.SamlToken_Valid,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultJsonWebKeyWithCertificate1,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        }
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_Valid_IssuerSigningKey_JsonWithParameters")
                    {
                        Token = ReferenceTokens.SamlToken_Valid,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultJsonWebKeyWithParameters1,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        }
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_Valid_Spaces_Added")
                    {
                        Token = ReferenceTokens.SamlToken_Valid_Spaces_Added,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultX509SigningCreds_2048_RsaSha2_Sha2.Key,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        }
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_AttributeTampered")
                    {
                        ExpectedException = ExpectedException.SecurityTokenInvalidSignatureException("IDX10514:"),
                        Token = ReferenceTokens.SamlToken_AttributeTampered,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultX509SigningCreds_2048_RsaSha2_Sha2.Key,
                        }
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_DigestTampered")
                    {
                        ExpectedException = ExpectedException.SecurityTokenInvalidSignatureException("IDX10514:"),
                        Token = ReferenceTokens.SamlToken_DigestTampered,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultX509SigningCreds_2048_RsaSha2_Sha2.Key,
                        }
                    },
                    // Removed until we have a way of matching a KeyInfo with a SecurityKey.
                    new SamlTheoryData("ReferenceTokens_SamlToken_Valid")
                    {
                        ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException("IDX10204:"),
                        Token = ReferenceTokens.SamlToken_Valid,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultAADSigningKey,
                        }
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_SignatureTampered")
                    {
                        ExpectedException = ExpectedException.SecurityTokenInvalidSignatureException("IDX10514:"),
                        Token = ReferenceTokens.SamlToken_SignatureTampered,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultX509SigningCreds_2048_RsaSha2_Sha2.Key,
                        }
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_Valid_IssuerSigningKeyResolver")
                    {
                        Token = ReferenceTokens.SamlToken_Valid,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            IssuerSigningKeyResolver = (token, securityToken, keyIdentifier, tvp) => { return new List<SecurityKey> { KeyingMaterial.DefaultJsonWebKeyWithCertificate1 }; },
                        }
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_Valid_RequireSignedTokens")
                    {
                        Token = ReferenceTokens.SamlToken_Valid,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            IssuerSigningKey = KeyingMaterial.DefaultX509SigningCreds_2048_RsaSha2_Sha2.Key,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        }
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_Valid_RequireSignedTokensNullSigningKey")
                    {
                        ExpectedException = ExpectedException.SecurityTokenSignatureKeyNotFoundException("IDX10500:"),
                        Token = ReferenceTokens.SamlToken_Valid,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            IssuerSigningKey = null,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        }
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_Valid_DontRequireSignedTokens")
                    {
                        Token = ReferenceTokens.SamlToken_Valid,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = false,
                            IssuerSigningKey = KeyingMaterial.DefaultX509SigningCreds_2048_RsaSha2_Sha2.Key,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        }
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_SignatureMissing_DontRequireSignedTokensNullSigningKey")
                    {
                        Token = ReferenceTokens.SamlToken_SignatureMissing,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = false,
                            IssuerSigningKey = null,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        }
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_NoAudienceRestrictions_NoSignature_RequireAudienceTrue")
                    {
                        Audiences = new List<string>(),
                        Token = ReferenceTokens.SamlToken_NoAudienceRestrictions_NoSignature,
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenException), "IDX11401:"),
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultAADSigningKey,
                            ValidAudience = "spn:fe78e0b4-6fe7-47e6-812c-fb75cee266a4",
                            ValidateLifetime = false,
                            ValidateIssuer = false,
                            RequireSignedTokens = false
                        }
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_NoAudienceRestrictions_NoSignature_RequireAudienceFalse")
                    {
                        Audiences = new List<string>(),
                        Token = ReferenceTokens.SamlToken_NoAudienceRestrictions_NoSignature,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultAADSigningKey,
                            RequireAudience = false,
                            ValidAudience = "spn:fe78e0b4-6fe7-47e6-812c-fb75cee266a4",
                            ValidateLifetime = false,
                            ValidateIssuer = false,
                            RequireSignedTokens = false
                        },
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_NoAudienceRestrictions_NoSignature_RequireAudienceFalseValidateLifetimeTrue")
                    {
                        Audiences = new List<string>(),
                        Token = ReferenceTokens.SamlToken_NoAudienceRestrictions_NoSignature,
                        ExpectedException = new ExpectedException(typeof(SecurityTokenExpiredException), "IDX10223:"),
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultAADSigningKey,
                            RequireAudience = false,
                            ValidAudience = "spn:fe78e0b4-6fe7-47e6-812c-fb75cee266a4",
                            ValidateLifetime = true,
                            ValidateIssuer = false,
                            RequireSignedTokens = false
                        },
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_NoConditions_NoSignature_RequireAudienceTrue")
                    {
                        Audiences = new List<string>(),
                        Token = ReferenceTokens.SamlToken_NoConditions_NoSignature,
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenException), "IDX11401:"),
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultAADSigningKey,
                            ValidAudience = "spn:fe78e0b4-6fe7-47e6-812c-fb75cee266a4",
                            ValidateLifetime = false,
                            ValidateIssuer = false,
                            RequireSignedTokens = false
                        }
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_NoConditions_NoSignature_RequireAudienceFalse")
                    {
                        Audiences = new List<string>(),
                        Token = ReferenceTokens.SamlToken_NoConditions_NoSignature,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultAADSigningKey,
                            RequireAudience = false,
                            ValidAudience = "spn:fe78e0b4-6fe7-47e6-812c-fb75cee266a4",
                            ValidateLifetime = false,
                            ValidateIssuer = false,
                            RequireSignedTokens = false
                        },
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_Valid_NotTryAllIssuerSigningKeys")
                    {
                        ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException("IDX10204:"),
                        Token = ReferenceTokens.SamlToken_Valid,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultAADSigningKey,
                            TryAllIssuerSigningKeys = false
                        }
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_Valid_SpecifyAlgorithm_AlgorithmInList")
                    {
                        Token = ReferenceTokens.SamlToken_Valid_WithRsaKeyValue,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultRsaSecurityKey1,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            ValidAlgorithms = new List<string> { SecurityAlgorithms.RsaSha256Signature }
                        }
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_Valid_SpecifyAlgorithm_EmptyList")
                    {
                        Token = ReferenceTokens.SamlToken_Valid_WithRsaKeyValue,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultRsaSecurityKey1,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            ValidAlgorithms = new List<string>(),
                        }
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_Valid_SpecifyAlgorithm_AlgorithnNotList")
                    {
                        Token = ReferenceTokens.SamlToken_Valid_WithRsaKeyValue,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultRsaSecurityKey1,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            ValidAlgorithms = new List<string> { SecurityAlgorithms.RsaSha512Signature }
                        },
                        ExpectedException = ExpectedException.SecurityTokenSignatureKeyNotFoundException("IDX10512")
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_Valid_SpecifyAlgorithm_AlgorithmValidationFails")
                    {
                        Token = ReferenceTokens.SamlToken_Valid_WithRsaKeyValue,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultRsaSecurityKey1,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            AlgorithmValidator = ValidationDelegates.AlgorithmValidatorBuilder(false)
                        },
                        ExpectedException = ExpectedException.SecurityTokenSignatureKeyNotFoundException("IDX10512")
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_Valid_SpecifyAlgorithm_AlgorithmValidationValidates")
                    {
                        Token = ReferenceTokens.SamlToken_Valid_WithRsaKeyValue,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultRsaSecurityKey1,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            AlgorithmValidator = ValidationDelegates.AlgorithmValidatorBuilder(true)
                        },
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_Valid_WithNoKeyInfo_NullSigningKey")
                    {
                        Token = ReferenceTokens.SamlToken_Valid_WithNoKeyInfo,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            IssuerSigningKey = null,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        },
                        ExpectedException = ExpectedException.SecurityTokenSignatureKeyNotFoundException("IDX10500:")
                    },
                    new SamlTheoryData("ReferenceTokens_SamlToken_Valid_WithNoKeyInfo_NotNullSigningKey")
                    {
                        Token = ReferenceTokens.SamlToken_Valid_WithNoKeyInfo,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            IssuerSigningKey = KeyingMaterial.DefaultAADSigningKey,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        },
                        ExpectedException = ExpectedException.SecurityTokenSignatureKeyNotFoundException("IDX10512:")
                    },
                };
            }
        }

        [Theory, MemberData(nameof(WriteTokenTheoryData))]
        public void WriteToken(SamlTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.WriteToken", theoryData);
            context.PropertiesToIgnoreWhenComparing = new Dictionary<Type, List<string>>
            {
                { typeof(SamlAssertion), new List<string> { "IssueInstant", "InclusiveNamespacesPrefixList", "Signature", "SigningCredentials", "CanonicalString" } },
                { typeof(SamlSecurityToken), new List<string> { "SigningKey" } },
            };

            try
            {
                var token = theoryData.Handler.WriteToken(theoryData.SecurityToken);
                theoryData.Handler.ValidateToken(token, theoryData.ValidationParameters, out SecurityToken validatedToken);
                IdentityComparer.AreEqual(validatedToken, theoryData.SecurityToken, context);
                if (!string.IsNullOrEmpty(theoryData.InclusiveNamespacesPrefixList))
                {
                    if (!string.Equals(theoryData.InclusiveNamespacesPrefixList, (theoryData.SecurityToken as SamlSecurityToken).Assertion.InclusiveNamespacesPrefixList))
                        context.Diffs.Add("!string.Equals(theoryData.InclusivePrefixList, (theoryData.SecurityToken as SamlSecurityToken).Assertion.InclusivePrefixList)");

                    if (!string.Equals(theoryData.InclusiveNamespacesPrefixList, (validatedToken as SamlSecurityToken).Assertion.Signature.SignedInfo.References[0].CanonicalizingTransfrom.InclusiveNamespacesPrefixList))
                        context.Diffs.Add("!string.Equals(theoryData.InclusivePrefixList, (validatedToken as SamlSecurityToken).Assertion.Signature.SignedInfo.References[0].CanonicalizingTransfrom.InclusivePrefixList))");
                }

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SamlTheoryData> WriteTokenTheoryData
        {
            get
            {
                var key = KeyingMaterial.X509SecurityKeySelfSigned2048_SHA256;
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Audience = Default.Audience,
                    NotBefore = Default.NotBefore,
                    Expires = Default.Expires,
                    Issuer = Default.Issuer,
                    SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.Sha256Digest),
                    Subject = new CaseSensitiveClaimsIdentity(Default.SamlClaims),
                };

                var validationParameters = new TokenValidationParameters
                {
                    AuthenticationType = "Federation",
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = false,
                    IssuerSigningKey = key
                };

                var tokenHandler = new SamlSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor) as SamlSecurityToken;
                token.Assertion.InclusiveNamespacesPrefixList = "#default saml ds xml";

                // uncomment to view exception displayed to user
                // ExpectedException.DefaultVerbose = true;
                var theoryData = new TheoryData<SamlTheoryData>();

                theoryData.Add(new SamlTheoryData
                {
                    ExpectedException = ExpectedException.ArgumentNullException("token"),
                    First = true,
                    SecurityToken = null,
                    TestId = nameof(ReferenceSaml.NullToken),
                });

                theoryData.Add(new SamlTheoryData
                {
                    ExpectedException = ExpectedException.ArgumentException("IDX11400:"),
                    SecurityToken = new JwtSecurityToken(Default.AsymmetricJwt),
                    TestId = nameof(ReferenceSaml.JwtToken),
                });

                theoryData.Add(new SamlTheoryData
                {
                    InclusiveNamespacesPrefixList = "#default saml ds xml",
                    SecurityToken = token,
                    TestId = "WithInclusivePrefixList",
                    ValidationParameters = validationParameters
                });

                theoryData.Add(new SamlTheoryData
                {
                    SecurityToken = tokenHandler.CreateToken(tokenDescriptor),
                    TestId = "WithoutInclusivePrefixList",
                    ValidationParameters = validationParameters
                });

                validationParameters = new TokenValidationParameters
                {
                    AuthenticationType = "Federation",
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKeyValidator = ValidationDelegates.IssuerSecurityKeyValidatorThrows,
                    IssuerSigningKey = key
                };

                theoryData.Add(new SamlTheoryData
                {
                    ExpectedException = new ExpectedException(typeof(SecurityTokenInvalidSigningKeyException)),
                    SecurityToken = tokenHandler.CreateToken(tokenDescriptor),
                    TestId = nameof(ValidationDelegates.IssuerSecurityKeyValidatorThrows),
                    ValidationParameters = validationParameters
                });

                validationParameters = new TokenValidationParameters
                {
                    AuthenticationType = "Federation",
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = false,
                    IssuerSigningKeyValidator = ValidationDelegates.IssuerSecurityKeyValidatorThrows,
                    IssuerSigningKey = key
                };

                theoryData.Add(new SamlTheoryData
                {
                    ExpectedException = new ExpectedException(typeof(SecurityTokenInvalidSigningKeyException)),
                    SecurityToken = tokenHandler.CreateToken(tokenDescriptor),
                    TestId = nameof(ValidationDelegates.IssuerSecurityKeyValidatorThrows) + "-false",
                    ValidationParameters = validationParameters
                });

                validationParameters = new TokenValidationParameters
                {
                    AuthenticationType = "Federation",
                    ValidateAudience = true,
                    ValidateIssuer = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = false,
                    AudienceValidator = ValidationDelegates.AudienceValidatorThrows,
                    IssuerSigningKey = key
                };

                theoryData.Add(new SamlTheoryData
                {
                    ExpectedException = new ExpectedException(typeof(SecurityTokenInvalidAudienceException)),
                    SecurityToken = tokenHandler.CreateToken(tokenDescriptor),
                    TestId = nameof(ValidationDelegates.AudienceValidatorThrows),
                    ValidationParameters = validationParameters
                });

                validationParameters = new TokenValidationParameters
                {
                    AuthenticationType = "Federation",
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = false,
                    AudienceValidator = ValidationDelegates.AudienceValidatorThrows,
                    IssuerSigningKey = key
                };

                theoryData.Add(new SamlTheoryData
                {
                    ExpectedException = new ExpectedException(typeof(SecurityTokenInvalidAudienceException)),
                    SecurityToken = tokenHandler.CreateToken(tokenDescriptor),
                    TestId = nameof(ValidationDelegates.AudienceValidatorThrows) + "-false",
                    ValidationParameters = validationParameters
                });

                return theoryData;
            }
        }

        [Theory, MemberData(nameof(WriteTokenXmlTheoryData))]
        public void WriteTokenXml(SamlTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.WriteTokenXml", theoryData);
            try
            {
                (theoryData.Handler as SamlSecurityTokenHandler).WriteToken(theoryData.XmlWriter, theoryData.TokenTestSet.SecurityToken);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SamlTheoryData> WriteTokenXmlTheoryData
        {
            get
            {
                // uncomment to view exception displayed to user
                // ExpectedException.DefaultVerbose = true;

                var theoryData = new TheoryData<SamlTheoryData>();
                var memoryStream = new MemoryStream();
                theoryData.Add(new SamlTheoryData
                {
                    ExpectedException = ExpectedException.ArgumentNullException("token"),
                    First = true,
                    TestId = nameof(ReferenceSaml.NullToken),
                    TokenTestSet = ReferenceSaml.NullToken,
                    XmlWriter = XmlDictionaryWriter.CreateTextWriter(memoryStream)
                });

                theoryData.Add(new SamlTheoryData
                {
                    ExpectedException = ExpectedException.ArgumentNullException("writer"),
                    TestId = "Null XmlWriter",
                    TokenTestSet = ReferenceSaml.SamlSecurityTokenValid
                });

                memoryStream = new MemoryStream();
                theoryData.Add(new SamlTheoryData
                {
                    ExpectedException = ExpectedException.ArgumentException("IDX11400:"),
                    MemoryStream = memoryStream,
                    TestId = nameof(ReferenceSaml.JwtToken),
                    TokenTestSet = ReferenceSaml.JwtToken,
                    XmlWriter = XmlDictionaryWriter.CreateTextWriter(memoryStream)
                });

                return theoryData;
            }
        }

        [Theory, MemberData(nameof(CreateSamlTokenUsingTokenDescriptorTheoryData))]
        public void CreateSamlTokenUsingTokenDescriptor(CreateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateSamlTokenUsingTokenDescriptor", theoryData);
            context.PropertiesToIgnoreWhenComparing = new Dictionary<Type, List<string>>
            {
                { typeof(SamlAssertion), new List<string> { "IssueInstant", "InclusiveNamespacesPrefixList", "Signature", "SigningCredentials", "CanonicalString" } },
                { typeof(SamlSecurityToken), new List<string> { "SigningKey" } },
            };
            if (!theoryData.AudiencesForSecurityTokenDescriptor.IsNullOrEmpty())
            {
                foreach (var audience in theoryData.AudiencesForSecurityTokenDescriptor)
                    theoryData.TokenDescriptor.Audiences.Add(audience);
            }

            try
            {
                SecurityToken samlTokenFromSecurityTokenDescriptor = theoryData.SamlSecurityTokenHandler.CreateToken(theoryData.TokenDescriptor) as SamlSecurityToken;
                string tokenFromTokenDescriptor = theoryData.SamlSecurityTokenHandler.WriteToken(samlTokenFromSecurityTokenDescriptor);

                var claimsIdentityFromTokenDescriptor = theoryData.SamlSecurityTokenHandler.ValidateToken(tokenFromTokenDescriptor, theoryData.ValidationParameters, out SecurityToken validatedTokenFromTokenDescriptor).Identity as ClaimsIdentity;
                IdentityComparer.AreEqual(validatedTokenFromTokenDescriptor, samlTokenFromSecurityTokenDescriptor, context);

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateTokenTheoryData> CreateSamlTokenUsingTokenDescriptorTheoryData
        {
            get
            {

                var validationParameters = new TokenValidationParameters
                {
                    AuthenticationType = "Federation",
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = false,
                    IssuerSigningKey = KeyingMaterial.X509SecurityKeySelfSigned2048_SHA256
                };
                var validationParametersWithAudiences = new TokenValidationParameters
                {
                    AuthenticationType = "Federation",
                    ValidateAudience = true,
                    ValidateIssuer = false,
                    ValidateLifetime = false,
                    IssuerSigningKey = KeyingMaterial.X509SecurityKeySelfSigned2048_SHA256,
                    ValidAudiences = Default.Audiences
                };
                var invalidAudience = "http://NotValid.Audience.com";
                var invalidAudiences = new List<string> { invalidAudience, "http://NotValid.Audience2.com" };
                return new TheoryData<CreateTokenTheoryData>
                {
                    new CreateTokenTheoryData
                    {
                        First =true,
                        TestId = "ValidAudiences",
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            NotBefore = Default.NotBefore,
                            Expires = Default.Expires,
                            Issuer = Default.Issuer,
                            SigningCredentials = new SigningCredentials(KeyingMaterial.X509SecurityKeySelfSigned2048_SHA256, SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.Sha256Digest),
                            EncryptingCredentials = null,
                            Claims = Default.SamlClaimsDictionary,
                        },
                        SamlSecurityTokenHandler = new SamlSecurityTokenHandler(),
                        ValidationParameters = validationParametersWithAudiences,
                        AudiencesForSecurityTokenDescriptor = Default.Audiences
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "InvalidAudiences",
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10214:"),
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            NotBefore = Default.NotBefore,
                            Expires = Default.Expires,
                            Issuer = Default.Issuer,
                            SigningCredentials = new SigningCredentials(KeyingMaterial.X509SecurityKeySelfSigned2048_SHA256, SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.Sha256Digest),
                            EncryptingCredentials = null,
                            Claims = Default.SamlClaimsDictionary,
                        },
                        SamlSecurityTokenHandler = new SamlSecurityTokenHandler(),
                        ValidationParameters = validationParametersWithAudiences,
                        AudiencesForSecurityTokenDescriptor = invalidAudiences
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "UsingAudienceAndAudiences_OnlyAudienceValid",
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            Audience = Default.Audience,
                            NotBefore = Default.NotBefore,
                            Expires = Default.Expires,
                            Issuer = Default.Issuer,
                            SigningCredentials = new SigningCredentials(KeyingMaterial.X509SecurityKeySelfSigned2048_SHA256, SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.Sha256Digest),
                            EncryptingCredentials = null,
                            Claims = Default.SamlClaimsDictionary,
                        },
                        SamlSecurityTokenHandler = new SamlSecurityTokenHandler(),
                        ValidationParameters = validationParametersWithAudiences,
                        AudiencesForSecurityTokenDescriptor = [invalidAudience]
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "UsingAudienceAndAudiences_OnlyAudiencesValid",
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            Audience = invalidAudience,
                            NotBefore = Default.NotBefore,
                            Expires = Default.Expires,
                            Issuer = Default.Issuer,
                            SigningCredentials = new SigningCredentials(KeyingMaterial.X509SecurityKeySelfSigned2048_SHA256, SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.Sha256Digest),
                            EncryptingCredentials = null,
                            Claims = Default.SamlClaimsDictionary,
                        },
                        SamlSecurityTokenHandler = new SamlSecurityTokenHandler(),
                        ValidationParameters = validationParametersWithAudiences,
                        AudiencesForSecurityTokenDescriptor = Default.Audiences
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "NotSupportedClaimValue",
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            Audience = Default.Audience,
                            NotBefore = Default.NotBefore,
                            Expires = Default.Expires,
                            Issuer = Default.Issuer,
                            SigningCredentials = new SigningCredentials(KeyingMaterial.X509SecurityKeySelfSigned2048_SHA256, SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.Sha256Digest),
                            EncryptingCredentials = null,
                            Claims = new Dictionary<string, object>()
                            {
                                { "https://www.listinlist.com", new List<object>{ new List<object> { "bob", new SecurityTokenDescriptor(), 12, 1.45 }, new List<object> { "bob", new SecurityTokenDescriptor(), 12, 1.45 } } },
                            },
                        },
                        SamlSecurityTokenHandler = new SamlSecurityTokenHandler(),
                        ValidationParameters = validationParameters,
                        ExpectedException = ExpectedException.NotSupportedException("IDX10105:")
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "NoSubjectClaims",
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            Audience = Default.Audience,
                            NotBefore = Default.NotBefore,
                            Expires = Default.Expires,
                            Issuer = Default.Issuer,
                            SigningCredentials = new SigningCredentials(KeyingMaterial.X509SecurityKeySelfSigned2048_SHA256, SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.Sha256Digest),
                            EncryptingCredentials = null,
                            Claims = Default.SamlClaimsDictionary,
                        },
                        SamlSecurityTokenHandler = new SamlSecurityTokenHandler(),
                        ValidationParameters = validationParameters
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "OnlySubjectClaims",
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            Audience = Default.Audience,
                            NotBefore = Default.NotBefore,
                            Expires = Default.Expires,
                            Issuer = Default.Issuer,
                            SigningCredentials = new SigningCredentials(KeyingMaterial.X509SecurityKeySelfSigned2048_SHA256, SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.Sha256Digest),
                            EncryptingCredentials = null,
                            Subject = new CaseSensitiveClaimsIdentity(Default.SamlClaims)
                        },
                        SamlSecurityTokenHandler = new SamlSecurityTokenHandler(),
                        ValidationParameters = validationParameters
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "BothIdenticalClaims",
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            Audience = Default.Audience,
                            NotBefore = Default.NotBefore,
                            Expires = Default.Expires,
                            Issuer = Default.Issuer,
                            SigningCredentials = new SigningCredentials(KeyingMaterial.X509SecurityKeySelfSigned2048_SHA256, SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.Sha256Digest),
                            EncryptingCredentials = null,
                            Claims = Default.SamlClaimsDictionary,
                            Subject = new CaseSensitiveClaimsIdentity(Default.SamlClaims)
                        },
                        SamlSecurityTokenHandler = new SamlSecurityTokenHandler(),
                        ValidationParameters = validationParameters
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "MoreDictionaryClaims",
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            Audience = Default.Audience,
                            NotBefore = Default.NotBefore,
                            Expires = Default.Expires,
                            Issuer = Default.Issuer,
                            SigningCredentials = new SigningCredentials(KeyingMaterial.X509SecurityKeySelfSigned2048_SHA256, SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.Sha256Digest),
                            EncryptingCredentials = null,
                            Claims = Default.SamlClaimsDictionary,
                            Subject = new CaseSensitiveClaimsIdentity
                            (
                                new List<Claim>
                                {
                                    new Claim(ClaimTypes.Country, "USA", ClaimValueTypes.String, Default.Issuer, Default.OriginalIssuer),
                                    new Claim(ClaimTypes.NameIdentifier, "Bob", ClaimValueTypes.String, Default.Issuer, Default.OriginalIssuer),
                                    new Claim(ClaimTypes.Email, "Bob@contoso.com", ClaimValueTypes.String, Default.Issuer, Default.OriginalIssuer)
                                }
                            )
                        },
                        SamlSecurityTokenHandler = new SamlSecurityTokenHandler(),
                        ValidationParameters = validationParameters
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "MoreSubjectClaims",
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            Audience = Default.Audience,
                            NotBefore = Default.NotBefore,
                            Expires = Default.Expires,
                            Issuer = Default.Issuer,
                            SigningCredentials = new SigningCredentials(KeyingMaterial.X509SecurityKeySelfSigned2048_SHA256, SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.Sha256Digest),
                            EncryptingCredentials = null,
                            Claims = new Dictionary<string, object>()
                            {
                                { ClaimTypes.Email, "Bob@contoso.com" },
                                { ClaimTypes.GivenName, "Bob" },
                                { ClaimTypes.Role, "HR" }
                            },
                            Subject = new CaseSensitiveClaimsIdentity(Default.SamlClaims)
                        },
                        SamlSecurityTokenHandler = new SamlSecurityTokenHandler(),
                        ValidationParameters = validationParameters
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "RepeatingClaimTypes",
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            Audience = Default.Audience,
                            NotBefore = Default.NotBefore,
                            Expires = Default.Expires,
                            Issuer = Default.Issuer,
                            SigningCredentials = new SigningCredentials(KeyingMaterial.X509SecurityKeySelfSigned2048_SHA256, SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.Sha256Digest),
                            EncryptingCredentials = null,
                            Claims = new Dictionary<string, object>()
                            {
                                { ClaimTypes.Email, "Alice@contoso.com" },
                                { ClaimTypes.GivenName, "Alice" },
                                { ClaimTypes.Role, "HR" }
                            },
                            Subject = new CaseSensitiveClaimsIdentity
                            (
                                new List<Claim>
                                {
                                    new Claim(ClaimTypes.Email, "Bob@contoso.com", ClaimValueTypes.String, Default.Issuer, Default.OriginalIssuer),
                                    new Claim(ClaimTypes.GivenName, "Bob", ClaimValueTypes.String, Default.Issuer, Default.OriginalIssuer),
                                    new Claim(ClaimTypes.Country, "India", ClaimValueTypes.String, Default.Issuer, Default.OriginalIssuer)
                                }
                            )
                        },
                        SamlSecurityTokenHandler = new SamlSecurityTokenHandler(),
                        ValidationParameters = validationParameters
                    },
                };
            }
        }

        [Theory, MemberData(nameof(SecurityKeyNotFoundExceptionTestTheoryData))]
        public void SamlSecurityKeyNotFoundExceptionTest(CreateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.SecurityKeyNotFoundExceptionTest", theoryData);

            try
            {
                var handler = new SamlSecurityTokenHandler();
                var token = handler.CreateToken(theoryData.TokenDescriptor);
                string samlToken = handler.WriteToken(token);
                handler.ValidateToken(samlToken, theoryData.ValidationParameters, out var validationResult);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateTokenTheoryData> SecurityKeyNotFoundExceptionTestTheoryData()
        {
            return SamlTestData.SecurityKeyNotFoundExceptionTestTheoryData();
        }

        private class SamlSecurityTokenHandlerPublic : SamlSecurityTokenHandler
        {
            public IEnumerable<ClaimsIdentity> CreateClaimsIdentitiesPublic(SamlSecurityToken samlToken, string issuer, TokenValidationParameters validationParameters)
            {
                return base.CreateClaimsIdentities(samlToken, issuer, validationParameters);
            }

            public void ValidateAudiencePublic(IEnumerable<string> audiences, SecurityToken token, TokenValidationParameters validationParameters)
            {
                base.ValidateAudience(audiences, token, validationParameters);
            }

            public string ValidateIssuerPublic(string issuer, SecurityToken token, TokenValidationParameters validationParameters)
            {
                return base.ValidateIssuer(issuer, token, validationParameters);
            }
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
