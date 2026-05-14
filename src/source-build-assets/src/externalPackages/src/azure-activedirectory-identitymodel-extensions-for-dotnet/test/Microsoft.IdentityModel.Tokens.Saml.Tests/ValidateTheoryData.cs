// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

namespace Microsoft.IdentityModel.Tokens.Saml.Tests
{
    public class ValidateTheoryData
    {
        public static void AddValidateAudienceTheoryData(List<TokenTheoryData> theoryData)
        {
            theoryData.Add(new TokenTheoryData
            {
                Audiences = new List<string>(),
                First = true,
                ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                TestId = "TokenValidationParameters null",
                ValidationParameters = null,
            });

            theoryData.Add(new TokenTheoryData
            {
                Audiences = new List<string>(),
                TestId = "ValidateAudience = false",
                ValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                },
            });

            theoryData.Add(new TokenTheoryData
            {
                Audiences = new List<string>(),
                ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10208:"),
                TestId = "no audiences in validationParameters",
                ValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                },
            });

            theoryData.Add(new TokenTheoryData
            {
                Audiences = new List<string> { "John" },
                ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10208:"),
                TestId = "audience has value, tvp has no values",
                ValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                },
            });

            theoryData.Add(new TokenTheoryData
            {
                Audiences = new List<string> { "John" },
                ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException("IDX10214:"),
                TestId = "audience not matched",
                ValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudience = "frank"
                },
            });

            theoryData.Add(new TokenTheoryData
            {
                Audiences = new List<string> { "John" },
                TestId = "AudienceValidator returns true",
                ValidationParameters = new TokenValidationParameters
                {
                    AudienceValidator = (aud, token, type) =>
                    {
                        return true;
                    },
                    ValidateAudience = true,
                    ValidAudience = "frank"
                },
            });

            theoryData.Add(new TokenTheoryData
            {
                Audiences = new List<string> { "John" },
                ExpectedException = ExpectedException.SecurityTokenInvalidAudienceException(),
                TestId = "AudienceValidator throws, validateAudience false",
                ValidationParameters = new TokenValidationParameters
                {
                    AudienceValidator = ValidationDelegates.AudienceValidatorThrows,
                    ValidateAudience = false,
                    ValidAudience = "frank"
                },
            });
        }

        public static void AddValidateIssuerTheoryData(List<TokenTheoryData> theoryData)
        {
            theoryData.Add(new TokenTheoryData
            {
                ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                First = true,
                Issuer = "bob",
                TestId = "ValidationParameters null",
                ValidationParameters = null,
            });

            theoryData.Add(new TokenTheoryData
            {
                TestId = "ValidateIssuer == false",
                ValidationParameters = new TokenValidationParameters { ValidateIssuer = false },
            });

            theoryData.Add(new TokenTheoryData
            {
                ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException("IDX10205:"),
                Issuer = "bob",
                TestId = "Issuer not matched",
                ValidationParameters = new TokenValidationParameters { ValidIssuer = "frank" }
            });

            theoryData.Add(new TokenTheoryData
            {
                Issuer = "bob",
                TestId = "Issuer matched",
                ValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidIssuer = "bob"
                }
            });

            theoryData.Add(new TokenTheoryData
            {
                ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException(substringExpected: "IDX10205:"),
                Issuer = "bob",
                TestId = "ValidIssuers set but not matched",
                ValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidIssuers = new List<string> { "john", "paul", "george", "ringo" }
                }
            });

            theoryData.Add(new TokenTheoryData
            {
                Issuer = "bob",
                TestId = "IssuerValidator - echo",
                ValidationParameters = new TokenValidationParameters
                {
                    IssuerValidator = ValidationDelegates.IssuerValidatorEcho,
                    ValidateAudience = false
                }
            });
        }
    }
}
