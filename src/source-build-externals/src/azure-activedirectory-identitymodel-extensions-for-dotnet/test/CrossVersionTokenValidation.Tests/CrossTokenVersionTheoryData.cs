// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.IdentityModel.Tokens.Saml2;
using Microsoft.IdentityModel.Tokens.Saml;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;

using SecurityTokenDescriptor4x = System.IdentityModel.Tokens.SecurityTokenDescriptor;
using TokenValidationParameters4x = System.IdentityModel.Tokens.TokenValidationParameters;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.CrossVersionTokenValidation.Tests
{
    public class CrossTokenVersionTheoryData : TheoryDataBase
    {
        public Tokens.Saml2.AuthenticationInformation AuthenticationInformationSaml2 { get; set; }

        public Tokens.Saml.AuthenticationInformation AuthenticationInformationSaml { get; set; }

        public string TokenString4x { get; set; }

        public string TokenString5x { get; set; }

        public SecurityTokenDescriptor4x TokenDescriptor4x { get; set; }

        public SecurityTokenDescriptor TokenDescriptor5x { get; set; }

        public TokenValidationParameters4x ValidationParameters4x { get; set; }

        public TokenValidationParameters ValidationParameters5x { get; set; }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
