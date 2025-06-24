// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens.Saml;
using Microsoft.IdentityModel.Tokens.Saml2;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Tests
{
    /// <summary>
    /// The purpose of these tests are to ensure that Saml, Saml2 and Jwt handling
    /// results in the same exceptions, claims etc.
    /// </summary>
    public class CrossTokenTests
    {
        [Theory, MemberData(nameof(CrossTokenValidateTokenTheoryData))]
        public void CrossTokenValidateToken(CrossTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CrossTokenValidateToken", theoryData);

            try
            {
                var samlToken = IdentityUtilities.CreateEncodedSaml(theoryData.SecurityTokenDescriptor, theoryData.SamlTokenHandler);
                var saml2Token = IdentityUtilities.CreateEncodedSaml2(theoryData.SecurityTokenDescriptor, theoryData.Saml2TokenHandler);
                var jwtToken = IdentityUtilities.CreateEncodedJwt(theoryData.SecurityTokenDescriptor, theoryData.JwtTokenHandler);

                var samlPrincipal = theoryData.SamlTokenHandler.ValidateToken(samlToken, theoryData.TokenValidationParameters, out SecurityToken samlValidatedToken);
                var saml2Principal = theoryData.Saml2TokenHandler.ValidateToken(saml2Token, theoryData.TokenValidationParameters, out SecurityToken saml2ValidatedToken);
                var jwtPrincipal = theoryData.JwtTokenHandler.ValidateToken(jwtToken, theoryData.TokenValidationParameters, out SecurityToken jwtValidatedToken);

                // false = ignore type of objects, we expect all objects in the principal to be of same type (no derived types)
                context.IgnoreType = false;
                IdentityComparer.AreEqual(samlPrincipal, saml2Principal, context);

                // true = ignore properties of claims, any mapped claims short to long for JWT's will have a property that represents the short type.
                context.IgnoreProperties = true;
                IdentityComparer.AreEqual(samlPrincipal, jwtPrincipal, context);
                IdentityComparer.AreEqual(saml2Principal, jwtPrincipal, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CrossTokenTheoryData> CrossTokenValidateTokenTheoryData
        {
            get
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var samlTokenHandler = new SamlSecurityTokenHandler();
                var saml2TokenHandler = new Saml2SecurityTokenHandler();
                jwtTokenHandler.InboundClaimFilter.Add("aud");
                jwtTokenHandler.InboundClaimFilter.Add("exp");
                jwtTokenHandler.InboundClaimFilter.Add("iat");
                jwtTokenHandler.InboundClaimFilter.Add("iss");
                jwtTokenHandler.InboundClaimFilter.Add("nbf");

                return new TheoryData<CrossTokenTheoryData>
                {
                    new CrossTokenTheoryData
                    {
                        First = true,
                        JwtTokenHandler = jwtTokenHandler,
                        SamlTokenHandler = samlTokenHandler,
                        Saml2TokenHandler = saml2TokenHandler,
                        TestId = "AsymmetricSignToken",
                        SecurityTokenDescriptor = Default.SecurityTokenDescriptor(null, Default.AsymmetricSigningCredentials, Default.SamlClaimsIssuerEqOriginalIssuer),
                        TokenValidationParameters =  Default.AsymmetricSignTokenValidationParameters
                    }
                };
            }
        }

        [Theory, MemberData(nameof(CanReadTokenTheoryData))]
        public void CanReadToken(TokenHandlerTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CanReadToken", theoryData);
            try
            {
                var tokenHandler = theoryData.TokenHandler;

                if (tokenHandler is SecurityTokenHandler securityTokenHandler)
                {
                    if (securityTokenHandler.CanReadToken(theoryData.Token) != theoryData.CanReadToken)
                        context.AddDiff("securityTokenHandler.CanReadToken(theoryData.Token) != theoryData.CanReadToken");
                }
                else if (tokenHandler is JsonWebTokenHandler jsonWebTokenHandler)
                {
                    if (jsonWebTokenHandler.CanReadToken(theoryData.Token) != theoryData.CanReadToken)
                        context.AddDiff("jsonWebTokenHandler.CanReadToken(theoryData.Token) != theoryData.CanReadToken");
                }
                else
                {
                    throw new Exception("Unable to cast TokenHandler");
                }

                theoryData.ExpectedException.ProcessNoException();
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<TokenHandlerTheoryData> CanReadTokenTheoryData
        {
            get
            {
                var largeToken = GenerateTokenLargerThanAllowed();

                return new TheoryData<TokenHandlerTheoryData>
                {
                    new TokenHandlerTheoryData
                    {
                        First = true,
                        TokenHandler = new JwtSecurityTokenHandler(),
                        Token = Default.AsymmetricJwt,
                        CanReadToken = true,
                        ExpectedException = ExpectedException.NoExceptionExpected,
                        TestId = "ValidJwt"
                    },
                    new TokenHandlerTheoryData
                    {
                        TokenHandler = new JwtSecurityTokenHandler(),
                        Token = largeToken,
                        CanReadToken = false,
                        ExpectedException = ExpectedException.NoExceptionExpected,
                        TestId = "TokenTooLargeJwt"
                    },
                    new TokenHandlerTheoryData
                    {
                        TokenHandler = new JsonWebTokenHandler(),
                        Token = Default.AsymmetricJwt,
                        CanReadToken = true,
                        ExpectedException = ExpectedException.NoExceptionExpected,
                        TestId = "ValidJsonWebToken"
                    },
                    new TokenHandlerTheoryData
                    {
                        TokenHandler = new JsonWebTokenHandler(),
                        Token = largeToken,
                        CanReadToken = false,
                        ExpectedException = ExpectedException.NoExceptionExpected,
                        TestId = "TokenTooLargeJsonWebToken"
                    },
                    new TokenHandlerTheoryData
                    {
                        TokenHandler = new SamlSecurityTokenHandler(),
                        Token = ReferenceTokens.SamlToken_Valid,
                        CanReadToken = true,
                        ExpectedException = ExpectedException.NoExceptionExpected,
                        TestId = "ValidSaml1"
                    },
                    new TokenHandlerTheoryData
                    {
                        TokenHandler = new SamlSecurityTokenHandler(),
                        Token = largeToken,
                        CanReadToken = false,
                        ExpectedException = ExpectedException.NoExceptionExpected,
                        TestId = "TokenTooLargeSaml1"
                    },
                    new TokenHandlerTheoryData
                    {
                        TokenHandler = new Saml2SecurityTokenHandler(),
                        Token = ReferenceTokens.Saml2Token_Valid,
                        CanReadToken = true,
                        ExpectedException = ExpectedException.NoExceptionExpected,
                        TestId = "ValidSaml2"
                    },
                    new TokenHandlerTheoryData
                    {
                        TokenHandler = new Saml2SecurityTokenHandler(),
                        Token = largeToken,
                        CanReadToken = false,
                        ExpectedException = ExpectedException.NoExceptionExpected,
                        TestId = "TokenTooLargeSaml2"
                    },
                };
            }
        }

        private static string GenerateTokenLargerThanAllowed()
        {
            byte[] buffer = new byte[(TokenValidationParameters.DefaultMaximumTokenSizeInBytes)];
            Random r = new Random();
            r.NextBytes(buffer);
            return Convert.ToBase64String(buffer);
        }
    }

    public class CrossTokenTheoryData : TheoryDataBase
    {
        public JwtSecurityTokenHandler JwtTokenHandler { get; set; }
        public SamlSecurityTokenHandler SamlTokenHandler { get; set; }
        public Saml2SecurityTokenHandler Saml2TokenHandler { get; set; }
        public SecurityTokenDescriptor SecurityTokenDescriptor { get; set; }
        public TokenValidationParameters TokenValidationParameters { get; set; }
    }

    public class TokenHandlerTheoryData : TheoryDataBase
    {
        public bool CanReadToken { get; set; }

        public TokenHandler TokenHandler { get; set; }

        public string Token { get; set; }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
