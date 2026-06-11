// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens.Json.Tests;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Logging;
using Xunit;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens.Experimental;

namespace Microsoft.IdentityModel.Tokens.Validation.Tests
{
    public class TokenTypeTests
    {
        [Theory, MemberData(nameof(InvalidTestCases), DisableDiscoveryEnumeration = true)]
        public void InvalidTokenTypes(TokenTypeTheoryData theoryData)
        {
            CompareContext context = TestUtilities.WriteHeader($"{this}.InvalidTokenTypes", theoryData);

            if (theoryData.TokenTypesToAdd != null)
            {
                foreach (string tokenType in theoryData.TokenTypesToAdd)
                    theoryData.ValidationParameters.ValidTypes.Add(tokenType);
            }

            try
            {
                ValidationResult<ValidatedTokenType, ValidationError> validationResult =
                    Validators.ValidateTokenType(
                        theoryData.Type,
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

                    Exception exception = validationError.GetException();
                    theoryData.ExpectedException.ProcessException(exception, context);
                }
            }
            catch (Exception ex)
            {
                context.AddDiff($"Did not expect an exception: {ex}");
            }

            TestUtilities.AssertFailIfErrors(context);

        }

        public static TheoryData<TokenTypeTheoryData> InvalidTestCases
        {
            get
            {
                string[] validTypesNoJwt = { "ID Token", "Refresh Token", "Access Token" };

                return new TheoryData<TokenTypeTheoryData>
                {
                    new TokenTypeTheoryData("SecurityTokenIsNull")
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                        Type = "JWT",
                        SecurityToken = null,
                        ValidationParameters = null,
                        ValidationResult = new TokenTypeValidationError(
                            new MessageDetail(
                                LogMessages.IDX10000,
                                LogHelper.MarkAsNonPII("securityToken")),
                            ValidationFailureType.NullArgument,
                            null,
                            "JWT")
                    },
                    new TokenTypeTheoryData("ValidationParametersAreNull")
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                        Type = "JWT",
                        SecurityToken = JsonUtilities.CreateUnsignedJsonWebToken(JwtRegisteredClaimNames.Typ, "JWT"),
                        ValidationParameters = null,
                        ValidationResult = new TokenTypeValidationError(
                            new MessageDetail(
                                LogMessages.IDX10000,
                                LogHelper.MarkAsNonPII("validationParameters")),
                            ValidationFailureType.NullArgument,
                            null,
                            "JWT")
                    },
                    new TokenTypeTheoryData("TokenTypeIsEmpty")
                    {
                        ExpectedException = ExpectedException.SecurityTokenInvalidTypeException("IDX10256:"),
                        Type = string.Empty,
                        SecurityToken = JsonUtilities.CreateUnsignedJsonWebToken(JwtRegisteredClaimNames.Typ, string.Empty),
                        ValidationParameters = new ValidationParameters(),
                        TokenTypesToAdd = validTypesNoJwt,
                        ValidationResult = new TokenTypeValidationError(
                            new MessageDetail(
                                LogMessages.IDX10256,
                                LogHelper.MarkAsNonPII("type")),
                            TokenTypeValidationFailure.ValidationFailed,
                            null,
                            "")
                    },
                    new TokenTypeTheoryData("TokenTypeIsNull")
                    {
                        ExpectedException = ExpectedException.SecurityTokenInvalidTypeException("IDX10256:"),
                        Type = null,
                        SecurityToken = JsonUtilities.CreateUnsignedJsonWebToken(JwtRegisteredClaimNames.Typ, null),
                        ValidationParameters = new ValidationParameters(),
                        TokenTypesToAdd = validTypesNoJwt,
                        ValidationResult = new TokenTypeValidationError(
                            new MessageDetail(
                                LogMessages.IDX10256,
                                LogHelper.MarkAsNonPII("type")),
                            TokenTypeValidationFailure.ValidationFailed,
                            null,
                            null)
                    },
                    new TokenTypeTheoryData("ValidationParametersValidTypesDoesNotSupportType")
                    {
                        ExpectedException = ExpectedException.SecurityTokenInvalidTypeException("IDX10257:"),
                        Type = "JWT",
                        SecurityToken = JsonUtilities.CreateUnsignedJsonWebToken(JwtRegisteredClaimNames.Typ, "JWT"),
                        ValidationParameters = new ValidationParameters(),
                        TokenTypesToAdd = validTypesNoJwt,
                        ValidationResult = new TokenTypeValidationError(
                            new MessageDetail(
                                LogMessages.IDX10257,
                                LogHelper.MarkAsNonPII("type"),
                                LogHelper.MarkAsNonPII(Utility.SerializeAsSingleCommaDelimitedString(validTypesNoJwt))),
                            TokenTypeValidationFailure.ValidationFailed,
                            null,
                            "JWT")
                    }
                };
            }
        }

        [Theory, MemberData(nameof(ValidTestCases), DisableDiscoveryEnumeration = true)]
        public void ValidTokenTypes(TokenTypeTheoryData theoryData)
        {
            CompareContext context = TestUtilities.WriteHeader($"{this}.ValidTokenTypes", theoryData);

            if (theoryData.TokenTypesToAdd != null)
            {
                foreach (string tokenType in theoryData.TokenTypesToAdd)
                    theoryData.ValidationParameters.ValidTypes.Add(tokenType);
            }

            try
            {
                ValidationResult<ValidatedTokenType, ValidationError> validationResult =
                    Validators.ValidateTokenType(
                        theoryData.Type,
                        theoryData.SecurityToken,
                        theoryData.ValidationParameters,
                        theoryData.CallContext);

                if (validationResult.Succeeded)
                {
                    IdentityComparer.AreValidatedTokenTypesEqual(
                        validationResult.Result,
                        theoryData.ValidationResult.Result,
                        context);
                }
                else
                {
                    context.AddDiff($"Expected validation to succeed, but it failed with error: {validationResult.Error}.");
                }
            }
            catch (Exception ex)
            {
                context.AddDiff($"Did not expect an exception: {ex}");
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<TokenTypeTheoryData> ValidTestCases
        {
            get
            {
                string[] tokenTypes = { "ID Token", "Refresh Token", "Access Token", "JWT" };

                return new TheoryData<TokenTypeTheoryData>
                {
                    new TokenTypeTheoryData("DefaultTokenTypeValidation")
                    {
                        Type = "JWT",
                        SecurityToken = JsonUtilities.CreateUnsignedJsonWebToken(JwtRegisteredClaimNames.Typ, "JWT"),
                        ValidationParameters = new ValidationParameters(),
                        TokenTypesToAdd = tokenTypes,
                        ValidationResult = new ValidatedTokenType("JWT", 4)
                    }
                };
            }
        }

        public class TokenTypeTheoryData : TheoryDataBase
        {
            public TokenTypeTheoryData(string testId) : base(testId) { }
            public string Type { get; set; }
            public SecurityToken SecurityToken { get; set; }
            public IList<string> TokenTypesToAdd { get; internal set; }
            internal ValidationParameters ValidationParameters { get; set; }
            internal ValidationResult<ValidatedTokenType, TokenTypeValidationError> ValidationResult { get; set; }
        }
    }
}
