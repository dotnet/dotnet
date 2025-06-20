// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

namespace Microsoft.IdentityModel.Tokens.Validation.Tests
{
    public class ReplayValidationResultTests
    {
        [Theory, MemberData(nameof(TokenReplayValidationTestCases), DisableDiscoveryEnumeration = true)]
        public void ValidateTokenReplay(TokenReplayTheoryData theoryData)
        {
            CompareContext context = TestUtilities.WriteHeader($"{this}.TokenReplayValidationResultTests", theoryData);

            ReplayValidationResult replayValidationResult = Validators.ValidateTokenReplay(
                theoryData.ExpirationTime,
                theoryData.SecurityToken,
                theoryData.ValidationParameters,
                new CallContext());

            if (replayValidationResult.Exception != null)
                theoryData.ExpectedException.ProcessException(replayValidationResult.Exception);
            else
                theoryData.ExpectedException.ProcessNoException();

            IdentityComparer.AreTokenReplayValidationResultsEqual(
                replayValidationResult,
                theoryData.ReplayValidationResult,
                context);

            TestUtilities.AssertFailIfErrors(context);

        }

        public static TheoryData<TokenReplayTheoryData> TokenReplayValidationTestCases
        {
            get
            {
                DateTime now = DateTime.UtcNow;
                DateTime oneHourAgo = now.AddHours(-1);
                DateTime oneHourFromNow = now.AddHours(1);

                return new TheoryData<TokenReplayTheoryData>
                {
                    new TokenReplayTheoryData
                    {
                        TestId = "Valid_ReplayCache_Null",
                        ExpirationTime = oneHourAgo,
                        SecurityToken = "token",
                        ValidationParameters = new TokenValidationParameters
                        {
                            TokenReplayCache = null,
                            ValidateTokenReplay = true
                        },
                        ReplayValidationResult = new ReplayValidationResult(oneHourAgo)
                    },
                    new TokenReplayTheoryData
                    {
                        TestId = "Valid_ReplayCache_NotNull",
                        ExpirationTime = oneHourFromNow,
                        SecurityToken = "token",
                        ValidationParameters = new TokenValidationParameters
                        {
                            TokenReplayCache = new TokenReplayCache { OnAddReturnValue = true, OnFindReturnValue = false },
                            ValidateTokenReplay = true
                        },
                        ReplayValidationResult = new ReplayValidationResult(oneHourFromNow)
                    },
                    new TokenReplayTheoryData
                    {
                        TestId = "Valid_ValidateTokenReplay_False",
                        ExpirationTime = oneHourFromNow,
                        SecurityToken = "token",
                        ValidationParameters = new TokenValidationParameters
                        {
                            TokenReplayCache = new TokenReplayCache
                            {
                                OnAddReturnValue = true, 
                                OnFindReturnValue = true // token already exists in cache, if ValidateTokenReplay were true, this would fail.
                            },
                            ValidateTokenReplay = false
                        },
                        ReplayValidationResult = new ReplayValidationResult(oneHourFromNow)
                    },
                    new TokenReplayTheoryData
                    {
                        TestId = "Valid_DelegateIsSet_ReturnsTrue",
                        ExpirationTime = oneHourFromNow,
                        SecurityToken = "token",
                        ValidationParameters = new TokenValidationParameters
                        {
                            TokenReplayValidator = (token, expirationTime, validationParameters) => true,
                            ValidateTokenReplay = true
                        },
                        ReplayValidationResult = new ReplayValidationResult(oneHourFromNow)
                    },
                    new TokenReplayTheoryData
                    {
                        TestId = "Invalid_SecurityToken_Null",
                        ExpirationTime = now,
                        SecurityToken = null,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidateTokenReplay = true
                        },
                        ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                        ReplayValidationResult = new ReplayValidationResult(
                            now,
                            ValidationFailureType.NullArgument,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10000,
                                    LogHelper.MarkAsNonPII("securityToken")),
                                typeof(ArgumentNullException),
                                new StackFrame(),
                                null))
                    },
                    new TokenReplayTheoryData
                    {
                        TestId = "Invalid_SecurityToken_Empty",
                        ExpirationTime = now,
                        SecurityToken = string.Empty,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidateTokenReplay = true
                        },
                        ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                        ReplayValidationResult = new ReplayValidationResult(
                            now,
                            ValidationFailureType.NullArgument,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10000,
                                    LogHelper.MarkAsNonPII("securityToken")),
                                typeof(ArgumentNullException),
                                new StackFrame(),
                                null))
                    },
                    new TokenReplayTheoryData
                    {
                        TestId = "Invalid_ValidationParameters_Null",
                        ExpirationTime = now,
                        SecurityToken = "token",
                        ValidationParameters = null,
                        ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                        ReplayValidationResult = new ReplayValidationResult(
                            now,
                            ValidationFailureType.NullArgument,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10000,
                                    LogHelper.MarkAsNonPII("validationParameters")),
                                typeof(ArgumentNullException),
                                new StackFrame(),
                                null))
                    },
                    new TokenReplayTheoryData
                    {
                        TestId = "Invalid_DelegateIsSet_ReturnsFalse",
                        ExpirationTime = now,
                        SecurityToken = "token",
                        ValidationParameters = new TokenValidationParameters
                        {
                            TokenReplayValidator = (token, expirationTime, validationParameters) => false,
                            ValidateTokenReplay = true
                        },
                        ExpectedException = ExpectedException.SecurityTokenReplayDetected("IDX10228"),
                        ReplayValidationResult = new ReplayValidationResult(
                            now,
                            ValidationFailureType.TokenReplayValidationFailed,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10228,
                                    LogHelper.MarkAsUnsafeSecurityArtifact("token", t => t.ToString())),
                                typeof(SecurityTokenReplayDetectedException),
                                new StackFrame(),
                                null))
                    },
                    new TokenReplayTheoryData
                    {
                        TestId = "Invalid_DelegateIsSet_ThrowsException",
                        ExpirationTime = now,
                        SecurityToken = "token",
                        ValidationParameters = new TokenValidationParameters
                        {
                            TokenReplayValidator = (token, expirationTime, validationParameters) => throw new SecurityTokenReplayDetectedException(),
                            ValidateTokenReplay = true
                        },
                        ExpectedException = ExpectedException.SecurityTokenReplayDetected("IDX10228:", innerTypeExpected: typeof(SecurityTokenReplayDetectedException)),
                        ReplayValidationResult = new ReplayValidationResult(
                            now,
                            ValidationFailureType.TokenReplayValidationFailed,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10228,
                                    LogHelper.MarkAsUnsafeSecurityArtifact("token", t => t.ToString())),
                                typeof(SecurityTokenReplayDetectedException),
                                new StackFrame(),
                                new SecurityTokenReplayDetectedException()))
                    },
                    new TokenReplayTheoryData
                    {
                        TestId = "Invalid_ReplayCacheIsPresent_ExpirationTimeIsNull",
                        ExpirationTime = null,
                        SecurityToken = "token",
                        ValidationParameters = new TokenValidationParameters
                        {
                            TokenReplayCache = new TokenReplayCache
                            {
                                OnAddReturnValue = true,
                                OnFindReturnValue = false
                            },
                            ValidateTokenReplay = true
                        },
                        ExpectedException = ExpectedException.SecurityTokenReplayDetected("IDX10227:"),
                        ReplayValidationResult = new ReplayValidationResult(
                            null,
                            ValidationFailureType.TokenReplayValidationFailed,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10227,
                                    LogHelper.MarkAsUnsafeSecurityArtifact("token", t => t.ToString())),
                                typeof(SecurityTokenReplayDetectedException),
                                new StackFrame(),
                                null))
                    },
                    new TokenReplayTheoryData
                    {
                        TestId = "Invalid_ReplayCacheIsPresent_TokenIsAlreadyInCache",
                        ExpirationTime = oneHourFromNow,
                        SecurityToken= "token",
                        ValidationParameters = new TokenValidationParameters
                        {
                            TokenReplayCache = new TokenReplayCache
                            {
                                OnAddReturnValue = true,
                                OnFindReturnValue = true
                            },
                            ValidateTokenReplay = true
                        },
                        ExpectedException = ExpectedException.SecurityTokenReplayDetected("IDX10228:"),
                        ReplayValidationResult = new ReplayValidationResult(
                            oneHourFromNow,
                            ValidationFailureType.TokenReplayValidationFailed,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10228,
                                    LogHelper.MarkAsUnsafeSecurityArtifact("token", t => t.ToString())),
                                typeof(SecurityTokenReplayDetectedException),
                                new StackFrame(),
                                null))
                    },
                    new TokenReplayTheoryData
                    {
                        TestId = "Invalid_ReplayCacheIsPresent_AddingTokenToCacheFails",
                        ExpirationTime = oneHourFromNow,
                        SecurityToken= "token",
                        ValidationParameters = new TokenValidationParameters
                        {
                            TokenReplayCache = new TokenReplayCache
                            {
                                OnAddReturnValue = false,
                                OnFindReturnValue = false
                            },
                            ValidateTokenReplay = true
                        },
                        ExpectedException = ExpectedException.SecurityTokenReplayAddFailed("IDX10229:"),
                        ReplayValidationResult = new ReplayValidationResult(
                            oneHourFromNow,
                            ValidationFailureType.TokenReplayValidationFailed,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10229,
                                    LogHelper.MarkAsUnsafeSecurityArtifact("token", t => t.ToString())),
                                typeof(SecurityTokenReplayAddFailedException),
                                new StackFrame(),
                                null))
                    }
                };
            }
        }
    }

    public class TokenReplayTheoryData : TheoryDataBase
    {
        public DateTime? ExpirationTime { get; set; }

        public string SecurityToken { get; set; }

        public TokenValidationParameters ValidationParameters { get; set; }

        internal ReplayValidationResult ReplayValidationResult { get; set; }
    }
}
