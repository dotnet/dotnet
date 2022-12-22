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

            // It's possible that DateTime.UtcNow will be slightly different from token.Assertion.IssueInstant, so we can't compare them directly. 
            var timeDiff = DateTime.UtcNow.Subtract(token.Assertion.IssueInstant).TotalMilliseconds;
            if (Math.Abs(timeDiff) >= 100)
                context.Diffs.Add("Math.Abs(DateTime.UtcNow.Subtract(token.Assertion.IssueInstant).TotalMilliseconds) >= 100");

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
