//------------------------------------------------------------------------------
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens.Saml.Tests;
using Microsoft.IdentityModel.Xml;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Saml2.Tests
{
    public class Saml2SecurityTokenHandlerTests
    {
        [Fact]
        public void Constructors()
        {
            var saml2SecurityTokenHandler = new Saml2SecurityTokenHandler();
        }

        [Fact]
        public void Defaults()
        {
            var samlSecurityTokenHandler = new Saml2SecurityTokenHandler();
            Assert.True(samlSecurityTokenHandler.MaximumTokenSizeInBytes == TokenValidationParameters.DefaultMaximumTokenSizeInBytes, "MaximumTokenSizeInBytes");
        }

        [Fact]
        public void GetSets()
        {
            var samlSecurityTokenHandler = new Saml2SecurityTokenHandler();
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

            samlSecurityTokenHandler = new Saml2SecurityTokenHandler();
            TestUtilities.SetGet(samlSecurityTokenHandler, "MaximumTokenSizeInBytes", (object)0, ExpectedException.ArgumentOutOfRangeException("IDX10101:"), context);
            TestUtilities.SetGet(samlSecurityTokenHandler, "MaximumTokenSizeInBytes", (object)1, ExpectedException.NoExceptionExpected, context);
            TestUtilities.SetGet(samlSecurityTokenHandler, "Serializer", null, ExpectedException.ArgumentNullException(), context);

            TestUtilities.AssertFailIfErrors("Saml2SecurityTokenHandlerTests_GetSets", context.Errors);
        }

        [Theory, MemberData(nameof(CanReadTokenTheoryData))]
        public void CanReadToken(Saml2TheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.CanReadToken", theoryData);
            var context = new CompareContext($"{this}.CanReadToken, {theoryData}");
            try
            {
                // TODO - need to pass actual Saml2Token
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

        public static TheoryData<Saml2TheoryData> CanReadTokenTheoryData
        {
            get => new TheoryData<Saml2TheoryData>
            {
                new Saml2TheoryData
                {
                    CanRead = false,
                    First = true,
                    Handler = new Saml2SecurityTokenHandler(),
                    TestId = "Null Token",
                    Token = null
                },
                new Saml2TheoryData
                {
                    CanRead = false,
                    Handler = new Saml2SecurityTokenHandler(),
                    TestId = "DefaultMaximumTokenSizeInBytes + 1",
                    Token = new string('S', TokenValidationParameters.DefaultMaximumTokenSizeInBytes + 2)
                },
                new Saml2TheoryData
                {
                    CanRead = true,
                    Handler = new Saml2SecurityTokenHandler(),
                    TestId = nameof(ReferenceTokens.Saml2Token_Valid),
                    Token = ReferenceTokens.Saml2Token_Valid
                },
                new Saml2TheoryData
                {
                    CanRead = false,
                    Handler = new Saml2SecurityTokenHandler(),
                    TestId = nameof(ReferenceTokens.SamlToken_Valid),
                    Token = ReferenceTokens.SamlToken_Valid
                }
            };
        }

        [Theory, MemberData(nameof(ConsolidateAttributesTheoryData))]
        public void ConsolidateAttributes(Saml2TheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ConsolidateAttributes", theoryData);
            var context = new CompareContext($"{this}.ConsolidateAttributes, {theoryData}");
            var handler = theoryData.Handler as Saml2SecurityTokenHandlerPublic;
            try
            {
                var consolidatedAttributes = handler.ConsolidateAttributesPublic(theoryData.Attributes);
                theoryData.ExpectedException.ProcessNoException(context);
                IdentityComparer.AreEnumsEqual(consolidatedAttributes, theoryData.ConsolidatedAttributes, context, AreSaml2AttributesEqual);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<Saml2TheoryData> ConsolidateAttributesTheoryData
        {
            get
            {
                var theoryData = new TheoryData<Saml2TheoryData>
                {
                    new Saml2TheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        First = true,
                        TestId = "param attributes null"
                    },
                    new Saml2TheoryData
                    {
                        Attributes = new List<Saml2Attribute>(),
                        ConsolidatedAttributes = new List<Saml2Attribute>(),
                        TestId = "Empty Attribute List"
                    },
                    new Saml2TheoryData
                    {
                        Attributes = new List<Saml2Attribute>
                        {
                            Default.Saml2AttributeSingleValue
                        },
                        ConsolidatedAttributes = new List<Saml2Attribute>
                        {
                            Default.Saml2AttributeSingleValue
                        },
                        TestId = nameof(Default.Saml2AttributeSingleValue)
                    },
                    new Saml2TheoryData
                    {
                        Attributes = new List<Saml2Attribute>
                        {
                            Default.Saml2AttributeSingleValue,
                            Default.Saml2AttributeSingleValue
                        },
                        ConsolidatedAttributes = new List<Saml2Attribute>
                        {
                            Default.Saml2AttributeMultiValue
                        },
                        TestId = nameof(Default.Saml2AttributeMultiValue)
                    }
                };

                var attribute = Default.Saml2AttributeSingleValue;
                attribute.AttributeValueXsiType = Guid.NewGuid().ToString();
                theoryData.Add(CreateAttributeTheoryData(attribute, "AttributeValueXsiType"));

                attribute = Default.Saml2AttributeSingleValue;
                attribute.FriendlyName = Guid.NewGuid().ToString();
                theoryData.Add(CreateAttributeTheoryData(attribute, "FriendlyName"));

                attribute = new Saml2Attribute(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
                theoryData.Add(CreateAttributeTheoryData(attribute, "Name, Value"));

                attribute = Default.Saml2AttributeSingleValue;
                attribute.NameFormat = new Uri(Default.Uri);
                theoryData.Add(CreateAttributeTheoryData(attribute, "NameFormat"));

                attribute = Default.Saml2AttributeSingleValue;
                attribute.OriginalIssuer = NotDefault.OriginalIssuer;
                theoryData.Add(CreateAttributeTheoryData(attribute, "OrginalIssuer"));

                return theoryData;
            }
        }

        private static Saml2TheoryData CreateAttributeTheoryData(Saml2Attribute attribute, string testId)
        {
            return new Saml2TheoryData
            {
                Attributes = new List<Saml2Attribute>
                {
                    Default.Saml2AttributeSingleValue,
                    attribute,
                    Default.Saml2AttributeSingleValue,
                },
                ConsolidatedAttributes = new List<Saml2Attribute>
                {
                    Default.Saml2AttributeMultiValue,
                    attribute
                },
                TestId = testId
            };
        }

        public static bool AreSaml2AttributesEqual(Saml2Attribute attribute1, Saml2Attribute attribute2, CompareContext context)
        {
            var localContext = new CompareContext("AreSaml2AttributesEqual");
            if (!IdentityComparer.ContinueCheckingEquality(attribute1, attribute2, localContext))
                return context.Merge(localContext);

            IdentityComparer.AreStringsEqual(attribute1.AttributeValueXsiType, attribute2.AttributeValueXsiType, localContext);
            IdentityComparer.AreStringsEqual(attribute1.FriendlyName, attribute2.FriendlyName, localContext);
            IdentityComparer.AreStringsEqual(attribute1.Name, attribute2.Name, localContext);
            IdentityComparer.AreStringsEqual(attribute1.NameFormat?.AbsoluteUri, attribute2.NameFormat?.AbsoluteUri, localContext);
            IdentityComparer.AreStringsEqual(attribute1.OriginalIssuer, attribute2.OriginalIssuer, localContext);

            return context.Merge(localContext);
        }

        [Theory, MemberData(nameof(ReadTokenTheoryData))]
        public void ReadToken(Saml2TheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ReadToken", theoryData);
            var context = new CompareContext($"{this}.ReadToken, {theoryData}");
            try
            {
                var token1 = theoryData.Handler.ReadToken(theoryData.Token);
                var token2 = theoryData.Handler.ReadToken(XmlUtilities.CreateXmlReader(theoryData.Token));
                var token3 = theoryData.Handler.ReadToken(XmlUtilities.CreateDictionaryReader(theoryData.Token));
                IdentityComparer.AreEqual(token1, token2, context);
                IdentityComparer.AreEqual(token1, token3, context);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<Saml2TheoryData> ReadTokenTheoryData
        {
            get
            {
                return new TheoryData<Saml2TheoryData>
                {
                    new Saml2TheoryData
                    {
                        ExpectedException = ExpectedException.NoExceptionExpected,
                        First = true,
                        Handler = new Saml2SecurityTokenHandler(),
                        TestId = nameof(ReferenceTokens.Saml2Token_Valid),
                        Token = ReferenceTokens.Saml2Token_Valid
                    },
                    new Saml2TheoryData
                    {
                        ExpectedException = ExpectedException.NoExceptionExpected,
                        Handler = new Saml2SecurityTokenHandler(),
                        TestId = nameof(ReferenceTokens.Saml2Token_InclusiveNamespaces_WithPrefix),
                        Token = ReferenceTokens.Saml2Token_InclusiveNamespaces_WithPrefix
                    },
                    new Saml2TheoryData
                    {
                        ExpectedException = ExpectedException.NoExceptionExpected,
                        Handler = new Saml2SecurityTokenHandler(),
                        TestId = nameof(ReferenceTokens.Saml2Token_InclusiveNamespaces_WithoutPrefix),
                        Token = ReferenceTokens.Saml2Token_InclusiveNamespaces_WithoutPrefix
                    }
                };
            }
        }

        [Theory, MemberData(nameof(RoundTripActorTheoryData))]
        public void RoundTripActor(Saml2TheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.RoundTripActor", theoryData);
            CompareContext context = new CompareContext($"{this}.RoundTripActor, {theoryData}");

            var handler = theoryData.Handler as Saml2SecurityTokenHandlerPublic;
            var actor = handler.CreateActorStringPublic(theoryData.TokenDescriptor.Subject);
        }

        [Theory, MemberData(nameof(WriteTokenTheoryData))]
        public void WriteToken(Saml2TheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.WriteToken", theoryData);
            context.PropertiesToIgnoreWhenComparing = new Dictionary<Type, List<string>>
            {
                { typeof(Saml2Assertion), new List<string> { "IssueInstant", "InclusiveNamespacesPrefixList", "Signature", "SigningCredentials", "CanonicalString" } },
                { typeof(Saml2SecurityToken), new List<string> { "SigningKey" } },
            };

            try
            {
                var token = theoryData.Handler.WriteToken(theoryData.SecurityToken);
                theoryData.Handler.ValidateToken(token, theoryData.ValidationParameters, out SecurityToken validatedToken);
                theoryData.ExpectedException.ProcessNoException(context);
                IdentityComparer.AreEqual(validatedToken, theoryData.SecurityToken, context);
                if (!string.IsNullOrEmpty(theoryData.InclusiveNamespacesPrefixList))
                {
                    if (!string.Equals(theoryData.InclusiveNamespacesPrefixList, (theoryData.SecurityToken as Saml2SecurityToken).Assertion.InclusiveNamespacesPrefixList))
                        context.Diffs.Add("!string.Equals(theoryData.InclusivePrefixList, (theoryData.SecurityToken as Saml2SecurityToken).Assertion.InclusivePrefixList)");

                    if (!string.Equals(theoryData.InclusiveNamespacesPrefixList, (validatedToken as Saml2SecurityToken).Assertion.Signature.SignedInfo.References[0].CanonicalizingTransfrom.InclusiveNamespacesPrefixList))
                        context.Diffs.Add("!string.Equals(theoryData.InclusivePrefixList, (validatedToken as Saml2SecurityToken).Assertion.Signature.SignedInfo.References[0].CanonicalizingTransfrom.InclusivePrefixList))");
                }
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<Saml2TheoryData> WriteTokenTheoryData
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
                    Subject = new ClaimsIdentity(Default.SamlClaims),
                };

                var validationParameters = new TokenValidationParameters
                {
                    AuthenticationType = "Federation",
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = false,
                    IssuerSigningKey = key
                };

                var tokenHandler = new Saml2SecurityTokenHandler();
                var theoryData = new TheoryData<Saml2TheoryData>();

                var token = tokenHandler.CreateToken(tokenDescriptor) as Saml2SecurityToken;
                token.Assertion.InclusiveNamespacesPrefixList = "#default saml ds xml";

                theoryData.Add(new Saml2TheoryData
                {
                    First = true,
                    InclusiveNamespacesPrefixList = "#default saml ds xml",
                    SecurityToken = token,
                    TestId = "WithInclusivePrefixList",
                    ValidationParameters = validationParameters
                });

                theoryData.Add(new Saml2TheoryData
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

                theoryData.Add(new Saml2TheoryData
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

                theoryData.Add(new Saml2TheoryData
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

                theoryData.Add(new Saml2TheoryData
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

                theoryData.Add(new Saml2TheoryData
                {
                    ExpectedException = new ExpectedException(typeof(SecurityTokenInvalidAudienceException)),
                    SecurityToken = tokenHandler.CreateToken(tokenDescriptor),
                    TestId = nameof(ValidationDelegates.AudienceValidatorThrows) + "-false",
                    ValidationParameters = validationParameters
                });

                return theoryData;
            }
        }

        public static TheoryData<Saml2TheoryData> RoundTripActorTheoryData
        {
            get => new TheoryData<Saml2TheoryData>
            {
                new Saml2TheoryData
                {
                    First = true,
                    Handler = new Saml2SecurityTokenHandlerPublic(),
                    TestId = nameof(ClaimSets.DefaultClaimsIdentity),
                    TokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = ClaimSets.DefaultClaimsIdentity
                    }
                }
            };
        }

        // Test checks to make sure that default times are correctly added to the token
        // upon token creation.
        [Fact]
        public void SetDefaultTimesOnTokenCreation()
        {
            TestUtilities.WriteHeader($"{this}.SetDefaultTimesOnTokenCreation");
            var context = new CompareContext();

            var tokenHandler = new Saml2SecurityTokenHandler();
            var descriptorNoTimeValues = new SecurityTokenDescriptor()
            {
                Issuer = Default.Issuer,
                Audience = Default.Audience,
                SigningCredentials = Default.AsymmetricSigningCredentials,
                Subject = new ClaimsIdentity()
            };

            var token = tokenHandler.CreateToken(descriptorNoTimeValues);
            var saml2SecurityToken = token as Saml2SecurityToken;

            Assert.NotEqual(DateTime.MinValue, saml2SecurityToken.ValidFrom);
            Assert.NotEqual(DateTime.MinValue, saml2SecurityToken.ValidTo);
        }

        [Theory, MemberData(nameof(ValidateAudienceTheoryData))]
        public void ValidateAudience(Saml2TheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ValidateAudience", theoryData);
            var context = new CompareContext($"{this}.ValidateAudience, {theoryData}");
            try
            {
                (theoryData.Handler as Saml2SecurityTokenHandlerPublic).ValidateAudiencePublic(theoryData.Audiences, theoryData.SecurityToken, theoryData.ValidationParameters);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<Saml2TheoryData> ValidateAudienceTheoryData
        {
            get
            {
                var tokenTheoryData = new List<TokenTheoryData>();
                var theoryData = new TheoryData<Saml2TheoryData>();

                ValidateTheoryData.AddValidateAudienceTheoryData(tokenTheoryData);
                foreach (var item in tokenTheoryData)
                    theoryData.Add(new Saml2TheoryData(item)
                    {
                        Handler = new Saml2SecurityTokenHandlerPublic()
                    });

                return theoryData;
            }
        }

        [Theory, MemberData(nameof(ValidateIssuerTheoryData))]
        public void ValidateIssuer(Saml2TheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ValidateIssuer", theoryData);
            var context = new CompareContext($"{this}.ValidateAudience, {theoryData}");
            try
            {
                (theoryData.Handler as Saml2SecurityTokenHandlerPublic).ValidateIssuerPublic(theoryData.Issuer, null, theoryData.ValidationParameters);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<Saml2TheoryData> ValidateIssuerTheoryData
        {
            get
            {
                var tokenTheoryData = new List<TokenTheoryData>();
                ValidateTheoryData.AddValidateIssuerTheoryData(tokenTheoryData);

                var theoryData = new TheoryData<Saml2TheoryData>();
                foreach (var item in tokenTheoryData)
                    theoryData.Add(new Saml2TheoryData(item)
                    {
                        Handler = new Saml2SecurityTokenHandlerPublic()
                    });

                return theoryData;
            }
        }

        [Theory, MemberData(nameof(ValidateTokenTheoryData))]
        public void ValidateToken(Saml2TheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ValidateToken", theoryData);
            var context = new CompareContext($"{this}.ValidateToken, {theoryData}");
            try
            {
                theoryData.Handler.ValidateToken(theoryData.Token, theoryData.ValidationParameters, out SecurityToken validatedToken);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<Saml2TheoryData> ValidateTokenTheoryData
        {
            get
            {
                // uncomment to view exception displayed to user
                // ExpectedException.DefaultVerbose = true;

                return new TheoryData<Saml2TheoryData>
                {
                    new Saml2TheoryData
                    {
                        Handler = new Saml2SecurityTokenHandler(),
                        TestId = nameof(ReferenceTokens.Saml2Token_Valid),
                        Token = ReferenceTokens.Saml2Token_Valid,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultAADSigningKey,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        }
                    },
                    new Saml2TheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                        First = true,
                        Handler = new Saml2SecurityTokenHandler(),
                        TestId = "Null-SecurityToken",
                        Token = null,
                        ValidationParameters = new TokenValidationParameters()
                    },
                    new Saml2TheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                        Handler = new Saml2SecurityTokenHandler(),
                        TestId = "NULL-TokenValidationParameters",
                        Token = "s",
                        ValidationParameters = null,
                    },
                    new Saml2TheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentException("IDX10209:"),
                        Handler = new Saml2SecurityTokenHandler { MaximumTokenSizeInBytes = 1 },
                        TestId = "SecurityTokenTooLarge",
                        Token = "ss",
                        ValidationParameters = new TokenValidationParameters(),
                    },
                    new Saml2TheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(Saml2SecurityTokenReadException), "IDX13106:"),
                        Handler = new Saml2SecurityTokenHandler(),
                        TestId = nameof(ReferenceTokens.Saml2Token_MissingVersion),
                        Token = ReferenceTokens.Saml2Token_MissingVersion,
                        ValidationParameters = new TokenValidationParameters(),
                    },
                    new Saml2TheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(Saml2SecurityTokenReadException), "IDX13137:"),
                        Handler = new Saml2SecurityTokenHandler(),
                        TestId = nameof(ReferenceTokens.Saml2Token_VersionNotV20),
                        Token = ReferenceTokens.Saml2Token_VersionNotV20,
                        ValidationParameters = new TokenValidationParameters(),
                    },
                    new Saml2TheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(Saml2SecurityTokenReadException), "IDX13106:"),
                        Handler = new Saml2SecurityTokenHandler(),
                        TestId = nameof(ReferenceTokens.Saml2Token_IdMissing),
                        Token = ReferenceTokens.Saml2Token_IdMissing,
                        ValidationParameters = new TokenValidationParameters(),
                    },
                    new Saml2TheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(Saml2SecurityTokenReadException), "IDX13106:"),
                        Handler = new Saml2SecurityTokenHandler(),
                        TestId = nameof(ReferenceTokens.Saml2Token_IssueInstantMissing),
                        Token = ReferenceTokens.Saml2Token_IssueInstantMissing,
                        ValidationParameters = new TokenValidationParameters(),
                    },
                    new Saml2TheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(Saml2SecurityTokenReadException), "IDX13102:", typeof(FormatException)),
                        Handler = new Saml2SecurityTokenHandler(),
                        TestId = nameof(ReferenceTokens.Saml2Token_IssueInstantFormatError),
                        Token = ReferenceTokens.Saml2Token_IssueInstantFormatError,
                        ValidationParameters = new TokenValidationParameters(),
                    },
                    new Saml2TheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(Saml2SecurityTokenReadException), "IDX13102:", typeof(XmlReadException)),
                        Handler = new Saml2SecurityTokenHandler(),
                        TestId = nameof(ReferenceTokens.Saml2Token_IssuerMissing),
                        Token = ReferenceTokens.Saml2Token_IssuerMissing,
                        ValidationParameters = new TokenValidationParameters(),
                    },
                    new Saml2TheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(Saml2SecurityTokenReadException), "IDX13108:"),
                        Handler = new Saml2SecurityTokenHandler(),
                        TestId = nameof(ReferenceTokens.Saml2Token_NoSubjectNoStatements),
                        Token = ReferenceTokens.Saml2Token_NoSubjectNoStatements,
                        ValidationParameters = new TokenValidationParameters(),
                    },
                    new Saml2TheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(Saml2SecurityTokenReadException), "IDX13138:"),
                        Handler = new Saml2SecurityTokenHandler(),
                        TestId = nameof(ReferenceTokens.Saml2Token_NoAttributes),
                        Token = ReferenceTokens.Saml2Token_NoAttributes,
                        ValidationParameters = new TokenValidationParameters(),
                    },
                    new Saml2TheoryData
                    {
                        Handler = new Saml2SecurityTokenHandler(),
                        TestId = $"{nameof(ReferenceTokens.Saml2Token_Valid)} IssuerSigningKey set",
                        Token = ReferenceTokens.Saml2Token_Valid,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultAADSigningKey,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        }
                    },
                    new Saml2TheoryData
                    {
                        Handler = new Saml2SecurityTokenHandler(),
                        TestId = nameof(ReferenceTokens.Saml2Token_Valid_Spaces_Added),
                        Token = ReferenceTokens.Saml2Token_Valid_Spaces_Added,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultAADSigningKey,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        }
                    },
                    new Saml2TheoryData
                    {
                        ExpectedException = ExpectedException.SecurityTokenInvalidSignatureException("IDX10514:"),
                        Handler = new Saml2SecurityTokenHandler(),
                        TestId = nameof(ReferenceTokens.Saml2Token_Formated),
                        Token = ReferenceTokens.Saml2Token_Formated,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultAADSigningKey,
                        }
                    },
                    new Saml2TheoryData
                    {
                        Handler = new Saml2SecurityTokenHandler(),
                        TestId = $"{nameof(ReferenceTokens.Saml2Token_Valid)} IssuerSigningKey Rsa",
                        Token = ReferenceTokens.Saml2Token_Valid_WithRsaKeyValue,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultRsaSecurityKey2,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        }
                    },
                    new Saml2TheoryData
                    {
                        Handler = new Saml2SecurityTokenHandler(),
                        TestId = $"{nameof(ReferenceTokens.Saml2Token_Valid)} IssuerSigningKey JsonWithCertificate",
                        Token = ReferenceTokens.Saml2Token_Valid,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultJsonWebKeyWithCertificate2,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        }
                    },
                    new Saml2TheoryData
                    {
                        Handler = new Saml2SecurityTokenHandler(),
                        TestId = $"{nameof(ReferenceTokens.Saml2Token_Valid)} IssuerSigningKey JsonWithParameters",
                        Token = ReferenceTokens.Saml2Token_Valid,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultJsonWebKeyWithParameters2,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        }
                    },
                    new Saml2TheoryData
                    {
                        ExpectedException = ExpectedException.SecurityTokenInvalidSignatureException("IDX10514:"),
                        Handler = new Saml2SecurityTokenHandler(),
                        TestId = nameof(ReferenceTokens.Saml2Token_AttributeTampered),
                        Token = ReferenceTokens.Saml2Token_AttributeTampered,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultAADSigningKey,
                        }
                    },
                    new Saml2TheoryData
                    {
                        ExpectedException = ExpectedException.SecurityTokenInvalidSignatureException("IDX10514:"),
                        Handler = new Saml2SecurityTokenHandler(),
                        TestId = nameof(ReferenceTokens.Saml2Token_DigestTampered),
                        Token = ReferenceTokens.Saml2Token_DigestTampered,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultAADSigningKey,
                        }
                    },
                    // Removed until we have a way of matching a SecurityKey with a KeyInfo.
                    new Saml2TheoryData
                    {
                        ExpectedException = ExpectedException.SecurityTokenUnableToValidateException("IDX10515:"),
                        Handler = new Saml2SecurityTokenHandler(),
                        TestId = nameof(ReferenceTokens.Saml2Token_AttributeTampered_NoKeyMatch),
                        Token = ReferenceTokens.Saml2Token_AttributeTampered_NoKeyMatch,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultAADSigningKey,
                        }
                    },
                    new Saml2TheoryData
                    {
                        ExpectedException = ExpectedException.SecurityTokenInvalidSignatureException("IDX10514:"),
                        Handler = new Saml2SecurityTokenHandler(),
                        TestId = nameof(ReferenceTokens.Saml2Token_SignatureTampered),
                        Token = ReferenceTokens.Saml2Token_SignatureTampered,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultAADSigningKey,
                        }
                    },
                    new Saml2TheoryData
                    {
                        Handler = new Saml2SecurityTokenHandler(),
                        TestId = nameof(ReferenceTokens.Saml2Token_SignatureMissing),
                        Token = ReferenceTokens.Saml2Token_SignatureMissing,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultAADSigningKey,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            RequireSignedTokens = false,
                        }
                    },
                    new Saml2TheoryData
                    {
                        Handler = new Saml2SecurityTokenHandler(),
                        TestId = $"{nameof(ReferenceTokens.Saml2Token_Valid)}IssuerSigningKeyResolver",
                        Token = ReferenceTokens.Saml2Token_Valid,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            IssuerSigningKeyResolver = (token, securityToken, keyIdentifier, tvp) => { return new List<SecurityKey> { KeyingMaterial.DefaultAADSigningKey }; },
                        }
                    },
                    new Saml2TheoryData
                    {
                        Handler = new Saml2SecurityTokenHandler(),
                        TestId = $"{nameof(ReferenceTokens.Saml2Token_Valid)}RequireSignedTokens",
                        Token = ReferenceTokens.Saml2Token_Valid,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            IssuerSigningKey = KeyingMaterial.DefaultJsonWebKeyWithCertificate2,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        }
                    },
                    new Saml2TheoryData
                    {
                        ExpectedException = ExpectedException.SecurityTokenSignatureKeyNotFoundException("IDX10513:"),
                        Handler = new Saml2SecurityTokenHandler(),
                        TestId = $"{nameof(ReferenceTokens.Saml2Token_Valid)}RequireSignedTokensNullSigningKey",
                        Token = ReferenceTokens.Saml2Token_Valid,
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
                    new Saml2TheoryData
                    {
                        Handler = new Saml2SecurityTokenHandler(),
                        TestId = $"{nameof(ReferenceTokens.Saml2Token_Valid)}DontRequireSignedTokens",
                        Token = ReferenceTokens.Saml2Token_Valid,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = false,
                            IssuerSigningKey = KeyingMaterial.DefaultJsonWebKeyWithCertificate2,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        }
                    },
                    new Saml2TheoryData
                    {
                        Handler = new Saml2SecurityTokenHandler(),
                        TestId = $"{nameof(ReferenceTokens.Saml2Token_SignatureMissing)}DontRequireSignedTokensNullSigningKey",
                        Token = ReferenceTokens.Saml2Token_SignatureMissing,
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
                     new Saml2TheoryData
                     {
                        Audiences = new List<string>(),
                        Token = ReferenceTokens.Saml2Token_NoAudienceRestrictions_NoSignature,
                        ExpectedException = new ExpectedException(typeof(Saml2SecurityTokenException), "IDX13002:"),
                        TestId = $"{nameof(ReferenceTokens.Saml2Token_NoAudienceRestrictions_NoSignature)}RequireAudienceTrue",
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultAADSigningKey,
                            ValidAudience = "spn:fe78e0b4-6fe7-47e6-812c-fb75cee266a4",
                            ValidateLifetime = false,
                            ValidateIssuer = false,
                            RequireSignedTokens = false
                        }
                    },
                    new Saml2TheoryData
                    {
                        Audiences = new List<string>(),
                        Token = ReferenceTokens.Saml2Token_NoAudienceRestrictions_NoSignature,
                        TestId = $"{nameof(ReferenceTokens.Saml2Token_NoAudienceRestrictions_NoSignature)}RequireAudienceFalse",
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
                    new Saml2TheoryData
                    {
                        Audiences = new List<string>(),
                        Token = ReferenceTokens.Saml2Token_NoAudienceRestrictions_NoSignature,
                        ExpectedException = new ExpectedException(typeof(SecurityTokenExpiredException), "IDX10223:"),
                        TestId = $"{nameof(ReferenceTokens.Saml2Token_NoAudienceRestrictions_NoSignature)}RequireAudienceFalseValidateLifetimeTrue",
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
                    new Saml2TheoryData
                    {
                        Audiences = new List<string>(),
                        Token = ReferenceTokens.Saml2Token_NoConditions_NoSignature,
                        ExpectedException = new ExpectedException(typeof(Saml2SecurityTokenException), "IDX13002:"),
                        TestId = $"{nameof(ReferenceTokens.Saml2Token_NoConditions_NoSignature)}RequireAudienceTrue",
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultAADSigningKey,
                            ValidAudience = "spn:fe78e0b4-6fe7-47e6-812c-fb75cee266a4",
                            ValidateLifetime = false,
                            ValidateIssuer = false,
                            RequireSignedTokens = false
                        }
                    },
                    new Saml2TheoryData
                    {
                        Audiences = new List<string>(),
                        Token = ReferenceTokens.Saml2Token_NoConditions_NoSignature,
                        TestId = $"{nameof(ReferenceTokens.Saml2Token_NoConditions_NoSignature)}RequireAudienceFalse",
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
                    new Saml2TheoryData
                    {
                        ExpectedException = ExpectedException.SecurityTokenUnableToValidateException("IDX10515:"),
                        Handler = new Saml2SecurityTokenHandler(),
                        TestId = $"{nameof(ReferenceTokens.Saml2Token_AttributeTampered_NoKeyMatch)}NotTryAllIssuerSigningKeys",
                        Token = ReferenceTokens.Saml2Token_AttributeTampered_NoKeyMatch,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultAADSigningKey,
                            TryAllIssuerSigningKeys = false
                        }
                    },
                    new Saml2TheoryData
                    {
                        TestId = $"{nameof(ReferenceTokens.Saml2Token_Valid)}_SpecifyAlgorithm_AlgorithnInList",
                        Token = ReferenceTokens.Saml2Token_Valid,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultAADSigningKey,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            ValidAlgorithms = new List<string> { SecurityAlgorithms.RsaSha256Signature }
                        }
                    },
                    new Saml2TheoryData
                    {
                        TestId = $"{nameof(ReferenceTokens.Saml2Token_Valid)}_SpecifyAlgorithm_EmptyList",
                        Token = ReferenceTokens.Saml2Token_Valid,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultAADSigningKey,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            ValidAlgorithms = new List<string>()
                        }
                    },
                    new Saml2TheoryData
                    {
                        TestId = $"{nameof(ReferenceTokens.Saml2Token_Valid)}_SpecifyAlgorithm_AlgorithnNotList",
                        Token = ReferenceTokens.Saml2Token_Valid,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultAADSigningKey,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            ValidAlgorithms = new List<string> { SecurityAlgorithms.RsaSha512Signature }
                        },
                        ExpectedException = ExpectedException.SecurityTokenInvalidSignatureException("IDX10514")
                    },
                    new Saml2TheoryData
                    {
                        TestId = $"{nameof(ReferenceTokens.Saml2Token_Valid)}_SpecifyAlgorithm_AlgorithmValidationFails",
                        Token = ReferenceTokens.Saml2Token_Valid,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultAADSigningKey,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            AlgorithmValidator = ValidationDelegates.AlgorithmValidatorBuilder(false)
                        },
                        ExpectedException = ExpectedException.SecurityTokenInvalidSignatureException("IDX10514")
                    },
                    new Saml2TheoryData
                    {
                        TestId = $"{nameof(ReferenceTokens.Saml2Token_Valid)}_SpecifyAlgorithm_AlgorithmValidationValidates",
                        Token = ReferenceTokens.Saml2Token_Valid,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultAADSigningKey,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            AlgorithmValidator = ValidationDelegates.AlgorithmValidatorBuilder(true)
                        },
                    },
                    new Saml2TheoryData
                    {
                        TestId = $"{nameof(ReferenceTokens.Saml2Token_Valid_WithNoKeyInfo)}NullSigningKey",
                        Token = ReferenceTokens.Saml2Token_Valid_WithNoKeyInfo,
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
                    new Saml2TheoryData
                    {
                        TestId = $"{nameof(ReferenceTokens.Saml2Token_Valid_WithNoKeyInfo)}NotNullSigningKey",
                        Token = ReferenceTokens.Saml2Token_Valid_WithNoKeyInfo,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            IssuerSigningKey = KeyingMaterial.DefaultSymmetricSecurityKey_1024,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        },
                        ExpectedException = ExpectedException.SecurityTokenSignatureKeyNotFoundException("IDX10512:")
                    },
                };
            }
        }

        [Theory, MemberData(nameof(CreateSaml2TokenUsingTokenDescriptorTheoryData))]
        public void CreateSaml2TokenUsingTokenDescriptor(CreateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateSaml2TokenUsingTokenDescriptor", theoryData);
            context.PropertiesToIgnoreWhenComparing = new Dictionary<Type, List<string>>
            {
                { typeof(Saml2Assertion), new List<string> { "IssueInstant", "InclusiveNamespacesPrefixList", "Signature", "SigningCredentials", "CanonicalString" } },
                { typeof(Saml2SecurityToken), new List<string> { "SigningKey" } },
            };

            try
            {
                SecurityToken samlTokenFromSecurityTokenDescriptor = theoryData.Saml2SecurityTokenHandler.CreateToken(theoryData.TokenDescriptor) as Saml2SecurityToken;
                string tokenFromTokenDescriptor = theoryData.Saml2SecurityTokenHandler.WriteToken(samlTokenFromSecurityTokenDescriptor);

                var claimsIdentityFromTokenDescriptor = theoryData.Saml2SecurityTokenHandler.ValidateToken(tokenFromTokenDescriptor, theoryData.ValidationParameters, out SecurityToken validatedTokenFromTokenDescriptor).Identity as ClaimsIdentity;
                IdentityComparer.AreEqual(validatedTokenFromTokenDescriptor, samlTokenFromSecurityTokenDescriptor, context);

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateTokenTheoryData> CreateSaml2TokenUsingTokenDescriptorTheoryData
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
                return new TheoryData<CreateTokenTheoryData>
                {
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
                        Saml2SecurityTokenHandler = new Saml2SecurityTokenHandler(),
                        ValidationParameters = validationParameters,
                        ExpectedException = ExpectedException.NotSupportedException("IDX10105:")
                    },
                    new CreateTokenTheoryData
                    {
                        First = true,
                        TestId = "OnlySubjectClaims",
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            Audience = Default.Audience,
                            NotBefore = Default.NotBefore,
                            Expires = Default.Expires,
                            Issuer = Default.Issuer,
                            SigningCredentials = new SigningCredentials(KeyingMaterial.X509SecurityKeySelfSigned2048_SHA256, SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.Sha256Digest),
                            EncryptingCredentials = null,
                            Subject = new ClaimsIdentity(Default.SamlClaims)
                        },
                        Saml2SecurityTokenHandler = new Saml2SecurityTokenHandler(),
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
                            Subject = new ClaimsIdentity(Default.SamlClaims)
                        },
                        Saml2SecurityTokenHandler = new Saml2SecurityTokenHandler(),
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
                            Subject = new ClaimsIdentity
                            (
                                new List<Claim>
                                {
                                    new Claim(ClaimTypes.Country, "USA", ClaimValueTypes.String, Default.Issuer, Default.OriginalIssuer),
                                    new Claim(ClaimTypes.NameIdentifier, "Bob", ClaimValueTypes.String, Default.Issuer, Default.OriginalIssuer),
                                    new Claim(ClaimTypes.Email, "Bob@contoso.com", ClaimValueTypes.String, Default.Issuer, Default.OriginalIssuer)
                                }
                            )
                        },
                        Saml2SecurityTokenHandler = new Saml2SecurityTokenHandler(),
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
                            Subject = new ClaimsIdentity(Default.SamlClaims)
                        },
                        Saml2SecurityTokenHandler = new Saml2SecurityTokenHandler(),
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
                            Subject = new ClaimsIdentity
                            (
                                new List<Claim>
                                {
                                    new Claim(ClaimTypes.Email, "Bob@contoso.com", ClaimValueTypes.String, Default.Issuer, Default.OriginalIssuer),
                                    new Claim(ClaimTypes.GivenName, "Bob", ClaimValueTypes.String, Default.Issuer, Default.OriginalIssuer),
                                    new Claim(ClaimTypes.Country, "India", ClaimValueTypes.String, Default.Issuer, Default.OriginalIssuer)
                                }
                            )
                        },
                        Saml2SecurityTokenHandler = new Saml2SecurityTokenHandler(),
                        ValidationParameters = validationParameters
                    },
                };
            }
        }

        [Theory, MemberData(nameof(CreateValidateActorClaimProcessing))]
        public void ValidateActorClaimProcessing(Saml2TheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateActorClaimProcessing", theoryData);
            try
            {
                var token = theoryData.Token;
                var actorName = "TestActor";
                ClaimsPrincipal claimPrinciple = theoryData.Handler.ValidateToken(token, theoryData.ValidationParameters, out SecurityToken validatedToken);
                theoryData.ExpectedException.ProcessNoException(context);
                ClaimsIdentity validatedIdentity = claimPrinciple?.Identities.FirstOrDefault(identity => identity.Actor != null);
                IdentityComparer.AreStringsEqual(actorName, validatedIdentity.Actor.Name, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }
            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<Saml2TheoryData> CreateValidateActorClaimProcessing
        {
            get
            {
                return new TheoryData<Saml2TheoryData>
                {
                    new Saml2TheoryData
                    {
                        Handler = new Saml2SecurityTokenHandler(),
                        TestId = nameof(ReferenceTokens.Saml2Token_Actor_Claim),
                        Token = ReferenceTokens.Saml2Token_Actor_Claim,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultAADSigningKey,
                            RequireSignedTokens = false,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        }
                    },
                };
            }
        }

        [Theory, MemberData(nameof(SecurityKeyNotFoundExceptionTestTheoryData))]
        public void Saml2SecurityKeyNotFoundExceptionTest(CreateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.SecurityKeyNotFoundExceptionTest", theoryData);

            try
            {
                var handler = new Saml2SecurityTokenHandler();
                var token = handler.CreateToken(theoryData.TokenDescriptor);
                handler.ValidateToken(handler.WriteToken(token), theoryData.ValidationParameters, out var validationResult);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateTokenTheoryData> SecurityKeyNotFoundExceptionTestTheoryData()
        {
            return new TheoryData<CreateTokenTheoryData>()
            {
                new CreateTokenTheoryData
                {
                    First = true,
                    TestId = "TokenExpired",
                    TokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(Default.SamlClaims),
                        Expires = DateTime.UtcNow.Subtract(new TimeSpan(0, 10, 0)),
                        IssuedAt = DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0)),
                        NotBefore = DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0)),
                        SigningCredentials = Default.AsymmetricSigningCredentials,
                        Issuer = Default.Issuer,
                     },
                     ValidationParameters = new TokenValidationParameters
                     {
                        IssuerSigningKey = Default.SymmetricSigningKey,
                        ValidIssuer = Default.Issuer,
                     },
                     ExpectedException = ExpectedException.SecurityTokenUnableToValidateException("IDX10515:")
                },
                new CreateTokenTheoryData
                {
                    TestId = "InvalidIssuer",
                    TokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(Default.SamlClaims),
                        SigningCredentials = Default.AsymmetricSigningCredentials,
                        Issuer = Default.Issuer,
                    },
                    ValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = Default.SymmetricSigningKey,
                    },
                    ExpectedException = ExpectedException.SecurityTokenUnableToValidateException("IDX10515:")
                },
                new CreateTokenTheoryData
                {
                    TestId = "ExpiredAndInvalidIssuer",
                    TokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(Default.SamlClaims),
                        Expires = DateTime.UtcNow.Subtract(new TimeSpan(0, 10, 0)),
                        IssuedAt = DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0)),
                        NotBefore = DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0)),
                        SigningCredentials = Default.AsymmetricSigningCredentials,
                        Issuer = Default.Issuer,
                    },
                    ValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = Default.SymmetricSigningKey,
                    },
                    ExpectedException = ExpectedException.SecurityTokenUnableToValidateException("IDX10515:")
                },
                new CreateTokenTheoryData
                {
                    TestId = "KeysDontMatchValidLifetimeAndIssuer",
                    TokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(Default.SamlClaims),
                        SigningCredentials = Default.AsymmetricSigningCredentials,
                        Issuer = Default.Issuer,
                    },
                    ValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = Default.SymmetricSigningKey,
                        ValidIssuer = Default.Issuer,
                    },
                    ExpectedException = ExpectedException.SecurityTokenSignatureKeyNotFoundException("IDX10513:")
                }
            };
        }
    }

    public class Saml2SecurityTokenHandlerPublic : Saml2SecurityTokenHandler
    {
        public ICollection<Saml2Attribute> ConsolidateAttributesPublic(ICollection<Saml2Attribute> attributes)
        {
            return ConsolidateAttributes(attributes);
        }

        public string CreateActorStringPublic(ClaimsIdentity identity)
        {
            return CreateActorString(identity);
        }

        public void ProcessAttributeStatementPublic(Saml2AttributeStatement statement, ClaimsIdentity identity, string issuer)
        {
            ProcessAttributeStatement(statement, identity, issuer);
        }

        public string ValidateIssuerPublic(string issuer, SecurityToken token, TokenValidationParameters validationParameters)
        {
            return base.ValidateIssuer(issuer, token, validationParameters);
        }

        public void ValidateAudiencePublic(IEnumerable<string> audiences, SecurityToken token, TokenValidationParameters validationParameters)
        {
            base.ValidateAudience(audiences, token, validationParameters);
        }
    }

    public class Saml2SecurityTokenPublic : Saml2SecurityToken
    {
        public Saml2SecurityTokenPublic(Saml2Assertion assertion)
            : base(assertion)
        {
        }
    }

    public class CreateTokenTheoryData : TheoryDataBase
    {
        public Dictionary<string, object> AdditionalHeaderClaims { get; set; }

        public string Payload { get; set; }

        public string CompressionAlgorithm { get; set; }

        public CompressionProviderFactory CompressionProviderFactory { get; set; }

        public EncryptingCredentials EncryptingCredentials { get; set; }

        public bool IsValid { get; set; } = true;

        public SigningCredentials SigningCredentials { get; set; }

        public SecurityTokenDescriptor TokenDescriptor { get; set; }

        public Saml2SecurityTokenHandler Saml2SecurityTokenHandler { get; set; }

        public string SamlToken { get; set; }

        public TokenValidationParameters ValidationParameters { get; set; }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
