// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.WsFederation;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Saml;
using Microsoft.IdentityModel.Tokens.Saml2;
using Microsoft.IdentityModel.Xml;
using Newtonsoft.Json.Linq;
using Xunit;
using ClaimProperties = Microsoft.IdentityModel.Tokens.Saml.ClaimProperties;

namespace Microsoft.IdentityModel.TestUtils
{
    public class IdentityComparerTests
    {
        [Fact]
        public void CompareClaims()
        {
            TestUtilities.WriteHeader($"{this}.CompareClaims", true);
            var context = new CompareContext($"{this}.CompareClaims");

            // Base claim that all tests will compare against.
            var originalClaim = new Claim(ClaimTypes.Country, Default.Country, ClaimValueTypes.String, Default.Issuer, Default.OriginalIssuer);

            var claimsToCompare = new List<Claim>()
            {    
                // Claim with different value for 'type'
                new Claim(Guid.NewGuid().ToString(), Default.Country, ClaimValueTypes.String, Default.Issuer, Default.OriginalIssuer),
                // Claim with different value for 'value'
                new Claim(ClaimTypes.Country, Guid.NewGuid().ToString(), ClaimValueTypes.String, Default.Issuer, Default.OriginalIssuer),
                // Claim with different value for 'valueType'
                new Claim(ClaimTypes.Country, Default.Country, Guid.NewGuid().ToString(), Default.Issuer, Default.OriginalIssuer),
                // Claim with different value for 'issuer'
                new Claim(ClaimTypes.Country, Default.Country, ClaimValueTypes.String, Guid.NewGuid().ToString(), Default.OriginalIssuer),
                // Claim with different value for 'originalIssuer'
                new Claim(ClaimTypes.Country, Default.Country, ClaimValueTypes.String, Default.Issuer, Guid.NewGuid().ToString()),
            };

            foreach (var otherClaim in claimsToCompare)
            {
                IdentityComparer.AreEqual(originalClaim, otherClaim, context);
            }

            // Lists all the properties which should have been marked as different in the compareContext.
            var propertiesToTest = new string[] { "Type:", "Value:", "ValueType:", "Issuer:", "OriginalIssuer:" };

            // Make sure that differences have been found for each of the properties listed above.
            Assert.True(propertiesToTest.ToList().Aggregate(0, (sum, next) => context.Diffs.Contains(next) ? sum + 1 : sum) == 5);

        }

        [Fact]
        public void CompareClaimsWithProperties()
        {
            TestUtilities.WriteHeader($"{this}.CompareClaimsWithProperties", true);
            var context = new CompareContext($"{this}.CompareClaimsWithProperties");

            // Base claim that all tests will compare against.
            var originalClaim = new Claim(ClaimTypes.Country, Default.Country, ClaimValueTypes.String, Default.Issuer, Default.OriginalIssuer);
            originalClaim.Properties[ClaimProperties.SamlNameIdentifierFormat] = Default.NameIdentifierFormat;
            originalClaim.Properties[ClaimProperties.SamlNameIdentifierNameQualifier] = Default.NameQualifier;

            // Claim with the same property names but different values for them
            var claim1 = new Claim(ClaimTypes.Country, Default.Country, ClaimValueTypes.String, Default.Issuer, Default.OriginalIssuer);
            claim1.Properties[ClaimProperties.SamlNameIdentifierFormat] = Guid.NewGuid().ToString();
            claim1.Properties[ClaimProperties.SamlNameIdentifierNameQualifier] = Guid.NewGuid().ToString();

            // Claim with one property that's the same but another that's different.
            var claim2 = new Claim(ClaimTypes.Country, Default.Country, ClaimValueTypes.String, Default.Issuer, Default.OriginalIssuer);
            claim2.Properties[ClaimProperties.SamlNameIdentifierFormat] = Default.NameIdentifierFormat;
            claim2.Properties[ClaimProperties.SamlNameIdentifierNameQualifier] = Guid.NewGuid().ToString();

            // Claim with the same number of properties as the original (but different names and values).
            var claim3 = new Claim(ClaimTypes.Country, Default.Country, ClaimValueTypes.String, Default.Issuer, Default.OriginalIssuer);
            claim3.Properties[Guid.NewGuid().ToString()] = Guid.NewGuid().ToString();
            claim3.Properties[Guid.NewGuid().ToString()] = Guid.NewGuid().ToString();

            // Claim with only one property (that's shared with the original).
            var claim4 = new Claim(ClaimTypes.Country, Default.Country, ClaimValueTypes.String, Default.Issuer, Default.OriginalIssuer);
            claim4.Properties[ClaimProperties.SamlNameIdentifierFormat] = Default.NameIdentifierFormat;

            // Claim with no properties.
            var claim5 = new Claim(ClaimTypes.Country, Default.Country, ClaimValueTypes.String, Default.Issuer, Default.OriginalIssuer);

            var claimsToCompare = new List<Claim>()
            {
                claim1, claim2, claim3, claim4, claim5,
            };

            foreach (var otherClaim in claimsToCompare)
            {
                IdentityComparer.AreEqual(originalClaim, otherClaim, context);
            }

            // Make sure that the properties don't match for all 5 of the claims in the list above.
            Assert.True(context.Diffs.Count(s => s == "Properties:") == 5);
        }

        [Fact]
        public void CompareClaimsIdentities()
        {
            TestUtilities.WriteHeader($"{this}.CompareClaimsIdentities", true);
            var context = new CompareContext($"{this}.CompareClaimsIdentities");

            var originalClaims = new List<Claim>()
            {
                new Claim(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()),
                new Claim(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()),
            };

            var claims2 = new List<Claim>()
            {
                new Claim(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()),
                new Claim(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()),
            };

            var originalAuthenticationType = Guid.NewGuid().ToString();
            var originalNameType = Guid.NewGuid().ToString();
            var originalRoleType = Guid.NewGuid().ToString();
            var originalBootstrapContext = Guid.NewGuid().ToString();
            var originalLabel = Guid.NewGuid().ToString();
            var originalActor = new ClaimsIdentity(Guid.NewGuid().ToString());

            // Base ClaimsIdentity to use for all future comparisons.
            var originalClaimsIdentity = CreateClaimsIdentity(originalClaims, originalAuthenticationType,
                originalNameType, originalRoleType, originalLabel, originalBootstrapContext, originalActor);

            // ClaimsIdentity with different Claims.
            var claimsIdentityDiffClaims = CreateClaimsIdentity(claims2, originalAuthenticationType,
                originalNameType, originalRoleType, originalLabel, originalBootstrapContext, originalActor);

            // ClaimsIdentity with different AuthenticationType.
            var claimsIdentityDiffAuthenticationType = CreateClaimsIdentity(originalClaims, Guid.NewGuid().ToString(),
                originalNameType, originalRoleType, originalLabel, originalBootstrapContext, originalActor);

            // ClaimsIdentity with different NameClaimType.
            var claimsIdentityDiffNameType = CreateClaimsIdentity(originalClaims, originalAuthenticationType,
                Guid.NewGuid().ToString(), originalRoleType, originalLabel, originalBootstrapContext, originalActor);

            // ClaimsIdentity with different RoleClaimType.
            var claimsIdentityDiffRoleType = CreateClaimsIdentity(originalClaims, originalAuthenticationType,
                originalNameType, Guid.NewGuid().ToString(), originalLabel, originalBootstrapContext, originalActor);

            // ClaimsIdentity with different Label.
            var claimsIdentityDiffLabel = CreateClaimsIdentity(originalClaims, originalAuthenticationType,
                originalNameType, originalRoleType, Guid.NewGuid().ToString(), originalBootstrapContext, originalActor);

            // ClaimsIdentity with different BootstrapContext.
            var claimsIdentityDiffBootstrapContext = CreateClaimsIdentity(originalClaims, originalAuthenticationType,
                originalNameType, originalRoleType, originalLabel, Guid.NewGuid().ToString(), originalActor);

            // ClaimsIdentity with null Actor.
            var claimsIdentityDiffActor = CreateClaimsIdentity(originalClaims, originalAuthenticationType,
                originalNameType, originalRoleType, originalLabel, originalBootstrapContext, null);

            var claimsIdentitiesToCompare = new List<ClaimsIdentity>()
            {
                claimsIdentityDiffClaims,
                claimsIdentityDiffAuthenticationType,
                claimsIdentityDiffNameType,
                claimsIdentityDiffRoleType,
                claimsIdentityDiffLabel,
                claimsIdentityDiffBootstrapContext,
                claimsIdentityDiffActor,
            };

            foreach (var claimsIdentity in claimsIdentitiesToCompare)
            {
                IdentityComparer.AreEqual(originalClaimsIdentity, claimsIdentity, context);
            }

            // Lists all the properties which should have been marked as different in the compareContext.
            var propertiesToTest = new string[] { "Claims:", "AuthenticationType:", "NameClaimType:", "RoleClaimType:", "Label:", "BootstrapContext:", "Actor:" };

            // Make sure that differences have been found for each of the properties listed above.
            Assert.True(propertiesToTest.ToList().Aggregate(0, (sum, next) => context.Diffs.Contains(next) ? sum + 1 : sum) == 7);
        }

        // Helper method for ClaimsIdentity creation.
        private ClaimsIdentity CreateClaimsIdentity(IEnumerable<Claim> claims, string authenticationType,
            string nameType, string roleType,
            string label, object bootstrapContext, ClaimsIdentity actor)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, authenticationType, nameType, roleType);
            claimsIdentity.Label = label;
            claimsIdentity.BootstrapContext = bootstrapContext;
            claimsIdentity.Actor = actor;

            return claimsIdentity;
        }

        [Fact]
        public void CompareClaimsPrinciples()
        {
            TestUtilities.WriteHeader($"{this}.CompareClaimsPrincipals", true);
            var context = new CompareContext($"{this}.CompareClaimsPrincipals");
            var claimsPrincipal1 = new ClaimsPrincipal(new List<ClaimsIdentity> { new ClaimsIdentity(Guid.NewGuid().ToString()) });
            var claimsPrincipal2 = new ClaimsPrincipal();
            IdentityComparer.AreEqual(claimsPrincipal1, claimsPrincipal2, context);

            Assert.True(context.Diffs.Count(s => s == "Identities:") == 1);
        }

        [Fact]
        public void CompareJArrays()
        {
            TestUtilities.WriteHeader($"{this}.CompareJArrays", true);
            var context = new CompareContext($"{this}.CompareJArrays");
            var jArray1 = new JArray { Guid.NewGuid().ToString() };
            var jArray2 = new JArray { Guid.NewGuid().ToString(), Guid.NewGuid().ToString()};
            IdentityComparer.AreEqual(jArray1, jArray2, context);

            Assert.True(context.Diffs.Count(s => s == "Count:") == 1);
        }

        [Fact]
        public void CompareJsonWebKeys()
        {
            TestUtilities.WriteHeader($"{this}.CompareJsonWebKeys", true);
            var context = new CompareContext($"{this}.CompareJsonWebKeys");
            var jsonWebKey1 = new JsonWebKey { Alg = Guid.NewGuid().ToString() };
            var jsonWebKey2 = new JsonWebKey { Alg = Guid.NewGuid().ToString() };
            IdentityComparer.AreEqual(jsonWebKey1, jsonWebKey2, context);

            Assert.True(context.Diffs.Count(s => s == "Alg:") == 1);
        }

        [Fact]
        public void CompareJsonWebKeySet()
        {
            TestUtilities.WriteHeader($"{this}.CompareJsonWebKeySet", true);
            var context = new CompareContext($"{this}.CompareJsonWebKeySet");
            var jsonWebKeySet1 = new JsonWebKeySet(KeyingMaterial.AADJWKS);
            var jsonWebKeySet2 = new JsonWebKeySet();
            IdentityComparer.AreEqual(jsonWebKeySet1, jsonWebKeySet2, context);

            Assert.True(context.Diffs.Count(s => s == "Keys:") == 1);
        }

        [Fact]
        public void CompareJwtHeaders()
        {
            TestUtilities.WriteHeader($"{this}.CompareJwtHeaders", true);
            var context = new CompareContext($"{this}.CompareJwtHeaders");
            var jwtHeader1 = new JwtHeader(Default.SymmetricSigningCredentials);
            var jwtHeader2 = new JwtHeader();
            IdentityComparer.AreEqual(jwtHeader1, jwtHeader2, context);

            Assert.True(context.Diffs.Count(s => s == "Alg:") == 1);
        }

        [Fact]
        public void CompareJwtPayload()
        {
            TestUtilities.WriteHeader($"{this}.CompareJwtPayload", true);
            var context = new CompareContext($"{this}.CompareJwtPayload");
            var jwtPayload1 = new JwtPayload(ClaimSets.DefaultClaimsAsCreatedInPayload());
            var jwtPayload2 = new JwtPayload();
            IdentityComparer.AreEqual(jwtPayload1, jwtPayload2, context);

            Assert.True(context.Diffs.Count(s => s == "Aud:") == 1);
            Assert.True(context.Diffs.Count(s => s == "Claims:") == 1);
        }

        [Fact]
        public void CompareJwtSecurityTokens()
        {
            TestUtilities.WriteHeader($"{this}.CompareJwtSecurityTokens", true);
            var context = new CompareContext($"{this}.CompareJwtSecurityTokens");
            var jwtSecurityToken1 = new JwtSecurityToken { SigningKey = Default.SymmetricEncryptionKey768 };
            var jwtSecurityToken2 = new JwtSecurityToken { SigningKey = Default.SymmetricEncryptionKey1024 };
            IdentityComparer.AreEqual(jwtSecurityToken1, jwtSecurityToken2, context);

            Assert.True(context.Diffs.Count(s => s == "SigningKey:") == 1);
        }


        [Fact]
        public void CompareJwtSecurityTokenHandlers()
        {
            TestUtilities.WriteHeader($"{this}.CompareJwtSecurityTokenHandlers", true);
            var context = new CompareContext($"{this}.CompareJwtSecurityHandlers");
            var jwtSecurityTokenHandler1 = new JwtSecurityTokenHandler { TokenLifetimeInMinutes = 1 };
            var jwtSecurityTokenHandler2 = new JwtSecurityTokenHandler { TokenLifetimeInMinutes = 2 };
            IdentityComparer.AreEqual(jwtSecurityTokenHandler1, jwtSecurityTokenHandler2, context);

            Assert.True(context.Diffs.Count(s => s == "TokenLifetimeInMinutes:") == 1);
        }

        [Fact]
        public void CompareKeyInfo()
        {
            TestUtilities.WriteHeader($"{this}.CompareKeyInfo", true);
            var context = new CompareContext($"{this}.CompareKeyInfo");
            var keyInfo1 = new KeyInfo(KeyingMaterial.CertSelfSigned2048_SHA256);
            var keyInfo2 = new KeyInfo(KeyingMaterial.Cert_LocalSts);
            IdentityComparer.AreEqual(keyInfo1, keyInfo2, context);

            Assert.True(context.Diffs.Count(s => s == "X509Data:") == 1);
            Assert.True(context.Diffs.Count(s => s == "Certificates:") == 1);
        }

        [Fact]
        public void CompareOpenIdConnectConfigurations()
        {
            TestUtilities.WriteHeader($"{this}.CompareOpenIdConnectConfigurations", true);
            var context = new CompareContext($"{this}.CompareOpenIdConnectConfigurations");
            var config1 = new OpenIdConnectConfiguration { EndSessionEndpoint = Guid.NewGuid().ToString() };
            var config2 = new OpenIdConnectConfiguration { EndSessionEndpoint = Guid.NewGuid().ToString() };
            IdentityComparer.AreEqual(config1, config2, context);

            Assert.True(context.Diffs.Count(s => s == "EndSessionEndpoint:") == 1);
        }

        [Fact]
        public void CompareOpenIdConnectMessages()
        {
            TestUtilities.WriteHeader($"{this}.CompareOpenIdConnectMessages", true);
            var context = new CompareContext($"{this}.CompareOpenIdConnectMessages");
            var message1 = new OpenIdConnectMessage { Prompt = Guid.NewGuid().ToString() };
            var message2 = new OpenIdConnectMessage { Prompt = Guid.NewGuid().ToString() };
            IdentityComparer.AreEqual(message1, message2, context);

            Assert.True(context.Diffs.Count(s => s == "Prompt:") == 1);
        }

        [Fact]
        public void CompareReferences()
        {
            TestUtilities.WriteHeader($"{this}.CompareReferences", true);
            var context = new CompareContext($"{this}.CompareReferences");
            var reference1 = new Reference { Type = Guid.NewGuid().ToString() };
            var reference2 = new Reference { Type = Guid.NewGuid().ToString() };
            IdentityComparer.AreEqual(reference1, reference2, context);

            Assert.True(context.Diffs.Count(s => s == "Type:") == 1);
        }

        [Fact]
        public void CompareRsaSecurityKeys()
        {
            TestUtilities.WriteHeader($"{this}.CompareRsaSecurityKeys", true);
            var context = new CompareContext($"{this}.CompareRsaSecurityKeys");
            var rsaSecurityKey1 = new RsaSecurityKey(KeyingMaterial.RsaParameters1);
            var rsaSecurityKey2 = new RsaSecurityKey(KeyingMaterial.RsaParameters1);
            TestUtilities.SetField(rsaSecurityKey1, "_hasPrivateKey", true);
            TestUtilities.SetField(rsaSecurityKey2, "_hasPrivateKey", false);
            IdentityComparer.AreEqual(rsaSecurityKey1, rsaSecurityKey2, context);

            Assert.True(context.Diffs.Count(s => s == "HasPrivateKey:") == 1);
        }

        [Fact]
        public void CompareRsaParameters()
        {
            TestUtilities.WriteHeader($"{this}.CompareRSAParameters", true);
            var context = new CompareContext($"{this}.CompareRSAParameters");
            var rsaParameters1 = KeyingMaterial.RsaParametersFromPing1;
            var rsaParameters2 = KeyingMaterial.RsaParametersFromPing2;
            IdentityComparer.AreEqual(rsaParameters1, rsaParameters2, context);

            Assert.True(context.Diffs.Count(s => s == "Modulus:") == 1);
        }

        [Fact]
        public void CompareSamlActions()
        {
            TestUtilities.WriteHeader($"{this}.CompareSamlActions", true);
            var context = new CompareContext($"{this}.CompareSamlActions");
            var samlAction1 = new SamlAction(Guid.NewGuid().ToString());
            var samlAction2 = new SamlAction(Guid.NewGuid().ToString());
            IdentityComparer.AreEqual(samlAction1, samlAction2, context);

            Assert.True(context.Diffs.Count(s => s == "Value:") == 1);
        }

        [Fact]
        public void CompareSamlAudienceRestrictionConditions()
        {
            TestUtilities.WriteHeader($"{this}.CompareSamlAudienceRestrictionConditions", true);
            var context = new CompareContext($"{this}.CompareSamlAudienceRestrictionConditions");
            var samlCondition1 = new SamlAudienceRestrictionCondition(new Uri(Default.Audiences.ElementAt(0)));
            var samlCondition2 = new SamlAudienceRestrictionCondition(new Uri(Default.Audiences.ElementAt(1)));
            IdentityComparer.AreEqual(samlCondition1, samlCondition2, context);

            Assert.True(context.Diffs.Count(s => s == "Audiences:") == 1);
        }

        [Fact]
        public void CompareSamlAssertions()
        {
            TestUtilities.WriteHeader($"{this}.CompareSamlAssertions", true);
            var context = new CompareContext($"{this}.CompareSamlAssertions");
            var samlAssertion1 = new SamlAssertion(Guid.NewGuid().ToString(), Default.Issuer, DateTime.Parse(Default.IssueInstantString), null, new SamlAdvice(), new List<SamlStatement> { new SamlAttributeStatement(new SamlSubject(), new List<SamlAttribute> { new SamlAttribute("1", "2", "3") } )});
            var samlAssertion2 = new SamlAssertion(Guid.NewGuid().ToString(), Default.Issuer, DateTime.Parse(Default.IssueInstantString), null, new SamlAdvice(), new List<SamlStatement> { new SamlAttributeStatement(new SamlSubject(), new List<SamlAttribute> { new SamlAttribute("1", "2", "3") }) });
            IdentityComparer.AreEqual(samlAssertion1, samlAssertion2, context);

            Assert.True(context.Diffs.Count(s => s == "AssertionId:") == 1);
        }

        [Fact]
        public void CompareSamlAttribute()
        {
            TestUtilities.WriteHeader($"{this}.CompareSamlAttributes", true);
            var context = new CompareContext($"{this}.CompareSamlAttributes");
            var samlAttribute1 = new SamlAttribute(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            var samlAttribute2 = new SamlAttribute(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            IdentityComparer.AreEqual(samlAttribute1, samlAttribute2, context);

            Assert.True(context.Diffs.Count(s => s == "ClaimType:") == 1);
            Assert.True(context.Diffs.Count(s => s == "Name:") == 1);
            Assert.True(context.Diffs.Count(s => s == "Namespace:") == 1);
            Assert.True(context.Diffs.Count(s => s == "Values:") == 1);
        }

        [Fact]
        public void CompareSamlAttributeStatements()
        {
            TestUtilities.WriteHeader($"{this}.CompareSamlAttributeStatements", true);
            var context = new CompareContext($"{this}.CompareSamlAttributeStatements");
            var samlAttributeStatement1 = new SamlAttributeStatement(new SamlSubject(),
                new List<SamlAttribute> { new SamlAttribute("1", "2", "3") });
            var samlAttributeStatement2 = new SamlAttributeStatement(new SamlSubject(),
                new List<SamlAttribute>());
            IdentityComparer.AreEqual(samlAttributeStatement1, samlAttributeStatement2, context);

            Assert.True(context.Diffs.Count(s => s == "Attributes:") == 1);
        }

        [Fact]
        public void CompareSamlAuthenticationStatements()
        {
            TestUtilities.WriteHeader($"{this}.CompareSamlAuthenticationStatements", true);
            var context = new CompareContext($"{this}.CompareSamlAuthenticationStatements");
            var samlAttributeStatement1 = new SamlAuthenticationStatement(new SamlSubject(),
                Guid.NewGuid().ToString(), DateTime.Parse(Default.AuthenticationInstant), null, null,
                new List<SamlAuthorityBinding>
                {
                    new SamlAuthorityBinding(new System.Xml.XmlQualifiedName(Default.AuthorityKind), Default.Location,
                        Default.Binding)
                });
            var samlAttributeStatement2 = new SamlAuthenticationStatement(new SamlSubject(),
                Guid.NewGuid().ToString(), DateTime.Parse(Default.AuthenticationInstant), null, null,
                new List<SamlAuthorityBinding>
                {
                    new SamlAuthorityBinding(new System.Xml.XmlQualifiedName(Default.AuthorityKind), Default.Location,
                        Default.Binding)
                });
            IdentityComparer.AreEqual(samlAttributeStatement1, samlAttributeStatement2, context);

            Assert.True(context.Diffs.Count(s => s == "AuthenticationMethod:") == 1);
        }

        [Fact]
        public void CompareSamlAuthorityBindings()
        {
            TestUtilities.WriteHeader($"{this}.CompareSamlAuthorityBindings", true);
            var context = new CompareContext($"{this}.CompareSamlAuthorityBindings");
            var samlAuthorityBinding1 = new SamlAuthorityBinding(new System.Xml.XmlQualifiedName(Default.AuthorityKind),
                Guid.NewGuid().ToString(), Default.Binding);
            var samlAuthorityBinding2 = new SamlAuthorityBinding(new System.Xml.XmlQualifiedName(Default.AuthorityKind),
                Guid.NewGuid().ToString(), Default.Binding);
            IdentityComparer.AreEqual(samlAuthorityBinding1, samlAuthorityBinding2, context);

            Assert.True(context.Diffs.Count(s => s == "Binding:") == 1);
        }

        [Fact]
        public void CompareSamlAuthorizationDecisionStatements()
        {
            TestUtilities.WriteHeader($"{this}.CompareSamlAuthorizationDecisionStatements", true);
            var context = new CompareContext($"{this}.CompareSamlAuthorizationDecisionStatements");
            var samlAction = new SamlAction(Guid.NewGuid().ToString());
            var samlAttributeStatement1 =
                new SamlAuthorizationDecisionStatement(new SamlSubject(), 
                    Guid.NewGuid().ToString(), Default.SamlAccessDecision, new List<SamlAction> { samlAction });
            var samlAttributeStatement2 =
                new SamlAuthorizationDecisionStatement(new SamlSubject(), 
                    Guid.NewGuid().ToString(), Default.SamlAccessDecision, new List<SamlAction> { samlAction });
            IdentityComparer.AreEqual(samlAttributeStatement1, samlAttributeStatement2, context);

            Assert.True(context.Diffs.Count(s => s == "Resource:") == 1);
        }

        [Fact]
        public void CompareSamlSecurityTokens()
        {
            TestUtilities.WriteHeader($"{this}.CompareSamlSecurityTokens", true);
            var context = new CompareContext($"{this}.CompareSamlSecurityTokens");
            var samlSecurityToken1 =
                new SamlSecurityToken(new SamlAssertion(Guid.NewGuid().ToString(), Default.Issuer,
                    DateTime.Parse(Default.IssueInstantString), null, new SamlAdvice(),
                    new List<SamlStatement>
                    {
                        new SamlAttributeStatement(new SamlSubject(),
                            new List<SamlAttribute> {new SamlAttribute("1", "2", "3")})
                    }));
            var samlSecurityToken2 = 
                new SamlSecurityToken(new SamlAssertion(Guid.NewGuid().ToString(), Default.Issuer,
                    DateTime.Parse(Default.IssueInstantString), null, new SamlAdvice(),
                    new List<SamlStatement>
                    {
                        new SamlAttributeStatement(new SamlSubject(),
                            new List<SamlAttribute> {new SamlAttribute("1", "2", "3")})
                    }));
            IdentityComparer.AreEqual(samlSecurityToken1, samlSecurityToken2, context);

            Assert.True(context.Diffs.Count(s => s == "Assertion:") == 1);
            Assert.True(context.Diffs.Count(s => s == "AssertionId:") == 1);
            Assert.True(context.Diffs.Count(s => s == "Id:") == 1);
        }

        [Fact]
        public void CompareSaml2SecurityTokens()
        {
            TestUtilities.WriteHeader($"{this}.CompareSaml2SecurityTokens", true);
            var context = new CompareContext($"{this}.CompareSaml2SecurityTokens");
            var saml2SecurityToken1 = new Saml2SecurityToken(new Saml2Assertion(new Saml2NameIdentifier(Guid.NewGuid().ToString())));
            var saml2SecurityToken2 = new Saml2SecurityToken(new Saml2Assertion(new Saml2NameIdentifier(Guid.NewGuid().ToString())));
            IdentityComparer.AreEqual(saml2SecurityToken1, saml2SecurityToken2, context);
            Assert.True(context.Diffs.Count(s => s == "Id:") == 2);
            Assert.True(context.Diffs.Count(s => s == "Issuer:") == 2);
        }

        [Fact]
        public void CompareSignatures()
        {
            TestUtilities.WriteHeader($"{this}.CompareSignatures", true);
            var context = new CompareContext($"{this}.CompareSignatures");
            var signature1 = new Signature {SignatureValue = Guid.NewGuid().ToString() };
            var signature2 = new Signature {SignatureValue = Guid.NewGuid().ToString()};
            IdentityComparer.AreEqual(signature1, signature2, context);

            Assert.True(context.Diffs.Count(s => s == "SignatureValue:") == 1);
        }

        [Fact]
        public void CompareSignedInfo()
        {
            TestUtilities.WriteHeader($"{this}.CompareSignedInfo", true);
            var context = new CompareContext($"{this}.CompareSignedInfo");
            var signedInfo1 = new SignedInfo { SignatureMethod = Guid.NewGuid().ToString() };
            var signedInfo2 = new SignedInfo { SignatureMethod = Guid.NewGuid().ToString() };
            IdentityComparer.AreEqual(signedInfo1, signedInfo2, context);

            Assert.True(context.Diffs.Count(s => s == "SignatureMethod:") == 1);
        }

        [Fact]
        public void CompareSigningCredentials()
        {
            TestUtilities.WriteHeader($"{this}.CompareSigningCredentials", true);
            var context = new CompareContext($"{this}.CompareSigningCredentials");
            var signingCredentials1 = new SigningCredentials(KeyingMaterial.DefaultX509Key_2048, Guid.NewGuid().ToString());
            var signingCredentials2 = new SigningCredentials(KeyingMaterial.DefaultX509Key_2048, Guid.NewGuid().ToString());
            IdentityComparer.AreEqual(signingCredentials1, signingCredentials2, context);

            Assert.True(context.Diffs.Count(s => s == "Algorithm:") == 1);
        }

        [Fact]
        public void CompareStrings()
        {
            TestUtilities.WriteHeader($"{this}.CompareStrings", true);
            var context = new CompareContext($"{this}.CompareStrings");
            var string1 = "hello";
            var string2 = "goodbye";
            IdentityComparer.AreEqual(string1, string2, context);

            Assert.True(context.Diffs.Count(s => s == "str1 != str2, StringComparison: 'Ordinal'") == 1);
            Assert.True(context.Diffs[1] == string1);
            Assert.True(context.Diffs[2] == string2);
        }

        [Fact]
        public void CompareSymmetricSecurityKeys()
        {
            TestUtilities.WriteHeader($"{this}.CompareSymmetricSecurityKeys", true);
            var context = new CompareContext($"{this}.CompareSymmetricSecurityKeys");
            var symmetricSecurityKey1 = new SymmetricSecurityKey(KeyingMaterial.SymmetricKeyBytes2_256);
            var symmetricSecurityKey2 = new SymmetricSecurityKey(KeyingMaterial.SymmetricKeyBytes2_128);
            IdentityComparer.AreEqual(symmetricSecurityKey1, symmetricSecurityKey2, context);

            Assert.True(context.Diffs.Count(s => s == "KeySize:") == 1);
            Assert.True(context.Diffs.Count(s => s == "Key:") == 1);
        }

        [Fact]
        public void CompareTokenValidationParameters()
        {
            TestUtilities.WriteHeader($"{this}.CompareTokenValidationParameters", true);
            var context = new CompareContext($"{this}.CompareTokenValidationParameters");
            var tokenValidationParameters1 =
                new TokenValidationParameters {AuthenticationType = Guid.NewGuid().ToString()};
            var tokenValidationParameters2 =
                new TokenValidationParameters() {AuthenticationType = Guid.NewGuid().ToString()};
            IdentityComparer.AreEqual(tokenValidationParameters1, tokenValidationParameters2, context);

            Assert.True(context.Diffs.Count(s => s == "AuthenticationType:") == 1);
        }

        [Fact]
        public void CompareWsFederationConfiguration()
        {
            TestUtilities.WriteHeader($"{this}.CompareWsFederationConfiguration", true);
            var context = new CompareContext($"{this}.CompareWsFederationConfiguration");
            var config1 = new WsFederationConfiguration { TokenEndpoint = Guid.NewGuid().ToString() };
            var config2 = new WsFederationConfiguration { TokenEndpoint = Guid.NewGuid().ToString() };
            IdentityComparer.AreEqual(config1, config2, context);

            Assert.True(context.Diffs.Count(s => s == "TokenEndpoint:") == 1);
        }

        [Fact]
        public void CompareWsFederationMessages()
        {
            TestUtilities.WriteHeader($"{this}.CompareWsFederationMessages", true);
            var context = new CompareContext($"{this}.CompareWsFederationMessages");
            var message1 = new WsFederationMessage { Wa = Guid.NewGuid().ToString() };
            var message2 = new WsFederationMessage { Wa = Guid.NewGuid().ToString() };
            IdentityComparer.AreEqual(message1, message2, context);

            Assert.True(context.Diffs.Count(s => s == "Wa:") == 1);
        }

        [Fact]
        public void CompareX509Certificate2()
        {
            TestUtilities.WriteHeader($"{this}.CompareX509Certificate2", true);

            var context = new CompareContext($"{this}.CompareX509Certificate2");
            var certificate = new X509Certificate2(Convert.FromBase64String(KeyingMaterial.DefaultX509Data_2048_Public));
            var certificateSame = new X509Certificate2(Convert.FromBase64String(KeyingMaterial.DefaultX509Data_2048_Public));
            var certificateDifferent = KeyingMaterial.CertSelfSigned1024_SHA256;

            IdentityComparer.AreEqual(certificate, certificateSame, context);
            Assert.True(context.Diffs.Count(s => s == "X509Certificate2:") == 0);

            IdentityComparer.AreEqual(certificate, certificateDifferent, context);
            Assert.True(context.Diffs.Count(s => s == "X509Certificate2:") == 1);

            context.Diffs.Clear();
            IdentityComparer.AreX509Certificate2Equal(certificate, null, context);
            Assert.True(context.Diffs.Count(s => s.Contains("X509Certificate2:")) == 1);

            context.Diffs.Clear();
            IdentityComparer.AreX509Certificate2Equal(null, certificate, context);
            Assert.True(context.Diffs.Count(s => s.Contains("X509Certificate2:")) == 1);
        }
    }
}
