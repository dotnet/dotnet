// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens.Json.Tests;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Xunit;
using Microsoft.IdentityModel.Tokens.Experimental;

namespace Microsoft.IdentityModel.Tokens.Validation.Tests
{
    public class IssuerValidationTests
    {
        [Theory, MemberData(nameof(InvalidIssuerTestCases), DisableDiscoveryEnumeration = true)]
        public async Task InvalidIssuers(IssuerValidationTheoryData theoryData)
        {
            CompareContext context = TestUtilities.WriteHeader($"{this}.InvalidIssuers", theoryData);

            if (theoryData.ValidIssuerToAdd != null)
                theoryData.ValidationParameters.ValidIssuers.Add(theoryData.ValidIssuerToAdd);

            try
            {
                ValidationResult<ValidatedIssuer, ValidationError> validationResult =
                    await Validators.ValidateIssuerAsync(
                        theoryData.Issuer,
                        theoryData.SecurityToken,
                        theoryData.ValidationParameters,
                        theoryData.CallContext,
                        CancellationToken.None);

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

        public static TheoryData<IssuerValidationTheoryData> InvalidIssuerTestCases
        {
            get
            {
                TheoryData<IssuerValidationTheoryData> theoryData = new();

                string validIssuer = Guid.NewGuid().ToString();
                string issClaim = Guid.NewGuid().ToString();
                //var validConfig = new OpenIdConnectConfiguration() { Issuer = issClaim };
                string[] validIssuers = new string[] { validIssuer };

                theoryData.Add(new IssuerValidationTheoryData("NULL_Issuer")
                {
                    ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException("IDX10211:"),
                    OperationResult = new IssuerValidationError(
                        new MessageDetail(
                            LogMessages.IDX10211,
                            LogHelper.MarkAsNonPII(null),
                            LogHelper.MarkAsNonPII(validIssuer),
                            LogHelper.MarkAsNonPII(Utility.SerializeAsSingleCommaDelimitedString(null)),
                            LogHelper.MarkAsNonPII(null)),
                        IssuerValidationFailure.NoIssuerInToken,
                        null,
                        null),
                    SecurityToken = JsonUtilities.CreateUnsignedJsonWebToken(JwtRegisteredClaimNames.Iss, issClaim),
                    ValidationParameters = new ValidationParameters()
                });

                theoryData.Add(new IssuerValidationTheoryData("NULL_ValidationParameters")
                {
                    ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                    Issuer = issClaim,
                    OperationResult = new IssuerValidationError(
                        new MessageDetail(
                            LogMessages.IDX10000,
                            LogHelper.MarkAsNonPII("validationParameters")),
                        ValidationFailureType.NullArgument,
                        null,
                        null),
                    SecurityToken = JsonUtilities.CreateUnsignedJsonWebToken(JwtRegisteredClaimNames.Iss, issClaim),
                    ValidationParameters = null
                });

                theoryData.Add(new IssuerValidationTheoryData("Invalid_Issuer")
                {
                    ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException("IDX10212:"),
                    Issuer = issClaim,
                    OperationResult = new IssuerValidationError(
                        new MessageDetail(
                            LogMessages.IDX10212,
                            LogHelper.MarkAsNonPII(issClaim),
                            LogHelper.MarkAsNonPII(Utility.SerializeAsSingleCommaDelimitedString(validIssuers)),
                            LogHelper.MarkAsNonPII(null)),
                        IssuerValidationFailure.ValidationFailed,
                        null,
                        issClaim),
                    SecurityToken = JsonUtilities.CreateUnsignedJsonWebToken(JwtRegisteredClaimNames.Iss, issClaim),
                    ValidationParameters = new ValidationParameters(),
                    ValidIssuerToAdd = validIssuer
                });

                return theoryData;
            }
        }

        [Theory, MemberData(nameof(ValidIssuerTestCases), DisableDiscoveryEnumeration = true)]
        public async Task ValidIssuers(IssuerValidationTheoryData theoryData)
        {
            CompareContext context = TestUtilities.WriteHeader($"{this}.ValidIssuers", theoryData);

            if (theoryData.ValidIssuerToAdd != null)
                theoryData.ValidationParameters.ValidIssuers.Add(theoryData.ValidIssuerToAdd);

            ValidationResult<ValidatedIssuer, ValidationError> validationResult =
                await Validators.ValidateIssuerAsync(
                    theoryData.Issuer,
                    theoryData.SecurityToken,
                    theoryData.ValidationParameters,
                    theoryData.CallContext,
                    CancellationToken.None);

            try
            {
                if (validationResult.Succeeded)
                {
                    IdentityComparer.AreValidatedIssuersEqual(
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

        public static TheoryData<IssuerValidationTheoryData> ValidIssuerTestCases
        {
            get
            {
                TheoryData<IssuerValidationTheoryData> theoryData = new();

                string validIssuer = Guid.NewGuid().ToString();
                string issClaim = Guid.NewGuid().ToString();
                var validConfig = new OpenIdConnectConfiguration() { Issuer = issClaim };
                string[] validIssuers = new string[] { validIssuer };

                theoryData.Add(new IssuerValidationTheoryData("FromConfig")
                {
                    Issuer = issClaim,
                    OperationResult = new ValidatedIssuer(issClaim, IssuerValidationSource.IssuerMatchedConfiguration),
                    SecurityToken = JsonUtilities.CreateUnsignedJsonWebToken(JwtRegisteredClaimNames.Iss, issClaim),
                    ValidationParameters = new ValidationParameters()
                    {
                        ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(validConfig)
                    }
                });

                theoryData.Add(new IssuerValidationTheoryData("FromValidationParametersValidIssuers")
                {
                    Issuer = issClaim,
                    OperationResult = new ValidatedIssuer(issClaim, IssuerValidationSource.IssuerMatchedValidationParameters),
                    SecurityToken = JsonUtilities.CreateUnsignedJsonWebToken(JwtRegisteredClaimNames.Iss, issClaim),
                    ValidationParameters = new ValidationParameters(),
                    ValidIssuerToAdd = issClaim
                });

                return theoryData;
            }
        }
    }

    public class IssuerValidationTheoryData : TheoryDataBase
    {
        public IssuerValidationTheoryData(string testId) : base(testId) { }

        public BaseConfiguration Configuration { get; set; }

        public string Issuer { get; set; }

        internal ValidationResult<ValidatedIssuer, IssuerValidationError> OperationResult { get; set; }

        public SecurityToken SecurityToken { get; set; }

        internal ValidationParameters ValidationParameters { get; set; }

        internal ValidationFailureType ValidationFailure { get; set; }
        public string ValidIssuerToAdd { get; internal set; }
    }
}
