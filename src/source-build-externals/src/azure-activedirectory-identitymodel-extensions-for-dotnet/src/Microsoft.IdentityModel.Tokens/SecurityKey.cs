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
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Json;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Base class for Security Key.
    /// </summary>
    public abstract class SecurityKey
    {
        private CryptoProviderFactory _cryptoProviderFactory;
        private Lazy<string> _internalId;

        internal SecurityKey(SecurityKey key)
        {
            _cryptoProviderFactory = key._cryptoProviderFactory;
            KeyId = key.KeyId;
            SetInternalId();
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public SecurityKey()
        {
            _cryptoProviderFactory = CryptoProviderFactory.Default;
            SetInternalId();
        }

        [JsonIgnore]
        internal virtual string InternalId { get => _internalId.Value; }

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
        /// <param name="algorithm">the algorithm to apply.</param>
        /// <returns>true if <see cref="SecurityKey.CryptoProviderFactory"/> can perform the cryptographic operation sepecified by the <paramref name="algorithm"/> with this <see cref="SecurityKey"/>.</returns>
        public virtual bool IsSupportedAlgorithm(string algorithm)
        {
            // do not throw if algorithm is null or empty to stay in sync with CryptoProviderFactory.IsSupportedAlgorithm.
            return CryptoProviderFactory.IsSupportedAlgorithm(algorithm, this);
        }

        /// <summary>
        /// Sets the <see cref="InternalId"/> to value of <see cref="SecurityKey"/>'s JWK thumbprint if it can be computed, otherwise sets the <see cref="InternalId"/> to <see cref="string.Empty"/>.
        /// </summary>
        private void SetInternalId()
        {
            _internalId = new Lazy<string>(() =>
            {
                if (CanComputeJwkThumbprint())
                    return Base64UrlEncoder.Encode(ComputeJwkThumbprint());
                else
                    return string.Empty;
            });
        }
    }
}
