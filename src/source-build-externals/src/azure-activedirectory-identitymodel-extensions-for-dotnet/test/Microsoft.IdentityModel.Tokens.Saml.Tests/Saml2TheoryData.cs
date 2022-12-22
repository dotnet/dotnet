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
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens.Saml2;

namespace Microsoft.IdentityModel.Tokens.Saml2.Tests
{
    public class Saml2TheoryData : TokenTheoryData
    {
        public Saml2TheoryData()
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
