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
using System.Reflection;
using System.Security.Claims;
using Microsoft.IdentityModel.Protocols.Extensions.OldVersion;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Saml2;
using Xunit;

using AuthenticationInformation4x = System.Security.Claims.AuthenticationInformation;
using LifeTime4x = System.IdentityModel.Protocols.WSTrust.Lifetime;
using Saml2Action4x = System.IdentityModel.Tokens.Saml2Action;
using Saml2Advice4x = System.IdentityModel.Tokens.Saml2Advice;
using Saml2Assertion4x = System.IdentityModel.Tokens.Saml2Assertion;
using Saml2Attribute4x = System.IdentityModel.Tokens.Saml2Attribute;
using Saml2AttributeStatement4x = System.IdentityModel.Tokens.Saml2AttributeStatement;
using Saml2AudienceRestriction4x = System.IdentityModel.Tokens.Saml2AudienceRestriction;
using Saml2AuthenticationContext4x = System.IdentityModel.Tokens.Saml2AuthenticationContext;
using Saml2AuthenticationStatement4x = System.IdentityModel.Tokens.Saml2AuthenticationStatement;
using Saml2AuthorizationDecisionStatement4x = System.IdentityModel.Tokens.Saml2AuthorizationDecisionStatement;
using Saml2Conditions4x = System.IdentityModel.Tokens.Saml2Conditions;
using Saml2Evidence4x = System.IdentityModel.Tokens.Saml2Evidence;
using Saml2NameIdentifier4x = System.IdentityModel.Tokens.Saml2NameIdentifier;
using Saml2ProxyRestriction4x = System.IdentityModel.Tokens.Saml2ProxyRestriction;
using Saml2SecurityToken4x = System.IdentityModel.Tokens.Saml2SecurityToken;
using Saml2Subject4x = System.IdentityModel.Tokens.Saml2Subject;
using Saml2SubjectLocality4x = System.IdentityModel.Tokens.Saml2SubjectLocality;
using SecurityKeyIdentifier4x = System.IdentityModel.Tokens.SecurityKeyIdentifier;
using SecurityToken4x = System.IdentityModel.Tokens.SecurityToken;
using SecurityTokenDescriptor4x = System.IdentityModel.Tokens.SecurityTokenDescriptor;
using SigningCredentials4x = System.IdentityModel.Tokens.SigningCredentials;
using TokenValidationParameters4x = System.IdentityModel.Tokens.TokenValidationParameters;
using X509SecurityKey4x = System.IdentityModel.Tokens.X509SecurityKey;
using X509SigningCredentials4x = System.IdentityModel.Tokens.X509SigningCredentials;
using X509ThumbprintKeyIdentifierClause4x = System.IdentityModel.Tokens.X509ThumbprintKeyIdentifierClause;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.CrossVersionTokenValidation.Tests
{
    public class Saml2CrossVersionTokenValidationTests
    {
        [Theory, MemberData(nameof(CrossVersionSaml2TokenTestTheoryData))]
        public void CrossVersionSaml2TokenTest(CrossTokenVersionTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CrossVersionSaml2TokenTest", theoryData);
            var samlHandler5x = new Tokens.Saml2.Saml2SecurityTokenHandler();

            var samlToken4x = CrossVersionUtility.CreateSaml2Token4x(theoryData.TokenDescriptor4x);
            var samlToken5x = samlHandler5x.CreateToken(theoryData.TokenDescriptor5x, theoryData.AuthenticationInformationSaml2) as Saml2SecurityToken;

            AreSaml2TokensEqual(samlToken4x, samlToken5x, context);

            var token4x = CrossVersionUtility.WriteSaml2Token(samlToken4x);
            var token5x = samlHandler5x.WriteToken(samlToken5x);

            var claimsPrincipalFrom4xUsing5xHandler = samlHandler5x.ValidateToken(token4x, theoryData.ValidationParameters5x, out SecurityToken validatedSamlToken4xUsing5xHandler);
            var claimsPrincipalFrom5xUsing5xHandler = samlHandler5x.ValidateToken(token5x, theoryData.ValidationParameters5x, out SecurityToken validatedSamlToken5xUsing5xHandler);
            var claimsPrincipalFrom4xUsing4xHandler = CrossVersionUtility.ValidateSaml2Token(token4x, theoryData.ValidationParameters4x, out SecurityToken4x validatedSamlToken4xUsing4xHandler);
            var claimsPrincipalFrom5xUsing4xHandler = CrossVersionUtility.ValidateSaml2Token(token5x, theoryData.ValidationParameters4x, out SecurityToken4x validatedSamlToken5xUsing4xHandler);

            IdentityComparer.AreClaimsPrincipalsEqual(claimsPrincipalFrom4xUsing4xHandler, claimsPrincipalFrom5xUsing4xHandler, context);
            IdentityComparer.AreClaimsPrincipalsEqual(claimsPrincipalFrom4xUsing5xHandler, claimsPrincipalFrom5xUsing4xHandler, context);
            IdentityComparer.AreClaimsPrincipalsEqual(claimsPrincipalFrom5xUsing5xHandler, claimsPrincipalFrom5xUsing4xHandler, context);

            // the results from ValidateTokenAsync() and ValidateToken() should be the same
            var tokenValidationResult = samlHandler5x.ValidateTokenAsync(token4x, theoryData.ValidationParameters5x);
            IdentityComparer.AreClaimsIdentitiesEqual(claimsPrincipalFrom5xUsing5xHandler.Identity as ClaimsIdentity, tokenValidationResult.Result.ClaimsIdentity, context);

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CrossTokenVersionTheoryData> CrossVersionSaml2TokenTestTheoryData
        {
            get
            {
                var certificate = KeyingMaterial.CertSelfSigned2048_SHA256;
                var key = KeyingMaterial.X509SecurityKeySelfSigned2048_SHA256;
                var keyClause = new X509ThumbprintKeyIdentifierClause4x(certificate);
                var keyIdentifier = new SecurityKeyIdentifier4x(keyClause);

                return new TheoryData<CrossTokenVersionTheoryData>
                {
                    new CrossTokenVersionTheoryData
                    {
                        AuthenticationInformationSaml2 = new Tokens.Saml2.AuthenticationInformation(Default.AuthenticationMethodUri, Default.AuthenticationInstantDateTime)
                        {
                             Address = Default.DNSAddress,
                             DnsName = Default.DNSName,
                             NotOnOrAfter = Default.NotOnOrAfter,
                             Session = Default.Session
                        },
                        First = true,
                        TestId = "Test1",
                        TokenDescriptor4x = new SecurityTokenDescriptor4x
                        {
                            AppliesToAddress = Default.Audience,
                            AuthenticationInfo = new AuthenticationInformation4x
                            {
                                Address = Default.DNSAddress,
                                DnsName = Default.DNSName,
                                NotOnOrAfter = Default.NotOnOrAfter,
                                Session = Default.Session
                            },
                            Lifetime = new LifeTime4x(Default.NotBefore, Default.Expires),
                            SigningCredentials = new SigningCredentials4x(new X509SecurityKey4x(certificate), SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.Sha256Digest, keyIdentifier),
                            Subject = AuthenticationClaimsIdentity,
                            TokenIssuerName = Default.Issuer,
                        },
                        TokenDescriptor5x = new SecurityTokenDescriptor
                        {
                            Audience = Default.Audience,
                            NotBefore = Default.NotBefore,
                            Expires = Default.Expires,
                            Issuer = Default.Issuer,
                            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.Sha256Digest),
                            Subject = new ClaimsIdentity(Default.SamlClaimsWithoutCRLF)
                        },
                        ValidationParameters4x = new TokenValidationParameters4x
                        {
                            ValidateAudience = false,
                            ValidateIssuer = false,
                            ValidateLifetime = false,
                            IssuerSigningKey = new X509SecurityKey4x(certificate)
                        },
                        ValidationParameters5x = new TokenValidationParameters
                        {
                            AuthenticationType = "Federation",
                            ValidateAudience = false,
                            ValidateIssuer = false,
                            ValidateLifetime = false,
                            IssuerSigningKey = key
                        }
                    },
                    new CrossTokenVersionTheoryData
                    {
                        AuthenticationInformationSaml2 = new Microsoft.IdentityModel.Tokens.Saml2.AuthenticationInformation(Default.AuthenticationMethodUri, Default.AuthenticationInstantDateTime)
                        {
                             Address = Default.DNSAddress,
                             DnsName = Default.DNSName,
                             NotOnOrAfter = Default.NotOnOrAfter,
                             Session = Default.Session
                        },
                        TestId = "Test2",
                        TokenDescriptor4x = new SecurityTokenDescriptor4x
                        {
                            AppliesToAddress = Default.Audience,
                            AuthenticationInfo = new AuthenticationInformation4x
                            {
                                Address = Default.DNSAddress,
                                DnsName = Default.DNSName,
                                NotOnOrAfter = Default.NotOnOrAfter,
                                Session = Default.Session
                            },
                            Lifetime = new LifeTime4x(Default.NotBefore, Default.Expires),
                            SigningCredentials = new SigningCredentials4x(new X509SecurityKey4x(certificate), SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.Sha256Digest, keyIdentifier),
                            Subject = AuthenticationClaimsIdentity,
                            TokenIssuerName = Default.Issuer,
                        },
                        TokenDescriptor5x = new SecurityTokenDescriptor
                        {
                            Audience = Default.Audience,
                            NotBefore = Default.NotBefore,
                            Expires = Default.Expires,
                            Issuer = Default.Issuer,
                            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.Sha256Digest),
                            Subject = AuthenticationClaimsIdentity
                        },
                        ValidationParameters4x = new TokenValidationParameters4x
                        {
                            ValidateAudience = false,
                            ValidateIssuer = false,
                            ValidateLifetime = false,
                            IssuerSigningKey = new X509SecurityKey4x(certificate)
                        },
                        ValidationParameters5x = new TokenValidationParameters
                        {
                            AuthenticationType = "Federation",
                            ValidateAudience = false,
                            ValidateIssuer = false,
                            ValidateLifetime = false,
                            IssuerSigningKey = key
                        },
                    }
                };
            }
        }

        [Theory, MemberData(nameof(CreateClaimsPrincipalCrossVersionTestTheoryData))]
        public void CreateClaimsPrincipalCrossVersionTest(CrossTokenVersionTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateClaimsPrincipalCrossVersionTest", theoryData);
            SecurityToken4x validatedToken4x = null;
            SecurityToken validatedToken5x = null;
            ClaimsPrincipal claimsPrincipal4x = null;
            ClaimsPrincipal claimsPrincipal5x = null;

            try
            {
                claimsPrincipal4x = CrossVersionUtility.ValidateSaml2Token(theoryData.TokenString4x, theoryData.ValidationParameters4x, out validatedToken4x);
            }
            catch (Exception ex)
            {
                context.Diffs.Add($"CrossVersionTokenValidationTestsData.ValidateToken threw: '{ex}'.");
            }

            try
            {
                claimsPrincipal5x = new Tokens.Saml2.Saml2SecurityTokenHandler().ValidateToken(theoryData.TokenString4x, theoryData.ValidationParameters5x, out validatedToken5x);
            }
            catch (Exception ex)
            {
                context.Diffs.Add($"Tokens.Saml.SamlSecurityTokenHandler().ValidateToken threw: '{ex}'.");
            }

            AreSaml2TokensEqual(validatedToken4x, validatedToken5x, context);
            IdentityComparer.AreClaimsPrincipalsEqual(claimsPrincipal4x, claimsPrincipal5x, context);

            // verify the results from asynchronous and synchronous are the same
            TokenValidationResult tokenValidationResult = null;
            try
            {
                tokenValidationResult = new Tokens.Saml2.Saml2SecurityTokenHandler().ValidateTokenAsync(theoryData.TokenString4x, theoryData.ValidationParameters5x).Result;
            }
            catch (Exception ex)
            {
                context.Diffs.Add($"CrossVersionTokenValidationTestsData.ValidateToken threw: '{ex}'.");
            }
            IdentityComparer.AreClaimsIdentitiesEqual(claimsPrincipal5x.Identity as ClaimsIdentity, tokenValidationResult.ClaimsIdentity, context);

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CrossTokenVersionTheoryData> CreateClaimsPrincipalCrossVersionTestTheoryData
        {
            get
            {
                var notBefore = DateTime.UtcNow;
                var expires = notBefore + TimeSpan.FromDays(1);
                var signingCredentials = new X509SigningCredentials4x(KeyingMaterial.DefaultCert_2048, SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.Sha256Digest);
                var tokenDescriptor4x = new SecurityTokenDescriptor4x
                {
                    AppliesToAddress = Default.Audience,
                    Lifetime = new LifeTime4x(notBefore, expires),
                    SigningCredentials = signingCredentials,
                    Subject = new ClaimsIdentity(Default.SamlClaimsWithoutCRLF),
                    TokenIssuerName = Default.Issuer,
                };

                var token4x_AttributeStatement = CrossVersionUtility.CreateSaml2Token4x(tokenDescriptor4x);

                tokenDescriptor4x.Subject = AuthenticationClaimsIdentity;
                tokenDescriptor4x.AuthenticationInfo = new AuthenticationInformation4x
                {
                    Address = Default.IPAddress,
                    DnsName = Default.DNSAddress
                };

                var token4x_AuthenticationStatement = CrossVersionUtility.CreateSaml2Token4x(tokenDescriptor4x);

                tokenDescriptor4x.Subject = AuthorizationDecisionClaimsIdentity;
                var samlAssertion_AuthorizationDecision = new Saml2Assertion4x(new Saml2NameIdentifier4x("name"))
                {
                    SigningCredentials = signingCredentials
                };
                samlAssertion_AuthorizationDecision.Statements.Add(new Saml2AuthorizationDecisionStatement4x(new Uri(Default.Uri), System.IdentityModel.Tokens.SamlAccessDecision.Permit));

                var token4x_AuthorizationDecisionStatement = CrossVersionUtility.CreateSaml2Token4x(tokenDescriptor4x) as Saml2SecurityToken4x;
                token4x_AuthorizationDecisionStatement.Assertion.Statements.Add(new Saml2AuthorizationDecisionStatement4x(new Uri(Default.Uri), System.IdentityModel.Tokens.SamlAccessDecision.Permit, new List<Saml2Action4x> { new Saml2Action4x("value", new Uri(Default.Uri)) }));
                var samlToken = CrossVersionUtility.WriteSaml2Token(token4x_AuthorizationDecisionStatement);

                var validationParameters4x = new TokenValidationParameters4x
                {
                    IssuerSigningKey = new X509SecurityKey4x(KeyingMaterial.DefaultCert_2048),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                };

                var validationParameters5x = new TokenValidationParameters
                {
                    IssuerSigningKey = new X509SecurityKey(KeyingMaterial.DefaultCert_2048),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                };

                return new TheoryData<CrossTokenVersionTheoryData>
                {
                    new CrossTokenVersionTheoryData
                    {
                        TestId = "AttributeStatement",
                        TokenString4x = CrossVersionUtility.WriteSaml2Token(token4x_AttributeStatement),
                        ValidationParameters4x = validationParameters4x,
                        ValidationParameters5x = validationParameters5x
                    },
                    new CrossTokenVersionTheoryData
                    {
                        TestId = "AuthenticationStatement",
                        TokenString4x = CrossVersionUtility.WriteSaml2Token(token4x_AuthenticationStatement),
                        ValidationParameters4x = validationParameters4x,
                        ValidationParameters5x = validationParameters5x
                    },
                    new CrossTokenVersionTheoryData
                    {
                        TestId = "AuthorizationDecisionStatement",
                        TokenString4x = CrossVersionUtility.WriteSaml2Token(token4x_AuthorizationDecisionStatement),
                        ValidationParameters4x = validationParameters4x,
                        ValidationParameters5x = validationParameters5x
                    }
                };
            }
        }

        private static ClaimsIdentity AuthenticationClaimsIdentity
        {
            get => new ClaimsIdentity(new List<Claim>(Default.SamlClaimsWithoutCRLF)
            {
                new Claim(ClaimTypes.AuthenticationMethod, Default.AuthenticationMethod, ClaimValueTypes.String, Default.Issuer),
                new Claim(ClaimTypes.AuthenticationInstant, Default.AuthenticationInstant, ClaimValueTypes.DateTime, Default.Issuer)
            });
        }

        private static ClaimsIdentity AuthorizationDecisionClaimsIdentity
        {
            get
            {
                var authorizationDecisionClaims = new List<Claim>
                {
                    Default.SamlClaimsWithoutCRLF.Find(x => x.Type == ClaimTypes.NameIdentifier)
                };

                return new ClaimsIdentity(authorizationDecisionClaims);
            }
        }

        private static bool AreSaml2ActionsEqual(Saml2Action4x action4x, Saml2Action action5x, CompareContext context)
        {
            var localContext = new CompareContext(context);
            if (!IdentityComparer.ContinueCheckingEquality(action4x, action5x, localContext))
                return context.Merge(localContext);

            CrossVersionUtility.AreUrisEqual(action4x.Namespace, action5x.Namespace, localContext);
            CrossVersionUtility.AreStringsEqual(action4x.Value, action5x.Value, localContext);

            return context.Merge(localContext);
        }

        private static bool AreSaml2AdvicesEqual(Saml2Advice4x advice4x, Saml2Advice advice5x, CompareContext context)
        {
            var localContext = new CompareContext(context);
            if (!IdentityComparer.ContinueCheckingEquality(advice4x, advice5x, localContext))
                return context.Merge(localContext);

            if (advice4x.AssertionIdReferences.Count != advice5x.AssertionIdReferences.Count)
                localContext.Diffs.Add(Environment.NewLine + $"advice1.AssertionIdReferences.Count != advice2.AssertionIdReferences.Count: {advice4x.AssertionIdReferences.Count}, {advice5x.AssertionIdReferences.Count}");

            AreSaml2ObjectEnumsEqual(advice4x.Assertions, advice5x.Assertions, localContext);
            return context.Merge(localContext);
        }

        private static bool AreSaml2AssertionsEqual(Saml2Assertion4x assertion4x, Saml2Assertion assertion5x, CompareContext context)
        {
            var localContext = new CompareContext(context);
            if (!IdentityComparer.ContinueCheckingEquality(assertion4x, assertion5x, localContext))
                return context.Merge(localContext);

            if (assertion4x.Version != assertion5x.Version)
                localContext.Diffs.Add(Environment.NewLine + $"assertion4x.Version != assertion2.Version: {assertion4x.Version}, {assertion5x.Version}");

            // Compare Statements
            AreSaml2ObjectEnumsEqual(assertion4x.Statements, assertion5x.Statements, localContext);

            return context.Merge(localContext);
        }

        private static bool AreSaml2AttributesEqual(Saml2Attribute4x attribute4x, Saml2Attribute attribute5x, CompareContext context)
        {
            var localContext = new CompareContext(context);
            if (!IdentityComparer.ContinueCheckingEquality(attribute4x, attribute5x, localContext))
                return context.Merge(localContext);

            if (String.CompareOrdinal(attribute4x.Name, attribute5x.Name) != 0)
                localContext.Diffs.Add($"attribute4x.Name != attribute5x.Name: {attribute4x.Name}, {attribute5x.Name}");

            if (String.CompareOrdinal(attribute4x.OriginalIssuer, attribute5x.OriginalIssuer) != 0)
                localContext.Diffs.Add($"attribute4x.OriginalIssuer != attribute5x.OriginalIssuer: {attribute4x.OriginalIssuer}, {attribute5x.OriginalIssuer}");

            if (String.CompareOrdinal(attribute4x.AttributeValueXsiType, attribute5x.AttributeValueXsiType) != 0)
                localContext.Diffs.Add($"attribute4x.AttributeValueXsiType != attribute5x.AttributeValueXsiType: {attribute4x.AttributeValueXsiType}, {attribute5x.AttributeValueXsiType}");

            AreSaml2ObjectEnumsEqual(attribute4x.Values, attribute5x.Values, localContext);
            return context.Merge(localContext);
        }

        private static bool AreSaml2AttributeStatementsEqual(Saml2AttributeStatement4x attributeStatement4x, Saml2AttributeStatement attributeStatement5x, CompareContext context)
        {
            var localContext = new CompareContext(context);
            if (!IdentityComparer.ContinueCheckingEquality(attributeStatement4x, attributeStatement5x, localContext))
                return context.Merge(localContext);

            AreSaml2ObjectEnumsEqual(attributeStatement4x.Attributes, attributeStatement5x.Attributes, localContext);
            return context.Merge(localContext);
        }

        private static bool AreSaml2AuthenticationContextsEqual(Saml2AuthenticationContext4x authenticationContext4x, Saml2AuthenticationContext authenticationContext5x, CompareContext context)
        {
            var localContext = new CompareContext(context);
            if (!IdentityComparer.ContinueCheckingEquality(authenticationContext4x, authenticationContext5x, localContext))
                return context.Merge(localContext);

            AreSaml2ObjectEnumsEqual(authenticationContext4x.AuthenticatingAuthorities, authenticationContext5x.AuthenticatingAuthorities, localContext);
            CrossVersionUtility.AreUrisEqual(authenticationContext4x.ClassReference, authenticationContext5x.ClassReference, localContext);
            CrossVersionUtility.AreUrisEqual(authenticationContext4x.ClassReference, authenticationContext5x.ClassReference, localContext);

            return context.Merge(localContext);
        }

        private static bool AreSaml2AuthenticationStatementsEqual(Saml2AuthenticationStatement4x authenticationStatement4x, Saml2AuthenticationStatement authenticationStatement5x, CompareContext context)
        {
            var localContext = new CompareContext(context);
            if (!IdentityComparer.ContinueCheckingEquality(authenticationStatement4x, authenticationStatement5x, localContext))
                return context.Merge(localContext);

            AreSaml2AuthenticationContextsEqual(authenticationStatement4x.AuthenticationContext, authenticationStatement5x.AuthenticationContext, localContext);
            CrossVersionUtility.AreDateTimesEqual(authenticationStatement4x.AuthenticationInstant, authenticationStatement5x.AuthenticationInstant, localContext);
            CrossVersionUtility.AreStringsEqual(authenticationStatement4x.SessionIndex, authenticationStatement5x.SessionIndex, localContext);
            CrossVersionUtility.AreDateTimesEqual(authenticationStatement4x.SessionNotOnOrAfter, authenticationStatement5x.SessionNotOnOrAfter, localContext);
            AreSaml2SubjectLocalitiesEqual(authenticationStatement4x.SubjectLocality, authenticationStatement5x.SubjectLocality, localContext);

            return context.Merge(localContext);
        }        

        private static bool AreSaml2AuthorizationDecisionStatementsEqual(Saml2AuthorizationDecisionStatement4x authorizationDecisionStatement4x, Saml2AuthorizationDecisionStatement authorizationDecisionStatement5x, CompareContext context)
        {
            var localContext = new CompareContext(context);
            if (!IdentityComparer.ContinueCheckingEquality(authorizationDecisionStatement4x, authorizationDecisionStatement5x, localContext))
                return context.Merge(localContext);

            AreSaml2ObjectEnumsEqual(authorizationDecisionStatement4x.Actions, authorizationDecisionStatement5x.Actions, localContext);
            CrossVersionUtility.AreStringsEqual(authorizationDecisionStatement4x.Decision.ToString(), authorizationDecisionStatement5x.Decision, localContext);
            AreSaml2EvidencesEqual(authorizationDecisionStatement4x.Evidence, authorizationDecisionStatement5x.Evidence, localContext);
            CrossVersionUtility.AreUrisEqual(authorizationDecisionStatement4x.Resource, authorizationDecisionStatement5x.Resource, localContext);

            return context.Merge(localContext);
        }

        private static bool AreSaml2AudienceRestrictionsEqual(Saml2AudienceRestriction4x audienceRestriction4x, Saml2AudienceRestriction audienceRestriction5x, CompareContext context)
        {
            var localContext = new CompareContext(context);
            if (!IdentityComparer.ContinueCheckingEquality(audienceRestriction4x, audienceRestriction5x, localContext))
                return context.Merge(localContext);

            AreSaml2ObjectEnumsEqual(audienceRestriction4x.Audiences, audienceRestriction5x.Audiences, localContext);

            return context.Merge(localContext);
        }

        private static bool AreSaml2AuthenticationContextEqual(Saml2AuthenticationContext4x audienceContext4x, Saml2AuthenticationContext audienceContext5x, CompareContext context)
        {
            var localContext = new CompareContext(context);
            if (!IdentityComparer.ContinueCheckingEquality(audienceContext4x, audienceContext5x, localContext))
                return context.Merge(localContext);

            AreSaml2ObjectEnumsEqual(audienceContext4x.AuthenticatingAuthorities, audienceContext5x.AuthenticatingAuthorities, localContext);
            CrossVersionUtility.AreUrisEqual(audienceContext4x.ClassReference, audienceContext5x.ClassReference, localContext);
            CrossVersionUtility.AreUrisEqual(audienceContext4x.DeclarationReference, audienceContext5x.DeclarationReference, localContext);

            return context.Merge(localContext);
        }
    
        private static bool AreSaml2ConditionsEnumsEqual(Saml2Conditions4x conditions4x, Saml2Conditions conditions5x, CompareContext context)
        {
            var localContext = new CompareContext(context);
            if (!IdentityComparer.ContinueCheckingEquality(conditions4x, conditions5x, localContext))
                return context.Merge(localContext);

            AreSaml2ObjectEnumsEqual(conditions4x.AudienceRestrictions, conditions5x.AudienceRestrictions, localContext);
            CrossVersionUtility.AreDateTimesEqual(conditions4x.NotBefore, conditions5x.NotBefore, localContext);
            CrossVersionUtility.AreDateTimesEqual(conditions4x.NotOnOrAfter, conditions5x.NotOnOrAfter, localContext);
            if (conditions4x.OneTimeUse != conditions5x.OneTimeUse)
                localContext.Diffs.Add($"conditions4x.OneTimeUse != conditions5x.OneTimeUse: {conditions4x.OneTimeUse}, {conditions5x.OneTimeUse}");

            AreSaml2ProxyRestrictionsEqual(conditions4x.ProxyRestriction, conditions5x.ProxyRestriction, localContext);

            return context.Merge(localContext);
        }

        public static bool AreSaml2NameIdentifiersEqual(Saml2NameIdentifier4x nameId4x, Saml2NameIdentifier nameId5x, CompareContext context)
        {
            var localContext = new CompareContext(context);
            if (!IdentityComparer.ContinueCheckingEquality(nameId4x, nameId5x, localContext))
                return context.Merge(localContext);

            CrossVersionUtility.AreUrisEqual(nameId4x.Format, nameId5x.Format, context);
            CrossVersionUtility.AreStringsEqual(nameId4x.NameQualifier, nameId5x.NameQualifier, context);
            CrossVersionUtility.AreStringsEqual(nameId4x.SPNameQualifier, nameId5x.SPNameQualifier, context);
            CrossVersionUtility.AreStringsEqual(nameId4x.SPProvidedId, nameId5x.SPProvidedId, context);
            CrossVersionUtility.AreStringsEqual(nameId4x.Value, nameId5x.Value, context);

            return context.Merge(localContext);
        }

        private static bool AreSaml2ObjectEnumsEqual<T1, T2>(ICollection<T1> objs1, ICollection<T2> objs2, CompareContext context)
        {
            var localContext = new CompareContext(context);
            if (!IdentityComparer.ContinueCheckingEquality(objs1, objs2, localContext))
                return context.Merge(localContext);

            if (objs1.Count() != objs2.Count())
                localContext.Diffs.Add(Environment.NewLine + $"obj1.Count != obj2.Count: {objs1.Count()}, {objs2.Count()}");

            List<T1> toMatch = new List<T1>(objs1);
            List<T2> expectedValues = new List<T2>(objs2);

            int numMatched = 0;
            int numToMatch = toMatch.Count();
            var notMatched = new List<T1>();
            foreach (var obj in toMatch)
            {
                var perObjectContext = new CompareContext(localContext);
                bool matched = false;
                for (int i = 0; i < expectedValues.Count(); i++)
                {
                    if (obj is string)
                    {
                        var str1 = obj as string;
                        var str2 = expectedValues.ElementAt(i) as string;
                        if (str1.Equals(str2))
                        {
                            numMatched++;
                            matched = true;
                            expectedValues.Remove(expectedValues.ElementAt(i));
                            break;
                        }
                    }
                    else if (obj is Uri)
                    {
                        var str1 = (obj as Uri).OriginalString;
                        var str2 = (expectedValues.ElementAt(i) as Uri).OriginalString;
                        if (str1.Equals(str2))
                        {
                            numMatched++;
                            matched = true;
                            expectedValues.Remove(expectedValues.ElementAt(i));
                            break;
                        }
                    }
                    else if (obj is Saml2Attribute4x)
                    {
                        if (AreSaml2AttributesEqual(obj as Saml2Attribute4x, expectedValues.ElementAt(i) as Saml2Attribute, perObjectContext))
                        {
                            numMatched++;
                            matched = true;
                            expectedValues.Remove(expectedValues.ElementAt(i));
                            break;
                        }
                    }
                    else if (obj is Saml2AttributeStatement4x)
                    {
                        if (AreSaml2AttributeStatementsEqual(obj as Saml2AttributeStatement4x, expectedValues.ElementAt(i) as Saml2AttributeStatement, perObjectContext))
                        {
                            numMatched++;
                            matched = true;
                            expectedValues.Remove(expectedValues.ElementAt(i));
                            break;
                        }
                    }
                    else if (obj is Saml2AuthenticationStatement4x)
                    {
                        if (AreSaml2AuthenticationStatementsEqual(obj as Saml2AuthenticationStatement4x, expectedValues.ElementAt(i) as Saml2AuthenticationStatement, perObjectContext))
                        {
                            numMatched++;
                            matched = true;
                            expectedValues.Remove(expectedValues.ElementAt(i));
                            break;
                        }
                    }
                    else if (obj is Saml2AuthorizationDecisionStatement4x)
                    {
                        if (AreSaml2AuthorizationDecisionStatementsEqual(obj as Saml2AuthorizationDecisionStatement4x, expectedValues.ElementAt(i) as Saml2AuthorizationDecisionStatement, perObjectContext))
                        {
                            numMatched++;
                            matched = true;
                            expectedValues.Remove(expectedValues.ElementAt(i));
                            break;
                        }
                    }
                    else if (obj is Saml2Action4x)
                    {
                        if (AreSaml2ActionsEqual(obj as Saml2Action4x, expectedValues.ElementAt(i) as Saml2Action, perObjectContext))
                        {
                            numMatched++;
                            matched = true;
                            expectedValues.Remove(expectedValues.ElementAt(i));
                            break;
                        }
                    }
                    else if (obj is Saml2Assertion4x)
                    {
                        if (AreSaml2AssertionsEqual(obj as Saml2Assertion4x, expectedValues.ElementAt(i) as Saml2Assertion, perObjectContext))
                        {
                            numMatched++;
                            matched = true;
                            expectedValues.Remove(expectedValues.ElementAt(i));
                            break;
                        }
                    }
                }

                if (!matched)
                {
                    notMatched.Add(obj);
                }
            }

            if (numMatched != numToMatch)
            {
                localContext.Diffs.Add(Environment.NewLine + "numMatched != numToMatch: " + numMatched + ", " + numToMatch);
                localContext.Diffs.Add(Environment.NewLine + $"objs1 NOT Matched: Type {toMatch.GetType()}:****************");
                foreach (var obj in notMatched)
                {
                    Type type = obj.GetType();
                    localContext.Diffs.Add(Environment.NewLine + $"Object Type: {type}");
                    PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    foreach (PropertyInfo propertyInfo in propertyInfos)
                    {
                        if (propertyInfo.GetMethod != null)
                        {
                            object val = propertyInfo.GetValue(obj, null);
                            if (val != null)
                                localContext.Diffs.Add(Environment.NewLine + $"{propertyInfo.Name}: {val}");
                        }
                    }
                }

                localContext.Diffs.Add(Environment.NewLine + $"objs2 NOT Matched: Type {expectedValues.GetType()}:****************");
                foreach (var obj in expectedValues)
                {
                    Type type = obj.GetType();
                    localContext.Diffs.Add(Environment.NewLine + $"Object Type: {type}");
                    PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    foreach (PropertyInfo propertyInfo in propertyInfos)
                    {
                        if (propertyInfo.GetMethod != null)
                        {
                            object val = propertyInfo.GetValue(obj, null);
                            if (val != null)
                                localContext.Diffs.Add(Environment.NewLine + $"{propertyInfo.Name}: {val}");
                        }
                    }
                }
            }

            return context.Merge(localContext);
        }

        private static bool AreSaml2ProxyRestrictionsEqual(Saml2ProxyRestriction4x proxyRestriction4x, Saml2ProxyRestriction proxyRestriction5x, CompareContext context)
        {
            var localContext = new CompareContext(context);
            if (!IdentityComparer.ContinueCheckingEquality(proxyRestriction4x, proxyRestriction4x, localContext))
                return context.Merge(localContext);

            AreSaml2ObjectEnumsEqual(proxyRestriction4x.Audiences, proxyRestriction5x.Audiences, localContext);
            if (proxyRestriction4x.Count.HasValue && proxyRestriction5x.Count.HasValue)
            {
                if (proxyRestriction4x.Count.Value != proxyRestriction5x.Count.Value)
                    localContext.Diffs.Add($"proxyRestriction4x.Count.Value != proxyRestriction5x.Count.Value: '{proxyRestriction4x.Count.Value}', {proxyRestriction5x.Count.Value}");
            }
            else if (!proxyRestriction4x.Count.HasValue || !proxyRestriction5x.Count.HasValue)
            {
                localContext.Diffs.Add($"proxyRestriction4x.Count.HasValue != proxyRestriction5x.Count.HasValue: '{proxyRestriction4x.Count.HasValue}', {proxyRestriction5x.Count.HasValue}");
            }

            return context.Merge(localContext);
        }

        private static bool AreSaml2SubjectsEqual(Saml2Subject4x subject4x, Saml2Subject subject5x, CompareContext context)
        {
            var localContext = new CompareContext(context);
            if (!IdentityComparer.ContinueCheckingEquality(subject4x, subject5x, localContext))
                return context.Merge(localContext);

            AreSaml2NameIdentifiersEqual(subject4x.NameId, subject5x.NameId, localContext);
            AreSaml2ObjectEnumsEqual(subject4x.SubjectConfirmations, subject5x.SubjectConfirmations, localContext);

            return context.Merge(localContext);
        }

        private static bool AreSaml2SubjectLocalitiesEqual(Saml2SubjectLocality4x subjectLocality4x, Saml2SubjectLocality subjectLocality5x, CompareContext context)
        {
            var localContext = new CompareContext(context);
            if (!IdentityComparer.ContinueCheckingEquality(subjectLocality4x, subjectLocality5x, localContext))
                return context.Merge(localContext);

            CrossVersionUtility.AreStringsEqual(subjectLocality4x.Address, subjectLocality5x.Address, localContext);
            CrossVersionUtility.AreStringsEqual(subjectLocality4x.DnsName, subjectLocality5x.DnsName, localContext);

            return context.Merge(localContext);
        }


        private static bool AreSaml2TokensEqual(SecurityToken4x token4x, SecurityToken token5x, CompareContext context)
        {
            var localContext = new CompareContext(context);
            if (!IdentityComparer.ContinueCheckingEquality(token4x, token5x, localContext))
                return context.Merge(localContext);

            if (!(token4x is Saml2SecurityToken4x samlToken4x))
                return false;

            if (!(token5x is Saml2SecurityToken samlToken5x))
                return false;

            if (!DateTime.Equals(samlToken4x.ValidFrom, samlToken5x.ValidFrom))
                localContext.Diffs.Add(Environment.NewLine + $"token1.ValidFrom != token2.ValidFrom: {samlToken4x.ValidFrom}, {samlToken5x.ValidFrom}");

            if (!DateTime.Equals(samlToken4x.ValidTo, samlToken5x.ValidTo))
                localContext.Diffs.Add(Environment.NewLine + $"token1.ValidTo != token2.ValidTo: {samlToken4x.ValidTo}  {samlToken5x.ValidTo}");

            AreSaml2AssertionsEqual(samlToken4x.Assertion, samlToken5x.Assertion, localContext);

            return context.Merge(localContext);
        }

        private static bool AreSaml2EvidencesEqual(Saml2Evidence4x evidence4x, Saml2Evidence evidence5x, CompareContext context)
        {
            var localContext = new CompareContext(context);
            if (!IdentityComparer.ContinueCheckingEquality(evidence4x, evidence5x, localContext))
                return context.Merge(localContext);

            AreSaml2ObjectEnumsEqual(evidence4x.AssertionIdReferences, evidence5x.AssertionIdReferences, localContext);
            AreSaml2ObjectEnumsEqual(evidence4x.Assertions, evidence5x.Assertions, localContext);
            AreSaml2ObjectEnumsEqual(evidence4x.AssertionUriReferences, evidence5x.AssertionUriReferences, localContext);

            return context.Merge(localContext);
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
