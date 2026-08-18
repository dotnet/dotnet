// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

#nullable enable
using System.Threading.Tasks;
using System.Threading;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;

namespace Microsoft.IdentityModel.JsonWebTokens.Tests
{
    public partial class JwtValidationTests
    {
        internal static async Task ValidateAndCompareResults(
            string jwtString,
            ValidateTokenTheoryData theoryData,
            CompareContext context)
        {
            JsonWebTokenHandler jsonWebTokenHandler = new JsonWebTokenHandler();

            // Validate the token using TokenValidationParameters
            TokenValidationResult legacyTokenValidationParametersResult =
                await jsonWebTokenHandler.ValidateTokenAsync(jwtString, theoryData.TokenValidationParameters);

            // Validate the token using ValidationParameters
            ValidationResult<ValidatedToken, ValidationError> validationResult =
                await jsonWebTokenHandler.ValidateTokenAsync(
                    jwtString, theoryData.ValidationParameters!, theoryData.CallContext, CancellationToken.None);

            // Ensure the validity of the results match the expected result
            if (legacyTokenValidationParametersResult.IsValid != theoryData.ExpectedIsValid)
                context.AddDiff($"tokenValidationParametersResult.IsValid != theoryData.ExpectedIsValid");

            if (validationResult.Succeeded != theoryData.ExpectedIsValid)
                context.AddDiff($"validationResult.Succeeded != theoryData.ExpectedIsValid");

            if (theoryData.ExpectedIsValid &&
                legacyTokenValidationParametersResult.IsValid &&
                validationResult.Succeeded)
            {
                // Compare the ClaimsPrincipal and ClaimsIdentity from one result against the other
                IdentityComparer.AreEqual(
                    legacyTokenValidationParametersResult.ClaimsIdentity,
                    validationResult.Result!.ClaimsIdentity,
                    context);
                IdentityComparer.AreEqual(
                    legacyTokenValidationParametersResult.Claims,
                    validationResult.Result!.Claims,
                    context);
            }
            else
            {
                // Verify the exception provided by the TokenValidationParameters path
                theoryData.ExpectedException.ProcessException(legacyTokenValidationParametersResult.Exception, context);

                if (!validationResult.Succeeded)
                {
                    // Verify the exception provided by the ValidationParameters path
                    if (theoryData.ExpectedExceptionValidationParameters is not null)
                    {
                        // If there is a special case for the ValidationParameters path, use that.
                        theoryData.ExpectedExceptionValidationParameters
                            .ProcessException(validationResult.Error!.GetException(), context);
                    }
                    else
                    {
                        theoryData.ExpectedException
                            .ProcessException(validationResult.Error!.GetException(), context);

                        // If the expected exception is the same in both paths, verify the message matches
                        IdentityComparer.AreStringsEqual(
                            legacyTokenValidationParametersResult.Exception.Message,
                            validationResult.Error!.GetException().Message,
                            context);
                    }
                }

                // Verify that the exceptions are of the same type.
                IdentityComparer.AreEqual(
                    legacyTokenValidationParametersResult.Exception.GetType(),
                    validationResult.Error!.GetException().GetType(),
                    context);

                if (legacyTokenValidationParametersResult.Exception is SecurityTokenException)
                {
                    // Verify that the custom properties are the same.
                    IdentityComparer.AreSecurityTokenExceptionsEqual(
                        legacyTokenValidationParametersResult.Exception,
                        validationResult.Error!.GetException(),
                        context);
                }
            }
        }

        internal async Task RunValidTest(ValidateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.RunValidTest", theoryData);

            // Validate the token using TokenValidationParameters
            TokenValidationResult tokenValidationResult =
                await theoryData.TokenHandler!.ValidateTokenAsync(
                    theoryData.Token,
                    theoryData.TokenValidationParameters);

            // Validate the token using ValidationParameters.
            ValidationResult<ValidatedToken, ValidationError> validationResult =
                await theoryData.TokenHandler.ValidateTokenAsync(
                    theoryData.Token,
                    theoryData.ValidationParameters!,
                    theoryData.CallContext,
                    CancellationToken.None);

            // Ensure the validity of the results match the expected result.
            if (tokenValidationResult.IsValid != validationResult.Succeeded)
                context.AddDiff($"tokenValidationResult.IsValid: '{tokenValidationResult.IsValid}' != OperationResult.Succeeded: '{validationResult.Succeeded}'");

            if (!validationResult.Succeeded)
                context.AddDiff($"Expected test to succeed, test failed: {theoryData.TestId}");

            ValidatedToken validatedToken = validationResult.Result!;
            IdentityComparer.AreEqual(validatedToken.SecurityToken, tokenValidationResult.SecurityToken, context);

            TestUtilities.AssertFailIfErrors(context);
        }
    }
}
#nullable restore
