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

using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Xml;

namespace Microsoft.IdentityModel.Tokens.Saml.Tests
{
    public class SamlTheoryData : TokenTheoryData
    {
        public SamlTheoryData()
        {
        }

        public SamlTheoryData(TokenTheoryData tokenTheoryData)
            : base(tokenTheoryData)
        {
        }

        public SamlActionTestSet ActionTestSet { get; set; }

        public SamlAdviceTestSet AdviceTestSet { get; set; }

        public SamlAssertionTestSet AssertionTestSet { get; set; }

        public SamlAttributeTestSet AttributeTestSet { get; set; }

        public SamlAttributeStatementTestSet AttributeStatementTestSet { get; set; }

        public SamlAudienceRestrictionConditionTestSet AudienceRestrictionConditionTestSet { get; set; }

        public SamlAuthenticationStatementTestSet AuthenticationStatementTestSet { get; set; }

        public SamlAuthorizationDecisionStatementTestSet AuthorizationDecisionTestSet { get; set; }

        public SamlConditionsTestSet ConditionsTestSet { get; set; }

        public DSigSerializer DSigSerializer { get; set; } = new DSigSerializer();

        public SamlEvidenceTestSet EvidenceTestSet { get; set; }

        public SamlSecurityTokenHandler Handler { get; set; } = new SamlSecurityTokenHandler();

        public string InclusiveNamespacesPrefixList { get; set; }

        public SamlSerializer SamlSerializer { get; set; } = new SamlSerializer();

        public SamlTokenTestSet SamlTokenTestSet { get; set; }

        public SamlSubjectTestSet SubjectTestSet { get; set; }

        public SamlTokenTestSet TokenTestSet { get; set; }

        public override string ToString()
        {
            return $"{TestId}, {ExpectedException}";
        }
    }
}
