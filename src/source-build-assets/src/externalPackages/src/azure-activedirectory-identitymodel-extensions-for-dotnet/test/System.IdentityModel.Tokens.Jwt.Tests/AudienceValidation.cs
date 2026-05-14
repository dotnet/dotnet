// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

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
