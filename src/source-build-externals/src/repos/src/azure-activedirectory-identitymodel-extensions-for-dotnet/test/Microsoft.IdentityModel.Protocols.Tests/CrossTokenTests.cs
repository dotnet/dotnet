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
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace Microsoft.IdentityModel.Protcols.Test
{
    /// <summary>
    /// The purpose of these tests are to ensure that Saml, Saml2 and Jwt handling 
    /// results in the same exceptions, claims etc.
    /// </summary>
    public class CrossTokenTests
    {
        [Fact]
        public void CrossToken_ValidateToken()
        {
            JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();
            Saml2TokenHandler saml2Handler = new Saml2TokenHandler();
            SamlTokenHandler samlHandler = new SamlTokenHandler();

            JwtSecurityTokenHandler.InboundClaimFilter.Add("aud");
            JwtSecurityTokenHandler.InboundClaimFilter.Add("exp");
            JwtSecurityTokenHandler.InboundClaimFilter.Add("iat");
            JwtSecurityTokenHandler.InboundClaimFilter.Add("iss");
            JwtSecurityTokenHandler.InboundClaimFilter.Add("nbf");

            string jwtToken = IdentityUtilities.CreateJwtToken(IdentityUtilities.DefaultAsymmetricSecurityTokenDescriptor, jwtHandler);

            // saml tokens created using Microsoft.IdentityModel.Extensions
            string imSaml2Token = IdentityUtilities.CreateSaml2Token(IdentityUtilities.DefaultAsymmetricSecurityTokenDescriptor, imSaml2Handler);
            string imSamlToken = IdentityUtilities.CreateSamlToken(IdentityUtilities.DefaultAsymmetricSecurityTokenDescriptor, imSamlHandler);

            // saml tokens created using System.IdentityModel.Tokens
            string smSaml2Token = IdentityUtilities.CreateSaml2Token(IdentityUtilities.DefaultAsymmetricSecurityTokenDescriptor, smSaml2Handler);
            string smSamlToken = IdentityUtilities.CreateSamlToken(IdentityUtilities.DefaultAsymmetricSecurityTokenDescriptor, smSamlHandler);

            ClaimsPrincipal jwtPrincipal = ValidateToken(jwtToken, IdentityUtilities.DefaultAsymmetricTokenValidationParameters, jwtHandler, ExpectedException.NoExceptionExpected);
            ClaimsPrincipal imSaml2Principal = ValidateToken(imSaml2Token, IdentityUtilities.DefaultAsymmetricTokenValidationParameters, imSaml2Handler, ExpectedException.NoExceptionExpected);
            ClaimsPrincipal imSamlPrincipal = ValidateToken(imSamlToken, IdentityUtilities.DefaultAsymmetricTokenValidationParameters, imSamlHandler, ExpectedException.NoExceptionExpected);
            ClaimsPrincipal smSaml2Principal = ValidateToken(smSaml2Token, IdentityUtilities.DefaultAsymmetricTokenValidationParameters, imSaml2Handler, ExpectedException.NoExceptionExpected);
            ClaimsPrincipal smSamlPrincipal = ValidateToken(smSamlToken, IdentityUtilities.DefaultAsymmetricTokenValidationParameters, imSamlHandler, ExpectedException.NoExceptionExpected);

            Assert.IsTrue(IdentityComparer.AreEqual<ClaimsPrincipal>(imSamlPrincipal, imSaml2Principal, new CompareContext { IgnoreSubject = true }));
            Assert.IsTrue(IdentityComparer.AreEqual<ClaimsPrincipal>(smSamlPrincipal, imSaml2Principal, new CompareContext { IgnoreSubject = true }));
            Assert.IsTrue(IdentityComparer.AreEqual<ClaimsPrincipal>(smSaml2Principal, imSaml2Principal, new CompareContext { IgnoreSubject = true }));

            // false = ignore type of objects, we expect all objects in the principal to be of same type (no derived types)
            // true = ignore subject, claims have a backpointer to their ClaimsIdentity.  Most of the time this will be different as we are comparing two different ClaimsIdentities.
            // true = ignore properties of claims, any mapped claims short to long for JWT's will have a property that represents the short type.
            Assert.IsTrue(IdentityComparer.AreEqual<ClaimsPrincipal>(jwtPrincipal, imSaml2Principal, new CompareContext { IgnoreType = false, IgnoreSubject = true, IgnoreProperties = true }));

            JwtSecurityTokenHandler.InboundClaimFilter.Clear();
        }

        private ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, ISecurityTokenValidator tokenValidator, ExpectedException expectedException)
        {
            ClaimsPrincipal princiapl = null;
            try
            {
                SecurityToken validatedToken;
                princiapl = tokenValidator.ValidateToken(securityToken, validationParameters, out validatedToken);
                expectedException.ProcessNoException();
            }
            catch (Exception exception)
            {
                expectedException.ProcessException(exception);
            }

            return princiapl;
        }


        [Fact]
        public void CrossToken_ValidateSignature()
        {
            // ensure jwt, saml1 and saml2 work the same
        }

        [Fact]
        public void CrossToken_ValidateAudience()
        {
            // ensure jwt, saml1 and saml2 work the same
        }

        [Fact]
        public void CrossToken_ValidateIssuer()
        {
            // ensure jwt, saml1 and saml2 work the same
        }

        [Fact]
        public void CrossToken_ValidateLifetime()
        {
            // ensure jwt, saml1 and saml2 work the same
        }
    }
}
