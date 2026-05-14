// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

namespace Microsoft.IdentityModel.Tokens.Validation.Tests
{
    public class AsyncValidatorTests
    {
        [Theory, MemberData(nameof(AsyncIssuerValidatorTestCases))]
        public async Task AsyncIssuerValidatorTests(IssuerValidatorTheoryData theoryData)
        {
            CompareContext context = TestUtilities.WriteHeader($"{this}.AsyncIssuerValidatorTests", theoryData);
            try
            {
                IssuerValidationResult result = await Validators.ValidateIssuerAsync(
                        theoryData.Issuer,
                        theoryData.SecurityToken,
                        theoryData.ValidationParameters,
                        null,
                        CancellationToken.None).ConfigureAwait(false);
                Exception exception = result.Exception;
                context.Diffs.Add("Exception: " + exception.ToString());
            }
            catch (Exception ex)
            {
                context.Diffs.Add("Exception: " + ex.ToString());
            }
        }

        public static TheoryData<IssuerValidatorTheoryData> AsyncIssuerValidatorTestCases
        {
            get
            {
                TheoryData<IssuerValidatorTheoryData> theoryData = new TheoryData<IssuerValidatorTheoryData>();

                theoryData.Add(new IssuerValidatorTheoryData
                {
                    Issuer = null,
                    ValidationParameters = new TokenValidationParameters(),
                });

                return theoryData;
            }
        }
    }

    public class IssuerValidatorTheoryData : TheoryDataBase
    {
        public string Issuer { get; set; }
        public TokenValidationParameters ValidationParameters { get; set; }
        public SecurityToken SecurityToken { get; set; }
    }
}
