// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Provides signing and verifying operations using a <see cref="SymmetricSecurityKey"/> and specifying an algorithm.
    /// </summary>
    public class SymmetricSignatureProvider : SignatureProvider
    {
        private bool _disposed;
        private DisposableObjectPool<KeyedHashAlgorithm> _keyedHashObjectPool;

        /// <summary>
        /// Mapping from algorithm to the expected signature size in bytes.
        /// </summary>
        internal static readonly Dictionary<string, int> ExpectedSignatureSizeInBytes = new Dictionary<string, int>
        {
            { SecurityAlgorithms.HmacSha256, 32 },
            { SecurityAlgorithms.HmacSha256Signature, 32 },
            { SecurityAlgorithms.HmacSha384, 48 },
            { SecurityAlgorithms.HmacSha384Signature, 48 },
            { SecurityAlgorithms.HmacSha512, 64 },
            { SecurityAlgorithms.HmacSha512Signature, 64 },
            { SecurityAlgorithms.Aes128CbcHmacSha256, 16 },
            { SecurityAlgorithms.Aes192CbcHmacSha384, 24 },
            { SecurityAlgorithms.Aes256CbcHmacSha512, 32 }
        };

        /// <summary>
        /// This is the minimum <see cref="SymmetricSecurityKey"/>.KeySize when creating and verifying signatures.
        /// </summary>
        public static readonly int DefaultMinimumSymmetricKeySizeInBits = 128;

        private int _minimumSymmetricKeySizeInBits = DefaultMinimumSymmetricKeySizeInBits;

        /// <summary>
        /// Initializes a new instance of the <see cref="SymmetricSignatureProvider"/> class that uses an <see cref="SecurityKey"/> to create and / or verify signatures over a array of bytes.
        /// </summary>
        /// <param name="key">The <see cref="SecurityKey"/> that will be used for signature operations.</param>
        /// <param name="algorithm">The signature algorithm to use.</param>
        /// <exception cref="ArgumentNullException">'key' is null.</exception>
        /// <exception cref="ArgumentNullException">'algorithm' is null or empty.</exception>
        /// <exception cref="NotSupportedException">If <see cref="SecurityKey"/> and algorithm pair are not supported.</exception>
        /// <exception cref="ArgumentOutOfRangeException">'<see cref="SecurityKey"/>.KeySize' is smaller than <see cref="SymmetricSignatureProvider.MinimumSymmetricKeySizeInBits"/>.</exception>
        public SymmetricSignatureProvider(SecurityKey key, string algorithm)
            : this(key, algorithm, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SymmetricSignatureProvider"/> class that uses an <see cref="SecurityKey"/> to create and / or verify signatures over a array of bytes.
        /// </summary>
        /// <param name="key">The <see cref="SecurityKey"/> that will be used for signature operations.</param>
        /// <param name="algorithm">The signature algorithm to use.</param>
        /// <param name="willCreateSignatures">indicates if this <see cref="SymmetricSignatureProvider"/> will be used to create signatures.</param>
        /// <exception cref="ArgumentNullException">'key' is null.</exception>
        /// <exception cref="ArgumentNullException">'algorithm' is null or empty.</exception>
        /// <exception cref="NotSupportedException">If <see cref="SecurityKey"/> and algorithm pair are not supported.</exception>
        /// <exception cref="ArgumentOutOfRangeException">'<see cref="SecurityKey"/>.KeySize' is smaller than <see cref="SymmetricSignatureProvider.MinimumSymmetricKeySizeInBits"/>.</exception>
        public SymmetricSignatureProvider(SecurityKey key, string algorithm, bool willCreateSignatures)
            : base(key, algorithm)
        {
            if (!key.CryptoProviderFactory.IsSupportedAlgorithm(algorithm, key))
                throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant(LogMessages.IDX10634, LogHelper.MarkAsNonPII((algorithm)), key)));

            if (key.KeySize < MinimumSymmetricKeySizeInBits)
                throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(nameof(key), LogHelper.FormatInvariant(LogMessages.IDX10653, LogHelper.MarkAsNonPII((algorithm)), LogHelper.MarkAsNonPII(MinimumSymmetricKeySizeInBits), key, LogHelper.MarkAsNonPII(key.KeySize))));

            WillCreateSignatures = willCreateSignatures;
            _keyedHashObjectPool = new DisposableObjectPool<KeyedHashAlgorithm>(CreateKeyedHashAlgorithm, key.CryptoProviderFactory.SignatureProviderObjectPoolCacheSize);
        }

        /// <summary>
        /// Gets or sets the minimum <see cref="SymmetricSecurityKey"/>.KeySize"/>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">'value' is smaller than <see cref="DefaultMinimumSymmetricKeySizeInBits"/>.</exception>
        public int MinimumSymmetricKeySizeInBits
        {
            get
            {
                return _minimumSymmetricKeySizeInBits;
            }
            set
            {
                if (value < DefaultMinimumSymmetricKeySizeInBits)
                    throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(nameof(value), LogHelper.FormatInvariant(LogMessages.IDX10628, LogHelper.MarkAsNonPII(DefaultMinimumSymmetricKeySizeInBits))));

                _minimumSymmetricKeySizeInBits = value;
            }
        }

        /// <summary>
        /// Called to obtain the byte[] needed to create a <see cref="KeyedHashAlgorithm"/>
        /// </summary>
        /// <param name="key"><see cref="SecurityKey"/>that will be used to obtain the byte[].</param>
        /// <returns><see cref="byte"/>[] that is used to populated the KeyedHashAlgorithm.</returns>
        /// <exception cref="ArgumentNullException">if key is null.</exception>
        /// <exception cref="ArgumentException">if a byte[] can not be obtained from SecurityKey.</exception>
        /// <remarks><see cref="SymmetricSecurityKey"/> and <see cref="JsonWebKey"/> are supported.
        /// <para>For a <see cref="SymmetricSecurityKey"/> .Key is returned</para>
        /// <para>For a <see cref="JsonWebKey"/>Base64UrlEncoder.DecodeBytes is called with <see cref="JsonWebKey.K"/> if <see cref="JsonWebKey.Kty"/> == JsonWebAlgorithmsKeyTypes.Octet</para>
        /// </remarks>
        protected virtual byte[] GetKeyBytes(SecurityKey key)
        {
            if (key == null)
                throw LogHelper.LogArgumentNullException(nameof(key));

            if (key is SymmetricSecurityKey symmetricSecurityKey)
                return symmetricSecurityKey.Key;

            if (key is JsonWebKey jsonWebKey && jsonWebKey.K != null && jsonWebKey.Kty == JsonWebAlgorithmsKeyTypes.Octet)
                return Base64UrlEncoder.DecodeBytes(jsonWebKey.K);

            throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10667, key)));
        }

        /// <summary>
        /// Returns a <see cref="KeyedHashAlgorithm"/>.
        /// This method is called just before a cryptographic operation.
        /// This provides the opportunity to obtain the <see cref="KeyedHashAlgorithm"/> from an object pool.
        /// If this method is overridden, it is importont to override <see cref="ReleaseKeyedHashAlgorithm(KeyedHashAlgorithm)"/>
        /// if custom releasing of the <see cref="KeyedHashAlgorithm"/> is desired.
        /// </summary>
        /// <param name="algorithm">The hash algorithm to use to create the hash value.</param>
        /// <param name="keyBytes">The byte array of the key.</param>
        /// <returns>An instance of <see cref="KeyedHashAlgorithm"/></returns>
        protected virtual KeyedHashAlgorithm GetKeyedHashAlgorithm(byte[] keyBytes, string algorithm)
        {
            return _keyedHashObjectPool.Allocate();
        }

        private KeyedHashAlgorithm CreateKeyedHashAlgorithm()
        {
            return Key.CryptoProviderFactory.CreateKeyedHashAlgorithm(GetKeyBytes(Key), Algorithm);
        }

        /// <summary>
        /// For testing purposes
        /// </summary>
        internal override int ObjectPoolSize => _keyedHashObjectPool.Size;

        /// <summary>
        /// This method is called just after the cryptographic operation.
        /// If <see cref="GetKeyedHashAlgorithm(byte[], string)"/> was overridden this method can be overridden for
        /// any custom handling such as returning the <see cref="KeyedHashAlgorithm"/> to an object pool.
        /// </summary>
        /// <param name="keyedHashAlgorithm">The <see cref="KeyedHashAlgorithm"/>" in use.</param>
        protected virtual void ReleaseKeyedHashAlgorithm(KeyedHashAlgorithm keyedHashAlgorithm)
        {
            if (keyedHashAlgorithm != null)
                _keyedHashObjectPool.Free(keyedHashAlgorithm);
        }

        /// <summary>
        /// Produces a signature over the 'input' using the <see cref="SymmetricSecurityKey"/> and 'algorithm' passed to <see cref="SymmetricSignatureProvider( SecurityKey, string )"/>.
        /// </summary>
        /// <param name="input">The bytes to sign.</param>
        /// <returns>Signed bytes</returns>
        /// <exception cref="ArgumentNullException">'input' is null. </exception>
        /// <exception cref="ArgumentException">'input.Length' == 0. </exception>
        /// <exception cref="ObjectDisposedException"><see cref="Dispose(bool)"/> has been called.</exception>
        /// <exception cref="InvalidOperationException"><see cref="KeyedHashAlgorithm"/> is null. This can occur if a derived type deletes it or does not create it.</exception>
        /// <remarks>Sign is thread safe.</remarks>
        public override byte[] Sign(byte[] input)
        {
            if (input == null || input.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(input));

            if (_disposed)
            {
                CryptoProviderCache?.TryRemove(this);
                throw LogHelper.LogExceptionMessage(new ObjectDisposedException(GetType().ToString()));
            }

            if (LogHelper.IsEnabled(EventLogLevel.Informational))
                LogHelper.LogInformation(LogMessages.IDX10642, input);

            KeyedHashAlgorithm keyedHashAlgorithm = GetKeyedHashAlgorithm(GetKeyBytes(Key), Algorithm);

            try
            {
                return keyedHashAlgorithm.ComputeHash(input);
            }
            catch
            {
                CryptoProviderCache?.TryRemove(this);
                Dispose(true);
                throw;
            }
            finally
            {
                if (!_disposed)
                    ReleaseKeyedHashAlgorithm(keyedHashAlgorithm);
            }
        }

#if NET6_0_OR_GREATER
        /// <inheritdoc/>
        public override bool Sign(ReadOnlySpan<byte> input, Span<byte> signature, out int bytesWritten)
        {
            if (input.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(input));

            if (_disposed)
            {
                CryptoProviderCache?.TryRemove(this);
                throw LogHelper.LogExceptionMessage(new ObjectDisposedException(GetType().ToString()));
            }

            KeyedHashAlgorithm keyedHashAlgorithm = GetKeyedHashAlgorithm(GetKeyBytes(Key), Algorithm);

            try
            {
                return keyedHashAlgorithm.TryComputeHash(input, signature, out bytesWritten);
            }
            catch
            {
                CryptoProviderCache?.TryRemove(this);
                Dispose(true);
                throw;
            }
            finally
            {
                if (!_disposed)
                    ReleaseKeyedHashAlgorithm(keyedHashAlgorithm);
            }
        }
#endif

        /// <inheritdoc/>
        public override byte[] Sign(byte[] input, int offset, int count)
        {
            if (input == null || input.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(input));

            if (_disposed)
            {
                CryptoProviderCache?.TryRemove(this);
                throw LogHelper.LogExceptionMessage(new ObjectDisposedException(GetType().ToString()));
            }

            if (LogHelper.IsEnabled(EventLogLevel.Informational))
                LogHelper.LogInformation(LogMessages.IDX10642, input);

            KeyedHashAlgorithm keyedHashAlgorithm = GetKeyedHashAlgorithm(GetKeyBytes(Key), Algorithm);

            try
            {
                return keyedHashAlgorithm.ComputeHash(input, offset, count);
            }
            catch
            {
                CryptoProviderCache?.TryRemove(this);
                Dispose(true);
                throw;
            }
            finally
            {
                if (!_disposed)
                    ReleaseKeyedHashAlgorithm(keyedHashAlgorithm);
            }
        }

        /// <summary>
        /// Verifies that a signature created over the 'input' matches the signature. Using <see cref="SymmetricSecurityKey"/> and 'algorithm' passed to <see cref="SymmetricSignatureProvider( SecurityKey, string )"/>.
        /// </summary>
        /// <param name="input">The bytes to verify.</param>
        /// <param name="signature">signature to compare against.</param>
        /// <returns>true if computed signature matches the signature parameter, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">'input' is null.</exception>
        /// <exception cref="ArgumentNullException">'signature' is null.</exception>
        /// <exception cref="ArgumentException">'input.Length' == 0.</exception>
        /// <exception cref="ArgumentException">'signature.Length' == 0. </exception>
        /// <exception cref="ObjectDisposedException"><see cref="Dispose(bool)"/> has been called.</exception>
        /// <exception cref="InvalidOperationException">If the internal <see cref="KeyedHashAlgorithm"/> is null. This can occur if a derived type deletes it or does not create it.</exception>
        /// <remarks>Verify is thread safe.</remarks>
        public override bool Verify(byte[] input, byte[] signature)
        {
            if (input == null || input.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(input));

            if (signature == null || signature.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(signature));

            // The reason this method doesn't call through to: Verify(input, 0, input.Length, signature, 0, signature.Length);
            // Is because this method's contract is to check the entire signature, if the signature was truncated and signature.Length
            // was passed, the signature may verify.

            if (_disposed)
            {
                CryptoProviderCache?.TryRemove(this);
                throw LogHelper.LogExceptionMessage(new ObjectDisposedException(GetType().ToString()));
            }

            if (LogHelper.IsEnabled(EventLogLevel.Informational))
                LogHelper.LogInformation(LogMessages.IDX10643, input);

            KeyedHashAlgorithm keyedHashAlgorithm = GetKeyedHashAlgorithm(GetKeyBytes(Key), Algorithm);
            try
            {
                return Utility.AreEqual(signature, keyedHashAlgorithm.ComputeHash(input));
            }
            catch
            {
                CryptoProviderCache?.TryRemove(this);
                Dispose(true);
                throw;
            }
            finally
            {
                if (!_disposed)
                    ReleaseKeyedHashAlgorithm(keyedHashAlgorithm);
            }
        }

        /// <summary>
        /// Verifies that a signature created over the 'input' matches the signature. Using <see cref="SymmetricSecurityKey"/> and 'algorithm' passed to <see cref="SymmetricSignatureProvider( SecurityKey, string )"/>.
        /// </summary>
        /// <param name="input">The bytes to verify.</param>
        /// <param name="signature">signature to compare against.</param>
        /// <param name="length">number of bytes of signature to use.</param>
        /// <returns>true if computed signature matches the signature parameter, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">'input' is null.</exception>
        /// <exception cref="ArgumentNullException">'signature' is null.</exception>
        /// <exception cref="ArgumentException">'input.Length' == 0.</exception>
        /// <exception cref="ArgumentException">'signature.Length' == 0. </exception>
        /// <exception cref="ArgumentException">'length &lt; 1'</exception>
        /// <exception cref="ObjectDisposedException"><see cref="Dispose(bool)"/> has been called.</exception>
        /// <exception cref="InvalidOperationException">If the internal <see cref="KeyedHashAlgorithm"/> is null. This can occur if a derived type deletes it or does not create it.</exception>
        public bool Verify(byte[] input, byte[] signature, int length)
        {
            if (input == null)
                throw LogHelper.LogArgumentNullException(nameof(input));

            return Verify(input, 0, input.Length, signature, 0, length);
        }

        /// <inheritdoc/>
        public override bool Verify(byte[] input, int inputOffset, int inputLength, byte[] signature, int signatureOffset, int signatureLength)
        {
            return Verify(input, inputOffset, inputLength, signature, signatureOffset, signatureLength, null);
        }

        /// <summary>
        /// This internal method is called from the AuthenticatedEncryptionProvider which passes in the algorithm that defines the size expected for the signature.
        /// The reason is the way the AuthenticationTag is validated.
        /// For example when "A128CBC-HS256" is specified, SHA256 will used to create the HMAC and 32 bytes will be generated, but only the first 16 will be validated.
        /// </summary>
        /// <param name="input">The bytes to verify.</param>
        /// <param name="inputOffset">offset in to input bytes to caculate hash.</param>
        /// <param name="inputLength">number of bytes of signature to use.</param>
        /// <param name="signature">signature to compare against.</param>
        /// <param name="signatureOffset">offset into signature array.</param>
        /// <param name="signatureLength">how many bytes to verfiy.</param>
        /// <param name="algorithm">algorithm passed by AuthenticatedEncryptionProvider.</param>
        /// <returns>true if computed signature matches the signature parameter, false otherwise.</returns>
#if NET6_0_OR_GREATER
        [SkipLocalsInit]
#endif
        internal bool Verify(byte[] input, int inputOffset, int inputLength, byte[] signature, int signatureOffset, int signatureLength, string algorithm)
        {
            if (input == null || input.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(input));

            if (signature == null || signature.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(signature));

            if (inputOffset < 0)
                throw LogHelper.LogExceptionMessage(new ArgumentException(
                    LogHelper.FormatInvariant(
                        LogMessages.IDX10716,
                        LogHelper.MarkAsNonPII(nameof(inputOffset)),
                        LogHelper.MarkAsNonPII(inputOffset))));

            if (inputLength < 1)
                throw LogHelper.LogExceptionMessage(new ArgumentException(
                    LogHelper.FormatInvariant(
                        LogMessages.IDX10655,
                        LogHelper.MarkAsNonPII(nameof(inputLength)),
                        LogHelper.MarkAsNonPII(inputLength))));

            if (inputOffset + inputLength > input.Length)
                throw LogHelper.LogExceptionMessage(new ArgumentException(
                    LogHelper.FormatInvariant(
                        LogMessages.IDX10717,
                        LogHelper.MarkAsNonPII(nameof(inputOffset)),
                        LogHelper.MarkAsNonPII(nameof(inputLength)),
                        LogHelper.MarkAsNonPII(nameof(input)),
                        LogHelper.MarkAsNonPII(inputOffset),
                        LogHelper.MarkAsNonPII(inputLength),
                        LogHelper.MarkAsNonPII(input.Length))));

            if (signatureOffset < 0)
                throw LogHelper.LogExceptionMessage(new ArgumentException(
                    LogHelper.FormatInvariant(
                        LogMessages.IDX10716,
                        LogHelper.MarkAsNonPII(nameof(signatureOffset)),
                        LogHelper.MarkAsNonPII(signatureOffset))));

            if (signatureLength < 1)
                throw LogHelper.LogExceptionMessage(new ArgumentException(
                    LogHelper.FormatInvariant(
                        LogMessages.IDX10655,
                        LogHelper.MarkAsNonPII(nameof(signatureLength)),
                        LogHelper.MarkAsNonPII(signatureLength))));

            if (signatureLength + signatureOffset > signature.Length)
                throw LogHelper.LogExceptionMessage(new ArgumentException(
                    LogHelper.FormatInvariant(
                        LogMessages.IDX10717,
                        LogHelper.MarkAsNonPII(nameof(signatureOffset)),
                        LogHelper.MarkAsNonPII(nameof(signatureLength)),
                        LogHelper.MarkAsNonPII(nameof(signature)),
                        LogHelper.MarkAsNonPII(signatureOffset),
                        LogHelper.MarkAsNonPII(signatureLength),
                        LogHelper.MarkAsNonPII(signature.Length))));

            string algorithmToValidate = algorithm ?? Algorithm;

            // Check that signature length matches algorithm.
            // If we don't have an entry for the algorithm in our dictionary, that is probably a bug.
            // This is why a new message was created, rather than using IDX10640.
            if (!ExpectedSignatureSizeInBytes.TryGetValue(algorithmToValidate, out int expectedSignatureLength))
                throw LogHelper.LogExceptionMessage(new ArgumentException(
                    LogHelper.FormatInvariant(
                        LogMessages.IDX10718,
                        LogHelper.MarkAsNonPII(algorithmToValidate),
                        LogHelper.MarkAsNonPII(Algorithm))));

            if (expectedSignatureLength != signatureLength)
                throw LogHelper.LogExceptionMessage(new ArgumentException(
                    LogHelper.FormatInvariant(
                        LogMessages.IDX10719,
                        LogHelper.MarkAsNonPII(signatureLength),
                        LogHelper.MarkAsNonPII(expectedSignatureLength))));

            if (_disposed)
            {
                CryptoProviderCache?.TryRemove(this);
                throw LogHelper.LogExceptionMessage(new ObjectDisposedException(GetType().ToString()));
            }

            if (LogHelper.IsEnabled(EventLogLevel.Informational))
                LogHelper.LogInformation(LogMessages.IDX10643, input);

            KeyedHashAlgorithm keyedHashAlgorithm = null;
            try
            {
                keyedHashAlgorithm = GetKeyedHashAlgorithm(GetKeyBytes(Key), Algorithm);

                scoped Span<byte> hash;
#if NET6_0_OR_GREATER
                hash = stackalloc byte[keyedHashAlgorithm.HashSize / 8]; // only known algorithms are used, all of which have a small enough hash size to stackalloc
                keyedHashAlgorithm.TryComputeHash(input.AsSpan(inputOffset, inputLength), hash, out int bytesWritten);
                Debug.Assert(bytesWritten == hash.Length);
#else
                hash = keyedHashAlgorithm.ComputeHash(input, inputOffset, inputLength).AsSpan();
#endif

                return Utility.AreEqual(signature, hash, signatureLength);
            }
            catch
            {
                Dispose(true);
                throw;
            }
            finally
            {
                if (!_disposed)
                    ReleaseKeyedHashAlgorithm(keyedHashAlgorithm);
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Disposes of internal components.
        /// </summary>
        /// <param name="disposing">true, if called from Dispose(), false, if invoked inside a finalizer.</param>
        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;

                if (disposing)
                {
                    foreach (var item in _keyedHashObjectPool.Items)
                        item.Value?.Dispose();

                    CryptoProviderCache?.TryRemove(this);
                }
            }
        }
        #endregion
    }
}
