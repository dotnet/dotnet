// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens.Experimental;

#nullable enable
namespace Microsoft.IdentityModel.Tokens.Extensibility.Tests
{
    public partial class ExtensibilityRunner
    {
        public static async Task RunInvalidTest(ExtensibilityTheoryData theoryData, object testInstance, string methodName)
        {
            var context = TestUtilities.WriteHeader($"{testInstance}.{methodName}", theoryData);
            context.IgnoreType = false;
            try
            {
                ValidationResult<ValidatedToken, ValidationError> validationResult = await theoryData.TokenHandler.ValidateTokenAsync(
                    theoryData.SecurityToken,
                    theoryData.ValidationParameters!,
                    theoryData.CallContext,
                    CancellationToken.None);

                if (validationResult.Succeeded)
                {
                    context.AddDiff("validationResult.Succeeded == true, expected false");
                }
                else
                {
                    ValidationError validationError = validationResult.Error!;
                    IdentityComparer.AreValidationErrorsEqual(
                        validationError,
                        theoryData.ValidationError,
                        context);

                    theoryData.ExpectedException.ProcessException(validationError.GetException(), context);

                    // In the algorithm validation case, we want to ensure the inner exception contains
                    // the expected message and not just assert its type.
                    if (theoryData.ExpectedInnerException is not null)
                        theoryData.ExpectedInnerException.ProcessException(validationError.GetException().InnerException, context);
                }
            }
            catch (Exception ex)
            {
                context.AddDiff($"ValidateTokenAsync threw an unexpected exception: {ex}.");
            }

            TestUtilities.AssertFailIfErrors(context);
        }
    }
}
#nullable restore
