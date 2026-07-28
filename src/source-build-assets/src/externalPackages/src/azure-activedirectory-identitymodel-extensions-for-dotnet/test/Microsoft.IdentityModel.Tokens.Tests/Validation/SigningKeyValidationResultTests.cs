// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens.Experimental;
using Xunit;

namespace Microsoft.IdentityModel.Tokens.Validation.Tests
{
    public class SigningKeyTests
    {
        [Theory, MemberData(nameof(InvalidTestCases), DisableDiscoveryEnumeration = true)]
        public void InvalidSigningKeys(SigningKeyValidationTheoryData theoryData)
        {
            CompareContext context = TestUtilities.WriteHeader($"{this}.InvalidSigningKeys", theoryData);

            try
            {
                ValidationResult<ValidatedSignatureKey, ValidationError> validationResult =
                    Validators.ValidateSignatureKey(
                        theoryData.SecurityKey,
                        theoryData.SecurityToken,
                        theoryData.ValidationParameters,
                        theoryData.CallContext);

                if (validationResult.Succeeded)
                {
                    context.AddDiff($"Expected validation to fail, but it succeeded with result: {validationResult.Result}.");
                }
                else
                {
                    ValidationError validationError = validationResult.Error;
                    IdentityComparer.AreStringsEqual(
                        validationError.FailureType.Name,
                        theoryData.ValidationResult.Error.FailureType.Name,
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

        public static TheoryData<SigningKeyValidationTheoryData> InvalidTestCases
        {
            get
            {
                MockTimeProvider timeProvider = new MockTimeProvider();
                DateTime utcNow = timeProvider.GetUtcNow().UtcDateTime;
                DateTime utcExpired = KeyingMaterial.ExpiredX509SecurityKey_Public.Certificate.NotAfter.ToUniversalTime();
                DateTime utcNotYetValid = KeyingMaterial.NotYetValidX509SecurityKey_Public.Certificate.NotBefore.ToUniversalTime();

                return new TheoryData<SigningKeyValidationTheoryData>
                {
                    // TODO Error message IDX10253 message is not accurate.
                    new SigningKeyValidationTheoryData("SecurityKeyIsNull")
                    {
                        ExpectedException = ExpectedException.SecurityTokenInvalidSigningKeyException(substringExpected: "IDX10253:"),
                        SecurityKey = null,
                        SecurityToken = new JwtSecurityToken(),
                        ValidationParameters = new ValidationParameters(){ TimeProvider = timeProvider },
                        ValidationResult = new SignatureKeyValidationError(
                            new MessageDetail(LogMessages.IDX10253),
                            SignatureKeyValidationFailure.KeyIsNull,
                            null,
                            null)
                    },
                    new SigningKeyValidationTheoryData("SecurityTokenIsNull")
                    {
                        ExpectedException = ExpectedException.ArgumentNullException(substringExpected: "IDX10000:"),
                        SecurityKey = KeyingMaterial.SymmetricSecurityKey2_256,
                        SecurityToken = null,
                        ValidationParameters = new ValidationParameters() { TimeProvider = timeProvider },
                        ValidationResult = new SignatureKeyValidationError(
                            new MessageDetail(
                                LogMessages.IDX10000,
                                LogHelper.MarkAsNonPII("securityToken")),
                            ValidationFailureType.NullArgument,
                            null,
                            null)
                    },
                    new SigningKeyValidationTheoryData("ValidationParametersIsNull")
                    {
                        ExpectedException = ExpectedException.ArgumentNullException(substringExpected: "IDX10000:"),
                        SecurityKey = KeyingMaterial.SymmetricSecurityKey2_256,
                        SecurityToken = new JwtSecurityToken(),
                        ValidationParameters = null,
                        ValidationResult = new SignatureKeyValidationError(
                            new MessageDetail(
                                LogMessages.IDX10000,
                                LogHelper.MarkAsNonPII("validationParameters")),
                            ValidationFailureType.NullArgument,
                            null,
                            null), // InvalidSigningKey
                    },
                    new SigningKeyValidationTheoryData("SecurityKeyIsExpired")
                    {
                        ExpectedException = ExpectedException.SecurityTokenInvalidSigningKeyException(substringExpected: "IDX10249:"),
                        SecurityKey = KeyingMaterial.ExpiredX509SecurityKey_Public,
                        SecurityToken = new JwtSecurityToken(),
                        ValidationParameters = new ValidationParameters() { TimeProvider = timeProvider },
                        ValidationResult = new SignatureKeyValidationError(
                            new MessageDetail(
                                LogMessages.IDX10249,
                                LogHelper.MarkAsNonPII(utcExpired),
                                LogHelper.MarkAsNonPII(utcNow)),
                            SignatureKeyValidationFailure.KeyExpired,
                            null,
                            null), // InvalidSigningKey
                    },
                    new SigningKeyValidationTheoryData("SecurityKeyIsNotYetValid")
                    {
                        ExpectedException = ExpectedException.SecurityTokenInvalidSigningKeyException(substringExpected: "IDX10248:"),
                        SecurityKey = KeyingMaterial.NotYetValidX509SecurityKey_Public,
                        SecurityToken = new JwtSecurityToken(),
                        ValidationParameters = new ValidationParameters() { TimeProvider = timeProvider },
                        ValidationResult = new SignatureKeyValidationError(
                            new MessageDetail(
                                LogMessages.IDX10248,
                                LogHelper.MarkAsNonPII(utcNotYetValid),
                                LogHelper.MarkAsNonPII(utcNow)),
                            SignatureKeyValidationFailure.NotYetValid,
                            null,
                            null), // InvalidSigningKey
                    }
                };
            }
        }

        [Theory, MemberData(nameof(ValidTestCases), DisableDiscoveryEnumeration = true)]
        public void ValidSigningKeys(SigningKeyValidationTheoryData theoryData)
        {
            CompareContext context = TestUtilities.WriteHeader($"{this}.ValidSigningKeys", theoryData);

            try
            {
                ValidationResult<ValidatedSignatureKey, ValidationError> validationResult =
                    Validators.ValidateSignatureKey(
                        theoryData.SecurityKey,
                        theoryData.SecurityToken,
                        theoryData.ValidationParameters,
                        theoryData.CallContext);

                if (validationResult.Succeeded)
                {
                    IdentityComparer.AreValidatedSigningKeyLifetimesEqual(
                        theoryData.ValidationResult.Result,
                        validationResult.Result,
                        context);
                }
                else
                {
                    context.AddDiff($"Expected validation to succeed, but it failed with error: {validationResult.Error}.");
                }
            }
            catch (Exception ex)
            {
                context.AddDiff($"Did not expect an exception: {ex}.");
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SigningKeyValidationTheoryData> ValidTestCases
        {
            get
            {
                MockTimeProvider timeProvider = new MockTimeProvider();
                DateTime utcNow = timeProvider.GetUtcNow().UtcDateTime;

                return new TheoryData<SigningKeyValidationTheoryData>
                {
                    new SigningKeyValidationTheoryData("SecurityTokenIsPresent")
                    {
                        SecurityKey = KeyingMaterial.SymmetricSecurityKey2_256,
                        SecurityToken = new JwtSecurityToken(),
                        ValidationParameters = new ValidationParameters(){ TimeProvider = timeProvider },
                        ValidationResult = new ValidatedSignatureKey(null, null, utcNow)
                    }
                };
            }
        }
    }

    public class SigningKeyValidationTheoryData : TheoryDataBase
    {
        public SigningKeyValidationTheoryData(string testId) : base(testId) { }
        public SecurityKey SecurityKey { get; set; }
        public SecurityToken SecurityToken { get; set; }
        internal ValidationParameters ValidationParameters { get; set; }
        public BaseConfiguration BaseConfiguration { get; set; }
        internal ValidationResult<ValidatedSignatureKey, SignatureKeyValidationError> ValidationResult { get; set; }
    }
}
