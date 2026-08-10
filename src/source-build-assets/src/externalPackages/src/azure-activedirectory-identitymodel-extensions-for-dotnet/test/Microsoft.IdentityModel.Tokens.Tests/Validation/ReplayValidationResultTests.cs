// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens.Experimental;
using Xunit;

namespace Microsoft.IdentityModel.Tokens.Validation.Tests
{
    public class ReplayValidationTests
    {
        [Theory, MemberData(nameof(InvalidTestCases), DisableDiscoveryEnumeration = true)]
        public void InvalidReplays(TokenReplayTheoryData theoryData)
        {
            CompareContext context = TestUtilities.WriteHeader($"{this}.InvalidReplays", theoryData);
            try
            {
                ValidationResult<DateTime?, ValidationError> validationResult = Validators.ValidateTokenReplay(
                    theoryData.ExpirationTime,
                    theoryData.SecurityToken,
                    theoryData.ValidationParameters,
                    theoryData.CallContext);

                if (validationResult.Succeeded)
                {
                    context.AddDiff($"Expected validationResult to fail, but it succeeded with: {validationResult.Result}.");
                }
                else
                {
                    ValidationError validationError = validationResult.Error;
                    IdentityComparer.AreStringsEqual(
                        validationError.FailureType.Name,
                        theoryData.OperationResult.Error.FailureType.Name,
                        context);

                    theoryData.ExpectedException.ProcessException(validationError.GetException(), context);
                }
            }
            catch (Exception ex)
            {
                context.AddDiff($"Did not expect an exception: {ex}.");
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<TokenReplayTheoryData> InvalidTestCases
        {
            get
            {
                DateTime now = DateTime.UtcNow;
                DateTime oneHourAgo = now.AddHours(-1);
                DateTime oneHourFromNow = now.AddHours(1);

                return new TheoryData<TokenReplayTheoryData>
                {
                    new TokenReplayTheoryData("SecurityToken_Null")
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                        ExpirationTime = now,
                        SecurityToken = null,
                        ValidationParameters = new ValidationParameters(),
                        OperationResult = new TokenReplayValidationError(
                            new MessageDetail(
                                LogMessages.IDX10000,
                                LogHelper.MarkAsNonPII("securityToken")),
                            ValidationFailureType.NullArgument,
                            null,
                            null),
                    },
                    new TokenReplayTheoryData("SecurityToken_Empty")
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                        ExpirationTime = now,
                        SecurityToken = string.Empty,
                        ValidationParameters = new ValidationParameters(),
                        OperationResult = new TokenReplayValidationError(
                            new MessageDetail(
                                LogMessages.IDX10000,
                                LogHelper.MarkAsNonPII("securityToken")),
                            ValidationFailureType.NullArgument,
                            null,
                            null),
                    },
                    new TokenReplayTheoryData("ValidationParameters_Null")
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                        ExpirationTime = now,
                        SecurityToken = "token",
                        ValidationParameters = null,
                        OperationResult = new TokenReplayValidationError(
                            new MessageDetail(
                                LogMessages.IDX10000,
                                LogHelper.MarkAsNonPII("validationParameters")),
                            ValidationFailureType.NullArgument,
                            null,
                            null),
                    },
                    new TokenReplayTheoryData("Invalid_ReplayCacheIsPresent_ExpirationTimeIsNull")
                    {
                        ExpectedException = ExpectedException.SecurityTokenReplayDetected("IDX10227:"),
                        ExpirationTime = null,
                        SecurityToken = "token",
                        ValidationParameters = new ValidationParameters
                        {
                            TokenReplayCache = new TokenReplayCache
                            {
                                OnAddReturnValue = true,
                                OnFindReturnValue = false
                            }
                        },
                        OperationResult = new TokenReplayValidationError(
                            new MessageDetail(
                                LogMessages.IDX10227,
                                LogHelper.MarkAsUnsafeSecurityArtifact("token", t => t.ToString())),
                            TokenReplayValidationFailure.NoExpiration,
                            null,
                            null),
                    },
                    new TokenReplayTheoryData("Invalid_ReplayCacheIsPresent_TokenIsAlreadyInCache")
                    {
                        ExpectedException = ExpectedException.SecurityTokenReplayDetected("IDX10228:"),
                        ExpirationTime = oneHourFromNow,
                        SecurityToken= "token",
                        ValidationParameters = new ValidationParameters
                        {
                            TokenReplayCache = new TokenReplayCache
                            {
                                OnAddReturnValue = true,
                                OnFindReturnValue = true
                            },
                        },
                        OperationResult = new TokenReplayValidationError(
                            new MessageDetail(
                                LogMessages.IDX10228,
                                LogHelper.MarkAsUnsafeSecurityArtifact("token", t => t.ToString())),
                            TokenReplayValidationFailure.TokenFoundInCache,
                            null,
                            null),
                    },
                    new TokenReplayTheoryData("Invalid_ReplayCacheIsPresent_AddingTokenToCacheFails")
                    {
                        ExpirationTime = oneHourFromNow,
                        SecurityToken= "token",
                        ValidationParameters = new ValidationParameters
                        {
                            TokenReplayCache = new TokenReplayCache
                            {
                                OnAddReturnValue = false,
                                OnFindReturnValue = false
                            }
                        },
                        ExpectedException = ExpectedException.SecurityTokenReplayDetected("IDX10229:"),
                        OperationResult = new TokenReplayValidationError(
                            new MessageDetail(
                                LogMessages.IDX10229,
                                LogHelper.MarkAsUnsafeSecurityArtifact("token", t => t.ToString())),
                            TokenReplayValidationFailure.AddToCacheFailed,
                            null,
                            null),
                    }
                };
            }
        }

        [Theory, MemberData(nameof(ValidTestCases), DisableDiscoveryEnumeration = true)]
        public void ValidReplays(TokenReplayTheoryData theoryData)
        {
            CompareContext context = TestUtilities.WriteHeader($"{this}.ValidReplays", theoryData);

            try
            {
                ValidationResult<DateTime?, ValidationError> validationResult = Validators.ValidateTokenReplay(
                    theoryData.ExpirationTime,
                    theoryData.SecurityToken,
                    theoryData.ValidationParameters,
                    theoryData.CallContext);

                if (validationResult.Succeeded)
                {
                    IdentityComparer.AreDateTimesEqualWithEpsilon(
                        validationResult.Result,
                        theoryData.OperationResult.Result,
                        1,
                        context);
                }
                else
                {
                    ValidationError validationError = validationResult.Error;
                    IdentityComparer.AreStringsEqual(
                        validationError.FailureType.Name,
                        theoryData.OperationResult.Error.FailureType.Name,
                        context);

                    theoryData.ExpectedException.ProcessException(validationError.GetException(), context);
                }
            }
            catch (Exception ex)
            {
                context.AddDiff($"Did not expect an exception: {ex}.");
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<TokenReplayTheoryData> ValidTestCases
        {
            get
            {
                DateTime now = DateTime.UtcNow;
                DateTime oneHourAgo = now.AddHours(-1);
                DateTime oneHourFromNow = now.AddHours(1);

                return new TheoryData<TokenReplayTheoryData>
                {
                    new TokenReplayTheoryData("Valid_ReplayCache_Null")
                    {
                        ExpirationTime = oneHourAgo,
                        SecurityToken = "token",
                        ValidationParameters = new ValidationParameters
                        {
                            TokenReplayCache = null
                        },
                        OperationResult = oneHourAgo,
                    },
                    new TokenReplayTheoryData("Valid_ReplayCache_NotNull")
                    {
                        ExpirationTime = oneHourFromNow,
                        SecurityToken = "token",
                        ValidationParameters = new ValidationParameters
                        {
                            TokenReplayCache = new TokenReplayCache { OnAddReturnValue = true, OnFindReturnValue = false },
                        },
                        OperationResult = oneHourFromNow,
                    },
                };
            }
        }
    }

    public class TokenReplayTheoryData : TheoryDataBase
    {
        public TokenReplayTheoryData(string testId) : base(testId) { }
        public DateTime? ExpirationTime { get; set; }
        public string SecurityToken { get; set; }
        internal ValidationParameters ValidationParameters { get; set; }
        internal ValidationResult<DateTime?, TokenReplayValidationError> OperationResult { get; set; }
    }
}
