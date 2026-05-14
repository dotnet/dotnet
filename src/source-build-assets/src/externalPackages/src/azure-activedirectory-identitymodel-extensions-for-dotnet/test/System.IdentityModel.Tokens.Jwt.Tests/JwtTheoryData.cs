// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;

namespace System.IdentityModel.Tokens.Jwt.Tests
{
    public class JwtTheoryData : TheoryDataBase
    {
        public JwtTheoryData(string testId) : base(testId)
        { }

        public JwtTheoryData() { }

        public string Actor { get; set; }

        public TokenValidationParameters ActorTokenValidationParameters { get; set; }

        public bool CanRead { get; set; } = true;

        public SecurityToken SecurityToken { get; set; }

        public string Token { get; set; }

        public SecurityTokenDescriptor TokenDescriptor { get; set; }

        public JwtSecurityTokenHandler TokenHandler { get; set; } = new JwtSecurityTokenHandler();

        public TokenType TokenType { get; set; }

        public TokenValidationParameters ValidationParameters { get; set; }

        public string TokenTypeHeader { get; set; }

        public bool ShouldSetLastKnownConfiguration { get; set; }

        public bool SetupIssuerLkg { get; set; } = false;

        public BaseConfigurationManager SetupIssuerLkgConfigurationManager { get; set; }
    }
}
