// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens.Saml2;

namespace Microsoft.IdentityModel.Tokens.Saml2.Tests
{
    public class Saml2TheoryData : TokenTheoryData
    {
        public Saml2TheoryData()
        {
        }

        public Saml2TheoryData(string testId) : base(testId)
        {
        }

        public Saml2TheoryData(TokenTheoryData tokenTheoryData)
            : base(tokenTheoryData)
        {
        }

        public string Xml { get; set; }

        public Saml2Action Action { get; set; }

        public Saml2Advice Advice { get; set; }

        public Saml2Assertion Assertion { get; set; }

        public Saml2Attribute Attribute { get; set; }

        public List<Saml2Attribute> Attributes { get; set; }

        public Saml2AttributeStatement AttributeStatement { get; set; }

        public Saml2AudienceRestriction AudienceRestriction { get; set; }

        public Saml2AuthenticationStatement AuthenticationStatement { get; set; }

        public Saml2AuthorizationDecisionStatement AuthorizationDecision { get; set; }

        public Saml2Conditions Conditions{ get; set; }

        public List<Saml2Attribute> ConsolidatedAttributes { get; set; }

        public Saml2Evidence Evidence { get; set; }

        public Saml2SecurityTokenHandler Handler { get; set; } = new Saml2SecurityTokenHandlerPublic();

        public string InclusiveNamespacesPrefixList { get; set; }

        public Saml2Serializer Saml2Serializer { get; set; } = new Saml2Serializer();

        public Saml2Subject Subject { get; set; }

        public Saml2ProxyRestriction ProxyRestriction { get; set; }
    }
}
