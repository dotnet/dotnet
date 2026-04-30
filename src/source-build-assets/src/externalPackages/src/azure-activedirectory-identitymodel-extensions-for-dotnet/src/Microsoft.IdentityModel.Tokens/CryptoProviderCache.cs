// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Abstract definition of a cache for cryptographic providers.
    /// </summary>
    public abstract class CryptoProviderCache
    {
        /// <summary>
        /// Returns the cache key used to look up an entry for a <see cref="SignatureProvider"/>.
        /// </summary>
        /// <param name="signatureProvider">The <see cref="SignatureProvider"/> to create the key for.</param>
        /// <returns>The cache key used for finding a <see cref="SignatureProvider"/>.</returns>
        protected abstract string GetCacheKey(SignatureProvider signatureProvider);

        /// <summary>
        /// Returns the cache key used to find a cryptographic provider in this cache.
        /// </summary>
        /// <param name="securityKey">The key used by the cryptographic provider.</param>
        /// <param name="algorithm">The algorithm used by the cryptographic provider.</param>
        /// <param name="typeofProvider">The type of the cryptographic provider obtained by calling object.GetType().</param>
        /// <returns>The cache key used for finding a cryptographic provider.</returns>
        protected abstract string GetCacheKey(SecurityKey securityKey, string algorithm, string typeofProvider);

        /// <summary>
        /// Tries to add a <see cref="SignatureProvider"/> to this cache.
        /// </summary>
        /// <param name="signatureProvider">The <see cref="SignatureProvider"/> to cache.</param>
        /// <returns>True if the <see cref="SignatureProvider"/> was added; false if the cache already contained the <see cref="SignatureProvider"/>.</returns>
        public abstract bool TryAdd(SignatureProvider signatureProvider);

        /// <summary>
        /// Tries to find a <see cref="SignatureProvider"/> in this cache.
        /// </summary>
        /// <param name="securityKey">The key used by the cryptographic provider.</param>
        /// <param name="algorithm">The algorithm used by the cryptographic provider.</param>
        /// <param name="typeofProvider">The type of the cryptographic provider obtained by calling object.GetType().</param>
        /// <param name="willCreateSignatures">If true, the provider will be used for creating signatures.</param>
        /// <param name="signatureProvider">The <see cref="SignatureProvider"/> if found.</param>
        /// <returns>True if a <see cref="SignatureProvider"/> was found; false otherwise.</returns>
        public abstract bool TryGetSignatureProvider(SecurityKey securityKey, string algorithm, string typeofProvider, bool willCreateSignatures, out SignatureProvider signatureProvider);

        /// <summary>
        /// Tries to remove a <see cref="SignatureProvider"/> from this cache.
        /// </summary>
        /// <param name="signatureProvider">The <see cref="SignatureProvider"/> to remove.</param>
        /// <returns>True if the <see cref="SignatureProvider"/> was removed; false if the <see cref="SignatureProvider"/> was not found.</returns>
        public abstract bool TryRemove(SignatureProvider signatureProvider);
    }
}
