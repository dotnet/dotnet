// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Provides Wrap key and Unwrap key services.
    /// </summary>
    public class SymmetricKeyWrapProvider : KeyWrapProvider
    {
        private static readonly byte[] _defaultIV = new byte[] { 0xA6, 0xA6, 0xA6, 0xA6, 0xA6, 0xA6, 0xA6, 0xA6 };
        private const int _blockSizeInBits = 64;
        private const int _blockSizeInBytes = _blockSizeInBits >> 3;
        private static readonly object _encryptorLock = new object();
        private static readonly object _decryptorLock = new object();

        private Lazy<SymmetricAlgorithm> _symmetricAlgorithm;
        private ICryptoTransform _symmetricAlgorithmEncryptor;
        private ICryptoTransform _symmetricAlgorithmDecryptor;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyWrapProvider"/> class used for wrap key and unwrap key.
        /// <param name="key">The <see cref="SecurityKey"/> that will be used for crypto operations.</param>
        /// <param name="algorithm">The KeyWrap algorithm to apply.</param>
        /// <exception cref="ArgumentNullException">'key' is null.</exception>
        /// <exception cref="ArgumentNullException">'algorithm' is null.</exception>
        /// <exception cref="ArgumentException">If <see cref="SecurityKey"/> and algorithm pair are not supported.</exception>
        /// <exception cref="ArgumentException">The <see cref="SecurityKey"/> cannot be converted to byte array</exception>
        /// <exception cref="ArgumentOutOfRangeException">The keysize doesn't match the algorithm.</exception>
        /// <exception cref="InvalidOperationException">Failed to create symmetric algorithm with provided key and algorithm.</exception>
        /// </summary>
        public SymmetricKeyWrapProvider(SecurityKey key, string algorithm)
        {
            if (key == null)
                throw LogHelper.LogArgumentNullException(nameof(key));

            if (string.IsNullOrEmpty(algorithm))
                throw LogHelper.LogArgumentNullException(nameof(algorithm));

            Algorithm = algorithm;
            Key = key;

            _symmetricAlgorithm = new Lazy<SymmetricAlgorithm>(CreateSymmetricAlgorithm);
        }

        /// <summary>
        /// Gets the KeyWrap algorithm that is being used.
        /// </summary>
        public override string Algorithm { get; }

        /// <summary>
        /// Gets or sets a user context for a <see cref="KeyWrapProvider"/>.
        /// </summary>
        /// <remarks>This is null by default. This can be used by runtimes or for extensibility scenarios.</remarks>
        public override string Context { get; set; }

        /// <summary>
        /// Gets the <see cref="SecurityKey"/> that is being used.
        /// </summary>
        public override SecurityKey Key { get; }

        private SymmetricAlgorithm CreateSymmetricAlgorithm()
        {
            if (!IsSupportedAlgorithm(Key, Algorithm))
                throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant(LogMessages.IDX10661, LogHelper.MarkAsNonPII(Algorithm), Key)));

            SymmetricAlgorithm symmetricAlgorithm = GetSymmetricAlgorithm(Key, Algorithm);

            if (symmetricAlgorithm == null)
                throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant(LogMessages.IDX10669)));

            return symmetricAlgorithm;
        }

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
                    if (_symmetricAlgorithm != null)
                    {
                        _symmetricAlgorithm.Value.Dispose();
                        _symmetricAlgorithm = null;
                    }

                    if (_symmetricAlgorithmEncryptor != null)
                    {
                        _symmetricAlgorithmEncryptor.Dispose();
                        _symmetricAlgorithmEncryptor = null;
                    }

                    if (_symmetricAlgorithmDecryptor != null)
                    {
                        _symmetricAlgorithmDecryptor.Dispose();
                        _symmetricAlgorithmDecryptor = null;
                    }

                    _disposed = true;
                }
            }
        }

        private static byte[] GetBytes(ulong i)
        {
            byte[] temp = BitConverter.GetBytes(i);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(temp);
            }

            return temp;
        }

        /// <summary>
        /// Returns the <see cref="SymmetricAlgorithm"/>.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException">The <see cref="SecurityKey"/> cannot be converted to byte array</exception>
        /// <exception cref="ArgumentOutOfRangeException">The keysize doesn't match the algorithm.</exception>
        /// <exception cref="InvalidOperationException">Failed to create symmetric algorithm with provided key and algorithm.</exception>
        protected virtual SymmetricAlgorithm GetSymmetricAlgorithm(SecurityKey key, string algorithm)
        {
            if (key == null)
                throw LogHelper.LogArgumentNullException(nameof(key));

            if (!IsSupportedAlgorithm(key, algorithm))
                throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant(LogMessages.IDX10661, LogHelper.MarkAsNonPII(algorithm), key)));

            byte[] keyBytes = null;

            if (key is SymmetricSecurityKey symmetricSecurityKey)
                keyBytes = symmetricSecurityKey.Key;
            else if (key is JsonWebKey jsonWebKey)
            {
                if (JsonWebKeyConverter.TryConvertToSymmetricSecurityKey(jsonWebKey, out SecurityKey securityKey))
                    keyBytes = (securityKey as SymmetricSecurityKey).Key;
            }

            if (keyBytes == null)
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10657, LogHelper.MarkAsNonPII(key.GetType()))));

            ValidateKeySize(keyBytes, algorithm);

            try
            {
                // Create the AES provider
                SymmetricAlgorithm symmetricAlgorithm = Aes.Create();
                symmetricAlgorithm.Mode = CipherMode.ECB;
                symmetricAlgorithm.Padding = PaddingMode.None;
                symmetricAlgorithm.KeySize = keyBytes.Length * 8;
                symmetricAlgorithm.Key = keyBytes;

                // Set the AES IV to Zeroes
                var aesIv = new byte[symmetricAlgorithm.BlockSize >> 3];
                Utility.Zero(aesIv);
                symmetricAlgorithm.IV = aesIv;

                return symmetricAlgorithm;
            }
            catch (Exception ex)
            {
                throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant(LogMessages.IDX10663, key, LogHelper.MarkAsNonPII(algorithm)), ex));
            }
        }

        /// <summary>
        /// Answers if an algorithm is supported
        /// </summary>
        /// <param name="key">the <see cref="SecurityKey"/></param>
        /// <param name="algorithm">the algorithm to use</param>
        /// <returns>true if the algorithm is supported; otherwise, false.</returns>
        protected virtual bool IsSupportedAlgorithm(SecurityKey key, string algorithm)
        {
            return SupportedAlgorithms.IsSupportedSymmetricKeyWrap(algorithm, key);
        }

        /// <summary>
        /// Unwrap a key using Symmetric decryption.
        /// </summary>
        /// <param name="keyBytes">bytes to unwrap</param>
        /// <returns>Unwraped key</returns>
        /// <exception cref="ArgumentNullException">'keyBytes' is null or length == 0.</exception>
        /// <exception cref="ArgumentException">'keyBytes' is not a multiple of 8.</exception>
        /// <exception cref="ObjectDisposedException">If <see cref="KeyWrapProvider.Dispose(bool)"/> has been called.</exception>
        /// <exception cref="SecurityTokenKeyWrapException">Failed to unwrap the wrappedKey.</exception>
        public override byte[] UnwrapKey(byte[] keyBytes)
        {
            if (keyBytes == null || keyBytes.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(keyBytes));

            if (keyBytes.Length % 8 != 0)
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10664, LogHelper.MarkAsNonPII(keyBytes.Length << 3)), nameof(keyBytes)));

            if (_disposed)
                throw LogHelper.LogExceptionMessage(new ObjectDisposedException(GetType().ToString()));

            try
            {
                return UnwrapKeyPrivate(keyBytes, 0, keyBytes.Length);
            }
            catch (Exception ex)
            {
                throw LogHelper.LogExceptionMessage(new SecurityTokenKeyWrapException(LogHelper.FormatInvariant(LogMessages.IDX10659, ex)));
            }
        }

        private byte[] UnwrapKeyPrivate(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            /*
                1) Initialize variables.

                    Set A = C[0]
                    For i = 1 to n
                        R[i] = C[i]

                2) Compute intermediate values.

                    For j = 5 to 0
                        For i = n to 1
                            B = AES-1(K, (A ^ t) | R[i]) where t = n*j+i
                            A = MSB(64, B)
                            R[i] = LSB(64, B)

                3) Output results.

                If A is an appropriate initial value (see 2.2.3),
                Then
                    For i = 1 to n
                        P[i] = R[i]
                Else
                    Return an error
            */

            // A = C[0]
            byte[] a = new byte[_blockSizeInBytes];

            Array.Copy(inputBuffer, inputOffset, a, 0, _blockSizeInBytes);

            // The number of input blocks
            var n = (inputCount - _blockSizeInBytes) >> 3;

            // The set of input blocks
            byte[] r = new byte[n << 3];

            Array.Copy(inputBuffer, inputOffset + _blockSizeInBytes, r, 0, inputCount - _blockSizeInBytes);

            if (_symmetricAlgorithmDecryptor == null)
            {
                lock (_decryptorLock)
                {
                    if (_symmetricAlgorithmDecryptor == null)
                        _symmetricAlgorithmDecryptor = _symmetricAlgorithm.Value.CreateDecryptor();
                }
            }

            byte[] block = new byte[16];

            // Calculate intermediate values
            for (var j = 5; j >= 0; j--)
            {
                for (var i = n; i > 0; i--)
                {
                    // T = ( n * j ) + i
                    var t = (ulong)((n * j) + i);

                    // B = AES-1(K, (A ^ t) | R[i] )

                    // First, A = ( A ^ t )
                    Utility.Xor(a, GetBytes(t), 0, true);

                    // Second, block = ( A | R[i] )
                    Array.Copy(a, block, _blockSizeInBytes);
                    Array.Copy(r, (i - 1) << 3, block, _blockSizeInBytes, _blockSizeInBytes);

                    // Third, b = AES-1( block )
                    var b = _symmetricAlgorithmDecryptor.TransformFinalBlock(block, 0, 16);

                    // A = MSB(64, B)
                    Array.Copy(b, a, _blockSizeInBytes);

                    // R[i] = LSB(64, B)
                    Array.Copy(b, _blockSizeInBytes, r, (i - 1) << 3, _blockSizeInBytes);
                }
            }

           if (Utility.AreEqual(a, _defaultIV))
            {
                var keyBytes = new byte[n << 3];

                for (var i = 0; i < n; i++)
                {
                    Array.Copy(r, i << 3, keyBytes, i << 3, 8);
                }

                return keyBytes;
            }
            else
            {
                throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogMessages.IDX10665));
            }
        }

        private void ValidateKeySize(byte[] key, string algorithm)
        {
            if (SecurityAlgorithms.Aes128KW.Equals(algorithm) || SecurityAlgorithms.Aes128KeyWrap.Equals(algorithm))
            {
                if (key.Length != 16)
                    throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(nameof(key), LogHelper.FormatInvariant(LogMessages.IDX10662, LogHelper.MarkAsNonPII(algorithm), LogHelper.MarkAsNonPII(128), Key.KeyId, LogHelper.MarkAsNonPII(key.Length << 3))));

                return;
            }

            if (SecurityAlgorithms.Aes192KW.Equals(algorithm) || SecurityAlgorithms.Aes192KeyWrap.Equals(algorithm))
            {
                if (key.Length != 24)
                    throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(nameof(key), LogHelper.FormatInvariant(LogMessages.IDX10662, LogHelper.MarkAsNonPII(algorithm), LogHelper.MarkAsNonPII(128), Key.KeyId, LogHelper.MarkAsNonPII(key.Length << 3))));

                return;
            }

            if (SecurityAlgorithms.Aes256KW.Equals(algorithm) || (SecurityAlgorithms.Aes256KeyWrap.Equals(algorithm)))
            {
                if (key.Length != 32)
                    throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(nameof(key), LogHelper.FormatInvariant(LogMessages.IDX10662, LogHelper.MarkAsNonPII(algorithm), LogHelper.MarkAsNonPII(256), Key.KeyId, LogHelper.MarkAsNonPII(key.Length << 3))));

                return;
            }

            throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(nameof(algorithm), LogHelper.FormatInvariant(LogMessages.IDX10652, LogHelper.MarkAsNonPII(algorithm))));
        }

        /// <summary>
        /// Wrap a key using Symmetric encryption.
        /// </summary>
        /// <param name="keyBytes">the key to be wrapped</param>
        /// <returns>The wrapped key result</returns>
        /// <exception cref="ArgumentNullException">'keyBytes' is null or has length 0.</exception>
        /// <exception cref="ArgumentException">'keyBytes' is not a multiple of 8.</exception>
        /// <exception cref="ObjectDisposedException">If <see cref="KeyWrapProvider.Dispose(bool)"/> has been called.</exception>
        /// <exception cref="SecurityTokenKeyWrapException">Failed to wrap 'keyBytes'.</exception>
        public override byte[] WrapKey(byte[] keyBytes)
        {
            if (keyBytes == null || keyBytes.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(keyBytes));

            if (keyBytes.Length % 8 != 0)
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10664, LogHelper.MarkAsNonPII(keyBytes.Length << 3)), nameof(keyBytes)));

            if (_disposed)
                throw LogHelper.LogExceptionMessage(new ObjectDisposedException(GetType().ToString()));

            try
            {
                return WrapKeyPrivate(keyBytes, 0, keyBytes.Length);
            }
            catch (Exception ex)
            {
                throw LogHelper.LogExceptionMessage(new SecurityTokenKeyWrapException(LogHelper.FormatInvariant(LogMessages.IDX10658, ex)));
            }
        }

        private byte[] WrapKeyPrivate(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            /*
               1) Initialize variables.

                   Set A = IV, an initial value (see 2.2.3)
                   For i = 1 to n
                       R[i] = P[i]

               2) Calculate intermediate values.

                   For j = 0 to 5
                       For i=1 to n
                           B = AES(K, A | R[i])
                           A = MSB(64, B) ^ t where t = (n*j)+i
                           R[i] = LSB(64, B)

               3) Output the results.

                   Set C[0] = A
                   For i = 1 to n
                       C[i] = R[i]
            */

            // The default initialization vector from RFC3394
            byte[] a = _defaultIV.Clone() as byte[];

            // The number of input blocks
            var n = inputCount >> 3;

            // The set of input blocks
            byte[] r = new byte[n << 3];

            Array.Copy(inputBuffer, inputOffset, r, 0, inputCount);

            if (_symmetricAlgorithmEncryptor == null)
            {
                lock (_encryptorLock)
                {
                    if (_symmetricAlgorithmEncryptor == null)
                        _symmetricAlgorithmEncryptor = _symmetricAlgorithm.Value.CreateEncryptor();
                }
            }

            byte[] block = new byte[16];

            // Calculate intermediate values
            for (var j = 0; j < 6; j++)
            {
                for (var i = 0; i < n; i++)
                {
                    // T = ( n * j ) + i
                    var t = (ulong)((n * j) + i + 1);

                    // B = AES( K, A | R[i] )

                    // First, block = A | R[i]
                    Array.Copy(a, block, a.Length);
                    Array.Copy(r, i << 3, block, 64 >> 3, 64 >> 3);

                    // Second, AES( K, block )
                    var b = _symmetricAlgorithmEncryptor.TransformFinalBlock(block, 0, 16);

                    // A = MSB( 64, B )
                    Array.Copy(b, a, 64 >> 3);

                    // A = A ^ t
                    Utility.Xor(a, GetBytes(t), 0, true);

                    // R[i] = LSB( 64, B )
                    Array.Copy(b, 64 >> 3, r, i << 3, 64 >> 3);
                }
            }

            var keyBytes = new byte[(n + 1) << 3];

            Array.Copy(a, keyBytes, a.Length);

            for (var i = 0; i < n; i++)
            {
                Array.Copy(r, i << 3, keyBytes, (i + 1) << 3, 8);
            }

            return keyBytes;
        }
    }
}
