// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IdentityModel.Tokens.Jwt.Tests;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Xunit;
using TokenLogMessages = Microsoft.IdentityModel.Tokens.LogMessages;

namespace Microsoft.IdentityModel.JsonWebTokens.Tests
{
    public class JsonWebTokenHandlerReadTokenTests
    {
        [Theory, MemberData(nameof(JsonWebTokenHandlerReadTokenTestCases), DisableDiscoveryEnumeration = true)]
        public void ReadToken(TokenReadingTheoryData theoryData)
        {
            CompareContext context = TestUtilities.WriteHeader($"{this}.JsonWebTokenHandlerReadTokenTests", theoryData);
            TokenReadingResult tokenReadingResult = JsonWebTokenHandler.ReadToken(
                theoryData.Token,
                new CallContext());

            if (tokenReadingResult.Exception != null)
                theoryData.ExpectedException.ProcessException(tokenReadingResult.Exception);
            else
                theoryData.ExpectedException.ProcessNoException();

            IdentityComparer.AreTokenReadingResultsEqual(
                tokenReadingResult,
                theoryData.TokenReadingResult,
                context);

            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void ReadToken_ThrowsIfAccessingSecurityTokenOnFailedRead()
        {
            TokenReadingResult tokenReadingResult = JsonWebTokenHandler.ReadToken(
                null,
                new CallContext());

            Assert.Throws<InvalidOperationException>(() => tokenReadingResult.SecurityToken());
        }

        public static TheoryData<TokenReadingTheoryData> JsonWebTokenHandlerReadTokenTestCases
        {
            get
            {
                var validToken = EncodedJwts.LiveJwt;
                return new TheoryData<TokenReadingTheoryData>
                {
                    new TokenReadingTheoryData
                    {
                        TestId = "Valid_Jwt",
                        Token = validToken,
                        TokenReadingResult = new TokenReadingResult(
                            new JsonWebToken(validToken),
                            validToken)
                    },
                    new TokenReadingTheoryData
                    {
                        TestId = "Invalid_NullToken",
                        Token = null,
                        ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                        TokenReadingResult = new TokenReadingResult(
                            null,
                            ValidationFailureType.NullArgument,
                            new ExceptionDetail(
                                new MessageDetail(
                                    TokenLogMessages.IDX10000,
                                    LogHelper.MarkAsNonPII("token")),
                                typeof(ArgumentNullException),
                                new System.Diagnostics.StackFrame()))
                    },
                    new TokenReadingTheoryData
                    {
                        TestId = "Invalid_EmptyToken",
                        Token = string.Empty,
                        ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                        TokenReadingResult = new TokenReadingResult(
                            string.Empty,
                            ValidationFailureType.NullArgument,
                            new ExceptionDetail(
                                new MessageDetail(
                                    TokenLogMessages.IDX10000,
                                    LogHelper.MarkAsNonPII("token")),
                                typeof(ArgumentNullException),
                                new System.Diagnostics.StackFrame()))
                    },
                    new TokenReadingTheoryData
                    {
                        TestId = "Invalid_MalformedToken",
                        Token = "malformed-token",
                        ExpectedException = ExpectedException.SecurityTokenMalformedTokenException(
                            "IDX14107:",
                            typeof(SecurityTokenMalformedException)),
                        TokenReadingResult = new TokenReadingResult(
                            "malformed-token",
                            ValidationFailureType.TokenReadingFailed,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX14107,
                                    LogHelper.MarkAsNonPII("token")),
                                typeof(SecurityTokenMalformedException),
                                new System.Diagnostics.StackFrame()))
                    }
                };
            }
        }
    }

    public class TokenReadingTheoryData : TheoryDataBase
    {
        public string Token { get; set; }
        public object TokenReadingResult { get; set; }
    }
}
