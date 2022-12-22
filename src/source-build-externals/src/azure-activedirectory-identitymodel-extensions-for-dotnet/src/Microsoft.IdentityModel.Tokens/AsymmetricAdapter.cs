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
using System.Security.Cryptography;
using Microsoft.IdentityModel.Logging;

#if NET45
using System.Reflection;
#endif

#if NET461 || NET472 || NETSTANDARD2_0 || NET6_0
using System.Security.Cryptography.X509Certificates;
#endif

namespace Microsoft.IdentityModel.Tokens
{
    delegate byte[] EncryptDelegate(byte[] bytes);
    delegate byte[] DecryptDelegate(byte[] bytes);
    delegate byte[] SignDelegate(byte[] bytes);
    delegate bool VerifyDelegate(byte[] bytes, byte[] signature);

    /// <summary>
    /// This adapter abstracts the 'RSA' differences between versions of .Net targets.
    /// </summary>
    internal class AsymmetricAdapter : IDisposable
    {
#if NET45
        // For users that have built targeting 4.5.1, 4.5.2 or 4.6.0 they will bind to our 4.5 target.
        // It is possible for the application to pass the call to X509Certificate2.GetRSAPublicKey() or X509Certificate2.GetRSAPrivateKey()
        // which returns RSACng(). Our 4.5 target doesn't know about this type and sees it as RSA, then things start to go bad.
        // We use reflection to detect that 4.6+ is available and access the appropriate signing or verifying methods.
        private static Type _hashAlgorithmNameType = typeof(object).Assembly.GetType("System.Security.Cryptography.HashAlgorithmName", false);
        private static Type _rsaEncryptionPaddingType = typeof(object).Assembly.GetType("System.Security.Cryptography.RSAEncryptionPadding", false);
        private static Type _rsaSignaturePaddingType = typeof(object).Assembly.GetType("System.Security.Cryptography.RSASignaturePadding", false);

        private Func<RSA, byte[], byte[]> _rsaDecrypt45Method;
        private Func<RSA, byte[], byte[]> _rsaEncrypt45Method;
        private Func<RSA, byte[], string, byte[]> _rsaPkcs1SignMethod;
        private Func<RSA, byte[], byte[], string, bool> _rsaPkcs1VerifyMethod;
        private string _lightUpHashAlgorithmName = string.Empty;
        private const string _dsaCngTypeName = "System.Security.Cryptography.DSACng";
        private const string _rsaCngTypeName = "System.Security.Cryptography.RSACng";
#endif

#if DESKTOP
        private bool _useRSAOeapPadding = false;
#endif

        private bool _disposeCryptoOperators = false;
        private bool _disposed = false;
        private DecryptDelegate DecryptFunction = DecryptFunctionNotFound;
        private EncryptDelegate EncryptFunction = EncryptFunctionNotFound;
        private SignDelegate SignatureFunction = SignatureFunctionNotFound;
        private VerifyDelegate VerifyFunction = VerifyFunctionNotFound;

        // Encryption algorithms do not need a HashAlgorithm, this is called by RSAKeyWrap
        internal AsymmetricAdapter(SecurityKey key, string algorithm, bool requirePrivateKey)
            : this(key, algorithm, null, requirePrivateKey)
        {
        }

        // This constructor will be used by NET45 for signing and for RSAKeyWrap
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

                // RSACryptoServiceProviderProxy will track if a new RSA object is created and dispose appropriately.
                _disposeCryptoOperators = true;
                return;
            }
#endif

#if NET45
            // This case required the user to get a RSA object by calling
            // X509Certificate2.GetRSAPrivateKey() OR X509Certificate2.GetRSAPublicKey()
            // This requires 4.6+ to be installed. If a dependent library is targeting 4.5, 4.5.1, 4.5.2 or 4.6
            // they will bind to our Net45 target, but the type is RSACng.
            // The 'lightup' code will bind to the correct operators.
            else if (rsa.GetType().ToString().Equals(_rsaCngTypeName) && IsRsaCngSupported())
            {
                _useRSAOeapPadding = algorithm.Equals(SecurityAlgorithms.RsaOAEP)
                                  || algorithm.Equals(SecurityAlgorithms.RsaOaepKeyWrap);

                _lightUpHashAlgorithmName = GetLightUpHashAlgorithmName();
                DecryptFunction = DecryptNet45;
                EncryptFunction = EncryptNet45;
                SignatureFunction = Pkcs1SignData;
                VerifyFunction = Pkcs1VerifyData;
                RSA = rsa;
                return;
            }
            else
            {
                // In NET45 we only support RSACryptoServiceProvider or "System.Security.Cryptography.RSACng"
                throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant(LogMessages.IDX10687, LogHelper.MarkAsNonPII(typeof(RSACryptoServiceProvider).ToString()), LogHelper.MarkAsNonPII(_rsaCngTypeName), LogHelper.MarkAsNonPII(rsa.GetType().ToString()))));
            }
#endif

#if NET461 || NET472 || NETSTANDARD2_0 || NET6_0
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
#endif
        }

        private void InitializeUsingRsaSecurityKey(RsaSecurityKey rsaSecurityKey, string algorithm)
        {
            if (rsaSecurityKey.Rsa != null)
            {
                InitializeUsingRsa(rsaSecurityKey.Rsa, algorithm);
            }
            else
            {
#if NET472 || NET6_0
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

        private static bool VerifyFunctionNotFound(byte[] bytes, byte[] signature)
        {
            // we should never get here, its a bug if we do.
            throw LogHelper.LogExceptionMessage(new NotSupportedException(LogMessages.IDX10686));
        }

        private bool VerifyWithECDsa(byte[] bytes, byte[] signature)
        {
            return ECDsa.VerifyHash(HashAlgorithm.ComputeHash(bytes), signature);
        }

        #region NET61+ related code
#if NET461 || NET472 || NETSTANDARD2_0 || NET6_0
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
#endif
#endregion

#region NET45 'lightup' code
        // the idea here is if a user has defined their application to target 4.6.1+ but some layer in the stack kicks down below, this code builds delegates
        // for decrypting, encryption, signing and validating when we detect that the instance of RSA is RSACng and RSACng is supported by the framework.
#if NET45
        private byte[] DecryptNet45(byte[] bytes)
        {
            if (_rsaDecrypt45Method == null)
            {
                // Decrypt(byte[] data, RSAEncryptionPadding padding)
                Type[] encryptionTypes = { typeof(byte[]), _rsaEncryptionPaddingType };
                MethodInfo encryptMethod = typeof(RSA).GetMethod(
                    "Decrypt",
                    BindingFlags.Public | BindingFlags.Instance,
                    null,
                    encryptionTypes,
                    null);

                Type delegateType = typeof(Func<,,,>).MakeGenericType(
                            typeof(RSA),
                            typeof(byte[]),
                            _rsaEncryptionPaddingType,
                            typeof(byte[]));

                PropertyInfo prop;
                if (_useRSAOeapPadding)
                    prop = _rsaEncryptionPaddingType.GetProperty("OaepSHA1", BindingFlags.Static | BindingFlags.Public);
                else
                    prop = _rsaEncryptionPaddingType.GetProperty("Pkcs1", BindingFlags.Static | BindingFlags.Public);

                Delegate openDelegate = Delegate.CreateDelegate(delegateType, encryptMethod);
                _rsaDecrypt45Method = (rsaArg, bytesArg) =>
                {
                    object[] args =
                    {
                        rsaArg,
                        bytesArg,
                        prop.GetValue(null)
                    };

                    return (byte[])openDelegate.DynamicInvoke(args);
                };
            }

            return _rsaDecrypt45Method(RSA, bytes);
        }

        private byte[] EncryptNet45(byte[] bytes)
        {
            if (_rsaEncrypt45Method == null)
            {
                // Encrypt(byte[] data, RSAEncryptionPadding padding)
                Type[] encryptionTypes = { typeof(byte[]), _rsaEncryptionPaddingType };
                MethodInfo encryptMethod = typeof(RSA).GetMethod(
                    "Encrypt",
                    BindingFlags.Public | BindingFlags.Instance,
                    null,
                    encryptionTypes,
                    null);

                Type delegateType = typeof(Func<,,,>).MakeGenericType(
                            typeof(RSA),
                            typeof(byte[]),
                            _rsaEncryptionPaddingType,
                            typeof(byte[]));

                PropertyInfo prop;
                if (_useRSAOeapPadding)
                    prop = _rsaEncryptionPaddingType.GetProperty("OaepSHA1", BindingFlags.Static | BindingFlags.Public);
                else
                    prop = _rsaEncryptionPaddingType.GetProperty("Pkcs1", BindingFlags.Static | BindingFlags.Public);

                Delegate openDelegate = Delegate.CreateDelegate(delegateType, encryptMethod);
                _rsaEncrypt45Method = (rsaArg, bytesArg) =>
                {
                    object[] args =
                    {
                        rsaArg,
                        bytesArg,
                        prop.GetValue(null)
                    };

                    return (byte[])openDelegate.DynamicInvoke(args);
                };
            }

            return _rsaEncrypt45Method(RSA, bytes);
        }

        private string GetLightUpHashAlgorithmName()
        {
            if (HashAlgorithm == null)
                return "SHA256";

            if (HashAlgorithm.HashSize == 256)
                return "SHA256";

            if (HashAlgorithm.HashSize == 384)
                return "SHA384";

            if (HashAlgorithm.HashSize == 512)
                return "SHA512";

            return HashAlgorithm.ToString();
        }

        /// <summary>
        /// The following code determines if RSACng is available on the .Net framework that is installed.
        /// </summary>
        private static Type GetSystemCoreType(string namespaceQualifiedTypeName)
        {
            Assembly systemCore = typeof(CngKey).Assembly;
            return systemCore.GetType(namespaceQualifiedTypeName, false);
        }

        private static bool IsRsaCngSupported()
        {
            Type rsaCng = GetSystemCoreType(_rsaCngTypeName);

            // If the type doesn't exist, there can't be good support for it.
            // (System.Core < 4.6)
            if (rsaCng == null)
                return false;

            Type dsaCng = GetSystemCoreType(_dsaCngTypeName);

            // The original implementation of RSACng returned shared objects in the CAPI fallback
            // pathway. That behavior is hard to test for, since CNG can load all CAPI software keys.
            // But, since DSACng was added in 4.6.2, and RSACng better guarantees uniqueness in 4.6.2
            // use that coincidence as a compatibility test.
            //
            // If DSACng is missing, RSACng usage might lead to attempting to use Disposed objects
            // (System.Core < 4.6.2)
            if (dsaCng == null)
                return false;

            // Create an RSACng instance and send it to RSAPKCS1KeyExchangeFormatter. It was adjusted to
            // be CNG-capable for 4.6.2; and other types in that library also are up-to-date.
            //
            // If mscorlib can't handle it properly, then other libraries probably can't, so we'll keep
            // preferring RSACryptoServiceProvider.
            try
            {
                new RSAPKCS1KeyExchangeFormatter((RSA)Activator.CreateInstance(rsaCng)).CreateKeyExchange(new byte[1]);
            }
            catch (Exception)
            {
                // (mscorlib < 4.6.2)
                return false;
            }

            return true;
        }

        private byte[] Pkcs1SignData(byte[] input)
        {
            if (_rsaPkcs1SignMethod == null)
            {
                // [X] SignData(byte[] data, HashAlgorithmName hashAlgorithmName, RSASignaturePadding padding)
                // [ ] SignData(byte[] data, int offset, int count, HashAlgorithmName hashAlgorithmName, RSASignaturePadding padding)
                // [ ] SignData(Stream data, HashAlgorithmName hashAlgorithmName, RSASignaturePadding padding)
                Type[] signatureTypes = { typeof(byte[]), _hashAlgorithmNameType, _rsaSignaturePaddingType };

                MethodInfo signDataMethod = typeof(RSA).GetMethod(
                    "SignData",
                    BindingFlags.Public | BindingFlags.Instance,
                    null,
                    signatureTypes,
                    null);

                Type delegateType = typeof(Func<,,,,>).MakeGenericType(
                            typeof(RSA),
                            typeof(byte[]),
                            _hashAlgorithmNameType,
                            _rsaSignaturePaddingType,
                            typeof(byte[]));

                Delegate openDelegate = Delegate.CreateDelegate(delegateType, signDataMethod);
                _rsaPkcs1SignMethod = (rsaArg, dataArg, algorithmArg) =>
                {
                    object[] args =
                    {
                        rsaArg,
                        dataArg,
                        Activator.CreateInstance(_hashAlgorithmNameType, algorithmArg),
                        _rsaSignaturePaddingType.GetProperty("Pkcs1", BindingFlags.Static | BindingFlags.Public).GetValue(null)
                    };

                    return (byte[])openDelegate.DynamicInvoke(args);
                };
            }

            return _rsaPkcs1SignMethod(RSA, input, _lightUpHashAlgorithmName);
        }

        private bool Pkcs1VerifyData(byte[] input, byte[] signature)
        {
            if (_rsaPkcs1VerifyMethod == null)
            {
                // [X] VerifyData(byte[] data, byte[] signature, HashAlgorithmName hashAlgorithmName, RSASignaturePadding padding)
                // [ ] VerifyData(byte[] data, int offset, int count, byte[] signature, HashAlgorithmName hashAlgorithmName, RSASignaturePadding padding)
                // [ ] VerifyData(Stream data, byte[] signature, HashAlgorithmName hashAlgorithmName, RSASignaturePadding padding)
                Type[] signatureTypes = { typeof(byte[]), typeof(byte[]), _hashAlgorithmNameType, _rsaSignaturePaddingType };
                MethodInfo verifyDataMethod = typeof(RSA).GetMethod(
                    "VerifyData",
                    BindingFlags.Public | BindingFlags.Instance,
                    null,
                    signatureTypes,
                    null);

                Type delegateType = typeof(Func<,,,,,>).MakeGenericType(
                    typeof(RSA),
                    typeof(byte[]),
                    typeof(byte[]),
                    _hashAlgorithmNameType,
                    _rsaSignaturePaddingType,
                    typeof(bool));

                Delegate verifyDelegate = Delegate.CreateDelegate(delegateType, verifyDataMethod);
                _rsaPkcs1VerifyMethod = (rsaArg, dataArg, signatureArg, algorithmArg) =>
                {
                    object[] args =
                    {
                        rsaArg,
                        dataArg,
                        signatureArg,
                        Activator.CreateInstance(_hashAlgorithmNameType, algorithmArg),
                        _rsaSignaturePaddingType.GetProperty("Pkcs1", BindingFlags.Static | BindingFlags.Public).GetValue(null)
                    };

                    return (bool)verifyDelegate.DynamicInvoke(args);
                };
            }

            return _rsaPkcs1VerifyMethod(RSA, input, signature, _lightUpHashAlgorithmName);
        }
#endif
#endregion

    }
}
