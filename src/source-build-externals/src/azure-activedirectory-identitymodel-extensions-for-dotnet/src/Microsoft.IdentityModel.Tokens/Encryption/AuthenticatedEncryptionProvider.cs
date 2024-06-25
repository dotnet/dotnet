// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens
{
    delegate AuthenticatedEncryptionResult EncryptionDelegate(byte[] plaintText, byte[] authenticatedData, byte[] iv);
    delegate byte[] DecryptionDelegate(byte[] cipherText, byte[] authenticatedData, byte[] iv, byte[] authenticationTag);

    /// <summary>
    /// Provides authenticated encryption and decryption services.
    /// </summary>
    public class AuthenticatedEncryptionProvider : IDisposable
    {
        private struct AuthenticatedKeys
        {
            public SymmetricSecurityKey AesKey;
            public SymmetricSecurityKey HmacKey;
        }

        private Lazy<AuthenticatedKeys> _authenticatedkeys;
        private DisposableObjectPool<AesGcm> _aesGcmObjectPool;
        private CryptoProviderFactory _cryptoProviderFactory;
        private bool _disposed;
        private Lazy<bool> _keySizeIsValid;
        private Lazy<SymmetricSignatureProvider> _symmetricSignatureProvider;
        private DecryptionDelegate DecryptFunction;
        private EncryptionDelegate EncryptFunction;
        private const string _className = "Microsoft.IdentityModel.Tokens.AuthenticatedEncryptionProvider";

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticatedEncryptionProvider"/> class used for encryption and decryption.
        /// </summary>
        /// <param name="key">The <see cref="SecurityKey"/> that will be used for crypto operations.</param>
        /// <param name="algorithm">The encryption algorithm to apply.</param>
        /// <exception cref="ArgumentNullException">'key' is null.</exception>
        /// <exception cref="ArgumentNullException">'algorithm' is null or whitespace.</exception>
        /// <exception cref="ArgumentOutOfRangeException">key size is not large enough.</exception>
        /// <exception cref="ArgumentException">'algorithm' is not supported.</exception>
        /// <exception cref="ArgumentException">a symmetricSignatureProvider is not created.</exception>
        public AuthenticatedEncryptionProvider(SecurityKey key, string algorithm)
        {
            if (key == null)
                throw LogHelper.LogArgumentNullException(nameof(key));

            if (string.IsNullOrWhiteSpace(algorithm))
                throw LogHelper.LogArgumentNullException(nameof(algorithm));

            Key = key;
            Algorithm = algorithm;
            _cryptoProviderFactory = key.CryptoProviderFactory;
            if (SupportedAlgorithms.IsSupportedEncryptionAlgorithm(algorithm, key))
            {
                if (SupportedAlgorithms.IsAesGcm(algorithm))
                {
#if NETSTANDARD2_0
                    if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        throw LogHelper.LogExceptionMessage(new PlatformNotSupportedException(LogHelper.FormatInvariant(LogMessages.IDX10713, LogHelper.MarkAsNonPII(algorithm))));
#elif NET6_0_OR_GREATER
                    if(!System.Security.Cryptography.AesGcm.IsSupported)
                        throw LogHelper.LogExceptionMessage(new PlatformNotSupportedException(LogHelper.FormatInvariant(LogMessages.IDX10713, LogHelper.MarkAsNonPII(algorithm))));
#endif
                    InitializeUsingAesGcm();
                }
                else
                    InitializeUsingAesCbc();
            }
            else
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10668, LogHelper.MarkAsNonPII(_className), LogHelper.MarkAsNonPII(algorithm), key)));
        }

        private void InitializeUsingAesGcm()
        {
            _keySizeIsValid = new Lazy<bool>(ValidKeySize);
            _aesGcmObjectPool = new DisposableObjectPool<AesGcm>(CreateAesGcmInstance);
            EncryptFunction = EncryptWithAesGcm;
            DecryptFunction = DecryptWithAesGcm;
        }

        private void InitializeUsingAesCbc()
        {
            _authenticatedkeys = new Lazy<AuthenticatedKeys>(CreateAuthenticatedKeys);
            _symmetricSignatureProvider = new Lazy<SymmetricSignatureProvider>(CreateSymmetricSignatureProvider);
            EncryptFunction = EncryptWithAesCbc;
            DecryptFunction = DecryptWithAesCbc;
        }

        internal bool ValidKeySize()
        {
            ValidateKeySize(Key, Algorithm);
            return true;
        }

        private AuthenticatedEncryptionResult EncryptWithAesGcm(byte[] plaintext, byte[] authenticatedData, byte[] iv)
        {
            throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant(LogMessages.IDX10715, LogHelper.MarkAsNonPII(Algorithm))));
        }

        private AesGcm CreateAesGcmInstance()
        {
            return new AesGcm(GetKeyBytes(Key));
        }

        private byte[] DecryptWithAesGcm(byte[] ciphertext, byte[] authenticatedData, byte[] iv, byte[] authenticationTag)
        {
            _ = _keySizeIsValid.Value;
            byte[] clearBytes = new byte[ciphertext.Length];
            AesGcm aes = null;
            try
            {
                aes = _aesGcmObjectPool.Allocate();
                aes.Decrypt(iv, ciphertext, authenticationTag, clearBytes, authenticatedData);
            }
            catch
            {
                Dispose(true);
                throw;
            }
            finally
            {
                if (!_disposed)
                    _aesGcmObjectPool.Free(aes);
            }

            return clearBytes;
        }

        private AuthenticatedEncryptionResult EncryptWithAesCbc(byte[] plaintext, byte[] authenticatedData, byte[] iv)
        {
            using Aes aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = _authenticatedkeys.Value.AesKey.Key;
            if (iv != null)
                aes.IV = iv;

            byte[] ciphertext;
            try
            {
                ciphertext = Transform(aes.CreateEncryptor(), plaintext, 0, plaintext.Length);
            }
            catch (Exception ex)
            {
                throw LogHelper.LogExceptionMessage(new SecurityTokenEncryptionFailedException(LogHelper.FormatInvariant(LogMessages.IDX10654, ex)));
            }

            byte[] al = Utility.ConvertToBigEndian(authenticatedData.Length * 8);
            byte[] macBytes = new byte[authenticatedData.Length + aes.IV.Length + ciphertext.Length + al.Length];
            Array.Copy(authenticatedData, 0, macBytes, 0, authenticatedData.Length);
            Array.Copy(aes.IV, 0, macBytes, authenticatedData.Length, aes.IV.Length);
            Array.Copy(ciphertext, 0, macBytes, authenticatedData.Length + aes.IV.Length, ciphertext.Length);
            Array.Copy(al, 0, macBytes, authenticatedData.Length + aes.IV.Length + ciphertext.Length, al.Length);
            byte[] macHash = _symmetricSignatureProvider.Value.Sign(macBytes);
            var authenticationTag = new byte[_authenticatedkeys.Value.HmacKey.Key.Length];
            Array.Copy(macHash, authenticationTag, authenticationTag.Length);

            return new AuthenticatedEncryptionResult(Key, ciphertext, aes.IV, authenticationTag);
        }

        private byte[] DecryptWithAesCbc(byte[] ciphertext, byte[] authenticatedData, byte[] iv, byte[] authenticationTag)
        {
            // Verify authentication Tag
            if (SymmetricSignatureProvider.ExpectedSignatureSizeInBytes.TryGetValue(Algorithm, out int expectedTagLength)
                && expectedTagLength != authenticationTag.Length)
                throw LogHelper.LogExceptionMessage(new SecurityTokenDecryptionFailedException(
                    LogHelper.FormatInvariant(LogMessages.IDX10625, authenticationTag.Length, expectedTagLength, Base64UrlEncoder.Encode(authenticationTag), Algorithm)));

            byte[] al = Utility.ConvertToBigEndian(authenticatedData.Length * 8);
            byte[] macBytes = new byte[authenticatedData.Length + iv.Length + ciphertext.Length + al.Length];
            Array.Copy(authenticatedData, 0, macBytes, 0, authenticatedData.Length);
            Array.Copy(iv, 0, macBytes, authenticatedData.Length, iv.Length);
            Array.Copy(ciphertext, 0, macBytes, authenticatedData.Length + iv.Length, ciphertext.Length);
            Array.Copy(al, 0, macBytes, authenticatedData.Length + iv.Length + ciphertext.Length, al.Length);
            if (!_symmetricSignatureProvider.Value.Verify(macBytes, 0, macBytes.Length, authenticationTag, 0, _authenticatedkeys.Value.HmacKey.Key.Length, Algorithm))
                throw LogHelper.LogExceptionMessage(new SecurityTokenDecryptionFailedException(LogHelper.FormatInvariant(LogMessages.IDX10650, Base64UrlEncoder.Encode(authenticatedData), Base64UrlEncoder.Encode(iv), Base64UrlEncoder.Encode(authenticationTag))));

            using Aes aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = _authenticatedkeys.Value.AesKey.Key;
            aes.IV = iv;
            try
            {
                return Transform(aes.CreateDecryptor(), ciphertext, 0, ciphertext.Length);
            }
            catch (Exception ex)
            {
                throw LogHelper.LogExceptionMessage(new SecurityTokenDecryptionFailedException(LogHelper.FormatInvariant(LogMessages.IDX10654, ex)));
            }
        }

        private AuthenticatedKeys CreateAuthenticatedKeys()
        {
            ValidateKeySize(Key, Algorithm);

            return GetAlgorithmParameters(Key, Algorithm);
        }

        internal SymmetricSignatureProvider CreateSymmetricSignatureProvider()
        {
            if (!IsSupportedAlgorithm(Key, Algorithm))
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10668, LogHelper.MarkAsNonPII(_className), LogHelper.MarkAsNonPII(Algorithm), Key)));

            ValidateKeySize(Key, Algorithm);

            SymmetricSignatureProvider symmetricSignatureProvider;

            if (Key.CryptoProviderFactory.GetType() == typeof(CryptoProviderFactory))
                symmetricSignatureProvider = Key.CryptoProviderFactory.CreateForSigning(_authenticatedkeys.Value.HmacKey, Algorithm, false) as SymmetricSignatureProvider;
            else
                symmetricSignatureProvider = Key.CryptoProviderFactory.CreateForSigning(_authenticatedkeys.Value.HmacKey, Algorithm) as SymmetricSignatureProvider;

            if (symmetricSignatureProvider == null)
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10649, LogHelper.MarkAsNonPII(Algorithm))));

            return symmetricSignatureProvider;
        }

        /// <summary>
        /// Gets the encryption algorithm that is being used.
        /// </summary>
        public string Algorithm { get; }

        /// <summary>
        /// Gets or sets a user context for a <see cref="AuthenticatedEncryptionProvider"/>.
        /// </summary>
        /// <remarks>This is null by default. This can be used by applications for extensibility scenarios.</remarks>
        public string Context { get; set; }

        /// <summary>
        /// Gets the <see cref="SecurityKey"/> that is being used.
        /// </summary>
        public SecurityKey Key { get; }

        /// <summary>
        /// Encrypts the 'plaintext'
        /// </summary>
        /// <param name="plaintext">the data to be encrypted.</param>
        /// <param name="authenticatedData">will be combined with iv and ciphertext to create an authenticationtag.</param>
        /// <returns><see cref="AuthenticatedEncryptionResult"/>containing ciphertext, iv, authenticationtag.</returns>
        /// <exception cref="ArgumentNullException">plaintext is null or empty.</exception>
        /// <exception cref="ArgumentNullException">authenticationData is null or empty.</exception>
        /// <exception cref="SecurityTokenEncryptionFailedException">AES crypto operation threw. See inner exception for details.</exception>
        public virtual AuthenticatedEncryptionResult Encrypt(byte[] plaintext, byte[] authenticatedData)
        {
            return Encrypt(plaintext, authenticatedData, null);
        }

        /// <summary>
        /// Encrypts the 'plaintext'
        /// </summary>
        /// <param name="plaintext">the data to be encrypted.</param>
        /// <param name="authenticatedData">will be combined with iv and ciphertext to create an authenticationtag.</param>
        /// <param name="iv">initialization vector for encryption.</param>
        /// <returns><see cref="AuthenticatedEncryptionResult"/>containing ciphertext, iv, authenticationtag.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="plaintext"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="authenticatedData"/> is null or empty.</exception>
        /// <exception cref="SecurityTokenEncryptionFailedException">Thrown if the AES crypto operation threw. See inner exception for details.</exception>
        /// <exception cref="ObjectDisposedException">Thrown if the internal <see cref="SignatureProvider"/> is disposed.</exception>
        public virtual AuthenticatedEncryptionResult Encrypt(byte[] plaintext, byte[] authenticatedData, byte[] iv)
        {
            if (plaintext == null || plaintext.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(plaintext));

            if (authenticatedData == null || authenticatedData.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(authenticatedData));

            if (_disposed)
                throw LogHelper.LogExceptionMessage(new ObjectDisposedException(GetType().ToString()));

            return EncryptFunction(plaintext, authenticatedData, iv);
        }

        /// <summary>
        /// Decrypts ciphertext into plaintext
        /// </summary>
        /// <param name="ciphertext">the encrypted text to decrypt.</param>
        /// <param name="authenticatedData">the authenticateData that is used in verification.</param>
        /// <param name="iv">the initialization vector used when creating the ciphertext.</param>
        /// <param name="authenticationTag">the authenticationTag that was created during the encyption.</param>
        /// <returns>decrypted ciphertext</returns>
        /// <exception cref="ArgumentNullException"><paramref name="ciphertext"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="authenticatedData"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="iv"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="authenticationTag"/> is null or empty.</exception>
        /// <exception cref="SecurityTokenDecryptionFailedException">Thrown if the signature over the authenticationTag fails to verify.</exception>
        /// <exception cref="SecurityTokenDecryptionFailedException">Thrown if the AES crypto operation threw. See inner exception.</exception>
        /// <exception cref="ObjectDisposedException">Thrown if the internal <see cref="SignatureProvider"/> is disposed.</exception>
        public virtual byte[] Decrypt(byte[] ciphertext, byte[] authenticatedData, byte[] iv, byte[] authenticationTag)
        {
            if (ciphertext == null || ciphertext.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(ciphertext));

            if (authenticatedData == null || authenticatedData.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(authenticatedData));

            if (iv == null || iv.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(iv));

            if (authenticationTag == null || authenticationTag.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(authenticationTag));

            if (_disposed)
                throw LogHelper.LogExceptionMessage(new ObjectDisposedException(GetType().ToString()));

            return DecryptFunction(ciphertext, authenticatedData, iv, authenticationTag);
        }

        /// <summary>
        /// Calls <see cref="Dispose(bool)"/> and <see cref="GC.SuppressFinalize"/>
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases managed resources.
        /// </summary>
        /// <param name="disposing">true, if called from Dispose(), false, if invoked inside a finalizer.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
                if (disposing)
                {
                    if (_symmetricSignatureProvider != null)
                    {
                        _cryptoProviderFactory.ReleaseSignatureProvider(_symmetricSignatureProvider.Value);
                    }

                    if (_aesGcmObjectPool != null)
                    {
                        foreach (var item in _aesGcmObjectPool.Items)
                            item.Value?.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// Checks if an 'key, algorithm' pair is supported
        /// </summary>
        /// <param name="key">the <see cref="SecurityKey"/></param>
        /// <param name="algorithm">the algorithm to check.</param>
        /// <returns>true if 'key, algorithm' pair is supported.</returns>
        protected virtual bool IsSupportedAlgorithm(SecurityKey key, string algorithm)
        {
            return SupportedAlgorithms.IsSupportedEncryptionAlgorithm(algorithm, key);
        }

        private AuthenticatedKeys GetAlgorithmParameters(SecurityKey key, string algorithm)
        {
            int keyLength = -1;
            if (algorithm.Equals(SecurityAlgorithms.Aes256CbcHmacSha512))
                keyLength = 32;
            else if (algorithm.Equals(SecurityAlgorithms.Aes192CbcHmacSha384))
                keyLength = 24;
            else if (algorithm.Equals(SecurityAlgorithms.Aes128CbcHmacSha256))
                keyLength = 16;
            else
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10668, LogHelper.MarkAsNonPII(_className), LogHelper.MarkAsNonPII(algorithm), key)));

            var keyBytes = GetKeyBytes(key);
            byte[] aesKey = new byte[keyLength];
            byte[] hmacKey = new byte[keyLength];
            Array.Copy(keyBytes, keyLength, aesKey, 0, keyLength);
            Array.Copy(keyBytes, hmacKey, keyLength);
            return new AuthenticatedKeys()
            {
                AesKey = new SymmetricSecurityKey(aesKey),
                HmacKey = new SymmetricSecurityKey(hmacKey)
            };
        }

        /// <summary>
        /// The algorithm parameter logically defines a HMAC algorithm.
        /// This method returns the HMAC to use.
        /// </summary>
        /// <param name="algorithm"></param>
        /// <returns></returns>
        private static string GetHmacAlgorithm(string algorithm)
        {
            if (SecurityAlgorithms.Aes128CbcHmacSha256.Equals(algorithm))
                return SecurityAlgorithms.HmacSha256;

            if (SecurityAlgorithms.Aes192CbcHmacSha384.Equals(algorithm))
                return SecurityAlgorithms.HmacSha384;

            if (SecurityAlgorithms.Aes256CbcHmacSha512.Equals(algorithm))
                return SecurityAlgorithms.HmacSha512;

            throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10652, LogHelper.MarkAsNonPII(algorithm)), nameof(algorithm)));
        }

        /// <summary>
        /// Called to obtain the byte[] needed to create a <see cref="KeyedHashAlgorithm"/>
        /// </summary>
        /// <param name="key"><see cref="SecurityKey"/>that will be used to obtain the byte[].</param>
        /// <returns><see cref="byte"/>[] that is used to populated the KeyedHashAlgorithm.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="key"/> is null.</exception>
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

            if (key is JsonWebKey jsonWebKey && JsonWebKeyConverter.TryConvertToSymmetricSecurityKey(jsonWebKey, out SecurityKey securityKey))
                return GetKeyBytes(securityKey);

            throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10667, key)));
        }

        internal static byte[] Transform(ICryptoTransform transform, byte[] input, int inputOffset, int inputLength)
        {
            if (transform.CanTransformMultipleBlocks)
                return transform.TransformFinalBlock(input, inputOffset, inputLength);

            using (var messageStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(messageStream, transform, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(input, inputOffset, inputLength);
                    cryptoStream.FlushFinalBlock();
                    return messageStream.ToArray();
                }
            }
        }

        /// <summary>
        /// Checks that the key has sufficient length
        /// </summary>
        /// <param name="key"><see cref="SecurityKey"/> that contains bytes.</param>
        /// <param name="algorithm">the algorithm to apply.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="key"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="algorithm"/> is null or empty.</exception>
        /// <exception cref="ArgumentException">if <paramref name="algorithm"/> is not a supported algorithm.</exception>
        protected virtual void ValidateKeySize(SecurityKey key, string algorithm)
        {
            if (key == null)
                throw LogHelper.LogArgumentNullException(nameof(key));

            if (string.IsNullOrEmpty(algorithm))
                throw LogHelper.LogArgumentNullException(nameof(algorithm));

            if (SecurityAlgorithms.Aes128CbcHmacSha256.Equals(algorithm))
            {
                if (key.KeySize < 256)
                    throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(nameof(key), LogHelper.FormatInvariant(LogMessages.IDX10653, LogHelper.MarkAsNonPII(SecurityAlgorithms.Aes128CbcHmacSha256), LogHelper.MarkAsNonPII(256), key.KeyId, LogHelper.MarkAsNonPII(key.KeySize))));

                return;
            }

            if (SecurityAlgorithms.Aes192CbcHmacSha384.Equals(algorithm))
            {
                if (key.KeySize < 384)
                    throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(nameof(key), LogHelper.FormatInvariant(LogMessages.IDX10653, LogHelper.MarkAsNonPII(SecurityAlgorithms.Aes192CbcHmacSha384), LogHelper.MarkAsNonPII(384), key.KeyId, LogHelper.MarkAsNonPII(key.KeySize))));

                return;
            }

            if (SecurityAlgorithms.Aes256CbcHmacSha512.Equals(algorithm))
            {
                if (key.KeySize < 512)
                    throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(nameof(key), LogHelper.FormatInvariant(LogMessages.IDX10653, LogHelper.MarkAsNonPII(SecurityAlgorithms.Aes256CbcHmacSha512), LogHelper.MarkAsNonPII(512), key.KeyId, LogHelper.MarkAsNonPII(key.KeySize))));

                return;
            }

            if (SecurityAlgorithms.Aes128Gcm.Equals(algorithm))
            {
                if (key.KeySize < 128)
                    throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(nameof(key), LogHelper.FormatInvariant(LogMessages.IDX10653, LogHelper.MarkAsNonPII(SecurityAlgorithms.Aes128Gcm), LogHelper.MarkAsNonPII(128), key.KeyId, LogHelper.MarkAsNonPII(key.KeySize))));

                return;
            }

            if (SecurityAlgorithms.Aes192Gcm.Equals(algorithm))
            {
                if (key.KeySize < 192)
                    throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(nameof(key), LogHelper.FormatInvariant(LogMessages.IDX10653, LogHelper.MarkAsNonPII(SecurityAlgorithms.Aes192Gcm), LogHelper.MarkAsNonPII(192), key.KeyId, LogHelper.MarkAsNonPII(key.KeySize))));

                return;
            }

            if (SecurityAlgorithms.Aes256Gcm.Equals(algorithm))
            {
                if (key.KeySize < 256)
                    throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(nameof(key), LogHelper.FormatInvariant(LogMessages.IDX10653, LogHelper.MarkAsNonPII(SecurityAlgorithms.Aes256Gcm), LogHelper.MarkAsNonPII(256), key.KeyId, LogHelper.MarkAsNonPII(key.KeySize))));

                return;
            }

            throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10652, LogHelper.MarkAsNonPII(algorithm))));
        }
    }
}
