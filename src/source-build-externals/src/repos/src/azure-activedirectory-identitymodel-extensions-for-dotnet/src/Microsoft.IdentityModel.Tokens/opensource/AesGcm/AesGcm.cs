// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.IdentityModel.Tokens
{
    internal class AesGcm : IDisposable
    {
        public const int NonceSize = 12;
        public const int TagSize = 16;

        private static readonly SafeAlgorithmHandle s_aesGcm = AesBCryptModes.OpenAesAlgorithm(Cng.BCRYPT_CHAIN_MODE_GCM).Value;
        private SafeKeyHandle _keyHandle;
        private bool _disposed = false;

        public AesGcm(byte[] key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            ImportKey(key);
        }

        private void ImportKey(byte[] key)
        {
            _keyHandle = Interop.BCrypt.BCryptImportKey(s_aesGcm, key);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _disposed = true;
                _keyHandle.Dispose();
            }
        }

        public void Decrypt(byte[] nonce, byte[] ciphertext, byte[] tag, byte[] plaintext, byte[] associatedData = null)
        {
            AesAead.CheckArgumentsForNull(nonce, plaintext, ciphertext, tag);
            AesAead.Decrypt(_keyHandle, nonce, associatedData, ciphertext, tag, plaintext, clearPlaintextOnFailure: true);
        }

#region FOR TESTING ONLY
        internal void Encrypt(byte[] nonce, byte[] plaintext, byte[] ciphertext, byte[] tag, byte[] associatedData = null)
        {
            AesAead.CheckArgumentsForNull(nonce, plaintext, ciphertext, tag);
            AesAead.Encrypt(_keyHandle, nonce, associatedData, plaintext, ciphertext, tag);
        }
#endregion
    }
}
