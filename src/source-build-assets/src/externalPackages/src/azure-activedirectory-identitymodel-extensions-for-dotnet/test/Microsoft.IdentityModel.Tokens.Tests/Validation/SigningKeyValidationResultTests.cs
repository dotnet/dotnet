// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

namespace Microsoft.IdentityModel.Tokens.Validation.Tests
{
    public class SigningKeyValidationResultTests
    {
        [Theory, MemberData(nameof(SigningKeyValidationTestCases), DisableDiscoveryEnumeration = true)]
        public void SecurityKey(SigningKeyValidationTheoryData theoryData)
        {
            CompareContext context = TestUtilities.WriteHeader($"{this}.SigningKeyValidationResultTests", theoryData);

            SigningKeyValidationResult signingKeyValidationResult = Validators.ValidateIssuerSecurityKey(
                theoryData.SecurityKey,
                theoryData.SecurityToken,
                theoryData.ValidationParameters,
                theoryData.BaseConfiguration,
                new CallContext());

            if (signingKeyValidationResult.Exception != null)
                theoryData.ExpectedException.ProcessException(signingKeyValidationResult.Exception);
            else
                theoryData.ExpectedException.ProcessNoException();

            IdentityComparer.AreSigningKeyValidationResultsEqual(
                signingKeyValidationResult,
                theoryData.SigningKeyValidationResult,
                context);

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SigningKeyValidationTheoryData> SigningKeyValidationTestCases
        {
            get
            {
                DateTime utcNow = DateTime.UtcNow;
                DateTime utcExpired = KeyingMaterial.ExpiredX509SecurityKey_Public.Certificate.NotAfter.ToUniversalTime();
                DateTime utcNotYetValid = KeyingMaterial.NotYetValidX509SecurityKey_Public.Certificate.NotBefore.ToUniversalTime();

                return new TheoryData<SigningKeyValidationTheoryData>
                {
                    new SigningKeyValidationTheoryData
                    {
                        TestId = "Valid_SecurityTokenIsPresent_ValidateIssuerSigningKeyIsTrue",
                        ExpectedException = ExpectedException.NoExceptionExpected,
                        SecurityKey = KeyingMaterial.SymmetricSecurityKey2_256,
                        SecurityToken = new JwtSecurityToken(),
                        ValidationParameters = new TokenValidationParameters { ValidateIssuerSigningKey = true },
                        SigningKeyValidationResult = new SigningKeyValidationResult(KeyingMaterial.SymmetricSecurityKey2_256)
                    },
                    new SigningKeyValidationTheoryData
                    {
                        TestId = "Valid_SecurityKeyIsNull_ValidateIssuerSigningKeyIsFalse",
                        ExpectedException = ExpectedException.NoExceptionExpected,
                        SecurityKey = null,
                        SecurityToken = new JwtSecurityToken(),
                        ValidationParameters = new TokenValidationParameters { ValidateIssuerSigningKey = false },
                        SigningKeyValidationResult = new SigningKeyValidationResult(null)
                    },
                    new SigningKeyValidationTheoryData
                    {
                        TestId = "Valid_SecurityTokenIsNull_ValidateIssuerSigningKeyIsFalse",
                        ExpectedException = ExpectedException.NoExceptionExpected,
                        SecurityKey = KeyingMaterial.SymmetricSecurityKey2_256,
                        SecurityToken = null,
                        ValidationParameters = new TokenValidationParameters { ValidateIssuerSigningKey = false },
                        SigningKeyValidationResult = new SigningKeyValidationResult(KeyingMaterial.SymmetricSecurityKey2_256)
                    },
                    new SigningKeyValidationTheoryData
                    {
                        TestId = "Valid_SecurityKeyIsNull_RequireSignedTokensIsFalse",
                        ExpectedException = ExpectedException.NoExceptionExpected,
                        SecurityKey = null,
                        SecurityToken = new JwtSecurityToken(),
                        ValidationParameters = new TokenValidationParameters { ValidateIssuerSigningKey = true, RequireSignedTokens = false },
                        SigningKeyValidationResult = new SigningKeyValidationResult(null)
                    },
                    new SigningKeyValidationTheoryData
                    {
                        TestId = "Valid_SecurityKeyIsPresent_RequireSignedTokensIsTrue",
                        ExpectedException = ExpectedException.NoExceptionExpected,
                        SecurityKey = KeyingMaterial.SymmetricSecurityKey2_256,
                        SecurityToken = new JwtSecurityToken(),
                        ValidationParameters = new TokenValidationParameters { ValidateIssuerSigningKey = true, RequireSignedTokens = true },
                        SigningKeyValidationResult = new SigningKeyValidationResult(KeyingMaterial.SymmetricSecurityKey2_256)
                    },
                    new SigningKeyValidationTheoryData
                    {
                        TestId = "Valid_SecurityKeyIsPresent_RequireSignedTokensIsFalse",
                        ExpectedException = ExpectedException.NoExceptionExpected,
                        SecurityKey = KeyingMaterial.SymmetricSecurityKey2_256,
                        SecurityToken = new JwtSecurityToken(),
                        ValidationParameters = new TokenValidationParameters { ValidateIssuerSigningKey = true, RequireSignedTokens = false },
                        SigningKeyValidationResult = new SigningKeyValidationResult(KeyingMaterial.SymmetricSecurityKey2_256)
                    },
                    new SigningKeyValidationTheoryData
                    {
                        TestId = "Valid_DelegateSet_ReturnsTrue",
                        ExpectedException = ExpectedException.NoExceptionExpected,
                        SecurityKey = KeyingMaterial.SymmetricSecurityKey2_256,
                        SecurityToken = new JwtSecurityToken(),
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKeyValidator = (SecurityKey securityKey, SecurityToken token, TokenValidationParameters validationParameters) => true
                        },
                        SigningKeyValidationResult = new SigningKeyValidationResult(KeyingMaterial.SymmetricSecurityKey2_256)
                    },
                    new SigningKeyValidationTheoryData
                    {
                        TestId = "Valid_DelegateUsingConfigurationSet_ReturnsTrue",
                        ExpectedException = ExpectedException.NoExceptionExpected,
                        SecurityKey = KeyingMaterial.SymmetricSecurityKey2_256,
                        SecurityToken = new JwtSecurityToken(),
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKeyValidatorUsingConfiguration = (SecurityKey securityKey, SecurityToken token, TokenValidationParameters validationParameters, BaseConfiguration configuration) => true
                        },
                        BaseConfiguration = new OpenIdConnectConfiguration(),
                        SigningKeyValidationResult = new SigningKeyValidationResult(KeyingMaterial.SymmetricSecurityKey2_256)
                    },
                    new SigningKeyValidationTheoryData
                    {
                        TestId = "Invalid_SecurityKeyIsNull",
                        ExpectedException = ExpectedException.ArgumentNullException(substringExpected: "IDX10253:"),
                        SecurityKey = null,
                        SecurityToken = new JwtSecurityToken(),
                        ValidationParameters = new TokenValidationParameters { ValidateIssuerSigningKey = true },
                        SigningKeyValidationResult = new SigningKeyValidationResult(
                            null, // SecurityKey
                            ValidationFailureType.NullArgument,
                            new ExceptionDetail(
                                new MessageDetail(LogMessages.IDX10253),
                                typeof(ArgumentNullException),
                                new StackFrame(true)))
                    },
                    new SigningKeyValidationTheoryData
                    {
                        TestId = "Invalid_SecurityTokenIsNullAndValidateIssuerSigningKeyTrue",
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        SecurityKey = KeyingMaterial.SymmetricSecurityKey2_256,
                        SecurityToken = null,
                        ValidationParameters = new TokenValidationParameters { ValidateIssuerSigningKey = true },
                        SigningKeyValidationResult = new SigningKeyValidationResult(
                            KeyingMaterial.SymmetricSecurityKey2_256,
                            ValidationFailureType.NullArgument,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10000,
                                    LogHelper.MarkAsNonPII("securityToken")),
                                typeof(ArgumentNullException),
                                new StackFrame(true)))
                    },
                    new SigningKeyValidationTheoryData
                    {
                        TestId = "Invalid_ValidationParametersIsNull",
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        SecurityKey = KeyingMaterial.SymmetricSecurityKey2_256,
                        SecurityToken = new JwtSecurityToken(),
                        ValidationParameters = null,
                        SigningKeyValidationResult = new SigningKeyValidationResult(
                            KeyingMaterial.SymmetricSecurityKey2_256,
                            ValidationFailureType.NullArgument,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10000,
                                    LogHelper.MarkAsNonPII("validationParameters")),
                                typeof(ArgumentNullException),
                                new StackFrame(true)))
                    },
                    new SigningKeyValidationTheoryData
                    {
                        TestId = "Invalid_SecurityKeyIsExpired",
                        ExpectedException = ExpectedException.SecurityTokenInvalidSigningKeyException(substringExpected: "IDX10249:"),
                        SecurityKey = KeyingMaterial.ExpiredX509SecurityKey_Public,
                        SecurityToken = new JwtSecurityToken(),
                        ValidationParameters = new TokenValidationParameters { ValidateIssuerSigningKey = true },
                        SigningKeyValidationResult = new SigningKeyValidationResult(
                            KeyingMaterial.ExpiredX509SecurityKey_Public,
                            ValidationFailureType.SigningKeyValidationFailed,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10249,
                                    LogHelper.MarkAsNonPII(utcExpired),
                                    LogHelper.MarkAsNonPII(utcNow)),
                                typeof(SecurityTokenInvalidSigningKeyException),
                                new StackFrame(true)))
                    },
                    new SigningKeyValidationTheoryData
                    {
                        TestId = "Invalid_SecurityKeyIsNotYetValid",
                        ExpectedException = ExpectedException.SecurityTokenInvalidSigningKeyException(substringExpected: "IDX10248:"),
                        SecurityKey = KeyingMaterial.NotYetValidX509SecurityKey_Public,
                        SecurityToken = new JwtSecurityToken(),
                        ValidationParameters = new TokenValidationParameters { ValidateIssuerSigningKey = true },
                        SigningKeyValidationResult = new SigningKeyValidationResult(
                            KeyingMaterial.NotYetValidX509SecurityKey_Public,
                            ValidationFailureType.SigningKeyValidationFailed,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10248,
                                    LogHelper.MarkAsNonPII(utcNotYetValid),
                                    LogHelper.MarkAsNonPII(utcNow)),
                                typeof(SecurityTokenInvalidSigningKeyException),
                                new StackFrame(true)))
                    },
                    new SigningKeyValidationTheoryData
                    {
                        TestId = "Invalid_SecurityKeyIsNull_RequireSignedTokensIsTrue",
                        ExpectedException = ExpectedException.ArgumentNullException(substringExpected: "IDX10253:"),
                        SecurityKey = null,
                        SecurityToken = new JwtSecurityToken(),
                        ValidationParameters = new TokenValidationParameters { ValidateIssuerSigningKey = true, RequireSignedTokens = true },
                        SigningKeyValidationResult = new SigningKeyValidationResult(
                            null,
                            ValidationFailureType.NullArgument,
                            new ExceptionDetail(
                                new MessageDetail(LogMessages.IDX10253),
                                typeof(ArgumentNullException),
                                new StackFrame(true)))
                    },
                    new SigningKeyValidationTheoryData
                    {
                        TestId = "Invalid_DelegateIsSet_ReturnsFalse",
                        ExpectedException = ExpectedException.SecurityTokenInvalidSigningKeyException(substringExpected: "IDX10232:"),
                        SecurityKey = KeyingMaterial.SymmetricSecurityKey2_256,
                        SecurityToken = new JwtSecurityToken(),
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKeyValidator = (SecurityKey securityKey, SecurityToken token, TokenValidationParameters validationParameters) => false
                        },
                        SigningKeyValidationResult = new SigningKeyValidationResult(
                            KeyingMaterial.SymmetricSecurityKey2_256,
                            ValidationFailureType.SigningKeyValidationFailed,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10232,
                                    KeyingMaterial.SymmetricSecurityKey2_256),
                                typeof(SecurityTokenInvalidSigningKeyException),
                                new StackFrame(true)))
                    },
                    new SigningKeyValidationTheoryData
                    {
                        TestId = "Invalid_DelegateUsingConfigurationSet_ReturnsFalse",
                        ExpectedException = ExpectedException.SecurityTokenInvalidSigningKeyException(substringExpected: "IDX10232:"),
                        SecurityKey = KeyingMaterial.SymmetricSecurityKey2_256,
                        SecurityToken = new JwtSecurityToken(),
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKeyValidatorUsingConfiguration = (SecurityKey securityKey, SecurityToken token, TokenValidationParameters validationParameters, BaseConfiguration configuration) => false
                        },
                        BaseConfiguration = new OpenIdConnectConfiguration(),
                        SigningKeyValidationResult = new SigningKeyValidationResult(
                            KeyingMaterial.SymmetricSecurityKey2_256,
                            ValidationFailureType.SigningKeyValidationFailed,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10232,
                                    KeyingMaterial.SymmetricSecurityKey2_256),
                                typeof(SecurityTokenInvalidSigningKeyException),
                                new StackFrame(true)))
                    },
                    new SigningKeyValidationTheoryData
                    {
                        TestId = "Invalid_DelegateIsSet_Throws",
                        ExpectedException = ExpectedException.SecurityTokenInvalidSigningKeyException(substringExpected: "IDX10232:", innerTypeExpected: typeof(SecurityTokenInvalidSigningKeyException)),
                        SecurityKey = KeyingMaterial.SymmetricSecurityKey2_256,
                        SecurityToken = new JwtSecurityToken(),
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKeyValidator = (SecurityKey securityKey, SecurityToken token, TokenValidationParameters validationParameters) => throw new SecurityTokenInvalidSigningKeyException()
                        },
                        SigningKeyValidationResult = new SigningKeyValidationResult(
                            KeyingMaterial.SymmetricSecurityKey2_256,
                            ValidationFailureType.SigningKeyValidationFailed,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10232,
                                    KeyingMaterial.SymmetricSecurityKey2_256),
                                typeof(SecurityTokenInvalidSigningKeyException),
                                new StackFrame(true),
                                new SecurityTokenInvalidSigningKeyException()))
                    },
                    new SigningKeyValidationTheoryData
                    {
                        TestId = "Invalid_DelegateUsingConfigurationSet_Throws",
                        ExpectedException = ExpectedException.SecurityTokenInvalidSigningKeyException(substringExpected: "IDX10232:", innerTypeExpected: typeof(SecurityTokenInvalidSigningKeyException)),
                        SecurityKey = KeyingMaterial.SymmetricSecurityKey2_256,
                        SecurityToken = new JwtSecurityToken(),
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKeyValidatorUsingConfiguration = (SecurityKey securityKey, SecurityToken token, TokenValidationParameters validationParameters, BaseConfiguration configuration) => throw new SecurityTokenInvalidSigningKeyException()
                        },
                        BaseConfiguration = new OpenIdConnectConfiguration(),
                        SigningKeyValidationResult = new SigningKeyValidationResult(
                            KeyingMaterial.SymmetricSecurityKey2_256,
                            ValidationFailureType.SigningKeyValidationFailed,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10232,
                                    KeyingMaterial.SymmetricSecurityKey2_256),
                                typeof(SecurityTokenInvalidSigningKeyException),
                                new StackFrame(true),
                                new SecurityTokenInvalidSigningKeyException()))
                        },
                    };
                }
            }
        }

        public class SigningKeyValidationTheoryData: TheoryDataBase
        {
            public SecurityKey SecurityKey { get; set; }
            public SecurityToken SecurityToken { get; set; }
            public TokenValidationParameters ValidationParameters { get; set; }
            public BaseConfiguration BaseConfiguration { get; set; }
            internal SigningKeyValidationResult SigningKeyValidationResult { get; set; }
        }
    }
