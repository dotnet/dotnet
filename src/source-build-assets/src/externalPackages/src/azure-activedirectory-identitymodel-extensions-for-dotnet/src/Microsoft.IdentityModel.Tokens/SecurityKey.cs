// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.IdentityModel.Logging;
using System.Text.Json.Serialization;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Base class for Security Key.
    /// </summary>
    public abstract class SecurityKey
    {
        private CryptoProviderFactory _cryptoProviderFactory;
        private object _internalIdLock = new object();
        private string _internalId;

        internal SecurityKey(SecurityKey key)
        {
            _cryptoProviderFactory = key._cryptoProviderFactory;
            KeyId = key.KeyId;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public SecurityKey()
        {
            _cryptoProviderFactory = CryptoProviderFactory.Default;
        }

        [JsonIgnore]
        internal virtual string InternalId
        {
            get
            {
                if (_internalId == null)
                {
                    lock (_internalIdLock)
                    {
                        if (_internalId == null)
                        {
                            if (CanComputeJwkThumbprint())
                                _internalId = Base64UrlEncoder.Encode(ComputeJwkThumbprint());
                            else
                                _internalId = string.Empty;
                        }
                    }
                }

                return _internalId;
            }
        }

        /// <summary>
        /// This must be overridden to get the size of this <see cref="SecurityKey"/>.
        /// </summary>
        public abstract int KeySize { get; }

        /// <summary>
        /// Gets the key id of this <see cref="SecurityKey"/>.
        /// </summary>
        [JsonIgnore]
        public virtual string KeyId { get; set; }

        /// <summary>
        /// Gets or sets <see cref="Microsoft.IdentityModel.Tokens.CryptoProviderFactory"/>.
        /// </summary>
        [JsonIgnore]
        public CryptoProviderFactory CryptoProviderFactory
        {
            get
            {
                return _cryptoProviderFactory;
            }
            set
            {
                _cryptoProviderFactory = value ?? throw LogHelper.LogArgumentNullException(nameof(value));
            }
        }

        /// <summary>
        /// Returns the formatted string: GetType(), KeyId: 'value', InternalId: 'value'.
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return $"{GetType()}, KeyId: '{KeyId}', InternalId: '{InternalId}'.";
        }

        /// <summary>
        /// Determines whether the <see cref="SecurityKey"/> can compute a JWK thumbprint.
        /// </summary>
        /// <returns><c>true</c> if JWK thumbprint can be computed; otherwise, <c>false</c>.</returns>
        /// <remarks>https://datatracker.ietf.org/doc/html/rfc7638</remarks>
        public virtual bool CanComputeJwkThumbprint()
        {
            return false;
        }

        /// <summary>
        /// Computes a sha256 hash over the <see cref="SecurityKey"/>.
        /// </summary>
        /// <returns>A JWK thumbprint.</returns>
        /// <remarks>https://datatracker.ietf.org/doc/html/rfc7638</remarks>
        public virtual byte[] ComputeJwkThumbprint()
        {
            throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant(LogMessages.IDX10710)));
        }

        /// <summary>
        /// Checks if <see cref="SecurityKey.CryptoProviderFactory"/> can perform the cryptographic operation specified by the <paramref name="algorithm"/> with this <see cref="SecurityKey"/>.
        /// </summary>
        /// <param name="algorithm">the algorithm to be used.</param>
        /// <returns>true if <see cref="SecurityKey.CryptoProviderFactory"/> can perform the cryptographic operation sepecified by the <paramref name="algorithm"/> with this <see cref="SecurityKey"/>.</returns>
        public virtual bool IsSupportedAlgorithm(string algorithm)
        {
            // do not throw if algorithm is null or empty to stay in sync with CryptoProviderFactory.IsSupportedAlgorithm.
            return CryptoProviderFactory.IsSupportedAlgorithm(algorithm, this);
        }
    }
}
