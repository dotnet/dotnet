// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Xml;

namespace Microsoft.IdentityModel.Tokens.Saml.Tests
{
    public class SamlTheoryData : TokenTheoryData
    {
        public SamlTheoryData()
        {
        }

        public SamlTheoryData(string testId) : base(testId)
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
