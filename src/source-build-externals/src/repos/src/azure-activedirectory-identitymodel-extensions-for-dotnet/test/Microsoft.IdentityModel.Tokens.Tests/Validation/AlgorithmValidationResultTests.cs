// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Logging;
using Xunit;

namespace Microsoft.IdentityModel.Tokens.Validation.Tests
{
    public class AlgorithmValidationResultTests
    {
        [Theory, MemberData(nameof(AlgorithmValidationTestCases), DisableDiscoveryEnumeration = true)]
        public void ValidateAlgorithm(AlgorithmTheoryData theoryData)
        {
            CompareContext context = TestUtilities.WriteHeader($"{this}.AlgorithmValidationResultTests", theoryData);

            AlgorithmValidationResult algorithmValidationResult = Validators.ValidateAlgorithm(
                theoryData.Algorithm,
                theoryData.SecurityKey,
                theoryData.SecurityToken,
                theoryData.ValidationParameters,
                new CallContext());

            if (algorithmValidationResult.Exception != null)
                theoryData.ExpectedException.ProcessException(algorithmValidationResult.Exception);
            else
                theoryData.ExpectedException.ProcessNoException();

            IdentityComparer.AreAlgorithmValidationResultsEqual(
                algorithmValidationResult,
                theoryData.AlgorithmValidationResult,
                context);

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<AlgorithmTheoryData> AlgorithmValidationTestCases
        {
            get
            {
                SecurityKey securityKey = new SymmetricSecurityKey(new byte[256]);

                return new TheoryData<AlgorithmTheoryData>
                {
                    new AlgorithmTheoryData
                    {
                        TestId = "Invalid_ValidationParametersAreNull",
                        ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                        Algorithm = null,
                        SecurityKey = null,
                        SecurityToken = null,
                        ValidationParameters = null,
                        AlgorithmValidationResult = new AlgorithmValidationResult(
                            null,
                            ValidationFailureType.NullArgument,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10000,
                                    LogHelper.MarkAsNonPII("validationParameters")),
                                typeof(ArgumentNullException),
                                new StackFrame(true)))
                    },
                    new AlgorithmTheoryData
                    {
                        TestId = "Invalid_ValidateAlgorithmNotAValidAlgorithm",
                        ExpectedException = ExpectedException.SecurityTokenInvalidAlgorithmException("IDX10696:"),
                        Algorithm = SecurityAlgorithms.Sha256,
                        SecurityKey = securityKey,
                        SecurityToken = null,
                        ValidationParameters = new ValidationParameters
                        {
                            ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 }
                        },
                        AlgorithmValidationResult = new AlgorithmValidationResult(
                            SecurityAlgorithms.Sha256,
                            ValidationFailureType.AlgorithmValidationFailed,
                            new ExceptionDetail(
                                new MessageDetail(
                                    LogMessages.IDX10696,
                                    LogHelper.MarkAsNonPII(SecurityAlgorithms.Sha256),
                                    securityKey),
                                typeof(SecurityTokenInvalidAlgorithmException),
                                new StackFrame(true)))
                    },
                    new AlgorithmTheoryData
                    {
                        TestId = "Valid_ValidateAlgorithmWhenValidAlgorithmsIsNull",
                        Algorithm = SecurityAlgorithms.Sha256,
                        SecurityKey = securityKey,
                        SecurityToken = null,
                        ValidationParameters = new ValidationParameters
                        {
                            ValidAlgorithms = null
                        },
                        AlgorithmValidationResult = new AlgorithmValidationResult(SecurityAlgorithms.Sha256)
                    },
                    new AlgorithmTheoryData
                    {
                        TestId = "Valid_ValidateAlgorithmDefaultAlgorithmValidation",
                        Algorithm = SecurityAlgorithms.Sha256,
                        SecurityKey = securityKey,
                        SecurityToken = null,
                        ValidationParameters = new ValidationParameters
                        {
                            ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256, SecurityAlgorithms.Sha256 }
                        },
                        AlgorithmValidationResult = new AlgorithmValidationResult(SecurityAlgorithms.Sha256)
                    }
                };
            }
        }

        public class AlgorithmTheoryData : TheoryDataBase
        {
            public string Algorithm { get; set; }

            public SecurityKey SecurityKey { get; set; }

            public SecurityToken SecurityToken { get; set; }

            internal ValidationParameters ValidationParameters { get; set; }

            internal AlgorithmValidationResult AlgorithmValidationResult { get; set; }
        }
    }
}
