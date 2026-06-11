// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens.Experimental;
using Xunit;

namespace Microsoft.IdentityModel.Tokens.Validation.Tests
{
    public class LifetimeValidationTests
    {
        [Theory, MemberData(nameof(InvalidTestCases), DisableDiscoveryEnumeration = true)]
        public void InvalidLifetimes(ValidateLifetimeTheoryData theoryData)
        {
            CompareContext context = TestUtilities.WriteHeader($"{this}.InvalidLifetimes", theoryData);

            try
            {
                ValidationResult<ValidatedLifetime, ValidationError> validationResult =
                    Validators.ValidateLifetime(
                        theoryData.NotBefore,
                        theoryData.Expires,
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
                        theoryData.OperationResult.Error!.FailureType.Name,
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

        public static TheoryData<ValidateLifetimeTheoryData> InvalidTestCases
        {
            get
            {
                MockTimeProvider timeProvider = new MockTimeProvider();
                DateTime utcNow = timeProvider.GetUtcNow().UtcDateTime;
                DateTime oneHourFromNow = utcNow.AddHours(1);
                DateTime twoHoursFromNow = utcNow.AddHours(2);
                DateTime sixMinutesFromNow = utcNow.AddMinutes(6);
                DateTime oneHourAgo = utcNow.AddHours(-1);
                DateTime twoHoursAgo = utcNow.AddHours(-2);
                DateTime sixMinutesAgo = utcNow.AddMinutes(-6);

                return new TheoryData<ValidateLifetimeTheoryData>
                {
                    new ValidateLifetimeTheoryData("ValidationParametersIsNull")
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                        Expires = oneHourFromNow,
                        NotBefore = oneHourAgo,
                        ValidationParameters = null,
                        OperationResult = new LifetimeValidationError(
                            new MessageDetail(LogMessages.IDX10000, "validationParameters"),
                            ValidationFailureType.NullArgument,
                            null,
                            oneHourAgo,
                            oneHourFromNow),
                    },
                    new ValidateLifetimeTheoryData("ExpiresIsNull")
                    {
                        ExpectedException = ExpectedException.SecurityTokenInvalidLifetimeException("IDX10225:"),
                        NotBefore = oneHourAgo,
                        ValidationParameters = new ValidationParameters() { TimeProvider = timeProvider },
                        OperationResult = new LifetimeValidationError(
                            new MessageDetail(LogMessages.IDX10225, "null"),
                            LifetimeValidationFailure.NoExpirationTime,
                            null,
                            oneHourAgo,
                            null),
                    },
                    new ValidateLifetimeTheoryData("NotBeforeIsAfterExpires")
                    {
                        ExpectedException = ExpectedException.SecurityTokenInvalidLifetimeException("IDX10224:"),
                        Expires = oneHourAgo,
                        NotBefore = oneHourFromNow,
                        ValidationParameters = new ValidationParameters() { TimeProvider = timeProvider },
                        OperationResult = new LifetimeValidationError(
                            new MessageDetail(
                                LogMessages.IDX10224,
                                LogHelper.MarkAsNonPII(oneHourFromNow),
                                LogHelper.MarkAsNonPII(oneHourAgo)),
                            LifetimeValidationFailure.NotbeforeGreaterThanExpirationTime,
                            null,
                            oneHourFromNow,
                            oneHourAgo),
                    },
                    new ValidateLifetimeTheoryData("NotYetValid")
                    {
                        ExpectedException = ExpectedException.SecurityTokenInvalidLifetimeException("IDX10222:"),
                        Expires = twoHoursFromNow,
                        NotBefore = oneHourFromNow,
                        ValidationParameters = new ValidationParameters() { TimeProvider = timeProvider },
                        OperationResult = new LifetimeValidationError(
                            new MessageDetail(
                                LogMessages.IDX10222,
                                LogHelper.MarkAsNonPII(oneHourFromNow),
                                LogHelper.MarkAsNonPII(utcNow)),
                            LifetimeValidationFailure.NotYetValid,
                            null,
                            oneHourFromNow,
                            twoHoursFromNow),
                    },
                    new ValidateLifetimeTheoryData("Expired")
                    {
                        ExpectedException = ExpectedException.SecurityTokenInvalidLifetimeException("IDX10223:"),
                        Expires = oneHourAgo,
                        NotBefore = twoHoursAgo,
                        ValidationParameters = new ValidationParameters() { TimeProvider = timeProvider },
                        OperationResult = new LifetimeValidationError(
                            new MessageDetail(
                                LogMessages.IDX10223,
                                LogHelper.MarkAsNonPII(oneHourAgo),
                                LogHelper.MarkAsNonPII(utcNow)),
                            LifetimeValidationFailure.Expired,
                            null,
                            twoHoursAgo,
                            oneHourAgo),
                    },
                    new ValidateLifetimeTheoryData("NotYetValid_SkewForward")
                    {
                        ExpectedException = ExpectedException.SecurityTokenInvalidLifetimeException("IDX10222:"),
                        Expires = oneHourFromNow,
                        NotBefore = sixMinutesFromNow,
                        ValidationParameters = new ValidationParameters {
                            ClockSkew = TimeSpan.FromMinutes(5),
                            TimeProvider = timeProvider
                        },
                        OperationResult = new LifetimeValidationError(
                            new MessageDetail(
                                LogMessages.IDX10222,
                                LogHelper.MarkAsNonPII(sixMinutesFromNow),
                                LogHelper.MarkAsNonPII(utcNow)),
                            LifetimeValidationFailure.NotYetValid,
                            null,
                            sixMinutesFromNow,
                            oneHourFromNow),
                    },
                    new ValidateLifetimeTheoryData("Expired_SkewBackward")
                    {
                        ExpectedException = ExpectedException.SecurityTokenInvalidLifetimeException("IDX10223:"),
                        Expires = sixMinutesAgo,
                        NotBefore = twoHoursAgo,
                        ValidationParameters = new ValidationParameters {
                            ClockSkew = TimeSpan.FromMinutes(5),
                            TimeProvider = timeProvider
                        },
                        OperationResult = new LifetimeValidationError(
                            new MessageDetail(
                                LogMessages.IDX10223,
                                LogHelper.MarkAsNonPII(sixMinutesAgo),
                                LogHelper.MarkAsNonPII(utcNow)),
                            LifetimeValidationFailure.Expired,
                            null,
                            twoHoursAgo,
                            sixMinutesAgo),
                    }
                };
            }
        }

        [Theory, MemberData(nameof(ValidTestCases), DisableDiscoveryEnumeration = true)]
        public void ValidLifetimes(ValidateLifetimeTheoryData theoryData)
        {
            CompareContext context = TestUtilities.WriteHeader($"{this}.ValidLifetimes", theoryData);

            try
            {
                ValidationResult<ValidatedLifetime, ValidationError> validationResult = Validators.ValidateLifetime(
                    theoryData.NotBefore,
                    theoryData.Expires,
                    theoryData.SecurityToken,
                    theoryData.ValidationParameters,
                    theoryData.CallContext);

                if (validationResult.Succeeded)
                {
                    IdentityComparer.AreValidatedLifetimesEqual(
                        theoryData.OperationResult.Result,
                        validationResult.Result,
                        context);
                }
                else
                {
                    context.AddDiff($"Expected validationResult to succeed, but it failed with: {validationResult.Error}.");
                }
            }
            catch (Exception ex)
            {
                context.AddDiff($"Did not expect an exception: {ex}.");
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ValidateLifetimeTheoryData> ValidTestCases
        {
            get
            {
                MockTimeProvider timeProvider = new MockTimeProvider();
                DateTime utcNow = timeProvider.GetUtcNow().UtcDateTime;
                DateTime oneHourFromNow = utcNow.AddHours(1);
                DateTime twoMinutesFromNow = utcNow.AddMinutes(2);
                DateTime oneHourAgo = utcNow.AddHours(-1);
                DateTime twoMinutesAgo = utcNow.AddMinutes(-2);
                DateTime oneMinuteAgo = utcNow.AddMinutes(-1);

                return new TheoryData<ValidateLifetimeTheoryData>
                {
                    new ValidateLifetimeTheoryData("Valid")
                    {
                        Expires = oneHourFromNow,
                        NotBefore = oneHourAgo,
                        OperationResult = new ValidatedLifetime(oneHourAgo, oneHourFromNow),
                        ValidationParameters = new ValidationParameters(){TimeProvider = timeProvider }
                    },
                    new ValidateLifetimeTheoryData("NotBeforeIsNull")
                    {
                        Expires = oneHourFromNow,
                        NotBefore = null,
                        OperationResult = new ValidatedLifetime(null, oneHourFromNow),
                        ValidationParameters = new ValidationParameters(){ TimeProvider = timeProvider }
                    },
                    new ValidateLifetimeTheoryData("SkewForward")
                    {
                        Expires = oneHourFromNow,
                        NotBefore = twoMinutesFromNow,
                        ValidationParameters = new ValidationParameters {
                            ClockSkew = TimeSpan.FromMinutes(5),
                            TimeProvider = timeProvider
                        },
                        OperationResult = new ValidatedLifetime(twoMinutesFromNow, oneHourFromNow),
                    },
                    new ValidateLifetimeTheoryData("SkewBackward")
                    {
                        Expires = oneMinuteAgo,
                        NotBefore = twoMinutesAgo,
                        ValidationParameters = new ValidationParameters {
                            ClockSkew = TimeSpan.FromMinutes(5),
                            TimeProvider = timeProvider
                        },
                        OperationResult = new ValidatedLifetime(twoMinutesAgo, oneMinuteAgo),
                    },
                };
            }
        }
    }

    public class ValidateLifetimeTheoryData : TheoryDataBase
    {
        public ValidateLifetimeTheoryData(string testId) : base(testId) { }
        public DateTime? NotBefore { get; set; }
        public DateTime? Expires { get; set; }
        public SecurityToken SecurityToken { get; set; }
        internal ValidationParameters ValidationParameters { get; set; }
        internal ValidationResult<ValidatedLifetime, LifetimeValidationError> OperationResult { get; set; }
        internal ValidationFailureType ValidationFailure { get; set; }
    }
}
