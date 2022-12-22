//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

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
