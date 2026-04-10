// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect.Tests
{
    public class OpenIdConnectTheoryData : TheoryDataBase
    {
        public OpenIdConnectTheoryData()
        {
            SigningCredentials = KeyingMaterial.DefaultX509SigningCreds_2048_RsaSha2_Sha2;
        }

        public OpenIdConnectTheoryData(string testId) : base(testId) { }

        public OpenIdConnectConfiguration CompareTo { get; set; }

        public string Json { get; set; }

        public string OpenIdConnectMetadataFileName { get; set; }

        public SigningCredentials SigningCredentials { get; set; }
    }
}
