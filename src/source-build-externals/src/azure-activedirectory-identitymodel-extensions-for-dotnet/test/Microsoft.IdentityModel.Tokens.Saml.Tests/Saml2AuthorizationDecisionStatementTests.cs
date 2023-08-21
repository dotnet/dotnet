// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Xunit;
using Microsoft.IdentityModel.Tokens.Saml2;

namespace Microsoft.IdentityModel.Tokens.Saml.Tests
{
    public class Saml2AuthorizationDecisionStatementTests
    {
        [Fact]
        public void Saml2AuthorizationDecisionStatement_RelativeClassReference_ArgumentException()
        {
            var resouce = new Uri("resource", UriKind.Relative);
            Assert.Throws<ArgumentException>(() => new Saml2AuthorizationDecisionStatement(resouce, Saml2Constants.AccessDecision.Permit));
        }

        [Fact]
        public void Saml2AuthorizationDecisionStatement_NullClassReference_ArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => new Saml2AuthorizationDecisionStatement(null, Saml2Constants.AccessDecision.Permit));
        }

        [Fact]
        public void Saml2AuthorizationDecisionStatement_AbsoluteClassReference_NoException()
        {
            var resouce = new Uri("http://resource", UriKind.Absolute);
            new Saml2AuthorizationDecisionStatement(resouce, Saml2Constants.AccessDecision.Permit);
        }

    }
}
