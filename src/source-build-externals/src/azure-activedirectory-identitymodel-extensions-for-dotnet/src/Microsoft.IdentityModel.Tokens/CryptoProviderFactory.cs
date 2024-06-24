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
        /// Default constructor for <see cref="CryptoProviderFactory"/>.
        /// </summary>
        public CryptoProviderFactory()
        {
            CryptoProviderCache = new InMemoryCryptoProviderCache() { CryptoProviderFactory = this };
        }

        /// <summary>
        /// Initializes an instance of a <see cref="CryptoProviderFactory"/>.
        /// </summary>
        /// <param name="cache">
        /// The cache to use for caching CryptoProviders
        /// </param>
        public CryptoProviderFactory(CryptoProviderCache cache)
        {
            CryptoProviderCache = cache ?? throw LogHelper.LogArgumentNullException(nameof(cache));
        }

        /// <summary>
        /// Constructor that creates a deep copy of given <see cref="CryptoProviderFactory"/> object.
        /// </summary>
        /// <param name="other"><see cref="CryptoProviderFactory"/> to copy from.</param>
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
        /// Gets the <see cref="CryptoProviderCache"/>
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
        /// Creates an instance of <see cref="AuthenticatedEncryptionProvider"/> for a specific &lt;SecurityKey, Algorithm>.
        /// </summary>
        /// <param name="key">the <see cref="SecurityKey"/> to use.</param>
        /// <param name="algorithm">the algorithm to use.</param>
        /// <exception cref="ArgumentNullException">thrown if <paramref name="key"/> is null.</exception>
        /// <exception cref="ArgumentNullException">thrown if <paramref name="algorithm"/> is null or empty.</exception>
        /// <exception cref="ArgumentException">thrown if <see cref="SecurityKey"/> and algorithm pair are not supported.</exception>
        /// <exception cref="InvalidOperationException">thrown if <see cref="ICryptoProvider.Create(string, object[])"/> returns a type that is not assignable from <see cref="KeyWrapProvider"/>.</exception>
        /// <remarks>
        /// <para>If <see cref="CustomCryptoProvider"/> is set and <see cref="ICryptoProvider.IsSupportedAlgorithm(string, object[])"/> returns true.
        /// <see cref="ICryptoProvider.Create(string, object[])"/> is called to obtain the <see cref="AuthenticatedEncryptionProvider"/>.
        /// </para>
        /// <para>When finished with the <see cref="KeyWrapProvider"/> call <see cref="ReleaseKeyWrapProvider(KeyWrapProvider)"/>.</para>
        /// </remarks>
        /// <returns>an instance of <see cref="AuthenticatedEncryptionProvider"/></returns>
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
        /// Creates an instance of <see cref="KeyWrapProvider"/> for a specific &lt;SecurityKey, Algorithm>.
        /// </summary>
        /// <param name="key">the <see cref="SecurityKey"/> to use.</param>
        /// <param name="algorithm">the algorithm to use.</param>
        /// <exception cref="ArgumentNullException">thrown if <paramref name="key"/> is null.</exception>
        /// <exception cref="ArgumentNullException">thrown if <paramref name="algorithm"/> is null or empty.</exception>
        /// <exception cref="NotSupportedException">thrown if <see cref="SecurityKey"/> and algorithm pair are not supported.</exception>
        /// <exception cref="InvalidOperationException">thrown if <see cref="ICryptoProvider.Create(string, object[])"/> returns a type not assignable from <see cref="KeyWrapProvider"/>.</exception>
        /// <remarks>
        /// <para>If <see cref="CustomCryptoProvider"/> is set and <see cref="ICryptoProvider.IsSupportedAlgorithm(string, object[])"/> returns true.
        /// <see cref="ICryptoProvider.Create(string, object[])"/> is called to obtain the <see cref="KeyWrapProvider"/>.
        /// </para>
        /// <para>When finished with the <see cref="KeyWrapProvider"/> call <see cref="ReleaseKeyWrapProvider(KeyWrapProvider)"/>.</para>
        /// </remarks>
        /// <returns>an instance of <see cref="KeyWrapProvider"/></returns>
        public virtual KeyWrapProvider CreateKeyWrapProvider(SecurityKey key, string algorithm)
        {
            return CreateKeyWrapProvider(key, algorithm, false);
        }

        /// <summary>
        /// Creates an instance of <see cref="KeyWrapProvider"/> for a specific &lt;SecurityKey, Algorithm>.
        /// </summary>
        /// <param name="key">the <see cref="SecurityKey"/> to use.</param>
        /// <param name="algorithm">the algorithm to use.</param>
        /// <exception cref="ArgumentNullException">thrown if <paramref name="key"/> is null.</exception>
        /// <exception cref="ArgumentNullException">thrown if <paramref name="algorithm"/> is null or empty.</exception>
        /// <exception cref="ArgumentException">thrown if <see cref="SecurityKey"/> and algorithm pair are not supported.</exception>
        /// <exception cref="InvalidOperationException">thrown if <see cref="ICryptoProvider.Create(string, object[])"/> returns a type that is not assignable from <see cref="KeyWrapProvider"/>.</exception>
        /// <remarks>
        /// <para>If <see cref="CustomCryptoProvider"/> is set and <see cref="ICryptoProvider.IsSupportedAlgorithm(string, object[])"/> returns true.
        /// <see cref="ICryptoProvider.Create(string, object[])"/> is called to obtain the <see cref="AuthenticatedEncryptionProvider"/>.
        /// </para>
        /// <para>When finished with the <see cref="KeyWrapProvider"/> call <see cref="ReleaseKeyWrapProvider(KeyWrapProvider)"/>.</para>
        /// </remarks>
        /// <returns>an instance of <see cref="KeyWrapProvider"/></returns>
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
        /// Creates a <see cref="SignatureProvider"/> that creates a signature with the algorithm and <see cref="SecurityKey"/>.
        /// </summary>
        /// <param name="key">the <see cref="SecurityKey"/> to use for signing.</param>
        /// <param name="algorithm">the algorithm to use for signing.</param>
        /// <exception cref="ArgumentNullException">thrown if <paramref name="key"/> is null.</exception>
        /// <exception cref="ArgumentNullException">thrown if <paramref name="algorithm"/> is null or empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">thrown if <see cref="SecurityKey.KeySize"/> is too small.</exception>
        /// <exception cref="NotSupportedException">thrown if <see cref="SecurityKey"/> is not assignable from <see cref="AsymmetricSecurityKey"/> or <see cref="SymmetricSecurityKey"/>.</exception>
        /// <exception cref="NotSupportedException">thrown if the key / algorithm is not supported.</exception>
        /// <exception cref="InvalidOperationException">thrown if <see cref="ICryptoProvider.Create(string, object[])"/> returns a type that is not assignable from <see cref="SignatureProvider"/>.</exception>
        /// <remarks>
        /// <para>AsymmetricSignatureProviders require access to a PrivateKey for Signing.</para>
        /// <para>When finished with the <see cref="SignatureProvider"/> call <see cref="ReleaseSignatureProvider(SignatureProvider)"/>.</para>
        /// <para>If <see cref="CustomCryptoProvider"/> is set and <see cref="ICryptoProvider.IsSupportedAlgorithm(string, object[])"/> returns true.
        /// <see cref="ICryptoProvider.Create(string, object[])"/> is called to obtain the <see cref="SignatureProvider"/>.
        /// </para>
        /// </remarks>
        /// <returns>A <see cref="SignatureProvider"/> that can be used to create a signature using the <see cref="SecurityKey"/> and algorithm.</returns>
        public virtual SignatureProvider CreateForSigning(SecurityKey key, string algorithm)
        {
            return CreateForSigning(key, algorithm, CacheSignatureProviders);
        }

        /// <summary>
        /// Creates a <see cref="SignatureProvider"/> that creates a signature with the algorithm and <see cref="SecurityKey"/>.
        /// </summary>
        /// <param name="key">the <see cref="SecurityKey"/> to use for signing.</param>
        /// <param name="algorithm">the algorithm to use for signing.</param>
        /// <param name="cacheProvider">indicates if the <see cref="SignatureProvider"/> should be cached for reuse.</param>
        /// <exception cref="ArgumentNullException">thrown if <paramref name="key"/> is null.</exception>
        /// <exception cref="ArgumentNullException">thrown if <paramref name="algorithm"/> is null or empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">thrown if <see cref="SecurityKey.KeySize"/> is too small.</exception>
        /// <exception cref="NotSupportedException">thrown if <see cref="SecurityKey"/> is not assignable from <see cref="AsymmetricSecurityKey"/> or <see cref="SymmetricSecurityKey"/>.</exception>
        /// <exception cref="NotSupportedException">thrown if the key / algorithm is not supported.</exception>
        /// <exception cref="InvalidOperationException">thrown if <see cref="ICryptoProvider.Create(string, object[])"/> returns a type that is not assignable from <see cref="SignatureProvider"/>.</exception>
        /// <remarks>
        /// <para>AsymmetricSignatureProviders require access to a PrivateKey for Signing.</para>
        /// <para>When finished with the <see cref="SignatureProvider"/> call <see cref="ReleaseSignatureProvider(SignatureProvider)"/>.</para>
        /// <para>If <see cref="CustomCryptoProvider"/> is set and <see cref="ICryptoProvider.IsSupportedAlgorithm(string, object[])"/> returns true.
        /// <see cref="ICryptoProvider.Create(string, object[])"/> is called to obtain the <see cref="SignatureProvider"/>.
        /// </para>
        /// </remarks>
        /// <returns>A <see cref="SignatureProvider"/> that can be used to create a signature using the <see cref="SecurityKey"/> and algorithm.</returns>
        public virtual SignatureProvider CreateForSigning(SecurityKey key, string algorithm, bool cacheProvider)
        {
            return CreateSignatureProvider(key, algorithm, true, cacheProvider);
        }

        /// <summary>
        /// Creates a <see cref="SignatureProvider"/> that supports the <see cref="SecurityKey"/> and algorithm.
        /// </summary>
        /// <param name="key">The <see cref="SecurityKey"/> to use for signature verification.</param>
        /// <param name="algorithm">The algorithm to use for verifying.</param>
        /// <exception cref="ArgumentNullException">thrown if <paramref name="key"/> is null.</exception>
        /// <exception cref="ArgumentNullException">thrown if <paramref name="algorithm"/> is null or empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">thrown if <see cref="SecurityKey.KeySize"/> is too small.</exception>
        /// <exception cref="NotSupportedException">thrown if <see cref="SecurityKey"/> is not assignable from <see cref="AsymmetricSecurityKey"/> or <see cref="SymmetricSecurityKey"/>.</exception>
        /// <exception cref="NotSupportedException">thrown if the key / algorithm is not supported.</exception>
        /// <exception cref="InvalidOperationException">thrown if <see cref="ICryptoProvider.Create(string, object[])"/> returns a type that is not assignable from <see cref="SignatureProvider"/>.</exception>
        /// <remarks>When finished with the <see cref="SignatureProvider"/> call <see cref="ReleaseSignatureProvider(SignatureProvider)"/>.
        /// <para>If <see cref="CustomCryptoProvider"/> is set and <see cref="ICryptoProvider.IsSupportedAlgorithm(string, object[])"/> returns true.
        /// <see cref="ICryptoProvider.Create(string, object[])"/> is called to obtain the <see cref="SignatureProvider"/>.
        /// </para>
        /// </remarks>
        /// <returns>A <see cref="SignatureProvider"/> that can be used to validate a signature using the <see cref="SecurityKey"/> and algorithm.</returns>
        public virtual SignatureProvider CreateForVerifying(SecurityKey key, string algorithm)
        {
            return CreateForVerifying(key, algorithm, CacheSignatureProviders);
        }

        /// <summary>
        /// Creates a <see cref="SignatureProvider"/> that supports the <see cref="SecurityKey"/> and algorithm.
        /// </summary>
        /// <param name="key">The <see cref="SecurityKey"/> to use for signature verification.</param>
        /// <param name="algorithm">The algorithm to use for verifying.</param>
        /// <param name="cacheProvider">should the <see cref="SignatureProvider"/> be cached.</param>
        /// <exception cref="ArgumentNullException">thrown if <paramref name="key"/> is null.</exception>
        /// <exception cref="ArgumentNullException">thrown if <paramref name="algorithm"/> is null or empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">thrown if <see cref="SecurityKey.KeySize"/> is too small.</exception>
        /// <exception cref="NotSupportedException">thrown if <see cref="SecurityKey"/> is not assignable from <see cref="AsymmetricSecurityKey"/> or <see cref="SymmetricSecurityKey"/>.</exception>
        /// <exception cref="NotSupportedException">thrown if the key / algorithm is not supported.</exception>
        /// <exception cref="InvalidOperationException">thrown if <see cref="ICryptoProvider.Create(string, object[])"/> returns a type that is not assignable from <see cref="SignatureProvider"/>.</exception>
        /// <remarks>When finished with the <see cref="SignatureProvider"/> call <see cref="ReleaseSignatureProvider(SignatureProvider)"/>.
        /// <para>If <see cref="CustomCryptoProvider"/> is set and <see cref="ICryptoProvider.IsSupportedAlgorithm(string, object[])"/> returns true.
        /// <see cref="ICryptoProvider.Create(string, object[])"/> is called to obtain the <see cref="SignatureProvider"/>.
        /// </para>
        /// </remarks>
        /// <returns>A <see cref="SignatureProvider"/> that can be used to validate a signature using the <see cref="SecurityKey"/> and algorithm.</returns>
        public virtual SignatureProvider CreateForVerifying(SecurityKey key, string algorithm, bool cacheProvider)
        {
            return CreateSignatureProvider(key, algorithm, false, cacheProvider);
        }

        /// <summary>
        /// Creates a <see cref="HashAlgorithm"/> for a specific algorithm.
        /// </summary>
        /// <param name="algorithm">the name of the hash algorithm to create.</param>
        /// <exception cref="ArgumentNullException">thrown if <paramref name="algorithm"/> is null or empty.</exception>
        /// <exception cref="InvalidOperationException">thrown if <see cref="ICryptoProvider.Create(string, object[])"/> returns a type that is not assignable from <see cref="HashAlgorithm"/>.</exception>
        /// <exception cref="NotSupportedException">thrown if <paramref name="algorithm"/> is not supported.</exception>
        /// <remarks>When finished with the <see cref="HashAlgorithm"/> call <see cref="ReleaseHashAlgorithm(HashAlgorithm)"/>.
        /// <para>If <see cref="CustomCryptoProvider"/> is set and <see cref="ICryptoProvider.IsSupportedAlgorithm(string, object[])"/> returns true.
        /// <see cref="ICryptoProvider.Create(string, object[])"/> is called to obtain the <see cref="HashAlgorithm"/>.
        /// </para>
        /// </remarks>
        /// <returns>A <see cref="HashAlgorithm"/>.</returns>
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
        /// Creates a <see cref="HashAlgorithm"/> for a specific algorithm.
        /// </summary>
        /// <param name="algorithm">the name of the hash algorithm to create.</param>
        /// <exception cref="ArgumentNullException">thrown if <paramref name="algorithm"/> is null or empty.</exception>
        /// <exception cref="InvalidOperationException">thrown if <see cref="ICryptoProvider.Create(string, object[])"/> returns a type that is not assignable from <see cref="HashAlgorithm"/>.</exception>
        /// <exception cref="NotSupportedException">thrown if <paramref name="algorithm"/> is not supported.</exception>
        /// <remarks>When finished with the <see cref="HashAlgorithm"/> call <see cref="ReleaseHashAlgorithm(HashAlgorithm)"/>.
        /// <para>If <see cref="CustomCryptoProvider"/> is set and <see cref="ICryptoProvider.IsSupportedAlgorithm(string, object[])"/> returns true.
        /// <see cref="ICryptoProvider.Create(string, object[])"/> is called to obtain the <see cref="HashAlgorithm"/>.
        /// </para>
        /// </remarks>
        /// <returns>A <see cref="HashAlgorithm"/>.</returns>
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
        /// Returns a <see cref="KeyedHashAlgorithm"/> for a specific algorithm.
        /// </summary>
        /// <param name="keyBytes">bytes to use to create the Keyed Hash.</param>
        /// <param name="algorithm">the name of the keyed hash algorithm to create.</param>
        /// <exception cref="ArgumentNullException">thrown if <paramref name="keyBytes"/> is null.</exception>
        /// <exception cref="ArgumentNullException">thrown if <paramref name="algorithm"/> is null or empty.</exception>
        /// <exception cref="InvalidOperationException">thrown if <see cref="ICryptoProvider.Create(string, object[])"/> returns a type that is not assignable from <see cref="KeyedHashAlgorithm"/>.</exception>
        /// <exception cref="NotSupportedException"><paramref name="algorithm"/> is not supported.</exception>
        /// <remarks>When finished with the <see cref="KeyedHashAlgorithm"/> call <see cref="ReleaseHashAlgorithm(HashAlgorithm)"/>.
        /// <para>If <see cref="CustomCryptoProvider"/> is set and <see cref="ICryptoProvider.IsSupportedAlgorithm(string, object[])"/> returns true.
        /// <see cref="ICryptoProvider.Create(string, object[])"/> is called to obtain the <see cref="KeyedHashAlgorithm"/>.
        /// </para>
        /// </remarks>
        /// <returns>A <see cref="KeyedHashAlgorithm"/>.</returns>
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
        /// <returns><c>True</c> if <paramref name="signatureProvider"/> should be cached, <c>false</c> otherwise.</returns>
        internal static bool ShouldCacheSignatureProvider(SignatureProvider signatureProvider)
        {
            _ = signatureProvider ?? throw new ArgumentNullException(nameof(signatureProvider));
            return signatureProvider.Key.InternalId.Length != 0;
        }

        /// <summary>
        /// Checks if an algorithm is supported.
        /// </summary>
        /// <param name="algorithm">the name of the Hash algorithm.</param>
        /// <remarks>Only considers known Hash algorithms.</remarks>
        /// <returns>true if:
        /// <para>
        /// If <see cref="CustomCryptoProvider"/> is set and <see cref="ICryptoProvider.IsSupportedAlgorithm(string, object[])"/> returns true.
        /// </para>
        /// <para>The algorithm is supported.
        /// </para>
        /// </returns>
        public virtual bool IsSupportedAlgorithm(string algorithm)
        {
            if (CustomCryptoProvider != null && CustomCryptoProvider.IsSupportedAlgorithm(algorithm))
                return true;

            return SupportedAlgorithms.IsSupportedHashAlgorithm(algorithm);
        }

        /// <summary>
        /// Checks if the algorithm and <see cref="SecurityKey"/> is supported.
        /// </summary>
        /// <param name="algorithm">the security algorithm to apply.</param>
        /// <param name="key">the <see cref="SecurityKey"/>.</param>
        /// <remarks>Algorithms are supported for specific key types.
        /// For example:
        /// <para><see cref="SecurityAlgorithms.RsaSha256"/> and <see cref="RsaSecurityKey"/> will return true.</para>
        /// <para><see cref="SecurityAlgorithms.RsaSha256"/> and <see cref="SymmetricSecurityKey"/> will return false.</para>
        /// </remarks>
        /// <returns>true if:
        /// <para>
        /// If <see cref="CustomCryptoProvider"/> is set and <see cref="ICryptoProvider.IsSupportedAlgorithm(string, object[])"/> returns true.
        /// </para>
        /// <para>The algorithm / key pair is supported.
        /// </para>
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
        /// When finished with a <see cref="HashAlgorithm"/> call this method for cleanup. The default behavior is to call <see cref="HashAlgorithm.Dispose()"/>
        /// </summary>
        /// <param name="hashAlgorithm"><see cref="HashAlgorithm"/> to be released.</param>
        /// <exception cref="ArgumentNullException">thrown if <paramref name="hashAlgorithm"/> is null.</exception>
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
        /// When finished with a <see cref="KeyWrapProvider"/> call this method for cleanup."/>
        /// </summary>
        /// <param name="provider"><see cref="KeyWrapProvider"/> to be released.</param>
        /// <exception cref="ArgumentNullException">thrown if <paramref name="provider"/> is null.</exception>
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
        /// When finished with a <see cref="RsaKeyWrapProvider"/> call this method for cleanup."/>
        /// </summary>
        /// <param name="provider"><see cref="RsaKeyWrapProvider"/> to be released.</param>
        /// <exception cref="ArgumentNullException">thrown if <paramref name="provider"/> is null.</exception>
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
        /// When finished with a <see cref="SignatureProvider"/> call this method for cleanup. The default behavior is to call <see cref="SignatureProvider.Dispose()"/>
        /// </summary>
        /// <param name="signatureProvider"><see cref="SignatureProvider"/> to be released.</param>
        /// <exception cref="ArgumentNullException">thrown if <paramref name="signatureProvider"/> is null.</exception>
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
