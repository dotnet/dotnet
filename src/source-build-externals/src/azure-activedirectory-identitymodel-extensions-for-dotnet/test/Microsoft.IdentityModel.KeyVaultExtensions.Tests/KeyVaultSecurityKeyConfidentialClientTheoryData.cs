// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.IdentityModel.KeyVaultExtensions.Tests
{
    public class KeyVaultSecurityKeyConfidentialClientTheoryData : KeyVaultSecurityKeyTheoryData
    {
        public string ClientId { get; set; } = $"{Guid.NewGuid():D}";
        public string ClientSecret { get; set; } = Guid.NewGuid().ToString();
    }
}
