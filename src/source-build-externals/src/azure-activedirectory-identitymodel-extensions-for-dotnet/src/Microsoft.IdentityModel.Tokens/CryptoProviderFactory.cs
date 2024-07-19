// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Creates cryptographic operators by specifying a <see cref="SecurityKey"/>'s and algorithms.
    /// </summary>
    public class CryptoProviderFactory
    {
        private static CryptoProviderFactory _default;
        private static readonly ConcurrentDictionary<string, string> _typeToAlgorithmMap = new ConcurrentDictionary<string, string>();
        private static readonly object _cacheLock = new object();
        private static int _defaultSignatureProviderObjectPoolCacheSize = Environment.ProcessorCount * 4;
        private int _signatureProviderObjectPoolCacheSize = _defaultSignatureProviderObjectPoolCacheSize;

        /// <summary>
        /// Returns the default <see cref="CryptoProviderFactory"/> instance.
        /// </summary>
        public static CryptoProviderFactory Default
        {
            get { return _default; }
            set
            {
                _default = value ?? throw LogHelper.LogArgumentNullException("value");
            }
        }

        /// <summary>
        /// Gets or sets the default value for caching of <see cref="SignatureProvider"/>'s.
        /// </summary>
        [DefaultValue(true)]
        public static bool DefaultCacheSignatureProviders { get; set; } = true;

        /// <summary>
        /// Gets or sets the maximum size of the object pool used by the SignatureProvider that are used for crypto objects.
        /// </summary>
        public static int DefaultSignatureProviderObjectPoolCacheSize
        {
            get => _defaultSignatureProviderObjectPoolCacheSize;
            set => _defaultSignatureProviderObjectPoolCacheSize = value > 0 ? value : throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(nameof(value), LogHelper.FormatInvariant(LogMessages.IDX10698, LogHelper.MarkAsNonPII(value))));
        }

        /// <summary>
        /// Static constructor that initializes the default <see cref="CryptoProviderFactory"/>.
        /// </summary>
        static CryptoProviderFactory()
        {
            Default = new CryptoProviderFactory();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CryptoProviderFactory"/> class.
        /// </summary>
        public CryptoProviderFactory()
        {
            CryptoProviderCache = new InMemoryCryptoProviderCache() { CryptoProviderFactory = this };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CryptoProviderFactory"/> class.
        /// </summary>
        /// <param name="cache">The cache to use for caching CryptoProviders.</param>
        public CryptoProviderFactory(CryptoProviderCache cache)
        {
            CryptoProviderCache = cache ?? throw LogHelper.LogArgumentNullException(nameof(cache));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CryptoProviderFactory"/> class.
        /// </summary>
        /// <param name="other">The <see cref="CryptoProviderFactory"/> to copy from.</param>
        public CryptoProviderFactory(CryptoProviderFactory other)
        {
            if (other == null)
                throw LogHelper.LogArgumentNullException(nameof(other));

            CryptoProviderCache = new InMemoryCryptoProviderCache() { CryptoProviderFactory = this };
            CustomCryptoProvider = other.CustomCryptoProvider;
            CacheSignatureProviders = other.CacheSignatureProviders;
            SignatureProviderObjectPoolCacheSize = other.SignatureProviderObjectPoolCacheSize;
        }

        /// <summary>
        /// Gets the <see cref="CryptoProviderCache"/>.
        /// </summary>
        public CryptoProviderCache CryptoProviderCache { get; internal set; }

        /// <summary>
        /// Extensibility point for creating custom cryptographic operators.
        /// </summary>
        /// <remarks>By default, if set, <see cref="ICryptoProvider.IsSupportedAlgorithm(string, object[])"/> will be called before creating cryptographic operators.
        /// If true is returned, then <see cref="ICryptoProvider.Create(string, object[])"/> will be called. The <see cref="CryptoProviderFactory"/> will throw if the
        /// Cryptographic operator returned is not of the correct type.</remarks>
        public ICryptoProvider CustomCryptoProvider { get; set; }

        /// <summary>
        /// Gets or sets a bool controlling if <see cref="SignatureProvider"/> should be cached.
        /// </summary>
        [DefaultValue(true)]
        public bool CacheSignatureProviders { get; set; } = DefaultCacheSignatureProviders;

        /// <summary>
        /// Gets or sets the maximum size of the object pool used by the SignatureProvider that are used for crypto objects.
        /// </summary>
        public int SignatureProviderObjectPoolCacheSize
        {
            get => _signatureProviderObjectPoolCacheSize;

            set => _signatureProviderObjectPoolCacheSize = value > 0 ? value : throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(nameof(value), LogHelper.FormatInvariant(LogMessages.IDX10698, LogHelper.MarkAsNonPII(value))));
        }

        /// <summary>
        /// Creates an instance of <see cref="AuthenticatedEncryptionProvider"/> for a specific <paramref name="key"/> and <paramref name="algorithm"/>.
        /// </summary>
        /// <param name="key">The <see cref="SecurityKey"/> to use.</param>
        /// <param name="algorithm">The algorithm to use.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="key"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="algorithm"/> is null or empty.</exception>
        /// <exception cref="ArgumentException">Thrown if the combination of <paramref name="key"/> and <paramref name="algorithm"/> is not supported.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the type returned by <see cref="ICryptoProvider.Create(string, object[])"/> is not assignable to <see cref="KeyWrapProvider"/>.</exception>
        /// <remarks>
        /// <para>If <see cref="CustomCryptoProvider"/> is set and <see cref="ICryptoProvider.IsSupportedAlgorithm(string, object[])"/> returns true,
        /// <see cref="ICryptoProvider.Create(string, object[])"/> is called to obtain the <see cref="AuthenticatedEncryptionProvider"/>.
        /// </para>
        /// <para>Once done with the <see cref="KeyWrapProvider"/>, call <see cref="ReleaseKeyWrapProvider(KeyWrapProvider)"/>.</para>
        /// </remarks>
        /// <returns>An instance of <see cref="AuthenticatedEncryptionProvider"/>.</returns>
        public virtual AuthenticatedEncryptionProvider CreateAuthenticatedEncryptionProvider(SecurityKey key, string algorithm)
        {
            if (key == null)
                throw LogHelper.LogArgumentNullException(nameof(key));

            if (string.IsNullOrEmpty(algorithm))
                throw LogHelper.LogArgumentNullException(nameof(algorithm));

            if (CustomCryptoProvider != null && CustomCryptoProvider.IsSupportedAlgorithm(algorithm, key))
            {
                var cryptoProvider = CustomCryptoProvider.Create(algorithm, key) as AuthenticatedEncryptionProvider;
                if (cryptoProvider == null)
                    throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant(LogMessages.IDX10646, LogHelper.MarkAsNonPII(algorithm), key, LogHelper.MarkAsNonPII(typeof(AuthenticatedEncryptionProvider)))));

                return cryptoProvider;
            }

            if (SupportedAlgorithms.IsSupportedEncryptionAlgorithm(algorithm, key))
                return new AuthenticatedEncryptionProvider(key, algorithm);

            throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10652, LogHelper.MarkAsNonPII(algorithm)), nameof(algorithm)));
        }

        /// <summary>
        /// Creates an instance of <see cref="KeyWrapProvider"/> for a specific <paramref name="key"/> and <paramref name="algorithm"/>.
        /// </summary>
        /// <param name="key">The <see cref="SecurityKey"/> to use.</param>
        /// <param name="algorithm">The algorithm to use.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="key"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="algorithm"/> is null or empty.</exception>
        /// <exception cref="NotSupportedException">Thrown if the combination of <paramref name="key"/> and <paramref name="algorithm"/> is not supported.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the type returned by <see cref="ICryptoProvider.Create(string, object[])"/> is not assignable to <see cref="KeyWrapProvider"/>.</exception>
        /// <remarks>
        /// <para>If <see cref="CustomCryptoProvider"/> is set and <see cref="ICryptoProvider.IsSupportedAlgorithm(string, object[])"/> returns true,
        /// <see cref="ICryptoProvider.Create(string, object[])"/> is called to obtain the <see cref="KeyWrapProvider"/>.
        /// </para>
        /// <para>Once done with the <see cref="KeyWrapProvider"/>, call <see cref="ReleaseKeyWrapProvider(KeyWrapProvider)"/>.</para>
        /// </remarks>
        /// <returns>An instance of <see cref="KeyWrapProvider"/>.</returns>
        public virtual KeyWrapProvider CreateKeyWrapProvider(SecurityKey key, string algorithm)
        {
            return CreateKeyWrapProvider(key, algorithm, false);
        }

        /// <summary>
        /// Creates an instance of <see cref="KeyWrapProvider"/> for a specific <paramref name="key"/> and <paramref name="algorithm"/>.
        /// </summary>
        /// <param name="key">The <see cref="SecurityKey"/> to use.</param>
        /// <param name="algorithm">The algorithm to use.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="key"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="algorithm"/> is null or empty.</exception>
        /// <exception cref="ArgumentException">Thrown if the combination of <paramref name="key"/> and <paramref name="algorithm"/> is not supported.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the type returned by <see cref="ICryptoProvider.Create(string, object[])"/> is not assignable to <see cref="KeyWrapProvider"/>.</exception>
        /// <remarks>
        /// <para>If <see cref="CustomCryptoProvider"/> is set and <see cref="ICryptoProvider.IsSupportedAlgorithm(string, object[])"/> returns true,
        /// <see cref="ICryptoProvider.Create(string, object[])"/> is called to obtain the <see cref="KeyWrapProvider"/>.
        /// </para>
        /// <para>Once done with the <see cref="KeyWrapProvider"/>, call <see cref="ReleaseKeyWrapProvider(KeyWrapProvider)"/>.</para>
        /// </remarks>
        /// <returns>An instance of <see cref="KeyWrapProvider"/>.</returns>
        public virtual KeyWrapProvider CreateKeyWrapProviderForUnwrap(SecurityKey key, string algorithm)
        {
            return CreateKeyWrapProvider(key, algorithm, true);
        }

        private KeyWrapProvider CreateKeyWrapProvider(SecurityKey key, string algorithm, bool willUnwrap)
        {
            if (key == null)
                throw LogHelper.LogArgumentNullException(nameof(key));

            if (string.IsNullOrEmpty(algorithm))
                throw LogHelper.LogArgumentNullException(nameof(algorithm));

            if (CustomCryptoProvider != null && CustomCryptoProvider.IsSupportedAlgorithm(algorithm, key, willUnwrap))
            {
                if (!(CustomCryptoProvider.Create(algorithm, key, willUnwrap) is KeyWrapProvider keyWrapProvider))
                    throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant(LogMessages.IDX10646, LogHelper.MarkAsNonPII(algorithm), key, LogHelper.MarkAsNonPII(typeof(SignatureProvider)))));

                return keyWrapProvider;
            }

            if (SupportedAlgorithms.IsSupportedRsaKeyWrap(algorithm, key))
                return new RsaKeyWrapProvider(key, algorithm, willUnwrap);

            if (SupportedAlgorithms.IsSupportedSymmetricKeyWrap(algorithm, key))
                return new SymmetricKeyWrapProvider(key, algorithm);

            throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant(LogMessages.IDX10661, LogHelper.MarkAsNonPII(algorithm), key)));
        }

        /// <summary>
        /// Creates a <see cref="SignatureProvider"/> for signing with the specified <paramref name="key"/> and <paramref name="algorithm"/>.
        /// </summary>
        /// <param name="key">The <see cref="SecurityKey"/> to use for signing.</param>
        /// <param name="algorithm">The algorithm to use for signing.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="key"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="algorithm"/> is null or empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <see cref="SecurityKey.KeySize"/> is too small.</exception>
        /// <exception cref="NotSupportedException">Thrown if <paramref name="key"/> is not assignable from <see cref="AsymmetricSecurityKey"/> or <see cref="SymmetricSecurityKey"/>.</exception>
        /// <exception cref="NotSupportedException">Thrown if the key or algorithm combination is not supported.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the type returned by <see cref="ICryptoProvider.Create(string, object[])"/> is not assignable to <see cref="SignatureProvider"/>.</exception>
        /// <remarks>
        /// <para>AsymmetricSignatureProviders require access to a PrivateKey for signing.</para>
        /// <para>Once done with the <see cref="SignatureProvider"/>, call <see cref="ReleaseSignatureProvider(SignatureProvider)"/>.</para>
        /// <para>If <see cref="CustomCryptoProvider"/> is set and <see cref="ICryptoProvider.IsSupportedAlgorithm(string, object[])"/> returns true,
        /// <see cref="ICryptoProvider.Create(string, object[])"/> is called to obtain the <see cref="SignatureProvider"/>.
        /// </para>
        /// </remarks>
        /// <returns>A <see cref="SignatureProvider"/> instance that can be used to create a signature.</returns>
        public virtual SignatureProvider CreateForSigning(SecurityKey key, string algorithm)
        {
            return CreateForSigning(key, algorithm, CacheSignatureProviders);
        }

        /// <summary>
        /// Creates a <see cref="SignatureProvider"/> for signing with the specified <paramref name="key"/> and <paramref name="algorithm"/>.
        /// </summary>
        /// <param name="key">The <see cref="SecurityKey"/> to use for signing.</param>
        /// <param name="algorithm">The algorithm to use for signing.</param>
        /// <param name="cacheProvider">Indicates whether the <see cref="SignatureProvider"/> should be cached for reuse.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="key"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="algorithm"/> is null or empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <see cref="SecurityKey.KeySize"/> is too small.</exception>
        /// <exception cref="NotSupportedException">Thrown if <paramref name="key"/> is not assignable from <see cref="AsymmetricSecurityKey"/> or <see cref="SymmetricSecurityKey"/>.</exception>
        /// <exception cref="NotSupportedException">Thrown if the combination of <paramref name="key"/> and <paramref name="algorithm"/> is not supported.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the type returned by <see cref="ICryptoProvider.Create(string, object[])"/> is not assignable to <see cref="SignatureProvider"/>.</exception>
        /// <remarks>
        /// <para>AsymmetricSignatureProviders require access to a PrivateKey for signing.</para>
        /// <para>Once done with the <see cref="SignatureProvider"/>, call <see cref="ReleaseSignatureProvider(SignatureProvider)"/>.</para>
        /// <para>If <see cref="CustomCryptoProvider"/> is set and <see cref="ICryptoProvider.IsSupportedAlgorithm(string, object[])"/> returns true,
        /// <see cref="ICryptoProvider.Create(string, object[])"/> is called to obtain the <see cref="SignatureProvider"/>.
        /// </para>
        /// </remarks>
        /// <returns>A <see cref="SignatureProvider"/> instance that can be used to create a signature.</returns>
        public virtual SignatureProvider CreateForSigning(SecurityKey key, string algorithm, bool cacheProvider)
        {
            return CreateSignatureProvider(key, algorithm, true, cacheProvider);
        }

        /// <summary>
        /// Creates a <see cref="SignatureProvider"/> for verifying signatures with the specified <paramref name="key"/> and <paramref name="algorithm"/>.
        /// </summary>
        /// <param name="key">The <see cref="SecurityKey"/> to use for signature verification.</param>
        /// <param name="algorithm">The algorithm to use for verifying signatures.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="key"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="algorithm"/> is null or empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <see cref="SecurityKey.KeySize"/> is too small.</exception>
        /// <exception cref="NotSupportedException">Thrown if <paramref name="key"/> is not assignable from <see cref="AsymmetricSecurityKey"/> or <see cref="SymmetricSecurityKey"/>.</exception>
        /// <exception cref="NotSupportedException">Thrown if the combination of <paramref name="key"/> and <paramref name="algorithm"/> is not supported.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the type returned by <see cref="ICryptoProvider.Create(string, object[])"/> is not assignable to <see cref="SignatureProvider"/>.</exception>
        /// <remarks>
        /// <para>Once done with the <see cref="SignatureProvider"/>, call <see cref="ReleaseSignatureProvider(SignatureProvider)"/>.</para>
        /// <para>If <see cref="CustomCryptoProvider"/> is set and <see cref="ICryptoProvider.IsSupportedAlgorithm(string, object[])"/> returns true,
        /// <see cref="ICryptoProvider.Create(string, object[])"/> is called to obtain the <see cref="SignatureProvider"/>.
        /// </para>
        /// </remarks>
        /// <returns>A <see cref="SignatureProvider"/> instance that can be used to validate signatures using the <see cref="SecurityKey"/> and algorithm.</returns>
        public virtual SignatureProvider CreateForVerifying(SecurityKey key, string algorithm)
        {
            return CreateForVerifying(key, algorithm, CacheSignatureProviders);
        }

        /// <summary>
        /// Creates a <see cref="SignatureProvider"/> for verifying signatures with the specified <paramref name="key"/> and <paramref name="algorithm"/>.
        /// </summary>
        /// <param name="key">The <see cref="SecurityKey"/> to use for signature verification.</param>
        /// <param name="algorithm">The algorithm to use for verifying signatures.</param>
        /// <param name="cacheProvider">Specifies whether the <see cref="SignatureProvider"/> should be cached for reuse.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="key"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="algorithm"/> is null or empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <see cref="SecurityKey.KeySize"/> is too small.</exception>
        /// <exception cref="NotSupportedException">Thrown if <paramref name="key"/> is not assignable from <see cref="AsymmetricSecurityKey"/> or <see cref="SymmetricSecurityKey"/>.</exception>
        /// <exception cref="NotSupportedException">Thrown if the combination of <paramref name="key"/> and <paramref name="algorithm"/> is not supported.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the type returned by <see cref="ICryptoProvider.Create(string, object[])"/> is not assignable to <see cref="SignatureProvider"/>.</exception>
        /// <remarks>
        /// <para>Once done with the <see cref="SignatureProvider"/>, call <see cref="ReleaseSignatureProvider(SignatureProvider)"/>.</para>
        /// <para>If <see cref="CustomCryptoProvider"/> is set and <see cref="ICryptoProvider.IsSupportedAlgorithm(string, object[])"/> returns true,
        /// <see cref="ICryptoProvider.Create(string, object[])"/> is called to obtain the <see cref="SignatureProvider"/>.
        /// </para>
        /// </remarks>
        /// <returns>A <see cref="SignatureProvider"/> instance that can be used to validate signatures using the <see cref="SecurityKey"/> and algorithm.</returns>
        public virtual SignatureProvider CreateForVerifying(SecurityKey key, string algorithm, bool cacheProvider)
        {
            return CreateSignatureProvider(key, algorithm, false, cacheProvider);
        }

        /// <summary>
        /// Creates a <see cref="HashAlgorithm"/> instance for a specific hash algorithm.
        /// </summary>
        /// <param name="algorithm">The name of the hash algorithm to create.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="algorithm"/> is null or empty.</exception>
        /// <exception cref="InvalidOperationException">Thrown if <see cref="ICryptoProvider.Create(string, object[])"/> returns a type that is not assignable to <see cref="HashAlgorithm"/>.</exception>
        /// <exception cref="NotSupportedException">Thrown if <paramref name="algorithm"/> is not supported.</exception>
        /// <remarks>
        /// <para>Once done with the <see cref="HashAlgorithm"/>, call <see cref="ReleaseHashAlgorithm(HashAlgorithm)"/>.</para>
        /// <para>If <see cref="CustomCryptoProvider"/> is set and <see cref="ICryptoProvider.IsSupportedAlgorithm(string, object[])"/> returns true,
        /// <see cref="ICryptoProvider.Create(string, object[])"/> is called to obtain the <see cref="HashAlgorithm"/>.
        /// </para>
        /// </remarks>
        /// <returns>A <see cref="HashAlgorithm"/> instance that corresponds to the specified <paramref name="algorithm"/>.</returns>
        public virtual HashAlgorithm CreateHashAlgorithm(HashAlgorithmName algorithm)
        {
            if (CustomCryptoProvider != null && CustomCryptoProvider.IsSupportedAlgorithm(algorithm.Name))
            {
                if (!(CustomCryptoProvider.Create(algorithm.Name) is HashAlgorithm hashAlgorithm))
                    throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant(LogMessages.IDX10647, LogHelper.MarkAsNonPII(algorithm), LogHelper.MarkAsNonPII(typeof(HashAlgorithm)))));

                _typeToAlgorithmMap[hashAlgorithm.GetType().ToString()] = algorithm.Name;
                return hashAlgorithm;
            }

            if (algorithm == HashAlgorithmName.SHA256)
                return SHA256.Create();

            if (algorithm == HashAlgorithmName.SHA384)
                return SHA384.Create();

            if (algorithm == HashAlgorithmName.SHA512)
                return SHA512.Create();

            throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant(LogMessages.IDX10640, LogHelper.MarkAsNonPII(algorithm))));
        }

        /// <summary>
        /// Creates a <see cref="HashAlgorithm"/> instance for a specific hash algorithm.
        /// </summary>
        /// <param name="algorithm">The name of the hash algorithm to create.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="algorithm"/> is null or empty.</exception>
        /// <exception cref="InvalidOperationException">Thrown if <see cref="ICryptoProvider.Create(string, object[])"/> returns a type that is not assignable to <see cref="HashAlgorithm"/>.</exception>
        /// <exception cref="NotSupportedException">Thrown if <paramref name="algorithm"/> is not supported.</exception>
        /// <remarks>
        /// Once done with the <see cref="HashAlgorithm"/>, call <see cref="ReleaseHashAlgorithm(HashAlgorithm)"/>.
        /// <para>If <see cref="CustomCryptoProvider"/> is set and <see cref="ICryptoProvider.IsSupportedAlgorithm(string, object[])"/> returns true,
        /// <see cref="ICryptoProvider.Create(string, object[])"/> is called to obtain the <see cref="HashAlgorithm"/>.
        /// </para>
        /// </remarks>
        /// <returns>A <see cref="HashAlgorithm"/> instance that corresponds to the specified <paramref name="algorithm"/>.</returns>
        public virtual HashAlgorithm CreateHashAlgorithm(string algorithm)
        {
            if (string.IsNullOrEmpty(algorithm))
                throw LogHelper.LogArgumentNullException(nameof(algorithm));

            if (CustomCryptoProvider != null && CustomCryptoProvider.IsSupportedAlgorithm(algorithm))
            {
                if (!(CustomCryptoProvider.Create(algorithm) is HashAlgorithm hashAlgorithm))
                    throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant(LogMessages.IDX10647, LogHelper.MarkAsNonPII(algorithm), LogHelper.MarkAsNonPII(typeof(HashAlgorithm)))));

                _typeToAlgorithmMap[hashAlgorithm.GetType().ToString()] = algorithm;

                return hashAlgorithm;
            }

            switch (algorithm)
            {
                case SecurityAlgorithms.Sha256:
                case SecurityAlgorithms.Sha256Digest:
                    return SHA256.Create();

                case SecurityAlgorithms.Sha384:
                case SecurityAlgorithms.Sha384Digest:
                    return SHA384.Create();

                case SecurityAlgorithms.Sha512:
                case SecurityAlgorithms.Sha512Digest:
                    return SHA512.Create();
            }

            throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant(LogMessages.IDX10640, LogHelper.MarkAsNonPII(algorithm))));
        }

        /// <summary>
        /// Creates a <see cref="KeyedHashAlgorithm"/> instance for a specific keyed hash algorithm.
        /// </summary>
        /// <param name="keyBytes">The bytes to use as the key for the keyed hash.</param>
        /// <param name="algorithm">The name of the keyed hash algorithm to create.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="keyBytes"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="algorithm"/> is null or empty.</exception>
        /// <exception cref="InvalidOperationException">Thrown if <see cref="ICryptoProvider.Create(string, object[])"/> returns a type that is not assignable to <see cref="KeyedHashAlgorithm"/>.</exception>
        /// <exception cref="NotSupportedException">Thrown if <paramref name="algorithm"/> is not supported.</exception>
        /// <remarks>
        /// Once done with the <see cref="KeyedHashAlgorithm"/>, call <see cref="ReleaseHashAlgorithm(HashAlgorithm)"/>.
        /// <para>If <see cref="CustomCryptoProvider"/> is set and <see cref="ICryptoProvider.IsSupportedAlgorithm(string, object[])"/> returns true,
        /// <see cref="ICryptoProvider.Create(string, object[])"/> is called to obtain the <see cref="KeyedHashAlgorithm"/>.
        /// </para>
        /// </remarks>
        /// <returns>A <see cref="KeyedHashAlgorithm"/> instance that corresponds to the specified <paramref name="algorithm"/>.</returns>
        public virtual KeyedHashAlgorithm CreateKeyedHashAlgorithm(byte[] keyBytes, string algorithm)
        {
            if (keyBytes == null)
                throw LogHelper.LogArgumentNullException(nameof(keyBytes));

            if (string.IsNullOrEmpty(algorithm))
                throw LogHelper.LogArgumentNullException(nameof(algorithm));

            if (CustomCryptoProvider != null && CustomCryptoProvider.IsSupportedAlgorithm(algorithm, keyBytes))
            {
                if (!(CustomCryptoProvider.Create(algorithm, keyBytes) is KeyedHashAlgorithm keyedHashAlgorithm))
                    throw LogHelper.LogExceptionMessage(
                        new InvalidOperationException(
                            LogHelper.FormatInvariant(
                                LogMessages.IDX10647,
                                LogHelper.MarkAsNonPII(algorithm),
                                LogHelper.MarkAsNonPII(typeof(KeyedHashAlgorithm)))));

                return keyedHashAlgorithm;
            }

            // In the case of Aes128CbcHmacSha256, Aes192CbcHmacSha384, Aes256CbcHmacSha512 which are Authenticated Encryption algorithms
            // SymmetricSignatureProvider will get passed a key with 1/2 the minimum keysize expected size for the HashAlgorithm. 16 bytes for SHA256, instead of 32 bytes.
            // see: https://datatracker.ietf.org/doc/html/rfc7518#section-5.2.2.1
            switch (algorithm)
            {
                case SecurityAlgorithms.Aes128CbcHmacSha256:
                {
                    ValidateKeySize(keyBytes, algorithm, 16);
                    return new HMACSHA256(keyBytes);
                }

                case SecurityAlgorithms.Aes192CbcHmacSha384:
                {
                    ValidateKeySize(keyBytes, algorithm, 24);
                    return new HMACSHA384(keyBytes);
                }

                case SecurityAlgorithms.Aes256CbcHmacSha512:
                {
                    ValidateKeySize(keyBytes, algorithm, 32);
                    return new HMACSHA512(keyBytes);
                }

                case SecurityAlgorithms.HmacSha256Signature:
                case SecurityAlgorithms.HmacSha256:
                {
                    ValidateKeySize(keyBytes, algorithm, 32);
                    return new HMACSHA256(keyBytes);
                }

                case SecurityAlgorithms.HmacSha384Signature:
                case SecurityAlgorithms.HmacSha384:
                {
                    ValidateKeySize(keyBytes, algorithm, 48);
                    return new HMACSHA384(keyBytes);
                }

                case SecurityAlgorithms.HmacSha512Signature:
                case SecurityAlgorithms.HmacSha512:
                {
                    ValidateKeySize(keyBytes, algorithm, 64);
                    return new HMACSHA512(keyBytes);
                }

                default:
                    throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant(LogMessages.IDX10666, LogHelper.MarkAsNonPII(algorithm))));
            }
        }

        private static void ValidateKeySize(byte[] keyBytes, string algorithm, int expectedNumberOfBytes)
        {
            if (keyBytes.Length < expectedNumberOfBytes)
                throw LogHelper.LogExceptionMessage(
                    new ArgumentOutOfRangeException(
                        nameof(keyBytes),
                        LogHelper.FormatInvariant(LogMessages.IDX10720,
                            LogHelper.MarkAsNonPII(algorithm),
                            LogHelper.MarkAsNonPII(expectedNumberOfBytes * 8),
                            LogHelper.MarkAsNonPII(keyBytes.Length * 8))));
        }

        private SignatureProvider CreateSignatureProvider(SecurityKey key, string algorithm, bool willCreateSignatures, bool cacheProvider)
        {
            if (key == null)
                throw LogHelper.LogArgumentNullException(nameof(key));

            if (string.IsNullOrEmpty(algorithm))
                throw LogHelper.LogArgumentNullException(nameof(algorithm));

            SignatureProvider signatureProvider;
            if (CustomCryptoProvider != null && CustomCryptoProvider.IsSupportedAlgorithm(algorithm, key, willCreateSignatures))
            {
                signatureProvider = CustomCryptoProvider.Create(algorithm, key, willCreateSignatures) as SignatureProvider;
                if (signatureProvider == null)
                    throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant(LogMessages.IDX10646, LogHelper.MarkAsNonPII(algorithm), key, LogHelper.MarkAsNonPII(typeof(SignatureProvider)))));

                return signatureProvider;
            }

            // types are checked in order of expected occurrence
            string typeofSignatureProvider = null;
            bool createAsymmetric = true;
            if (key is AsymmetricSecurityKey)
            {
                typeofSignatureProvider = typeof(AsymmetricSignatureProvider).ToString();
            }
            else if (key is JsonWebKey jsonWebKey)
            {
                try
                {
                    if (JsonWebKeyConverter.TryConvertToSecurityKey(jsonWebKey, out SecurityKey convertedSecurityKey))
                    {
                        if (convertedSecurityKey is AsymmetricSecurityKey)
                        {
                            typeofSignatureProvider = typeof(AsymmetricSignatureProvider).ToString();
                        }
                        else if (convertedSecurityKey is SymmetricSecurityKey)
                        {
                            typeofSignatureProvider = typeof(SymmetricSignatureProvider).ToString();
                            createAsymmetric = false;
                        }
                    }
                    // this code is simply to maintain the same exception thrown
                    else
                    {
                        if (jsonWebKey.Kty == JsonWebAlgorithmsKeyTypes.RSA || jsonWebKey.Kty == JsonWebAlgorithmsKeyTypes.EllipticCurve)
                            typeofSignatureProvider = typeof(AsymmetricSignatureProvider).ToString();
                        else if (jsonWebKey.Kty == JsonWebAlgorithmsKeyTypes.Octet)
                        {
                            typeofSignatureProvider = typeof(SymmetricSignatureProvider).ToString();
                            createAsymmetric = false;
                        }
                    }
                }
                catch(Exception ex)
                {
                    throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant(LogMessages.IDX10694, key, ex), ex));
                }
            }
            else if (key is SymmetricSecurityKey)
            {
                typeofSignatureProvider = typeof(SymmetricSignatureProvider).ToString();
                createAsymmetric = false;
            }

            if (typeofSignatureProvider == null)
                throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant(LogMessages.IDX10621, LogHelper.MarkAsNonPII(typeof(SymmetricSignatureProvider)), LogHelper.MarkAsNonPII(typeof(SecurityKey)), LogHelper.MarkAsNonPII(typeof(AsymmetricSecurityKey)), LogHelper.MarkAsNonPII(typeof(SymmetricSecurityKey)), LogHelper.MarkAsNonPII(key.GetType()))));

            if (CacheSignatureProviders && cacheProvider)
            {
                if (CryptoProviderCache.TryGetSignatureProvider(key, algorithm, typeofSignatureProvider, willCreateSignatures, out signatureProvider))
                {
                    signatureProvider.AddRef();
                    return signatureProvider;
                }

                lock (_cacheLock)
                {
                    if (CryptoProviderCache.TryGetSignatureProvider(key, algorithm, typeofSignatureProvider, willCreateSignatures, out signatureProvider))
                    {
                        signatureProvider.AddRef();
                        return signatureProvider;
                    }

                    if (!IsSupportedAlgorithm(algorithm, key))
                        throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant(LogMessages.IDX10634, LogHelper.MarkAsNonPII(algorithm), key)));

                    if (createAsymmetric)
                        signatureProvider = new AsymmetricSignatureProvider(key, algorithm, willCreateSignatures, this);
                    else
                        signatureProvider = new SymmetricSignatureProvider(key, algorithm, willCreateSignatures);

                    if (ShouldCacheSignatureProvider(signatureProvider))
                        CryptoProviderCache.TryAdd(signatureProvider);
                }
            }
            else
            {
                if (!IsSupportedAlgorithm(algorithm, key))
                    throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant(LogMessages.IDX10634, LogHelper.MarkAsNonPII(algorithm), key)));

                if (createAsymmetric)
                {
                    signatureProvider = new AsymmetricSignatureProvider(key, algorithm, willCreateSignatures);
                }
                else
                {
                    signatureProvider = new SymmetricSignatureProvider(key, algorithm, willCreateSignatures);
                }
            }

            return signatureProvider;
        }

        /// <summary>
        /// For some security key types, in some runtimes, it's not possible to extract public key material and create an <see cref="SecurityKey.InternalId"/>.
        /// In these cases, <see cref="SecurityKey.InternalId"/> will be an empty string, and these keys should not be cached.
        /// </summary>
        /// <param name="signatureProvider"><see cref="SignatureProvider"/> to be examined.</param>
        /// <returns><see langword="true"/> if <paramref name="signatureProvider"/> should be cached; otherwise, <see langword="false"/>.</returns>
        internal static bool ShouldCacheSignatureProvider(SignatureProvider signatureProvider)
        {
            _ = signatureProvider ?? throw new ArgumentNullException(nameof(signatureProvider));
            return signatureProvider.Key.InternalId.Length != 0;
        }

        /// <summary>
        /// Determines whether the specified hash algorithm is supported.
        /// </summary>
        /// <param name="algorithm">The name of the hash algorithm.</param>
        /// <remarks>Considers only known hash algorithms.</remarks>
        /// <returns>
        /// <see langword="true"/> if:
        /// <list type="bullet">
        /// <item><description><see cref="CustomCryptoProvider"/> is set and <see cref="ICryptoProvider.IsSupportedAlgorithm(string, object[])"/> returns true,</description></item>
        /// <item><description>The algorithm is supported.</description></item>
        /// </list>
        /// Otherwise, <see langword="false"/>.
        /// </returns>
        public virtual bool IsSupportedAlgorithm(string algorithm)
        {
            if (CustomCryptoProvider != null && CustomCryptoProvider.IsSupportedAlgorithm(algorithm))
                return true;

            return SupportedAlgorithms.IsSupportedHashAlgorithm(algorithm);
        }

        /// <summary>
        /// Checks if the specified algorithm and <see cref="SecurityKey"/> are supported.
        /// </summary>
        /// <param name="algorithm">The security algorithm to be used.</param>
        /// <param name="key">The <see cref="SecurityKey"/>.</param>
        /// <remarks>
        /// Algorithms are supported for specific key types.
        /// For example:
        /// <list type="bullet">
        /// <item><description><see cref="SecurityAlgorithms.RsaSha256"/> and <see cref="RsaSecurityKey"/> will return true.</description></item>
        /// <item><description><see cref="SecurityAlgorithms.RsaSha256"/> and <see cref="SymmetricSecurityKey"/> will return false.</description></item>
        /// </list>
        /// </remarks>
        /// <returns>
        /// <see langword="true"/> if:
        /// <list type="bullet">
        /// <item><description><see cref="CustomCryptoProvider"/> is set and <see cref="ICryptoProvider.IsSupportedAlgorithm(string, object[])"/> returns true,</description></item>
        /// <item><description>The algorithm / key pair is supported.</description></item>
        /// </list>
        /// Otherwise, <see langword="false"/>.
        /// </returns>
        public virtual bool IsSupportedAlgorithm(string algorithm, SecurityKey key)
        {
            if (CustomCryptoProvider != null && CustomCryptoProvider.IsSupportedAlgorithm(algorithm, key))
                return true;

            return SupportedAlgorithms.IsSupportedAlgorithm(
                        algorithm,
                        (key is JsonWebKey jsonWebKey && jsonWebKey.ConvertedSecurityKey != null)
                        ? jsonWebKey.ConvertedSecurityKey
                        : key);
        }

        /// <summary>
        /// Releases resources associated with a <see cref="HashAlgorithm"/> instance. The default behavior is to call <see cref="HashAlgorithm.Dispose()"/>.
        /// </summary>
        /// <param name="hashAlgorithm">The <see cref="HashAlgorithm"/> instance to release.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="hashAlgorithm"/> is null.</exception>
        public virtual void ReleaseHashAlgorithm(HashAlgorithm hashAlgorithm)
        {
            if (hashAlgorithm == null)
                throw LogHelper.LogArgumentNullException(nameof(hashAlgorithm));
            else if (CustomCryptoProvider != null && _typeToAlgorithmMap.TryGetValue(hashAlgorithm.GetType().ToString(), out var algorithm) && CustomCryptoProvider.IsSupportedAlgorithm(algorithm))
                CustomCryptoProvider.Release(hashAlgorithm);
            else
                hashAlgorithm.Dispose();
        }

        /// <summary>
        /// Releases resources associated with a <see cref="KeyWrapProvider"/> instance.
        /// </summary>
        /// <param name="provider">The <see cref="KeyWrapProvider"/> instance to release.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="provider"/> is null.</exception>
        public virtual void ReleaseKeyWrapProvider(KeyWrapProvider provider)
        {
            if (provider == null)
                throw LogHelper.LogArgumentNullException(nameof(provider));
            else if (CustomCryptoProvider != null && CustomCryptoProvider.IsSupportedAlgorithm(provider.Algorithm))
                CustomCryptoProvider.Release(provider);
            else
                provider.Dispose();
        }

        /// <summary>
        /// Releases resources associated with an <see cref="RsaKeyWrapProvider"/> instance.
        /// </summary>
        /// <param name="provider">The <see cref="RsaKeyWrapProvider"/> instance to release.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="provider"/> is null.</exception>
        public virtual void ReleaseRsaKeyWrapProvider(RsaKeyWrapProvider provider)
        {
            if (provider == null)
                throw LogHelper.LogArgumentNullException(nameof(provider));
            else if (CustomCryptoProvider != null && CustomCryptoProvider.IsSupportedAlgorithm(provider.Algorithm))
                CustomCryptoProvider.Release(provider);
            else
                provider.Dispose();
        }

        /// <summary>
        /// Releases resources associated with a <see cref="SignatureProvider"/> instance.
        /// </summary>
        /// <param name="signatureProvider">The <see cref="SignatureProvider"/> instance to release.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="signatureProvider"/> is null.</exception>
        public virtual void ReleaseSignatureProvider(SignatureProvider signatureProvider)
        {
            if (signatureProvider == null)
                throw LogHelper.LogArgumentNullException(nameof(signatureProvider));

            signatureProvider.Release();
            if (CustomCryptoProvider != null && CustomCryptoProvider.IsSupportedAlgorithm(signatureProvider.Algorithm))
                CustomCryptoProvider.Release(signatureProvider);
            else if (signatureProvider.CryptoProviderCache == null && signatureProvider.RefCount == 0)
                signatureProvider.Dispose();
        }
    }
}
