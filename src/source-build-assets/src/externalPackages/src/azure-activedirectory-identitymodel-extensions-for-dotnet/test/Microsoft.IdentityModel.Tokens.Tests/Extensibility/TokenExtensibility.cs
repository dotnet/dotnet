// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens.Saml;
using Microsoft.IdentityModel.Tokens.Saml2;
using Xunit;

#nullable enable
namespace Microsoft.IdentityModel.Tokens.Extensibility.Tests
{
    public class TokenExtensibility
    {
        [Theory, MemberData(nameof(JwtInvalidExtensibilityTestCases), DisableDiscoveryEnumeration = true)]
        public async Task JwtInvalidExtensibilityTests(ExtensibilityTheoryData theoryData)
        {
            await ExtensibilityRunner.RunInvalidTest(theoryData, this, nameof(TokenExtensibility));
        }

        public static TheoryData<ExtensibilityTheoryData> JwtInvalidExtensibilityTestCases()
        {
            TheoryData<ExtensibilityTheoryData> theoryData = new();
            var tokenHandler = new JsonWebTokenHandler();
            var token = Default.JsonWebToken();

            return GenerateInvalidTestCases(tokenHandler, token);
        }

        [Theory, MemberData(nameof(Saml2InvalidExtensibilityTestCases), DisableDiscoveryEnumeration = true)]
        public async Task Saml2InvalidExtensibilityTests(ExtensibilityTheoryData theoryData)
        {
            await ExtensibilityRunner.RunInvalidTest(theoryData, this, nameof(TokenExtensibility));
        }

        public static TheoryData<ExtensibilityTheoryData> Saml2InvalidExtensibilityTestCases()
        {
            TheoryData<ExtensibilityTheoryData> theoryData = new();
            var tokenHandler = new Saml2SecurityTokenHandler();
            var token = Default.Saml2SecurityToken();

            return GenerateInvalidTestCases(tokenHandler, token);
        }


        [Theory, MemberData(nameof(SamlInvalidExtensibilityTestCases), DisableDiscoveryEnumeration = true)]
        public async Task SamlExtensibilityTests(ExtensibilityTheoryData theoryData)
        {
            await ExtensibilityRunner.RunInvalidTest(theoryData, this, nameof(TokenExtensibility));
        }

        public static TheoryData<ExtensibilityTheoryData> SamlInvalidExtensibilityTestCases()
        {
            TheoryData<ExtensibilityTheoryData> theoryData = new();
            var tokenHandler = new SamlSecurityTokenHandler();
            var token = Default.SamlSecurityToken();

            return GenerateInvalidTestCases(tokenHandler, token);
        }


        public static TheoryData<ExtensibilityTheoryData> GenerateInvalidTestCases(TokenHandler tokenHandler, SecurityToken token)
        {
            TheoryData<ExtensibilityTheoryData> theoryData = new();

            foreach (var test in ExtensibilityTestProvider.GenerateInvalidAlgorithmTestCases(tokenHandler, token, "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256"))
                theoryData.Add(test);

            foreach (var test in ExtensibilityTestProvider.GenerateAudienceTestCases(tokenHandler, token))
                theoryData.Add(test);

            foreach (var test in ExtensibilityTestProvider.GenerateInvalidIssuerTestCases(tokenHandler, token))
                theoryData.Add(test);

            foreach (var test in ExtensibilityTestProvider.GenerateInvalidIssuerSigningKeyTestCases(tokenHandler, token))
                theoryData.Add(test);

            foreach (var test in ExtensibilityTestProvider.GenerateInvalidLifetimeTestCases(tokenHandler, token))
                theoryData.Add(test);

            foreach (var test in ExtensibilityTestProvider.GenerateInvalidSignatureTestCases(tokenHandler, token))
                theoryData.Add(test);

            foreach (var test in ExtensibilityTestProvider.GenerateInvalidTokenReplayTestCases(tokenHandler, token))
                theoryData.Add(test);

            // SAML and SAML2 do not have token type extensibility.
            if (tokenHandler is JsonWebTokenHandler)
            {
                foreach (var test in ExtensibilityTestProvider.GenerateInvalidTokenTypeTestCases(tokenHandler, token))
                    theoryData.Add(test);
            }

            return theoryData;
        }
    }
}
#nullable restore
