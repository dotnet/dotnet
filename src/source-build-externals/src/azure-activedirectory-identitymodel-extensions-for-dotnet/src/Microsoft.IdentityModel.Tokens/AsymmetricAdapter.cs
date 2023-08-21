// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens
{
    delegate byte[] EncryptDelegate(byte[] bytes);
    delegate byte[] DecryptDelegate(byte[] bytes);
    delegate byte[] SignDelegate(byte[] bytes);
    delegate bool VerifyDelegate(byte[] bytes, byte[] signature);
    delegate bool VerifyDelegateWithLength(byte[] bytes, int start, int length, byte[] signature);

    /// <summary>
    /// This adapter abstracts the 'RSA' differences between versions of .Net targets.
    /// </summary>
    internal class AsymmetricAdapter : IDisposable
    {
#if DESKTOP
        private bool _useRSAOeapPadding = false;
#endif
        private bool _disposeCryptoOperators = false;
        private bool _disposed = false;
        private DecryptDelegate DecryptFunction = DecryptFunctionNotFound;
        private EncryptDelegate EncryptFunction = EncryptFunctionNotFound;
        private SignDelegate SignatureFunction = SignatureFunctionNotFound;
        private VerifyDelegate VerifyFunction = VerifyFunctionNotFound;
        private VerifyDelegateWithLength VerifyFunctionWithLength = VerifyFunctionWithLengthNotFound;

        // Encryption algorithms do not need a HashAlgorithm, this is called by RSAKeyWrap
        internal AsymmetricAdapter(SecurityKey key, string algorithm, bool requirePrivateKey)
            : this(key, algorithm, null, requirePrivateKey)
        {
        }

        internal AsymmetricAdapter(SecurityKey key, string algorithm, HashAlgorithm hashAlgorithm, bool requirePrivateKey)
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
                    else
                        throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant(LogMessages.IDX10684, LogHelper.MarkAsNonPII(algorithm), key)));
                }
            }
            else if (key is ECDsaSecurityKey ecdsaKey)
            {
                InitializeUsingEcdsaSecurityKey(ecdsaKey);
            }
            else
                throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant(LogMessages.IDX10684, LogHelper.MarkAsNonPII(algorithm), key)));
        }

        internal byte[] Decrypt(byte[] data)
        {
            return DecryptFunction(data);
        }

        internal static byte[] DecryptFunctionNotFound(byte[] _)
        {
            // we should never get here, its a bug if we do.
            throw LogHelper.LogExceptionMessage(new NotSupportedException(LogMessages.IDX10711));
        }

        /// <summary>
        /// Calls <see cref="Dispose(bool)"/> and <see cref="GC.SuppressFinalize"/>
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

        internal byte[] Encrypt(byte[] data)
        {
            return EncryptFunction(data);
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
            SignatureFunction = SignWithECDsa;
            VerifyFunction = VerifyWithECDsa;
            VerifyFunctionWithLength = VerifyWithECDsaWithLength;
        }

        private void InitializeUsingRsa(RSA rsa, string algorithm)
        {
            // The return value for X509Certificate2.GetPrivateKey OR X509Certificate2.GetPublicKey.Key is a RSACryptoServiceProvider
            // These calls return an AsymmetricAlgorithm which doesn't have API's to do much and need to be cast.
            // RSACryptoServiceProvider is wrapped with RSACryptoServiceProviderProxy as some CryptoServideProviders (CSP's) do
            // not natively support SHA2.
#if DESKTOP
            if (rsa is RSACryptoServiceProvider rsaCryptoServiceProvider)
            {
                _useRSAOeapPadding = algorithm.Equals(SecurityAlgorithms.RsaOAEP)
                                  || algorithm.Equals(SecurityAlgorithms.RsaOaepKeyWrap);

                RsaCryptoServiceProviderProxy = new RSACryptoServiceProviderProxy(rsaCryptoServiceProvider);
                DecryptFunction = DecryptWithRsaCryptoServiceProviderProxy;
                EncryptFunction = EncryptWithRsaCryptoServiceProviderProxy;
                SignatureFunction = SignWithRsaCryptoServiceProviderProxy;
                VerifyFunction = VerifyWithRsaCryptoServiceProviderProxy;
#if NET461_OR_GREATER
                VerifyFunctionWithLength = VerifyWithRsaCryptoServiceProviderProxyWithLength;
#endif
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
            DecryptFunction = DecryptWithRsa;
            EncryptFunction = EncryptWithRsa;
            SignatureFunction = SignWithRsa;
            VerifyFunction = VerifyWithRsa;
            VerifyFunctionWithLength = VerifyWithRsaWithLength;
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
#else
                var rsa = RSA.Create();
                rsa.ImportParameters(rsaSecurityKey.Parameters);
#endif
                InitializeUsingRsa(rsa, algorithm);
                _disposeCryptoOperators = true;
            }
        }

        private void InitializeUsingX509SecurityKey(X509SecurityKey x509SecurityKey, string algorithm, bool requirePrivateKey)
        {
            if (requirePrivateKey)
                InitializeUsingRsa(x509SecurityKey.PrivateKey as RSA, algorithm);
            else
                InitializeUsingRsa(x509SecurityKey.PublicKey as RSA, algorithm);
        }

        private RSA RSA { get; set; }

        internal byte[] Sign(byte[] bytes)
        {
            return SignatureFunction(bytes);
        }

        private static byte[] SignatureFunctionNotFound(byte[] _)
        {
            // we should never get here, its a bug if we do.
            throw LogHelper.LogExceptionMessage(new CryptographicException(LogMessages.IDX10685));
        }

        private byte[] SignWithECDsa(byte[] bytes)
        {
            return ECDsa.SignHash(HashAlgorithm.ComputeHash(bytes));
        }

        internal bool Verify(byte[] bytes, byte[] signature)
        {
            return VerifyFunction(bytes, signature);
        }

        internal bool Verify(byte[] bytes, int start, int length, byte[] signature)
        {
            return VerifyFunctionWithLength(bytes, start, length, signature);
        }

        private static bool VerifyFunctionNotFound(byte[] bytes, byte[] signature)
        {
            // we should never get here, its a bug if we do.
            throw LogHelper.LogExceptionMessage(new NotSupportedException(LogMessages.IDX10686));
        }

        private static bool VerifyFunctionWithLengthNotFound(byte[] bytes, int start, int length, byte[] signature)
        {
            // we should never get here, its a bug if we do.
            throw LogHelper.LogExceptionMessage(new NotSupportedException(LogMessages.IDX10686));
        }

        private bool VerifyWithECDsa(byte[] bytes, byte[] signature)
        {
            return ECDsa.VerifyHash(HashAlgorithm.ComputeHash(bytes), signature);
        }

        private bool VerifyWithECDsaWithLength(byte[] bytes, int start, int length, byte[] signature)
        {
            return ECDsa.VerifyHash(HashAlgorithm.ComputeHash(bytes, start, length), signature);
        }

#region NET61+ related code
#if NET461 || NET462 || NET472 || NETSTANDARD2_0 || NET6_0_OR_GREATER

        // HasAlgorithmName was introduced into Net46
        internal AsymmetricAdapter(SecurityKey key, string algorithm, HashAlgorithm hashAlgorithm, HashAlgorithmName hashAlgorithmName, bool requirePrivateKey)
            : this(key, algorithm, hashAlgorithm, requirePrivateKey)
        {
            HashAlgorithmName = hashAlgorithmName;
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

        private byte[] SignWithRsa(byte[] bytes)
        {
            return RSA.SignHash(HashAlgorithm.ComputeHash(bytes), HashAlgorithmName, RSASignaturePadding);
        }

        private bool VerifyWithRsa(byte[] bytes, byte[] signature)
        {
            return RSA.VerifyHash(HashAlgorithm.ComputeHash(bytes), signature, HashAlgorithmName, RSASignaturePadding);
        }

        private bool VerifyWithRsaWithLength(byte[] bytes, int start, int length, byte[] signature)
        {
            return RSA.VerifyHash(HashAlgorithm.ComputeHash(bytes, start, length), signature, HashAlgorithmName, RSASignaturePadding);
        }
#endif
#endregion

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

        private bool VerifyWithRsaCryptoServiceProviderProxy(byte[] bytes, byte[] signature)
        {
            return RsaCryptoServiceProviderProxy.VerifyData(bytes, HashAlgorithm, signature);
        }

    #if NET461_OR_GREATER
        private bool VerifyWithRsaCryptoServiceProviderProxyWithLength(byte[] bytes, int offset, int length, byte[] signature)
        {
            return RsaCryptoServiceProviderProxy.VerifyDataWithLength(bytes, offset, length, HashAlgorithm, HashAlgorithmName, signature);
        }
    #endif

#endif
#endregion

    }
}
