// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Tokens.Tests
{
    public class ValidateTokenTheoryData : TheoryDataBase
    {
        public ValidateTokenTheoryData(string testId) : base(testId)
        { }

        public TokenValidationParameters ValidationParameters { get; set; }

        public TokenHandler TokenHandler { get; set; }

        public string JsonWebToken { get; set; }
    }
}
