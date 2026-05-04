// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.IdentityModel.TestExtensions;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace Microsoft.IdentityModel.SampleTests
{
    /// <summary>
    /// A class containing a sample implementation of unit tests for a library that validates tokens with Microsoft.IdentityModel.
    /// </summary>
    /// <remarks>
    /// This class, along with <see cref="SampleTokenValidationClass"/>, are meant to act as a blue print for how to leverage
    /// <see cref="TestTokenCreator"/> to exercise common token types validation code should be able to handle.
    /// </remarks>
    public class SampleTokenValidationClassTests
    {
        /// <summary>
        /// A static insatnce of the <see cref="TestTokenCreator"/> which is responsible for creating the tokens
        /// for the implementation under test.
        /// </summary>
        public static TestTokenCreator testTokenCreator = new TestTokenCreator()
        {
            Audience = "http://Default.Audience.com",
            Issuer = "http://Default.Issuer.com",
            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials
        };

        #region Current Model Token Validation Tests
        /// <summary>
        /// Tests how the class under test handles a valid token.
        /// </summary>
        [Fact]
        public void ValidToken()
        {
            SampleTokenValidationClass classUnderTest = new SampleTokenValidationClass();
            classUnderTest.ValidateTokenShim(testTokenCreator.CreateDefaultValidToken());
        }

        /// <summary>
        /// Tests how the class under test handles a bogus token; one that in no way conforms to the expected JWS format.
        /// </summary>
        [Fact]
        public void BogusToken()
        {
            TestWithGeneratedToken(
                () => "InvalidToken",
                typeof(SecurityTokenMalformedException),
                "IDX14100");
        }

        /// <summary>
        /// Tests how the class under test handles a token with a missing signature.
        /// </summary>
        [Fact]
        public void TokenWithoutSignature()
        {
            TestWithGeneratedToken(
                testTokenCreator.CreateTokenWithNoSignature,
                typeof(SecurityTokenInvalidSignatureException),
                "IDX10504:");
        }

        /// <summary>
        /// Tests how the class under test handles a token with a malformed signature.
        /// </summary>
        [Fact]
        public void TokenWithBadSignature()
        {
            TestWithGeneratedToken(
                testTokenCreator.CreateTokenWithInvalidSignature,
                typeof(SecurityTokenInvalidSignatureException),
                "IDX10511:");
        }

        /// <summary>
        /// Tests how the class under test handles a token which is expired.
        /// </summary>
        [Fact]
        public void ExpiredToken()
        {
            TestWithGeneratedToken(
                testTokenCreator.CreateExpiredToken,
                typeof(SecurityTokenExpiredException),
                "IDX10223");
        }

        /// <summary>
        /// Tests how the class under test handles a token which is not yet valid
        /// </summary>
        [Fact]
        public void NetYetValidToken()
        {
            TestWithGeneratedToken(
                testTokenCreator.CreateNotYetValidToken,
                typeof(SecurityTokenNotYetValidException),
                "IDX10222");
        }

        /// <summary>
        /// Tests how the class under test handles a token with an issuer that doesn't match expectations.
        /// </summary>
        [Fact]
        public void TokenWithWrongIssuer()
        {
            TestWithGeneratedToken(
                testTokenCreator.CreateTokenWithBadIssuer,
                typeof(SecurityTokenInvalidIssuerException),
                "IDX10205");
        }

        /// <summary>
        /// Tests how the class under test handles a token with an audience that doesn't match expectations.
        /// </summary>
        [Fact]
        public void TokenWithWrongAudience()
        {
            TestWithGeneratedToken(
                testTokenCreator.CreateTokenWithBadAudience,
                typeof(SecurityTokenInvalidAudienceException),
                "IDX10214");
        }

        /// <summary>
        /// Tests how the class under test handles a token signed with a key different than the one expected.
        /// </summary>
        [Fact]
        public void TokenWithBadSignatureKey()
        {
            TestWithGeneratedToken(
                testTokenCreator.CreateTokenWithBadSignatureKey,
                typeof(SecurityTokenSignatureKeyNotFoundException),
                "IDX10503");
        }

        /// <summary>
        /// Tests how the class under test handles a token missing the iss claim.
        /// </summary>
        [Fact]
        public void TokenWithMissingIssuer()
        {
            TestWithGeneratedToken(
                testTokenCreator.CreateTokenWithMissingIssuer,
                typeof(SecurityTokenInvalidIssuerException),
                "IDX10211");
        }

        /// <summary>
        /// Tests how the class under test handles a token missing the aud claim.
        /// </summary>
        [Fact]
        public void TokenWithMissingAudience()
        {
            TestWithGeneratedToken(
                testTokenCreator.CreateTokenWithMissingAudience,
                typeof(SecurityTokenInvalidAudienceException),
                "IDX10206");
        }

        /// <summary>
        /// Tests how the class under test handles a token with a iat claim indicating it has not yet been issued.
        /// </summary>
        [Fact]
        public void TokenWithFutureIssuedAt()
        {
            // NOTE: This is not currently validated and there's no way to enforce its presence.
            //       It may be enforceable in the future, in which case this will be updated with proper checks.
            SampleTokenValidationClass classUnderTest = new SampleTokenValidationClass();
            classUnderTest.ValidateTokenShim(testTokenCreator.CreateTokenWithFutureIssuedAt());
        }

        /// <summary>
        /// Tests how the class under test handles a token missing the iat claim.
        /// </summary>
        [Fact]
        public void TokenWithMissingIssuedAt()
        {
            // NOTE: This is not currently validated and there's no way to enforce its presence.
            //       It may be enforceable in the future, in which case this will be updated with proper checks.
            SampleTokenValidationClass classUnderTest = new SampleTokenValidationClass();
            classUnderTest.ValidateTokenShim(testTokenCreator.CreateTokenWithMissingIssuedAt());
        }

        /// <summary>
        /// Tests how the class under test handles a token missing the nbf claim.
        /// </summary>
        [Fact]
        public void TokenWithMissingNotBefore()
        {
            // NOTE: This is not currently validated and there's no way to enforce its presence.
            //       It may be enforceable in the future, in which case this will be updated with proper checks.
            SampleTokenValidationClass classUnderTest = new SampleTokenValidationClass();
            classUnderTest.ValidateTokenShim(testTokenCreator.CreateTokenWithMissingNotBefore());
        }

        /// <summary>
        /// Tests how the class under test handles a token missing the exp claim.
        /// </summary>
        [Fact]
        public void TokenWithMissingExpires()
        {
            TestWithGeneratedToken(
                testTokenCreator.CreateTokenWithMissingExpires,
                typeof(SecurityTokenNoExpirationException),
                "IDX10225");
        }

        /// <summary>
        /// Test how the class under test handles a token without a signing key (i.e. alg=none, no signature).
        /// </summary>
        [Fact]
        public void TokenWithMissingSecurityCredentials()
        {
            TestWithGeneratedToken(
                testTokenCreator.CreateTokenWithMissingKey,
                typeof(SecurityTokenInvalidSignatureException),
                "IDX10504");
        }
        #endregion

        #region Deprecated Model Token Validation Tests
        /// <summary>
        /// Tests how a class under test using JwtSecurityTokenHandler handles a valid token.
        /// </summary>
        [Fact]
        public void ValidToken_Deprecated()
        {
            SampleTokenValidationClass classUnderTest = new SampleTokenValidationClass();
            classUnderTest.ValidateTokenShimWithDeprecatedModel(testTokenCreator.CreateDefaultValidToken());
        }

        /// <summary>
        /// Tests how the class under test using JwtSecurityTokenHandler handles a bogus token; one that in
        /// no way conforms to the expected JWS format.
        /// </summary>
        [Fact]
        public void BogusToken_Deprecated()
        {
            TestWithGeneratedToken_Deprecated(
                () => "InvalidToken",
                typeof(SecurityTokenMalformedException),
                "IDX12741");
        }

        /// <summary>
        /// Tests how the class under test using JwtSecurityTokenHandler handles a token with a missing signature.
        /// </summary>
        [Fact]
        public void TokenWithoutSignature_Deprecated()
        {
            TestWithGeneratedToken_Deprecated(
                testTokenCreator.CreateTokenWithNoSignature,
                typeof(SecurityTokenInvalidSignatureException),
                "IDX10504");
        }

        /// <summary>
        /// Tests how the class under test using JwtSecurityTokenHandler handles a token with a malformed signature.
        /// </summary>
        [Fact]
        public void TokenWithBadSignature_Deprecated()
        {
            TestWithGeneratedToken_Deprecated(
                testTokenCreator.CreateTokenWithInvalidSignature,
                typeof(SecurityTokenInvalidSignatureException),
                "IDX10511");
        }

        /// <summary>
        /// Tests how the class under test using JwtSecurityTokenHandler handles a token which is expired.
        /// </summary>
        [Fact]
        public void ExpiredToken_Deprecated()
        {
            TestWithGeneratedToken_Deprecated(
                testTokenCreator.CreateExpiredToken,
                typeof(SecurityTokenExpiredException),
                "IDX10223");
        }

        /// <summary>
        /// Tests how the class under test using JwtSecurityTokenHandler handles a token which is not yet valid
        /// </summary>
        [Fact]
        public void NetYetValidToken_Deprecated()
        {
            TestWithGeneratedToken_Deprecated(
                testTokenCreator.CreateNotYetValidToken,
                typeof(SecurityTokenNotYetValidException),
                "IDX10222");
        }

        /// <summary>
        /// Tests how the class under test using JwtSecurityTokenHandler handles a token with an issuer that doesn't match expectations.
        /// </summary>
        [Fact]
        public void TokenWithWrongIssuer_Deprecated()
        {
            TestWithGeneratedToken_Deprecated(
                testTokenCreator.CreateTokenWithBadIssuer,
                typeof(SecurityTokenInvalidIssuerException),
                "IDX10205");
        }

        /// <summary>
        /// Tests how the class under test using JwtSecurityTokenHandler handles a token with an audience that doesn't match expectations.
        /// </summary>
        [Fact]
        public void TokenWithWrongAudience_Deprecated()
        {
            TestWithGeneratedToken_Deprecated(
                testTokenCreator.CreateTokenWithBadAudience,
                typeof(SecurityTokenInvalidAudienceException),
                "IDX10214");
        }

        /// <summary>
        /// Tests how the class under test using JwtSecurityTokenHandler handles a token signed with a key different than the one expected.
        /// </summary>
        [Fact]
        public void TokenWithBadSignatureKey_Deprecated()
        {
            TestWithGeneratedToken_Deprecated(
                testTokenCreator.CreateTokenWithBadSignatureKey,
                typeof(SecurityTokenSignatureKeyNotFoundException),
                "IDX10503");
        }

        /// <summary>
        /// Tests how the class under test using JwtSecurityTokenHandler handles a token missing the iss claim.
        /// </summary>
        [Fact]
        public void TokenWithMissingIssuer_Deprecated()
        {
            TestWithGeneratedToken_Deprecated(
                testTokenCreator.CreateTokenWithMissingIssuer,
                typeof(SecurityTokenInvalidIssuerException),
                "IDX10211");
        }

        /// <summary>
        /// Tests how the class under test using JwtSecurityTokenHandler handles a token missing the aud claim.
        /// </summary>
        [Fact]
        public void TokenWithMissingAudience_Deprecated()
        {
            TestWithGeneratedToken_Deprecated(
                testTokenCreator.CreateTokenWithMissingAudience,
                typeof(SecurityTokenInvalidAudienceException),
                "IDX10206");
        }

        /// <summary>
        /// Tests how the class under test using JwtSecurityTokenHandler handles a token with a iat claim
        /// indicating it has not yet been issued.
        /// </summary>
        [Fact]
        public void TokenWithFutureIssuedAt_Deprecated()
        {
            // NOTE: This is not currently validated and there's no way to enforce its presence.
            //       It may be enforceable in the future, in which case this will be updated with proper checks.
            SampleTokenValidationClass classUnderTest = new SampleTokenValidationClass();
            classUnderTest.ValidateTokenShimWithDeprecatedModel(testTokenCreator.CreateTokenWithFutureIssuedAt());
        }

        /// <summary>
        /// Tests how the class under test using JwtSecurityTokenHandler handles a token missing the iat claim.
        /// </summary>
        [Fact]
        public void TokenWithMissingIssuedAt_Deprecated()
        {
            // NOTE: This is not currently validated and there's no way to enforce its presence.
            //       It may be enforceable in the future, in which case this will be updated with proper checks.
            SampleTokenValidationClass classUnderTest = new SampleTokenValidationClass();
            classUnderTest.ValidateTokenShimWithDeprecatedModel(testTokenCreator.CreateTokenWithMissingIssuedAt());
        }

        /// <summary>
        /// Tests how the class under test using JwtSecurityTokenHandler handles a token missing the nbf claim.
        /// </summary>
        [Fact]
        public void TokenWithMissingNotBefore_Deprecated()
        {
            // NOTE: This is not currently validated and there's no way to enforce its presence.
            //       It may be enforceable in the future, in which case this will be updated with proper checks.
            SampleTokenValidationClass classUnderTest = new SampleTokenValidationClass();
            classUnderTest.ValidateTokenShimWithDeprecatedModel(testTokenCreator.CreateTokenWithMissingNotBefore());
        }

        /// <summary>
        /// Tests how the class under test using JwtSecurityTokenHandler handles a token missing the exp claim.
        /// </summary>
        [Fact]
        public void TokenWithMissingExpires_Deprecated()
        {
            TestWithGeneratedToken_Deprecated(
                testTokenCreator.CreateTokenWithMissingExpires,
                typeof(SecurityTokenNoExpirationException),
                "IDX10225");
        }

        /// <summary>
        /// Test how the class under test using JwtSecurityTokenHandler handles a token without a signing key
        /// (i.e. alg=none, no signature).
        /// </summary>
        [Fact]
        public void TokenWithMissingSecurityCredentials_Deprecated()
        {
            TestWithGeneratedToken_Deprecated(
                testTokenCreator.CreateTokenWithMissingKey,
                typeof(SecurityTokenInvalidSignatureException),
                "IDX10504");
        }
        #endregion

        /// <summary>
        /// Calls the class under test using JwtSecurityTokenHandler with a token and validates the outcome.
        /// </summary>
        /// <param name="generateTokenToTest">Function which returns the JWS to test with.</param>
        /// <param name="expectedInnerExceptionType">The inner exception type expected.</param>
        /// <param name="expectedInnerExceptionMessagePart">A string the inner exception message is expected to contain.</param>
        internal void TestWithGeneratedToken_Deprecated(
            Func<string> generateTokenToTest,
            Type expectedInnerExceptionType,
            string expectedInnerExceptionMessagePart)
        {
            SampleTokenValidationClass classUnderTest = new SampleTokenValidationClass();
            TestWithGeneratedToken(
                classUnderTest.ValidateTokenShimWithDeprecatedModel,
                generateTokenToTest,
                expectedInnerExceptionType,
                expectedInnerExceptionMessagePart);
        }

        /// <summary>
        /// Calls the class under test with a token and validates the outcome.
        /// </summary>
        /// <param name="generateTokenToTest">Function which returns the JWS to test with.</param>
        /// <param name="expectedInnerExceptionType">The inner exception type expected.</param>
        /// <param name="expectedInnerExceptionMessagePart">A string the inner exception message is expected to contain.</param>
        internal void TestWithGeneratedToken(Func<string> generateTokenToTest, Type expectedInnerExceptionType, string expectedInnerExceptionMessagePart)
        {
            SampleTokenValidationClass classUnderTest = new SampleTokenValidationClass();
            TestWithGeneratedToken(
                classUnderTest.ValidateTokenShim,
                generateTokenToTest,
                expectedInnerExceptionType,
                expectedInnerExceptionMessagePart);
        }

        /// <summary>
        /// Calls a passed <paramref name="validate"/> action with a generated token and validates the outcome.
        /// </summary>
        /// <param name="validate">Action which takes in the token generated by <paramref name="generateTokenToTest"/>.</param>
        /// <param name="generateTokenToTest">Function which returns the JWS to test with.</param>
        /// <param name="expectedInnerExceptionType">The inner exception type expected.</param>
        /// <param name="expectedInnerExceptionMessagePart">A string the inner exception message is expected to contain.</param>
        internal void TestWithGeneratedToken(
            Action<string> validate,
            Func<string> generateTokenToTest,
            Type expectedInnerExceptionType,
            string expectedInnerExceptionMessagePart)
        {
            Action testAction = () =>
            {
                validate(generateTokenToTest());
            };

            AssertValidationException(testAction, expectedInnerExceptionType, expectedInnerExceptionMessagePart);
        }

        /// <summary>
        /// Asserts the passed validation <paramref name="action"/> throws the expected exceptions.
        /// </summary>
        /// <param name="action">Action which validates a test token.</param>
        /// <param name="innerExceptionType">The inner exception type expected.</param>
        /// <param name="innerExceptionMessagePart">A string the inner exception message is expected to contain.</param>
        internal void AssertValidationException(Action action, Type innerExceptionType, string innerExceptionMessagePart)
        {
            try
            {
                action();

                if (innerExceptionType != null || !string.IsNullOrEmpty(innerExceptionMessagePart))
                    throw new TestException(
                        string.Format(
                            "Expected an exception of type '{0}' containing '{1}' in the message.",
                            innerExceptionType,
                            innerExceptionMessagePart));
            }
            catch (Exception e)
            {
                Assert.Equal(typeof(SampleTestTokenValidationException), e.GetType());
                Assert.Equal(innerExceptionType, e.InnerException.GetType());

                if (!string.IsNullOrEmpty(innerExceptionMessagePart))
                {
                    Assert.Contains(innerExceptionMessagePart, e.InnerException.Message);
                }
            }
        }
    }
}
