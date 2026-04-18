// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Security.Claims;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

namespace Microsoft.IdentityModel.Tokens.Saml.Tests
{
    public class SamlSecurityTokenTests
    {
        /// <summary>
        /// Tests that the values set by the default constructor haven't changed.
        /// </summary>
        [Fact]
        public void Defaults()
        {
            var context = new CompareContext();
            var token = new SamlSecurityTokenPublic();

            if (token.Assertion == null)
                context.Diffs.Add("token.Assertion == null");

            if (token.Assertion.AssertionId == null)
                context.Diffs.Add("token.AssertionId == null");

            if (!token.Assertion.Issuer.Equals(ClaimsIdentity.DefaultIssuer))
                context.Diffs.Add("token.Assertion.Issuer.Equals(ClaimsIdentity.DefaultIssuer)");

            if (token.Assertion.Conditions == null)
                context.Diffs.Add("token.Assertion.Conditions == null");

            if (token.Assertion.Advice == null)
                context.Diffs.Add("token.Assertion.Advice == null");

            if (token.Assertion.Statements == null)
                context.Diffs.Add("token.Assertion.Statements == null");

            if (token.Id == null)
                context.Diffs.Add("token.Id ==  null");

            if (token.Issuer == null)
                context.Diffs.Add("token.Issuer == null");

            if (token.SecurityKey != null)
                context.Diffs.Add("token.SecurityKey != null");

            if (token.SigningKey != null)
                context.Diffs.Add("token.SigningKey != null");

            if (!token.ValidFrom.Equals(DateTimeUtil.GetMinValue(DateTimeKind.Utc)))
                context.Diffs.Add("!token.ValidFrom.Equals(DateTimeUtil.GetMinValue(DateTimeKind.Utc))");

            if (!token.ValidTo.Equals(DateTimeUtil.GetMaxValue(DateTimeKind.Utc)))
                context.Diffs.Add("DateTimeUtil.GetMaxValue(DateTimeKind.Utc)");

            TestUtilities.AssertFailIfErrors(context);
        }

        private class SamlSecurityTokenPublic : SamlSecurityToken
        {
            public SamlSecurityTokenPublic() : base()
            {
            }
        }
    }
}
