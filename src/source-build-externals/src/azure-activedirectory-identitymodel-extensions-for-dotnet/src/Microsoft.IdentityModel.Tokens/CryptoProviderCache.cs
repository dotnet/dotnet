// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Definition of cache for crypto providers
    /// </summary>
    public abstract class CryptoProviderCache
    {
        /// <summary>
        /// Returns the cache key to use when looking up an entry into the cache for a <see cref="SignatureProvider" />
        /// </summary>
        /// <param name="signatureProvider">the <see cref="SignatureProvider"/> to create the key for.</param>
        /// <returns>the cache key to use for finding a <see cref="SignatureProvider"/>.</returns>
        protected abstract string GetCacheKey(SignatureProvider signatureProvider);

        /// <summary>
        /// Returns the 'key' that will be used to find a crypto provider in this cache.
        /// </summary>
        /// <param name="securityKey">the key that is used to by the crypto provider.</param>
        /// <param name="algorithm">the algorithm that is used by the crypto provider.</param>
        /// <param name="typeofProvider">the typeof the crypto provider obtained by calling object.GetType().</param>
        /// <returns>the cache key to use for finding a crypto provider.</returns>
        protected abstract string GetCacheKey(SecurityKey securityKey, string algorithm, string typeofProvider);

        /// <summary>
        /// Trys to adds a <see cref="SignatureProvider"/> to this cache.
        /// </summary>
        /// <param name="signatureProvider"><see cref="SignatureProvider"/> to cache.</param>
        /// <returns>true if the <see cref="SignatureProvider"/> was added, false if the cache already contained the <see cref="SignatureProvider"/></returns>
        public abstract bool TryAdd(SignatureProvider signatureProvider);

        /// <summary>
        /// Trys to find a <see cref="SignatureProvider"/> in this cache.
        /// </summary>
        /// <param name="securityKey">the key that is used to by the crypto provider.</param>
        /// <param name="algorithm">the algorithm that is used by the crypto provider.</param>
        /// <param name="typeofProvider">the typeof the crypto provider obtained by calling object.GetType().</param>
        /// <param name="willCreateSignatures">a bool to indicate if the <see cref="SignatureProvider"/> will be used to sign.</param>
        /// <param name="signatureProvider">the <see cref="SignatureProvider"/> if found.</param>
        /// <returns>true if a <see cref="SignatureProvider"/> was found, false otherwise.</returns>
        public abstract bool TryGetSignatureProvider(SecurityKey securityKey, string algorithm, string typeofProvider, bool willCreateSignatures, out SignatureProvider signatureProvider);

        /// <summary>
        /// Trys to remove a <see cref="SignatureProvider"/> from this cache.
        /// </summary>
        /// <param name="signatureProvider"><see cref="SignatureProvider"/> to remove.</param>
        /// <returns>true if the <see cref="SignatureProvider"/> was removed, false if the <see cref="SignatureProvider"/> was not found.</returns>
        public abstract bool TryRemove(SignatureProvider signatureProvider);
    }
}
