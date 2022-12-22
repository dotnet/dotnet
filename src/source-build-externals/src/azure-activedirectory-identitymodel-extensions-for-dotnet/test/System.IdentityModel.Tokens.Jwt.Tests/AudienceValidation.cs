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

using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace System.IdentityModel.Tokens.Jwt.Tests
{
    public class AudienceValidationTests
    {
        [Fact]
        public void Variations()
        {
            var context = new CompareContext { IgnoreType = true };
            RunAudienceVariation(ClaimSets.MultipleAudiences(), Default.Audiences, context);
            RunAudienceVariation(ClaimSets.SingleAudience(), new List<string> { Default.Audience }, context);

            TestUtilities.AssertFailIfErrors("AudienceValidation: ", context.Diffs);
        }

        private void RunAudienceVariation(List<Claim> audienceClaims, List<string> expectedAudiences, CompareContext context)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenDescriptor = Default.AsymmetricSignSecurityTokenDescriptor(audienceClaims);
            tokenDescriptor.Audience = null;
            var jwt = handler.CreateEncodedJwt(tokenDescriptor);

            SecurityToken token = null;
            var claimsPrincipal = handler.ValidateToken(jwt, Default.AsymmetricSignTokenValidationParameters, out token);
            var jwtToken = token as JwtSecurityToken;
            var audiences = jwtToken.Audiences;

            IdentityComparer.AreEqual(audiences, expectedAudiences as IEnumerable<string>, context);

            ClaimsIdentity identity = claimsPrincipal.Identity as ClaimsIdentity;
            IdentityComparer.AreEqual(identity.FindAll(JwtRegisteredClaimNames.Aud), audienceClaims.AsReadOnly(), context);
        }
    }
}
