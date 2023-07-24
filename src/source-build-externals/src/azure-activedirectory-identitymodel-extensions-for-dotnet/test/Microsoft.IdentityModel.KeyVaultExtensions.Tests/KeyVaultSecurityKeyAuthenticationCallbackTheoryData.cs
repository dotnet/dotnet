// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Microsoft.IdentityModel.KeyVaultExtensions.Tests
{
    public class KeyVaultSecurityKeyAuthenticationCallbackTheoryData : KeyVaultSecurityKeyTheoryData
    {
        public KeyVaultSecurityKey.AuthenticationCallback Callback { get; set; } = new KeyVaultSecurityKey.AuthenticationCallback((string authority, string resource, string scope) => Task.FromResult(string.Empty));
    }
}
