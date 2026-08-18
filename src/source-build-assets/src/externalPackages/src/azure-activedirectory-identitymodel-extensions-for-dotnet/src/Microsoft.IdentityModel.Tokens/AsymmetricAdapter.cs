// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
#if NET6_0_OR_GREATER
using System.Buffers;
using System.Diagnostics;
#endif
using System.Security.Cryptography;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens
{
    delegate byte[] EncryptDelegate(byte[] bytes);
    delegate byte[] DecryptDelegate(byte[] bytes);
    delegate byte[] SignDelegate(byte[] bytes);
    delegate byte[] SignUsingOffsetDelegate(byte[] bytes, int offset, int count);
#if NET6_0_OR_GREATER
    delegate bool SignUsingSpanDelegate(ReadOnlySpan<byte> bytes, Span<byte> signature, out int bytesWritten);
#endif
    delegate bool VerifyDelegate(byte[] bytes, byte[] signature);
    delegate bool VerifyUsingOffsetDelegate(byte[] bytes, int offset, int count, byte[] signature);

    /// <summary>
    /// This adapter abstracts the 'RSA' differences between versions of .NET targets.
    /// </summary>
    internal class AsymmetricAdapter : IDisposable
    {
#if DESKTOP
        private bool _useRSAOeapPadding;
#endif
        private bool _disposeCryptoOperators;
        private bool _disposed;
        private object _mlDsaSyncLock;
        private DecryptDelegate _decryptFunction = DecryptFunctionNotFound;
        private EncryptDelegate _encryptFunction = EncryptFunctionNotFound;
        private SignDelegate _signFunction = SignFunctionNotFound;
        private SignUsingOffsetDelegate _signUsingOffsetFunction = SignUsingOffsetNotFound;
#if NET6_0_OR_GREATER
        private SignUsingSpanDelegate _signUsingSpanFunction = SignUsingSpanNotFound;
#endif
        private VerifyDelegate _verifyFunction = VerifyNotFound;
        private VerifyUsingOffsetDelegate _verifyUsingOffsetFunction = VerifyUsingOffsetNotFound;

        // Encryption algorithms do not need a HashAlgorithm, this is called by RSAKeyWrap
        internal AsymmetricAdapter(SecurityKey key, string algorithm, bool requirePrivateKey)
            : this(key, algorithm, null, requirePrivateKey)
        {
        }

        internal AsymmetricAdapter(
            SecurityKey key,
            string algorithm,
            HashAlgorithm hashAlgorithm,
            HashAlgorithmName hashAlgorithmName,
            bool requirePrivateKey)
            : this(key, algorithm, hashAlgorithm, requirePrivateKey)
        {

            HashAlgorithmName = hashAlgorithmName;
        }

        internal AsymmetricAdapter(
            SecurityKey key,
            string algorithm,
            HashAlgorithm hashAlgorithm,
            bool requirePrivateKey)
        {
            HashAlgorithm = hashAlgorithm;

            // RsaSecurityKey has either Rsa OR RsaParameters.
            // If we use the RsaParameters, we create a new RSA object and will need to dispose.
            if (key is RsaSecurityKey rsaKey)
            {
                InitializeUsingRsaSecurityKey(rsaKey, algorithm);
            }
            else if (key is X509SecurityKey x509Key)
            {
                InitializeUsingX509SecurityKey(x509Key, algorithm, requirePrivateKey);
            }
            else if (key is JsonWebKey jsonWebKey)
            {
                if (JsonWebKeyConverter.TryConvertToSecurityKey(jsonWebKey, out SecurityKey securityKey))
                {
                    if (securityKey is RsaSecurityKey rsaSecurityKeyFromJsonWebKey)
                        InitializeUsingRsaSecurityKey(rsaSecurityKeyFromJsonWebKey, algorithm);
                    else if (securityKey is X509SecurityKey x509SecurityKeyFromJsonWebKey)
                        InitializeUsingX509SecurityKey(x509SecurityKeyFromJsonWebKey, algorithm, requirePrivateKey);
                    else if (securityKey is ECDsaSecurityKey edcsaSecurityKeyFromJsonWebKey)
                        InitializeUsingEcdsaSecurityKey(edcsaSecurityKeyFromJsonWebKey);
                    else if (securityKey is MlDsaSecurityKey mlDsaSecurityKeyFromJsonWebKey)
                        InitializeUsingMlDsaSecurityKey(mlDsaSecurityKeyFromJsonWebKey, requirePrivateKey);
                    else
                        throw LogHelper.LogExceptionMessage(
                            new NotSupportedException(
                                LogHelper.FormatInvariant(
                                    LogMessages.IDX10684,
                                    LogHelper.MarkAsNonPII(algorithm), LogHelper.MarkAsNonPII(key.KeyId))));
                }
            }
            else if (key is ECDsaSecurityKey ecdsaKey)
            {
                InitializeUsingEcdsaSecurityKey(ecdsaKey);
            }
            else if (key is MlDsaSecurityKey mlDsaKey)
            {
                InitializeUsingMlDsaSecurityKey(mlDsaKey, requirePrivateKey);
            }
            else
                throw LogHelper.LogExceptionMessage(
                    new NotSupportedException(
                        LogHelper.FormatInvariant(
                            LogMessages.IDX10684,
                            LogHelper.MarkAsNonPII(algorithm), LogHelper.MarkAsNonPII(key.KeyId))));
        }

        internal byte[] Decrypt(byte[] data)
        {
            return _decryptFunction(data);
        }

        internal static byte[] DecryptFunctionNotFound(byte[] _)
        {
            // we should never get here, its a bug if we do.
            throw LogHelper.LogExceptionMessage(new NotSupportedException(LogMessages.IDX10711));
        }

        /// <summary>
        /// Calls <see cref="Dispose(bool)"/> and <see cref="GC.SuppressFinalize"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
                if (disposing)
                {
                    if (_disposeCryptoOperators)
                    {
                        if (ECDsa != null)
                            ECDsa.Dispose();

                        if (MLDsa != null)
                            MLDsa.Dispose();
#if DESKTOP
                        if (RsaCryptoServiceProviderProxy != null)
                            RsaCryptoServiceProviderProxy.Dispose();
#endif
                        if (RSA != null)
                            RSA.Dispose();
                    }
                }
            }
        }

        private ECDsa ECDsa { get; set; }

        private MLDsa MLDsa { get; set; }

        internal byte[] Encrypt(byte[] data)
        {
            return _encryptFunction(data);
        }

        internal static byte[] EncryptFunctionNotFound(byte[] _)
        {
            // we should never get here, its a bug if we do.
            throw LogHelper.LogExceptionMessage(new NotSupportedException(LogMessages.IDX10712));
        }

        private HashAlgorithm HashAlgorithm { get; set; }

        private void InitializeUsingEcdsaSecurityKey(ECDsaSecurityKey ecdsaSecurityKey)
        {
            ECDsa = ecdsaSecurityKey.ECDsa;
            _signFunction = SignECDsa;
            _signUsingOffsetFunction = SignUsingOffsetECDsa;
#if NET6_0_OR_GREATER
            _signUsingSpanFunction = SignUsingSpanECDsa;
#endif
            _verifyFunction = VerifyECDsa;
            _verifyUsingOffsetFunction = VerifyUsingOffsetECDsa;
        }

        private void InitializeUsingMlDsaSecurityKey(MlDsaSecurityKey mlDsaSecurityKey, bool requirePrivateKey)
        {
            InitializeUsingMlDsa(mlDsaSecurityKey.MLDsa, requirePrivateKey);
        }

        private void InitializeUsingMlDsa(MLDsa mlDsa, bool includePrivateKey)
        {
            // Clone the MLDsa instance so each adapter holds an independent copy.
            // MLDsa instance methods are not guaranteed thread-safe, and multiple
            // adapters from the object pool may reference the same source key.
            MLDsa clone = MlDsaAdapter.CloneMlDsa(mlDsa, includePrivateKey);

            if (clone is not null)
            {
                // Clone succeeded — each adapter owns its independent instance.
                MLDsa = clone;
                _disposeCryptoOperators = true;
            }
            else
            {
                // Key is non-exportable (e.g., HSM-backed) — share the original
                // instance and serialize access with a lock.
                MLDsa = mlDsa;
                _mlDsaSyncLock = new object();
            }

            _signFunction = SignMlDsa;
            _signUsingOffsetFunction = SignUsingOffsetMlDsa;
#if NET6_0_OR_GREATER
            _signUsingSpanFunction = SignUsingSpanMlDsa;
#endif
            _verifyFunction = VerifyMlDsa;
            _verifyUsingOffsetFunction = VerifyUsingOffsetMlDsa;
        }

        private void InitializeUsingRsa(RSA rsa, string algorithm)
        {
            // The return value for X509Certificate2.GetPrivateKey OR X509Certificate2.GetPublicKey.Key is a RSACryptoServiceProvider
            // These calls return an AsymmetricAlgorithm which doesn't have API's to do much and need to be cast.
            // RSACryptoServiceProvider is wrapped with RSACryptoServiceProviderProxy as some CryptoServiceProviders (CSP's) do
            // not natively support SHA2.
#if DESKTOP
            if (rsa is RSACryptoServiceProvider rsaCryptoServiceProvider)
            {
                _useRSAOeapPadding = algorithm.Equals(SecurityAlgorithms.RsaOAEP)
                                  || algorithm.Equals(SecurityAlgorithms.RsaOaepKeyWrap);

                RsaCryptoServiceProviderProxy = new RSACryptoServiceProviderProxy(rsaCryptoServiceProvider);
                _decryptFunction = DecryptWithRsaCryptoServiceProviderProxy;
                _encryptFunction = EncryptWithRsaCryptoServiceProviderProxy;
                _signFunction = SignWithRsaCryptoServiceProviderProxy;
                _signUsingOffsetFunction = SignWithRsaCryptoServiceProviderProxyUsingOffset;
                _verifyFunction = VerifyWithRsaCryptoServiceProviderProxy;
                _verifyUsingOffsetFunction = VerifyWithRsaCryptoServiceProviderProxyUsingOffset;
                // RSACryptoServiceProviderProxy will track if a new RSA object is created and dispose appropriately.
                _disposeCryptoOperators = true;
                return;
            }
#endif

            if (algorithm.Equals(SecurityAlgorithms.RsaSsaPssSha256) ||
                algorithm.Equals(SecurityAlgorithms.RsaSsaPssSha256Signature) ||
                algorithm.Equals(SecurityAlgorithms.RsaSsaPssSha384) ||
                algorithm.Equals(SecurityAlgorithms.RsaSsaPssSha384Signature) ||
                algorithm.Equals(SecurityAlgorithms.RsaSsaPssSha512) ||
                algorithm.Equals(SecurityAlgorithms.RsaSsaPssSha512Signature))
            {
                RSASignaturePadding = RSASignaturePadding.Pss;
            }
            else
            {
                // default RSASignaturePadding for other supported RSA algorithms is Pkcs1
                RSASignaturePadding = RSASignaturePadding.Pkcs1;
            }

            RSAEncryptionPadding = (algorithm.Equals(SecurityAlgorithms.RsaOAEP) || algorithm.Equals(SecurityAlgorithms.RsaOaepKeyWrap))
                        ? RSAEncryptionPadding.OaepSHA1
                        : RSAEncryptionPadding.Pkcs1;
            RSA = rsa;
            _decryptFunction = DecryptWithRsa;
            _encryptFunction = EncryptWithRsa;
            _signFunction = SignRsa;
            _signUsingOffsetFunction = SignUsingOffsetRsa;
#if NET6_0_OR_GREATER
            _signUsingSpanFunction = SignUsingSpanRsa;
#endif
            _verifyFunction = VerifyRsa;
            _verifyUsingOffsetFunction = VerifyUsingOffsetRsa;
        }

        private void InitializeUsingRsaSecurityKey(RsaSecurityKey rsaSecurityKey, string algorithm)
        {
            if (rsaSecurityKey.Rsa != null)
            {
                InitializeUsingRsa(rsaSecurityKey.Rsa, algorithm);
            }
            else
            {
#if NET472 || NET6_0_OR_GREATER
                var rsa = RSA.Create(rsaSecurityKey.Parameters);
#elif  NET462
                var rsa = new RSACng();
                rsa.ImportParameters(rsaSecurityKey.Parameters);
#else
                var rsa = RSA.Create();
                rsa.ImportParameters(rsaSecurityKey.Parameters);
#endif
                InitializeUsingRsa(rsa, algorithm);
                _disposeCryptoOperators = true;
            }
        }

        private void InitializeUsingX509SecurityKey(
            X509SecurityKey x509SecurityKey,
            string algorithm,
            bool requirePrivateKey)
        {
            if (x509SecurityKey.MlDsaPublicKey != null)
            {
                InitializeUsingX509MlDsa(x509SecurityKey, algorithm, requirePrivateKey);
            }
            else if (x509SecurityKey.PublicKey is RSA)
            {
                InitializeUsingX509Rsa(x509SecurityKey, algorithm, requirePrivateKey);
            }
            else if (x509SecurityKey.PublicKey is ECDsa ecDsa)
            {
                InitializeUsingEcdsaSecurityKey(new ECDsaSecurityKey(ecDsa));
            }
            else
            {
                // Certificate key type is not recognized (not RSA, ECDSA, or ML-DSA).
                throw LogHelper.LogExceptionMessage(
                    new NotSupportedException(
                        LogHelper.FormatInvariant(
                            LogMessages.IDX10725,
                            LogHelper.MarkAsNonPII(algorithm),
                            LogHelper.MarkAsNonPII(x509SecurityKey.KeyId))));
            }
        }

        private void InitializeUsingX509MlDsa(
            X509SecurityKey x509SecurityKey,
            string algorithm,
            bool requirePrivateKey)
        {
            MLDsa mlDsa = requirePrivateKey ? x509SecurityKey.MlDsaPrivateKey : x509SecurityKey.MlDsaPublicKey;
            if (mlDsa == null)
                throw LogHelper.LogExceptionMessage(
                    new InvalidOperationException(
                        LogHelper.FormatInvariant(
                            LogMessages.IDX10723,
                            LogHelper.MarkAsNonPII(algorithm),
                            LogHelper.MarkAsNonPII(x509SecurityKey.KeyId))));

            InitializeUsingMlDsa(mlDsa, requirePrivateKey);
        }

        private void InitializeUsingX509Rsa(
            X509SecurityKey x509SecurityKey,
            string algorithm,
            bool requirePrivateKey)
        {
            if (requirePrivateKey)
            {
                if (x509SecurityKey.PrivateKey == null)
                    throw LogHelper.LogExceptionMessage(
                        new InvalidOperationException(
                            LogHelper.FormatInvariant(
                                LogMessages.IDX10638,
                                LogHelper.MarkAsNonPII(x509SecurityKey.KeyId))));

                InitializeUsingRsa(x509SecurityKey.PrivateKey as RSA, algorithm);
            }
            else
            {
                InitializeUsingRsa(x509SecurityKey.PublicKey as RSA, algorithm);
            }
        }

        private RSA RSA { get; set; }

        internal byte[] Sign(byte[] bytes)
        {
            return _signFunction(bytes);
        }

#if NET6_0_OR_GREATER
        internal bool SignUsingSpan(
            ReadOnlySpan<byte> data,
            Span<byte> destination,
            out int bytesWritten)
        {
            return _signUsingSpanFunction(data, destination, out bytesWritten);
        }
#endif

        internal byte[] SignUsingOffset(byte[] bytes, int offset, int count)
        {
            return _signUsingOffsetFunction(bytes, offset, count);
        }

        private static byte[] SignFunctionNotFound(byte[] _)
        {
            // we should never get here, its a bug if we do.
            throw LogHelper.LogExceptionMessage(new CryptographicException(LogMessages.IDX10685));
        }

        private static byte[] SignUsingOffsetNotFound(byte[] b, int c, int d)
        {
            // we should never get here, its a bug if we do.
            throw LogHelper.LogExceptionMessage(new CryptographicException(LogMessages.IDX10685));
        }

#if NET6_0_OR_GREATER
#pragma warning disable CA1801 // Review unused parameters
        private static bool SignUsingSpanNotFound(
            ReadOnlySpan<byte> data,
            Span<byte> destination,
            out int bytesWritten)
#pragma warning restore CA1801 // Review unused parameters
        {
            // we should never get here, its a bug if we do.
            throw LogHelper.LogExceptionMessage(new CryptographicException(LogMessages.IDX10685));
        }
#endif

        private byte[] SignECDsa(byte[] bytes)
        {
            return ECDsa.SignHash(HashAlgorithm.ComputeHash(bytes));
        }

#if NET6_0_OR_GREATER
        internal bool SignUsingSpanECDsa(
            ReadOnlySpan<byte> data,
            Span<byte> destination,
            out int bytesWritten)
        {
            // ECDSA.TrySignData will return true and set bytesWritten = 64, if destination is null.
            if (destination.Length == 0)
            {
                bytesWritten = 0;
                return false;
            }

            bool success = ECDsa.TrySignData(data, destination, HashAlgorithmName, out bytesWritten);
            if (!success || bytesWritten == 0)
                return false;

            return destination.Length >= bytesWritten;
        }
#endif

        private byte[] SignUsingOffsetECDsa(byte[] bytes, int offset, int count)
        {
            return ECDsa.SignHash(HashAlgorithm.ComputeHash(bytes, offset, count));
        }

        private byte[] SignMlDsa(byte[] bytes)
        {
            if (_mlDsaSyncLock is not null)
            {
                lock (_mlDsaSyncLock)
                    return MLDsa.SignData(bytes, context: null);
            }

            return MLDsa.SignData(bytes, context: null);
        }

        internal bool SignUsingSpanMlDsa(
            ReadOnlySpan<byte> data,
            Span<byte> destination,
            out int bytesWritten)
        {
            int signatureSize = MLDsa.Algorithm.SignatureSizeInBytes;
            if (destination.Length < signatureSize)
            {
                bytesWritten = 0;
                return false;
            }

            // MLDsa.SignData requires destination to be exactly SignatureSizeInBytes.
            if (_mlDsaSyncLock is not null)
            {
                lock (_mlDsaSyncLock)
                    MLDsa.SignData(data, destination.Slice(0, signatureSize), context: default);
            }
            else
            {
                MLDsa.SignData(data, destination.Slice(0, signatureSize), context: default);
            }

            bytesWritten = signatureSize;
            return true;
        }

        private byte[] SignUsingOffsetMlDsa(byte[] bytes, int offset, int count)
        {
            int signatureSize = MLDsa.Algorithm.SignatureSizeInBytes;
            byte[] signature = new byte[signatureSize];

            if (_mlDsaSyncLock is not null)
            {
                lock (_mlDsaSyncLock)
                    MLDsa.SignData(
                        new ReadOnlySpan<byte>(bytes, offset, count),
                        signature.AsSpan(),
                        context: default);
            }
            else
            {
                MLDsa.SignData(
                    new ReadOnlySpan<byte>(bytes, offset, count),
                    signature.AsSpan(),
                    context: default);
            }

            return signature;
        }

        internal bool Verify(byte[] bytes, byte[] signature)
        {
            return _verifyFunction(bytes, signature);
        }

        internal bool VerifyUsingOffset(byte[] bytes, int offset, int count, byte[] signature)
        {
            return _verifyUsingOffsetFunction(bytes, offset, count, signature);
        }

        private static bool VerifyNotFound(byte[] bytes, byte[] signature)
        {
            // we should never get here, its a bug if we do.
            throw LogHelper.LogExceptionMessage(new NotSupportedException(LogMessages.IDX10686));
        }

        private static bool VerifyUsingOffsetNotFound(byte[] bytes, int offset, int count, byte[] signature)
        {
            // we should never get here, its a bug if we do.
            throw LogHelper.LogExceptionMessage(new NotSupportedException(LogMessages.IDX10686));
        }

        private bool VerifyECDsa(byte[] bytes, byte[] signature)
        {
#if NET6_0_OR_GREATER
            return VerifyUsingSpan(isRSA: false, bytes, signature);
#else
            return ECDsa.VerifyHash(HashAlgorithm.ComputeHash(bytes), signature);
#endif
        }

        private bool VerifyUsingOffsetECDsa(byte[] bytes, int offset, int count, byte[] signature)
        {
#if NET6_0_OR_GREATER
            return VerifyUsingSpan(isRSA: false, bytes.AsSpan(offset, count), signature);
#else
            return ECDsa.VerifyHash(HashAlgorithm.ComputeHash(bytes, offset, count), signature);
#endif
        }

        private bool VerifyMlDsa(byte[] bytes, byte[] signature)
        {
            if (_mlDsaSyncLock is not null)
            {
                lock (_mlDsaSyncLock)
                    return MLDsa.VerifyData(bytes, signature, context: null);
            }

            return MLDsa.VerifyData(bytes, signature, context: null);
        }

        private bool VerifyUsingOffsetMlDsa(byte[] bytes, int offset, int count, byte[] signature)
        {
            if (_mlDsaSyncLock is not null)
            {
                lock (_mlDsaSyncLock)
                    return MLDsa.VerifyData(
                        new ReadOnlySpan<byte>(bytes, offset, count),
                        signature.AsSpan(),
                        context: default);
            }

            return MLDsa.VerifyData(
                new ReadOnlySpan<byte>(bytes, offset, count),
                signature.AsSpan(),
                context: default);
        }

        private byte[] DecryptWithRsa(byte[] bytes)
        {
            return RSA.Decrypt(bytes, RSAEncryptionPadding);
        }

        private byte[] EncryptWithRsa(byte[] bytes)
        {
            return RSA.Encrypt(bytes, RSAEncryptionPadding);
        }

        private HashAlgorithmName HashAlgorithmName { get; set; }

        private RSAEncryptionPadding RSAEncryptionPadding { get; set; }

        private RSASignaturePadding RSASignaturePadding { get; set; }

        private byte[] SignRsa(byte[] bytes)
        {
            return RSA.SignHash(HashAlgorithm.ComputeHash(bytes), HashAlgorithmName, RSASignaturePadding);
        }

#if NET6_0_OR_GREATER
        internal bool SignUsingSpanRsa(ReadOnlySpan<byte> data, Span<byte> destination, out int bytesWritten)
        {
            return RSA.TrySignData(data, destination, HashAlgorithmName, RSASignaturePadding, out bytesWritten);
        }
#endif

        private byte[] SignUsingOffsetRsa(byte[] bytes, int offset, int count)
        {
            return RSA.SignData(bytes, offset, count, HashAlgorithmName, RSASignaturePadding);
        }

        private bool VerifyRsa(byte[] bytes, byte[] signature)
        {
#if NET6_0_OR_GREATER
            return VerifyUsingSpan(isRSA: true, bytes, signature);
#else
            return RSA.VerifyHash(HashAlgorithm.ComputeHash(bytes), signature, HashAlgorithmName, RSASignaturePadding);
#endif
        }

        private bool VerifyUsingOffsetRsa(byte[] bytes, int offset, int count, byte[] signature)
        {
#if NET6_0_OR_GREATER
            return VerifyUsingSpan(isRSA: true, bytes.AsSpan(offset, count), signature);
#else
            return RSA.VerifyHash(
                HashAlgorithm.ComputeHash(bytes, offset, count),
                signature,
                HashAlgorithmName,
                RSASignaturePadding);
#endif
        }

#if NET6_0_OR_GREATER
        private bool VerifyUsingSpan(bool isRSA, ReadOnlySpan<byte> bytes, byte[] signature)
        {
            int hashByteLength = HashAlgorithm.HashSize / 8;
            byte[] array = null;
            Span<byte> hash = hashByteLength <= 256 ? stackalloc byte[256] : array = ArrayPool<byte>.Shared.Rent(hashByteLength);
            hash = hash.Slice(0, hashByteLength);

            try
            {
                bool hashResult = HashAlgorithm.TryComputeHash(bytes, hash, out int bytesWritten);
                Debug.Assert(hashResult && bytesWritten == hashByteLength, "HashAlgorithm.TryComputeHash failed");

                return isRSA ?
                    RSA.VerifyHash(hash, signature, HashAlgorithmName, RSASignaturePadding) :
                    ECDsa.VerifyHash(hash, signature);
            }
            finally
            {
                if (array is not null)
                {
                    ArrayPool<byte>.Shared.Return(array, clearArray: true);
                }
            }
        }
#endif

        #region DESKTOP related code
#if DESKTOP
        internal byte[] DecryptWithRsaCryptoServiceProviderProxy(byte[] bytes)
        {
            return RsaCryptoServiceProviderProxy.Decrypt(bytes, _useRSAOeapPadding);
        }

        internal byte[] EncryptWithRsaCryptoServiceProviderProxy(byte[] bytes)
        {
            return RsaCryptoServiceProviderProxy.Encrypt(bytes, _useRSAOeapPadding);
        }

        private RSACryptoServiceProviderProxy RsaCryptoServiceProviderProxy { get; set; }

        internal byte[] SignWithRsaCryptoServiceProviderProxy(byte[] bytes)
        {
            return RsaCryptoServiceProviderProxy.SignData(bytes, HashAlgorithm);
        }
        internal byte[] SignWithRsaCryptoServiceProviderProxyUsingOffset(byte[] bytes, int offset, int length)
        {
            return RsaCryptoServiceProviderProxy.SignData(bytes, offset, length, HashAlgorithm);
        }

        private bool VerifyWithRsaCryptoServiceProviderProxy(byte[] bytes, byte[] signature)
        {
            return RsaCryptoServiceProviderProxy.VerifyData(bytes, HashAlgorithm, signature);
        }

        private bool VerifyWithRsaCryptoServiceProviderProxyUsingOffset(byte[] bytes, int offset, int length, byte[] signature)
        {
            return RsaCryptoServiceProviderProxy.VerifyDataWithLength(bytes, offset, length, HashAlgorithm, HashAlgorithmName, signature);
        }
#endif
        #endregion

    }
}
