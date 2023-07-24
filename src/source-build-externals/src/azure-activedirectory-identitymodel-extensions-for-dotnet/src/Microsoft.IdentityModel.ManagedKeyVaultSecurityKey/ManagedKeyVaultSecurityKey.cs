// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.IdentityModel.KeyVaultExtensions;
using Microsoft.IdentityModel.Logging;
using System;

namespace Microsoft.IdentityModel.ManagedKeyVaultSecurityKey
{
    /// <summary>
    /// Provides signing and verifying operations using Azure Key Vault
    /// for resources that are using Managed identities for Azure resources.
    /// </summary>
    public class ManagedKeyVaultSecurityKey : KeyVaultSecurityKey
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ManagedKeyVaultSecurityKey"/> class.
        /// </summary>
        /// <param name="keyIdentifier">The key identifier that is recognized by KeyVault.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="keyIdentifier"/> is null or empty.</exception>
        public ManagedKeyVaultSecurityKey(string keyIdentifier)
            : base(keyIdentifier, new AuthenticationCallback((new AzureServiceTokenProvider()).KeyVaultTokenCallback))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagedKeyVaultSecurityKey"/> class.
        /// </summary>
        /// <param name="keyIdentifier">The key identifier that is recognized by KeyVault.</param>
        /// <param name="callback">The authentication callback.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="keyIdentifier"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="callback"/>is null.</exception>
        public ManagedKeyVaultSecurityKey(string keyIdentifier, AuthenticationCallback callback)
            : base(keyIdentifier, callback)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagedKeyVaultSecurityKey"/> class.
        /// </summary>
        /// <param name="keyIdentifier">The key identifier.</param>
        /// <param name="clientId">Identifier of the client.</param>
        /// <param name="clientSecret">Secret of the client identity.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="keyIdentifier"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="clientId"/>is null or empty.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="clientSecret"/>is null or clientSecret.</exception>
        public ManagedKeyVaultSecurityKey(string keyIdentifier, string clientId, string clientSecret)
        {
            if (string.IsNullOrEmpty(keyIdentifier))
                throw LogHelper.LogArgumentNullException(nameof(keyIdentifier));

            if (string.IsNullOrEmpty(clientId))
                throw LogHelper.LogArgumentNullException(nameof(clientId));

            if (string.IsNullOrEmpty(clientSecret))
                throw LogHelper.LogArgumentNullException(nameof(clientSecret));

            KeyId = keyIdentifier;
            Callback = new AuthenticationCallback(async (string authority, string resource, string scope) =>
                (await (new AuthenticationContext(authority, TokenCache.DefaultShared)).AcquireTokenAsync(resource, new ClientCredential(clientId, clientSecret)).ConfigureAwait(false)).AccessToken);
        }
    }
}
