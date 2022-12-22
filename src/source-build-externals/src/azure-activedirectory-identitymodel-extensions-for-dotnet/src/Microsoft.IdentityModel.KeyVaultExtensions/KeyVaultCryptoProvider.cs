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
using System.Linq;
using Microsoft.Azure.KeyVault.WebKey;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.KeyVaultExtensions
{
    /// <summary>
    /// Provides cryptographic operators based on Azure Key Vault.
    /// </summary>
    public class KeyVaultCryptoProvider : ICryptoProvider
    {
        private readonly CryptoProviderCache _cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyVaultCryptoProvider"/> class.
        /// </summary>
        public KeyVaultCryptoProvider()
        {
            _cache = new InMemoryCryptoProviderCache();
        }

        /// <summary>
        /// Gets the <see cref="CryptoProviderCache"/>
        /// </summary>
        internal CryptoProviderCache CryptoProviderCache => _cache;

        /// <summary>
        /// Returns a cryptographic operator that supports the algorithm.
        /// </summary>
        /// <param name="algorithm">the algorithm that defines the cryptographic operator.</param>
        /// <param name="args">the arguments required by the cryptographic operator. May be null.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="algorithm"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="args"/> is null.</exception>
        /// <exception cref="NotSupportedException">if <paramref name="args"/> does not contain a <see cref="KeyVaultSecurityKey"/>.</exception>
        /// <remarks>call <see cref="ICryptoProvider.Release(object)"/> when finished with the object.</remarks>
        public object Create(string algorithm, params object[] args)
        {
            if (string.IsNullOrEmpty(algorithm))
                throw LogHelper.LogArgumentNullException(nameof(algorithm));

            if (args == null)
                throw LogHelper.LogArgumentNullException(nameof(args));

            if (args.FirstOrDefault() is KeyVaultSecurityKey key)
            {
                if (JsonWebKeyEncryptionAlgorithm.AllAlgorithms.Contains(algorithm, StringComparer.Ordinal))
                    return new KeyVaultKeyWrapProvider(key, algorithm);
                else if (JsonWebKeySignatureAlgorithm.AllAlgorithms.Contains(algorithm, StringComparer.Ordinal))
                {
                    var willCreateSignatures = (bool)(args.Skip(1).FirstOrDefault() ?? false);

                    if (_cache.TryGetSignatureProvider(key, algorithm, typeofProvider: key.GetType().ToString(), willCreateSignatures, out var cachedProvider))
                        return cachedProvider;

                    var signatureProvider = new KeyVaultSignatureProvider(key, algorithm, willCreateSignatures);
                    if (CryptoProviderFactory.ShouldCacheSignatureProvider(signatureProvider))
                        _cache.TryAdd(signatureProvider);

                    return signatureProvider;
                }
            }

            throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant(LogMessages.IDX10652, LogHelper.MarkAsNonPII(algorithm))));
        }

        /// <summary>
        /// Called to determine if a cryptographic operation is supported.
        /// </summary>
        /// <param name="algorithm">the algorithm that defines the cryptographic operator.</param>
        /// <param name="args">the arguments required by the cryptographic operator. May be null.</param>
        /// <returns>true if supported</returns>
        public bool IsSupportedAlgorithm(string algorithm, params object[] args)
        {
            if (string.IsNullOrEmpty(algorithm))
                throw LogHelper.LogArgumentNullException(nameof(algorithm));

            if (args == null)
                throw LogHelper.LogArgumentNullException(nameof(args));

            return args.FirstOrDefault() is KeyVaultSecurityKey
                && (JsonWebKeyEncryptionAlgorithm.AllAlgorithms.Contains(algorithm, StringComparer.Ordinal) || JsonWebKeySignatureAlgorithm.AllAlgorithms.Contains(algorithm, StringComparer.Ordinal));
        }

        /// <summary>
        /// Called to release the object returned from <see cref="ICryptoProvider.Create(string, object[])"/>
        /// </summary>
        /// <param name="cryptoInstance">the object returned from <see cref="ICryptoProvider.Create(string, object[])"/>.</param>
        public void Release(object cryptoInstance)
        {
            if (cryptoInstance is SignatureProvider signatureProvider)
                _cache.TryRemove(signatureProvider);

            if (cryptoInstance is IDisposable obj)
                obj.Dispose();
        }
    }
}
