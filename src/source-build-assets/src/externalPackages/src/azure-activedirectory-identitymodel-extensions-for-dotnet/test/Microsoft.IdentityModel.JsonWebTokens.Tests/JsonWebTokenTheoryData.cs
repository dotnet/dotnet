// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.JsonWebTokens.Tests
{
    public class JsonWebTokenTheoryData : TheoryDataBase
    {
        public TokenValidationParameters ValidationParameters { get; set; }

        public JsonWebTokenHandler TokenHandler { get; set; }

        public string AccessToken { get; set; }
    }
}
