// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

#nullable enable
namespace Microsoft.IdentityModel.Tokens.TokenValidation.Tests
{
    public class JwtValidation
    {
        #region Algorithm
        [Theory, MemberData(nameof(InvalidAlgorithmTestCases), DisableDiscoveryEnumeration = true)]
        public async Task InvalidAlgorithm(ValidateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.InvalidAlgorithm", theoryData);

            await ValidationUtils.RunInvalidTest(theoryData, context);

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ValidateTokenTheoryData> InvalidAlgorithmTestCases()
        {
            ITestingTokenHandler tokenHandler = new JsonWebTestingTokenHandler();
            tokenHandler.SetDefaultTimesOnTokenCreation = false;
            return TestCaseProvider.GenerateInvalidAlgorithmTestCases(tokenHandler);
        }

        [Theory, MemberData(nameof(ValidAlgorithmTestCases), DisableDiscoveryEnumeration = true)]
        public async Task ValidAlgorithm(ValidateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidAlgorithm", theoryData);

            await ValidationUtils.RunValidTest(theoryData, context);

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ValidateTokenTheoryData> ValidAlgorithmTestCases()
        {
            return TestCaseProvider.GenerateValidAlgorithmTestCases(new JsonWebTestingTokenHandler());
        }
        #endregion

        #region Audience
        [Theory, MemberData(nameof(InvalidAudienceTestCases), DisableDiscoveryEnumeration = true)]
        public async Task InvalidAudience(ValidateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.InvalidAudience", theoryData);

            await ValidationUtils.RunInvalidTest(theoryData, context);

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ValidateTokenTheoryData> InvalidAudienceTestCases()
        {
            return TestCaseProvider.GenerateInvalidAudienceTestCases(new JsonWebTestingTokenHandler());
        }

        [Theory, MemberData(nameof(ValidAudienceTestCases), DisableDiscoveryEnumeration = true)]
        public async Task ValidAudience(ValidateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidAudience", theoryData);

            await ValidationUtils.RunValidTest(theoryData, context);

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ValidateTokenTheoryData> ValidAudienceTestCases()
        {
            return TestCaseProvider.GenerateValidAudienceTestCases(new JsonWebTestingTokenHandler());
        }
        #endregion

        #region Issuer
        [Theory, MemberData(nameof(InvalidIssuerTestCases), DisableDiscoveryEnumeration = true)]
        public async Task InvalidIssuer(ValidateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.InvalidIssuer", theoryData);

            await ValidationUtils.RunInvalidTest(theoryData, context);

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ValidateTokenTheoryData> InvalidIssuerTestCases()
        {
            return TestCaseProvider.GenerateInvalidIssuerTestCases(new JsonWebTestingTokenHandler());
        }

        [Theory, MemberData(nameof(ValidIssuerTestCases), DisableDiscoveryEnumeration = true)]
        public async Task ValidIssuer(ValidateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidIssuer", theoryData);

            await ValidationUtils.RunValidTest(theoryData, context);

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ValidateTokenTheoryData> ValidIssuerTestCases()
        {
            return TestCaseProvider.GenerateValidIssuerTestCases(new JsonWebTestingTokenHandler());
        }
        #endregion

        #region IssuerSigningKey
        [Theory, MemberData(nameof(InvalidIssuerSigningKeyTestCases), DisableDiscoveryEnumeration = true)]
        public async Task InvalidIssuerSigningKey(ValidateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.InvalidIssuerSigningKey", theoryData);
            await ValidationUtils.RunInvalidTest(theoryData, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ValidateTokenTheoryData> InvalidIssuerSigningKeyTestCases()
        {
            ITestingTokenHandler tokenHandler = new JsonWebTestingTokenHandler();
            tokenHandler.SetDefaultTimesOnTokenCreation = false;
            return TestCaseProvider.GenerateInvalidIssuerSigningKeyTestCases(tokenHandler);
        }

        [Theory, MemberData(nameof(ValidIssuerSigningKeyTestCases), DisableDiscoveryEnumeration = true)]
        public async Task ValidIssuerSigningKey(ValidateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidIssuerSigningKey", theoryData);
            await ValidationUtils.RunValidTest(theoryData, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ValidateTokenTheoryData> ValidIssuerSigningKeyTestCases()
        {
            return TestCaseProvider.GenerateValidIssuerSigningKeyTestCases(new JsonWebTestingTokenHandler());
        }
        #endregion

        #region Lifetime
        [Theory, MemberData(nameof(InvalidLifetimeTestCases), DisableDiscoveryEnumeration = true)]
        public async Task InvalidLifetime(ValidateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.InvalidLifetime", theoryData);

            await ValidationUtils.RunInvalidTest(theoryData, context);

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ValidateTokenTheoryData> InvalidLifetimeTestCases()
        {
            JsonWebTestingTokenHandler tokenHandler = new JsonWebTestingTokenHandler();
            tokenHandler.SetDefaultTimesOnTokenCreation = false;
            return TestCaseProvider.GenerateInvalidLifetimeTestCases(tokenHandler);
        }

        [Theory, MemberData(nameof(ValidLifetimeTestCases), DisableDiscoveryEnumeration = true)]
        public async Task ValidLifetime(ValidateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidLifetime", theoryData);

            await ValidationUtils.RunValidTest(theoryData, context);

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ValidateTokenTheoryData> ValidLifetimeTestCases()
        {
            return TestCaseProvider.GenerateValidLifetimeTestCases(new JsonWebTestingTokenHandler());
        }
        #endregion

        #region ReadToken
        [Theory, MemberData(nameof(InvalidReadTokenTestCases), DisableDiscoveryEnumeration = true)]
        public async Task InvalidReadToken(ValidateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.InvalidReadToken", theoryData);

            await ValidationUtils.RunInvalidTest(theoryData, context);

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ValidateTokenTheoryData> InvalidReadTokenTestCases()
        {
            JsonWebTestingTokenHandler tokenHandler = new JsonWebTestingTokenHandler();
            tokenHandler.SetDefaultTimesOnTokenCreation = false;
            return TestCaseProvider.GenerateInvalidReadTokenTestCases(tokenHandler);
        }

        [Theory, MemberData(nameof(ValidReadTokenTestCases), DisableDiscoveryEnumeration = true)]
        public async Task ValidReadToken(ValidateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidReadToken", theoryData);

            await ValidationUtils.RunValidTest(theoryData, context);

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ValidateTokenTheoryData> ValidReadTokenTestCases()
        {
            return TestCaseProvider.GenerateValidReadTokenTestCases(new JsonWebTestingTokenHandler());
        }
        #endregion

        #region Signature
        [Theory, MemberData(nameof(InvalidSignatureTestCases), DisableDiscoveryEnumeration = true)]
        public async Task InvalidSignature(ValidateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.InvalidSignature", theoryData);
            await ValidationUtils.RunInvalidTest(theoryData, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ValidateTokenTheoryData> InvalidSignatureTestCases()
        {
            ITestingTokenHandler tokenHandler = new JsonWebTestingTokenHandler();
            tokenHandler.SetDefaultTimesOnTokenCreation = false;
            return TestCaseProvider.GenerateInvalidSignatureTestCases(tokenHandler);
        }

        [Theory, MemberData(nameof(ValidSignatureTestCases), DisableDiscoveryEnumeration = true)]
        public async Task ValidSignature(ValidateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidSignature", theoryData);
            await ValidationUtils.RunValidTest(theoryData, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ValidateTokenTheoryData> ValidSignatureTestCases()
        {
            return TestCaseProvider.GenerateValidSignatureTestCases(new JsonWebTestingTokenHandler());
        }
        #endregion

        #region TokenReplay
        [Theory, MemberData(nameof(InvalidTokenReplayTestCases), DisableDiscoveryEnumeration = true)]
        public async Task InvalidTokenReplay(ValidateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.InvalidTokenReplay", theoryData);

            await ValidationUtils.RunInvalidTest(theoryData, context);

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ValidateTokenTheoryData> InvalidTokenReplayTestCases()
        {
            JsonWebTestingTokenHandler tokenHandler = new JsonWebTestingTokenHandler();
            tokenHandler.SetDefaultTimesOnTokenCreation = false;
            return TestCaseProvider.GenerateInvalidTokenReplayTestCases(tokenHandler);
        }

        [Theory, MemberData(nameof(ValidTokenReplayTestCases), DisableDiscoveryEnumeration = true)]
        public async Task ValidTokenReplay(ValidateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidTokenReplay", theoryData);

            await ValidationUtils.RunValidTest(theoryData, context);

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ValidateTokenTheoryData> ValidTokenReplayTestCases()
        {
            return TestCaseProvider.GenerateValidTokenReplayTestCases(new JsonWebTestingTokenHandler());
        }
        #endregion
    }
}
#nullable restore
