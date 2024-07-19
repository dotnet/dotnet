// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Defines a cache for crypto providers.
    /// Current support is limited to <see cref="SignatureProvider"/> only.
    /// </summary>
    public class InMemoryCryptoProviderCache: CryptoProviderCache, IDisposable

    {
        internal CryptoProviderCacheOptions _cryptoProviderCacheOptions;
        private bool _disposed = false;
        private readonly EventBasedLRUCache<string, SignatureProvider> _signingSignatureProviders;
        private readonly EventBasedLRUCache<string, SignatureProvider> _verifyingSignatureProviders;

        /// <summary>
        /// Creates a new instance of <see cref="InMemoryCryptoProviderCache"/> using the default <see cref="CryptoProviderCacheOptions"/>.
        /// </summary>
        public InMemoryCryptoProviderCache() : this(new CryptoProviderCacheOptions())
        {
        }

        internal CryptoProviderFactory CryptoProviderFactory { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="InMemoryCryptoProviderCache"/> using the specified <paramref name="cryptoProviderCacheOptions"/>.
        /// </summary>
        /// <param name="cryptoProviderCacheOptions">The options used to configure the <see cref="InMemoryCryptoProviderCache"/>.</param>
        public InMemoryCryptoProviderCache(CryptoProviderCacheOptions cryptoProviderCacheOptions)
        {
            if (cryptoProviderCacheOptions == null)
                throw LogHelper.LogArgumentNullException(nameof(cryptoProviderCacheOptions));

            _cryptoProviderCacheOptions = cryptoProviderCacheOptions;
            _signingSignatureProviders = new EventBasedLRUCache<string, SignatureProvider>(cryptoProviderCacheOptions.SizeLimit, removeExpiredValues: false, comparer: StringComparer.Ordinal) { OnItemRemoved = (SignatureProvider signatureProvider) => signatureProvider.CryptoProviderCache = null };
            _verifyingSignatureProviders = new EventBasedLRUCache<string, SignatureProvider>(cryptoProviderCacheOptions.SizeLimit, removeExpiredValues: false, comparer: StringComparer.Ordinal) { OnItemRemoved = (SignatureProvider signatureProvider) => signatureProvider.CryptoProviderCache = null };
        }

        /// <summary>
        /// Creates a new instance of <see cref="InMemoryCryptoProviderCache"/> using the specified <paramref name="cryptoProviderCacheOptions"/>.
        /// </summary>
        /// <param name="cryptoProviderCacheOptions">The options used to configure the <see cref="InMemoryCryptoProviderCache"/>.</param>
        /// <param name="options">Options used to create the event queue thread.</param>
        /// <param name="tryTakeTimeout">The time used in ms for the timeout interval of the event queue. Defaults to 500 ms.</param>
        internal InMemoryCryptoProviderCache(CryptoProviderCacheOptions cryptoProviderCacheOptions, TaskCreationOptions options, int tryTakeTimeout = 500)
        {
            if (cryptoProviderCacheOptions == null)
                throw LogHelper.LogArgumentNullException(nameof(cryptoProviderCacheOptions));

            if (tryTakeTimeout <= 0)
                throw LogHelper.LogArgumentException<ArgumentException>(nameof(tryTakeTimeout), $"{nameof(tryTakeTimeout)} must be greater than zero");

            _cryptoProviderCacheOptions = cryptoProviderCacheOptions;
            _signingSignatureProviders = new EventBasedLRUCache<string, SignatureProvider>(cryptoProviderCacheOptions.SizeLimit, options, StringComparer.Ordinal, false) { OnItemRemoved = (SignatureProvider signatureProvider) => signatureProvider.CryptoProviderCache = null };
            _verifyingSignatureProviders = new EventBasedLRUCache<string, SignatureProvider>(cryptoProviderCacheOptions.SizeLimit, options, StringComparer.Ordinal, false) { OnItemRemoved = (SignatureProvider signatureProvider) => signatureProvider.CryptoProviderCache = null };
        }

        /// <summary>
        /// Returns the cache key to use when looking up an entry into the cache for a <see cref="SignatureProvider" />
        /// </summary>
        /// <param name="signatureProvider">the <see cref="SignatureProvider"/> to create the key for.</param>
        /// <exception cref="ArgumentNullException">if signatureProvider is null.</exception>
        /// <returns>the cache key to use for finding a <see cref="SignatureProvider"/>.</returns>
        protected override string GetCacheKey(SignatureProvider signatureProvider)
        {
            if (signatureProvider == null)
                throw LogHelper.LogArgumentNullException(nameof(signatureProvider));

            return GetCacheKeyPrivate(signatureProvider.Key, signatureProvider.Algorithm, signatureProvider.GetType().ToString());
        }

        /// <summary>
        /// Returns the 'key' that will be used to find a crypto provider in this cache.
        /// </summary>
        /// <param name="securityKey">the key that is used to by the crypto provider.</param>
        /// <param name="algorithm">the algorithm that is used by the crypto provider.</param>
        /// <param name="typeofProvider">the typeof the crypto provider obtained by calling object.GetType().</param>
        /// <exception cref="ArgumentNullException">if securityKey is null.</exception>
        /// <exception cref="ArgumentNullException">if algorithm is null or empty string.</exception>
        /// <exception cref="ArgumentNullException">if typeofProvider is null or empty string.</exception>
        /// <returns>the cache key to use for finding a crypto provider.</returns>
        protected override string GetCacheKey(SecurityKey securityKey, string algorithm, string typeofProvider)
        {
            if (securityKey == null)
                throw LogHelper.LogArgumentNullException(nameof(securityKey));

            if (string.IsNullOrEmpty(algorithm))
                throw LogHelper.LogArgumentNullException(nameof(algorithm));

            if (string.IsNullOrEmpty(typeofProvider))
                throw LogHelper.LogArgumentNullException(nameof(typeofProvider));

            return GetCacheKeyPrivate(securityKey, algorithm, typeofProvider);
        }

        private static string GetCacheKeyPrivate(SecurityKey securityKey, string algorithm, string typeofProvider)
        {
            return $"{securityKey.GetType()}-{securityKey.InternalId}-{algorithm}-{typeofProvider}";
        }

        /// <summary>
        /// Trys to adds a <see cref="SignatureProvider"/> to this cache.
        /// </summary>
        /// <param name="signatureProvider"><see cref="SignatureProvider"/> to cache.</param>
        /// <exception cref="ArgumentNullException">if signatureProvider is null.</exception>
        /// <returns>
        /// <c>true</c> if the <see cref="SignatureProvider"/> was added, <c>false</c> if the cache already contained the <see cref="SignatureProvider"/> or if <see cref="SignatureProvider"/> should not be cached.
        /// </returns>
        /// <remarks>if the <see cref="SignatureProvider"/> is added <see cref="SignatureProvider.CryptoProviderCache"/> will be set to 'this'.</remarks>
        public override bool TryAdd(SignatureProvider signatureProvider)
        {
            if (signatureProvider == null)
                throw LogHelper.LogArgumentNullException(nameof(signatureProvider));

            var cacheKey = GetCacheKey(signatureProvider);
            EventBasedLRUCache<string, SignatureProvider> signatureProviderCache;
            // Determine if we are caching a signing or a verifying SignatureProvider.
            if (signatureProvider.WillCreateSignatures)
                signatureProviderCache = _signingSignatureProviders;
            else
                signatureProviderCache = _verifyingSignatureProviders;

            // The cache does NOT already have a crypto provider associated with this key.
            if (!signatureProviderCache.Contains(cacheKey))
            {
                signatureProviderCache.SetValue(cacheKey, signatureProvider);
                signatureProvider.CryptoProviderCache = this;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Trys to find a <see cref="SignatureProvider"/> to this cache.
        /// </summary>
        /// <param name="securityKey">the key that is used to by the crypto provider.</param>
        /// <param name="algorithm">the algorithm that is used by the crypto provider.</param>
        /// <param name="typeofProvider">the typeof the crypto provider obtained by calling object.GetType().</param>
        /// <param name="willCreateSignatures">If true, the provider will be used for creating signatures.</param>
        /// <param name="signatureProvider">the <see cref="SignatureProvider"/> if found.</param>
        /// <exception cref="ArgumentNullException">if securityKey is null.</exception>
        /// <exception cref="ArgumentNullException">if algorithm is null or empty string.</exception>
        /// <exception cref="ArgumentNullException">if typeofProvider is null or empty string.</exception>
        /// <returns>true if a <see cref="SignatureProvider"/> was found, false otherwise.</returns>
        public override bool TryGetSignatureProvider(SecurityKey securityKey, string algorithm, string typeofProvider, bool willCreateSignatures, out SignatureProvider signatureProvider)
        {
            if (securityKey == null)
                throw LogHelper.LogArgumentNullException(nameof(securityKey));

            if (string.IsNullOrEmpty(algorithm))
                throw LogHelper.LogArgumentNullException(nameof(algorithm));

            if (string.IsNullOrEmpty(typeofProvider))
                throw LogHelper.LogArgumentNullException(nameof(typeofProvider));

            var cacheKey = GetCacheKeyPrivate(securityKey, algorithm, typeofProvider);
            if (willCreateSignatures)
                return _signingSignatureProviders.TryGetValue(cacheKey, out signatureProvider);
            else
                return _verifyingSignatureProviders.TryGetValue(cacheKey, out signatureProvider);
        }

        /// <summary>
        /// Trys to remove a <see cref="SignatureProvider"/> from this cache.
        /// </summary>
        /// <param name="signatureProvider"><see cref="SignatureProvider"/> to remove.</param>
        /// <exception cref="ArgumentNullException">if signatureProvider is null.</exception>
        /// <returns>true if the <see cref="SignatureProvider"/> was removed, false if the <see cref="SignatureProvider"/> was not found.</returns>
        /// <remarks>if the <see cref="SignatureProvider"/> is removed <see cref="SignatureProvider.CryptoProviderCache"/> will be set to null.</remarks>
        public override bool TryRemove(SignatureProvider signatureProvider)
        {
            if (signatureProvider == null)
                throw LogHelper.LogArgumentNullException(nameof(signatureProvider));

            if (!ReferenceEquals(signatureProvider.CryptoProviderCache, this))
                return false;

            var cacheKey = GetCacheKey(signatureProvider);
            EventBasedLRUCache<string, SignatureProvider> signatureProviderCache;

            // Determine if we are caching a signing or a verifying SignatureProvider.
            if (signatureProvider.WillCreateSignatures)
                signatureProviderCache = _signingSignatureProviders;
            else
                signatureProviderCache = _verifyingSignatureProviders;

            try
            {
                return signatureProviderCache.TryRemove(cacheKey, out SignatureProvider provider);
            }
            catch (Exception ex)
            {
                if (LogHelper.IsEnabled(EventLogLevel.Warning))
                    LogHelper.LogWarning(LogHelper.FormatInvariant(LogMessages.IDX10699, cacheKey, ex));

                return false;
            }
        }

        /// <summary>
        /// Calls <see cref="Dispose(bool)"/> and <see cref="GC.SuppressFinalize"/>
        /// Note: the EventBasedLRUCache is no longer being disposed of, but since this is a public class and can be used as base class of
        /// custom cache implementations, we need to keep it as some implementations may override Dispose().
        /// </summary>
        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// If <paramref name="disposing"/> is true, this method disposes of <see cref="_signingSignatureProviders"/> and <see cref="_verifyingSignatureProviders"/>.
        /// </summary>
        /// <param name="disposing">True if called from the <see cref="Dispose()"/> method, false otherwise.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                // Stop the event queue tasks if they are running.
                _signingSignatureProviders.StopEventQueueTask();
                _verifyingSignatureProviders.StopEventQueueTask();

                _disposed = true;
            }
        }

#region FOR TESTING (INTERNAL ONLY)
        /// <summary>
        /// FOR TESTING ONLY.
        /// </summary>
        internal long LinkedListCountSigning()
        {
            return _signingSignatureProviders.LinkedListCount;
        }

        /// <summary>
        /// FOR TESTING ONLY.
        /// </summary>
        internal long LinkedListCountVerifying()
        {
            return _verifyingSignatureProviders.LinkedListCount;
        }

        /// <summary>
        /// FOR TESTING ONLY.
        /// </summary>
        internal long MapCountSigning()
        {
            return _signingSignatureProviders.MapCount;
        }

        /// <summary>
        /// FOR TESTING ONLY.
        /// </summary>
        internal long MapCountVerifying()
        {
            return _verifyingSignatureProviders.MapCount;
        }

        /// <summary>
        /// FOR TESTING ONLY.
        /// </summary>
        internal long EventQueueCountSigning()
        {
            return _signingSignatureProviders.EventQueueCount;
        }

        /// <summary>
        /// FOR TESTING ONLY.
        /// </summary>
        internal long EventQueueCountVerifying()
        {
            return _verifyingSignatureProviders.EventQueueCount;
        }

        /// <summary>
        /// FOR TESTING PURPOSES ONLY.
        /// </summary>
        internal long TaskCount => _signingSignatureProviders.TaskCount + _verifyingSignatureProviders.TaskCount;

        #endregion
    }
}
