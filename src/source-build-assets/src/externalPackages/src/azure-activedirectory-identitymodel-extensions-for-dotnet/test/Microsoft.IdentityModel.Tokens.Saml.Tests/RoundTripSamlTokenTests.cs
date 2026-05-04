// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens.Saml2;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Saml.Tests
{
    /// <summary>
    /// 
    /// </summary>
    public class RoundTripSamlTokenTests
    {

        [Theory, MemberData(nameof(RoundTripTokenTheoryData))]
        public void RoundTripTokens(SamlRoundTripTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.RoundTripTokens", theoryData);
            if (theoryData.PropertiesToIgnoreWhenComparing.Count > 0)
                throw new TestException("This test should not ignore any properties.");

            try
            {
                var samlToken = theoryData.Handler.CreateToken(theoryData.TokenDescriptor);
                var token = theoryData.Handler.WriteToken(samlToken);
                var principal = theoryData.Handler.ValidateToken(token, theoryData.ValidationParameters, out SecurityToken validatedToken);
                ClaimsPrincipal principal2 = null;
                ClaimsPrincipal principal3 = null;
                ClaimsPrincipal principal4 = null;
                SecurityToken validatedToken2 = null;
                SecurityToken validatedToken3 = null;
                SecurityToken validatedToken4 = null;

                if (theoryData.Handler is SamlSecurityTokenHandler samlTokenHandler)
                {
                    principal2 = samlTokenHandler.ValidateToken(XmlUtilities.CreateXmlReader(token), theoryData.ValidationParameters, out validatedToken2);
                    principal3 = samlTokenHandler.ValidateToken(XmlUtilities.CreateDictionaryReader(token), theoryData.ValidationParameters, out validatedToken3);
                    principal4 = theoryData.Handler.ValidateToken((validatedToken as SamlSecurityToken).Assertion.CanonicalString, theoryData.ValidationParameters, out validatedToken4);
                }

                if (theoryData.Handler is Saml2SecurityTokenHandler saml2TokenHandler)
                {
                    principal2 = saml2TokenHandler.ValidateToken(XmlUtilities.CreateXmlReader(token), theoryData.ValidationParameters, out validatedToken2);
                    principal3 = saml2TokenHandler.ValidateToken(XmlUtilities.CreateDictionaryReader(token), theoryData.ValidationParameters, out validatedToken3);
                    principal4 = theoryData.Handler.ValidateToken((validatedToken as Saml2SecurityToken).Assertion.CanonicalString, theoryData.ValidationParameters, out validatedToken4);
                }

                // ensure the SamlAssertion.CanonicalString can be validated needed for OnBehalfOf flows.
                var principal5 = theoryData.Handler.ValidateToken((principal.Identity as ClaimsIdentity).BootstrapContext as string, theoryData.ValidationParameters, out SecurityToken validatedToken5);

                IdentityComparer.AreEqual(validatedToken, validatedToken2, context);
                IdentityComparer.AreEqual(validatedToken, validatedToken3, context);
                IdentityComparer.AreEqual(validatedToken, validatedToken4, context);
                IdentityComparer.AreEqual(validatedToken, validatedToken5, context);

                IdentityComparer.AreEqual(principal, principal2, context);
                IdentityComparer.AreEqual(principal, principal3, context);
                IdentityComparer.AreEqual(principal, principal4, context);
                IdentityComparer.AreEqual(principal, principal5, context);

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SamlRoundTripTheoryData> RoundTripTokenTheoryData
        {
            get
            {
                var theoryData = new TheoryData<SamlRoundTripTheoryData>();
                foreach (var item in GetRoundTripTests(new Saml2SecurityTokenHandler()))
                    theoryData.Add(item);

                foreach (var item in GetRoundTripTests(new SamlSecurityTokenHandler()))
                    theoryData.Add(item);

                return theoryData;
            }
        }

        public static IEnumerable<SamlRoundTripTheoryData> GetRoundTripTests(SecurityTokenHandler tokenHandler)
        {
            return new List<SamlRoundTripTheoryData>
            {
                new SamlRoundTripTheoryData(tokenHandler)
                {
                    First = true,
                    TestId = nameof(Default.ClaimsIdentity),
                    TokenDescriptor = new SecurityTokenDescriptor
                    {
                        Expires = DateTime.UtcNow + TimeSpan.FromDays(1),
                        Audience = Default.Audience,
                        SigningCredentials = Default.AsymmetricSigningCredentials,
                        Issuer = Default.Issuer,
                        Subject = Default.SamlClaimsIdentity
                    },
                    ValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = Default.AsymmetricSigningKey,
                        SaveSigninToken = true,
                        ValidAudience = Default.Audience,
                        ValidIssuer = Default.Issuer,
                    }
                },
                new SamlRoundTripTheoryData(tokenHandler)
                {
                    TestId = nameof(Default.ClaimsIdentity) + nameof(KeyingMaterial.RsaSigningCreds_2048),
                    TokenDescriptor = new SecurityTokenDescriptor
                    {
                        Expires = DateTime.UtcNow + TimeSpan.FromDays(1),
                        Audience = Default.Audience,
                        SigningCredentials = KeyingMaterial.RsaSigningCreds_2048,
                        Issuer = Default.Issuer,
                        Subject = Default.SamlClaimsIdentity
                    },
                    ValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = KeyingMaterial.RsaSigningCreds_2048_Public.Key,
                        SaveSigninToken = true,
                        ValidAudience = Default.Audience,
                        ValidIssuer = Default.Issuer,
                    },
                },
                new SamlRoundTripTheoryData(tokenHandler)
                {
                    TestId = nameof(Default.ClaimsIdentity) + nameof(KeyingMaterial.RsaSigningCreds_2048_FromRsa),
                    TokenDescriptor = new SecurityTokenDescriptor
                    {
                        Expires = DateTime.UtcNow + TimeSpan.FromDays(1),
                        Audience = Default.Audience,
                        SigningCredentials = KeyingMaterial.RsaSigningCreds_2048_FromRsa,
                        Issuer = Default.Issuer,
                        Subject = Default.SamlClaimsIdentity
                    },
                    ValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = KeyingMaterial.RsaSigningCreds_2048_FromRsa_Public.Key,
                        SaveSigninToken = true,
                        ValidAudience = Default.Audience,
                        ValidIssuer = Default.Issuer,
                    },
                },
                new SamlRoundTripTheoryData(tokenHandler)
                {
                    TestId = nameof(Default.ClaimsIdentity) + nameof(KeyingMaterial.JsonWebKeyRsa256SigningCredentials),
                    TokenDescriptor = new SecurityTokenDescriptor
                    {
                        Expires = DateTime.UtcNow + TimeSpan.FromDays(1),
                        Audience = Default.Audience,
                        SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                        Issuer = Default.Issuer,
                        Subject = Default.SamlClaimsIdentity
                    },
                    ValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256PublicSigningCredentials.Key,
                        SaveSigninToken = true,
                        ValidAudience = Default.Audience,
                        ValidIssuer = Default.Issuer,
                    }
                }
            };
        }
    }

    public class SamlRoundTripTheoryData : TokenTheoryData
    {
        public SamlRoundTripTheoryData(SecurityTokenHandler tokenHandler)
        {
            Handler = tokenHandler;
        }

        public SecurityTokenHandler Handler { get; }

        public override string ToString()
        {
            return $"{TestId}, {ExpectedException}";
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
