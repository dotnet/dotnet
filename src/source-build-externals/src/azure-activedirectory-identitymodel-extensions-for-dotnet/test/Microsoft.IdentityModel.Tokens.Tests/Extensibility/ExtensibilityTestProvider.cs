// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens.Experimental;
using Xunit;

#nullable enable
namespace Microsoft.IdentityModel.Tokens.Extensibility.Tests
{
    /// <summary>
    /// These extensibility tests are designed to show consistency across different token handlers for the different validation steps.
    /// Coverage is provided for the following:
    /// 1. The delegates throwing
    /// 2. The delegates return a xxxValidationError, with a ValidationFailureType that is known to the xxxValidationError and one that is not known.
    /// 3. The delegates return a CustomxxxValidationError, with a ValidationFailureType that is known to the CustomxxxValidationError and one that is not known.
    /// 4. The delegates sets a ValidationFailureType that is NOT known to the xxxValidationError or the CustomxxxValidationError.
    /// </summary>
    public class ExtensibilityTestProvider
    {
        #region Algorithm
        public static TheoryData<ExtensibilityTheoryData> GenerateInvalidAlgorithmTestCases(
            TokenHandler tokenHandler,
            SecurityToken securityToken,
            string algorithm = "algorithm")
        {
            TheoryData<ExtensibilityTheoryData> theoryData = new();
            CallContext callContext = new CallContext();

            #region CustomAlgorithmValidationError
            // Delegate sets ValidationFailureType to a value that is known to CustomAlgorithmValidationError
            // ValidationError: CustomAlgorithmValidationError
            // FailureType: CustomValidationFailure.ValidationFailed
            // Exception: CustomSecurityTokenInvalidAlgorithmException
            theoryData.Add(new ExtensibilityTheoryData(
                "CustomAlgorithmValidationFailed",
                tokenHandler,
                securityToken,
                CustomAlgorithmValidationDelegates.CustomAlgorithmValidationFailed)
            {
                ExpectedException = new ExpectedException(
                    typeof(CustomSecurityTokenInvalidAlgorithmException),
                    nameof(CustomAlgorithmValidationDelegates.CustomAlgorithmValidationFailed)),
                ValidationError = new CustomAlgorithmValidationError(
                    new MessageDetail(nameof(CustomAlgorithmValidationDelegates.CustomAlgorithmValidationFailed)),
                    CustomValidationFailure.AlgorithmValidationFailed,
                    Default.GetStackFrame(),
                    $"{algorithm}")
            });

            // Delegate sets ValidationFailureType to a value that is known to AlgorithmValidationError
            // ValidationError: AlgorithmValidationError
            // FailureType: AlgorithmValidationFailure.ValidationFailed
            // Exception: SecurityTokenInvalidAlgorithmException
            theoryData.Add(new ExtensibilityTheoryData(
                "AlgorithmValidationFailed",
                tokenHandler,
                securityToken,
                CustomAlgorithmValidationDelegates.AlgorithmValidationFailed)
            {
                ExpectedException = ExpectedException.SecurityTokenInvalidAlgorithmException(
                    nameof(CustomAlgorithmValidationDelegates.AlgorithmValidationFailed)),
                ValidationError = new CustomAlgorithmValidationError(
                    new MessageDetail(nameof(CustomAlgorithmValidationDelegates.AlgorithmValidationFailed)),
                    AlgorithmValidationFailure.ValidationFailed,
                    Default.GetStackFrame(),
                    $"{algorithm}"),
            });

            // Delegate sets ValidationFailureType to a value that is NOT known to AlgorithmValidationError or CustomAlgorithnmValidationError
            // ValidationError: CustomAlgorithmValidationError
            // FailureType: AudienceValidationFailure.AudienceDidNotMatch
            // Exception: SecurityTokenValidationException
            theoryData.Add(new ExtensibilityTheoryData(
                "AlgorithmUnknownValidationFailure",
                tokenHandler,
                securityToken,
                CustomAlgorithmValidationDelegates.UnknownValidationFailure)
            {
                ExpectedException = ExpectedException.SecurityTokenValidationException(
                    nameof(CustomAlgorithmValidationDelegates.UnknownValidationFailure)),
                ValidationError = new CustomAlgorithmValidationError(
                    new MessageDetail(nameof(CustomAlgorithmValidationDelegates.UnknownValidationFailure)),
                    AudienceValidationFailure.AudienceDidNotMatch,
                    Default.GetStackFrame(),
                    $"{algorithm}"),
            });
            #endregion

            #region AlgorithmValidationError
            // Delegate returns AlgorithmValidationError and sets ValidationFailureType to a value that is known to AlgorithmValidationError
            // ValidationError: AlgorithmValidationError
            // FailureType: AlgorithmValidationFailure.ValidationFailed
            // Exception:  SecurityTokenInvalidAlgorithmException
            theoryData.Add(new ExtensibilityTheoryData(
                "AlgorithmValidatorDelegate",
                tokenHandler,
                securityToken,
                CustomAlgorithmValidationDelegates.AlgorithmValidatorDelegate)
            {
                ExpectedException = ExpectedException.SecurityTokenInvalidAlgorithmException(
                    nameof(CustomAlgorithmValidationDelegates.AlgorithmValidatorDelegate)),
                ValidationError = new AlgorithmValidationError(
                    new MessageDetail(nameof(CustomAlgorithmValidationDelegates.AlgorithmValidatorDelegate)),
                    AlgorithmValidationFailure.ValidationFailed,
                    Default.GetStackFrame(),
                    $"{algorithm}")
            });

            // Delegate throws CustomSecurityTokenAlgorithmException
            // ValidationError: AlgorithmValidationError
            // Exception: SecurityInvalidAlgorithmException
            // FailureType: AlgorithmValidationFailure.ValidatorThrew
            // InnerException: CustomSecurityTokenInvalidAlgorithmException
            ExtensibilityTheoryData testCase = new ExtensibilityTheoryData(
                "AlgorithmValidatorThrows",
                tokenHandler,
                securityToken,
                CustomAlgorithmValidationDelegates.AlgorithmValidatorThrows)
            {
                ExpectedException = ExpectedException.SecurityTokenInvalidAlgorithmException(
                    LogMessages.IDX10273,
                    typeof(CustomSecurityTokenInvalidAlgorithmException)),
                ValidationError = new AlgorithmValidationError(
                    new MessageDetail(LogMessages.IDX10273),
                    AlgorithmValidationFailure.ValidatorThrew,
                    Default.GetStackFrame(),
                    $"{algorithm}",
                    new CustomSecurityTokenInvalidAlgorithmException(nameof(CustomAlgorithmValidationDelegates.AlgorithmValidatorThrows), null)
                )
            };

            // throwing makes the StackFramesCount and StackFrames properties different, so we ignore them when comparing
            testCase.PropertiesToIgnoreWhenComparing[typeof(AlgorithmValidationError)] = new List<string> { "StackFramesCount", "StackFrames" };
            theoryData.Add(testCase);
            #endregion

            return theoryData;
        }
        #endregion

        #region Audience
        public static TheoryData<ExtensibilityTheoryData> GenerateAudienceTestCases(
            TokenHandler tokenHandler,
            SecurityToken securityToken)
        {
            var theoryData = new TheoryData<ExtensibilityTheoryData>();
            var audience = Default.Audience;
            List<string> tokenAudiences = [audience];

            #region CustomAudienceValidationError
            // Delegate sets ValidationFailureType to a value that is known to CustomAudienceValidationError
            // ValidationError: CustomAudienceValidationError
            // FailureType: CustomValidationFailure.AudienceValidationFailed
            // Exception: CustomSecurityTokenInvalidAudienceException
            theoryData.Add(new ExtensibilityTheoryData(
                "CustomAudiencdValidationFailed",
                tokenHandler,
                securityToken,
                CustomAudienceValidationDelegates.CustomValidationFailed)
            {
                ExpectedException = new ExpectedException(
                    typeof(CustomSecurityTokenInvalidAudienceException),
                    nameof(CustomAudienceValidationDelegates.CustomValidationFailed)),
                ValidationError = new CustomAudienceValidationError(
                    new MessageDetail(nameof(CustomAudienceValidationDelegates.CustomValidationFailed)),
                    CustomValidationFailure.AudienceValidationFailed,
                    Default.GetStackFrame(),
                    tokenAudiences,
                    null)
            });

            // Delegate sets ValidationFailureType to a value that is known to AudienceValidationError
            // ValidationError: CustomAudienceValidationError
            // FailureType: AudienceValidationFailure.AudienceDidNotMatch
            // Exception: SecurityTokenInvalidAudienceException
            theoryData.Add(new ExtensibilityTheoryData(
                "AudienceDidNotMatch",
                tokenHandler,
                securityToken,
                CustomAudienceValidationDelegates.AudienceDidNotMatch)
            {
                ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException(
                    nameof(CustomAudienceValidationDelegates.AudienceDidNotMatch)),
                ValidationError = new CustomAudienceValidationError(
                    new MessageDetail(nameof(CustomAudienceValidationDelegates.AudienceDidNotMatch)),
                    AudienceValidationFailure.AudienceDidNotMatch,
                    Default.GetStackFrame(),
                    tokenAudiences,
                    null),
            });

            // Delegate sets ValidationFailureType to a value that is NOT known to AudienceValidationError or CustomAudienceValidationError
            // ValidationError: CustomAudienceValidationError
            // FailureType: ValidationFailureType.AlgorithmIsNotSupported
            // Exception: SecurityTokenValidationException
            theoryData.Add(new ExtensibilityTheoryData(
                "AudienceUnknownAudienceValidationFailure",
                tokenHandler,
                securityToken,
                CustomAudienceValidationDelegates.UnknownValidationFailure)
            {
                ExpectedException = ExpectedException.SecurityTokenValidationException(
                    nameof(CustomAudienceValidationDelegates.UnknownValidationFailure)),
                ValidationError = new CustomAudienceValidationError(
                    new MessageDetail(nameof(CustomAudienceValidationDelegates.UnknownValidationFailure)),
                    AlgorithmValidationFailure.AlgorithmIsNotSupported,
                    Default.GetStackFrame(),
                    tokenAudiences,
                    null),
            });
            #endregion

            #region AudienceValidationError
            // Delegate returns AudienceValidationError and sets ValidationFailureType to a value that is known to AudienceValidationError
            // ValidationError: AudienceValidationError
            // FailureType: AudienceValidationFailure.AudienceDidNotMatch
            // Exception:  SecurityTokenInvalidAudienceException
            theoryData.Add(new ExtensibilityTheoryData(
                "AudienceDidNotMatch",
                tokenHandler,
                securityToken,
                CustomAudienceValidationDelegates.AudienceValidatorDelegate)
            {
                ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException(
                    nameof(CustomAudienceValidationDelegates.AudienceValidatorDelegate)),
                ValidationError = new AudienceValidationError(
                    new MessageDetail(nameof(CustomAudienceValidationDelegates.AudienceValidatorDelegate)),
                    AudienceValidationFailure.AudienceDidNotMatch,
                    Default.GetStackFrame(),
                    tokenAudiences,
                    null)
            });

            // Delegate throws CustomSecurityTokenInvalidAudienceException
            // ValidationError: AudienceValidationError
            // Exception: SecurityTokenInvalidAudienceException
            // FailureType: AudienceValidationFailure.ValidatorThrew
            // InnerException: CustomSecurityTokenInvalidAudienceException
            ExtensibilityTheoryData testCase = new ExtensibilityTheoryData(
                "AudienceValidatorThrows",
                tokenHandler,
                securityToken,
                CustomAudienceValidationDelegates.AudienceValidatorThrows)
            {
                ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException(
                    LogMessages.IDX10270,
                    typeof(CustomSecurityTokenInvalidAudienceException)),
                ValidationError = new AudienceValidationError(
                    new MessageDetail(LogMessages.IDX10270),
                    AudienceValidationFailure.ValidatorThrew,
                    Default.GetStackFrame(),
                    tokenAudiences,
                    null,
                    new SecurityTokenInvalidAudienceException(nameof(CustomAudienceValidationDelegates.AudienceValidatorThrows))
                )
            };

            // throwing makes the StackFramesCount and StackFrames properties different, so we ignore them when comparing
            testCase.PropertiesToIgnoreWhenComparing[typeof(AudienceValidationError)] = new List<string> { "StackFramesCount", "StackFrames" };
            theoryData.Add(testCase);
            #endregion

            return theoryData;
        }
        #endregion

        #region Issuer
        public static TheoryData<ExtensibilityTheoryData> GenerateInvalidIssuerTestCases(
            TokenHandler tokenHandler,
            SecurityToken securityToken)
        {
            var theoryData = new TheoryData<ExtensibilityTheoryData>();
            CallContext callContext = new CallContext();
            string issuerGuid = Guid.NewGuid().ToString();

            #region return CustomIssuerValidationError
            // Delegate sets ValidationFailureType to a value that is known to CustomIssuerValidationError
            // ValidationError: CustomIssuerValidationError
            // FailureType: CustomValidationError.ValidationFailed
            // Exception: CustomSecurityTokenInvalidIssuerException
            theoryData.Add(new ExtensibilityTheoryData(
                "CustomIssuerValidationFailed",
                tokenHandler,
                securityToken,
                CustomIssuerValidationDelegates.CustomValidationFailed)
            {
                ExpectedException = new ExpectedException(
                    typeof(CustomSecurityTokenInvalidIssuerException),
                    nameof(CustomIssuerValidationDelegates.CustomValidationFailed)),
                ValidationError = new CustomIssuerValidationError(
                    new MessageDetail(nameof(CustomIssuerValidationDelegates.CustomValidationFailed)),
                    CustomValidationFailure.IssuerValidationFailed,
                    Default.GetStackFrame(),
                    issuerGuid)
            });

            // Delegate sets ValidationFailureType to a value that is known to IssuerValidationError
            // ValidationError: CustomIssuerValidationError
            // FailureType: IssuerValidationFailure.ValidationFailed
            // Exception: SecurityTokenInvalidIssuerException
            theoryData.Add(new ExtensibilityTheoryData(
                "IssuerValidationFailed",
                tokenHandler,
                securityToken,
                CustomIssuerValidationDelegates.IssuerValidationFailed)
            {
                ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException(
                    nameof(CustomIssuerValidationDelegates.IssuerValidationFailed)),
                ValidationError = new CustomIssuerValidationError(
                    new MessageDetail(nameof(CustomIssuerValidationDelegates.IssuerValidationFailed)),
                    IssuerValidationFailure.ValidationFailed,
                    Default.GetStackFrame(),
                    issuerGuid),
            });

            // Delegate sets ValidationFailureType to a value that is not known to IssuerValidationError or CustomIssuerValidationError
            // ValidationError: CustomIssuerValidationError
            // FailureType: AlgorithmValidationFailure.AlgorithmIsNotSupported
            // Exception: SecurityTokenValidationException
            theoryData.Add(new ExtensibilityTheoryData(
                "IssuerUnknownValidationFailure",
                tokenHandler,
                securityToken,
                CustomIssuerValidationDelegates.UnknownValidationFailure)
            {
                ExpectedException = ExpectedException.SecurityTokenValidationException(
                    nameof(CustomIssuerValidationDelegates.UnknownValidationFailure)),
                ValidationError = new CustomIssuerValidationError(
                    new MessageDetail(nameof(CustomIssuerValidationDelegates.UnknownValidationFailure)),
                    AlgorithmValidationFailure.AlgorithmIsNotSupported,
                    Default.GetStackFrame(),
                    issuerGuid),
            });
            #endregion

            #region return IssuerValidationError
            // Delegate returns IssuerValidationError and sets ValidationFailureType to a value that is known to IssuerValidationError
            // ValidationError: IssuerValidationError
            // FailureType: IssuerValidationFailure.ValidationFailed
            // Exception:  SecurityTokenInvalidIssuerException
            theoryData.Add(new ExtensibilityTheoryData(
                "IssuerValidatorDelegate",
                tokenHandler,
                securityToken,
                CustomIssuerValidationDelegates.IssuerValidatorDelegateAsync)
            {
                ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException(
                    nameof(CustomIssuerValidationDelegates.IssuerValidatorDelegateAsync)),
                ValidationError = new IssuerValidationError(
                    new MessageDetail(nameof(CustomIssuerValidationDelegates.IssuerValidatorDelegateAsync)),
                    IssuerValidationFailure.ValidationFailed,
                    Default.GetStackFrame(),
                    issuerGuid)
            });

            // Delegate throws CustomSecurityTokenInvalidIssuerException
            // ValidationError: IssuerValidationError
            // Exception: SecurityTokenInvalidIssuerException
            // FailureType: IssuerValidationFailure.ValidatorThrew
            // InnerException: CustomSecurityTokenInvalidIssuerException
            ExtensibilityTheoryData testCase = new ExtensibilityTheoryData(
                "IssuerValidatorThrows",
                tokenHandler,
                securityToken,
                CustomIssuerValidationDelegates.IssuerValidatorThrows)
            {
                ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException(
                        LogMessages.IDX10269,
                        typeof(CustomSecurityTokenInvalidIssuerException)),
                ValidationError = new IssuerValidationError(
                    new MessageDetail(LogMessages.IDX10269),
                    IssuerValidationFailure.ValidatorThrew,
                    Default.GetStackFrame(),
                    issuerGuid)
            };

            // throwing makes the StackFramesCount and StackFrames properties different, so we ignore them when comparing
            testCase.PropertiesToIgnoreWhenComparing[typeof(IssuerValidationError)] = new List<string> { "StackFramesCount", "StackFrames" };
            theoryData.Add(testCase);
            #endregion

            return theoryData;
        }
        #endregion

        #region IssuerSigningKey
        public static TheoryData<ExtensibilityTheoryData> GenerateInvalidIssuerSigningKeyTestCases(
            TokenHandler tokenHandler,
            SecurityToken securityToken)
        {
            var theoryData = new TheoryData<ExtensibilityTheoryData>();
            CallContext callContext = new CallContext();
            string issuerGuid = Guid.NewGuid().ToString();

            #region return CustomIssuerSigningKeyValidationError
            // Delegate sets ValidationFailureType to a value that is known to CustomIssuerSigningKeyValidationError
            // ValidationError: CustomIssuerSigningKeyValidationError
            // FailureType: CustomValidationFailure.ValidationFailed
            // Exception: CustomSecurityTokenInvalidSigningKeyException
            theoryData.Add(new ExtensibilityTheoryData(
                "CustomIssuerSigningKeyValidationFailed",
                tokenHandler,
                securityToken,
                CustomIssuerSigningKeyValidationDelegates.CustomValidationFailed)
            {
                ExpectedException = new ExpectedException(
                    typeof(CustomSecurityTokenInvalidSigningKeyException),
                    nameof(CustomIssuerSigningKeyValidationDelegates.CustomValidationFailed)),
                ValidationError = new CustomIssuerSigningKeyValidationError(
                    new MessageDetail(nameof(CustomIssuerSigningKeyValidationDelegates.CustomValidationFailed)),
                    CustomValidationFailure.IssuerSigningKeyValidationFailed,
                    Default.GetStackFrame(),
                    null)
            });

            // Delegate sets ValidationFailureType to a value that is known to SignatureKeyValidationError
            // ValidationError: CustomIssuerSigningKeyValidationError
            // FailureType: ValidationFailureType.ValidationFailed
            // Exception: SecurityTokenInvalidIssuerSigningKeyException
            theoryData.Add(new ExtensibilityTheoryData(
                "IssuerSigningKeyValidationFailed",
                tokenHandler,
                securityToken,
                CustomIssuerSigningKeyValidationDelegates.IssuerSigningKeyValidationFailed)
            {
                ExpectedException = ExpectedException.SecurityTokenInvalidSigningKeyException(
                    nameof(CustomIssuerSigningKeyValidationDelegates.IssuerSigningKeyValidationFailed)),
                ValidationError = new CustomIssuerSigningKeyValidationError(
                    new MessageDetail(nameof(CustomIssuerSigningKeyValidationDelegates.IssuerSigningKeyValidationFailed)),
                    SignatureKeyValidationFailure.ValidationFailed,
                    Default.GetStackFrame(),
                    null)
            });

            // Delegate sets ValidationFailureType to a value that is NOT known to SignatureKeyValidationError or CustomIssuerSigningKeyValidationError
            // ValidationError: CustomIssuerSigningKeyValidationError
            // FailureType: ValidationFailureType.AlgorithmIsNotSupported
            // Exception: SecurityTokenValidationException
            theoryData.Add(new ExtensibilityTheoryData(
                "IssuerSigningKeyUnknownValidationFailure",
                tokenHandler,
                securityToken,
                CustomIssuerSigningKeyValidationDelegates.UnknownValidationFailure)
            {
                ExpectedException = ExpectedException.SecurityTokenValidationException("UnknownValidationFailure"),
                ValidationError = new CustomIssuerSigningKeyValidationError(
                    new MessageDetail(nameof(CustomIssuerSigningKeyValidationDelegates.UnknownValidationFailure)),
                    AlgorithmValidationFailure.AlgorithmIsNotSupported,
                    Default.GetStackFrame(),
                    null)
            });
            #endregion

            #region SignatureKeyValidationError
            // Delegate returns SignatureKeyValidationError and sets ValidationFailureType to a value that is known to SignatureKeyValidationError
            // ValidationError: SignatureKeyValidationError
            // FailureType: ValidationFailureType.ValidationFailed
            // Exception:  SecurityTokenInvalidIssuerSigningKeyException
            theoryData.Add(new ExtensibilityTheoryData(
                "IssuerSigningKeyDelegate",
                tokenHandler,
                securityToken,
                CustomIssuerSigningKeyValidationDelegates.IssuerSigningKeyDelegate)
            {
                ExpectedException = ExpectedException.SecurityTokenInvalidSigningKeyException(
                    nameof(CustomIssuerSigningKeyValidationDelegates.IssuerSigningKeyDelegate)),
                ValidationError = new CustomIssuerSigningKeyValidationError(
                    new MessageDetail(nameof(CustomIssuerSigningKeyValidationDelegates.IssuerSigningKeyDelegate)),
                    SignatureKeyValidationFailure.ValidationFailed,
                    Default.GetStackFrame(),
                    null),
            });

            // Delegate throws CustomSecurityTokenInvalidIssuerSigningKeyException
            // ValidationError: SignatureKeyValidationError
            // Exception: SecurityTokenInvalidIssuerSigningKeyException
            // FailureType: ValidationFailureType.ValidatorThrew
            // InnerException: CustomSecurityTokenInvalidSigningKeyException
            ExtensibilityTheoryData testCase = new ExtensibilityTheoryData(
            "IssuerSigningKeyValidatorThrows",
            tokenHandler,
            securityToken,
            CustomIssuerSigningKeyValidationDelegates.IssuerSigningKeyValidatorThrows)
            {
                ExpectedException = ExpectedException.SecurityTokenInvalidSigningKeyException(
                    LogMessages.IDX10274,
                    typeof(CustomSecurityTokenInvalidSigningKeyException)),
                ValidationError = new SignatureKeyValidationError(
                new MessageDetail(LogMessages.IDX10274),
                SignatureKeyValidationFailure.ValidatorThrew,
                Default.GetStackFrame(),
                null,
                new SecurityTokenInvalidSigningKeyException(nameof(CustomIssuerSigningKeyValidationDelegates.IssuerSigningKeyValidatorThrows))
            )
            };

            // throwing makes the StackFramesCount and StackFrames properties different, so we ignore them when comparing
            testCase.PropertiesToIgnoreWhenComparing[typeof(SignatureKeyValidationError)] = new List<string> { "StackFramesCount", "StackFrames" };
            theoryData.Add(testCase);
            #endregion

            return theoryData;
        }
        #endregion

        #region Lifetime
        public static TheoryData<ExtensibilityTheoryData> GenerateInvalidLifetimeTestCases(
            TokenHandler tokenHandler,
            SecurityToken securityToken)
        {
            TheoryData<ExtensibilityTheoryData> theoryData = new();
            CallContext callContext = new CallContext();
            DateTime utcNow = DateTime.UtcNow;
            DateTime utcPlusOneHour = utcNow.AddHours(1);

            #region return CustomLifetimeValidationError
            // Delegate sets ValidationFailureType to a value that is known to CustomLifetimeValidationError
            // ValidationError: CustomLifetimeValidationError
            // FailureType: CustomValidationFailure.LifetimeValidationFailed
            // Exception: CustomSecurityTokenInvalidLifetimeException
            theoryData.Add(new ExtensibilityTheoryData(
                "CustomLifetimeValidatorDelegate",
                tokenHandler,
                securityToken,
                CustomLifetimeValidationDelegates.CustomLifetimeValidationFailed)
            {
                ExpectedException = new ExpectedException(
                    typeof(CustomSecurityTokenInvalidLifetimeException),
                    nameof(CustomLifetimeValidationDelegates.CustomLifetimeValidationFailed)),
                ValidationError = new CustomLifetimeValidationError(
                    new MessageDetail(
                        nameof(CustomLifetimeValidationDelegates.CustomLifetimeValidationFailed)),
                    CustomValidationFailure.LifetimeValidationFailed,
                    Default.GetStackFrame(),
                    utcNow,
                    utcPlusOneHour)
            });

            // Delegate sets ValidationFailureType to a value that is known to LifetimeValidationError
            // ValidationError: LifetimeValidationError
            // FailureType: LifetimeValidationFailure.ValidationFailed
            // Exception: SecurityTokenInvalidLifetimeException
            theoryData.Add(new ExtensibilityTheoryData(
                "LifetimeValidationFailed",
                tokenHandler,
                securityToken,
                CustomLifetimeValidationDelegates.LifetimeValidationFailed)
            {
                ExpectedException = ExpectedException.SecurityTokenInvalidLifetimeException(
                    nameof(LifetimeValidationFailure.ValidationFailed)),
                ValidationError = new LifetimeValidationError(
                    new MessageDetail(
                        nameof(LifetimeValidationFailure.ValidationFailed)),
                    LifetimeValidationFailure.ValidationFailed,
                    Default.GetStackFrame(),
                    utcNow,
                    utcPlusOneHour),
            });

            // Delegate sets ValidationFailureType to a value that is NOT known to LifetimeValidationError or CustomLifetimeValidationError
            // ValidationError: CustomLifetimeValidationError
            // FailureType: ValidationFailureType.AlgorithmIsNotSupported
            // Exception: SecurityTokenValidationException
            theoryData.Add(new ExtensibilityTheoryData(
                "LifetimeUnknownValidationFailure",
                tokenHandler,
                securityToken,
                CustomLifetimeValidationDelegates.CustomUnknownValidationFailure)
            {
                ExpectedException = ExpectedException.SecurityTokenValidationException(
                        nameof(CustomLifetimeValidationDelegates.CustomUnknownValidationFailure)),
                ValidationError = new CustomLifetimeValidationError(
                    new MessageDetail(
                        nameof(CustomLifetimeValidationDelegates.CustomUnknownValidationFailure)),
                    AlgorithmValidationFailure.AlgorithmIsNotSupported,
                    Default.GetStackFrame(),
                    utcNow,
                    utcPlusOneHour),
            });
            #endregion

            #region return IssuerSigningKeyValidationError
            // Delegate returns LifetimeValidationError and sets ValidationFailureType to a value that is known to LifetimeValidationError
            // ValidationError: LifetimeValidationError
            // FailureType: LifetimeValidationFailure.ValidationFailed
            // Exception:  SecurityTokenLifetimeSigningKeyException
            theoryData.Add(new ExtensibilityTheoryData(
                "LifetimeValidator",
                tokenHandler,
                securityToken,
                CustomLifetimeValidationDelegates.LifetimeValidator)
            {
                ExpectedException = ExpectedException.SecurityTokenInvalidLifetimeException(
                    nameof(CustomLifetimeValidationDelegates.LifetimeValidator)),
                ValidationError = new LifetimeValidationError(
                    new MessageDetail(nameof(CustomLifetimeValidationDelegates.LifetimeValidator)),
                    LifetimeValidationFailure.ValidationFailed,
                    Default.GetStackFrame(),
                    utcNow,
                    utcPlusOneHour),
            });

            // Delegate throws CustomSecurityTokenInvalidLifetimeException
            // ValidationError: LifetimeValidationError
            // Exception: SecurityTokenInvalidLifetimeException
            // FailureType: LifetimeValidationFailure.ValidatorThrew
            // InnerException: CustomSecurityTokenInvalidLifetimeException
            ExtensibilityTheoryData testCase = new ExtensibilityTheoryData(
                "LifetimeValidatorThrows",
                tokenHandler,
                securityToken,
                CustomLifetimeValidationDelegates.ValidatorThrows)
            {
                ExpectedException = ExpectedException.SecurityTokenInvalidLifetimeException(
                    LogMessages.IDX10271,
                    typeof(CustomSecurityTokenInvalidLifetimeException)),
                ValidationError = new LifetimeValidationError(
                    new MessageDetail(LogMessages.IDX10271),
                    LifetimeValidationFailure.ValidatorThrew,
                    Default.GetStackFrame(),
                    utcNow,
                    utcPlusOneHour,
                    new SecurityTokenInvalidLifetimeException(nameof(CustomLifetimeValidationDelegates.ValidatorThrows))
                )
            };

            // throwing makes the StackFramesCount and StackFrames properties different, so we ignore them when comparing
            testCase.PropertiesToIgnoreWhenComparing[typeof(LifetimeValidationError)] = new List<string> { "StackFramesCount", "StackFrames" };

            theoryData.Add(testCase);
            #endregion

            return theoryData;
        }

        #endregion

        #region Signature
        public static TheoryData<ExtensibilityTheoryData> GenerateInvalidSignatureTestCases(
            TokenHandler tokenHandler,
            SecurityToken securityToken)
        {
            var theoryData = new TheoryData<ExtensibilityTheoryData>();
            CallContext callContext = new CallContext();
            string issuerGuid = Guid.NewGuid().ToString();

            #region return CustomSignatureValidationError
            // Delegate sets ValidationFailureType to a value that is known to CustomSignatureValidationError
            // ValidationError: CustomSignatureValidationError
            // FailureType: CustomValidationFailures.ValidationFailed
            // Exception: CustomSecurityTokenInvalidSignatureException
            theoryData.Add(new ExtensibilityTheoryData(
                "CustomSignatureValidationFailed",
                tokenHandler,
                securityToken,
                CustomSignatureValidationDelegates.CustomSignatureValidationFailed)
            {
                ExpectedException = new ExpectedException(
                    typeof(CustomSecurityTokenInvalidSignatureException),
                    nameof(CustomSignatureValidationDelegates.CustomSignatureValidationFailed)),
                ValidationError = new CustomSignatureValidationError(
                    new MessageDetail(nameof(CustomSignatureValidationDelegates.CustomSignatureValidationFailed)),
                    CustomValidationFailure.SignatureValidationFailed,
                    Default.GetStackFrame())
            });

            // Delegate sets ValidationFailureType to a value that is known to SignatureValidationError
            // ValidationError: SignatureValidationError
            // FailureType: ValidationFailureType.ValidationFailed
            // Exception: SecurityTokenInvalidSignatureException
            theoryData.Add(new ExtensibilityTheoryData(
                "SignatureValidationFailed",
                tokenHandler,
                securityToken,
                CustomSignatureValidationDelegates.SignatureValidationFailed)
            {
                ExpectedException = ExpectedException.SecurityTokenInvalidSignatureException(
                        nameof(CustomSignatureValidationDelegates.SignatureValidationFailed)),
                ValidationError = new CustomSignatureValidationError(
                    new MessageDetail(nameof(CustomSignatureValidationDelegates.SignatureValidationFailed)),
                    SignatureValidationFailure.ValidationFailed,
                    Default.GetStackFrame()),
            });

            // Delegate sets ValidationFailureType to a value that is NOT known to SignatureValidationError or CustomSignatureValidationError
            // ValidationError: CustomLifetimeValidationError
            // FailureType: ValidationFailureType.AlgorithmIsNotSupported
            // Exception: SecurityTokenValidationException
            theoryData.Add(new ExtensibilityTheoryData(
                "SignatureUnknownValidationFailure",
                tokenHandler,
                securityToken,
                CustomSignatureValidationDelegates.UnknownValidationFailure)
            {
                ExpectedException = ExpectedException.SecurityTokenValidationException(
                    nameof(CustomSignatureValidationDelegates.UnknownValidationFailure)),
                ValidationError = new CustomSignatureValidationError(
                    new MessageDetail(nameof(CustomSignatureValidationDelegates.UnknownValidationFailure)),
                    AlgorithmValidationFailure.AlgorithmIsNotSupported,
                    Default.GetStackFrame()),
            });
            #endregion

            #region return SignatureValidationError
            // Delegate returns SignatureValidationError and sets ValidationFailureType to a value that is known to SignatureValidationError
            // ValidationError: SignatureValidationError
            // FailureType: SignatureValidationFailure.ValidationFailed
            // Exception:  SecurityTokenInvalidSignatureException
            theoryData.Add(new ExtensibilityTheoryData(
                "SignatureValidatorDelegate",
                tokenHandler,
                securityToken,
                CustomSignatureValidationDelegates.SignatureValidatorDelegate)
            {
                ExpectedException = ExpectedException.SecurityTokenInvalidSignatureException(
                    nameof(CustomSignatureValidationDelegates.SignatureValidatorDelegate)),
                ValidationError = new SignatureValidationError(
                    new MessageDetail(nameof(CustomSignatureValidationDelegates.SignatureValidatorDelegate)),
                    SignatureValidationFailure.ValidationFailed,
                    Default.GetStackFrame())
            });

            // Delegate throws CustomSecurityTokenInvalidSignatureException
            // ValidationError: SignatureValidationError
            // Exception: SecurityTokenInvalidSignatureException
            // FailureType: ValidationFailureType.ValidatorThrew
            // InnerException: CustomSecurityTokenInvalidSignatureException
            ExtensibilityTheoryData testCase = new ExtensibilityTheoryData(
                "SignatureValidatorThrows",
                tokenHandler,
                securityToken,
                CustomSignatureValidationDelegates.SignatureValidatorThrows)
            {
                ExpectedException = ExpectedException.SecurityTokenInvalidSignatureException(
                        LogMessages.IDX10272,
                        typeof(CustomSecurityTokenInvalidSignatureException)),
                ValidationError = new SignatureValidationError(
                    new MessageDetail(LogMessages.IDX10272),
                    SignatureValidationFailure.ValidatorThrew,
                    Default.GetStackFrame())
            };

            // throwing makes the StackFramesCount and StackFrames properties different, so we ignore them when comparing
            testCase.PropertiesToIgnoreWhenComparing[typeof(SignatureValidationError)] = new List<string> { "StackFramesCount", "StackFrames" };

            theoryData.Add(testCase);
            #endregion

            return theoryData;
        }
        #endregion

        #region TokenReplay
        public static TheoryData<ExtensibilityTheoryData> GenerateInvalidTokenReplayTestCases(
            TokenHandler tokenHandler,
            SecurityToken securityToken)
        {
            var theoryData = new TheoryData<ExtensibilityTheoryData>();
            CallContext callContext = new CallContext();
            DateTime expirationTime = DateTime.UtcNow + TimeSpan.FromHours(1);

            #region return CustomTokenReplayValidationError
            // Delegate sets ValidationFailureType to a value that is known to CustomTokenReplayValidationError
            // ValidationError: CustomTokenReplayValidationError
            // FailureType: CustomValidationFailure.ValidationFailed
            // Exception: CustomSecurityTokenReplayDetectedException
            theoryData.Add(new ExtensibilityTheoryData(
                "CustomTokenReplayValidationDelegate",
                tokenHandler,
                securityToken,
                CustomTokenReplayValidationDelegates.CustomTokenReplayValidationFailed)
            {
                ExpectedException = new ExpectedException(
                    typeof(CustomSecurityTokenReplayDetectedException),
                    nameof(CustomTokenReplayValidationDelegates.CustomTokenReplayValidationFailed)),
                ValidationError = new CustomTokenReplayValidationError(
                    new MessageDetail(nameof(CustomTokenReplayValidationDelegates.CustomTokenReplayValidationFailed)),
                    CustomValidationFailure.TokenReplayValidationFailed,
                    Default.GetStackFrame(),
                    expirationTime)
            });

            // Delegate sets ValidationFailureType to a value that is known to TokenReplayDetectedError
            // ValidationError: TokenReplayValidationError
            // FailureType: TokenReplayValidationFailure.ValidationFailed
            // Exception: SecurityTokenReplayDetectedException
            theoryData.Add(new ExtensibilityTheoryData(
                "TokenReplayValidationFailed",
                tokenHandler,
                securityToken,
                CustomTokenReplayValidationDelegates.TokenReplayValidationFailed)
            {
                ExpectedException = ExpectedException.SecurityTokenReplayDetectedException(
                    nameof(CustomTokenReplayValidationDelegates.TokenReplayValidationFailed)),
                ValidationError = new CustomTokenReplayValidationError(
                    new MessageDetail(nameof(CustomTokenReplayValidationDelegates.TokenReplayValidationFailed)),
                    TokenReplayValidationFailure.ValidationFailed,
                    Default.GetStackFrame(),
                    expirationTime),
            });

            // Delegate sets ValidationFailureType to a value that is NOT known to
            //   TokenReplayValidationError or CustomTokenReplayValidationError
            // ValidationError: CustomTokenReplayValidationError
            // FailureType: ValidationFailureType.AlgorithmIsNotSupported
            // Exception: SecurityTokenValidationException
            theoryData.Add(new ExtensibilityTheoryData(
                "TokenReplayUnknownValidationFailure",
                tokenHandler,
                securityToken,
                CustomTokenReplayValidationDelegates.UnknownValidationFailure)
            {
                ExpectedException = ExpectedException.SecurityTokenValidationException(
                        nameof(CustomTokenReplayValidationDelegates.UnknownValidationFailure)),
                ValidationError = new CustomTokenReplayValidationError(
                    new MessageDetail(nameof(CustomTokenReplayValidationDelegates.UnknownValidationFailure)),
                    AlgorithmValidationFailure.AlgorithmIsNotSupported,
                    Default.GetStackFrame(),
                    expirationTime)
            });
            #endregion

            #region return TokenReplayValidationError
            // Delegate returns TokenReplayValidationError and sets ValidationFailureType to a value that is known to TokenReplayValidationError
            // ValidationError: TokenReplayValidationError
            // FailureType: TokenReplayValidationFailure.ValidationFailed
            // Exception:  SecurityTokenReplayDetectedException
            theoryData.Add(new ExtensibilityTheoryData(
                "TokenReplayValidationDelegate",
                tokenHandler,
                securityToken,
                CustomTokenReplayValidationDelegates.TokenReplayValidationDelegate)
            {
                ExpectedException = ExpectedException.SecurityTokenReplayDetectedException(
                    nameof(CustomTokenReplayValidationDelegates.TokenReplayValidationDelegate)),
                ValidationError = new TokenReplayValidationError(
                    new MessageDetail(nameof(CustomTokenReplayValidationDelegates.TokenReplayValidationDelegate)),
                    TokenReplayValidationFailure.ValidationFailed,
                    Default.GetStackFrame(),
                    expirationTime)
            });

            // Delegate throws CustomSecurityTokenTokenReplayDetectedException
            // ValidationError: TokenReplayValidationError
            // Exception: SecurityTokenReplayDetectedException
            // FailureType: TokenReplayValidationFailure.ValidatorThrew
            // InnerException: CustomSecurityTokenReplayDetectedException
            ExtensibilityTheoryData testCase = new ExtensibilityTheoryData(
                "TokenReplayValidatorThrows",
                tokenHandler,
                securityToken,
                CustomTokenReplayValidationDelegates.TokenReplayValidatorThrows)
            {
                ExpectedException = ExpectedException.SecurityTokenReplayDetectedException(
                    LogMessages.IDX10276,
                    typeof(CustomSecurityTokenReplayDetectedException)),
                ValidationError = new TokenReplayValidationError(
                    new MessageDetail(LogMessages.IDX10276),
                    TokenReplayValidationFailure.ValidatorThrew,
                    Default.GetStackFrame(),
                    expirationTime,
                    new CustomSecurityTokenReplayDetectedException(
                        nameof(CustomTokenReplayValidationDelegates.TokenReplayValidatorThrows),
                        new TokenReplayValidationError(
                            new MessageDetail(nameof(CustomTokenReplayValidationDelegates.TokenReplayValidatorThrows)),
                            TokenReplayValidationFailure.ValidatorThrew,
                            Default.GetStackFrame(),
                            expirationTime),
                        null)
                )
            };

            // throwing makes the StackFramesCount and StackFrames properties different, so we ignore them when comparing
            testCase.PropertiesToIgnoreWhenComparing[typeof(TokenReplayValidationError)] = new List<string> { "StackFramesCount", "StackFrames" };

            theoryData.Add(testCase);

            #endregion

            return theoryData;
        }
        #endregion

        #region TokenType
        public static TheoryData<ExtensibilityTheoryData> GenerateInvalidTokenTypeTestCases(
            TokenHandler tokenHandler,
            SecurityToken securityToken)
        {
            var theoryData = new TheoryData<ExtensibilityTheoryData>();
            CallContext callContext = new CallContext();
            string tokenType = "NOTJWT";

            #region return CustomTokenTypeValidationError
            // Delegate sets ValidationFailureType to a value that is known to CustomTokenTypeValidationError
            // ValidationError: CustomTokenTypeValidationError
            // FailureType: CustomValidationFailure.ValidationFailed
            // Exception: CustomSecurityTokenInvalidTypeException
            theoryData.Add(new ExtensibilityTheoryData(
                "CustomTokenTypeValidationFailed",
                tokenHandler,
                securityToken,
                CustomTokenTypeValidationDelegates.CustomTokenTypeValidationFailed)
            {
                ExpectedException = new ExpectedException(
                    typeof(CustomSecurityTokenInvalidTypeException),
                    nameof(CustomTokenTypeValidationDelegates.CustomTokenTypeValidationFailed)),
                ValidationError = new CustomTokenTypeValidationError(
                    new MessageDetail(nameof(CustomTokenTypeValidationDelegates.CustomTokenTypeValidationFailed)),
                    CustomValidationFailure.TokenTypeValidationFailed,
                    Default.GetStackFrame(),
                    tokenType)
            });

            // Delegate sets ValidationFailureType to a value that is known to TokenTypeValidationError
            // ValidationError: TokenTypeValidationError
            // FailureType: TokenTypeValidationFailure.ValidationFailed
            // Exception: SecurityTokenInvalidTypeException
            theoryData.Add(new ExtensibilityTheoryData(
                "TokenTypeValidationFailed",
                tokenHandler,
                securityToken,
                CustomTokenTypeValidationDelegates.TokenTypeValidationFailed)
            {
                ExpectedException = ExpectedException.SecurityTokenInvalidTypeException(
                    nameof(CustomTokenTypeValidationDelegates.TokenTypeValidationFailed)),
                ValidationError = new CustomTokenTypeValidationError(
                    new MessageDetail(nameof(CustomTokenTypeValidationDelegates.TokenTypeValidationFailed)),
                    TokenTypeValidationFailure.ValidationFailed,
                    Default.GetStackFrame(),
                    tokenType)
            });

            // Delegate sets ValidationFailureType to a value that is NOT known to TokenTypeValidationError or CustomTokenTypeValidationError
            // ValidationError: CustomTokenTypeValidationError
            // FailureType: ValidationFailureType.AlgorithmIsNotSupported
            // Exception: SecurityTokenValidationException
            theoryData.Add(new ExtensibilityTheoryData(
                "TokenTypeUnknownValidationFailure",
                tokenHandler,
                securityToken,
                CustomTokenTypeValidationDelegates.UnknownValidationFailure)
            {
                ExpectedException = ExpectedException.SecurityTokenValidationException(
                    nameof(CustomTokenTypeValidationDelegates.UnknownValidationFailure)),
                ValidationError = new CustomTokenTypeValidationError(
                    new MessageDetail(nameof(CustomTokenTypeValidationDelegates.UnknownValidationFailure)),
                    AlgorithmValidationFailure.AlgorithmIsNotSupported,
                    Default.GetStackFrame(),
                    tokenType),
            });
            #endregion

            #region return TokenTypeValidationError
            // Delegate returns TokenTypeValidationError and sets ValidationFailureType to a value that is known to TokenTypeValidationError
            // ValidationError: TokenTypeValidationError
            // FailureType: TokenTypeValidationFailure.ValidationFailed
            // Exception:  SecurityTokenInvalidTokenTypeException
            theoryData.Add(new ExtensibilityTheoryData(
                "TokenTypeValidatorDelegate",
                tokenHandler,
                securityToken,
                CustomTokenTypeValidationDelegates.TokenTypeValidatorDelegate)
            {
                ExpectedException = ExpectedException.SecurityTokenInvalidTypeException(
                    nameof(CustomTokenTypeValidationDelegates.TokenTypeValidatorDelegate)),
                ValidationError = new TokenTypeValidationError(
                    new MessageDetail(nameof(CustomTokenTypeValidationDelegates.TokenTypeValidatorDelegate)),
                    TokenTypeValidationFailure.ValidationFailed,
                    Default.GetStackFrame(),
                    tokenType)
            });

            // Delegate throws CustomSecurityTokenInvalidTypeException
            // ValidationError: TokenTypeValidationError
            // Exception: SecurityTokenInvalidTypeException
            // FailureType: TokenTypeValidationFailure.ValidatorThrew
            // InnerException: CustomSecurityTokenInvalidTypeException
            ExtensibilityTheoryData testCase = new ExtensibilityTheoryData(
                "TokenTypeValidatorThrows",
                tokenHandler,
                securityToken,
                CustomTokenTypeValidationDelegates.TokenTypeValidatorThrows)
            {
                ExpectedException = ExpectedException.SecurityTokenInvalidTypeException(
                    LogMessages.IDX10275,
                    typeof(CustomSecurityTokenInvalidTypeException)),
                ValidationError = new TokenTypeValidationError(
                    new MessageDetail(LogMessages.IDX10275),
                    TokenTypeValidationFailure.ValidatorThrew,
                    Default.GetStackFrame(),
                    tokenType,
                    new SecurityTokenInvalidTypeException(nameof(CustomTokenTypeValidationDelegates.TokenTypeValidatorThrows))
                )
            };

            // throwing makes the StackFramesCount and StackFrames properties different, so we ignore them when comparing
            testCase.PropertiesToIgnoreWhenComparing[typeof(TokenTypeValidationError)] = new List<string> { "StackFramesCount", "StackFrames" };

            theoryData.Add(testCase);

            #endregion

            return theoryData;
        }
        #endregion
    }
}
#nullable restore
