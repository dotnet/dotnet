// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.KeyVaultExtensions
{
    /// <summary>
    /// Provides wrap and unwrap operations using Azure Key Vault.
    /// </summary>
    public class KeyVaultKeyWrapProvider : KeyWrapProvider
    {
        private readonly IKeyVaultClient _client;
        private readonly KeyVaultSecurityKey _key;
        private readonly string _algorithm;
        private bool _disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyVaultKeyWrapProvider"/> class.
        /// </summary>
        /// <param name="key">The <see cref="SecurityKey"/> that will be used for key wrap operations.</param>
        /// <param name="algorithm">The key wrap algorithm to apply.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="key"/> is null.</exception>
        /// <exception cref="NotSupportedException">if <paramref name="key"/> is not a <see cref="KeyVaultSecurityKey"/>.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="algorithm"/> is null or empty.</exception>
        public KeyVaultKeyWrapProvider(SecurityKey key, string algorithm)
            : this(key, algorithm, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyVaultKeyWrapProvider"/> class.
        /// </summary>
        /// <param name="key">The <see cref="SecurityKey"/> that will be used for key wrap operations.</param>
        /// <param name="algorithm">The key wrap algorithm to apply.</param>
        /// <param name="client">A mock <see cref="IKeyVaultClient"/> used for testing purposes.</param>
        internal KeyVaultKeyWrapProvider(SecurityKey key, string algorithm, IKeyVaultClient? client)
        {
            _algorithm = string.IsNullOrEmpty(algorithm) ? throw LogHelper.LogArgumentNullException(nameof(algorithm)) : algorithm;
            if (key == null)
                throw LogHelper.LogArgumentNullException(nameof(key));

            _key = key as KeyVaultSecurityKey ?? throw LogHelper.LogExceptionMessage(new NotSupportedException(key.GetType().ToString()));
            _client = client ?? new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(_key.Callback!));
        }

        /// <summary>
        /// Gets the KeyWrap algorithm that is being used.
        /// </summary>
        public override string Algorithm => _algorithm;

        /// <summary>
        /// Gets or sets a user context for a <see cref="KeyWrapProvider"/>.
        /// </summary>
        /// <remarks>This is null by default. This can be used by runtimes or for extensibility scenarios.</remarks>
        public override string? Context { get; set; }

        /// <summary>
        /// Gets the <see cref="SecurityKey"/> that is being used.
        /// </summary>
        public override SecurityKey Key => _key;

        /// <summary>
        /// Unwrap a key.
        /// </summary>
        /// <param name="keyBytes">key to unwrap.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="keyBytes"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="keyBytes"/>.Length == 0.</exception>
        /// <returns>Unwrapped key.</returns>
        public override byte[] UnwrapKey(byte[] keyBytes)
        {
            return UnwrapKeyAsync(keyBytes, CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Wrap a key.
        /// </summary>
        /// <param name="keyBytes">the key to be wrapped</param>
        /// <exception cref="ArgumentNullException">if <paramref name="keyBytes"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="keyBytes"/>.Length == 0.</exception>
        /// <returns>wrapped key.</returns>
        public override byte[] WrapKey(byte[] keyBytes)
        {
            return WrapKeyAsync(keyBytes, CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">true, if called from Dispose(), false, if invoked inside a finalizer</param>
        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _disposed = true;
                    _client.Dispose();
                }
            }
        }

        /// <summary>
        /// Unwraps a symmetric key using Azure Key Vault.
        /// </summary>
        /// <param name="keyBytes">key to unwrap.</param>
        /// <param name="cancellation">Propagates notification that operations should be canceled.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="keyBytes"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="keyBytes"/>.Length == 0.</exception>
        /// <returns>Unwrapped key.</returns>
        private async Task<byte[]> UnwrapKeyAsync(byte[] keyBytes, CancellationToken cancellation)
        {
            if (keyBytes == null || keyBytes.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(keyBytes));

            return (await _client.UnwrapKeyAsync(_key.KeyId, Algorithm, keyBytes, cancellation).ConfigureAwait(false)).Result;
        }

        /// <summary>
        /// Wraps a symmetric key using Azure Key Vault.
        /// </summary>
        /// <param name="keyBytes">the key to be wrapped</param>
        /// <param name="cancellation">Propagates notification that operations should be canceled.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="keyBytes"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="keyBytes"/>.Length == 0.</exception>
        /// <returns>wrapped key.</returns>
        private async Task<byte[]> WrapKeyAsync(byte[] keyBytes, CancellationToken cancellation)
        {
            if (keyBytes == null || keyBytes.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(keyBytes));

            return (await _client.WrapKeyAsync(_key.KeyId, Algorithm, keyBytes, cancellation).ConfigureAwait(false)).Result;
        }
    }
}
