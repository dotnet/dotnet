// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

#if DESKTOP

using System;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// The purpose of this class is to ensure that we obtain an RsaCryptoServiceProvider that supports SHA-256 signatures.
    /// If the original RsaCryptoServiceProvider doesn't support SHA-256, we create a new one using the same KeyContainer.
    /// </summary>
    /// <remarks>
    /// There is no support for <see cref="CspParameters"/> and <see cref="CspKeyContainerInfo"/> on non-Windows platforms which makes <see cref="RSACryptoServiceProviderProxy"/> a Windows-specific class.
    /// </remarks>
    public class RSACryptoServiceProviderProxy : RSA
    {
        // CryptoApi provider type for an RSA provider supporting sha-256 digital signatures
        private const int PROV_RSA_AES = 24;

        // CryptoApi provider type for an RSA provider only supporting sha1 digital signatures
        private const int PROV_RSA_FULL = 1;
        private const int PROV_RSA_SCHANNEL = 12;

        private bool _disposed = false;
        private bool _disposeRsa = false;

        // Only dispose of the RsaCryptoServiceProvider object if we created a new instance that supports SHA-256,
        // otherwise do not disposed of the referenced RsaCryptoServiceProvider
        //private bool _disposeRsa;
        private RSACryptoServiceProvider _rsa;

        /// <summary>
        /// Gets the SignatureAlgorithm
        /// </summary>
        public override string SignatureAlgorithm => _rsa.SignatureAlgorithm;

        /// <summary>
        /// Gets the KeyExchangeAlgorithm
        /// </summary>
        public override string KeyExchangeAlgorithm => _rsa.KeyExchangeAlgorithm;

        /// <summary>
        /// Initializes an new instance of <see cref="RSACryptoServiceProviderProxy"/>.
        /// </summary>
        /// <param name="rsa"><see cref="RSACryptoServiceProvider"/></param>
        /// <exception cref="ArgumentNullException">if <paramref name="rsa"/> is null.</exception>
        public RSACryptoServiceProviderProxy(RSACryptoServiceProvider rsa)
        {
            if (rsa == null)
                throw LogHelper.LogArgumentNullException(nameof(rsa));

            // Level up the provider type only if:
            // 1. it is PROV_RSA_FULL or PROV_RSA_SCHANNEL which denote CSPs that only understand Sha1 algorithms
            // 2. it is not associated with a hardware key
            // 3. we are not running on mono (which reports PROV_RSA_FULL but doesn't need a workaround)
            var isSha1Provider = rsa.CspKeyContainerInfo.ProviderType == PROV_RSA_FULL || rsa.CspKeyContainerInfo.ProviderType == PROV_RSA_SCHANNEL;
            var isMono = Type.GetType("Mono.Runtime") != null;
            if (isSha1Provider && !rsa.CspKeyContainerInfo.HardwareDevice)
            {
                var csp = new CspParameters();
                csp.ProviderType = PROV_RSA_AES;
                csp.KeyContainerName = rsa.CspKeyContainerInfo.KeyContainerName;
                csp.KeyNumber = (int)rsa.CspKeyContainerInfo.KeyNumber;
                if (rsa.CspKeyContainerInfo.MachineKeyStore)
                    csp.Flags = CspProviderFlags.UseMachineKeyStore;

                // If UseExistingKey is not specified, the CLR will generate a key for a non-existent group.
                // With this flag, a CryptographicException is thrown instead.
                csp.Flags |= CspProviderFlags.UseExistingKey;

                try
                {
                    _rsa = new RSACryptoServiceProvider(csp);
                    // since we created a new RsaCryptoServiceProvider we need to dispose it
                    _disposeRsa = true;
                }
                catch (CryptographicException) when (isMono)
                {
                    // On mono, this exception is expected behavior.
                    // The solution is to simply not level up the provider as this workaround is not needed on mono.
                    _rsa = rsa;
                }
            }
            else
            {
                // no work-around necessary
                _rsa = rsa;
            }
        }

        /// <summary>
        /// Decrypts data with the System.Security.Cryptography.RSA algorithm.
        /// </summary>
        /// <param name="input">The data to be decrypted.</param>
        /// <param name="fOAEP">true to perform direct System.Security.Cryptography.RSA decryption using OAEP padding
        /// (only available on a computer running Microsoft Windows XP or later) otherwise, false to use PKCS#1 v1.5 padding.</param>
        /// <returns>decrypted bytes.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="input"/> is null or has Length == 0.</exception>
        public byte[] Decrypt(byte[] input, bool fOAEP)
        {
            if (input == null || input.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(input));

            return _rsa.Decrypt(input, fOAEP);
        }

        /// <summary>
        /// Decrypts the input.
        /// </summary>
        /// <param name="input">the bytes to decrypt.</param>
        /// <returns>decrypted bytes</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="input"/> is null or Length == 0.</exception>
        public override byte[] DecryptValue(byte[] input)
        {
            if (input == null || input.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(input));

            return _rsa.DecryptValue(input);
        }

        /// <summary>
        ///  Encrypts data with the System.Security.Cryptography.RSA algorithm.
        /// </summary>
        /// <param name="input">The data to be encrypted.</param>
        /// <param name="fOAEP">true to perform direct System.Security.Cryptography.RSA encryption using OAEP padding (only available on a computer running Microsoft Windows XP or later); 
        /// otherwise, false to use PKCS#1 v1.5 padding.</param>
        /// <returns>encrypted bytes.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="input"/> is null or has Length == 0.</exception>
        public byte[] Encrypt(byte[] input, bool fOAEP)
        {
            if (input == null || input.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(input));

            return _rsa.Encrypt(input, fOAEP);
        }

        /// <summary>
        /// Encrypts the input.
        /// </summary>
        /// <param name="input">the bytes to encrypt.</param>
        /// <returns>encrypted bytes.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="input"/> is null or Length == 0.</exception>
        public override byte[] EncryptValue(byte[] input)
        {
            if (input == null || input.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(input));

            return _rsa.EncryptValue(input);
        }

        /// <summary>
        /// Computes the hash value of the specified byte array using the specified hash algorithm, and signs the resulting hash value.
        /// </summary>
        /// <param name="input">The input byte array for which to compute the hash.</param>
        /// <param name="hash">The hash algorithm to use to create the hash value. </param>
        /// <returns>The <see cref="RSA"/> Signature for the specified data.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="input"/> is null or Length == 0.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="hash"/> is null.</exception>
        public byte[] SignData(byte[] input, object hash)
        {
            if (input == null || input.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(input));

            if (hash == null)
                throw LogHelper.LogArgumentNullException(nameof(hash));

            return _rsa.SignData(input, hash);
        }

        internal byte[] SignData(byte[] input, int offset, int length, object hash)
        {
            if (input == null || input.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(input));

            _  = hash ?? throw LogHelper.LogArgumentNullException(nameof(hash));

            return _rsa.SignData(input, offset, length, hash);
        }

        /// <summary>
        /// Verifies that a digital signature is valid by determining the hash value in the signature using the provided public key and comparing it to the hash value of the provided data.
        /// </summary>
        /// <param name="input">The input byte array.</param>
        /// <param name="hash">The hash algorithm to use to create the hash value.</param>
        /// <param name="signature">The signature byte array to be verified.</param>
        /// <returns>true if the signature is valid; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="input"/> is null or Length == 0.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="hash"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="signature"/> is null or Length == 0.</exception>
        public bool VerifyData(byte[] input, object hash, byte[] signature)
        {
            if (input == null || input.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(input));

            if (hash == null)
                throw LogHelper.LogArgumentNullException(nameof(hash));

            if (signature == null || signature.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(signature));

            return _rsa.VerifyData(input, hash, signature);
        }

#if NET462 || NET472 || NETSTANDARD2_0
        /// <summary>
        /// Verifies that a digital signature is valid by determining the hash value in the signature using the provided public key and comparing it to the hash value of the provided data.
        /// </summary>
        /// <param name="input">The input byte array.</param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <param name="hash">The hash algorithm to use to create the hash value.</param>
        /// <param name="hashAlgorithmName"></param>
        /// <param name="signature">The signature byte array to be verified.</param>
        /// <returns>true if the signature is valid; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="input"/> is null or Length == 0.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="hash"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="signature"/> is null or Length == 0.</exception>
        public bool VerifyDataWithLength(byte[] input, int offset, int length, object hash, HashAlgorithmName hashAlgorithmName, byte[] signature)
        {
            if (input == null || input.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(input));

            if (hash == null)
                throw LogHelper.LogArgumentNullException(nameof(hash));

            if (signature == null || signature.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(signature));

            if (offset < 0)
                throw LogHelper.LogExceptionMessage(new ArgumentException(
                    LogHelper.FormatInvariant(
                        LogMessages.IDX10716,
                        LogHelper.MarkAsNonPII(nameof(offset)),
                        LogHelper.MarkAsNonPII(offset))));

            if (length < 1)
                throw LogHelper.LogExceptionMessage(new ArgumentException(
                    LogHelper.FormatInvariant(
                        LogMessages.IDX10655,
                        LogHelper.MarkAsNonPII(nameof(length)),
                        LogHelper.MarkAsNonPII(length))));

            if (offset + length > input.Length)
                throw LogHelper.LogExceptionMessage(new ArgumentException(
                    LogHelper.FormatInvariant(
                        LogMessages.IDX10717,
                        LogHelper.MarkAsNonPII(nameof(offset)),
                        LogHelper.MarkAsNonPII(nameof(length)),
                        LogHelper.MarkAsNonPII(nameof(input)),
                        LogHelper.MarkAsNonPII(offset),
                        LogHelper.MarkAsNonPII(length),
                        LogHelper.MarkAsNonPII(input.Length))));

            return _rsa.VerifyHash(
                (hash as HashAlgorithm).ComputeHash(input, offset, length),
                signature,
                hashAlgorithmName,
                RSASignaturePadding.Pkcs1);
        }
#endif

        /// <summary>
        /// Exports rsa parameters as <see cref="RSAParameters"/>
        /// </summary>
        /// <param name="includePrivateParameters">flag to control is private parameters are included.</param>
        /// <returns><see cref="RSAParameters"/></returns>
        public override RSAParameters ExportParameters(bool includePrivateParameters)
        {
            return _rsa.ExportParameters(includePrivateParameters);
        }

        /// <summary>
        /// Imports rsa parameters as <see cref="RSAParameters"/>
        /// </summary>
        /// <param name="parameters">to import.</param>
        public override void ImportParameters(RSAParameters parameters)
        {
            _rsa.ImportParameters(parameters);
        }

        /// <summary>
        /// Calls to release managed resources.
        /// </summary>
        /// <param name="disposing">true, if called from Dispose(), false, if invoked inside a finalizer.</param>
        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
                if (disposing)
                {
                    if (_disposeRsa)
                    {
                        _rsa.Dispose();
                    }
                }
            }

            base.Dispose(disposing);
        }
    }
}

#endif
