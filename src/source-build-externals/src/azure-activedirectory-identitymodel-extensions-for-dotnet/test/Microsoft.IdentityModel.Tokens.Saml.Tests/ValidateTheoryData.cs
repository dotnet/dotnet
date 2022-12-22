//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

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
