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
