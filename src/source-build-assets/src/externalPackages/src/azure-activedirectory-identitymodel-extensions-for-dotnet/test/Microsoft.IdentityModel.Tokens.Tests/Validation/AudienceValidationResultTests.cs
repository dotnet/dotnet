// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens.Experimental;
using Microsoft.IdentityModel.Tokens.Json.Tests;
using Xunit;

namespace Microsoft.IdentityModel.Tokens.Validation.Tests
{
    public class AudienceValidationTests
    {
        [Theory, MemberData(nameof(InvalidParameterTestCases), DisableDiscoveryEnumeration = true)]
        public void InvalidAudienceParameters(AudienceValidationTheoryData theoryData)
        {
            // TODO should not run in sequential
            AppContext.SetSwitch(AppContextSwitches.DoNotScrubExceptionsSwitch, theoryData.DoNotScrubErrorMessages);
            CompareContext context = TestUtilities.WriteHeader($"{this}.InvalidAudienceParameters", theoryData);

            try
            {
                if (theoryData.ValidAudiences != null)
                {
                    foreach (string audience in theoryData.ValidAudiences)
                        theoryData.ValidationParameters.ValidAudiences.Add(audience);
                }

                ValidationResult<string, ValidationError> validationResult =
                    Validators.ValidateAudience(
                        theoryData.TokenAudiences,
                        theoryData.SecurityToken,
                        theoryData.ValidationParameters,
                        theoryData.CallContext);

                if (validationResult.Succeeded)
                    context.Diffs.Add($"Expected validation to fail, but it succeeded. TestId {theoryData.TestId}.");
                else
                {
                    ValidationError validationError = validationResult.Error;
                    IdentityComparer.AreStringsEqual(
                        validationError.FailureType.Name,
                        theoryData.OperationResult.Error.FailureType.Name,
                        context);

                    IdentityComparer.AreStringsEqual(
                        validationError.MessageDetail.Message,
                        theoryData.OperationResult.Error.MessageDetail.Message,
                        context);

                    theoryData.ExpectedException.ProcessException(validationError.GetException(), context);
                }
            }
            catch (Exception ex)
            {
                context.Diffs.Add($"Unexpected exception thrown: {ex.Message}. TestId {theoryData.TestId}.");
            }
            finally
            {
                AppContextSwitches.ResetAllSwitches();
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<AudienceValidationTheoryData> InvalidParameterTestCases
        {
            get
            {
                return new TheoryData<AudienceValidationTheoryData>
                {
                    new AudienceValidationTheoryData("ValidationParametersNull")
                    {
                        TokenAudiences = new List<string> { "audience1" },
                        ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                        ValidationParameters = null,
                        OperationResult = new AudienceValidationError(
                            MessageDetail.NullParameter("validationParameters"),
                            ValidationFailureType.NullArgument,
                            null,
                            null,
                            null)
                    },
                    new AudienceValidationTheoryData("AudiencesNull")
                    {
                        TokenAudiences = null,
                        ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                        OperationResult = new AudienceValidationError(
                            MessageDetail.NullParameter("tokenAudiences"),
                            ValidationFailureType.NullArgument,
                            null,
                            null,
                            null)
                    },
                    new AudienceValidationTheoryData("AudiencesEmptyList")
                    {
                        TokenAudiences = new List<string> { },
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10206:"),
                        ValidationParameters = new ValidationParameters(),
                        OperationResult = new AudienceValidationError(
                            new MessageDetail(
                                LogMessages.IDX10206,
                                null),
                            AudienceValidationFailure.NoAudienceInToken,
                            null,
                            null,
                            null)
                    },
                    new AudienceValidationTheoryData("AudiencesEmptyString_ScrubbedMessage")
                    {
                        TokenAudiences = new List<string> { string.Empty },
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10215:"),
                        ValidationParameters = new ValidationParameters(),
                        ValidAudiences = ["audience1"],
                        OperationResult = new AudienceValidationError(
                            new MessageDetail(
                                LogMessages.IDX10215S),
                            AudienceValidationFailure.AudienceDidNotMatch,
                            null,
                            null,
                            null)
                    },
                    new AudienceValidationTheoryData("AudiencesWhiteSpace_ScrubbedMessage")
                    {
                        TokenAudiences = new List<string> { "    " },
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10215:"),
                        ValidationParameters = new ValidationParameters(),
                        ValidAudiences = ["audience1"],
                        OperationResult = new AudienceValidationError(
                            new MessageDetail(
                                LogMessages.IDX10215S),
                            AudienceValidationFailure.AudienceDidNotMatch,
                            null,
                            null,
                            null)
                    },

                    new AudienceValidationTheoryData("AudiencesEmptyString")
                    {
                        TokenAudiences = new List<string> { string.Empty },
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10215:"),
                        ValidationParameters = new ValidationParameters(),
                        ValidAudiences = ["audience1"],
                        OperationResult = new AudienceValidationError(
                            new MessageDetail(
                                LogMessages.IDX10215,
                                LogHelper.MarkAsNonPII(string.Empty),
                                LogHelper.MarkAsNonPII("audience1")),
                            AudienceValidationFailure.AudienceDidNotMatch,
                            null,
                            null,
                            null),
                        DoNotScrubErrorMessages = true
                    },
                    new AudienceValidationTheoryData("AudiencesWhiteSpace")
                    {
                        TokenAudiences = new List<string> { "    " },
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10215:"),
                        ValidationParameters = new ValidationParameters(),
                        ValidAudiences = ["audience1"],
                        OperationResult = new AudienceValidationError(
                            new MessageDetail(
                                LogMessages.IDX10215,
                                LogHelper.MarkAsNonPII("    "),
                                LogHelper.MarkAsNonPII("audience1")),
                            AudienceValidationFailure.AudienceDidNotMatch,
                            null,
                            null,
                            null),
                        DoNotScrubErrorMessages = true
                    },
                };
            }
        }

        [Theory, MemberData(nameof(InValidTestCases), DisableDiscoveryEnumeration = true)]
        public void InvalidAudiences(AudienceValidationTheoryData theoryData)
        {
            CompareContext context = TestUtilities.WriteHeader($"{this}.InvalidAudiences", theoryData);

            if (theoryData.ValidAudiences != null)
            {
                foreach (string audience in theoryData.ValidAudiences)
                    theoryData.ValidationParameters.ValidAudiences.Add(audience);
            }

            try
            {
                ValidationResult<string, ValidationError> validationResult =
                    Validators.ValidateAudience(
                        theoryData.TokenAudiences,
                        theoryData.SecurityToken,
                        theoryData.ValidationParameters,
                        theoryData.CallContext);

                if (validationResult.Succeeded)
                {
                    context.Diffs.Add($"Expected validation to fail, but it succeeded. TestId {theoryData.TestId}.");
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
                context.Diffs.Add($"Unexpected exception thrown: {ex.Message}. TestId {theoryData.TestId}.");
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<AudienceValidationTheoryData> InValidTestCases
        {
            get
            {
                var audience1 = "http://audience1.com";
                var audience2 = "http://audience2.com";
                List<string> audiences1 = new List<string> { "", audience1 };
                List<string> audiences1WithSlash = new List<string> { "", audience1 + "/" };
                List<string> audiences1WithTwoSlashes = new List<string> { "", audience1 + "//" };
                List<string> audiences2 = new List<string> { "", audience2 };
                List<string> audiences2WithSlash = new List<string> { "", audience2 + "/" };

                var commaAudience1 = ", " + audience1;
                var commaAudience2 = ", " + audience2;
                var audience1Slash = audience1 + "/";
                var audience2Slash = audience2 + "/";
                var commaAudience1Slash = commaAudience1 + "/";
                var commaAudience2Slash = commaAudience2 + "/";

                return new TheoryData<AudienceValidationTheoryData>
                {
                    new AudienceValidationTheoryData("SameLengthNotMatched")
                    {
                        TokenAudiences = audiences1,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10215:"),
                        ValidationParameters = new ValidationParameters(),
                        ValidAudiences = [audience2],
                        SecurityToken = JsonUtilities.CreateUnsignedJsonWebToken(JwtRegisteredClaimNames.Iss, "Issuer"),
                        OperationResult = new AudienceValidationError(
                            new MessageDetail(
                                LogMessages.IDX10215,
                                LogHelper.MarkAsNonPII(commaAudience1),
                                LogHelper.MarkAsNonPII(audience2)),
                            AudienceValidationFailure.AudienceDidNotMatch,
                            null,
                            audiences1,
                            [audience2])
                    },
                    new AudienceValidationTheoryData("AudiencesValidAudienceWithSlashNotMatched")
                    {
                        TokenAudiences = audiences1,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10215:"),
                        ValidationParameters = new ValidationParameters(),
                        ValidAudiences = [audience2 + "/"],
                        SecurityToken = JsonUtilities.CreateUnsignedJsonWebToken(JwtRegisteredClaimNames.Iss, "Issuer"),
                        OperationResult = new AudienceValidationError(
                            new MessageDetail(
                                LogMessages.IDX10215,
                                LogHelper.MarkAsNonPII(commaAudience1),
                                LogHelper.MarkAsNonPII(audience2Slash)),
                            AudienceValidationFailure.AudienceDidNotMatch,
                            null,
                            audiences1,
                            [audience2 + "/"])
                    },
                    new AudienceValidationTheoryData("AudiencesWithSlashValidAudienceSameLengthNotMatched")
                    {
                        TokenAudiences = audiences2WithSlash,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10215:"),
                        ValidationParameters = new ValidationParameters(),
                        ValidAudiences = [audience1],
                        OperationResult = new AudienceValidationError(
                            new MessageDetail(
                                LogMessages.IDX10215,
                                LogHelper.MarkAsNonPII(commaAudience2Slash),
                                LogHelper.MarkAsNonPII(audience1)),
                            AudienceValidationFailure.AudienceDidNotMatch,
                            null,
                            audiences1WithSlash,
                            [audience1])
                    },
                    new AudienceValidationTheoryData("ValidAudienceWithSlash_IgnoreTrailingSlashFalse")
                    {
                        TokenAudiences = audiences1,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10215:"),
                        ValidationParameters = new ValidationParameters{ IgnoreTrailingSlashWhenValidatingAudience = false },
                        ValidAudiences = [audience1 + "/"],
                        OperationResult = new AudienceValidationError(
                            new MessageDetail(
                                LogMessages.IDX10215,
                                LogHelper.MarkAsNonPII(commaAudience1),
                                LogHelper.MarkAsNonPII(audience1Slash)),
                            AudienceValidationFailure.AudienceDidNotMatch,
                            null,
                            audiences1,
                            [audience1 + "/"])
                    },
                    new AudienceValidationTheoryData("ValidAudiencesWithSlash_IgnoreTrailingSlashFalse")
                    {
                        TokenAudiences = audiences1,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10215:"),
                        ValidationParameters = new ValidationParameters{ IgnoreTrailingSlashWhenValidatingAudience = false },
                        ValidAudiences = audiences1WithSlash,
                        OperationResult = new AudienceValidationError(
                            new MessageDetail(
                                LogMessages.IDX10215,
                                LogHelper.MarkAsNonPII(commaAudience1),
                                LogHelper.MarkAsNonPII(commaAudience1Slash)),
                            AudienceValidationFailure.AudienceDidNotMatch,
                            null,
                            audiences1,
                            audiences1WithSlash)
                    },
                    new AudienceValidationTheoryData("ValidAudienceWithExtraChar")
                    {
                        TokenAudiences = audiences1,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10215:"),
                        ValidationParameters = new ValidationParameters(),
                        ValidAudiences = [audience1 + "A"],
                        OperationResult = new AudienceValidationError(
                            new MessageDetail(
                                LogMessages.IDX10215,
                                LogHelper.MarkAsNonPII(commaAudience1),
                                LogHelper.MarkAsNonPII(audience1 + "A")),
                            AudienceValidationFailure.AudienceDidNotMatch,
                            null,
                            audiences1,
                            [audience1 + "A"])
                    },
                    new AudienceValidationTheoryData("AudienceWithDoubleSlash_IgnoreTrailingSlashTrue")
                    {
                        TokenAudiences = audiences1,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10215:"),
                        ValidationParameters = new ValidationParameters(),
                        ValidAudiences = [audience1 + "//"],
                        OperationResult = new AudienceValidationError(
                            new MessageDetail(
                                LogMessages.IDX10215,
                                LogHelper.MarkAsNonPII(commaAudience1),
                                LogHelper.MarkAsNonPII(audience1 + "//")),
                            AudienceValidationFailure.AudienceDidNotMatch,
                            null,
                            audiences1,
                            [audience1 + "//"])
                    },
                    new AudienceValidationTheoryData("AudiencesWithDoubleSlash_IgnoreTrailingSlashTrue")
                    {
                        TokenAudiences = audiences1,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10215:"),
                        ValidationParameters = new ValidationParameters(),
                        ValidAudiences = audiences1WithTwoSlashes,
                        OperationResult = new AudienceValidationError(
                            new MessageDetail(
                                LogMessages.IDX10215,
                                LogHelper.MarkAsNonPII(commaAudience1),
                                LogHelper.MarkAsNonPII(commaAudience1 + "//")),
                            AudienceValidationFailure.AudienceDidNotMatch,
                            null,
                            audiences1,
                            audiences1WithTwoSlashes)
                    },
                    new AudienceValidationTheoryData("TokenAudienceWithSlash_IgnoreTrailingSlashFalse")
                    {
                        TokenAudiences = audiences1WithSlash,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10215:"),
                        ValidationParameters = new ValidationParameters{ IgnoreTrailingSlashWhenValidatingAudience = false },
                        ValidAudiences = [audience1],
                        OperationResult = new AudienceValidationError(
                            new MessageDetail(
                                LogMessages.IDX10215,
                                LogHelper.MarkAsNonPII(commaAudience1Slash),
                                LogHelper.MarkAsNonPII(audience1)),
                            AudienceValidationFailure.AudienceDidNotMatch,
                            null,
                            audiences1WithSlash,
                            [audience1])
                    },
                    new AudienceValidationTheoryData("TokenAudienceWithSlashNotEqual")
                    {
                        TokenAudiences = audiences2WithSlash,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10215:"),
                        ValidationParameters = new ValidationParameters(),
                        ValidAudiences = [audience1],
                        OperationResult = new AudienceValidationError(
                            new MessageDetail(
                                LogMessages.IDX10215,
                                LogHelper.MarkAsNonPII(commaAudience2Slash),
                                LogHelper.MarkAsNonPII(audience1)),
                            AudienceValidationFailure.AudienceDidNotMatch,
                            null,
                            audiences2WithSlash,
                            [audience1])
                    },
                    new AudienceValidationTheoryData("TokenAudiencesWithSlash_IgnoreTrailingSlashFalse")
                    {
                        TokenAudiences = audiences1WithSlash,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10215:"),
                        ValidationParameters = new ValidationParameters{ IgnoreTrailingSlashWhenValidatingAudience = false },
                        ValidAudiences = [audience1],
                        OperationResult = new AudienceValidationError(
                            new MessageDetail(
                                LogMessages.IDX10215,
                                LogHelper.MarkAsNonPII(commaAudience1Slash),
                                LogHelper.MarkAsNonPII(audience1)),
                            AudienceValidationFailure.AudienceDidNotMatch,
                            null,
                            audiences1WithSlash,
                            [audience1])
                    },
                    new AudienceValidationTheoryData("TokenAudiencesWithSlashValidAudiencesNotMatched_IgnoreTrailingSlashTrue")
                    {
                        TokenAudiences = audiences1WithSlash,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10215:"),
                        ValidationParameters = new ValidationParameters(),
                        ValidAudiences = audiences2,
                        OperationResult = new AudienceValidationError(
                            new MessageDetail(
                                LogMessages.IDX10215,
                                LogHelper.MarkAsNonPII(commaAudience1Slash),
                                LogHelper.MarkAsNonPII(commaAudience2)),
                            AudienceValidationFailure.AudienceDidNotMatch,
                            null,
                            audiences1WithSlash,
                            audiences2)
                    },
                    new AudienceValidationTheoryData("TokenAudienceWithTwoSlashesVPTrue")
                    {
                        TokenAudiences = audiences1WithTwoSlashes,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10215:"),
                        ValidationParameters = new ValidationParameters(),
                        ValidAudiences = [audience1],
                        OperationResult = new AudienceValidationError(
                            new MessageDetail(
                                LogMessages.IDX10215,
                                LogHelper.MarkAsNonPII(commaAudience1 + "//"),
                                LogHelper.MarkAsNonPII(audience1)),
                            AudienceValidationFailure.AudienceDidNotMatch,
                            null,
                            audiences1WithTwoSlashes,
                            [audience1])
                    }
                };
            }
        }

        [Theory, MemberData(nameof(ValidTestCases), DisableDiscoveryEnumeration = true)]
        public void ValidAudiences(AudienceValidationTheoryData theoryData)
        {
            CompareContext context = TestUtilities.WriteHeader($"{this}.ValidAudiences", theoryData);

            if (theoryData.ValidAudiences != null)
            {
                foreach (string audience in theoryData.ValidAudiences)
                    theoryData.ValidationParameters.ValidAudiences.Add(audience);
            }

            try
            {
                ValidationResult<string, ValidationError> validationResult =
                    Validators.ValidateAudience(
                        theoryData.TokenAudiences,
                        theoryData.SecurityToken,
                        theoryData.ValidationParameters,
                        theoryData.CallContext);

                if (validationResult.Succeeded)
                {
                    IdentityComparer.AreStringsEqual(
                        validationResult.Result,
                        theoryData.OperationResult.Result,
                        context);
                }
                else
                {
                    context.Diffs.Add($"Expected validation to succeed, but it failed with: {validationResult.Error}.");
                }
            }
            catch (Exception ex)
            {
                context.Diffs.Add($"Unexpected exception thrown: {ex.Message}. TestId {theoryData.TestId}.");
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<AudienceValidationTheoryData> ValidTestCases
        {
            get
            {
                var audience1 = "http://audience1.com";
                var audience1Slash = audience1 + "/";
                List<string> audiences1 = new List<string> { "", audience1 };
                List<string> audiences1WithSlash = new List<string> { "", audience1 + "/" };

                return new TheoryData<AudienceValidationTheoryData>
                {
                    new AudienceValidationTheoryData("SameLengthMatched")
                    {
                        TokenAudiences = audiences1,
                        ValidationParameters = new ValidationParameters(),
                        ValidAudiences = [audience1],
                        SecurityToken = JsonUtilities.CreateUnsignedJsonWebToken(JwtRegisteredClaimNames.Iss, "Issuer"),
                        OperationResult = audience1
                    },
                    new AudienceValidationTheoryData("AudienceWithSlash_IgnoreTrailingSlashTrue")
                    {
                        TokenAudiences = audiences1,
                        ValidationParameters = new ValidationParameters(),
                        ValidAudiences = [audience1 + "/"],
                        OperationResult = audience1
                    },
                    new AudienceValidationTheoryData("AudiencesWithSlash_IgnoreTrailingSlashTrue")
                    {
                        TokenAudiences = audiences1,
                        ValidationParameters = new ValidationParameters(),
                        ValidAudiences = audiences1WithSlash,
                        OperationResult = audience1
                    },
                    new AudienceValidationTheoryData("TokenAudienceWithSlash_IgnoreTrailingSlashTrue")
                    {
                        TokenAudiences = audiences1WithSlash,
                        ValidationParameters = new ValidationParameters(),
                        ValidAudiences = [audience1],
                        OperationResult = audience1Slash
                    },
                    new AudienceValidationTheoryData("TokenAudiencesWithSlash_IgnoreTrailingSlashTrue")
                    {
                        TokenAudiences = audiences1WithSlash,
                        ValidationParameters = new ValidationParameters(),
                        ValidAudiences = [audience1],
                        OperationResult = audience1Slash
                    }
                };
            }
        }

        public class AudienceValidationTheoryData : TheoryDataBase
        {
            public AudienceValidationTheoryData(string testId) : base(testId) { }
            public List<string> TokenAudiences { get; set; }
            public SecurityToken SecurityToken { get; set; }
            internal ValidationParameters ValidationParameters { get; set; } = new ValidationParameters();
            internal ValidationFailureType ValidationFailure { get; set; }
            public List<string> ValidAudiences { get; set; }
            internal ValidationResult<string, AudienceValidationError> OperationResult { get; set; }
            internal bool DoNotScrubErrorMessages { get; set; }
        }
    }
}
