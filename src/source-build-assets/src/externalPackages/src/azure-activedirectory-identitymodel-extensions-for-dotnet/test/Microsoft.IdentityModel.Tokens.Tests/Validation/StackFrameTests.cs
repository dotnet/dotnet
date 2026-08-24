// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens.Experimental;
using Xunit;

namespace Microsoft.IdentityModel.Tokens.Validation.Tests
{
    public class StackFrameTests
    {
        [Theory, MemberData(nameof(StackFrameTestCases), DisableDiscoveryEnumeration = true)]
        public async Task CallStack(StackFrameTheoryData theoryData)
        {
            CompareContext context = TestUtilities.WriteHeader($"{this}.CallStack", theoryData);
            JsonWebTokenHandler jsonWebTokenHandler = new JsonWebTokenHandler();

            ValidationResult<ValidatedToken, ValidationError> validationResult =
                await jsonWebTokenHandler.ValidateTokenAsync(
                    theoryData.SecurityToken,
                    theoryData.ValidationParameters,
                    theoryData.CallContext,
                    CancellationToken.None);

            validationResult =
                await jsonWebTokenHandler.ValidateTokenAsync(
                    theoryData.SecurityToken,
                    theoryData.ValidationParameters,
                    theoryData.CallContext,
                    CancellationToken.None);

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<StackFrameTheoryData> StackFrameTestCases
        {
            get
            {
                TheoryData<StackFrameTheoryData> theoryData = new TheoryData<StackFrameTheoryData>();

                theoryData.Add(new StackFrameTheoryData("Expired")
                {
                    SecurityToken = CreateToken(DateTime.UtcNow, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow.AddMinutes(-20)),
                    ValidationParameters = CreateValidationParameters()
                });

                return theoryData;
            }
        }

        static ValidationParameters CreateValidationParameters(TimeSpan? clockSkew = null)
        {
            ValidationParameters validationParameters = new ValidationParameters();

            if (clockSkew is not null)
                validationParameters.ClockSkew = clockSkew.Value;

            // Skip all validations except lifetime
            validationParameters.AlgorithmValidator = SkipValidationDelegates.SkipAlgorithmValidation;
            validationParameters.AudienceValidator = SkipValidationDelegates.SkipAudienceValidation;
            validationParameters.IssuerValidatorAsync = SkipValidationDelegates.SkipIssuerValidation;
            validationParameters.SignatureKeyValidator = SkipValidationDelegates.SkipIssuerSigningKeyValidation;
            validationParameters.SignatureValidator = SkipValidationDelegates.SkipSignatureValidation;

            return validationParameters;
        }

        private static JsonWebToken CreateToken(DateTime? issuedAt, DateTime? notBefore, DateTime? expires)
        {
            JsonWebTokenHandler jsonWebTokenHandler = new JsonWebTokenHandler();
            jsonWebTokenHandler.SetDefaultTimesOnTokenCreation = false; // Allow for null values to be passed in to validate.

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = Default.ClaimsIdentity,
                SigningCredentials = KeyingMaterial.RsaSigningCreds_2048,
                IssuedAt = issuedAt,
                NotBefore = notBefore,
                Expires = expires,
            };

            return new JsonWebToken(jsonWebTokenHandler.CreateToken(securityTokenDescriptor));
        }
    }

    public class StackFrameTheoryData : TheoryDataBase
    {
        public StackFrameTheoryData(string testId) : base(testId) { }
        public SecurityToken SecurityToken { get; set; }
        internal ValidationParameters ValidationParameters { get; set; }
        internal ValidationResult<ValidatedLifetime, LifetimeValidationError> OperationResult { get; set; }
        internal ValidationFailureType ValidationFailure { get; set; }
    }
}
