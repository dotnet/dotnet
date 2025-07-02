// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens.Saml2;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Saml.Tests
{
    public class CreateTokenTheoryData : TheoryDataBase
    {
        public Dictionary<string, object> AdditionalHeaderClaims { get; set; }

        public string Payload { get; set; }

        public string CompressionAlgorithm { get; set; }

        public CompressionProviderFactory CompressionProviderFactory { get; set; }

        public EncryptingCredentials EncryptingCredentials { get; set; }

        public bool IsValid { get; set; } = true;

        public SigningCredentials SigningCredentials { get; set; }

        public SecurityTokenDescriptor TokenDescriptor { get; set; }

        public SamlSecurityTokenHandler SamlSecurityTokenHandler { get; set; }

        public Saml2SecurityTokenHandler Saml2SecurityTokenHandler { get; set; }

        public string SamlToken { get; set; }

        public TokenValidationParameters ValidationParameters { get; set; }

        public List<string> AudiencesForSecurityTokenDescriptor { get; set; }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
