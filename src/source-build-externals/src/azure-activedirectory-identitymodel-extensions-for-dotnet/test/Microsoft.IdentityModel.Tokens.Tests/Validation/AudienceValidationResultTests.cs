// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens.Json.Tests;
using Xunit;
using System.Collections.Generic;

namespace Microsoft.IdentityModel.Tokens.Validation.Tests
{
    public class AudienceValidationResultTests
    {
        [Theory, MemberData(nameof(ValidateAudienceParametersTestCases), DisableDiscoveryEnumeration = true)]
        public void ValidateAudienceParameters(AudienceValidationTheoryData theoryData)
        {
            CompareContext context = TestUtilities.WriteHeader($"{this}.AudienceValidatorResultTests", theoryData);

            AudienceValidationResult audienceValidationResult = Validators.ValidateAudience(
                theoryData.Audiences,
                theoryData.SecurityToken,
                theoryData.ValidationParameters,
                new CallContext());

            if (audienceValidationResult.Exception == null)
                theoryData.ExpectedException.ProcessNoException();
            else
                theoryData.ExpectedException.ProcessException(audienceValidationResult.Exception, context);

            IdentityComparer.AreAudienceValidationResultsEqual(
                audienceValidationResult,
                theoryData.AudienceValidationResult,
                context);

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<AudienceValidationTheoryData> ValidateAudienceParametersTestCases
        {
            get
            {
                return new TheoryData<AudienceValidationTheoryData>
                {
                    new AudienceValidationTheoryData
                    {
                        Audiences = new List<string> { "audience1" },
                        ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                        TestId = "ValidationParametersNull",
                        ValidationParameters = null,
                        AudienceValidationResult = new AudienceValidationResult(
                            "audience1",
                            ValidationFailureType.NullArgument,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10000,
                                    LogHelper.MarkAsNonPII("validationParameters")),
                                typeof(ArgumentNullException),
                                new StackFrame(true),
                                null)),
                    },
                    new AudienceValidationTheoryData
                    {
                        Audiences = new List<string> { "" },
                        ExpectedException =  ExpectedException.SecurityTokenInvalidAudienceException("IDX10214:"),
                        TestId = "AudiencesEmptyString",
                        ValidationParameters = new TokenValidationParameters{ ValidAudience = "audience"},
                        AudienceValidationResult = new AudienceValidationResult(
                            "",
                            ValidationFailureType.NullArgument,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10214,
                                    LogHelper.MarkAsNonPII(""),
                                    LogHelper.MarkAsNonPII("audience"),
                                    LogHelper.MarkAsNonPII("null")),
                                typeof(SecurityTokenInvalidAudienceException),
                                new StackFrame(true),
                                null)),
                    },
                    new AudienceValidationTheoryData
                    {
                        Audiences = new List<string> { "    " },
                        ExpectedException =  ExpectedException.SecurityTokenInvalidAudienceException("IDX10214:"),
                        TestId = "AudiencesWhiteSpace",
                        ValidationParameters = new TokenValidationParameters{ ValidAudience = "audience"},
                        AudienceValidationResult = new AudienceValidationResult(
                            "    ",
                            ValidationFailureType.NullArgument,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10214,
                                    LogHelper.MarkAsNonPII("    "),
                                    LogHelper.MarkAsNonPII("audience"),
                                    LogHelper.MarkAsNonPII("null")),
                                typeof(SecurityTokenInvalidAudienceException),
                                new StackFrame(true),
                                null)),
                    },
                    new AudienceValidationTheoryData
                    {
                        Audiences = null,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10207:"),
                        TestId = "AudiencesNull",
                        AudienceValidationResult = new AudienceValidationResult(
                            "null",
                            ValidationFailureType.NullArgument,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10207,
                                    null),
                                typeof(SecurityTokenInvalidAudienceException),
                                new StackFrame(true),
                                null)),
                    },
                    new AudienceValidationTheoryData
                    {
                        Audiences = new List<string>{ },
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10206:"),
                        TestId = "AudiencesEmptyList",
                        ValidationParameters = new TokenValidationParameters{ ValidAudience = "audience"},
                        AudienceValidationResult = new AudienceValidationResult(
                            "empty",
                            ValidationFailureType.NullArgument,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10206,
                                    null),
                                typeof(SecurityTokenInvalidAudienceException),
                                new StackFrame(true),
                                null)),
                    },
                    new AudienceValidationTheoryData
                    {
                        Audiences = new List<string>{ },
                        TestId = "ValidateAudienceFalseAudiencesEmptyList",
                        ValidationParameters = new TokenValidationParameters{ ValidateAudience = false },
                        AudienceValidationResult = new AudienceValidationResult("empty")
                    },
                    new AudienceValidationTheoryData
                    {
                        Audiences = null,
                        TestId = "ValidateAudienceFalseAudiencesNull",
                        ValidationParameters = new TokenValidationParameters{ ValidateAudience = false },
                        AudienceValidationResult = new AudienceValidationResult("null")
                    },
                    new AudienceValidationTheoryData
                    {
                        Audiences = new List<string> { "audience1" },
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10208:"),
                        TestId = "ValidAudienceEmptyString",
                        ValidationParameters = new TokenValidationParameters{ ValidAudience = "" },
                        AudienceValidationResult = new AudienceValidationResult(
                            "audience1",
                            ValidationFailureType.NullArgument,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10208,
                                    null),
                                typeof(SecurityTokenInvalidAudienceException),
                                new StackFrame(true),
                                null)),
                    },
                    new AudienceValidationTheoryData
                    {
                        Audiences = new List<string> { "audience1" },
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10208:"),
                        TestId = "ValidAudienceWhiteSpace",
                        ValidationParameters = new TokenValidationParameters{ ValidAudience = "    " },
                        AudienceValidationResult = new AudienceValidationResult(
                            "audience1",
                            ValidationFailureType.NullArgument,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10208,
                                    null),
                                typeof(SecurityTokenInvalidAudienceException),
                                new StackFrame(true),
                                null)),
                    },
                    new AudienceValidationTheoryData
                    {
                        Audiences = new List<string> { "audience1" },
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10214:"),
                        TestId = "ValidAudiencesEmptyString",
                        ValidationParameters = new TokenValidationParameters{ ValidAudiences = new List<string>{ "" } },
                        AudienceValidationResult = new AudienceValidationResult(
                            "audience1",
                            ValidationFailureType.NullArgument,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10214,
                                    LogHelper.MarkAsNonPII("audience1"),
                                    LogHelper.MarkAsNonPII("null"),
                                    LogHelper.MarkAsNonPII("")),
                                typeof(SecurityTokenInvalidAudienceException),
                                new StackFrame(true),
                                null)),
                    },
                    new AudienceValidationTheoryData
                    {
                        Audiences = new List<string> { "audience1" },
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10214:"),
                        TestId = "ValidAudiencesWhiteSpace",
                        ValidationParameters = new TokenValidationParameters{ ValidAudiences = new List<string>{ "    " } },
                        AudienceValidationResult = new AudienceValidationResult(
                            "audience1",
                            ValidationFailureType.NullArgument,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10214,
                                    LogHelper.MarkAsNonPII("audience1"),
                                    LogHelper.MarkAsNonPII("null"),
                                    LogHelper.MarkAsNonPII("    ")),
                                typeof(SecurityTokenInvalidAudienceException),
                                new StackFrame(true),
                                null)),
                    },
                    new AudienceValidationTheoryData
                    {
                        Audiences = new List<string> { "audience1" },
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10208:"),
                        TestId = "ValidateAudienceTrueValidAudienceAndValidAudiencesNull",
                        AudienceValidationResult = new AudienceValidationResult(
                            "audience1",
                            ValidationFailureType.NullArgument,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10208,
                                    null),
                                typeof(SecurityTokenInvalidAudienceException),
                                new StackFrame(true),
                                null)),
                    }
                };
            }
        }

        [Theory, MemberData(nameof(ValidateAudienceTheoryData))]
        public void ValidateAudience(AudienceValidationTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateAudience", theoryData);
            AudienceValidationResult audienceValidationResult = Validators.ValidateAudience(
                theoryData.Audiences,
                theoryData.SecurityToken,
                theoryData.ValidationParameters,
                new CallContext());

            if (audienceValidationResult.Exception != null)
                theoryData.ExpectedException.ProcessException(audienceValidationResult.Exception);
            else
                theoryData.ExpectedException.ProcessNoException(context);

            IdentityComparer.AreAudienceValidationResultsEqual(
                audienceValidationResult,
                theoryData.AudienceValidationResult,
                context);

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<AudienceValidationTheoryData> ValidateAudienceTheoryData
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
                    new AudienceValidationTheoryData
                    {
                        Audiences = audiences1,
                        TestId = "SameLengthMatched",
                        ValidationParameters = new TokenValidationParameters{ ValidAudience = audience1 },
                        SecurityToken = JsonUtilities.CreateUnsignedJsonWebToken(JwtRegisteredClaimNames.Iss, "Issuer"),
                        AudienceValidationResult = new AudienceValidationResult(audience1)
                    },
                    new AudienceValidationTheoryData
                    {
                        Audiences = audiences1,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10214:"),
                        TestId = "SameLengthNotMatched",
                        ValidationParameters = new TokenValidationParameters{ ValidAudience = audience2 },
                        SecurityToken = JsonUtilities.CreateUnsignedJsonWebToken(JwtRegisteredClaimNames.Iss, "Issuer"),
                        AudienceValidationResult = new AudienceValidationResult(
                            commaAudience1,
                            ValidationFailureType.NullArgument,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10214,
                                    LogHelper.MarkAsNonPII(commaAudience1),
                                    LogHelper.MarkAsNonPII(audience2),
                                    LogHelper.MarkAsNonPII("null")),
                                typeof(SecurityTokenInvalidAudienceException),
                                new StackFrame(true),
                                null)),
                    },
                    new AudienceValidationTheoryData
                    {
                        Audiences = audiences1,
                        TestId = "NoMatchTVPValidateFalse",
                        ValidationParameters = new TokenValidationParameters{ ValidAudience = audience2, ValidateAudience = false },
                        SecurityToken = JsonUtilities.CreateUnsignedJsonWebToken(JwtRegisteredClaimNames.Iss, "Issuer"),
                        AudienceValidationResult = new AudienceValidationResult(commaAudience1)
                    },
                    new AudienceValidationTheoryData
                    {
                        Audiences = audiences1,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10214:"),
                        TestId = "AudiencesValidAudienceWithSlashNotMatched",
                        ValidationParameters = new TokenValidationParameters{ ValidAudience = audience2 + "/" },
                        SecurityToken = JsonUtilities.CreateUnsignedJsonWebToken(JwtRegisteredClaimNames.Iss, "Issuer"),
                        AudienceValidationResult = new AudienceValidationResult(
                            commaAudience1,
                            ValidationFailureType.NullArgument,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10214,
                                    LogHelper.MarkAsNonPII(commaAudience1),
                                    LogHelper.MarkAsNonPII(audience2Slash),
                                    LogHelper.MarkAsNonPII("null")),
                                typeof(SecurityTokenInvalidAudienceException),
                                new StackFrame(true),
                                null)),
                    },
                    new AudienceValidationTheoryData
                    {
                        Audiences = audiences2WithSlash,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10214:"),
                        TestId = "AudiencesWithSlashValidAudienceSameLengthNotMatched",
                        ValidationParameters = new TokenValidationParameters{ ValidAudience = audience1 },
                        AudienceValidationResult = new AudienceValidationResult(
                            commaAudience2Slash,
                            ValidationFailureType.NullArgument,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10214,
                                    LogHelper.MarkAsNonPII(commaAudience2Slash),
                                    LogHelper.MarkAsNonPII(audience1),
                                    LogHelper.MarkAsNonPII("null")),
                                typeof(SecurityTokenInvalidAudienceException),
                                new StackFrame(true),
                                null)),
                    },
                    new AudienceValidationTheoryData
                    {
                        Audiences = audiences1,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10214:"),
                        TestId = "ValidAudienceWithSlashTVPFalse",
                        ValidationParameters = new TokenValidationParameters{ IgnoreTrailingSlashWhenValidatingAudience = false, ValidAudience = audience1 + "/" },
                        AudienceValidationResult = new AudienceValidationResult(
                            commaAudience1,
                            ValidationFailureType.NullArgument,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10214,
                                    LogHelper.MarkAsNonPII(commaAudience1),
                                    LogHelper.MarkAsNonPII(audience1Slash),
                                    LogHelper.MarkAsNonPII("null")),
                                typeof(SecurityTokenInvalidAudienceException),
                                new StackFrame(true),
                                null)),
                    },
                    new AudienceValidationTheoryData
                    {
                        Audiences = audiences1,
                        TestId = "ValidAudienceWithSlashTVPTrue",
                        ValidationParameters = new TokenValidationParameters{ ValidAudience = audience1 + "/" },
                        AudienceValidationResult = new AudienceValidationResult(audience1)
                    },
                    new AudienceValidationTheoryData
                    {
                        Audiences = audiences1,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10214:"),
                        TestId = "ValidAudiencesWithSlashTVPFalse",
                        ValidationParameters = new TokenValidationParameters{ IgnoreTrailingSlashWhenValidatingAudience = false, ValidAudiences = audiences1WithSlash },
                        AudienceValidationResult = new AudienceValidationResult(
                            commaAudience1,
                            ValidationFailureType.NullArgument,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10214,
                                    LogHelper.MarkAsNonPII(commaAudience1),
                                    LogHelper.MarkAsNonPII("null"),
                                    LogHelper.MarkAsNonPII(commaAudience1Slash)),
                                typeof(SecurityTokenInvalidAudienceException),
                                new StackFrame(true),
                                null)),
                    },
                    new AudienceValidationTheoryData
                    {
                        Audiences = audiences1,
                        TestId = "ValidAudiencesWithSlashTVPTrue",
                        ValidationParameters = new TokenValidationParameters{ ValidAudiences = audiences1WithSlash },
                        AudienceValidationResult = new AudienceValidationResult(audience1)
                    },
                    new AudienceValidationTheoryData
                    {
                        Audiences = audiences1,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10214:"),
                        TestId = "ValidAudienceWithExtraChar",
                        ValidationParameters = new TokenValidationParameters{ ValidAudience = audience1 + "A" },
                        AudienceValidationResult = new AudienceValidationResult(
                            commaAudience1,
                            ValidationFailureType.NullArgument,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10214,
                                    LogHelper.MarkAsNonPII(commaAudience1),
                                    LogHelper.MarkAsNonPII(audience1 + "A"),
                                    LogHelper.MarkAsNonPII("null")),
                                typeof(SecurityTokenInvalidAudienceException),
                                new StackFrame(true),
                                null)),
                    },
                    new AudienceValidationTheoryData
                    {
                        Audiences = audiences1,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10214:"),
                        TestId = "ValidAudienceWithDoubleSlashTVPTrue",
                        ValidationParameters = new TokenValidationParameters{ ValidAudience = audience1 + "//" },
                        AudienceValidationResult = new AudienceValidationResult(
                            commaAudience1,
                            ValidationFailureType.NullArgument,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10214,
                                    LogHelper.MarkAsNonPII(commaAudience1),
                                    LogHelper.MarkAsNonPII(audience1 + "//"),
                                    LogHelper.MarkAsNonPII("null")),
                                typeof(SecurityTokenInvalidAudienceException),
                                new StackFrame(true),
                                null)),
                    },
                    new AudienceValidationTheoryData
                    {
                        Audiences = audiences1,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10214:"),
                        TestId = "ValidAudiencesWithDoubleSlashTVPTrue",
                        ValidationParameters = new TokenValidationParameters{ ValidAudiences = audiences1WithTwoSlashes },
                        AudienceValidationResult = new AudienceValidationResult(
                            commaAudience1,
                            ValidationFailureType.NullArgument,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10214,
                                    LogHelper.MarkAsNonPII(commaAudience1),
                                    LogHelper.MarkAsNonPII("null"),
                                    LogHelper.MarkAsNonPII(commaAudience1 + "//")),
                                typeof(SecurityTokenInvalidAudienceException),
                                new StackFrame(true),
                                null)),
                    },
                    new AudienceValidationTheoryData
                    {
                        Audiences = audiences1WithSlash,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10214:"),
                        TestId = "TokenAudienceWithSlashTVPFalse",
                        ValidationParameters = new TokenValidationParameters{ IgnoreTrailingSlashWhenValidatingAudience = false, ValidAudience = audience1 },
                        AudienceValidationResult = new AudienceValidationResult(
                            commaAudience1Slash,
                            ValidationFailureType.NullArgument,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10214,
                                    LogHelper.MarkAsNonPII(commaAudience1Slash),
                                    LogHelper.MarkAsNonPII(audience1),
                                    LogHelper.MarkAsNonPII("null")),
                                typeof(SecurityTokenInvalidAudienceException),
                                new StackFrame(true),
                                null)),
                    },
                    new AudienceValidationTheoryData
                    {
                        Audiences = audiences1WithSlash,
                        TestId = "TokenAudienceWithSlashTVPTrue",
                        ValidationParameters = new TokenValidationParameters{ ValidAudience = audience1 },
                        AudienceValidationResult = new AudienceValidationResult(audience1Slash)
                    },
                    new AudienceValidationTheoryData
                    {
                        Audiences = audiences2WithSlash,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10214:"),
                        TestId = "TokenAudienceWithSlashNotEqual",
                        ValidationParameters = new TokenValidationParameters{ ValidAudience = audience1 },
                        AudienceValidationResult = new AudienceValidationResult(
                            commaAudience2Slash,
                            ValidationFailureType.NullArgument,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10214,
                                    LogHelper.MarkAsNonPII(commaAudience2Slash),
                                    LogHelper.MarkAsNonPII(audience1),
                                    LogHelper.MarkAsNonPII("null")),
                                typeof(SecurityTokenInvalidAudienceException),
                                new StackFrame(true),
                                null)),
                    },
                    new AudienceValidationTheoryData
                    {
                        Audiences = audiences1WithSlash,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10214:"),
                        TestId = "TokenAudiencesWithSlashTVPFalse",
                        ValidationParameters = new TokenValidationParameters{ IgnoreTrailingSlashWhenValidatingAudience = false, ValidAudience = audience1 },
                        AudienceValidationResult = new AudienceValidationResult(
                            commaAudience1Slash,
                            ValidationFailureType.NullArgument,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10214,
                                    LogHelper.MarkAsNonPII(commaAudience1Slash),
                                    LogHelper.MarkAsNonPII(audience1),
                                    LogHelper.MarkAsNonPII("null")),
                                typeof(SecurityTokenInvalidAudienceException),
                                new StackFrame(true),
                                null)),
                    },
                    new AudienceValidationTheoryData
                    {
                        Audiences = audiences1WithSlash,
                        TestId = "TokenAudiencesWithSlashTVPTrue",
                        ValidationParameters = new TokenValidationParameters{ ValidAudience = audience1 },
                        AudienceValidationResult = new AudienceValidationResult(audience1Slash)
                    },
                    new AudienceValidationTheoryData
                    {
                        Audiences = audiences1WithSlash,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10214:"),
                        TestId = "TokenAudiencesWithSlashValidAudiencesNotMatchedTVPTrue",
                        ValidationParameters = new TokenValidationParameters{ ValidAudiences = audiences2 },
                        AudienceValidationResult = new AudienceValidationResult(
                            commaAudience1Slash,
                            ValidationFailureType.NullArgument,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10214,
                                    LogHelper.MarkAsNonPII(commaAudience1Slash),
                                    LogHelper.MarkAsNonPII("null"),
                                    LogHelper.MarkAsNonPII(commaAudience2)),
                                typeof(SecurityTokenInvalidAudienceException),
                                new StackFrame(true),
                                null)),
                    },
                    new AudienceValidationTheoryData
                    {
                        Audiences = audiences1WithTwoSlashes,
                        ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10214:"),
                        TestId = "TokenAudienceWithTwoSlashesTVPTrue",
                        ValidationParameters = new TokenValidationParameters{ ValidAudience = audience1 },
                        AudienceValidationResult = new AudienceValidationResult(
                            commaAudience1 + "//",
                            ValidationFailureType.NullArgument,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10214,
                                    LogHelper.MarkAsNonPII(commaAudience1 + "//"),
                                    LogHelper.MarkAsNonPII(audience1),
                                    LogHelper.MarkAsNonPII("null")),
                                typeof(SecurityTokenInvalidAudienceException),
                                new StackFrame(true),
                                null)),
                    }
                };
            }
        }

        public class AudienceValidationTheoryData : TheoryDataBase
        {
            public List<string> Audiences { get; set; }

            internal AudienceValidationResult AudienceValidationResult { get; set; }

            public SecurityToken SecurityToken { get; set; }

            public TokenValidationParameters ValidationParameters { get; set; } = new TokenValidationParameters();

            internal ValidationFailureType ValidationFailureType { get; set; }
        }


    }
}
