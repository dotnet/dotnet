// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Provides RSA Wrap key and Unwrap key services.
    /// </summary>
    public class RsaKeyWrapProvider : KeyWrapProvider
    {
        private Lazy<AsymmetricAdapter> _asymmetricAdapter;
        private bool _disposed = false;
        private bool _willUnwrap;

        /// <summary>
        /// Initializes a new instance of the <see cref="RsaKeyWrapProvider"/> class used for wrapping and unwrapping keys.
        /// These keys are usually symmetric session keys that are wrapped using the recipient's public key.
        /// </summary>
        /// <param name="key">The <see cref="SecurityKey"/> that will be used for cryptographic operations.</param>
        /// <param name="algorithm">The KeyWrap algorithm to be used.</param>
        /// <param name="willUnwrap">Whether this <see cref="RsaKeyWrapProvider"/> is required to unwrap keys. If true, the private key is required.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="key"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="algorithm"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the key size doesn't match the algorithm.</exception>
        /// <exception cref="ArgumentException">Thrown if the <see cref="SecurityKey"/> and <paramref name="algorithm"/> pair are not supported.</exception>
        /// <exception cref="NotSupportedException">Thrown if failed to create RSA algorithm with the provided key and algorithm.</exception>
        public RsaKeyWrapProvider(SecurityKey key, string algorithm, bool willUnwrap)
        {
            if (key == null)
                throw LogHelper.LogArgumentNullException(nameof(key));

            if (string.IsNullOrEmpty(algorithm))
                throw LogHelper.LogArgumentNullException(nameof(algorithm));

            Algorithm = algorithm;
            Key = key;
            _willUnwrap = willUnwrap;
            _asymmetricAdapter = new Lazy<AsymmetricAdapter>(CreateAsymmetricAdapter);
        }

        internal AsymmetricAdapter CreateAsymmetricAdapter()
        {
            if (!IsSupportedAlgorithm(Key, Algorithm))
                throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant(LogMessages.IDX10661, LogHelper.MarkAsNonPII(Algorithm), Key)));

            return new AsymmetricAdapter(Key, Algorithm, _willUnwrap);
        }

        /// <summary>
        /// Gets the KeyWrap algorithm that is being used.
        /// </summary>
        public override string Algorithm { get; }

        /// <summary>
        /// Gets or sets a user context for a <see cref="KeyWrapProvider"/>.
        /// </summary>
        /// <remarks>This is null by default. This is for use by the application and not used by this SDK.</remarks>
        public override string Context { get; set; }

        /// <summary>
        /// Gets the <see cref="SecurityKey"/> that is being used.
        /// </summary>
        public override SecurityKey Key { get; }

        /// <summary>
        /// Releases the resources used by the current instance.
        /// </summary>
        /// <param name="disposing">If true, release both managed and unmanaged resources; otherwise, release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _disposed = true;
                    _asymmetricAdapter.Value.Dispose();
                }
            }
        }

        /// <summary>
        /// Checks if the specified algorithm is supported.
        /// </summary>
        /// <param name="key">The <see cref="SecurityKey"/> that will be used for cryptographic operations.</param>
        /// <param name="algorithm">The KeyWrap algorithm to be used.</param>
        /// <returns>true if the algorithm is supported; otherwise, false.</returns>
        protected virtual bool IsSupportedAlgorithm(SecurityKey key, string algorithm)
        {
            return SupportedAlgorithms.IsSupportedRsaKeyWrap(algorithm, key);
        }

        /// <summary>
        /// Unwraps a key using RSA decryption.
        /// </summary>
        /// <param name="keyBytes">The bytes to unwrap.</param>
        /// <returns>The unwrapped key.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="keyBytes"/> is null or has a length of 0.</exception>
        /// <exception cref="ObjectDisposedException">Thrown if <see cref="RsaKeyWrapProvider.Dispose(bool)"/> has been called.</exception>
        /// <exception cref="SecurityTokenKeyWrapException">Thrown if the key unwrapping operation fails.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the internal RSA algorithm is null.</exception>
        public override byte[] UnwrapKey(byte[] keyBytes)
        {
            if (keyBytes == null || keyBytes.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(keyBytes));

            if (_disposed)
                throw LogHelper.LogExceptionMessage(new ObjectDisposedException(GetType().ToString()));

            try
            {
                return _asymmetricAdapter.Value.Decrypt(keyBytes);
            }
            catch (Exception ex)
            {
                throw LogHelper.LogExceptionMessage(new SecurityTokenKeyWrapException(LogHelper.FormatInvariant(LogMessages.IDX10659, ex)));
            }
        }

        /// <summary>
        /// Wraps a key using RSA encryption.
        /// </summary>
        /// <param name="keyBytes">The key to be wrapped.</param>
        /// <returns>A wrapped key.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="keyBytes"/> is null or has a length of 0.</exception>
        /// <exception cref="ObjectDisposedException">Thrown if <see cref="RsaKeyWrapProvider.Dispose(bool)"/> has been called.</exception>
        /// <exception cref="SecurityTokenKeyWrapException">Thrown if the key wrapping operation fails.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the internal RSA algorithm is null.</exception>
        public override byte[] WrapKey(byte[] keyBytes)
        {
            if (keyBytes == null || keyBytes.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(keyBytes));

            if (_disposed)
                throw LogHelper.LogExceptionMessage(new ObjectDisposedException(GetType().ToString()));

            try
            {
                return _asymmetricAdapter.Value.Encrypt(keyBytes);
            }
            catch (Exception ex)
            {
                throw LogHelper.LogExceptionMessage(new SecurityTokenKeyWrapException(LogHelper.FormatInvariant(LogMessages.IDX10658, ex)));
            }
        }
    }
}
