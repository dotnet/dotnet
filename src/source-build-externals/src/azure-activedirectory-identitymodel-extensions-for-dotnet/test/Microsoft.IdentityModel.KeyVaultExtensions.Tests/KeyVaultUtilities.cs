// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.IdentityModel.KeyVaultExtensions.Tests
{
    public static class KeyVaultUtilities
    {
        public static string CreateKeyIdentifier() => CreateKeyIdentifier("contoso.vault.azure.net", nameof(KeyVaultUtilities), $"{Guid.NewGuid():N}");

        public static string CreateKeyIdentifier(string vaultBaseUrl, string vaultKeyName, string vaultKeyVersion)
        {
            return new UriBuilder(Uri.UriSchemeHttps, vaultBaseUrl, -1, $"/keys/{vaultKeyName}/{vaultKeyVersion}").Uri.ToString();
        }
    }
}
