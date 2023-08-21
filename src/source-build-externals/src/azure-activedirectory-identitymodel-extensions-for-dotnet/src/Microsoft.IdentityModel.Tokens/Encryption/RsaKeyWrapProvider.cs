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
        /// Initializes a new instance of <see cref="RsaKeyWrapProvider"/> used for wrapping and un-wrappping keys.
        /// These keys are usually symmetric session keys that are wrapped using the recipients public key.
        /// <param name="key">The <see cref="SecurityKey"/> that will be used for cryptographic operations.</param>
        /// <param name="algorithm">The KeyWrap algorithm to apply.</param>
        /// <param name="willUnwrap">Whether this <see cref="RsaKeyWrapProvider"/> is required to un-wrap keys. If true, the private key is required.</param>
        /// <exception cref="ArgumentNullException">'key' is null.</exception>
        /// <exception cref="ArgumentNullException">'algorithm' is null.</exception>
        /// <exception cref="ArgumentException">The key size doesn't match the algorithm.</exception>
        /// <exception cref="ArgumentException">If <see cref="SecurityKey"/> and algorithm pair are not supported.</exception>
        /// <exception cref="NotSupportedException">Failed to create RSA algorithm with provided key and algorithm.</exception>
        /// </summary>
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
        /// Disposes of internal components.
        /// </summary>
        /// <param name="disposing">true, if called from Dispose(), false, if invoked inside a finalizer.</param>
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
        /// Checks if an algorithm is supported.
        /// </summary>
        /// <param name="key">The <see cref="SecurityKey"/> that will be used for crypto operations.</param>
        /// <param name="algorithm">The KeyWrap algorithm to apply.</param>
        /// <returns>true if the algorithm is supported; otherwise, false.</returns>
        protected virtual bool IsSupportedAlgorithm(SecurityKey key, string algorithm)
        {
            return SupportedAlgorithms.IsSupportedRsaKeyWrap(algorithm, key);
        }

        /// <summary>
        /// Unwrap a key using RSA decryption.
        /// </summary>
        /// <param name="keyBytes">the bytes to unwrap.</param>
        /// <returns>Unwrapped key</returns>
        /// <exception cref="ArgumentNullException">'keyBytes' is null or length == 0.</exception>
        /// <exception cref="ObjectDisposedException">If <see cref="RsaKeyWrapProvider.Dispose(bool)"/> has been called.</exception>
        /// <exception cref="SecurityTokenKeyWrapException">Failed to unwrap the wrappedKey.</exception>
        /// <exception cref="InvalidOperationException">If the internal RSA algorithm is null.</exception>
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
        /// Wrap a key using RSA encryption.
        /// </summary>
        /// <param name="keyBytes">the key to be wrapped</param>
        /// <returns>A wrapped key</returns>
        /// <exception cref="ArgumentNullException">'keyBytes' is null or has length == 0.</exception>
        /// <exception cref="ObjectDisposedException">If <see cref="RsaKeyWrapProvider.Dispose(bool)"/> has been called.</exception>
        /// <exception cref="SecurityTokenKeyWrapException">Failed to wrap the 'keyBytes'.</exception>
        /// <exception cref="InvalidOperationException">If the internal RSA algorithm is null.</exception>
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
